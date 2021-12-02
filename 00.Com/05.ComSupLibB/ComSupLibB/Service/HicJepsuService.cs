namespace ComSupLibB.Service
{
    using System.Collections.Generic;
    using ComSupLibB.Repository;
    using ComSupLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuService
    {
        
        private HicJepsuRepository hicJepsuRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuService()
        {
			this.hicJepsuRepository = new HicJepsuRepository();
        }

        public HIC_JEPSU GetItemByPtnoSDate(string argPtno, string argSDate)
        {
            return hicJepsuRepository.GetItemByPtnoSDate(argPtno, argSDate);
        }
    }
}
