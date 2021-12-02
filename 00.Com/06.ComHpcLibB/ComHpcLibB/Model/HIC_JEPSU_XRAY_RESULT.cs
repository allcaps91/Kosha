namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_XRAY_RESULT : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JEPDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public string GBREAD { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public string XRAYNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RESULT2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RESULT4 { get; set; }


        public HIC_JEPSU_XRAY_RESULT()
        {
        }
    }
}
