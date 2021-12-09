namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
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

            parameter.AppendSql("SELECT JOBNAME, CASE WHEN NVL(SCODES, '') != '' THEN UCODES || '{★}' || SCODES ");
            parameter.AppendSql("                     ELSE UCODES END AS VUCODE  ");
            parameter.AppendSql("      ,JONG, DECODE(JONG, '1','배치','2','특수','3','일특','9','기타','') AS JONG_NM ");
            parameter.AppendSql("      ,JIKJONG,GONGJENG,ROWID AS RID  ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD_UCODE ");
            parameter.AppendSql(" WHERE 1 = 1  ");
            parameter.AppendSql("   AND LTDCODE=:LTDCODE ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            if (!argJong.IsNullOrEmpty() && argJong != "*")
            {
                parameter.AppendSql(" AND JONG =:JONG ");
            }
            
            parameter.AppendSql(" ORDER BY JOBNAME ");

            parameter.Add("LTDCODE", argLtdCode.To<long>());
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            if (!argJong.IsNullOrEmpty() && argJong != "*")
            {
                parameter.Add("JONG", argJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
                

            return ExecuteReader<HIC_LTD_UCODE>(parameter);
        }


        public List<HIC_LTD_UCODE> GetListByCodeJong1(long argLtdCode, string argJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JOBNAME,UCODES, SCODES, ROWID AS RID ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD_UCODE ");
            parameter.AppendSql(" WHERE 1 = 1 ");
            parameter.AppendSql("   AND LTDCODE=:LTDCODE ");
            parameter.AppendSql("   AND JONG=:JONG ");
            parameter.AppendSql("   AND (UCODES IS NOT NULL OR SCODES IS NOT NULL) ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   ORDER BY JOBNAME ");

            parameter.Add("LTDCODE", argLtdCode);
            parameter.Add("JONG", argJong);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HIC_LTD_UCODE>(parameter);
        }

        public int UpDateDateByItem(HIC_LTD_UCODE item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_LTD_UCODE ");
            parameter.AppendSql("   SET JOBNAME  = :JOBNAME ");
            parameter.AppendSql("      ,JONG     = :JONG ");
            parameter.AppendSql("      ,UCODES   = :UCODES ");
            parameter.AppendSql("      ,SCODES   = :SCODES ");
            parameter.AppendSql("      ,JIKJONG  = :JIKJONG ");
            parameter.AppendSql("      ,GONGJENG = :GONGJENG ");
            parameter.AppendSql("      ,JOBSABUN = :JOBSABUN ");
            parameter.AppendSql("      ,ENTTIME  = SYSDATE ");
            parameter.AppendSql(" WHERE ROWID = :RID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("JOBNAME",  item.JOBNAME);
            parameter.Add("JONG",     item.JONG);
            parameter.Add("UCODES",   item.UCODES);
            parameter.Add("SCODES",   item.SCODES);
            parameter.Add("JIKJONG",  item.JIKJONG);
            parameter.Add("GONGJENG", item.GONGJENG);
            parameter.Add("JOBSABUN", item.JOBSABUN);
            parameter.Add("RID",      item.RID);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteNonQuery(parameter);
        }

        public int DeleteByRowid(string fstrROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HIC_LTD_UCODE ");
            parameter.AppendSql(" WHERE ROWID = :RID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("RID", fstrROWID);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteNonQuery(parameter);
        }

        public int InsertData(HIC_LTD_UCODE code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT ADMIN.HIC_LTD_UCODE (LTDCODE,JONG,JOBNAME,UCODES,JIKJONG,");
            parameter.AppendSql("       GONGJENG,JOBSABUN,ENTTIME,SWLICENSE ) ");
            parameter.AppendSql(" VALUES (:LTDCODE,:JONG,:JOBNAME,:UCODES,:JIKJONG,");
            parameter.AppendSql("         :GONGJENG,:JOBSABUN,SYSDATE,:SWLICENSE ) ");

            parameter.Add("LTDCODE",    code.LTDCODE);
            parameter.Add("JONG",       code.JONG);
            parameter.Add("JOBNAME",    code.JOBNAME);
            parameter.Add("UCODES",     code.UCODES);
            parameter.Add("JIKJONG",    code.JIKJONG);
            parameter.Add("GONGJENG",   code.GONGJENG);
            parameter.Add("JOBSABUN",   code.JOBSABUN);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteNonQuery(parameter);
        }

        public HIC_LTD_UCODE GetMaxNameByLtdCodeLikeName(long fnLtdCode, string strBuse)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MAX(JOBNAME) AS JOBNAME ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD_UCODE ");
            parameter.AppendSql(" WHERE 1 = 1 ");
            parameter.AppendSql("   AND LTDCODE=:LTDCODE ");
            parameter.AppendSql("   AND JOBNAME LIKE :JOBNAME ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("LTDCODE", fnLtdCode);
            parameter.AddLikeStatement("JOBNAME", strBuse);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HIC_LTD_UCODE>(parameter);
        }

        public HIC_LTD_UCODE GetRowidByLtdCodeLikeName(long fnLtdCode, string strBuse, string strUCodes)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JOBNAME,UCODES ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD_UCODE ");
            parameter.AppendSql(" WHERE 1 = 1 ");
            parameter.AppendSql("   AND LTDCODE=:LTDCODE ");
            parameter.AppendSql("   AND JOBNAME LIKE :JOBNAME ");
            parameter.AppendSql("   AND UCODES=:UCODES ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("LTDCODE", fnLtdCode);
            parameter.AddLikeStatement("JOBNAME", strBuse);
            parameter.Add("UCODES", strUCodes);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HIC_LTD_UCODE>(parameter);
        }

        public HIC_LTD_UCODE GetItemByLtdCode(long argLtdCode, string strJobName = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT LTDCODE,JONG,JOBNAME,UCODES,JIKJONG,GONGJENG,JOBSABUN, ");
            parameter.AppendSql("       ENTTIME,SCODES,LTDCODE1, LTDCODE2, ROWID AS RID ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD_UCODE ");
            parameter.AppendSql(" WHERE 1 = 1 ");
            parameter.AppendSql("   AND LTDCODE=:LTDCODE ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            if (!strJobName.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND JOBNAME=:JOBNAME ");
            }

            parameter.Add("LTDCODE", argLtdCode);
            if (!strJobName.IsNullOrEmpty())
            {
                parameter.Add("JOBNAME", strJobName);
            }
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HIC_LTD_UCODE>(parameter);
        }

        public HIC_LTD_UCODE GetItemByRowid(string argROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT LTDCODE,JONG,JOBNAME,UCODES,JIKJONG,GONGJENG,JOBSABUN, ");
            parameter.AppendSql("       ENTTIME,SCODES,LTDCODE1, LTDCODE2, ROWID AS RID ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD_UCODE ");
            parameter.AppendSql(" WHERE 1 = 1 ");
            parameter.AppendSql("   AND ROWID=:RID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("RID", argROWID);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HIC_LTD_UCODE>(parameter);
        }
    }
}
