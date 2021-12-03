﻿using HC.Core.Dto;
using HC.Core.Repository;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Repository.StatusReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Service.StatusReport
{
    public class WorkerHealthCheckMacroService
    {
        public HealthChecMacroRepository healthChecMacroRepository  { get; }

        public WorkerHealthCheckMacroService()
        {
            this.healthChecMacroRepository = new HealthChecMacroRepository();
        }

        public WorkerHealthCheckMacrowordDto Save(WorkerHealthCheckMacrowordDto dto)
        {
            if(dto.SUGESSTION == null)
            {
                dto.SUGESSTION = "";
            }
            if(dto.CONTENT == null)
            {
                dto.CONTENT = "";
            }
            if (dto.ID > 0)
            {
                return this.healthChecMacroRepository.Update(dto);
            }
            else
            {
                return this.healthChecMacroRepository.Insert(dto);
            }
        }

    
       
    }
}
