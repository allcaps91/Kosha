namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_RES_SPECIAL_LTD : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NAME { get; set; }

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

        public string JEPDATE { get; set; }

        public long PANO { get; set; }

        public string SEX { get; set; }

        public string SEXAMS { get; set; }        

        public string UCODES { get; set; }

        public string GJJONG { get; set; }

        public string GJBANGI { get; set; }

        public string SECOND_FLAG { get; set; }

        public string SNAME { get; set; }

        public long AGE { get; set; }

        public string HEMSMIRSAYU { get; set; }

        public string SUCHUPYN { get; set; }

        public string GBGUKGO { get; set; }

        public long PAN_R { get; set; }

        public string GJYEAR { get; set; }

        public string ROWID { get; set; }        

        public HIC_JEPSU_RES_SPECIAL_LTD()
        {
        }
    }
}
