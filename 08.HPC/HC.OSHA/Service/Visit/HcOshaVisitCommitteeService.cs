namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using System;
    using ComBase;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaVisitCommitteeService
    {
        
        private HcOshaVisitCommitteeRepository hcOshaVisitCommitteeRepository;
        

        public void Save(HC_OSHA_VISIT_COMMITTEE dto)
        {
            hcOshaVisitCommitteeRepository.Insert(dto);
        }
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
                        hcOshaVisitCommitteeRepository.Update(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcOshaVisitCommitteeRepository.Delete(dto.ID);
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
