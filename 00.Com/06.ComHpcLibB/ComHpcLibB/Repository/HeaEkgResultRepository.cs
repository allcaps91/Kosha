namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaEkgResultRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaEkgResultRepository()
        {
        }

        public string GetResultbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EKGRESULT                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EKG_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", fnWRTNO);
            
            return ExecuteScalar<string>(parameter);
        }

        public string GetRowIdbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EKG_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateResult(long nWrtNo, string strResult)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_EKG_RESULT ");
            parameter.AppendSql("       (WRTNO , EKGRESULT , ENTDATE)   ");
            parameter.AppendSql(" VALUES                                ");
            parameter.AppendSql("       (:WRTNO , :EKGRESULT , SYSDATE) ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("EKGRESULT", strResult);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateResult(string strResult, string strRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_EKG_RESULT SET  ");
            parameter.AppendSql("       EKGRESULT = :EKGRESULT          ");
            parameter.AppendSql("     , ENTDATE   = SYSDATE             ");
            parameter.AppendSql(" WHERE ROWID     = :RID                ");

            parameter.Add("EKGRESULT", strResult);
            parameter.Add("RID", strRowId);

            return ExecuteNonQuery(parameter);
        }

        public int DeletebyRowId(string strRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HEA_EKG_RESULT  ");
            parameter.AppendSql(" WHERE ROWID = :RID                ");

            parameter.Add("RID", strRowId);

            return ExecuteNonQuery(parameter);
        }
    }
}
