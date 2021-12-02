using ComBase;
using System;
using System.Windows.Forms;

namespace ComMedLibB
{
    public partial class frmMedDrugRepeateSayu : Form
    {
        public delegate void SendText(string strText);
        public event SendText rSendText;

        public frmMedDrugRepeateSayu()
        {
            InitializeComponent();
        }

        void rdoGu2_CheckedChanged(object sender, EventArgs e)
        {
            if(rdoGu2.Checked == true)
            {
                txtEtcSayu.Enabled = true;
                txtEtcSayu.Focus();
            }
            else
            {
                txtEtcSayu.Enabled = false;
                txtEtcSayu.Text = "";
            }
        }

        void frmMedDrugRepeateSayu_Load(object sender, EventArgs e)
        {
            txtEtcSayu.Enabled = false;
            txtEtcSayu.Text = "";
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            if(rdoGu0.Checked == false && rdoGu1.Checked == false && rdoGu2.Checked == false)
            {
                ComFunc.MsgBox("추가처방 사유를 선택해주세요.");
                return;
            }

            if (rdoGu0.Checked == true)
            {
                rSendText("1");
            }
            else if (rdoGu1.Checked == true)
            {
                rSendText("2");
            }
            else if (rdoGu2.Checked == true)
            {
                rSendText(txtEtcSayu.Text.Trim());
            }

            this.Close();
            return;

        }
    }
}
