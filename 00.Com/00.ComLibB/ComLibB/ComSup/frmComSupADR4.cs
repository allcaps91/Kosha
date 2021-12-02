using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupADR4.cs
    /// Description     : 약물이상반응(ADR) 평가 보고서
    /// Author          : 이정현
    /// Create Date     : 2018-01-15
    /// <history> 
    /// 약물이상반응(ADR) 평가 보고서
    /// </history>
    /// <seealso>
    /// PSMH\drug\dradr\FrmADR4.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\dradr\dradr.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupADR4 : Form
    {
        private frmComSupADR1_1 frmComSupADR1_1Event = null;
        private frmComSupADR2_1 frmComSupADR2_1Event = null;
        private frmComSupADR3_1 frmComSupADR3_1Event = null;

        private string GstrSEQNO = "";
        private string GstrROWID = "";

        public frmComSupADR4()
        {
            InitializeComponent();
        }

        public frmComSupADR4(string strSEQNO)
        {
            InitializeComponent();

            GstrSEQNO = strSEQNO;
        }

        private void frmComSupADR4_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            btnSave.Enabled = false;
            btnDelete.Enabled = false;

            if (clsType.User.BuseCode == "044101" || VB.Left(clsType.User.BuseCode, 2) == "01")
            {
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }

            frmComSupADR1_1Event = new frmComSupADR1_1(GstrSEQNO);

            if (frmComSupADR1_1Event != null)
            {
                pSubFormToControl(frmComSupADR1_1Event, panADR1_1);
            }

            frmComSupADR2_1Event = new frmComSupADR2_1(GstrSEQNO);

            if (frmComSupADR2_1Event != null)
            {
                pSubFormToControl(frmComSupADR2_1Event, panADR2_1);
            }

            frmComSupADR3_1Event = new frmComSupADR3_1(GstrSEQNO);

            if (frmComSupADR3_1Event != null)
            {
                pSubFormToControl(frmComSupADR3_1Event, panADR3_1);
            }

            if (GstrSEQNO != "")
            {
                GstrROWID = readROWID(clsDB.DbCon, GstrSEQNO);
                dataView();
                viewSS1(GstrSEQNO);
            }
        }

        private void pSubFormToControl(Form frm, Control pControl)
        {
            frm.Owner = this;
            frm.TopLevel = false;
            this.Controls.Add(frm);
            frm.Parent = pControl;
            frm.Text = "";
            frm.ControlBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Top = 0;
            frm.Left = 0;
            frm.WindowState = FormWindowState.Normal;
            frm.Height = pControl.Height;
            frm.Width = pControl.Width;
            //frm.Dock = DockStyle.Fill;
            frm.Show();
        }

        private string readROWID(PsmhDb pDbCon, string strSEQNO)
        {
            if (ComQuery.IsJobAuth(this, "R", pDbCon) == false) return ""; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT ROWID FROM " + ComNum.DB_ERP + "DRUG_ADR4";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void dataView()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     REPORT1, REPORT2";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR4";
                SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + GstrROWID + "' ";

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
                    rdoreport1.Checked = dt.Rows[0]["REPORT1"].ToString().Trim() == "1" ? true : false;
                    rdoreport2.Checked = dt.Rows[0]["REPORT2"].ToString().Trim() == "1" ? true : false;
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

        private void viewSS1(string strSEQNO)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            ssView_Sheet1.Cells[0, 2].Text = "";
            ssView_Sheet1.Cells[0, 4].Text = "";
            ssView_Sheet1.Cells[0, 6].Text = "";

            ssView_Sheet1.Cells[1, 2].Text = "";
            ssView_Sheet1.Cells[1, 4].Text = "";
            ssView_Sheet1.Cells[1, 6].Text = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.WDATE,'YYYY-MM-DD') AS WDATE, A.WNAME, A.WBUSE, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(C.WDATE,'YYYY-MM-DD') AS ADRDATE, B.WNAME AS PHARMACY,";
                SQL = SQL + ComNum.VBLF + "     C.WNAME AS DOCTOR";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR1 A,";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_ERP + "DRUG_ADR2 B,";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_ERP + "DRUG_ADR3 C,";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_ERP + "DRUG_ADR4 D";
                SQL = SQL + ComNum.VBLF + "     WHERE A.SEQNO = B.SEQNO(+)";
                SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = C.SEQNO(+)";
                SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = D.SEQNO(+)";
                SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = " + strSEQNO;

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
                    ssView_Sheet1.Cells[0, 2].Text = dt.Rows[0]["WDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[0, 4].Text = dt.Rows[0]["WBUSE"].ToString().Trim();
                    ssView_Sheet1.Cells[0, 6].Text = dt.Rows[0]["WNAME"].ToString().Trim();

                    ssView_Sheet1.Cells[1, 2].Text = dt.Rows[0]["ADRDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[1, 4].Text = dt.Rows[0]["PHARMACY"].ToString().Trim();
                    ssView_Sheet1.Cells[1, 6].Text = dt.Rows[0]["DOCTOR"].ToString().Trim();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SAVE_DATA() == true) { }
        }

        private bool SAVE_DATA(string strGBN = "")
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strROWID = "";
            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (GstrSEQNO == "")
                {
                    GstrSEQNO = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_ERP.Replace(".", ""), "SEQ_ADR").ToString();
                }
                else
                {
                    strROWID = readROWID(clsDB.DbCon, GstrSEQNO);

                    if (strROWID != "")
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR4_HISTORY";
                        SQL = SQL + ComNum.VBLF + "     SELECT * FROM " + ComNum.DB_ERP + "DRUG_ADR4";
                        SQL = SQL + ComNum.VBLF + "         WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        SQL = "";
                        SQL = "DELETE " + ComNum.DB_ERP + "DRUG_ADR4";
                        SQL = SQL + ComNum.VBLF + "         WHERE ROWID = '" + strROWID + "' ";

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

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR4";
                SQL = SQL + ComNum.VBLF + "     (SEQNO, REPORT1, REPORT2, WRITEDATE, WRITESABUN)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         " + GstrSEQNO + ", ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoreport1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoreport2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "         '" + clsType.User.Sabun + "' ";
                SQL = SQL + ComNum.VBLF + "     )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (GstrSEQNO == "")
            {
                ComFunc.MsgBox("저장 된 내용이 없습니다.");
            }
            else
            {
                if (ComFunc.MsgBoxQ("작성 된 내용을 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (DEL_DATA() == true)
                    {
                        this.Close();
                    }
                }
            }
        }

        private bool DEL_DATA()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strROWID = readROWID(clsDB.DbCon, GstrSEQNO);

            if (strROWID == "") { return rtnVal; }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_ADR4";
                SQL = SQL + ComNum.VBLF + "         WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
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
    }
}
