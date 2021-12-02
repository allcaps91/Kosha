namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HcPanjengPatlistService
    {
        
        private HcPanjengPatlistRepository hcPanjengPatlistRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcPanjengPatlistService()
        {
			this.hcPanjengPatlistRepository = new HcPanjengPatlistRepository();
        }

        public List<HC_PANJENG_PATLIST> GetPanjengPatListbyJepDate(PAN_PATLIST_SEARCH sItem)
        {
            return hcPanjengPatlistRepository.GetPanjengPatListbyJepDate(sItem);
        }
    }
}
