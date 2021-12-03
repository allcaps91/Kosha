namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class BasIllsService
    {
        
        private BasIllsRepository basIllsRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public BasIllsService()
        {
			this.basIllsRepository = new BasIllsRepository();
        }

        public string GetIllNameKbyIllCode(string argCode)
        {
            return basIllsRepository.GetIllNameKbyIllCode(argCode);
        }

        public List<BAS_ILLS> GetListByIllNameK(string strName)
        {
            return basIllsRepository.GetListByIllNameK(strName);
        }
    }
}
