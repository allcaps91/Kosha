using ComBase;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmScanPageView : Form
    {
        string strImagePath = string.Empty;
        private Form mCallForm = null;

        //public delegate void CloseEvent();
        //public event CloseEvent rClosed;

        public frmScanPageView(Form mCallForm, string strPath)
        {
            this.mCallForm = mCallForm;
            strImagePath = strPath;
            InitializeComponent();
        }

        private void frmScanPageView_Load(object sender, System.EventArgs e)
        {
            MoveImgView();
        }

        /// <summary>
        /// 이미지 중앙 이동
        /// </summary>
        void MoveImgView()
        {
            Image BackImage = GetImage(strImagePath);

            Screen screen = Screen.FromControl(mCallForm);

            double horzRatio = (double)screen.WorkingArea.Width / BackImage.Width;
            double vertRatio = (double)screen.WorkingArea.Height / BackImage.Height;

            double ratio = horzRatio < vertRatio ? horzRatio : vertRatio;

            picBig.Width = (int)(BackImage.Width * ratio);
            picBig.Height = (int)(BackImage.Height * ratio) - 60;

            picBig.Left = (mCallForm.Width) - picBig.Width / 2;
            picBig.Top = (mCallForm.Height) - picBig.Height / 2;

            Width = (int)(BackImage.Width * ratio);
            Height = (int)(BackImage.Height * ratio);

            Left = screen.WorkingArea.X;
            Top = 0;

            if (picBig.Width == 1280 && picBig.Height == 1024)
            {
            }
            else
            {
                Top += (screen.WorkingArea.Height - mCallForm.Height) / 2;
            }

            picBig.SizeMode = PictureBoxSizeMode.Zoom;
            picBig.Visible = true;
            picBig.Image = BackImage;
        }

        /// <summary>
        /// Image 불러오기
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        private Image GetImage(string strFileName)
        {
            Image rtnVal = null;

            if (strFileName.IndexOf(".env") != -1)
            {
                rtnVal = clsCyper.DecryptImage(strFileName);
            }
            else
            {
                byte[] buff = File.ReadAllBytes(strFileName);
                using (MemoryStream ms = new MemoryStream(buff))
                {
                    rtnVal = Image.FromStream(ms);
                }
            }

            return rtnVal;
        }


        private void picBig_MouseUp(object sender, MouseEventArgs e)
        {
            Close();
        }
    }
}