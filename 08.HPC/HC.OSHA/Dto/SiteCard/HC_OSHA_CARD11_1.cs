namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 건강증진운동
    /// </summary>
    public class HC_OSHA_CARD11_1 : BaseDto
    {
        
        /// <summary>
        /// 건강증진운동 추진
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
        /// 개시일자
        /// </summary>
        public string PUBLISHDATE { get; set; } 

        /// <summary>
        /// 추진일자
        /// </summary>
		public string ACTDATE { get; set; } 

        /// <summary>
        /// 시설장비현황
        /// </summary>
		public string STATUS { get; set; } 

        /// <summary>
        /// 내용
        /// </summary>
		public string CONTENT { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD11_1()
        {
        }
    }
}
