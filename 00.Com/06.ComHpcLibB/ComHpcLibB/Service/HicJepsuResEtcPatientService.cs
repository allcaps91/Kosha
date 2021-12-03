namespace ComHpcLibB
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicJepsuResEtcPatientService
    {
        private HicJepsuResEtcPatientRepository hicJepsuResEtcPatientRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResEtcPatientService()
        {
            this.hicJepsuResEtcPatientRepository = new HicJepsuResEtcPatientRepository();
        }

        public List<HIC_JEPSU_RES_ETC_PATIENT> GetItembyJepDate(string strFrDate, string strToDate, long nLtdCode, string strSName, string strGbRe, string strGjjong, long nWrtno, string strCert)
        {
            return hicJepsuResEtcPatientRepository.GetItembyJepDate(strFrDate, strToDate, nLtdCode, strSName, strGbRe, strGjjong, nWrtno, strCert);
        }

        public HIC_JEPSU_RES_ETC_PATIENT GetItemByWrtnoGubun(long nWrtno, string strGubun)
        {
            return hicJepsuResEtcPatientRepository.GetItemByWrtnoGubun(nWrtno, strGubun);
        }


    }
}
