namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_SCHOOL_NEW_SANGDAM_WAIT : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? JEPDATE { get; set; }

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
		public string SANGDAMDRNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string DPANDRNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? DPANDATE { get; set; }

        
        public HIC_JEPSU_SCHOOL_NEW_SANGDAM_WAIT()
        {
        }
    }
}
