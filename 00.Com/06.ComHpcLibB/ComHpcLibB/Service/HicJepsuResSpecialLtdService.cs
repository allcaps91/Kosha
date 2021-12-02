namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuResSpecialLtdService
    {
        
        private HicJepsuResSpecialLtdRepository hicJepsuResSpecialLtdRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResSpecialLtdService()
        {
			this.hicJepsuResSpecialLtdRepository = new HicJepsuResSpecialLtdRepository();
        }

        public List<HIC_JEPSU_RES_SPECIAL_LTD> GetItembyJepDateGjYearGjBangi(string strFrDate, string strToDate, string strGjYear, string strBangi, string strJob, string strLtdCode)
        {
            return hicJepsuResSpecialLtdRepository.GetItembyJepDateGjYearGjBangi(strFrDate, strToDate, strGjYear, strBangi, strJob, strLtdCode);
        }

        public List<HIC_JEPSU_RES_SPECIAL_LTD> GetItemCountbyJepDateGjYearGjBangi(string strFrDate, string strToDate, string strGjYear, string strBangi, string strJob, string strLtdCode)
        {
            return hicJepsuResSpecialLtdRepository.GetItemCountbyJepDateGjYearGjBangi(strFrDate, strToDate, strGjYear, strBangi, strJob, strLtdCode);
        }

        public List<HIC_JEPSU_RES_SPECIAL_LTD> GetItembyJepDateGjBangiLtdCode(string strFrDate, string strToDate, long nLtdCode, string strJob, string fstrGjBangi, string strGjYear)
        {
            return hicJepsuResSpecialLtdRepository.GetItembyJepDateGjBangiLtdCode(strFrDate, strToDate, nLtdCode, strJob, fstrGjBangi, strGjYear);
        }

        public List<HIC_JEPSU_RES_SPECIAL_LTD> GetSpcItemCountbyJepDateGjYearGjBangi(string strFrDate, string strToDate, string fstrGjYear, string fstrGjBangi, string strJob, string strLtdCode)
        {
            return hicJepsuResSpecialLtdRepository.GetSpcItemCountbyJepDateGjYearGjBangi(strFrDate, strToDate, fstrGjYear, fstrGjBangi, strJob, strLtdCode);
        }

        public List<HIC_JEPSU_RES_SPECIAL_LTD> GetItembyJepDateGjYearGjJongLtdCodeWrtNo(string strFDate, string strTDate, string strYear, long nLtdCode, long nWrtNo, string strJong, string strDel, string strSort)
        {
            return hicJepsuResSpecialLtdRepository.GetItembyJepDateGjYearGjJongLtdCodeWrtNo(strFDate, strTDate, strYear, nLtdCode, nWrtNo, strJong, strDel, strSort);
        }

        public List<HIC_JEPSU_RES_SPECIAL_LTD> GetItembyJepDateGjYear(string strFrDate, string strToDate, string strRdoChk, string strRdoBook, string strYear, long nLtdCode, string sSort = "")
        {
            return hicJepsuResSpecialLtdRepository.GetItembyJepDateGjYear(strFrDate, strToDate, strRdoChk, strRdoBook, strYear, nLtdCode, sSort);
        }
    }
}
