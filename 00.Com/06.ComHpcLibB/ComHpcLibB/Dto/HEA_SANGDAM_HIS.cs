namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_SANGDAM_HIS : BaseDto
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
		public string SEX { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long AGE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GBCALL { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public DateTime? CALLTIME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long WAITNO { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string ENTGUBUN { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HEA_SANGDAM_HIS()
        {
        }
    }
}
