using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmSimsa.cs
    /// Description     : 보험심사과에서보낸메시지
    /// Author          : 박창욱
    /// Create Date     : 2018-02-09
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrinfo\FrmSIMSA.frm(FrmSIMSA.frm) >> frmSimsa.cs 폼이름 재정의" />	
    public partial class frmSimsa : Form
    {
        string FstrROWID = "";
        string FstrBUSE = "";

        public frmSimsa()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtSIMSA.Text = "";
            txtWARD.Text = "";
            btnSave.Enabled = false;
            FstrROWID = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (FstrROWID == "")
            {
                return;
            }

            if (clsVbfunc.NurseSystemManagerChk(clsDB.DbCon, clsType.User.Sabun) == true)
            {
                return;
            }

            if (txtWARD.Text.Trim() == "")
            {
                ComFunc.MsgBox("회신할 내용이 공란입니다.");
                return;
            }

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " UPDATE " + ComNum.DB_PMPA + "ETC_MSG_LIST SET ";
                SQL = SQL + ComNum.VBLF + " RETMEMO = '" + txtWARD.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + " WARDSABUN = " + clsType.User.Sabun + ", ";
                SQL = SQL + ComNum.VBLF + " WARDWRITEDATE = SYSDATE, ";
                SQL = SQL + ComNum.VBLF + " GBOK = '1' ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");

                Search_Data();

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

            Search_Data();
        }

        void Search_Data()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            txtSIMSA.Text = "";
            txtWARD.Text = "";
            btnSave.Enabled = false;
            FstrROWID = "";

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(WRITEDATE,'YYYY-MM-DD HH24:MI') WRITEDATE, WRITESABUN, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(CONFIRMDATE,'YYYY-MM-DD HH24:MI') CONFIRMDATE, MEMO, RETMEMO, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "ETC_MSG_LIST ";
                SQL = SQL + ComNum.VBLF + " WHERE WRITEDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "    AND WRITEDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
                if (clsVbfunc.NurseSystemManagerChk(clsDB.DbCon, clsType.User.Sabun) == true)
                {
                }
                else
                {
                    if (FstrBUSE == "033104")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND BUSE IN ('033104','101230') ";
                    }
                    else if (FstrBUSE == "101753")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND BUSE IN ('033116','101753')";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND BUSE = '" + FstrBUSE + "' ";
                    }
                }
                switch (cboGUBUN.Text.Trim())
                {
                    case "미완료":
                        SQL = SQL + ComNum.VBLF + "  AND RETMEMO IS NULL ";
                        break;
                    case "완료":
                        SQL = SQL + ComNum.VBLF + "  AND RETMEMO IS NOT NULL ";
                        break;
                }
                SQL = SQL + ComNum.VBLF + " AND GUBUN = 'SIMSA' ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY WRITEDATE DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WRITEDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["WRITESABUN"].ToString().Trim());
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["CONFIRMDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["RETMEMO"].ToString().Trim() != "" ? "완료" : "";
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["MEMO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

        private void frmSimsa_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            cboGUBUN.Items.Clear();
            cboGUBUN.Items.Add("전체");
            cboGUBUN.Items.Add("미완료");
            cboGUBUN.Items.Add("완료");
            cboGUBUN.SelectedIndex = 1;

            dtpSDate.Value = dtpSDate.Value.AddDays(-7);

            txtSIMSA.Text = "";
            txtWARD.Text = "";
            btnSave.Enabled = false;
            FstrROWID = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT BUSE";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "INSA_MST";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + VB.Val(clsType.User.Sabun).ToString("00000") + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    FstrBUSE = dt.Rows[0]["BUSE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                Search_Data();

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.Row < 0)
            {
                return;
            }

            FstrROWID = ssView_Sheet1.Cells[e.Row, 5].Text.Trim();

            txtSIMSA.Text = "";
            txtWARD.Text = "";
            btnSave.Enabled = false;

            if (FstrROWID == "")
            {
                return;
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT MEMO, RETMEMO";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "ETC_MSG_LIST ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    txtSIMSA.Text = dt.Rows[0]["MEMO"].ToString().Trim();
                    txtWARD.Text = dt.Rows[0]["RETMEMO"].ToString().Trim();
                    btnSave.Enabled = true;

                    if (clsVbfunc.NurseSystemManagerChk(clsDB.DbCon, clsType.User.Sabun) == true)
                    {
                    }
                    else
                    {
                        SQL = "";
                        SQL = " UPDATE " + ComNum.DB_PMPA + "ETC_MSG_LIST SET ";
                        SQL = SQL + ComNum.VBLF + " CONFIRMDATE = SYSDATE";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

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
                }

                dt.Dispose();
                dt = null;

                //clsDB.setRollbackTran(clsDB.DbCon);   //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
