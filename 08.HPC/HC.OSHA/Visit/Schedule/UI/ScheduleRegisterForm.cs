using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Exceptions;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using HC.Core.BaseCode.Management.Dto;
using HC.Core.BaseCode.Management.Repository;
using HC.Core.BaseCode.Management.Service;
using HC.Core.Common.Extension;
using HC.Core.Common.Interface;
using HC.Core.Common.Service;
using HC.Core.Common.UI;
using HC.Core.Site.Dto;
using HC.Core.Site.Model;
using HC.Core.Site.UI;
using HC.OSHA.Site.Card.UI;
using HC.OSHA.Site.Management.Dto;
using HC.OSHA.Site.Management.Model;
using HC.OSHA.Site.Management.Service;
using HC.OSHA.Visit.Management.UI;
using HC.OSHA.Visit.Schedule.Dto;
using HC.OSHA.Visit.Schedule.Model;
using HC.OSHA.Visit.Schedule.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;




namespace HC.OSHA.Visit.Schedule.UI
{
    public partial class ScheduleRegisterForm : CommonForm
    {
        private HcUserService hcUsersService;
        private HcOshaScheduleService hcOshaScheduleService;
        private HcOshaVisitService hcOshaVisitService;
        private SchduleModelService schduleModelService;
        private List<CommonForm> TabForms;
        /// <summary>
        /// 이벤트 고유아이디
        /// </summary>
        public string EventId { get; set; }
        /// <summary>
        /// 달력 네이비게이션 타입
        /// </summary>
        public string ViewType { get; set; }
        /// <summary>
        /// 이벤트 시작일
        /// </summary>
        public string StartDate { get; set; }
        public ScheduleRegisterForm()
        {
            InitializeComponent();
            hcUsersService = new HcUserService();
            hcOshaScheduleService = new HcOshaScheduleService();
            hcOshaVisitService = new HcOshaVisitService();
            schduleModelService = new SchduleModelService();
            TabForms = new List<CommonForm>();
        }

        private void ScheduleRegisterForm_Load(object sender, EventArgs e)
        {
            List<HC_USER> OSHAUsers = hcUsersService.GetOsha();
            List<HC_USER> doctors = hcUsersService.GetDoctors();

            DtpVISITRESERVEDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_SCHEDULE.VISITRESERVEDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD, });
            DtpVISITSTARTTIME.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_SCHEDULE.VISITSTARTTIME), DataBaseFormat = DateTimeType.HH_MM, DisplayFormat = DateTimeType.HH_MM });
            DtpVISITENDTIME.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_SCHEDULE.VISITENDTIME), DataBaseFormat = DateTimeType.HH_MM, DisplayFormat = DateTimeType.HH_MM });

            DtpDEPARTUREDATETIME.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_SCHEDULE.DEPARTUREDATETIME), DataBaseFormat = DateTimeType.HH_MM, DisplayFormat = DateTimeType.HH_MM });
            DtpARRIVALTIME.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_SCHEDULE.ARRIVALTIME), DataBaseFormat = DateTimeType.HH_MM, DisplayFormat = DateTimeType.HH_MM });
            NumWORKERCOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_SCHEDULE.WORKERCOUNT), });

            CboVISITUSERID.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_SCHEDULE.VISITUSERID) });
            CboVISITMANAGERID.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_SCHEDULE.VISITMANAGERID) });
       
            CboVISITUSERID.SetItems(OSHAUsers, "Name", "UserId");
            CboVISITMANAGERID.SetItems(OSHAUsers, "Name", "UserId");

            TxtREMARK.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_ESTIMATE.REMARK) });

            TxtINDOCPRINTDATETIME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_SCHEDULE.INDOCPRINTDATETIME), DisplayFormat = DateTimeType.YYYY_MM_DD_HH_MM, ReadOnly = true });
            TxtOUTDOCPRINTDATETIME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_SCHEDULE.OUTDOCPRINTDATETIME), DisplayFormat = DateTimeType.YYYY_MM_DD_HH_MM, ReadOnly = true });
            TxtSENDMAILDATETIME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_SCHEDULE.SENDMAILDATETIME), DisplayFormat = DateTimeType.YYYY_MM_DD_HH_MM, ReadOnly = true });
            TxtMODIFIED.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_SCHEDULE.MODIFIED), DisplayFormat = DateTimeType.YYYY_MM_DD_HH_MM, ReadOnly = true });
            TxtMODIFIEDUSER.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_SCHEDULE.MODIFIEDUSER), ReadOnly = true });
            
          


            //방문내역폼
            DtpVISITDATETIME.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_VISIT.VISITDATETIME), DataBaseFormat = DateTimeType.None, DisplayFormat = DateTimeType.YYYY_MM_DD, });
            DtpSTARTTIME.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_VISIT.STARTTIME), DataBaseFormat = DateTimeType.HH_MM, DisplayFormat = DateTimeType.HH_MM });
            DtpENDTIME.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_VISIT.ENDTIME), DataBaseFormat = DateTimeType.HH_MM, DisplayFormat = DateTimeType.HH_MM });

            TxtTakeHourAndMinute.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_VISIT.TakeHourAndMinute) });

            CboVISITUSER.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_VISIT.VISITUSER) });
            CboVISITDOCTOR.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_VISIT.VISITDOCTOR) });
            CboVISITTYPE.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_VISIT.VISITTYPE) });

            panVisit.SetData(new HC_OSHA_VISIT());
         
            CboVISITUSER.SetItems(OSHAUsers, "Name", "UserId");
            CboVISITDOCTOR.SetItems(doctors, "Name", "UserId");
            CboVISITTYPE.SetItems(codeService.FindActiveCodeByGroupCode("VISIT_TYPE", "OSHA"), "codename", "code" );
            TxtVisitREMARK.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_VISIT.REMARK) });
            ChkISFEE.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_VISIT.ISFEE), CheckValue = "Y", UnCheckValue = "N" });
            //ChkKUKGO.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_VISIT.ISFEE), CheckValue = "Y", UnCheckValue = "N" })

            InitalizeScheduleAndVisit();

            //검색 초기화
            InitializeSearchForm(OSHAUsers);

            EducationReportForm form = new EducationReportForm();
            form.SelectedSite = base.SelectedSite;
            AddForm(form, TabEduPage);
            TabForms.Add(form);
            CommitteeForm committeeForm = new CommitteeForm();
            form.SelectedSite = base.SelectedSite;
            AddForm(committeeForm, TabCommitteePage);
            TabForms.Add(committeeForm);

            InformationForm informationForm = new InformationForm();
            form.SelectedSite = base.SelectedSite;
            AddForm(informationForm, TabInformation);
            TabForms.Add(informationForm);

            IndustrialAccidentForm accForm = new IndustrialAccidentForm();
            accForm.SelectedSite = base.SelectedSite;
            AddForm(accForm, TabAccident);
            TabForms.Add(accForm);


            


        }

        public void InitalizeScheduleAndVisit()
        {
            if (EventId.IsNullOrEmpty())
            {
                HC_OSHA_SCHEDULE dto = new HC_OSHA_SCHEDULE();
                dto.EVENTSTARTDATETIME = DateUtil.stringToDateTime(StartDate.Substring(0, 10), DateTimeType.YYYY_MM_DD);
                dto.VISITRESERVEDATE = StartDate.Substring(0, 10);
                dto.VISITUSERID = CommonService.Instance.Session.UserId;
                panSchedule.SetData(dto);
            }
            else
            {
               
                SetScheduleAndVisitForm(long.Parse(EventId));
            }
        }

        private void InitializeSearchForm(List<HC_USER> OSHAUsers)
        {
            CboSearchUnVisitUserId.SetItems(OSHAUsers, "Name", "UserId", "전체", "", AddComboBoxPosition.Top);
            CboSearchUnVisitUserId.SetValue(CommonService.Instance.Session.UserId);

            DtpSearchUnVisitStartDate.SetValue(this.StartDate);

            CboSearchVisitUserId.SetItems(OSHAUsers, "Name", "UserId", "전체", "", AddComboBoxPosition.Top);
            //CboSearchVisitUserId.SetItems(OSHAUsers, "Name", "UserId", "전체", "", AddComboBoxPosition.Top);

            CboSearchVisitUserId.SetValue(CommonService.Instance.Session.UserId);
            CboSearchUnVisitUserId.SetValue(CommonService.Instance.Session.UserId);

            DateTime startDate = DateUtil.stringToDateTime(this.StartDate, DateTimeType.YYYY_MM_DD);
            DtpSearchUnVisitStartDate.SetValue(startDate.GetFirstDate());
            DtpSearchUnVisitEndDate.SetValue(startDate.GetLastDate());
            DtpSearchVisitStartDate.SetValue(startDate.GetFirstDate());
            DtpSearchVisitEndDate.SetValue(startDate.GetLastDate());


            SSUnVisit.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSUnVisit.AddColumnText("코드", nameof(UnvisitSiteModel.SITE_ID), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSUnVisit.AddColumnText("사업장명", nameof(UnvisitSiteModel.NAME), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSUnVisit.AddColumnText("예정일", nameof(UnvisitSiteModel.VISITRESERVEDATE), 76, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            BtnSearchUnVisit.PerformClick();

            SSVisit.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSVisit.AddColumnText("코드", nameof(VisitSiteModel.SITE_ID), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSVisit.AddColumnText("사업장명", nameof(VisitSiteModel.NAME), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSVisit.AddColumnText("방문일", nameof(VisitSiteModel.VISITDATETIME), 76, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            BtnSearchVisit.PerformClick();
            

        }
        private void BtnSaveSchedule_Click(object sender, EventArgs e)
        {
            if(base.SelectedSite == null)
            {
                MessageUtil.Info("사업장을 선택하세요");
            }
            else if (base.SelectedEstimate == null)
            {
                MessageUtil.Info("계약사항이 없는 사업장입니다");
            }
            else
            {
                if (panSchedule.Validate<HC_OSHA_SCHEDULE>())
                {
                    HC_OSHA_SCHEDULE dto = panSchedule.GetData<HC_OSHA_SCHEDULE>();

                    HC_OSHA_SCHEDULE saved = hcOshaScheduleService.Save(dto);

                    panSchedule.SetData(saved);

                    MessageUtil.Info(saved.SITE_NAME + " 일정이 저장되었습니다");
                }
            }
        

        }

        private void BtnDeleteSchedule_Click(object sender, EventArgs e)
        {
            HC_OSHA_SCHEDULE dto = panSchedule.GetData<HC_OSHA_SCHEDULE>();
            if (dto.ID > 0)
            {
                hcOshaScheduleService.Delete(dto);
                MessageUtil.Info("일정이 삭제 되었습니다");

                EventId = string.Empty;
                InitalizeScheduleAndVisit();
            }
        }

        private void BtnNewSchedule_Click(object sender, EventArgs e)
        {
            InitalizeScheduleAndVisit();
        }

    

   

        private void BtnSaveVisit_Click(object sender, EventArgs e)
        {
            HC_OSHA_SCHEDULE schedule = panSchedule.GetData<HC_OSHA_SCHEDULE>();
            if(schedule.ID == 0)
            {
                MessageUtil.Alert("일정을 먼저 등록하세요");
            }
            else
            {
                HC_OSHA_VISIT dto = panVisit.GetData<HC_OSHA_VISIT>();
                TimeSpan takeTime = DtpENDTIME.Value - DtpSTARTTIME.Value;


                dto.SCHEDULE_ID = schedule.ID;
                dto.TAKEHOUR = takeTime.Hours;
                dto.TAKEMINUTE = takeTime.Minutes;

                if (panVisit.Validate<HC_OSHA_VISIT>())
                {
                    hcOshaVisitService.Save(dto);

                    MessageUtil.Info(LblSiteName.Text + " 방문내역을 저장하였습니다");

                }
            }
           
        }

       
        private void BtnSearchUnVisit_Click(object sender, EventArgs e)
        {
            string visitUserId = CboSearchUnVisitUserId.GetValue();
            string visitStartDate = DtpSearchUnVisitStartDate.GetValue();
            string visitEndDate = DtpSearchUnVisitEndDate.GetValue();
            SSUnVisit.SetDataSource(schduleModelService.scheduleModelRepository.FindUnvisitSiteList(visitUserId, visitStartDate, visitEndDate));
        }


        private void BtnSearchVisit_Click(object sender, EventArgs e)
        {
            string visitUserId = CboSearchVisitUserId.GetValue();
            string visitStartDate = DtpSearchVisitStartDate.GetValue();
            string visitEndDate = DtpSearchVisitEndDate.GetValue();
            SSVisit.SetDataSource(schduleModelService.scheduleModelRepository.FindVisitSiteList(visitUserId, visitStartDate, visitEndDate));
        }

        private void SSUnVisit_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            UnvisitSiteModel model = SSUnVisit.GetRowData(e.Row) as UnvisitSiteModel;
            SetScheduleAndVisitForm(model.SCHEDULE_ID);

        }

        private void SSVisit_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
             VisitSiteModel model = SSVisit.GetRowData(e.Row) as VisitSiteModel;
   
             SetScheduleAndVisitForm(model.SCHEDULE_ID);
        }

        private void SetScheduleAndVisitForm(long schduleId)
        {
            HC_OSHA_SCHEDULE dto = hcOshaScheduleService.FindOne(schduleId);
            panSchedule.SetData(dto);
            LblSiteName.Text = dto.SITE_NAME;

            SetSelectedSite(dto.SITE_ID);
            SetSelectedEstimate(dto.ESTIMATE_ID);

            foreach (CommonForm form in TabForms)
            {
                form.SetSelectedSite(dto.SITE_ID);
                (form as ISelectSite).Select(oshaSiteList.GetSite);

            }
            HC_OSHA_VISIT visitDto = hcOshaVisitService.FindByScheduleId(schduleId);
            if (visitDto == null)
            {
                //미방문탭
                tabControl2.SelectedIndex = 1;
                if(dto.ID != 0)
                {
                    visitDto = new HC_OSHA_VISIT();
                    visitDto.SCHEDULE_ID = dto.ID;
                    visitDto.VISITDATETIME = DateUtil.stringToDateTime(dto.VISITRESERVEDATE, DateTimeType.YYYY_MM_DD);
                    panVisit.SetData(visitDto);
                }
            }
            else
            {
                //방문탭
                tabControl2.SelectedIndex = 2;
                panVisit.SetData(visitDto);
            }

        }

        private void OshaSiteList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            base.SelectedSite = oshaSiteList.GetSite;
  

            foreach (CommonForm form in TabForms)
            {
                form.SelectedSite = oshaSiteList.GetSite;
                if(form is ISelectSite)
                {
                    (form as ISelectSite).Select(oshaSiteList.GetSite);
                }
                
            }

            oshaSiteEstimateList1.Searh(oshaSiteList.GetSite.ID);

            LblSiteName.Text = oshaSiteList.GetSite.NAME;
         

        }
        private void OshaSiteEstimateList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(oshaSiteEstimateList1.GetEstimateModel != null)
            {
                base.SelectedEstimate = oshaSiteEstimateList1.GetEstimateModel;
                foreach (CommonForm form in TabForms)
                {
                    form.SelectedEstimate = oshaSiteEstimateList1.GetEstimateModel;
                    if (form is ISelectEstimate)
                    {
                        (form as ISelectEstimate).Select(oshaSiteEstimateList1.GetEstimateModel);
                    }
                }

                HC_OSHA_SCHEDULE dto = new HC_OSHA_SCHEDULE();
                dto.SITE_ID = oshaSiteList.GetSite.ID;
                dto.VISITRESERVEDATE = this.StartDate;
                dto.ESTIMATE_ID = oshaSiteEstimateList1.GetEstimateModel.ID;
                panSchedule.SetData(dto);
            }
      

        }

        private void BtnSendMail_Click(object sender, EventArgs e)
        {
            ScheduleMailForm form = new ScheduleMailForm();
            form.ShowDialog();
        }
    }
}
