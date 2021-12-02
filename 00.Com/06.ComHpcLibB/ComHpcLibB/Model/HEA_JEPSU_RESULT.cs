namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_JEPSU_RESULT : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string JEPDATE { get; set; }

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
		public long PANO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EXCODE { get; set; }

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
		public string RID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string AMPM2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string XRAYNO { get; set; }

        public string ENTTIME { get; set; }

        public long LTDCODE { get; set; }

        public string GBDAILY { get; set; }

        public string EXAMCHANGE { get; set; }
        



        public HEA_JEPSU_RESULT()
        {
        }
    }
}
