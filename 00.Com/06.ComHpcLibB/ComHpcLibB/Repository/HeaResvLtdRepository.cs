namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaResvLtdRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaResvLtdRepository()
        {
        }

        public int UpDateInwonClear(long argLtd, string argDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HEA_RESV_LTD                    ");
            parameter.AppendSql("   SET AMJEPSU = 0                                 ");
            parameter.AppendSql("      ,PMJEPSU = 0                                 ");
            parameter.AppendSql("      ,AMJAN   = AMINWON                           ");
            parameter.AppendSql("      ,PMJAN   = PMINWON                           ");
            if (argDate.IsNullOrEmpty())
            {
                parameter.AppendSql(" WHERE SDATE >= TRUNC(SYSDATE)                 ");
            }
            else
            {
                parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')   ");
            }
            
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                          ");

            #region Query 변수대입

            if (!argDate.IsNullOrEmpty())
            {
                parameter.Add("SDATE", argDate);
            }
            parameter.Add("LTDCODE", argLtd);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public int UpDateInwon(HEA_RESV_LTD data)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HEA_RESV_LTD        ");
            parameter.AppendSql("   SET AMJEPSU = :AMJEPSU              ");
            parameter.AppendSql("     , PMJEPSU = :PMJEPSU              ");
            parameter.AppendSql("     , AMJAN   = :AMJAN                ");
            parameter.AppendSql("     , PMJAN   = :PMJAN                ");
            if (data.AMINWON < 999)
            {
                parameter.AppendSql("     , AMINWON   = :AMINWON        ");
            }

            if (data.PMINWON < 999)
            {
                parameter.AppendSql("     , PMINWON   = :PMINWON        ");
            }

            if (!data.YOIL.IsNullOrEmpty())
            {
                parameter.AppendSql("     , YOIL   = :YOIL              ");
            }

            if (data.LTDCODE > 0)
            {
                parameter.AppendSql("     , LTDCODE   = :LTDCODE        ");
            }

            if (data.ENTSABUN > 0)
            {
                parameter.AppendSql("      , ENTSABUN   = :ENTSABUN     ");
            }

            parameter.AppendSql("     , ENTTIME   = SYSDATE             ");

            parameter.AppendSql(" WHERE ROWID   = :RID                  ");

            #region Query 변수대입
            parameter.Add("AMJEPSU", data.AMJEPSU);
            parameter.Add("PMJEPSU", data.PMJEPSU);
            parameter.Add("AMJAN",   data.AMJAN);
            parameter.Add("PMJAN",   data.PMJAN);
            if (data.AMINWON < 999)
            {
                parameter.Add("AMINWON", data.AMINWON);
            }

            if (data.PMINWON < 999)
            {
                parameter.Add("PMINWON", data.PMINWON);
            }

            if (!data.YOIL.IsNullOrEmpty())
            {
                parameter.Add("YOIL", data.YOIL);
            }

            if (data.LTDCODE > 0)
            {
                parameter.Add("LTDCODE", data.LTDCODE);
            }

            if (data.ENTSABUN > 0)
            {
                parameter.Add("ENTSABUN", data.ENTSABUN);
            }

            parameter.Add("RID",     data.RID);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public int UpDateInwon1(HEA_RESV_LTD data)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HEA_RESV_LTD        ");
            parameter.AppendSql("   SET AMJEPSU = :AMJEPSU              ");
            parameter.AppendSql("     , PMJEPSU = :PMJEPSU              ");
            parameter.AppendSql("     , AMJAN   = :AMJAN                ");
            parameter.AppendSql("     , PMJAN   = :PMJAN                ");
            
            parameter.AppendSql(" WHERE ROWID   = :RID                  ");

            #region Query 변수대입
            parameter.Add("AMJEPSU", data.AMJEPSU);
            parameter.Add("PMJEPSU", data.PMJEPSU);
            parameter.Add("AMJAN", data.AMJAN);
            parameter.Add("PMJAN", data.PMJAN);
            parameter.Add("RID", data.RID);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public int DeleteByGubunIsNull()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HEA_RESV_LTD    ");
            parameter.AppendSql(" WHERE SDATE  >= TRUNC(SYSDATE)    ");
            parameter.AppendSql("   AND GUBUN IS NULL               ");

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_RESV_LTD> GetListAmPmInwonBySDateGubun(string argDate, string argGbn)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT AMINWON, PMINWON                        ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_LTD                ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                          ");

            parameter.Add("SDATE", argDate);
            parameter.Add("GUBUN", argGbn);

            return ExecuteReader<HEA_RESV_LTD>(parameter);
        }

        public List<HEA_RESV_LTD> GetListAmPmJanBySDateGubun(string argDate, string argGbn, int argRow)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ADMIN.FC_HIC_LTDNAME(LTDCODE) AS LTDNAME      ");
            parameter.AppendSql("      ,AMJAN, PMJAN, ENTSABUN                              ");
            parameter.AppendSql("      ,ADMIN.FC_BAS_USER_NAME(ENTSABUN) AS JOBNAME    ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_LTD                            ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')               ");

            if (argRow == 1)
            {
                parameter.AppendSql("  AND GUBUN = '00'                                      ");
            }
            else
            {
                parameter.AppendSql("  AND GUBUN = :GUBUN                                    ");
            }
            

            parameter.AppendSql("  AND (AMJAN != 0 OR PMJAN != 0)                          ");

            parameter.Add("SDATE", argDate);
            if (argRow != 1)
            {
                parameter.Add("GUBUN", argGbn);
            }
           
            return ExecuteReader<HEA_RESV_LTD>(parameter);
        }

        public int DeleteDataByRowid(string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HEA_RESV_LTD WHERE ROWID =:RID  ");
            
            parameter.Add("RID", argRowid);

            return ExecuteNonQuery(parameter);
        }

        public HEA_RESV_LTD GetSumAmPmJanByGubun(string argDate, string argGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(AMJAN) AS AMJAN, SUM(PMJAN) AS PMJAN ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_LTD                 ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                           ");

            parameter.Add("SDATE", argDate);
            parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_RESV_LTD>(parameter);
        }

        public List<HEA_RESV_LTD> GetListByLtdCode(long argLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, GUBUN, AMINWON, PMINWON, AMJAN, PMJAN    ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_LTD                                                    ");
            parameter.AppendSql(" WHERE 1 =1                                                                        ");
            parameter.AppendSql("   AND SDATE >= TRUNC(SYSDATE-1)                                                   ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                          ");
            parameter.AppendSql(" ORDER BY SDATE, GUBUN                                                             ");
            
            parameter.Add("LTDCODE", argLtdCode);

            return ExecuteReader<HEA_RESV_LTD>(parameter);
        }

        public List<HEA_RESV_LTD> GetListInwonByLtdCode(long argLtdCode, string argSDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUBUN, AMINWON, PMINWON, AMJEPSU, PMJEPSU, AMJAN, PMJAN, ROWID AS RID   ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_LTD                                                ");
            parameter.AppendSql(" WHERE 1 =1                                                                    ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE,'YYYY-MM-DD')                                    ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                      ");
            parameter.AppendSql(" ORDER BY GUBUN                                                                ");

            parameter.Add("SDATE", argSDate);
            parameter.Add("LTDCODE", argLtdCode);

            return ExecuteReader<HEA_RESV_LTD>(parameter);
        }

        public int DelDataByNotResv(List<long> argLtdList)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HEA_RESV_LTD        ");
            parameter.AppendSql(" WHERE SDATE >= TRUNC(SYSDATE)         ");
            if (!argLtdList.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND LTDCODE NOT IN (:LTDCODE)   ");
            }

            if (!argLtdList.IsNullOrEmpty())
            {
                parameter.AddInStatement("LTDCODE", argLtdList);
            }
            
            return ExecuteNonQuery(parameter);
        }

        public IList<HEA_RESV_EXAM> GetItemsToGroupby(string argDate, long argLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBEXAM,SUM(DECODE(A.AMPM,'A',1,1)) AS AMCNT             ");
            parameter.AppendSql("      ,SUM(DECODE(A.AMPM,'A',0,1)) AS PMCNT                    ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_EXAM A                             ");
            parameter.AppendSql("       ,ADMIN.HIC_PATIENT B                              ");
            parameter.AppendSql(" WHERE 1 =1                                                    ");
            parameter.AppendSql("   AND A.RTIME >= TO_DATE(:RTIME, 'YYYY-MM-DD')                ");
            parameter.AppendSql("   AND A.RTIME <= TO_DATE(:RTIME, 'YYYY-MM-DD') + 0.99999      ");
            parameter.AppendSql("   AND A.DELDATE IS NULL                                       ");
            parameter.AppendSql("   AND A.PANO = B.PANO(+)                                      ");
            parameter.AppendSql("   AND B.LTDCODE = :LTDCODE                                    ");
            parameter.AppendSql(" GROUP BY GBEXAM");

            parameter.Add("RTIME", argDate);
            parameter.Add("LTDCODE", argLtdCode);

            return ExecuteReader<HEA_RESV_EXAM>(parameter);
        }

        public int InsertData(HEA_RESV_LTD data)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO ADMIN.HEA_RESV_LTD (                            ");
            parameter.AppendSql("        SDATE,GUBUN,LTDCODE,ENTSABUN,ENTTIME,AMINWON,PMINWON,      ");
            parameter.AppendSql("        AMJEPSU,PMJEPSU,AMJAN,PMJAN,YOIL                           ");
            parameter.AppendSql(") VALUES (                                                         ");
            parameter.AppendSql("       TO_DATE(:SDATE,'YYYY-MM-DD'),:GUBUN,:LTDCODE,:ENTSABUN,SYSDATE,:AMINWON,:PMINWON ");
            parameter.AppendSql("      ,:AMJEPSU,:PMJEPSU,:AMJAN,:PMJAN,:YOIL )                     ");

            #region Query 변수대입
            parameter.Add("SDATE",      data.SDATE);
            parameter.Add("GUBUN",      data.GUBUN);
            parameter.Add("LTDCODE",    data.LTDCODE);
            parameter.Add("ENTSABUN",   data.ENTSABUN);
            parameter.Add("AMINWON",    data.AMINWON);
            parameter.Add("PMINWON",    data.PMINWON);
            parameter.Add("AMJEPSU",    data.AMJEPSU);
            parameter.Add("PMJEPSU",    data.PMJEPSU);
            parameter.Add("AMJAN",      data.AMJAN);
            parameter.Add("PMJAN",      data.PMJAN);
            parameter.Add("YOIL",       data.YOIL);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public HEA_RESV_LTD GetInwonAmPm(string argDate, long argLtdCode, string argGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT AMINWON, PMINWON, ROWID AS RID          ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_LTD                ");
            parameter.AppendSql(" WHERE SDATE   = TO_DATE(:SDATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                      ");
            parameter.AppendSql("   AND GUBUN   = :GUBUN                        ");

            parameter.Add("SDATE", argDate);
            parameter.Add("LTDCODE", argLtdCode);
            parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_RESV_LTD>(parameter);
        }
    }
}
