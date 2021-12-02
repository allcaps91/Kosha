namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicSpcPanjengJepsuService
    {
        
        private HicSpcPanjengJepsuRepository hicSpcPanjengJepsuRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcPanjengJepsuService()
        {
			this.hicSpcPanjengJepsuRepository = new HicSpcPanjengJepsuRepository();
        }

        public List<HIC_SPC_PANJENG_JEPSU> GetItembyJepDateWrtnoLtdCode(string strFrDate, string strToDate, long nWrtNo, long nLtdCode)
        {
            return hicSpcPanjengJepsuRepository.GetItembyJepDateWrtnoLtdCode(strFrDate, strToDate, nWrtNo, nLtdCode);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtNo(long gnWRTNO)
        {
            return hicSpcPanjengJepsuRepository.GetItembyWrtNo(gnWRTNO);
        }
    }
}
