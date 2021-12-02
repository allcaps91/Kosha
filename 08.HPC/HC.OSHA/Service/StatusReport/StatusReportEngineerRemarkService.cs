using HC.OSHA.Dto;
using HC.OSHA.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Service.StatusReport
{
    public class StatusReportEngineerRemarkService
    {
        public StatusReportEnginnerRemarkRepository repository { get; }

        public StatusReportEngineerRemarkService()
        {
            repository = new StatusReportEnginnerRemarkRepository();
        }
        public StatusReportEngineerRemarkDto Save(StatusReportEngineerRemarkDto dto)
        {
            if(dto.ID == 0)
            {
                
                return repository.Insert(dto);

            }
            else
            {
                return repository.Update(dto);
            }
        }

        
    }
}
