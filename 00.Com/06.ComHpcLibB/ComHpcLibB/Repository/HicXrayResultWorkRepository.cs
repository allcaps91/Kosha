
namespace ComHpcLibB
{

    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicXrayResultWorkRepository : BaseRepository
    {
        public HicXrayResultWorkRepository()
        {
        }

        public List<HIC_XRAY_RESULT_WORK> GetListByItem(string argDate, string argPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT *                                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT_WORK                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                          ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("   AND PANO = :PANO                                                   ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                ");

            parameter.Add("JEPDATE", argDate);
            parameter.Add("PANO", argPano);

            return ExecuteReader<HIC_XRAY_RESULT_WORK>(parameter);
        }

        public IList<HIC_XRAY_RESULT_WORK> GetChulListByItem(string argDate, long argLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JEPDATE, 'YYYY-MM-DD') JEPDATE, PANO, SNAME, AGE, SEX, LTDCODE              ");
            parameter.AppendSql(" ,KOSMOS_PMPA.FC_HIC_LTDNAME(LTDCODE) LTDNAME, ROWID AS RID                                ");
            parameter.AppendSql(" ,CASE WHEN GBREAD = '1' THEN  '일반' ELSE '분진' END AS GBREAD                             ");
            parameter.AppendSql(" ,SUBSTR(XRAYNO, 1, 8) || ' ' || SUBSTR(XRAYNO, 9, 5) AS XRAYNO                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT_WORK                                                    ");
            parameter.AppendSql(" WHERE 1 = 1                                                                               ");
            parameter.AppendSql("   AND JEPDATE >= TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                          ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                                     ");
            parameter.AppendSql("   AND GBCHUL = 'Y'                                                                        ");
            if (argLtdCode > 0) { parameter.AppendSql(" AND LTDCODE =:LTDCODE                                               "); }
            parameter.AppendSql("   ORDER BY LTDCODE, JEPDATE, SNAME                                                        ");

            parameter.Add("JEPDATE", argDate);
            if (argLtdCode > 0) { parameter.Add("LTDCODE", argLtdCode); }

            return ExecuteReader<HIC_XRAY_RESULT_WORK>(parameter);
        }



    }
}
