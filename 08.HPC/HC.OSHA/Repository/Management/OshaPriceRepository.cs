using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Utils;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC_Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Repository
{
    public class OshaPriceRepository : BaseRepository
    {
        public List<OSHA_PRICE> FindAllBySite(string nameOrCode, string gbGukgo, bool isParent)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("  SELECT B.ID AS SITE_ID, B.NAME AS SITE_NAME, B.GBGUKGO, A.CONTRACTDATE,  A.contractstartdate , A.contractenddate , C.* FROM HIC_OSHA_CONTRACT A     ");
            parameter.AppendSql("INNER JOIN HC_SITE_VIEW B                                                                   ");
            parameter.AppendSql("ON A.OSHA_SITE_ID = B.ID                                                                    ");
            parameter.AppendSql("AND A.ISCONTRACT = 'Y'                                                                      ");
            parameter.AppendSql("INNER JOIN HIC_OSHA_SITE BB                                                                 ");
            parameter.AppendSql("ON B.ID = BB.ID                                                                             ");
            parameter.AppendSql("INNER JOIN HIC_OSHA_PRICE C                                                                 ");
            parameter.AppendSql("ON C.ESTIMATE_ID = A.ESTIMATE_ID                                                            ");
            parameter.AppendSql("AND C.ISDELETED = 'N'                                                                       ");
            parameter.AppendSql("INNER JOIN(                                                                                 ");
            parameter.AppendSql("            SELECT MAX(A.ID) AS ID FROM  HIC_OSHA_PRICE A                                   ");
            parameter.AppendSql("            INNER JOIN HIC_OSHA_CONTRACT B                                                  ");
            parameter.AppendSql("            ON A.ESTIMATE_ID = B.ESTIMATE_ID                                                ");
            parameter.AppendSql("            WHERE A.ISDELETED= 'N'                                                          ");
            parameter.AppendSql("            AND B.ISDELETED= 'N'                                                            ");
            parameter.AppendSql("            AND B.ISCONTRACT = 'Y'                                                          ");
            parameter.AppendSql("            AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("            AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("            GROUP BY B.OSHA_SITE_ID                                                         ");
            parameter.AppendSql("            ) D                                                                             ");
            parameter.AppendSql("ON C.ID = D.ID                                                                              ");
            parameter.AppendSql("WHERE 1 = 1                                                                           ");
            parameter.AppendSql("AND BB.ISACTIVE = 'Y'                                                                 ");
            parameter.AppendSql("AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("AND BB.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("AND C.SWLICENSE = :SWLICENSE ");
            if (nameOrCode.NotEmpty())
            {
                if (nameOrCode.IsNumeric())
                {
                    parameter.AppendSql("AND B.ID = :SITE_ID                                                                              ");
                }
                else
                {
                    parameter.AppendSql("AND B.NAME LIKE :NAME                                         ");
                }
            }
            if(gbGukgo == "Y")
            {
                parameter.AppendSql("AND B.GBGUKGO = 'Y'                                                          ");
            }
            if (nameOrCode.NotEmpty())
            {
                if (nameOrCode.IsNumeric())
                {
                    parameter.Add("SITE_ID", nameOrCode.To<long>(0));
                }
                else
                {
                    parameter.AddLikeStatement("NAME", nameOrCode);
                }
            }

            if (isParent)
            {
                parameter.AppendSql("AND BB.ISPARENTCHARGE = 'Y' ");
            }
            parameter.AppendSql("ORDER BY B.NAME, B.ID                                                                       ");
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<OSHA_PRICE>(parameter);
        }

        public OSHA_PRICE FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID");
            parameter.AppendSql("     , A.ESTIMATE_ID");
            parameter.AppendSql("     , A.WORKERTOTALCOUNT");
            parameter.AppendSql("     , A.UNITPRICE");
            parameter.AppendSql("     , A.UNITTOTALPRICE");
            parameter.AppendSql("     , A.TOTALPRICE");
            parameter.AppendSql("     , A.ISFIX");
            parameter.AppendSql("     , A.ISBILL");
            parameter.AppendSql("     , A.ISDELETED");
            parameter.AppendSql("     , A.MODIFIED");
            parameter.AppendSql("     , B.NAME AS MODIFIEDUSER");
            parameter.AppendSql("     , A.CREATED");
            parameter.AppendSql("     , A.CREATEDUSER");
            parameter.AppendSql("  FROM HIC_OSHA_PRICE A");
            parameter.AppendSql("  INNER JOIN HIC_USERS B");
            parameter.AppendSql("ON A.MODIFIEDUSER = B.USERID");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);

            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<OSHA_PRICE>(parameter);
        }

        public OSHA_PRICE FindMaxIdByEstimate(long estimateId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID");
            parameter.AppendSql("     , A.ESTIMATE_ID");
            parameter.AppendSql("     , A.WORKERTOTALCOUNT");
            parameter.AppendSql("     , A.UNITPRICE");
            parameter.AppendSql("     , A.UNITTOTALPRICE");
            parameter.AppendSql("     , A.TOTALPRICE");
            parameter.AppendSql("     , A.ISFIX");
            parameter.AppendSql("     , A.ISBILL");
            parameter.AppendSql("     , A.ISDELETED");
            parameter.AppendSql("     , A.MODIFIED");
            parameter.AppendSql("     , B.NAME AS MODIFIEDUSER");
            parameter.AppendSql("     , A.CREATED");
            parameter.AppendSql("     , A.CREATEDUSER");
            parameter.AppendSql("  FROM HIC_OSHA_PRICE A");
            parameter.AppendSql("  INNER JOIN HIC_USERS B");
            parameter.AppendSql("  ON A.MODIFIEDUSER = B.USERID");
            parameter.AppendSql("  WHERE ID = ( SELECT max(id) FROM HIC_OSHA_PRICE WHERE ESTIMATE_ID = :ID AND ISDELETED ='N') ");
            parameter.AppendSql("    AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("    AND B.SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", estimateId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<OSHA_PRICE>(parameter);
        }

        internal OSHA_PRICE FindMaxIdBySite(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID                                                         ");
            parameter.AppendSql("     , A.ESTIMATE_ID                                                ");
            parameter.AppendSql("     , A.WORKERTOTALCOUNT                                           ");
            parameter.AppendSql("     , A.UNITPRICE                                                  ");
            parameter.AppendSql("     , A.UNITTOTALPRICE                                             ");
            parameter.AppendSql("     , A.TOTALPRICE                                                 ");
            parameter.AppendSql("     , A.ISFIX                                                      ");
            parameter.AppendSql("     , A.ISBILL                                                     ");
            parameter.AppendSql("     , A.ISDELETED                                                  ");
            parameter.AppendSql("     , A.MODIFIED                                                   ");
            parameter.AppendSql("     , B.NAME AS MODIFIEDUSER                                       ");
            parameter.AppendSql("     , A.CREATED                                                    ");
            parameter.AppendSql("     , A.CREATEDUSER                                                ");
            parameter.AppendSql("  FROM HIC_OSHA_PRICE A                                             ");
            parameter.AppendSql("  INNER JOIN HIC_USERS B                                            ");
            parameter.AppendSql("          ON A.MODIFIEDUSER = B.USERID                              ");
            parameter.AppendSql(" WHERE 1 = 1                                                        ");
            parameter.AppendSql("   AND ID = (SELECT MAX(ID)                                         ");
            parameter.AppendSql("               FROM HIC_OSHA_PRICE                                  ");
            parameter.AppendSql("              WHERE 1 = 1                                           ");
            parameter.AppendSql("                AND SWLICENSE = :SWLICENSE                         ");
            parameter.AppendSql("                AND ISDELETED = 'N'                                 ");
            parameter.AppendSql("                AND ESTIMATE_ID IN (SELECT ID FROM HIC_OSHA_ESTIMATE");
            parameter.AppendSql("                                     WHERE OSHA_SITE_ID = :SITE_ID) ");
            parameter.AppendSql("            )                                                       ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<OSHA_PRICE>(parameter);
        }

        public OSHA_PRICE FindMaxIdBySiteId(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID");
            parameter.AppendSql("     , A.ESTIMATE_ID");
            parameter.AppendSql("     , A.WORKERTOTALCOUNT");
            parameter.AppendSql("     , A.UNITPRICE");
            parameter.AppendSql("     , A.UNITTOTALPRICE");
            parameter.AppendSql("     , A.TOTALPRICE");
            parameter.AppendSql("     , A.ISFIX");
            parameter.AppendSql("     , A.ISBILL");
            parameter.AppendSql("     , A.ISDELETED");
            parameter.AppendSql("     , A.MODIFIED");
            parameter.AppendSql("     , B.NAME AS MODIFIEDUSER");
            parameter.AppendSql("     , A.CREATED");
            parameter.AppendSql("     , A.CREATEDUSER");
            parameter.AppendSql("  FROM HIC_OSHA_PRICE A");
            parameter.AppendSql("  INNER JOIN HIC_USERS B");
            parameter.AppendSql("  ON A.MODIFIEDUSER = B.USERID");
            parameter.AppendSql("  WHERE ID = ( SELECT MAX(A.Id) FROM HIC_OSHA_PRICE A    ");
            parameter.AppendSql("               INNER JOIN HIC_OSHA_ESTIMATE B   ");
            parameter.AppendSql("               ON A.ESTIMATE_ID = B.ID   ");
            parameter.AppendSql("               WHERE B.OSHA_SITE_ID = :SITE_ID   ");
            parameter.AppendSql("               AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("               AND A.ISDELETED = 'N'   ");
            parameter.AppendSql("               AND B.ISDELETED = 'N'    ) ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE ");
            parameter.Add("SITE_ID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<OSHA_PRICE>(parameter);
        }
        public List<OSHA_PRICE> FindAllByEstimateId(long estimateId, string isDeleted = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID");
            parameter.AppendSql("     , A.ESTIMATE_ID");
            parameter.AppendSql("     , A.WORKERTOTALCOUNT");
            parameter.AppendSql("     , A.UNITPRICE");
            parameter.AppendSql("     , A.UNITTOTALPRICE");
            parameter.AppendSql("     , A.TOTALPRICE");
            parameter.AppendSql("     , A.ISFIX");
            parameter.AppendSql("     , A.ISBILL");
            parameter.AppendSql("     , A.ISDELETED");
            parameter.AppendSql("     , A.MODIFIED");
            parameter.AppendSql("     , B.NAME AS MODIFIEDUSER");
            parameter.AppendSql("     , A.CREATED");
            parameter.AppendSql("     , A.CREATEDUSER");
            parameter.AppendSql("     , D.ISPARENTCHARGE");
            parameter.AppendSql("     , D.ISQUARTERCHARGE");
            parameter.AppendSql("  FROM HIC_OSHA_PRICE A        ");
            parameter.AppendSql("  INNER JOIN HIC_USERS B       ");
            parameter.AppendSql("  ON A.MODIFIEDUSER = B.USERID  ");
            parameter.AppendSql("  INNER JOIN HIC_OSHA_ESTIMATE C    ");
            parameter.AppendSql("  ON A.ESTIMATE_ID = C.ID  ");
            parameter.AppendSql("  INNER JOIN HIC_OSHA_SITE D    ");
            parameter.AppendSql("  ON C.OSHA_SITE_ID = D.ID  ");
            parameter.AppendSql("  WHERE ESTIMATE_ID = :ID ");
            parameter.AppendSql("    AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("    AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("    AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("    AND D.SWLICENSE = :SWLICENSE ");

            if (isDeleted.NotEmpty())
            {
                parameter.AppendSql("  AND ISDELETED = :ISDELETED ");
            }

            parameter.AppendSql("  ORDER BY A.ID DESC ");
            parameter.Add("ID", estimateId);
            if (isDeleted.NotEmpty())
            {
                parameter.Add("ISDELETED", isDeleted);
            }
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<OSHA_PRICE>(parameter);
        }
        public List<OSHA_PRICE> FindAllByParent(long parentSiteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID as SITE_ID, C.ID AS ESTIMATE_ID, B.NAME AS SITE_NAME, D.ID, D.WORKERTOTALCOUNT,");
            parameter.AppendSql("       UNITPRICE, UNITTOTALPRICE, TOTALPRICE, ISBILL, ISFIX ");
            parameter.AppendSql("  FROM HIC_OSHA_SITE A ");
            parameter.AppendSql("       INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("             ON A.ID = B.ID ");
            parameter.AppendSql("             AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("       INNER JOIN HIC_OSHA_ESTIMATE C ");
            parameter.AppendSql("             ON A.ID = C.OSHA_SITE_ID ");
            parameter.AppendSql("             AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("       LEFT OUTER JOIN (SELECT A.* FROM HIC_OSHA_PRICE A ");
            parameter.AppendSql("                               INNER JOIN (SELECT MAX(A.ID) AS ID ");
            parameter.AppendSql("                                             FROM  HIC_OSHA_PRICE A ");
            parameter.AppendSql("                                                   INNER JOIN HIC_OSHA_ESTIMATE B ");
            parameter.AppendSql("                                                         ON A.ESTIMATE_ID = B.ID ");
            parameter.AppendSql("                                                         AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("                                             WHERE A.ISDELETED = 'N' ");
            parameter.AppendSql("                                               AND B.ISDELETED ='N' ");
            parameter.AppendSql("                                               AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("                                            GROUP BY B.OSHA_SITE_ID) BB ");
            parameter.AppendSql("                                    ON A.ID = BB.ID) D ");
            parameter.AppendSql("            ON D.ESTIMATE_ID = C.ID ");
            parameter.AppendSql("      INNER JOIN HIC_OSHA_CONTRACT E ");
            parameter.AppendSql("            ON e.estimate_id = C.ID ");
            parameter.AppendSql("            ANd E.OSHA_SITE_ID = C.OSHA_SITE_ID ");
            parameter.AppendSql("            AND E.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" WHERE A.PARENTSITE_ID = :PARENTSITE_ID ");
            parameter.AppendSql("    AND A.ISACTIVE = 'Y' ");
            parameter.AppendSql("    AND C.ISDELETED = 'N' ");
            parameter.AppendSql("    AND E.ISDELETED = 'N' ");
            parameter.AppendSql("    AND A.ISPARENTCHARGE = 'N' ");
            parameter.AppendSql("    AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  ORDER BY B.NAME ");
            
            parameter.Add("PARENTSITE_ID", parentSiteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<OSHA_PRICE>(parameter);
        }
        public OSHA_PRICE Insert(OSHA_PRICE dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_PRICE_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_PRICE");
            parameter.AppendSql("(");
            parameter.AppendSql("    ID");
            parameter.AppendSql("  , ESTIMATE_ID");
            parameter.AppendSql("  , WORKERTOTALCOUNT");
            parameter.AppendSql("  , UNITPRICE");
            parameter.AppendSql("  , UNITTOTALPRICE");
            parameter.AppendSql("  , TOTALPRICE");
            parameter.AppendSql("  , ISFIX");
            parameter.AppendSql("  , ISBILL");
            parameter.AppendSql("  , ISDELETED");
            parameter.AppendSql("  , MODIFIED");
            parameter.AppendSql("  , MODIFIEDUSER");
            parameter.AppendSql("  , CREATED");
            parameter.AppendSql("  , CREATEDUSER");
            parameter.AppendSql("  , SWLICENSE");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :ID");
            parameter.AppendSql("  , :ESTIMATE_ID");
            parameter.AppendSql("  , :WORKERTOTALCOUNT");
            parameter.AppendSql("  , :UNITPRICE");
            parameter.AppendSql("  , :UNITTOTALPRICE");
            parameter.AppendSql("  , :TOTALPRICE");
            parameter.AppendSql("  , :ISFIX");
            parameter.AppendSql("  , :ISBILL");
            parameter.AppendSql("  , 'N'");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :MODIFIEDUSER");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :CREATEDUSER");
            parameter.AppendSql("  , :SWLICENSE");
            parameter.AppendSql(") ");

            parameter.Add("ID", dto.ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("WORKERTOTALCOUNT", dto.WORKERTOTALCOUNT);
            parameter.Add("UNITPRICE", dto.UNITPRICE);
            parameter.Add("UNITTOTALPRICE", dto.UNITTOTALPRICE);
            parameter.Add("TOTALPRICE", dto.TOTALPRICE);
            parameter.Add("ISFIX", dto.ISFIX);
            parameter.Add("ISBILL", dto.ISBILL);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
          
           // DataSyncService.Instance.Insert("HIC_OSHA_PRICE", dto.ID);

            return FindOne(dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_PRICE");
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

          //  DataSyncService.Instance.Delete("HIC_OSHA_PRICE", id);

        }
    }
}
