namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_HEA_EXJONG : BaseDto
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
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ACTMEMO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string INBODY { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string AMPM2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GONGDAN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBEXAM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long GWRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DELDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string JEPDATE { get; set; }

        /// <summary>
        /// 1: 일반검진, 2: 종합검진
        /// </summary>
		public string JEPGBN { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
		public string RID { get; set; }

        public HIC_JEPSU_HEA_EXJONG()
        {
        }
    }
}
