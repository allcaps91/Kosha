using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Model
{
    /// <summary>
    /// 방문수수료 내역 
    /// </summary>
    public class OshaVisitPriceModel
    {
        public long ID { get; set; }
        /// <summary>
        ///방문일시
        /// <summary>
        public DateTime? VISITDATETIME { get; set; }

        /// <summary>
        /// 방문자
        /// </summary>
        public string VISITUSERNAME { get; set; }

        public long WORKERCOUNT { get; set; }
        /// <summary>
        ///단가
        /// <summary>
        public double UNITPRICE { get; set; }
        /// <summary>
        ///단가금액
        /// <summary>
        public long UNITTOTALPRICE { get; set; }
        /// <summary>
        ///계약금액
        /// <summary>
        public long TOTALPRICE { get; set; }

        /// <summary>
        /// 선청구여부
        /// </summary>
        public string ISPRECHARGE { get; set; }

        /// <summary>
        /// 수수료발생일
        /// </summary>
        public string CREATED { get; set; }
        /// <summary>
        /// 수수료삭제여부
        /// </summary>
        public string ISDELETED { get; set; }
    }
}
