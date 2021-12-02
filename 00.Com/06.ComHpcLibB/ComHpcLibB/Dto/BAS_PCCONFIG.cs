namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;


    /// <summary>
    /// 
    /// </summary>
    public class BAS_PCCONFIG : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
        
		public string IPADDRESS { get; set; } 

        /// <summary>
        /// 코드구분
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 문자값
        /// </summary>
		public string VALUEV { get; set; } 

        /// <summary>
        /// 숫자값
        /// </summary>
		public long VALUEN { get; set; } 

        /// <summary>
        /// 정렬순서
        /// </summary>
		public long DISSEQNO { get; set; } 

        /// <summary>
        /// 입력일자
        /// </summary>
		public string INPDATE { get; set; } 

        /// <summary>
        /// 입력시간
        /// </summary>
		public string INPTIME { get; set; } 

        /// <summary>
        /// 삭제여부 0:사용 1:삭제
        /// </summary>
		public string DELGB { get; set; } 

        
        /// <summary>
        /// PC 환경 설정
        /// </summary>
        public BAS_PCCONFIG()
        {
        }
    }
}
