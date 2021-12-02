namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuResEtcSangdamNewLtdService
    {
        
        private HicJepsuResEtcSangdamNewLtdRepository hicJepsuResEtcSangdamNewLtdRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResEtcSangdamNewLtdService()
        {
			this.hicJepsuResEtcSangdamNewLtdRepository = new HicJepsuResEtcSangdamNewLtdRepository();
        }

        public List<HIC_JEPSU_RES_ETC_SANGDAM_NEW_LTD> GetItembyJepDate(string strFrDate, string strToDate, long nLtdCode, string strSName, string strJob, string strSort, long nLicense, int nPan)
        {
            return hicJepsuResEtcSangdamNewLtdRepository.GetItembyJepDate(strFrDate, strToDate, nLtdCode, strSName, strJob, strSort, nLicense, nPan);
        }
    }
}
