namespace ComHpcLibB.Service
{

    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicResBohum1JepsuPatientService

    {
        private HicResBohum1JepsuPatientRepository hicResBohum1JepsuPatientRepository;
        
        /// <summary>
        /// 생성자 
        /// </summary>
        public HicResBohum1JepsuPatientService()
        {
            this.hicResBohum1JepsuPatientRepository = new HicResBohum1JepsuPatientRepository();
        }

        public HIC_RES_BOHUM1_JEPSU_PATIENT GetItembyWrtNo(long fnWrtNo)
        {
            return hicResBohum1JepsuPatientRepository.GetItembyWrtNo(fnWrtNo);
        }

        public HIC_RES_BOHUM1_JEPSU_PATIENT GetItembyJepDateMirNoWrtNo(string sJepDate, long argMirno, long argWRTNO)
        {
            return hicResBohum1JepsuPatientRepository.GetItembyJepDateMirNoWrtNo(sJepDate, argMirno, argWRTNO);
        }

        public List <HIC_RES_BOHUM1_JEPSU_PATIENT> GetItembyJepdateGubun(string strFdate, string strTdate, string strName, string strLtdcode, string strSort, string strRePrint, long nWrtno, string strCert)
        {
            return hicResBohum1JepsuPatientRepository.GetItembyJepdateGubun(strFdate, strTdate, strName, strLtdcode, strSort, strRePrint, nWrtno, strCert);
        }

    }
}
