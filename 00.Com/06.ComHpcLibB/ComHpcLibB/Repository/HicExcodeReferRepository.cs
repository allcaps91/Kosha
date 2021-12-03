namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicExcodeReferRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicExcodeReferRepository()
        {
        }

        public List<HIC_EXCODE_REFER> GetItemByCode(string argCode, string strFDate, string argGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,FROMDATE,TODATE,EXCODE,MIN_M,MIN_MB,MIN_MR,MAX_M,MAX_MB,MAX_MR         ");
            parameter.AppendSql("      ,MIN_F, MIN_FB, MIN_FR, MAX_F, MAX_FB, MAX_FR, EXAMFRTO, RESULTTYPE, UNIT    ");
            parameter.AppendSql("      ,GUBUN, DECODE(GUBUN, '1','일반','2','종검','3','특수','') AS GUBUNNM        ");
            parameter.AppendSql("  FROM ADMIN.HIC_EXCODE_REFER                                                ");
            parameter.AppendSql(" WHERE CODE = :CODE                                                                ");
            parameter.AppendSql("   AND FROMDATE >= TO_DATE(:FROMDATE,'YYYY-MM-DD')                                 ");
            //parameter.AppendSql("   AND (TODATE IS NULL OR TODATE < TO_DATE(:FROMDATE,'YYYY-MM-DD'))                ");
            parameter.AppendSql("   AND GUBUN =:GUBUN                                                               ");
            parameter.AppendSql(" ORDER By FROMDATE DESC                                                            ");

            parameter.Add("CODE", argCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FROMDATE", strFDate);
            parameter.Add("GUBUN", argGbn);

            return ExecuteReader<HIC_EXCODE_REFER>(parameter);
        }

        public void Update(HIC_EXCODE_REFER code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_EXCODE_REFER        ");
            parameter.AppendSql("   SET FROMDATE =:FROMDATE                 ");
            parameter.AppendSql("       MIN_M =:MIN_M                       ");
            parameter.AppendSql("       MIN_MB =:MIN_MB                     ");
            parameter.AppendSql("       MIN_MR =:MIN_MR                     ");
            parameter.AppendSql("       MAX_M =:MAX_M                       ");
            parameter.AppendSql("       MAX_MB =:MAX_MB                     ");
            parameter.AppendSql("       MAX_MR =:MAX_MR                     ");

            parameter.AppendSql("       MIN_F =:MIN_F                       ");
            parameter.AppendSql("       MIN_FB =:MIN_FB                     ");
            parameter.AppendSql("       MIN_FR =:MIN_FR                     ");
            parameter.AppendSql("       MAX_F =:MAX_F                       ");
            parameter.AppendSql("       MAX_FB =:MAX_FB                     ");
            parameter.AppendSql("       MAX_FR =:MAX_FR                     ");

            parameter.Add("FROMDATE", code.FROMDATE);
            parameter.Add("MIN_M",    code.MIN_M);
            parameter.Add("MIN_MB",   code.MIN_MB);
            parameter.Add("MIN_MR",   code.MIN_MR);
            parameter.Add("MAX_M",    code.MAX_M);
            parameter.Add("MAX_MB",   code.MAX_MB);
            parameter.Add("MAX_MR",   code.MAX_MR);
            parameter.Add("MIN_F",    code.MIN_F);
            parameter.Add("MIN_FB",   code.MIN_FB);
            parameter.Add("MIN_FR",   code.MIN_FR);
            parameter.Add("MAX_F",    code.MAX_F);
            parameter.Add("MAX_FB",   code.MAX_FB);
            parameter.Add("MAX_FR",   code.MAX_FR);
          
            ExecuteNonQuery(parameter);
        }
        
        public void Insert(HIC_EXCODE_REFER code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_EXCODE_REFER (                                             ");
            parameter.AppendSql("   CODE,FROMDATE,MIN_M,MIN_MB,MIN_MR,MAX_M,MAX_MB,MAX_MR                               ");
            parameter.AppendSql("  ,MIN_F, MIN_FB, MIN_FR, MAX_F, MAX_FB, MAX_FR, GUBUN                                 ");
            parameter.AppendSql(" ) VALUES (                                                                            ");
            parameter.AppendSql("   :CODE,:FROMDATE,:MIN_M,:MIN_MB,:MIN_MR,:MAX_M,:MAX_MB,:MAX_MR                       ");
            parameter.AppendSql("  ,:MIN_F,:MIN_FB,:MIN_FR,:MAX_F,:MAX_FB,:MAX_FR,:GUBUN )                              ");

            parameter.Add("CODE",       code.CODE);
            parameter.Add("FROMDATE",   code.FROMDATE);
            //parameter.Add("TODATE",     code.TODATE);
            //parameter.Add("EXCODE",     code.EXCODE);
            parameter.Add("MIN_M",      code.MIN_M);
            parameter.Add("MIN_MB",     code.MIN_MB);
            parameter.Add("MIN_MR",     code.MIN_MR);
            parameter.Add("MAX_M",      code.MAX_M);
            parameter.Add("MAX_MB",     code.MAX_MB);
            parameter.Add("MAX_MR",     code.MAX_MR);
            parameter.Add("MIN_F",      code.MIN_F);
            parameter.Add("MIN_FB",     code.MIN_FB);
            parameter.Add("MIN_FR",     code.MIN_FR);
            parameter.Add("MAX_F",      code.MAX_F);
            parameter.Add("MAX_FB",     code.MAX_FB);
            parameter.Add("MAX_FR",     code.MAX_FR);
            //parameter.Add("EXAMFRTO",   code.EXAMFRTO);
            //parameter.Add("RESULTTYPE", code.RESULTTYPE);
            //parameter.Add("UNIT",       code.UNIT);
            parameter.Add("GUBUN",      code.GUBUN);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_EXCODE_REFER> FindAll(string strResultType)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, FROMDATE, TODATE                  ");
            parameter.AppendSql("     , EXCODE, MIN_M, MIN_MB                   ");
            parameter.AppendSql("     , MIN_MR, MAX_M, MAX_MB                   ");
            parameter.AppendSql("     , MAX_MR, MIN_F, MIN_FB                   ");
            parameter.AppendSql("     , MIN_FR, MAX_F, MAX_FB                   ");
            parameter.AppendSql("     , MAX_FR, EXAMFRTO, RESULTTYPE            ");
            parameter.AppendSql("     , UNIT                                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_EXCODE_REFER            ");
            parameter.AppendSql(" WHERE RESULTTYPE :RESULTTYPE                  ");
            parameter.AppendSql(" ORDER BY Code                                 ");

            parameter.Add("RESULTTYPE", strResultType, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_EXCODE_REFER>(parameter);
        }
    }
}
