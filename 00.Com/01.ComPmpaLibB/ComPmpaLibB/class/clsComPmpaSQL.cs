using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Data;
using System;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : clsComPmpaSQL.cs
    /// Description     : 원무마감관련 쿼리관련 class
    /// Author          : 김해수
    /// Create Date     : 2018-10-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history> 
    class clsComPmpaSQL
    {
        //string SQL = "";
        //string SqlErr = ""; //에러문 받는 변수

        #region 함수 관련

        #endregion

        #region NON 트랜잭션 쿼리

        #region frmPmpaViewOutSunapList.cs 쿼리
        public DataTable sel_VIEW01_Check(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                         ";
            SQL += ComNum.VBLF + "  a.Pano,a.Bi,a.SuNext,c.SuNameK,d.SName,SUM(a.Amt) Amt                        ";
            SQL += ComNum.VBLF + "FROM  IPD_NEW_CASH a,IPD_TRANS b,BAS_SUN c,IPD_NEW_MASTER d                    ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                      ";
            SQL += ComNum.VBLF + "  AND a.ActDate=TO_DATE('" + argDate + "','YYYY-MM-DD')                        ";
            SQL += ComNum.VBLF + "  AND a.Bun IN ('88','92','96','98')                                           ";
            SQL += ComNum.VBLF + "  AND a.Amt <> 0                                                               ";
            SQL += ComNum.VBLF + "  AND a.TRSNO=b.TRSNO(+)                                                       ";
            SQL += ComNum.VBLF + "  AND (b.ActDate IS NULL OR b.ActDate>TO_DATE('" + argDate + "','YYYY-MM-DD')) ";
            SQL += ComNum.VBLF + "  AND a.SuNext=c.SuNext(+)                                                     ";
            SQL += ComNum.VBLF + "  AND a.IPDNO=d.IPDNO(+)                                                       ";
            SQL += ComNum.VBLF + "GROUP BY a.Pano,a.Bi,a.SuNext,c.SuNameK,d.SName                                ";
            SQL += ComNum.VBLF + "ORDER BY a.Pano,a.Bi,a.SuNext                                                  ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }
        #endregion

        #region frmPmpaMagamDailyCheck.cs 쿼리 

        #region 점검 수납 II  쿼리
        public DataTable sel_DailyCheck_SunsuNap(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;
            ComFunc CF = new ComFunc();

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                ";
            SQL += ComNum.VBLF + "  a.PTNO ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDATE,b.SName, COUNT(a.PTNO) CNT              ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER a, ADMIN.BAS_PATIENT b                      ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                             ";
            SQL += ComNum.VBLF + "  AND a.Ptno=b.Pano(+)                                                                ";
            SQL += ComNum.VBLF + "  AND a.PTNO IN ( SELECT PANO FROM ADMIN.IPD_TRANS WHERE OUTDATE >=TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, argDate, -1) + "','YYYY-MM-DD')  AND ACTDATE IS NULL AND GBIPD NOT IN ('D')) ";  //퇴원대상자만
            SQL += ComNum.VBLF + "  AND a.BDate >=TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, argDate, -1) + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  AND a.GBSEND = ' '                                                                  ";
            SQL += ComNum.VBLF + "  AND a.GBPRN ='A'                                                                    ";
            SQL += ComNum.VBLF + "  AND a.GBSTATUS NOT IN ('D','D-','D+')                                               ";
            SQL += ComNum.VBLF + "  AND (a.GBIOE ='I' OR a.GBIOE IS NULL )                                              ";
            SQL += ComNum.VBLF + "  GROUP BY a.Ptno,TO_CHAR(a.BDate,'YYYY-MM-DD'), b.SName                              ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 개인점검1 쿼리 
        public DataTable sel_DailyCheck_Search1(PsmhDb pDbCon, string argDate, string argPart)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                    ";
            SQL += ComNum.VBLF + "  PANO,DEPTCODE,SUM(AMT) AMT                              ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH                    ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND ActDate =TO_DATE('" + argDate + "','YYYY-MM-DD')    ";
            SQL += ComNum.VBLF + "  AND BUN IN  ('85','87','89')                            ";//입원은 보증금, 중간납, 퇴원금
            SQL += ComNum.VBLF + "  AND PART ='" + argPart + "'                             ";
            SQL += ComNum.VBLF + "GROUP BY PANO,DEPTCODE                                    ";
            SQL += ComNum.VBLF + "  HAVING SUM(AMT) >= 300000                               ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 개인점검1 서브 쿼리
        public DataTable sel_DailyCheck_Search1_1(PsmhDb pDbCon, string argDate, string argPart, string argPano, string argDept)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs1 = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                        ";
            SQL += ComNum.VBLF + "  SUM(DECODE(TranHeader,'2',TradeAmt * -1, TradeAmt)) CardAmt ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV                        ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "  AND ACTDATE =TO_DATE('" + argDate + "','YYYY-MM-DD')        ";
            SQL += ComNum.VBLF + "  AND PANO ='" + argPano + "'                                 ";
            SQL += ComNum.VBLF + "  AND DEPTCODE IN ('" + argDept + "' )                        ";
            SQL += ComNum.VBLF + "  AND GUBUN  IN ('1','2')                                     ";
            SQL += ComNum.VBLF + "  AND PTGUBUN ='1'                                            ";
            SQL += ComNum.VBLF + "  AND GBIO ='I'                                               ";
            SQL += ComNum.VBLF + "  AND PART ='" + argPart + "'                                 ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs1, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs1;
        }
        #endregion

        #region 점검1쿼리
        public DataTable sel_DailyCheck_JumGum1(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT IPDNO,TRSNO, PANO FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "  AND ACTDATE IS NULL                                         ";
            SQL += ComNum.VBLF + "  AND GBIPD = '1'                                             ";
            SQL += ComNum.VBLF + "  AND GBSTS = '0'                                             ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검1_1쿼리
        public DataTable sel_DailyCheck_JumGum1_1(PsmhDb pDbCon, string argIPDNO, string argTRSNO)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT GBSTS, PANO, SNAME, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                         ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                         ";
            SQL += ComNum.VBLF + "  AND IPDNO = " + argIPDNO + "                                       ";
            SQL += ComNum.VBLF + "  AND LASTTRS = " + argTRSNO + "                                  ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검1_2쿼리
        public DataTable sel_DailyCheck_JumGum1_2(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT IPDNO,TRSNO, PANO FROM " + ComNum.DB_PMPA + "IPD_TRANS  ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "  AND ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')       ";
            SQL += ComNum.VBLF + "  AND GBIPD = '9'                                             ";
            SQL += ComNum.VBLF + "  AND ( OUTDATE IS NULL OR OUTDATE ='' )                      ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검1_3쿼리
        public DataTable sel_DailyCheck_JumGum1_3(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT SNAME,IPDNO,PANO,TO_CHAR(INDATE,'YYYY-MM-DD') INDATE   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "  AND ACTDATE IS NULL                                         ";
            SQL += ComNum.VBLF + "  AND OUTDATE IS NULL                                         ";
            SQL += ComNum.VBLF + "  AND GBSTS  <> '0'                                           ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검1_4쿼리
        public DataTable sel_DailyCheck_JumGum1_4(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT PANO,SNAME,TO_CHAR(INDATE,'YYYY-MM-DD') INDATE     ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                 ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND (ActDate IS NOT NULL OR ActDate ='')                ";
            SQL += ComNum.VBLF + "  AND (OutDate IS NULL OR OutDate ='')                    ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검2쿼리
        public DataTable sel_DailyCheck_JumGum2(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT IPDNO, PANO, SNAME, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                         ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                         ";
            SQL += ComNum.VBLF + "  AND ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')           ";
            SQL += ComNum.VBLF + "  AND  (OutDate IS NULL OR OutDate ='')                           ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검2_1쿼리
        public DataTable sel_DailyCheck_JumGum2_1(PsmhDb pDbCon, string argIPDNO)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT GBSTS FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
            SQL += ComNum.VBLF + "WHERE 1=1                                         ";
            SQL += ComNum.VBLF + "  AND IPDNO = " + argIPDNO + "                    ";
            SQL += ComNum.VBLF + "  AND GBIPD <> 'D'                                ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검2_2쿼리
        public DataTable sel_DailyCheck_JumGum2_2(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT IPDNO, PANO, SNAME, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                         ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                         ";
            SQL += ComNum.VBLF + "  AND JDATE >= TO_DATE('1999-01-01','YYYY-MM-DD')                 ";
            SQL += ComNum.VBLF + "  AND GBSTS  >= '1'                                               ";
            SQL += ComNum.VBLF + "  AND (OUTDATE ='' OR OUTDATE IS NULL )                           ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검3쿼리
        public DataTable sel_DailyCheck_JumGum3(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT IPDNO, PANO, SNAME, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                         ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                         ";
            SQL += ComNum.VBLF + "  AND ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')           ";
            SQL += ComNum.VBLF + "  AND GBSTS  = '7'                                                ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검3_1쿼리
        public DataTable sel_DailyCheck_JumGum3_1(PsmhDb pDbCon, string argIPDNO)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT BUN, SUM(AMT) AMT                  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH    ";
            SQL += ComNum.VBLF + "WHERE 1=1                                 ";
            SQL += ComNum.VBLF + "  AND IPDNO = " + argIPDNO + "            ";
            SQL += ComNum.VBLF + "  AND BUN IN ('85','87','88')             ";
            SQL += ComNum.VBLF + "GROUP BY BUN                              ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검4쿼리
        public DataTable sel_DailyCheck_JumGum4(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT TRSNO, AMT50, PANO                                 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS                      ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "  AND GBSTS  = '7'                                        ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검4_1쿼리
        public DataTable sel_DailyCheck_JumGum4_1(PsmhDb pDbCon, string argTRSNO)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT SUM(AMT1+AMT2) AMT                 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                 ";
            SQL += ComNum.VBLF + "  AND TRSNO  = " + argTRSNO + "           ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검5쿼리
        public DataTable sel_DailyCheck_JumGum5(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT A.PANO, A.TRSNO, A.IPDNO, B.SNAME, SUM(AMT) AMT        ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH A, IPD_NEW_MASTER B   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "  AND A.ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')     ";
            SQL += ComNum.VBLF + "  AND A.BUN = '88'                                            ";
            SQL += ComNum.VBLF + "  AND A.IPDNO = B.IPDNO                                       ";
            SQL += ComNum.VBLF + "GROUP BY A.PANO, A.TRSNO, A.IPDNO, B.SNAME                    ";
            SQL += ComNum.VBLF + "HAVING SUM(AMT) <> 0                                          ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검5_1쿼리
        public DataTable sel_DailyCheck_JumGum5_1(PsmhDb pDbCon, string argTRSNO)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT PANO                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                 ";
            SQL += ComNum.VBLF + "  AND TRSNO = " + argTRSNO + "            ";
            SQL += ComNum.VBLF + "  AND BUN IN ('89','91')                  ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검6쿼리
        public DataTable sel_DailyCheck_JumGum6(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE, A.PANO, B.SNAME   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS A, IPD_NEW_MASTER B           ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                         ";
            SQL += ComNum.VBLF + "  AND A.ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')         ";
            SQL += ComNum.VBLF + "  AND A.IPDNO = B.IPDNO                                           ";
            SQL += ComNum.VBLF + "  AND A.GBIPD <> 'D'                                              ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검7쿼리
        public DataTable sel_DailyCheck_JumGum7(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT PANO, LASTTRS, DRCODE, SNAME                                           ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                      ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "  AND (OUTDATE IS NULL OR OUTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD'))  ";
            SQL += ComNum.VBLF + "  AND GBSTS NOT IN ('9')                                                      ";
            SQL += ComNum.VBLF + "ORDER BY PANO                                                                 ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검7_1쿼리
        public DataTable sel_DailyCheck_JumGum7_1(PsmhDb pDbCon, string argLASTTRS)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT DRCODE                         ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS  ";
            SQL += ComNum.VBLF + "WHERE 1=1                             ";
            SQL += ComNum.VBLF + "  AND TRSNO = " + argLASTTRS + "      ";
            SQL += ComNum.VBLF + "  AND GBIPD NOT IN ('D','9')          ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검8쿼리
        public DataTable sel_DailyCheck_JumGum8(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT TRSNO, PANO                                        ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS                      ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "  AND GBSTS NOT IN ('9')                                  ";
            SQL += ComNum.VBLF + "  AND BI IN ('41','42','43','51','55')                    ";
            SQL += ComNum.VBLF + "ORDER BY PANO                                             ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검8_1쿼리
        public DataTable sel_DailyCheck_JumGum8_1(PsmhDb pDbCon, string argTRSNO)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT PANO                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                 ";
            SQL += ComNum.VBLF + "  AND TRSNO = " + argTRSNO + "            ";
            SQL += ComNum.VBLF + "  AND BUN  = '98'                         ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검9쿼리
        public DataTable sel_DailyCheck_JumGum9(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT TRSNO                                              ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS                      ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')   ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검9_1쿼리
        public DataTable sel_DailyCheck_JumGum9_1(PsmhDb pDbCon, string argTRSNO)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT TRSNO, PANO                    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS  ";
            SQL += ComNum.VBLF + "WHERE 1=1                             ";
            SQL += ComNum.VBLF + "  AND TRSNO = " + argTRSNO + "        ";
            SQL += ComNum.VBLF + "  AND OUTDATE IS NULL                 ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검10쿼리
        public DataTable sel_DailyCheck_JumGum10(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT PANO, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE              ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "  AND (ACTDATE IS NULL                                        ";
            SQL += ComNum.VBLF + "      OR ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD'))   ";
            SQL += ComNum.VBLF + "  AND GBSTS <> '9'                                            ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검10_1쿼리
        public DataTable sel_DailyCheck_JumGum10_1(PsmhDb pDbCon, string argPANO, string argDate, string argDateAdd)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT PANO, SNAME, BI, AMT7, DEPTCODE, DRCODE,           ";
            SQL += ComNum.VBLF + "  TO_CHAR(DATE3,'YYYY-MM-DD') YDATE,                      ";
            SQL += ComNum.VBLF + "  TO_CHAR(DATE3,'HH24:MI') YTIME                          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW               ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND PANO = '" + argPANO + "'                            ";
            SQL += ComNum.VBLF + "  AND TRANSDATE IS NULL                                   ";
            SQL += ComNum.VBLF + "  AND RETDATE IS NULL                                     ";
            SQL += ComNum.VBLF + "  AND DATE3 >= TO_DATE('" + argDate + "','YYYY-MM-DD')    ";
            SQL += ComNum.VBLF + "  AND DATE3 < TO_DATE('" + argDateAdd + "','YYYY-MM-DD')  ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검11쿼리
        public DataTable sel_DailyCheck_JumGum11(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT PANO                                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "  AND (NU = ' ' OR NU IS NULL OR NU ='' )                 ";
            SQL += ComNum.VBLF + "GROUP BY PANO                                             ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검12쿼리
        public DataTable sel_DailyCheck_JumGum12(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT PANO                                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS                      ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "  AND  SangAmt > 0                                        ";
            SQL += ComNum.VBLF + "  AND GBIPD <> 'D'                                        ";
            SQL += ComNum.VBLF + "GROUP BY PANO                                             ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검13쿼리
        public DataTable sel_DailyCheck_JumGum13(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT a.PANO,a.Trsno,a.Bi,SUM(AMT1) AMT1                 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a                 ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND a.ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "GROUP BY a.PANO,a.Trsno,a.Bi                              ";
            SQL += ComNum.VBLF + "HAVING SUM(AMT1) <> 0                                     ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검13_1쿼리
        public DataTable sel_DailyCheck_JumGum13_1(PsmhDb pDbCon, string argTRSNO, string argBI)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT PANO, BI                       ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS   ";
            SQL += ComNum.VBLF + "WHERE 1=1                             ";
            SQL += ComNum.VBLF + "  AND TRSNO = " + argTRSNO + "         ";
            SQL += ComNum.VBLF + "  AND BI = '" + argBI + "'            ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검14쿼리
        public DataTable sel_DailyCheck_JumGum14(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT b.SNAME,a.PANO,a.Trsno,a.Bi                                            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS a, " + ComNum.DB_PMPA + "BAS_PATIENT b    ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "  AND a.Pano=b.Pano                                                           ";
            SQL += ComNum.VBLF + "  AND ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')                       ";
            SQL += ComNum.VBLF + "GROUP BY b.SNAME,a.PANO,a.Trsno,a.Bi                                          ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검14_1쿼리
        public DataTable sel_DailyCheck_JumGum14_1(PsmhDb pDbCon, string argTRSNO, string argBI, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT PANO, BI                                           ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND TRSNO = " + argTRSNO + "                            ";
            SQL += ComNum.VBLF + "  AND BI = '" + argBI + "'                                ";
            SQL += ComNum.VBLF + "  AND ACTDATE >TO_DATE('" + argDate + "','YYYY-MM-DD')    ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검14_2쿼리
        public DataTable sel_DailyCheck_JumGum14_2(PsmhDb pDbCon, string argSysDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT b.SNAME,a.PANO,a.Trsno,a.Bi                                            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, " + ComNum.DB_PMPA + "BAS_PATIENT b  ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "  AND a.Pano=b.Pano                                                           ";
            SQL += ComNum.VBLF + "  AND ACTDATE > TO_DATE('" + argSysDate + "','YYYY-MM-DD')                    ";
            SQL += ComNum.VBLF + "GROUP BY b.SNAME,a.PANO,a.Trsno,a.Bi                                          ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검14_3쿼리
        public DataTable sel_DailyCheck_JumGum14_3(PsmhDb pDbCon, string argTRSNO, string argSysDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT SUM(AMT1) TAmt                                         ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "  AND TRSNO = " + argTRSNO + "                                ";
            SQL += ComNum.VBLF + "  AND ACTDATE >TO_DATE('" + argSysDate + "','YYYY-MM-DD')     ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검15쿼리
        public DataTable sel_DailyCheck_JumGum15(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT b.SNAME,a.TrsNo,a.IpdNo,a.PANO,                                    ";
            SQL += ComNum.VBLF + "TO_CHAR(INDATE,'YYYY-MM-DD') INDATE,TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS a, " + ComNum.DB_PMPA + "BAS_PATIENT b";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
            SQL += ComNum.VBLF + "  AND a.Pano=b.Pano                                                       ";
            SQL += ComNum.VBLF + "  AND OutDate = TO_DATE('" + argDate + "','YYYY-MM-DD')                   ";
            SQL += ComNum.VBLF + "AND ( GBIPD NOT IN ('D') OR GBIPD IS NULL )                               ";
            SQL += ComNum.VBLF + "GROUP BY b.SNAME,a.TrsNo,a.IpdNo,a.PANO,                                  ";
            SQL += ComNum.VBLF + "TO_CHAR(INDATE,'YYYY-MM-DD') ,TO_CHAR(OUTDATE,'YYYY-MM-DD')               ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검15_1쿼리
        public DataTable sel_DailyCheck_JumGum15_1(PsmhDb pDbCon, string argTRSNO, string argIPDNO, string argInDate, string argOutDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT SUM(AMT1+AMT2) TAMT                ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP    ";
            SQL += ComNum.VBLF + "WHERE 1=1                                 ";
            SQL += ComNum.VBLF + "  AND TRSNO = " + argTRSNO + "            ";
            SQL += ComNum.VBLF + "  AND IPDNO = " + argIPDNO + "             ";
            SQL += ComNum.VBLF + "  AND SUCODE NOT IN ('BBBBBB')            ";  //2011-07-06

            if (argInDate != "" && argOutDate != "")
            {
                SQL += ComNum.VBLF + "  AND ( BDATE < TO_DATE('" + argInDate + "','YYYY-MM-DD')     ";
                SQL += ComNum.VBLF + "  OR  BDATE > TO_DATE('" + argOutDate + "','YYYY-MM-DD') )    ";
            }
            else if (argInDate != "" && argOutDate == "")
            {
                SQL += ComNum.VBLF + "  AND  BDATE < TO_DATE('" + argInDate + "','YYYY-MM-DD')      ";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검15_2쿼리
        public DataTable sel_DailyCheck_JumGum15_2(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT Pano,trsno,bdate,sucode                            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                   ";
            SQL += ComNum.VBLF + "where 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND actdate =to_date('" + argDate + "','YYYY-MM-DD')    ";
            SQL += ComNum.VBLF + "  AND nal < 0                                             ";
            SQL += ComNum.VBLF + "GROUP BY Pano,trsno,bdate,sucode                          ";
            SQL += ComNum.VBLF + "Having Sum(Amt1 * Amt2) < 0                               ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검16쿼리
        public DataTable sel_DailyCheck_JumGum16(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT Pano,IPDNO                         ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS      ";
            SQL += ComNum.VBLF + "WHERE 1=1                                 ";
            SQL += ComNum.VBLF + "  AND ACTDATE IS NOT NULL                 ";
            SQL += ComNum.VBLF + "  AND ( OUTDATE IS NULL OR OUTDATE ='')   ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검16_1쿼리
        public DataTable sel_DailyCheck_JumGum16_1(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT Pano,IPDNO                                                             ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "  AND (ACTDATE IS NULL OR ACTDATE >=TO_DATE('" + argDate + "','YYYY-MM-DD') ) ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검16_2쿼리
        public DataTable sel_DailyCheck_JumGum16_2(PsmhDb pDbCon, string argPANO, string argIPDNO, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT Pano,IPDNO,TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
            SQL += ComNum.VBLF + "  AND Pano ='" + argPANO + "'                                                             ";
            SQL += ComNum.VBLF + "  AND IPDNO =" + argIPDNO + "                                                             ";
            SQL += ComNum.VBLF + "  AND TRUNC(BDATE) > TRUNC(ACTDATE)                                                       ";
            SQL += ComNum.VBLF + "  AND TRUNC(ENTDATE) =TO_DATE('" + argDate + "','YYYY-MM-DD')                             ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검16_3쿼리
        public DataTable sel_DailyCheck_JumGum16_3(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT a.Pano,a.IPDNO,TRUNC(a.INDATE),MIN(b.INDATE),MAX(b.OUTDATE),a.OUTDATE-MAX(b.OUTDATE)   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, " + ComNum.DB_PMPA + "IPD_TRANS b                  ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
            SQL += ComNum.VBLF + "  AND a.IPDNO = b.IPDNO                                                                       ";
            SQL += ComNum.VBLF + "  AND a.OUTDATE >=TRUNC(SYSDATE -10)                                                          ";
            SQL += ComNum.VBLF + "  AND a.GBSTS ='7'                                                                            ";
            SQL += ComNum.VBLF + "  AND b.GBIPD <>'D'                                                                           ";
            SQL += ComNum.VBLF + "  AND b.GBSTS <> '9'                                                                          ";
            SQL += ComNum.VBLF + "GROUP BY a.Pano,a.IPDNO,TRUNC(a.INDATE),a.OUTDATE                                             ";
            SQL += ComNum.VBLF + "Having a.OutDate - Max(b.OutDate) <> 0                                                        ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검17쿼리
        public DataTable sel_DailyCheck_JumGum17(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT Pano,IPDNO,SNAME,TO_CHAR(INDATE,'YYYY-MM-DD') INDATE   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                      ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "  AND ACTDATE IS NULL                                         ";
            SQL += ComNum.VBLF + "  AND TRUNC(InDate) =TO_DATE('" + argDate + "','YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "  AND TRUNC(InDate) =TRUNC(IpwonTime)                         ";
            SQL += ComNum.VBLF + "  AND ( OUTDATE IS NULL OR OUTDATE ='')                       ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검17_1쿼리
        public DataTable sel_DailyCheck_JumGum17_1(PsmhDb pDbCon, string argDate, string argPANO)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT PANO                                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND ACTDATE =TO_DATE('" + argDate + "','YYYY-MM-DD')    ";
            SQL += ComNum.VBLF + "  AND PANO ='" + argPANO + "'                             ";
            SQL += ComNum.VBLF + "  AND REP ='#'                                            ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검18쿼리
        public DataTable sel_DailyCheck_JumGum18(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT a.Pano,a.Sunext,c.SNAME                                            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH a, " + ComNum.DB_PMPA + "IPD_Trans b, " + ComNum.DB_PMPA + "IPD_NEW_MASTER c  ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
            SQL += ComNum.VBLF + "  AND a.TRSNO=b.TRSNO                                                     ";
            SQL += ComNum.VBLF + "  AND a.IPDNO =c.IPDNO                                                    ";
            SQL += ComNum.VBLF + "  AND a.ACTDATE =TO_DATE('" + argDate + "','YYYY-MM-DD')                  ";
            SQL += ComNum.VBLF + "  AND a.Bun ='92'                                                         ";
            SQL += ComNum.VBLF + "  AND a.SuNext <> 'Y92O'                                                  ";
            SQL += ComNum.VBLF + "AND b.GbGamek='55'                                                        ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검19쿼리
        public DataTable sel_DailyCheck_JumGum19(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT PANO,SUM(AMT) AMT                                  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND ActDate =TO_DATE('" + argDate + "','YYYY-MM-DD')    ";
            SQL += ComNum.VBLF + "  AND BUN IN  ('85','87','89')                            "; //입원은 보증금, 중간납, 퇴원금
            SQL += ComNum.VBLF + "  AND PART <> '#'                                         ";
            SQL += ComNum.VBLF + "GROUP BY PANO                                             ";
            SQL += ComNum.VBLF + "HAVING SUM(AMT) >= 300000                                 ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검19_1쿼리
        public DataTable sel_DailyCheck_JumGum19_1(PsmhDb pDbCon, string argDate, string argPANO)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT  SUM(DECODE(TranHeader,'2',TradeAmt * -1, TradeAmt)) CardAmt   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV                                ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                             ";
            SQL += ComNum.VBLF + "  AND ACTDATE =TO_DATE('" + argDate + "','YYYY-MM-DD')                ";
            SQL += ComNum.VBLF + "  AND PANO ='" + argPANO + "'                                         ";
            SQL += ComNum.VBLF + "  AND GUBUN  IN ('1','2')                                             "; // 현금, 카드 포함
            SQL += ComNum.VBLF + "  AND PTGUBUN IN ( '1','3')                                           "; // 코세스카드만
            SQL += ComNum.VBLF + "  AND GBIO ='I'                                                       "; //외래만
            SQL += ComNum.VBLF + "  AND PART <> '#'                                                     ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검20쿼리
        public DataTable sel_DailyCheck_JumGum20(PsmhDb pDbCon, string argDate, string argDateAdd)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT Bun,Nu                                                 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "  AND ACTDATE >=TO_DATE('" + argDateAdd + "','YYYY-MM-DD')    ";
            SQL += ComNum.VBLF + "  AND ACTDATE <=TO_DATE('" + argDate + "','YYYY-MM-DD')       ";
            SQL += ComNum.VBLF + "GROUP BY Bun,Nu                                               ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검21쿼리
        public DataTable sel_DailyCheck_JumGum21(PsmhDb pDbCon, string argDateAdd_1, string argDateAdd_2)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT Pano,Trsno,Ipdno                                       ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS                           ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "  AND EntDate >=TO_DATE('" + argDateAdd_1 + "','YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "  AND EntDate <TO_DATE('" + argDateAdd_2 + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "  AND GbIPD <> 'D'                                            ";
            SQL += ComNum.VBLF + "  AND Gbilban2 ='Y'                                           ";
            SQL += ComNum.VBLF + "  AND Bi <> '51'                                              ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검22쿼리
        public DataTable sel_DailyCheck_JumGum22(PsmhDb pDbCon, string argDate, string argDateAdd)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, DEPTCODE,BI,SNAME,PANO    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                             ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                             ";
            SQL += ComNum.VBLF + "  AND TRUNC(INDATE) >=TO_DATE('" + argDateAdd + "','YYYY-MM-DD')      ";
            SQL += ComNum.VBLF + "  AND TRUNC(INDATE) <=TO_DATE('" + argDate + "','YYYY-MM-DD')         ";
            SQL += ComNum.VBLF + "  AND GBSTS NOT IN ('9')                                              "; //취소제외

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검22_1쿼리
        public DataTable sel_DailyCheck_JumGum22_1(PsmhDb pDbCon, string argInDate, string argPANO, string argDeptCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT Pano,SName,Bi,Jin,MCode,DeptCode                                                       ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                         ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
            SQL += ComNum.VBLF + "  AND ActDate =TO_DATE('" + argInDate + "','YYYY-MM-DD')                                      ";
            SQL += ComNum.VBLF + "  AND PANO = '" + argPANO + "'                                                                ";
            SQL += ComNum.VBLF + "  AND DeptCode ='" + argDeptCode + "'                                                         ";
            SQL += ComNum.VBLF + "  AND ( BI IN ('21') OR ( MCode IN ('E000','F000') AND BI IN ('11','12','13')) OR Jin='2' )   ";
            SQL += ComNum.VBLF + "  AND Amt1 = 0                                                                                ";
            SQL += ComNum.VBLF + "  AND REP ='#'                                                                                ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검22_2쿼리
        public DataTable sel_DailyCheck_JumGum22_2(PsmhDb pDbCon, string argPano, string argInDate, string argDate, string argDeptCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT COUNT(*) CNT,SUM(AMT1) AMT                                         ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
            SQL += ComNum.VBLF + "  AND PANO = '" + argPano + "'                                             ";
            SQL += ComNum.VBLF + "  AND ACTDATE >= TO_DATE('" + argInDate + "','YYYY-MM-DD')                ";
            SQL += ComNum.VBLF + "  AND ACTDATE <= TO_DATE('" + argDate + "','YYYY-MM-DD')                  ";
            SQL += ComNum.VBLF + "  AND DEPTCODE = '" + argDeptCode + "'                                    ";
            SQL += ComNum.VBLF + "  AND SUNEXT IN (SELECT SUNEXT FROM BAS_ACCOUNT_CONV                      ";
            SQL += ComNum.VBLF + "                  WHERE GUBUN = '1'                                       ";
            SQL += ComNum.VBLF + "                  AND SDATE <=TO_DATE('" + argDate + "','YYYY-MM-DD'))    ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검22_3쿼리
        public DataTable sel_DailyCheck_JumGum22_3(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT Pano,SName,Bi,Jin,MCode,DeptCode                                                       ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                         ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
            SQL += ComNum.VBLF + "  AND ActDate =TO_DATE('" + argDate + "','YYYY-MM-DD')                                        ";
            SQL += ComNum.VBLF + "  AND DeptCode ='ER'                                                                          ";
            SQL += ComNum.VBLF + "  AND REP ='#'                                                                                ";
            SQL += ComNum.VBLF + "  AND ( BI IN ('21') OR ( MCode IN ('E000','F000') AND BI IN ('11','12','13')) OR Jin='2' )   ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검22_4쿼리
        public DataTable sel_DailyCheck_JumGum22_4(PsmhDb pDbCon, string argDateAdd, string argDate, string argPANO)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, DEPTCODE,BI,SNAME,PANO    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                             ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                             ";
            SQL += ComNum.VBLF + "  AND TRUNC(INDATE) >=TO_DATE('" + argDateAdd + "','YYYY-MM-DD')      ";
            SQL += ComNum.VBLF + "  AND TRUNC(INDATE) <=TO_DATE('" + argDate + "','YYYY-MM-DD')          ";
            SQL += ComNum.VBLF + "  AND PANO = '" + argPANO + "'                                        ";
            SQL += ComNum.VBLF + "  AND GBSTS NOT IN ('9')                                              "; // 취소제외

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검22_5쿼리
        public DataTable sel_DailyCheck_JumGum22_5(PsmhDb pDbCon, string argPano, string argDate, string argInDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs1 = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT COUNT(*) CNT,SUM(AMT1) AMT                                         ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
            SQL += ComNum.VBLF + "  AND PANO = '" + argPano + "'                                            ";
            SQL += ComNum.VBLF + "  AND ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD')                  ";
            SQL += ComNum.VBLF + "  AND ACTDATE <= TO_DATE('" + argInDate + "','YYYY-MM-DD')                ";
            SQL += ComNum.VBLF + "  AND SUNEXT IN (SELECT SUNEXT FROM BAS_ACCOUNT_CONV                      ";
            SQL += ComNum.VBLF + "                  WHERE GUBUN = '1'                                       ";
            SQL += ComNum.VBLF + "                  AND SDATE <=TO_DATE('" + argInDate + "','YYYY-MM-DD'))  ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs1, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs1;
        }
        #endregion

        #region 점검23쿼리
        public DataTable sel_DailyCheck_JumGum23(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT IPDNO,TRSNO, PANO,  TO_CHAR(INDATE,'YYYY-MM-DD') INDATE,AMT64  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS                                  ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                             ";
            SQL += ComNum.VBLF + "  AND OUTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')               ";
            SQL += ComNum.VBLF + "  AND Amt64 < 0                                                       ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검23_1쿼리
        public DataTable sel_DailyCheck_JumGum23_1(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt= null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT IPDNO,TRSNO, PANO,  TO_CHAR(INDATE,'YYYY-MM-DD') INDATE,AMT64  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS                                  ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                             ";
            SQL += ComNum.VBLF + "  AND OUTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')               ";
            SQL += ComNum.VBLF + "  AND GBIPD NOT IN ('D')                                              ";
            SQL += ComNum.VBLF + "  AND Amt64 > 0                                                       ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검23_2쿼리
        public DataTable sel_DailyCheck_JumGum23_2(PsmhDb pDbCon, string argTRSNO)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT  SUM(AMT1) AMT                     ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                 ";
            SQL += ComNum.VBLF + "  AND TRSNO = " + argTRSNO + "            ";
            SQL += ComNum.VBLF + "  AND SUCODE IN ('BBBBBB')                ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검23_3쿼리
        public DataTable sel_DailyCheck_JumGum23_3(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT TRSNO, PANO, SUM(AMT64) AMT64                      ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS                      ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "  AND GBIPD NOT IN ('D')                                  ";
            SQL += ComNum.VBLF + "GROUP BY TRSNO,PANO                                       ";
            SQL += ComNum.VBLF + "HAVING SUM(AMT64) <> 0                                    ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검23_4쿼리
        public DataTable sel_DailyCheck_JumGum23_4(PsmhDb pDbCon, string argTRSNO)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT AMT                                ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                 ";
            SQL += ComNum.VBLF + "  AND TRSNO = " + argTRSNO + "            ";
            SQL += ComNum.VBLF + "  AND SUNEXT ='Y98D'                      ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검23_5쿼리
        public DataTable sel_DailyCheck_JumGum23_5(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT TRSNO, PANO, SUM(AMT) AMT                          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "  AND SUNEXT IN ('Y98D')                                  ";
            SQL += ComNum.VBLF + "GROUP BY TRSNO,PANO                                       ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검23_6쿼리
        public DataTable sel_DailyCheck_JumGum23_6(PsmhDb pDbCon, string argTRSNO, int Gubun)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            if (Gubun == 0)
            {
                SQL += ComNum.VBLF + "SELECT  GBIPD, AMT64                  ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS  ";
                SQL += ComNum.VBLF + "WHERE 1=1                             ";
                SQL += ComNum.VBLF + "  AND TRSNO = " + argTRSNO + "        ";
            }
            else if (Gubun == 1)
            {
                SQL += ComNum.VBLF + "SELECT  SUM(AMT1) AMT_SLIP            ";
                SQL += ComNum.VBLF + "FROM IPD_NEW_SLIP                     ";
                SQL += ComNum.VBLF + "WHERE 1=1                             ";
                SQL += ComNum.VBLF + "  AND TRSNO = " + argTRSNO + "        ";
                SQL += ComNum.VBLF + "  AND SUNEXT IN ('BBBBBB')            ";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검24쿼리
        public DataTable sel_DailyCheck_JumGum24(PsmhDb pDbCon, string argDate, string argDateAdd)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT TO_CHAR(DATE1,'YYYY-MM-DD') DATE1,Pano,DeptCode,SName                          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW                                           ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                             ";
            SQL += ComNum.VBLF + "  AND DATE3 >= TO_DATE('" + argDate + "','YYYY-MM-DD')                                ";
            SQL += ComNum.VBLF + "  AND DATE3 < TO_DATE('" + argDateAdd + "','YYYY-MM-DD')                              ";
            SQL += ComNum.VBLF + "  AND RETDATE IS NULL                                                                 ";
            SQL += ComNum.VBLF + "  AND (PANO,DeptCode) IN ( SELECT PANO,DeptCode FROM OPD_MASTER                       ";
            SQL += ComNum.VBLF + "                            WHERE ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "                              AND RESERVED  = '1'                                     ";
            SQL += ComNum.VBLF + "                              AND BI <> '21'                                          ";
            SQL += ComNum.VBLF + "                              AND REP ='#')                                           ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검24_1쿼리
        public DataTable sel_DailyCheck_JumGum24_1(PsmhDb pDbCon, string argPANO, string argDeptCode, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT SUM(NAL) NAL                                   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP                    ";
            SQL += ComNum.VBLF + "WHERE 1=1                                             ";
            SQL += ComNum.VBLF + "  AND PANO = '" + argPANO + "'                        ";
            SQL += ComNum.VBLF + "  AND DEPTCODE = '" + argDeptCode + "'                ";
            SQL += ComNum.VBLF + "  AND BDATE = TO_DATE('" + argDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  AND SUNEXT = 'AA702'                                ";
            SQL += ComNum.VBLF + "  AND PART <> '#'                                     ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs;
        }
        #endregion

        #region 점검24_2쿼리
        public DataTable sel_DailyCheck_JumGum24_2(PsmhDb pDbCon, string argPANO, string argDeptCode, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable rs1 = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT SUM(NAL) NAL                                   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP               ";
            SQL += ComNum.VBLF + "WHERE 1=1                                             ";
            SQL += ComNum.VBLF + "  AND PANO = '" + argPANO + "'                        ";
            SQL += ComNum.VBLF + "  AND DEPTCODE = '" + argDeptCode + "'                ";
            SQL += ComNum.VBLF + "  AND BDATE = TO_DATE('" + argDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  AND SUNEXT = 'AA702'                                ";
            SQL += ComNum.VBLF + "  AND PART <> '#'                                     ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs1, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rs1;
        }
        #endregion

        #region 점검25쿼리
        public DataTable sel_DailyCheck_JumGum25(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT Pano,DeptCode,DrCode,IPDNO,bi                                          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "  AND (ACTDATE =TO_DATE('" + argDate + "','YYYY-MM-DD') OR ActDate IS NULL )  ";
            SQL += ComNum.VBLF + "  AND InDate >=TO_DATE('2011-06-01','YYYY-MM-DD')                             ";
            SQL += ComNum.VBLF + "  AND (GbSPC <>'1' OR GbSPC IS NULL )                                         ";
            SQL += ComNum.VBLF + "  AND GBSTS <> '9'                                                            ";
            SQL += ComNum.VBLF + "GROUP BY  Pano,DeptCode,DrCode,IPDNO,bi                                       ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검25_1쿼리
        public DataTable sel_DailyCheck_JumGum25_1(PsmhDb pDbCon, string argDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT Pano,DeptCode,DrCode,IPDNO,bi                                          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS                                          ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "  AND (ACTDATE =TO_DATE('" + argDate + "','YYYY-MM-DD') OR ActDate IS NULL )  ";
            SQL += ComNum.VBLF + "  AND InDate >=TO_DATE('2011-06-01','YYYY-MM-DD')                             ";
            SQL += ComNum.VBLF + "  AND (GbSPC <>'1' OR GbSPC IS NULL )                                         ";
            SQL += ComNum.VBLF + "  AND GBIPD <> 'D'                                                            ";
            SQL += ComNum.VBLF + "GROUP BY  Pano,DeptCode,DrCode,IPDNO,bi                                       ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검25_2쿼리
        public DataTable sel_DailyCheck_JumGum25_2(PsmhDb pDbCon, string argDateAdd)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT PANO,SNAME,BI,AGE,SEX,GBSTS,IPDNO,                 ";
            SQL += ComNum.VBLF + "  DEPTCODE,DRCODE,WARDCODE,ROOMCODE,                      ";
            SQL += ComNum.VBLF + "  TO_CHAR(INDATE,'YYYY-MM-DD') INDATE,                    ";
            SQL += ComNum.VBLF + "  TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE,                  ";
            SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE                   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                 ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND ACTDATE =TO_DATE('" + argDateAdd + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  AND (GbSPC ='1' OR GbSPC2 ='1' )                        ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검26쿼리
        public DataTable sel_DailyCheck_JumGum26(PsmhDb pDbCon, string argDate)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT a.Pano,a.SName,a.IPDNO,SUM(b.Amt) amt                                          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a ," + ComNum.DB_PMPA + "IPD_NEW_CASH b     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                             ";
            SQL += ComNum.VBLF + "  AND a.IPDNO=b.IPDNO                                                                 ";
            SQL += ComNum.VBLF + "  AND b.ACTDATE =TO_DATE('" + argDate + "','YYYY-MM-DD')                              ";
            SQL += ComNum.VBLF + "  AND b.Sunext ='Y96'                                                                 ";
            SQL += ComNum.VBLF + "GROUP BY  a.Pano,a.SName,a.IPDNO                                                      ";
            SQL += ComNum.VBLF + "HAVING SUM(b.Amt) <> 0                                                                ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검26_1쿼리
        public DataTable sel_DailyCheck_JumGum26_1(PsmhDb pDbCon, string argPANO, string argDate)
        {
            string SQL = ""; string SqlErr = ""; DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT SUM(AMT) MISUAMT                                   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP                  ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND PANO ='" + argPANO + "'                             ";
            SQL += ComNum.VBLF + "  AND BDATE =TO_DATE('" + argDate + "','YYYY-MM-DD')      ";
            SQL += ComNum.VBLF + "  AND GUBUN1 ='1'                                         "; //미수발생
            SQL += ComNum.VBLF + "  AND GUBUN2 ='3'                                         "; //입원발생

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return rs;
        }
        #endregion

        #region 점검27쿼리
        public DataTable sel_DailyCheck_JumGum27(PsmhDb pDbCon, string argDate)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT Pano,IPDNO,TRSNO,GbSTS                             ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS                       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND OUTDATE =TO_DATE('" + argDate + "','YYYY-MM-DD')    ";
            SQL += ComNum.VBLF + "  AND GbSTS ='5'                                          ";
            SQL += ComNum.VBLF + "  AND GBDRG ='D'                                          ";
            SQL += ComNum.VBLF + "ORDER BY Pano                                             ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region 점검27_1쿼리
        public DataTable sel_DailyCheck_JumGum27_1(PsmhDb pDbCon, string argPANO, string argDate, string argIPDNO, string argTRSNo)
        {
            string SQL = ""; string SqlErr = ""; DataTable rs = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT SUM(QTY*NAL) NQTY                                  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "  AND PANO ='" + argPANO + "'                             ";
            SQL += ComNum.VBLF + "  AND IPDNO = " + argIPDNO + "                            ";
            SQL += ComNum.VBLF + "  AND TRSNO = " + argTRSNo + "                            ";
            SQL += ComNum.VBLF + "  AND ACTDATE =TO_DATE('" + argDate + "','YYYY-MM-DD')    ";
            SQL += ComNum.VBLF + "  AND SUNEXT IN ('DRG001','DRG002')                       "; //미수발생

            try
            {
                SqlErr = clsDB.GetDataTable(ref rs, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return rs;
        }
        #endregion

        #region 메인 인서트 부분
        public string ins_DailyCheck_Insert(PsmhDb pDbCon, string argSaBun, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "INSERT INTO IPD_ILJI(JOBDATE, SABUN) VALUES ( ";
            SQL += ComNum.VBLF + "  SYSDATE , '" + argSaBun + "') ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    //ComFunc.MsgBox(SqlErr);
                    ComFunc.MsgBox("일일점검 테이블에 에러 발생함");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }
        #endregion

        #endregion

        #region frmPmpaMagamIpdMirCheckUpdate2.cs 쿼리 

        public DataTable sel_IpdMirCheckUpdate2_Check1(PsmhDb pDbCon, string argYYMM, string argYYMM2, string argJong, string argGubun, string argFDate, string argADDTDate)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;
            if (argGubun == "2")//중간청구
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                            ";
                SQL += ComNum.VBLF + "  Pano,Bi,'' ActDate,SName,'2' Gubun,                                             ";
                SQL += ComNum.VBLF + "  TO_CHAR(StartDate,'YYYY-MM-DD') InDate,YYMM,                                    ";
                SQL += ComNum.VBLF + "  TO_CHAR(EndDate,'YYYY-MM-DD') OutDate,DeptCode,BuildJAmt Johap,                 ";
                SQL += ComNum.VBLF + "  TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,WRTNO,JepJAmt,Remark,ROWID , '' BDATE     ";
                SQL += ComNum.VBLF + "FROM MIR_IPDID                                                                    ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                         ";
                SQL += ComNum.VBLF + "  AND YYMM>='200112'                                                              ";
                SQL += ComNum.VBLF + "  AND BuildDate>=TO_DATE('" + argFDate + "','YYYY-MM-DD')                         ";
                SQL += ComNum.VBLF + "  AND BuildDate <TO_DATE('" + argADDTDate + "','YYYY-MM-DD')                      ";
                SQL += ComNum.VBLF + "  AND Flag='1'                                                                    ";

                switch (argJong)
                {
                    case "1":
                        SQL += ComNum.VBLF + "  AND (SUBi ='1' AND BI IN ('11','12','13','44','32'))                    ";
                        break;
                    case "2":
                        SQL += ComNum.VBLF + "  AND (SUBI ='2' AND Bi IN ('21','22','23','24'))                         ";
                        break;
                    case "3":
                        SQL += ComNum.VBLF + "  AND (SUBI ='3' AND Bi IN ('31','32'))                                   ";
                        break;
                    case "4":
                        SQL += ComNum.VBLF + "  AND (SUBI ='4' AND Bi = '52' )                                          ";
                        break;
                }

                if (argJong == "3")
                {
                    SQL += ComNum.VBLF + "ORDER BY Bi,SNAME,Pano                                                        ";
                }
                else
                {
                    SQL += ComNum.VBLF + "ORDER BY Bi,Pano                                                              ";
                }
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                ";
                SQL += ComNum.VBLF + "  YYMM, Pano,Bi,SName, JOHAP, JepJAmt,Remark,ROWID ,  ";
                SQL += ComNum.VBLF + "    '' INDATE, BDATE                                  ";
                SQL += ComNum.VBLF + "FROM MISU_BALCHECK_PANO                               ";
                SQL += ComNum.VBLF + "WHERE 1=1                                             ";

                //if (argGubun =="1" || argGubun == "4")
                //{
                SQL += ComNum.VBLF + "  AND YYMM >= '" + argYYMM + "'                        ";
                SQL += ComNum.VBLF + "  AND YYMM <= '" + argYYMM2 + "'                       ";
                //}
                //else
                //{
                //    SQL += ComNum.VBLF + "  AND YYMM = '" + argYYMM + "'                        ";
                //}

                if (argGubun == "1")
                {
                    SQL += ComNum.VBLF + "  AND Gubun = '1'                                 "; //퇴원자
                }
                else if (argGubun == "2")
                {
                    SQL += ComNum.VBLF + "  AND Gubun = '2'                                 "; //중간청구
                }
                else if (argGubun == "3")
                {
                    SQL += ComNum.VBLF + "  AND Gubun = '3'                                 "; //응급실6시간이상, 정신과 낮병동
                }
                else if (argGubun == "4")
                {
                    SQL += ComNum.VBLF + "  AND Gubun = '4'                                 "; //외래
                }

                if (argJong != "0")
                {
                    SQL += ComNum.VBLF + "  AND SuBi = '" + argJong + "'                    ";
                }

                if (argJong == "3")
                {
                    SQL += ComNum.VBLF + "ORDER BY Bi,SNAME,Pano                            ";
                }
                else
                {
                    SQL += ComNum.VBLF + "ORDER BY Bi,Pano                                  ";
                }
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public string udt_IpdMirCheckUpdate2_update1(PsmhDb pDbCon, string argRemark, string argROWID, string argGnJobSabun, string argGuBun, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            if (argGuBun == "2")
            {
                SQL += ComNum.VBLF + "UPDATE MIR_IPDID SET ";
                SQL += ComNum.VBLF + "  Remark='" + argRemark + "'";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "  AND ROWID='" + argROWID + "' ";
            }
            else
            {
                SQL += ComNum.VBLF + "UPDATE MISU_BALCHECK_PANO SET ";
                SQL += ComNum.VBLF + "  Remark='" + argRemark + "',";
                SQL += ComNum.VBLF + "  EntDate=SYSDATE,EntSabun=" + clsType.User.IdNumber + " ";
                //SQL += ComNum.VBLF + "  EntDate=SYSDATE,EntSabun=" + "45316" + " ";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "  AND ROWID='" + argROWID + "' ";
            }

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }
        #endregion

        #region frmPmpaMagamMisuView.cs 쿼리
        public DataTable sel_VIEW04_Check1(PsmhDb pDbCon, string FDate, string TDate, string strYYMM, string strJong, string argSort, string argGubun, string argIOGubun)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                ";
            SQL += ComNum.VBLF + "  A.Class,TO_CHAR(B.BDate,'YYYY-MM-DD') BDate,A.MisuID,A.IpdOpd,A.Bun,";
            SQL += ComNum.VBLF + "  A.TongGbn, A.MirYYMM, SUM(AMT) NAMT, A.Remark,a.edimirno            ";
            SQL += ComNum.VBLF + "FROM MISU_IDMST A, MISU_SLIP B                                        ";

            if (argGubun == "0")
            {
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "  AND B.BDate>=TO_DATE('" + FDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND B.BDate<=TO_DATE('" + TDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND B.GUBUN IN ('11','12','13','14','15','16','17','18','19','20')";
            }
            else
            {
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "  AND A.MirYYMM = '" + strYYMM + "' ";
                SQL += ComNum.VBLF + "  AND B.GUBUN IN ('11','12','13','14','15','16','17','18','19','20')";
            }

            SQL += ComNum.VBLF + "  AND A.WRTNO = B.WRTNO ";

            switch (strJong)
            {
                case "0":
                    SQL += ComNum.VBLF + "  AND A.Class <= '07'";
                    break;
                case "1":
                    SQL += ComNum.VBLF + "  AND A.Class IN ('01','02','03') ";
                    break;
                case "2":
                    SQL += ComNum.VBLF + "  AND A.Class = '04' ";
                    break;
                case "3":
                    SQL += ComNum.VBLF + "  AND A.Class = '05' ";
                    break;
                case "4":
                    SQL += ComNum.VBLF + "  AND A.Class = '07' ";
                    break;
                default:
                    break;
            }

            if (argIOGubun == "0")
            {
                SQL += ComNum.VBLF + "  AND A.IpdOpd='O' ";
            }
            else if (argIOGubun == "1")
            {
                SQL += ComNum.VBLF + "  AND A.IpdOpd='I' ";
            }

            SQL += ComNum.VBLF + "GROUP BY A.CLASS, B.BDATE, A.MISUID, A.IPDOPD, A.BUN, A.TONGGBN, A.MIRYYMM, A.REMARK,a.edimirno";

            if (argSort == "1")
            {
                SQL += ComNum.VBLF + "ORDER BY a.edimirno,A.Class,A.IpdOpd,A.MisuID,B.BDate,a.edimirno ";
            }
            else
            {
                SQL += ComNum.VBLF + "ORDER BY a.edimirno,A.Class,B.BDate,A.IpdOpd, A.MisuID,a.edimirno ";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_VIEW04_Check2(PsmhDb pDbCon, string argedimirno)
        {
            string SQL = "";
            string SqlErr = "";        
            DataTable dt1 = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                 ";
            SQL += ComNum.VBLF + "  wrtno from mir_insid                 ";
            SQL += ComNum.VBLF + "where 1=1                              ";
            SQL += ComNum.VBLF + "  AND edimirno = '" + argedimirno + "' ";
            SQL += ComNum.VBLF + "  AND deptcode1 ='ER'                  ";
            SQL += ComNum.VBLF + "  AND upcnt1 <> '9'                    ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt1;
        }

        public DataTable sel_VIEW04_Check3(PsmhDb pDbCon, string argedimirno)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt1 = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                 ";
            SQL += ComNum.VBLF + "  DTHU from EDI_JEPSU                 ";
            SQL += ComNum.VBLF + "where 1=1                              ";
            SQL += ComNum.VBLF + "  AND MIRNO = '" + argedimirno + "' ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt1;
        }
        #endregion

        #region frmPmpaViewSmokeMisuInput.cs 쿼리
        public DataTable sel_SmokemisuInput_1(PsmhDb pDbCon, string TxtFDate, string TxtTDate,int chk, string Gubun, string TxtM)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT a.PANO, a.GUBUN1, a.GUBUN2,a.Amt, a.REMARK, b.SName,           ";
            SQL += ComNum.VBLF + "  SUBSTR(a.MISUDTL,1,1) OPDIPD, SUBSTR(a.MISUDTL,2,2) DEPTCODE, DECODE(SUBSTR(a.MISUDTL,4,2),'13','필수예방',SUBSTR(a.MISUDTL,4,2)) MISUDTL2, ";
            SQL += ComNum.VBLF + "  a.MISUDTL, SUBSTR(a.MISUDTL,4,2) MISUDTL22,                         ";
            SQL += ComNum.VBLF + "  a.IDNO, a.FLAG, a.PART, a.GRADE, A.SABUN, a.POBUN,                  ";
            SQL += ComNum.VBLF + "  TO_CHAR(a.BDATE, 'YYYY-MM-DD') BDATE, a.ROWID, A.Chk                ";
            SQL += ComNum.VBLF + "FROM MISU_GAINSLIP a, BAS_PATIENT b                                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                             ";
            SQL += ComNum.VBLF + "  AND a.PANO=b.PANO(+)                                                ";
            SQL += ComNum.VBLF + "  AND a.PANO IN (                                                     ";
            SQL += ComNum.VBLF + "                  SELECT Pano                                         ";
            SQL += ComNum.VBLF + "                  FROM MISU_GAINSLIP                                  ";
            SQL += ComNum.VBLF + "                  WHERE BDATE >=TO_DATE('2012-01-01','YYYY-MM-DD')    ";
            SQL += ComNum.VBLF + "                  AND SUBSTR(MISUDTL,4,2)='" + Gubun + "'                        ";
            SQL += ComNum.VBLF + "                  GROUP BY PANO                                       ";
            SQL += ComNum.VBLF + "                  HAVING SUM(DECODE(GUBUN1,'2', AMT *-1, AMT)) <> 0   ";
            SQL += ComNum.VBLF + "                 )                                                    ";
            SQL += ComNum.VBLF + "  AND a.BDATE>=TO_DATE('" + TxtFDate + "','YYYY-MM-DD')               ";
            SQL += ComNum.VBLF + "  AND a.BDATE<=TO_DATE('" + TxtTDate + "','YYYY-MM-DD')               ";
            SQL += ComNum.VBLF + "  AND SUBSTR(a.MISUDTL,4,2)='" + Gubun + "' ";

            if (TxtM != "")
            {
                SQL += ComNum.VBLF + "   AND a.REMARK LIKE   '%" + TxtM + "%' ";
            }
            if (chk == 1)
            {
                SQL += ComNum.VBLF + "  AND (a.Chk IS NULL OR a.Chk <> '1' )                            ";
                SQL += ComNum.VBLF + "  AND a.Gubun1 <> '2'                                             ";
            }
            SQL += ComNum.VBLF + "ORDER BY a.PANO, a.GUBUN1, a.BDATE                                    ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_SmokemisuInput_2(PsmhDb pDbCon, string ArgPano)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ROWID, MAmt, IAmt, JAmt    ";
            SQL += ComNum.VBLF + "FROM MISU_GAINMST                 ";
            SQL += ComNum.VBLF + "WHERE 1=1                         ";
            SQL += ComNum.VBLF + "  AND Pano = '" + ArgPano + "'    ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public string udt_SmokeMisuUpdate_1(PsmhDb pDbCon, string ArgRowid, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "UPDATE MISU_GAINSLIP SET          ";
            SQL += ComNum.VBLF + "       Chk = '1'                  ";
            SQL += ComNum.VBLF + "WHERE ROWID = '" + ArgRowid + "'  ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string udt_SmokeMisuUpdate_2(PsmhDb pDbCon, string ArgPano, double nAmt1, double nAmt2, string strROWID, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            if (strROWID == "")
            {
                SQL += ComNum.VBLF + "INSERT INTO MISU_GAINMST                          ";
                SQL += ComNum.VBLF + "       (Pano, MAmt, IAmt, JAmt)                   ";
                SQL += ComNum.VBLF + "VALUES                                            ";
                SQL += ComNum.VBLF + "       ('" + ArgPano + "', " + nAmt1 + ",         ";
                SQL += ComNum.VBLF + "        " + nAmt2 + ", " + (nAmt1 - nAmt2) + ")   ";
            }
            else
            {
                SQL += ComNum.VBLF + "UPDATE MISU_GAINMST                               ";
                SQL += ComNum.VBLF + "       SET MAmt = " + nAmt1 + ",                  ";
                SQL += ComNum.VBLF + "           IAmt = " + nAmt2 + ",                  ";
                SQL += ComNum.VBLF + "           JAmt = " + (nAmt1 - nAmt2) + "         ";
                SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'                  ";
            }

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_SmokeMisuInsert_1(PsmhDb pDbCon, string ArgPano, string ArgBuse, double ArgAmt, string ArgRemark, string strMisu, string strPobun, string Date, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "INSERT INTO MISU_GAINSLIP                                                     ";
            SQL += ComNum.VBLF + "       (Pano,Gubun1,Gubun2,Bdate,Amt,Remark,IDno,Flag,                        ";
            SQL += ComNum.VBLF + "        Part,MisuDtl,EntTime, CardSeqNo, SABUN, POBUN)                        ";
            SQL += ComNum.VBLF + " VALUES ('" + ArgPano + "', '2',                                              ";
            SQL += ComNum.VBLF + "        '" + ArgBuse + "',                                                    ";
            SQL += ComNum.VBLF + "       TO_DATE('" + Date + "','YYYY-MM-DD'),                                  ";
            SQL += ComNum.VBLF + "        " + ArgAmt + " , '" + ArgRemark + "',                                 ";
            SQL += ComNum.VBLF + "       '" + clsType.User.IdNumber + "', '0', '" + clsType.User.IdNumber + "', ";
            //SQL += ComNum.VBLF + "       '" +  + "', '0', '" + "45316" + "',                             ";
            SQL += ComNum.VBLF + "       '" + strMisu + "', SYSDATE,                                            ";
            SQL += ComNum.VBLF + "       '' ,                                                                   ";
            SQL += ComNum.VBLF + "       '','" + strPobun + "')                                                 ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string udt_SmokeMisuUpdate_3(PsmhDb pDbCon, string ArgPano, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "UPDATE MISU_GAINSLIP SET          ";
            SQL += ComNum.VBLF + "FLAG = '0'                        "; //입금표완성표시
            SQL += ComNum.VBLF + "WHERE Pano = '" + ArgPano + "'    ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }
        #endregion

        #region frmPmpaMisuMast1.cs 쿼리
        public string Del_MisuMast1_1(PsmhDb pDbcon, string argWRTNO, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA +"MISU_IDMST WHERE WRTNO = " + argWRTNO + " ";
            
            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MisuMast1_1(PsmhDb pDbcon,string argDate,string argTime, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA +"MISU_HISTORY                                                              ";
            SQL += ComNum.VBLF + "       (WRTNO, UpdateTime,UpdateTable,UpdateJob,                                                           ";
            SQL += ComNum.VBLF + "        UpdateSabun,MisuID, Bdate, GelCode,IpdOpd,Class,                                                   ";
            SQL += ComNum.VBLF + "        Gubun,EntDate,EntPart)                                                                             ";
            SQL += ComNum.VBLF + "VALUES                                                                                                     ";
            SQL += ComNum.VBLF + "        ('" + clsPmpaType.TMM.WRTNO + "',TO_DATE('" + argDate + " " + argTime + "','yyyy-mm-dd hh24:mi'),  ";
            SQL += ComNum.VBLF + "        'M','D', '" + clsType.User.IdNumber + "','" + clsPmpaType.TMM.MisuID + "',                         ";
            SQL += ComNum.VBLF + "        TO_DATE('" + clsPmpaType.TMM.BDate + "','YYYY-MM-DD'),'" + clsPmpaType.TMM.GelCode + "',           ";
            SQL += ComNum.VBLF + "        '" + clsPmpaType.TMM.IpdOpd + "','" + clsPmpaType.TMM.Class + "','" + clsPmpaType.TMM.Bun + "',    ";
            SQL += ComNum.VBLF + "        TO_DATE('" + clsPmpaType.TMM.EntDate + "','yyyy-mm-dd hh24:mi'),                                   ";
            SQL += ComNum.VBLF + "        " + clsPmpaType.TMM.EntPart + ")                                                                   ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public DataTable sel_MisuMast1_1(PsmhDb pDbCon, long argWRTNO)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT TO_CHAR(Bdate,'YYYY-MM-DD') Bdate,Gubun,Qty,TAmt,Amt,Remark,ROWID,     ";
            SQL += ComNum.VBLF + "       TO_CHAR(EntDate,'YYYY-MM-DD HH24:Mi') EntDate,EntPart, CHASU           ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_SLIP                                          ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "  AND WRTNO = " + argWRTNO +"                                                 ";
            SQL += ComNum.VBLF + "ORDER BY Bdate,Gubun                                                          ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_MisuMast1_2(PsmhDb pDbCon, long argWRTNO)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT YYMM,IwolAmt,MisuAmt,IpgumAmt,SakAmt,BanAmt,EtcAmt,JanAmt, Sakamt2 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_MONTHLY                                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
            SQL += ComNum.VBLF + "  AND WRTNO=" + argWRTNO + "                                              ";
            SQL += ComNum.VBLF + "ORDER BY YYMM                                                             ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_MisuMast1_CmdOK_IDMST_INSERT(PsmhDb pDbCon)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT MAX(WRTNO) mWRTNO FROM " + ComNum.DB_PMPA + "MISU_IDMST";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public string Int_MisuMast1_CmdOK_IDMST_INSERT(PsmhDb pDbCon, string argFromDate, string argToDate, ref int intRowAffected)
        {

            string SQL = "";
            string SqlErr = "";

            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "MISU_IDMST (WRTNO, MisuID, Bdate, Class,                                                                                                                    ";
            SQL += ComNum.VBLF + "       IpdOpd, GelCode, Bun,FromDate, ToDate, Ilsu, DeptCode,                                                                                                                                 ";
            SQL += ComNum.VBLF + "       MgrRank, Qty1, Qty2, Qty3, Qty4,                                                                                                                                                       ";
            SQL += ComNum.VBLF + "       Amt1, Amt2, Amt3, Amt4, Amt5, Amt6, Amt7, Amt8,                                                                                                                                        ";
            SQL += ComNum.VBLF + "       GbEnd, Remark,JepsuNo, TongGbn, MirYYMM, EntDate, EntPart)                                                                                                                             ";
            SQL += ComNum.VBLF + "VALUES                                                                                                                                                                                        ";
            SQL += ComNum.VBLF + "       ('" + clsPmpaType.TMN.WRTNO + "','" + clsPmpaType.TMN.MisuID.Trim() + "',TO_DATE('" + clsPmpaType.TMN.BDate + "','YYYY-MM-DD'),                                                        ";
            SQL += ComNum.VBLF + "       '" + clsPmpaType.TMN.Class + "', '" + clsPmpaType.TMN.IpdOpd.Trim() + "','" + clsPmpaType.TMN.GelCode.Trim() + "','" + clsPmpaType.TMN.Bun.Trim() + "',                                ";
            SQL += ComNum.VBLF + "       TO_DATE('" + argFromDate + "','YYYY-MM-DD'),TO_DATE('" + argToDate + "','YYYY-MM-DD'),'" + clsPmpaType.TMN.Ilsu + "',                                                                  ";
            SQL += ComNum.VBLF + "       '" + clsPmpaType.TMN.DeptCode + "', '" + clsPmpaType.TMN.MgrRank + "','" + clsPmpaType.TMN.Qty[0] + "','" + clsPmpaType.TMN.Qty[1] + "',                                               ";
            SQL += ComNum.VBLF + "       '" + clsPmpaType.TMN.Qty[2] + "','" + clsPmpaType.TMN.Qty[3] + "', '" + clsPmpaType.TMN.Amt[0] + "','" + clsPmpaType.TMN.Amt[1] + "',                                                  ";
            SQL += ComNum.VBLF + "       '" + clsPmpaType.TMN.Amt[2] + "','" + clsPmpaType.TMN.Amt[3] + "','" + clsPmpaType.TMN.Amt[4] + "','" + clsPmpaType.TMN.Amt[5] + "',                                                   ";
            SQL += ComNum.VBLF + "       '" + clsPmpaType.TMN.Amt[6] + "', '" + clsPmpaType.TMN.Amt[7] + "', '" + clsPmpaType.TMN.GbEnd + "','" + clsPmpaType.TMN.Remark.Trim() + "','" + clsPmpaType.TMN.JepsuNo.Trim() + "',  ";
            SQL += ComNum.VBLF + "       '" + clsPmpaType.TMN.TongGbn.Trim() + "','" + clsPmpaType.TMN.MirYYMM.Trim() + "',                                                                                                     ";
            SQL += ComNum.VBLF + "       SYSDATE,'" + clsType.User.IdNumber + "')                                                                                                                                               ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Upt_MisuMast1_CmdOK_IDMST_UPDATE(PsmhDb pDbcon,string argIdChange, string strFromDate, string strToDate, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            if (argIdChange == "OK")
            {
                //SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "MISU_IDMST SET                               ";
                SQL += ComNum.VBLF + "UPDATE MISU_IDMST SET                                                     ";
                SQL += ComNum.VBLF + "       MisuID = '" + clsPmpaType.TMN.MisuID.Trim() + "',                  ";
                SQL += ComNum.VBLF + "       Bdate = TO_DATE('" + clsPmpaType.TMN.BDate + "','YYYY-MM-DD'),     ";
                SQL += ComNum.VBLF + "       Class = '" + clsPmpaType.TMN.Class + "',                           ";
                SQL += ComNum.VBLF + "       IpdOpd= '" + clsPmpaType.TMN.IpdOpd.Trim() + "',                   ";
                SQL += ComNum.VBLF + "       Bi = '" + clsPmpaType.TMN.Bi + "',                                 ";
                SQL += ComNum.VBLF + "       GelCode='" + clsPmpaType.TMN.GelCode + "',                         ";
                SQL += ComNum.VBLF + "       Bun='" + clsPmpaType.TMN.Bun + "',                                 ";
                SQL += ComNum.VBLF + "       FromDate=TO_DATE('" + strFromDate + "','YYYY-MM-DD'),              ";
                SQL += ComNum.VBLF + "       ToDate=TO_DATE('" + strToDate + "','YYYY-MM-DD'),                  ";
                SQL += ComNum.VBLF + "       Ilsu='" + clsPmpaType.TMN.Ilsu + "',                               ";
                SQL += ComNum.VBLF + "       DeptCode='" + clsPmpaType.TMN.DeptCode + "',                       ";
                SQL += ComNum.VBLF + "       MgrRank='" + clsPmpaType.TMN.MgrRank + "',                         ";
                SQL += ComNum.VBLF + "       Qty1='" + clsPmpaType.TMN.Qty[0] + "',                             ";
                SQL += ComNum.VBLF + "       Qty2='" + clsPmpaType.TMN.Qty[1] + "',                             ";
                SQL += ComNum.VBLF + "       Qty3='" + clsPmpaType.TMN.Qty[2] + "',                             ";
                SQL += ComNum.VBLF + "       Qty4='" + clsPmpaType.TMN.Qty[3] + "',                             ";
                SQL += ComNum.VBLF + "       Amt1='" + clsPmpaType.TMN.Amt[0] + "',                             ";
                SQL += ComNum.VBLF + "       Amt2='" + clsPmpaType.TMN.Amt[1] + "',                             ";
                SQL += ComNum.VBLF + "       Amt3='" + clsPmpaType.TMN.Amt[2] + "',                             ";
                SQL += ComNum.VBLF + "       Amt4='" + clsPmpaType.TMN.Amt[3] + "',                             ";
                SQL += ComNum.VBLF + "       Amt5='" + clsPmpaType.TMN.Amt[4] + "',                             ";
                SQL += ComNum.VBLF + "       Amt6='" + clsPmpaType.TMN.Amt[5] + "',                             ";
                SQL += ComNum.VBLF + "       Amt7='" + clsPmpaType.TMN.Amt[6] + "',                             ";
                SQL += ComNum.VBLF + "       Amt8='" + clsPmpaType.TMN.Amt[7] + "',                             ";
                SQL += ComNum.VBLF + "       GbEnd='" + clsPmpaType.TMN.GbEnd + "',                             ";
                SQL += ComNum.VBLF + "       Remark='" + clsPmpaType.TMN.Remark.Trim() + "',                    ";
                SQL += ComNum.VBLF + "       EntDate= SYSDATE,                                                  ";
                SQL += ComNum.VBLF + "       TongGbn='" + clsPmpaType.TMN.TongGbn + "',                         ";
                SQL += ComNum.VBLF + "       MirYYMM='" + clsPmpaType.TMN.MirYYMM + "',                         ";
                SQL += ComNum.VBLF + "       JepsuNo='" + clsPmpaType.TMN.JepsuNo + "',                         ";
                SQL += ComNum.VBLF + "       EntPart='" + clsType.User.IdNumber + "'                            ";
                SQL += ComNum.VBLF + "WHERE WRTNO = '" + clsPmpaType.TMN.WRTNO +"'                              ";
            }
            else
            {
                //SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "MISU_IDMST SET";
                SQL += ComNum.VBLF + "UPDATE MISU_IDMST SET                                                     ";
                SQL += ComNum.VBLF + "       Qty1='" + clsPmpaType.TMN.Qty[0] + "',                             ";
                SQL += ComNum.VBLF + "       Qty2='" + clsPmpaType.TMN.Qty[1] + "',                             ";
                SQL += ComNum.VBLF + "       Qty3='" + clsPmpaType.TMN.Qty[2] + "',                             ";
                SQL += ComNum.VBLF + "       Qty4='" + clsPmpaType.TMN.Qty[3] + "',                             ";
                SQL += ComNum.VBLF + "       Amt1='" + clsPmpaType.TMN.Amt[0] + "',                             ";
                SQL += ComNum.VBLF + "       Amt2='" + clsPmpaType.TMN.Amt[1] + "',                             ";
                SQL += ComNum.VBLF + "       Amt3='" + clsPmpaType.TMN.Amt[2] + "',                             ";
                SQL += ComNum.VBLF + "       Amt4='" + clsPmpaType.TMN.Amt[3] + "',                             ";
                SQL += ComNum.VBLF + "       Amt5='" + clsPmpaType.TMN.Amt[4] + "',                             ";
                SQL += ComNum.VBLF + "       Amt6='" + clsPmpaType.TMN.Amt[5] + "',                             ";
                SQL += ComNum.VBLF + "       Amt7='" + clsPmpaType.TMN.Amt[6] + "',                             ";
                SQL += ComNum.VBLF + "       Amt8='" + clsPmpaType.TMN.Amt[7] + "',                             ";
                SQL += ComNum.VBLF + "       GbEnd='" + clsPmpaType.TMN.GbEnd + "'                              ";
                SQL += ComNum.VBLF + "WHERE WRTNO = '" + clsPmpaType.TMN.WRTNO + "'                             ";
            }

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Upt_MisuMast1_CmdOK_IDMST_UPDATE2(PsmhDb pDbcon, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "MISU_SLIP SET        ";
            SQL += ComNum.VBLF + "       Class='" + clsPmpaType.TMN.Class + "',     ";
            SQL += ComNum.VBLF + "       MisuID='" + clsPmpaType.TMN.MisuID + "',   ";
            SQL += ComNum.VBLF + "       IpdOpd='" + clsPmpaType.TMN.IpdOpd + "',   ";
            SQL += ComNum.VBLF + "       GelCode='" + clsPmpaType.TMN.GelCode + "'  ";
            SQL += ComNum.VBLF + "WHERE WRTNO = '" + clsPmpaType.TMN.WRTNO + "'     ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MisuMast1_CmdOK_IDMST_History(PsmhDb pDbCon, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "MISU_HISTORY                                                        ";
            SQL += ComNum.VBLF + "       (WRTNO, UpdateTime,UpdateTable,UpdateJob,                                                      ";
            SQL += ComNum.VBLF + "       UpdateSabun,MisuID, Bdate, GelCode,IpdOpd,Class,                                               ";
            SQL += ComNum.VBLF + "       Gubun,EntDate,EntPart)                                                                         ";
            SQL += ComNum.VBLF + "VALUES                                                                                                ";
            SQL += ComNum.VBLF + "       ('" + clsPmpaType.TMN.WRTNO + "', SYSDATE,                                                     ";
            SQL += ComNum.VBLF + "       'M','M', '" + clsType.User.IdNumber + "','" + clsPmpaType.TMM.MisuID + "',                     ";
            SQL += ComNum.VBLF + "       TO_DATE('" + clsPmpaType.TMM.BDate + "','YYYY-MM-DD'),'" + clsPmpaType.TMM.GelCode + "',       ";
            SQL += ComNum.VBLF + "       '" + clsPmpaType.TMM.IpdOpd + "','" + clsPmpaType.TMM.Class + "','" +clsPmpaType.TMM.Bun + "', ";
            SQL += ComNum.VBLF + "       SYSDATE," + clsPmpaType.TMM.EntPart + ")                                                       ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Del_MisuMast1_CmdOK_MisuSlip_Delete(PsmhDb pDbcon, string argROWID, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA +"MISU_SLIP WHERE ROWID = '" + argROWID + "' ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MisuMast1_CmdOK_MisuSlip_History(PsmhDb pDbCon, string argDate, string argTime, string argBdate2, string argGubun2, double argnQty2, double argnTAmt2, double argnAmt2, string argRemark2, string argEntDate2, long argnEntPart2, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "MISU_HISTORY (WRTNO, UpdateTime,UpdateTable,UpdateJob,                          ";
            SQL += ComNum.VBLF + "       UpdateSabun,MisuID, Bdate, GelCode,IpdOpd,Class,                                                           ";
            SQL += ComNum.VBLF + "       Gubun,Qty,TAmt,Amt,Remark,EntDate,EntPart)                                                                 ";
            SQL += ComNum.VBLF + "VALUES ('" + clsPmpaType.TMN.WRTNO + "',TO_DATE('" + argDate + " " + argTime + "','yyyy-mm-dd hh24:mi'),'S','D',  ";
            SQL += ComNum.VBLF + "       '" + clsType.User.IdNumber + "','" + clsPmpaType.TMM.MisuID + "',                                          ";
            SQL += ComNum.VBLF + "       TO_DATE('" + argBdate2 + "','YYYY-MM-DD'),'" + clsPmpaType.TMM.GelCode +"',                                ";
            SQL += ComNum.VBLF + "       '" + clsPmpaType.TMM.IpdOpd + "','" + clsPmpaType.TMM.Class + "','" + argGubun2 + "',                      ";
            SQL += ComNum.VBLF + "       "+ argnQty2 + "," + argnTAmt2 + "," + argnAmt2 + ",'" + argRemark2 + "',                                   ";
            SQL += ComNum.VBLF + "       TO_DATE('" + argEntDate2 + "','yyyy-mm-dd hh24:mi'),                                                       ";
            SQL += ComNum.VBLF + "       " + argnEntPart2 + ")                                                                                      ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MisuMast1_CmdOK_MisuSlip_Insert(PsmhDb pDbCon, string argBdate1, string argGubun1, double argnQty1, double argnTAmt1, double argnAmt1, string argRemark1, string argChasu1, string argDate, string argTime, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "MISU_SLIP (WRTNO,MisuID,Bdate,GelCode,IpdOpd,                                               ";
            SQL += ComNum.VBLF + "       Class,Gubun,Qty,TAmt,Amt,Remark,chasu, EntDate,EntPart)                                                                ";
            SQL += ComNum.VBLF + "VALUES                                                                                                                        ";
            SQL += ComNum.VBLF + "       ('" + clsPmpaType.TMN.WRTNO + "','" + clsPmpaType.TMN.MisuID.Trim() + "',TO_DATE('" + argBdate1 + "','YYYY-MM-DD'),    ";
            SQL += ComNum.VBLF + "       '" + clsPmpaType.TMN.GelCode.Trim() + "','" + clsPmpaType.TMN.IpdOpd.Trim() + "',                                      ";
            SQL += ComNum.VBLF + "       '" + clsPmpaType.TMN.Class + "','" + argGubun1.Trim() + "','" + argnQty1 + "','" + argnTAmt1 + "','" + argnAmt1 + "',  ";
            SQL += ComNum.VBLF + "       '" + argRemark1.Trim() + "', '" + argChasu1 + "', TO_DATE('" + argDate + " " + argTime + "','YYYY-MM-DD hh24:mi'),     ";
            SQL += ComNum.VBLF + "       '" + clsType.User.IdNumber + "')                                                                                       ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Upt_MisuMast1_CmdOK_MisuSlip_Update(PsmhDb pDbcon, string argBdate1, string argGubun1, double argnQty1, double argnTAmt1, double argnAmt1, string argChasu1, string argRemark1, string argDate, string argTime, string argROWID, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA+ "MISU_SLIP SET                                         ";
            SQL += ComNum.VBLF + "       MisuID = '" + clsPmpaType.TMN.MisuID.Trim() + "',                          ";
            SQL += ComNum.VBLF + "       Bdate = TO_DATE('" + argBdate1 + "','YYYY-MM-DD'),                         ";
            SQL += ComNum.VBLF + "       GelCode = '" + clsPmpaType.TMN.GelCode.Trim() + "',                        ";
            SQL += ComNum.VBLF + "       IpdOpd = '" + clsPmpaType.TMN.IpdOpd + "',                                 ";
            SQL += ComNum.VBLF + "       Class = '" + clsPmpaType.TMN.Class + "',                                   ";
            SQL += ComNum.VBLF + "       Gubun = '" + argGubun1 + "',                                               ";
            SQL += ComNum.VBLF + "       Qty    = '" + argnQty1 + "',                                               ";
            SQL += ComNum.VBLF + "       TAmt  = '" + argnTAmt1 + "',                                               ";
            SQL += ComNum.VBLF + "       Amt   = '" + argnAmt1 + "',                                                ";
            SQL += ComNum.VBLF + "       CHASU   = '" + argChasu1 + "',                                             ";
            SQL += ComNum.VBLF + "       Remark = '" + argRemark1.Trim() + "',                                      ";
            SQL += ComNum.VBLF + "       EntDate = TO_DATE('" + argDate + " " + argTime + "','YYYY-MM-DD hh24:mi'), ";
            SQL += ComNum.VBLF + "       EntPart = '" + clsType.User.IdNumber + "'                                  ";
            SQL += ComNum.VBLF + "WHERE ROWID = '" + argROWID + "'                                                  ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MisuMast1_CmdOK_MisuHISTORY_insert(PsmhDb pDbcon, string argDate, string argTime, string argBdate2, double argnQty2, double argnTAmt2,double argnAmt2, string argGubun2, string argRemark2, long argnEntPart2, string argEntDate2, ref int intRowAffected)
        {

            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "MISU_HISTORY                                                            ";
            SQL += ComNum.VBLF + "       (WRTNO, UpdateTime,UpdateTable,UpdateJob,                                                          ";
            SQL += ComNum.VBLF + "       UpdateSabun,MisuID, Bdate, GelCode,IpdOpd,Class,                                                   ";
            SQL += ComNum.VBLF + "       Gubun,Qty,TAmt,Amt,Remark,EntDate,EntPart)                                                         ";
            SQL += ComNum.VBLF + "VALUES                                                                                                    ";
            SQL += ComNum.VBLF + "       ('" + clsPmpaType.TMN.WRTNO + "',TO_DATE('" + argDate + " " + argTime + "','yyyy-mm-dd hh24:mi'),  ";
            SQL += ComNum.VBLF + "       'S','M', '" + clsType.User.IdNumber + "','" + clsPmpaType.TMM.MisuID + "',                         ";
            SQL += ComNum.VBLF + "       TO_DATE('" + argBdate2 + "','YYYY-MM-DD'),'" + clsPmpaType.TMM.GelCode + "',                       ";
            SQL += ComNum.VBLF + "       '" + clsPmpaType.TMM.IpdOpd + "','" + clsPmpaType.TMM.Class + "','" + argGubun2 + "',              ";
            SQL += ComNum.VBLF + "       " + argnQty2 + "," + argnTAmt2 + "," + argnAmt2 + ",'" + argRemark2 + "',                          ";
            SQL += ComNum.VBLF + "       TO_DATE('" + argEntDate2 + "','yyyy-mm-dd hh24:mi'),                                               ";
            SQL += ComNum.VBLF + "       " + argnEntPart2 + ")                                                                              ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MisuMast1_3(PsmhDb pDbcon, string argDate, string argTime, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "MISU_HISTORY                                                                             ";
            SQL += ComNum.VBLF + "       (WRTNO, UpdateTime,UpdateTable,UpdateJob,                                                                           ";
            SQL += ComNum.VBLF + "       UpdateSabun,MisuID, Bdate, GelCode,IpdOpd,Class,                                                                    ";
            SQL += ComNum.VBLF + "       Gubun,Qty,TAmt,Amt,Remark,EntDate,EntPart)                                                                          ";
            SQL += ComNum.VBLF + "SELECT '" + clsPmpaType.TMM.WRTNO + "',TO_DATE('" + argDate + " " + argTime + "','yyyy-mm-dd hh24:mi'),                    ";
            SQL += ComNum.VBLF + "       'S','D', '" + clsType.User.IdNumber + "','" + clsPmpaType.TMM.MisuID + "',Bdate,SUBSTR(GELCODE,1, 9),IpdOpd,Class,  ";
            SQL += ComNum.VBLF + "       Gubun,Qty,TAmt,Amt,Remark,EntDate,EntPart                                                                           ";
            SQL += ComNum.VBLF + "FROM MISU_SLIP                                                                                                             ";
            SQL += ComNum.VBLF + "WHERE WRTNO = " + clsPmpaType.TMM.WRTNO + "";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Del_MisuMast1_4(PsmhDb pDbcon, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "MISU_SLIP WHERE WRTNO = " + clsPmpaType.TMM.WRTNO + " ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        #endregion

        #region frmPmpaMisuMast2.cs 쿼리
        public string Del_MisuMast2_BtnDel_IDMST(PsmhDb pDbcon, long argWRTNO, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "MISU_IDMST WHERE WRTNO = " + argWRTNO + " ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MisuMast2_BtnDel_History(PsmhDb pDbcon, string argDate, string argTime, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "MISU_HISTORY                                                              ";
            SQL += ComNum.VBLF + "       (WRTNO, UpdateTime,UpdateTable,UpdateJob,                                                           ";
            SQL += ComNum.VBLF + "        UpdateSabun,MisuID, Bdate, GelCode,IpdOpd,Class,                                                   ";
            SQL += ComNum.VBLF + "        Gubun,EntDate,EntPart)                                                                             ";
            SQL += ComNum.VBLF + "VALUES                                                                                                     ";
            SQL += ComNum.VBLF + "        ('" + clsPmpaType.TMM.WRTNO + "',TO_DATE('" + argDate + " " + argTime + "','yyyy-mm-dd hh24:mi'),  ";
            SQL += ComNum.VBLF + "        'M','D', '" + clsType.User.IdNumber + "','" + clsPmpaType.TMM.MisuID + "',                         ";
            SQL += ComNum.VBLF + "        TO_DATE('" + clsPmpaType.TMM.BDate + "','YYYY-MM-DD'),'" + clsPmpaType.TMM.GelCode + "',           ";
            SQL += ComNum.VBLF + "        '" + clsPmpaType.TMM.IpdOpd + "','" + clsPmpaType.TMM.Class + "','" + clsPmpaType.TMM.Bun + "',    ";
            SQL += ComNum.VBLF + "        TO_DATE('" + clsPmpaType.TMM.EntDate + "','yyyy-mm-dd hh24:mi'),                                   ";
            SQL += ComNum.VBLF + "        " + clsPmpaType.TMM.EntPart + ")                                                                   ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MisuMast_BtnDel_SlipHistory(PsmhDb pDbcon, string argDate, string argTime, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "MISU_HISTORY                                                                    ";
            SQL += ComNum.VBLF + "       (WRTNO, UpdateTime,UpdateTable,UpdateJob,                                                                  ";
            SQL += ComNum.VBLF + "       UpdateSabun,MisuID, Bdate, GelCode,IpdOpd,Class,                                                           ";
            SQL += ComNum.VBLF + "       Gubun,EntDate,EntPart)                                                                                     ";
            SQL += ComNum.VBLF + "SELECT WRTNO,TO_DATE('" + argDate + " " + argTime + "','yyyy-mm-dd hh24:mi'),'S','D',                             ";
            SQL += ComNum.VBLF + "       '" + clsType.User.IdNumber + "','" + clsPmpaType.TMM.MisuID + "',Bdate,SUBSTR(GELCODE,1, 9),IpdOpd,Class,  ";
            SQL += ComNum.VBLF + "       Gubun,EntDate,EntPart                                                                                      ";
            SQL += ComNum.VBLF + "FROM MISU_SLIP                                                                                                    ";
            SQL += ComNum.VBLF + "WHERE WRTNO = " + clsPmpaType.TMM.WRTNO + "                                                                       ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Del_MisuMast2_BtnDel_Slip(PsmhDb pDbcon, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "MISU_SLIP WHERE WRTNO = " + clsPmpaType.TMM.WRTNO + " ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public DataTable Sel_MisuMast2_BtnOk_IDMST_INSERT(PsmhDb pDbCon)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT MAX(WRTNO) mWRTNO FROM MISU_IDMST";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public string Int_MisuMast2_BtnOk_IDMST_INSERT(PsmhDb pDbcon, string argFromDate, string argToDate, string argDate, string argTime, string argcboTong, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "INSERT INTO MISU_IDMST";
            SQL += ComNum.VBLF + "      (WRTNO, MisuID, Bdate, Class,";
            SQL += ComNum.VBLF + "      IpdOpd, GelCode, Bun,FromDate, ToDate, Ilsu, DeptCode,";
            SQL += ComNum.VBLF + "      MgrRank, Qty1, Qty2, Qty3, Qty4,";
            SQL += ComNum.VBLF + "      Amt1, Amt2, Amt3, Amt4, Amt5, Amt6, Amt7,";
            SQL += ComNum.VBLF + "      GbEnd, Remark, EntDate, EntPart,TongGbn,MirYYMM,DRCODE,TDATE, JDATE, CARNO, DRIVER, COPNAME )";
            SQL += ComNum.VBLF + "VALUES ";
            SQL += ComNum.VBLF + "      ('" + clsPmpaType.TMN.WRTNO + "','" + clsPmpaType.TMN.MisuID.Trim() +"',TO_DATE('" + clsPmpaType.TMN.BDate + "','YYYY-MM-DD'),";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMN.Class + "','" + clsPmpaType.TMN.IpdOpd.Trim() + "','" + clsPmpaType.TMN.GelCode.Trim() + "','" + clsPmpaType.TMN.Bun.Trim() + "',";
            SQL += ComNum.VBLF + "      TO_DATE('" + argFromDate + "','YYYY-MM-DD'),TO_DATE('" + argToDate + "','YYYY-MM-DD'),";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMN.Ilsu + "','" + clsPmpaType.TMN.DeptCode + "','" + clsPmpaType.TMN.MgrRank + "','" + clsPmpaType.TMN.Qty[1-1] + "',";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMN.Qty[2-1] + "','" + clsPmpaType.TMN.Qty[3-1] + "','" + clsPmpaType.TMN.Qty[4-1] + "',";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMN.Amt[1-1] + "','" + clsPmpaType.TMN.Amt[2-1] + "','" + clsPmpaType.TMN.Amt[3-1] + "','" + clsPmpaType.TMN.Amt[4-1] + "',";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMN.Amt[5-1] + "','" + clsPmpaType.TMN.Amt[6-1] + "','" + clsPmpaType.TMN.Amt[7-1] + "',";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMN.GbEnd + "','" + clsPmpaType.TMN.Remark + "',";
            SQL += ComNum.VBLF + "      TO_DATE('" + argDate + " " + VB.Left(argTime,5) + "','yyyy-mm-dd hh24:mi'),'" + clsType.User.IdNumber + "',";
            SQL += ComNum.VBLF + "      '" + argcboTong + "','" + clsPmpaType.TMN.MirYYMM + "', '" + clsPmpaType.TMN.DrCode + "', ";
            SQL += ComNum.VBLF + "      TO_DATE('" + clsPmpaType.TMN.TDATE + "','YYYY-MM-DD') , TO_DATE('" + clsPmpaType.TMN.JDATE + "','YYYY-MM-DD') , ";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMN.CARNO + "', '" + clsPmpaType.TMN.DRIVER + "', '" + clsPmpaType.TMN.COPNAME + "' )                  ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Upt_MisuMast2_BtnOk_IDMST_UPDATE(PsmhDb pDbcon, string argFromDate, string argToDate, string argDate, string argTime, string argCboTongGbn, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            SQL += ComNum.VBLF + "UPDATE MISU_IDMST SET ";
            SQL += ComNum.VBLF + "      MisuID='" + clsPmpaType.TMN.MisuID.Trim() + "',";
            SQL += ComNum.VBLF + "      Bdate=TO_DATE('" + clsPmpaType.TMN.BDate + "','YYYY-MM-DD'),";
            SQL += ComNum.VBLF + "      Class='" + clsPmpaType.TMN.Class + "',";
            SQL += ComNum.VBLF + "      IpdOpd='" + clsPmpaType.TMN.IpdOpd.Trim() + "',";
            SQL += ComNum.VBLF + "      Bi='" + clsPmpaType.TMN.Bi.Trim() + "', ";
            SQL += ComNum.VBLF + "      GelCode='" + clsPmpaType.TMN.GelCode.Trim() + "', ";
            SQL += ComNum.VBLF + "      Bun='" + clsPmpaType.TMN.Bun.Trim() + "', ";
            SQL += ComNum.VBLF + "      FromDate=TO_DATE('" + argFromDate + "','YYYY-MM-DD'), ";
            SQL += ComNum.VBLF + "      ToDate=TO_DATE('" + argToDate + "','YYYY-MM-DD'),";
            SQL += ComNum.VBLF + "      Ilsu='" + clsPmpaType.TMN.Ilsu + "',";
            SQL += ComNum.VBLF + "      DeptCode='" + clsPmpaType.TMN.DeptCode + "',";
            SQL += ComNum.VBLF + "      MgrRank='" + clsPmpaType.TMN.MgrRank + "', ";
            SQL += ComNum.VBLF + "      Qty1='" + clsPmpaType.TMN.Qty[1-1] + "', ";
            SQL += ComNum.VBLF + "      Qty2='" + clsPmpaType.TMN.Qty[2-1] + "',";
            SQL += ComNum.VBLF + "      Qty3='" + clsPmpaType.TMN.Qty[3-1] + "',";
            SQL += ComNum.VBLF + "      Qty4='" + clsPmpaType.TMN.Qty[4-1] + "',";
            SQL += ComNum.VBLF + "      Amt1='" + clsPmpaType.TMN.Amt[1-1] + "', ";
            SQL += ComNum.VBLF + "      Amt2='" + clsPmpaType.TMN.Amt[2-1] + "',";
            SQL += ComNum.VBLF + "      Amt3='" + clsPmpaType.TMN.Amt[3-1] + "',";
            SQL += ComNum.VBLF + "      Amt4='" + clsPmpaType.TMN.Amt[4-1] + "',";
            SQL += ComNum.VBLF + "      Amt5='" + clsPmpaType.TMN.Amt[5-1] + "',";
            SQL += ComNum.VBLF + "      Amt6='" + clsPmpaType.TMN.Amt[6-1] + "',";
            SQL += ComNum.VBLF + "      Amt7='" + clsPmpaType.TMN.Amt[7-1] + "',";
            SQL += ComNum.VBLF + "      GbEnd='" + clsPmpaType.TMN.GbEnd + "',";
            SQL += ComNum.VBLF + "      Remark='" + clsPmpaType.TMN.Remark + "', ";
            SQL += ComNum.VBLF + "      EntDate=TO_DATE('" + argDate + " " + VB.Left(argTime,5) + "','yyyy-mm-dd hh24:mi'), ";
            SQL += ComNum.VBLF + "      EntPart='" + clsType.User.IdNumber + "', ";
            SQL += ComNum.VBLF + "      MirYYMM='" + clsPmpaType.TMN.MirYYMM + "',";
            SQL += ComNum.VBLF + "      TongGbn='" + argCboTongGbn + "',";
            SQL += ComNum.VBLF + "      DrCode = '" + clsPmpaType.TMN.DrCode + "',";
            SQL += ComNum.VBLF + "      TDATE = TO_DATE('" + clsPmpaType.TMN.TDATE + "','YYYY-MM-DD'), ";
            SQL += ComNum.VBLF + "      JDATE = TO_DATE('" + clsPmpaType.TMN.JDATE + "','YYYY-MM-DD'), ";
            SQL += ComNum.VBLF + "      CARNO = '" + clsPmpaType.TMN.CARNO + "', ";
            SQL += ComNum.VBLF + "      DRIVER = '" + clsPmpaType.TMN.DRIVER + "', ";
            SQL += ComNum.VBLF + "      COPNAME= '" + clsPmpaType.TMN.COPNAME + "' ";
            SQL += ComNum.VBLF + "WHERE WRTNO = '" + clsPmpaType.TMN.WRTNO + "'";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Upt_MisuMast2_BtnOk_IDMST_UPDATE2(PsmhDb pDbcon, string argFromDate, string argToDate, string argCboTongGbn, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            SQL += ComNum.VBLF + "UPDATE MISU_IDMST SET ";
            SQL += ComNum.VBLF + "      Bun='" + clsPmpaType.TMN.Bun.Trim() + "', ";
            SQL += ComNum.VBLF + "      FromDate=TO_DATE('" + argFromDate + "','YYYY-MM-DD'), ";
            SQL += ComNum.VBLF + "      ToDate=TO_DATE('" + argToDate + "','YYYY-MM-DD'),";
            SQL += ComNum.VBLF + "      Ilsu='" + clsPmpaType.TMN.Ilsu + "',";
            SQL += ComNum.VBLF + "      DeptCode='" + clsPmpaType.TMN.DeptCode + "',";
            SQL += ComNum.VBLF + "      Remark='" + clsPmpaType.TMN.Remark + "',";
            SQL += ComNum.VBLF + "      Qty1='" + clsPmpaType.TMN.Qty[1 - 1] + "', ";
            SQL += ComNum.VBLF + "      Qty2='" + clsPmpaType.TMN.Qty[2 - 1] + "',";
            SQL += ComNum.VBLF + "      Qty3='" + clsPmpaType.TMN.Qty[3 - 1] + "',";
            SQL += ComNum.VBLF + "      Qty4='" + clsPmpaType.TMN.Qty[4 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt1='" + clsPmpaType.TMN.Amt[1 - 1] + "', ";
            SQL += ComNum.VBLF + "      Amt2='" + clsPmpaType.TMN.Amt[2 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt3='" + clsPmpaType.TMN.Amt[3 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt4='" + clsPmpaType.TMN.Amt[4 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt5='" + clsPmpaType.TMN.Amt[5 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt6='" + clsPmpaType.TMN.Amt[6 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt7='" + clsPmpaType.TMN.Amt[7 - 1] + "',";
            SQL += ComNum.VBLF + "      GbEnd='" + clsPmpaType.TMN.GbEnd + "',";
            SQL += ComNum.VBLF + "      TongGbn='" + argCboTongGbn + "',";
            SQL += ComNum.VBLF + "      DrCode = '" + clsPmpaType.TMN.DrCode + "',";
            SQL += ComNum.VBLF + "      TDATE = TO_DATE('" + clsPmpaType.TMN.TDATE + "','YYYY-MM-DD'), ";
            SQL += ComNum.VBLF + "      JDATE = TO_DATE('" + clsPmpaType.TMN.JDATE + "','YYYY-MM-DD'), ";
            SQL += ComNum.VBLF + "      CARNO = '" + clsPmpaType.TMN.CARNO + "', ";
            SQL += ComNum.VBLF + "      DRIVER = '" + clsPmpaType.TMN.DRIVER + "', ";
            SQL += ComNum.VBLF + "      COPNAME= '" + clsPmpaType.TMN.COPNAME + "' ";
            SQL += ComNum.VBLF + "WHERE WRTNO = '" + clsPmpaType.TMN.WRTNO + "'";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Upt_MisuMast2_BtnOk_SLIP_UPDATE(PsmhDb pDbcon, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            SQL += ComNum.VBLF + "UPDATE MISU_SLIP SET ";
            SQL += ComNum.VBLF + "      MisuID='" + clsPmpaType.TMN.MisuID + "',";
            SQL += ComNum.VBLF + "      IpdOpd='" + clsPmpaType.TMN.IpdOpd + "',";
            SQL += ComNum.VBLF + "      GelCode='" + clsPmpaType.TMN.GelCode + "',";
            SQL += ComNum.VBLF + "      Class='" + clsPmpaType.TMN.Class + "'";
            SQL += ComNum.VBLF + "WHERE WRTNO = '" + clsPmpaType.TMN.WRTNO + "'";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Upt_MisuMast2_BtnOk_HISTORY_UPDATE(PsmhDb pDbcon, string argDate, string  argTime, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            SQL += ComNum.VBLF + "INSERT INTO MISU_HISTORY ";
            SQL += ComNum.VBLF + "      (WRTNO, UpdateTime,UpdateTable,UpdateJob,";
            SQL += ComNum.VBLF + "      UpdateSabun,MisuID, Bdate, GelCode,IpdOpd,Class,";
            SQL += ComNum.VBLF + "      Gubun,EntDate,EntPart)";
            SQL += ComNum.VBLF + "VALUES";
            SQL += ComNum.VBLF + "      ('" + clsPmpaType.TMN.WRTNO + "',TO_DATE('" + argDate + " " + argTime + "','yyyy-mm-dd hh24:mi'),";
            SQL += ComNum.VBLF + "      'M','M', '" + clsType.User.IdNumber + "','" + clsPmpaType.TMM.MisuID + "',";
            SQL += ComNum.VBLF + "      TO_DATE('" + clsPmpaType.TMM.BDate + "','YYYY-MM-DD'),'" + clsPmpaType.TMM.GelCode + "',";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMM.IpdOpd + "','" + clsPmpaType.TMM.Class + "','" + clsPmpaType.TMM.Bun + "', ";
            SQL += ComNum.VBLF + "      TO_DATE('" + clsPmpaType.TMM.EntDate + "','yyyy-mm-dd hh24:mi'),";
            SQL += ComNum.VBLF + "      " + clsPmpaType.TMM.EntPart + ")";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Del_MisuMast2_BtnOK_Slip(PsmhDb pDbcon, string argROWID, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "MISU_SLIP WHERE ROWID = '" + argROWID + "' ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MisuMast2_BtnOK_HISTORY_INSERT(PsmhDb pDbcon, string argDate, string argTime, string argBdate2, string argGubun2, double argnQty2, double argnTAmt2, double argnAmt2, string argRemark2, string argEntDate2, long argnEntPart2, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "INSERT INTO MISU_HISTORY (WRTNO, UpdateTime,UpdateTable,UpdateJob,";
            SQL += ComNum.VBLF + "      UpdateSabun,MisuID, Bdate, GelCode,IpdOpd,Class,";
            SQL += ComNum.VBLF + "      Gubun,Qty,TAmt,Amt,Remark,EntDate,EntPart)";
            SQL += ComNum.VBLF + "VALUES";
            SQL += ComNum.VBLF + "      ('" + clsPmpaType.TMN.WRTNO + "',TO_DATE('" + argDate + " " + VB.Left(argTime,5) + "','yyyy-mm-dd hh24:mi'),";
            SQL += ComNum.VBLF + "      'S','D','" + clsType.User.IdNumber + "','" + clsPmpaType.TMM.MisuID + "', ";
            SQL += ComNum.VBLF + "      TO_DATE('" + argBdate2 + "','YYYY-MM-DD'),'" + clsPmpaType.TMM.GelCode + "',";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMM.IpdOpd + "','" + clsPmpaType.TMM.Class + "','" + argGubun2 + "',";
            SQL += ComNum.VBLF + "      " + argnQty2 + "," + argnTAmt2 + "," + argnAmt2 + ",'" + argRemark2 + "',";
            SQL += ComNum.VBLF + "      TO_DATE('" + argEntDate2 + "','yyyy-mm-dd hh24:mi'),";
            SQL += ComNum.VBLF + "      " + argnEntPart2 + ")";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MisuMast2_BtnOK_MisuSlip_INSERT(PsmhDb pDbcon, string argGubun1, double argnQty1, double argnTAmt1, double argnAmt1, string argBdate1,string argRemark1, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            SQL += ComNum.VBLF + "INSERT INTO MISU_SLIP ";
            SQL += ComNum.VBLF + "      (WRTNO,MisuID,Bdate,GelCode,IpdOpd,";
            SQL += ComNum.VBLF + "      Class,Gubun,Qty,TAmt,Amt,Remark,EntDate,EntPart) ";
            SQL += ComNum.VBLF + "VALUES";
            SQL += ComNum.VBLF + "      ('" + clsPmpaType.TMN.WRTNO + "','" + clsPmpaType.TMN.MisuID.Trim() + "',TO_DATE('" + argBdate1 + "','YYYY-MM-DD'),";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMN.GelCode.Trim() + "','" + clsPmpaType.TMN.IpdOpd.Trim() + "',";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMN.Class + "','" + argGubun1 + "'," + argnQty1 + "," + argnTAmt1 + "," + argnAmt1 + ",";
            SQL += ComNum.VBLF + "      '" + argRemark1 + "',SYSDATE, ";
            SQL += ComNum.VBLF + "      '" + clsType.User.IdNumber + "')";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Upt_MisuMast2_BtnOk_MisuSlip_UPDATE(PsmhDb pDbcon,string argBdate1, string argGubun1, double argnQty1, double argnTAmt1, double argnAmt1, string argRemark1,string argROWID , ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            SQL += ComNum.VBLF + "UPDATE MISU_SLIP SET";
            SQL += ComNum.VBLF + "      MisuID = '" + clsPmpaType.TMN.MisuID.Trim() + "',";
            SQL += ComNum.VBLF + "      Bdate = TO_DATE('" + argBdate1 + "','YYYY-MM-DD'),";
            SQL += ComNum.VBLF + "      GelCode = '" + clsPmpaType.TMN.GelCode.Trim() + "',";
            SQL += ComNum.VBLF + "      IpdOpd = '" + clsPmpaType.TMN.IpdOpd.Trim() + "',";
            SQL += ComNum.VBLF + "      Class = '" + clsPmpaType.TMN.Class + "',";
            SQL += ComNum.VBLF + "      Gubun = '" + argGubun1 + "',";
            SQL += ComNum.VBLF + "      Qty    = " + argnQty1 + ",";
            SQL += ComNum.VBLF + "      TAmt  = " + argnTAmt1 + ", ";
            SQL += ComNum.VBLF + "      Amt   = " + argnAmt1 + ",";
            SQL += ComNum.VBLF + "      Remark = '" + argRemark1 + "',";
            SQL += ComNum.VBLF + "      EntDate = SYSDATE , ";
            SQL += ComNum.VBLF + "      EntPart = '" + clsType.User.IdNumber + "'";
            SQL += ComNum.VBLF + "WHERE ROWID = '" + argROWID + "'";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MisuMast2_BtnOK_MisuHISTOTY_INSERT(PsmhDb pDbcon, string argDate, string argTime, string argBdate2, string argGubun2, double argnQty2, double argnTAmt2, double argnAmt2, string argRemark2, string argEntDate2, long argnEntPart2, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            SQL += ComNum.VBLF + " INSERT INTO MISU_HISTORY                                                                              ";
            SQL += ComNum.VBLF + "        (WRTNO, UpdateTime,UpdateTable,UpdateJob,                                                      ";
            SQL += ComNum.VBLF + "        UpdateSabun,MisuID, Bdate, GelCode,IpdOpd,Class,                                               ";
            SQL += ComNum.VBLF + "        Gubun,Qty,TAmt,Amt,Remark,EntDate,EntPart)                                                     ";
            SQL += ComNum.VBLF + " VALUES                                                                                                ";
            SQL += ComNum.VBLF + "        ('" + clsPmpaType.TMN.WRTNO + "',TO_DATE('" + argDate + " " + argTime + "','yyyy-mm-dd hh24:mi'),  ";
            SQL += ComNum.VBLF + "        'S','M', '" + clsType.User.IdNumber + "','" + clsPmpaType.TMM.MisuID + "',                                            ";
            SQL += ComNum.VBLF + "        TO_DATE('" + argBdate2 + "','YYYY-MM-DD'),'" + clsPmpaType.TMM.GelCode + "',                               ";
            SQL += ComNum.VBLF + "        '" + clsPmpaType.TMM.IpdOpd + "','" + clsPmpaType.TMM.Class + "','" + argGubun2 + "',                                  ";
            SQL += ComNum.VBLF + "        " + argnQty2 + "," + argnTAmt2 + "," + argnAmt2 + ",'" + argRemark2 + "',                               ";
            SQL += ComNum.VBLF + "        TO_DATE('" + argEntDate2 + "','yyyy-mm-dd hh24:mi'),                                           ";
            SQL += ComNum.VBLF + "        " + argnEntPart2 + ")                                                                             ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public DataTable Sel_MisuMast2_DISPLAY_SELECT1(PsmhDb pDbCon)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT DRCODE,DRNAME";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
            SQL += ComNum.VBLF + "WHERE TOUR ='N'";
            if(clsPmpaType.TMM.DeptCode != "ER")
            {
                SQL += ComNum.VBLF + "AND DRDEPT1 = '" + clsPmpaType.TMM.DeptCode + "'";
            }
            SQL += ComNum.VBLF + "ORDER BY DRCODE";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable Sel_MisuMast2_DISPLAY_SELECT2(PsmhDb pDbCon)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT DRNAME";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
            SQL += ComNum.VBLF + "WHERE DRCODE = '" +clsPmpaType.TMM.DrCode + "'";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable Sel_MisuMast2_DISPLAY_SELECT3(PsmhDb pDbCon, long argGnWRTNO)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(Bdate,'YYYY-MM-DD') Bdate,Gubun,Qty,Amt,Remark,ROWID,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EntDate,'YYYY-MM-DD HH24:Mi') EntDate,EntPart          ";
            SQL += ComNum.VBLF + "   FROM MISU_SLIP                                                      ";
            SQL += ComNum.VBLF + "  WHERE WRTNO = " + argGnWRTNO + "                                     ";
            SQL += ComNum.VBLF + "  ORDER BY Bdate,Gubun                                                 ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion

        #region frmPmpaMisuMast.cs 쿼리
        public string Del_MISU_IDMST_CmdDelete(PsmhDb pDbcon,long argWRTNO , ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "MISU_IDMST WHERE WRTNO = " + argWRTNO + " ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MISU_HISTORY_CmdDelete(PsmhDb pDbcon, string argDate, string argTime, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "MISU_HISTORY                                                              ";
            SQL += ComNum.VBLF + "       (WRTNO, UpdateTime,UpdateTable,UpdateJob,                                                           ";
            SQL += ComNum.VBLF + "        UpdateSabun,MisuID, Bdate, GelCode,IpdOpd,Class,                                                   ";
            SQL += ComNum.VBLF + "        Gubun,EntDate,EntPart)                                                                             ";
            SQL += ComNum.VBLF + "VALUES                                                                                                     ";
            SQL += ComNum.VBLF + "        ('" + clsPmpaType.TMM.WRTNO + "',TO_DATE('" + argDate + " " + argTime + "','yyyy-mm-dd hh24:mi'),  ";
            SQL += ComNum.VBLF + "        'M','D', '" + clsType.User.IdNumber + "','" + clsPmpaType.TMM.MisuID + "',                         ";
            SQL += ComNum.VBLF + "        TO_DATE('" + clsPmpaType.TMM.BDate + "','YYYY-MM-DD'),'" + clsPmpaType.TMM.GelCode + "',           ";
            SQL += ComNum.VBLF + "        '" + clsPmpaType.TMM.IpdOpd + "','" + clsPmpaType.TMM.Class + "','" + clsPmpaType.TMM.Bun + "',    ";
            SQL += ComNum.VBLF + "        TO_DATE('" + clsPmpaType.TMM.EntDate + "','yyyy-mm-dd hh24:mi'),                                   ";
            SQL += ComNum.VBLF + "        " + clsPmpaType.TMM.EntPart + ")                                                                   ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MISU_HISTORY_CmdDelete1(PsmhDb pDbcon, string argDate, string argTime, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "MISU_HISTORY                                                                    ";
            SQL += ComNum.VBLF + "       (WRTNO, UpdateTime,UpdateTable,UpdateJob,                                                                  ";
            SQL += ComNum.VBLF + "       UpdateSabun,MisuID, Bdate, GelCode,IpdOpd,Class,                                                           ";
            SQL += ComNum.VBLF + "       Gubun,EntDate,EntPart)                                                                                     ";
            SQL += ComNum.VBLF + "SELECT WRTNO,TO_DATE('" + argDate + " " + argTime + "','yyyy-mm-dd hh24:mi'),'S','D',                             ";
            SQL += ComNum.VBLF + "       '" + clsType.User.IdNumber + "','" + clsPmpaType.TMM.MisuID + "',Bdate,SUBSTR(GELCODE,1, 9),IpdOpd,Class,  ";
            SQL += ComNum.VBLF + "       Gubun,EntDate,EntPart                                                                                      ";
            SQL += ComNum.VBLF + "FROM MISU_SLIP                                                                                                    ";
            SQL += ComNum.VBLF + "WHERE WRTNO = " + clsPmpaType.TMM.WRTNO + "                                                                       ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Del_MISU_SLIP_CmdDelete(PsmhDb pDbcon, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "MISU_SLIP WHERE WRTNO = " + clsPmpaType.TMM.WRTNO + " ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public DataTable Sel_MISU_IDMST_CmdOK_IDMST_INSERT(PsmhDb pDbCon)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT MAX(WRTNO) mWRTNO FROM " + ComNum.DB_PMPA+ "MISU_IDMST";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public string Int_MISU_IDMST_CmdOK_IDMST_INSERT(PsmhDb pDbcon, string argFromDate, string argToDate, string argDate, string argTime, string argJiGubun, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "INSERT INTO MISU_IDMST";
            SQL += ComNum.VBLF + "      (WRTNO, MisuID, Bdate, Class,";
            SQL += ComNum.VBLF + "      IpdOpd, GelCode, Bun,FromDate, ToDate, Ilsu, DeptCode,";
            SQL += ComNum.VBLF + "      MgrRank, Qty1, Qty2, Qty3, Qty4,";
            SQL += ComNum.VBLF + "      Amt1, Amt2, Amt3, Amt4, Amt5, Amt6, Amt7,";
            SQL += ComNum.VBLF + "      GbEnd, Remark, EntDate, EntPart,MirYYMM,Gubun)";
            SQL += ComNum.VBLF + "VALUES ";
            SQL += ComNum.VBLF + "      ('" + clsPmpaType.TMN.WRTNO + "','" + clsPmpaType.TMN.MisuID.Trim() + "',TO_DATE('" + clsPmpaType.TMN.BDate + "','YYYY-MM-DD'),";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMN.Class + "','" + clsPmpaType.TMN.IpdOpd.Trim() + "','" + clsPmpaType.TMN.GelCode.Trim() + "','" + clsPmpaType.TMN.Bun.Trim() + "',";
            SQL += ComNum.VBLF + "      TO_DATE('" + argFromDate + "','YYYY-MM-DD'),TO_DATE('" + argToDate + "','YYYY-MM-DD'),";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMN.Ilsu + "','" + clsPmpaType.TMN.DeptCode + "','" + clsPmpaType.TMN.MgrRank + "','" + clsPmpaType.TMN.Qty[0] + "',";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMN.Qty[1] + "','" + clsPmpaType.TMN.Qty[2] + "','" + clsPmpaType.TMN.Qty[3] + "'," + clsPmpaType.TMN.Amt[0] + "," + clsPmpaType.TMN.Amt[1] + ",";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMN.Amt[2] + "','" + clsPmpaType.TMN.Amt[3] + "','" + clsPmpaType.TMN.Amt[4] + "','" + clsPmpaType.TMN.Amt[5] + "'," + clsPmpaType.TMN.Amt[6] +",";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMN.GbEnd + "','" + clsPmpaType.TMN.Remark + "',";
            SQL += ComNum.VBLF + "      TO_DATE('" + argDate + " " + VB.Left(argTime, 5) + "','yyyy-mm-dd hh24:mi'),";
            SQL += ComNum.VBLF + "      '" + clsType.User.IdNumber + "','" + clsPmpaType.TMN.MirYYMM + "', '" + argJiGubun + "') ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Upt_MISU_IDMST_CmdOK_IDMST_UPDATE(PsmhDb pDbcon, string argFromDate, string argToDate, string argDate, string argTime, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            SQL += ComNum.VBLF + "UPDATE MISU_IDMST SET ";
            SQL += ComNum.VBLF + "      MisuID='" + clsPmpaType.TMN.MisuID.Trim() + "',";
            SQL += ComNum.VBLF + "      Bdate=TO_DATE('" + clsPmpaType.TMN.BDate + "','YYYY-MM-DD'),";
            SQL += ComNum.VBLF + "      Class='" + clsPmpaType.TMN.Class + "',";
            SQL += ComNum.VBLF + "      IpdOpd='" + clsPmpaType.TMN.IpdOpd.Trim() + "',";
            SQL += ComNum.VBLF + "      Bi='" + clsPmpaType.TMN.Bi.Trim() + "', ";
            SQL += ComNum.VBLF + "      GelCode='" + clsPmpaType.TMN.GelCode.Trim() + "', ";
            SQL += ComNum.VBLF + "      Bun='" + clsPmpaType.TMN.Bun.Trim() + "', ";
            SQL += ComNum.VBLF + "      FromDate=TO_DATE('" + argFromDate + "','YYYY-MM-DD'), ";
            SQL += ComNum.VBLF + "      ToDate=TO_DATE('" + argToDate + "','YYYY-MM-DD'),";
            SQL += ComNum.VBLF + "      Ilsu='" + clsPmpaType.TMN.Ilsu + "',";
            SQL += ComNum.VBLF + "      DeptCode='" + clsPmpaType.TMN.DeptCode + "',";
            SQL += ComNum.VBLF + "      MgrRank='" + clsPmpaType.TMN.MgrRank + "', ";
            SQL += ComNum.VBLF + "      Qty1='" + clsPmpaType.TMN.Qty[1 - 1] + "', ";
            SQL += ComNum.VBLF + "      Qty2='" + clsPmpaType.TMN.Qty[2 - 1] + "',";
            SQL += ComNum.VBLF + "      Qty3='" + clsPmpaType.TMN.Qty[3 - 1] + "',";
            SQL += ComNum.VBLF + "      Qty4='" + clsPmpaType.TMN.Qty[4 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt1='" + clsPmpaType.TMN.Amt[1 - 1] + "', ";
            SQL += ComNum.VBLF + "      Amt2='" + clsPmpaType.TMN.Amt[2 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt3='" + clsPmpaType.TMN.Amt[3 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt4='" + clsPmpaType.TMN.Amt[4 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt5='" + clsPmpaType.TMN.Amt[5 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt6='" + clsPmpaType.TMN.Amt[6 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt7='" + clsPmpaType.TMN.Amt[7 - 1] + "',";
            SQL += ComNum.VBLF + "      GbEnd='" + clsPmpaType.TMN.GbEnd + "',";
            SQL += ComNum.VBLF + "      Remark='" + clsPmpaType.TMN.Remark + "', ";
            SQL += ComNum.VBLF + "      EntDate=TO_DATE('" + argDate + " " + VB.Left(argTime, 5) + "','yyyy-mm-dd hh24:mi'), ";
            SQL += ComNum.VBLF + "      EntPart='" + clsType.User.IdNumber + "', ";
            SQL += ComNum.VBLF + "      MirYYMM='" + clsPmpaType.TMN.MirYYMM + "'";
            SQL += ComNum.VBLF + "WHERE WRTNO = '" + clsPmpaType.TMN.WRTNO + "'";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Upt_MISU_IDMST_CmdOK_IDMST_UPDATE1(PsmhDb pDbcon, string argFromDate, string argToDate, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            SQL += ComNum.VBLF + "UPDATE MISU_IDMST SET ";
            SQL += ComNum.VBLF + "      Bun='" + clsPmpaType.TMN.Bun.Trim() + "', ";
            SQL += ComNum.VBLF + "      FromDate=TO_DATE('" + argFromDate + "','YYYY-MM-DD'), ";
            SQL += ComNum.VBLF + "      ToDate=TO_DATE('" + argToDate + "','YYYY-MM-DD'),";
            SQL += ComNum.VBLF + "      Ilsu='" + clsPmpaType.TMN.Ilsu + "',";
            SQL += ComNum.VBLF + "      DeptCode='" + clsPmpaType.TMN.DeptCode + "',";
            SQL += ComNum.VBLF + "      Remark='" + clsPmpaType.TMN.Remark + "',";
            SQL += ComNum.VBLF + "      Qty1='" + clsPmpaType.TMN.Qty[1 - 1] + "', ";
            SQL += ComNum.VBLF + "      Qty2='" + clsPmpaType.TMN.Qty[2 - 1] + "',";
            SQL += ComNum.VBLF + "      Qty3='" + clsPmpaType.TMN.Qty[3 - 1] + "',";
            SQL += ComNum.VBLF + "      Qty4='" + clsPmpaType.TMN.Qty[4 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt1='" + clsPmpaType.TMN.Amt[1 - 1] + "', ";
            SQL += ComNum.VBLF + "      Amt2='" + clsPmpaType.TMN.Amt[2 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt3='" + clsPmpaType.TMN.Amt[3 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt4='" + clsPmpaType.TMN.Amt[4 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt5='" + clsPmpaType.TMN.Amt[5 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt6='" + clsPmpaType.TMN.Amt[6 - 1] + "',";
            SQL += ComNum.VBLF + "      Amt7='" + clsPmpaType.TMN.Amt[7 - 1] + "',";
            SQL += ComNum.VBLF + "      GbEnd='" + clsPmpaType.TMN.GbEnd + "'";
            SQL += ComNum.VBLF + "WHERE WRTNO = '" + clsPmpaType.TMN.WRTNO + "'";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Upt_MISU_SLIP_CmdOK_IDMST_UPDATE(PsmhDb pDbcon, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            SQL += ComNum.VBLF + "UPDATE MISU_SLIP SET ";
            SQL += ComNum.VBLF + "      MisuID='" + clsPmpaType.TMN.MisuID + "',";
            SQL += ComNum.VBLF + "      IpdOpd='" + clsPmpaType.TMN.IpdOpd + "',";
            SQL += ComNum.VBLF + "      GelCode='" + clsPmpaType.TMN.GelCode + "'";
            SQL += ComNum.VBLF + "WHERE WRTNO = '" + clsPmpaType.TMN.WRTNO + "'";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MISU_HISTORY_CmdOK_IDMST_UPDATE(PsmhDb pDbcon, string argDate, string argTime, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            SQL += ComNum.VBLF + "INSERT INTO MISU_HISTORY ";
            SQL += ComNum.VBLF + "      (WRTNO, UpdateTime,UpdateTable,UpdateJob,";
            SQL += ComNum.VBLF + "      UpdateSabun,MisuID, Bdate, GelCode,IpdOpd,Class,";
            SQL += ComNum.VBLF + "      Gubun,EntDate,EntPart)";
            SQL += ComNum.VBLF + "VALUES";
            SQL += ComNum.VBLF + "      ('" + clsPmpaType.TMN.WRTNO + "',TO_DATE('" + argDate + " " + argTime + "','yyyy-mm-dd hh24:mi'),";
            SQL += ComNum.VBLF + "      'M','M', '" + clsType.User.IdNumber + "','" + clsPmpaType.TMM.MisuID + "',";
            SQL += ComNum.VBLF + "      TO_DATE('" + clsPmpaType.TMM.BDate + "','YYYY-MM-DD'),'" + clsPmpaType.TMM.GelCode + "',";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMM.IpdOpd + "','" + clsPmpaType.TMM.Class + "','" + clsPmpaType.TMM.Bun + "', ";
            SQL += ComNum.VBLF + "      TO_DATE('" + clsPmpaType.TMM.EntDate + "','yyyy-mm-dd hh24:mi'),";
            SQL += ComNum.VBLF + "      " + clsPmpaType.TMM.EntPart + ")";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Del_MISU_SLIP_CmdOK_MisuSlip_Delete(PsmhDb pDbcon, string argROWID, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "MISU_SLIP WHERE ROWID = '" + argROWID + "' ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MISU_HISTORY_CmdOK_MisuSlip_Delete(PsmhDb pDbcon, string argDate, string argTime, string argBdate2, string argGubun2, double argnQty2, double argnTAmt2, double argnAmt2, string argRemark2, string argEntDate2, long argnEntPart2, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";
            SQL += ComNum.VBLF + "INSERT INTO MISU_HISTORY (WRTNO, UpdateTime,UpdateTable,UpdateJob,";
            SQL += ComNum.VBLF + "      UpdateSabun,MisuID, Bdate, GelCode,IpdOpd,Class,";
            SQL += ComNum.VBLF + "      Gubun,Qty,TAmt,Amt,Remark,EntDate,EntPart)";
            SQL += ComNum.VBLF + "VALUES";
            SQL += ComNum.VBLF + "      ('" + clsPmpaType.TMN.WRTNO + "',TO_DATE('" + argDate + " " + VB.Left(argTime, 5) + "','yyyy-mm-dd hh24:mi'),";
            SQL += ComNum.VBLF + "      'S','D','" + clsType.User.IdNumber + "','" + clsPmpaType.TMM.MisuID + "', ";
            SQL += ComNum.VBLF + "      TO_DATE('" + argBdate2 + "','YYYY-MM-DD'),'" + clsPmpaType.TMM.GelCode + "',";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMM.IpdOpd + "','" + clsPmpaType.TMM.Class + "','" + argGubun2 + "',";
            SQL += ComNum.VBLF + "      " + argnQty2 + "," + argnTAmt2 + "," + argnAmt2 + ",'" + argRemark2 + "',";
            SQL += ComNum.VBLF + "      TO_DATE('" + argEntDate2 + "','yyyy-mm-dd hh24:mi'),";
            SQL += ComNum.VBLF + "      " + argnEntPart2 + ")";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MISU_SLIP_CmdOK_MisuSlip_Insert(PsmhDb pDbcon, string argGubun1, double argnQty1, double argnTAmt1, double argnAmt1, string argBdate1, string argRemark1, string argDate, string argTime, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            SQL += ComNum.VBLF + "INSERT INTO MISU_SLIP ";
            SQL += ComNum.VBLF + "      (WRTNO,MisuID,Bdate,GelCode,IpdOpd,";
            SQL += ComNum.VBLF + "      Class,Gubun,Qty,TAmt,Amt,Remark,EntDate,EntPart) ";
            SQL += ComNum.VBLF + "VALUES";
            SQL += ComNum.VBLF + "      ('" + clsPmpaType.TMN.WRTNO + "','" + clsPmpaType.TMN.MisuID.Trim() + "',TO_DATE('" + argBdate1 + "','YYYY-MM-DD'),";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMN.GelCode.Trim() + "','" + clsPmpaType.TMN.IpdOpd.Trim() + "',";
            SQL += ComNum.VBLF + "      '" + clsPmpaType.TMN.Class + "','" + argGubun1 + "'," + argnQty1 + "," + argnTAmt1 + "," + argnAmt1 + ",";
            SQL += ComNum.VBLF + "      '" + argRemark1 + "', ";
            SQL += ComNum.VBLF + "      TO_DATE('" + argDate + " " + argTime + "','YYYY-MM-DD hh24:mi'),'" + clsType.User.IdNumber + "')  ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Upt_MISU_SLIP_CmdOK_MisuSlip_Update(PsmhDb pDbcon, string argBdate1, string argGubun1, double argnQty1, double argnTAmt1, double argnAmt1, string argRemark1, string argROWID, string argDate, string argTime, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            SQL += ComNum.VBLF + "UPDATE MISU_SLIP SET";
            SQL += ComNum.VBLF + "      MisuID = '" + clsPmpaType.TMN.MisuID.Trim() + "',";
            SQL += ComNum.VBLF + "      Bdate = TO_DATE('" + argBdate1 + "','YYYY-MM-DD'),";
            SQL += ComNum.VBLF + "      GelCode = '" + clsPmpaType.TMN.GelCode.Trim() + "',";
            SQL += ComNum.VBLF + "      IpdOpd = '" + clsPmpaType.TMN.IpdOpd.Trim() + "',";
            SQL += ComNum.VBLF + "      Class = '" + clsPmpaType.TMN.Class + "',";
            SQL += ComNum.VBLF + "      Gubun = '" + argGubun1 + "',";
            SQL += ComNum.VBLF + "      Qty    = " + argnQty1 + ",";
            SQL += ComNum.VBLF + "      TAmt  = " + argnTAmt1 + ", ";
            SQL += ComNum.VBLF + "      Amt   = " + argnAmt1 + ",";
            SQL += ComNum.VBLF + "      Remark = '" + argRemark1 + "',";
            SQL += ComNum.VBLF + "      EntDate = TO_DATE('" + argDate + " " + argTime + "','YYYY-MM-DD hh24:mi'), ";
            SQL += ComNum.VBLF + "      EntPart = '" + clsType.User.IdNumber + "'";
            SQL += ComNum.VBLF + "WHERE ROWID = '" + argROWID + "'";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public string Int_MISU_HISTORY_CmdOK_MISUSlip_Update(PsmhDb pDbcon, string argDate, string argTime, string argBdate2, string argGubun2, double argnQty2, double argnTAmt2, double argnAmt2, string argRemark2, string argEntDate2, long argnEntPart2, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            SQL += ComNum.VBLF + " INSERT INTO MISU_HISTORY                                                                              ";
            SQL += ComNum.VBLF + "        (WRTNO, UpdateTime,UpdateTable,UpdateJob,                                                      ";
            SQL += ComNum.VBLF + "        UpdateSabun,MisuID, Bdate, GelCode,IpdOpd,Class,                                               ";
            SQL += ComNum.VBLF + "        Gubun,Qty,TAmt,Amt,Remark,EntDate,EntPart)                                                     ";
            SQL += ComNum.VBLF + " VALUES                                                                                                ";
            SQL += ComNum.VBLF + "        ('" + clsPmpaType.TMN.WRTNO + "',TO_DATE('" + argDate + " " + argTime + "','yyyy-mm-dd hh24:mi'),  ";
            SQL += ComNum.VBLF + "        'S','M', '" + clsType.User.IdNumber + "','" + clsPmpaType.TMM.MisuID + "',                                            ";
            SQL += ComNum.VBLF + "        TO_DATE('" + argBdate2 + "','YYYY-MM-DD'),'" + clsPmpaType.TMM.GelCode + "',                               ";
            SQL += ComNum.VBLF + "        '" + clsPmpaType.TMM.IpdOpd + "','" + clsPmpaType.TMM.Class + "','" + argGubun2 + "',                                  ";
            SQL += ComNum.VBLF + "        " + argnQty2 + "," + argnTAmt2 + "," + argnAmt2 + ",'" + argRemark2 + "',                               ";
            SQL += ComNum.VBLF + "        TO_DATE('" + argEntDate2 + "','yyyy-mm-dd hh24:mi'),                                           ";
            SQL += ComNum.VBLF + "        " + argnEntPart2 + ")                                                                             ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return "";
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                return ex.Message;
            }

            return SqlErr;
        }

        public DataTable Sel_MISU_SLIP_Display(PsmhDb pDbCon, long argWRTNO)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT TO_CHAR(Bdate,'YYYY-MM-DD') Bdate,Gubun,Qty,Amt,Remark,ROWID,";
            SQL += ComNum.VBLF + "       TO_CHAR(EntDate,'YYYY-MM-DD HH24:Mi') EntDate,EntPart";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_SLIP";
            SQL += ComNum.VBLF + "WHERE WRTNO = " + argWRTNO + "";
            SQL += ComNum.VBLF + "ORDER BY Bdate,Gubun";


            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable Sel_MISU_MONTHLY_Display(PsmhDb pDbCon, long argWRTNO)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT YYMM,IwolAmt,MisuAmt,IpgumAmt,SakAmt,BanAmt,EtcAmt,JanAmt";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_MONTHLY";
            SQL += ComNum.VBLF + "WHERE WRTNO=" + argWRTNO + "";
            SQL += ComNum.VBLF + "ORDER BY YYMM";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }



        #endregion

        #endregion

        #region 트랜잭션 쿼리 + enum INSERT, UPDATE,DELETE .... 

        #endregion
    }
}