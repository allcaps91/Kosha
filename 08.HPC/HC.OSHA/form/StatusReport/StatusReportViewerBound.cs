using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.StatusReport
{
  
    public class StatusReportViewerBound
    {
        private StatusReportViewer statusReportViewer;
    

        public StatusReportViewerBound(StatusReportViewer statusReportViewer)
        {
            this.statusReportViewer = statusReportViewer;

        }
        public void ReadyStatusReportEngineer()
        {
            statusReportViewer.LoadStatusReportEngineerDto();
        }
        public void ReadyStatusReportNurse()
        {
            statusReportViewer.LoadStatusReportNurseDto();
        }
        public void ReadyStatusReportDoctor()
        {
            statusReportViewer.LoadStatusReportDoctorDto();
        }
        
    }
}
