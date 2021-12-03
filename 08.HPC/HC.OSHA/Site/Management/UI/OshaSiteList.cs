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
using HC.OSHA.Site.Management.Model;
using HC.OSHA.Site.Management.Service;

namespace HC.OSHA.Site.Management.UI
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

        public delegate void CellDoubleClickEventHandler(object sender, FarPoint.Win.Spread.CellClickEventArgs e);

        [Category("A-MTS-Framework-Event")]
        [Description("스프레드 더블클릭")]
        public event CellDoubleClickEventHandler CellDoubleClick;
        
       
        HcOshaSiteService service;

        public OshaSiteList()
        {
            InitializeComponent();
            GetSite = new HC_OSHA_SITE_MODEL();
            service = new HcOshaSiteService();
         
        }

        private void OshaSiteList_Load(object sender, EventArgs e)
        {

            TxtName.SetExecuteButton(BtnSearch);

            SSOshaList.Initialize(new SpreadOption() { IsRowSelectColor = true});
            SSOshaList.AddColumnText("코드", nameof(HC_OSHA_SITE_MODEL.ID), 55, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSOshaList.AddColumnText("사업장명", nameof(HC_OSHA_SITE_MODEL.NAME), 200, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });

            if (!DesignMode)
            {
                BtnSearch.PerformClick();
            }

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {

            SSOshaList.SetDataSource(service.Search(TxtName.Text));
        }

        private void SSOshaList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            GetSite = SSOshaList.GetRowData(e.Row) as HC_OSHA_SITE_MODEL;
      
            CellDoubleClick?.Invoke(sender, e);
        }
    }
}
