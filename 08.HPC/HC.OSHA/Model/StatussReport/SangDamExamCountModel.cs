using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.Model.StatussReport
{
    /// <summary>
    /// 건강상담 혈압측정 간이검사 건수
    /// </summary>
    public class SangDamExamCountModel
    {
        public string Worker_ID { get; set; }
        
        /// <summary>
        /// 혈압
        /// </summary>
        public long BpCount { get; set; }
        /// <summary>
        /// 혈당
        /// </summary>
        public long BstCount { get; set; }
        /// <summary>
        /// 단백뇨
        /// </summary>
        public long DanCount { get; set; }
        /// <summary>
        /// 체지방
        /// </summary>
        public long BMICount { get; set; }
        
    }
}
