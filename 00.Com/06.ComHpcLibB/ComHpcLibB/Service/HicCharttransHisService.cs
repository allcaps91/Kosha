namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HicCharttransHisService
    {
        
        private HicCharttransHisRepository hicCharttransHisRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicCharttransHisService()
        {
			this.hicCharttransHisRepository = new HicCharttransHisRepository();
        }

        public int Insert(HIC_CHARTTRANS_HIS item)
        {
            return hicCharttransHisRepository.Insert(item);
        }

        public int InsertDel(HIC_CHARTTRANS_HIS item)
        {
            return hicCharttransHisRepository.InsertDel(item);
        }
    }
}
