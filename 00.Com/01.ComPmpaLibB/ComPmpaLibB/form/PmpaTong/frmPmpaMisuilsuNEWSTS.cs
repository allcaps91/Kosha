using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmpaMisuilsuNEWSTS : Form
    {

        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : frmPmpaMisuilsuNEWSTS.cs
        /// Description     : 청구 미수금 미수 일수 분석 (2013년까지)
        /// Author          : 김효성
        /// Create Date     : 2017-08-30
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "psmh\misu\misubs\misubs.vbp(청구미수금분석.FRM) >> frmPmpaMisuilsuNEWSTS.cs 폼이름 재정의" />	
        /// 
        string FstrFYYMM = "";
        string FstrTYYMM = "";
        string GstrRet = "";
        string GstrHelpCode = "";
        string GstrSQL = "";
        string GstrDate = "";
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc cf = new ComFunc();

        public frmPmpaMisuilsuNEWSTS(string strFYYMM, string strTYYMM, string strSQL, string strDate, string strHelpCode, string strRet)
        {
            string GstrHelpCode = strHelpCode;
            string GstrRet = strRet;
            string GstrDate = strDate;
            string GstrSQL = strSQL;
            string FstrFYYMM = strFYYMM;
            string FstrTYYMM = strTYYMM;

            InitializeComponent();
        }

        public frmPmpaMisuilsuNEWSTS()
        {
            InitializeComponent();
        }

        private void frmPmpaMisuilsuNEWSTS_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            int i = 0;
            int nYY = 0;
            int nMM = 0;

            nYY = (int)VB.Val(VB.Left(strDTP, 4));
            nMM = int.Parse(VB.Mid(strDTP, 6, 2)) - 1;

            cboYYYY.Items.Clear();

            for (i = 1; i <= 60; i++)
            {
                cboYYYY.Items.Add(nYY.ToString("0000") + "-" + nMM.ToString("00"));
                nMM = nMM - 1;

                if (nMM == 0)
                {
                    nYY = nYY - 1;
                    nMM = 12;
                }
            }
            cboYYYY.SelectedIndex = 0;

            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboYYYY.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            int i = 0;
            int j = 0;
            int nRow = 0;
            int nIlsu = 0;
            long nIlsuTot = 0;
            string strFd = "";
            double nSlipJan = 0;
            double nSlipJan2 = 0;                   //' 외래금액
            double nSlipJan3 = 0;                   //' 건진금액
            double nJanAmt = 0;             //' 미수잔액
            double nJanAmtTot = 0;                  //' 미수잔액 합계
            double nDayDiv = 0;             //' 평균미수일수
            int nReadTot = 0;
            double nAmtTot1 = 0;                    //' 외래금액
            double nAmtTot2 = 0;                    //' 건진금액
            double[] nData = new double[7];    //' 부분별 미수잔액합계
            double[] nAmtData1 = new double[7];        //' 전체미수잔액합계
            double[] nAmtData2 = new double[7];      //' 전체미수잔액합계

            nAmtTot1 = 0;
            nAmtTot2 = 0;
            nSlipJan2 = 0;
            nSlipJan3 = 0;

            for (i = 0; i <= 6; i++)
            {
                nAmtData1[i] = 0;
                nAmtData2[i] = 0;
            }
            FstrFYYMM = VB.Left(cboYYYY.Text, 4) + VB.Right(cboYYYY.Text, 2);
            strFd = cf.READ_LASTDAY(clsDB.DbCon, cboYYYY.Text + "-" + "01");

            if (int.Parse(FstrFYYMM) >= int.Parse("201401"))
            {
                ComFunc.MsgBox("2014년 이후부터는 <공무원, 사업장, 암검진> ==> <공단검진> 변경됩니다.", "2014년 이후는 다른 메뉴를 선택 해주세요");
                return;
            }

            CmdOK_Screen_Clear();

            for (j = 1; j <= 4; j++)
            {
                CmdOK_Data_Display(j, ref nReadTot, ref nIlsu, strFd, ref nIlsuTot, ref nJanAmt, ref nSlipJan, ref nSlipJan2, ref nJanAmtTot, ref nAmtTot1, ref nData, ref nAmtData1, ref nDayDiv, ref nSlipJan3, ref nAmtTot2, ref nRow);               //'일반 미수
            }
            for (j = 1; j <= 9; j++)
            {
                CmdOK_Data_Display3(j, nReadTot, ref nIlsu, strFd, ref nIlsuTot, ref nJanAmt, ref nSlipJan, ref nSlipJan3, ref nJanAmtTot, ref nAmtTot2, ref nData, ref nAmtData2, ref nDayDiv, ref nSlipJan2, ref nRow);            //'건진 미수
            }
            Total_All_TOT_Rtn(ref nRow, nAmtTot1, nAmtTot2, nAmtData1, nAmtData2);                  //'일반+센터 합계
        }

        #region GoSub
        private void CmdOK_Screen_Clear() //'화면을 Clear
        {
            ssView_Sheet1.Cells[0, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
        }

        private void CmdOK_Data_Display(int j, ref int nReadTot, ref int nIlsu, string strFd, ref long nIlsuTot, ref double nJanAmt, ref double nSlipJan, ref double nSlipJan2, ref double nJanAmtTot, ref double nAmtTot1, ref double[] nData, ref double[] nAmtData1, ref double nDayDiv, ref double nSlipJan3, ref double nAmtTot2, ref int nRow)  //' 발생일별 미수 상세내역 Display
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.WRTNO,a.Class,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate,";
                SQL = SQL + ComNum.VBLF + " a.JanAmt ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_MONTHLY a, " + ComNum.DB_PMPA + "MISU_IDMST b  ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + " AND a.YYMM = '" + FstrFYYMM + "' ";

                if (otp1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND A.IPDOPD ='I'";
                }
                if (otp2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND A.IPDOPD ='O'";
                }

                SQL = SQL + ComNum.VBLF + "   AND a.WRTNO = b.WRTNO ";
                SQL = SQL + ComNum.VBLF + "   AND a.JanAmt <> 0  ";

                if (j == 1)
                {
                    SQL = SQL + ComNum.VBLF + " AND a.Class in('01','02','03') ";   //'공단,직장,지역
                }
                else if (j == 2)
                {
                    SQL = SQL + ComNum.VBLF + " AND a.Class = '04' ";   //'보호
                }
                else if (j == 3)
                {
                    SQL = SQL + ComNum.VBLF + " AND a.Class = '07' ";   //'자보
                }
                else if (j == 4)
                {
                    SQL = SQL + ComNum.VBLF + " AND a.Class = '05' ";   //'산재
                }

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nReadTot = nReadTot + dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nIlsu = (int)VB.DateDiff("d", Convert.ToDateTime(dt.Rows[i]["BDATE"].ToString().Trim()), Convert.ToDateTime(strFd));
                    nIlsuTot = nIlsuTot + nIlsu;
                    nJanAmt = VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());
                    nSlipJan = nSlipJan + (nIlsu * nJanAmt);
                    nSlipJan2 = nSlipJan2 + (nIlsu * nJanAmt);
                    nJanAmtTot = nJanAmtTot + nJanAmt;
                    nAmtTot1 = nAmtTot1 + nJanAmt;

                    if (nIlsu >= 0 && nIlsu <= 30)
                    {
                        nData[1] = nData[1] + nJanAmt;
                        nAmtData1[1] = nAmtData1[1] + nJanAmt;
                    }
                    else if (nIlsu >= 31 && nIlsu <= 60)
                    {
                        nData[2] = nData[2] + nJanAmt;
                        nAmtData1[2] = nAmtData1[2] + nJanAmt;
                    }
                    else if (nIlsu >= 61 && nIlsu <= 90)
                    {
                        nData[3] = nData[3] + nJanAmt;
                        nAmtData1[3] = nAmtData1[3] + nJanAmt;
                    }
                    else if (nIlsu >= 91 && nIlsu <= 180)
                    {
                        nData[4] = nData[4] + nJanAmt;
                        nAmtData1[4] = nAmtData1[4] + nJanAmt;
                    }
                    else if (nIlsu >= 181 && nIlsu <= 365)
                    {
                        nData[5] = nData[5] + nJanAmt;
                        nAmtData1[5] = nAmtData1[5] + nJanAmt;
                    }
                    else if (nIlsu > 365)
                    {
                        nData[6] = nData[6] + nJanAmt;
                        nAmtData1[6] = nAmtData1[6] + nJanAmt;
                    }
                }

                if (nIlsuTot != 0)
                {
                    nDayDiv = (int)(nSlipJan / nJanAmtTot);
                }

                Total_TOT_Rtn(ref nRow, ref nJanAmtTot, ref nData, ref nDayDiv, ref nIlsuTot, ref nIlsu, ref nSlipJan);

                if (j == 4)
                {
                    nJanAmtTot = nAmtTot1;
                    for (i = 1; i <= 6; i++)
                    {
                        nData[i] = nAmtData1[i];
                    }
                    if (nSlipJan3 == 0 && nAmtTot2 == 0)
                    {
                        nDayDiv = 0;
                    }
                    else
                    {
                        nDayDiv = (int)(nSlipJan2 / nAmtTot1); //소계 미수일수
                    }
                    nAmtData1[0] = nDayDiv;
                    Total_TOT_Rtn(ref nRow, ref nJanAmtTot, ref nData, ref nDayDiv, ref nIlsuTot, ref nIlsu, ref nSlipJan);  //소계 display
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }


        private void CmdOK_Data_Display3(int j, int nReadTot, ref int nIlsu, string strFd, ref long nIlsuTot, ref double nJanAmt, ref double nSlipJan, ref double nSlipJan3, ref double nJanAmtTot, ref double nAmtTot2, ref double[] nData, ref double[] nAmtData2, ref double nDayDiv, ref double nSlipJan2, ref int nRow)  //'  발생일별 미수 상세내역 Display
        {
            int i = 0;

            DataTable dtFu = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.JanAmt ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a, " + ComNum.DB_PMPA + "HIC_MISU_MST b, " + ComNum.DB_PMPA + "HIC_EXJONG c ";
                SQL = SQL + ComNum.VBLF + " WHERE a.YYMM='" + FstrFYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO ";
                SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                SQL = SQL + ComNum.VBLF + "   AND a.JanAmt <> 0 ";

                if (j == 1)
                {
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG = '83'    ";
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '7'    ";    //'종검
                }

                else if (j == 2)
                {
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";

                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '1'    ";    //'성인병
                }

                else if (j == 3)
                {
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";

                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '2'    ";    //'공무원
                }

                else if (j == 4)
                {
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";

                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '3'    ";    //'사업장
                }

                else if (j == 5)
                {
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";

                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '4'    ";    //'암검진

                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG IN ('31','35')    ";
                }

                else if (j == 6)
                {
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";

                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '4'    "; //    '학생검진

                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG IN ('56')    ";
                }

                else if (j == 7)
                {
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";

                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '4'    ";    //'기타검진

                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG NOT IN ('31','35','56')    ";
                }

                else if (j == 8)
                {
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";

                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '5'    "; //    '작업측정
                }
                else if (j == 9)
                {
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";

                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '6'    "; // '보건대행
                }


                SQL = clsDB.GetDataTable(ref dtFu, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nReadTot = nReadTot + dtFu.Rows.Count;

                for (i = 0; i < dtFu.Rows.Count; i++)
                {
                    nIlsu = (int)VB.DateDiff("d", Convert.ToDateTime(dtFu.Rows[i]["BDATE"].ToString().Trim()), Convert.ToDateTime(strFd));

                    nIlsuTot = (nIlsuTot + nIlsu);
                    nJanAmt = VB.Val(dtFu.Rows[i]["JanAmt"].ToString().Trim());
                    nSlipJan = nSlipJan + (nIlsu * nJanAmt);
                    nSlipJan3 = nSlipJan3 + (nIlsu * nJanAmt);
                    nJanAmtTot = (nJanAmtTot + nJanAmt);
                    nAmtTot2 = (nAmtTot2 + nJanAmt);

                    if (nIlsu >= 0 && nIlsu <= 30)
                    {
                        nData[1] = nData[1] + nJanAmt;
                        nAmtData2[1] = nAmtData2[1] + nJanAmt;
                    }
                    else if (nIlsu >= 31 && nIlsu <= 60)
                    {
                        nData[2] = nData[2] + nJanAmt;
                        nAmtData2[2] = nAmtData2[2] + nJanAmt;
                    }
                    else if (nIlsu >= 61 && nIlsu <= 90)
                    {
                        nData[3] = nData[3] + nJanAmt;
                        nAmtData2[3] = nAmtData2[3] + nJanAmt;
                    }
                    else if (nIlsu >= 91 && nIlsu <= 180)
                    {
                        nData[4] = nData[4] + nJanAmt;
                        nAmtData2[4] = nAmtData2[4] + nJanAmt;
                    }
                    else if (nIlsu >= 181 && nIlsu <= 365)
                    {
                        nData[5] = nData[5] + nJanAmt;
                        nAmtData2[5] = nAmtData2[5] + nJanAmt;
                    }
                    else if (nIlsu > 365)
                    {
                        nData[6] = nData[6] + nJanAmt;
                        nAmtData2[6] = nAmtData2[6] + nJanAmt;
                    }
                }

                if (nIlsuTot != 0)
                {
                    nDayDiv = (int)(nSlipJan / nJanAmtTot);
                }

                Total_TOT_Rtn(ref nRow, ref nJanAmtTot, ref nData, ref nDayDiv, ref nIlsuTot, ref nIlsu, ref nSlipJan);

                if (j == 9)
                {
                    nJanAmtTot = nAmtTot2;

                    for (i = 1; i <= 6; i++)
                    {
                        nData[i] = nAmtData2[i];
                    }

                    if (nSlipJan3 == 0 && nAmtTot2 == 0)
                    {
                        nDayDiv = 0;
                    }
                    else
                    {
                        nDayDiv = (int)(nSlipJan3 / nAmtTot2); //소계 미수일수
                    }
                    nAmtData2[0] = nDayDiv;
                    Total_TOT_Rtn(ref nRow, ref nJanAmtTot, ref nData, ref nDayDiv, ref nIlsuTot, ref nIlsu, ref nSlipJan);  //소계 Display
                }

                dtFu.Dispose();
                dtFu = null;


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Total_TOT_Rtn(ref int nRow, ref double nJanAmtTot, ref double[] nData, ref double nDayDiv, ref long nIlsuTot, ref int nIlsu, ref double nSlipJan)  //'합계
        {
            nRow = nRow + 1;
            ssView_Sheet1.Cells[nRow - 1, 1].Text = nJanAmtTot.ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 2].Text = nData[1].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 3].Text = nData[2].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 4].Text = nData[3].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 5].Text = nData[4].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 6].Text = nData[5].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 7].Text = nData[6].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 8].Text = nDayDiv.ToString();

            nJanAmtTot = 0;
            nDayDiv = 0;
            nIlsuTot = 0;
            nIlsu = 0;
            nSlipJan = 0;

            for (int i = 1; i <= 6; i++)
            {
                nData[i] = 0;
            }

        }

        private void Total_All_TOT_Rtn(ref int nRow, double nAmtTot1, double nAmtTot2, double[] nAmtData1, double[] nAmtData2) //  '외래+센터 합계
        {
            nRow = nRow + 1;

            ssView_Sheet1.Cells[nRow - 1, 1].Text = (nAmtTot1 + nAmtTot2).ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 2].Text = (nAmtData1[1] + nAmtData2[1]).ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 3].Text = (nAmtData1[2] + nAmtData2[2]).ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 4].Text = (nAmtData1[3] + nAmtData2[3]).ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 5].Text = (nAmtData1[4] + nAmtData2[4]).ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 6].Text = (nAmtData1[5] + nAmtData2[5]).ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 7].Text = (nAmtData1[6] + nAmtData2[6]).ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 8].Text = ((int)(nAmtData1[0] + nAmtData2[0]) / 2).ToString();
        }
        #endregion

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            Cursor.Current = Cursors.WaitCursor;

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";
            string sFont3 = "";
            string sFoot = "";

            DateTime mdtp;
            mdtp = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            strFont1 = "/c/fn\"굴림체\" /fz\"16\"  /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + cboYYYY.Text + " 청구미수금 미수일수 분석 " + "/f1/n";
            strHead2 = strHead2 + "/n/c" + "인쇄일자 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A") + VB.Space(20) + "PAGE :" + "/p";

            btnPrint.Enabled = false;

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2 + "/n";
            ssView_Sheet1.PrintInfo.Footer = sFont3 + sFoot;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            int j = 0;
            string strFDate = "";
            string strTdate = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            GstrSQL = "";
            GstrDate = "";

            strFDate = VB.Left(cboYYYY.Text, 4) + "-" + VB.Right(cboYYYY.Text, 2) + "-01";
            strTdate = cf.READ_LASTDAY(clsDB.DbCon, cboYYYY.Text + "-" + "01");

            //'말일기준으로 일자 설정
            switch (e.Column)
            {
                case 3: //31 ~ 60 일
                    strFDate = (Convert.ToDateTime(strTdate).AddDays(-30)).ToString();
                    strTdate = strTdate;
                    break;
                case 4: //61 ~ 90 일
                    strFDate = (Convert.ToDateTime(strTdate).AddDays(-60)).ToString();
                    strTdate = (Convert.ToDateTime(strTdate).AddDays(-30)).ToString();
                    break;
                case 5: //91 ~ 180 일
                    strFDate = (Convert.ToDateTime(strTdate).AddDays(-90)).ToString();
                    strTdate = (Convert.ToDateTime(strTdate).AddDays(-60)).ToString();
                    break;
                case 6: //91 ~ 180 일
                    strFDate = (Convert.ToDateTime(strTdate).AddDays(-180)).ToString();
                    strTdate = (Convert.ToDateTime(strTdate).AddDays(-90)).ToString();
                    break;
                case 7: //91 ~ 180 일
                    strFDate = (Convert.ToDateTime(strTdate).AddDays(-365)).ToString();
                    strTdate = (Convert.ToDateTime(strTdate).AddDays(-180)).ToString();
                    break;
                case 8: //91 ~ 180 일
                    strFDate = (Convert.ToDateTime(strTdate).AddDays(-5000)).ToString();
                    strTdate = (Convert.ToDateTime(strTdate).AddDays(-365)).ToString();
                    break;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (e.Row == 0 || e.Row == 1 || e.Row == 2 || e.Row == 3)
                {
                    SQL = SQL + ComNum.VBLF + "SELECT a.YYMM,  B.MISUID  WRTNO,a.Class,a.ipdopd,a.GelCode,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate,";
                    SQL = SQL + ComNum.VBLF + "       b.deptcode,a.ipgumamt,b.amt3,a.yymm,a.iwolamt,a.misuamt,a.sakamt,a.etcamt,  ";
                    SQL = SQL + ComNum.VBLF + "       a.janamt,a.banamt,b.remark ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_MONTHLY a, " + ComNum.DB_PMPA + "MISU_IDMST b  ";
                    SQL = SQL + ComNum.VBLF + "   WHERE a.YYMM = '" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND a.WRTNO = b.WRTNO ";

                    if (otp1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND A.IPDOPD ='I'";
                    }
                    if (otp2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND A.IPDOPD ='O'";
                    }

                    SQL = SQL + ComNum.VBLF + "     AND a.JanAmt <> 0  ";
                    if (e.Row == 0)
                    {
                        SQL = SQL + ComNum.VBLF + " AND a.Class in('01','02','03') ";   //'공단,직장,지역
                    }
                    else if (e.Row == 1)
                    {
                        SQL = SQL + ComNum.VBLF + " AND a.Class = '04' ";//   '보호
                    }
                    else if (e.Row == 2)
                    {
                        SQL = SQL + ComNum.VBLF + " AND a.Class = '07' ";   //'자보
                    }
                    else if (e.Row == 3)
                    {
                        SQL = SQL + ComNum.VBLF + " AND a.Class = '05' ";   //'산재

                    }
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE < TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }
                else if (e.Row == 5)
                {
                    SQL = SQL + ComNum.VBLF + " SELECT a.YYMM, a.WRTNO,'' Class , '' ipdopd,  TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.yymm, ";
                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt, 0 etcamt,  ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt,b.remark ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a," + ComNum.DB_PMPA + "HIC_MISU_MST b, " + ComNum.DB_PMPA + "HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND a.JanAmt <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG='83'    ";
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '7'    ";//    '종검
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE < TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.Wrtno,b.BDate ";
                }

                else if (e.Row == 6)
                {
                    SQL = SQL + ComNum.VBLF + " SELECT a.YYMM, a.WRTNO, '' Class, '' ipdopd,  TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.yymm, ";
                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt,  0 etcamt, ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt ,b.remark";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a," + ComNum.DB_PMPA + "HIC_MISU_MST b, " + ComNum.DB_PMPA + "HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND a.JanAmt <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '1'              ";      //'성인병
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE < TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }
                else if (e.Row == 7)
                {
                    SQL = SQL + ComNum.VBLF + " SELECT a.YYMM, a.WRTNO, '' Class , '' ipdopd,  TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.yymm, ";
                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt, 0 etcamt, ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt,b.remark ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a," + ComNum.DB_PMPA + "HIC_MISU_MST b, " + ComNum.DB_PMPA + "HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND a.JanAmt <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '2'    ";   // '공무원
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE < TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }
                else if (e.Row == 8)
                {
                    SQL = SQL + ComNum.VBLF + " SELECT a.YYMM, a.WRTNO, '' Class, '' ipdopd, TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.yymm, ";
                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt,  0 etcamt, ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt ,b.remark";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a," + ComNum.DB_PMPA + "HIC_MISU_MST b, " + ComNum.DB_PMPA + "HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND a.JanAmt <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '3'    ";    //'사업장
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE < TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }
                else if (e.Row == 9)
                {
                    SQL = SQL + ComNum.VBLF + " SELECT a.YYMM,  a.WRTNO, '' Class, '' ipdopd,  TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.yymm, ";
                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt,  0 etcamt, ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt,b.remark ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a," + ComNum.DB_PMPA + "HIC_MISU_MST b, " + ComNum.DB_PMPA + "HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND a.JanAmt <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '4'    ";  //		    '기타검진
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG NOT IN ('31','35','56')    ";
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE < TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }
                else if (e.Row == 10)
                {
                    SQL = SQL + ComNum.VBLF + " SELECT a.YYMM, a.WRTNO, '' Class, '' ipdopd, TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.yymm, ";
                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt,  0 etcamt, ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt ,b.remark";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a," + ComNum.DB_PMPA + "HIC_MISU_MST b, " + ComNum.DB_PMPA + "HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND a.JanAmt <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '5'    ";//		    '작업측정
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE < TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }
                else if (e.Row == 11)
                {
                    SQL = SQL + ComNum.VBLF + " SELECT a.YYMM, a.WRTNO,'' Class, '' ipdopd,  TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.yymm, ";
                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt, 0 etcamt,  ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt,b.remark ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a," + ComNum.DB_PMPA + "HIC_MISU_MST b, " + ComNum.DB_PMPA + "HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND a.JanAmt <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '6'    ";  //'보건대행
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE < TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }
                else if (e.Row == 12)
                {
                    SQL = SQL + ComNum.VBLF + " SELECT a.YYMM, a.WRTNO, '' Class, '' ipdopd, TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.yymm, ";
                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt,  0 etcamt, ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt ,b.remark";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a," + ComNum.DB_PMPA + "HIC_MISU_MST b, " + ComNum.DB_PMPA + "HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND a.JanAmt <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '5'    ";    //'작업측정
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE < TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }
                else if (e.Row == 13)
                {
                    SQL = SQL + ComNum.VBLF + " SELECT a.YYMM, a.WRTNO,'' Class, '' ipdopd,  TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.yymm, ";
                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt, 0 etcamt,  ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt,b.remark ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a," + ComNum.DB_PMPA + "HIC_MISU_MST b, " + ComNum.DB_PMPA + "HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND a.JanAmt <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '6'    ";         //'보건대행


                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";

                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE < TO_DATE('" + strTdate + "','YYYY-MM-DD')";

                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }
                GstrSQL = SQL;
                GstrHelpCode = e.Column.ToString();

                if (GstrSQL != "")
                {
                    GstrRet = e.Row.ToString();
                    //TODO : FrmIpgemGiganDetail.Show 1
                }

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BSNSCLS"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            //스프레드 출력문
            ssView_Sheet1.RowCount = dt.Rows.Count;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BSNSCLS"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.Cells[0, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
        }
    }
}
