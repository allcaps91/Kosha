using System;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Data;

namespace ComSupLibB.SupDrst
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupDrst 
    /// File Name       : clsComSupDrstSQL.cs
    /// Description     : 진료지원 공통 약제 쿼리 관련 class
    /// Author          : 윤조연
    /// Create Date     : 2018-11-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>    
    public class clsComSupDrstSQL
    {
        clsQuery Query = new clsQuery();

        string SQL = "";
        string SqlErr = ""; //에러문 받는 변수

        
        
        #region 변수 class 

        public class cDurSend
        {
            public string Job = "";
            public string Gubun = "";
            public string STS = "";
            public string fDate = "";
            public string tDate = "";
            public string Dept = "";
                                    
        }

        #endregion


        public DataTable sel_DurSend(PsmhDb pDbCon, cDurSend argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                                                             \r\n";            
            SQL += "        ENTDATE, ENTTIME, GBN, TUYAKNO, PANO, JUMIN1, JUMIN3, SNAME                                                                                 \r\n";
            SQL += "        , BI, DEPTCODE, DRNAME, DRBUNHO, ENTDATE2, DRCODE                                                                                           \r\n";
            SQL += "        ,KOSMOS_OCS.FC_DUR_MASER_CHK(PANO,ENTDATE,DEPTCODE,DRCODE,'0') FC_DUR_MASER_CHK                                                             \r\n";
            SQL += "        ,KOSMOS_OCS.FC_DUR_MASER_CHK(PANO,ENTDATE,DEPTCODE,DRCODE,TUYAKNO) FC_DUR_MASER_CHK2                                                        \r\n";
            SQL += "    FROM   (                                                                                                                                        \r\n";
            if (argCls.Gubun =="퇴원약" || argCls.Gubun == "응급실")
            {
                SQL += "    SELECT TO_CHAR(A.BDATE, 'YYYY-MM-DD') ENTDATE, TO_CHAR(A.ENTDATE, 'HH24:MI:SS') ENTTIME                                                     \r\n";
                SQL += "    ,DECODE(A.GBIO, '3','퇴원약','응급실') GBN, A.TUYAKNO, A.PANO, B.JUMIN1, B.JUMIN3, B.SNAME, D.BI, A.DEPTCODE, C.DRNAME, C.DRBUNHO           \r\n";
                SQL += "    ,TO_CHAR(D.BDATE, 'YYYY-MM-DD') ENTDATE2, C.DRCODE                                                                                          \r\n";
                SQL += "  FROM KOSMOS_OCS.OCS_DRUGATC A, KOSMOS_PMPA.BAS_PATIENT B, KOSMOS_OCS.OCS_DOCTOR C, KOSMOS_OCS.OCS_IORDER D                                    \r\n";
                SQL += "   WHERE 1=1                                                                                                                                    \r\n";
                SQL += "    AND A.BDATE >= TO_DATE('" + argCls.fDate + "','YYYY-MM-DD')                                                                                 \r\n";
                SQL += "    AND A.BDATE <= TO_DATE('" + argCls.tDate + "','YYYY-MM-DD')                                                                                 \r\n";
                SQL += "    AND A.SENDKEY = 'Y'                                                                                                                         \r\n";
                SQL += "     AND A.PANO = B.PANO                                                                                                                        \r\n";
                if (argCls.Gubun == "응급실")
                {
                    SQL += "     AND A.DRCODE = C.DRCODE                                                                                                                \r\n";
                    SQL += "     AND A.DEPTCODE = 'ER'                                                                                                                  \r\n";                    
                }
                else
                {
                    SQL += "     AND A.DRCODE = C.SABUN                                                                                                                 \r\n";
                    SQL += "     AND D.GBTFLAG = 'T'                                                                                                                    \r\n";
                }
                SQL += "     AND A.PANO = D.PTNO                                                                                                                        \r\n";
                SQL += "     AND A.ORDERNO = D.ORDERNO                                                                                                                  \r\n";
                SQL += "     AND A.BDATE = D.BDATE                                                                                                                      \r\n";
                SQL += "     GROUP BY TO_CHAR(A.BDATE, 'YYYY-MM-DD'), TO_CHAR(A.ENTDATE, 'HH24:MI:SS')                                                                  \r\n";
                SQL += "              ,DECODE(A.GBIO, '3','퇴원약','응급실'), A.TUYAKNO, A.PANO, B.JUMIN1, B.JUMIN3, B.SNAME, D.BI, A.DEPTCODE, C.DRNAME, C.DRBUNHO     \r\n";
                SQL += "              ,TO_CHAR(D.BDATE, 'YYYY-MM-DD'),  C.DRCODE                                                                                        \r\n";

            }
            else if (argCls.Gubun == "외래약" )
            {
                SQL += "    SELECT TO_CHAR(A.BDATE, 'YYYY-MM-DD') ENTDATE, TO_CHAR(A.ENTDATE, 'HH24:MI:SS') ENTTIME                                                     \r\n";
                SQL += "    ,DECODE(A.GBIO, '3','퇴원약','외래약') GBN, A.TUYAKNO, A.PANO, B.JUMIN1, B.JUMIN3, B.SNAME, D.BI, A.DEPTCODE, C.DRNAME, C.DRBUNHO           \r\n";
                SQL += "    ,TO_CHAR(D.BDATE, 'YYYY-MM-DD') ENTDATE2, C.DRCODE                                                                                          \r\n";
                SQL += "  FROM KOSMOS_OCS.OCS_DRUGATC A, KOSMOS_PMPA.BAS_PATIENT B, KOSMOS_OCS.OCS_DOCTOR C, KOSMOS_OCS.OCS_IORDER D                                    \r\n";
                SQL += "   WHERE 1=1                                                                                                                                    \r\n";
                SQL += "    AND A.BDATE >= TO_DATE('" + argCls.fDate + "','YYYY-MM-DD')                                                                                 \r\n";
                SQL += "    AND A.BDATE <= TO_DATE('" + argCls.tDate + "','YYYY-MM-DD')                                                                                 \r\n";
                SQL += "    AND A.SENDKEY = 'Y'                                                                                                                         \r\n";
                SQL += "     AND A.PANO = B.PANO                                                                                                                        \r\n";
                SQL += "     AND A.GBIO IN ('1','3')                                                                                                                    \r\n";
                SQL += "     AND A.DRCODE = C.DRCODE                                                                                                                    \r\n";
                SQL += "     AND A.PANO = B.PANO                                                                                                                        \r\n";
                SQL += "     AND A.PANO = D.PTNO                                                                                                                        \r\n";
                if (argCls.Dept != "**")
                {
                    SQL += "     AND A.DeptCode = '" + argCls.Dept + "'                                                                                                 \r\n";
                }
                SQL += "     AND A.ORDERNO = D.ORDERNO                                                                                                                  \r\n";
                SQL += "     AND A.BDATE = D.BDATE                                                                                                                      \r\n";
                SQL += "     GROUP BY TO_CHAR(A.BDATE, 'YYYY-MM-DD'), TO_CHAR(A.ENTDATE, 'HH24:MI:SS')                                                                  \r\n";
                SQL += "              ,DECODE(A.GBIO, '3','퇴원약','외래약'), A.TUYAKNO, A.PANO, B.JUMIN1, B.JUMIN3, B.SNAME, D.BI, A.DEPTCODE, C.DRNAME, C.DRBUNHO     \r\n";
                SQL += "              ,TO_CHAR(D.BDATE, 'YYYY-MM-DD'),  C.DRCODE                                                                                        \r\n";

            }


            SQL += "         ) MST                                                                                                                                      \r\n";

            if (argCls.Gubun =="퇴원약")
            {
                SQL += "   WHERE MST.GBN IN ('퇴원약')                                                                                                                  \r\n";
            }
            else if (argCls.Gubun == "외래약")
            {
                SQL += "   WHERE MST.GBN IN ('외래약')                                                                                                                  \r\n";
            }
            else if(argCls.Gubun == "응급실")
            {
                SQL += "   WHERE MST.GBN IN ('응급실')                                                                                                                  \r\n";
            }
            else
            {
                SQL += "   WHERE MST.GBN IN ('퇴원약','외래약')                                                                                                         \r\n";
            }

            if (argCls.STS == "전송" || argCls.STS == "미전송")
            {
                if (argCls.STS == "전송")
                {
                    SQL += "   AND EXISTS                                                                                                                               \r\n";                    

                }
                else if (argCls.STS == "미전송")
                {
                    SQL += "   AND NOT EXISTS                                                                                                                           \r\n";
                }
                SQL += "   ( SELECT * FROM KOSMOS_PMPA.DUR_MASTER SUB                                                                                                   \r\n";
                SQL += "   WHERE SUB.BDATE = TO_CHAR(MST.ENTDATE)                                                                                                       \r\n";
                SQL += "   AND SUB.PANO = MST.PANO                                                                                                                      \r\n";
                SQL += "   AND SUB.DRUG = '1')                                                                                                                          \r\n";
            }
            

            SQL += "   ORDER BY  ENTDATE, TUYAKNO ASC                                                                                                                   \r\n";


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

        public DataTable sel_DurSend_Detail(PsmhDb pDbCon, string argGbn, string argPano,string argBDate,string argTuyak)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                     \r\n";
            SQL += "        A.SUCODE, A.QTY, A.NAL, B.GBDIV, B.DOSNAME                                          \r\n";           
            if (argGbn == "병실약" )
            {
                SQL += "    , (SELECT GBSELF                                                                    \r\n";
                SQL += "    FROM KOSMOS_PMPA.IPD_NEW_SLIP                                                       \r\n";
                SQL += "     WHERE PANO = A.PANO                                                                \r\n";
                SQL += "        AND ORDERNO = A.ORDERNO GROUP BY GBSELF    HAVING SUM(QTY * NAL) > 0            \r\n";                
                SQL += "    ) GBSELF                                                                            \r\n";
            }
            else 
            {
                SQL += "    , (SELECT GBSELF                                                                    \r\n";
                SQL += "    FROM KOSMOS_PMPA.OPD_SLIP                                                           \r\n";
                SQL += "     WHERE PANO = A.PANO                                                                \r\n";
                SQL += "        AND ORDERNO = A.ORDERNO GROUP BY GBSELF    HAVING SUM(QTY * NAL) > 0            \r\n";
                SQL += "    ) GBSELF                                                                            \r\n";
            }

            SQL += "      FROM KOSMOS_OCS.OCS_DRUGATC A, KOSMOS_OCS.OCS_ODOSAGE B                               \r\n";
            SQL += "       WHERE a.Pano = '" + argPano + "'                                                     \r\n";
            SQL += "        AND a.BDate = TO_DATE('" + argBDate + "','YYYY-MM-DD')                             \r\n";
            SQL += "        AND A.TUYAKNO = '" + argTuyak + "'                                                \r\n";
            SQL += "        AND A.DOSCODE = B.DOSCODE                                                           \r\n";

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

        public DataTable sel_DurSend_EDI_SUGA(PsmhDb pDbCon, string argSuga)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                 \r\n";
            SQL += "        A.SUGBS, B.CODE,  B.SCODE, B.JONG, B.PNAME                      \r\n";            
            SQL += "      FROM KOSMOS_PMPA.BAS_SUN A, KOSMOS_PMPA.EDI_SUGA B                \r\n";
            SQL += "       WHERE A.SUNEXT = '" + argSuga + "'                               \r\n";
            SQL += "       WHERE B.SCODE IS NOT NULL                                        \r\n";
            SQL += "        AND A.BCODE IS NOT NULL                                         \r\n";
            SQL += "        AND A.BCODE = B.CODE                                            \r\n";

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

        #region 트랜잭션 쿼리 + INSERT, UPDATE,DELETE .... 

        #endregion
    }
}
