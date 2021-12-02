namespace ComSupLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComSupLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuRepository()
        {
        }

        public HIC_JEPSU GetItemByPtnoSDate(string argPtno, string argSDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, AGE, SEX, JEPDATE                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");
            parameter.AppendSql("   AND PTNO =:PTNO                                 ");
            parameter.AppendSql("   AND JEPDATE =TO_DATE(:JEPDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("JEPDATE", argSDate);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }
    }
}
