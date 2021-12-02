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

namespace ComNurLibB
{
    public partial class frmPicUpCaution : Form
    {
        public frmPicUpCaution()
        {
            InitializeComponent();
        }

        private void frmPicUpCaution_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtOrdercode.Text = "";
            chkMy.Checked = false;

            ssView_Sheet1.RowCount = 0;

            btnSearch_Click(null, null);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strCODE = txtOrdercode.Text.Trim();

            ssView_Sheet1.RowCount = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "";
                SQL = " SELECT ORDERCODE, ORDERNAME, MSG, TO_CHAR(WRITEDATE,'YYYY-MM-DD') WRITEDATE, WRITESABUN, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_PICKUP_MSG ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1 ";

                if (strCODE.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND (ORDERCODE LIKE '%" + strCODE + "%' OR ORDERNAME LIKE '%" + strCODE + "%')  ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND WRITESABUN = " + clsType.User.Sabun;
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["MSG"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["WRITEDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["WRITESABUN"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 6].Text = "";
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSearch1_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strCODE = "";

            strCODE = VB.InputBox("오더코드를 입력하여 주십시요.(수가코드가 아닌 오더코드)", "오더코드 입력");
            strCODE = strCODE.ToUpper();

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = " SELECT ORDERNAME ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_ORDERCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE ORDERCODE = '" + strCODE + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = strCODE;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[0]["ORDERNAME"].ToString().Trim();
                }
                else
                {
                    ComFunc.MsgBox("해당 오더코드는 없는 코드입니다.", "확인");
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strCODE = "";
            string strName = "";
            string strMsg = "";

            string strROWID = "";
            string strChange = "";

            Cursor.Current = Cursors.WaitCursor;
            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strCODE = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    strName = ssView_Sheet1.Cells[i, 2].Text.Trim();
                    strMsg = ssView_Sheet1.Cells[i, 3].Text.Trim();
                    strChange = ssView_Sheet1.Cells[i, 6].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i, 7].Text.Trim();

                    if (strChange != "")
                    {
                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = " INSERT INTO KOSMOS_PMPA.NUR_PICKUP_MSG(";
                            SQL = SQL + ComNum.VBLF + " ORDERCODE, ORDERNAME, MSG, WRITEDATE, WRITESABUN) VALUES (";
                            SQL = SQL + ComNum.VBLF + " '" + strCODE + "','" + strName + "','" + strMsg + "', SYSDATE, " + clsType.User.Sabun + ")";
                        }
                        else
                        {
                            SQL = "";
                            SQL = " UPDATE KOSMOS_PMPA.NUR_PICKUP_MSG SET ";
                            SQL = SQL + ComNum.VBLF + " MSG = '" + strMsg + "'";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                        }
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                btnSearch_Click(null, null);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strROWID = "";

            Cursor.Current = Cursors.WaitCursor;
            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strROWID = ssView_Sheet1.Cells[i, 7].Text.Trim();

                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = " INSERT INTO KOSMOS_PMPA.NUR_PICKUP_MSG_HISTORY( ";
                            SQL = SQL + ComNum.VBLF + " ORDERCODE, ORDERNAME, MSG, WRITEDATE, WRITESABUN, DELDATE, DELSABUN) ";
                            SQL = SQL + ComNum.VBLF + " SELECT ORDERCODE, ORDERNAME, MSG, WRITEDATE, WRITESABUN, SYSDATE, " + clsType.User.Sabun;
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_PICKUP_MSG ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            SQL = "";
                            SQL = " DELETE KOSMOS_PMPA.NUR_PICKUP_MSG ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
