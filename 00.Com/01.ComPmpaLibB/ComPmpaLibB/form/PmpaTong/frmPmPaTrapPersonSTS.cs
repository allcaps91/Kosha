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
    /// File Name       : frmPmPaTrapPersonSTS
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-16
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "D:\psmh\misu\misuta.vbp\MISUT210.FRM(FrmTrapPerson.frm) >> frmPmPaTrapPersonSTS.cs 폼이름 재정의" />
    public partial class frmPmPaTrapPersonSTS : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmPaTrapPersonSTS()
        {
            InitializeComponent();
        }

        private void frmPmPaTrapPersonSTS_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            string strYY = "";
            string strMM = "";
            int nYY = 0;
            int nMM = 0;
            int nMM1 = 0;
            int i = 0;

            strYY = VB.Left(strDTP, 4);
            strMM = VB.Mid(strDTP, 6, 2);

            nYY = Convert.ToInt32(strYY);
            nMM = Convert.ToInt32(strMM);
            nMM1 = Convert.ToInt32(strMM);

            for (i = 1; i <= 5; i++)
            {
                if (nMM == 0)
                {
                    nYY = nYY - 1;
                    nMM = 12;
                    nMM1 = nMM;
                }
                nMM = nMM - 1;
                strYY = Convert.ToInt32(nYY).ToString("0000");
                strMM = Convert.ToInt32(nMM).ToString("00");
                cboYYMM.Items.Add(strYY + "년" + strMM + "월분");
                nMM1 = nMM1 - 1;
            }
            cboYYMM.SelectedIndex = 1;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int nRows = 0;
            int nRateGasan = 0;
            int nSubCnt = 0;
            int nOpdCnt = 0;
            int nIpdCnt = 0;
            int nTotCnt = 0;
            int nWRTNO = 0;
            int nJinAmt = 0;
            string strIpdOpd = "";
            string strYYMM = "";
            string strGelCode1 = "";
            string strGelCode2 = "";
            string strGelCode3 = "";
            string strMianame = "";
            string strPano = "";
            double nSubJamt = 0;
            double nOpdJamt = 0;
            double nIpdJamt = 0;
            double nTotJamt = 0;

            nTotJamt = 0;
            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2);

            MIR_SANID_PREPARE(strYYMM, ref nRows, ref nRateGasan, ref nWRTNO, ref nJinAmt, ref strGelCode1, ref strGelCode2, ref nSubCnt, ref nSubJamt, ref strGelCode3, strMianame, ref strIpdOpd, ref strPano, ref nTotJamt, ref nTotCnt, ref nIpdJamt, ref nIpdCnt, ref nOpdJamt, ref nOpdCnt);

        }

        private void MIR_SANID_PREPARE(string strYYMM, ref int nRows, ref int nRateGasan, ref int nWRTNO, ref int nJinAmt, ref string strGelCode1, ref string strGelCode2, ref int nSubCnt, ref double nSubJamt, ref string strGelCode3, string strMianame, ref string strIpdOpd, ref string strPano, ref double nTotJamt, ref int nTotCnt, ref double nIpdJamt, ref int nIpdCnt, ref double nOpdJamt, ref int nOpdCnt)
        {
            int i = 0;
            DataTable dt = null;
            DataTable dtFn = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int nDataCnt = 0;
            double nAmts1 = 0;
            double nAmts2 = 0;
            double nAmts3 = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT GelCode, Pano, Sname, TO_CHAR(Frdate,'YY.MM.DD') Fdate,    ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(Todate,'YY-MM-DD') Tdate, IpdOpd, CarNo,DeptCode1, ";
                SQL = SQL + ComNum.VBLF + "        WrtNo, RateGasan                                           ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MIR_TAID                           ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                                        ";
                SQL = SQL + ComNum.VBLF + "    AND YYMM     =  '" + strYYMM + "'                              ";
                SQL = SQL + ComNum.VBLF + "    AND Upcnt1  !=  '9'                                            ";
                SQL = SQL + ComNum.VBLF + "    AND Upcnt2  !=  '9'                                            ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY GelCode,Pano                                            ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                nRows = 0;
                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                nDataCnt = dt.Rows.Count;
                for (i = 0; i < nDataCnt; i++)
                {
                    nRateGasan = (int)VB.Val(dt.Rows[i]["RateGasan"].ToString().Trim());
                    nWRTNO = (int)VB.Val(dt.Rows[i]["WrtNo"].ToString().Trim());
                    nJinAmt = 0;

                    #region GoSub MIR_SANDTL_PREPARE    자보 Detail Select

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT SUM(AMT1) Samt1            ";
                    SQL = SQL + ComNum.VBLF + "   FROM MIR_TAAMT                  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE WRTNO = '" + nWRTNO + "'   ";
                    SQL = SQL + ComNum.VBLF + "  GROUP BY WRTNO                   ";

                    SqlErr = clsDB.GetDataTable(ref dtFn, SQL, clsDB.DbCon);

                    if (dtFn.Rows.Count > 0)
                    {
                        nAmts1 = VB.Val(dtFn.Rows[0]["Samt1"].ToString().Trim());
                    }
                    else
                    {
                        nAmts1 = 0;
                    }
                    dtFn.Dispose();
                    dtFn = null;

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT SUM(AMT2) Samt2            ";
                    SQL = SQL + ComNum.VBLF + "   FROM MIR_TAAMT                  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE WRTNO = '" + nWRTNO + "'   ";
                    SQL = SQL + ComNum.VBLF + "    AND SubscriptNo > 2            ";
                    SQL = SQL + ComNum.VBLF + "  GROUP BY WRTNO                   ";

                    SqlErr = clsDB.GetDataTable(ref dtFn, SQL, clsDB.DbCon);

                    if (dtFn.Rows.Count > 0)
                    {
                        nAmts2 = VB.Val(dtFn.Rows[0]["Samt2"].ToString().Trim());
                    }
                    else
                    {
                        nAmts2 = 0;
                    }

                    dtFn.Dispose();
                    dtFn = null;

                    nAmts3 = nAmts2 * nRateGasan / 100;
                    //' 청구금액 가산후 사사오입함
                    nAmts3 = Convert.ToDouble(VB.Fix((int)(nAmts3 + 0.5)));
                    nJinAmt = (int)(nAmts1 + nAmts2 + nAmts3);
                    nJinAmt = (int)(nJinAmt / 10) * 10;

                    #endregion

                    strGelCode1 = dt.Rows[i]["GelCode"].ToString().Trim();

                    if (i == 0)
                    {
                        strGelCode2 = strGelCode1;
                    }

                    if (strGelCode1 != strGelCode2)
                    {
                        #region Gel_Total_rtn
                        nRows = nRows + 1;

                        if (nRows > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRows;
                        }
                        ssView_Sheet1.Rows[nRows - 1].BackColor = Color.FromArgb(80, 240, 180);
                        ssView_Sheet1.Cells[nRows - 1, 0].Text = "     ** 소    계 ** ";
                        ssView_Sheet1.Cells[nRows - 1, 1].Text = " ";
                        ssView_Sheet1.Cells[nRows - 1, 2].Text = " ";
                        ssView_Sheet1.Cells[nRows - 1, 3].Text = "    " + nSubCnt.ToString("##0") + " 건 ";
                        ssView_Sheet1.Cells[nRows - 1, 4].Text = " ";
                        ssView_Sheet1.Cells[nRows - 1, 5].Text = " ";
                        ssView_Sheet1.Cells[nRows - 1, 6].Text = " ";
                        ssView_Sheet1.Cells[nRows - 1, 7].Text = nSubJamt.ToString("####,###,##0");
                        ssView_Sheet1.Cells[nRows - 1, 8].Text = " ";
                        ssView_Sheet1.Cells[nRows - 1, 9].Text = " ";

                        nRows = nRows + 1;
                        if (nRows > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRows;
                        }

                        nSubJamt = 0;
                        nSubCnt = 0;
                        strGelCode2 = strGelCode1;
                        #endregion
                        strGelCode2 = strGelCode1;
                    }

                    nRows = nRows + 1;

                    if (nRows > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRows;
                    }

                    if (CPF.GET_BAS_MIA(clsDB.DbCon, strGelCode1) == "")
                    {
                        strMianame = "";
                    }
                    else
                    {
                        strMianame = CPF.GET_BAS_MIA(clsDB.DbCon, strGelCode1).Trim();
                    }

                    ssView_Sheet1.Cells[nRows - 1, 0].Text = strMianame;

                    if (strGelCode1 != strGelCode3)
                    {
                        ssView_Sheet1.Cells[nRows - 1, 0].Text = strMianame;
                        strGelCode3 = strGelCode1;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRows - 1, 0].Text = "";
                    }

                    ssView_Sheet1.Cells[nRows - 1, 1].Text = VB.Val(dt.Rows[i]["PANO"].ToString().Trim()).ToString("00000000");
                    ssView_Sheet1.Cells[nRows - 1, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[nRows - 1, 3].Text = Convert.ToDateTime(dt.Rows[i]["Fdate"].ToString().Trim()).ToString("yy-MM-dd");
                    ssView_Sheet1.Cells[nRows - 1, 3].Text += " ~ ";
                    ssView_Sheet1.Cells[nRows - 1, 3].Text += Convert.ToDateTime(dt.Rows[i]["Tdate"].ToString().Trim()).ToString("yy-MM-dd");
                    strIpdOpd = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    switch (strIpdOpd)
                    {
                        case "I":
                            ssView_Sheet1.Cells[nRows - 1, 4].Text = "입원";
                            break;
                        case "O":
                            ssView_Sheet1.Cells[nRows - 1, 4].Text = "외래";
                            break;
                        default:
                            ssView_Sheet1.Cells[nRows - 1, 4].Text = "";
                            break;
                    }

                    strPano = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[nRows - 1, 5].Text = CPF.Get_BasPatient(clsDB.DbCon, strPano).Rows[0]["PNAME"].ToString().Trim();

                    ssView_Sheet1.Cells[nRows - 1, 6].Text = dt.Rows[i]["CarNo"].ToString().Trim();
                    ssView_Sheet1.Cells[nRows - 1, 7].Text = nJinAmt.ToString("####,###,##0");

                    nSubJamt = Convert.ToDouble(nSubJamt + nJinAmt);
                    nSubCnt = nSubCnt + 1;
                    nTotJamt = Convert.ToDouble(nTotJamt + nJinAmt);
                    nTotCnt = nTotCnt + 1;

                    switch (strIpdOpd)
                    {
                        case "I":
                            nIpdJamt = nIpdJamt + nJinAmt;

                            nIpdCnt = nIpdCnt + 1;
                            break;

                        case "O":
                            nOpdJamt = Convert.ToDouble(nOpdJamt + nJinAmt);

                            nOpdCnt = nOpdCnt + 1;
                            break;
                    }

                    ssView_Sheet1.Cells[nRows - 1, 9].Text = dt.Rows[i]["deptCode1"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;

                if (nDataCnt > 0)
                {



                    ssView_Sheet1.RowCount = nRows + 5;
                    nRows = nRows + 1;
                    ssView_Sheet1.Rows[nRows - 1].BackColor = Color.FromArgb(80, 240, 180);
                    ssView_Sheet1.Cells[nRows - 1, 0].Text = "    ** 소    계 ** ";
                    ssView_Sheet1.Cells[nRows - 1, 1].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 2].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 3].Text = "    " + nSubCnt.ToString("##0") + " 건 ";
                    ssView_Sheet1.Cells[nRows - 1, 4].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 5].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 6].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 7].Text = nSubJamt.ToString("####,###,##0");
                    ssView_Sheet1.Cells[nRows - 1, 8].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 9].Text = " ";

                    nRows = nRows + 2;
                    ssView_Sheet1.Rows[nRows - 1].BackColor = Color.FromArgb(80, 240, 180);
                    ssView_Sheet1.Cells[nRows - 1, 0].Text = "    ** 외래합계 ** ";
                    ssView_Sheet1.Cells[nRows - 1, 1].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 2].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 3].Text = "    " + nOpdCnt.ToString("##0") + " 건 ";
                    ssView_Sheet1.Cells[nRows - 1, 4].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 5].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 6].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 7].Text = nOpdJamt.ToString("####,###,##0");
                    ssView_Sheet1.Cells[nRows - 1, 8].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 9].Text = " ";

                    nRows = nRows + 1;
                    ssView_Sheet1.Rows[nRows - 1].BackColor = Color.FromArgb(80, 240, 180);
                    ssView_Sheet1.Cells[nRows - 1, 0].Text = "    ** 입원합계 ** ";
                    ssView_Sheet1.Cells[nRows - 1, 1].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 2].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 3].Text = "    " + nIpdCnt.ToString("##0") + " 건 ";
                    ssView_Sheet1.Cells[nRows - 1, 4].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 5].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 6].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 7].Text = nIpdJamt.ToString("####,###,##0");
                    ssView_Sheet1.Cells[nRows - 1, 8].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 9].Text = " ";

                    nRows = nRows + 1;
                    ssView_Sheet1.Rows[nRows - 1].BackColor = Color.FromArgb(80, 240, 180);
                    ssView_Sheet1.Cells[nRows - 1, 0].Text = "    ** 합    계 ** ";
                    ssView_Sheet1.Cells[nRows - 1, 1].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 2].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 3].Text = "    " + nTotCnt.ToString("##0") + " 건 ";
                    ssView_Sheet1.Cells[nRows - 1, 4].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 5].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 6].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 7].Text = nTotJamt.ToString("####,###,##0");
                    ssView_Sheet1.Cells[nRows - 1, 8].Text = " ";
                    ssView_Sheet1.Cells[nRows - 1, 9].Text = " ";
                }
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "자보 진료비청구 총괄표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("해당년월 : " + cboYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + strDTP + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnRebut_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

