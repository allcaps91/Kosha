namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuSangdamDentalRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuSangdamDentalRepository()
        {
        }

        public List<HEA_JEPSU_SANGDAM_DENTAL> GetItembyDentRoom(string strFrDate, string strToDate, string dENT_ROOM, string strLtdCode, string strSName, string strJob, string strSort)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO, a.SNAME,TO_CHAR(a.SDATE,'YY-MM-DD') SDATE                          ");
            parameter.AppendSql("     , TO_CHAR(a.SDATE, 'YYYY-MM-DD') SDATE2                                       ");
            parameter.AppendSql("     , TRIM(a.GJJONG) || '.' || KOSMOS_PMPA.FC_HEA_GJJONG_NAME(a.GJJONG) GJJONG    ");
            if (strJob == "1")
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU        a                                          ");
                parameter.AppendSql("     , KOSMOS_PMPA.HEA_SANGDAM_WAIT b                                          ");
                parameter.AppendSql("     , KOSMOS_PMPA.HEA_DENTAL       c                                          ");
                parameter.AppendSql(" WHERE b.ENTTIME >= TO_DATE(:FRDATE,'YYYY-MM-DD')                              ");
                parameter.AppendSql("   AND b.ENTTIME <  TO_DATE(:TODATE,'YYYY-MM-DD')                              ");
            }
            else
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU  a                                                ");
                parameter.AppendSql("     , KOSMOS_PMPA.HEA_DENTAL c                                                ");
                parameter.AppendSql(" WHERE c.JEPDATE = TO_DATE(:FRDATE, 'YYYY-MM-DD')                              ");
            }
            if (strJob == "1")
            {
                parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                    ");
                parameter.AppendSql("   AND b.GUBUN = :GUBUN                                                        ");
            }
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO(+)                                                        ");
            if (strLtdCode != "")
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                    ");
            }
            if (strSName.Trim() != "")
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                     ");
            }
            if (strJob == "1")
            {
                parameter.AppendSql("   AND (c.GBSTS IS  NULL OR c.GBSTS = '')                                      ");
                parameter.AppendSql("   AND (c.PANJENGDRNO IS  NULL OR c.PANJENGDRNO = 0)                           ");
            }
            else
            {
                parameter.AppendSql("   AND c.GBSTS = 'Y'                                                           ");
                parameter.AppendSql("   AND c.PANJENGDRNO > 0                                                       ");
            }
            parameter.AppendSql("   AND a.GBSTS NOT IN ('D')                                                        ");
            parameter.AppendSql("   AND a.DELDATE IS  NULL                                                          ");
            if (strSort == "1")
            {
                parameter.AppendSql(" ORDER BY a.GJJONG, a.LTDCODE, a.SNAME                                         ");
            }
            else if (strSort == "2")
            {
                parameter.AppendSql(" ORDER BY a.SNAME, a.SDATE, a.GJJONG                                           ");
            }
            else if (strSort == "3")
            {
                parameter.AppendSql(" ORDER BY a.SDATE, a.SNAME, a.GJJONG                                           ");
            }
            else if (strSort == "4")
            {
                if (strJob == "1")
                {
                    parameter.AppendSql(" ORDER BY b.WAITNO                                                         ");
                }
            }

            parameter.Add("FRDATE", strFrDate);
            if (strJob == "1")
            {
                parameter.Add("TODATE", strToDate);
            }
            if (strLtdCode != "")
            {
                parameter.Add("LTDCODE", strLtdCode);
            }
            if (strJob == "1")
            {
                parameter.Add("GUBUN", dENT_ROOM, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (strSName.Trim() != "")
            {
                parameter.AddLikeStatement("SNAME", strSName);
            }

            return ExecuteReader<HEA_JEPSU_SANGDAM_DENTAL>(parameter);
        }
    }
}
