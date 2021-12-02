namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSpcPanjengScodeService
    {
        
        private HicSpcPanjengScodeRepository hicSpcPanjengScodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcPanjengScodeService()
        {
			this.hicSpcPanjengScodeRepository = new HicSpcPanjengScodeRepository();
        }

        public List<HIC_SPC_PANJENG_SCODE> GetItembyWrtNo(long fnWRTNO)
        {
            return hicSpcPanjengScodeRepository.GetItembyWrtNo(fnWRTNO);
        }
    }
}
