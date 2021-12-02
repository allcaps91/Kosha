namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class EndoJupmstResultRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public EndoJupmstResultRepository()
        {
        }

        public List<ENDO_JUPMST_RESULT> GetItembyPtNoJDate(string argPTNO, string argJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.RESULTDRCODE,a.SeqNo,a.Ptno,a.Gb_Clean,a.GbNew,a.GbJob                                        ");
            parameter.AppendSql("     , a.GBPRE_1,a.GBPRE_2,a.GBPRE_21,a.GBPRE_22,a.GBPRE_3,a.GBCON_1,a.GBCON_2,a.GBCON_21              ");
            parameter.AppendSql("     , a.GBCON_22,a.GBCON_3,a.GBCON_31,a.GBCON_32,a.GBCON_4,a.GBCON_41,a.GBCON_42,a.GBPRO_1,a.GBPRO_2  ");
            parameter.AppendSql("     , a.GBPRE_31, b.Remark1, b.Remark2, b.Remark3, b.Remark4, b.Remark5, b.Remark6                    ");
            parameter.AppendSql("     , b.Remark6_2,b.Remark6_3,b.Remark                                                                ");
            parameter.AppendSql("     , TO_CHAR(a.JDATE,'YYYY-MM-DD') JDATE                                                             ");
            parameter.AppendSql("     , TO_CHAR(a.RDATE,'YYYY-MM-DD HH24:MI') RDATE                                                     ");
            parameter.AppendSql("     , TO_CHAR(a.RESULTDATE,'YYYY-MM-DD HH24:MI') RESULTDATE                                           ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ENDO_JUPMST a,KOSMOS_OCS.ENDO_RESULT b                                               ");
            parameter.AppendSql(" WHERE a.PTNO = :PTNO                                                                                  ");
            parameter.AppendSql("   AND a.JDATE = TO_DATE(:JDATE, 'YYYY-MM-DD')                                                         ");
            parameter.AppendSql("   AND a.SEQNO = b.SEQNO(+)                                                                            ");
            parameter.AppendSql("   AND a.DeptCode IN ('HR','TO')                                                                       "); //건진,종검
            parameter.AppendSql("   AND a.GbSunap IN ('1','7')                                                                          ");

            parameter.Add("PTNO", argPTNO, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("JDATE", argJepDate);

            return ExecuteReader<ENDO_JUPMST_RESULT>(parameter);
        }
    }
}
