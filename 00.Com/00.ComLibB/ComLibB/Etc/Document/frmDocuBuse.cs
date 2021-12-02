using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmDocuBuse : Form, MainFormMessage
    {
        #region //MainFormMessage
        public string mPara1 = "";
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {
            mPara1 = strPara;
        }
        #endregion //MainFormMessage

        public frmDocuBuse()
        {
            InitializeComponent();
        }

        public frmDocuBuse(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmDocuBuse(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
        }


        private void frmDocuBuse_Load(object sender, EventArgs e)
        {
            Screen_Clear();
            GetData();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (LblBUSE.Text == "")
            {
                ComFunc.MsgBox("부서명을 확인하시기 바랍니다.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "INSERT INTO KOSMOS_ADM.INSA_DOCU_BUSE (BUSE) VALUES (";
                SQL = SQL + ComNum.VBLF + "'" + LblBUSE.Text.Trim() + "')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Screen_Clear();
                GetData();
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

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strOK = string.Empty;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            strOK = "OK";

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < SsView_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(SsView_Sheet1.Cells[i, 0].Value) == true)
                    {
                        SQL = "DELETE FROM KOSMOS_ADM.INSA_DOCU_BUSE";
                        SQL = SQL + " WHERE BUSE = '" + SsView_Sheet1.Cells[i, 1].Text.Trim() + "'";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            strOK = "NO";
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                if (strOK != "OK")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("삭제 중 에러 발생");
                }
                else
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("저장하였습니다.");
                }

                Cursor.Current = Cursors.Default;
                GetData();
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

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GetData()
        {
            DataTable dt = null;
            int i = 0;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SsView_Sheet1.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = string.Empty;
                SQL = "SELECT BUSE, DELDATE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU_BUSE";
                SQL = SQL + ComNum.VBLF + " ORDER BY RANKING ASC, BUSE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    SsView_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SsView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BUSE"].ToString().Trim();
                        SsView_Sheet1.Cells[i, 2].Text = clsVbfunc.READ_BAS_BUSE(clsDB.DbCon, dt.Rows[i]["BUSE"].ToString().Trim());
                        SsView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DELDATE"].ToString().Trim();
                    }
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

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Screen_Clear()
        {
            TxtBuse.Text = string.Empty;
            LblBUSE.Text = string.Empty;
        }

        private void TxtBuse_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            LblBUSE.Text = string.Empty;

            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (TxtBuse.Text.Trim() == "")
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = string.Empty;
                SQL = "SELECT BUCODE FROM KOSMOS_PMPA.BAS_BUSE";
                SQL = SQL + ComNum.VBLF + " WHERE NAME LIKE '%" + TxtBuse.Text.Trim() + "%'";
                SQL = SQL + ComNum.VBLF + "   AND BUCODE < '090000' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY BUCODE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    LblBUSE.Text = dt.Rows[0]["BUCODE"].ToString().Trim();
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

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void frmDocuBuse_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmDocuBuse_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }
    }
}
