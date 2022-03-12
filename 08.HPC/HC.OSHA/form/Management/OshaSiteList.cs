using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using HC.OSHA.Model;
using HC.OSHA.Service;
using System.Collections;
using HC.Core.Model;
using HC.Core.Dto;
using HC.Core.Service;
using ComHpcLibB.Model;

namespace HC_OSHA
{
    /// <summary>
    /// 보건관리전문 사업장 견적 및 계약 목록 
    /// </summary>
    [Description("보건관리전문 사업장 견적 및 계약 목록 ")]
    public partial class OshaSiteList : UserControl
    {
        /// <summary>
        /// 선택된 사업장
        /// </summary>
        public HC_OSHA_SITE_MODEL GetSite {get; set;}
        private HcUserService hcUsersService;
        
        public delegate void CellDoubleClickEventHandler(object sender, FarPoint.Win.Spread.CellClickEventArgs e);

        public delegate void CellClickEventHandler(object sender, FarPoint.Win.Spread.CellClickEventArgs e);

        [Category("A-MTS-Framework-Event")]
        [Description("스프레드 셀더블클릭")]
        public event CellDoubleClickEventHandler CellDoubleClick;

        [Category("A-MTS-Framework-Event")]
        [Description("스프레드 셀클릭")]
        public event CellDoubleClickEventHandler CellClick;

        private HcOshaSiteService service;

        public OshaSiteList()
        {
            InitializeComponent();
            GetSite = new HC_OSHA_SITE_MODEL();
            service = new HcOshaSiteService();
            hcUsersService = new HcUserService();
        }

        private void OshaSiteList_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                List<HC_USER> OSHAUsers = hcUsersService.GetOsha();
                CboManager.SetItems(OSHAUsers, "Name", "UserId", "전체", "", AddComboBoxPosition.Top);
                CboManager.SetValue(CommonService.Instance.Session.UserId);

            }
            TxtName.SetExecuteButton(BtnSearch);

            SSOshaList.Initialize(new SpreadOption() { IsRowSelectColor = true});
            SSOshaList.AddColumnText("코드", nameof(HC_OSHA_SITE_MODEL.ID), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSOshaList.AddColumnText("사업장명", nameof(HC_OSHA_SITE_MODEL.NAME), 140, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true,Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left,  sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            
            //  SSOshaList.Sheets[0].AllowGroup = true;
            // SSOshaList.Sheets[0].Cells[0,0].
            //  SSOshaList.Sheets[0].GroupBarInfo.Visible = true;
            // SSOshaList.Sheets[0].SetChildVisible(new CustomSheet(), true);

            if (!DesignMode)
            {
                BtnSearch.PerformClick();
            }

        }
        public void SelectSite(ISiteModel siteModel)
        {
            for(int i=0; i < SSOshaList.ActiveSheet.RowCount; i++)
            {
                HC_OSHA_SITE_MODEL site = SSOshaList.GetRowData(i) as HC_OSHA_SITE_MODEL;
                if(site.ID == siteModel.ID)
                {
                  
                    FarPoint.Win.Spread.SpreadView sv = new FarPoint.Win.Spread.SpreadView(SSOshaList);
                    SSOshaList_CellDoubleClick(SSOshaList, new FarPoint.Win.Spread.CellClickEventArgs(sv, i, 0, 0, 0, MouseButtons.Left, false, false));
                    SSOshaList.ActiveSheet.AddSelection(i, 0, 1, 1);
                    break;
                }
            }
             
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            List<HC_OSHA_SITE_MODEL> list = service.Search(TxtName.Text, CboManager.GetValue(), true, ChkSchedule.Checked);
            SSOshaList.SetDataSource(list);
        }

        private void SSOshaList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            GetSite = SSOshaList.GetRowData(e.Row) as HC_OSHA_SITE_MODEL;
      
            CellDoubleClick?.Invoke(sender, e);
        }
        private void SSOshaList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            GetSite = SSOshaList.GetRowData(e.Row) as HC_OSHA_SITE_MODEL;

            CellClick?.Invoke(sender, e);
        }

        private void CboManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnSearch.PerformClick();
        }

        private void ChkSchedule_CheckedChanged(object sender, EventArgs e)
        {
            BtnSearch.PerformClick();
        }

        private void TxtName_TextChanged(object sender, EventArgs e)
        {
            if (TxtName.Text.Length == 0)
            {
                BtnSearch.PerformClick();
            }
        }

     
    }
}
