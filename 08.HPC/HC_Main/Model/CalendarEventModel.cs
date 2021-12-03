using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_Main.Model
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 달력 이벤트 모델
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
