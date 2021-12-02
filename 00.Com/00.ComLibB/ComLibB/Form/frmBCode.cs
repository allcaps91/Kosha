using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmBCode : Form
    {
        private string GstrCode = string.Empty;
        private string GstrName = string.Empty;
        
        public frmBCode()
        {
            InitializeComponent();
        }

        public frmBCode(string strCode, string strName)
        {
            InitializeComponent();

            GstrCode = strCode;
            GstrName = strName;
        }

        private void frmBCode_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            Set_Combo();

            if (!string.IsNullOrEmpty(GstrCode))
            {
                cboCode.Text = GstrCode;
                lblName.Text = GstrName;
                cboCode.Enabled = false;
            }
        }

        void Set_Combo()
        {
            string SQL = string.Empty;
            DataTable dt = null;
            string SqlErr = string.Empty;

            cboCode.Items.Clear();

            try
            {
                SQL = "SELECT GUBUN";
                SQL = SQL + ComNum.VBLF + "From " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "GROUP BY GUBUN";
                SQL = SQL + ComNum.VBLF + "ORDER BY GUBUN";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cboCode.Items.Add(dt.Rows[i]["GUBUN"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            cboCode.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void cboCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = string.Empty;
            DataTable dt = null;
            ssGichoCode_Sheet1.RowCount = 0;

            try
            {
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "   CODE, NAME, JDATE, DELDATE, ENTSABUN, ENTDATE, SORT, PART, CNT,";
                SQL = SQL + ComNum.VBLF + "   GUBUN2, GUBUN3, GUBUN4, GUBUN5, GUNUM1, GUNUM2, GUNUM3, ROWID";
                SQL = SQL + ComNum.VBLF + "From " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = '" + cboCode.Text.Trim() + "'";
                SQL = SQL + ComNum.VBLF + "ORDER BY SORT";

                string SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssGichoCode_Sheet1.RowCount = dt.Rows.Count + 1;
                    ssGichoCode_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssGichoCode_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ssGichoCode_Sheet1.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ssGichoCode_Sheet1.Cells[i, 3].Text = dt.Rows[i]["JDATE"].ToString().Trim();
                        ssGichoCode_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DELDATE"].ToString().Trim();
                        ssGichoCode_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ENTSABUN"].ToString().Trim();
                        ssGichoCode_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                        ssGichoCode_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SORT"].ToString().Trim();
                        ssGichoCode_Sheet1.Cells[i, 8].Text = dt.Rows[i]["PART"].ToString().Trim();
                        ssGichoCode_Sheet1.Cells[i, 9].Text = dt.Rows[i]["CNT"].ToString().Trim();
                        ssGichoCode_Sheet1.Cells[i, 10].Text = dt.Rows[i]["GUBUN2"].ToString().Trim();
                        ssGichoCode_Sheet1.Cells[i, 11].Text = dt.Rows[i]["GUBUN3"].ToString().Trim();
                        ssGichoCode_Sheet1.Cells[i, 12].Text = dt.Rows[i]["GUBUN4"].ToString().Trim();
                        ssGichoCode_Sheet1.Cells[i, 13].Text = dt.Rows[i]["GUBUN5"].ToString().Trim();
                        ssGichoCode_Sheet1.Cells[i, 14].Text = dt.Rows[i]["GUNUM1"].ToString().Trim();
                        ssGichoCode_Sheet1.Cells[i, 15].Text = dt.Rows[i]["GUNUM2"].ToString().Trim();
                        ssGichoCode_Sheet1.Cells[i, 16].Text = dt.Rows[i]["GUNUM3"].ToString().Trim();
                        ssGichoCode_Sheet1.Cells[i, 17].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
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
            }
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (SaveData() == true)
            {
                GetData();
            }
        }

        private bool SaveData()
        {
            string strCODE = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                int i;
                for (i = 0; i < ssGichoCode_Sheet1.RowCount; i++)
                {
                    string strROWID = ssGichoCode_Sheet1.Cells[i, 17].Text.Trim();

                    if (string.IsNullOrEmpty(strCODE) == false)
                    {
                        if (string.IsNullOrEmpty(strROWID))
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_BCODE";
                            SQL = SQL + ComNum.VBLF + "     (CODE, NAME, JDATE, DELDATE, ENTSABUN, ENTDATE, SORT, PART, CNT,";
                            SQL = SQL + ComNum.VBLF + "     GUBUN2, GUBUN3, GUBUN4, GUBUN5, GUNUM1, GUNUM2, GUNUM3)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";
                            SQL = SQL + ComNum.VBLF + "         '" + cboCode.Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + ssGichoCode_Sheet1.Cells[i, 1].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + ssGichoCode_Sheet1.Cells[i, 2].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + ssGichoCode_Sheet1.Cells[i, 3].Text.Trim() + "', 'YYYY-MM-DD'), ";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + ssGichoCode_Sheet1.Cells[i, 4].Text.Trim() + "', 'YYYY-MM-DD'), ";
                            SQL = SQL + ComNum.VBLF + "         " + ssGichoCode_Sheet1.Cells[i, 5].Text.Trim() + ", ";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + ssGichoCode_Sheet1.Cells[i, 6].Text.Trim() + "', 'YYYY-MM-DD'), ";
                            SQL = SQL + ComNum.VBLF + "         " + ssGichoCode_Sheet1.Cells[i, 7].Text.Trim() + ", ";
                            SQL = SQL + ComNum.VBLF + "         '" + ssGichoCode_Sheet1.Cells[i, 8].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         " + ssGichoCode_Sheet1.Cells[i, 9].Text.Trim() + ", ";
                            SQL = SQL + ComNum.VBLF + "         '" + ssGichoCode_Sheet1.Cells[i, 10].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + ssGichoCode_Sheet1.Cells[i, 11].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + ssGichoCode_Sheet1.Cells[i, 12].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + ssGichoCode_Sheet1.Cells[i, 13].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         " + ssGichoCode_Sheet1.Cells[i, 14].Text.Trim() + ", ";
                            SQL = SQL + ComNum.VBLF + "         " + ssGichoCode_Sheet1.Cells[i, 15].Text.Trim() + ", ";
                            SQL = SQL + ComNum.VBLF + "         " + ssGichoCode_Sheet1.Cells[i, 16].Text.Trim();
                            SQL = SQL + ComNum.VBLF + "     )";
                        }
                        else
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_BCODE";
                            SQL = SQL + ComNum.VBLF + "SET ";
                            SQL = SQL + ComNum.VBLF + "   CODE     = '" + ssGichoCode_Sheet1.Cells[i, 1].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "   NAME     = '" + ssGichoCode_Sheet1.Cells[i, 2].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "   JDATE    = TO_DATE('" + ssGichoCode_Sheet1.Cells[i, 3].Text.Trim() + "', 'YYYY-MM-DD'), ";
                            SQL = SQL + ComNum.VBLF + "   DELDATE  = TO_DATE('" + ssGichoCode_Sheet1.Cells[i, 4].Text.Trim() + "', 'YYYY-MM-DD'), ";
                            SQL = SQL + ComNum.VBLF + "   ENTSABUN = '" + ssGichoCode_Sheet1.Cells[i, 5].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "   ENTDATE  = TO_DATE('" + ssGichoCode_Sheet1.Cells[i, 6].Text.Trim() + "', 'YYYY-MM-DD'), ";
                            SQL = SQL + ComNum.VBLF + "   SORT     = "  + ssGichoCode_Sheet1.Cells[i, 7].Text.Trim() + ", ";
                            SQL = SQL + ComNum.VBLF + "   PART     = '" + ssGichoCode_Sheet1.Cells[i, 8].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "   CNT      = "  + ssGichoCode_Sheet1.Cells[i, 9].Text.Trim() + ", ";
                            SQL = SQL + ComNum.VBLF + "   GUBUN2   = '" + ssGichoCode_Sheet1.Cells[i, 10].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "   GUBUN3   = '" + ssGichoCode_Sheet1.Cells[i, 11].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "   GUBUN4   = '" + ssGichoCode_Sheet1.Cells[i, 12].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "   GUBUN5   = '" + ssGichoCode_Sheet1.Cells[i, 13].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "   GUNUM1   = "  + ssGichoCode_Sheet1.Cells[i, 14].Text.Trim();
                            SQL = SQL + ComNum.VBLF + "   GUNUM2   = "  + ssGichoCode_Sheet1.Cells[i, 15].Text.Trim();
                            SQL = SQL + ComNum.VBLF + "   GUNUM3   = "  + ssGichoCode_Sheet1.Cells[i, 16].Text.Trim();
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                        }

                        string SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인
            
            if(Delete_Data())
            {
                GetData();
            }

            return;
        }

        private bool Delete_Data()
        {
            string SQL = string.Empty;
            int intRowAffected = 0;

            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (int i = 0; i < ssGichoCode_Sheet1.RowCount; i++)
                {
                    string strRowID = ssGichoCode_Sheet1.Cells[i, 4].Text.Trim();

                    if (ssGichoCode_Sheet1.Cells[i, 0].Text.Trim().Equals("True") && 
                        string.IsNullOrEmpty(strRowID) == false)
                    {
                        SQL = "DELETE " + ComNum.DB_PMPA + "BAS_BCODE ";
                        SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + strRowID + "' ";

                        string SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
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

                rtnVal = true;
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
            return rtnVal;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ssGichoCode_Sheet1.RowCount += 1;
            ssGichoCode_Sheet1.SetRowHeight(ssGichoCode_Sheet1.RowCount - 1, ComNum.SPDROWHT);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
