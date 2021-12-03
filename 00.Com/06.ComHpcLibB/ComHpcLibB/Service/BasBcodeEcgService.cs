namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class BasBcodeEcgService
    {
        
        private BasBcodeEcgRepository basBcodeEcgRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public BasBcodeEcgService()
        {
			this.basBcodeEcgRepository = new BasBcodeEcgRepository();
        }

        public List<BAS_BCODE_ECG> GetCodeAll()
        {
            return basBcodeEcgRepository.GetCodeAll();
        }

        public int DeletebyRowId(string strROWID)
        {
            return basBcodeEcgRepository.DeletebyRowId(strROWID);
        }

        public int InsertAll(string strCODE, string strName)
        {
            return basBcodeEcgRepository.InsertAll(strCODE, strName);
        }

        public List<BAS_BCODE_ECG> GetItemAll()
        {
            return basBcodeEcgRepository.GetItemAll();
        }

        public int UPdate(string strCODE, string strName, string strROWID)
        {
            return basBcodeEcgRepository.UPdate(strCODE, strName, strROWID);
        }
    }
}
