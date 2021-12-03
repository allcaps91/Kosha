namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;

    public class HeaAutoPanMatchResultRepository : BaseRepository
    {
        public HeaAutoPanMatchResultRepository()
        {

        }

        public List<HEA_AUTOPAN_MATCH_RESULT> GetItembyWrtno(string argWrtNo, string argJepNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.MCODE, A.EXCODE, B.RESULT     ");
            parameter.AppendSql("  FROM ADMIN.HEA_AUTOPAN_MATCH A ");
            parameter.AppendSql("     , ADMIN.HEA_RESULT        B ");
            parameter.AppendSql(" WHERE A.WRTNO = :WRTNOA               ");
            parameter.AppendSql("   AND B.WRTNO = :WRTNOB               ");
            parameter.AppendSql("   AND A.EXCODE = B.EXCODE             ");

            parameter.AddLikeStatement("WRTNOA", argWrtNo);
            parameter.AddLikeStatement("WRTNOB", argJepNo);

            return ExecuteReader<HEA_AUTOPAN_MATCH_RESULT>(parameter);
        }
    }
}
