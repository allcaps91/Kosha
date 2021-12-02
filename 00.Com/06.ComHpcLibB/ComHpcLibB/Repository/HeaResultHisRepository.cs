namespace ComHpcLibB.Repository
{

    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HeaResultHisRepository : BaseRepository
    {

        public HeaResultHisRepository()
        {

        }

        public int Result_History_Insert2(long SABUN, string RESULT, long WRTNO, string EXCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_RESULT_HISTORY                                 ");
            parameter.AppendSql("       (JOBTIME,JOBSABUN,WRTNO,EXCODE,RESULT_OLD,RESULT_NEW)               ");
            parameter.AppendSql("SELECT SYSDATE, :SABUN, WRTNO, EXCODE, RESULT, :RESULT                     ");
            parameter.AppendSql("  From KOSMOS_PMPA.HEA_RESULT                                              ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                      ");
            parameter.AppendSql("   AND EXCODE = :EXCODE                                                    ");
            parameter.AppendSql("   AND RESULT IS NOT NULL                                                  ");
            parameter.AppendSql("   AND Result != :RESULT                                                   ");

            parameter.Add("SABUN", SABUN);
            parameter.Add("RESULT", RESULT.Replace("'", "`"));
            parameter.Add("WRTNO", WRTNO);
            parameter.Add("EXCODE", EXCODE, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int Result_History_Update(string RESULT, long ENTSABUN, long WRTNO, string EXCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_RESULT SET              ");
            parameter.AppendSql("       RESULT   = :RESULT                      ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                    ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE                      ");
            parameter.AppendSql("     , ACTIVE   = 'Y'                          ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                       ");
            parameter.AppendSql("   AND EXCODE   = :EXCODE                      ");

            parameter.Add("RESULT", RESULT.Replace("'", "`"));
            parameter.Add("ENTSABUN", ENTSABUN);
            parameter.Add("WRTNO", WRTNO);
            parameter.Add("EXCODE", EXCODE, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }


    }
}
