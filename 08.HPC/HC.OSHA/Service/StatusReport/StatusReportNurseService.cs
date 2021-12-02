using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Repository.StatusReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Service
{
    /// <summary>
    /// 상태보고서 간호사
    /// </summary>
    public class StatusReportNurseService
    {
        public StatusReportNurseRepository StatusReportNurseRepository { get; }

        public StatusReportNurseService()
        {
            this.StatusReportNurseRepository = new StatusReportNurseRepository();
        }
        public StatusReportNurseDto Save(StatusReportNurseDto dto)
        {
            if (dto.ID <= 0)
            {
               return StatusReportNurseRepository.Insert(dto);
            }
            else
            {
              return  StatusReportNurseRepository.Update(dto);
            }
        }
        public bool IsGranted(long id)
        {
            StatusReportNurseDto tmp = StatusReportNurseRepository.FindOne(id);
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
