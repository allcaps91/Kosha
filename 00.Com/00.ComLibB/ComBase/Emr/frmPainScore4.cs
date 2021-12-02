using System;
using System.Windows.Forms;

namespace ComBase
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-02-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= d:\psmh\nurse\nrinfo\Frm통증스코어4.frm" >> frmPainScore4.cs 폼이름 재정의" />

    public partial class frmPainScore4 : Form
    {
        public delegate void SetHelpCode(string strHelpCode);
        public event SetHelpCode rSetHelpCode;

        public frmPainScore4()
        {
            InitializeComponent();
        }

        private void frmPainScore4_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;

            TxtJumsu.Text = "";

            for (i = 0; i <= 13; i++)
            {
                SS1_Sheet1.Cells[i, 5].Text = "";
            }
        }

        private void SS1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            int nScore = 0;
            int nTOT = 0;

            if (e.Column < 1 || e.Row < 1 || e.Column > 3 || e.Row == 10)
            {
                return;
            }

            SS1_Sheet1.Cells[e.Row, 5].Text = "";
            nScore = (int)VB.Val(SS1_Sheet1.Cells[e.Row, 3].Text);

            if (e.Row < 4)
            {
                SS1_Sheet1.Cells[1, 5].Text = nScore.ToString();
            }
            else if (e.Row < 7)
            {
                //nScore = (int)VB.Val(SS1_Sheet1.Cells[4, 5].Text);
                SS1_Sheet1.Cells[4, 5].Text = nScore.ToString();

            }
            else if (e.Row < 10)
            {
                //nScore = (int)VB.Val(SS1_Sheet1.Cells[7, 5].Text);
                SS1_Sheet1.Cells[7, 5].Text = nScore.ToString();
            }
            else
            {
                //nScore = (int)VB.Val(SS1_Sheet1.Cells[11, 5].Text);
                SS1_Sheet1.Cells[11, 5].Text = nScore.ToString();
            }

            for (i = 0; i <= 13; i++)
            {
                nTOT = nTOT + (int)VB.Val(SS1_Sheet1.Cells[i, 5].Text);
            }

            TxtJumsu.Text = nTOT.ToString();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            rSetHelpCode(TxtJumsu.Text.Trim());
            this.Close();
        }
    }
}
