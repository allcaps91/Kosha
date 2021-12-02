namespace HC.Core.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HC_VIEW_USER : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string UserId { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string Name { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string Jumin { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public DateTime? JoinDate { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string Dept { get; set; }

        public string CERTNO { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public HC_VIEW_USER()
        {
        }
    }
}
