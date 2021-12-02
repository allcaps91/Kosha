using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HC.Core.Model
{
    public class AutoCompleteFeeModel
    {  
        /// <summary>
        /// 수수료 항목번호
        /// </summary>
        public long M_ID { get; set; }
        /// <summary>
        /// 수수료 적용년도
        /// </summary>
        public string M_YEAR { get; set; }

        /// <summary>
        /// 측정방법
        /// </summary>
        public string M_NAME { get; set; }
        /// <summary>
        /// 산정내역명
        /// </summary>
        public string M_SANNAME { get; set; }
        /// <summary>
        /// 단가
        /// </summary>
        public long M_DANGA { get; set; }
    }
}
