namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSchoolNewJepsuService
    {
        
        private HicSchoolNewJepsuRepository hicSchoolNewJepsuRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSchoolNewJepsuService()
        {
			this.hicSchoolNewJepsuRepository = new HicSchoolNewJepsuRepository();
        }

        public HIC_SCHOOL_NEW_JEPSU GetCntbyGjJong(string strGjJong, long nLicense)
        {
            return hicSchoolNewJepsuRepository.GetCntbyGjJong(strGjJong, nLicense);
        }
    }
}
