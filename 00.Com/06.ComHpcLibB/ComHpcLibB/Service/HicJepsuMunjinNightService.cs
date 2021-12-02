namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuMunjinNightService
    {
        
        private HicJepsuMunjinNightRepository hicJepsuMunjinNightRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuMunjinNightService()
        {
			this.hicJepsuMunjinNightRepository = new HicJepsuMunjinNightRepository();
        }

        public List<HIC_JEPSU_MUNJIN_NIGHT> GetItembyPaNoGjYear(long nPano, string strGjYear)
        {
            return hicJepsuMunjinNightRepository.GetItembyPaNoGjYear(nPano, strGjYear);
        }
    }
}
