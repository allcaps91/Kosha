namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuLtdExjongService
    {
        
        private HicJepsuLtdExjongRepository hicJepsuLtdExjongRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuLtdExjongService()
        {
			this.hicJepsuLtdExjongRepository = new HicJepsuLtdExjongRepository();
        }

        public List<HIC_JEPSU_LTD_EXJONG> GetItembyJepDate(string strFDate, string strTDate, string strJong, long nLtdCode)
        {
            return hicJepsuLtdExjongRepository.GetItembyJepDate(strFDate, strTDate, strJong, nLtdCode);
        }

        public List<HIC_JEPSU_LTD_EXJONG> GetItembyGjJong(string strGjJong, long nLtdCode, string strTongbo)
        {
            return hicJepsuLtdExjongRepository.GetItembyGjJong(strGjJong, nLtdCode, strTongbo);
        }
    }
}
