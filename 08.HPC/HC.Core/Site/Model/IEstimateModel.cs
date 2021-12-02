using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Core.Site.Model
{
    public interface IEstimateModel
    {
        /// <summary>
        /// 견적 아이디
        /// </summary>
        long ID { get; set; }

        /// <summary>
        /// 견적일자
        /// </summary>
		string ESTIMATEDATE { get; set; }
    }
}
