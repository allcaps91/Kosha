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
using ComHpcLibB.Model;
using HC.OSHA.Model;
using ComBase.Mvc.Enums;
using HC.OSHA.Service;
using FarPoint.Win.Spread;

namespace HC_OSHA
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
            SSList.AddColumnText("계약기간", nameof(HC_ESTIMATE_MODEL.ContractPeriod), 126, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("계약여부", nameof(HC_ESTIMATE_MODEL.ISCONTRACT), 64, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

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

        /// <summary>
        /// 사업자아이디로 견적 및 계약가져와서 첫번째 행을 더블클릭
        /// </summary>
        /// <param name="siteId"></param>
        public void SearhAndDoubleClik(long siteId, bool isAll)
        {
            SSList.SetDataSource(oshaEstimateModelService.FindBySiteId(siteId, isAll));
            if (SSList.RowCount() > 0)
            {
                 SSList_CellDoubleClick(SSList, new CellClickEventArgs(null, 0, 0, 0, 0, MouseButtons.Left, false, false, false));
            }
        }
        public int GetRowCount()
        {
            return SSList.RowCount();
        }
        /// <summary>
        /// 사업자아이디로 견적 및 계약 가져오기
        /// </summary>
        /// <param name="siteId"></param>
        public void Searh(long siteId, bool isALL)
        {
            SSList.SetDataSource(oshaEstimateModelService.FindBySiteId(siteId, true));

        }

    }
}
