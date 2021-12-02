namespace HC.Core.Model
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;


    /// <summary>
    /// 검진 접수 내역
    /// </summary>
    public class HealthCareReciptModel : BaseDto
    {
        public long SiteId { get; set; }
        public string SiteName { get; set; }
        public long WRTNO { get; set; }
        public string PANO { get; set; }
        public string NAME { get; set; }
        public string JEPDATE { get; set; }

        public string BuseName { get; set; }
        public string IpsaDate { get; set; }
        public string Tel { get; set; }

        public string UCodes { get; set; }
        
        public int AGE { get; set; }
        /// <summary>
        /// 검진종류
        /// </summary>
        public string EXNAME { get; set; }

    }
}
