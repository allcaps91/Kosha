namespace HC.OSHA.Model
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HC_OSHA_CONTRACT_MANAGER_MODEL : BaseDto
    {
        public long ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public long ESTIMATE_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string WORKER_ID { get; set; }

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
