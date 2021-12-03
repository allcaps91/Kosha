using System;
using System.Windows.Forms;
using ComBase;
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    /// <summary>
    /// 간호 EMR 참고사항
    /// frmBIGO.frm
    /// </summary>
    public partial class frmBigo : Form
    {
        string GstrHelpCode = string.Empty;
        

        /// <summary>
        /// 이거 사용하세요.
        /// </summary>
        /// <param name="strHelpCode">GstrHelpCode</param>
        public frmBigo(string strHelpCode)
        {
            GstrHelpCode = strHelpCode;
            InitializeComponent();
        }


        private void FrmBigo_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등


            btnSave.Enabled = NURSE_System_Manager_Check(clsType.User.IdNumber);

            if(VB.IsNumeric(GstrHelpCode) == false)
            {
                GstrHelpCode = SET_FORMNO(GstrHelpCode);
            }
            else
            {
                GetFormName();
            }

            READ_BIGO(GstrHelpCode);
        }

        /// <summary>
        /// 관리자 권한 체크
        /// </summary>
        /// <param name="sabun"></param>
        /// <returns></returns>
        bool NURSE_System_Manager_Check(string sabun)
        {
            bool rtnVal = false;

            OracleDataReader reader = null;

            string SQL = "SELECT CODE  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL += ComNum.VBLF + "WHERE GUBUN = 'NUR_시스템관리자'";
            SQL += ComNum.VBLF + "  AND Code = " + sabun;
            SQL += ComNum.VBLF + "  AND DELDATE IS NULL";

            string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, sqlErr);
                return rtnVal;
            }

            if (reader.HasRows)
            {
                rtnVal = true;
            }

            reader.Dispose();

            return rtnVal;
        }

        string SET_FORMNO(string arg)
        {
            switch(arg)
            {
                case "욕창":
                    lblFormNAME.Text = "욕창사정도구표";
                    return "90000";
                case "낙상":
                    lblFormNAME.Text = "낙상사정도구표";
                    return "90001";
                case "중증도":
                    lblFormNAME.Text = "환자중증도도구표";
                    return "90002";
            }

            return string.Empty;
        }
        

        /// <summary>
        /// 비고 읽는 함수
        /// </summary>
        /// <param name="strFormNo"></param>
        void READ_BIGO(string strFormNo)
        {

            OracleDataReader reader = null;
            string SQL = string.Empty;

            try
            {

                SQL = "SELECT BIGO  ";
                SQL += ComNum.VBLF + "ADMIN.EMRFORM_BIGO";
                SQL += ComNum.VBLF + "WHERE FORMNO = " + strFormNo;

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (reader.HasRows & reader.Read())
                {
                    txtBigo.Text = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();

            }

            catch(Exception EX)
            {
                clsDB.SaveSqlErrLog(EX.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, EX.Message);
            }
        }


        /// <summary>
        /// 폼 이름 가져오기
        /// </summary>
        /// <param name="sabun"></param>
        /// <returns></returns>
        void GetFormName()
        {
            OracleDataReader reader = null;

            string SQL = "SELECT FORMNAME  ";
            SQL += ComNum.VBLF + "FROM ADMIN.EMRFORM";
            SQL += ComNum.VBLF + "WHERE FORMNO = " + GstrHelpCode;

            string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, sqlErr);
                return;
            }

            if (reader.HasRows && reader.Read())
            {
                lblFormNAME.Text = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();
        }


        private void BtnSave_Click(object sender, EventArgs e)
        {
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return;

            if (Save_Data() == true)
            {
                READ_BIGO(GstrHelpCode);
                ComFunc.MsgBoxEx(this, "저장 되었습니다.");
            }

            return;

        }

        bool Save_Data()
        {
            bool rtnVal = false;
            OracleDataReader reader = null;
            string SQL = string.Empty;
            string strRowid = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRFORM_BIGO ";
                SQL = SQL + ComNum.VBLF + " WHERE FORMNO = " + GstrHelpCode;

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SQL, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }

                strRowid = reader.HasRows && reader.Read() ? reader.GetValue(0).ToString().Trim() : string.Empty;

                reader.Dispose();

                if(strRowid.Length == 0)
                {
                    SQL = " INSERT INTO ADMIN.EMRFORM_BIGO(FORMNO, BIGO, BDATE, SABUN) VALUES (";
                    SQL += ComNum.VBLF + GstrHelpCode + ",";
                    SQL += ComNum.VBLF + "'" + txtBigo.Text.Trim().Replace("'", "`") + "',";
                    SQL += ComNum.VBLF + "TRUNC(SYSDATE), ";
                    SQL += ComNum.VBLF + clsType.User.IdNumber;
                    SQL += ComNum.VBLF + ")";
                }
                else
                {
                    SQL = " UPDATE ADMIN.EMRFORM_BIGO SET ";
                    SQL += ComNum.VBLF + " BIGO = '" + txtBigo.Text.Trim().Replace("'", "`") + "' ";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";
                }

                sqlErr =  clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }


                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
