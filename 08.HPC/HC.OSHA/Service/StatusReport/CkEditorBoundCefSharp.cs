using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Service.StatusReport
{
    public class CkEditorBoundCefSharp
    {
        public string Opinion { get; set; }
        public CkEditor CkEditor { get; set; }


        public void SaveCard19(string content)
        {
            CkEditor.SaveCard19(content);
        }


        public void SaveEngineerOpinion(string content)
        {
            Opinion = content;
            CkEditor.SaveEngineerOpinion(content);
        }
        public void SaveNurseOpinion(string content)
        {
            Opinion = content;
            CkEditor.SaveNurseOpinion(content);
        }

        public void ReadyStatusReportNurse()
        {
            CkEditor.LoadOpinionByNurse();
        }

        public void ReadyStatusReportEngineer()
        {
            CkEditor.LoadOpinionByEngineer();
        }
    }
}
