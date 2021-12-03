using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : 
    /// Description     : 간호진단 코드 관리
    /// Author          : 이현종
    /// Create Date     : 2019-06-01
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\mtsEmr\EMRWARD\FrmEMRNurJinNew.frm" >> frmEmrNurJinNew.cs 폼이름 재정의" />
    /// 
    public partial class frmEmrNurJinNew : Form
    {
        string gsWard = string.Empty;
        string fstrCODE1 = string.Empty;
        string fstrCODE2 = string.Empty;
        string fstrCODE3 = string.Empty;

        string fstrDEL = string.Empty;

        EmrPatient emrPatient;

        public frmEmrNurJinNew(string strWard)
        {
            InitializeComponent();
            gsWard = strWard;
        }

        public frmEmrNurJinNew(string strWard, EmrPatient emrPatient)
        {
            InitializeComponent();
            this.gsWard = strWard;
            this.emrPatient = emrPatient;
        }

        public frmEmrNurJinNew()
        {
            InitializeComponent();
        }

        private void FrmEmrNurJinNew_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

            txtResult.Clear();

            if(string.IsNullOrEmpty(txt1.Text))
            {
                ComFunc.MsgBoxEx(this, "내용을 입력하세요.");
                return;
            }

            cboWard_SET();

            cboWard.Enabled = NURSE_Manager_Check(Convert.ToInt64(clsType.User.IdNumber));

            btnSearch.PerformClick();
        }

        private void cboWard_SET()
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int nIndex = -1;

            cboWard.Items.Clear();

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "SELECT CODE  ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.NUR_CODE ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "  AND SUBUSE = '1' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SUBRANKING ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWard.Items.Add(dt.Rows[i]["CODE"].ToString().Trim());
                        if(cboWard.Items[i].ToString().Equals(gsWard))
                        {
                            nIndex = i;
                        }
                    }
                }

                if(nIndex != - 1)
                {
                    cboWard.SelectedIndex = nIndex;
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private bool NURSE_Manager_Check(long ArgSabun)
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader oracleDataReader = null;

            try
            {
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "  CODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "  AND GUBUN='NUR_간호부관리자사번'";
                SQL = SQL + ComNum.VBLF + "  AND CODE=" + ArgSabun + "";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL";

                SqlErr = clsDB.GetAdoRs(ref oracleDataReader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (oracleDataReader.HasRows && oracleDataReader.Read() && VB.Val(oracleDataReader.GetValue(0).ToString()) > 0)
                {
                    rtnVal = true;
                }

                oracleDataReader.Dispose();
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            return rtnVal;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if(chkAll.Checked)
            {
                READ_JIN1();
            }
            else
            {
                if(string.IsNullOrEmpty(cboWard.Text.Trim()))
                {
                    ComFunc.MsgBoxEx(this, "병동을 선택해주세요.");
                    return;
                }

                ss1_Sheet1.RowCount = 0;
                READ_JIN2("", cboWard.Text.Trim());
            }
        }

        void READ_JIN1()
        {
            #region 변수 초기화
            fstrCODE1 = string.Empty;
            fstrCODE2 = string.Empty;
            fstrCODE3 = string.Empty;

            ss1_Sheet1.RowCount = 0;
            ss2_Sheet1.RowCount = 0;
            ss3_Sheet1.RowCount = 0;
            ss4_Sheet1.RowCount = 0;
            ss5_Sheet1.RowCount = 0;
            ss6_Sheet1.RowCount = 0;
            #endregion

            DataTable dt = null;
            string SQL = string.Empty;
            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "SELECT CODE, NAME  ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.NUR_CODE_DAR ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = '1'";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODE ";

                string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.RowCount = dt.Rows.Count;
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
            }

        }

        void READ_JIN2(string arg, string argWard = "")
        {
            #region 변수 초기화
            fstrCODE2 = string.Empty;
            fstrCODE3 = string.Empty;

            fstrCODE1 = arg;

            ss2_Sheet1.RowCount = 0;
            ss3_Sheet1.RowCount = 0;
            ss4_Sheet1.RowCount = 0;
            ss5_Sheet1.RowCount = 0;
            ss6_Sheet1.RowCount = 0;
            #endregion

            DataTable dt = null;
            string SQL = string.Empty;
            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "SELECT CODE, NAME  ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.NUR_CODE_DAR A";
                SQL = SQL + ComNum.VBLF + "WHERE A.GUBUN = '2'";

                if(string.IsNullOrEmpty(argWard) == false)
                {
                    SQL = SQL + ComNum.VBLF + "    AND EXISTS ( ";
                    SQL = SQL + ComNum.VBLF + "        SELECT 1 ";
                    SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_PMPA.NUR_CODE_DAR_WARDSET SUB";
                    SQL = SQL + ComNum.VBLF + "         WHERE SUB.WARDCODE = '" + argWard + "' ";
                    SQL = SQL + ComNum.VBLF + "           AND A.CODE = SUB.CODE";
                    SQL = SQL + ComNum.VBLF + "           AND SUB.GUBUN = '2')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND CODE LIKE '" + arg + "%'";
                }

                SQL = SQL + ComNum.VBLF + "    AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "  ORDER BY CODE ";

                string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss2_Sheet1.RowCount = dt.Rows.Count;
                    ss2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ss2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        void READ_JIN3(string arg)
        {
            #region 변수 초기화
            fstrCODE3 = string.Empty;

            fstrCODE2 = arg;

            ss3_Sheet1.RowCount = 0;
            ss4_Sheet1.RowCount = 0;
            ss5_Sheet1.RowCount = 0;
            ss6_Sheet1.RowCount = 0;
            #endregion

            DataTable dt = null;
            string SQL = string.Empty;
            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "SELECT CODE, NAME  ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.NUR_CODE_DAR ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = '3'";
                SQL = SQL + ComNum.VBLF + "  AND CODE LIKE '" + arg + "%'";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "  ORDER BY CODE ";

                string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss3_Sheet1.RowCount = dt.Rows.Count;
                    ss3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ss3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        void READ_JIN4(string arg)
        {
            #region 변수 초기화
            ss4_Sheet1.RowCount = 0;
            #endregion

            DataTable dt = null;
            string SQL = string.Empty;
            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "SELECT CODE, NAME  ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.NUR_CODE_DAR ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = 'D'";
                SQL = SQL + ComNum.VBLF + "  AND CODE LIKE '" + arg + "%'";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "  ORDER BY CODE ";

                string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss4_Sheet1.RowCount = dt.Rows.Count;
                    ss4_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ss4_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ss4_Sheet1.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();                        
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        void READ_JIN5(string arg)
        {
            #region 변수 초기화
            ss5_Sheet1.RowCount = 0;
            #endregion

            DataTable dt = null;
            string SQL = string.Empty;
            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "SELECT CODE, NAME  ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.NUR_CODE_DAR ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = 'A'";
                SQL = SQL + ComNum.VBLF + "  AND CODE LIKE '" + arg + "%'";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "  ORDER BY CODE ";

                string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss5_Sheet1.RowCount = dt.Rows.Count;
                    ss5_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ss5_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ss5_Sheet1.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();                        
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        void READ_JIN6(string arg)
        {
            #region 변수 초기화
            ss6_Sheet1.RowCount = 0;
            #endregion

            DataTable dt = null;
            string SQL = string.Empty;
            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "SELECT CODE, NAME  ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.NUR_CODE_DAR ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = 'R'";
                SQL = SQL + ComNum.VBLF + "  AND CODE LIKE '" + arg + "%'";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "  ORDER BY CODE ";

                string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss6_Sheet1.RowCount = dt.Rows.Count;
                    ss6_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ss6_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ss6_Sheet1.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();                        
                    }
                }

                dt.Dispose();
                dt = null;

                if(ss6_Sheet1.RowCount == 0)
                {
                    ss6_Sheet1.RowCount = 1;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }


            if(SaveData())
            {
                Close();
            }
        }

        bool SaveData()
        {
            bool rtnVal = false;
            clsEmrPublic.GstrNurData.Clear();
            clsEmrPublic.GstrNurAction.Clear();
            clsEmrPublic.GstrNurCodeDAR = "";
            StringBuilder strGoal = new StringBuilder();
            DataTable dt = null;

            
            try
            {
                for(int i = 0; i < ss4_Sheet1.RowCount; i++)
                {
                    if(Convert.ToBoolean(ss4_Sheet1.Cells[i, 0].Value) == true)
                    {
                        if(string.IsNullOrEmpty(ss4_Sheet1.Cells[i, 2].Text.Trim()) == false)
                        {
                            clsEmrPublic.GstrNurData.Add(ss4_Sheet1.Cells[i, 2].Text.Trim());
                        }
                    }
                }

                if(clsEmrPublic.GstrNurData.Count == 0)
                {
                    ComFunc.MsgBoxEx(this, "Data를 선택해주세요.");
                    return rtnVal;
                }

                for (int i = 0; i < ss5_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ss5_Sheet1.Cells[i, 0].Value) == true)
                    {
                        if (string.IsNullOrEmpty(ss5_Sheet1.Cells[i, 2].Text.Trim()) == false)
                        {
                            clsEmrPublic.GstrNurAction.Add(ss5_Sheet1.Cells[i, 2].Text.Trim());
                        }
                    }
                }

                if (clsEmrPublic.GstrNurAction.Count == 0)
                {
                    ComFunc.MsgBoxEx(this, "Action을 선택해주세요.");
                    return rtnVal;
                }


                for (int i = 0; i < ss6_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ss6_Sheet1.Cells[i, 0].Value) == true)
                    {
                        if (string.IsNullOrEmpty(ss6_Sheet1.Cells[i, 2].Text.Trim()) == false)
                        {
                            strGoal.Append(ss6_Sheet1.Cells[i, 2].Text.Trim());
                        }
                    }
                }

                clsEmrPublic.GstrNurResult1 = txtResult.Text.Trim();
                clsEmrPublic.GstrNurCodeDAR = fstrCODE3;

                clsDB.setBeginTran(clsDB.DbCon);

                if (strGoal.Length > 0)
                {
                    #region 조회 
                    string SQL = " SELECT NAME ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_CODE_DAR";
                    SQL = SQL + ComNum.VBLF + " WHERE CODE = '" + fstrCODE3 + "'";

                    string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (string.IsNullOrEmpty(sqlErr) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, sqlErr);
                        return rtnVal;
                    }

                    string strProblem = dt.Rows.Count > 0 ? dt.Rows[0]["NAME"].ToString().Trim() : string.Empty;
                    #endregion

                    #region INSERT
                    SQL = " INSERT INTO KOSMOS_PMPA.NUR_CARE_GOAL (";
                    SQL = SQL + ComNum.VBLF + " PTNO, INDATE, SDATE, PROBLEM, ";
                    SQL = SQL + ComNum.VBLF + " GOAL, WRITEDATE, WRITESABUN, RANKING, DARCODE) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + "'" + emrPatient.ptNo + "', TO_DATE('" + ComFunc.FormatStrToDateEx(emrPatient.medFrDate,"D","-") + "','YYYY-MM-DD'), TO_DATE('" + dtpDate.Value.ToShortDateString() + "','YYYY-MM-DD'),'" + strProblem.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "'" + strGoal.ToString().Trim() + "', SYSDATE, " + clsType.User.IdNumber + ", 0, '" +  fstrCODE3 + "')"; 

                    int RowAffected = 0;
                    sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                    if (string.IsNullOrEmpty(sqlErr) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, sqlErr);
                        return rtnVal;
                    }
                    #endregion
                }


                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return rtnVal;
            }
        }

        private void BtnSaveNur_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if(SaveOnly())
            {
                Close();
            }

        }

        bool SaveOnly()
        {
            bool rtnVal = false;
            StringBuilder strGoal = new StringBuilder();
            DataTable dt = null;

            clsEmrPublic.GstrNurCodeDAR = "";

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for(int i = 0; i < ss6_Sheet1.RowCount; i++)
                {
                    if(Convert.ToBoolean(ss6_Sheet1.Cells[i, 0].Value) == true)
                    {
                        if(string.IsNullOrEmpty(ss6_Sheet1.Cells[i, 2].Text.Trim()) == false)
                        {
                            strGoal.Append(ss6_Sheet1.Cells[i, 2].Text.Trim());
                        }
                    }
                }

                if(strGoal.Length > 0)
                {
                    #region 조회 
                    string SQL = " SELECT NAME ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_CODE_DAR";
                    SQL = SQL + ComNum.VBLF + " WHERE CODE = '" + fstrCODE3 + "'";

                    string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (string.IsNullOrEmpty(sqlErr) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, sqlErr);
                        return rtnVal;
                    }

                    string strProblem = dt.Rows.Count > 0 ? dt.Rows[0]["NAME"].ToString().Trim() : string.Empty;
                    #endregion

                    #region INSERT
                    SQL = " INSERT INTO KOSMOS_PMPA.NUR_CARE_GOAL (";
                    SQL = SQL + ComNum.VBLF + " PTNO, INDATE, SDATE, PROBLEM, ";
                    SQL = SQL + ComNum.VBLF + " GOAL, WRITEDATE, WRITESABUN, RANKING, DARCODE) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + "'" + emrPatient.ptNo + "', TO_DATE('" + emrPatient.medFrDate + "','YYYY-MM-DD'), TO_DATE('" + dtpDate.Value.ToShortDateString() + "','YYYY-MM-DD'),'" + strProblem.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "'" + strGoal.ToString().Trim() + "', SYSDATE, " + clsType.User.IdNumber + ", 0, '" + clsEmrPublic.GstrNurCodeDAR + "')"; 

                    int RowAffected = 0;
                    sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                    if (string.IsNullOrEmpty(sqlErr) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, sqlErr);
                        return rtnVal;
                    }
                    #endregion
                }

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return rtnVal;
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            if(txt1.Text.Trim().Length == 0)
            {
                ComFunc.MsgBoxEx(this, "내용이 없습니다.");
                return;
            }

            string strGubun = string.Empty;
            string strCode = string.Empty;
            string strCode2 = string.Empty;

            if(rdo1.Checked)
            {
                strGubun = "1";
                strCode2 = string.Empty;
            }
            else if(rdo2.Checked)
            {
                strGubun = "2";
                strCode2 = fstrCODE1;
            }
            else if (rdo3.Checked)
            {
                strGubun = "3";
                strCode2 = fstrCODE2;
            }
            else if (rdo4.Checked)
            {
                strGubun = "D";
                strCode2 = fstrCODE3;
            }
            else if (rdo5.Checked)
            {
                strGubun = "A";
                strCode2 = fstrCODE3;
            }
            else if (rdo6.Checked)
            {
                strGubun = "R";
                strCode2 = fstrCODE3;
            }

            if (string.IsNullOrEmpty(strGubun))
            {
                ComFunc.MsgBoxEx(this, "분류를 선택하세요.");
                return;
            }

            strCode = strCode2 + ReadMinCode(strGubun, strCode2);

            ContentChk(strGubun);

            if(string.IsNullOrEmpty(strCode))
            {
                ComFunc.MsgBoxEx(this, "더 이상 추가 할 수 없습니다.");
                return;
            }

            #region Insert 구문
            string SQL = string.Empty;
            string sqlErr = string.Empty;
            int RowAffected = 0;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = " INSERT INTO KOSMOS_PMPA.NUR_CODE_DAR";
                SQL = SQL + ComNum.VBLF + " (GUBUN, CODE, NAME) VALUES (";
                SQL = SQL + ComNum.VBLF + "'" + strGubun + "','" + strCode + "','" + txt1.Text.Trim() + "')";

                sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if(string.IsNullOrEmpty(sqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            #endregion 

            #region Insert 후 라디오 버튼에 맞게 자동조회
            switch (strGubun)
            { 
                case "1":
                    READ_JIN1();
                    break;
                case "2":
                    READ_JIN2(strCode2);
                    break;
                case "3":
                    READ_JIN3(strCode2);
                    break;

                case "D":
                    READ_JIN4(strCode2);
                    break;

                case "A":
                    READ_JIN5(strCode2);
                    break;

                case "R":
                    READ_JIN6(strCode2);
                    break;
            }
            #endregion
        }

        /// <summary>
        /// 분류 중복 체크
        /// </summary>
        /// <param name="strGubun"></param>
        void ContentChk(string strGubun)
        {
            string SQL = string.Empty;
            OracleDataReader oracleDataReader = null;

            try
            {
                SQL = " SELECT CODE";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_CODE_DAR";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '" + strGubun + "'";
                SQL = SQL + ComNum.VBLF + "   AND NAME  = '" + txt1.Text.Trim() + "'";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL";

                string SqlErr = clsDB.GetAdoRs(ref oracleDataReader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (oracleDataReader.HasRows)
                {
                    ComFunc.MsgBoxEx(this, "해당 분류에 같은 내용이 있습니다.");
                }

                oracleDataReader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        string ReadMinCode(string arg, string arg2)
        {
            string rtnVal = "01";
            string SQL = string.Empty;
            OracleDataReader oracleDataReader = null;

            try
            {
                SQL = " SELECT MIN(SEQ_STRING) MINCODE";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_SEQ_DIGIT A";
                SQL = SQL + ComNum.VBLF + " WHERE NOT EXISTS (";
                SQL = SQL + ComNum.VBLF + "  SELECT CODE FROM (";
                SQL = SQL + ComNum.VBLF + "  SELECT TRIM(SUBSTR(CODE, LENGTH(TRIM(CODE))-1, LENGTH(TRIM(CODE)))) CODE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_CODE_DAR";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '" + arg + "'";
                if (string.IsNullOrEmpty(arg2) == false)
                {
                    SQL = SQL + ComNum.VBLF + "     AND CODE LIKE '" + arg2 + "%'";
                }
                SQL = SQL + ComNum.VBLF + " ) B";
                SQL = SQL + ComNum.VBLF + " WHERE TRIM(A.SEQ_STRING) = TRIM(B.CODE))";

                string SqlErr = clsDB.GetAdoRs(ref oracleDataReader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (oracleDataReader.HasRows && oracleDataReader.Read()) 
                {
                    rtnVal = oracleDataReader.GetValue(0).ToString().Trim();
                }

                oracleDataReader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            return rtnVal;
        }

        private void BtnSaveWard_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return; //권한 확인

            if (chkAll.Checked == false)
            {
                ComFunc.MsgBoxEx(this, "전체조회 선택 후 항목을 선택하셔서 등록하시기 바랍니다.");
                return;
            }

            if(SaveWard())
            {
                ComFunc.MsgBoxEx(this, "저장되었습니다.");
            }

        }
        
        bool SaveWard()
        {
            bool rtnVal = false;
            DataTable dt = null;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for(int i = 0; i < ss2_Sheet1.RowCount; i++)
                {
                    string strCode = ss2_Sheet1.Cells[i, 1].Text.Trim();

                    if (Convert.ToBoolean(ss2_Sheet1.Cells[i, 0].Value) == true)
                    {
                        string SQL = " SELECT CODE ";
                        SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_CODE_DAR_WARDSET "            ;
                        SQL += ComNum.VBLF + " WHERE GUBUN = '2'"                                 ;
                        SQL += ComNum.VBLF + "   AND WARDCODE = '" + cboWard.Text.Trim() + "' "  ;
                        SQL += ComNum.VBLF + "   AND CODE = '" + strCode + "' ";

                        string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (string.IsNullOrEmpty(sqlErr) == false)
                        {
                            ComFunc.MsgBoxEx(this, "조회중 오류가 발생했습니다.");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return rtnVal;
                        }

                        if(dt.Rows.Count == 0)
                        {
                            SQL = "INSERT INTO KOSMOS_PMPA.NUR_CODE_DAR_WARDSET(CODE, GUBUN, WARDCODE)";
                            SQL += ComNum.VBLF + "VALUES (";
                            SQL += ComNum.VBLF + "'" + strCode + "',";
                            SQL += ComNum.VBLF + "'2',";
                            SQL += ComNum.VBLF + "'" + cboWard.Text.Trim() + "',";
                            SQL += ComNum.VBLF + ")";

                            int RowAffected = 0;
                            sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                            if (string.IsNullOrEmpty(sqlErr) == false)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return rtnVal;
                            }
                        }

                        dt.Dispose();
                    }
                } 

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
                return rtnVal;
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
               return rtnVal;
            }
        }

        private void BtnDeleteWard_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                return; //권한 확인

            if (chkAll.Checked == true)
            {
                ComFunc.MsgBoxEx(this, "전체조회 해제하신 후 삭제하시기 바랍니다.");
                return;
            }

            if(ComFunc.MsgBoxQ("등록한 내역을 삭제하시겠습니까?", "확인") == DialogResult.No)
                return;


            if (DeleteWard())
            {
                ComFunc.MsgBoxEx(this, "삭제되었습니다.");
                btnSearch.PerformClick();
            }
        }

        bool DeleteWard()
        {
            bool rtnVal = false;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (int i = 0; i < ss2_Sheet1.RowCount; i++)
                {
                    string strCode = ss2_Sheet1.Cells[i, 1].Text.Trim();

                    if (Convert.ToBoolean(ss2_Sheet1.Cells[i, 0].Value) == true)
                    {
                        string SQL = string.Empty;
                        SQL = "DELETE KOSMOS_PMPA.NUR_CODE_DAR_WARDSET";
                        SQL += ComNum.VBLF + " WHERE GUBUN = '2'";
                        SQL += ComNum.VBLF + "  AND WARDCODE = '" + cboWard.Text.Trim() + "' ";
                        SQL += ComNum.VBLF + "  AND CODE = '" + strCode + "' ";

                        int RowAffected = 0;
                        string sqlErr = string.Empty;
                        sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                        if (string.IsNullOrEmpty(sqlErr) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return rtnVal;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// 코드 삭제 공용함수
        /// </summary>
        /// <param name="spd"></param>
        /// <returns></returns>
        bool CodeDEL(FarPoint.Win.Spread.FpSpread spd)
        {
            string SQL = string.Empty;
            bool rtnVal = false;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                for (int i = 0; i < spd.ActiveSheet.RowCount; i++)
                {
                    if (Convert.ToBoolean(spd.ActiveSheet.Cells[i, 0].Value) == true)
                    {
                        if (string.IsNullOrEmpty(fstrDEL))
                        {
                            fstrDEL = spd.Name.ToUpper();
                        }

                        string strCode = spd.ActiveSheet.Cells[i, 1].Text.Trim();

                        SQL = "UPDATE KOSMOS_PMPA.NUR_CODE_DAR";
                        SQL = SQL + ComNum.VBLF + "SET";
                        SQL = SQL + ComNum.VBLF + "DELDATE = SYSDATE";
                        SQL = SQL + ComNum.VBLF + "WHERE CODE LIKE '" + strCode + "%'";
                        if (fstrDEL.Equals("SS4"))
                        {
                            SQL = SQL + ComNum.VBLF + "   AND GUBUN = 'D'";
                        } 
                        if (fstrDEL.Equals("SS5"))
                        {
                            SQL = SQL + ComNum.VBLF + "   AND GUBUN = 'A'";
                        } 
                        if (fstrDEL.Equals("SS6"))
                        {
                            SQL = SQL + ComNum.VBLF + "   AND GUBUN = 'R'";
                        }

                        string SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(this, "삭제중 문제가 발생했습니다");
                            return rtnVal;
                        }
                    }
                }

                rtnVal = true;
                clsDB.setCommitTran(clsDB.DbCon);
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// 라디오 값 설정
        /// </summary>
        /// <param name="radio"></param>
        void RdoValue(RadioButton radio)
        {
            rdo1.Checked = false;
            rdo2.Checked = false;
            rdo3.Checked = false;
            rdo4.Checked = false;
            rdo5.Checked = false;
            rdo6.Checked = false;

            radio.Checked = true;

            txt1.Clear();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            RdoValue(rdo1);
            Ss1_CellDoubleClick(null, e);
        }

        private void Ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ss1_Sheet1.RowCount == 0)
                return;

            READ_JIN2(ss1_Sheet1.Cells[e.Row, 1].Text.Trim());
        }

        private void Ss2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            RdoValue(rdo2);
            Ss2_CellDoubleClick(null, e);
        }

        private void Ss2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ss2_Sheet1.RowCount == 0)
                return;

            READ_JIN3(ss2_Sheet1.Cells[e.Row, 1].Text.Trim());
        }

        private void Ss3_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            RdoValue(rdo3);
            Ss3_CellDoubleClick(null, e);
        }

        private void Ss3_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ss3_Sheet1.RowCount == 0)
                return;

            string strTmp = ss3_Sheet1.Cells[e.Row, 1].Text.Trim();

            READ_JIN4(strTmp);
            READ_JIN5(strTmp);
            READ_JIN6(strTmp);

            fstrCODE3 = strTmp;
        }

        private void Ss4_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            RdoValue(rdo4);
        }

        private void Ss5_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            RdoValue(rdo5);
        }

        private void Ss6_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            if (e.Row != 1)
                return;

            ss6_Sheet1.Cells[e.Row, 0].Text = "True";
            ss6_Sheet1.SetRowHeight(e.Row, Convert.ToInt32(ss6_Sheet1.GetPreferredRowHeight(e.Row)) + 10);
        }

        private void ChkAll_CheckedChanged(object sender, EventArgs e)
        {
            if(chkAll.Checked)
            {
                ComFunc.MsgBoxEx(this, "전체 간호진단 조회일 경우 대분류부터 선택하셔야 중분류가 조회가 됩니다.");
            }
        }

        private void BtnDeleteData_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                return; //권한 확인

            if (ComFunc.MsgBoxQ("코드를 삭제하시겠습니까?") == DialogResult.No)
                return;

            fstrDEL = string.Empty;

            CodeDEL(ss1);
            CodeDEL(ss2);
            CodeDEL(ss3);
            CodeDEL(ss4);
            CodeDEL(ss5);
            CodeDEL(ss6);

            switch(fstrDEL)
            {
                case "SS1":
                    READ_JIN1();
                    break;
                case "SS2":
                    READ_JIN2(fstrCODE1);
                    break;
                case "SS3":
                    READ_JIN3(fstrCODE2);
                    break;
                case "SS4":
                    READ_JIN4(fstrCODE3);
                    break;
                case "SS5":
                case "SS6":
                    READ_JIN5(fstrCODE3);
                    break;
            }
        }

        private void BtnDeleteAction_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                return; //권한 확인

            if (ComFunc.MsgBoxQ("코드를 삭제하시겠습니까?") == DialogResult.No)
                return;

            fstrDEL = string.Empty;

            CodeDEL(ss5);

            READ_JIN5(fstrCODE3);
        }

        private void BtnAddRow_Click(object sender, EventArgs e)
        {
            ss6_Sheet1.RowCount += 1;
        }

        private void BtnDeleteNur_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                return; //권한 확인

            if (ComFunc.MsgBoxQ("코드를 삭제하시겠습니까?") == DialogResult.No)
                return;

            fstrDEL = string.Empty;

            CodeDEL(ss6);

            READ_JIN6(fstrCODE3);
        }
    }
}
