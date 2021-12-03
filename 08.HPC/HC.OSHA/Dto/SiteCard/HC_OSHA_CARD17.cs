namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// 사업장 안전보건교육
    /// </summary>
    public class HC_OSHA_CARD17 : BaseDto
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
        /// 일자
        /// </summary>
        [MTSNotEmpty]
		public string EDUDATE { get; set; }

        /// <summary>
        /// 교육종류
        /// </summary>
        [MTSNotEmpty]
        public string EDUTYPE { get; set; } 

        /// <summary>
        /// 장소
        /// </summary>
		public string EDUPLACE { get; set; }

        /// <summary>
        /// 교육방법
        /// </summary>
        [MTSNotEmpty]
        public string EDUUSAGE { get; set; } 

        /// <summary>
        /// 실시자
        /// </summary>
		public string EDUNAME { get; set; } 

        /// <summary>
        /// 대상인원
        /// </summary>
		public string TARGETCOUNT { get; set; } 

        /// <summary>
        /// 참석인원
        /// </summary>
		public string ACTCOUNT { get; set; } 

        /// <summary>
        /// 교육내용
        /// </summary>
		public string CONTENT { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD17()
        {
        }
    }
}
