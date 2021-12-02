using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.Model.Visit
{
    public class DailyReportVisitModel
    {
        public string Name { get; set; }
        public string VisitTime { get; set; }
        public string VisitUserName{ get; set; }
        public string VisitUserID { get; set; }
        public long VisitID { get; set; }
        public string Role { get; set; }
        public string REMARK { get; set; }

    }
}
