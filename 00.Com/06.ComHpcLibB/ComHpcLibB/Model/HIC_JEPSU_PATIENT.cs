namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_PATIENT : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long WRTNO1 { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public long WRTNO2 { get; set; }

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
		public string TEL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HPHONE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GJBANGI { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long AGE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string IPSADATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JEPDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JEPDATE_STR { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GONGJENG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string BUSENAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string UCODES { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBSTS { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GJNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBMUNJIN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBMUNJIN1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBMUNJIN2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBSPCMUNJIN { get; set; }        

        /// <summary>
        /// 
        /// </summary>
		public string JUMIN2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JUMIN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SECOND_EXAMS { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SECOND_EXAMS_NNM { get; set; }
        
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
        public string SECOND_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SECOND_TONGBO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TONGBODATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BOGUNSO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string KIHO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long PANO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long PANO1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long PANO2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GJYEAR { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GJCHASU { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBN { get; set; }

        /// <summary>
        /// 암검진 구분
        /// </summary>
        public string GBAM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long CLASS { get; set; }

        public string GBCHUL { get; set; }

        public string GBCHUL2 { get; set; }

        public long BAN { get; set; }

        public long BUN { get; set; }

        public string GBDENTAL { get; set; }

        public string JONGGUMYN { get; set; }

        public string GBADDPAN { get; set; }

        public string SANGDAMDATE { get; set; }

        public long SANGDAMDRNO { get; set; }

        public long PANJENGDRNO { get; set; }

        public DateTime? PANJENGDATE { get; set; }

        public long PRTSABUN { get; set; }

        public string WEBPRINTSEND { get; set; }

        public DateTime? WEBPRINTREQ { get; set; }

        public long IEMUNNO { get; set; }

        public string JOBSABUN { get; set; }

        public string JISA { get; set; }

        public string GBINWON { get; set; }

        public string GKIHO { get; set; }        

        public string SEXAMS { get; set; }

        public string MAILCODE { get; set; }
        
        public string JUSO { get; set; }

        public string JUSO1 { get; set; }

        public string JUSO2 { get; set; }

        public string BURATE { get; set; }

        public string JIKGBN { get; set; }

        public string YOUNGUPSO { get; set; }

        public string MILEAGEAM { get; set; }

        public string MURYOAM { get; set; }

        public string GUMDAESANG { get; set; }

        public string MILEAGEAMGBN { get; set; }

        public string MURYOGBN { get; set; }

        public string REMARK { get; set; }

        public string EMAIL { get; set; }

        public string PHONE { get; set; }

        public string GUBUN { get; set; }

        public string RTIME { get; set; }

        public string RTIME2 { get; set; }

        public string GBCHK1 { get; set; }
        public string GBCHK2 { get; set; }
        public string GBCHK3 { get; set; }
        public string GBJUSO { get; set; }
        public string SLIP_SMOKE { get; set; }

        public string SLIP_DRINK { get; set; }

        public string SLIP_ACTIVE { get; set; }

        public string SLIP_FOOD { get; set; }

        public string SLIP_BIMAN { get; set; }

        public string T_SMOKE1 { get; set; }

        public string TMUN0003 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AUTOJEP { get; set; }

        public string JUSOAA { get; set; }

        public string MUN0105 { get; set; }

        public string TMUN0106 { get; set; }

        public string TMUN0107 { get; set; }

        public string TMUN0108 { get; set; }

        public string TMUN0109 { get; set; }

        public string TMUN0110 { get; set; }

        public string TMUN0111 { get; set; }

        public string TMUN0112 { get; set; }

        public string TMUN0113 { get; set; }

        public string TMUN0114 { get; set; }

        public string TMUN0115 { get; set; }

        public string TMUN0116 { get; set; }

        public string TMUN0117 { get; set; }

        public string TMUN0118 { get; set; }

        public string TMUN0119 { get; set; }

        public string TMUN0120 { get; set; }

        public string TMUN0121 { get; set; }

        public string TMUN0122 { get; set; }

        public string TMUN0123 { get; set; }

        public string TMUN0124 { get; set; }

        public string RES_MUNJIN { get; set; }

        public string RES_JOCHI { get; set; }

        public string RES_RESULT { get; set; }

        public string T_PANJENG_SOGEN { get; set; }

        public string SANGDAM { get; set; }

        public long MIRNO1 { get; set; }

        public long MIRNO2 { get; set; }

        public long MIRNO3 { get; set; }

        public long MIRNO4 { get; set; }

        public long MIRNO5 { get; set; }

        public string SDATE { get; set; }

        public string SABUN { get; set; }

        public HIC_JEPSU_PATIENT()
        {
        }
    }
}
