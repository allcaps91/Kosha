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
    public class HicMcodeRepository : BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicMcodeRepository()
        {
        }

        public IList<HIC_MCODE> SelMCode_Many(string argSql)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,NAME,YNAME,UCODE,JUGI,GBSELECT,ENTSABUN,ENTDATE,TONGBUN,GBDENT ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MCODE                                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND Code IN (" + argSql + ")                                            ");
            parameter.AppendSql(" ORDER BY Code                                                             ");

            return ExecuteReader<HIC_MCODE>(parameter);
        }

        public string Read_UCode(string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT UCODE                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MCODE   ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", argCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_MCODE> GetCodeNamebyNotInCode(List<string> strUSQL)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MCODE   ");
            if (strUSQL.Count == 0)
            {
                parameter.AppendSql(" WHERE CODE NOT IN ('ZZZ') ");
            }
            else
            {
                parameter.AppendSql(" WHERE CODE NOT IN (:CODE) ");
                parameter.AppendSql("   AND CODE NOT IN ('ZZZ') ");
            }

            if (strUSQL.Count > 0)
            {
                parameter.AddInStatement("CODE", strUSQL);
            }

            return ExecuteReader<HIC_MCODE>(parameter);
        }

        public void Delete(HIC_MCODE code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_MCODE                 ");
            parameter.AppendSql(" WHERE ROWID =:RID                            ");
            parameter.Add("RID", code.RID);

            ExecuteNonQuery(parameter);
        }

        public void Update(HIC_MCODE code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MCODE                   ");
            parameter.AppendSql("   SET CODE        =:CODE                      ");
            parameter.AppendSql("       NAME        =:NAME                      ");
            parameter.AppendSql("       YNAME       =:YNAME                     ");
            parameter.AppendSql("       UCODE       =:UCODE                     ");
            parameter.AppendSql("       JUGI        =:JUGI                      ");
            parameter.AppendSql("       GBSELECT    =:GBSELECT                  ");
            parameter.AppendSql("       ENTSABUN    =:ENTSABUN                  ");
            parameter.AppendSql("       GBDENT      =:GBDENT                    ");
            parameter.AppendSql("       ENTDATE     =SYSDATE                    ");
            parameter.AppendSql("       WHERE ROWID =:RID                       ");

            parameter.Add("CODE", code.CODE);
            parameter.Add("NAME", code.NAME);
            parameter.Add("YNAME", code.YNAME);
            parameter.Add("UCODE", code.UCODE);
            parameter.Add("JUGI", code.JUGI);
            parameter.Add("GBSELECT", code.GBSELECT);
            parameter.Add("ENTSABUN", clsType.User.IdNumber);
            parameter.Add("GBDENT", code.GBDENT);
            parameter.Add("RID", code.RID);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_MCODE> GetCodeNamebyNotInNight(List<string> strCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MCODE   ");
            parameter.AppendSql(" WHERE CODE NOT IN (:CODE)     ");
            parameter.AppendSql(" ORDER BY NAME                 ");

            parameter.AddInStatement("CODE", strCode);

            return ExecuteReader<HIC_MCODE>(parameter);
        }

        public List<HIC_MCODE> GetListByCodeName(string strGubun, string strName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MCODE   ");
            if (!strName.IsNullOrEmpty())
            {
                parameter.AppendSql(" WHERE NAME LIKE :NAME     ");
            }
            else
            {
                parameter.AppendSql(" WHERE NAME IS NOT NULL   ");
            }
            parameter.AppendSql("   AND ROWNUM <= 500          ");

            parameter.AppendSql(" ORDER BY CODE                ");

            if (!strName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("NAME", strGubun);
            }

            return ExecuteReader<HIC_MCODE>(parameter);
        }

        public string GetUCodebyCode(string fstrMCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT UCODE                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MCODE   ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");
            parameter.AppendSql(" ORDER BY NAME                 ");

            parameter.Add("CODE", fstrMCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_MCODE> GetCodeListByCodeNotIn(List<string> strTemp1)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MCODE   ");
            if (strTemp1 == null)
            {
                parameter.AppendSql(" WHERE CODE > ' '          ");
            }
            else
            {
                parameter.AppendSql(" WHERE CODE NOT IN (:CODE) ");
            }
            parameter.AppendSql(" ORDER BY NAME                 ");

            parameter.AddInStatement("CODE", strTemp1);

            return ExecuteReader<HIC_MCODE>(parameter);
        }

        public List<HIC_MCODE> GetCodeListByCodeIn(List<string> strTemp1)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MCODE   ");
            parameter.AppendSql(" WHERE CODE IN (:CODE)         ");
            parameter.AppendSql(" ORDER BY NAME                 ");

            parameter.AddInStatement("CODE", strTemp1);

            return ExecuteReader<HIC_MCODE>(parameter);
        }

        public string GetNameByCode(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MCODE                                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND CODE =:CODE                                                         ");

            parameter.Add("CODE", argCode);

            return ExecuteScalar<string>(parameter);
        }
        public int GetTongBunbyCode(string strUCodes)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TONGBUN                             ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_MCODE     ");
            parameter.AppendSql(" WHERE CODE = :CODE                        ");

            parameter.Add("CODE", strUCodes);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_MCODE> GetTongBunCountbyCode(List<string> argSQL)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TONGBUN, COUNT(*) CNT               ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_MCODE     ");
            parameter.AppendSql(" WHERE CODE IN (:CODE)                     ");
            parameter.AppendSql(" GROUP BY TONGBUN                          ");
            parameter.AppendSql(" ORDER BY TONGBUN                          ");

            if (!argSQL.IsNullOrEmpty())
            {
                parameter.AddInStatement("CODE", argSQL);
            }

            return ExecuteReader<HIC_MCODE>(parameter);
        }

        public List<HIC_MCODE> GetItemByLikeName(string argName = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME                            ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_MCODE       ");
            parameter.AppendSql("  WHERE 1 = 1                                ");
            if (!argName.IsNullOrEmpty())
            {
                parameter.AppendSql("  AND NAME LIKE :NAME                                         ");
            }
            parameter.AppendSql(" ORDER BY NAME                                         ");

            if (!argName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("NAME", argName);
            }

            return ExecuteReader<HIC_MCODE>(parameter);
        }

        public int UpDateSpcExamInfoByCode(HIC_MCODE item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MCODE            ");
            parameter.AppendSql("   SET NAME        =:NAME               ");
            parameter.AppendSql("       UCODE       =:UCODE              ");
            parameter.AppendSql("       TONGBUN     =:TONGBUN            ");
            parameter.AppendSql(" WHERE CODE =:RID                       ");

            parameter.Add("NAME", item.NAME);
            parameter.Add("UCODE", item.UCODE);
            parameter.Add("TONGBUN", item.TONGBUN);
            parameter.Add("CODE", item.CODE);

            return ExecuteNonQuery(parameter);
        }

        public HIC_MCODE GetItemByCode(string strCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,NAME,YNAME,UCODE,JUGI,GBSELECT,ENTSABUN,ENTDATE                ");
            parameter.AppendSql("      ,KOSMOS_PMPA.FC_HIC_CODE_NM('09', UCODE) AS UNAME, TONGBUN           ");
            parameter.AppendSql("      ,GBDENT,ROWID AS RID                                                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MCODE                                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND CODE =:CODE                                                         ");
            parameter.AppendSql(" ORDER BY Code                                                             ");

            parameter.Add("CODE", strCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_MCODE>(parameter);
        }

        public void Insert(HIC_MCODE code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT KOSMOS_PMPA.HIC_MCODE (                                                     ");
            parameter.AppendSql("   CODE,NAME,YNAME,UCODE,JUGI,GBSELECT,ENTSABUN,ENTDATE,TONGBUN,GBDENT )           ");
            parameter.AppendSql(" VALUES (                                                                          ");
            parameter.AppendSql("  :CODE,:NAME,:YNAME,:UCODE,:JUGI,:GBSELECT,:ENTSABUN,SYSDATE,:TONGBUN,:GBDENT )   ");

            parameter.Add("CODE", code.CODE);
            parameter.Add("NAME", code.NAME);
            parameter.Add("YNAME", code.YNAME);
            parameter.Add("UCODE", code.UCODE);
            parameter.Add("JUGI", code.JUGI);
            parameter.Add("GBSELECT", code.GBSELECT);
            parameter.Add("ENTSABUN", clsType.User.IdNumber);
            parameter.Add("TONGBUN", code.TONGBUN);
            parameter.Add("GBDENT", code.GBDENT);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_MCODE> GetCodeNamebyCode(List<string> strUSQL)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MCODE   ");
            if (strUSQL.Count > 0)
            {
                parameter.AppendSql(" WHERE CODE IN (:CODE)         ");
            }
            parameter.AppendSql(" ORDER BY CODE                 ");

            if (strUSQL.Count > 0)
            {
                parameter.AddInStatement("CODE", strUSQL);
            }

            return ExecuteReader<HIC_MCODE>(parameter);
        }

        public List<HIC_MCODE> FindAll()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,NAME,YNAME,UCODE,JUGI,GBSELECT,ENTSABUN,ENTDATE                ");
            parameter.AppendSql("      ,KOSMOS_PMPA.FC_HIC_CODE_NM('09', UCODE) AS UNAME, TONGBUN,GBDENT,ROWID AS RID ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MCODE                                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND NAME IS NOT NULL                                                    ");
            parameter.AppendSql(" ORDER BY Code                                                             ");

            return ExecuteReader<HIC_MCODE>(parameter);
        }

        public string Read_MCode_Name(string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MCODE   ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", argCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_JUGI(string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT JUGI                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MCODE   ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", argCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_GbDent(string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBDENT                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MCODE   ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", argCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }
    }
}
