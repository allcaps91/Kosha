using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.Model.Visit
{
    /// <summary>
    /// 출장일지 사업장
    /// </summary>
    public class DailyReportSiteModel : BaseDto
    {
        public long SITE_ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string SITE_MANAGER { get; set; }
        public string VISITUSER { get; set; }
        public string VISITUSERNAME { get; set; }
        public string ROLE { get; set; }
        public string VISITDOCTOR { get; set; }
        public string VISITDOCTORNAME { get; set; }
        public string REMARK { get; set; }
    }
}
