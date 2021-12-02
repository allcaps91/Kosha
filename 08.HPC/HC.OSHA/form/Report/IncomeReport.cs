using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComHpcLibB.Model;
using HC.Core.Common.Extension;
using HC.Core.Common.Interface;
using HC.Core.Common.Util;
using HC.Core.Model;
using HC.OSHA.Model;
using HC_Core;
using HC_OSHA.Service.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_OSHA.form.Report
{
    public partial class IncomeReport : CommonForm, ISelectSite
    {
        private IncomeService incomService;
        public IncomeReport()
        {
            InitializeComponent();
            incomService = new IncomeService();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void IncomeReport_Load(object sender, EventArgs e)
        {
            DateTime currentDate = codeService.CurrentDate;
            //    DtpStart.SetValue(currentDate.GetFirstDate());
            DtpStart.SetValue(currentDate);
            DtpEnd.SetValue(currentDate);

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderHeight = 30 });
            SSList.AddColumnText("회계일자", nameof(IncomeModel.Created), 83, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Center, mergePolicy= FarPoint.Win.Spread.Model.MergePolicy.Always });
            SSList.AddColumnText("코드", nameof(IncomeModel.SiteId), 83, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Center });
            SSList.AddColumnText("회사명", nameof(IncomeModel.SiteName), 170, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SSList.AddColumnText("방문일자", nameof(IncomeModel.VisitDate), 85, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Center });
            SSList.AddColumnText("방문자 성명", nameof(IncomeModel.VisitUserName), 99, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SSList.AddColumnText("인원", nameof(IncomeModel.inwon), 54, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Center });
            SSList.AddColumnText("단가", nameof(IncomeModel.danga), 95, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Right });
            SSList.AddColumnText("보건대행수입금", nameof(IncomeModel.income), 110, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Right });
            
            TxtSiteName.Text = "";
            this.SelectedSite = new HC_SITE_VIEW();
            Search();

        }
        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;
            if (SelectedSite != null)
            {
                if (SelectedSite.ID > 0)
                {
                  
                    Search();
                }
            }
        }

        private void Search()
        {
            long siteId = SelectedSite.ID;

            if (ChkAll.Checked == true)
            {
                siteId = 0;
            }
            List<IncomeModel> list = incomService.FindAll(siteId, DtpStart.GetValue(), DtpEnd.GetValue(), ChkIsHisotry.Checked);
            SSList.SetDataSource(list);

            for(int i=0; i<SSList.RowCount(); i++)
            {
                IncomeModel incomeModel = SSList.GetRowData(i) as IncomeModel;
                if(incomeModel.IsDeleted == "Y")
                {
                    SSList.ActiveSheet.Rows[i].BackColor = Color.FromArgb(237, 211, 237);
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void oshaSiteLastTree1_NodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TxtSiteName.Text = oshaSiteLastTree1.GetSite.NAME;

            base.SelectedSite = oshaSiteLastTree1.GetSite;
            base.SelectedSite.ID = oshaSiteLastTree1.GetSite.ID;
            if (SelectedSite != null)
            {
                if (SelectedSite.ID > 0)
                {

                    Search();
                }
            }
       
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            //  2020년도 계장 -> 2021 팀장 조회년월 기준으로 변경
            SpreadPrint sp = new SpreadPrint(SSList, PrintStyle.HEAD_APPROVAL);
            sp.Title = "보건대행 수입 통계";
            sp.Period = DtpStart.GetValue() + " ~ " + DtpEnd.GetValue();
            sp.Execute();
        }

        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            float width = 0;
            float height = 0;
            for(int i=0; i< SSList.ActiveSheet.Columns.Count; i++)
            {
                width += SSList.ActiveSheet.Columns[i].Width;

            }
            for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
            {
                height += SSList.ActiveSheet.Rows[i].Height;

            }
      
            Rectangle printAbleRect = Rectangle.Round(e.PageSettings.PrintableArea);
            StringFormat CenterFormat = new StringFormat();
            Rectangle titleHeader = new Rectangle();
            titleHeader.X = 0 + printAbleRect.X;
            titleHeader.Y = 15 + printAbleRect.Y;
            titleHeader.Width = printAbleRect.Width;
            titleHeader.Height = 115;
            Font titleFont = new Font("Arial", 16f, FontStyle.Bold);
            e.Graphics.DrawString("보건대행", titleFont, Brushes.Black, titleHeader, CenterFormat);

            Rectangle rect = new Rectangle(0, 100, 833, 2000);
            int pageCount = SSList.GetOwnerPrintPageCount(e.Graphics, rect, 0);
            SSList.OwnerPrintDraw(e.Graphics, rect, 0, pageCount);
         //   e.Graphics.DrawString("End of Print Job", new Font("MS Sans Serif", 10), new SolidBrush(Color.Black), new Rectangle(e.PageBounds.X, e.PageBounds.Y + e.PageBounds.Height / 2, e.PageBounds.Width / 2, e.PageBounds.Height / 2));
        }

        private void ChkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkAll.Checked)
            {
                TxtSiteName.Text = "";
            //    SelectedSite.ID = 0;

            
            }
            Search();
        }
    }
}
