namespace HC.OSHA.Site.Management.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Site.Management.Repository;
    using HC.OSHA.Site.Management.Dto;
    using ComBase;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HcSiteWorkerService
    {
        
        private HcSiteWorkerRepository hcSiteWorkerRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcSiteWorkerService()
        {
			this.hcSiteWorkerRepository = new HcSiteWorkerRepository();
        }

        public List<HC_SITE_WORKER> FindAll(long siteId)
        {
            return this.hcSiteWorkerRepository.FindAll(siteId);
        }

        public void Delete(long id)
        {
            this.hcSiteWorkerRepository.Delete(id);
        }

        public bool Save(long siteId, IList<HC_SITE_WORKER> list)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HC_SITE_WORKER dto in list)
                {
                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        dto.SITEID = siteId;
                        hcSiteWorkerRepository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcSiteWorkerRepository.Update(dto);
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
    }
}
