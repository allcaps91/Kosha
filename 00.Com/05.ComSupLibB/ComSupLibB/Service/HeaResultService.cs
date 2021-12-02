namespace ComSupLibB.Service
{
    using System.Collections.Generic;
    using ComSupLibB.Repository;
    using ComSupLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaResultService
    {
        
        private HeaResultRepository heaResultRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaResultService()
        {
			this.heaResultRepository = new HeaResultRepository();
        }

        public string GetResultByWrtnoExCD(long argWRTNO, string argExCode)
        {
            return heaResultRepository.GetResultByWrtnoExCD(argWRTNO, argExCode);
        }

        public int UpDate(string argResult, long argSabun, long argWRTNO, string argExCode)
        {
            return heaResultRepository.UpDate(argResult, argSabun, argWRTNO, argExCode);
        }
    }
}
