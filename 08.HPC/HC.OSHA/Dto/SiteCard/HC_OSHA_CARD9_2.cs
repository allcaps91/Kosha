namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 9. 위험성평가 수시평가
    /// </summary>
    public class HC_OSHA_CARD9_2 : BaseDto
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
        /// 대상작업 또는 공정
        /// </summary>
		public string WORKNAME { get; set; }
        public string PERIOD { get; set; }
        
        /// <summary>
        /// 실시기간
        /// </summary>
		public string STARTDATE { get; set; } 

        /// <summary>
        /// 실시기간
        /// </summary>
		public string ENDDATE { get; set; } 

        /// <summary>
        /// 실시결과
        /// </summary>
		public string CONTENT { get; set; } 

        /// <summary>
        /// 실시자1 직책
        /// </summary>
		public string GRADE1 { get; set; } 

        /// <summary>
        /// 실시자1 성명
        /// </summary>
		public string NAME1 { get; set; } 

        /// <summary>
        /// 실시자2 직책
        /// </summary>
		public string GRADE2 { get; set; } 

        /// <summary>
        /// 실시자2 성명
        /// </summary>
		public string NAME2 { get; set; }

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
