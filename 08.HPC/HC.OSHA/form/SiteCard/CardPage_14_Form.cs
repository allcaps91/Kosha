using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using HC.Core.Common.Util;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Repository;
using HC.Core.Service;
using HC.Core.Common.Interface;
using HC.OSHA.Dto;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Repository;
using HC.OSHA.Repository.StatusReport;
using HC.OSHA.Service.StatusReport;
using HC_Core;
using HC_Core.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data;

namespace HC_OSHA
{
    public partial class CardPage_14_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {
        private HealthCheckService healthCheckService;

        public CardPage_14_Form()
        {
            InitializeComponent();
            healthCheckService = new HealthCheckService();
        }

        public void Select(IEstimateModel estimateModel)
        {
            base.SelectedEstimate = estimateModel;
            Search();
        }

        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;
        }

        private void Search()
        {
            if (base.SelectedSite == null) return;

            string strStartDate = base.SelectedEstimate.CONTRACTSTARTDATE.Left(4) + "0101";
            string strEndDate = base.SelectedEstimate.CONTRACTSTARTDATE.Left(4) + "1231";
            long nReportId = -1;
            string strSangdam = "";
            string strName1 = "";
            string strName2 = "";
            string strOldDate = "";

            List<HealthCheckDto> list = healthCheckService.healthCheckRepository.FindAll(base.SelectedSite.ID,0, strStartDate, strEndDate, false);

            SSCard.ActiveSheet.RowCount = 2;
            int row = 2;

            for (int i = 0; i < list.Count; i++)
            {
                SSCard.ActiveSheet.RowCount++;
                if (strOldDate!= list[i].CHARTDATE)
                {
                    strOldDate = list[i].CHARTDATE;
                    SSCard.ActiveSheet.Cells[row, 0].Value = list[i].CHARTDATE.Substring(4, 2) + "-" + list[i].CHARTDATE.Substring(6, 2);
                }
                SSCard.ActiveSheet.Cells[row, 0].ColumnSpan = 1;
                SSCard.ActiveSheet.Cells[row, 1].Value = list[i].name;
                SSCard.ActiveSheet.Cells[row, 2].Value = list[i].dept;
                SSCard.ActiveSheet.Cells[row, 3].Value = list[i].gender + "(" + list[i].age + ")";

                string exam = string.Empty;
                if (!list[i].bpl.IsNullOrEmpty())
                {
                    exam += "혈압: " + list[i].bpl + "/" + list[i].bpr + " ";
                }
                if (!list[i].bst.IsNullOrEmpty())
                {
                    exam += "혈당: " + list[i].bst + " ";
                }
                if (!list[i].dan.IsNullOrEmpty())
                {
                    exam += "단백뇨: " + list[i].dan + " ";
                }
                if (!list[i].WEIGHT.IsNullOrEmpty())
                {
                    exam += "체중: " + list[i].WEIGHT + " ";
                }
                if (!list[i].BMI.IsNullOrEmpty())
                {
                    exam += "체지방: " + list[i].BMI + " ";
                }
                if (!list[i].ALCHOL.IsNullOrEmpty())
                {
                    exam += "음주량: " + list[i].ALCHOL + " ";
                }
                if (!list[i].SMOKE.IsNullOrEmpty())
                {
                    exam += "흡연량: " + list[i].SMOKE + " ";
                }

                SSCard.ActiveSheet.Cells[row, 4].Text = exam + "\n" + list[i].content + "\n";
                SSCard.ActiveSheet.Cells[row, 5].Text = list[i].suggestion;

                // 상담자, 사업주확인
                if (list[i].REPORT_ID != nReportId)
                {
                    nReportId = list[i].REPORT_ID;
                    strSangdam = Get_SangdamName(nReportId);
                    strName1 = "";
                    strName2 = "";
                    if (strSangdam != "")
                    {
                        strName1 = VB.Pstr(strSangdam, "/", 1);
                        strName2 = VB.Pstr(strSangdam, "/", 2);
                    }
                }
                SSCard.ActiveSheet.Cells[row, 6].Text = strName1;
                SSCard.ActiveSheet.Cells[row, 7].Text = strName2;
                SSCard_Sheet1.Rows[row].Height = SSCard_Sheet1.Rows[row].GetPreferredHeight();

                row = row + 1;
            }

        }

        //상담자, 사업주확인 찾기
        private string Get_SangdamName(long reportId)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strSangdamName = "";

            SQL = "SELECT NURSENAME,SITEMANAGERNAME FROM HIC_OSHA_REPORT_NURSE ";
            SQL = SQL + ComNum.VBLF + "WHERE ID=" + reportId + " ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                strSangdamName = dt.Rows[0]["NURSENAME"].ToString().Trim() + "/";
                strSangdamName += dt.Rows[0]["SITEMANAGERNAME"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            if (strSangdamName == "")
            {
                SQL = "SELECT DOCTORNAME,SITEMANAGERNAME FROM HIC_OSHA_REPORT_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE ID=" + reportId + " ";
                SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    strSangdamName = dt.Rows[0]["DOCTORNAME"].ToString().Trim() + "/";
                    strSangdamName += dt.Rows[0]["SITEMANAGERNAME"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }

            return strSangdamName;
        }

        public void Print()
        {
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
        }

        public bool NewPrint()
        {
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
            return true;
        }

        private void BtnPrint_Click_1(object sender, EventArgs e)
        {
            Print();
        }
    }
}
