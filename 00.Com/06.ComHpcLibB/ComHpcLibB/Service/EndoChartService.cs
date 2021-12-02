namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class EndoChartService
    {
        
        private EndoChartRepository endoChartRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public EndoChartService()
        {
			this.endoChartRepository = new EndoChartRepository();
        }

        public int UpdatebyRowId(ENDO_CHART item3)
        {
            return endoChartRepository.UpdatebyRowId(item3);
        }

        public string GetRowIdbyPtNoJepDate(string fstrPtno, string fstrJepDate)
        {
            return endoChartRepository.GetRowIdbyPtNoJepDate(fstrPtno, fstrJepDate);
        }

        public ENDO_CHART GetItembyRowId(string argEndoRowId)
        {
            return endoChartRepository.GetItembyRowId(argEndoRowId);
        }

        public int GetCountbyPtnoRDate(string argPtno, string argJepDate)
        {
            return endoChartRepository.GetCountbyPtnoRDate(argPtno, argJepDate);
        }

        public int Insert(ENDO_CHART item)
        {
            return endoChartRepository.Insert(item);
        }
    }
}
