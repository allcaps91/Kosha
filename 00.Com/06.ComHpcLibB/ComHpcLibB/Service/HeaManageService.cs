namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaManageService
    {
        
        private HeaManageRepository heaManageRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaManageService()
        {
			this.heaManageRepository = new HeaManageRepository();
        }

        public string GetWrtNobyWrtNo(long nWRTNO)
        {
            return heaManageRepository.GetWrtNobyWrtNo(nWRTNO);
        }

        public int Insert(long nWRTNO, string strJEPDATE, string strLtdCode)
        {
            return heaManageRepository.Insert(nWRTNO, strJEPDATE, strLtdCode);
        }
    }
}
