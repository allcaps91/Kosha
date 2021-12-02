namespace ComHpcLibB
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HC_PANJENG_PATLIST_SCHOOL : BaseDto
    {
        public string SELECTTAB { get; set; }

        /// <summary>
        /// N : 미판정 / Y : 판정
        /// </summary>
        public string JOB { get; set; }        

        /// <summary>
        /// 
        /// </summary>
		public string LTDNAME { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
		public string JEPDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PANO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

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
		public long WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JUMIN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JUMIN2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public decimal CLASS { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public decimal BAN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public decimal BUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBPAN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long SANGDAMDRNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long GWRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string FRDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TODATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MUN { get; set; }

        public HC_PANJENG_PATLIST_SCHOOL()
        {
        }
    }
}
