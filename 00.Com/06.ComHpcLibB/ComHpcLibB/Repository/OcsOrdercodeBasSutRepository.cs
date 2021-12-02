namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class OcsOrdercodeBasSutRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public OcsOrdercodeBasSutRepository()
        {
        }

        public List<OCS_ORDERCODE_BAS_SUT> GetItemAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.ORDERCODE, a.ORDERNAME, a.ORDERNAMES, a.GBINPUT, a.DISPSPACE  ");
            parameter.AppendSql("     , a.GBINFO, a.GBBOTH, a.BUN, a.NEXTCODE, a.SUCODE, a.GBDOSAGE     ");
            parameter.AppendSql("     , a.SPECCODE, a.SLIPNO, a.GBIMIV, a.SUBRATE                       ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_ORDERCODE a                                      ");
            parameter.AppendSql("     , KOSMOS_PMPA.BAS_SUT      b                                      ");
            parameter.AppendSql(" WHERE a.SLIPNO = '0042'                                               ");
            parameter.AppendSql("   AND a.SEQNO <> 0                                                    ");    
            parameter.AppendSql("   AND (a.GBSUB != '1' OR a.GBSUB IS NULL)                             ");
            parameter.AppendSql("   AND RTRIM(a.SENDDEPT) IS NULL                                       ");
            parameter.AppendSql("   AND a.SUCODE = b.SUCODE(+)                                          ");
            parameter.AppendSql("   AND (b.SUGBJ <> '2' OR b.SUGBJ IS NULL)                             ");
            parameter.AppendSql(" ORDER BY a.SEQNO                                                      ");
            
            return ExecuteReader<OCS_ORDERCODE_BAS_SUT>(parameter);
        }

        public List<OCS_ORDERCODE_BAS_SUT> GetItemCode()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.ORDERCODE, a.ORDERNAME                ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_ORDERCODE a              ");
            parameter.AppendSql("     , KOSMOS_PMPA.BAS_SUT      b              ");
            parameter.AppendSql(" WHERE a.SLIPNO = '0042'                       ");
            parameter.AppendSql("   AND a.SEQNO <> 0                            ");
            parameter.AppendSql("   AND (a.GBSUB != '1' OR a.GBSUB IS NULL)     ");
            parameter.AppendSql("   AND RTRIM(a.SENDDEPT) IS NULL               ");
            parameter.AppendSql("   AND a.SUCODE = b.SUCODE(+)                  ");
            parameter.AppendSql("   AND (b.SUGBJ <> '2' OR b.SUGBJ IS NULL)     ");
            parameter.AppendSql(" ORDER BY a.SEQNO                              ");

            return ExecuteReader<OCS_ORDERCODE_BAS_SUT>(parameter);
        }
    }
}
