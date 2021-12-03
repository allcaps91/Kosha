namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaAutopanLogicRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaAutopanLogicRepository()
        {
        }

        public int GetCountbyWrtNo(string argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM ADMIN.HEA_AUTOPAN_LOGIC   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql(" GROUP BY EXCODE                       ");

            parameter.AddLikeStatement("WRTNO", argWrtNo);

            return ExecuteScalar<int>(parameter);
        }

        public string GetExCodebyWrtNo(string argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE                          ");
            parameter.AppendSql("  FROM ADMIN.HEA_AUTOPAN_LOGIC   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql(" GROUP BY EXCODE                       ");

            parameter.AddLikeStatement("WRTNO", argWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public int GetCountbyWrtNoSeqNoJepNo_Implement(string argWrtNo, string argSeqno, string argJepNo, string argLogic)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                                                      ");
            parameter.AppendSql("  FROM ADMIN.HEA_AUTOPAN_LOGIC_EXAM A, ADMIN.HIC_RESULT  B     ");
            parameter.AppendSql(" WHERE b.WRTNO = :JEPNO                                                    ");
            parameter.AppendSql("   AND A.EXCODE = B.EXCODE                                                 ");
            parameter.AppendSql("   AND A.SEQNO = :SEQNO                                                    ");            
            parameter.AppendSql("   AND A.WRTNO = :WRTNO                                                    ");
            parameter.AppendSql("   AND A.LOGIC = :LOGIC                                                    ");

            parameter.AddLikeStatement("WRTNO", argWrtNo);
            parameter.AddLikeStatement("JEPNO", argJepNo);
            parameter.AddLikeStatement("SEQNO", argSeqno);
            parameter.AddLikeStatement("LOGIC", argLogic);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyWrtNoSeqNoJepNo(string argWrtNo, string argSeqno, string argJepNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                                                      ");
            parameter.AppendSql("  FROM ADMIN.HEA_AUTOPAN_LOGIC_EXAM A, ADMIN.HIC_RESULT  B     ");
            parameter.AppendSql(" WHERE b.WRTNO = :JEPNO                                                    ");
            parameter.AppendSql("   AND A.EXCODE = B.EXCODE                                                 ");
            parameter.AppendSql("   AND A.SEQNO = :SEQNO                                                    ");
            parameter.AppendSql("   AND B.RESULT NOT IN ('.','0')                                           ");
            parameter.AppendSql("   AND A.WRTNO = :WRTNO                                                    ");

            parameter.AddLikeStatement("WRTNO", argWrtNo);
            parameter.AddLikeStatement("JEPNO", argJepNo);
            parameter.AddLikeStatement("SEQNO", argSeqno);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyWrtNoSeqNo(string argWrtNo, string argSeqno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                                                              ");
            parameter.AppendSql("  FROM ADMIN.HEA_AUTOPAN_LOGIC_EXAM                                          ");
            parameter.AppendSql(" WHERE EXETC IN ('혈압약 복용 중','당뇨병 치료 중','흡연','고혈압 치료 중','음주') ");
            parameter.AppendSql("   AND WRTNO = :WRTNO                                                              ");
            parameter.AppendSql("   AND SEQNO = :SEQNO                                                              ");

            parameter.AddLikeStatement("WRTNO", argWrtNo);
            parameter.AddLikeStatement("SEQNO", argSeqno);

            return ExecuteScalar<int>(parameter);
        }
    }
}
