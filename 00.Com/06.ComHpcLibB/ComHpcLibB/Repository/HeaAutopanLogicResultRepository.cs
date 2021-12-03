namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HeaAutopanLogicResultRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaAutopanLogicResultRepository()
        {
        }

        public List<HEA_AUTOPAN_LOGIC_RESULT> GetItembyWrtNoSeqNo(string argWrtNo, string argSeqno, string argJepNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.EXCODE, A.GUBUN, A.SEX, B.RESULT, LOGIC, RESULTVALUE              ");
            parameter.AppendSql("  FROM ADMIN.HEA_AUTOPAN_LOGIC A, ADMIN.HIC_RESULT  B          ");
            parameter.AppendSql(" WHERE b.WRTNO = :JEPNO                                                    ");
            parameter.AppendSql("   AND A.EXCODE = B.EXCODE                                                 ");
            parameter.AppendSql("   AND A.WRTNO = :WRTNO                                                    ");
            parameter.AppendSql("   AND A.GUBUN = '1'                                                       ");
            parameter.AppendSql("   AND A.SEQNO = :SEQNO                                                    ");
            parameter.AppendSql("   AND B.RESULT NOT IN ('.','0')                                           ");
            parameter.AppendSql("   AND A.EXCODE IN (                                                       ");
            parameter.AppendSql("                     SELECT EXCODE                                         ");
            parameter.AppendSql("                       FROM ADMIN.HEA_AUTOPAN_LOGIC                  ");
            parameter.AppendSql("                      WHERE WRTNO = :WRTNO                                 ");
            parameter.AppendSql("                        AND SEQNO = :SEQNO                                 ");
            parameter.AppendSql("                      GROUP BY EXCODE                                      ");
            parameter.AppendSql("                     HAVING SUM(1) < 2 OR SUM(1) > 2                       ");
            parameter.AppendSql("                   )                                                       ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("SEQNO", argSeqno);
            parameter.Add("JEPNO", argJepNo);

            return ExecuteReader<HEA_AUTOPAN_LOGIC_RESULT>(parameter);
        }

        public List<HEA_AUTOPAN_LOGIC_RESULT> GetItembyWrtNoSeqNo_Forth(string argWrtNo, string argSeqno, string argJepNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.SEX, A.EXCODE1, A.EXCODE2, B.RETVAL1, A.CALC, C.RETVAL2, A.LOGIC, A.RESULTVALUE   ");
            parameter.AppendSql("  FROM ADMIN.HEA_AUTOPAN_LOGIC_CALC A                                                ");
            parameter.AppendSql("     , (SELECT EXCODE1, RESULTVALUE RETVAL1                                                ");
            parameter.AppendSql("          FROM ADMIN.HEA_AUTOPAN_LOGIC_CALC A, ADMIN.HIC_RESULT  B             ");
            parameter.AppendSql("         WHERE a.EXCODE1 = b.EXCODE                                                        ");
            parameter.AppendSql("           AND B.WRTNO = :JEPNO                                                            ");
            parameter.AppendSql("           AND A.SEQNO = :SEQNO                                                            ");
            parameter.AppendSql("           AND B.RESULT NOT IN ('.','0')                                                   ");
            parameter.AppendSql("           AND A.WRTNO = :WRTNO                                                            ");
            parameter.AppendSql("       ) B                                                                                 ");
            parameter.AppendSql("     , (SELECT EXCODE2, RESULTVALUE RETVAL2                                                ");
            parameter.AppendSql("          FROM ADMIN.HEA_AUTOPAN_LOGIC_CALC A, ADMIN.HIC_RESULT  B             ");
            parameter.AppendSql("         WHERE a.EXCODE2 = b.EXCODE                                                        ");
            parameter.AppendSql("           AND B.WRTNO = :JEPNO                                                            ");
            parameter.AppendSql("           AND A.SEQNO = :SEQNO                                                            ");
            parameter.AppendSql("           AND B.RESULT NOT IN ('.','0')                                                   ");
            parameter.AppendSql("           AND A.WRTNO = :WRTNO                                                            ");
            parameter.AppendSql("       ) C                                                                                 ");
            parameter.AppendSql(" WHERE a.EXCODE1 = b.EXCODE1                                                               ");
            parameter.AppendSql("   AND A.EXCODE2 = C.EXCODE2(+)                                                            ");
            parameter.AppendSql("   AND A.WRTNO = :WRTNO                                                                    ");
            parameter.AppendSql("   AND A.SEQNO = :SEQNO                                                                    ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("SEQNO", argSeqno);
            parameter.Add("JEPNO", argJepNo);

            return ExecuteReader<HEA_AUTOPAN_LOGIC_RESULT>(parameter);
        }

        public List<HEA_AUTOPAN_LOGIC_RESULT> GetItembyWrtNoSeqNo_Third(string argWrtNo, string argSeqno, string argJepNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.EXCODE, A.GUBUN, A.SEX, B.RESULT, LOGIC, RESULTVALUE              ");
            parameter.AppendSql("  FROM ADMIN.HEA_AUTOPAN_LOGIC A, ADMIN.HIC_RESULT  B          ");
            parameter.AppendSql(" WHERE b.WRTNO = :JEPNO                                                    ");
            parameter.AppendSql("   AND A.EXCODE = B.EXCODE                                                 ");
            parameter.AppendSql("   AND A.SEQNO = :SEQNO                                                    ");
            parameter.AppendSql("   AND A.WRTNO = :WRTNO                                                    ");            
            parameter.AppendSql("   AND B.RESULT NOT IN ('.','0')                                           ");
            parameter.AppendSql("   AND A.GUBUN = '2'                                                       ");            

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("SEQNO", argSeqno);
            parameter.Add("JEPNO", argJepNo);

            return ExecuteReader<HEA_AUTOPAN_LOGIC_RESULT>(parameter);
        }

        public List<HEA_AUTOPAN_LOGIC_RESULT> GetItembyWrtNoSeqNo_Second(string argWrtNo, string argSeqno, string argJepNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.EXCODE, A.GUBUN, A.SEX, B.RESULT, LOGIC, RESULTVALUE              ");
            parameter.AppendSql("  FROM ADMIN.HEA_AUTOPAN_LOGIC A, ADMIN.HIC_RESULT  B          ");
            parameter.AppendSql(" WHERE b.WRTNO = :JEPNO                                                    ");
            parameter.AppendSql("   AND A.EXCODE = B.EXCODE                                                 ");
            parameter.AppendSql("   AND A.WRTNO = :WRTNO                                                    ");
            parameter.AppendSql("   AND A.GUBUN = '1'                                                       ");
            parameter.AppendSql("   AND A.SEQNO = :SEQNO                                                    ");
            parameter.AppendSql("   AND B.RESULT NOT IN ('.','0')                                           ");
            parameter.AppendSql("   AND A.EXCODE IN (                                                       ");
            parameter.AppendSql("                     SELECT EXCODE                                         ");
            parameter.AppendSql("                       FROM ADMIN.HEA_AUTOPAN_LOGIC                  ");
            parameter.AppendSql("                      WHERE WRTNO = :WRTNO                                 ");
            parameter.AppendSql("                        AND SEQNO = :SEQNO                                 ");
            parameter.AppendSql("                      GROUP BY EXCODE                                      ");
            parameter.AppendSql("                     HAVING SUM(1) = 2                                     ");
            parameter.AppendSql("                   )                                                       ");
            parameter.AppendSql(" ORDER BY A.EXCODE, DECODE(LOGIC, '<', 1, '<=', 2, '>', 3, '>=', 4) ASC    ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("SEQNO", argSeqno);
            parameter.Add("JEPNO", argJepNo);

            return ExecuteReader<HEA_AUTOPAN_LOGIC_RESULT>(parameter);
        }
    }
}
