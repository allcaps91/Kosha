using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    public partial class frmOcsCpSub : Form
    {
        public frmOcsCpSub()
        {
            InitializeComponent();
        }

        private void frmOcsCpSub_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssView_Sheet1.RowCount = 0;

            SetCboMain();
        }

        private void SetCboMain()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboMainCode.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "    CODE, NAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "    WHERE GUBUN = 'CP_관리' ";
                SQL = SQL + ComNum.VBLF + "        AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "ORDER BY SORT";

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
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboMainCode.Items.Add(dt.Rows[i]["CODE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                    }

                    cboMainCode.SelectedIndex = 0;
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView_Sheet1.RowCount = 0;

            string strGUBUN = "";

            if (cboSubCode.Text.Trim() != "")
            {
                strGUBUN = VB.Split(cboSubCode.Text.Trim(), ".")[1].ToString().Trim();
            }
            else
            {
                strGUBUN = VB.Split(cboMainCode.Text.Trim(), ".")[1].ToString().Trim();
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "    CODE, NAME, ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "    WHERE GUBUN = '" + strGUBUN + "' ";
                SQL = SQL + ComNum.VBLF + "        AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "ORDER BY SORT";

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
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (SaveData() == true)
            {
                GetData();
            }
        }

        private bool SaveData()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strGUBUN = "";
            string strSysDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            if (cboSubCode.Text.Trim() != "")
            {
                strGUBUN = VB.Split(cboSubCode.Text.Trim(), ".")[1].ToString().Trim();
            }
            else
            {
                strGUBUN = VB.Split(cboMainCode.Text.Trim(), ".")[1].ToString().Trim();
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true)
                    {
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "    CODE, NAME, ROWID";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                        SQL = SQL + ComNum.VBLF + "    WHERE GUBUN = '" + strGUBUN + "' ";
                        SQL = SQL + ComNum.VBLF + "        AND DELDATE IS NULL";
                        SQL = SQL + ComNum.VBLF + "ORDER BY SORT";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        if (dt.Rows.Count == 0)
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_BCODE";
                            SQL = SQL + ComNum.VBLF + "     (GUBUN, CODE, NAME, JDATE, ENTSABUN, ENTDATE, SORT)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";
                            SQL = SQL + ComNum.VBLF + "         '" + strGUBUN + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + ssView_Sheet1.Cells[i, 1].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + ssView_Sheet1.Cells[i, 2].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strSysDate + "', 'YYYY-MM-DD'), ";
                            SQL = SQL + ComNum.VBLF + "         '" + clsType.User.Sabun + "', ";
                            SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                            SQL = SQL + ComNum.VBLF + "         '" + VB.Val(ssView_Sheet1.Cells[i, 1].Text.Trim()) + "' ";
                            SQL = SQL + ComNum.VBLF + "     )";
                        }
                        else
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_BCODE";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         NAME = '" + ssView_Sheet1.Cells[i, 2].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         ENTSABUN = '" + clsType.User.Sabun + "', ";
                            SQL = SQL + ComNum.VBLF + "         ENTDATE = SYSDATE";
                            SQL = SQL + ComNum.VBLF + "WHERE GUBUN = '" + strGUBUN + "' ";
                            SQL = SQL + ComNum.VBLF + "     AND CODE = '" + ssView_Sheet1.Cells[i, 1].Text.Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "     AND DELDATE IS NULL";
                        }

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        dt.Dispose();
                        dt = null;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (DelData() == true)
            {
                GetData();
            }
        }

        private bool DelData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strSysDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true)
                    {
                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_BCODE";
                        SQL = SQL + ComNum.VBLF + "     SET";
                        SQL = SQL + ComNum.VBLF + "         DELDATE = TO_DATE('" + strSysDate + "', 'YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + ssView_Sheet1.Cells[i, 3].Text.Trim() + "' ";

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
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboMainCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strGUBUN = VB.Split(cboMainCode.Text.Trim(), ".")[1].ToString().Trim();

            cboSubCode.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "    CODE, NAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "    WHERE GUBUN = '" + strGUBUN + "' ";
                SQL = SQL + ComNum.VBLF + "        AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "ORDER BY SORT";

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
                    GetData();
                }
                else
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboSubCode.Items.Add(dt.Rows[i]["CODE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                    }

                    cboSubCode.SelectedIndex = 0;
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void cboSubCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void ssView_EditModeOff(object sender, EventArgs e)
        {
            if (ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 1].Text == "")
            {
                ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 0].Value = true;

                if (ssView_Sheet1.ActiveRowIndex == 0)
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 1].Text = "01";
                }
                else
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 1].Text = (VB.Val(ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex - 1, 1].Text) + 1).ToString("00");
                }
            }

            if (ssView_Sheet1.ActiveRowIndex == ssView_Sheet1.RowCount - 1)
            {
                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            }
        }
    }
}
