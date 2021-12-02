namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSjJindanService
    {
        
        private HicSjJindanRepository hicSjJindanRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSjJindanService()
        {
			this.hicSjJindanRepository = new HicSjJindanRepository();
        }

        public int Insert(string strGjYear, long nLtdCode, string strPan, string strBuse, string strYuhe, string strJanggi, long nInwon)
        {
            return hicSjJindanRepository.Insert(strGjYear, nLtdCode, strPan, strBuse, strYuhe, strJanggi, nInwon);
        }

        public int Delete(string strROWID)
        {
            return hicSjJindanRepository.Delete(strROWID);
        }

        public int Update(string strPan, string strBuse, string strYuhe, string strJanggi, long nInwon, string strROWID)
        {
            return hicSjJindanRepository.Update(strPan, strBuse, strYuhe, strJanggi, nInwon, strROWID);
        }

        public int DeletebyGjYearLtdCode(string strGjYear, long nLtdCode)
        {
            return hicSjJindanRepository.DeletebyGjYearLtdCode(strGjYear, nLtdCode);
        }

        public HIC_SJ_JINDAN GetInwonRowIdbyGjYear(string strGjYear, long nLtdCode, string strPanjeng, string strBuseName, string strYuhe)
        {
            return hicSjJindanRepository.GetInwonRowIdbyGjYear(strGjYear, nLtdCode, strPanjeng, strBuseName, strYuhe);
        }

        public int UpdateInwon(long nInwon, string rID)
        {
            return hicSjJindanRepository.UpdateInwon(nInwon, rID);
        }

        public List<HIC_SJ_JINDAN> GetItembyGjYearLtdCode(string strGjYear, long nLtdCode)
        {
            return hicSjJindanRepository.GetItembyGjYearLtdCode(strGjYear, nLtdCode);
        }
    }
}
