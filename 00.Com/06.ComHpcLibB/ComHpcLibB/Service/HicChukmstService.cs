namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicChukmstService
    {
        
        private HicChukmstRepository hicChukmstRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicChukmstService()
        {
			this.hicChukmstRepository = new HicChukmstRepository();
        }

        public List<HIC_CHUKMST> GetSDatebySDateLtdCode(string strFrDate, long nLtdCode)
        {
            return hicChukmstRepository.GetSDatebySDateLtdCode(strFrDate, nLtdCode);
        }
    }
}
