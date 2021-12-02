namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_RESULT : BaseDto
    {        
        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long PANO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EXCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ACTIVE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JEPDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string XRAYNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RESCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RESULT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GCODE1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EXAMNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EXAMCHANGE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AMPM2 { get; set; }
                
        /// <summary>
        /// 
        /// </summary>
        public string ENTTIME { get; set; }
                
        /// <summary>
        /// 
        /// </summary>
        public long LTDCODE { get; set; }
                
        /// <summary>
        /// 
        /// </summary>
        public string GBDAILY { get; set; }
                
        /// <summary>
        /// 
        /// </summary>
        public string FAMILLY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GJYEAR { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string JONGGUMYN { get; set; }

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
        public string GBCHUL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UCODES { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NAME { get; set; }
        public long COUNT { get; set; }

        public HIC_JEPSU_RESULT()
        {
        }
    }
}
