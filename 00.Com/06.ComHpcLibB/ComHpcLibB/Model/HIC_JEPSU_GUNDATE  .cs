namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_GUNDATE   : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MINDATE1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MAXDATE1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MINDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MAXDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GUNDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JEPDATE { get; set; }

        public long WRTNO { get; set; }

        public string SNAME { get; set; }

        public long AGE { get; set; }

        public string GJJONG { get; set; }

        public string MIRSAYU { get; set; }

        public string BOGUNSO { get; set; }
        

        public HIC_JEPSU_GUNDATE  ()
        {
        }
    }
}
