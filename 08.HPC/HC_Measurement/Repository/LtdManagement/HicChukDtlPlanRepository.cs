namespace HC_Measurement.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC_Measurement.Dto;
    using HC_Measurement.Model;

    /// <summary>
    /// 
    /// </summary>
    public class HicChukDtlPlanRepository :BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicChukDtlPlanRepository()
        {
        }

        public void InSert(HIC_CHUKDTL_PLAN dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.HIC_CHUKDTL_PLAN");
            parameter.AppendSql("(");
            parameter.AppendSql("    WRTNO");
            parameter.AppendSql("  , SEQNO");
            parameter.AppendSql("  , PROCESS");
            parameter.AppendSql("  , PROCESS_NM");
            parameter.AppendSql("  , MCODE");
            parameter.AppendSql("  , MCODE_NM");
            parameter.AppendSql("  , JUGI");
            parameter.AppendSql("  , INWON");
            parameter.AppendSql("  , JTIME");
            parameter.AppendSql("  , PTIME");
            parameter.AppendSql("  , CHKWAY");
            parameter.AppendSql("  , CHKWAY_CD");
            parameter.AppendSql("  , CHKWAY_NM");
            parameter.AppendSql("  , ANALWAY_NM");
            parameter.AppendSql("  , CHKCOUNT");
            parameter.AppendSql("  , ENTDATE");
            parameter.AppendSql("  , ENTSABUN");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :WRTNO");
            parameter.AppendSql("  , :SEQNO");
            parameter.AppendSql("  , :PROCESS");
            parameter.AppendSql("  , :PROCESS_NM");
            parameter.AppendSql("  , :MCODE");
            parameter.AppendSql("  , :MCODE_NM");
            parameter.AppendSql("  , :JUGI");
            parameter.AppendSql("  , :INWON");
            parameter.AppendSql("  , :JTIME");
            parameter.AppendSql("  , :PTIME");
            parameter.AppendSql("  , :CHKWAY");
            parameter.AppendSql("  , :CHKWAY_CD");
            parameter.AppendSql("  , :CHKWAY_NM");
            parameter.AppendSql("  , :ANALWAY_NM");
            parameter.AppendSql("  , :CHKCOUNT");
            parameter.AppendSql("  , SYSDATE");
            parameter.AppendSql("  , :ENTSABUN");
            parameter.AppendSql(") ");

            parameter.Add("WRTNO", dto.WRTNO);
            parameter.Add("SEQNO", dto.SEQNO);
            parameter.Add("PROCESS", dto.PROCESS);
            parameter.Add("PROCESS_NM", dto.PROCESS_NM);
            parameter.Add("MCODE", dto.MCODE);
            parameter.Add("MCODE_NM", dto.MCODE_NM);
            parameter.Add("JUGI", dto.JUGI);
            parameter.Add("INWON", dto.INWON);
            parameter.Add("JTIME", dto.JTIME);
            parameter.Add("PTIME", dto.PTIME);
            parameter.Add("CHKWAY", dto.CHKWAY);
            parameter.Add("CHKWAY_CD", dto.CHKWAY_CD);
            parameter.Add("CHKWAY_NM", dto.CHKWAY_NM);
            parameter.Add("ANALWAY_NM", dto.ANALWAY_NM);
            parameter.Add("CHKCOUNT", dto.CHKCOUNT);
            parameter.Add("ENTSABUN", dto.ENTSABUN);

            ExecuteNonQuery(parameter);
        }

        public void DeleteAll(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HIC_CHUKDTL_PLAN");
            parameter.AppendSql("   SET DELDATE = SYSDATE ");
            parameter.AppendSql(" WHERE WRTNO =:WRTNO ");

            parameter.Add("WRTNO", nWRTNO);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_CHUKDTL_PLAN_SUGA> GetAccountByPlan(long nWRTNO, long nYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.MCODE");
            parameter.AppendSql("     , ADMIN.FC_HICCHUK_MCODENM(A.MCODE) AS MCODE_NM");
            parameter.AppendSql("     , B.AMT");
            parameter.AppendSql("     , B.GAMT");
            parameter.AppendSql("     , B.SUCODE");
            parameter.AppendSql("     , B.SUNAME");
            parameter.AppendSql("     , A.CHKWAY_CD");
            parameter.AppendSql("     , ADMIN.FC_HIC_CODE_GCODE('15', A.CHKWAY_CD) AS CHKWAY_NM");
            parameter.AppendSql("     , ADMIN.FC_HIC_CODE_GCODE1('15', A.CHKWAY_CD) AS ANALWAY_NM");
            parameter.AppendSql("     , SUM(A.CHKCOUNT) AS CHKCOUNT");
            parameter.AppendSql("  FROM ADMIN.HIC_CHUKDTL_PLAN A");
            parameter.AppendSql("     , ADMIN.HIC_CHK_SUGA B");
            parameter.AppendSql(" WHERE A.WRTNO=:WRTNO");
            parameter.AppendSql("   AND A.DELDATE IS NULL");
            parameter.AppendSql("   AND B.GJYEAR = :GJYEAR");
            parameter.AppendSql("   AND A.CHKWAY_CD = B.CHKCODE(+)");
            parameter.AppendSql("   AND B.GBUSE = 'Y'");
            parameter.AppendSql("   AND A.CHKWAY_CD IS NOT NULL ");
            parameter.AppendSql("   AND A.CHKWAY_CD > '000' "); 
            parameter.AppendSql("   AND A.CHKWAY_CD < '500' ");     //固没备 备盒 力寇
            parameter.AppendSql("   AND A.CHKCOUNT > 0 ");
            parameter.AppendSql(" GROUP BY A.MCODE, B.AMT, B.GAMT, B.SUCODE,B.SUNAME, A.CHKWAY_CD ");
            parameter.AppendSql("        , ADMIN.FC_HIC_CODE_GCODE('15', A.CHKWAY_CD) ");
            parameter.AppendSql("        , ADMIN.FC_HIC_CODE_GCODE1('15', A.CHKWAY_CD) ");
            parameter.AppendSql(" ORDER BY B.SUCODE ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("GJYEAR", nYear);

            return ExecuteReader<HIC_CHUKDTL_PLAN_SUGA>(parameter);
        }

        public List<HIC_CHUKDTL_PLAN> GetListByWrtno(long argWRTNO, bool bDel)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.PROCESS");
            parameter.AppendSql("     , A.PROCESS_NM");
            parameter.AppendSql("     , A.MCODE");
            parameter.AppendSql("     , A.MCODE_NM");
            parameter.AppendSql("     , A.JUGI");
            parameter.AppendSql("     , A.INWON");
            parameter.AppendSql("     , A.JTIME");
            parameter.AppendSql("     , A.PTIME");
            parameter.AppendSql("     , A.CHKWAY");
            parameter.AppendSql("     , A.CHKCOUNT");
            parameter.AppendSql("     , DECODE(A.DELDATE, '', A.SEQNO, 0) AS SEQNO");
            parameter.AppendSql("     , A.ENTSABUN");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , A.DELDATE");
            parameter.AppendSql("     , A.CHKWAY_CD");
            parameter.AppendSql("     , DECODE(A.DELDATE, '', 'N', 'Y') AS IsDelete");
            parameter.AppendSql("     , ADMIN.FC_HIC_CODE_GCODE('15', A.CHKWAY_CD) AS CHKWAY_NM");
            parameter.AppendSql("     , ADMIN.FC_HIC_CODE_GCODE1('15', A.CHKWAY_CD) AS ANALWAY_NM");
            parameter.AppendSql("     , A.ROWID AS RID");
            parameter.AppendSql("  FROM HIC_CHUKDTL_PLAN A");
            parameter.AppendSql(" WHERE A.WRTNO=:WRTNO");

            if (!bDel)
            {
                parameter.AppendSql("   AND A.DELDATE IS NULL");
            }

            parameter.AppendSql(" ORDER BY A.SEQNO ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<HIC_CHUKDTL_PLAN>(parameter);
        }

        public void Delete(HIC_CHUKDTL_PLAN dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HIC_CHUKDTL_PLAN");
            parameter.AppendSql("   SET DELDATE = SYSDATE ");
            parameter.AppendSql(" WHERE ROWID =:RID ");

            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }

        public void UpDate(HIC_CHUKDTL_PLAN dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_CHUKDTL_PLAN");
            parameter.AppendSql("   SET SEQNO = :SEQNO");
            parameter.AppendSql("     , PROCESS = :PROCESS");
            parameter.AppendSql("     , PROCESS_NM = :PROCESS_NM");
            parameter.AppendSql("     , MCODE = :MCODE");
            parameter.AppendSql("     , MCODE_NM = :MCODE_NM");
            parameter.AppendSql("     , JUGI = :JUGI");
            parameter.AppendSql("     , INWON = :INWON");
            parameter.AppendSql("     , JTIME = :JTIME");
            parameter.AppendSql("     , PTIME = :PTIME");
            parameter.AppendSql("     , CHKWAY = :CHKWAY");
            parameter.AppendSql("     , CHKWAY_CD = :CHKWAY_CD");
            parameter.AppendSql("     , CHKWAY_NM = :CHKWAY_NM");
            parameter.AppendSql("     , ANALWAY_NM = :ANALWAY_NM");
            parameter.AppendSql("     , CHKCOUNT = :CHKCOUNT");
            parameter.AppendSql("     , ENTDATE = SYSDATE");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN");
            parameter.AppendSql(" WHERE ROWID =:RID ");

            parameter.Add("SEQNO", dto.SEQNO);
            parameter.Add("PROCESS", dto.PROCESS);
            parameter.Add("PROCESS_NM", dto.PROCESS_NM);
            parameter.Add("MCODE", dto.MCODE);
            parameter.Add("MCODE_NM", dto.MCODE_NM);
            parameter.Add("JUGI", dto.JUGI);
            parameter.Add("INWON", dto.INWON);
            parameter.Add("JTIME", dto.JTIME);
            parameter.Add("PTIME", dto.PTIME);
            parameter.Add("CHKWAY", dto.CHKWAY);
            parameter.Add("CHKWAY_CD", dto.CHKWAY_CD);
            parameter.Add("CHKWAY_NM", dto.CHKWAY_NM);
            parameter.Add("ANALWAY_NM", dto.ANALWAY_NM);
            parameter.Add("CHKCOUNT", dto.CHKCOUNT);
            parameter.Add("ENTSABUN", dto.ENTSABUN);
            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }

    }
}
