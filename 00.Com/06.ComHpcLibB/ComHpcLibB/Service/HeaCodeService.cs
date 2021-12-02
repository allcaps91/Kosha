namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaCodeService
    {
        
        private HeaCodeRepository heaCodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaCodeService()
        {
			this.heaCodeRepository = new HeaCodeRepository();
        }

        public List<HEA_CODE> GetGroupNameByGubun(string argGbn)
        {
            return heaCodeRepository.GetGroupNameByGubun(argGbn);
        }

        public List<HEA_CODE> GetItemByGubunGroupBy(string argGbn, List<string> argExams = null, string argViewGbn = "")
        {
            return heaCodeRepository.GetItemByGubunGroupBy(argGbn, argExams, argViewGbn);
        }

        public string GetItemByGubun1(string argExCode)
        {
            return heaCodeRepository.GetItemByGubun1(argExCode);
        }

        public List<HEA_CODE> GetItemByGubun(string argExCode)
        {
            return heaCodeRepository.GetItemByGubun(argExCode);
        }
        public string GetGubun1ByCode(string argExCode)
        {
            return heaCodeRepository.GetGubun1ByCode(argExCode);
        }

        public List<HEA_CODE> GetListByCodeName(string strGubun, string strName = "")
        {
            return heaCodeRepository.GetListByCodeName(strGubun, strName);
        }

        public string GetGubun2ByGubunCode(string argGubun, string argCode)
        {
            return heaCodeRepository.GetGubun2ByGubunCode(argGubun, argCode);
        }

        public string GetNameByGubunCode(string argGubun, string argCode)
        {
            return heaCodeRepository.GetNameByGubunCode(argGubun, argCode);
        }

        public List<HEA_CODE> Hea_Part_Jepsu(string strGubun)
        {
            return heaCodeRepository.Hea_Part_Jepsu(strGubun);
        }

        public List<HEA_CODE> GetNameByGubun(string argGbn)
        {
            return heaCodeRepository.GetNameByGubun(argGbn);
        }

        public List<HEA_CODE> GetListByGubunName(string argGubun, string argName)
        {
            return heaCodeRepository.GetListByGubunName(argGubun, argName);
        }

        public int UpdateSortbyRowId(string cODE, string rID)
        {
            return heaCodeRepository.UpdateSortbyRowId(cODE, rID);
        }

        public HEA_CODE GetItemByActPart(string argActPart)
        {
            return heaCodeRepository.GetItemByActPart(argActPart);
        }

        public string GetGubun2ByNameGubun(string argGbName, string argGubun)
        {
            return heaCodeRepository.GetGubun2ByNameGubun(argGbName, argGubun);
        }

        public string GetCodeByGubun(string argGubun)
        {
            return heaCodeRepository.GetCodeByGubun(argGubun);
        }

        public List<HEA_CODE> GetAllbyGubunCode(string strCODE, string strJong, string strPtNo)
        {
            return heaCodeRepository.GetAllbyGubunCode(strCODE, strJong, strPtNo);
        }

        public List<HEA_CODE> FindOne(string argGubun)
        {
            return heaCodeRepository.FindOne(argGubun);
        }

        public string GetNameByGubunGubun2(string argGubun, string argGubun2)
        {
            return heaCodeRepository.GetNameByGubunGubun2(argGubun, argGubun2);
        }
        public long GetCodebyGubun(string strGubun)
        {
            return heaCodeRepository.GetCodebyGubun(strGubun);
        }
    }
}
