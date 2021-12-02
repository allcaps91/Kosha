using ComBase.Mvc;
using ComBase.Mvc.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{
    public class OSHA_VISIT_PRICE : BaseDto
    {

        public OSHA_VISIT_PRICE()
        {
            this.ISPRECHARGE = "N";
        }
        /// <summary>
        /// 
        /// 방문 수수료 PK
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        ///방문등록아이디
        /// <summary>
        public long VISIT_ID { get; set; }
        /// <summary>
        ///인원수
        /// <summary>
     
        public long WORKERCOUNT { get; set; }
        /// <summary>
        ///단가
        /// <summary>
       
        public long UNITPRICE { get; set; }
        /// <summary>
        ///계약금액
        /// <summary>
       
        public long UNITTOTALPRICE { get; set; }
        /// <summary>
        ///발생금액
        /// <summary>
        public long TOTALPRICE { get; set; }

        /// <summary>
        /// 청구금액
        /// </summary>
        public long CHARGEPRICE { get; set; }

        /// <summary>
        /// 선청구여부
        /// </summary>
        public string ISPRECHARGE { get; set; }
        /// <summary>
        ///
        /// <summary>
        public DateTime? CREATED { get; set; }
        /// <summary>
        ///
        /// <summary>
        public string CREATEDUSER { get; set; }
        /// <summary>
        ///삭제여부
        /// <summary>
        public string ISDELETED { get; set; }

    }
}
