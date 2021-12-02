namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_SUNAP : BaseDto
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
        /// 조합부담금
        /// </summary>
		public long JOHAPAMT { get; set; } 

        /// <summary>
        /// 회사부담금
        /// </summary>
		public long LTDAMT { get; set; } 

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
        /// 수납액
        /// </summary>
		public long SUNAPAMT { get; set; }

        /// <summary>
        /// 현금수납액
        /// </summary>
        public long SUNAPAMT1 { get; set; }

        /// <summary>
        /// 수납자 사번
        /// </summary>
        public long JOBSABUN { get; set; } 

        /// <summary>
        /// 수납시각
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 보건소 금액 -  사용안함
        /// </summary>
		public long BOGUNSOAMT { get; set; } 

        /// <summary>
        /// 신용카드 승인일련번호(현금은 NULL)
        /// </summary>
		public long CARDSEQNO { get; set; } 

        /// <summary>
        /// 건강보험1,2차 청구번호
        /// </summary>
		public long MIRNO1 { get; set; } 

        /// <summary>
        /// 구강검진 청구번호
        /// </summary>
		public long MIRNO2 { get; set; } 

        /// <summary>
        /// 암검진   청구번호(공단암)
        /// </summary>
		public long MIRNO3 { get; set; } 

        /// <summary>
        /// 회사청구 미수번호
        /// </summary>
		public long MISUNO1 { get; set; } 

        /// <summary>
        /// 암검진 보건소청구번호
        /// </summary>
		public long MIRNO4 { get; set; } 

        /// <summary>
        /// 카드금액
        /// </summary>
		public long SUNAPAMT2 { get; set; } 

        /// <summary>
        /// 공단청구 미수번호
        /// </summary>
		public long MISUNO2 { get; set; } 

        /// <summary>
        /// 암검진 의료급여청구번호
        /// </summary>
		public long MIRNO5 { get; set; } 

        /// <summary>
        /// 공단청구 구강미수번호
        /// </summary>
		public long MISUNO3 { get; set; } 

        /// <summary>
        /// 보건소부담금
        /// </summary>
		public long BOGENAMT { get; set; } 

        /// <summary>
        /// 보건소청구 미수번호
        /// </summary>
		public long MISUNO4 { get; set; }

        /// <summary>
        /// 접수순번
        /// </summary>
        public string JEPBUN { get; set; }

        /// <summary>
        /// 수검자명
        /// </summary>
        public string SNAME { get; set; }

        /// <summary>
        /// 진료과
        /// </summary>
        public string DEPTCODE { get; set; }

        /// <summary>
        /// 등록번호
        /// </summary>
        public string PTNO { get; set; }

        /// <summary>
        /// 진료일자
        /// </summary>
        public string JEPDATE { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 건강검진 수납 마스타
        /// </summary>
        public HIC_SUNAP()
        {
        }
    }
}
