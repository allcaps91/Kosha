using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.Model.StatussReport
{
    /// <summary>
    /// 건강상담 직업병 건수
    /// </summary>
    public class SangDamSpecialCountModel
    {
        /// <summary>
        /// 소음 D1
        /// </summary>
        public long TONGBUN1_D { get; set; }
        /// <summary>
        /// 소음 C1
        /// </summary>
        public long TONGBUN1_C { get; set; }
        /// <summary>
        /// 분진
        /// </summary>
        public long TONGBUN2_D { get; set; }
        public long TONGBUN2_C { get; set; }

        /// <summary>
        /// 화학물질
        /// </summary>
        public long TONGBUN6_D { get; set; }
        public long TONGBUN6_C { get; set; }

        /// <summary>
        /// 금속류
        /// </summary>
        public long TONGBUN7_D { get; set; }
        public long TONGBUN7_C { get; set; }



        /// <summary>
        /// 기타
        /// </summary>
        public long TONGBUNETC_D { get; set; }
        public long TONGBUNETC_C { get; set; }






    }
}
