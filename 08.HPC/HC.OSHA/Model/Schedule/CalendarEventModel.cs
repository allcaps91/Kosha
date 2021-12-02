namespace HC.OSHA.Model
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
        public string VISITMANAGERNAME { get; set; }

        public string VISITSTARTTIME { get; set; }
        

        /// <summary>
        /// 
        /// </summary>
		public string ISFEE { get; set; }

        
        public CalendarEventModel()
        {
        }
    }
}
