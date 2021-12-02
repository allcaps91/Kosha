namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using HC.Core.Service;
    using static HC.Core.Service.LogService;
    using HC.Core.Dto;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaEstimateService
    {
        
        public HcOshaEstimateRepository hcOshaEstimateRepository { get;  }
        private HcOshaSiteService hcOShaSiteService;
        private HcSiteViewService hcSiteViewService;
        /// <summary>
        /// 
        /// </summary>
        public HcOshaEstimateService()
        {
			this.hcOshaEstimateRepository = new HcOshaEstimateRepository();
            hcOShaSiteService = new HcOshaSiteService();
            hcSiteViewService = new HcSiteViewService();
        }
        public HC_OSHA_ESTIMATE FindById(long id)
        {
           return  hcOshaEstimateRepository.FindById(id);
        }
        public HC_OSHA_ESTIMATE Save(HC_OSHA_ESTIMATE dto)
        {

            LogService.Instance.Task(dto.OSHA_SITE_ID, TaskName.ESTIMATE);
            HC_SITE_VIEW view = hcSiteViewService.FindById(dto.OSHA_SITE_ID);
            hcOShaSiteService.Save(view);            

            if (dto.ID == 0)
            {
               return  hcOshaEstimateRepository.Insert(dto);

            }
            else
            {
                return hcOshaEstimateRepository.Update(dto);

            }

            
        }
        public void Delete(HC_OSHA_ESTIMATE dto)
        {
            hcOshaEstimateRepository.Delete(dto);
        }
    }
}
