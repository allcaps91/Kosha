namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaMailsendRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaMailsendRepository()
        {
        }

        public List<HEA_MAILSEND> GetItembySendDate(string strSendDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO, MAILCODE, JUSO, SNAME                ");
            parameter.AppendSql("     , TO_CHAR(SENDDATE,'YYYY-MM-DD') SendDate     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_MAILSEND                    ");
            parameter.AppendSql(" WHERE SENDDATE = TO_DATE(:SENDDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql(" ORDER BY SEQ                                      ");

            parameter.Add("SENDDATE", strSendDate);

            return ExecuteReader<HEA_MAILSEND>(parameter);
        }

        public int Insert(HEA_MAILSEND item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_MAILSEND                               ");
            parameter.AppendSql("       (MAILCODE, JUSO, SNAME, SENDDATE, WRTNO, ENTSABUN )         ");
            parameter.AppendSql(" VALUES                                                            ");
            parameter.AppendSql("       (:MAILCODE, :JUSO, :SNAME, TO_DATE(:SENDDATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("     , :WRTNO, :ENTSABUN )                                         ");

            parameter.Add("MAILCODE", item.MAILCODE);
            parameter.Add("JUSO", item.JUSO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SENDDATE", item.SENDDATE);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("ENTSABUN", item.ENTSABUN);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbySendDate(long nWrtNo, string strSendDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_MAILSEND                    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND SENDDATE = TO_DATE(:SENDDATE,'YYYY-MM-DD')  ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("SENDDATE", strSendDate);

            return ExecuteScalar<int>(parameter);
        }
    }
}
