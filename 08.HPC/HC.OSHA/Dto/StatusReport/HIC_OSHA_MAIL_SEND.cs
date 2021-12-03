using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{
    /// <summary>
    /// 메일전송
    /// </summary>
    public class HIC_OSHA_MAIL_SEND : BaseDto
    {
        public long ID { get; set; }
        public long SITE_ID { get; set; }
        public DateTime SEND_DATE { get; set; }
        public string SEND_USER { get; set; }
        public string SEND_TYPE { get; set; }
        public long WRTNO { get; set; }

        public DateTime? CREATED { get; set; }
        /// <summary>
        ///
        /// <summary>
        public string CREATEDUSER { get; set; }
        public string USERNAME { get; set; }
    }
}
