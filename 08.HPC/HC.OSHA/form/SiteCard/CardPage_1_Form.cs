using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using FarPoint.Win.Spread;
using HC.Core.Common.Interface;
using HC.Core.Common.Util;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Repository;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC.OSHA.Service;
using HC_Core;
using System;
using System.Collections.Generic;
using static HC.Core.Service.LogService;

namespace HC_OSHA
{
    public partial class CardPage_1_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {
        private CardPage1Service cardPage1Service;
        private HcOshaContractService hcOshaContractService;
        private HcUsersRepository hcUsersRepository;
        private List<HC_OSHA_CONTRACT_MANAGER_MODEL> workerList;
        public CardPage_1_Form()
        {
            InitializeComponent();
            cardPage1Service = new CardPage1Service();
            hcOshaContractService = new HcOshaContractService();
            hcUsersRepository = new HcUsersRepository();
        }
        private void CardPage_1_Form_Load(object sender, EventArgs e)
        {
            SpreadComboBoxData SITE_CARD_ACCIDENT_TYPE = codeService.GetSpreadComboBoxData("SITE_CARD_TASK_TYPE", "OSHA");

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSList.AddColumnComboBox("책임자구분", nameof(HC_OSHA_CARD3.TASKTYPE), 150, IsReadOnly.N, SITE_CARD_ACCIDENT_TYPE, new SpreadCellTypeOption { });
            SSList.AddColumnText("성명 또는 대행기관명", nameof(HC_OSHA_CARD3.NAME), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnDateTime("선임일자(계약일자", nameof(HC_OSHA_CARD3.DELACEDATE), 120, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String });
            SSList.AddColumnText("자격면허종류", nameof(HC_OSHA_CARD3.CERT), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("학력", nameof(HC_OSHA_CARD3.EDUCATION), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("산업안전보건경력", nameof(HC_OSHA_CARD3.CARRER), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("교육과정명", nameof(HC_OSHA_CARD3.EDUATIONNAME), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnDateTime("교육기간", nameof(HC_OSHA_CARD3.EDUCATIONSTARTDATE), 100, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String });
            SSList.AddColumnDateTime("교육기간", nameof(HC_OSHA_CARD3.EDUCATIONENDDATE), 100, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String });
            SSList.AddColumnButton(" ", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += (s, ev) => { SSList.DeleteRow(ev.Row); };

            Search();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (SSList.Validate())
                {
                    IList<HC_OSHA_CARD3> list = SSList.GetEditbleData<HC_OSHA_CARD3>();
                    if (list.Count > 0)
                    {
                        //if (cardPage1Service.Save(list, base.SelectedSite.ID, base.SelectedEstimate.ID, base.GetCurrentYear()))
                        if (cardPage1Service.Save(list, base.SelectedSite.ID, base.SelectedEstimate.ID, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4)))
                        {
                         //   MessageUtil.Info("저장하였습니다");
                            Search();
                            LogService.Instance.Task(base.SelectedSite.ID, TaskName.SITE_CARD_1);

                        }
                        else
                        {
                            MessageUtil.Alert("오류가 발생하였습니다. ");

                        }
                    }
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null || base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                SSList.AddRows();
            }
        }

        public void Select(IEstimateModel estimateModel)
        {
            base.SelectedEstimate = estimateModel;
            Search();
        }

        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;

            SSList.SetDataSource(new List<HC_OSHA_CARD3>());
        }

        private void Search()
        {
            Clear();

            if (base.SelectedSite != null && base.SelectedEstimate != null)
            {
                //List<HC_OSHA_CARD3> list = cardPage1Service.FindAll(base.SelectedEstimate.ID, base.GetCurrentYear());
                List<HC_OSHA_CARD3> list = cardPage1Service.FindAll(base.SelectedEstimate.ID, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4));
                SSList.SetDataSource(list);
                GetSSCard(list);
            }
            else
            {
                SSList.SetDataSource(new List<HC_OSHA_CARD6>());
            }
           

        }
        //private HC_OSHA_CONTRACT_MANAGER_MODEL GetWorker(string worker_role)
        //{
        //    foreach(HC_OSHA_CONTRACT_MANAGER_MODEL model in workerList)
        //    {
        //        if(model.WORKER_ROLE == worker_role)
        //        {
        //            return model;
        //        }
        //    }
        //    return null;
        //}
        public void Clear()
        {
            SSCard.ActiveSheet.Cells[3, 1].Value = "";
            SSCard.ActiveSheet.Cells[3, 10].Value = "";
            SSCard.ActiveSheet.Cells[3, 14].Value = "";
            SSCard.ActiveSheet.Cells[5, 14].Value = "";
            SSCard.ActiveSheet.Cells[7, 14].Value = "";

            //위탁기간
            SSCard.ActiveSheet.Cells[3, 20].Value = "";
            SSCard.ActiveSheet.Cells[6, 20].Value = "";

            //사업장명
            SSCard.ActiveSheet.Cells[11, 4].Value = "";
            SSCard.ActiveSheet.Cells[11, 16].Value = "";
            SSCard.ActiveSheet.Cells[11, 25].Value = "";

            SSCard.ActiveSheet.Cells[12, 4].Value = "";

            SSCard.ActiveSheet.Cells[12, 14].Value = false; 
            SSCard.ActiveSheet.Cells[12, 16].Value = false; 
            SSCard.ActiveSheet.Cells[12, 19].Value = false; 
            SSCard.ActiveSheet.Cells[12, 22].Value = false;

            //설립일
            SSCard.ActiveSheet.Cells[13, 4].Value = "";
            SSCard.ActiveSheet.Cells[13, 12].Value = "";
            SSCard.ActiveSheet.Cells[13, 22].Value = "";

            //업종코드

            SSCard.ActiveSheet.Cells[14, 5].Value = "";
            SSCard.ActiveSheet.Cells[14, 9].Value = "";
            SSCard.ActiveSheet.Cells[14, 22].Value = "";
            //주요생산품
            SSCard.ActiveSheet.Cells[15, 4].Value = "";

            //근로자수
            SSCard.ActiveSheet.Cells[18, 4].Value = "";
            SSCard.ActiveSheet.Cells[18, 6].Value = "";
            SSCard.ActiveSheet.Cells[18, 8].Value = "";
            SSCard.ActiveSheet.Cells[18, 10].Value = "";
            //노동자수 합
            SSCard.ActiveSheet.Cells[16, 0].Value = "";

            //교대제
            SSCard.ActiveSheet.Cells[18, 12].Value = "";
            SSCard.ActiveSheet.Cells[18, 14].Value = "";
            SSCard.ActiveSheet.Cells[18, 16].Value = "";
            SSCard.ActiveSheet.Cells[18, 18].Value = "";
            SSCard.ActiveSheet.Cells[18, 20].Value = "";
            SSCard.ActiveSheet.Cells[18, 22].Value = "";
            SSCard.ActiveSheet.Cells[18, 24].Value = "";
            SSCard.ActiveSheet.Cells[18, 25].Value = "";
            SSCard.ActiveSheet.Cells[18, 26].Value = "";


            //시작시간
            SSCard.ActiveSheet.Cells[20, 3].Value = "";
            SSCard.ActiveSheet.Cells[20, 5].Value = "";
            SSCard.ActiveSheet.Cells[20, 7].Value = "";
            SSCard.ActiveSheet.Cells[20, 9].Value = "";
            SSCard.ActiveSheet.Cells[20, 13].Value = "";
            SSCard.ActiveSheet.Cells[20, 16].Value = "";
            SSCard.ActiveSheet.Cells[20, 22].Value = "";
            SSCard.ActiveSheet.Cells[20, 25].Value = "";

            //근로자 대표
            SSCard.ActiveSheet.Cells[21, 3].Value = "";
            SSCard.ActiveSheet.Cells[21, 18].Value = "";

            //안전업무 `명
            SSCard.ActiveSheet.Cells[26, 7].Value = "";
            SSCard.ActiveSheet.Cells[26, 14].Value = "";
            SSCard.ActiveSheet.Cells[26, 20].Value = "";
            SSCard.ActiveSheet.Cells[26, 24].Value = "";
            //보건 3 명까지
            for(int i=27; i<30; i++)
            {
                SSCard.ActiveSheet.Cells[i, 7].Value = "";
                SSCard.ActiveSheet.Cells[i, 14].Value = "";
                SSCard.ActiveSheet.Cells[i, 20].Value = "";
                SSCard.ActiveSheet.Cells[i, 24].Value = "";
            }

            for (int i=34; i< 39; i++)
            {
                SSCard.ActiveSheet.Cells[i, 4].Value = "";
                SSCard.ActiveSheet.Cells[i, 8].Value = "";
                //SSCard.ActiveSheet.Cells[i, 12].Value = "";
                //SSCard.ActiveSheet.Cells[i, 15].Value = "";
                //SSCard.ActiveSheet.Cells[i, 17].Value = "";
                //SSCard.ActiveSheet.Cells[i, 20].Value = "";
                //SSCard.ActiveSheet.Cells[i, 24].Value = "";

                SSCard.ActiveSheet.Cells[i, 11].Value = "";
                SSCard.ActiveSheet.Cells[i, 16].Value = "";
                SSCard.ActiveSheet.Cells[i, 18].Value = "";
                SSCard.ActiveSheet.Cells[i, 21].Value = "";
                SSCard.ActiveSheet.Cells[i, 25].Value = "";
            }
            SSCard.ActiveSheet.Cells[24, 22].Value = "";

        }
        /// <summary>
        /// 1. 사업자현황
        /// </summary>
        private void GetSSCard(List<HC_OSHA_CARD3> managerList)
        {
            HC_OSHA_CONTRACT contract = hcOshaContractService.FindByEstimateId(base.SelectedEstimate.ID);
            if (contract == null )
            {
                return;
            }
            if(contract.CONTRACTENDDATE == null || contract.CONTRACTSTARTDATE == null)
            {
                MessageUtil.Alert("계약 시작 또는 종료일이 없습니다");
                return;
            }
            HC_SITE_VIEW site = new HcSiteViewRepository().FindById(base.SelectedSite.ID);
          //  workerList = new HcOshaContractManagerService().FindSiteWorker(base.SelectedSite.ID, base.SelectedEstimate.ID);
            
            string year = contract.CONTRACTSTARTDATE.Substring(0, 4);

            SSCard.ActiveSheet.Cells[3, 1].Value = year;
            SSCard.ActiveSheet.Cells[3, 10].Value = clsType.HosInfo.strNameKor;

            if (contract.MANAGEDOCTOR!=null)
                SSCard.ActiveSheet.Cells[3,14].Value = hcUsersRepository.FindOne(contract.MANAGEDOCTOR).Name;

            if (contract.MANAGENURSE != null)
                SSCard.ActiveSheet.Cells[5, 14].Value = hcUsersRepository.FindOne(contract.MANAGENURSE).Name;

            if (contract.MANAGEENGINEER != null)
                SSCard.ActiveSheet.Cells[7, 14].Value = hcUsersRepository.FindOne(contract.MANAGEENGINEER).Name;

            //위탁기간
            SSCard.ActiveSheet.Cells[3, 20].Value = contract.CONTRACTSTARTDATE;
            SSCard.ActiveSheet.Cells[6, 20].Value = contract.CONTRACTENDDATE;

            //사업장명
            SSCard.ActiveSheet.Cells[11, 4].Value = site.NAME;
            SSCard.ActiveSheet.Cells[11, 16].Value = site.BIZNUMBER;
            SSCard.ActiveSheet.Cells[11, 25].Value = site.CEONAME;

            SSCard.ActiveSheet.Cells[12, 4].Value = site.ADDRESS;

            if (contract.POSITION == "0") { SSCard.ActiveSheet.Cells[12, 14].Value = true; }
            if (contract.POSITION == "1") { SSCard.ActiveSheet.Cells[12, 16].Value = true; }
            if (contract.POSITION == "2") { SSCard.ActiveSheet.Cells[12, 19].Value = true; }
            if (contract.POSITION == "3") { SSCard.ActiveSheet.Cells[12, 22].Value = true; }

            //설립일
            if (site.BIZCREATEDATE != null)
            {
                SSCard.ActiveSheet.Cells[13, 4].Value = ((DateTime)site.BIZCREATEDATE).ToString("yyyy-MM-dd");
            }
            
            SSCard.ActiveSheet.Cells[13, 12].Value = site.TEL;
            SSCard.ActiveSheet.Cells[13, 22].Value = site.FAX;
            //업종코드
            SSCard.ActiveSheet.Cells[14, 5].Value = site.BizUPJONG;
            SSCard.ActiveSheet.Cells[14, 9].Value = site.BIZUPJONGNAME;
            SSCard.ActiveSheet.Cells[14, 22].Value = site.BIZKIHO; //사업장관리번호
            //주요생산품
            SSCard.ActiveSheet.Cells[15, 4].Value = site.JEPUMLIST;

            //근로자수
            SSCard.ActiveSheet.Cells[18, 4].Value = contract.WORKERWHITEMALECOUNT;
            SSCard.ActiveSheet.Cells[18, 6].Value = contract.WORKERWHITEFEMALECOUNT;
            SSCard.ActiveSheet.Cells[18, 8].Value = contract.WORKERBLUEMALECOUNT;
            SSCard.ActiveSheet.Cells[18, 10].Value = contract.WORKERBLUEFEMALECOUNT;
            //근로자수 합
            long sum = contract.WORKERWHITEMALECOUNT + contract.WORKERWHITEFEMALECOUNT + contract.WORKERBLUEMALECOUNT + contract.WORKERBLUEFEMALECOUNT;
            SSCard.ActiveSheet.Cells[16, 0].Value = string.Concat("근로자수\n(계 : ", sum, ")");
 

            //교대제
            if (contract.ISROTATION == "0")
            {
                SSCard.ActiveSheet.Cells[18, 12].Value = "○";
            }
            else
            {
                SSCard.ActiveSheet.Cells[18, 14].Value = "○";
            }
            //건물소유
            if (contract.BUILDINGTYPE == "0")
            {
                SSCard.ActiveSheet.Cells[18, 16].Value = "○";
            }
            else
            {
                SSCard.ActiveSheet.Cells[18, 18].Value = "○";
            }
            //생산방식
            if (contract.ISPRODUCTTYPE == "0")
            {
                SSCard.ActiveSheet.Cells[18, 20].Value = "○";
            }
            else
            {
                SSCard.ActiveSheet.Cells[18, 22].Value = "○";
            }
            //노동조합
            if (contract.ISLABOR == "0")
            {
                SSCard.ActiveSheet.Cells[18, 24].Value = "○";
            }
            else
            {
                SSCard.ActiveSheet.Cells[18, 25].Value = "○";
            }
            List<HC_OSHA_CONTRACT_MANAGER_MODEL> LABOR_ROLE_LIST = new HcOshaContractManagerService().hcOshaContractManagerRepository.FindContractManagerByRole(base.SelectedEstimate.ID, "LABOR_ROLE");
            if (LABOR_ROLE_LIST.Count > 0)
            {
                HC_OSHA_CONTRACT_MANAGER_MODEL LABOR_ROLE = LABOR_ROLE_LIST[0];
                if (LABOR_ROLE != null)
                {
                    SSCard.ActiveSheet.Cells[18, 26].Value = LABOR_ROLE.NAME;//노동조합대표
                }
            }

            //  사업장 업무일정
            if(contract.VISITWEEK.NotEmpty() && contract.VISITDAY.NotEmpty())
            {
                SSCard.ActiveSheet.Cells[20, 0].Value = string.Concat(contract.VISITWEEK, ", ", contract.VISITDAY);
            }
            
            //시작시간
            SSCard.ActiveSheet.Cells[20, 3].Value = contract.WORKSTARTTIME;
            SSCard.ActiveSheet.Cells[20, 5].Value = contract.WORKENDTIME;
            SSCard.ActiveSheet.Cells[20, 7].Value = contract.WORKMEETTIME;//조회시간
            SSCard.ActiveSheet.Cells[20, 9].Value = contract.WORKROTATIONTIME; // 고대시간
            SSCard.ActiveSheet.Cells[20, 13].Value = contract.WORKLUANCHTIME;//점심시간
            SSCard.ActiveSheet.Cells[20, 16].Value = contract.WORKRESTTIME;//휴식시간
            SSCard.ActiveSheet.Cells[20, 22].Value = contract.WORKEDUTIME;//교육시간
            SSCard.ActiveSheet.Cells[20, 25].Value = contract.WORKETCTIME;//기타시간

            //근로자 대표
            List<HC_OSHA_CONTRACT_MANAGER_MODEL> WM_ROLE_LIST = new HcOshaContractManagerService().hcOshaContractManagerRepository.FindContractManagerByRole(base.SelectedEstimate.ID, "WM_ROLE");
            if (WM_ROLE_LIST.Count > 0)
            {
                HC_OSHA_CONTRACT_MANAGER_MODEL WM_ROLE = WM_ROLE_LIST[0];
                if (WM_ROLE != null)
                {
                    SSCard.ActiveSheet.Cells[21, 3].Value = "성명: " + WM_ROLE.NAME + " 소속(직책): " + WM_ROLE.DEPT;
                }
            }
            List<HC_OSHA_CONTRACT_MANAGER_MODEL> DIRECTOR_ROLE_LIST = new HcOshaContractManagerService().hcOshaContractManagerRepository.FindContractManagerByRole(base.SelectedEstimate.ID, "DIRECTOR_ROLE");
            if (DIRECTOR_ROLE_LIST.Count > 0)
            {
                HC_OSHA_CONTRACT_MANAGER_MODEL DIRECTOR_ROLE = DIRECTOR_ROLE_LIST[0];
                if (DIRECTOR_ROLE != null)
                {
                    SSCard.ActiveSheet.Cells[21, 18].Value = "성명: " + DIRECTOR_ROLE.NAME + " 소속(직책): " + DIRECTOR_ROLE.DEPT;
                }

            }

            //EMP_ROLE 사원
            //SAFE_ROLE 안전담당자
            //HEALTH_ROLE 보건담당자
            //DIRECTOR_ROLE 명예산업안전감독관
            //LABOR_ROLE 노동조합대표자
            //WM_ROLE 근로자대표

            //안전업무안전담당자 1명
            //HC_OSHA_CONTRACT_MANAGER_MODEL SAFE_ROLE = GetWorker("SAFE_ROLE");
            List<HC_OSHA_CONTRACT_MANAGER_MODEL> SAFE_ROLE_LIST = new HcOshaContractManagerService().hcOshaContractManagerRepository.FindContractManagerByRole(base.SelectedEstimate.ID, "SAFE_ROLE");
            if (SAFE_ROLE_LIST.Count > 0)
            {
                HC_OSHA_CONTRACT_MANAGER_MODEL SAFE_ROLE = SAFE_ROLE_LIST[0];
                if (SAFE_ROLE_LIST != null)
                {
                    SSCard.ActiveSheet.Cells[26, 7].Value = SAFE_ROLE.NAME;
                    SSCard.ActiveSheet.Cells[26, 14].Value = SAFE_ROLE.DEPT;
                    SSCard.ActiveSheet.Cells[26, 20].Value = SAFE_ROLE.TEL;
                    SSCard.ActiveSheet.Cells[26, 24].Value = SAFE_ROLE.HP;
                }
            }

            //보건 3 명까지
            List<HC_OSHA_CONTRACT_MANAGER_MODEL> HealthList = new HcOshaContractManagerService().hcOshaContractManagerRepository.FindContractManagerByRole(base.SelectedEstimate.ID, "HEALTH_ROLE");
            int rowIndex = 27;
            foreach(HC_OSHA_CONTRACT_MANAGER_MODEL model in HealthList)
            {
                SSCard.ActiveSheet.Cells[rowIndex, 7].Value = model.NAME;
                SSCard.ActiveSheet.Cells[rowIndex, 14].Value = model.DEPT;
                SSCard.ActiveSheet.Cells[rowIndex, 20].Value = model.TEL;
                SSCard.ActiveSheet.Cells[rowIndex, 24].Value = model.HP;
                rowIndex += 1;
            }
            if (HealthList != null && HealthList.Count>0)
            {
                //보건담당자 이메일
                SSCard.ActiveSheet.Cells[24, 22].Value = HealthList[0].EMAIL;
            }

            //관리책임자등 선임현황
            //0   안전보건관리책임자
            //1   안전보건 총괄책임자
            //2   안전관리자
            //3   보건관리자
            //4   산업보건의
            foreach (HC_OSHA_CARD3 model in managerList)
            {
                if(model.TASKTYPE == "0")
                {
                    SSCard.ActiveSheet.Cells[34, 4].Value = model.NAME;
                    SSCard.ActiveSheet.Cells[34, 8].Value = model.DELACEDATE;
                    //SSCard.ActiveSheet.Cells[34, 12].Value = model.CERT;
                    //SSCard.ActiveSheet.Cells[34, 15].Value = model.EDUCATION;
                    //SSCard.ActiveSheet.Cells[34, 17].Value = model.CARRER;
                    //SSCard.ActiveSheet.Cells[34, 20].Value = model.EDUATIONNAME;

                    SSCard.ActiveSheet.Cells[34, 11].Value = model.CERT;
                    SSCard.ActiveSheet.Cells[34, 16].Value = model.EDUCATION;
                    SSCard.ActiveSheet.Cells[34, 18].Value = model.CARRER;
                    SSCard.ActiveSheet.Cells[34, 21].Value = model.EDUATIONNAME;

                    //SSCard.ActiveSheet.Cells[34, 20].Value = model.EDUCATION;
                    if (model.EDUCATIONSTARTDATE.NotEmpty())
                    {
                        //SSCard.ActiveSheet.Cells[34, 24].Value = model.EDUCATIONSTARTDATE + "\n" + model.EDUCATIONENDDATE;
                        SSCard.ActiveSheet.Cells[34, 25].Value = model.EDUCATIONSTARTDATE + "\n" + model.EDUCATIONENDDATE;
                    }
                    
                }
                else if (model.TASKTYPE == "1")
                {
                    SSCard.ActiveSheet.Cells[35, 4].Value = model.NAME;
                    SSCard.ActiveSheet.Cells[35, 8].Value = model.DELACEDATE;
                    //SSCard.ActiveSheet.Cells[35, 12].Value = model.CERT;
                    //SSCard.ActiveSheet.Cells[35, 15].Value = model.EDUCATION;
                    //SSCard.ActiveSheet.Cells[35, 17].Value = model.CARRER;
                    //SSCard.ActiveSheet.Cells[35, 20].Value = model.EDUATIONNAME;

                    SSCard.ActiveSheet.Cells[35, 11].Value = model.CERT;
                    SSCard.ActiveSheet.Cells[35, 16].Value = model.EDUCATION;
                    SSCard.ActiveSheet.Cells[35, 18].Value = model.CARRER;
                    SSCard.ActiveSheet.Cells[35, 21].Value = model.EDUATIONNAME;
                    if (model.EDUCATIONSTARTDATE.NotEmpty())
                    {
                        //SSCard.ActiveSheet.Cells[35, 24].Value = model.EDUCATIONSTARTDATE + "\n" + model.EDUCATIONENDDATE;
                        SSCard.ActiveSheet.Cells[35, 25].Value = model.EDUCATIONSTARTDATE + "\n" + model.EDUCATIONENDDATE;
                    }
                }
                else if (model.TASKTYPE == "2")
                {
                    SSCard.ActiveSheet.Cells[36, 4].Value = model.NAME;
                    SSCard.ActiveSheet.Cells[36, 8].Value = model.DELACEDATE;
                    //SSCard.ActiveSheet.Cells[36, 12].Value = model.CERT;
                    //SSCard.ActiveSheet.Cells[36, 15].Value = model.EDUCATION;
                    //SSCard.ActiveSheet.Cells[36, 17].Value = model.CARRER;
                    //SSCard.ActiveSheet.Cells[36, 20].Value = model.EDUATIONNAME;
                    SSCard.ActiveSheet.Cells[36, 11].Value = model.CERT;
                    SSCard.ActiveSheet.Cells[36, 16].Value = model.EDUCATION;
                    SSCard.ActiveSheet.Cells[36, 18].Value = model.CARRER;
                    SSCard.ActiveSheet.Cells[36, 21].Value = model.EDUATIONNAME;
                    if (model.EDUCATIONSTARTDATE.NotEmpty())
                    {
                        //SSCard.ActiveSheet.Cells[36, 24].Value = model.EDUCATIONSTARTDATE + "\n" + model.EDUCATIONENDDATE;
                        SSCard.ActiveSheet.Cells[36, 25].Value = model.EDUCATIONSTARTDATE + "\n" + model.EDUCATIONENDDATE;
                    }
                }
                else if (model.TASKTYPE == "3")
                {
                    SSCard.ActiveSheet.Cells[37, 4].Value = model.NAME;
                    SSCard.ActiveSheet.Cells[37, 8].Value = model.DELACEDATE;
                    //SSCard.ActiveSheet.Cells[37, 12].Value = model.CERT;
                    //SSCard.ActiveSheet.Cells[37, 15].Value = model.EDUCATION;
                    //SSCard.ActiveSheet.Cells[37, 17].Value = model.CARRER;
                    //SSCard.ActiveSheet.Cells[37, 20].Value = model.EDUATIONNAME;

                    SSCard.ActiveSheet.Cells[37, 11].Value = model.CERT;
                    SSCard.ActiveSheet.Cells[37, 16].Value = model.EDUCATION;
                    SSCard.ActiveSheet.Cells[37, 18].Value = model.CARRER;
                    SSCard.ActiveSheet.Cells[37, 21].Value = model.EDUATIONNAME;

                    if (model.EDUCATIONSTARTDATE.NotEmpty())
                    {
                        //SSCard.ActiveSheet.Cells[34, 24].Value = model.EDUCATIONSTARTDATE + "\n" + model.EDUCATIONENDDATE;
                        SSCard.ActiveSheet.Cells[34, 25].Value = model.EDUCATIONSTARTDATE + "\n" + model.EDUCATIONENDDATE;
                    }
                }
                else if (model.TASKTYPE == "4")
                {
                    SSCard.ActiveSheet.Cells[38, 4].Value = model.NAME;
                    SSCard.ActiveSheet.Cells[38, 8].Value = model.DELACEDATE;
                    //SSCard.ActiveSheet.Cells[38, 12].Value = model.CERT;
                    //SSCard.ActiveSheet.Cells[38, 15].Value = model.EDUCATION;
                    //SSCard.ActiveSheet.Cells[38, 17].Value = model.CARRER;
                    //SSCard.ActiveSheet.Cells[38, 20].Value = model.EDUATIONNAME;
                    SSCard.ActiveSheet.Cells[38, 11].Value = model.CERT;
                    SSCard.ActiveSheet.Cells[38, 16].Value = model.EDUCATION;
                    SSCard.ActiveSheet.Cells[38, 18].Value = model.CARRER;
                    SSCard.ActiveSheet.Cells[38, 21].Value = model.EDUATIONNAME;
                    if (model.EDUCATIONSTARTDATE.NotEmpty())
                    {
                    }
                }
            }
        }

        public void Print()
        {
            Search();

            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.printCompletedEventHandler += Print_printCompletedEventHandler;
            print.Execute(SSCard.ActiveSheet);
            //Task.WaitAny(print.Execute());
        }

        public bool NewPrint()
        {
            Search();

            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.printCompletedEventHandler += Print_printCompletedEventHandler;
            print.Execute(SSCard.ActiveSheet);

            return true;
        }

        private void Print_printCompletedEventHandler(object sender, PrintMessageBoxEventArgs e)
        {
            bool xxxxx = e.BeginPrinting;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void SSCard_CellClick(object sender, CellClickEventArgs e)
        {

        }
    }
}
