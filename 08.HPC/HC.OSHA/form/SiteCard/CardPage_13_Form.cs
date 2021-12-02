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
    /// <summary>
    /// 사업장 안전 보건 교육
    /// </summary>
    public partial class CardPage_13_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {
        private HcOshaCard17Service hcOshaCard17Service;
        public CardPage_13_Form()
        {
            InitializeComponent();
            this.hcOshaCard17Service = new HcOshaCard17Service();
        }

        private void CardPage_13_Form_Load(object sender, EventArgs e)
        {
            Init();
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

      


        #region 
        private void Init()
        {

            DtpEDUDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD17.EDUDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            CboEDUTYPE.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_CARD17.EDUTYPE) });
            CboEDUTYPE.SetItems(codeService.FindActiveCodeByGroupCode("SITE_CARD_EDUTYPE", "OSHA"), "CodeName", "Code", "OSHA");

            CboEDUUSAGE.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_CARD17.EDUUSAGE) });
            CboEDUUSAGE.SetItems(codeService.FindActiveCodeByGroupCode("SITE_CARD_EDUUSAGE", "OSHA"), "CodeName", "Code", "OSHA");

            TxtEDUPLACE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD17.EDUPLACE) });
            TxtEDUNAME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD17.EDUNAME) });
            TxtCONTENT.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD17.CONTENT) });

            NumTARGETCOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CARD17.TARGETCOUNT), Min = 0 });
            NumACTCOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CARD17.ACTCOUNT), Min = 0 } );

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSList.AddColumnText("일자", nameof(HC_OSHA_CARD17.EDUDATE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("교육종류", nameof(HC_OSHA_CARD17.EDUTYPE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("교육내용", nameof(HC_OSHA_CARD17.CONTENT), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("교육인원(대상)", nameof(HC_OSHA_CARD17.TARGETCOUNT), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("교육인원(참석)", nameof(HC_OSHA_CARD17.ACTCOUNT), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("교육방법", nameof(HC_OSHA_CARD17.EDUUSAGE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("장소", nameof(HC_OSHA_CARD17.EDUPLACE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("실시자", nameof(HC_OSHA_CARD17.EDUNAME), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

            Clear();

        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (pan17.Validate<HC_OSHA_CARD17>())
                {
                    HC_OSHA_CARD17 dto = pan17.GetData<HC_OSHA_CARD17>();
                    dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                    dto.SITE_ID = base.SelectedSite.ID;
                    //HC_OSHA_CARD17 saved = hcOshaCard17Service.Save(dto, base.GetCurrentYear());
                    HC_OSHA_CARD17 saved = hcOshaCard17Service.Save(dto, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4));

                    //pan17.SetData(saved);
                    pan17.Initialize();
                    Search();
                }
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            HC_OSHA_CARD17 dto = pan17.GetData<HC_OSHA_CARD17>();

            if (dto.ID > 0)
            {
                if (MessageUtil.Confirm("삭제하시겠습니까?") == DialogResult.Yes)
                {
                    hcOshaCard17Service.Delete(dto);
                    pan17.Initialize();

                    Search();
                }
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            pan17.Initialize();
        }

        private void Clear()
        {
            SSList.ActiveSheet.RowCount = 0;
            pan17.Initialize();

            int row = 0;
            for (int i = 0; i < 30; i++)
            {
                row = i + 4;
                SSCard.ActiveSheet.Cells[row, 0].Value = "";
                SSCard.ActiveSheet.Cells[row, 1].Value = "";
                SSCard.ActiveSheet.Cells[row, 2].Value = "";
                SSCard.ActiveSheet.Cells[row, 3].Value = "";
                SSCard.ActiveSheet.Cells[row, 4].Value = "";
                SSCard.ActiveSheet.Cells[row, 5].Value = "";
                SSCard.ActiveSheet.Cells[row, 6].Value = "";
                SSCard.ActiveSheet.Cells[row, 7].Value = "";
            }
        }
        private void Search()
        {
            Clear();
            if (base.SelectedSite == null)
            {
                return;
            }
            string startDate = base.SelectedEstimate.CONTRACTSTARTDATE;
            string endDate = base.SelectedEstimate.CONTRACTENDDATE;
            //List<HC_OSHA_CARD17> list = hcOshaCard17Service.hcOshaCard17Repository.FindAll(base.SelectedSite.ID, startDate, endDate);
            //List<HC_OSHA_CARD17> list = hcOshaCard17Service.hcOshaCard17Repository.FindAll(base.SelectedSite.ID, base.GetCurrentYear());
            List<HC_OSHA_CARD17> list = hcOshaCard17Service.hcOshaCard17Repository.FindAll(base.SelectedSite.ID, startDate.Left(4));
            if (list.Count > 0)
            {
                SSList.SetDataSource(list);

                int row = 0;
                for(int i=0; i < list.Count; i++)
                {
                    row = i + 4;
                    SSCard.ActiveSheet.Cells[row, 0].Value = list[i].EDUDATE.Substring(5,2) + "/"+ list[i].EDUDATE.Substring(8, 2);
                    SSCard.ActiveSheet.Cells[row, 1].Value = list[i].EDUTYPE;
                    SSCard.ActiveSheet.Cells[row, 2].Value = list[i].CONTENT;
                    SSCard.ActiveSheet.Cells[row, 3].Value = list[i].TARGETCOUNT;
                    SSCard.ActiveSheet.Cells[row, 4].Value = list[i].ACTCOUNT;
                    SSCard.ActiveSheet.Cells[row, 5].Value = list[i].EDUUSAGE;
                    SSCard.ActiveSheet.Cells[row, 6].Value = list[i].EDUPLACE;
                    SSCard.ActiveSheet.Cells[row, 7].Value = list[i].EDUNAME;

                }
            }
            else
            {
                SSList.SetDataSource(new List<HC_OSHA_CARD17>());
            }
        }

        private void SSList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            HC_OSHA_CARD17 dto = SSList.GetRowData(e.Row) as HC_OSHA_CARD17;
            HC_OSHA_CARD17 saved =  hcOshaCard17Service.hcOshaCard17Repository.FindOne(dto.ID);
            pan17.SetData(saved);

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
    }
}
