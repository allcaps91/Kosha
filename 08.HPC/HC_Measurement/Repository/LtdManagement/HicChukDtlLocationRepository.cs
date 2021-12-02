namespace HC_Measurement.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC_Measurement.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HicChukDtlLocationRepository :BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicChukDtlLocationRepository()
        {
        }

        public void InSert(HIC_CHUKDTL_LOCATION dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_CHUKDTL_LOCATION");
            parameter.AppendSql("(");
            parameter.AppendSql("    WRTNO");
            parameter.AppendSql("  , SEQNO");
            parameter.AppendSql("  , FILENAME");
            parameter.AppendSql("  , REMARK");
            parameter.AppendSql("  , IMAGEDATA");
            parameter.AppendSql("  , ENTDATE");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :WRTNO");
            parameter.AppendSql("  , :SEQNO");
            parameter.AppendSql("  , :FILENAME");
            parameter.AppendSql("  , :REMARK");
            parameter.AppendSql("  , :IMAGEDATA");
            parameter.AppendSql("  , SYSDATE");
            parameter.AppendSql(") ");

            parameter.Add("WRTNO", dto.WRTNO);
            parameter.Add("SEQNO", dto.SEQNO);
            parameter.Add("FILENAME", dto.FILENAME);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("IMAGEDATA", dto.IMAGEDATA, Oracle.ManagedDataAccess.Client.OracleDbType.LongRaw);

            ExecuteNonQuery(parameter);
        }

        public void Delete(HIC_CHUKDTL_LOCATION dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_CHUKDTL_LOCATION");
            parameter.AppendSql(" WHERE ROWID = :RID ");

            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }

        public HIC_CHUKDTL_LOCATION FindImageByRowid(string strRid)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.SEQNO");
            parameter.AppendSql("     , A.FILENAME");
            parameter.AppendSql("     , A.REMARK");
            parameter.AppendSql("     , A.IMAGEDATA");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , A.ROWID AS RID");
            parameter.AppendSql("  FROM HIC_CHUKDTL_LOCATION A");
            parameter.AppendSql(" WHERE A.ROWID=:RID");

            parameter.Add("RID", strRid);

            return ExecuteReaderSingle<HIC_CHUKDTL_LOCATION>(parameter);
        }

        public List<HIC_CHUKDTL_LOCATION> GetListByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.SEQNO");
            parameter.AppendSql("     , A.FILENAME");
            parameter.AppendSql("     , A.REMARK");
            parameter.AppendSql("     , A.IMAGEDATA");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , A.ROWID AS RID");
            parameter.AppendSql("  FROM HIC_CHUKDTL_LOCATION A");
            parameter.AppendSql(" WHERE A.WRTNO=:WRTNO");
            parameter.AppendSql(" ORDER By A.SEQNO");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<HIC_CHUKDTL_LOCATION>(parameter);
        }

        public void UpDate(HIC_CHUKDTL_LOCATION dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CHUKDTL_LOCATION");
            parameter.AppendSql("   SET SEQNO = :SEQNO");
            parameter.AppendSql("     , REMARK = :REMARK");
            parameter.AppendSql("     , IMAGEDATA = :IMAGEDATA");
            parameter.AppendSql("     , ENTDATE = SYSDATE");
            parameter.AppendSql(" WHERE ROWID = :RID ");

            parameter.Add("SEQNO", dto.SEQNO);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("IMAGEDATA", dto.IMAGEDATA, Oracle.ManagedDataAccess.Client.OracleDbType.LongRaw);
            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }
    }
}
