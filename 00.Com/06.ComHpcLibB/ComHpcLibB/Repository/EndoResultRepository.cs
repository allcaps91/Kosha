namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class EndoResultRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public EndoResultRepository()
        {
        }

        public ENDO_RESULT GetItemBySeqno(long nSEQNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SEQNO,REMARK1,REMARK2,REMARK3,REMARK4,REMARK5,PICXY ");
            parameter.AppendSql("      ,REMARK6,REMARK6_2,REMARK6_3,REMARK                  ");
            parameter.AppendSql("  FROM ADMIN.ENDO_RESULT                              ");
            parameter.AppendSql(" WHERE SEQNO = :SEQNO                                      ");

            parameter.Add("SEQNO", nSEQNO);

            return ExecuteReaderSingle<ENDO_RESULT>(parameter);
        }
    }
}
