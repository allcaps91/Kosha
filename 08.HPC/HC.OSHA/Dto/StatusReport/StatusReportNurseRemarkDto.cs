using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{ /// <summary>
  /// 상태보고서 간호사 문제점 및 개선
  /// </summary>
    public class StatusReportNurseRemarkDto : BaseDto
    {
        /// <summary>
        ///아이디
        /// <summary>
        public long ID { get; set; }
        /// <summary>
        ///상태보고서 간호사  FK
        /// <summary>
        public long REPORTNURSE_ID { get; set; }
        /// <summary>
        ///현실태및문제점
        /// <summary>
        public string PROBLEM { get; set; }
        /// <summary>
        ///개선의견
        /// <summary>
        public string OPINION { get; set; }
        /// <summary>
        ///
        /// <summary>
        public string ISDELETED { get; set; }
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
