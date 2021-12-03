namespace ComHpcLibB.Repository
{
    using System;
    using ComBase;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class BasSunRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public BasSunRepository()
        {
        }

        /// <summary>
        /// 수가이름 읽어오기
        /// </summary>
        /// <param name="gstrRetValue"></param>
        /// <returns></returns>
        /// <seealso cref="HaMain.bas> READ_Suga_Name"/>
        public BAS_SUN FindSugaName(string gstrRetValue)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SuNext,SuNameK                     ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "BAS_SUN      ");
            parameter.AppendSql(" WHERE 1 = 1                              ");
            parameter.AppendSql("   AND SUNEXT =:SUNEXT                   ");

            parameter.Add("SUNEXT", gstrRetValue.Trim(), Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<BAS_SUN>(parameter);
        }

        public BAS_SUN GetSuNamekUnitbySuCode(string strSuCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUNAMEK, UNIT           ");
            parameter.AppendSql("  FROM ADMIN.BAS_SUN     ");
            parameter.AppendSql(" WHERE SUNEXT = :SUNEXT        ");

            parameter.Add("SUNEXT", strSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<BAS_SUN>(parameter);
        }

        public BAS_SUN GetSuNameKbySuNext(string strSuCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUNAMEK,UNITNEW2        ");
            parameter.AppendSql("  FROM ADMIN.BAS_SUN     ");
            parameter.AppendSql(" WHERE SUNEXT = :SUNEXT        ");

            parameter.Add("SUNEXT", strSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<BAS_SUN>(parameter);
        }

        public BAS_SUN GetItembySuCode(string strSuCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT UNITNEW1, UNITNEW2, UNITNEW3, UNITNEW4  ");
            parameter.AppendSql("  FROM ADMIN.BAS_SUN                     ");
            parameter.AppendSql(" WHERE SUNEXT = :SUNEXT                        ");

            parameter.Add("SUNEXT", strSuCode);

            return ExecuteReaderSingle<BAS_SUN>(parameter);
        }
    }
}
