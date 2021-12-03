using System;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmSupJaewonManagement : Form
    {
        public frmSupJaewonManagement()
        {
            InitializeComponent();
        }

        private void frmSupJaewonManagement_Load(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
