namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_RESULT_EXCODE_JEPSU : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PART { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EXCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string HNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RESULT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RESCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PANJENG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MIN_M { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MAX_M { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MIN_F { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MAX_F { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RESULTTYPE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBCODEUSE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string UNIT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PTNO { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string JEPDATE { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string ENDOGUBUN1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ENDOGUBUN2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ENDOGUBUN3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ENDOGUBUN4 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ENDOGUBUN5 { get; set; }
        public string SEX { get; set; }
        public string GJYEAR { get; set; }
        public string EXAMBUN { get; set; }



        public HIC_RESULT_EXCODE_JEPSU()
        {
        }
    }
}
