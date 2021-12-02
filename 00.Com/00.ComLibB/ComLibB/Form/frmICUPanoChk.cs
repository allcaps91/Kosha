using System;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmICUPanoChk
    /// File Name : frmICUPanoChk.cs
    /// Title or Description : 병록번호 검증 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-02
    /// <history> 
    /// </history>
    /// </summary>
    public partial class frmPanoChk : Form
    {
        public frmPanoChk()
        {
            InitializeComponent();
        }

        private void frmPanoChk_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            txtPano.Focus();
            lblPano.Text = "";
            txtPano.Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPano_Enter(object sender, EventArgs e)
        {
            txtPano.SelectionStart = 0;
            txtPano.SelectionLength = txtPano.Text.Length;
        }

        private void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                ChkDGT_CALC();
                txtPano.Text = "";
                txtPano.Focus();
            }
        }

        private void ChkDGT_CALC()
        {
            string strPano = "";
            int result = 0;
            int i = 0;
            int j = 0;
            int Sum = 0;
            int na = 0;
            int mok = 0;

            if (txtPano.Text == null || txtPano.Text == "")
            {
                return;
            }

            strPano = String.Format("{0:0000000}", txtPano.Text);
            
            Int32.TryParse(txtPano.Text, out result);
            if (result == 0)
            {
                lblPano.Text = "ERROR";
                return;
            }
            
            i = 7;
            for (j = 0; j <= 6; j++) 
            {
                Sum = Sum + (Int32.Parse(strPano.Substring(j, 1)) * i);
                i -= 1;
            }
            
            mok = Sum / 11;
            na = Sum - (mok * 11);
            na = 11 - na;

            if (na == 10 || na == 11) 
            {
                na = 0;
            }

            lblPano.Text = strPano + "-" + String.Format("{0:0}", na);
        }
    }
}
