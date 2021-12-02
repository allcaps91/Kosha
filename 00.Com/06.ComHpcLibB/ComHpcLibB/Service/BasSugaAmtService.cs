namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class BasSugaAmtService
    {
        
        private BasSugaAmtRepository basSugaAmtRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public BasSugaAmtService()
        {
			this.basSugaAmtRepository = new BasSugaAmtRepository();
        }

        public long GetBAmtBySuNext(string argSucode, string argBDate)
        {
            return basSugaAmtRepository.GetBAmtBySuNext(argSucode, argBDate);
        }
    }
}
