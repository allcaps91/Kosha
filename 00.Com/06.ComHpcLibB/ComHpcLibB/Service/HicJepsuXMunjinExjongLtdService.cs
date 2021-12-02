namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuXMunjinExjongLtdService
    {
        
        private HicJepsuXMunjinExjongLtdRepository hicJepsuXMunjinExjongLtdRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuXMunjinExjongLtdService()
        {
			this.hicJepsuXMunjinExjongLtdRepository = new HicJepsuXMunjinExjongLtdRepository();
        }

        public List<HIC_JEPSU_X_MUNJIN_EXJONG_LTD> GetItembyJepDateLtdCodeSName(string strFrDate, string strToDate, long nLtdCode, string strSName, long gnHicLicense, string strJob, string strAll, string strAllDoctor, string strSort)
        {
            return hicJepsuXMunjinExjongLtdRepository.GetItembyJepDateLtdCodeSName(strFrDate, strToDate, nLtdCode, strSName, gnHicLicense, strJob, strAll, strAllDoctor, strSort);
        }
    }
}
