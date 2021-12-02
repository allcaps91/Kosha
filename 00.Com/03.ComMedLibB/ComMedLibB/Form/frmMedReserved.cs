using ComBase; //기본 클래스
using System;
using System.Data;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// Class Name      : ComMedLibB
    /// File Name       : frmMedReserved.cs
    /// Description     : 진료 예약
    /// Author          : 박창욱
    /// Create Date     : 2017-11-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// TODO : 서브폼으로 호출 된 뒤 테스트 필요.
    /// </history>
    /// <seealso cref= "\Ocs\OpdOcs\Oorder\Oorder78.frm(FrmReserved.frm) >> frmMedReserved.cs 폼이름 재정의" />	
    public partial class frmMedReserved : Form
    {
        string[] strRTime = new string[16];
        string strProcessDate = "";

        DataTable dt = null;
        string SQL = "";    //Query문
        string SqlErr = ""; //에러문 받는 변수

        string GstrReserved = "";
        string GstrDrCode = "";

        public frmMedReserved()
        {
            InitializeComponent();
        }

        public frmMedReserved(string strReserved, string strDrCode)
        {
            InitializeComponent();
            GstrReserved = strReserved;
            GstrDrCode = strDrCode;
        }

        void Reserved_Data_Show()
        {
            string strDate = "";

            //기수납한 예약자료 수정 못하게
            if (GstrReserved == "OK")
            {
                btnRegist.Enabled = false;
                btnCancel.Enabled = false;
                lblStat.Text = "수납 완료";
            }

            strDate = ComFunc.FormatStrToDate(clsOrdFunction.Pat.RDATE, "D");

            if (strDate != "")
            {
                dtpRDate.Value = Convert.ToDateTime(strDate);
            }

            switch (clsOrdFunction.Pat.RTime.Trim())
            {
                case "09:00":
                    rdoRtime0.Checked = true;
                    break;
                case "09:30":
                    rdoRtime1.Checked = true;
                    break;
                case "10:00":
                    rdoRtime2.Checked = true;
                    break;
                case "10:30":
                    rdoRtime3.Checked = true;
                    break;
                case "11:00":
                    rdoRtime4.Checked = true;
                    break;
                case "11:30":
                    rdoRtime5.Checked = true;
                    break;
                case "12:00":
                    rdoRtime6.Checked = true;
                    break;
                case "12:30":
                    rdoRtime7.Checked = true;
                    break;
                case "13:00":
                    rdoRtime8.Checked = true;
                    break;
                case "13:30":
                    rdoRtime9.Checked = true;
                    break;
                case "14:00":
                    rdoRtime10.Checked = true;
                    break;
                case "14:30":
                    rdoRtime11.Checked = true;
                    break;
                case "15:00":
                    rdoRtime12.Checked = true;
                    break;
                case "15:30":
                    rdoRtime13.Checked = true;
                    break;
                case "16:00":
                    rdoRtime14.Checked = true;
                    break;
                case "16:30":
                    rdoRtime15.Checked = true;
                    break;
            }

            Search_Data();
        }

        void Schedule_Read()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strDrCode = "";
            string strDate = "";
            string strJin = "";

            lblInfo.Text = "";

            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {
                if (ctl is RadioButton)
                {
                    if (VB.Left(((RadioButton)ctl).Name, 8) == "rdoRtime")
                    {
                        ((RadioButton)ctl).Enabled = true;
                    }
                }
            }

            strDrCode = GstrDrCode;
            strDate = strProcessDate;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT GbJin";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                SQL = SQL + ComNum.VBLF + " WHERE DrCode = '" + strDrCode + "'";
                SQL = SQL + ComNum.VBLF + "   AND SchDate  = TO_DATE('" + strProcessDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strJin = dt.Rows[0]["GbJin"].ToString().Trim();

                    switch (strJin)
                    {
                        case "1":
                            lblInfo.Text = "종일 진료일";
                            break;
                        case "2":
                            lblInfo.Text = "오전 진료일";
                            break;
                        case "3":
                            lblInfo.Text = "오후 진료일";
                            break;
                        case "4":
                            lblInfo.Text = "당일 휴진";
                            break;
                        case "5":
                            lblInfo.Text = "당일 휴가";
                            break;
                        case "6":
                            lblInfo.Text = "당일 출장";
                            break;
                        default:
                            break;
                    }
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Screen_Clear()
        {
            int i = 0;
            int k = 0;

            ssList_Sheet1.RowCount = 0;
            ssList_Sheet1.RowCount = 1;

            for (i = 2; i < 5; i += 2)
            {
                for (k = 1; k < ssView_Sheet1.ColumnCount; k++)
                {
                    ssView_Sheet1.Cells[i - 1, k - 1].Text = "";
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clsOrdFunction.Pat.RDATE = "";
            clsOrdFunction.Pat.RTime = "";
            clsOrdFunction.Pat.RDrCode = "";
            clsOrdFunction.Pat.Exam = "";
            clsOrdFunction.Pat.ResSMSNot = "";

            this.Close();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            Reserved_Date_Checke();

            clsOrdFunction.Pat.RDATE = dtpRDate.Value.ToString("yyyyMMdd");

            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {
                if (ctl is RadioButton)
                {
                    if (VB.Left(((RadioButton)ctl).Name, 8) == "rdoRtime")
                    {
                        if (((RadioButton)ctl).Checked == true)
                        {
                            clsOrdFunction.Pat.RTime = strRTime[Convert.ToInt32(VB.Right(((RadioButton)ctl).Name, 1))];
                            break;
                        }
                    }
                }
            }

            clsOrdFunction.Pat.RDrCode = GstrDrCode.Trim();
            clsOrdFunction.Pat.GbChojae = "3";   //재진

            this.Close();
        }

        void Reserved_Date_Checke()
        {
            string strFlag = "";

            if (dtpRDate.Value <= Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")))
            {
                ComFunc.MsgBox("예약일은 오늘 이후부터 가능합니다.");
                dtpRDate.Focus();
                return;
            }

            strFlag = HolyDay_Check(dtpRDate.Value.ToString("yyyy-MM-dd"));

            switch (strFlag)
            {
                case "Sun":
                    ComFunc.MsgBox("일요일입니다. 날짜 확인 요망 !!");
                    dtpRDate.Focus();
                    break;
                case "*":
                    ComFunc.MsgBox("공휴일입니다. 날짜 확인 요망 !!");
                    dtpRDate.Focus();
                    break;
                default:
                    break;
            }
        }

        string HolyDay_Check(string argDate)
        {
            string rtnVal = "";

            if ((int)Convert.ToDateTime(argDate).DayOfWeek == 1)
            {
                rtnVal = "Sun";
                return rtnVal;
            }

            if ((int)Convert.ToDateTime(argDate).DayOfWeek == 7)
            {
                rtnVal = "Sat";
                return rtnVal;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT HolyDay";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_JOB ";
                SQL = SQL + ComNum.VBLF + " WHERE JobDate = TO_DATE('" + argDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["HolyDay"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        void Search_Data()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            int nRow = 0;
            int nCol = 0;

            string strRCnt = "";

            Reserved_Date_Checke();
            Screen_Clear();
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT R.Sname, R.Pano, TO_CHAR(Date1,'yy-mm-dd') Date33, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(Date3,'HH24:Mi') RTime, Tel ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW R, ";
                SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_PATIENT  P  ";
                SQL = SQL + ComNum.VBLF + " WHERE Date3 >= TO_DATE('" + dtpRDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND Date3 < TO_DATE('" + dtpRDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND R.DrCode = '" + GstrDrCode.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   AND R.TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND R.RETDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND R.Pano   = P.Pano(+) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY TO_CHAR(Date3,'HH24:Mi')           ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssList_Sheet1.RowCount = dt.Rows.Count;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Date33"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["RTime"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Tel"].ToString().Trim();

                    #region Time_Add

                    if (Convert.ToDateTime("00:00") >= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim())
                        && Convert.ToDateTime("09:00") <= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim()))
                    {
                        nRow = 1;
                        nCol = 0;
                    }
                    else if (Convert.ToDateTime("09:01") >= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim())
                        && Convert.ToDateTime("09:30") <= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim()))
                    {
                        nRow = 1;
                        nCol = 1;
                    }
                    else if (Convert.ToDateTime("09:31") >= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim())
                        && Convert.ToDateTime("10:00") <= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim()))
                    {
                        nRow = 1;
                        nCol = 2;
                    }
                    else if (Convert.ToDateTime("10:01") >= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim())
                        && Convert.ToDateTime("10:30") <= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim()))
                    {
                        nRow = 1;
                        nCol = 3;
                    }
                    else if (Convert.ToDateTime("10:31") >= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim())
                        && Convert.ToDateTime("11:00") <= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim()))
                    {
                        nRow = 1;
                        nCol = 4;
                    }
                    else if (Convert.ToDateTime("11:01") >= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim())
                        && Convert.ToDateTime("11:30") <= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim()))
                    {
                        nRow = 1;
                        nCol = 5;
                    }
                    else if (Convert.ToDateTime("11:31") >= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim())
                        && Convert.ToDateTime("12:00") <= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim()))
                    {
                        nRow = 1;
                        nCol = 6;
                    }
                    else if (Convert.ToDateTime("12:01") >= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim())
                        && Convert.ToDateTime("12:30") <= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim()))
                    {
                        nRow = 1;
                        nCol = 7;
                    }
                    else if (Convert.ToDateTime("12:31") >= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim())
                        && Convert.ToDateTime("13:00") <= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim()))
                    {
                        nRow = 3;
                        nCol = 0;
                    }
                    else if (Convert.ToDateTime("13:01") >= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim())
                        && Convert.ToDateTime("13:30") <= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim()))
                    {
                        nRow = 3;
                        nCol = 1;
                    }
                    else if (Convert.ToDateTime("13:31") >= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim())
                        && Convert.ToDateTime("14:00") <= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim()))
                    {
                        nRow = 3;
                        nCol = 2;
                    }
                    else if (Convert.ToDateTime("14:01") >= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim())
                        && Convert.ToDateTime("14:30") <= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim()))
                    {
                        nRow = 3;
                        nCol = 3;
                    }
                    else if (Convert.ToDateTime("14:31") >= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim())
                        && Convert.ToDateTime("15:00") <= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim()))
                    {
                        nRow = 3;
                        nCol = 4;
                    }
                    else if (Convert.ToDateTime("15:01") >= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim())
                        && Convert.ToDateTime("15:30") <= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim()))
                    {
                        nRow = 3;
                        nCol = 5;
                    }
                    else if (Convert.ToDateTime("15:31") >= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim())
                        && Convert.ToDateTime("16:00") <= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim()))
                    {
                        nRow = 3;
                        nCol = 6;
                    }
                    else if (Convert.ToDateTime("16:01") >= Convert.ToDateTime(dt.Rows[i]["RTime"].ToString().Trim()))
                    {
                        nRow = 3;
                        nCol = 7;
                    }

                    strRCnt = (VB.Val(ssView_Sheet1.Cells[nRow, nCol].Text.Trim()) + 1).ToString();
                    ssView_Sheet1.Cells[nRow, nCol].Text = strRCnt;

                    #endregion

                }
                dt.Dispose();
                dt = null;


                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmMedReserved_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            int i = 0;

            Screen_Clear();

            btnRegist.Enabled = true;
            btnCancel.Enabled = true;

            lblName.Text = clsOrdFunction.Pat.sName;
            lblStat.Text = "수 납 전";

            if (clsOrdFunction.Pat.RDATE != "")
            {
                Reserved_Data_Show();
            }

            strProcessDate = dtpRDate.Value.ToString("yyyy-MM-dd");

            Schedule_Read();

            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {
                if (ctl is RadioButton)
                {
                    if (VB.Left(((RadioButton)ctl).Name, 8) == "rdoRtime")
                    {
                        strRTime[i] = ((RadioButton)ctl).Text;
                        i++;
                    }
                }
            }

            dtpRDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(1);
        }

        private void dtpRDate_Leave(object sender, EventArgs e)
        {
            string strJobDate = "";

            strJobDate = dtpRDate.Value.ToString("yyyy-MM-dd");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT HolyDay";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_JOB ";
                SQL = SQL + ComNum.VBLF + " WHERE JobDate = TO_DATE('" + strJobDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["HolyDay"].ToString().Trim() == "*")
                    {
                        ComFunc.MsgBox("지정하신 예약일자는 공휴일입니다.");
                        if (dtpRDate.Enabled == true)
                        {
                            dtpRDate.Focus();
                        }
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                dt.Dispose();
                dt = null;

                if (VB.DateDiff("D", Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")), dtpRDate.Value) > 60)
                {
                    ComFunc.MsgBox("예약일자가 60일 이후입니다. 확인 요망 !!");
                }

                strProcessDate = dtpRDate.Value.ToString("yyyy-MM-dd");

                Schedule_Read();

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
