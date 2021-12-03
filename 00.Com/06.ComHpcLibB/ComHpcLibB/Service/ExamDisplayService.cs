namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;    
    
    /// <summary>
    /// 
    /// </summary>
    public class ExamDisplayService
    {        
        private ExamDisplayRepository examDisplayRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public ExamDisplayService()
        {
			this.examDisplayRepository = new ExamDisplayRepository();
        }
        
        public List<EXAM_DISPLAY> Read_ExamList(long nWrtNo, List<string> strPartG, string strGubun, List<long> AllWrtNo)
        {
            return examDisplayRepository.Read_ExamList(nWrtNo, strPartG, strGubun, AllWrtNo);
        }

        public List<EXAM_DISPLAY> Read_Result(long nWrtNo, string[] strExCode)
        {
            return examDisplayRepository.Read_Result(nWrtNo, strExCode);
        }
    }
}
