using CefSharp;
using CefSharp.WinForms;
using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using ComHpcLibB.Repository;
using FarPoint.Win.Spread;
using HC.Core.Common.Interface;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Model;
using HC.OSHA.Repository;
using HC.OSHA.Repository.StatusReport;
using HC.OSHA.Service;
using HC.OSHA.Service.StatusReport;
using HC_Core;
using HC_Core.Macroword;
using HC_OSHA.Model.StatussReport;
using HC_OSHA.Repository.StatusReport;
using HC_OSHA.StatusReport;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HC_OSHA
{
    public partial class StatusReportByNurseForm : CommonForm, ISelectSite
    {
        private AutoCompleteMacro autoCompleteMacro;
        private ChromiumWebBrowser browser = null;
        private CkEditor ckEditor = null;
        private StatusReportNurseService statusReportNurseService;
        private HcOshaContractRepository hcOshaContractRepository;
        private HcSiteViewService hcSiteViewService;
        private HcSiteWorkerService hcSiteWorkerService;

        private WorkerHealthCheckForm workerHealthCheckForm;
        private HcOshaCard19Service hcOshaCard19Service;
        private StatusReportViewer statusReportViewer;
        private List<MacrowordDto> card19List = null;
        private StatusReportMemoRepository statusReportMemoRepository;

        private HcOshaCard5Service hcOshaCard5Service;
        public StatusReportByNurseForm()
        {
            InitializeComponent();
            statusReportNurseService = new StatusReportNurseService();
            hcOshaContractRepository = new HcOshaContractRepository();
            hcSiteViewService = new HcSiteViewService();
            workerHealthCheckForm = new WorkerHealthCheckForm();
            card19List = new List<MacrowordDto>();
            hcOshaCard19Service = new HcOshaCard19Service();
            hcSiteWorkerService = new HcSiteWorkerService();
            statusReportMemoRepository = new StatusReportMemoRepository();
            hcOshaCard5Service = new HcOshaCard5Service();
        }

        private void StatusReportByNurseForm_Load(object sender, EventArgs e)
        {
            //autoCompleteMacro = new AutoCompleteMacro(this.Name);
            //autoCompleteMacro.Add(TxtIsEduTitle);
            //this.Controls.Add(autoCompleteMacro);

            workerHealthCheckForm.Dock = DockStyle.Fill;
            workerHealthCheckForm.FormBorderStyle = FormBorderStyle.None;
            workerHealthCheckForm.TopLevel = false;
            workerHealthCheckForm.Show();
            tabPage2.Controls.Add(workerHealthCheckForm);

            DtpVisitDate.SetOptions(new DateTimePickerOption { DataField = nameof(StatusReportNurseDto.VISITDATE), DataBaseFormat = DateTimeType.YYYYMMDD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpVisitReserveDate.SetOptions(new DateTimePickerOption { DataField = nameof(StatusReportNurseDto.VISITRESERVEDATE), DataBaseFormat = DateTimeType.YYYYMMDD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            TxtSITEMANAGERGRADE.SetOptions(new TextBoxOption { DataField = nameof(StatusReportNurseDto.SITEMANAGERGRADE) });
            TxtSITEMANAGERNAME.SetOptions(new TextBoxOption { DataField = nameof(StatusReportNurseDto.SITEMANAGERNAME) });
            TxtNURSENAME.SetOptions(new TextBoxOption { DataField = nameof(StatusReportNurseDto.NURSENAME) });
            ChkIsOshaPlan.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsOshaPlan), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsOshaGeneral.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsOshaGeneral), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsMsds.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsMsds), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsMsdsInstall.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsMsdsInstall), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsMsdsSpeacial.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsMsdsSpeacial), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsHcPlan.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsHcPlan), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsHcManage.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsHcManage), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsHcExplain.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsHcExplain), CheckValue = "Y", UnCheckValue = "N" });

            ChkIsDesease.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsDesease), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsDesease2.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsDesease2), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsDeseaseCheck.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsDeseaseCheck), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsDeseaseCheck2.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsDeseaseCheck2), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsEmergency.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsEmergency), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsEmergencyManage.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsEmergencyManage), CheckValue = "Y", UnCheckValue = "N" });

            ChkIsJobManage.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsJobManage), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsJobManage2.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsJobManage2), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsCommite.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsCommite), CheckValue = "Y", UnCheckValue = "N" });

            ChkIsProtection1.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsProtection1), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsProtection2.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsProtection2), CheckValue = "Y", UnCheckValue = "N" });


            ChkIsEdu.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsEdu), CheckValue = "Y", UnCheckValue = "N" });
            RdoIsEduType1.SetOptions(new RadioButtonOption { DataField = nameof(NursePerformContentJson.IsEduType), CheckValue = "Y", UnCheckValue = "N" });
            RdoIsEduType2.SetOptions(new RadioButtonOption { DataField = nameof(NursePerformContentJson.IsEduType), CheckValue = "N", UnCheckValue = "Y" });
            ChkIsEduKind1.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsEduKind1), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsEduKind2.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsEduKind2), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsEduKind3.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsEduKind3), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsEduKind4.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsEduKind4), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsEduKind5.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsEduKind5), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsEduKind6.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsEduKind6), CheckValue = "Y", UnCheckValue = "N" });

            NumIsEduCount.SetOptions(new NumericUpDownOption { DataField = nameof(NursePerformContentJson.IsEduCount) });
            TxtIsEduTitle.SetOptions(new TextBoxOption { DataField = nameof(NursePerformContentJson.IsEduTitle) });

            //건강증진
            ChkIsHealth1.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsHealth1), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsHealth2.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsHealth2), CheckValue = "Y", UnCheckValue = "N" });

            ChkIsDe1.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsDe1), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsDe2.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsDe2), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsDe3.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsDe3), CheckValue = "Y", UnCheckValue = "N" });

            ChkIsHe1.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsHe1), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsHe2.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsHe2), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsHe3.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsHe3), CheckValue = "Y", UnCheckValue = "N" });




            //건강상담
            NumSangdamCount.SetOptions(new NumericUpDownOption { DataField = nameof(NursePerformContentJson.SangdamCount) });
            ChkIsSangdam1.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsSangdam1), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsSangdam2.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsSangdam2), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsSangdam3.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsSangdam3), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsSangdam4.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsSangdam4), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsSangdam5.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsSangdam5), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsSangdam6.SetOptions(new CheckBoxOption { DataField = nameof(NursePerformContentJson.IsSangdam6), CheckValue = "Y", UnCheckValue = "N" });


            NumCheckCount1.SetOptions(new NumericUpDownOption { DataField = nameof(NursePerformContentJson.CheckCount1) });
            NumCheckCount2.SetOptions(new NumericUpDownOption { DataField = nameof(NursePerformContentJson.CheckCount2) });
            NumCheckCount3.SetOptions(new NumericUpDownOption { DataField = nameof(NursePerformContentJson.CheckCount3) });
            NumCheckCount4.SetOptions(new NumericUpDownOption { DataField = nameof(NursePerformContentJson.CheckCount4) });
            TxtOshaData.SetOptions(new TextBoxOption { DataField = nameof(NursePerformContentJson.OshaData) });

            //    Clear();

            ckEditor = new CkEditor("nurseCkeditor.html");
            browser = ckEditor.GetBrowser();
            PanWeb.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            browser.Show();

            TabReport.SelectedIndex = 1;
            TabReport.SelectedIndex = 0;


            SSCard19.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSCard19.AddColumnText("업무수행내용", nameof(MacrowordDto.TITLE), 150, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true });
            SSCard19.AddColumnText("문제점 및 개선내용", nameof(MacrowordDto.SUBTITLE), 150, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true });
            SSCard19.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += StatusReportByEngineer_ButtonClick;
            SSCard19.SetDataSource(new List<MacrowordDto>());

            SSReportList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSReportList.AddColumnText("ID", nameof(VisitDateModel.ID), 59, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSReportList.AddColumnText("방문일자", nameof(VisitDateModel.VisitDate), 78, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSReportList.AddColumnText("작성자", nameof(VisitDateModel.Name), 55, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

            SSperformContent.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSperformContent.AddColumnCheckBox("", "", 30, new CheckBoxBooleanCellType());
            SSperformContent.AddColumnText("수행내용", nameof(PerformContentModel.CheckboxText), 304, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Left });
            SSperformContent.AddColumnCheckBox("선택여부", nameof(PerformContentModel.IsChecked), 35, new CheckBoxStringCellType {  IsHeaderCheckBox = false,  CheckedValue = "Y", UnCheckedValue = "N" }, new SpreadCellTypeOption { IsSort = false, });

            WindowState = FormWindowState.Maximized;
        }
        private void StatusReportByEngineer_ButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            SSCard19.ActiveSheet.RemoveRows(e.Row, 1);
        }


        private void BtnSearchSite_Click(object sender, EventArgs e)
        {
            //SiteListForm form = new SiteListForm();

            //HC_SITE_VIEW siteView = form.Search(TxtSearchSite.Text);
            //if(siteView == null)
            //{
            //    DialogResult result = form.ShowDialog();
                
            //    if (result == DialogResult.OK)
            //    {
            //        siteView = form.SelectedSite;

            //    }
            //}
            //else
            //{
            //    form.Close();
            //}

            //if (siteView != null)
            //{
            //    SetSite(siteView);
            //}
        }
        public void SetSite()
        {

            ContentTitle.TitleText = "보건관리상태보고서 간호사 - " + base.SelectedSite.NAME;

            Clear();

            siteStatusControl.Initialize(this, DtpVisitDate.GetValue());
            siteStatusControl.SetTxtOshaData(TxtOshaData);
            siteStatusControl.SetSitName(base.SelectedSite.NAME);

            workerHealthCheckForm.Init();
       
            if (TabReport.SelectedIndex == 2)
            {
                workerHealthCheckForm.Select(base.SelectedSite);
            }

          
            SearchReport();

            HIC_OSHA_MEMO memo = statusReportMemoRepository.FindOne(base.SelectedSite.ID);
            if (memo != null)
            {
                TxtMemo.Text = memo.MEMO;
            }
            else
            {
                TxtMemo.Text = "";
            }
        }

    
        public void Clear()
        {
         
            PanStatausReportNurse.SetData(new StatusReportNurseDto());
            siteStatusControl.Initialize(this, DtpVisitDate.GetValue());
            GrpPerformContent.Initialize();

            ckEditor.Clear();
            card19List.Clear();
            TxtNURSENAME.Text = clsType.User.UserName;//CommonService.Instance.Session.UserName;
            workerHealthCheckForm.Init();
            workerHealthCheckForm.StatusReportDoctorDto = null;
            workerHealthCheckForm.StatusReportNurseDto = null;
            if (SelectedSite != null)
            {
                ContentTitle.TitleText = "보건관리상태보고서 간호사 - " + base.SelectedSite.NAME;
                HC_SITE_WORKER worker = hcSiteWorkerService.FindHealthRole(SelectedSite.ID);
                if (worker != null)
                {
                    TxtSITEMANAGERGRADE.Text = "보건담당자";
                    TxtSITEMANAGERNAME.Text = worker.NAME;
                }
                siteStatusControl.SetSitName(SelectedSite.NAME);
            }
          
        }
      
              

        private void SetData(StatusReportNurseDto dto)
        {
            PanStatausReportNurse.SetData(dto);
            if (dto.SiteStatusDto != null)
            {
                siteStatusControl.SetData(dto.SiteStatusDto);
            }
            

            if (dto.PERFORMCONTENT.NotEmpty())
            {
                NursePerformContentJson jsonDto = JsonConvert.DeserializeObject<NursePerformContentJson>(dto.PERFORMCONTENT);
                panel2.SetData(jsonDto);


                //    jsonDto.CheckCount1 = 123;

                //NumCheckCount1.SetValue(jsonDto.CheckCount1);
            }

            string month = dto.VISITDATE.Substring(4, 2);
            if (month.StartsWith("0"))
            {
                month = month.Substring(1, 1);
            }

            ContentTitle.TitleText = "보건관리상태보고서 간호사 - " + base.SelectedSite.NAME + " " + month + "월 상태보고서";

            ckEditor.SetStatusReportNurseDto(dto);
            workerHealthCheckForm.StatusReportNurseDto = dto;
            //GrpPerformContent.Initialize();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null)
            {
                MessageUtil.Alert("사업장을 선택하세요");
            }
            else
            {
                if (base.SelectedEstimate == null)
                {
                    MessageUtil.Alert("계약 내용이 없습니다");
                    return;
                }
                StatusReportNurseDto dto = PanStatausReportNurse.GetData<StatusReportNurseDto>();

                PanStatausReportNurse.Validate<StatusReportNurseDto>();

                dto.SITE_ID = base.SelectedSite.ID;
                string visitdate = dto.VISITDATE.Substring(0, 4) + "-" + dto.VISITDATE.Substring(4, 2) + "-" + dto.VISITDATE.Substring(6, 2);
                HC_OSHA_CONTRACT contract = hcOshaContractRepository.FindByDate(dto.SITE_ID, visitdate);
                if(contract == null)
                {
                    MessageUtil.Alert("방문일자가 계약일자에 해당되지 않습니다.");
                    return;
                }
                dto.ESTIMATE_ID = contract.ESTIMATE_ID;
                SiteStatusDto siteStatusDto = siteStatusControl.GetData();

                dto.SiteStatusDto = siteStatusDto;

                NursePerformContentJson performContentJson = panel2.GetData<NursePerformContentJson>();

                //업무수행내용
                string json = JsonConvert.SerializeObject(performContentJson);
                dto.PERFORMCONTENT = json;

                if (dto.ID > 0)
                {
                    if (statusReportNurseService.IsGranted(dto.ID) == false)
                    {
                        MessageUtil.Alert("수정 권한이 없습니다");
                        return;
                    }
                }

                StatusReportNurseDto saved = this.statusReportNurseService.Save(dto);

                //  사원 입/퇴사자 정보 저장
                HC_OSHA_CARD5 workInOutCheck = new HC_OSHA_CARD5
                {
                    ESTIMATE_ID = base.SelectedEstimate.ID,
                    SITE_ID = base.SelectedSite.ID,
                    REGISTERDATE = DtpVisitDate.Text,
                    JOINCOUNT = siteStatusDto.NEWWORKERCOUNT,
                    QUITCOUNT = siteStatusDto.RETIREWORKERCOUNT
                };

                hcOshaCard5Service.InsertOrUpdate(workInOutCheck);

                try
                {
                    HIC_OSHA_MEMO memo = this.statusReportMemoRepository.FindOne(dto.SITE_ID);
                    if (memo == null)
                    {
                        memo = new HIC_OSHA_MEMO();
                        memo.SITEID = dto.SITE_ID;
                        memo.MEMO = TxtMemo.Text;
                        this.statusReportMemoRepository.Insert(memo);
                    }
                    else
                    {
                        memo.MEMO = TxtMemo.Text;
                        //if (!memo.MEMO.IsNullOrEmpty())
                        {
                            this.statusReportMemoRepository.Update(memo);
                        }

                    }
                }
                catch(Exception ex)
                {
                    Log.Error(ex);
                }
         

                ckEditor.SetStatusReportNurseDto(saved);

                //종합의견
                browser.ExecuteScriptAsync("save()");

                SearchReport();
              //  Clear();
                MessageUtil.Info("저장하였습니다");
                for (int i = 0; i < SSReportList.ActiveSheet.RowCount; i++)
                {
                    long id = SSReportList.ActiveSheet.Cells[i, 0].Value.To<long>(0);
                    if (id == saved.ID)
                    {
                        SSReportList_CellDoubleClick(SSReportList, new CellClickEventArgs(null, i, 0, 0, 0, MouseButtons.Left, false, false, false));
                    }
                }
            }
        }

        private void SearchReport()
        {
            List<VisitDateModel> list = statusReportNurseService.StatusReportNurseRepository.FindAll(base.SelectedSite.ID);
            SSReportList.SetDataSource(list);
            if (SSReportList.RowCount() > 0)
            {
                SSReportList_CellDoubleClick(SSReportList, new CellClickEventArgs(null, 0, 0, 0, 0, MouseButtons.Left, false, false, false));
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            //List<string> test = new List<string>();
            //for (int i = 0; i < 30; i++)
            //{
            //    string tmp = "A10" + i;
            //    test.Add(tmp);
            //}

     //       string sqls = "EXCODE_0,:EXCODE_1,:EXCODE_2,:EXCODE_3,:EXCODE_4,:EXCODE_1,:E";
     //       string key = "EXCODE_1";
     //       string searchText = "\\b:"+ key+"\\b";
            
     ////       string pattern = searchText.Replace("\\", "");
     //       Regex regex = new Regex(searchText, RegexOptions.Compiled);
     //       bool xxxx = regex.IsMatch(sqls);

     //       MatchCollection matches2 = regex.Matches(sqls);
     //       int mm = matches2.Count;
     //       MatchCollection matches = Regex.Matches(sqls, @"ex1\b");
     //       //MatchCollection matches = Regex.Matches(sqls, @searchText);
     //       int matchCount = matches.Count;
     //       string x = "";

     //       HicResultExCodeRepository repo = new HicResultExCodeRepository();
     //       repo.GetItembyWrtNoInExCodes(11, test);

            StatusReportNurseDto dto = PanStatausReportNurse.GetData<StatusReportNurseDto>();
            bool isDeleted = false;
            if (dto.ID > 0)
            {
                HealthCheckService healthCheckService = new HealthCheckService();
                List<HealthCheckDto> list = healthCheckService.healthCheckRepository.FindAll(dto.ID);
                if (list.Count > 0)
                {
                    if (MessageUtil.Confirm(dto.ID + "번 상태보고서를 삭제 하시겠습니까? 근로자 상담이 작성되어 있습니다.(상담내용을 확인하세요) ") == DialogResult.Yes) 
                    {
                        isDeleted = true;
                    }
                }
                else
                {
                    if (MessageUtil.Confirm(dto.ID + "번 상태보고서를 삭제 하시겠습니까?") == DialogResult.Yes)
                    {
                        isDeleted = true;
                      
                    }
                }
                if (isDeleted)
                {
                    statusReportNurseService.StatusReportNurseRepository.Delete(dto.ID);

                    Clear();

                    SearchReport();

                }


            }
        }
   
        private void BtnNew_Click(object sender, EventArgs e)
        {
            Clear();
            ckEditor.Clear();
            //NumCheckCount1.Value = 123;
        }

        private void BtnLast_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null)
            {
                return;
            }

            StatusReportNurseDto dto = statusReportNurseService.StatusReportNurseRepository.FindLast(base.SelectedSite.ID);
            if (dto != null)
            {
                dto.ID = 0;
                dto.PERFORMCONTENT = null;
                dto.SANGDAMSIGN = "";
              
                SetData(dto);
            }
        }

     
        private void SaveOpinionBtn_Click(object sender, EventArgs e)
        {
            if (browser.CanExecuteJavascriptInMainFrame)
            {
                if(MessageUtil.Confirm("저장 하시겠습니까?", this, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    browser.ExecuteScriptAsync("save()");
                }
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            StatusReportNurseDto dto = PanStatausReportNurse.GetData<StatusReportNurseDto>();
            if (dto.ID > 0)
            {
                dto = this.statusReportNurseService.StatusReportNurseRepository.FindOne(dto.ID);
             
                if (statusReportViewer != null)
                { 
                    if (!statusReportViewer.IsDisposed)
                    {
                        statusReportViewer.Dispose();
                    }
                }
                statusReportViewer = new StatusReportViewer("statusReportByNurse.html", base.SelectedSite.ID);
                statusReportViewer.PrintStatusReportNurseDto(dto, ContentTitle.TitleText);
                statusReportViewer.ShowDialog();
            }
        }

        public void Select(ISiteModel siteModel)
        {
            if (siteModel.ID > 0)
            {
               // SetSite(hcSiteViewService.FindById(siteModel.ID));
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMacro_Click(object sender, EventArgs e)
        {
          
            MacrowordCkeditorForm form = new MacrowordCkeditorForm("statusReportByNurseForm", "OPINON", this.browser);
            form.ShowDialog();

            foreach (MacrowordDto dto in form.GetCard19List())
            {
                this.card19List.Add(dto);
            }

            List<MacrowordDto> list = new List<MacrowordDto>();
            list.AddRange(card19List);
            SSCard19.SetDataSource(list);
        }

        private void DtpVisitDate_ValueChanged(object sender, EventArgs e)
        {
            if (base.SelectedSite != null)
            {
                string dtpVisitDate = DtpVisitDate.GetValue();
                string visitdate = dtpVisitDate.Substring(0, 4) + "-" + dtpVisitDate.Substring(4, 2) + "-" + dtpVisitDate.Substring(6, 2);

                HcEstimateModelRepository repo = new HcEstimateModelRepository();
                HC_OSHA_CONTRACT contract = hcOshaContractRepository.FindByDate(base.SelectedSite.ID, visitdate);
                if (contract == null)
                {
                    MessageUtil.Alert("방문일자가 계약일자에 해당되지 않습니다.");
                    return;
                }
                this.SelectedEstimate = repo.FindByEstimateId(contract.ESTIMATE_ID);
            }
        }

        private void BtnWorker_Click(object sender, EventArgs e)
        {
            SiteWorkerPopupForm form = new SiteWorkerPopupForm();
            form.SelectedSite = base.SelectedSite;
            form.ShowDialog();
            foreach (HC_SITE_WORKER worker in form.GetWorker())
            {
                TxtSITEMANAGERGRADE.Text = codeService.FindActiveCodeByGroupAndCode("WORKER_ROLE", worker.WORKER_ROLE, "OSHA").CodeName;
                TxtSITEMANAGERNAME.Text = worker.NAME;
            }
        }

        private void BtnOshaCard19_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null)
            {
                MessageUtil.Alert("상태보고서를 선택하세요");
                return;
            }
            try
            {
                string content = string.Empty;
                string opinion = string.Empty;
                for (int i = 0; i < SSCard19.RowCount(); i++)
                {
                    MacrowordDto macroword = SSCard19.GetRowData(i) as MacrowordDto;
                    content += macroword.TITLE + "\n";
                    opinion += macroword.SUBTITLE + "\n";
                }

                HC_OSHA_CARD19 dto = new HC_OSHA_CARD19();
                dto.REGDATE = DateUtil.stringToDateTime(DtpVisitDate.GetValue(), ComBase.Controls.DateTimeType.YYYYMMDD);
                dto.CONTENT = content;
                dto.OPINION = opinion;
                dto.SITE_ID = SelectedSite.ID;
                dto.ESTIMATE_ID = SelectedEstimate.ID;
                dto.NAME = clsType.User.UserName;
                dto.CERT = "간호사";
                hcOshaCard19Service.Save(dto);

                MessageUtil.Info("위탁업무수행일지 저장하였습니다.");
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                MessageUtil.Alert(ex.Message);
            }
            //browser.ExecuteScriptAsync("saveCard19()");
           
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabReport.SelectedIndex == 2)
            {
                workerHealthCheckForm.SelectedSite = base.SelectedSite;
                workerHealthCheckForm.SetDept();
                workerHealthCheckForm.SetPanjeong();
            }
        }

        private void oshaSiteList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            base.SelectedSite = oshaSiteList1.GetSite;
            SetSite();
        }

        /// <summary>
        /// 보건관리상태보고서 하단 스프레드 더블클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SSReportList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (!e.ColumnHeader)
            {
                VisitDateModel model = SSReportList.GetRowData(e.Row) as VisitDateModel;
                StatusReportNurseDto dto = this.statusReportNurseService.StatusReportNurseRepository.FindOne(model.ID);
                if (dto != null)
                {
                    SetData(dto);

                    //  메크로 정보 조회하기
                    //MacrowordDto macroword = this.statusReportNurseService.StatusReportNurseRepository.FindOneCard(dto);
                }
                else
                {
                    SetData(new StatusReportNurseDto());
                }
                workerHealthCheckForm.Select(base.SelectedSite);
            }
            
        }

        private void BtnLoadCount_Click(object sender, EventArgs e)
        {
            StatusReportNurseDto dto = PanStatausReportNurse.GetData<StatusReportNurseDto>();
            if (dto.ID > 0)
            {
                SangDamCountRepository sangDamCountRepository = new SangDamCountRepository();
                List<SangDamVitalCountModel> list = sangDamCountRepository.FindSangDamVital(dto.ID);
                NumSangdamCount.SetValue(list[0].COUNT);
                NumCheckCount1.SetValue(list[1].COUNT); //혈압
                NumCheckCount2.SetValue(list[2].COUNT); // 혈당
                NumCheckCount3.Value = list[3].COUNT; // 소변
                NumCheckCount4.Value = list[4].COUNT; //BMI
            }
               
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
           

        }

        private void BtnGetPerformCheckbox_Click(object sender, EventArgs e)
        {
            List<PerformContentModel> list = new List<PerformContentModel>();
            foreach (Control item in GrpPerformContent.Controls)
            {
                if (item is CheckBox)
                {
                    string xx = item.Text;
                    CheckBox chk = item as CheckBox;
                    PerformContentModel model = new PerformContentModel();
                    model.CheckboxText = chk.Text;
                    if (chk.Checked)
                    {
                        model.IsChecked = "Y";
                    }
                    else
                    {
                        model.IsChecked = "N";
                    }
                    list.Add(model);
                }
            }

            SSperformContent.SetDataSource(list);
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            string content = string.Empty;
            for(int i=0; i< SSperformContent.RowCount(); i++)
            {
                if (SSperformContent.ActiveSheet.Cells[i, 0].Value == null)
                {
                    continue;
                }

                if (SSperformContent.ActiveSheet.Cells[i, 0].Value.Equals(true))
                {
                    PerformContentModel model = SSperformContent.GetRowData(i) as PerformContentModel;
                    //content += model.CheckboxText + "<br>";
                    if (browser != null)
                    {
                        if (browser.CanExecuteJavascriptInMainFrame)
                        {
                            browser.ExecuteScriptAsync("performContentLoad('" + model.CheckboxText + "')");

                        }
                    }
                }
            }
      
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }

        private void BtnManageOshaCard19_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null || base.SelectedEstimate == null)
            {
                return;
            }

            CardPage_15_Form form = new CardPage_15_Form();
            form.Show();

            if (base.SelectedSite.ID > 0)
            {
                (form as ISelectSite).Select(base.SelectedSite);
            }

            if (base.SelectedEstimate.ID > 0)
            {
                (form as ISelectEstimate).Select(base.SelectedEstimate);

            }
        }

        private void oshaSiteList1_CellClick(object sender, CellClickEventArgs e)
        {

        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (MessageUtil.Confirm("내용을 전부 지우시겠습니까?", this, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                browser.ExecuteScriptAsync("clear()");
            }
        }

        private void BtnInfo_Click(object sender, EventArgs e)
        {
            InformationForm informationForm = new InformationForm(true);
            informationForm.SelectedSite = base.SelectedSite;
            informationForm.OnSelected += (item) =>
            {
                if (item != null)
                {
                    TxtOshaData.Text = item.REMARK;
                }
            };

            informationForm.Width = 900;
            informationForm.Show(this);
        }

        private void oshaSiteList1_Load(object sender, EventArgs e)
        {

        }
    }
}
