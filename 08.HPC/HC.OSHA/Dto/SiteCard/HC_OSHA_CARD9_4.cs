namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 9. 위험성평가 컨설팅
    /// </summary>
    public class HC_OSHA_CARD9_4 : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SITE_ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long ESTIMATE_ID { get; set; } 

        /// <summary>
        /// 컨설팅 시작
        /// </summary>
		public string STARTDATE { get; set; }

        public string PERIOD { get; set; }

        /// <summary>
        /// 컨설팅 종료
        /// </summary>
        public string ENDDATE { get; set; } 

        /// <summary>
        /// 수료기관 및 전ㅁ누가
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 주요컨설팅내용
        /// </summary>
		public string CONTENT { get; set; } 

        /// <summary>
        /// 참고ㅎ사항
        /// </summary>
		public string REMARK { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? MODIFIED { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MODIFIEDUSER { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? CREATED { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string CREATEDUSER { get; set; }
    }
}
