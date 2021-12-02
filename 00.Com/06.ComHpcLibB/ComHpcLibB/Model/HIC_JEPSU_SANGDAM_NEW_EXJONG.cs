namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_SANGDAM_NEW_EXJONG : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long PANO { get; set; }

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
		public string GJYEAR { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string UCODES { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long SANGDAMDRNO { get; set; }

        
        public HIC_JEPSU_SANGDAM_NEW_EXJONG()
        {
        }
    }
}
