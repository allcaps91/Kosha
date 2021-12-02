using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComHpcLibB.Model
{
    /// <summary>
    /// 사업장 인터페이스
    /// </summary>
    public interface ISiteModel
    {   /// <summary>
        /// 사업자아이디
        /// </summary>
        long ID { get; set; }


        /// <summary>
        /// 사업자명
        /// </summary>
        string NAME { get; set; }
    }
}
