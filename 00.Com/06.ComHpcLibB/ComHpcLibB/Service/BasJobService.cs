namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class BasJobService
    {
        
        private BasJobRepository basJobRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public BasJobService()
        {
			this.basJobRepository = new BasJobRepository();
        }

        public List<BAS_JOB> READ_Holyday(string strFDate, string strTDate)
        {
            return basJobRepository.READ_Holyday(strFDate, strTDate);
        }

        public string GetHolyDay(string strDate)
        {
            return basJobRepository.GetHolyDay(strDate);
        }
    }
}
