namespace ComSupLibB.Service
{
    using System.Collections.Generic;
    using ComSupLibB.Repository;
    using ComSupLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicResultService
    {
        
        private HicResultRepository hicResultRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicResultService()
        {
			this.hicResultRepository = new HicResultRepository();
        }

        public string GetResultByWrtnoExCD(long argWRTNO, string argExCode)
        {
            return hicResultRepository.GetResultByWrtnoExCD(argWRTNO, argExCode);
        }

        public int UpDate(string argResult, long argSabun, long argWRTNO, string argExCode)
        {
            return hicResultRepository.UpDate(argResult, argSabun, argWRTNO, argExCode);
        }
    }
}
