namespace HC_Measurement.Repository
{
    using ComBase.Controls;
    using ComBase.Mvc;
    using HC_Measurement.Dto;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class HicChkSugaRepository :BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicChkSugaRepository()
        {
        }

        public List<HIC_CHK_SUGA> GetItemAll(long nYEAR, string argKeyWord)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT GJYEAR,SUCODE,SUNAME,CHKNAME,CALNAME,ACCNAME,AMT,GAMT,CHKCODE,GCODE ");
            parameter.AppendSql("      , GBUSE, ENTSABUN, ENTDATE, MCODE    ");
            parameter.AppendSql("      , KOSMOS_PMPA.FC_HICCHUK_MCODENM(MCODE) AS MCODE_NM");
            parameter.AppendSql("      , KOSMOS_PMPA.FC_HIC_CODE_NM('15', CHKCODE) AS HANG1");
            parameter.AppendSql("      , KOSMOS_PMPA.FC_HIC_CODE_GCODE('15', CHKCODE) AS HANG2");
            parameter.AppendSql("      , KOSMOS_PMPA.FC_HIC_CODE_GCODE1('15', CHKCODE) AS HANG3");
            parameter.AppendSql("      , ROWID AS RID  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CHK_SUGA                 ");
            parameter.AppendSql(" WHERE 1 = 1                                    ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                         ");
            if (!argKeyWord.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SUNAME LIKE :SUNAME                         ");
            }
            parameter.AppendSql(" ORDER BY SUCODE                                ");

            parameter.Add("GJYEAR", nYEAR);

            if (!argKeyWord.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SUNAME", argKeyWord);
            }
            
            return ExecuteReader<HIC_CHK_SUGA>(parameter);
        }

        public void Update(HIC_CHK_SUGA dto)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CHK_SUGA");
            parameter.AppendSql("   SET GJYEAR = :GJYEAR");
            parameter.AppendSql("     , SUCODE = :SUCODE");
            parameter.AppendSql("     , SUNAME = :SUNAME");
            parameter.AppendSql("     , CHKNAME = :CHKNAME");
            parameter.AppendSql("     , CALNAME = :CALNAME");
            parameter.AppendSql("     , ACCNAME = :ACCNAME");
            parameter.AppendSql("     , AMT = :AMT");
            parameter.AppendSql("     , GAMT = :GAMT");
            parameter.AppendSql("     , CHKCODE = :CHKCODE");
            parameter.AppendSql("     , GCODE = :GCODE");
            parameter.AppendSql("     , GBUSE = :GBUSE");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN");
            parameter.AppendSql("     , ENTDATE = :ENTDATE");
            parameter.AppendSql("     , MCODE = :MCODE");
            parameter.AppendSql(" WHERE ROWID =:RID ");

            parameter.Add("GJYEAR", dto.GJYEAR);
            parameter.Add("SUCODE", dto.SUCODE);
            parameter.Add("SUNAME", dto.SUNAME);
            parameter.Add("CHKNAME", dto.CHKNAME);
            parameter.Add("CALNAME", dto.CALNAME);
            parameter.Add("ACCNAME", dto.ACCNAME);
            parameter.Add("AMT", dto.AMT);
            parameter.Add("GAMT", dto.GAMT);
            parameter.Add("CHKCODE", dto.CHKCODE);
            parameter.Add("GCODE", dto.GCODE);
            parameter.Add("GBUSE", dto.GBUSE);
            parameter.Add("ENTSABUN", dto.ENTSABUN);
            parameter.Add("ENTDATE", dto.ENTDATE);
            parameter.Add("MCODE", dto.MCODE);
            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }

        public void Insert(HIC_CHK_SUGA dto)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_CHK_SUGA");
            parameter.AppendSql("(");
            parameter.AppendSql("    GJYEAR");
            parameter.AppendSql("  , SUCODE");
            parameter.AppendSql("  , SUNAME");
            parameter.AppendSql("  , CHKNAME");
            parameter.AppendSql("  , CALNAME");
            parameter.AppendSql("  , ACCNAME");
            parameter.AppendSql("  , AMT");
            parameter.AppendSql("  , GAMT");
            parameter.AppendSql("  , CHKCODE");
            parameter.AppendSql("  , GCODE");
            parameter.AppendSql("  , GBUSE");
            parameter.AppendSql("  , ENTSABUN");
            parameter.AppendSql("  , ENTDATE");
            parameter.AppendSql("  , MCODE");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :GJYEAR");
            parameter.AppendSql("  , :SUCODE");
            parameter.AppendSql("  , :SUNAME");
            parameter.AppendSql("  , :CHKNAME");
            parameter.AppendSql("  , :CALNAME");
            parameter.AppendSql("  , :ACCNAME");
            parameter.AppendSql("  , :AMT");
            parameter.AppendSql("  , :GAMT");
            parameter.AppendSql("  , :CHKCODE");
            parameter.AppendSql("  , :GCODE");
            parameter.AppendSql("  , :GBUSE");
            parameter.AppendSql("  , :ENTSABUN");
            parameter.AppendSql("  , :ENTDATE");
            parameter.AppendSql("  , :MCODE");
            parameter.AppendSql(") ");

            parameter.Add("GJYEAR", dto.GJYEAR);
            parameter.Add("SUCODE", dto.SUCODE);
            parameter.Add("SUNAME", dto.SUNAME);
            parameter.Add("CHKNAME", dto.CHKNAME);
            parameter.Add("CALNAME", dto.CALNAME);
            parameter.Add("ACCNAME", dto.ACCNAME);
            parameter.Add("AMT", dto.AMT);
            parameter.Add("GAMT", dto.GAMT);
            parameter.Add("CHKCODE", dto.CHKCODE);
            parameter.Add("GCODE", dto.GCODE);
            parameter.Add("GBUSE", dto.GBUSE);
            parameter.Add("ENTSABUN", dto.ENTSABUN);
            parameter.Add("ENTDATE", dto.ENTDATE);
            parameter.Add("MCODE", dto.MCODE);

            ExecuteNonQuery(parameter);
        }

        public HIC_CHK_SUGA GetItemByRid(string argRid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.GJYEAR");
            parameter.AppendSql("     , A.SUCODE");
            parameter.AppendSql("     , A.SUNAME");
            parameter.AppendSql("     , A.CHKNAME");
            parameter.AppendSql("     , A.CALNAME");
            parameter.AppendSql("     , A.ACCNAME");
            parameter.AppendSql("     , A.AMT");
            parameter.AppendSql("     , A.GAMT");
            parameter.AppendSql("     , A.CHKCODE");
            parameter.AppendSql("     , A.GCODE");
            parameter.AppendSql("     , A.GBUSE");
            parameter.AppendSql("     , A.ENTSABUN");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , A.MCODE");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HICCHUK_MCODENM(A.MCODE) AS MCODE_NM");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_CODE_NM('15', A.CHKCODE) AS HANG1");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_CODE_GCODE('15', A.CHKCODE) AS HANG2");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_CODE_GCODE1('15', A.CHKCODE) AS HANG3");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CHK_SUGA A ");
            parameter.AppendSql(" WHERE A.ROWID = :RID");

            parameter.Add("RID", argRid);

            return ExecuteReaderSingle<HIC_CHK_SUGA>(parameter);
        }

        public HIC_CHK_SUGA GetItemByCode(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.GJYEAR");
            parameter.AppendSql("     , A.SUCODE");
            parameter.AppendSql("     , A.SUNAME");
            parameter.AppendSql("     , A.CHKNAME");
            parameter.AppendSql("     , A.CALNAME");
            parameter.AppendSql("     , A.ACCNAME");
            parameter.AppendSql("     , A.AMT");
            parameter.AppendSql("     , A.GAMT");
            parameter.AppendSql("     , A.CHKCODE");
            parameter.AppendSql("     , A.GCODE");
            parameter.AppendSql("     , A.GBUSE");
            parameter.AppendSql("     , A.ENTSABUN");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , A.MCODE");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HICCHUK_MCODENM(A.MCODE) AS MCODE_NM");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_CODE_NM('15', A.CHKCODE) AS HANG1");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_CODE_GCODE('15', A.CHKCODE) AS HANG2");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_CODE_GCODE1('15', A.CHKCODE) AS HANG3");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CHK_SUGA A ");
            parameter.AppendSql(" WHERE A.SUCODE = :SUCODE");

            parameter.Add("SUCODE", argCode);

            return ExecuteReaderSingle<HIC_CHK_SUGA>(parameter);
        }
    }
}
