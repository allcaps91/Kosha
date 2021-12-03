namespace HC.Core.BaseCode.MSDS.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HC_MSDS : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CHEMID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CASNO { get; set; } 

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
        /// 허용기준
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

        public DateTime? MODIFIED { get; set; }
        public string MODIFIEDUSER { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }














        /// <summary>
        /// 
        /// </summary>
        public HC_MSDS()
        {
        }
    }
}
