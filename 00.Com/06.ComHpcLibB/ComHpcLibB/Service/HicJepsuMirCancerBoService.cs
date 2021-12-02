namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuMirCancerBoService
    {
        
        private HicJepsuMirCancerBoRepository hicJepsuMirCancerBoRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuMirCancerBoService()
        {
			this.hicJepsuMirCancerBoRepository = new HicJepsuMirCancerBoRepository();
        }

        public List<HIC_JEPSU_MIR_CANCER_BO> GetListByItems(string ArgYear, string ArgFdate, string ArgTdate, string ArgJohap, string ArgJong, string ArgLtdcode)
        {
            return hicJepsuMirCancerBoRepository.GetListByItems(ArgYear, ArgFdate, ArgTdate, ArgJohap, ArgJong, ArgLtdcode);
        }
    }
}
