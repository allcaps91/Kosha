namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_LTD_TAX : BaseDto
    {
        
        /// <summary>
        /// 회사코드
        /// </summary>
		public long LTDCODE { get; set; } 

        /// <summary>
        /// 부서구분(1.종검 2.일반 3.측정 4.대행)
        /// </summary>
		public string BUSE { get; set; } 

        /// <summary>
        /// 담당자 성명
        /// </summary>
		public string DAMNAME { get; set; } 

        /// <summary>
        /// 담당자 직책
        /// </summary>
		public string DAMJIK { get; set; } 

        /// <summary>
        /// 전화번호
        /// </summary>
		public string TEL { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string HPHONE { get; set; } 

        /// <summary>
        /// 이메일
        /// </summary>
		public string EMAIL { get; set; } 

        /// <summary>
        /// 전자세금계산서 관련 참고사항
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 등록자 사번
        /// </summary>
		public long JOBSABUN { get; set; } 

        /// <summary>
        /// 등록 일시
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long LTDCODE1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 건강증진센타 전자세금계산서 발행 정보
        /// </summary>
        public HIC_LTD_TAX()
        {
        }
    }
}
