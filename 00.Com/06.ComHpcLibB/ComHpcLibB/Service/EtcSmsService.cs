namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class EtcSmsService
    {
        
        private EtcSmsRepository etcSmsRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public EtcSmsService()
        {
			this.etcSmsRepository = new EtcSmsRepository();
        }

        public List<ETC_SMS> GetListByBigoGubun(string argSmsLtdCode, string argGubun)
        {
            return etcSmsRepository.GetListByBigoGubun(argSmsLtdCode, argGubun);
        }

        public int Insert(ETC_SMS eSMS)
        {
            return etcSmsRepository.Insert(eSMS);
        }

        public int GetCountbyRTimePanoDeptcode(string strTime, string strPANO, string strDeptCode)
        {
            return etcSmsRepository.GetCountbyRTimePanoDeptcode(strTime, strPANO, strDeptCode);
        }

        public int InsertAll(ETC_SMS item)
        {
            return etcSmsRepository.InsertAll(item);
        }
    }
}
