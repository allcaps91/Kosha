namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class ENDO_JUPMST_ORDERCODE : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string PTNO { get; set; }

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
		public string S_AGE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? BIRTHDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string DISPHEADER { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ORDERNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string DEPTCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string DRCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string DRNAME { get; set; }

        public ENDO_JUPMST_ORDERCODE()
        {
        }
    }
}
