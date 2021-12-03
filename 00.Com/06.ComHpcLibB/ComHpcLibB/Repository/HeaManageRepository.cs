namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaManageRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaManageRepository()
        {
        }

        public string GetWrtNobyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID AS RID                ");
            parameter.AppendSql("  FROM ADMIN.HEA_MANAGE  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int Insert(long nWRTNO, string strJEPDATE, string strLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HEA_MANAGE                             ");
            parameter.AppendSql("       (WRTNO, JEPDATE, LTDCODE)                               ");
            parameter.AppendSql("VALUES                                                         ");
            parameter.AppendSql("       (:WRTNO, TO_DATE(:JEPDATE, 'YYYY-MM-DD'), :LTDCODE)     ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("JEPDATE", strJEPDATE);
            parameter.Add("LTDCODE", strLtdCode);

            return ExecuteNonQuery(parameter);
        }
    }
}
