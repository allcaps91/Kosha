namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class ExamSpecodeService
    {
        
        private ExamSpecodeRepository examSpecodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public ExamSpecodeService()
        {
			this.examSpecodeRepository = new ExamSpecodeRepository();
        }

        public EXAM_SPECODE GetItemByCodeGubun(string argSpecCode, string argGubun)
        {
            return examSpecodeRepository.GetItemByCodeGubun(argSpecCode, argGubun);
        }

        public List<EXAM_SPECODE> GetNamebyCode(string vOLUMECODE)
        {
            return examSpecodeRepository.GetNamebyCode(vOLUMECODE);
        }
    }
}
