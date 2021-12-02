namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaDayService
    {
        
        private HeaDayRepository heaDayRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaDayService()
        {
			this.heaDayRepository = new HeaDayRepository();
        }

        public string GetRemarkByDate(string ArgCurMonth)
        {
            return heaDayRepository.GetRemarkByDate(ArgCurMonth);
        }
    }
}
