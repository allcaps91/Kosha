namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;

    public class HeaJepsuSunapRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuSunapRepository()
        {

        } 

        public List<HEA_JEPSU_SUNAP> GetItembyPaNo(long nPano)
        {
            MParameter parameter = CreateParameter();
            
            parameter.AppendSql("SELECT to_char(a.SDATE,'YYYY-MM-DD') SDATE, a.WRTNO    ");
            parameter.AppendSql("     , a.GJJONG, a.SANGDAM_ONE, SUM(b.TOTAMT) TOTAMT   ");
            parameter.AppendSql("  FROM ADMIN.HEA_JEPSU a                         ");
            parameter.AppendSql("     , ADMIN.HEA_SUNAP b                         ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                                  ");
            parameter.AppendSql("   AND a.GbSTS NOT IN ('0','D')                        ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                            ");
            parameter.AppendSql(" GROUP BY a.SDate, a.WRTNO, a.GJJONG, a.SANGDAM_ONE    ");
            parameter.AppendSql(" ORDER BY a.SDate, a.WRTNO, a.GJJONG, a.SANGDAM_ONE    ");

            parameter.Add("PANO", nPano);

            return ExecuteReader<HEA_JEPSU_SUNAP>(parameter);
        }

        public List<HEA_JEPSU_SUNAP> GetListSum(string fstrFDate, string fstrTDate, string fstrJong, string fstrJob, long fnLtdCode, string fstrSName, bool fbZero, List<long> AllWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT FC_HEA_GJJONG_NAME(b.GJJONG) AS GJNAME, a.PANO,b.SNAME                                              ");
            parameter.AppendSql("      ,a.SEQNO, a.WRTNO, '' GJCHASU, '' KIHO, b.PTNO, b.REMARK                                               ");
            parameter.AppendSql("      ,TO_CHAR(b.JEPDATE, 'MM/DD') JEPDATE_MMDD, b.GJJONG                                                  ");
            parameter.AppendSql("      ,TO_CHAR(a.SUDATE, 'MM/DD') SUDATE_MMDD, b.LTDCODE, c.LTDAMT                                         ");
            parameter.AppendSql("      ,c.SUNAPAMT AS SUNAPAMT1, c.SUNAPAMT2, c.TOTAMT, c.JOHAPAMT                                          ");
            parameter.AppendSql("      ,c.BONINAMT, c.MISUAMT, c.HALINAMT, '' UCODES, a.JOBSABUN                                            ");
            parameter.AppendSql("      ,ADMIN.FC_HC_PATIENT_JUMINNO(b.PTNO) AS JUMINNO                                                ");
            parameter.AppendSql("      ,ADMIN.FC_HIC_LTDNAME(b.LTDCODE) AS LTDNAME                                                    ");
            parameter.AppendSql("      ,ADMIN.FC_INSA_MST_KORNAME(a.JOBSABUN) JOBNAME                                                  ");
            parameter.AppendSql("      ,CASE WHEN a.SUNAPAMT2 > 0 THEN '◎' ELSE '' END AS GBCARD                                            ");
            parameter.AppendSql("  FROM ADMIN.HEA_SUNAP a                                                                             ");
            parameter.AppendSql("     , ADMIN.HEA_JEPSU b                                                                             ");
            parameter.AppendSql("     ,(                                                                                                    ");
            parameter.AppendSql("       SELECT a.WRTNO, MAX(a.SEQNO) AS SEQNO, SUM(a.SUNAPAMT1) AS SUNAPAMT, SUM(a.SUNAPAMT2) AS SUNAPAMT2  ");
            parameter.AppendSql("             ,SUM(a.TOTAMT) AS TOTAMT, 0 AS JOHAPAMT, SUM(a.LTDAMT) AS LTDAMT                              ");
            parameter.AppendSql("             ,SUM(a.BONINAMT) AS BONINAMT, SUM(a.MISUAMT) AS MISUAMT                                       ");
            parameter.AppendSql("             ,SUM(a.HALINAMT) AS HALINAMT                                                                  ");
            parameter.AppendSql("         FROM HEA_SUNAP A, HEA_JEPSU B                                                                     ");
            parameter.AppendSql("        WHERE a.SUDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("          AND a.SUDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("          AND a.WRTNO = b.WRTNO(+)                                                                         ");
            parameter.AppendSql("          AND a.WRTNO > 0                                                                                  ");
            parameter.AppendSql("        GROUP BY a.WRTNO                                                                                   ");
            parameter.AppendSql("      ) c                                                                                                  ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                               ");
            parameter.AppendSql("   AND A.PANO NOT IN (991,992,993,994,995,996,997,998,999)                                                 ");
            parameter.AppendSql("   AND a.SUDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                                            ");
            parameter.AppendSql("   AND a.SUDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                                                            ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                                ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO                                                                                   ");
            parameter.AppendSql("   AND a.SEQNO = c.SEQNO                                                                                   ");
            parameter.AppendSql("   AND c.TOTAMT > 0                                                                                        ");
            if (fbZero)
            {
                parameter.AppendSql("   AND a.SUNAPAMT > 0                                          ");
            }

            if (!fstrJong.Equals("**"))
            {
                parameter.AppendSql("   AND b.GJJONG = :GJJONG                                      ");
            }

            if (fnLtdCode > 0)
            {
                parameter.AppendSql("   AND LTDCODE =:LTDCODE                                       ");
            }

            if (!fstrSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND b.SNAME LIKE :SNAME                                    ");
            }

            if (AllWrtNo.Count > 0)
            {
                parameter.AppendSql("   AND B.WRTNO NOT IN (:WRTNO)                                ");
            }

            parameter.AppendSql(" ORDER BY b.GJJONG, b.JEPDATE, a.PANO, a.WRTNO, a.SeqNo            ");

            parameter.Add("FDATE", fstrFDate);
            parameter.Add("TDATE", fstrTDate);

            if (!fstrJong.Equals("**"))
            {
                parameter.Add("GJJONG", fstrJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (fnLtdCode > 0)
            {
                parameter.Add("LTDCODE", fnLtdCode);
            }

            if (!fstrSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", fstrSName);
            }

            if (AllWrtNo.Count > 0)
            {
                parameter.AddInStatement("WRTNO", AllWrtNo);
            }
            

            return ExecuteReader<HEA_JEPSU_SUNAP>(parameter);
        }

        public List<HEA_JEPSU_SUNAP> GetListAll(string fstrFDate, string fstrTDate, string fstrJong, string fstrJob, long fnLtdCode, string fstrSName, bool fbZero)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT FC_HEA_GJJONG_NAME(b.GJJONG) AS GJNAME, a.PANO, b.SNAME, b.SUNAP    ");
            parameter.AppendSql("      ,a.SEQNO, a.WRTNO, b.PTNO, b.REMARK, a.MISUGYE                       ");
            parameter.AppendSql("      ,TO_CHAR(b.SDATE, 'MM/DD') SDATE_MMDD, b.GJJONG, b.AGE, b.SEX        ");
            parameter.AppendSql("      ,TO_CHAR(a.SUDATE, 'MM/DD') SUDATE_MMDD, b.LTDCODE, a.LTDAMT         ");
            parameter.AppendSql("      ,a.SUNAPAMT1, a.SUNAPAMT2, a.TOTAMT, b.GBSTS                         ");
            parameter.AppendSql("      ,a.BONINAMT, a.MISUAMT, a.HALINAMT, a.JOBSABUN                       ");
            parameter.AppendSql("      ,ADMIN.FC_HC_PATIENT_JUMINNO(b.PTNO) AS JUMINNO                ");
            parameter.AppendSql("      ,ADMIN.FC_HIC_LTDNAME(b.LTDCODE) AS LTDNAME                    ");
            parameter.AppendSql("      ,ADMIN.FC_INSA_MST_KORNAME(a.JOBSABUN) JOBNAME                  ");
            parameter.AppendSql("      ,CASE WHEN a.SUNAPAMT2 > 0 THEN '◎' ELSE '' END AS GBCARD            ");
            parameter.AppendSql("      ,FC_HEA_GAMCOME_NAME(a.HALINGYE) AS HALINGYE_NM                      ");
            parameter.AppendSql("  FROM ADMIN.HEA_SUNAP a                                             ");
            parameter.AppendSql("     , ADMIN.HEA_JEPSU b                                             ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND A.PANO NOT IN (991,992,993,994,995,996,997,998,999)                 ");
            parameter.AppendSql("   AND a.SUDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                            ");
            parameter.AppendSql("   AND a.SUDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                            ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                ");
            parameter.AppendSql("   AND ((b.GBSTS = 'D' AND a.MISUGYE = '01') OR b.GBSTS NOT IN ('0') OR (b.GBSTS = '0' AND (a.SUNAPAMT1 > 0 OR a.SUNAPAMT2 > 0))) ");
            if (fbZero)
            {
                parameter.AppendSql("   AND a.SUNAPAMT > 0                                          ");
            }

            if (!fstrJong.Equals("**"))
            {
                parameter.AppendSql("   AND b.GJJONG = :GJJONG                                      ");
            }

            if (fstrJob.Equals("2"))
            {
                parameter.AppendSql("   AND CHUNGGUYN IS NULL                                       ");
            }
            else if (fstrJob.Equals("3"))
            {
                parameter.AppendSql("   AND CHUNGGUYN ='Y'                                          ");
            }

            if (fnLtdCode > 0)
            {
                parameter.AppendSql("   AND LTDCODE =:LTDCODE                                       ");
            }

            if (!fstrSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND b.SNAME LIKE :SNAME                                       ");
            }

            parameter.AppendSql(" ORDER BY b.GJJONG, b.JEPDATE, a.PANO, a.WRTNO, a.SEQNO            ");

            parameter.Add("FDATE", fstrFDate);
            parameter.Add("TDATE", fstrTDate);

            if (!fstrJong.Equals("**"))
            {
                parameter.Add("GJJONG", fstrJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (fnLtdCode > 0)
            {
                parameter.Add("LTDCODE", fnLtdCode);
            }

            if (!fstrSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", fstrSName);
            }

            return ExecuteReader<HEA_JEPSU_SUNAP>(parameter);
        }


        public List<HEA_JEPSU_SUNAP> GetListSum2(string fstrFDate, string fstrTDate, string fstrJong, string fstrJob, long fnLtdCode, string fstrSName, bool fbZero)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT FC_HEA_GJJONG_NAME(b.GJJONG) AS GJNAME, a.PANO,b.SNAME                                              ");
            parameter.AppendSql("      ,a.SEQNO, a.WRTNO, '' GJCHASU, '' KIHO, b.PTNO, b.REMARK                                               ");
            parameter.AppendSql("      ,TO_CHAR(b.JEPDATE, 'MM/DD') JEPDATE_MMDD, b.GJJONG                                                  ");
            parameter.AppendSql("      ,TO_CHAR(a.SUDATE, 'MM/DD') SUDATE_MMDD, b.LTDCODE, c.LTDAMT                                         ");
            parameter.AppendSql("      ,c.SUNAPAMT AS SUNAPAMT1, c.SUNAPAMT2, c.TOTAMT, c.JOHAPAMT                                          ");
            parameter.AppendSql("      ,c.BONINAMT, c.MISUAMT, c.HALINAMT, '' UCODES, a.JOBSABUN                                            ");
            parameter.AppendSql("      ,ADMIN.FC_HC_PATIENT_JUMINNO(b.PTNO) AS JUMINNO                                                ");
            parameter.AppendSql("      ,ADMIN.FC_HIC_LTDNAME(b.LTDCODE) AS LTDNAME                                                    ");
            parameter.AppendSql("      ,ADMIN.FC_INSA_MST_KORNAME(a.JOBSABUN) JOBNAME                                                  ");
            parameter.AppendSql("      ,CASE WHEN a.SUNAPAMT2 > 0 THEN '◎' ELSE '' END AS GBCARD                                            ");
            parameter.AppendSql("  FROM ADMIN.HEA_SUNAP a                                                                             ");
            parameter.AppendSql("     , ADMIN.HEA_JEPSU b                                                                             ");
            parameter.AppendSql("     ,(                                                                                                    ");
            parameter.AppendSql("       SELECT a.WRTNO, MAX(a.SEQNO) AS SEQNO, SUM(a.SUNAPAMT1) AS SUNAPAMT, SUM(a.SUNAPAMT2) AS SUNAPAMT2  ");
            parameter.AppendSql("             ,SUM(a.TOTAMT) AS TOTAMT, 0 AS JOHAPAMT, SUM(a.LTDAMT) AS LTDAMT                              ");
            parameter.AppendSql("             ,SUM(a.BONINAMT) AS BONINAMT, SUM(a.MISUAMT) AS MISUAMT                                       ");
            parameter.AppendSql("             ,SUM(a.HALINAMT) AS HALINAMT                                                                  ");
            parameter.AppendSql("         FROM HEA_SUNAP A, HEA_JEPSU B                                                                     ");
            parameter.AppendSql("        WHERE a.SUDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("          AND a.SUDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("          AND a.WRTNO = b.WRTNO(+)                                                                         ");
            parameter.AppendSql("          AND a.WRTNO > 0                                                                                  ");
            parameter.AppendSql("        GROUP BY a.WRTNO                                                                                   ");
            parameter.AppendSql("      ) c                                                                                                  ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                               ");
            parameter.AppendSql("   AND A.PANO NOT IN (991,992,993,994,995,996,997,998,999)                                                 ");
            parameter.AppendSql("   AND a.SUDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                                            ");
            parameter.AppendSql("   AND a.SUDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                                                            ");
            parameter.AppendSql("   AND b.SDATE <> TO_DATE(:FDATE,'YYYY-MM-DD')                                                            ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                                ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO                                                                                   ");
            parameter.AppendSql("   AND a.SEQNO = c.SEQNO                                                                                   ");
            //parameter.AppendSql("   AND c.TOTAMT > 0                                                                                        ");
            if (fbZero)
            {
                parameter.AppendSql("   AND a.SUNAPAMT > 0                                          ");
            }

            if (!fstrJong.Equals("**"))
            {
                parameter.AppendSql("   AND b.GJJONG = :GJJONG                                      ");
            }

            if (fnLtdCode > 0)
            {
                parameter.AppendSql("   AND LTDCODE =:LTDCODE                                       ");
            }

            if (!fstrSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND b.SNAME LIKE :SNAME                                    ");
            }

            parameter.AppendSql(" ORDER BY b.GJJONG, b.JEPDATE, a.PANO, a.WRTNO, a.SeqNo            ");

            parameter.Add("FDATE", fstrFDate);
            parameter.Add("TDATE", fstrTDate);

            if (!fstrJong.Equals("**"))
            {
                parameter.Add("GJJONG", fstrJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (fnLtdCode > 0)
            {
                parameter.Add("LTDCODE", fnLtdCode);
            }

            if (!fstrSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", fstrSName);
            }

            return ExecuteReader<HEA_JEPSU_SUNAP>(parameter);
        }
    }
}
