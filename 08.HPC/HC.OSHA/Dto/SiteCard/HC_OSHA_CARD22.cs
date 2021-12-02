using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{
    /// <summary>
    /// 사업장 약도
    /// </summary>
    public class HC_OSHA_CARD22 : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long SITE_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long ESTIMATE_ID { get; set; }

        public string YEAR { get; set; }

        public byte[] ImageData { get; set; }
    }
}
