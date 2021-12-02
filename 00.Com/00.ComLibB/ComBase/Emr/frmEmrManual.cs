using System;
using System.Windows.Forms;

namespace ComBase
{
    public partial class frmEmrManual : Form
    {
        public frmEmrManual()
        {
            InitializeComponent();
        }

        private void FrmEmrManual_Load(object sender, EventArgs e)
        {
            GetPdf();
        }

        void GetPdf()
        {
            using (Ftpedt FtpedtX = new Ftpedt())
            {
                string strFile = @"C:\PSMHEXE\Emr_Manul.pdf";
                string strHost = "/psnfs/psmhexe/manual";
                string strHostFile = "Manual_Emr_Doctor.pdf";

                if (FtpedtX.FtpDownload("192.168.100.35", "pcnfs", "pcnfs1", strFile, strHostFile, strHost) == true)
                {
                    //WB.Url = new Uri("file:///" + strFile);
                    //WB.Navigate("file:///" + strFile);
                    System.Diagnostics.Process.Start(strFile);
                }
            }

        }
    }
}
