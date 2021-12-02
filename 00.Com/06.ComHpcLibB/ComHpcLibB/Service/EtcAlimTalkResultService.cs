namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class EtcAlimTalkResultService
    {
        private EtcAlimTalkResultRepository etcAlimTalkResultRepository;
        /// <summary>
        /// 생성자 
        /// </summary>
        public EtcAlimTalkResultService()
        {
            etcAlimTalkResultRepository = new EtcAlimTalkResultRepository();
        }

        public ETC_ALIMTALK_RESULT GetBigobyGubun(string argGubun, string argResCode)
        {
            return etcAlimTalkResultRepository.GetBigobyGubun(argGubun, argResCode);
        }
    }
}
