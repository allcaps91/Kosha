namespace HC.OSHA.Visit.Schedule.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// �̹湮 �����
    /// </summary>
    public class UnvisitSiteModel : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long SCHEDULE_ID { get; set; }

        /// <summary>
        /// ������
        /// </summary>
		public string NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long SITE_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string VISITRESERVEDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string VISITUSERNAME { get; set; }

        
        public UnvisitSiteModel()
        {
        }
    }
}
