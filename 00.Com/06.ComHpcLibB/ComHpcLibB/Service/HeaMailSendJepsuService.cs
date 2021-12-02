namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;
    using ComHpcLibB.Dto;

    public class HeaMailSendJepsuService
    {
        private HeaMailSendJepsuRepository heaMailSendJepsuRepository;

        /// <summary>
        /// 
        /// </summary>
        public HeaMailSendJepsuService()
        {
            this.heaMailSendJepsuRepository = new HeaMailSendJepsuRepository();
        }

        public List<HEA_MAILSEND_JEPSU> GetItembySendDate(string strSendDate)
        {
            return heaMailSendJepsuRepository.GetItembySendDate(strSendDate);
        }

        public HEA_MAILSEND GetSendDateByWrtno(long wRTNO)
        {
            return heaMailSendJepsuRepository.GetSendDateByWrtno(wRTNO);
        }
    }
}
