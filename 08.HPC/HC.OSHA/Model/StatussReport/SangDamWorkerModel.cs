using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.Model.StatussReport
{
    public class SangDamWorkerModel
    {
        public long WRTNO { get; set; }

        /// <summary>
        /// 근로자 상당 아이디
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 근로자 아이디
        /// </summary>
        public string WORKER_ID { get; set; }

        public long PANO { get; set; }
    }
}
