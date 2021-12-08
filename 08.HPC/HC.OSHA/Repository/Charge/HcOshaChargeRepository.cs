using ComBase.Controls;
using ComBase;
using ComBase.Mvc;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC_OSHA.Model.Visit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Repository
{
    public class HcOshaChargeRepository : BaseRepository
    {
        public List<CHARGE_MODEL> FindSite(string startDate, string endDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT B.NAME AS SITE_NAME, MAX(A.ID) AS SCHEDULE_ID, A.SITE_ID FROM HIC_OSHA_SCHEDULE A    ");
            parameter.AppendSql("   INNER JOIN HC_SITE_VIEW B    ");
            parameter.AppendSql("   ON A.SITE_ID = B.ID    ");
            parameter.AppendSql("   INNER JOIN HIC_OSHA_SITE C    ");
            parameter.AppendSql("   ON A.SITE_ID = C.ID    ");
            parameter.AppendSql("   WHERE C.PARENTSITE_ID IS NULL    ");
            parameter.AppendSql("   AND A.VISITRESERVEDATE >= TO_DATE(:startDate,'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND A.VISITRESERVEDATE <= TO_DATE(:endDate,'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND A.isdeleted = 'N'    ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   GROUP BY B.NAME, A.SITE_ID    ");
            parameter.AppendSql("   ORDER BY B.NAME, A.SITE_ID    ");
            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReader<CHARGE_MODEL>(parameter);
        }

        /// <summary>
        /// UNION 쿼리
        /// 해당사업장 + 하청 사업장
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<CHARGE_MODEL> FindAll(long siteId, string startDate, string endDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID AS SCHEDULE_ID, A.VISITRESERVEDATE, B.ID AS VISIT_ID, A.SITE_ID,A.SITE_ID as PARENT_SITE_ID, A.ESTIMATE_ID, AA.NAME AS SITE_NAME, B.VISITDATETIME, B.VISITUSERNAME, B.ISPRECHARGE, C.WORKERCOUNT, C.UNITPRICE, C.UNITTOTALPRICE, C.TOTALPRICE, C.ID AS VISIT_PRICE_ID,  ");
            parameter.AppendSql("      E.ID, E.WRTNO,  E.CHARGEDATE, E.WORKERCOUNT AS E_WORKERCOUNT, E.UNITPRICE AS E_UNITPRICE, E.TOTALPRICE AS E_TOTALPRICE, E.ISCOMPLETE, E.ISVISIT, E.CREATED , E.CREATEDUSER, E.REMARK   ");
            parameter.AppendSql("FROM HIC_OSHA_SCHEDULE A                                           ");
            parameter.AppendSql("INNER JOIN HC_SITE_VIEW AA                                         ");
            parameter.AppendSql("ON A.SITE_ID = AA.ID                                               ");
            parameter.AppendSql("LEFT OUTER JOIN HIC_OSHA_VISIT B                                   ");
            parameter.AppendSql("ON A.ID = B.SCHEDULE_ID                                            ");
            parameter.AppendSql("LEFT OUTER JOIN HIC_OSHA_VISIT_PRICE C                             ");
            parameter.AppendSql("ON B.ID = C.VISIT_ID                                               ");
            parameter.AppendSql("AND C.ISDELETED = 'N'                                              ");
            parameter.AppendSql("LEFT OUTER JOIN HIC_OSHA_CHARGE E                                  ");
            parameter.AppendSql("ON C.ID = E.VISIT_ID                                               ");
            parameter.AppendSql("AND E.ISDELETED = 'N'                                              ");
            parameter.AppendSql("WHERE A.SITE_ID = :SITE_ID                                         ");
            parameter.AppendSql("  AND A.ISDELETED = 'N'                                            ");
            parameter.AppendSql("  AND A.VISITRESERVEDATE >= TO_DATE(:startDate,'YYYY-MM-DD')       ");
            parameter.AppendSql("  AND A.VISITRESERVEDATE <= TO_DATE(:endDate, 'YYYY-MM-DD')        ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  AND E.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("UNION ALL                                                          ");
            parameter.AppendSql("SELECT A.ID AS SCHEDULE_ID, A.VISITRESERVEDATE, B.ID AS VISIT_ID, A.SITE_ID, BB.PARENTSITE_ID AS PARENT_SITE_ID, A.ESTIMATE_ID, AA.NAME AS SITE_NAME, B.VISITDATETIME, B.VISITUSERNAME, B.ISPRECHARGE, C.WORKERCOUNT, C.UNITPRICE, C.UNITTOTALPRICE, C.TOTALPRICE, C.ID AS VISIT_PRICE_ID,  ");
            parameter.AppendSql("         E.ID, E.WRTNO,  E.CHARGEDATE, E.WORKERCOUNT AS E_WORKERCOUNT, E.UNITPRICE AS E_UNITPRICE, E.TOTALPRICE AS E_TOTALPRICE, E.ISCOMPLETE, E.ISVISIT, E.CREATED , E.CREATEDUSER, E.REMARK   ");
            parameter.AppendSql("FROM HIC_OSHA_SCHEDULE A                                           ");
            parameter.AppendSql("INNER JOIN HC_SITE_VIEW AA                                         ");
            parameter.AppendSql("ON A.SITE_ID = AA.ID                                               ");
            parameter.AppendSql("INNER JOIN HIC_OSHA_SITE BB                                        ");
            parameter.AppendSql("ON AA.ID = BB.ID                                                   ");
            parameter.AppendSql("LEFT OUTER JOIN HIC_OSHA_VISIT B                                   ");
            parameter.AppendSql("ON A.ID = B.SCHEDULE_ID                                            ");
            parameter.AppendSql("LEFT OUTER JOIN HIC_OSHA_VISIT_PRICE C                             ");
            parameter.AppendSql("ON B.ID = C.VISIT_ID                                               ");
            parameter.AppendSql("AND C.ISDELETED = 'N'                                              ");
            parameter.AppendSql("LEFT OUTER JOIN HIC_OSHA_CHARGE E                                  ");
            parameter.AppendSql("ON C.ID = E.VISIT_ID                                               ");
            parameter.AppendSql("AND E.ISDELETED = 'N'                                              ");
            parameter.AppendSql("WHERE A.SITE_ID = (select ID from HIC_OSHA_SITE                    ");
            parameter.AppendSql("                     where PARENTSITE_ID = :SITE_ID                ");
            parameter.AppendSql("                       AND SWLICENSE = :SWLICENSE                  ");
            parameter.AppendSql("                 )                                                 ");
            parameter.AppendSql("AND A.ISDELETED = 'N'                                              ");
            parameter.AppendSql("AND A.VISITRESERVEDATE >=  TO_DATE(:startDate,'YYYY-MM-DD')        ");
            parameter.AppendSql("AND A.VISITRESERVEDATE <=  TO_DATE(:endDate, 'YYYY-MM-DD')         ");
            parameter.AppendSql("AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("AND AA.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("AND BB.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("AND E.SWLICENSE = :SWLICENSE ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReader<CHARGE_MODEL>(parameter);
        }

        public void UpdateWrtNo(long id, long wrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CHARGE");
            parameter.AppendSql("    SET ");
            parameter.AppendSql("       WRTNO = :WRTNO ");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("WRTNO", wrtNo);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
        }
        public long Insert(HC_OSHA_CHARGE dto)
        {
            dto.ID = GetSequenceNextVal("HC_CHARGE_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CHARGE");
            parameter.AppendSql("(");
            parameter.AppendSql("    ID");
            parameter.AppendSql("  , SITE_ID");
            parameter.AppendSql("  , ESTIMATE_ID");
            parameter.AppendSql("  , VISIT_ID");
            parameter.AppendSql("  , VISIT_PRICE_ID");
            parameter.AppendSql("  , CHARGEDATE");
            parameter.AppendSql("  , WORKERCOUNT");
            parameter.AppendSql("  , UNITPRICE");
            parameter.AppendSql("  , TOTALPRICE");
            parameter.AppendSql("  , ISVISIT");
            parameter.AppendSql("  , ISCOMPLETE");
            parameter.AppendSql("  , REMARK");
            parameter.AppendSql("  , ISDELETED");
            parameter.AppendSql("  , MODIFIED");
            parameter.AppendSql("  , MODIFIEDUSER");
            parameter.AppendSql("  , CREATED");
            parameter.AppendSql("  , CREATEDUSER");
            parameter.AppendSql("  , SWLICENSE");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :ID");
            parameter.AppendSql("  , :SITE_ID");
            parameter.AppendSql("  , :ESTIMATE_ID");
            parameter.AppendSql("  , :VISIT_ID");
            parameter.AppendSql("  , :VISIT_PRICE_ID");
            parameter.AppendSql("  , :CHARGEDATE");
            parameter.AppendSql("  , :WORKERCOUNT");
            parameter.AppendSql("  , :UNITPRICE");
            parameter.AppendSql("  , :TOTALPRICE");
            parameter.AppendSql("  , 'N'");
            parameter.AppendSql("  , :ISCOMPLETE");
            parameter.AppendSql("  , :REMARK");
            parameter.AppendSql("  , 'N'");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :MODIFIEDUSER");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :CREATEDUSER");
            parameter.AppendSql("  , :SWLICENSE");
            parameter.AppendSql(") ");

            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("VISIT_ID", dto.VISIT_ID);
            parameter.Add("VISIT_PRICE_ID", dto.VISIT_PRICE_ID);
            parameter.Add("CHARGEDATE", dto.CHARGEDATE);
            parameter.Add("WORKERCOUNT", dto.WORKERCOUNT);
            parameter.Add("UNITPRICE", dto.UNITPRICE);
            parameter.Add("TOTALPRICE", dto.TOTALPRICE);
          //  parameter.Add("ISVISIT", dto.ISVISIT);
            parameter.Add("ISCOMPLETE", dto.ISCOMPLETE);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            return dto.ID;
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CHARGE");
            parameter.AppendSql("    SET ");
            parameter.AppendSql("       ISDELETED = 'Y' ");
            parameter.AppendSql("      , MODIFIED =  SYSTIMESTAMP");
            parameter.AppendSql("      , MODIFIEDUSER = :MODIFIEDUSER");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
        }


        public List<ChargeDocumentModel> FindChargeDocument(string startDate, string endDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT *                                                                                   ");
            parameter.AppendSql("  FROM                                                                                     ");
            parameter.AppendSql("  (                                                                                        ");
            parameter.AppendSql("        SELECT TO_CHAR(B.BDATE,'YYYY-MM-DD') BDATE                                         ");
            parameter.AppendSql("             , B.WRTNO                                                                     ");
            parameter.AppendSql("             , A.GJONG                                                                     ");
            parameter.AppendSql("             , A.LTDCODE                                                                   ");
            parameter.AppendSql("             , E.NAME AS SITENAME                                                          ");
            parameter.AppendSql("             , B.GEACODE                                                                   ");
            parameter.AppendSql("             , TO_CHAR(B.SLIPAMT, '999,999,999,999') AS SLIPAMT                            ");
            parameter.AppendSql("             , A.REMARK                                                                    ");
            parameter.AppendSql("             , D.USERID                                                                    ");
            parameter.AppendSql("             , D.NAME                                                                      ");
            parameter.AppendSql("             , C.MISUJONG MJONG                                                            ");
            parameter.AppendSql("             , F.EMAIL                                                                     ");
            parameter.AppendSql("             , F.DAMNAME                                                                   ");
            parameter.AppendSql("             , F.REMARK AS EMAIL2                                                          ");
            parameter.AppendSql("          FROM HIC_MISU_MST A                                                              ");
            parameter.AppendSql("          RIGHT OUTER JOIN HIC_MISU_SLIP B                                                 ");
            parameter.AppendSql("                        ON A.WRTNO = B.WRTNO                                               ");
            parameter.AppendSql("                       AND B.GEACODE = '11'                                                ");
            parameter.AppendSql("          RIGHT OUTER JOIN HIC_EXJONG C                                                    ");
            parameter.AppendSql("                        ON C.CODE = A.GJONG                                                ");
            parameter.AppendSql("                       AND C.MISUJONG = '6'                                                ");
            parameter.AppendSql("          LEFT OUTER JOIN HIC_USERS D                                                      ");
            parameter.AppendSql("                       ON TRIM(B.ENTSABUN) = D.USERID                                      ");
            parameter.AppendSql("          INNER JOIN HIC_LTD E                                                             ");
            parameter.AppendSql("                  ON B.LTDCODE = E.CODE                                                    ");
            parameter.AppendSql("          INNER JOIN HIC_LTD_TAX F                                                         ");
            parameter.AppendSql("                  ON E.CODE = F.LTDCODE                                                    ");
            parameter.AppendSql("                 AND F.BUSE = 4                                                            ");
            parameter.AppendSql("         WHERE B.BDATE >= TO_DATE(:startDate, 'YYYY-MM-DD')                                ");
            parameter.AppendSql("           AND B.BDATE <= TO_DATE(:endDate, 'YYYY-MM-DD')                                  ");
            parameter.AppendSql("           AND A.SWLICENSE = :SWLICENSE                                                   ");
            parameter.AppendSql("           AND B.SWLICENSE = :SWLICENSE                                                   ");
            parameter.AppendSql("           AND D.SWLICENSE = :SWLICENSE                                                   ");
            parameter.AppendSql("           AND E.SWLICENSE = :SWLICENSE                                                   ");
            parameter.AppendSql("           AND F.SWLICENSE = :SWLICENSE                                                   ");
            parameter.AppendSql("        ORDER BY B.BDATE, B.WRTNO,B.GEACODE                                                ");
            parameter.AppendSql("  ) A LEFT OUTER JOIN (                                                                    ");
            parameter.AppendSql("        SELECT TO_CHAR(A.SEND_DATE, 'YYYY-MM-DD') AS SENDDATE                              ");
            parameter.AppendSql("             , A.SEND_USER                                                                 ");
            parameter.AppendSql("             , A.SITE_ID                                                                   ");
            parameter.AppendSql("             , B.NAME AS SENDNAME                                                          ");
            parameter.AppendSql("          FROM                                                                             ");
            parameter.AppendSql("          (                                                                                ");
            parameter.AppendSql("                SELECT ROW_NUMBER() OVER(PARTITION BY WRTNO ORDER BY SEND_DATE DESC) AS NUM");
            parameter.AppendSql("                     , SEND_DATE                                                           ");
            parameter.AppendSql("                     , SEND_USER                                                           ");
            parameter.AppendSql("                     , SITE_ID                                                             ");
            parameter.AppendSql("                  FROM HIC_OSHA_MAIL_SEND                                                  ");
            parameter.AppendSql("                 WHERE SEND_TYPE = 'CHARGE'                                                ");
            parameter.AppendSql("                   AND SEND_DATE >= TO_DATE(:startDate, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("                   AND SEND_DATE <= TO_DATE(:endDate, 'YYYY-MM-DD')                        ");
            parameter.AppendSql("                   AND SWLICENSE = :SWLICENSE                                                   ");
            parameter.AppendSql("          ) A LEFT OUTER JOIN HIC_USERS B                                                  ");
            parameter.AppendSql("                           ON A.SEND_USER = TRIM(B.USERID)                                 ");
            parameter.AppendSql("         WHERE NUM = 1                                                                     ");
            parameter.AppendSql("  ) B ON A.LTDCODE  = B.SITE_ID                                                            ");

            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReader<ChargeDocumentModel>(parameter);
        }
    }
}
