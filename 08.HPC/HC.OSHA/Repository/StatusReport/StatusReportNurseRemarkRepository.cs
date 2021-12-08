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
    public class StatusReportNurseRemarkRepository : BaseRepository
    {
        public StatusReportNurseRemarkDto FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_REPORT_NURSE_REMARK ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND ISDELETED = 'N' ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            StatusReportNurseRemarkDto dto = ExecuteReaderSingle<StatusReportNurseRemarkDto>(parameter);
            return dto;
        }
        public List<StatusReportNurseRemarkDto> FindAllByReportNurseId(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_REPORT_NURSE_REMARK ");
            parameter.AppendSql(" WHERE REPORTNURSE_ID = :ID ");
            parameter.AppendSql("   AND ISDELETED = 'N' ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReader<StatusReportNurseRemarkDto>(parameter);
        }

        public StatusReportNurseRemarkDto Insert(StatusReportNurseRemarkDto dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_REPORT_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_REPORT_NURSE_REMARK (");
            parameter.AppendSql("    ID");
            parameter.AppendSql("  , REPORTNURSE_ID");
            parameter.AppendSql("  , PROBLEM");
            parameter.AppendSql("  , OPINION");
            parameter.AppendSql("  , ISDELETED");
            parameter.AppendSql("  , MODIFIED");
            parameter.AppendSql("  , MODIFIEDUSER");
            parameter.AppendSql("  , CREATED");
            parameter.AppendSql("  , CREATEDUSER");
            parameter.AppendSql("  , SWLICENSE) ");
            parameter.AppendSql("VALUES ( ");
            parameter.AppendSql("    :ID");
            parameter.AppendSql("  , :REPORTNURSE_ID");
            parameter.AppendSql("  , :PROBLEM");
            parameter.AppendSql("  , :OPINION");
            parameter.AppendSql("  , 'N' ");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :MODIFIEDUSER");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :CREATEDUSER");
            parameter.AppendSql("  , :SWLICENSE) ");

            parameter.Add("ID", dto.ID);
            parameter.Add("REPORTNURSE_ID", dto.REPORTNURSE_ID);
            parameter.Add("PROBLEM", dto.PROBLEM);
            parameter.Add("OPINION", dto.OPINION);

            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            return FindOne(dto.ID);
        }

        public StatusReportNurseRemarkDto Update(StatusReportNurseRemarkDto dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_NURSE_REMARK ");
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
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_NURSE_REMARK ");
            parameter.AppendSql("   SET ISDELETED = 'Y', ");
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
