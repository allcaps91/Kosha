namespace HC.OSHA.Site.Management.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Enums;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HC_SITE_WORKER : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SITEID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        [MTSNotEmpty]
		public string NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MTSNotEmpty]
        public string WORKER_ROLE { get; set; } 

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
        [MTSEmail]
		public string EMAIL { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string ISDELETED { get; set; }

        [MTSNotEmpty]
        public string ISRETIRE { get; set; }

        public DateTime? MODIFIED { get; set; }
        public string MODIFEDUSER { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public HC_SITE_WORKER()
        {
            ISRETIRE = "N";
            ISDELETED = "N";
        }
    }
}
