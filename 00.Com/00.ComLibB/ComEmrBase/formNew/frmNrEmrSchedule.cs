using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmNrEmrSchedule : Form
    {
        public frmNrEmrSchedule()
        {
            InitializeComponent();
        }

        private void frmNrEmrSchedule_Load(object sender, EventArgs e)
        {
            if (clsVbfunc.GetBCodeCODE(clsDB.DbCon, "NUR_간호부관리자사번IP", "").Trim().Equals(clsCompuInfo.gstrCOMIP) == false)
            {
                if(IsLeader() == false)
                {
                    ComFunc.MsgBoxEx(this, "간호부 당직 컴퓨터가 아닙니다.");
                    return;
                }
            }

            GetSearchData();
        }

        /// <summary>
        /// 간호부 부장, 팀장 여부
        /// </summary>
        bool IsLeader()
        {
            bool rtnVal = false;
            ssView_Sheet1.RowCount = 0;

            OracleDataReader dataReader = null;

            string SQL = "SELECT KORNAME";
            SQL += ComNum.VBLF + "FROM ADMIN.INSA_MST";
            SQL += ComNum.VBLF + "WHERE SABUN = '" + clsType.User.Sabun + "'";
            SQL += ComNum.VBLF + "  AND JIK IN ('04', '13')";

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }


                if (dataReader.HasRows)
                {
                    rtnVal = true;
                }

                dataReader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 등록된 챠팅 예외사번 가져오기
        /// </summary>
        void GetSearchData()
        {
            ssView_Sheet1.RowCount = 0;

            OracleDataReader dataReader = null;

            string SQL = "SELECT BASCD, B.KORNAME, C.NAME";
            SQL += ComNum.VBLF + "FROM ADMIN.BAS_BASCD A";
            SQL += ComNum.VBLF + "  INNER JOIN ADMIN.INSA_MST B";
            SQL += ComNum.VBLF + "     ON A.BASCD = TRIM(B.SABUN)";
            SQL += ComNum.VBLF + "  INNER JOIN ADMIN.BAS_BUSE C";
            SQL += ComNum.VBLF + "     ON B.BUSE = C.BUCODE";
            SQL += ComNum.VBLF + "WHERE GRPCD = '예외 사번'";
            SQL += ComNum.VBLF + "  AND USECLS = '1'";
            SQL += ComNum.VBLF + "ORDER BY DISPSEQ";

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }


                if(dataReader.HasRows)
                {
                    while(dataReader.Read())
                    {
                        ssView_Sheet1.RowCount += 1;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dataReader.GetValue(0).ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dataReader.GetValue(1).ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dataReader.GetValue(2).ToString().Trim();
                    }
                }

                ssView_Sheet1.RowCount += 5;
                dataReader.Dispose();

            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            int RowAffected = 0;
            OracleDataReader dataReader = null;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for(int i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (ssView_Sheet1.Cells[i, 0].Text.Trim().Equals("False") || ssView_Sheet1.Cells[i, 0].Text.Trim().Length == 0)
                        continue;

                    if (ssView_Sheet1.Cells[i, 1].Text.Trim().Length == 0)
                        continue;

                    bool bNew = false;

                    SQL = "SELECT BASCD";
                    SQL += ComNum.VBLF + "FROM ADMIN.BAS_BASCD";
                    SQL += ComNum.VBLF + "WHERE GRPCDB = '간호EMR 관리'";
                    SQL += ComNum.VBLF + "  AND GRPCD  = '예외 사번'";
                    SQL += ComNum.VBLF + "  AND BASCD  = '" + ssView_Sheet1.Cells[i, 1].Text.Trim() + "'";

                    string sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        ComFunc.MsgBoxEx(this, sqlErr);
                        return;
                    }

                    if (dataReader.HasRows)
                    {
                        bNew = true;
                    }

                    dataReader.Dispose();

                    //이미 사번 등록되어있으면 넘어감
                    if (bNew)
                        continue;

                    SQL = "INSERT INTO ADMIN.BAS_BASCD";
                    SQL += ComNum.VBLF + "(";
                    SQL += ComNum.VBLF + "GRPCDB, GRPCD, BASCD, APLFRDATE, APLENDDATE";
                    SQL += ComNum.VBLF + ",INPDATE, INPTIME";
                    SQL += ComNum.VBLF + ",USECLS , DISPSEQ";
                    SQL += ComNum.VBLF + ")";
                    
                    SQL += ComNum.VBLF + "VALUES";
                    SQL += ComNum.VBLF + "(";
                    SQL += ComNum.VBLF + "'간호EMR 관리', '예외 사번',                        --GRPCDB, GRPCD, ";
                    SQL += ComNum.VBLF + "'" + ssView_Sheet1.Cells[i, 1].Text.Trim() + "', --BASCD";
                    SQL += ComNum.VBLF + "TO_CHAR(SYSDATE, 'YYYYMMDD'),                    --APLFRDATE";
                    SQL += ComNum.VBLF + "'99981231',                                      --APLENDDATE";
                    SQL += ComNum.VBLF + "TO_CHAR(SYSDATE, 'YYYYMMDD'),                    --INPDATE";
                    SQL += ComNum.VBLF + "TO_CHAR(SYSDATE, 'HH24MISS'),                    --INPTIME";
                    SQL += ComNum.VBLF + "'1', " + i;
                    SQL += ComNum.VBLF + ")";

                    sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                    if(sqlErr.Length > 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, sqlErr);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            GetSearchData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (int i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (ssView_Sheet1.Cells[i, 0].Text.Trim().Equals("False"))
                        continue;

                    SQL = "DELETE ADMIN.BAS_BASCD";
                    SQL += ComNum.VBLF + "WHERE BASCD = '" + ssView_Sheet1.Cells[i, 1].Text.Trim() + "'";

                    string sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, sqlErr);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            GetSearchData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_EditModeOff(object sender, EventArgs e)
        {
            //사번 칼럼아니면 빠져나감
            if (ssView_Sheet1.ActiveColumnIndex != 1)
                return;

            string strSabun = ComFunc.SetAutoZero(ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 1].Text.Trim(), 5);
            ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 1].Text = strSabun;
            ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 2].Text = clsVbfunc.GetInSaName(clsDB.DbCon, strSabun);
            ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 3].Text = clsVbfunc.GetSaBunBuSeName(clsDB.DbCon, strSabun);
        }
    }
}
