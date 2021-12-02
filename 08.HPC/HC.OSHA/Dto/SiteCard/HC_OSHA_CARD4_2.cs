namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;


    /// <summary>
    /// 사업장관리카드 4.업무개요(2)
    /// </summary>
    public class HC_OSHA_CARD4_2 : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long ESTIMATE_ID { get; set; }
        public string YEAR { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [MTSNotEmpty]
		public string TASK { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MTSNotEmpty]
        public string TASKUNIT { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string MSDS { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string WORKERCOUNT { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string WORKDESC { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD4_2()
        {
        }
    }
}
