namespace HC.OSHA.Site.Management.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Site.Management.Repository;
    using HC.OSHA.Site.Management.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HcOshaEstimateService
    {
        
        private HcOshaEstimateRepository hcOshaEstimateRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaEstimateService()
        {
			this.hcOshaEstimateRepository = new HcOshaEstimateRepository();
        }
        public HC_OSHA_ESTIMATE FindById(long id)
        {
           return  hcOshaEstimateRepository.FindById(id);
        }
        public HC_OSHA_ESTIMATE Save(HC_OSHA_ESTIMATE dto)
        {
            dto.Validate();
            if(dto.ID == 0)
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
