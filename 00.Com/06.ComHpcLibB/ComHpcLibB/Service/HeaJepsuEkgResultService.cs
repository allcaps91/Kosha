namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuEkgResultService
    {
        
        private HeaJepsuEkgResultRepository heaJepsuEkgResultRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuEkgResultService()
        {
			this.heaJepsuEkgResultRepository = new HeaJepsuEkgResultRepository();
        }

        public List<HEA_JEPSU_EKG_RESULT> GetItembySDate(string strFrDate, string strToDate, List<string> strGbSts, string strEkg)
        {
            return heaJepsuEkgResultRepository.GetItembySDate(strFrDate, strToDate, strGbSts, strEkg);
        }
    }
}
