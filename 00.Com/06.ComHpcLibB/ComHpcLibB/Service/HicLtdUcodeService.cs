namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicLtdUcodeService
    {
        
        private HicLtdUcodeRepository hicLtdUcodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicLtdUcodeService()
        {
			this.hicLtdUcodeRepository = new HicLtdUcodeRepository();
        }

        public IList<HIC_LTD_UCODE> GetListByCodeJong(long argLtdCode, string argJong)
        {
            return hicLtdUcodeRepository.GetListByCodeJong(argLtdCode, argJong);
        }

        public List <HIC_LTD_UCODE> GetListByCodeJong1(long argLtdCode, string argJong)
        {
            return hicLtdUcodeRepository.GetListByCodeJong1(argLtdCode, argJong);
        }

        public HIC_LTD_UCODE GetItemByRowid(string argROWID)
        {
            return hicLtdUcodeRepository.GetItemByRowid(argROWID);
        }

        public HIC_LTD_UCODE GetItemByLtdCode(long argLtdCode, string strJobName = "")
        {
            return hicLtdUcodeRepository.GetItemByLtdCode(argLtdCode, strJobName);
        }

        public HIC_LTD_UCODE GetRowidByLtdCodeLikeName(long fnLtdCode, string strBuse, string strUCodes)
        {
            return hicLtdUcodeRepository.GetRowidByLtdCodeLikeName(fnLtdCode, strBuse, strUCodes);
        }

        public HIC_LTD_UCODE GetMaxNameByLtdCodeLikeName(long fnLtdCode, string strBuse)
        {
            return hicLtdUcodeRepository.GetMaxNameByLtdCodeLikeName(fnLtdCode, strBuse);
        }

        public int InsertData(HIC_LTD_UCODE item)
        {
            return hicLtdUcodeRepository.InsertData(item);
        }

        public int DeleteByRowid(string fstrROWID)
        {
            return hicLtdUcodeRepository.DeleteByRowid(fstrROWID);
        }

        public int UpDateDateByItem(HIC_LTD_UCODE item)
        {
            return hicLtdUcodeRepository.UpDateDateByItem(item);
        }
    }
}
