using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{
    /// <summary>
    /// 상담관리 환자 메모
    /// </summary>
    public class HIC_OSHA_PATIENT_MEMO : BaseDto
    {
        public long ID { get; set; }
        public string WORKER_ID { get; set; }
        public string MEMO { get; set; }
        public DateTime? CREATED { get; set; }
        /// <summary>
        ///
        /// <summary>
        public string CREATEDUSER { get; set; }

        public string WriteDate { get; set; }
        public string WriteName { get; set; }
    }
}
