namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class InsaMsthService
    {
        
        private InsaMsthRepository insaMsthRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public InsaMsthService()
        {
			this.insaMsthRepository = new InsaMsthRepository();
        }

        public string GetGunTaebySabunYear(string strSabun, string strYear)
        {
            return insaMsthRepository.GetGunTaebySabunYear(strSabun, strYear);
        }
    }
}
