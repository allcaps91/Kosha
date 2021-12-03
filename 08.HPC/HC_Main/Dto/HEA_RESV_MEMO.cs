namespace HC_Main.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_RESV_MEMO : BaseDto
    {
        
        /// <summary>
        /// 검진일자
        /// </summary>
		public string SDATE { get; set; } 

        /// <summary>
        /// 수검자명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 검사명
        /// </summary>
		public string EXAMENAME { get; set; } 

        /// <summary>
        /// 검체제출일자
        /// </summary>
		public string SUBDATE { get; set; } 

        /// <summary>
        /// 연락처
        /// </summary>
		public string TEL { get; set; } 

        /// <summary>
        /// 입력시각
        /// </summary>
		public string ENTTIME { get; set; } 

        /// <summary>
        /// 입력사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 확인여부
        /// </summary>
		public string CONFIRM { get; set; } 

        /// <summary>
        /// 상담내용
        /// </summary>
		public string SANGDAM { get; set; }

        /// <summary>
        /// 상담내용
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 선택검사 관리노트 메모
        /// </summary>
        public HEA_RESV_MEMO()
        {
        }
    }
}
