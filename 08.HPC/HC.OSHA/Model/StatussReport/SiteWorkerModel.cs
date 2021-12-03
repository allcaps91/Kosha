using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HC.OSHA.Model
{
    public class SiteWorkerModel : BaseDto
    {
        public string PTNO { get; set; }
        public string SNAME { get; set; }
        public string JUMIN { get; set; }
        public string SEX { get; set; }
        public string AGE { get; set; }
        public string TEL { get; set; }
        public string IPSADATE { get; set; }
        public string DEPT { get; set; }
    }
}
