namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;


    /// <summary>
    /// 
    /// </summary>
    public class HicMisuGiroService
    {
        
        private HicMisuGiroRepository hicMisuGiroRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicMisuGiroService()
        {
			this.hicMisuGiroRepository = new HicMisuGiroRepository();
        }

        public List<HIC_MISU_GIRO> getGiro(long nGiroNo)
        {
            return hicMisuGiroRepository.getGiro(nGiroNo);
        }
    }
}
