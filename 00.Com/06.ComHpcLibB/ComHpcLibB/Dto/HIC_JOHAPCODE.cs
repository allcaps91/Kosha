namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JOHAPCODE : BaseDto
    {
        
        /// <summary>
        /// 조합코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 조합구분(1.공단 2.직장 3.지역)
        /// </summary>
		public string CLASS { get; set; } 

        /// <summary>
        /// 조합명칭
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 전화번호
        /// </summary>
		public string TEL { get; set; } 

        /// <summary>
        /// FAX
        /// </summary>
		public string FAX { get; set; } 

        /// <summary>
        /// 우편번호
        /// </summary>
		public string MAIL { get; set; } 

        /// <summary>
        /// 주소① : 주소
        /// </summary>
		public string JUSO1 { get; set; } 

        /// <summary>
        /// 주소② : 건물 동호수
        /// </summary>
		public string JUSO2 { get; set; } 

        /// <summary>
        /// 주소③ : 수취인 성명 및 직위
        /// </summary>
		public string JUSO3 { get; set; } 

        /// <summary>
        /// 삭제일자
        /// </summary>
		public DateTime? DELDATE { get; set; } 

        
        /// <summary>
        /// 일반건진 조합코드
        /// </summary>
        public HIC_JOHAPCODE()
        {
        }
    }
}
