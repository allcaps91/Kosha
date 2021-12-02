namespace HC.OSHA.Visit.Management.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Visit.Management.Repository;
    using HC.OSHA.Visit.Management.Dto;
    using System;
    using ComBase;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaVisitInformationService
    {
        
        private HcOshaVisitInformationRepository hcOshaVisitInformationRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaVisitInformationService()
        {
			this.hcOshaVisitInformationRepository = new HcOshaVisitInformationRepository();
        }

        public List<HC_OSHA_VISIT_INFORMATION> FindAll(long siteId)
        {
            return hcOshaVisitInformationRepository.FindAll(siteId);
        }
        public bool Save(IList<HC_OSHA_VISIT_INFORMATION> list, long siteId)
        {
            try
            {
                foreach (HC_OSHA_VISIT_INFORMATION dto in list)
                {

                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)

                    {
                        dto.SITE_ID = siteId;
                        hcOshaVisitInformationRepository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcOshaVisitInformationRepository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcOshaVisitInformationRepository.Delete(dto);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }
    }
}
