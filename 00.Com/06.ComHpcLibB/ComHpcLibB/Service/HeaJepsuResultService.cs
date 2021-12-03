namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuResultService
    {
        
        private HeaJepsuResultRepository heaJepsuResultRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuResultService()
        {
			this.heaJepsuResultRepository = new HeaJepsuResultRepository();
        }

        public List<HEA_JEPSU_RESULT> GetItembySDate(string argBDate, string argGubun)
        {
            return heaJepsuResultRepository.GetItembySDate(argBDate, argGubun);
        }

        public int UpdateResultActiveEntSabunbyRowIdWrtNo(string strActValue, string srActive, long idNumber, string strROWID, long nWRTNO, string argGubun)
        {
            return heaJepsuResultRepository.UpdateResultActiveEntSabunbyRowIdWrtNo(strActValue, srActive, idNumber, strROWID, nWRTNO, argGubun);
        }

        public List<HEA_JEPSU_RESULT> GetItembyPart(string strPart)
        {
            return heaJepsuResultRepository.GetItembyPart(strPart);
        }

        public List<HEA_JEPSU_RESULT> GetListBySDatePano(string argCurDate, long nPano)
        {
            return heaJepsuResultRepository.GetListBySDatePano(argCurDate, nPano);
        }

        public List<HEA_JEPSU_RESULT> GetItembySDate(string strSDate)
        {
            return heaJepsuResultRepository.GetItembySDate(strSDate);
        }
    }
}
