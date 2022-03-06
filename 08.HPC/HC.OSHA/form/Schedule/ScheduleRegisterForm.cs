using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Exceptions;
using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using HC.Core.Common.Extension;
using HC.Core.Common.Interface;
using HC.Core.Common.Util;
using HC.Core.Dto;
using HC.Core.Repository;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC.OSHA.Repository;
using HC.OSHA.Service;
using HC.OSHA.Service.Schedule;
using HC_Core;
using HC_Core.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HC_OSHA
{
    public partial class ScheduleRegisterForm : CommonForm
    {
        private HcUserService hcUsersService;
        private HcOshaScheduleService hcOshaScheduleService;
        private HcOshaVisitService hcOshaVisitService;
        private SchduleModelService schduleModelService;
        private OshaPriceService oshaPriceService;
        private List<CommonForm> TabForms;
        private HcOshaSiteService hcOshaSiteService;
        private OshaVisitPriceService oshaVisitPriceService = new OshaVisitPriceService();
        private HcOshaContractService hcOshaContractService;
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
            oshaPriceService = new OshaPriceService();
            TabForms = new List<CommonForm>();
            hcOshaSiteService = new HcOshaSiteService();
            hcOshaContractService = new HcOshaContractService();
        }

        private void ScheduleRegisterForm_Load(object sender, EventArgs e)
        {
            DateTime dateTime = codeService.CurrentDate;
            dateTime = dateTime.AddMonths(1);
            for (int i = 0; i <= 24; i++)
            {
                CboMonth.Items.Add(dateTime.AddMonths(-i).ToString("yyyy-MM"));
            }
            CboMonth.SelectedIndex = 1;

            TxtSearchUnvisitSiteIdOrName.SetExecuteButton(BtnSearchUnVisit);
            TxtSearchVisitSiteIdOrName.SetExecuteButton(BtnSearchVisit);

            if (DataSyncService.Instance.IsLocalDB == true)
            {
                BtnSaveVisit.Enabled = false;
                BtnVisitDelete.Enabled = false;
                BtnDeleteSchedule.Enabled = false;
                BtnSaveSchedule.Enabled = false;
            }

           // List<HC_OSHA_SITE_MODEL> siteList = hcOshaSiteService.Search("", "");
            List<HC_USER> OSHAUsers = hcUsersService.GetOsha();
            List<HC_USER> doctors = hcUsersService.GetDoctors();

            DtpVISITRESERVEDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_SCHEDULE.VISITRESERVEDATE), DisplayFormat = DateTimeType.YYYY_MM_DD, });
            txtVISITTIME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_SCHEDULE.VISITSTARTTIME) });
            CboVISITUSERID.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_SCHEDULE.VISITUSERID) });
            CboVISITMANAGERID.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_SCHEDULE.VISITMANAGERID) });
            CboVISITUSERID.SetItems(OSHAUsers, "Name", "UserId");
            CboVISITMANAGERID.SetItems(OSHAUsers, "Name", "UserId", "", "없음", AddComboBoxPosition.Top);
            TxtREMARK.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_SCHEDULE.REMARK) });
            TxtVISITPLACE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_SCHEDULE.VISITPLACE) });

            //CboSearchScheduleSite.SetItems(siteList, "Name", "Id", "전체", "", AddComboBoxPosition.Top);
            CboSearchScheduleVisitUserId.SetItems(OSHAUsers, "Name", "UserId", "전체", "", AddComboBoxPosition.Top);
            CboSearchScheduleVisitUserId.SetValue(CommonService.Instance.Session.UserId);
            CboSearchScheduleVisitUserId2.SetItems(OSHAUsers, "Name", "UserId", "전체", "", AddComboBoxPosition.Top);
            TxtMODIFIED.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_SCHEDULE.MODIFIED),  ReadOnly = true });
            TxtMODIFIEDUSER.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_SCHEDULE.MODIFIEDUSER), ReadOnly = true });

            //방문내역폼
            DtpVISITDATETIME.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_VISIT.VISITDATETIME), DataBaseFormat = DateTimeType.None, DisplayFormat = DateTimeType.YYYY_MM_DD, });
            txtStartTime.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_VISIT.STARTTIME) });
            txtEndTime.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_VISIT.ENDTIME) });
            TxtTakeHourAndMinute.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_VISIT.TakeHourAndMinute) });

            CboVISITUSER.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_VISIT.VISITUSER) });
            CboVISITDOCTOR.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_VISIT.VISITDOCTOR) });
            CboVISITTYPE.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_VISIT.VISITTYPE) });

         
            CboVISITUSER.SetItems(OSHAUsers, "Name", "UserId");
            CboVISITDOCTOR.SetItems(doctors, "Name", "UserId", "","", AddComboBoxPosition.Top);
            CboVISITTYPE.SetItems(codeService.FindActiveCodeByGroupCode("VISIT_TYPE", "OSHA"), "codename", "code" );
            TxtVisitREMARK.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_VISIT.REMARK) });
            ChkISFEE.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_VISIT.ISFEE), CheckValue = "Y", UnCheckValue = "N" });
            ChkISKUKGO.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_VISIT.ISKUKGO), CheckValue = "Y", UnCheckValue = "N" });

            //수수료
            NumVisitWORKERCOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(OSHA_VISIT_PRICE.WORKERCOUNT), Min = 0, Max= 99999999999, TextAlign = HorizontalAlignment.Right });
            NumVisitUNITPRICE.SetOptions(new NumericUpDownOption { DataField = nameof(OSHA_VISIT_PRICE.UNITPRICE), Min = 0, Max = 99999999999, TextAlign = HorizontalAlignment.Right });          
            NumVisitUNITTOALPRICE.SetOptions(new NumericUpDownOption { DataField = nameof(OSHA_VISIT_PRICE.UNITTOTALPRICE), Min = 0, Max = 99999999999, TextAlign = HorizontalAlignment.Right });
            NumVisitTOTALPRICE.SetOptions(new NumericUpDownOption { DataField = nameof(OSHA_VISIT_PRICE.TOTALPRICE ), Min = 0, Max = 99999999999, TextAlign = HorizontalAlignment.Right });
            NumChargePrice.SetOptions(new NumericUpDownOption { DataField = nameof(OSHA_VISIT_PRICE.CHARGEPRICE), Min = 0, Max = 99999999999, TextAlign = HorizontalAlignment.Right });

            //SSVisitList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            // SSVisitList.AddColumnDateTime("방문일", nameof(HC_OSHA_VISIT.VISITDATETIME), 120, IsReadOnly.Y, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = true, IsShowCalendarButton = false });
            // SSVisitList.AddColumnText("방문자", nameof(HC_OSHA_VISIT.VISITUSERNAME), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });


            panVisit.SetData(new HC_OSHA_VISIT());


            //수수료 내역
            SSVisitPriceList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSVisitPriceList.AddColumnDateTime("방문일", nameof(OshaVisitPriceModel.VISITDATETIME), 120, IsReadOnly.Y, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = true, IsShowCalendarButton = false });
            SSVisitPriceList.AddColumnText("방문자", nameof(OshaVisitPriceModel.VISITUSERNAME), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSVisitPriceList.AddColumnText("인원", nameof(OshaVisitPriceModel.WORKERCOUNT), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSVisitPriceList.AddColumnNumber("계약단가", nameof(OshaVisitPriceModel.UNITPRICE), 80, new SpreadCellTypeOption { IsSort = false });


            SSVisitPriceList.AddColumnNumber("계산금액", nameof(OshaVisitPriceModel.UNITTOTALPRICE), 80, new SpreadCellTypeOption { IsSort = false });

            SSVisitPriceList.AddColumnNumber("발생금액", nameof(OshaVisitPriceModel.TOTALPRICE), 80, new SpreadCellTypeOption { IsSort = false });

            SSVisitPriceList.AddColumnText("선청구여부", nameof(OshaVisitPriceModel.ISPRECHARGE), 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSVisitPriceList.AddColumnText("발생일", nameof(OshaVisitPriceModel.CREATED), 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSVisitPriceList.AddColumnText("삭제여부", nameof(OshaVisitPriceModel.ISDELETED), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });


            //단가변경 이력
            //SSPrice.Initialize(new SpreadOption() { IsRowSelectColor = false });
            //SSPrice.AddColumnText("ID", nameof(OSHA_PRICE.ID), 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            //SSPrice.AddColumnText("인원", nameof(OSHA_PRICE.WORKERTOTALCOUNT), 70, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            //SSPrice.AddColumnText("단가", nameof(OSHA_PRICE.UNITPRICE), 90, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSPrice.AddColumnText("발생(계산)금액", nameof(OSHA_PRICE.UNITTOTALPRICE), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSPrice.AddColumnText("청구(계약)금액", nameof(OSHA_PRICE.TOTALPRICE), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSPrice.AddColumnText("수정일시", nameof(OSHA_PRICE.MODIFIED), 163, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSPrice.AddColumnText("사용자", nameof(OSHA_PRICE.MODIFIEDUSER), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });


            //검색 초기화
            InitializeSearchForm(OSHAUsers);

            EducationForm form = new EducationForm();
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

            InOutEmployeeForm inOutEmployeeForm = new InOutEmployeeForm();
            inOutEmployeeForm.SelectedSite = base.SelectedSite;
            AddForm(inOutEmployeeForm, TabInOut);
            TabForms.Add(inOutEmployeeForm);

            ReceiptForm receiptForm = new ReceiptForm();
            receiptForm.SelectedSite = base.SelectedSite;
            AddForm(receiptForm, TabReceipt);
            TabForms.Add(receiptForm);

            InitalizeForm();

            //방문예정일 목록
            DateTime currentDate = codeService.CurrentDate;

            SSScheduleList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSScheduleList.AddColumnText("방문예정일", "", 106, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, mergePolicy = FarPoint.Win.Spread.Model.MergePolicy.Always });
            SSScheduleList.AddColumnText("방문시간", nameof(HC_OSHA_SCHEDULE.VISITSTARTTIME), 77, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSScheduleList.AddColumnText("회사명", nameof(HC_OSHA_SCHEDULE.SITE_NAME), 161, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SSScheduleList.AddColumnText("방문자", nameof(HC_OSHA_SCHEDULE.VISITUSERNAME), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSScheduleList.AddColumnText("동행자", nameof(HC_OSHA_SCHEDULE.VISITMANAGERNAME), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSScheduleList.AddColumnText("비고사항", nameof(HC_OSHA_SCHEDULE.REMARK), 150, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SearchScheduleList();
        }

        /// <summary>
        /// 달력 이벤트 폼 초기화
        /// </summary>
        public void InitalizeForm()
        {
            if (EventId.IsNullOrEmpty())
            {
                if (StartDate.IsNullOrEmpty())
                {
                    StartDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                HC_OSHA_SCHEDULE dto = new HC_OSHA_SCHEDULE();
                dto.EVENTSTARTDATETIME = DateUtil.stringToDateTime(StartDate.Substring(0, 10), DateTimeType.YYYY_MM_DD);
                dto.VISITRESERVEDATE =dto.EVENTSTARTDATETIME;//StartDate.Substring(0, 10);
                dto.VISITUSERID = CommonService.Instance.Session.UserId;
                panSchedule.SetData(dto);
            }
            else
            {
                try
                {

                    SetScheduleAndVisitForm(long.Parse(EventId));
                }catch(Exception ex)
                {
                    Log.Error(ex);
                }
                
            }
        }
        private void ClearVisitForm()
        {
            panVisit.SetData(new HC_OSHA_VISIT());
            SearchVisitList();
        }
        private void InitializeSearchForm(List<HC_USER> OSHAUsers)
        {
            CboSearchUnVisitUserId.SetItems(OSHAUsers, "Name", "UserId", "전체", "", AddComboBoxPosition.Top);
            CboSearchUnVisitUserId.SetValue(CommonService.Instance.Session.UserId);

            //if(StartDate == null)
            //{
            //          DtpSearchUnVisitStartDate.SetValue(this.StartDate);

            //}
            //else
            //{
            //    DtpSearchUnVisitStartDate.SetValue(this.StartDate);

            //}

            CboSearchVisitUserId.SetItems(OSHAUsers, "Name", "UserId", "전체", "", AddComboBoxPosition.Top);
            CboSearchVisitUserId.SetValue(CommonService.Instance.Session.UserId);


            CboSearchPreVisitUserId.SetItems(OSHAUsers, "Name", "UserId", "전체", "", AddComboBoxPosition.Top);
            CboSearchPreVisitUserId.SetValue(CommonService.Instance.Session.UserId);
            

            DateTime startDate = DateTime.Now;
            if (StartDate.NotEmpty())
            {
                startDate = DateUtil.stringToDateTime(this.StartDate, DateTimeType.YYYY_MM_DD);
            }

            DtpSearchUnVisitStartDate.SetValue(startDate.GetFirstDate());
            DtpSearchUnVisitEndDate.SetValue(startDate.GetLastDate());
            //DtpSearchUnVisitStartDate.SetValue(startDate);
            //DtpSearchUnVisitEndDate.SetValue(startDate);

            DtpSearchVisitStartDate.SetValue(startDate.GetFirstDate());
            DtpSearchVisitEndDate.SetValue(startDate.GetLastDate());
          
            DtpSearchPreVisitStartDate.SetValue(startDate.GetFirstDate());
            DtpSearchPreVisitEndDate.SetValue(startDate.GetLastDate());

            SSUnVisit.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSUnVisit.AddColumnText("코드", nameof(UnvisitSiteModel.SITE_ID), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSUnVisit.AddColumnText("사업장명", nameof(UnvisitSiteModel.NAME), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SSUnVisit.AddColumnText("예정일", nameof(UnvisitSiteModel.VISITRESERVEDATE), 76, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSUnVisit.AddColumnText("방문자", nameof(UnvisitSiteModel.VISITUSERNAME), 76, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSUnVisit.AddColumnText("선청구", nameof(UnvisitSiteModel.ISPRECHARGE), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            BtnSearchUnVisit.PerformClick();

            SSVisit.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSVisit.AddColumnText("코드", nameof(VisitSiteModel.SITE_ID), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSVisit.AddColumnText("사업장명", nameof(VisitSiteModel.NAME), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SSVisit.AddColumnText("수수료", nameof(VisitSiteModel.ISFEE), 62, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSVisit.AddColumnText("방문일", nameof(VisitSiteModel.VISITDATETIME), 76, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSVisit.AddColumnText("방문자", nameof(VisitSiteModel.VISITUSERNAME), 76, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            BtnSearchVisit.PerformClick();

            SSPreCharge.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSPreCharge.AddColumnText("코드", nameof(VisitSiteModel.SITE_ID), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSPreCharge.AddColumnText("사업장명", nameof(VisitSiteModel.NAME), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SSPreCharge.AddColumnText("방문일", nameof(VisitSiteModel.VISITDATETIME), 76, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSPreCharge.AddColumnText("방문자", nameof(VisitSiteModel.VISITUSERNAME), 76, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            BtnSearchPreVisit.PerformClick();


        }
        private void BtnSaveSchedule_Click(object sender, EventArgs e)
        {
            if(base.SelectedSite == null || base.SelectedSite.ID <=0)
            {
                MessageUtil.Info("사업장을 선택하세요");
            }
            else if (base.SelectedEstimate == null || base.SelectedEstimate.ID <= 0)
            {
                MessageUtil.Info("계약사항이 없는 사업장입니다");
            }
            else
            {
                try
                {
                    HC_OSHA_SCHEDULE dto = panSchedule.GetData<HC_OSHA_SCHEDULE>();
                    HC_USER user = CboVISITUSERID.GetSelectedItem() as HC_USER;
                    dto.VISITUSERID = user.UserId;
                    dto.VISITUSERNAME = user.Name;

                    user = CboVISITMANAGERID.GetSelectedItem() as HC_USER;
                    if (user != null)
                    {
                        dto.VISITMANAGERID = user.UserId;
                        dto.VISITMANAGERNAME = user.Name;

                    }
                    HC_OSHA_VISIT visit = hcOshaVisitService.FindByScheduleId(dto.ID);
                    if (visit != null)
                    {
                        OSHA_VISIT_PRICE visitPriceDto = oshaVisitPriceService.oshaVisitPriceRepository.FindMaxId(visit.ID);
                        if (visitPriceDto != null)
                        {
                            if(visitPriceDto.ISPRECHARGE == "N")
                            {
                                MessageUtil.Info("수수료가 발생된 일정입니다 수정할 수 없습니다");
                                return;
                            }
                        }
                    }

                    HC_OSHA_SCHEDULE saved = hcOshaScheduleService.Save(dto);

                    panSchedule.SetData(saved);
                    BtnSearchUnVisit.PerformClick();
                    SearchScheduleList();
                    MessageUtil.Info(saved.SITE_NAME + " 일정이 저장되었습니다");
                }
                catch(Exception ex)
                {
                    Log.Error(ex);
                }
             
            }
        

        }

        private void BtnDeleteSchedule_Click(object sender, EventArgs e)
        {
            HC_OSHA_SCHEDULE dto = panSchedule.GetData<HC_OSHA_SCHEDULE>();
            if (dto.ID > 0)
            {
                HC_OSHA_VISIT saved = hcOshaVisitService.FindByScheduleId(dto.ID);
                if (saved != null)
                {
                    MessageUtil.Info("방문 등록 된 일정은 삭제할 수 없습니다");
                    return;
                }

                hcOshaScheduleService.Delete(dto);
                MessageUtil.Info("일정이 삭제 되었습니다");

                EventId = string.Empty;
                InitalizeForm();

                BtnSearchUnVisit.PerformClick();
                SearchScheduleList();
            }
        }

        private void BtnNewSchedule_Click(object sender, EventArgs e)
        {
            InitalizeForm();
        }



        /// <summary>
        /// Created가 다르면 기존 Raw 데이타는 두고 새로 금액을 마이너스로 발생시킨다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnVisitDelete_Click(object sender, EventArgs e)
        {
          
            HC_OSHA_VISIT dto = panVisit.GetData<HC_OSHA_VISIT>();
            if (dto.ID > 0)
            {
                if(MessageUtil.Confirm("방문등록을 삭제하시겠습니까?") == DialogResult.Yes)
                {
                    OSHA_VISIT_PRICE priceDto = PanPrice.GetData<OSHA_VISIT_PRICE>();

                    int result = hcOshaVisitService.Delete(dto, priceDto);
                    if(result == 0)
                    {
                        MessageUtil.Info("방문등록을 삭제하였습니다");
                        
                    }
                    else if (result == 1)
                    {
                        MessageUtil.Alert("방문등록을 삭제하고 마이너스 수수료를 발생하였습니다. ");
                    }
                    else 
                    {
                        MessageUtil.Alert(" 방문등록 삭제중 오류가 발생하였습니다. ");
                    }
             
                    ClearVisitForm();

                    BtnSearchUnVisit.PerformClick();
                    BtnSearchVisit.PerformClick();
         
                }
                
            }
        }

        /// <summary>
        /// 방문내역 등록
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveVisit_Click(object sender, EventArgs e)
        {
            DateTime DtpStartTime;
            DateTime DtpEndTime;

            HC_OSHA_SCHEDULE schedule = panSchedule.GetData<HC_OSHA_SCHEDULE>();
            if(schedule.ID == 0)
            {
                MessageUtil.Alert("일정을 먼저 등록하세요");
            }
            else
            {
                HC_OSHA_VISIT dto = panVisit.GetData<HC_OSHA_VISIT>();
                if (VB.Len(dto.STARTTIME) ==5 && VB.Len(dto.ENDTIME) == 5)
                {
                    DtpStartTime = Convert.ToDateTime(DtpVISITDATETIME.Value.ToString("yyyy-MM-dd") + " " + dto.STARTTIME);
                    DtpEndTime = Convert.ToDateTime(DtpVISITDATETIME.Value.ToString("yyyy-MM-dd") + " " + dto.ENDTIME);
                    TimeSpan takeTime = DtpEndTime - DtpStartTime;
                    dto.TAKEHOUR = takeTime.Hours;
                    dto.TAKEMINUTE = takeTime.Minutes;
                }
                dto.ESTIMATE_ID = SelectedEstimate.ID;
                dto.SITE_ID = SelectedSite.ID;
                dto.SCHEDULE_ID = schedule.ID;

                if (panVisit.Validate<HC_OSHA_VISIT>())
                {

                    HC_USER user = CboVISITUSER.GetSelectedItem() as HC_USER;
                    dto.VISITUSER = user.UserId;
                    dto.VISITUSERNAME = user.Name;
                    dto.ISPRECHARGE = "N";
                    HC_USER doctor = CboVISITDOCTOR.GetSelectedItem() as HC_USER;
                    if( doctor != null)
                    {
                        dto.VISITDOCTORNAME = doctor.Name;

                    }

                    OSHA_VISIT_PRICE priceDto = PanPrice.GetData<OSHA_VISIT_PRICE>();

                    try
                    {
                      

                        if (ChkISFEE.Checked)
                        {
                            dto.ISFEE = "Y";
                            if (ChkISKUKGO.Checked)
                            {
                                dto.ISKUKGO = "Y";
                            }

                            if (MessageUtil.Confirm("수수료 발생이 선택되어 있습니다. 계속 하시겠습니까?") == DialogResult.No)
                            {
                                priceDto = null;
                            }
                            else
                            {
                                OSHA_VISIT_PRICE visitPriceDto = oshaVisitPriceService.oshaVisitPriceRepository.FindMaxId(dto.ID);

                                if (visitPriceDto != null && visitPriceDto.ID>0)
                                {                                                                    
                                        priceDto.ID = visitPriceDto.ID;
                                        priceDto.VISIT_ID = visitPriceDto.VISIT_ID;
                                        priceDto.CHARGEPRICE = visitPriceDto.CHARGEPRICE;
                                        priceDto.ISPRECHARGE = visitPriceDto.ISPRECHARGE;
                                }
                                else
                                {
                                    //동일사업장의 수수료는 한번만 발생할 수 있음
                                  
                                        //string startDate = DateUtil.DateTimeToStrig(Convert.ToDateTime(dto.VISITDATETIME).GetFirstDate(), ComBase.Controls.DateTimeType.YYYY_MM_DD);
                                        //string endDate = DateUtil.DateTimeToStrig(Convert.ToDateTime(dto.VISITDATETIME).GetLastDate(), ComBase.Controls.DateTimeType.YYYY_MM_DD);

                                        //OSHA_VISIT_PRICE savedPriceDto = oshaVisitPriceService.oshaVisitPriceRepository.FindVisitPrice(dto.SITE_ID, StartDate, endDate);
                                        //if(savedPriceDto != null)
                                        //{
                                        //    MessageUtil.Alert("수수료를 발생할 수 없습니다 이미 수수료가 발생 되었습니다.");
                                        //    ChkISFEE.Checked = false;
                                        //    return;
                                        //}
                                        
                                    
                                }

                            }
                            
                        }

                        clsDB.setBeginTran(clsDB.DbCon);

                        HC_OSHA_VISIT saved = hcOshaVisitService.Save(dto, priceDto);

                        if (ChkISFEE.Checked && priceDto != null)
                        {
                            PanPrice.Validate<OSHA_VISIT_PRICE>();

                            OSHA_PRICE lastPrice = oshaPriceService.OshaPriceRepository.FindMaxIdByEstimate(SelectedEstimate.ID);
                            if (lastPrice != null)
                            {
                                long totalPrice = lastPrice.TOTALPRICE;
                                List<OSHA_PRICE> childPrice = oshaPriceService.OshaPriceRepository.FindAllByParent(SelectedSite.ID);
                                bool hasChild = false;
                                foreach (OSHA_PRICE child in childPrice)
                                {
                                    hasChild = true;
                                    totalPrice += child.TOTALPRICE;
                                }
                                if (!hasChild)
                                {
                                    //하청업체가있을 경우 계약금액 갱신 안함.
                                    OSHA_PRICE unitPriceDto = PanPrice.GetData<OSHA_PRICE>();
                                    unitPriceDto.WORKERTOTALCOUNT = priceDto.WORKERCOUNT;
                                    unitPriceDto.UNITPRICE = priceDto.UNITPRICE;
                                    unitPriceDto.UNITTOTALPRICE = priceDto.UNITTOTALPRICE;
                                    unitPriceDto.TOTALPRICE = priceDto.TOTALPRICE;
                                    unitPriceDto.ESTIMATE_ID = SelectedEstimate.ID;

                                    unitPriceDto.ISFIX = lastPrice.ISFIX;
                                    unitPriceDto.ISBILL = lastPrice.ISBILL;
                                    oshaPriceService.Save(unitPriceDto);
                                }
                            }
                        }
                  

                        clsDB.setCommitTran(clsDB.DbCon);

                        BtnSearchUnVisit.PerformClick();
                        BtnSearchVisit.PerformClick();

                        MessageUtil.Info("방문등록을 저장하였습니다");
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        throw new MTSException("방문 등록 중 오류가 발생하였습니다");
                    }


                    SearchVisitList();
                    SearchVisitPriceList();
                    BtnSearchPreVisit.PerformClick();

                    //MessageUtil.Info(LblSiteName.Text + " 방문내역을 저장하였습니다");


                }
            }
           
        }

       
        private void BtnSearchUnVisit_Click(object sender, EventArgs e)
        {
            string visitUserId = CboSearchUnVisitUserId.GetValue();
            string visitStartDate = DtpSearchUnVisitStartDate.Value.ToString("yyyy-MM-dd");
            string visitEndDate = DtpSearchUnVisitEndDate.GetValue();
            string siteIdOrName = TxtSearchUnvisitSiteIdOrName.Text;
            List<UnvisitSiteModel>  list = schduleModelService.scheduleModelRepository.FindUnvisitSiteList(visitUserId, visitStartDate, visitEndDate, siteIdOrName);
            SSUnVisit.SetDataSource(list);

            LblUnvisitCount.Text = list.Count + " 건";
        }


        private void BtnSearchVisit_Click(object sender, EventArgs e)
        {
            string visitUserId = CboSearchVisitUserId.GetValue();
            string visitStartDate = DtpSearchVisitStartDate.GetValue();
            string visitEndDate = DtpSearchVisitEndDate.GetValue();
            string siteIdOrName = TxtSearchVisitSiteIdOrName.Text;
            List<VisitSiteModel> list = schduleModelService.scheduleModelRepository.FindVisitSiteList(visitUserId, visitStartDate, visitEndDate, siteIdOrName);

            SSVisit.SetDataSource(list);
            LblVisitCount.Text = list.Count + " 건";

        }

        private void SSUnVisit_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            UnvisitSiteModel model = SSUnVisit.GetRowData(e.Row) as UnvisitSiteModel;
            SetScheduleAndVisitForm(model.SCHEDULE_ID, model.VISIT_ID);
        

        }

        private void SSVisit_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
             VisitSiteModel model = SSVisit.GetRowData(e.Row) as VisitSiteModel;
   
             SetScheduleAndVisitForm(model.SCHEDULE_ID, model.VISIT_ID);
         
        }
        
        /// <summary>
        /// 1. 달력에서 스케쥴 선택했을경우
        /// 2. 미방문 스프레드에서 선택했을경우
        /// </summary>
        /// <param name="schduleId"></param>
        private void SetScheduleAndVisitForm(long schduleId, long visitId = 0)
        {
       //     tabControl1.SelectedIndex = 1;

            HC_OSHA_SCHEDULE dto = hcOshaScheduleService.FindOne(schduleId);
            panSchedule.SetData(dto);
            LblSiteName.Text = dto.SITE_NAME;

            SetSelectedSite(dto.SITE_ID);
            SetSelectedEstimate(dto.ESTIMATE_ID);

      //      OSHA_PRICE lastPrice = oshaPriceService.OshaPriceRepository.FindMaxIdByEstimate(SelectedEstimate.ID);

            OshaSiteLastTree.Search(dto.SITE_NAME);
            oshaSiteEstimateList1.Searh(dto.SITE_ID, false);

            foreach (CommonForm form in TabForms)
            {
                form.SetSelectedSite(dto.SITE_ID);
                (form as ISelectSite).Select(OshaSiteLastTree.GetSite);
                ISelectEstimate selectEstimate = (form as ISelectEstimate);
                if (selectEstimate != null)
                {
                    selectEstimate.Select(base.SelectedEstimate);
                }
                

            }
            //방문내역
            //HC_OSHA_VISIT visitDto = hcOshaVisitService.FindByScheduleId(schduleId);
            HC_OSHA_VISIT visitDto = hcOshaVisitService.FindById(visitId);
            if (visitDto == null)
            {
                CboSearchUnVisitUserId.SetValue(dto.VISITUSERID);

                if (dto.ID != 0)
                {
                    visitDto = new HC_OSHA_VISIT();
                    visitDto.SCHEDULE_ID = dto.ID;
                    visitDto.VISITDATETIME = dto.VISITRESERVEDATE;
                    
                    panVisit.SetData(visitDto);

                    //초기화...
                    PanPrice.Initialize();
                    ClearPreice();
                    SetPriceEnable(false);
                    //OSHA_PRICE price = oshaPriceService.OshaPriceRepository.FindMaxIdByEstimate(SelectedEstimate.ID);
                    //if (price != null)
                    //{
                    //    SetPrice(price.WORKERTOTALCOUNT, price.UNITPRICE, price.UNITTOTALPRICE, price.TOTALPRICE);

                    //}


                }
                //미방문탭
                tabControl2.SelectedIndex = 1;
            }
            else
            {
                panVisit.SetData(visitDto);
                if(visitDto.ISFEE == "Y")
                {
                    ChkISFEE.Checked = true;
                    if(visitDto.ISPRECHARGE == "Y")
                    {
                        ChkIsPreCharge.Checked = true;
                    }
                    else
                    {
                        ChkIsPreCharge.Checked = false;
                    }
                            
                    //금액을 가져온다
                    //OshaVisitPriceService ovp = new zOshaVisitPriceService();
                    OSHA_VISIT_PRICE visitPriceDto = oshaVisitPriceService.oshaVisitPriceRepository.FindMaxId(visitDto.ID);
                  ///  OSHA_VISIT_PRICE visitPriceDto = oshaVisitPriceService.oshaVisitPriceRepository.FindMaxId(visitDto.ID);//
                    if (visitPriceDto != null)
                    {                        
                        SetPrice(visitPriceDto.WORKERCOUNT, visitPriceDto.UNITPRICE, visitPriceDto.UNITTOTALPRICE, visitPriceDto.TOTALPRICE, visitPriceDto.CHARGEPRICE);
                    }
                    else
                    {
                        //초기화...
                        PanPrice.Initialize();
                        ClearPreice();
                        SetPriceEnable(false);
                    }
                }
                else
                {

                    //초기화...
                    PanPrice.Initialize();
                    ClearPreice();
                    SetPriceEnable(false);
                }
                //방문탭
                //if (tabControl2.SelectedIndex != 3)
                //{
                //    tabControl2.SelectedIndex = 2;
                //}
                //tabControl2.SelectedIndex = 2;
                tabControl1.SelectedIndex = 1;

            }
            SearchVisitList();
            SearchVisitPriceList();
        }
        private void SetPrice(long workerCount, double UnitPrice, long UnitTotalPrice, long totalPrice, long chargePrice)
        {
            ClearPreice();
            SetPriceEnable(true);
            NumVisitWORKERCOUNT.SetValue(workerCount);
            NumVisitUNITPRICE.SetValue(UnitPrice);
            NumVisitUNITTOALPRICE.SetValue(UnitTotalPrice);
            NumVisitTOTALPRICE.SetValue(totalPrice);
            NumChargePrice.SetValue(chargePrice);

            if (totalPrice > 0)
            {
                OSHA_PRICE dto = oshaPriceService.OshaPriceRepository.FindMaxIdByEstimate(SelectedEstimate.ID); // 스케쥴 테이블의 estimateId가 잘못 들어가 있음 

                if (dto != null)
                {
                    if (dto.ISFIX == "Y")
                    {
                        ChkIsFix.Checked = true;
                    }
                    else
                    {
                        ChkIsFix.Checked = false;
                    }

                }
            }
        }
        private void SetPriceEnable(bool enable)
        {
            ChkISFEE.Checked = enable;

            NumVisitUNITPRICE.Enabled = enable;
            NumVisitUNITTOALPRICE.Enabled = enable;
            NumVisitTOTALPRICE.Enabled = enable;
            NumVisitWORKERCOUNT.Enabled = enable;
        }

        private void OshaSiteLastTree_NodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Clear();

            base.SelectedSite = OshaSiteLastTree.GetSite;
            base.SelectedEstimate = null;

            foreach (CommonForm form in TabForms)
            {
                form.SelectedSite = OshaSiteLastTree.GetSite;
                if (form is ISelectSite)
                {
                    (form as ISelectSite).Select(OshaSiteLastTree.GetSite);
                }

            }

            oshaSiteEstimateList1.SearhAndDoubleClik(OshaSiteLastTree.GetSite.ID, false);

            LblSiteName.Text = OshaSiteLastTree.GetSite.NAME;

            SearchVisitPriceList();

            SearchScheduleList();
        }

        private void Clear()
        {
            HC_OSHA_SCHEDULE dto = new HC_OSHA_SCHEDULE();
            dto.SITE_ID = OshaSiteLastTree.GetSite.ID;
            dto.VISITRESERVEDATE = DateUtil.stringToDateTime(this.StartDate.Substring(0, 10), DateTimeType.YYYY_MM_DD); 
            dto.VISITUSERID = CommonService.Instance.Session.UserId;
            panSchedule.SetData(dto);

            panVisit.SetData(new HC_OSHA_VISIT());
            PanPrice.Initialize();

            ClearPreice();

        }

        /// <summary>
        /// 사업장 계약 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OshaSiteEstimateList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(oshaSiteEstimateList1.GetEstimateModel != null)
            {
                base.SelectedEstimate = oshaSiteEstimateList1.GetEstimateModel;

                SearchVisitList();

                SearchScheduleList();

                foreach (CommonForm form in TabForms)
                {
                    form.SelectedEstimate = oshaSiteEstimateList1.GetEstimateModel;
                    if (form is ISelectEstimate)
                    {
                        (form as ISelectEstimate).Select(oshaSiteEstimateList1.GetEstimateModel);
                    }
                }

                HC_OSHA_SCHEDULE dto = new HC_OSHA_SCHEDULE();
                dto.SITE_ID = OshaSiteLastTree.GetSite.ID;
                dto.VISITRESERVEDATE = DateUtil.stringToDateTime(this.StartDate.Substring(0, 10), DateTimeType.YYYY_MM_DD); //this.StartDate;
                dto.VISITUSERID = CommonService.Instance.Session.UserId;
                dto.ESTIMATE_ID = oshaSiteEstimateList1.GetEstimateModel.ID;

                //인원수
                //OSHA_PRICE price = oshaPriceService.OshaPriceRepository.FindMaxIdByEstimate(SelectedEstimate.ID);
                //if (price != null)
                //{
                //    dto.WORKERCOUNT = price.WORKERTOTALCOUNT;

                //}
                HC_OSHA_CONTRACT contract = hcOshaContractService.FindByEstimateId(SelectedEstimate.ID);
                dto.WORKERCOUNT = contract.WORKERTOTALCOUNT;
                panSchedule.SetData(dto);
            }
      

        }

        /// <summary>
        /// 방문내역(견적별)
        /// </summary>
        private void SearchVisitList()
        {
          //  List<HC_OSHA_VISIT> list = hcOshaVisitService.hcOshaVisitRepository.FindAllByEstimate(base.SelectedEstimate.ID);
          //  SSVisitList.SetDataSource(list);
        }

        /// <summary>
        /// 방문내역별 수수료적용목록
        /// </summary>
        private void SearchVisitPriceList()
        {
            if(base.SelectedEstimate != null)
            {
                List<OshaVisitPriceModel> list = oshaVisitPriceService.oshaVisitPriceRepository.FindAllByEstimate(base.SelectedEstimate.ID);
                SSVisitPriceList.SetDataSource(list);

            }

            //SearchPriceList();
        }
        private void SearchPriceList()
        {
            //List<OSHA_PRICE> list = oshaPriceService.OshaPriceRepository.FindAllByEstimateId(SelectedEstimate.ID);
            //SSPrice.SetDataSource(list);
        }
        private void ChkISFEE_Click(object sender, EventArgs e)
        {
            if (SelectedSite == null)
            {
                MessageUtil.Alert("사업장을 선택하세요 ");
            }
            else if (SelectedEstimate == null)
            {
                MessageUtil.Alert("견적 및 계약 정보가 없습니다");
            }
            else
            {
                if (ChkISFEE.Checked)
                {
                    SetPriceEnable(true);

                    HcOshaEstimateRepository hcOshaEstimateRepository = new HcOshaEstimateRepository();
                    long estimateId = hcOshaEstimateRepository.GetEstimateId(SelectedSite.ID);
                    OSHA_PRICE dto = oshaPriceService.OshaPriceRepository.FindMaxIdByEstimate(estimateId); // 스케쥴 테이블의 estimateId가 잘못 들어가 있음 

                    //OSHA_PRICE dto = oshaPriceService.OshaPriceRepository.FindMaxIdByEstimate(SelectedEstimate.ID);
                    if (dto != null)
                    {
                        if (dto.ISFIX == "Y")
                        {
                            ChkIsFix.Checked = true;
                        }
                        else
                        {
                            ChkIsFix.Checked = false;
                        }

                        HC_OSHA_VISIT visitDto = panVisit.GetData<HC_OSHA_VISIT>();
                        if (visitDto.ISPRECHARGE == "N" || visitDto.ISPRECHARGE == null)
                        {
                            string startDate = DateUtil.DateTimeToStrig(DtpVISITDATETIME.Value.GetFirstDate(), ComBase.Controls.DateTimeType.YYYY_MM_DD);
                            string endDate = DateUtil.DateTimeToStrig(DtpVISITDATETIME.Value.GetLastDate(), ComBase.Controls.DateTimeType.YYYY_MM_DD);

                            List<OSHA_VISIT_PRICE> savedPriceDtoList = oshaVisitPriceService.oshaVisitPriceRepository.FindVisitPrice(SelectedSite.ID, startDate, endDate);
                            if (savedPriceDtoList != null)
                            {
                                long totalPrice = 0;
                                if (savedPriceDtoList.Count > 0)
                                {
                                    foreach(OSHA_VISIT_PRICE price in savedPriceDtoList)
                                    {
                                        totalPrice += price.TOTALPRICE;
                                    }
                                    if (totalPrice > 0)
                                    {
                                        MessageUtil.Alert("해당 사업장은 수수료를 발생할 수 없습니다 이미 수수료가 발생 되었습니다.");

                                        PanPrice.Initialize();
                                        ClearPreice();
                                        SetPriceEnable(false);
                                        return;
                                    }
                                }

                            }
                          

                            List<OSHA_PRICE> childPrice = oshaPriceService.OshaPriceRepository.FindAllByParent(SelectedSite.ID);
                            foreach (OSHA_PRICE child in childPrice)
                            {
                                dto.WORKERTOTALCOUNT += child.WORKERTOTALCOUNT;
                                //dto.UNITPRICE += child.UNITPRICE;
                                //dto.UNITTOTALPRICE += child.UNITTOTALPRICE;
                                dto.TOTALPRICE += child.TOTALPRICE;
                            }

                            // 하청이 있을경우 인원만 합하고 단가는 원청의 단가를 사용
                            SetPrice(dto.WORKERTOTALCOUNT, dto.UNITPRICE, dto.UNITTOTALPRICE, dto.TOTALPRICE, 0);

                            
                        }
                    }
                }
                else
                {

                    HC_OSHA_VISIT visitDto = panVisit.GetData<HC_OSHA_VISIT>();

                    if (visitDto.ISPRECHARGE == "Y")
                    {
                        MessageUtil.Alert("선청구는 수수료를 발생하여야 합니다.");
                        ChkISFEE.Checked = true;
                    }

                    else
                    {
                        PanPrice.Initialize();
                        ClearPreice();
                        SetPriceEnable(false);
                    }

                }
            }

        }
        private void ChkISFEE_CheckedChanged(object sender, EventArgs e)
        {
           
            
        }

        private void ClearPreice()
        {
            NumVisitUNITPRICE.Minimum = -9999999999;
            NumVisitUNITTOALPRICE.Minimum = -9999999999;
            NumVisitTOTALPRICE.Minimum = -9999999999;
            NumVisitWORKERCOUNT.Minimum = -9999999999;
            NumChargePrice.Minimum = -9999999999;

            NumVisitUNITPRICE.Value = 0;
            NumVisitUNITTOALPRICE.Value = 0;
            NumVisitTOTALPRICE.Value = 0;
            NumVisitWORKERCOUNT.Value = 0;
            NumChargePrice.Minimum = 0;
        }

        private void TabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl2.SelectedIndex == 2)
            {
                BtnSearchVisit.PerformClick();
            }
            else if (tabControl2.SelectedIndex == 1)
            {
                BtnSearchUnVisit.PerformClick(); 
            }
            
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            InitalizeForm();
        }

        private void DtpENDTIME_ValueChanged(object sender, EventArgs e)
        {
            ChaneTime();
        }
        private void DtpSTARTTIME_ValueChanged(object sender, EventArgs e)
        {
            ChaneTime();
        }

        private void ChaneTime()
        {
            DateTime DtpStartTime;
            DateTime DtpEndTime;

            if (VB.Len(txtStartTime.Text.Trim()) != 5) return;
            if (VB.Len(txtEndTime.Text.Trim()) != 5) return;

            DtpStartTime = Convert.ToDateTime(DtpVISITDATETIME.Value.ToString("yyyy-MM-dd") + " " + txtStartTime.Text);
            DtpEndTime = Convert.ToDateTime(DtpVISITDATETIME.Value.ToString("yyyy-MM-dd") + " " + txtEndTime.Text);
            TimeSpan takeTime = DtpEndTime - DtpStartTime;
            TxtTakeHourAndMinute.SetValue(Math.Round(takeTime.TotalHours));

            TxtTakeHourAndMinuteText.Text = Math.Round(takeTime.TotalHours) + "시간 ";
            if (takeTime.Minutes > 0)
            {
                TxtTakeHourAndMinuteText.Text += takeTime.Minutes + "분";
            }
        }

        private void NumVisitWORKERCOUNT_ValueChanged(object sender, EventArgs e)
        {
            SetTotalPrice();

        }
        private void NumVisitWORKERCOUNT_KeyUp(object sender, KeyEventArgs e)
        {
            SetTotalPrice();
        }
        private void SetTotalPrice()
        {
            try
            {
                double totalPrice = oshaPriceService.GetTotalPrice(NumVisitWORKERCOUNT.GetValue(), NumVisitUNITPRICE.GetValue());
                // NumVisitUNITTOALPRICE.SetValue(totalPrice);
                NumVisitUNITTOALPRICE.SetValue(totalPrice);

                if (ChkIsFix.Checked)
                {
                    //정액은 금액 변동이 되지 않음
                }
                else
                {
                    NumVisitTOTALPRICE.SetValue(totalPrice);
                }
                
            }
            catch(Exception ex)
            {
                Log.Error(ex);
            }
            
        }


        private void NumVisitUNITPRICE_KeyUp(object sender, KeyEventArgs e)
        {
            SetTotalPrice();
        }
        private void NumVisitUNITPRICE_ValueChanged(object sender, EventArgs e)
        {
            SetTotalPrice();
        }
        private void NumVisitUNITTOALPRICE_KeyUp(object sender, KeyEventArgs e)
        {
            // double unitPrice = oshaPriceService.GetUnitPrice(NumVisitWORKERCOUNT.GetValue(), NumVisitUNITTOALPRICE.GetValue());
            // NumVisitUNITPRICE.SetValue(unitPrice);
            // NumVisitTOTALPRICE.SetValue(NumVisitUNITTOALPRICE.GetValue());
        }

        private void BtnGetVisit_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null)
            {
                MessageUtil.Info("사업장을 선택하세요");
            }
            else if (base.SelectedEstimate == null)
            {
                MessageUtil.Info("계약사항이 없는 사업장입니다");
            }
            else
            {
                HC_OSHA_SCHEDULE dto = panSchedule.GetData<HC_OSHA_SCHEDULE>();

                txtStartTime.Text=dto.VISITSTARTTIME;
                DtpVISITDATETIME.Value = DtpVISITRESERVEDATE.Value;

                ChaneTime();
                string xx = CboVISITMANAGERID.Text;

                HcUsersRepository repo = new HcUsersRepository();
                HC_USER user = repo.FindOne(dto.VISITMANAGERID);
                if (user != null)
                {
                    if (user.Role == "DOCTOR")
                    {
                        CboVISITDOCTOR.SetValue(user.UserId);
                    }
                }

                CboVISITUSER.SetValue(dto.VISITUSERID);
              
                
              //  dto.CboVISITMANAGERID
            }

          
        }

        private void BtnShowCalendar_Click(object sender, EventArgs e)
        {
            new CalendarForm().Show();
        }

        private void BtnSearchSchedule_Click(object sender, EventArgs e)
        {
            SearchScheduleList();
        }


        private void SearchScheduleList()
        {
            string month = CboMonth.GetValue();
            string startDate = month + "-01";  
            string endDate = month + "-31";  
            long siteId = 0;
            if (base.SelectedSite != null)
            {
                siteId = base.SelectedSite.ID;
            }
            if (ChkSearchSchedule.Checked)
            {
                siteId = 0;
            }

            //CboSearchScheduleSite.GetValue().To<long>(0);
            //long visitUserid =CboSearchScheduleVisitUserId.GetValue().To<long>(0);
            //long visituserId2 = CboSearchScheduleVisitUserId2.GetValue().To<long>(0);

            string visitUserid = CboSearchScheduleVisitUserId.GetValue();
            string visituserId2 = CboSearchScheduleVisitUserId2.GetValue();

            List<HC_OSHA_SCHEDULE> list = hcOshaScheduleService.FindAll(siteId, startDate, endDate, visitUserid, visituserId2);
            SSScheduleList.SetDataSource(list);
            HC_OSHA_SCHEDULE beforDto = null;
            for (int i = 0; i < SSScheduleList.RowCount(); i++)
            {
                HC_OSHA_SCHEDULE dto = SSScheduleList.GetRowData(i) as HC_OSHA_SCHEDULE;
         
                DateTime date = (DateTime)dto.VISITRESERVEDATE;

                string dayOfWeek = DateUtil.ToDayOfWeek(date);
                SSScheduleList.ActiveSheet.Cells[i, 0].Text = date.ToString("yyyy-MM-dd")  + "(" + dayOfWeek.Substring(0,1) +")";
                if (dto.ID == 754)
                {
                    string xx = ";";
                }
                if(dto.GBCHANGE == null)
                {

                }
                else if (dto.GBCHANGE.Equals("1"))
                {
                    SSScheduleList.ActiveSheet.Rows[i].ForeColor = Color.FromArgb(0, 0, 160);
                }
                else if (dto.GBCHANGE.Equals("Y"))
                {
                    SSScheduleList.ActiveSheet.Rows[i].ForeColor = Color.FromArgb(255, 0, 0);
                }

                //if(beforDto != null)
                //{
                //    if (dto.VISITRESERVEDATE != beforDto.VISITRESERVEDATE)
                //    {
                //        SSScheduleList.ActiveSheet.Rows[i-1].Border = new FarPoint.Win.LineBorder(Color.Red, 2, false, false, false, true);
                //    }
                //}
                
                

                if (i > 0)
                {
                    if(list[i-1].VISITRESERVEDATE == dto.VISITRESERVEDATE && list[i-1].SITE_ID == dto.SITE_ID) {
                        if(dto.VISITUSERID == "25091" || dto.VISITUSERID == "44918")
                        {
                            SSScheduleList.ActiveSheet.Rows[i].ForeColor = Color.Green;
                            SSScheduleList.ActiveSheet.Rows[i - 1].ForeColor = Color.Green;
                        }
                            
                    }
                }
                beforDto = dto;
            }
        }

        private void BtnPrintSchedule_Click(object sender, EventArgs e)
        {
            SpreadPrint print = new SpreadPrint(SSScheduleList, PrintStyle.STANDARD_APPROVAL, false);
            print.orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            print.Execute();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSearchPreVisit_Click(object sender, EventArgs e)
        {
            string visitUserId = CboSearchPreVisitUserId.GetValue();
            string visitStartDate = DtpSearchPreVisitStartDate.GetValue();
            string visitEndDate = DtpSearchPreVisitEndDate.GetValue();
            List<VisitSiteModel> list = schduleModelService.scheduleModelRepository.FindPrehargeVisitSiteList(visitUserId, visitStartDate, visitEndDate);

            SSPreCharge.SetDataSource(list);
        }

        private void SSPreCharge_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            VisitSiteModel model = SSPreCharge.GetRowData(e.Row) as VisitSiteModel;

            SetScheduleAndVisitForm(model.SCHEDULE_ID, model.VISIT_ID);
        }

        private void SSScheduleList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            HC_OSHA_SCHEDULE model =  SSScheduleList.GetRowData(e.Row) as HC_OSHA_SCHEDULE;
            SetScheduleAndVisitForm(model.ID, model.VISIT_ID);

        }

        private void CboVISITMANAGERID_Click(object sender, EventArgs e)
        {

        }

        private void ChkDoctor_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkDoctor.Checked)
            {
                CboVISITMANAGERID.SetValue("33398");
            }
            else
            {
                CboVISITMANAGERID.Text = "";
            }
        }

        private void DtpDEPARTUREDATETIME_ValueChanged(object sender, EventArgs e)
        {
            //string start = DtpDEPARTUREDATETIME.GetValue();
            
            ////DtpARRIVALTIME.SetValue(start);

            //DtpARRIVALTIME.Value = DtpDEPARTUREDATETIME.Value.AddHours(2);
        }

        private void CboSearchScheduleVisitUserId_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchScheduleList();
        }

        private void TxtSearchUnvisitSiteIdOrName_TextChanged(object sender, EventArgs e)
        {
            if (TxtSearchUnvisitSiteIdOrName.Text.Length ==0) 
            {
                BtnSearchUnVisit.PerformClick();

            }
        }

        private void TxtSearchVisitSiteIdOrName_TextChanged(object sender, EventArgs e)
        {
            if (TxtSearchVisitSiteIdOrName.Text.Length == 0)
            {
                BtnSearchVisit.PerformClick();

            }
        }

        private void CboSearchUnVisitUserId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnSearchVisit.PerformClick();
        }

        private void CboSearchVisitUserId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnSearchVisit.PerformClick();

        }

        private void CboSearchPreVisitUserId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnSearchPreVisit.PerformClick();
        }

        private void txtStartTime_TextChanged(object sender, EventArgs e)
        {
            ChaneTime();
        }

        private void txtEndTime_TextChanged(object sender, EventArgs e)
        {
            ChaneTime();
        }

        private void panSchedule_Paint(object sender, PaintEventArgs e)
        {

        }

        private void OshaSiteLastTree_Load(object sender, EventArgs e)
        {

        }
    }
}
