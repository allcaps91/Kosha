namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaMailsendService
    {
        
        private HeaMailsendRepository heaMailsendRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaMailsendService()
        {
			this.heaMailsendRepository = new HeaMailsendRepository();
        }

        public List<HEA_MAILSEND> GetItembySendDate(string strSendDate)
        {
            return heaMailsendRepository.GetItembySendDate(strSendDate);
        }

        public int GetCountbySendDate(long nWrtNo, string strSendDate)
        {
            return heaMailsendRepository.GetCountbySendDate(nWrtNo, strSendDate);
        }

        public int Insert(HEA_MAILSEND item)
        {
            return heaMailsendRepository.Insert(item);
        }
    }
}
