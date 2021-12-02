namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicResEtcBohum1Service
    {
        
        private HicResEtcBohum1Repository hicResEtcBohum1Repository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicResEtcBohum1Service()
        {
			this.hicResEtcBohum1Repository = new HicResEtcBohum1Repository();
        }

        public HIC_RES_ETC_BOHUM1 GetItembyWrtNo(long fnWRTNO)
        {
            return hicResEtcBohum1Repository.GetItembyWrtNo(fnWRTNO);
        }
    }
}
