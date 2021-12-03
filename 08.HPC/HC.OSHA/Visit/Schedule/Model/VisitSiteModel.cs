namespace HC.OSHA.Visit.Schedule.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 방문 사업장
    /// </summary>
    public class VisitSiteModel : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long SCHEDULE_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long VISIT_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long SITE_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? VISITDATETIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string VISITUSERNAME { get; set; }

        
        public VisitSiteModel()
        {
        }
    }
}
