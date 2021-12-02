namespace HC.OSHA.Site.Management.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HC_OSHA_CONTRACT_MANAGER_MODEL : BaseDto
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
		public string NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string DEPT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string TEL { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string HP { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EMAIL { get; set; }

        
        public HC_OSHA_CONTRACT_MANAGER_MODEL()
        {
        }
    }
}
