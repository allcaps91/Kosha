namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_TITEM : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 구분(아래참조)
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 점수
        /// </summary>
		public long JUMSU { get; set; } 

        /// <summary>
        /// 검진번호
        /// </summary>
		public long PANO { get; set; } 

        /// <summary>
        /// 검진년도
        /// </summary>
		public string GJYEAR { get; set; }

        public string ROWID { get; set; }

        public long TOTAL { get; set; }

        /// <summary>
        /// 생활습관도구표
        /// </summary>
        public HIC_TITEM()
        {
        }
    }
}
