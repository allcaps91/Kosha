namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class EndoResultService
    {
        
        private EndoResultRepository endoResultRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public EndoResultService()
        {
			this.endoResultRepository = new EndoResultRepository();
        }

        public ENDO_RESULT GetItemBySeqno(long nSEQNO)
        {
            return endoResultRepository.GetItemBySeqno(nSEQNO);
        }
    }
}
