namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaGroupexamExcodeService
    {
        
        private HeaGroupexamExcodeRepository heaGroupexamExcodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaGroupexamExcodeService()
        {
			this.heaGroupexamExcodeRepository = new HeaGroupexamExcodeRepository();
        }

        public List<HEA_GROUPEXAM_EXCODE> GetItemsByCode(string strCode)
        {
            return heaGroupexamExcodeRepository.GetItemsByCode(strCode);
        }

        public List<HEA_GROUPEXAM_EXCODE> GetItemsByCodeIN(List<string> lstCode)
        {
            return heaGroupexamExcodeRepository.GetItemsByCodeIN(lstCode);
        }

        public string GetRowidByGroupCode(string argGrpCD)
        {
            return heaGroupexamExcodeRepository.GetRowidByGroupCode(argGrpCD);
        }

        public List<HEA_GROUPEXAM_EXCODE> GetGbEndoCodeByCodeIN(List<string> lstgrpCode)
        {
            return heaGroupexamExcodeRepository.GetGbEndoCodeByCodeIN(lstgrpCode);
        }
    }
}
