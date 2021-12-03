namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_CANCER_RESV1 : BaseDto
    {
        
        /// <summary>
        /// 검사일자
        /// </summary>
		public string JOBDATE { get; set; } 

        /// <summary>
        /// 요일(0.일요일 또는 휴일,1.월,2.화,.. 6.토요일)
        /// </summary>
		public string WEEK { get; set; } 

        /// <summary>
        /// 오전 위장조영   가능 건수
        /// </summary>
		public long UGI { get; set; } 

        /// <summary>
        /// 오전 내시경검사 가능 건수
        /// </summary>
		public long GFS { get; set; } 

        /// <summary>
        /// 오전 유방검사   가능 건수
        /// </summary>
		public long MAMMO { get; set; } 

        /// <summary>
        /// 오전 결직장검사 가능 건수
        /// </summary>
		public long RECTUM { get; set; } 

        /// <summary>
        /// 오전 초음파검사 가능 건수
        /// </summary>
		public long SONO { get; set; } 

        /// <summary>
        /// 오전 참고사항
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 최종 등록/변경자 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 최종 등록/변경   시각
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 오후 위장조영   가능 건수
        /// </summary>
		public long UGI1 { get; set; } 

        /// <summary>
        /// 오후 내시경검사 가능 건수
        /// </summary>
		public long GFS1 { get; set; } 

        /// <summary>
        /// 오후 유방검사   가능 건수
        /// </summary>
		public long MAMMO1 { get; set; } 

        /// <summary>
        /// 오후 결직장검사 가능 건수
        /// </summary>
		public long RECTUM1 { get; set; } 

        /// <summary>
        /// 오후 초음파검사 가능 건수
        /// </summary>
		public long SONO1 { get; set; } 

        /// <summary>
        /// 오후 참고사항
        /// </summary>
		public string REMARK1 { get; set; } 

        /// <summary>
        /// 오전 자궁경부암 가능 건수
        /// </summary>
		public long WOMB { get; set; } 

        /// <summary>
        /// 오후 자궁경부암 가능 건수
        /// </summary>
		public long WOMB1 { get; set; } 

        /// <summary>
        /// 오전 검진1차 가능건수
        /// </summary>
		public long BOHUM { get; set; } 

        /// <summary>
        /// 오후 검진1차 가능건수
        /// </summary>
		public long BOHUM1 { get; set; } 

        /// <summary>
        /// 오전 내시경검사(종검) 가능 건수
        /// </summary>
		public long GFSH { get; set; } 

        /// <summary>
        /// 오후 내시경검사(종검) 가능 건수
        /// </summary>
		public long GFSH1 { get; set; } 

        /// <summary>
        /// 오전 CT 가능건수
        /// </summary>
		public long CT { get; set; } 

        /// <summary>
        /// 오후 CT 가능건수
        /// </summary>
		public long CT1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long LUNG_SANGDAM { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long LUNG_SANGDAM1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ROWID { get; set; }

        /// <summary>
        /// 암검진 일자별 예약가능 검사건수 설정
        /// </summary>
        public HIC_CANCER_RESV1()
        {
        }
    }
}
