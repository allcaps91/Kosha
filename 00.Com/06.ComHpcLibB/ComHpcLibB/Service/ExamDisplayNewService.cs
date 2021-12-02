
namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Model;
    using ComHpcLibB.Repository;

    public class ExamDisplayNewService
    {

        private ExamDisplayNewRepository examDisplayNewRepository;

        public ExamDisplayNewService()
        {
            this.examDisplayNewRepository = new ExamDisplayNewRepository();
        }

        public IList<EXAM_DISPLAY_NEW> GetItemsInResultExCode(long argWRTNO, string[] strExamList = null)
        {
            return examDisplayNewRepository.GetItemsInResultExCode(argWRTNO, strExamList);
        }
        public IList<EXAM_DISPLAY_NEW> GetItemsInResultHaExCode(long argWRTNO, string[] strExamList = null)
        {
            return examDisplayNewRepository.GetItemsInResultHaExCode(argWRTNO, strExamList);
        }


        public IList<EXAM_DISPLAY_NEW> GetItemsInJepsuResult(string Ptno, string JepDate, string[] strExcodes = null)
        {
            return examDisplayNewRepository.GetItemsInJepsuResult(Ptno, JepDate, strExcodes);
        }

        public EXAM_DISPLAY_NEW GetItemForBloodActing(long argWrtno)
        {
            return examDisplayNewRepository.GetItemForBloodActing(argWrtno);
        }

        public long GetWrtnoForBloodActing(long argPano, string argDate)
        {
            return examDisplayNewRepository.GetWrtnoForBloodActing(argPano, argDate);
        }
    }
}
