using ComBase.Mvc;
using ComBase.Mvc.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{
    /// <summary>
    /// 사업장 기본 단가 이력
    /// </summary>
    public class OSHA_PRICE : BaseDto
    {
        /// <summary>
        ///아이디
        /// <summary>
        public long ID { get; set; }
        /// <summary>
        ///견적아이디
        /// <summary>
        public long ESTIMATE_ID { get; set; }


        public long SITE_ID { get; set; }
        public string SITE_NAME { get; set; }
        public string CONTRACTDATE { get; set; }
        public string CONTRACTSTARTDATE { get; set; }
        public string CONTRACTENDDATE { get; set; }

        /// <summary>
        ///총근로자수
        /// <summary>
        [MTSZero]
        public long WORKERTOTALCOUNT { get; set; }
        /// <summary>
        ///단가
        /// <summary>
        [MTSZero]
        public long UNITPRICE { get; set; }

        /// <summary>
        ///청구금액
        /// <summary>
     
        public long UNITTOTALPRICE { get; set; }

        /// <summary>
        ///계약금액
        /// <summary>
        [MTSZero]
        public long TOTALPRICE { get; set; }
        /// <summary>
        ///정액계약여부
        /// <summary>
        public string ISFIX { get; set; }
        /// <summary>
        ///계산서인원단가표시여부
        /// <summary>
        public string ISBILL { get; set; }

        /// <summary>
        /// 청구시 원청으로 표시할지 여부
        /// </summary>
        public string ISPARENTCHARGE { get; set; }
        /// <summary>
        /// 분기청구 여부
        /// </summary>
        public string ISQUARTERCHARGE { get; set; }
        /// <summary>
        ///삭제여부
        /// <summary>
        public string ISDELETED { get; set; }

        /// <summary>
        /// 국고여부
        /// </summary>
        public string GBGUKGO { get; set; }
        /// <summary>
        ///
        /// <summary>
        public DateTime? MODIFIED { get; set; }
        /// <summary>
        ///
        /// <summary>
        public string MODIFIEDUSER { get; set; }
        /// <summary>
        ///
        /// <summary>
        public DateTime? CREATED { get; set; }
        /// <summary>
        ///
        /// <summary>
        public string CREATEDUSER { get; set; }

    }
}
