using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using HC_Core.Dto;
using HC.Core.Service;
using HC.OSHA.Dto.StatusReport;
using HC_Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HC.OSHA.Repository.StatusReport
{
    public class HealthChecMacroRepository : BaseRepository
    {
        public WorkerHealthCheckMacrowordDto FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT *   ");
            parameter.AppendSql("  FROM HIC_OSHA_HEALTHCHECK_MACROWORD  ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<WorkerHealthCheckMacrowordDto>(parameter);

        }

        public List<WorkerHealthCheckMacrowordDto> FindAll(string userId, string gubun, string macroType)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*    ");
            parameter.AppendSql("  FROM HIC_OSHA_HEALTHCHECK_MACROWORD A");
            parameter.AppendSql(" WHERE 1=1 ");
            parameter.AppendSql(" AND  A.GUBUN = :gubun ");
            parameter.AppendSql(" AND  A.MACROTYPE = :macroType ");
            parameter.AppendSql(" AND  A.SWLICENSE = :SWLICENSE ");
            if (macroType == "0")
            {
                parameter.AppendSql(" AND  A.CREATEDUSER = :userId ");
            }
            parameter.AppendSql(" ORDER BY DISPSEQ ");
            if (macroType == "0")
            {
                parameter.Add("userId", userId);
            }
        
            parameter.Add("gubun", gubun);
            parameter.Add("macroType", macroType);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader <WorkerHealthCheckMacrowordDto>(parameter);

        }

        public WorkerHealthCheckMacrowordDto Update(WorkerHealthCheckMacrowordDto item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_HEALTHCHECK_MACROWORD");
            parameter.AppendSql("    SET ");
            parameter.AppendSql("       TITLE = :TITLE");
            parameter.AppendSql("      , CONTENT = :CONTENT");
            parameter.AppendSql("      , SUGESSTION = :SUGESSTION");
            parameter.AppendSql("      , DISPSEQ = :DISPSEQ");            
            parameter.AppendSql("      , MODIFIED = SYSTIMESTAMP");
            parameter.AppendSql("      , MODIFIEDUSER = :MODIFIEDUSER");
            parameter.AppendSql("     WHERE ID =:ID");
            parameter.AppendSql("       AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", item.ID);
            parameter.Add("SUGESSTION", item.SUGESSTION);
            parameter.Add("TITLE", item.TITLE);
            parameter.Add("CONTENT", item.CONTENT);
            parameter.Add("DISPSEQ", item.DISPSEQ);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            if (item.MODIFIEDUSER.IsNullOrEmpty())
            {
                parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            }
            else
            {
                parameter.Add("MODIFIEDUSER", item.MODIFIEDUSER);
            }

            ExecuteNonQuery(parameter);
            
            DataSyncService.Instance.Update("HIC_OSHA_HEALTHCHECK_MACROWORD", item.ID);

            return FindOne(item.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_HEALTHCHECK_MACROWORD   ");
            parameter.AppendSql("WHERE ID = :ID             ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_HEALTHCHECK_MACROWORD", id);
        }
        public WorkerHealthCheckMacrowordDto Insert(WorkerHealthCheckMacrowordDto item)
        {
            item.ID = GetSequenceNextVal("HC_MACROWORD_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_HEALTHCHECK_MACROWORD");
            parameter.AppendSql("(");
            parameter.AppendSql("    ID");
            parameter.AppendSql("  , TITLE");
            parameter.AppendSql("  , CONTENT");
            parameter.AppendSql("  , SUGESSTION");
            parameter.AppendSql("  , DISPSEQ");
            parameter.AppendSql("  , GUBUN");
            parameter.AppendSql("  , MACROTYPE");
            parameter.AppendSql("  , MODIFIED");
            parameter.AppendSql("  , MODIFIEDUSER");
            parameter.AppendSql("  , CREATED");
            parameter.AppendSql("  , CREATEDUSER ");
            parameter.AppendSql("  , SWLICENSE ");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :ID");
            parameter.AppendSql("  , :TITLE");
            parameter.AppendSql("  , :CONTENT");
            parameter.AppendSql("  , :SUGESSTION");
            parameter.AppendSql("  , :DISPSEQ");
            parameter.AppendSql("  , :GUBUN");
            parameter.AppendSql("  , :MACROTYPE");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :MODIFIEDUSER");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :CREATEDUSER ");
            parameter.AppendSql("  , :SWLICENSE ");
            parameter.AppendSql(") ");

            parameter.Add("ID", item.ID);
            parameter.Add("SUGESSTION", item.SUGESSTION);
            parameter.Add("TITLE", item.TITLE);
            parameter.Add("CONTENT", item.CONTENT);
            parameter.Add("DISPSEQ", item.DISPSEQ);
            parameter.Add("GUBUN", item.GUBUN);
            parameter.Add("MACROTYPE", item.MACROTYPE);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            if (item.CREATEDUSER.IsNullOrEmpty())
            {
                parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
                parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            }
            else
            {
                parameter.Add("MODIFIEDUSER", item.CREATEDUSER);
                parameter.Add("CREATEDUSER", item.CREATEDUSER);
            }

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_HEALTHCHECK_MACROWORD", item.ID);

            return FindOne(item.ID);
        }
    }
}
