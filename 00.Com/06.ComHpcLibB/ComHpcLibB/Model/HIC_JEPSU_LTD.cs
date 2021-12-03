namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_LTD : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? JEPDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SANGHO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long CNT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MINDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MAXDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }        

        /// <summary>
        /// 
        /// </summary>
        public string SEX { get; set; }




        public HIC_JEPSU_LTD()
        {
        }
    }
}
