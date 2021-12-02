using System;
using System.Linq;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;

namespace ComEmrBase
{
    public partial class frmEmrIVRate : Form
    {
        public frmEmrIVRate()
        {
            InitializeComponent();
        }

        private void frmEmrIVRate_Load(object sender, EventArgs e)
        {
            rdoCalc1.Checked = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtClear();
        }

        private void txtClear()
        {
            foreach(TextBox text in panel2.Controls.OfType<TextBox>())
            {
                text.Clear();
            }

            foreach (TextBox text in panel5.Controls.OfType<TextBox>())
            {
                text.Clear();
            }
        }

        private void chkWeight_CheckedChanged(object sender, EventArgs e)
        {
            txtWeight.Visible = chkWeight.Checked;
            cboWeight.Visible = chkWeight.Checked;
            lblDose.Text = chkWeight.Checked ? "/kg/" : "/";
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            double dblRate = (VB.Val(txtDose.Text) * (chkWeight.Checked ?  VB.Val(txtWeight.Text) : 1) * (cboDoseTime.Text.Equals("hr") ? 1 : 60)) / ((VB.Val(txtConcentration1_1.Text) / VB.Val(txtConcentration1_2.Text)) * 1000);
            txtIVRate.Text = dblRate.ToString("N1");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void rdoCalc1_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                if (sender.Equals(rdoCalc1))
                {
                    lblInfo.Text = "IV Rate";
                    panIV.Visible = true;
                    panDose.Visible = false;
                    label6.Visible = true;

                    cboDoses.Visible = false;
                    cboDoses2.Visible = false;
                    lblKg.Visible = false;
                }
                else
                {
                    lblInfo.Text = "Dose";
                    panIV.Visible = false;
                    panDose.Visible = true;

                    cboDoses.Visible = true;
                    cboDoses2.Visible = true;
                    lblKg.Visible = true;
                }
            }
            
        }

        private void btnCalc2_Click(object sender, EventArgs e)
        {
            double tInf = 0;
            double cDose = VB.Val(txtConcentration2_1.Text);
            double cVol = VB.Val(txtConcentration2_2.Text);

            if (cboDoseTime2.Text.Equals("cc/min")) { tInf = 1; }

            if (cboDoseTime2.Text.Equals("cc/hr")) { tInf = 60; }

            if (cboDoseTime2.Text.Equals("cc/day")) { tInf = 60 * 24; }

            if (cboConcentration2.Text.Equals("nanograms")) { cDose /= (1000 * 1000); }

            if (cboConcentration2.Text.Equals("mcg")) { cDose /= 1000; }

            if (cboConcentration2.Text.Equals("grams")) { cDose *= 1000; }

            decimal fDose =((txtDose2.Text).To<decimal>() * ((cDose / cVol).To<decimal>()) / ((chkWeight2.Checked ? VB.Val(txtWeight2.Text) : 1) * tInf).To<decimal>()).To< decimal>();
            decimal doseVal = fDose;
            
            if (cboDoses.Text.Equals("ng"))
            {
                doseVal *= (1000 * 1000);
            }

            if (cboDoses.Text.Equals("mcg"))
            {
                doseVal *= 1000;
            }

            if (cboDoses.Text.Equals("grams"))
            {
                doseVal /= 1000;
            }


            if (cboDoses2.Text.Equals("hr"))
            {
                doseVal *= 60;
            }

            if (cboDoses2.Text.Equals("day"))
            {
                doseVal *= (60 * 24);
            }

            //txtIVRate.Text = doseVal.ToString();
            txtIVRate.Text = roundNum(doseVal, 1).ToString("N1");
        }

        private decimal roundNum(decimal thisNum, int dec)
        {
            thisNum *= Math.Pow(10, dec).To<decimal>();

            thisNum = Math.Round(thisNum).To<decimal>();

            thisNum /= Math.Pow(10, dec).To<decimal>();

            return thisNum;
        }

        private void chkWeight2_CheckedChanged(object sender, EventArgs e)
        {
            txtWeight2.Visible = chkWeight2.Checked;
            cboWeight2.Visible = chkWeight2.Checked;

            cboDoses.Visible = chkWeight2.Checked;
            cboDoses2.Visible = chkWeight2.Checked;
            lblKg.Visible = chkWeight2.Checked;
        }

        private void cboDoses_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCalc2.PerformClick();
        }

        private void cboDose_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCalc.PerformClick();
        }
    }
}
