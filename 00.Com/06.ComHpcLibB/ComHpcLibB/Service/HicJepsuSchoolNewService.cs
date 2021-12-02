namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuSchoolNewService
    {
        
        private HicJepsuSchoolNewRepository hicJepsuSchoolNewRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuSchoolNewService()
        {
			this.hicJepsuSchoolNewRepository = new HicJepsuSchoolNewRepository();
        }

        public List<HIC_JEPSU_SCHOOL_NEW> GetItembyJepDate(string strFDate, string strTDate, string strJob, string strSName, long nLtdCode, long nLicense)
        {
            return hicJepsuSchoolNewRepository.GetItembyJepDate(strFDate, strTDate, strJob, strSName, nLtdCode, nLicense);
        }

        public HIC_JEPSU_SCHOOL_NEW GetItembyJepDateWrtNo(string strFrDate, string strToDate, long nWrtNo)
        {
            return hicJepsuSchoolNewRepository.GetItembyJepDateWrtNo(strFrDate, strToDate, nWrtNo);
        }

        public List<HIC_JEPSU_SCHOOL_NEW> GetItembyJepDateClassBanBun(string strFrDate, string strToDate, string strSName, long nLtdCode, string strClass, string strBan, string strBun, string strSort)
        {
            return hicJepsuSchoolNewRepository.GetItembyJepDateClassBanBun(strFrDate, strToDate, strSName, nLtdCode, strClass, strBan, strBun, strSort);
        }
    }
}
