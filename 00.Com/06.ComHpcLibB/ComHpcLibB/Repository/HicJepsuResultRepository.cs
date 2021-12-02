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
    public class HicJepsuResultRepository : BaseRepository
    {        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResultRepository()
        {
        }

        public List<HIC_JEPSU_RESULT> GetItemByWrtnoResult(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE, RESULTCODE, RESULT  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU  A    ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_RESULT B    ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND A.WRTNO = B.WRTNO           ");
            parameter.AppendSql("   AND B.RESULT <> '미실시'         ");
            parameter.AppendSql("   ORDER BY EXCODE                 ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }
        

        public List<HIC_JEPSU_RESULT> GetNoActingbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Name                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU  a    ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_RESULT b    ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            if (fnWRTNO > 0)
            {
                parameter.AppendSql("   AND WRTNO = :WRTNO          ");
            }
            else
            {
                parameter.AppendSql("   AND a.SDate = TRUNC(SYSDATE)");
                parameter.AppendSql("   AND a.DelDate IS NULL       ");
            }
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)        ");
            parameter.AppendSql("   AND b.Part = '5'                ");
            parameter.AppendSql("   AND b.Result IS NULL            ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public HIC_JEPSU_RESULT GetItembyPtNo(string strJepDate, string strPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(A.JEPDATE,'YYYY-MM-DD') JEPDATE, A.GJYEAR, A.SEX, A.AGE, A.GBCHUL   ");
            parameter.AppendSql("     , A.PTNO,A.WRTNO,B.RESULT                                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU  a                                                    ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_RESULT b                                                    ");
            parameter.AppendSql(" WHERE a.JEPDATE = TO_DATE(:JEPDATE,'YYYY-MM-DD')                                  ");
            parameter.AppendSql("   AND a.PTNO   = :PTNO                                                            ");
            parameter.AppendSql("   AND a.WRTNO  = b.WRTNO(+)                                                       ");
            parameter.AppendSql("   AND b.EXCODE = 'A142'                                                           ");
            parameter.AppendSql("   AND (b.RESULT IS NULL OR b.RESULT = '')                                         ");

            parameter.Add("JEPDATE", strJepDate);
            parameter.Add("PTNO", strPtNo);

            return ExecuteReaderSingle<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetItembyJepDateJong(string strFrDate, string strToDate, string strSName, string strGubun, long nLtdCode, string strJong, string strGjJong, List<string> strTemp, string strSort, string strGbStart)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.UCodes,a.WRTNO,a.EXCODE,a.RESULT, TO_CHAR(b.JepDate,'YYYY-MM-DD') JepDate, b.LtdCode,b.Sname,b.GjJong     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT a, KOSMOS_PMPA.HIC_JEPSU b                                                           ");
            parameter.AppendSql(" WHERE b.WRTNO = a.WRTNO(+)                                                                                        ");
            parameter.AppendSql("   AND b.JEPDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                                                  ");
            parameter.AppendSql("   AND b.JEPDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')                                                                  ");
            parameter.AppendSql("   AND b.DELDATE IS  NULL                                                                                          ");
            parameter.AppendSql("   AND b.GjJong NOT IN ('55','58','81','82','83')                                                                  "); //운전면허,측정,대행,종검은 제외
            if (strSName != "")
            {
                parameter.AppendSql("   AND b.SNAME LIKE :SNAME                                                                                     ");
            }
            if (strGubun == "1")
            {
                parameter.AppendSql("   AND b.GBCHUL = 'N'                                                                                          "); //내원검진만
            }
            if (strGubun == "2")
            {
                parameter.AppendSql("   AND b.GBCHUL = 'Y'                                                                                          "); //출장검진만
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND  b.LTDCODE = :LTDCODE                                                                                   ");
            }
            if (strGjJong != "**" && !strGjJong.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND b.GJJONG = :GJJONG                                                                                      ");
            }
            if (strJong == "간촬")
            {
                parameter.AppendSql("   AND a.EXCODE = ('A214')                                                                                    ");
            }
            else if (strJong == "직촬")
            {
                parameter.AppendSql("   AND a.EXCODE = ('A215')                                                                                    ");
            }

            if (strGbStart != "")
            {
                parameter.AppendSql("   AND a.RESULT = (:RESULT)                                                                                   ");
            }
            if (strSort == "1")
            {
                parameter.AppendSql(" ORDER BY b.SNAME                                                                                              ");
            }
            else
            {
                parameter.AppendSql(" ORDER BY a.RESULT                                                                                             ");
            }

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (strSName != "")
            {
                parameter.AddLikeStatement("SNAME", strSName);
            }

            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            if (strGjJong != "**" && !strGjJong.IsNullOrEmpty())
            {
                parameter.Add("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (strGbStart != "")
            {
                parameter.AddInStatement("RESULT", strTemp);
            }

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetListJepDateExCodeIN(string strDate, List<string> strExCodes)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.PTNO, TO_CHAR(a.JepDate,'YYYY-MM-DD') JEPDATE, a.PANO, a.SNAME             ");
            parameter.AppendSql("     , b.EXCODE, a.WRTNO, b.ACTIVE, KOSMOS_PMPA.FC_HIC_EXCODE_NM(b.EXCODE) EXAMNAME ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a                                                      ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_RESULT b                                                     ");
            parameter.AppendSql(" WHERE a.JEPDATE =TO_DATE(:JEPDATE ,'YYYY-MM-DD')                              ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                       ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                    ");
            parameter.AppendSql("   AND b.EXCODE IN (:EXCODE)                                                   ");
            parameter.AppendSql(" ORDER BY a.JEPDATE,a.SNAME                                                    ");

            parameter.Add("JEPDATE", strDate);
            parameter.AddInStatement("EXCODE", strExCodes, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetItembyJepDateGbChulExCode(string argBDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') Jepdate                                 ");
            parameter.AppendSql("     , a.Ptno,a.Pano,a.Wrtno,a.XrayNo,b.ExCode,b.ResCode,b.Result,b.ROWID RID  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RESULT b                       ");
            parameter.AppendSql(" WHERE a.JEPDATE =TO_DATE(:JEPDATE ,'YYYY-MM-DD')                              ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                       ");
            parameter.AppendSql("   AND a.GbChul = 'Y'                                                          ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                    ");
            parameter.AppendSql("   AND b.ExCode IN ('A135','A139')                                             ");
            parameter.AppendSql("   AND (b.RESULT IS NULL OR b.RESULT ='' )                                     "); //액팅처리안된것
            parameter.AppendSql(" ORDER BY a.WRTNO,b.ExCode                                                     ");

            parameter.Add("JEPDATE", argBDate);

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetItembyGbChulExCode()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') Jepdate                                 ");
            parameter.AppendSql("     , a.Ptno,a.Pano,a.Wrtno,a.XrayNo,b.ExCode,b.ResCode,b.Result,b.ROWID RID  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RESULT b                       ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TRUNC(SYSDATE)-7                                           ");
            parameter.AppendSql("   AND a.JEPDATE < TRUNC(SYSDATE)                                              ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                       ");
            parameter.AppendSql("   AND a.GbChul = 'Y'                                                          ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                    ");
            parameter.AppendSql("   AND b.ExCode IN ('A112','A136','A898','A989')                               ");
            parameter.AppendSql("   AND (b.RESULT IS NULL OR b.RESULT ='' )                                     "); //액팅처리안된것
            parameter.AppendSql(" ORDER BY a.WRTNO,b.ExCode                                                     ");

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetHeaItembySDate(string strJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.PANO,a.WRTNO,a.SNAME,a.GJJONG,a.LTDCODE,a.ExamChange,a.GBDaily,a.AMPM2                    ");
            parameter.AppendSql("     , TO_CHAR(a.EntTime,'HH24:MI:SS') EntTime                                                     ");
            parameter.AppendSql("     , TO_CHAR(a.SDate,'YYYY-MM-DD') SDate, c.Familly                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a, KOSMOS_PMPA.HEA_RESULT b, KOSMOS_PMPA.HIC_PATIENT c                ");
            parameter.AppendSql(" WHERE a.SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("   AND a.DELDATE IS  NULL                                                                          ");
            parameter.AppendSql("   AND a.GBSTS NOT IN ('0','D')                                                                    ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO                                                                           ");
            parameter.AppendSql("   AND a.PTNO = c.PTNO                                                                             ");
            parameter.AppendSql(" GROUP BY a.SDate,a.PANO,a.WRTNO,a.SNAME,a.GJJONG,a.LTDCODE,a.EntTime,a.ExamChange,a.GBDaily       ");
            parameter.AppendSql("        , a.AMPM2,c.Familly                                                                        ");
            parameter.AppendSql(" ORDER By a.SName,a.Pano                                                                           ");

            parameter.Add("SDATE", strJepDate);

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetItembyJepDatePart(string strJepDate, string strPart, string strGb)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Wrtno, b.Sname,b.Sex,b.Gjjong, C.NAME                                         ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_RESULT a, KOSMOS_PMPA.HIC_JEPSU b, KOSMOS_PMPA.HIC_EXJONG c     ");
            parameter.AppendSql("  WHERE a.Wrtno = b.Wrtno                                                              ");
            parameter.AppendSql("    AND b.JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                    ");
            parameter.AppendSql("    AND b.DELDATE IS  NULL                                                             ");
            parameter.AppendSql("    AND b.JONGGUMYN <> '1'                                                             ");
            if (strGb == "1")
            {
                parameter.AppendSql("   AND b.GBCHUL = 'N'                                                              "); //내원
            }
            if (strGb == "2")
            {
                parameter.AppendSql("   AND b.GBCHUL = 'Y'                                                              "); //사업장
            }
            parameter.AppendSql("    AND a.PART = :PART                                                                 ");
            parameter.AppendSql("    AND (a.Active is Null OR a.Active = ' ')                                           ");
            parameter.AppendSql("    AND B.GJJONG = C.CODE(+)                                                           ");
            parameter.AppendSql("  GROUP By a.Wrtno, b.Sname,b.Sex, b.Gjjong,C.NAME                                     ");
            parameter.AppendSql("  ORDER By a.Wrtno                                                                     ");

            parameter.Add("JEPDATE", strJepDate);
            parameter.Add("PART", strPart, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetListSpcExamByPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.EXCODE, b.RESULT                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a                             ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_RESULT b                            ");
            parameter.AppendSql(" WHERE a.PTNO = :PTNO                                      ");
            parameter.AppendSql("   AND a.JEPDATE = TRUNC(SYSDATE)                          ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                   ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO                                   ");
            parameter.AppendSql("   AND EXCODE IN ('MU11','MU15','MU12','MU74','LM10','TR11','TH12','TH22','TZ08','A899','A902','A992','A993','A803') ");
            
            parameter.Add("PTNO", argPtno);

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetItembyJepDateExCode(string strFrDate, string strToDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.Pano,a.SName,a.LtdCode,b.ExCode,b.Result                          ");
            parameter.AppendSql("     , TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RESULT b                           ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                           ");
            parameter.AppendSql("   AND a.LtdCode IS NOT NULL                                                       ");
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                                          ");
            parameter.AppendSql("   AND b.ExCode IN (SELECT Code FROM HIC_BCode WHERE Gubun='HIC_생체시료검사코드') ");
            parameter.AppendSql(" ORDER BY a.JepDate,a.SName                                                        ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetItembyExCode(string strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.SName,a.XRayno,a.PTno                         ");
            parameter.AppendSql("     , TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RESULT b       ");
            parameter.AppendSql(" WHERE a.JepDate>=TRUNC(SYSDATE-20)                            ");
            parameter.AppendSql("   AND a.PanjengDate IS NULL                                   ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                       ");
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                      ");
            parameter.AppendSql("   AND b.EXCODE = :EXCODE                                      ");
            parameter.AppendSql("   AND (b.Result IS NULL OR b.Result='')                       ");
            parameter.AppendSql(" ORDER BY a.SName                                              ");

            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetItemAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.SName,a.LtdCode,a.GjJong,a.GbSTS          ");
            parameter.AppendSql("     , TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,b.Result    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RESULT b   ");
            parameter.AppendSql(" WHERE a.JepDate >= TRUNC(SYSDATE-20)                      ");
            parameter.AppendSql("   AND a.PanjengDate IS NULL                               ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                   ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                ");
            parameter.AppendSql("   AND b.ExCode = 'TR11'                                   ");
            parameter.AppendSql("   AND (b.Result IS NULL OR b.Result = '')                 ");
            parameter.AppendSql(" ORDER BY a.SName                                          ");

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetItembyJepDateExCode(string strFrDate, string strToDate, string strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,b.ExCode,b.Result               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RESULT b                               ");
            parameter.AppendSql(" WHERE a.JepDate>=TO_DATE(:FRDATE, 'YYYY-MM-DD')                                       ");
            parameter.AppendSql("   AND a.JepDate<=TO_DATE(:TODATE, 'YYYY-MM-DD')                                       ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                               ");
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                                              ");
            parameter.AppendSql("   AND b.EXCODE = :EXCODE                                                              ");
            parameter.AppendSql("   AND b.Result IN ('01','02','03','04','05','06','07','08','09','10','12','13','14')  ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetItembyJepDate(string strFrDate, string strToDate, string strGubn)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JEPDATE,'YYYY-MM-DD') JEPDATE, a.WRTNO, a.SNAME, a.GJJONG ");
            parameter.AppendSql("     , a.LTDCODE, a.XRAYNO, a.GBCHUL, a.PTNO, a.JONGGUMYN, a.PANO          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU  a                                            ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_RESULT b                                            ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                   ");
            if (strGubn == "1")
            {
                parameter.AppendSql("   AND a.GbChul <> 'Y'                                                 ");
            }
            else if (strGubn == "2")
            {
                parameter.AppendSql("   AND a.GbChul = 'Y'                                                  ");
            }
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                ");
            parameter.AppendSql("   AND b.EXCODE IN ('A142','TX13','TX14','TX11','A213','TX16','A211')      ");
            parameter.AppendSql(" ORDER BY a.JEPDATE, a.SNAME                                               ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetItem()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO, a.PANO, a.SNAME, b.ROWID   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU  a            ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_RESULT b            ");
            parameter.AppendSql(" WHERE a.JEPDATE = TRUNC(SYSDATE)          ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                ");
            parameter.AppendSql("   AND b.PART IN ('6','Q')                 "); //흉부촬영,요추촬영
            parameter.AppendSql("   AND b.RESULT IS NULL                    "); //결과 없는것
            parameter.AppendSql("   AND b.ACTIVE IS NULL                    ");
            parameter.AppendSql(" ORDER BY a.WRTNO                                                                                 ");

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetWrtNobySysDate(string SORT)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.PANO, a.WRTNO, a.SNAME, a.PTNO, a.GJJONG, a.LTDCODE                       ");
            parameter.AppendSql("     , TO_CHAR(a.ENTTIME,'HH24:MI:SS') ENTTIME, a.EXAMCHANGE, a.AMPM2              ");
            parameter.AppendSql("     , TO_CHAR(a.SDATE,'YYYY-MM-DD') SDATE, c.FAMILLY                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU   a                                                   ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_RESULT  b                                                   ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT c                                                   ");
            parameter.AppendSql(" WHERE a.SDATE = TRUNC(SYSDATE)                                                    ");
            parameter.AppendSql("   AND a.DELDATE IS  NULL                                                          ");
            parameter.AppendSql("   AND a.GBSTS IN ('1','2')                                                        ");
            parameter.AppendSql("   AND a.WRTNO   = b.WRTNO                                                         ");
            parameter.AppendSql("   AND a.PTNO    = c.PTNO                                                          ");
            parameter.AppendSql("   AND a.EndoGbn = '1'                                                             ");
            parameter.AppendSql("   AND a.WRTNO   = b.WRTNO(+)                                                      ");
            parameter.AppendSql("   AND b.EXCODE IN ('TX23','TX20','TX32','TX64','TX41')                            ");
            parameter.AppendSql(" GROUP BY a.PANO, a.WRTNO, a.SNAME, a.PTNO, a.GJJONG, a.LTDCODE                    ");
            parameter.AppendSql("     , a.ENTTIME, a.EXAMCHANGE, a.AMPM2, TO_CHAR(a.SDATE,'YYYY-MM-DD'), c.FAMILLY  ");
            if (SORT == "ENTTIME")
            {
                parameter.AppendSql(" ORDER By a.ENTTIME                                                            ");
            }
            else if (SORT == "SNAME")
            {
                parameter.AppendSql(" ORDER By a.SName,a.Pano                                                       ");
            }

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetActingInfobyBDate(string argBDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') Jepdate                                                             ");
            parameter.AppendSql("     , a.Ptno, a.Pano, a.Wrtno, a.XrayNo, b.ExCode, b.ResCode, b.Result, c.Code, c.GCode1, b.ROWID RID     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU  a                                                                            ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_RESULT b,                                                                           ");
            parameter.AppendSql("       (SELECT Code, GCode, GCode1 FROM KOSMOS_PMPA.HIC_CODE WHERE GUBUN = 'A1') c                         ");
            parameter.AppendSql(" WHERE a.WRTNO   = b.WRTNO(+)                                                                              ");
            parameter.AppendSql("   AND b.ExCode  = c.gCode                                                                                 ");
            parameter.AppendSql("   AND a.JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                                         ");
            parameter.AppendSql("   AND a.GbChul  = 'N'                                                                                     ");
            parameter.AppendSql("   AND(a.GBSTS IS NULL OR a.GBSTS <> 'D')                                                                  ");
            parameter.AppendSql("   AND EXCODE IN(SELECT gCODE FROM KOSMOS_PMPA.HIC_CODE WHERE GUBUN = 'A1')                                "); //액팅할코드만
            parameter.AppendSql("   AND(RESULT IS NULL OR RESULT = '')                                                                      "); //액팅처리안된것
            parameter.AppendSql(" ORDER BY a.WRTNO,b.ExCode                                                                                 ");

            parameter.Add("JEPDATE", argBDate);

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetActingInfobyBDate_Chul(string argBDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') Jepdate                                                             ");
            parameter.AppendSql("     , a.Ptno, a.Pano, a.Wrtno, a.XrayNo, b.ExCode, b.ResCode, b.Result, c.Code, c.GCode1, b.ROWID RID     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU  a                                                                            ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_RESULT b                                                                            ");
            parameter.AppendSql(" WHERE a.JEPDATE = TO_DATE(:JEPDATE,'YYYY-MM-DD')                                                          ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                                   ");
            parameter.AppendSql("   AND a.GbChul = 'Y'                                                                                      ");
            parameter.AppendSql("   AND a.WRTNO  = b.WRTNO(+)                                                                               ");
            parameter.AppendSql("   AND b.ExCode IN ('A135')                                                                                ");
            parameter.AppendSql("   AND (b.RESULT IS NULL OR b.RESULT ='' )                                                                 ");//액팅처리안된것
            parameter.AppendSql(" ORDER BY a.WRTNO,b.ExCode                                                                                 ");

            parameter.Add("JEPDATE", argBDate);

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }

        public List<HIC_JEPSU_RESULT> GetCountJepdateExcodeResult(string argFDATE, string argTDATE, string argExcode, string argResult)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JEPDATE, 'YYYYMM') JEPDATE, COUNT(*) COUNT  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU A,                            ");
            parameter.AppendSql("       KOSMOS_PMPA.HIC_RESULT B                            ");
            parameter.AppendSql(" WHERE A.WRTNO = B.WRTNO                                   ");
            parameter.AppendSql(" AND A.JEPDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')            ");
            parameter.AppendSql(" AND A.JEPDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')            ");
            parameter.AppendSql(" AND A.DELDATE IS NULL                                     ");
            parameter.AppendSql(" AND (B.EXCODE = :EXCODE AND B.RESULT = :RESULT)           "); 
            parameter.AppendSql(" GROUP BY TO_CHAR(A.JEPDATE, 'YYYYMM')                     ");

            parameter.Add("FDATE", argFDATE);
            parameter.Add("TDATE", argTDATE);
            parameter.Add("EXCODE", argExcode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RESULT", argResult);

            return ExecuteReader<HIC_JEPSU_RESULT>(parameter);
        }
    }
}
