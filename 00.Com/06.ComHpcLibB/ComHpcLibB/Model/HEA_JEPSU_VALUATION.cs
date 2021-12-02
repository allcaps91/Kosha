namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_JEPSU_VALUATION : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JEPDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SEX { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long AGE { get; set; }

        
        public HEA_JEPSU_VALUATION()
        {
        }
    }
}
