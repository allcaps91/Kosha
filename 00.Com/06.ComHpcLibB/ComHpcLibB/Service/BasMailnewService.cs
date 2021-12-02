namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class BasMailnewService
    {
        
        private BasMailnewRepository basMailnewRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public BasMailnewService()
        {
			this.basMailnewRepository = new BasMailnewRepository();
        }
        
        public string Read_MailName(string strCode)
        {
            return basMailnewRepository.Read_MailName(strCode);
        }

        public string GetMailJiyekbyMailCode(string strZipCode)
        {
            return basMailnewRepository.GetMailJiyekbyMailCode(strZipCode);
        }
    }
}
