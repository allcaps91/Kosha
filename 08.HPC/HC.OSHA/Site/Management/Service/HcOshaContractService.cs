namespace HC.OSHA.Site.Management.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Site.Management.Repository;
    using HC.OSHA.Site.Management.Dto;
    using HC.Core.Site.Dto;

    /// <summary>
    /// 보건관리 계약 서비스
    /// </summary>
    public class HcOshaContractService
    {
        
        private HcOshaContractRepository hcOshaContractRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaContractService()
        {
			this.hcOshaContractRepository = new HcOshaContractRepository();
        }

        public HC_OSHA_CONTRACT FindByEstimateId(long estimateId)
        {
            return hcOshaContractRepository.FindByEstimateId(estimateId);
        }
        public void Delete(long estimateId)
        {
            hcOshaContractRepository.Delete(estimateId);
        }
        public HC_OSHA_CONTRACT Save(HC_OSHA_CONTRACT dto)
        {
            dto.Validate();

            HC_OSHA_CONTRACT saved =  hcOshaContractRepository.FindByEstimateId(dto.ESTIMATE_ID);
            if(saved == null)
            {
                return hcOshaContractRepository.Insert(dto);
            }
            else
            {
                if(dto.ISDELETED == ComBase.Mvc.Enums.IsDeleted.Y)
                {
                    dto.ISDELETED = ComBase.Mvc.Enums.IsDeleted.N;
                }
                return hcOshaContractRepository.Update(dto);
            }
            
        }
    }
}
