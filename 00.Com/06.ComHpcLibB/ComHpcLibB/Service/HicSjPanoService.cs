namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSjPanoService
    {
        
        private HicSjPanoRepository hicSjPanoRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSjPanoService()
        {
			this.hicSjPanoRepository = new HicSjPanoRepository();
        }

        public int UPdateGbDelbyLtdCode(string strYear, long nLtdCode)
        {
            return hicSjPanoRepository.UPdateGbDelbyLtdCode(strYear, nLtdCode);
        }

        public HIC_SJ_PANO GetItembyGjYearLtdCodeWrtNo(string strYear, long nLtdCode, long nWRTNO)
        {
            return hicSjPanoRepository.GetItembyGjYearLtdCodeWrtNo(strYear, nLtdCode, nWRTNO);
        }

        public int Insert(HIC_SJ_PANO item)
        {
            return hicSjPanoRepository.Insert(item);
        }

        public int UpdatePanjengGbDelbyRowId(HIC_SJ_PANO item)
        {
            return hicSjPanoRepository.UpdatePanjengGbDelbyRowId(item);
        }

        public int UpdatebyRowId(string strBuse, string strChukResult, string strPanjeng, string strROWID)
        {
            return hicSjPanoRepository.UpdatebyRowId(strBuse, strChukResult, strPanjeng, strROWID);
        }
    }
}
