namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuPatientSchoolService
    {
        
        private HicJepsuPatientSchoolRepository hicJepsuPatientSchoolRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuPatientSchoolService()
        {
			this.hicJepsuPatientSchoolRepository = new HicJepsuPatientSchoolRepository();
        }

        public HIC_JEPSU_PATIENT_SCHOOL GetItembyJepDateWrtNo(string strFrDate, string strToDate, long nWrtNo)
        {
            return hicJepsuPatientSchoolRepository.GetItembyJepDateWrtNo(strFrDate, strToDate, nWrtNo);
        }

        public List<HIC_JEPSU_PATIENT_SCHOOL> GetItembyJepDate(string strFrDate, string strToDate, string strChkRePrint, string strSName, long nLtdCode, string strClass, string strBan)
        {
            return hicJepsuPatientSchoolRepository.GetItembyJepDate(strFrDate, strToDate, strChkRePrint, strSName, nLtdCode, strClass, strBan);
        }

        public List<HIC_JEPSU_PATIENT_SCHOOL> GetItembyJepDateLtdCode(string strFrDate, string strToDate, long nLtdCode)
        {
            return hicJepsuPatientSchoolRepository.GetItembyJepDateLtdCode(strFrDate, strToDate, nLtdCode);
        }

        public List<HIC_JEPSU_PATIENT_SCHOOL> GetSexbyJepDate(string strFrDate, string strToDate, long nLtdCode)
        {
            return hicJepsuPatientSchoolRepository.GetSexbyJepDate(strFrDate, strToDate, nLtdCode);
        }

        public List<HIC_JEPSU_PATIENT_SCHOOL> GetItembyJepDatePrtCnt(string strFrDate, string strToDate, string strPrt, long nLtdCode)
        {
            return hicJepsuPatientSchoolRepository.GetItembyJepDatePrtCnt(strFrDate, strToDate, strPrt, nLtdCode);
        }

        public List<HIC_JEPSU_PATIENT_SCHOOL> GetItembyJepDate(string argDate1, string argDate2, string argLtdCode, string argClass)
        {
            return hicJepsuPatientSchoolRepository.GetItembyJepDate(argDate1, argDate2, argLtdCode, argClass);
        }

        public HIC_JEPSU_PATIENT_SCHOOL GetMinMaxDate(string strFrDate, string strToDate, long nLtdCode)
        {
            return hicJepsuPatientSchoolRepository.GetMinMaxDate(strFrDate, strToDate, nLtdCode);
        }

        public List<HIC_JEPSU_PATIENT_SCHOOL> GetItemCntbyJepDate(string strFrDate, string strToDate, long nLtdCode)
        {
            return hicJepsuPatientSchoolRepository.GetItemCntbyJepDate(strFrDate, strToDate, nLtdCode);
        }

        public HIC_JEPSU_PATIENT_SCHOOL GetItembyJepDateSingle(string strFrDate, string strToDate, long nLtdCode1, string strClass)
        {
            return hicJepsuPatientSchoolRepository.GetItembyJepDateSingle(strFrDate, strToDate, nLtdCode1, strClass);
        }

        public List<HIC_JEPSU_PATIENT_SCHOOL> GetItembyJepDateGroup(string argDate1, string argDate2, string argLtdCode, string argClass)
        {
            return hicJepsuPatientSchoolRepository.GetItembyJepDateGroup(argDate1, argDate2, argLtdCode, argClass);
        }
    }
}
