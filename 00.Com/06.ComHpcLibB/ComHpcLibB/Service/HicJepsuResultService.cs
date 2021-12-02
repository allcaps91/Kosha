namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuResultService
    {
        
        private HicJepsuResultRepository hicJepsuResultRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResultService()
        {
			this.hicJepsuResultRepository = new HicJepsuResultRepository();
        }

        public List<HIC_JEPSU_RESULT> GetItemByWrtnoResult(long fnWRTNO)
        {
            return hicJepsuResultRepository.GetItemByWrtnoResult(fnWRTNO);
        }
        
        public List<HIC_JEPSU_RESULT> GetNoActingbyWrtNo(long fnWRTNO)
        {
            return hicJepsuResultRepository.GetNoActingbyWrtNo(fnWRTNO);
        }

        public List<HIC_JEPSU_RESULT> GetActingInfobyBDate(string argBDate)
        {
            return hicJepsuResultRepository.GetActingInfobyBDate(argBDate);
        }

        public List<HIC_JEPSU_RESULT> GetActingInfobyBDate_Chul(string argBDate)
        {
            return hicJepsuResultRepository.GetActingInfobyBDate_Chul(argBDate);
        }

        public List<HIC_JEPSU_RESULT> GetWrtNobySysDate(string SORT)
        {
            return hicJepsuResultRepository.GetWrtNobySysDate(SORT);
        }

        public List<HIC_JEPSU_RESULT> GetItem()
        {
            return hicJepsuResultRepository.GetItem();
        }

        public HIC_JEPSU_RESULT GetItembyPtNo(string strJepDate, string strPtNo)
        {
            return hicJepsuResultRepository.GetItembyPtNo(strJepDate, strPtNo);
        }

        public List<HIC_JEPSU_RESULT> GetItembyJepDate(string strFrDate, string strToDate, string strGubun)
        {
            return hicJepsuResultRepository.GetItembyJepDate(strFrDate, strToDate, strGubun);
        }

        public List<HIC_JEPSU_RESULT> GetItembyJepDateExCode(string strFrDate, string strToDate, string strExCode)
        {
            return hicJepsuResultRepository.GetItembyJepDateExCode(strFrDate, strToDate, strExCode);
        }

        public List<HIC_JEPSU_RESULT> GetItemAll()
        {
            return hicJepsuResultRepository.GetItemAll();
        }

        public List<HIC_JEPSU_RESULT> GetItembyExCode(string strExCode)
        {
            return hicJepsuResultRepository.GetItembyExCode(strExCode);
        }

        public List<HIC_JEPSU_RESULT> GetItembyJepDateExCode(string strFrDate, string strToDate)
        {
            return hicJepsuResultRepository.GetItembyJepDateExCode(strFrDate, strToDate);
        }

        public List<HIC_JEPSU_RESULT> GetItembyJepDateJong(string strFrDate, string strToDate, string strSName, string strGubun, long nLtdCode, string strJong, string strGjJong, List<string> strTemp, string strSort, string strGbStart)
        {
            return hicJepsuResultRepository.GetItembyJepDateJong(strFrDate, strToDate, strSName, strGubun, nLtdCode, strJong, strGjJong, strTemp, strSort, strGbStart);
        }

        public List<HIC_JEPSU_RESULT> GetListSpcExamByPtno(string argPtno)
        {
            return hicJepsuResultRepository.GetListSpcExamByPtno(argPtno);
        }

        public List<HIC_JEPSU_RESULT> GetItembyJepDatePart(string strJepDate, string strPart, string strGb)
        {
            return hicJepsuResultRepository.GetItembyJepDatePart(strJepDate, strPart, strGb);
        }

        public List<HIC_JEPSU_RESULT> GetHeaItembySDate(string strJepDate)
        {
            return hicJepsuResultRepository.GetHeaItembySDate(strJepDate);
        }

        public List<HIC_JEPSU_RESULT> GetItembyJepDateGbChulExCode(string argBDate)
        {
            return hicJepsuResultRepository.GetItembyJepDateGbChulExCode(argBDate);
        }

        public List<HIC_JEPSU_RESULT> GetItembyGbChulExCode()
        {
            return hicJepsuResultRepository.GetItembyGbChulExCode();
        }
        public List<HIC_JEPSU_RESULT> GetCountJepdateExcodeResult(string argFDATE, string argTDATE, string argExcode, string argResult)
        {
            return hicJepsuResultRepository.GetCountJepdateExcodeResult(argFDATE, argTDATE, argExcode, argResult);
        }

        public List<HIC_JEPSU_RESULT> GetListJepDateExCodeIN(string strDate, List<string> strExCodes)
        {
            return hicJepsuResultRepository.GetListJepDateExCodeIN(strDate, strExCodes);
        }
    }
}
