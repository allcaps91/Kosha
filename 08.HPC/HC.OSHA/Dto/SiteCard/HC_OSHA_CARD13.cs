namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 사업장관리카드 13. 보호구
    /// </summary>
    public class HC_OSHA_CARD13 : BaseDto
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
        public string YEAR { get; set; }
        /// <summary>
        /// 보호구명
        /// </summary>
        public string NAME { get; set; } 

        /// <summary>
        /// 지급대상 작업명
        /// </summary>
		public string TARGET { get; set; } 

        /// <summary>
        /// 근로자수
        /// </summary>
		public string WORKERCOUNT { get; set; } 

        /// <summary>
        /// 지급수향
        /// </summary>
		public string TARGETCOUNT { get; set; } 

        /// <summary>
        /// 검정합격번호
        /// </summary>
		public string PASSNUMBER { get; set; } 

        /// <summary>
        /// 변동사항
        /// </summary>
		public string REMARK { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD13()
        {
        }
    }
}
