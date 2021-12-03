using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmToiSabun_Cert
    /// Description     : FrmToiSabun_Cert
    /// Author          : 이현종
    /// Create Date     : 2019-08-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= " PSMH\mtsEmr(frmToiSabun_Cert.frm) >> frmToiSabun_Cert.cs 폼이름 재정의" />
    /// 
    public partial class frmToiSabun_Cert : Form
    {
        public delegate void CloseEvent();
        public event CloseEvent rClosed;

        public frmToiSabun_Cert()
        {
            InitializeComponent();
        }

        private void FrmToiSabun_Cert_Load(object sender, EventArgs e)
        {
            lblMsg.Text = clsEmrFunc.TOISABUN_CERT(clsDB.DbCon) ? "허용" : "차단";

            cboSET.Items.Clear();
            cboSET.Items.Add("차단");
            cboSET.Items.Add("허용");
            cboSET.SelectedIndex = -1;

            GetSearhData();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            GetSearhData();
        }


        void GetSearhData()
        {

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            SS1_Sheet1.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;
            try
            {

                SQL = " SELECT A.SABUN, TO_CHAR(A.WRITEDATE, 'YYYY-MM-DD') WRITEDATE, A.WRITESABUN, B.KORNAME, C.NAME, TO_CHAR(B.TOIDAY, 'YYYY-MM-DD') TOIDAY";
                SQL += ComNum.VBLF + " FROM ADMIN.EMRXML_MOCIFY_CERT_B A";
                SQL += ComNum.VBLF + "   INNER JOIN ADMIN.INSA_MST B";
                SQL += ComNum.VBLF + "      ON A.SABUN = B.SABUN";
                SQL += ComNum.VBLF + "   INNER JOIN ADMIN.BAS_BUSE C";
                SQL += ComNum.VBLF + "      ON C.BUCODE = B.BUSE";
                SQL += ComNum.VBLF + " ORDER BY B.KORNAME ASC ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.RowCount = dt.Rows.Count;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["TOIDAY"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["WRITEDATE"].ToString().Trim();
                    }
                }

                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnSaveAdd_Click(object sender, EventArgs e)
        {
            if(lblKorName.Text.Length == 0)
            {
                ComFunc.MsgBoxEx(this, "사번을 확인하세요");
                return;
            }
            SaveData();
        }
            
        bool SaveData()
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {

                SQL = " INSERT INTO ADMIN.EMRXML_MOCIFY_CERT_HISTORY(";
                SQL += ComNum.VBLF + " SABUN, WRITEDATE, WRITESABUN, GUBUN ) VALUES (";
                SQL += ComNum.VBLF + txtSabun.Text.Trim() + ", SYSDATE, " + clsType.User.IdNumber + ",'I')";

                string sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }

                SQL = " INSERT INTO ADMIN.EMRXML_MOCIFY_CERT_B(";
                SQL += ComNum.VBLF + " SABUN, WRITEDATE, WRITESABUN ) VALUES (";
                SQL += ComNum.VBLF + txtSabun.Text.Trim() + ", SYSDATE, " + clsType.User.IdNumber + ")";

                sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                GetSearhData();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

        bool DeleteData()
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {

                for (int i = 0; i < SS1_Sheet1.RowCount; i++)
                {
                    string strSabun = ComFunc.SetAutoZero(SS1_Sheet1.Cells[i, 3].Text.Trim(), 5);

                    if(SS1_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                    {
                        SQL = " INSERT INTO ADMIN.EMRXML_MOCIFY_CERT_HISTORY(";
                        SQL += ComNum.VBLF + " SABUN, WRITEDATE, WRITESABUN, GUBUN ) VALUES (";
                        SQL += ComNum.VBLF + strSabun + ", SYSDATE, " + clsType.User.IdNumber + ",'D')";

                        string sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                        if (sqlErr.Length > 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, sqlErr);
                            return rtnVal;
                        }

                        SQL = " DELETE ADMIN.EMRXML_MOCIFY_CERT_B ";
                        SQL += ComNum.VBLF + " WHERE SABUN = '" + strSabun + "'";

                        sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                        if (sqlErr.Length > 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, sqlErr);
                            return rtnVal;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                GetSearhData();

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        private void BtnSaveSet_Click(object sender, EventArgs e)
        {
            if (cboSET.Text.Trim().Length == 0)
                return;


            SaveSet();
        }

        bool SaveSet()
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = " INSERT INTO ADMIN.EMRXML_MOCIFY_CERT_HISTORY(CERT, WRITEDATE, WRITESABUN, DELDATE, DELSABUN)";
                SQL += ComNum.VBLF + "SELECT CERT, WRITEDATE, WRITESABUN, SYSDATE, " + clsType.User.IdNumber;
                SQL += ComNum.VBLF + "  FROM ADMIN.EMRXML_MOCIFY_CERT";

                string sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }

                if(cboSET.Text.Trim().Equals("허용"))
                {
                    SQL = " INSERT INTO ADMIN.EMRXML_MOCIFY_CERT(";
                    SQL += ComNum.VBLF + " CERT, WRITEDATE, WRITESABUN) VALUES ( ";
                    SQL += ComNum.VBLF + " '1',  SYSDATE, " + clsType.User.IdNumber + ")";
                }
                else
                {
                    SQL = " DELETE ADMIN.EMRXML_MOCIFY_CERT ";
                }


                sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                lblMsg.Text = clsEmrFunc.TOISABUN_CERT(clsDB.DbCon) ? "허용" : "차단";

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (rClosed == null)
            {
                Close();
            }
            else
            {
                rClosed();
            }
        }

        private void Frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (rClosed == null)
            {
                Close();
            }
            else
            {
                rClosed();
            }
        }

        private void txtSabun_Leave(object sender, EventArgs e)
        {
            lblKorName.Text = clsVbfunc.GetInSaName(clsDB.DbCon, txtSabun.Text);
        }

        private void txtSabun_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSaveAdd.Focus();
            }
        }
    }
}
