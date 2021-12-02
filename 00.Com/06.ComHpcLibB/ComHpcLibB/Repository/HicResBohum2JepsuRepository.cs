namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicResBohum2JepsuRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicResBohum2JepsuRepository()
        {
        }

        public HIC_RES_BOHUM2_JEPSU GetItembyLtdCodeJepDate(long nPaNo, string fstrJepDate, string strYear, string strBangi)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT b.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RES_BOHUM2 b       ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                                              ");
            parameter.AppendSql("   AND a.JEPDATE > TO_DATE(:JEPDATE, 'YYYY-MM-DD')                 ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                           ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                          ");
            if (strBangi != "")
            {
                parameter.AppendSql("   AND a.GJBANGI = :GJBANGI                                    ");
            }
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                        ");
            parameter.AppendSql(" ORDER BY b.WRTNO                                                  ");

            parameter.Add("PANO", nPaNo);
            parameter.Add("JEPDATE", fstrJepDate);
            parameter.Add("GJYEAR", strYear, Oracle.DataAccess.Client.OracleDbType.Char);
            if (strBangi != "")
            {
                parameter.Add("GJBANGI", strBangi, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReaderSingle<HIC_RES_BOHUM2_JEPSU>(parameter);
        }
    }
}
