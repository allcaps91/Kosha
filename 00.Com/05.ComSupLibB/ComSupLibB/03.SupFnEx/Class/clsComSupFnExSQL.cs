using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Data;

namespace ComSupLibB.SupFnEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray 
    /// File Name       : clsComSupXray.cs
    /// Description     : 진료지원 공통 기능검사 쿼리관련 class
    /// Author          : 윤조연
    /// Create Date     : 2017-06-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history> 
    public class clsComSupFnExSQL
    {
        string SQL = "";
        string SqlErr = ""; //에러문 받는 변수

        #region NON 트랜잭션 쿼리

        public DataTable sel_Etc_Jupmst(PsmhDb pDbCon,  string argROWID, string argPtno, string argCols, string argWhere, bool bLog)
        {
            DataTable dt = null;

            if (argPtno == "" && argROWID == "")
            {
                return null;
            }
                        
            SQL = "";
            SQL += " SELECT                                                                                     \r\n";
            if (argCols !="")
            {
                SQL += "    " + argCols + "                                                                     \r\n";
            }
            else
            {
                SQL += "     a.GbIO                                                                             \r\n";
                SQL += "   , a.Bun                                                                              \r\n";
                SQL += "   , a.Ptno                                                                             \r\n";
                SQL += "   , a.SName                                                                            \r\n";
                SQL += "   , (a.Sex ||'/'|| a.Age) SAge                                                         \r\n";
                SQL += "   , a.Age                                                                              \r\n";
                SQL += "   , a.DeptCode                                                                         \r\n";
                SQL += "   , a.DrCode                                                                           \r\n";
                SQL += "   , a.ORDERNO                                                                          \r\n";
                SQL += "   , a.OrderCode                                                                        \r\n";
                SQL += "   , a.GuBun                                                                            \r\n";
                SQL += "   , a.ASA                                                                              \r\n";
                SQL += "   , TO_CHAR(a.RDate,'YYYY-MM-DD HH24:MI') RDate                                        \r\n";
                SQL += "   , TO_CHAR(a.RDate,'YYYY-MM-DD HH24:MI') RDate1                                       \r\n";
                SQL += "   , a.Remark                                                                           \r\n";
                SQL += "   , a.GbEr                                                                             \r\n";
                SQL += "   , a.gbselect                                                                         \r\n";
                SQL += "   , TO_CHAR(a.BDate,'YYYY-MM-DD') BDay                                                 \r\n";
                SQL += "   , TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                                \r\n";
                SQL += "   , a.GbPort                                                                           \r\n";                
                SQL += "   , '' WardCode                                                                        \r\n";
                SQL += "   , a.ROOMCODE                                                                         \r\n";
                SQL += "   , TO_CHAR(a.ArrDate,'YYYY-MM-DD HH24:MI') ArrDate                                    \r\n";
                SQL += "   , TO_CHAR(a.startDate,'YYYY-MM-DD HH24:MI') startDate                                \r\n";
                SQL += "   , TO_CHAR(a.cRDate,'YYYY-MM-DD HH24:MI') cRDate                                      \r\n";
                SQL += "   , TO_CHAR(A.ENTDATE, 'YYYY-MM-DD HH24:MI') ENTDATE                                   \r\n";
                SQL += "   , a.Gubun2                                                                           \r\n";                
                SQL += "   , a.Cp                                                                               \r\n";
                SQL += "   , a.GbECG                                                                            \r\n";
                SQL += "   , a.GbFTP                                                                            \r\n";
                SQL += "   , a.CVR                                                                              \r\n";
                SQL += "   , a.CVRDetail                                                                        \r\n";
                SQL += "   , a.ResultDrSabun                                                                    \r\n";
                SQL += "   , a.GbJob                                                                            \r\n";
                SQL += "   , a.Gubun3                                                                           \r\n";
                SQL += "   , a.ROWID                                                                            \r\n";
            }            
            SQL += " FROM " + ComNum.DB_MED + "ETC_JUPMST a                                                     \r\n";
            SQL += "  WHERE 1=1                                                                                 \r\n";            
            if (argROWID != "")
            {
                SQL += "    AND a.ROWID = '" + argROWID + "'                                                    \r\n";
            }
            if (argPtno != "")
            {
                SQL += "    AND a.Ptno = '" + argPtno + "'                                                      \r\n";
            }
            if (argWhere != "")
            {
                SQL += "      " + argWhere + "                                                                  \r\n";
            }

            try
            {
                if (bLog == true)
                {
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                }
                else
                {
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                }                

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

        /// <summary>
        /// 오더기준 심초음파 시디복사 쿼리
        /// </summary>
        /// <param name="argSDate"></param>
        /// <param name="argTDate"></param>
        /// <returns></returns>
        public DataTable sel_orderView(PsmhDb pDbCon, string argSDate, string argTDate)
        {            
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT  ";
            SQL += "  'I' GBN, a.PTNO,TO_CHAR(a.BDATE,'YYYY-MM-DD') BDate,a.DEPTCODE,b.SNAME, A.WARDCODE, A.ROOMCODE, SUM(QTY*NAL) CNT          \r\n";
            SQL += " FROM " + ComNum.DB_MED + "OCS_IORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b                                               \r\n";
            SQL += "  WHERE 1=1                                                                                                                 \r\n";
            SQL += "   AND a.Ptno=b.Pano(+)                                                                                                     \r\n";
            SQL += "   AND a.BDate>=TO_DATE('" + argSDate + "','YYYY-MM-DD')                                                                   \r\n";
            SQL += "    AND a.BDate<=TO_DATE('" + argTDate + "','YYYY-MM-DD')                                                                   \r\n";
            SQL += "    AND a.SuCode ='CUSCOPY'                                                                                                 \r\n";
            SQL += "    AND A.GBSEND =' '                                                                                                       \r\n";
            SQL += " GROUP BY 1,a.PTNO,TO_CHAR(a.BDATE,'YYYY-MM-DD'),a.DEPTCODE,b.SNAME, A.WARDCODE, A.ROOMCODE                                 \r\n";
            SQL += " UNION ALL                                                                                                                  \r\n";
            SQL += "SELECT                                                                                                                      \r\n";
            SQL += "  'O' GBN, a.PTNO,TO_CHAR(a.BDATE,'YYYY-MM-DD') BDate,a.DEPTCODE,b.SNAME, '' WARDCODE, '' ROOMCODE, SUM(QTY*NAL)            \r\n";
            SQL += " FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b                                                \r\n";
            SQL += "  WHERE 1=1                                                                                                                 \r\n";
            SQL += "   AND a.Ptno=b.Pano(+)                                                                                                     \r\n";
            SQL += "   AND a.BDate>=TO_DATE('" + argSDate + "','YYYY-MM-DD')                                                                    \r\n";
            SQL += "   AND a.BDate<=TO_DATE('" + argTDate + "','YYYY-MM-DD')                                                                    \r\n";
            SQL += "    AND a.SuCode ='CUSCOPY'                                                                                                 \r\n";
            SQL += "    AND a.GbSunap ='1'                                                                                                      \r\n";
            SQL += " GROUP BY 1,a.PTNO,TO_CHAR(a.BDATE,'YYYY-MM-DD'),a.DEPTCODE,b.SNAME                                                         \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
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
                
        public  DataTable sel_ipdNewMaster(PsmhDb pDbCon, string argPano)
        {            
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                              \r\n";
            SQL += "  WardCode,RoomCode                                 \r\n";
            SQL += " ,TO_CHAR(InDate,'YYYY-MM-DD HH24:MI') InDate       \r\n";
            SQL += " ,TO_CHAR(InDate,'HH24:MI') InTime                  \r\n";
            SQL += " ,TO_CHAR(InDate,'YYYY-MM-DD') InDate2              \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER          \r\n";
            SQL += "  WHERE 1=1                                         \r\n";
            SQL += "   AND Pano= '" + argPano + "'                      \r\n";
            SQL += "   AND JDate=TO_DATE('1900-01-01','YYYY-MM-DD')     \r\n";
            SQL += "   AND GbSTS NOT IN ('9','7')                       \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
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

        /// <summary>
        /// ABR 등록에 사용되는 쿼리
        /// </summary>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public DataTable sel_ExamABRSch(PsmhDb pDbCon,string argPart, string argFDate, string argTDate,string argROWID="")
        {

            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                         \r\n";
            SQL += "  A.EXDATE RDATE1,TO_CHAR(A.EXDATE,'YYYY-MM-DD') RDATE2,TO_CHAR(A.EXDATE,'HH24:MI') RDATE3,     \r\n";
            SQL += "  A.EXAMNAME, A.PANO,a.WRITESABUN, a.ROWID                                                      \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_ABR_SCHEDULE a                                                \r\n";
            SQL += "  WHERE 1 = 1                                                                                   \r\n";
            if (argPart != "")
            {
                SQL += "    AND a.Gubun ='" + argPart + "'                                                          \r\n";
            }
            else
            {
                SQL += "    AND (a.Gubun IS NULL OR a.Gubun ='01')                                                   \r\n";
            }
            if (argROWID!="")
            {
                SQL += "    AND A.ROWID = '" + argROWID + "'                                                        \r\n";               
            }
            else
            {                
                SQL += "    AND A.EXDATE >= TO_DATE('" + argFDate + "','YYYY-MM-DD')                                \r\n";
                SQL += "    AND A.EXDATE <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')                  \r\n";
                SQL += "   ORDER BY A.EXDATE                                                                        \r\n";
            }
            

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
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

        /// <summary>
        /// ABR 스케쥴에 사용되는 쿼리
        /// </summary>
        /// <param name="argSDate"></param>
        /// <param name="argTDate"></param>
        /// <returns></returns>
        public DataTable sel_ExamABRSch2(PsmhDb pDbCon, string argPart, string argSDate, string argTDate = "")
        {
           
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                         \r\n";
            SQL += "  A.EXDATE RDATE1,TO_CHAR(A.EXDATE,'YYYY-MM-DD') RDATE2,TO_CHAR(A.EXDATE,'HH24:MI') RDATE3,     \r\n";
            SQL += "  A.EXAMNAME, A.PANO, B.SNAME                                                                   \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_ABR_SCHEDULE a, " + ComNum.DB_PMPA + "BAS_PATIENT b           \r\n";
            SQL += "  WHERE 1 = 1                                                                                   \r\n";
            SQL += "    AND A.PANO = B.PANO(+)                                                                      \r\n";
            if (argPart !="")
            {
                SQL += "    AND a.Gubun ='" + argPart + "'                                                          \r\n";
            }
            else
            {
                SQL += "    AND (a.Gubun IS NULL OR a.Gubun ='01')                                                  \r\n";
            }
            if (argTDate == "")
            {
                SQL += "    AND A.EXDATE >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                                \r\n";
                SQL += "    AND A.EXDATE <= TO_DATE('" + argSDate + " 23:59','YYYY-MM-DD HH24:MI')                  \r\n";
            }
            else
            {
                SQL += "    AND A.EXDATE >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                                \r\n";
                SQL += "    AND A.EXDATE <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')                  \r\n";
            }
            SQL += "   ORDER BY A.EXDATE, B.SNAME                                                                   \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
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

        /// <summary>
        /// 산부인과 검사 스케쥴에 사용되는 쿼리 - frmComSupFnExSCH01.cs frmComSupFnExSET01.cs
        /// </summary>
        /// <param name="argSDate"></param>
        /// <param name="argTDate"></param>
        /// <returns></returns>
        public DataTable sel_ETC_SCHEDULE_OG(PsmhDb pDbCon, string  argJob, string argSDate, string argTDate = "")
        {

            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                         \r\n";
            SQL += "  'A' GBN, Pano, STIME, Remark,DelDate,Gubun,ROWID                                              \r\n";
            SQL += "  ,TO_CHAR(SCHDATE ,'YYYY-MM-DD') SCHDATE, ENTDATE                                                       \r\n";

            SQL += "   FROM " + ComNum.DB_PMPA + "ETC_SCHEDULE_OG                                                   \r\n";
            SQL += "  WHERE 1 = 1                                                                                   \r\n";            
            if (argTDate == "")
            {
                SQL += "    AND SCHDate >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                                 \r\n";
                SQL += "    AND SCHDate <= TO_DATE('" + argSDate + " 23:59','YYYY-MM-DD HH24:MI')                   \r\n";
            }
            else
            {
                SQL += "    AND SCHDate >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                                 \r\n";
                SQL += "    AND SCHDate <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')                   \r\n";
            }

            SQL += "    AND (DelDate IS NULL OR DelDate='')                                                         \r\n";

            if (argJob =="1")
            {
                SQL += "    AND Gubun ='" + argJob  + "'                                                            \r\n";
            }
            else if (argJob == "2")
            {
                SQL += "    AND Gubun ='" + argJob + "'                                                             \r\n";
            }

            SQL += "   ORDER BY SCHDate, STIME, Pano                                                                \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
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

        public DataTable sel_consultView(PsmhDb pDbCon, string GBIO, string argPano, string argInDate,string argDept,string argManual ="")
        {            
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT  ";
            SQL += "  A.ENTDATE, A.PTNO, B.SNAME, A.FRDEPTCODE, A.FRDRCODE, A.TODEPTCODE, A.TODRCODE, A.GBCONFIRM,              \r\n";
            SQL += "  TO_CHAR(A.SDATE,'YYYY-MM-DD HH24:MI')  SDATE,                                                             \r\n";
            SQL += "  TO_CHAR(A.EDATE,'YYYY-MM-DD HH24:MI')  EDATE,                                                             \r\n";
            SQL += "  TO_CHAR(A.BDATE,'YYYY-MM-DD')  BDATE,                                                                     \r\n";
            SQL += "  A.FRREMARK, A.TOREMARK ,a.GbSTS, a.ROWID,                                                                 \r\n";
            SQL += "  C.DRNAME CDRNAME, D.DRNAME DDRNAME, TO_CHAR(A.INPDATE, 'YYYY-MM-DD') INPDATE, GBEMSMS                     \r\n";
            SQL += " FROM " + ComNum.DB_MED + "OCS_ITRANSFER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ,                           \r\n";
            SQL += "  " + ComNum.DB_PMPA + "BAS_DOCTOR c, " + ComNum.DB_PMPA + "BAS_DOCTOR d                                    \r\n";
            SQL += "  WHERE 1=1                                                                                                 \r\n";
            SQL += "    AND A.PTNO  = B.PANO(+)                                                                                 \r\n";
            SQL += "    AND A.FRDRCODE = C.DRCODE(+)                                                                            \r\n";
            SQL += "    AND A.TODRCODE = D.DRCODE(+)                                                                            \r\n";
            SQL += "    AND (A.GBSEND IS NULL OR A.GBSEND = ' ')                                                                \r\n";
            SQL += "    AND (A.GbDel IS NULL OR A.GbDel <>'*')                                                                  \r\n";
            SQL += "    AND A.PTNO ='" + argPano + "'                                                                           \r\n";
            if (argDept !="")
            {
                SQL += "    AND A.ToDeptCode ='" + argDept + "'                                                                 \r\n";
            }
            if (argManual == "Y")
            {

            }
            else
            {
                if (GBIO == "I")
                {
                    if (argInDate != "")
                    {
                        SQL += "   AND a.BDate>=TO_DATE('" + argInDate + "','YYYY-MM-DD')                                           \r\n";
                    }

                }
                else
                {
                    
                }
            }
            


            SQL += " ORDER BY BDATE DESC                                                                                        \r\n";


            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
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

        public  DataTable sel_ipdNewMaster_indate(PsmhDb pDbCon, string argPano, string argInDate)
        {           
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                  \r\n";
            SQL += "  TO_CHAR(INDATE,'YYYY-MM-DD') INDATE                                   \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                              \r\n";
            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "   AND Pano= '" + argPano + "'                                          \r\n";
            SQL += "   AND InDate<=TO_DATE('" + argInDate + " 23:59','YYYY-MM-DD HH24:MI')  \r\n";
            SQL += "   AND ( OutDate>=TO_DATE('" + argInDate + "','YYYY-MM-DD')             \r\n";
            SQL += "       OR JDate=TO_DATE('1900-01-01','YYYY-MM-DD'))                     \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
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

        public DataTable sel_HEA_RESV_EXAM(PsmhDb pDbCon, string Job, string argSDate, string argTDate , string argExCode,string argNE)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                              \r\n";
            SQL += "  'TO' GBN                                                                          \r\n";            
            SQL += "  ,TO_CHAR(b.RTIME,'YYYY-MM-DD HH24:MI') RDate1                                     \r\n";
            SQL += "  ,a.PTNO,a.PANO,a.SNAME,b.GBEXAM,b.EXCODE                                          \r\n";
            SQL += "  ,a.SDATE,a.WRTNO,b.EXAMNAME,c.ACTIVE                                              \r\n";
            SQL += "  ,TO_CHAR(b.RTIME,'HH24:MI') RDate2                                                \r\n";
            SQL += "  ,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate                                          \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "HEA_JEPSU  a                                           \r\n";
            SQL += "          ," + ComNum.DB_PMPA + "HEA_RESV_EXAM  b                                   \r\n";
            SQL += "          ," + ComNum.DB_PMPA + "HEA_RESULT  c                                      \r\n";
            SQL += " WHERE 1=1                                                                          \r\n";
            SQL += "  AND  a.Pano=b.Pano                                                                \r\n";
            SQL += "  AND  a.SDate=b.SDate                                                              \r\n";
            SQL += "  AND  a.WRTNO=C.WRTNO                                                              \r\n";
            SQL += "  AND  b.ExCode=C.ExCode                                                            \r\n";
            SQL += "  AND  a.DELDATE IS NULL                                                            \r\n";
            SQL += "  AND  b.DELDATE IS NULL                                                            \r\n";
            if (argTDate == "")
            {
                SQL += "  AND b.RTIME >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                       \r\n";
                SQL += "  AND b.RTIME <= TO_DATE('" + argSDate + " 23:59','YYYY-MM-DD HH24:MI')         \r\n";
            }
            else if (argTDate != "")
            {
                SQL += "  AND b.RTIME >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                       \r\n";
                SQL += "  AND b.RTIME <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')         \r\n";
            }            
            SQL += "  AND b.EXCODE IN (" + argExCode + ")                                               \r\n";

            if (Job =="UNION")
            {
                SQL += " UNION ALL                                                                      \r\n";
                SQL += "SELECT                                                                          \r\n";
                SQL += "  'HR' GBN                                                                      \r\n";                
                SQL += "  ,TO_CHAR(a.JepDate,'YYYY-MM-DD HH24:MI') RDate1                               \r\n";
                SQL += "  ,a.PTNO,a.PANO,a.SNAME,'99' GbExam,b.EXCODE                                   \r\n";
                SQL += "  ,a.JepDATE SDate,a.WRTNO,c.HNAME ExamName,b.ACTIVE                            \r\n";
                SQL += "  ,TO_CHAR(a.JepDate,'HH24:MI') RDate2                                          \r\n";
                SQL += "  ,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate                                      \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "HIC_JEPSU  a                                       \r\n";
                SQL += "          ," + ComNum.DB_PMPA + "HIC_RESULT  b                                  \r\n";
                SQL += "          ," + ComNum.DB_PMPA + "HIC_EXCODE  c                                  \r\n";                
                SQL += " WHERE 1=1                                                                      \r\n";                
                SQL += "  AND  a.WRTNO=b.WRTNO(+)                                                       \r\n";
                SQL += "  AND  b.ExCode=c.Code(+)                                                       \r\n";
                SQL += "  AND  a.DELDATE IS NULL                                                        \r\n";                
                if (argTDate == "")
                {
                    SQL += "  AND a.JepDate >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                 \r\n";
                    SQL += "  AND a.JepDate <= TO_DATE('" + argSDate + " 23:59','YYYY-MM-DD HH24:MI')   \r\n";
                }
                else if (argTDate != "")
                {
                    SQL += "  AND a.JepDate >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                 \r\n";
                    SQL += "  AND a.JepDate <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')   \r\n";
                }
                SQL += "  AND b.EXCODE IN (" + argExCode + ")                                           \r\n";
                
            }
            if (argNE == "True")
            {
                SQL += " UNION ALL                                                                      \r\n";
                SQL += "SELECT                                                                          \r\n";
                SQL += "  'PS' GBN                                                                      \r\n";
                SQL += "  ,TO_CHAR(a.EXAMRES15,'YYYY-MM-DD HH24:MI') RDate1                             \r\n";
                SQL += "  ,a.PaNO Ptno,0 PANO,b.SNAME,'' GbExam,'' ExCode                               \r\n";
                SQL += "  ,a.EXAMRES15 SDate,0 ,'' ,''                                                  \r\n";
                SQL += "  ,TO_CHAR(a.EXAMRES15,'HH24:MI') RDate2                                        \r\n";
                SQL += "  ,TO_CHAR(a.EXAMRES15,'YYYY-MM-DD') JepDate                                    \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO a                                \r\n";
                SQL += "          ," + ComNum.DB_PMPA + "BAS_PATIENT  b                                 \r\n";
                SQL += " WHERE 1=1                                                                      \r\n";
                SQL += "  AND  a.Pano=b.Pano                                                            \r\n";

                if (argTDate == "")
                {
                    SQL += "  AND a.EXAMRES15 >= TO_DATE('" + argSDate + "','YYYY-MM-DD')               \r\n";
                    SQL += "  AND a.EXAMRES15 <= TO_DATE('" + argSDate + " 23:59','YYYY-MM-DD HH24:MI') \r\n";
                }
                else if (argTDate != "")
                {
                    SQL += "  AND a.EXAMRES15 >= TO_DATE('" + argSDate + "','YYYY-MM-DD')               \r\n";
                    SQL += "  AND a.EXAMRES15 <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI') \r\n";
                }
            }
            
            SQL += "  ORDER BY 2,3,5                                                                    \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
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

        public DataTable sel_BAS_PATIENT_POSCO(PsmhDb pDbCon, string argSDate,string argTDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT  ";
            SQL += "  TO_CHAR(JDATE,'YYYY-MM-DD') JDATE , SINGU, SNAME, SABUN,Buse, PANO,JOBNAME,Gubun,ROWID            \r\n";
            SQL += "  ,JUMIN1 || JUMIN2 JUMIN , TEL, HPHONE, TO_CHAR(EXAMRES1,'MM-DD') EXAM1                            \r\n";
            SQL += "  ,TO_CHAR(EXAMRES2,'MM-DD HH24:MI') EXAM2, TO_CHAR(EXAMRES3,'MM-DD') EXAM3                         \r\n";
            SQL += "  ,TO_CHAR(EXAMRES4,'MM-DD HH24:MI') EXAM4, TO_CHAR(EXAMRES6,'MM-DD') EXAM6                         \r\n";
            SQL += "  ,TO_CHAR(EXAMRES7,'MM-DD HH24:MI') EXAM7,TO_CHAR(EXAMRES8,'MM-DD') EXAM8                          \r\n";
            SQL += "  ,TO_CHAR(EXAMRES9,'MM-DD HH24:MI') EXAM9,TO_CHAR(EXAMRES10,'MM-DD') EXAM10                        \r\n";
            SQL += "  ,TO_CHAR(EXAMRES11,'MM-DD HH24:MI') EXAM11,TO_CHAR(EXAMRES12,'MM-DD') EXAM12                      \r\n";
            SQL += "  ,TO_CHAR(EXAMRES13,'MM-DD HH24:MI') EXAM13,TO_CHAR(EXAMRES14,'MM-DD HH24:MI') EXAM14              \r\n";
            SQL += "  ,TO_CHAR(EXAMRES15,'MM-DD HH24:MI') EXAM15                                                        \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO                                                       \r\n";            
            SQL += "  WHERE 1=1                                                                                         \r\n";
            SQL += "    AND ( EXAMRES13 >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                                     \r\n";
            SQL += "          AND EXAMRES13 <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI'))                  \r\n";
            SQL += "     OR ( EXAMRES14 >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                                     \r\n";
            SQL += "          AND EXAMRES14 <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI'))                  \r\n";
            SQL += "     OR ( EXAMRES15 >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                                     \r\n";
            SQL += "          AND EXAMRES15 <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI'))                  \r\n";
            SQL += " ORDER BY JDATE, SNAME                                                                              \r\n";


            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
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

        /// <summary>
        /// 외래 낙상고위험군 테이블 쿼리
        /// </summary>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public DataTable sel_Nur_Fall_Scale_Opd(PsmhDb pDbCon, string argPano, string argDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                      \r\n";
            SQL += "  PANO,ROWID                                                \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "NUR_FALL_SCALE_OPD               \r\n";
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND Pano  = '" + argPano + "'                           \r\n";
            SQL += "    AND ACTDATE =TO_DATE('" + argDate + "','YYYY-MM-DD')    \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
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

        /// <summary>
        /// 치매척도검사 조회쿼리
        /// </summary>
        /// <param name="argGubun"></param>
        /// <param name="argFDate"></param>
        /// <param name="argTDate"></param>
        /// <param name="argDept"></param>
        /// <returns></returns>
        public DataTable sel_ETC_RESULT_DEMENTIA(PsmhDb pDbCon, string argGubun, string argFDate, string argTDate, string argDept, string argROWID = "" ,string argSpano = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                 \r\n";
            SQL += "   A.PTNO, A.SNAME, A.SEX, A.AGE, A.DEPTCODE                            \r\n";
            SQL += "   ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                 \r\n";
            SQL += "   ,TO_CHAR(a.JDate,'YYYY-MM-DD') JDate                                 \r\n";
            SQL += "   ,TO_CHAR(a.RDate,'YYYY-MM-DD') RDate                                 \r\n";
            SQL += "   ,a.RESULT,a.ORDERCODE,a.ORDERNO,a.ResultSabun                        \r\n";
            SQL += "   ,A.DRCODE, A.GBIO, A.ROWID,  B.DRNAME  , C.ORDERNAME                 \r\n";
            SQL += "  FROM " + ComNum.DB_MED + "ETC_RESULT_DEMENTIA a                       \r\n";
            SQL += "     , " + ComNum.DB_PMPA + "BAS_DOCTOR b                               \r\n";
            SQL += "     , " + ComNum.DB_MED + "OCS_ORDERCODE c                             \r\n";
            SQL += "   WHERE 1 = 1                                                          \r\n";
            SQL += "    AND a.DRCODE =b.DRCODE(+)                                           \r\n";
            SQL += "    AND a.ORDERCODE =c.ORDERCODE(+)                                     \r\n";
            SQL += "    AND (a.DDate IS NULL OR a.DDate ='')                                \r\n";

            if (string.IsNullOrEmpty(argSpano) == false)
            {
                SQL += "    AND a.ptno = '" + argSpano + "'                                \r\n";
            }

            if (argROWID != "")
            {
                SQL += "    AND a.ROWID = '" + argROWID + "'                                \r\n";
            }
            else
            {
                if (argGubun != "ALL")
                {
                    SQL += "    AND a.GUBUN = '" + argGubun + "'                            \r\n";
                }
                if (argDept != "ALL")
                {
                    SQL += "    AND a.DEPTCODE IN (" + argDept + " )                        \r\n";
                }
                SQL += "    AND a.BDate >= TO_DATE('" + argFDate + "','YYYY-MM-DD')         \r\n";
                SQL += "    AND a.BDate <= TO_DATE('" + argTDate + "','YYYY-MM-DD')         \r\n";
                SQL += "   ORDER BY a.BDATE ,a.PTNO                                         \r\n";
            }


            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

        /// <summary>
        /// 치매척도검사 검사코드 콤보 쿼리
        /// </summary>
        /// <param name="argIO"></param>
        /// <param name="argFDate"></param>
        /// <param name="argTDate"></param>
        /// <param name="argDept"></param>
        /// <returns></returns>
        public DataTable sel_OCS_ORDERCODE_dementia(PsmhDb pDbCon)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                         \r\n";
            SQL += "    trim(ORDERCODE) || '.' || ORDERNAME Code                    \r\n";
            SQL += "   ,ORDERCODE, ORDERNAME                                        \r\n";            
            SQL += "  FROM " + ComNum.DB_MED + "OCS_ORDERCODE                       \r\n";            
            SQL += "   WHERE 1 = 1                                                  \r\n";
            SQL += "    AND SENDDEPT <> 'N'                                         \r\n";
            SQL += "    AND SUCODE IN (                                             \r\n";
            SQL += "                   SELECT SUNEXT                                \r\n";
            SQL += "                     FROM " + ComNum.DB_PMPA + "BAS_SUN         \r\n";
            SQL += "                      WHERE 1=1                                 \r\n";
            SQL += "                       AND DTLBUN ='0801'                       \r\n";
            SQL += "                    )                                           \r\n";            
            SQL += "   ORDER BY SUCODE                                              \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

        #region 트랜잭션 쿼리 + enum INSERT, UPDATE,DELETE .... 

        /// <summary>
        /// ETC_JUPMST 특정컬럼 갱신
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argROWID"></param>
        /// <param name="argUpCols"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_Etc_JupMst(PsmhDb pDbCon, string argROWID, string argUpCols, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ETC_JUPMST  SET            \r\n";

            SQL += "    " + argUpCols + "                                   \r\n";

            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "    AND ROWID = '" + argROWID + "'                      \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_Etc_JupMst_Sim(PsmhDb pDbCon, string argGubun, string argPano, string argBdate, string argUpCols, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ETC_JUPMST  SET            \r\n";

            SQL += "    " + argUpCols + "                                   \r\n";

            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "    AND PTNO = '" + argPano + "'                      \r\n";
            SQL += "    AND GUBUN = '" + argGubun + "'                      \r\n";
            SQL += "    AND BDATE = '" + argBdate + "'                      \r\n";
            SQL += "    AND GUBUN4 IS NULL                      \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_Etc_JupMst_IN(PsmhDb pDbCon, string argJob, string argROWID_IN, string argPtno, string argUpCols,string argWhere, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argROWID_IN =="" && argPtno =="")
            {
                return "에러";
            }
            if (argUpCols =="")
            {
                return "에러";
            }

            SQL = "";
            if (argJob =="00")
            {
                SQL += " UPDATE " + ComNum.DB_MED + "ETC_JUPMST  SET            \r\n";

                SQL += "    " + argUpCols + "                                   \r\n";

                SQL += "  WHERE 1=1                                             \r\n";                
                SQL += "    AND ROWID IN ( " + argROWID_IN + " )                \r\n";                
                
                if (argWhere != "")
                {
                    SQL += "      " + argWhere + "                              \r\n";
                }
            }
           else if (argJob == "01")
            {
                if (argWhere =="")
                {
                    return "에러";
                }
                SQL += " UPDATE " + ComNum.DB_MED + "ETC_JUPMST  SET            \r\n";
                SQL += "    " + argUpCols + "                                   \r\n";
                SQL += "  WHERE 1=1                                             \r\n";                                
                SQL += "    AND Ptno ='" + argPtno + "'                         \r\n";
                SQL += "      " + argWhere + "                                  \r\n";
                
            }
            else
            {

                return "에러";

                //SQL += " UPDATE " + ComNum.DB_MED + "ETC_JUPMST  SET            \r\n";

                //SQL += "    " + argUpCols + "                                   \r\n";

                //SQL += "  WHERE 1=1                                             \r\n";
                //if (argROWID_IN != "")
                //{
                //    SQL += "    AND ROWID IN ( " + argROWID_IN + " )            \r\n";
                //}
                //if (argPtno != "")
                //{
                //    SQL += "    AND Ptno ='" + argPtno + "'                     \r\n";
                //}
                //if (argWhere != "")
                //{
                //    SQL += "      " + argWhere + "                              \r\n";
                //}

            }
            

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_Etc_JupMst_del(PsmhDb pDbCon,string argJob, string argROWID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argROWID == "")
            {
                return "ROWID 공백";
            }
                        
            if (argJob =="00")
            {
                SQL = "";
                SQL += " INSERT INTO " + ComNum.DB_MED + "ETC_JUPMST_DEL                                                \r\n";
                SQL += " ( BDATE, PTNO, SNAME, SEX, AGE, ORDERCODE, ORDERNO, GBIO, BUN, DEPTCODE						\r\n";
                SQL += " 	,DRCODE, REMARK, GBJOB, RDATE, AMT, ENTDATE, GBER, ROOMCODE, GUBUN 	                        \r\n";
                SQL += " 	,GBPORT, RESULTDRSABUN, ORDERDATE, SENDDATE, EXAM_WRTNO, READ_WRTNO,  IMAGE_SIZE )        	\r\n";
                SQL += "  SELECT                                                                                        \r\n";
                SQL += "   BDATE, PTNO, SNAME, SEX, AGE, ORDERCODE, ORDERNO, GBIO, BUN, DEPTCODE						\r\n";
                SQL += " 	,DRCODE, REMARK, GBJOB, RDATE, AMT, ENTDATE, GBER, ROOMCODE, GUBUN 	                        \r\n";
                SQL += " 	,GBPORT, RESULTDRSABUN, ORDERDATE, SENDDATE, EXAM_WRTNO, READ_WRTNO,  IMAGE_SIZE )        	\r\n";
                SQL += "   FROM " + ComNum.DB_MED + "ETC_JUPMST                                                         \r\n";
                SQL += "    WHERE 1=1                                                                                   \r\n";
                SQL += "     AND ROWID ='" + argROWID + "'                                                              \r\n";
            }            
            else if (argJob == "01")
            {
                SQL = "";
                SQL += " INSERT INTO " + ComNum.DB_MED + "ETC_JUPMST_DEL                                                \r\n";
                SQL += " ( BDATE, PTNO, SNAME, SEX, AGE, ORDERCODE, ORDERNO, GBIO, BUN, DEPTCODE						\r\n";
                SQL += " 	,DRCODE, REMARK, GBJOB, RDATE, AMT, ENTDATE, GBER, ROOMCODE, GUBUN 	                        \r\n";
                SQL += " 	,GBPORT, RESULTDRSABUN, ORDERDATE, SENDDATE, EXAM_WRTNO, READ_WRTNO,  IMAGE_SIZE )        	\r\n";
                SQL += "  SELECT                                                                                        \r\n";
                SQL += "   BDATE, PTNO, SNAME, SEX, AGE, ORDERCODE, ORDERNO, GBIO, BUN, DEPTCODE						\r\n";
                SQL += " 	,DRCODE, REMARK, GBJOB, RDATE, AMT, ENTDATE, GBER, ROOMCODE, GUBUN 	                        \r\n";
                SQL += " 	,GBPORT, RESULTDRSABUN, ORDERDATE, SENDDATE, EXAM_WRTNO, READ_WRTNO,  IMAGE_SIZE )        	\r\n";
                SQL += "   FROM " + ComNum.DB_MED + "ETC_JUPMST                                                         \r\n";
                SQL += "    WHERE 1=1                                                                                   \r\n";
                SQL += "     AND ROWID IN ( " + argROWID + " )                                                          \r\n";
            }
            else
            {
                SQL = "";
                SQL += " INSERT INTO " + ComNum.DB_MED + "ETC_JUPMST_DEL                                                \r\n";
                SQL += " ( BDATE, PTNO, SNAME, SEX, AGE, ORDERCODE, ORDERNO, GBIO, BUN, DEPTCODE						\r\n";
                SQL += " 	,DRCODE, REMARK, GBJOB, RDATE, AMT, ENTDATE, GBER, ROOMCODE, GUBUN 	                        \r\n";
                SQL += " 	,GBPORT, RESULTDRSABUN, ORDERDATE, SENDDATE, EXAM_WRTNO, READ_WRTNO,  IMAGE_SIZE )        	\r\n";
                SQL += "  SELECT                                                                                        \r\n";
                SQL += "   BDATE, PTNO, SNAME, SEX, AGE, ORDERCODE, ORDERNO, GBIO, BUN, DEPTCODE						\r\n";
                SQL += " 	,DRCODE, REMARK, GBJOB, RDATE, AMT, ENTDATE, GBER, ROOMCODE, GUBUN 	                        \r\n";
                SQL += " 	,GBPORT, RESULTDRSABUN, ORDERDATE, SENDDATE, EXAM_WRTNO, READ_WRTNO,  IMAGE_SIZE )        	\r\n";
                SQL += "   FROM " + ComNum.DB_MED + "ETC_JUPMST                                                         \r\n";
                SQL += "    WHERE 1=1                                                                                   \r\n";
                SQL += "     AND ROWID ='" + argROWID + "'                                                              \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string del_Etc_JupMst(PsmhDb pDbCon,string argJob, string argROWID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argROWID == "")
            {
                return "ROWID 공백";
            }

            if (argJob =="00")
            {
                SQL = "";
                SQL += " DELETE FROM " + ComNum.DB_MED + "ETC_JUPMST                \r\n";
                SQL += "  WHERE 1=1                                                 \r\n";
                SQL += "    AND ROWID = '" + argROWID + "'                          \r\n";
            }
            else if (argJob == "01")
            {
                SQL = "";
                SQL += " DELETE FROM " + ComNum.DB_MED + "ETC_JUPMST                \r\n";
                SQL += "  WHERE 1=1                                                 \r\n";
                SQL += "    AND ROWID IN ( " + argROWID + " )                       \r\n";
            }
            else
            {
                SQL = "";
                SQL += " DELETE FROM " + ComNum.DB_MED + "ETC_JUPMST                \r\n";
                SQL += "  WHERE 1=1                                                 \r\n";
                SQL += "    AND ROWID = '" + argROWID + "'                          \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// ABR 검사 테이블 삭제 쿼리
        /// </summary>
        /// <param name="argROWID"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <param name="bBack"></param>
        /// <returns></returns>
        public string del_EXAM_ABR_SCHEDULE(PsmhDb pDbCon, string argROWID, ref int intRowAffected, bool bBack = true)
        {
            string SqlErr = string.Empty;

            if (bBack) //백업실행
            {
                SQL = "";
                SQL += "  INSERT INTO " + ComNum.DB_MED + "EXAM_ABR_SCHEDULE_HISTORY        \r\n";
                SQL += "   (EXDATE,PANO,EXAMNAME, WRITEDATE, WRITESABUN                     \r\n";
                SQL += "    ,GUBUN,OPTIME,DEPTCODE,OPSTAFF,AGE,SEX)                         \r\n";
                SQL += "  SELECT EXDATE,PANO,EXAMNAME, WRITEDATE, WRITESABUN                \r\n";
                SQL += "    ,GUBUN,OPTIME,DEPTCODE,OPSTAFF,AGE,SEX                          \r\n";
                SQL += "   FROM " + ComNum.DB_MED + "EXAM_ABR_SCHEDULE                      \r\n";
                SQL += "   WHERE 1=1                                                        \r\n";
                SQL += "    AND ROWID = '" + argROWID + "'                                  \r\n";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            }

            SQL = "";
            SQL += "  DELETE FROM " + ComNum.DB_MED + "EXAM_ABR_SCHEDULE                    \r\n";
            SQL += "   WHERE 1=1                                                            \r\n";
            SQL += "    AND ROWID = '" + argROWID + "'                                      \r\n";
            
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_EXAM_ABR_SCHEDULE(PsmhDb pDbCon, string argPart, string argPano,string argExDate,string argExName,long argJobSabun, string argDeptcode, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
                        
            SQL = "";
            SQL += "  INSERT INTO " + ComNum.DB_MED + "EXAM_ABR_SCHEDULE                \r\n";
            SQL += "   (EXDATE,PANO,EXAMNAME,Gubun, WRITEDATE, WRITESABUN, DEPTCODE )   \r\n";
            SQL += "   VALUES (                                                         \r\n";
            SQL += "   TO_DATE('" + argExDate + "','YYYY-MM-DD HH24:MI')                \r\n";
            SQL += "     ,'" + argPano + "'                                             \r\n";
            SQL += "     ,'" + argExName + "'                                           \r\n";
            SQL += "     ,'" + argPart + "'                                             \r\n";
            SQL += "     ,SYSDATE                                                       \r\n";
            SQL += "     ," + argJobSabun + "                                           \r\n";
            SQL += "     ,'" + argDeptcode + "'                                             \r\n";
            SQL += "         )                                                          \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }


        /// <summary>
        /// 산부인과 영상검사 스케쥴 등록 관련
        /// </summary>
        public  class c_ETC_SCHEDULE_OG
        {
            public string Pano = "";
            public string DeptCode = "";
            public string DrCode = "";
            public string RDate = "";
            public string RTime = "";
            public string Jong = "";
            public string Remark = "";
            public long Sabun = 0;
            public string strChange = "";
            public string ROWID = "";
        }

        /// <summary>
        /// 산부인과 영상검사 스케쥴 등록
        /// </summary>
        /// <param name="argCls"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string ins_ETC_SCHEDULE_OG(PsmhDb pDbCon, c_ETC_SCHEDULE_OG argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  INSERT INTO " + ComNum.DB_PMPA + "ETC_SCHEDULE_OG                                     \r\n";
            SQL += "   ( Pano,DEPTCODE, DRCODE, SCHDATE, STIME ,Gubun, REMARK , ENTDATE, ENTSABUN )         \r\n";
            SQL += "   VALUES (                                                                             \r\n";
            SQL += "     '" + argCls.Pano + "'                                                              \r\n";
            SQL += "     ,'" + argCls.DeptCode + "'                                                         \r\n";
            SQL += "     ,'" + argCls.DrCode + "'                                                           \r\n";
            SQL += "     ,TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')                                      \r\n";
            SQL += "     ,'" + argCls.RTime + "'                                                            \r\n";
            SQL += "     ,'" + argCls.Jong + "'                                                             \r\n";
            SQL += "     ,'" + argCls.Remark + "'                                                           \r\n";
            SQL += "     ,SYSDATE                                                                           \r\n";
            SQL += "     ," + argCls.Sabun + "                                                              \r\n";
            SQL += "         )                                                                              \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
        
        public string up_ETC_SCHEDULE_OG(PsmhDb pDbCon, c_ETC_SCHEDULE_OG argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  UPDATE " + ComNum.DB_PMPA + "ETC_SCHEDULE_OG  SET                 \r\n";  
            SQL += "     DRCODE = '" + argCls.DrCode + "'                               \r\n";
            SQL += "    ,SCHDATE = TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')         \r\n";
            SQL += "    ,STIME = '" + argCls.RTime + "'                                 \r\n";
            SQL += "    ,Gubun = '" + argCls.Jong + "'                                  \r\n";
            SQL += "    ,REMARK = '" + argCls.Remark + "'                               \r\n";
            //SQL += "    ,ENTDATE = SYSDATE                                              \r\n";
            //SQL += "    ,ENTSABUN = " + argCls.Sabun + "                                \r\n";
            SQL += "  WHERE 1=1                                                         \r\n";
            SQL += "   AND ROWID ='" + argCls.ROWID + "'                                \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
                
        public string del_ETC_SCHEDULE_OG(PsmhDb pDbCon, c_ETC_SCHEDULE_OG argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  UPDATE " + ComNum.DB_PMPA + "ETC_SCHEDULE_OG  SET                 \r\n";
            SQL += "    DELDATE = SYSDATE                                              \r\n";            
            SQL += "  WHERE 1=1                                                         \r\n";
            SQL += "   AND ROWID ='" + argCls.ROWID + "'                                \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 외래 낙상 위험군 등록 
        /// </summary>
        /// <param name="argPano"></param>
        /// <param name="argDate"></param>
        /// <param name="argRemark"></param>
        /// <param name="argSabun"></param>
        /// <param name="argGubun"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string ins_NUR_FALL_SCALE_OPD( string argPano, string argDate, long argSabun, string argGubun, PsmhDb pDbCon, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + "NUR_FALL_SCALE_OPD   \r\n";
            SQL += "   (PANO, ACTDATE, ENTSABUN, ENTDATE, ENTGUBUN)         \r\n";
            SQL += "   VALUES (                                             \r\n";
            SQL += "   '" + argPano + "'                                    \r\n";
            SQL += "   ,TO_DATE('" + argDate + "','YYYY-MM-DD')             \r\n";
            SQL += "   ," + argSabun + "                                    \r\n";
            SQL += "   ,SYSDATE                                             \r\n";
            SQL += "   ,'" + argGubun + "'                                  \r\n";
            SQL += "          )                                             \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 외래 낙상 위험군 삭제시 백업본
        /// </summary>
        /// <param name="argPano"></param>
        /// <param name="argDate"></param>
        /// <param name="argSabun"></param>
        /// <param name="argGubun"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string ins_NUR_FALL_SCALE_OPD_his( string argPano, string argDate, long argSabun, PsmhDb pDbCon, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + "NUR_FALL_SCALE_OPD_HISTORY           \r\n";
            SQL += "   (PANO, ACTDATE, ENTSABUN, ENTDATE, ENTGUBUN, DELDATE, DELSABUN)      \r\n";
            SQL += "  SELECT                                                                \r\n";
            SQL += "   PANO, ACTDATE, ENTSABUN, ENTDATE, ENTGUBUN, SYSDATE," + argSabun + " \r\n";
            SQL += "    FROM  " + ComNum.DB_PMPA + "NUR_FALL_SCALE_OPD                      \r\n";
            SQL += "      WHERE 1=1                                                         \r\n";
            SQL += "       AND Pano ='" + argPano + "'                                      \r\n";
            SQL += "       AND ActDAte=TO_DATE('" + argDate + "','YYYY-MM-DD')              \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 외래 낙상 고위험 데이타 삭제
        /// </summary>
        /// <param name="argPano"></param>
        /// <param name="argDate"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string del_NUR_FALL_SCALE_OPD( string argPano, string argDate, PsmhDb pDbCon, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  DELETE FROM " + ComNum.DB_PMPA + "NUR_FALL_SCALE_OPD          \r\n";
            SQL += "   WHERE 1=1                                                    \r\n";
            SQL += "    AND Pano = '" + argPano + "'                                \r\n";
            SQL += "    AND ActDate = TO_DATE('" + argDate + "','YYYY-MM-DD')       \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 치매척도 검사결과등록 관련 데이타 변수관련 class
        /// </summary>
        public class c_Etc_Result_Dementia
        {
            public string Pano = "";
            public string sName = "";
            public string Sex = "";
            public int Age = 0;
            public string Gubun = "";
            public string BDate = "";
            public string RDate = "";
            public string JDate = "";
            public string DDate = "";
            public string OrderCode = "";
            public long OrderNo = 0;
            public string DeptCode = "";
            public string DrCode = "";
            public string GbIO = "";
            public string WardCode = "";
            public string RoomCode = "";
            public long sabun = 0;
            public string Result = "";
            public string ResultDate = "";
            public string ROWID = "";

        }

        /// <summary>
        /// 치매척도 검사 업데이트 
        /// </summary>
        /// <param name="argCls"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_ETC_RESULT_DEMENTIA(PsmhDb pDbCon, c_Etc_Result_Dementia argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ETC_RESULT_DEMENTIA   SET          \r\n";
            if (argCls.DDate != "")
            {
                SQL += "     DDate =SYSDATE                                         \r\n";
            }
            else
            {
                SQL += "      SEX = '" + argCls.Sex + "'                            \r\n";
                SQL += "     ,AGE = " + argCls.Age + "                              \r\n";
                SQL += "     ,BDate =TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')   \r\n";
                SQL += "     ,JDate =TO_DATE('" + argCls.JDate + "','YYYY-MM-DD')   \r\n";
                SQL += "     ,RDate =TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')   \r\n";
                SQL += "     ,DeptCode = '" + argCls.DeptCode + "'                  \r\n";
                SQL += "     ,DrCode = '" + argCls.DrCode + "'                      \r\n";
                SQL += "     ,GbIO = '" + argCls.GbIO + "'                          \r\n";
                SQL += "     ,Result = '" + argCls.Result + "'                      \r\n";
                SQL += "     ,ORDERCODE = '" + argCls.OrderCode + "'                \r\n";
                SQL += "     ,Gubun = '" + argCls.Gubun + "'                        \r\n";
                SQL += "     ,RESULTDATE =SYSDATE                                   \r\n";
                SQL += "     ,ResultSabun = " + argCls.sabun + "                    \r\n";
            }

            SQL += " WHERE 1=1                                                      \r\n";
            SQL += "  AND ROWID ='" + argCls.ROWID + "'                             \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 치매척도검사 INSERT 문
        /// </summary>
        /// <param name="argCls"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string ins_ETC_RESULT_DEMENTIA(PsmhDb pDbCon, c_Etc_Result_Dementia argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "ETC_RESULT_DEMENTIA       \r\n";
            SQL += "    (PTNO ,SNAME,SEX,AGE,GUBUN,BDATE,JDATE,RDATE,DDATE      \r\n";
            SQL += "    ,ORDERCODE,ORDERNO,DEPTCODE,DRCODE,GBIO,WARDCODE        \r\n";
            SQL += "    ,ROOMCODE,RESULTDATE,RESULTSABUN, RESULT ) VALUES       \r\n";
            SQL += "    (                                                       \r\n";
            SQL += "     '" + argCls.Pano + "'                                  \r\n";
            SQL += "     ,'" + argCls.sName + "'                                \r\n";
            SQL += "     ,'" + argCls.Sex + "'                                  \r\n";
            SQL += "     ," + argCls.Age + "                                    \r\n";
            SQL += "     ,'" + argCls.Gubun + "'                                \r\n";
            SQL += "     ,TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')          \r\n";
            SQL += "     ,TO_DATE('" + argCls.JDate + "','YYYY-MM-DD')          \r\n";
            SQL += "     ,TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')          \r\n";
            SQL += "     ,''                                                    \r\n";
            SQL += "     ,'" + argCls.OrderCode + "'                            \r\n";
            SQL += "     ," + argCls.OrderNo + "                                \r\n";
            SQL += "     ,'" + argCls.DeptCode + "'                             \r\n";
            SQL += "     ,'" + argCls.DrCode + "'                               \r\n";
            SQL += "     ,'" + argCls.GbIO + "'                                 \r\n";
            SQL += "     ,'" + argCls.WardCode + "'                             \r\n";
            SQL += "     ,'" + argCls.RoomCode + "'                             \r\n";
            SQL += "     ,SYSDATE                                               \r\n";
            SQL += "     ," + argCls.sabun + "                                  \r\n";
            SQL += "     ,'" + argCls.Result + "'                               \r\n";
            SQL += "    )                                                       \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
        #endregion

    }
}
