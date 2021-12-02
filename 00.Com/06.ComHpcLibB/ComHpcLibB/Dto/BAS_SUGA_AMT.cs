namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class BAS_SUGA_AMT : BaseDto
    {
        
        /// <summary>
        /// 수가코드
        /// </summary>
		public string SUCODE { get; set; } 

        /// <summary>
        /// 품목코드(수가코드)
        /// </summary>
		public string SUNEXT { get; set; } 

        /// <summary>
        /// 적용일자
        /// </summary>
		public DateTime? SUDATE { get; set; } 

        /// <summary>
        /// 일반가
        /// </summary>
		public long IAMT { get; set; } 

        /// <summary>
        /// 자보가
        /// </summary>
		public long TAMT { get; set; } 

        /// <summary>
        /// 보험가
        /// </summary>
		public long BAMT { get; set; } 

        /// <summary>
        /// 삭제일자
        /// </summary>
		public DateTime? DELDATE { get; set; } 

        /// <summary>
        /// 삭제한사번
        /// </summary>
		public long DELSABUN { get; set; } 

        /// <summary>
        /// 약제상한액차액(표준단가 - 구입가(현주수가))
        /// </summary>
		public long SAMT { get; set; } 

        /// <summary>
        /// 선택진료비
        /// </summary>
		public long SELAMT { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CSUCODE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CSUNEXT { get; set; } 

        
        /// <summary>
        /// 수가 금액 관리 테이블
        /// </summary>
        public BAS_SUGA_AMT()
        {
        }
    }
}
