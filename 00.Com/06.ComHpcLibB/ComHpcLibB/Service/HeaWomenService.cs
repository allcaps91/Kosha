namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HeaWomenService
    {        
        private HeaWomenRepository heaWomenRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaWomenService()
        {
			this.heaWomenRepository = new HeaWomenRepository();
        }

        public HEA_WOMEN Read_Women_Reference(long nWrtNo)
        {
            return heaWomenRepository.Read_Women_Reference(nWrtNo);
        }

        public int Merge_Women_Reference(string strMIN_ONE, string strMAX_ONE, string strMIN_TWO, string strMAX_TWO, long nWRTNO, string strCode)
        {
            return heaWomenRepository.Merge_Women_Reference(strMIN_ONE, strMAX_ONE, strMIN_TWO, strMAX_TWO, nWRTNO, strCode);
        }
    }
}
