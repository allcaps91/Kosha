namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class DoctorInfoService
    {
        
        private DoctorInfoRepository doctorInfoRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public DoctorInfoService()
        {
			this.doctorInfoRepository = new DoctorInfoRepository();
        }

        public DOCTOR_INFO Read_Hic_Doctor_Info(string sSabun, long lSabun)
        {
            return doctorInfoRepository.Read_Hic_Doctor_Info(sSabun, lSabun);
        }
    }
}
