using System;
using System.Windows.Forms;

namespace ComNurLibB
{
    public partial class frmRunVBForm : Form
    {
        string mstrJobFlag = "";

        public frmRunVBForm()
        {
            InitializeComponent();
        }

        public frmRunVBForm(string strJobFlag)
        {
            InitializeComponent();
            mstrJobFlag = strJobFlag;
        }

        private void frmRunVBForm_Load(object sender, EventArgs e)
        {
            switch (mstrJobFlag)
            {
                case "25"://	비치약품청구∙반환
                    //PSMHNROLD.clsNr_Old psmhNrOld25 = null;
                    //psmhNrOld25 = new PSMHNROLD.clsNr_Old();
                    //psmhNrOld25.DbCon();
                    //psmhNrOld25.ShowForm_FrmDrKeep(clsType.User.IdNumber);
                    //psmhNrOld25.DbDisCon();
                    //psmhNrOld25 = null;
                    break;
            }

            this.Close();
        }
    }
}
