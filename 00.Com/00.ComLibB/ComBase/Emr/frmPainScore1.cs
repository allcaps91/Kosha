using System;
using System.Windows.Forms;

namespace ComBase
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-02-21
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= d:\psmh\nurse\nrinfo\Frm통증스코어1.frm" >> frmPainScore1.cs 폼이름 재정의" />

    public partial class frmPainScore1 : Form
    {
        public delegate void SetHelpCode(string strHelpCode, string strHelpName);
        public event SetHelpCode rSetHelpCode;

        public delegate void EventClose();
        public event EventClose rEventClose;

        public frmPainScore1()
        {
            InitializeComponent();
        }

        private void NRS0_Click(object sender, EventArgs e)
        {
            rSetHelpCode("1", "0");
            this.Close();
        }

        private void NRS1_Click(object sender, EventArgs e)
        {
            rSetHelpCode("1", "1");
            this.Close();
        }

        private void NRS2_Click(object sender, EventArgs e)
        {
            rSetHelpCode("1", "2");
            this.Close();
        }

        private void NRS3_Click(object sender, EventArgs e)
        {
            rSetHelpCode("1", "3");
            this.Close();
        }

        private void NRS4_Click(object sender, EventArgs e)
        {
            rSetHelpCode("1", "4");
            this.Close();
        }

        private void NRS5_Click(object sender, EventArgs e)
        {
            rSetHelpCode("1", "5");
            this.Close();
        }

        private void NRS6_Click(object sender, EventArgs e)
        {
            rSetHelpCode("1", "6");
            this.Close();
        }

        private void NRS7_Click(object sender, EventArgs e)
        {
            rSetHelpCode("1", "7");
            this.Close();
        }

        private void NRS8_Click(object sender, EventArgs e)
        {
            rSetHelpCode("1", "8");
            this.Close();

        }

        private void NRS9_Click(object sender, EventArgs e)
        {
            rSetHelpCode("1", "9");
            this.Close();

        }

        private void NRS10_Click(object sender, EventArgs e)
        {
            rSetHelpCode("1", "10");
            this.Close();
        }

        private void VAS01_Click(object sender, EventArgs e)
        {
            rSetHelpCode("2", "0");
            this.Close();
        }

        private void VAS02_Click(object sender, EventArgs e)
        {
            rSetHelpCode("2", "2");
            this.Close();
        }

        private void VAS03_Click(object sender, EventArgs e)
        {
            rSetHelpCode("2", "4");
            this.Close();
        }

        private void VAS04_Click(object sender, EventArgs e)
        {
            rSetHelpCode("2", "6");
            this.Close();
        }

        private void VAS05_Click(object sender, EventArgs e)
        {
            rSetHelpCode("2", "8");
            this.Close();
        }

        private void VAS06_Click(object sender, EventArgs e)
        {
            rSetHelpCode("2", "10");
            this.Close();
        }

        private void VAS11_Click(object sender, EventArgs e)
        {
            rSetHelpCode("2", "0");
            this.Close();

        }

        private void VAS12_Click(object sender, EventArgs e)
        {
            rSetHelpCode("2", "2");
            this.Close();
        }

        private void VAS13_Click(object sender, EventArgs e)
        {
            rSetHelpCode("2", "4");
            this.Close();
        }

        private void VAS14_Click(object sender, EventArgs e)
        {
            rSetHelpCode("2", "6");
            this.Close();
        }

        private void VAS15_Click(object sender, EventArgs e)
        {
            rSetHelpCode("2", "8");
            this.Close();
        }

        private void VAS16_Click(object sender, EventArgs e)
        {
            rSetHelpCode("2", "10");
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
