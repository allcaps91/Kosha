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

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaJengSanSubmitExceptPat.cs
    /// Description     : 제출 제외 환자 리스트
    /// Author          : 이정현
    /// Create Date     : 2018-08-16
    /// <history> 
    /// 제출 제외 환자 리스트
    /// </history>
    /// <seealso>
    /// PSMH\OPD\jengsan\Frm제출제외환자.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\OPD\jengsan\jengsan.vbp
    /// </vbp>
    /// </summary>
    public partial class frmPmpaJengSanSubmitExceptPat : Form
    {
        public frmPmpaJengSanSubmitExceptPat()
        {
            InitializeComponent();
        }

        private void frmPmpaJengSanSubmitExceptPat_Load(object sender, EventArgs e)
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
            ssView_Sheet1.RowCount = 50;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
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
            ssView_Sheet1.RowCount = 30;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, B.SNAME, A.JUMIN1, A.JUMIN2, A.JUMIN_NEW, A.REMARK, A.ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_JUNREMARK A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "         AND A.YEAR = '" + dtpYear.Value.Year + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PANO ";

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
                    ssView_Sheet1.RowCount = 0;
                    ssView_Sheet1.RowCount = dt.Rows.Count + 30;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["JUMIN1"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = clsAES.DeAES(dt.Rows[i]["JUMIN_NEW"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
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
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strPANO = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strRemark = "";
            string strRowId = "";
            string strYear = dtpYear.Value.Year.ToString();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.NonEmptyRowCount; i++)
                {
                    strPANO = ssView_Sheet1.Cells[i, 0].Text.Trim();
                    strJumin1 = ssView_Sheet1.Cells[i, 2].Text.Trim();
                    strJumin2 = ssView_Sheet1.Cells[i, 3].Text.Trim();
                    strRemark = ssView_Sheet1.Cells[i, 4].Text.Trim();
                    strRowId = ssView_Sheet1.Cells[i, 5].Text.Trim();

                    if (strRowId == "")
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_JUNREMARK";
                        SQL = SQL + ComNum.VBLF + "     (PANO, JUMIN1, JUMIN2, REMARK, ENTDATE, YEAR, JUMIN_NEW)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         '" + strPANO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strJumin1 + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strJumin2 + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strRemark + "', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strYear + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + clsAES.AES(strJumin2) + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";
                    }
                    else
                    {
                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_PMPA + "ETC_JUNREMARK";
                        SQL = SQL + ComNum.VBLF + "     SET";
                        SQL = SQL + ComNum.VBLF + "         JUMIN1 = '" + strJumin1 + "', ";
                        SQL = SQL + ComNum.VBLF + "         JUMIN2 = '" + strJumin2 + "', ";
                        SQL = SQL + ComNum.VBLF + "         JUMIN_NEW = '" + clsAES.AES(strJumin2) + "', ";
                        SQL = SQL + ComNum.VBLF + "         REMARK = '" + strRemark + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strRowId + "' ";
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
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

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

            string strRowid = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.NonEmptyRowCount; i++)
                {
                    if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 6].Value) == true)
                    {
                        strRowid = ssView_Sheet1.Cells[i, 5].Text.Trim();

                        SQL = "";
                        SQL = "DELETE " + ComNum.DB_PMPA + "ETC_JUNREMARK ";
                        SQL = SQL + ComNum.VBLF + "     WHERE ROWID  = '" + strRowid + "' ";

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

        private void ssView_EditModeOff(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ssView_Sheet1.ActiveColumnIndex != 0) { return; }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strPANO = "";

            ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 0].Text = ComFunc.LPAD(ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 0].Text, 8, "0");
            strPANO = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 0].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SNAME, JUMIN1, JUMIN2, JUMIN3";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPANO + "' ";

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
                    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 1].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 2].Text = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 3].Text = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
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
    }
}
