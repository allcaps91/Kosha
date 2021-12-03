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
    public class HicChkMcodeRepository :BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicChkMcodeRepository()
        {
        }

        public List<HIC_CHK_MCODE> GetItemAll(string argKeyWord)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT CODE,K2BCODE AS GCODE,FULLNAME, NAME,ENAME,CAS_NO,EC_NO,BUN,ANAL_BUN,CYCLE,TWA_PPM,TWA_MG ");
            parameter.AppendSql("       ,STEL_PPM, STEL_MG, CEILING, CEILING_UNIT, RET_RATE, PAS_RATE, FACTOR_VAL, CHK_WAY, CHK_UNIT ");
            parameter.AppendSql("       ,MOLECULE, CAL_CONST, APPLY_VALUE1, APPLY_VALUE2, GBCHK, GBSPC, GBMIX, GBCAN, GBUSE ");
            parameter.AppendSql("       ,SORT, ENTSABUN, ENTDATE, APPLY_VALUE3, ROWID AS RID  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CHK_MCODE                             ");
            parameter.AppendSql(" WHERE 1 = 1                                                 ");
            if (!argKeyWord.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND FULLNAME LIKE :FULLNAME                              ");
            }

            parameter.AppendSql(" ORDER BY CODE                                                 ");

            if (!argKeyWord.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("FULLNAME", argKeyWord);
            }

            return ExecuteReader<HIC_CHK_MCODE>(parameter);
        }

        public void Update(HIC_CHK_MCODE dto)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CHK_MCODE");
            parameter.AppendSql("   SET CODE = :CODE");
            parameter.AppendSql("     , K2BCODE = :K2BCODE");
            parameter.AppendSql("     , FULLNAME = :FULLNAME");
            parameter.AppendSql("     , NAME = :NAME");
            parameter.AppendSql("     , ENAME = :ENAME");
            parameter.AppendSql("     , CAS_NO = :CAS_NO");
            parameter.AppendSql("     , EC_NO = :EC_NO");
            parameter.AppendSql("     , BUN = :BUN");
            parameter.AppendSql("     , ANAL_BUN = :ANAL_BUN");
            parameter.AppendSql("     , CYCLE = :CYCLE");
            parameter.AppendSql("     , TWA_PPM = :TWA_PPM");
            parameter.AppendSql("     , TWA_MG = :TWA_MG");
            parameter.AppendSql("     , STEL_PPM = :STEL_PPM");
            parameter.AppendSql("     , STEL_MG = :STEL_MG");
            parameter.AppendSql("     , CEILING = :CEILING");
            parameter.AppendSql("     , CEILING_UNIT = :CEILING_UNIT");
            parameter.AppendSql("     , RET_RATE = :RET_RATE");
            parameter.AppendSql("     , PAS_RATE = :PAS_RATE");
            parameter.AppendSql("     , FACTOR_VAL = :FACTOR_VAL");
            parameter.AppendSql("     , CHK_WAY = :CHK_WAY");
            parameter.AppendSql("     , CHK_UNIT = :CHK_UNIT");
            parameter.AppendSql("     , MOLECULE = :MOLECULE");
            parameter.AppendSql("     , CAL_CONST = :CAL_CONST");
            parameter.AppendSql("     , APPLY_VALUE1 = :APPLY_VALUE1");
            parameter.AppendSql("     , APPLY_VALUE2 = :APPLY_VALUE2");
            parameter.AppendSql("     , GBCHK = :GBCHK");
            parameter.AppendSql("     , GBSPC = :GBSPC");
            parameter.AppendSql("     , GBMIX = :GBMIX");
            parameter.AppendSql("     , GBCAN = :GBCAN");
            parameter.AppendSql("     , GBUSE = :GBUSE");
            parameter.AppendSql("     , SORT = :SORT");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN");
            parameter.AppendSql("     , ENTDATE = :ENTDATE");
            parameter.AppendSql("     , APPLY_VALUE3 = :APPLY_VALUE3");
            parameter.AppendSql(" WHERE ROWID =:RID ");

            parameter.Add("CODE", dto.CODE);
            parameter.Add("K2BCODE", dto.K2BCODE);
            parameter.Add("FULLNAME", dto.FULLNAME);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("ENAME", dto.ENAME);
            parameter.Add("CAS_NO", dto.CAS_NO);
            parameter.Add("EC_NO", dto.EC_NO);
            parameter.Add("BUN", dto.BUN);
            parameter.Add("ANAL_BUN", dto.ANAL_BUN);
            parameter.Add("CYCLE", dto.CYCLE);
            parameter.Add("TWA_PPM", dto.TWA_PPM);
            parameter.Add("TWA_MG", dto.TWA_MG);
            parameter.Add("STEL_PPM", dto.STEL_PPM);
            parameter.Add("STEL_MG", dto.STEL_MG);
            parameter.Add("CEILING", dto.CEILING);
            parameter.Add("CEILING_UNIT", dto.CEILING_UNIT);
            parameter.Add("RET_RATE", dto.RET_RATE);
            parameter.Add("PAS_RATE", dto.PAS_RATE);
            parameter.Add("FACTOR_VAL", dto.FACTOR_VAL);
            parameter.Add("CHK_WAY", dto.CHK_WAY);
            parameter.Add("CHK_UNIT", dto.CHK_UNIT);
            parameter.Add("MOLECULE", dto.MOLECULE);
            parameter.Add("CAL_CONST", dto.CAL_CONST);
            parameter.Add("APPLY_VALUE1", dto.APPLY_VALUE1);
            parameter.Add("APPLY_VALUE2", dto.APPLY_VALUE2);
            parameter.Add("GBCHK", dto.GBCHK);
            parameter.Add("GBSPC", dto.GBSPC);
            parameter.Add("GBMIX", dto.GBMIX);
            parameter.Add("GBCAN", dto.GBCAN);
            parameter.Add("GBUSE", dto.GBUSE);
            parameter.Add("SORT", dto.SORT);
            parameter.Add("ENTSABUN", dto.ENTSABUN);
            parameter.Add("ENTDATE", dto.ENTDATE);
            parameter.Add("APPLY_VALUE3", dto.APPLY_VALUE3);
            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }

        public HIC_CHK_MCODE GetItemByCode(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT CODE,K2BCODE,FULLNAME,NAME,ENAME,CAS_NO,EC_NO,BUN,ANAL_BUN,CYCLE,TWA_PPM,TWA_MG ");
            parameter.AppendSql("       ,STEL_PPM, STEL_MG, CEILING, CEILING_UNIT, RET_RATE, PAS_RATE, FACTOR_VAL, CHK_WAY, CHK_UNIT ");
            parameter.AppendSql("       ,MOLECULE, CAL_CONST, APPLY_VALUE1, APPLY_VALUE2, GBCHK, GBSPC, GBMIX, GBCAN, GBUSE ");
            parameter.AppendSql("       ,SORT, ENTSABUN, ENTDATE, APPLY_VALUE3, ROWID AS RID  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CHK_MCODE                             ");
            parameter.AppendSql(" WHERE CODE =:CODE                                           ");

            parameter.Add("CODE", argCode);

            return ExecuteReaderSingle<HIC_CHK_MCODE>(parameter);
        }

        public HIC_CHK_MCODE GetItemByRid(string argRid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT CODE,K2BCODE,FULLNAME, NAME,ENAME,CAS_NO,EC_NO,BUN,ANAL_BUN,CYCLE,TWA_PPM,TWA_MG ");
            parameter.AppendSql("       ,STEL_PPM, STEL_MG, CEILING, CEILING_UNIT, RET_RATE, PAS_RATE, FACTOR_VAL, CHK_WAY, CHK_UNIT ");
            parameter.AppendSql("       ,MOLECULE, CAL_CONST, APPLY_VALUE1, APPLY_VALUE2, GBCHK, GBSPC, GBMIX, GBCAN, GBUSE ");
            parameter.AppendSql("       ,SORT, ENTSABUN, ENTDATE, APPLY_VALUE3, ROWID AS RID  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CHK_MCODE                             ");
            parameter.AppendSql(" WHERE ROWID =:RID                                           ");

            parameter.Add("RID", argRid);

            return ExecuteReaderSingle<HIC_CHK_MCODE>(parameter);
        }

        public void Insert(HIC_CHK_MCODE dto)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_CHK_MCODE");
            parameter.AppendSql("(");
            parameter.AppendSql("    CODE");
            parameter.AppendSql("  , K2BCODE");
            parameter.AppendSql("  , FULLNAME");
            parameter.AppendSql("  , NAME");
            parameter.AppendSql("  , ENAME");
            parameter.AppendSql("  , CAS_NO");
            parameter.AppendSql("  , EC_NO");
            parameter.AppendSql("  , BUN");
            parameter.AppendSql("  , ANAL_BUN");
            parameter.AppendSql("  , CYCLE");
            parameter.AppendSql("  , TWA_PPM");
            parameter.AppendSql("  , TWA_MG");
            parameter.AppendSql("  , STEL_PPM");
            parameter.AppendSql("  , STEL_MG");
            parameter.AppendSql("  , CEILING");
            parameter.AppendSql("  , CEILING_UNIT");
            parameter.AppendSql("  , RET_RATE");
            parameter.AppendSql("  , PAS_RATE");
            parameter.AppendSql("  , FACTOR_VAL");
            parameter.AppendSql("  , CHK_WAY");
            parameter.AppendSql("  , CHK_UNIT");
            parameter.AppendSql("  , MOLECULE");
            parameter.AppendSql("  , CAL_CONST");
            parameter.AppendSql("  , APPLY_VALUE1");
            parameter.AppendSql("  , APPLY_VALUE2");
            parameter.AppendSql("  , GBCHK");
            parameter.AppendSql("  , GBSPC");
            parameter.AppendSql("  , GBMIX");
            parameter.AppendSql("  , GBCAN");
            parameter.AppendSql("  , GBUSE");
            parameter.AppendSql("  , SORT");
            parameter.AppendSql("  , ENTSABUN");
            parameter.AppendSql("  , ENTDATE");
            parameter.AppendSql("  , APPLY_VALUE3");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :CODE");
            parameter.AppendSql("  , :K2BCODE");
            parameter.AppendSql("  , :FULLNAME");
            parameter.AppendSql("  , :NAME");
            parameter.AppendSql("  , :ENAME");
            parameter.AppendSql("  , :CAS_NO");
            parameter.AppendSql("  , :EC_NO");
            parameter.AppendSql("  , :BUN");
            parameter.AppendSql("  , :ANAL_BUN");
            parameter.AppendSql("  , :CYCLE");
            parameter.AppendSql("  , :TWA_PPM");
            parameter.AppendSql("  , :TWA_MG");
            parameter.AppendSql("  , :STEL_PPM");
            parameter.AppendSql("  , :STEL_MG");
            parameter.AppendSql("  , :CEILING");
            parameter.AppendSql("  , :CEILING_UNIT");
            parameter.AppendSql("  , :RET_RATE");
            parameter.AppendSql("  , :PAS_RATE");
            parameter.AppendSql("  , :FACTOR_VAL");
            parameter.AppendSql("  , :CHK_WAY");
            parameter.AppendSql("  , :CHK_UNIT");
            parameter.AppendSql("  , :MOLECULE");
            parameter.AppendSql("  , :CAL_CONST");
            parameter.AppendSql("  , :APPLY_VALUE1");
            parameter.AppendSql("  , :APPLY_VALUE2");
            parameter.AppendSql("  , :GBCHK");
            parameter.AppendSql("  , :GBSPC");
            parameter.AppendSql("  , :GBMIX");
            parameter.AppendSql("  , :GBCAN");
            parameter.AppendSql("  , :GBUSE");
            parameter.AppendSql("  , :SORT");
            parameter.AppendSql("  , :ENTSABUN");
            parameter.AppendSql("  , :ENTDATE");
            parameter.AppendSql("  , :APPLY_VALUE3");
            parameter.AppendSql(") ");

            parameter.Add("CODE", dto.CODE);
            parameter.Add("K2BCODE", dto.K2BCODE);
            parameter.Add("FULLNAME", dto.FULLNAME);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("ENAME", dto.ENAME);
            parameter.Add("CAS_NO", dto.CAS_NO);
            parameter.Add("EC_NO", dto.EC_NO);
            parameter.Add("BUN", dto.BUN);
            parameter.Add("ANAL_BUN", dto.ANAL_BUN);
            parameter.Add("CYCLE", dto.CYCLE);
            parameter.Add("TWA_PPM", dto.TWA_PPM);
            parameter.Add("TWA_MG", dto.TWA_MG);
            parameter.Add("STEL_PPM", dto.STEL_PPM);
            parameter.Add("STEL_MG", dto.STEL_MG);
            parameter.Add("CEILING", dto.CEILING);
            parameter.Add("CEILING_UNIT", dto.CEILING_UNIT);
            parameter.Add("RET_RATE", dto.RET_RATE);
            parameter.Add("PAS_RATE", dto.PAS_RATE);
            parameter.Add("FACTOR_VAL", dto.FACTOR_VAL);
            parameter.Add("CHK_WAY", dto.CHK_WAY);
            parameter.Add("CHK_UNIT", dto.CHK_UNIT);
            parameter.Add("MOLECULE", dto.MOLECULE);
            parameter.Add("CAL_CONST", dto.CAL_CONST);
            parameter.Add("APPLY_VALUE1", dto.APPLY_VALUE1);
            parameter.Add("APPLY_VALUE2", dto.APPLY_VALUE2);
            parameter.Add("GBCHK", dto.GBCHK);
            parameter.Add("GBSPC", dto.GBSPC);
            parameter.Add("GBMIX", dto.GBMIX);
            parameter.Add("GBCAN", dto.GBCAN);
            parameter.Add("GBUSE", dto.GBUSE);
            parameter.Add("SORT", dto.SORT);
            parameter.Add("ENTSABUN", dto.ENTSABUN);
            parameter.Add("ENTDATE", dto.ENTDATE);
            parameter.Add("APPLY_VALUE3", dto.APPLY_VALUE3);

            ExecuteNonQuery(parameter);
        }

    }
}
