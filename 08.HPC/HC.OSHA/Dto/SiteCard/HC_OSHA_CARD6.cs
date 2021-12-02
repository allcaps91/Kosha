namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 산업재해
    /// </summary>
    public class HC_OSHA_CARD6 : BaseDto
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
		public string IND_ACC_TYPE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string JUMIN_NO { get; set; } 

        /// <summary>
        /// 발생일
        /// </summary>
		public string ACC_DATE { get; set; } 

        /// <summary>
        /// 산재요양 승인일
        /// </summary>
		public string APPROVE_DATE { get; set; } 

        /// <summary>
        /// 산재요양 신청일
        /// </summary>
		public string REQUEST_DATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string ILLNAME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string REMARK { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD6()
        {
        }
    }
}
