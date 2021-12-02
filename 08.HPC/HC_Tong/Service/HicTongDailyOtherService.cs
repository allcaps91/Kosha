namespace HC_Tong.Service
{
    using HC_Tong.Model;
    using HC_Tong.Repository;
    using System.Collections.Generic;


    /// <summary>
    /// 
    /// </summary>
    public class HicTongDailyOtherService
    {
        
        private HicTongDailyOtherRepository hicTongDailyExjongRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicTongDailyOtherService()
        {
			this.hicTongDailyExjongRepository = new HicTongDailyOtherRepository();
        }

        public List<HIC_TONGDAILY_OTHER> GetTongData(string argFDate, string argTDate, bool rdoBtnCheck)
        {
            return hicTongDailyExjongRepository.GetTongData(argFDate, argTDate, rdoBtnCheck);
        }
    }
}
