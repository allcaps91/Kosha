namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using System;
    using ComBase;
    using HC.Core.Service;
    using HC.Core.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaVisitEduService
    {
        
        public HcOshaVisitEduRepository hcOshaVisitEduRepository { get;  }

        private HcUserService hcUserService { get; }
        /// <summary>
        /// 
        /// </summary>
        public HcOshaVisitEduService()
        {
			this.hcOshaVisitEduRepository = new HcOshaVisitEduRepository();
            this.hcUserService = new HcUserService();
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
                    HC_USER user = hcUserService.FindByUserId(dto.EDUUSERID);
                    dto.EDUUSERNAME = user.Name;
                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)

                    {
                        dto.SITE_ID = siteId;
                      
                        hcOshaVisitEduRepository.Insert(dto);
                        

                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {

                            hcOshaVisitEduRepository.Update(dto);
                        
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcOshaVisitEduRepository.Delete(dto.ID);
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
