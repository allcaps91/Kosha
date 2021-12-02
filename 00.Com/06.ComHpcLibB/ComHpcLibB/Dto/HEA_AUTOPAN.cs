namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_AUTOPAN : BaseDto
    {
        
        /// <summary>
        /// 순번
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 정렬
        /// </summary>
		public long RANKING { get; set; } 

        /// <summary>
        /// 작성시간
        /// </summary>
		public DateTime? WRITEDATE { get; set; } 

        /// <summary>
        /// 작성자
        /// </summary>
		public long WRITESABUN { get; set; } 

        /// <summary>
        /// 내용
        /// </summary>
		public string TEXT { get; set; } 

        /// <summary>
        /// 그룹
        /// </summary>
		public long GRPNO { get; set; } 

        
        /// <summary>
        /// 종검가판정 마스터 테이블
        /// </summary>
        public HEA_AUTOPAN()
        {
        }
    }
}
