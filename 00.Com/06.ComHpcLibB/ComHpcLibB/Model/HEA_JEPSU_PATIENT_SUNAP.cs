namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_JEPSU_PATIENT_SUNAP : BaseDto
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
		public long AGE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SEX { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string LTDNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string HPHONE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string VIPREMARK { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long TOTAMT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// 
		public long BONINAMT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SOSOK { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string JUMIN2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string JOBNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PTNO { get; set; }

        public HEA_JEPSU_PATIENT_SUNAP()
        {
        }
    }
}
