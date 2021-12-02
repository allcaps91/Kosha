namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_DAY : BaseDto
    {
        
        /// <summary>
        /// 날짜
        /// </summary>
		public DateTime? HOO_DATE { get; set; } 

        /// <summary>
        /// 참고사항
        /// </summary>
		public string REMARK { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HEA_DAY()
        {
        }
    }
}
