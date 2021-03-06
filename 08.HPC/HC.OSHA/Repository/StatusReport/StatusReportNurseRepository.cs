using ComBase;
using ComBase.Mvc;
using HC.Core.Dto;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC_Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Repository.StatusReport
{
    public class StatusReportNurseRepository : BaseRepository
    {
        public StatusReportNurseDto FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_REPORT_NURSE ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            SiteStatusDto siteStatusdto = ExecuteReaderSingle<SiteStatusDto>(parameter);

            StatusReportNurseDto dto = ExecuteReaderSingle<StatusReportNurseDto>(parameter);

            if (dto != null)
            {
                dto.SiteStatusDto = siteStatusdto;
            }

            return dto;
        }
        public void UpdateApprove(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_NURSE ");
            parameter.AppendSql("   SET APPROVE = SYSTIMESTAMP ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Update("HIC_OSHA_REPORT_NURSE", id);
        }

        public void UpdateSign(long id, string base64Image)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_NURSE ");
            parameter.AppendSql("   SET SITEMANAGERSIGN = :SITEMANAGERSIGN ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SITEMANAGERSIGN", base64Image);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Update("HIC_OSHA_REPORT_NURSE", id);

        }
        public void UpdateSangdamSign(long id, string base64Image)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_NURSE ");
            parameter.AppendSql("   SET SANGDAMSIGN = :SANGDAMSIGN ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SANGDAMSIGN", base64Image);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_REPORT_NURSE", id);
        }
        public StatusReportNurseDto FindLast(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_REPORT_NURSE ");
            parameter.AppendSql(" WHERE SITE_ID = :SITE_ID ");
            parameter.AppendSql("   AND VISITDATE = (SELECT MAX(VISITDATE) FROM HIC_OSHA_REPORT_NURSE ");
            parameter.AppendSql("                     WHERE SITE_ID = :SITE_ID ");
            parameter.AppendSql("                       AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("                       AND ISDELETED = 'N' ) ");
            parameter.AppendSql("   AND ISDELETED = 'N' ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            SiteStatusDto siteStatusdto = ExecuteReaderSingle<SiteStatusDto>(parameter);

            StatusReportNurseDto dto = ExecuteReaderSingle<StatusReportNurseDto>(parameter);
            if (dto != null)
            {
                dto.SiteStatusDto = siteStatusdto;
            }
          
            return dto;
        }

        public List<VisitDateModel> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.ID, To_DATE(A.VISITDATE, 'YYYY-MM-DD') AS VISITDATE, B.NAME, ");
            parameter.AppendSql("       SUBSTR(A.VISITDATE,0,4)  AS VISITYEAR ");
            parameter.AppendSql("  FROM HIC_OSHA_REPORT_NURSE A ");
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

            parameter.AppendSql("SELECT VISITDATE, ID FROM HIC_OSHA_REPORT_NURSE ");
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

        public StatusReportNurseDto Update(StatusReportNurseDto dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_NURSE SET ");
            parameter.AppendSql("       CURRENTWORKERCOUNT = :CURRENTWORKERCOUNT");
            parameter.AppendSql("      , SITENAME = :SITENAME");
            parameter.AppendSql("      , EXTDATA = :EXTDATA");
            parameter.AppendSql("      , VISITDATE = :VISITDATE");
            parameter.AppendSql("      , VISITRESERVEDATE = :VISITRESERVEDATE");
            parameter.AppendSql("      , NEWWORKERCOUNT = :NEWWORKERCOUNT");
            parameter.AppendSql("      , RETIREWORKERCOUNT = :RETIREWORKERCOUNT");
            parameter.AppendSql("      , CHANGEWORKERCOUNT = :CHANGEWORKERCOUNT");
            parameter.AppendSql("      , ACCIDENTWORKERCOUNT = :ACCIDENTWORKERCOUNT");
            parameter.AppendSql("      , DEADWORKERCOUNT = :DEADWORKERCOUNT");
            parameter.AppendSql("      , INJURYWORKERCOUNT = :INJURYWORKERCOUNT");
            parameter.AppendSql("      , BIZDISEASEWORKERCOUNT = :BIZDISEASEWORKERCOUNT");
            parameter.AppendSql("      , GENERALHEALTHCHECKDATE = :GENERALHEALTHCHECKDATE");
            parameter.AppendSql("      , SPECIALHEALTHCHECKDATE = :SPECIALHEALTHCHECKDATE");
            parameter.AppendSql("      , GENERALTOTALCOUNT = :GENERALTOTALCOUNT");
            parameter.AppendSql("      , SPECIALTOTALCOUNT = :SPECIALTOTALCOUNT");
            parameter.AppendSql("      , GENERALD2COUNT = :GENERALD2COUNT");
            parameter.AppendSql("      , GENERALC2COUNT = :GENERALC2COUNT");
            parameter.AppendSql("      , SPECIALD1COUNT = :SPECIALD1COUNT");
            parameter.AppendSql("      , SPECIALC1COUNT = :SPECIALC1COUNT");
            parameter.AppendSql("      , SPECIALD2COUNT = :SPECIALD2COUNT");
            parameter.AppendSql("      , SPECIALC2COUNT = :SPECIALC2COUNT");
            parameter.AppendSql("      , SPECIALDNCOUNT = :SPECIALDNCOUNT");
            parameter.AppendSql("      , SPECIALCNCOUNT = :SPECIALCNCOUNT");
            parameter.AppendSql("      , WEMDATE = :WEMDATE");
            parameter.AppendSql("      , WEMEXPORSURE = :WEMEXPORSURE");
            parameter.AppendSql("      , WEMEXPORSURE2 = :WEMEXPORSURE2");
            parameter.AppendSql("      , WEMEXPORSUREREMARK = :WEMEXPORSUREREMARK");
            parameter.AppendSql("      , WEMHARMFULFACTORS = :WEMHARMFULFACTORS");
            parameter.AppendSql("      , PERFORMCONTENT = :PERFORMCONTENT");
            parameter.AppendSql("      , SITEMANAGERNAME = :SITEMANAGERNAME");
            parameter.AppendSql("      , SITEMANAGERGRADE = :SITEMANAGERGRADE");
            parameter.AppendSql("      , NURSENAME = :NURSENAME");
            parameter.AppendSql("      , ISDELETED = :ISDELETED");
            parameter.AppendSql("      , MODIFIED = SYSTIMESTAMP");
            parameter.AppendSql("      , MODIFIEDUSER = :MODIFIEDUSER");
            parameter.AppendSql("      , DEPTNAME = :DEPTNAME ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", dto.ID);
            parameter.Add("CURRENTWORKERCOUNT", dto.SiteStatusDto.CURRENTWORKERCOUNT);
            parameter.Add("SITENAME", dto.SiteStatusDto.SITENAME);
            parameter.Add("EXTDATA", dto.SiteStatusDto.EXTDATA);
            parameter.Add("VISITDATE", dto.VISITDATE);
            parameter.Add("VISITRESERVEDATE", dto.VISITRESERVEDATE);
            parameter.Add("NEWWORKERCOUNT", dto.SiteStatusDto.NEWWORKERCOUNT);
            parameter.Add("RETIREWORKERCOUNT", dto.SiteStatusDto.RETIREWORKERCOUNT);
            parameter.Add("CHANGEWORKERCOUNT", dto.SiteStatusDto.CHANGEWORKERCOUNT);
            parameter.Add("ACCIDENTWORKERCOUNT", dto.SiteStatusDto.ACCIDENTWORKERCOUNT);
            parameter.Add("DEADWORKERCOUNT", dto.SiteStatusDto.DEADWORKERCOUNT);
            parameter.Add("INJURYWORKERCOUNT", dto.SiteStatusDto.INJURYWORKERCOUNT);
            parameter.Add("BIZDISEASEWORKERCOUNT", dto.SiteStatusDto.BIZDISEASEWORKERCOUNT);
            parameter.Add("GENERALHEALTHCHECKDATE", dto.SiteStatusDto.GENERALHEALTHCHECKDATE);
            parameter.Add("SPECIALHEALTHCHECKDATE", dto.SiteStatusDto.SPECIALHEALTHCHECKDATE);
            parameter.Add("GENERALTOTALCOUNT", dto.SiteStatusDto.GENERALTOTALCOUNT);
            parameter.Add("SPECIALTOTALCOUNT", dto.SiteStatusDto.SPECIALTOTALCOUNT);

            parameter.Add("GENERALD2COUNT", dto.SiteStatusDto.GENERALD2COUNT);
            parameter.Add("GENERALC2COUNT", dto.SiteStatusDto.GENERALC2COUNT);
            parameter.Add("SPECIALD1COUNT", dto.SiteStatusDto.SPECIALD1COUNT);
            parameter.Add("SPECIALC1COUNT", dto.SiteStatusDto.SPECIALC1COUNT);
            parameter.Add("SPECIALD2COUNT", dto.SiteStatusDto.SPECIALD2COUNT);
            parameter.Add("SPECIALC2COUNT", dto.SiteStatusDto.SPECIALC2COUNT);
            parameter.Add("SPECIALDNCOUNT", dto.SiteStatusDto.SPECIALDNCOUNT);
            parameter.Add("SPECIALCNCOUNT", dto.SiteStatusDto.SPECIALCNCOUNT);
            parameter.Add("WEMDATE", dto.SiteStatusDto.WEMDATE);
            parameter.Add("WEMEXPORSURE", dto.SiteStatusDto.WEMEXPORSURE);
            parameter.Add("WEMEXPORSURE2", dto.SiteStatusDto.WEMEXPORSURE2);
            parameter.Add("WEMEXPORSUREREMARK", dto.SiteStatusDto.WEMEXPORSUREREMARK);
            parameter.Add("WEMHARMFULFACTORS", dto.SiteStatusDto.WEMHARMFULFACTORS);
            parameter.Add("PERFORMCONTENT", dto.PERFORMCONTENT);
            parameter.Add("SITEMANAGERNAME", dto.SITEMANAGERNAME);
            parameter.Add("SITEMANAGERGRADE", dto.SITEMANAGERGRADE);
            parameter.Add("NURSENAME", dto.NURSENAME);
            parameter.Add("ISDELETED", dto.ISDELETED);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("DEPTNAME", dto.SiteStatusDto.DEPTNAME);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Update("HIC_OSHA_REPORT_NURSE", dto.ID);

            if (dto.SITEMANAGERSIGN != null)
            {
                UpdateSign(dto.ID, dto.SITEMANAGERSIGN);
            }
            if (dto.SANGDAMSIGN != null)
            {
                UpdateSangdamSign(dto.ID, dto.SANGDAMSIGN);
            }
            return FindOne(dto.ID);
        }
        public void UpdateSelf(long oldId, long newId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_NURSE ");
            parameter.AppendSql("   SET ID = :newId ");
            parameter.AppendSql(" WHERE ID = :oldId ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("newId", newId);
            parameter.Add("oldId", oldId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
        }
        public StatusReportNurseDto Insert(StatusReportNurseDto dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_REPORT_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_REPORT_NURSE ( ");
            parameter.AppendSql("    ID ");
            parameter.AppendSql("  , SITE_ID");
            parameter.AppendSql("  , ESTIMATE_ID");
            parameter.AppendSql("  , SITENAME");
            parameter.AppendSql("  , EXTDATA");
            parameter.AppendSql("  , CURRENTWORKERCOUNT");
            parameter.AppendSql("  , VISITDATE");
            parameter.AppendSql("  , VISITRESERVEDATE");
            parameter.AppendSql("  , NEWWORKERCOUNT");
            parameter.AppendSql("  , RETIREWORKERCOUNT");
            parameter.AppendSql("  , CHANGEWORKERCOUNT");
            parameter.AppendSql("  , ACCIDENTWORKERCOUNT");
            parameter.AppendSql("  , DEADWORKERCOUNT");
            parameter.AppendSql("  , INJURYWORKERCOUNT");
            parameter.AppendSql("  , BIZDISEASEWORKERCOUNT");
            parameter.AppendSql("  , GENERALHEALTHCHECKDATE");
            parameter.AppendSql("  , SPECIALHEALTHCHECKDATE");
            parameter.AppendSql("  , GENERALTOTALCOUNT");
            parameter.AppendSql("  , SPECIALTOTALCOUNT");
            parameter.AppendSql("  , GENERALD2COUNT");
            parameter.AppendSql("  , GENERALC2COUNT");
            parameter.AppendSql("  , SPECIALD1COUNT");
            parameter.AppendSql("  , SPECIALC1COUNT");
            parameter.AppendSql("  , SPECIALD2COUNT");
            parameter.AppendSql("  , SPECIALC2COUNT");
            parameter.AppendSql("  , SPECIALDNCOUNT");
            parameter.AppendSql("  , SPECIALCNCOUNT");
            parameter.AppendSql("  , WEMDATE");
            parameter.AppendSql("  , WEMEXPORSURE");
            parameter.AppendSql("  , WEMEXPORSURE2");
            parameter.AppendSql("  , WEMEXPORSUREREMARK");
            parameter.AppendSql("  , WEMHARMFULFACTORS");
            parameter.AppendSql("  , PERFORMCONTENT");
            parameter.AppendSql("  , SITEMANAGERNAME");
            parameter.AppendSql("  , SITEMANAGERGRADE");

            parameter.AppendSql("  , SITEMANAGERSIGN");
            parameter.AppendSql("  , SANGDAMSIGN");

            parameter.AppendSql("  , NURSENAME");
            parameter.AppendSql("  , ISDELETED");
            parameter.AppendSql("  , MODIFIED");
            parameter.AppendSql("  , MODIFIEDUSER");
            parameter.AppendSql("  , CREATED");
            parameter.AppendSql("  , CREATEDUSER");
            parameter.AppendSql("  , DEPTNAME ");
            parameter.AppendSql("  , SWLICENSE ");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :ID ");       
            parameter.AppendSql("  , :SITE_ID");
            parameter.AppendSql("  , :ESTIMATE_ID");
            parameter.AppendSql("  ,  :SITENAME ");
            parameter.AppendSql("  ,  :EXTDATA ");
            parameter.AppendSql("  , :CURRENTWORKERCOUNT");
            parameter.AppendSql("  , :VISITDATE");
            parameter.AppendSql("  , :VISITRESERVEDATE");
            parameter.AppendSql("  , :NEWWORKERCOUNT");
            parameter.AppendSql("  , :RETIREWORKERCOUNT");
            parameter.AppendSql("  , :CHANGEWORKERCOUNT");
            parameter.AppendSql("  , :ACCIDENTWORKERCOUNT");
            parameter.AppendSql("  , :DEADWORKERCOUNT");
            parameter.AppendSql("  , :INJURYWORKERCOUNT");
            parameter.AppendSql("  , :BIZDISEASEWORKERCOUNT");
            parameter.AppendSql("  , :GENERALHEALTHCHECKDATE");
            parameter.AppendSql("  , :SPECIALHEALTHCHECKDATE");
            parameter.AppendSql("  , :GENERALTOTALCOUNT");
            parameter.AppendSql("  , :SPECIALTOTALCOUNT");
            parameter.AppendSql("  , :GENERALD2COUNT");
            parameter.AppendSql("  , :GENERALC2COUNT");
            parameter.AppendSql("  , :SPECIALD1COUNT");
            parameter.AppendSql("  , :SPECIALC1COUNT");
            parameter.AppendSql("  , :SPECIALD2COUNT");
            parameter.AppendSql("  , :SPECIALC2COUNT");
            parameter.AppendSql("  , :SPECIALDNCOUNT");
            parameter.AppendSql("  , :SPECIALCNCOUNT");
            parameter.AppendSql("  , :WEMDATE");
            parameter.AppendSql("  , :WEMEXPORSURE");
            parameter.AppendSql("  , :WEMEXPORSURE2");
            parameter.AppendSql("  , :WEMEXPORSUREREMARK");
            parameter.AppendSql("  , :WEMHARMFULFACTORS");
            parameter.AppendSql("  , :PERFORMCONTENT");
            parameter.AppendSql("  , :SITEMANAGERNAME");
            parameter.AppendSql("  , :SITEMANAGERGRADE");

            parameter.AppendSql("  , :SITEMANAGERSIGN");
            parameter.AppendSql("  , :SANGDAMSIGN");

            parameter.AppendSql("  , :NURSENAME");
            parameter.AppendSql("  , 'N'");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :MODIFIEDUSER");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :CREATEDUSER");
            parameter.AppendSql("  , :DEPTNAME ");
            parameter.AppendSql("  , :SWLICENSE ");
            parameter.AppendSql(") ");

            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);         
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("SITENAME", dto.SiteStatusDto.SITENAME);
            parameter.Add("EXTDATA", dto.SiteStatusDto.EXTDATA);
            parameter.Add("CURRENTWORKERCOUNT", dto.SiteStatusDto.CURRENTWORKERCOUNT);
            parameter.Add("VISITDATE", dto.VISITDATE);
            parameter.Add("VISITRESERVEDATE", dto.VISITRESERVEDATE);
            parameter.Add("NEWWORKERCOUNT", dto.SiteStatusDto.NEWWORKERCOUNT);
            parameter.Add("RETIREWORKERCOUNT", dto.SiteStatusDto.RETIREWORKERCOUNT);
            parameter.Add("CHANGEWORKERCOUNT", dto.SiteStatusDto.CHANGEWORKERCOUNT);
            parameter.Add("ACCIDENTWORKERCOUNT", dto.SiteStatusDto.ACCIDENTWORKERCOUNT);
            parameter.Add("DEADWORKERCOUNT", dto.SiteStatusDto.DEADWORKERCOUNT);
            parameter.Add("INJURYWORKERCOUNT", dto.SiteStatusDto.INJURYWORKERCOUNT);
            parameter.Add("BIZDISEASEWORKERCOUNT", dto.SiteStatusDto.BIZDISEASEWORKERCOUNT);
            parameter.Add("GENERALHEALTHCHECKDATE", dto.SiteStatusDto.GENERALHEALTHCHECKDATE);
            parameter.Add("SPECIALHEALTHCHECKDATE", dto.SiteStatusDto.SPECIALHEALTHCHECKDATE);
            parameter.Add("GENERALTOTALCOUNT", dto.SiteStatusDto.GENERALTOTALCOUNT);
            parameter.Add("SPECIALTOTALCOUNT", dto.SiteStatusDto.SPECIALTOTALCOUNT);
            parameter.Add("GENERALD2COUNT", dto.SiteStatusDto.GENERALD2COUNT);
            parameter.Add("GENERALC2COUNT", dto.SiteStatusDto.GENERALC2COUNT);
            parameter.Add("SPECIALD1COUNT", dto.SiteStatusDto.SPECIALD1COUNT);
            parameter.Add("SPECIALC1COUNT", dto.SiteStatusDto.SPECIALC1COUNT);
            parameter.Add("SPECIALD2COUNT", dto.SiteStatusDto.SPECIALD2COUNT);
            parameter.Add("SPECIALC2COUNT", dto.SiteStatusDto.SPECIALC2COUNT);
            parameter.Add("SPECIALDNCOUNT", dto.SiteStatusDto.SPECIALDNCOUNT);
            parameter.Add("SPECIALCNCOUNT", dto.SiteStatusDto.SPECIALCNCOUNT);
            parameter.Add("WEMDATE", dto.SiteStatusDto.WEMDATE);
            parameter.Add("WEMEXPORSURE", dto.SiteStatusDto.WEMEXPORSURE);
            parameter.Add("WEMEXPORSURE2", dto.SiteStatusDto.WEMEXPORSURE2);
            parameter.Add("WEMEXPORSUREREMARK", dto.SiteStatusDto.WEMEXPORSUREREMARK);
            parameter.Add("WEMHARMFULFACTORS", dto.SiteStatusDto.WEMHARMFULFACTORS);
            parameter.Add("PERFORMCONTENT", dto.PERFORMCONTENT);
            parameter.Add("SITEMANAGERNAME", dto.SITEMANAGERNAME);
            parameter.Add("SITEMANAGERGRADE", dto.SITEMANAGERGRADE);

            parameter.Add("SITEMANAGERSIGN", dto.SITEMANAGERSIGN);
            parameter.Add("SANGDAMSIGN", dto.SANGDAMSIGN);

       
            parameter.Add("NURSENAME", dto.NURSENAME);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("DEPTNAME", dto.SiteStatusDto.DEPTNAME);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            if (dto.CREATEDUSER!= null){
                parameter.Add("CREATEDUSER", dto.CREATEDUSER);
            }
            else
            {
                parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            }
            

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_REPORT_NURSE", dto.ID);

            if (dto.SITEMANAGERSIGN != null)
            {
                UpdateSign(dto.ID, dto.SITEMANAGERSIGN);
            }
            return FindOne(dto.ID);
        }
        public void UpdateOptioin(long id, String html)
        {

            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_NURSE");
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

            DataSyncService.Instance.Update("HIC_OSHA_REPORT_NURSE", id);
        }
        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_REPORT_NURSE ");
            parameter.AppendSql("   SET ISDELETED = 'Y', ");
            parameter.AppendSql("       MODIFIED = SYSTIMESTAMP, ");
            parameter.AppendSql("       MODIFIEDUSER = :MODIFIEDUSER ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_OSHA_REPORT_NURSE", id);
        }

        //internal MacrowordDto FindOneCard(StatusReportNurseDto dto)
        //{
        //    MParameter parameter = CreateParameter();
        //    parameter.AppendSql("SELECT A.ID                                         ");
        //    parameter.AppendSql("     , A.SITE_ID                                    ");
        //    parameter.AppendSql("     , A.ESTIMATE_ID                                ");
        //    parameter.AppendSql("     , A.REGDATE                                    ");
        //    parameter.AppendSql("     , A.CERT                                       ");
        //    parameter.AppendSql("     , A.NAME                                       ");
        //    parameter.AppendSql("     , A.CONTENT                                    ");
        //    parameter.AppendSql("     , A.OPINION AS CONTENT2                        ");
        //    parameter.AppendSql("  FROM HIC_OSHA_CARD19 A                            ");
        //    parameter.AppendSql(" WHERE 1 = 1                                        ");
        //    parameter.AppendSql("   AND SITE_ID = :SITE_ID                           ");
        //    parameter.AppendSql("   AND TO_CHAR(REGDATE, 'YYYYMMDD') = :REGDATE      ");

        //    parameter.Add("SITE_ID", dto.SITE_ID);
        //    parameter.Add("REGDATE", dto.VISITDATE);

        //    return ExecuteReaderSingle<MacrowordDto>(parameter);
        //}
    }
}
