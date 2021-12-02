using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using System.Drawing;
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    public partial class frmPmpaViewTongMisuDtl : Form
    {
        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : frmPmpaViewTongMisuDtl.cs
        /// Description     : 청구금액 세부내역(미수기준)
        /// Author          : 김효성
        /// Create Date     : 2017-08-30
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "psmh\misu\misubs\misubs.vbp(misubs62.frm) >> frmPmpaViewTongMisuDtl.cs 폼이름 재정의" />	
        /// 

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsVbfunc cf = new clsVbfunc();
        clsSpread CS = new clsSpread();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        double[,] FnAmt = new double[12, 9];
        string GstrYYMM = "";
        //string GstrMenu = "";
        //string GstrSMenu = "";
        string GstrMsgList = "";

        public frmPmpaViewTongMisuDtl(string strYYMM, string strMenu, string strSMenu, string strMsgList)
        {
            string GstrSMenu = strSMenu;
            string GstrMenu = strMenu;
            string GstrYYMM = strYYMM;
            string GstrMsgList = strMsgList;

            InitializeComponent();
        }
        public frmPmpaViewTongMisuDtl()
        {
            InitializeComponent();
        }

        private void frmPmpaViewTongMisuDtl_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등
            clsVbfunc.SetCboDate(clsDB.DbCon, cboFdate, 36, "", "1");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTdate, 36, "", "1");

            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboFdate.DropDownStyle = ComboBoxStyle.DropDown;
                    cboTdate.DropDownStyle = ComboBoxStyle.DropDown;
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
            //int K = 0;
            //int nRead = 0;
            //int nBiNo = 0;
            int nClass = 0;
            int nJong = 0;
            double nAmt = 0;
            string strYYMM = "";
            string strFYYMM = "";
            string strTYYMM = "";
            string strFDate = "";
            string strTdate = "";
            //string strBi = "";
            string strIpdOpd = "";
            string strClass = "";
            string strTongGbn = "";
            string strBiGbn = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //누적할 배열을 Clear
            for (i = 1; i <= 11; i++)
            {
                for (j = 1; j <= 8; j++)
                {
                    FnAmt[i, j] = 0;
                }
            }

            strYYMM = VB.Left(cboFdate.Text, 4) + VB.Mid(cboFdate.Text, 7, 2);
            strFYYMM = VB.Left(cboFdate.Text, 4) + VB.Mid(cboFdate.Text, 7, 2);
            strTYYMM = VB.Left(cboTdate.Text, 4) + VB.Mid(cboTdate.Text, 7, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTdate = clsVbfunc.LastDay((int)VB.Val(VB.Left(cboTdate.Text, 4)), (int)VB.Val(VB.Mid(cboTdate.Text, 7, 2)));

            //'jjy(2003-01-13) '통계 remark 등록 공용변수
            GstrYYMM = strYYMM;
            //GstrMenu = "4";
            //GstrSMenu = "12";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //'자료중 오류가 있는지 Check
                SQL = "";
                SQL = SQL + ComNum.VBLF + "	SELECT COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + "	 FROM " + ComNum.DB_PMPA + "MISU_IDMST ";
                SQL = SQL + ComNum.VBLF + "	WHERE WRTNO IN (SELECT WRTNO FROM MISU_SLIP ";
                SQL = SQL + ComNum.VBLF + "	    WHERE BDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "	    AND BDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "	    AND Gubun >= '11' AND Gubun <= '20' ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY WRTNO) ";
                SQL = SQL + ComNum.VBLF + "	    AND Class <= '07' ";
                SQL = SQL + ComNum.VBLF + "	    AND (TongGbn IS NULL OR MirYYMM IS NULL) ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    if (int.Parse(dt.Rows[0]["CNT"].ToString().Trim()) > 0)
                    {
                        GstrMsgList = "당월분 자료중 청구종류,통계월이 NULL인 자료가";
                        GstrMsgList = GstrMsgList + dt.Rows[0]["CNT"].ToString().Trim() + "건이 있습니다.";
                        GstrMsgList = GstrMsgList + "오류를 수정후 다시 작업을 하세요.";

                        dt.Dispose();
                        dt = null;

                        ComFunc.MsgBox(GstrMsgList, "오류");
                        return;
                    }
                }

                dt.Dispose();
                dt = null;


                //'당월 미수발생액 ADD
                //'** 미수종류(Class) **
                //'01.공단 02.직장 03.지역 04.보호 05.산재 07.자보
                strBiGbn = "''";
                if (chk0.Checked == true)
                {
                    strBiGbn = strBiGbn + ",'01','02','03','04'";
                }
                if (chk1.Checked == true)
                {
                    strBiGbn = strBiGbn + ",'05','07'";
                }

                //'자료조회
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT b.IpdOpd,b.Class,b.TongGbn,SUM(a.Amt) Amt ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_SLIP a," + ComNum.DB_PMPA + "MISU_IDMST b ";
                SQL = SQL + ComNum.VBLF + "WHERE a.BDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.BDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.Gubun>='11' AND a.Gubun<='20' ";
                SQL = SQL + ComNum.VBLF + "  AND a.Amt <> 0 ";
                SQL = SQL + ComNum.VBLF + "  AND a.WRTNO=b.WRTNO(+) ";

                if (strBiGbn != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND b.CLASS IN (" + strBiGbn + " ) ";
                }
                SQL = SQL + ComNum.VBLF + "GROUP BY b.IpdOpd,b.Class,b.TongGbn ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strIpdOpd = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    strClass = dt.Rows[i]["Class"].ToString().Trim();
                    strTongGbn = dt.Rows[i]["TongGbn"].ToString().Trim();
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());

                    //'보험종류
                    switch (strClass)
                    {
                        case "01":
                        case "02":
                        case "03":
                            nClass = 1;//'보험
                            break;
                        case "04":
                            nClass = 2;//'보호
                            break;
                        case "05":
                            nClass = 3;//'산재
                            break;
                        default:
                            nClass = 4;//'자보
                            break;
                    }
                    //'청구종류
                    switch (strTongGbn)
                    {
                        case "1":
                            nJong = 3;//  '퇴원청구
                            break;
                        case "2":
                            nJong = 2;//  '중간청구
                            break;
                        case "3":
                            nJong = 5;//  '재청구
                            break;
                        case "4":
                            nJong = 6;//  '추가청구
                            break;
                        case "5":
                            nJong = 7;//  '이의신청
                            break;
                        case "6":
                            nJong = 8;//  '기타청구
                            break;
                        default:
                            nJong = 8;//  '기타청구
                            break;
                    }
                    if (strIpdOpd == "O")
                    {
                        FnAmt[nClass, nJong] = FnAmt[nClass, nJong] + nAmt;
                        FnAmt[nClass, 1] = FnAmt[nClass, 1] + nAmt;   //'청구합계
                        FnAmt[5, nJong] = FnAmt[5, nJong] + nAmt;     //'소계
                        FnAmt[5, 1] = FnAmt[5, 1] + nAmt;             //'소계 청구합계
                    }
                    else
                    {
                        nClass = nClass + 5;
                        FnAmt[nClass, nJong] = FnAmt[nClass, nJong] + nAmt;
                        FnAmt[nClass, 1] = FnAmt[nClass, 1] + nAmt;//'청구합계
                        FnAmt[10, nJong] = FnAmt[10, nJong] + nAmt;//'소계
                        FnAmt[10, 1] = FnAmt[10, 1] + nAmt;//'소계 청구합계
                    }
                    FnAmt[11, nJong] = FnAmt[11, nJong] + nAmt;    //'합계
                    FnAmt[11, 1] = FnAmt[11, 1] + nAmt;    //'합계 청구합계

                }
                dt.Dispose();
                dt = null;

                //' SUBI    CHAR(1)     구분(1.보험 2.보호 3.산재 4.자보 5.일반)
                strBiGbn = "''";
                if (chk0.Checked == true)
                {
                    strBiGbn = strBiGbn + ",'1','2','5'";
                }
                if (chk1.Checked == true)
                {
                    strBiGbn = strBiGbn + ",'3','4'";
                }

                //'자료조회
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(JEPDATE,'YYYYMM') YYMM,  Bi,SUM(JepJAmt) Amt ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
                SQL = SQL + ComNum.VBLF + "WHERE ActDate>=TO_DATE('2001-12-01','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND Gubun='3' ";// '응급6시간,NP낮병동
                SQL = SQL + ComNum.VBLF + "  AND JepDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND JepDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";

                if (strBiGbn != "")
                {
                    SQL = SQL + " AND SUBI IN (" + strBiGbn + " ) ";
                }
                SQL = SQL + ComNum.VBLF + "GROUP BY Bi, TO_CHAR(JEPDATE,'YYYYMM') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Bi ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strClass = dt.Rows[i]["BI"].ToString().Trim();
                    nAmt = Convert.ToDouble(dt.Rows[i]["AMT"].ToString().Trim());

                    //'보험종류
                    switch (strClass)
                    {
                        case "11":
                        case "12":
                        case "13":
                            nClass = 6; //'보험
                            break;

                        case "21":
                        case "22":
                        case "23":
                        case "24":
                        case "25":
                        case "26":
                        case "27":
                        case "28":
                        case "29":
                            nClass = 7; //'보호
                            break;
                        case "31":
                            nClass = 8; //'산재
                            break;
                        case "44":
                            nClass = 6; //'보험
                            break;
                        case "52":
                            nClass = 9; //'자보
                            break;
                        default:
                            nClass = 6; //'보험
                            break;
                    }

                    if (nClass == 6 && string.Compare(dt.Rows[i]["YYMM"].ToString().Trim(), "201602") >= 0)//'보험
                    {
                        FnAmt[nClass, 4] = FnAmt[nClass, 4] + nAmt;//'외래수익청구
                        FnAmt[nClass, 3] = FnAmt[nClass, 3] - nAmt;//'퇴원청구
                        FnAmt[10, 4] = FnAmt[10, 4] + nAmt;// '소계 외래수익청구
                        FnAmt[10, 3] = FnAmt[10, 3] - nAmt;//'소계 퇴원청구
                        FnAmt[11, 4] = FnAmt[11, 4] + nAmt;//'합계 외래수익청구
                        FnAmt[11, 3] = FnAmt[11, 3] - nAmt;//'합계 퇴원청구
                    }
                    //else if ( Convert . ToDateTime ( dt . Rows [ i ] [ "YYMM" ] . ToString ( ) . Trim ( ) ) >= Convert . ToDateTime ( "201603" ) ) //'보험
                    else if (string.Compare(dt.Rows[i]["YYMM"].ToString().Trim(), "201603") >= 0) //'보험
                    {
                        FnAmt[nClass, 4] = FnAmt[nClass, 4] + nAmt;//'외래수익청구
                        FnAmt[nClass, 3] = FnAmt[nClass, 3] - nAmt;//'퇴원청구
                        FnAmt[10, 4] = FnAmt[10, 4] + nAmt;//'소계 외래수익청구
                        FnAmt[10, 3] = FnAmt[10, 3] - nAmt;//'소계 퇴원청구
                        FnAmt[11, 4] = FnAmt[11, 4] + nAmt;//'합계 외래수익청구
                        FnAmt[11, 3] = FnAmt[11, 3] - nAmt;//'합계 퇴원청구
                    }
                    else
                    {
                        FnAmt[nClass, 4] = FnAmt[nClass, 4] + nAmt;//'외래수익청구
                        FnAmt[nClass, 2] = FnAmt[nClass, 2] - nAmt;//'퇴원청구
                        FnAmt[10, 4] = FnAmt[10, 4] + nAmt;//'소계 외래수익청구
                        FnAmt[10, 2] = FnAmt[10, 2] - nAmt;//'소계 퇴원청구
                        FnAmt[11, 4] = FnAmt[11, 4] + nAmt;//'합계 외래수익청구
                        FnAmt[11, 2] = FnAmt[11, 2] - nAmt;//'합계 퇴원청구
                    }
                }
                dt.Dispose();
                dt = null;

                for (i = 1; i <= 11; i++)
                {
                    for (j = 1; j <= 8; j++)
                    {
                        ssView_Sheet1.Cells[i - 1, (j + 2) - 1].Text = FnAmt[i, j].ToString("###,###,###,##0 ");
                    }

                }
                
                //ssView_Sheet1.Rows[10].BackColor = Color.FromArgb(166, 166, 166);

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void chk0_CheckedChanged(object sender, EventArgs e)
        {
            if (chk0.Checked == true)//'건강보험 + 의료급여
            {
                ssView_Sheet1.Rows[0].Visible = true;
                ssView_Sheet1.Rows[1].Visible = true;
                ssView_Sheet1.Rows[5].Visible = true;
                ssView_Sheet1.Rows[6].Visible = true;
            }
            else
            {
                ssView_Sheet1.Rows[0].Visible = false;
                ssView_Sheet1.Rows[1].Visible = false;
                ssView_Sheet1.Rows[5].Visible = false;
                ssView_Sheet1.Rows[6].Visible = false;
            }

            if (chk1.Checked == true)//'산재 + 자보
            {
                ssView_Sheet1.Rows[2].Visible = true;
                ssView_Sheet1.Rows[3].Visible = true;
                ssView_Sheet1.Rows[7].Visible = true;
                ssView_Sheet1.Rows[8].Visible = true;
            }
            else
            {
                ssView_Sheet1.Rows[2].Visible = false;
                ssView_Sheet1.Rows[3].Visible = false;
                ssView_Sheet1.Rows[7].Visible = false;
                ssView_Sheet1.Rows[8].Visible = false;
            }
            //Clear
            ssView_Sheet1.Cells[0, 0, 9, 0].Text = "";
            if (chk0.Checked == true && chk1.Checked == true)
            {
                ssView_Sheet1.Cells[1, 0].Text = "외";
                ssView_Sheet1.Cells[3, 0].Text = "래";
                ssView_Sheet1.Cells[6, 0].Text = "입";
                ssView_Sheet1.Cells[8, 0].Text = "원";
            }
            else if (chk1.Checked == true)
            {
                ssView_Sheet1.Cells[2, 0].Text = "외";
                ssView_Sheet1.Cells[3, 0].Text = "래";
                ssView_Sheet1.Cells[7, 0].Text = "입";
                ssView_Sheet1.Cells[8, 0].Text = "원";
            }
            else if (chk0.Checked == true)
            {
                ssView_Sheet1.Cells[0, 0].Text = "외";
                ssView_Sheet1.Cells[1, 0].Text = "래";
                ssView_Sheet1.Cells[5, 0].Text = "입";
                ssView_Sheet1.Cells[6, 0].Text = "원";
            }

            if (chk0.Checked == false && chk1.Checked == false)
            {
                ssView_Sheet1.Rows[4].Visible = false;
                ssView_Sheet1.Rows[9].Visible = false;
                ssView_Sheet1.Rows[10].Visible = false;
            }
            else
            {
                ssView_Sheet1.Rows[4].Visible = true;
                ssView_Sheet1.Rows[9].Visible = true;
                ssView_Sheet1.Rows[10].Visible = true;
            }

            ssView_Sheet1.Cells[0, 2, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            strTitle = "월별 청구금액 세부내역";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String(VB.Space(15) + "미수발생월 :" + cboFdate.Text + " ~ " + cboTdate.Text + VB.Space(50) + "출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, false, true, true, true, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);

            Cursor.Current = Cursors.Default;
        }
    }
}
