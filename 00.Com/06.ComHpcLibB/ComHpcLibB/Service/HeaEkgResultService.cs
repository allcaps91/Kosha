namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaEkgResultService
    {
        
        private HeaEkgResultRepository heaEkgResultRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaEkgResultService()
        {
			this.heaEkgResultRepository = new HeaEkgResultRepository();
        }

        public string GetResultbyWrtNo(long fnWRTNO)
        {
            return heaEkgResultRepository.GetResultbyWrtNo(fnWRTNO);
        }

        public string GetRowIdbyWrtNo(long fnWRTNO)
        {
            return heaEkgResultRepository.GetRowIdbyWrtNo(fnWRTNO);
        }

        public int DeletebyRowId(string strRowId)
        {
            return heaEkgResultRepository.DeletebyRowId(strRowId);
        }

        public int UpdateResult(string strResult, string strRowId)
        {
            return heaEkgResultRepository.UpdateResult(strResult, strRowId);
        }

        public int InsertAll(long nWrtNo, string strResult)
        {
            return heaEkgResultRepository.UpdateResult(nWrtNo, strResult);
        }
    }
}
