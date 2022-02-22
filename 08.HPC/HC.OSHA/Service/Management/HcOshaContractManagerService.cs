namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using HC.OSHA.Model;
    using ComBase.Controls;
    using HC.Core.Repository;
    using HC.Core.Dto;



    /// <summary>
    /// 사업장 담당자 관리
    /// </summary>
    public class HcOshaContractManagerService
    {
        
        public HcOshaContractManagerRepository hcOshaContractManagerRepository { get; }
        private HcSiteWorkerRepository hcSiteWorkerRepository;

        /// <summary>
        /// 
        /// </summary>
        public HcOshaContractManagerService()
        {
			hcOshaContractManagerRepository = new HcOshaContractManagerRepository();
            hcSiteWorkerRepository = new HcSiteWorkerRepository();
        }


        /// <summary>
        /// 사업장 소속된 전체 근로자를 가져온다.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public List<HC_OSHA_CONTRACT_MANAGER_MODEL> FindSiteWorker(long siteId, long estimateId)
        {
            List<HC_OSHA_CONTRACT_MANAGER_MODEL> managerList = new List<HC_OSHA_CONTRACT_MANAGER_MODEL>();

            List<HC_SITE_WORKER> list = hcSiteWorkerRepository.FindWorkerRole(siteId);
            foreach(HC_SITE_WORKER dto in list)
            {
                HC_OSHA_CONTRACT_MANAGER_MODEL model = new HC_OSHA_CONTRACT_MANAGER_MODEL()
                {
                    RowStatus = ComBase.Mvc.RowStatus.Insert,
                    ESTIMATE_ID = estimateId,                    
                    DEPT = dto.DEPT,
                    EMAIL = dto.EMAIL,
                    HP = dto.HP,
                    TEL = dto.TEL,
                    NAME = dto.NAME,
                    WORKER_ID = dto.ID,
                    WORKER_ROLE = dto.WORKER_ROLE
                    
                };
                managerList.Add(model);
            }

            return managerList;
        }
        /// <summary>
        /// 계약에 작성된 사업자 근로자를 가져온다
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="estimateId"></param>
        /// <returns></returns>
        public List<HC_OSHA_CONTRACT_MANAGER_MODEL> FindContractManager(long estimateId)
        {
            return hcOshaContractManagerRepository.FindContractManager(estimateId);
        }
        public void Save(IList<HC_OSHA_CONTRACT_MANAGER_MODEL> list)
        {
            foreach(HC_OSHA_CONTRACT_MANAGER_MODEL model in list)
            {
                if (model.WORKER_ROLE.NotEmpty())
                {
                    HC_OSHA_CONTRACT_MANAGER dto = new HC_OSHA_CONTRACT_MANAGER()
                    {
                        ID = model.ID,
                        ESTIMATE_ID = model.ESTIMATE_ID,
                        //   WORKER_ID = model.WORKER_ID,
                        WORKER_ROLE = model.WORKER_ROLE,
                        NAME = model.NAME,
                        DEPT = model.DEPT,
                        TEL = model.TEL,
                        HP = model.HP,
                        EMAIL = model.EMAIL,
                        EMAILSEND = model.EMAILSEND,
                        ISDELETED = "N"
                    };


                    if (model.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        this.hcOshaContractManagerRepository.Insert(dto);
                    }
                    else if (model.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        this.hcOshaContractManagerRepository.Update(dto);
                    }
                    else if (model.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        this.hcOshaContractManagerRepository.Delete(dto.ID);
                    }
                }
             
               
            }
            

        }
    }
}
