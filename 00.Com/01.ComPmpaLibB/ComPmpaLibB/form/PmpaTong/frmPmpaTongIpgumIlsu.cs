using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaTongIpgumIlsu.cs
    /// Description     : 미수금회수기간통계
    /// Author          : 박창욱
    /// Create Date     : 2017-09-21
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUM206.FRM(FrmIpgumIlsu.frm) >> frmPmpaTongIpgumIlsu.cs 폼이름 재정의" />	
    public partial class frmPmpaTongIpgumIlsu : Form
    {
        clsPmpaMisu cpm = new clsPmpaMisu();
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        double[,] nTotAmt = new double[5, 10];
        double[] nTotQty = new double[5];
        double[] nTotIlsu = new double[5];

        public frmPmpaTongIpgumIlsu()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            int nRead = 0;
            int nRow = 0;
            double nJanAmt = 0;
            string strYYMM = "";
            string strYYMM2 = "";
            string strFDate = "";
            string strTDate = "";

            string strNewData = "";
            string strOldData = "";

            strYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 7, 2);
            strYYMM2 = VB.Left(cboTYYMM.Text, 4) + VB.Mid(cboTYYMM.Text, 7, 2);

            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTDate = clsVbfunc.LastDay((int)VB.Val(VB.Left(strYYMM2, 4)), (int)VB.Val(VB.Right(strYYMM, 2)));

            btnSearch.Enabled = false;
            btnPrint.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            //화면을 Clear
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            //합계용 누적 field Clear
            for (i = 1; i < 5; i++)
            {
                for (k = 1; k < 10; k++)
                {
                    nTotAmt[i, k] = 0;
                }
                nTotQty[i] = 0;
                nTotIlsu[i] = 0;
            }

            try
            {
                //당월 변동자 명단조회(청구제외)

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO, b.Class, TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate,";
                SQL = SQL + ComNum.VBLF + "        b.GelCode, b.MisuID, b.IpdOpd,";
                SQL = SQL + ComNum.VBLF + "        b.Bun, b.Amt2, b.Amt3,";
                SQL = SQL + ComNum.VBLF + "        a.IpgumAmt, a.SakAmt + a.SakAmt2 SakAmt, a.BanAmt,";
                SQL = SQL + ComNum.VBLF + "        a.EtcAmt,a.JanAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_MONTHLY a, " + ComNum.DB_PMPA + "MISU_IDMST b";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM >= '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM <= '" + strYYMM2 + "'";
                SQL = SQL + ComNum.VBLF + "    AND (IpgumAmt <> 0 OR SakAmt <> 0 OR SakAmt2 <> 0 OR EtcAmt <> 0)";
                SQL = SQL + ComNum.VBLF + "    AND a.WRTNO = b.WRTNO";
                SQL = SQL + ComNum.VBLF + "    AND a.Class < '05'";
                SQL = SQL + ComNum.VBLF + "  ORDER BY b.Class,b.GelCode,b.Bdate,b.IpdOpd,b.MisuID,b.Bun";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                strOldData = VB.Space(18);
                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    strNewData = dt.Rows[i]["Class"].ToString().Trim() + VB.Left(dt.Rows[i]["GelCode"].ToString().Trim() + VB.Space(8), 8);

                    //미수 종류별 소계 인쇄
                    if (VB.Left(strNewData, 2) != VB.Left(strOldData, 2))
                    {
                        if (strOldData.Trim() != "")
                        {
                            JohapTot_Rtn(strOldData, ref nRow);
                            SubTot_Rtn(ref nRow);
                        }
                    }
                    else if (strNewData != strOldData)
                    {
                        JohapTot_Rtn(strOldData, ref nRow);
                    }

                    for (k = 1; k < 9; k++)
                    {
                        nTotAmt[1, k] = 0;
                    }
                    nTotQty[1] = 0;
                    nTotIlsu[1] = 0;

                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    //미수종류, 청구일자가 같은 것 Display 제외
                    if (strNewData != strOldData)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = cpm.READ_MisuClass(VB.Left(strNewData, 2));
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = cpm.READ_BAS_MIA(VB.Mid(strNewData, 3, 8).Trim());
                        strOldData = strNewData;
                    }

                    nTotAmt[1, 1] = VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());
                    nTotAmt[1, 2] = VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());
                    nTotAmt[1, 3] = VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim());
                    nTotAmt[1, 5] = VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim()) + VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim());
                    nTotAmt[1, 6] = VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());

                    //당월이전 입금
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT SUM(Amt) BIPAmt";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND WRTNO = " + VB.Val(dt.Rows[i]["WRTNO"].ToString().Trim());
                    SQL = SQL + ComNum.VBLF + "    AND Bdate < TO_DATE('" + strFDate + "','YYYY-MM-DD')   ";
                    SQL = SQL + ComNum.VBLF + "    AND Gubun >= '21'";
                    SQL = SQL + ComNum.VBLF + "    AND Gubun <= '29'";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        btnPrint.Enabled = true;
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        nTotAmt[1, 7] = VB.Val(dt1.Rows[0]["BIPAmt"].ToString().Trim());
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //당월이전 삭감반송
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT SUM(Amt) BSAKAmt";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND WRTNO = " + VB.Val(dt.Rows[i]["WRTNO"].ToString().Trim()) + "";
                    SQL = SQL + ComNum.VBLF + "    AND Bdate < TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND Gubun IN ('31','32','35')";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        btnPrint.Enabled = true;
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        nTotAmt[1, 8] = VB.Val(dt1.Rows[0]["BSAKAmt"].ToString().Trim());
                    }

                    nJanAmt = nTotAmt[1, 1] - nTotAmt[1, 2] - nTotAmt[1, 3] - nTotAmt[1, 5];

                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["MisuID"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = cpm.READ_MisuIpdOpd(dt.Rows[i]["IpdOpd"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = cpm.READ_MisuBunya(dt.Rows[i]["Bun"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[1, 1].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotAmt[1, 7].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = nTotAmt[1, 2].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = nTotAmt[1, 8].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = (nTotAmt[1, 3] + nTotAmt[1, 5]).ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = nTotAmt[1, 6].ToString("###,###,###,##0");

                    dt1.Dispose();
                    dt1 = null;

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(MAX(Bdate),'YYYY-MM-DD') sBdate,";
                    SQL = SQL + ComNum.VBLF + "        SUM(DECODE(Gubun,'31',TAmt,0)) sSakTAmt,";
                    SQL = SQL + ComNum.VBLF + "        SUM(DECODE(Gubun,'26',Amt,0)) sSimIpgum";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND WRTNO = " + VB.Val(dt.Rows[i]["WRTNO"].ToString().Trim()) + "";
                    SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND Gubun > '20'";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        btnPrint.Enabled = true;
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count == 0)
                    {
                        nTotIlsu[1] = 0;
                        nTotQty[1] = 0;
                        nTotAmt[1, 4] = 0;
                    }
                    else
                    {
                        nTotQty[1] = 1;
                        nTotIlsu[1] = VB.DateDiff("D", Convert.ToDateTime(dt.Rows[i]["Bdate"].ToString().Trim()),
                                                       Convert.ToDateTime(dt1.Rows[0]["sBdate"].ToString().Trim()));
                        if (nTotIlsu[1] == 0)
                        {
                            nTotQty[1] = 0;
                        }
                        nTotAmt[1, 4] = VB.Val(dt1.Rows[0]["sSakTAmt"].ToString().Trim());
                    }

                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt1.Rows[0]["sBdate"].ToString().Trim();

                    if (rdoJob0.Checked == true && VB.Val(dt1.Rows[0]["sSimIpgum"].ToString().Trim()) > 0)  //심사중
                    {
                        nTotQty[1] = 0;
                        nTotIlsu[1] = 0;
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = "****";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = nTotIlsu[1].ToString("###,##0.0");
                    }
                    dt1.Dispose();
                    dt1 = null;

                    //소계,누계에 금액을 누적
                    nTotQty[2] += nTotQty[1];
                    nTotIlsu[2] += nTotIlsu[1];
                    nTotQty[3] += nTotQty[1];
                    nTotIlsu[3] += nTotIlsu[1];
                    nTotQty[4] += nTotQty[1];
                    nTotIlsu[4] += nTotIlsu[1];

                    for (k = 1; k < 9; k++)
                    {
                        nTotAmt[2, k] += nTotAmt[1, k]; //조합별계
                        nTotAmt[3, k] += nTotAmt[1, k]; //소계
                        nTotAmt[4, k] += nTotAmt[1, k]; //누계
                    }
                }
                dt.Dispose();
                dt = null;

                JohapTot_Rtn(strOldData, ref nRow);
                SubTot_Rtn(ref nRow);

                //전체합계
                nRow += 1;
                ssView_Sheet1.RowCount = nRow;

                ssView_Sheet1.Cells[nRow - 1, 1].Text = "-< 전체합계 >-";
                if (nTotQty[4] != 0 && nTotIlsu[4] != 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = "평균";
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = (nTotIlsu[4] / nTotQty[4]).ToString("###,##0.0");
                }
                else
                {
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = "";
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = "";
                }

                ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[4, 1].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotAmt[4, 7].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 10].Text = nTotAmt[4, 2].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 11].Text = nTotAmt[4, 8].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 12].Text = (nTotAmt[4, 3] + nTotAmt[4, 5]).ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 13].Text = nTotAmt[4, 6].ToString("###,###,###,##0");

                for (k = 1; k < 9; k++)
                {
                    nTotAmt[4, k] = 0;
                }
                nTotQty[4] = 0;
                nTotIlsu[4] = 0;

                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        //조합별 소계
        void JohapTot_Rtn(string strOldData, ref int nRow)
        {
            int i = 0;
            string strJohapOK = "";

            strJohapOK = "OK";
            if (string.Compare(VB.Left(strOldData, 2), "03") < 0)
            {
                strJohapOK = "NO";
            }
            if (nTotQty[2] == 1)
            {
                strJohapOK = "NO";
            }

            if (strJohapOK == "OK")
            {
                nRow += 1;
                if (nRow > ssView_Sheet1.RowCount)
                {
                    ssView_Sheet1.RowCount = nRow;
                }

                ssView_Sheet1.Cells[nRow - 1, 1].Text = "-< 조합별계 >-";
                if (nTotQty[2] != 0 && nTotIlsu[2] != 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = "평균";
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = (nTotIlsu[2] / nTotQty[2]).ToString("###,##0.0");
                }
                else
                {
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = "";
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = "";
                }

                ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[2, 1].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotAmt[2, 7].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 10].Text = nTotAmt[2, 2].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 11].Text = nTotAmt[2, 8].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 12].Text = (nTotAmt[2, 3] + nTotAmt[2, 5]).ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 13].Text = nTotAmt[2, 6].ToString("###,###,###,##0");
            }

            for (i = 1; i < 9; i++)
            {
                nTotAmt[2, i] = 0;
            }
            nTotQty[2] = 0;
            nTotIlsu[2] = 0;
        }


        //종류별 소계
        void SubTot_Rtn(ref int nRow)
        {
            int i = 0;

            nRow += 1;
            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }

            ssView_Sheet1.Cells[nRow - 1, 1].Text = "-< 종류별계 >-";
            if (nTotQty[3] != 0 && nTotIlsu[3] != 0)
            {
                ssView_Sheet1.Cells[nRow - 1, 3].Text = "평균";
                ssView_Sheet1.Cells[nRow - 1, 4].Text = (nTotIlsu[3] / nTotQty[3]).ToString("###,##0.0");
            }
            else
            {
                ssView_Sheet1.Cells[nRow - 1, 3].Text = "";
                ssView_Sheet1.Cells[nRow - 1, 4].Text = "";
            }
            ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[3, 1].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotAmt[3, 7].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 10].Text = nTotAmt[3, 2].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 11].Text = nTotAmt[3, 8].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 12].Text = (nTotAmt[3, 3] + nTotAmt[3, 5]).ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 13].Text = nTotAmt[3, 6].ToString("###,###,###,##0");

            for (i = 1; i < 9; i++)
            {
                nTotAmt[3, i] = 0;
            }

            nTotQty[3] = 0;
            nTotIlsu[3] = 0;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = cboFYYMM.Text + "~" + cboTYYMM.Text + " 미수금 회수기간 통계";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력시간 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력자 : " + clsType.User.JobMan + " 인 " + VB.Space(15), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void frmPmpaTongIpgumIlsu_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int nMM = 0;
            int i = 0;
            string strSysDate = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon,"D"), "D");

            nYY = (int)VB.Val(VB.Left(strSysDate, 4));
            nMM = (int)VB.Val(VB.Mid(strSysDate, 6, 2));

            for (i = 1; i < 16; i++)
            {
                cboFYYMM.Items.Add(nYY.ToString("0000") + "년 " + nMM.ToString("00") + "월분");
                cboTYYMM.Items.Add(nYY.ToString("0000") + "년 " + nMM.ToString("00") + "월분");
                nMM -= 1;
                if (nMM == 0)
                {
                    nMM = 12;
                    nYY -= 1;
                }
            }
            cboFYYMM.SelectedIndex = 0;
            cboTYYMM.SelectedIndex = 0;
        }
    }
}
