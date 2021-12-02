namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicPatientHeaJepsuService
    {
        
        private HicPatientHeaJepsuRepository hicPatientHeaJepsuRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicPatientHeaJepsuService()
        {
			this.hicPatientHeaJepsuRepository = new HicPatientHeaJepsuRepository();
        }

        public HIC_PATIENT_HEA_JEPSU GetItembyJuminOrBirth(string strJumin, string strAesJumin, string strBirth, string strYearFr, string strYearTo, string strSName)
        {
            return hicPatientHeaJepsuRepository.GetItembyJuminOrBirth(strJumin, strAesJumin, strBirth, strYearFr, strYearTo, strSName);
        }
    }
}
