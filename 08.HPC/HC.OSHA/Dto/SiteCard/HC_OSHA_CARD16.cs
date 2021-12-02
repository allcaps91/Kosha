namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// 유해물질
    /// </summary>
    public class HC_OSHA_CARD16 : BaseDto
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
        /// 취급공정
        /// </summary>
        [MTSNotEmpty]
		public string TASKNAME { get; set; }

        /// <summary>
        /// 분류
        /// </summary>
        /// 
        [MTSNotEmpty]
        public string TASKTYPE { get; set; } 

        /// <summary>
        /// 물질명
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 사용요도
        /// </summary>
		public string USAGE { get; set; } 

        /// <summary>
        /// 취급향
        /// </summary>
		public string QTY { get; set; } 

        /// <summary>
        /// 노툴수준
        /// </summary>
		public string EXPOSURE { get; set; } 

        /// <summary>
        /// 밀폐상태
        /// </summary>
		public string COSENESS { get; set; } 

        /// <summary>
        /// 보호구상태
        /// </summary>
		public string PROTECTION { get; set; } 

        /// <summary>
        /// 게시여부
        /// </summary>
		public string ISMSDSPUBLISH { get; set; } 

        /// <summary>
        /// 경고표지 여부
        /// </summary>
		public string ISALET { get; set; } 

        /// <summary>
        /// 교육실시여부
        /// </summary>
		public string ISMSDSEDUCATION { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD16()
        {
        }
    }
}
