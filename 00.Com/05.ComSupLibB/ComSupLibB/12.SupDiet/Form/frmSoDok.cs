using System.Windows.Forms;

namespace ComSupLibB
{
    public partial class frmSoDok : Form
    {
        public delegate void SetHelpCode(string strHelpCode);
        public event SetHelpCode rSetHelpCode;

        string GstrHelpCode = "";
         
        public frmSoDok()
        { 
            InitializeComponent();
        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            GstrHelpCode = "YES";
            rSetHelpCode(GstrHelpCode);
            this.Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            GstrHelpCode = "NO";
            rSetHelpCode(GstrHelpCode);
            this.Close();
        }
    }
}
