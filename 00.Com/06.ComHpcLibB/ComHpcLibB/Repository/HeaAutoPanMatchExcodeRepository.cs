namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;

    public class HeaAutoPanMatchExcodeRepository : BaseRepository
    {
        public List<HEA_AUTOPAN_MATCH_EXCODE> GetItembyWrtNo(string argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT '' CHK, A.MCODE, A.EXCODE, B.HNAME, B.UNIT, A.ROWID ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_AUTOPAN_MATCH A                     ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE        B                     ");
            parameter.AppendSql(" WHERE A.EXCODE = B.CODE                                   ");
            parameter.AppendSql("   AND A.WRTNO = :WRTNO                                    ");
            parameter.AppendSql(" ORDER BY MCODE                                            ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReader<HEA_AUTOPAN_MATCH_EXCODE>(parameter);
        }
    }
}
