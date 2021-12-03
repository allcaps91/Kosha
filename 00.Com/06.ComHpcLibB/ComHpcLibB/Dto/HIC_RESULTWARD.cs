namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_RESULTWARD : BaseDto
    {
        
        /// <summary>
        /// 판정의사 사번
        /// </summary>
		public long SABUN { get; set; } 

        /// <summary>
        /// 판정 상병코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 상병내용
        /// </summary>
		public string WARDNAME { get; set; } 

        /// <summary>
        /// 질환종류
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 순서
        /// </summary>
		public long SEQNO { get; set; } 

        /// <summary>
        /// 소견분류
        /// </summary>
		public string GUBUN2 { get; set; } 

        /// <summary>
        /// 검사종류
        /// </summary>
		public string EXAMWARD { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
		public string ROWID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HIC_RESULTWARD()
        {
        }
    }
}
