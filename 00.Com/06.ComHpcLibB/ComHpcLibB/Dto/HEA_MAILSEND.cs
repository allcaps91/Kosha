namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_MAILSEND : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 우편번호
        /// </summary>
		public string MAILCODE { get; set; } 

        /// <summary>
        /// 주소
        /// </summary>
		public string JUSO { get; set; } 

        /// <summary>
        /// 수검자명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 발송일자
        /// </summary>
		public string SENDDATE { get; set; } 

        /// <summary>
        /// 우편물순서
        /// </summary>
		public long SEQ { get; set; } 

        /// <summary>
        /// 작업사번
        /// </summary>
		public long ENTSABUN { get; set; }

        /// <summary>
        /// 작업성명
        /// </summary>
        public string ENTNAME { get; set; }

        /// <summary>
        /// 발송정보
        /// </summary>
        public string SEND_INFO { get; set; }

        /// <summary>
        /// 종검 결과지 우편물처
        /// </summary>
        public HEA_MAILSEND()
        {
        }
    }
}
