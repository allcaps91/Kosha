using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// Class Name      : ComLibB.dll
    /// File Name       : frmOpdCalljubsu.cs
    /// Description     : 외래용 당일 전화 접수
    /// Author          : 김효성
    /// Create Date     : 2017-07-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// VB\Ocs\OpdOcs\ojumstFrm전화접수_외래용.frm => frmOpdCalljubsu.cs 으로 변경함
    /// 
    /// Load에서 삭제 나중에 파라로 받게 수정 해야함.----------------
    /// //'2015-05-18 OPMAIN에서 clsPublic.GstrRetValue 변수에 값을 전달하는 경우
    /// //    If UCase(Trim (App.EXEName)) = "OPMAIN" Then
    /// if (clsPublic.GstrRetValue != "")
    /// {
    ///     txtPano.Text = VB.Pstr(clsPublic.GstrRetValue, "^^", 1);
    ///     cboDept.Text = VB.Pstr(clsPublic.GstrRetValue, "^^", 2);
    /// }
    /// --------------------------------------------------------------
    /// 
    /// 
    /// 
    /// </history>
    /// <seealso> 
    /// VB\Ocs\OpdOcs\ojumstFrm전화접수_외래용(Frm전화접수_외래용.FRM)
    /// </seealso>
    /// <vbp>
    /// default : VB\Ocs\OpdOcs\ojumst\ojumst.Vbp
    /// </vbp>

    public partial class frmOpdCalljubsu : Form
    {
        int[,] nTotQty = new int[6, 3];
        string strOldDoct = "";
        string FstrBi = "";

        //clsPublic.GstrFM_Only = strFM_전용여부;
        //clsPublic.GstrRetValue = strRetValue;
        //clsPublic.GstrHelpCode = strHelpCode;
        //clsPublic.GstrTempHoliday = strTempHoliday;

        string strDTP = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
        string strDTPTime = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M", ":");

        public frmOpdCalljubsu()
        {
            InitializeComponent();
        }

        private void 의사별스케쥴_Dislpaly(FarPoint.Win.Spread.SheetView ssSpread_sheet, int argROW, string ArgDrCode, string ArgDate)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            //ssSpread_sheet.Cells

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT GBJIN, GBJIN2 FROM KOSMOS_PMPA.BAS_SCHEDULE ";
                SQL += ComNum.VBLF + " WHERE DRCODE  = '" + ArgDrCode + "' ";
                SQL += ComNum.VBLF + "   AND SCHDATE = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssSpread_sheet.Cells[argROW, 2].BackColor = Color.FromArgb(255, 255, 255);

                    switch (dt.Rows[0]["GBJIN"].ToString().Trim())
                    {
                        case "1":
                            ssSpread_sheet.Cells[argROW, 2].Text = "진료";
                            ssSpread_sheet.Cells[argROW, 2].BackColor = Color.FromArgb(255, 196, 255);
                            break;
                        case "2":
                            ssSpread_sheet.Cells[argROW, 2].Text = "수술";
                            break;
                        case "3":
                            ssSpread_sheet.Cells[argROW, 2].Text = "특검";
                            break;
                        case "4":
                            ssSpread_sheet.Cells[argROW, 2].Text = "진료없음";
                            break;
                        case "5":
                            ssSpread_sheet.Cells[argROW, 2].Text = "학회";
                            break;
                        case "6":
                            ssSpread_sheet.Cells[argROW, 2].Text = "휴가";
                            break;
                        case "7":
                            ssSpread_sheet.Cells[argROW, 2].Text = "출장";
                            break;
                        case "8":
                            ssSpread_sheet.Cells[argROW, 2].Text = "기타";
                            break;
                        case "9":
                            ssSpread_sheet.Cells[argROW, 2].Text = "OFF";
                            break;
                    }

                    ssSpread_sheet.Cells[argROW, 3].BackColor = Color.FromArgb(255, 255, 255);

                    switch (dt.Rows[0]["GBJIN2"].ToString().Trim())
                    {
                        case "1":
                            ssSpread_sheet.Cells[argROW, 3].Text = "진료";
                            ssSpread_sheet.Cells[argROW, 3].BackColor = Color.FromArgb(255, 196, 255);
                            break;
                        case "2":
                            ssSpread_sheet.Cells[argROW, 3].Text = "수술";
                            break;
                        case "3":
                            ssSpread_sheet.Cells[argROW, 3].Text = "특검";
                            break;
                        case "4":
                            ssSpread_sheet.Cells[argROW, 3].Text = "진료없음";
                            break;
                        case "5":
                            ssSpread_sheet.Cells[argROW, 3].Text = "학회";
                            break;
                        case "6":
                            ssSpread_sheet.Cells[argROW, 3].Text = "휴가";
                            break;
                        case "7":
                            ssSpread_sheet.Cells[argROW, 3].Text = "출장";
                            break;
                        case "8":
                            ssSpread_sheet.Cells[argROW, 3].Text = "기타";
                            break;
                        case "9":
                            ssSpread_sheet.Cells[argROW, 3].Text = "OFF";
                            break;
                    }
                }
                else
                {
                    ssSpread_sheet.Cells[argROW, 2].Text = "스케쥴 없음";
                    ssSpread_sheet.Cells[argROW, 3].Text = "스케쥴 없음";
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

        private void 예약_Display()
        {
            string strTime = "";
            string strTime2 = "";
            string strYTimeGbn = "";
            string strAmTime = "";//    '오전예약시간
            string strAMTime2 = "";//    '오전예약종료
            string strPmTime = "";//    '오후예약시간
            string strPMTime2 = "";//    '오후예약종료
            int nYinWon = 0;      //    '오전예약인원
            int nYinWon2 = 0;      //    '오후예약인원
            int nTotjin = 0;
            //int nTimeBun = 0;
            //int nTimeBun2 = 0;
            int nStartBun = 0;// '오전
            int nStartBun2 = 0;// '오후
            int nEndBun = 0;
            int nEndbun2 = 0;
            int nYTimeBun = 0;
            int i = 0;
            //int j = 0;
            int nTelcnt = 0;
            int nCnt1 = 0;
            int nCnt2 = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            lblPa3.Text = "";
            lblPa5.Text = "";

            if (cboDoct.Text == "")
            {
                return;
            }

            try
            {
                SQL = "";
                SQL += " SELECT GBJIN, GBJIN2 FROM KOSMOS_PMPA.BAS_SCHEDULE ";
                SQL += ComNum.VBLF + " WHERE DRCODE  = '" + cboDoct.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "   AND SCHDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

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
                    ComFunc.MsgBox("스케줄이 없습니다. 확인요망", "확인");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                switch (dt.Rows[0]["GBJIN"].ToString().Trim())
                {
                    case "1":
                        lblPa3.Text = "진료";
                        break;
                    case "2":
                        lblPa3.Text = "수술";
                        break;
                    case "3":
                        lblPa3.Text = "특검";
                        break;
                    case "4":
                        lblPa3.Text = "진료없음";
                        break;
                    case "5":
                        lblPa3.Text = "학회";
                        break;
                    case "6":
                        lblPa3.Text = "휴가";
                        break;
                    case "7":
                        lblPa3.Text = "출장";
                        break;
                    case "8":
                        lblPa3.Text = "기타";
                        break;
                    case "9":
                        lblPa3.Text = "OFF";
                        break;
                    default:
                        lblPa3.Text = "NULL";
                        break;
                }
                lblPa3.Text = "AM:" + lblPa3.Text;

                switch (dt.Rows[0]["GBJIN2"].ToString().Trim())
                {
                    case "1":
                        lblPa5.Text = "진료";
                        break;
                    case "2":
                        lblPa5.Text = "수술";
                        break;
                    case "3":
                        lblPa5.Text = "특검";
                        break;
                    case "4":
                        lblPa5.Text = "진료없음";
                        break;
                    case "5":
                        lblPa5.Text = "학회";
                        break;
                    case "6":
                        lblPa5.Text = "휴가";
                        break;
                    case "7":
                        lblPa5.Text = "출장";
                        break;
                    case "8":
                        lblPa5.Text = "기타";
                        break;
                    case "9":
                        lblPa5.Text = "OFF";
                        break;
                    default:
                        lblPa5.Text = "NULL";
                        break;
                }
                lblPa5.Text = "PM:" + lblPa5.Text;

                //진료과에서 마감확인
                if (dtpDate.Value.ToString("yyyy-MM-dd") == strDTP)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT TO_CHAR(A.SDATE,'HH24:MI') SDATE, TO_CHAR(A.EDATE,'HH24:MI') EDATE, B.NAME, A.MESSAGE1,a.Message ";
                    SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_MESSAGE A, KOSMOS_PMPA.NUR_MESSAGE_CODE B ";
                    SQL += ComNum.VBLF + " WHERE DRCODE  = '" + cboDoct.Text.Trim() + "' ";
                    SQL += ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND A.MESSAGE1 = B.CODE ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        lblPa4.Text = dt.Rows[0]["SDATE"].ToString().Trim() + "부터" + dt.Rows[0]["EDATE"].ToString().Trim() + " 까지 " + dt.Rows[0]["NAME"].ToString().Trim() + "입니다" + dt.Rows[0]["MESSAGE"].ToString().Trim() + ")";
                    }

                    dt.Dispose();
                    dt = null;
                }
                SQL = "";
                SQL += " SELECT YTIMEGBN,TELCNT,TOTJIN,AMTIME,PMTIME,YINWON,AMTIME2,PMTIME2,YINWON2 ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_DOCTOR ";
                SQL += ComNum.VBLF + " WHERE DRCODE  = '" + cboDoct.Text.Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    return;
                }
                strYTimeGbn = dt.Rows[0]["YTIMEGBN"].ToString().Trim();
                nTelcnt = Convert.ToInt32(VB.Val(dt.Rows[0]["TELCNT"].ToString().Trim()));
                nTotjin = Convert.ToInt32(VB.Val(dt.Rows[0]["TOTJIN"].ToString().Trim()));
                nYinWon = Convert.ToInt32(VB.Val(dt.Rows[0]["YINWON"].ToString().Trim()));
                nYinWon2 = Convert.ToInt32(VB.Val(dt.Rows[0]["YINWON2"].ToString().Trim()));
                strAmTime = dt.Rows[0]["AMTIME"].ToString().Trim();
                strAMTime2 = dt.Rows[0]["AMTIME2"].ToString().Trim();
                strPmTime = dt.Rows[0]["PMTIME"].ToString().Trim();
                strPMTime2 = dt.Rows[0]["PMTIME2"].ToString().Trim();

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                switch (strYTimeGbn)
                {
                    case "1":
                        nYTimeBun = 10;
                        break;
                    case "2":
                        nYTimeBun = 15;
                        break;
                    case "3":
                        nYTimeBun = 20;
                        break;
                    case "4":
                        nYTimeBun = 30;
                        break;
                }

                if (VB.Right(cboDoct.Text.Trim(), 2) == "99")
                {
                    nStartBun = 600;
                    nStartBun2 = 810;
                    nEndBun = 720; //12:00
                    nEndbun2 = 1020; //17:00
                }
                else
                {
                    nStartBun = clsVbfunc.TimeToMi(strAmTime);
                    nStartBun2 = clsVbfunc.TimeToMi(strPmTime);
                    nEndBun = clsVbfunc.TimeToMi(strAMTime2);
                    nEndbun2 = clsVbfunc.TimeToMi(strAMTime2);
                }

                //' 시간대별 예약자,전화접수 인원 조회

                SS1_Sheet1.RowCount = 0;

                if (clsVbfunc.TimeToMi(strAmTime) != 0)
                {
                    for (i = nStartBun; i <= nEndBun; i = i + nYTimeBun)
                    {
                        SS1_Sheet1.RowCount = SS1_Sheet1.RowCount + 1;

                        strTime = Convert.ToInt32(i / 60).ToString("00") + ":" + (((i / 60) - Convert.ToInt32(i / 60)) * 60).ToString("00");
                        strTime2 = Convert.ToInt32(i / 60).ToString("00") + ":" + (((i / 60) - Convert.ToInt32(i / 60)) * 60 + (nYTimeBun - 1)).ToString("00");
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0].Text = strTime;

                        if (dtpDate.Value > Convert.ToDateTime(strDTP))
                        {
                            int dd = 0;

                            dd.ToString("00");

                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "SELECT COUNT(PANO) CNT ";
                            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_RESERVED_NEW ";
                            SQL = SQL + ComNum.VBLF + " WHERE DATE3 >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + Convert.ToDateTime(strTime).ToString("HH:mm") + "','YYYY-MM-DDHH24:MI') ";
                            SQL = SQL + ComNum.VBLF + "   AND DATE3 < TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + Convert.ToDateTime(strTime2).ToString("HH:mm") + "','YYYY-MM-DDHH24:MI') ";
                            SQL = SQL + ComNum.VBLF + "   AND DATE3 >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND RETDATE IS NULL ";
                            SQL = SQL + ComNum.VBLF + "   AND DrCode = '" + cboDoct.Text + "' ";
                        }
                        else
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "SELECT COUNT(PANO) CNT ";
                            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_Master ";
                            SQL = SQL + ComNum.VBLF + " WHERE ActDate = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND JTIME >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + Convert.ToDateTime(strTime).ToString("HH:mm") + "','YYYY-MM-DD HH24:MI') ";
                            SQL = SQL + ComNum.VBLF + "   AND JTIME <= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + Convert.ToDateTime(strTime).ToString("HH:mm") + "','YYYY-MM-DD HH24:MI') ";
                            SQL = SQL + ComNum.VBLF + "   AND DeptCode = '" + cboDept.Text + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND DrCode = '" + cboDoct.Text + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND Reserved = '1' ";
                        }

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt == null)
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        nCnt1 = Convert.ToInt32(dt.Rows[0]["CNT"].ToString().Trim());
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 1].Text = Convert.ToString(nCnt1);

                        dt.Dispose();
                        dt = null;

                        //  ' 기존 전화접수된 환자를 조회
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT COUNT(PANO) CNT ";
                        SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_TELRESV ";
                        SQL = SQL + ComNum.VBLF + " WHERE RDate = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "' ,'YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND RTIME = '" + strTime + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND DeptCode = '" + cboDept.Text + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND DrCode = '" + cboDoct.Text + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        nCnt2 = Convert.ToInt32(dt.Rows[0]["CNT"].ToString().Trim());
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 2].Text = Convert.ToString(nCnt2);

                        dt.Dispose();
                        dt = null;

                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 3].Text = Convert.ToString(nYinWon - nCnt1 - nCnt2);
                    }
                }

                SS3_Sheet1.RowCount = 0;

                if (clsVbfunc.TimeToMi(strPmTime) != 0) //'And Right(Trim(ComboDoct.Text), 2) <> "99" Then
                {
                    for (i = nStartBun2; i < nEndbun2; i = i + nYTimeBun)
                    {
                        SS3_Sheet1.RowCount = SS3_Sheet1.RowCount + 1;

                        strTime = VB.Format((i / 60).ToString("00")) + ":" + VB.Format((i / 60) - (i / 60) * 60, "00");
                        if ((((float)i / 60) - VB.Int(i / 60)) * 60 + (nYTimeBun - 1) >= 60)
                        {
                            strTime2 = VB.Format(VB.Int(i / 60) + 1, "00") + ":" + VB.Format((((float)i / 60) - VB.Int(i / 60)) * 60 + (nYTimeBun - 1) - 60, "00");
                        }
                        else
                        {
                            strTime2 = VB.Format(VB.Int(i / 60), "00") + ":" + VB.Format((((float)i / 60) - VB.Int(i / 60)) * 60 + (nYTimeBun - 1), "00");
                        }
                        //strTime2 = (i / 60).ToString("00") + ":" + VB.Format(((i / 60) - (i / 60)) * 60 + (nYTimeBun - 1), "00");

                        SS3_Sheet1.Cells[SS3_Sheet1.RowCount - 1, 0].Text = strTime;

                        if (dtpDate.Value > Convert.ToDateTime(strDTP))
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "SELECT COUNT(PANO) CNT ";
                            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_RESERVED_NEW ";
                            SQL = SQL + ComNum.VBLF + " WHERE DATE3 >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + Convert.ToDateTime(strTime).ToString("HH:mm") + "','YYYY-MM-DD HH24:MI') ";
                            SQL = SQL + ComNum.VBLF + "   AND DATE3 <= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + Convert.ToDateTime(strTime2).ToString("HH:mm") + "','YYYY-MM-DD HH24:MI') ";
                            SQL = SQL + ComNum.VBLF + "   AND DATE3 >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND RETDATE IS NULL ";
                            SQL = SQL + ComNum.VBLF + "   AND DrCode = '" + cboDoct.Text + "' ";
                        }
                        else
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "SELECT COUNT(PANO) CNT ";
                            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_Master ";
                            SQL = SQL + ComNum.VBLF + " WHERE ActDate = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND JTIME >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + Convert.ToDateTime(strTime).ToString("HH:mm") + "','YYYY-MM-DD HH24:MI') ";
                            SQL = SQL + ComNum.VBLF + "   AND JTIME <= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + Convert.ToDateTime(strTime2).ToString("HH:mm") + "','YYYY-MM-DD HH24:MI') ";
                            SQL = SQL + ComNum.VBLF + "   AND DeptCode = '" + cboDept.Text + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND DrCode = '" + cboDoct.Text + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND Reserved = '1' ";
                        }

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        nCnt1 = Convert.ToInt32(dt.Rows[0]["CNT"].ToString().Trim());

                        SS3_Sheet1.Cells[SS3_Sheet1.RowCount - 1, 1].Text = Convert.ToString(nCnt1);
                        //' 기존 전화접수된 환자를 조회

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT COUNT(PANO) CNT ";
                        SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_TELRESV ";
                        SQL = SQL + ComNum.VBLF + " WHERE RDate = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND RTIME = '" + strTime + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND DeptCode = '" + cboDept.Text + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND DrCode = '" + cboDoct.Text + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        nCnt2 = Convert.ToInt32(dt.Rows[0]["CNT"].ToString().Trim());

                        SS3_Sheet1.Cells[SS3_Sheet1.RowCount - 1, 2].Text = Convert.ToString(nCnt2);

                        dt.Dispose();
                        dt = null;
                        SS3_Sheet1.Cells[SS3_Sheet1.RowCount - 1, 3].Text = Convert.ToString(nYinWon2 - nCnt1 - nCnt2);
                    }
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        //Chk_To_Click
        private void chk_To_Click(object sender, EventArgs e)
        {
            cboDept_Sub();
        }

        //ChkIlban_Click
        private void chkllban_Click(object sender, EventArgs e)
        {
            if (chkllban.Checked == true)
            {
                SS2_Sheet1.Cells[3, 1].Text = " 51(일반)";
            }
            else if (chkFmBohum.Checked == true)
            {
                //2018-09-05 김현욱 계장 요청사항으로 추가
                SS2_Sheet1.Cells[3, 1].Text = " 43(보험총액)";
            }
            else
            {
                if (FstrBi != "")
                {
                    SS2_Sheet1.Cells[3, 1].Text = FstrBi;
                    switch (FstrBi)
                    {
                        case "11":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(공무원)";
                            break;
                        case "12":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(연합회)";
                            break;
                        case "13":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(지역)";
                            break;
                        case "21":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(보호1종)";
                            break;
                        case "22":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(보호2종)";
                            break;
                        case "23":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(보호3종)";
                            break;
                        case "24":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(행려환자)";
                            break;
                        case "31":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(산재)";
                            break;
                        case "32":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(공상)";
                            break;
                        case "41":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(공단100)";
                            break;
                        case "42":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(직장100)";
                            break;
                        case "43":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(지역100)";
                            break;
                        case "44":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(가족계획)";
                            break;
                        case "51":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(일반)";
                            break;
                        case "52":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(자보)";
                            break;
                        case "53":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(계약처)";
                            break;
                        case "54":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(미확인)";
                            break;
                        case "55":
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(자보일반)";
                            break;
                        default:
                            SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text + "(기타)";
                            break;
                    }
                }
                else
                {
                    SS2_Sheet1.Cells[3, 1].Text = "";
                }
            }
        }

        //CmdCancel_Click
        private void btnCancel_Click(object sender, EventArgs e)
        {
            clsPublic.GstrHelpCode = "";
            Screen_Clear();
            txtPano.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //CmdHic_Click
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strSname = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strGKiho = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (txtPano.Text == "")
            {
                ComFunc.MsgBox("등록번호 공란!!", "등록번호 입력 후 조회");
                return;
            }

            if (cboDept.Text == "")
            {
                ComFunc.MsgBox("과 공란!!");
                cboDept.Focus();
                return;
            }

            try
            {
                //환자정보 읽기
                SQL = " SELECT PANO,SNAME, JUMIN1,JUMIN2,Jumin3 ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT ";
                SQL += ComNum.VBLF + "  WHERE PANO = '" + txtPano.Text.Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strSname = dt.Rows[0]["SNAME"].ToString().Trim();
                    strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();

                    if (dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                    {
                        strJumin2 = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    }
                    else
                    {
                        strJumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                clsPublic.GstrHelpCode = txtPano.Text.Trim() + "," + cboDept.Text + "," + strSname + ",";
                clsPublic.GstrHelpCode = clsPublic.GstrHelpCode + strJumin1 + strJumin2 + "," + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");


                frmPmpaCheckNhic frmPmpaCheckNhicX = new frmPmpaCheckNhic(txtPano.Text, cboDept.Text, strSname,strJumin1,strJumin2, ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), "");

                frmPmpaCheckNhicX.ShowDialog();

                btnSave.Enabled = true;

                if (clsPublic.GstrHelpCode != "")
                {
                    if (VB.Pstr(clsPublic.GstrHelpCode, ";", 1) != "")
                    {
                        if (VB.Left(VB.Pstr(clsPublic.GstrHelpCode, ";", 2), 1) != "7" && VB.Left(VB.Pstr(clsPublic.GstrHelpCode, ";", 2), 1) != "8")
                        {
                            strGKiho = VB.Pstr(clsPublic.GstrHelpCode, ";", 5);
                            txtGKiho.Text = VB.Left(strGKiho, 1) + "-" + VB.Mid(strGKiho, 2, VB.Len(strGKiho));
                        }
                    }
                }

                clsPublic.GstrHelpCode = "";

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        //CmdOK_Click
        private void btnSave_Click(object sender, EventArgs e)
        {
            string GBinternet = "";
            string GBDrug = "";
            string GbChojin = "";
            string cSname = "";
            string strMsg = "";
            string cBi = "";
            int cAge = 0;
            int cSeqno = 0;
            string cSex = "";
            string strFax = "";
            string strGam = "";//'2014-01-09
            string strJumin1 = "";//'2014-01-09
            string strJumin2 = "";//'2014-01-09
            string strOldGbn = "";// '어르신먼저
            string strGbSPC = "";// '선택진료여부
            string strFM_Cho = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT PANO,SNAME, JUMIN1,JUMIN2,Jumin3 ";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + "  WHERE PANO = '" + txtPano.Text.Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    if (dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                    {
                        strJumin2 = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    }
                    else
                    {
                        strJumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;

                if (SS2_Sheet1.Cells[0, 1].Text == "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("환자 인적 정보가 없습니다.", "확인");
                    return;
                }

                if (cboDoct.Text == "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("의사 코드가 공란입니다.");
                    return;
                }

                if (clsVbfunc.GetYoIl(strDTP) == "토요일")
                {
                    if (VB.UCase(cboDept.Text) == "FM")
                    {
                        ComFunc.MsgBox("스테이션 접수 후 고객이 안 올경우 반드시 원무과에 전화하세요!!", "확인");
                    }
                    else
                    {
                        if (Convert.ToDateTime(strDTPTime) > Convert.ToDateTime("12:00"))
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("외래용 전화 접수는 오후 12시까지 가능합니다." + ComNum.VBLF + ComNum.VBLF + "시간 이후에 변경작업은 원무과에 연락하세요.");
                            return;
                        }
                    }
                }
                else
                {
                    if (Convert.ToDateTime(strDTPTime) > Convert.ToDateTime("17:00"))
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("외래용 전화접수는 오후 5시까지 가능합니다.." + ComNum.VBLF + ComNum.VBLF + "시간 이후에 변경작업은 원무과에 연락하십시오");
                        return;
                    }
                }

                if (VB.UCase(cboDept.Text.Trim()) == "FM" && clsPublic.GstrFM_Only != "Y")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("가정 의학과 접수는 가정 의학과에서만 가능합니다.", "접수실패");
                    return;
                }

                //2020-12-15 김도균과장 당일 전화접수 막음, 의뢰서
                if (cboDoct.Text.Trim() == "3111")
                {
                    ComFunc.MsgBox("김도균과장님은 상담후에 접수가 이루어지기 때문에 당일 대리접수는 불가능합니다.", "확인");
                    return;
                }

                //'DRG예약자체크 -  DRG 2016-06-08

                SQL = " SELECT pano ";
                SQL += ComNum.VBLF + " FROM IPD_reserved";
                SQL += ComNum.VBLF + "  WHERE ReDate =to_date('" + strDTP.Trim() + "','yyyy-mm-dd')";
                SQL += ComNum.VBLF + "   AND Pano ='" + txtPano.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "   AND (GBCHK IS NULL OR GBCHK <> '1' ) ";
                SQL += ComNum.VBLF + "   AND GbDRG ='Y' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count >= 1)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("당일 DRG 예약자 입니다. 접수불가", "작업 실패");
                    dt.Dispose();
                    dt = null;
                    return;
                }
                dt.Dispose();
                dt = null;

                lblTime.Text = strDTPTime;

                if (lblTime.Text == "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("예약 시각이 공란입니다.", "확인");
                    return;
                }

                if (VB.Left(txtPano.Text, 1) == "9")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("접수 할 수 없는 등록번호 (9로 시작하는 것) 입니다.", "");
                    return;
                }

                if (dtpDate.Value < Convert.ToDateTime("2010-10-01"))
                {
                    switch (VB.UCase(cboDept.Text))
                    {
                        case "MG":
                            ComFunc.MsgBox("내과 세부과는 2010-10-01일 부터 시행합니다.", "");
                            break;
                        case "MC":
                            ComFunc.MsgBox("내과 세부과는 2010-10-01일 부터 시행합니다.", "");
                            break;
                        case "MP":
                            ComFunc.MsgBox("내과 세부과는 2010-10-01일 부터 시행합니다.", "");
                            break;
                        case "ME":
                            ComFunc.MsgBox("내과 세부과는 2010-10-01일 부터 시행합니다.", "");
                            break;
                        case "MN":
                            ComFunc.MsgBox("내과 세부과는 2010-10-01일 부터 시행합니다.", "");
                            break;
                        case "MR":
                            ComFunc.MsgBox("내과 세부과는 2010-10-01일 부터 시행합니다.", "");
                            break;
                    }
                }
                else
                {
                    if (VB.UCase(cboDept.Text.Trim()) == "MD")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("내과(MD)는 2010-10-01일 부터 세부 내과로 접수하세요.");
                        return;
                    }
                    else if (VB.UCase(cboDept.Text.Trim()) == "PD")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("소아과 접수는 시행 안 함.");
                        return;
                    }
                }

                //'과제한 -풀어줌 2010-11-23

                if (txtGKiho.Text == "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("자격 조회 시 증번호가 공란입니다. 다시 자격 조회 후 작업하세요.", "확인");
                    return;
                }

                //'선택진료여부
                strGbSPC = "0";

                //'선택의사 체크
                if (Convert.ToDateTime(strDTP) >= Convert.ToDateTime("2011-06-01") && ComFunc.READ_SELECT_DOCTOR_CHK(clsDB.DbCon, "0", cboDoct.Text.Trim()) == "OK")
                {
                    if (ComFunc.Read_Pano_SELECT_MST(clsDB.DbCon, txtPano.Text, "0", cboDoct.Text, strDTP, 0) == "OK")
                    {
                        strGbSPC = "1";
                        if (FstrBi == "21" || FstrBi == "22")
                        {
                            strGbSPC = "0";
                        }
                        else
                        {
                            if (cboDept.Text == "PC" || cboDept.Text == "MG") { }
                            else
                            {
                                ComFunc.MsgBox("선택 의사이며 신청서가 없습니다." + ComNum.VBLF + ComNum.VBLF + "시청서를 원무과에서 반드시 작성 후 진료 보십시오." + ComNum.VBLF + ComNum.VBLF + "스테이션 접수는 정상 적용됩니다.", "확인");
                            }

                        }
                    }
                }

                //'당일 스케쥴 점검
                if (lblPa3.Text == "AM:진료" && lblPa5.Text == "PM:진료") { }
                else if (lblPa3.Text == "AM:진료" && lblPa5.Text != "PM:진료")
                {
                    if (ComFunc.MsgBoxQ("오전만 진료가 있습니다. 진행 하시겠습니까?", "진료확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
                else if (lblPa3.Text != "AM:진료" && lblPa5.Text == "PM:진료")
                {
                    if (ComFunc.MsgBoxQ("오후만 진료가 있습니다. 진행 하시겠습니까?", "진료확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
                else
                {
                    if (ComFunc.MsgBoxQ("진료 스케쥴을 확인!. 이대로 진행 하시겠습니까?", "진료확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
                //2020-01-28 임시공휴일 구분 읽어오기
                ComFunc CF = new ComFunc();
                CF.DATE_HUIL_CHECK(clsDB.DbCon, dtpDate.Value.ToString("yyyy-MM-dd"));

                //'당일 전화접수 중복점검                
                SQL = "   SELECT PANO FROM KOSMOS_PMPA.OPD_TELRESV                                                        ";
                SQL += ComNum.VBLF + " WHERE Pano = '" + txtPano.Text.Trim() + "'                                         ";
                SQL += ComNum.VBLF + "   AND RDate = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND DeptCode = '" + cboDept.Text.Trim() + "'                                     ";
                //SQL += ComNum.VBLF + "   AND Drcode = '" + cboDoct.Text.Trim() + "'                                       ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("당일 진료과에 전화접수가 되어 있습니다. 다시 확인 후 작업을 해주세요.!", "확인");
                    dt.Dispose();
                    dt = null;
                    return;
                }
                dt.Dispose();
                dt = null;

                //'당일외래 마스타 중복점검
                SQL = " SELECT PANO FROM KOSMOS_PMPA.OPD_MASTER  ";
                SQL += ComNum.VBLF + " WHERE BDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND PANO = '" + txtPano.Text + "' ";
                SQL += ComNum.VBLF + "   AND DEPTCODE = '" + cboDept.Text.Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("당일 진료과에 접수가 되어 있습니다. 다시 확인 후 작업을 해주세요.!", "확인");
                    dt.Dispose();
                    dt = null;
                    return;
                }
                dt.Dispose();
                dt = null;
                
                //'당일 전화접수 중복점검                
                SQL = "   SELECT PANO FROM KOSMOS_PMPA.OPD_TELRESV                                                        ";                
                SQL += ComNum.VBLF + " WHERE Pano = '" + txtPano.Text.Trim() + "'                                         ";
                SQL += ComNum.VBLF + "   AND RDate = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND DeptCode = '" + cboDept.Text.Trim() + "'                                     ";
                SQL += ComNum.VBLF + "   AND Drcode = '" + cboDoct.Text.Trim() + "'                                       ";
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("당일 진료과에 전화접수가 되어 있습니다. 다시 확인 후 작업을 해주세요.!", "확인");
                    dt.Dispose();
                    dt = null;
                    return;
                }
                dt.Dispose();
                dt = null;

                //'당일이후일경우 예약테이블 중복점검 2010-01-22
                SQL = " SELECT PANO FROM KOSMOS_PMPA.OPD_Reserved_new  ";
                SQL += ComNum.VBLF + " WHERE TRUNC(Date3) = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND PANO = '" + txtPano.Text + "' ";
                SQL += ComNum.VBLF + "   AND DEPTCODE = '" + cboDept.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "   AND RetDate IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("당일 진료과에 접수가 되어 있습니다. 다시 확인 후 작업을 해주세요.!", "확인");
                    dt.Dispose();
                    dt = null;
                    return;
                }
                dt.Dispose();
                dt = null;

                //'과 초진 추가
                strFM_Cho = "J";
                SQL = " SELECT PANO FROM KOSMOS_PMPA.OPD_MASTER  ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + txtPano.Text + "' ";
                SQL += ComNum.VBLF + "   AND DEPTCODE = '" + cboDept.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "   AND ActDate< TRUNC(SYSDATE) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    strFM_Cho = "C";
                }
                dt.Dispose();
                dt = null;


                //'당일만 처리
                if (dtpDate.Value.ToString("yyyy-MM-dd") == strDTP)
                {
                    strOldGbn = "";

                    if (TELRESV_INSERT(ref cSname, GBinternet, GBDrug, GbChojin, strFax, strGbSPC, strFM_Cho) == false)
                    {
                        return;
                    }

                    if (OPD_Work_Insert(cBi, strMsg, cAge, strOldGbn, cSeqno, cSname) == false)
                    {
                        return;
                    }

                    if (OPD_Master_Insert(cBi, cAge, cSex, strGam, GbChojin, cSname, strGbSPC, strOldGbn, strFM_Cho) == false)
                    {
                        return;
                    }
                }
                                
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                Screen_Clear();
                txtPano.Focus();
                chkGB.Checked = false;
                chkGbDrug.Checked = false;
                ChkGbFax.Checked = false;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private bool TELRESV_INSERT(ref string cSname, string GBinternet, string GBDrug, string GbChojin, string strFax, string strGbSPC, string strFM_Cho)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            cSname = SS2_Sheet1.Cells[0, 1].Text.Trim();

            if (chkGB.Checked == true)
            {
                GBinternet = "Y";
            }
            else
            {
                GBinternet = "N";
            }

            if (chkGbDrug.Checked == true)
            {
                GBDrug = "Y";
            }
            else
            {
                GBDrug = "N";
            }

            if (chkChojin.Checked == true)
            {
                GbChojin = "Y";
            }
            else
            {
                GbChojin = "N";
            }

            if (ChkGbFax.Checked == true)
            {
                strFax = "Y";
            }
            else
            {
                strFax = "N";
            }

            GbChojin = "N";

            //'초진제외 부분 2014-01-09

            SQL = " SELECT SinGu ";
            SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE Pano= '" + txtPano.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "   AND ACTDATE >= SYSDATE -90 ";
            SQL += ComNum.VBLF + "   AND ACTDATE < SYSDATE ";
            SQL += ComNum.VBLF + "   AND DEPTCODE = '" + cboDept.Text.Trim() + "' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return false;
            }

            if (dt.Rows.Count == 0)
            {
                GbChojin = "Y";
            }
            dt.Dispose();
            dt = null;


            // 이전쿼리
            SQL = "";
            SQL += ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.OPD_TELRESV ";
            SQL += ComNum.VBLF + " (RDate, RTime, Pano, SName, DeptCode, Drcode, EntDate, ";
            SQL += ComNum.VBLF + "  EntSabun, Gbinternet, GbDrug, GbChojin,GBFAX,GbFlag,GbSPC,GWACHOJAE,GKIHO) ";
            SQL += ComNum.VBLF + "VALUES (TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),";
            SQL += ComNum.VBLF + "        '" + lblTime.Text + "', '" + txtPano.Text.Trim() + "',";
            SQL += ComNum.VBLF + "        '" + cSname + "', '" + cboDept.Text.Trim() + "',";
            SQL += ComNum.VBLF + "        '" + cboDoct.Text.Trim() + "', SYSDATE , ";
            SQL += ComNum.VBLF + "        '" + clsType.User.IdNumber + "', '" + GBinternet + "', ";
            SQL += ComNum.VBLF + "        '" + GBDrug + "','" + GbChojin + "','" + strFax + "','Y','" + strGbSPC + "','" + strFM_Cho + "','" + txtGKiho.Text.Trim() + "' ) ";
            
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            //'2013-12-27
            if (txtGKiho.Text != "")
            {
                SQL = " UPDATE KOSMOS_PMPA.BAS_PATIENT SET ";
                SQL += ComNum.VBLF + " GKiho = '" + txtGKiho.Text.Trim() + "' ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + txtPano.Text.Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("전화번호 수정 시 에러 발생함", "확인");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
            }

            return true;

        }

        private bool OPD_Work_Insert(string cBi, string strMsg, int cAge, string strOldGbn, int cSeqno, string cSname)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            cBi = VB.Left(SS2_Sheet1.Cells[3, 1].Text.Trim(), 2);
            strMsg = VB.Format(ComFunc.AgeCalcEx(SS2_Sheet1.Cells[1, 1].Text, strDTP), "000");
            cAge = ComFunc.AgeCalc(clsDB.DbCon, SS2_Sheet1.Cells[1, 1].Text.Trim().ToUpper().Replace("(M)", "").Replace("(F)", "").Replace("-", ""));

            //'어르신먼저 75세이상점검
            if (Convert.ToInt32(cAge) >= 75)
            {
                strOldGbn = "Y";
            }

            SQL = "";
            SQL = "SELECT KOSMOS_PMPA.SEQ_OPDWORK.NEXTVAL AS NEXTVAL FROM DUAL";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return false;
            }

            if (dt == null)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("접수증 전송 DATA 오류 : 전산실에 연락요망!", "경고");
                return false;
            }
            if (dt.Rows.Count > 0)
            {
                cSeqno = Convert.ToInt32(dt.Rows[0]["NEXTVAL"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.OPD_WORK";
            SQL += ComNum.VBLF + " (BDATE,SEQNO,PANO,DEPTCODE,DRCODE,SNAME,BI,CHOJAE,SINGU,";
            SQL += ComNum.VBLF + "  DELMARK,WRTTIME,DRSEQNO, PART,JIN,TELTIME,AGE,CHANGE,EMR,OLDMAN,Gubun) ";
            SQL += ComNum.VBLF + " VALUES(TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), " + cSeqno + ",";
            SQL += ComNum.VBLF + "        '" + txtPano.Text.Trim() + "', '" + cboDept.Text.Trim() + "',";
            SQL += ComNum.VBLF + "        '" + cboDoct.Text.Trim() + "', '" + cSname + "', '" + cBi + "',";
            SQL += ComNum.VBLF + "        '3', '0', ' ', '" + strDTPTime + "','1', '333', 'Z',";
            SQL += ComNum.VBLF + "        '" + lblTime.Text.Trim() + "', " + cAge + ", ' ', '0','" + strOldGbn + "','5' ) ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
            return true;

        }

        //'외래접수 OPD_MASTER INSERT
        private bool OPD_Master_Insert(string cBi, int cAge, string cSex, string strGam, string GbChojin, string cSname, string strGbSPC, string strOldGbn, string strFM_Cho)
        {
            string strChojae = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;
            string strJumin = SS2_Sheet1.Cells[1, 1].Text.Trim().ToUpper().Replace("(M)", "").Replace("(F)", "").Replace("-", "");

            cBi = VB.Left(SS2_Sheet1.Cells[3, 1].Text.Trim(), 2);
            cAge = ComFunc.AgeCalc(clsDB.DbCon, strJumin);
            cSex = VB.Mid(SS2_Sheet1.Cells[1, 1].Text.Trim(), 16, 1);

            //Read_Bas_Gamf_emrprt 임시로 사용.
            strGam = Read_Bas_Gamf_emrprt(strJumin);

            strChojae = "1";


            //'초진제외 부분 2014-01-09

            SQL = " SELECT SinGu ";
            SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE Pano= '" + txtPano.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "   AND ACTDATE >= SYSDATE -90 ";
            SQL += ComNum.VBLF + "   AND ACTDATE < SYSDATE ";
            SQL += ComNum.VBLF + "   AND DEPTCODE = '" + cboDept.Text.Trim() + "' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return false;
            }

            if (dt.Rows.Count == 0)
            {
                GbChojin = "Y";
            }
            dt.Dispose();
            dt = null;


            // if (clsPublic.GstrTempHoliday == "*")
            // {
            //     strChojae = GbChojin == "Y" ? "5" : "6";
            // }
            // else
            // {
            //    strChojae = GbChojin == "Y" ? "1" : "3";
            // }

            if (clsPublic.GstrTempHoliday == "*")
            {
                if (GbChojin.Trim() == "Y")
                {
                    strChojae = "5";
                }
                else
                {
                    strChojae = "6";
                }
                //strChojae = GbChojin == "Y" ? "5" : "6";
            }
            else
            {
                if (GbChojin.Trim() == "Y")
                {
                    strChojae = "1";
                }
                else
                {
                    strChojae = "3";
                }

                // strChojae = GbChojin == "Y" ? "1" : "3";
            }


            SQL = "";
            SQL += ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.OPD_MASTER";
            SQL += ComNum.VBLF + " (ACTDATE, PANO, DEPTCODE, BI, SNAME, SEX, AGE, JICODE, ";
            SQL += ComNum.VBLF + " DRCODE, RESERVED, CHOJAE, GBGAMEK, GBSPC, JIN, SINGU, BOHUN, CHANGE, ";
            SQL += ComNum.VBLF + " SHEET, REP, PART, JTIME, STIME, FEE1, FEE2, FEE3, FEE31, FEE5, FEE51, ";
            SQL += ComNum.VBLF + " FEE7, AMT1, AMT2, AMT3, AMT4, AMT5, AMT6, AMT7, GELCODE, OCSJIN, ";
            SQL += ComNum.VBLF + " BDATE, BUNUP, BONRATE, TEAGBE, EMR,GbUse,MksJin,OLDMAN,GWACHOJAE,JIWON) ";
            SQL += ComNum.VBLF + " VALUES (TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD'), '" + txtPano.Text.Trim() + "',";
            SQL += ComNum.VBLF + "         '" + cboDept.Text + "', '" + cBi + "', '" + cSname.Trim() + "', '" + cSex + "', ";
            SQL += ComNum.VBLF + "          " + cAge + ", '"+ lblMAILJIYEK.Text.Trim() + "', '" + cboDoct.Text.Trim() + "', '0', '" + strChojae + "',  ";

            if (strGam == "00")
            {
                SQL += ComNum.VBLF + "  '00', '" + strGbSPC + "', 'E', '0', ";
            }
            else
            {
                SQL += ComNum.VBLF + " '" + strGam + "', '" + strGbSPC + "', 'E', '0', ";
            }

            SQL += ComNum.VBLF + "         '0', '0', '', ' ', " + clsType.User.IdNumber + ",";
            SQL += ComNum.VBLF + "          TO_DATE('" + strDTP + " " + lblTime.Text.Trim() + "','YYYY-MM-DD HH24:MI'), ";
            SQL += ComNum.VBLF + "         '','','','','','','','',";
            SQL += ComNum.VBLF + " '0','0','0','0','0','0','0','','',TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), '','','','0','Y','E','" + strOldGbn + "','" + strFM_Cho + "','Y' ) ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            //'jjy(2004-01-16) 추가

            SQL = "";
            SQL += ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.OPD_DEPTJEPSU";
            SQL += ComNum.VBLF + " (ACTDATE, DEPTCODE, DRCODE, PANO, SNAME, JEPTIME, ";
            SQL += ComNum.VBLF + "  GUBUN, RTIME, CHOJAE, DEPTGBN, DRNUM, DRSEQNO,";
            SQL += ComNum.VBLF + "  JINFLAG, DEPTJTIME, JINTIME) ";
            SQL += ComNum.VBLF + "VALUES (TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), '" + cboDept.Text + "',";
            SQL += ComNum.VBLF + "        '" + cboDoct.Text.Trim() + "', '" + txtPano.Text.Trim() + "',";
            SQL += ComNum.VBLF + "        '" + cSname + "', SYSDATE, '5', '" + lblTime.Text.Trim() + "',";
            SQL += ComNum.VBLF + "        '3', '', '', '', '0', '', '' )";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 원무 감액 함수 임시로 만들어서 사용. 2017.08.17 정영록
        /// </summary>
        /// <param name="strJumin"></param>
        /// <returns></returns>
        private string Read_Bas_Gamf_emrprt(string strJumin)
        {
            //'==================================================================================================
            //    '2013-11-27 감액구분 부담율 변경사항
            //    '감액코드 25 한얼시용자 -> 병원시용자로 같이 사용
            //    '감액코드 26 한얼직원-> 병원직원과 동일한 감액율 적용
            //    '감액코드 27 한얼가족-> 병원직원 직계존비속과 동일한 감액율 적용
            //    '공통사항 : 입원/외래 동일한 기준이며, 입원자는 2013-12-01일부 입원자에 한함.
            //    '치과 감액은 별도의 적용이 없음.

            string strVal = "";
            string strDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            string strSabun = "";

            int nSabun = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                strVal = "00";

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return strVal; //권한 확인

                SQL = "";
                SQL = "SELECT GAMCODE,  GAMMESSAGE, TO_CHAR(GAMEND, 'YYYY-MM-DD') GAMEND ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_GAMF WHERE GAMJUMIN3  = '" + clsAES.AES(strJumin) + "' ";   //'2013-02-20;
                SQL = SQL + ComNum.VBLF + "   AND (GAMEND >= TO_DATE('" + strDate + "','YYYY-MM-DD') OR GAMEND IS NULL)";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["GAMCODE"].ToString().Trim();
                }
                else
                {
                //5234989
                    //        '퇴사자인지 확인
                    SQL = "";
                    SQL = "SELECT IPSADAY,KORNAME, TO_CHAR(TOIDAY, 'YYYY-MM-DD') TOIDAY ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_MST ";
                    SQL = SQL + ComNum.VBLF + " WHERE JUMIN3  = '" + clsAES.AES(strJumin) + "'  ";   //'2013-02-20;
                    SQL = SQL + ComNum.VBLF + "   AND TOIDAY < TRUNC(SYSDATE)  ";
                    SQL = SQL + ComNum.VBLF + "   AND SABUN <'60000' ";
                    SQL = SQL + ComNum.VBLF + " UNION ALL ";
                    SQL = SQL + ComNum.VBLF + "  SELECT JDATE,NAME KORNAME,TO_CHAR(JDATE, 'YYYY-MM-DD') TOIDAY ";
                    SQL = SQL + ComNum.VBLF + "   FROM  KOSMOS_PMPA.BAS_BCODE ";
                    SQL = SQL + ComNum.VBLF + "    WHERE  GUBUN ='원무강제퇴사자감액' ";
                    SQL = SQL + ComNum.VBLF + "    AND  TRIM(CODE) = '" + strJumin + "' ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY 1  DESC  ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return strVal;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        strVal = "42";
                    }
                    else
                    {
                        //천주교 신자 감액(자원봉사, 퇴직자 신자 감액에서 제외 원무과 요청)
                        SQL = "";
                        SQL = "SELECT GAMPANO FROM KOSMOS_PMPA.BAS_GAMFSINGA ";
                        //'주민암호화
                        SQL = SQL + ComNum.VBLF + " WHERE  GAMJUMIN_new  = '" + clsAES.AES(strJumin) + "'  ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return strVal;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            strVal = "51";
                        }

                        dt2.Dispose();
                        dt2 = null;
                    }

                    dt1.Dispose();
                    dt1 = null;
                }

                dt.Dispose();
                dt = null;

                //    '감액퇴사 점검 2013-05-14 ------------------

                SQL = "";
                SQL = " SELECT GAMSABUN ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_GAMF WHERE  GAMJUMIN3  = '" + clsAES.AES(strJumin) + "' ";   //'2013-02-20;
                SQL = SQL + ComNum.VBLF + "   AND (GAMEND >= TO_DATE('" + strDate + "','YYYY-MM-DD') OR GAMEND IS NULL)";
                SQL = SQL + ComNum.VBLF + "   AND GAMSABUN IS NOT NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    nSabun = Convert.ToInt32(VB.Val(dt.Rows[0]["GAMSABUN"].ToString().Trim()));
                    //        'bas_gamf 사번길이 치환

                    if (nSabun >= 600000)
                    {
                        strSabun = nSabun.ToString("000000");
                    }
                    else
                    {
                        strSabun = nSabun.ToString("00000");
                    }

                    SQL = " SELECT SABUN FROM KOSMOS_ADM.INSA_MST ";
                    SQL = SQL + ComNum.VBLF + "WHERE SABUN ='" + strSabun + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND ( TOIDAY>= TO_DATE('" + strDate + "','YYYY-MM-DD') OR TOIDAY IS NULL) ";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return strVal;
                    }

                    if (dt1.Rows.Count == 0)
                    {
                        strVal = "00";
                    }

                    dt1.Dispose();
                    dt1 = null;
                }

                dt.Dispose();
                dt = null;
                //    '감액퇴사 점검 2013-05-14 ----------------
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }


            return strVal;
        }

        //ComboDept_Click
        private void cboDept_Click(object sender, EventArgs e)
        {
            cboDept_Sub();
        }

        private void cboDept_Sub()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            lblDeptName.Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, cboDept.Text);

            SQL = " SELECT DRCODE,DrName FROM KOSMOS_PMPA.BAS_DOCTOR ";
            SQL = SQL + ComNum.VBLF + " WHERE DRDEPT1 = '" + cboDept.Text.Trim() + "' ";
            SQL = SQL + ComNum.VBLF + "   AND TOUR = 'N' ";

            if (chk_To.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "   AND DrCode NOT IN ('1109','1113') ";
            }
            SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }
            cboDoct.Items.Clear();

            for (i = 0; i < dt.Rows.Count; i++)
            {
                cboDoct.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim());
            }
            dt.Dispose();
            dt = null;
        }

        private void Screen_Clear()
        {
            txtPano.Enabled = true;
            cboDept.Enabled = true;

            lblGuide.Text = "";
            txtPano.Text = "";
            cboDept.Text = "";
            cboDoct.Text = "";
            lblDeptName.Text = "";
            lblDoctName.Text = "";
            lblDoctName.BackColor = Color.LightBlue;
            txtGKiho.Text = "";
            lblDoctName.Text = "";

            SS2_Sheet1.Cells[0, 1, SS2_Sheet1.RowCount - 1, 1].Text = "";

        }

        private void SSDoct_Clear()
        {
            ssDoct2_Sheet1.RowCount = 0;
            ssDoct2_Sheet1.RowCount = 6;
        }

        private void frmOpdCalljubsu_Load(object sender, EventArgs e)
        {

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssDoct2_Sheet1.RowCount = 0;

            lblPSel.Text = "";

            lblPa3.Text = "";
            lblPa4.Text = "";
            lblPa5.Text = "";

            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {

                SQL = " SELECT DEPTCODE FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL += ComNum.VBLF + " WHERE DEPTCODE NOT IN ('HD','OC','ER','II','R6','TO','HR','PT','AN','MD','OM','LM','HC','PD') "; //'2010-11-26 PD제외
                SQL += ComNum.VBLF + " ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                cboDept.Items.Clear();

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
                    cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                Screen_Clear();

                //'2014-11-25 FM과 일반자격으로 접수 추가
                if (clsType.User.Grade == "EDPS" || clsPublic.GstrFM_Only == "Y")
                {
                    chkllban.Visible = true;
                    //2018-09-05 김현욱 계장 요청사항으로 추가
                    chkFmBohum.Visible = true;
                }
                
                Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private string READ_DeptName(string Arg)
        {
            string SQL = "";
            string SqlErr = "";
            string strrtn = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (Arg == "")
                {
                    strrtn = "";
                    return strrtn;
                }

                SQL = "";
                SQL += ComNum.VBLF + "SELECT DeptNameK FROM " + ComNum.DB_PMPA + "BAS_ClinicDept ";
                SQL += ComNum.VBLF + " WHERE DeptCode = '" + VB.UCase(Arg) + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return strrtn; ;
                }

                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return strrtn;

                }

                strrtn = dt.Rows[0]["DEPTNAMEK"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return strrtn;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strrtn;
            }
        }

        private void READ_DoctName()
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            lblDoctName.Text = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT DrName FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL += ComNum.VBLF + "WHERE DRCODE = '" + cboDoct.Text + "' ";
                SQL += ComNum.VBLF + "  AND TOUR   = 'N' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt == null)
                {
                    return;
                }
                if (dt.Rows.Count == 1)
                {
                    lblDoctName.Text = dt.Rows[0]["DRNAME"].ToString().Trim();

                }

                dt.Dispose();
                dt = null;

                //' 의사코드가 없는 경우 주과장을 해당의사로 Setting
                SQL = "";
                SQL += ComNum.VBLF + " SELECT DrCode,DrName FROM KOSMOS_PMPA.BAS_DOCTOR ";
                SQL += ComNum.VBLF + "WHERE DrDept1 = '" + cboDept.Text + "' ";
                SQL += ComNum.VBLF + "  AND TOUR = 'N' ";

                if (chk_To.Checked == true)
                {
                    SQL += ComNum.VBLF + "   AND DrCode NOT IN ('1109','1113') ";
                }

                SQL += ComNum.VBLF + "ORDER BY PrintRanking ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

                if (dt.Rows.Count == 2)
                {
                    // '''PanelGuide.Caption = "의사코드가 오류입니다"
                    cboDoct.Text = dt.Rows[0]["DRCODE"].ToString().Trim();
                    lblDoctName.Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                    cboDoct.Focus();

                    dt.Dispose();
                    dt = null;
                    return;
                }

                cboDoct.Focus();

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

        //ComboDept_LostFocus()
        private void cboDeptSerch()
        {
            int i = 0;
            int K = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strSname = "";
            string strJumin1 = "";
            string strJumin2 = "";
            DataTable dt = null;


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (cboDept.Text == "")
                {
                    return;
                }
                cboDept.Text = VB.UCase(cboDept.Text);

                lblDeptName.Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, cboDept.Text.Trim());

                if (lblDeptName.Text == "")
                {
                    cboDept.Text = "";
                    lblGuide.Text = "진료과 등록 안 됨";
                    cboDept.Focus();
                    return;
                }

                //해당과의 진료의사 Combo를 Setting
                SSDoct_Clear();

                SQL = "";
                SQL += ComNum.VBLF + "SELECT DrCode,DrName,DECODE(GbChoice,'Y','Y','') GbChoice  ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL += ComNum.VBLF + " WHERE DrDept1 = '" + cboDept.Text + "' ";
                SQL += ComNum.VBLF + "   AND Tour = 'N' ";
                if (chk_To.Checked == true)
                {
                    SQL += ComNum.VBLF + "   AND DrCode NOT IN ('1109','1113','1402','1405','0104') ";
                }
                SQL += ComNum.VBLF + " ORDER BY PrintRanking,DrCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                ssDoct2_Sheet1.RowCount = dt.Rows.Count;
                ssDoct2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                cboDoct.Items.Clear();
                ssDoct2_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDoct.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim());

                    if (VB.Right(dt.Rows[i]["DRCODE"].ToString().Trim(), 2) != "99")
                    {
                        K = K + 1;
                        ssDoct2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                        ssDoct2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                        if (dt.Rows[i]["GBCHOICE"].ToString().Trim() == "Y")
                        {
                            ssDoct2_Sheet1.Cells[i, 4].Text = "Y";
                            ssDoct2_Sheet1.Cells[i, 4].BackColor = Color.FromArgb(255, 0, 0);
                        }
                        else
                        {
                            ssDoct2_Sheet1.Cells[i, 4].Text = "";
                            ssDoct2_Sheet1.Cells[i, 4].BackColor = Color.FromArgb(255, 255, 255);
                        }

                        의사별스케쥴_Dislpaly(ssDoct2_Sheet1, i, dt.Rows[i]["DRCODE"].ToString().Trim(), strDTP);
                    }
                }
                dt.Dispose();
                dt = null;

                ssDoct2_Sheet1.RowCount = K;

                //'당일이후일경우 예약테이블 중복점검 2010-01-22
                SQL = " SELECT PANO,DeptCode,TO_CHAR(DATE3,'YYYY-MM-DD') Date3, ";
                SQL += ComNum.VBLF + "       TO_CHAR(DATE3, 'HH24:MI') Time3 ";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_Reserved_new  ";
                SQL += ComNum.VBLF + " WHERE TRUNC(Date3) = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND PANO = '" + txtPano.Text + "' ";
                SQL += ComNum.VBLF + "   AND DEPTCODE = '" + cboDept.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "   AND RetDate IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("해당 과 이미 예약 접수됨.");
                    cboDept.Text = "";
                    cboDept.Focus();
                    dt.Dispose();
                    dt = null;
                    return;
                }
                dt.Dispose();
                dt = null;

                //환자 정보 읽기
                SQL = " SELECT PANO,SNAME, JUMIN1,JUMIN2,Jumin3 FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + txtPano.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strSname = dt.Rows[0]["SNAME"].ToString().Trim();
                    strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    if (dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                    {
                        strJumin2 = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    }
                    else
                    {
                        strJumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;

                //당일 동일과 퇴원환자

                SQL = " SELECT PANO, WARDCODE FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + txtPano.Text + "' ";
                SQL += ComNum.VBLF + "  AND OUTDATE =TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND DEPTCODE ='" + cboDept.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "  AND GBSTS NOT IN ('D') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count >= 1)
                {
                    ComFunc.MsgBox("당일 동일 과 토원 환자임. 전화 예약 안 됨.", "확인");
                    dt.Dispose();
                    dt = null;
                    return;
                }
                dt.Dispose();
                dt = null;

                //당일 해당 과 전화접수 여부 Check

                SQL = "";
                SQL += ComNum.VBLF + "SELECT Pano FROM " + ComNum.DB_PMPA + "OPD_TELRESV ";
                SQL += ComNum.VBLF + " WHERE Rdate = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND Pano     = '" + txtPano.Text + "' ";
                SQL += ComNum.VBLF + "   AND DeptCode = '" + cboDept.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count == 1)
                {
                    lblGuide.Text = "해당 과 이미 전화 접수됨";
                    cboDept.Text = "";
                    cboDept.Focus();
                    dt.Dispose();
                    dt = null;
                    return;
                }
                dt.Dispose();
                dt = null;

                //당일 접수여부 Check
                SQL = "";
                SQL += ComNum.VBLF + "SELECT Pano FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL += ComNum.VBLF + " WHERE ActDate = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND Pano     = '" + txtPano.Text + "' ";
                SQL += ComNum.VBLF + "   AND DeptCode = '" + cboDept.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count == 1)
                {
                    lblGuide.Text = "해당 과 이래로 접수 되어 있습니다.";
                    ComFunc.MsgBox("이미 해당 과로 접수가 되어 있습니다.");
                    cboDept.Text = "";
                    cboDept.Focus();
                    dt.Dispose();
                    dt = null;
                    return;
                }
                dt.Dispose();
                dt = null;

                // 최종 진료의사를 Select
                SQL = "";
                SQL += ComNum.VBLF + "SELECT DrCode FROM KOSMOS_PMPA.BAS_LASTEXAM ";
                SQL += ComNum.VBLF + " WHERE Pano = '" + txtPano.Text + "' ";
                SQL += ComNum.VBLF + "   AND DeptCode = '" + cboDept.Text + "' ";

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
                    strOldDoct = "";
                }
                else
                {
                    strOldDoct = dt.Rows[0]["DRCODE"].ToString().Trim();
                }

                cboDoct.Text = strOldDoct;

                dt.Dispose();
                dt = null;

                READ_DoctName();

                예약_Display();

                btnCancel.Enabled = true;
                lblGuide.Text = "";

                SS2_Sheet1.Cells[10, 1].Text = "";

                if (FstrBi != "21" && FstrBi != "22")
                {
                    return;
                }

                //진료소견서 내용 (보호자)

                SQL = "";
                SQL += ComNum.VBLF + "SELECT TO_CHAR(EntDate,'YYYY-MM-DD') EDate, Gubun  ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_SHEET  ";
                SQL += ComNum.VBLF + " WHERE Pano = '" + txtPano.Text + "' ";
                SQL += ComNum.VBLF + "   AND DeptCode = '" + cboDept.Text + "' ";
                SQL += ComNum.VBLF + "   AND Gubun='2' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    SS2_Sheet1.Cells[10, 1].Text = " ▶제출일자: " + dt.Rows[0]["EDate"].ToString().Trim();
                }
                else
                {
                    if (txtPano.Text != "" && (FstrBi == "21" || FstrBi == "22"))
                    {
                        SS2_Sheet1.Cells[10, 1].Text = " ▶소견서 제출요망";
                        ComFunc.MsgBox("해당 보호 환자는 진료 의뢰서가 필요 합니다.");
                    }

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

        //ComboDoct_GotFocus
        private void cboDoct_Enter(object sender, EventArgs e)
        {
            cboDoct.SelectedIndex = 0;
            cboDoct.SelectionLength = VB.Len(cboDoct.Text);
        }

        //SS1_DblClick
        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strFTime = "";
            string strJinTime = "";
            string strJinTime2 = "";
            string StrJin = "";
            string strJinName = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (VB.Val(SS1_Sheet1.Cells[e.Row, 3].Text) == 0)
                {
                    return;
                }
                lblTime.Text = SS1_Sheet1.Cells[e.Row, 0].Text;
                strFTime = lblTime.Text;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT GBJIN FROM KOSMOS_PMPA.BAS_SCHEDULE "; //'오전
                SQL = SQL + ComNum.VBLF + " WHERE SCHDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DrCode = '" + cboDoct.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                StrJin = dt.Rows[0]["Gbjin"].ToString().Trim();

                switch (StrJin)
                {
                    case "1":
                        strJinName = "진료";
                        strJinTime = "07:30";
                        strJinTime2 = "17:30";
                        break;
                    case "2":
                        strJinName = "수술 입니다. 시간을 확인하세요.";
                        strJinTime = "07:30";
                        strJinTime2 = "12:30";
                        break;
                    case "3":
                        strJinName = "특검 입니다. 시간을 확인하세요.";
                        strJinTime = "13:00";
                        strJinTime2 = "17:30";
                        break;
                    case "4":
                        strJinName = "진료 안합니다.?";
                        break;
                    case "5":
                        strJinName = "학회 입니다.";
                        break;
                    case "6":
                        strJinName = "휴가 입니다.";
                        break;
                    case "7":
                        strJinName = "출장 입니다.";
                        break;
                    case "8":
                        strJinName = "기타 입니다. 해당부서에 확인하세요.";
                        break;
                    case "9":
                        strJinName = "OFF  입니다. 해당부서에 확인하세요.";
                        break;
                }
                dt.Dispose();
                dt = null;

                if (StrJin == "1") //'오전진료
                {
                    if (Convert.ToDateTime(strFTime) > Convert.ToDateTime(strJinTime) && Convert.ToDateTime(strFTime) < Convert.ToDateTime(strJinTime2)) { }
                    else
                    {
                        ComFunc.MsgBox(strJinName, "확인");
                        return;
                    }
                }
                else if (StrJin == "2")
                {
                    ComFunc.MsgBox(strJinName, "확인");
                    txtPano.Focus();
                    return;
                }
                else if (StrJin == "3")
                {
                    ComFunc.MsgBox(strJinName, "확인");
                    txtPano.Focus();
                    return;
                }
                else if (StrJin == "3")
                {
                    ComFunc.MsgBox(strJinName, "확인");
                    txtPano.Focus();
                    return;
                }
                else if (StrJin == "4")
                {
                    ComFunc.MsgBox(strJinName, "확인");
                    txtPano.Focus();
                    return;
                }
                else if (StrJin == "5")
                {
                    ComFunc.MsgBox(strJinName, "확인");
                    txtPano.Focus();
                    return;
                }
                else if (StrJin == "6")
                {
                    ComFunc.MsgBox(strJinName, "확인");
                    txtPano.Focus();
                    return;
                }
                else if (StrJin == "7")
                {
                    ComFunc.MsgBox(strJinName, "확인");
                    txtPano.Focus();
                    return;
                }
                else if (StrJin == "8")
                {
                    ComFunc.MsgBox(strJinName, "확인");
                    txtPano.Focus();
                    return;
                }

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
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        //SS3_DblClick
        private void SS3_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strFTime = "";
            string strJinTime = "";
            string strJinTime2 = "";
            string StrJin = "";
            string strJinName = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (VB.Val(SS3_Sheet1.Cells[e.Row, 3].Text) == 0)
            {
                return;
            }

            lblTime.Text = SS3_Sheet1.Cells[e.Row, 0].Text.Trim();
            strFTime = lblTime.Text;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT GBJIN2 FROM KOSMOS_PMPA.BAS_SCHEDULE "; //'오후
                SQL += ComNum.VBLF + " WHERE SCHDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND DrCode = '" + cboDoct.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                StrJin = dt.Rows[0]["Gbjin2"].ToString().Trim();

                switch (StrJin)
                {
                    case "1":
                        strJinName = strJinName + "진료";
                        strJinTime = "13:29";
                        strJinTime2 = "17:30";
                        break;
                    case "2":
                        strJinName = "수술 입니다. 시간을 확인하세요.";
                        break;
                    case "3":
                        strJinName = "특수검사 입니다. 시간을 확인하세요.";
                        break;
                    case "4":
                        strJinName = "진료 안합니다.";
                        break;
                    case "5":
                        strJinName = "학회 입니다";
                        break;
                    case "6":
                        strJinName = "휴가 입니다";
                        break;
                    case "7":
                        strJinName = "출장 입니다";
                        break;
                    case "8":
                        strJinName = "기타 입니다. 해당부서 확인하세요.";
                        break;
                    case "9":
                        strJinName = "OFF 입니다. 해당부서 입니다.";
                        break;
                }
                dt.Dispose();
                dt = null;

                if (StrJin == "1")
                {
                    if (Convert.ToDateTime(strFTime) > Convert.ToDateTime(strJinTime) && Convert.ToDateTime(strFTime) < Convert.ToDateTime(strJinTime2)) { }
                    else
                    {
                        ComFunc.MsgBox(strJinName, "확인");
                        txtPano.Focus();
                        return;
                    }
                }
                else if (StrJin == "2")
                {
                    ComFunc.MsgBox(strJinName, "확인");
                    txtPano.Focus();
                    return;
                }
                else if (StrJin == "3")
                {
                    ComFunc.MsgBox(strJinName, "확인");
                    txtPano.Focus();
                    return;
                }
                else if (StrJin == "4")
                {
                    ComFunc.MsgBox(strJinName, "확인");
                    txtPano.Focus();
                    return;
                }
                else if (StrJin == "5")
                {
                    ComFunc.MsgBox(strJinName, "확인");
                    txtPano.Focus();
                    return;
                }
                else if (StrJin == "6")
                {
                    ComFunc.MsgBox(strJinName, "확인");
                    txtPano.Focus();
                    return;
                }
                else if (StrJin == "7")
                {
                    ComFunc.MsgBox(strJinName, "확인");
                    txtPano.Focus();
                    return;
                }
                else if (StrJin == "8")
                {
                    ComFunc.MsgBox(strJinName, "확인");
                    txtPano.Focus();
                    return;
                }
                else if (StrJin == "9")
                {
                    ComFunc.MsgBox(strJinName, "확인");
                    txtPano.Focus();
                    return;
                }
                btnSave.Focus();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            string strData = "";
            DataTable dt = null;
            string SqlErr = "";

            if (dtpDate.Value < Convert.ToDateTime(strDTP))
            {
                dtpDate.Value = Convert.ToDateTime(strDTP);
                ComFunc.MsgBox("예약 일자가 오늘 보다 적음", "확인");
                return;
            }

            //    '공휴일 Check
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT HOLYDAY FROM KOSMOS_PMPA.BAS_Job ";
                SQL = SQL + ComNum.VBLF + " WHERE JobDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows[0]["HolyDay"].ToString().Trim() == "*" && Convert.ToDateTime(strDTP) != Convert.ToDateTime("2002-06-13"))
                {
                    ComFunc.MsgBox(dtpDate.Value.ToString("yyyy-MM-dd") + "일은 휴일 입니다.", "확인");
                    dtpDate.Value = Convert.ToDateTime(strDTP);
                    dtpDate.Focus();
                    dt.Dispose();
                    dt = null;
                }
                dt.Dispose();
                dt = null;

                //' 당일 전화접수 접수과 Display
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Pano,DeptCode,DrCode, TO_CHAR(RDATE,'YYYY-MM-DD') RDATE, RTime ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_TELRESV ";
                SQL = SQL + ComNum.VBLF + " WHERE Rdate >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND Pano  = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY RTime,DeptCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //List2
                SSList_Sheet1.RowCount = 0;

                if (dt.Rows.Count > 0)
                {

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SSList_Sheet1.RowCount = SSList_Sheet1.RowCount + 1;
                        SSList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                        strData = "<전화접수>";
                        strData = strData + dt.Rows[i]["PANO"].ToString().Trim() + " ";
                        strData = strData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + " ";
                        strData = strData + dt.Rows[i]["RDATE"].ToString().Trim() + " ";
                        strData = strData + dt.Rows[i]["RTIME"].ToString().Trim();

                        SSList_Sheet1.Cells[SSList_Sheet1.RowCount - 1, 0].Text = strData;
                    }
                }

                dt.Dispose();
                dt = null;

                // ' 당일 외래예약 접수과 Display

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Pano,DeptCode,DrCode ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_Master ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND Pano  = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY DeptCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }


                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SSList_Sheet1.RowCount = SSList_Sheet1.RowCount + 1;
                    SSList_Sheet1.SetRowHeight(SSList_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                    strData = "<외래접수>";
                    strData = strData + dt.Rows[i]["PANO"].ToString().Trim() + " ";
                    strData = strData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + " ";
                    strData = strData + "00:00";

                    SSList_Sheet1.Cells[SSList_Sheet1.RowCount - 1, 0].Text = strData;
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

        //TxtDate_KeyPress
        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cboDept.Focus();
            }
        }

        //TxtPano_LostFocus
        private void txtPanoSherch()
        {
            int i = 0;
            string strData = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = "";    // sql에러로그


            Cursor.Current = Cursors.WaitCursor;

            if (txtPano.Text == "")
            {
                return;
            }
            txtPano.Text = Convert.ToInt32(txtPano.Text).ToString("00000000");

            try
            {
                SQL = " SELECT PANO, WARDCODE FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND OUTDATE IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("현재" + dt.Rows[0]["WARDCODE"].ToString().Trim() + "병동에 입원 환자임. 전화 예약 안 됨", "확인");
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                dt.Dispose();
                dt = null;
                //미수구분에  11.기관청구
                //13.필수접종국가지원
                //15.금연처방
                //제외하고 미수만 확인하기
                //'2014-08-26 미수금관련 체크
                //SQL = "";
                //SQL = SQL + ComNum.VBLF + "SELECT  JAmt FROM  KOSMOS_PMPA.Misu_GAINMST ";
                //SQL = SQL + ComNum.VBLF + " WHERE Pano  = '" + txtPano.Text + "'";
                //SQL = SQL + ComNum.VBLF + "   AND JAmt > 0 ";

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  SUM(AMT) JAMT from ( ";
                SQL = SQL + ComNum.VBLF + "        SELECT  pano , SUM(DECODE(GUBUN1,'1',0,AMT*-1)) amt ";
                SQL = SQL + ComNum.VBLF + "        FROM KOSMOS_PMPA.MISU_GAINSLIP ";
                SQL = SQL + ComNum.VBLF + "        WHERE GUBUN1 IN ('2','3','4','5') ";
                SQL = SQL + ComNum.VBLF + "        AND   SUBSTR(MISUDTL,4,2) NOT IN ('11','13','15') ";
                SQL = SQL + ComNum.VBLF + "        GROUP BY PANO ";
                SQL = SQL + ComNum.VBLF + "        HAVING  SUM(DECODE(GUBUN1,'1',0,AMT*-1))  <>0 ";
                SQL = SQL + ComNum.VBLF + "         UNION ALL  ";
                SQL = SQL + ComNum.VBLF + "        SELECT  pano , SUM(DECODE(GUBUN1,'1',AMT,0))  amt ";
                SQL = SQL + ComNum.VBLF + "        FROM KOSMOS_PMPA.MISU_GAINSLIP ";
                SQL = SQL + ComNum.VBLF + "        WHERE GUBUN1 IN ('1') ";
                SQL = SQL + ComNum.VBLF + "        AND   SUBSTR(MISUDTL,4,2) NOT IN ('11','13','15') ";
                SQL = SQL + ComNum.VBLF + "        GROUP BY PANO ";
                SQL = SQL + ComNum.VBLF + "        HAVING SUM(DECODE(GUBUN1,'1',AMT,0)) <>0  )";

                SQL = SQL + ComNum.VBLF + " WHERE Pano  = '" + txtPano.Text + "'";
                SQL = SQL + ComNum.VBLF + " GROUP BY PANO  HAVING NVL(SUM(AMT),0) <> 0";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["JAMT"].ToString().Trim()) > 0)
                    {
                        ComFunc.MsgBox("현재 외래 미수금 내역이 남아 있습니다." + ComNum.VBLF + ComNum.VBLF + "외래 접수 창구에서 접수 할 수 있습니다.", "확인");
                        dt.Dispose();
                        dt = null;
                        return;
                    }
                }

                dt.Dispose();
                dt = null;

                //'인적사항을 Display
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Sname,Jumin1,Jumin2,Jumin3,Tel,Bi,Gwange,TEL,HPHONE,";
                SQL = SQL + ComNum.VBLF + "       Pname,Kiho,GKiho,ZipCode1,ZipCode2, Juso, sex, AIFLU ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_Patient ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + txtPano.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    lblGuide.Text = "등록 번호가 오류입니다";
                    txtPano.Text = "";
                    txtPano.Focus();
                    dt.Dispose();
                    dt = null;
                    return;
                }

                SS2_Sheet1.Cells[0, 1].Text = " " + dt.Rows[0]["SNAME"].ToString().Trim();

                if (dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                {
                    SS2_Sheet1.Cells[1, 1].Text = " " + dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim()) + "(" + dt.Rows[0]["SEX"].ToString().Trim() + ")";
                }
                else
                {
                    SS2_Sheet1.Cells[1, 1].Text = " " + dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + dt.Rows[0]["JUMIN2"].ToString().Trim() + "(" + dt.Rows[0]["SEX"].ToString().Trim() + ")";
                }

                SS2_Sheet1.Cells[2, 1].Text = " " + dt.Rows[0]["TEL"].ToString().Trim();
                SS2_Sheet1.Cells[3, 1].Text = " " + dt.Rows[0]["BI"].ToString().Trim();
                txtTel.Text = dt.Rows[0]["TEL"].ToString().Trim();
                txtHpone.Text = dt.Rows[0]["HPHONE"].ToString().Trim();

                FstrBi = dt.Rows[0]["BI"].ToString().Trim();

                switch (FstrBi)
                {
                    case "11":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(공무원)";
                        break;
                    case "12":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(연합회)";
                        break;
                    case "13":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(지역)";
                        break;
                    case "21":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(보호1종)";
                        break;
                    case "22":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(보호2종";
                        break;
                    case "23":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(보호3종)";
                        break;
                    case "24":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(행려환자)";
                        break;
                    case "31":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(산재)";
                        break;
                    case "32":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(공상)";
                        break;
                    case "41":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(공단100%)";
                        break;
                    case "42":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(직장100%)";
                        break;
                    case "43":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(지역100%)";
                        break;
                    case "44":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(가족계획)";
                        break;
                    case "51":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(일반)";
                        break;
                    case "52":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(자보)";
                        break;

                    case "53":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(계약처)";
                        break;
                    case "54":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(미확인)";
                        break;
                    case "55":
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(자보일반)";
                        break;
                    default:
                        SS2_Sheet1.Cells[3, 1].Text = SS2_Sheet1.Cells[3, 1].Text.Trim() + "(기타)";
                        break;
                }

                SS2_Sheet1.Cells[4, 1].Text = " " + dt.Rows[0]["GWANGE"].ToString().Trim();
                SS2_Sheet1.Cells[5, 1].Text = " " + dt.Rows[0]["KIHO"].ToString().Trim();
                SS2_Sheet1.Cells[6, 1].Text = " " + dt.Rows[0]["GKIHO"].ToString().Trim();
                SS2_Sheet1.Cells[7, 1].Text = " " + dt.Rows[0]["PNAME"].ToString().Trim();

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT MAILJUSO, MAILJIYEK FROM KOSMOS_PMPA.BAS_Mail ";
                SQL = SQL + ComNum.VBLF + " WHERE MAILCODE = '" + dt.Rows[0]["ZipCode1"].ToString().Trim() + dt.Rows[0]["ZipCode2"].ToString().Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                lblMAILJIYEK.Text = "";

                if (dt1.Rows.Count == 1)
                {
                    SS2_Sheet1.Cells[8, 1].Text = " " + dt1.Rows[0]["MAILJUSO"].ToString().Trim();
                }

                if (dt1.Rows.Count > 0)
                {
                    lblMAILJIYEK.Text = dt1.Rows[0]["MAILJIYEK"].ToString().Trim();
                }

                SS2_Sheet1.Cells[9, 1].Text = " " + dt.Rows[0]["JUSO"].ToString().Trim();
                SS2_Sheet1.Cells[10, 1].Text = "";

                dt.Dispose();
                dt = null;
                dt1.Dispose();
                dt1 = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT PANO, DEPTCODE, DRCODE ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE Actdate = TO_DATE('" + strDTP + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND Pano  = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND JIN NOT IN ('E','H') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                SSList_Sheet1.RowCount = 0;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SSList_Sheet1.RowCount = SSList_Sheet1.RowCount + 1;

                        strData = "<당일접수>";
                        strData = strData + dt.Rows[i]["PANO"].ToString().Trim() + " ";
                        strData = strData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + " ";

                        SSList_Sheet1.Cells[SSList_Sheet1.RowCount - 1, 0].Text = strData;
                    }
                }
                dt.Dispose();
                dt = null;

                //' 당일 전화접수 접수과 Display
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Pano,DeptCode,DrCode, TO_CHAR(RDATE,'YYYY-MM-DD') RDATE, RTime ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_TELRESV ";
                SQL = SQL + ComNum.VBLF + " WHERE Rdate >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND Pano  = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY RTime,DeptCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SSList_Sheet1.RowCount = SSList_Sheet1.RowCount + 1;

                        strData = "<전화접수> ";
                        strData = strData + dt.Rows[i]["Pano"].ToString().Trim() + " ";
                        strData = strData + dt.Rows[i]["DeptCode"].ToString().Trim() + " ";
                        strData = strData + dt.Rows[i]["RDATE"].ToString().Trim() + " ";
                        strData = strData + dt.Rows[i]["RTime"].ToString().Trim();

                        SSList_Sheet1.Cells[SSList_Sheet1.RowCount - 1, 0].Text = strData;
                    }
                }
                dt.Dispose();
                dt = null;

                // '예약환자

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT PANO, DEPTCODE, TO_CHAR(DATE3,'YYYY-MM-DD') Date3, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(DATE3, 'HH24:MI') Time3 ";
                SQL = SQL + ComNum.VBLF + "  From KOSMOS_PMPA.OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + " WHERE DATE3 >= TO_DATE('" + strDTP + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND RETDATE IS NULL";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SSList_Sheet1.RowCount = SSList_Sheet1.RowCount + 1;

                        strData = "<예약접수> ";
                        strData = strData + dt.Rows[i]["Pano"].ToString().Trim();
                        strData = strData + dt.Rows[i]["DeptCode"].ToString().Trim();
                        strData = strData + dt.Rows[i]["Date3"].ToString().Trim();
                        strData = strData + dt.Rows[i]["Time3"].ToString().Trim();

                        SSList_Sheet1.Cells[SSList_Sheet1.RowCount - 1, 0].Text = strData;
                    }
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

        private void cboDoct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDoct.Text == "")
            {
                return;
            }

            lblPSel.Text = "";
            lblDoctName.BackColor = Color.LightBlue;

            if (ComFunc.READ_SELECT_DOCTOR_CHK(clsDB.DbCon, "O", cboDoct.Text.Trim(), "") == "OK")
            {
                lblDoctName.BackColor = Color.Pink;
                lblPSel.Text = ComFunc.READ_PANO_SELECT_MST_BDATE(clsDB.DbCon, txtPano.Text, "0", cboDoct.Text, dtpDate.Value.ToString("yyyy-MM-dd"));
            }

            READ_DoctName();

            예약_Display();
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboDeptSerch();
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            txtPanoSherch();
        }
    }
}
