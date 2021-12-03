namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicSpcSahuRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcSahuRepository()
        {
        }

        public int GetRowIdbyWrtNo(long fnWRTNO, string fstrGjYear, string fstrLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_SAHU   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR            ");
            if (fstrLtdCode != "")
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE      ");
            }

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("GJYEAR", fstrGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (fstrLtdCode != "")
            {
                parameter.Add("LTDCODE", fstrLtdCode);
            }

            return ExecuteScalar<int>(parameter);
        }

        public int Insert(HIC_SPC_SAHU item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SPC_SAHU                           ");
            parameter.AppendSql("       (GJYEAR, LTDCODE, WRTNO, GUBUN, SOGEN, EXAM, JOCHI)     ");
            parameter.AppendSql("VALUES                                                         ");
            parameter.AppendSql("       (:GJYEAR,:LTDCODE,:WRTNO,:GUBUN,:SOGEN,:EXAM,:JOCHI)    ");

            parameter.Add("GJYEAR", item.GJYEAR, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", item.LTDCODE);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("GUBUN", item.GUBUN);
            parameter.Add("SOGEN", item.SOGEN);
            parameter.Add("EXAM", item.EXAM);
            parameter.Add("JOCHI", item.JOCHI);

            return ExecuteNonQuery(parameter);
        }
    }
}
