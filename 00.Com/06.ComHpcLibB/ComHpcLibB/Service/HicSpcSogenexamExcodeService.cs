namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSpcSogenexamExcodeService
    {
        
        private HicSpcSogenexamExcodeRepository hicSpcSogenexamExcodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcSogenexamExcodeService()
        {
			this.hicSpcSogenexamExcodeRepository = new HicSpcSogenexamExcodeRepository();
        }

        public List<HIC_SPC_SOGENEXAM_EXCODE> GetItembySogenCode(string fstrCode)
        {
            return hicSpcSogenexamExcodeRepository.GetItembySogenCode(fstrCode);
        }
    }
}
