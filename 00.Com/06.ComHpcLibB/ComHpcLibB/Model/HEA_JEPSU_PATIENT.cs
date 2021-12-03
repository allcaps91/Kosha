namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;

    public class HEA_JEPSU_PATIENT : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
		public long PANO { get; set; }

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
		public string JUMIN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JUMIN2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string BUSENAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PANREMARK { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string YDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string LTDNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MAILDATE { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
		public string MAILWEIGHT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PRTDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string FAMILLY { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RECVDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string BIRTHDAY { get; set; }        

        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ARRIVE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBSANGDAM { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? IDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? JEPDATE { get; set; }

        public string HPHONE { get; set; }

        public string STIME { get; set; }

        public string AMPM2 { get; set; }

        public string JUSO1 { get; set; }

        public string JUSO2 { get; set; }

        public string TEL { get; set; }

        public string GBSTS { get; set; }

        public string GUBUN { get; set; }

        public string MAILCODE { get; set; }
        
        public string ADDRESS { get; set; }
        
        public string EMAIL { get; set; }
        


        public HEA_JEPSU_PATIENT()
        {
        }
    }
}
