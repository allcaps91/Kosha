namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuHicLtdPatientSangdamService
    {
        
        private HeaJepsuHicLtdPatientSangdamRepository heaJepsuHicLtdPatientSangdamRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuHicLtdPatientSangdamService()
        {
			this.heaJepsuHicLtdPatientSangdamRepository = new HeaJepsuHicLtdPatientSangdamRepository();
        }

        public List<HEA_JEPSU_HIC_LTD_PATIENT_SANGDAM> GetItembyEntTimeGubun(string idNumber, string sJob, string strFrDate, string strToDate, string strWToDate, string strLtd, string strSort, long nLicenceNo, string argSName)
        {
            return heaJepsuHicLtdPatientSangdamRepository.GetItembyEntTimeGubun(idNumber, sJob, strFrDate, strToDate, strWToDate, strLtd, strSort, nLicenceNo, argSName);
        }
    }
}
