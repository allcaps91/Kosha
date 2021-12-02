namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuJinGbnService
    {
        
        private HicJepsuJinGbnRepository hicJepsuJinGbnRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuJinGbnService()
        {
			this.hicJepsuJinGbnRepository = new HicJepsuJinGbnRepository();
        }

        public List<HIC_JEPSU_JIN_GBN> GetItembyJepDate(string strFrDate, string strToDate, long nLtdCode)
        {
            return hicJepsuJinGbnRepository.GetItembyJepDate(strFrDate, strToDate, nLtdCode);
        }

        public HIC_JEPSU_JIN_GBN GetItembyWrtNo(long fnWRTNO)
        {
            return hicJepsuJinGbnRepository.GetItembyWrtNo(fnWRTNO);
        }
    }
}
