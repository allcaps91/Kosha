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
using HC.OSHA.Site.Management.Model;
using ComBase.Mvc.Enums;
using HC.OSHA.Site.Management.Service;
using FarPoint.Win.Spread;

namespace HC.OSHA.Site.Management.UI
{
    public partial class OshaSiteEstimateList : UserControl
    {

        private OshaEstimateModelService oshaEstimateModelService;
        /// <summary>
        /// 선택된 견적 & 계약
        /// </summary>
        public HC_ESTIMATE_MODEL GetEstimateModel { get; set; }

        public delegate void CellDoubleClickEventHandler(object sender, FarPoint.Win.Spread.CellClickEventArgs e);

        private void OshaSiteEstimateList_Load(object sender, EventArgs e)
        {
            GetEstimateModel = new HC_ESTIMATE_MODEL();
            SSList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSList.AddColumnText("계약기간", nameof(HC_ESTIMATE_MODEL.ContractPeriod), 190, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("계약여부", nameof(HC_ESTIMATE_MODEL.ISCONTRACT), 69, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

        }

        [Category("A-MTS-Framework-Event")]
        [Description("스프레드 더블클릭")]
        public event CellDoubleClickEventHandler CellDoubleClick;

        public OshaSiteEstimateList()
        {
            InitializeComponent();
            oshaEstimateModelService = new OshaEstimateModelService();
        }

        private void SSList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            GetEstimateModel = SSList.GetRowData(e.Row) as HC_ESTIMATE_MODEL;
            CellDoubleClick?.Invoke(sender, e);
        }

        public void Searh(long siteId)
        {
            SSList.SetDataSource(oshaEstimateModelService.FindBySiteId(siteId));
          //  if (SSList.RowCount() > 0)
          //  {

          ////      SSList_CellDoubleClick(SSList, new CellClickEventArgs(null, 0, 0, 0, 0, MouseButtons.Left, false, false, false));
          //  }

            SSList_CellDoubleClick(SSList, new CellClickEventArgs(null, 0, 0, 0, 0, MouseButtons.Left, false, false, false));
        }
    
    }
}
