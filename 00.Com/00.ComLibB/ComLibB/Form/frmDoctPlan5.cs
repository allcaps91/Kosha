using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-01-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\nurse\nropd\nropd04.frm >> frmDoctPlan5.cs 폼이름 재정의" />

    public partial class frmDoctPlan5 : Form
    {
        ComFunc CF = new ComFunc();

        string[] FstrHolyDay = new string[32];
        string[] FstrYoil = new string[32];
        string[] FstrYoilJin = new string[7];

        public frmDoctPlan5()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
            ComboYYMM.Select();
        }

        private void Cancel()
        {
            btnSearch.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            SS1_Sheet1.RowCount = 1;
            SS1_Sheet1.RowCount = 0;
        }

        private bool Delete()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            //int j = 0;
            int nLastDay = 0;
            string strFDate = "";
            string strTDate = "";
            string strDrCode = "";
            //string strDate = "";
            //string strDayGbn = "";
            string[] strGbn = new string[2];
            string[] strYoil = new string[2];
            //string strOldGbn = "";

            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;

            strFDate = ComboYYMM.Text + "-01";
            strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            nLastDay = (int)VB.Val(VB.Right(strTDate, 2));

            if (ComFunc.MsgBoxQ("정말 삭제를 하시겠습니까?", "경고", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 2; i <= SS1_Sheet1.RowCount; i++)
                {
                    strDrCode = SS1_Sheet1.Cells[i - 1, 33].Text;

                    //'해당의사의 스케쥴을 삭제함
                    SQL = "";
                    SQL = "DELETE BAS_SELECT_SCHE ";
                    SQL = SQL + ComNum.VBLF + "WHERE DrCode = '" + strDrCode + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND SDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제 하였습니다.");
                Cursor.Current = Cursors.Default;

                Cancel();
                ComboYYMM.Select();

                rtnVal = true;
                return rtnVal;

            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }


        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private bool Save()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            int j = 0;
            int nLastDay = 0;
            string strFDate = "";
            string strTDate = "";
            string strDrCode = "";
            string strDate = "";
            string strDayGbn = "";
            string strGbn = "";
            string[] strYoil = new string[2];

            Cursor.Current = Cursors.WaitCursor;

            btnSave.Enabled = false;
            btnCancel.Enabled = false;

            strFDate = ComboYYMM.Text + "-01";
            strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            nLastDay = (int)VB.Val(VB.Right(strTDate, 2));

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 2; i <= SS1_Sheet1.RowCount; i++)
                {
                    strDrCode = SS1_Sheet1.Cells[i - 1, 33].Text;

                    for (j = 1; j <= nLastDay; j++)
                    {
                        strGbn = SS1_Sheet1.Cells[i - 1, (j + 2) - 1].Text;


                        /// <summary>
                        ///   CmdOK_INSERT_Rtn
                        /// </summary>
                        #region 

                        strDate = ComboYYMM.Text + "-" + j.ToString("00");

                        SQL = "";
                        SQL = "DELETE BAS_SELECT_SCHE ";
                        SQL = SQL + ComNum.VBLF + "WHERE DrCode = '" + strDrCode + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND SchDate = TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        strDayGbn = "1";
                        if (FstrHolyDay[j] == "*")
                        {
                            strDayGbn = "3";
                        }
                        if (FstrHolyDay[j] == "#")
                        {
                            strDayGbn = "2";
                        }

                        SQL = "";
                        SQL = "INSERT INTO BAS_SELECT_SCHE VALUES('" + strDrCode + "',";
                        SQL = SQL + "TO_DATE('" + strDate + "','YYYY-MM-DD'),'" + strDayGbn + "','";
                        SQL = SQL + strGbn + "',' ') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        #endregion
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Save() == true)
            {
                Cancel();
                ComboYYMM.Select();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int j = 0;
            int nREAD = 0;
            int nDay = 0;
            int nLastDay = 0;
            string strFDate = "";
            string strTDate = "";
            string strYoil = "";
            string strDate = "";
            string strYYMM = "";
            string strDrCode = "";
            string strSCHEDULE = "";
            string strGbn = "";
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dtFn = null;
            string SqlErr = "";

            btnSearch.Enabled = false;

            strYYMM = VB.Left(ComboYYMM.Text, 4) + VB.Mid(ComboYYMM.Text, 1, 6);

            strFDate = ComboYYMM.Text + "-01";
            strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            nLastDay = (int)VB.Val(VB.Right(strTDate, 2));

            for (i = 1; i <= 31; i++)
            {
                FstrHolyDay[i] = "";
                FstrYoil[i] = " ";
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //'해당월에 의사별 스케쥴을 만들었는지 Check

                SQL = "";
                SQL = "SELECT COUNT(*) CNT FROM BAS_SELECT_SCHE ";
                SQL = SQL + ComNum.VBLF + "WHERE SDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND SDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    if (ComFunc.MsgBoxQ("해당월의 스케쥴을 신규로 만드시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                }
                dt.Dispose();
                dt = null;

                //'일자별 휴일을 SET
                SQL = "";
                SQL = "SELECT TO_CHAR(JobDate,'DD') ILJA FROM BAS_JOB ";
                SQL = SQL + ComNum.VBLF + "WHERE JobDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND JobDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND HolyDay = '*' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nDay = (int)VB.Val(dt.Rows[i]["ILJA"].ToString().Trim());
                        FstrHolyDay[nDay] = "*";
                    }

                    for (i = 1; i <= nLastDay; i++)
                    {
                        strDate = ComboYYMM.Text + "-" + i.ToString("00");
                        strYoil = CF.READ_YOIL(clsDB.DbCon, strDate);

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

                        SS1_Sheet1.Cells[0, (i + 2) - 1].Text = VB.Left(strYoil, 2);

                        if (FstrHolyDay[i] == "*")
                        {
                            SS1_Sheet1.Cells[0, (i + 2) - 1].BackColor = System.Drawing.Color.FromArgb(255, 200, 200);
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    //'진료과별 의사코드,성명을 READ
                    SQL = "";
                    SQL = "SELECT A.DrDept1, A.DrCode, A.DrName ";
                    SQL = SQL + ComNum.VBLF + " FROM BAS_DOCTOR A, BAS_CLINICDEPT B ";
                    SQL = SQL + ComNum.VBLF + "WHERE A.TOUR <> 'Y' ";
                    SQL = SQL + ComNum.VBLF + "  AND A.SCHEDULE IS  NULL ";
                    SQL = SQL + ComNum.VBLF + "  AND A.DrDept1 NOT IN ('HD','HR','PT','TO','R6') ";
                    SQL = SQL + ComNum.VBLF + "  AND A.DRDEPT1 = B.DEPTCODE(+)";
                    SQL = SQL + ComNum.VBLF + "  AND A.GBCHOICE ='Y' ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY B.PrintRanking, A.DrDept1, A.PrintRanking ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    //의사별로 자료를 READ
                    SS1_Sheet1.RowCount = dt.Rows.Count + 1;

                    //의사별로 자료를 READ
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDrCode = dt.Rows[i]["Decode"].ToString().Trim();

                        SS1_Sheet1.Cells[i + 1, 0].Text = dt.Rows[i]["Dedrpt1"].ToString().Trim();
                        SS1_Sheet1.Cells[i + 1, 1].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        SS1_Sheet1.Cells[i + 1, 33].Text = strDrCode;

                        //'비교용 내역을 Clear
                        for (j = 35; j <= 65; j++)
                        {
                            SS1_Sheet1.Cells[i + 1, j - 1].Text = "";
                        }

                        //'진료여부를 READ
                        SQL = "SELECT TO_CHAR(SDate,'DD') ILJA,GbJin FROM BAS_SELECT_SCHE ";
                        SQL = SQL + ComNum.VBLF + "WHERE DrCode = '" + strDrCode + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND SDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  AND SDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.GetDataTable(ref dtFn, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dtFn.Rows.Count > 0)
                        {
                            for (j = 0; j < dtFn.Rows.Count; j++)
                            {
                                nDay = (int)VB.Val(dtFn.Rows[j]["ILJA"].ToString().Trim());
                                strGbn = dtFn.Rows[j]["GbJin"].ToString().Trim();

                                SS1_Sheet1.Cells[i + 1, (nDay + 2) - 1].Text = strGbn;

                                switch (strGbn)
                                {
                                    case "1":
                                        SS1_Sheet1.Cells[i + 1, (nDay + 2) - 1].BackColor = lbl0.BackColor;
                                        break;
                                    default:
                                        SS1_Sheet1.Cells[i + 1, (nDay + 2) - 1].BackColor = lbl1.BackColor;
                                        break;
                                }
                                SS1_Sheet1.Cells[i + 1, (nDay + 34) - 1].Text = strGbn;
                            }
                        }
                        dtFn.Dispose();
                        dtFn = null;

                        // '주40시간에 관련한  OFF
                        SQL = "";
                        SQL = " SELECT B.SCHEDULE ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_DOCTOR A, KOSMOS_PMPA.NUR_SCHEDULE1 B";
                        SQL = SQL + ComNum.VBLF + " WHERE A.DRCODE = '" + strDrCode + "'";
                        SQL = SQL + ComNum.VBLF + "   AND A.SABUN = B.SABUN";
                        SQL = SQL + ComNum.VBLF + "   AND B.YYMM = '" + strYYMM + "' ";

                        SqlErr = clsDB.GetDataTable(ref dtFn, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dtFn.Rows.Count > 0)
                        {
                            strSCHEDULE = dtFn.Rows[0]["Schedule"].ToString().Trim();

                            for (j = 0; j <= 31; j++)
                            {
                                switch (VB.Mid(strSCHEDULE, j * 4 + 1, 4))
                                {
                                    case "OFFA":
                                    case "OFFP":
                                        SS1_Sheet1.Cells[i - 1, (j + 3) - 1].ForeColor = Color.FromArgb(255, 0, 0);
                                        break;
                                }
                            }
                        }
                        dtFn.Dispose();
                        dtFn = null;
                    }

                    //'토요일,휴일 SHEET에 DATA SET
                    for (i = 1; i <= nLastDay; i++)
                    {
                        if (FstrHolyDay[i] != " ")
                        {
                            for (j = 1; j <= SS1_Sheet1.RowCount; j++)
                            {
                                if (FstrHolyDay[i] == "*")
                                {
                                    SS1_Sheet1.Cells[(j + 1) - 1, (i + 2) - 1].BackColor = Color.FromArgb(255, 200, 200);
                                    SS1_Sheet1.Cells[(j + 1) - 1, (i + 2) - 1].Locked = true;
                                }
                                else if (FstrHolyDay[i] == "#")//토요일
                                {
                                    if (strGbn == "1" || strGbn == "3")
                                    {
                                        SS1_Sheet1.Cells[(j + 1) - 1, (i + 2) - 1].Text = "";
                                        SS1_Sheet1.Cells[(j + 1) - 1, (i + 2) - 1].BackColor = Color.FromArgb(255, 200, 200);
                                    }
                                }
                            }
                        }
                    }
                }

                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnDelete.Enabled = true;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void frmDoctPlan5_Load(object sender, EventArgs e)
        {
            int i = 0;
            int nYY = 0;
            int nMM = 0;

            for (i = 35; i <= 65; i++)
            {
                SS1_Sheet1.Columns[i - 1].Visible = false;
            }

            nYY = (int)VB.Val(VB.Left(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"), 4));
            nMM = (int)VB.Val(VB.Mid(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"), 6, 2));

            ComboYYMM.Items.Clear();

            for (i = 1; i <= 12; i++)
            {
                ComboYYMM.Items.Add(nYY.ToString("0000") + "-" + nMM.ToString("00"));
                nMM = nMM + 1;

                if (nMM == 13)
                {
                    nYY = nYY + 1;
                    nMM = 1;
                }
                ComboYYMM.SelectedIndex = 0;

                SS1_Sheet1.Columns[33].Visible = true;
            }

            Cancel();
        }

        private void btnDoctPlan5_Click(object sender, EventArgs e)
        {
            frmDoctPlan1 frmDoctPlan1X = new frmDoctPlan1();
            frmDoctPlan1X.StartPosition = FormStartPosition.CenterParent;
            frmDoctPlan1X.ShowDialog();
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column == 2 || e.Row < 1)
            {
                return;
            }

            if (SS1_Sheet1.Cells[e.Row, e.Column].Text == "*")
            {
                SS1_Sheet1.Cells[e.Row, e.Column].Text = "";
                SS1_Sheet1.Cells[e.Row, e.Column].BackColor = Color.White;
            }
            else
            {
                SS1_Sheet1.Cells[e.Row, e.Column].Text = "*";
                SS1_Sheet1.Cells[e.Row, e.Column].BackColor = Color.FromArgb(200, 200, 255);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
