namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicMunjinNightService
    {
        
        private HicMunjinNightRepository hicMunjinNightRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicMunjinNightService()
        {
			this.hicMunjinNightRepository = new HicMunjinNightRepository();
        }

        public string GetCountMunjinbyIemunNo(long nWRTNO)
        {
            return hicMunjinNightRepository.GetCountMunjinbyIemunNo(nWRTNO);
        }

        public int Insert(HIC_MUNJIN_NIGHT item)
        {
            return hicMunjinNightRepository.Insert(item);
        }

        public int GetCountbyWrtNo(long argWrtNo)
        {
            return hicMunjinNightRepository.GetCountbyWrtNo(argWrtNo);
        }

        public HIC_MUNJIN_NIGHT GetAllbyWrtNo(long argWrtNo)
        {
            return hicMunjinNightRepository.GetAllbyWrtNo(argWrtNo);
        }

        public int SavebyWrtNo(HIC_MUNJIN_NIGHT item, string strGubun)
        {
            return hicMunjinNightRepository.SavebyWrtNo(item, strGubun);
        }

        public HIC_MUNJIN_NIGHT GetItembyWrtNo(long fnWRTNO)
        {
            return hicMunjinNightRepository.GetItembyWrtNo(fnWRTNO);
        }


        public int UpDate(HIC_MUNJIN_NIGHT Item)
        {
            return hicMunjinNightRepository.UpDate(Item);
        }

        public void DeleteByWrtno(long argWrtno)
        {
            hicMunjinNightRepository.DeleteByWrtno(argWrtno);
        }
    }
}
