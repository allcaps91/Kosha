namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 산업안전보건위언회
    /// </summary>
    public class HC_OSHA_VISIT_COMMITTEE : BaseDto
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
        /// 개최일자
        /// </summary>
		public string REGDATE { get; set; } 

        /// <summary>
        /// 참석구분
        /// </summary>
		public string METTINGTYPE { get; set; } 

        /// <summary>
        /// 회의종류
        /// </summary>
		public string MEETINGKIND { get; set; } 

        /// <summary>
        /// 참석위임자
        /// </summary>
		public string MEETINGUSER { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_VISIT_COMMITTEE()
        {
        }
    }
}
