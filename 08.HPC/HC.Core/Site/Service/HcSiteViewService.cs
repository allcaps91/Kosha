namespace HC.Core.Site.Service
{
    using System.Collections.Generic;
    using HC.Core.Site.Repository;
    using HC.Core.Site.Dto;
    using ComBase.Mvc.Utils;


    /// <summary>
    /// 
    /// </summary>
    public class HcSiteViewService
    {
        
        private HcSiteViewRepository hcSiteViewRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcSiteViewService()
        {
			this.hcSiteViewRepository = new HcSiteViewRepository();
        }

        public List<HC_SITE_VIEW> Search(string idOrName)
        {
            if (idOrName.IsNumeric())
            {
                return hcSiteViewRepository.FindById(idOrName);
            }
            else
            {
                return hcSiteViewRepository.FindByName(idOrName);
            }
        }
    }
}
