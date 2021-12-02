namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_EXCODE_REFER : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string FROMDATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TODATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string EXCODE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string MIN_M { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string MIN_MB { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string MIN_MR { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string MAX_M { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string MAX_MB { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string MAX_MR { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string MIN_F { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string MIN_FB { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string MIN_FR { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string MAX_F { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string MAX_FB { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string MAX_FR { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string EXAMFRTO { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string RESULTTYPE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string UNIT { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GUBUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GUBUNNM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HIC_EXCODE_REFER()
        {
        }
    }
}
