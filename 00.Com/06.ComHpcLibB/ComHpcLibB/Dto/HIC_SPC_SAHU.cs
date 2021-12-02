namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_SPC_SAHU : BaseDto
    {
        
        /// <summary>
        /// 검진년도
        /// </summary>
		public string GJYEAR { get; set; } 

        /// <summary>
        /// 회사코드
        /// </summary>
		public long LTDCODE { get; set; } 

        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 판정구분(C1,D1,C2,D2)
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 소견코드
        /// </summary>
		public string SOGEN { get; set; } 

        /// <summary>
        /// 검사항목들
        /// </summary>
		public string EXAM { get; set; } 

        /// <summary>
        /// 조치사항
        /// </summary>
		public string JOCHI { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE2 { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HIC_SPC_SAHU()
        {
        }
    }
}
