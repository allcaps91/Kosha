namespace ComHpcLibB.Service
{
    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// 
    /// </summary>
    public class HicTongamService
    {

        private HicTongamRepository hicTongamRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicTongamService()
        {
            this.hicTongamRepository = new HicTongamRepository();
        }

        public List<HIC_TONGAM> GetCancerCount(string strSdate, string strEdate)
        {
            return hicTongamRepository.GetCancerCount(strSdate, strEdate);
        }

        public List<HIC_TONGAM> GetCancerTong(string dtpFDate, string dtpTDate, string cboJONG)
        {
            return hicTongamRepository.GetCancerTong(dtpFDate, dtpTDate, cboJONG);
        }

        public List<HIC_TONGAM> GetCancerData(string dtpFDate, string dtpTDate)
        {
            return hicTongamRepository.GetCancerData(dtpFDate, dtpTDate);
        }
    }
}
