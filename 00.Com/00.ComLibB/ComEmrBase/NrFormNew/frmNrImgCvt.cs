using ComBase;
using ComBase.Controls;
using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmNrImgCvt : Form
    {
        string TreatNo = string.Empty;

        public frmNrImgCvt(string TreatNo)
        {
            this.TreatNo = TreatNo;
            InitializeComponent();
        }

        private void frmNrImgCvt_Load(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd") + "날짜에 검사/영상 변환된 기록이 없거나\r\n마지막 변환 후 발생된 결과가 있습니다. 변환해주세요!!";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            EmrPatient AcpEmr = clsEmrChart.ClearPatient();
            AcpEmr.acpNo = TreatNo;
            clsEmrChart.SetAcpInfo(clsDB.DbCon, ref AcpEmr, "N");

            if (AcpEmr != null)
            {
                AcpEmr.medEndDate = AcpEmr.medEndDate.IsNullOrEmpty() ? ComQuery.CurrentDateTime(clsDB.DbCon, "D") : AcpEmr.medEndDate;
                clsImgcvt.Convertimg(AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate, AcpEmr.medDeptCd, AcpEmr.inOutCls, AcpEmr.acpNo, AcpEmr.medDrCd, 0, null);
                ComFunc.MsgBoxEx(this, "작업이 완료되었습니다. 다시 복사신청을 해주세요.");
                Close();
            }
        }
    }
}
