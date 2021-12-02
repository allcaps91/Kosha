namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_SCODE : BaseDto
    {
        
        /// <summary>
        /// 코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 내역
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 판정
        /// </summary>
		public string PANJENG { get; set; } 

        /// <summary>
        /// 차수
        /// </summary>
		public string CHASU { get; set; } 

        /// <summary>
        /// 유해인자
        /// </summary>
		public string UCODE { get; set; } 

        /// <summary>
        /// 작업자사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 작업일자
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// S코드
        /// </summary>
		public string SCODE { get; set; } 

        /// <summary>
        /// J코드
        /// </summary>
		public string JCODE { get; set; }

        public string RID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HIC_SCODE()
        {
        }
    }
}
