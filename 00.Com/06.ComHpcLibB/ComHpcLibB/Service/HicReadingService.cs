namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicReadingService
    {
        
        private HicReadingRepository hicReadingRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicReadingService()
        {
			this.hicReadingRepository = new HicReadingRepository();
        }

        public List<HIC_READING> GetListByYear(string argYear = "", string argCode = "")
        {
            return hicReadingRepository.GetListByYear(argYear, argCode);
        }

        public int Delete(string fstrRowid)
        {
            return hicReadingRepository.Delete(fstrRowid);
        }

        public string GetItemByRowid(string strROWID)
        {
            return hicReadingRepository.GetItemByRowid(strROWID);
        }

        public int Insert(HIC_READING item)
        {
            return hicReadingRepository.Insert(item);
        }

        public int UpDate(HIC_READING item)
        {
            return hicReadingRepository.UpDate(item);
        }
    }
}
