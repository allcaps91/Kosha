namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicSunapdtlGroupcodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSunapdtlGroupcodeRepository()
        {
        }

        public List<HIC_SUNAPDTL_GROUPCODE> GetCodeNamebyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Code, b.Name                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL a, KOSMOS_PMPA.HIC_GROUPCODE b ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                                        ");
            parameter.AppendSql("   AND a.Code >= 'Z0000'                                       ");
            parameter.AppendSql("   AND a.Code  = b.Code(+)                                     ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_SUNAPDTL_GROUPCODE>(parameter);
        }

        public HIC_SUNAPDTL_GROUPCODE GetYNameByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.CODE, b.YNAME, b.NAME             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAPDTL a          ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HEA_GROUPCODE b         ");
            parameter.AppendSql(" WHERE a.WRTNO =:WRTNO                     ");
            parameter.AppendSql("   AND a.CODE = b.CODE(+)                  ");
            parameter.AppendSql("   AND b.JONG IN ('11', '12')              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_SUNAPDTL_GROUPCODE>(parameter);
        }

        public List<HIC_SUNAPDTL_GROUPCODE> GetWrtNoCodeNamebyWrtNoCode(long nWRTNO, List<string> strTemp, List<string> fstrNotAddPanList)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO, a.Code, b.Name                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL a, KOSMOS_PMPA.HIC_GROUPCODE b     ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                            ");
            parameter.AppendSql("   AND a.Code IN (:INCODE)                                         ");
            parameter.AppendSql("   AND a.CODE NOT IN (:NOTINCODE)                                  ");
            parameter.AppendSql("   AND a.Code = b.Code(+)                                          ");
            parameter.AppendSql("   AND (b.GbNotAddpan IS NULL OR b.GbNotAddpan='')                 ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("INCODE", strTemp);
            parameter.Add("NOTINCODE", fstrNotAddPanList);

            return ExecuteReader<HIC_SUNAPDTL_GROUPCODE>(parameter);
        }

        public List<HIC_SUNAPDTL_GROUPCODE> GetNamebyWrtNoInCode(long fnWRTNO, List<string> strTemp, List<string> strNotCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.Name                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL  a                 ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_GROUPCODE b                 ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                            ");
            if (strTemp.Count > 0)
            {
                parameter.AppendSql("   AND a.CODE IN (:CODE)                       ");
            }
            //2015-09-15 생애 B형간염검사 및 66세 골밀도검사 추가판정 안함
            if (strNotCode.Count > 0)
            {
                parameter.AppendSql("   AND a.Code NOT IN (:NOTCODE)                ");
            }
            parameter.AppendSql("   AND a.Code = b.Code(+)                          ");
            parameter.AppendSql("   AND (b.GbNotAddpan IS NULL OR b.GbNotAddpan='') ");

            parameter.Add("WRTNO", fnWRTNO);
            if (strTemp.Count > 0)
            {
                parameter.AddInStatement("CODE", strTemp);
            }
            if (strNotCode.Count > 0)
            {
                parameter.AddInStatement("NOTCODE", strNotCode);
            }

            return ExecuteReader<HIC_SUNAPDTL_GROUPCODE>(parameter);
        }

        public List<HIC_SUNAPDTL_GROUPCODE> GetGbSelfGbAmbyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.GbSelf, b.GBAM                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL  a                 ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_GROUPCODE b                 ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                            ");
            parameter.AppendSql("   AND a.CODE = b.CODE(+)                          ");
            parameter.AppendSql(" AND (b.HANG <> 'H' OR b.CODE in('3166','3167'))   ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReader<HIC_SUNAPDTL_GROUPCODE>(parameter);
        }

        public List<HIC_SUNAPDTL_GROUPCODE> GetCodeGbSangdambyWrtNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.CODE, b.GBSANGDAM                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL a, KOSMOS_PMPA.HIC_GROUPCODE b ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                                        ");
            parameter.AppendSql("   AND b.GbSangdam IS NOT NULL                                 ");
            parameter.AppendSql("   AND a.Code  = b.Code(+)                                     ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<HIC_SUNAPDTL_GROUPCODE>(parameter);
        }

        public List<HIC_SUNAPDTL_GROUPCODE> GetExamNamebyWrtNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT B.EXAMNAME                                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL a, KOSMOS_PMPA.HIC_GROUPCODE b     ");
            parameter.AppendSql(" WHERE 1 = 1                                                       ");
            parameter.AppendSql(" AND A.CODE = B.CODE                                               ");
            parameter.AppendSql(" AND A.WRTNO   = :WRTNO                                            ");
            parameter.AppendSql(" AND A.CODE   = 'J400'                                             ");
            parameter.AppendSql(" AND B.EXAMNAME IS NOT NULL                                        ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<HIC_SUNAPDTL_GROUPCODE>(parameter);
        }
    }
}
