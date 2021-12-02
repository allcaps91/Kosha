namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuPatientJinGbdService
    {
        
        private HicJepsuPatientJinGbdRepository hicJepsuPatientJinGbdRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuPatientJinGbdService()
        {
			this.hicJepsuPatientJinGbdRepository = new HicJepsuPatientJinGbdRepository();
        }

        public HIC_JEPSU_PATIENT_JIN_GBD GetItembyWrtNoGjJong(long nWRTNO, string strGjJong, string strGbPrint)
        {
            return hicJepsuPatientJinGbdRepository.GetItembyWrtNoGjJong(nWRTNO, strGjJong, strGbPrint);
        }

        public List<HIC_JEPSU_PATIENT_JIN_GBD> GetItembyJepDate(string strFDate, string strTDate, long nLtdcode, string strSName, string strGbRe)
        {
            return hicJepsuPatientJinGbdRepository.GetItembyJepDate(strFDate, strTDate, nLtdcode, strSName, strGbRe);
        }
    }
}
