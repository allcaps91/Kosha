namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class OcsDoctorService
    {
        
        private OcsDoctorRepository ocsDoctorRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public OcsDoctorService()
        {
			this.ocsDoctorRepository = new OcsDoctorRepository();
        }

        public string GetDrCodebySabun(string strSabun)
        {
            return ocsDoctorRepository.GetDrCodebySabun(strSabun);
        }

        public string GetDrNamebySabun(string strSabun)
        {
            return ocsDoctorRepository.GetDrNamebySabun(strSabun);
        }

        public string GetRedadDrNmaebyDrBunho(long pANJENGDRNO8)
        {
            return ocsDoctorRepository.GetRedadDrNmaebyDrBunho(pANJENGDRNO8);
        }

        public string GetSabunByDrCode(string argDrCode)
        {
            return ocsDoctorRepository.GetSabunByDrCode(argDrCode);
        }
    }
}
