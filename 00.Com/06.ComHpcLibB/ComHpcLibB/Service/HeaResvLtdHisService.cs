namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaResvLtdHisService
    {
        
        private HeaResvLtdHisRepository heaResvLtdHisRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaResvLtdHisService()
        {
			this.heaResvLtdHisRepository = new HeaResvLtdHisRepository();
        }

        public int InsertData(HEA_RESV_LTD_HIS item)
        {
            return heaResvLtdHisRepository.InsertData(item);
        }
    }
}
