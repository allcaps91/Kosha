namespace HC.OSHA.Site.Management.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 보건 사업장 담당자(계약)
    /// </summary>
    public class HC_OSHA_CONTRACT_MANAGER : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ESTIMATE_ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long WORKER_ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string WORKER_ROLE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string ISDELETED { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CONTRACT_MANAGER()
        {
        }
    }
}
