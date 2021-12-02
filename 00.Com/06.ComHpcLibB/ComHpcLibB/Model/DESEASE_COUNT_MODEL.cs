using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComHpcLibB.Model
{
    public class DESEASE_COUNT_MODEL
    {
        /// <summary>
        /// 일반검진 총근로자수
        /// </summary>
        public long GeneralTotalCount { get; set; }
        /// <summary>
        /// 특수검진 총근로자수
        /// </summary>
        public long SpecialTotalCount { get; set; }
        /// <summary>
        /// 질병 유소견자수
        /// </summary>
        public long D2 { get; set; }
        /// <summary>
        /// 요관찰자수
        /// </summary>
        public long C2 { get; set; }
        /// <summary>
        /// 직업성 유소견자수
        /// </summary>
        public long D1 { get; set; }
        /// <summary>
        /// 직업성 요관찰자 수
        /// </summary>
        public long C1 { get; set; }
        /// <summary>
        /// 야간 유소견자수
        /// </summary>

        public long DN { get; set; }
        /// <summary>
        /// 야간 요관잘자 수
        /// </summary>
        public long CN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? JEPDATE { get; set; }
    }

}
