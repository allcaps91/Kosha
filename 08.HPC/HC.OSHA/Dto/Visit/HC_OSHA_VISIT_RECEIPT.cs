namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 수령증 발급
    /// </summary>
    public class HC_OSHA_VISIT_RECEIPT : BaseDto
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
		public string REGDATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string REGUSERID { get; set; }

        public string REGUSERNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CANCELDATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TAKEOVER { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string REMARK { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_VISIT_RECEIPT()
        {
        }
    }
}
