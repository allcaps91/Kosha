namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSangdamNewJepsuExjongService
    {
        
        private HicSangdamNewJepsuExjongRepository hicSangdamNewJepsuExjongRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSangdamNewJepsuExjongService()
        {
			this.hicSangdamNewJepsuExjongRepository = new HicSangdamNewJepsuExjongRepository();
        }

        public HIC_SANGDAM_NEW_JEPSU_EXJONG GetCntCnt2(long nLicense)
        {
            return hicSangdamNewJepsuExjongRepository.GetCntCnt2(nLicense);
        }
    }
}
