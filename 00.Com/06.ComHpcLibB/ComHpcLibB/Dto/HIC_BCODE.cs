namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HIC_BCODE : BaseDto
    {        
        /// <summary>
        /// 코드구분
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 코드명칭
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 적용일자
        /// </summary>
		public DateTime? JDATE { get; set; } 

        /// <summary>
        /// 우선순위
        /// </summary>
		public long SORT { get; set; } 

        /// <summary>
        /// 삭제(적용취소)일자
        /// </summary>
		public DateTime? DELDATE { get; set; } 

        /// <summary>
        /// 최종 등록/변경 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 최종 등록/변경 일시
        /// </summary>
		public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
		public string RID { get; set; }       


        /// <summary>
        /// 건강검진 자료사전 Table
        /// </summary>
        public HIC_BCODE()
        {
        }
    }
}
