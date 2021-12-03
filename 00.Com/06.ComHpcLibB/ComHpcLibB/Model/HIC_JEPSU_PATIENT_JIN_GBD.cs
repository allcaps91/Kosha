namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_PATIENT_JIN_GBD : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ENTSABUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JEPDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long AGE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SEX { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public string GUBUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long PANJENGDRNO { get; set; }

        public string PRTSABUN { get; set; }
        public string TONGBODATE { get; set; }



        public HIC_JEPSU_PATIENT_JIN_GBD()
        {
        }
    }
}
