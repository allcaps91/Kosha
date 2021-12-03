namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class EtcAlimTalkTemplateRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public EtcAlimTalkTemplateRepository()
        {
        }

        public ETC_ALIMTALK_TEMPLATE GetItembyTempCd(string argTempCD)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SENDGAP, SENDDAY, SENDBF, SENDTIME  ");
            parameter.AppendSql("  FROM ADMIN.ETC_ALIMTALK_TEMPLATE   ");
            parameter.AppendSql(" WHERE TEMPCD = :TEMPCD                    ");

            parameter.Add("TEMPCD", argTempCD);

            return ExecuteReaderSingle<ETC_ALIMTALK_TEMPLATE>(parameter);
        }

        public ETC_ALIMTALK_TEMPLATE GetTitlebyTempCd(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TITLE                               ");
            parameter.AppendSql("  FROM ADMIN.ETC_ALIMTALK_TEMPLATE   ");
            parameter.AppendSql(" WHERE TEMPCD = :TEMPCD                    ");

            parameter.Add("TEMPCD", argCode);

            return ExecuteReaderSingle<ETC_ALIMTALK_TEMPLATE>(parameter);
        }

        public List<ETC_ALIMTALK_TEMPLATE> GetTempCdTitleRowId()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TEMPCD, TITLE, ROWID                ");
            parameter.AppendSql("  FROM ADMIN.ETC_ALIMTALK_TEMPLATE   ");
            parameter.AppendSql(" WHERE GBSTS = '3'                         ");
            parameter.AppendSql("   AND DELDATE IS NULL                     ");
            parameter.AppendSql(" ORDER BY TITLE                            ");

            return ExecuteReader<ETC_ALIMTALK_TEMPLATE>(parameter);
        }

        public ETC_ALIMTALK_TEMPLATE GetSendSmsbyTempCd(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SENDSMS                             ");
            parameter.AppendSql("  FROM ADMIN.ETC_ALIMTALK_TEMPLATE   ");
            parameter.AppendSql(" WHERE TEMPCD = :TEMPCD                    ");

            parameter.Add("TEMPCD", argCode);

            return ExecuteReaderSingle<ETC_ALIMTALK_TEMPLATE>(parameter);
        }

        public ETC_ALIMTALK_TEMPLATE GetMessagebyTempCd(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MESSAGE                             ");
            parameter.AppendSql("  FROM ADMIN.ETC_ALIMTALK_TEMPLATE   ");
            parameter.AppendSql(" WHERE TEMPCD = :TEMPCD                    ");

            parameter.Add("TEMPCD", argCode);

            return ExecuteReaderSingle<ETC_ALIMTALK_TEMPLATE>(parameter);
        }
    }
}
