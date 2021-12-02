namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicOrgancodeService
    {
        
        private HicOrgancodeRepository hicOrgancodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicOrgancodeService()
        {
			this.hicOrgancodeRepository = new HicOrgancodeRepository();
        }

        public List<HIC_ORGANCODE> GetListAll(string argGubun, string argCode = "")
        {
            return hicOrgancodeRepository.GetListAll(argGubun, argCode);
        }

        public int DeleteByRowid(string strROWID)
        {
            return hicOrgancodeRepository.DeleteByRowid(strROWID);
        }

        public string GetRowidByCode(string strCODE)
        {
            return hicOrgancodeRepository.GetRowidByCode(strCODE);
        }

        public int Data_InSert(HIC_ORGANCODE item)
        {
            return hicOrgancodeRepository.Data_InSert(item);
        }

        public int Data_UpDate(HIC_ORGANCODE item)
        {
            return hicOrgancodeRepository.Data_UpDate(item);
        }

        public string GetRowidBySayuCode(string strSayuCode, string strCODE)
        {
            return hicOrgancodeRepository.GetRowidBySayuCode(strSayuCode, strCODE);
        }

        public List<HIC_ORGANCODE> GetSayuCodeNamebyGubunCode(string strGubun, string strCode)
        {
            return hicOrgancodeRepository.GetSayuCodeNamebyGubunCode(strGubun, strCode);
        }
    }
}
