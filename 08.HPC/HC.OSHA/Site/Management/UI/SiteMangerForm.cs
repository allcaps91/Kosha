using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using HC.OSHA.Site.Management.Dto;
using HC.OSHA.Site.Management.Model;
using HC.OSHA.Site.Management.Service;
using HC.Core.BaseCode.Management.Dto;
using HC.Core.BaseCode.Management.Repository;
using HC.Core.BaseCode.Management.Service;
using HC.Core.Common.Service;
using HC.Core.Common.UI;
using HC.Core.Site.Dto;
using HC.Core.Site.Model;
using HC.Core.Site.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC.OSHA.Site.Management.UI
{
    public partial class SiteMangerForm : CommonForm
    {
        private HcOshaSiteService hcOShaSiteService;
        private HcOshaEstimateService hcOshaEstimateService;
        private HcOshaContractService hcOshaContractService;
      
        private OshaEstimateModelService oshaEstimateModelService;

        private HcOshaContractManagerService hcOshaContractManagerService;

        public SiteMangerForm()
        {
            InitializeComponent();
            hcOShaSiteService = new HcOshaSiteService();
            hcOshaEstimateService = new HcOshaEstimateService();
            oshaEstimateModelService = new OshaEstimateModelService();
            hcOshaContractService = new HcOshaContractService();
            hcOshaContractManagerService = new HcOshaContractManagerService();
        }
        private void SiteMangerForm_Load(object sender, EventArgs e)
        {

            SpreadComboBoxData comboBoxData = codeService.GetSpreadComboBoxData("WORKER_ROLE","OSHA");

            SSWorkerList.Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderHeight = 25 });
            SSWorkerList.AddColumnText("이름", nameof(HC_OSHA_CONTRACT_MANAGER_MODEL.NAME), 75, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SSWorkerList.AddColumnText("소속", nameof(HC_OSHA_CONTRACT_MANAGER_MODEL.DEPT), 117, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnComboBox("직책", nameof(HC_OSHA_CONTRACT_MANAGER_MODEL.WORKER_ROLE), 140, IsReadOnly.Y, comboBoxData, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("전화", nameof(HC_OSHA_CONTRACT_MANAGER_MODEL.TEL), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("휴대폰", nameof(HC_OSHA_CONTRACT_MANAGER_MODEL.HP), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("이메일", nameof(HC_OSHA_CONTRACT_MANAGER_MODEL.EMAIL), 168, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnButton("", 50, new SpreadCellTypeOption { IsSort = false, ButtonText = "삭제" }).ButtonClick += SSSWorkerList_DeleteButtonClick;

            SSWorkerList.SetDataSource(new List<HC_OSHA_CONTRACT_MANAGER_MODEL>());


            SetEstimateBiding();
            SetContractBiding();

            List<HC_CODE> codes = codeService.FindAllByGroupCode("OSHA_FEE", "OSHA");
            foreach(HC_CODE code in codes)
            {
                if (code.Code.Equals("FEE_TYPE"))
                {
                    TxtFEETYPE.SetValue(code.CodeName);
                }
                else if (code.Code.Equals("FEE_NOTICE"))
                {
                    NumOFFICIALFEE.SetValue(code.CodeName);
                }
            }
            
        }

        private void SSSWorkerList_DeleteButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            SSWorkerList.DeleteRow(e.Row);
            //SSWorkerList.GetRowData(e.Row) as HC_OSHA_CONTRACT
        }

        private void SetEstimateBiding()
        {
            DtpESTIMATEDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_ESTIMATE.ESTIMATEDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD});
            DtpSTARTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_ESTIMATE.STARTDATE) , DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD});
            NumOFFICIALFEE.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_ESTIMATE.OFFICIALFEE), });
            NumSITEFEE.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_ESTIMATE.SITEFEE) });
            NumMONTHLYFEE.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_ESTIMATE.MONTHLYFEE) });
            NumWORKERTOTALCOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_ESTIMATE.WORKERTOTALCOUNT) });
            TxtFEETYPE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_ESTIMATE.FEETYPE) });
            TxtRemark.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_ESTIMATE.REMARK) });
            TxtSENDMAILDATE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_ESTIMATE.SENDMAILDATE) });
            TxtPRINTDATE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_ESTIMATE.PRINTDATE) });

            PanEstimate.SetData(new HC_OSHA_ESTIMATE());
        }

        private void SetContractBiding()
        {
            //계약 및 근무일
            RdoIsContract_N.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CONTRACT.ISCONTRACT), CheckValue = "N", UnCheckValue = "Y" });
            RdoIsContract_Y.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CONTRACT.ISCONTRACT), CheckValue = "Y", UnCheckValue = "N" });
            DtpCONTRACTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.CONTRACTDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpCONTRACTSTARTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.CONTRACTSTARTDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpCONTRACTENDDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.CONTRACTENDDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpDECLAREDAY.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.DECLAREDAY), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            TxtVISITDAY.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.VISITDAY) });
            TxtVISITWEEK.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.VISITWEEK) });
            NumCOMMISSION.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.COMMISSION), });
            TxtSPECIALCONTRACT.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.SPECIALCONTRACT) });

            //위치 
            RdoPosition_0.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CONTRACT.POSITION), CheckValue = "0"});
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
            NumWORKERWHITEMALECOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.WORKERWHITEMALECOUNT), });
            NumWORKERWHITEFEMALECOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.WORKERWHITEFEMALECOUNT), });
            NumWORKERBLUEMALECOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.WORKERBLUEMALECOUNT), });
            NumWORKERBLUEFEMALECOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.WORKERBLUEFEMALECOUNT), });
            NumContractWORKERTOTALCOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.WORKERTOTALCOUNT), });

            //   CommonService.Instance.GetDoctors().SetComboBox(CboManageDoctor);
            //  CommonService.Instance.GetNurse().SetComboBox(CboManageNurse);
            // CommonService.Instance.GetEngineerByOsha().SetComboBox(CboMANAGEENGINEER);
            //    CboMANAGEENGINEER.SetItems()
            //    CboManageDoctor.SetItems()
            //담당요원및방문주기
            HcUserService hcUsersService = new HcUserService();
            CboManageDoctor.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGEDOCTOR) });
            CboManageDoctor.SetItems(hcUsersService.GetDoctors(), "Name", "UserId");
            CboManageNurse.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGENURSE), });
            CboManageNurse.SetItems(hcUsersService.GetNurse(), "Name", "UserId");
            CboMANAGEENGINEER.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGEENGINEER), });
            CboMANAGEENGINEER.SetItems(hcUsersService.GetEngineerByOsha(), "Name", "UserId");
            DtpMANAGEDOCTORSTARTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGEDOCTORSTARTDATE), DataBaseFormat = DateTimeType.YYYYMM, DisplayFormat = DateTimeType.YYYY_MM });
            DtpMANAGENURSESTARTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGENURSESTARTDATE), DataBaseFormat = DateTimeType.YYYYMM, DisplayFormat = DateTimeType.YYYY_MM });
            DtpMANAGEENGINEERSTARTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGEENGINEERSTARTDATE), DataBaseFormat = DateTimeType.YYYYMM, DisplayFormat = DateTimeType.YYYY_MM });

            NumMANAGEDOCTORCOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGEDOCTORCOUNT), });
            NumMANAGENURSECOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGENURSECOUNT), });
            NumMANAGEENGINEERCOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGEENGINEERCOUNT), });
            NumMANAGEWORKERCOUNT.SetOptions(new NumericUpDownOption { DataField = nameof(HC_OSHA_CONTRACT.MANAGEWORKERCOUNT), });
            //사업장업무일정
            DtpWORKSTARTTIME.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.WORKSTARTTIME), DataBaseFormat = DateTimeType.HH_MM, DisplayFormat = DateTimeType.HH_MM });
            DtpWORKENDTIME.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.WORKENDTIME), DataBaseFormat = DateTimeType.HH_MM, DisplayFormat = DateTimeType.HH_MM });
            DtpWORKMEETTIME.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CONTRACT.WORKMEETTIME), DataBaseFormat = DateTimeType.HH_MM, DisplayFormat = DateTimeType.HH_MM });
            TxtWORKROTATIONTIME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.WORKROTATIONTIME) });
            TxtWORKLUANCHTIME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.WORKLUANCHTIME) });
            TxtWORKRESTTIME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.WORKRESTTIME) });
            TxtWORKEDUTIME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.WORKEDUTIME) });
            TxtWORKETCTIME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CONTRACT.WORKETCTIME) });

            //추가항목
            ChkISWEM.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_CONTRACT.ISWEM), CheckValue="Y", UnCheckValue="N" });
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
            if(result == DialogResult.OK)
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
        }

        private void BtnEstimateSave_Click(object sender, EventArgs e)
        {

            PanEstimate.Validate< HC_OSHA_ESTIMATE>();


            HC_OSHA_ESTIMATE dto = PanEstimate.GetData<HC_OSHA_ESTIMATE>();


            dto.OSHA_SITE_ID = SelectedSite.ID;

            if(dto.OSHA_SITE_ID == 0)
            {
                MessageUtil.Alert("사업장을 선택하세요");
            }
            else
            {
               

                //try
                //{

                //    PanEstimate.SetData(hcOshaEstimateService.Save(dto));


                //    MessageUtil.Info("견적을 저장하였습니다");
                //}
                //catch (MTSValidationException ex)
                //{
                //    PanEstimate.SetValidationResult(ex.ValidationResult);

                //}
            }
        
        }
     
        private void BtnDeleteEstimate_Click(object sender, EventArgs e)
        {

        }

        private void BtnSaveContract_Click(object sender, EventArgs e)
        {
            if(SelectedSite == null)
            {
                MessageUtil.Alert("사업장을 선택하세요");
            }
            else
            {
                HC_OSHA_CONTRACT contract = PanContract.GetData<HC_OSHA_CONTRACT>();
                contract.ESTIMATE_ID = SelectedEstimate.ID;
                contract.OSHA_SITE_ID = SelectedSite.ID;

                hcOshaContractService.Save(contract);

                
                IList<HC_OSHA_CONTRACT_MANAGER_MODEL> list = SSWorkerList.GetEditbleData<HC_OSHA_CONTRACT_MANAGER_MODEL>();
                if (list != null)
                {
                    hcOshaContractManagerService.Save(list);

                    GetContractManager();
                
                }
                MessageUtil.Info("저장하였습니다.");
                oshaSiteEstimateList.Searh(OshaSiteList.GetSite.ID);
            }
            
        }
        private void BtnDeleteContract_Click(object sender, EventArgs e)
        {
            HC_OSHA_CONTRACT contract = PanContract.GetData<HC_OSHA_CONTRACT>();
            hcOshaContractService.Delete(contract.ESTIMATE_ID);
            MessageUtil.Info("삭제하였습니다");

            PanContract.SetData(new HC_OSHA_CONTRACT());

            oshaSiteEstimateList.Searh(OshaSiteList.GetSite.ID);
        }
        private void oshaSiteList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
          
            
            base.SelectedSite = OshaSiteList.GetSite;
            SetSiteInfo(OshaSiteList.GetSite);
            oshaSiteEstimateList.Searh(OshaSiteList.GetSite.ID);

        }
        private void oshaSiteEstimateList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(oshaSiteEstimateList.GetEstimateModel != null)
            {

                base.SelectedEstimate = oshaSiteEstimateList.GetEstimateModel;

                HC_OSHA_ESTIMATE estimate = hcOshaEstimateService.FindById(base.SelectedEstimate.ID);
                PanEstimate.SetData(estimate);


                //계약정보
                HC_OSHA_CONTRACT contract = hcOshaContractService.FindByEstimateId(estimate.ID);
                
                if (contract != null)
                {
                    PanContract.SetData(contract);
                    GetContractManager();
                }
            }
            else
            {
                PanEstimate.Initialize();
                PanContract.Initialize();
                SSWorkerList.SetDataSource(null);
            }
        }
        private void GetContractManager()
        {
            IList list = hcOshaContractManagerService.FindContractManager(base.SelectedEstimate.ID);
            SSWorkerList.SetDataSource(list);
        }
      

   

     

        private void BtnLastContract_Click(object sender, EventArgs e)
        {
            //PanContract.Initialize();
            PanContract.SetData(new HC_OSHA_CONTRACT());
        }

        private void BtnWorkerAdd_Click(object sender, EventArgs e)
        {

            //사업장 근로자 데이터 가졍오기
            IList list = hcOshaContractManagerService.FindSiteWorker(base.SelectedSite.ID, base.SelectedEstimate.ID);
            SSWorkerList.SetDataSource(list);


        }

        private void BtnSendMailEstimate_Click(object sender, EventArgs e)
        {
            EstimateMailForm form = new EstimateMailForm();
            form.GetMailForm().SenderMailAddress = "faye12005@gmail.com";
            form.GetMailForm().ReciverMailSddress.Add("faye12005@gmail.com");
            form.GetMailForm().ReciverMailSddress.Add("faye12005@gmail.com");
            form.ShowDialog();
        }
    }
}
