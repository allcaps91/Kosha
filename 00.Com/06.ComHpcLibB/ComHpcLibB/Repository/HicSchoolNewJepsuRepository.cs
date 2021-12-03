namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicSchoolNewJepsuRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSchoolNewJepsuRepository()
        {
        }

        public HIC_SCHOOL_NEW_JEPSU GetCntbyGjJong(string strGjJong, long License)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(DECODE(a.DPANDRNO, :LICENSE, 1)) CNT              ");
            parameter.AppendSql("     , COUNT(DECODE(a.DPANDRNO,'', 1)) CNT2                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_SCHOOL_NEW a, ADMIN.HIC_JEPSU b   ");
            parameter.AppendSql(" WHERE b.WRTNO = a.WRTNO                                       ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                       ");
            parameter.AppendSql("   AND b.GJJONG = :GJJONG                                      ");
            parameter.AppendSql("   AND b.JEPDATE = TRUNC(SYSDATE)                              ");

            parameter.Add("LICENSE", License);
            parameter.Add("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_SCHOOL_NEW_JEPSU>(parameter);
        }
    }
}
