using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class ucImageCon : UserControl
    {
        public ucImageCon()
        {
            InitializeComponent();
        }

        private void ucImageCon_Load(object sender, EventArgs e)
        {

        }

        private void panHead_Click(object sender, EventArgs e)
        {
            if (panHead.BackColor == Color.LightGray)
            {
                panHead.BackColor = Color.LightPink;
                lblName.BackColor = Color.LightPink;
                this.BackColor = Color.LightPink;
                picImage.BackColor = Color.LightPink;
            }
            else
            {
                panHead.BackColor = Color.LightGray;
                lblName.BackColor = Color.LightGray;
                this.BackColor = Color.LightGray;
                picImage.BackColor = Color.LightGray;
            }
        }

        private void lblName_Click(object sender, EventArgs e)
        {
            if (lblName.BackColor == Color.LightGray)
            {
                lblName.BackColor = Color.LightPink;
                panHead.BackColor = Color.LightPink;
                this.BackColor = Color.LightPink;
                picImage.BackColor = Color.LightPink;
            }
            else
            {
                lblName.BackColor = Color.LightGray;
                panHead.BackColor = Color.LightGray;
                this.BackColor = Color.LightGray;
                picImage.BackColor = Color.LightGray;
            }
        }

        private void picImage_Click(object sender, EventArgs e)
        {
            if (panHead.BackColor == Color.LightGray)
            {
                panHead.BackColor = Color.LightPink;
                lblName.BackColor = Color.LightPink;
                this.BackColor = Color.LightPink;
                picImage.BackColor = Color.LightPink;
            }
            else
            {
                panHead.BackColor = Color.LightGray;
                lblName.BackColor = Color.LightGray;
                this.BackColor = Color.LightGray;
                picImage.BackColor = Color.LightGray;
            }
        }

    }
}
