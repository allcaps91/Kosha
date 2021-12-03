namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicCancerResv2PatientService
    {
        
        private HicCancerResv2PatientRepository hicCancerResv2PatientRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicCancerResv2PatientService()
        {
			this.hicCancerResv2PatientRepository = new HicCancerResv2PatientRepository();
        }

        public List<HIC_CANCER_RESV2_PATIENT> GetItembyRTime(string strDate)
        {
            return hicCancerResv2PatientRepository.GetItembyRTime(strDate);
        }

        public List<HIC_CANCER_RESV2_PATIENT> GetItembySDate(string strDate)
        {
            return hicCancerResv2PatientRepository.GetItembySDate(strDate);
        }
    }
}
