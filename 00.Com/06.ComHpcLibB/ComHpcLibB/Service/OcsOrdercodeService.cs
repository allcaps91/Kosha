namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;


    /// <summary>
    /// 
    /// </summary>
    public class OcsOrdercodeService
    {
        
        private OcsOrdercodeRepository ocsOrdercodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public OcsOrdercodeService()
        {
			this.ocsOrdercodeRepository = new OcsOrdercodeRepository();
        }

        public List<OCS_ORDERCODE> FindSlipNo(string argSlipNo)
        {
            return ocsOrdercodeRepository.FindSlip(argSlipNo);
        }

        public string GetOrderCodebyItemCd(string fstrORDERCODE)
        {
            return ocsOrdercodeRepository.GetOrderCodebyItemCd(fstrORDERCODE);
        }

        public OCS_ORDERCODE GetItembyOrderCode(string argOrderCode)
        {
            return ocsOrdercodeRepository.GetItembyOrderCode(argOrderCode);
        }
    }
}
