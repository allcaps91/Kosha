namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_SCHOOL_NEW : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

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
		public string UCODES { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SANGDAMDRNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string DPANDRNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string DPANDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JUMIN2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long LTDCODE { get; set; }

        public int CLASS { get; set; }
        public int BAN { get; set; }
        public int BUN { get; set; }
        public string SEX { get; set; }
        public long AGE { get; set; }
        public string PPANA1 { get; set; }
        public string PPANA2 { get; set; }
        public string PPANA3 { get; set; }
        public string PPANA4 { get; set; }



        public HIC_JEPSU_SCHOOL_NEW()
        {
        }
    }
}
