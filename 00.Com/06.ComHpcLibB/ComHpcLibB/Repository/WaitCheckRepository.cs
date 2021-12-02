namespace ComHpcLibB.Repository
{
    using ComBase;
    using ComBase.Mvc;
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;
    using ComBase.Controls;

    /// <summary>
    /// 
    /// </summary>
    public class WaitCheckRepository : BaseRepository
    {   
        /// <summary>
        /// 
        /// </summary>
        public WaitCheckRepository()
        {
        }

        public List<WAIT_CHECK> Read_Wait(string SDATE, string HEAPART)
        {
            MParameter parameter = CreateParameter();
            
            parameter.AppendSql("SELECT a.WRTNO, a.ActPART, a.ACTIVE                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULT a                                        ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_JEPSU  b                                        ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO                                               ");
            parameter.AppendSql("   AND a.Result IS NULL                                                ");
            parameter.AppendSql("   AND a.EXCODE NOT IN ('A101','A102','A103','TX87')                   ");
            parameter.AppendSql("   AND b.SDate = TO_DATE(:SDATE, 'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND b.GBSTS NOT IN ('0','D')                                        ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                               ");
            if (!HEAPART.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND Trim(a.ActPART) = :HEAPART                                  ");
            }
            parameter.AppendSql("   AND (a.ACTIVE IS NULL OR a.ACTIVE = '')                             ");
            parameter.AppendSql(" GROUP BY a.WRTNO,a.ActPART,a.ACTIVE                                   ");
            parameter.AppendSql(" ORDER BY a.WRTNO,a.ActPART,a.ACTIVE                                   ");
            
            parameter.Add("SDATE", SDATE);
            if (!HEAPART.IsNullOrEmpty())
            {
                parameter.Add("HEAPART", HEAPART.Trim(), Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<WAIT_CHECK>(parameter);
        }

        public List<WAIT_CHECK> Read_Wait_Hic(string JEPDATE, string ENTPART, string strGB)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO, a.PART, a.ACTIVE                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT a                                        ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_JEPSU  b                                        ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO                                               ");
            parameter.AppendSql("   AND b.JepDate = TO_DATE(:JEPDATE,'YYYY-MM-DD')                      ");
            if (!ENTPART.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND PART = :ENTPART                                             ");
            }
            parameter.AppendSql("   AND(a.ACTIVE IS NULL OR a.ACTIVE = ' ')                             ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                               ");
            parameter.AppendSql("   AND b.JongGumYN != '1'                                              ");
            parameter.AppendSql("   AND a.EXCODE <> 'A134'                                              "); 
            if (strGB == "1")
            {
                parameter.AppendSql("   AND b.GBCHUL = 'N'                                              "); //내원
            }
            else if (strGB == "2")
            {
                parameter.AppendSql("   AND b.GBCHUL = 'Y'                                              "); //사업장
            }
            parameter.AppendSql(" GROUP BY a.WRTNO, a.PART, a.ACTIVE                                    ");
            parameter.AppendSql(" ORDER BY a.WRTNO, a.PART, a.ACTIVE                                    ");

            parameter.Add("JEPDATE", JEPDATE);
            if (!ENTPART.IsNullOrEmpty())
            {
                parameter.Add("ENTPART", ENTPART, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<WAIT_CHECK>(parameter);
        }

        public List<WAIT_CHECK> Read_Wait_All(string JEPDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO, a.PART, a.ACTIVE, a.ROWID RID                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT a                                        ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_JEPSU  b                                        ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO                                               ");
            parameter.AppendSql("   AND b.JepDate = TO_DATE(:JEPDATE,'YYYY-MM-DD')                      ");
            parameter.AppendSql("   AND(a.ACTIVE IS NULL OR a.ACTIVE = ' ')                             ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                               ");
            parameter.AppendSql("   AND b.GbChul <> 'Y'                                                 ");
            parameter.AppendSql(" GROUP BY a.WRTNO, a.PART, a.ACTIVE                                    ");
            parameter.AppendSql(" ORDER BY a.WRTNO, a.PART, a.ACTIVE                                    ");

            parameter.Add("JEPDATE", JEPDATE);

            return ExecuteReader<WAIT_CHECK>(parameter);
        }

        public HEA_SANGDAM_WAIT Read_Exam_Wait(string SDATE, string HEAPART)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SANGDAM_WAIT                            ");
            parameter.AppendSql(" WHERE ENTTIME >= TO_DATE(:SDATE, 'yyyy-MM-dd')                ");
            parameter.AppendSql("   AND Gubun <> '12'                                           "); //상담대기
            if (!HEAPART.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND TRIM(GUBUN) = :HEAPART                              ");
            }
            parameter.AppendSql("   AND (GbCall IS NULL OR GbCall = '')                         ");

            parameter.Add("SDATE", SDATE);
            if (!HEAPART.IsNullOrEmpty())
            {
                parameter.Add("HEAPART", HEAPART.Trim(), Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReaderSingle <HEA_SANGDAM_WAIT>(parameter);
        }

        public int Read_Exam_Wait_Hic(List<string> strGbWait)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(WRTNO) CNT                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT                            ");
            parameter.AppendSql(" WHERE 1 = 1                                                   ");
            if (strGbWait.Count > 0)
            {
                parameter.AppendSql("   AND GUBUN IN (:HEAPART)                                 ");
            }
            parameter.AppendSql("   AND (GbCall IS NULL OR GbCall = '')                         ");

            if (strGbWait.Count > 0)
            {
                parameter.AddInStatement("HEAPART", strGbWait, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteScalar<int>(parameter);
        }
    }
}
