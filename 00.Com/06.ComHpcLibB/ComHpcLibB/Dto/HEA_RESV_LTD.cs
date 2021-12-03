namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_RESV_LTD : BaseDto
    {
        
        /// <summary>
        /// 수검일자
        /// </summary>
		public string SDATE { get; set; } 

        /// <summary>
        /// 회사코드
        /// </summary>
		public long LTDCODE { get; set; }

        /// <summary>
        /// 회사명
        /// </summary>
        public string LTDNAME { get; set; }

        /// <summary>
        /// 검사코드(HEA_CODE 13번 참조)
        /// </summary>
        public string GUBUN { get; set; } 

        /// <summary>
        /// 오전 예약인원
        /// </summary>
		public long AMINWON { get; set; } 

        /// <summary>
        /// 오후 예약인원
        /// </summary>
		public long PMINWON { get; set; } 

        /// <summary>
        /// 오전 접수인원
        /// </summary>
		public long AMJEPSU { get; set; } 

        /// <summary>
        /// 오후 접수인원
        /// </summary>
		public long PMJEPSU { get; set; } 

        /// <summary>
        /// 오전 미접수 인원
        /// </summary>
		public long AMJAN { get; set; } 

        /// <summary>
        /// 오후 미접수 인원
        /// </summary>
		public long PMJAN { get; set; } 

        /// <summary>
        /// 요일코드
        /// </summary>
		public string YOIL { get; set; }

        /// <summary>
        /// 작업자명
        /// </summary>
		public string JOBNAME { get; set; }
        
        /// <summary>
        /// 최종 등록 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 최종 등록 일시
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 종합건진 회사별 예약인원
        /// </summary>
        public HEA_RESV_LTD()
        {
        }

    }
}
