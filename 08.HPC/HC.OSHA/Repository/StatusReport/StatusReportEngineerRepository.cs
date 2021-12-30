using ComBase;
using ComBase.Mvc;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC_Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Repository
{
    public class StatusReportEngineerRepository : BaseRepository
    {
        public StatusReportEngineerDto FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_REPORT_ENGINEER ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            StatusReportEngineerDto dto = ExecuteReaderSingle<StatusReportEngineerDto>(parameter);
            return dto;
        }
        public void UpdateSign(long id, string base64Image)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_ENGINEER ");
            parameter.AppendSql("   SET SITEMANAGERSIGN = :SITEMANAGERSIGN ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SITEMANAGERSIGN", base64Image);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Update("HIC_OSHA_REPORT_ENGINEER", id);
        }
        public StatusReportEngineerDto FindLast(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_REPORT_ENGINEER ");
            parameter.AppendSql(" WHERE SITE_ID = :SITE_ID ");
            parameter.AppendSql("   AND VISITDATE = (SELECT MAX(VISITDATE) FROM HIC_OSHA_REPORT_ENGINEER ");
            parameter.AppendSql("                     WHERE SITE_ID = :SITE_ID AND ISDELETED = 'N') ");
            parameter.AppendSql("   AND ISDELETED = 'N' ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            StatusReportEngineerDto dto = ExecuteReaderSingle<StatusReportEngineerDto>(parameter);
            return dto;
        }

        public List<VisitDateModel> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.ID, To_DATE(A.VISITDATE, 'YYYY-MM-DD') AS VISITDATE, B.NAME,");
            parameter.AppendSql("       SUBSTR(A.VISITDATE,0,4)  AS VISITYEAR ");
            parameter.AppendSql("  FROM HIC_OSHA_REPORT_ENGINEER A ");
            parameter.AppendSql("       INNER JOIN HIC_USERS B ");
            parameter.AppendSql("             ON A.MODIFIEDUSER = B.USERID ");
            parameter.AppendSql(" WHERE A.SITE_ID = :SITE_ID ");
            parameter.AppendSql("   AND A.ISDELETED = 'N' ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" ORDER BY A.VISITDATE DESC ");
            parameter.Add("SITE_ID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<VisitDateModel>(parameter);
        }
        public List<VisitDateModel> FindVisitDate(long siteId, string year)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT VISITDATE, ID ");
            parameter.AppendSql("  FROM HIC_OSHA_REPORT_ENGINEER ");
            parameter.AppendSql(" WHERE SITE_ID = :SITE_ID ");
            parameter.AppendSql("   AND ISDELETED = 'N' ");
            parameter.AppendSql("   AND SUBSTR(VISITDATE,0,4) = :YEAR ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" ORDER BY VISITDATE DESC ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("YEAR", year);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<VisitDateModel>(parameter);
        }

        public List<VisitDateModel> FindVisitDate(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT VISITDATE, ID FROM HIC_OSHA_REPORT_NURSE ");
            parameter.AppendSql(" WHERE SITE_ID = :SITE_ID ");
            parameter.AppendSql("   AND ISDELETED = 'N' ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("SITE_ID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<VisitDateModel>(parameter);
        }

        public void UpdateOptioin(long id, String html)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_ENGINEER ");
            parameter.AppendSql("   SET OPINION = :OPINION, ");
            parameter.AppendSql("       MODIFIED = SYSTIMESTAMP, ");
            parameter.AppendSql("       MODIFIEDUSER = :MODIFIEDUSER ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("OPINION", html);
            parameter.Add("ID", id);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId); ;
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Update("HIC_OSHA_REPORT_ENGINEER", id);
        }
        public StatusReportEngineerDto Update(StatusReportEngineerDto dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_ENGINEER ");
            parameter.AppendSql("   SET VISITDATE = :VISITDATE, ");            
            parameter.AppendSql("       VISITRESERVEDATE = :VISITRESERVEDATE, ");
            parameter.AppendSql("       WORKERCOUNT = :WORKERCOUNT, ");
            parameter.AppendSql("       WEMDATE = :WEMDATE, ");
            parameter.AppendSql("       WEMDATE2 = :WEMDATE2, ");
            parameter.AppendSql("       WEMDATEREMARK = :WEMDATEREMARK, ");
            parameter.AppendSql("       WEMHARMFULFACTORS = :WEMHARMFULFACTORS, ");
            parameter.AppendSql("       WEMEXPORSURE = :WEMEXPORSURE. ");
            parameter.AppendSql("       WEMEXPORSURE1 = :WEMEXPORSURE1, ");
            parameter.AppendSql("       WEMEXPORSUREREMARK = :WEMEXPORSUREREMARK, ");
            parameter.AppendSql("       WORKCONTENT = :WORKCONTENT, ");
            parameter.AppendSql("       OSHADATE = :OSHADATE, ");
            parameter.AppendSql("       OSHACONTENT = :OSHACONTENT, ");
            parameter.AppendSql("       EDUTARGET = :EDUTARGET, ");
            parameter.AppendSql("       EDUPERSON = :EDUPERSON, ");
            parameter.AppendSql("       EDUAN = :EDUAN, ");
            parameter.AppendSql("       EDUTITLE = :EDUTITLE, ");
            parameter.AppendSql("       EDUTYPEJSON = :EDUTYPEJSON, ");
            parameter.AppendSql("       EDUMETHODJSON = :EDUMETHODJSON, ");
            parameter.AppendSql("       ENVCHECKJSON1 = :ENVCHECKJSON1, ");
            parameter.AppendSql("       ENVCHECKJSON2 = :ENVCHECKJSON2, ");
            parameter.AppendSql("       ENVCHECKJSON3 = :ENVCHECKJSON3, ");
            parameter.AppendSql("       SITEMANAGERNAME = :SITEMANAGERNAME, ");
            parameter.AppendSql("       SITEMANAGERGRADE = :SITEMANAGERGRADE, ");
            parameter.AppendSql("       ENGINEERNAME = :ENGINEERNAME, ");
            parameter.AppendSql("       ISDELETED = :ISDELETED, ");
            parameter.AppendSql("       MODIFIED = SYSTIMESTAMP, ");
            parameter.AppendSql("       MODIFIEDUSER = :MODIFIEDUSER, ");
            parameter.AppendSql("       SITENAME = :SITENAME, ");
            parameter.AppendSql("       SITEOWENER = :SITEOWENER, ");
            parameter.AppendSql("       SITETEL = :SITETEL, ");
            parameter.AppendSql("       SITEADDRESS = :SITEADDRESS ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", dto.ID);
            parameter.Add("VISITDATE", dto.VISITDATE);
            parameter.Add("VISITRESERVEDATE", dto.VISITRESERVEDATE);
            parameter.Add("WORKERCOUNT", dto.WORKERCOUNT);
            parameter.Add("WEMDATE", dto.WEMDATE);
            parameter.Add("WEMDATE2", dto.WEMDATE2);
            parameter.Add("WEMDATEREMARK", dto.WEMDATEREMARK);
            parameter.Add("WEMHARMFULFACTORS", dto.WEMHARMFULFACTORS);
            parameter.Add("WEMEXPORSURE", dto.WEMEXPORSURE);
            parameter.Add("WEMEXPORSURE1", dto.WEMEXPORSURE1);
            parameter.Add("WEMEXPORSUREREMARK", dto.WEMEXPORSUREREMARK);
            parameter.Add("WORKCONTENT", dto.WORKCONTENT);
            parameter.Add("OSHADATE", dto.OSHADATE);
            parameter.Add("OSHACONTENT", dto.OSHACONTENT);
            parameter.Add("EDUTARGET", dto.EDUTARGET);
            parameter.Add("EDUPERSON", dto.EDUPERSON);
            parameter.Add("EDUAN", dto.EDUAN);
            parameter.Add("EDUTITLE", dto.EDUTITLE);
            parameter.Add("EDUTYPEJSON", dto.EDUTYPEJSON);
            parameter.Add("EDUMETHODJSON", dto.EDUMETHODJSON);
            parameter.Add("ENVCHECKJSON1", dto.ENVCHECKJSON1);
            parameter.Add("ENVCHECKJSON2", dto.ENVCHECKJSON2);
            parameter.Add("ENVCHECKJSON3", dto.ENVCHECKJSON3);
            parameter.Add("SITEMANAGERNAME", dto.SITEMANAGERNAME);
            parameter.Add("SITEMANAGERGRADE", dto.SITEMANAGERGRADE);
            parameter.Add("ENGINEERNAME", dto.ENGINEERNAME);
            parameter.Add("ISDELETED", dto.ISDELETED);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId); ;
            parameter.Add("SITENAME", dto.SITENAME);
            parameter.Add("SITEOWENER", dto.SITEOWENER);
            parameter.Add("SITETEL", dto.SITETEL);
            parameter.Add("SITEADDRESS", dto.SITEADDRESS);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Update("HIC_OSHA_REPORT_ENGINEER", dto.ID);
            if(dto.SITEMANAGERSIGN != null)
            {
                UpdateSign(dto.ID, dto.SITEMANAGERSIGN);
            }
            return FindOne(dto.ID);

        }
        public StatusReportEngineerDto Insert(StatusReportEngineerDto dto)
        {

            dto.ID = GetSequenceNextVal("HC_OSHA_REPORT_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_REPORT_ENGINEER ( ");
            parameter.AppendSql("    ID,SITE_ID,ESTIMATE_ID,SITENAME,SITEOWENER,SITETEL,SITEADDRESS,");
            parameter.AppendSql("    VISITDATE,VISITRESERVEDATE,WORKERCOUNT,WEMDATE,WEMDATE2, ");
            parameter.AppendSql("    WEMDATEREMARK,WEMHARMFULFACTORS,WEMEXPORSURE,WEMEXPORSURE1, ");
            parameter.AppendSql("    WEMEXPORSUREREMARK,WORKCONTENT,OSHADATE,OSHACONTENT,EDUTARGET,");
            parameter.AppendSql("    EDUPERSON,EDUAN,EDUTITLE,EDUTYPEJSON,EDUMETHODJSON,");
            parameter.AppendSql("    ENVCHECKJSON1,ENVCHECKJSON2,ENVCHECKJSON3,SITEMANAGERNAME,");
            parameter.AppendSql("    SITEMANAGERGRADE,ENGINEERNAME,ISDELETED,MODIFIED,MODIFIEDUSER,");
            parameter.AppendSql("    CREATED,CREATEDUSER,SWLICENSE ) ");
            parameter.AppendSql("VALUES ( ");
            parameter.AppendSql("    :ID");
            parameter.AppendSql("  , :SITE_ID");
            parameter.AppendSql("  , :ESTIMATE_ID");
            parameter.AppendSql("  , :SITENAME");
            parameter.AppendSql("  , :SITEOWENER");
            parameter.AppendSql("  , :SITETEL");
            parameter.AppendSql("  , :SITEADDRESS");
            parameter.AppendSql("  , :VISITDATE");
            parameter.AppendSql("  , :VISITRESERVEDATE");
            parameter.AppendSql("  , :WORKERCOUNT");
            parameter.AppendSql("  , :WEMDATE");
            parameter.AppendSql("  , :WEMDATE2");
            parameter.AppendSql("  , :WEMDATEREMARK");
            parameter.AppendSql("  , :WEMHARMFULFACTORS");
            parameter.AppendSql("  , :WEMEXPORSURE");
            parameter.AppendSql("  , :WEMEXPORSURE1");
            parameter.AppendSql("  , :WEMEXPORSUREREMARK");
            parameter.AppendSql("  , :WORKCONTENT");
            parameter.AppendSql("  , :OSHADATE");
            parameter.AppendSql("  , :OSHACONTENT");
            parameter.AppendSql("  , :EDUTARGET");
            parameter.AppendSql("  , :EDUPERSON");
            parameter.AppendSql("  , :EDUAN");
            parameter.AppendSql("  , :EDUTITLE");
            parameter.AppendSql("  , :EDUTYPEJSON");
            parameter.AppendSql("  , :EDUMETHODJSON");
            parameter.AppendSql("  , :ENVCHECKJSON1");
            parameter.AppendSql("  , :ENVCHECKJSON2");
            parameter.AppendSql("  , :ENVCHECKJSON3");
            parameter.AppendSql("  , :SITEMANAGERNAME");
            parameter.AppendSql("  , :SITEMANAGERGRADE");
            parameter.AppendSql("  , :ENGINEERNAME");
            parameter.AppendSql("  , :ISDELETED");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :MODIFIEDUSER");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :CREATEDUSER " );
            parameter.AppendSql("  , :SWLICENSE ) ");

            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("SITENAME", dto.SITENAME);
            parameter.Add("SITEOWENER", dto.SITEOWENER);
            parameter.Add("SITETEL", dto.SITETEL);
            parameter.Add("SITEADDRESS", dto.SITEADDRESS);
            parameter.Add("VISITDATE", dto.VISITDATE);
            parameter.Add("VISITRESERVEDATE", dto.VISITRESERVEDATE);
            parameter.Add("WORKERCOUNT", dto.WORKERCOUNT);
            parameter.Add("WEMDATE", dto.WEMDATE);
            parameter.Add("WEMDATE2", dto.WEMDATE2);
            parameter.Add("WEMDATEREMARK", dto.WEMDATEREMARK);
            parameter.Add("WEMHARMFULFACTORS", dto.WEMHARMFULFACTORS);
            parameter.Add("WEMEXPORSURE", dto.WEMEXPORSURE);
            parameter.Add("WEMEXPORSURE1", dto.WEMEXPORSURE1);
            parameter.Add("WEMEXPORSUREREMARK", dto.WEMEXPORSUREREMARK);
            parameter.Add("WORKCONTENT", dto.WORKCONTENT);
            parameter.Add("OSHADATE", dto.OSHADATE);
            parameter.Add("OSHACONTENT", dto.OSHACONTENT);
            parameter.Add("EDUTARGET", dto.EDUTARGET);
            parameter.Add("EDUPERSON", dto.EDUPERSON);
            parameter.Add("EDUAN", dto.EDUAN);
            parameter.Add("EDUTITLE", dto.EDUTITLE);
            parameter.Add("EDUTYPEJSON", dto.EDUTYPEJSON);
            parameter.Add("EDUMETHODJSON", dto.EDUMETHODJSON);
            parameter.Add("ENVCHECKJSON1", dto.ENVCHECKJSON1);
            parameter.Add("ENVCHECKJSON2", dto.ENVCHECKJSON2);
            parameter.Add("ENVCHECKJSON3", dto.ENVCHECKJSON3);
            parameter.Add("SITEMANAGERNAME", dto.SITEMANAGERNAME);
            parameter.Add("SITEMANAGERGRADE", dto.SITEMANAGERGRADE);
            parameter.Add("ENGINEERNAME", dto.ENGINEERNAME);
            parameter.Add("ISDELETED", dto.ISDELETED);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_REPORT_ENGINEER", dto.ID);
            if (dto.SITEMANAGERSIGN != null)
            {
                UpdateSign(dto.ID, dto.SITEMANAGERSIGN);
            }
            return FindOne(dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_ENGINEER ");
            parameter.AppendSql("   SET ISDELETED = 'Y' ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_OSHA_REPORT_ENGINEER", id);
        }
    }
}

