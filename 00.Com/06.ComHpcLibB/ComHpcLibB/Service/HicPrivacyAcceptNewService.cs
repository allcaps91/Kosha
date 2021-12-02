namespace ComHpcLibB.Service
{

    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicPrivacyAcceptNewService
    {
        private HicPrivacyAcceptNewRepository hicPrivacyAcceptNewRepository;
        /// <summary>
        /// 생성자 
        /// </summary>
        public HicPrivacyAcceptNewService()
        {
            this.hicPrivacyAcceptNewRepository = new HicPrivacyAcceptNewRepository();
        }

        public HIC_PRIVACY_ACCEPT_NEW GetIetmByPtnoYear(string argPtno, string argYear)
        {
            return hicPrivacyAcceptNewRepository.GetIetmByPtnoYear(argPtno, argYear);
        }

        public bool Insert(HIC_PRIVACY_ACCEPT_NEW nHC)
        {
            try
            {
                hicPrivacyAcceptNewRepository.Insert(nHC);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
