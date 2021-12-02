namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class ExamMasterSubService
    {
        
        private ExamMasterSubRepository examMasterSubRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public ExamMasterSubService()
        {
			this.examMasterSubRepository = new ExamMasterSubRepository();
        }

        public List<EXAM_MASTER_SUB> GetNormalByCode(string argCode)
        {
            return examMasterSubRepository.GetNormalByCode(argCode);
        }
    }
}
