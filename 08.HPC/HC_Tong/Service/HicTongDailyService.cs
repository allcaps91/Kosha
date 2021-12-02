namespace HC_Tong.Service
{
    using HC_Tong.Dto;
    using HC_Tong.Repository;
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// 
    /// </summary>
    public class HicTongDailyService
    {
        
        private HicTongDailyRepository hicTongDailyRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicTongDailyService()
        {
			this.hicTongDailyRepository = new HicTongDailyRepository();
        }

        public HIC_TONGDAILY GetEntTimeJobSabunByDate(string argFDate, string argTDate)
        {
            return hicTongDailyRepository.GetEntTimeJobSabunByDate(argFDate, argTDate);
        }

        public List<HIC_TONGDAILY> GetSumDailyData(string argFDate, string argTDate, string argGbn)
        {
            return hicTongDailyRepository.GetSumDailyData(argFDate, argTDate, argGbn);
        }

        public List<HIC_TONGDAILY> GetCheckUpCash(string argFDate, string argTDate, string argGbn)
        {
            return hicTongDailyRepository.GetCheckUpCash(argFDate, argTDate, argGbn);
        }

        public List<HIC_TONGDAILY> GetTotalData(string dtpFDate, string dtpTDate)
        {
            return hicTongDailyRepository.GetTotalData(dtpFDate, dtpTDate);
        }

        public List<HIC_TONGDAILY> GetCashData(string dtpFDate, string strNextDate)
        {
            return hicTongDailyRepository.GetCashData(dtpFDate, strNextDate);
        }

        internal List<HIC_TONGDAILY> GetTongData(string dtpFDate, string dtpTDate)
        {
            return hicTongDailyRepository.GetTongData(dtpFDate, dtpTDate);
        }
    }
}
