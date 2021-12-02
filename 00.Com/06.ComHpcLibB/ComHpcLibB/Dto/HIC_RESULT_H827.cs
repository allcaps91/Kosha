namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_RESULT_H827 : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 등록번호
        /// </summary>
		public long PANO { get; set; } 

        /// <summary>
        /// 접수일자
        /// </summary>
		public string JEPDATE { get; set; } 

        /// <summary>
        /// 채혈일자
        /// </summary>
		public string BLOODDATE { get; set; } 

        /// <summary>
        /// 재판정여부(Y or NULL)
        /// </summary>
		public string REPANJENG { get; set; } 

        /// <summary>
        /// 참고사항
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 채혈등록자 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 등록일시
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 검사코드
        /// </summary>
		public string EXCODE { get; set; } 

        
        /// <summary>
        /// H827(혈중카르복시헤모글로빈) 채혈 및 판정 관리
        /// </summary>
        public HIC_RESULT_H827()
        {
        }
    }
}
