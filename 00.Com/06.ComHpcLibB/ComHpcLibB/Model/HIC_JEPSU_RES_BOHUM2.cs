namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_RES_BOHUM2 : BaseDto
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
		public string BUSENAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJCHASU { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long PANO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PANJENG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string CHEST_RES { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string CYCLE_RES { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GOJI_RES { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string LIVER_RES { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string KIDNEY_RES { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string AMEMIA_RES { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string DIABETES_RES { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PANJENGDATE { get; set; }

        public string SEX { get; set; }

        public long AGE { get; set; }

        public long PANJENGDRNO { get; set; }

        public string TONGBODATE { get; set; }


        public HIC_JEPSU_RES_BOHUM2()
        {
        }
    }
}
