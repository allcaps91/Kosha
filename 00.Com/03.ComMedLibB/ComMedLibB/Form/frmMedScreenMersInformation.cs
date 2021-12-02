using System;
using System.Windows.Forms;

namespace ComMedLibB
{
    public partial class frmMedScreenMersInformation : Form
    {        
        public frmMedScreenMersInformation(string argNotice, string argMessage)
        {
            InitializeComponent();

            this.Text = "점검 결과";
            lblTitle.Text = argMessage;
            txtNotice.Text = argNotice;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
