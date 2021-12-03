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
    public class HicJepsuLtdResBohum1Service
    {
        
        private HicJepsuLtdResBohum1Repository hicJepsuLtdResBohum1Repository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuLtdResBohum1Service()
        {
			this.hicJepsuLtdResBohum1Repository = new HicJepsuLtdResBohum1Repository();
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyJepDateGjYearGjBangi(string strFrDate, string strToDate, string strGjYear, string strGjBangi, string strLtdCode)
        {
            return hicJepsuLtdResBohum1Repository.GetItembyJepDateGjYearGjBangi(strFrDate, strToDate, strGjYear, strGjBangi, strLtdCode);
        }

        public HIC_RES_BOHUM1 GetItembyWrtNo(long argWrtNo)
        {
            return hicJepsuLtdResBohum1Repository.GetItembyWrtNo(argWrtNo);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyJepDateGjYearGjBangi_GenSpc(string strFrDate, string strToDate, string strGjYear, string strBangi, string strJob, long nLtdCode)
        {
            return hicJepsuLtdResBohum1Repository.GetItembyJepDateGjYearGjBangi_GenSpc(strFrDate, strToDate, strGjYear, strBangi, strJob, nLtdCode);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyPaNoJepDateGjYear(long argPano, string argJepDate, string argGjYear)
        {
            return hicJepsuLtdResBohum1Repository.GetItembyPaNoJepDateGjYear(argPano, argJepDate, argGjYear);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyPaNoJepDateGjJong(long argPano, string argJepDate)
        {
            return hicJepsuLtdResBohum1Repository.GetItembyPaNoJepDateGjJong(argPano, argJepDate);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyPaNoJepDate(long argPano, string argJepDate, string strRowNum = "")
        {
            return hicJepsuLtdResBohum1Repository.GetItembyPaNoJepDate(argPano, argJepDate, strRowNum);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyJepDate()
        {
            return hicJepsuLtdResBohum1Repository.GetItembyJepDate();
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyJepDateGjYearLtdCode(string fstrFDate, string fstrTDate, string strYear, string strBangi, string fstrLtdCode)
        {
            return hicJepsuLtdResBohum1Repository.GetItembyJepDateGjYearLtdCode(fstrFDate, fstrTDate, strYear, strBangi, fstrLtdCode);
        }

        public long GetCountbyJepDateLtdCodeGjYearGjBangi(string strFDate, string strTDate, string fstrLtdCode, string strYear, string strBangi)
        {
            return hicJepsuLtdResBohum1Repository.GetCountbyJepDateLtdCodeGjYearGjBangi(strFDate, strTDate, fstrLtdCode, strYear, strBangi);
        }

        public long GetCountbyJepDateLtdCodeGjYearGjBangi_New(string strFDate, string strTDate, string fstrLtdCode, string strYear, string strBangi)
        {
            return hicJepsuLtdResBohum1Repository.GetCountbyJepDateLtdCodeGjYearGjBangi_New(strFDate, strTDate, fstrLtdCode, strYear, strBangi);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyGjYearGjjong(string strGjYear, string strGjJong, long nLtdCode, string strJob, List<string> strFirstGjJong, List<string> strDAT, string strChkFirst1, string strChkFirst2, string strSort)
        {
            return hicJepsuLtdResBohum1Repository.GetItembyGjYearGjjong(strGjYear, strGjJong, nLtdCode, strJob, strFirstGjJong, strDAT, strChkFirst1, strChkFirst2, strSort);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyGjJongPanjengDate(string strFDate, string strTDate, string strJong)
        {
            return hicJepsuLtdResBohum1Repository.GetItembyGjJongPanjengDate(strFDate, strTDate, strJong);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetPanjengPatListbyJepDate(string fstrJob, string fstrFDate, string fstrTDate, bool fbSort)
        {
            return hicJepsuLtdResBohum1Repository.GetPanjengPatListbyJepDate(fstrJob, fstrFDate, fstrTDate, fbSort);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyMunjinDrNo(long gnHicLicense, string b02_PANJENG_DRNO, long nMunjinDrNo, string idNumber)
        {
            return hicJepsuLtdResBohum1Repository.GetItembyMunjinDrNo(gnHicLicense, b02_PANJENG_DRNO, nMunjinDrNo, idNumber);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyJepDateMirNo(string argFrDate, string argToDate, long argMirno, string argChasu)
        {
            return hicJepsuLtdResBohum1Repository.GetItembyJepDateMirNo(argFrDate, argToDate, argMirno, argChasu);
        }
    }
}
