namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_EXJONG : BaseDto
    {   
        /// <summary>
        /// 코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 종합검진 명칭
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 종합검진 약어
        /// </summary>
		public string YNAME { get; set; } 

        /// <summary>
        /// 분류(아래참조)
        /// </summary>
		public string BUN { get; set; } 

        /// <summary>
        /// 부담율코드(아래참조)
        /// </summary>
		public string BURATE { get; set; } 

        /// <summary>
        /// 접수시 부담율 변경가능 여부(Y/N)
        /// </summary>
		public string BUCHANGE { get; set; } 

        /// <summary>
        /// 참고사항
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 검진종류별 인원통계 기본적용 분류
        /// </summary>
		public string GBINWON { get; set; } 

        /// <summary>
        /// 최종 변경 일시
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 최종 작업자 사번
        /// </summary>
		public long ENTSABUN { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 종합검진 종류 코드
        /// </summary>
        public HEA_EXJONG()
        {
        }
    }
}
