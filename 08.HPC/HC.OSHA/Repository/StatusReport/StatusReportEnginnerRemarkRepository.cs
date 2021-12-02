using ComBase.Mvc;
using HC.Core.Service;
using HC.OSHA.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Repository
{
    public class StatusReportEnginnerRemarkRepository : BaseRepository
    {
        public StatusReportEngineerRemarkDto FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_REPORT_ENGINEER_REMARK    ");
            parameter.AppendSql("WHERE ID = :ID");
            parameter.AppendSql("AND ISDELETED = 'N'");
            parameter.Add("ID", id);
            StatusReportEngineerRemarkDto dto = ExecuteReaderSingle<StatusReportEngineerRemarkDto>(parameter);
            return dto;
        }
        public List<StatusReportEngineerRemarkDto> FindAllByReportEngineerId(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_REPORT_ENGINEER_REMARK    ");
            parameter.AppendSql("WHERE REPORTENGINEER_ID = :ID");
            parameter.AppendSql("AND ISDELETED = 'N'");
            parameter.Add("ID", id);
            return ExecuteReader<StatusReportEngineerRemarkDto>(parameter);
        }

        public StatusReportEngineerRemarkDto Insert(StatusReportEngineerRemarkDto dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_REPORT_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_REPORT_ENGINEER_REMARK");
            parameter.AppendSql("(");
            parameter.AppendSql("    ID");
            parameter.AppendSql("  , REPORTENGINEER_ID");
            parameter.AppendSql("  , PROBLEM");
            parameter.AppendSql("  , OPINION");
            parameter.AppendSql("  , ISDELETED");
            parameter.AppendSql("  , MODIFIED");
            parameter.AppendSql("  , MODIFIEDUSER");
            parameter.AppendSql("  , CREATED");
            parameter.AppendSql("  , CREATEDUSER");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :ID");
            parameter.AppendSql("  , :REPORTENGINEER_ID");
            parameter.AppendSql("  , :PROBLEM");
            parameter.AppendSql("  , :OPINION");
            parameter.AppendSql("  , 'N' ");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :MODIFIEDUSER");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :CREATEDUSER");
            parameter.AppendSql(") ");

            parameter.Add("ID", dto.ID);
            parameter.Add("REPORTENGINEER_ID", dto.REPORTENGINEER_ID);
            parameter.Add("PROBLEM", dto.PROBLEM);
            parameter.Add("OPINION", dto.OPINION);

            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

            return FindOne(dto.ID);
        }

        public StatusReportEngineerRemarkDto Update(StatusReportEngineerRemarkDto dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_ENGINEER_REMARK");
            parameter.AppendSql("    SET    ");
            parameter.AppendSql("       PROBLEM = :PROBLEM");
            parameter.AppendSql("      , OPINION = :OPINION");
            parameter.AppendSql("      , MODIFIED = SYSTIMESTAMP");
            parameter.AppendSql("      , MODIFIEDUSER = :MODIFIEDUSER");
            parameter.AppendSql("WHERE ID =:ID ");

            parameter.Add("ID", dto.ID);
            parameter.Add("PROBLEM", dto.PROBLEM);
            parameter.Add("OPINION", dto.OPINION);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);


            ExecuteNonQuery(parameter);

            return FindOne(dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_ENGINEER_REMARK");
            parameter.AppendSql("    SET    ");
            parameter.AppendSql("       ISDELETED = 'Y' ");
            parameter.AppendSql("      , MODIFIED = SYSTIMESTAMP");
            parameter.AppendSql("      , MODIFIEDUSER = :MODIFIEDUSER");
            parameter.AppendSql("WHERE ID =:ID ");

            parameter.Add("ID", id);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

        }
    }
}
