namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJinGbnService
    {
        
        private HicJinGbnRepository hicJinGbnRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJinGbnService()
        {
			this.hicJinGbnRepository = new HicJinGbnRepository();
        }

        public string GetItembyWrtNo(long nWrtNo)
        {
            return hicJinGbnRepository.GetItembyWrtNo(nWrtNo);
        }

        public int Update(HIC_JIN_GBN item)
        {
            return hicJinGbnRepository.Update(item);
        }

        public int PanJengDrNoUpdate(HIC_JIN_GBN item)
        {
            return hicJinGbnRepository.PanJengDrNoUpdate(item);
        }

        public int PrintUpdate(HIC_JIN_GBN item)
        {
            return hicJinGbnRepository.PrintUpdate(item);
        }

        public int Insert(HIC_JIN_GBN item)
        {
            return hicJinGbnRepository.Insert(item);
        }

        public HIC_JIN_GBN GetItemByWrtno(long nWRTNO)
        {
            return hicJinGbnRepository.GetItemByWrtno(nWRTNO);
        }
    }
}
