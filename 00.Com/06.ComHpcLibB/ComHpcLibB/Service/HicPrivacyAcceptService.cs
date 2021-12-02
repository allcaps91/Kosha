
namespace ComHpcLibB.Service
{

    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicPrivacyAcceptService 
    {
        private HicPrivacyAcceptRepository hicPrivacyAcceptRepository;
        /// <summary>
        /// 생성자 
        /// </summary>
        public HicPrivacyAcceptService()
        {
            this.hicPrivacyAcceptRepository = new HicPrivacyAcceptRepository();
        }

        public HIC_PRIVACY_ACCEPT GetIetmByPtnoYear(string argPtno, string argYear)
        {
            return hicPrivacyAcceptRepository.GetIetmByPtnoYear(argPtno, argYear);
        }

        public bool Insert(HIC_PRIVACY_ACCEPT nHC)
        {
            try
            {
                hicPrivacyAcceptRepository.Insert(nHC);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
