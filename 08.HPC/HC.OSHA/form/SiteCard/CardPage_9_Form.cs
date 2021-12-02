using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using FarPoint.Win.Spread;
using HC.Core.Common.Interface;
using HC_Core;
using HC.Core.Common.Util;
using HC.Core.Model;
using HC.OSHA.Dto;
using HC.OSHA.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComHpcLibB.Model;

namespace HC_OSHA
{
    public partial class CardPage_9_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {
        private HcOshaCard13Service hcOshaCard13Service;
        public CardPage_9_Form()
        {
            InitializeComponent();
            this.hcOshaCard13Service = new HcOshaCard13Service();
        }

        private void CardPage_9_Form_Load(object sender, EventArgs e)
        {
            Init();
            Clear13();
            SSCard.ActiveSheet.ActiveRowIndex = 0;
            SSCard.ShowRow(0, 0, FarPoint.Win.Spread.VerticalPosition.Top);
        }

        public void Select(IEstimateModel estimateModel)
        {
            base.SelectedEstimate = estimateModel;
            Clear();
            Search();
            
        }

        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;
            Clear();
        }

        private void Clear()
        {
            SSList.ActiveSheet.RowCount = 0;
            pan13.Initialize();
        }


        #region 보호구
        private void Init()
        {

            
            TxtName.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD13.NAME) });
            TxtTarget.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD13.TARGET) });
            TxtWorkerCount.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD13.WORKERCOUNT) });

            TxtTargetCOunt.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD13.TARGETCOUNT) });
            TxtPassNumber.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD13.PASSNUMBER) });
            TxtRemark.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD13.REMARK) });

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSList.AddColumnText("보호구명", nameof(HC_OSHA_CARD13.NAME), 135, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("지급대상 작업명", nameof(HC_OSHA_CARD13.TARGET), 65, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("지급대상 근로자수", nameof(HC_OSHA_CARD13.WORKERCOUNT), 71, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("지급수량", nameof(HC_OSHA_CARD13.TARGETCOUNT), 63, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("검정합격번호", nameof(HC_OSHA_CARD13.PASSNUMBER), 115, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("변동사항", nameof(HC_OSHA_CARD13.REMARK), 170, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

        }


        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (pan13.Validate<HC_OSHA_CARD13>())
                {
                    HC_OSHA_CARD13 dto = pan13.GetData<HC_OSHA_CARD13>();
                    dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                    dto.SITE_ID = base.SelectedSite.ID;
                    //HC_OSHA_CARD13 saved = hcOshaCard13Service.Save(dto, base.GetCurrentYear());
                    HC_OSHA_CARD13 saved = hcOshaCard13Service.Save(dto, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4));

                    //pan13.SetData(saved);
                    pan13.Initialize();

                    Search();
                }
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            HC_OSHA_CARD13 dto = pan13.GetData<HC_OSHA_CARD13>();

            if (dto.ID > 0)
            {
                if (MessageUtil.Confirm("삭제하시겠습니까?") == DialogResult.Yes)
                {
                    hcOshaCard13Service.Delete(dto);
                    pan13.Initialize();

                    Search();
                }
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            pan13.Initialize();
        }

        private void Clear13()
        {
            for (int i = 0; i <15; i++)
            {
                SSCard.ActiveSheet.Cells[i + 18, 0].Value = "";
                SSCard.ActiveSheet.Cells[i + 18, 1].Value = "";
                SSCard.ActiveSheet.Cells[i + 18, 3].Value = "";
                SSCard.ActiveSheet.Cells[i + 18, 5].Value = "";
                SSCard.ActiveSheet.Cells[i + 18, 7].Value = "";
                SSCard.ActiveSheet.Cells[i + 18, 9].Value = "";
            }
        }
        //보호구
        private void Search()
        {
            Clear13();

            if (base.SelectedSite == null)
            {
                return;
            }

            if (base.SelectedEstimate.CONTRACTENDDATE == null)
            {
                return;
            }
            List<HC_OSHA_CARD13> list = hcOshaCard13Service.hcOshaCard13Repository.FindAll(base.SelectedSite.ID, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4));
            //List<HC_OSHA_CARD13> list = hcOshaCard13Service.hcOshaCard13Repository.FindAll(base.SelectedSite.ID, base.GetCurrentYear());
            if (list.Count > 0)
            {
                SSList.SetDataSource(list);
                
                for(int i=0; i< list.Count; i++)
                {
                    SSCard.ActiveSheet.Cells[i + 18, 0].Value = list[i].NAME;
                    SSCard.ActiveSheet.Cells[i + 18, 1].Value = list[i].TARGET;
                    SSCard.ActiveSheet.Cells[i + 18, 3].Value = list[i].WORKERCOUNT;
                    SSCard.ActiveSheet.Cells[i + 18, 5].Value = list[i].TARGETCOUNT;
                    SSCard.ActiveSheet.Cells[i + 18, 7].Value = list[i].PASSNUMBER;
                    SSCard.ActiveSheet.Cells[i + 18, 9].Value = list[i].REMARK;
                }
            }
            else
            {

                SSList.SetDataSource(new List<HC_OSHA_CARD13>());
            }
        }

        private void SSList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            HC_OSHA_CARD13 dto = SSList.GetRowData(e.Row) as HC_OSHA_CARD13;
            pan13.SetData(dto);

        }
        #endregion

        public void Print()
        {
            Search();
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
        }

        public bool NewPrint()
        {
            Search();
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
            return true;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void SSCard_CellClick(object sender, CellClickEventArgs e)
        {

        }
    }
}
