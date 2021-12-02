namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class ExamSpecmstService
    {
        
        private ExamSpecmstRepository examSpecmstRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public ExamSpecmstService()
        {
			this.examSpecmstRepository = new ExamSpecmstRepository();
        }

        public int InsertData(EXAM_SPECMST item)
        {
            return examSpecmstRepository.InsertData(item);
        }

        public List<EXAM_SPECMST> GetListByHicList(string argFDate, string argTDate, string argBi, string argSName, string argSendFlag, string argJobFlag)
        {
            return examSpecmstRepository.GetListByHicList(argFDate, argTDate, argBi, argSName, argSendFlag, argJobFlag);
        }

        public int UpDateSendFlag(string fstrSpecNo)
        {
            return examSpecmstRepository.UpDateSendFlag(fstrSpecNo);
        }

        public List<EXAM_SPECMST> GetSpecNoByHicNo(long wRTNO, string argPtno)
        {
            return examSpecmstRepository.GetSpecNoByHicNo(wRTNO, argPtno);
        }

        public string GetResultbyPtNoMsCode(string argPTNO, string strMSCode)
        {
            return examSpecmstRepository.GetResultbyPtNoMsCode(argPTNO, strMSCode);
        }

        public List<EXAM_SPECMST> GetItembySpecNo(string strSpecNo)
        {
            return examSpecmstRepository.GetItembySpecNo(strSpecNo);
        }

        public int UpdateHicnoByPtno(long nWrtno, string strPtno, string strDeptcode)
        {
            return examSpecmstRepository.UpdateHicnoByPtno(nWrtno, strPtno, strDeptcode);
        }
    }
}
