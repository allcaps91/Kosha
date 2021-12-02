using ComBase.Controls;
using ComBase.Mvc.Enums;
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
using HC.OSHA.Repository;
using ComBase;
using ComHpcLibB.Model;

namespace HC_OSHA
{
    public partial class CardPage_5_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {
        HcOshaCard5Service hcOshaCard5Service;
        public CardPage_5_Form()
        {
            InitializeComponent();
            hcOshaCard5Service = new HcOshaCard5Service();
            Clear();
            SSCard.ActiveSheet.ActiveRowIndex = 0;
            SSCard.ShowRow(0, 0, FarPoint.Win.Spread.VerticalPosition.Top);
        }

        public void Select(IEstimateModel estimateModel)
        {
            base.SelectedEstimate = estimateModel;
            Search();
        }



        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;
            
            Clear();
        }

        private void Clear()
        {
            
            SSCard.ActiveSheet.Cells[2, 0].Value = "";
            SSCard.ActiveSheet.Cells[14, 0].Value = "";

            for (int i = 2; i < 26; i++)
            {
                SSCard.ActiveSheet.Cells[i, 4].Value = "";
                SSCard.ActiveSheet.Cells[i, 7].Value = "";
                SSCard.ActiveSheet.Cells[i, 10].Value = "";
                SSCard.ActiveSheet.Cells[i, 13].Value = "";
                SSCard.ActiveSheet.Cells[i, 16].Value = "";
                SSCard.ActiveSheet.Cells[i, 19].Value = "";
                SSCard.ActiveSheet.Cells[i, 22].Value = "";
            }

        }
        public void Search()
        {
            if (base.SelectedSite == null)
            {
                return;
            }
            Clear();

            string year = base.SelectedEstimate.CONTRACTSTARTDATE.Left(4); //GetCurrentYear();
            string lastYear = GetLastYear(year);
            string lastStartYear = lastYear + "-01-01 00:00:00";
            string lastEndYear = lastYear + "-12-31 23:59:59";

            SSCard.ActiveSheet.Cells[2, 0].Value = lastYear;
            SSCard.ActiveSheet.Cells[14, 0].Value = year ;

            HcOshaCard6Repository hcOshaCard6Repository = new HcOshaCard6Repository();
            List<HC_OSHA_CARD6> accLastYear = hcOshaCard6Repository.FindAllByYear(base.SelectedEstimate.ID, lastStartYear, lastEndYear);

            //작년 재해자별현황
            int row = 2;
            for(int i =0; i<accLastYear.Count; i++)
            {
             
                if(row == 14)
                {
                    break;
                }
                SSCard.ActiveSheet.Cells[row, 4].Value = accLastYear[i].NAME;

                string jumin  = clsAES.DeAES(accLastYear[i].JUMIN_NO);

                if (jumin.Length > 6)
                {
                    jumin = jumin.Substring(0, 6) + "-" + jumin.Substring(6, 1) + "*****";
                }

                SSCard.ActiveSheet.Cells[row, 7].Value = jumin;
                SSCard.ActiveSheet.Cells[row, 10].Value = accLastYear[i].REQUEST_DATE;
                SSCard.ActiveSheet.Cells[row, 13].Value = accLastYear[i].APPROVE_DATE;
                SSCard.ActiveSheet.Cells[row, 16].Value = accLastYear[i].ACC_DATE;
                SSCard.ActiveSheet.Cells[row, 19].Value = accLastYear[i].ILLNAME;
                SSCard.ActiveSheet.Cells[row, 22].Value = accLastYear[i].REMARK;
                row++;
            }

            string startYear = year + "-01-01 00:00:00";
            string endYear = year + "-12-31 23:59:59";
            List<HC_OSHA_CARD6> accYear = hcOshaCard6Repository.FindAllByYear(base.SelectedEstimate.ID, startYear, endYear);
            //금년 재해자별현황
             row = 14;
            for (int i = 0; i < accYear.Count; i++)
            {
             
                row +=i;
                if (row > SSCard.ActiveSheet.RowCount)
                {
                    break;
                }

                string jumin = clsAES.DeAES(accYear[i].JUMIN_NO);

                if (jumin.Length > 6)
                {
                    jumin = jumin.Substring(0, 6) + "-" + jumin.Substring(6, 1) + "*****";
                }
                SSCard.ActiveSheet.Cells[row, 4].Value = accYear[i].NAME;
                SSCard.ActiveSheet.Cells[row, 7].Value = jumin;
                SSCard.ActiveSheet.Cells[row, 10].Value = accYear[i].REQUEST_DATE;
                SSCard.ActiveSheet.Cells[row, 13].Value = accYear[i].APPROVE_DATE;
                SSCard.ActiveSheet.Cells[row, 16].Value = accYear[i].ACC_DATE;
                SSCard.ActiveSheet.Cells[row, 19].Value = accYear[i].ILLNAME;
                SSCard.ActiveSheet.Cells[row, 22].Value = accYear[i].REMARK;
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

        private void CardPage_5_Form_Load(object sender, EventArgs e)
        {
            //IndustrialAccidentForm accForm = new IndustrialAccidentForm();
            //accForm.SelectedSite = base.SelectedSite;
            //AddForm(accForm, tabPage2);
        }
    }
}
