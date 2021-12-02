using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// EMRLIST 사용자 상용구 관리
    /// </summary>
    public partial class frmEmrListMacro : Form
    {
        public delegate void SetSendData(bool Refresh);
        public event SetSendData rSetSendData;

        public frmEmrListMacro()
        {
            InitializeComponent();
        }

        private void frmEmrListMacro_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            GetSearchData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetSearchData();
        }

        private void GetSearchData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            SS1_Sheet1.RowCount = 0;

            string SQL = string.Empty;
            OracleDataReader reader = null;

            SQL = " SELECT CODE, NAME";
            SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE";
            SQL += ComNum.VBLF + " WHERE GUBUN = 'EMR_LIST_사용자_상용구'";

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
            if (SqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                return;
            }

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    SS1_Sheet1.RowCount += 1;

                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 1].Text = reader.GetValue(0).ToString().Trim();
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 2].Text = reader.GetValue(1).ToString().Trim();
                }

                SS1_Sheet1.RowCount += 5;
            }
            else
            {
                SS1_Sheet1.RowCount = 5;
            }
            reader.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if (SaveData() == true)
            {
                GetSearchData();
                rSetSendData(true);
            }
        }

        private bool SaveData()
        {
            #region 변수
            bool rtnVal = false;
            string SQL = string.Empty;
            string sqlErr = string.Empty;
            int RowAffected = 0;
            #endregion


            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = " DELETE KOSMOS_PMPA.BAS_BCODE";
                SQL += ComNum.VBLF + "WHERE GUBUN = 'EMR_LIST_사용자_상용구'";

                sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }

                #region sshis
                for (int i = 0; i < SS1_Sheet1.RowCount; i++)
                {

                    string strCODE = (i).ToString("00");
                    string strNAME = SS1_Sheet1.Cells[i, 2].Text.Trim();

                    if (string.IsNullOrWhiteSpace(strNAME))
                        continue;

                    SQL = " INSERT INTO KOSMOS_PMPA.BAS_BCODE(";
                    SQL += ComNum.VBLF + " GUBUN, CODE, NAME, ENTDATE, ENTSABUN, SORT) VALUES (";
                    SQL += ComNum.VBLF + "'EMR_LIST_사용자_상용구','" + strCODE + "','" + strNAME + "', SYSDATE, " + clsType.User.IdNumber + ", " + i + ")";

                    sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, sqlErr);
                        return rtnVal;
                    }
                }
                #endregion


                rtnVal = true;
                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }

            return rtnVal;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인


            if (DeleteData() == true)
            {
                GetSearchData();
                rSetSendData(true);
            }
        }

        private bool DeleteData()
        {
            #region 변수
            bool rtnVal = false;
            string SQL = string.Empty;
            string sqlErr = string.Empty;
            int RowAffected = 0;
            #endregion


            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                #region sshis
                for (int i = 0; i < SS1_Sheet1.RowCount; i++)
                {
                    if (SS1_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                    {
                        SQL = " DELETE KOSMOS_PMPA.BAS_BCODE";
                        SQL += ComNum.VBLF + " WHERE GUBUN = 'EMR_LIST_사용자_상용구' ";
                        SQL += ComNum.VBLF + "   AND CODE  = '" + SS1_Sheet1.Cells[i, 1].Text.Trim() + "'";

                        sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                        if (sqlErr.Length > 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, sqlErr);
                            return rtnVal;
                        }
                    }
                }
                #endregion


                rtnVal = true;
                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }

            return rtnVal;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SS1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SS1_Sheet1.RowCount == 0)
                return;

            if (e.ColumnHeader)
            {
                clsSpread.gSpdSortRow(SS1, e.Column);
            }
        }
    }
}
