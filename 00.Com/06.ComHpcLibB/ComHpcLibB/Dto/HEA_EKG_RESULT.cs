namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_EKG_RESULT : BaseDto
    {
        
        /// <summary>
        /// 종검번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 결과
        /// </summary>
		public string EKGRESULT { get; set; } 

        /// <summary>
        /// 등록일시
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string EKGRESULT2 { get; set; } 

        
        /// <summary>
        /// ekg 결과 판독
        /// </summary>
        public HEA_EKG_RESULT()
        {
        }
    }
}
