namespace HC.OSHA.Model
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HC_SITE_PRODUCT_MSDS_MODEL : BaseDto
    {
		public long SITE_PRODUCT_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CASNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MTSNotEmpty]
		public string QTY { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EXPOSURE_MATERIAL { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string WEM_MATERIAL { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SPECIALHEALTH_MATERIAL { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MANAGETARGET_MATERIAL { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SPECIALMANAGE_MATERIAL { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string STANDARD_MATERIAL { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PERMISSION_MATERIAL { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PSM_MATERIAL { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GHS_PICTURE { get; set; }

        public HC_SITE_PRODUCT_MSDS_MODEL()
        {
        }
    }
}
