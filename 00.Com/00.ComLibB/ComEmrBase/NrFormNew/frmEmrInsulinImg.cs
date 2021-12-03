using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrInsulinImg : Form
    {
        public delegate void SendSelect(string Val);
        public event SendSelect rSendSelect;

        /// <summary>
        /// 피하주사순서
        /// </summary>
        public frmEmrInsulinImg()
        {
            InitializeComponent();
        }

        private void frmEmrInsulinImg_Load(object sender, EventArgs e)
        {
            foreach(Control control in Controls)
            {
                if (control is Label)
                {
                    control.Text = control.Name.Replace("lbl", "");
                    control.Font = new Font("맑은 고딕", 10f, FontStyle.Bold);
                    control.BackColor = Color.White;
                    control.Click += lbl_Click;
                }
            }
        }

        private void lbl_Click(object sender, EventArgs e)
        {
            rSendSelect((sender as Label).Text);
        }
    }
}
