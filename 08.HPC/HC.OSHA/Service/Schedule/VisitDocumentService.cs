using HC_OSHA.Repository.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.Service.Schedule
{
    public class VisitDocumentService
    {
        public VIsitDocumentRepository VisitDocumentRepository { get; }

        public VisitDocumentService()
        {
            this.VisitDocumentRepository = new VIsitDocumentRepository();
        }


    }
}
