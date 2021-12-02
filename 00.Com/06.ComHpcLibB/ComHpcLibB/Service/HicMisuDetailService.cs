namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComBase;


    /// <summary>
    /// 
    /// </summary>
    public class HicMisuDetailService
    {
        
        private HicMisuDetailRepository hicMisuDetailRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicMisuDetailService()
        {
			this.hicMisuDetailRepository = new HicMisuDetailRepository();
        }

        public int InsertList(HIC_MISU_DETAIL item)
        {
            return hicMisuDetailRepository.InsertList(item);
        }

        public int GongDanInsert(HIC_MISU_DETAIL item)
        {
            return hicMisuDetailRepository.GongDanInsert(item);
        }
    }
}
