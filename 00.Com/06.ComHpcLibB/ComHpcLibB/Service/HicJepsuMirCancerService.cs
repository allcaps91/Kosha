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
    public class HicJepsuMirCancerService
    {
        
        private HicJepsuMirCancerRepository hicJepsuMirCancerRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuMirCancerService()
        {
			this.hicJepsuMirCancerRepository = new HicJepsuMirCancerRepository();
        }

        public List<HIC_JEPSU_MIR_CANCER> GetListByItems(string ArgYear, string ArgFdate, string ArgTdate, string ArgJohap,string ArgJong, string ArgLtdcode)
        {
            return hicJepsuMirCancerRepository.GetListByItems(ArgYear, ArgFdate, ArgTdate, ArgJohap, ArgJong, ArgLtdcode);
        }




    }
}
