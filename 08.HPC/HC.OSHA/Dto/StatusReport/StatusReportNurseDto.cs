using ComBase.Mvc;
using ComBase.Mvc.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{
    /// <summary>
    /// 상태보고서 간호사
    /// </summary>
    public class StatusReportNurseDto : BaseDto
    {
        public StatusReportNurseDto()
        {
            this.VISITDATE = DateTime.Now.ToString("yyyyMMdd");
      //      this.VISITRESERVEDATE = DateTime.Now.ToString("yyyyMMdd");
        }
        /// <summary>
        ///아이디
        /// <summary>
        public long ID { get; set; }
        public long SITE_ID { get; set; }
        public long ESTIMATE_ID { get; set; }

        /// <summary>
        /// 방문일자
        /// </summary>
        [MTSNotEmpty]
        public string VISITDATE { get; set; }
        /// <summary>
        ///방문예정일
        /// <summary>
        public string VISITRESERVEDATE { get; set; }

        /// 1.사업장현황
        [JsonIgnore]
        public SiteStatusDto SiteStatusDto { get; set; }
        ///2.업무수행내용 JSON
        [JsonIgnore]
        public string PERFORMCONTENT { get; set; }
        /// <summary>
        ///사업장 성명
        /// <summary>
        public string SITEMANAGERNAME { get; set; }
        /// <summary>
        ///사업장 직위
        /// <summary>
        public string SITEMANAGERGRADE { get; set; }
        /// <summary>
        ///보건관리자 간호사
        /// <summary>
        public string NURSENAME { get; set; }
        /// <summary>
        /// 사업장  서명
        /// </summary>
        public string SITEMANAGERSIGN { get; set; }

        /// <summary>
        /// 사업장 근로자 상담  서명
        /// </summary>
        public string SANGDAMSIGN { get; set; }
        /// <summary>
        ///삭제여부
        /// <summary>
        public string ISDELETED { get; set; }
        /// <summary>
        ///
        /// <summary>
        public DateTime? MODIFIED { get; set; }
        /// <summary>
        ///
        /// <summary>
        public string MODIFIEDUSER { get; set; }
        /// <summary>
        ///
        /// <summary>
        public DateTime? CREATED { get; set; }
        /// <summary>
        ///
        /// <summary>
        public string CREATEDUSER { get; set; }
        [JsonIgnore]

        public string OPINION { get; set; }
    }
}
