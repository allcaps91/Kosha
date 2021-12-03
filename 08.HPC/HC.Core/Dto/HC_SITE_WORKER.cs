namespace HC.Core.Dto
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
		public string ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SITEID { get; set; }
        public string PTNO { get; set; }
        public long PANO { get; set; }
        public string IPSADATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MTSNotEmpty]
		public string NAME { get; set; }
   //     [MTSNotEmpty]
        public string JUMIN { get; set; }
        public string GENDER { get; set; }
        public string AgeAndGender { get; set; }
        public int AGE { get; set; }
        

        /// <summary>
        /// 
        /// </summary>
        [MTSNotEmpty]
        public string WORKER_ROLE { get; set; }

        /// <summary>
        /// 
        /// </summary>
   //     [MTSNotEmpty]
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
       // [MTSEmail]
		public string EMAIL { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string ISDELETED { get; set; }

        /// <summary>
        /// 중점관리대상 여부
        /// </summary>
        public string ISMANAGEOSHA { get; set; }
        
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
            WORKER_ROLE = "EMP_ROLE";
        }
    }
}
