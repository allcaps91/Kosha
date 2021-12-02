namespace ComHpcLibB.Repository
{
    using ComBase.Mvc;
    using ComHpcLibB.Model;
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// 
    /// </summary>
    public class HicComHpcRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicComHpcRepository()
        {
        }

        public List<COMHPC> GetStatistics(string strSdate, string strEdate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT TO_CHAR(JEPDATE,'YYYYMM') YYMM,GBINWON  GJJONG,GJCHASU CHASU,COUNT(*) JEPCNT ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_JEPSU");
            parameter.AppendSql("   WHERE       JEPDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     JEPDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     DELDATE IS NULL  ");
            parameter.AppendSql("       AND GBSTS NOT IN ( '0','D')  ");
            parameter.AppendSql("       AND GBINWON NOT IN ('57')  ");
            parameter.AppendSql("   GROUP BY TO_CHAR(JEPDATE,'YYYYMM'),GBINWON, GJCHASU  ");
            parameter.AppendSql("       UNION ALL  ");

            parameter.AppendSql("   SELECT TO_CHAR(CDATE,'YYYYMM') YYMM,DECODE(GBKUKGO,'Y','72','71') GJJONG,'1' CHASU,COUNT(*) JEPCNT ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_CHUKDTL");
            parameter.AppendSql("   WHERE       CDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     CDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("   GROUP BY TO_CHAR(CDATE,'YYYYMM'),GBKUKGO  ");
            parameter.AppendSql("       UNION ALL  ");


            parameter.AppendSql("   SELECT TO_CHAR(BDATE,'YYYYMM') YYMM,DECODE(GBKUKGO,'Y','82','81') GJJONG,'1' CHASU,COUNT(*) JEPCNT ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_BOGENMST");
            parameter.AppendSql("   WHERE       BDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     BDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     DELDATE IS NULL  ");
            parameter.AppendSql("   GROUP BY TO_CHAR(BDATE,'YYYYMM'),GBKUKGO  ");
            parameter.AppendSql("       UNION ALL  ");


            parameter.AppendSql("   SELECT  TO_CHAR(SDATE,'YYYYMM') YYMM,DECODE(SUBSTR(GJJONG,1,1) , '2' ,'92','91') GJJONG, '1'  CHASU,COUNT(*) JEPCNT ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HEA_JEPSU");
            parameter.AppendSql("   WHERE       SDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     SDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     GBSTS NOT IN ('0','D')  ");
            parameter.AppendSql("   GROUP BY TO_CHAR(SDATE,'YYYYMM'),DECODE(SUBSTR(GJJONG,1,1) , '2' ,'92','91')  ");

            parameter.Add("FDATE", strSdate);
            parameter.Add("TDATE", strEdate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetBillNo()
        {
            MParameter parameter = CreateParameter();
            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetGongDanListData(string fstrCOLNM, long nMirNo, string fstrLtdCode, string fstrJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT             TO_CHAR(A.JEPDATE,'YYYY-MM-DD') JEPDATE,A.WRTNO,A.PANO,A.KIHO                                                ");
            parameter.AppendSql("                    , A.SNAME,A.SEX,A.AGE,A.GJJONG,A.GJCHASU,A.SEXAMS,A.UCODES,C.JUMIN,A.MURYOAM, A.DENTAMT                        ");
            parameter.AppendSql("                    , SUM(B.TOTAMT) TOTAMT,SUM(B.LTDAMT) LTDAMT,SUM(B.JOHAPAMT) JOHAPAMT                                           ");

            parameter.AppendSql("   FROM               HIC_JEPSU A,HIC_SUNAP B,HIC_PATIENT C                                                                        ");
            parameter.AppendSql("   WHERE              A.PANO <> 999                                                                                                ");  // 전산실 연습제외

            if (fstrJong == "4")
            {
                parameter.AppendSql("       AND            A.MIRNO3 = :MIRNO                                                                                            ");  // 청구번호가 같은것
            }
            else if (fstrJong == "6")
            {
                parameter.AppendSql("       AND            A.MIRNO2 = :MIRNO                                                                                            ");  // 청구번호가 같은것
            }
            else
            {
                parameter.AppendSql("       AND            A.MIRNO1 = :MIRNO                                                                                            ");  // 청구번호가 같은것
            }
  
            if(fstrLtdCode != "0174")
            {
                parameter.AppendSql("       AND        (A.LTDCODE = :LTDCODE OR A.KIHO = :LTDCODE)                                                                    ");
            }
            else if(fstrJong != "6")
            {
                parameter.AppendSql("       AND        (A.MISUNO2 IS NULL OR A.MISUNO2 = 0)                                                                         ");  // 공단미청구
            }
            else
            {
                parameter.AppendSql("       AND        (A.MISUNO3 IS NULL OR A.MISUNO3 = 0)                                                                         ");  // 구강공단미청구
                parameter.AppendSql("       AND        A.GJCHASU = '1'                                                                                              ");  // 구강은 1차만
                parameter.AppendSql("       AND        A.GBDENTAL = 'Y'                                                                                             ");  // 구강 체크
            }
            
            parameter.AppendSql("       AND            A.WRTNO = B.WRTNO(+)                                                                                         ");
            parameter.AppendSql("       AND            B.JOHAPAMT <> 0                                                                                              ");  // 공단부담액이 있는것
            parameter.AppendSql("       AND            A.PANO = C.PANO(+)                                                                                           ");
            parameter.AppendSql("       AND          ( A.DELDATE IS  NULL OR A.GBSTS <> 'D' )                                                                       ");

            parameter.AppendSql("  GROUP BY            A.GJJONG,A.JEPDATE,A.WRTNO,A.PANO,A.KIHO,A.SNAME,A.SEX,A.AGE,A.GJCHASU,A.SEXAMS,A.UCODES,C.JUMIN,A.MURYOAM,A.DENTAMT   ");
            parameter.AppendSql("  ORDER BY            A.SNAME,A.PANO,A.JEPDATE,A.WRTNO                                                                             ");


            if (fstrJong == "4")
            {
                parameter.Add("MIRNO", nMirNo, Oracle.DataAccess.Client.OracleDbType.Char);
            }
            else if (fstrJong == "6")
            {
                parameter.Add("MIRNO", nMirNo, Oracle.DataAccess.Client.OracleDbType.Char);
            }
            else
            {
                parameter.Add("MIRNO", nMirNo, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            if (fstrLtdCode != "0174")
            {
                parameter.Add("LTDCODE", fstrLtdCode);
            }
            
            
            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetBogunListData(string fstrFDate, string fstrTDate, string strBogenso, string txtKiho, long nMirNo, bool ChkW_Am, bool ChkGub, bool ChkBogen)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT             TO_CHAR(A.JEPDATE,'YYYY-MM-DD') JEPDATE,A.WRTNO,A.PANO,A.KIHO                                                ");
            parameter.AppendSql("                    , A.SNAME,A.SEX,A.AGE,A.GJJONG,A.GJCHASU,A.SEXAMS,A.UCODES,C.JUMIN,A.MURYOAM                                   ");
            parameter.AppendSql("                    , SUM(B.TOTAMT) TOTAMT,SUM(B.LTDAMT) LTDAMT,SUM(B.JOHAPAMT) JOHAPAMT, SUM(B.BOGENAMT) BOGENAMT                 ");

            parameter.AppendSql("   FROM               HIC_JEPSU A,HIC_SUNAP B,HIC_PATIENT C                                                                        ");

            parameter.AppendSql("   WHERE              A.JEPDATE>=TO_DATE(:FDATE, 'YYYY-MM-DD')                                                                     ");
            parameter.AppendSql("       AND            A.JEPDATE<=TO_DATE(:TDATE, 'YYYY-MM-DD')                                                                     ");
            parameter.AppendSql("       AND            A.PANO <> 999                                                                                                ");
            parameter.AppendSql("       AND            A.BOGUNSO=:BOGUNSO                                                                                           ");
            parameter.AppendSql("       AND          ( A.MISUNO4 IS NULL OR A.MISUNO4 = 0 )                                                                         ");
            parameter.AppendSql("       AND            A.GJJONG IN ('31','35')                                                                                      ");

            if(txtKiho != "")
            {
                parameter.AppendSql("   AND            A.KIHO = :KIHO                                                                                               ");
            }

            // 자궁경부암
            if(ChkW_Am == true)
            {
                parameter.AppendSql("   AND          ( SUBSTR(A.GBAM,1,1) + SUBSTR(A.GBAM,3,1) + SUBSTR(A.GBAM,5,1) + SUBSTR(A.GBAM,7,1) +                          ");
                parameter.AppendSql("                  SUBSTR(GBAM,9,1) + SUBSTR(GBAM,11,1) ) =1                                                                    ");
                parameter.AppendSql("   AND            SUBSTR(A.GBAM,11,1) =1                                                                                       ");
            }

            // 의료급여암
            if (ChkGub == true)
            {
                parameter.AppendSql("   AND            A.MIRNO5=:MIRNO                                                                                              ");
            }

            // 보건소암
            if (ChkBogen == true)
            {
                parameter.AppendSql("   AND            A.MIRNO4=:MIRNO                                                                                              ");
            }

            parameter.AppendSql("       AND            A.WRTNO=B.WRTNO(+)                                                                                           ");
            parameter.AppendSql("       AND            A.PANO=C.PANO(+)                                                                                             ");
            parameter.AppendSql("       AND          ( A.DELDATE IS  NULL OR A.GBSTS <> 'D' )                                                                       ");

            parameter.AppendSql("  GROUP BY            A.GJJONG,A.JEPDATE,A.WRTNO,A.PANO,A.KIHO,A.SNAME,A.SEX,A.AGE,A.GJCHASU,A.SEXAMS,A.UCODES,C.JUMIN,A.MURYOAM   ");
            parameter.AppendSql("  ORDER BY            A.SNAME,A.PANO,A.JEPDATE,A.WRTNO                                                                             ");


            parameter.Add("FDATE", fstrFDate);
            parameter.Add("TDATE", fstrTDate);
            parameter.Add("BOGUNSO", strBogenso);

            if (txtKiho != "")
            {
                parameter.Add("KIHO", txtKiho);
            }

            if (ChkGub == true)
            {
                parameter.Add("MIRNO", nMirNo);
            }
            if (ChkBogen == true)
            {
                parameter.Add("MIRNO", nMirNo);
            }
            

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetIndividual_A(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT B.BUSE,C.NAME,B.KORNAME,COUNT(*) CNT ");
            parameter.AppendSql("   FROM HEA_JEPSU A, KOSMOS_ADM.INSA_MST B, BAS_BUSE C");
            parameter.AppendSql("   WHERE       A.SABUN =B.SABUN(+)  ");
            parameter.AppendSql("       AND     B.BUSE=C.BUCODE(+)  ");
            parameter.AppendSql("       AND     A.SDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     A.SDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     A.SABUN > 0  ");
            parameter.AppendSql("       AND     A.GBSTS NOT IN ( '0','D')  ");
            parameter.AppendSql("       AND     A.DELDATE IS NULL  ");
            parameter.AppendSql("       AND     A.GAMCODE IN ( '911','909','910','912','930' )  ");

            parameter.AppendSql("   GROUP BY B.BUSE,C.NAME,B.KORNAME  ");
            parameter.AppendSql("   ORDER BY B.BUSE,C.NAME,B.KORNAME  ");

            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetIndividual_B(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT B.BUSE,C.NAME,B.KORNAME,COUNT(*) CNT ");
            parameter.AppendSql("   FROM HEA_JEPSU A, KOSMOS_ADM.INSA_MST B, BAS_BUSE C");
            parameter.AppendSql("   WHERE       A.SABUN =B.SABUN(+)  ");
            parameter.AppendSql("       AND     B.BUSE=C.BUCODE(+)  ");
            parameter.AppendSql("       AND     A.SDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     A.SDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     A.SABUN >  0  ");
            parameter.AppendSql("       AND     A.GBSTS NOT IN ( '0','D')  ");
            parameter.AppendSql("       AND     A.DELDATE IS NULL  ");
            parameter.AppendSql("       AND     A.GAMCODE NOT IN ( '911','909','910','912','930' )  ");

            parameter.AppendSql("   GROUP BY B.BUSE,C.NAME,B.KORNAME  ");
            parameter.AppendSql("   ORDER BY B.BUSE,C.NAME,B.KORNAME  ");

            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetMonthData(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT B.BUSE, TO_CHAR(SDATE,'MM') SDATE,COUNT(*) CNT ");
            parameter.AppendSql("   FROM HEA_JEPSU A, KOSMOS_ADM.INSA_MST B, BAS_BUSE C");
            parameter.AppendSql("   WHERE       A.SABUN =B.SABUN(+)  ");
            parameter.AppendSql("       AND     B.BUSE=C.BUCODE(+)  ");
            parameter.AppendSql("       AND     A.SDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     A.SDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     A.SABUN > 0  ");
            parameter.AppendSql("       AND     A.GBSTS NOT IN ( '0','D')  ");
            parameter.AppendSql("       AND     A.DELDATE IS NULL  ");
            parameter.AppendSql("   GROUP BY B.BUSE,TO_CHAR(SDATE,'MM')  ");


            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetDepartment(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT B.BUSE,COUNT(*) CNT ");
            parameter.AppendSql("   FROM HEA_JEPSU A, KOSMOS_ADM.INSA_MST B, BAS_BUSE C");
            parameter.AppendSql("   WHERE       A.SABUN =B.SABUN(+)  ");
            parameter.AppendSql("       AND     B.BUSE=C.BUCODE(+)  ");
            parameter.AppendSql("       AND     A.SDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     A.SDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     A.SABUN > 0  ");
            parameter.AppendSql("       AND A.GBSTS NOT IN ( '0','D')  ");
            parameter.AppendSql("       AND A.DELDATE IS NULL  ");
            parameter.AppendSql("   GROUP BY B.BUSE  ");


            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetItemByDate(string argFDate, string argTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT A.PACSNO AS XRAYNO, B.GJJONG, B.LTDCODE, A.SNAME, A.AGE, A.SEX            ");
            parameter.AppendSql("        ,'' AS RESULT2, '' AS RESULT3, '' AS RESULT4, '0' AS GBSTS                ");
            parameter.AppendSql("        ,DECODE(A.PACSSTUDYID, '', '', 'Y') AS GBPACS, B.EXCODE AS XCODE          ");
            parameter.AppendSql("        ,A.PANO AS PTNO                                                           ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.XRAY_DETAIL A,                                                ");
            parameter.AppendSql("       (SELECT J.JEPDATE, J.PTNO, C.XRAYCODE, J.GJJONG, J.LTDCODE, R.EXCODE       ");
            parameter.AppendSql("          FROM KOSMOS_PMPA.HIC_JEPSU J,                                           ");
            parameter.AppendSql("               KOSMOS_PMPA.HIC_RESULT R,                                          ");
            parameter.AppendSql("               KOSMOS_PMPA.HIC_EXCODE C                                           ");
            parameter.AppendSql("         Where 1 = 1                                                              ");
            parameter.AppendSql("           AND J.WRTNO = R.WRTNO(+)                                               ");
            parameter.AppendSql("           AND R.EXCODE = C.CODE                                                  ");
            parameter.AppendSql("           AND J.JEPDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                          ");
            parameter.AppendSql("           AND J.JEPDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                          ");
            parameter.AppendSql("           AND J.DELDATE IS NULL                                                  ");
            parameter.AppendSql("           AND C.XRAYCODE IS NOT NULL                                             ");
            parameter.AppendSql("           AND C.CODE IN ( SELECT NAME FROM KOSMOS_PMPA.HIC_BCODE                 ");
            parameter.AppendSql("                            WHERE GUBUN='HIC_흉부방사선목록') ) B                  ");
            parameter.AppendSql("  WHERE 1 = 1                                                                     ");
            parameter.AppendSql("    AND A.BDATE = B.JEPDATE                                                       ");
            parameter.AppendSql("    AND A.PANO  = B.PTNO                                                          ");
            parameter.AppendSql("    AND A.XCODE = B.XRAYCODE                                                      ");
            parameter.AppendSql("    AND (A.EXINFO IS NULL OR A.EXINFO < 1001)                                      ");

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);

            return ExecuteReader<COMHPC>(parameter);
        }

    }
}
