using HC.Core.Site.Dto;
using System;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Spread.CustomCellType;
using ComBase.Mvc.Utils;
using ComBase.Mvc.Validation;
using FarPoint.Win.Spread.Model;
using System.Windows.Forms;
using HC.Core.Site.Repository;
using HC.Core.Site.Service;

namespace HC.Core.Site.UI
{
    public partial class SiteListForm : Form
    {
        /// <summary>
        /// 선택된 사업장 정보
        /// </summary>
        public HC_SITE_VIEW SelectedSite { get; set; }
        HcSiteViewService hcSiteViewService;
        public SiteListForm()
        {
            InitializeComponent();
            hcSiteViewService = new HcSiteViewService();
        }
        private void SiteListForm_Load(object sender, EventArgs e)
        {
            TxtIdOrName.SetExecuteButton(BtnSearch);

            SSSiteList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSSiteList.AddColumnText("코드", nameof(HC_SITE_VIEW.ID), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSSiteList.AddColumnText("사업장명", nameof(HC_SITE_VIEW.NAME), 290, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true , sortIndicator = SortIndicator.Ascending });
            SSSiteList.AddColumnText("사업자번호", nameof(HC_SITE_VIEW.BIZNUMBER), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
          
            BtnSearch.PerformClick();
        }
        private void ContextMenu_Color(object sender, System.EventArgs e)
        {
           
            MessageBox.Show("You chose color.");
        }
        private void BtnOk_Click(object sender, EventArgs e)
        {
            if(this.SelectedSite == null)
            {
                MessageUtil.Alert("사업장을 선택하세요");
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
            
        }

        private void SSSiteList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            this.SelectedSite = SSSiteList.GetRowData(e.Row) as HC_SITE_VIEW;
            BtnOk.PerformClick();
        }

        private void SSSiteList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            this.SelectedSite = SSSiteList.GetRowData(e.Row) as HC_SITE_VIEW;
         
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SSSiteList.DataSource = hcSiteViewService.Search(TxtIdOrName.Text);

        }


    }
}
