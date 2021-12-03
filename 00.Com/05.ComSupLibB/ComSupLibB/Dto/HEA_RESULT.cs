namespace ComSupLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_RESULT : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 검사코드
        /// </summary>
		public string EXCODE { get; set; } 

        /// <summary>
        /// 검사부서(1.신체계측 2.임상병리과 3.방사선과 4.기타검사)
        /// </summary>
		public string PART { get; set; } 

        /// <summary>
        /// 검사결과분류코드(기타는 NULL)
        /// </summary>
		public string RESCODE { get; set; } 

        /// <summary>
        /// 검사결과
        /// </summary>
		public string PANJENG { get; set; } 

        /// <summary>
        /// 검사결과판정(1.정상 2.비정상)
        /// </summary>
		public string RESULT { get; set; } 

        /// <summary>
        /// ABC시행부서
        /// </summary>
		public string OPER_DEPT { get; set; } 

        /// <summary>
        /// ABC시행의사(사번)
        /// </summary>
		public string OPER_DCT { get; set; } 

        /// <summary>
        /// 미상담여부(미상담:Y 상담: Null or N)
        /// </summary>
		public string SANGDAM { get; set; } 

        /// <summary>
        /// 액팅여부
        /// </summary>
		public string ACTIVE { get; set; } 

        /// <summary>
        /// 액팅파트
        /// </summary>
		public string ACTPART { get; set; } 

        /// <summary>
        /// 입력사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 입력시각
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 판독시각
        /// </summary>
		public DateTime? READTIME { get; set; } 

        
        /// <summary>
        /// 종합검진 검진결과 상세내역
        /// </summary>
        public HEA_RESULT()
        {
        }
    }
}
