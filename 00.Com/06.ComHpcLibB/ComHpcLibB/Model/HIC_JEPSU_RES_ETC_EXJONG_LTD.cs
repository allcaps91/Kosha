namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_RES_ETC_EXJONG_LTD : BaseDto
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
		public DateTime? JEPDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string UCODES { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SEXAMS { get; set; }

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
		public long PANJENGDRNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EXNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string LTDNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBPANJENG { get; set; }

        
        public HIC_JEPSU_RES_ETC_EXJONG_LTD()
        {
        }
    }
}
