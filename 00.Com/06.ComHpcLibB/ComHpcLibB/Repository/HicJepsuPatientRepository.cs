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
    public class HicJepsuPatientRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuPatientRepository()
        {
        }

        public HIC_JEPSU_PATIENT GetItembyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO, a.Pano, a.SName, a.Sex, a.Age, TO_CHAR(a.IpsaDate,'YYYY-MM-DD') IpsaDate   ");
            parameter.AppendSql("     , a.Ptno, a.XrayNo, a.GjJong, a.GjYear, a.GjChasu, a.UCodes, b.Jumin2                 ");
            parameter.AppendSql("     , TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate, b.GongJeng, b.BuseName                     ");
            parameter.AppendSql("     , a.Gbn, a.Class, a.GbChul, a.GBN, a.Class, a.Ban, a.Bun, a.Ltdcode                   ");
            parameter.AppendSql("     , B.JUMIN, TO_CHAR(a.SANGDAMDATE,'YYYY-MM-DD') SANGDAMDATE, a.GBDENTAL, a.SANGDAMDRNO ");
            parameter.AppendSql("     , TO_CHAR(a.TONGBODATE,'YYYY-MM-DD') TONGBODATE                                       ");
            parameter.AppendSql("     , TO_CHAR(a.PANJENGDATE,'YYYY-MM-DD') PANJENGDATE                                     ");
            parameter.AppendSql("     , TO_CHAR(a.WEBPRINTSEND, 'YYYY-MM-DD') WEBPRINTSEND, a.MAILCODE, a.PANJENGDRNO       ");
            parameter.AppendSql("     , a.WEBPRINTREQ, a.GBCHK1, a.GBCHK2, a.GBCHK3, a.GBJUSO, b.HPHONE, a.JUSO1, a.JUSO2   ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b                                  ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                    ");
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                                                                  ");
            parameter.AppendSql(" ORDER BY a.Wrtno DESC                                                                     ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_JEPSU_PATIENT>(parameter);
        }

        public HIC_JEPSU_PATIENT GetItemByWrtnoJepdate(long fnWRTNO, string argJepdate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT B.PANO, B.PTNO, B.SNAME, B.JUSO1||'  '||B.JUBO2 AS JUSO,  B.JUMIN                   ");
            parameter.AppendSql(" , B.LTDCODE, B.AGE, DECODE(B.SEX,'M','남','여') SEX ,B.TEL, B.JEPDATE, B.JUMIN2           ");
            parameter.AppendSql(" FROM  HIC_JEPSU A , HIC_PATIENT B                                                         ");
            parameter.AppendSql(" WHERE 1 = 1                                                                               ");
            parameter.AppendSql(" AND B.JepDate>=TO_DATE(:JEPDATE,'YYYY-MM-DD')                                             ");
            parameter.AppendSql(" AND B..Pano = B.Pano(+)                                                                   ");
            parameter.AppendSql(" AND B.WRTNO = :WRTNO                                                                      ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("JEPDATE", argJepdate);

            return ExecuteReaderSingle<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyJepDateGjYear(string strFrDate, string strToDate, string strGjYear, string strBangi, string strLtdCode, string strTongbo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,a.SName,b.Jumin2,a.UCodes,TO_CHAR(a.JepDate,'YYYY/MM/DD') JepDate       ");
            parameter.AppendSql("     , a.SECOND_EXAMS,a.SECOND_Sayu                                                    ");
            parameter.AppendSql("     , TO_CHAR(a.SECOND_TONGBO,'YYYY/MM/DD') SECOND_TONGBO                             ");
            parameter.AppendSql("     , TO_CHAR(a.SECOND_DATE,'YYYY/MM/DD')   SECOND_DATE                               ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b                              ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE,'YYYY-MM-DD')                                      ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                               ");
            if (!strGjYear.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.GJYEAR  = :GJYEAR                                                         ");
            }
            if (!strBangi.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.GJBANGI = :GJBANGI                                                        ");
            }
            parameter.AppendSql("   AND a.SECOND_FLAG = 'Y'                                                             "); //2차대상자
            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                        ");
            }
            if (strTongbo == "1")
            {
                parameter.AppendSql("   AND(a.SECOND_Tongbo IS NOT NULL OR a.SECOND_DATE   IS NOT NULL)                 "); //통보자만
            }
            else if (strTongbo == "2")
            {
                parameter.AppendSql("   AND a.SECOND_Tongbo IS NULL                                                     ");//통보자제외
                parameter.AppendSql("   AND a.SECOND_DATE   IS NULL                                                     ");//2차검진자
            }
            parameter.AppendSql("   AND a.GjJong IN ('11','12','14','23')                                               "); //사업장1차,특수검진
            parameter.AppendSql("   AND a.Pano=b.Pano(+)                                                                ");
            parameter.AppendSql(" ORDER BY a.SName,a.JepDate                                                            ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strGjYear.IsNullOrEmpty())
            {
                parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (!strBangi.IsNullOrEmpty())
            {
                parameter.Add("GJBNAGI", strBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", strLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyJepDateSDateLtdCode(string strGubun, string strFrDate, string strToDate, long nLtdCode, string strToDate1, string sGjJong)
        {
            MParameter parameter = CreateParameter();

            if (strGubun == "0")
            {
                parameter.AppendSql("SELECT a.Pano as Pano1,c.Pano as Pano2, a.Wrtno as Wrtno1,c.Wrtno as Wrtno2, a.SName,d.Jumin2                                              ");
                parameter.AppendSql("     , TO_CHAR(a.SDate,'YYYY-MM-DD') SDate ,TO_CHAR(c.JepDate,'YYYY-MM-DD') JepDate ,c.GjJong, c.LtdCode,c.Mirno1, c.Mirno3, '1' as Gubun  ");
                parameter.AppendSql("  FROM ADMIN.HEA_JEPSU a, ADMIN.HIC_JEPSU c, ADMIN.HIC_PATIENT d                                                         ");
                parameter.AppendSql(" WHERE a.Pano = d.Pano                                                                                                                     ");
                parameter.AppendSql("   AND c.Pano = d.Pano                                                                                                                     ");
                parameter.AppendSql("   AND c.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                                         ");
                parameter.AppendSql("   AND c.JepDate <= TO_DATE(:TODATE1,'YYYY-MM-DD')                                                                                         ");
                parameter.AppendSql("   AND c.DelDate IS NULL                                                                                                                   ");
                parameter.AppendSql("   AND c.GbSts <> 'D'                                                                                                                      ");
                if (sGjJong == "1")
                {
                    parameter.AppendSql("   AND c.GjJong IN('11','13','23','69','31')                                                                                           ");
                }
                else if (sGjJong == "2")
                {
                    parameter.AppendSql("   AND c.GjJong IN('11','31')                                                                                                          ");
                }
                parameter.AppendSql("   AND a.SDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                                           ");
                parameter.AppendSql("   AND a.SDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                                           ");
                parameter.AppendSql("   AND a.DelDate IS NULL                                                                                                                   ");
                parameter.AppendSql("   AND a.GbSts <> 'D'                                                                                                                      ");
                if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
                {
                    parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                                                            ");
                }
                parameter.AppendSql("  UNION ALL                                                                                                                                ");
                parameter.AppendSql(" SELECT 0 as Pano1,c.Pano as Pano2, 0 as Wrtno1, c.Wrtno as Wrtno2, c.SName,d.JUMIN2                                                       ");
                parameter.AppendSql("     , '0' as Sdate, TO_CHAR(c.JepDate,'YYYY-MM-DD') JepDate ,c.GjJong, c.LtdCode,c.Mirno1, c.Mirno3, '2' as Gubun                         ");
                parameter.AppendSql("  FROM ADMIN.HIC_JEPSU c, ADMIN.HIC_PATIENT d                                                                                  ");
                parameter.AppendSql(" WHERE c.Pano = d.Pano(+)                                                                                                                  ");
                parameter.AppendSql("   AND c.JepDate>= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                                          ");
                parameter.AppendSql("   AND c.JepDate<= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                                          ");
                parameter.AppendSql("   AND c.DelDate IS NULL                                                                                                                   ");
                parameter.AppendSql("   AND c.GbSts <> 'D'                                                                                                                      ");
                if (sGjJong == "1")
                {
                    parameter.AppendSql("   AND c.GjJong IN('11','13','23','69','31')                                                                                           ");
                }
                else if (sGjJong == "2")
                {
                    parameter.AppendSql("   AND c.GjJong IN('11','31')                                                                                                          ");
                }
                if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
                {
                    parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                                                            ");
                }
                parameter.AppendSql("   AND d.JUMIN2 NOT IN ( SELECT b.JUMIN2                                                                                                   ");
                parameter.AppendSql("                           FROM ADMIN.HEA_JEPSU a, ADMIN.HIC_PATIENT b                                                         ");
                parameter.AppendSql("                          WHERE a.PANO = b.PANO(+)                                                                                         ");
                parameter.AppendSql("                            AND a.SDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                  ");
                parameter.AppendSql("                            AND a.SDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                  ");
                parameter.AppendSql("                            AND a.DELDATE IS NULL                                                                                          ");
                parameter.AppendSql("                            AND a.GBSts <> 'D'                                                                                             ");
                if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
                {
                    parameter.AppendSql("                            AND a.LTDCODE = :LTDCODE                                                                                   ");
                }
                parameter.AppendSql("                       )                                                                                                                   ");
                parameter.AppendSql(" ORDER BY Gubun, Pano2, 9                                                                                                                  ");
            }
            else if (strGubun == "1")
            {
                parameter.AppendSql("SELECT a.Pano as Pano1,c.Pano as Pano2, a.Wrtno as Wrtno1,c.Wrtno as Wrtno2, a.SName,d.JUMIN2                                              ");
                parameter.AppendSql("     , TO_CHAR(a.SDate,'YYYY-MM-DD') SDate ,TO_CHAR(c.JepDate,'YYYY-MM-DD') JepDate ,c.GjJong,  c.LtdCode,c.Mirno1, c.Mirno3, '1' as Gubun ");
                parameter.AppendSql("  FROM ADMIN.HEA_JEPSU a, ADMIN.HIC_JEPSU c, ADMIN.HIC_PATIENT d                                                         ");
                parameter.AppendSql(" WHERE a.Pano = d.Pano                                                                                                                     ");
                parameter.AppendSql("   AND c.Pano = d.Pano                                                                                                                     ");
                parameter.AppendSql("   AND c.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                                         ");
                parameter.AppendSql("   AND c.JepDate <= TO_DATE(:TODATE1,'YYYY-MM-DD')                                                                                         ");
                parameter.AppendSql("   AND c.DelDate IS NULL                                                                                                                   ");
                parameter.AppendSql("   AND c.GbSts <> 'D'                                                                                                                      ");
                if (sGjJong == "1")
                {
                    parameter.AppendSql("   AND c.GjJong IN('11','13','23','69','31')                                                                                           ");
                }
                else if (sGjJong == "2")
                {
                    parameter.AppendSql("   AND c.GjJong IN('11','31')                                                                                                          ");
                }
                parameter.AppendSql("   AND a.SDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                                           ");
                parameter.AppendSql("   AND a.SDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                                           ");
                parameter.AppendSql("   AND a.DelDate IS NULL                                                                                                                   ");
                parameter.AppendSql("   AND a.GbSts <> 'D'                                                                                                                      ");
                if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
                {
                    parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                                                            ");
                }
                parameter.AppendSql(" ORDER BY 2, 9                                                                                                                             ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql("SELECT c.Pano as Pano2, c.Wrtno as Wrtno2, c.SName, d.JUMIN2                                                                               ");
                parameter.AppendSql("     , TO_CHAR(c.JepDate,'YYYY-MM-DD') JepDate ,c.GjJong,  c.LtdCode,c.Mirno1, c.Mirno3, '2' as Gubun                                      ");
                parameter.AppendSql("  FROM ADMIN.HIC_JEPSU c, ADMIN.HIC_PATIENT d                                                                                  ");
                parameter.AppendSql(" WHERE c.Pano = d.Pano(+)                                                                                                                  ");
                parameter.AppendSql("   AND c.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                                         ");
                parameter.AppendSql("   AND c.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                                         ");
                parameter.AppendSql("   AND c.DelDate IS NULL                                                                                                                   ");
                parameter.AppendSql("   AND c.GbSts <> 'D'                                                                                                                      ");
                if (sGjJong == "1")
                {
                    parameter.AppendSql("   AND c.GjJong IN('11','13','23','69','31')                                                                                           ");
                }
                else if (sGjJong == "2")
                {
                    parameter.AppendSql("   AND c.GjJong IN('11','31')                                                                                                          ");
                }
                if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
                {
                    parameter.AppendSql("   AND c.LTDCODE = :LTDCODE                                                                                                            ");
                }
                parameter.AppendSql("   AND d.JUMIN2 NOT IN ( SELECT b.JUMIN2                                                                                                   ");
                parameter.AppendSql("                           FROM ADMIN.HEA_JEPSU a, ADMIN.HIC_PATIENT b                                                         ");
                parameter.AppendSql("                          WHERE a.PANO = b.PANO(+)                                                                                         ");
                parameter.AppendSql("                            AND a.SDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                                                   ");
                parameter.AppendSql("                            AND a.SDate <= TO_DATE(:TODATE,'YYYY-MM-DD')                                                                   ");
                parameter.AppendSql("                            AND a.DELDATE IS NULL                                                                                          ");
                parameter.AppendSql("                            AND a.GBSts <> 'D'                                                                                             ");
                if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
                {
                    parameter.AppendSql("                       AND a.LTDCODE = :LTDCODE                                                                                        ");
                }
                parameter.AppendSql("                       )                                                                                                                   ");
                parameter.AppendSql(" ORDER BY 1, 6                                                                                                                             ");
            }

            if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
                
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (strGubun != "2")
            {
                parameter.Add("TODATE1", strToDate1);
            }

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetSendListItembyJepDate(string strFDate, string strTDate, string strChasu, string strJohap, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.GjJong, a.GjChasu,b.Juso1,b.Juso2, a.SName, b.JuMin2, TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate                     ");
            parameter.AppendSql("     , a.GbInWon, c.Wrtno,c.TongboDate, a.Ltdcode                                                                          ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b ,                                                                ");
            parameter.AppendSql("       ( SELECT a.Wrtno, TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate                                                     ");
            parameter.AppendSql("           FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_Bohum1 b                                                      ");
            parameter.AppendSql("          WHERE a.Wrtno = b.Wrtno(+)                                                                                       ");
            parameter.AppendSql("            AND a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                ");
            parameter.AppendSql("            AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                ");
            parameter.AppendSql("            AND a.GjJong IN ('11','12','13','14','23')                                                                     ");
            parameter.AppendSql("            AND a.Deldate IS NULL                                                                                          ");
            parameter.AppendSql("            AND a.GbSts <> 'D'                                                                                             ");
            parameter.AppendSql("            AND b.GbPrint IS NOT NULL                                                                                      ");
            parameter.AppendSql("          UNION ALL                                                                                                        ");
            parameter.AppendSql("         SELECT a.Wrtno,TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate                                                      ");
            parameter.AppendSql("           FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_Bohum2 b                                                      ");
            parameter.AppendSql("          WHERE a.Wrtno = b.Wrtno(+)                                                                                       ");
            parameter.AppendSql("            AND a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                ");
            parameter.AppendSql("            AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                ");
            parameter.AppendSql("            AND a.GjJong IN ('16','17','18','19')                                                                          ");
            parameter.AppendSql("            AND a.Deldate IS Null                                                                                          ");
            parameter.AppendSql("            AND a.GbSts <> 'D'                                                                                             ");
            parameter.AppendSql("            AND b.GbPrint IS NOT NULL                                                                                      ");
            parameter.AppendSql("          UNION ALL                                                                                                        ");
            parameter.AppendSql("         SELECT a.Wrtno,TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate                                                      ");
            parameter.AppendSql("           FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_special b                                                     ");
            parameter.AppendSql("          WHERE a.Wrtno = b.Wrtno(+)                                                                                       ");
            parameter.AppendSql("            AND a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                ");
            parameter.AppendSql("            AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                ");
            parameter.AppendSql("            AND a.GjJong IN ('11','16','23','28')                                                                          ");
            parameter.AppendSql("            AND a.Deldate IS NULL                                                                                          ");
            parameter.AppendSql("            AND a.GbSts <> 'D'                                                                                             ");
            parameter.AppendSql("            AND b.GbPrint IS NOT NULL                                                                                      ");
            parameter.AppendSql("          UNION ALL                                                                                                        ");
            parameter.AppendSql("         SELECT a.Wrtno,TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate                                                      ");
            parameter.AppendSql("           FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_CANCER_NEW b                                                      ");
            parameter.AppendSql("          WHERE a.Wrtno =b.Wrtno(+)                                                                                        ");
            parameter.AppendSql("            AND a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                ");
            parameter.AppendSql("            AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                ");
            parameter.AppendSql("            AND a.GjJong IN ('31')                                                                                         ");
            parameter.AppendSql("            AND a.Deldate IS NULL                                                                                          ");
            parameter.AppendSql("            AND a.GbSts <> 'D'                                                                                             ");
            parameter.AppendSql("            AND b.GbPrint IS NOT NULL                                                                                      ");
            parameter.AppendSql("          GROUP BY a.Wrtno,b.TongboDate                                                                                    ");
            parameter.AppendSql("       ) c                                                                                                                 ");
            parameter.AppendSql(" WHERE a.DelDate IS NULL                                                                                                   ");
            parameter.AppendSql("   AND a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                         ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                         ");
            parameter.AppendSql("   AND a.Pano = b.Pano(+)                                                                                                  ");
            parameter.AppendSql("   AND a.Wrtno = c.Wrtno(+)                                                                                                ");
            parameter.AppendSql("   AND c.Wrtno > 0                                                                                                         ");
            if (strChasu == "1")
            {
                parameter.AppendSql("   AND a.GjChasu = '1'                                                                                                 ");
            }
            else if (strChasu == "2")
            {
                parameter.AppendSql("   AND a.GjChasu = '2'                                                                                                 ");
            }
            switch (strJohap)
            {
                case "사업장":
                    parameter.AppendSql("   AND a.GjJong IN ('11','14','23','16','19','28')                                                                 ");
                    if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
                    {
                        parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                                    ");
                    }
                    break;
                case "공무원":
                    parameter.AppendSql("   AND a.GjJong IN ('12','17')                                                                                     ");
                    if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
                    {
                        parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                                    ");
                    }
                    break;
                case "성인병":
                    parameter.AppendSql("   AND a.GjJong IN ('13','18')                                                                                     ");
                    break;
                case "암검진":
                    parameter.AppendSql("   AND a.GjJong IN ('31')                                                                                          ");
                    break;
                default:
                    break;
            }
            parameter.AppendSql(" GROUP BY a.GjJong, a.GjChasu,b.Juso1,b.Juso2, a.SName, b.JuMin2, a.JepDate, a.GbInWon, c.Wrtno,c.TongboDate, a.Ltdcode    ");
            parameter.AppendSql(" ORDER BY a.Jepdate,a.SName                                                                                                ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetLifeItembyJepDateMirno1(string sJepDate, long argMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.LtdCode,a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.GjJong,a.GjChasu,a.Age  ");
            parameter.AppendSql("     , a.Ptno, a.SECOND_Flag,TO_CHAR(a.SECOND_Date,'YYYY-MM-DD') Second_Date, a.GbChul     ");
            parameter.AppendSql("     , a.GbChul2, a.Juso1 || ' ' || a.Juso2  JusoAA, b.Jumin2, a.Kiho, a.Gkiho ,c.*        ");
            parameter.AppendSql("     , TO_CHAR(c.PanjengDate,'YYYY-MM-DD') PanjengDate                                     ");
            parameter.AppendSql("     , TO_CHAR(c.TongboDate,'YYYY-MM-DD') TongboDate, a.SANGDAMDRNO, a.SANGDAMDATE, a.SEX  ");
            parameter.AppendSql("     , a.GBCHK1, a.GBCHK2                                                                  ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b , ADMIN.HIC_RES_BOHUM1 c   ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:JEPDATE,'YYYY-MM-DD')                                           ");
            parameter.AppendSql("   AND a.MIRNO1 = :MIRNO1                                                                  ");
            parameter.AppendSql("   AND a.Pano = b.Pano(+)                                                                  ");
            parameter.AppendSql("   AND a.Wrtno = c.Wrtno                                                                   ");
            parameter.AppendSql("   AND A.SEXAMS LIKE '%1164%'                                                              ");
            parameter.AppendSql("   AND c.PanjengDrNo > 0                                                                   ");
            parameter.AppendSql(" ORDER BY a.Sname, a.JepDate                                                               ");

            parameter.Add("JEPDATE", sJepDate);
            parameter.Add("MIRNO1", argMirno);

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyJepDateMirNo(string strFrDate, string strToDate, string strMirno, string strJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.Wrtno,TO_CHAR(a.JepDate,'YYYY-MM-DD')JepDate                                                      ");
            parameter.AppendSql("     , a.Sname, a.Age, a.GjJong, a.GjChasu,a.UCodes, a.SExams                                              ");
            parameter.AppendSql("     , a.Mirno1,a.Mirno2,a.Mirno3,a.Mirno4,a.Mirno5,a.Kiho,a.LTDCODE,b.Jumin2,a.MURYOAM,a.BOGUNSO,a.GKiho  ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b                                                  ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                                          ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE,'YYYY-MM-DD')                                                          ");
            parameter.AppendSql("   AND a.Pano = b.Pano(+)                                                                                  ");
            if (strJong == "1")
            {
                parameter.AppendSql("   AND a.MIRNO1 = :MIRNO                                                                               ");
            }
            else if (strJong == "3")
            {
                parameter.AppendSql("   AND a.MIRNO2 = :MIRNO                                                                               ");
            }
            else if (strJong == "4" || strJong == "E")
            {
                if (strJong != "E")
                {
                    parameter.AppendSql("   AND a.Mirno3 =  :MIRNO                                                                          ");
                }
                else
                {
                    parameter.AppendSql("   AND a.Mirno5 =  :MIRNO                                                                          ");
                }
                if (strJong == "4")
                {
                    parameter.AppendSql("   AND (a.GubDaeSang <> 'Y' OR a.GubDaeSang IS NULL)                                               "); //공단암
                }
                else if (strJong == "E")
                {
                    parameter.AppendSql("   AND a.GubDaeSang = 'Y'                                                                          "); //의료급여암
                }
            }
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                                   ");
            parameter.AppendSql(" ORDER BY a.SName,a.Pano,a.WRTNO                                                                           ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("MIRNO", strMirno);

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyJepDateMirNoJong(string sJepDate, string strJong, long argMirNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.LtdCode,a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.GjJong,a.GjChasu            ");
            parameter.AppendSql("     , a.SECOND_Flag,TO_CHAR(a.SECOND_Date,'YYYY-MM-DD') Second_Date, b.Email, b.Tel,b.HPHONe  ");
            parameter.AppendSql("     , b.Jumin2, a.Kiho, a.Gkiho , a.Sex,c.*, a.MAILCODE, a.JUSO1, A.JUSO2                     ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b , ADMIN.HIC_CANCER_NEW c       ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                            ");
            if (strJong == "4")
            {
                parameter.AppendSql("   AND a.MIRNO3 = :MIRNO                                                                   ");
            }
            else if (strJong == "E")
            {
                parameter.AppendSql("   AND a.MIRNO5 =  = :MIRNO                                                                ");
            }
            parameter.AppendSql("   AND a.Pano = b.Pano  AND a.Wrtno = c.Wrtno                                                  ");
            parameter.AppendSql(" ORDER BY a.Pano,a.WRTNO                                                                       ");

            parameter.Add("JEPDATE", sJepDate);
            parameter.Add("MIRNO", argMirNo);

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetDentalItembyJepDateMirNo2(string sJepDate, long argMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.LtdCode,a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.GjJong,a.GjChasu,a.Ptno ");
            parameter.AppendSql("     , a.SECOND_Flag,TO_CHAR(a.SECOND_Date,'YYYY-MM-DD') Second_Date, a.GbChul,a.GbChul2   ");
            parameter.AppendSql("     , b.Jumin2, a.Kiho, a.Gkiho ,c.*, TO_CHAR(c.TongBoDate,'YYYY-MM-DD') TongBoDate       ");
            parameter.AppendSql("     , a.MAILCODE, a.JUSO1, A.JUSO2,c.*                                                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b , ADMIN.HIC_RES_DENTAL c   ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:JEPDATE,'YYYY-MM-DD')                                           ");
            parameter.AppendSql("   AND a.Pano = b.Pano                                                                     ");
            parameter.AppendSql("   AND a.Wrtno = c.Wrtno                                                                   ");
            parameter.AppendSql("   AND a.MIRNO2 = :MIRNO2                                                                  ");
            parameter.AppendSql("   AND c.PanjengDrNo > 0                                                                   ");
            parameter.AppendSql(" ORDER BY a.LtdCode,a.WRTNO                                                                ");

            parameter.Add("JEPDATE", sJepDate);
            parameter.Add("MIRNO2", argMirno);

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetFirstMunbyJepDateMirNo1(string sJepDate, long argMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.LtdCode,a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.GjJong,a.GjChasu,       ");
            parameter.AppendSql("     , a.SECOND_Flag,TO_DATE(a.SECOND_Date,'YYYY-MM-DD') Second_Date,b.Tel, b.Email,       ");
            parameter.AppendSql("     , b.Jumin2, a.Kiho, a.Gkiho, A.MAILCODE, A.JUSO1, A.JUSO2 ,c.*                        ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b , ADMIN.HIC_RES_BOHUM1 c   ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:JEPDATE,'YYYY-MM-DD')                                           ");
            parameter.AppendSql("   AND a.MIRNO1 = :MIRNO1                                                                  ");
            parameter.AppendSql("   AND a.Pano = b.Pano(+)                                                                  ");
            parameter.AppendSql("   AND a.Wrtno = c.Wrtno                                                                   ");
            parameter.AppendSql("   AND a.GjChasu ='1'                                                                      ");
            parameter.AppendSql("   AND c.PanjengDrNo > 0                                                                   ");
            parameter.AppendSql(" ORDER BY a.Sname, a.JepDate                                                               ");

            parameter.Add("JEPDATE", sJepDate);
            parameter.Add("MIRNO1", argMirno);

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyJepDateMirNo1(string sJepDate, long argMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.LtdCode,a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.GjJong,a.GjChasu,a.Age,a.Ptno   ");
            parameter.AppendSql("     , a.SECOND_Flag,TO_CHAR(a.SECOND_Date,'YYYY-MM-DD') Second_Date, a.GbChul,a.GbChul2           ");
            parameter.AppendSql("     , a.Juso1 || ' ' || a.Juso2  JusoAA                                                           ");
            parameter.AppendSql("     , b.Jumin2, a.Kiho, a.Gkiho ,c.*, TO_CHAR(c.PanjengDate,'YYYY-MM-DD') PanjengDate             ");
            parameter.AppendSql("     , TO_CHAR(c.TongboDate,'YYYY-MM-DD') TongboDate,a.SANGDAMDRNO, a.SANGDAMDATE, a.SEX, a.GBCHK1 ");
            parameter.AppendSql("     , a.GBCHK2, A.MAILCODE, A.JUSO1, A.JUSO2                                                      ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b , ADMIN.HIC_RES_BOHUM1 c           ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:JEPDATE,'YYYY-MM-DD')                                                 ");
            parameter.AppendSql("   AND a.MIRNO1 = :MIRNO1                                                                          ");
            parameter.AppendSql("   AND a.Pano = b.Pano(+)                                                                          ");
            parameter.AppendSql("   AND a.Wrtno = c.Wrtno                                                                           ");
            parameter.AppendSql("   AND c.PanjengDrNo > 0                                                                           ");
            parameter.AppendSql(" ORDER BY a.Sname, a.JepDate                                                                       ");            

            parameter.Add("JEPDATE", sJepDate);
            parameter.Add("MIRNO1", argMirno);

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyTongBoDate(string strFDate, string strTDate, List<string> strJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Ptno,a.SName,b.HPhone,b.Tel,TO_CHAR(a.JEPDATE,'YYYY-MM-DD') YDate     ");
            parameter.AppendSql("     , ADMIN.FC_HIC_LTDNAME(a.LTDCODE) LTDNAME, A.GBCHUL, a.GJJONG       ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b                      ");
            parameter.AppendSql(" WHERE a.Pano = b.Pano(+)                                                      ");
            parameter.AppendSql("   AND a.TONGBODATE  >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                         ");    //통보일자
            parameter.AppendSql("   AND a.TONGBODATE  <  TO_DATE(:TODATE, 'YYYY-MM-DD')                         ");    //통보일자
            parameter.AppendSql("   AND A.GJJONG IN (:GJJONG)                                                   ");
            parameter.AppendSql("   AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019')           ");
            parameter.AppendSql("   AND A.WEBPRINTREQ IS NULL                                                   ");
            parameter.AppendSql("   AND A.JONGGUMYN = '0'                                                       "); //순수HR 대상자만
            parameter.AppendSql("   AND A.GBCHK3 <> 'Y'                                                         ");
            parameter.AppendSql(" ORDER BY JEPDATE,SName,HPhone                                                 ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            parameter.AddInStatement("GJJONG", strJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public HIC_JEPSU_PATIENT GetLtdCodebyRDateJumin2SName(string strRDate, string strJumin2, List<string> b04_NOT_PATIENT)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.LtdCode                                                       ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b              ");
            parameter.AppendSql(" WHERE a.PANO = b.Pano                                                 ");
            parameter.AppendSql("   AND a.JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND a.GBSTS <> 'D'                                                  ");
            parameter.AppendSql("   AND b.JUMIN2 = :JUMIN2                                              ");
            if (b04_NOT_PATIENT.Count > 0)
            {
                parameter.AppendSql("   AND b.SNAME NOT IN (:SNAME)                                     ");
            }
            parameter.AppendSql(" GROUP BY b.JUMIN2, b.LTDCODE                                          ");

            parameter.Add("JEPDATE", strRDate);
            parameter.Add("JUMIN2", strJumin2);
            if (b04_NOT_PATIENT.Count > 0)
            {
                parameter.AddInStatement("SNAME", b04_NOT_PATIENT);
            }

            return ExecuteReaderSingle<HIC_JEPSU_PATIENT>(parameter);
        }

        public int GetCountbyJumin2(string strRDate, string strJumin2, List<string> B04_NOT_PATIENT)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT FROM HIC_JEPSU a, HIC_PATIENT b      ");
            parameter.AppendSql(" WHERE a.Pano=b.Pano                                       ");
            parameter.AppendSql(" AND a.JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql(" AND a.Gbsts <> 'D'                                        ");
            parameter.AppendSql(" AND b.JUMIN2 = :JUMIN2                                    ");
            parameter.AppendSql(" AND b.SNAME NOT IN (:SNAME)                               ");
            parameter.AppendSql("  GROUP BY b.Jumin2,b.LtdCode                              ");

            parameter.Add("JEPDATE", strRDate);
            parameter.Add("JUMIN2", strJumin2);
            parameter.AddInStatement("SNAME", B04_NOT_PATIENT);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyRecvTime(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Ptno,a.SName,b.HPhone,b.Tel,TO_CHAR(a.JEPDATE,'YYYY-MM-DD') YDate     ");
            parameter.AppendSql("     , ADMIN.FC_HIC_LTDNAME(a.LTDCODE) LTDNAME, a.GJJONG                 ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU            a                                      ");
            parameter.AppendSql("     , ADMIN.HIC_PATIENT          b                                      ");
            parameter.AppendSql("     , ADMIN.HIC_CHARTTRANS_PRINT c                                      ");
            parameter.AppendSql(" WHERE a.Pano = b.Pano(+)                                                      ");
            parameter.AppendSql("   AND A.WRTNO = c.WRTNO                                                       ");
            parameter.AppendSql("   AND c.RECVTIME  >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                           ");    //통보일자
            parameter.AppendSql("   AND c.RECVTIME  <  TO_DATE(:TODATE, 'YYYY-MM-DD')                           ");    //통보일자
            parameter.AppendSql("   AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019')           ");
            parameter.AppendSql("   AND a.GBCHK3 = 'Y'                                                          ");
            parameter.AppendSql("   AND c.JOBTIME IS NULL                                                       ");
            parameter.AppendSql(" ORDER BY A.JEPDATE, A.SName, B.HPhone                                         ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyWebPrintSend(string strFDate, string gstrSysDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.PTNO, B.SNAME, B.HPHONE, A.JEPDATE, A.GJJONG,A.SEX        ");
            parameter.AppendSql("     , A.AGE, A.WebPrintSend, B.JUMIN, B.JUMIN2, 1 GUBUN           ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU A, ADMIN.HIC_PATIENT B          ");
            parameter.AppendSql(" WHERE a.PTNO = b.PTNO(+)                                          ");
            parameter.AppendSql("   AND B.SNAME <> '이중챠트'                                       ");
            parameter.AppendSql("   AND A.WebPrintSend >= TO_DATE(:FRDATE, 'YYYY-MM-DD')            ");   //파일전송날짜
            parameter.AppendSql("   AND A.WebPrintSend <  TO_DATE(:TODATE, 'YYYY-MM-DD')            ");   //파일전송날짜
            parameter.AppendSql("   AND A.WEBSEND = '*'                                             ");
            parameter.AppendSql(" UNION ALL                                                         ");
            parameter.AppendSql("SELECT A.PTNO, B.SNAME, B.HPHONE, A.SDATE JEPDATE, A.GJJONG,A.SEX  ");
            parameter.AppendSql("     , A.AGE, A.WebPrintSend, B.JUMIN, B.JUMIN2, 2 GUBUN           ");
            parameter.AppendSql("  FROM ADMIN.HEA_JEPSU A, ADMIN.HIC_PATIENT B          ");
            parameter.AppendSql(" WHERE a.PTNO = b.PTNO(+)                                          ");
            parameter.AppendSql("   AND B.SNAME <> '이중챠트'                                       ");
            parameter.AppendSql("   AND A.WebPrintSend >= TO_DATE(:FRDATE, 'YYYY-MM-DD')            ");   //파일전송날짜
            parameter.AppendSql("   AND A.WebPrintSend <  TO_DATE(:TODATE, 'YYYY-MM-DD')            ");   //파일전송날짜
            parameter.AppendSql("   AND A.WEBSEND = '*'                                             ");
            parameter.AppendSql(" ORDER BY PTNO                                                     ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", gstrSysDate);

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public int GetCountbyJuminGjYear(string strJumin, string strGjYear, string strJong, string strJepdate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                              ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b          ");
            parameter.AppendSql(" WHERE a.PANO = b.Pano                                             ");
            parameter.AppendSql("   AND b.JUMIN2 = :JUMIN2                                          ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                          ");
            parameter.AppendSql("   AND a.JEPDATE >= TO_DATE(:JEPDATE,'YYYY-MM-DD')                  ");

            switch (strJong)
            {
                case "11":
                case "12":
                case "13":
                case "14":
                case "41":
                case "42":
                case "43":
                    parameter.AppendSql("   AND a.GjJong IN ('16','17','18','19','44','45','46')    ");
                    break;
                case "23":
                    parameter.AppendSql("   AND a.GjJong IN ('28')                                  ");
                    break;
                default:
                    break;
            }

            parameter.Add("JUMIN2", strJumin);
            parameter.Add("GJYEAR", strGjYear);
            parameter.Add("JEPDATE", strJepdate);
            
            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyJepDateLtdCode(string strFrDate, string strToDate, string strGjJong, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.GjJong,a.Pano,a.SName,a.Sex,a.Age,a.GbSabun                       ");
            parameter.AppendSql("     , a.WRTNO,a.LtdCode,a.BuseName,b.Jumin2,a.SExams,a.UCodes,a.Jisa,a.gkiho,a.Kiho,b.PTno,a.gbchul,a.gbinwon     ");
            parameter.AppendSql("     , a.GJYEAR,a.GJCHASU,a.GJBANGI,a.GBCHUL,a.MAILCODE,a.JUSO1,a.JUSO2,a.BURATE,a.JIKGBN                          ");
            parameter.AppendSql("     , a.JIKJONG,TO_CHAR(a.IPSADATE,'YYYY-MM-DD')IPSADATE ,a.GBSUCHEP, a.GBDENTAL,a.BOGUNSO,a.TEL,a.LIVER2         ");
            parameter.AppendSql("     , a.YOUNGUPSO,a.MILEAGEAM, a.MURYOAM,a.GUMDAESANG,a.MILEAGEAMGBN,a.MURYOGBN , a.REMARK, a.EMAIL,b.Hphone      ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b                                                          ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                 ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                 ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                                           ");
            if (strGjJong != "00" && !strGjJong.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.GJJONG = :GJJONG                                                                                      ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                                    ");
            }
            parameter.AppendSql("   AND a.Pano=b.Pano(+)                                                                                            ");
            parameter.AppendSql(" ORDER BY a.GjJong,a.SName                                                                                         ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (strGjJong != "00" && !strGjJong.IsNullOrEmpty())
            {
                parameter.Add("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }


            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyJepDateSname(string strFrDate, string strToDate, string strSname )
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT  A.WRTNO, A.PTNO, B.SNAME, B.HPHONE, A.JEPDATE, A.GJJONG,A.SEX                      ");
            parameter.AppendSql(" , A.AGE, A.WebPrintSend, B.JUMIN, B.JUMIN2                                                ");
            parameter.AppendSql(" FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b                                   ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql(" AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                           ");
            parameter.AppendSql(" AND a.DelDate IS NULL                                                                     ");
            parameter.AppendSql(" AND A.WEBPRINTSEND IS NOT NULL                                                            ");
            if (!strSname.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME = :SNAME                                                                ");
            }
            parameter.AppendSql("   AND a.PTNO=b.PTNO(+)                                                                    ");
            parameter.AppendSql(" ORDER BY PTNO                                                                             ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strSname.IsNullOrEmpty())
            {
                parameter.Add("SNAME", strSname);
            }

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetNightItembyJepDateSName(string strFrDate, string strToDate, string strSName, string fstrID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO, a.SName, a.Sex, a.Age, TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate         ");
            parameter.AppendSql("     , a.GjJong, a.LTDCODE, a.Pano, b.JUMIN2                                           ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b                              ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND a.Pano=b.Pano(+)                                                                ");
            if (fstrID == "출장접수")
            {
                parameter.AppendSql("   AND a.GbChul = 'Y'                                                              ");
            }
            parameter.AppendSql("   AND a.DelDate IS NULL                                                               ");
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                         ");
            }
            parameter.AppendSql(" ORDER BY a.JepDate, a.wrtno, a.SName                                                  ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", strSName);
            }

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyWrtNoJepDateSNameLtdCode(long nWrtNo, string strFrDate, string strToDate, string strResult, string strPart, string strGjJong, string strSchool, string strSName, long nLtdCode, string strHea, string strSort, string strUcode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.GjJong,a.SName,a.Sex,a.Age,b.JUMIN2,a.UCODES      ");
            parameter.AppendSql("     , a.WRTNO,a.LtdCode,a.GjChasu,a.UCodes,a.GbMunjin1,a.GbMunjin2,a.GbMunjin3,a.GbDental         ");
            parameter.AppendSql("     , a.GbSpcMunjin,a.Jobsabun,a.GbSTS,a.IEMunNo, a.GJYEAR, a.PANO                                ");
            parameter.AppendSql("     , TO_CHAR(a.SangDamDate,'YYYY-MM-DD') SangDamDate,a.SangDamDrno                               ");
            parameter.AppendSql("     , TO_CHAR(a.PanjengDate,'YYYY-MM-DD') PanjengDate,a.PanjengDrno                               ");
            parameter.AppendSql("     , TO_CHAR(a.TongboDate,'YYYY-MM-DD') TongboDate, TO_CHAR(a.EntTime,'YYYY-MM-DD') EntTime      ");
            parameter.AppendSql("     , a.webprintsend, A.PrtSabun, A.WEBPRINTREQ, A.GBADDPAN                                       ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b                                          ");
            if (nWrtNo > 0)
            {
                parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                        ");
            }
            else
            {
                parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                              ");
                parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')                                              ");
                parameter.AppendSql("   AND a.DELDATE IS NULL                                                                       ");
                parameter.AppendSql("   AND a.GJJONG NOT IN ('55')                                                                  ");
                if (strResult == "Y")
                {
                    parameter.AppendSql("   AND a.GbSTS IN ('0','1')                                                                ");
                }
                if (strPart == "1")
                {
                    parameter.AppendSql("   AND a.GbChul = 'N'                                                                      "); //내원검진만
                }
                if (strPart == "2")
                {
                    parameter.AppendSql("   AND a.GbChul = 'Y'                                                                      "); //출장검진
                }
                if (strGjJong != "00" && strGjJong != "")
                {
                    parameter.AppendSql("   AND a.GJJONG = :GJJONG                                                                  ");
                    if (strGjJong == "56") //학생검진
                    {
                        if (strSchool == "1") //초교 1,4학년
                        {
                            parameter.AppendSql("   AND a.Gbn = '1'                                                                 "); //초등학교
                            parameter.AppendSql("   AND a.Class IN (1,4)                                                            "); //1,4학년
                        }
                        else if (strSchool == "2") //초교 2,3,5,6학년
                        {
                            parameter.AppendSql("   AND a.Gbn = '1'                                                                 "); //초등학교
                            parameter.AppendSql("   AND a.Class IN (2,3,5,6)                                                        "); //2,3,5,6학년
                        }
                    }
                }
                if (!strSName.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                                 ");
                }
                if (nLtdCode != 0)
                {
                    parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                ");
                }
                if (strHea == "Y")
                {
                    parameter.AppendSql("   AND a.PtNo IN (SELECT PTNO FROM ADMIN.HEA_JEPSU                                   ");
                    parameter.AppendSql("                   WHERE SDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                           ");
                    parameter.AppendSql("                     AND SDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                           ");
                    parameter.AppendSql("                     AND DELDATE IS NULL)                                                  ");
                }

                if (strUcode == "Y")
                {
                    parameter.AppendSql("   AND a.UCODES IS NOT NULL                                                                ");
                }
            }
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                                                                          ");


            if (strSort == "0")
            {
                parameter.AppendSql(" ORDER BY a.SNAME, a.JEPDATE                                                                   ");
            }
            else
            {
                parameter.AppendSql(" ORDER BY a.JEPDATE ,a.SNAME                                                                   ");
            }

            if (!strSName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", strSName);
            }

            if (nWrtNo > 0)
            {
                parameter.Add("WRTNO", nWrtNo);
            }
            else
            {
                parameter.Add("FRDATE", strFrDate);
                parameter.Add("TODATE", strToDate);
                if (strGjJong != "00" && strGjJong != "")
                {
                    parameter.Add("GJJONG", strGjJong);
                }

                if (nLtdCode > 0)
                {
                    parameter.Add("LTDCODE", nLtdCode);
                }
            }
            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetListByDate(string argFDate, string argTDate, string argJong, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JEPDATE,'YYYY-MM-DD') JEPDATE,a.GJJONG,a.PANO,a.SNAME,a.SEX,a.AGE     ");
            parameter.AppendSql("     , a.WRTNO,a.LTDCODE,a.BUSENAME,b.JUMIN,b.JUMIN2,a.UCODES,a.KIHO,b.PTNO,a.BOGUNSO  ");
            parameter.AppendSql("     , a.GJYEAR,a.GJBANGI,TO_CHAR(a.TONGBODATE,'YYYY-MM-DD') TONGBODATE,a.JONGGUMYN    ");
            parameter.AppendSql("     , b.TEL, b.HPHONE                                                                 ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b                              ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')                                      ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                               ");
            //학생신검제외
            parameter.AppendSql("   AND a.GJJONG <> '56'                                                                ");
            if (argJong != "**")
            {
                parameter.AppendSql("   AND a.GJJONG = :GJJONG                                                          ");
            }
            
            if (nLtdCode > 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                        ");
            }
            
            parameter.AppendSql("   AND a.PANO=b.PANO(+)                                                                ");
            parameter.AppendSql(" ORDER BY a.JEPDATE, a.SNAME                                                           ");

            parameter.Add("FRDATE", argFDate);
            parameter.Add("TODATE", argTDate);
            if (argJong != "**")
            { 
                parameter.Add("GJJONG", argJong);
            }

            if (nLtdCode > 0)
            { 
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetSecondListByTongboDate(string argFDate, string argTDate, string argView, List<string> lstJob, string argSName, long argLtdCode, string argJong, string argHea)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(a.JEPDATE,'MM/DD') JEPDATE, a.SNAME, a.SEX, a.AGE               ");
            parameter.AppendSql("      ,a.GJJONG, ADMIN.FC_HIC_GJJONG_NAME(a.GJJONG, a.UCODES) AS GJNAME  ");
            parameter.AppendSql("      ,a.WRTNO, a.LTDCODE, a.UCODES, c.TEL, a.PANO, a.JONGGUMYN, a.GJYEAR      ");
            parameter.AppendSql("      ,a.GJYEAR, a.GJBANGI, a.SECOND_EXAMS, a.SECOND_SAYU, a.SECOND_MISAYU     ");
            parameter.AppendSql("      ,a.GBSTS, TO_CHAR(a.SECOND_TONGBO,'MM/DD') AS SECOND_TONGBO              ");

            parameter.AppendSql("      ,CASE a.GJJONG WHEN '11' THEN DECODE(a.SECOND_DATE, '', ADMIN.FC_HIC_SECOND_DATE(a.PTNO, a.GJYEAR, a.GJJONG, TO_CHAR(a.JEPDATE,'YYYY-MM-DD')), TO_CHAR(a.SECOND_DATE,'MM/DD')) ");
            parameter.AppendSql("                     WHEN '23' THEN DECODE(a.SECOND_DATE, '', ADMIN.FC_HIC_SECOND_DATE(a.PTNO, a.GJYEAR, a.GJJONG, TO_CHAR(a.JEPDATE,'YYYY-MM-DD')), TO_CHAR(a.SECOND_DATE,'MM/DD')) ");
            parameter.AppendSql("       END AS SECOND_DATE");

            parameter.AppendSql("      ,FC_HIC_LTDNAME(a.LTDCODE) AS LTDNAME, a.PTNO                            ");
            parameter.AppendSql("      ,SUBSTR(c.JUMIN, 1, 6) || '-' || SUBSTR(c.JUMIN, 7, 1) || '******' AS JUMIN, A.TONGBODATE ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT c                      ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                             ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                             ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                       ");
            parameter.AppendSql("   AND a.SECOND_FLAG = 'Y'                                                     ");
            if (argHea == "Y")     //통보
            { 
                parameter.AppendSql("   AND a.JONGGUMYN = '1'                                                     ");
            }
            if (argView == "1")     //통보
            {
                parameter.AppendSql("   AND (a.SECOND_TONGBO IS NOT NULL OR a.SECOND_DATE IS NOT NULL)          ");
            }
            else if (argView == "2")    //미통보
            {
                parameter.AppendSql("   AND a.SECOND_TONGBO IS NULL                                             "); 
                parameter.AppendSql("   AND a.SECOND_DATE   IS NULL                                             "); 
            }

            parameter.AppendSql("   AND a.GJJONG IN (:GJJONG)                                                   ");

            if (argLtdCode > 0) { parameter.AppendSql("   AND a.LTDCODE =:LTDCODE "); }
            if (!argSName.IsNullOrEmpty()) { parameter.AppendSql("   AND a.SNAME LIKE :SNAME "); }
            if (argJong == "4")
            {
                parameter.AppendSql(" AND (SECOND_EXAMS != '3' AND SECOND_EXAMS != '6' AND SECOND_EXAMS != '3,6') ");
            }

            parameter.AppendSql("   AND a.PANO = c.PANO(+)                                                        ");
            parameter.AppendSql(" ORDER BY a.LTDCODE, a.SNAME, a.JEPDATE                                          ");

            parameter.Add("FRDATE", argFDate);
            parameter.Add("TODATE", argTDate);
            parameter.AddInStatement("GJJONG", lstJob);

            if (argLtdCode > 0) { parameter.Add("LTDCODE", argLtdCode); }
            if (!argSName.IsNullOrEmpty()) { parameter.AddLikeStatement("SNAME", argSName); }

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public IList<HIC_JEPSU_PATIENT> GetNhicListByDate(string argFDate, string argTDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JEPDATE,'YYYY-MM-DD') JEPDATE_STR, a.GJYEAR,a.GJJONG,a.SNAME          ");
            parameter.AppendSql("      ,a.WRTNO,b.JUMIN2,a.SEXAMS,a.GBCHUL,a.GBCHUL2,a.GBDENTAL,a.GBAM, a.GBDENTONLY    ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b                              ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                               ");
            parameter.AppendSql("   AND a.GJJONG IN ('11', '31')                                                        ");
            parameter.AppendSql("   AND (a.GBSUJIN_SET IS NULL OR a.GBSUJIN_SET <> 'Y')                                 ");
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                                                              ");
            parameter.AppendSql(" ORDER BY a.GJYEAR,a.JEPDATE,a.GJJONG,a.WRTNO                                          ");

            parameter.Add("FRDATE", argFDate);
            parameter.Add("TODATE", argTDate);
           
            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyJepDateGjjongLtdCode(string strDate1, string strDate2, string strJong, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.Pano,a.SName,b.Tel, b.Jumin2  ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b                      ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                             ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                             ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                       ");
            parameter.AppendSql("   AND a.GBSTS NOT IN ('0','D')                                                ");
            if (strJong != "00")
            {
                parameter.AppendSql("   AND a.GJJONG = :GJJONG                                                  ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                ");
            }
            parameter.AppendSql("   AND a.Pano = b.Pano(+)                                                      ");
            parameter.AppendSql(" GROUP BY a.JepDate,a.Pano,a.SName,b.Tel,b.Jumin2                              ");
            parameter.AppendSql(" ORDER BY a.JepDate,a.SName,a.SName,b.Tel,b.Jumin2                             ");

            parameter.Add("FRDATE", strDate1);
            parameter.Add("TODATE", strDate2);
            if (strJong != "00")
            {
                parameter.Add("GJJONG", strJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyWrtNoLtdCode(List<string> strList, long nWrtNo, long nLtdCode, string strSort)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,TO_CHAR(a.JepDate,'YYYYMMDD') JepDate , FC_HC_PATIENT_JUMINNO(B.PTNO) JUMIN         ");
            parameter.AppendSql("     , a.PANJENGDRNO, a.GJJONG, a.SNAME, a.LTDCODE, a.SEX, a.AGE, b.TEL, b.HPHONE, A.TONGBODATE    ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b                          ");
            parameter.AppendSql(" WHERE a.DELDATE IS NULL                                                           ");
            if (strList.Count != 0)
            {
                parameter.AppendSql("   AND a.WRTNO IN (:LIST)                                                      ");
            }
            
            parameter.AppendSql("   AND a.GJJONG IN ('11','14','16','19','23','28','41','44')                       ");
            if (nWrtNo != 0)
            {
                parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                        ");
            }
            else if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                    ");
            }
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                                                          ");
            if (strSort == "1")
            {
                parameter.AppendSql(" ORDER BY a.SNAME, a.WRTNO, a.JEPDATE                                          ");
            }
            else if (strSort == "2")
            {
                parameter.AppendSql(" ORDER BY a.JEPDATE, a.SNAME, a.WRTNO                                          ");
            }
            else if (strSort == "3")
            {
                parameter.AppendSql(" ORDER BY a.LTDCODE, a.SNAME, a.WRTNO                                          ");
            }
            if (strList.Count != 0)
            {
                parameter.AddInStatement("LIST", strList);
            }
            if (nWrtNo != 0)
            {
                parameter.Add("WRTNO", nWrtNo);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public HIC_JEPSU_PATIENT GetItembyJuMin(string strJuMin, string strJOBDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.ROWID                                                 ");
            parameter.AppendSql(" FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b       ");
            parameter.AppendSql(" WHERE 1 = 1                                                   ");
            parameter.AppendSql(" AND a.PANO = b.PANO                                           ");
            parameter.AppendSql(" AND A.DELDATE IS NULL                                         ");
            parameter.AppendSql(" AND A.AUTOJEP = 'Y'                                           ");
            parameter.AppendSql(" AND B.JUMIN2 = :JUMIN                                         ");
            parameter.AppendSql(" AND A.JEPDATE = TO_DATE(:JOBDATE, 'YYYY-MM-DD')               ");

            parameter.Add("JUMIN", strJuMin);
            parameter.Add("JOBDATE", strJOBDATE);

            return ExecuteReaderSingle<HIC_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_PATIENT> GetListByJepdateJong(string argFDate, string argTDate, string argJong, string argSname, long nLtdCode, string argTongbo, string argGubun1, string argSort)
        {        
            MParameter parameter = CreateParameter();


            parameter.AppendSql(" SELECT a.WRTNO,a.SNAME,a.GJJONG,a.UCodes,a.Pano,a.Sex,a.LtdCode,TO_CHAR(a.JepDate,'YYYY-MM-DD') JEPDATE           ");
            parameter.AppendSql(" ,TO_CHAR(a.TongboDate,'YYYY-MM-DD') TongboDate,b.JUMIN,b.BuseName,b.Sabun                                         ");
            parameter.AppendSql(" ,a.Juso1||'  '||a.Juso2 as juso,a.GbDental,a.GjYear,a.GjChasu,a.SExams                                            ");
            parameter.AppendSql(" ,a.PanjengDrno,TO_CHAR(a.PanjengDate,'YYYY-MM-DD') PanjengDate,a.WebPrintReq,a.Ptno, a.MAILCODE                   ");
            parameter.AppendSql(" FROM HIC_JEPSU a, HIC_PATIENT b                                                                                   ");
            parameter.AppendSql(" WHERE a.JepDate>=TO_DATE(:FDATE,'YYYY-MM-DD')                                                                     ");
            parameter.AppendSql(" AND a.JepDate<=TO_DATE(:TDATE,'YYYY-MM-DD')                                                                       ");
            parameter.AppendSql(" AND a.DelDate IS NULL                                                                                             ");
            parameter.AppendSql(" AND a.GjYear>='2021'                                                                                              ");
            parameter.AppendSql(" AND a.WebPrintReq IS NULL                                                                                         ");
            parameter.AppendSql(" AND a.PANO = b.PANO(+)                                                                                            ");
            parameter.AppendSql(" AND a.PanjengDate IS NOT NULL                                                                                     ");
            parameter.AppendSql(" AND (a.GBSTS > '1' AND a.GBSTS NOT IN ('D'))                                                                      ");
            
            //발행구분
            if(argTongbo.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND a.TongboDate IS NULL          ");
            }
            else
            {
                parameter.AppendSql(" AND a.TongboDate IS NOT NULL      ");
            }

            if(argJong != "**")
            {
                parameter.AppendSql(" AND a.Gjjong = :GJJONG            ");
            }
            
            if (argJong =="**" & argGubun1.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND a.GJJONG IN('11','12','14','16','17','19','41','42','44','45')                ");
            }
            else
            {
                parameter.AppendSql(" AND a.GJJONG IN('21','22','23','24','25','26','27','28','29','30','33','49')      ");
            }

            if (!argSname.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                         ");
            }

            if (nLtdCode > 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                        ");
            }

            if (argSort == "1")
            {
                parameter.AppendSql(" ORDER BY a.LtdCode,a.SName,a.JepDate                      ");
            }
            else if (argSort == "2")
            {
                parameter.AppendSql(" ORDER BY a.SName,a.JepDate,a.LtdCode                      ");
            }
            else if(argSort == "3")
            {
                parameter.AppendSql(" ORDER BY a.JepDate,a.LtdCode,a.SName                      ");
            }

            parameter.Add("FRDATE", argFDate);
            parameter.Add("TODATE", argTDate);
            
            if (nLtdCode > 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            if (!argSname.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", argSname);
            }


            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }


        public List<HIC_JEPSU_PATIENT> GetListByItems(string argFDate, string argTDate, string argSname, string argWrtno, string argLtdCode, string argRePrt, string argSort, string argJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT a.WRTNO,a.SNAME,a.GJJONG,a.UCodes,a.Pano,a.Sex,a.LtdCode,TO_CHAR(a.JepDate,'YYYY-MM-DD') JEPDATE           ");
            parameter.AppendSql(" ,TO_CHAR(a.TongboDate,'YYYY-MM-DD') TongboDate,b.JUMIN,b.BuseName,b.Sabun                                         ");
            parameter.AppendSql(" ,a.Juso1||'  '||a.Juso2 as juso,a.GbDental,a.GjYear,a.GjChasu,a.SExams                                            ");
            parameter.AppendSql(" ,a.PanjengDrno,TO_CHAR(a.PanjengDate,'YYYY-MM-DD') PanjengDate,a.WebPrintReq,a.Ptno, a.MAILCODE                   ");
            parameter.AppendSql(" FROM HIC_JEPSU a, HIC_PATIENT b                                                                                   ");
            parameter.AppendSql(" WHERE a.JepDate>=TO_DATE(:FDATE,'YYYY-MM-DD')                                                                     ");
            parameter.AppendSql(" AND a.JepDate<=TO_DATE(:TDATE,'YYYY-MM-DD')                                                                       ");
            parameter.AppendSql(" AND a.GJJONG = :GJJONG                                                                                            ");
            parameter.AppendSql(" AND a.DelDate IS NULL                                                                                             ");
            parameter.AppendSql(" AND a.WebPrintReq IS NULL                                                                                         ");
            parameter.AppendSql(" AND a.PANO = b.PANO(+)                                                                                            ");
            parameter.AppendSql(" AND a.PanjengDate IS NOT NULL                                                                                     ");
            parameter.AppendSql(" AND (a.GBSTS > '1' AND a.GBSTS NOT IN ('D'))                                                                      ");

            if(argRePrt.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND a.TONGBODATE IS NULL                                                                                       ");
            }
            else
            {
                parameter.AppendSql(" AND a.TONGBODATE IS NOT NULL                                                                                   ");
            }

            if (!argSname.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                         ");
            }
            if (!argLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                            ");
            }

            if (!argLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                        ");
            }

            if (argSort == "1")
            {
                parameter.AppendSql(" ORDER BY a.LtdCode,a.SName,a.JepDate                      ");
            }
            else if (argSort == "2")
            {
                parameter.AppendSql(" ORDER BY a.SName,a.JepDate,a.LtdCode                      ");
            }
            else if (argSort == "3")
            {
                parameter.AppendSql(" ORDER BY a.JepDate,a.LtdCode,a.SName                      ");
            }


            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);
            parameter.Add("GJJONG", argJong);

            if (!argSname.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", argSname);
            }

            if (!argWrtno.IsNullOrEmpty())
            {
                parameter.Add("WRTNO", argWrtno);
            }
            if (!argLtdCode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", argLtdCode);
            }


            return ExecuteReader<HIC_JEPSU_PATIENT>(parameter);
        }

        public HIC_JEPSU_PATIENT GetEndoItembyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Ptno, a.Sname, b.Jumin2,b.JUSO1 || b.JUSO2 Juso,a.Sex,a.Age               ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b                          ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                            ");
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                                                          ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReaderSingle<HIC_JEPSU_PATIENT>(parameter);
        }
    }
}
