namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class BAS_MAILNEW : BaseDto
    {
        
        /// <summary>
        /// 우편번호
        /// </summary>
		public string MAILCODE { get; set; } 

        /// <summary>
        /// 동명칭
        /// </summary>
		public string MAILDONG { get; set; } 

        /// <summary>
        /// 시군동명(주소)
        /// </summary>
		public string MAILJUSO { get; set; } 

        /// <summary>
        /// 지역코드
        /// </summary>
		public long MAILJIYEK { get; set; } 

        /// <summary>
        /// 기록실지역코드
        /// </summary>
		public string MAILGUBUN { get; set; } 

        /// <summary>
        /// 삭제(*),사용NULL+공백
        /// </summary>
		public string GBDEL { get; set; } 

        /// <summary>
        /// 구분(1:기존주소, 2:도로명주소)
        /// </summary>
		public string GUBUN { get; set; } 

        
        /// <summary>
        /// 우편번호(동명칭,주소)
        /// </summary>
        public BAS_MAILNEW()
        {
        }
    }
}
