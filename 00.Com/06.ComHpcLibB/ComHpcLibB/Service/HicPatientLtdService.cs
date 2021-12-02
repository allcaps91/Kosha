namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HicPatientLtdService
    {
        
        private HicPatientLtdRepository hicPatientLtdRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicPatientLtdService()
        {
			this.hicPatientLtdRepository = new HicPatientLtdRepository();
        }

        public HIC_PATIENT_LTD GetItembyJumin(string strJumin)
        {
            return hicPatientLtdRepository.GetItembyJumin(strJumin);
        }
    }
}
