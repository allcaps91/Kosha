namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_AUTOPAN_MATCH : BaseDto
    {
        
        /// <summary>
        /// 순번
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 검사코드
        /// </summary>
		public string EXCODE { get; set; } 

        /// <summary>
        /// 매칭코드
        /// </summary>
		public string MCODE { get; set; } 

        
        /// <summary>
        /// 종검가판정 계산로직
        /// </summary>
        public HEA_AUTOPAN_MATCH()
        {
        }
    }
}
