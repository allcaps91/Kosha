using HC.OSHA.Service;
using ComBase;
using HC_Core;
using HC_OSHA.Service.Visit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase.Mvc;
using ComBase.Controls;
using HC_OSHA.Model.Visit;
using ComBase.Mvc.Utils;
using HC.OSHA.Repository;
using HC.OSHA.Model;
using HC.Core.Common.Util;

namespace HC_OSHA.Visit
{
    /// <summary>
    /// 출장일지
    /// </summary>
    public partial class DailyReportForm : CommonForm
    {
        private DailyVisitReportService dailyVisitReportService;
        private ScheduleModelRepository scheduleModelRepository;
        public DailyReportForm()
        {
            string strGPath = "";
            string strNow = DateTime.Now.ToString("yyyy-MM-dd");

            InitializeComponent();
            dailyVisitReportService = new DailyVisitReportService();
            scheduleModelRepository = new ScheduleModelRepository();
            SSList.ShowRow(0, 0, FarPoint.Win.Spread.VerticalPosition.Top);

            //작업일자 기준으로 결재정보를 읽음
            strGPath = Get_Approve_Path(strNow);

            SSList.ActiveSheet.Cells[2, 27].Text = VB.Pstr(strGPath, ",", 1);
            SSList.ActiveSheet.Cells[2, 30].Text = VB.Pstr(strGPath, ",", 2);
            SSList.ActiveSheet.Cells[2, 33].Text = VB.Pstr(strGPath, ",", 3);
        }
        public void Clear()
        {
            int rowIndex = 8;
            for (int i = rowIndex; i < 24; i++)
            {
                SSList.ActiveSheet.Cells[i, 0].Value = "";
                SSList.ActiveSheet.Cells[i, 6].Value = "";
                SSList.ActiveSheet.Cells[i, 19].Value = "";
                SSList.ActiveSheet.Cells[i, 23].Value = "";
                SSList.ActiveSheet.Cells[i, 26].Value = "";
                SSList.ActiveSheet.Cells[i, 29].Value = "";
                SSList.ActiveSheet.Cells[i, 32].Value = "";
            }
            SSList.ActiveSheet.ActiveRowIndex = 0;
            SSList.ShowRow(0, 0, FarPoint.Win.Spread.VerticalPosition.Top);


            //6칸식
            for (int i = 27; i < SSList.RowCount(); i++)
            {
                SSList.ActiveSheet.Cells[i, 1].Value = "";
                SSList.ActiveSheet.Cells[i, 4].Value = "";
                SSList.ActiveSheet.Cells[i, 7].Value = "";
                SSList.ActiveSheet.Cells[i, 18].Value = "";
            }
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string strGPath = "";

            Clear();

            string date = DtpVisitDate.Value.ToString("yyyy-MM-dd");

            //작업일자 기준으로 결재정보를 읽음
            strGPath = Get_Approve_Path(date);

            SSList.ActiveSheet.Cells[2, 26].Text = VB.Pstr(strGPath, ",", 1);
            SSList.ActiveSheet.Cells[2, 29].Text = VB.Pstr(strGPath, ",", 2);
            SSList.ActiveSheet.Cells[2, 32].Text = VB.Pstr(strGPath, ",", 3);
        
            string title = "◎출장일:" + date.Substring(0, 4) + "년 " + date.Substring(5, 2) + "월 " + date.Substring(8, 2) + "일(" + DateUtil.ToDayOfWeek(DtpVisitDate.Value).Substring(0, 1) + ")";

            SSList.ActiveSheet.Cells[4, 0].Value = title;

            List<DailyReportSiteModel> list = dailyVisitReportService.DailyReportRepository.FindVisitSite(date);
            int rowIndex = 8;
            for (int i = 0; i < list.Count; i++)
            {
                SSList.ActiveSheet.Cells[rowIndex, 0].Value = list[i].Name;
                SSList.ActiveSheet.Cells[rowIndex, 6].Value = list[i].Address;
                SSList.ActiveSheet.Cells[rowIndex, 19].Value = list[i].Tel;

                SSList.ActiveSheet.Cells[rowIndex, 23].Value = list[i].SITE_MANAGER;
                if (list[i].ROLE == "NURSE")
                {
                    SSList.ActiveSheet.Cells[rowIndex, 26].Value = list[i].VISITUSERNAME;
                }
                else
                {
                    SSList.ActiveSheet.Cells[rowIndex, 29].Value = list[i].VISITUSERNAME;
                }

                SSList.ActiveSheet.Cells[rowIndex, 32].Value = list[i].VISITDOCTORNAME;
                ++rowIndex;
            }

            List<DailyReportVisitModel> visitList = dailyVisitReportService.DailyReportRepository.FindVisitList(date);
            //rowIndex = 25;
            rowIndex = 27;
            DailyReportVisitModel preItem = null;
            string siteNames = "";

            string doctorRemark = "＊직업병 예방 활동" + Environment.NewLine + "＊건강증진지도" + Environment.NewLine + "＊뇌심혈관질환 예방활동" + Environment.NewLine + "＊현장 순회";
            string nurseRemark = "＊질병유소견자 관리지도" + Environment.NewLine + "＊건강증진지도" + Environment.NewLine + "＊뇌심혈관질환 예방활동" + Environment.NewLine + "＊건강정보지 제공" + Environment.NewLine + "＊보건관련서류 점검";
            string engRemark = "＊현장순회 및 점검" + Environment.NewLine + "＊산업재해 예방지도" + Environment.NewLine + "＊보건관련 서류점검" + Environment.NewLine + "＊보건관리 업무 협의";

            int lastRowIndex = 0;
            for (int i = 0; i < visitList.Count; i++)
            {
                for (int j = rowIndex + 6; j < SSList.ActiveSheet.RowCount; j++)
                {
                     SSList.ActiveSheet.Rows[j].Visible = true;
                }
                if (i == 0)
                {

                }
                //if (preItem != null && preItem.VisitUserID != visitList[i].VisitUserID)
                if (preItem != null && preItem.VisitUserID != visitList[i].VisitUserID)
                {
                    rowIndex = rowIndex + 6;
                    siteNames = "";
                }
                string tomorrowDate = DtpVisitDate.Value.AddDays(1).ToString("yyyy-MM-dd");
                List<UnvisitSiteModel> scsheduleList = scheduleModelRepository.FindUnvisitSiteList(visitList[i].VisitUserID, tomorrowDate, tomorrowDate, "");
                string tomorrowSiteNames = "";
                foreach (UnvisitSiteModel site in scsheduleList)
                {
                    tomorrowSiteNames += site.NAME + Environment.NewLine;

                }
                SSList.ActiveSheet.Cells[rowIndex, 29].Value = tomorrowSiteNames;

                if (visitList[i].Role == "DOCTOR")
                {
                    SSList.ActiveSheet.Cells[rowIndex, 4].Value = "의사";
                    SSList.ActiveSheet.Cells[rowIndex, 18].Text = doctorRemark + Environment.NewLine;
                    if (visitList[i].REMARK.NotEmpty())
                    {
                        SSList.ActiveSheet.Cells[rowIndex, 18].Text = SSList.ActiveSheet.Cells[rowIndex, 18].Text + " * " + visitList[i].REMARK + Environment.NewLine;
                    }
                   
                }
                else if (visitList[i].Role == "NURSE")
                {
                    SSList.ActiveSheet.Cells[rowIndex, 4].Value = "간호사";
                    SSList.ActiveSheet.Cells[rowIndex, 18].Text = nurseRemark + Environment.NewLine;
                    if (visitList[i].REMARK.NotEmpty())
                    {
                        SSList.ActiveSheet.Cells[rowIndex, 18].Text = SSList.ActiveSheet.Cells[rowIndex, 18].Text + " * " + visitList[i].REMARK + Environment.NewLine;

                    }
                   

                }
                else
                {
                    SSList.ActiveSheet.Cells[rowIndex, 4].Value = "산업위생";
                    SSList.ActiveSheet.Cells[rowIndex, 18].Text = engRemark + Environment.NewLine;
                    if (visitList[i].REMARK.NotEmpty())
                    {
                        SSList.ActiveSheet.Cells[rowIndex, 18].Text = SSList.ActiveSheet.Cells[rowIndex, 18].Text +" * " + visitList[i].REMARK + Environment.NewLine;
                    }
                }
                siteNames += visitList[i].VisitTime +" "+ visitList[i].Name + Environment.NewLine;
                SSList.ActiveSheet.Cells[rowIndex, 7].Text = siteNames;
                
                SSList.ActiveSheet.Cells[rowIndex, 1].Value =  visitList[i].VisitUserName;

                preItem = visitList[i];
                lastRowIndex = rowIndex;
            }

            if (visitList.Count > 0)
            {
                for (int j = lastRowIndex + 6; j < SSList.ActiveSheet.RowCount; j++)
                {
                    SSList.ActiveSheet.Rows[j].Visible = false;
                }
            }
        }
        private void SSList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void DailyReportForm_Load(object sender, EventArgs e)
        {
            Clear();
            BtnSearch.PerformClick();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                SpreadPrint sp = new SpreadPrint(SSList, PrintStyle.STANDARD_APPROVAL);
                sp.Execute();
            }
            catch(Exception ex)
            {
                MessageUtil.Alert(ex.Message);
            }
            
        }

        // 특정일자를 기준으로 결재 경로명을 찾기
        public string Get_Approve_Path(string strGDate)
        {
            string SQL = "";
            string SqlErr = "";
            string strResult = "";
            DataTable dt = null;

            strResult = "";
            try
            {
                SQL = "";
                SQL = "SELECT * FROM HIC_CODES ";
                SQL = SQL + ComNum.VBLF + "WHERE SWLICENSE = '" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GROUPCODE = 'OSHA_APPROVE_PATH1' ";
                SQL = SQL + ComNum.VBLF + "  AND ISDELETED = 'N' ";
                SQL = SQL + ComNum.VBLF + "  AND CODE<='" + strGDate + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY CODE DESC ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0) strResult = dt.Rows[0]["CODENAME"].ToString().Trim();
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBox(ex.Message);
            }
            return strResult;
        }

    }
}
