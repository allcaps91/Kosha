using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC_Core.Service;
using System;
using System.Collections.Generic;

namespace HC.OSHA.Repository.StatusReport
{
    public class WorkerEndRepository : BaseRepository
    {
        internal int Update(HIC_OSHA_WORKER_END item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_SITE_WORKER ");
            parameter.AppendSql("   SET END_DATE     = :END_DATE, ");
            parameter.AppendSql("       MODIFIED      = SYSTIMESTAMP, ");
            parameter.AppendSql("       MODIFIEDUSER  = :CREATEDUSER ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", item.ID);
            parameter.Add("END_DATE", item.END_DATE);
            parameter.Add("CREATEDUSER", item.CREATEDUSER);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteNonQuery(parameter);
        }
    }
}
