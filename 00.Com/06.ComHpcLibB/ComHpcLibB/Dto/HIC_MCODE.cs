namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_MCODE : BaseDto
    {
        
        /// <summary>
        /// 코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 취급물질명
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 취급물질 약어
        /// </summary>
		public string YNAME { get; set; }

        /// <summary>
        /// 취급물질명
        /// </summary>
        public string UNAME { get; set; }

        /// <summary>
        /// 유해인자코드
        /// </summary>
        public string UCODE { get; set; } 

        /// <summary>
        /// 검사주기(개월수)
        /// </summary>
		public long JUGI { get; set; } 

        /// <summary>
        /// 선택검사 유무(Y/N)
        /// </summary>
		public string GBSELECT { get; set; } 

        /// <summary>
        /// 최종 등록자 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 최종 변경 일시
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 특수건강진단표 유해인자 분류코드
        /// </summary>
		public long TONGBUN { get; set; } 

        /// <summary>
        /// 특수구강상담여부(Y/N)
        /// </summary>
		public string GBDENT { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string IsDelete { get; set; }

        /// <summary>
        /// 취급물질명(유해인자)코드
        /// </summary>
        public HIC_MCODE()
        {
        }
    }
}
