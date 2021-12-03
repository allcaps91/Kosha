namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;


    /// <summary>
    /// 
    /// </summary>
    public class OcsSubcodeService
    {
        
        private OcsSubcodeRepository ocsSubcodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public OcsSubcodeService()
        {
			this.ocsSubcodeRepository = new OcsSubcodeRepository();
        }

        public List<OCS_SUBCODE> FindSubCode(string strOrderCode)
        {
            return ocsSubcodeRepository.FindSubCode(strOrderCode);
        }
    }
}
