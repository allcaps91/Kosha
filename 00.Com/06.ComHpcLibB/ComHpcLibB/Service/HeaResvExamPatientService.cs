namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaResvExamPatientService
    {
        
        private HeaResvExamPatientRepository heaResvExamPatientRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaResvExamPatientService()
        {
			this.heaResvExamPatientRepository = new HeaResvExamPatientRepository();
        }

        public List<HEA_RESV_EXAM_PATIENT> GetItembyRTime(string strBDateFr, string strBDateTo, long nSabun)
        {
            return heaResvExamPatientRepository.GetItembyRTime(strBDateFr, strBDateTo, nSabun);
        }

        public List<HEA_RESV_EXAM_PATIENT> GetListByLtdCode(long argLtdCode)
        {
            return heaResvExamPatientRepository.GetListByLtdCode(argLtdCode);
        }

        public HEA_RESV_EXAM_PATIENT GetItembyPtnoExam(string argPano, string argExam)
        {
            return heaResvExamPatientRepository.GetItembyPtnoExam(argPano, argExam);
        }

    }
}
