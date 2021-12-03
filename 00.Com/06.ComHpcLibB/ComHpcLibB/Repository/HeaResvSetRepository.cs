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
    public class HeaResvSetRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaResvSetRepository()
        {
        }

        public HEA_RESV_SET GetItemByGubun(string argDate, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT AMINWON, PMINWON, ROWID AS RID          ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_SET                ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')   ");
            if (argGubun.Equals("00"))
            {
                parameter.AppendSql("   AND GUBUN = 'TT'                        ");
            }
            else
            {
                parameter.AppendSql("   AND GUBUN = :GUBUN                      ");
                parameter.AppendSql("   AND GBRESV='1'                          ");

                parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            parameter.Add("SDATE", argDate);

            return ExecuteReaderSingle<HEA_RESV_SET>(parameter);
        }

        public HEA_RESV_SET GetCountByGubunYoil(string argDate, string argResv, string argGubun, string argYoil)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT AMINWON,PMINWON,YOIL,ROWID AS RID               ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_SET                        ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            parameter.AppendSql("   AND SDATE  = TO_DATE(:SDATE, 'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND GBRESV = :GBRESV                                ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN                                 ");
            parameter.AppendSql("   AND YOIL   = :YOIL                                  ");

            parameter.Add("SDATE", argDate);
            parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBRESV", argResv, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("YOIL", argYoil, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_RESV_SET>(parameter);
        }

        public long GetSumAmPmInwonBySDate(string argDate, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SUM(AMINWON + PMINWON) AS TOTCNT  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_SET                        ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            parameter.AppendSql("   AND SDATE  = TO_DATE(:SDATE, 'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN                                 ");

            parameter.Add("SDATE", argDate);
            parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<long>(parameter);
        }

        public void Delete(HEA_RESV_SET code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE FROM ADMIN.HEA_RESV_SET  ");
            parameter.AppendSql(" WHERE ROWID =:RID                    ");

            parameter.Add("RID", code.RID);

            ExecuteNonQuery(parameter);
        }

        public List<HEA_RESV_SET> GetListBySDateGubun(string strRDate, string strGb)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GUBUN,EXAMNAME,ENTSABUN,ENTTIME,AMINWON,PMINWON,GAINWONAM,GAINWONPM,GBRESV ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_SET                        ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            parameter.AppendSql("   AND SDATE  = TO_DATE(:SDATE, 'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN                                 ");

            parameter.Add("SDATE", strRDate);
            parameter.Add("GUBUN", strGb, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_RESV_SET>(parameter);
        }

        public List<HEA_RESV_SET> GetItembyYDateLtdCode(string strFDate, string strTDate, string strLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,TO_CHAR(YDATE,'YYYY-MM-DD') YDATE, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE          ");
            parameter.AppendSql("     , PANO,SNAME,SEX,AGE,TO_CHAR(IDATE,'YYYY-MM-DD') IDATE,GBSTS,GJJONG,GRPPER,LTDCODE    ");
            parameter.AppendSql("     , REMARK,GAMCODE,SABUN,MAILCODE,JUSO1,JUSO2,PTNO,BURATE,SEXAMS,GBEXAM,GBINWON,DELDATE ");
            parameter.AppendSql("     , JOBSABUN,enttime,ROWID                                                              ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESERVED                                                            ");
            parameter.AppendSql(" WHERE YDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                             ");
            parameter.AppendSql("   AND YDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                             ");
            parameter.AppendSql("   and deldate is null                                                                     ");
            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                              ");
            }

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", strLtdCode);
            }

            return ExecuteReader<HEA_RESV_SET>(parameter);
        }

        public void DeleteBySDate(string argGetSDate, string argGetLDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE FROM ADMIN.HEA_RESV_SET           ");
            parameter.AppendSql(" WHERE SDATE >=TO_DATE(:FDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND SDATE <=TO_DATE(:TDATE, 'YYYY-MM-DD')   ");

            parameter.Add("FDATE", argGetSDate);
            parameter.Add("TDATE", argGetLDate);

            ExecuteNonQuery(parameter);
        }

        public IList<HEA_RESV_SET> GetSumGaInwonSDateBySDateGbResv(string argGetSDate, string argGetLDate, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SUM(GAINWONAM) AS GAINWONAM, SUM(GAINWONPM) AS GAINWONPM    ");
            parameter.AppendSql("      ,TO_CHAR(SDATE,'YYYY-MM-DD') SDATE                           ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_SET                                    ");
            parameter.AppendSql(" WHERE 1 = 1                                                       ");
            parameter.AppendSql("   AND SDATE >= TO_DATE(:FSDATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND SDATE <= TO_DATE(:TSDATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN                                             ");
            parameter.AppendSql("   AND GBRESV = '2'                                                ");
            parameter.AppendSql(" GROUP BY SDATE                                                ");
            parameter.AppendSql(" ORDER BY SDATE                                                ");

            parameter.Add("FSDATE", argGetSDate);
            parameter.Add("TSDATE", argGetLDate);
            parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_RESV_SET>(parameter);
        }

        public List<HEA_RESV_SET> GetListGaInwonBySDate(string argDate, string argGbn, int argRow)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT EXAMNAME, GAINWONAM, GAINWONPM, ENTSABUN                            ");
            parameter.AppendSql("     , ADMIN.FC_INSA_MST_KORNAME(ENTSABUN) AS JOBNAME, ROWID AS RID   ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_SET                                            ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND SDATE  = TO_DATE(:SDATE, 'YYYY-MM-DD')                              ");
            if (argRow == 1)
            {
                parameter.AppendSql("   AND GUBUN  = '00'                                                   ");
            }
            else
            {
                parameter.AppendSql("   AND GUBUN  = :GUBUN                                                 ");
            }
            parameter.AppendSql("   AND GBRESV = '2'                                                        ");
            

            parameter.Add("SDATE", argDate);

            if (argRow != 1)
            {
                parameter.Add("GUBUN", argGbn, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            

            return ExecuteReader<HEA_RESV_SET>(parameter);
        }

        public HEA_RESV_SET GetSumInwonAMPMByGubun(string argDate, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SUM(AMINWON) AS AMINWON,     SUM(PMINWON) AS PMINWON        ");
            parameter.AppendSql("      ,SUM(GAINWONAM) AS GAINWONAM, SUM(GAINWONPM) AS GAINWONPM    ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_SET                                    ");
            parameter.AppendSql(" WHERE 1 = 1                                                       ");
            parameter.AppendSql("   AND SDATE  = TO_DATE(:SDATE, 'YYYY-MM-DD')                      ");
            if (argGubun == "00")
            {
                parameter.AppendSql("   AND GUBUN  = 'TT'                                           ");
            }
            else
            {
                parameter.AppendSql("   AND GUBUN = :GUBUN                                          ");
            }
            
            parameter.Add("SDATE", argDate);

            if (argGubun != "00")
            { 
                parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReaderSingle<HEA_RESV_SET>(parameter);
        }

        public HEA_RESV_SET GetAmPmInwonByGubun(string argDate, string argResv, string argGubun, string argYoil)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT AMINWON, PMINWON, YOIL, ROWID AS RID            ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_SET                        ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            parameter.AppendSql("   AND SDATE  = TO_DATE(:SDATE, 'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND GBRESV = :GBRESV                                ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN                                 ");
            if (!argYoil.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND YOIL  = :YOIL                               ");
            }

            parameter.Add("SDATE", argDate);
            parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBRESV", argResv, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!argYoil.IsNullOrEmpty())
            {
                parameter.Add("YOIL", argYoil);
            }

            return ExecuteReaderSingle<HEA_RESV_SET>(parameter);
        }

        public int UpDate(HEA_RESV_SET dto)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HEA_RESV_SET    ");
            parameter.AppendSql("   SET AMINWON   =:AMINWON         ");
            parameter.AppendSql("      ,PMINWON   =:PMINWON         ");
            parameter.AppendSql("      ,ENTSABUN  =:ENTSABUN        ");
            parameter.AppendSql("      ,YOIL      =:YOIL            ");
            parameter.AppendSql("      ,ENTTIME   =SYSDATE          ");

            parameter.AppendSql("      ,EXAMNAME   =:EXAMNAME          ");
            parameter.AppendSql("      ,GAINWONAM  =:GAINWONAM          ");
            parameter.AppendSql("      ,GAINWONPM  =:GAINWONPM          ");

            if (dto.GBRESV != "" && dto.GBRESV != null)
            {
                parameter.AppendSql("      ,GBRESV   =:GBRESV          ");
            }
            

            parameter.AppendSql(" WHERE ROWID     =:RID             ");

            parameter.Add("AMINWON", dto.AMINWON);
            parameter.Add("PMINWON", dto.PMINWON);
            parameter.Add("ENTSABUN", dto.ENTSABUN);
            parameter.Add("YOIL", dto.YOIL, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            parameter.Add("EXAMNAME", dto.EXAMNAME);
            parameter.Add("GAINWONAM", dto.GAINWONAM);
            parameter.Add("GAINWONPM", dto.GAINWONPM);

            if (dto.GBRESV != "" && dto.GBRESV != null)
            {
                parameter.Add("GBRESV", dto.GBRESV, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            parameter.Add("RID", dto.RID);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(HEA_RESV_SET dto)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HEA_RESV_SET (                                 ");
            parameter.AppendSql("       SDATE,GUBUN,EXAMNAME,ENTSABUN,ENTTIME,AMINWON,PMINWON           ");
            parameter.AppendSql("      ,GAINWONAM,GAINWONPM,GBRESV,YOIL )                               ");
            parameter.AppendSql(" VALUES (                                                              ");
            parameter.AppendSql("      TO_DATE(:SDATE,'YYYY-MM-DD'),:GUBUN,:EXAMNAME,:ENTSABUN,SYSDATE  ");
            parameter.AppendSql("      ,:AMINWON,:PMINWON,:GAINWONAM,:GAINWONPM,:GBRESV,:YOIL)          ");

            parameter.Add("SDATE",      dto.SDATE);
            parameter.Add("GUBUN",      dto.GUBUN);
            parameter.Add("EXAMNAME",   dto.EXAMNAME);
            parameter.Add("ENTSABUN",   dto.ENTSABUN);
            parameter.Add("AMINWON",    dto.AMINWON);
            parameter.Add("PMINWON",    dto.PMINWON);
            parameter.Add("GAINWONAM",  dto.GAINWONAM);
            parameter.Add("GAINWONPM",  dto.GAINWONPM);
            parameter.Add("GBRESV",     dto.GBRESV);
            parameter.Add("YOIL",       dto.YOIL);            

            return ExecuteScalar<int>(parameter);
        }

        public List<HEA_RESV_SET> GetListByInwonSet(string argDate, string argResv, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT AMINWON, PMINWON, YOIL, ENTSABUN                          ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_SET                        ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            parameter.AppendSql("   AND SDATE  = TO_DATE(:SDATE, 'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND GBRESV = :GBRESV                                ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN                                 ");

            parameter.Add("SDATE", argDate);
            parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBRESV", argResv, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_RESV_SET>(parameter);
        }

        public HEA_RESV_SET GetSumGaInwonAMPMByGubun(string argDate, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SUM(GAINWONAM) AS GAINWONAM, SUM(GAINWONPM) AS GAINWONPM  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESV_SET                        ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            parameter.AppendSql("   AND SDATE  = TO_DATE(:SDATE, 'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN                                 ");
            parameter.AppendSql("   AND GBRESV = '2'                                    ");
            
            parameter.Add("SDATE", argDate);
            parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_RESV_SET>(parameter);
        }
    }
}
