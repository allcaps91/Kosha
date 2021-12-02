namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicResultH827Service
    {
        
        private HicResultH827Repository hicResultH827Repository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicResultH827Service()
        {
			this.hicResultH827Repository = new HicResultH827Repository();
        }

        public HIC_RESULT_H827 GetBloodDatebyWrtNoExCode(long nWRTNO, string strExCode)
        {
            return hicResultH827Repository.GetBloodDatebyWrtNoExCode(nWRTNO, strExCode);
        }

        public int Insert(HIC_RESULT_H827 item)
        {
            return hicResultH827Repository.Insert(item);
        }

        public int Update(HIC_RESULT_H827 item, string strBloodDate)
        {
            return hicResultH827Repository.Update(item, strBloodDate);
        }

        public int Delete(string strFrDate, string strToDate)
        {
            return hicResultH827Repository.Delete(strFrDate, strToDate);
        }

        public int UpdateRemarkbyWrtNo(string strRemark, long nWRTNO)
        {
            return hicResultH827Repository.UpdateRemarkbyWrtNo(strRemark, nWRTNO);
        }
    }
}
