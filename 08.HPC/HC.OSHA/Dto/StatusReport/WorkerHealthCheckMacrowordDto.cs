using ComBase.Mvc;
using ComBase.Mvc.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto.StatusReport
{
    /// <summary>
    /// 상용구
    /// </summary>
    public class WorkerHealthCheckMacrowordDto :  BaseDto
    {

        public long ID { get; set; }
      
        /// <summary>
        ///제목
        /// <summary>
        [MTSNotEmpty]
        public string TITLE { get; set; }
        /// <summary>
        ///내용
        /// <summary>
        public string CONTENT { get; set; }
        public double DISPSEQ { get; set; }
        /// <summary>
        ///내용
        /// <summary>
        public string SUGESSTION { get; set; }
        /// <summary>
        /// 1: content, 2: SUGESSTION
        /// </summary>
        public string GUBUN { get; set; }

        /// <summary>
        /// 개인0 의사1 간호사2
        /// </summary>

        public string MACROTYPE { get; set; }

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
