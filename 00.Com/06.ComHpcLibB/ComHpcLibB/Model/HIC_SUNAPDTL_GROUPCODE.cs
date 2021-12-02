namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 2020-03-26 HIC_SUNAPDTL, HEA_SUNAPDTL 같이 사용함 KMC
    /// </summary>
    public class HIC_SUNAPDTL_GROUPCODE : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string YNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBSANGDAM { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBAM { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBSELF { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EXAMNAME { get; set; }
        

        public HIC_SUNAPDTL_GROUPCODE()
        {
        }
    }
}
