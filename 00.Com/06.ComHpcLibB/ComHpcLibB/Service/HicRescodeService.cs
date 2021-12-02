namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;


    /// <summary>
    /// 
    /// </summary>
    public class HicRescodeService
    {
        
        private HicRescodeRepository hicRescodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicRescodeService()
        {
			this.hicRescodeRepository = new HicRescodeRepository();
        }
        
        public List<HIC_RESCODE> Read_ResCode(string strGubun)
        {
            return hicRescodeRepository.Read_ResCode(strGubun);
        }
        
        public HIC_RESCODE Read_ResCode_Single(string strGubun, string strCode)
        {
            return hicRescodeRepository.Read_ResCode_Single(strGubun, strCode);
        }

        public List<HIC_RESCODE> Read_HIc_ResCode(string strGubun, string strCode, string strBun)
        {
            return hicRescodeRepository.Read_HIc_ResCode(strGubun, strCode, strBun);
        }

        public string Read_Hic_ResCodeName(string strGubun, string strCode)
        {
            return hicRescodeRepository.Read_Hic_ResCodeName(strGubun, strCode);
        }

        public List<HIC_RESCODE> Read_Res_ComboSet(string argGubun)
        {
            return hicRescodeRepository.Read_Res_ComboSet(argGubun);
        }

        public string GetNameByGubun(string argCode)
        {
            return hicRescodeRepository.GetNameByGubun(argCode);
        }
        
        public List<HIC_RESCODE> Read_Hic_ResCode_All(string argGubun)
        {
            return hicRescodeRepository.Read_Hic_ResCode_All(argGubun);
        }

        public List<HIC_RESCODE> GetCodeNamebyResCode(string strResCode)
        {
            return hicRescodeRepository.GetCodeNamebyResCode(strResCode);
        }

        public List<HIC_RESCODE> GetCodeNamebyGubun(string strYear)
        {
            return hicRescodeRepository.GetCodeNamebyGubun(strYear);
        }

        public List<HIC_RESCODE> GetCodeNamebyBindGubun(string strResCode)
        {
            return hicRescodeRepository.GetCodeNamebyBindGubun(strResCode);
        }

        public HIC_RESCODE GetCodeNamebyGubunCode(string strResCode, string strResult)
        {
            return hicRescodeRepository.GetCodeNamebyGubunCode(strResCode, strResult);
        }
    }
}
