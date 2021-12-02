namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComHpcLibB.Model;


    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicCancerNewJepsuPatientService
    {
        private HicCancerNewJepsuPatientRepository hicCancerNewJepsuPatientRepository;


        public HicCancerNewJepsuPatientService()
        {
            this.hicCancerNewJepsuPatientRepository = new HicCancerNewJepsuPatientRepository();
        }


        public List<HIC_CANCER_NEW_JEPSU_PATIENT> GetItembyJepdateGubun(string strFdate, string strTdate, string strName, string strLtdcode, string strSort, string strRePrint, long nWrtno, string strCert)
        {
            return hicCancerNewJepsuPatientRepository.GetItembyJepdateGubun(strFdate, strTdate, strName, strLtdcode, strSort, strRePrint, nWrtno, strCert);
        }

        public HIC_CANCER_NEW_JEPSU_PATIENT GetIetmbyWrtNo(long fnWrtNo)
        {
            return hicCancerNewJepsuPatientRepository.GetIetmbyWrtNo(fnWrtNo);
        }

    }
}
