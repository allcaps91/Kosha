namespace ComHpcLibB.Service
{
    using ComHpcLibB.Model;
    using ComHpcLibB.Repository;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class HicLastThisYearService
    {
        
        private HicLastThisYearRepository hicLastThisYearRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicLastThisYearService()
        {
			this.hicLastThisYearRepository = new HicLastThisYearRepository();
        }

        public List<COMHPC> GetStatistics(string strSdate, string strEdate)
        {
            return hicLastThisYearRepository.GetStatistics(strSdate, strEdate);

        }
    }
}
