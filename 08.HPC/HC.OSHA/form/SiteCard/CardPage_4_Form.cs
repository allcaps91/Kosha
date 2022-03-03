using ComBase.Controls;
using ComHpcLibB.Model;
using FarPoint.Win.Spread;
using HC.Core.Common.Interface;
using HC.Core.Common.Util;
using HC.OSHA.Dto;
using HC.OSHA.Repository;
using HC.OSHA.Repository.Schedule;
using HC.OSHA.Service;
using HC_Core;
using HC_OSHA.Model.Visit;
using System;
using System.Collections.Generic;

namespace HC_OSHA
{
    public partial class CardPage_4_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {

        InOutEmployeeForm inOutEmployeeForm;
        IndustrialAccidentForm accForm;

        HcOshaCard5Service hcOshaCard5Service;
        OshaVisitPriceRepository oshaVisitPriceRepository;

        public CardPage_4_Form()
        {
            InitializeComponent();
            hcOshaCard5Service= new HcOshaCard5Service();
            oshaVisitPriceRepository = new OshaVisitPriceRepository();
            inOutEmployeeForm = new InOutEmployeeForm();
            accForm = new IndustrialAccidentForm();

            SSCard.ActiveSheet.ActiveRowIndex = 0;
            SSCard.ShowRow(0, 0, FarPoint.Win.Spread.VerticalPosition.Top);
        }
        public void Search()
        {
            Clear();
            if(base.SelectedSite == null)
            {
                return;
            }
            string year = this.SelectedEstimate.CONTRACTSTARTDATE.Left(4);//  SelectedEstimate GetCurrentYear();
            string lastYear = (year.To<int>() - 1).ToString();// GetLastYear(year);
            SSCard.ActiveSheet.Cells[3, 0].Text = lastYear.Substring(2, 2);
            SSCard.ActiveSheet.Cells[5, 0].Text = year.Substring(2, 2);

            List<Dictionary<string, int>> workerInfo = new List<Dictionary<string, int>>();
            List<HC_OSHA_CARD5> list = null;
            
            list = hcOshaCard5Service.hcOshaCard5Repository.FindYear(base.SelectedSite.ID, lastYear);
            Darw(list, 3, true);
            Darw(list, 4, false);

            list = hcOshaCard5Service.hcOshaCard5Repository.FindYear(base.SelectedSite.ID, year);
            Darw(list, 5, true);
            Darw(list, 6, false);

            //6.산업재해발생현황
            //작년 월별 근로자수

            SSCard.ActiveSheet.Cells[15, 0].Value = lastYear.Substring(2, 2);

            //  금년도 조회
            string startYear = year + "-01-01 00:00:00";
            string endYear = year + "-12-31 23:59:59";
            List<VisitPriceModel> thisYearList = oshaVisitPriceRepository.FindByYear(base.SelectedSite.ID, startYear, endYear);

            //  전년도 조회
            string lastStartYear = lastYear + "-01-01 00:00:00";
            string lastEndYear = lastYear + "-12-31 23:59:59";
            List<VisitPriceModel> lastYearList = oshaVisitPriceRepository.FindByYear(base.SelectedSite.ID, lastStartYear, lastEndYear);


            foreach (VisitPriceModel price in lastYearList)
            {
                if (price.VISITDATETIME.Month == 1)  { SSCard.ActiveSheet.Cells[15, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 2)  { SSCard.ActiveSheet.Cells[16, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 3)  { SSCard.ActiveSheet.Cells[17, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 4)  { SSCard.ActiveSheet.Cells[18, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 5)  { SSCard.ActiveSheet.Cells[19, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 6)  { SSCard.ActiveSheet.Cells[20, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 7)  { SSCard.ActiveSheet.Cells[21, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 8)  { SSCard.ActiveSheet.Cells[22, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 9)  { SSCard.ActiveSheet.Cells[23, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 10) { SSCard.ActiveSheet.Cells[24, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 11) { SSCard.ActiveSheet.Cells[25, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 12) { SSCard.ActiveSheet.Cells[26, 4].Value = price.WORKERCOUNT + " 명"; }
            }

            SSCard.ActiveSheet.Cells[27, 0].Value = year.Substring(2, 2);
            foreach (VisitPriceModel price in thisYearList)
            {
                if (price.VISITDATETIME.Month == 1)  { SSCard.ActiveSheet.Cells[27, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 2)  { SSCard.ActiveSheet.Cells[28, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 3)  { SSCard.ActiveSheet.Cells[29, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 4)  { SSCard.ActiveSheet.Cells[30, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 5)  { SSCard.ActiveSheet.Cells[31, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 6)  { SSCard.ActiveSheet.Cells[32, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 7)  { SSCard.ActiveSheet.Cells[33, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 8)  { SSCard.ActiveSheet.Cells[34, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 9)  { SSCard.ActiveSheet.Cells[35, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 10) { SSCard.ActiveSheet.Cells[36, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 11) { SSCard.ActiveSheet.Cells[37, 4].Value = price.WORKERCOUNT + " 명"; }
                if (price.VISITDATETIME.Month == 12) { SSCard.ActiveSheet.Cells[38, 4].Value = price.WORKERCOUNT + " 명"; }
            }


            //재해자수(사망,부상,직업병) - 방문등록의 산업재해 연동
            HcOshaCard6Repository hcOshaCard6Repository = new HcOshaCard6Repository();
            List<HC_OSHA_CARD6> accLastYear = hcOshaCard6Repository.FindAllByYear(base.SelectedEstimate.ID, lastStartYear, lastEndYear);
            string month = "";
            foreach(HC_OSHA_CARD6 card6 in accLastYear)
            {
                month = card6.ACC_DATE.Substring(5, 2);
                if (month == "01") { SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                if (month == "02") { SSCard.ActiveSheet.Cells[16, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                if (month == "03") { SSCard.ActiveSheet.Cells[17, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                if (month == "04") { SSCard.ActiveSheet.Cells[18, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                if (month == "05") { SSCard.ActiveSheet.Cells[19, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                if (month == "06") { SSCard.ActiveSheet.Cells[20, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                if (month == "07") { SSCard.ActiveSheet.Cells[21, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                if (month == "08") { SSCard.ActiveSheet.Cells[22, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                if (month == "09") { SSCard.ActiveSheet.Cells[23, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                if (month == "10") { SSCard.ActiveSheet.Cells[24, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                if (month == "11") { SSCard.ActiveSheet.Cells[25, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                if (month == "12") { SSCard.ActiveSheet.Cells[26, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
            }
           
            
          

            List<HC_OSHA_CARD6> accYear = hcOshaCard6Repository.FindAllByYear(base.SelectedEstimate.ID, startYear, endYear);
            foreach (HC_OSHA_CARD6 card6 in accYear)
            {
                month = card6.ACC_DATE.Substring(5, 2);
                if (card6.IND_ACC_TYPE != null)
                {
                    if (month == "01") { SSCard.ActiveSheet.Cells[27, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                    if (month == "02") { SSCard.ActiveSheet.Cells[28, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                    if (month == "03") { SSCard.ActiveSheet.Cells[29, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                    if (month == "04") { SSCard.ActiveSheet.Cells[30, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                    if (month == "05") { SSCard.ActiveSheet.Cells[31, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                    if (month == "06") { SSCard.ActiveSheet.Cells[32, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                    if (month == "07") { SSCard.ActiveSheet.Cells[33, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                    if (month == "08") { SSCard.ActiveSheet.Cells[34, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                    if (month == "09") { SSCard.ActiveSheet.Cells[35, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                    if (month == "10") { SSCard.ActiveSheet.Cells[36, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                    if (month == "11") { SSCard.ActiveSheet.Cells[37, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                    if (month == "12") { SSCard.ActiveSheet.Cells[38, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value = (int)SSCard.ActiveSheet.Cells[15, GetColmunIndex(int.Parse(card6.IND_ACC_TYPE))].Value + 1; }
                }
            
            }

            //IND_ACC_TYPE 1사망 , 2부상, 3직업병
            //재해자수 계 구하기, //작년 재해율
            int IND_ACC_TYPE1 = 0;
            int IND_ACC_TYPE2 = 0;
            int IND_ACC_TYPE3 = 0;
            double total = 0;
            double workerCount = 0;
            for (int i = 15; i < 39; i++)
            {
                IND_ACC_TYPE1 = (int)SSCard.ActiveSheet.Cells[i, 11].Value;
                IND_ACC_TYPE2 = (int)SSCard.ActiveSheet.Cells[i, 14].Value;
                IND_ACC_TYPE3 = (int)SSCard.ActiveSheet.Cells[i, 17].Value;
                total = IND_ACC_TYPE1 + IND_ACC_TYPE2 + IND_ACC_TYPE3;
                if (total == 3)
                {
                    string x = "l";
                }
                SSCard.ActiveSheet.Cells[i, 8].Value = total;
                
                string count = SSCard.ActiveSheet.Cells[i, 4].Value.ToString();
                string[] tmp = count.Split(' ');
                if (tmp[0].IsNullOrEmpty())
                {
                    workerCount = 0;
                }
                else
                {
                    workerCount = double.Parse(tmp[0]);
                }
                if (workerCount > 0 && total > 0)
                {
                    SSCard.ActiveSheet.Cells[i, 20].Value = Math.Round((total / workerCount) * 100, 3);
                }
            }
        }
        private int GetColmunIndex(int IND_ACC_TYPE)
        {
            if (IND_ACC_TYPE == 1)
            {
                return 11;
            }
            else if (IND_ACC_TYPE == 2)
            {
                return 14;
            }
            else if (IND_ACC_TYPE == 3)
            {
                return 17;
            }
            else
            {
                throw new Exception("산업재해 구분이 없습니다");
            }
        }
        private void Darw(List<HC_OSHA_CARD5> list, int rowIndex, bool isJoin)
        {
            string month = string.Empty;
            int count = 0;
            foreach (HC_OSHA_CARD5 dto in list)
            {
                month = dto.REGISTERDATE.Substring(5, 2);
                if (isJoin)
                {
                    count = (int)dto.JOINCOUNT;
                }else
                {
                    count = (int)dto.QUITCOUNT;
                }
                switch (month)
                {
                    case "01":
                        SSCard.ActiveSheet.Cells[rowIndex, 4].Value = count;
                        break;
                    case "02":
                        SSCard.ActiveSheet.Cells[rowIndex, 6].Value = count;
                        break;
                    case "03":
                        SSCard.ActiveSheet.Cells[rowIndex, 8].Value = count;
                        break;
                    case "04":
                        SSCard.ActiveSheet.Cells[rowIndex, 10].Value = count;
                        break;
                    case "05":
                        SSCard.ActiveSheet.Cells[rowIndex, 12].Value = count;
                        break;
                    case "06":
                        SSCard.ActiveSheet.Cells[rowIndex, 14].Value = count;
                        break;
                    case "07":
                        SSCard.ActiveSheet.Cells[rowIndex, 16].Value = count;
                        break;
                    case "08":
                        SSCard.ActiveSheet.Cells[rowIndex, 18].Value =  count;;
                        break;
                    case "09":
                        SSCard.ActiveSheet.Cells[rowIndex, 20].Value =  count;;
                        break;
                    case "10":
                        SSCard.ActiveSheet.Cells[rowIndex, 22].Value =  count;;
                        break;
                    case "11":
                        SSCard.ActiveSheet.Cells[rowIndex, 24].Value =  count;;
                        break;
                    case "12":
                        SSCard.ActiveSheet.Cells[rowIndex, 26].Value =  count;;
                        break;
                }
            }
        }
        public void Select(IEstimateModel estimateModel)
        {
            base.SelectedEstimate = estimateModel;
            Search();


            inOutEmployeeForm.SelectedEstimate = base.SelectedEstimate;
            accForm.SelectedEstimate = base.SelectedEstimate;

            inOutEmployeeForm.Select();
            accForm.Select();
        }

      

        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;
            Clear();

            inOutEmployeeForm.SelectedSite = base.SelectedSite;
            accForm.SelectedSite = base.SelectedSite;

            inOutEmployeeForm.Select();
            accForm.Select();
        }

        private void Clear()
        {
            SSCard.ActiveSheet.Cells[3, 0].Text = string.Empty;
            SSCard.ActiveSheet.Cells[5, 0].Text = string.Empty;

            for (int i=4; i< 27; i++)
            {
                SSCard.ActiveSheet.Cells[3, i].Text = "0";
                SSCard.ActiveSheet.Cells[4, i].Text = "0";
                SSCard.ActiveSheet.Cells[5, i].Text = "0";
                SSCard.ActiveSheet.Cells[6, i].Text = "0";
            }


            //재해자수
            for(int i=1; i<4; i++)
            {
                SSCard.ActiveSheet.Cells[15, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[16, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[17, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[18, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[19, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[20, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[21, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[22, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[23, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[24, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[25, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[26, GetColmunIndex(i)].Value = 0;

                SSCard.ActiveSheet.Cells[27, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[28, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[29, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[30, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[31, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[32, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[33, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[34, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[35, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[36, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[37, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[38, GetColmunIndex(i)].Value = 0;
            }
            for (int i = 15; i < 39; i++)
            {
                SSCard.ActiveSheet.Cells[i, 4].Value = "";

                SSCard.ActiveSheet.Cells[i, 11].Value = 0;
                SSCard.ActiveSheet.Cells[i, 14].Value = 0;
                SSCard.ActiveSheet.Cells[i, 17].Value = 0;
                SSCard.ActiveSheet.Cells[i, 8].Value =  0;
                SSCard.ActiveSheet.Cells[i, 5].Value = 0;
                SSCard.ActiveSheet.Cells[i, 21].Value = "";
                
            }
        }

        public void Print()
        {
            Search();
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
            //Task.WaitAny(print.Execute());
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

        private void CardPage_4_Form_Load(object sender, EventArgs e)
        {
            
            inOutEmployeeForm.SelectedSite = base.SelectedSite;
            AddForm(inOutEmployeeForm, tabPage2);
       
            accForm.SelectedSite = base.SelectedSite;
            AddForm(accForm, tabPage3);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Search();
        }
    }
}
