namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_EXJONG_PATIENT : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string JEPDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SEX { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long AGE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBSTS { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string UCODES { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string TEL { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JUMIN2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SECOND_EXAMS { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SECOND_SAYU { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SECOND_MISAYU { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JONGGUMYN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SECOND_TONGBO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SECOND_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MAILCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JUSO1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JUSO2 { get; set; }

        public string GBMUNJIN { get; set; }
        public string GBMUNJIN1 { get; set; }
        public string GBMUNJIN2 { get; set; }
        public string GBMUNJIN3 { get; set; }

        public string GBDENTAL { get; set; }

        public long PANO { get; set; }

        public string BUSENAME { get; set; }

        public string JISA { get; set; }

        public string PTNO { get; set; }
        public string KIHO { get; set; }
        public string GBINWON { get; set; }
        public string GKIHO { get; set; }
        public string BURATE { get; set; }
        public string JIKGBN { get; set; }
        public DateTime? IPSADATE { get; set; }
        public string BOGUNSO { get; set; }
        public string YOUNGUPSO { get; set; }
        public string MILEAGEAM { get; set; }
        public string MURYOAM { get; set; }
        public string GUMDAESANG { get; set; }
        public string MILEAGEAMGBN { get; set; }
        public string MURYOGBN { get; set; }
        public string REMARK { get; set; }
        public string EMAIL { get; set; }
        public string HPHONE { get; set; }


        public HIC_JEPSU_EXJONG_PATIENT()
        {
        }
    }
}
