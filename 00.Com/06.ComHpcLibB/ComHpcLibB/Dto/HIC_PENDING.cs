namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_PENDING : BaseDto
    {
        
        /// <summary>
        /// 접수일자
        /// </summary>
		public string JEPDATE { get; set; } 

        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 누락검사명
        /// </summary>
		public string EXAMNAME { get; set; } 

        /// <summary>
        /// 사유
        /// </summary>
		public string SAYU { get; set; } 

        /// <summary>
        /// 등록일시
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 보고자 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 검체제출일
        /// </summary>
		public DateTime? ENDDATE { get; set; } 

        /// <summary>
        /// 완료자 사번
        /// </summary>
		public long ENDSABUN { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 일반검진 : 1 / 종검 : 2
        /// </summary>
        public string GUBUN { get; set; }


        /// <summary>
        /// 보류대장
        /// </summary>
        public HIC_PENDING()
        {
        }
    }
}
