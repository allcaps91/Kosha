namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSpcSahuService
    {
        
        private HicSpcSahuRepository hicSpcSahuRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcSahuService()
        {
			this.hicSpcSahuRepository = new HicSpcSahuRepository();
        }

        public int GetRowIdbyWrtNo(long fnWRTNO, string fstrGjYear, string fstrLtdCode)
        {
            return hicSpcSahuRepository.GetRowIdbyWrtNo(fnWRTNO, fstrGjYear, fstrLtdCode);
        }

        public int Insert(HIC_SPC_SAHU item)
        {
            return hicSpcSahuRepository.Insert(item);
        }
    }
}
