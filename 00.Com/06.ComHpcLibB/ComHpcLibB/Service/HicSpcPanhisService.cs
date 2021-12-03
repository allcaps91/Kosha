namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSpcPanhisService
    {
        
        private HicSpcPanhisRepository hicSpcPanhisRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcPanhisService()
        {
			this.hicSpcPanhisRepository = new HicSpcPanhisRepository();
        }

        public int GetCountbyWrtNoMCodePyoJanggi(long fnWrtNo, string strMCode, string strPyoJangi)
        {
            return hicSpcPanhisRepository.GetCountbyWrtNoMCodePyoJanggi(fnWrtNo, strMCode, strPyoJangi);
        }

        public int Insert(string strRowId)
        {
            return hicSpcPanhisRepository.Insert(strRowId);
        }

        public List<HIC_SPC_PANHIS> GetMCodeRowIdbyWrtNo(long fnWrtNo)
        {
            return hicSpcPanhisRepository.GetMCodeRowIdbyWrtNo(fnWrtNo);
        }

        public HIC_SPC_PANHIS GetItembyWrtNoMCodePyojanggi(long fnWrtNo, string strMCode, string strPyoJangi)
        {
            return hicSpcPanhisRepository.GetItembyWrtNoMCodePyojanggi(fnWrtNo, strMCode, strPyoJangi);
        }

        public List<HIC_SPC_PANHIS> GetItembyWrtNoRowId(long fnWrtNo, List<string> strList)
        {
            return hicSpcPanhisRepository.GetItembyWrtNoRowId(fnWrtNo, strList);
        }

        public HIC_SPC_PANHIS GetItembyRowId(string fstrPROWID)
        {
            return hicSpcPanhisRepository.GetItembyRowId(fstrPROWID);
        }

        public int DeleteByRowId(string strROWID)
        {
            return hicSpcPanhisRepository.DeleteByRowId(strROWID);
        }

        public int DeletebyJepDate(string strFrDate, string strToDate)
        {
            return hicSpcPanhisRepository.DeletebyJepDate(strFrDate, strToDate);
        }

        public int GetCountbyWrtNo(long wRTNO)
        {
            return hicSpcPanhisRepository.GetCountbyWrtNo(wRTNO);
        }

        public int GetPanRbyWrtNo(long wRTNO)
        {
            return hicSpcPanhisRepository.GetPanRbyWrtNo(wRTNO);
        }
    }
}
