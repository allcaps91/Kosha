using HC.Core.Dto;
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
using HC.Core.Repository;
using HC.Core.Service;
using System.Collections.Generic;
using ComHpcLibB.Model;

namespace HC_Core
{
    public partial class SiteListForm : Form
    {
        /// <summary>
        /// 선택된 사업장 정보
        /// </summary>
        public HC_SITE_VIEW SelectedSite { get; set; }
        private HcSiteViewService hcSiteViewService;
        private HcUserService hcUsersService;
        public SiteListForm()
        {
            InitializeComponent();
            hcSiteViewService = new HcSiteViewService();
            hcUsersService = new HcUserService();
        }
        private void SiteListForm_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                List<HC_USER> OSHAUsers = hcUsersService.GetOsha();
                CboManager.SetItems(OSHAUsers, "Name", "UserId", "전체", "all", AddComboBoxPosition.Top);
                CboManager.SetValue(CommonService.Instance.Session.UserId);

            }

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
            SSSiteList.DataSource = hcSiteViewService.Search(TxtIdOrName.Text, CboManager.GetValue());

        }

        public HC_SITE_VIEW Search(string idOrName)
        {
            List<HC_SITE_VIEW> list = null;
           
            if (idOrName.IsNullOrEmpty())
            {
                return null;
            }
            else
            {
                TxtIdOrName.Text = idOrName;
                list =  hcSiteViewService.Search(idOrName, CboManager.GetValue());
                if(list.Count == 1)
                {
                    return list[0];
                }
                else
                {
                    return null;
                }
            }
            
        }

        private void CboManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnSearch.PerformClick();
        }
    }
}
