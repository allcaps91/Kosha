using System;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmEmrFluidCalc : Form
    {
        public frmEmrFluidCalc()
        {
            InitializeComponent();
        }

        private void frmEmrFluidCalc_Load(object sender, EventArgs e)
        {

        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            //시간당 용량
            double HourCC = VB.Val(txtFluid.Text) / VB.Val(txtHour.Text);
            double Minutegtt = (VB.Val(txtFluid.Text) * 20) / (VB.Val(txtHour.Text) * 60);
            double Secondgtt = (VB.Val(txtHour.Text) * 60 * 60) / (VB.Val(txtFluid.Text) * 20);

            txtResult.Text = "시간당 주입량(ml/hr)  : " + string.Format("{0:f2}", HourCC);
            txtResult.Text += ComNum.VBLF + "분당 방울 수          : " + string.Format("{0:f2}", Minutegtt);
            txtResult.Text += ComNum.VBLF + "1방울 점적에 걸리는 초 : " + string.Format("{0:f2}", Secondgtt);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
