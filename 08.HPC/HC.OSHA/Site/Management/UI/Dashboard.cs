using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using HC.OSHA.Site.Management.UI;
using HC.OSHA.Visit.Schedule.UI;
using HC.Core.Common.Interface;
using HC.Core.Common.Service;
using HC.Core.Common.UI;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace HC.OSHA.Site.ETC.UI
{
    public partial class Dashboard : CommonForm
    {
        /// <summary>
        /// 실행중인 폼
        /// </summary>
        private CommonForm ActiveForm { get; set; }
        /// <summary>
        /// 선택된 사업장
        /// </summary>
     //   public HC_OSHA_SITE_MODEL SelectedSite { get; set; }
        /// <summary>
        /// 선택된 계약정보
        /// </summary>
     //   public HC_ESTIMATE_MODEL SelectedSiteEstimate { get; set; }
        public Dashboard()
        {
            InitializeComponent();
        }
        private void Dashboard_Load(object sender, EventArgs e)
        {
            CalendarForm form = new CalendarForm();
        }
        private void OshaSiteList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            if (ActiveForm != null)
            {
                (ActiveForm as ISelectSite).Select(oshaSiteList.GetSite);
            }


             oshaSiteEstimateList.Searh(oshaSiteList.GetSite.ID);

        }

        private void OshaSiteEstimateList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ActiveForm != null)
            {
                (ActiveForm as ISelectEstimate).Select(oshaSiteEstimateList.GetEstimateModel);
            }
        }

        private void BtnMSDS_Click(object sender, EventArgs e)
        {
            if (SiteValidate())
            {
                SiteMSDSListForm form = new SiteMSDSListForm();
                form.SelectedSite = base.SelectedSite;
                form.SelectedEstimate = base.SelectedEstimate;
                CreateForm(form);
            //    CommonService.Instance.AddForm(panFrame, form);
            }
            
        }

        private bool SiteValidate()
        {
            bool result = true;
            if (base.SelectedSite == null)
            {
                result = false;
                MessageUtil.Info("사업장을 선택하세요");
            }
            else if ( base.SelectedEstimate == null)
            {
                result = false;
                MessageUtil.Info("사업장 견적 내역을 선택하세요");
            }
            return result;
        }

        private void BtnWorker_Click(object sender, EventArgs e)
        {
            //if (SiteValidate())
            //{
            //    SiteWorkerForm form = new SiteWorkerForm();
            //    form.SelectedSite = base.SelectedSite;
            //    form.SelectedEstimate = base.SelectedEstimate;
            //    AddForm(form);
            //}
            SiteWorkerForm form = new SiteWorkerForm();
            form.SelectedSite = base.SelectedSite;
            form.SelectedEstimate = base.SelectedEstimate;

            CreateForm(form);

        }
        public void CreateForm(CommonForm form)
        {
            if (ActiveForm != null)
            {
                ActiveForm.Close();
                ActiveForm.Dispose();
            }
            this.ActiveForm = AddForm(form, panFrame);
        }

        private void OshaSiteList_Load(object sender, EventArgs e)
        {

        }

        private void TableBody_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
