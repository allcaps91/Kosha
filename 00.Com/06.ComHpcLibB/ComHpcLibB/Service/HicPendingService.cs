namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicPendingService
    {
        
        private HicPendingRepository hicPendingRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicPendingService()
        {
			this.hicPendingRepository = new HicPendingRepository();
        }

        public HIC_PENDING GetIetmbyWrtNo(long fnWrtNo, string strGubun)
        {
            return hicPendingRepository.GetIetmbyWrtNo(fnWrtNo, strGubun);
        }

        public int DeletebyRowId(string fstrROWID)
        {
            return hicPendingRepository.DeletebyRowId(fstrROWID);
        }

        public int Insert(HIC_PENDING item)
        {
            return hicPendingRepository.Insert(item);
        }

        public int Update(HIC_PENDING item)
        {
            return hicPendingRepository.Update(item);
        }
    }
}
