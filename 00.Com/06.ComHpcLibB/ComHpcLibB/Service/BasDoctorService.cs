namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class BasDoctorService
    {
        
        private BasDoctorRepository basDoctorRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public BasDoctorService()
        {
			this.basDoctorRepository = new BasDoctorRepository();
        }

        public BAS_DOCTOR GetItembyDrCord(string strDrCode)
        {
            return basDoctorRepository.GetItembyDrCode(strDrCode);
        }
        public List<BAS_DOCTOR> GetItembyDrCodes(List<string> strDrCode)
        {
            return basDoctorRepository.GetItembyDrCodes(strDrCode);
        }

        public List<BAS_DOCTOR> GetItembyDrDept1DrCode(string strDeptCode, string strDRCODE)
        {
            return basDoctorRepository.GetItembyDrDept1DrCode(strDeptCode, strDRCODE);
        }
    }
}
