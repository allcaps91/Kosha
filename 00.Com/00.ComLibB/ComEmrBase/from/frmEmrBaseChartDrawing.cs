using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using ComBase;

namespace ComEmrBase
{
    public partial class frmEmrBaseChartDrawing : Form
    {
        const int IMG_X1 = 55;
        const int IMG_X2 = 660;
        const int IMG_Y1 = 150;
        const int IMG_Y2 = 970;

        int mRow = -1;
        double mEMRNO = 0;
        string mFORMNAME = "";
        //string mPTNAME = "";
        string mCHARTDATE = "";
        string mMODE = "W";

        //Form Unload
        public delegate void EventClosed();
        public event EventClosed rEventClosed;
        //Delete
        public delegate void DeleteChartRemark(int Row, double pEMRNO);
        public event DeleteChartRemark rDeleteChartRemark;
        //Save
        public delegate void SaveChartRemark(int Row, double pEMRNO);
        public event SaveChartRemark rSaveChartRemark;

        public frmEmrBaseChartDrawing()
        {
            InitializeComponent();
        }

        public frmEmrBaseChartDrawing(int Row, double pEMRNO, string pFORMNAME, string pCHARTDATE, string pMODE )
        {
            InitializeComponent();
            mRow = Row;
            mEMRNO = pEMRNO;
            mFORMNAME = pFORMNAME;
            mCHARTDATE = pCHARTDATE;
            mMODE = pMODE;
        }

        private void frmEmrBaseChartDrawing_Load(object sender, EventArgs e)
        {
            lblDate.Text = ComFunc.FormatStrToDate(mCHARTDATE, "D");
            lblFormName.Text = mFORMNAME;
            //TODO : 주석제거해야함
            //ComFunc.DeleteFoldAll(@"C:\HealthSoft\\TabletImage\\");

            penCanvas.InitForm();
            LoadImage();
        }

        private void LoadImage()
        {
            Image[] ImageOrg = new Image[1];
            Image ImageOrg1 = Image.FromFile(@"C:\HealthSoft\FormToImage\" + mEMRNO.ToString() + ".tif", true);

            ImageOrg[0] = ImageOrg1;
            //ImageOrg[0] = CropImage(ImageOrg1, IMG_X1, IMG_Y1, IMG_X2, IMG_Y2, ImageOrg1.RawFormat);

            penCanvas.SetChartInfo(mEMRNO, @"C:\HealthSoft", ImageOrg, null, true);
            ImageOrg1.Dispose(); 
            ImageOrg1 = null;
            if (mMODE == "V")
            {
                penCanvas.setPageButton(1);
            }
        }

        /// <summary>
        /// Crops the image.
        /// </summary>
        private Image CropImage(Image img, int x1, int y1, int x2, int y2, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            return CropAndResizeImage(img, x2 - x1, y2 - y1, x1, y1, x2, y2, imageFormat);
        }

        /// <summary>
        /// Crops and resizes the image.
        /// </summary>
        private Image CropAndResizeImage(Image img, int targetWidth, int targetHeight, int x1, int y1, int x2, int y2, ImageFormat imageFormat)
        {
            try
            {

                var bmp = new Bitmap(targetWidth, targetHeight);
                Graphics g = Graphics.FromImage(bmp);

                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;

                int width = x2 - x1;
                int height = y2 - y1;

                g.DrawImage(img, new Rectangle(0, 0, targetWidth, targetHeight), x1, y1, width, height, GraphicsUnit.Pixel);
                //g.DrawImage(img, new Rectangle(x1, y1, targetWidth, targetHeight), x1, y1, width, height, GraphicsUnit.Pixel);
                g.Dispose();
                g = null;
                var memStream = new MemoryStream();
                bmp.Save(memStream, imageFormat);
                GC.Collect();  //GC 를 공격한다
                GC.WaitForFullGCComplete(-1);

                return Image.FromStream(memStream);
            }
            catch
            {
                return null;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            penCanvas.SaveDataAll();
            rSaveChartRemark(mRow, mEMRNO);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            rDeleteChartRemark(mRow, mEMRNO);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }
    }
}
