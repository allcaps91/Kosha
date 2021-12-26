namespace HC.Core.Service
{
    using System.Collections.Generic;
    using ComBase;
    using System;
    using HC_Core.Service;
    using static HC.Core.Service.LogService;
    using HC.Core.Repository;
    using HC.Core.Model;
    using ComBase.Controls;
    using ComBase.Mvc.Utils;
    using HC.Core.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HcSiteWorkerService
    {
        public HcSiteWorkerRepository hcSiteWorkerRepository { get;  }
        private HealthCarePatientRepository healthCarePatientRepository;

        /// <summary>
        /// 
        /// </summary>
        public HcSiteWorkerService()
        {
			this.hcSiteWorkerRepository = new HcSiteWorkerRepository();
            healthCarePatientRepository = new HealthCarePatientRepository();
        }

        /// <summary>
        /// 사업장의 보건관리자 가져오기
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public HC_SITE_WORKER FindHealthRole(long siteId)
        {
            List<HC_SITE_WORKER> list = hcSiteWorkerRepository.FindWorkerByRole(siteId, "HEALTH_ROLE");
            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }
        public HC_SITE_WORKER CopyPatient(long siteId, string pano, string startDate, string endDate)
        {
            PatientModel model =  healthCarePatientRepository.FindOne(siteId, pano, startDate, endDate);
            if (model != null)
            {
                HC_SITE_WORKER worker = new HC_SITE_WORKER();
                worker.SITEID = siteId;
                worker.NAME = model.NAME;
                worker.DEPT = model.DEPT;
                worker.TEL = model.TEL;
                worker.ISDELETED = "N";
                worker.JUMIN = model.JUMIN;
                worker.PANO = long.Parse(model.PANO);
                worker.PTNO = model.PTNO;
                worker.WORKER_ROLE = "EMP_ROLE";
                worker.IPSADATE = model.IPSADATE;
                HC_SITE_WORKER saved = hcSiteWorkerRepository.FindByJumin(worker.JUMIN);
                if(saved == null)
                {
                    saved = hcSiteWorkerRepository.Insert(worker);
                }
                else
                {
                    worker.ID = saved.ID;
                    saved = hcSiteWorkerRepository.Update(worker);
                }

                return saved;
            }
            return null;
        }
        public void CopyHealthCheck(long siteId)
        {
            HcCodeService hcCodeService = new HcCodeService();
            DateTime currentDate = hcCodeService.CurrentDate;
            string startDate = currentDate.AddYears(-1).ToString("yyyy-MM-dd");
            string endDate = currentDate.ToString("yyyy-MM-dd");

            List<PatientModel> list = healthCarePatientRepository.FindAll(siteId, startDate, endDate);
            foreach(PatientModel model in list)
            {
                HC_SITE_WORKER worker = new HC_SITE_WORKER();
                worker.SITEID = siteId;
                worker.NAME = model.NAME;
                worker.DEPT = model.DEPT;
                worker.TEL = model.TEL;
                worker.ISDELETED = "N";
                worker.JUMIN = model.JUMIN;
                worker.PANO = long.Parse(model.PANO);
                worker.PTNO = model.PTNO;
                worker.WORKER_ROLE = "EMP_ROLE";
                worker.IPSADATE = model.IPSADATE;
                HC_SITE_WORKER saved = hcSiteWorkerRepository.FindByJumin(worker.JUMIN);
                if(saved == null)
                {
                    List<HC_SITE_WORKER> savedWorker = hcSiteWorkerRepository.FindAllByName(worker.SITEID, worker.NAME);
                    if (savedWorker.Count==1)
                    {
                        savedWorker[0].PANO = worker.PANO;
                        savedWorker[0].PTNO = worker.PTNO;
                        savedWorker[0].IPSADATE = model.IPSADATE;

                        hcSiteWorkerRepository.Update(savedWorker[0]);
                    }
                    else
                    {
                        hcSiteWorkerRepository.Insert(worker);
                    }

                }
                else
                {
                    saved.PANO = worker.PANO;
                    saved.PTNO = worker.PTNO;
                    saved.IPSADATE = model.IPSADATE;

                    hcSiteWorkerRepository.Update(saved);
                }
            }
        }
        
        public HC_SITE_WORKER FindOne(string id)
        {
            return hcSiteWorkerRepository.FindOne(id);
        }
        public HC_SITE_WORKER FindOneByPano(string pano)
        {
            return hcSiteWorkerRepository.FindOneByPano(pano);
        }
        public HC_SITE_WORKER FindOneByBirth(long siteid,string name,string birth)
        {
            return hcSiteWorkerRepository.FindOneByBirth(siteid, name, birth);
        }
        //public List<HC_SITE_WORKER> FindAll(long siteId, string dept)
        //{

        //    List<HC_SITE_WORKER> list = this.hcSiteWorkerRepository.FindAll(siteId, dept);
        //    foreach(HC_SITE_WORKER worker in list)
        //    {
        //        string jumin = clsAES.DeAES(worker.JUMIN);
        //        if (jumin.NotEmpty())
        //        {
        //            worker.JUMIN = jumin.Substring(0, 6) + "-" + jumin.Substring(7, 1);
        //        }


        //    }
        //    return list;
        //}


        public List<HC_SITE_WORKER> FindAll(long siteId, string name, string dept, string workerRole)
        {
            if(dept == "전체")
            {
                dept = string.Empty;
            }
            return this.hcSiteWorkerRepository.FindAll(siteId, name, dept, workerRole);
        }
        public void Delete(string id)
        {
            this.hcSiteWorkerRepository.Delete(id);
        }
        //public void Save(HealthCheckDto dto)
        //{
        //    /// opd 신환번호 생성 루틴 확인필요함
        //    HC_SITE_WORKER worker = new HC_SITE_WORKER();
        //    //worker.DEPT = dto.dept;
        //    //worker.NAME = dto.name;
        //    //worker.
        //    //hcSiteWorkerRepository.Insert()
        //}
        public bool Save(long siteId, IList<HC_SITE_WORKER> list)
        {
            try
            {
                LogService.Instance.Task(siteId, TaskName.CONTRACT);

                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HC_SITE_WORKER dto in list)
                {
                    if (!dto.JUMIN.IsNullOrEmpty())
                    {
                        if (!StringUtil.IsNumeric(dto.JUMIN))
                        {
                            dto.JUMIN = dto.JUMIN.Replace("-", "");
                            
                      //      dto.JUMIN = clsAES.AES(dto.JUMIN);
                        }
                    }
                    

                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        dto.SITEID = siteId;
                        dto.ISMANAGEOSHA = "N";
                        hcSiteWorkerRepository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        //  시퀸스발생이 변경되므로 수정
                        if(!dto.ID.IsNumeric())
                        {
                            hcSiteWorkerRepository.Update(dto);
                        }
                        else
                        {
                            hcSiteWorkerRepository.UpdatePatientWorkerRole(dto);
                        }
                        
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcSiteWorkerRepository.Delete(dto.ID);
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }
        }

        /// <summary>
        /// 중점관리대상 선정 및 취소
        /// </summary>
        /// <param name="workerId"></param>
        public bool UpdateManageOsha(string workerId, bool isChecked)
        {
            string isManageOsha = "N";
            if (isChecked)
            {
                isManageOsha = "Y";
            }

            HC_SITE_WORKER dto = FindOne(workerId);
            if(dto.ISMANAGEOSHA != isManageOsha)
            {
                dto.ISMANAGEOSHA = isManageOsha;

                if (dto.ID.Substring(0, 1) == "P")
                {
                    hcSiteWorkerRepository.Update(dto);
                }
                else
                {
                    hcSiteWorkerRepository.UpdatePatientWorkerRole(dto);
                }
                return true;
            }
            return false;
        }
    }
}
