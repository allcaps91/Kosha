namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class OcsOrdercodeBasSutService
    {
        
        private OcsOrdercodeBasSutRepository ocsOrdercodeBasSutRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public OcsOrdercodeBasSutService()
        {
			this.ocsOrdercodeBasSutRepository = new OcsOrdercodeBasSutRepository();
        }

        public List<OCS_ORDERCODE_BAS_SUT> GetItemAll()
        {
            return ocsOrdercodeBasSutRepository.GetItemAll();
        }

        public List<OCS_ORDERCODE_BAS_SUT> GetItemCode()
        {
            return ocsOrdercodeBasSutRepository.GetItemCode();
        }
    }
}
