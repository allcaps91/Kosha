namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HC_SITE_PRODUCT_MSDS : BaseDto
    {
        public long ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public long SITE_PRODUCT_ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long MSDS_ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string QTY { get; set; }

        public DateTime? MODIFIED { get; set; }
        public string MODIFIEDUSER { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }
      
    }
}
