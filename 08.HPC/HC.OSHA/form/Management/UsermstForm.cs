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
        static int FnJobSabun = 0;
        static string FstrLicno = "1234-1234-1234";

        public UsermstForm()
        {
            InitializeComponent();

            FstrLicno = clsType.HosInfo.SwLicense;
            FnJobSabun = Int32.Parse(clsType.User.IdNumber);

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
            txtJik.Text = "";
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

            삭제ToolStripMenuItem1.Visible = false;
            비밀번호초기화ToolStripMenuItem.Visible = false;
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

        private void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 삭제ToolStripMenuItem_Click(object sender, EventArgs e)
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
            int nLtdCode = 0;
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
            if (txtJik.Text.Trim() == "") { ComFunc.MsgBox("직책이 공란입니다."); return; }
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

            Cursor.Current = Cursors.WaitCursor;

            DataTable Dt = new DataTable();

            try
            {
                SQL = "";
                if (FbNew == true)
                {
                    SQL += ComNum.VBLF + " INSERT INTO HIC_USERMST ";
                    SQL += ComNum.VBLF + "        (LicNo, SABUN, NAME, BUSE, JIK, INDATE, TESADATE,";
                    SQL += ComNum.VBLF + "         JIKMU01, JIKMU02, JIKMU03, JIKMU04,JIKMU05,JIKMU06,";
                    SQL += ComNum.VBLF + "         JIKMU07, JIKMU08, JIKMU09, JIKMU10,JIKMU11,";
                    SQL += ComNum.VBLF + "         MYENHENO, Password, GBLTDUSER, LTDCODE, ENTTIME, ENTSABUN) ";
                    SQL += ComNum.VBLF + " VALUES ('" + FstrLicno + "', ";
                    SQL += ComNum.VBLF + "         " + nID + ", ";
                    SQL += ComNum.VBLF + "         '" + txtName.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtBuse.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtJik.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + dtpInDate.Value.ToString("yyyy-MM-dd") + "', ";
                    SQL += ComNum.VBLF + "         '" + txtTesaDate.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + strJikmu01 + "', ";
                    SQL += ComNum.VBLF + "         '" + strJikmu02 + "', ";
                    SQL += ComNum.VBLF + "         '" + strJikmu03 + "', ";
                    SQL += ComNum.VBLF + "         '" + strJikmu04 + "', ";
                    SQL += ComNum.VBLF + "         '" + strJikmu05 + "', ";
                    SQL += ComNum.VBLF + "         '" + strJikmu06 + "', ";
                    SQL += ComNum.VBLF + "         '" + strJikmu07 + "', ";
                    SQL += ComNum.VBLF + "         '" + strJikmu08 + "', ";
                    SQL += ComNum.VBLF + "         '" + strJikmu09 + "', ";
                    SQL += ComNum.VBLF + "         '" + strJikmu10 + "', ";
                    SQL += ComNum.VBLF + "         '" + strJikmu11 + "', ";
                    SQL += ComNum.VBLF + "         '" + txtMyenhe.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + strNewPass + "', ";
                    SQL += ComNum.VBLF + "         '" + strLtduser + "', ";
                    SQL += ComNum.VBLF + "          " + nLtdCode + ", ";
                    SQL += ComNum.VBLF + "         SYSDATE,"  + FnJobSabun + ") ";
                }
                else
                {
                    SQL += ComNum.VBLF + " UPDATE HIC_USERMST ";
                    SQL += ComNum.VBLF + "    SET NAME          = '" + txtName.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        Buse          = '" + txtBuse.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        Jik           = '" + txtJik.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        InDate        = '" + dtpInDate.Value.ToString("yyyy-MM-dd") + "', ";
                    SQL += ComNum.VBLF + "        TesaDate      = '" + txtTesaDate.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        JIKMU01       = '" + strJikmu01 + "', ";
                    SQL += ComNum.VBLF + "        JIKMU02       = '" + strJikmu02 + "', ";
                    SQL += ComNum.VBLF + "        JIKMU03       = '" + strJikmu03 + "', ";
                    SQL += ComNum.VBLF + "        JIKMU04       = '" + strJikmu04 + "', ";
                    SQL += ComNum.VBLF + "        JIKMU05       = '" + strJikmu05 + "', ";
                    SQL += ComNum.VBLF + "        JIKMU06       = '" + strJikmu06 + "', ";
                    SQL += ComNum.VBLF + "        JIKMU07       = '" + strJikmu07 + "', ";
                    SQL += ComNum.VBLF + "        JIKMU08       = '" + strJikmu08 + "', ";
                    SQL += ComNum.VBLF + "        JIKMU09       = '" + strJikmu09 + "', ";
                    SQL += ComNum.VBLF + "        JIKMU10       = '" + strJikmu10 + "', ";
                    SQL += ComNum.VBLF + "        JIKMU11       = '" + strJikmu11 + "', ";
                    SQL += ComNum.VBLF + "        MyenheNo      = '" + txtMyenhe.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        GBLTDUSER     = '" + strLtduser + "', ";
                    SQL += ComNum.VBLF + "        LTDCODE       =  " + nLtdCode + ", ";
                    SQL += ComNum.VBLF + "        ENTTIME = SYSDATE, ENTSABUN = " + FnJobSabun + " ";
                    SQL += ComNum.VBLF + "  WHERE LicNo         = '" + FstrLicno + "'";
                    SQL += ComNum.VBLF + "    AND IDNO          = " + nID + " ";
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
                SQL = SQL + ComNum.VBLF + "      Sabun, Name, Buse, Jik ";
                SQL = SQL + ComNum.VBLF + " FROM HIC_USERMST ";
                SQL = SQL + ComNum.VBLF + " WHERE Licno='" + FstrLicno + "' ";
                if (txtViewName.TextLength > 0) SQL = SQL + " AND Name LIKE '%" + txtViewName.Text + "%' ";
                if (chkTejik.Checked == true) SQL = SQL + " AND (TesaDate='' OR TesaDate IS NULL) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Name,Sabun ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.RowCount = dt.Rows.Count;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Sabun"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Buse"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Jik"].ToString().Trim();
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
                SQL += ComNum.VBLF + " DELETE FROM HIC_USERMST ";
                SQL += ComNum.VBLF + "  WHERE LicNo         = '" + FstrLicno + "'";
                SQL += ComNum.VBLF + "    AND Sabun         = " + nID + " ";
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
            DataTable dt = null;
            int i = 0;

            int nID = Int32.Parse(SS1.ActiveSheet.Cells[e.Row, 0].Value.ToString());

            Screen_Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM HIC_USERMST ";
                SQL = SQL + ComNum.VBLF + "Where Licno = '" + FstrLicno + "' ";
                SQL = SQL + ComNum.VBLF + "  And Sabun = " + nID + " ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    txtID.Text = dt.Rows[i]["Sabun"].ToString().Trim();
                    txtName.Text = dt.Rows[i]["Name"].ToString().Trim();
                    txtBuse.Text = dt.Rows[i]["Buse"].ToString().Trim();
                    txtJik.Text = dt.Rows[i]["Jik"].ToString().Trim();
                    txtMyenhe.Text = dt.Rows[i]["MyenheNo"].ToString().Trim();
                    dtpInDate.Text = dt.Rows[0]["InDate"].ToString().Trim();
                    txtTesaDate.Text = dt.Rows[0]["TesaDate"].ToString().Trim();
                    if (dt.Rows[0]["JIKMU01"].ToString().Trim() == "Y") chkJik01.Checked = true;
                    if (dt.Rows[0]["JIKMU02"].ToString().Trim() == "Y") chkJik02.Checked = true;
                    if (dt.Rows[0]["JIKMU03"].ToString().Trim() == "Y") chkJik03.Checked = true;
                    if (dt.Rows[0]["JIKMU04"].ToString().Trim() == "Y") chkJik04.Checked = true;
                    if (dt.Rows[0]["JIKMU05"].ToString().Trim() == "Y") chkJik05.Checked = true;
                    if (dt.Rows[0]["JIKMU06"].ToString().Trim() == "Y") chkJik06.Checked = true;
                    if (dt.Rows[0]["JIKMU07"].ToString().Trim() == "Y") chkJik07.Checked = true;
                    if (dt.Rows[0]["JIKMU08"].ToString().Trim() == "Y") chkJik08.Checked = true;
                    if (dt.Rows[0]["JIKMU09"].ToString().Trim() == "Y") chkJik09.Checked = true;
                    if (dt.Rows[0]["JIKMU10"].ToString().Trim() == "Y") chkJik10.Checked = true;
                    if (dt.Rows[0]["JIKMU11"].ToString().Trim() == "Y") chkJik11.Checked = true;
                }

                dt.Dispose();
                dt = null;

                if (nID > 1)
                {
                    삭제ToolStripMenuItem1.Visible = true;
                    if (clsType.User.Sabun == "admin") 비밀번호초기화ToolStripMenuItem.Visible = true;
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
                SQL += ComNum.VBLF + " UPDATE HIC_USERMST SET ";
                SQL += ComNum.VBLF + "        Password      = '" + strPass + "', ";
                SQL += ComNum.VBLF + "        PassChange    = ''  ";
                SQL += ComNum.VBLF + "  WHERE LicNo         = '" + FstrLicno + "'";
                SQL += ComNum.VBLF + "    AND Sabun         = " + nID + " ";
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
    }
}

