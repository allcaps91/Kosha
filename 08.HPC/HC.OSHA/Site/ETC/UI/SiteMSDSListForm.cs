using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComHpcLibB.Model;
using FarPoint.Win.Spread.Model;
using HC.OSHA.Site.ETC.Model;
using HC.OSHA.Site.ETC.Service;
using System;
using System.Collections.Generic;
using HC.Core.Common.UI;
using HC.Core.BaseCode.MSDS.Dto;
using HC.Core.BaseCode.MSDS.Service;
using HC.Core.Site.Model;


namespace HC.OSHA.Site.ETC.UI
{
    public partial class SiteMSDSListForm : CommonForm, ISelectSite, ISelectEstimate
    {
        private HcSiteMsdsModelService hcSiteMsdsModelService;

        public SiteMSDSListForm()
        {
            InitializeComponent();


            hcSiteMsdsModelService = new HcSiteMsdsModelService();

            SSMSDSList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSMSDSList.AddColumnText("취급공정", nameof(HC_SITE_MSDS_MODEL.PROCESS), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, mergePolicy = MergePolicy.Always });
            SSMSDSList.AddColumnText("제품명", nameof(HC_SITE_MSDS_MODEL.PRODUCTNAME), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, mergePolicy = MergePolicy.Always });
            SSMSDSList.AddColumnText("권고용도", nameof(HC_SITE_MSDS_MODEL.USAGE), 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, mergePolicy = MergePolicy.Always });
            SSMSDSList.AddColumnText("제조사", nameof(HC_SITE_MSDS_MODEL.MANUFACTURER), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, mergePolicy = MergePolicy.Always });
            SSMSDSList.AddColumnText("월취급량", nameof(HC_SITE_MSDS_MODEL.MONTHLYAMOUNT), 67, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, mergePolicy = MergePolicy.Always });
            SSMSDSList.AddColumnText("개정일자", nameof(HC_SITE_MSDS_MODEL.REVISIONDATE), 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, mergePolicy = MergePolicy.Always });
            SSMSDSList.AddColumnImage("그림문자", nameof(HC_SITE_MSDS_MODEL.GHS_IMAGE), 90,  new SpreadCellTypeOption { IsSort = false });
            


            SSMSDSList.AddColumnText("물질명", nameof(HC_SITE_MSDS_MODEL.NAME), 150, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("CasNo", nameof(HC_SITE_MSDS_MODEL.CASNO), 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("노출기준설정", nameof(HC_SITE_MSDS_MODEL.EXPOSURE_MATERIAL), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("작업환경측정", nameof(HC_SITE_MSDS_MODEL.WEM_MATERIAL), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("특수건강진단", nameof(HC_SITE_MSDS_MODEL.SPECIALHEALTH_MATERIAL), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("관리대상유해", nameof(HC_SITE_MSDS_MODEL.MANAGETARGET_MATERIAL), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("특별관리", nameof(HC_SITE_MSDS_MODEL.SPECIALMANAGE_MATERIAL), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("허용기준설정", nameof(HC_SITE_MSDS_MODEL.STANDARD_MATERIAL), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("허가대상유해", nameof(HC_SITE_MSDS_MODEL.PERMISSION_MATERIAL), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("PSM제출대상", nameof(HC_SITE_MSDS_MODEL.PSM_MATERIAL), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
        }
    

        private void Search()
        {

            SSMSDSList.SetDataSource(hcSiteMsdsModelService.FindAll(base.SelectedEstimate.ID));


            //imagecelltype은 자동머지가 안됨(MergePolicy) 수동으로 머지함.
            List<GHSRowSpan> rowSpanList = new List<GHSRowSpan>();
            for (int i = 0; i < SSMSDSList.ActiveSheet.RowCount; i++)
            {
                if (i == 0)
                {
                    GHSRowSpan rowSpan = new GHSRowSpan()
                    {
                        ProductName = SSMSDSList.ActiveSheet.Cells[i, 1].Value.ToString(),
                        RowIndex = i,
                        RowSpan = 1
                    };

                    rowSpanList.Add(rowSpan);
                }
                else
                {
                    bool isNewRow = true;
                    string productName = SSMSDSList.ActiveSheet.Cells[i, 1].Value.ToString();
                    foreach (GHSRowSpan rowSpan in rowSpanList)
                    {
                        if (rowSpan.ProductName.Equals(productName))
                        {
                            rowSpan.RowSpan += 1;
                            isNewRow = false;
                        }
                    }
                    if (isNewRow)
                    {
                        GHSRowSpan rowSpan = new GHSRowSpan()
                        {
                            ProductName = SSMSDSList.ActiveSheet.Cells[i, 1].Value.ToString(),
                            RowIndex = i,
                            RowSpan = 1
                        };

                        rowSpanList.Add(rowSpan);
                    }
                }

                foreach (GHSRowSpan rowSpan in rowSpanList)
                {
                    SSMSDSList.ActiveSheet.Cells[rowSpan.RowIndex, 6].RowSpan = rowSpan.RowSpan;
                }
            }
        }
        private void BtnMange_Click(object sender, EventArgs e)
        {
            SiteMSDSForm form = new SiteMSDSForm();
            form.SelectedSite = base.SelectedSite;
            form.ShowDialog();
            Search();

        }

        void ISelectEstimate.Select(IEstimateModel estimateModel)
        {
            base.SelectedEstimate = estimateModel;
            Search();
          
        }

        void ISelectSite.Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;
            LblSite.Text = base.SelectedSite.NAME;

            Search();
        }
    }

    public class GHSRowSpan
    {
        public string ProductName { get; set; }
        public int RowIndex { get; set; }
        public int RowSpan { get; set; }

    }
}
