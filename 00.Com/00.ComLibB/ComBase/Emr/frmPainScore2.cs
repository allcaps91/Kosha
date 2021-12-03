using System;
using System.Windows.Forms;

namespace ComBase
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-02-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= d:\psmh\nurse\nrinfo\Frm통증스코어2.frm" >> frmPainScore2.cs 폼이름 재정의" />

    public partial class frmPainScore2 : Form
    {
        public delegate void SetHelpCode(string strHelpCode);
        public event SetHelpCode rSetHelpCode;

        public frmPainScore2()
        {
            InitializeComponent();
        }

        private void frmPainScore2_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;

            TxtJumsu.Text = "";

            for (i = 0; i <= 4; i++)
            {
                SS1_Sheet1.Cells[i, 5].Text = "";
            }
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            int nScore = 0;
            int nTOT = 0;

            if (e.Column < 1 || e.Row < 0 || e.Column > 3)
            {
                return;
            }

            nScore = e.Column - 1;

            SS1_Sheet1.Cells[e.Row, 5].Text = nScore.ToString();

            for (i = 0; i <= 4; i++)
            {
                nTOT = nTOT + (int)VB.Val(SS1_Sheet1.Cells[i, 5].Text);
            }

            TxtJumsu.Text = nTOT.ToString();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            rSetHelpCode(TxtJumsu.Text.Trim());
            this.Close();
        }
    }
}
