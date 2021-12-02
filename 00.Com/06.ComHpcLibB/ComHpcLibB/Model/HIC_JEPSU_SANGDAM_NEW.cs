namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_SANGDAM_NEW : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string UCODES { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJCHASU { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBSTS { get; set; }

        
        public HIC_JEPSU_SANGDAM_NEW()
        {
        }
    }
}
