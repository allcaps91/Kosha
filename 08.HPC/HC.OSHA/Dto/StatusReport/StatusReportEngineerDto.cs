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
    /// 상태보고서 산업위생기사
    /// </summary>
    public class StatusReportEngineerDto : BaseDto
    {
        public StatusReportEngineerDto()
        {
            this.VISITDATE = DateTime.Now.ToString("yyyyMMdd");
       //     this.VISITRESERVEDATE = DateTime.Now.ToString("yyyyMMdd");
        }
        /// <summary>
        ///아이디
        /// <summary>
        public long ID { get; set; }
        /// <summary>
        ///
        /// <summary>
        public long SITE_ID { get; set; }
        /// <summary>
        ///
        /// <summary>
        public long ESTIMATE_ID { get; set; }

        public string SITENAME { get; set; }
        public string SITEOWENER { get; set; }
        public string SITETEL { get; set; }
        public string SITEADDRESS { get; set; }
        /// <summary>
        ///
        /// <summary>
        [MTSNotEmpty]
        public string VISITDATE { get; set; }
        /// <summary>
        ///
        /// <summary>
        public string VISITRESERVEDATE { get; set; }
        /// <summary>
        ///총원
        /// <summary>
        public long WORKERCOUNT { get; set; }
        /// <summary>
        ///측정예정일
        /// <summary>
        public string WEMDATE { get; set; }
        public string WEMDATE2 { get; set; }
        public string WEMDATEREMARK { get; set; }
        /// <summary>
        ///유해인자
        /// <summary>
        public string WEMHARMFULFACTORS { get; set; }
        /// <summary>
        ///노출기준초과여부
        /// <summary>
        public string WEMEXPORSURE { get; set; }
        public string WEMEXPORSURE1 { get; set; }
        /// <summary>
        ///초과공정
        /// <summary>
        public string WEMEXPORSUREREMARK { get; set; }
        /// <summary>
        /// 사업장 담당자 업무협의 주요내용
        /// <summary>
        public string WORKCONTENT { get; set; }
        /// <summary>
        ///산업안전보건위원회실시예정일
        /// <summary>
        public string OSHADATE { get; set; }
        /// <summary>
        ///산업안전보건 주요내용
        /// <summary>
        public string OSHACONTENT { get; set; }
        /// <summary>
        ///보건교육대상
        /// <summary>
        public string EDUTARGET { get; set; }
        /// <summary>
        ///보건교육 참석자
        /// <summary>
         [JsonIgnore]
        public string EDUPERSON { get; set; }
        /// <summary>
        ///보건교육 교안여부
        /// <summary>
        public string EDUAN { get; set; }
        /// <summary>
        ///보건교육 주제
        /// <summary>
        public string EDUTITLE { get; set; }
        /// <summary>
        ///보건교육 종류JSON
        /// <summary>
        [JsonIgnore]
        public string EDUTYPEJSON { get; set; }
        /// <summary>
        ///보건교육방법JSON
        /// <summary>
        [JsonIgnore]
        public string EDUMETHODJSON { get; set; }



        /// <summary>
        ///작업환경점검JSON
        /// <summary>
        [JsonIgnore]
        public string ENVCHECKJSON1 { get; set; }
        [JsonIgnore]
        public string ENVCHECKJSON2 { get; set; }
        [JsonIgnore]
        public string ENVCHECKJSON3 { get; set; }
        /// <summary>
        ///사업장 성명
        /// <summary>
        public string SITEMANAGERNAME { get; set; }
        /// <summary>
        ///사업장 직위
        /// <summary>
        public string SITEMANAGERGRADE { get; set; }
        /// <summary>
        ///보건관리자
        /// <summary>
        public string ENGINEERNAME { get; set; }
        /// <summary>
        /// 사업장  서명
        /// </summary>
        public string SITEMANAGERSIGN { get; set; }
        /// <summary>
        ///삭제여부
        /// <summary>
        public string ISDELETED { get; set; }
        /// <summary>
        ///
        /// <summary>
        public DateTime? APPROVE { get; set; }
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
        public string PERFORMCONTENT { get; internal set; }

        [JsonIgnore]
        public string OPINION { get; set; }
    }
}
 