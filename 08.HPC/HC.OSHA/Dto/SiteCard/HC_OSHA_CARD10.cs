namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// 무재해 운동추진
    /// </summary>
    public class HC_OSHA_CARD10 : BaseDto
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
        /// 게시일자
        /// </summary>
        [MTSNotEmpty]
		public string PUBLISHDATE { get; set; } 

        /// <summary>
        /// 목표시간
        /// </summary>
		public string GOAL { get; set; } 

        /// <summary>
        /// 달성시간
        /// </summary>
		public string COMPLETE { get; set; } 

        /// <summary>
        /// 목표달성
        /// </summary>
		public string STATUS { get; set; }

        public DateTime? MODIFIED { get; set; }

        public string MODIFIEDUSER { get; set; }

        public DateTime? CREATED { get; set; }

        public string CREATEDUSER { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD10()
        {
        }
    }
}
