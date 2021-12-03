using ComBase.Mvc;
using ComBase.Mvc.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{
    /// <summary>
    /// 특수검진결과표 빌드
    /// </summary>
    public class HIC_OSHA_SPECIAL_RESULT : BaseDto
    {
        public string YEAR { get; set; }
        public long SITE_ID { get; set; }
        public long D2COUNT { get; set; }
        public long C2COUNT { get; set; }
        public long D1COUNT { get; set; }
        public long C1COUNT { get; set; }
        public long DNCOUNT { get; set; }
        public long CNCOUNT { get; set; }
        /// <summary>
        /// 총근로자수
        /// </summary>
        public long TOTALCOUNT { get; set; }
        public DateTime CREATED { get; set; }

        /// <summary>
        /// 최초접수일자
        /// </summary>
        public DateTime? JEPDATE { get; set; }
    }
}

