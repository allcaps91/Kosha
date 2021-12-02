using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrVitalSignSaveError : Form
    {
        string mError = "";

        public frmEmrVitalSignSaveError()
        {
            InitializeComponent();
        }

        public frmEmrVitalSignSaveError(string pError)
        {
            InitializeComponent();

            mError = pError;
        }

        private void frmEmrVitalSignSaveError_Load(object sender, EventArgs e)
        {
            txtError.Text = "";
            txtError.Text = mError;
        }
    }
}
