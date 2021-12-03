namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_READING : BaseDto
    {
        
        /// <summary>
        /// 사업장코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 사업장이름
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 등록일자
        /// </summary>
		public DateTime? REG_DATE { get; set; } 

        /// <summary>
        /// 등록자
        /// </summary>
		public string REG_SABUN { get; set; }

        /// <summary>
        /// 등록자명
        /// </summary>
        public string REG_SABUN_NM { get; set; }

        /// <summary>
        /// 등록자료
        /// </summary>
        public string DATA1 { get; set; } 

        /// <summary>
        /// 삭제날짜
        /// </summary>
		public DateTime? DEL_DATE { get; set; } 

        /// <summary>
        /// 등록년도
        /// </summary>
		public string YEAR { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HIC_READING()
        {
        }
    }
}
