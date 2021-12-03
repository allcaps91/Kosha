namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicResBohum1JepsuService
    {
        
        private HicResBohum1JepsuRepository hicResBohum1JepsuRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicResBohum1JepsuService()
        {
			this.hicResBohum1JepsuRepository = new HicResBohum1JepsuRepository();
        }

        public HIC_RES_BOHUM1_JEPSU GetItembyWrtNo(long fnWrtNo)
        {
            return hicResBohum1JepsuRepository.GetItembyWrtNo(fnWrtNo);
        }
    }
}
