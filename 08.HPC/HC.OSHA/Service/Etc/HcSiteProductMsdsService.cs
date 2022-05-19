namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using ComBase;
    using System;
    using HC.OSHA.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HcSiteProductMsdsService
    {
        
        private HcSiteProductMsdsRepository hcSiteProductMsdsRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcSiteProductMsdsService()
        {
			this.hcSiteProductMsdsRepository = new HcSiteProductMsdsRepository();
        }

        public List<HC_SITE_PRODUCT_MSDS_MODEL> FindAll(long SITE_PRODUCT_ID)
        {
            return hcSiteProductMsdsRepository.FindAll(SITE_PRODUCT_ID);
        }
        public void Delete(long id)
        {
            hcSiteProductMsdsRepository.Delete(id);
        }
    }
}
