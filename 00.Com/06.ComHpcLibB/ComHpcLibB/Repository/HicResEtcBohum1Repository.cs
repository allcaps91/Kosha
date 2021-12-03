namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicResEtcBohum1Repository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicResEtcBohum1Repository()
        {
        }

        public HIC_RES_ETC_BOHUM1 GetItembyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO                                                                                         ");
            parameter.AppendSql("     , TO_CHAR(a.PANJENGDATE,'YYYY-MM-DD') PANJENGDATE,a.PANJENGDRNO PANJENGDRNO1,a.SOGEN AS AddSo     ");
            parameter.AppendSql("     , TO_CHAR(a.GUNDATE,'YYYY-MM-DD') GUNDATE,a.GBPANJENG, a.GBPRINT, b.*                             ");
            parameter.AppendSql("  FROM ADMIN.HIC_RES_ETC a, ADMIN.HIC_RES_BOHUM1 b                                         ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                                ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                            ");
            parameter.AppendSql("   AND a.Gubun = '2'                                                                                   ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_RES_ETC_BOHUM1>(parameter);
        }
    }
}
