using ComBase.Mvc.Spread;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using HC.OSHA.Model;
using Newtonsoft.Json;
using HC.OSHA.Dto.StatusReport;
using HC.Core.Common.Interface;
using HC.Core.Model;
using HC.Core.Service;
using HC.OSHA.Repository;
using ComBase;
using HC.OSHA.Service;
using HC_Core;
using HC_OSHA.StatusReport;

namespace HC_OSHA
{
    public partial class SangDamCountForm : CommonForm
    {
        private StatusReportByDoctor statusReportByDoctor;
        public SangDamCountForm()
        {
            InitializeComponent();
        }

        public void SetStatusReportByDoctor(StatusReportByDoctor statusReportByDoctor)
        {
            this.statusReportByDoctor = statusReportByDoctor;
        }

        private void BtnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
