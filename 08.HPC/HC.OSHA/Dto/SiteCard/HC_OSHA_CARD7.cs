namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 7.안전보건관리규정
    /// </summary>
    public class HC_OSHA_CARD7 : BaseDto
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
        /// 
        /// </summary>
		public string RULETYPE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string RULEDATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CONTENT { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD7()
        {
        }
    }
}
