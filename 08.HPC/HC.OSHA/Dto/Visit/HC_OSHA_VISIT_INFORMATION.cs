namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// 정보자료제공 대장
    /// </summary>
    public class HC_OSHA_VISIT_INFORMATION : BaseDto
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
        [MTSNotEmpty]
		public string REGUSERID { get; set; }
        public string REGUSERNAME { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
		public string INFORMATIONTYPE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string REMARK { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_VISIT_INFORMATION()
        {
        }
    }
}
