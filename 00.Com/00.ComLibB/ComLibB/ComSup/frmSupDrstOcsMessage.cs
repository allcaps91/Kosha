using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstOcsMessage.cs
    /// Description     : OCS 전달사항 등록
    /// Author          : 이정현
    /// Create Date     : 2017-12-04
    /// <history> 
    /// OCS 전달사항 등록
    /// </history>
    /// <seealso>
    /// PSMH\drug\drinfo\FrmOcsMessage.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drinfo\drinfo.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstOcsMessage : Form
    {
        private string GstrMsgROWID = "";

        public frmSupDrstOcsMessage()
        {
            InitializeComponent();
        }

        private void frmSupDrstOcsMessage_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            txtInfo.Text = "";

            GetMessage();
        }

        private void GetMessage()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     ROWID, Remark";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG_MESSAGE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    GstrMsgROWID = "";
                    txtInfo.Text = "";

                    btnDelete.Enabled = false;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                GstrMsgROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                txtInfo.Text = dt.Rows[0]["REMARK"].ToString().Trim();

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

            if (VB.Len(txtInfo.Text.Trim()) > 2000)
            {
                ComFunc.MsgBox("약국전달사항은 최대 2000Byte만 가능합니다.");
                txtInfo.Focus();
                return;
            }

            txtInfo.Text = txtInfo.Text.Replace("'", "`").Replace("·", ".");

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (GstrMsgROWID != "")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG_MESSAGE_HIS";
                    SQL = SQL + ComNum.VBLF + "     SELECT * FROM " + ComNum.DB_MED + "OCS_DRUG_MESSAGE";
                    SQL = SQL + ComNum.VBLF + "         WHERE ROWID = '" + GstrMsgROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_MED + "OCS_DRUG_MESSAGE";
                    SQL = SQL + ComNum.VBLF + "     SET";
                    SQL = SQL + ComNum.VBLF + "         InDate = TRUNC(SYSDATE), ";
                    SQL = SQL + ComNum.VBLF + "         Remark = '" + txtInfo.Text.Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + GstrMsgROWID + "' ";
                }
                else
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG_MESSAGE";
                    SQL = SQL + ComNum.VBLF + "     (InDate, Remark)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         TRUNC(SYSDATE), ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtInfo.Text.Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                this.Close();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 기능 삭제
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG_MESSAGE_HIS";
                SQL = SQL + ComNum.VBLF + "     SELECT * FROM " + ComNum.DB_MED + "OCS_DRUG_MESSAGE";
                SQL = SQL + ComNum.VBLF + "         WHERE ROWID = '" + GstrMsgROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = "DELETE " + ComNum.DB_MED + "OCS_DRUG_MESSAGE ";
                SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + GstrMsgROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                this.Close();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion
    }
}
