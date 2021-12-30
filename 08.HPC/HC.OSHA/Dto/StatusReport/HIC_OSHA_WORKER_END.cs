using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{
    /// <summary>
    /// 상태보고서 참고사항
    /// </summary>
    public class HIC_OSHA_WORKER_END : BaseDto
    {
        public string ID { get; set; }
        public long SITE_ID { get; set; }
        public long PANO { get; set; }
        public string WORKER_ID { get; set; }
        public DateTime? END_DATE { get; set; }
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
