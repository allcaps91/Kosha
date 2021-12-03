namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class EtcAlimTalkResultRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public EtcAlimTalkResultRepository()
        {
        }

        public ETC_ALIMTALK_RESULT GetBigobyGubun(string argGubun, string argResCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT BIGO                                ");
            parameter.AppendSql("  FROM ADMIN.ETC_ALIMTALK_RESULT     ");
            parameter.AppendSql(" WHERE 1 = 1                               ");
            if (argGubun == "알림톡")
            {
                parameter.AppendSql("   AND GUBUN = '2'                     ");
            }
            else if (argGubun == "SMS")
            {
                parameter.AppendSql("   AND GUBUN = '3'                     ");
            }
            parameter.AppendSql("   AND CODE = :CODE                        ");

            parameter.Add("CODE", argResCode);

            return ExecuteReaderSingle<ETC_ALIMTALK_RESULT>(parameter);
        }
    }
}
