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
    public class HcOshaVisitInformationService
    {
        
        private HcOshaVisitInformationRepository hcOshaVisitInformationRepository;
        private HcUserService hcUserService { get; }
        /// <summary>
        /// 
        /// </summary>
        public HcOshaVisitInformationService()
        {
			this.hcOshaVisitInformationRepository = new HcOshaVisitInformationRepository();
            this.hcUserService = new HcUserService();
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
                    HC_USER user = hcUserService.FindByUserId(dto.REGUSERID);
                    dto.REGUSERNAME = user.Name;
                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)

                    {
                        dto.SITE_ID = siteId;
                        hcOshaVisitInformationRepository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcOshaVisitInformationRepository.Update(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcOshaVisitInformationRepository.Delete(dto.ID);
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
