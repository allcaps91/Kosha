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
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-10-12
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\misu\misubs\misubs.vbp\misubs51.frm(FrmTongIpdMirCheck.frm)" >> frmSupLbExSTS15.cs 폼이름 재정의" />
    public partial class frmPmpaViewmisubs51 : Form
    {
        double[,] FnAmt = new double[16, 6];

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu cpm = new clsPmpaMisu();

        string GstrYYMM = "";
        //string GstrMenu = "";
        //string GstrSMenu = "";

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaViewmisubs51()
        {
            InitializeComponent(); 
        }

        public frmPmpaViewmisubs51(string strstrYYMM, string strstrMenu, string strstrSMenu)
        {
            string GstrYYMM = strstrYYMM;
            string GstrMenu = strstrMenu;
            string GstrSMenu = strstrSMenu;

            InitializeComponent();
        }

        private void frmPmpaViewmisubs51_Load(object sender, System.EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            panlHelp.Visible = false;

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFDate, 24, "", "1");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTDate, 24, "", "1");

            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboFDate.DropDownStyle = ComboBoxStyle.DropDown;
                    cboTDate.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }

        }

        private void SS_SET()
        {
            if (chk0.Checked == true) //'건강보험
            {
                ssView_Sheet1.Rows[0].Visible = true;
                ssView_Sheet1.Rows[5].Visible = true;
                ssView_Sheet1.Rows[10].Visible = true;
                ssPrint_Sheet1.Rows[6].Visible = true;
                ssPrint_Sheet1.Rows[11].Visible = true;
                ssPrint_Sheet1.Rows[16].Visible = true;
            }
            else
            {
                ssView_Sheet1.Rows[0].Visible = false;
                ssView_Sheet1.Rows[5].Visible = false;
                ssView_Sheet1.Rows[10].Visible = false;
                ssPrint_Sheet1.Rows[6].Visible = false;
                ssPrint_Sheet1.Rows[11].Visible = false;
                ssPrint_Sheet1.Rows[16].Visible = false;
            }

            if (chk1.Checked == true) //'의료급여
            {
                ssView_Sheet1.Rows[1].Visible = true;
                ssView_Sheet1.Rows[6].Visible = true;
                ssView_Sheet1.Rows[11].Visible = true;
                ssPrint_Sheet1.Rows[7].Visible = true;
                ssPrint_Sheet1.Rows[12].Visible = true;
                ssPrint_Sheet1.Rows[17].Visible = true;

            }
            else
            {
                ssView_Sheet1.Rows[1].Visible = false;
                ssView_Sheet1.Rows[6].Visible = false;
                ssView_Sheet1.Rows[11].Visible = false;
                ssPrint_Sheet1.Rows[7].Visible = false;
                ssPrint_Sheet1.Rows[12].Visible = false;
                ssPrint_Sheet1.Rows[17].Visible = false;
            }

            if (chk2.Checked == true) //자보
            {
                ssView_Sheet1.Rows[3].Visible = true;
                ssView_Sheet1.Rows[8].Visible = true;
                ssView_Sheet1.Rows[13].Visible = true;
                ssPrint_Sheet1.Rows[9].Visible = true;
                ssPrint_Sheet1.Rows[14].Visible = true;
                ssPrint_Sheet1.Rows[19].Visible = true;
            }
            else
            {
                ssView_Sheet1.Rows[3].Visible = false;
                ssView_Sheet1.Rows[8].Visible = false;
                ssView_Sheet1.Rows[13].Visible = false;
                ssPrint_Sheet1.Rows[9].Visible = false;
                ssPrint_Sheet1.Rows[14].Visible = false;
                ssPrint_Sheet1.Rows[19].Visible = false;
            }

            if (chk3.Checked == true) //산재
            {
                ssView_Sheet1.Rows[2].Visible = true;
                ssView_Sheet1.Rows[7].Visible = true;
                ssView_Sheet1.Rows[12].Visible = true;
                ssPrint_Sheet1.Rows[8].Visible = true;
                ssPrint_Sheet1.Rows[13].Visible = true;
                ssPrint_Sheet1.Rows[18].Visible = true;

            }
            else
            {
                ssView_Sheet1.Rows[2].Visible = false;
                ssView_Sheet1.Rows[7].Visible = false;
                ssView_Sheet1.Rows[12].Visible = false;
                ssPrint_Sheet1.Rows[8].Visible = false;
                ssPrint_Sheet1.Rows[13].Visible = false;
                ssPrint_Sheet1.Rows[18].Visible = false;
            }

            ssView_Sheet1.Columns[0].Visible = true;

            //Clear
            ssView_Sheet1.Cells[0, 2, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
            ssPrint_Sheet1.Cells[6, 2, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

            btnMenuRemark.Enabled = false;
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            int j = 0;
            int nBiNo = 0;
            double nAmt = 0;
            string strYYMM = "";
            string strFYYMM = "";
            string strTYYMM = "";
            string strFDate = "";
            string strTdate = "";
            string strBi = "";
            string strBiGbn = "";
            string SQL = "";    //Query문

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;

            //'누적할 배열을 Clear
            for (i = 1; i <= 15; i++)
            {
                for (j = 1; j <= 5; j++)
                {
                    FnAmt[i, j] = 0;
                }
            }


            strYYMM = VB.Left(cboFDate.Text, 4) + VB.Mid(cboFDate.Text, 7, 2);
            strFYYMM = VB.Left(cboFDate.Text, 4) + VB.Mid(cboFDate.Text, 7, 2);
            strFDate = VB.Left(strFYYMM, 4) + "-" + VB.Right(strFYYMM, 2) + "-01";
            strTYYMM = VB.Left(cboTDate.Text, 4) + VB.Mid(cboTDate.Text, 7, 2);
            strTdate = CF.READ_LASTDAY(clsDB.DbCon, VB.Left(strTYYMM, 4) + "-" + VB.Right(strTYYMM, 2) + "-01");

            //'jjy(2003-01-13) '통계 remark 등록 공용변수
            GstrYYMM = strFYYMM;
            //GstrMenu = "4";
            //GstrSMenu = "1";
            try
            {

                btnSearch.Enabled = false;

                CmdView_Slip_ADD(strYYMM, ref strBiGbn, strFDate, strTdate, ref nBiNo, ref nAmt);         //'퇴원자 조합부담금
                CmdView_JungganMir_ADD(strFYYMM, strTYYMM, ref strBiGbn, ref nBiNo, ref nAmt, strYYMM);   //'중간청구 조합부담금
                CmdView_Em6TimveOver_ADD(strFYYMM, ref strBiGbn, strFDate, strTdate, ref nBiNo, ref nAmt); //'응급실6시간이상,NP낮병동
                CmdView_SilMir_ADD(ref strBiGbn, strFYYMM, strTYYMM, ref strBi, ref nAmt, strYYMM);        //'당월분 실청구액

                for (i = 1; i <= 15; i++)
                {
                    //  '청구차액=미수발생액 - 발생미수액
                    FnAmt[i, 3] = FnAmt[i, 2] - FnAmt[i, 1];
                    //'EDI차액=발생미수액 - EDI접수액
                    FnAmt[i, 5] = FnAmt[i, 2] - FnAmt[i, 4];
                    //SS1.Row = i
                    for (j = 1; j <= 5; j++)
                    {
                        //  SS1.Col = j + 2
                        if (i == 7 && j == 4)
                        {
                            //i = i;
                        }
                        ssView_Sheet1.Cells[i - 1, j + 1].Text = FnAmt[i, j].ToString("###,###,###,##0 ");
                    }
                }
                ssView_Sheet1.Cells[0, 7].Text = " ";

                btnSearch.Enabled = true;
                btnMenuRemark.Enabled = true;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        #region 조회 관련 서브 함수

        /// <summary>
        /// 입원 당월퇴원 조합부담 발생액을 MISU_BALDAILY에서 READ
        /// </summary>
        /// <param name="strYYMM"></param>
        /// <param name="strBiGbn"></param>
        /// <param name="strFDate"></param>
        /// <param name="strTdate"></param>
        /// <param name="nBiNo"></param>
        /// <param name="nAmt"></param>
        private void CmdView_Slip_ADD(string strYYMM, ref string strBiGbn, string strFDate, string strTdate, ref int nBiNo, ref double nAmt)
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            if (string.Compare(strYYMM, "200112") == 0)
            {
                return;
            }
            //'입원 당월퇴원 조합부담 발생액을 MISU_BALDAILY에서 READ
            //'BIGBN   종류(1.보험 2.보호 3.산재 4.자보 5.일반)


            strBiGbn = "''";
            if (chk0.Checked == true)
            {
                strBiGbn = strBiGbn + ",'1','5'";
            }
            if (chk1.Checked == true)
            {
                strBiGbn = strBiGbn + ",'2'";
            }
            if (chk3.Checked == true)
            {
                strBiGbn = strBiGbn + ",'3'";
            }
            if (chk2.Checked == true)
            {
                strBiGbn = strBiGbn + ",'4'";
            }


            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT BiGbn,SUM(Amt33) Amt ";
            ;
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALDAILY ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "  AND  ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  AND ActDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  AND IpdOpd='I' ";
            if (strBiGbn != "")
            {
                SQL = SQL + ComNum.VBLF + " AND BIGBN IN (" + strBiGbn + " ) ";
            }


            SQL = SQL + ComNum.VBLF + "GROUP BY BiGbn ";


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

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nBiNo = (int)VB.Val(dt.Rows[i]["BiGbn"].ToString().Trim());
                nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                if (nAmt != 0)
                {
                    switch (nBiNo)
                    {
                        case 1:
                            j = 1;  //보험
                            break;
                        case 2:
                            j = 2;  //보호
                            break;
                        case 3:
                            j = 3;  //산재
                            break;
                        case 4:
                            j = 4;  //자보
                            break;
                        default:
                            j = 1;  //기타는 보험으로
                            break;
                    }
                }
                FnAmt[j, 1] += nAmt;
                FnAmt[5, 1] += nAmt;
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

        }

        /// <summary>
        /// 중간청구 조합부담금
        /// </summary>
        /// <param name="strFYYMM"></param>
        /// <param name="strTYYMM"></param>
        /// <param name="strBiGbn"></param>
        /// <param name="nBiNo"></param>
        /// <param name="nAmt"></param>
        /// <param name="strYYMM"></param>
        private void CmdView_JungganMir_ADD(string strFYYMM, string strTYYMM, ref string strBiGbn, ref int nBiNo, ref double nAmt, string strYYMM)
        {
            int i = 0;
            int j = 0;
            string strJungFDate = "";
            string strJungTDate = "";
            string strOK = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int nRead = 0;



            strJungFDate = Convert.ToDateTime(clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strFYYMM, 4)), Convert.ToInt32(VB.Right(strFYYMM, 2)))).AddDays(1).ToString("yyyy-MM-dd");
            strJungTDate = Convert.ToDateTime(clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strTYYMM, 4)), Convert.ToInt32(VB.Right(strTYYMM, 2)))).AddDays(1).ToString("yyyy-MM-dd");
            strJungTDate = Convert.ToDateTime(clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strJungTDate, 4)), Convert.ToInt32(VB.Mid(strJungTDate, 6, 2)))).AddDays(1).ToString("yyyy-MM-dd");

            //SUBI     구분(1.보험 2.보호 3.산재 4.자보 5.일반)
            strBiGbn = "''";
            if (chk0.Checked == true)
            {
                strBiGbn += ",'1','5'";
            }
            if (chk1.Checked == true)
            {
                strBiGbn += ",'2'";
            }
            if (chk3.Checked == true)
            {
                strBiGbn += ",'3'";
            }
            if (chk2.Checked == true)
            {
                strBiGbn += ",'4'";
            }

            //일월 1일부터 말일까지 중간청구 Build 조합부담 발생액을 Read
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT Bi, SUBI, SUM(BuildJAmt) Amt, SUM(JepJAmt) JepJAmt ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MIR_IPDID";
            SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
            SQL = SQL + ComNum.VBLF + "   AND YYMM>='200112'";
            SQL = SQL + ComNum.VBLF + "   AND BuildDate>=TO_DATE('" + strJungFDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND BuildDate< TO_DATE('" + strJungTDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND Flag='1' ";      //청구Build한 내역
            if (strBiGbn != "")
            {
                SQL = SQL + ComNum.VBLF + " AND SUBI IN (" + strBiGbn + " ) ";
            }
            SQL = SQL + ComNum.VBLF + "GROUP BY Bi, SUBI, Pano ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            nRead = dt.Rows.Count;
            for (i = 0; i < nRead; i++)
            {
                strOK = "NO";
                nBiNo = (int)VB.Val(dt.Rows[i]["SUBI"].ToString().Trim());
                if (dt.Rows[i]["SUBI"].ToString().Trim() == "")
                {
                    nBiNo = cpm.READ_Bi_SuipTong(dt.Rows[i]["Bi"].ToString().Trim(), strJungFDate);
                }
                nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());

                if (strYYMM == "200306" || nAmt != 0)
                {
                    strOK = "OK";
                }
                if (strYYMM == "200412" && nAmt == 0)
                {
                    strOK = "OK";
                }
                if (strYYMM == "201103" || nAmt == 0)
                {
                    strOK = "OK";
                }
                //경리과 요청(2005-02-02) 김혜향 요청

                if (strOK == "OK")
                {
                    switch (nBiNo)
                    {
                        case 1:
                            j = 6;  //보험
                            break;
                        case 2:
                            j = 7;  //보호
                            break;
                        case 3:
                            j = 8;  //산재
                            break;
                        case 4:
                            j = 9;  //자보
                            break;
                        default:
                            j = 6;  //기타는 보험으로
                            break;
                    }

                    //중간청구 대상 금액
                    FnAmt[j, 1] += nAmt;
                    FnAmt[10, 1] += nAmt;

                    //중간청구 접수금액
                    nAmt = VB.Val(dt.Rows[i]["JepJAmt"].ToString().Trim());
                    FnAmt[j, 2] += nAmt;
                    FnAmt[10, 2] += nAmt;
                }
            }
            dt.Dispose();
            dt = null;

        }

        /// <summary>
        ///  '응급실6시간이상,NP낮병동 조합부담액 ADD
        /// </summary>
        /// <param name="strFYYMM"></param>
        /// <param name="strBiGbn"></param>
        /// <param name="strFDate"></param>
        /// <param name="strTDate"></param>
        /// <param name="nBiNo"></param>
        /// <param name="nAmt"></param>
        private void CmdView_Em6TimveOver_ADD(string strFYYMM, ref string strBiGbn, string strFDate, string strTDate, ref int nBiNo, ref double nAmt)
        {
            int i = 0;
            int j = 0;
            int nRead = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (strFYYMM != "200112")
            {
                //응급실6시간이상,NP낮병동 조합부담액 ADD
                //SUBI    CHAR(1)     구분(1.보험 2.보호 3.산재 4.자보 5.일반)
                strBiGbn = "''";
                if (chk0.Checked == true)
                {
                    strBiGbn += ",'1','5'";
                }
                if (chk1.Checked == true)
                {
                    strBiGbn += ",'2'";
                }
                if (chk3.Checked == true)
                {
                    strBiGbn += ",'3'";
                }
                if (chk2.Checked == true)
                {
                    strBiGbn += ",'4'";
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(ACTDATE,'YYYYMM') YYYYMM, SuBi, SUM(Johap) Amt, SUM(JepJAmt) JepJAmt ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ActDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND Gubun='3'";  //응급6시간초과
                if (strBiGbn != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND SUBI IN (" + strBiGbn + " ) ";
                }
                SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(ACTDATE,'YYYYMM'), SuBi ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;
                for (i = 0; i < nRead; i++)
                {
                    nBiNo = (int)VB.Val(dt.Rows[i]["SuBi"].ToString().Trim());
                    switch (nBiNo)
                    {
                        case 1:
                            j = 11;  //보험
                            break;
                        case 2:
                            j = 12;  //보호
                            break;
                        case 3:
                            j = 13;  //산재
                            break;
                        case 4:
                            j = 14;  //자보
                            break;
                        default:
                            j = 11;  //기타는 보험으로
                            break;
                    }
                    if (nBiNo == 1 && string.Compare(dt.Rows[i]["YYYYMM"].ToString().Trim(), "201601") >= 0)
                    {
                        nAmt = VB.Val(dt.Rows[i]["JepJAmt"].ToString().Trim());
                        FnAmt[1, 2] -= nAmt;
                        FnAmt[5, 2] -= nAmt;
                    }
                    else if (nBiNo == 2 && string.Compare(dt.Rows[i]["YYYYMM"].ToString().Trim(), "201602") >= 0)
                    {
                        nAmt = VB.Val(dt.Rows[i]["JepJAmt"].ToString().Trim());
                        FnAmt[2, 2] -= nAmt;
                        FnAmt[5, 2] -= nAmt;
                    }
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    FnAmt[j, 1] += nAmt;
                    FnAmt[15, 1] += nAmt;
                    nAmt = VB.Val(dt.Rows[i]["JepJAmt"].ToString().Trim());
                    FnAmt[j, 2] += nAmt;
                    FnAmt[15, 2] += nAmt;
                }
                dt.Dispose();
                dt = null;
            }

        }

        /// <summary>
        /// 당월분 실청구액
        /// </summary>
        /// <param name="strBiGbn"></param>
        /// <param name="strFYYMM"></param>
        /// <param name="strTYYMM"></param>
        /// <param name="strBi"></param>
        /// <param name="nAmt"></param>
        /// <param name="strYYMM"></param>
        private void CmdView_SilMir_ADD(ref string strBiGbn, string strFYYMM, string strTYYMM, ref string strBi, ref double nAmt, string strYYMM)
        {
            int i = 0;
            int j = 0;
            int nRead = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //미수관리에서 입원 청구액을 ADD
            //** 미수종류(Class) **
            //01.공단 02.직장 03.지역 04.보호 05.산재 07.자보
            strBiGbn = "''";
            if (chk0.Checked == true)
            {
                strBiGbn += ",'01','02','03'";
            }
            if (chk1.Checked == true)
            {
                strBiGbn += ",'04'";
            }
            if (chk3.Checked == true)
            {
                strBiGbn += ",'05'";
            }
            if (chk2.Checked == true)
            {
                strBiGbn += ",'07'";
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT Class, TongGbn, SUM(Amt2) Amt";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_IDMST";
            SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
            SQL = SQL + ComNum.VBLF + "   AND MirYYMM >= '" + strFYYMM + "'";
            SQL = SQL + ComNum.VBLF + "   AND MirYYMM <= '" + strTYYMM + "'";
            if (strBiGbn != "")
            {
                SQL = SQL + ComNum.VBLF + "   AND CLASS IN (" + strBiGbn + " )";
            }
            SQL = SQL + ComNum.VBLF + "   AND TongGbn = '1'";        //퇴원청구
            SQL = SQL + ComNum.VBLF + "   AND IpdOpd='I'";
            SQL = SQL + ComNum.VBLF + " GROUP BY Class,TongGbn";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            nRead = dt.Rows.Count;
            for (i = 0; i < nRead; i++)
            {
                strBi = dt.Rows[i]["Class"].ToString().Trim();
                nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                switch (strBi)
                {
                    case "01":
                    case "02":
                    case "03":
                        j = 1;  //보험
                        break;
                    case "04":
                        j = 2;  //보호
                        break;
                    case "05":
                        j = 3;  //산재
                        break;
                    case "07":
                        j = 4;  //자보
                        break;
                    default:
                        j = 1;  //기타는 보험으로
                        break;
                }

                if (dt.Rows[i]["TongGbn"].ToString().Trim() == "1") //퇴원
                {
                    if (strYYMM != "200112")
                    {
                        FnAmt[j, 2] += nAmt;
                        FnAmt[5, 2] += nAmt;
                    }
                }
                else  //중간청구
                {
                    FnAmt[j + 5, 2] += nAmt;
                    FnAmt[10, 2] += nAmt;
                }
            }
            dt.Dispose();
            dt = null;
        }

        #endregion

        private void btnSubClose_Click(object sender, System.EventArgs e)
        {
            panlHelp.Visible = false;
        }

        private void chk_CheckedChanged(object sender, EventArgs e)
        {
            SS_SET();
        }

        private void btnMenual_Click(object sender, EventArgs e)
        {
            panlHelp.Visible = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            int k = 0;
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if(chk0.Checked == true)
            {
                //퇴원
                ssPrint_Sheet1.Cells[6, 2].Text = ssView_Sheet1.Cells[0, 2].Text;
                ssPrint_Sheet1.Cells[6, 3].Text = ssView_Sheet1.Cells[0, 3].Text;
                ssPrint_Sheet1.Cells[6, 6].Text = ssView_Sheet1.Cells[0, 4].Text;
                ssPrint_Sheet1.Cells[6,10].Text = ssView_Sheet1.Cells[0, 7].Text;
                //중간
                ssPrint_Sheet1.Cells[11, 2].Text = ssView_Sheet1.Cells[5, 2].Text;
                ssPrint_Sheet1.Cells[11, 3].Text = ssView_Sheet1.Cells[5, 3].Text;
                ssPrint_Sheet1.Cells[11, 6].Text = ssView_Sheet1.Cells[5, 4].Text;
                ssPrint_Sheet1.Cells[11, 10].Text = ssView_Sheet1.Cells[5, 7].Text;
                //응급
                ssPrint_Sheet1.Cells[16, 2].Text = ssView_Sheet1.Cells[10, 2].Text;
                ssPrint_Sheet1.Cells[16, 3].Text = ssView_Sheet1.Cells[10, 3].Text;
                ssPrint_Sheet1.Cells[16, 6].Text = ssView_Sheet1.Cells[10, 4].Text;
                ssPrint_Sheet1.Cells[16, 10].Text = ssView_Sheet1.Cells[10, 7].Text;
            }

            if (chk1.Checked == true)
            {
                //퇴원
                ssPrint_Sheet1.Cells[7, 2].Text = ssView_Sheet1.Cells[1, 2].Text;
                ssPrint_Sheet1.Cells[7, 3].Text = ssView_Sheet1.Cells[1, 3].Text;
                ssPrint_Sheet1.Cells[7, 6].Text = ssView_Sheet1.Cells[1, 4].Text;
                ssPrint_Sheet1.Cells[7, 10].Text = ssView_Sheet1.Cells[1, 7].Text;
                //중간
                ssPrint_Sheet1.Cells[12, 2].Text = ssView_Sheet1.Cells[6, 2].Text;
                ssPrint_Sheet1.Cells[12, 3].Text = ssView_Sheet1.Cells[6, 3].Text;
                ssPrint_Sheet1.Cells[12, 6].Text = ssView_Sheet1.Cells[6, 4].Text;
                ssPrint_Sheet1.Cells[12, 10].Text = ssView_Sheet1.Cells[6, 7].Text;
                //응급
                ssPrint_Sheet1.Cells[17, 2].Text = ssView_Sheet1.Cells[11, 2].Text;
                ssPrint_Sheet1.Cells[17, 3].Text = ssView_Sheet1.Cells[11, 3].Text;
                ssPrint_Sheet1.Cells[17, 6].Text = ssView_Sheet1.Cells[11, 4].Text;
                ssPrint_Sheet1.Cells[17, 10].Text = ssView_Sheet1.Cells[11, 7].Text;
            }

            if (chk2.Checked == true)//자보
            {
                //퇴원
                ssPrint_Sheet1.Cells[9, 2].Text = ssView_Sheet1.Cells[3, 2].Text;
                ssPrint_Sheet1.Cells[9, 3].Text = ssView_Sheet1.Cells[3, 3].Text;
                ssPrint_Sheet1.Cells[9, 6].Text = ssView_Sheet1.Cells[3, 4].Text;
                ssPrint_Sheet1.Cells[9, 10].Text = ssView_Sheet1.Cells[3, 7].Text;
                //중간
                ssPrint_Sheet1.Cells[14, 2].Text = ssView_Sheet1.Cells[8, 2].Text;
                ssPrint_Sheet1.Cells[14, 3].Text = ssView_Sheet1.Cells[8, 3].Text;
                ssPrint_Sheet1.Cells[14, 6].Text = ssView_Sheet1.Cells[8, 4].Text;
                ssPrint_Sheet1.Cells[14, 10].Text = ssView_Sheet1.Cells[8, 7].Text;
                //응급
                ssPrint_Sheet1.Cells[19, 2].Text = ssView_Sheet1.Cells[13, 2].Text;
                ssPrint_Sheet1.Cells[19, 3].Text = ssView_Sheet1.Cells[13, 3].Text;
                ssPrint_Sheet1.Cells[19, 6].Text = ssView_Sheet1.Cells[13, 4].Text;
                ssPrint_Sheet1.Cells[19, 10].Text = ssView_Sheet1.Cells[13, 7].Text;
            }

            if (chk3.Checked == true)//산재
            {
                //퇴원
                ssPrint_Sheet1.Cells[8, 2].Text = ssView_Sheet1.Cells[2, 2].Text;
                ssPrint_Sheet1.Cells[8, 3].Text = ssView_Sheet1.Cells[2, 3].Text;
                ssPrint_Sheet1.Cells[8, 6].Text = ssView_Sheet1.Cells[2, 4].Text; 
                ssPrint_Sheet1.Cells[8, 10].Text = ssView_Sheet1.Cells[2, 7].Text;
                //중간
                ssPrint_Sheet1.Cells[13, 2].Text = ssView_Sheet1.Cells[7, 2].Text;
                ssPrint_Sheet1.Cells[13, 3].Text = ssView_Sheet1.Cells[7, 3].Text;
                ssPrint_Sheet1.Cells[13, 6].Text = ssView_Sheet1.Cells[7, 4].Text;
                ssPrint_Sheet1.Cells[13, 10].Text = ssView_Sheet1.Cells[7, 7].Text;
                //응급
                ssPrint_Sheet1.Cells[18, 2].Text = ssView_Sheet1.Cells[12, 2].Text;
                ssPrint_Sheet1.Cells[18, 3].Text = ssView_Sheet1.Cells[12, 3].Text;
                ssPrint_Sheet1.Cells[18, 6].Text = ssView_Sheet1.Cells[12, 4].Text;
                ssPrint_Sheet1.Cells[18, 10].Text = ssView_Sheet1.Cells[12, 7].Text;
            }

            //퇴원
            ssPrint_Sheet1.Cells[10, 2].Text = ssView_Sheet1.Cells[4, 2].Text;
            ssPrint_Sheet1.Cells[10, 3].Text = ssView_Sheet1.Cells[4, 3].Text;
            ssPrint_Sheet1.Cells[10, 6].Text = ssView_Sheet1.Cells[4, 4].Text;
            ssPrint_Sheet1.Cells[10, 10].Text = ssView_Sheet1.Cells[4, 7].Text;
            //중간
            ssPrint_Sheet1.Cells[15, 2].Text = ssView_Sheet1.Cells[9, 2].Text;
            ssPrint_Sheet1.Cells[15, 3].Text = ssView_Sheet1.Cells[9, 3].Text;
            ssPrint_Sheet1.Cells[15, 6].Text = ssView_Sheet1.Cells[9, 4].Text;
            ssPrint_Sheet1.Cells[15, 10].Text = ssView_Sheet1.Cells[9, 7].Text;
            //응급
            ssPrint_Sheet1.Cells[20, 2].Text = ssView_Sheet1.Cells[14, 2].Text;
            ssPrint_Sheet1.Cells[20, 3].Text = ssView_Sheet1.Cells[14, 3].Text;
            ssPrint_Sheet1.Cells[20, 6].Text = ssView_Sheet1.Cells[14, 4].Text;
            ssPrint_Sheet1.Cells[20, 10].Text = ssView_Sheet1.Cells[14, 7].Text;
            ssPrint_Sheet1.Cells[3, 0].Text = "출력일자:" + VB.Now().ToString();


            strTitle = "[" + cboFDate.Text + "~" + cboTDate.Text + "]월별 입원청구액 점검표";

            if (chkGel.Checked == true)
            {
                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 18, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                //strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false,(float)1.1f);
            }
            else
            {
                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 18, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += "\n\n";

                //strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                //strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Left, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, false, false, false, false, false,(float)1.1f); 

            }

            ssView_Sheet1.Columns[0].Visible = false;

            if(chkGel.Checked == true)
            {
                CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter); 
            }
            else
            {
                CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter); //
            }            
        }

    }
}
