using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.Model.Schedule
{
    /// <summary>
    /// 방문 공문 모델
    /// </summary>
    public class VisitDocumentModel
    {
        public long Id { get; set; }
        public string SiteName { get; set; }
        public long SiteId { get; set; }
        public long ESTIMATE_ID { get; set; }
        public string VisitReserveDate { get; set; }
        public string visitUserId { get; set; }
        public string visitUserName { get; set; }
        public string visitUserRole { get; set; }
        public string visitUserId2 { get; set; }
        public string visitUserName2 { get; set; }
        public string visitUserRole2 { get; set; }
        public string visitPlace { get; set; }
        public string SENDDATE { get; set; }
        public string SENDNAME { get; set; }
        public string SENDMAIL { get; set; }
    }
}
