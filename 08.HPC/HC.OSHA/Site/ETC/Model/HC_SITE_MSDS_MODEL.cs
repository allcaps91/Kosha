namespace HC.OSHA.Site.ETC.Model
{
    using ComBase.Mvc;
    using System;
    using System.Drawing;


    /// <summary>
    /// 
    /// </summary>
    public class HC_SITE_MSDS_MODEL : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string PROCESS { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PRODUCTNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string USAGE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MONTHLYAMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string REVISIONDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GHSPICTURE { get; set; }
        public Bitmap GHS_IMAGE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MANUFACTURER { get; set; }

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

        
        public HC_SITE_MSDS_MODEL()
        {
        }
    }
}
