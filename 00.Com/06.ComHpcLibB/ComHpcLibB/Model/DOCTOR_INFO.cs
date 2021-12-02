namespace ComHpcLibB.Model
{
    using ComBase.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class DOCTOR_INFO : BaseDto
    {   
        /// <summary>
        /// 
        /// </summary>
		public string DRNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string LICENCE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string IPSADAY { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string REDAY { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string TOIDAY { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ROOM { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PAN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBDENT { get; set; }

        
        public DOCTOR_INFO()
        {
        }
    }
}
