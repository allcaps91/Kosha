using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Core.Site.Model
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
    }
}
