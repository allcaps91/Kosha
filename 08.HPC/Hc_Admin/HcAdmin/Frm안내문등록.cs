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
    public partial class Frm안내문등록 : Form
    {

        public Frm안내문등록()
        {
            InitializeComponent();

            //서버접속
            if (clsDbMySql.DBConnect("115.68.23.223", "3306", "dhson", "@thsehdgml#", "dhson") == false)
            {
                ComFunc.MsgBox("라이선스 서버에 접속할 수 없습니다");
                Application.Exit();
            }
            Data_Display();
        }

        private void 닫기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string strTime = DateTime.Now.ToString("yyyy-MM-dd mm:ss");
            bool SqlErr;

            Cursor.Current = Cursors.WaitCursor;

            DataTable Dt = new DataTable();

            try
            {
                SQL = "";
                if (chkNew.Checked == true)
                {
                    SQL += ComNum.VBLF + " INSERT INTO LICMSG ";
                    SQL += ComNum.VBLF + "        (Remark, EntTime) ";
                    SQL += ComNum.VBLF + " VALUES ('" + txtRemark.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + strTime + "') ";
                }
                else
                {
                    SQL += ComNum.VBLF + " UPDATE LICMSG ";
                    SQL += ComNum.VBLF + "    SET Remark   = '" + txtRemark.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        EntTime  = '" + strTime + "' ";
                }
                SqlErr = clsDbMySql.ExecuteNonQuery(SQL);

                if (SqlErr == false)
                {
                    ComFunc.MsgBox("저장 실패", "알림");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ComFunc.MsgBox("저장되었습니다.", "알림");
                Cursor.Current = Cursors.Default;
                this.Close();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        private void Data_Display()
        {
            string SQL = "";
            DataTable dt = null;
            Cursor.Current = Cursors.WaitCursor;
            chkNew.Checked = true;

            try
            {
                SQL = "SELECT Remark FROM LICMSG";
                dt = clsDbMySql.GetDataTable(SQL);

                if (dt.Rows.Count > 0)
                {
                    chkNew.Checked = false;
                    txtRemark.Text = dt.Rows[0]["Remark"].ToString().Trim();
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

        private void Frm안내문등록_Load(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
