namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HicRescodeRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HicRescodeRepository()
        {
        }

        public List<HIC_RESCODE> Read_ResCode(string GUBUN)
        {
            MParameter parameter = CreateParameter();
            //parameter.AppendSql("SELECT '' GUBUN, '' CODE, '' NAME, '' GBFLAG, '' MARK, '' RANK         ");
            //parameter.AppendSql("     , '' CODENAME                                                     ");
            //parameter.AppendSql("  FROM DUAL                                                            ");
            //parameter.AppendSql(" UNION ALL                                                             ");
            parameter.AppendSql("SELECT GUBUN, CODE, NAME, GBFLAG, MARK, RANK                           ");
            parameter.AppendSql("     , CODE || '.' || NAME CODENAME                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESCODE                                         ");
            parameter.AppendSql(" WHERE Gubun = :GUBUN                                                  ");

            parameter.Add("GUBUN", GUBUN, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESCODE>(parameter);
        }

        public HIC_RESCODE Read_ResCode_Single(string GUBUN, string CODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUBUN, CODE, NAME, GBFLAG, MARK, RANK       ");
            parameter.AppendSql("     , CODE || '.' || NAME CODENAME                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESCODE                     ");
            parameter.AppendSql(" WHERE Gubun = :GUBUN                              ");
            parameter.AppendSql("   AND Code  = :CODE                               ");

            parameter.Add("GUBUN", GUBUN, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", CODE, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_RESCODE>(parameter);
        }

        public string GetNameByGubun(string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESCODE ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            parameter.AppendSql(" ORDER BY CODE                 ");

            parameter.Add("GUBUN", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_RESCODE GetCodeNamebyGubunCode(string strResCode, string strResult)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESCODE ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            parameter.AppendSql("   AND CODE  = :CODE           ");
            parameter.AppendSql(" ORDER BY CODE                 ");

            parameter.Add("GUBUN", strResCode, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", strResult, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_RESCODE>(parameter);
        }

        public List<HIC_RESCODE> GetCodeNamebyBindGubun(string strResCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESCODE ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            parameter.AppendSql(" ORDER BY CODE                 ");

            parameter.Add("GUBUN", strResCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESCODE>(parameter);
        }

        public List<HIC_RESCODE> GetCodeNamebyGubun(string strYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESCODE ");
            if (string.Compare(strYear, "2019") >= 0)
            {
                parameter.AppendSql(" WHERE GUBUN = '114'       ");
            }
            else
            {
                parameter.AppendSql(" WHERE GUBUN = '113'       ");
            }            
            parameter.AppendSql(" ORDER BY CODE                 ");

            return ExecuteReader<HIC_RESCODE>(parameter);
        }

        public List<HIC_RESCODE> GetCodeNamebyResCode(string strResCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESCODE ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            parameter.AppendSql(" ORDER BY RANK, CODE           ");

            parameter.Add("GUBUN", strResCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESCODE>(parameter);
        }

        public List<HIC_RESCODE> Read_Res_ComboSet(string argGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESCODE ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESCODE>(parameter);
        }

        public List<HIC_RESCODE> Read_Hic_ResCode_All(string GUBUN)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME, ROWID RID                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESCODE                 ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                          ");
            parameter.AppendSql(" ORDER BY CODE                                 ");

            parameter.Add("GUBUN", GUBUN, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESCODE>(parameter);
        }

        public List<HIC_RESCODE> Read_HIc_ResCode(string GUBUN, string CODE, string sTemp)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME, ROWID RID                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESCODE                 ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                          ");
            if (sTemp == "IN")
            {
                parameter.AppendSql("   AND CODE IN :CODE                       ");
                parameter.AppendSql(" ORDER BY CODE                             ");
            }
            else
            {
                parameter.AppendSql("   AND CODE  = :CODE                       ");
            }

            parameter.Add("GUBUN", GUBUN, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", CODE, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESCODE>(parameter);
        }

        public string Read_Hic_ResCodeName(string GUBUN, string CODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESCODE                 ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                          ");
            parameter.AppendSql("   AND CODE  = :CODE                           ");

            parameter.Add("GUBUN", GUBUN, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", CODE, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }
    }
}





