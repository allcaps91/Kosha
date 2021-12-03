namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaSangdamHisService
    {
        
        private HeaSangdamHisRepository heaSangdamHisRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaSangdamHisService()
        {
			this.heaSangdamHisRepository = new HeaSangdamHisRepository();
        }

        public int InsertSangdamHis(HEA_SANGDAM_HIS item)
        {
            return heaSangdamHisRepository.InsertSangdamHis(item);
        }

        public int InsertSangdam(HEA_SANGDAM_HIS item2)
        {
            return heaSangdamHisRepository.InsertSangdam(item2);
        }
    }
}
