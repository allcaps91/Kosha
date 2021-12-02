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
    public class HcOshaVisitEduService
    {
        
        private HcOshaVisitEduRepository hcOshaVisitEduRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaVisitEduService()
        {
			this.hcOshaVisitEduRepository = new HcOshaVisitEduRepository();
        }

        public List<HC_OSHA_VISIT_EDU> FindAll(long siteId)
        {
           return hcOshaVisitEduRepository.FindAll(siteId);
        }
        public bool Save(IList<HC_OSHA_VISIT_EDU> list, long siteId)
        {
            try
            {
                foreach (HC_OSHA_VISIT_EDU dto in list)
                {
                    
                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)

                    {
                        dto.SITE_ID = siteId;
                        hcOshaVisitEduRepository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcOshaVisitEduRepository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcOshaVisitEduRepository.Delete(dto);
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
