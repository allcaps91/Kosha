using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmGFRCalc : Form
    {
        public frmGFRCalc()
        {
            InitializeComponent();
        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            double nCre;
            int nAge;
            double nWeight;
            double nHEIGHT;
            double nIBW;
            double nResult1 = 0;
            double nResult2 = 0;

            if (txtAge.Text.Trim() == "")
            {
                MessageBox.Show("나이가 공란입니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtAge.Focus();
                return;
            }

            nAge = int.Parse(txtAge.Text);
            if (txtWeight.Text.Trim() != "")
            {
                nWeight = double.Parse(txtWeight.Text);
            }
            else
            {
                nWeight = 0;
            }

            nIBW = 0;

            if (txtHeight.Text.Trim() == "" && nWeight == 0)
            {
                MessageBox.Show("신장이 공란입니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtHeight.Focus();
                return;
            }

            if (txtHeight.Text.Trim() == "")
            {
                nHEIGHT = 0;
            }
            else
            {
                nHEIGHT = double.Parse(txtHeight.Text);
            }

            if (nWeight > 0 && nHEIGHT > 0)
            {
                MessageBox.Show("실제체중과 신장 모두 입력시 계산 되지 않습니다!!" + "\r\n\r\n" + "신장(IBW) 혹은 실제체중만 입력 후 계산하십시오!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            nIBW = (nHEIGHT - 100) * 0.9;

            txtIBW.Text = nIBW.ToString();

            if (txtCreatinine.Text.Trim() == "")
            {
                MessageBox.Show("크레아티닌결과값 공란입니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCreatinine.Focus();
                return;
            }

            nCre = double.Parse(txtCreatinine.Text);

            if (txtIBW.Text.Trim() == "")
            {
                MessageBox.Show("IBW 공란 입니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtIBW.Focus();
                return;
            }

            if (rdoSex1.Checked == false && rdoSex2.Checked == false)
            {
                MessageBox.Show("성별 확인하세요!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (rdoSex1.Checked == true)
            {
                if (nWeight > 0 && nHEIGHT == 0)
                {
                    //남(140 - 나이 * 몸무게) / (72 * 크레아틴)
                    nResult1 = ((140 - nAge) * nWeight) / (72 * nCre);
                    txtIBW.Text = "";
                }
                else
                {
                    //남(140 - 나이 * 몸무게) / (72 * 크레아틴)
                    nResult1 = ((140 - nAge) * nIBW) / (72 * nCre);
                }
                nResult2 = 186 * (Math.Pow(nCre, -1.154)) * (Math.Pow(nAge, -0.203));
                //nResult2 = 175 * (Math.Pow(nCre, -1.154)) * (Math.Pow(nAge, -0.203));
            }
            else if (rdoSex2.Checked == true)
            {
                if (nWeight > 0 && nHEIGHT == 0)
                {
                    //여 ((140-나이 * 몸무게) / (72 * 크레아틴) ) * 0.85
                    nResult1 = ((140 - nAge) * nWeight) / (72 * nCre) * 0.85;
                    txtIBW.Text = "";
                }
                else
                {
                    //여 ((140-나이 * 몸무게) / (72 * 크레아틴) ) * 0.85
                    nResult1 = ((140 - nAge) * nIBW) / (72 * nCre) * 0.742;
                }

                nResult2 = 186 * (Math.Pow(nCre, -1.154)) * (Math.Pow(nAge, -0.203));
                //nResult2 = 175 * (Math.Pow(nCre, -1.154)) * (Math.Pow(nAge, -0.203));
            }

            nResult1 = Math.Round(nResult1, 1);
            nResult2 = Math.Round(nResult2, 1);

            txtCockcroft.Text = nResult1.ToString();
            txtMDRD.Text = nResult2.ToString();
        }

        private void frmGFRCalc_Load(object sender, EventArgs e)
        {

        }
    }
}
