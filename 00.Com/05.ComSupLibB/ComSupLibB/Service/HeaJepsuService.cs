namespace ComSupLibB.Service
{
    using System.Collections.Generic;
    using ComSupLibB.Repository;
    using ComSupLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuService
    {
        
        private HeaJepsuRepository heaJepsuRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuService()
        {
			this.heaJepsuRepository = new HeaJepsuRepository();
        }

        public HEA_JEPSU GetItemByPtnoSDate(string argPtno, string argSDate)
        {
            return heaJepsuRepository.GetItemByPtnoSDate(argPtno, argSDate);
        }
    }
}
