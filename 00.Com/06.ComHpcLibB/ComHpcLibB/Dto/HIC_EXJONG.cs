namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HIC_EXJONG : BaseDto
    {
        
        /// <summary>
        /// 코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 건강검진 명칭
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 분류(1.건강진단 2.암검진 3.측정 4.대행 5.기타검진)
        /// </summary>
		public string BUN { get; set; } 

        /// <summary>
        /// 건강진단 차수(1.1차 2.2차 3.기타검진)
        /// </summary>
		public string CHASU { get; set; } 

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
        /// 최종 변경 일시
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 최종 작업자 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 수가적용구분(아래참조)
        /// </summary>
		public string GBSUGA { get; set; } 

        /// <summary>
        /// 문진구분(아래참조)
        /// </summary>
		public string GBMUNJIN { get; set; } 

        /// <summary>
        /// 미수종류(아래참조)
        /// </summary>
		public string MISUJONG { get; set; } 

        /// <summary>
        /// 검진종류별 인원통계 기본적용 분류
        /// </summary>
		public string GBINWON { get; set; } 

        /// <summary>
        /// 내원검진 임상진찰 여부(Y/N)
        /// </summary>
		public string GBJIN { get; set; } 

        /// <summary>
        /// 1차검사결과지 인쇄 여부(Y/N)
        /// </summary>
		public string GBPRT1 { get; set; } 

        /// <summary>
        /// 2차검사결과지 인쇄 여부(Y/N)
        /// </summary>
		public string GBPRT2 { get; set; } 

        /// <summary>
        /// 특수검진 결과지 인쇄 여부(Y/N)
        /// </summary>
		public string GBPRT3 { get; set; } 

        /// <summary>
        /// 암검진 결과지 인쇄 여부(Y/N)
        /// </summary>
		public string GBPRT4 { get; set; } 

        /// <summary>
        /// 구강검진 결과지 인쇄 여부(Y/N)
        /// </summary>
		public string GBPRT5 { get; set; } 

        /// <summary>
        /// 학생검진 결과지 인쇄 여부(Y/N)
        /// </summary>
		public string GBPRT6 { get; set; } 

        /// <summary>
        /// abc  인식부서코드(bas_buse의  bucode)
        /// </summary>
		public string BUCODE { get; set; } 

        /// <summary>
        /// 상담여부(Y/N)
        /// </summary>
		public string GBSANGDAM { get; set; } 

        /// <summary>
        /// 자격조회여부(Y/N or Null)
        /// </summary>
		public string GBNHIC { get; set; } 

        /// <summary>
        /// 차수연계 아래참조
        /// </summary>
		public string CODE2 { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string ROWID { get; set; }
        
        /// <summary>
        /// 건강검진 종류 마스타
        /// </summary>
        public HIC_EXJONG()
        {

        }
    }
}
