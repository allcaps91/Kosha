namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaValuationService
    {
        
        private HeaValuationRepository heaValuationRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaValuationService()
        {
			this.heaValuationRepository = new HeaValuationRepository();
        }

        public HEA_VALUATION GetAllbyWrtNo(long fnWRTNO)
        {
            return heaValuationRepository.GetAllbyWrtNo(fnWRTNO);
        }

        public int Delete(long fnWRTNO)
        {
            return heaValuationRepository.Delete(fnWRTNO);
        }

        public int Insert(HEA_VALUATION item)
        {
            return heaValuationRepository.Insert(item);
        }

        public int Update(HEA_VALUATION item)
        {
            return heaValuationRepository.Update(item);
        }
    }
}
