namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSpcPanjengMcodeService
    {
        
        private HicSpcPanjengMcodeRepository hicSpcPanjengMcodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcPanjengMcodeService()
        {
			this.hicSpcPanjengMcodeRepository = new HicSpcPanjengMcodeRepository();
        }

        public List<HIC_SPC_PANJENG_MCODE> GetItembyWrtNoJepDate(long fnWRTNO, string strFrDate, string strToDate)
        {
            return hicSpcPanjengMcodeRepository.GetItembyWrtNoJepDate(fnWRTNO, strFrDate, strToDate);
        }

        public int GetCountbyWrtNoUCode(long wRTNO, string strMCode)
        {
            return hicSpcPanjengMcodeRepository.GetCountbyWrtNoUCode(wRTNO, strMCode);
        }
    }
}
