namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComHpcLibB.Model;

    /// <summary>
    /// 
    /// </summary>
    public class PatientInfoService
    {        
        private PatientInfoRepository patientInfoRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public PatientInfoService()
        {
			this.patientInfoRepository = new PatientInfoRepository();
        }

        public PATIENT_INFO GetItembyWrtNoPart(long nWrtNo, string sHcPart)
        {
            return patientInfoRepository.GetItembyWrtNoPart(nWrtNo, sHcPart);
        }
    }
}
