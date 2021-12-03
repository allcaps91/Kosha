namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuLtdExjongRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuLtdExjongRepository()
        {
        }

        public List<HIC_JEPSU_LTD_EXJONG> GetItembyJepDate(string strFDate, string strTDate, string strJong, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.WRTNO,a.SName,a.LtdCode           ");
            parameter.AppendSql("     , a.GjJong,a.GjChasu,a.GbSTS,a.UCodes,a.SExams,a.GbMunjin1,a.GbMunjin2        ");
            parameter.AppendSql("     , a.GbMunjin3,a.GbDental,b.Name LtdName,c.Name ExName                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_LTD b, KOSMOS_PMPA.HIC_EXJONG c    ");
            parameter.AppendSql(" WHERE a.JEPDATE > =TO_DATE(:FRDATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("   AND a.JEPDATE < =TO_DATE(:TODATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                           ");
            if (strJong != "**" && !strJong.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.GJJONG = :GJJONG                                                      ");
            }
            else
            {
                //운전면허,학생신검,간염검사,간염접종,측정,대행,종검은 제외
                parameter.AppendSql("   AND a.GjJong NOT IN ('55','56','57','58','81','82','83')                    ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                    ");
            }            
            parameter.AppendSql("   AND a.LtdCode=b.Code(+)                                                         ");
            parameter.AppendSql("   AND a.GjJong = c.Code(+)                                                        ");
            parameter.AppendSql(" ORDER BY a.JepDate, a.WRTNO                                                       ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            if (strJong != "**")
            {
                parameter.Add("GJJONG", strJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_LTD_EXJONG>(parameter);
        }

        public List<HIC_JEPSU_LTD_EXJONG> GetItembyGjJong(string strGjJong, long nLtdCode, string strTongbo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.WRTNO,a.SName,a.LtdCode                       ");
            parameter.AppendSql("     , a.GjJong,a.GjChasu,a.GbSTS,a.UCodes,a.SExams,a.GbMunjin1,a.GbMunjin2                    ");
            parameter.AppendSql("     , TO_CHAR(a.ErTongbo,'MM/DD')  ErTongbo                                                   ");
            parameter.AppendSql("     , a.GbMunjin3,a.GbDental,b.Name LtdName,c.Name ExName                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_LTD b, KOSMOS_PMPA.HIC_EXJONG c                ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TRUNC(SYSDATE-200)                                                         ");
            //parameter.AppendSql("   AND a.ErFlag = 'Y'                                                                          ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                       ");
            if (strGjJong != "**" && strGjJong != "")
            {
                parameter.AppendSql("   AND a.GJJONG = :GJJONG                                                                  ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                ");
            }
            parameter.AppendSql("   AND (a.WRTNO IN (SELECT WRTNO FROM HIC_RES_BOHUM1  WHERE PanjengDate>=TRUNC(SYSDATE-200))   ");
            parameter.AppendSql("    OR  a.WRTNO IN (SELECT WRTNO FROM HIC_RES_SPECIAL WHERE PanjengDate>=TRUNC(SYSDATE-200)))  ");
            parameter.AppendSql("   AND a.LtdCode = b.Code(+)                                                                   ");
            parameter.AppendSql("   AND a.GjJong  = c.Code(+)                                                                   ");
            if (strTongbo == "2")
            {
                parameter.AppendSql("   AND a.ErTongbo IS NOT NULL                                                              ");
            }
            else if (strTongbo == "3")
            {
                parameter.AppendSql("   AND a.ErTongbo IS NULL                                                                  ");
            }
            parameter.AppendSql(" ORDER BY a.JepDate,a.WRTNO ");

            if (strGjJong != "**" && strGjJong != "")
            {
                parameter.Add("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_LTD_EXJONG>(parameter);
        }
    }
}
