using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 문자서비스 사용 권한 설정
/// Author : 김형범
/// Create Date : 2017.06.20
/// </summary>
/// <history>
/// 완료
/// </history>
namespace ComLibB
{
    /// <summary> 문자서비스 사용 권한 설정 </summary>
    public partial class frmSMSManager : Form
    {
        /// <summary> 문자서비스 사용 권한 설정 </summary>
        public frmSMSManager()
        {
            InitializeComponent();
        }

        void frmSMSManager_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ClearScreen();
            GetData();
        }

        void btnView_Click(object sender, EventArgs e)
        {
            GetData();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strSabun = "";
            string strMemo = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int intRowAffected = 0;

            try
            {
                strSabun = VB.Val(txtSabun.Text.Trim()).ToString("00000");
                strMemo = txtMemo.Text.Trim().Replace("'", "`");

                SQL = "";
                SQL = SQL + " SELECT SABUN ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "REPORT_SMS ";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + strSabun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("해당 사번은 SMS 사용권한이 등록되어 있습니다.", "확인");
                    dt.Dispose();
                    dt = null;
                    return;
                }

                dt.Dispose();
                dt = null;

                clsDB.setBeginTran(clsDB.DbCon);
                

                try
                {

                    SQL = "";
                    SQL = SQL + " INSERT INTO " + ComNum.DB_PMPA + "REPORT_SMS(SABUN, GRADE, MEMO, MMS) VALUES(";
                    SQL = SQL + ComNum.VBLF + "'" + strSabun + "','1', '" + strMemo + "','0')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("저장 하였습니다.");

                    GetData();

                    return;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (ssSMS_Sheet1.RowCount < 1)
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < ssSMS_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssSMS_Sheet1.Cells[i, 0].Value) == true)
                    {
                        if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                        {
                            return; //권한 확인
                        }

                        SQL = "";
                        SQL = SQL + " UPDATE " + ComNum.DB_PMPA + "REPORT_SMS SET ";
                        SQL = SQL + ComNum.VBLF + " GRADE = '0', ";
                        SQL = SQL + ComNum.VBLF + " DELDATE = TO_DATE('" + (ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  WHERE DELDATE IS NULL ";
                        SQL = SQL + ComNum.VBLF + "    AND SABUN = '" + ssSMS_Sheet1.Cells[i, 1].Text.Trim() + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                        ComFunc.MsgBox("저장 하였습니다.");

                        GetData();

                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                ClearScreen();

                SQL = "";
                SQL = SQL + "SELECT A.SABUN, B.KORNAME, A.MEMO ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "REPORT_SMS A, " + ComNum.DB_ERP + "INSA_MST B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND A.SABUN = B.SABUN ";
                SQL = SQL + ComNum.VBLF + " ORDER BY B.KORNAME ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssSMS_Sheet1.Rows.Count = dt.Rows.Count;
                ssSMS_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssSMS_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                    ssSMS_Sheet1.Cells[i, 2].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                    ssSMS_Sheet1.Cells[i, 3].Text = dt.Rows[i]["MEMO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void ClearScreen()
        {
            txtSabun.Text = "";
            txtPanel.Text = "";
            txtMemo.Text = "";
            ssSMS_Sheet1.RowCount = 0;
        }

        private void txtSabun_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void txtSabun_Leave(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = SQL + "SELECT KORNAME ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "INSA_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + VB.Val(txtSabun.Text).ToString("00000") + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    txtPanel.Text = "";
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                txtPanel.Text = dt.Rows[0]["KORNAME"].ToString().Trim();

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }
    }
}
