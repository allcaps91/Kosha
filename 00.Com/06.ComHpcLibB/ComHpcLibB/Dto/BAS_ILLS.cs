namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class BAS_ILLS : BaseDto
    {
        
        /// <summary>
        /// 전산코드
        /// </summary>
		public string ILLCODE { get; set; } 

        /// <summary>
        /// 상병분류
        /// </summary>
		public string ILLCLASS { get; set; } 

        /// <summary>
        /// 상병명(한글)
        /// </summary>
		public string ILLNAMEK { get; set; } 

        /// <summary>
        /// 상병명(영문)
        /// </summary>
		public string ILLNAMEE { get; set; } 

        /// <summary>
        /// Name Spacing,Print시의 자리수 "Default:0"
        /// </summary>
		public string NAMESPACING { get; set; } 

        /// <summary>
        /// Display Header,Print시의 앞부분의 내용(제목)
        /// </summary>
		public string DISPHEADER { get; set; } 

        /// <summary>
        /// 리턴값
        /// </summary>
		public long RETURNVAL { get; set; } 

        /// <summary>
        /// 불완전코드
        /// </summary>
		public string NOUSE { get; set; } 

        /// <summary>
        /// 성별
        /// </summary>
		public string SEX { get; set; } 

        /// <summary>
        /// 감염여부
        /// </summary>
		public string INFECT { get; set; } 

        /// <summary>
        /// 서울응급전송시 해당되는 상병일 경우 1, 나머지 NULL
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 상위상병코드
        /// </summary>
		public string ILLUPCODE { get; set; } 

        /// <summary>
        /// 상병코드
        /// </summary>
		public string ILLCODED { get; set; } 

        /// <summary>
        /// 감염종류
        /// </summary>
		public string GBINFECT { get; set; } 

        /// <summary>
        /// KCD6 차 여부
        /// </summary>
		public string KCD6 { get; set; } 

        /// <summary>
        /// 사용일자
        /// </summary>
		public DateTime? SDATE { get; set; } 

        /// <summary>
        /// 삭제일자
        /// </summary>
		public DateTime? DDATE { get; set; } 

        /// <summary>
        /// 불완전적용일
        /// </summary>
		public DateTime? NOUSEDATE { get; set; } 

        /// <summary>
        /// *: V252
        /// </summary>
		public string GBV252 { get; set; } 

        /// <summary>
        /// V특정코드(2018년도까지만 사용)
        /// </summary>
		public string GBVCODE { get; set; } 

        /// <summary>
        /// KCD7 차 여부
        /// </summary>
		public string KCD7 { get; set; } 

        /// <summary>
        /// 상병명(한글) old
        /// </summary>
		public string ILLNAMEK_OLD { get; set; } 

        /// <summary>
        /// 상병명(영문) old
        /// </summary>
		public string ILLNAMEE_OLD { get; set; } 

        /// <summary>
        /// 2016-01-02  N:이전 불완전 풀린경우
        /// </summary>
		public string GBCHK { get; set; } 

        /// <summary>
        /// KCD6 old
        /// </summary>
		public string KCDOLD { get; set; } 

        /// <summary>
        /// 입원료 본인부담 주상병 예외대상(Y,기본 Null 및 N)
        /// </summary>
		public string IPDETC { get; set; } 

        /// <summary>
        /// 권역중증상병(*) *2017년 기준 
        /// </summary>
		public string GBER { get; set; } 

        /// <summary>
        /// 경증상병
        /// </summary>
		public string GBV352 { get; set; } 

        /// <summary>
        /// V특정코드(희귀:2019년도부터 사용)
        /// </summary>
		public string GBVCODE1 { get; set; } 

        /// <summary>
        /// V특정코드(중증난치:2019년도부터 사용)
        /// </summary>
		public string GBVCODE2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GBVCODE3 { get; set; } 

        
        /// <summary>
        /// 상병명 Table (국제4단 분류)
        /// </summary>
        public BAS_ILLS()
        {
        }
    }
}
