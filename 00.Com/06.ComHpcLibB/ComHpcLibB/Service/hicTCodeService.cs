namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicTCodeService 
    {
        private HicTCodeRepository hicTCodeRepository;
        /// <summary>
        /// 생성자 
        /// </summary>
        public HicTCodeService()
        {
            this.hicTCodeRepository = new HicTCodeRepository();
        }

        public List<HIC_TCODE> GetItembyGubun(int argGubun)
        {
            return hicTCodeRepository.GetItembyGubun(argGubun);
        }
    }
}
