namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_ORGANCODE : BaseDto
    {
        
        /// <summary>
        /// 표적장기코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 표적장기명
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 선정사유코드
        /// </summary>
		public string SAYUCODE { get; set; } 

        /// <summary>
        /// 선정사유
        /// </summary>
		public string SAYUNAME { get; set; } 

        /// <summary>
        /// 구분(1. 대분류  2.소분류)
        /// </summary>
		public string GUBUN { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 특수검진 표적장기 코드
        /// </summary>
        public HIC_ORGANCODE()
        {
        }
    }
}
