namespace HC_Measurement.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using HC_Measurement.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HicChukDtlResultRepository :BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicChukDtlResultRepository()
        {
        }

        public void InSert(HIC_CHUKDTL_RESULT dto)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_CHUKDTL_RESULT");
            parameter.AppendSql("(");
            parameter.AppendSql("    WRTNO");
            parameter.AppendSql("  , GUBUN");
            parameter.AppendSql("  , SEQNO");
            parameter.AppendSql("  , DEPT_NM");
            parameter.AppendSql("  , PROCS_CD");
            parameter.AppendSql("  , PROCS_NM");
            parameter.AppendSql("  , UNIT_WRKRUM_NM");
            parameter.AppendSql("  , CHMCLS_CD");
            parameter.AppendSql("  , CHMCLS_NM");
            parameter.AppendSql("  , UCODE_GROUP_CD");
            parameter.AppendSql("  , UCODE_GROUP_SEQ");
            parameter.AppendSql("  , LABRR_CD");
            parameter.AppendSql("  , OPERT_CN");
            parameter.AppendSql("  , LABOR_CND");
            parameter.AppendSql("  , LABOR_TIME");
            parameter.AppendSql("  , UCODE_EXPSR_TIME");
            parameter.AppendSql("  , UCODE_EXPSR_CYCLE");
            parameter.AppendSql("  , WEM_LC");
            parameter.AppendSql("  , LABRR_NM");
            parameter.AppendSql("  , WEM_TIME_FROM");
            parameter.AppendSql("  , WEM_TIME_TO");
            parameter.AppendSql("  , WEM_CO");
            parameter.AppendSql("  , WEM_VALUE_AVRG_ETC");
            parameter.AppendSql("  , WEM_VALUE_AVRG");
            parameter.AppendSql("  , WEM_VALUE_PREV_ETC");
            parameter.AppendSql("  , WEM_VALUE_PREV");
            parameter.AppendSql("  , WEM_VALUE_NOW_ETC");
            parameter.AppendSql("  , WEM_VALUE_NOW");
            parameter.AppendSql("  , EXPSR_STDR_DEFAULT");
            parameter.AppendSql("  , EXPSR_STDR_VALUE");
            parameter.AppendSql("  , EXPSR_STDR_SE");
            parameter.AppendSql("  , EXPSR_STDR_UNIT");
            parameter.AppendSql("  , WEN_EVL_RESULT");
            parameter.AppendSql("  , ANALS_MTH_CD");
            parameter.AppendSql("  , WEM_MTH_NM");
            parameter.AppendSql("  , ANALS_MTH_NM");
            parameter.AppendSql("  , REMARK");
            parameter.AppendSql("  , ENTDATE");
            parameter.AppendSql("  , ENTSABUN");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :WRTNO");
            parameter.AppendSql("  , :GUBUN");
            parameter.AppendSql("  , :SEQNO");
            parameter.AppendSql("  , :DEPT_NM");
            parameter.AppendSql("  , :PROCS_CD");
            parameter.AppendSql("  , :PROCS_NM");
            parameter.AppendSql("  , :UNIT_WRKRUM_NM");
            parameter.AppendSql("  , :CHMCLS_CD");
            parameter.AppendSql("  , :CHMCLS_NM");
            parameter.AppendSql("  , :UCODE_GROUP_CD");
            parameter.AppendSql("  , :UCODE_GROUP_SEQ");
            parameter.AppendSql("  , :LABRR_CD");
            parameter.AppendSql("  , :OPERT_CN");
            parameter.AppendSql("  , :LABOR_CND");
            parameter.AppendSql("  , :LABOR_TIME");
            parameter.AppendSql("  , :UCODE_EXPSR_TIME");
            parameter.AppendSql("  , :UCODE_EXPSR_CYCLE");
            parameter.AppendSql("  , :WEM_LC");
            parameter.AppendSql("  , :LABRR_NM");
            parameter.AppendSql("  , :WEM_TIME_FROM");
            parameter.AppendSql("  , :WEM_TIME_TO");
            parameter.AppendSql("  , :WEM_CO");
            parameter.AppendSql("  , :WEM_VALUE_AVRG_ETC");
            parameter.AppendSql("  , :WEM_VALUE_AVRG");
            parameter.AppendSql("  , :WEM_VALUE_PREV_ETC");
            parameter.AppendSql("  , :WEM_VALUE_PREV");
            parameter.AppendSql("  , :WEM_VALUE_NOW_ETC");
            parameter.AppendSql("  , :WEM_VALUE_NOW");
            parameter.AppendSql("  , :EXPSR_STDR_DEFAULT");
            parameter.AppendSql("  , :EXPSR_STDR_VALUE");
            parameter.AppendSql("  , :EXPSR_STDR_SE");
            parameter.AppendSql("  , :EXPSR_STDR_UNIT");
            parameter.AppendSql("  , :WEN_EVL_RESULT");
            parameter.AppendSql("  , :ANALS_MTH_CD");
            parameter.AppendSql("  , :WEM_MTH_NM");
            parameter.AppendSql("  , :ANALS_MTH_NM");
            parameter.AppendSql("  , :REMARK");
            parameter.AppendSql("  , SYSDATE");
            parameter.AppendSql("  , :ENTSABUN");
            parameter.AppendSql(") ");

            parameter.Add("WRTNO",              dto.WRTNO);
            parameter.Add("GUBUN",              dto.GUBUN);
            parameter.Add("SEQNO",              dto.SEQNO);
            parameter.Add("DEPT_NM",            dto.DEPT_NM);
            parameter.Add("PROCS_CD",           dto.PROCS_CD);
            parameter.Add("PROCS_NM",           dto.PROCS_NM);
            parameter.Add("UNIT_WRKRUM_NM",     dto.UNIT_WRKRUM_NM);
            parameter.Add("CHMCLS_CD",          dto.CHMCLS_CD);
            parameter.Add("CHMCLS_NM",          dto.CHMCLS_NM);
            parameter.Add("UCODE_GROUP_CD",     dto.UCODE_GROUP_CD);
            parameter.Add("UCODE_GROUP_SEQ",    dto.UCODE_GROUP_SEQ);
            parameter.Add("LABRR_CD",           dto.LABRR_CD);
            parameter.Add("OPERT_CN",           dto.OPERT_CN);
            parameter.Add("LABOR_CND",          dto.LABOR_CND);
            parameter.Add("LABOR_TIME",         dto.LABOR_TIME);
            parameter.Add("UCODE_EXPSR_TIME",   dto.UCODE_EXPSR_TIME);
            parameter.Add("UCODE_EXPSR_CYCLE",  dto.UCODE_EXPSR_CYCLE);
            parameter.Add("WEM_LC",             dto.WEM_LC);
            parameter.Add("LABRR_NM",           dto.LABRR_NM);
            parameter.Add("WEM_TIME_FROM",      dto.WEM_TIME_FROM);
            parameter.Add("WEM_TIME_TO",        dto.WEM_TIME_TO);
            parameter.Add("WEM_CO",             dto.WEM_CO);
            parameter.Add("WEM_VALUE_AVRG_ETC", dto.WEM_VALUE_AVRG_ETC);
            parameter.Add("WEM_VALUE_AVRG",     dto.WEM_VALUE_AVRG);
            parameter.Add("WEM_VALUE_PREV_ETC", dto.WEM_VALUE_PREV_ETC);
            parameter.Add("WEM_VALUE_PREV",     dto.WEM_VALUE_PREV);
            parameter.Add("WEM_VALUE_NOW_ETC",  dto.WEM_VALUE_NOW_ETC);
            parameter.Add("WEM_VALUE_NOW",      dto.WEM_VALUE_NOW);
            parameter.Add("EXPSR_STDR_DEFAULT", dto.EXPSR_STDR_default);
            parameter.Add("EXPSR_STDR_VALUE",   dto.EXPSR_STDR_VALUE);
            parameter.Add("EXPSR_STDR_SE",      dto.EXPSR_STDR_SE);
            parameter.Add("EXPSR_STDR_UNIT",    dto.EXPSR_STDR_UNIT);
            parameter.Add("WEN_EVL_RESULT",     dto.WEN_EVL_RESULT);
            parameter.Add("ANALS_MTH_CD",       dto.ANALS_MTH_CD);
            parameter.Add("WEM_MTH_NM",         dto.WEM_MTH_NM);
            parameter.Add("ANALS_MTH_NM",       dto.ANALS_MTH_NM);
            parameter.Add("REMARK",             dto.REMARK);
            parameter.Add("ENTSABUN",           dto.ENTSABUN);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_CHUKDTL_RESULT> GetPlanListByWrtno(long wRTNO, string argGbn)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.PROCESS AS PROCS_CD");
            parameter.AppendSql("     , A.PROCESS_NM AS PROCS_NM");
            parameter.AppendSql("     , A.MCODE AS CHMCLS_CD");
            parameter.AppendSql("     , A.MCODE_NM AS CHMCLS_NM");
            parameter.AppendSql("     , A.JUGI AS UCODE_EXPSR_CYCLE");
            parameter.AppendSql("     , A.INWON AS LABRR_CD");
            parameter.AppendSql("     , A.JTIME AS LABOR_TIME");
            parameter.AppendSql("     , A.CHKWAY_CD AS ANALS_MTH_CD");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_CODE_GCODE('15', A.CHKWAY_CD) AS WEM_MTH_NM");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_CODE_GCODE1('15', A.CHKWAY_CD) AS ANALS_MTH_NM");
            parameter.AppendSql("     , A.CHKCOUNT AS WEM_CO");
            parameter.AppendSql("     , DECODE (A.MCODE, '31001', '90', '') AS EXPSR_STDR_DEFAULT");
            parameter.AppendSql("     , A.SEQNO");
            parameter.AppendSql("  FROM HIC_CHUKDTL_PLAN A");
            parameter.AppendSql(" WHERE A.WRTNO=:WRTNO");
            parameter.AppendSql("   AND A.DELDATE IS NULL");
            if (argGbn == "2")
            {
                parameter.AppendSql("   AND A.MCODE IN ('31001','31002')");
            }
            else
            {
                parameter.AppendSql("   AND A.MCODE NOT IN ('31001','31002')");
            }
            parameter.AppendSql(" ORDER BY A.SEQNO ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReader<HIC_CHUKDTL_RESULT>(parameter);
        }

        public List<HIC_CHUKDTL_RESULT> GetMCodeListByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.CHMCLS_CD");
            parameter.AppendSql("     , B.NAME AS MCODE_NM");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CHUKDTL_RESULT A");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_CHK_MCODE B");
            parameter.AppendSql(" WHERE A.WRTNO=:WRTNO");
            parameter.AppendSql("   AND A.DELDATE IS NULL");
            parameter.AppendSql("   AND A.CHMCLS_CD = B.CODE");
            parameter.AppendSql("   AND B.GBUSE = 'Y'");
            parameter.AppendSql(" ORDER BY A.SEQNO ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_CHUKDTL_RESULT>(parameter);
        }

        public List<HIC_CHUKDTL_RESULT> GetListMCodeByWrtno(long nWRTNO, long nYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.SEQNO");
            parameter.AppendSql("     , A.PROCS_CD");
            parameter.AppendSql("     , A.PROCS_NM	");
            parameter.AppendSql("     , A.CHMCLS_CD");
            parameter.AppendSql("     , A.CHMCLS_NM");
            parameter.AppendSql("     , A.UCODE_EXPSR_CYCLE");
            parameter.AppendSql("     , A.LABRR_CD");
            parameter.AppendSql("     , A.WEM_TIME_FROM");
            parameter.AppendSql("     , A.WEM_TIME_TO");
            parameter.AppendSql("     , A.ANALS_MTH_CD");
            parameter.AppendSql("     , A.WEM_CO");
            parameter.AppendSql("     , A.DELDATE");
            parameter.AppendSql("     , B.GJYEAR");
            parameter.AppendSql("     , B.SUCODE");
            parameter.AppendSql("     , B.SUNAME");
            parameter.AppendSql("     , B.CHKNAME");
            parameter.AppendSql("     , B.CALNAME");
            parameter.AppendSql("     , B.ACCNAME");
            parameter.AppendSql("     , B.AMT");
            parameter.AppendSql("     , B.GAMT");
            parameter.AppendSql("     , B.CHKCODE");
            parameter.AppendSql("     , B.GCODE");
            parameter.AppendSql("     , B.GBUSE");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CHUKDTL_RESULT A");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_CHK_SUGA B");
            parameter.AppendSql(" WHERE A.WRTNO=:WRTNO");
            parameter.AppendSql("   AND A.DELDATE IS NULL");
            parameter.AppendSql("   AND B.GJYEAR = :GJYEAR");
            parameter.AppendSql("   AND A.CHMCLS_CD = B.MCODE");
            parameter.AppendSql("   AND B.GBUSE = 'Y'");
            parameter.AppendSql(" ORDER BY A.SEQNO ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("GJYEAR", nYear);

            return ExecuteReader<HIC_CHUKDTL_RESULT>(parameter);
        }

        public void Delete(HIC_CHUKDTL_RESULT dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CHUKDTL_RESULT");
            parameter.AppendSql("   SET DELDATE = SYSDATE ");
            parameter.AppendSql(" WHERE ROWID =:RID ");

            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }

        public void UpDate(HIC_CHUKDTL_RESULT dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CHUKDTL_RESULT");
            parameter.AppendSql("   SET GUBUN = :GUBUN");
            parameter.AppendSql("     , SEQNO = :SEQNO");
            parameter.AppendSql("     , DEPT_NM = :DEPT_NM");
            parameter.AppendSql("     , PROCS_CD = :PROCS_CD");
            parameter.AppendSql("     , PROCS_NM = :PROCS_NM");
            parameter.AppendSql("     , UNIT_WRKRUM_NM = :UNIT_WRKRUM_NM");
            parameter.AppendSql("     , CHMCLS_CD = :CHMCLS_CD");
            parameter.AppendSql("     , CHMCLS_NM = :CHMCLS_NM");
            parameter.AppendSql("     , UCODE_GROUP_CD = :UCODE_GROUP_CD");
            parameter.AppendSql("     , UCODE_GROUP_SEQ = :UCODE_GROUP_SEQ");
            parameter.AppendSql("     , LABRR_CD = :LABRR_CD");
            parameter.AppendSql("     , OPERT_CN = :OPERT_CN");
            parameter.AppendSql("     , LABOR_CND = :LABOR_CND");
            parameter.AppendSql("     , LABOR_TIME = :LABOR_TIME");
            parameter.AppendSql("     , UCODE_EXPSR_TIME = :UCODE_EXPSR_TIME");
            parameter.AppendSql("     , UCODE_EXPSR_CYCLE = :UCODE_EXPSR_CYCLE");
            parameter.AppendSql("     , WEM_LC = :WEM_LC");
            parameter.AppendSql("     , LABRR_NM = :LABRR_NM");
            parameter.AppendSql("     , WEM_TIME_FROM = :WEM_TIME_FROM");
            parameter.AppendSql("     , WEM_TIME_TO = :WEM_TIME_TO");
            parameter.AppendSql("     , WEM_CO = :WEM_CO");
            parameter.AppendSql("     , WEM_VALUE_AVRG_ETC = :WEM_VALUE_AVRG_ETC");
            parameter.AppendSql("     , WEM_VALUE_AVRG = :WEM_VALUE_AVRG");
            parameter.AppendSql("     , WEM_VALUE_PREV_ETC = :WEM_VALUE_PREV_ETC");
            parameter.AppendSql("     , WEM_VALUE_PREV = :WEM_VALUE_PREV");
            parameter.AppendSql("     , WEM_VALUE_NOW_ETC = :WEM_VALUE_NOW_ETC");
            parameter.AppendSql("     , WEM_VALUE_NOW = :WEM_VALUE_NOW");
            parameter.AppendSql("     , EXPSR_STDR_DEFAULT = :EXPSR_STDR_DEFAULT"); 
            parameter.AppendSql("     , EXPSR_STDR_VALUE = :EXPSR_STDR_VALUE");
            parameter.AppendSql("     , EXPSR_STDR_SE = :EXPSR_STDR_SE");
            parameter.AppendSql("     , EXPSR_STDR_UNIT = :EXPSR_STDR_UNIT");
            parameter.AppendSql("     , WEN_EVL_RESULT = :WEN_EVL_RESULT");
            parameter.AppendSql("     , ANALS_MTH_CD = :ANALS_MTH_CD");
            parameter.AppendSql("     , WEM_MTH_NM = :WEM_MTH_NM");
            parameter.AppendSql("     , ANALS_MTH_NM = :ANALS_MTH_NM");
            parameter.AppendSql("     , REMARK = :REMARK");
            parameter.AppendSql("     , ENTDATE = SYSDATE");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN");
            parameter.AppendSql(" WHERE ROWID = :RID");

            parameter.Add("GUBUN", dto.GUBUN);
            parameter.Add("SEQNO", dto.SEQNO);
            parameter.Add("DEPT_NM", dto.DEPT_NM);
            parameter.Add("PROCS_CD", dto.PROCS_CD);
            parameter.Add("PROCS_NM", dto.PROCS_NM);
            parameter.Add("UNIT_WRKRUM_NM", dto.UNIT_WRKRUM_NM);
            parameter.Add("CHMCLS_CD", dto.CHMCLS_CD);
            parameter.Add("CHMCLS_NM", dto.CHMCLS_NM);
            parameter.Add("UCODE_GROUP_CD", dto.UCODE_GROUP_CD);
            parameter.Add("UCODE_GROUP_SEQ", dto.UCODE_GROUP_SEQ);
            parameter.Add("LABRR_CD", dto.LABRR_CD);
            parameter.Add("OPERT_CN", dto.OPERT_CN);
            parameter.Add("LABOR_CND", dto.LABOR_CND);
            parameter.Add("LABOR_TIME", dto.LABOR_TIME);
            parameter.Add("UCODE_EXPSR_TIME", dto.UCODE_EXPSR_TIME);
            parameter.Add("UCODE_EXPSR_CYCLE", dto.UCODE_EXPSR_CYCLE);
            parameter.Add("WEM_LC", dto.WEM_LC);
            parameter.Add("LABRR_NM", dto.LABRR_NM);
            parameter.Add("WEM_TIME_FROM", dto.WEM_TIME_FROM);
            parameter.Add("WEM_TIME_TO", dto.WEM_TIME_TO);
            parameter.Add("WEM_CO", dto.WEM_CO);
            parameter.Add("WEM_VALUE_AVRG_ETC", dto.WEM_VALUE_AVRG_ETC);
            parameter.Add("WEM_VALUE_AVRG", dto.WEM_VALUE_AVRG);
            parameter.Add("WEM_VALUE_PREV_ETC", dto.WEM_VALUE_PREV_ETC);
            parameter.Add("WEM_VALUE_PREV", dto.WEM_VALUE_PREV);
            parameter.Add("WEM_VALUE_NOW_ETC", dto.WEM_VALUE_NOW_ETC);
            parameter.Add("WEM_VALUE_NOW", dto.WEM_VALUE_NOW);
            parameter.Add("EXPSR_STDR_DEFAULT", dto.EXPSR_STDR_default);
            parameter.Add("EXPSR_STDR_VALUE", dto.EXPSR_STDR_VALUE);
            parameter.Add("EXPSR_STDR_SE", dto.EXPSR_STDR_SE);
            parameter.Add("EXPSR_STDR_UNIT", dto.EXPSR_STDR_UNIT);
            parameter.Add("WEN_EVL_RESULT", dto.WEN_EVL_RESULT);
            parameter.Add("ANALS_MTH_CD", dto.ANALS_MTH_CD);
            parameter.Add("WEM_MTH_NM", dto.WEM_MTH_NM);
            parameter.Add("ANALS_MTH_NM", dto.ANALS_MTH_NM);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("ENTSABUN", dto.ENTSABUN);
            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }


        public List<HIC_CHUKDTL_RESULT> GetListByWrtno(long argWRTNO, string argGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.GUBUN");
            parameter.AppendSql("     , A.SEQNO");
            parameter.AppendSql("     , A.DEPT_NM");
            parameter.AppendSql("     , A.PROCS_CD");
            parameter.AppendSql("     , A.PROCS_NM");
            parameter.AppendSql("     , A.UNIT_WRKRUM_NM");
            parameter.AppendSql("     , A.CHMCLS_CD");
            parameter.AppendSql("     , A.CHMCLS_NM");
            parameter.AppendSql("     , A.UCODE_GROUP_CD");
            parameter.AppendSql("     , A.UCODE_GROUP_SEQ");
            parameter.AppendSql("     , A.LABRR_CD");
            parameter.AppendSql("     , A.OPERT_CN");
            parameter.AppendSql("     , A.LABOR_CND");
            parameter.AppendSql("     , A.LABOR_TIME");
            parameter.AppendSql("     , A.UCODE_EXPSR_TIME");
            parameter.AppendSql("     , A.UCODE_EXPSR_CYCLE");
            parameter.AppendSql("     , A.WEM_LC");
            parameter.AppendSql("     , A.LABRR_NM");
            parameter.AppendSql("     , A.WEM_TIME_FROM");
            parameter.AppendSql("     , A.WEM_TIME_TO");
            parameter.AppendSql("     , A.WEM_CO");
            parameter.AppendSql("     , A.WEM_VALUE_AVRG_ETC");
            parameter.AppendSql("     , A.WEM_VALUE_AVRG");
            parameter.AppendSql("     , A.WEM_VALUE_PREV_ETC");
            parameter.AppendSql("     , A.WEM_VALUE_PREV");
            parameter.AppendSql("     , A.WEM_VALUE_NOW_ETC");
            parameter.AppendSql("     , A.WEM_VALUE_NOW");
            parameter.AppendSql("     , A.EXPSR_STDR_default"); 
            parameter.AppendSql("     , A.EXPSR_STDR_VALUE");
            parameter.AppendSql("     , A.EXPSR_STDR_SE");
            parameter.AppendSql("     , A.EXPSR_STDR_UNIT");
            parameter.AppendSql("     , A.WEN_EVL_RESULT");
            parameter.AppendSql("     , A.ANALS_MTH_CD");
            parameter.AppendSql("     , A.WEM_MTH_NM");
            parameter.AppendSql("     , A.ANALS_MTH_NM");
            parameter.AppendSql("     , A.REMARK");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , A.ENTSABUN");
            parameter.AppendSql("     , A.DELDATE");
            parameter.AppendSql("     , A.ROWID AS RID");
            parameter.AppendSql("  FROM HIC_CHUKDTL_RESULT A");
            parameter.AppendSql(" WHERE A.WRTNO=:WRTNO");
            parameter.AppendSql("   AND A.GUBUN =:GUBUN");
            parameter.AppendSql("   AND A.DELDATE IS NULL");
            parameter.AppendSql(" ORDER BY A.SEQNO ");

            parameter.Add("WRTNO", argWRTNO);
            parameter.Add("GUBUN", argGubun);

            return ExecuteReader<HIC_CHUKDTL_RESULT>(parameter);
        }

    }
}
