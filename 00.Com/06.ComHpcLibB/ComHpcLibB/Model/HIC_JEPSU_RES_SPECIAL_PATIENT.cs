
namespace ComHpcLibB.Model
{

    using ComBase.Mvc;


    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HIC_JEPSU_RES_SPECIAL_PATIENT : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long PANO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JEPDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JUMIN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string HNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBSPC { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string TEL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SEXAMS { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ROWID { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public long AGE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SEX { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GJJONG { get; set; }
        public string UCODES { get; set; }
        public string GJCHASU { get; set; }
        public string GBJUSO { get; set; }
        public string JUSO1 { get; set; }
        public string JUSO2 { get; set; }

        public string MAILCODE { get; set; }
        public string BUSENAME { get; set; }
        public string SABUN { get; set; }
        public string HPHONE { get; set; }
        public string JIKJONG { get; set; }
        public string GONGJENG { get; set; }
        public string IPSADATE { get; set; }
        public string JENIPDATE { get; set; }

        public string OLDGONG1 { get; set; }
        public long OLDMYEAR1 { get; set; }
        public long OLDDAYTIME1 { get; set; }
        public string OLDGONG2 { get; set; }
        public long OLDMYEAR2 { get; set; }
        public long OLDDAYTIME2 { get; set; }
        public string MUN_OLDMSYM { get; set; }
        public string MUN_GAJOK { get; set; }
        public string MUN_GIINSUNG { get; set; }

        public string JIN_NEURO { get; set; }
        public string JIN_HEAD { get; set; }
        public string JIN_SKIN { get; set; }
        public string JENGSANG { get; set; }

        public string DENTSOGEN { get; set; }
        public long DENTDOCT { get; set; }
        
    }
}

