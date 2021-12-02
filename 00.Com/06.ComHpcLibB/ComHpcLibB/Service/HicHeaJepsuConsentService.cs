using System.Collections.Generic;
using ComHpcLibB.Repository;
using ComHpcLibB.Dto;
using System;
using ComBase;
using ComHpcLibB.Model;

namespace ComHpcLibB.Service
{
    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicHeaJepsuConsentService
    {
        private HicHeaJepsuConsentRepository hicHeaJepsuConsentRepository;
        /// <summary>
        /// 생성자 
        /// </summary>
        public HicHeaJepsuConsentService()
        {
            hicHeaJepsuConsentRepository = new HicHeaJepsuConsentRepository();
        }

        public List<HIC_HEA_JEPSU_CONSENT> GetItembyJepDate(string strFrDate, string strToDate, string strSName)
        {
            return hicHeaJepsuConsentRepository.GetItembyJepDate(strFrDate, strToDate, strSName);
        }
    }
}
