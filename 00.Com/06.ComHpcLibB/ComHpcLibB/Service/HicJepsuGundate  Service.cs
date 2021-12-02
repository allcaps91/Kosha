namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuGundateService
    {
        private HicJepsuGundateRepository hicJepsuGundateRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicJepsuGundateService()
        {
            this.hicJepsuGundateRepository = new HicJepsuGundateRepository();
        }

        public List<HIC_JEPSU_GUNDATE> GetItembyGjYear(string strGjYear)
        {
            return hicJepsuGundateRepository.GetItembyGjYear(strGjYear);
        }

        public List<HIC_JEPSU_GUNDATE> GetItembyJepDateGjYear(string fstrFDate, string fstrTDate, string strGjYear, string fstrLtdCode)
        {
            return hicJepsuGundateRepository.GetItembyJepDateGjYear(fstrFDate, fstrTDate, strGjYear, fstrLtdCode);
        }

        public HIC_JEPSU_GUNDATE GetItembyWrtNo(string fstrFDate, string fstrTDate, string strGjYear, string fstrLtdCode, string strJob)
        {
            return hicJepsuGundateRepository.GetItembyWrtNo(fstrFDate, fstrTDate, strGjYear, fstrLtdCode, strJob);
        }

        public List<HIC_JEPSU_GUNDATE> GetItembyInLineWrtNo(string fstrFDate, string fstrTDate, string strJob, string fstrGjBangi, string strGjYear, string fstrLtdCode)
        {
            return hicJepsuGundateRepository.GetItembyInLineWrtNo(fstrFDate, fstrTDate, strJob, fstrGjBangi, strGjYear, fstrLtdCode);
        }

        public HIC_JEPSU_GUNDATE GetGunDateByWrtno(long fnWrtno)
        {
            return hicJepsuGundateRepository.GetGunDateByWrtno(fnWrtno);
        }

        public List<HIC_JEPSU_GUNDATE> GetItembyJepDateGjYearBogunso(string strFrDate, string strToDate, string strYear, long nLtdCode, long nWrtNo, string strJep, string strDel, string strJong, string strJong1, string strJohap, string strBogunso, string strSort)
        {
            return hicJepsuGundateRepository.GetItembyJepDateGjYearBogunso(strFrDate, strToDate, strYear, nLtdCode, nWrtNo, strJep, strDel, strJong, strJong1, strJohap, strBogunso, strSort);
        }
    }
}
