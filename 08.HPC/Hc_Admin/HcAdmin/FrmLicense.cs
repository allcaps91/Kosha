using ComBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace HcAdmin
{
    public partial class FrmLicense : Form
    {
        public bool FbNew = false;

        public FrmLicense()
        {
            InitializeComponent();

            FbNew = false;

            //서버접속
            if (clsDbMySql.DBConnect("115.68.23.223", "3306", "dhson", "@thsehdgml#", "dhson") == false)
            {
                ComFunc.MsgBox("라이선스 서버에 접속할 수 없습니다");
                Application.Exit();
            }
            SS1.ActiveSheet.ColumnCount = 3;
            List_Search();
        }

        private void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string strChkHic1 = "N";
            string strChkHic2 = "N";
            string strChkHic3 = "N";

            bool SqlErr;
            
            if (dptSDate.Text == "") { ComFunc.MsgBox("발급일자가 공란입니다."); dptSDate.Focus(); return; }
            if (txtLicCnt.Text == "") { ComFunc.MsgBox("라이선스 수량이 공란입니다."); txtLicCnt.Focus(); return; }
            if (int.Parse(txtLicCnt.Text) == 0) { ComFunc.MsgBox("라이선스 수량이 공란입니다."); txtLicCnt.Focus(); return; }
            if (txtAdminpass.Text == "") { ComFunc.MsgBox("관리자 비밀번호가 공란입니다."); txtAdminpass.Focus(); return; }
            if (check건강검진.Checked == true) strChkHic1 = "Y";
            if (check보건대행.Checked == true) strChkHic2 = "Y";
            if (check작업측정.Checked == true) strChkHic3 = "Y";

            Cursor.Current = Cursors.WaitCursor;

            DataTable Dt = new DataTable();

            try
            {
                SQL = "";
                if (FbNew == true)
                {
                    SQL += ComNum.VBLF + " INSERT INTO LICMST ";
                    SQL += ComNum.VBLF + "        (LicNo, AdminPass, SDate, EDate, LicCnt,";
                    SQL += ComNum.VBLF + "         chkHic1, chkHic2, chkHic3, Sangho,Juso,";
                    SQL += ComNum.VBLF + "         Damdang, Tel, EMail,DbConnect,Remark) ";
                    SQL += ComNum.VBLF + " VALUES ('" + txtLicno.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtAdminpass.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + dptSDate.Value.ToString("yyyy-MM-dd") + "', ";
                    SQL += ComNum.VBLF + "         '" + dptEDate.Value.ToString("yyyy-MM-dd") + "', ";
                    SQL += ComNum.VBLF + "         " + txtLicCnt.Text.Trim() + ", ";
                    SQL += ComNum.VBLF + "         '" + strChkHic1 + "', ";
                    SQL += ComNum.VBLF + "         '" + strChkHic2 + "', ";
                    SQL += ComNum.VBLF + "         '" + strChkHic3 + "', ";
                    SQL += ComNum.VBLF + "         '" + txtSangho.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtJuso.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtDamdang.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtTel.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtEmail.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtDbConnect.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtRemark.Text.Trim() + "') ";
                }
                else
                {
                    SQL += ComNum.VBLF + " UPDATE LICMST ";
                    SQL += ComNum.VBLF + "    SET AdminPass     = '" + txtAdminpass.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        SDate         = '" + dptSDate.Value.ToString("yyyy-MM-dd") + "', ";
                    SQL += ComNum.VBLF + "        EDate         = '" + dptEDate.Value.ToString("yyyy-MM-dd") + "', ";
                    SQL += ComNum.VBLF + "        LicCnt        = " + txtLicCnt.Text.Trim() + ", ";
                    SQL += ComNum.VBLF + "        chkHic1       = '" + strChkHic1 + "', ";
                    SQL += ComNum.VBLF + "        chkHic2       = '" + strChkHic2 + "', ";
                    SQL += ComNum.VBLF + "        chkHic3       = '" + strChkHic3 + "', ";
                    SQL += ComNum.VBLF + "        Sangho        = '" + txtSangho.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        Juso          = '" + txtJuso.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        Damdang       = '" + txtDamdang.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        Tel           = '" + txtTel.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        EMail         = '" + txtEmail.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        DbConnect     = '" + txtDbConnect.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        Remark        = '" + txtRemark.Text.Trim() + "'  ";
                    SQL += ComNum.VBLF + "  WHERE LicNo         = '" + txtLicno.Text.Trim() + "'";
                }
                SqlErr = clsDbMySql.ExecuteNonQuery(SQL);

                if (SqlErr == false)
                {
                    ComFunc.MsgBox("저장 실패", "알림");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                OciDb_AdminPass_Update();

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

        // 클라우드 서버에 관리자 기본정보 Update //
        private void OciDb_AdminPass_Update()
        {
            string SQL = "";
            string SqlErr = "";
            string strNewPass = "";
            DataTable dt = null;
            int intRowAffected = 0;
            int i = 0;

            strNewPass = clsAES.AES(txtAdminpass.Text.Trim());

            try
            {
                SQL = "";
                SQL = "SELECT * FROM HIC_USERMST ";
                SQL = SQL + ComNum.VBLF + "WHERE LicNo = '" + txtLicno.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND Sabun = 1 ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count == 0)
                {
                    SQL = "";
                    SQL = " INSERT INTO HIC_USERMST ";
                    SQL +=  "        (LicNo, SABUN, NAME, BUSE, JIK, INDATE, ";
                    SQL +=  "         JIKMU01, JIKMU02, JIKMU03, JIKMU04,JIKMU05,JIKMU06,";
                    SQL +=  "          Password, GBLTDUSER, LTDCODE, ENTTIME, ENTSABUN) ";
                    SQL +=  " VALUES ('" + txtLicno.Text.Trim() + "', 1,'관리자','관리자','관리자', ";
                    SQL +=  "         '" + dptSDate.Value.ToString("yyyy-MM-dd") + "', ";
                    SQL +=  "         'Y','Y','Y','Y','Y','Y',";
                    SQL +=  "         '" + strNewPass + "','N',0, ";
                    SQL +=  "         SYSDATE,1) ";
                }
                else
                {
                    SQL = "";
                    SQL =  " UPDATE HIC_USERMST ";
                    SQL += "    SET NAME          = '관리자', ";
                    SQL += "        Buse          = '관리자', ";
                    SQL +=  "       Jik           = '관리자', ";
                    SQL +=  "       InDate        = '" + dptSDate.Value.ToString("yyyy-MM-dd") + "', ";
                    SQL +=  "       TesaDate      = '', ";
                    SQL +=  "       GBLTDUSER     = 'N', ";
                    SQL +=  "       LTDCODE       =  0, ";
                    SQL +=  "       ENTTIME = SYSDATE, ENTSABUN = 1 ";
                    SQL +=  " WHERE LicNo         = '" + txtLicno.Text.Trim() + "'";
                    SQL +=  "   AND Sabun          = 1 ";
                }
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            List_Search();
        }

        private void List_Search()
        { 
            string SQL = "";
            DataTable dt = null;
            int i = 0;

            SS1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "      LicNo, Sangho, SDATE ";
                SQL = SQL + ComNum.VBLF + " FROM LICMST ";
                SQL = SQL + ComNum.VBLF + "Where SDATE >= '" + dtpViewSDate.Value.ToString("yyyy-MM-dd") + "' ";
                if (TxtViewSangho.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND Sangho LIKE '%" + TxtViewSangho.Text.Trim() + "%' ";
                }
                if (checkEnd.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND EDate >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY SDATE ";

                dt = clsDbMySql.GetDataTable(SQL);

                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.RowCount = dt.Rows.Count;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["LicNo"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Sangho"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SDATE"].ToString().Trim();
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

        private void Screen_Clear()
        {
            txtLicno.Text = "";
            txtSangho.Text = "";
            txtAdminpass.Text = "";
            dptSDate.Text = "";
            dptEDate.Text = "";
            txtLicCnt.Text = "";
            check건강검진.Checked = false;
            check보건대행.Checked = false;
            check작업측정.Checked = false;
            txtJuso.Text = "";
            txtDamdang.Text = "";
            txtEmail.Text = "";
            txtTel.Text = "";
            txtRemark.Text = "";
            txtDbConnect.Text = "";
            FbNew = false;
            삭제ToolStripMenuItem.Enabled = false;
        }

        private void FrmLicense_Load(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void SS1_CellClick_1(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            string strLicno = SS1.ActiveSheet.Cells[e.Row, 0].Value.ToString();

            string SQL = "";

            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM LICMST ";
                SQL = SQL + ComNum.VBLF + "Where Licno = '" + strLicno + "' ";

                dt = clsDbMySql.GetDataTable(SQL);

                if (dt.Rows.Count > 0)
                {

                    txtLicno.Text = strLicno;
                    txtAdminpass.Text = dt.Rows[0]["AdminPass"].ToString().Trim();
                    dptSDate.Text = dt.Rows[0]["SDate"].ToString().Trim();
                    dptEDate.Text = dt.Rows[0]["EDate"].ToString().Trim();
                    txtLicCnt.Text = dt.Rows[0]["LicCnt"].ToString().Trim();
                    if (dt.Rows[0]["chkHic1"].ToString().Trim() == "Y") check건강검진.Checked = true;
                    if (dt.Rows[0]["chkHic2"].ToString().Trim() == "Y") check보건대행.Checked = true;
                    if (dt.Rows[0]["chkHic3"].ToString().Trim() == "Y") check작업측정.Checked = true;
                    txtSangho.Text = dt.Rows[0]["Sangho"].ToString().Trim();
                    txtJuso.Text = dt.Rows[0]["Juso"].ToString().Trim();
                    txtDamdang.Text = dt.Rows[0]["Damdang"].ToString().Trim();
                    txtTel.Text = dt.Rows[0]["Tel"].ToString().Trim();
                    txtEmail.Text = dt.Rows[0]["EMail"].ToString().Trim();
                    txtDbConnect.Text = dt.Rows[0]["DbConnect"].ToString().Trim();
                    txtRemark.Text = dt.Rows[0]["Remark"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                FbNew = false;
                삭제ToolStripMenuItem.Enabled = true;
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

        private void 닫기ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 신규발급ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //                0123456789
            string strConv = "P583E96T2M";
            int[] rank = new int[] { 11, 8, 4, 10, 1, 6, 2, 5, 3, 7, 12, 9 };
            string strTime = DateTime.Now.ToString("yyyyMMddmmss");
            string strResult = "";
            int i = 0;

            for (i = 0; i < 12; i++)
            {
                int inx = int.Parse(strTime.Substring(rank[i] - 1, 1));
                string strChar = strConv.Substring(inx, 1);
                strResult = strResult + strChar;
                if (i == 3 || i == 7) strResult += "-";
            }
            Screen_Clear();

            삭제ToolStripMenuItem.Enabled = false;
            FbNew = true;
            txtLicno.Text = strResult;
            dptSDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

        }

        private void 삭제ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            bool SqlErr;

            if (FbNew == true) return;

            if (ComFunc.MsgBoxQ("삭제하시겠습니까") == DialogResult.No) return;

            Cursor.Current = Cursors.WaitCursor;

            DataTable Dt = new DataTable();

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " DELETE FROM LICMST ";
                SQL += ComNum.VBLF + "  WHERE LicNo = '" + txtLicno.Text.Trim() + "'";

                SqlErr = clsDbMySql.ExecuteNonQuery(SQL);

                if (SqlErr == false)
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
    }
}
