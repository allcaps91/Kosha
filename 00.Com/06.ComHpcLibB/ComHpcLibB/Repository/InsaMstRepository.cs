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
    public class InsaMstRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public InsaMstRepository()
        {
        }

        public string GetToiDay(string idNumber)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(TOIDAY,'YYYY-MM-DD') TOIDAY     ");
            parameter.AppendSql("  FROM ADMIN.INSA_MST                     ");
            parameter.AppendSql(" WHERE SABUN = :SABUN                          ");

            parameter.Add("SABUN", idNumber, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<INSA_MST> GetSabunCodebyKorName(string strName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SABUN CODE,KORNAME NAME                 ");
            parameter.AppendSql("  FROM ADMIN.INSA_MST                     ");
            if (!strName.IsNullOrEmpty())
            {
                parameter.AppendSql(" WHERE KORNAME LIKE :KORNAME               ");
            }
            else
            {
                parameter.AppendSql(" WHERE KORNAME IS NOT NULL                 ");
            }
            parameter.AppendSql("   AND JAEGU = '0'                             ");
            parameter.AppendSql("   AND ROWNUM <= 500                           ");
            parameter.AppendSql(" ORDER BY KORNAME                              ");

            if (!strName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("KORNAME", strName);
            }

            return ExecuteReader<INSA_MST>(parameter);
        }

        public string GetKornameBySabun(string argSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT KORNAME                                 ");
            parameter.AppendSql("  FROM ADMIN.INSA_MST                     ");
            parameter.AppendSql(" WHERE SABUN = :SABUN                          ");

            parameter.Add("SABUN", argSabun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetKornameByMyenBunho(string argBunho)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT KORNAME                              ");
            parameter.AppendSql("  FROM ADMIN.INSA_MST                     ");
            parameter.AppendSql(" WHERE MYEN_BUNHO = :MYENBUNHO                 ");

            parameter.Add("MYENBUNHO", argBunho);

            return ExecuteScalar<string>(parameter);
        }

    }
}
