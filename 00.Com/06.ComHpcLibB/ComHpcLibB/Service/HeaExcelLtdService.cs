namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaExcelLtdService
    {
        
        private HeaExcelLtdRepository heaExcelLtdRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaExcelLtdService()
        {
			this.heaExcelLtdRepository = new HeaExcelLtdRepository();
        }

        public List<HEA_EXCEL_LTD> GetLtdNamebyYear(string text, string strChk, string strName = "")
        {
            return heaExcelLtdRepository.GetLtdNamebyYear(text, strChk, strName);
        }

        public List<HEA_EXCEL_LTD> GetItemAllbyYear(string text, string strChk)
        {
            return heaExcelLtdRepository.GetItemAllbyYear(text, strChk);
        }
    }
}
