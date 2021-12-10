using ComBase;
using ComDbB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_OSHA
{
    public partial class UsermstForm : Form
    {
        static bool FbNew = false;
        static string FstrJobSabun = clsType.User.IdNumber;
        static string FstrLicno = clsType.HosInfo.SwLicense;

        public UsermstForm()
        {
            InitializeComponent();

            //관리자만 비밀번호 초기화, 삭제 가능
            if (FstrJobSabun != "1")
            {
                비밀번호초기화ToolStripMenuItem.Visible = false;
                삭제ToolStripMenuItem1.Visible = false;
            } 
            Screen_Clear();
            List_Search();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Screen_Clear()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtBuse.Text = "";
            cboRole.Text = "";
            txtMyenhe.Text = "";
            dtpInDate.Text = "";
            txtTesaDate.Text = "";
            chkJik01.Checked = false;
            chkJik02.Checked = false;
            chkJik03.Checked = false;
            chkJik04.Checked = false;
            chkJik05.Checked = false;
            chkJik06.Checked = false;
            chkJik07.Checked = false;
            chkJik08.Checked = false;
            chkJik09.Checked = false;
            chkJik10.Checked = false;
            chkJik11.Checked = false;
            chkLtduser.Checked = false;
            txtLtdcode.Text = "";
            lblLtdname.Text = "";

            삭제ToolStripMenuItem1.Enabled = false;
            비밀번호초기화ToolStripMenuItem.Enabled = false;
            txtID.Enabled = false;
            FbNew = false;
        }

        private void Frm사용자등록_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {

        }

        private void 신규ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            FbNew = true;
            txtID.Enabled = true;
            txtID.Focus();
        }

        private void 저장ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strNewPass = "";
            string strJikmu = "";
            int nLtdCode = 0;
            string strGbActive = "Y";
            string strGbDelete = "N";
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strJikmu01 = "N";
            string strJikmu02 = "N";
            string strJikmu03 = "N";
            string strJikmu04 = "N";
            string strJikmu05 = "N";
            string strJikmu06 = "N";
            string strJikmu07 = "N";
            string strJikmu08 = "N";
            string strJikmu09 = "N";
            string strJikmu10 = "N";
            string strJikmu11 = "N";

            string strLtduser = "N";

            if (txtID.Text.Trim() == "") { ComFunc.MsgBox("아이디가 공란입니다."); return; }
            int nID = Int32.Parse(txtID.Text.Trim());
            if (nID < 1 || nID > 999999) { ComFunc.MsgBox("아이디는 1~999999만 가능합니다."); return; }
            if (txtName.Text.Trim() == "") { ComFunc.MsgBox("성명이 공란입니다."); return; }
            if (txtBuse.Text.Trim() == "") { ComFunc.MsgBox("부서명이 공란입니다."); return; }
            if (cboRole.Text.Trim() == "") { ComFunc.MsgBox("권한이 공란입니다."); return; }
            if (FbNew == true)
            {
                if (nID == 1) { ComFunc.MsgBox("아이디 1은 사용이 불가능 합니다."); return; }
                strNewPass = clsAES.AES("1234");
            }
            nLtdCode = 0;
            if (txtLtdcode.Text.Trim() != "") { nLtdCode = Int32.Parse(txtLtdcode.Text.Trim()); }
            if (nLtdCode > 0 && chkLtduser.Checked == false) { ComFunc.MsgBox("관계사 사용 아이디 오류입니다."); return; }
            if (nLtdCode == 0 && chkLtduser.Checked == true) { ComFunc.MsgBox("관계사 사용 아이디 오류입니다."); return; }

            if (chkJik01.Checked == true) strJikmu01 = "Y";
            if (chkJik02.Checked == true) strJikmu02 = "Y";
            if (chkJik03.Checked == true) strJikmu03 = "Y";
            if (chkJik04.Checked == true) strJikmu04 = "Y";
            if (chkJik05.Checked == true) strJikmu05 = "Y";
            if (chkJik06.Checked == true) strJikmu06 = "Y";
            if (chkJik07.Checked == true) strJikmu07 = "Y";
            if (chkJik08.Checked == true) strJikmu08 = "Y";
            if (chkJik09.Checked == true) strJikmu09 = "Y";
            if (chkJik10.Checked == true) strJikmu10 = "Y";
            if (chkJik11.Checked == true) strJikmu11 = "Y";
            if (chkLtduser.Checked == true) strLtduser = "Y";

            strJikmu = strJikmu01 + strJikmu02 + strJikmu03 + strJikmu04 + strJikmu05;
            strJikmu += strJikmu06 + strJikmu07 + strJikmu08 + strJikmu09 + strJikmu10;
            strJikmu += strJikmu11 + "NNNN";

            if (txtTesaDate.Text.Trim() == "") { strGbActive = "Y"; strGbDelete = "N"; }

            Cursor.Current = Cursors.WaitCursor;

            DataTable Dt = new DataTable();

            try
            {
                SQL = "";
                if (FbNew == true)
                {
                    SQL += ComNum.VBLF + " INSERT INTO HIC_USERS ";
                    SQL += ComNum.VBLF + "        (SWLICENSE, USERID, NAME, DEPT, ROLE, INDATE, TESADATE,";
                    SQL += ComNum.VBLF + "         ISACTIVE, ISDELETED, JIKMU, PASSHASH256,CERTNO,SEQ_WORD,";
                    SQL += ComNum.VBLF + "         LTDUSER,MODIFIED, MODIFIEDUSER, CREATED, CREATEDUSER) ";
                    SQL += ComNum.VBLF + " VALUES ('" + FstrLicno + "', ";
                    SQL += ComNum.VBLF + "         '" + txtID.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtName.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtBuse.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + cboRole.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + dtpInDate.Value.ToString("yyyy-MM-dd") + "', ";
                    SQL += ComNum.VBLF + "         '" + txtTesaDate.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + strGbActive + "', ";
                    SQL += ComNum.VBLF + "         '" + strGbDelete + "', ";
                    SQL += ComNum.VBLF + "         '" + strJikmu + "', ";
                    SQL += ComNum.VBLF + "         '" + strNewPass + "', ";
                    SQL += ComNum.VBLF + "         '" + txtMyenhe.Text.Trim() + "','', ";
                    SQL += ComNum.VBLF + "         '" + txtLtdcode.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         SYSDATE,'" + clsType.User.IdNumber + "', ";
                    SQL += ComNum.VBLF + "         SYSDATE,'" + clsType.User.IdNumber + "') ";
                }
                else
                {
                    SQL += ComNum.VBLF + " UPDATE HIC_USERS ";
                    SQL += ComNum.VBLF + "    SET NAME          = '" + txtName.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        DEPT          = '" + txtBuse.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        ROLE          = '" + cboRole.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        InDate        = '" + dtpInDate.Value.ToString("yyyy-MM-dd") + "', ";
                    SQL += ComNum.VBLF + "        TesaDate      = '" + txtTesaDate.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        ISACTIVE      = '" + strGbActive + "', ";
                    SQL += ComNum.VBLF + "        ISDELETED     = '" + strGbDelete + "', ";
                    SQL += ComNum.VBLF + "        JIKMU         = '" + strJikmu + "', ";
                    SQL += ComNum.VBLF + "        CERTNO        = '" + txtMyenhe.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        LTDUSER       = '" + txtLtdcode.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        MODIFIEDUSER  = '" + clsType.User.IdNumber + "', ";
                    SQL += ComNum.VBLF + "        MODIFIED      = SYSDATE ";
                    SQL += ComNum.VBLF + "  WHERE SWLICENSE     = '" + FstrLicno + "' ";
                    SQL += ComNum.VBLF + "    AND USERID        = '" + txtID.Text.Trim() + "' ";
                }
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("저장 실패", "알림");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ComFunc.MsgBox("저장되었습니다.", "알림");
                Cursor.Current = Cursors.Default;
                List_Search();
                Screen_Clear();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        private void 명단조회ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void List_Search()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int i = 0;

            SS1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "      USERID , Name, DEPT, ROLE ";
                SQL = SQL + ComNum.VBLF + " FROM HIC_USERS ";
                SQL = SQL + ComNum.VBLF + " WHERE SWLicense='" + FstrLicno + "' ";
                if (txtViewName.TextLength > 0) SQL = SQL + " AND Name LIKE '%" + txtViewName.Text + "%' ";
                if (chkTejik.Checked == true) SQL = SQL + " AND (TesaDate='' OR TesaDate IS NULL) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Name,USERID ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.RowCount = dt.Rows.Count;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["USERID"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPT"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROLE"].ToString().Trim();
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void 삭제ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = "";
            int intRowAffected = 0; //변경된 Row 받는 변수

            int nID = Int32.Parse(txtID.Text.Trim());

            Cursor.Current = Cursors.WaitCursor;

            DataTable Dt = new DataTable();

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " DELETE FROM HIC_USERS ";
                SQL += ComNum.VBLF + "  WHERE SWLICENSE     = '" + FstrLicno + "'";
                SQL += ComNum.VBLF + "    AND USERID        = '" + txtID.Text.Trim() + "' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("삭제 실패", "알림");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ComFunc.MsgBox("삭제되었습니다.", "알림");
                Cursor.Current = Cursors.Default;
                List_Search();
                Screen_Clear();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }


        private void 닫기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SS1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            string strJikmu = "";
            DataTable dt = null;
            int i = 0;

            string strUserID = SS1.ActiveSheet.Cells[e.Row, 0].Value.ToString();

            Screen_Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM HIC_USERS ";
                SQL = SQL + ComNum.VBLF + "Where SWLICENSE = '" + FstrLicno + "' ";
                SQL = SQL + ComNum.VBLF + "  And USERID = '" + strUserID + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    txtID.Text = dt.Rows[i]["USERID"].ToString().Trim();
                    txtName.Text = dt.Rows[i]["Name"].ToString().Trim();
                    txtBuse.Text = dt.Rows[i]["DEPT"].ToString().Trim();
                    cboRole.Text = dt.Rows[i]["ROLE"].ToString().Trim();
                    txtMyenhe.Text = dt.Rows[i]["CERTNO"].ToString().Trim();
                    dtpInDate.Text = dt.Rows[0]["InDate"].ToString().Trim();
                    txtTesaDate.Text = dt.Rows[0]["TesaDate"].ToString().Trim();
                    strJikmu = dt.Rows[0]["JIKMU"].ToString().Trim();

                    if (VB.Left(strJikmu,1)=="Y") chkJik01.Checked = true;
                    if (VB.Mid(strJikmu, 2, 1) == "Y") chkJik02.Checked = true;
                    if (VB.Mid(strJikmu, 3, 1) == "Y") chkJik03.Checked = true;
                    if (VB.Mid(strJikmu, 4, 1) == "Y") chkJik04.Checked = true;
                    if (VB.Mid(strJikmu, 5, 1) == "Y") chkJik05.Checked = true;
                    if (VB.Mid(strJikmu, 6, 1) == "Y") chkJik06.Checked = true;
                    if (VB.Mid(strJikmu, 7, 1) == "Y") chkJik07.Checked = true;
                    if (VB.Mid(strJikmu, 8, 1) == "Y") chkJik08.Checked = true;
                    if (VB.Mid(strJikmu, 9, 1) == "Y") chkJik09.Checked = true;
                    if (VB.Mid(strJikmu, 10, 1) == "Y") chkJik10.Checked = true;
                    if (VB.Mid(strJikmu, 11, 1) == "Y") chkJik11.Checked = true;
                }

                dt.Dispose();
                dt = null;

                삭제ToolStripMenuItem1.Enabled = true;
                if (clsType.User.IdNumber == "1") //관리자
                {
                    비밀번호초기화ToolStripMenuItem.Visible = true;
                    비밀번호초기화ToolStripMenuItem.Enabled = true;
                }

                Cursor.Current = Cursors.Default;
                txtName.Focus();

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            List_Search();
        }

        private void 비밀번호초기화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = "";
            string strPass = "";
            int intRowAffected = 0; //변경된 Row 받는 변수

            int nID = Int32.Parse(txtID.Text.Trim());
            if (nID ==1) { ComFunc.MsgBox("관리자비번은 초기화 불가능합니다.", "알림"); return; }

            strPass = clsAES.AES("1234");

            Cursor.Current = Cursors.WaitCursor;

            DataTable Dt = new DataTable();

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE HIC_USERS SET ";
                SQL += ComNum.VBLF + "        PASSHASH256   = '" + strPass + "', ";
                SQL += ComNum.VBLF + "        MODIFIEDUSER  = '" + clsType.User.IdNumber + "', ";
                SQL += ComNum.VBLF + "        MODIFIED      = SYSDATE ";
                SQL += ComNum.VBLF + "  WHERE SWLICENSE     = '" + FstrLicno + "'";
                SQL += ComNum.VBLF + "    AND USERID        = '" + txtID.Text.Trim() + "' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("비밀번호 초기화 실패", "알림");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ComFunc.MsgBox("초기화되었습니다.", "알림");
                Cursor.Current = Cursors.Default;
                List_Search();
                Screen_Clear();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void txtLtdcode_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

