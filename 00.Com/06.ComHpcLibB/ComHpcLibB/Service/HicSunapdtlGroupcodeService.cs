namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSunapdtlGroupcodeService
    {
        
        private HicSunapdtlGroupcodeRepository hicSunapdtlGroupcodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSunapdtlGroupcodeService()
        {
			this.hicSunapdtlGroupcodeRepository = new HicSunapdtlGroupcodeRepository();
        }

        public List<HIC_SUNAPDTL_GROUPCODE> GetCodeNamebyWrtNo(long nWRTNO)
        {
            return hicSunapdtlGroupcodeRepository.GetCodeNamebyWrtNo(nWRTNO);
        }

        public List<HIC_SUNAPDTL_GROUPCODE> GetCodeGbSangdambyWrtNo(long argWRTNO)
        {
            return hicSunapdtlGroupcodeRepository.GetCodeGbSangdambyWrtNo(argWRTNO);
        }

        public HIC_SUNAPDTL_GROUPCODE GetYNameByWrtno(long nWRTNO)
        {
            return hicSunapdtlGroupcodeRepository.GetYNameByWrtno(nWRTNO);
        }

        public List<HIC_SUNAPDTL_GROUPCODE> GetGbSelfGbAmbyWrtNo(long fnWrtNo)
        {
            return hicSunapdtlGroupcodeRepository.GetGbSelfGbAmbyWrtNo(fnWrtNo);
        }

        public List<HIC_SUNAPDTL_GROUPCODE> GetNamebyWrtNoInCode(long fnWRTNO, List<string> strTemp, List<string> strNotCode)
        {
            return hicSunapdtlGroupcodeRepository.GetNamebyWrtNoInCode(fnWRTNO, strTemp, strNotCode);
        }

        public List<HIC_SUNAPDTL_GROUPCODE> GetWrtNoCodeNamebyWrtNoCode(long nWRTNO, List<string> strTemp, List<string> fstrNotAddPanList)
        {
            return hicSunapdtlGroupcodeRepository.GetWrtNoCodeNamebyWrtNoCode(nWRTNO, strTemp, fstrNotAddPanList);
        }

        public List<HIC_SUNAPDTL_GROUPCODE> GetExamNamebyWrtNo(long fnWrtNo)
        {
            return hicSunapdtlGroupcodeRepository.GetExamNamebyWrtNo(fnWrtNo);
        }
    }
}
