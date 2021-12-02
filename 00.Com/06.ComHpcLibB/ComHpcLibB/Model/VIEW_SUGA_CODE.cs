namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class VIEW_SUGA_CODE : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string BUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string NU { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SUCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SUNEXT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SUNAMEK { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SUNAMEG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string HCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? SUDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string BCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string BAMT { get; set; }

        
        public VIEW_SUGA_CODE()
        {
        }
    }
}
