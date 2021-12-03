namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuSangdamNewExjongService
    {
        
        private HicJepsuSangdamNewExjongRepository hicJepsuSangdamNewExjongRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuSangdamNewExjongService()
        {
			this.hicJepsuSangdamNewExjongRepository = new HicJepsuSangdamNewExjongRepository();
        }

        public List<HIC_JEPSU_SANGDAM_NEW_EXJONG> GetItembyJepDate(string strFrDate, string strToDate, int nGBn, long fnTab, string strSName, long nLtdCode, string strGubun)
        {
            return hicJepsuSangdamNewExjongRepository.GetItembyJepDate(strFrDate, strToDate, nGBn, fnTab, strSName, nLtdCode, strGubun);
        }

        public List<HIC_JEPSU_SANGDAM_NEW_EXJONG> GetItembyJepDateGjJong(string strFrDate, string strToDate, string strChul, string strGbn, string strJob, string strRoom, List<string> strDrList, List<string> strGubun, string strSName, string strJong, long nLtdCode)
        {
            return hicJepsuSangdamNewExjongRepository.GetItembyJepDateGjJong(strFrDate, strToDate, strChul, strGbn, strJob, strRoom, strDrList, strGubun, strSName, strJong, nLtdCode);
        }
    }
}
