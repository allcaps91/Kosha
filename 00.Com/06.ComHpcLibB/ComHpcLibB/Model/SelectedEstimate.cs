using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComHpcLibB.Model
{
    public class SelectedEstimate : IEstimateModel
    {
        /// <summary>
        /// 견적 아이디
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 견적일자
        /// </summary>
		public string ESTIMATEDATE { get; set; }

        /// <summary>
        /// 계약일
        /// </summary>
        public string CONTRACTDATE { get; set; }

        /// <summary>
        /// 계약시작일
        /// </summary>
        public string CONTRACTSTARTDATE { get; set; }

        /// <summary>
        /// 계약종료일
        /// </summary>
        public  string CONTRACTENDDATE { get; set; }
    }
}
