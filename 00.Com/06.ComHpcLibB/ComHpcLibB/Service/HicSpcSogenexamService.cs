namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSpcSogenexamService
    {
        
        private HicSpcSogenexamRepository hicSpcSogenexamRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcSogenexamService()
        {
			this.hicSpcSogenexamRepository = new HicSpcSogenexamRepository();
        }

        public HIC_SPC_SOGENEXAM GetHangbySogenCode(string fstrCode)
        {
            return hicSpcSogenexamRepository.GetHangbySogenCode(fstrCode);
        }
    }
}
