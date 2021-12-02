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
    public partial class frmMibiPOPUP : Form
    {
        public frmMibiPOPUP()
        {
            InitializeComponent();
        }

        private void btnidentify_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
