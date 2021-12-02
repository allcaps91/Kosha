namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaAutopanLogicService
    {
        
        private HeaAutopanLogicRepository heaAutopanLogicRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaAutopanLogicService()
        {
			this.heaAutopanLogicRepository = new HeaAutopanLogicRepository();
        }

        public int GetCountbyWrtNo(string argWrtNo)
        {
            return heaAutopanLogicRepository.GetCountbyWrtNo(argWrtNo);
        }

        public string GetExCodebyWrtNo(string argWrtNo)
        {
            return heaAutopanLogicRepository.GetExCodebyWrtNo(argWrtNo);
        }

        public int GetCountbyWrtNoSeqNo(string argWrtNo, string argSeqno)
        {
            return heaAutopanLogicRepository.GetCountbyWrtNoSeqNo(argWrtNo, argSeqno);
        }

        public int GetCountbyWrtNoSeqNoJepNo(string argWrtNo, string argSeqno, string argJepNo)
        {
            return heaAutopanLogicRepository.GetCountbyWrtNoSeqNoJepNo(argWrtNo, argSeqno, argJepNo);
        }

        public int GetCountbyWrtNoSeqNoJepNo_Implement(string argWrtNo, string argSeqno, string argJepNo, string argLogic)
        {
            return heaAutopanLogicRepository.GetCountbyWrtNoSeqNoJepNo_Implement(argWrtNo, argSeqno, argJepNo, argLogic);
        }
    }
}
