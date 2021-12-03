namespace HC_Measurement.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC_Measurement.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HicChukDtlDstrbRepository :BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicChukDtlDstrbRepository()
        {
        }

        public void InSert(HIC_CHUKDTL_DSTRB dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_CHUKDTL_DSTRB");
            parameter.AppendSql("(");
            parameter.AppendSql("    WRTNO");
            parameter.AppendSql("  , REMARK");
            parameter.AppendSql("  , ENTDATE");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :WRTNO");
            parameter.AppendSql("  , :REMARK");
            parameter.AppendSql("  , :ENTDATE");
            parameter.AppendSql(") ");

            parameter.Add("WRTNO", dto.WRTNO);
            parameter.Add("REMARK", dto.REMARK, Oracle.ManagedDataAccess.Client.OracleDbType.LongRaw);
            parameter.Add("ENTDATE", dto.ENTDATE);

            ExecuteNonQuery(parameter);
        }

        public void Delete(HIC_CHUKDTL_DSTRB dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE ADMIN.HIC_CHUKDTL_DSTRB");
            parameter.AppendSql(" WHERE ROWID = :RID ");

            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }

        public HIC_CHUKDTL_DSTRB FindImageByRowid(string strRid)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.REMARK");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , A.ROWID AS RID");
            parameter.AppendSql("  FROM HIC_CHUKDTL_DSTRB A");
            parameter.AppendSql(" WHERE A.ROWID=:RID");

            parameter.Add("RID", strRid);

            return ExecuteReaderSingle<HIC_CHUKDTL_DSTRB>(parameter);
        }

        public List<HIC_CHUKDTL_DSTRB> GetListByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.REMARK");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , A.ROWID AS RID");
            parameter.AppendSql("  FROM HIC_CHUKDTL_DSTRB A");
            parameter.AppendSql(" WHERE A.WRTNO=:WRTNO");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<HIC_CHUKDTL_DSTRB>(parameter);
        }

        public HIC_CHUKDTL_DSTRB GetItemByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.REMARK");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , A.ROWID AS RID");
            parameter.AppendSql("  FROM HIC_CHUKDTL_DSTRB A");
            parameter.AppendSql(" WHERE A.WRTNO=:WRTNO");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_CHUKDTL_DSTRB>(parameter);
        }

        public void UpDate(HIC_CHUKDTL_DSTRB dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_CHUKDTL_DSTRB");
            parameter.AppendSql("   SET REMARK = :REMARK");
            parameter.AppendSql("     , ENTDATE = SYSDATE");
            parameter.AppendSql(" WHERE ROWID = :RID ");

            parameter.Add("REMARK", dto.REMARK, Oracle.ManagedDataAccess.Client.OracleDbType.LongRaw);
            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }
    }
}
