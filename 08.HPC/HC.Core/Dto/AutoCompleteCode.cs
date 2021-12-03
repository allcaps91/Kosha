using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_Core.Dto
{
    public class AutoCompleteCode : BaseDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
    }
}
