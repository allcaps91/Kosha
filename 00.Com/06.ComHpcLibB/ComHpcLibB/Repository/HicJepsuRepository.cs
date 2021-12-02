namespace ComHpcLibB.Repository
{
    using ComBase;
    using ComBase.Mvc;
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using ComBase.Controls;

    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuRepository :BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuRepository()
        {
        }

        public HIC_JEPSU Read_Jepsu(long PANO, string JEPDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') SDATE1    ");
            parameter.AppendSql("     , ROWID RID, WRTNO                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                   ");
            parameter.AppendSql(" WHERE PANO = :PANO                            ");
            parameter.AppendSql("   AND JEPDATE < TO_DATE(:JEPDATE,'YYYY-MM-DD')");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND GJJONG NOT IN('31','35')                ");
            parameter.AppendSql("   AND JONGGUMYN = '1'                         ");

            parameter.Add("PANO", PANO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", JEPDATE);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public long Read_Jepsu_Wrtno2(long nPano, string strJepDate, string strGjYear, string strChasu, string strCode2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU                                            ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                                ");
            if (strChasu == "2" || strChasu == "4" || strChasu == "6")
            {
                parameter.AppendSql("   AND JepDate > TO_DATE(:JEPDATE, 'YYYY-MM-DD')                               ");
            }
            else
            {
                parameter.AppendSql("   AND JepDate < TO_DATE(:JEPDATE, 'YYYY-MM-DD')                               ");
            }
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                            ");
            parameter.AppendSql("   AND GjChasu = :GJCHASU                                                          ");
            parameter.AppendSql("   AND ( DelDate IS NULL OR DelDate ='' )                                          ");
            parameter.AppendSql("   AND GjJong IN (SELECT CODE FROM KOSMOS_PMPA.HIC_EXJONG WHERE Code2 IN (:CODE2))");
            parameter.AppendSql(" ORDER BY WRTNO                                                                    ");

            parameter.Add("PANO", nPano);
            parameter.Add("JEPDATE", strJepDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GJCHASU", strChasu, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE2", strCode2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<long>(parameter);
        }

        public List<HIC_JEPSU> GetJepsuGunsu(string dtpFDate, string dtpTDate, string cboJONG)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,PANO,WRTNO,SNAME,GBINWON GJJONG , ");

            parameter.AppendSql("          DECODE(SUBSTR(GBAM,1,1),'1',1,0)     CAN1, ");
            parameter.AppendSql("          DECODE(SUBSTR(GBAM,3,1),'1',1,0)     CAN2, ");
            parameter.AppendSql("          DECODE(SUBSTR(GBAM,5,1),'1',1,0)     CAN3, ");
            parameter.AppendSql("          DECODE(SUBSTR(GBAM,7,1),'1',1,0)     CAN4, ");
            parameter.AppendSql("          DECODE(SUBSTR(GBAM,9,1),'1',1,0)     CAN5, ");
            parameter.AppendSql("          DECODE(SUBSTR(GBAM,11,1),'1',1,0)    CAN6 ");

            parameter.AppendSql("   FROM  KOSMOS_PMPA.HIC_JEPSU");

            parameter.AppendSql("   WHERE JEPDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("          AND JEPDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("          AND DELDATE IS NULL   AND GBSTS NOT IN ( '0','D')  ");

            if (cboJONG != "00")
            {
                parameter.AppendSql("          AND GJJONG = :CBOJONG  ");
            }

            parameter.AppendSql("   ORDER BY JepDate,WRTNO  ");
            
            parameter.Add("TDATE", dtpTDate);
            parameter.Add("FDATE", dtpFDate);
            
            if(cboJONG != "00"){
                parameter.Add("CBOJONG", cboJONG);
            }
            
            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int GetTongAm(string dtpFDate, string dtpTDate, string cboJONG)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" CREATE OR REPLACE VIEW VIEW_HIC_TONGAM_NEW ( ");
            parameter.AppendSql("       JEPDATE, PANO, WRTNO, SNAME, GBINWON, GJJONG, CNT1, CNT2, CNT3, CNT4, CNT5, CNT6 ) ");
            parameter.AppendSql(" AS ");
            parameter.AppendSql(" SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') AS JEPDATE, PANO, WRTNO, SNAME, GBINWON, GJJONG  ");
            parameter.AppendSql("        , DECODE(SUBSTR(GBAM, 1,  1), '1', 1, 0)  AS CNT1 ");
            parameter.AppendSql("        , DECODE(SUBSTR(GBAM, 3,  1), '1', 1, 0)  AS CNT2 ");
            parameter.AppendSql("        , DECODE(SUBSTR(GBAM, 5,  1), '1', 1, 0)  AS CNT3 ");
            parameter.AppendSql("        , DECODE(SUBSTR(GBAM, 7,  1), '1', 1, 0)  AS CNT4 ");
            parameter.AppendSql("        , DECODE(SUBSTR(GBAM, 9,  1), '1', 1, 0)  AS CNT5 ");
            parameter.AppendSql("        , DECODE(SUBSTR(GBAM, 11, 1), '1', 1, 0)  AS CNT6 ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_JEPSU ");
            parameter.AppendSql("  WHERE JEPDATE >= TO_DATE('" + dtpFDate + "', 'YYYY-MM-DD')  ");
            parameter.AppendSql("    AND JEPDATE <= TO_DATE('" + dtpTDate + "', 'YYYY-MM-DD')  ");
            parameter.AppendSql("    AND DELDATE IS NULL  ");
            parameter.AppendSql("    AND GBSTS NOT IN ('0', 'D')  ");

            if (cboJONG != "00") { parameter.AppendSql("   AND GJJONG = '" + cboJONG + "'  "); }

            parameter.AppendSql("  ORDER BY JEPDATE, WRTNO  ");

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetTongAm_View(string dtpFDate, string dtpTDate, string cboJONG)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT SUBSTR(JEPDATE,1,7) JEPDATE, SUM(CNT1) CNT1,SUM(CNT2) CNT2,SUM(CNT3) CNT3,SUM(CNT4) CNT4,SUM(CNT5) CNT5,SUM(CNT6) CNT6 ");
            parameter.AppendSql("   FROM  KOSMOS_PMPA.VIEW_HIC_TONGAM_NEW");
            parameter.AppendSql("   GROUP BY SUBSTR(JEPDATE,1,7)  ");

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public void UpdateSangdamDrnobyWrtno(string strDate, long nSabun, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET ");
            if (strDate.IsNullOrEmpty())
            {
                parameter.AppendSql("       SANGDAMDATE = ''      ");
                parameter.AppendSql("     , SANGDAMDRNO = 0       ");
            }
            else
            {
                parameter.AppendSql("       SANGDAMDATE = TO_DATE(:SANGDAMDATE, 'YYYY-MM-DD')   ");
                parameter.AppendSql("     , SANGDAMDRNO = :SANGDAMDRNO                          ");
            }

            parameter.AppendSql("  WHERE WRTNO    = :WRTNO                  ");

            if (!strDate.IsNullOrEmpty())
            {
                parameter.Add("SANGDAMDATE", strDate);
                parameter.Add("SANGDAMDRNO", nSabun);
            }

            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public void UpdateWebPrtSendbyWrtno(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET   ");
            parameter.AppendSql("       WEBPRINTSEND = ''           ");
            parameter.AppendSql("     , TONGBODATE = ''             ");
            parameter.AppendSql("     , PRTSABUN = ''               ");
            parameter.AppendSql("  WHERE WRTNO    = :WRTNO          ");

            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetExRemarkByPanoJepDate(long nPANO, string strDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXAMREMARK                                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                           ");
            parameter.AppendSql(" WHERE JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                       ");
            parameter.AppendSql("   AND DelDate IS NULL                                                 ");
            parameter.AppendSql("   AND PANO = :PANO                                                    ");

            parameter.Add("JEPDATE", strDate);
            parameter.Add("PANO", nPANO);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int UpdateHemsMirSayubyWrtNo(string strSayu, string strHemSNo, long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET               ");
            parameter.AppendSql("       HEMSMIRSAYU = :HEMSMIRSAYU              ");
            parameter.AppendSql("     , HEMSNO      = :HEMSNO                   "); 
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO                    ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("HEMSMIRSAYU", strSayu);
            parameter.Add("HEMSNO", strHemSNo);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDateGjYearGjJong(string strFrDate, string strToDate, string strYear, string strLtdName, string strKiho, string strChung, string strchk1, string strchk2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Wrtno, Sname,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate, Kiho, GjJong, Age ,LtdCode      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                               ");
            parameter.AppendSql(" WHERE JepDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                            ");
            parameter.AppendSql("   AND JepDate <= TO_DATE(:TODATE,'YYYY-MM-DD')                                            ");
            parameter.AppendSql("   AND DelDate IS NULL                                                                     "); //삭제는 제외
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                                    ");
            parameter.AppendSql("   AND GjJong IN ('12','17')                                                               ");
            if (strChung == "1")
            {
                parameter.AppendSql("   AND (MirNo1 IS NULL OR MirNo1 = 0)                                                  ");
            }
            else if (strChung == "0")
            {
                parameter.AppendSql("   AND MirNo1 > 0                                                                      "); //청구만
            }
            parameter.AppendSql("   AND LtdCode  IN ( SELECT Code FROM KOSMOS_PMPA.HIC_LTD WHERE NAME = :NAME)              ");
            if (!strKiho.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND KIHO = :KIHO                                                                    ");
            }
            if (strchk1 == "Y")
            {
                parameter.AppendSql("   AND GjChasu = '1'                                                                   ");
            }
            if (strchk2 == "Y")
            {
                parameter.AppendSql("   AND GjChasu = '2'                                                                   ");
            }
            

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("NAME", strLtdName);
            if (!strKiho.IsNullOrEmpty())
            {
                parameter.Add("KIHO", strKiho, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int UpdateHemsNobyWrtNo(long wRTNO, long nMirNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU                   ");
            parameter.AppendSql("   SET HEMSNO = :HEMSNO                        ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", wRTNO);
            parameter.Add("HEMSNO", nMirNo);

            return ExecuteNonQuery(parameter);
        }

        public HIC_JEPSU GetItemByJepDateWrtno(long nWRTNO, string strDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PANO, WRTNO, SNAME, AGE, SEX, LTDCODE, TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE, GJYEAR, PTNO  ");
            parameter.AppendSql("     , TO_CHAR(IPSADATE,'yyyy-mm-dd') IPSADATE, GJJONG, UCODES, SEXAMS, ERFLAG, WAITREMARK         ");
            parameter.AppendSql("     , SABUN, BUSENAME, GBSUCHEP, JIKJONG, GJCHASU, GJBANGI, EXAMREMARK                            ");
            parameter.AppendSql("     , JONGGUMYN, GBHEAENDO, IEMUNNO, GBN, CLASS, GBCHK3                                           ");
            parameter.AppendSql("     , TEL, BUSEIPSA, SANGDAMDRNO                                                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                              ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                                   ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                                             ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("JEPDATE", strDate);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetGjJongCntbyMirNo(string strJong, long nMirNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GjJong, COUNT(GjJong) Cnt                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE DELDATE IS NULL                             ");
            if (strJong == "1")
            {
                parameter.AppendSql("   AND MIRNO1 = :MIRNO                         ");
            }
            else if (strJong == "3")
            {
                parameter.AppendSql("   AND MIRNO2 = :MIRNO                         ");
            }
            else if (strJong == "4")
            {
                parameter.AppendSql("   AND MIRNO3 = :MIRNO                         ");
            }
            else if (strJong == "E")
            {
                parameter.AppendSql("   AND MIRNO4 = :MIRNO                         ");
            }
            parameter.AppendSql(" GROUP BY GjJong                                   ");

            parameter.Add("MIRNO", nMirNo);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyWrtNo(string strFrDate, string strToDate, string strSname, long nWRTNO, string strOut, string strAmPm, string strNoTrans, string strJob)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE, WRTNO, SNAME, GJJONG, GBCHUL ");
            parameter.AppendSql("     , IEMUNNO, SEXAMS, UCODES, PTNO, JONGGUMYN, PANO                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                               ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                           ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                           ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                     ");
            if (!strSname.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SNAME = :SNAME                                                  ");
            }
            if (nWRTNO != 0)
            {
                parameter.AppendSql("   AND WRTNO = :WRTNO                                                  ");
            }
            if (strOut == "0")
            {
                parameter.AppendSql("   AND GBCHUL = 'N'                                                    ");
            }
            else if (strOut == "1")
            {
                parameter.AppendSql("   AND GBCHUL = 'Y'                                                    ");
            }
            if (strAmPm == "0")
            {
                parameter.AppendSql("   AND TO_CHAR(ENTTIME,'HH24:MI') <= '12:30'                           ");
            }
            else if (strAmPm == "1")
            {
                parameter.AppendSql("   AND TO_CHAR(ENTTIME,'HH24:MI') >= '12:31'                           ");
            }
            if (strNoTrans == "1")
            {
                parameter.AppendSql("   AND GJJONG NOT IN ('55','62')                                       "); //운전면허,혈액종검
            }
            else if (strNoTrans == "2")
            {
                parameter.AppendSql("   AND GJJONG IN ('11','31')                                           "); //순수일반 + 암검진
                parameter.AppendSql("   AND UCODES IS NULL                                                  "); 
            }
            if (strJob == "2")
            {
                parameter.AppendSql("   AND WRTNO NOT IN (SELECT WRTNO                                      ");
                parameter.AppendSql("                       FROM KOSMOS_PMPA.HIC_CHARTTRANS                 ");
                parameter.AppendSql("                      WHERE TRDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')   ");
                parameter.AppendSql("                        AND TRDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')   ");
                parameter.AppendSql("                        AND ENTTIME IS NOT NULL)                       ");
            }
            parameter.AppendSql(" ORDER BY JEPDATE, SNAME                                                   ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strSname.IsNullOrEmpty())
            {
                parameter.Add("SNAME", strSname);
            }
            if (nWRTNO > 0)
            {
                parameter.Add("WRTNO", nWRTNO);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int GongDanChungGu(long nMisuNo, List<string> wrtno, string fstrJong)
        {
            MParameter parameter = CreateParameter();

            if(fstrJong != "6")
            {
                parameter.AppendSql("UPDATE             KOSMOS_PMPA.HIC_JEPSU SET                   ");
                parameter.AppendSql("                   MISUNO2=:MISUNO                             ");
                parameter.AppendSql("WHERE              WRTNO IN (:WRTNO)                           ");
                parameter.AppendSql("   AND             (MISUNO2 IS NULL OR MISUNO2 = 0)            ");
            }
            else
            {
                parameter.AppendSql("UPDATE             KOSMOS_PMPA.HIC_JEPSU SET                   ");
                parameter.AppendSql("                   MISUNO3=:MISUNO                             ");
                parameter.AppendSql("WHERE              WRTNO IN (:WRTNO)                           ");
                parameter.AppendSql("   AND             (MISUNO3 IS NULL OR MISUNO3 = 0)            ");
            }

            if (fstrJong != "6")
            {
                parameter.Add("MISUNO", nMisuNo);
                parameter.AddInStatement("WRTNO", wrtno);
            }
            else
            {
                parameter.Add("MISUNO", nMisuNo);
                parameter.AddInStatement("WRTNO", wrtno);
            }
            


            return ExecuteNonQuery(parameter);
        }

        public int CompanyChunguUpdate(long nMisuNo, string strFDate, string strTDate, string strltdcode, string fstrJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE             KOSMOS_PMPA.HIC_JEPSU SET                                   ");
            parameter.AppendSql("                   MISUNO2=:MISUNO                                             ");
            parameter.AppendSql("WHERE              JEPDATE>=TO_DATE(:FDATE,'YYYY-MM-DD')                       ");
            parameter.AppendSql("   AND             JEPDATE<=TO_DATE(:TDATE,'YYYY-MM-DD')                       ");
            parameter.AppendSql("   AND             LTDCODE = :LTDCODE                                          ");
            parameter.AppendSql("   AND             DELDATE IS NULL                                             ");
            parameter.AppendSql("   AND             (MISUNO2 IS NULL OR MISUNO2 = 0)                            ");

            switch (fstrJong)
            {
                case "1":
                    parameter.AppendSql("   AND     GJJONG IN ('13','18')                                       ");
                    break;
                case "2":
                    parameter.AppendSql("   AND     GJJONG IN ('12','17')                                       ");
                    break;
                case "3":
                    parameter.AppendSql("   AND     (GJJONG <= '30' AND GJJONG NOT IN ('12','13','17','18'))    ");
                    break;
                case "4":
                    parameter.AppendSql("   AND     GJJONG IN ('31')                                            ");
                    break;
                case "5":
                    parameter.AppendSql("   AND     (GJJONG >= '31' AND GJJONG <= '80')                         ");
                    break;
            }
            
            parameter.Add("MISUNO", nMisuNo);
            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);
            parameter.Add("LTDCODE", strltdcode);
            
            return ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetCountGongDan(string strMinDate, string strMaxDate, string strOldData)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT         COUNT(*) CNT                                      ");
            parameter.AppendSql("FROM           KOSMOS_PMPA.HIC_JEPSU                             ");
            parameter.AppendSql("WHERE          JEPDATE>=TO_DATE(:MINDATE, 'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND         JEPDATE<=TO_DATE(:MAXDATE, 'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND         (LTDCODE = :OLDDATA OR KIHO = :OLDDATA )          ");
            parameter.AppendSql("   AND         DELDATE IS NULL                                   ");
            parameter.AppendSql("   AND         PANO <> 999                                       ");
            parameter.AppendSql("   AND         SECOND_FLAG='Y' AND SECOND_DATE IS NULL           ");
            parameter.AppendSql("   AND         (MISUNO2 IS NULL OR MISUNO2 = 0)                  ");

            
            parameter.Add("MINDATE", strMinDate, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("MAXDATE", strMaxDate, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("OLDDATA", strOldData, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int UpdateBogunMisu(long nMisuNo, List<string> WRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE             KOSMOS_PMPA.HIC_JEPSU SET                   ");
            parameter.AppendSql("                   MISUNO4=:MISUNO                             ");
            parameter.AppendSql("WHERE              WRTNO IN (:WRTNO)                            ");
            parameter.AppendSql("   AND             (MISUNO4 IS NULL OR MISUNO4 = 0)            ");

            parameter.Add("MISUNO", nMisuNo);
            parameter.AddInStatement("WRTNO", WRTNO);

            return ExecuteNonQuery (parameter);
        }

        internal List<HIC_JEPSU> GetWRTNO(long nWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT         *                       ");
            parameter.AppendSql("FROM           KOSMOS_PMPA.HIC_JEPSU   ");
            parameter.AppendSql("WHERE          WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", nWrtno);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyWrtNoList(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Wrtno ,Sname, GjJong , Mirno1, Mirno2, Mirno3, Mirno4,Mirno5    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                           ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                  ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int UpdatebyMirSayuMirNobyWrtNo(string strSayu, long nWRTNO, string strJong, string strBogenso)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET               ");
            if (strSayu.IsNullOrEmpty())
            {
                parameter.AppendSql("       MIRSAYU = ''                        ");
                //사업장일 경우 접수테이블에 청구번호 1, 2,3,5 업데이트
                if (strJong == "1")
                {
                    parameter.AppendSql("     , MIRNO1 = '0'                    ");
                }
                else if (strJong == "3")
                {
                    parameter.AppendSql("     , MIRNO2 = '0'                    ");
                }
                else if (strJong == "4")
                {   
                    parameter.AppendSql("     , MIRNO3 = '0'                    ");
                    parameter.AppendSql("     , MIRNO4 = '0'                    ");
                }
                else if (strJong == "E")
                {
                    parameter.AppendSql("     , MIRNO5 = '0'                    ");
                }
            }
            else
            {
                parameter.AppendSql("       MIRSAYU = :MIRSAYU                  ");
                //사업장일 경우 접수테이블에 청구번호 1, 2,3,4 업데이트
                if (strJong == "1")
                {
                    parameter.AppendSql("     , MIRNO1 = '1'                    ");
                }
                else if (strJong == "3")
                {
                    parameter.AppendSql("     , MIRNO2 = '2'                    ");
                }
                else if (strJong == "4")
                {
                    if (strBogenso.IsNullOrEmpty())
                    {
                        parameter.AppendSql("     , MIRNO3 = '3'                ");
                    }
                    else
                    {
                        parameter.AppendSql("     , MIRNO3 = '3'                ");
                        parameter.AppendSql("     , MIRNO4 = '4'                ");
                    }
                }
                else if (strJong == "E")
                {
                    parameter.AppendSql("     , MIRNO5 = '5'                    ");
                }
            }
            
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", nWRTNO);
            if (!strSayu.IsNullOrEmpty())
            {
                parameter.Add("MIRSAYU", strSayu);
            }

            return ExecuteNonQuery(parameter);
        }

        public HIC_JEPSU GetUCodesbyPaNoGjYear(long nPano, string strGjYear)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT UCODES                                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                           ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                    ");
            parameter.AppendSql("   AND (GJCHASU ='1' OR GJJONG ='14')                                  ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                ");
            parameter.AppendSql("   AND UCODES IS NOT NULL                                              ");

            parameter.Add("PANO", nPano);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDateMirNoCjChasu(string argFDate, string argToDate, long argMirno, string strChasu)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,Pano,Kiho,GjJong,JikGbn,Sname,Sex,Age,GbChul              ");
            parameter.AppendSql("     , TO_CHAR(JepDate,'YYYY-MM-DD') JepDate                           ");
            parameter.AppendSql("     , TO_CHAR(DelDate,'YYYY-MM-DD') DelDate                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                           ");
            parameter.AppendSql(" WHERE JepDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')                        ");
            parameter.AppendSql("   AND JepDate <= TO_DATE(:TODATE,'YYYY-MM-DD')                        ");
            parameter.AppendSql("   AND MIRNO1 = :MIRNO1                                                ");
            parameter.AppendSql("   AND GJCHASU = :GJCHASU                                              ");

            parameter.Add("FRDATE", argFDate);
            parameter.Add("TODATE", argToDate);
            parameter.Add("MIRNO1", argMirno);
            parameter.Add("GJCHASU", strChasu, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItemMirNobyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO ,SNAME, GJJONG , MIRNO1, MIRNO2, MIRNO3, MIRNO4,MIRNO5    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                           ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                  ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyAll(string strFDate, string strTDate, string strJong, string strJohap2, List<string> str검진종류세팅, string strLtdCode, string strLife2, string strGubun1, string strGubun2, string strW_Am, HIC_JEPSU item)
        {
            MParameter parameter = CreateParameter();

            //건강검진
            if (strJong == "1")
            {
                parameter.AppendSql("SELECT TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,WRTNO,SName,Sex,GbSts                                 ");
                parameter.AppendSql("     , SECOND_Flag,TO_CHAR(SECOND_Date,'YYYY-MM-DD') Second_Date,MURYOAM                           ");
                parameter.AppendSql("     , Age,GjJong,GjChasu,SExams,UCodes,Kiho ,Gkiho,BoGunSo                                        ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                                       ");
                parameter.AppendSql(" WHERE JEPDATE > =TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                   ");
                parameter.AppendSql("   AND JEPDATE < =TO_DATE(:TODATE, 'YYYY-MM-DD')                                                   ");
                parameter.AppendSql("   AND DELDATE IS NULL                                                                             "); //삭제는 제외
                parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                                            ");
                parameter.AppendSql("   AND (MirNo1 IS NULL OR MirNo1 = 0)                                                              "); //미청구만
                parameter.AppendSql("   AND PanjengDate IS NOT NULL                                                                     ");
                parameter.AppendSql("   AND TongboDate IS NOT NULL                                                                      "); //결과지통보
                parameter.AppendSql("   AND GJJONG IN (:GJJONG)                                                                         ");
                parameter.AppendSql("   AND BURATE NOT IN ('2','02')                                                                    ");
                if (!item.WRTNO.IsNullOrEmpty() && item.WRTNO != 0)
                {
                    parameter.AppendSql("   AND WRTNO = :WRTNO                                                                          ");
                }
                if (!item.BOGUNSO.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND BOGUNSO = :BOGUNSO                                                                      ");
                }
                parameter.AppendSql("   AND WRTNO NOT IN (SELECT WRTNO FROM HIC_MIR_ERROR_TONGBO WHERE OKDate IS NULL AND Gubun='1')    ");
                switch (strJohap2)
                {
                    case "사업장":
                        if (string.Compare(strLtdCode, "0000") > 0)
                        {
                            parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                              ");
                        }
                        parameter.AppendSql(" ORDER BY LtdCode,SName,GjChasu                                                            ");
                        break;
                    case "공무원":
                        if (string.Compare(item.LTDCODE.To<string>(), "0000") > 0)
                        {
                            parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                              ");
                        }
                        if (!item.KIHO.IsNullOrEmpty())
                        {
                            parameter.AppendSql("   ND KIHO = :KIHO                                                                     ");
                        }
                        parameter.AppendSql(" ORDER BY LtdCode,SName,GjChasu                                                            ");
                        break;
                    case "성인병":
                        if (strLtdCode == "급여")
                        {
                            parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('9')                                                      ");
                        }
                        else if (strLtdCode == "직장")
                        {
                            parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('7','8')                                                  ");
                        }
                        else if (strLtdCode == "공교")
                        {
                            parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('5','6')                                                  ");
                        }
                        else if (strLtdCode == "지역")
                        {
                            parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('1','2','3')                                              ");
                        }
                        parameter.AppendSql(" ORDER BY SName,GjChasu                                                                    ");
                        break;
                    default:
                        break;
                }
            }
            else if (strJong == "3")
            {
                parameter.AppendSql("SELECT TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,WRTNO,SName,Sex,GbSts                                 ");
                parameter.AppendSql("     , '' SECOND_Flag,'' Second_Date,MURYOAM                                                       ");
                parameter.AppendSql("     , Age,GjJong,GjChasu,SExams,UCodes,Kiho,BoGunSo,Gkiho                                         ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                                       ");
                parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                                    ");
                parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')                                                    ");
                parameter.AppendSql("   AND DELDATE IS NULL                                                                             "); //삭제는 제외
                parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                                            ");
                parameter.AppendSql("   AND GbDental = 'Y'                                                                              "); // 구강검진자
                parameter.AppendSql("   AND TongboDate IS NOT NULL                                                                      "); //결과지통보
                parameter.AppendSql("   AND (MirNo2 IS NULL OR MirNo2 = 0)                                                              "); //구강검사 미청구
                parameter.AppendSql("   AND GJJONG IN (:GJJONG)                                                                         ");
                if (!item.WRTNO.IsNullOrEmpty() && item.WRTNO != 0)
                {
                    parameter.AppendSql("   AND WRTNO = :WRTNO                                                                          ");
                }
                if (!item.BOGUNSO.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND BOGUNSO = :BOGUNSO                                                                      ");
                }
                parameter.AppendSql("   AND WRTNO NOT IN (SELECT WRTNO FROM HIC_MIR_ERROR_TONGBO WHERE OKDate IS NULL AND Gubun='3')    ");
                switch (strJohap2)
                {
                    case "사업장":
                        if (string.Compare(item.LTDCODE.To<string>(), "0000") > 0)
                        {
                            parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                              ");
                        }
                        parameter.AppendSql(" ORDER BY LtdCode,SName,GjChasu                                                            ");
                        break;
                    case "공무원":
                        if (string.Compare(strLtdCode, "0000") > 0)
                        {
                            parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                              ");
                        }
                        if (!item.KIHO.IsNullOrEmpty())
                        {
                            parameter.AppendSql("   ND KIHO = :KIHO                                                                     ");
                        }
                        parameter.AppendSql(" ORDER BY LtdCode,SName,GjChasu                                                            ");
                        break;
                    case "성인병":
                        if (strLtdCode == "급여")
                        {
                            parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('9')                                                      ");
                        }
                        else if (strLtdCode == "직장")
                        {
                            parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('7','8')                                                  ");
                        }
                        else if (strLtdCode == "공교")
                        {
                            parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('5','6')                                                  ");
                        }
                        else if (strLtdCode == "지역")
                        {
                            parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('1','2','3')                                              ");
                        }
                        parameter.AppendSql(" ORDER BY SName,GjChasu                                                                    ");
                        break;
                    default:
                        break;
                }
            }
            //암검진
            else
            {
                parameter.AppendSql("SELECT WRTNO,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,GjJong,GjChasu,GbSts                            ");
                parameter.AppendSql("     , SName,Age,Sex,SExams,UCodes, LtdCode, BoGunSo ,MURYOAM                                      ");
                parameter.AppendSql("     , SECOND_Flag,TO_CHAR(SECOND_Date,'YYYY-MM-DD') Second_Date,Kiho                              ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                                       ");
                parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                   ");
                parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                   ");
                if (strLife2.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND GjJong IN ('31')                                                                        ");     //암검진
                }
                else
                {
                    parameter.AppendSql("   AND GjJong IN ('35')                                                                        ");     //암검진 생애
                }
                if (strGubun1 == "1")
                {
                    parameter.AppendSql("   AND SExams LIKE '%3116%'                                                                    ");
                }
                if (strGubun2 == "1")
                {
                    parameter.AppendSql("   AND SExams NOT LIKE '%3170%'                                                                ");
                }
                parameter.AppendSql("   AND DelDate IS NULL                                                                             "); //삭제는 제외
                parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                                            ");
                parameter.AppendSql("   AND TONGBODATE IS NOT NULL                                                                      "); //결과지통보
                if (strJohap2 == "사업장")
                {
                    parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('7','8')                                                          ");
                }
                else if (strJohap2 == "공무원")
                {
                    parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('5','6')                                                          ");
                }
                else if (strJohap2 == "성인병")
                {
                    parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('1','2','3')                                                      ");
                }
                if (!strLtdCode.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                                      ");
                }
                if (strJong == "4")
                {
                    parameter.AppendSql("   AND (MirNo3 IS NULL OR MirNo3 = 0)                                                          "); //미청구만
                    parameter.AppendSql("   AND (MileageAm<>'Y' OR MileageAm IS NULL)                                                   "); //공단암
                    parameter.AppendSql("   AND (GubDaeSang IS NULL OR GubDaeSang <> 'Y')                                               "); //의료급여암
                    if (!strLtdCode.IsNullOrEmpty())
                    {
                        parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                                  ");
                    }
                    if (!item.KIHO.IsNullOrEmpty())
                    {
                        parameter.AppendSql("   AND KIHO = :KIHO                                                                        ");
                    }
                }
                else if (strJong == "E")
                {
                    parameter.AppendSql("   AND (MirNo5 IS NULL OR MirNo5 = 0)                                                          "); //미청구만
                    parameter.AppendSql("   AND GubDaeSang = 'Y'                                                                        "); //의료급여암
                    if (!strLtdCode.IsNullOrEmpty())
                    {
                        parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                                  ");
                    }
                    if (!item.KIHO.IsNullOrEmpty())
                    {
                        parameter.AppendSql("   AND KIHO = :KIHO                                                                        ");
                    }
                }
                else
                {
                    parameter.AppendSql("   AND (MirNo4 IS NULL OR MirNo4 = 0)                                                          "); //미청구만
                    parameter.AppendSql("   AND MURYOAM = 'Y'                                                                           "); //보건소암
                    parameter.AppendSql("   AND MURYOGBN > '0'                                                                          ");
                    if (!item.KIHO.IsNullOrEmpty())
                    {
                        parameter.AppendSql("   AND KIHO = :KIHO                                                                        ");
                    }
                    if (!item.WRTNO.IsNullOrEmpty() && item.WRTNO != 0)
                    {
                        parameter.AppendSql("   AND WRTNO = :WRTNO                                                                      ");
                    }
                    //자궁경부암만
                    if (strW_Am == "1")
                    {
                        parameter.AppendSql("   AND ( SUBSTR(GbAm,1,1) + SUBSTR(GbAm,3,1) + SUBSTR(GbAm,5,1) + SUBSTR(GbAm,7,1) + ");
                    }
                    parameter.AppendSql("    SUBSTR(GbAm,9,1) + SUBSTR(GbAm,11,1) ) =1                                                  ");
                    parameter.AppendSql("   AND SUBSTR(GbAm,11,1) =1                                                                    ");
                }
                if (!item.BOGUNSO.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND BOGUNSO = :BOGUNSO                                                                      ");
                }
                parameter.AppendSql("   AND WRTNO NOT IN (SELECT WRTNO FROM HIC_MIR_ERROR_TONGBO WHERE OKDate IS NULL AND Gubun IN ('4','E'))   ");
                parameter.AppendSql("   AND Burate NOT IN ('2','02','3','03')                                                           ");
                parameter.AppendSql(" ORDER BY WRTNO                                                                                    ");
            }

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            if (strJong == "1" || strJong == "3")
            {
                if (!item.WRTNO.IsNullOrEmpty() && item.WRTNO != 0)
                {
                    parameter.Add("WRTNO", item.WRTNO);
                }
                if (!item.BOGUNSO.IsNullOrEmpty())
                {
                    parameter.Add("BOGUNSO", item.BOGUNSO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                }
                switch (strJohap2)
                {
                    case "사업장":
                        if (string.Compare(strLtdCode, "0000") > 0)
                        {
                            parameter.Add("LTDCODE", strLtdCode);
                        }
                        break;
                    case "공무원":
                        if (string.Compare(strLtdCode, "0000") > 0)
                        {
                            parameter.Add("LTDCODE", strLtdCode);
                        }
                        if (!item.KIHO.IsNullOrEmpty())
                        {
                            parameter.Add("KIHO", item.KIHO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (!item.GJYEAR.IsNullOrEmpty())
                {
                    parameter.Add("GJYEAR", item.GJYEAR);
                }
                if (item.LTDCODE.IsNullOrEmpty() && item.LTDCODE != 0)
                {
                    parameter.Add("LTDCODE", item.LTDCODE);
                }
                if (strJong == "4" || strJong == "E")
                {
                    if (string.Compare(strLtdCode, "0000") > 0)
                    {
                        parameter.Add("LTDCODE", strLtdCode);
                    }
                    if (!item.KIHO.IsNullOrEmpty())
                    {
                        parameter.Add("KIHO", item.KIHO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                    }
                }
                else
                {
                    if (!item.KIHO.IsNullOrEmpty())
                    {
                        parameter.Add("KIHO", item.KIHO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                    }
                }
                if (!item.BOGUNSO.IsNullOrEmpty())
                {
                    parameter.Add("BOGUNSO", item.BOGUNSO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                }
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetListByPanoGjYearJepDateGjJonIN(long fnPano, string fstrJepDate, string strGjYear, string[] strGjJong, string FstrGjJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE, WRTNO, GJCHASU   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE PANO = :PANO                                            ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                        ");
            parameter.AppendSql("   AND GJJONG IN (:GJJONG)                                     ");
            if (string.Compare(FstrGjJong, "21") >= 0 && string.Compare(FstrGjJong, "26") <= 0)
            {
                parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE,'YYYY-MM-DD')                ");
            }
            else if (FstrGjJong == "69")
            {
                parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE,'YYYY-MM-DD')                ");
            }
            else
            {
                parameter.AppendSql("   AND JEPDATE < TO_DATE(:JEPDATE,'YYYY-MM-DD')                ");
            }

            parameter.AppendSql("   AND DELDATE IS NULL                                         ");
            parameter.AppendSql(" ORDER BY JEPDATE DESC, WRTNO                                  ");

            parameter.Add("PANO", fnPano);
            parameter.Add("GJYEAR", strGjYear);
            parameter.AddInStatement("GJJONG", strGjJong);
            parameter.Add("JEPDATE", fstrJepDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetCountChcasubyMirNo(long nMirno, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(*) CNT,SUM(DECODE(GjChasu,'1',1,0)) CNT1                  ");
            parameter.AppendSql("     , SUM(DECODE(GjChasu,'1',0,1)) CNT2                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                           ");
            if (strGubun == "1")
            {
                parameter.AppendSql(" WHERE MIRNO1 = :MIRNO                                             ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql(" WHERE MIRNO2 = :MIRNO                                             ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql(" WHERE MIRNO3 = :MIRNO                                             ");
            }
            else
            {
                parameter.AppendSql(" WHERE MIRNO5 = :MIRNO                                             ");
            }

            parameter.Add("MIRNO1", nMirno);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public int UpdateMirNo0byWrtNo(long nWRTNO, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET               ");
            if (strGubun == "1")
            {
                parameter.AppendSql("       MIRNO1 = 0                          ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql("       MIRNO2 = 0                          ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql("       MIRNO3 = 0                          ");
                parameter.AppendSql("     , MIRNO4 = 0                          ");
            }
            else if (strGubun == "E")
            {
                parameter.AppendSql("       MIRNO5 = 0                          ");
            }
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetWrtNobyJepDateGjYearMirno1(string fRDATE, string tODATE, string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                   ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')");
            parameter.AppendSql("   AND MIRNO1 = :MIRNO1                        ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                        ");
            parameter.AppendSql("   AND GbDental = 'Y'                          ");

            parameter.Add("FRDATE", fRDATE);
            parameter.Add("TODATE", tODATE);
            parameter.Add("GJYEAR", strGjYear);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public string GetBogunsobyJepDateMirNo(string frDate, string toDate, long argMirno, string sGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT BOGUNSO                                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                   ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')");
            if (sGubun == "1")
            {
                parameter.AppendSql("   AND MIRNO1 = :MIRNO                     ");
            }
            else if (sGubun == "2")
            {
                parameter.AppendSql("   AND MIRNO2 = :MIRNO                     ");
            }
            else if (sGubun == "3")
            {
                parameter.AppendSql("   AND MIRNO3 = :MIRNO                     ");
            }
            else if (sGubun == "4")
            {
                parameter.AppendSql("   AND MIRNO4 = :MIRNO                     ");
            }
            else if (sGubun == "5")
            {
                parameter.AppendSql("   AND MIRNO5 = :MIRNO                     ");
            }
            parameter.AppendSql("   AND ROWNUM = 1                              ");

            parameter.Add("FRDATE", frDate);
            parameter.Add("TODATE", toDate);
            parameter.Add("MIRNO", argMirno);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_JEPSU> GetExpenseItembyJepDateGjYear_Dental(string strFDate, string strTDate, string strYear, long nWrtNo, string strLtdCode, string strBogunso, List<string> strGjJong, string strJohap2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT LtdCode,WRTNO,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,GjJong,GjChasu,Sname, Age, Sex,GbSts,SExams     ");
            parameter.AppendSql("    , UCodes, SECOND_Flag,TO_DATE(SECOND_Date,'YYYY-MM-DD') Second_Date,Kiho,Gkiho                        ");
            parameter.AppendSql("     , DECODE(SUBSTR(Gkiho,1,1),'1', '지역' ,                                                              ");
            parameter.AppendSql("                                '2', '지역' ,                                                              ");
            parameter.AppendSql("                                '3', '지역' ,                                                              ");
            parameter.AppendSql("                                '5', '공교' ,                                                              ");
            parameter.AppendSql("                                '6', '공교' ,                                                              ");
            parameter.AppendSql("                                '7', '직장' ,                                                              ");
            parameter.AppendSql("                                '8', '직장',                                                               ");
            parameter.AppendSql("                                '9', '급여'                                                                ");
            parameter.AppendSql("                                      ) as JOHAP                                                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                                               ");
            parameter.AppendSql(" WHERE JepDate >= TO_DATE(:FDATE, 'YYYY-MM-DD')                                                            ");
            parameter.AppendSql("   AND JepDate <= TO_DATE(:TDATE, 'YYYY-MM-DD')                                                            ");
            parameter.AppendSql("   AND DelDate IS NULL                                                                                     "); //삭제는 제외
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                                                    ");
            parameter.AppendSql("   AND (MirNo1 IS NULL OR MirNo1 = 0)                                                                      "); //미청구만
            parameter.AppendSql("   AND GBDENTAL = 'Y'                                                                                      "); //구강검사가 있는것
            parameter.AppendSql("   AND WRTNO NOT IN (SELECT WRTNO FROM HIC_MIR_ERROR_TONGBO WHERE OKDate IS NULL AND Gubun='1')            ");
            parameter.AppendSql("   AND TongboDate IS NOT NULL                                                                              "); //결과지 통보
            parameter.AppendSql("   AND (MirNo2 IS NULL OR MirNo2 = 0)                                                                      "); //구강검사 미청구만
            parameter.AppendSql("   AND BURATE NOT IN ('2','02','3','03')                                                                   ");
            if (!nWrtNo.IsNullOrEmpty() && nWrtNo != 0)
            {
                parameter.AppendSql("   AND WRTNO = :WRTNO                                                                                  ");
            }
            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                                              ");
            }
            if (!strBogunso.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND BOGUNSO = :BOGUNSO                                                                              ");
            }
            parameter.AppendSql("   AND GJJONG IN (:GJJONG)                                                                                                             ");
            //=============================================================================================================================================================
            //READ_HIC_MIR_공단부담체크("1", strTempSQL)
            //=============================================================================================================================================================
            parameter.AppendSql("   AND WRTNO NOT IN(SELECT WRTNO FROM KOSMOS_PMPA.HIC_SUNAPDTL                                                                         ");
            parameter.AppendSql("                     WHERE WRTNO IN (SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU                                                           ");
            parameter.AppendSql("                                      WHERE JepDate >= TO_DATE(:FDATE, 'YYYY-MM-DD')                                                   ");
            parameter.AppendSql("                                        AND JepDate <= TO_DATE(:TDATE, 'YYYY-MM-DD')                                                   ");
            parameter.AppendSql("                                        AND DelDate IS NULL                                                                            "); //삭제는 제외
            parameter.AppendSql("                                        AND GJYEAR = :GJYEAR                                                                           ");
            parameter.AppendSql("                                        AND (MirNo1 IS NULL OR MirNo1 = 0)                                                             "); //미청구만
            parameter.AppendSql("                                        AND GBDENTAL = 'Y'                                                                             "); //구강검사가 있는것
            parameter.AppendSql("                                        AND WRTNO NOT IN (SELECT WRTNO FROM HIC_MIR_ERROR_TONGBO WHERE OKDate IS NULL AND Gubun='1')   ");
            parameter.AppendSql("                                        AND TongboDate IS NOT NULL                                                                     "); //결과지 통보
            parameter.AppendSql("                                        AND (MirNo2 IS NULL OR MirNo2 = 0)                                                             "); //구강검사 미청구만
            parameter.AppendSql("                                        AND BURATE NOT IN ('2','02','3','03')                                                          ");
            if (!nWrtNo.IsNullOrEmpty() && nWrtNo != 0)
            {
                parameter.AppendSql("                                        AND WRTNO = :WRTNO                                                                         ");
            }
            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql("                                        AND LTDCODE = :LTDCODE                                                                     ");
            }
            if (!strBogunso.IsNullOrEmpty())
            {
                parameter.AppendSql("                                        AND BOGUNSO = :BOGUNSO                                                                     ");
            }
            parameter.AppendSql("                                        AND GJJONG IN (:GJJONG)                                                                        ");
            parameter.AppendSql("                                        AND Code IN(SELECT Code FROM KOSMOS_PMPA.HIC_CODE WHERE GUBUN = 'M1' AND GCODE = '001')        ");
            parameter.AppendSql("                                        AND RTRIM(GBSELF) NOT IN( '1','01')                                                            ");
            parameter.AppendSql("                                      GROUP BY WRTNO                                                                                   ");
            parameter.AppendSql("                                    )                                                                                                  ");
            parameter.AppendSql("                       AND Code IN(SELECT Code FROM KOSMOS_PMPA.HIC_CODE WHERE GUBUN = 'M1' AND GCODE = '001')                         ");
            parameter.AppendSql("                       AND RTRIM(GBSELF) NOT IN( '1','01')                                                                             ");
            parameter.AppendSql("                     GROUP BY WRTNO                                                                                                    ");
            parameter.AppendSql("                   )                                                                                                                   ");
            //=============================================================================================================================================================
            //위의 READ_HIC_MIR_공단부담체크 라인과 순서가 바뀌면 안됨
            //=============================================================================================================================================================
            parameter.AppendSql("   AND WRTNO NOT IN(SELECT WRTNO FROM HIC_MIR_ERROR_TONGBO WHERE OKDate IS NULL AND Gubun = '3')           ");
            switch (strJohap2)
            {
                case "사업장":
                    parameter.AppendSql(" ORDER BY WRTNO                                                                                    ");
                    break;
                case "공무원":
                    parameter.AppendSql(" ORDER BY WRTNO                                                                                    ");
                    break;
                case "성인병":
                    parameter.AppendSql(" ORDER BY GKIHO                                                                                    ");
                    break;
                case "통합":
                    parameter.AppendSql(" ORDER BY WRTNO                                                                                    ");
                    break;
                default:
                    break;
            }

            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);
            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!nWrtNo.IsNullOrEmpty() && nWrtNo != 0)
            {
                parameter.Add("WRTNO", nWrtNo);
            }
            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", strLtdCode);
            }
            if (!strBogunso.IsNullOrEmpty())
            {
                parameter.Add("BOGUNSO", strBogunso, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            parameter.AddInStatement("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetCancerListByDate(string strFDate, string strTDate, string strJob, string strSort,long nLicense, List<string> strJobSabun,string strHea, string strSName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, SNAME, TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE     ");
            parameter.AppendSql("     , AGE || '/' || SEX  AS SEX                               ");
            parameter.AppendSql("     , CASE WHEN SUBSTR(GBAM, 1, 1) = '1' THEN '○'             ");
            parameter.AppendSql("            WHEN SUBSTR(GBAM, 3, 1) = '1' THEN '○'             ");
            parameter.AppendSql("       ELSE '' END AS GBAM1                                    ");
            parameter.AppendSql("     , DECODE(SUBSTR(GBAM, 5,  1), '1', '○', '') AS GBAM2      ");
            parameter.AppendSql("     , DECODE(SUBSTR(GBAM, 7,  1), '1', '○', '') AS GBAM3      ");
            parameter.AppendSql("     , DECODE(SUBSTR(GBAM, 9,  1), '1', '○', '') AS GBAM4      ");
            parameter.AppendSql("     , DECODE(SUBSTR(GBAM, 11, 1), '1', '○', '') AS GBAM5      ");
            parameter.AppendSql("     , DECODE(SUBSTR(GBAM, 13, 1), '1', '○', '') AS GBAM6      ");
            parameter.AppendSql("     , CASE WHEN IEMUNNO > 0 THEN 'I'                          ");
            parameter.AppendSql("            WHEN GBMUNJIN1 = 'Y' THEN 'P' ELSE '' END GBIEMUNJIN   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')                ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')                ");
            parameter.AppendSql("   AND DELDATE IS NULL                                         ");
            parameter.AppendSql("   AND (SEXAMS <> '3170,' AND SEXAMS <> '3170')            ");
            if (strJob == "NEW")
            {
                parameter.AppendSql("   AND (PANJENGDRNO IS NULL OR PANJENGDRNO = 0)          ");

                if (nLicense > 0)
                {
                    if (strHea == "Y")
                    {
                        parameter.AppendSql("   AND SANGDAMDRNO IN (:JOBSABUN)                          ");
                    }
                    else
                    {
                        parameter.AppendSql("   AND SANGDAMDRNO NOT IN (:JOBSABUN)                      ");
                    }
                }    
            }
            else if (strJob == "OLD")
            {
                parameter.AppendSql("   AND PANJENGDRNO > 0                                   ");
                if (nLicense > 0)
                {
                    if (strHea == "Y")
                    {
                        parameter.AppendSql("   AND SANGDAMDRNO IN (:JOBSABUN)                          ");
                    }
                }
            }

            if (!strSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SNAME LIKE :SNAME                                                       ");
            }
            
            parameter.AppendSql("   AND GJJONG ='31'                                            ");

            if (strSort == "1")
            {
                parameter.AppendSql(" ORDER By SNAME, JEPDATE                                            ");
            }
            else if (strSort == "2")
            {
                parameter.AppendSql(" ORDER By JEPDATE, SNAME                                           "); 
            }
            else
            {
                parameter.AppendSql(" ORDER By WRTNO                                           ");
            }

            if (!strSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", strSName);
            }

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            if (strJob == "NEW")
            {
                if (nLicense > 0 || strHea == "Y")
                {
                    parameter.AddInStatement("JOBSABUN", strJobSabun);
                }
            }
            else if (strJob == "OLD")
            {
                if (nLicense > 0 && strHea == "Y")
                {
                    parameter.AddInStatement("JOBSABUN", strJobSabun);
                }
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDateMriNo(string argFrDate, string argToDate, long argMirno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,Pano,Kiho,GjJong,JikGbn,Sname,Sex,Age,GbChul      ");
            parameter.AppendSql("     , TO_CHAR(JepDate,'YYYY-MM-DD') JepDate                   ");
            parameter.AppendSql("     , TO_CHAR(DelDate,'YYYY-MM-DD') DelDate                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')                ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')                ");
            parameter.AppendSql("   AND MIRNO1  = :MIRNO1                                       ");
            parameter.AppendSql("   AND GJCHASU ='2'                                            ");

            parameter.Add("MIRNO1", argMirno);
            parameter.Add("FRDATE", argFrDate);
            parameter.Add("TODATE", argToDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetMisuno2byMirNo(long nMirno, string strGubun)
        {
            MParameter parameter = CreateParameter();
            if (strGubun == "1")
            {
                parameter.AppendSql("SELECT MISUNO1                                                 ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
                parameter.AppendSql(" WHERE MIRNO = :MIRNO                                          ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql("SELECT MISUNO2                                                 ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
                parameter.AppendSql(" WHERE MIRNO1 = :MIRNO                                         ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql("SELECT MISUNO3                                                 ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
                parameter.AppendSql(" WHERE MIRNO2= :MIRNO                                          ");
            }

            parameter.Add("MIRNO", nMirno);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetMisuNo2MisuNo4byMirNo3(long nMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MISUNO2, MISUNO4                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE MIRNO3 = :MIRNO                                         ");

            parameter.Add("MIRNO", nMirno);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int UpdatebyMirNo1(long nMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET               ");
            parameter.AppendSql("       MIRNO1 = 0                              ");
            parameter.AppendSql(" WHERE MIRNO1 = :MIRNO                         ");

            parameter.Add("MIRNO", nMirno);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateMirno2byMirNo2(long nMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET               ");
            parameter.AppendSql("       MIRNO2 = 0                              ");
            parameter.AppendSql(" WHERE MIRNO2 = :MIRNO                         ");

            parameter.Add("MIRNO", nMirno);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateMirNo3byMirNo3(long nMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET               ");
            parameter.AppendSql("       Mirno3 = 0                              ");
            parameter.AppendSql(" WHERE MIRNO3 = :MIRNO                         ");

            parameter.Add("MIRNO", nMirno);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateMirNo5byMirNo5(long nMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET               ");
            parameter.AppendSql("       Mirno5 = 0                              ");
            parameter.AppendSql(" WHERE MIRNO5 = :MIRNO                         ");

            parameter.Add("MIRNO", nMirno);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetMisuNo4byMirNo5(long nMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MISUNO4                                                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE MIRNO5 = :MIRNO                                         ");

            parameter.Add("MIRNO", nMirno);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetCancerItembyJepDateGjYear(string strGubun, string strFDate, string strTDate, string strLife, string strKiho, string strW_Am, string strYear, long nWrtNo, string strLtdCode, string strBogunso, string strJohap, string strJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,GjJong,GjChasu,Sname,Age,Sex,SExams,UCodes,Kiho,GbSts       ");
            parameter.AppendSql("     , SECOND_Flag,TO_DATE(SECOND_Date,'YYYY-MM-DD') Second_Date,Gkiho                                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                                                   ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                                                 ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TRDATE,'YYYY-MM-DD')                                                                ");
            if (strLife == "1")
            {
                parameter.AppendSql("   AND GjJong IN ('31')                                                                                    ");     //암검진
            }
            else
            {
                parameter.AppendSql("   AND GjJong IN ('35')                                                                                    ");     //암검진 생애
            }
            if (strGubun == "1")
            {
                parameter.AppendSql("   AND SExams LIKE '%3116%'                                                                                ");
            }
            if (strGubun == "")
            {
                parameter.AppendSql("   AND SExams NOT LIKE '%3170%'                                                                            ");
            }
            parameter.AppendSql("   AND DelDate IS NULL                                                                                         "); //삭제는 제외
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                                                        ");
            parameter.AppendSql("   AND TongboDate IS NOT NULL                                                                                  ");  //결과지통보
            parameter.AppendSql("   AND BURATE NOT IN ('2','02')                                                                                ");
            if (strJohap == "사업장")
            {
                parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('7','8')                                                                      ");
            }
            else if (strJohap == "공무원")
            {
                parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('5','6')                                                                      ");
            }
            else if (strJohap == "성인병")
            {
                parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('1','2','3')                                                                  ");
            }
            if (!strLtdCode.IsNullOrEmpty() && strLtdCode != "0")
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                                                  ");
            }
            if (!strBogunso.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND BOGUNSO = :BOGUNSO                                                                                  ");
            }
            if (strJong == "4")
            {
                parameter.AppendSql("   AND (MirNo3 IS NULL OR MirNo3 = 0 )                                                                     ");  //미청구만
                parameter.AppendSql("   AND (MileageAm<>'Y' OR MileageAm IS NULL)                                                               ");  //공단암
                parameter.AppendSql("   AND (GubDaeSang <> 'Y' OR GubDaeSang IS NULL )                                                          ");
                if (!strKiho.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND KIHO = :KIHO                                                                                    ");
                }
            }
            else if (strJong == "E")//'의료보험암
            {
                parameter.AppendSql("   AND (MirNo5 IS NULL OR MirNo5 = 0)                                                                      ");  //미청구만
                parameter.AppendSql("   AND GubDaeSang ='Y'                                                                                     ");
                if (!strKiho.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND KIHO = :KIHO                                                                                    ");
                }
            }
            if (!nWrtNo.IsNullOrEmpty() && nWrtNo != 0)
            {
                parameter.AppendSql("   AND WRTNO = :WRTNO                                                                                      ");
            }

            //자궁경부암만
            if (strW_Am == "1")
            {
                parameter.AppendSql("   AND GBAM = '0,0,0,0,0,1,'                                                                               ");
            }
            parameter.AppendSql("   AND BURATE NOT IN ('2','02','3','03')                                                                       ");
            parameter.AppendSql("   AND WRTNO NOT IN (SELECT WRTNO FROM HIC_MIR_ERROR_TONGBO WHERE OKDate IS NULL AND Gubun IN ('4','E'))       ");
            parameter.AppendSql(" ORDER BY WRTNO                                                                                                ");

            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);
            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!nWrtNo.IsNullOrEmpty() && nWrtNo != 0)
            {
                parameter.Add("WRTNO", nWrtNo);
            }
            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", strLtdCode);
            }
            if (!strBogunso.IsNullOrEmpty())
            {
                parameter.Add("BOGUNSO", strBogunso, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (!strKiho.IsNullOrEmpty())
            {
                parameter.Add("KIHO", strKiho, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetWrtNoJepDatebyMirNo(long fnMirNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, TO_CHAR(JepDate,'YYYY-MM-DD') JepDate                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                               ");
            parameter.AppendSql(" WHERE MIRNO3 = :MIRNO                                                     ");
            parameter.AppendSql("   AND MURYOAM = 'Y'                                                       ");
            parameter.AppendSql("   AND MURYOGBN > '0'                                                      ");

            parameter.Add("MIRNO", fnMirNo);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetWrtNobyMirno(long fnMirNo, string sGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                                                               ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_JEPSU                                               ");
            if (sGubun == "1")
            {
                parameter.AppendSql(" WHERE MIRNO1 = :MIRNO                                                 ");
            }
            else if (sGubun == "2")
            {
                parameter.AppendSql(" WHERE MIRNO2 = :MIRNO                                                 ");
            }
            else if (sGubun == "3")
            {
                parameter.AppendSql(" WHERE MIRNO3 = :MIRNO                                                 ");
            }
            else if (sGubun == "4")
            {
                parameter.AppendSql(" WHERE MIRNO4 = :MIRNO                                                 ");
            }
            else if (sGubun == "5")
            {
                parameter.AppendSql(" WHERE MIRNO5 = :MIRNO                                                 ");
            }

            parameter.Add("MIRNO", fnMirNo);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetWrtNobyJepDateGjYear(string fRDATE, string tODATE, string strGjYear, long argMirno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO From KOSMOS_PMPA.HIC_JEPSU                                    ");
            parameter.AppendSql(" WHERE JepDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')                            ");
            parameter.AppendSql("   AND JepDate <= TO_DATE(:TODATE,'YYYY-MM-DD')                            ");
            parameter.AppendSql("   AND DelDate IS NULL                                                     ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                    ");
            parameter.AppendSql("   AND GbDental = 'Y'                                                      ");
            parameter.AppendSql("   AND MIRNO2 = :MIRNO2                                                    ");

            parameter.Add("FRDATE", fRDATE);
            parameter.Add("TODATE", tODATE);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("MIRNO2", argMirno);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetExpenseItembyJepDAteGjYearWrtNo(string argFDate, string argTDate, string strYear, string strLife, List<string> strWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GjYear,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,WRTNO,Kiho         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                           ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                 "); //삭제는 제외
            parameter.AppendSql("   AND GBSTS <> 'D'                                                    ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                ");
            if (strLife.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GjJong IN ('31')                                            "); //암검진
            }
            else
            {
                parameter.AppendSql("   AND GjJong IN ('35')                                            "); //암검진 생애
            }
            parameter.AppendSql("   AND WRTNO IN (:WRTNO)                                               ");
            parameter.AppendSql("   AND MURYOAM = 'Y'                                                   ");
            parameter.AppendSql("   AND MURYOGBN > '0'                                                  ");
            parameter.AppendSql("   AND Burate NOT IN ('2','02','3','03')                               ");
            parameter.AppendSql(" ORDER BY GjYear,JepDate,WRTNO                                         ");

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);
            parameter.Add("GJYEAR", strYear);
            parameter.AddInStatement("WRTNO", strWRTNO);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int UpdateMirNobyWrtNo(long nWRTNO, long nMirno, string sGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET               ");
            if (sGubun == "1")
            {
                parameter.AppendSql("       MIRNO1 = :MIRNO                     ");
            }
            else if (sGubun == "2")
            {
                parameter.AppendSql("       MIRNO2 = :MIRNO                     ");
            }
            else if (sGubun == "3")
            {
                parameter.AppendSql("       MIRNO3 = :MIRNO                     ");
            }
            else if (sGubun == "4")
            {
                parameter.AppendSql("       MIRNO4 = :MIRNO                     ");
            }
            else if (sGubun == "5")
            {
                parameter.AppendSql("       MIRNO5 = :MIRNO                     ");
            }
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("MIRNO", nMirno);
            parameter.Add("WRTNO", nWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetCancerJepDateWrtNoKinoPanobyJepDate(string argFDate, string argTDate, string strYear, string strLife, string argLtdCode, string strJohap, string strJong, string strkiho, string strWRTNO, string strW_Am, string strBogenso)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GjYear,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,WRTNO,Kiho,Pano        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                               ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                             ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                             ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                     "); //삭제는 제외
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                    ");
            parameter.AppendSql("   AND TongboDate IS NOT NULL                                              "); //결과지 통보
            parameter.AppendSql("   AND BURATE NOT IN ('2','02')                                            ");
            if (strLife.IsNullOrEmpty())
            {
                parameter.AppendSql("  AND GjJong IN ('31')                                                 "); //암검진
            }
            else
            {
                parameter.AppendSql("  AND GjJong IN ('35')                                                 "); //암검진 생애
            }
            if (!argLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                              ");
            }
            if (strJohap == "사업장")
            {
                parameter.AppendSql(" AND SUBSTR(Gkiho,1,1) IN ('7','8')                                    ");
            }
            else if (strJohap == "공무원")
            {
                parameter.AppendSql(" AND SUBSTR(Gkiho,1,1) IN ('5','6')                                    ");
            }
            else if (strJohap == "성인병")
            {
                parameter.AppendSql(" AND SUBSTR(Gkiho,1,1) IN ('1','2','3')                                ");
            }

            if (strJong == "4")
            {
                parameter.AppendSql("   AND (MirNo3 IS NULL OR MirNo3 = 0)                                  "); //미청구만
                parameter.AppendSql("   AND (MileageAm<>'Y' OR MileageAm IS NULL)                           "); //공단암
                parameter.AppendSql("   AND WRTNO NOT IN (SELECT WRTNO FROM HIC_MIR_ERROR_TONGBO WHERE OKDate IS NULL AND Gubun='4') ");
                parameter.AppendSql("   AND (GubDaeSang <> 'Y' OR GubDaeSang IS NULL )                      ");
                if (!argLtdCode.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND LTDCODE = :LTDCODE                                          ");
                }
                if (!strkiho.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND KIHO = :KIHO                                                ");
                }
            }
            else if (strJong == "E")//의료급여암
            {
                parameter.AppendSql("   AND (MirNo5 IS NULL OR MirNo5 = 0)                                  "); //미청구만
                parameter.AppendSql("   AND GubDaeSang = 'Y'                                                ");
                parameter.AppendSql("   AND WRTNO NOT IN (SELECT WRTNO FROM HIC_MIR_ERROR_TONGBO WHERE OKDate IS NULL AND Gubun='E') ");
                if (!argLtdCode.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND LTDCODE = :LTDCODE                                          ");
                }
                if (!strkiho.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND KIHO = :KIHO                                                ");
                }
            }
            if (!strWRTNO.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND WRTNO = :WRTNO                                                  ");
            }
            if (strW_Am == "1")
            {
                parameter.AppendSql("   AND ( SUBSTR(GbAm,1,1) + SUBSTR(GbAm,3,1) + SUBSTR(GbAm,5,1) + SUBSTR(GbAm,7,1) + BSTR(GbAm,9,1) + SUBSTR(GbAm,11,1) ) = 1 ");
                parameter.AppendSql("   AND SUBSTR(GbAm,11,1) = 1                                           ");
            }
            if (!strBogenso.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND BOGUNSO = :BOGUNSO                                              ");
            }
            parameter.AppendSql("   AND Burate NOT IN ('2','02','3','03')                                   ");
            parameter.AppendSql(" ORDER BY GjYear,JepDate,WRTNO                                             ");

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);
            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!strWRTNO.IsNullOrEmpty())
            {
                parameter.Add("WRTNO", strWRTNO);
            }
            if (!argLtdCode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", argLtdCode);
            }
            if (!strBogenso.IsNullOrEmpty())
            {
                parameter.Add("BOGUNSO", strBogenso, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (!strkiho.IsNullOrEmpty())
            {
                parameter.Add("KIHO", strkiho, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetDentalItembyJepDateGjYear(string argFDate, string argTDate, string strYear, List<string> strGjJong, long nWRTNO, string argLtdCode, string strBogenso, string strJohap2, string strKiho)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GJYEAR,TO_CHAR(JEPDATE,'YYYY-MM-DD') JepDate,WRTNO,Kiho                                                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                                                               ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                                                             ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                                                                             ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                                                                     "); //삭제는 제외
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                                                                    ");
            parameter.AppendSql("   AND GBDENTAL = 'Y'                                                                                                      "); //구강검진 대상자
            parameter.AppendSql("   AND TONGBODATE IS NOT NULL                                                                                              "); //결과지 통보
            parameter.AppendSql("   AND (MirNo2 IS NULL OR MirNo2 = 0)                                                                                      "); //미청구만
            parameter.AppendSql("   AND GJJONG IN (:GJJONG)                                                                                                 ");
            parameter.AppendSql("   AND BURATE NOT IN ('2','02','3','03')                                                                                   ");

            if (!nWRTNO.IsNullOrEmpty() && nWRTNO != 0)
            {
                parameter.AppendSql("   AND WRTNO in (:WRTNO)                                                                                               ");
            }
            if (!argLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                                                              ");
            }
            if (!strBogenso.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND BOGUNSO = :BOGUNSO                                                                                              ");
            }

            switch (strJohap2)
            {
                case "사업장":
                    if (string.Compare(argLtdCode, "0000") > 0)
                    {
                        parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                                                      ");
                    }
                    break;
                case "공무원":
                    if (string.Compare(argLtdCode, "0000") > 0)
                    {
                        parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                                                      ");
                    }
                    if (!strKiho.IsNullOrEmpty())
                    {
                        parameter.AppendSql("   AND KIHO = :KIHO                                                                                            ");
                    }
                    break;
                case "성인병":
                    if (argLtdCode == "급여")
                    {
                        parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('9')                                                                              ");
                    }
                    else if (argLtdCode == "직장")
                    {
                        parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('7','8')                                                                          ");
                    }
                    else if (argLtdCode == "공교")
                    {
                        parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('5','6')                                                                          ");
                    }
                    else if (argLtdCode == "지역")
                    {
                        parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('1','2','3')                                                                      ");
                    }
                    break;
                case "통합":
                    if (string.Compare(argLtdCode, "0000") > 0)
                    {
                        parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                                                      ");
                    }
                    break;
                default:
                    break;
            }
            //==============================================================================================================================================
            //READ_HIC_MIR_공단부담체크("1", strTempSQL)
            //==============================================================================================================================================
            parameter.AppendSql("   AND WRTNO NOT IN(SELECT WRTNO FROM KOSMOS_PMPA.HIC_SUNAPDTL                                                             ");
            parameter.AppendSql("                     WHERE WRTNO IN (SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU                                               ");
            parameter.AppendSql("                                      WHERE JEPDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                        ");
            parameter.AppendSql("                                        AND JEPDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                                        ");
            parameter.AppendSql("                                        AND DELDATE IS NULL                                                                "); //삭제는 제외
            parameter.AppendSql("                                        AND GJYEAR = :GJYEAR                                                               ");
            parameter.AppendSql("                                        AND GBDENTAL = 'Y'                                                                 "); //구강검진 대상자
            parameter.AppendSql("                                        AND TONGBODATE IS NOT NULL                                                         "); //결과지 통보
            parameter.AppendSql("                                        AND (MirNo2 IS NULL OR MirNo2 = 0)                                                 "); //미청구만
            parameter.AppendSql("                                        AND GJJONG IN (:GJJONG)                                                            ");
            parameter.AppendSql("                                        AND BURATE NOT IN ('2','02','3','03')                                              ");

            if (!nWRTNO.IsNullOrEmpty() && nWRTNO != 0)
            {
                parameter.AppendSql("                                    AND WRTNO in (:WRTNO)                                                              ");
            }
            if (!argLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql("                                    AND LTDCODE = :LTDCODE                                                             ");
            }
            if (!strBogenso.IsNullOrEmpty())
            {
                parameter.AppendSql("                                    AND BOGUNSO = :BOGUNSO                                                             ");
            }

            switch (strJohap2)
            {
                case "사업장":
                    if (string.Compare(argLtdCode, "0000") > 0)
                    {
                        parameter.AppendSql("                                    AND LTDCODE = :LTDCODE                                                     ");
                    }
                    break;
                case "공무원":
                    if (string.Compare(argLtdCode, "0000") > 0)
                    {
                        parameter.AppendSql("                                    AND LTDCODE = :LTDCODE                                                     ");
                    }
                    if (!strKiho.IsNullOrEmpty())
                    {
                        parameter.AppendSql("                                    AND KIHO = :KIHO                                                           ");
                    }
                    break;
                case "성인병":
                    if (argLtdCode == "급여")
                    {
                        parameter.AppendSql("                                    AND SUBSTR(Gkiho,1,1) IN ('9')                                             ");
                    }
                    else if (argLtdCode == "직장")
                    {
                        parameter.AppendSql("                                    AND SUBSTR(Gkiho,1,1) IN ('7','8')                                         ");
                    }
                    else if (argLtdCode == "공교")
                    {
                        parameter.AppendSql("                                    AND SUBSTR(Gkiho,1,1) IN ('5','6')                                         ");
                    }
                    else if (argLtdCode == "지역")
                    {
                        parameter.AppendSql("                                    AND SUBSTR(Gkiho,1,1) IN ('1','2','3')                                     ");
                    }
                    break;
                case "통합":
                    if (string.Compare(argLtdCode, "0000") > 0)
                    {
                        parameter.AppendSql("                                    AND LTDCODE = :LTDCODE                                                     ");
                    }
                    break;
                default:
                    break;
            }
            parameter.AppendSql("                                    )                                                                                      ");
            parameter.AppendSql("                       AND Code IN(SELECT Code FROM KOSMOS_PMPA.HIC_CODE WHERE GUBUN = 'M1' AND GCODE = '001')             ");
            parameter.AppendSql("                       AND RTRIM(GBSELF) NOT IN( '1','01')                                                                 ");
            parameter.AppendSql("                     GROUP BY WRTNO                                                                                        ");
            parameter.AppendSql("                   )                                                                                                       ");
            //==============================================================================================================================================
            //위의 READ_HIC_MIR_공단부담체크 라인과 순서가 바뀌면 안됨
            //==============================================================================================================================================
            parameter.AppendSql("   AND WRTNO NOT IN (SELECT WRTNO FROM HIC_MIR_ERROR_TONGBO WHERE OKDate IS NULL AND Gubun='3')                            ");
            parameter.AppendSql(" ORDER BY GjYear,JepDate,WRTNO                                                                                             ");

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);
            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!nWRTNO.IsNullOrEmpty() && nWRTNO != 0)
            {
                parameter.Add("WRTNO", nWRTNO);
            }
            if (!argLtdCode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", argLtdCode);
            }
            if (!strBogenso.IsNullOrEmpty())
            {
                parameter.Add("BOGUNSO", strBogenso, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (!strKiho.IsNullOrEmpty())
            {
                parameter.Add("KIHO", strKiho, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (!strGjJong.IsNullOrEmpty())
            {
                parameter.AddInStatement("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetBohumItembyJepDateGjYear(string argFDate, string argTDate, string strYear, List<string> strGjJong, long nWRTNO, string argLtdCode, string strBogenso, string strJohap2, string strKiho)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Pano,GjYear,GjChasu,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,WRTNO,Kiho,Gkiho,SExams,GjJong    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                                       ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                                             "); //삭제는 제외
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                                            ");
            parameter.AppendSql("   AND (MirNo1 IS NULL OR MirNo1 = 0)                                                              "); //미청구만
            parameter.AppendSql("   AND PanjengDate IS NOT NULL                                                                     ");
            parameter.AppendSql("   AND TongboDate IS NOT NULL                                                                      "); //결과지 통보
            parameter.AppendSql("   AND WRTNO NOT IN (SELECT WRTNO FROM HIC_MIR_ERROR_TONGBO WHERE OKDate IS NULL AND Gubun='1')    ");
            parameter.AppendSql("   AND GJJONG IN (:GJJONG)                                                                         ");
            parameter.AppendSql("   AND BURATE NOT IN ('2','02','3','03')                                                           ");
            if (!nWRTNO.IsNullOrEmpty() && nWRTNO != 0)
            {
                parameter.AppendSql("   AND WRTNO = :WRTNO                                                                          ");
            }
            if (!argLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                                      ");
            }
            if (!strBogenso.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND BOGUNSO = :BOGUNSO                                                                      ");
            }

            switch (strJohap2)
            {
                case "사업장":
                    if (string.Compare(argLtdCode, "0000") > 0)
                    {
                        parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                              ");
                    }
                    break;
                case "공무원":
                    if (string.Compare(argLtdCode, "0000") > 0)
                    {
                        parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                              ");
                    }
                    if (!strKiho.IsNullOrEmpty())
                    {
                        parameter.AppendSql("   AND KIHO = :KIHO                                                                    ");
                    }
                    break;
                case "성인병":
                    if (argLtdCode == "급여")
                    {
                        parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('9')                                                      ");
                    }
                    else if (argLtdCode == "직장")
                    {
                        parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('7','8')                                                  ");
                    }
                    else if (argLtdCode == "공교")
                    {
                        parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('5','6')                                                  ");
                    }
                    else if (argLtdCode == "지역")
                    {
                        parameter.AppendSql("   AND SUBSTR(Gkiho,1,1) IN ('1','2','3')                                              ");
                    }
                    break;
                case "통합":
                    if (string.Compare(argLtdCode, "0000") > 0)
                    {
                        parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                              ");
                    }
                    break;
                default:
                    break;
            }
            parameter.AppendSql(" ORDER BY Pano,GjYear,GjChasu,JepDate,WRTNO                                                        ");

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);
            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!nWRTNO.IsNullOrEmpty() && nWRTNO != 0)
            {
                parameter.Add("WRTNO", nWRTNO);
            }
            if (!argLtdCode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", argLtdCode);
            }
            if (!strBogenso.IsNullOrEmpty())
            {
                parameter.Add("BOGUNSO", strBogenso, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (!strKiho.IsNullOrEmpty())
            {
                parameter.Add("KIHO", strKiho, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            parameter.AddInStatement("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetExpenseItembyJepDateGjYear(string strFDate, string strTDate, string strYear, long nWrtNo, string strLtdCode, string strBogunso, List<string> strGjJong, string strLife66, string strLife2018, string strJohap2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT LtdCode,WRTNO,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,GjJong,GjChasu,Sname, Age, Sex,GbSts            ");
            parameter.AppendSql("     , SECOND_Flag,TO_CHAR(SECOND_Date,'YYYY-MM-DD') Second_Date,Gkiho,SExams,UCodes,Kiho                  ");
            parameter.AppendSql("     , DECODE(SUBSTR(Gkiho,1,1),'1', '지역' ,                                                              ");
            parameter.AppendSql("                                '2', '지역' ,                                                              ");
            parameter.AppendSql("                                '3', '지역' ,                                                              ");
            parameter.AppendSql("                                '5', '공교' ,                                                              ");
            parameter.AppendSql("                                '6', '공교' ,                                                              ");
            parameter.AppendSql("                                '7', '직장' ,                                                              ");
            parameter.AppendSql("                                '8', '직장',                                                               ");
            parameter.AppendSql("                                '9', '급여'                                                                ");
            parameter.AppendSql("                                      ) as JOHAP                                                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                                               ");
            parameter.AppendSql(" WHERE JepDate>=TO_DATE(:FDATE, 'YYYY-MM-DD')                                                              ");
            parameter.AppendSql("   AND JepDate<=TO_DATE(:TDATE, 'YYYY-MM-DD')                                                              ");
            parameter.AppendSql("   AND DelDate IS NULL                                                                                     "); //삭제는 제외
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                                                    ");
            parameter.AppendSql("   AND (MirNo1 IS NULL OR MirNo1 = 0)                                                                      "); //미청구만
            parameter.AppendSql("   AND (GbDentOnly IS NULL OR GbDentOnly = 'N')                                                            "); //구강검진만 한 수검자는 제외
            parameter.AppendSql("   AND WRTNO NOT IN (SELECT WRTNO FROM HIC_MIR_ERROR_TONGBO WHERE OKDate IS NULL AND Gubun='1')            ");
            parameter.AppendSql("   AND PanjengDate IS NOT NULL                                                                             "); //판정완료
            parameter.AppendSql("   AND TongboDate IS NOT NULL                                                                              "); //결과지 통보
            parameter.AppendSql("   AND BURATE NOT IN ('2','02','3','03')                                                                   ");
            if (!nWrtNo.IsNullOrEmpty() && nWrtNo != 0)
            {
                parameter.AppendSql("   AND WRTNO = :WRTNO                                                                                  ");
            }
            if (strLife66 == "1")
            {
                parameter.AppendSql("   AND GBCHK2 IS NULL                                                                                  ");  //의료급여비대상만조회
            }
            if (strLife2018 == "1")
            {
                parameter.AppendSql("   AND (AGE <>'40' AND AGE <>'50' AND AGE <>'60' AND AGE <>'70')                                       ");  //생활습관 제외           
            }
            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                                              ");
            }
            if (!strBogunso.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND BOGUNSO = :BOGUNSO                                                                              ");
            }
            parameter.AppendSql("   AND GJJONG IN (:GJJONG)                                                                                 ");
            switch (strJohap2)
            {
                case "사업장":
                    parameter.AppendSql(" ORDER BY WRTNO                                                                                    ");
                    break;
                case "공무원":
                    parameter.AppendSql(" ORDER BY WRTNO                                                                                    ");
                    break;
                case "성인병":
                    parameter.AppendSql(" ORDER BY GKIHO                                                                                    ");
                    break;
                case "통합":
                    parameter.AppendSql(" ORDER BY WRTNO                                                                                    ");
                    break;
                default:
                    break;
            }

            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);
            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!nWrtNo.IsNullOrEmpty() && nWrtNo != 0)
            {
                parameter.Add("WRTNO", nWrtNo);
            }
            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", strLtdCode);
            }
            if (!strBogunso.IsNullOrEmpty())
            {
                parameter.Add("BOGUNSO", strBogunso, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            parameter.AddInStatement("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetMirNo1byWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MIRNO1                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU              ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                     ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public void UpdatePanjengInfobyWrtno(long fnWRTNO, string strDate, long nDrNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET                           ");
            if (nDrNO > 0)
            {
                parameter.AppendSql("       TongboDate = TO_DATE(:TongboDate, 'YYYY-MM-DD')     ");
                parameter.AppendSql("     , PanjengDate = TO_DATE(:PanjengDate, 'YYYY-MM-DD')   ");
            }
            else
            {
                parameter.AppendSql("       TongboDate = ''     ");
                parameter.AppendSql("     , PanjengDate = ''   ");
            }
            parameter.AppendSql("     , PanjengDrNo = :PanjengDrNo                          ");
            parameter.AppendSql("  WHERE WRTNO    = :WRTNO                                  ");

            if (nDrNO > 0)
            {
                parameter.Add("TongboDate", strDate);
                parameter.Add("PanjengDate", strDate);
            }

            parameter.Add("PanjengDrNo", nDrNO);
            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public void UpdateNotPanjengbyWrtno(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU       ");
            parameter.AppendSql("   SET TongboDate = ''             ");
            parameter.AppendSql("     , PanjengDate = ''            ");
            parameter.AppendSql("     , PanjengDrNo = 0             ");
            parameter.AppendSql("     , WEBPRINTSEND = ''           ");
            parameter.AppendSql("     , PRTSABUN = ''               ");
            parameter.AppendSql("  WHERE WRTNO    = :WRTNO          ");

            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public HIC_JEPSU GetItemHicHeabyWrtNo(string fstrGubun, long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            if (fstrGubun == "HIC")
            {
                parameter.AppendSql("SELECT PANO, WRTNO, SNAME, AGE, SEX, LTDCODE, TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE, GJYEAR, PTNO  ");
                parameter.AppendSql("     , TO_CHAR(IPSADATE,'yyyy-mm-dd') IPSADATE, GJJONG, UCODES, SEXAMS, ERFLAG, WAITREMARK         ");
                parameter.AppendSql("     , SABUN, BUSENAME, GBSUCHEP, JIKJONG, GJCHASU, GJBANGI                                        ");
                parameter.AppendSql("     , JONGGUMYN, GBHEAENDO, IEMUNNO, GBN, CLASS, GBCHK3                                           ");
                parameter.AppendSql("     , TEL, BUSEIPSA, SANGDAMDRNO                                                                  ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                                       ");
                parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                              ");
                parameter.AppendSql("   AND DELDATE IS NULL                                                                             ");
            }
            else
            {
                parameter.AppendSql("SELECT PANO, WRTNO, SNAME, AGE, SEX, LTDCODE, TO_CHAR(SDATE,'YYYY-MM-DD') JEPDATE, '' GJYEAR, PTNO ");
                parameter.AppendSql("     , '' IPSADATE, GJJONG, '' UCODES, SEXAMS, '' ERFLAG, '' WAITREMARK                            ");
                parameter.AppendSql("     , SABUN, '' BUSENAME, '' GBSUCHEP, '' JIKJONG, '' GJCHASU, '' GJBANGI                         ");
                parameter.AppendSql("     , '' JONGGUMYN, '' GBHEAENDO, IEMUNNO, '' GBN, '' CLASS, '' GBCHK3                            ");
                parameter.AppendSql("     , '' TEL, '' BUSEIPSA, DRSABUN SANGDAMDRNO                                                    ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                                                                       ");
                parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                              ");
                parameter.AppendSql("   AND DELDATE IS NULL                                                                             ");
            }

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public int GetCountbyPtNoGjJongJepDate(string pTNO, string strGjJong, string strJEPDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT                                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                           ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                                    ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                       ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                 ");
            parameter.AppendSql("   AND GJJONG <> :GJJONG                                               ");

            parameter.Add("PTNO", pTNO);
            parameter.Add("JEPDATE", strJEPDATE);
            parameter.Add("GJJONG", strGjJong);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDate(string strJobDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, PANO, SNAME, SEX, AGE, GJJONG, PTNO, XRAYNO, LTDCODE     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                           ");
            parameter.AppendSql(" WHERE JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                       ");
            parameter.AppendSql("   AND DelDate IS NULL                                                 ");
            parameter.AppendSql("   AND GbChul = 'Y'                                                    "); //출장검진
            parameter.AppendSql("   AND XRayNo IS NOT NULL                                              ");

            parameter.Add("JEPDATE", strJobDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public void UpdateTongBoInfobyWrtNo(long fnWRTNO, string strDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET                      ");
            if (!strDate.IsNullOrEmpty())
            {
                parameter.AppendSql("       TONGBODATE  = TO_DATE(:TONGBODATE, 'YYYY-MM-DD')");
            }
            else
            {
                parameter.AppendSql("       TONGBODATE  = ''                                ");
                parameter.AppendSql("     , GBPRINT = ''                                   ");
                parameter.AppendSql("     , PRTSABUN  = ''                                  ");
            }

            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                     ");

            if (!strDate.IsNullOrEmpty())
            {
                parameter.Add("TONGBODATE", strDate);
            }

            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public HIC_JEPSU GetWrtNoJepDatebyPaNoGjJongGjYear(long fnPano, string strGjJong, string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                           ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            if (!fnPano.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND PANO   = :PANO                              ");
            }
            if (!strGjJong.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GJJONG = :GJJONG                            ");
            }
            parameter.AppendSql("   AND DELDATE IS NUll                                 "); //출장검진
            if (!strGjYear.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GJYEAR = :GJYEAR                            ");
            }

            if (!fnPano.IsNullOrEmpty())
            {
                parameter.Add("PANO", fnPano);
            }
            if (!strGjJong.IsNullOrEmpty())
            {
                parameter.Add("GJJONG", strGjJong);
            }
            if (!strGjYear.IsNullOrEmpty())
            {
                parameter.Add("GJYEAR", strGjYear);
            }

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetCountbyPtNoJepDate(string pTNO, string strBDate, string strJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT JEPDATE, PTNO                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                               ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                        ");
            parameter.AppendSql("   AND JEPDATE >= TO_DATE(:BDATE, 'YYYY-MM-DD')            ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:JEPDATE, 'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND GJJONG = '31'                                       ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')                    ");

            parameter.Add("PTNO", pTNO);
            parameter.Add("BDATE", strBDate);
            parameter.Add("JEPDATE", strJepDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDateLtdCodeJong(string strFrDate, string strToDate, string strJong, long nLtdCode, string strJob)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,SName,TO_CHAR(JepDate,'YY-MM-DD') JepDate,GjJong,ltdcode  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                           ");
            parameter.AppendSql(" WHERE JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                       ");
            parameter.AppendSql("   AND JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                       ");
            parameter.AppendSql("   AND DelDate IS NULL                                                 ");
            if (strJob == "1")
            {
                parameter.AppendSql("   AND GbMunjin3 = 'N'                                             ");
            }
            else
            {
                parameter.AppendSql("   AND GbMunjin3 = 'Y'                                             ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                          ");
            }
            if (!strJong.IsNullOrEmpty() && strJong != "**")
            {
                parameter.AppendSql("   AND GJJONG = :GJJONG                                            ");
            }
            parameter.AppendSql(" ORDER BY SName,WRTNO,Jepdate                                          ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strJong.IsNullOrEmpty() && strJong != "**")
            {
                parameter.Add("GJJONG", strJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<long> GetCountByPtnoJepDate(string argPtno, string argJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU                    ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                        ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')                    ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("JEPDATE", argJepDate);

            return ExecuteReader<long>(parameter);
        }

        public int GetCountbyPaNo(long nPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(*) CNT                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU       ");
            parameter.AppendSql(" WHERE PANO = :PANO                ");

            parameter.Add("PANO", nPano);

            return ExecuteScalar<int>(parameter);
        }

        public string GetRowidByPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU     ");
            parameter.AppendSql(" WHERE PTNO = :PTNO              ");
            parameter.AppendSql("   AND JEPDATE >= TRUNC(SYSDATE) ");
            parameter.AppendSql("   AND DELDATE IS NULL           ");

            parameter.Add("PTNO", argPtno);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateGbMunjin1NullbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            parameter.AppendSql("       GBMUNJIN1 = ''                  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                 ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyPtNo(string strPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU           ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                    ");
            parameter.AppendSql("   AND JEPDATE >= TRUNC(SYSDATE)       ");
            parameter.AppendSql("   AND DELDATE IS NULL                 ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDateGjChasu()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,TO_CHAR(JEPDATE,'YYYY-MM-DD') JepDate,PANO    ");
            parameter.AppendSql("     , SNAME,GJJONG,GJCHASU,GJBANGI,GBSTS,GjYear           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                               ");
            parameter.AppendSql(" WHERE JEPDATE>=TRUNC(SYSDATE-90)                          ");
            parameter.AppendSql("   AND GJCHASU = '2'                                       ");
            parameter.AppendSql("   AND (FIRSTPANDRNO IS NULL OR FIRSTPANDRNO = 0)          ");
            parameter.AppendSql("   AND GjJong NOT IN ('50','51')                           ");
            parameter.AppendSql("   AND DELDATE IS NULL                                     ");

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetItembyPtNo(string strPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Sex,Age,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate           ");
            parameter.AppendSql("     , LTDCODE, TEL, TO_CHAR(BuseIpsa, 'YYYY-MM-DD') BuseIpsa  ");
            parameter.AppendSql("     , SANGDAMDRNO, UCODES                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                            ");
            parameter.AppendSql(" AND JEPDATE = TRUNC(SYSDATE)                                  ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public string GetIEMUNNObyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT IEMUNNO                                                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                          ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_JEPSU GetJepDAtebyJepDatePano(string strDate3, string strDate4, long pANO, string strJong, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JepDate,'YY/MM/DD') JepDate,ROWID           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                               ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("   AND DELDATE IS NULL                                     ");
            parameter.AppendSql("   AND GbSts NOT IN ('D')                                  ");
            parameter.AppendSql("   AND PANO = :PANO                                        ");
            if (strJong != "00")
            {
                parameter.AppendSql("   AND GJJONG = :GJJONG                                ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                              ");
            }
            parameter.Add("FRDATE", strDate3);
            parameter.Add("TODATE", strDate4);
            parameter.Add("PANO", pANO);
            if (strJong != "00")
            {
                parameter.Add("GJJONG", strJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetListByPtnoJepDate(string pTNO, string sDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,TO_CHAR(JEPDATE,'YYYY-MM-DD') JepDate,PANO    ");
            parameter.AppendSql("     , SNAME,GJJONG,GJCHASU,GJBANGI,GBSTS,GjYear,UCODES    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                               ");
            parameter.AppendSql(" WHERE JEPDATE=TO_DATE(:JEPDATE, 'YYYY-MM-DD')             ");
            parameter.AppendSql("   AND PTNO =:PTNO                                         ");
            parameter.AppendSql("   AND DELDATE IS NULL                                     ");

            parameter.Add("JEPDATE", sDATE);
            parameter.Add("PTNO", pTNO);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int UpdateDelDatebyFnWrtNo(HIC_JEPSU item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET           ");
            parameter.AppendSql("       DELDATE   = TRUNC(SYSDATE)          ");
            parameter.AppendSql("     , JOBSABUN  = :JOBSABUN               ");
            parameter.AppendSql("     , GBSTS     = :GBSTS                  ");
            parameter.AppendSql("     , GBMUNJIN1 = ''                      ");
            parameter.AppendSql("     , GBMUNJIN2 = ''                      ");
            parameter.AppendSql("     , ENTTIME   = SYSDATE                 ");
            parameter.AppendSql("  WHERE WRTNO    = :WRTNO                  ");

            parameter.Add("JOBSABUN", item.JOBSABUN);
            parameter.Add("GBSTS", item.GBSTS);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public string GetXrayNoByWrtno(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT XRAYNO                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_JEPSU> GetUnionGjJongbyWrtNo(long argWrtNo, string argJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GWRTNO, WRTNO, GJJONG, (SELECT NAME FROM HIC_EXJONG WHERE CODE = A.GJJONG) JONGNAME         ");
            parameter.AppendSql("     , GBADDPAN, UCODES ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU A                                                                     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                              ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                                   ");
            parameter.AppendSql("   AND GBSTS <> 'D'                                                                                "); //접수취소(삭제)는 제외
            parameter.AppendSql(" UNION ALL                                                                                         ");
            parameter.AppendSql("SELECT GWRTNO, WRTNO, '00' GJJONG, (SELECT NAME FROM HIC_EXJONG WHERE CODE = A.GJJONG) JONGNAME    ");
            parameter.AppendSql("     , '' AS GBADDPAN, '' AS UCODES ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU A                                                                     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                              ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("   AND GBSTS <> 'D'                                                                                "); //접수취소(삭제)는 제외
            parameter.AppendSql(" ORDER BY GJJONG                                                                                   ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("JEPDATE", argJepDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetWrtNobyPtNo(string argPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                ");
            parameter.AppendSql("   AND GJJONG IN ('11', '14', '16')                ");

            parameter.Add("PTNO", argPtNo);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetWrtNobyJepDatePtNo(string argPtNo, string argJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO, UCODES                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PTNO    = :PTNO                             ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND GJJONG IN ('11', '14', '16')                ");

            parameter.Add("PTNO", argPtNo);
            parameter.Add("JEPDATE", argJepDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET               ");
            parameter.AppendSql("       PANO = :PANO                            ");
            parameter.AppendSql(" WHERE PANO IN (SELECT PANO                    ");
            parameter.AppendSql("                  FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql("                 WHERE JUMIN2 = :JUMIN2        ");
            parameter.AppendSql("                   AND PANO <> :PANO)          ");

            parameter.Add("PANO", argPaNo);
            parameter.Add("JUMIN2", argJumin2);

            return ExecuteNonQuery(parameter);
        }

        public int InsertAllbySelect(long fnWrtNo, string strJong, string strGjChasu)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_JEPSU                                                              ");
            parameter.AppendSql("       (WRTNO,GJYEAR,JEPDATE,PANO,SNAME,SEX,AGE,GJJONG,GJCHASU,GJBANGI,GBCHUL                  ");
            parameter.AppendSql("      , GBSTS,GBINWON,DELDATE,LTDCODE,MAILCODE,JUSO1,JUSO2,TEL,PTNO,BURATE,JISA,KIHO,GKIHO     ");
            parameter.AppendSql("      , JIKGBN,UCODES,SEXAMS,JIKJONG,SABUN,IPSADATE,BUSENAME,OLDJIKJONG,BUSEIPSA,OLDSDATE      ");
            parameter.AppendSql("      , OLDEDATE,GBSUCHEP,BALYEAR,BALSEQ,GBEXAM,BOGUNSO,JEPSUGBN,YOUNGUPSO,JEPNO,GBSPCMUNJIN   ");
            parameter.AppendSql("      , GBPRINT,GBSABUN,GBJINCHAL,GBMUNJIN1,GBMUNJIN2,GBMUNJIN3,GBDENTAL,SECOND_FLAG           ");
            parameter.AppendSql("      , SECOND_DATE,SECOND_TONGBO,SECOND_EXAMS,SECOND_SAYU,CHUNGGUYN,DENTCHUNGGUYN             ");
            parameter.AppendSql("      , CANCERCHUNGGUYN,LIVER2,MAMT,FAMT,MILEAGEAM,MURYOAM,GUMDAESANG,JONGGUMYN,SEND           ");
            parameter.AppendSql("      , MILEAGEAMGBN,MURYOGBN,NEGODATE,JOBSABUN,ENTTIME,SECOND_MISAYU,MIRNO1,MIRNO2            ");
            parameter.AppendSql("      , MIRNO3,EMAIL,REMARK,OHMSNO,MISUNO1,FIRSTPANDRNO,ERFLAG,MIRNO4,MIRSAYU,ERTONGBO         ");
            parameter.AppendSql("      , MISUNO2,GUBDAESANG,MIRNO5,MISUNO3,XRAYNO,CARDSEQNO,GBADDPAN,GBAM,BALDATE,TONGBODATE    ");
            parameter.AppendSql("      , GBSUJIN_SET,GBCHUL2,GBJINCHAL2,SANGDAMDRNO,SANGDAMDATE,SENDMSG,GBN,CLASS,BAN           ");
            parameter.AppendSql("      , BUN , HEMSNO, HEMSMIRSAYU)                                                             ");
            parameter.AppendSql("SELECT :WRTNO,GJYEAR,JEPDATE,PANO,SNAME,SEX,AGE,:GJJONG,:GJCHASU,GJBANGI,GBCHUL                ");
            parameter.AppendSql("      , GBSTS,GBINWON,DELDATE,LTDCODE,MAILCODE,JUSO1,JUSO2,TEL,PTNO,BURATE,JISA,KIHO,GKIHO     ");
            parameter.AppendSql("      , JIKGBN,UCODES,SEXAMS,JIKJONG,SABUN,IPSADATE,BUSENAME,OLDJIKJONG,BUSEIPSA,OLDSDATE      ");
            parameter.AppendSql("      , OLDEDATE,GBSUCHEP,BALYEAR,BALSEQ,GBEXAM,BOGUNSO,JEPSUGBN,YOUNGUPSO,JEPNO,GBSPCMUNJIN   ");
            parameter.AppendSql("      , GBPRINT,GBSABUN,GBJINCHAL,GBMUNJIN1,GBMUNJIN2,GBMUNJIN3,GBDENTAL,SECOND_FLAG           ");
            parameter.AppendSql("      , SECOND_DATE,SECOND_TONGBO,SECOND_EXAMS,SECOND_SAYU,CHUNGGUYN,DENTCHUNGGUYN             ");
            parameter.AppendSql("      , CANCERCHUNGGUYN,LIVER2,MAMT,FAMT,MILEAGEAM,MURYOAM,GUMDAESANG,JONGGUMYN,SEND           ");
            parameter.AppendSql("      , MILEAGEAMGBN,MURYOGBN,NEGODATE,JOBSABUN,ENTTIME,SECOND_MISAYU,MIRNO1,MIRNO2            ");
            parameter.AppendSql("      , MIRNO3,EMAIL,REMARK,OHMSNO,MISUNO1,FIRSTPANDRNO,ERFLAG,MIRNO4,MIRSAYU,ERTONGBO         ");
            parameter.AppendSql("      , MISUNO2,GUBDAESANG,MIRNO5,MISUNO3,XRAYNO,CARDSEQNO,GBADDPAN,GBAM,BALDATE,TONGBODATE    ");
            parameter.AppendSql("      , GBSUJIN_SET,GBCHUL2,GBJINCHAL2,SANGDAMDRNO,SANGDAMDATE,SENDMSG,GBN,CLASS,BAN           ");
            parameter.AppendSql("      , BUN , HEMSNO, HEMSMIRSAYU                                                              ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_JEPSU                                                                  ");
            parameter.AppendSql("  WHERE WRTNO = :WRTNO                                                                         ");

            parameter.Add("GJJONG", strJong);
            parameter.Add("GJCHASU", strGjChasu);
            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetWrtNoJepDatePanjeng(long fnPano, string fstrJepDate, string strGubun)
        {
            MParameter parameter = CreateParameter();

            if (strGubun == "HIC")
            {
                parameter.AppendSql("SELECT a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate             ");
                parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_RES_BOHUM1_GET_PANJENG(a.WRTNO) PANJENG  ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a                                     ");
                parameter.AppendSql(" WHERE a.PANO = :PANO                                              ");
                parameter.AppendSql("   AND a.JEPDATE < TO_DATE(:JEPDATE, 'YYYY-MM-DD')                 ");
                parameter.AppendSql("   AND a.GjJong NOT IN('31', '35')                                 ");
                parameter.AppendSql("   AND a.GbSTS <> 'D'                                              "); //접수취소(삭제)는 제외
                parameter.AppendSql(" ORDER BY a.JepDate DESC                                           ");
            }
            else if (strGubun == "HEA")
            {
                parameter.AppendSql("SELECT a.WRTNO, TO_CHAR(a.SDate,'YYYY-MM-DD') JepDate              ");
                parameter.AppendSql("     , PANCODE,PANREMARK, SANGDAMGBN,DRNAME                        ");
                parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_RES_BOHUM1_GET_PANJENG(a.WRTNO) PANJENG  ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a                                     ");
                parameter.AppendSql(" WHERE a.PANO = :PANO                                              ");
                parameter.AppendSql("   AND a.SDATE < TO_DATE(:JEPDATE, 'YYYY-MM-DD')                   ");
                parameter.AppendSql("   AND a.GbSts NOT IN ('0','D')                                    ");
                parameter.AppendSql(" ORDER BY a.SDate DESC                                             ");
            }
            else if (strGubun == "CAN")
            {
                parameter.AppendSql("SELECT a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate             ");
                parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_RES_BOHUM1_GET_PANJENG(a.WRTNO) PANJENG  ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a                                     ");
                parameter.AppendSql(" WHERE a.PANO = :PANO                                              ");
                parameter.AppendSql("   AND a.JEPDATE < TO_DATE(:JEPDATE, 'YYYY-MM-DD')                 ");
                parameter.AppendSql("   AND a.GbSts NOT IN ('0','D')                                    ");
                parameter.AppendSql("   AND a.GjJong IN('31', '35')                                     ");
                parameter.AppendSql(" ORDER BY a.JEPDate DESC                                           ");
            }

            parameter.Add("PANO", fnPano);
            parameter.Add("JEPDATE", fstrJepDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetListGjNameByPtnoJepDate(string argDate, string argPtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GJJONG,KOSMOS_PMPA.FC_HIC_GJJONG_NAME(GJJONG, UCODES) AS NAME               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                       ");
            parameter.AppendSql(" WHERE 1 = 1                                                                       ");
            parameter.AppendSql("   AND PTNO = :PTNO                                                                ");
            parameter.AppendSql("   AND JEPDATE =TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                    ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                             ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("JEPDATE", argDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDateGbSts(string argBDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,GJYEAR,SEX,AGE,GBCHUL,WRTNO, PTNO     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                       ");
            parameter.AppendSql(" WHERE JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                             ");
            parameter.AppendSql("   AND GBSTS NOT IN ('2','D')                                                      ");

            parameter.Add("JEPDATE", argBDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetListByPtnoYearJepDate(string argPtno, string argYear, string argDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GJJONG,UCODES                                                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                       ");
            parameter.AppendSql(" WHERE 1 = 1                                                                       ");
            parameter.AppendSql("   AND PTNO = :PTNO                                                                ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                            ");
            parameter.AppendSql("   AND JEPDATE =TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                    ");
            parameter.AppendSql("   AND GJJONG IN ('11','12','16','17','23','41','42','44','45','31','35','62','69')");
            parameter.AppendSql("   AND DELDATE IS NULL                                                             ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("GJYEAR", argYear);
            parameter.Add("JEPDATE", argDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int UpdatePanjengDateDrNobyWrtNo(string strPanjengDrNo, string strPanDate, long fnWrtNo, string strTemp)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            if (strTemp == "Y")
            {
                parameter.AppendSql("       PANJENGDRNO = :PANJENGDRNO  ");
                parameter.AppendSql("     , PANJENGDATE = TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')  ");
            }
            else
            {
                parameter.AppendSql("       PANJENGDRNO = ''            ");
                parameter.AppendSql("     , PANJENGDATE = ''            ");
            }
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO              ");

            parameter.Add("WRTNO", fnWrtNo);
            if (strTemp == "Y")
            {
                parameter.Add("PANJENGDRNO", strPanjengDrNo);
                parameter.Add("PANJENGDATE", strPanDate);
            }

            return ExecuteNonQuery(parameter);
        }

        public int UpdateGbMunjin1byWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            parameter.AppendSql("       GBMUNJIN1 = 'Y'                 ");
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO              ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public HIC_JEPSU GetPanjengDatebyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PANJENGDATE, WRTNO                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public int GetGjJOngbyPtnoJepDate(string strDrno, string strBDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GJJONG                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND GJJONG IN ('21','22','24','29','69')        ");

            parameter.Add("PTNO", strDrno);
            parameter.Add("JEPDATE", strBDate);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateGbJinChal2byWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            parameter.AppendSql("       GBJINCHAL2 = 'Y'                ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO             ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatePanjengDrNoPanjengDateTongboDatebyWrtNo(long fnDrno, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            parameter.AppendSql("       PANJENGDRNO = :PANJENGDRNO      ");
            parameter.AppendSql("     , PANJENGDATE = TRUNC(SYSDATE)    ");
            parameter.AppendSql("     , TONGBODATE  = TRUNC(SYSDATE)    ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO            ");

            parameter.Add("PANJENGDRNO", fnDrno);
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public HIC_JEPSU GetGbDentalbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBDENTAL                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetGjJongbyPtnoJepDate2(string argPtno, string argJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PTNO, GJJONG, COUNT(*) CNT                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");
            parameter.AppendSql(" AND PTNO = :PTNO                                  ");
            parameter.AppendSql(" AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')     ");
            parameter.AppendSql(" AND DELDATE IS NULL                               ");
            parameter.AppendSql(" AND GBCHK3 = 'Y'                                  ");
            parameter.AppendSql(" GROUP BY PTNO, GJJONG                             ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("JEPDATE", argJepDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }



        public long GetWrtNobyJepDateCardSeqNo(string strBDATE, long nCardSeq)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND CARDSEQNO = :CARDSEQNO                      ");

            parameter.Add("JEPDATE", strBDATE);
            parameter.Add("CARDSEQNO", nCardSeq);

            return ExecuteScalar<long>(parameter);
        }

        public HIC_JEPSU GetSexAgeGjJongbyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME,SEX,AGE,GJJONG, PANO                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public string GetResultReceivePositionbyWrtNo(long fnWRTNO, string argJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CASE WHEN GBCHK3 = 'Y' THEN '방문수령'          ");
            parameter.AppendSql("            WHEN WEBPRINTREQ IS NOT NULL THEN '알림톡' ");
            parameter.AppendSql("            WHEN GBJUSO = 'Y' THEN '집'                ");
            parameter.AppendSql("            WHEN GBJUSO = 'N' THEN '회사'              ");
            parameter.AppendSql("            WHEN WEBPRINTREQ IS NULL THEN '우편'       ");
            parameter.AppendSql("        END AS RECEIVEPOSITION                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                           ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                  ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'yyyy-MM-dd')       ");
            parameter.AppendSql("   AND DELDATE IS NULL                                 ");
            parameter.AppendSql("   AND GBSTS NOT IN ('0','D')                          ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("JEPDATE", argJepDate);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_JEPSU Read_Jepsu_Wait_List(long argWrtNo, string argJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'yyyy-MM-dd')   ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            parameter.AppendSql("   AND GBSTS NOT IN ('0','D')                      ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("JEPDATE", argJepDate);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public int UpdateExamRemarkbyWrtNo(string strExamRemark, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET                   ");
            parameter.AppendSql("       EXAMREMARK = :EXAMREMARK                    ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO                         ");

            parameter.Add("EXAMREMARK", strExamRemark);
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatePtNoRecvFormbyRowId(string strPtNo, string strRecvForm, long fnJepsuNo, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET                   ");
            parameter.AppendSql("       PTNO     = :PTNO                            ");
            parameter.AppendSql("     , RECVFORM = :RECVFORM                        ");
            parameter.AppendSql("     , GBDBSEND = 'Y'                              ");
            parameter.AppendSql("     , GBMUN4   = 'Y'                              ");
            parameter.AppendSql("     , WRTNO4   = :WRTNO4                          ");
            parameter.AppendSql(" WHERE ROWID    = :RID                             ");

            parameter.Add("PTNO", strPtNo);
            parameter.Add("RECVFORM", strRecvForm);
            parameter.Add("WRTNO4", fnJepsuNo);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateIEMunNobyWrtNo(long fnJepsuNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET                   ");
            parameter.AppendSql("       IEMUNNO = :IEMUNNO                          ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                            ");

            parameter.Add("WRTNO", fnJepsuNo);

            return ExecuteNonQuery(parameter);
        }

        public HIC_JEPSU GetJepDateSexbyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JepDate,'YYYY-MM-DD') JEPDATE, SEX  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public void UpDateJonggumYNByPtnoJepDate(string argPtno, string argJepDate, string argFlag)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU                   ");
            parameter.AppendSql("   SET JONGGUMYN = :JONGGUMYN                  ");
            parameter.AppendSql(" WHERE PTNO =:PTNO                             ");
            parameter.AppendSql("   AND JEPDATE =TO_DATE(:JEPDATE,'YYYY-MM-DD') ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND GBSTS IN ('0','1')                      ");

            parameter.Add("JONGGUMYN", argFlag);
            parameter.Add("PTNO", argPtno);
            parameter.Add("JEPDATE", argJepDate);

            ExecuteNonQuery(parameter);
        }
        public void UpDateGWRTNOByPtnoJepDate(string argPtno, string argJepDate, long argGWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU                   ");
            parameter.AppendSql("   SET GWRTNO = :GWRTNO                        ");
            parameter.AppendSql(" WHERE PTNO =:PTNO                             ");
            parameter.AppendSql("   AND JEPDATE =TO_DATE(:JEPDATE,'YYYY-MM-DD') ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND GBSTS IN ('0','1')                      ");

            parameter.Add("GWRTNO", argGWRTNO);
            parameter.Add("PTNO", argPtno);
            parameter.Add("JEPDATE", argJepDate);

            ExecuteNonQuery(parameter);
        }
        public void UpDateJusoByWrtnoIN(HIC_PATIENT nHP, List<long> lstHcWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU   ");
            parameter.AppendSql("   SET SNAME    = :SNAME       ");
            parameter.AppendSql("     , SEX      = :SEX         ");
            parameter.AppendSql("     , MAILCODE = :MAILCODE    ");
            parameter.AppendSql("     , JUSO1    = :JUSO1       ");
            parameter.AppendSql("     , JUSO2    = :JUSO2       ");
            parameter.AppendSql(" WHERE WRTNO IN (:WRTNO)       ");

            parameter.Add("SNAME", nHP.SNAME);
            parameter.Add("SEX", nHP.SEX);
            parameter.Add("MAILCODE", nHP.MAILCODE);
            parameter.Add("JUSO1", nHP.JUSO1);
            parameter.Add("JUSO2", nHP.JUSO2);
            parameter.AddInStatement("WRTNO", lstHcWrtno);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetItembyWrtNo(string strFrDate, string strToDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT * from HIC_JEPSU                                                ");
            parameter.AppendSql(" WHERE WRTNO IN (                                                      ");
            parameter.AppendSql("                 SELECT WRTNO FROM HIC_RESULT                          ");
            parameter.AppendSql("                  WHERE RESULT IS NULL                                 ");
            parameter.AppendSql("                    AND EXCODE IN ('A999','ZD99','A135','A136') )      ");
            parameter.AppendSql("   AND JEPDATE >= TO_DATE(:FRDATE,'yyyy-mm-dd')                        ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE,'yyyy-mm-dd')                        ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                 ");
            parameter.AppendSql("   AND GBSTS NOT IN ('D')                                              ");
            parameter.AppendSql(" ORDER BY SNAME,GJJONG,JEPDATE                                         ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public string GetJepDAtebyPaNoJepDateGjYear(long fnPano, string fstrJepDate, string strGjYear)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JepDate,'YYYY-MM-DD') JepDate       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");
            parameter.AppendSql("   AND JEPDATE > TO_DATE(:JEPDATE  ,'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND GjChasu = '2'                               ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                            ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("PANO", fnPano);
            parameter.Add("JEPDATE", fstrJepDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_JEPSU> GetDetailbyPaNo(long nPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE, SNAME    ");
            parameter.AppendSql("     , PANO, AGE, SEX, GJJONG, GBSTS                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                           ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");

            parameter.Add("PANO", nPano);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetWrtNoJepDatebyPanoJepDate(long fnPano, string fstrJepDate, string fstrGjYear, string strJob)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a                             ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                                      ");
            parameter.AppendSql("   AND a.JEPDATE > TO_DATE(:JEPDATE, 'YYYY-MM-DD')         ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                   ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                  ");
            if (strJob == "0")  //특수
            {
                parameter.AppendSql("   AND a.Gjjong IN ('16','17','19','28','44','45')     ");
            }
            else if (strJob == "1") //배치전
            {
                parameter.AppendSql("   AND a.Gjjong = '29'                                 ");
            }
            else if (strJob == "2") //수시
            {
                parameter.AppendSql("   AND a.Gjjong IN ('29')                              ");
            }
            else                               //임시
            {
                parameter.AppendSql("   AND a.Gjjong  IN ('29')                             ");
            }
            parameter.AppendSql(" ORDER BY a.WRTNO                                          ");

            parameter.Add("PANO", fnPano);
            parameter.Add("JEPDATE", fstrJepDate);
            parameter.Add("GJYEAR", fstrGjYear);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public string GetWebPrintReqByJepDatePtno(string argJepDate, string argPtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(WEBPRINTREQ,'YYYY-MM-DD HH24:MI') WEBPRINTREQ       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                       ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                                ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE  ,'YYYY-MM-DD')                  ");
            parameter.AppendSql("   AND WEBPRINTREQ IS NOT NULL                                     ");
            parameter.AppendSql("   AND DELDATE IS NULL                                             ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("JEPDATE", argJepDate);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_JEPSU GetWrtNobyPtNoJepDate(string fstrPtno, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            parameter.AppendSql("   AND GJJONG IN('11', '21', '22', '23', '24')     ");
            parameter.AppendSql("   AND JONGGUMYN = '1'                             ");
            parameter.AppendSql(" ORDER BY GJJONG                                   ");

            parameter.Add("PTNO", fstrPtno);
            parameter.Add("JEPDATE", strJepDate);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public int GetCountbyPaNoJepDate(long argPano, string argDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X')                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("PANO", argPano);
            parameter.Add("JEPDATE", argDate);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_JEPSU> Read_Wrtno_SDate(long fnPano, string argJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO, JEPDATE                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");
            parameter.AppendSql("   AND JEPDATE < TO_DATE(:SDATE, 'yyyy-MM-dd')     ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            parameter.AppendSql("   AND GJJONG NOT IN('31','35','69','52','55','54')");
            parameter.AppendSql("   AND GjChasu <> '2'                              ");
            parameter.AppendSql(" ORDER BY JEPDATE DESC                             ");

            parameter.Add("PANO", fnPano);
            parameter.Add("SDATE", argJepDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetExamRemarkbyPanoJepDate(long fnPano, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT EXAMREMARK                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                           ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')       ");

            parameter.Add("PANO", fnPano);
            parameter.Add("JEPDATE", strJepDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetGbAmbyPaNoJepDate(long fnPano, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBAM                                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                           ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND GJJONG IN('31', '35')                           ");

            parameter.Add("PANO", fnPano);
            parameter.Add("JEPDATE", strJepDate);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetSecondJongItemByPanoGjYear(long nPano, string strYear)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,GJJONG,GJCHASU        ");
            parameter.AppendSql("      ,LTDCODE,UCODES,SEXAMS,ROWID AS RID                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                ");
            parameter.AppendSql("   AND GJYEAR >= :GJYEAR                                           ");
            parameter.AppendSql("   AND DELDATE IS NULL                                             ");
            parameter.AppendSql("   AND GJJONG IN ('16','28')                                       ");
            parameter.AppendSql(" ORDER BY JEPDATE DESC                                             ");

            parameter.Add("PANO", nPano);
            parameter.Add("GJYEAR", strYear);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public int UpdatePanjengbyWrtNo(long nPanDrno, string strPanDate, long fnWrtNo, string strOK)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET                               ");
            if (strOK == "OK")
            {
                parameter.AppendSql("       PANJENGDRNO   = :PANJENGDRNO                        ");
                parameter.AppendSql("     , PANJENGDATE = TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')   ");
            }
            else
            {
                parameter.AppendSql("       PANJENGDRNO   = :PANJENGDRNO                        ");
            }
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO                ");

            if (strOK == "OK")
            {
                parameter.Add("PANJENGDRNO", nPanDrno);
                parameter.Add("PANJENGDATE", strPanDate);
                
            }
            else
            {
                parameter.Add("PANJENGDRNO", nPanDrno);
            }

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public void UpDateWebPrintReq(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU       ");
            parameter.AppendSql("   SET WEBPRINTREQ = SYSDATE       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", nWRTNO);

            ExecuteNonQuery(parameter);
        }

        public void UpDateGBHEAENDO(string argPtno, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU           ");
            parameter.AppendSql("   SET GBHEAENDO = :GBHEAENDO          ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                    ");
            parameter.AppendSql(" AND JEPDATE =TRUNC(SYSDATE)           ");
            parameter.AppendSql(" AND GJJONG = '31'                     ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("GBHEAENDO", argGubun);

            ExecuteNonQuery(parameter);
        }

        public void UpdateGbAmByWRTNO(string argGbAm, long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU   ");
            parameter.AppendSql("   SET GBAM = :GBAM            ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("GBAM", argGbAm);
            parameter.Add("WRTNO", nWRTNO);

            ExecuteNonQuery(parameter);
        }

        public void UpdateItemsByWRTNO(long nWRTNO, HIC_PATIENT iHP)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU   ");
            parameter.AppendSql("   SET SEX     = :SEX          ");
            parameter.AppendSql("      ,SNAME   = :SNAME        ");
            parameter.AppendSql("      ,MAILCODE= :MAILCODE     ");
            parameter.AppendSql("      ,JUSO1   = :JUSO1        ");
            parameter.AppendSql("      ,JUSO2   = :JUSO2        ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("SEX", iHP.SEX);
            parameter.Add("SNAME", iHP.SNAME);
            parameter.Add("MAILCODE", iHP.MAILCODE);
            parameter.Add("JUSO1", iHP.JUSO1);
            parameter.Add("JUSO2", iHP.JUSO2);
            parameter.Add("WRTNO", nWRTNO);

            ExecuteNonQuery(parameter);
        }

        public void InsertBySelectWork(long nWRTNO, long nPANO, string argJONG, string argJYEAR, string argJEPDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_JEPSU (                                                    ");
            parameter.AppendSql("    WRTNO,GJYEAR,JEPDATE,PANO,SNAME,SEX,AGE,GJJONG,GJCHASU,GJBANGI                     ");
            parameter.AppendSql("   ,GBCHUL,GBSTS,LTDCODE,MAILCODE,JUSO1,JUSO2,PTNO,BURATE,JISA,KIHO,GKIHO,JIKGBN       ");
            parameter.AppendSql("   ,UCODES,SEXAMS,JIKJONG,SABUN,IPSADATE,BUSENAME,OLDJIKJONG,BUSEIPSA,OLDSDATE         ");
            parameter.AppendSql("   ,OLDEDATE,SECOND_DATE,GBSUCHEP,BALYEAR,BALSEQ,JOBSABUN,ENTTIME,GBEXAM               ");
            parameter.AppendSql("   ,GBDENTAL,GBINWON,BOGUNSO                                                           ");
            parameter.AppendSql("   ,TEL,LIVER2,YOUNGUPSO,MILEAGEAM,MURYOAM,GUMDAESANG,JEPSUGBN,MILEAGEAMGBN            ");
            parameter.AppendSql("   ,MURYOGBN,NEGODATE,MAMT,FAMT,GBSABUN,REMARK,EMAIL,XRAYNO,GBN,CLASS,BAN,BUN          ");
            parameter.AppendSql("   ,GBADDPAN,AUTOJEP,JONGGUMYN )                                                       ");
            parameter.AppendSql("SELECT :WRTNO,GJYEAR,TRUNC(SYSDATE),PANO,SNAME,SEX,AGE,GJJONG,GJCHASU,GJBANGI          ");
            parameter.AppendSql("   ,'N','1',LTDCODE,MAILCODE,JUSO1,JUSO2,PTNO,BURATE,JISA,KIHO,GKIHO,JIKGBN            ");
            parameter.AppendSql("   ,UCODES,SEXAMS,JIKJONG,SABUN,IPSADATE,BUSENAME,OLDJIKJONG,BUSEIPSA,OLDSDATE         ");
            parameter.AppendSql("   ,OLDEDATE,SECOND_DATE,GBSUCHEP,BALYEAR,BALSEQ,'222',SYSDATE,GBEXAM                  ");
            parameter.AppendSql("   ,GBDENTAL,GBINWON,BOGUNSO                                                           ");
            parameter.AppendSql("   ,TEL,LIVER2,YOUNGUPSO,MILEAGEAM,MURYOAM,GUMDAESANG,JEPSUGBN,MILEAGEAMGBN            ");
            parameter.AppendSql("   ,MURYOGBN,NEGODATE,MAMT,FAMT,GBSABUN,REMARK,EMAIL,XRAYNO,GBN,CLASS,BAN,BUN          ");
            parameter.AppendSql("   ,GBADDPAN,'Y','1'                                                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK                                                      ");
            parameter.AppendSql(" WHERE PANO    = :PANO                                                                 ");
            parameter.AppendSql("   AND GJYEAR  = :GJYEAR                                                               ");
            parameter.AppendSql("   AND GJJONG  = :GJJONG                                                               ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                       ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("PANO", nPANO);
            parameter.Add("GJYEAR", argJYEAR);
            parameter.Add("GJJONG", argJONG);
            parameter.Add("JEPDATE", argJEPDATE);

            ExecuteNonQuery(parameter); ;
        }

        public HIC_JEPSU GetSexJepDatebyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SEX, TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public void SelectWorkJepsuInsert(HIC_JEPSU nHJ, string argRID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_JEPSU (                                            ");
            parameter.AppendSql("       WRTNO,GJYEAR,JEPDATE,PANO,SNAME,SEX,AGE,GJJONG,TEL                      ");
            parameter.AppendSql("      ,GJCHASU,GJBANGI,LTDCODE,BUSENAME,MAILCODE,JUSO1,JUSO2,UCODES            ");
            parameter.AppendSql("      ,SEXAMS,PTNO,BURATE,BALYEAR,BALSEQ,JISA,KIHO,GKIHO,JIKGBN,GBEXAM         ");
            parameter.AppendSql("      ,GBCHUL,GBDENTAL,GBINWON,JOBSABUN,ENTTIME,BOGUNSO,LIVER2,YOUNGUPSO       ");
            parameter.AppendSql("      ,IPSADATE,GBN,EMAIL,REMARK,GBSTS,AUTOJEP,JONGGUMYN,WEBPRINTREQ,GWRTNO )  ");
            parameter.AppendSql(" SELECT :WRTNO,GJYEAR,TRUNC(SYSDATE),PANO,SNAME,SEX,AGE,GJJONG,TEL             ");
            parameter.AppendSql("      ,GJCHASU,:GJBANGI,LTDCODE,BUSENAME,MAILCODE,JUSO1,JUSO2,UCODES           ");
            parameter.AppendSql("      ,SEXAMS,PTNO,BURATE,BALYEAR,BALSEQ,JISA,KIHO,GKIHO,JIKGBN,:GBEXAM        ");
            parameter.AppendSql("      ,:GBCHUL,GBDENTAL,GBINWON,:JOBSABUN,SYSDATE,BOGUNSO,LIVER2,YOUNGUPSO     ");
            parameter.AppendSql("      ,IPSADATE,GBN,EMAIL,REMARK,:GBSTS,:AUTOJEP,:JONGGUMYN,:WEBPRINTREQ,:GWRTNO");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK                                              ");
            parameter.AppendSql(" WHERE ROWID=:RID                                                              ");

            parameter.Add("WRTNO", nHJ.WRTNO);
            parameter.Add("GJBANGI", nHJ.GJBANGI);
            parameter.Add("GBEXAM", nHJ.GBEXAM);
            parameter.Add("GBCHUL", nHJ.GBCHUL);
            parameter.Add("JOBSABUN", nHJ.JOBSABUN);
            parameter.Add("GBSTS", nHJ.GBSTS);
            parameter.Add("AUTOJEP", nHJ.AUTOJEP);
            parameter.Add("JONGGUMYN", nHJ.JONGGUMYN);
            parameter.Add("WEBPRINTREQ", nHJ.WEBPRINTREQ);
            parameter.Add("GWRTNO", nHJ.GWRTNO);
            parameter.Add("RID", argRID);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDate(string strFrDate, string strToDate, long nLtdCode, string strJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PTNO, GJJONG, TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE     ");
            parameter.AppendSql("     , TO_CHAR(JEPDATE, 'MM/DD') JDATE, SNAME, PANO, WRTNO     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE JEPDATE >=TO_DATE(:FRDATE, 'YYYY-MM-DD')                ");
            parameter.AppendSql("   AND JEPDATE <=TO_DATE(:TODATE, 'YYYY-MM-DD')                ");
            parameter.AppendSql("   AND DELDATE IS NULL                                         ");
            parameter.AppendSql("   AND GBSTS NOT IN ('D')                                      ");
            if (strJong != "00" && strJong != "")
            {
                parameter.AppendSql("   AND GJJONG = :GJJONG                                    ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                  ");
            }
            parameter.AppendSql(" ORDER BY JEPDATE, GJJONG                                      ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (strJong != "00" && strJong != "")
            {
                parameter.Add("GJJONG", strJong);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDateGjYearLtdCodeGjJong(string strJepDate, string strGjYear, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, SNAME, BUSENAME, GJJONG, UCODES, PANO            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:JEPDATE, 'YYYY-MM-DD')              ");
            parameter.AppendSql("   AND GJYEAR  = :GJYEAR                                       ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                                      ");
            parameter.AppendSql("   AND GJJONG IN ('11','12','14','23','25','26','41','42')     ");
            parameter.AppendSql("   AND UCodes IS NOT NULL                                      ");
            parameter.AppendSql("   AND DELDATE IS NULL                                         ");

            parameter.Add("JEPDATE", strJepDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", nLtdCode);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int InsertSangdamWaitbyPaNo(HIC_SANGDAM_WAIT wait)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SANGDAM_WAIT               ");
            parameter.AppendSql("       (WRTNO, SNAME, SEX, AGE, GJJONG                 ");
            parameter.AppendSql("     , GUBUN, ENTTIME, WAITNO, PANO, NEXTROOM)         ");
            parameter.AppendSql("VALUES                                                 ");
            parameter.AppendSql("       (:WRTNO, :SNAME, :SEX, :AGE, :GJJONG            ");
            parameter.AppendSql("     , :GUBUN, SYSDATE, :WAITNO, :PANO, :NEXTROOM)     ");

            parameter.Add("WRTNO", wait.WRTNO);
            parameter.Add("SNAME", wait.SNAME);
            parameter.Add("SEX", wait.SEX, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE", wait.AGE);
            parameter.Add("GJJONG", wait.GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GUBUN", wait.GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WAITNO", wait.WAITNO);
            parameter.Add("PANO", wait.PANO);
            parameter.Add("NEXTROOM", wait.NEXTROOM);

            return ExecuteNonQuery(parameter);
        }

        public HIC_JEPSU GetItembyWrtNoINGjJong(long argWrtNo, List<string> strGjJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,a.SName,a.Sex,a.Age,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.GjJong,Pano   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a                                                             ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                    ");
            parameter.AppendSql("   AND a.GJJONG IN (:GJJONG)                                                               ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                   ");
            parameter.AppendSql(" ORDER BY a.jepdate,a.wrtno,a.SName                                                        ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.AddInStatement("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetItemByPtnoJepDateExCode(string argPtno, string argDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.PANO, a.GJJONG                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a                     ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_RESULT b                    ");
            parameter.AppendSql(" WHERE a.JEPDATE = TO_DATE(:JEPDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND a.PTNO = :PTNO                              ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                           ");
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                          ");
            parameter.AppendSql("   AND b.EXCODE IN ('A141','A142')                 ");

            parameter.Add("JEPDATE", argDate);
            parameter.Add("PTNO", argPtno);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetItemByWRTNO(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,GJYEAR,TO_CHAR(JEPDATE, 'YYYY-MM-DD') JEPDATE,PANO,SNAME,SEX,AGE,GJJONG,GJCHASU,GJBANGI,GBCHUL  ");
            parameter.AppendSql("      ,GBSTS, GBINWON, DELDATE, LTDCODE, MAILCODE, JUSO1, JUSO2, TEL, PTNO, BURATE, JISA, KIHO, GKIHO        ");
            parameter.AppendSql("      ,JIKGBN, UCODES, SEXAMS, JIKJONG, SABUN, IPSADATE, BUSENAME, OLDJIKJONG, BUSEIPSA, OLDSDATE            ");
            parameter.AppendSql("      ,OLDEDATE, GBSUCHEP, BALYEAR, BALSEQ, GBEXAM, BOGUNSO, JEPSUGBN, YOUNGUPSO, JEPNO, GBSPCMUNJIN         ");
            parameter.AppendSql("      ,GBPRINT, GBSABUN, GBJINCHAL, GBMUNJIN1, GBMUNJIN2, GBMUNJIN3, GBDENTAL, SECOND_FLAG                   ");
            parameter.AppendSql("      ,SECOND_DATE, SECOND_TONGBO, SECOND_EXAMS, SECOND_SAYU, CHUNGGUYN, DENTCHUNGGUYN                       ");
            parameter.AppendSql("      ,CANCERCHUNGGUYN, LIVER2, MAMT, FAMT, MILEAGEAM, MURYOAM, GUMDAESANG, JONGGUMYN, SEND                  ");
            parameter.AppendSql("      ,MILEAGEAMGBN, MURYOGBN, NEGODATE, JOBSABUN, ENTTIME, SECOND_MISAYU, MIRNO1, MIRNO2                    ");
            parameter.AppendSql("      ,MIRNO3, EMAIL, REMARK, OHMSNO, MISUNO1, FIRSTPANDRNO, ERFLAG, MIRNO4, MIRSAYU, ERTONGBO               ");
            parameter.AppendSql("      ,MISUNO2, GUBDAESANG, MIRNO5, MISUNO3, XRAYNO, CARDSEQNO, GBADDPAN, GBAM, BALDATE, TONGBODATE          ");
            parameter.AppendSql("      ,GBSUJIN_SET, GBCHUL2, GBJINCHAL2, SANGDAMDRNO, SANGDAMDATE, SENDMSG, GBN, CLASS, BAN                  ");
            parameter.AppendSql("      ,BUN, HEMSNO, HEMSMIRSAYU, AUTOJEP, MISUNO4, P_WRTNO, PANJENGDATE, PANJENGDRNO, GBAUDIO2EXAM           ");
            parameter.AppendSql("      ,GBMUNJINPRINT, WAITREMARK, PDFPATH, WEBSEND, WEBSENDDATE, GBNAKSANG, GBDENTONLY                       ");
            parameter.AppendSql("      ,GBAUTOPAN, IEMUNNO, WEBPRINTREQ, GBSUJIN_CHK, WEBPRINTSEND, LTDCODE2, DENTAMT, GBHUADD                ");
            parameter.AppendSql("      ,EXAMREMARK, PRTSABUN, GBHEAENDO, GBCHK1, GBCHK2, GBCHK3, GBLIFE, GBJUSO, GWRTNO, JEPBUN               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                                                 ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                                        ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public string GET_HEA_JepsuDate(long fnHeaWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(SDate,'YYYY-MM-DD') JEPDATE                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                               ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                      ");

            parameter.Add("WRTNO", fnHeaWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public List<long> GetHeaWrtnoListByPtnoJepDate(string argPtno, string argJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO FROM KOSMOS_PMPA.HEA_JEPSU         ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                        ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')                    ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("JEPDATE", argJepDate);

            return ExecuteReader<long>(parameter);
        }

        public long GetGbCancerWrtnoByJepDatePano(long pANO, string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            parameter.AppendSql("   AND JEPDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND GJJONG IN ('31', '35')                      ");
            parameter.AppendSql(" ORDER BY WRTNO                                    ");

            parameter.Add("PANO", pANO);
            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);

            return ExecuteScalar<long>(parameter);
        }

        public HIC_JEPSU GetItembyPtnoGjJong(string strPANO, List<string> strjong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') YDate, SECOND_Exams, GJJONG, TONGBODATE   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                                            ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                         ");
            parameter.AppendSql("   AND GJJONG IN (:GJJONG)                                                     ");
            parameter.AppendSql(" ORDER BY JEPDATE DESC                                                                                          ");

            parameter.Add("PTNO", strPANO);
            parameter.AddInStatement("GJJONG", strjong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public void UpDateGbNaksangByWrtno(long wRTNO, string strGbNaksang)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HIC_JEPSU          ");
            parameter.AppendSql("    SET GBNAKSANG =:GBNAKSANG          ");
            parameter.AppendSql("  WHERE WRTNO =:WRTNO                  ");

            parameter.Add("GBNAKSANG", strGbNaksang);
            parameter.Add("WRTNO", wRTNO);

            ExecuteNonQuery(parameter);
        }

        public string GetGbStsByWRTNO(long wRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBSTS                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public void UpDateGbMunjin2ByWrtno(long argWrtno, string argM1, string argM2, string argM3)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HIC_JEPSU          ");
            parameter.AppendSql("    SET GBMUNJIN1 =:GBMUNJIN1          ");
            parameter.AppendSql("       ,GBMUNJIN2 =:GBMUNJIN2          ");
            parameter.AppendSql("       ,GBMUNJIN3 =:GBMUNJIN3          ");
            parameter.AppendSql("  WHERE WRTNO =:WRTNO                  ");

            parameter.Add("GBMUNJIN1", argM1.To<string>(""));
            parameter.Add("GBMUNJIN2", argM2.To<string>(""));
            parameter.Add("GBMUNJIN3", argM3.To<string>(""));
            parameter.Add("WRTNO", argWrtno);

            ExecuteNonQuery(parameter);
        }

        public void InsertAll(HIC_JEPSU nHJ)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_JEPSU (                                                         ");
            parameter.AppendSql("       WRTNO,GJYEAR,JEPDATE,PANO,SNAME,SEX,AGE,GJJONG,TEL                                   ");
            parameter.AppendSql("      ,GJCHASU,GJBANGI,LTDCODE,BUSENAME,MAILCODE,JUSO1,JUSO2,UCODES                         ");
            parameter.AppendSql("      ,SEXAMS,JONGGUMYN,PTNO,BURATE,BALYEAR,BALSEQ,JISA,KIHO,GKIHO,JIKGBN,GBEXAM            ");
            parameter.AppendSql("      ,GBCHUL,GBDENTAL,GBINWON,JOBSABUN,ENTTIME,BOGUNSO,IPSADATE                            ");
            parameter.AppendSql("      ,GBN,CLASS,BAN,BUN,SABUN,MURYOAM,GUBDAESANG,EMAIL,REMARK                              ");
            parameter.AppendSql("      ,CARDSEQNO,GBADDPAN,GBAM, GBSTS,SENDMSG,GBDENTONLY,DENTAMT,GBHUADD,GBHEAENDO          ");
            parameter.AppendSql("      ,GBCHK1,GBCHK2,GBCHK3,SANGDAMDRNO, SANGDAMDATE, GBLIFE, GBJUSO, WEBPRINTREQ           ");
            parameter.AppendSql("      ,GWRTNO,XRAYNO                                                                         ");
            parameter.AppendSql(" ) VALUES (                                                                                 ");
            parameter.AppendSql("      :WRTNO,:GJYEAR,TO_DATE(:JEPDATE,'YYYY-MM-DD'),:PANO,:SNAME,:SEX,:AGE,:GJJONG,:TEL                           ");
            parameter.AppendSql("     ,:GJCHASU,:GJBANGI,:LTDCODE,:BUSENAME,:MAILCODE,:JUSO1,:JUSO2,:UCODES                  ");
            parameter.AppendSql("     ,:SEXAMS,:JONGGUMYN,:PTNO,:BURATE,:BALYEAR,:BALSEQ,:JISA,:KIHO,:GKIHO,:JIKGBN,:GBEXAM  ");
            parameter.AppendSql("     ,:GBCHUL,:GBDENTAL,:GBINWON,:JOBSABUN,SYSDATE,:BOGUNSO,TO_DATE(:IPSADATE,'YYYY-MM-DD')                        ");
            parameter.AppendSql("     ,:GBN,:CLASS,:BAN,:BUN,:SABUN,:MURYOAM,:GUBDAESANG,:EMAIL,:REMARK                      ");
            parameter.AppendSql("     ,:CARDSEQNO,:GBADDPAN,:GBAM,:GBSTS,:SENDMSG,:GBDENTONLY,:DENTAMT,:GBHUADD,:GBHEAENDO   ");
            parameter.AppendSql("     ,:GBCHK1,:GBCHK2,:GBCHK3,:SANGDAMDRNO,:SANGDAMDATE,:GBLIFE,:GBJUSO,:WEBPRINTREQ        ");
            parameter.AppendSql("     ,:GWRTNO,:XRAYNO                                                                               ");
            parameter.AppendSql(" )                                                                                          ");

            parameter.Add("WRTNO", nHJ.WRTNO);
            parameter.Add("GJYEAR", nHJ.GJYEAR);
            parameter.Add("JEPDATE", nHJ.JEPDATE);
            parameter.Add("PANO", nHJ.PANO);
            parameter.Add("SNAME", nHJ.SNAME);
            parameter.Add("SEX", nHJ.SEX);
            parameter.Add("AGE", nHJ.AGE);
            parameter.Add("GJJONG", nHJ.GJJONG);
            parameter.Add("TEL", nHJ.TEL);
            parameter.Add("GJCHASU", nHJ.GJCHASU);
            parameter.Add("GJBANGI", nHJ.GJBANGI);
            parameter.Add("LTDCODE", nHJ.LTDCODE);
            parameter.Add("BUSENAME", nHJ.BUSENAME);
            parameter.Add("MAILCODE", nHJ.MAILCODE);
            parameter.Add("JUSO1", nHJ.JUSO1);
            parameter.Add("JUSO2", nHJ.JUSO2);
            parameter.Add("UCODES", nHJ.UCODES);
            parameter.Add("SEXAMS", nHJ.SEXAMS);
            parameter.Add("JONGGUMYN", nHJ.JONGGUMYN);
            parameter.Add("PTNO", nHJ.PTNO);
            parameter.Add("BURATE", nHJ.BURATE);
            parameter.Add("BALYEAR", nHJ.BALYEAR);
            parameter.Add("BALSEQ", nHJ.BALSEQ);
            parameter.Add("JISA", nHJ.JISA);
            parameter.Add("KIHO", nHJ.KIHO);
            parameter.Add("GKIHO", nHJ.GKIHO);
            parameter.Add("JIKGBN", nHJ.JIKGBN);
            parameter.Add("GBEXAM", nHJ.GBEXAM);
            parameter.Add("GBCHUL", nHJ.GBCHUL);
            parameter.Add("GBDENTAL", nHJ.GBDENTAL);
            parameter.Add("GBINWON", nHJ.GBINWON);
            parameter.Add("JOBSABUN", nHJ.JOBSABUN);
            parameter.Add("BOGUNSO", nHJ.BOGUNSO);
            parameter.Add("IPSADATE", nHJ.IPSADATE);
            parameter.Add("GBN", nHJ.GBN);
            parameter.Add("CLASS", nHJ.CLASS);
            parameter.Add("BAN", nHJ.BAN);
            parameter.Add("BUN", nHJ.BUN);
            parameter.Add("SABUN", nHJ.SABUN);
            parameter.Add("MURYOAM", nHJ.MURYOAM);
            parameter.Add("GUBDAESANG", nHJ.GUBDAESANG);
            parameter.Add("EMAIL", nHJ.EMAIL);
            parameter.Add("REMARK", nHJ.REMARK);
            parameter.Add("CARDSEQNO", nHJ.CARDSEQNO);
            parameter.Add("GBADDPAN", nHJ.GBADDPAN);
            parameter.Add("GBAM", nHJ.GBAM);
            parameter.Add("GBSTS", nHJ.GBSTS);
            parameter.Add("SENDMSG", nHJ.SENDMSG);
            parameter.Add("GBDENTONLY", nHJ.GBDENTONLY);
            parameter.Add("DENTAMT", nHJ.DENTAMT);
            parameter.Add("GBHUADD", nHJ.GBHUADD);
            parameter.Add("GBHEAENDO", nHJ.GBHEAENDO);
            parameter.Add("GBCHK1", nHJ.GBCHK1);
            parameter.Add("GBCHK2", nHJ.GBCHK2);
            parameter.Add("GBCHK3", nHJ.GBCHK3);
            parameter.Add("SANGDAMDRNO", nHJ.SANGDAMDRNO);
            parameter.Add("SANGDAMDATE", nHJ.SANGDAMDATE);
            parameter.Add("GBLIFE", nHJ.GBLIFE);
            parameter.Add("GBJUSO", nHJ.GBJUSO);
            parameter.Add("WEBPRINTREQ", nHJ.WEBPRINTREQ);
            parameter.Add("GWRTNO", nHJ.GWRTNO);
            parameter.Add("XRAYNO", nHJ.XRAYNO);


            ExecuteNonQuery(parameter);
        }

        public void UpDateAll(HIC_JEPSU nHJ)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET     ");
            parameter.AppendSql("       GJYEAR        =:GJYEAR        ");
            parameter.AppendSql("      ,JEPDATE       =TO_DATE(:JEPDATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("      ,SNAME         =:SNAME         ");
            parameter.AppendSql("      ,SEX           =:SEX           ");
            parameter.AppendSql("      ,GJJONG        =:GJJONG        ");
            parameter.AppendSql("      ,TEL           =:TEL           ");
            parameter.AppendSql("      ,GJCHASU       =:GJCHASU       ");
            parameter.AppendSql("      ,LTDCODE       =:LTDCODE       ");
            parameter.AppendSql("      ,BUSENAME      =:BUSENAME      ");
            parameter.AppendSql("      ,MAILCODE      =:MAILCODE      ");
            parameter.AppendSql("      ,JUSO1         =:JUSO1         ");
            parameter.AppendSql("      ,JUSO2         =:JUSO2         ");
            parameter.AppendSql("      ,UCODES        =:UCODES        ");
            parameter.AppendSql("      ,SEXAMS        =:SEXAMS        ");
            parameter.AppendSql("      ,PTNO          =:PTNO          ");
            parameter.AppendSql("      ,BURATE        =:BURATE        ");
            parameter.AppendSql("      ,JISA          =:JISA          ");
            parameter.AppendSql("      ,KIHO          =:KIHO          ");
            parameter.AppendSql("      ,GKIHO         =:GKIHO         ");
            parameter.AppendSql("      ,JIKGBN        =:JIKGBN        ");
            parameter.AppendSql("      ,GBEXAM        =:GBEXAM        ");
            parameter.AppendSql("      ,GBCHUL        =:GBCHUL        ");
            parameter.AppendSql("      ,GBAM          =:GBAM          ");
            parameter.AppendSql("      ,JONGGUMYN     =:JONGGUMYN     ");
            parameter.AppendSql("      ,MURYOAM       =:MURYOAM       ");
            parameter.AppendSql("      ,GUBDAESANG    =:GUBDAESANG    ");
            parameter.AppendSql("      ,GBDENTAL      =:GBDENTAL      ");
            parameter.AppendSql("      ,GBDENTONLY    =:GBDENTONLY    ");
            parameter.AppendSql("      ,GBINWON       =:GBINWON       ");
            parameter.AppendSql("      ,JOBSABUN      =:JOBSABUN      ");
            parameter.AppendSql("      ,IPSADATE      = TO_DATE(:IPSADATE,'YYYY-MM-DD') ");
            parameter.AppendSql("      ,BOGUNSO       =:BOGUNSO       ");
            parameter.AppendSql("      ,YOUNGUPSO     =:YOUNGUPSO     ");
            parameter.AppendSql("      ,EMAIL         =:EMAIL         ");
            parameter.AppendSql("      ,GBADDPAN      =:GBADDPAN      ");
            parameter.AppendSql("      ,REMARK        =:REMARK        ");
            parameter.AppendSql("      ,GBN           =:GBN           ");
            parameter.AppendSql("      ,CLASS         =:CLASS         ");
            parameter.AppendSql("      ,BAN           =:BAN           ");
            parameter.AppendSql("      ,BUN           =:BUN           ");
            parameter.AppendSql("      ,GBHEAENDO     =:GBHEAENDO     ");
            //parameter.AppendSql("      ,DENTAMT       =:DENTAMT       ");
            parameter.AppendSql("      ,GBHUADD       =:GBHUADD       ");
            parameter.AppendSql("      ,CARDSEQNO     =:CARDSEQNO     ");
            parameter.AppendSql("      ,GBCHK1        =:GBCHK1        ");
            parameter.AppendSql("      ,GBCHK2        =:GBCHK2        ");
            parameter.AppendSql("      ,GBCHK3        =:GBCHK3        ");   //결과지 방문수령
            parameter.AppendSql("      ,GBJUSO        =:GBJUSO        ");
            parameter.AppendSql("      ,ENTTIME       =SYSDATE        ");
            parameter.AppendSql("      ,WEBPRINTREQ   =:WEBPRINTREQ   ");
            if ( nHJ.GBCHUL =="Y")
            {
                parameter.AppendSql("      ,XRAYNO   =:XRAYNO   ");
            }
            parameter.AppendSql(" WHERE WRTNO         =:WRTNO         ");

            parameter.Add("GJYEAR", nHJ.GJYEAR);
            parameter.Add("JEPDATE", nHJ.JEPDATE);
            parameter.Add("SNAME", nHJ.SNAME);
            parameter.Add("SEX", nHJ.SEX);
            parameter.Add("GJJONG", nHJ.GJJONG);
            parameter.Add("TEL", nHJ.TEL);
            parameter.Add("GJCHASU", nHJ.GJCHASU);
            parameter.Add("LTDCODE", nHJ.LTDCODE);
            parameter.Add("BUSENAME", nHJ.BUSENAME);
            parameter.Add("MAILCODE", nHJ.MAILCODE);
            parameter.Add("JUSO1", nHJ.JUSO1);
            parameter.Add("JUSO2", nHJ.JUSO2);
            parameter.Add("UCODES", nHJ.UCODES);
            parameter.Add("SEXAMS", nHJ.SEXAMS);
            parameter.Add("PTNO", nHJ.PTNO);
            parameter.Add("BURATE", nHJ.BURATE);
            parameter.Add("JISA", nHJ.JISA);
            parameter.Add("KIHO", nHJ.KIHO);
            parameter.Add("GKIHO", nHJ.GKIHO);
            parameter.Add("JIKGBN", nHJ.JIKGBN);
            parameter.Add("GBEXAM", nHJ.GBEXAM);
            parameter.Add("GBCHUL", nHJ.GBCHUL);
            parameter.Add("GBAM", nHJ.GBAM);
            parameter.Add("JONGGUMYN", nHJ.JONGGUMYN);
            parameter.Add("MURYOAM", nHJ.MURYOAM);
            parameter.Add("GUBDAESANG", nHJ.GUBDAESANG);
            parameter.Add("GBDENTAL", nHJ.GBDENTAL);
            parameter.Add("GBDENTONLY", nHJ.GBDENTONLY);
            parameter.Add("GBINWON", nHJ.GBINWON);
            parameter.Add("JOBSABUN", nHJ.JOBSABUN);
            parameter.Add("IPSADATE", nHJ.IPSADATE);
            parameter.Add("BOGUNSO", nHJ.BOGUNSO);
            parameter.Add("YOUNGUPSO", nHJ.YOUNGUPSO);
            parameter.Add("EMAIL", nHJ.EMAIL);
            parameter.Add("GBADDPAN", nHJ.GBADDPAN);
            parameter.Add("REMARK", nHJ.REMARK);
            parameter.Add("GBN", nHJ.GBN);
            parameter.Add("CLASS", nHJ.CLASS);
            parameter.Add("BAN", nHJ.BAN);
            parameter.Add("BUN", nHJ.BUN);
            parameter.Add("GBHEAENDO", nHJ.GBHEAENDO);
            //parameter.Add("DENTAMT", nHJ.DENTAMT);
            parameter.Add("GBHUADD", nHJ.GBHUADD);
            parameter.Add("CARDSEQNO", nHJ.CARDSEQNO);
            parameter.Add("GBCHK1", nHJ.GBCHK1);
            parameter.Add("GBCHK2", nHJ.GBCHK2);
            parameter.Add("GBCHK3", nHJ.GBCHK3);
            parameter.Add("GBJUSO", nHJ.GBJUSO);
            parameter.Add("WEBPRINTREQ", nHJ.WEBPRINTREQ);
            if (nHJ.GBCHUL == "Y")
            {
                parameter.Add("XRAYNO", nHJ.XRAYNO);
            }
            parameter.Add("WRTNO", nHJ.WRTNO);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetHistoryByPtnoYear(string argPtno, string argYear)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PTNO, GJJONG, TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE     ");
            parameter.AppendSql("     , LTDCODE, SNAME, PANO, WRTNO                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE PTNO =:PTNO                                             ");
            parameter.AppendSql("   AND GJYEAR =:GJYEAR                                         ");
            parameter.AppendSql("   AND DELDATE IS NULL                                         ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("GJYEAR", argYear);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public string GetTongboDateByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(TONGBODATE,'YYYY-MM-DD') TONGBODATE ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public void UpDateDelDateGbMunJinByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET   ");
            parameter.AppendSql("       DELDATE = TRUNC(SYSDATE)    ");
            parameter.AppendSql("     , JOBSABUN = :JOBSABUN        ");
            parameter.AppendSql("     , GBSTS = 'D'                 ");
            parameter.AppendSql("     , GBMUNJIN1 = ''              ");
            parameter.AppendSql("     , GBMUNJIN2 = ''              ");
            parameter.AppendSql("     , ENTTIME = SYSDATE           ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("JOBSABUN", clsType.User.IdNumber.To<long>());
            parameter.Add("WRTNO", nWRTNO);

            ExecuteNonQuery(parameter);
        }

        public int UpdateGbMunjin2GbJinChal2byWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET   ");
            parameter.AppendSql("       GBMUNJIN2  = 'Y'            ");
            parameter.AppendSql("     , GBJINCHAL2 = 'Y'            ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO         ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatePanjengDrNobyWrtNo(long fnWRTNO, long nPangjengDrno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            parameter.AppendSql("       PANJENGDATE = TRUNC(SYSDATE)    ");
            parameter.AppendSql("     , PANJENGDRNO = :PANJENGDRNO      ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO            ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("PANJENGDRNO", nPangjengDrno);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateGbDentalbyWrtno(string strFrDate, string strToDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET                                                       ");
            parameter.AppendSql("       GBDENTAL = 'Y'                                                                  ");
            parameter.AppendSql(" WHERE WRTNO IN (SELECT a.WRTNO FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RESULT b ");
            parameter.AppendSql("                  WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                    ");
            parameter.AppendSql("                    AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                    ");
            parameter.AppendSql("                    AND (a.GbDental<>'Y' OR a.GbDental IS NULL)                        ");
            parameter.AppendSql("                    AND a.DelDate IS NULL                                              ");
            parameter.AppendSql("                    AND a.GjJong IN ('11','12','13','14','41','42','43')               "); //사업장1차,공무원1차,성인병1차,사업장1차(회사부담)
            parameter.AppendSql("                    AND a.WRTNO=b.WRTNO(+)                                             ");
            parameter.AppendSql("                    AND b.ExCode='ZD00')                                               ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetGjJongbyGWrtNo(long argGWrtNo, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GWRTNO, WRTNO, GJJONG, (SELECT NAME FROM HIC_EXJONG WHERE CODE = A.GJJONG) JONGNAME, UCODES ");
            parameter.AppendSql("     , GBADDPAN ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU A                                                                     ");
            parameter.AppendSql(" WHERE GWRTNO = :GWRTNO                                                                            ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                                   ");
            parameter.AppendSql("   AND GBSTS <> 'D'                                                                                "); //접수취소(삭제)는 제외
            parameter.AppendSql(" UNION ALL                                                                                         ");
            parameter.AppendSql("SELECT GWRTNO, WRTNO, '00' GJJONG, (SELECT NAME FROM HIC_EXJONG WHERE CODE = A.GJJONG) JONGNAME    ");
            parameter.AppendSql("      ,'' AS UCODES, '' AS GBADDPAN    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU A                                                                     ");
            parameter.AppendSql(" WHERE GWRTNO = :GWRTNO                                                                            ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("   AND GBSTS <> 'D'                                                                                "); //접수취소(삭제)는 제외
            parameter.AppendSql(" ORDER BY GJJONG                                                                                   ");

            parameter.Add("GWRTNO", argGWrtNo);
            parameter.Add("JEPDATE", strJepDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetPrtSabunbyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PRTSABUN                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public int GetCountbyPaNoGjYearGjjong(string strPANO, string strGjYear, string strJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE PANO = :PANO                                            ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                        ");
            parameter.AppendSql("   AND GJJONG = :GJJONG                                        ");
            parameter.AppendSql("   AND DELDATE IS NULL                                         ");

            parameter.Add("PANO", strPANO);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GJJONG", strJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_JEPSU> GetWrtNoJepDatebyPanoJepDateGjJong(object fnPano, object fstrJepDate, string[] strGjJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE PANO = :PANO                                            ");
            parameter.AppendSql("   AND JEPDATE < TO_DATE(:JEPDATE, 'YYYY-MM-DD')               ");
            parameter.AppendSql("   AND GJJONG IN (:GJJONG)                                     ");
            parameter.AppendSql("   AND GBSTS <> 'D'                                            "); //접수취소(삭제)는 제외
            parameter.AppendSql(" ORDER BY JepDate DESC                                         ");

            parameter.Add("PANO", fnPano);
            parameter.Add("JEPDATE", fstrJepDate);
            parameter.AddInStatement("GJJONG", strGjJong);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int UpdateGbJinChalSangdamdrnobyWrtNo(string idNumber, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            parameter.AppendSql("       GBJINCHAL   = 'Y'               ");
            parameter.AppendSql("     , SANGDAMDRNO = :SANGDAMDRNO      ");
            parameter.AppendSql("     , SANGDAMDATE = SYSDATE           ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO            ");

            parameter.Add("SANGDAMDRNO", idNumber);
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetGroupByListByLtdCode(long argLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT BUSENAME,GJJONG,UCODES,COUNT(*) AS CNT          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                           ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE('2014-02-01', 'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND DELDATE IS NULL                                 ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                              ");
            parameter.AppendSql("   AND UCODES IS NOT NULL                              ");
            parameter.AppendSql(" GROUP BY BUSENAME,GJJONG,UCODES                       ");
            parameter.AppendSql(" ORDER BY BUSENAME,GJJONG,UCODES                       ");

            parameter.Add("LTDCODE", argLtdCode);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int GetCountbyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                               ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                      ");
            parameter.AppendSql("   AND (PANJENGDRNO IS NOT NULL and PANJENGDRNO <> '0')    ");
            parameter.AppendSql("   AND SEXAMS <> '3170,'                                   ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateGbNunjinbyWrtNo(long nSabun, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            parameter.AppendSql("       GBMUNJIN1   = 'Y'               ");
            parameter.AppendSql("     , GBJINCHAL   = 'Y'               ");
            parameter.AppendSql("     , SANGDAMDRNO = :SANGDAMDRNO      ");
            parameter.AppendSql("     , SANGDAMDATE = SYSDATE           ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO            ");

            parameter.Add("SANGDAMDRNO", nSabun);
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateGbAutoPanbyWrtNo(HIC_JEPSU item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            parameter.AppendSql("       GBAUTOPAN = :GBAUTOPAN          ");
            parameter.AppendSql("     , ERFLAG    = :ERFLAG             ");
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO              ");

            parameter.Add("GBAUTOPAN", item.GBAUTOPAN);
            parameter.Add("ERFLAG", item.ERFLAG);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateUcodesbyWrtNo(long gnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET   ");
            parameter.AppendSql("       UCODES = 'ZZZ'              ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO             ");

            parameter.Add("WRTNO", gnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateErTongbobyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET   ");
            parameter.AppendSql("       ERTONGBO = TRUNC(SYSDATE)   ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO           ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatePanjengDatebyWrtNo(string strPanDate, long nPanDrno, long fnWRTNO, string strOK)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET                                                       ");
            if (strOK == "OK")
            {
                parameter.AppendSql("       PANJENGDATE = TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')                           ");
                parameter.AppendSql("     , PANJENGDRNO = :PANJENGDRNO                                                  ");
            }
            else
            {
                parameter.AppendSql("       PANJENGDATE = ''                                                            ");
                parameter.AppendSql("     , PANJENGDRNO = ''                                                            ");

            }
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                  ");

            if (strOK == "OK")
            {
                parameter.Add("PANJENGDATE", strPanDate);
                parameter.Add("PANJENGDRNO", nPanDrno);
            }
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public string GetSExamsbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SEXAMS                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU               ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int GetCntbyWrtNo(long wRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                               ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                      ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DelDate='')                     ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateCounselbyWrtNo(string idNumber, long fnWRTNO, int nTabSel, string strGbSang)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET                           ");
            parameter.AppendSql("       GBJINCHAL = 'Y'                                     ");
            if (nTabSel == 3)
            {
                parameter.AppendSql("     , GBMUNJIN1 = 'Y'                                 ");
            }
            parameter.AppendSql("     , SANGDAMDRNO = :SANGDAMDRNO                          ");
            if (strGbSang == "N")
            {
                parameter.AppendSql("     , SANGDAMDATE = SYSDATE                           ");
            }
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO                                ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("SANGDAMDRNO", idNumber);

            return ExecuteNonQuery(parameter);
        }

        public HIC_JEPSU GetSangdamDrNoPaNobyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SANGDAMDRNO, PANO                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                               ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                      ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDateLtdCodeGjJong(string strFrDate, string strToDate, string strGjYear, string strGjJong, List<string> strFirstGjJong, long nLtdCode, string strSort)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,SName,Age,Sex,TO_CHAR(JEPDate,'YYYY-MM-DD') JEPDate,GjJong    ");
            parameter.AppendSql("     , LtdCode,Pano,ChungguYN,DentChungguYN,GjYear,GjBangi,GbSTS           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                               ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                           ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                           ");
            parameter.AppendSql("   AND GJYEAR   = :GJYEAR                                                  ");
            parameter.AppendSql("   AND GbSTS <> 'D'                                                        ");
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                              ");
            }
            if (strGjJong == "**")
            {
                parameter.AppendSql("   AND GJJONG IN (:FIRSTGJJONG)                                        ");
            }
            else
            {
                parameter.AppendSql("   AND GJJONG = :GJJONG                                                ");
            }
            if (strSort == "2")
            {
                parameter.AppendSql(" ORDER BY SNAME, JEPDATE, GJJONG                                       ");
            }
            else
            {
                parameter.AppendSql(" ORDER BY JEPDATE, SNAME, GJJONG                                       ");
            }

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            if (strGjJong == "**")
            {
                parameter.AddInStatement("FIRSTGJJONG", strFirstGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            else
            {
                parameter.Add("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetEntTimebyWrtNoJepDate(long fnWrtNo, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(ENTTIME, 'HH24:MI') ENTTIME         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND GBSTS <> 'D'                                ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");

            parameter.Add("WRTNO", fnWrtNo);
            parameter.Add("JEPDATE", fstrJepDate);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public long GetPanjengDrNobyWrtNo(long fnWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PANJENGDRNO             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", fnWrtno);

            return ExecuteScalar<long>(parameter);
        }

        public string GetPanjengDrNobyWrtNo(List<long> fnWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID AS RID            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   ");
            parameter.AppendSql(" WHERE WRTNO IN (:WRTNO)       ");
            parameter.AppendSql("   AND (PANJENGDRNO = 0 OR PANJENGDRNO IS NULL)        ");
            parameter.AppendSql("   AND DELDATE IS NULL         ");

            parameter.AddInStatement("WRTNO", fnWrtno);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDateLtdCodeGjYearBangiJob(string fstrFDate, string fstrTDate, string fstrLtdCode, string strGjYear, string strBangi, string strJob, string pano = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,PANO,SNAME,TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE      ");
            parameter.AppendSql("     , GJJONG,GJCHASU,UCODES,GJYEAR,GJBANGI,SEX                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                       ");
            parameter.AppendSql(" WHERE JEPDATE>=TO_DATE(:FRDATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND JEPDATE<=TO_DATE(:TODATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND DELDATE IS NULL                                             ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                                          ");

            if (!pano.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND PANO  = :PANO                                           ");
            }
            if (!strGjYear.IsNullOrEmpty())
            {

                parameter.AppendSql("   AND GJYEAR  = :GJYEAR                                           ");
            }


            switch (strBangi)
            {
                case "상반기":
                    parameter.AppendSql("   AND GJBANGI = '1'                                       ");
                    break;
                case "하반기":
                    parameter.AppendSql("   AND GJBANGI = '2'                                       ");
                    break;
                default:
                    break;
            }
            if (strJob == "1") //특수
            {
                //parameter.AppendSql("   AND Gjjong IN ('11','12','14','23','41')                    ");
                parameter.AppendSql("   AND Gjjong IN ('11','12','14','23','41','22','24','28')      ");
            }
            else if (strJob == "2") //채용
            {
                parameter.AppendSql("   AND Gjjong IN ('22','24','30')                              ");
            }
            else
            {
                parameter.AppendSql("   AND Gjjong  IN ('69')                                       ");
            }
            parameter.AppendSql("   AND PanjengDrno IS NOT NULL                                     "); //판정완료된것만 읽음

            if (!pano.IsNullOrEmpty())
            {
                parameter.AppendSql("  ORDER BY GJYEAR DESC, SName,Pano,JepDate,WRTNO                                  ");
            }
            else
            {
                parameter.AppendSql(" ORDER BY SName,Pano,JepDate,WRTNO                                  ");
            }


            parameter.Add("FRDATE", fstrFDate);
            parameter.Add("TODATE", fstrTDate);
            parameter.Add("LTDCODE", fstrLtdCode);

            if (!pano.IsNullOrEmpty())
            {
                parameter.Add("PANO", pano);
            }
            if (!strGjYear.IsNullOrEmpty())
            {
                parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }


            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public long GetWrtNobyWrtNoJepdateGjYear(long argWrtNo, string fstrJepDate, string fstrYear, string argJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Wrtno                                                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                               ");
            parameter.AppendSql(" WHERE PANO IN (                                                           ");
            parameter.AppendSql("                SELECT PANO                                                ");
            parameter.AppendSql("                  FROM KOSMOS_PMPA.HIC_JEPSU                               ");
            parameter.AppendSql("                 WHERE WRTNO = :WRTNO                                      ");
            parameter.AppendSql("               )                                                           ");
            parameter.AppendSql("   AND JEPDATE < TO_DATE(:JEPDATE,'YYYY-MM-DD')                            ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                    ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                     ");
            switch (argJong)
            {
                case "16":
                case "17":
                case "18":
                case "19":
                    parameter.AppendSql("   AND GjJong IN ('11','12','13','14')                             ");
                    break;
                case "27":
                case "28":
                case "29":
                    parameter.AppendSql("   AND GjJong IN ('21','22','23')                                  ");
                    break;
                case "44":
                case "45":
                case "46":
                    parameter.AppendSql("   AND GjJong IN ('41','42','43')                                  ");
                    break;
                default:
                    break;
            }
            parameter.AppendSql(" ORDER BY JEPDATE DESC                                                     ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("JEPDATE", fstrJepDate);
            parameter.Add("GJYEAR", fstrYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<long>(parameter);
        }

        public int UpdateSangdamDrnobyRowId(string idNumber, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            parameter.AppendSql("       SANGDAMDRNO = :SANGDAMDRNO      ");
            parameter.AppendSql("     , SANGDAMDATE = SYSDATE           ");
            parameter.AppendSql(" WHERE ROWID       = :RID              ");

            parameter.Add("SANGDAMDRNO", idNumber);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDatePtno(string fstrJepDate, string fstrPtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SNAME, PTNO, JEPDATE, GJJONG, ROWID         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND PTNO    = :PTNO                             ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            parameter.AppendSql("   AND GJJONG IN ('62','69')                       ");

            parameter.Add("JEPDATE", fstrJepDate);
            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetJuso1Juso2byWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JUSO1, JUSO2            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public long GetWrtNobyJepdatePano(string argJepDate, long argPaNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO                                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                           ");
            parameter.AppendSql(" WHERE JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND PANO = :PANO                                    ");
            parameter.AppendSql("   AND DELDATE IS NULL                                 ");
            parameter.AppendSql("   AND GJJONG IN ('31','35')                           ");

            parameter.Add("JEPDATE", argJepDate);
            parameter.Add("PANO", argPaNo);

            return ExecuteScalar<long>(parameter);
        }

        public List<HIC_JEPSU> GetWrtNoJepDatebyJepDate(string strFrDate, string strToDate, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                           ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND DelDate IS NULL                                 ");
            parameter.AppendSql("   AND GjJong IN('50', '51')                           ");
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                          ");
            }
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public string GetGbAmbyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBAM                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdatePanjengDrNoPanDatebyWrtNo(long nWrtNo, long nPPanDrno, string strPanDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET                           ");
            parameter.AppendSql("       PANJENGDRNO = :PANJENGDRNO                          ");
            parameter.AppendSql("     , PANJENGDATE = TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO                                ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("PANJENGDRNO", nPPanDrno);
            parameter.Add("PANJENGDATE", strPanDate);

            return ExecuteNonQuery(parameter);
        }

        public string GetJusobyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Juso1 || Juso2 JUSO                                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                               ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                      ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_JEPSU> GetItembyPaNoGjYearGjBangiJepDate(long fnPano, string strGjYear, string strGjBangi, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JepDate,'YYYY-MM-DD') JEPDATE, WRTNO        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                               ");
            parameter.AppendSql(" WHERE PANO     = :PANO                                    ");
            parameter.AppendSql("   AND GJYEAR   = :GJYEAR                                  ");
            parameter.AppendSql("   AND GJBANGI  = :GJBANGI                                 ");
            parameter.AppendSql("   AND GJJONG IN ('50','51')                               ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:JEPDATE, 'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND DELDATE IS NULL                                     ");
            parameter.AppendSql(" ORDER BY JEPDATE, WRTNO                                   ");

            parameter.Add("PANO", fnPano);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GJBANGI", strGjBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", strJepDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int UpdateGbSujinbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET   ");
            parameter.AppendSql("       GBSUJIN_SET = 'Y'            ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO             ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetJepDatebyJepDateGjJong(string strFrDate, string strToDate, long nLtdCode, string strSName, string argJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO, TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                               ");
            parameter.AppendSql(" WHERE JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("   AND JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("   AND DelDate IS NULL                                     ");
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                              ");
            }
            if (strSName != "")
            {
                parameter.AppendSql("   AND SNAME = :SNAME                                  ");
            }
            parameter.AppendSql("   AND GJJONG    = :GJJONG                                 ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("LTDCODE", nLtdCode);
            parameter.Add("SNAME", strSName);
            parameter.Add("GJJONG", argJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public long GetSangdamDrNobyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SANGDAMDRNO                                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                               ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                                    ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<long>(parameter);
        }

        public int UpdateGbDentalbyResult(string strFrDate, string strToDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE HIC_JEPSU SET                                                                   ");
            parameter.AppendSql("       GBDENTAL = 'Y'                                                                  ");
            parameter.AppendSql(" WHERE WRTNO IN (SELECT a.WRTNO FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RESULT b ");
            parameter.AppendSql("                  WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                    ");
            parameter.AppendSql("                    AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                    ");
            parameter.AppendSql("                    AND (a.GbDental<>'Y' OR a.GbDental IS NULL)                        ");
            parameter.AppendSql("                    AND a.DelDate IS NULL                                              ");
            parameter.AppendSql("                    AND a.GjJong IN ('11','12','13','14','41','42','43')               "); //사업장1차,공무원1차,성인병1차,사업장1차(회사부담)
            parameter.AppendSql("                    AND a.WRTNO = b.WRTNO(+)                                           ");
            parameter.AppendSql("                    AND b.ExCode='ZD00')                                               ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetItembyPaNoJepDate(long fnPano, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,GjJong,SName,Sex,Age                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PANO    = :PANO                             ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("PANO", fnPano);
            parameter.Add("JEPDATE", strJepDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyUnionPaNo(long fnPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE, GJJONG, GJCHASU, WRTNO               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                                ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                             ");
            if (fnPano > 0)
            {
                parameter.AppendSql(" UNION ALL                                                                     ");
                parameter.AppendSql("SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE, 'XX' GJJONG, '1' GJCHASU, WRTNO  ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                                                   ");
                parameter.AppendSql(" WHERE PANO = :PANO                                                            ");
                parameter.AppendSql("   AND DELDATE IS NULL                                                         ");
            }
            parameter.AppendSql(" ORDER BY 1 DESC, 2                                                                ");

            parameter.Add("PANO", fnPano);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public string GetGjJongbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GJJONG                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDate(string strFrDate, string strToDate, string strJob, string strJong, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, SNAME, TO_CHAR(JEPDATE,'YY-MM-DD') JEPDATE, GJJONG   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                       ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                   ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                   ");
            parameter.AppendSql("   AND GBDENTAL = 'Y'                                              ");
            parameter.AppendSql("   AND DELDATE IS NULL                                             ");
            parameter.AppendSql("   AND GJJONG IN ('11','12','13','14','41','42','43')              "); //사업장1차,공무원1차,성인병1차,사업장1차(회사부담)
            if (strJob == "1") //신규
            {
                parameter.AppendSql("   AND (GBMUNJIN2 = 'N' or GBMUNJIN2 IS NULL )                 ");
            }
            else
            {
                parameter.AppendSql("   AND GBMUNJIN2 = 'Y'                                         ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                      ");
            }
            if (strJong != "**" && !strJong.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GJJONG = :GJJONG                                        ");
            }
            parameter.AppendSql("   AND GJYEAR >= '2009'                                            ");   //2009년부터
            parameter.AppendSql(" ORDER BY SNAME, WRTNO                                             ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (strJong != "**" && !strJong.IsNullOrEmpty())
            {
                parameter.Add("GJJONG", strJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepdateGbAddPan(string strFrDate, string strToDate, string strGJJONG)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                           ");
            parameter.AppendSql(" WHERE JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND DelDate IS NULL                                 ");
            parameter.AppendSql("   AND GjJong =:GJJONG                                 ");
            parameter.AppendSql("   AND GbAddPan ='Y'                                   ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GJJONG", strGJJONG);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyPaNoJepDateWrtNo(long argPano, string argJepDate, long fnWrtno1, long fnWrtno2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                ");
            parameter.AppendSql("   AND JEPDATE < TO_DATE(:JEPDATE, 'YYYY-MM-DD')                   ");
            parameter.AppendSql("   AND WRTNO NOT IN (:WRTNO1, :WRTNO2)                             "); //금년자료 제외
            parameter.AppendSql("   AND GbSTS <> 'D'                                                "); //접수취소(삭제)는 제외
            parameter.AppendSql(" ORDER BY JepDate DESC                                             ");

            parameter.Add("PANO", argPano);
            parameter.Add("JEPDATE", argJepDate);
            parameter.Add("WRTNO1", fnWrtno1);
            parameter.Add("WRTNO2", fnWrtno2);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int UpdatePanjengDatebyWrtNo(long fnWRTNO, string strOK, string strPanDate, long nPanDrno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET                               ");
            if (strOK == "OK")
            {
                parameter.AppendSql("       PANJENGDATE = TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')   ");
                parameter.AppendSql("     , PANJENGDRNO = :PANJENGDRNO                          ");
            }
            else
            {
                parameter.AppendSql("       PANJENGDATE = ''                                    ");
                parameter.AppendSql("     , PANJENGDRNO = ''                                    ");
            }
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO                                      ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("PANJENGDATE", strPanDate);
            parameter.Add("PANJENGDRNO", nPanDrno);

            return ExecuteNonQuery(parameter);
        }

        public HIC_JEPSU GetMuryoAmGubDaeSangbyWrtNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MURYOAM, GUBDAESANG         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDate(string strFrDate, string strToDate, string strSName, string strChkNew, string strGbChul, long nLtdCode, string sgrGjJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,SName,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,GjJong,LtdCode,Pano       ");
            parameter.AppendSql(" ,AGE, SEX                                                                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                       ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("   AND DELDATE IS  NULL                                                            ");
            if (!strSName.IsNullOrEmpty()) parameter.AppendSql("   AND SNAME LIKE :SNAME                                       ");
            if (strChkNew == "1") parameter.AppendSql("   AND GbSTS in ('1','2')                                    "); //접수자만
            if (strGbChul == "1") parameter.AppendSql("   AND GbChul = 'N'                                          "); //내원검진만
            if (strGbChul == "2") parameter.AppendSql("   AND GbChul = 'Y'                                          "); //출장검진만
            if (nLtdCode != 0) parameter.AppendSql("   AND LTDCODE = :LTDCODE                                       ");
            if (sgrGjJong != "**" && sgrGjJong != "") parameter.AppendSql("   AND GJJONG = :GJJONG                                     ");
            if (nLtdCode > 0)
            {
                parameter.AppendSql(" ORDER BY LtdCode,SName,WRTNO                                              "); //정렬(사업장,이름,접수번호)
            }
            else
            {
                parameter.AppendSql(" ORDER BY SName,WRTNO                                                      ");
            }

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strSName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", strSName);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            if (sgrGjJong != "**" && sgrGjJong != "")
            {
                parameter.Add("GJJONG", sgrGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetWrtNobyPanoGjYearBangiJepDate(long fnPano, string strGjYear, string strGjBangi, string strJepDate, string fstrGjJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,WRTNO                                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                       ");
            parameter.AppendSql(" WHERE PANO    = :PANO                                                             ");
            parameter.AppendSql("   AND GJYEAR  = :GJYEAR                                                           ");
            parameter.AppendSql("   AND GJBANGI = :GJBANGI                                                          ");
            parameter.AppendSql("   AND GJJONG NOT IN ('12','13','17','18','31','55','56','57','58','81','82','83') ");
            if (string.Compare(fstrGjJong, "21") >= 0 && string.Compare(fstrGjJong, "26") <= 0)
            {
                parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                               ");
            }
            else
            {
                parameter.AppendSql("   AND JEPDATE <= TO_DATE(:JEPDATE, 'YYYY-MM-DD')                              ");
            }
            parameter.AppendSql("   AND DELDATE IS NULL                                                             ");
            parameter.AppendSql(" ORDER BY JEPDATE, WRTNO                                                           ");

            parameter.Add("PANO", fnPano);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GJBANGI", strGjBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", strJepDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public string GetSexbyWrtNo(string argJepNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SEX                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", argJepNo);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdatebyWrtNo(HIC_JEPSU item2, string argGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET   ");
            if (argGbn == "저장")
            {
                parameter.AppendSql("       GBSPCMUNJIN = 'Y'                           ");
                parameter.AppendSql("     , GBJINCHAL   = 'Y'                           ");
                parameter.AppendSql("     , GBMUNJIN3   = 'Y'                           ");
            }
            else
            {
                parameter.AppendSql("       GBSPCMUNJIN = 'N'                           ");
                parameter.AppendSql("     , GBMUNJIN3   = 'N'                           ");
            }
            parameter.AppendSql("     , SABUN     = :SABUN                              ");
            parameter.AppendSql("     , IPSADATE  = TO_DATE(:IPSADATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("     , BUSENAME  = :BUSENAME                           ");
            parameter.AppendSql("     , BUSEIPSA  = TO_DATE(:BUSEIPSA, 'YYYY-MM-DD')    ");
            parameter.AppendSql("     , GBSUCHEP  = :GBSUCHEP                           ");
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO                              ");

            parameter.Add("SABUN", item2.SABUN);
            parameter.Add("IPSADATE", item2.IPSADATE);
            parameter.Add("BUSENAME", item2.BUSENAME);
            parameter.Add("BUSEIPSA", item2.BUSEIPSA);
            parameter.Add("GBSUCHEP", item2.GBSUCHEP);
            parameter.Add("WRTNO", item2.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateXrayNo(string strResult, long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET   ");
            parameter.AppendSql("       XRAYNO = :XRAYNO            ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO             ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("XRAYNO", strResult);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDatePanjengDate()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, SNAME, LTDCODE, GJJONG, GBSTS    ");
            parameter.AppendSql("     , TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                   ");
            parameter.AppendSql(" WHERE JEPDATE >= TRUNC(SYSDATE - 20)          ");
            parameter.AppendSql("   AND PANJENGDATE IS NULL                     ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND GbSTS IN ('1','2')                      ");
            parameter.AppendSql(" ORDER BY SNAME                                ");

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public long GetItembyPaNoGjYearGjBangiJepdateGjJong(long fnPano, string strGjYear, string strGjBangi, string strJepDate, string fstrGjJong, string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE, WRTNO, GJJONG        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                       ");
            parameter.AppendSql(" WHERE PANO    = :PANO                                             ");
            parameter.AppendSql("   AND DELDATE IS NULL                                             ");
            parameter.AppendSql("   AND GJYEAR  = :GJYEAR                                           ");
            parameter.AppendSql("   AND GJBANGI = :GJBANGI                                          ");

            if (strGubun == "1")
            {
                parameter.AppendSql("   AND JEPDATE > TO_DATE(:JEPDATE, 'YYYY-MM-DD')               ");
                switch (fstrGjJong)
                {
                    case "21":
                        parameter.AppendSql("   AND GJJONG  ='27'                                   "); //일반채용2차
                        break;
                    case "22":
                        parameter.AppendSql("   AND GJJONG  ='29'                                   "); //채용배치전 2차
                        break;
                    case "23":
                        parameter.AppendSql("   AND GJJONG  ='28'                                   "); //특수검진2차
                        break;
                    case "24":
                        parameter.AppendSql("   AND GJJONG  ='33'                                   "); //배치전2차
                        break;
                    default:
                        break;
                }
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql("   AND JEPDATE < TO_DATE(:JEPDATE, 'YYYY-MM-DD')               ");
                switch (fstrGjJong)
                {
                    case "27":
                        parameter.AppendSql("   AND GJJONG  ='21'                                   "); //일반채용2차
                        break;
                    case "29":
                        parameter.AppendSql("   AND GJJONG  ='22'                                   "); //채용배치전 2차
                        break;
                    case "28":
                        parameter.AppendSql("   AND GJJONG  ='23'                                   "); //특수검진2차
                        break;
                    case "33":
                        parameter.AppendSql("   AND GJJONG  ='24'                                   "); //배치전2차
                        break;
                    default:
                        break;
                }
            }
            parameter.AppendSql(" ORDER BY JEPDATE, WRTNO                                           ");

            parameter.Add("PANO", fnPano);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GJBANGI", strGjBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", strJepDate);

            return ExecuteReaderSingle<long>(parameter);
        }

        public string GetUcodesbyWrtNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT UCODES                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_JEPSU GetItembyWrtNo1(long fnWrtno1)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Pano,SName,Age,Sex,LtdCode,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,GjJong,UCodes  ");
            parameter.AppendSql("     , Sabun,TO_CHAR(IpsaDate,'YYYY-MM-DD') IpsaDate,BuseName,GbSuchep,Jikjong         ");
            parameter.AppendSql("     , GjYear,GjChasu,GjBangi,LtdCode                                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                           ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                  ");

            parameter.Add("WRTNO", fnWrtno1);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyPanoGjYear(long fnPano, string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(JEPDATE, 'YYYY-MM-DD') JEPDATE, WRTNO, GJJONG                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                   ");
            parameter.AppendSql(" WHERE PANO   = :PANO                                                          ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                         ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                        ");
            parameter.AppendSql("   AND GJJONG NOT IN ('21','22','23','24','27','28','29','33','31','32','35')  ");
            parameter.AppendSql("   AND GJJONG <='46'                                                           ");
            parameter.AppendSql(" ORDER BY JEPDATE, WRTNO                                                       ");

            parameter.Add("PANO", fnPano);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public string GetJepDatebyWrtNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public string GetNamebyMirNo(long argMirno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.NAME                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a     ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_CODE  b     ");
            parameter.AppendSql(" WHERE a.MIRNO4 = :MIRNO4          ");
            parameter.AppendSql("   AND b.GUBUN = '25'              ");  //보건소코드
            parameter.AppendSql("   AND a.BOGUNSO=b.CODE(+)         ");
            parameter.AppendSql(" GROUP BY b.NAME                   ");

            parameter.Add("WRTNO", argMirno);

            return ExecuteScalar<string>(parameter);
        }

        public long GetWrtNo2byPano(long pANO, string strYear, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE PANO = :PANO                                            ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                        ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                                      ");
            parameter.AppendSql("   AND GJJONG IN ('11','12','14','23','25','26','41','42')     ");
            parameter.AppendSql("   AND UCodes IS NOT NULL                                      ");
            parameter.AppendSql("   AND DelDate IS NULL                                         ");

            parameter.Add("PANO", pANO);
            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", nLtdCode);

            return ExecuteScalar<long>(parameter);
        }

        public List<HIC_JEPSU> GetIetmbyJepDate(string strJepDate, string strYear, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,SName,BuseName,GjJong,UCodes,Pano                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE JepDate >= TO_DATE(:JEPDATE, 'YYYY-MM-DD')              ");
            parameter.AppendSql("   AND GjYEAR  = :GjYEAR                                       ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                                      ");
            parameter.AppendSql("   AND GjJong IN ('11','12','14','23','25','26','41','42')     ");
            parameter.AppendSql("   AND UCodes IS NOT NULL                                      ");
            parameter.AppendSql("   AND DelDate IS NULL                                         ");

            parameter.Add("JEPDATE", strJepDate);
            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", nLtdCode);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int UpdatePanjengDatePanjenghDrNobyWrtNo(string strPanDate, long gnHicLicense, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET                           ");
            parameter.AppendSql("       PANJENGDATE = TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("     , PANJENGDRNO = :PANJENGDRNO                          ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO                                ");

            parameter.Add("PANJENGDATE", strPanDate);
            parameter.Add("PANJENGDRNO", gnHicLicense);
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDateLtdCodeSName(string strFrDate, string strToDate, long nLtdCode, string strSName, string[] strGjJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                       ");
            parameter.AppendSql(" WHERE JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                   ");
            parameter.AppendSql("   AND JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                   ");
            parameter.AppendSql("   AND DelDate IS NULL                                             ");
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                      ");
            }
            if (strSName != "")
            {
                parameter.AppendSql("   AND SNAME = :SNAME                                          ");
            }
            parameter.AppendSql("   AND GJJONG IN (:GJJONG)                                         ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("FRDATE", strToDate);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            if (strSName != "")
            {
                parameter.Add("SNAME", strSName);
            }
            parameter.AddInStatement("GJJONG", strGjJong);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyOnlyPaNo(long fnPano, long nHeaPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(JEPDATE, 'YYYY-MM-DD') JEPDATE, GJJONG, GJCHASU, WRTNO          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                   ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                            ");
            parameter.AppendSql("   AND DelDate IS NULL                                                         ");
            if (nHeaPano > 0)
            {
                parameter.AppendSql(" UNION ALL                                                                 ");
                parameter.AppendSql("SELECT TO_CHAR(SDate,'YYYY-MM-DD') JepDate,'XX' GjJong,'1' GjChasu,WRTNO   ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                                               ");
                parameter.AppendSql(" WHERE PANO = :PANO2                                                        ");
                parameter.AppendSql("   AND DelDate IS NULL                                                     ");
            }
            parameter.AppendSql(" ORDER BY 1 DESC,2                                                             ");

            parameter.Add("PANO", fnPano);
            if (nHeaPano > 0)
            {
                parameter.Add("PANO2", nHeaPano);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public string GetGjYearbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GJYEAR                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO         ");
            parameter.AppendSql(" ORDER BY WRTNO                ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_JEPSU GetItembyWrtNo(long fWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PANO, WRTNO, SNAME, AGE, SEX, LTDCODE, TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE, GJYEAR, PTNO  ");
            parameter.AppendSql("     , TO_CHAR(IPSADATE,'yyyy-mm-dd') IPSADATE, GJJONG, UCODES, SEXAMS, ERFLAG, WAITREMARK         ");
            parameter.AppendSql("     , SABUN, BUSENAME, GBSUCHEP, JIKJONG, GJCHASU, GJBANGI, GBADDPAN                              ");
            parameter.AppendSql("     , JONGGUMYN, GBHEAENDO, IEMUNNO, GBN, CLASS, GBCHK1, GBCHK2, GBCHK3                           ");
            parameter.AppendSql("     , TEL, BUSEIPSA, SANGDAMDRNO, SANGDAMDATE, GBSTS                                              ");
            parameter.AppendSql("     , MIRNO1, MIRNO2, MIRNO3, MIRNO4,MIRNO5                                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                              ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                                             ");

            parameter.Add("WRTNO", fWrtNo);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public int UpdateGbMinjin2byWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            parameter.AppendSql("       GBMUNJIN2 = 'Y'                 ");
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO              ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetJepsuInfobyPano(long argPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, GJJONG, SNAME, SEX, AGE          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                   ");
            parameter.AppendSql(" WHERE PANO = :PANO                            ");
            parameter.AppendSql("   AND JEPDATE = TRUNC(SYSDATE)                ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");

            parameter.Add("PANO", argPano);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDateLtdCodeGjYearBangi(string strFrDate, string strToDate, string strGjJong, string strGjYear, string strBangi)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT LTDCODE, SECOND_FLAG, SECOND_TONGBO, COUNT(*) CNT               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                           ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                       ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                       ");
            if (strGjJong == "1")
            {
                parameter.AppendSql("   AND GJJONG IN ('11','14','23')                                  ");     //사업장1차,특수검진
            }
            else if (strGjJong == "2")
            {
                parameter.AppendSql("   AND GJJONG IN ('12')                                            "); //공무원
            }
            else
            {
                parameter.AppendSql("   AND GJJONG IN ('11','12','14','23')                             "); //전체
            }
            parameter.AppendSql("   AND GJCHASU = '1'                                                   ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                 ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                ");
            if (strBangi != "")
            {
                parameter.AppendSql("   AND GJBANGI = :GJBANGI                                          ");
            }
            parameter.AppendSql(" GROUP BY LTDCODE,SECOND_FLAG,SECOND_TONGBO                            ");
            parameter.AppendSql(" ORDER BY LTDCODE,SECOND_FLAG,SECOND_TONGBO                            ");

            if (strBangi != "")
            {
                parameter.Add("GJBANGI", strBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDateGjYearLtdCode(string strFrDate, string strToDate, string strGjYear, long nLtdCode, string strGbn)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(MIN(JepDate),'YYYY-MM-DD') StartDate            ");
            parameter.AppendSql("     , TO_CHAR(MAX(JepDate),'YYYY-MM-DD') EndDate              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')               ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')               ");
            parameter.AppendSql("   AND GJYEAR  = :GJYEAR                                       ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                                      ");
            if (strGbn == "")
            {
                parameter.AppendSql("   AND GJJONG IN ('11','12','23','41','42')                ");
            }
            else if (strGbn == "Y")
            {
                parameter.AppendSql("   AND GJJONG IN ('11','12','14','23','25','26','41','42') ");
            }
            parameter.AppendSql("   AND DELDATE IS NULL                                         ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", nLtdCode);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetJepDatebyGjYear(string strFrDate, string strToDate, string strGjYear, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(MIN(JepDate),'YYYY-MM-DD') StartDate            ");
            parameter.AppendSql("     , TO_CHAR(MAX(JepDate),'YYYY-MM-DD') EndDate              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')               ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')               ");
            parameter.AppendSql("   AND GJYEAR  = :GJYEAR                                       ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                                      ");
            parameter.AppendSql("   AND GJJONG IN ('11','12','23','41','42')                    ");
            parameter.AppendSql("   AND DELDATE IS NULL                                         ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", nLtdCode);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public int UpdateXrayResultbyWrtNo(long nPano, string strXrayno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            parameter.AppendSql("       DelDate = ''                    ");
            parameter.AppendSql(" WHERE PANO    = :PANO                 ");
            parameter.AppendSql("   AND XRAYNO  = :XRAYNO               ");

            parameter.Add("PANO", nPano);
            parameter.Add("XRAYNO", strXrayno);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateXrayNobyWrtNo(string strXrayno, long wRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            parameter.AppendSql("       XRAYNO = :XRAYNO               ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                 ");

            parameter.Add("XRAYNO", strXrayno);
            parameter.Add("WRTNO", wRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int DeleteSangdamWaitbyPaNo(long fPaNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE PANO = :PANO                    ");

            parameter.Add("PANO", fPaNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateSecond_TongBobyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            parameter.AppendSql("       SECOND_Tongbo = TRUNC(SYSDATE)  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateGbMunJinbyWrtNo(long fnWRTNO, string argGbn, string argGbMunjin)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            if (argGbn == "저장")
            {
                if (argGbMunjin != "수정")
                {
                    parameter.AppendSql("       GBMUNJIN1 = 'Y'         ");
                    parameter.AppendSql("     , GBJINCHAL = 'Y'         ");
                }
                else
                {
                    parameter.AppendSql("       GBMUNJIN1 = 'Y'         ");
                }
            }
            else
            {
                parameter.AppendSql("       GBMUNJIN1 = 'N'             ");
            }
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyPanjengDrNoWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                           ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                  ");
            parameter.AppendSql("   AND (PANJENGDRNO IS NOT NULL and PANJENGDRNO <>'0') ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateGbExamByWrtno(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU   ");
            parameter.AppendSql("   SET GBEXAM = 'Y'            ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO       ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateWaitRemarkbyWrtNo(string strRemark, long fWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET   ");
            parameter.AppendSql("       WAITREMARK = :WAITREMARK    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", fWrtNo);
            parameter.Add("WAITREMARK", strRemark);

            return ExecuteNonQuery(parameter);
        }

        public long GetWrtnoByPanoJepDate(long nPano, string argBDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND GJJONG IN ('31', '35')                      ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("PANO", nPano);
            parameter.Add("JEPDATE", argBDate);

            return ExecuteScalar<long>(parameter);
        }

        public HIC_JEPSU GetWrtnoByPanoJepDateGjJong(long fnPano, string fstrJepDate, string strGjYear, string strJob)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");
            parameter.AppendSql("   AND JEPDATE > TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                            ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            if (strJob == "1")
            {
                parameter.AppendSql("   AND Gjjong IN('16', '17', '19', '28', '44') ");
            }
            else if (strJob == "2")
            {
                parameter.AppendSql("   AND Gjjong IN ('29','30')                   ");
            }
            else if (strJob == "3")
            {
                parameter.AppendSql("   AND Gjjong  IN ('69')                       ");
            }
            parameter.AppendSql(" ORDER BY WRTNO                                    ");

            parameter.Add("PANO", fnPano);
            parameter.Add("JEPDATE", fstrJepDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetMinMaxDatebyWrtNo(string strFDate, string strTDate, string strGjYear, string fstrLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MIN(TO_CHAR(JepDate,'YYYY-MM-DD')) MinDate,MAX(TO_CHAR(JepDate,'YYYY-MM-DD')) MaxDate   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                                   ");
            parameter.AppendSql(" WHERE WRTNO IN (                                                                              ");
            parameter.AppendSql("                  SELECT a.wrtno as wrtno                                                      ");
            parameter.AppendSql("                    FROM KOSMOS_PMPA.HIC_JEPSU    a                                            ");
            parameter.AppendSql("                       , KOSMOS_PMPA.HIC_X_MUNJIN b                                            ");
            parameter.AppendSql("                       , KOSMOS_PMPA.HIC_LTD      c                                            ");
            parameter.AppendSql("                   WHERE a.JepDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')                            ");
            parameter.AppendSql("                     AND a.JepDate <= TO_DATE(:TODATE,'YYYY-MM-DD')                            ");
            parameter.AppendSql("                     AND a.DelDate IS NULL                                                     ");
            parameter.AppendSql("                     AND a.GJYEAR = :GJYEAR                                                    ");
            parameter.AppendSql("                     AND a.Gjjong IN ('50','51')                                               ");
            parameter.AppendSql("                     AND a.WRTNO = b.WRTNO(+)                                                  ");
            parameter.AppendSql("                     AND b.PanjengDrno IS NOT NULL                                             ");
            parameter.AppendSql("                     AND a.LTDCODE = c.Code(+)                                                 ");
            parameter.AppendSql("                     AND a.LTDCODE = :LTDCODE)                                                 ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", fstrLtdCode);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public string GetJepDatebyPanoGjYear(long nPano, string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(JEPDATE,'YYYYMMDD') JEPDATE         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                            ");
            parameter.AppendSql("   AND GJCHASU = '2'                               ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("PANO", nPano);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetTongDatebyPanoGjYear(long nPano, string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(TONGBODATE,'YYYYMMDD') TONGBODATE   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                            ");
            parameter.AppendSql("   AND GJCHASU = '2'                               ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("PANO", nPano);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetWrtnobyPanoGjYear(long nPano, string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                            ");
            parameter.AppendSql("   AND GJJONG IN ('16','28')                       ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("PANO", nPano);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_JEPSU GetPaNoGjYearbyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO, GJYEAR, GJCHASU                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public string GetMaxDatebyJepDate(string strFDate, string strTDate, string fstrLtdCode, string strGjYear, string strGjBangi, string strJob)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(TO_CHAR(JepDate,'YYYY-MM-DD')) MAXDATE                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                       ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                             ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                          ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                                            ");
            if (strGjBangi != "")
            {
                parameter.AppendSql("   AND GJBANGI = :GJBANGI                                                      ");
            }
            if (strJob == "1")
            {
                parameter.AppendSql("   AND Gjjong IN ('11','12','14','16','17','19','23','28','41','42','44','45') ");
            }
            else if (strJob == "2")
            {
                parameter.AppendSql("   AND Gjjong IN ('22','24','29','30','33')                                    ");
            }
            else
            {
                parameter.AppendSql("   AND Gjjong IN ('69')                                                        ");
            }

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            parameter.Add("LTDCODE", fstrLtdCode);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (strGjBangi != "")
            {
                parameter.Add("GJBANGI", strGjBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteScalar<string>(parameter);
        }

        public long GetWrtnoByPanoJepDateGjYear(long fnPano, string strJepDate, string strGjYear, string strBangi)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");
            parameter.AppendSql("   AND JEPDATE >= TO_DATE(:JEPDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND GJJONG IN ('16','17','18','19','44','45')   ");
            parameter.AppendSql("   AND GJCHASU = '2'                               ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                            ");
            parameter.AppendSql("   AND (UCodes IS NULL OR UCodes = 'ZZZ')          ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            if (strBangi != "")
            {
                parameter.AppendSql("   AND GJBANGI = :GJBANGI                      ");
            }

            parameter.Add("PANO", fnPano);
            parameter.Add("JEPDATE", strJepDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (strBangi != "")
            {
                parameter.Add("GJBANGI", strBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteScalar<long>(parameter);
        }

        public List<HIC_JEPSU> GetItembyJepDateLtdCodeGjYear(string fstrFDate, string fstrTDate, string fstrLtdCode, string strGjYear, string strBangi)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,PANO,TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE, GJJONG, GBINWON, SNAME    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                       ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                          ");
            parameter.AppendSql("   AND GJYEAR  = :GJYEAR                                                           ");
            parameter.AppendSql("   AND GJJONG IN ('11','12','14','41','42')                                        "); //사업장1차,공무원1차,사업장회사부담1차
            parameter.AppendSql("   AND GJCHASU = '1'                                                               ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                             ");
            parameter.AppendSql("   AND GBINWON IN ('21','22','23','31','32','64','65','66','67','68')              ");
            switch (strBangi)
            {
                case "상반기":
                    parameter.AppendSql("   AND GJBANGI = '1'                                                       ");
                    break;
                case "하반기":
                    parameter.AppendSql("   AND GJBANGI = '2'                                                       ");
                    break;
                default:
                    break;
            }
            parameter.AppendSql(" ORDER BY SNAME, WRTNO                                                             ");

            parameter.Add("FRDATE", fstrFDate);
            parameter.Add("TODATE", fstrTDate);
            parameter.Add("LTDCODE", fstrLtdCode);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetListByItems(string argFDate, string argTDate, string argJong, string argSName, long argLtdCode, bool fbChul, bool fbJongGum, bool fbKaTalk, bool fbDel, bool fnEndo, bool fnHabit)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT KOSMOS_PMPA.FC_CHECK_HC_ENDOJUPMST(PTNO, TO_CHAR(JEPDATE,'YYYY-MM-DD')) AS ENDOGBN              ");
            parameter.AppendSql("      ,KOSMOS_PMPA.FC_READ_HC_ENDO_RESULTDATE(PTNO, TO_CHAR(JEPDATE,'YYYY-MM-DD')) AS ENDORESULT       ");
            parameter.AppendSql("      ,KOSMOS_PMPA.FC_HIC_GRPCD_SUNAPCHK(WRTNO) AS CHKSUNAP                                            ");
            parameter.AppendSql("      ,WRTNO,GJYEAR,JEPDATE,PANO,SNAME,SEX,AGE,GJJONG,FC_HIC_GJJONG_NAME(GJJONG, UCODES) AS NAME,GJCHASU ");
            parameter.AppendSql("      ,GJBANGI,GBCHUL,GBSTS, GBINWON, DELDATE, FC_HIC_LTDNAME(LTDCODE) AS LTDNAME, LTDCODE, MAILCODE   ");
            parameter.AppendSql("      ,JUSO1, JUSO2, TEL,PTNO, BURATE, JISA, KIHO, GKIHO                                               ");
            parameter.AppendSql("      ,JIKGBN, UCODES, SEXAMS, JIKJONG, SABUN, IPSADATE, BUSENAME, OLDJIKJONG, BUSEIPSA, OLDSDATE      ");
            parameter.AppendSql("      ,OLDEDATE, GBSUCHEP, BALYEAR, BALSEQ, GBEXAM, BOGUNSO, JEPSUGBN, YOUNGUPSO, JEPNO, GBSPCMUNJIN   ");
            parameter.AppendSql("      ,GBPRINT, GBSABUN, GBJINCHAL, GBMUNJIN1, GBMUNJIN2, GBMUNJIN3, GBDENTAL, SECOND_FLAG             ");
            parameter.AppendSql("      ,SECOND_DATE, SECOND_TONGBO, SECOND_EXAMS, SECOND_SAYU, CHUNGGUYN, DENTCHUNGGUYN                 ");
            parameter.AppendSql("      ,CANCERCHUNGGUYN, LIVER2, MAMT, FAMT, MILEAGEAM, MURYOAM, GUMDAESANG                             ");
            parameter.AppendSql("      ,DECODE(JONGGUMYN, '1', 'Y', '') AS JONGGUMYN, SEND                                              ");
            parameter.AppendSql("      ,MILEAGEAMGBN, MURYOGBN, NEGODATE, JOBSABUN, ENTTIME, SECOND_MISAYU, MIRNO1, MIRNO2              ");
            parameter.AppendSql("      ,MIRNO3, EMAIL, REMARK, OHMSNO, MISUNO1, FIRSTPANDRNO, ERFLAG, MIRNO4, MIRSAYU, ERTONGBO         ");
            parameter.AppendSql("      ,MISUNO2, GUBDAESANG, MIRNO5, MISUNO3, XRAYNO, CARDSEQNO, GBADDPAN, GBAM, BALDATE, TONGBODATE    ");
            parameter.AppendSql("      ,GBSUJIN_SET, GBCHUL2, GBJINCHAL2, SANGDAMDRNO, SANGDAMDATE, SENDMSG, GBN, CLASS, BAN            ");
            parameter.AppendSql("      ,BUN, HEMSNO, HEMSMIRSAYU, AUTOJEP, MISUNO4, P_WRTNO, PANJENGDATE, PANJENGDRNO, GBAUDIO2EXAM     ");
            parameter.AppendSql("      ,GBMUNJINPRINT, WAITREMARK, PDFPATH, WEBSEND, WEBSENDDATE, GBNAKSANG, GBDENTONLY                 ");
            parameter.AppendSql("      ,GBAUTOPAN, CASE WHEN IEMUNNO > 0 THEN '▦' ELSE '' END AS GBIEMUN, IEMUNNO, WEBPRINTREQ          ");
            parameter.AppendSql("      ,GBSUJIN_CHK,WEBPRINTSEND, LTDCODE2, DENTAMT, GBHUADD                                            ");
            parameter.AppendSql("      ,EXAMREMARK, PRTSABUN, GBHEAENDO, GBCHK1, GBCHK2, GBCHK3, GBLIFE, GBJUSO, GWRTNO, JEPBUN         ");
            parameter.AppendSql("      ,FC_HIC_GBPRIVACY_DATE(PTNO) AS PRIVACY_DATE                                                     ");
            parameter.AppendSql("      ,KOSMOS_PMPA.FC_HC_PATIENT_JUMINNO(PTNO) AS JUMINNO                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                                           ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                           ");
            parameter.AppendSql("   AND JEPDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                                                        ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')                                                        ");

            if (fbDel) { parameter.AppendSql("   AND DELDATE IS NOT NULL                                                                "); }
            else { parameter.AppendSql("   AND DELDATE IS NULL                                                                          "); }

            if (fbChul) { parameter.AppendSql("   AND GBCHUL = 'Y'                                                                      "); }
            if (fbJongGum) { parameter.AppendSql("   AND JONGGUMYN = '1'                                                                "); }
            if (fbKaTalk) { parameter.AppendSql("   AND WEBPRINTREQ IS NOT NULL                                                         "); }
            if (fnEndo) { parameter.AppendSql("   AND KOSMOS_PMPA.FC_HIC_ENDO_LIST(WRTNO) IS NOT NULL                                   "); }
            if (fnHabit) { parameter.AppendSql("   AND (SEXAMS LIKE '%1164%' OR SEXAMS LIKE '%1165%' OR SEXAMS LIKE '%1166%')           "); }

            if (!argJong.Equals("**"))
            {
                parameter.AppendSql("   AND GJJONG = :GJJONG                                                                            ");
            }

            if (!argSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SNAME LIKE  :SNAME                                                                          ");
            }

            if (argLtdCode > 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                                          ");
            }

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);

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

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetSexAgebyPtNo(string strPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SEX, AGE                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                   ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                           ");
            parameter.AppendSql("   AND JEPDATE = TRUNC(SYSDATE)                ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");

            parameter.Add("PTNO", strPtno);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyGbSts(string strGbSts)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, PTNO, XRAYNO, GJJONG, SEX            ");
            parameter.AppendSql("     , AGE,TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE JEPDATE >= TRUNC(SYSDATE-20)                ");
            parameter.AppendSql("   AND JEPDATE <= TRUNC(SYSDATE-1)                 ");
            parameter.AppendSql("   AND JONGGUMYN = '1'                             ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            parameter.AppendSql("   AND GBSTS = :GBSTS                              ");

            parameter.Add("GBSTS", strGbSts, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyUCodes(string sUCodes1, string sUCodes2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, SNAME, IEMUNNO                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                           ");
            parameter.AppendSql(" WHERE JEPDATE >= TRUNC(SYSDATE - 6)                   ");
            parameter.AppendSql("   AND JONGGUMYN = '1'                                 ");
            parameter.AppendSql("   AND (UCODES LIKE :UCODES1 OR UCODES LIKE :UCODES2)  ");

            parameter.AddLikeStatement("UCODES1", sUCodes1);
            parameter.AddLikeStatement("UCODES2", sUCodes2);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public string GetPtnoByGWRTNO(long argGWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PTNO FROM KOSMOS_PMPA.HIC_JEPSU WHERE GWRTNO = :GWRTNO  ");

            parameter.Add("GWRTNO", argGWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public IList<HIC_JEPSU> GetListsByGWRTNO(long nGWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,GJYEAR,TO_CHAR(JEPDATE, 'YYYY-MM-DD') JEPDATE,PANO,SNAME,SEX,AGE,GJJONG,GJCHASU,GJBANGI,GBCHUL  ");
            parameter.AppendSql("      ,GBSTS, GBINWON, DELDATE, LTDCODE, MAILCODE, JUSO1, JUSO2, TEL, PTNO, BURATE, JISA, KIHO, GKIHO  ");
            parameter.AppendSql("      ,JIKGBN, UCODES, SEXAMS, JIKJONG, SABUN, IPSADATE, BUSENAME, OLDJIKJONG, BUSEIPSA, OLDSDATE      ");
            parameter.AppendSql("      ,OLDEDATE, GBSUCHEP, BALYEAR, BALSEQ, GBEXAM, BOGUNSO, JEPSUGBN, YOUNGUPSO, JEPNO, GBSPCMUNJIN   ");
            parameter.AppendSql("      ,GBPRINT, GBSABUN, GBJINCHAL, GBMUNJIN1, GBMUNJIN2, GBMUNJIN3, GBDENTAL, SECOND_FLAG             ");
            parameter.AppendSql("      ,SECOND_DATE, SECOND_TONGBO, SECOND_EXAMS, SECOND_SAYU, CHUNGGUYN, DENTCHUNGGUYN                 ");
            parameter.AppendSql("      ,CANCERCHUNGGUYN, LIVER2, MAMT, FAMT, MILEAGEAM, MURYOAM, GUMDAESANG, JONGGUMYN, SEND            ");
            parameter.AppendSql("      ,MILEAGEAMGBN, MURYOGBN, NEGODATE, JOBSABUN, ENTTIME, SECOND_MISAYU, MIRNO1, MIRNO2              ");
            parameter.AppendSql("      ,MIRNO3, EMAIL, REMARK, OHMSNO, MISUNO1, FIRSTPANDRNO, ERFLAG, MIRNO4, MIRSAYU, ERTONGBO         ");
            parameter.AppendSql("      ,MISUNO2, GUBDAESANG, MIRNO5, MISUNO3, XRAYNO, CARDSEQNO, GBADDPAN, GBAM, BALDATE, TONGBODATE    ");
            parameter.AppendSql("      ,GBSUJIN_SET, GBCHUL2, GBJINCHAL2, SANGDAMDRNO, SANGDAMDATE, SENDMSG, GBN, CLASS, BAN            ");
            parameter.AppendSql("      ,BUN, HEMSNO, HEMSMIRSAYU, AUTOJEP, MISUNO4, P_WRTNO, PANJENGDATE, PANJENGDRNO, GBAUDIO2EXAM     ");
            parameter.AppendSql("      ,GBMUNJINPRINT, WAITREMARK, PDFPATH, WEBSEND, WEBSENDDATE, GBNAKSANG, GBDENTONLY                 ");
            parameter.AppendSql("      ,GBAUTOPAN, IEMUNNO, WEBPRINTREQ, GBSUJIN_CHK, WEBPRINTSEND, LTDCODE2, DENTAMT, GBHUADD          ");
            parameter.AppendSql("      ,EXAMREMARK, PRTSABUN, GBHEAENDO, GBCHK1, GBCHK2, GBCHK3, GBLIFE, GBJUSO, GWRTNO, JEPBUN         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                                           ");
            parameter.AppendSql(" WHERE GWRTNO = :GWRTNO                                                                                ");
            parameter.AppendSql(" ORDER BY GJJONG                                                                                       ");

            parameter.Add("GWRTNO", nGWRTNO);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetItembyPaNo(long fPaNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, GJJONG, SNAME, SEX, AGE          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                   ");
            parameter.AppendSql(" WHERE PANO = :PANO                            ");
            parameter.AppendSql("   AND JEPDATE = TRUNC(SYSDATE)                ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");

            parameter.Add("PANO", fPaNo);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetUcodesbyPaNo(long fPaNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GJJONG, UCODES              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU       ");
            parameter.AppendSql(" WHERE PANO = :PANO                ");
            parameter.AppendSql("   AND JEPDATE = TRUNC(SYSDATE)    ");
            parameter.AppendSql("   AND DELDATE IS NULL             ");

            parameter.Add("PANO", fPaNo);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetAllbyJumin2(string gJJONG, string jUMIN2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_JEPSU                        ");
            parameter.AppendSql(" WHERE JepDate=TRUNC(SYSDATE)                  ");
            parameter.AppendSql("   AND GJJONG = :GJJONG                        ");
            parameter.AppendSql("   AND Pano IN (SELECT Pano FROM HIC_PATIENT   ");
            parameter.AppendSql("                 WHERE JUMIN2 = :JUMIN2        ");

            parameter.Add("GJJONG", gJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMIN2", jUMIN2);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetJepsuInfobyJepDate(string argDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,GJYEAR,SEX,AGE,GBCHUL,WRTNO, PTNO     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                       ");
            parameter.AppendSql(" WHERE JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                             ");
            parameter.AppendSql("   AND GBSTS NOT IN ('2','D')                                                      ");

            parameter.Add("JEPDATE", argDate);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public IList<Dictionary<string, object>> ValidJepsu(string argPtno, string argDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.GWRTNO                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a                       ");
            parameter.AppendSql(" WHERE a.PTNO    = :PTNO                             ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                             ");
            parameter.AppendSql("   AND a.JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql(" UNION ALL                                           ");
            parameter.AppendSql("SELECT b.GWRTNO                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU b                       ");
            parameter.AppendSql(" WHERE b.PTNO    = :PTNO                             ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                             ");
            parameter.AppendSql("   AND b.SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')       ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("JEPDATE", argDate);
            parameter.Add("SDATE", argDate);

            return ExecuteReader<Dictionary<string, object>>(parameter);
        }

        public long Read_Jepsu_Wrtno3(string argPano, string argYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU       ");
            parameter.AppendSql(" WHERE PANO    = :PANO             ");
            parameter.AppendSql("   AND GJJONG = '11'               ");
            parameter.AppendSql("   AND DELDATE IS NULL             ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR            ");

            parameter.Add("PANO", argPano);
            parameter.Add("GJYEAR", argYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<long>(parameter);
        }

        public long GetWrtnoByPano_Cholesterol(long nPano, DateTime argDate, string argYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU             ");
            parameter.AppendSql(" WHERE PANO    = :PANO                             ");
            parameter.AppendSql("   AND JEPDATE < :JEPDATE                          ");
            parameter.AppendSql("   AND GJJONG IN ('11','12','13','14','21','23')   ");
            parameter.AppendSql("   AND GJYEAR  = :GJYEAR                           ");
            parameter.AppendSql(" ORDER BY JEPDATE DESC                             ");

            parameter.Add("PANO", nPano);
            parameter.Add("JEPDATE", argDate);
            parameter.Add("GJYEAR", argYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<long>(parameter);
        }

        public long GetPanoByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO FROM KOSMOS_PMPA.HIC_JEPSU ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                   ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<long>(parameter);
        }

        public int UpDate_FirstPanDrno(HIC_JEPSU item1)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU           ");
            parameter.AppendSql("   SET FIRSTPANDRNO =:FIRSTPANDRNO     ");
            parameter.AppendSql(" WHERE WRTNO        =:WRTNO          ");

            #region Query 변수대입
            parameter.Add("FIRSTPANDRNO", item1.FIRSTPANDRNO);
            parameter.Add("WRTNO", item1.WRTNO);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public int UpDate_GbSTS(string[] strMunOK, string strSTS, long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU       ");
            parameter.AppendSql("   SET GBSTS     = :GBSTS          ");

            if (strMunOK != null)
            {
                parameter.AppendSql("      ,GBMUNJIN1 =:GBMUNJIN1      ");
                parameter.AppendSql("      ,GBMUNJIN2 =:GBMUNJIN2      ");
                parameter.AppendSql("      ,GBMUNJIN3 =:GBMUNJIN3      ");
            }

            parameter.AppendSql(" WHERE WRTNO       = :WRTNO          ");

            #region Query 변수대입
            parameter.Add("GBSTS", strSTS);

            if (strMunOK != null)
            {
                parameter.Add("GBMUNJIN1", strMunOK[1]);
                parameter.Add("GBMUNJIN2", strMunOK[2]);
                parameter.Add("GBMUNJIN3", strMunOK[3]);
            }

            parameter.Add("WRTNO", argWRTNO);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public string Read_JepsuSts(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBSTS                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_GjJong(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GJJONG                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_JEPSU Read_Jepsu_Wrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Ptno, Pano, SName, Age, Sex, GjYear, GjJong       ");
            parameter.AppendSql("     , JepDate,GjChasu, LtdCode, UCodes            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE = '')           ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }


        public string Read_SName(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public long Read_Jepsu_WrtNo(long argPano, string argJepDate, string argGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                           ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                                ");
            parameter.AppendSql("   AND JEPDATE < TO_DATE(:JEPDATE,'YYYY-MM-DD')        ");
            parameter.AppendSql("   AND DELDATE IS NULL                                 ");

            parameter.Add("PANO", argPano);
            parameter.Add("GJYEAR", argGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", argJepDate);

            return ExecuteScalar<long>(parameter);
        }

        public List<HIC_JEPSU> GetWrtnobyGubun(string strWaitRoom, string strSName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT A.WRTNO, A.SNAME, A.GJJONG, B.NAME, C.PTNO,C.SEX   ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT A                     ");
            parameter.AppendSql("      , KOSMOS_PMPA.HIC_EXJONG       B                     ");
            parameter.AppendSql("      , KOSMOS_PMPA.HIC_JEPSU        C                     ");
            parameter.AppendSql("  WHERE A.EntTime >= TRUNC(SYSDATE)                        ");
            parameter.AppendSql("    AND (A.CallTime IS NULL OR a.CallTime = '')            ");
            parameter.AppendSql("    AND A.GUBUN  = :GUBUN                                  ");
            parameter.AppendSql("    AND A.GJJONG = B.CODE(+)                               ");
            parameter.AppendSql("    AND A.WRTNO  = C.WRTNO(+)                              ");
            parameter.AppendSql("    AND C.GBCHUL = 'N'                                     "); //내원
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AppendSql("    AND C.SNAME LIKE :SNAME                            ");
            }
            parameter.AppendSql("  ORDER By A.WaitNo,A.WRTNO                                ");

            parameter.Add("GUBUN", strWaitRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!strSName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", strSName);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetWrtnobyJepDate(string strJepDate, string strGbChul, string sSName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO, A.SNAME, A.GJJONG, B.NAME, A.PTNO, A.SEX               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU  A                                        ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXJONG B                                        ");
            parameter.AppendSql(" WHERE A.JEPDATE = TO_DATE(:JEPSUDATE, 'YYYY-MM-DD')                   ");
            parameter.AppendSql("   AND A.DELDATE IS  NULL                                              ");
            parameter.AppendSql("   AND A.GJJONG = B.CODE(+)                                            ");
            if (strGbChul == "1")
            {
                parameter.AppendSql("   AND A.GBCHUL = 'N'                                              "); //내원
            }
            else if (strGbChul == "2")
            {
                parameter.AppendSql("   AND A.GBCHUL = 'Y'                                              "); //사업장
            }
            if (!sSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND A.SNAME LIKE :SNAME                                         ");
            }
            parameter.AppendSql(" ORDER By WRTNO                                                        ");

            parameter.Add("JEPSUDATE", strJepDate);
            if (!sSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", sSName);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetWrtnoByPano_All(long nPano, string strJepDAte)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                        ");
            parameter.AppendSql(" WHERE PANO    = :PANO                             ");
            parameter.AppendSql("   AND JEPDATE = to_date(:JEPDATE, 'yyyy-MM-dd')   ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("PANO", nPano);
            parameter.Add("JEPDATE", strJepDAte);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public int GetJepsuCountbyPaNo(long argPaNo, string argJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT count('X') CNT                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("PANO", argPaNo);
            parameter.Add("JEPDATE", argJepDate);

            return ExecuteScalar<int>(parameter);
        }


        public long GetAgeByWrtno(long ArgWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT AGE                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", ArgWrtno);

            return ExecuteScalar<long>(parameter);
        }

        public HIC_JEPSU GetAutoJepByWrtno(long argWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT AUTOJEP                                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                   ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            parameter.AppendSql("   AND WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetItemByPanoJepdateGjjong(string argPtno, string argJepDate, string argGjJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT JOBSABUN, UCODES                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");
            parameter.AppendSql("   AND PTNO = :PTNO                                ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE,'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND GJJONG = :GJJONG                            ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("JEPDATE", argJepDate);
            parameter.Add("GJJONG", argGjJong);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public int UpdateWebPrintSendByWrtNo(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET                   ");
            parameter.AppendSql(" PDFPATH = ''                                      ");
            parameter.AppendSql(" ,WEBPRINTSEND = SYSDATE                           ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                            ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_JEPSU> GetItemByJepdateLife(string argFrDate, string argToDate, string argSName, long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO, a.SNAME, a.SEX, a.AGE                                                                  ");
            parameter.AppendSql("     , a.JEPDATE, a.GJJONG, a.LTDCODE, a.ERFLAG, a.PANO, b.JUMIN2                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   a                                                                       ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT b                                                                       ");
            parameter.AppendSql(" WHERE a.PTNO = b.PTNO                                                                                 ");
            parameter.AppendSql("   AND a.JEPDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                                       ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                                                       ");
            parameter.AppendSql("   AND a.GJJONG IN ('11')                                                                              ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                                               ");
            //parameter.AppendSql("   AND (a.AGE = '20' OR a.AGE = '30' OR a.AGE = '40' OR a.AGE = '50' OR a.AGE = '60' OR a.AGE = '70')  ");
            parameter.AppendSql("   AND (a.SEXAMS LIKE '%1164%' OR a.SEXAMS LIKE '%1165%' OR a.SEXAMS LIKE '%1166%')  ");

            if (argWrtno > 0)
            {
                parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                                            ");
            }
            if (!argSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME = :SNAME                                                                            ");
            }

            parameter.AppendSql(" ORDER BY a.JEPDATE, a.WRTNO, a.SNAME                                                                  ");


            parameter.Add("FDATE", argFrDate);
            parameter.Add("TDATE", argToDate);
            if (!argSName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", argSName);
            }
            if (argWrtno > 0)
            {
                parameter.Add("WRTNO", argWrtno);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }


        public List<HIC_JEPSU> GetItemByJepdateLiverC(string argFrDate, string argToDate, string argSName, long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO, a.SNAME, a.SEX, a.AGE                                                                  ");
            parameter.AppendSql("     , a.JEPDATE, a.GJJONG, a.LTDCODE, a.ERFLAG, a.PANO, b.JUMIN2                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   a                                                                       ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT b                                                                       ");
            parameter.AppendSql(" WHERE a.PTNO = b.PTNO                                                                                 ");
            parameter.AppendSql("   AND a.JEPDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                                       ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                                                       ");
            parameter.AppendSql("   AND a.GJJONG IN ('11')                                                                              ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                                               ");
            parameter.AppendSql("   AND A.SEXAMS LIKE '%1170%'                                                                          ");

            if (argWrtno > 0)
            {
                parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                                            ");
            }
            if (!argSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME = :SNAME                                                                            ");
            }

            parameter.AppendSql(" ORDER BY a.JEPDATE, a.WRTNO, a.SNAME                                                                  ");


            parameter.Add("FDATE", argFrDate);
            parameter.Add("TDATE", argToDate);
            if (!argSName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", argSName);
            }
            if (argWrtno > 0)
            {
                parameter.Add("WRTNO", argWrtno);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU Read_Jepsu_GWRTNO(string argPtno, string argJepdate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GJJONG, GWRTNO                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE,'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND ROWNUM = 1                                  ");
            parameter.AppendSql(" ORDER BY JEPDATE DESC                             ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("JEPDATE", argJepdate);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }


        public int UpdateTongbodatePrtsabunbyWrtNo(long argWRTNO, long argSABUN)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            parameter.AppendSql("       TONGBODATE = SYSDATE            ");
            parameter.AppendSql("      ,PRTSABUN = :SABUN              ");
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO              ");

            parameter.Add("WRTNO", argWRTNO);
            parameter.Add("SABUN", argSABUN);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatePrtsabunbyWrtNo(long argWRTNO, long argSABUN)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            parameter.AppendSql("      PRTSABUN = :SABUN               ");
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO              ");

            parameter.Add("WRTNO", argWRTNO);
            parameter.Add("SABUN", argSABUN);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatePRINTbyWrtNo(long argWRTNO, long argSABUN, string strTongDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET                           ");
            parameter.AppendSql("      GBPRINT = 'Y'                                        ");
            parameter.AppendSql("      ,TONGBODATE = TO_DATE(:TONGBODATE,'YYYY-MM-DD')      ");
            parameter.AppendSql("      ,PRTSABUN = :SABUN                                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                      ");

            parameter.Add("WRTNO", argWRTNO);
            parameter.Add("SABUN", argSABUN);
            parameter.Add("TONGBODATE", strTongDate);
            

            return ExecuteNonQuery(parameter);
        }
        public int UpdateSnamebyWrtNo(long argWRTNO, string argSNAME)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU SET       ");
            parameter.AppendSql("       SNAME = :SNAME                  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                 ");

            parameter.Add("WRTNO", argWRTNO);
            parameter.Add("SNAME", argSNAME);

            return ExecuteNonQuery(parameter);
        }
        public HIC_JEPSU GetItemByPanoGjyearJepdateGjjong(string argPano, string argGjyear, string argJepdate, string argGjjongCha)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, UCODES                                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                   ");
            parameter.AppendSql(" WHERE 1=1                                                     ");
            parameter.AppendSql(" AND GJCHASU = '1'                                             ");
            parameter.AppendSql(" AND DELDATE IS NULL                                           ");
            parameter.AppendSql(" AND PANO = :PANO                                              ");
            parameter.AppendSql(" AND GJYEAR = :GJYEAR                                          ");
            parameter.AppendSql(" AND JEPDATE = :JEPDATE                                        ");
            if (argGjjongCha == "1")
            {
                parameter.AppendSql(" AND GJJONG IN ('11','12','13','14','41','42','43')        ");
            }
            else
            {
                parameter.AppendSql(" AND GJJONG IN ('21','22','23','24','25','26')             ");
            }


            parameter.Add("PANO", argPano);
            parameter.Add("GJYEAR", argGjyear);
            parameter.Add("JEPDATE", argJepdate);


            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        
        public List<HIC_JEPSU> GetItembyJepdate1(string strFrDate, string strToDate, string strSName, string strGbChul, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO, TO_CHAR(JEPDATE, 'YYYY-MM-DD') JEPDATE, GJJONG, SNAME, SEX, AGE, GBCHUL, PTNO        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                       ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("   AND DELDATE IS  NULL                                                            ");

            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE OR KIHO = :LTDCODE                                   ");
            }

            if (!strSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SNAME LIKE :SNAME                                                        ");
            }
            if (strGbChul == "1") parameter.AppendSql("   AND GbChul = 'N'                                          "); //내원검진만
            if (strGbChul == "2") parameter.AppendSql("   AND GbChul = 'Y'                                          "); //출장검진만
            parameter.AppendSql(" ORDER BY SName,Jepdate                                                            ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strSName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", strSName);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public List<HIC_JEPSU> GetWrtnoYearByPanoJepdateJong(long argPano, string argJepdate, string argGjjong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO, GJYEAR        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                       ");
            parameter.AppendSql(" WHERE JEPDATE < TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("   AND PANO = :PANO                                                                ");
            parameter.AppendSql("   AND DELDATE IS  NULL                                                            ");
            parameter.AppendSql("   AND GJJONG = :GJJONG                                                            ");
            parameter.AppendSql(" ORDER BY JEPDATE DESC                                                             ");

            parameter.Add("PANO", argPano);
            parameter.Add("JEPDATE", argJepdate);
            parameter.Add("GJJONG", argGjjong);

            return ExecuteReader<HIC_JEPSU>(parameter);
        }

        public HIC_JEPSU GetItemByWrtnoGjjong(long argWrtNo, string agrGjjong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(TONGBODATE,'YYYY-MM-DD') TONGBODATE ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql(" AND GJJONG = :GJJONG                              ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("GJJONG", agrGjjong);

            return ExecuteReaderSingle<HIC_JEPSU>(parameter);
        }

        public int GetCountChkBySexams(long argWrtNo, string argSexams)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT  COUNT('X') CNT FROM KOSMOS_PMPA.HIC_JEPSU    ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                            ");
            parameter.AppendSql("   AND SEXAMS LIKE :SEXAMS                       ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.AddLikeStatement("SEXAMS", argSexams);

            return ExecuteScalar<int>(parameter);
        }

    }
}
