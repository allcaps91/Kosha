namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_SPC_SOGENEXAM : BaseDto
    {
        
        /// <summary>
        /// 특수검진 판정소견 코드
        /// </summary>
		public string SOGENCODE { get; set; } 

        /// <summary>
        /// 검사코드
        /// </summary>
		public string EXCODE { get; set; } 

        /// <summary>
        /// 소견항목
        /// </summary>
		public string HANG { get; set; } 

        
        /// <summary>
        /// 특수검진 판정소견별 관련검사 항목 설정
        /// </summary>
        public HIC_SPC_SOGENEXAM()
        {
        }
    }
}
