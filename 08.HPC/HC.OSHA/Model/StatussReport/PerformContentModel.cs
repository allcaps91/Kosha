using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.Model.StatussReport
{
    public class PerformContentModel : BaseDto
    {
        public string CheckboxText { get; set; }
        public string IsChecked { get; set; }
    }
}
