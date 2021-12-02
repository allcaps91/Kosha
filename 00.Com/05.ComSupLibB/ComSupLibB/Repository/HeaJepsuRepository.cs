namespace ComSupLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComSupLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuRepository()
        {
        }

        public HEA_JEPSU GetItemByPtnoSDate(string argPtno, string argSDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, AGE, SEX, SDATE                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");
            parameter.AppendSql("   AND PTNO =:PTNO                                 ");
            parameter.AppendSql("   AND JEPDATE =TO_DATE(:JEPDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("JEPDATE", argSDate);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }
    }
}
