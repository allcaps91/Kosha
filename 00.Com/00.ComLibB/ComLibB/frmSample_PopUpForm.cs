using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmSample_PopUpForm : Form
    {
        /// <summary>
        /// 샘플폼 디자인
        /// </summary>
        public frmSample_PopUpForm()
        {
            InitializeComponent();
        }

        private void frmSample_PopUpForm_Load(object sender, EventArgs e)
        {

        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.ShowDialog();
        }
    }
}
