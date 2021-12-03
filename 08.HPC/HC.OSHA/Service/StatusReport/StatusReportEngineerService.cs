using HC.OSHA.Dto;
using HC.OSHA.Repository;
using HC.OSHA.Repository.StatusReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Service
{
    /// <summary>
    /// 산업위생기사 상태보고서
    /// </summary>
    public class StatusReportEngineerService
    {
        public StatusReportEngineerRepository StatusReportEngineerRepository { get; }

        public StatusReportEngineerService()
        {
            this.StatusReportEngineerRepository = new StatusReportEngineerRepository();
        }
        public StatusReportEngineerDto Save(StatusReportEngineerDto dto)
        {
            if (dto.ID <= 0)
            {
               return  StatusReportEngineerRepository.Insert(dto);
            }
            else
            {
                return StatusReportEngineerRepository.Update(dto);
            }
        }

      

    }
}
