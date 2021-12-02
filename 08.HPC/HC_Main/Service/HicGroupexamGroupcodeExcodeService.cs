namespace HC_Main.Service
{
    using System.Collections.Generic;
    using HC_Main.Repository;
    using HC_Main.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicGroupexamGroupcodeExcodeService
    {
        
        private HicGroupexamGroupcodeExcodeRepository hicGroupexamGroupcodeExcodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicGroupexamGroupcodeExcodeService()
        {
			this.hicGroupexamGroupcodeExcodeRepository = new HicGroupexamGroupcodeExcodeRepository();
        }

        public List<HIC_GROUPEXAM_GROUPCODE_EXCODE> GetListByCode(string argCODE, List<string> lstExList)
        {
            return hicGroupexamGroupcodeExcodeRepository.GetHicListByCode(argCODE, lstExList);
        }
        public List<HIC_GROUPEXAM_GROUPCODE_EXCODE> GetListByCodeIn(string argCODE, List<string> lstExList)
        {
            return hicGroupexamGroupcodeExcodeRepository.GetListByCodeIn(argCODE, lstExList);
        }

        public List<HIC_GROUPEXAM_GROUPCODE_EXCODE> GetHeaListByCode(string argCODE, List<string> lstExList)
        {
            return hicGroupexamGroupcodeExcodeRepository.GetHeaListByCode(argCODE, lstExList);
        }

        public List<HIC_GROUPEXAM_GROUPCODE_EXCODE> GetListByCodesIN(List<string> lstExList)
        {
            return hicGroupexamGroupcodeExcodeRepository.GetListByCodesIN(lstExList);
        }
    }
}
