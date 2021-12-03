namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComHpcLibB.Model;

    public class HeaJepsuSunapdtlService 
    {
        private HeaJepsuSunapdtlRepository heaJepsuSunapdtlRepository;

        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuSunapdtlService()
        {
            this.heaJepsuSunapdtlRepository = new HeaJepsuSunapdtlRepository();
        }

        public List<HEA_JEPSU_SUNAPDTL> GetGamInfoByWrtno(long nWRTNO)
        {
            return heaJepsuSunapdtlRepository.GetGamInfoByWrtno(nWRTNO);
        }
    }
}
