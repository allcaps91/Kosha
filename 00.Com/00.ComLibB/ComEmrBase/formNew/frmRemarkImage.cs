using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmRemarkImage : Form
    {
        object image = null;
        public frmRemarkImage(object img)
        {
            image = img;
            InitializeComponent();
        }

        private void frmRemarkImage_Load(object sender, EventArgs e)
        {
            if (image is Image)
            {
                PictureBox pic = new PictureBox();
                pic.Size = (image as Image).Size;
                pic.Image = image as Image;
                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.Dock = DockStyle.Top;
                pic.Parent = panImg;
                panImg.Controls.Add(pic);
            }
            else
            {
                Close();
            }

        }
    }
}
