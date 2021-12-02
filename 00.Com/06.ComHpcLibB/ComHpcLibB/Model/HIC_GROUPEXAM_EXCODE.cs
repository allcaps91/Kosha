namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_GROUPEXAM_EXCODE : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
		public long SEQNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EXCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string HNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SUGAGBN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? SUDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long GAMT1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long SAMT1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long JAMT1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long IAMT1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? SUDATE2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long GAMT2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long SAMT2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long JAMT2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long IAMT2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ENDOSCOPE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ENDOGUBUN1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ENDOGUBUN2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ENDOGUBUN3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ENDOGUBUN4 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ENDOGUBUN5 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RID { get; set; }

        public string GBSUGA { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long AMT1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long AMT2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long AMT3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long AMT4 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long AMT5 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long AMT6 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long OLDAMT1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long OLDAMT2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long OLDAMT3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long OLDAMT4 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long OLDAMT5 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long OLDAMT6 { get; set; }

        public string GROUPCODE { get; set; }

        public string GROUPNAME { get; set; }



        public HIC_GROUPEXAM_EXCODE()
        {
        }
    }
}
