namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;

    public class HeaMailSendJepsuRepository : BaseRepository
    {
        public HeaMailSendJepsuRepository()
        {
        }

        public List<HEA_MAILSEND_JEPSU> GetItembySendDate(string strSendDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,a.MAILCODE,a.JUSO,b.SNAME,TO_CHAR(a.SENDDATE,'YYYY-MM-DD') SendDate ");
            parameter.AppendSql("     , TO_CHAR(b.RecvDATE,'YYYY-MM-DD') RecvDATE                                   ");
            parameter.AppendSql("  From ADMIN.HEA_MAILSEND a                                                  ");
            parameter.AppendSql("     , ADMIN.HEA_JEPSU    b                                                  ");
            parameter.AppendSql(" WHERE a.SENDDATE = TO_DATE(:SENDDATE,'YYYY-MM-DD')                                ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                        ");
            parameter.AppendSql("   AND b.DelDate IS NULL                                                           ");
            parameter.AppendSql(" ORDER BY SEQ                                                                      ");

            parameter.Add("SENDDATE", strSendDate);

            return ExecuteReader<HEA_MAILSEND_JEPSU>(parameter);
        }

        public HEA_MAILSEND GetSendDateByWrtno(long wRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(SENDDATE, 'YYYY-MM-DD') || CHR(10) || ADMIN.FC_INSA_MST_KORNAME(ENTSABUN) AS SEND_INFO   ");
            parameter.AppendSql("      ,SENDDATE                                                                    ");
            parameter.AppendSql("  From ADMIN.HEA_MAILSEND                                                    ");
            parameter.AppendSql(" WHERE WRTNO =:WRTNO                                                               ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReaderSingle<HEA_MAILSEND>(parameter);
        }
    }
}
