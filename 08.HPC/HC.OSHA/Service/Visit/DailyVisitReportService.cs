using HC_OSHA.Repository.Visit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.Service.Visit
{
    public class DailyVisitReportService
    {
        public DailyReportRepository DailyReportRepository { get; }

        public DailyVisitReportService()
        {
            this.DailyReportRepository = new DailyReportRepository();
        }
    }
}
