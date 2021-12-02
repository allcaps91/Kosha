using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmDoctPlan2.cs
    /// Description     : 의사별진료스케쥴
    /// Author          : 박창욱
    /// Create Date     : 2018-01-20
    /// Update History  : 
    /// </summary>
    /// <history>      
    /// </history>
    /// <seealso cref= "\nurse\nropd\nropd12.frm(FrmDoctPlan2.frm) >> frmDoctPlan2.cs 폼이름 재정의" />	
    public partial class frmDoctPlan2 : Form
    {
        string[] FstrHolyDay = new string[32];
        string[] FstrYoil = new string[32];
        string[] FstrYoilJin = new string[7];

        public frmDoctPlan2()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            cboYYMM.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int nLastDay = 0;
            string strFDate = "";
            string strTDate = "";

            string strDrCode = "";

            ComFunc cf = new ComFunc();

            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;

            strFDate = cboYYMM.Text + "-01";
            strTDate = cf.READ_LASTDAY(clsDB.DbCon, strFDate);
            nLastDay = (int)VB.Val(VB.Right(strTDate, 2));

            if (ComFunc.MsgBoxQ("정말 삭제를 하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 2; i <= ssView_Sheet1.RowCount; i++)
                {
                    strDrCode = ssView_Sheet1.Cells[i - 1, 33].Text.Trim();

                    //해당의사의 스케쥴을 삭제함
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                    SQL = SQL + ComNum.VBLF + "WHERE DrCode = '" + strDrCode + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SchDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND SchDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                Screen_Clear();
                cboYYMM.Focus();

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;
            int k = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int nLastDay = 0;
            string strFDate = "";
            string strTDate = "";
            string strDrCode = "";
            string strDate = "";
            string strDayGbn = "";
            string strGbn = "";
            string strOK = "";

            ComFunc cf = new ComFunc();

            btnSave.Enabled = false;
            btnCancel.Enabled = false;

            strFDate = cboYYMM.Text + "-01";
            strTDate = cf.READ_LASTDAY(clsDB.DbCon, strFDate);
            nLastDay = (int)VB.Val(VB.Right(strTDate, 2));

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 2; i <= ssView_Sheet1.RowCount; i++)
                {
                    strDrCode = ssView_Sheet1.Cells[i - 1, 33].Text.Trim();

                    //jjy(2005-06-21) 중복발생함..그래서 삭제후 insert 만 함. 루틴변경시 확인바람.
                    //자료를 UPDATE
                    for (k = 1; k <= nLastDay; k++)
                    {
                        strGbn = ssView_Sheet1.Cells[i - 1, k + 1].Text;
                        strOK = "OK";

                        #region Insert_Rtn

                        strDate = cboYYMM.Text + "-" + k.ToString("00");

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                        SQL = SQL + ComNum.VBLF + " WHERE DrCode = '" + strDrCode + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND SchDate = TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        strDayGbn = "1";
                        if (FstrHolyDay[k] == "*")
                        {
                            strDayGbn = "3";
                        }
                        if (FstrHolyDay[k] == "#")
                        {
                            strDayGbn = "2";
                        }

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "BAS_SCHEDULE VALUES('" + strDrCode + "',";
                        SQL = SQL + ComNum.VBLF + "TO_DATE('" + strDate + "','YYYY-MM-DD'),'" + strDayGbn + "','";
                        SQL = SQL + ComNum.VBLF + strGbn + "',' ') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        #endregion

                        if (strOK == "OK")
                        {
                            clsDB.setCommitTran(clsDB.DbCon);
                        }
                        else
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                        }
                    }
                }

                Screen_Clear();
                cboYYMM.Focus();

                Cursor.Current = Cursors.Default;
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
            int nDay = 0;
            int nLastDay = 0;
            string strGbn = "";
            string strFDate = "";
            string strTDate = "";
            string strYoil = "";
            string strDate = "";
            string strYYMM = "";
            string strDrCode = "";
            string strSchedule = "";

            ComFunc cf = new ComFunc();

            btnSearch.Enabled = false;
            cboYYMM.Enabled = false;

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, cboYYMM.Text.Length - 1);

            strFDate = cboYYMM.Text + "-01";
            strTDate = cf.READ_LASTDAY(clsDB.DbCon, strFDate);
            nLastDay = (int)VB.Val(VB.Right(strTDate, 2));

            for (i = 0; i < 32; i++)
            {
                FstrHolyDay[i] = "";
                FstrYoil[i] = "";
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //해당월에 의사별 스케쥴을 만들었는지 Check
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT COUNT(*) CNT";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                SQL = SQL + ComNum.VBLF + " WHERE SchDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND SchDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) == 0)
                {
                    if (ComFunc.MsgBoxQ("해당월의 스케쥴을 신규로 만드시겠습니까?") == DialogResult.No)
                    {
                        dt.Dispose();
                        dt = null;
                        return;
                    }
                }

                dt.Dispose();
                dt = null;

                //일자별 휴일을 SET
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(JobDate,'DD') ILJA";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_JOB ";
                SQL = SQL + ComNum.VBLF + " WHERE JobDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND JobDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND HolyDay = '*' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nDay = (int)VB.Val(dt.Rows[i]["ILJA"].ToString().Trim());
                    FstrHolyDay[nDay] = "*";
                }

                dt.Dispose();
                dt = null;

                //일자별 요일을 SET
                for (i = 1; i <= nLastDay; i++)
                {
                    strDate = cboYYMM.Text + "-" + i.ToString("00");
                    strYoil = clsVbfunc.GetYoIl(strDate);
                    switch (strYoil)
                    {
                        case "월요일":
                            FstrYoil[i] = "1";
                            break;
                        case "화요일":
                            FstrYoil[i] = "2";
                            break;
                        case "수요일":
                            FstrYoil[i] = "3";
                            break;
                        case "목요일":
                            FstrYoil[i] = "4";
                            break;
                        case "금요일":
                            FstrYoil[i] = "5";
                            break;
                        case "토요일":
                            FstrYoil[i] = "6";
                            break;
                        case "일요일":
                            FstrYoil[i] = "7";
                            break;
                    }
                    if (FstrHolyDay[i] != "*" && FstrYoil[i] == "6")
                    {
                        FstrHolyDay[i] = "#";
                    }

                    ssView_Sheet1.Cells[0, i + 1].Text = ComFunc.LeftH(strYoil, 2);

                    if (FstrHolyDay[i] == "*")
                    {
                        ssView_Sheet1.Cells[0, i + 1].BackColor = lbl1.BackColor;
                    }
                    else if (FstrHolyDay[i] == "#")
                    {
                        ssView_Sheet1.Cells[0, i + 1].BackColor = lbl2.BackColor;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[0, i + 1].BackColor = lbl4.BackColor;
                    }
                }


                //진료과별 의사코드, 성명을 Read
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.DrDept1, A.DrCode, A.DrName ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_DOCTOR A, " + ComNum.DB_PMPA + "BAS_CLINICDEPT B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.TOUR <> 'Y' ";
                SQL = SQL + ComNum.VBLF + "   AND A.SCHEDULE IS  NULL ";
                SQL = SQL + ComNum.VBLF + "   AND A.DrDept1 NOT IN ('HD','HR','PT','TO','R6') ";
                SQL = SQL + ComNum.VBLF + "   AND A.DRDEPT1 = B.DEPTCODE(+)";
                SQL = SQL + ComNum.VBLF + " ORDER BY B.PrintRanking, A.DrDept1, A.PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count + 1;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                //의사별로 자료를 Read
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strDrCode = dt.Rows[i]["DrCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i + 1, 0].Text = dt.Rows[i]["DrDept1"].ToString().Trim();
                    ssView_Sheet1.Cells[i + 1, 1].Text = dt.Rows[i]["DrName"].ToString().Trim();
                    ssView_Sheet1.Cells[i + 1, 33].Text = strDrCode;

                    //비교용 내역을 Clear
                    for (k = 35; k < 66; k++)
                    {
                        ssView_Sheet1.Cells[i + 1, k - 1].Text = "";
                    }

                    //진료여부를 Read
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(SchDate,'DD') ILJA,GbJin";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                    SQL = SQL + ComNum.VBLF + " WHERE DrCode = '" + strDrCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND SchDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND SchDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    for (k = 0; k < dt1.Rows.Count; k++)
                    {
                        nDay = (int)VB.Val(dt1.Rows[k]["ILJA"].ToString().Trim());
                        strGbn = dt1.Rows[k]["GbJin"].ToString().Trim();

                        ssView_Sheet1.Cells[i + 1, nDay + 1].Text = strGbn;
                        switch (strGbn)
                        {
                            case "1":
                                ssView_Sheet1.Cells[i + 1, nDay + 1].BackColor = lbl1.BackColor;
                                break;
                            case "2":
                                ssView_Sheet1.Cells[i + 1, nDay + 1].BackColor = lbl2.BackColor;
                                break;
                            case "3":
                                ssView_Sheet1.Cells[i + 1, nDay + 1].BackColor = lbl3.BackColor;
                                break;
                            default:
                                ssView_Sheet1.Cells[i + 1, nDay + 1].BackColor = lbl4.BackColor;
                                break;
                        }
                        ssView_Sheet1.Cells[i + 1, nDay + 33].Text = strGbn;
                    }
                    dt1.Dispose();
                    dt1 = null;

                    //주 40시간에 관련한 OFF
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT B.SCHEDULE ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_DOCTOR A, " + ComNum.DB_PMPA + "NUR_SCHEDULE1 B";
                    SQL = SQL + ComNum.VBLF + " WHERE A.DRCODE = '" + strDrCode + "'";
                    SQL = SQL + ComNum.VBLF + "   AND A.SABUN = B.SABUN";
                    SQL = SQL + ComNum.VBLF + "   AND B.YYMM = '" + strYYMM + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        strSchedule = dt1.Rows[0]["Schedule"].ToString().Trim();
                        for (k = 0; k < 32; k++)
                        {
                            switch (VB.Mid(strSchedule, k * 4 + 1, 4).Trim())
                            {
                                case "OFFA":
                                case "OFFP":
                                    ssView_Sheet1.Cells[i + 1, k + 2].ForeColor = Color.FromArgb(255, 0, 0);
                                    break;
                            }
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //의사별 요일별 스케쥴을 Read
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(SchDate,'DD') ILJA,GbJin";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                    SQL = SQL + ComNum.VBLF + " WHERE DrCode = '" + strDrCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND SchDate >= TO_DATE('1990-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND SchDate <= TO_DATE('1990-01-06','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    for (k = 1; k < 7; k++)
                    {
                        FstrYoilJin[k] = " ";
                    }
                    for (k = 0; k < dt1.Rows.Count; k++)
                    {
                        nDay = (int)VB.Val(dt1.Rows[k]["ILJA"].ToString().Trim());
                        strGbn = dt1.Rows[k]["GbJin"].ToString().Trim();
                        if (strGbn != " ")
                        {
                            FstrYoilJin[nDay] = strGbn;
                        }
                    }
                    dt1.Dispose();
                    dt1 = null;

                    for (k = 1; k <= nLastDay; k++)
                    {
                        if (ssView_Sheet1.Cells[i + 1, k + 1].Text == "" && FstrYoil[k] != "7")
                        {
                            strGbn = FstrYoilJin[(int)VB.Val(FstrYoil[k])];
                            ssView_Sheet1.Cells[i + 1, k + 1].Text = strGbn;
                            switch (strGbn)
                            {
                                case "1":
                                    ssView_Sheet1.Cells[i + 1, k + 1].BackColor = lbl1.BackColor;
                                    break;
                                case "2":
                                    ssView_Sheet1.Cells[i + 1, k + 1].BackColor = lbl2.BackColor;
                                    break;
                                case "3":
                                    ssView_Sheet1.Cells[i + 1, k + 1].BackColor = lbl3.BackColor;
                                    break;
                                default:
                                    ssView_Sheet1.Cells[i + 1, k + 1].BackColor = lbl4.BackColor;
                                    break;
                            }
                        }
                    }
                }

                //토요일, 휴일 Sheet에 Data Set
                for (i = 1; i <= nLastDay; i++)
                {
                    if (FstrHolyDay[i] != " ")
                    {
                        for (k = 1; k < ssView_Sheet1.NonEmptyRowCount; k++)
                        {
                            strGbn = ssView_Sheet1.Cells[k, i + 1].Text;
                            if (FstrHolyDay[i] == "*")
                            {
                                //휴일
                                ssView_Sheet1.Cells[k, i + 1].Text = "4";
                                ssView_Sheet1.Cells[k, i + 1].BackColor = lbl4.BackColor;
                                ssView_Sheet1.Cells[k, i + 1].Locked = true;
                            }
                            else if (FstrHolyDay[i] == "#")
                            {
                                //토요일
                                if (strGbn == "1" || strGbn == "3")
                                {
                                    ssView_Sheet1.Cells[k, i + 1].Text = "2";
                                    ssView_Sheet1.Cells[k, i + 1].BackColor = lbl2.BackColor;
                                }
                            }
                        }
                    }
                }

                lblMsg.Text = "";
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnDelete.Enabled = true;
                ssView.Enabled = true;
                ssView.Focus();

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSearch_Enter(object sender, EventArgs e)
        {
            clsPublic.GstrMsgList = "스케쥴이 입력 안 된 일자는 요일별 스케쥴을 기준하여 자동작성됩니다.";
            lblMsg.Text = clsPublic.GstrMsgList;
        }

        private void btnSearch_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void cboYYMM_Enter(object sender, EventArgs e)
        {
            clsPublic.GstrMsgList = "스케쥴이 입력 안 된 일자는 요일별 스케쥴을 기준하여 자동작성됩니다.";
            lblMsg.Text = clsPublic.GstrMsgList;
        }

        private void cboYYMM_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void frmDoctPlan2_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            int nYY = 0;
            int nMM = 0;

            string strSysDate = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            for (i = 35; i < 65; i++)
            {
                ssView_Sheet1.Columns[i].Visible = false;
            }

            //년월 cbo Set
            nYY = (int)VB.Val(VB.Left(strSysDate, 4));
            nMM = (int)VB.Val(VB.Mid(strSysDate, 6, 2));
            cboYYMM.Items.Clear();
            for (i = 1; i < 13; i++)
            {
                cboYYMM.Items.Add(nYY.ToString("0000") + "-" + nMM.ToString("00"));
                nMM += 1;
                if (nMM == 13)
                {
                    nYY += 1;
                    nMM = 1;
                }
                cboYYMM.SelectedIndex = 0;
                ssView_Sheet1.Columns[33].Visible = false;
            }

            Screen_Clear();
        }

        private void btnPlan_Click(object sender, EventArgs e)
        {
            frmDoctPlan1 frmDoctPlan1X = new frmDoctPlan1();
            frmDoctPlan1X.StartPosition = FormStartPosition.CenterParent;
            frmDoctPlan1X.ShowDialog();
        }

        private void lbl4_DoubleClick(object sender, EventArgs e)
        {
            btnDelete.Enabled = true;
        }

        private void ssView_EditModeOff(object sender, EventArgs e)
        {
            if (ssView_Sheet1.ActiveColumnIndex < 2 || ssView_Sheet1.ActiveColumnIndex > 32)
            {
                return;
            }

            string strData = "";

            strData = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, ssView_Sheet1.ActiveColumnIndex].Text.Trim();

            if (string.Compare(strData, "1") < 0 || string.Compare(strData, "8") > 0)
            {
                ComFunc.MsgBox("구분은 1-8까지만 가능합니다.");
                strData = "";
            }

            if (FstrHolyDay[ssView_Sheet1.ActiveColumnIndex - 3] == "#")
            {
                //토요일
                if (strData == "1" || strData == "3")
                {
                    strData = "2";
                    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, ssView_Sheet1.ActiveColumnIndex].Text = "2";
                }

                switch (strData)
                {
                    case "1":
                        ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, ssView_Sheet1.ActiveColumnIndex].BackColor = lbl1.BackColor;
                        break;
                    case "2":
                        ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, ssView_Sheet1.ActiveColumnIndex].BackColor = lbl2.BackColor;
                        break;
                    case "3":
                        ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, ssView_Sheet1.ActiveColumnIndex].BackColor = lbl3.BackColor;
                        break;
                    default:
                        ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, ssView_Sheet1.ActiveColumnIndex].BackColor = lbl4.BackColor;
                        ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, ssView_Sheet1.ActiveColumnIndex].Text = "";
                        break;
                }
            }
        }

        void Screen_Clear()
        {
            int i = 0;

            cboYYMM.Enabled = true;
            btnSearch.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            ssView_Sheet1.RowCount = 1;
            ssView_Sheet1.RowCount = 30;

            for (i = 1; i <= ssView_Sheet1.ColumnCount; i++)
            {
                ssView_Sheet1.Cells[0, i - 1].Text = "";
                ssView_Sheet1.Cells[0, i - 1].BackColor = lbl4.BackColor;
            }
        }
    }
}
