using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{
    /// <summary>
    /// 상태보고서 참고사항
    /// </summary>
    public class HIC_OSHA_MEMO : BaseDto
    { 
        public long SITEID { get; set; }
        public string  MEMO { get; set; }
    }
}
