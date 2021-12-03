namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicAmtCancerService
    {
        
        private HicAmtCancerRepository hicAmtCancerRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicAmtCancerService()
        {
			this.hicAmtCancerRepository = new HicAmtCancerRepository();
        }

        public IList<HIC_AMT_CANCER> FindAll()
        {
            return hicAmtCancerRepository.FindAll();
        }

        public IList<HIC_AMT_CANCER> GetListBySDate(string strDate)
        {
            return hicAmtCancerRepository.GetListBySDate(strDate);
        }

        public int DeleteByRowid(string argRowid)
        {
            return hicAmtCancerRepository.DeleteByRowid(argRowid);
        }

        public List<HIC_AMT_CANCER> GetItembySDate(string strFDate, string strSDate)
        {
            return hicAmtCancerRepository.GetItembySDate(strFDate, strSDate);
        }
    }
}
