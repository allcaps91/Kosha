namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class BasBcodeEcgRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public BasBcodeEcgRepository()
        {
        }

        public List<BAS_BCODE_ECG> GetCodeAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME                  ");
            parameter.AppendSql("  FROM ADMIN.BAS_BCODE_ECG   ");
            parameter.AppendSql(" ORDER BY SORT,CODE                ");

            return ExecuteReader<BAS_BCODE_ECG>(parameter);
        }

        public int DeletebyRowId(string strROWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE ADMIN.BAS_BCODE_ECG   ");
            parameter.AppendSql(" WHERE ROWID = :RID                ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public List<BAS_BCODE_ECG> GetItemAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME , SORT, ROWID    ");
            parameter.AppendSql("  FROM ADMIN.BAS_BCODE_ECG   ");
            parameter.AppendSql(" ORDER BY CODE                     ");

            return ExecuteReader<BAS_BCODE_ECG>(parameter);
        }

        public int UPdate(string strCODE, string strName, string strROWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.BAS_BCODE_ECG SET   ");
            parameter.AppendSql("       CODE = :CODE                    ");
            parameter.AppendSql("     , NAME = :NAME                    ");
            parameter.AppendSql(" WHERE ROWID = :RID                    ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int InsertAll(string strCODE, string strName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.BAS_BCODE_ECG  ");
            parameter.AppendSql("       ( CODE, NAME )                  ");
            parameter.AppendSql("VALUES                                 ");
            parameter.AppendSql("       ( :CODE, :NAME )                ");

            parameter.Add("CODE", strCODE);
            parameter.Add("NAME", strName);

            return ExecuteNonQuery(parameter);
        }
    }
}
