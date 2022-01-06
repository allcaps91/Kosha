using ComBase.Controls;
using ComBase.Mvc.Enums;
using HC.OSHA.Dto;
using HC.OSHA.Service;
using HC_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_OSHA.form.Management
{
    public partial class SitePriceManageForm : CommonForm
    {
        private OshaPriceService oshaPriceService;
        private bool isParent = false;
        public SitePriceManageForm()
        {
            InitializeComponent();
            oshaPriceService = new OshaPriceService();
        }
        public SitePriceManageForm(bool isParent)
        {
            InitializeComponent();
            oshaPriceService = new OshaPriceService();
            this.isParent = isParent;
        }
        private void SitePriceManageForm_Load(object sender, EventArgs e)
        {
            TxtNameOrCode.SetExecuteButton(BtnSearch);

            SSPrice.Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderHeight = 35 });
            SSPrice.AddColumnText("ID", nameof(OSHA_PRICE.SITE_ID), 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending, Aligen= FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SSPrice.AddColumnText("사업장명", nameof(OSHA_PRICE.SITE_NAME), 150, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending , Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left});
            SSPrice.AddColumnText("계약일", nameof(OSHA_PRICE.CONTRACTDATE), 85, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SSPrice.AddColumnText("계약시작일", nameof(OSHA_PRICE.CONTRACTSTARTDATE), 94, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SSPrice.AddColumnText("계약종료일", nameof(OSHA_PRICE.CONTRACTENDDATE), 94, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });

            SSPrice.AddColumnText("인원", nameof(OSHA_PRICE.WORKERTOTALCOUNT), 70, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SSPrice.AddColumnNumber("단가", nameof(OSHA_PRICE.UNITPRICE), 90, new SpreadCellTypeOption { IsSort = true, Aligen= FarPoint.Win.Spread.CellHorizontalAlignment.Right });

            // SSPrice.AddColumnText("단가금액", nameof(OSHA_PRICE.UNITTOTALPRICE), 90, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSPrice.AddColumnNumber("계산금액", nameof(OSHA_PRICE.UNITTOTALPRICE), 90,  new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Right });
            SSPrice.AddColumnNumber("계약금액", nameof(OSHA_PRICE.TOTALPRICE), 90,  new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Right });

            SSPrice.AddColumnText("정액여부", nameof(OSHA_PRICE.ISFIX), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSPrice.AddColumnText("계산서인원단가", nameof(OSHA_PRICE.ISBILL), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSPrice.AddColumnText("삭제여부", nameof(OSHA_PRICE.ISDELETED), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSPrice.AddColumnText("수정일시", nameof(OSHA_PRICE.MODIFIED), 163, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSPrice.AddColumnText("사용자", nameof(OSHA_PRICE.MODIFIEDUSER), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            Search();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            string gbGukgo = "";
            if (RdoGbGukgo.Checked)
            {
                gbGukgo = "Y";
            }
            List<OSHA_PRICE> list = oshaPriceService.FindAll(TxtNameOrCode.Text, gbGukgo, isParent);
            if (isParent)
            {
                foreach(OSHA_PRICE dto in list)
                {
                    List<OSHA_PRICE> childPrice = oshaPriceService.OshaPriceRepository.FindAllByParent(dto.SITE_ID);
                    foreach (OSHA_PRICE child in childPrice)
                    {
                        dto.WORKERTOTALCOUNT += child.WORKERTOTALCOUNT;
                        //dto.UNITPRICE += child.UNITPRICE;
                        dto.UNITTOTALPRICE += child.UNITTOTALPRICE;
                        dto.TOTALPRICE += child.TOTALPRICE;
                    }
                  
                }
            }
            SSPrice.SetDataSource(list);

            lblCount.Text = "총: " + list.Count + " 건";
        }
    }
}
