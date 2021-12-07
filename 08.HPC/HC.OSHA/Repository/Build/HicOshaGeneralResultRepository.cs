namespace HC.OSHA.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using HC.Core.Dto;
    using HC.Core.Service;
    using HC.OSHA.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicOshaGeneralResultRepository : BaseRepository
    {
        public List<HIC_OSHA_GENEAL_RESULT> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT YEAR                                         ");
            parameter.AppendSql("     , SITE_ID                                      ");
            parameter.AppendSql("     , D2COUNT                                      ");
            parameter.AppendSql("     , C2COUNT                                      ");
            parameter.AppendSql("     , CREATED                                      ");
            parameter.AppendSql("     , TOTALCOUNT                                   ");
            parameter.AppendSql("     , STARTDATE                                    ");
            parameter.AppendSql("     , (                                            ");
            parameter.AppendSql("            SELECT MIN(JEPDATE)                     ");
            parameter.AppendSql("              FROM HIC_JEPSU                        ");
            parameter.AppendSql("             WHERE LTDCODE = A.SITE_ID              ");
            parameter.AppendSql("               AND TO_CHAR(JEPDATE, 'YYYY') = A.YEAR");
            parameter.AppendSql("       ) AS JEPDATE                                 ");
            parameter.AppendSql("  FROM HIC_OSHA_GENEAL_RESULT A                     ");
            parameter.AppendSql(" WHERE SITE_ID = :SITE_ID                           ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE                       ");
            parameter.AppendSql("ORDER BY YEAR DESC                                  ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HIC_OSHA_GENEAL_RESULT>(parameter);
        }

        public List<HIC_OSHA_GENEAL_RESULT> FindAllNew(long siteId, string startYear, string endYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.YEAR                                                                                                             ");
            parameter.AppendSql("     , COUNT(*) AS TOTALCOUNT                                                                                             ");
            parameter.AppendSql("     , SUM(D2) AS D2COUNT                                                                                                 ");
            parameter.AppendSql("     , SUM(C2) AS C2COUNT                                                                                                 ");
            parameter.AppendSql("  FROM                                                                                                                    ");
            parameter.AppendSql("  (                                                                                                                       ");
            parameter.AppendSql("        SELECT A.YEAR                                                                                                     ");
            parameter.AppendSql("             , A.SITE_ID                                                                                                  ");
            parameter.AppendSql("             , CASE WHEN     BROWID IS NOT NULL                                                                           ");
            parameter.AppendSql("                        AND (R1 IS NOT NULL OR R2 IS NOT NULL OR D IS NOT NULL OR D2 IS NOT NULL) THEN                    ");
            parameter.AppendSql("                    CASE WHEN   (R2 IS NOT NULL AND CYCLE_RES = '3')                                                      ");
            parameter.AppendSql("                            OR  (R2 IS NOT NULL AND DIABETES_RES = '3')                                                   ");
            parameter.AppendSql("                            OR  D2 IS NOT NULL                                                                            ");
            parameter.AppendSql("                            OR  D1 IS NOT NULL                                                                            ");
            parameter.AppendSql("                            OR  D IS NOT NULL THEN                                                                        ");
            parameter.AppendSql("                        1                                                                                                 ");
            parameter.AppendSql("                    END                                                                                                   ");
            parameter.AppendSql("               END AS D2                                                                                                  ");
            parameter.AppendSql("             , R1, R2, CYCLE_RES, DIABETES_RES                                                                            ");
            parameter.AppendSql("             , CASE WHEN    R1 IS NOT NULL                                                                                ");
            parameter.AppendSql("                        OR (R2 IS NOT NULL AND CYCLE_RES = '2')                                                           ");
            parameter.AppendSql("                        OR (R2 IS NOT NULL AND DIABETES_RES = '2')                                                        ");
            parameter.AppendSql("               THEN 1 END AS C2                                                                                           ");
            parameter.AppendSql("          FROM                                                                                                            ");
            parameter.AppendSql("          (                                                                                                               ");
            parameter.AppendSql("                SELECT TO_CHAR(A.JEPDATE, 'YYYY') AS YEAR                                                                 ");
            parameter.AppendSql("                     , A.LTDCODE AS SITE_ID                                                                               ");
            parameter.AppendSql("                     , B.ROWID AS BROWID                                                                                  ");
            parameter.AppendSql("                     , CASE WHEN    (B.PANJENGR1  = '1' AND (B.PANJENGD21 || B.PANJENGD22 || B.PANJENGD23 NOT LIKE '%J%'))");
            parameter.AppendSql("                                OR  (B.PANJENGR2  = '1' AND (B.PANJENGD21 || B.PANJENGD22 || B.PANJENGD23 NOT LIKE '%J%'))");
            parameter.AppendSql("                                OR  (B.PANJENGR4  = '1' AND (B.PANJENGD21 || B.PANJENGD22 || B.PANJENGD23 NOT LIKE '%E%'))");
            parameter.AppendSql("                                OR  (B.PANJENGR5  = '1' AND (B.PANJENGD21 || B.PANJENGD22 || B.PANJENGD23 NOT LIKE '%K%'))");
            parameter.AppendSql("                                OR  B.PANJENGR7  = '1'                                                                    ");
            parameter.AppendSql("                                OR  (B.PANJENGR8  = '1' AND (B.PANJENGD21 || B.PANJENGD22 || B.PANJENGD23 NOT LIKE '%D%'))");
            parameter.AppendSql("                                OR  B.PANJENGR9  = '1'                                                                    ");
            parameter.AppendSql("                                OR  B.PANJENGR10  = '1'                                                                   ");
            parameter.AppendSql("                       THEN '1' END AS R1                                                                                 ");
            parameter.AppendSql("                     , CASE WHEN    B.PANJENGR3  = '1'                                                                    ");
            parameter.AppendSql("                                OR  B.PANJENGR6  = '1'                                                                    ");
            parameter.AppendSql("                       THEN '1' END AS R2                                                                                 ");
            parameter.AppendSql("                     , CASE WHEN    TRIM(B.PANJENGD11) IS NOT NULL                                                        ");
            parameter.AppendSql("                                OR  TRIM(B.PANJENGD12) IS NOT NULL                                                        ");
            parameter.AppendSql("                                OR  TRIM(B.PANJENGD13) IS NOT NULL                                                        ");
            parameter.AppendSql("                       THEN '1' END AS D1 -- D1                                                                           ");
            parameter.AppendSql("                     , CASE WHEN    TRIM(B.PANJENGD21) IS NOT NULL                                                        ");
            parameter.AppendSql("                                OR  TRIM(B.PANJENGD22) IS NOT NULL                                                        ");
            parameter.AppendSql("                                OR  TRIM(B.PANJENGD23) IS NOT NULL                                                        ");
            parameter.AppendSql("                       THEN '1' END AS D2 -- D2                                                                           ");
            parameter.AppendSql("                     , CASE WHEN    B.PANJENGU1 = '1'                                                                     ");
            parameter.AppendSql("                                OR  B.PANJENGU2 = '1'                                                                     ");
            parameter.AppendSql("                                OR  B.PANJENGU3 = '1'                                                                     ");
            parameter.AppendSql("                                OR  B.PANJENGU4 = '1'                                                                     ");
            parameter.AppendSql("                       THEN '1' END AS D -- D                                                                             ");
            parameter.AppendSql("                     , CASE WHEN NVL(C.PANJENGDRNO, 0) = 0 THEN ''                                                        ");
            parameter.AppendSql("                            ELSE C.CYCLE_RES                                                                              ");
            parameter.AppendSql("                       END AS CYCLE_RES                                                                                   ");
            parameter.AppendSql("                     , CASE WHEN NVL(C.PANJENGDRNO, 0) = 0 THEN ''                                                        ");
            parameter.AppendSql("                            ELSE C.DIABETES_RES                                                                           ");
            parameter.AppendSql("                       END DIABETES_RES                                                                                   ");
            parameter.AppendSql("                     , NVL(B.PANJENGDRNO, 0) AS PANJENGDRNO                                                               ");
            parameter.AppendSql("                  FROM ADMIN.HIC_JEPSU A                                                                            ");
            parameter.AppendSql("                  LEFT OUTER JOIN ADMIN.HIC_RES_BOHUM1 B                                                            ");
            parameter.AppendSql("                               ON A.WRTNO = B.WRTNO                                                                       ");
            parameter.AppendSql("                  LEFT OUTER JOIN ADMIN.HIC_RES_BOHUM2 C                                                            ");
            parameter.AppendSql("                               ON A.WRTNO = C.WRTNO                                                                       ");
            parameter.AppendSql("                 WHERE 1 = 1                                                                                              ");
            parameter.AppendSql("                   AND A.JEPDATE BETWEEN TO_DATE(:START_DATE, 'YYYY-MM-DD')                                               ");
            parameter.AppendSql("                                     AND TO_DATE(:END_DATE, 'YYYY-MM-DD')                                                 ");
            parameter.AppendSql("                   AND A.GJCHASU = '1'                                                                                    ");
            parameter.AppendSql("                   AND A.LTDCODE = :SITE_ID                                                                               ");
            parameter.AppendSql("                   AND A.DELDATE IS NULL                                                                                  ");
            parameter.AppendSql("                   AND A.GBINWON IN ('21', '22', '23', '31', '32', '64', '65', '66', '67', '68')                          ");
            parameter.AppendSql("          ) A                                                                                                             ");
            parameter.AppendSql("  ) A                                                                                                                     ");
            parameter.AppendSql("GROUP BY A.YEAR                                                                                                           ");
            parameter.AppendSql("ORDER BY A.YEAR DESC                                                                                                      ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("START_DATE", string.Concat(startYear, "-01-01"));
            parameter.Add("END_DATE", string.Concat(endYear, "-12-31"));

            return ExecuteReader<HIC_OSHA_GENEAL_RESULT>(parameter);
        }

        internal DateTime? FindByMinJepDate(long siteId, string year)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MIN(JEPDATE) AS JEPDATE                                                  ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU A                                                  ");
            parameter.AppendSql(" WHERE 1 = 1                                                                    ");
            parameter.AppendSql("   AND A.JEPDATE BETWEEN TO_DATE(:START_DATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("                     AND TO_DATE(:END_DATE, 'YYYY-MM-DD')                       ");
            parameter.AppendSql("   AND A.GJCHASU = '1'                                                          ");
            parameter.AppendSql("   AND A.LTDCODE = :SITE_ID                                                     ");
            parameter.AppendSql("   AND A.DELDATE IS NULL                                                        ");
            parameter.AppendSql("   AND A.GBINWON IN ('21', '22', '23', '31', '32', '64', '65', '66', '67', '68')");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("START_DATE", string.Concat(year, "-01-01"));
            parameter.Add("END_DATE", string.Concat(year, "-12-01"));

            return ExecuteScalar<DateTime?>(parameter);
        }

        internal DateTime? FindBySpecialMinJepDate(long siteId, string year)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MIN(JEPDATE) AS JEPDATE                                 ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU A                                 ");
            parameter.AppendSql(" WHERE 1 = 1                                                   ");
            parameter.AppendSql("   AND A.JEPDATE BETWEEN TO_DATE(:START_DATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("                     AND TO_DATE(:END_DATE, 'YYYY-MM-DD')      ");
            parameter.AppendSql("   AND A.UCODES IS NOT NULL                                    ");
            parameter.AppendSql("   AND A.LTDCODE = :SITE_ID                                    ");
            parameter.AppendSql("   AND A.DELDATE IS NULL                                       ");
            parameter.AppendSql("   AND A.GJJONG IN ('11','12','14','23','41','42')             ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("START_DATE", string.Concat(year, "-01-01"));
            parameter.Add("END_DATE", string.Concat(year, "-12-01"));

            return ExecuteScalar<DateTime?>(parameter);
        }

        public void Delete(string YEAR, long SITE_ID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_GENEAL_RESULT   ");
            parameter.AppendSql("   WHERE SITE_ID = :SITE_ID     ");
            parameter.AppendSql("   AND YEAR = :YEAR   ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("SITE_ID", SITE_ID);
            parameter.Add("YEAR", YEAR);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);
        }

        public void Insert(HIC_OSHA_GENEAL_RESULT dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_GENEAL_RESULT ");
            parameter.AppendSql("(");
            parameter.AppendSql("   YEAR");
            parameter.AppendSql("  , SITE_ID");
            parameter.AppendSql("  , D2COUNT");
            parameter.AppendSql("  , C2COUNT");
            parameter.AppendSql("  , TOTALCOUNT");
            parameter.AppendSql("  , CREATED");
            parameter.AppendSql("  , SWLICENSE");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("   :YEAR");
            parameter.AppendSql("  , :SITE_ID");
            parameter.AppendSql("  , :D2COUNT");
            parameter.AppendSql("  , :C2COUNT");
            parameter.AppendSql("  , :TOTALCOUNT");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :SWLICENSE");
            parameter.AppendSql(") ");

            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("D2COUNT", dto.D2COUNT);
            parameter.Add("C2COUNT", dto.C2COUNT);
            parameter.Add("TOTALCOUNT", dto.TOTALCOUNT);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);
        }
    }
}

