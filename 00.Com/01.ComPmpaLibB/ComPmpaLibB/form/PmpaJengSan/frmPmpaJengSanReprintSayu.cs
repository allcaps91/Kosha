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
    /// File Name       : frmPmpaJengSanReprintSayu.cs
    /// Description     : 재증명 재발급
    /// Author          : 이정현
    /// Create Date     : 2018-08-16
    /// <history> 
    /// 재증명 재발급
    /// </history>
    /// <seealso>
    /// PSMH\OPD\jengsan\FrmReprintSayu.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\OPD\jengsan\jengsan.vbp
    /// </vbp>
    /// </summary>
    public partial class frmPmpaJengSanReprintSayu : Form
    {
        public delegate void SendDataHandler(string strHelpCode, string stSINNAME, string strBDATE, string strDEPTCODE, string strDRNAME, string strDRCODE, string strNAME, string strSINSAYU);
        public event SendDataHandler SendEvent;

        private string GstrMCClass = "";
        private string GstrMCNO = "";
        private string GstrSEQNO = "";
        private string GstrSEQDATE = "";
        private string GstrBDATE = "";
        private string GstrPTNO = "";
        private string GstrLoading = "";

        public frmPmpaJengSanReprintSayu(string strMCClass, string strMCNO, string strSEQNO, string strSEQDATE, string strPTNO, string strBDATE)
        {
            InitializeComponent();

            GstrMCClass = strMCClass;
            GstrMCNO = strMCNO;
            GstrSEQNO = strSEQNO;
            GstrSEQDATE = strSEQDATE;
            GstrBDATE = strBDATE;
            GstrPTNO = strPTNO;
        }

        private void frmPmpaJengSanReprintSayu_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            GstrLoading = "OK";

            SCREEN_CLEAR();

            SetPatInfo();

            GstrLoading = "";
        }

        private void SCREEN_CLEAR()
        {
            dtpBDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            txtPtno.Text = "";
            txtSname.Text = "";
            cboDept.Items.Clear();
            txtDrCode.Text = "";
            txtDrName.Text = "";
            txtSinName.Text = "";
            txtSinSayu.Text = "";
            txtBigo.Text = "";
        }

        private void SetPatInfo()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.BDATE, A.PANO, A.SNAME, A.DEPTCODE, A.DRCODE, B.DRNAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.DRCODE = B.DRCODE ";
                SQL = SQL + ComNum.VBLF + "         AND A.BDATE = TO_DATE('" + GstrBDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = '" + GstrPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.JIN = '4'";

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
                    txtPtno.Text = GstrPTNO;
                    txtSname.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    dtpBDate.Value = Convert.ToDateTime(dt.Rows[0]["BDATE"].ToString().Trim());

                    cboDept.Items.Clear();

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                    }

                    txtDrName.Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                    txtDrCode.Text = dt.Rows[0]["DRCODE"].ToString().Trim();

                    cboDept.SelectedIndex = 0;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strMCCLASS = "";
            string strMCNO = "";
            string strSEQNO = "";
            string strSEQDATE = "";
            string strBDate = "";
            string strPANO = "";
            string strDeptCode = "";
            string strDRCODE = "";
            string strSINNAME = "";
            string strSINSAYU = "";
            string strREPRINT = "";
            string strBIGO = "";
            string strDRNAME = "";
            string strNAME = "";

            strMCCLASS = GstrMCClass;
            strMCNO = GstrMCNO;
            strSEQNO = GstrSEQNO;
            strSEQDATE = GstrSEQDATE;
            strBDate = dtpBDate.Value.ToString("yyyy-MM-dd");
            strPANO = txtPtno.Text.Trim();
            strDeptCode = cboDept.Text.Trim();
            strDRCODE = txtDrCode.Text.Trim();
            strSINNAME = txtSinName.Text.Trim();
            strSINSAYU = txtSinSayu.Text.Trim();
            strREPRINT = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"); ;
            strBIGO = txtBigo.Text.Trim();
            strDRNAME = txtDrName.Text.Trim();

            strNAME = chklist00.Checked == true ? "1" : "0";
            strNAME += chklist01.Checked == true ? "1" : "0";
            strNAME += chklist02.Checked == true ? "1" : "0";
            strNAME += chklist03.Checked == true ? "1" : "0";
            strNAME += chklist04.Checked == true ? "1" : "0";
            strNAME += chklist05.Checked == true ? "1" : "0";
            strNAME += chklist06.Checked == true ? "1" : "0";
            strNAME += chklist07.Checked == true ? "1" : "0";
            strNAME += chklist08.Checked == true ? "1" : "0";
            strNAME += chklist09.Checked == true ? "1" : "0";
            strNAME += chklist10.Checked == true ? "1" : "0";
            strNAME += chklist11.Checked == true ? "1" : "0";
            strNAME += chklist12.Checked == true ? "1" : "0";

            if (strMCCLASS == "" || strMCNO == "" || strSEQNO == "" || strSEQDATE == "")
            {
                ComFunc.MsgBox("재발급 서류 정보 읽기 오류입니다. 다시 선택하여 주시기 바랍니다.");
                return;
            }

            if (strBDate == "" || strPANO == "" || strDeptCode == "" || strDRCODE == "")
            {
                ComFunc.MsgBox("외래 접수 내역 정보 읽기 오류입니다. 접수내역을 확인하여 주시기 바랍니다.");
                return;
            }

            if (strSINNAME == "")
            {
                ComFunc.MsgBox("신청자 명이 공란입니다.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU_REPRINT";
                SQL = SQL + ComNum.VBLF + "     (MCCLASS, MCNO, SEQNO, SEQDATE, BDATE, PANO, DEPTCODE, DRCODE, ";
                SQL = SQL + ComNum.VBLF + "     SINNAME, SINSAYU, REPRINT, BIGO, WRITEDATE, WRITESABUN)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         '" + strMCCLASS + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strMCNO + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strSEQNO + "', ";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strSEQDATE + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strBDate + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "         '" + strPANO + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strDeptCode + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strDRCODE + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strSINNAME + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strSINSAYU + "', ";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strREPRINT + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "         '" + strBIGO + "', ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun;
                SQL = SQL + ComNum.VBLF + "     )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                SendEvent("", strSINNAME, strBDate, strDeptCode, strDRNAME, strDRCODE, strNAME, strSINSAYU);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SINNAME, SINSAYU, BIGO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU_REPRINT";
                SQL = SQL + ComNum.VBLF + "     WHERE BDATE = TO_DATE('" + dtpBDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + txtPtno.Text.Trim() + "'";
                SQL = SQL + ComNum.VBLF + "         AND SINNAME IS NOT NULL ";

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
                    txtSinName.Text = dt.Rows[0]["SINNAME"].ToString().Trim();
                    txtSinSayu.Text = dt.Rows[0]["SINSAYU"].ToString().Trim();
                    txtBigo.Text = dt.Rows[0]["BIGO"].ToString().Trim();
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

        private void btnRePrint_Click(object sender, EventArgs e)
        {
            SendEvent("OK", "", "", "", "", "", "", "");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            SendEvent("PASS", "", "", "", "", "", "", "");
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            if (GstrLoading == "OK") { return; }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.DEPTCODE, A.DRCODE, B.DRNAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.DRCODE = B.DRCODE ";
                SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE = '" + cboDept.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = '" + GstrPTNO + "' ";

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
                    txtDrName.Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                    txtDrCode.Text = dt.Rows[0]["DRCODE"].ToString().Trim();
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
