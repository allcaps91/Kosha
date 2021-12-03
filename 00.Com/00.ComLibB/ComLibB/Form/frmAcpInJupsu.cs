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

namespace ComLibB
{
    public partial class frmAcpInJupsu : Form
    {
        public frmAcpInJupsu()
        {
            InitializeComponent();
        }

        private void panTitle_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblCan_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAcpInJupsu_Resize(object sender, EventArgs e)
        {
            
        }

        private void frmAcpInJupsu_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등
        }
    }
}
