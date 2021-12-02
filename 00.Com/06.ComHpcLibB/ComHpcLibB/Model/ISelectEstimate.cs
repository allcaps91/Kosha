
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComHpcLibB.Model
{
    /// <summary>
    /// 사업장 견적 선택 이벤트를 받는 폼은 구현한다
    /// </summary>
    public interface ISelectEstimate
    {
        
        /// <summary>
        /// 사업장 견적 선택
        /// </summary>
        /// <param name="estimateModel"></param>
        void Select(IEstimateModel estimateModel);
        
    }
}
