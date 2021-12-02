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
    public class HcOshaVisitReceiptService
    {
        
        private HcOshaVisitReceiptRepository hcOshaVisitReceiptRepository = null;
        private HcUserService hcUserService { get; }
        /// <summary>
        /// 
        /// </summary>
        public HcOshaVisitReceiptService()
        {
			this.hcOshaVisitReceiptRepository = new HcOshaVisitReceiptRepository();
            this.hcUserService = new HcUserService();
        }

        public List<HC_OSHA_VISIT_RECEIPT> FindAll(long siteId)
        {
            return hcOshaVisitReceiptRepository.FindAll(siteId);
        }
        public bool Save(IList<HC_OSHA_VISIT_RECEIPT> list, long siteId)
        {
            try
            {
                foreach (HC_OSHA_VISIT_RECEIPT dto in list)
                {
                    HC_USER user = hcUserService.FindByUserId(dto.REGUSERID);
                    dto.REGUSERNAME = user.Name;


                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)

                    {
                        dto.SITE_ID = siteId;
                        hcOshaVisitReceiptRepository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcOshaVisitReceiptRepository.Update(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcOshaVisitReceiptRepository.Delete(dto.ID);
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
