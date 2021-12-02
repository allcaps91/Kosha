namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuHeaExjongService
    {        
        private HicJepsuHeaExjongRepository hicJepsuHeaExjongRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuHeaExjongService()
        {
			this.hicJepsuHeaExjongRepository = new HicJepsuHeaExjongRepository();
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetGbStsbyWrtNo(string strSDate, string strSName = "")
        {
            return hicJepsuHeaExjongRepository.GetGbStsbyWrtNo(strSDate, strSName);
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetWrtnobyGubun_All(string strJepDate)
        {
            return hicJepsuHeaExjongRepository.GetWrtnobyGubun_All(strJepDate);
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetWrtnobyGubun_All_HEA(string strJepDate)
        {
            return hicJepsuHeaExjongRepository.GetWrtnobyGubun_All_HEA(strJepDate);
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetWrtnobyGubun_All_HIC(string strJepDate)
        {
            return hicJepsuHeaExjongRepository.GetWrtnobyGubun_All_HIC(strJepDate);
        }

        public IList<HIC_JEPSU_HEA_EXJONG> ValidJepsu(string argPtno, string argDate)
        {
            return hicJepsuHeaExjongRepository.ValidJepsu(argPtno, argDate);
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetItembyJepDate(string strFrDate, string strToDate, string strJob, string strLtdCode, string strGjJong)
        {
            return hicJepsuHeaExjongRepository.GetItembyJepDate(strFrDate, strToDate, strJob, strLtdCode, strGjJong);
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetListWrtnoByHic(string argPtno, string argDate)
        {
            return hicJepsuHeaExjongRepository.GetListWrtnoByHic(argPtno, argDate);
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetListWrtnoByHicYear(string argPtno, string argDate, string argYear)
        {
            return hicJepsuHeaExjongRepository.GetListWrtnoByHicYear(argPtno, argDate, argYear);
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetListGaJepsuByPtnoYear(string argPtno, string argYear)
        {
            return hicJepsuHeaExjongRepository.GetListGaJepsuByPtnoYear(argPtno, argYear);
        }

        public HIC_JEPSU_HEA_EXJONG GetHeaJepInfo(string argPtno, string argDate)
        {
            return hicJepsuHeaExjongRepository.GetHeaJepInfo(argPtno, argDate);
        }

        public HIC_JEPSU_HEA_EXJONG GetHeaJepInfoByWrtno(long argWRTNO)
        {
            return hicJepsuHeaExjongRepository.GetHeaJepInfoByWrtno(argWRTNO);
        }
    }
}
