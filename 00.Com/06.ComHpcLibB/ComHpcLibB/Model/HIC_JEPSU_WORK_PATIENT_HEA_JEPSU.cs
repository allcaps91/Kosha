namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_WORK_PATIENT_HEA_JEPSU : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JUMINNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JUMINNO2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string LTDNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PANO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? JEPDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJYEAR { get; set; }

        
        public HIC_JEPSU_WORK_PATIENT_HEA_JEPSU()
        {
        }
    }
}
