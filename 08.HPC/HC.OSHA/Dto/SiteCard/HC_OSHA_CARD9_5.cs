namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// 9. 위험성평가 인정제도의 참여
    /// </summary>
    public class HC_OSHA_CARD9_5 : BaseDto
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

        /// <summary>
        /// 인정신청일
        /// </summary>
		public string REQUESTDATE { get; set; } 

        /// <summary>
        /// 인정결정일
        /// </summary>
		public string APPROVEDATE { get; set; } 

        /// <summary>
        /// 인증여부
        /// </summary>
        [MTSNotEmpty]
		public string ISAPPROVE { get; set; }

        public string ISAPPROVE_TEXT { get; set; }

        /// <summary>
        /// 인증유효기가
        /// </summary>
        public string STARTDATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string ENDDATE { get; set; }

        public string PERIOD { get; set; }
        /// <summary>
        /// 기타
        /// </summary>
        public string REMARK { get; set; }


       
        public DateTime? MODIFIED { get; set; }
        
		public string MODIFIEDUSER { get; set; }

		public DateTime? CREATED { get; set; }

		public string CREATEDUSER { get; set; }
    }
}
