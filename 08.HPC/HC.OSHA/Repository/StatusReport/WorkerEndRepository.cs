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
        internal List<HIC_OSHA_WORKER_END> FindByWorker(HIC_OSHA_WORKER_END worker)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT *                       ");
            parameter.AppendSql("  FROM HIC_OSHA_WORKER_END A   ");
            parameter.AppendSql(" WHERE A.SITE_ID   = :SITE_ID  ");
            parameter.AppendSql("   AND A.WORKER_ID = :WORKER_ID");

            parameter.Add("SITE_ID", worker.SITE_ID);
            parameter.Add("WORKER_ID", worker.WORKER_ID);

            return ExecuteReader<HIC_OSHA_WORKER_END>(parameter);
        }

        internal int Insert(HIC_OSHA_WORKER_END item)
        {
            if(item.ID == 0)
            {
                item.ID = GetSequenceNextVal("HIC_OSHA_WORKER_END_SEQ");
            }

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_WORKER_END (");
            parameter.AppendSql("    ID                           ");
            parameter.AppendSql("  , SITE_ID                      ");
            parameter.AppendSql("  , PANO                         ");
            parameter.AppendSql("  , WORKER_ID                    ");
            parameter.AppendSql("  , END_DATE                     ");
            parameter.AppendSql("  , CREATED                      ");
            parameter.AppendSql("  , CREATEDUSER                  ");
            parameter.AppendSql(") VALUES (                       ");
            parameter.AppendSql("    :ID                          ");
            parameter.AppendSql("  , :SITE_ID                     ");
            parameter.AppendSql("  , :PANO                        ");
            parameter.AppendSql("  , :WORKER_ID                   ");
            parameter.AppendSql("  , :END_DATE                    ");
            parameter.AppendSql("  , SYSTIMESTAMP                 ");
            parameter.AppendSql("  , :CREATEDUSER                 ");
            parameter.AppendSql(")                                ");

            parameter.Add("ID", item.ID);
            parameter.Add("SITE_ID", item.SITE_ID);
            parameter.Add("PANO", item.PANO);
            parameter.Add("WORKER_ID", item.WORKER_ID);
            parameter.Add("END_DATE", item.END_DATE);
            parameter.Add("CREATEDUSER", item.CREATEDUSER);

            DataSyncService.Instance.Insert("HIC_OSHA_WORKER_END", item.ID);

            return ExecuteNonQuery(parameter);
        }

        internal int Update(HIC_OSHA_WORKER_END item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_WORKER_END        ");
            parameter.AppendSql("   SET END_DATE     = :END_DATE   ");
            parameter.AppendSql("     , CREATED      = SYSTIMESTAMP");
            parameter.AppendSql("     , CREATEDUSER  = :CREATEDUSER");
            parameter.AppendSql(" WHERE ID = :ID                   ");

            parameter.Add("ID", item.ID);
            parameter.Add("END_DATE", item.END_DATE);
            parameter.Add("CREATEDUSER", item.CREATEDUSER);

            DataSyncService.Instance.Update("HIC_OSHA_WORKER_END", item.ID);

            return ExecuteNonQuery(parameter);
        }

        internal HIC_OSHA_WORKER_END FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT *                       ");
            parameter.AppendSql("  FROM HIC_OSHA_WORKER_END A   ");
            parameter.AppendSql(" WHERE A.ID        = :ID       ");

            parameter.Add("ID", id);

            return ExecuteReaderSingle<HIC_OSHA_WORKER_END>(parameter);
        }

        internal HIC_OSHA_WORKER_END InsertAndSelect(HIC_OSHA_WORKER_END saved)
        {
            saved.ID = GetSequenceNextVal("HIC_OSHA_WORKER_END_SEQ");
            Insert(saved);

            return FindOne(saved.ID);
        }

        internal int Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE                         ");
            parameter.AppendSql("  FROM HIC_OSHA_WORKER_END A   ");
            parameter.AppendSql(" WHERE A.ID        = :ID       ");

            parameter.Add("ID", id);
            DataSyncService.Instance.Delete("HIC_OSHA_WORKER_END", id);

            return ExecuteNonQuery(parameter);
        }
    }
}
