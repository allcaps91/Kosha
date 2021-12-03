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
    /// File Name       : frmPmpaViewGyeIpgumReport.cs
    /// Description     : 계약처 진료비 입금내역
    /// Author          : 박창욱
    /// Create Date     : 2017-10-16
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUR204.FRM(FrmIpgumReport.frm) >> frmPmpaViewGyeIpgumReport.cs 폼이름 재정의" />	
    public partial class frmPmpaViewGyeIpgumReport : Form
    {
        clsPmpaMisu cpm = new clsPmpaMisu();
        clsPmpaFunc cpf = new clsPmpaFunc();

        public frmPmpaViewGyeIpgumReport()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
         //  if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
         //   {
         //       return;  //권한확인
         //   }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "계약처 진료비 입금 현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력자:" + clsType.User.JobName + " 인 ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 100, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.80f);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strYYMM = "";
            string strFDate = "";
            string strTDate = "";
            //string strOldData = "";
            //string strNewData = "";
            int nRowCnt = 0;

            string strCoIPDOPD = "";
            string strCoGel = "";
            string strCoWrtno = "";
            string strFirst = "";
            int nCnt = 0;
            int nSCnt = 0;
            int nTCnt = 0;
            double nSTAmt = 0;
            double nSMAmt = 0;
            double nSIAmt = 0;
            double nSSAmt = 0;
            double nSJAmt = 0;
            double nSMSak = 0;

            double nTTAmt = 0;
            double nTMAmt = 0;
            double nTIAmt = 0;
            double nTSAmt = 0;
            double nTJAmt = 0;
            double nTMSak = 0;

            int nOTcnt = 0;
            double nOTTAmt = 0;
            double nOTMAmt = 0;
            double nOTIAmt = 0;
            double nOTSAmt = 0;
            double nOTJAmt = 0;
            double nOTMSak = 0;

            int nITcnt = 0;
            double nITTAmt = 0;
            double nITMAmt = 0;
            double nITIAmt = 0;
            double nITSAmt = 0;
            double nITJAmt = 0;
            double nITMSak = 0;

            string strIpdOpd = "";
            string strWrtno = "";
            string strPano = "";
            string strSName = "";
            string strGelName = "";
            string strGelCode = "";
            string strDeptCode = "";
            //string strBname = "";
            string strDate1 = "";
            string strDate2 = "";
            string strDate3 = "";
            string strDate4 = "";
            //double nIMAmt = 0;   //청구액(외래)
            //double nOMAmt = 0;   //청구액(입원)
            double nTAmt = 0;   //입금완료분 청구액
            double nMAmt = 0;   //청구액
            double nSAmt = 0;   //삭감액
            double nIAmt = 0;   //회입액
            double nJamt = 0;   //현잔액

            ssView_Sheet1.RowCount = 0;

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTDate = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(cboYYMM.Text, 4)), Convert.ToInt32(VB.Mid(cboYYMM.Text, 7, 2)));

            try
            {
                //해당월 마감여부 Checking
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Count(*) Cnt";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_MONTHLY";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND Class = '08'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0 || VB.Val(dt.Rows[0]["Cnt"].ToString().Trim()) == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당월의 통계가 형성되지 않았습니다.");
                    return;
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT WRTNO, GelCode, IpdOpd,";
                SQL = SQL + ComNum.VBLF + "       MisuID, TO_CHAR(Bdate,'YYYY-MM-DD') IDate, SUM(Amt) IAmt";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND Bdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND Bdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                if ( VB.Left(ComboClass.Text, 2) == "00")
                {
                    SQL = SQL + ComNum.VBLF + " AND Class >= '08'  ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND Class = '" + VB.Left(ComboClass.Text, 2) + "'  ";
                }
   

             //   SQL = SQL + ComNum.VBLF + "  AND Class = '08'";
                SQL = SQL + ComNum.VBLF + "  AND Gubun >= '21'";
                SQL = SQL + ComNum.VBLF + "  AND Gubun <= '29'";
                SQL = SQL + ComNum.VBLF + "GROUP BY WRTNO,GelCode,IpdOpd,MisuID,Bdate";
                SQL = SQL + ComNum.VBLF + "ORDER BY 2,3,4,5";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                strFirst = "OK";
                nRowCnt = dt.Rows.Count;

                for (i = 0; i < nRowCnt; i++)
                {
                    #region ADD_SPREAD

                    strWrtno = dt.Rows[i]["Wrtno"].ToString().Trim();

                    //월말 현재 잔액을 READ
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT JanAmt";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_Monthly";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = '" + strWrtno + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND YYMM  = '" + strYYMM + "'  ";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    nJamt = 0;
                    if (dt1.Rows.Count == 1)
                    {
                        nJamt = VB.Val(dt1.Rows[0]["JanAmt"].ToString().Trim());
                    }
                    dt1.Dispose();
                    dt1 = null;

                    cpm.READ_MISU_IDMST((long)VB.Val(strWrtno));

                    nCnt += 1;
                    nSCnt += 1;
                    nTCnt += 1;

                    strIpdOpd = clsPmpaType.TMM.IpdOpd;
                    strPano = clsPmpaType.TMM.MisuID;
                    strSName = cpf.Get_BasPatient(clsDB.DbCon, clsPmpaType.TMM.MisuID.Trim()).Rows[0]["SName"].ToString().Trim();
                    strGelCode = clsPmpaType.TMM.GelCode;
                    strDeptCode = clsPmpaType.TMM.DeptCode;
                    strDate1 = dt.Rows[i]["IDate"].ToString().Trim();   //입금일
                    strDate2 = clsPmpaType.TMM.BDate;                  //청구일
                    strDate3 = clsPmpaType.TMM.FromDate;               //진료기간(From)
                    strDate4 = clsPmpaType.TMM.ToDate;                 //진료기간(To)
                    nMAmt = clsPmpaType.TMM.Amt[2];                    //청구액
                    nIAmt = VB.Val(dt.Rows[i]["IAmt"].ToString().Trim());     //입금액
                    nSAmt = 0;
                    if (nJamt == 0)
                    {
                        nTAmt = clsPmpaType.TMM.Amt[2]; //청구액(입금완료분)
                        nSAmt = clsPmpaType.TMM.Amt[4];  //삭감액
                    }
                    if (clsPmpaType.TMM.IpdOpd == "O")
                    {
                        strIpdOpd = "외래";
                    }
                    if (clsPmpaType.TMM.IpdOpd == "I")
                    {
                        strIpdOpd = "입원";
                    }

                    strGelName = "";
                    if (strFirst == "OK")
                    {
                        strCoGel = strGelCode;
                        strCoIPDOPD = strIpdOpd;
                        strFirst = "NO";
                        strGelName = cpf.GET_BAS_MIA(clsDB.DbCon, strGelCode);
                        nCnt = 0;
                        nSCnt = 0;
                        nTCnt = 0;
                    }

                    if (strGelCode != strCoGel)
                    {
                       // Misu_Sub_IPDOPD_Rtn(strCoIPDOPD, nCnt, ref nSMAmt, ref nSIAmt, ref nSSAmt, nOTMSak, nOTSAmt, ref nSMSak, ref nSJAmt, ref nSTAmt, ref strCoGel, strGelCode);
                        Misu_Sub_Rtn(ref nSCnt, ref nTMAmt, ref nTIAmt, ref nTSAmt, ref nTTAmt, ref nTMSak, ref nTJAmt, ref strCoGel, strGelCode);
                        strCoIPDOPD = strIpdOpd;
                        strGelName = cpf.GET_BAS_MIA(clsDB.DbCon, strGelCode);
                    }

                    //if (strIpdOpd != strCoIPDOPD)
                    //{
                    //    Misu_Sub_IPDOPD_Rtn(strCoIPDOPD, nCnt, ref nSMAmt, ref nSIAmt, ref nSSAmt, nOTMSak, nOTSAmt, ref nSMSak, ref nSJAmt, ref nSTAmt, ref strCoGel, strGelCode);
                    //    strCoIPDOPD = strIpdOpd;
                    //}

                    if (strCoWrtno != strWrtno)
                    {
                        strCoWrtno = strWrtno;
                        ssView_Sheet1.RowCount += 1;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strGelName;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = strDate1;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = strDate2;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = VB.DateDiff("D", Convert.ToDateTime(strDate2), Convert.ToDateTime(strDate1)).ToString();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = strIpdOpd;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = strPano;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = strSName;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = strDate3 + "-" + strDate4;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = strDeptCode;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nMAmt.ToString();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nIAmt.ToString();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = nSAmt.ToString();
                        if (nMAmt != 0 && nSAmt != 0)
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = (nSAmt / nMAmt * 100).ToString("#0.00") + "%";
                        }
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = nJamt.ToString();

                        nSTAmt += nTAmt;
                        nSMAmt += nMAmt;
                        nSIAmt += nIAmt;
                        nSSAmt += nSAmt;
                        nSJAmt += nJamt;
                        if (nJamt < 1)
                        {
                            nSMSak += nMAmt;
                        }

                        nTTAmt += nTAmt;
                        nTMAmt += nMAmt;
                        nTIAmt += nIAmt;
                        nTSAmt += nSAmt;
                        nTJAmt += nJamt;
                        if (nJamt < 1)
                        {
                            nTMSak += nMAmt;
                        }

                        if (strIpdOpd == "입원")
                        {
                            nITcnt += 1;
                            nITTAmt += nTAmt;
                            nITMAmt += nMAmt;
                            nITIAmt += nIAmt;
                            nITSAmt += nSAmt;
                            nITJAmt += nJamt;
                            if (nJamt < 1)
                            {
                                nITMSak += nMAmt;
                            }
                        }
                        else
                        {
                            nOTcnt += 1;
                            nOTTAmt += nTAmt;
                            nOTMAmt += nMAmt;
                            nOTIAmt += nIAmt;
                            nOTSAmt += nSAmt;
                            nOTJAmt += nJamt;
                            if (nJamt < 1)
                            {
                                nOTMSak += nMAmt;
                            }
                        }
                    }

                    #endregion
                }
                dt.Dispose();
                dt = null;

               // Misu_Sub_IPDOPD_Rtn(strCoIPDOPD, nCnt, ref nSMAmt, ref nSIAmt, ref nSSAmt, nOTMSak, nOTSAmt, ref nSMSak, ref nSJAmt, ref nSTAmt, ref strCoGel, strGelCode);
                Misu_Sub_Rtn(ref nSCnt, ref nTMAmt, ref nTIAmt, ref nTSAmt, ref nTTAmt, ref nTMSak, ref nTJAmt, ref strCoGel, strGelCode);

                //ssView_Sheet1.RowCount += 1;
                //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "    외        래";
                //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nOTcnt + " 건";
                //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nOTMAmt.ToString();
                //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nOTIAmt.ToString();
                //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = nOTSAmt.ToString();
                //if (nOTMSak != 0 && nOTSAmt != 0)
                //{
                //    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = (nOTSAmt / nOTMSak * 100).ToString("#0.00") + "%";
                //}
                //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = nOTJAmt.ToString();

                //ssView_Sheet1.RowCount += 1;
                //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "    입        원";
                //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nITcnt + " 건";
                //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nITMAmt.ToString();
                //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nITIAmt.ToString();
                //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = nITSAmt.ToString();
                //if (nOTMSak != 0 && nOTSAmt != 0)
                //{
                //    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = (nITSAmt / nITMSak * 100).ToString("#0.00") + "%";
                //}
                //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = nITJAmt.ToString();

                ssView_Sheet1.RowCount += 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "    전        체";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = (nOTcnt + nITcnt) + " 건";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = (nOTMAmt + nITMAmt).ToString();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = (nOTIAmt + nITIAmt).ToString();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = (nOTSAmt + nITSAmt).ToString();
                if (nOTMSak != 0 && nOTSAmt != 0)
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = ((nOTSAmt + nITSAmt) / (nOTMSak + nITMSak) * 100).ToString("#0.00") + "%";
                }
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = (nOTJAmt + nITJAmt).ToString();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Misu_Sub_IPDOPD_Rtn(string strCoIPDOPD, int nCnt, ref double nSMAmt, ref double nSIAmt, ref double nSSAmt, double nOTMSak, double nOTSAmt, ref double nSMSak, ref double nSJAmt, ref double nSTAmt, ref string strCoGel, string strGelCode)
        {
            ssView_Sheet1.RowCount += 1;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = strCoIPDOPD + " 소계 ";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nCnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nSMAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nSIAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = nSSAmt.ToString();
            if (nOTMSak != 0 && nOTSAmt != 0)
            {
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = (nSSAmt / nSMSak * 100).ToString("#0.00") + "%";
            }
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = nSJAmt.ToString();

            nCnt = 0;
            nSTAmt = 0;
            nSMAmt = 0;
            nSIAmt = 0;
            nSSAmt = 0;
            nSJAmt = 0;
            nSMSak = 0;
            strCoGel = strGelCode;
        }

        void Misu_Sub_Rtn(ref int nSCnt, ref double nTMAmt, ref double nTIAmt, ref double nTSAmt, ref double nTTAmt, ref double nTMSak, ref double nTJAmt, ref string strCoGel, string strGelCode)
        {
            ssView_Sheet1.RowCount += 1;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "소  계";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nSCnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nTMAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nTIAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = nTSAmt.ToString();
            if (nTMSak != 0 && nTSAmt != 0)
            {
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = (nTSAmt / nTMSak * 100).ToString("#0.00") + "%";
            }
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = nTJAmt.ToString();

            nSCnt = 0;
            nTTAmt = 0;
            nTMAmt = 0;
            nTSAmt = 0;
            nTIAmt = 0;
            nTJAmt = 0;
            nTMSak = 0;
            strCoGel = strGelCode;
        }

        private void frmPmpaViewGyeIpgumReport_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 12, "", "1");
            cboYYMM.SelectedIndex = 0;


            ComboClass.Items.Clear();
            ComboClass.Items.Add("00.전체미수");
            ComboClass.Items.Add("08.계약처");
            ComboClass.Items.Add("09.헌혈미수");
            ComboClass.Items.Add("11.보훈청미수");
            ComboClass.Items.Add("12.시각장애자");
            ComboClass.Items.Add("13.심신장애진단비");
            ComboClass.Items.Add("14.장애인보장구");
            ComboClass.Items.Add("15.직원대납");
            ComboClass.Items.Add("16.노인장기요양소견서");
            ComboClass.Items.Add("17.방문간호지시서");
            ComboClass.Items.Add("18.치매검사");
            ComboClass.SelectedIndex = 1;


        
        }
    }
}
