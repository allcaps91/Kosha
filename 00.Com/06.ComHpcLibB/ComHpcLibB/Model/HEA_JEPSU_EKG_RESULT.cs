namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_JEPSU_EKG_RESULT : BaseDto
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
		public string SDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBEKG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EKGRESULT { get; set; }

        public string PTNO { get; set; }

        public HEA_JEPSU_EKG_RESULT()
        {
        }
    }
}
