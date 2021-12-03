namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_AMT_CANCER : BaseDto
    {
        
        /// <summary>
        /// 적용 시작일자
        /// </summary>
		public string SDATE { get; set; } 

        /// <summary>
        /// 암검진 상담료
        /// </summary>
		public string AMT01 { get; set; } 

        /// <summary>
        /// 암검진 상담료(공휴가산)
        /// </summary>
		public string AMT02 { get; set; } 

        /// <summary>
        /// 위장조영검사
        /// </summary>
		public string AMT03 { get; set; } 

        /// <summary>
        /// 위내시경검사
        /// </summary>
		public string AMT04 { get; set; } 

        /// <summary>
        /// 위조직검사(1-3개)
        /// </summary>
		public string AMT05 { get; set; } 

        /// <summary>
        /// 위조직검사(4-6개)
        /// </summary>
		public string AMT06 { get; set; } 

        /// <summary>
        /// 위조직검사(7-9개)
        /// </summary>
		public string AMT07 { get; set; } 

        /// <summary>
        /// 위조직검사(10-12개)
        /// </summary>
		public string AMT08 { get; set; } 

        /// <summary>
        /// 위조직검사(13개이상)
        /// </summary>
		public string AMT09 { get; set; } 

        /// <summary>
        /// 간암:ALT
        /// </summary>
		public string AMT10 { get; set; } 

        /// <summary>
        /// 간암:B형간염항원정밀
        /// </summary>
		public string AMT11 { get; set; } 

        /// <summary>
        /// 간암:C형간염항체정밀
        /// </summary>
		public string AMT12 { get; set; } 

        /// <summary>
        /// 간암:간초음파검사
        /// </summary>
		public string AMT13 { get; set; } 

        /// <summary>
        /// 간암:혈청알파태아단백
        /// </summary>
		public string AMT14 { get; set; } 

        /// <summary>
        /// 대장암:잠혈반응(RPHA)
        /// </summary>
		public string AMT15 { get; set; } 

        /// <summary>
        /// 대장암:잠혈반응(분변혈색소)
        /// </summary>
		public string AMT16 { get; set; } 

        /// <summary>
        /// 대장암:대장이중조영검사
        /// </summary>
		public string AMT17 { get; set; } 

        /// <summary>
        /// 대장암:대장내시경검사
        /// </summary>
		public string AMT18 { get; set; } 

        /// <summary>
        /// 대장조직검사(1-3개)
        /// </summary>
		public string AMT19 { get; set; } 

        /// <summary>
        /// 대장조직검사(4-6개)
        /// </summary>
		public string AMT20 { get; set; } 

        /// <summary>
        /// 대장조직검사(7-9개)
        /// </summary>
		public string AMT21 { get; set; } 

        /// <summary>
        /// 대장조직검사(10-12개)
        /// </summary>
		public string AMT22 { get; set; } 

        /// <summary>
        /// 대장조직검사(13개이상)
        /// </summary>
		public string AMT23 { get; set; } 

        /// <summary>
        /// 유방암:유방촬영
        /// </summary>
		public string AMT24 { get; set; } 

        /// <summary>
        /// 자궁경부세포검사
        /// </summary>
		public string AMT25 { get; set; } 

        /// <summary>
        /// 조직검사용 포셉
        /// </summary>
		public string AMT26 { get; set; } 

        /// <summary>
        /// 유방암:편측촬영(한쪽)
        /// </summary>
		public string AMT27 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string AMT28 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string AMT29 { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 공단청구 기준금액(암검진)
        /// </summary>
        public HIC_AMT_CANCER()
        {
        }
    }
}
