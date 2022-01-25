using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.form.Charge
{
    public class ChargeEmailModel
    {
        public string PdfFileName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AttachFileName { get; set; }
    }
}
