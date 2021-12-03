namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;


    /// <summary>
    /// 
    /// </summary>
    public class ExamMasterService
    {
        
        private ExamMasterRepository examMasterRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public ExamMasterService()
        {
			this.examMasterRepository = new ExamMasterRepository();
        }

        public EXAM_MASTER FindExamName(string gstrRetValue)
        {
            return examMasterRepository.FindExamName(gstrRetValue);
        }

        public string GetExamNamebyMasterCode(string argCode)
        {
            return examMasterRepository.GetExamNamebyMasterCode(argCode);
        }

        public List<EXAM_MASTER> GetMasterCodebyWsCode1(string sWsCode1Fr, string sWsCode1To)
        {
            return examMasterRepository.GetMasterCodebyWsCode1(sWsCode1Fr, sWsCode1To);
        }

        public List<EXAM_MASTER> GetHeaBarcodeBySunalDtl(long argWrtno)
        {
            return examMasterRepository.GetHeaBarcodeBySunalDtl(argWrtno);
        }

        public EXAM_MASTER GetItemsByMasterCode(string argMasterCode)
        {
            return examMasterRepository.GetItemsByMasterCode(argMasterCode);
        }
    }
}
