namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaAutopanLogicResultService
    {
        
        private HeaAutopanLogicResultRepository heaAutopanLogicResultRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaAutopanLogicResultService()
        {
			this.heaAutopanLogicResultRepository = new HeaAutopanLogicResultRepository();
        }

        public List<HEA_AUTOPAN_LOGIC_RESULT> GetItembyWrtNoSeqNo(string argWrtNo, string argSeqno, string argJepNo)
        {
            return heaAutopanLogicResultRepository.GetItembyWrtNoSeqNo(argWrtNo, argSeqno, argJepNo);
        }

        public List<HEA_AUTOPAN_LOGIC_RESULT> GetItembyWrtNoSeqNo_Second(string argWrtNo, string argSeqno, string argJepNo)
        {
            return heaAutopanLogicResultRepository.GetItembyWrtNoSeqNo_Second(argWrtNo, argSeqno, argJepNo);
        }

        public List<HEA_AUTOPAN_LOGIC_RESULT> GetItembyWrtNoSeqNo_Third(string argWrtNo, string argSeqno, string argJepNo)
        {
            return heaAutopanLogicResultRepository.GetItembyWrtNoSeqNo_Third(argWrtNo, argSeqno, argJepNo);
        }

        public List<HEA_AUTOPAN_LOGIC_RESULT> GetItembyWrtNoSeqNo_Forth(string argWrtNo, string argSeqno, string argJepNo)
        {
            return heaAutopanLogicResultRepository.GetItembyWrtNoSeqNo_Forth(argWrtNo, argSeqno, argJepNo);
        }
    }
}
