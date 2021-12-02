namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuValuationService
    {
        
        private HeaJepsuValuationRepository heaJepsuValuationRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuValuationService()
        {
			this.heaJepsuValuationRepository = new HeaJepsuValuationRepository();
        }

        public List<HEA_JEPSU_VALUATION> GetItembyLtdCode(string strGubun, string strFrDate, string strToDate, string strLtdCode)
        {
            return heaJepsuValuationRepository.GetItembyLtdCode(strGubun, strFrDate, strToDate, strLtdCode);
        }
    }
}
