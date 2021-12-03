using System;
namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;

    public class ExamSpecmstResultAnatmstService
    {
        private ExamSpecmstResultAnatmstRepository examSpecmstResultAnatmstRepository;

        /// <summary>
        /// 
        /// </summary>
        public ExamSpecmstResultAnatmstService()
        {
            this.examSpecmstResultAnatmstRepository = new ExamSpecmstResultAnatmstRepository();
        }

        public List<EXAM_SPECMST_RESULT_ANATMST> GetItembyMasterGroup(string strFrDate, string strToDate, List<string> strMasterGrop, string strGubun)
        {
            return examSpecmstResultAnatmstRepository.GetItembyMasterGroup(strFrDate, strToDate, strMasterGrop, strGubun);
        }
    }
}
