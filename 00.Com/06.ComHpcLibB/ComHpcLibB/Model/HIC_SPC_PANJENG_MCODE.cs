namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_SPC_PANJENG_MCODE : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long TONGBUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PANJENG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long CNT { get; set; }

        
        public HIC_SPC_PANJENG_MCODE()
        {
        }
    }
}
