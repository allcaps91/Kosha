
namespace ComHpcLibB.Repository
{

    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicSunapdtlGroupexamRepository : BaseRepository
    {
        public HicSunapdtlGroupexamRepository()
        {
        }

        public List<HIC_SUNAPDTL_GROUPEXAM> GetExcodeByWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT B.EXCODE                                                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL  a                                         ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_GROUPEXAM b                                         ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                      ");
            parameter.AppendSql("   AND a.CODE = b.GROUPCODE(+)                                             ");
            parameter.AppendSql("   AND (a.UCode IS NOT NULL OR a.Code IN ('J224','1160','J231','K226'))    ");
            parameter.AppendSql("   GROUP BY B.EXCODE                                                       ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_SUNAPDTL_GROUPEXAM>(parameter);
        }



    }
}
