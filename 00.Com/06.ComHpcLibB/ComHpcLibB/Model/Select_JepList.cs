namespace ComHpcLibB.Model
{
    using ComBase.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class Select_JepList : BaseDto
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
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string AMPM2 { get; set; }

        public string SEX { get; set; }

        public string PTNO { get; set; }       


        public Select_JepList()
        {
        }
    }
}
