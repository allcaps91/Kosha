namespace ComHpcLibB.Model
{
    using ComBase.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class PATIENT_INFO : BaseDto
    {   
        /// <summary>
        /// 
        /// </summary>
		public string PANO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public int AGE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SEX { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JEPDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JUMIN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JUMIN2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ACTMEMO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EXAMREMARK { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GONGDAN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBEXAM { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBNAKSANG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string XRAYNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SUNAP { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJYEAR { get; set; }

        /// <summary>
        /// 당일접수정보(종류전체)
        /// </summary>
		public string JEPSUJONG { get; set; }
        

        public PATIENT_INFO()
        {
        }
    }
}
