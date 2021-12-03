using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// 간호진단 코드 - Result
    /// </summary>
    public partial class frmEmrNurJinResult : Form
    {

        string GstrHelpCode = string.Empty;

        /// <summary>
        /// 이것만 사용하세요.
        /// </summary>
        /// <param name="strHelpCode"></param>
        public frmEmrNurJinResult(string strHelpCode)
        {
            GstrHelpCode = strHelpCode;
            InitializeComponent();
        }

        private void frmEmrNurJinResult_Load(object sender, EventArgs e)
        {
            GetSearchData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetSearchData();
        }

        void GetSearchData()
        {
            SS6_Sheet1.RowCount = 0;
            clsEmrPublic.GstrNurResult.Clear();
            string strCode = string.Empty;

            #region 쿼리
            OracleDataReader reader = null;
            string SQL = " SELECT CODE ";
            SQL += ComNum.VBLF + " FROM ADMIN.NUR_CODE_DAR ";
            SQL += ComNum.VBLF + " WHERE CODE = '" + GstrHelpCode + "'";
            SQL += ComNum.VBLF + "    AND GUBUN = '3' ";

            string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, sqlErr);
                return;
            }

            if (reader.HasRows && reader.Read())
            {
                strCode = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();
            #endregion

            if (string.IsNullOrWhiteSpace(strCode))
                return;

            READ_JIN6(strCode);
        }

        private void READ_JIN6(string Code)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            SS6_Sheet1.RowCount = 0;
            clsEmrPublic.GstrNurResult.Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {


                SQL = " SELECT CODE, NAME ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.NUR_CODE_DAR";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'D'";
                SQL = SQL + ComNum.VBLF + "    AND CODE LIKE '" + Code + "%'";
                SQL = SQL + ComNum.VBLF + "    AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "  ORDER BY CODE ";

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
                    SS6_Sheet1.RowCount = dt.Rows.Count;
                    SS6_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS6_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        SS6_Sheet1.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        SS6_Sheet1.Rows[i].Height = SS6_Sheet1.Rows[i].GetPreferredHeight() + 5;
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
                }

                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            clsEmrPublic.GstrNurResult.Clear();

            for(int i = 0; i < SS6_Sheet1.RowCount; i++)
            {
                if (SS6_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                {
                    if (SS6_Sheet1.Cells[i, 2].Text.Trim().Length > 0)
                    {
                        clsEmrPublic.GstrNurResult.Add(SS6_Sheet1.Cells[i, 2].Text.Trim());
                    }
                }
            }

            if (clsEmrPublic.GstrNurResult.Count == 0)
            {
                ComFunc.MsgBoxEx(this, "Result를 선택하여 주십시요.");
                return;
            }

            clsEmrPublic.GstrNurCodeDAR = GstrHelpCode;
            Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
