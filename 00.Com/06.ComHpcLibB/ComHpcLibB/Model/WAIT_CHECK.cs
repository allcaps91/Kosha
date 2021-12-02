namespace ComHpcLibB.Model
{
    using ComBase.Mvc;


    /// <summary>
    /// 
    /// </summary>
    public class WAIT_CHECK : BaseDto
    {   
        /// <summary>
        /// 
        /// </summary>
		public string WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ACTPART { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ACTIVE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CNT { get; set; }


        public WAIT_CHECK()
        {
        }
    }
}
