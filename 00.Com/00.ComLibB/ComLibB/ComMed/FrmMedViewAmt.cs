using ComBase;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class FrmMedViewAmt : Form
    {
        string strGbInfo;

        public FrmMedViewAmt()
        {
            InitializeComponent();
        }

        public FrmMedViewAmt(string sGbInfo)
        {
            InitializeComponent();

            strGbInfo = sGbInfo;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //if (VB.IsNumeric(txtInput.Text) == true)
            {
                if (txtInput.Text.Trim() == "")
                {
                    if (MessageBox.Show("금액을 산정하지 않으시겠습니까?", "금액산정여부", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        clsOrdFunction.GstrSELECTGbInfo = "";
                        this.Close();
                        return;
                    }
                    else
                    {
                        txtInput.Focus();
                        return;
                    }
                }
                clsOrdFunction.GstrSELECTGbInfo = "AMT" + txtInput.Text.Trim();
            }
            

            this.Close();
        }

        private void FrmMedViewAmt_Load(object sender, EventArgs e)
        {
            this.Location = new Point(300, 300);

            if (VB.Left(strGbInfo, 3) == "AMT")
            {
                txtInput.Text = VB.Mid(strGbInfo, 4, strGbInfo.Length);
                //ComFunc.StartLen(txtInput);
            }
        }

        private void txtInput_Click(object sender, EventArgs e)
        {
            //ComFunc.StartLen(txtInput);
        }        

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            if (txtInput.Text.Trim() != "")
            {
                txtInput.Text = string.Format("{0:###,###,###}", long.Parse(txtInput.Text.Replace(",", "")));
                txtInput.SelectionStart = txtInput.Text.Length + 1;
            }
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            string strAmt = "";

            if (txtInput.Text.Trim() == "") return;
             
            if (e.KeyCode == Keys.Enter)
            {
                strAmt = string.Format("{0:###,###,##0}", long.Parse(txtInput.Text.Replace(",", "")));
                //if (strAmt.Trim() == txtInput.Text.Trim())
                //{
                //    return;
                //}
                txtInput.Text = strAmt;
                txtInput.SelectionStart = txtInput.Text.Length + 1;

                btnOk_Click(btnOk, new EventArgs());
            }
        }
    }
}
