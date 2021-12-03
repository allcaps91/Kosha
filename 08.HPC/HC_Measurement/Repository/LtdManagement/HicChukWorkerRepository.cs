namespace HC_Measurement.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC_Measurement.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HicChukWorkerRepository :BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicChukWorkerRepository()
        {
        }

        public List<HIC_CHUK_WORKER> GetListByWrtno(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.WORKER_SABUN");
            parameter.AppendSql("     , ADMIN.FC_INSA_MST_KORNAME(A.WORKER_SABUN) WORKER_NAME");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , A.ENTSABUN");
            parameter.AppendSql("     , B.ROLE");
            parameter.AppendSql("     , A.BIGO");
            parameter.AppendSql("     , A.ROWID AS RID");
            parameter.AppendSql("  FROM ADMIN.HIC_CHUK_WORKER A ");
            parameter.AppendSql("     , ADMIN.HIC_USERS B ");
            parameter.AppendSql(" WHERE A.WRTNO = :WRTNO ");
            parameter.AppendSql("   AND A.WORKER_SABUN = B.USERID ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_CHUK_WORKER>(parameter);
        }

        public void Insert(HIC_CHUK_WORKER dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.HIC_CHUK_WORKER");
            parameter.AppendSql("(");
            parameter.AppendSql("    WRTNO");
            parameter.AppendSql("  , WORKER_SABUN");
            parameter.AppendSql("  , ENTDATE");
            parameter.AppendSql("  , ENTSABUN");
            parameter.AppendSql("  , BIGO");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :WRTNO");
            parameter.AppendSql("  , :WORKER_SABUN");
            parameter.AppendSql("  , SYSDATE");
            parameter.AppendSql("  , :ENTSABUN");
            parameter.AppendSql("  , :BIGO");
            parameter.AppendSql(") ");

            parameter.Add("WRTNO", dto.WRTNO);
            parameter.Add("WORKER_SABUN", dto.WORKER_SABUN);
            parameter.Add("ENTSABUN", dto.ENTSABUN);
            parameter.Add("BIGO", dto.BIGO);

            ExecuteNonQuery(parameter);
        }

        public void DeleteAll(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE ADMIN.HIC_CHUK_WORKER");
            parameter.AppendSql(" WHERE WRTNO =:WRTNO ");

            parameter.Add("WRTNO", nWRTNO);

            ExecuteNonQuery(parameter);
        }

        public void Delete(HIC_CHUK_WORKER code)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE ADMIN.HIC_CHUK_WORKER");
            parameter.AppendSql(" WHERE ROWID =:RID ");
            
            parameter.Add("RID", code.RID);

            ExecuteNonQuery(parameter);
        }

        public void UpdatebyRowId(HIC_CHUK_WORKER dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_CHUK_WORKER");
            parameter.AppendSql("   SET WRTNO = :WRTNO");
            parameter.AppendSql("     , WORKER_SABUN = :WORKER_SABUN");
            parameter.AppendSql("     , ENTDATE = SYSDATE");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN");
            parameter.AppendSql("     , BIGO = :BIGO");

            parameter.Add("WRTNO", dto.WRTNO);
            parameter.Add("WORKER_SABUN", dto.WORKER_SABUN);
            parameter.Add("ENTSABUN", dto.ENTSABUN);
            parameter.Add("BIGO", dto.BIGO);

            ExecuteNonQuery(parameter);
        }
    }
}
