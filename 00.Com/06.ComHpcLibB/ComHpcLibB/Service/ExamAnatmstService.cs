namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class ExamAnatmstService
    {
        
        private ExamAnatmstRepository examAnatmstRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public ExamAnatmstService()
        {
			this.examAnatmstRepository = new ExamAnatmstRepository();
        }

        public EXAM_ANATMST GetItembyRowId(string argRowId)
        {
            return examAnatmstRepository.GetItembyRowId(argRowId);
        }

        public int Update(EXAM_ANATMST item)
        {
            return examAnatmstRepository.Update(item);
        }

        public int Insert(EXAM_ANATMST item)
        {
            return examAnatmstRepository.Insert(item);
        }

        public EXAM_ANATMST GetCountbyPaNo(string rECEIVEDATE, string strPtno)
        {
            return examAnatmstRepository.GetCountbyPaNo(rECEIVEDATE, strPtno);
        }

        public List<EXAM_ANATMST> GetItembyOrderCode()
        {
            return examAnatmstRepository.GetItembyOrderCode();
        }

        public List<EXAM_ANATMST> GetItembyOrderCodeGbJob()
        {
            return examAnatmstRepository.GetItembyOrderCodeGbJob();
        }

        public EXAM_ANATMST GetItembyPtNoJepDateOrder(string fstrPtno, string fstrJepDate, List<string> fstrOrderCode)
        {
            return examAnatmstRepository.GetItembyPtNoJepDateOrder(fstrPtno, fstrJepDate, fstrOrderCode);
        }
        public EXAM_ANATMST GetItemBySpecnoAnatno(string argSpecNo, string argAnatNO)
        {
            return examAnatmstRepository.GetItemBySpecnoAnatno(argSpecNo, argAnatNO);
        }

        public List<EXAM_ANATMST> GetItembyPtNoJepDate(string fstrPtno, string fstrJepDate)
        {
            return examAnatmstRepository.GetItembyPtNoJepDate(fstrPtno, fstrJepDate);
        }
    }
}
