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

namespace HcAdmin
{
    public partial class FrmMainform : Form
    {
        public FrmMainform()
        {
            InitializeComponent();
            닫기ToolStripMenuItem.Enabled = false;
            라이선스ToolStripMenuItem.Enabled = false;
            안내문등록ToolStripMenuItem.Enabled = false;
            DoLogin_Cloud();
        }

        private void DoLogin_Cloud()
        {
            //TODO : 커넥션 교체하기전까지 막음
            clsDB.GetDbInfo();
            clsDB.DbCon = clsDB.DBConnect_Cloud();
            clsCompuInfo.SetComputerInfo();

            if (clsDB.DbCon == null)
            {
                Application.Exit();
                return;
            }

        }

        private void FrmMainform_Load(object sender, EventArgs e)
        {

        }

        private void 닫기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 라이선스ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLicense form = new FrmLicense();
            form.Show();
        }

        private void CmdLogin_Click_1(object sender, EventArgs e)
        {
            if (TxtPass.Text.Trim() == "0542894349")
            {
                닫기ToolStripMenuItem.Enabled = true;
                라이선스ToolStripMenuItem.Enabled = true;
                안내문등록ToolStripMenuItem.Enabled = true;
                panLogin.Visible = false;
            }
            else
            {
                ComFunc.MsgBox("관리자 비빌번호를 확인하세요");
            }

        }

        private void CmdExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 안내문등록ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm안내문등록 form = new Frm안내문등록();
            form.Show();
        }
    }
}
