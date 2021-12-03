using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.Model.Visit
{
    public class VisitPriceModel
    {
       public  DateTime VISITDATETIME { get; set; }
        public long VISIT_ID { get; set; }
        public long WORKERCOUNT { get; set; }
        public double UNITPRICE { get; set; }
        public long TOTALPRICE { get; set; }
    }
}
