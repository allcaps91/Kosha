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
    public class HicJepsuResSpecialRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResSpecialRepository()
        {
        }

        public List<HIC_JEPSU_RES_SPECIAL> GetItembyJepDateLtdCodeGjYear(string fstrFDate, string fstrTDate, string fstrLtdCode, string strGjYear, string strBangi, string strJob)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.Pano,a.SName,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate      ");
            parameter.AppendSql("     , a.GjJong,a.GjChasu,a.UCodes,a.GjYear,a.GjBangi,a.Sex                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RES_SPECIAL b              ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')                          ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE,'YYYY-MM-DD')                          ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                   ");
            parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                ");
            parameter.AppendSql("   AND a.UCodes IS NOT NULL                                                "); //취급물질이 있는 사람만
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                                  ");
            if (strBangi != "")
            {
                parameter.AppendSql("   AND a.GJBANGI = :GJBANGI                                            ");
            }
            if (strJob == "1") //특수
            {
                parameter.AppendSql("   AND a.Gjjong IN ('11','12','14','23','41')                          ");
            }
            else if (strJob == "2") //채용
            {
                parameter.AppendSql("   AND a.Gjjong IN ('22','24','30')                                    ");
            }
            else
            {
                parameter.AppendSql("   AND a.Gjjong  IN ('69')                                             ");
            }
            //특수검진인지 점검
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                ");
            parameter.AppendSql("   AND b.PANJENGDRNO IS NOT NULL                                           "); //판정완료된것만 읽음
            parameter.AppendSql(" ORDER BY a.SName,a.Pano,a.JepDate,a.WRTNO                                 ");

            parameter.Add("FRDATE", fstrFDate);
            parameter.Add("TODATE", fstrTDate);
            parameter.Add("LTDCODE", fstrLtdCode);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (strBangi != "")
            {
                parameter.Add("GJBANGI", strBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_JEPSU_RES_SPECIAL>(parameter);
        }

        public HIC_JEPSU_RES_SPECIAL GetItembyJepDatePaNoGjYear(string strDate, long pANO, string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JEPDATE             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RES_SPECIAL b      ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO                                           ");
            parameter.AppendSql("   AND a.JEPDATE > TO_DATE(:JEPDATE, 'YYYY-MM-DD')                 ");
            parameter.AppendSql("   AND a.PANO = :PANO                                              ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                           ");
            parameter.AppendSql("   AND a.GJJONG IN ('16','17','19','28','44','45')                 ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                          ");
            parameter.AppendSql(" ORDER BY b.WRTNO                                                  ");

            parameter.Add("JEPDATE", strDate);
            parameter.Add("PANO", pANO);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_JEPSU_RES_SPECIAL>(parameter);
        }

        public HIC_JEPSU_RES_SPECIAL GetWrtNoJepDatebyJepDatePaNoGjYear(string strDate, long pANO, string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RES_SPECIAL b              ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO                                                   ");
            parameter.AppendSql("   AND a.JEPDATE > TO_DATE(:JEPDATE,'YYYY-MM-DD')                          ");
            parameter.AppendSql("   AND a.PANO = :PANO                                                      ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                   ");
            parameter.AppendSql("   AND a.GJJONG IN ('16','17','19','28','44','45','51','50')               ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                                  ");
            parameter.AppendSql(" ORDER BY b.WRTNO                                                          ");

            parameter.Add("JEPDATE", strDate);
            parameter.Add("PANO", pANO);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_JEPSU_RES_SPECIAL>(parameter);
        }

        public List<HIC_JEPSU_RES_SPECIAL> GetItembyJepDateGjYear(string strFrDate, string strToDate, string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.LtdCode,COUNT(*) CNT                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RES_SPECIAL b          ");
            parameter.AppendSql(" WHERE a.JepDate>=TO_DATE(:FRDATE, 'YYYY-MM-DD')                       ");
            parameter.AppendSql("   AND a.JepDate<=TO_DATE(:TODATE, 'YYYY-MM-DD')                       ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                               ");
            parameter.AppendSql("   AND a.LtdCode IS NOT NULL                                           ");
            parameter.AppendSql("   AND a.UCodes IS NOT NULL                                            ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                              ");
            parameter.AppendSql("   AND a.Gjjong IN ('11','12','14','23','41','42')                     ");
            parameter.AppendSql("   AND a.PanjengDrno NOT IN (61439,71797,86651)                        "); //주철효,이주령,정해진과장 제외
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                              ");
            parameter.AppendSql("   AND b.PanjengDrno IS NOT NULL                                       "); //판정완료 
            parameter.AppendSql(" GROUP BY a.LtdCode                                                    ");
            parameter.AppendSql(" ORDER BY a.LtdCode                                                    ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GJYEAR", strGjYear);

            return ExecuteReader<HIC_JEPSU_RES_SPECIAL>(parameter);
        }

        public List<HIC_JEPSU_RES_SPECIAL> GetItemsbyJepDate(string strFrDate, string strToDate, string strJob, long nLtdCode, string strJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.SName,TO_CHAR(a.JepDate,'YY-MM-DD') JepDate,a.GjJong,a.Ltdcode,b.GbOHMS   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a , KOSMOS_PMPA.HIC_RES_SPECIAL b                             ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                   ");
            parameter.AppendSql("   AND a.Wrtno = b.Wrtno(+)                                                                ");
            if (strJob == "1")
            {
                parameter.AppendSql("   AND (b.GbOHMS = 'N' OR b.GbOHMS IS NULL)                                            ");
            }
            else
            {
                parameter.AppendSql("   AND b.GbOHMS = 'Y'                                                                  ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                            ");
            }
            if (strJong != "**" && !strJong.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.GJJONG = :GJJONG                                                              ");
            }
            parameter.AppendSql(" ORDER BY a.SName,a.WRTNO,a.Jepdate                                                        ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            if (strJong != "**" && !strJong.IsNullOrEmpty())
            {
                parameter.Add("GJJONG", strJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_JEPSU_RES_SPECIAL>(parameter);
        }

        public List<HIC_JEPSU_RES_SPECIAL> GetItembyPaNoJepDateWrtNo(long argPano, string argJepDate, long fnWrtno1, long fnWrtno2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RES_SPECIAL b          ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                                                  ");
            parameter.AppendSql("   AND a.JEPDATE < TO_DATE(:JEPDATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND a.WRTNO NOT IN (:WRTNO1, :WRTNO2)                               "); //금년자료 제외
            parameter.AppendSql("   AND a.GbSTS <> 'D'                                                  "); //접수취소(삭제)는 제외
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                              ");
            parameter.AppendSql("ORDER BY a.JepDate DESC                                                ");

            parameter.Add("PANO", argPano);
            parameter.Add("JEPDATE", argJepDate);
            parameter.Add("WRTNO1", fnWrtno1);
            parameter.Add("WRTNO2", fnWrtno2);

            return ExecuteReader<HIC_JEPSU_RES_SPECIAL>(parameter);
        }

        public List<HIC_JEPSU_RES_SPECIAL> GetItembyJepDateGjYearPanDrNo(string strFrDate, string strToDate, string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.PanjengDrno,a.LtdCode,COUNT(*) CNT                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RES_SPECIAL b          ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                               ");
            parameter.AppendSql("   AND a.LtdCode IS NOT NULL                                           ");
            parameter.AppendSql("   AND a.UCodes IS NOT NULL                                            ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                              ");
            parameter.AppendSql("   AND a.Gjjong IN ('11','12','14','23','41','42')                     ");
            parameter.AppendSql("   AND a.PANJENGDRNO NOT IN (61439, 71797, 86651)                      "); //주철효,이주령,정해진과장 제외
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                              ");
            parameter.AppendSql("   AND b.PanjengDrno IS NOT NULL                                       "); //판정완료
            parameter.AppendSql(" GROUP BY a.PanjengDrno,a.LtdCode                                      ");
            parameter.AppendSql(" ORDER BY a.PanjengDrno,a.LtdCode                                      ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU_RES_SPECIAL>(parameter);
        }

        public List<HIC_JEPSU_RES_SPECIAL> GetItembyMunjinNameLtdCodeCount(string strFrate, string strToDate, string strGjYear, string strGjBangi, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT c.Name,a.LtdCode,COUNT(*) CNT                                   ");
            parameter.AppendSql("     , MIN(TO_CHAR(a.JepDate,'YYYY-MM-DD')) MINDATE                    ");
            parameter.AppendSql("     , MAX(TO_CHAR(a.JepDate,'YYYY-MM-DD')) MAXDATE                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU    a                                      ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_X_MUNJIN b                                      ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_LTD      c                                      ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                               ");
            parameter.AppendSql("   AND a.LtdCode IS NOT NULL                                           ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                              ");
            if (strGjBangi != "")
            {
                parameter.AppendSql("   AND a.GJBANGI = :GJBANGI                                        ");
            }
            parameter.AppendSql("   AND a.Gjjong  IN ('50','51')                                        ");
            //특수검진인지 점검
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                              ");
            //parameter.AppendSql("   AND b.PanjengDrno IS NOT NULL                                       ");
            //회사명
            parameter.AppendSql("   AND a.LtdCode = c.Code(+)                                           ");
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                        ");
            }
            parameter.AppendSql(" GROUP BY c.Name, a.LtdCode                                            ");
            parameter.AppendSql(" ORDER BY c.Name, a.LtdCode                                            ");

            parameter.Add("FRDATE", strFrate);
            parameter.Add("TODATE", strToDate);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (strGjBangi != "")
            {
                parameter.Add("GJBANGI", strGjBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_JEPSU_RES_SPECIAL>(parameter);
        }

        public List<HIC_JEPSU_RES_SPECIAL> GetItembyMunjinJepDateLtdCodeGjYear(string fstrFDate, string fstrTDate, string fstrLtdCode, string strGjYear, string strBangi, string strJob)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.Pano,a.SName,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate      ");
            parameter.AppendSql("     , a.GjJong, a.GjChasu, a.UCodes, a.GjYear, a.GjBangi, a.Sex, a.Age    ");
            parameter.AppendSql("     , b.Sogen, b.Panjeng                                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_X_MUNJIN b                 ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')                          ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE,'YYYY-MM-DD')                          ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                   ");
            parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                ");
            //parameter.AppendSql("   AND a.UCodes IS NOT NULL                                                "); //취급물질이 있는 사람만
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                                  ");
            if (strBangi != "")
            {
                parameter.AppendSql("   AND a.GJBANGI = :GJBANGI                                            ");
            }
            parameter.AppendSql("   AND a.Gjjong IN ('50', '51')                                            ");            
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                ");
            //parameter.AppendSql("   AND b.PANJENGDRNO IS NOT NULL                                           "); //판정완료된것만 읽음
            parameter.AppendSql(" ORDER BY a.SName,a.Pano,a.JepDate,a.WRTNO                                 ");

            parameter.Add("FRDATE", fstrFDate);
            parameter.Add("TODATE", fstrTDate);
            parameter.Add("LTDCODE", fstrLtdCode);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (strBangi != "")
            {
                parameter.Add("GJBANGI", strBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_JEPSU_RES_SPECIAL>(parameter);
        }


        public List<HIC_JEPSU_RES_SPECIAL> GetItembyPanoLtdYearChasuGjjong(long artPano, string argLtdcode, string argGjyear, string argChasu, string argGjjong, string argOpt1)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.WRTNO,a.GjJong,a.GjChasu,b.GbSpc                 ");
            parameter.AppendSql(" ,TO_CHAR(a.TongboDate, 'YYYY-MM-DD') TongboDate, TO_CHAR(b.PanjengDate, 'YYYY-MM-DD') PanjengDate ");
            parameter.AppendSql("  FROM HIC_JEPSU a,HIC_RES_SPECIAL b ");
            parameter.AppendSql(" WHERE a.Pano = :PANO");

            if (!argLtdcode.IsNullOrEmpty() )
            {
                parameter.AppendSql("  AND a.LtdCode = :LTDCODE ");
            }
            parameter.AppendSql("  AND a.GjYear= :GJYEAR ");
            parameter.AppendSql("  AND a.Gbsts NOT IN ('D') ");
            parameter.AppendSql("  AND a.Deldate IS NULL ");

            if (argOpt1 == "일특")
            {
                if( argChasu == "2")
                {

                    switch (argGjjong)
                    {
                        case "11":
                        case "16":
                            parameter.AppendSql("   AND a.GjJong IN ('11','16') ");
                            break;
                        case "12":
                        case "17":
                            parameter.AppendSql("   AND a.GjJong IN ('12','17') ");
                            break;
                        case "14":
                        case "19":
                            parameter.AppendSql("   AND a.GjJong IN ('14','19') ");
                            break;
                        case "41":
                        case "44":
                            parameter.AppendSql("   AND a.GjJong IN ('41','44') ");
                            break;
                        case "42":
                        case "45":
                            parameter.AppendSql("   AND a.GjJong IN ('42','45') ");
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    parameter.AppendSql("   AND a.GjJong = :GJJONG          ");
                }
            }
            else if (argOpt1 == "특수")
            {
                if (argChasu == "2")
                {

                    switch (argGjjong)
                    {
                        case "21":
                        case "27":
                            parameter.AppendSql("   AND a.GjJong IN ('21','27') ");
                            break;
                        case "22":
                        case "29":
                            parameter.AppendSql("   AND a.GjJong IN ('22','29')  ");
                            break;
                        case "23":
                        case "28":
                            parameter.AppendSql("   AND a.GjJong IN ('23','28') ");
                            break;
                        case "24":
                        case "33":
                            parameter.AppendSql("   AND a.GjJong IN ('22','24','33')"   );
                            break;
                        default:
                            parameter.AppendSql("   AND a.GjJong NOT IN ('11','12','13','14','16','17','18','19','41','42','43','43','44','45','46','55','51','50') ");
                            break;
                    }
                }
                else
                {
                    parameter.AppendSql("   AND a.GjJong = :GJJONG          ");
                }
            }
            parameter.AppendSql("    AND a.WRTNO=b.WRTNO(+)         ");

            if (argChasu =="2")
            {
                parameter.AppendSql("    ORDER BY a.JepDate         ");
            }
            else
            {
                parameter.AppendSql("    ORDER BY a.JepDate DESC         ");
            }


            parameter.Add("PANO", artPano);
            if (!argLtdcode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", argLtdcode);
            }
            parameter.Add("GJYEAR", argGjyear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("CHASU", argChasu);
            parameter.Add("GJJONG", argGjjong);

            return ExecuteReader<HIC_JEPSU_RES_SPECIAL>(parameter);
        }
    }
}
