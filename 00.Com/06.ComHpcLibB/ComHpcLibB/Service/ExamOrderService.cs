namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using System.Windows.Forms;


    /// <summary>
    /// 
    /// </summary>
    public class ExamOrderService
    {
        
        private ExamOrderRepository examOrderRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public ExamOrderService()
        {
			this.examOrderRepository = new ExamOrderRepository();
        }

        public int UpDate(EXAM_ORDER itemSub01, string strSuCode)
        {
            return examOrderRepository.UpDate(itemSub01, strSuCode);
        }

        public string GetRowidByBDatePanoMasterCD(string argBDate, string argPtno, string argBi, string argMasterCD)
        {
            return examOrderRepository.GetRowidByBDatePanoMasterCD(argBDate, argPtno, argBi, argMasterCD);
        }

        public bool Insert(EXAM_ORDER item)
        {
            try
            {
                examOrderRepository.Insert(item);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
