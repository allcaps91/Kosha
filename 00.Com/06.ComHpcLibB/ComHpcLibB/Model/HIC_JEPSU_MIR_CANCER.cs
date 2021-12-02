namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;



    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_MIR_CANCER : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string MIRNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? JEPDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string WRTNO { get; set; }

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
		public string SNAME { get; set; }

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
		public string GKIHO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string KIHO { get; set; }

        
        public HIC_JEPSU_MIR_CANCER()
        {
        }
    }
}
