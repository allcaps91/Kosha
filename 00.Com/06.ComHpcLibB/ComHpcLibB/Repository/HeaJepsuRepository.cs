namespace ComHpcLibB.Repository
{
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuRepository()
        {
        }

        public HEA_JEPSU Get_WrtNo(string PTNO, string SDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE, 'yyyy-MM-dd')       ");
            parameter.AppendSql("   AND GBSTS <> 'D'                                ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE = '')           ");

            parameter.Add("PTNO", PTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SDATE", SDATE);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public List<HEA_JEPSU> Read_Wrtno_SDate(long PANO, string SDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, SDATE                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");
            parameter.AppendSql("   AND SDATE < TO_DATE(:SDATE, 'yyyy-MM-dd')       ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            parameter.AppendSql(" ORDER BY JEPDATE DESC                             ");

            parameter.Add("PANO", PANO);
            parameter.Add("SDATE", SDATE);

            return ExecuteReader<HEA_JEPSU>(parameter);
        }

        public long GetWrtNobyHeaPaNo(long nHeaPano, string strSDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE PANO  = :PANO                           ");
            parameter.AppendSql("   AND SDate = TO_DATE(:SDATE,'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND GBSTS NOT IN ( 'D' ,'0' )               ");

            parameter.Add("PANO", nHeaPano);
            parameter.Add("SDATE", strSDate);

            return ExecuteScalar<long>(parameter);
        }

        public List<HEA_JEPSU> GetListSNameSTimeBySDate(string argDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME, AMPM2, STIME, TO_CHAR(SDATE, 'YYYY-MM-DD') SDATE      ");
            parameter.AppendSql("      ,AGE || '/' || SEX || DECODE(AMPM2, '2', '(후)', '') AS RTIME ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                                        ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')                        ");
            parameter.AppendSql("   AND DELDATE IS NULL                                              ");
            parameter.AppendSql(" ORDER BY AMPM2,SNAME                                               ");

            parameter.Add("SDATE", argDate);

            return ExecuteReader<HEA_JEPSU>(parameter);
        }

        public List<HEA_JEPSU> GetListSNameBySDateSTime(string argDate, string argSTime)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME, AMPM2, STIME, TO_CHAR(SDATE, 'YYYY-MM-DD') SDATE      ");
            parameter.AppendSql("      ,AGE || '/' || SEX || DECODE(AMPM2, '2', '(후)', '') AS RTIME ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                                        ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')                        ");
            parameter.AppendSql("   AND STIME = :STIME                                               ");
            parameter.AppendSql("   AND DELDATE IS NULL                                              ");
            parameter.AppendSql(" ORDER BY STIME,SNAME                                               ");

            parameter.Add("SDATE", argDate);
            parameter.Add("STIME", argSTime);

            return ExecuteReader<HEA_JEPSU>(parameter);
        }

        public List<HEA_JEPSU> GetCountSexBySDate(string strDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(WRTNO) AS WCNT, SEX               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE,'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql(" GROUP BY SEX                                  ");

            parameter.Add("SDATE", strDate);

            return ExecuteReader<HEA_JEPSU>(parameter);
        }

        public List<HEA_JEPSU> GetItembySuDateGbSts(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,JEPDATE,SDATE,IDATE,PANO,SNAME,SEX,AGE,MAILCODE,GBSTS,GJJONG,BURATE                       ");
            parameter.AppendSql("      ,LTDCODE, GAMCODE, SABUN, GBSANGDAM, PANCODE, PANREMARK, DRSABUN, DRNAME, DELDATE, PTNO          ");
            parameter.AppendSql("      ,PRTDATE, MAILDATE, SEXAMS, REMARK, JOBSABUN, ENTTIME, GBEXAM, PANRECODE, MISUNO, HIC_WRTNO      ");
            parameter.AppendSql("      ,HIC_AMT, HALINYN, CARDSEQNO, SANGDAMGBN, GBNBMD, SANGDAMTEL, GBENDO, SANGDAM_ONE                ");
            parameter.AppendSql("      ,GBDUST, RESULTSEND, SANGDAMOUT, GBEKG, PANMEMO, ACTMEMO, GBDAILY, ENDOGBN, SUNAP, EXAMREMARK    ");
            parameter.AppendSql("      ,GUIDETEL, AMPM, EXAMCHANGE, NRSABUN, SANGDAMYN, PANDATE, GAPANDATE, RECVDATE, GBAM              ");
            parameter.AppendSql("      ,ETCAM, GAMCODE2, ENDODATE_S, ENDODATE_C, CDATE, VIPREMARK, INBODY, AMPM2, GAMAMT, GBPRIVACY     ");
            parameter.AppendSql("      ,GONGDAN, PANREMARK2, LTDSAMT, GONGDANAMT, TICKET, GBJIKWON, SANGSABUN, MAILWEIGHT               ");
            parameter.AppendSql("      ,GBPDF, PDFPATH, WEBSEND, WEBSENDDATE, STIME, GBNAKSANG, GBMUNJIN, IEMUNNO, WEBPRINTREQ          ");
            parameter.AppendSql("      ,WEBPRINTSEND, LTDCODE2, KEYNO, PANO2, JUSO1, JUSO2, SANGDAMNOT, PRTSABUN, RECVSABUN             ");
            parameter.AppendSql("      ,GWRTNO, FC_HIC_LTDNAME(LTDCODE) AS LTDNAME, TO_CHAR(ENTTIME, 'HH24:MI') AS ENTTIME_HHMI         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                                                                           ");
            parameter.AppendSql(" WHERE WRTNO IN ( SELECT WRTNO FROM KOSMOS_PMPA.HEA_SUNAP                                              ");
            parameter.AppendSql("                   WHERE SUDATE >=TO_DATE(:SUFRDATE,  'YYYY-MM-DD')                                    ");
            parameter.AppendSql("                     AND SUDATE <=TO_DATE(:SUTODATE', 'YYYY-MM-DD')                                    ");
            parameter.AppendSql("                     AND MISUGYE ='01'  )                                                              "); //예약선수만
            parameter.AppendSql("   AND GBSTS = '0'                                                                                     "); //예약인경우만

            parameter.Add("SUFRDATE", strFDate);
            parameter.Add("SUTODATE", strTDate);

            return ExecuteReader<HEA_JEPSU>(parameter);
        }

        public string GetSnameByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                ");
            
            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public List<HEA_JEPSU> GetCountAmPm2BySDate(string argDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(WRTNO) AS WCNT, AMPM2                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND GBSTS != 'D'                                ");
            parameter.AppendSql(" GROUP BY AMPM2                                    ");

            parameter.Add("SDATE", argDate);

            return ExecuteReader<HEA_JEPSU>(parameter);
        }

        public List<HEA_JEPSU> GetCountSDateSexAmPm2BySDate(string argFDate, string argTDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(SDATE, 'YYYY-MM-DD') SDATE, SEX     ");
            parameter.AppendSql("      ,COUNT(WRTNO) AS WCNT, AMPM2                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE SDATE >= TO_DATE(:SFDATE, 'YYYY-MM-DD')     ");
            parameter.AppendSql("   AND SDATE <= TO_DATE(:STDATE, 'YYYY-MM-DD')     ");
            parameter.AppendSql("   AND GBSTS != 'D'                                ");
            parameter.AppendSql(" GROUP BY SDATE, AMPM2, SEX                        ");
            parameter.AppendSql(" ORDER BY SDATE, AMPM2, SEX                        ");

            parameter.Add("SFDATE", argFDate);
            parameter.Add("STDATE", argTDate);

            return ExecuteReader<HEA_JEPSU>(parameter);
        }

        public HEA_JEPSU GetItembyJumin(string strJumin)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.LTDCODE, b.WRTNO, b.GJJONG    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT a       ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_JEPSU   b       ");
            parameter.AppendSql(" WHERE a.JUMIN2 = :JUMIN               ");
            parameter.AppendSql("   AND b.PANO   = a.PANO(+)            ");
            parameter.AppendSql("   AND b.SDATE  = TRUNC(SYSDATE)       ");

            parameter.Add("JUMIN", strJumin);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public long GetListWrtnoByPtnoSDate(string fstrPano, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                ");
            parameter.AppendSql("   AND GBSTS NOT IN ('D')                      ");
            parameter.AppendSql("   AND SDATE  = TO_DATE(:SDATE, 'YYYY-MM-DD')    ");

            parameter.Add("PTNO", fstrPano);
            parameter.Add("SDATE", fstrJepDate);

            return ExecuteScalar<long>(parameter);
        }

        public void UpdateJepsuInfo(HEA_JEPSU code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql("   SET GBDAILY = :GBDAILY                      ");
            parameter.AppendSql("     , AMPM = :AMPM                            ");
            parameter.AppendSql("     , IDATE = TO_DATE(:IDATE,'YYYY-MM-DD')    ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                       ");

            parameter.Add("GBDAILY", code.GBDAILY);
            parameter.Add("AMPM", code.AMPM);
            parameter.Add("IDATE", code.IDATE);
            parameter.Add("WRTNO", code.WRTNO);

            ExecuteNonQuery(parameter);
        }

        public int GetCountByPtnoSDate(string fstrPano, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(WRTNO) CNT                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                ");
            parameter.AppendSql("   AND GBSTS NOT IN ('D')                          ");
            parameter.AppendSql("   AND SDATE  = TO_DATE(:SDATE, 'YYYY-MM-DD')      ");

            parameter.Add("PTNO", fstrPano);
            parameter.Add("SDATE", fstrJepDate);

            return ExecuteScalar<int>(parameter);
        }

        public HEA_JEPSU GetItemJepsuEkgResultbyWrtNO(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.Pano, A.SName, A.Age, A.Sex,A.LtdCode,TO_CHAR(SDate,'YYYY-MM-DD') SDate                                ");
            parameter.AppendSql("    , GjJong,PanCode,PanRemark,DrSabun,DrName,SangDamGbn,SangDamTel,SangDamOut,SangDam_One, ptno,JobSabun      ");
            parameter.AppendSql("    , EKGRESULT,a.PanMemo,A.NrSabun,A.SangDamYN,A.GbAm,A.EtcAm                                                 ");
            parameter.AppendSql("    , TO_CHAR(A.PanDate,'YYYY-MM-DD HH24:MI') PanDate, TO_CHAR(A.GaPanDate,'YYYY-MM-DD HH24:MI') GaPanDate     ");
            parameter.AppendSql("    , PanRemark2,a.IEMunNo                                                                                     ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.Hea_JEPSU A , KOSMOS_PMPA.HEA_EKG_RESULT B                                                   ");
            parameter.AppendSql(" WHERE A.WRTNO = :WRTNO                                                                                        ");
            parameter.AppendSql("   AND A.WRTNO = B.WRTNO(+)                                                                                    ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public int UpdateGbDust(string v, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql("   SET GBDUST = :GBDUST        ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO       ");

            parameter.Add("GBDUST", v, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public string GetSDatebyPtNo(string strPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                ");
            parameter.AppendSql("   AND SDATE > TRUNC(SYSDATE)                      ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            parameter.AppendSql("   AND TO_CHAR(SDate,'MMDD') <> '1225'             ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateGbExamByWrtno(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql("   SET GBEXAM = 'Y'            ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO         ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteNonQuery(parameter);
        }

        public long GetWrtNobyJepDateCardSeqNo(string strBDATE, long nCardSeq)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND CARDSEQNO = :CARDSEQNO                      ");

            parameter.Add("SDATE", strBDATE);
            parameter.Add("CARDSEQNO", nCardSeq);

            return ExecuteScalar<long>(parameter);
        }

        public List<HEA_JEPSU> GetJepsuCountAMPM(long argLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT LTDCODE, TO_CHAR(SDATE, 'YYYY-MM-DD') SDATE ");
            parameter.AppendSql("     , SUM(DECODE(AMPM2,'1',1,1)) AMCNT            ");
            parameter.AppendSql("      ,SUM(DECODE(AMPM2,'1',0,1)) PMCNT            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");
            parameter.AppendSql("   AND SDATE >= TRUNC(SYSDATE)                     ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            parameter.AppendSql("   AND LTDCODE IN (:LTDCODE)                       ");
            parameter.AppendSql(" GROUP BY LTDCODE, SDATE                           ");

            parameter.Add("LTDCODE", argLtdCode);

            return ExecuteReader<HEA_JEPSU>(parameter);
        }

        public HEA_JEPSU GetItemByWrtno(long argwRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,JEPDATE,TO_CHAR(SDATE, 'YYYY-MM-DD') SDATE,IDATE,PANO,SNAME,SEX,AGE,MAILCODE,GBSTS        ");
            parameter.AppendSql("      ,GJJONG,BURATE,GBCHK3,GBJUSO                                                                     ");
            parameter.AppendSql("      ,LTDCODE, GAMCODE, SABUN, GBSANGDAM, PANCODE, PANREMARK, DRSABUN, DRNAME, DELDATE, PTNO          ");
            parameter.AppendSql("      ,PRTDATE, MAILDATE, SEXAMS, REMARK, JOBSABUN, ENTTIME, GBEXAM, PANRECODE, MISUNO, HIC_WRTNO      ");
            parameter.AppendSql("      ,HIC_AMT, HALINYN, CARDSEQNO, SANGDAMGBN, GBNBMD, SANGDAMTEL, GBENDO, SANGDAM_ONE                ");
            parameter.AppendSql("      ,GBDUST, RESULTSEND, SANGDAMOUT, GBEKG, PANMEMO, ACTMEMO, GBDAILY, ENDOGBN, SUNAP, EXAMREMARK    ");
            parameter.AppendSql("      ,GUIDETEL, AMPM, EXAMCHANGE, NRSABUN, SANGDAMYN, PANDATE, GAPANDATE, RECVDATE, GBAM              ");
            parameter.AppendSql("      ,ETCAM, GAMCODE2, ENDODATE_S, ENDODATE_C, CDATE, VIPREMARK, INBODY, AMPM2, GAMAMT, GBPRIVACY     ");
            parameter.AppendSql("      ,GONGDAN, PANREMARK2, LTDSAMT, GONGDANAMT, TICKET, GBJIKWON, SANGSABUN, MAILWEIGHT               ");
            parameter.AppendSql("      ,GBPDF, PDFPATH, WEBSEND, WEBSENDDATE, STIME, GBNAKSANG, GBMUNJIN, IEMUNNO                       ");
            parameter.AppendSql("      ,TO_CHAR(WEBPRINTREQ, 'YYYY-MM-DD HH24:MI') WEBPRINTREQ                                          ");
            parameter.AppendSql("      ,WEBPRINTSEND, LTDCODE2, KEYNO, PANO2, JUSO1, JUSO2, SANGDAMNOT, PRTSABUN, RECVSABUN             ");
            parameter.AppendSql("      ,GWRTNO, FC_HIC_LTDNAME(LTDCODE) AS LTDNAME, TO_CHAR(ENTTIME, 'HH24:MI') AS ENTTIME_HHMI         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                                                                           ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                           ");
            parameter.AppendSql("   AND WRTNO =:WRTNO                                                                                 ");

            parameter.Add("WRTNO", argwRTNO);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public string GetGbExamByWrtno(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBEXAM                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public string GetSangdamYNbyWrtno(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SANGDAMYN                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int GetCountBySDateSTime(string argSDate, string argSTime)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(WRTNO)                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND STIME =:STIME                               ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("SDATE", argSDate);
            parameter.Add("STIME", argSTime);

            return ExecuteScalar<int>(parameter);
        }

        public void UpDateGbJusoByWrtno(long fnWRTNO, string strGbJUSO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql("   SET GBJUSO = :GBJUSO        ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO         ");

            parameter.Add("GBJUSO", strGbJUSO);
            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public void UpDateGbChk3ByWrtno(long fnWRTNO, string strGbChk3)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql("   SET GBCHK3 = :GBCHK3        ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO         ");

            parameter.Add("GBCHK3", strGbChk3);
            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public void UpDateJusoMailCodeByItem(HEA_JEPSU item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql("   SET MAILCODE = :MAILCODE    ");
            parameter.AppendSql("     , JUSO1 = :JUSO1          ");
            parameter.AppendSql("     , JUSO2 = :JUSO2          ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO         ");

            parameter.Add("MAILCODE", item.MAILCODE);
            parameter.Add("JUSO1", item.JUSO1);
            parameter.Add("JUSO2", item.JUSO2);
            parameter.Add("WRTNO", item.WRTNO);

            ExecuteNonQuery(parameter);
        }

        public string GetResultReceivePositionbyWrtNo(long fnWRTNO, string strSDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CASE WHEN GBCHK3 = 'Y' THEN '방문수령'          ");
            parameter.AppendSql("            WHEN WEBPRINTREQ IS NOT NULL THEN '알림톡' ");
            parameter.AppendSql("            WHEN GBJUSO = 'Y' THEN '집'                ");
            parameter.AppendSql("            WHEN GBJUSO = 'N' THEN '회사'              ");
            parameter.AppendSql("            WHEN WEBPRINTREQ IS NULL THEN '우편'       ");
            parameter.AppendSql("        END AS RECEIVEPOSITION                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                           ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                  ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:JEPDATE, 'yyyy-MM-dd')         ");
            parameter.AppendSql("   AND DELDATE IS NULL                                 ");
            parameter.AppendSql("   AND GBSTS NOT IN ('0','D')                          ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("JEPDATE", strSDate);

            return ExecuteScalar<string>(parameter);
        }

        public void UpDateWebPrintReq(long nWRTNO, bool bOK)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU   ");
            if (bOK)
            {
                parameter.AppendSql("   SET WEBPRINTREQ = SYSDATE   ");
            }
            else
            {
                parameter.AppendSql("   SET WEBPRINTREQ = ''   ");
            }
            
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO         ");

            parameter.Add("WRTNO", nWRTNO);

             ExecuteNonQuery(parameter);
        }

        public void Insert(HEA_JEPSU nHJ)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.HEA_JEPSU (                                                                       ");
            parameter.AppendSql("       WRTNO,JEPDATE,SDATE,PANO,SNAME,SEX,AGE,GJJONG,LTDCODE,REMARK,GAMCODE,SABUN,MAILCODE                 ");
            parameter.AppendSql("      ,JUSO1,JUSO2,PTNO,BURATE,GBSTS,HALINYN,GBNBMD,GBENDO,GBDUST,RESULTSEND                               ");
            parameter.AppendSql("      ,JOBSABUN,ENTTIME,ENDOGBN,EXAMREMARK,SUNAP,GUIDETEL,EXAMCHANGE,AMPM2,GAMAMT,GONGDAN                  ");
            parameter.AppendSql("      ,GONGDANAMT,TICKET,GBJIKWON,STIME,GBNAKSANG,KEYNO,EMAIL,GBCHK3,GBJUSO                                ");
            parameter.AppendSql(" ) VALUES (                                                                                                ");
            parameter.AppendSql("       :WRTNO,TO_DATE(:JEPDATE, 'YYYY-MM-DD'),TO_DATE(:SDATE,'YYYY-MM-DD'),:PANO,:SNAME,:SEX,:AGE,:GJJONG  ");
            parameter.AppendSql("      ,:LTDCODE,:REMARK,:GAMCODE,:SABUN,:MAILCODE                                                          ");
            parameter.AppendSql("      ,:JUSO1,:JUSO2,:PTNO,:BURATE,:GBSTS,:HALINYN,:GBNBMD,:GBENDO,:GBDUST,:RESULTSEND                     ");
            parameter.AppendSql("      ,:JOBSABUN,SYSDATE,:ENDOGBN,:EXAMREMARK,:SUNAP,:GUIDETEL,:EXAMCHANGE,:AMPM2,:GAMAMT,:GONGDAN         ");
            parameter.AppendSql("      ,:GONGDANAMT,:TICKET,:GBJIKWON,:STIME,:GBNAKSANG,:KEYNO,:EMAIL,:GBCHK3,:GBJUSO                       ");
            parameter.AppendSql(" )                                                                                                         ");

            parameter.Add("WRTNO",      nHJ.WRTNO);
            parameter.Add("JEPDATE",    nHJ.JEPDATE);
            parameter.Add("SDATE",      nHJ.SDATE);
            parameter.Add("PANO",       nHJ.PANO);
            parameter.Add("SNAME",      nHJ.SNAME);
            parameter.Add("SEX",        nHJ.SEX);
            parameter.Add("AGE",        nHJ.AGE);
            parameter.Add("GJJONG",     nHJ.GJJONG);
            parameter.Add("LTDCODE",    nHJ.LTDCODE);
            parameter.Add("REMARK",     nHJ.REMARK);
            parameter.Add("GAMCODE",    nHJ.GAMCODE);
            parameter.Add("SABUN",      nHJ.SABUN);
            parameter.Add("MAILCODE",   nHJ.MAILCODE);
            parameter.Add("JUSO1",      nHJ.JUSO1);
            parameter.Add("JUSO2",      nHJ.JUSO2);
            parameter.Add("PTNO",       nHJ.PTNO);
            parameter.Add("BURATE",     nHJ.BURATE);
            parameter.Add("GBSTS",      nHJ.GBSTS);
            parameter.Add("HALINYN",    nHJ.HALINYN);
            parameter.Add("GBNBMD",     nHJ.GBNBMD);
            parameter.Add("GBENDO",     nHJ.GBENDO);
            parameter.Add("GBDUST",     nHJ.GBDUST);
            parameter.Add("RESULTSEND", nHJ.RESULTSEND);
            parameter.Add("JOBSABUN",   nHJ.JOBSABUN);
            parameter.Add("ENDOGBN",    nHJ.ENDOGBN);
            parameter.Add("EXAMREMARK", nHJ.EXAMREMARK);
            parameter.Add("SUNAP",      nHJ.SUNAP);
            parameter.Add("GUIDETEL",   nHJ.GUIDETEL);
            parameter.Add("EXAMCHANGE", nHJ.EXAMCHANGE);
            parameter.Add("AMPM2",      nHJ.AMPM2);
            parameter.Add("GAMAMT",     nHJ.GAMAMT);
            parameter.Add("GONGDAN",    nHJ.GONGDAN);
            parameter.Add("GONGDANAMT", nHJ.GONGDANAMT);
            parameter.Add("TICKET",     nHJ.TICKET);
            parameter.Add("GBJIKWON",   nHJ.GBJIKWON);
            parameter.Add("STIME",      nHJ.STIME);
            parameter.Add("GBNAKSANG",  nHJ.GBNAKSANG);
            parameter.Add("KEYNO",      nHJ.KEYNO);
            parameter.Add("EMAIL",      nHJ.EMAIL);
            parameter.Add("GBCHK3",     nHJ.GBCHK3);
            parameter.Add("GBJUSO",     nHJ.GBJUSO);

            ExecuteNonQuery(parameter);
        }

        public void Update(HEA_JEPSU nHJ)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU       ");
            parameter.AppendSql("   SET SDATE	    =TO_DATE(:SDATE, 'YYYY-MM-DD')	        ");
            parameter.AppendSql("      ,JEPDATE	    =TO_DATE(:JEPDATE, 'YYYY-MM-DD')	    ");
            parameter.AppendSql("      ,STIME	    =:STIME	        ");
            parameter.AppendSql("      ,SNAME	    =:SNAME	        ");
            parameter.AppendSql("      ,SEX		    =:SEX		    ");
            parameter.AppendSql("      ,AGE		    =:AGE		    ");
            parameter.AppendSql("      ,GJJONG	    =:GJJONG	    ");
            parameter.AppendSql("      ,GBSTS	    =:GBSTS	        ");
            parameter.AppendSql("      ,LTDCODE	    =:LTDCODE	    ");
            parameter.AppendSql("      ,REMARK 	    =:REMARK 	    ");
            parameter.AppendSql("      ,GAMCODE     =:GAMCODE       ");
            parameter.AppendSql("      ,GAMAMT 	    =:GAMAMT 	    ");
            parameter.AppendSql("      ,SABUN 	    =:SABUN 	    ");
            parameter.AppendSql("      ,MAILCODE    =:MAILCODE      ");
            parameter.AppendSql("      ,JUSO1	    =:JUSO1	        ");
            parameter.AppendSql("      ,JUSO2	    =:JUSO2	        ");
            parameter.AppendSql("      ,HALINYN	    =:HALINYN	    ");
            parameter.AppendSql("      ,PTNO	    =:PTNO	        ");
            parameter.AppendSql("      ,BURATE	    =:BURATE	    ");
            parameter.AppendSql("      ,GBEXAM 	    =:GBEXAM 	    ");
            parameter.AppendSql("      ,GBNBMD 	    =:GBNBMD 	    ");
            parameter.AppendSql("      ,GBENDO 	    =:GBENDO 	    ");
            parameter.AppendSql("      ,ENDOGBN     =:ENDOGBN       ");
            parameter.AppendSql("      ,GBDUST 	    =:GBDUST 	    ");
            parameter.AppendSql("      ,GONGDAN     =:GONGDAN       ");
            parameter.AppendSql("      ,GONGDANAMT  =:GONGDANAMT    ");
            parameter.AppendSql("      ,RESULTSEND  =:RESULTSEND    ");
            parameter.AppendSql("      ,EXAMREMARK  =:EXAMREMARK    ");
            parameter.AppendSql("      ,SUNAP       =:SUNAP         ");
            parameter.AppendSql("      ,GUIDETEL    =:GUIDETEL      ");
            parameter.AppendSql("      ,EXAMCHANGE  =:EXAMCHANGE    ");
            parameter.AppendSql("      ,AMPM2       =:AMPM2         ");
            parameter.AppendSql("      ,TICKET      =:TICKET        ");
            parameter.AppendSql("      ,GBNAKSANG   =:GBNAKSANG     ");
            parameter.AppendSql("      ,GBJIKWON    =:GBJIKWON      ");
            parameter.AppendSql("      ,KEYNO       =:KEYNO         ");
            parameter.AppendSql("      ,JOBSABUN    =:JOBSABUN      ");
            parameter.AppendSql("      ,EMAIL       =:EMAIL         ");
            parameter.AppendSql("      ,GBCHK3      =:GBCHK3        ");
            parameter.AppendSql("      ,GBJUSO      =:GBJUSO        ");
            if (!nHJ.WEBPRINTREQ.IsNullOrEmpty())
            {
                parameter.AppendSql("      ,WEBPRINTREQ =:WEBPRINTREQ   ");
            }
            
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("SDATE",      nHJ.SDATE);
            parameter.Add("JEPDATE",    nHJ.JEPDATE);
            parameter.Add("STIME",      nHJ.STIME);
            parameter.Add("SNAME",      nHJ.SNAME);
            parameter.Add("SEX",        nHJ.SEX);
            parameter.Add("AGE",        nHJ.AGE);
            parameter.Add("GJJONG",     nHJ.GJJONG);
            parameter.Add("GBSTS",      nHJ.GBSTS);
            parameter.Add("LTDCODE",    nHJ.LTDCODE);
            parameter.Add("REMARK",     nHJ.REMARK);
            parameter.Add("GAMCODE",    nHJ.GAMCODE);
            parameter.Add("GAMAMT",     nHJ.GAMAMT);
            parameter.Add("SABUN",      nHJ.SABUN);
            parameter.Add("MAILCODE",   nHJ.MAILCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUSO1",      nHJ.JUSO1);
            parameter.Add("JUSO2",      nHJ.JUSO2);
            parameter.Add("HALINYN",    nHJ.HALINYN);
            parameter.Add("PTNO",       nHJ.PTNO);
            parameter.Add("BURATE",     nHJ.BURATE);
            parameter.Add("GBEXAM",     nHJ.GBEXAM);
            parameter.Add("GBNBMD",     nHJ.GBNBMD);
            parameter.Add("GBENDO",     nHJ.GBENDO);
            parameter.Add("ENDOGBN",    nHJ.ENDOGBN);
            parameter.Add("GBDUST",     nHJ.GBDUST);
            parameter.Add("GONGDAN",    nHJ.GONGDAN);
            parameter.Add("GONGDANAMT", nHJ.GONGDANAMT);
            parameter.Add("RESULTSEND", nHJ.RESULTSEND);
            parameter.Add("EXAMREMARK", nHJ.EXAMREMARK);
            parameter.Add("SUNAP",      nHJ.SUNAP);
            parameter.Add("GUIDETEL",   nHJ.GUIDETEL);
            parameter.Add("EXAMCHANGE", nHJ.EXAMCHANGE);
            parameter.Add("AMPM2",      nHJ.AMPM2);
            parameter.Add("TICKET",     nHJ.TICKET);
            parameter.Add("GBNAKSANG",  nHJ.GBNAKSANG);
            parameter.Add("GBJIKWON",   nHJ.GBJIKWON);
            parameter.Add("KEYNO",      nHJ.KEYNO);
            parameter.Add("JOBSABUN",   nHJ.JOBSABUN);
            parameter.Add("EMAIL",      nHJ.EMAIL);
            parameter.Add("GBCHK3",     nHJ.GBCHK3);
            parameter.Add("GBJUSO",     nHJ.GBJUSO);
            if (!nHJ.WEBPRINTREQ.IsNullOrEmpty())
            {
                parameter.Add("WEBPRINTREQ", nHJ.WEBPRINTREQ);
            }
            
            parameter.Add("WRTNO",      nHJ.WRTNO);

            ExecuteNonQuery(parameter);
        }

        public void Delete(HEA_JEPSU nHJ)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU       ");
            parameter.AppendSql("   SET GBSTS = :GBSTS              ");
            parameter.AppendSql("     , DELDATE = TRUNC(SYSDATE)    ");
            parameter.AppendSql("     , ENTTIME = SYSDATE           ");
            parameter.AppendSql("     , JOBSABUN = :JOBSABUN        ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("GBSTS", nHJ.GBSTS);
            parameter.Add("JOBSABUN", nHJ.JOBSABUN);
            parameter.Add("WRTNO", nHJ.WRTNO);

            ExecuteNonQuery(parameter);
        }

        public string GetResvRowidByPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO            ");
            parameter.AppendSql("   AND GBSTS IN ('0','1')      ");
            parameter.AppendSql("   AND DELDATE IS NULL         ");

            parameter.Add("PTNO", argPtno);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_JepsuSts(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBSTS                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public void UpDateGbSTSCDate(long nWRTNO, string gstrSysTime, long nGWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql("   SET GBSTS = '1'             ");
            parameter.AppendSql("     , CDATE = :CDATE          ");
            parameter.AppendSql("     , ENTTIME = SYSDATE       ");
            parameter.AppendSql("     , GWRTNO = :GWRTNO        ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("CDATE", gstrSysTime, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("GWRTNO", nGWRTNO);


            ExecuteNonQuery(parameter);
        }

        public string GetRowidBySDateKioskPano(string argCurDate, string argAesjumin)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE,'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND PANO IN (                               ");
            parameter.AppendSql("       SELECT PANO FROM KOSMOS_PMPA.HIC_PATIENT WHERE JUMIN2 =:JUMIN2 ) ");

            parameter.Add("SDATE", argCurDate);
            parameter.Add("JUMIN2", argAesjumin);

            return ExecuteScalar<string>(parameter);
        }

        public HEA_JEPSU GetItemByRid(string argRid)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,TO_CHAR(SDATE,'YYYY-MM-DD') SDATE                   ");
            parameter.AppendSql("      ,TO_CHAR(IDATE,'YYYY-MM-DD') IDATE,PANO,SNAME,SEX,AGE,MAILCODE,GBSTS,GJJONG,BURATE               ");
            parameter.AppendSql("      ,LTDCODE, GAMCODE, SABUN, GBSANGDAM, PANCODE, PANREMARK, DRSABUN, DRNAME, DELDATE, PTNO          ");
            parameter.AppendSql("      ,PRTDATE, MAILDATE, SEXAMS, REMARK, JOBSABUN, ENTTIME, GBEXAM, PANRECODE, MISUNO, HIC_WRTNO      ");
            parameter.AppendSql("      ,HIC_AMT, HALINYN, CARDSEQNO, SANGDAMGBN, GBNBMD, SANGDAMTEL, GBENDO, SANGDAM_ONE                ");
            parameter.AppendSql("      ,GBDUST, RESULTSEND, SANGDAMOUT, GBEKG, PANMEMO, ACTMEMO, GBDAILY, ENDOGBN, SUNAP, EXAMREMARK    ");
            parameter.AppendSql("      ,GUIDETEL, AMPM, EXAMCHANGE, NRSABUN, SANGDAMYN, PANDATE, GAPANDATE, RECVDATE, GBAM              ");
            parameter.AppendSql("      ,ETCAM, GAMCODE2, ENDODATE_S, ENDODATE_C, CDATE, VIPREMARK, INBODY, AMPM2, GAMAMT, GBPRIVACY     ");
            parameter.AppendSql("      ,GONGDAN, PANREMARK2, LTDSAMT, GONGDANAMT, TICKET, GBJIKWON, SANGSABUN, MAILWEIGHT               ");
            parameter.AppendSql("      ,GBPDF, PDFPATH, WEBSEND, WEBSENDDATE, STIME, GBNAKSANG, GBMUNJIN, IEMUNNO, WEBPRINTREQ          ");
            parameter.AppendSql("      ,WEBPRINTSEND, LTDCODE2, KEYNO, PANO2, JUSO1, JUSO2, SANGDAMNOT, PRTSABUN, RECVSABUN             ");
            parameter.AppendSql("      ,GWRTNO, FC_HIC_LTDNAME(LTDCODE) AS LTDNAME, TO_CHAR(ENTTIME, 'HH24:MI') AS ENTTIME_HHMI         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                                                                           ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                           ");
            parameter.AppendSql("   AND ROWID =:RID                                                                                 ");

            parameter.Add("RID", argRid);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public HEA_JEPSU GetItemByGWrtno(object argGWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,TO_CHAR(SDATE,'YYYY-MM-DD') SDATE                   ");
            parameter.AppendSql("      ,TO_CHAR(IDATE,'YYYY-MM-DD') IDATE,PANO,SNAME,SEX,AGE,MAILCODE,GBSTS,GJJONG,BURATE               ");
            parameter.AppendSql("      ,LTDCODE, GAMCODE, SABUN, GBSANGDAM, PANCODE, PANREMARK, DRSABUN, DRNAME, DELDATE, PTNO          ");
            parameter.AppendSql("      ,PRTDATE, MAILDATE, SEXAMS, REMARK, JOBSABUN, ENTTIME, GBEXAM, PANRECODE, MISUNO, HIC_WRTNO      ");
            parameter.AppendSql("      ,HIC_AMT, HALINYN, CARDSEQNO, SANGDAMGBN, GBNBMD, SANGDAMTEL, GBENDO, SANGDAM_ONE                ");
            parameter.AppendSql("      ,GBDUST, RESULTSEND, SANGDAMOUT, GBEKG, PANMEMO, ACTMEMO, GBDAILY, ENDOGBN, SUNAP, EXAMREMARK    ");
            parameter.AppendSql("      ,GUIDETEL, AMPM, EXAMCHANGE, NRSABUN, SANGDAMYN, PANDATE, GAPANDATE, RECVDATE, GBAM              ");
            parameter.AppendSql("      ,ETCAM, GAMCODE2, ENDODATE_S, ENDODATE_C, CDATE, VIPREMARK, INBODY, AMPM2, GAMAMT, GBPRIVACY     ");
            parameter.AppendSql("      ,GONGDAN, PANREMARK2, LTDSAMT, GONGDANAMT, TICKET, GBJIKWON, SANGSABUN, MAILWEIGHT               ");
            parameter.AppendSql("      ,GBPDF, PDFPATH, WEBSEND, WEBSENDDATE, STIME, GBNAKSANG, GBMUNJIN, IEMUNNO, WEBPRINTREQ          ");
            parameter.AppendSql("      ,WEBPRINTSEND, LTDCODE2, KEYNO, PANO2, JUSO1, JUSO2, SANGDAMNOT, PRTSABUN, RECVSABUN             ");
            parameter.AppendSql("      ,GWRTNO, FC_HIC_LTDNAME(LTDCODE) AS LTDNAME, TO_CHAR(ENTTIME, 'HH24:MI') AS ENTTIME_HHMI         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                                                                           ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                           ");
            parameter.AppendSql("   AND GWRTNO =:GWRTNO                                                                                 ");

            parameter.Add("WRTNO", argGWRTNO);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public long GetJepCountBySDate(string argDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(PANO) CNT                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND GBSTS != 'D'                            ");

            parameter.Add("SDATE", argDate);

            return ExecuteScalar<long>(parameter);
        }

        public List<HEA_JEPSU> GetListEndoByRTime(string argCurDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO, a.SNAME, a.PANO, b.AMPM, a.PTNO        ");
            parameter.AppendSql("      ,TO_CHAR(b.RTIME,'HH24:MI') RTIME                ");
            parameter.AppendSql("      ,TO_CHAR(a.SDATE,'YYYY-MM-DD') SDATE             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a                         ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HEA_RESV_EXAM b                     ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            parameter.AppendSql("   AND TRUNC(b.RTIME) = TO_DATE(:RTIME, 'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                               ");
            parameter.AppendSql("   AND a.SDATE = b.SDATE                               ");
            parameter.AppendSql("   AND a.PANO = b.PANO                                 ");
            parameter.AppendSql("   AND b.EXCODE IN ('TX32', 'TX64', 'TX41')            ");
            parameter.AppendSql(" ORDER BY b.RTIME                                      ");

            parameter.Add("RTIME", argCurDate);

            return ExecuteReader<HEA_JEPSU>(parameter);
        }

        public string GetAmPm2bySDatePano(string argCurDate, long nPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT AMPM2                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE,'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND PANO =:PANO                             ");
            parameter.AppendSql("   AND GBSTS != 'D'                            ");

            parameter.Add("SDATE", argCurDate);
            parameter.Add("PANO", nPano);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdatebyRowId(HEA_JEPSU item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU                           ");
            parameter.AppendSql("   SET GBSANGDAM = :GBSANGDAM                          ");
            parameter.AppendSql("     , IDATE     = TO_DATE(:IDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("     , MAILDATE  = TO_DATE(:MAILDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql(" WHERE ROWID     = :RID                                ");

            parameter.Add("GBSANGDAM", item.GBSANGDAM);
            parameter.Add("IDATE", item.IDATE);
            parameter.Add("MAILDATE", item.MAILDATE);
            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateGbKg(string strGbEkg, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql("   SET GBEKG = :GBEKG          ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("GBEKG", strGbEkg, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateSangdamByWrtno(string strSangdam, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU       ");
            parameter.AppendSql("   SET SANGDAM = :SANGDAM          ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("SANGDAM", strSangdam);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateGbSangdambyRowId(string strEntTime, string strROWID, string strGbSangdam)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU                                       ");
            parameter.AppendSql("   SET GBSANGDAM = :GBSANGDAM                                      ");
            if (strGbSangdam == "A")
            {
                parameter.AppendSql("     , ENTTIME   = TO_DATE(:ENTTIME, 'YYYY-MM-DD HH24:MI:SS')  ");
            }
            else
            {
                parameter.AppendSql("     , ENTTIME   = TO_DATE(:ENTTIME, 'YYYY-MM-DD HH24:MI')     ");
            }
            parameter.AppendSql(" WHERE ROWID     = :RID                                            ");

            parameter.Add("GBSANGDAM", strEntTime, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTTIME", strGbSangdam);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public long GetWrtNobyHeaPaNoJepDateGbsts(long nHeaPano, string jEPDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO FROM KOSMOS_PMPA.HEA_JEPSU                ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("   AND DelDate IS NULL                                 ");
            parameter.AppendSql("   AND Gbsts NOT IN ( 'D' ,'0' )                       ");
            parameter.AppendSql(" ORDER BY SDATE DESC                                   ");

            parameter.Add("PANO", nHeaPano);
            parameter.Add("SDATE", jEPDATE);

            return ExecuteScalar<long>(parameter);
        }

        public long GetWrtNobySDatePtNo(string argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO FROM KOSMOS_PMPA.HEA_JEPSU    ");
            parameter.AppendSql(" WHERE SDATE >= TRUNC(SYSDATE)             ");
            parameter.AppendSql("   AND SDATE <  TRUNC(SYSDATE+1)           ");
            parameter.AppendSql("   AND Ptno   = :PTNO                      ");

            parameter.Add("PTNO", argWrtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<long>(parameter);
        }

        public int GetCountbyPtNoJepDate(string fstrPtno, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT FROM KOSMOS_PMPA.HEA_JEPSU       ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                    ");
            parameter.AppendSql("   AND GBSTS NOT IN ('0','D')                          ");
            parameter.AppendSql("   AND SDATE <= TO_DATE(:SDATE, 'YYYY-MM-DD')          ");

            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SDATE", fstrJepDate);

            return ExecuteScalar<int>(parameter);
        }

        public long GetWrtNobyHeaPaNoJepDate(long nHeaPano, string strStartJepDate, string strJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE PANO  = :PANO                           ");
            parameter.AppendSql("   AND SDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND SDate <= TO_DATE(:TODATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND GBSTS NOT IN ( 'D' ,'0' )               ");
            parameter.AppendSql(" ORDER BY SDATE DESC                           ");

            parameter.Add("PANO", nHeaPano);
            parameter.Add("FRDATE", strStartJepDate);
            parameter.Add("TODATE", strJepDate);

            return ExecuteScalar<long>(parameter);
        }

        public HEA_JEPSU GetDrSabunDrNmaebyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DRSABUN, DRNAME, PRTDATE, WEBPRINTSEND      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public int UpdateDrSabunbyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU SET   ");
            parameter.AppendSql("       DRSABUN    = 0              ");
            parameter.AppendSql("     , DRNAME     = ''             ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO         ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateSangdamOutbyWrtNo(string strWorkTemp, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU SET   ");
            parameter.AppendSql("       SANGDAMOUT = :SANGDAMOUT    ");
            parameter.AppendSql("     , SANGDAMNOT = ''             ");
            parameter.AppendSql("     , SANGDAMTEL = ''             ");
            parameter.AppendSql("     , SANGDAMYN  = ''             ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO         ");

            parameter.Add("SANGDAMOUT", strWorkTemp);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateSangdamGbnbyWrtNo(string strDate, string strSangDam_One, string strPanRemark, string strPanRemark2, string strAm, string strAmETC, string strSabun, string strPanReCode, long nWrtNo, string strChk, string strSysDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU SET                                   ");
            parameter.AppendSql("       SANGDAM_ONE = :SANGDAM_ONE                                  ");
            parameter.AppendSql("     , PANREMARK   = :PANREMARK                                    ");
            if (strDate != "")
            {
                parameter.AppendSql("     , SANGDAMGBN = :SANGDAMGBN                                ");
            }
            if (strPanRemark2 != "")
            {
                parameter.AppendSql("     , PANREMARK2 = :PANREMARK2                                ");
            }
            else
            {
                parameter.AppendSql("     , PANREMARK2 = ''                                         ");
            }

            parameter.AppendSql("     , GBAM   = :GBAM                                              ");
            parameter.AppendSql("     , ETCAM  = :ETCAM                                             ");
            if (strChk == "Y")
            {
                parameter.AppendSql("     , GbSTS     = '5'                                         ");
                parameter.AppendSql("     , NRSABUN   = :NRSABUN                                    ");
                parameter.AppendSql("     , GAPANDATE = TO_DATE(:GAPANDATE, 'YYYY-MM-DD HH24:MI')   ");
            }
            else
            {
                parameter.AppendSql("     , GbSTS     = '3'                                         ");
                parameter.AppendSql("     , NRSABUN   = 0                                           ");
                parameter.AppendSql("     , GAPANDATE = ''                                          ");
            }
            parameter.AppendSql("     , PANRECODE  = :PANRECODE                                     ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO                                         ");

            if (strDate != "")
            {
                parameter.Add("SANGDAMGBN", strDate);
            }
            if (strPanRemark2 != "")
            {
                parameter.Add("PANREMARK2", strPanRemark2);
            }
            parameter.Add("SANGDAM_ONE", strSangDam_One, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANREMARK", strPanRemark);
            
            parameter.Add("GBAM", strAm);
            parameter.Add("ETCAM", strAmETC);
            if (strChk == "Y")
            {
                parameter.Add("NRSABUN", strSabun);
                parameter.Add("GAPANDATE", strSysDate);
            }
            parameter.Add("PANRECODE", strPanReCode);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateSangdamGbnGbAmbyWrtNo(string strDate, string strAm, string strEtcAm, string strSabun, string strSangDam_One, long nWrtNo, string strSangDam)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU SET   ");
            parameter.AppendSql("       SANGDAMGBN  = :SANGDAMGBN   ");
            parameter.AppendSql("     , GBAM        = :GBAM         ");
            parameter.AppendSql("     , ETCAM       = :ETCAM        ");
            parameter.AppendSql("     , SANGSABUN   = :SANGSABUN    ");
            parameter.AppendSql("     , SANGDAM_ONE = :SANGDAM_ONE  ");
            parameter.AppendSql("     , SANGDAM     = :SANGDAM      ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO        ");

            parameter.Add("SANGDAMGBN", strDate);
            parameter.Add("GBAM", strAm);
            parameter.Add("ETCAM", strEtcAm);
            parameter.Add("SANGSABUN", strSabun);
            parameter.Add("SANGDAM_ONE", strSangDam_One, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SANGDAM", strSangDam);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public string GetSangdamGbnbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SANGDAMGBN                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO            ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateSangdamNotbyWrtNo(string strWorkTemp, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU SET       ");
            if (!strWorkTemp.IsNullOrEmpty())
            {
                parameter.AppendSql("       SANGDAMNOT = :SANGDAMNOT    ");
            }
            else
            {
                parameter.AppendSql("       SANGDAMNOT = ''             ");
            }
            parameter.AppendSql("     , SANGDAMTEL = ''                 ");
            parameter.AppendSql("     , SANGDAMOUT = ''                 ");
            parameter.AppendSql("     , SANGDAMYN = ''                 ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO             ");

            if (!strWorkTemp.IsNullOrEmpty())
            {
                parameter.Add("SANGDAMNOT", strWorkTemp);
            }
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public string GetSangdamNotbyWrtno(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SANGDAMNOT                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public string GetSangdamOutbyWrtno(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SANGDAMOUT                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateNrSabunbyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU SET   ");
            parameter.AppendSql("       NRSABUN    = 0              ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO         ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public long GetDrSabunbyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DRSABUN                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU           ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<long>(parameter);
        }

        public int UpdateSangdamYNbyWrtNo(string strWorkTemp, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU SET           ");
            parameter.AppendSql("       SANGDAMYN    = :SANGDAMYN           ");
            parameter.AppendSql("     , SANGDAMNOT   = ''                   ");
            parameter.AppendSql("     , SANGDAMOUT   = ''                   ");
            parameter.AppendSql("     , SANGDAMTEL    = ''                   ");
            parameter.AppendSql(" WHERE WRTNO        = :WRTNO               ");

            parameter.Add("SANGDAMYN", strWorkTemp);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateSangdamTelbyWrtNo(string strWorkTemp, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU SET           ");
            if (strWorkTemp.IsNullOrEmpty())
            {
                parameter.AppendSql("       SANGDAMTEL   = ''               ");
            }
            else
            {
                parameter.AppendSql("       SANGDAMTEL   = :SANGDAMTEL      ");
            }
            parameter.AppendSql("     , SANGDAMNOT   = ''                   ");
            parameter.AppendSql("     , SANGDAMOUT   = ''                   ");
            parameter.AppendSql("     , SANGDAMYN    = ''                   ");

            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                     ");

            if (!strWorkTemp.IsNullOrEmpty())
            {
                parameter.Add("SANGDAMTEL", strWorkTemp);
            }
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateGbAmETCAmbyWrtNo(string strAm, string strAmEtc, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql("   SET GBAM   = :GBAM          ");
            parameter.AppendSql("     , ETCAM  = :ETCAM         ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO         ");

            parameter.Add("GBAM", strAm);
            parameter.Add("ETCAM", strAmEtc);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatepanRecode(string gstrRefValue1, string strPanRemark, string strPanRemark2, string strAm, string strEtcAm, string idNumber, string userName, string gstrSysDate, string gstrSysTime, long nWrtNo, string strChkYN)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU                                   ");
            parameter.AppendSql("   SET PANRECODE = :PANRECODE                                  ");
            parameter.AppendSql("     , PANREMARK = :PANREMARK                                  ");
            if (strPanRemark2 != "")
            {
                parameter.AppendSql("     , PANREMARK2 = :PANREMARK2                            ");
            }
            else
            {
                parameter.AppendSql("     , PANREMARK2 = :PANREMARK2                            ");
            }
            parameter.AppendSql("     , GBAM = :GBAM                                            ");
            parameter.AppendSql("     , ETCAM = :ETCAM                                          ");
            if (strChkYN == "Y")
            {
                parameter.AppendSql("     , GbSTS = '9'                                         ");
                parameter.AppendSql("     , DRSABUN = :DRSABUN                                  ");
                parameter.AppendSql("     , DRNAME = :DRNAME                                    ");
                parameter.AppendSql("     , PANDATE = TO_DATE(:PANDATE, 'YYYY-MM-DD HH24:MI')   ");
            }
            else
            {
                parameter.AppendSql("     , GbSTS = '5'                                         ");
                parameter.AppendSql("     , DrSabun = 0                                         ");
                parameter.AppendSql("     , DrName = ''                                         ");
                parameter.AppendSql("     , PanDate = ''                                        ");
            }
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                          ");

            parameter.Add("PANRECODE", gstrRefValue1);
            parameter.Add("PANREMARK", strPanRemark);
            parameter.Add("PANREMARK2", strPanRemark2);
            parameter.Add("GBAM", strAm);
            parameter.Add("ETCAM", strEtcAm);
            if (strChkYN == "Y")
            {
                parameter.Add("DRSABUN", idNumber);
                parameter.Add("DRNAME", userName);
                parameter.Add("PANDATE", gstrSysDate + " " + gstrSysTime);
            }
            
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_JEPSU> GetItembySDateLtdCode(string strFrDate, string strToDate, long strLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,SName,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,GjJong,LtdCode    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                                               ");
            parameter.AppendSql(" WHERE SDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                             ");
            parameter.AppendSql("   AND SDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                             ");
            parameter.AppendSql("   AND DelDate IS NULL                                                     ");
            if (strLtdCode != 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                              ");
            }

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (strLtdCode != 0)
            {
                parameter.Add("LTDCODE", strLtdCode);
            }

            return ExecuteReader<HEA_JEPSU>(parameter);
        }

        public string GetSangdamTelbyWrtno(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SANGDAMTEL              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public List<HEA_JEPSU> GetItembyPaNoSDateGbSts(long nPano, string strSDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(SDate, 'YYYY-MM-DD') SDate,WRTNO,PanCode,PanRemark,DrName, SangDam   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                                               ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                        ");
            parameter.AppendSql("   AND SDATE < TO_DATE(:SDATE, 'YYYY-MM-DD')                               ");
            parameter.AppendSql("   AND DelDate IS NULL                                                     ");
            parameter.AppendSql("   AND GbSts NOT IN ('0','D')                                               ");
            parameter.AppendSql(" ORDER BY SDate DESC                                                       ");

            parameter.Add("PANO", nPano);
            parameter.Add("SDATE", strSDate);

            return ExecuteReader<HEA_JEPSU>(parameter);
        }

        public List<HEA_JEPSU> GetItembyPaNo(long fnPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PTNO, WRTNO, PANO, SNAME, AGE, SEX, LTDCODE     ");
            parameter.AppendSql("     , TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, PANMEMO      ");
            parameter.AppendSql("     , GJJONG, PANCODE, PANREMARK, DRSABUN, DRNAME     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                           ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE = '')               ");
            parameter.AppendSql(" ORDER BY SDATE DESC                                   ");

            parameter.Add("PANO", fnPano);

            return ExecuteReader<HEA_JEPSU>(parameter);
        }

        public List<HEA_JEPSU> GetItembyPaNoSDate(long nPano, string strSDate, string strGbStsYN = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(SDate, 'YYYY-MM-DD') SDate,WRTNO,PanCode,PanRemark,DrName   ");
            parameter.AppendSql("     , SNAME, SEX, AGE, PTNO                                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                                               ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                        ");
            parameter.AppendSql("   AND SDATE < TO_DATE(:SDATE, 'YYYY-MM-DD')                               ");
            if (strGbStsYN == "Y")
            {
                parameter.AppendSql("   AND GbSTS IN ('3','9')                                              "); //입력완료/판정완료
            }
            parameter.AppendSql(" ORDER BY SDate DESC                                                       ");

            parameter.Add("PANO", nPano);
            parameter.Add("SDATE", strSDate);

            return ExecuteReader<HEA_JEPSU>(parameter);
        }

        public string GetSexbyWrtNo(string argJepNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SEX                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", argJepNo);

            return ExecuteScalar<string>(parameter);
        }

        public HEA_JEPSU GetPrtDatebyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PRTDATE, TO_CHAR(WEBPRINTSEND, 'YYYY-MM-DD HH24:MI:SS') WEBPRINTSEND ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public HEA_JEPSU GetWrtNobySDatebyPtNo(string argPano, string strSDate, string strEdate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                ");
            parameter.AppendSql("   AND SDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')     ");
            parameter.AppendSql("   AND SDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')     ");
            parameter.AppendSql("   AND GBSTS NOT IN ('0','D','9')                  ");

            parameter.Add("PTNO", argPano);
            parameter.Add("FRDATE", strSDate);
            parameter.Add("TODATE", strEdate);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public long GetWrtNobySDatePano(string argPano, string argDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                            ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND GBSTS NOT IN ('0','D','9')              ");
            parameter.AppendSql("   AND DelDate IS NULL                         ");

            parameter.Add("PTNO", argPano);
            parameter.Add("SDATE", argDate);

            return ExecuteScalar<long>(parameter);
        }

        public List<HEA_JEPSU> GetItembyLtdCode(string strFrDate, string strToDate, string strLtdCode, string strGubun, string argSName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO, a.SName, TO_CHAR(a.SDate,'YY-MM-DD') SDate, a.GjJong           ");
            parameter.AppendSql("     , a.GBEKG, a.SangDamGbn, a.Gbsts, a.PrtDate, a.SangDam_One, a.SangSabun   ");
            parameter.AppendSql("     , TO_CHAR(a.WEBPRINTSEND, 'YYYY-MM-DD HH24:MI') WEBPRINTSEND              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a                                                 ");
            parameter.AppendSql(" WHERE a.SDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                               ");
            parameter.AppendSql("   AND a.SDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                               ");
            parameter.AppendSql("   AND a.GbSTS IN ('1','2','3','5','9')                                        ");
            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                  ");
            }
            parameter.AppendSql("   AND DELDATE IS NULL                                                         ");

            if (!argSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SNAME LIKE :SNAME                                                   ");
            }

            if (strGubun == "1")
            {
                parameter.AppendSql(" ORDER BY SName, WRTNO                                                     ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql(" ORDER BY WRTNO                                                            ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql(" ORDER BY SDate, SName                                                     ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql(" ORDER BY GjJong, SName                                                    ");
            }
            else
            {
                parameter.AppendSql(" ORDER BY SName, WRTNO                                                     ");
            }

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", strLtdCode);
            }

            if (!argSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", argSName);
            }

            return ExecuteReader<HEA_JEPSU>(parameter);
        }

        public HEA_JEPSU GetItembyWrtNoGbSts(long fnWRTNO, string strGbSts)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO, SNAME, AGE, SEX, LTDCODE, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE           ");
            parameter.AppendSql("     , GJJONG, PANCODE, PANREMARK, DRSABUN, DRNAME, SANGDAMGBN, SANGDAMTEL         ");
            parameter.AppendSql("     , SANGDAM_ONE, PTNO, JOBSABUN, PANMEMO, NRSABUN, SANGDAMYN, GBAM, ETCAM       ");
            parameter.AppendSql("     , TO_CHAR(PANDATE,'YYYY-MM-DD HH24:MI') PANDATE, SANGDAMOUT, SANGDAM          ");
            parameter.AppendSql("     , TO_CHAR(GAPANDATE,'YYYY-MM-DD HH24:MI') GAPANDATE, PANREMARK2, WRTNO        ");
            parameter.AppendSql("     , IEMUNNO, SANGSABUN, SANGDAMNOT, FC_HC_PATIENT_JUMINNO(PTNO) AS BIRTHDAY     ");
            parameter.AppendSql("     , WEBPRINTREQ, GBJUSO, GBCHK3, ROWID AS RID                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND GBSTS <> :GBSTS                         ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("GBSTS", strGbSts, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public HEA_JEPSU GetItembyPtNoBdate(string strPTNO1, string strBDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME, SEX, AGE, WRTNO, PTNO, TO_CHAR(JEPDATE, 'YYYY-MM-DD') JEPDATE, LTDCODE            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                            ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')   ");

            parameter.Add("PTNO", strPTNO1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strBDATE);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public string GetAmPm2byWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT AMPM2                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public HEA_JEPSU GetWrtNoAgebyPtNo(string strPtNo, string strSDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, AGE                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                           ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SDATE", strSDate);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public int GetCountbyPtNo(string pTNO, string strSDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                           ");
            parameter.AppendSql("   AND SDATE >= TO_DATE(:SDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");

            parameter.Add("PTNO", pTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SDATE", strSDate);

            return ExecuteScalar<int>(parameter);
        }

        public HEA_JEPSU GetWrtNobyPtNo(string strPtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO           ");
            parameter.AppendSql("   AND SDATE = TRUNC(SYSDATE)  ");
            parameter.AppendSql("   AND GBSTS <> 'D'            ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public HEA_JEPSU GetSnamebyPano(string strPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME,JEPDATE           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql(" WHERE PANO  = :PANO           ");
            parameter.AppendSql("   AND GBSTS <> 'D'            ");

            parameter.Add("PANO", strPano, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public List<HEA_JEPSU> GetListByItems(string argFDate, string argTDate, string argSTS, string argJong, string argSName, long argLtdCode, bool fnPrvcy = false, bool bGPan = false, bool bPan = false, bool bPrt = false, bool bBal = false)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT KOSMOS_PMPA.FC_CHECK_HC_ENDOJUPMST(a.PTNO, TO_CHAR(a.JEPDATE,'YYYY-MM-DD')) AS ENDOGBN, a.STIME                                         ");
            parameter.AppendSql("      ,TO_CHAR(a.SDATE,'YYYY-MM-DD') SDATE, a.GJJONG, a.PANO, a.SNAME, a.SEX, a.AGE, a.GBSTS, a.PTNO, a.GBDAILY, a.GBDAILY AS GBDAILY2, a.AMPM ");
            parameter.AppendSql("      ,DECODE(a.AMPM2, '2', '오후', '') AS AMPM2, a.AGE || '/' || a.SEX AS AGESEX, b.FAMILLY, b.BUSENAME, b.JUMIN2                             ");
            parameter.AppendSql("      ,DECODE(a.GBSTS, '0','예약접수', '1','수검등록', '2','결과입력중', '3','결과입력완료', '5','가판정완료', '9','판정완료', 'D','접수취소') AS GBSTS_NM     ");
            parameter.AppendSql("      ,TO_CHAR(a.IDATE,'YYYY-MM-DD') IDATE, a.GBMUNJIN, a.IEMUNNO, TO_CHAR(a.ENTTIME,'HH24:MI') AS ENTTIME_HHMI, a.CDATE, a.GWRTNO             ");
            parameter.AppendSql("      ,a.WRTNO, a.LTDCODE, a.PTNO, a.EXAMCHANGE, a.GUIDETEL, FC_HIC_LTDNAME(a.LTDCODE) AS LTDNAME                                              ");
            parameter.AppendSql("      ,DECODE(a.SUNAP, 'Y', '완납', 'N', '미납', '') AS SUNAP, TO_CHAR(a.JEPDATE,'YYYY-MM-DD') AS JEPDATE                                      ");
            parameter.AppendSql("      ,a.MAILDATE, DECODE(SUBSTR(a.GJJONG, 1, 1), '1', '개인', '단체') AS JONG_GB                                                              ");
            parameter.AppendSql("      ,TO_CHAR(a.GAPANDATE, 'YYYY-MM-DD') || CHR(10) || KOSMOS_OCS.FC_INSA_MST_KORNAME(a.NRSABUN) AS GAPAN_INFO                                ");
            parameter.AppendSql("      ,TO_CHAR(a.PRTDATE, 'YYYY-MM-DD') || CHR(10) || KOSMOS_OCS.FC_INSA_MST_KORNAME(a.PRTSABUN) AS PRT_INFO                                   ");
            parameter.AppendSql("      ,TO_CHAR(a.RECVDATE, 'YYYY-MM-DD') || CHR(10) || KOSMOS_OCS.FC_INSA_MST_KORNAME(a.RECVSABUN) AS RECV_INFO                                ");
            parameter.AppendSql("      ,TO_CHAR(a.PANDATE, 'YYYY-MM-DD') || CHR(10) || DRNAME AS PAN_INFO, TO_CHAR(a.WEBPRINTSEND, 'YYYY-MM-DD') AS WEBPRINTSEND                ");
            parameter.AppendSql("      ,b.HPHONE, b.JUSO1 || b.JUSO2 AS JUSO, DECODE(a.WEBPRINTREQ, '', '', '○') AS WEBPRINTREQ                                                 ");
            parameter.AppendSql("      ,KOSMOS_PMPA.FC_HIC_WAIT_JEPTIME(b.JUMIN2, TO_CHAR(a.SDATE, 'YYYY-MM-DD')) AS WAITDATE, TO_CHAR(a.RECVDATE, 'YYYY-MM-DD') RECVDATE       ");
            parameter.AppendSql("      ,KOSMOS_OCS.FC_INSA_MST_KORNAME(a.JOBSABUN) AS JOBNAME                                                                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a                                                                                                                 ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT b                                                                                                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                                                                   ");
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                                                                                                                      ");
            parameter.AppendSql("   AND a.SDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                                                                                                ");
            parameter.AppendSql("   AND a.SDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')                                                                                                ");
            //parameter.AppendSql("   AND a.DELDATE IS NULL                                                                                                                       ");
            if (argSTS == "예약")
            {
                parameter.AppendSql("   AND a.GBSTS = '0'                                                                                                               ");
            }
            else if (argSTS == "접수")
            {
                parameter.AppendSql("   AND a.GBSTS NOT IN ('0', 'D')                                                                                                     ");
            }
            else
            {
                parameter.AppendSql("   AND a.GBSTS =:GBSTS                                                                                                               ");
            }

            if (!argJong.Equals("**"))
            {
                parameter.AppendSql("   AND a.GJJONG = :GJJONG                                                                    ");
            }

            if (!argSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME LIKE  :SNAME                                                                  ");
            }

            if (argLtdCode > 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                  ");
            }
            //개인정보동의
            if (fnPrvcy)
            {
                parameter.AppendSql("   AND b.GBPRIVACY IS NULL                                                                   ");
            }
            //가판정 미비
            if (bGPan)
            {
                parameter.AppendSql("   AND (a.NRSABUN IS NULL OR a.NRSABUN = 0)                                                      ");
            }
            //판정 미비
            if (bPan)
            {
                parameter.AppendSql("   AND (a.DRSABUN IS NULL OR a.DRSABUN = 0)                                                      ");
            }
            //출력 미비
            if (bPrt)
            {
                parameter.AppendSql("   AND ((a.PRTDATE IS NULL OR a.PRTDATE = '') AND (a.WebPrintSend IS NULL OR a.WebPrintSend = ''))       ");
            }
            //발송 미비
            if (bBal)
            {
                parameter.AppendSql("   AND ((a.MailDATE IS NULL OR a.MailDATE = '') AND (a.RecvDATE IS NULL OR a.RecvDATE = '') AND (a.WebPrintSend IS NULL OR a.WebPrintSend = ''))     ");
            }
            parameter.AppendSql("  ORDER BY SDate,SName,LtdCode,Sex  ");

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);

            if (argSTS == "예약")
            {

            }
            else if (argSTS == "접수")
            {

            }
            else
            {
                parameter.Add("GBSTS", argSTS);
            }

            if (!argJong.Equals("**"))
            {
                parameter.Add("GJJONG", argJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (!argSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", argSName);
            }

            if (argLtdCode > 0)
            {
                parameter.Add("LTDCODE", argLtdCode);
            }

            return ExecuteReader<HEA_JEPSU>(parameter);
        }

        public int UpdateRecVDate(HEA_JEPSU item2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql("   SET RECVDATE = TO_DATE(:RECVDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("     , RECVSABUN = :RECVSABUN                      ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                           ");

            parameter.Add("RECVDATE", item2.RECVDATE);
            parameter.Add("RECVSABUN", item2.RECVSABUN);
            parameter.Add("WRTNO", item2.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateMailInfo(HEA_JEPSU item1)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU SET                       ");
            parameter.AppendSql("       MAILWEIGHT = :MAILWEIGHT                        ");
            parameter.AppendSql("     , MAILDATE   = TO_DATE(:MAILDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO                             ");

            parameter.Add("WRTNO", item1.WRTNO);
            parameter.Add("MAILWEIGHT", item1.MAILWEIGHT);
            parameter.Add("MAILDATE", item1.MAILDATE);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateMailDatebyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU       ");
            parameter.AppendSql("   SET MAILDATE = ''               ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO           ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateRevcDatebyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU       ");
            parameter.AppendSql("   SET RECVDATE = ''               ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO           ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateMailWeightbyWrtNo(long nMailWeight, long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU       ");
            parameter.AppendSql("   SET MAILWEIGHT = :MAILWEIGHT    ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO         ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("MAILWEIGHT", nMailWeight);

            return ExecuteNonQuery(parameter);
        }

        public HEA_JEPSU GetMailCodebyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAILCODE,JUSO1 || JUSO2 as JUSO, SNAME       ");
            parameter.AppendSql("     , TO_CHAR(RECVDATE,'YYYY-MM-DD') RECVDATE     ");
            parameter.AppendSql("     , TO_CHAR(MAILDATE,'YYYY-MM-DD') MAILDATE     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public long GetWrtNobyJepDate(string strPtNo, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                       ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                               ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            parameter.AppendSql("   AND Gbsts NOT IN( 'D' ,'0' )                    ");
            parameter.AppendSql(" ORDER BY SDATE DESC                               ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", fstrJepDate);

            return ExecuteScalar<long>(parameter);
        }

        public string GetEndoGbnbyPtNo(string pTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ENDOGBN                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU       ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO               ");
            parameter.AppendSql("   AND SDATE = TRUNC(SYSDATE)      ");
            parameter.AppendSql("   AND DELDATE IS NULL             ");

            parameter.Add("PTNO", pTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public HEA_JEPSU GetSexAgebyPtNo(string strPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SEX, AGE                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                           ");
            parameter.AppendSql("   AND SDATE = TRUNC(SYSDATE)                  ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");

            parameter.Add("PTNO", strPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public List<HEA_JEPSU> GetWrtNobySDate(string strSDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, PTNO, AMPM2, SNAME, PANO         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'yyyy-mm-dd')   ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql(" ORDER By AMPM2, SNAME                         ");

            parameter.Add("SDATE", strSDate);

            return ExecuteReader<HEA_JEPSU>(parameter);
        }

        public HEA_JEPSU GetItembyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO, SNAME, AGE, SEX, LTDCODE, GbSTS, WRTNO    ");
            parameter.AppendSql("     , TO_CHAR(SDATE, 'YYYY-MM-DD') JEPDATE, GJJONG    ");
            parameter.AppendSql("     , MAILCODE, JUSO1 || JUSO2 as JUSO                ");
            parameter.AppendSql("     , TO_CHAR(RECVDATE,'YYYY-MM-DD') RECVDATE         ");
            parameter.AppendSql("     , TO_CHAR(MAILDATE,'YYYY-MM-DD') MAILDATE         ");
            parameter.AppendSql("     , PTNO, GBEKG, AGE, IEMUNNO                       ");
            parameter.AppendSql("     , PANCODE, PANREMARK, DRSABUN, DRNAME             ");
            parameter.AppendSql("     , TO_CHAR(SDATE, 'YYYY-MM-DD') SDATE              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                           ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                 ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public HEA_JEPSU GetHeaJepsubyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO, SNAME, AGE || '/' || SEX SEXAGE, LTDCODE                  ");
            parameter.AppendSql("     , TO_CHAR(SDATE, 'YYYY-MM-DD') SDATE                              ");
            parameter.AppendSql("     , GJJONG || '.' || KOSMOS_PMPA.FC_HEA_GJJONG_NAME(GJJONG) GJNAME  ");
            parameter.AppendSql("     , GJJONG, TO_CHAR(JEPDATE, 'YYYY-MM-DD') JEPDATE, PTNO            ");
            //parameter.AppendSql("     , PTNO, GBSTS, GBEKG, PANMEMO                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                                           ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                                 ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public int GetCountbyWrtNo(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                         ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public long GetWrtNobyPano(long nHeaPano, string fstrGjYear, string strJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE PANO  = :PANO                           ");
            parameter.AppendSql("   AND SDate >= TO_DATE(:SDATEFR,'YYYY-MM-DD') ");
            parameter.AppendSql("   AND SDate <= TO_DATE(:SDATETO,'YYYY-MM-DD') ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND GBSTS NOT IN ( 'D' ,'0' )               ");
            parameter.AppendSql(" ORDER By SDate DESC                           ");

            parameter.Add("PANO", nHeaPano);
            parameter.Add("SDATEFR", fstrGjYear);
            parameter.Add("SDATETO", strJepDate);

            return ExecuteScalar<long>(parameter);
        }

        public HEA_JEPSU GetSumAmPmCount(string argDate, long argLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(DECODE(AMPM2,'1',1,1)) AMCNT    ");
            parameter.AppendSql("     , SUM(DECODE(AMPM2,'1',0,1)) PMCNT    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU               ");
            parameter.AppendSql(" WHERE SDATE =TO_DATE(:SDATE,'YYYY-MM-DD') ");
            if (argLtdCode > 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                  ");
            }

            parameter.AppendSql("   AND DELDATE IS NULL                     ");

            parameter.Add("SDATE", argDate);

            if (argLtdCode > 0)
            {
                parameter.Add("LTDCODE", argLtdCode);
            }

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public HEA_JEPSU GetSumAmPmCount1(string argDate, long argLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(DECODE(AMPM2,'1',1,0)) AMCNT    ");
            parameter.AppendSql("     , SUM(DECODE(AMPM2,'1',0,1)) PMCNT    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU               ");
            parameter.AppendSql(" WHERE SDATE =TO_DATE(:SDATE,'YYYY-MM-DD') ");
            if (argLtdCode > 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                  ");
            }

            parameter.AppendSql("   AND DELDATE IS NULL                     ");

            parameter.Add("SDATE", argDate);

            if (argLtdCode > 0)
            {
                parameter.Add("LTDCODE", argLtdCode);
            }

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public string GetGbStsByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GbSTS                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public HEA_JEPSU Read_Jepsu(long PANO, string JEPDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE1      ");
            parameter.AppendSql("     , ROWID RID, Pano, SName, Age, Sex        ");
            parameter.AppendSql("     , LtdCode, GjJong, GbSTS                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE PANO = :PANO                            ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND SDATE < TO_DATE(:SDATE,'YYYY-MM-DD')    ");
            parameter.AppendSql(" ORDER BY SDATE DESC                           ");

            parameter.Add("PANO", PANO);
            parameter.Add("SDATE", JEPDATE);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public HEA_JEPSU Read_Jepsu2(long WRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE1      ");
            parameter.AppendSql("     , ROWID RID, Pano, SName, Age, Sex        ");
            parameter.AppendSql("     , LtdCode, GjJong, GbSTS, PTNO            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", WRTNO);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public HEA_JEPSU Read_Jepsu3(string argPtno, string argSdate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, WRTNO");
            parameter.AppendSql("     , LtdCode, GjJong, GbSTS, GWRTNO,SANGDAM  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                            ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE,'YYYY-MM-DD')    ");
            parameter.AppendSql(" ORDER BY SDATE DESC                           ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("SDATE", argSdate);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }


        public int Update_Hea_Jepsu_GbSts(string GBSTS, string ACTMEMO, long WRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU SET   ");
            parameter.AppendSql("       GbSTS   = :GBSTS            ");
            if (ACTMEMO != "")
            {
                parameter.AppendSql("     , ACTMEMO = :ACTMEMO      ");
            }
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO            ");

            parameter.Add("GBSTS", GBSTS, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (ACTMEMO != "")
            {
                parameter.Add("ACTMEMO", ACTMEMO);
            }
            parameter.Add("WRTNO", WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public HEA_JEPSU Read_Jepsu_Wait_List(long WRTNO, string SDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE, 'yyyy-MM-dd')   ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND GBSTS NOT IN ('0','D')                  ");

            parameter.Add("WRTNO", WRTNO);
            parameter.Add("SDATE", SDATE);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public HEA_JEPSU GetItembyWrtNoPaNoPtNo(long fnWRTNO, string strSDate, string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME, Age, LtdCode, SEX,PTNO, GBDUST   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            if (strGubun == "1")
            {
                parameter.AppendSql(" WHERE PANO = :WRTNO                       ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql(" WHERE PTNO = :WRTNO                       ");
            }
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE,'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND GBSTS <> 'D'                            ");


            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("SDATE", strSDate);

            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public int UpdateGbstsCdateGbexamByWrtno(string strGbsts, string strGbexam, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql("   SET GBSTS  = :GBSTS         ");
            parameter.AppendSql("     , GBEXAM  = :GBEXAM       ");
            parameter.AppendSql("     , CDATE  = ''             ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO         ");

            parameter.Add("GBSTS", strGbsts);
            parameter.Add("GBEXAM", strGbexam);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }


        public int HisInsert(HEA_JEPSU item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO HEA_JEPSU_HIS ( WRTNO,JepDATE,SDATE,PANO,SNAME,SEX,AGE,GJJONG,LTDCODE                         ");
            parameter.AppendSql(" ,Remark,GamCode,SABUN,MAILCODE,PTNO,BURATE,GbSts,HalinYN,SEXAMS,CARDSEQNO,GbnBmd,GbEndo,GbDust            ");
            parameter.AppendSql(" ,ResultSend,JOBSABUN,EntTime,EndoGbn,ExamRemark,Sunap,GuideTel,GamAmt)                                    ");                                                                                                
            parameter.AppendSql(" SELECT WRTNO, JepDATE, SDATE, PANO, SNAME, SEX, AGE, GJJONG, LTDCODE                                      ");
            parameter.AppendSql(" ,'예약일자변경', GamCode,SABUN,MAILCODE,PTNO,BURATE,GbSts,HalinYN,SEXAMS,CARDSEQNO,GbnBmd,GbEndo,GbDust    ");
            parameter.AppendSql(" ,ResultSend,:JOBSABUN,TRUNC(SYSDATE),EndoGbn,ExamRemark,Sunap,GuideTel,GamAmt                                     ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HEA_JEPSU                                                                                ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                                      ");

            #region Query 변수대입
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("JOBSABUN", item.JOBSABUN);

            #endregion

            return ExecuteNonQuery(parameter);
        }
    }
}
