using System;
using System.Windows.Forms;

namespace ComBase
{
    /// <summary>
    /// Class Name      : MedIpdNr
    /// File Name       : frmPainScore3.cs
    /// Description     : NIPS(Neonatalinfantpain)
    /// Author          : 박창욱
    /// Create Date     : 2018-02-08
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrinfo\Frm통증스코어3.frm(Frm통증스코어3.frm) >> frmPainScore3.cs 폼이름 재정의" />	
    public partial class frmPainScore3 : Form
    {
        public delegate void SetHelpCode(string strHelpCode);
        public event SetHelpCode rSetHelpCode;

        string GstrHelpCode = "";

        public frmPainScore3()
        {
            InitializeComponent();
        }

        public frmPainScore3(string strHelpCode)
        {
            InitializeComponent();
            this.GstrHelpCode = strHelpCode;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            GstrHelpCode = txtJumsu.Text.Trim();
            rSetHelpCode(GstrHelpCode);
            this.Close();
        }

        private void frmPainScore3_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtJumsu.Text = "";

            for (int i = 1; i < 6; i++)
            {
                ssView_Sheet1.Cells[i - 1, 5].Text = "";
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            int nScore = 0;
            int nTOT = 0;

            if (e.Column < 1 || e.Row < 0 || e.Column > 3)
            {
                return;
            }

            nScore = e.Column - 1;

            ssView_Sheet1.Cells[e.Row, 5].Text = nScore.ToString();

            for (i = 1; i < 7; i++)
            {
                nTOT += (int)VB.Val(ssView_Sheet1.Cells[i - 1, 5].Text);
            }

            txtJumsu.Text = nTOT.ToString();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
