namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// 보건교육지원
    /// </summary>
    public class HC_OSHA_VISIT_EDU : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; } 

        /// <summary>
        /// 사업장아이디
        /// </summary>
		public long SITE_ID { get; set; } 

        /// <summary>
        /// 교육일
        /// </summary>
        [MTSNotEmpty]
		public string EDUDATE { get; set; } 

        /// <summary>
        /// 종류
        /// </summary>
		public string EDUTYPE { get; set; }

        /// <summary>
        /// 교육대상
        /// </summary>
        public string TARGET { get; set; }

        /// <summary>
        /// 교육제묵
        /// </summary>
        public string TITLE { get; set; } 

        /// <summary>
        /// 교육장소
        /// </summary>
		public string LOCATION { get; set; }

        /// <summary>
        /// 실시자이이디
        /// </summary>
        [MTSNotEmpty]
        public string EDUUSERID { get; set; } 

        /// <summary>
        /// 실시자명
        /// </summary>
		public string EDUUSERNAME { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_VISIT_EDU()
        {
        }
    }
}
