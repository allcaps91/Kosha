using HC_Core.Macroword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_Core.Macroword
{
    public class MacrowordCkEditorBound
    {
        public MacrowordCkeditorForm MacrowordCkeditorForm { get; set; }

        public void saveMacro(string html)
        {
            MacrowordCkeditorForm.SaveMacro(html);
        }
       

        public void ReadyStatusReportNurse()
        {
          //  MacrowordCkeditorForm.LoadOpinionByNurse();
        }

       
    }
}
