using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComHpcLibB.Model
{
    public class HC_OSHA_SITE : BaseDto
    {
        /// <summary>
        ///아이디(사업장 코드 LTDCODE)
        /// <summary>
        public long ID { get; set; }

        /// <summary>
        ///사용여부
        /// <summary>
        public string ISACTIVE { get; set; }
        /// <summary>
        ///마지막업무명
        /// <summary>
        public string TASKNAME { get; set; }
        /// <summary>
        ///마지막 수정일시
        /// <summary>
        public DateTime? MODIFIED { get; set; }
        /// <summary>
        ///마지막수정자
        /// <summary>
        public string MODIFIEDUSER { get; set; }

        /// <summary>
        /// 하청을 가지고 있는지 여부
        /// </summary>
        public string HASCHILD { get; set; }

        //원청사업장
        public long PARENTSITE_ID { get; set; }

        /// <summary>
        /// 청구시 원청으로 표시할지 여부
        /// </summary>
        public string ISPARENTCHARGE { get; set; }
        /// <summary>
        /// 분기청구 여부
        /// </summary>
        public string ISQUARTERCHARGE { get; set; }

    }
}
