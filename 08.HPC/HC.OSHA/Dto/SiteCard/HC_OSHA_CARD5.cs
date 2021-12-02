namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// ÀÔÅð»çÀÚ
    /// </summary>
    public class HC_OSHA_CARD5 : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long ESTIMATE_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long SITE_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string REGISTERDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MTSZero]
		public long JOINCOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MTSZero]
        public long QUITCOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? MODIFIED { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MODIFIEDUSER { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? CREATED { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string CREATEDUSER { get; set; }

        
        public HC_OSHA_CARD5()
        {
        }
    }
}
