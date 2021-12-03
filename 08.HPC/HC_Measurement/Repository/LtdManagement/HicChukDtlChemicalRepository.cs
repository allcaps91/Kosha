namespace HC_Measurement.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC_Measurement.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HicChukDtlChemicalRepository :BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicChukDtlChemicalRepository()
        {
        }

        public void InSert(HIC_CHUKDTL_CHEMICAL dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.HIC_CHUKDTL_CHEMICAL");
            parameter.AppendSql("(");
            parameter.AppendSql("    WRTNO");
            parameter.AppendSql("  , PROCESS");
            parameter.AppendSql("  , PROCESS_K2BCD");
            parameter.AppendSql("  , PRODUCT_NM");
            parameter.AppendSql("  , GBUSE");
            parameter.AppendSql("  , USAGE");
            parameter.AppendSql("  , TREATMENT");
            parameter.AppendSql("  , UNIT");
            parameter.AppendSql("  , REMARK");
            parameter.AppendSql("  , SEQNO");
            parameter.AppendSql("  , JOBSABUN");
            parameter.AppendSql("  , ENTDATE");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :WRTNO");
            parameter.AppendSql("  , :PROCESS");
            parameter.AppendSql("  , :PROCESS_K2BCD");
            parameter.AppendSql("  , :PRODUCT_NM");
            parameter.AppendSql("  , :GBUSE");
            parameter.AppendSql("  , :USAGE");
            parameter.AppendSql("  , :TREATMENT");
            parameter.AppendSql("  , :UNIT");
            parameter.AppendSql("  , :REMARK");
            parameter.AppendSql("  , :SEQNO");
            parameter.AppendSql("  , :JOBSABUN");
            parameter.AppendSql("  , SYSDATE");
            parameter.AppendSql(") ");

            parameter.Add("WRTNO", dto.WRTNO);
            parameter.Add("PROCESS", dto.PROCESS);
            parameter.Add("PROCESS_K2BCD", dto.PROCESS_K2BCD);
            parameter.Add("PRODUCT_NM", dto.PRODUCT_NM);
            parameter.Add("GBUSE", dto.GBUSE);
            parameter.Add("USAGE", dto.USAGE);
            parameter.Add("TREATMENT", dto.TREATMENT);
            parameter.Add("UNIT", dto.UNIT);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("SEQNO", dto.SEQNO);
            parameter.Add("JOBSABUN", dto.JOBSABUN);

            ExecuteNonQuery(parameter);
        }

        public void DeleteAll(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HIC_CHUKDTL_CHEMICAL");
            parameter.AppendSql("   SET DELDATE = SYSDATE ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO ");

            parameter.Add("WRTNO", nWRTNO);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_CHUKDTL_CHEMICAL> GetListByWrtno(long argWRTNO, bool bDel)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.PROCESS");
            parameter.AppendSql("     , ADMIN.FC_HIC_CODE_NM('C4', A.PROCESS) AS PROCESS_NM");
            parameter.AppendSql("     , A.PROCESS_K2BCD");
            parameter.AppendSql("     , A.PRODUCT_NM");
            parameter.AppendSql("     , A.GBUSE");
            parameter.AppendSql("     , A.USAGE");
            parameter.AppendSql("     , A.TREATMENT");
            parameter.AppendSql("     , A.UNIT");
            parameter.AppendSql("     , A.REMARK");
            parameter.AppendSql("     , A.SEQNO");
            parameter.AppendSql("     , A.JOBSABUN");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , DECODE(A.DELDATE, '', 'N', 'Y') AS IsDelete");
            parameter.AppendSql("     , A.ROWID AS RID");
            parameter.AppendSql("  FROM HIC_CHUKDTL_CHEMICAL A");
            parameter.AppendSql(" WHERE A.WRTNO=:WRTNO");
            if (!bDel)
            {
                parameter.AppendSql("   AND A.DELDATE IS NULL");
            }
            
            parameter.AppendSql(" ORDER BY A.SEQNO");


            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<HIC_CHUKDTL_CHEMICAL>(parameter);
        }

        public void Delete(HIC_CHUKDTL_CHEMICAL dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HIC_CHUKDTL_CHEMICAL");
            parameter.AppendSql("   SET DELDATE = SYSDATE ");
            parameter.AppendSql(" WHERE ROWID = :RID ");

            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }

        public void UpDate(HIC_CHUKDTL_CHEMICAL dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_CHUKDTL_CHEMICAL");
            parameter.AppendSql("   SET PROCESS = :PROCESS");
            parameter.AppendSql("     , PROCESS_K2BCD = :PROCESS_K2BCD"); 
            parameter.AppendSql("     , PRODUCT_NM = :PRODUCT_NM");
            parameter.AppendSql("     , GBUSE = :GBUSE");
            parameter.AppendSql("     , USAGE = :USAGE");
            parameter.AppendSql("     , TREATMENT = :TREATMENT");
            parameter.AppendSql("     , UNIT = :UNIT");
            parameter.AppendSql("     , REMARK = :REMARK");
            parameter.AppendSql("     , SEQNO = :SEQNO");
            parameter.AppendSql("     , JOBSABUN = :JOBSABUN");
            parameter.AppendSql("     , ENTDATE = SYSDATE");
            parameter.AppendSql(" WHERE ROWID = :RID ");

            parameter.Add("PROCESS", dto.PROCESS);
            parameter.Add("PROCESS_K2BCD", dto.PROCESS_K2BCD);
            parameter.Add("PRODUCT_NM", dto.PRODUCT_NM);
            parameter.Add("GBUSE", dto.GBUSE);
            parameter.Add("USAGE", dto.USAGE);
            parameter.Add("TREATMENT", dto.TREATMENT);
            parameter.Add("UNIT", dto.UNIT);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("SEQNO", dto.SEQNO);
            parameter.Add("JOBSABUN", dto.JOBSABUN);
            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }

    }
}
