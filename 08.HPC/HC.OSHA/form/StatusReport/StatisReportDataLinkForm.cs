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

        //일반검진표
        //frmHcPanGenMedExamResult_New frmHcPanGenMedExamResult_New;
        //특수검진표
        //frmHcPanSpcDiagnosisResultReport frmHcPanSpcDiagnosisResultReport;

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
            SSGeneralHealthCare.AddColumnText("C2", nameof(HIC_OSHA_GENEAL_RESULT.C2COUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSGeneralHealthCare.AddColumnButton("", 70, new SpreadCellTypeOption { ButtonText = "적용" }).ButtonClick += StatisReportDataLinkForm_ButtonClick2; ;

            SSSpecialHealthCheckList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSSpecialHealthCheckList.AddColumnText("년도", nameof(HIC_OSHA_SPECIAL_RESULT.YEAR), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSSpecialHealthCheckList.AddColumnText("총대상자수", nameof(HIC_OSHA_SPECIAL_RESULT.TOTALCOUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSSpecialHealthCheckList.AddColumnText("D1", nameof(HIC_OSHA_SPECIAL_RESULT.D1COUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSSpecialHealthCheckList.AddColumnText("C1", nameof(HIC_OSHA_SPECIAL_RESULT.C1COUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSSpecialHealthCheckList.AddColumnText("D2", nameof(HIC_OSHA_SPECIAL_RESULT.D2COUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSSpecialHealthCheckList.AddColumnText("C2", nameof(HIC_OSHA_SPECIAL_RESULT.C2COUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSSpecialHealthCheckList.AddColumnText("DN", nameof(HIC_OSHA_SPECIAL_RESULT.DNCOUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSSpecialHealthCheckList.AddColumnText("CN", nameof(HIC_OSHA_SPECIAL_RESULT.CNCOUNT), 40, IsReadOnly.Y, new SpreadCellTypeOption { });
            SSSpecialHealthCheckList.AddColumnButton("", 70, new SpreadCellTypeOption { ButtonText = "적용" }).ButtonClick += StatisReportDataLinkForm_ButtonClick3;
        }

        private void StatisReportDataLinkForm_ButtonClick3(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            HIC_OSHA_SPECIAL_RESULT dto = SSSpecialHealthCheckList.GetRowData(e.Row) as HIC_OSHA_SPECIAL_RESULT;
            dto.SITE_ID = siteStatusControl.getCommonForm().SelectedSite.ID;
            dto.JEPDATE = hicOshaGeneralResultRepository.FindBySpecialMinJepDate(dto.SITE_ID, dto.YEAR);

            DESEASE_COUNT_MODEL model = new DESEASE_COUNT_MODEL();
            model.D2 = dto.D2COUNT;
            model.C2 = dto.C2COUNT;
            model.D1 = dto.D1COUNT;
            model.C1 = dto.C1COUNT;
            model.CN = dto.CNCOUNT;
            model.DN = dto.DNCOUNT;
            model.JEPDATE = dto.JEPDATE;
            model.SpecialTotalCount = dto.TOTALCOUNT;
            siteStatusControl.SetSpecialCount(model);
        }

        private void StatisReportDataLinkForm_ButtonClick2(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            HIC_OSHA_GENEAL_RESULT dto = SSGeneralHealthCare.GetRowData(e.Row) as HIC_OSHA_GENEAL_RESULT;
            dto.SITE_ID = siteStatusControl.getCommonForm().SelectedSite.ID;
            dto.JEPDATE = hicOshaGeneralResultRepository.FindByMinJepDate(dto.SITE_ID, dto.YEAR);

            DESEASE_COUNT_MODEL model = new DESEASE_COUNT_MODEL();
            model.D2 = dto.D2COUNT;
            model.C2 = dto.C2COUNT;
            model.GeneralTotalCount = dto.TOTALCOUNT;
            model.JEPDATE = dto.JEPDATE;
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
            string startYear = DateTime.Now.AddYears(-5).ToString("yyyy");
            string endYear = DateTime.Now.ToString("yyyy");

            //List<HIC_OSHA_SPECIAL_RESULT> list = hicOshaSpecialResultRepository.FindAll(siteStatusControl.getCommonForm().SelectedSite.ID);
            List<HIC_OSHA_SPECIAL_RESULT> list = hicOshaSpecialResultRepository.FindAllNew(siteStatusControl.getCommonForm().SelectedSite.ID, startYear, endYear);
            SSSpecialHealthCheckList.SetDataSource(list);
        }

        /// <summary>
        /// 속도문제로 빌드 방식으로 변경함.
        /// </summary>
        private void GetGeneralHealthCheck()
        {
            //일반검진 접수명단
            string startYear = DateTime.Now.AddYears(-5).ToString("yyyy");
            string endYear = DateTime.Now.ToString("yyyy");
            List<HIC_OSHA_GENEAL_RESULT> list = hicOshaGeneralResultRepository.FindAllNew(siteStatusControl.getCommonForm().SelectedSite.ID, startYear, endYear);
            SSGeneralHealthCare.SetDataSource(list);
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
