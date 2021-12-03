namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_RES_DENTAL : BaseDto
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
		public string GBJINCHAL2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJYEAR { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long PANJENGDRNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PANJENGDATE { get; set; }

        public string SEX { get; set; }

        public long AGE { get; set; }

        public string GBCHUL { get; set; }        

        /// <summary>
        /// 
        /// </summary>
		public string PTNO { get; set; }

        public HIC_JEPSU_RES_DENTAL()
        {
        }
    }
}
