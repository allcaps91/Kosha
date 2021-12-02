using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Utils;
using HC.Core.Dto;
using HC.Core.Repository;
using HC.OSHA.Dto;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Model;
using HC.OSHA.Repository;
using HC.OSHA.Repository.StatusReport;
using HC.OSHA.Service;
using HC_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_OSHA.Migration
{
    public partial class MigrationForm : CommonForm
    {
        private HcOshaSiteService hcOshaSiteService;
        private MigrationRepository mig;
        HcOshaContractService hcOshaContractService;
        private HcOshaEstimateService hcOshaEstimateService;
        HcOshaContractManagerService hcOshaContractManagerService;
        HcSiteWorkerRepository hcSiteWorkerRepository;
        public MigrationForm()
        {
            InitializeComponent();
            hcOshaSiteService = new HcOshaSiteService();
            mig = new MigrationRepository();
            hcOshaContractService = new HcOshaContractService();
            hcOshaEstimateService = new HcOshaEstimateService();
            hcOshaContractManagerService = new HcOshaContractManagerService();
            hcSiteWorkerRepository = new HcSiteWorkerRepository();
        }

        private long GetMaxEstimateId(long siteId)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            long estimateId = 0;
            SQL = " ";
            SQL = SQL + ComNum.VBLF + "   select max(id) as estimateId from HIC_OSHA_ESTIMATE ";
            SQL = SQL + ComNum.VBLF + "    where osha_site_id = " + siteId + " ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
            }
            if (dt.Rows.Count > 0)
            {

                estimateId = dt.Rows[0]["estimateId"].To<long>(0);

              

            }
            return estimateId;

        }
        /// <summary>
        ///  truncate table HIC_OSHA_CONTRACT_MANAGER 하고 돌려야 함.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            List<HC_OSHA_SITE_MODEL>  list = hcOshaSiteService.Search("", "", true, false);
            foreach (HC_OSHA_SITE_MODEL model in list)
            {
                string rtnVal = "";
                DataTable dt = null;
                string SQL = "";
                string SqlErr = ""; //에러문 받는 변수
                long estimateId = 0;
                SQL = " ";
                SQL = SQL + ComNum.VBLF + "   select max(id) as estimateId from HIC_OSHA_ESTIMATE ";
                SQL = SQL + ComNum.VBLF + "    where osha_site_id = " + model.ID + " ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                }
                if (dt.Rows.Count > 0)
                {

                    rtnVal = dt.Rows[0]["estimateId"].ToString().Trim();

                    if(rtnVal == "")
                    {
                        //견적생성
                        HC_OSHA_ESTIMATE dto = new HC_OSHA_ESTIMATE();
                        dto.OSHA_SITE_ID = model.ID;
                        dto.EXCELPATH = "C:\\PSMHEXE\\exenet\\Resources\\견적서.xlsx";
                        HC_OSHA_ESTIMATE saved =  hcOshaEstimateService.Save(dto);
                        estimateId = saved.ID;
                         
                    }
                    else
                    {
                        estimateId = long.Parse(rtnVal);
                    }

                }


                List<SiteMaxModel> maxList = mig.GetMaxContract(model.ID);
                try
                {
                    dt = null;
                    if (maxList.Count <= 0)
                    {
                        continue;
                    }
                    SQL = " ";
                    SQL = SQL + ComNum.VBLF + "   SELECT LTDCODE, YYYYMM, TINWON, MINWON1, FINWON1, MINWON2, FINWON2, GO_TINWON, GO_SYYYYMM, GO_SYYYYMM2, GO_SYYYYMM3,    ";
                    SQL = SQL + ComNum.VBLF + "   GO_DOCTOR, GO_DOCTOR_C, GO_NURSE, GO_NURSE_C, GO_BOGEN, GO_BOGEN_C, SET_WEEK, SET_YOIL,    ";
                    SQL = SQL + ComNum.VBLF + "   TO_CHAR(SET_SUNIMDAY, 'YYYY-MM-DD') SET_SUNIMDAY,     ";
                    SQL = SQL + ComNum.VBLF + "   TO_CHAR(SET_GEYAKDAY1, 'YYYY-MM-DD') as  SET_GEYAKDAY1,TO_CHAR(SET_GEYAKDAY2, 'YYYY-MM-DD') SET_GEYAKDAY2,     ";
                    SQL = SQL + ComNum.VBLF + "   SET_TAX,SET_PLACE,SET_CHANGE,SET_HOME,    ";
                    SQL = SQL + ComNum.VBLF + "   SET_MAKE,SET_JOHAP,WORK_STIME,WORK_ETIME,WORK_JTIME,WORK_CTIME,WORK_LTIME,WORK_RTIME,    ";
                    SQL = SQL + ComNum.VBLF + "   WORK_EDTIME,WORK_ETCTIME,SET_SDAM11,SET_SDAM12,SET_SDAM13,SET_SDAM14,SET_SDAM15,SET_SDAM21,    ";
                    SQL = SQL + ComNum.VBLF + "   SET_SDAM22,SET_SDAM23,SET_SDAM24,SET_SDAM25,SET_SDAM31,SET_SDAM32,SET_SDAM33,SET_SDAM34,SET_SDAM35,    ";
                    SQL = SQL + ComNum.VBLF + "   SET_BDAM11,SET_BDAM12,SET_BDAM13,SET_BDAM14,SET_BDAM15,SET_BDAM21,SET_BDAM22,SET_BDAM23,    ";
                    SQL = SQL + ComNum.VBLF + "   SET_BDAM24,SET_BDAM25,SET_BDAM31,SET_BDAM32,SET_BDAM33,SET_BDAM34,SET_BDAM35,DELDATE, ENTDATE, ENTPART,EtcRemark,     ";
                    SQL = SQL + ComNum.VBLF + "   GEYAK_01,GEYAK_02,GEYAK_03_11,GEYAK_03_12,GEYAK_03_21,GEYAK_03_22,GEYAK_03_31,GEYAK_03_32,GEYAK_04,GEYAK_05,GEYAK_06,     ";
                    SQL = SQL + ComNum.VBLF + "   UNION_DAEPYO,WORK_DAEPYO,WORK_DAEPYO_BUSE,HONOR_DAEPYO,HONOR_DAEPYO_BUSE, ADDITEM_1,ADDITEM_1_DATA,ADDITEM_2,ADDITEM_3,ADDITEM_3_DATA,ADDITEM_4,ADDITEM_4_DATA, ADDITEM_5,ADDITEM_5_DATA,ADDITEM_6,ADDITEM_6_DATA,ADDITEM_7,ADDITEM_7_DATA, ADDITEM_8,ADDITEM_8_DATA,ROWID FROM KOSMOS_PMPA.HIC_BOGENLTDSET    ";
                    SQL = SQL + ComNum.VBLF + "   WHERE LTDCODE = '"+ model.ID+ "'    ";
                    SQL = SQL + ComNum.VBLF + "   AND YYYYMM = '"+maxList[0].YYYYMM +"'    ";
                    SQL = SQL + ComNum.VBLF + "   AND(DELDATE IS NULL OR DELDATE = '')    ";
                    SQL = SQL + ComNum.VBLF + "   ORDER BY YYYYMM DESC    ";


                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    }
                    if (dt.Rows.Count > 0)
                    {

                        //     rtnVal = dt.Rows[0]["KTASLEVL"].ToString().Trim();

                        HC_OSHA_CONTRACT contract = new HC_OSHA_CONTRACT();
                        contract.ESTIMATE_ID = estimateId;
                        contract.OSHA_SITE_ID = model.ID;

                        if (model.ID == 8865)
                        {
                            string x = "";
                        }
                        contract.WORKERTOTALCOUNT = dt.Rows[0]["TINWON"].To<long>(0);
                        contract.MANAGEWORKERCOUNT = dt.Rows[0]["GO_TINWON"].To<long>(0); 
                        contract.WORKERWHITEMALECOUNT = dt.Rows[0]["MINWON1"].To<long>(0); // 사무직남
                        contract.WORKERWHITEFEMALECOUNT = dt.Rows[0]["FINWON1"].To<long>(0);

                        contract.WORKERBLUEMALECOUNT = dt.Rows[0]["MINWON2"].To<long>(0); // 비사무직 남
                        contract.WORKERBLUEFEMALECOUNT = dt.Rows[0]["FINWON2"].To<long>(0); // 비사무직 여

                        contract.MANAGEDOCTOR = dt.Rows[0]["GO_DOCTOR"].ToString();
                        contract.MANAGEDOCTORSTARTDATE = dt.Rows[0]["GO_SYYYYMM"].ToString();
                        contract.MANAGEDOCTORCOUNT = dt.Rows[0]["GO_DOCTOR_C"].To<long>(0);

                        contract.MANAGENURSE = dt.Rows[0]["GO_NURSE"].ToString();
                        contract.MANAGENURSESTARTDATE = dt.Rows[0]["GO_SYYYYMM2"].ToString();
                        contract.MANAGENURSECOUNT = dt.Rows[0]["GO_NURSE_C"].To<long>(0);

                        contract.MANAGEENGINEER = dt.Rows[0]["GO_BOGEN"].ToString();
                        contract.MANAGEENGINEERSTARTDATE  = dt.Rows[0]["GO_SYYYYMM3"].ToString();
                        contract.MANAGEENGINEERCOUNT = dt.Rows[0]["GO_BOGEN_C"].To<long>(0);

                        contract.VISITWEEK = dt.Rows[0]["SET_WEEK"].ToString();
                        contract.VISITDAY = dt.Rows[0]["SET_YOIL"].ToString();
                        contract.DECLAREDAY = dt.Rows[0]["SET_SUNIMDAY"].ToString();

                        contract.CONTRACTSTARTDATE = dt.Rows[0]["SET_GEYAKDAY1"].ToString();
                        contract.CONTRACTENDDATE = dt.Rows[0]["SET_GEYAKDAY2"].ToString();

                        if (!contract.IsNullOrEmpty())
                        {
                            if (maxList[0].YYYYMM.To<int>(0) > contract.CONTRACTSTARTDATE.Substring(0, 4).To<int>(0))
                            {
                                contract.CONTRACTSTARTDATE = maxList[0].YYYYMM.Substring(0,4) + "-01-01";
                                contract.CONTRACTENDDATE = maxList[0].YYYYMM.Substring(0, 4) + "-12-31";
                            }
                        }
                        else
                        {
                            contract.CONTRACTSTARTDATE = maxList[0].YYYYMM.Substring(0, 4) + "-01-01";
                            contract.CONTRACTENDDATE = maxList[0].YYYYMM.Substring(0,4) + "-12-31";
                        }

                        contract.COMMISSION = dt.Rows[0]["SET_TAX"].To<long>(0);

                        contract.POSITION = dt.Rows[0]["SET_PLACE"].ToString(); // 위치

                        contract.ISROTATION = dt.Rows[0]["SET_CHANGE"].ToString(); // 고대제유무
                        contract.BUILDINGTYPE = dt.Rows[0]["SET_HOME"].ToString(); // 건물소유
                        contract.ISPRODUCTTYPE = dt.Rows[0]["SET_MAKE"].ToString(); // 생산방식
                        contract.ISLABOR = dt.Rows[0]["SET_JOHAP"].ToString(); // 노동조합

                        contract.WORKSTARTTIME = dt.Rows[0]["WORK_STIME"].ToString(); // 시작시간
                        if (contract.WORKSTARTTIME == "24:00")
                        {
                            contract.WORKSTARTTIME = "00:00";
                        }
                        contract.WORKENDTIME = dt.Rows[0]["WORK_ETIME"].ToString(); // 종료시간
                        if (contract.WORKENDTIME == "24:00")
                        {
                            contract.WORKENDTIME = "00:00";
                        }
                        contract.WORKMEETTIME = dt.Rows[0]["WORK_JTIME"].ToString(); // 조회시간
                        if (contract.WORKMEETTIME == "24:00")
                        {
                            contract.WORKMEETTIME = "00:00";
                        }
                        contract.WORKROTATIONTIME = dt.Rows[0]["WORK_CTIME"].ToString(); // 교대시간
                        contract.WORKLUANCHTIME = dt.Rows[0]["WORK_LTIME"].ToString(); // 점심시간
                        contract.WORKRESTTIME = dt.Rows[0]["WORK_RTIME"].ToString(); // 휴식시간
                        contract.WORKEDUTIME = dt.Rows[0]["WORK_EDTIME"].ToString(); // 교육시간
                        contract.WORKETCTIME = dt.Rows[0]["WORK_ETCTIME"].ToString(); // 기타시간



                        //
                        List<HC_OSHA_CONTRACT_MANAGER_MODEL> managerList = new List<HC_OSHA_CONTRACT_MANAGER_MODEL>();
                        for(int i=1; i<=3; i++)
                        {
                            int num = i * 10;
                            if (dt.Rows[0]["SET_SDAM" + (num + 1)] != null)
                            {
                                //안전담당자
                                HC_SITE_WORKER worker = new HC_SITE_WORKER();
                                worker.SITEID = model.ID;
                                worker.NAME = dt.Rows[0]["SET_SDAM" + (num + 1)].ToString().Trim();
                                worker.DEPT = dt.Rows[0]["SET_SDAM" + (num + 2)].ToString();
                                worker.EMAIL = dt.Rows[0]["SET_SDAM" + (num + 4)].ToString();
                                worker.HP = dt.Rows[0]["SET_SDAM" + (num + 5)].ToString();
                                worker.ISDELETED = "N";
                                worker.JUMIN = "";
                                worker.PANO = 0;
                                worker.PTNO = "";
                                worker.WORKER_ROLE = "SAFE_ROLE";
                                worker.IPSADATE = "";
                                if (worker.NAME.NotEmpty() && worker.NAME != "-")
                                {
                                    //List<HC_SITE_WORKER> workerList =  hcSiteWorkerRepository.FindAllByName(model.ID, worker.NAME.Trim());;
                                    //if (workerList.Count <= 0)
                                    //{
                                    //    worker = hcSiteWorkerRepository.Insert(worker);
                                    //}
                                    //else
                                    //{
                                    //    worker = workerList[0];
                                    //}
                                    HC_OSHA_CONTRACT_MANAGER_MODEL s1 = new HC_OSHA_CONTRACT_MANAGER_MODEL();
                                   // s1.WORKER_ID = worker.ID;
                                    s1.ESTIMATE_ID = estimateId;
                                    s1.WORKER_ROLE = worker.WORKER_ROLE;
                                    s1.NAME = worker.NAME;
                                    s1.DEPT = worker.DEPT;
                                    s1.TEL = worker.TEL;
                                    s1.HP = worker.HP;
                                    s1.EMAIL = worker.EMAIL;
                                    s1.RowStatus = RowStatus.Insert;
                                    managerList.Add(s1);
                                   
                                }
                             
                            }
                        }
                        hcOshaContractManagerService.Save(managerList);

                        List<HC_OSHA_CONTRACT_MANAGER_MODEL> managerList2 = new List<HC_OSHA_CONTRACT_MANAGER_MODEL>();
                        for (int i = 1; i <= 3; i++)
                        {
                            int num = i * 10;
                            if (dt.Rows[0]["SET_BDAM" + (num + 1)] != null)
                            {
                                //보건담당자
                                HC_SITE_WORKER worker = new HC_SITE_WORKER();
                                worker.SITEID = model.ID;
                                worker.NAME = dt.Rows[0]["SET_BDAM" + (num + 1)].ToString().Trim();
                                worker.DEPT = dt.Rows[0]["SET_BDAM" + (num + 2)].ToString();
                                worker.EMAIL = dt.Rows[0]["SET_BDAM" + (num + 4)].ToString();
                                worker.HP = dt.Rows[0]["SET_BDAM" + (num + 5)].ToString();
                                worker.ISDELETED = "N";
                                worker.JUMIN = "";
                                worker.PANO = 0;
                                worker.PTNO = "";
                                
                                worker.IPSADATE = "";
                                if (worker.NAME.NotEmpty() && worker.NAME != "-")
                                {
                                    //List<HC_SITE_WORKER> workerList = hcSiteWorkerRepository.FindAllByName(model.ID, worker.NAME.Trim());
                                    //if (workerList.Count <= 0)
                                    //{
                                    //    worker = hcSiteWorkerRepository.Insert(worker);
                                    //}
                                    //else
                                    //{
                                    //    worker = workerList[0];
                                    //}
                                    worker.WORKER_ROLE = "HEALTH_ROLE";
                                    HC_OSHA_CONTRACT_MANAGER_MODEL s1 = new HC_OSHA_CONTRACT_MANAGER_MODEL();
                                  //  s1.WORKER_ID = worker.ID;
                                    s1.ESTIMATE_ID = estimateId;
                                    s1.WORKER_ROLE = worker.WORKER_ROLE;
                                    s1.NAME = worker.NAME;
                                    s1.DEPT = worker.DEPT;
                                    s1.TEL = worker.TEL;
                                    s1.HP = worker.HP;
                                    s1.EMAIL = worker.EMAIL;
                                    s1.RowStatus = RowStatus.Insert;
                                    managerList2.Add(s1);

                                }

                            }

                        }

                        hcOshaContractManagerService.Save(managerList2);

                        contract.SPECIALCONTRACT = dt.Rows[0]["ETCREMARK"].ToString(); // 특약사항

                        if (dt.Rows[0]["UNION_DAEPYO"].ToString() != "")
                        {
                            List<HC_OSHA_CONTRACT_MANAGER_MODEL> managerList11 = new List<HC_OSHA_CONTRACT_MANAGER_MODEL>();
                            //노동조합대표
                            HC_SITE_WORKER worker = new HC_SITE_WORKER();
                            worker.SITEID = model.ID;
                            worker.NAME = dt.Rows[0]["UNION_DAEPYO"].ToString().Trim();
                            worker.DEPT = "노동조합 대표";
                            worker.EMAIL = "";
                            worker.HP = "";
                            worker.ISDELETED = "N";
                            worker.JUMIN = "";
                            worker.PANO = 0;
                            worker.PTNO = "";
                            worker.WORKER_ROLE = "LABOR_ROLE";
                            worker.IPSADATE = "";

                            if (worker.NAME.NotEmpty() && worker.NAME != "-")
                            {
                                //List<HC_SITE_WORKER> workerList =  hcSiteWorkerRepository.FindAllByName(model.ID, worker.NAME.Trim());;
                                //if (workerList.Count <= 0)
                                //{
                                //    worker = hcSiteWorkerRepository.Insert(worker);
                                //}
                                //else
                                //{
                                //    worker = workerList[0];
                                //}
                                worker.WORKER_ROLE = "LABOR_ROLE";
                                HC_OSHA_CONTRACT_MANAGER_MODEL s1 = new HC_OSHA_CONTRACT_MANAGER_MODEL();
                                   // s1.WORKER_ID = worker.ID;
                                    s1.ESTIMATE_ID = estimateId;
                                    s1.WORKER_ROLE = worker.WORKER_ROLE;
                                    s1.NAME = worker.NAME;
                                    s1.DEPT = worker.DEPT;
                                    s1.TEL = worker.TEL;
                                    s1.HP = worker.HP;
                                    s1.EMAIL = worker.EMAIL;
                                s1.RowStatus = RowStatus.Insert;
                                managerList11.Add(s1);
                                 
                                

                            }
                            hcOshaContractManagerService.Save(managerList11);

                        }

                        if (dt.Rows[0]["WORK_DAEPYO"].ToString() != "")
                        {
                            List<HC_OSHA_CONTRACT_MANAGER_MODEL> managerList11 = new List<HC_OSHA_CONTRACT_MANAGER_MODEL>();
                            //근로자대표
                            HC_SITE_WORKER worker = new HC_SITE_WORKER();
                            worker.SITEID = model.ID;
                            worker.NAME = dt.Rows[0]["WORK_DAEPYO"].ToString().Trim();
                            worker.DEPT = dt.Rows[0]["WORK_DAEPYO_BUSE"].ToString().Trim();
                            worker.EMAIL = "";
                            worker.HP = "";
                            worker.ISDELETED = "N";
                            worker.JUMIN = "";
                            worker.PANO = 0;
                            worker.PTNO = "";
                            worker.WORKER_ROLE = "WM_ROLE";
                            worker.IPSADATE = "";
                            if (worker.NAME.NotEmpty() && worker.NAME != "-")
                            {
                                //List<HC_SITE_WORKER> workerList =  hcSiteWorkerRepository.FindAllByName(model.ID, worker.NAME.Trim());;
                                //if (workerList.Count <= 0)
                                //{
                                //    worker = hcSiteWorkerRepository.Insert(worker);
                                //}
                                //else
                                //{
                                //    worker = workerList[0];
                                //}
                                worker.WORKER_ROLE = "WM_ROLE";
                                HC_OSHA_CONTRACT_MANAGER_MODEL s1 = new HC_OSHA_CONTRACT_MANAGER_MODEL();
                                  //  s1.WORKER_ID = worker.ID;
                                    s1.ESTIMATE_ID = estimateId;
                                    s1.WORKER_ROLE = worker.WORKER_ROLE;
                                    s1.NAME = worker.NAME;
                                    s1.DEPT = worker.DEPT;
                                    s1.TEL = worker.TEL;
                                    s1.HP = worker.HP;
                                    s1.EMAIL = worker.EMAIL;
                                s1.RowStatus = RowStatus.Insert;
                                managerList11.Add(s1);
                                  
                                
                            }
                            hcOshaContractManagerService.Save(managerList11);
                        }
                        if (dt.Rows[0]["HONOR_DAEPYO"].ToString() != "")
                        {
                            List<HC_OSHA_CONTRACT_MANAGER_MODEL> managerList11 = new List<HC_OSHA_CONTRACT_MANAGER_MODEL>();
                            //명예산업안전감독관
                            HC_SITE_WORKER worker = new HC_SITE_WORKER();
                            worker.SITEID = model.ID;
                            worker.NAME = dt.Rows[0]["HONOR_DAEPYO"].ToString().Trim();
                            worker.DEPT = dt.Rows[0]["HONOR_DAEPYO_BUSE"].ToString().Trim();
                            worker.EMAIL = "";
                            worker.HP = "";
                            worker.ISDELETED = "N";
                            worker.JUMIN = "";
                            worker.PANO = 0;
                            worker.PTNO = "";
                            worker.WORKER_ROLE = "DIRECTOR_ROLE";
                            worker.IPSADATE = "";
                            if (worker.NAME.NotEmpty() && worker.NAME != "-")
                            {
                                //List<HC_SITE_WORKER> workerList =  hcSiteWorkerRepository.FindAllByName(model.ID, worker.NAME.Trim());;
                                //if (workerList.Count <= 0)
                                //{
                                //    worker = hcSiteWorkerRepository.Insert(worker);
                                //}
                                //else
                                //{
                                //    worker = workerList[0];
                                //}
                                worker.WORKER_ROLE = "DIRECTOR_ROLE";

                                HC_OSHA_CONTRACT_MANAGER_MODEL s1 = new HC_OSHA_CONTRACT_MANAGER_MODEL();
                               // s1.WORKER_ID = worker.ID;
                                s1.ESTIMATE_ID = estimateId;
                                s1.WORKER_ROLE = worker.WORKER_ROLE;
                                s1.NAME = worker.NAME;
                                s1.DEPT = worker.DEPT;
                                s1.TEL = worker.TEL;
                                s1.HP = worker.HP;
                                s1.EMAIL = worker.EMAIL;
                                s1.RowStatus = RowStatus.Insert;
                                managerList11.Add(s1);
                              
                                
                            }
                            hcOshaContractManagerService.Save(managerList11);

                        }

                        contract.ISWEM = dt.Rows[0]["ADDITEM_1"].ToString();
                        contract.ISWEMDATA = dt.Rows[0]["ADDITEM_1_DATA"].ToString();

                        contract.ISCOMMITTEE = dt.Rows[0]["ADDITEM_2"].ToString();

                        contract.ISSKELETON = dt.Rows[0]["ADDITEM_3"].ToString();
                        contract.ISSKELETONDATE = dt.Rows[0]["ADDITEM_3_DATA"].ToString();

                        contract.ISSPACEPROGRAM = dt.Rows[0]["ADDITEM_4"].ToString();
                        contract.ISSPACEPROGRAMDATE = dt.Rows[0]["ADDITEM_4_DATA"].ToString();

                        contract.ISEARPROGRAM = dt.Rows[0]["ADDITEM_5"].ToString();
                        contract.ISEARPROGRAMDATE = dt.Rows[0]["ADDITEM_5_DATA"].ToString();

                        contract.ISSTRESS = dt.Rows[0]["ADDITEM_6"].ToString();
                        contract.ISSTRESSDATE = dt.Rows[0]["ADDITEM_6_DATA"].ToString();

                        contract.ISBRAINTEST = dt.Rows[0]["ADDITEM_7"].ToString();
                        contract.ISBRAINTESTDATE = dt.Rows[0]["ADDITEM_7_DATA"].ToString();

                        contract.ISSPECIAL = dt.Rows[0]["ADDITEM_8"].ToString();
                        contract.ISSPECIALDATA = dt.Rows[0]["ADDITEM_8_DATA"].ToString();

                        contract.ISCONTRACT = "Y";
                        contract.CONTRACTDATE = contract.CONTRACTSTARTDATE;

                        if(contract.OSHA_SITE_ID == 8865)
                        {
                            string sadawd = "";
                        }
                        //저장
                        hcOshaContractService.Save(contract);

                   

                    }
                    dt.Dispose();
                    dt = null;
                }
                catch (Exception ex)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox(ex.Message);
                }


            }//for
            Cursor.Current = Cursors.Default;
            MessageUtil.Info("사업자 현황 완료");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            long estimateId = 0;
            SQL = " ";
            SQL = SQL + ComNum.VBLF + "   select trim(ltdcode) as ltdcode, trim(yyyymm) year, image from HIC_BOGEN_DATA ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
            }
            HcOshaCard21Service service = new HcOshaCard21Service();
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                long ltdcode = dt.Rows[i]["ltdcode"].To<long>(0);
                string year = dt.Rows[i]["year"].ToString();
                byte[] image = dt.Rows[i]["image"] as byte[];

                HC_OSHA_CARD22 dto = new HC_OSHA_CARD22();
                dto.SITE_ID = ltdcode;
                dto.ESTIMATE_ID = GetMaxEstimateId(ltdcode);
                dto.ImageData = image;
                dto.YEAR = base.GetCurrentYear();
                service.SaveImage(dto);

            }

            MessageUtil.Info("완료");
        }

        private string GetUserId(string name)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SQL = " ";
            SQL = " ";
            SQL = SQL + ComNum.VBLF + "   SELECT userId FROM HIC_USERS ";
            SQL = SQL + ComNum.VBLF + "  WHERE NAME = '" + name + "' ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
            }
         
            string userid = dt.Rows[0]["USERID"].ToString();
            return userid;
        }



        /// <summary>
        /// truncate  table  HIC_OSHA_SCHEDULE
        /// truncate table  HIC_OSHA_VISIT
        /// truncate  table HIC_OSHA_VISIT_PRICE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSchedule_Click(object sender, EventArgs e)
        {

            // clsDB.setBeginTran(clsDB.DbCon);
            HcOshaScheduleService hcOshaScheduleService = new HcOshaScheduleService();
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            long estimateId = 0;
            string startDate = "2020-04-01";
            string endDate = "2021-12-30";
            DateTime? VISITDATE = null;
            string VISITsTIME = "";
            string VISITeTIME = "";

            SQL = " ";
            SQL = SQL + ComNum.VBLF + "  SELECT Rdate, trim(LtdCode) as LtdCode, Rtime, Inwon, STime, STime1, Man, trim(MAN_SABUN) as MAN_SABUN, trim(Man1) as Man1,  Remark, trim(JobSabun) as JobSabun,  ";
            SQL = SQL + ComNum.VBLF + "  TO_CHAR(EntTime, 'YYYY-MM-DD HH24:MI') EntTime,GbChange, ROWID    ";
            SQL = SQL + ComNum.VBLF + "  FROM HIC_BORESV    ";
            SQL = SQL + ComNum.VBLF + "  WHERE Rdate >= TO_DATE('" + startDate + "', 'YYYY-MM-DD')    ";
            SQL = SQL + ComNum.VBLF + "  AND Rdate<= TO_DATE('" + endDate + "', 'YYYY-MM-DD')    ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY Rdate,RTime    ";


            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
            }
          
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {

                    HC_OSHA_SCHEDULE dto = new HC_OSHA_SCHEDULE();
                    dto.SITE_ID = dt.Rows[i]["LtdCode"].To<long>(0);
                    dto.ESTIMATE_ID = GetMaxEstimateId(dto.SITE_ID);
                    VISITDATE = Convert.ToDateTime(dt.Rows[i]["Rdate"]);

                    VISITsTIME = dt.Rows[i]["Rtime"].ToString().Substring(0, 5);
                    VISITeTIME = dt.Rows[i]["Rtime"].ToString().Substring(6, 5);

                    dto.EVENTSTARTDATETIME = DateUtil.stringToDateTime(DateUtil.DateTimeToStrig(VISITDATE, DateTimeType.YYYY_MM_DD) + " " + VISITsTIME, DateTimeType.YYYY_MM_DD_HH_MM);
                    if (VISITeTIME.Equals("10\"00"))
                    {
                        VISITeTIME = "10:00";
                    }
                    dto.EVENTENDDATETIME = DateUtil.stringToDateTime(DateUtil.DateTimeToStrig(VISITDATE, DateTimeType.YYYY_MM_DD) + " " + VISITeTIME, DateTimeType.YYYY_MM_DD_HH_MM);
                    dto.VISITRESERVEDATE = VISITDATE;
                    dto.VISITSTARTTIME = VISITsTIME;
                    dto.VISITENDTIME = VISITeTIME;

                    dto.DEPARTUREDATETIME = dt.Rows[i]["STime"].ToString();
                    dto.ARRIVALTIME = dt.Rows[i]["STime1"].ToString();

                    //방문자
                    dto.VISITUSERID = dt.Rows[i]["MAN_SABUN"].ToString();
                    dto.VISITUSERNAME = dt.Rows[i]["Man"].ToString();

                    //동행자
                    dto.VISITMANAGERNAME = dt.Rows[i]["Man1"].ToString();
                    if (!dto.VISITMANAGERNAME.IsNullOrEmpty())
                    {
                        dto.VISITMANAGERID =  GetUserId(dto.VISITMANAGERNAME.Trim());
                    }

                    
                    dto.WORKERCOUNT = dt.Rows[i]["Inwon"].ToString().To<long>(0);
                    dto.GBCHANGE = dt.Rows[i]["GbChange"].ToString();
                    dto.REMARK = dt.Rows[i]["Remark"].ToString();

                    if (dto.SITE_ID == 8865)
                    {
                        string x = "";
                    }

                    hcOshaScheduleService.Save(dto);
                 
                }
                catch (Exception Ex)
                {
                  
                   // MessageUtil.Alert(" 방문예정일 작업 실패 " + Ex.Message);
                }

             
            }//
            MessageUtil.Alert(" 방문 예정 완료");
            //     rtnVal = dt.Rows[0]["KTASLEVL"].ToString().Trim();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            long estimateId = 0;
            DateTime? VISITDATE = null;
            string VISITsTIME = "";
            string VISITeTIME = "";

            SQL = " ";


            SQL = SQL + ComNum.VBLF + " select A.osha_site_id, bb.* from hic_osha_estimate a   ";
            SQL = SQL + ComNum.VBLF + " inner     join (   ";
            SQL = SQL + ComNum.VBLF + " select * from HIC_OSHA_CONTRACT_MANAGER   ";
            SQL = SQL + ComNum.VBLF + " where id in (";
            SQL = SQL + ComNum.VBLF + " select  min(id) from HIC_OSHA_CONTRACT_MANAGER   ";
            SQL = SQL + ComNum.VBLF + " where worker_role = 'HEALTH_ROLE'   ";
            SQL = SQL + ComNum.VBLF + " and isdeleted = 'N'   ";
            SQL = SQL + ComNum.VBLF + " group by estimate_id   ";
            SQL = SQL + ComNum.VBLF + " )    ";
            SQL = SQL + ComNum.VBLF + " ) bb   ";
            SQL = SQL + ComNum.VBLF + " on a.id = bb.estimate_id   ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
            }
            int totalcount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                long siteid = dt.Rows[i]["osha_site_id"].ToString().To<long>(0);
                string sname = dt.Rows[i]["name"].ToString();
                string email = dt.Rows[i]["email"].ToString();

                totalcount= totalcount + updatePatient(email, sname, siteid);
            }
            MessageUtil.Alert("이메일 업데이티 완료 totalcount:" + totalcount);
        }

        private int updatePatient(string email, string name, long ltdcode)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int count = 0;
            SQL = " ";
            SQL = SQL + ComNum.VBLF + " update KOSMOS_PMPA.HIC_PATIENT ";
            SQL = SQL + ComNum.VBLF + " set email = '"+email+"'";
            SQL = SQL + ComNum.VBLF + " , WORKER_ROLE = 'HEALTH_ROLE'";
            SQL = SQL + ComNum.VBLF + " where ltdcode = "+ ltdcode;
            SQL = SQL + ComNum.VBLF + " and trim(sname) = trim('"+name+"')";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref count, clsDB.DbCon);

            if(count == 0)
            {
                saveWorker(name, email, ltdcode);
            }
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
            }

            return count;

        }
        private void saveWorker(string name, string email, long siteid)
        {
            HC_SITE_WORKER worker = new HC_SITE_WORKER();
            worker.SITEID = siteid;
            worker.NAME = name;
            worker.DEPT = "";
            worker.EMAIL = email;
            worker.HP = "";
            worker.ISDELETED = "N";
            worker.JUMIN = "";
            worker.PANO = 0;
            worker.PTNO = "";
            worker.WORKER_ROLE = "HEALTH_ROLE";
            worker.IPSADATE = "";
            hcSiteWorkerRepository.Insert(worker);
        }
  
       private void button4_Click(object sender, EventArgs e)
        {
            HealthCheckRepository healthCheckRepository = new HealthCheckRepository();

            DataSyncRepository dataSyncRepository = new DataSyncRepository();

            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            long estimateId = 0;
            DateTime? VISITDATE = null;
            string VISITsTIME = "";
            string VISITeTIME = "";

            SQL = " ";

            SQL = SQL + ComNum.VBLF + "      select * From HIC_OSHA_DATASYNC   ";
            SQL = SQL + ComNum.VBLF + " where  tablename = 'HIC_OSHA_HEALTHCHECK'  ";
            SQL = SQL + ComNum.VBLF + " and dmltype = 'I'  ";
            SQL = SQL + ComNum.VBLF + " and issync = 'Y'  ";
            SQL = SQL + ComNum.VBLF + " and newtablekey is not null  ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                long id = dt.Rows[i]["NEWTABLEKEY"].To<long>(0);
                if (id > 0)
                {
                    HealthCheckDto dto =  healthCheckRepository.FindOne(id);
                    dataSyncRepository.UpdateHealthcheckWorker(dto.worker_id, dto.CREATEDUSER, dto.id);
                    //dataSyncRepository.UpdateHealthcheckWorker("P8480", "28638", 540);
                }

            }

            
        }
    }
}
