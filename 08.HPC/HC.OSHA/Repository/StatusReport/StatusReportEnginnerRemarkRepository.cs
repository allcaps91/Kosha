using ComBase;
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
            parameter.AppendSql("SELECT * FROM HIC_OSHA_REPORT_ENGINEER_REMARK ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND ISDELETED = 'N' ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            StatusReportEngineerRemarkDto dto = ExecuteReaderSingle<StatusReportEngineerRemarkDto>(parameter);
            return dto;
        }
        public List<StatusReportEngineerRemarkDto> FindAllByReportEngineerId(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_REPORT_ENGINEER_REMARK ");
            parameter.AppendSql(" WHERE REPORTENGINEER_ID = :ID ");
            parameter.AppendSql("   AND ISDELETED = 'N' ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReader<StatusReportEngineerRemarkDto>(parameter);
        }

        public StatusReportEngineerRemarkDto Insert(StatusReportEngineerRemarkDto dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_REPORT_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_REPORT_ENGINEER_REMARK ( ");
            parameter.AppendSql("    ID,REPORTENGINEER_ID,PROBLEM,OPINION,ISDELETED,MODIFIED, ");
            parameter.AppendSql("    MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE ) ");
            parameter.AppendSql("VALUES (:ID,:REPORTENGINEER_ID,:PROBLEM,:OPINION,'N', ");
            parameter.AppendSql("    SYSTIMESTAMP,:MODIFIEDUSER,SYSTIMESTAMP,:CREATEDUSER,:SWLICENSE) ");
            parameter.Add("ID", dto.ID);
            parameter.Add("REPORTENGINEER_ID", dto.REPORTENGINEER_ID);
            parameter.Add("PROBLEM", dto.PROBLEM);
            parameter.Add("OPINION", dto.OPINION);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            return FindOne(dto.ID);
        }

        public StatusReportEngineerRemarkDto Update(StatusReportEngineerRemarkDto dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_ENGINEER_REMARK ");
            parameter.AppendSql("   SET PROBLEM = :PROBLEM, ");
            parameter.AppendSql("       OPINION = :OPINION, ");
            parameter.AppendSql("       MODIFIED = SYSTIMESTAMP, ");
            parameter.AppendSql("       MODIFIEDUSER = :MODIFIEDUSER ");
            parameter.AppendSql(" WHERE ID =:ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", dto.ID);
            parameter.Add("PROBLEM", dto.PROBLEM);
            parameter.Add("OPINION", dto.OPINION);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            return FindOne(dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_ENGINEER_REMARK ");
            parameter.AppendSql("   SET ISDELETED = 'Y' ");
            parameter.AppendSql("       MODIFIED = SYSTIMESTAMP, ");
            parameter.AppendSql("       MODIFIEDUSER = :MODIFIEDUSER ");
            parameter.AppendSql(" WHERE ID =:ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
        }
    }
}
