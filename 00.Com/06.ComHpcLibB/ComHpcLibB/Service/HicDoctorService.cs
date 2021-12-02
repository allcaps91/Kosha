namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicDoctorService
    {
        
        private HicDoctorRepository hicDoctorRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicDoctorService()
        {
			this.hicDoctorRepository = new HicDoctorRepository();
        }

        public string Read_License_DrName(long argLicence)
        {
            return hicDoctorRepository.Read_License_DrName(argLicence);
        }

        public HIC_DOCTOR Read_Hic_DrCode(long argSabun)
        {
            return hicDoctorRepository.Read_Hic_DrCode(argSabun);
        }

        public string Read_Hic_OcsDrcode(long argSabun)
        {
            return hicDoctorRepository.Read_Hic_OcsDrcode(argSabun);
        }

        public long Read_Hic_DrSabun(string argDrLicence)
        {
            return hicDoctorRepository.Read_Hic_DrSabun(argDrLicence);
        }

        public List<HIC_DOCTOR> Read_Combo_HisDoctor(string strGubun)
        {
            return hicDoctorRepository.Read_Combo_HisDoctor(strGubun);
        }

        public List<HIC_DOCTOR> GetItemAll()
        {
            return hicDoctorRepository.GetItemAll();
        }

        public List<HIC_DOCTOR> GetIDrCode()
        {
            return hicDoctorRepository.GetIDrCode();
        }

        public HIC_DOCTOR GetIDrNameLicencebyDrSabun(string strDRSABUN)
        {
            return hicDoctorRepository.GetIDrNameLicencebyDrSabun(strDRSABUN);
        }

        public string GetLicensebySabun(long nDrSabun)
        {
            return hicDoctorRepository.GetLicensebySabun(nDrSabun);
        }

        public List<HIC_DOCTOR> GetSabunbyRoom(List<string> strGubun)
        {
            return hicDoctorRepository.GetSabunbyRoom(strGubun);
        }


        public List<HIC_DOCTOR> GetListbyReday(string strREDAY)
        {
            return hicDoctorRepository.GetListbyReday(strREDAY);
        }

        public int Read_Doctor_Hea(long nDrSabun)
        {
            return hicDoctorRepository.Read_Doctor_Hea(nDrSabun);
        }

        public string Chk_Hea_Doct(long nDrSabun)
        {
            return hicDoctorRepository.Chk_Hea_Doct(nDrSabun);
        }

        public HIC_DOCTOR Read_Hic_DrCode3(long argSabun)
        {
            return hicDoctorRepository.Read_Hic_DrCode3(argSabun);
        }
    }
}
