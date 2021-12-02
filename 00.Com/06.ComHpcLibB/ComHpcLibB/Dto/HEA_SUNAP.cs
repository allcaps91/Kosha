namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_SUNAP : BaseDto
    {
        
        /// <summary>
        /// 접수 일련번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 수납일자
        /// </summary>
		public DateTime? SUDATE { get; set; } 

        /// <summary>
        /// 접수 일련번호별 영수증 차수(1,2,3,...)
        /// </summary>
		public long SEQNO { get; set; } 

        /// <summary>
        /// 검진번호
        /// </summary>
		public long PANO { get; set; } 

        /// <summary>
        /// 총진료비
        /// </summary>
		public long TOTAMT { get; set; } 

        /// <summary>
        /// 할인계정
        /// </summary>
		public string HALINGYE { get; set; } 

        /// <summary>
        /// 할인금액
        /// </summary>
		public long HALINAMT { get; set; } 

        /// <summary>
        /// 회사부담금
        /// </summary>
		public long LTDAMT { get; set; }

        /// <summary>
        /// 일부부담금
        /// </summary>
        public long LTDSAMT { get; set; }

        /// <summary>
        /// 개인부담금
        /// </summary>
        public long BONINAMT { get; set; } 

        /// <summary>
        /// 미수계정
        /// </summary>
		public string MISUGYE { get; set; } 

        /// <summary>
        /// 개인미수금
        /// </summary>
		public long MISUAMT { get; set; } 

        /// <summary>
        /// 현금수납액
        /// </summary>
		public long SUNAPAMT1 { get; set; } 

        /// <summary>
        /// 카드수납액
        /// </summary>
		public long SUNAPAMT2 { get; set; } 

        /// <summary>
        /// 카드번호
        /// </summary>
		public string CARDNO { get; set; } 

        /// <summary>
        /// 검사항목(묶음코드)
        /// </summary>
		public string EXAMCODES { get; set; } 

        /// <summary>
        /// 수납자 사번
        /// </summary>
		public long JOBSABUN { get; set; } 

        /// <summary>
        /// 수납시각
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 신용카드 승인일련번호(현금은 NULL)
        /// </summary>
		public long CARDSEQNO { get; set; } 

        /// <summary>
        /// 수납1
        /// </summary>
		public long YSUNAPAMT1 { get; set; } 

        /// <summary>
        /// 수납2
        /// </summary>
		public long YSUNAPAMT2 { get; set; } 

        /// <summary>
        /// 회사미수 자동형성 번호
        /// </summary>
		public long MISUNO { get; set; } 

        /// <summary>
        /// 할인계정2
        /// </summary>
		public string HALINGYE2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long PANO2 { get; set; }

        /// <summary>
        /// 현금금액
        /// </summary>
        public long CASHAMT { get; set; }

        /// <summary>
        /// 카드금액
        /// </summary>
		public long CARDAMT { get; set; }

        /// <summary>
        /// 카드금액
        /// </summary>
		public long CHAAMT { get; set; }

        public string RID { get; set; }
        /// <summary>
        /// 종합검진 수납내역
        /// </summary>
        public HEA_SUNAP()
        {
        }
    }
}
