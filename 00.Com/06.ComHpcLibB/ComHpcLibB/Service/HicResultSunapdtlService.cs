
namespace ComHpcLibB.Service
{

    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicResultSunapdtlService
    {
        private HicResultSunapdtlRepository hicResultSunapdtlRepository;

        public HicResultSunapdtlService()
        {
            hicResultSunapdtlRepository = new HicResultSunapdtlRepository();
        }

        public List<HIC_RESULT_SUNAPDTL> GetItembyWrtnoExcodeIN(long argWrtno, List<string> lstExcode)
        {
            return hicResultSunapdtlRepository.GetItembyWrtnoExcodeIN(argWrtno, lstExcode);
        }

        public List<HIC_RESULT_SUNAPDTL> GetExCodeResultGbSelfbyWrtNo(long argWRTNO, string argLtd, string argOK3)
        {
            return hicResultSunapdtlRepository.GetExCodeResultGbSelfbyWrtNo(argWRTNO, argLtd, argOK3);
        }

        public List<HIC_RESULT_SUNAPDTL> GetItembyWrtNoExCode(long nWRTNO, string eXCODE)
        {
            return hicResultSunapdtlRepository.GetItembyWrtNoExCode(nWRTNO, eXCODE);
        }

        public List<HIC_RESULT_SUNAPDTL> GetCodebyWrtNo(long nWRTNO)
        {
            return hicResultSunapdtlRepository.GetCodebyWrtNo(nWRTNO);
        }

        public List<HIC_RESULT_SUNAPDTL> GetCodebyWrtNoExCode(long nWRTNO, string eXCODE)
        {
            return hicResultSunapdtlRepository.GetCodebyWrtNoExCode(nWRTNO, eXCODE);
        }

        public int GetCountbyWrtNoExCode(long nWRTNO, string eXCODE)
        {
            return hicResultSunapdtlRepository.GetCountbyWrtNoExCode(nWRTNO, eXCODE);
        }
    }
}
