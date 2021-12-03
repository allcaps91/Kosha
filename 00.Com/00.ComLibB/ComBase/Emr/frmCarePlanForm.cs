using System;
using System.Data;
using System.Windows.Forms;

namespace ComBase
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-05-03
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\mtsEmr\CarePlan\FrmCarePlanForm" >> frmSupLbExSTS15.cs 폼이름 재정의" />

    public partial class frmCarePlanForm : Form
    {
        public frmCarePlanForm()
        {
            InitializeComponent();
        }

        private void frmCarePlanForm_Load(object sender, EventArgs e)
        {

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            View();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            View();
        }

        private void View()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            SS1_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = " SELECT A.FORMNO, A.UPDATENO, PART01, PART01_2, PART02, PART02_2, B.FORMNAME, A.ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMR_CAREPLAN_CHART A, " + ComNum.DB_EMR + "AEMRFORM B";
                SQL = SQL + ComNum.VBLF + " WHERE A.GUBUN = '1' ";
                SQL = SQL + ComNum.VBLF + "   AND A.FORMNO = B.FORMNO ";
                SQL = SQL + ComNum.VBLF + "   AND A.UPDATENO = B.UPDATENO ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY A.FORMNO ASC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.RowCount = dt.Rows.Count + 5;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["UPDATENO"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PART01"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["PART01_2"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["PART02"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["PART02_2"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }
                else
                {
                    SS1_Sheet1.RowCount = dt.Rows.Count + 5;
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (Save() == false)
            {
                return;
            }
            View();
        }

        private bool Save()
        {
            string strFormNo = "";
            string strUpdateNo = "";
            string strPART01 = "";
            string strPART02 = "";
            string strPART01_2 = "";
            string strPART02_2 = "";
            string strROWID = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 1; i <= SS1_Sheet1.RowCount; i++)
                {
                    strFormNo = SS1_Sheet1.Cells[i - 1, 1].Text.Trim();
                    strUpdateNo = SS1_Sheet1.Cells[i - 1, 2].Text.Trim();
                    strROWID = SS1_Sheet1.Cells[i - 1, 8].Text.Trim();

                    if (strFormNo != "")
                    {
                        strPART01 = SS1_Sheet1.Cells[i - 1, 4].Text.Trim();
                        strPART01_2 = SS1_Sheet1.Cells[i - 1, 5].Text.Trim();
                        strPART02 = SS1_Sheet1.Cells[i - 1, 6].Text.Trim();
                        strPART02_2 = SS1_Sheet1.Cells[i - 1, 7].Text.Trim();

                        if (strROWID == "")
                        {
                            SQL = "";
                            SQL = " INSERT INTO " + ComNum.DB_EMR + "EMR_CAREPLAN_CHART(";
                            SQL = SQL + ComNum.VBLF + " GUBUN, FORMNO, UPDATENO, PART01, PART01_2, PART02, PART02_2) VALUES ( ";
                            SQL = SQL + ComNum.VBLF + "'1'," + strFormNo + ", " + strUpdateNo + ", '" + strPART01 + "','" + strPART02 + "','" + strPART01_2 + "','" + strPART02_2 + "')";

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
                        else
                        {
                            SQL = "";
                            SQL = " UPDATE " + ComNum.DB_EMR + "EMR_CAREPLAN_CHART SET ";
                            SQL = SQL + ComNum.VBLF + " PART01 = '" + strPART01 + "',";
                            SQL = SQL + ComNum.VBLF + " PART01_2 = '" + strPART01_2 + "',";
                            SQL = SQL + ComNum.VBLF + " PART02 = '" + strPART02 + "',";
                            SQL = SQL + ComNum.VBLF + " PART02_2 = '" + strPART02_2 + "'";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            if (Del() == false)
            {
                return;
            }
            View();
        }

        private bool Del()
        {
            string strROWID = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            if (ComFunc.MsgBoxQ("삭제 후 복구 불가능합니다. 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return rtnVal;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 1; i <= SS1_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, 0].Value) == true)
                    {
                        strROWID = SS1_Sheet1.Cells[i - 1, 8].Text.Trim();

                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = " DELETE KOSMOS_EMR.EMR_CAREPLAN_CHART ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

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
    }
}
