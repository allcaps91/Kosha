namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class EtcAlimTalkTemplateService
    {
        private EtcAlimTalkTemplateRepository etcAlimTalkTemplateRepository;
        /// <summary>
        /// 생성자 
        /// </summary>
        public EtcAlimTalkTemplateService()
        {
			this.etcAlimTalkTemplateRepository = new EtcAlimTalkTemplateRepository();
        }

        public ETC_ALIMTALK_TEMPLATE GetItembyTempCd(string argTempCD)
        {
            return etcAlimTalkTemplateRepository.GetItembyTempCd(argTempCD);
        }

        public ETC_ALIMTALK_TEMPLATE GetTitlebyTempCd(string argCode)
        {
            return etcAlimTalkTemplateRepository.GetTitlebyTempCd(argCode);
        }

        public ETC_ALIMTALK_TEMPLATE GetMessagebyTempCd(string argCode)
        {
            return etcAlimTalkTemplateRepository.GetMessagebyTempCd(argCode);
        }

        public ETC_ALIMTALK_TEMPLATE GetSendSmsbyTempCd(string argCode)
        {
            return etcAlimTalkTemplateRepository.GetSendSmsbyTempCd(argCode);
        }

        public List<ETC_ALIMTALK_TEMPLATE> GetTempCdTitleRowId()
        {
            return etcAlimTalkTemplateRepository.GetTempCdTitleRowId();
        }
    }
}
