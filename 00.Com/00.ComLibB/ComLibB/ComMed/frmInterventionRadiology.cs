using ComBase;
using ComBase.Controls;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// 전산업무 의뢰서 2021-....
    /// 선작업 진행함
    /// </summary>
    public partial class frmInterventionRadiologyReq : Form
    {
        public frmInterventionRadiologyReq()
        {
            InitializeComponent();
        }

        private void frmInterventionRadiologyReq_Load(object sender, EventArgs e)
        {
            GetData();

        }

        private void GetData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
         

            SQL = "";
            SQL += ComNum.VBLF + "SELECT  PTNO                                  ";
            SQL += ComNum.VBLF + "      ,   PROCEDURE                           ";
            SQL += ComNum.VBLF + "      ,   PROCEDURE_CHK                       ";
            SQL += ComNum.VBLF + "      ,   TO_CHAR(PROCEDURE_DATE, 'YYYY-MM-DD')  PROCEDURE_DATE                    ";
            SQL += ComNum.VBLF + "      ,   ANTI_HANG_CHK                       ";
            SQL += ComNum.VBLF + "      ,   ANTI_HANG_CHK_BIGO                  ";
            SQL += ComNum.VBLF + "      ,   ANTI_HANG_BIGO                      ";
            SQL += ComNum.VBLF + "      ,   EXAM_RESULT_PLT                     ";
            SQL += ComNum.VBLF + "      ,   EXAM_RESULT_PILMR                   ";
            SQL += ComNum.VBLF + "      ,   EXAM_RESULT_CR                      ";
            SQL += ComNum.VBLF + "      ,   EXAM_RESULT_BIGO                    ";
            SQL += ComNum.VBLF + "      ,   PROCEDURE_POSITION_NM               ";
            SQL += ComNum.VBLF + "      ,   PROCEDURE_POSITION_CHK              ";
            SQL += ComNum.VBLF + "      ,   PROCEDURE_POSITION_BIGO             ";
            SQL += ComNum.VBLF + "      ,   CREATESABUN                         ";
            SQL += ComNum.VBLF + "      ,   CREATEDATE                          ";
            SQL += ComNum.VBLF + "      ,   MEMO                                ";
            SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "ETC_RD_REQ         ";
            SQL += ComNum.VBLF + "WHERE PTNO = '" + clsOrdFunction.Pat.PtNo + "'";
            SQL += ComNum.VBLF + "  AND CREATEDATE >= TRUNC(SYSDATE)";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                txtProcedure.Text = dt.Rows[0]["PROCEDURE"].ToString().Trim();
                if (dt.Rows[0]["PROCEDURE_CHK"].ToString().Trim().Equals("0"))
                {
                    RdoPro_0.Checked = true;
                }
                else if (dt.Rows[0]["PROCEDURE_CHK"].ToString().Trim().Equals("1"))
                {
                    RdoPro_1.Checked = true;
                }
                else if (dt.Rows[0]["PROCEDURE_CHK"].ToString().Trim().Equals("2"))
                {
                    RdoPro_2.Checked = true;
                }

                dtpDate.Value = dt.Rows[0]["PROCEDURE_DATE"].To<DateTime>();
                if (dt.Rows[0]["ANTI_HANG_CHK"].ToString().Trim().Equals("0"))
                {
                    rdoAnti_0.Checked = true;
                }
                else if (dt.Rows[0]["ANTI_HANG_CHK"].ToString().Trim().Equals("1"))
                {
                    rdoAnti_1.Checked = true;
                }

                txtHang_Chk_Bigo.Text = dt.Rows[0]["ANTI_HANG_CHK_BIGO"].ToString().Trim();
                txtHang_Bigo.Text = dt.Rows[0]["ANTI_HANG_BIGO"].ToString().Trim();

                txtPLT.Text = dt.Rows[0]["EXAM_RESULT_PLT"].ToString().Trim();
                txtPTINR.Text = dt.Rows[0]["EXAM_RESULT_PILMR"].ToString().Trim();
                txtCR.Text = dt.Rows[0]["EXAM_RESULT_CR"].ToString().Trim();
                txtExamResult.Text = dt.Rows[0]["EXAM_RESULT_BIGO"].ToString().Trim();

                txtPosition.Text = dt.Rows[0]["PROCEDURE_POSITION_NM"].ToString().Trim();
                if (dt.Rows[0]["PROCEDURE_POSITION_CHK"].ToString().Trim().Equals("0"))
                {
                    rdoPosition_0.Checked = true;
                }
                else if (dt.Rows[0]["PROCEDURE_POSITION_CHK"].ToString().Trim().Equals("1"))
                {
                    rdoPosition_1.Checked = true;
                }
                txtPositionBigo.Text = dt.Rows[0]["PROCEDURE_POSITION_BIGO"].ToString().Trim();
                txtBigo.Text = dt.Rows[0]["MEMO"].ToString().Trim();
            }

            dt.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Chk_Value() && Save_Data())
            {
                Close();
            }
        }

        bool Chk_Value()
        {
            bool rtnVal = false;

            if (txtProcedure.Text.IsNullOrEmpty())
            {
                ComFunc.MsgBoxEx(this, "시술명을 입력해주세요.");
                return rtnVal;
            }

            if (RdoPro_0.Checked == false && RdoPro_1.Checked == false && RdoPro_2.Checked == false)
            {
                ComFunc.MsgBoxEx(this, "시술관련 부위를 선택해주세요.");
                return rtnVal;
            }


            if (rdoAnti_0.Checked == false && rdoAnti_1.Checked == false)
            {
                ComFunc.MsgBoxEx(this, "항혈전제 유무를 선택해주세요.");
                return rtnVal;
            }

            if (rdoPosition_0.Checked == false && rdoPosition_1.Checked == false)
            {
                ComFunc.MsgBoxEx(this, "시술자세 항목을 선택해주세요.");
                return rtnVal;
            }

            rtnVal = true;
            return rtnVal;
        }

        bool Save_Data()
        {
            int intRowAffected = 0; //변경된 Row 받는 변수

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strROWID = "";

            OracleDataReader reader = null;

            bool rtnVal = false;

            string PROCEDURE = string.Empty;
            string PROCEDURE_CHK = string.Empty;
            string PROCEDURE_DATE = string.Empty;
            string ANTI_HANG_CHK = string.Empty;
            string ANTI_HANG_CHK_BIGO = string.Empty;
            string ANTI_HANG_BIGO = string.Empty;
            string EXAM_RESULT_PLT = string.Empty;
            string EXAM_RESULT_PILMR = string.Empty;
            string EXAM_RESULT_CR = string.Empty;
            string EXAM_RESULT_BIGO = string.Empty;
            string PROCEDURE_POSITION_NM = string.Empty;
            string PROCEDURE_POSITION_CHK = string.Empty;
            string PROCEDURE_POSITION_BIGO = string.Empty;
            string CREATESABUN = clsType.User.Sabun;
            string MEMO = string.Empty;
            string PTNO = clsOrdFunction.Pat.PtNo;


            #region 변수 내용 
            //시술
            PROCEDURE = txtProcedure.Text.Trim();
            if (RdoPro_0.Checked)
            {
                PROCEDURE_CHK = "0";
            }
            else if (RdoPro_1.Checked)
            {
                PROCEDURE_CHK = "1";
            }
            else if (RdoPro_2.Checked)
            {
                PROCEDURE_CHK = "2";
            }

            PROCEDURE_DATE = dtpDate.Value.ToString("yyyy-MM-dd");

            //항혈전제
            if (rdoAnti_0.Checked)
            {
                ANTI_HANG_CHK = "0";
            }
            else if (rdoAnti_1.Checked)
            {
                ANTI_HANG_CHK = "1";
            }

            ANTI_HANG_CHK_BIGO = txtHang_Chk_Bigo.Text.Trim();
            ANTI_HANG_BIGO = txtHang_Bigo.Text.Trim();

            //검사결과
            EXAM_RESULT_PLT   = txtPLT.Text.Trim();
            EXAM_RESULT_PILMR = txtPTINR.Text.Trim();
            EXAM_RESULT_CR    = txtCR.Text.Trim();
            EXAM_RESULT_BIGO = txtExamResult.Text.Trim();

            //시술자세
            PROCEDURE_POSITION_NM = txtPosition.Text.Trim();
            if (rdoPosition_0.Checked)
            {
                PROCEDURE_POSITION_CHK = "0";
            }
            else if (rdoPosition_1.Checked)
            {
                PROCEDURE_POSITION_CHK = "1";
            }
            PROCEDURE_POSITION_BIGO = txtPositionBigo.Text.Trim();

            //비고사항
            MEMO = txtBigo.Text.Trim();
            #endregion

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ROWID";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_MED + "ETC_RD_REQ";
            SQL += ComNum.VBLF + " WHERE PTNO       = '" + clsOrdFunction.Pat.PtNo + "'";
            SQL += ComNum.VBLF + "   AND CREATEDATE = TRUNC(SYSDATE)";
            SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                strROWID = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();
            reader = null;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            if (strROWID == "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "INSERT INTO ADMIN.ETC_RD_REQ     \r";
                SQL += ComNum.VBLF + "(	                                    \r";
                SQL += ComNum.VBLF + "	 PTNO                               \r";
                SQL += ComNum.VBLF + "	,PROCEDURE                          \r";
                SQL += ComNum.VBLF + "	,PROCEDURE_CHK                      \r";
                SQL += ComNum.VBLF + "	,PROCEDURE_DATE                     \r";
                SQL += ComNum.VBLF + "	,ANTI_HANG_CHK                      \r";
                SQL += ComNum.VBLF + "	,ANTI_HANG_CHK_BIGO                 \r";
                SQL += ComNum.VBLF + "	,ANTI_HANG_BIGO                     \r";
                SQL += ComNum.VBLF + "	,EXAM_RESULT_PLT                    \r";
                SQL += ComNum.VBLF + "	,EXAM_RESULT_PILMR                  \r";
                SQL += ComNum.VBLF + "	,EXAM_RESULT_CR                     \r";
                SQL += ComNum.VBLF + "	,EXAM_RESULT_BIGO                   \r";
                SQL += ComNum.VBLF + "	,PROCEDURE_POSITION_NM              \r";
                SQL += ComNum.VBLF + "	,PROCEDURE_POSITION_CHK             \r";
                SQL += ComNum.VBLF + "	,PROCEDURE_POSITION_BIGO            \r";
                SQL += ComNum.VBLF + "	,CREATESABUN                        \r";
                SQL += ComNum.VBLF + "	,CREATEDATE                         \r";
                SQL += ComNum.VBLF + "	,MEMO                               \r";
                SQL += ComNum.VBLF + ")                                     \r";

                SQL += ComNum.VBLF + "VALUES(";
                SQL += ComNum.VBLF + " '" + PTNO + "'";
                SQL += ComNum.VBLF + ", '" + PROCEDURE + "'";
                SQL += ComNum.VBLF + ",'" + PROCEDURE_CHK + "'";
                SQL += ComNum.VBLF + ",TO_DATE('" + PROCEDURE_DATE + "', 'YYYY-MM-DD')";

                SQL += ComNum.VBLF + ",'" + ANTI_HANG_CHK + "'";
                SQL += ComNum.VBLF + ",'" + ANTI_HANG_CHK_BIGO.Replace("'", "`") + "'";
                SQL += ComNum.VBLF + ",'" + ANTI_HANG_BIGO.Replace("'", "`") + "'";

                SQL += ComNum.VBLF + ",'" + EXAM_RESULT_PLT + "'";
                SQL += ComNum.VBLF + ",'" + EXAM_RESULT_PILMR + "'";
                SQL += ComNum.VBLF + ",'" + EXAM_RESULT_CR + "'";
                SQL += ComNum.VBLF + ",'" + EXAM_RESULT_BIGO + "'";

                SQL += ComNum.VBLF + ",'" + PROCEDURE_POSITION_NM.Replace("'", "`") + "'";
                SQL += ComNum.VBLF + ",'" + PROCEDURE_POSITION_CHK.Replace("'", "`") + "'";
                SQL += ComNum.VBLF + ",'" + PROCEDURE_POSITION_BIGO.Replace("'", "`") + "'";

                SQL += ComNum.VBLF + ",'" + CREATESABUN + "'";
                SQL += ComNum.VBLF + ",SYSDATE                  ";
                SQL += ComNum.VBLF + ",'" + MEMO.Replace("'", "`") + "'";

                SQL += ComNum.VBLF + ")";
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "UPDATE ADMIN.ETC_RD_REQ";
                SQL += ComNum.VBLF + "SET ";
                SQL += ComNum.VBLF + " PROCEDURE                = '" + PROCEDURE            +"'	    \r";
                SQL += ComNum.VBLF + ",PROCEDURE_CHK            = '" + PROCEDURE_CHK        + "'	\r";
                SQL += ComNum.VBLF + ",PROCEDURE_DATE           = '" + PROCEDURE_DATE       + "'	\r";

                SQL += ComNum.VBLF + ",ANTI_HANG_CHK            = '" + ANTI_HANG_CHK + "'	        \r";
                SQL += ComNum.VBLF + ",ANTI_HANG_CHK_BIGO       = '" + ANTI_HANG_CHK_BIGO.Replace("'", "`") + "'	    \r";
                SQL += ComNum.VBLF + ",ANTI_HANG_BIGO           = '" + ANTI_HANG_BIGO.Replace("'", "`") + "'	        \r";

                SQL += ComNum.VBLF + ",EXAM_RESULT_PLT          = '" + EXAM_RESULT_PLT + "'	        \r";
                SQL += ComNum.VBLF + ",EXAM_RESULT_PILMR        = '" + EXAM_RESULT_PILMR + "'	    \r";
                SQL += ComNum.VBLF + ",EXAM_RESULT_CR           = '" + EXAM_RESULT_CR + "'	        \r";
                SQL += ComNum.VBLF + ",EXAM_RESULT_BIGO         = '" + EXAM_RESULT_BIGO.Replace("'", "`") + "'	    \r";

                SQL += ComNum.VBLF + ",PROCEDURE_POSITION_NM    = '" + PROCEDURE_POSITION_NM.Replace("'", "`") + "'	\r";
                SQL += ComNum.VBLF + ",PROCEDURE_POSITION_CHK   = '" + PROCEDURE_POSITION_CHK + "'	\r";
                SQL += ComNum.VBLF + ",PROCEDURE_POSITION_BIGO  = '" + PROCEDURE_POSITION_BIGO.Replace("'", "`") + "'	\r";

                SQL += ComNum.VBLF + ",CREATESABUN              = '" + CREATESABUN + "'	            \r";
                SQL += ComNum.VBLF + ",CREATEDATE               = SYSDATE       	                \r";    

                SQL += ComNum.VBLF + ",MEMO                     = '" + MEMO.Replace("'", "`") + "'	                \r";
                SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";
            }

            try
            {
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
