namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicResDentalJepsuService
    {
        
        private HicResDentalJepsuRepository hicResDentalJepsuRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicResDentalJepsuService()
        {
			this.hicResDentalJepsuRepository = new HicResDentalJepsuRepository();
        }

        public HIC_RES_DENTAL_JEPSU GetCountbySysDate(long nLicense)
        {
            return hicResDentalJepsuRepository.GetCountbySysDate(nLicense);
        }
    }
}
