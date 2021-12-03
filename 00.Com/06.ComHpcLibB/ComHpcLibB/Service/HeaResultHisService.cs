namespace ComHpcLibB.Service
{

    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HeaResultHisService 
    {
        private HeaResultHisRepository heaResultHisRepository;
        /// <summary>
        /// 생성자 
        /// </summary>
        public HeaResultHisService()
        {
            this.heaResultHisRepository = new HeaResultHisRepository();
        }

        public int Result_History_Insert2(long SABUN, string RESULT, long WRTNO, string EXCODE)
        {
            return heaResultHisRepository.Result_History_Insert2(SABUN, RESULT, WRTNO, EXCODE);
        }

        public int Result_History_Update(string RESULT, long ENTSABUN, long WRTNO, string EXCODE)
        {
            return heaResultHisRepository.Result_History_Update(RESULT, ENTSABUN, WRTNO, EXCODE);
        }

    }
}
