using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto.StatusReport
{
    /// <summary>
    /// 근로자상담관리
    /// </summary>
    public class HealthCheckDto : BaseDto
    {
        public HealthCheckDto()
        {
            this.content = string.Empty;
            this.suggestion = string.Empty;
        }
        /// <summary>
        ///아이디
        /// <summary>
        public long id { get; set; }
        /// <summary>
        ///근로자아이디
        /// <summary>
        public string worker_id { get; set; }
        /// <summary>
        /// 사업장아이디
        /// </summary>
        public long site_id { get; set; }
        /// <summary>
        ///이름
        /// <summary>
        public string name { get; set; }
        /// <summary>
        ///소속(부서)
        /// <summary>
        public string dept { get; set; }
        /// <summary>
        ///성별
        /// <summary>
        public string gender { get; set; }
        /// <summary>
        ///나이
        /// <summary>
        public long age { get; set; }
        /// <summary>
        ///상담내용
        /// <summary>
        public string content { get; set; }
        /// <summary>
        ///상담후건의사항
        /// <summary>
        public string suggestion { get; set; }
        /// <summary>
        ///최고혈압
        /// <summary>
        public string bpl { get; set; }
        /// <summary>
        ///최저혈압
        /// <summary>
        public string bpr { get; set; }
        /// <summary>
        /// 혈압
        /// </summary>
        public string bp { get; set; }
        /// <summary>
        /// 혈당
        /// </summary>
        public string bst { get; set; }

        /// <summary>
        /// 단백뇨
        /// </summary>
        public string dan { get; set; }

        /// <summary>
        /// 체중
        /// </summary>
        public string WEIGHT { get; set; }

        /// <summary>
        /// 체지방
        /// </summary>
        public string BMI { get; set; }
        /// <summary>
        /// 음주량
        /// </summary>
        public string ALCHOL { get; set; }


        /// <summary>
        /// 흡연량
        /// </summary>
        public string SMOKE { get; set; }


        /// <summary>
        /// 상태보고서 아이디
        /// </summary>
        public long REPORT_ID { get; set; }

        /// <summary>
        /// 외래진료검사의뢰
        /// </summary>
        public string EXAM { get; set; }
        /// <summary>
        /// 상태보고서 의사 여부
        /// </summary>
        public string ISDOCTOR { get; set; }

        /// <summary>
        ///삭제여부
        /// <summary>
        public string IsDeleted { get; set; }
        /// <summary>
        ///
        /// <summary>
        public DateTime? MODIFIED { get; set; }
        /// <summary>
        ///
        /// <summary>
        public string MODIFIEDUSER { get; set; }

        public string CHARTDATE { get; set; }
        public string CHARTTIME { get; set; }
        public string WriteDateString { get; set; }
        public DateTime? WriteDate { get; set; }
        /// <summary>
        ///
        /// <summary>
        public DateTime? END_DATE { get; set; }
        public string REMARK { get; set; }
        /// <summary>
        ///
        /// <summary>
        public DateTime? CREATED { get; set; }
        /// <summary>
        ///
        /// <summary>
        public string CREATEDUSER { get; set; }


    }
}
