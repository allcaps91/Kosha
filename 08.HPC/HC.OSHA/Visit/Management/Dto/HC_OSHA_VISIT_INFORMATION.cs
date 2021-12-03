namespace HC.OSHA.Visit.Management.Dto
{
    using ComBase.Mvc;
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
		public string REGUSERID { get; set; } 

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
