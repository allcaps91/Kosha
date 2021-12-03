namespace ComHpcLibB.Model
{
    using ComBase.Mvc;

    public class HEA_JEPSU_SUNAPDTL : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
		public string BURATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GAMCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GAMCODE2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long AMT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBSELF { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBHALIN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long GAMAMT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long LTDSAMT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long BONINAMT { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public long LTDAMT { get; set; }
        
        public HEA_JEPSU_SUNAPDTL()
        {
        }
    }
}
