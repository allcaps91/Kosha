namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicResultExcodeJepsuService
    {
        
        private HicResultExcodeJepsuRepository hicResultExcodeJepsuRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicResultExcodeJepsuService()
        {
			this.hicResultExcodeJepsuRepository = new HicResultExcodeJepsuRepository();
        }

        public List<HIC_RESULT_EXCODE_JEPSU> GetItembyWrtNo(string fstrGjChasu, long fnPano, long fnWrtno, long fnWrtno1, long fnWrtno2)
        {
            return hicResultExcodeJepsuRepository.GetItembyWrtNo(fstrGjChasu, fnPano, fnWrtno, fnWrtno1, fnWrtno2);
        }

        public List<HIC_RESULT_EXCODE_JEPSU> GetItembyOnlyWrtNo(long fnWRTNO)
        {
            return hicResultExcodeJepsuRepository.GetItembyOnlyWrtNo(fnWRTNO);
        }

        //회사추가검진 결과지 사용쿼리
        public List<HIC_RESULT_EXCODE_JEPSU> GetItemByWrtNo1(long fnWRTNO)
        {
            return hicResultExcodeJepsuRepository.GetItemByWrtNo1(fnWRTNO);
        }
        public List<HIC_RESULT_EXCODE_JEPSU> GetItemByWrtNo2(long fnWRTNO)
        {
            return hicResultExcodeJepsuRepository.GetItemByWrtNo2(fnWRTNO);
        }


    }
}
