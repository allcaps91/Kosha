namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class BAS_GAMF : BaseDto
    {
        
        /// <summary>
        /// 주민등록번호
        /// </summary>
		public string GAMJUMIN { get; set; } 

        /// <summary>
        /// 사원번호
        /// </summary>
		public string GAMSABUN { get; set; } 

        /// <summary>
        /// 감액구분(2005-08-08부터 사용 안함)
        /// </summary>
		public string GAMGUBUN { get; set; } 

        /// <summary>
        /// 입사일자
        /// </summary>
		public DateTime? GAMENTER { get; set; } 

        /// <summary>
        /// 퇴사일자
        /// </summary>
		public DateTime? GAMOUT { get; set; } 

        /// <summary>
        /// 감액적용종료일자
        /// </summary>
		public DateTime? GAMEND { get; set; } 

        /// <summary>
        /// 감액대상자내용
        /// </summary>
		public string GAMMESSAGE { get; set; } 

        /// <summary>
        /// 이름
        /// </summary>
		public string GAMNAME { get; set; } 

        /// <summary>
        /// 근무처
        /// </summary>
		public string GAMSOSOK { get; set; } 

        /// <summary>
        /// 감액구분(2005-08-08 부터 사용)
        /// </summary>
		public string GAMCODE { get; set; } 

        /// <summary>
        /// 암화화된 주민번호
        /// </summary>
		public string GAMJUMIN3 { get; set; } 

        /// <summary>
        /// abc 작업 시 등록번호 갱신함.
        /// </summary>
		public string PANO { get; set; } 

        /// <summary>
        /// 입력일자
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 입력사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        
        /// <summary>
        /// 직원 및 직계 감액 Table
        /// </summary>
        public BAS_GAMF()
        {
        }
    }
}
