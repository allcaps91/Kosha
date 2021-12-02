namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_RES_SPECIAL : BaseDto
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
		public string NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JEPDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJCHASU { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string UCODES { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJYEAR { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJBANGI { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SEX { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long AGE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SOGEN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PANJENG { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long CNT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public DateTime? MINDATE { get; set; }
        public string MINDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public DateTime? MAXDATE { get; set; }
        public string MAXDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long PANJENGDRNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBOHMS { get; set; }

        public string GBSPC { get; set; }
        public HIC_JEPSU_RES_SPECIAL()
        {
        }
    }
}
