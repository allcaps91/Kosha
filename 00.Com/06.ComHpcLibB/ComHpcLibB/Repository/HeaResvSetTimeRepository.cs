
namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HeaResvSetTimeRepository : BaseRepository
    {

        public HeaResvSetTimeRepository()
        {

        }

        public HEA_RESV_SET_TIME GetItemByGubun(string argDate, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT INWON, GAINWON, ROWID AS RID            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_SET_TIME           ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                          ");

            parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SDATE", argDate);

            return ExecuteReaderSingle<HEA_RESV_SET_TIME>(parameter);
        }

        public HEA_RESV_SET_TIME GetItemBySTimeGubun(string argDate, string argSTime, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT INWON, GAINWON, ROWID AS RID            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_SET_TIME           ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                          ");
            parameter.AppendSql("   AND STIME = :STIME                          ");

            parameter.Add("SDATE", argDate);
            parameter.Add("STIME", argSTime);
            parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            
            return ExecuteReaderSingle<HEA_RESV_SET_TIME>(parameter);
        }

        public List<HEA_RESV_SET_TIME> GetSumInwonAMPMByGubun(string argDate, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SDATE, STIME, SUM(INWON ) AS INWON, SUM(GAINWON) AS GAINWON ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_SET_TIME                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                       ");
            parameter.AppendSql("   AND SDATE  = TO_DATE(:SDATE, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                                              ");
            parameter.AppendSql("   GROUP BY SDATE, STIME                                           ");
            parameter.AppendSql("   ORDER BY STIME                                          ");

            parameter.Add("SDATE", argDate);
            parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_RESV_SET_TIME>(parameter);
        }

        public void DeleteBySDate(string argGetSDate, string argGetLDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE FROM KOSMOS_PMPA.HEA_RESV_SET_TIME      ");
            parameter.AppendSql(" WHERE SDATE >=TO_DATE(:FDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND SDATE <=TO_DATE(:TDATE, 'YYYY-MM-DD')   ");

            parameter.Add("FDATE", argGetSDate);
            parameter.Add("TDATE", argGetLDate);

            ExecuteNonQuery(parameter);
        }
        public List <HEA_RESV_SET_TIME> GetAmPmInwonByGubun(string argDate, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT STIME, INWON, GAINWON, ROWID AS RID             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_SET_TIME                   ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            parameter.AppendSql("   AND SDATE  = TO_DATE(:SDATE, 'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN                                 ");

            parameter.Add("SDATE", argDate);
            parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_RESV_SET_TIME>(parameter);
        }

        public int Insert(HEA_RESV_SET_TIME dto)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_RESV_SET_TIME (                            ");
            parameter.AppendSql("       SDATE,STIME,GUBUN,ENTSABUN,ENTTIME,INWON,GAINWON)               ");
            parameter.AppendSql(" VALUES (                                                              ");
            parameter.AppendSql("      TO_DATE(:SDATE,'YYYY-MM-DD'),:STIME,:GUBUN,:ENTSABUN,SYSDATE     ");
            parameter.AppendSql("      ,:INWON,:GAINWON)          ");

            parameter.Add("SDATE", dto.SDATE);
            parameter.Add("STIME", dto.STIME);
            parameter.Add("GUBUN", dto.GUBUN);
            parameter.Add("ENTSABUN", dto.ENTSABUN);
            parameter.Add("INWON", dto.INWON);
            parameter.Add("GAINWON", dto.GAINWON);


            return ExecuteScalar<int>(parameter);
        }

        public int UpDate(HEA_RESV_SET_TIME dto)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_RESV_SET_TIME   ");
            parameter.AppendSql("   SET INWON     =:INWON               ");
            parameter.AppendSql("      ,GAINWON   =:GAINWON             ");
            parameter.AppendSql("      ,STIME     =:STIME               ");
            parameter.AppendSql("      ,ENTSABUN  =:ENTSABUN            ");
            parameter.AppendSql("      ,ENTTIME   =SYSDATE              ");
            parameter.AppendSql(" WHERE ROWID     =:RID                 ");

            parameter.Add("INWON", dto.INWON);
            parameter.Add("GAINWON", dto.GAINWON);
            parameter.Add("STIME", dto.STIME);
            parameter.Add("ENTSABUN", dto.ENTSABUN);
            parameter.Add("RID", dto.RID);

            return ExecuteNonQuery(parameter);
        }


    }
}
