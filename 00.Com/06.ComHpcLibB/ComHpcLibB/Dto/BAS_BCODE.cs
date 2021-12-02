namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class BAS_BCODE : BaseDto
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
		public string JDATE { get; set; } 

        /// <summary>
        /// 삭제(적용취소)일자
        /// </summary>
		public string DELDATE { get; set; } 

        /// <summary>
        /// 최종 등록(변경) 작업자 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 최종 등록(변경) 작업 시간(YYYY-MM-DD HH24:MI)
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// DISPLAY 순서
        /// </summary>
		public long SORT { get; set; } 

        /// <summary>
        /// 기본은 NULL, 간호진단구분(1:기본, 2:일반병동 3:3C, 4:3A)
        /// </summary>
		public string PART { get; set; } 

        /// <summary>
        /// 건수
        /// </summary>
		public long CNT { get; set; } 

        /// <summary>
        /// 구분항목
        /// </summary>
		public string GUBUN2 { get; set; } 

        /// <summary>
        /// 구분항목
        /// </summary>
		public string GUBUN3 { get; set; } 

        /// <summary>
        /// 구분항목
        /// </summary>
		public string GUBUN4 { get; set; } 

        /// <summary>
        /// 구분항목
        /// </summary>
		public string GUBUN5 { get; set; } 

        /// <summary>
        /// 구분항목
        /// </summary>
		public long GUNUM1 { get; set; } 

        /// <summary>
        /// 구분항목
        /// </summary>
		public long GUNUM2 { get; set; } 

        /// <summary>
        /// 구분항목
        /// </summary>
		public long GUNUM3 { get; set; }

        /// <summary>
        /// 구분항목
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 원무행정,OCS등 코드명칭 Table
        /// </summary>
        public BAS_BCODE()
        {
        }
    }
}
