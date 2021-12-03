namespace HC.OSHA.Site.Management.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Enums;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// 보건관리전문 견적 테이블
    /// </summary>
    public class HC_OSHA_ESTIMATE : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        
        [MTSDecimalMinAttribute(Min = 0)]
        public long OSHA_SITE_ID { get; set; }

        /// <summary>
        /// 발행일
        /// </summary>
        [MTSNotEmpty]
        public string ESTIMATEDATE { get; set; }

        /// <summary>
        /// 업무싲가일
        /// </summary>
        [MTSNotEmpty]
        public string STARTDATE { get; set; }

        /// <summary>
        /// 총근로자수
        /// </summary>
        [MTSDecimalMinAttribute(Min = 0)]
        public long WORKERTOTALCOUNT { get; set; }

        /// <summary>
        /// 공표 수수료
        /// </summary>
        [MTSDecimalMinAttribute(Min = 0)]
        public long OFFICIALFEE { get; set; }

        /// <summary>
        /// 사업장 수수료
        /// </summary>
        [MTSDecimalMinAttribute(Min = 0)]
        public long SITEFEE { get; set; }

        /// <summary>
        /// 월별 수수료
        /// </summary>
        [MTSDecimalMinAttribute(Min = 0)]
        public long MONTHLYFEE { get; set; }

        /// <summary>
        /// 수수료산정방법
        /// </summary>
        [MTSNotEmpty]
        public string FEETYPE { get; set; } 

        /// <summary>
        /// 인쇄일시
        /// </summary>
		public DateTime? PRINTDATE { get; set; } 

        /// <summary>
        /// 메일정송일시
        /// </summary>
		public DateTime? SENDMAILDATE { get; set; }
        /// <summary>
        /// 비고
        /// </summary>
        public string REMARK { get; set; }
        /// <summary>
        /// 삭제여뷰
        /// </summary>
        public IsDeleted ISDELETED { get; set; }

        public DateTime? MODIFIED { get; set; }
        public string MODIFIEDUSER { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_ESTIMATE()
        {
        }
    }
}
