using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using FarPoint.Win.Spread;
using HC.Core.Common.Interface;
using HC.Core.Common.Util;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC.OSHA.Service;
using HC_Core;
using HC_Core.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HC_OSHA
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SiteManageForm : CommonForm , ISelectSite, ISelectEstimate
    {
        private HcOshaSiteService hcOShaSiteService;
        private HcOshaEstimateService hcOshaEstimateService;
        private HcOshaContractService hcOshaContractService;

        private OshaEstimateModelService oshaEstimateModelService;

        private HcOshaContractManagerService hcOshaContractManagerService;
        private OshaPriceService oshaPriceService;
        private HcSiteWorkerService hcSiteWorkerService;
        HcUserService hcUsersService;
        HC_CODE estimateExcelPath;
        HC_CODE sendMailAddress;

        List<HC_USER> doctorList;
        List<HC_USER> nurseList;
        List<HC_USER> engineerList;
        string excelSetValue = "";
        //SitePriceManageForm sitePriceManageForm;
        //SitePriceManageForm sitePriceManageForm2; //원청현황
        public SiteManageForm()
        {
            InitializeComponent();
            hcOShaSiteService = new HcOshaSiteService();
            hcOshaEstimateService = new HcOshaEstimateService();
            oshaEstimateModelService = new OshaEstimateModelService();
            hcOshaContractService = new HcOshaContractService();
            hcOshaContractManagerService = new HcOshaContractManagerService();
            oshaPriceService = new OshaPriceService();
            hcSiteWorkerService = new HcSiteWorkerService();
            hcUsersService = new HcUserService();
            //sitePriceManageForm = new SitePriceManageForm();
            //sitePriceManageForm2 = new SitePriceManageForm(true);
        }
        private void SiteMangerForm_Load(object sender, EventArgs e)
        {
            // 관계사 직원
            if (clsType.User.LtdUser != "")
            {
                BtnLastContract.Enabled = false;
                BtnSaveContract.Enabled = false;
                BtnDeleteContract.Enabled = false;
                BtnDeleteEstimate.Enabled = false;
                BtnSaveEstimate.Enabled = false;
                BtnSavePrice.Enabled = false;
                BtnDeletePrice.Enabled = false;
                BtnNewPrice.Enabled = false;
                btnNewEstimate.Enabled = false;
            }
            //sitePriceManageForm.Dock = DockStyle.Fill;
            //sitePriceManageForm.FormBorderStyle = FormBorderStyle.None;
            //sitePriceManageForm.TopLevel = false;
            //sitePriceManageForm.Show();
            //tabPage5.Controls.Add(sitePriceManageForm);

            //sitePriceManageForm2.Dock = DockStyle.Fill;
            //sitePriceManageForm2.FormBorderStyle = FormBorderStyle.None;
            //sitePriceManageForm2.TopLevel = false;
            //sitePriceManageForm2.Show();
            //tabPage4.Controls.Add(sitePriceManageForm2);

            SpreadComboBoxData comboBoxData = codeService.GetSpreadComboBoxData("WORKER_ROLE", "OSHA");

            SSWorkerList.Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderHeight = 25 });
            SSWorkerList.AddColumnText("이름", nameof(HC_OSHA_CONTRACT_MANAGER_MODEL.NAME), 75, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SSWorkerList.AddColumnText("소속", nameof(HC_OSHA_CONTRACT_MANAGER_MODEL.DEPT), 117, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnComboBox("직책", nameof(HC_OSHA_CONTRACT_MANAGER_MODEL.WORKER_ROLE), 140, IsReadOnly.N, comboBoxData, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("전화", nameof(HC_OSHA_CONTRACT_MANAGER_MODEL.TEL), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("휴대폰", nameof(HC_OSHA_CONTRACT_MANAGER_MODEL.HP), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("이메일", nameof(HC_OSHA_CONTRACT_MANAGER_MODEL.EMAIL), 168, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("메일전송", nameof(HC_OSHA_CONTRACT_MANAGER_MODEL.EMAILSEND), 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnButton("삭제", 50, new SpreadCellTypeOption { IsSort = false, ButtonText = "삭제" }).ButtonClick += SSSWorkerList_DeleteButtonClick;

            SSWorkerList.SetDataSource(new List<HC_OSHA_CONTRACT_MANAGER_MODEL>());

            SetEstimateBiding();
            SetContractBiding();
            SetPriceBiding();

            sendMailAddress = codeService.FindActiveCodeByGroupAndCode("OSHA_MANAGER", "mail", "OSHA");

        }

        private void SSSWorkerList_DeleteButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            SSWorkerList.DeleteRow(e.Row);
        }
        /// <summary>
        /// 견적 입력폼 
        /// 견적서 서식 갱신시 엑셀 파일명을 기초코드에 세팅하면 새로운 견적서를 적용가능함
        /// </summary>
        private void SetEstimateBiding()
        {
            DtpESTIMATEDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_ESTIMATE.ESTIMATEDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpSTARTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_ESTIMATE.STARTDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            NumOFFICIALFEE.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_ESTIMATE.OFFICIALFEE),  Min = 0, });
            NumSITEFEE.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_ESTIMATE.SITEFEE), Min = 0 });
            NumMONTHLYFEE.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_ESTIMATE.MONTHLYFEE), Min = 0 });
            NumWORKERTOTALCOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_ESTIMATE.WORKERTOTALCOUNT), Min = 0,});
            //TxtFEETYPE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_ESTIMATE.FEETYPE) });
            TxtRemark.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_ESTIMATE.REMARK) });
            TxtSENDMAILDATE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_ESTIMATE.SENDMAILDATE) });
            TxtPRINTDATE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_ESTIMATE.PRINTDATE)});

            //엑셀 견적서 서식을 읽어 시트에 셋팅
            //estimateExcelPath = codeService.FindActiveCodeByGroupAndCode("EXCEL_PATH", "OSHA_ESTIMATE", "OSHA");
            HC_OSHA_ESTIMATE dto = new HC_OSHA_ESTIMATE();
            string fileName = @"C:\HealthSoft\엑셀서식\견적서.xlsx";
            SSEstimate.ActiveSheet.OpenExcel(fileName, 0);

            string str = "";
            int nMaxRows = 0;
            int nMaxCols = 0;

            //RowCount 설정
            for (int i=100;i>5;i--)
            {
                str = "";
                for (int j=0;j<20;j++)
                {
                    str += SSEstimate.ActiveSheet.Cells[i, j].Text.Trim();
                }
                if (str != "")
                {
                    nMaxRows = i + 1;
                    break;
                }
            }

            //ColumnCount 설정
            for (int i = 100; i > 5; i--)
            {
                str = "";
                for (int j = 0; j < nMaxRows; j++)
                {
                    str += SSEstimate.ActiveSheet.Cells[j, i].Text.Trim();
                }
                if (str != "")
                {
                    nMaxCols = i + 1;
                    break;
                }
            }

            //구하지 못하였으면 오류
            if (nMaxRows == 0 || nMaxCols == 0) return;

            SSEstimate.ActiveSheet.RowCount = nMaxRows;
            SSEstimate.ActiveSheet.ColumnCount = nMaxCols;
            SSEstimate.ActiveSheet.ColumnHeader.RowCount = 0;
            SSEstimate.ActiveSheet.RowHeader.ColumnCount = 0;

            //값을 대체할 위치를 찾음
            excelSetValue = "";
            for (int i=0;i< SSEstimate.ActiveSheet.RowCount;i++)
            {
                for (int j=0;j< SSEstimate.ActiveSheet.ColumnCount;j++)
                {
                    str = SSEstimate.ActiveSheet.Cells[i, j].Text.Trim();
                    if (VB.Left(str,1)=="~")
                    {
                        excelSetValue += str + ";" + i + ";" + j + "{}";
                    }
                }
            }

            dto.ESTIMATEDATE = DateUtil.TodayAsYYYY_MM_DD();

            PanEstimate.SetData(dto);
            ExcelDataSet(dto);

        }

        private void SetContractBiding()
        {
            //계약 및 근무일
            DtpCONTRACTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.CONTRACTDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpTERMINATEDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.TERMINATEDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpCONTRACTSTARTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.CONTRACTSTARTDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpCONTRACTENDDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.CONTRACTENDDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpDECLAREDAY.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.DECLAREDAY), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            TxtVISITDAY.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.VISITDAY) });
            TxtVISITWEEK.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.VISITWEEK) });
            NumCOMMISSION.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.COMMISSION), Min = 0 });
            TxtSPECIALCONTRACT.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.SPECIALCONTRACT) });

            //위치 
            RdoPosition_0.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CONTRACT.POSITION), CheckValue = "0" });
            RdoPosition_1.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CONTRACT.POSITION), CheckValue = "1" });
            RdoPosition_2.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CONTRACT.POSITION), CheckValue = "2" });
            RdoPosition_3.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CONTRACT.POSITION), CheckValue = "3" });
            RdoBUILDINGTYPE_0.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CONTRACT.BUILDINGTYPE), CheckValue = "0" });
            RdoBUILDINGTYPE_1.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CONTRACT.BUILDINGTYPE), CheckValue = "1" });
            RdoISROTATION_0.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CONTRACT.ISROTATION), CheckValue = "0" });
            RdoISROTATION_1.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CONTRACT.ISROTATION), CheckValue = "1" });
            RdoISPRODUCTTYPE_0.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CONTRACT.ISPRODUCTTYPE), CheckValue = "0" });
            RdoISPRODUCTTYPE_1.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CONTRACT.ISPRODUCTTYPE), CheckValue = "1" });

            // 근로자수
            NumWORKERWHITEMALECOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.WORKERWHITEMALECOUNT), Min=0 });
            NumWORKERWHITEFEMALECOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.WORKERWHITEFEMALECOUNT), Min = 0 });
            NumWORKERBLUEMALECOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.WORKERBLUEMALECOUNT), Min = 0 } );
            NumWORKERBLUEFEMALECOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.WORKERBLUEFEMALECOUNT), Min = 0 });
            NumContractWORKERTOTALCOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.WORKERTOTALCOUNT), Min = 0 });

            //담당요원및방문주기
            engineerList = hcUsersService.GetEngineerByOsha();
            doctorList = hcUsersService.GetDoctors();
            nurseList = hcUsersService.GetNurse();
            CboManageDoctor.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGEDOCTOR) });
            CboManageDoctor.SetItems(doctorList, "Name", "UserId");
            CboManageNurse.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGENURSE), });
            CboManageNurse.SetItems(nurseList, "Name", "UserId");
            CboMANAGEENGINEER.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGEENGINEER), });
            CboMANAGEENGINEER.SetItems(engineerList, "Name", "UserId");
            DtpMANAGEDOCTORSTARTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGEDOCTORSTARTDATE), DataBaseFormat = DateTimeType.YYYYMM, DisplayFormat = DateTimeType.YYYY_MM });
            DtpMANAGENURSESTARTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGENURSESTARTDATE), DataBaseFormat = DateTimeType.YYYYMM, DisplayFormat = DateTimeType.YYYY_MM });
            DtpMANAGEENGINEERSTARTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGEENGINEERSTARTDATE), DataBaseFormat = DateTimeType.YYYYMM, DisplayFormat = DateTimeType.YYYY_MM });

            NumMANAGEDOCTORCOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGEDOCTORCOUNT), Min = 0 });
            NumMANAGENURSECOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGENURSECOUNT), Min = 0 });
            NumMANAGEENGINEERCOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGEENGINEERCOUNT), Min = 0 });
            NumMANAGEWORKERCOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGEWORKERCOUNT), Min = 0 });
            TxtVISITPLACE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.VISITPLACE) });

            //사업장업무일정
            TxtWORKROTATIONTIME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.WORKROTATIONTIME) });
            TxtWORKLUANCHTIME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.WORKLUANCHTIME) });
            TxtWORKRESTTIME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.WORKRESTTIME) });
            TxtWORKEDUTIME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.WORKEDUTIME) });
            TxtWORKETCTIME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.WORKETCTIME) });

            //추가항목
            ChkISWEM.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISWEM), CheckValue = "Y", UnCheckValue = "N" });
            TxtISWEMDATA.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISWEMDATA) });

            ChkISCOMMITTEE.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISCOMMITTEE), CheckValue = "Y", UnCheckValue = "N" });
            ChkISSKELETON.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISSKELETON), CheckValue = "Y", UnCheckValue = "N" });
            TxtISSKELETONDATE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISSKELETONDATE) });

            ChkISSPECIAL.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISSPACEPROGRAM), CheckValue = "Y", UnCheckValue = "N" });
            TxtISSPACEPROGRAMDATE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISSPACEPROGRAMDATE) });

            ChkISEARPROGRAM.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISEARPROGRAM), CheckValue = "Y", UnCheckValue = "N" });
            TxtISEARPROGRAMDATE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISEARPROGRAMDATE) });

            ChkISBRAINTEST.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISBRAINTEST), CheckValue = "Y", UnCheckValue = "N" });
            TxtISBRAINTESTDATE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISBRAINTESTDATE) });

            ChkISSPACEPROGRAM.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISSPACEPROGRAM), CheckValue = "Y", UnCheckValue = "N" });
            TxtISSPACEPROGRAMDATE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISSPACEPROGRAMDATE) });

            ChkISSTRESS.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISSTRESS), CheckValue = "Y", UnCheckValue = "N" });
            TxtISSTRESSDATE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISSTRESSDATE) });

            ChkISSPECIAL.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISSPECIAL), CheckValue = "Y", UnCheckValue = "N" });
            TxtISSPECIALDATA.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISSPECIALDATA) });

            PanContract.SetData(new HC_OSHA_CONTRACT());
        }

        private void BtnSiteConfirm_Click(object sender, EventArgs e)
        {
            SiteListForm form = new SiteListForm();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                HC_SITE_VIEW view = form.SelectedSite;
                if (hcOShaSiteService.Save(view))
                {
                    HC_OSHA_SITE_MODEL model = hcOShaSiteService.FindById(view.ID);
                    SetSiteInfo(model);
                }
                else
                {
                    MessageUtil.Alert("이미 등록된 사업장입니다");
                }
            }
        }

        private void SetSiteInfo(HC_OSHA_SITE_MODEL model)
        {
            TxtOshaSiteId.Text = model.ID.ToString();
            TxtSiteName.Text = model.NAME;
            lblSitename.Text = model.NAME;
            List<HC_SITE_WORKER> list = hcSiteWorkerService.hcSiteWorkerRepository.FindWorkerByRole(model.ID, "HEALTH_ROLE");
            if (list.Count > 0)
            {
                txtHalthManager.Text = list[0].NAME;
                txtReceiveEmail.Text = list[0].EMAIL;

            }
            else
            {
                txtHalthManager.Text = string.Empty;
                txtReceiveEmail.Text = string.Empty;

            }
        }
        private void btnNewEstimate_Click(object sender, EventArgs e)
        {
            InitForm();
        }
        private void BtnDeleteEstimate_Click(object sender, EventArgs e)
        {
            HC_OSHA_ESTIMATE dto = PanEstimate.GetData<HC_OSHA_ESTIMATE>();
            if (dto.ID > 0){

                DialogResult result = MessageUtil.Confirm(dto.ESTIMATEDATE +" 견적서를 삭제하시겠습니까?");
                if (result == DialogResult.Yes)
                {

                    hcOshaEstimateService.Delete(dto);
                    PanEstimate.Initialize();
                    OshaSiteEstimateList.SearhAndDoubleClik(oshaSiteLastTree.GetSite.ID, true);
                }
            }
        }
        private void BtnEstimateSave_Click(object sender, EventArgs e)
        {
            if (SelectedSite == null)
            {
                MessageUtil.Alert("사업장을 선택하세요");
                return;
            }
         
            if (PanEstimate.Validate<HC_OSHA_ESTIMATE>())
            {

                HC_OSHA_ESTIMATE dto = PanEstimate.GetData<HC_OSHA_ESTIMATE>();
            
                dto.OSHA_SITE_ID = SelectedSite.ID;
                dto.EXCELPATH = @"C:\HealthSoft\엑셀서식\견적서.xlsx";

                if (dto.OSHA_SITE_ID == 0)
                {
                    MessageUtil.Alert("사업장을 선택하세요");
                }
                else
                {
                    
                    if(sender == null)
                    {
                        dto.ESTIMATEDATE = DtpCONTRACTDATE.GetValue();
                    }
                    dto = hcOshaEstimateService.Save(dto);
                    PanEstimate.SetData(dto);
                    MessageUtil.Info("견적을 저장하였습니다");
                    ExcelDataSet(dto); //견적서

                    //원청사업장
                    if (TxtParentSiteName.Text.NotEmpty())
                    {
                        try
                        {
                            clsDB.setBeginTran(clsDB.DbCon);


                            bool isPossible = true;
                            HC_OSHA_SITE self = hcOShaSiteService.HcOshaSiteRepository.FindById(dto.OSHA_SITE_ID);
                            if (self.HASCHILD == "Y")
                            {
                                MessageUtil.Alert(SelectedSite.NAME + " 사업장은 원청 사업장으로 등록되어 있습니다 " + TxtParentSiteName.Text + "의 하청사업장이 될 수 없습니다");
                                isPossible = false;

                            }
                            HC_OSHA_SITE parent = hcOShaSiteService.HcOshaSiteRepository.FindById(TxtParentSiteId.Text.To<long>(0));
                            if (parent.PARENTSITE_ID > 0)
                            {
                                MessageUtil.Alert("등록하려는 " + TxtParentSiteName.Text + " 원청 사업장은 하청사업장으로 등록되어 있습니다. 하청으로 등록할 수 없습니다");
                                isPossible = false;

                            }

                            if (isPossible)
                            {
                                hcOShaSiteService.HcOshaSiteRepository.UpdateParentSite(dto.OSHA_SITE_ID, TxtParentSiteId.Text.To<long>(0));
                                oshaSiteLastTree.Search();
                            }


                            clsDB.setCommitTran(clsDB.DbCon);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                            MessageUtil.Alert(ex.Message);
                            clsDB.setRollbackTran(clsDB.DbCon);
                        }
                    }

                    if (sender != null)
                    {
                        OshaSiteEstimateList.SearhAndDoubleClik(oshaSiteLastTree.GetSite.ID, true);
                    }

                    // 사업장현황에 견적내용을 표시
                    if (NumContractWORKERTOTALCOUNT.Value == 0)
                    {
                        NumCOMMISSION.Value = NumSITEFEE.Value;
                        NumWORKERWHITEMALECOUNT.Value = long.Parse(TxtWhiteMale.Text.Trim());
                        NumWORKERWHITEFEMALECOUNT.Value = long.Parse(TxtWhiteFeMale.Text.Trim());
                        NumWORKERBLUEMALECOUNT.Value = long.Parse(TxtBlueMale.Text.Trim());
                        NumWORKERBLUEFEMALECOUNT.Value = long.Parse(TxtBlueFeMale.Text.Trim());
                        NumContractWORKERTOTALCOUNT.Value = NumWORKERTOTALCOUNT.Value;
                        NumMANAGEWORKERCOUNT.Value = NumWORKERTOTALCOUNT.Value;
                    }
                }
            }
        }

        private void BtnSaveContract_Click(object sender, EventArgs e)
        {

            if (SelectedSite == null)
            {
                MessageUtil.Alert("사업장을 선택하세요");
            }
            else
            {
            
                //if(RdoIsContract_Y.Checked == false && RdoIsContract_N.Checked == false)
                //{
                //    MessageUtil.Alert("계약 또는 계약해지를 선택해주세요");
                //    return;
                //}
                //if (RdoIsContract_N.Checked)
                //{
                //    if (DtpTERMINATEDATE.GetValue().IsNullOrEmpty())
                //    {
                //        MessageUtil.Alert("계약해지일을 선택해주세요");
                //        return;
                //    }
                //}

                //if (RdoIsContract_Y.Checked)
                //{
                //    if (DtpCONTRACTDATE.GetValue().IsNullOrEmpty())
                //    {
                //        MessageUtil.Alert("계약일을 선택해주세요");
                //        return;
                //    }
                //}
                if (SelectedEstimate == null)
                {
                     MessageUtil.Alert("견적 정보가 없습니다 견적을 먼저 저장하세요");
                     return;
                }

                ManageDoctorCount_AutoSet(); //방문주기 자동설정

                if (PanContract.Validate<HC_OSHA_CONTRACT>())
                {
                    HC_OSHA_CONTRACT contract = PanContract.GetData<HC_OSHA_CONTRACT>();
                    HC_OSHA_ESTIMATE estimate = PanEstimate.GetData<HC_OSHA_ESTIMATE> () ;
                    contract.ESTIMATE_ID = estimate.ID;
                    contract.OSHA_SITE_ID = SelectedSite.ID;

                    hcOshaContractService.Save(contract);

                    OshaSiteEstimateList.Searh(oshaSiteLastTree.GetSite.ID, false);

                    IList<HC_OSHA_CONTRACT_MANAGER_MODEL> list = SSWorkerList.GetEditbleData<HC_OSHA_CONTRACT_MANAGER_MODEL>();
                    if (list != null)
                    {
                        hcOshaContractManagerService.Save(list);

                        GetContractManager();

                    }
                    MessageUtil.Info("저장하였습니다.");
                }
            }

        }
        private void BtnDeleteContract_Click(object sender, EventArgs e)
        {
            HC_OSHA_CONTRACT contract = PanContract.GetData<HC_OSHA_CONTRACT>();

            if (contract.ESTIMATE_ID > 0)
            {
                if (MessageUtil.Confirm("삭제하시겠습니까") == DialogResult.Yes)
                {
                    if (hcOshaContractService.Delete(contract.ESTIMATE_ID))
                    {
                        MessageUtil.Info("삭제하였습니다");
                    }


                    PanContract.SetData(new HC_OSHA_CONTRACT());

                    OshaSiteEstimateList.SearhAndDoubleClik(oshaSiteLastTree.GetSite.ID, true);
                }
            }
          
        }

        private void GetContractManager()
        {
           IList workerList = hcOshaContractManagerService.FindContractManager(base.SelectedEstimate.ID);
            SSWorkerList.SetDataSource(workerList);
        }


        private void BtnLastContract_Click(object sender, EventArgs e)
        {
            if (SelectedSite == null)
            {
                MessageUtil.Alert("사업장을 선택하세요");
                return;
            }

            if (SelectedEstimate == null)
            {
                
                MessageUtil.Alert("견적 정보가 없습니다");
                return;
            }

            if (OshaSiteEstimateList.GetRowCount() == 1)
            {
                MessageUtil.Alert("새로운 견적 정보를 생성하세요!");
                return;
            }


            if(hcOshaContractService.Copy(SelectedSite.ID, SelectedEstimate.ID))
            {
                MessageUtil.Alert("복사 하였습니다");
            }
            else
            {
                MessageUtil.Alert("복사 할 수 업습니다.");
            }
             //PanContract.SetData(new HC_OSHA_CONTRACT());
            
            //PanContract.Initialize();
            
        }

        private void BtnWorkerAdd_Click(object sender, EventArgs e)
        {
            SiteWorkerPopupForm form = new SiteWorkerPopupForm();
            form.SelectedSite = base.SelectedSite;
            form.ShowDialog();

            //      List<HC_SITE_WORKER> list = form.GetWorker();
            //        SSWorkerList.SetDataSource(list);


            List<HC_OSHA_CONTRACT_MANAGER_MODEL> workerList2 = SSWorkerList.DataSource as List<HC_OSHA_CONTRACT_MANAGER_MODEL>;

            if (workerList2 == null)
            {
                workerList2 = new List<HC_OSHA_CONTRACT_MANAGER_MODEL>();
            }
            List<HC_OSHA_CONTRACT_MANAGER_MODEL>  workerList = new List<HC_OSHA_CONTRACT_MANAGER_MODEL>();
            //선택된 근로자
            foreach (HC_SITE_WORKER worker in form.GetWorker())
            {
                HC_OSHA_CONTRACT_MANAGER_MODEL model = new HC_OSHA_CONTRACT_MANAGER_MODEL();
                model.ESTIMATE_ID = base.SelectedEstimate.ID;
              //  model.WORKER_ID = worker.ID;
                model.WORKER_ROLE = worker.WORKER_ROLE;
                model.NAME = worker.NAME;
                model.DEPT = worker.DEPT;
                model.TEL = worker.TEL;
                model.HP = worker.HP;
                model.EMAIL = worker.EMAIL;
                model.RowStatus = RowStatus.Insert;
               
                workerList.Add(model);
            }
            foreach(HC_OSHA_CONTRACT_MANAGER_MODEL model in workerList2)
            {
                workerList.Add(model);
            }
            //등록된 근로자 + 선택된 근로자 
            SSWorkerList.SetDataSource(workerList);
            //사업장 근로자 데이터 가졍오기
            //    IList list = hcOshaContractManagerService.FindSiteWorker(base.SelectedSite.ID, base.SelectedEstimate.ID);



        }

        private void BtnSendMailEstimate_Click(object sender, EventArgs e)
        {
            //if(sendMailAddress== null)
            //{
            //    MessageUtil.Alert("메일 발송 주소 기초코드가 없습니다");
            //    return;
            //}
            //if (txtReceiveEmail.Text.Length <= 0)
            //{
            //    MessageUtil.Alert("받는 사람 메일 발송 주소가 없습니다");
            //    return;
            //}
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                HC_OSHA_ESTIMATE dto = PanEstimate.GetData<HC_OSHA_ESTIMATE>();
                if (dto.ID > 0)
                {
                    HC_CODE pdfPath = codeService.FindActiveCodeByGroupAndCode("PDF_PATH", "OSHA_ESTIMATE", "OSHA");
                    SpreadPrint print = new SpreadPrint(SSEstimate, PrintStyle.STANDARD_APPROVAL, false);

                    string pdfFileName = pdfPath.CodeName + "\\" + SelectedSite.NAME+"_" + DtpESTIMATEDATE.GetValue() + dto.ID + ".pdf";
                    print.ExportPDFNoWait(pdfFileName, SSEstimate.ActiveSheet);

                    EstimateMailForm form = new EstimateMailForm();
                    form.SelectedSite = base.SelectedSite;
                    form.GetMailForm().SenderMailAddress = sendMailAddress.CodeName;

                    string[] receiver = txtReceiveEmail.Text.Split(',');
                    foreach (string email in receiver)
                    {
                        form.GetMailForm().ReciverMailSddress.Add(email);
                    }

                    form.GetMailForm().AttachmentsList.Add(pdfFileName);
                    DialogResult result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        dto.SENDMAILDATE = DateTime.Now;
                        dto = hcOshaEstimateService.hcOshaEstimateRepository.UpdateSendMail(dto);
                        PanEstimate.SetData(dto);
                    }

                }
                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex)
            {
                MessageUtil.Alert(ex.Message);
                Cursor.Current = Cursors.Default;
            }
          

        }

        private void BtnLoadExcel_Click(object sender, EventArgs e)
        {
        }

        private void ExcelDataSet(HC_OSHA_ESTIMATE dto)
        {
            long nCnt = 0;
            string strData = "";
            string strGubun = "";
            int nRow = 0;
            int nCol = 0;

            if (dto.ID <= 0) return;
            if (excelSetValue == "") return;

            nCnt = VB.L(excelSetValue, "{}");
            for (int i=1;i<nCnt;i++)
            {
                strData = VB.Pstr(excelSetValue, "{}", i);
                strGubun = VB.Pstr(strData, ";", 1);
                nRow = Int32.Parse(VB.Pstr(strData, ";", 2));
                nCol = Int32.Parse(VB.Pstr(strData, ";", 3));

                if (strGubun== "~PublishDate") SSEstimate.ActiveSheet.Cells[nRow, nCol].Value = DateUtil.ToKorean(dto.ESTIMATEDATE, DateTimeType.YYYY_MM_DD);
                if (strGubun == "~SiteName") SSEstimate.ActiveSheet.Cells[nRow, nCol].Value = SelectedSite.NAME;
                if (strGubun == "~StartDate") SSEstimate.ActiveSheet.Cells[nRow, nCol].Value = DateUtil.ToKorean(dto.STARTDATE, DateTimeType.YYYY_MM_DD) + " 부터";
                if (strGubun == "~Title") SSEstimate.ActiveSheet.Cells[nRow, nCol].Value = dto.STARTDATE.Substring(0, 4) + "년 보건관리업무 수수료 견적";
                if (strGubun == "~FEE_NOTICE") SSEstimate.ActiveSheet.Cells[nRow, nCol].Value = NumberUtil.Comma(dto.OFFICIALFEE) + "원";
                if (strGubun == "~SITEFEE") SSEstimate.ActiveSheet.Cells[nRow, nCol].Value = NumberUtil.Comma(dto.SITEFEE) + "원";
                if (strGubun == "~WorkerCount") SSEstimate.ActiveSheet.Cells[nRow, nCol].Value = NumberUtil.Comma(dto.WORKERTOTALCOUNT) + "명";
                if (strGubun == "~CalText") SSEstimate.ActiveSheet.Cells[nRow, nCol].Value = NumberUtil.Comma(dto.WORKERTOTALCOUNT) + "명" + " * " + NumberUtil.Comma(dto.SITEFEE) + "원";
                if (strGubun == "~TotalFee") SSEstimate.ActiveSheet.Cells[nRow, nCol].Value = NumberUtil.Comma(dto.SITEFEE * dto.WORKERTOTALCOUNT) + "원";
                if (strGubun == "~Year") SSEstimate.ActiveSheet.Cells[nRow, nCol].Value = new DateTime().Year + "년";
                if (strGubun == "~Today") SSEstimate.ActiveSheet.Cells[nRow, nCol].Value = DateUtil.ToKorean();
                if (strGubun == "~BlueMale") SSEstimate.ActiveSheet.Cells[nRow, nCol].Value = dto.BLUEMALE;
                if (strGubun == "~BlueFeMale") SSEstimate.ActiveSheet.Cells[nRow, nCol].Value = dto.BLUEFEMALE;
                if (strGubun == "~WhiteMale") SSEstimate.ActiveSheet.Cells[nRow, nCol].Value = dto.WHITEMALE;
                if (strGubun == "~WhiteFeMale") SSEstimate.ActiveSheet.Cells[nRow, nCol].Value = dto.WHITEFEMALE;
            }
        }

        private void BtnPrintEstimate_Click(object sender, EventArgs e)
        {
            HC_OSHA_ESTIMATE dto = PanEstimate.GetData<HC_OSHA_ESTIMATE>();
            if (dto.ID > 0)
            {
                SpreadPrint print = new SpreadPrint(SSEstimate, PrintStyle.STANDARD_APPROVAL, true);
           //     print.Title = "견적서";
                print.Execute();
             
                dto = hcOshaEstimateService.hcOshaEstimateRepository.UpdatePRINTDATE(dto);

                PanEstimate.SetData(dto);
            }

        }

        private void BtnSearchParent_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite != null)
            {
                SiteListForm form = new SiteListForm();
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (form.SelectedSite.ID == base.SelectedSite.ID)
                    {
                        MessageUtil.Alert("자신의 사업장을 원청 사업장으로 등록할 수 없습니다.");
                        return;
                    }
                    HC_SITE_VIEW view = form.SelectedSite;

                    TxtParentSiteId.Text = view.ID.ToString();
                    TxtParentSiteName.Text = view.NAME;
                }
            }
    
        }

        private void OshaSiteLastTree_NodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            InitForm();
            base.SelectedSite = oshaSiteLastTree.GetSite;
            if(this.Parent.Parent.Parent is Dashboard)
            {
                (this.Parent.Parent.Parent as Dashboard).SelectedSite = base.SelectedSite;
            }
            SetSiteInfo(oshaSiteLastTree.GetSite);
            OshaSiteEstimateList.SearhAndDoubleClik(oshaSiteLastTree.GetSite.ID, true);
     
        }

        private void OshaSiteEstimateList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            long nSiteFee = 0;

            Application.DoEvents();
            base.SelectedEstimate = OshaSiteEstimateList.GetEstimateModel;
            if (OshaSiteEstimateList.GetEstimateModel != null)
            {

                HC_OSHA_ESTIMATE dto = hcOshaEstimateService.FindById(base.SelectedEstimate.ID);
                nSiteFee = dto.SITEFEE;
                PanEstimate.SetData(dto);
                NumSITEFEE.Value = nSiteFee;

                //원청
                HC_OSHA_SITE_MODEL parent = hcOShaSiteService.FindById(dto.OSHA_SITE_ID);
                if (parent != null)
                {
                    if (parent.PARENTSITE_NAME.NotEmpty())
                    {
                        TxtParentSiteId.Text = parent.PARENTSITE_ID.ToString();
                        TxtParentSiteName.Text = parent.PARENTSITE_NAME;

                    }
                }

                //견적서
                if (dto.ID > 0) ExcelDataSet(dto);

                //계약정보
                HC_OSHA_CONTRACT contract = hcOshaContractService.FindByEstimateId(dto.ID);

                if (contract != null)
                {

                    SetCombo(contract.MANAGEDOCTOR);
                    SetCombo(contract.MANAGENURSE);
                    SetCombo(contract.MANAGEENGINEER);
                    PanContract.SetData(contract);
                    DtpDECLAREDAY.SetValue(contract.DECLAREDAY);
                    GetContractManager();
                }
                else
                {
                    PanContract.Initialize();
                }

                SearchPrice();
            }
            else
            {
                InitForm();
            }
        }
        private void SetCombo(string userId)
        {
            bool isDeleteUser = true;
            foreach(HC_USER user in doctorList)
            {
                if (user.UserId == userId)
                {
                    isDeleteUser = false;
                }
            }
            foreach (HC_USER user in nurseList)
            {
                if (user.UserId == userId)
                {
                    isDeleteUser = false;
                }
            }
            foreach (HC_USER user in engineerList)
            {
                if (user.UserId == userId)
                {
                    isDeleteUser = false;
                }
            }
            if (isDeleteUser)
            {
                HC_USER user =  hcUsersService.FindByUserId(userId);
                if (user == null)
                {
                    return;
                }
                Role r = (Role)Enum.Parse(typeof(Role), user.Role);
                if (r == Role.DOCTOR)
                {
                    doctorList.Add(user);
                    CboManageDoctor.Clear();
                    CboManageDoctor.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGEDOCTOR) });
                    CboManageDoctor.SetItems(doctorList, "Name", "UserId");
                }
                else if (r == Role.NURSE)
                {
                    nurseList.Add(user);
                    CboManageNurse.Clear();
                    CboManageNurse.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGENURSE), });
                    CboManageNurse.SetItems(nurseList, "Name", "UserId");
                }
                else if (r == Role.ENGINEER)
                {
                    engineerList.Add(user);
                    CboMANAGEENGINEER.Clear();
                    CboMANAGEENGINEER.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGEENGINEER), });
                    CboMANAGEENGINEER.SetItems(engineerList, "Name", "UserId");
                }
            }
        }
        private void InitForm()
        {
            PanContract.Initialize();
            PanEstimate.Initialize();
            PanEstimate.SetData(new HC_OSHA_ESTIMATE());
            
            SSWorkerList.SetDataSource(null);

            PanPrice.SetData(new OSHA_PRICE());
            ChkCharge.Checked = false;
            ChkQuarterCharge.Checked = false;
            SSPrice.SetDataSource(null);

            List<HC_CODE> codes = codeService.FindActiveCodeByGroupCode("OSHA_FEE", "OSHA");
            foreach (HC_CODE code in codes)
            {
                if (code.Code.Equals("FEE_NOTICE"))
                {
                    NumOFFICIALFEE.SetValue(code.CodeName);
                }
            }
        }

        /// <summary>
        /// 단가
        /// </summary>
        private void SetPriceBiding()
        {
            NumPriceWORKERTOTALCOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(OSHA_PRICE.WORKERTOTALCOUNT), Min = 0 });
            NumUNITPRICE.SetOptions(new NumericUpDownOption { DataField = nameof(OSHA_PRICE.UNITPRICE), Min = 0 });
            NumUNITTOTALPRICE.SetOptions(new NumericUpDownOption { DataField = nameof(OSHA_PRICE.UNITTOTALPRICE), Min = 0 });
            NumTOTALPRICE.SetOptions(new NumericUpDownOption { DataField = nameof(OSHA_PRICE.TOTALPRICE), Min = 0 });

            ChkIsFix.SetOptions(new CheckBoxOption { DataField = nameof(OSHA_PRICE.ISFIX), CheckValue = "Y", UnCheckValue = "N" });
            ChkIsBill.SetOptions(new CheckBoxOption { DataField = nameof(OSHA_PRICE.ISBILL), CheckValue = "Y", UnCheckValue = "N" });

            PanPrice.SetData(new OSHA_PRICE());

            SSPrice.Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderHeight = 35 });
            SSPrice.AddColumnText("ID", nameof(OSHA_PRICE.ID), 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SSPrice.AddColumnText("인원", nameof(OSHA_PRICE.WORKERTOTALCOUNT), 70, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SSPrice.AddColumnText("단가", nameof(OSHA_PRICE.UNITPRICE), 90, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
           // SSPrice.AddColumnText("단가금액", nameof(OSHA_PRICE.UNITTOTALPRICE), 90, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSPrice.AddColumnText("계산금액", nameof(OSHA_PRICE.UNITTOTALPRICE ), 90, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSPrice.AddColumnText("계약금액", nameof(OSHA_PRICE.TOTALPRICE), 90, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

            SSPrice.AddColumnText("정액여부", nameof(OSHA_PRICE.ISFIX), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSPrice.AddColumnText("계산서인원단가", nameof(OSHA_PRICE.ISBILL), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSPrice.AddColumnText("청구원청표시", nameof(OSHA_PRICE.ISPARENTCHARGE), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSPrice.AddColumnText("분기청구", nameof(OSHA_PRICE.ISQUARTERCHARGE), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSPrice.AddColumnText("삭제여부", nameof(OSHA_PRICE.ISDELETED), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSPrice.AddColumnText("생성일시", nameof(OSHA_PRICE.MODIFIED), 163, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSPrice.AddColumnText("사용자", nameof(OSHA_PRICE.MODIFIEDUSER), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

            SSChildPrice.Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderHeight = 35 });
            SSChildPrice.AddColumnText("사업장명", nameof(OSHA_PRICE.SITE_NAME), 180, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });            
            SSChildPrice.AddColumnText("인원", nameof(OSHA_PRICE.WORKERTOTALCOUNT), 70, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SSChildPrice.AddColumnText("단가", nameof(OSHA_PRICE.UNITPRICE), 90, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSChildPrice.AddColumnText("계산금액", nameof(OSHA_PRICE.UNITTOTALPRICE), 90, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSChildPrice.AddColumnText("계약금액", nameof(OSHA_PRICE.TOTALPRICE), 90, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSChildPrice.AddColumnCheckBox("정액여부", nameof(OSHA_PRICE.ISFIX), 60,  new CheckBoxFlagEnumCellType<IsFix>());
            SSChildPrice.AddColumnCheckBox("계산서인원단가", nameof(OSHA_PRICE.ISBILL),  112, new CheckBoxFlagEnumCellType<IsBill>());
        }

        private void SearchPrice()
        {
            if(SelectedEstimate != null)
            {
                List<OSHA_PRICE> list = oshaPriceService.OshaPriceRepository.FindAllByEstimateId(SelectedEstimate.ID);
                
                SSPrice.SetDataSource(list);
                for(int i=0; i< list.Count; i++)
                {
                    if(list[i].ISDELETED == "Y")
                    {
                        SSPrice.ActiveSheet.Rows[i].BackColor = Color.LightGray;
                    }

                }

                OSHA_PRICE dto = oshaPriceService.OshaPriceRepository.FindMaxIdByEstimate(SelectedEstimate.ID);
                if (dto == null)
                {
                    dto = new OSHA_PRICE();
                    dto.SITE_NAME = base.SelectedSite.NAME;
                    dto.SITE_ID = base.SelectedSite.ID;
                    dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                }
                else
                {
                    dto.SITE_NAME = base.SelectedSite.NAME;
                }

                List<OSHA_PRICE> chilPriceList = new List<OSHA_PRICE>();
                chilPriceList.Add(dto);

                List<OSHA_PRICE> childPrice = oshaPriceService.OshaPriceRepository.FindAllByParent(SelectedSite.ID);
                foreach (OSHA_PRICE child in childPrice)
                {
                    chilPriceList.Add(child);
                }

                SSChildPrice.SetDataSource(chilPriceList);


                SumChildPrice();
                //if(dto != null)
                //{
                //    PanPrice.SetData(dto);
                //}
                //else
                //{
                //    PanPrice.SetData(new OSHA_PRICE());
                //}
                HC_OSHA_SITE osha = hcOShaSiteService.HcOshaSiteRepository.FindById(SelectedSite.ID);

                if (osha.ISPARENTCHARGE == "Y")
                {
                    ChkCharge.Checked = true;
                }
                else
                {
                    ChkCharge.Checked = false;
                }

                if (osha.ISQUARTERCHARGE == "Y")
                {
                    ChkQuarterCharge.Checked = true;
                }
                else
                {
                    ChkQuarterCharge.Checked = false;
                }

            }
            
        }
        private void BtnSavePrice_Click(object sender, EventArgs e)
        {
            if (SelectedSite == null)
            {
                MessageUtil.Alert("사업장을 선택하세요");
            }
            else
            {
                if (SelectedEstimate == null)
                {
                    MessageUtil.Alert("견적 정보가 없습니다");
                }
                else
                {
                    //if (PanPrice.Validate<OSHA_PRICE>())
                    //{

                    //}
                    OSHA_PRICE dto = PanPrice.GetData<OSHA_PRICE>();
                    dto.ESTIMATE_ID = SelectedEstimate.ID;

                    OSHA_PRICE saved = oshaPriceService.Save(dto);

                    PanPrice.SetData(saved);


                    //하청이 있더라도 청구에서는 원청만 표시하게됨.
                    if (ChkCharge.Checked)
                    {
                        hcOShaSiteService.HcOshaSiteRepository.UpdateCharge(SelectedSite.ID, "Y");
                    }
                    else
                    {
                        hcOShaSiteService.HcOshaSiteRepository.UpdateCharge(SelectedSite.ID, "N");
                    }

                    if (ChkQuarterCharge.Checked)
                    {
                        hcOShaSiteService.HcOshaSiteRepository.UpdateQuarterCharge(SelectedSite.ID, "Y");
                    }
                    else
                    {
                        hcOShaSiteService.HcOshaSiteRepository.UpdateQuarterCharge(SelectedSite.ID, "N");
                    }

                    SearchPrice();
                }
            }
        }

        private void BtnNewPrice_Click(object sender, EventArgs e)
        {
            OSHA_PRICE price = new OSHA_PRICE();

            HC_OSHA_ESTIMATE estimate = PanEstimate.GetData<HC_OSHA_ESTIMATE>();

            price.UNITPRICE = estimate.MONTHLYFEE;
            price.WORKERTOTALCOUNT = estimate.WORKERTOTALCOUNT;
            
            PanPrice.SetData(price);

        }
        private void BtnDeletePrice_Click(object sender, EventArgs e)
        {
            OSHA_PRICE dto = PanPrice.GetData<OSHA_PRICE>();
            if (dto.ID > 0)
            {
                oshaPriceService.Delete(dto.ID);
                PanPrice.Initialize();
                SearchPrice();
            }
        }

        private void SSPrice_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            OSHA_PRICE dto = SSPrice.GetRowData(e.Row) as OSHA_PRICE;
            PanPrice.SetData(dto);
            NumUNITTOTALPRICE.SetValue(dto.UNITTOTALPRICE);
            NumTOTALPRICE.SetValue(dto.TOTALPRICE);

            HC_OSHA_SITE osha = hcOShaSiteService.HcOshaSiteRepository.FindById(SelectedSite.ID);
            
            if(osha.ISPARENTCHARGE == "Y")
            {
                ChkCharge.Checked = true;
            }
            else
            {
                ChkCharge.Checked = false;
            }

            if (osha.ISQUARTERCHARGE == "Y")
            {
                ChkQuarterCharge.Checked = true;
            }
            else
            {
                ChkQuarterCharge.Checked = false;
            }
        }
   

        private void NumPriceWORKERTOTALCOUNT_ValueChanged(object sender, EventArgs e)
        {
            SetTotalPrice();
        }

        private void NumUNITPRICE_ValueChanged(object sender, EventArgs e)
        {
            SetTotalPrice();
        }

        private void NumPriceWORKERTOTALCOUNT_KeyUp(object sender, KeyEventArgs e)
        {
            SetTotalPrice();
        }

        private void NumUNITPRICE_KeyUp(object sender, KeyEventArgs e)
        {
            SetTotalPrice();
        }
        private void SetTotalPrice()
        {
            try{
                double totalPrice = oshaPriceService.GetTotalPrice(NumPriceWORKERTOTALCOUNT.GetValue(), NumUNITPRICE.GetValue());
                NumUNITTOTALPRICE.SetValue(totalPrice);
            }
            catch(Exception ex)
            {
                Log.Error(ex);
            }
            

        }

      

        private void contentTitle1_Load(object sender, EventArgs e)
        {

        }

        private void OshaSiteEstimateList_Load(object sender, EventArgs e)
        {

        }

        private void oshaSiteLastTree_Load(object sender, EventArgs e)
        {

        }

        private void formTItle1_Load(object sender, EventArgs e)
        {

        }

        private void PanContract_Paint(object sender, PaintEventArgs e)
        {

        }

     

        private void NumUNITTOTALPRICE_ValueChanged(object sender, EventArgs e)
        {
            if (ChkIsFix.Checked == false)
            {
                NumTOTALPRICE.SetValue(NumUNITTOTALPRICE.Value);
            }
           
        }

        private void NumWORKERTOTALCOUNT_ValueChanged(object sender, EventArgs e)
        {
            SetFee();
        }

        private void NumSITEFEE_ValueChanged(object sender, EventArgs e)
        {
            SetFee();
        }
        private void SetFee()
        {
            HC_OSHA_ESTIMATE dto = PanEstimate.GetData<HC_OSHA_ESTIMATE>();
            if (dto != null)
            {
                NumMONTHLYFEE.SetValue(NumberUtil.Comma(dto.SITEFEE * dto.WORKERTOTALCOUNT));
            }
        }

        public void Select(ISiteModel siteModel)
        {
            oshaSiteLastTree.SelecteNode(siteModel.ID);
        }

        public void Select(IEstimateModel estimateModel)
        {
           
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            PanContract.SetData(new HC_OSHA_CONTRACT());

            base.SelectedEstimate = null;
            PanEstimate.Initialize();
            PanContract.Initialize();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SSWorkerList.AddRows();
        }

        private void oshaSiteLastTree_Load_1(object sender, EventArgs e)
        {

        }

        private void BtnPdf_Click(object sender, EventArgs e)
        {
            HC_OSHA_ESTIMATE dto = PanEstimate.GetData<HC_OSHA_ESTIMATE>();
            if (dto.ID > 0)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF파일|*.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName.Length != 0)
                {
                    SpreadPrint print = new SpreadPrint(SSEstimate, PrintStyle.STANDARD_APPROVAL);

                    print.ExportPDFNoWait(saveFileDialog.FileName, SSEstimate.ActiveSheet);

                    MessageUtil.Info("PDF 저장하였습니다");
                }
            }
        }

        private void BtnSaveChildPrice_Click(object sender, EventArgs e)
        {
            for(int i=0; i< SSChildPrice.RowCount(); i++)
            {
                OSHA_PRICE dto = SSChildPrice.GetRowData(i) as OSHA_PRICE;                
                oshaPriceService.Save(dto);
            }
            SearchPrice();
        }

        private void SSChildPrice_Change(object sender, ChangeEventArgs e)
        {
            OSHA_PRICE dto = SSChildPrice.GetRowData(e.Row) as OSHA_PRICE;
            long totalPrice = (long)oshaPriceService.GetTotalPrice(dto.WORKERTOTALCOUNT, dto.UNITPRICE);
            dto.UNITTOTALPRICE = totalPrice;
            dto.TOTALPRICE = totalPrice;

            //SSChildPrice.ActiveSheet.Cells[e.Row, 3].Text = totalPrice.ToString();
            //SSChildPrice.ActiveSheet.Cells[e.Row, 4].Text = totalPrice.ToString();
            SSChildPrice.ActiveSheet.SetValue(e.Row, 3, totalPrice);
            SSChildPrice.ActiveSheet.SetValue(e.Row, 4, totalPrice);
            SumChildPrice();
        }
        private void SumChildPrice()
        {

            long sumWorkerCount = 0;
            long sumUnitPrice = 0;
            long sumotalPrice = 0;
            for (int i = 0; i < SSChildPrice.RowCount(); i++)
            {
                OSHA_PRICE price = SSChildPrice.GetRowData(i) as OSHA_PRICE;
                sumWorkerCount += price.WORKERTOTALCOUNT;
                sumUnitPrice += price.UNITTOTALPRICE;
                sumotalPrice += price.TOTALPRICE;
            }
            TxtTotalWorkerCount.Text = string.Format("{0:#,0}", sumWorkerCount);
            TxtTotalUnitPrice.Text = string.Format("{0:#,0}", sumUnitPrice);
            TxtTotalPrice.Text = string.Format("{0:#,0}", sumotalPrice);
        }
        private void RdoIsContract_Y_Click(object sender, EventArgs e)
        {
            //DtpTERMINATEDATE.SetValue(null);
        }

        private void RdoIsContract_N_Click(object sender, EventArgs e)
        {

        }

        private void BtnDeleteParent_Click(object sender, EventArgs e)
        {
            if (SelectedSite != null)
            {
                if (SelectedSite.ID > 0)
                {
                    HC_OSHA_SITE self = hcOShaSiteService.HcOshaSiteRepository.FindById(SelectedSite.ID);
                    if(self.PARENTSITE_ID > 0)
                    {
                        if (MessageUtil.Confirm(SelectedSite.NAME + " 하청 사업장의 원청 소속을 해제 하시겠습니까?") == DialogResult.Yes)
                        {
                            try
                            {
                                clsDB.setBeginTran(clsDB.DbCon);
                                
                                hcOShaSiteService.HcOshaSiteRepository.UpdateCandelParentSite(SelectedSite.ID, Convert.ToInt64(TxtParentSiteId.Text));

                                clsDB.setCommitTran(clsDB.DbCon);
                            }
                            catch(Exception ex)
                            {
                                Log.Error(ex);
                                MessageUtil.Alert(ex.Message);
                                clsDB.setRollbackTran(clsDB.DbCon);
                            }
                            
                        }
                    }
                    
                        
                }
            }
           
        }

        private void label20_Click(object sender, EventArgs e)
        {
            DtpCONTRACTSTARTDATE.Value = DtpSTARTDATE.Value;
            NumCOMMISSION.Value = NumSITEFEE.Value;
            NumWORKERWHITEMALECOUNT.Value = long.Parse(TxtWhiteMale.Text.Trim());
            NumWORKERWHITEFEMALECOUNT.Value = long.Parse(TxtWhiteFeMale.Text.Trim());
            NumWORKERBLUEMALECOUNT.Value = long.Parse(TxtBlueMale.Text.Trim());
            NumWORKERBLUEFEMALECOUNT.Value = long.Parse(TxtBlueFeMale.Text.Trim());
            NumContractWORKERTOTALCOUNT.Value = NumWORKERTOTALCOUNT.Value;
            NumMANAGEWORKERCOUNT.Value = NumWORKERTOTALCOUNT.Value;
        }

        private void NumMANAGEWORKERCOUNT_ValueChanged(object sender, EventArgs e)
        {
            ManageDoctorCount_AutoSet();
        }

        private void ManageDoctorCount_AutoSet()
        {
            if (NumMANAGEWORKERCOUNT.Value >= 100)
            {
                NumMANAGEDOCTORCOUNT.SetValue(3);
                NumMANAGENURSECOUNT.SetValue(1);
                NumMANAGEENGINEERCOUNT.SetValue(2);
            }
            else
            {
                NumMANAGEDOCTORCOUNT.SetValue(6);
                NumMANAGENURSECOUNT.SetValue(1);
                NumMANAGEENGINEERCOUNT.SetValue(3);
            }
        }
        //private void NumUNITTOTALPRICE_KeyUp(object sender, KeyEventArgs e)
        //{
        //    double unitPrice = oshaPriceService.GetUnitPrice(NumPriceWORKERTOTALCOUNT.GetValue(), NumUNITTOTALPRICE.GetValue());
        //    NumUNITPRICE.SetValue(unitPrice);
        //    NumTOTALPRICE.SetValue(NumUNITTOTALPRICE.GetValue());
        //}


    }
    
}
