namespace HC.OSHA.Site.Management.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// ���� ����� �����(���)
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
