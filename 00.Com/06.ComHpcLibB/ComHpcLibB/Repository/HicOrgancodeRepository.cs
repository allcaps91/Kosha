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
    public class HicOrgancodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicOrgancodeRepository()
        {
        }

        public List<HIC_ORGANCODE> GetListAll(string argGubun, string argCode = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,NAME,ROWID             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_ORGANCODE   ");
            parameter.AppendSql(" WHERE GUBUN =:GUBUN               ");
            if (!argCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND CODE =:CODE               ");
            }
            parameter.AppendSql(" ORDER BY CODE                     ");

            parameter.Add("GUBUN", argGubun);

            if (!argCode.IsNullOrEmpty())
            {
                parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_ORGANCODE>(parameter);
        }

        public string GetRowidByCode(string strCODE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID AS RID                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_ORGANCODE  ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", strCODE, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_ORGANCODE> GetSayuCodeNamebyGubunCode(string strGubun, string strCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SAYUCODE, SAYUNAME          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_ORGANCODE   ");
            parameter.AppendSql(" WHERE GUBUN =:GUBUN               ");
            if (!strCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND CODE =:CODE             ");
            }
            parameter.AppendSql(" ORDER BY CODE                     ");

            parameter.Add("GUBUN", strGubun);            
            if (!strCode.IsNullOrEmpty())
            {
                parameter.Add("CODE", strCode, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_ORGANCODE>(parameter);
        }

        public string GetRowidBySayuCode(string strSayuCode, string strCODE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID AS RID                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_ORGANCODE   ");
            parameter.AppendSql(" WHERE SAYUCODE = :SAYUCODE        ");
            parameter.AppendSql("   AND CODE = :CODE                ");

            parameter.Add("SAYUCODE", strSayuCode, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", strCODE, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int Data_InSert(HIC_ORGANCODE item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_ORGANCODE (CODE,NAME,GUBUN)  ");
            parameter.AppendSql(" VALUES (:CODE,:NAME,:GUBUN)                             ");

            parameter.Add("CODE", item.CODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("NAME", item.NAME);
            parameter.Add("GUBUN", item.GUBUN, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int Data_UpDate(HIC_ORGANCODE item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_OCS.HIC_ORGANCODE SET    ");
            parameter.AppendSql("       CODE    = :CODE                 ");
            parameter.AppendSql("     , NAME    = :NAME                 ");
            parameter.AppendSql(" WHERE ROWID   = :RID                  ");

            parameter.Add("CODE", item.CODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("NAME", item.NAME);
            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        public int DeleteByRowid(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_ORGANCODE   ");
            parameter.AppendSql(" WHERE ROWID = :RID                ");

            #region Query 변수대입
            parameter.Add("RID", strROWID);
            #endregion
            return ExecuteNonQuery(parameter);
        }
    }
}
