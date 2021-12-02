namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuResSpecialService
    {
        
        private HicJepsuResSpecialRepository hicJepsuResSpecialRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResSpecialService()
        {
			this.hicJepsuResSpecialRepository = new HicJepsuResSpecialRepository();
        }

        public List<HIC_JEPSU_RES_SPECIAL> GetItembyJepDateLtdCodeGjYear(string fstrFDate, string fstrTDate, string fstrLtdCode, string strGjYear, string strBangi, string strJob)
        {
            return hicJepsuResSpecialRepository.GetItembyJepDateLtdCodeGjYear(fstrFDate, fstrTDate, fstrLtdCode, strGjYear, strBangi, strJob);
        }

        public List<HIC_JEPSU_RES_SPECIAL> GetItembyMunjinJepDateLtdCodeGjYear(string fstrFDate, string fstrTDate, string fstrLtdCode, string strGjYear, string strBangi, string strJob)
        {
            return hicJepsuResSpecialRepository.GetItembyMunjinJepDateLtdCodeGjYear(fstrFDate, fstrTDate, fstrLtdCode, strGjYear, strBangi, strJob);
        }

        public List<HIC_JEPSU_RES_SPECIAL> GetItembyMunjinNameLtdCodeCount(string strFrate, string strToDate, string strGjYear, string strGjBangi, long nLtdCode)
        {
            return hicJepsuResSpecialRepository.GetItembyMunjinNameLtdCodeCount(strFrate, strToDate, strGjYear, strGjBangi, nLtdCode);
        }

        public List<HIC_JEPSU_RES_SPECIAL> GetItembyJepDateGjYearPanDrNo(string strFrDate, string strToDate, string strGjYear)
        {
            return hicJepsuResSpecialRepository.GetItembyJepDateGjYearPanDrNo(strFrDate, strToDate, strGjYear);
        }

        public List<HIC_JEPSU_RES_SPECIAL> GetItembyPaNoJepDateWrtNo(long argPano, string argJepDate, long fnWrtno1, long fnWrtno2)
        {
            return hicJepsuResSpecialRepository.GetItembyPaNoJepDateWrtNo(argPano, argJepDate, fnWrtno1, fnWrtno2);
        }

        public List<HIC_JEPSU_RES_SPECIAL> GetItemsbyJepDate(string strFrDate, string strToDate, string strJob, long nLtdCode, string strJong)
        {
            return hicJepsuResSpecialRepository.GetItemsbyJepDate(strFrDate, strToDate, strJob, nLtdCode, strJong);
        }

        public List<HIC_JEPSU_RES_SPECIAL> GetItembyJepDateGjYear(string strFrDate, string strToDate, string strGjYear)
        {
            return hicJepsuResSpecialRepository.GetItembyJepDateGjYear(strFrDate, strToDate, strGjYear);
        }

        public HIC_JEPSU_RES_SPECIAL GetWrtNoJepDatebyJepDatePaNoGjYear(string strDate, long pANO, string strGjYear)
        {
            return hicJepsuResSpecialRepository.GetWrtNoJepDatebyJepDatePaNoGjYear(strDate, pANO, strGjYear);
        }

        public HIC_JEPSU_RES_SPECIAL GetItembyJepDatePaNoGjYear(string strDate, long pANO, string strGjYear)
        {
            return hicJepsuResSpecialRepository.GetItembyJepDatePaNoGjYear(strDate, pANO, strGjYear);
        }
        public List<HIC_JEPSU_RES_SPECIAL> GetItembyPanoLtdYearChasuGjjong(long artPano, string argLtdcode, string argGjyear, string argChasu, string argGjjong, string argOpt1)
        {
            return hicJepsuResSpecialRepository.GetItembyPanoLtdYearChasuGjjong(artPano, argLtdcode, argGjyear, argChasu, argGjjong, argOpt1);
        }
    }
}
