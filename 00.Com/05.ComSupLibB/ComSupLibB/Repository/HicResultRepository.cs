namespace ComSupLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComSupLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicResultRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicResultRepository()
        {
        }

        public string GetResultByWrtnoExCD(long argWRTNO, string argExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RESULT FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE 1 = 1                               ");
            parameter.AppendSql("   AND WRTNO =:WRTNO                       ");
            parameter.AppendSql("   AND EXCODE=:EXCODE                      ");

            parameter.Add("WRTNO", argWRTNO);
            parameter.Add("EXCODE", argExCode);

            return ExecuteScalar<string>(parameter);
        }

        public int UpDate(string argResult, long argSabun, long argWRTNO, string argExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql("   SET RESULT     = :RESULT        ");
            parameter.AppendSql("      ,ACTIVE     = 'Y'            ");
            parameter.AppendSql("      ,ENTTIME    = SYSDATE        ");
            parameter.AppendSql("      ,ENTSABUN   = :ENTSABUN      ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO         ");
            parameter.AppendSql("   AND EXCODE     = :EXCODE        ");

            #region Query 변수대입
            parameter.Add("RESULT", argResult);
            parameter.Add("ENTSABUN", argSabun);
            parameter.Add("WRTNO", argWRTNO);
            parameter.Add("EXCODE", argExCode);
            #endregion

            return ExecuteNonQuery(parameter);
        }
    }
}
