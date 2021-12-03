namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class ViewSugaCodeService
    {
        
        private ViewSugaCodeRepository viewSugaCodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public ViewSugaCodeService()
        {
			this.viewSugaCodeRepository = new ViewSugaCodeRepository();
        }

        public long GetBAmtByLikeSucode(string argSucode)
        {
            return viewSugaCodeRepository.GetBAmtByLikeSucode(argSucode);
        }
    }
}
