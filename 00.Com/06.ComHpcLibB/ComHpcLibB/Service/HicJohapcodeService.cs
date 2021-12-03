namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJohapcodeService
    {
        
        private HicJohapcodeRepository hicJohapcodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJohapcodeService()
        {
			this.hicJohapcodeRepository = new HicJohapcodeRepository();
        }

        public string Read_Johap_Name(string argCode)
        {
            return hicJohapcodeRepository.Read_Johap_Name(argCode);
        }
    }
}
