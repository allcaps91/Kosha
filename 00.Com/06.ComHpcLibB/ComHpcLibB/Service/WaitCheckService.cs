namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComHpcLibB.Model;
    
    /// <summary>
    /// 
    /// </summary>
    public class WaitCheckService
    {        
        private WaitCheckRepository waitCheckRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public WaitCheckService()
        {
			this.waitCheckRepository = new WaitCheckRepository();
        }

        public List<WAIT_CHECK> Read_Wait(string SDate, string HeaPart)
        {
            return waitCheckRepository.Read_Wait(SDate, HeaPart);
        }

        public List<WAIT_CHECK> Read_Wait_Hic(string SDate, string HeaPart, string strGB)
        {
            return waitCheckRepository.Read_Wait_Hic(SDate, HeaPart, strGB);
        }

        public HEA_SANGDAM_WAIT Read_Exam_Wait(string SDate, string HeaPart)
        {
            return waitCheckRepository.Read_Exam_Wait(SDate, HeaPart);
        }
        
        public int Read_Exam_Wait_Hic(List<string> strGbWait)
        {
            return waitCheckRepository.Read_Exam_Wait_Hic(strGbWait);
        }

        public List<WAIT_CHECK> Read_Wait_All(string argDate)
        {
            return waitCheckRepository.Read_Wait_All(argDate);
        }
    }
}
