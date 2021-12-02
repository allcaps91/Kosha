namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuSangdamNewService
    {
        
        private HicJepsuSangdamNewRepository hicJepsuSangdamNewRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuSangdamNewService()
        {
			this.hicJepsuSangdamNewRepository = new HicJepsuSangdamNewRepository();
        }

        public List<HIC_JEPSU_SANGDAM_NEW> GetItembyPaNoJepDate(long argPaNo, string argJepDate)
        {
            return hicJepsuSangdamNewRepository.GetItembyPaNoJepDate(argPaNo, argJepDate);
        }
    }
}
