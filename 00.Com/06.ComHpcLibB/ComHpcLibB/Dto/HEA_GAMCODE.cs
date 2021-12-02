namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_GAMCODE : BaseDto
    {
        
        /// <summary>
        /// 감액코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 감액코드명칭
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 적용방법(1.할인율 2.감액금액 3.감액직접입력)
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 할인율(%)
        /// </summary>
		public long RATE { get; set; } 

        /// <summary>
        /// 감액금액(남)
        /// </summary>
		public long AMT1 { get; set; } 

        /// <summary>
        /// 감액금액(여)
        /// </summary>
		public long AMT2 { get; set; } 

        /// <summary>
        /// 최종변경일자
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 최종변경자
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 삭제일자
        /// </summary>
		public string DELDATE { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 종합검진 감액코드
        /// </summary>
        public HEA_GAMCODE()
        {
        }
    }
}
