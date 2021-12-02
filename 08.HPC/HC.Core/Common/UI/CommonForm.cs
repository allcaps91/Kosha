using ComBase.Mvc;
using HC.Core.BaseCode.Management.Service;
using HC.Core.Common.Service;
using HC.Core.Site.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC.Core.Common.UI
{
    public partial class CommonForm : BaseForm
    {
        protected HcCodeService codeService;
        protected HcUserService userService;
        protected Session session;
        /// <summary>
        /// 선택된 사업장
        /// </summary>
        public ISiteModel SelectedSite { get; set; }

        /// <summary>
        /// 선택된 계약정보
        /// </summary>
        public IEstimateModel SelectedEstimate{ get; set; }
        public CommonForm()
        {
            InitializeComponent();
            
            codeService = CommonService.Instance.CodeService;
            userService = CommonService.Instance.UserService;
            session = CommonService.Instance.Session;
        }
        public void SetSelectedSite(long siteId)
        {
            SelectedSite model = new SelectedSite
            {
                ID = siteId
            };
            this.SelectedSite = model;
        }

        public void SetSelectedEstimate(long estimateId)
        {
            SelectedEstimate model = new SelectedEstimate
            {
                ID = estimateId
            };
            this.SelectedEstimate = model;
        }
        public CommonForm AddForm(CommonForm form, Control target)
        {
        

            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;
            
            target.Controls.Add(form);
            form.Show();
            return form;
        }

    }
}
