namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicGroupexamExcodeService
    {
        
        private HicGroupexamExcodeRepository hicGroupexamExcodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicGroupexamExcodeService()
        {
			this.hicGroupexamExcodeRepository = new HicGroupexamExcodeRepository();
        }

        public List<HIC_GROUPEXAM_EXCODE> GetEndoGubunbyGroupCode(string strCode)
        {
            return hicGroupexamExcodeRepository.GetEndoGubunbyGroupCode(strCode);
        }

        public List<HIC_GROUPEXAM_EXCODE> GetItemsByCode(string argCode)
        {
            return hicGroupexamExcodeRepository.GetItemsByCode(argCode);
        }

        public List<HIC_GROUPEXAM_EXCODE> GetItembyGroupCode(string strCode, List<string> strExamList)
        {
            return hicGroupexamExcodeRepository.GetItembyGroupCode(strCode, strExamList);
        }
    }
}
