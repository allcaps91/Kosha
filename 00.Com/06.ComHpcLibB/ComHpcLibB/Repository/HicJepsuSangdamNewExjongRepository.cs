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
    public class HicJepsuSangdamNewExjongRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuSangdamNewExjongRepository()
        {
        }

        public List<HIC_JEPSU_SANGDAM_NEW_EXJONG> GetItembyJepDate(string strFrDate, string strToDate, int nGBn, long fnTab, string strSName, long nLtdCode, string strGuBun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT b.WRTNO, a.SNAME, TO_CHAR(a.JEPDATE,'YYYY-MM-DD') JEPDATE, b.GjJong,a.UCodes,a.SangDamDrno  ");
            if (fnTab == 4)
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_SANGDAM_NEW b, KOSMOS_PMPA.HIC_EXJONG c        ");
            }
            else
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_SANGDAM_WAIT b, KOSMOS_PMPA.HIC_EXJONG c       ");
            }
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'yyyy-mm-dd')                                                 ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'yyyy-mm-dd')                                                 ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                        ");
            parameter.AppendSql("   AND a.GJJONG = c.CODE(+)                                                                        ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                           ");
            parameter.AppendSql("   AND c.GBSANGDAM = 'Y'                                                                           ");
            switch (nGBn)
            {
                case 0:
                    break;
                case 1:
                    parameter.AppendSql("   AND a.GJJONG NOT IN ('56','50','51')                                                    ");
                    break;
                case 2:
                    parameter.AppendSql("   AND a.GJJONG = '56'                                                                     ");
                    break;
                case 3:
                    parameter.AppendSql("   AND a.GJJONG IN ('50','51')                                                             ");
                    break;
                default:
                    break;
            }
            if (fnTab != 4)
            {
                parameter.AppendSql("   AND b.GUBUN = :GUBUN                                                                        ");
            }
            if (strSName != "")
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                                     ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                    ");
            }
            if (fnTab == 4)
            {
                parameter.AppendSql(" ORDER BY a.SName, a.WRTNO, a.GjJong                                                           ");
            }
            else
            {
                parameter.AppendSql(" ORDER BY b.WaitNo,a.SName,a.WRTNO,a.GjJong                                                    ");
            }

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (strSName != "")
            {
                parameter.Add("SNAME", strSName);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            if (fnTab != 4)
            {
                parameter.Add("GUBUN", strGuBun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_JEPSU_SANGDAM_NEW_EXJONG>(parameter);
        }

        public List<HIC_JEPSU_SANGDAM_NEW_EXJONG> GetItembyJepDateGjJong(string strFrDate, string strToDate, string strChul, string strGbn, string strJob, string strRoom, List<string> strDrList, List<string> strGubun, string strSName, string strJong, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            //parameter.AppendSql("SELECT a.WRTNO, a.SNAME, a.Pano, a.GjJong, a.UCodes, a.SangDamDrno                                 ");
            parameter.AppendSql("SELECT a.WRTNO, a.SNAME, a.Pano, FC_HIC_GJJONG_NAME(a.GjJong, a.Ucodes) GjJong, a.UCodes, a.SangDamDrno                                 ");
            parameter.AppendSql("     , TO_CHAR(a.JEPDATE,'YYYY-MM-DD') JEPDATE, GJYear                                             ");
            if (strRoom == "4" || strJob == "1")
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_SANGDAM_NEW b, KOSMOS_PMPA.HIC_EXJONG c        ");
            }
            else
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_SANGDAM_WAIT b, KOSMOS_PMPA.HIC_EXJONG c       ");
            }
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'yyyy-mm-dd')                                                 ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'yyyy-mm-dd')                                                 ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                        ");
            parameter.AppendSql("   AND a.GJJONG = c.CODE(+)                                                                        ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                           ");
            parameter.AppendSql("   AND c.GBSANGDAM = 'Y'                                                                           ");
            parameter.AppendSql("   AND a.SName <> '{수검자호출}'                                                                   ");
            switch (strGbn)
            {
                case "0":
                    break;
                case "1":
                    parameter.AppendSql("   AND a.GJJONG NOT IN ('56','59','50','51')                                               ");
                    break;
                case "2":
                    parameter.AppendSql("   AND a.GJJONG IN ('56','59')                                                             ");
                    break;
                case "3":
                    parameter.AppendSql("   AND a.GJJONG IN ('50','51')                                                             ");
                    break;
                default:
                    break;
            }
            if (strChul == "0") //내원
            {
                parameter.AppendSql("   AND a.GBCHUL = 'N'                                                                          ");
            }
            else if (strChul == "1")             //출장
            {
                parameter.AppendSql("   AND a.GBCHUL = 'Y'                                                                          ");
            }
            if (strRoom == "1" || strJob == "1")
            {
                if (!strSName.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                                 ");
                }
                else
                {
                    if (strRoom == "0")
                    {
                        if (strJob == "1")
                        {
                            if (strDrList.Count > 0 && !strDrList[0].IsNullOrEmpty())
                            {
                                parameter.AppendSql("   AND a.SangdamDrno IN (:SANGDAMDRNO)                                         ");
                            }
                        }
                    }
                }
            }
            else
            {
                if (strGubun.Count > 0 && !strGubun[0].IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND b.GUBUN IN (:GUBUN)                                                                 ");
                }
                if (!strSName.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                                 ");
                }
            }
            if (strJong != "**" && !strJong.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.GJJONG = :GJJONG                                                                      ");
            }

            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE LIKE :LTDCODE                                                                 ");
            }
            if (strRoom == "1" || strJob == "1")
            {
                parameter.AppendSql(" ORDER BY a.SName,a.WRTNO,a.GjJong                                                             ");
            }
            else
            {
                parameter.AppendSql(" ORDER BY b.WaitNo,a.SName,a.WRTNO,a.GjJong                                                    ");
            }

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            if (!strSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", strSName);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            if (strJong != "**" && !strJong.IsNullOrEmpty())
            {
                parameter.Add("GJJONG", strJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (strRoom == "1" || strJob == "1")
            {
                if (strSName.IsNullOrEmpty())
                { 
                    if (strRoom == "0")
                    {
                        if (strJob == "1")
                        {
                            if (strDrList.Count > 0 && !strDrList[0].IsNullOrEmpty())
                            {
                                parameter.AddInStatement("SANGDAMDRNO", strDrList);
                            }
                        }
                    }
                }
            }

            if (strRoom != "1" && strJob != "1")
            {
                if (strGubun.Count > 0 && !strGubun[0].IsNullOrEmpty())
                {
                    parameter.AddInStatement("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                }
            }

            return ExecuteReader<HIC_JEPSU_SANGDAM_NEW_EXJONG>(parameter);
        }
    }
}
