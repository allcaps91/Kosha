using ComBase;
using System;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class FrmMedDurCheckCancel : Form
    {
        string FstrDate;
        string FstrcPrscAdmSym;
        string FstrcGrantNo;

        clsOrdFunction OF = new clsOrdFunction();

        public FrmMedDurCheckCancel()
        {
            InitializeComponent();
        }

        public FrmMedDurCheckCancel(string strDate, string strcPrscAdmSym, string strcGrantNo)
        {
            InitializeComponent();

            FstrDate = strDate;
            FstrcPrscAdmSym = strcPrscAdmSym;
            FstrcGrantNo = strcGrantNo;
        }


        private void FrmMedDurCheckCancel_Load(object sender, EventArgs e)
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            cReasonCd.SelectedIndex = 0;

            cPrscDd.Text = FstrDate;
            cPrscAdmSym.Text = FstrcPrscAdmSym;
            cGrantNo.Text = FstrcGrantNo;
        }

        private void btnCheckCancel_Click(object sender, EventArgs e)
        {
            //clsOrdFunction.bOK = true;
            clsOrdFunction.GstrDurCancelGrantNo = cGrantNo.Text;     //DUR 점검취소 처방전 교부번호
            clsOrdFunction.GstrDurPrscAdmSym = cPrscAdmSym.Text;     //DUR 점검취소 처방전발행기관기호
            clsOrdFunction.GstrDurCheckCancel = true;                //DUR 점검취소 여부
            clsOrdFunction.GstrDurCancelReasonCd = cReasonCd.Text;   //DUR 점검취소 사유코드
            clsOrdFunction.GstrDurCancelPrscDd = cPrscDd.Text;       //DUR 점검취소 처방일자
            clsOrdFunction.GstrDurCancelReasonMsg = cReasonMsg.Text; //DUR 점검취소 사유
            this.Close();
        }

        private void cReasonCd_Click(object sender, EventArgs e)
        {
            cReasonMsg.Enabled = false;
            cReasonMsg.Text = "";
            if (VB.Left(cReasonCd.Text, 2) == "MT")
            {
                cReasonMsg.Enabled = true;
            }
            else
            {
                cReasonMsg.Text = VB.Mid(cReasonCd.Text, 0, VB.InStr(cReasonCd.Text, ":") + 1).Trim();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clsOrdFunction.GstrDurCancelReasonCd = "";
            this.Close();
        }
    }
}
