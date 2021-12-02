namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_RESV_EXAM_PATIENT : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public DateTime? SDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long PANO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SEX { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JUSO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JUMIN2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string AMPM2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long AMCNT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long PMCNT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBEXAM { get; set; }

        public HEA_RESV_EXAM_PATIENT()
        {
        }
    }
}
