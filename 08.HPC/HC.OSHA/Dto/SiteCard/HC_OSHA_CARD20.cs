namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 사업장 만족도
    /// </summary>
    public class HC_OSHA_CARD20 : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SITE_ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long ESTIMATE_ID { get; set; }

        public string YEAR { get; set; }

        /// <summary>
        /// 상하반기
        /// </summary>
        public string QUARTER { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string STATISFACTION { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NAME { get; set; }

        /// <summary>
        /// 서명
        /// </summary>
        public string SITESIGN { get; set; }

        /// <summary>
        /// 서명여부
        /// </summary>
        public string ISSIGN { get; set; }

        public DateTime? MODIFIED { get; set; }
        public string MODIFIEDUSER { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }
       
    }
}
