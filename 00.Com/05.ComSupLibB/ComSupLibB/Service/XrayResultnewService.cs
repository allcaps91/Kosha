namespace ComSupLibB.Service
{
    using System.Collections.Generic;
    using ComSupLibB.Repository;
    using ComSupLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class XrayResultnewService
    {
        
        private XrayResultnewRepository xrayResultnewRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public XrayResultnewService()
        {
			this.xrayResultnewRepository = new XrayResultnewRepository();
        }

        public XRAY_RESULTNEW GetItemByPtnoXCode(string argPtno, string argOrderCode, string argSeekDate)
        {
            return xrayResultnewRepository.GetItemByPtnoXCode(argPtno, argOrderCode, argSeekDate); 
        }
    }
}
