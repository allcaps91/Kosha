using CefSharp;
using CefSharp.WinForms;
using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using FarPoint.Win.Spread;
using HC.Core.Common.Interface;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC.OSHA.Repository;
using HC.OSHA.Repository.StatusReport;
using HC.OSHA.Service;
using HC.OSHA.Service.StatusReport;
using HC_Core;
using HC_Core.Macroword;
using HC_OSHA.StatusReport;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_OSHA
{
    /// <summary>
    /// 상태보고서 산업위생기사
    /// </summary>
    public partial class StatusReportByEngineer : CommonForm, ISelectSite
    {
        private HcOshaContractRepository hcOshaContractRepository;
        private HcSiteViewService hcSiteViewService;
        private ChromiumWebBrowser browser = null;
        private CkEditor ckEditor = null;
        private readonly StatusReportEngineerService statusReportEngineerService;
        private readonly StatusReportEngineerRemarkService statusReportEngineerRemarkService;
        private StatusReportViewer statusReportViewer;
        private HcOshaCard19Service hcOshaCard19Service;
        private HcSiteWorkerService hcSiteWorkerService;
        private List<MacrowordDto> card19List = null;

        private StatusReportMemoRepository statusReportMemoRepository;

        public StatusReportByEngineer()
        {
            InitializeComponent();
            statusReportEngineerService = new StatusReportEngineerService();
            statusReportEngineerRemarkService = new StatusReportEngineerRemarkService();
            hcSiteViewService = new HcSiteViewService();
            hcOshaContractRepository = new HcOshaContractRepository();
            card19List = new List<MacrowordDto>();
            hcOshaCard19Service = new HcOshaCard19Service();
            hcSiteWorkerService = new HcSiteWorkerService();
            statusReportMemoRepository = new StatusReportMemoRepository();
        }

        private void StatusReportByEngineer_Load(object sender, EventArgs e)
        {

            CboAccType.SetItems(codeService.FindActiveCodeByGroupCode("SITE_CARD_ACCIDENT_TYPE", "OSHA"), "codename", "code", "", "", AddComboBoxPosition.Top);

            DtpAccDate.SetOptions(new DateTimePickerOption { DataField = nameof(ENVCHECKJSON3.ENVCHECK3_13), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpVisitDate.SetOptions(new DateTimePickerOption { DataField = nameof(StatusReportEngineerDto.VISITDATE), DataBaseFormat = DateTimeType.YYYYMMDD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpVisitReserveDate.SetOptions(new DateTimePickerOption { DataField = nameof(StatusReportEngineerDto.VISITRESERVEDATE), DataBaseFormat = DateTimeType.YYYYMMDD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            TxtSITEMANAGERGRADE.SetOptions(new TextBoxOption { DataField = nameof(StatusReportEngineerDto.SITEMANAGERGRADE) });
            TxtSITEMANAGERNAME.SetOptions(new TextBoxOption { DataField = nameof(StatusReportEngineerDto.SITEMANAGERNAME) });
            TxtENGINEERNAME.SetOptions(new TextBoxOption { DataField = nameof(StatusReportEngineerDto.ENGINEERNAME) });
            TxtSiteManagerTel.SetOptions(new TextBoxOption { DataField = nameof(StatusReportEngineerDto.SITETEL) });
            
            NumWORKERCOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(StatusReportEngineerDto.WORKERCOUNT) });

            DtpWEMDate.SetOptions(new TextBoxOption { DataField = nameof(StatusReportEngineerDto.WEMDATE) });
            DtpWEMDate2.SetOptions(new TextBoxOption { DataField = nameof(StatusReportEngineerDto.WEMDATE2) });
            TxtWEMDateRemark.SetOptions(new TextBoxOption { DataField = nameof(StatusReportEngineerDto.WEMDATEREMARK) });

            RdoWEMEXPORSURE_N.SetOptions(new CheckBoxOption { DataField = nameof(StatusReportEngineerDto.WEMEXPORSURE), CheckValue = "Y", UnCheckValue = "N" });
            RdoWEMEXPORSURE_Y.SetOptions(new CheckBoxOption { DataField = nameof(StatusReportEngineerDto.WEMEXPORSURE1), CheckValue = "Y", UnCheckValue = "N" });
            TxtWEMExporsureRemark.SetOptions(new TextBoxOption { DataField = nameof(StatusReportEngineerDto.WEMEXPORSUREREMARK) });
            TxtWEMHarmfulFactors.SetOptions(new TextBoxOption { DataField = nameof(StatusReportEngineerDto.WEMHARMFULFACTORS) });

            TxtWORKCONTENT.SetOptions(new TextBoxOption { DataField = nameof(StatusReportEngineerDto.WORKCONTENT) });
            DtpOSHADATE.SetOptions(new TextBoxOption { DataField = nameof(StatusReportEngineerDto.OSHADATE) });
            TxtOSHACONTENT.SetOptions(new TextBoxOption { DataField = nameof(StatusReportEngineerDto.OSHACONTENT) });

            TxtEDUTARGET.SetOptions(new TextBoxOption { DataField = nameof(StatusReportEngineerDto.EDUTARGET) });
            TxtEDUPERSON.SetOptions(new TextBoxOption { DataField = nameof(StatusReportEngineerDto.EDUPERSON) });
            TxtEDUAN.SetOptions(new TextBoxOption { DataField = nameof(StatusReportEngineerDto.EDUAN) });
            TxtEDUTITLE.SetOptions(new TextBoxOption { DataField = nameof(StatusReportEngineerDto.EDUTITLE) });

            ChkEduType1.SetOptions(new CheckBoxOption { DataField = nameof(EduTypeJson.ChkEduType1), CheckValue = "Y", UnCheckValue = "N" });
            ChkEduType2.SetOptions(new CheckBoxOption { DataField = nameof(EduTypeJson.ChkEduType2), CheckValue = "Y", UnCheckValue = "N" });
            ChkEduType3.SetOptions(new CheckBoxOption { DataField = nameof(EduTypeJson.ChkEduType3), CheckValue = "Y", UnCheckValue = "N" });
            ChkEduType4.SetOptions(new CheckBoxOption { DataField = nameof(EduTypeJson.ChkEduType4), CheckValue = "Y", UnCheckValue = "N" });

            ChkEduMethod1.SetOptions(new CheckBoxOption { DataField = nameof(EduMethodJson.ChkEduMethod1), CheckValue = "Y", UnCheckValue = "N" });
            ChkEduMethod2.SetOptions(new CheckBoxOption { DataField = nameof(EduMethodJson.ChkEduMethod2), CheckValue = "Y", UnCheckValue = "N" });
            ChkEduMethod3.SetOptions(new CheckBoxOption { DataField = nameof(EduMethodJson.ChkEduMethod3), CheckValue = "Y", UnCheckValue = "N" });

            SSCard19.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSCard19.AddColumnText("업무수행내용", nameof(MacrowordDto.TITLE), 150, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true });
            SSCard19.AddColumnText("문제점 및 개선내용", nameof(MacrowordDto.SUBTITLE), 150, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true });
            SSCard19.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += StatusReportByEngineer_ButtonClick;
            SSCard19.SetDataSource(new List<MacrowordDto>());

            SSReportList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSReportList.AddColumnText("ID", nameof(VisitDateModel.ID), 59, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSReportList.AddColumnText("방문일자", nameof(VisitDateModel.VisitDate), 78, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSReportList.AddColumnText("작성자", nameof(VisitDateModel.Name), 55, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

            ckEditor = new CkEditor("engineerCkeditor.html");
            browser = ckEditor.GetBrowser();
            
            PanWeb.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            browser.Show();
            InitForm();
            
            tabControl1.SelectedIndex = 4;//browser 초기화를 위해서
            tabControl1.SelectedIndex = 0;

            WindowState = FormWindowState.Maximized;

        }

        private void StatusReportByEngineer_ButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            SSCard19.ActiveSheet.RemoveRows(e.Row,1);
        }


      
        public void InitForm()
        {
            TxtSiteName.Text = string.Empty;
            TxtSiteManagerTel.Text = string.Empty;
            TxtSiteCeoName.Text = string.Empty;
            TxtSiteAddress.Text = string.Empty;

            PanStatausReport.SetData(new StatusReportEngineerDto());
            InitENVCHECKJSON(GrpEnvCheckJson1, "RdoENVCHECKJSON");
            InitENVCHECKJSON(GrpEnvCheckJson2, "RdoENVCHECKJSON2");
            InitENVCHECKJSON(GrpEnvCheckJson3, "RdoENVCHECKJSON3");

            InitRadioButton(GrpEnvCheckJson1, "RdoENVCHECKJSON","");
            InitRadioButton(GrpEnvCheckJson2, "RdoENVCHECKJSON2","");
            InitRadioButton(GrpEnvCheckJson3, "RdoENVCHECKJSON3","");

            ckEditor.Clear();
            card19List.Clear();
            TxtENGINEERNAME.Text = clsType.User.UserName;
            DtpAccDate.Value = DateTimePicker.MinimumDateTime;
            if (SelectedSite != null)
            {
                ContentTitle.TitleText = "보건관리상태보고서 산업위생기사 - " + base.SelectedSite.NAME;
                HC_SITE_WORKER worker = hcSiteWorkerService.FindHealthRole(SelectedSite.ID);
                if (worker != null)
                {
                    TxtSITEMANAGERGRADE.Text = "보건담당자";
                    TxtSITEMANAGERNAME.Text = worker.NAME;
                    TxtSiteManagerTel.Text = worker.HP;
                }
            }
        }

        private void InitENVCHECKJSON(GroupBox groupBox, string rdoButtonStartName)
        {
            string value = string.Empty;
            string number = string.Empty;
            foreach (Control control in groupBox.Controls)
            {
                if (control is FlowLayoutPanel)
                {
                    foreach (Control rdo in control.Controls)
                    {
                        if (rdo.Name.StartsWith(rdoButtonStartName))
                        {
                            if (rdoButtonStartName == "RdoENVCHECKJSON")
                            {
                                value = rdo.Name.Split(new char[] { '_' })[1];
                                number = rdo.Name.Split(new char[] { '_' })[0].Replace(rdoButtonStartName, "");

                            }
                            else
                            {
                                value = rdo.Name.Split(new char[] { '_' })[2];
                                number = rdo.Name.Split(new char[] { '_' })[1];

                            }

                            RadioButton radioButton = rdo as RadioButton;
                            if (rdoButtonStartName == "RdoENVCHECKJSON")
                            {
                                radioButton.SetOptions(new RadioButtonOption { DataField = "ENVCHECK" + number, CheckValue = value, UnCheckValue = "" });
                            }
                            else if (rdoButtonStartName == "RdoENVCHECKJSON2")
                            {
                                radioButton.SetOptions(new RadioButtonOption { DataField = "ENVCHECK2_" + number, CheckValue = value, UnCheckValue = "" });
                            }
                            else if (rdoButtonStartName == "RdoENVCHECKJSON3")
                            {
                                radioButton.SetOptions(new RadioButtonOption { DataField = "ENVCHECK3_" + number, CheckValue = value, UnCheckValue = "" });
                            }

                            //radioButton.SetValue("N");
                        }
                    }
                }
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            InitForm();
            ckEditor.Clear();
            SetSiteStatus();
        }
        /// <summary>
        /// 산업재해 사업장 관리카드 저장
        /// </summary>
        private void SaveCard6(long report_id)
        {
            HcOshaCard6Service service = new HcOshaCard6Service();
            HC_OSHA_CARD6 dto = new HC_OSHA_CARD6();
            service.DeleteByReportid(report_id); //기존의 내용을 삭제
            if (TxtAccName.Text.Trim()!="")
            {
                dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                dto.REPORT_ID = report_id;
                dto.ACC_DATE = DateUtil.DateTimeToStrig(DtpAccDate.Value, DateTimeType.YYYY_MM_DD);
                dto.REMARK = TxtAccRemark.GetValue();
                dto.IND_ACC_TYPE = CboAccType.GetValue();
                dto.NAME = TxtAccName.GetValue();
                dto.ILLNAME = TxtAcIllName.GetValue();
                service.Save(dto);
            }
      
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {

            //   browser.ExecuteScriptAsync("save()");

            //Cef.Shutdown();
            if (base.SelectedSite == null)
            {
                MessageUtil.Alert("사업장을 선택하세요");
            }
            else
            {
                if(base.SelectedEstimate == null)
                {
                    MessageUtil.Alert("계약 내용이 없습니다");
                        return;
                }
                StatusReportEngineerDto dto = PanStatausReport.GetData<StatusReportEngineerDto>();
                PanStatausReport.Validate<StatusReportEngineerDto>();
                string jsonString = JsonConvert.SerializeObject(dto);

                dto.SITE_ID = base.SelectedSite.ID;
                dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                dto.ISDELETED = "N";

                dto.SITENAME = TxtSiteName.Text;
                dto.SITEOWENER = TxtSiteCeoName.Text;
                dto.SITETEL = TxtSiteManagerTel.Text;
                dto.SITEADDRESS = TxtSiteAddress.Text;

                //보건교육 교육종류
                EduTypeJson eduTypeJson = panEduTypeJson.GetData<EduTypeJson>();
                dto.EDUTYPEJSON = JsonConvert.SerializeObject(eduTypeJson);

                //보건교육 교육방법
                EduMethodJson eduMethodJson = panEduMethodJson.GetData<EduMethodJson>();
                dto.EDUMETHODJSON = JsonConvert.SerializeObject(eduMethodJson);

                //작업환경점검1
                ENVCHECKJSON1 eNVCHECKJSON1 = panENVCHECKJSON1.GetData<ENVCHECKJSON1>();
                dto.ENVCHECKJSON1 = JsonConvert.SerializeObject(eNVCHECKJSON1);

                //작업환경점검2
                ENVCHECKJSON2 eNVCHECKJSON2 = panENVCHECKJSON2.GetData<ENVCHECKJSON2>();
                dto.ENVCHECKJSON2 = JsonConvert.SerializeObject(eNVCHECKJSON2);

                //작업환경점검3
                ENVCHECKJSON3 eNVCHECKJSON3 = panENVCHECKJSON3.GetData<ENVCHECKJSON3>();
                dto.ENVCHECKJSON3 = JsonConvert.SerializeObject(eNVCHECKJSON3);
               
               StatusReportEngineerDto saved = this.statusReportEngineerService.Save(dto);

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
                        if (!memo.MEMO.IsNullOrEmpty())
                        {
                            this.statusReportMemoRepository.Update(memo);
                        }

                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }

                SaveCard6(saved.ID);

                ckEditor.SetStatusReportEngineerDto(saved);

                if (browser.CanExecuteJavascriptInMainFrame)
                {
                    browser.ExecuteScriptAsync("save()");
                }

                SearchReport();
            //    InitForm();
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

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            StatusReportEngineerDto dto = PanStatausReport.GetData<StatusReportEngineerDto>();

            if (dto.ID > 0)
            {
                if (MessageUtil.Confirm(dto.ID + "번 상태보고서를 삭제 하시겠습니까?") == DialogResult.Yes)
                {
                    statusReportEngineerService.StatusReportEngineerRepository.Delete(dto.ID);
                   
                    InitForm();

                    SearchReport();

                }

            }

        }


        private void BtnSearchSite_Click(object sender, EventArgs e)
        {
            //SiteListForm form = new SiteListForm();

            //HC_SITE_VIEW siteView = form.Search(TxtSearchSite.Text);
            //if (siteView == null)
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

        /// <summary>
        /// 사업장 선택
        /// </summary>
        /// <param name="siteView"></param>
        public void SetSite()
        {

            InitForm();
         
            ContentTitle.TitleText = "보건관리상태보고서 산업위생기사 - " + base.SelectedSite.NAME;

            SetSiteStatus();
            SearchReport();

            //보건담당자명 자동 표시
            if (TxtSITEMANAGERNAME.Text.Trim() == "")
            {
                TxtSITEMANAGERGRADE.Text = "보건담당자";
                TxtSITEMANAGERNAME.Text = GetLtdBogen(base.SelectedSite.ID);
            }
            //의사 이름 자동 표시
            if (TxtENGINEERNAME.Text.Trim() == "") TxtENGINEERNAME.Text = clsType.User.JobName;

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

        //회사 보건관리자 이름 찾기
        private string GetLtdBogen(long Site_ID)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strName = "";
            string strNow = DateTime.Now.ToString("yyyy-MM-dd");

            SQL = "";
            SQL = "SELECT NAME FROM HIC_OSHA_CONTRACT_MANAGER ";
            SQL = SQL + ComNum.VBLF + "WHERE ESTIMATE_ID IN (SELECT ESTIMATE_ID FROM HIC_OSHA_CONTRACT ";
            SQL = SQL + ComNum.VBLF + "      WHERE OSHA_SITE_ID=" + Site_ID + " ";
            SQL = SQL + ComNum.VBLF + "        AND CONTRACTSTARTDATE<='" + strNow + "' ";
            SQL = SQL + ComNum.VBLF + "        AND CONTRACTENDDATE>='" + strNow + "' ";
            SQL = SQL + ComNum.VBLF + "        AND ISDELETED='N') ";
            SQL = SQL + ComNum.VBLF + "  AND WORKER_ROLE='HEALTH_ROLE' ";
            SQL = SQL + ComNum.VBLF + "  AND ISDELETED='N' ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            strName = "";
            if (dt.Rows.Count > 0) strName = dt.Rows[0]["NAME"].ToString().Trim();
            dt.Dispose();
            dt = null;

            return strName;
        }

        private void SearchReport()
        {

            List<VisitDateModel> list = statusReportEngineerService.StatusReportEngineerRepository.FindAll(base.SelectedSite.ID);
            SSReportList.SetDataSource(list);
            if (SSReportList.RowCount() > 0)
            {
                SSReportList_CellDoubleClick(SSReportList, new CellClickEventArgs(null, 0, 0, 0, 0, MouseButtons.Left, false, false, false));
            }
            
        }
     

        private void SetData(StatusReportEngineerDto dto)
        {
            PanStatausReport.SetData(dto);

            //보건교육 교육종류
            EduTypeJson eduTypeJson = JsonConvert.DeserializeObject<EduTypeJson>(dto.EDUTYPEJSON);
            panEduTypeJson.SetData(eduTypeJson);

            //보건교육 교육방법
            EduMethodJson eduMethodJson = JsonConvert.DeserializeObject<EduMethodJson>(dto.EDUMETHODJSON);
            panEduMethodJson.SetData(eduMethodJson);

            //작업환경점검1
            ENVCHECKJSON1 envCheckJson1 = JsonConvert.DeserializeObject<ENVCHECKJSON1>(dto.ENVCHECKJSON1);
            panENVCHECKJSON1.SetData(envCheckJson1);


            //작업환경점검2
            ENVCHECKJSON2 envCheckJson2 = JsonConvert.DeserializeObject<ENVCHECKJSON2>(dto.ENVCHECKJSON2);
            panENVCHECKJSON2.SetData(envCheckJson2);

            //작업환경점검3
            ENVCHECKJSON3 envCheckJson3 = JsonConvert.DeserializeObject<ENVCHECKJSON3>(dto.ENVCHECKJSON3);
            panENVCHECKJSON3.SetData(envCheckJson3);


            string month = dto.VISITDATE.Substring(4, 2);
            if (month.StartsWith("0"))
            {
                month = month.Substring(1, 1);
            }

            ContentTitle.TitleText = "보건관리상태보고서 산업위생기사 - " + base.SelectedSite.NAME + " " + month + "월 상태보고서";

            if (dto.ID <= 0)
            {
                SetSiteStatus();
            }
            


            ckEditor.SetStatusReportEngineerDto(dto);

            HIC_OSHA_MEMO memo = statusReportMemoRepository.FindOne(dto.SITE_ID);
            if (memo != null)
            {
                TxtMemo.Text = memo.MEMO;
            }
        }
        private void SetSiteStatus()
        {
            if (base.SelectedSite == null)
            {
                return;
            }
            HcSiteViewService hcSiteViewService = new HcSiteViewService();
            
            HC_SITE_VIEW view = hcSiteViewService.FindById(base.SelectedSite.ID);
            TxtSiteName.Text = view.NAME;
            TxtSiteCeoName.Text = view.CEONAME;
            TxtSiteAddress.Text = view.ADDRESS;

            SetEstimateId();
            if (this.SelectedEstimate != null)
            {
                OshaPriceService oshaPriceService = new OshaPriceService();
                OSHA_PRICE dto = oshaPriceService.OshaPriceRepository.FindMaxIdByEstimate(SelectedEstimate.ID);
                if (dto != null)
                {
                    NumWORKERCOUNT.SetValue(dto.WORKERTOTALCOUNT);
                }
            }
            TxtWORKCONTENT.Text = "현장순회점검, 보건관련서류점검, 작업환경관리지도 등";


        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }

   

        private void button3_Click(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }

     
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if(base.SelectedSite != null)
            {
                StatusReportEngineerDto dto = PanStatausReport.GetData<StatusReportEngineerDto>();
                dto = statusReportEngineerService.StatusReportEngineerRepository.FindOne(dto.ID);
                if (dto != null)
                {
                    if(statusReportViewer != null)
                    {
                        if (!statusReportViewer.IsDisposed)
                        {
                            statusReportViewer.Dispose();
                        }
                    }
                    
                    statusReportViewer = new StatusReportViewer("statusReportByEngineer.html", base.SelectedSite.ID);
                    statusReportViewer.PrintStatusReportEngineerDto(dto, ContentTitle.TitleText);
                    statusReportViewer.ShowDialog();

                    
                }                
            }
        }

     

        private void SaveOpinionBtn_Click(object sender, EventArgs e)
        {
            //StatusReportEngineerDto dto = PanStatausReport.GetData<StatusReportEngineerDto>();
            if (MessageUtil.Confirm("저장 하시겠습니까?", this, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                browser.ExecuteScriptAsync("save()");
            }
        }

        public void Select(ISiteModel siteModel)
        {
            if (siteModel.ID > 0)
            {
              //  SetSite(hcSiteViewService.FindById(siteModel.ID));
            }
        }

        private void btnMacro_Click(object sender, EventArgs e)
        {

            MacrowordCkeditorForm form = new MacrowordCkeditorForm("statusReportByEngineerForm", "OPINON", this.browser);
            form.ShowDialog();

            foreach(MacrowordDto dto in form.GetCard19List())
            {
                this.card19List.Add(dto);
            }

            List<MacrowordDto> list = new List<MacrowordDto>();
            list.AddRange(card19List);
            SSCard19.SetDataSource(list);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DtpVisitDate_ValueChanged(object sender, EventArgs e)
        {
            SetEstimateId();
        }

        private void SetEstimateId()
        {
            if(base.SelectedSite != null)
            {
                string dtpVisitDate = DtpVisitDate.GetValue();
                string visitdate = dtpVisitDate.Substring(0, 4) + "-" + dtpVisitDate.Substring(4, 2) + "-" + dtpVisitDate.Substring(6, 2);

                HcEstimateModelRepository repo = new HcEstimateModelRepository();
                HC_OSHA_CONTRACT contract = hcOshaContractRepository.FindByDate(base.SelectedSite.ID, visitdate);
                if (contract == null)
                {
                    //MessageUtil.Alert("방문일자가 계약일자에 해당되지 않습니다.");
                    this.SelectedEstimate = null;
                    return;
                }

                this.SelectedEstimate = repo.FindByEstimateId(contract.ESTIMATE_ID);
            }
      
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void DtpAccDate_ValueChanged(object sender, EventArgs e)
        {
            if (!DtpAccDate.Checked)
            {
                TxtAccName.Text = "";
                TxtAccRemark.Text = "";
                TxtAcIllName.Text = "";
                CboAccType.SetValue ("");
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
                dto.NAME = clsType.User.JobName;
                dto.CERT = "산업위생기사";
                hcOshaCard19Service.Save(dto);

                MessageUtil.Info("위탁업무수행일지 저장하였습니다.");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                MessageUtil.Alert(ex.Message);
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            browser.ShowDevTools();
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

        public void InsertOption(HC_OSHA_EQUIPMENT dto)
        {

            //browser.ExecuteScriptAsync("insertEquipment('"+ dto.NAME + "','" + dto.MODELNAME + "','" + dto.SERIALNUMBER + "')");
            // browser.ExecuteScriptAsync("insertEquipment('" + dto.NAME + "','" + dto.MODELNAME + "','')");
            SetEquipemtnText(dto.NAME, dto.REMARK);

        }
        private void SetEquipemtnText(string name, string reamark)
        {
            if (TxtTmp.Text.IsNullOrEmpty())
            {
                TxtTmp.Text += "\r\n";
            }
            TxtTmp.Text += "장비명:" + name + " , 사용내역:" + reamark + "\r\n";
        }
        private void BtnEquipment_Click(object sender, EventArgs e)
        {
            SiteEquipMentForm form = new SiteEquipMentForm();
            form.SelectedSite = base.SelectedSite;
            form.SelectedEstimate = base.SelectedEstimate;
            form.SetStatusReportByEngineer(this);
            form.Show();
            //HcOshaCard19Service hcOshaCard19Service = new HcOshaCard19Service();
            //List<HC_OSHA_CARD19> list = hcOshaCard19Service.hcOshaCard19Repository.FindAll(base.SelectedSite.ID);
            //SSEquipment.SetDataSource(list);
        }

        private void BtnGetEquip_Click(object sender, EventArgs e)
        {
            HcOshaEquipmentService hcOshaEquipmentService = new HcOshaEquipmentService();
            List<HC_OSHA_EQUIPMENT> list = hcOshaEquipmentService.FindAll(base.SelectedSite.ID);
            foreach(HC_OSHA_EQUIPMENT dto in list)
            {
                SetEquipemtnText(dto.NAME, dto.REMARK);
            }
        }

        private void BtnLast_Click(object sender, EventArgs e)
        {
            if(base.SelectedSite == null)
            {
                return;
            }
            StatusReportEngineerDto dto = statusReportEngineerService.StatusReportEngineerRepository.FindLast(base.SelectedSite.ID);
            if(dto != null)
            {
                dto.ID = 0;
                dto.VISITDATE = DateTime.Now.ToString("yyyyMMdd");
                //dto.SITEMANAGERSIGN = "";
                SetData(dto);
            }
        }

        private void oshaSiteList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            base.SelectedSite = oshaSiteList1.GetSite;
            SetSite();
        }

        private void SSReportList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            VisitDateModel model = SSReportList.GetRowData(e.Row) as VisitDateModel;
            StatusReportEngineerDto dto = this.statusReportEngineerService.StatusReportEngineerRepository.FindOne(model.ID);
            if (dto != null)
            {
                SetData(dto);
            }
            else
            {
               // SetData(new StatusReportEngineerDto()); ;
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (MessageUtil.Confirm("내용을 전부 지우시겠습니까?", this, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                browser.ExecuteScriptAsync("clear()");
            }
        }

        private void BtnManageOshaCard19_Click(object sender, EventArgs e)
        {
            if(base.SelectedSite == null || base.SelectedEstimate == null)
            {
                return;
            }

            CardPage_15_Form form = new CardPage_15_Form();
            form.Show();

            if (base.SelectedSite.ID>0)
            {
                (form as ISelectSite).Select(base.SelectedSite);
            }

            if (base.SelectedEstimate.ID>0)
            {
                (form as ISelectEstimate).Select(base.SelectedEstimate);

            }
        

        }

        private void btnBogen_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = "";
            string strDate = "";
            DataTable dt = new DataTable();

            DtpOSHADATE.SetValue(null);
            if (base.SelectedSite.ID == 0) return;

            string strYear = DateUtil.DateTimeToStrig(DtpVisitDate.Value, DateTimeType.YYYY);

            SQL = "SELECT * FROM HIC_OSHA_CARD7_1 ";
            SQL += ComNum.VBLF + "WHERE SITE_ID = " + base.SelectedSite.ID + " ";
            SQL += ComNum.VBLF + "  AND SWLICENSE = '" + clsType.HosInfo.SwLicense + "' ";
            SQL += ComNum.VBLF + "  AND YEAR = '" + strYear + "' ";
            SQL += ComNum.VBLF + "ORDER BY MEETDATE DESC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                DtpOSHADATE.Text = dt.Rows[0]["MEETDATE"].ToString().Trim();
                TxtOSHACONTENT.Text = dt.Rows[0]["CONTENT"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

        private void label26_Click(object sender, EventArgs e)
        {
            InitRadioButton(GrpEnvCheckJson1, "RdoENVCHECKJSON", "1,2");
        }

        private void label31_Click(object sender, EventArgs e)
        {
            InitRadioButton(GrpEnvCheckJson1, "RdoENVCHECKJSON", "3,4,5,6,7");
        }

        private void label41_Click(object sender, EventArgs e)
        {
            InitRadioButton(GrpEnvCheckJson1, "RdoENVCHECKJSON", "8,9,10,11,12,13,14,15,16");
        }

        private void label69_Click(object sender, EventArgs e)
        {
            InitRadioButton(GrpEnvCheckJson2, "RdoENVCHECKJSON2", "_1,_2,_3,_4,_5,_6,_7,_8,_9,_10");
        }

        private void label88_Click(object sender, EventArgs e)
        {
            InitRadioButton(GrpEnvCheckJson2, "RdoENVCHECKJSON2", "_11,_12,_13,_14,_15,_16,_17");
        }

        private void label137_Click(object sender, EventArgs e)
        {
            InitRadioButton(GrpEnvCheckJson2, "RdoENVCHECKJSON2", "_100,_101,_102,_103,_104,_105,_106,_107");
        }

        private void label82_Click(object sender, EventArgs e)
        {
            InitRadioButton(GrpEnvCheckJson2, "RdoENVCHECKJSON2", "_18,_19,_20,_21,_22,_23,_24,_25");
        }

        private void label85_Click(object sender, EventArgs e)
        {
            InitRadioButton(GrpEnvCheckJson2, "RdoENVCHECKJSON2", "_26,_27,_28,_29,_30,_31,_32,_33,_34,_35,_36,_37,_38,_39");
        }

        private void label107_Click(object sender, EventArgs e)
        {
            InitRadioButton(GrpEnvCheckJson3, "RdoENVCHECKJSON3", "_1,_2,_3,_4,_5,_6,_7,_8,_9,_10,_11,_12");
        }

        private void label122_Click(object sender, EventArgs e)
        {
            InitRadioButton(GrpEnvCheckJson3, "RdoENVCHECKJSON3", "_18,_19,_20,_21,_22,_23,_24,_25,_26");
        }

        private void label127_Click(object sender, EventArgs e)
        {
            InitRadioButton(GrpEnvCheckJson3, "RdoENVCHECKJSON3", "_27,_28,_29,_30,_31,_32,_33,_34");
        }

        // Radio버튼을 선택 안함으로 초기화
        private void InitRadioButton(GroupBox groupBox, string rdoButtonStartName, string strList)
        {
            string strName = "";
            bool bOK = false;

            foreach (Control control in groupBox.Controls)
            {
                if (control is FlowLayoutPanel)
                {
                    foreach (Control rdo in control.Controls)
                    {
                        if (rdo.Name.StartsWith(rdoButtonStartName))
                        {
                            bOK = true;
                            if (strList != "")
                            {
                                strName = rdo.Name.Trim();
                                bOK = false;
                                for (int i = 1; i <= VB.L(strList, ","); i++)
                                {
                                    if (VB.InStr(strName, rdoButtonStartName + VB.Pstr(strList, ",", i) + "_") > 0)
                                    {
                                        bOK = true;
                                        break;
                                    }
                                }
                            }
                            if (bOK == true)
                            {
                                RadioButton radioButton = rdo as RadioButton;
                                if (radioButton.Checked == true) radioButton.Checked = false;
                            }

                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = "";
            string strDate = "";
            DataTable dt = new DataTable();
            string strID = "";
            string strWEMDATE = "";
            string strWEMDATE2 = "";
            string strOSHADATE = "";
            string strTemp = "";
            int intRowAffected = 0;

            SQL = "SELECT ID,WEMDATE,WEMDATE2,OSHADATE ";
            SQL += ComNum.VBLF + " FROM HIC_OSHA_REPORT_ENGINEER ";
            //SQL += ComNum.VBLF + "WHERE WEMDATE IS NOT NULL ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strID = dt.Rows[i]["ID"].ToString().Trim();
                    strTemp = dt.Rows[i]["WEMDATE"].ToString().Trim();
                    strWEMDATE = strTemp;
                    if (VB.Len(strTemp) == 8)
                    {
                        strWEMDATE = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp,5, 2) + "-" + VB.Right(strTemp, 2);
                    }
                    strTemp = dt.Rows[i]["WEMDATE2"].ToString().Trim();
                    strWEMDATE2 = strTemp;
                    if (VB.Len(strTemp) == 8)
                    {
                        strWEMDATE2 = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp,5, 2) + "-" + VB.Right(strTemp, 2);
                    }
                    strTemp = dt.Rows[i]["OSHADATE"].ToString().Trim();
                    strOSHADATE = strTemp;
                    if (VB.Len(strTemp) == 8)
                    {
                        strOSHADATE = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp,5, 2) + "-" + VB.Right(strTemp, 2);
                    }

                    SQL = "UPDATE HIC_OSHA_REPORT_ENGINEER SET ";
                    SQL += ComNum.VBLF + " WEMDATE='" + strWEMDATE + "', ";
                    SQL += ComNum.VBLF + " WEMDATE2='" + strWEMDATE2 + "', ";
                    SQL += ComNum.VBLF + " OSHADATE='" + strOSHADATE + "'  ";
                    SQL += ComNum.VBLF + " WHERE ID=" + strID + " ";
                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                }
            }
            dt.Dispose();
            dt = null;

            MessageUtil.Info("작업 완료");
        }

        private void btnDB재접속_Click(object sender, EventArgs e)
        {
            clsDB.DisDBConnect(clsDB.DbCon);
            clsDB.DbCon = clsDB.DBConnect_Cloud();
            ComFunc.MsgBox("DB 재접속 완료", "알림");
        }
    }
}
