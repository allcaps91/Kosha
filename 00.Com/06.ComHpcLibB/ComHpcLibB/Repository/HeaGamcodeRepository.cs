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
    public class HeaGamcodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaGamcodeRepository()
        {
        }
        
        public string Read_Hea_GamName(string CODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GAMCODE     ");
            parameter.AppendSql(" WHERE CODE =:CODE                ");

            parameter.Add("CODE", CODE, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HEA_GAMCODE> GetListItems(bool bDel)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME, GUBUN, RATE, AMT1, AMT2                 ");
            parameter.AppendSql("      ,TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE ,ROWID AS RID ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GAMCODE                             ");
            parameter.AppendSql(" WHERE 1 = 1                                               ");
            if (!bDel)
            {
                parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE>TRUNC(SYSDATE))     ");
            }
            parameter.AppendSql(" ORDER BY CODE                                             ");

            return ExecuteReader<HEA_GAMCODE>(parameter);
        }

        public void Delete(HEA_GAMCODE code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_GAMCODE                 ");
            parameter.AppendSql("   SET DELDATE =TRUNC(SYSDATE)                 ");
            parameter.AppendSql(" WHERE ROWID =:RID                             ");

            parameter.Add("RID", code.RID);

            ExecuteNonQuery(parameter);
        }

        public HEA_GAMCODE GetItemByCode(string strGamCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME, GUBUN, RATE, AMT1, AMT2                 ");
            parameter.AppendSql("      ,TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE ,ROWID AS RID ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GAMCODE                             ");
            parameter.AppendSql(" WHERE CODE = :CODE                                        ");

            parameter.Add("CODE", strGamCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_GAMCODE>(parameter);
        }

        public void Update(HEA_GAMCODE code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_GAMCODE                 ");
            parameter.AppendSql("   SET NAME =:NAME                             ");
            parameter.AppendSql("      ,GUBUN =:GUBUN                           ");
            parameter.AppendSql("      ,RATE =:RATE                             ");
            parameter.AppendSql("      ,AMT1 =:AMT1                             ");
            parameter.AppendSql("      ,AMT2 =:AMT2                             ");
            parameter.AppendSql("      ,ENTTIME =SYSDATE                        ");
            parameter.AppendSql("      ,ENTSABUN =ENTSABUN                      ");
            parameter.AppendSql(" WHERE ROWID =:RID                            ");

            parameter.Add("NAME", code.NAME);
            parameter.Add("GUBUN", code.GUBUN);
            parameter.Add("RATE", code.RATE);
            parameter.Add("AMT1", code.AMT1);
            parameter.Add("AMT2", code.AMT2);
            parameter.Add("ENTSABUN", clsType.User.IdNumber);
            parameter.Add("RID", code.RID);

            ExecuteNonQuery(parameter);
        }

        public void Insert(HEA_GAMCODE code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT KOSMOS_PMPA.HEA_GAMCODE (                           ");
            parameter.AppendSql("   CODE,NAME,GUBUN,RATE,AMT1,AMT2,ENTTIME,ENTSABUN )       ");
            parameter.AppendSql(" VALUES (                                                  ");
            parameter.AppendSql("   :CODE,:NAME,:GUBUN,:RATE,:AMT1,:AMT2,SYSDATE,:ENTSABUN )");

            parameter.Add("CODE", code.CODE);
            parameter.Add("NAME", code.NAME);
            parameter.Add("GUBUN", code.GUBUN);
            parameter.Add("RATE", code.RATE);
            parameter.Add("AMT1", code.AMT1);
            parameter.Add("AMT2", code.AMT2);
            parameter.Add("ENTSABUN", clsType.User.IdNumber);

            ExecuteNonQuery(parameter);
        }
    }
}
