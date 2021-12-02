namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_CHARTTRANS_PRINT : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string JEPDATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string BIRTH { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string LTDNAME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public DateTime? RECVTIME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long RECVSABUN { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public DateTime? JOBTIME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long JOBSABUN { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HIC_CHARTTRANS_PRINT()
        {
        }
    }
}
