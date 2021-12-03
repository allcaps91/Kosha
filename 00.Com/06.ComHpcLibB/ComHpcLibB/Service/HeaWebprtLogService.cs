namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HeaWebprtLogService
    {

        private HeaWebprtLogRepository heaWebprtLogRepository;
        /// <summary>
        /// 생성자 
        /// </summary>
        public HeaWebprtLogService()
        {
            this.heaWebprtLogRepository = new HeaWebprtLogRepository();
        }

        public int Insert(HEA_WEBPRT_LOG item)
        {
            return heaWebprtLogRepository.Insert(item);
        }

        public HEA_WEBPRT_LOG GetItemByWrtnoGjjong(long artWrtno, string argGjjong)
        {
            return heaWebprtLogRepository.GetItemByWrtnoGjjong(artWrtno, argGjjong);
        }
    }
}