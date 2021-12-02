namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_MANAGE : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 접수일자
        /// </summary>
		public DateTime? JEPDATE { get; set; } 

        /// <summary>
        /// 회사코드
        /// </summary>
		public long LTDCODE { get; set; } 

        /// <summary>
        /// 근무중치료
        /// </summary>
		public string WORK1 { get; set; } 

        /// <summary>
        /// 추적검사
        /// </summary>
		public string WORK2 { get; set; } 

        /// <summary>
        /// 교육
        /// </summary>
		public string WORK3 { get; set; } 

        /// <summary>
        /// 영양관리(음주)
        /// </summary>
		public string WORK4 { get; set; } 

        /// <summary>
        /// 식이요법
        /// </summary>
		public string WORK5 { get; set; } 

        /// <summary>
        /// 운동관리
        /// </summary>
		public string WORK6 { get; set; } 

        /// <summary>
        /// 스트레스관리
        /// </summary>
		public string WORK7 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE2 { get; set; } 

        
        /// <summary>
        /// 종검 판정 관리 등록
        /// </summary>
        public HEA_MANAGE()
        {
        }
    }
}
