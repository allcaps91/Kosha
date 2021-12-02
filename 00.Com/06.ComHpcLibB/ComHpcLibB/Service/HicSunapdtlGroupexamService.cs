
namespace ComHpcLibB.Service
{

    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;
    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicSunapdtlGroupexamService
    {
        private HicSunapdtlGroupexamRepository hicSunapdtlGroupexamRepository;

        public HicSunapdtlGroupexamService()
        {
            this.hicSunapdtlGroupexamRepository = new HicSunapdtlGroupexamRepository();
        }

        public List<HIC_SUNAPDTL_GROUPEXAM> GetExcodeByWrtNo(long argWrtNo)
        {
            return hicSunapdtlGroupexamRepository.GetExcodeByWrtNo(argWrtNo);
        }


    }

    

}
