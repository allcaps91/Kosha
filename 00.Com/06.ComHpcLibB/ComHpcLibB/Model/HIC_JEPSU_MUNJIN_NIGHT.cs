namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_MUNJIN_NIGHT : BaseDto
    {
        
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
		public string GJYEAR { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJCHASU { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ITEM1_DATA { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ITEM2_DATA { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ITEM3_DATA { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ITEM4_DATA { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ITEM5_DATA { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long ITEM1_JEMSU { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long ITEM2_JEMSU { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long ITEM3_JEMSU { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long ITEM4_JEMSU { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long ITEM5_JEMSU { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ITEM1_PANJENG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ITEM2_PANJENG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ITEM3_PANJENG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ITEM4_PANJENG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ITEM5_PANJENG { get; set; }


        public HIC_JEPSU_MUNJIN_NIGHT()
        {
        }
    }
}
