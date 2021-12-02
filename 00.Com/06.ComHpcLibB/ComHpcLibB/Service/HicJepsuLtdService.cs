namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuLtdService
    {
        
        private HicJepsuLtdRepository hicJepsuLtdRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuLtdService()
        {
			this.hicJepsuLtdRepository = new HicJepsuLtdRepository();
        }

        public List<HIC_JEPSU_LTD> GetItembyJepDateGjYearGjBangiLtdCode(string strFDate, string strTDate, string strYear, string strBangi, long nLtdCode)
        {
            return hicJepsuLtdRepository.GetItembyJepDateGjYearGjBangiLtdCode(strFDate, strTDate, strYear, strBangi, nLtdCode);
        }

        public List<HIC_JEPSU_LTD> GetItembyJepDateGjYearGjBangiLtdCode_New(string strFDate, string strTDate, string strYear, string strBangi, long nLtdCode)
        {
            return hicJepsuLtdRepository.GetItembyJepDateGjYearGjBangiLtdCode_New(strFDate, strTDate, strYear, strBangi, nLtdCode);
        }

        public List<HIC_JEPSU_LTD> GetListByItems(string strFDate, string strTDate)
        {
            return hicJepsuLtdRepository.GetListByItems(strFDate, strTDate);
        }

        public List<HIC_JEPSU_LTD> GetNamebyWrtNo(string fstrWRTNO)
        {
            return hicJepsuLtdRepository.GetNamebyWrtNo(fstrWRTNO);
        }
    }
}
