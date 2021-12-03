namespace HC.OSHA.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using HC.Core.Dto;
    using HC.Core.Service;
    using HC.OSHA.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicOshaSpecialResultRepository : BaseRepository
    {
        public List<HIC_OSHA_SPECIAL_RESULT> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT YEAR                                         ");
            parameter.AppendSql("     , SITE_ID                                      ");
            parameter.AppendSql("     , D2COUNT                                      ");
            parameter.AppendSql("     , C2COUNT                                      ");
            parameter.AppendSql("     , D1COUNT                                      ");
            parameter.AppendSql("     , C1COUNT                                      ");
            parameter.AppendSql("     , DNCOUNT                                      ");
            parameter.AppendSql("     , CNCOUNT                                      ");
            parameter.AppendSql("     , CREATED                                      ");
            parameter.AppendSql("     , TOTALCOUNT                                   ");
            parameter.AppendSql("     , (                                            ");
            parameter.AppendSql("            SELECT MIN(JEPDATE)                     ");
            parameter.AppendSql("              FROM HIC_JEPSU                        ");
            parameter.AppendSql("             WHERE LTDCODE = A.SITE_ID              ");
            parameter.AppendSql("               AND TO_CHAR(JEPDATE, 'YYYY') = A.YEAR");
            parameter.AppendSql("       ) AS JEPDATE                                 ");
            parameter.AppendSql("  FROM HIC_OSHA_SPECIAL_RESULT A                    ");
            parameter.AppendSql(" WHERE SITE_ID = :SITE_ID                           ");
            parameter.AppendSql("ORDER BY YEAR DESC                                  ");
            
            parameter.Add("SITE_ID", siteId);

            return ExecuteReader<HIC_OSHA_SPECIAL_RESULT>(parameter);

        }

        public void Delete(string YEAR, long SITE_ID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_SPECIAL_RESULT   ");
            parameter.AppendSql("   WHERE SITE_ID = :SITE_ID     ");
            parameter.AppendSql("   AND YEAR = :YEAR   ");

            parameter.Add("SITE_ID", SITE_ID);
            parameter.Add("YEAR", YEAR);
            ExecuteNonQuery(parameter);
        }

        public void Insert(HIC_OSHA_SPECIAL_RESULT dto)
        {

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_SPECIAL_RESULT ");
            parameter.AppendSql("(");
            parameter.AppendSql("   YEAR");
            parameter.AppendSql("  , SITE_ID");
            parameter.AppendSql("  , D2COUNT");
            parameter.AppendSql("  , C2COUNT");
            parameter.AppendSql("  , D1COUNT");
            parameter.AppendSql("  , C1COUNT");
            parameter.AppendSql("  , DNCOUNT");
            parameter.AppendSql("  , CNCOUNT");
            parameter.AppendSql("  , TOTALCOUNT");
            parameter.AppendSql("  , CREATED");

            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("   :YEAR");
            parameter.AppendSql("  , :SITE_ID");
            parameter.AppendSql("  , :D2COUNT");
            parameter.AppendSql("  , :C2COUNT");
            parameter.AppendSql("  , :D1COUNT");
            parameter.AppendSql("  , :C1COUNT");
            parameter.AppendSql("  , :DNCOUNT");
            parameter.AppendSql("  , :CNCOUNT");
            parameter.AppendSql("  , :TOTALCOUNT");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql(") ");

            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("D2COUNT", dto.D2COUNT);
            parameter.Add("C2COUNT", dto.C2COUNT);
            parameter.Add("D1COUNT", dto.D1COUNT);
            parameter.Add("C1COUNT", dto.C1COUNT);
            parameter.Add("DNCOUNT", dto.DNCOUNT);
            parameter.Add("CNCOUNT", dto.CNCOUNT);
            parameter.Add("TOTALCOUNT", dto.TOTALCOUNT);
            ExecuteNonQuery(parameter);
        }

        internal List<HIC_OSHA_SPECIAL_RESULT> FindAllNew(long siteId, string startYear, string endYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("WITH TOTAL AS(                                                                 ");
            parameter.AppendSql("    SELECT COUNT(AA.WRTNO) AS TOTALCOUNT                                       ");
            parameter.AppendSql("         , AA.LTDCODE                                                          ");
            parameter.AppendSql("         , AA.GJYEAR                                                           ");
            parameter.AppendSql("      FROM ADMIN.HIC_JEPSU AA                                            ");
            parameter.AppendSql("      LEFT OUTER JOIN ADMIN.HIC_RES_SPECIAL BB                           ");
            parameter.AppendSql("                   ON AA.WRTNO = BB.WRTNO                                      ");
            parameter.AppendSql("     WHERE 1 = 1                                                               ");
            parameter.AppendSql("       AND AA.JEPDATE >= TO_DATE(:START_DATE, 'YYYY-MM-DD')                    ");
            parameter.AppendSql("       AND AA.JEPDATE <= TO_DATE(:END_DATE, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("       AND AA.DELDATE IS NULL                                                  ");
            parameter.AppendSql("       AND AA.LTDCODE = :SITE_ID                                               ");
            parameter.AppendSql("       AND AA.UCODES IS NOT NULL                                               ");
            parameter.AppendSql("       AND AA.GJJONG IN ('11','12','14','23','41','42')                        ");
            parameter.AppendSql("       AND BB.PANJENGDRNO IS NOT NULL                                          ");
            parameter.AppendSql("      GROUP BY AA.LTDCODE, AA.GJYEAR                                           ");
            parameter.AppendSql(")                                                                              ");
            parameter.AppendSql("SELECT A.YEAR                                                                  ");
            parameter.AppendSql("     , A.SITE_ID                                                               ");
            parameter.AppendSql("     , B.TOTALCOUNT                                                            ");
            parameter.AppendSql("     , SUM(NVL(CASE WHEN A.GBN = 'A1' THEN CNT END, 0)) AS D1COUNT             ");
            parameter.AppendSql("     , SUM(NVL(CASE WHEN A.GBN = 'A2' THEN CNT END, 0)) AS DNCOUNT             ");
            parameter.AppendSql("     , SUM(NVL(CASE WHEN A.GBN = 'A3' THEN CNT END, 0)) AS D2COUNT             ");
            parameter.AppendSql("     , SUM(NVL(CASE WHEN A.GBN = 'B1' THEN CNT END, 0)) AS C1COUNT             ");
            parameter.AppendSql("     , SUM(NVL(CASE WHEN A.GBN = 'B2' THEN CNT END, 0)) AS CNCOUNT             ");
            parameter.AppendSql("     , SUM(NVL(CASE WHEN A.GBN = 'B3' THEN CNT END, 0)) AS C2COUNT             ");
            parameter.AppendSql("  FROM                                                                         ");
            parameter.AppendSql("  (                                                                            ");
            parameter.AppendSql("        SELECT A.PANJENG                                                       ");
            parameter.AppendSql("             , A.GBN                                                           ");
            parameter.AppendSql("             , A.YEAR                                                          ");
            parameter.AppendSql("             , A.SITE_ID                                                       ");
            parameter.AppendSql("             , COUNT(A.GBN) AS CNT                                             ");
            parameter.AppendSql("          FROM                                                                 ");
            parameter.AppendSql("          (                                                                    ");
            parameter.AppendSql("                SELECT DISTINCT B.PANJENG                                      ");
            parameter.AppendSql("                     , B.SAHUCODE                                              ");
            parameter.AppendSql("                     , CASE WHEN B.PANJENG = '5' THEN 'A1'                     ");
            parameter.AppendSql("                            WHEN B.PANJENG = 'A' THEN 'A2'                     ");
            parameter.AppendSql("                            WHEN B.PANJENG = '6' THEN 'A3'                     ");
            parameter.AppendSql("                            WHEN B.PANJENG = '3' THEN 'B1'                     ");
            parameter.AppendSql("                            WHEN B.PANJENG = '9' THEN 'B2'                     ");
            parameter.AppendSql("                            WHEN B.PANJENG = '4' THEN 'B3'                     ");
            parameter.AppendSql("                       END AS GBN                                              ");
            parameter.AppendSql("                     , A.LTDCODE AS SITE_ID                                    ");
            parameter.AppendSql("                     , A.WRTNO                                                 ");
            parameter.AppendSql("                     , A.YEAR                                                  ");
            parameter.AppendSql("                  FROM                                                         ");
            parameter.AppendSql("                  (                                                            ");
            parameter.AppendSql("                        SELECT A.LTDCODE                                       ");
            parameter.AppendSql("                             , A.WRTNO                                         ");
            parameter.AppendSql("                             , TO_CHAR(A.JEPDATE, 'YYYY') AS YEAR              ");
            parameter.AppendSql("                          FROM ADMIN.HIC_JEPSU A                         ");
            parameter.AppendSql("                          LEFT OUTER JOIN ADMIN.HIC_RES_SPECIAL B        ");
            parameter.AppendSql("                                       ON A.WRTNO = B.WRTNO                    ");
            parameter.AppendSql("                         WHERE 1 = 1                                           ");
            parameter.AppendSql("                           AND A.JEPDATE >= TO_DATE(:START_DATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("                           AND A.JEPDATE <= TO_DATE(:END_DATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("                           AND A.DELDATE IS NULL                               ");
            parameter.AppendSql("                           AND A.LTDCODE = :SITE_ID                            ");
            parameter.AppendSql("                           AND A.UCODES IS NOT NULL                            ");
            parameter.AppendSql("                           AND A.GJYEAR BETWEEN :START_YEAR                    ");
            parameter.AppendSql("                                            AND :END_YEAR                      ");
            parameter.AppendSql("                           AND A.GJJONG IN ('11','12','14','23','41','42')     ");
            parameter.AppendSql("                           AND B.PANJENGDRNO IS NOT NULL                       ");
            parameter.AppendSql("                         ORDER BY A.WRTNO                                      ");
            parameter.AppendSql("                  ) A LEFT OUTER JOIN ADMIN.HIC_SPC_PANJENG B            ");
            parameter.AppendSql("                                 ON A.LTDCODE = B.LTDCODE                      ");
            parameter.AppendSql("                               AND A.WRTNO   = B.WRTNO                         ");
            parameter.AppendSql("                               AND B.PANJENG IN ('3','4','5','6','9','A')      ");
            parameter.AppendSql("                               AND B.DELDATE IS NULL                           ");
            parameter.AppendSql("          ) A                                                                  ");
            parameter.AppendSql("         WHERE A.PANJENG IS NOT NULL                                           ");
            parameter.AppendSql("        GROUP BY A.PANJENG, A.GBN, A.YEAR, A.SITE_ID                           ");
            parameter.AppendSql("        ORDER BY A.YEAR, A.GBN                                                 ");
            parameter.AppendSql("  ) A LEFT OUTER JOIN TOTAL B                                                  ");
            parameter.AppendSql("                   ON A.SITE_ID = B.LTDCODE                                    ");
            parameter.AppendSql("                  AND A.YEAR = B.GJYEAR                                        ");
            parameter.AppendSql("GROUP BY A.YEAR, A.SITE_ID, B.TOTALCOUNT                                       ");
            parameter.AppendSql("ORDER BY A.YEAR DESC                                                           ");

            parameter.Add("START_DATE", string.Concat(startYear, "-01-01"));
            parameter.Add("END_DATE", string.Concat(endYear, "-12-01"));
            parameter.Add("START_YEAR", startYear);
            parameter.Add("END_YEAR", endYear);
            parameter.Add("SITE_ID", siteId);

            return ExecuteReader<HIC_OSHA_SPECIAL_RESULT>(parameter);
        }
    }
}


