namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class HIC_BARCODE_REQ : BaseDto
    {   
        /// <summary>
        /// 
        /// </summary>
		public string GBBUSE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HIC_BARCODE_REQ()
        {
        }
    }
}
