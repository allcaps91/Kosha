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
    /// 19.위탁업무수행일지 2페이지, 20. 사업장만족도
    /// </summary>
    public partial class CardPage_16_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {
        HcOshaCard20Service hcOshaCard20Service;
        public CardPage_16_Form()
        {
            InitializeComponent();
            hcOshaCard20Service = new HcOshaCard20Service();
        }


       
        private void CardPage_16_Form_Load(object sender, EventArgs e)
        {
            Init();
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
            pan20.Initialize();


            // 금년 상반기
            SSCard.ActiveSheet.Cells[13, 3].Value = "";
            SSCard.ActiveSheet.Cells[13, 6].Value = "";
            //금년 핫반기
            SSCard.ActiveSheet.Cells[14, 3].Value = "";
            SSCard.ActiveSheet.Cells[14, 6].Value = "";

            SSCard.ActiveSheet.Cells[11, 3].Value = "";
            SSCard.ActiveSheet.Cells[11, 6].Value = "";
            SSCard.ActiveSheet.Cells[12, 3].Value = "";
            SSCard.ActiveSheet.Cells[12, 6].Value = "";
        }


        #region 
        private void Init()
        {
            DateTime dateTime = codeService.CurrentDate;
            for (int i = 0; i <= 5; i++)
            {
                CboYear.Items.Add(dateTime.AddYears(-i).ToString("yyyy"));
            }
            CboYear.SelectedIndex = 0;



            RdoSang.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CARD20.QUARTER), CheckValue = "상반기", UnCheckValue = "" });
            RdoHa.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CARD20.QUARTER), CheckValue = "하반기", UnCheckValue = "" });

            TxtSTATISFACTION.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD20.STATISFACTION) });
            TxtNAME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD20.NAME) });
            
            SSList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSList.AddColumnText("년도", nameof(HC_OSHA_CARD20.YEAR), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("반기", nameof(HC_OSHA_CARD20.QUARTER), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("만족도", nameof(HC_OSHA_CARD20.STATISFACTION), 262, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("대행업무 Counter-part명(직책)", nameof(HC_OSHA_CARD20.NAME), 200, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null || base.SelectedSite == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (pan20.Validate<HC_OSHA_CARD20>())
                {
                    HC_OSHA_CARD20 dto = pan20.GetData<HC_OSHA_CARD20>();
                    dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                    dto.SITE_ID = base.SelectedSite.ID;
                    dto.YEAR = CboYear.Text;
                    HC_OSHA_CARD20 saved = hcOshaCard20Service.Save(dto);

                    //pan20.SetData(saved);
                    pan20.Initialize();

                    Search();
                }
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            HC_OSHA_CARD20 dto = pan20.GetData<HC_OSHA_CARD20>();

            if (dto.ID > 0)
            {
                if (MessageUtil.Confirm("삭제하시겠습니까?") == DialogResult.Yes)
                {
                    hcOshaCard20Service.Delete(dto);
                    pan20.Initialize();

                    Search();
                }
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            pan20.Initialize();
        }


        private void Search()
        {
            if (base.SelectedSite == null)
            {
                return;
            }

            string startDate = base.SelectedEstimate.CONTRACTSTARTDATE;
            string endDate = base.SelectedEstimate.CONTRACTENDDATE;
            List<HC_OSHA_CARD20> list = hcOshaCard20Service.hcOshaCard20Repository.FindAll(base.SelectedEstimate.ID);

            SSList.SetDataSource(list);

            DateTime currentDate = codeService.CurrentDate;

            //List<HC_OSHA_CARD20> currentYearList = hcOshaCard20Service.hcOshaCard20Repository.FindAllByYear(currentDate.Year.ToString(), SelectedSite.ID);
            List<HC_OSHA_CARD20> currentYearList = hcOshaCard20Service.hcOshaCard20Repository.FindAllByYear(startDate.Left(4), SelectedSite.ID);
            if (list.Count > 0)
            {
                //금년
                foreach(HC_OSHA_CARD20 card in currentYearList)
                {
                    //if(currentDate.Year.ToString() == card.YEAR)
                    if (startDate.Left(4) == card.YEAR)
                    {
                        SSCard.ActiveSheet.Cells[13, 0].Value = card.YEAR.ToString().Substring(2, 2);
                        if (card.QUARTER == "상반기")
                        {
                            SSCard.ActiveSheet.Cells[13, 3].Value = card.STATISFACTION;
                            // 금년 상반기 card.STATISFACTION;
                            SSCard.ActiveSheet.Cells[13, 6].Value = card.NAME;
                        }
                        else
                        {
                            //금년 핫반기
                            SSCard.ActiveSheet.Cells[14, 3].Value = card.STATISFACTION;
                            SSCard.ActiveSheet.Cells[14, 6].Value = card.NAME;
                        }
                    }
                    //thisYear = ((DateTime)card.CREATED).Year;
                }
            }
            else
            {
                SSList.SetDataSource(new List<HC_OSHA_CARD20>());
                SSCard.ActiveSheet.Cells[13, 0].Value = startDate.Left(4).Right(2);
            }

            //List<HC_OSHA_CARD20> lastYearList = hcOshaCard20Service.hcOshaCard20Repository.FindAllByYear(currentDate.AddYears(-1).Year.ToString(), SelectedSite.ID);
            List<HC_OSHA_CARD20> lastYearList = hcOshaCard20Service.hcOshaCard20Repository.FindAllByYear((startDate.Left(4).To<int>() - 1).ToString(), SelectedSite.ID);

            //작년
            foreach (HC_OSHA_CARD20 card in lastYearList)
            {                
                SSCard.ActiveSheet.Cells[11, 0].Value = card.YEAR.Substring(2, 2);
                if (card.QUARTER == "상반기")
                {
                    SSCard.ActiveSheet.Cells[11, 3].Value = card.STATISFACTION;
                    SSCard.ActiveSheet.Cells[11, 6].Value = card.NAME;
                }
                else
                {
                    SSCard.ActiveSheet.Cells[12, 3].Value = card.STATISFACTION;
                    SSCard.ActiveSheet.Cells[12, 6].Value = card.NAME;
                }
            }
        }

        private void SSList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            HC_OSHA_CARD20 dto = SSList.GetRowData(e.Row) as HC_OSHA_CARD20;
            HC_OSHA_CARD20 saved = hcOshaCard20Service.hcOshaCard20Repository.FindOne(dto.ID);
            pan20.SetData(saved);
            CboYear.SelectedItem = saved.YEAR;

        }
        #endregion


        public void Print()
        {
            Search();
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute(SSCard.ActiveSheet);
        }

        public bool NewPrint()
        {
            Search();
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute(SSCard.ActiveSheet);
            return true;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }
    }
}
