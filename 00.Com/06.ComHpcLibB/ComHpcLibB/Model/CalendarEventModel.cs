using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComHpcLibB
{
    public class CalendarEventModel
    {
        /// <summary>
        /// 
        /// </summary>
		public long EVENT_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long INWON { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string LTDNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? RDATE { get; set; }
        public string RTIME { get; set; }
        public DateTime? STARTTIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SPECIAL { get; set; }
        public string REMARK { get; set; }

        public string GBCHANGE { get; set; }


        public CalendarEventModel()
        {
        }
    }
}
