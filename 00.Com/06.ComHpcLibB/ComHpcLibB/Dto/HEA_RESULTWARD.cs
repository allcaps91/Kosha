namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_RESULTWARD : BaseDto
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
        /// 단계
        /// </summary>
		public string STEP { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
		public string ROWID { get; set; }        

        /// <summary>
        /// 
        /// </summary>
        public HEA_RESULTWARD()
        {
        }
    }
}
