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
    public class HcOshaVisitCommitteeService
    {
        
        private HcOshaVisitCommitteeRepository hcOshaVisitCommitteeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaVisitCommitteeService()
        {
			this.hcOshaVisitCommitteeRepository = new HcOshaVisitCommitteeRepository();
        }
        public List<HC_OSHA_VISIT_COMMITTEE> FindAll(long siteId)
        {
            return hcOshaVisitCommitteeRepository.FindAll(siteId);
        }
        public bool Save(IList<HC_OSHA_VISIT_COMMITTEE> list, long siteId)
        {
            try
            {
                foreach (HC_OSHA_VISIT_COMMITTEE dto in list)
                {

                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)

                    {
                        dto.SITE_ID = siteId;
                        hcOshaVisitCommitteeRepository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcOshaVisitCommitteeRepository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcOshaVisitCommitteeRepository.Delete(dto);
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
