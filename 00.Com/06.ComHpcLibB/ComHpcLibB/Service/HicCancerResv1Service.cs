namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicCancerResv1Service
    {
        
        private HicCancerResv1Repository hicCancerResv1Repository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicCancerResv1Service()
        {
			this.hicCancerResv1Repository = new HicCancerResv1Repository();
        }

        public int Insert(HIC_CANCER_RESV1 item)
        {
            return hicCancerResv1Repository.Insert(item);
        }

        public int Update1(HIC_CANCER_RESV1 item)
        {
            return hicCancerResv1Repository.Update1(item);
        }

        public int Update2(HIC_CANCER_RESV1 item)
        {
            return hicCancerResv1Repository.Update2(item);
        }

        public List<HIC_CANCER_RESV1> GetItembyJobDate(string strFDate, string strTDate)
        {
            return hicCancerResv1Repository.GetItembyJobDate(strFDate, strTDate);
        }

        public HIC_CANCER_RESV1 GetItembyJobDate(string strDate)
        {
            return hicCancerResv1Repository.GetItembyJobDate(strDate);
        }
    }
}
