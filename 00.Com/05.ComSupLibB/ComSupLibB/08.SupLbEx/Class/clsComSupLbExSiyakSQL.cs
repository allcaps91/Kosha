using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComDbB;
using ComBase;
using System.Data;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name : ComSupLibB.SupLbEx
    /// File Name : clsLbExSQL.cs
    /// Title or Description : 진단검사의학과 SQL
    /// Author : 안정수
    /// Create Date : 2017-05-15
    /// Update History : 
    /// </summary>
    public class clsComSupLbExSiyakSQL
    {
        string SQL = "";
        string SqlErr = "";

        public class cSupLbExSIDTL
        { 
            public string ACTDATE = "";
            public string JEPCODE = "";
            public string GBN = "";
            public string QTY = "";
            public string BUSECODE = ""; 
            public string REMARK = "";
            public string PART = "";
            public string SEQNO = "";
            public string LOTNO = "";
            public string EXPIREDATE = "";
            public string STORESTAT = ""; 
            public string ABSTRACT = "";
            public string GBNEW = "";
            public string INPT_DT = "";
            public string UPPS = "";
            public string UP_DT = "";
            public string ENTDATE = "";
            public string JEPNAME = "";
            public string SYSDATE = "";
            public int OutSeqno = 0;    //출고순서대로 표기하기 위함 

            //public string SEQNO = "";
            public string ROWID = "";

        }
        #region NONE 트랜잭션(단순 조회 쿼리 모음)

        /// <summary>
        /// 해당파트(ex.혈액학) 콤보박스 생성시 사용
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        public DataTable sel_Exam_Sicode(PsmhDb pDbCon)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                     \r\n";
            SQL += "  SICODE, SINAME                                                            \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_SICODE                                    \r\n";
            SQL += "  WHERE 1 = 1                                                               \r\n";
            SQL += "   AND GUBUN = '1'                                                          \r\n";
            SQL += " ORDER BY SICODE                                                            \r\n";

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

        /// <summary>
        /// 해당 시약코드에대한 회사가 등록되어있는지 여부를 확인
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argJEPCODE"></param>
        /// <returns></returns>
        public DataTable sel_AIS_LTD_LTDCODE(PsmhDb pDbCon, string argJEPCODE)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                     \r\n";
            SQL += "  Count(*)                                                                  \r\n";
            SQL += "   FROM " + ComNum.DB_ERP + "DRUG_JEP A, " + ComNum.DB_ERP + "AIS_LTD B     \r\n";
            SQL += "  WHERE 1 = 1                                                               \r\n";
            SQL += "   AND A.GELCODE = B.LTDCODE                                                \r\n";
            SQL += "   AND A.JEPCODE = '" + argJEPCODE + "'                                     \r\n";

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

        /// <summary>
        /// 입고된시약 조회
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argSeqNo"></param>
        /// <param name="argGubun"></param>
        /// <param name="argFDate"></param>
        /// <param name="argTDate"></param>
        /// <returns></returns>
        public DataTable sel_EXAM_SIDTL_In(PsmhDb pDbCon, string argSeqNo, string argGubun, string argLTDGubun, string argFDate = "", string argTDate = "", string argMenual = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                     \r\n";
            SQL += "  A.ACTDATE,                                                                \r\n";
            SQL += "  A.JEPCODE,                                                                \r\n";
            SQL += "  B.EXAMNAME,                                                               \r\n";
            SQL += "  B.EXAMUNITDTL,                                                            \r\n";
            SQL += "  B.UNIT1,                                                                  \r\n";
            SQL += "  B.UNIT2,                                                                  \r\n";
            SQL += "  B.UNIT3,                                                                  \r\n";
            SQL += "  A.LotNo,                                                                  \r\n";
            SQL += "  A.ExpireDate,                                                             \r\n";
            SQL += "  A.QTY,                                                                    \r\n";
            SQL += "  A.StoreStat,                                                              \r\n";
            SQL += "  A.Abstract,                                                               \r\n";
            SQL += "  C.NAME,                                                                   \r\n";
            SQL += "  A.SEQNO,                                                                  \r\n";
            SQL += "  B.JEPNAMEK,                                                               \r\n";
            SQL += "  A.ROWID                                                                   \r\n";
            SQL += "FROM " + ComNum.DB_MED + "EXAM_SIDTL A,                                     \r\n";
            SQL += ComNum.DB_ERP + "DRUG_JEP B,                                                 \r\n";
            SQL += ComNum.DB_ERP + "AIS_LTD C                                                   \r\n";
            SQL += "WHERE 1=1                                                                   \r\n";
            SQL += "       AND A.JEPCODE = B.JEPCODE                                            \r\n";
            SQL += "       AND B.GELCODE = C.LTDCODE                                            \r\n";
            if (argFDate != "" && argTDate != "")
            {
                SQL += "       AND A.ACTDATE >= TO_DATE('" + argFDate + "', 'YYYY-MM-DD')                           \r\n"; 
                SQL += "       AND A.ACTDATE <= TO_DATE('" + argTDate + " 23:59:59', 'YYYY-MM-DD HH24:MI:SS')       \r\n"; 
            }
            SQL += "       AND A.GBNEW = 'Y'                                                    \r\n";
            SQL += "       AND A.GBN = 'I'                                                      \r\n";
            if (argSeqNo != "")
            {
                SQL += "   AND A.SEQNO = '" + argSeqNo + "'                                     \r\n";
            }
            if (argGubun != "")
            {
                SQL += "   AND B.EXAMGUBUN = '" + argGubun + "'                                 \r\n";
            }
            if (argLTDGubun != "")
            {
                SQL += "   AND C.LTDCODE = '" + argLTDGubun + "'                                \r\n";
            }
            if (argMenual != "")
            {
                SQL += "   AND A.REMARK = 'M'                                                   \r\n";
            }

            SQL += "ORDER BY A.ACTDATE, C.NAME, A.JEPCODE                                       \r\n";

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

        /// <summary>
        /// 출고된 시약 조회
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argSeqNo"></param>
        /// <param name="argGubun"></param>
        /// <param name="argFDate"></param>
        /// <param name="argTDate"></param>
        /// <returns></returns>
        public DataTable sel_EXAM_SIDTL_Out(PsmhDb pDbCon, string argSeqNo, string argGubun, string argLTDGubun, string argOrderBy,string argFDate = "", string argTDate = "", string argManual = "")
        {
            DataTable dt = null; 

            SQL = "";
            SQL += " SELECT                                                                     \r\n";
            SQL += "  A.ACTDATE,                                                                \r\n";
            SQL += "  A.JEPCODE,                                                                \r\n";
            SQL += "  B.EXAMNAME,                                                               \r\n";
            SQL += "  B.EXAMUNITDTL,                                                            \r\n";
            SQL += "  B.UNIT1,                                                                  \r\n";
            SQL += "  B.UNIT2,                                                                  \r\n";
            SQL += "  B.UNIT3,                                                                  \r\n";
            SQL += "  A.LotNo,                                                                  \r\n";
            SQL += "  A.ExpireDate,                                                             \r\n";
            SQL += "  A.QTY,                                                                    \r\n";
            SQL += "  A.StoreStat,                                                              \r\n";
            SQL += "  A.Abstract,                                                               \r\n";
            SQL += "  C.NAME,                                                                   \r\n";
            SQL += "  A.SEQNO,                                                                  \r\n";
            SQL += "  B.JEPNAMEK,                                                               \r\n";
            SQL += "  A.ROWID                                                                   \r\n";
            SQL += "FROM " + ComNum.DB_MED + "EXAM_SIDTL A,                                     \r\n";
            SQL += ComNum.DB_ERP + "DRUG_JEP B,                                                 \r\n";
            SQL += ComNum.DB_ERP + "AIS_LTD C                                                   \r\n";
            SQL += "WHERE 1=1                                                                   \r\n";
            SQL += "       AND A.JEPCODE = B.JEPCODE                                            \r\n";
            SQL += "       AND B.JGELCODE = C.LTDCODE                                            \r\n";
            if (argFDate != "" && argTDate != "'")
            {
                SQL += "       AND A.ACTDATE >= TO_DATE('" + argFDate + "', 'YYYY-MM-DD')                           \r\n";
                SQL += "       AND A.ACTDATE <= TO_DATE('" + argTDate + " 23:59:59', 'YYYY-MM-DD HH24:MI:SS')       \r\n";
            }
            SQL += "       AND A.GBNEW IS NOT NULL                                                   \r\n";
            SQL += "       AND A.GBN = 'C'                                                      \r\n";

            if (argManual != "")
            {
                SQL += "       AND A.REMARK = 'M'                                               \r\n";
            }
            if (argSeqNo != "")
            {
                SQL += "   AND A.SEQNO = '" + argSeqNo + "'                                     \r\n";
            }
            if (argGubun != "")
            {
                SQL += "   AND B.EXAMGUBUN = '" + argGubun + "'                                 \r\n";
            }
            if (argLTDGubun != "")
            {
                SQL += "   AND C.LTDCODE = '" + argLTDGubun + "'                                \r\n";
            }
            if (argOrderBy != "")
            {
                SQL += "ORDER BY PRINTRANK DESC                                                 \r\n";
            }
            else
            {
                SQL += "ORDER BY A.ENTDATE DESC                                                 \r\n";
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

        /// <summary>
        /// 입,출고 현황 조회
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argSeqNo"></param>
        /// <param name="argGubun"></param>
        /// <param name="argFDate"></param>
        /// <param name="argTDate"></param>
        /// <returns></returns>
        public DataTable sel_EXAM_SIDTL_IO(PsmhDb pDbCon, string argGubun, string argJob, string argFDate = "", string argTDate = "")
        {
            DataTable dt = null;

            if (argJob == "00")
            {
                SQL = "";
                SQL += " SELECT                                                                                         \r\n";
                SQL += "  A.SINAME, A.JEPCODE, A.REMARK, QTY_IWOL, QTY_IPGO, QTY_CHLGO,                                 \r\n";
                SQL += "  QTY_IWOL + QTY_IPGO - QTY_CHLGO JEGO , A.QTY_JEGO, A.ROWID,                                   \r\n";
                SQL += "  B.UNIT1 || ' ' || B.UNIT2 || '*' || B.UNIT3 as UNIT                                           \r\n";
                SQL += "FROM " + ComNum.DB_MED + "EXAM_SIJEGO_NEW A,                                                    \r\n";
                SQL += ComNum.DB_ERP + "DRUG_JEP B                                                                      \r\n";
                SQL += "WHERE 1=1                                                                                       \r\n";
                SQL += "       AND A.YYMM  = '" + argFDate + "'                                                         \r\n";
                if (argGubun != "")
                {
                    SQL += "   AND B.EXAMGUBUN = '" + argGubun + "'                                                     \r\n";
                }
                SQL += "  AND (B.DELDATE IS NULL OR B.DELDATE = '')                                                     \r\n";
                SQL += "  AND A.JEPCODE = B.JEPCODE(+)                                                                  \r\n";

                //SQL += "  AND b.DELDATE IS NULL                                                                         \r\n";

                SQL += "ORDER BY A.JEPCODE, A.SINAME, B.EXAMRANK                                                        \r\n";
            }

            else if (argJob == "01")
            {
                SQL = "";
                SQL += " SELECT                                                                                         \r\n";
                SQL += "  A.JEPCODE,                                                                                    \r\n";
                SQL += "  B.EXAMNAME,                                                                                   \r\n";
                SQL += "  B.UNIT1 || ' ' || B.UNIT2 || '*' || B.UNIT3 as UNIT,                                          \r\n";
                SQL += "  SUM(DECODE(A.GBN,'I',A.QTY,0)) JepIn,                                                         \r\n";
                SQL += "  SUM(DECODE(A.GBN,'C',A.QTY,0)) JepOut,                                                        \r\n";
                SQL += "  SUM(DECODE(A.GBN,'I',A.QTY,0)) - SUM(DECODE(A.GBN,'C',A.QTY,0)) JepSave                       \r\n"; 
                SQL += "FROM " + ComNum.DB_MED + "EXAM_SIDTL A,                                                         \r\n";
                SQL += ComNum.DB_ERP + "DRUG_JEP B                                                                      \r\n";
                SQL += "WHERE 1=1                                                                                       \r\n";
                SQL += "       AND A.JEPCODE = B.JEPCODE                                                                \r\n";
                SQL += "       AND A.GBNEW = 'Y'                                                                        \r\n";
                //SQL += "       AND A.GBNEW IS NULL                                                                     \r\n";
                if (argFDate != "" && argTDate != "'")
                {
                    SQL += "       AND A.ACTDATE >= TO_DATE('" + argFDate + "', 'YYYY-MM-DD')                           \r\n";
                    SQL += "       AND A.ACTDATE <= TO_DATE('" + argTDate + " 23:59:59', 'YYYY-MM-DD HH24:MI:SS')       \r\n";
                }
                if (argGubun != "")
                {
                    SQL += "   AND B.EXAMGUBUN = '" + argGubun + "'                                                     \r\n";
                }
                SQL += "GROUP BY A.JEPCODE, B.EXAMNAME, B.UNIT1, B.UNIT2, B.UNIT3                                       \r\n";
                SQL += "ORDER BY A.JEPCODE                                                                              \r\n";
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

        /// <summary>
        /// 제약회사명을 읽어온다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argJepCode"></param>
        /// <returns></returns>
        public string sel_Read_LTDName(PsmhDb pDbCon, string argJepCode)
        {
            DataTable dt = null;
            string rtnVal = "";

            SQL = "";
            SQL += " SELECT                                                                     \r\n";
            SQL += "  B.NAME                                                                    \r\n";            
            SQL += "FROM " + ComNum.DB_ERP + "DRUG_JEP A,                                       \r\n";
            SQL += ComNum.DB_ERP + "AIS_LTD B                                                   \r\n";
            SQL += "WHERE 1=1                                                                   \r\n";
            SQL += "       AND A.JEPCODE = '" + argJepCode + "'                                 \r\n";
            SQL += "       AND A.GELCODE = B.LTDCODE                                            \r\n";            

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                if(dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rtnVal;
        }

        public DataTable sel_Company_List(PsmhDb pDbCon, string argPart)
        {
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  LTDCODE, NAME                                                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "AIS_LTD                                             ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "      AND LTDCODE IN (                                                        ";
            SQL += ComNum.VBLF + "                      SELECT GELCODE                                          ";
            SQL += ComNum.VBLF + "                      FROM " + ComNum.DB_ERP + "DRUG_JEP                      ";
            SQL += ComNum.VBLF + "                      WHERE 1=1                                               ";
            if(argPart != "")
            {
                SQL += ComNum.VBLF + "                      AND EXAMGUBUN = '" + argPart + "'                   ";
            }            
            SQL += ComNum.VBLF + "                      AND JEPCODE IN (SELECT JEPCODE                          ";
            SQL += ComNum.VBLF + "                                      FROM " + ComNum.DB_MED + "EXAM_SIDTL    ";
            SQL += ComNum.VBLF + "                                      WHERE 1=1                               ";
            SQL += ComNum.VBLF + "                                      AND GBNEW = 'Y'                         ";
            SQL += ComNum.VBLF + "                                      AND GBN = 'I'                           ";
            SQL += ComNum.VBLF + "                                      GROUP BY JEPCODE)                       ";
            SQL += ComNum.VBLF + "                      GROUP BY GELCODE)                                       ";
            SQL += ComNum.VBLF + "ORDER BY NAME                                                                 ";

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

        public DataTable sel_Exam_Sidtl_List(PsmhDb pDbCon, string argGubun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  A.JEPCODE, A.EXAMGUBUN,  A.EXAMNAME, A.EXAMRANK,                            ";
            SQL += ComNum.VBLF + "  A.UNIT1, A.UNIT2, A.UNIT3, A.DELDATE, A.GELCODE,                            ";
            SQL += ComNum.VBLF + "  A.GELBARCODE, A.ROWID, B.NAME                                               ";            
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_JEP A, " + ComNum.DB_ERP + "AIS_LTD B          ";            
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "      AND A.EXAMGUBUN IS NOT NULL                                             ";
            SQL += ComNum.VBLF + "      AND A.EXAMGUBUN = '" + argGubun + "'                                    ";
            //2019-05-11 안정수, 우수희t 요청으로 면역학에서 한일진단 제외처리 
            if(argGubun == "02")
            {
                SQL += ComNum.VBLF + "  AND B.LTDCODE <> '1637'                                                 ";
            }
            SQL += ComNum.VBLF + "      AND A.JGELCODE = B.LTDCODE(+)                                           ";
            SQL += ComNum.VBLF + "      AND A.DELDATE IS NULL                                                   ";
            SQL += ComNum.VBLF + "ORDER BY EXAMRANK                                                             ";

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

        /// <summary>
        /// EXAM_SIDTL 테이블에서 출고순서(PrintRank)를 읽어온다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argJepCode"></param>
        /// <returns></returns>
        public int sel_Sidtl_PrintRank(PsmhDb pDbCon, string argJobDate) 
        {
            DataTable dt = null;
            int rtnVal = 0;

            SQL = "";
            SQL += " SELECT                                                                     \r\n";
            SQL += "  MAX(PRINTRANK) AS PRINTRANK                                               \r\n";
            SQL += "FROM " + ComNum.DB_MED + "EXAM_SIDTL                                        \r\n";            
            SQL += "WHERE 1=1                                                                   \r\n";
            SQL += "       AND GBNEW = 'Y'                                                      \r\n";
            SQL += "       AND GBN = 'C'                                                        \r\n";
            SQL += "       AND ACTDATE >= TO_DATE('" + argJobDate + "', 'YYYY-MM-DD')           \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {                    
                    rtnVal = Convert.ToInt32(VB.Val(dt.Rows[0]["PRINTRANK"].ToString().Trim()) + 1);                    
                }

                dt.Dispose();
                dt = null;

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            return rtnVal;
        }


        #endregion

        #region 트랜잭션(up, ins, del 모음)


        /// <summary>
        /// 해당 시약의 입출고
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCls"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string ins_EXAM_SIDTL(PsmhDb pDbCon, cSupLbExSIDTL argCls, ref int intRowAffected) 
        {
            string SqlErr = "";
            
            SQL = " INSERT INTO " + ComNum.DB_MED + "EXAM_SIDTL                                             \r\n";
            SQL += "   (ActDate,JEPCODE,GBN,QTY,ENTDATE                                                     \r\n";
            SQL += "    ,PART,SEQNO,LOTNO,EXPIREDATE,STORESTAT                                              \r\n";
            SQL += "    ,ABSTRACT, GBNEW, INPT_DT, REMARK, PRINTRANK) VALUES                                \r\n";
            SQL += "   (                                                                                    \r\n";
            SQL += "      TO_DATE('" + argCls.ACTDATE + "', 'YYYY-MM-DD HH24:MI:ss')                        \r\n";            
            SQL += "     ,'" + argCls.JEPCODE + "'                                                          \r\n";            
            SQL += "     ,'" + argCls.GBN + "'                                                              \r\n";

            //2019-06-24 안정수, 우수희 팀장요청으로 HS-CHEK 항목은 10개씩 입고 및 출고되도록 변경
            if (argCls.JEPCODE == "HS-CHEK" || argCls.JEPCODE == "SJ-E0235" || argCls.JEPCODE == "SJ-X0019")
            {
                SQL += " ,'10'                                                                              \r\n";
            }
            else if (argCls.JEPCODE == "GW-INFORM")
            {
                SQL += " ,'10'                                                                              \r\n";
            }
            //else if (argCls.JEPCODE == "GW-PERFO")
            //{
            //    SQL += " ,'6'                                                                              \r\n";
            //}
            else if (argCls.JEPCODE == "MD-FIF20")
            {
                SQL += " ,'25'                                                                              \r\n";
            }
            else if (argCls.JEPCODE == "MD-FIF7")
            {
                SQL += " ,'12'                                                                              \r\n";
            }
            else
            {
                SQL += " ,'1'                                                                               \r\n";
            }            

            SQL += "     ,TO_DATE('" + argCls.SYSDATE + "', 'YYYY-MM-DD HH24:MI:ss')                        \r\n";
            SQL += "     ,'" + argCls.PART + "'                                                             \r\n";
            SQL += "     ,'" + argCls.SEQNO + "'                                                            \r\n";
            SQL += "     ,'" + argCls.LOTNO + "'                                                            \r\n";
            SQL += "     ,TO_DATE('" + argCls.EXPIREDATE + "', 'YYYY-MM-DD')                                \r\n";
            SQL += "     ,'" + argCls.STORESTAT + "'                                                        \r\n";
            SQL += "     ,'" + argCls.ABSTRACT + "'                                                         \r\n";
            SQL += "     ,'" + argCls.GBNEW + "'                                                            \r\n";
            SQL += "     ,SYSDATE                                                                           \r\n";
            SQL += "     ,'" + argCls.REMARK + "'                                                           \r\n";
            SQL += "     ,'" + argCls.OutSeqno + "'                                                         \r\n";
            SQL += "   )                                                                                    \r\n";  


            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 250);

            return SqlErr;
        }

        /// <summary>
        /// 변경사항 update이나 실제 쓰이지 않음
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCls"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_EXAM_SIDTL(PsmhDb pDbCon, cSupLbExSIDTL argCls, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = " UPDATE " + ComNum.DB_MED + "EXAM_SIDTL                                                  \r\n";
            SQL += "   SET                                                                                  \r\n";            
            SQL += "      '" + argCls.ACTDATE + "'                                                          \r\n";
            SQL += "     ,'" + argCls.JEPCODE + "'                                                          \r\n";
            SQL += "     ,'" + argCls.GBN + "'                                                              \r\n";
            //SQL += "     ,'" + argCls.QTY + "'                                                              \r\n";
            SQL += "     ,'1'                                                                               \r\n";
            SQL += "     ,SYSDATE                                                                           \r\n";
            SQL += "     ,'" + argCls.PART + "'                                                             \r\n";
            SQL += "     ,'" + argCls.SEQNO + "'                                                            \r\n";
            SQL += "     ,'" + argCls.LOTNO + "'                                                            \r\n";
            SQL += "     ,'" + argCls.EXPIREDATE + "'                                                       \r\n";
            SQL += "     ,'" + argCls.STORESTAT + "'                                                        \r\n";
            SQL += "     ,'" + argCls.ABSTRACT + "'                                                         \r\n";
            SQL += "     ,'" + argCls.GBNEW + "'                                                            \r\n";
            SQL += "     ,SYSDATE                                                                           \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 250);

            return SqlErr;
        }

        public string up_EXAM_SIDTL_M(PsmhDb pDbCon, cSupLbExSIDTL argCls, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = " UPDATE " + ComNum.DB_MED + "EXAM_SIDTL                                                  \r\n";
            SQL += "   SET                                                                                  \r\n";            
            SQL += "      QTY = " + argCls.QTY + "                                                          \r\n";
            SQL += "     ,LOTNO = '" + argCls.LOTNO + "'                                                    \r\n";
            SQL += "     ,EXPIREDATE = TO_DATE('" + argCls.EXPIREDATE + "', 'YYYY-MM-DD')                   \r\n";                        
            SQL += "     ,STORESTAT = '" + argCls.STORESTAT + "'                                            \r\n";
            SQL += "     ,ABSTRACT = '" + argCls.ABSTRACT + "'                                              \r\n";
            SQL += "     ,UPPS = '" + clsType.User.IdNumber + "'                                            \r\n";
            SQL += "     ,UP_DT = SYSDATE                                                                   \r\n";                                                                                           
            SQL += "WHERE 1=1                                                                               \r\n";
            SQL += "        AND ROWID = '" + argCls.ROWID + "'                                              \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 250);

            return SqlErr;
        }

        /// <summary>
        /// 출고된 시약 취소처리
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argRowid"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string del_EXAM_SIDTL(PsmhDb pDbCon, string argRowid, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = " DELETE " + ComNum.DB_MED + "EXAM_SIDTL                                                  \r\n";
            SQL += "   WHERE ROWID = '" + argRowid + "'                                                     \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 250);

            return SqlErr;
        }

        public string up_EXAM_SIJEGO_NEW(PsmhDb pDbCon, int argIWOL, int argIPGO, int argCHLGO, int argJAEGO, string argRowid, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = " UPDATE " + ComNum.DB_MED + "EXAM_SIJEGO_NEW                                             \r\n";
            SQL += "   SET                                                                                  \r\n";
            SQL += "    QTY_IWOL  = " + argIWOL + "                                                         \r\n";
            SQL += "   ,QTY_IPGO  = " + argIPGO + "                                                         \r\n";
            SQL += "   ,QTY_CHLGO = " + argCHLGO + "                                                        \r\n";
            SQL += "   ,QTY_JEGO  = " + argJAEGO + "                                                        \r\n";
            SQL += "   WHERE ROWID = '" + argRowid + "'                                                     \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 250);

            return SqlErr;
        }

        #endregion


    }
}
