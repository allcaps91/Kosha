namespace ComHpcLibB.Model
{
    using ComBase.Mvc;

    public class HEA_JEPSU_MEMO : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MEMO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ENTTIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long JOBSABUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RID { get; set; }

                /// <summary>
        /// 
        /// </summary>
        public string SDATE { get; set; }
        

        public HEA_JEPSU_MEMO()
        {
        }
    }
}
