using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMonTrans.cs
    /// Description     : 미수금변동월보
    /// Author          : 박창욱
    /// Create Date     : 2017-09-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUM205.FRM(FrmMonTrans.frm) >> frmPmpaViewMonTrans.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMonTrans : Form
    {
        clsPmpaMisu cpm = new clsPmpaMisu();
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        double[,] nTotAmt = new double[4, 11];
        double[] nTotQty = new double[11];

        public frmPmpaViewMonTrans()
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
            string strYYMM = "";
            string strFDate = "";
            string strTDate = "";
            string strNewData = "";
            string strOldData = "";
            int nSimQty = 0;
            double nSimTAmt = 0;
            double nSimMAmt = 0;
            double nSimIAmt = 0;

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTDate = clsVbfunc.LastDay((int)VB.Val(VB.Left(strYYMM, 4)), (int)VB.Val(VB.Right(strYYMM, 2)));

            btnSearch.Enabled = false;
            btnPrint.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            try
            {
                //당월 변동자 명단조회(청구제외)
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO, b.Class, TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate,";
                SQL = SQL + ComNum.VBLF + "        b.GelCode, b.MisuID, b.IpdOpd,";
                SQL = SQL + ComNum.VBLF + "        b.Bun";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_MONTHLY a, " + ComNum.DB_PMPA + "MISU_IDMST b";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM = '" + strYYMM + "'";
                if (rdoClass0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '01'"; //공단
                }
                else if (rdoClass1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '02'"; //직장
                }
                else if (rdoClass2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '03'"; //지역
                }
                else if (rdoClass3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '04'"; //보호
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class < '05'"; //전체
                }
                SQL = SQL + ComNum.VBLF + "    AND (IpgumAmt <> 0 OR SakAmt <> 0 OR BanAmt <> 0 OR EtcAmt <> 0)";
                SQL = SQL + ComNum.VBLF + "    AND a.WRTNO = b.WRTNO";
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
                    strNewData += VB.Left(dt.Rows[i]["Bdate"].ToString().Trim() + VB.Space(10), 10);

                    //미수종류별 소계 인쇄
                    if (VB.Left(strNewData, 2) != VB.Left(strOldData, 2))
                    {
                        if (strOldData.Trim() != "")
                        {
                            SubTot_Rtn(ref nRow);
                        }
                    }
                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }


                    //미수종류, 청구일자가 같은 것 Display 제외
                    if (strNewData != strOldData)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = cpm.READ_MisuClass(VB.Left(strNewData, 2));
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = VB.Mid(strNewData, 3, 8).Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = VB.Right(strNewData, 10);
                        strOldData = strNewData;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["MisuID"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = cpm.READ_MisuIpdOpd(dt.Rows[i]["IpdOpd"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = cpm.READ_MisuBunya(dt.Rows[i]["Bun"].ToString().Trim());
                    nSimQty = 0;
                    nSimTAmt = 0;
                    nSimMAmt = 0;
                    nSimIAmt = 0;

                    //해당월까지 총진료비, 청구, 입금, 삭감액 등 READ & Display
                    for (k = 1; k < 11; k++)
                    {
                        nTotAmt[1, k] = 0;
                        nTotQty[k] = 0;
                    }

                    SQL = "";
                    SQL = SQL + " SELECT TO_CHAR(Bdate,'YYYYMMDD') sBdate, Gubun sGubun, Qty sQty,";
                    SQL = SQL + "         TAmt sTamt, Amt sAmt";
                    SQL = SQL + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                    SQL = SQL + "  WHERE 1 = 1";
                    SQL = SQL + "    AND WRTNO = " + VB.Val(dt.Rows[i]["WRTNO"].ToString().Trim());
                    SQL = SQL + "    AND Bdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                    SQL = SQL + "  ORDER BY Bdate,Gubun";

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
                        for (k = 0; k < dt1.Rows.Count; k++)
                        {
                            switch (dt1.Rows[k]["sGubun"].ToString().Trim())
                            {
                                case "11":  //청구액
                                case "12":
                                case "13":
                                case "14":
                                case "15":
                                case "16":
                                case "17":
                                case "18":
                                case "19":
                                    nTotQty[1] += VB.Val(dt1.Rows[k]["sQty"].ToString().Trim());
                                    nTotAmt[1, 1] += VB.Val(dt1.Rows[k]["sTAmt"].ToString().Trim());
                                    nTotAmt[1, 2] += VB.Val(dt1.Rows[k]["sAmt"].ToString().Trim());
                                    break;
                                case "21":  //입금액
                                case "22":
                                case "23":
                                case "24":
                                case "25":
                                case "26":
                                case "27":
                                case "28":
                                case "29":
                                    if (VB.Left(dt1.Rows[k]["sBdate"].ToString().Trim(), 6) == strYYMM)
                                    {
                                        nTotQty[3] += VB.Val(dt1.Rows[k]["sQty"].ToString().Trim());
                                        nTotAmt[1, 3] += VB.Val(dt1.Rows[k]["sAmt"].ToString().Trim());
                                    }
                                    break;
                                case "31": //삭감액
                                case "35":
                                    if (VB.Left(dt1.Rows[k]["sBdate"].ToString().Trim(), 6) == strYYMM)
                                    {
                                        nTotQty[4] += VB.Val(dt1.Rows[k]["sQty"].ToString().Trim());
                                        nTotAmt[1, 4] += VB.Val(dt1.Rows[k]["sAmt"].ToString().Trim());
                                    }
                                    break;
                                case "32": //반송액
                                    if (VB.Left(dt1.Rows[k]["sBdate"].ToString().Trim(), 6) == strYYMM)
                                    {
                                        nTotQty[5] += VB.Val(dt1.Rows[k]["sQty"].ToString().Trim());
                                        nTotAmt[1, 5] += VB.Val(dt1.Rows[k]["sAmt"].ToString().Trim());
                                    }
                                    break;
                                case "33": //과입금
                                    if (VB.Left(dt1.Rows[k]["sBdate"].ToString().Trim(), 6) == strYYMM)
                                    {
                                        nTotAmt[1, 6] += VB.Val(dt1.Rows[k]["sAmt"].ToString().Trim());
                                    }
                                    break;
                                case "34": //기타입금
                                    if (VB.Left(dt1.Rows[k]["sBdate"].ToString().Trim(), 6) == strYYMM)
                                    {
                                        nTotAmt[1, 7] += VB.Val(dt1.Rows[k]["sAmt"].ToString().Trim());
                                    }
                                    break;
                                case "10": //심사중
                                    nTotQty[8] += VB.Val(dt1.Rows[k]["sQty"].ToString().Trim());
                                    nTotAmt[1, 8] += VB.Val(dt1.Rows[k]["sAmt"].ToString().Trim());
                                    break;
                                case "09": //지급보류
                                    nTotQty[9] += VB.Val(dt1.Rows[k]["sQty"].ToString().Trim());
                                    nTotAmt[1, 9] += VB.Val(dt1.Rows[k]["sAmt"].ToString().Trim());
                                    break;
                            }

                            //심사중 청구액, 심사중 입금액 계산
                            switch (dt1.Rows[k]["sGubun"].ToString().Trim())
                            {
                                case "26":  //당월 심사중 입금액
                                    if (VB.Left(dt1.Rows[k]["sBdate"].ToString().Trim(), 6) == strYYMM)
                                    {
                                        nSimIAmt += VB.Val(dt1.Rows[k]["sAmt"].ToString().Trim());
                                    }
                                    break;
                                case "10":  //심사중
                                    nSimQty += (int)VB.Val(dt1.Rows[k]["sQty"].ToString().Trim());
                                    nSimTAmt += VB.Val(dt1.Rows[k]["sTAmt"].ToString().Trim());
                                    nSimMAmt += VB.Val(dt1.Rows[k]["SAmt"].ToString().Trim());
                                    break;
                            }
                        }
                        dt1.Dispose();
                        dt1 = null;


                        nTotAmt[1, 10] = nTotAmt[1, 2];
                        for (k = 3; k < 10; k++)
                        {
                            nTotAmt[1, 10] -= nTotAmt[1, k];
                        }

                        //97.04.28 심사계 레아 요청으로 수정함
                        if (nSimIAmt != 0)
                        {
                            nTotQty[1] = nSimQty;
                            nTotAmt[1, 1] = nSimTAmt;
                            nTotAmt[1, 2] = nSimMAmt;
                            nTotQty[8] = 0;
                            nTotAmt[1, 8] = 0;
                            nTotQty[9] = 0;
                            nTotAmt[1, 9] = 0;
                        }

                        ssView_Sheet1.Cells[nRow - 1, 6].Text = nTotQty[1].ToString("####0");
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = nTotAmt[1, 1].ToString("###,###,###,##0");
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[1, 2].ToString("###,###,###,##0");
                        ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotQty[3].ToString("####0");
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = nTotAmt[1, 3].ToString("###,###,###,##0");
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = nTotQty[4].ToString("####0");
                        ssView_Sheet1.Cells[nRow - 1, 12].Text = nTotAmt[1, 4].ToString("###,###,###,##0");
                        ssView_Sheet1.Cells[nRow - 1, 13].Text = nTotQty[5].ToString("####0");
                        ssView_Sheet1.Cells[nRow - 1, 14].Text = nTotAmt[1, 5].ToString("###,###,###,##0");
                        ssView_Sheet1.Cells[nRow - 1, 15].Text = nTotAmt[1, 6].ToString("###,###,###,##0");
                        ssView_Sheet1.Cells[nRow - 1, 16].Text = nTotAmt[1, 7].ToString("###,###,###,##0");
                        ssView_Sheet1.Cells[nRow - 1, 17].Text = nTotQty[8].ToString("####0");
                        ssView_Sheet1.Cells[nRow - 1, 18].Text = nTotAmt[1, 8].ToString("###,###,###,##0");
                        ssView_Sheet1.Cells[nRow - 1, 19].Text = nTotQty[9].ToString("####0");
                        ssView_Sheet1.Cells[nRow - 1, 20].Text = nTotAmt[1, 9].ToString("###,###,###,##0");
                        if (nTotAmt[1, 10] != 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 19].Text = "****";
                        }

                        //소계, 누계에 금액을 누적
                        for (k = 1; k < 11; k++)
                        {
                            nTotAmt[2, k] += nTotAmt[1, k]; //소계
                            nTotAmt[3, k] += nTotAmt[1, k]; //누계
                        }

                        nSimQty = 0;
                        nSimTAmt = 0;
                        nSimMAmt = 0;
                        nSimIAmt = 0;
                    }
                }
                dt.Dispose();
                dt = null;

                if (rdoClass4.Checked == true)
                {
                    SubTot_Rtn(ref nRow);
                }


                //전체합계
                nRow += 1;
                if (nRow > ssView_Sheet1.RowCount)
                {
                    ssView_Sheet1.RowCount = nRow;
                }

                ssView_Sheet1.Cells[nRow - 1, 2].Text = "합 계";
                ssView_Sheet1.Cells[nRow - 1, 7].Text = nTotAmt[3, 1].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[3, 2].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 10].Text = nTotAmt[3, 3].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 12].Text = nTotAmt[3, 4].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 14].Text = nTotAmt[3, 5].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 15].Text = nTotAmt[3, 6].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 16].Text = nTotAmt[3, 7].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 18].Text = nTotAmt[3, 8].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 20].Text = nTotAmt[3, 9].ToString("###,###,###,##0");

                for (i = 1; i < 11; i++)
                {
                    nTotAmt[3, i] = 0;
                }

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

        //종류별 소계
        void SubTot_Rtn(ref int nRow)
        {
            int i = 0;

            nRow += 1;
            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }

            ssView_Sheet1.Cells[nRow - 1, 2].Text = "소 계";
            ssView_Sheet1.Cells[nRow - 1, 7].Text = nTotAmt[2, 1].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[2, 2].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 10].Text = nTotAmt[2, 3].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 12].Text = nTotAmt[2, 4].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 14].Text = nTotAmt[2, 5].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 15].Text = nTotAmt[2, 6].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 16].Text = nTotAmt[2, 7].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 18].Text = nTotAmt[2, 8].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 20].Text = nTotAmt[2, 9].ToString("###,###,###,##0");

            for (i = 1; i < 11; i++)
            {
                nTotAmt[2, i] = 0;
            }
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
            string strClass = "";
            bool PrePrint = true;

            if (rdoClass0.Checked == true)
            {
                strClass = "(공단)";
            }
            else if (rdoClass1.Checked == true)
            {
                strClass = "(직장)";
            }
            else if (rdoClass2.Checked == true)
            {
                strClass = "(지역)";
            }
            else if (rdoClass3.Checked == true)
            {
                strClass = "(보호)";
            }
            else
            {
                strClass = "(전체)";
            }

            strTitle = cboYYMM.Text + " 개인별 변동명세서 " + strClass;

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력시간 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력자 : " + clsType.User.JobName + " 인 " + VB.Space(5), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void frmPmpaViewMonTrans_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 15, "", "1");
            cboYYMM.SelectedIndex = 0;
        }
    }
}
