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
    public class HicReadingRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicReadingRepository()
        {
        }

        public List<HIC_READING> GetListByYear(string argYear = "", string argCode = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ADMIN.FC_INSA_MST_KORNAME(REG_SABUN) AS REG_SABUN_NM       ");
            parameter.AppendSql("      ,CODE,NAME,REG_DATE,REG_SABUN,DATA1,DEL_DATE,YEAR, ROWID AS RID  ");
            parameter.AppendSql("  FROM ADMIN.HIC_READING                                         ");
            parameter.AppendSql(" WHERE 1 = 1                                                           ");
            parameter.AppendSql("   AND DEL_DATE IS NULL                                                ");

            if (!argYear.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND YEAR = :YEAR                                                ");
            }
            
            if (!argCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND CODE = :CODE                                                ");
            }
            parameter.AppendSql(" ORDER BY NAME                                                         ");

            if (!argYear.IsNullOrEmpty())
            {
                parameter.Add("YEAR", argYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (!argCode.IsNullOrEmpty())
            {
                parameter.Add("CODE", argCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_READING>(parameter);
        }

        public int UpDate(HIC_READING item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_READING     ");
            parameter.AppendSql("   SET REG_DATE    =:REG_DATE      ");
            parameter.AppendSql("      ,REG_SABUN   =:REG_SABUN     ");
            parameter.AppendSql("      ,DATA1       =:DATA1         ");
            parameter.AppendSql(" WHERE ROWID       =:RID           ");

            parameter.Add("REG_DATE", item.REG_DATE);
            parameter.Add("REG_SABUN", item.REG_SABUN);
            parameter.Add("DATA1", item.DATA1);
            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_READING item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_READING (             ");
            parameter.AppendSql("       CODE,NAME,REG_DATE,REG_SABUN,DATA1,YEAR    ");
            parameter.AppendSql(" ) VALUES (                                       ");
            parameter.AppendSql(" :CODE,:NAME,:REG_DATE,:REG_SABUN,:DATA1,:YEAR  ) ");

            parameter.Add("CODE", item.CODE);
            parameter.Add("NAME", item.NAME);
            parameter.Add("REG_DATE", item.REG_DATE);
            parameter.Add("REG_SABUN", item.REG_SABUN);
            parameter.Add("DATA1", item.DATA1);
            parameter.Add("YEAR", item.YEAR);

            return ExecuteNonQuery(parameter);
        }

        public string GetItemByRowid(string strROWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DATA1 FROM ADMIN.HIC_READING  ");
            parameter.AppendSql(" WHERE 1 = 1                               ");
            parameter.AppendSql("   AND ROWID = :RID                        ");

            return ExecuteScalar<string>(parameter);
        }

        public int Delete(string fstrRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_READING     ");
            parameter.AppendSql("   SET DEL_DATE       = SYSDATE    ");
            parameter.AppendSql(" WHERE ROWID          =:RID        ");

            parameter.Add("RID", fstrRowid);
           
            return ExecuteNonQuery(parameter);
        }
    }
}
