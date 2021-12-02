namespace HC.OSHA.Visit.Schedule.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// �޷� �̺�Ʈ ��
    /// </summary>
    public class CalendarEventModel : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long EVENT_ID { get; set; }

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
		public DateTime? EVENTSTARTDATETIME { get; set; }
        public DateTime? EVENTENDDATETIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string VISITUSERNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ISFEE { get; set; }

        
        public CalendarEventModel()
        {
        }
    }
}
