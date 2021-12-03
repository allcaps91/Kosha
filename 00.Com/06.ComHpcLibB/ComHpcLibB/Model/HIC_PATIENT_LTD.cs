namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_PATIENT_LTD : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string NAME { get; set; }


        public HIC_PATIENT_LTD()
        {
        }
    }
}
