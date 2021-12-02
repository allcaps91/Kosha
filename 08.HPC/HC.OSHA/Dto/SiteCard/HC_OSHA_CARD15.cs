namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// 위험물질
    /// </summary>
    public class HC_OSHA_CARD15 : BaseDto
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
        /// 취급공정
        /// </summary>
        [MTSNotEmpty]
        public string TASKNAME { get; set; }

        /// <summary>
        /// 분류
        /// </summary>
        [MTSNotEmpty]
        public string TASKTYPE { get; set; } 

        /// <summary>
        /// 물질명
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 취급량
        /// </summary>
		public string QTY { get; set; } 

        /// <summary>
        /// 조치상황
        /// </summary>
		public string STATUS { get; set; } 

        /// <summary>
        /// 변동사항
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 관리상태
        /// </summary>
		public string MANAGESTATUS { get; set; }

        public string YEAR { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD15()
        {
        }
    }
}
