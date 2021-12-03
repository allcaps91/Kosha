namespace HC_Main.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using HC_Main.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaTicketRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaTicketRepository()
        {
        }

        public HEA_TICKET GetItemByTicketNo(long argTicketNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT GBSMS1,GBSMS2,BSAWON,SMS_SEND1,SMS_SEND2,ROWID AS RID  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_TICKET                                  ");
            parameter.AppendSql(" WHERE NO =:NO                                                 ");

            parameter.Add("NO", argTicketNo);

            return ExecuteReaderSingle<HEA_TICKET>(parameter);
        }

        public void UpDate(HEA_TICKET iHaTCK)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_TICKET          ");
            parameter.AppendSql("   SET SDATE     =:SDATE               ");
            parameter.AppendSql("      ,JEPDATE   =:JEPDATE             ");
            parameter.AppendSql("      ,JEPSUNO   =:JEPSUNO             ");
            parameter.AppendSql("      ,SNAME     =:SNAME               ");
            parameter.AppendSql("      ,SMS_SEND1 =:SMS_SEND1           ");
            parameter.AppendSql("      ,SMS_SEND2 =:SMS_SEND2           ");
            parameter.AppendSql(" WHERE ROWID =:RID                     ");

            parameter.Add("SDATE",      iHaTCK.SDATE);
            parameter.Add("JEPDATE",    iHaTCK.JEPDATE);
            parameter.Add("JEPSUNO",    iHaTCK.JEPSUNO);
            parameter.Add("SNAME",      iHaTCK.SNAME);
            parameter.Add("SMS_SEND1",  iHaTCK.SMS_SEND1);
            parameter.Add("SMS_SEND2",  iHaTCK.SMS_SEND2);

            ExecuteNonQuery(parameter);
        }
    }
}
