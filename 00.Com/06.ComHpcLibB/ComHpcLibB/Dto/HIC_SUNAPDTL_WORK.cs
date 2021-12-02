namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_SUNAPDTL_WORK : BaseDto
    {
        
        /// <summary>
        /// 검진접수 일련번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 묶음코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 유해물질
        /// </summary>
		public string UCODE { get; set; } 

        /// <summary>
        /// 금액
        /// </summary>
		public long AMT { get; set; } 

        /// <summary>
        /// 부담율
        /// </summary>
		public string GBSELF { get; set; } 

        /// <summary>
        /// 수납일자
        /// </summary>
		public string SUDATE { get; set; } 

        /// <summary>
        /// 검진번호
        /// </summary>
		public long PANO { get; set; } 

        /// <summary>
        /// 검진종류 (2010-10-01 추가  김민철)
        /// </summary>
		public string GJJONG { get; set; }

        public string EXCODE { get; set; }

        public string HNAME { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public HIC_SUNAPDTL_WORK()
        {
        }
    }
}
