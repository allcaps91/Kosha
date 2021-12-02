namespace ComHpcLibB
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HC_PANJENG_PATLIST : BaseDto
    {
        public string SELECTTAB { get; set; }

        /// <summary>
        /// N : 미판정 / Y : 판정
        /// </summary>
        public string JOB { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

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
		public string LTDCODE { get; set; }

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
        public string JEPDATE { get; set; }

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
        public string BURATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJCHASU { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DOCTOR { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long WRTNO { get; set; }

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
		public string RID { get; set; }        

        /// <summary>
        /// 
        /// </summary>
		public string PTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBCHUL { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBSTSNM { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBSTS { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long PANJENGDRNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PANDRNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SANGDAMDRNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long PANO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long IEMUNNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GAPANJENGNAME { get; set; }

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
        
        /// <summary>
        /// 
        /// </summary>
        public string ERFLAG { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UCODES { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long MUNJINDRNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string JONGGUMYN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PANJENG { get; set; }

        /// <summary>
        /// 판독여부
        /// </summary>
        public string XREAD { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SANGDAMDRNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBAUTOPAN { get; set; }

        /// <summary>
        /// 회사추가검진 판정여부
        /// </summary>
        public string GBADDPAN { get; set; }

        public HC_PANJENG_PATLIST()
        {
        }
    }
}
