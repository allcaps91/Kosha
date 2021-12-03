namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_SUNAP_WORK : BaseDto
    {
        
        /// <summary>
        /// 접수 일련번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 수납일자
        /// </summary>
		public string SUDATE { get; set; } 

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
        /// 수납자 사번
        /// </summary>
		public long JOBSABUN { get; set; } 

        /// <summary>
        /// 수납시각
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 보건소부담금액
        /// </summary>
		public long BOGUNSOAMT { get; set; } 

        /// <summary>
        /// 카드번호
        /// </summary>
		public long CARDSEQNO { get; set; } 

        /// <summary>
        /// 검진종류
        /// </summary>
		public string GJJONG { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HIC_SUNAP_WORK()
        {
        }
    }
}
