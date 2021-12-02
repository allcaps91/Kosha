namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuResEtcExjongLtdService
    {
        
        private HicJepsuResEtcExjongLtdRepository hicJepsuResEtcExjongLtdRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResEtcExjongLtdService()
        {
			this.hicJepsuResEtcExjongLtdRepository = new HicJepsuResEtcExjongLtdRepository();
        }

        public List<HIC_JEPSU_RES_ETC_EXJONG_LTD> GetItembyJepDate(string strFrDate, string strToDate, long nLtdCode, string strSName, string strJob, string strSort)
        {
            return hicJepsuResEtcExjongLtdRepository.GetItembyJepDate(strFrDate, strToDate, nLtdCode, strSName, strJob, strSort);
        }
    }
}
