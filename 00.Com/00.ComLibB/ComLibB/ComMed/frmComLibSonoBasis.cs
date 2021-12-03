using ComBase;
using System;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmComLibSonoBasis : Form
    {
        public frmComLibSonoBasis()
        {
            InitializeComponent();
        }

        private void frmComLibSonoBasis_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
