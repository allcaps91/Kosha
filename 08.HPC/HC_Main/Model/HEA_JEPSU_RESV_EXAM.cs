namespace HC_Main.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_JEPSU_RESV_EXAM : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public DateTime? RTIME { get; set; }

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
		public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBEXAM { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EXAMNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string TONGBODATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string TONGBOSABUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string TONGBONAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string CONFIRM { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? SDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long ENTSABUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EXCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

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
		public string A_SEX { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string HPHONE { get; set; }
        

        /// <summary>
        /// 
        /// </summary>
		public string RID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string AMPM { get; set; }
        
        public HEA_JEPSU_RESV_EXAM()
        {
        }
    }
}
