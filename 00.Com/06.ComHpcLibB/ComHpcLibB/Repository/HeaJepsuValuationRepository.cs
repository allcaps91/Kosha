namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuValuationRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuValuationRepository()
        {
        }

        public List<HEA_JEPSU_VALUATION> GetItembyLtdCode(string strGubun, string strFrDate, string strToDate, string strLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO, a.SNAME,TO_CHAR(a.JEPDATE,'YY-MM-DD') JEPDATE  ");
            parameter.AppendSql("     , a.GJJONG, a.SEX, a.AGE                                  ");
            parameter.AppendSql("  FROM ADMIN.HEA_JEPSU     a                             ");
            parameter.AppendSql("     , ADMIN.HEA_VALUATION b                             ");
            parameter.AppendSql("     , ADMIN.HIC_PATIENT   c                             ");
            parameter.AppendSql(" WHERE a.SDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')               ");
            parameter.AppendSql("   AND a.SDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')               ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                       ");
            parameter.AppendSql("   AND a.GbSTS NOT IN ('0', 'D')                               ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                    ");
            parameter.AppendSql("   AND a.PANO = c.PANO                                         ");
            if (strLtdCode.Trim() != "")
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                ");
            }
            if (strGubun == "1")
            {
                parameter.AppendSql("   AND (b.GBSTS IS NULL OR b.GBSTS <> 'Y')                 ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql("   AND b.GBSTS = 'Y'                                       ");
            }
            parameter.AppendSql(" ORDER BY c.SNAME, c.BUSENAME, c.SABUN, c.JUMIN                ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (strLtdCode.Trim() != "")
            {
                parameter.Add("LTDCODE", strLtdCode);
            }

            return ExecuteReader<HEA_JEPSU_VALUATION>(parameter);
        }
    }
}
