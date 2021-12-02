namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicCancerChkService
    {
        
        private HicCancerChkRepository hicCancerChkRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicCancerChkService()
        {
			this.hicCancerChkRepository = new HicCancerChkRepository();
        }

        public string GetRemarkbyPanoYear(long pANO, string strYear)
        {
            return hicCancerChkRepository.GetRemarkbyPanoYear(pANO, strYear);
        }

        public int Insert(HIC_CANCER_CHK item)
        {
            return hicCancerChkRepository.Insert(item);
        }

        public int Update(HIC_CANCER_CHK item)
        {
            return hicCancerChkRepository.Update(item);
        }

        public string GetRowIdbyPanoYear(string strPano, string strYear)
        {
            return hicCancerChkRepository.GetRowIdbyPanoYear(strPano, strYear);
        }
    }
}
