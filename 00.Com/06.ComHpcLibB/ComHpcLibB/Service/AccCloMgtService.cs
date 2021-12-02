namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;
    using ComHpcLibB.Repository;


    /// <summary>
    /// 
    /// </summary>
    public class AccCloMgtService
    {
        
        private AccCloMgtRepository AccCloMgtRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public AccCloMgtService()
        {
			this.AccCloMgtRepository = new AccCloMgtRepository();
        }

        public List<ACC_CLO_MGT> GetMagamDay(string strCLOYMD)
        {
            return AccCloMgtRepository.GetMagamDay(strCLOYMD);
        }
       
    }
}
