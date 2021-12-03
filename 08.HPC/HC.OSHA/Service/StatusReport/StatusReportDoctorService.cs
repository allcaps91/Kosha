using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Repository.StatusReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Service.StatusReport
{
    public class StatusReportDoctorService
    {
        public StatusReportDoctorRepository StatusReportDoctorRepository { get; }
  
        public StatusReportDoctorService()
        {
            this.StatusReportDoctorRepository = new StatusReportDoctorRepository();
        }
        public StatusReportDoctorDto Save(StatusReportDoctorDto dto)
        {
            StatusReportDoctorDto saved = null;
            if (dto.ID <= 0)
            {
                saved = StatusReportDoctorRepository.Insert(dto);
            }
            else
            {
                saved = StatusReportDoctorRepository.Update(dto);
            }
            return saved;
        }

        public void UpdateOpinion(StatusReportDoctorDto dto)
        {
            StatusReportDoctorRepository.UpdateOpinion(dto); 
        }

        public bool IsGranted(long id)
        {
            StatusReportDoctorDto tmp = StatusReportDoctorRepository.FindOne(id);
            if (tmp.MODIFIEDUSER == CommonService.Instance.Session.UserId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
