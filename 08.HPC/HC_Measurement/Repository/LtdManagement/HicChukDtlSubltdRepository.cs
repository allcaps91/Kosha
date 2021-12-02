namespace HC_Measurement.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC_Measurement.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HicChukDtlSubltdRepository :BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicChukDtlSubltdRepository()
        {
        }

        public void InSert(HIC_CHUKDTL_SUBLTD dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_CHUKDTL_SUBLTD");
            parameter.AppendSql("(");
            parameter.AppendSql("    WRTNO");
            parameter.AppendSql("  , LTDCODE");
            parameter.AppendSql("  , SUB_LTDCODE");
            parameter.AppendSql("  , JOBSABUN");
            parameter.AppendSql("  , ENTDATE");
            parameter.AppendSql("  , REMARK");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :WRTNO");
            parameter.AppendSql("  , :LTDCODE");
            parameter.AppendSql("  , :SUB_LTDCODE");
            parameter.AppendSql("  , :JOBSABUN");
            parameter.AppendSql("  , SYSDATE");
            parameter.AppendSql("  , :REMARK");
            parameter.AppendSql(") ");

            parameter.Add("WRTNO", dto.WRTNO);
            parameter.Add("LTDCODE", dto.LTDCODE);
            parameter.Add("SUB_LTDCODE", dto.SUB_LTDCODE);
            parameter.Add("JOBSABUN", dto.JOBSABUN);
            parameter.Add("REMARK", dto.REMARK);

            ExecuteNonQuery(parameter);
        }

        public void Delete(HIC_CHUKDTL_SUBLTD dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CHUKDTL_SUBLTD");
            parameter.AppendSql("   SET DELDATE = SYSDATE ");
            parameter.AppendSql(" WHERE ROWID = :RID ");

            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }

        public void DeleteAll(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CHUKDTL_SUBLTD");
            parameter.AppendSql("   SET DELDATE = SYSDATE ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO ");

            parameter.Add("WRTNO", nWRTNO);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_CHUKDTL_SUBLTD> GetListByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.LTDCODE");
            parameter.AppendSql("     , A.SUB_LTDCODE");
            parameter.AppendSql("     , A.JOBSABUN");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , A.DELDATE");
            parameter.AppendSql("     , A.REMARK");
            parameter.AppendSql("     , A.ROWID AS RID");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(A.SUB_LTDCODE) SUB_LTDNAME");
            parameter.AppendSql("  FROM HIC_CHUKDTL_SUBLTD A");
            parameter.AppendSql(" WHERE A.WRTNO=:WRTNO");
            parameter.AppendSql("   AND A.DELDATE IS NULL");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<HIC_CHUKDTL_SUBLTD>(parameter);
        }

        public void UpDate(HIC_CHUKDTL_SUBLTD dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CHUKDTL_SUBLTD");
            parameter.AppendSql("   SET SUB_LTDCODE = :SUB_LTDCODE");
            parameter.AppendSql("     , JOBSABUN = :JOBSABUN");
            parameter.AppendSql("     , ENTDATE = SYSDATE");
            parameter.AppendSql("     , REMARK = :REMARK");
            parameter.AppendSql(" WHERE ROWID = :RID ");

            parameter.Add("SUB_LTDCODE", dto.SUB_LTDCODE);
            parameter.Add("JOBSABUN", dto.JOBSABUN);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }
    }
}
