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
    public class HicLtdUcodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicLtdUcodeRepository()
        {
        }

        public IList<HIC_LTD_UCODE> GetListByCodeJong(long argLtdCode, string argJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JOBNAME, CASE WHEN NVL(SCODES, '') != '' THEN UCODES || '{★}' || SCODES         ");
            parameter.AppendSql("                     ELSE UCODES END AS VUCODE                                         ");
            parameter.AppendSql("      ,JONG, DECODE(JONG, '1','배치','2','특수','3','일특','9','기타','') AS JONG_NM   ");
            parameter.AppendSql("      ,JIKJONG,GONGJENG,ROWID AS RID                                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_LTD_UCODE                                                       ");
            parameter.AppendSql(" WHERE 1 = 1                                                                           ");
            parameter.AppendSql("   AND LTDCODE=:LTDCODE                                                                ");
            if (!argJong.IsNullOrEmpty() && argJong != "*")
            {
                parameter.AppendSql("   AND JONG =:JONG                                                         ");
            }
            
            parameter.AppendSql(" ORDER BY JOBNAME                                                              ");

            parameter.Add("LTDCODE", argLtdCode.To<long>());

            if (!argJong.IsNullOrEmpty() && argJong != "*")
            {
                parameter.Add("JONG", argJong, Oracle.DataAccess.Client.OracleDbType.Char);
            }
                

            return ExecuteReader<HIC_LTD_UCODE>(parameter);
        }


        public List<HIC_LTD_UCODE> GetListByCodeJong1(long argLtdCode, string argJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JOBNAME,UCODES, SCODES, ROWID AS RID                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_LTD_UCODE                                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                                   ");
            parameter.AppendSql("   AND LTDCODE=:LTDCODE                                                        ");
            parameter.AppendSql("   AND JONG=:JONG                                                              ");
            parameter.AppendSql("   AND (UCODES IS NOT NULL OR SCODES IS NOT NULL)                              ");
            parameter.AppendSql("   ORDER BY JOBNAME                                                            ");

            parameter.Add("LTDCODE", argLtdCode);
            parameter.Add("JONG", argJong);

            return ExecuteReader<HIC_LTD_UCODE>(parameter);
        }

        public int UpDateDateByItem(HIC_LTD_UCODE item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_LTD_UCODE   ");
            parameter.AppendSql("   SET JOBNAME  = :JOBNAME         ");
            parameter.AppendSql("      ,JONG     = :JONG            ");
            parameter.AppendSql("      ,UCODES   = :UCODES          ");
            parameter.AppendSql("      ,SCODES   = :SCODES          ");
            parameter.AppendSql("      ,JIKJONG  = :JIKJONG         ");
            parameter.AppendSql("      ,GONGJENG = :GONGJENG        ");
            parameter.AppendSql("      ,JOBSABUN = :JOBSABUN        ");
            parameter.AppendSql("      ,ENTTIME  = SYSDATE          ");
            parameter.AppendSql(" WHERE ROWID = :RID                ");

            parameter.Add("JOBNAME",  item.JOBNAME);
            parameter.Add("JONG",     item.JONG);
            parameter.Add("UCODES",   item.UCODES);
            parameter.Add("SCODES",   item.SCODES);
            parameter.Add("JIKJONG",  item.JIKJONG);
            parameter.Add("GONGJENG", item.GONGJENG);
            parameter.Add("JOBSABUN", item.JOBSABUN);
            parameter.Add("RID",      item.RID);

            return ExecuteNonQuery(parameter);
        }

        public int DeleteByRowid(string fstrROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_LTD_UCODE   ");
            parameter.AppendSql(" WHERE ROWID = :RID                ");
            
            parameter.Add("RID", fstrROWID);
           
            return ExecuteNonQuery(parameter);
        }

        public int InsertData(HIC_LTD_UCODE code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT KOSMOS_PMPA.HIC_LTD_UCODE (                                         ");
            parameter.AppendSql("   LTDCODE,JONG,JOBNAME,UCODES,JIKJONG,GONGJENG,JOBSABUN,ENTTIME )         ");
            parameter.AppendSql(" VALUES (                                                                  ");
            parameter.AppendSql("  :LTDCODE,:JONG,:JOBNAME,:UCODES,:JIKJONG,:GONGJENG,:JOBSABUN,SYSDATE )   ");

            parameter.Add("LTDCODE",    code.LTDCODE);
            parameter.Add("JONG",       code.JONG);
            parameter.Add("JOBNAME",    code.JOBNAME);
            parameter.Add("UCODES",     code.UCODES);
            parameter.Add("JIKJONG",    code.JIKJONG);
            parameter.Add("GONGJENG",   code.GONGJENG);
            parameter.Add("JOBSABUN",   code.JOBSABUN);

            return ExecuteNonQuery(parameter);
        }

        public HIC_LTD_UCODE GetMaxNameByLtdCodeLikeName(long fnLtdCode, string strBuse)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MAX(JOBNAME) AS JOBNAME     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_LTD_UCODE   ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND LTDCODE=:LTDCODE            ");
            parameter.AppendSql("   AND JOBNAME LIKE :JOBNAME       ");

            parameter.Add("LTDCODE", fnLtdCode);
            parameter.AddLikeStatement("JOBNAME", strBuse);

            return ExecuteReaderSingle<HIC_LTD_UCODE>(parameter);
        }

        public HIC_LTD_UCODE GetRowidByLtdCodeLikeName(long fnLtdCode, string strBuse, string strUCodes)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JOBNAME,UCODES              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_LTD_UCODE   ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND LTDCODE=:LTDCODE            ");
            parameter.AppendSql("   AND JOBNAME LIKE :JOBNAME       ");
            parameter.AppendSql("   AND UCODES=:UCODES              ");

            parameter.Add("LTDCODE", fnLtdCode);
            parameter.AddLikeStatement("JOBNAME", strBuse);
            parameter.Add("UCODES", strUCodes);

            return ExecuteReaderSingle<HIC_LTD_UCODE>(parameter);
        }

        public HIC_LTD_UCODE GetItemByLtdCode(long argLtdCode, string strJobName = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT LTDCODE,JONG,JOBNAME,UCODES,JIKJONG,GONGJENG,JOBSABUN,ENTTIME,SCODES ");
            parameter.AppendSql("      ,LTDCODE1, LTDCODE2, ROWID AS RID                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_LTD_UCODE                                            ");
            parameter.AppendSql(" WHERE 1 = 1                                                                ");
            parameter.AppendSql("   AND LTDCODE=:LTDCODE                                                     ");
            if (!strJobName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND JOBNAME=:JOBNAME                                                     ");
            }

            parameter.Add("LTDCODE", argLtdCode);
            if (!strJobName.IsNullOrEmpty())
            {
                parameter.Add("JOBNAME", strJobName);
            }

            return ExecuteReaderSingle<HIC_LTD_UCODE>(parameter);
        }

        public HIC_LTD_UCODE GetItemByRowid(string argROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT LTDCODE,JONG,JOBNAME,UCODES,JIKJONG,GONGJENG,JOBSABUN,ENTTIME,SCODES ");
            parameter.AppendSql("      ,LTDCODE1, LTDCODE2, ROWID AS RID                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_LTD_UCODE                                            ");
            parameter.AppendSql(" WHERE 1 = 1                                                                ");
            parameter.AppendSql("   AND ROWID=:RID                                                           ");
            
            parameter.Add("RID", argROWID);

            return ExecuteReaderSingle<HIC_LTD_UCODE>(parameter);
        }
    }
}
