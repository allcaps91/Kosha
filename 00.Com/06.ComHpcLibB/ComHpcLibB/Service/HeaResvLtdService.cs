namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaResvLtdService
    {
        
        private HeaResvLtdRepository heaResvLtdRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaResvLtdService()
        {
			this.heaResvLtdRepository = new HeaResvLtdRepository();
        }

        public int UpDateInwonClear(long argLtdCode, string argDate = "")
        {
            return heaResvLtdRepository.UpDateInwonClear(argLtdCode, argDate);
        }

        public HEA_RESV_LTD GetInwonAmPm(string argDate, long argLtdCode, string argGubun)
        {
            return heaResvLtdRepository.GetInwonAmPm(argDate, argLtdCode, argGubun);
        }

        public int InsertData(HEA_RESV_LTD data)
        {
            return heaResvLtdRepository.InsertData(data);
        }

        public int UpDateInwon(HEA_RESV_LTD data)
        {
            return heaResvLtdRepository.UpDateInwon(data);
        }

        public int UpDateInwon1(HEA_RESV_LTD data)
        {
            return heaResvLtdRepository.UpDateInwon1(data);
        }

        public IList<HEA_RESV_EXAM> GetItemsToGroupby(string argDate, long argLtdCode)
        {
            return heaResvLtdRepository.GetItemsToGroupby(argDate, argLtdCode);
        }

        public int DelDataByNotResv(List<long> argLtdList)
        {
            return heaResvLtdRepository.DelDataByNotResv(argLtdList);
        }

        public List<HEA_RESV_LTD> GetListByLtdCode(long argLtdCode)
        {
            return heaResvLtdRepository.GetListByLtdCode(argLtdCode);
        }

        public List<HEA_RESV_LTD> GetListInwonByLtdCode(long argLtdCode, string argSDate)
        {
            return heaResvLtdRepository.GetListInwonByLtdCode(argLtdCode, argSDate);
        }

        public HEA_RESV_LTD GetSumAmPmJanByGubun(string argDate, string argGubun)
        {
            return heaResvLtdRepository.GetSumAmPmJanByGubun(argDate, argGubun);
        }

        public int DeleteDataByRowid(string argRowid)
        {
            return heaResvLtdRepository.DeleteDataByRowid(argRowid);
        }

        public List<HEA_RESV_LTD> GetListAmPmJanBySDateGubun(string argDate, string argGbn, int argRow = 0)
        {
            return heaResvLtdRepository.GetListAmPmJanBySDateGubun(argDate, argGbn, argRow);
        }

        public List<HEA_RESV_LTD> GetListAmPmInwonBySDateGubun(string argDate, string argGbn)
        {
            return heaResvLtdRepository.GetListAmPmInwonBySDateGubun(argDate, argGbn);
        }

        public int DeleteByGubunIsNull()
        {
            return heaResvLtdRepository.DeleteByGubunIsNull();
        }
    }
}
