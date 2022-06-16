using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using HC.OSHA.Dto;
using HC.OSHA.Repository;
using HC.OSHA.Service;
using HC_Core;
using HC_OSHA.StatusReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HC_OSHA
{

    /// <summary>
    /// 상태보고서 검진 연동 데이타 가져오기
    /// </summary>
    public partial class StatisReportDataLinkForm : CommonForm
    {
        private SiteStatusControl siteStatusControl;
        private HcOshaCard5Service hcOshaCard5Service;
        private HcOshaCard6Service hcOshaCard6Service;
        private HicJepsuLtdService hicJepsuLtdService;
        private HicJepsuResSpecialLtdService hicJepsuResSpecialLtdService;
        HcOshaVisitInformationService hcOshaVisitInformationService;

        HicOshaGeneralResultRepository hicOshaGeneralResultRepository;
        HicOshaSpecialResultRepository hicOshaSpecialResultRepository;

        public StatisReportDataLinkForm()
        {
            InitializeComponent();

            hicOshaGeneralResultRepository = new HicOshaGeneralResultRepository();
            hicOshaSpecialResultRepository = new HicOshaSpecialResultRepository();

            hcOshaCard5Service = new HcOshaCard5Service();
            hcOshaCard6Service = new HcOshaCard6Service();
            hicJepsuLtdService = new HicJepsuLtdService();
            hicJepsuResSpecialLtdService = new HicJepsuResSpecialLtdService();
            hcOshaVisitInformationService = new HcOshaVisitInformationService();
            SSInoutList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSInoutList.AddColumnDateTime("등록일자", nameof(HC_OSHA_CARD5.REGISTERDATE), 120, IsReadOnly.Y, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String });
            
            SSInoutList.AddColumnText("입사자", nameof(HC_OSHA_CARD5.JOINCOUNT), 60, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSInoutList.AddColumnText("퇴사자", nameof(HC_OSHA_CARD5.QUITCOUNT), 60, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSInoutList.AddColumnButton("", 60, new SpreadCellTypeOption { ButtonText = "적용" }).ButtonClick += StatisReportDataLinkForm_ButtonClick;
            
            SpreadComboBoxData INFORMATIONTYPE = codeService.GetSpreadComboBoxData("VISIT_INFORMATION_KIND", "OSHA");
            SSInformationList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSInformationList.AddColumnDateTime("제공일자", nameof(HC_OSHA_VISIT_INFORMATION.REGDATE), 120, IsReadOnly.Y, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = true, IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String });
            SSInformationList.AddColumnComboBox("정보구분", nameof(HC_OSHA_VISIT_INFORMATION.INFORMATIONTYPE), 100, IsReadOnly.Y, INFORMATIONTYPE, new SpreadCellTypeOption { IsSort = false });
            SSInformationList.AddColumnText("내용", nameof(HC_OSHA_VISIT_INFORMATION.REMARK), 310, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSInformationList.AddColumnText("발급자", nameof(HC_OSHA_VISIT_INFORMATION.REGUSERNAME), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSInformationList.AddColumnButton("", 60, new SpreadCellTypeOption { ButtonText = "적용" }).ButtonClick += StatisReportDataLinkForm_ButtonClick1;

            SSGeneralHealthCare.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSGeneralHealthCare.AddColumnText("년도", nameof(HIC_OSHA_GENEAL_RESULT.YEAR), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSGeneralHealthCare.AddColumnText("총대상자수", nameof(HIC_OSHA_GENEAL_RESULT.TOTALCOUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSGeneralHealthCare.AddColumnText("D2", nameof(HIC_OSHA_GENEAL_RESULT.D2COUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSGeneralHealthCare.AddColumnText("C", nameof(HIC_OSHA_GENEAL_RESULT.C2COUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSGeneralHealthCare.AddColumnButton("적용", 70, new SpreadCellTypeOption { ButtonText = "적용" }).ButtonClick += StatisReportDataLinkForm_ButtonClick2; ;

            SSSpecialHealthCheckList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSSpecialHealthCheckList.AddColumnText("년도", nameof(HIC_OSHA_SPECIAL_RESULT.YEAR), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSSpecialHealthCheckList.AddColumnText("총대상자수", nameof(HIC_OSHA_SPECIAL_RESULT.TOTALCOUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSSpecialHealthCheckList.AddColumnText("D1", nameof(HIC_OSHA_SPECIAL_RESULT.D1COUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSSpecialHealthCheckList.AddColumnText("C1", nameof(HIC_OSHA_SPECIAL_RESULT.C1COUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSSpecialHealthCheckList.AddColumnText("D2", nameof(HIC_OSHA_SPECIAL_RESULT.D2COUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSSpecialHealthCheckList.AddColumnText("C2", nameof(HIC_OSHA_SPECIAL_RESULT.C2COUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSSpecialHealthCheckList.AddColumnText("DN", nameof(HIC_OSHA_SPECIAL_RESULT.DNCOUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSSpecialHealthCheckList.AddColumnText("CN", nameof(HIC_OSHA_SPECIAL_RESULT.CNCOUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSSpecialHealthCheckList.AddColumnButton("적용", 70, new SpreadCellTypeOption { ButtonText = "적용" }).ButtonClick += StatisReportDataLinkForm_ButtonClick3;
        }

        private void StatisReportDataLinkForm_ButtonClick3(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            DESEASE_COUNT_MODEL model = new DESEASE_COUNT_MODEL();

            model.D2 = long.Parse(SSSpecialHealthCheckList_Sheet1.Cells[e.Row, 4].Text);
            model.C2 = long.Parse(SSSpecialHealthCheckList_Sheet1.Cells[e.Row, 5].Text);
            model.D1 = long.Parse(SSSpecialHealthCheckList_Sheet1.Cells[e.Row, 2].Text);
            model.C1 = long.Parse(SSSpecialHealthCheckList_Sheet1.Cells[e.Row, 3].Text);
            model.CN = long.Parse(SSSpecialHealthCheckList_Sheet1.Cells[e.Row, 7].Text);
            model.DN = long.Parse(SSSpecialHealthCheckList_Sheet1.Cells[e.Row, 6].Text);
            model.JEPDATE = null;  //dto.JEPDATE;
            model.SpecialTotalCount = long.Parse(SSSpecialHealthCheckList_Sheet1.Cells[e.Row, 1].Text);
            siteStatusControl.SetSpecialCount(model);
        }

        private void StatisReportDataLinkForm_ButtonClick2(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            DESEASE_COUNT_MODEL model = new DESEASE_COUNT_MODEL();
            model.D2 = long.Parse(SSGeneralHealthCare_Sheet1.Cells[e.Row, 2].Text);
            model.C2 = long.Parse(SSGeneralHealthCare_Sheet1.Cells[e.Row, 3].Text);
            model.GeneralTotalCount = long.Parse(SSGeneralHealthCare_Sheet1.Cells[e.Row, 1].Text);
            model.JEPDATE = null;  //dto.JEPDATE;
            siteStatusControl.SetGeneralCount(model);
        }

        private void StatisReportDataLinkForm_ButtonClick1(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            SetInformation(e.Row);
        }
        private void SetInformation(int row)
        {
            HC_OSHA_VISIT_INFORMATION dto = SSInformationList.GetRowData(row) as HC_OSHA_VISIT_INFORMATION;
            if (dto != null)
            {
                siteStatusControl.SetInformation(dto.REMARK);
            }
        }

        private void StatisReportDataLinkForm_ButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            SetInoutEmployee(e.Row);
        }

        private void SetInoutEmployee(int row)
        {
            HC_OSHA_CARD5 card5 = SSInoutList.GetRowData(row) as HC_OSHA_CARD5;
            if (card5 != null)
            {
                siteStatusControl.SetInoutEmployee((int)card5.JOINCOUNT, (int)card5.QUITCOUNT);
                //MessageUtil.Info(" 적용하였습니다 ");
            }
        }

        public void SetStatisReportDataLinkForm(SiteStatusControl siteStatusControl)
        {
            this.siteStatusControl = siteStatusControl;
            
        }
        private void SSInoutList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            SetInoutEmployee(e.Row);
        }

        private void StatisReportDataLinkForm_Load(object sender, EventArgs e)
        {
            //DateTime currentDate = codeService.CurrentDate;
            //for(int i=0; i<10; i++)
            //{
            //    DateTime yearDate = currentDate.AddYears(-i);
            //    CboGeneralYear.Items.Add(yearDate.Year);
            //    CboSpecialYear.Items.Add(yearDate.Year);
            //}
            //CboGeneralYear.SelectedIndex = 0;
            //CboSpecialYear.SelectedIndex = 0;

           // frmHcPanGenMedExamResult_New = new frmHcPanGenMedExamResult_New();
           // frmHcPanSpcDiagnosisResultReport = new frmHcPanSpcDiagnosisResultReport();



            //입퇴사자
            List<HC_OSHA_CARD5> list = hcOshaCard5Service.FindAll(siteStatusControl.getCommonForm().SelectedEstimate.ID);
            SSInoutList.SetDataSource(list);

            //정보자료제공
            List<HC_OSHA_VISIT_INFORMATION> list2 = hcOshaVisitInformationService.FindAll(siteStatusControl.getCommonForm().SelectedSite.ID);
            SSInformationList.SetDataSource(list2);

            //산업재해
            IndustrailAccidentLoad();

            GetGeneralHealthCheck();
               
            GetSpecailHealthCheck();

        }
        /// <summary>
        /// 특수건강진단 유소견자 수
        /// </summary>
        private  void GetSpecailHealthCheck()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int i = 0;
            long nCnt = 0;
            string strNewData = "";
            string strOldData = "";
            string strGGubun = "";
            long nTotCnt = 0;
            long nD1 = 0;
            long nC1 = 0;
            long nD2 = 0;
            long nC2 = 0;
            long nDN = 0;
            long nCN = 0;

            int nRow = 0;

            List<HIC_OSHA_SPECIAL_RESULT> list = new List<HIC_OSHA_SPECIAL_RESULT>();
            
            string startYear = DateTime.Now.AddYears(-5).ToString("yyyy");

            try
            {
                SQL = "SELECT YEAR,GGUBUN,COUNT(*) AS CNT ";
                SQL = SQL + ComNum.VBLF + " FROM HIC_LTD_RESULT3 ";
                SQL = SQL + ComNum.VBLF + "WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SITEID=" + siteStatusControl.getCommonForm().SelectedSite.ID + " ";
                SQL = SQL + ComNum.VBLF + "  AND JONG='특수' ";
                SQL = SQL + ComNum.VBLF + "  AND YEAR>='" + startYear + "' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY YEAR,GGUBUN ";
                SQL = SQL + ComNum.VBLF + "ORDER BY YEAR DESC,GGUBUN ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strNewData = dt.Rows[i]["YEAR"].ToString().Trim();
                        nCnt = long.Parse(dt.Rows[i]["CNT"].ToString());
                        if (strOldData == "") strOldData = strNewData;
                        if (strOldData != strNewData)
                        {
                            nRow++;
                            SSSpecialHealthCheckList.ActiveSheet.RowCount = nRow;
                            SSSpecialHealthCheckList_Sheet1.Cells[nRow - 1, 0].Text = strOldData;
                            SSSpecialHealthCheckList_Sheet1.Cells[nRow - 1, 1].Text = nTotCnt.ToString();
                            SSSpecialHealthCheckList_Sheet1.Cells[nRow - 1, 2].Text = nC1.ToString();
                            SSSpecialHealthCheckList_Sheet1.Cells[nRow - 1, 3].Text = nC2.ToString();
                            SSSpecialHealthCheckList_Sheet1.Cells[nRow - 1, 4].Text = nD1.ToString();
                            SSSpecialHealthCheckList_Sheet1.Cells[nRow - 1, 5].Text = nD2.ToString();
                            SSSpecialHealthCheckList_Sheet1.Cells[nRow - 1, 6].Text = nCN.ToString();
                            SSSpecialHealthCheckList_Sheet1.Cells[nRow - 1, 7].Text = nDN.ToString();

                            strOldData = strNewData;
                            nC2 = 0;
                            nD2 = 0;
                            nC1 = 0;
                            nD1 = 0;
                            nCN = 0;
                            nDN = 0;
                        }
                        strGGubun = dt.Rows[i]["GGUBUN"].ToString().Trim();
                        nTotCnt += nCnt;
                        if (VB.InStr(strGGubun, "D1") > 0) nD1 += nCnt;
                        if (VB.InStr(strGGubun, "C1") > 0) nC1 += nCnt;
                        if (VB.InStr(strGGubun, "D2") > 0) nD2 += nCnt;
                        if (VB.InStr(strGGubun, "C2") > 0) nC2 += nCnt;
                        if (VB.InStr(strGGubun, "DN") > 0) nDN += nCnt;
                        if (VB.InStr(strGGubun, "CN") > 0) nCN += nCnt;
                    }
                    nRow++;
                    SSSpecialHealthCheckList.ActiveSheet.RowCount = nRow;
                    SSSpecialHealthCheckList_Sheet1.Cells[nRow - 1, 0].Text = strOldData;
                    SSSpecialHealthCheckList_Sheet1.Cells[nRow - 1, 1].Text = nTotCnt.ToString();
                    SSSpecialHealthCheckList_Sheet1.Cells[nRow - 1, 2].Text = nC1.ToString();
                    SSSpecialHealthCheckList_Sheet1.Cells[nRow - 1, 3].Text = nC2.ToString();
                    SSSpecialHealthCheckList_Sheet1.Cells[nRow - 1, 4].Text = nD1.ToString();
                    SSSpecialHealthCheckList_Sheet1.Cells[nRow - 1, 5].Text = nD2.ToString();
                    SSSpecialHealthCheckList_Sheet1.Cells[nRow - 1, 6].Text = nCN.ToString();
                    SSSpecialHealthCheckList_Sheet1.Cells[nRow - 1, 7].Text = nDN.ToString();
                }

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
        }

        /// <summary>
        /// 일반건강검진 유소견자 수
        /// </summary>
        private void GetGeneralHealthCheck()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            long nCnt = 0;
            int i = 0;
            string strNewData = "";
            string strOldData = "";
            string strGGubun = "";
            long nTotCnt = 0;
            long nD2 = 0;
            long nC2 = 0;
            int nRow = 0;

            List<HIC_OSHA_GENEAL_RESULT> list = new List<HIC_OSHA_GENEAL_RESULT>();

            string startYear = DateTime.Now.AddYears(-5).ToString("yyyy");

            try
            {
                SQL = "SELECT YEAR,GGUBUN,COUNT(*) AS CNT ";
                SQL = SQL + ComNum.VBLF + " FROM HIC_LTD_RESULT3 ";
                SQL = SQL + ComNum.VBLF + "WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SITEID=" + siteStatusControl.getCommonForm().SelectedSite.ID + " ";
                SQL = SQL + ComNum.VBLF + "  AND JONG='일반' ";
                SQL = SQL + ComNum.VBLF + "  AND YEAR>='" + startYear + "' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY YEAR,GGUBUN ";
                SQL = SQL + ComNum.VBLF + "ORDER BY YEAR DESC,GGUBUN ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strNewData = dt.Rows[i]["YEAR"].ToString().Trim();
                        nCnt = long.Parse(dt.Rows[i]["CNT"].ToString());
                        if (strOldData == "") strOldData = strNewData;
                        if (strOldData != strNewData)
                        {
                            nRow++;
                            SSGeneralHealthCare.ActiveSheet.RowCount = nRow;
                            SSGeneralHealthCare_Sheet1.Cells[nRow - 1, 0].Text = strOldData;
                            SSGeneralHealthCare_Sheet1.Cells[nRow - 1, 1].Text = nTotCnt.ToString();
                            SSGeneralHealthCare_Sheet1.Cells[nRow - 1, 2].Text = nD2.ToString();
                            SSGeneralHealthCare_Sheet1.Cells[nRow - 1, 3].Text = nC2.ToString();

                            strOldData = strNewData;
                            nC2 = 0;
                            nD2 = 0;
                        }
                        strGGubun = dt.Rows[i]["GGUBUN"].ToString().Trim();
                        nTotCnt += nCnt;
                        if (VB.InStr(strGGubun, "C") > 0) nC2 += nCnt;
                        if (VB.InStr(strGGubun, "D2") > 0) nD2 += nCnt; 
                    }
                    nRow++;
                    SSGeneralHealthCare.ActiveSheet.RowCount = nRow;
                    SSGeneralHealthCare_Sheet1.Cells[nRow - 1, 0].Text = strOldData;
                    SSGeneralHealthCare_Sheet1.Cells[nRow - 1, 1].Text = nTotCnt.ToString();
                    SSGeneralHealthCare_Sheet1.Cells[nRow - 1, 2].Text = nD2.ToString();
                    SSGeneralHealthCare_Sheet1.Cells[nRow - 1, 3].Text = nC2.ToString();
                }

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
        }

        private void SSHealthCareJepsuList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //Cursor.Current = Cursors.WaitCursor;
            //try
            //{
            //    HIC_JEPSU_LTD model = SSGeneralHealthCare.GetRowData(e.Row) as HIC_JEPSU_LTD;

            //    DESEASE_COUNT_MODEL count = frmHcPanGenMedExamResult_New.Search(model.MINDATE, model.MAXDATE, CboGeneralYear.Text, model.LTDCODE);
            //    //      NumGeneralD2Count.Value = count.D2;
            //    //    NumGeneralC2Count.Value = count.C2;
            //    PanGeneral.SetData(count);

            //    MessageUtil.Info("일반검진 현황을 가져왔습니다");
            //}
            //catch (Exception ex)
            //{
            //    MessageUtil.Alert(ex.Message);
            //}
            //finally
            //{
            //    Cursor.Current = Cursors.Default;
            //}
        }

  
        private void BtnInOutEmployee_Click(object sender, EventArgs e)
        {
            InOutEmployeeForm form = new InOutEmployeeForm();
            form.SelectedEstimate = siteStatusControl.getCommonForm().SelectedEstimate;
            form.SelectedSite = siteStatusControl.getCommonForm().SelectedSite;
            form.Select();
            form.ShowDialog();

            List<HC_OSHA_CARD5> list = hcOshaCard5Service.FindAll(siteStatusControl.getCommonForm().SelectedEstimate.ID);
            SSInoutList.SetDataSource(list);
        }

        private void BtnAccident_Click(object sender, EventArgs e)
        {

            IndustrialAccidentForm form = new IndustrialAccidentForm();
            form.SelectedEstimate = siteStatusControl.getCommonForm().SelectedEstimate;
            form.SelectedSite = siteStatusControl.getCommonForm().SelectedSite;
            form.Select();
            form.ShowDialog();


            IndustrailAccidentLoad();
        }

        private void IndustrailAccidentLoad()
        {
            int deathCount = 0;
            int accidentCount = 0;
            int diseaseCount = 0;
            List<HC_OSHA_CARD6> list = hcOshaCard6Service.FindAll(siteStatusControl.getCommonForm().SelectedEstimate.ID);
            foreach (HC_OSHA_CARD6 dto in list)
            {
                if (dto.IND_ACC_TYPE == "1")
                {
                    ++deathCount;
                }
                else if (dto.IND_ACC_TYPE == "2")
                {
                    ++accidentCount;
                }
                else if (dto.IND_ACC_TYPE == "2")
                {
                    ++diseaseCount;
                }
            }

            TxtDeathCount.Text = deathCount.ToString();
            TxtAccidentCount.Text = accidentCount.ToString();
            TxtDiseaseCount.Text = diseaseCount.ToString();
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            siteStatusControl.SetAcciendt(TxtDeathCount.Text.To<int>(0), TxtAccidentCount.Text.To<int>(0), TxtDiseaseCount.Text.To<int>(0)) ; 
        }

        private void BtnInformation_Click(object sender, EventArgs e)
        {
            InformationForm form = new InformationForm();
            form.SelectedEstimate = siteStatusControl.getCommonForm().SelectedEstimate;
            form.SelectedSite = siteStatusControl.getCommonForm().SelectedSite;
            form.Select();
            form.ShowDialog();

            List<HC_OSHA_VISIT_INFORMATION> list = hcOshaVisitInformationService.FindAll(siteStatusControl.getCommonForm().SelectedSite.ID);
            SSInformationList.SetDataSource(list);
        }

        private void SSInformationList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            SetInformation(e.Row);
        }

        private void BtnSearchGeneral_Click(object sender, EventArgs e)
        {
            GetGeneralHealthCheck();
        }

        private void BtnSearchSpecial_Click(object sender, EventArgs e)
        {
            GetSpecailHealthCheck();
        }
    }
}
