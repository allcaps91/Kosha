namespace ComHpcLibB.Repository
{
    using ComBase;
    using ComBase.Mvc;
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;

    /// <summary>
    /// 
    /// </summary>
    public class ActingCheckRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public ActingCheckRepository()
        {
        }

        public List<ACTING_CHECK> aCTING_CHECKs(long WRTNO, string SDATE)
        {
            MParameter parameter = CreateParameter();
            if (WRTNO == 0)
            {
                parameter.AppendSql("SELECT max(E.HaRoom) HaRoom, E.HeaPART, b.NAME                         ");
                parameter.AppendSql("     , NVL(TO_NUMBER(b.SORT), 999) SORT                                ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULT a                                        ");
                parameter.AppendSql("     , KOSMOS_PMPA.HEA_CODE   b                                        ");
                parameter.AppendSql("     , KOSMOS_PMPA.HEA_JEPSU  c                                        ");
                parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE E                                        ");
                parameter.AppendSql(" WHERE E.HeaPART = b.CODE                                              ");
                parameter.AppendSql("   AND A.EXCODE NOT IN ('A101','A102','A103','TX87')                   ");
                parameter.AppendSql("   AND a.WRTNO = C.WRTNO                                               ");
                parameter.AppendSql("   AND b.GUBUN = 'A9'                                                  ");
                parameter.AppendSql("   AND c.SDATE = TO_DATE(:SDATE,'YYYY-MM-DD')                          ");
                parameter.AppendSql("   AND c.DELDATE IS NULL                                               ");
                parameter.AppendSql("   AND c.GBSTS NOT IN ('0','D')                                        ");
                parameter.AppendSql("   AND A.EXCODE = E.CODE(+)                                            ");
                parameter.AppendSql("   AND (E.HeaPART IS NOT NULL AND E.HeaPART <> ' ')                    ");
                parameter.AppendSql(" GROUP BY NVL(TO_NUMBER(b.SORT), 999), e.HeaPART, b.NAME               ");
                parameter.AppendSql(" ORDER BY NVL(TO_NUMBER(b.SORT), 999), e.HeaPART, b.NAME               ");
                //parameter.AppendSql(" GROUP BY NVL(TO_NUMBER(b.SORT), 999), e.HaRoom, e.HeaPART, b.NAME     ");
                //parameter.AppendSql(" ORDER BY NVL(TO_NUMBER(b.SORT), 999), e.HaRoom, e.HeaPART, b.NAME     ");

                parameter.Add("SDATE", SDATE);
            }
            else
            {
                //당일 암검진이 있으면 같이 표시
                parameter.AppendSql("SELECT max(E.HaRoom) HaRoom , E.HeaPART, b.NAME                        ");
                parameter.AppendSql("     , NVL(TO_NUMBER(b.SORT), 999) SORT                                ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULT a                                        ");
                parameter.AppendSql("     , KOSMOS_PMPA.HEA_CODE   b                                        ");
                parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE E                                        ");
                parameter.AppendSql(" WHERE E.HeaPART = b.CODE                                              ");
                parameter.AppendSql("   AND b.GUBUN = 'A9'                                                  ");
                parameter.AppendSql("   AND A.WRTNO = :WRTNO                                                ");
                parameter.AppendSql("   AND A.EXCODE = E.CODE(+)                                            ");
                parameter.AppendSql("   AND A.EXCODE NOT IN ('A101','A102','A103')                          ");
                parameter.AppendSql("   AND (E.HeaPART IS NOT NULL AND  E.HeaPART <>' ')                    ");
                parameter.AppendSql(" GROUP BY NVL(TO_NUMBER(b.SORT), 999), e.HeaPART, b.NAME               ");
                parameter.AppendSql(" ORDER BY NVL(TO_NUMBER(b.SORT), 999), e.HeaPART, b.NAME               ");
                //parameter.AppendSql(" GROUP BY NVL(TO_NUMBER(b.SORT), 999), e.HaRoom, e.HeaPART, b.NAME     ");
                //parameter.AppendSql(" ORDER BY NVL(TO_NUMBER(b.SORT), 999), e.HaRoom, e.HeaPART, b.NAME     ");

                parameter.Add("WRTNO", WRTNO);
            }

            return ExecuteReader<ACTING_CHECK>(parameter);
        }

        public List<ACTING_CHECK> ACTING_CHECKbyWrtNOGubun11(long argWRTNO, string argDate)
        {
            MParameter parameter = CreateParameter();
            if (argWRTNO == 0)
            {
                parameter.AppendSql("SELECT max(E.HaRoom) HaRoom, E.HeaPART, b.NAME                         ");
                parameter.AppendSql("     , NVL(TO_NUMBER(b.SORT), 999) SORT                                ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULT a                                        ");
                parameter.AppendSql("     , KOSMOS_PMPA.HEA_CODE   b                                        ");
                parameter.AppendSql("     , KOSMOS_PMPA.HEA_JEPSU  c                                        ");
                parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE e                                        ");
                parameter.AppendSql(" WHERE e.HeaPART = b.CODE                                              ");
                parameter.AppendSql("   AND a.EXCODE NOT IN ('A101','A102','A103','TX87')                   ");
                parameter.AppendSql("   AND a.WRTNO = c.WRTNO                                               ");
                parameter.AppendSql("   AND b.GUBUN = '11'                                                  ");
                parameter.AppendSql("   AND c.SDATE = TO_DATE(:SDATE,'YYYY-MM-DD')                          ");
                parameter.AppendSql("   AND c.DELDATE IS NULL                                               ");
                parameter.AppendSql("   AND c.GBSTS NOT IN ('0','D')                                        ");
                parameter.AppendSql("   AND a.EXCODE = e.CODE(+)                                            ");
                parameter.AppendSql("   AND (e.HeaPART IS NOT NULL AND e.HeaPART <> ' ')                    ");
                parameter.AppendSql(" GROUP BY NVL(TO_NUMBER(b.SORT), 999), e.HeaPART, b.NAME               ");
                parameter.AppendSql(" ORDER BY NVL(TO_NUMBER(b.SORT), 999), e.HeaPART, b.NAME               ");
                //parameter.AppendSql(" GROUP BY NVL(TO_NUMBER(b.SORT), 999), e.HaRoom, e.HeaPART, b.NAME     ");
                //parameter.AppendSql(" ORDER BY NVL(TO_NUMBER(b.SORT), 999), e.HaRoom, e.HeaPART, b.NAME     ");

                parameter.Add("SDATE", argDate);
            }
            else
            {
                //당일 암검진이 있으면 같이 표시
                parameter.AppendSql("SELECT max(E.HaRoom) HaRoom, E.HeaPART, b.NAME                         ");
                parameter.AppendSql("     , NVL(TO_NUMBER(b.SORT), 999) SORT                                ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULT a                                        ");
                parameter.AppendSql("     , KOSMOS_PMPA.HEA_CODE   b                                        ");
                parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE e                                        ");
                parameter.AppendSql(" WHERE e.HeaPART = b.CODE                                              ");
                parameter.AppendSql("   AND b.GUBUN = '11'                                                  ");
                parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                ");
                parameter.AppendSql("   AND a.EXCODE = e.CODE(+)                                            ");
                parameter.AppendSql("   AND a.EXCODE NOT IN ('A101','A102','A103')                          ");
                parameter.AppendSql("   AND (e.HeaPART IS NOT NULL AND  e.HeaPART <>' ')                    ");
                parameter.AppendSql(" GROUP BY NVL(TO_NUMBER(b.SORT), 999), e.HeaPART, b.NAME               ");
                parameter.AppendSql(" ORDER BY NVL(TO_NUMBER(b.SORT), 999), e.HeaPART, b.NAME               ");
                //parameter.AppendSql(" GROUP BY NVL(TO_NUMBER(b.SORT), 999), e.HaRoom, e.HeaPART, b.NAME     ");
                //parameter.AppendSql(" ORDER BY NVL(TO_NUMBER(b.SORT), 999), e.HaRoom, e.HeaPART, b.NAME     ");

                parameter.Add("WRTNO", argWRTNO);
            }

            return ExecuteReader<ACTING_CHECK>(parameter);
        }

        public List<ACTING_CHECK> ACTING_CHECK_AM(string strJepDate, long nPaNo)
        {
            MParameter parameter = CreateParameter();

            //당일 암검진이 있으면 같이 표시
            parameter.AppendSql("SELECT /*+rule*/ E.ENTPART, b.NAME                                     ");
            parameter.AppendSql("     , NVL(TO_NUMBER(b.SORT), 999) SORT                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT a                                        ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_CODE   b                                        ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE e                                        ");
            parameter.AppendSql(" WHERE e.ENTPART = b.CODE                                              ");
            parameter.AppendSql("   AND b.GUBUN   = '72'                                                ");
            parameter.AppendSql("   AND a.WRTNO IN (SELECT WRTNO                                        ");
            parameter.AppendSql("                     FROM KOSMOS_PMPA.HIC_JEPSU                        ");
            parameter.AppendSql("                    WHERE PANO = :PANO                                 ");
            parameter.AppendSql("                      AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("                      AND (DELDATE IS NULL OR GJJONG IN ('31'))        ");
            parameter.AppendSql("                  )                                                    ");
            parameter.AppendSql("   AND a.EXCODE = e.CODE(+)                                            ");
            parameter.AppendSql("   AND (e.ENTPART IS NOT NULL AND e.ENTPART <> ' ')                    ");
            parameter.AppendSql(" GROUP BY NVL(TO_NUMBER(b.SORT), 999), e.ENTPART, b.NAME               ");
            //parameter.AppendSql(" ORDER BY NVL(TO_NUMBER(b.SORT), 999), e.HaRoom, e.HeaPART, b.NAME     ");


            parameter.Add("JEPDATE", strJepDate);
            parameter.Add("PANO", nPaNo);

            return ExecuteReader<ACTING_CHECK>(parameter);
        }

        public List<ACTING_CHECK> ACTING_CHECK_HIC(long nWrtNo, string strJepDate, long nPaNo, string sChk, string strJong)
        {
            MParameter parameter = CreateParameter();
            if (nWrtNo == 0)
            {
                parameter.AppendSql("SELECT /*+rule*/ e.HCROOM, e.ENTPART, b.NAME               ");
                parameter.AppendSql("     , NVL(TO_NUMBER(b.SORT), 999) SORT                    ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT a                            ");
                parameter.AppendSql("     , KOSMOS_PMPA.HIC_CODE   b                            ");
                parameter.AppendSql("     , KOSMOS_PMPA.HIC_JEPSU  c                            ");
                parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE e                            ");
                parameter.AppendSql(" WHERE e.ENTPART = b.CODE                                  ");
                parameter.AppendSql("   AND a.WRTNO   = c.WRTNO                                 ");
                parameter.AppendSql("   AND b.GUBUN   = '72'                                    ");
                parameter.AppendSql("   AND c.JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')         ");
                parameter.AppendSql("   AND a.EXCODE  = e.CODE(+)                               ");
                parameter.AppendSql("   AND(e.ENTPART IS NOT NULL AND e.ENTPART <> ' ')         ");
                parameter.AppendSql(" GROUP BY e.HCROOM, b.SORT, e.ENTPART, b.NAME              ");
                parameter.AppendSql(" ORDER BY NVL(TO_NUMBER(b.SORT), 999), e.ENTPART, b.NAME   ");

                parameter.Add("JEPDATE", strJepDate);
            }
            else
            {
                //당일 암검진이 있으면 같이 표시
                parameter.AppendSql("SELECT /*+rule*/ e.HCROOM, e.ENTPART, b.NAME                               ");
                parameter.AppendSql("     , NVL(TO_NUMBER(b.SORT), 999) SORT                                    ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT a                                            ");
                parameter.AppendSql("     , KOSMOS_PMPA.HIC_CODE   b                                            ");
                parameter.AppendSql("     , KOSMOS_PMPA.HIC_JEPSU  c                                            ");
                parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE e                                            ");
                parameter.AppendSql(" WHERE e.ENTPART = b.CODE                                                  ");
                parameter.AppendSql("   AND a.WRTNO   = c.WRTNO                                                 ");
                parameter.AppendSql("   AND b.GUBUN   = '72'                                                    ");
                if (strJong == "Y")
                {  
                    parameter.AppendSql("   AND a.WRTNO IN (SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU             ");
                    parameter.AppendSql("                    WHERE PANO = :PANO                                 ");
                    parameter.AppendSql("                      AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')    ");
                    parameter.AppendSql("                      AND DELDATE IS NULL                              ");
                    parameter.AppendSql("                  )                                                    ");    
                }
                else
                {
                    parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                ");
                }
                parameter.AppendSql("   AND a.EXCODE = e.CODE(+)                                                ");
                parameter.AppendSql("   AND (e.ENTPART IS NOT NULL AND e.ENTPART <>' ')                         ");
                if (sChk == "OK")
                {
                    parameter.AppendSql("   AND NAME NOT IN('일반상담')                                         ");
                }
                parameter.AppendSql(" GROUP BY NVL(TO_NUMBER(b.SORT), 999), e.HCROOM, e.ENTPART, b.NAME         ");
                parameter.AppendSql(" ORDER BY NVL(TO_NUMBER(b.SORT), 999), e.HCROOM, e.ENTPART, b.NAME         ");

                         
                if (strJong == "Y")
                {
                    parameter.Add("JEPDATE", strJepDate);
                    parameter.Add("PANO", nPaNo);
                }
                else
                {
                    parameter.Add("WRTNO", nWrtNo);
                }
            }

            return ExecuteReader<ACTING_CHECK>(parameter);
        }

        public List<ACTING_CHECK> ACTING_CHECK_ALL(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.EXCODE, a.RESULT, a.ACTIVE, b.HNAME   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT_NEW a            ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE     b            ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            parameter.AppendSql("   AND a.PART = '5'                            ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                    ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                        ");
            parameter.AppendSql(" ORDER BY a.EXCODE, b.HNAME                    ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<ACTING_CHECK>(parameter);
        }

        public List<ACTING_CHECK> ACTING_CHECK_WAIT(long nWrtNo, string strJepDate, long nPaNo, string strJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT /*+rule*/ E.ENTPART, b.NAME                                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT a                                            ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_CODE   b                                            ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_JEPSU  c                                            ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE E                                            ");
            parameter.AppendSql(" WHERE E.ENTPART = b.CODE                                                  ");
            parameter.AppendSql("   AND a.WRTNO   = C.WRTNO                                                 ");
            parameter.AppendSql("   AND b.GUBUN   = '72'                                                    ");
            if (strJong == "N")
            {
                parameter.AppendSql("   AND A.WRTNO = :WRTNO                                                ");
            }
            else
            {
                parameter.AppendSql("   AND A.WRTNO IN (SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU             ");
                parameter.AppendSql("                    WHERE PANO = :PANO                                 ");
                parameter.AppendSql("                      AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')    ");
                parameter.AppendSql("                      AND DELDATE IS NULL)                             ");
            }
            parameter.AppendSql("   AND A.EXCODE = E.CODE(+)                                                ");
            parameter.AppendSql("   AND (E.ENTPART IS NOT NULL AND  E.ENTPART <> ' ')                       ");
            parameter.AppendSql(" GROUP BY E.ENTPART, b.NAME                                                ");
            parameter.AppendSql(" ORDER BY E.ENTPART, b.NAME                                                ");

            if (strJong == "N")
            {
                parameter.Add("WRTNO", nWrtNo);
            }
            else
            {
                parameter.Add("PANO", nPaNo);
                parameter.Add("JEPDATE", strJepDate);
            }

            return ExecuteReader<ACTING_CHECK>(parameter);
        }

        public string GetCodebyWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            
            parameter.AppendSql("SELECT CODE                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", argWRTNO);            

            return ExecuteScalar<string>(parameter);
        }
    }
}
