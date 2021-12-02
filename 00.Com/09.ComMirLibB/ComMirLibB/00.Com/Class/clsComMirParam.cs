using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComMirLibB.Com
{
    /// <summary>
    /// Class Name      : ComMirLibB.Com
    /// File Name       : clsComMirParam.cs
    /// Description     : 청구 공통 함수들 모음
    /// Author          : 전종윤
    /// Create Date     : 2017-12-05
    /// Update History  : 
    /// </summary>
    /// <history>       
    /// </history>
    /// <seealso cref= "신규" />
    public class clsComMirParam
    {       
        /// <summary> 콤버박스에서 All을 삽입여부</summary>
        public enum enmComParamComboType
        {
            /// <summary> ****.전체 형태로 삽입</summary>
            ALL,
            /// <summary> Null.형태로 삽입</summary>
            NULL,
            /// <summary> 데이터만 삽입</summary>
            None
        }
    }
}
