namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class ExamResultcService
    {
        
        private ExamResultcRepository examResultcRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public ExamResultcService()
        {
			this.examResultcRepository = new ExamResultcRepository();
        }

        public string ChkExcuteExamByPanoCode(string argPano, string argSpecNo, string argMasterCode, string argSubCode)
        {
            return examResultcRepository.ChkExcuteExamByPanoCode(argPano, argSpecNo, argMasterCode, argSubCode);
        }

        public int InsertData(EXAM_RESULTC item)
        {
            return examResultcRepository.InsertData(item);
        }

        public List<EXAM_RESULTC> GetListBySpecno(string argSpecNo)
        {
            return examResultcRepository.GetListBySpecno(argSpecNo);
        }

        public List<EXAM_RESULTC> GetABODataBySpecNo(long argWRTNO, string fstrBDate)
        {
            return examResultcRepository.GetABODataBySpecNo(argWRTNO, fstrBDate);
        }

        public string GetRowidByMstCodeSpecNoIN(string strPtno, string strMasterCode, List<string> lstSpecno)
        {
            return examResultcRepository.GetRowidByMstCodeSpecNoIN(strPtno, strMasterCode, lstSpecno);
        }

        public List<EXAM_RESULTC> GetMasterCodebySpecNoMasterCode(string sSpecNo, string sMasterCode)
        {
            return examResultcRepository.GetMasterCodebySpecNoMasterCode(sSpecNo, sMasterCode);
        }

        //public string GetResultbyPTNO_RESULT_BÇü°£¿°(long ArgWRTNO, string strPtno, string strMasterCode)
        //{
        //    return examResultcRepository.GetResultbyPTNO_RESULT_BÇü°£¿°(ArgWRTNO, strPtno, strMasterCode);
        //}
    }
}
