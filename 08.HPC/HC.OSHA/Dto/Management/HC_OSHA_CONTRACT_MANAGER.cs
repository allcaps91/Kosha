namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 보건 사업장 담당자(계약)
    /// </summary>
    public class HC_OSHA_CONTRACT_MANAGER : BaseDto
    {
        public long ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long ESTIMATE_ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
	//	public string WORKER_ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string WORKER_ROLE { get; set; }

        public string NAME { get; set; }
        public string DEPT { get; set; }
        public string TEL { get; set; }
        public string HP { get; set; }
        public string EMAIL { get; set; }
        public string EMAILSEND { get; set; }

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
