using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmMSG_BEDSORE : Form
    {
        public frmMSG_BEDSORE()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmMSG_BST_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }
    }
}
