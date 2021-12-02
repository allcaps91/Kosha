using System;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray 
    /// File Name       : clsComSupXraySQL.cs
    /// Description     : 진료지원 공통 영상의학과 쿼리 관련 class
    /// Author          : 윤조연
    /// Create Date     : 2017-06-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    ///     
    /// </history>    
    public class clsComSupXraySQL
    {
        clsQuery Query = new clsQuery();

        string SQL = ""; 
        string SqlErr = ""; //에러문 받는 변수
        public string[] argstr = null; //쿼리에 사용될 변수 배열값
        //clsComSupXrayRead 참조하지마세요

        #region //영상의학과 기초관련
        /// <summary>
        /// 영상의학과 기본사용량 등록에 사용되는 재료구분 찾기
        /// </summary>
        /// <param name="argCode"></param>
        /// <param name="Gubun"></param>
        /// <param name="bFull"></param>
        /// <returns></returns>
        public string read_Xray_Use_Gubun(PsmhDb pDbCon, string argCode, string argFull = "B", string Gubun = "XRAY_USE_GUBUN")
        {

            if (argCode == "") return "";

            DataTable dt = Query.Get_BasBcode(pDbCon, Gubun, argCode, "");
            if (dt != null && dt.Rows.Count > 0)
            {
                if (argFull == "A")
                {
                    return dt.Rows[0]["Code"].ToString().Trim();
                }
                else if (argFull == "B")
                {
                    return dt.Rows[0]["Name"].ToString().Trim();
                }
                else if (argFull == "C")
                {
                    return dt.Rows[0]["Code"].ToString().Trim() + "." + dt.Rows[0]["Name"].ToString().Trim();
                }
                else
                {
                    return dt.Rows[0]["Name"].ToString().Trim();
                }

            }
            else
            {
                return "";
            }

        }
                

        /// <summary>
        /// xray_use 테이블 쿼리
        /// </summary>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public DataTable sel_Xray_Use(PsmhDb pDbCon, string argDate)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                                         \r\n";
                SQL += "  MgrNo cSeqNo,Qty cQty                                                         \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_USE                                          \r\n";
                SQL += "  WHERE 1 = 1                                                                   \r\n";
                SQL += "    AND SeekDate >= TO_DATE('" + argDate + "', 'YYYY-MM-DD')                    \r\n";
                SQL += "    AND SeekDate <= TO_DATE('" + argDate + " 23:59', 'YYYY-MM-DD HH24:MI')      \r\n";
                SQL += "    AND MCODE IN ('HPS','INWON1','INWON2','INWON3')                             \r\n";
                SQL += "  ORDER BY MgrNo                                                                \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        /// <summary>
        /// xray_use, xray_mcode 사용하는 쿼리
        /// </summary>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public DataTable sel_Xray_Use_Jaje_Tong(PsmhDb pDbCon, string argDate)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                                         \r\n";
                SQL += "  M.GbMCode,U.MCode,U.GbUse,M.Mname,SUM(U.Qty) Qty1                             \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_USE u, " + ComNum.DB_PMPA + "XRAY_MCODE m    \r\n";
                SQL += "  WHERE 1 = 1                                                                   \r\n";
                SQL += "    AND SeekDate >= TO_DATE('" + argDate + "', 'YYYY-MM-DD')                    \r\n";
                SQL += "    AND SeekDate <= TO_DATE('" + argDate + " 23:59', 'YYYY-MM-DD HH24:MI')      \r\n";
                SQL += "    AND U.Mcode NOT IN ('INWON1','INWON2','INWON3')                             \r\n";
                SQL += "    AND U.Mcode = M.Mcode                                                       \r\n";
                SQL += "   GROUP BY M.GbMCode,U.MCode,U.GbUse,M.Mname                                   \r\n";
                SQL += "   ORDER BY M.GbMCode,U.MCode,U.GbUse                                           \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        /// <summary>
        /// 영상의학과 기사통계 사용 쿼리
        /// </summary>
        /// <param name="argIO"></param>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public DataTable sel_Xray_Use_Gisa_Tong(PsmhDb pDbCon, string argIO, string argDate)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                                         \r\n";
                SQL += "  D.ExID,SUM(U.QTY) Cnt1                                                        \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_USE u, " + ComNum.DB_PMPA + "XRAY_DETAIL d   \r\n";
                SQL += "  WHERE 1 = 1                                                                   \r\n";
                SQL += "    AND u.SeekDate >= TO_DATE('" + argDate + "', 'YYYY-MM-DD')                  \r\n";
                SQL += "    AND u.SeekDate <= TO_DATE('" + argDate + " 23:59', 'YYYY-MM-DD HH24:MI')    \r\n";
                SQL += "    AND U.GbUse > 0                                                             \r\n";
                SQL += "    AND U.MgrNo = D.MgrNo                                                       \r\n";
                SQL += "    AND (D.GbHIC IS NULL OR D.GbHIC <> 'Y')                                     \r\n";
                if (argIO != "") SQL += "    AND d.IpdOpd ='" + argIO + "'                              \r\n";
                SQL += "   GROUP BY ExID                                                                \r\n";
                SQL += "   ORDER BY ExID                                                                \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        /// <summary>
        /// xray_subcode 테이블 쿼리
        /// </summary>
        /// <param name="argClass"></param>
        /// <param name="argSub"></param>
        /// <returns></returns>
        public DataTable sel_Xray_SubClass(PsmhDb pDbCon, string argClass, string argSub, bool sub00 = false)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                                                             \r\n";
                SQL += "  SubCode || '.' || SubName Code ,ClassCode ,SubCode ,SubName,ROWID                                 \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_SubClass                                                         \r\n";
                SQL += "  WHERE 1 = 1                                                                                       \r\n";
                SQL += "    AND ClassCode = '" + ComFunc.SetAutoZero(clsComSup.setP(argClass, ".", 1).Trim(), 2) + "'     \r\n";
                if (argSub != "") SQL += "    AND SubCode = '" + ComFunc.SetAutoZero(argSub, 2) + "'                         \r\n";
                if (sub00) SQL += "    AND SubCode <> '00'                                                                  \r\n";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        /// <summary>
        /// xray_subclass + xray_class 쿼리
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public DataTable sel_Xray_SubClass(PsmhDb pDbCon, string argCode)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                                         \r\n";
                SQL += "  a.SubCode, a.SubName, b.ClassName, a.ClassCode,a.ROWID                        \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_SUBCLASS a,                                  \r\n";
                SQL += "   " + ComNum.DB_PMPA + "XRAY_CLASS b                                           \r\n";
                SQL += "  WHERE 1 = 1                                                                   \r\n";
                SQL += "   AND a.ClassCode = b.ClassCode                                                \r\n";
                if (clsComSup.setP(argCode, ".", 1).Trim() != "**")
                {
                    SQL += "   AND a.ClassCode = '" + clsComSup.setP(argCode, ".", 1).Trim() + "'    \r\n";
                }

                SQL += "   ORDER BY a.SubCode                                                           \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        /// <summary>
        /// xray_class 쿼리
        /// </summary>
        /// <returns></returns>
        public DataTable sel_Xray_Class_Combo(PsmhDb pDbCon)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                 \r\n";
                SQL += "  ClassCode || '.' || ClassName Code                    \r\n";
                SQL += "  ,ClassCode ,ClassName, ROWID                          \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "Xray_Class                \r\n";
                SQL += "  WHERE 1 = 1                                           \r\n";
                SQL += "   ORDER BY CLASSCODE                                   \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        public DataTable sel_Xray_SubClass_Combo(PsmhDb pDbCon,string argClass,string argSub,string argCol="",string argWhere ="",string argOrderBy ="")
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                 \r\n";
                if (argCol != "")
                {
                    SQL += "   '" + argCol + "'                                 \r\n";
                }
                else
                {
                    SQL += "   SubCode || '.' || SubName Codes                  \r\n";
                    SQL += "  ,ClassCode,SubCode,SubName,ROWID                  \r\n";
                }                                
                SQL += "   FROM " + ComNum.DB_PMPA + "Xray_SubClass             \r\n";
                SQL += "  WHERE 1 = 1                                           \r\n";
                if (argClass !="")
                {
                    SQL += "   AND ClassCode ='" + argClass + "'                \r\n";
                }
                if (argSub != "")
                {
                    SQL += "   AND SubCode ='" + argSub + "'                    \r\n";
                }
                if (argWhere != "")
                {
                    SQL += "   '" + argWhere + "'                               \r\n";
                }
                if (argOrderBy != "")
                {
                    SQL += "   '" + argOrderBy + "'                             \r\n";
                }
                else
                {
                    SQL += "   ORDER BY SubCode                                 \r\n";
                }
                

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        /// <summary>
        /// xray_class 쿼리
        /// </summary>
        /// <param name="argClass"></param>
        /// <returns></returns>
        public DataTable sel_Xray_Class(PsmhDb pDbCon, string argClass)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                     \r\n";
                SQL += "  ClassCode ,ClassName, ROWID                               \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "Xray_Class                    \r\n";
                SQL += "  WHERE 1 = 1                                               \r\n";
                if (argClass != "") SQL += "   AND ClassCode = '" + argClass + "'     \r\n";
                SQL += "   ORDER BY CLASSCODE                                       \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        public DataTable sel_Xray_Code(PsmhDb pDbCon, string argCode, string argClass, bool argDel)
        {
            DataTable dt = null;
            
            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                                     \r\n";
                SQL += "  XCODE, XNAME, SUBCODE,                                                    \r\n";
                SQL += "   DECODE(GbReserved, 'Y', 'Y', 'N') GbReserved,                            \r\n";
                SQL += "  DECODE(EXinfo, '1', 'Left', '2', 'Rigth', '3', 'Both', '기타') EXinfo,    \r\n";
                SQL += "  Remark1,Remark2,Remark3,Remark4,GbDate,BCnt,CCnt,                         \r\n";
                SQL += "  TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,GbPacs,                             \r\n";
                SQL += "  BCNT, CCNT, CLASSCODE, BuCode,ROWID                                       \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_CODE                                     \r\n";
                SQL += "  WHERE 1 = 1                                                               \r\n";
                if (argCode != "") SQL += "   AND XCODE = '" + argCode + "'                         \r\n";
                if (argClass != "**") SQL += "   AND CLASSCODE = '" + argClass + "'                 \r\n";
                if (argDel == true) SQL += "   AND DELDATE IS NULL                                  \r\n";
                SQL += "   ORDER BY XCode                                                           \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        /// <summary>
        /// 영상의학과 기초코드 코드조회 쿼리
        /// </summary>
        /// <returns></returns>
        public DataTable sel_Xray_Code_Sub(PsmhDb pDbCon, string argCode, string argClass, bool argDel)
        {
            DataTable dt = null;
            string strClass = clsComSup.setP(argClass, ".", 1).Trim();

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                                     \r\n";
                SQL += "  XCODE, XNAME, C.SUBCODE,                                                  \r\n";
                SQL += "   DECODE(GbReserved, 'Y', 'Y', 'N') GbReserved,                            \r\n";
                SQL += "  DECODE(EXinfo, '1', 'Left', '2', 'Rigth', '3', 'Both', '기타') EXinfo,    \r\n";
                SQL += "  c.Remark1,c.Remark2,c.Remark3,c.Remark4,c.GbDate,c.BCnt,c.CCnt,           \r\n";
                SQL += "  TO_CHAR(c.DelDate,'YYYY-MM-DD') DelDate,c.GbPacs,                         \r\n";
                SQL += "  BCNT, CCNT, S.SUBNAME, C.CLASSCODE, C.BuCode,c.ROWID,D.NAME               \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_CODE c,                                  \r\n";
                SQL += "   " + ComNum.DB_PMPA + "XRAY_SubCLASS s,                                   \r\n";
                SQL += "   " + ComNum.DB_PMPA + "BAS_BUSE d                                         \r\n";
                SQL += "  WHERE 1 = 1                                                               \r\n";
                if (argCode != "") SQL += "   AND C.XCODE = '" + argCode + "'                       \r\n";
                if (strClass != "**") SQL += "   AND C.CLASSCODE = '" + strClass + "'               \r\n";
                if (argDel == true) SQL += "   AND C.DELDATE IS NULL                                \r\n";
                SQL += "   AND C.CLASSCODE = S.CLASSCODE(+)                                         \r\n";
                SQL += "   AND C.SUBCODE   = S.SUBCODE(+)                                           \r\n";
                SQL += "   AND  C.BUCODE = D.BUCODE(+)                                              \r\n";
                SQL += "   ORDER BY XCode                                                           \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }               

        /// <summary>
        /// 영상의학과 XRAY_MCODE 쿼리
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public DataTable sel_Xray_MCode(PsmhDb pDbCon, string argMCode, string argClass, string Orderby = "", bool bMCodeLike = false)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                                                 \r\n";
                SQL += "  MCode,MName,JepCode,GbMCode,Qty,Unit,PrintRanking,ROWID                               \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_MCODE                                                \r\n";
                SQL += "  WHERE 1 = 1                                                                           \r\n";
                if (bMCodeLike == false)
                {
                    if (argMCode != "") SQL += "   AND MCODE = '" + argMCode + "'                               \r\n";
                }
                else if (bMCodeLike == true)
                {
                    if (argMCode != "") SQL += "   AND UPPER(MCODE) LIKE '%" + argMCode + "%'                          \r\n";
                }
                if (argClass != "") SQL += "   AND GbMcode = '" + clsComSup.setP(argClass, ".", 1).Trim() + "' \r\n";
                if (Orderby != "") SQL += "   ORDER BY " + Orderby + "                                          \r\n";



                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        /// <summary>
        /// 영상의학과 XRAY_CODE 쿼리
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public DataTable sel_Xray_Code(PsmhDb pDbCon, string argCode, string argClass)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                                                     \r\n";
                SQL += "  XCode,XName,ClassCode,SubCode,GBDATE                                                      \r\n";
                SQL += "  ,Remark1,Remark2,Remark3,Remark4                                                          \r\n";
                SQL += "  ,TO_CHAR(DelDate,'YYYY-MM-DD') DELDATE,ROWID                                              \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_CODE                                                     \r\n";
                SQL += "  WHERE 1 = 1                                                                               \r\n";
                if (argCode != "") SQL += "   AND XCODE = '" + argCode + "'                                         \r\n";
                if (argClass != "") SQL += "   AND ClassCode = '" + clsComSup.setP(argClass, ".", 1).Trim() + "'    \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        /// <summary>
        /// 영상의학과 기본사용량 관련 쿼리 xray_basuse + xray_mcode
        /// </summary>
        /// <param name="argCode"></param>
        /// <param name="argClass"></param>
        /// <returns></returns>
        public DataTable sel_Xray_BasUse(PsmhDb pDbCon, string argCode, string argGbn ="")
        {
            DataTable dt = null;

            SQL = ""; 

            try
            {
                SQL = "";
                SQL += " SELECT                                                 \r\n";
                SQL += "  GbXCode2,U.MCode, MName, U.Qty,u.Agree, U.ROWID       \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_BASUSE u,            \r\n";
                SQL += "    " + ComNum.DB_PMPA + "XRAY_MCODE m                  \r\n";
                SQL += "  WHERE 1 = 1                                           \r\n";
                SQL += "   AND U.MCode = M.MCode(+)                             \r\n";
                if (argCode != "")
                {
                    SQL += "   AND u.XCODE = '" + argCode + "'                  \r\n"; 
                }
                if (argGbn !="")
                {
                    SQL += "   AND u.GbXcode2 IN ('" + argGbn + "','0')         \r\n";
                }
                SQL += "   ORDER BY GbXCode2, U.MCode                           \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        public DataTable sel_Xray_Use_MCode(PsmhDb pDbCon, string argDate,int argMrgNo = 0)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                             \r\n";
                SQL += "  U.Mcode, M.Mname, U.Qty MQty, GbUse, U.RowID mRowID               \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_USE u,                           \r\n";
                SQL += "    " + ComNum.DB_PMPA + "XRAY_MCODE m                              \r\n";
                SQL += "  WHERE 1 = 1                                                       \r\n";
                SQL += "   AND U.MCode = M.MCode(+)                                         \r\n";
                if (argDate != "")
                {
                    SQL += "   AND SeekDate  = TO_DATE('" + argDate + "','YYYY-MM-DD')      \r\n";
                }
                if (argMrgNo !=0)
                {
                    SQL += "   AND MgrNo  = " + argMrgNo + "                                \r\n";
                }         
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        /// <summary>
        ///  BAS_BUSE 쿼리
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public DataTable sel_Bas_Buse(PsmhDb pDbCon, string argCode, string argName)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                 \r\n";
                SQL += "  BUCODE, NAME, ROWID                                   \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_BUSE                  \r\n";
                SQL += "  WHERE 1 = 1                                           \r\n";
                if (argCode != "") SQL += "   AND BUCODE = '" + argCode + "'      \r\n";
                if (argName != "") SQL += "   AND Name LIKE '%" + argName + "%' \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        /// <summary>
        ///  영상의 판독 상용결과 등록 쿼리
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public DataTable sel_Ord_Jep(PsmhDb pDbCon, string argJCode)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                     \r\n";
                SQL += "  Jepcode, Jepname, ROWID                                   \r\n";
                SQL += "   FROM " + ComNum.DB_ERP + "ORD_JEP                        \r\n";
                SQL += "  WHERE 1 = 1                                               \r\n";
                if (argJCode != "") SQL += "   AND Jepcode = '" + argJCode + "'     \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        /// <summary>
        ///  영상의 오더판넬 선택 쿼리
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public DataTable sel_OrderCode(PsmhDb pDbCon, string argSlipno, string argsubRate)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                     \r\n";
                SQL += "  OrderCode, OrderName, OrderNameS, GbInput, DispSpace,     \r\n";
                SQL += "  GbInfo,    GbBoth,    Bun,        NextCode,SuCode,        \r\n";
                SQL += "  GbDosage,  SpecCode,  Slipno,     GbImiv,  SubRate,       \r\n";
                SQL += "   ROWID                                                    \r\n";
                SQL += "  FROM " + ComNum.DB_MED + "OCS_ORDERCODE                   \r\n";
                SQL += "  WHERE 1 = 1                                               \r\n";
                if (argSlipno != "") SQL += "   AND Slipno = '" + argSlipno + "'    \r\n";
                SQL += "   AND SubRate <>   '" + argsubRate + "'                    \r\n";
                SQL += "   AND SubRate LIKE '" + VB.Left(argsubRate, 2) + "%'       \r\n";
                SQL += "  ORDER BY Seqno                                            \r\n";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        public DataTable sel_OCS_SUBCODE(PsmhDb pDbCon, string argOrderCode)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                         \r\n";
                SQL += "  SuCode,SubName                                                \r\n";
                SQL += "   ,ROWID                                                       \r\n";
                SQL += "  FROM " + ComNum.DB_MED + "OCS_SUBCODE                         \r\n";
                SQL += "  WHERE 1 = 1                                                   \r\n";
                if (argOrderCode != "")
                {
                    SQL += "   AND OrderCode = '" + argOrderCode + "'                   \r\n";
                }
                //2020-03-11 김욱동 삭제 수가코드 안보이게 수정
                SQL += "   AND DELDATE IS NULL                   \r\n";
                SQL += "  ORDER BY Seqno                                                \r\n";
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        public DataTable sel_DExam_DImage(PsmhDb pDbCon, string argPano, string argPacsNo)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                             \r\n";
                SQL += "  b.RawSize,b.MidSize                                               \r\n";                
                SQL += "  FROM " + ComNum.DB_PACS + "DExam a                                \r\n";
                SQL += "     , " + ComNum.DB_PACS + "DImage b                               \r\n";
                SQL += "  WHERE 1 = 1                                                       \r\n";
                SQL += "   AND a.id=b.DExam                                                 \r\n";
                SQL += "   AND a.Patid ='" + argPano + "'                                   \r\n";
                SQL += "   AND a.SerialNo ='" + argPacsNo + "'                              \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        /// <summary>
        /// 인피니트 팍스 pacsno당 파일사이즈 체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPano"></param>
        /// <param name="argPacsNo"></param>
        /// <returns></returns>
        public double Read_Pacs_Size(PsmhDb pDbCon,string argPano, string argPacsNo)
        {
            double size = 0;
            int i = 0;

            DataTable dt = sel_DExam_DImage(pDbCon, argPano, argPacsNo);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                for ( i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["RawSize"].ToString().Trim()!="")
                    {
                        size += Convert.ToDouble(dt.Rows[i]["RawSize"].ToString().Trim());
                    }
                }

                return size;
            }
            else
            {
                return 0;
            }
                        
        }

        public DataTable sel_BAS_Z300FONT(PsmhDb pDbCon, cBas_Z300Font argCls)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                     \r\n";                
                SQL += "   Z300Code,EngName,ROWID                                   \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "BAS_Z300FONT                   \r\n";
                SQL += "  WHERE 1 = 1                                               \r\n";                
                SQL += "   AND Z300Code >= '" + argCls.Search + "'                  \r\n";                
                SQL += "  ORDER BY Z300Code                                         \r\n";
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }
        
        public DataTable sel_ETC_PCSET(PsmhDb pDbCon, cEtc_PCSet argCls)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                     \r\n";
                SQL += "   Sabun,READ_FLAG1,READ_FLAG2,READ_FLAG3                   \r\n";
                SQL += "   ,READ_FLAG4,READ_FLAG5,READ_FLAG6                        \r\n";
                SQL += "   ,READ_FLAG7,READ_FLAG8,ROWID                             \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "ETC_PCSET                      \r\n";
                SQL += "  WHERE 1 = 1                                               \r\n";
                SQL += "   AND Sabun = " + argCls.Sabun + "                         \r\n";                

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        public cEtc_PCSet set_XRayBase(PsmhDb pDbCon,cEtc_PCSet argCls)
        {
            argCls.Sabun = Convert.ToInt32(clsType.User.Sabun);

            DataTable dt = sel_ETC_PCSET(pDbCon, argCls);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                argCls.FLAG_PRT = dt.Rows[0]["READ_FLAG1"].ToString().Trim();
                argCls.FLAG_CAN = dt.Rows[0]["READ_FLAG2"].ToString().Trim();
                argCls.FLAG_SAVE = dt.Rows[0]["READ_FLAG3"].ToString().Trim();
                argCls.FLAG_VIEW = dt.Rows[0]["READ_FLAG4"].ToString().Trim();
                argCls.FLAG_PACSVIEW = dt.Rows[0]["READ_FLAG5"].ToString().Trim();
                argCls.FLAG_LINK_YN = dt.Rows[0]["READ_FLAG6"].ToString().Trim();
                argCls.FLAG_LINK_READ = dt.Rows[0]["READ_FLAG7"].ToString().Trim();
                argCls.FLAG_LINK_RESULT = dt.Rows[0]["READ_FLAG8"].ToString().Trim();

                argCls.ROWID = dt.Rows[0]["ROWID"].ToString().Trim();
            }            

            return argCls;
        }

        public DataTable sel_OPD_ILLS(PsmhDb pDbCon, string argJob , string argPano,string argDept)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                             \r\n";
                SQL += "   YYMM                                             \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "OPD_ILLS               \r\n";
                SQL += "  WHERE 1 = 1                                       \r\n";
                if (argJob =="00")
                {
                    SQL += "   AND Pano = '" + argPano + "'                 \r\n";
                    SQL += "   AND DeptCode = '" + argDept + "'             \r\n";
                }
                else
                {
                    SQL += "   AND Pano = '" + argPano + "'                 \r\n";
                }

                if (argJob == "00")
                {
                    SQL += "    ORDER BY YYMM DESC                          \r\n";
                }
                else
                {

                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        public DataTable sel_OPD_ILLS(PsmhDb pDbCon, string argJob, string argPano, string argYYMM, string argDept)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                             \r\n";
                SQL += "   a.StartDate,a.IllCode,b.IllNameK                 \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "OPD_ILLS a             \r\n";
                SQL += "     , " + ComNum.DB_PMPA + "BAS_ILLS b             \r\n";
                SQL += "  WHERE 1 = 1                                       \r\n";
                SQL += "   AND a.IllCode = b.IllCode                        \r\n";
                if (argJob == "00")
                {
                    SQL += "   AND a.Pano = '" + argPano + "'               \r\n";
                    SQL += "   AND a.YYMM = '" + argYYMM + "'               \r\n";
                    SQL += "   AND a.DeptCode = '" + argDept + "'           \r\n";
                    
                }
                else
                {
                    SQL += "   AND a.Pano = '" + argPano + "'               \r\n";
                }

                if (argJob == "00")
                {
                    SQL += "    ORDER BY a.StartDate,a.IllCode              \r\n";
                }
                else
                {

                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        public DataTable sel_OCS_IILLS(PsmhDb pDbCon, string argJob, string argPano, string ArgDate)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                         \r\n";
                SQL += "   a.BDate,a.EntDate,a.IllCode,b.IllNameK                       \r\n";
                SQL += "  FROM " + ComNum.DB_MED + "OCS_IILLS a                         \r\n";
                SQL += "     , " + ComNum.DB_PMPA + "BAS_ILLS b                         \r\n";
                SQL += "  WHERE 1 = 1                                                   \r\n";
                SQL += "   AND a.IllCode = b.IllCode                                    \r\n";
                if (argJob == "00")
                {
                    SQL += "   AND a.Ptno = '" + argPano + "'                           \r\n";                    
                    SQL += "   AND a.BDate >= TO_DATE('" + ArgDate + "','YYYY-MM-DD')   \r\n";
                }
                else
                {
                    SQL += "   AND a.Ptno = '" + argPano + "'                           \r\n";
                }

                if (argJob == "00")
                {
                    SQL += "    ORDER BY a.BDate,a.EntDate,a.IllCode                    \r\n";
                }
                else
                {

                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }                               

        public DataTable sel_IPD_NEW_MASTER(PsmhDb pDbCon, string Job, string argPano)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                     \r\n";
            SQL += "   Pano, Sname, Age, Sex, Deptcode,HEIGHT, WEIGHT                           \r\n";
            SQL += "   ,DrCode, Wardcode, Roomcode,Bi                                           \r\n";
            SQL += "   ,TO_CHAR(InDate,'YYYY-MM-DD') InDate                                     \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                 \r\n";
            SQL += "   WHERE 1 = 1                                                              \r\n";
            if (Job == "00")
            {
                SQL += "    AND Pano = '" + argPano + "'                                        \r\n";
                SQL += "    AND AmSet6 = ' '                                                    \r\n";
                SQL += "    AND (GBSTS = '0' OR OUTDATE = TRUNC(SYSDATE))                       \r\n";
                SQL += "    AND ACTDATE IS NULL                                                 \r\n";
            }
            else if (Job == "01")
            {
                SQL += "    AND Pano = '" + argPano + "'                                        \r\n";
                SQL += "    AND GBSTS IN ('0', '2', '3', '4')                                   \r\n";
                SQL += "    AND OUTDATE IS NULL                                                 \r\n";
            }
            else if (Job == "02")
            {
                SQL += "    AND Pano = '" + argPano + "'                                        \r\n";
                SQL += "    AND HEIGHT IS NOT NULL                                              \r\n";                
            }
            else
            {
                SQL += "    AND Pano = '" + argPano + "'                                        \r\n";
            }

            if (Job == "02")
            {
                SQL += "    ORDER BY INDATE DESC                                                \r\n";
            }
            else
            {

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

        public DataTable sel_NUR_JINDAN(PsmhDb pDbCon, string Job, string argPano)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                     \r\n";
            SQL += "   Pano, Diagnosys                                                          \r\n";            
            SQL += "   ,TO_CHAR(InDate,'YYYY-MM-DD') InDate                                     \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "NUR_JINDAN                                     \r\n";
            SQL += "   WHERE 1 = 1                                                              \r\n";
            if (Job == "00")
            {
                SQL += "    AND Pano = '" + argPano + "'                                        \r\n";                
            }
            else if (Job == "01")
            {
                SQL += "    AND Pano = '" + argPano + "'                                        \r\n";                
            }
            else
            {
                SQL += "    AND Pano = '" + argPano + "'                                        \r\n";
            }

            if (Job == "00")
            {

            }
            else
            {

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
        /// 2019-09-27 안정수 추가, 조영제 출력에 사용
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public DataTable sel_Read_MCode(PsmhDb pDbCon, string argCode)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                 \r\n";
            SQL += "  MCODE, MNAME                                                          \r\n";            
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_MCODE                                \r\n";            
            SQL += "  WHERE 1 = 1                                                           \r\n";
            SQL += "   AND MCODE IN (" + argCode + ")                                       \r\n";

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
        /// 2019-09-27 안정수 추가, 조영제 출력에 사용
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argROWID"></param>
        /// <returns></returns>
        public DataTable sel_Read_Detail(PsmhDb pDbCon, string argROWID)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                \r\n";
            SQL += "  PANO, SNAME, SEX, AGE, ROWID, BDATE, XCODE, XJONG, IPDOPD, SEEKDATE  \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                              \r\n";
            SQL += "  WHERE 1 = 1                                                          \r\n";
            SQL += "   AND ROWID = '" + argROWID + "'                                      \r\n";

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
        /// 2019-10-08 안정수 추가, 접수 향정 목록을 읽어 온다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCls"></param>
        /// <returns></returns>
        public DataTable sel_ORDER_XRAY(PsmhDb pDbCon, string argPtno, string argBDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                        \r\n";
            SQL += "  'O' IO, A.SuCode, A.QTY, B.SUNAMEK,a.OrderNo, A.ROWID, B.UNITNEW1,B.UNITNEW4, A.REALQTY      \r\n";
            SQL += "  FROM " + ComNum.DB_MED + "OCS_OORDER a                                                       \r\n";
            SQL += "     , " + ComNum.DB_PMPA + "BAS_SUN b                                                         \r\n";
            SQL += "   WHERE 1 = 1                                                                                 \r\n";
            SQL += "    AND a.Ptno = '" + argPtno + "'                                                             \r\n";
            SQL += "    AND a.BDATE = TO_DATE('" + argBDate + "','YYYY-MM-DD')                                     \r\n";
            SQL += "    AND a.GbSunap IN ('0','1')                                                                 \r\n";
            //2018-08-10 안정수, 검사전 입원수속할 경우 중복으로 표시 안되도록 추가함                              
            SQL += "    AND (a.AUTO_SEND IS NULL OR a.AUTO_SEND <>'1')                                             \r\n";
            SQL += "    AND a.Nal >0                                                                               \r\n";
            SQL += "    AND a.SuCode = b.SUNEXT(+)                                                                 \r\n";
            //if (argCls.Dept == "HR" || argCls.Dept == "TO")
            //{
            //    SQL += "    AND a.DeptCode IN ('HR','TO')                                                          \r\n";
            //}
            //else
            //{
            //    SQL += "    AND a.DeptCode = '" + argCls.Dept + "'                                                 \r\n";
            //}
            SQL += "    AND TRIM(a.SuCode) IN (                                                                    \r\n";
            SQL += "                          SELECT TRIM(CODE)                                                    \r\n";
            SQL += "                            FROM  " + ComNum.DB_PMPA + "BAS_BCODE                              \r\n";
            SQL += "                             WHERE 1 = 1                                                       \r\n";
            SQL += "                              AND GUBUN ='XRAY_마약향정코드'                                   \r\n";
            SQL += "                              AND (DELDATE IS NULL OR DELDATE ='')                             \r\n";
            SQL += "                        )                                                                      \r\n";
            SQL += " UNION ALL                                                                                     \r\n"; //UNION
            SQL += " SELECT                                                                                        \r\n";
            SQL += "  'I' IO, A.SuCode, A.QTY, B.SUNAMEK,a.OrderNo, A.ROWID, B.UNITNEW1,B.UNITNEW4, A.REALQTY      \r\n";
            SQL += "  FROM " + ComNum.DB_MED + "OCS_IORDER a                                                       \r\n";
            SQL += "     , " + ComNum.DB_PMPA + "BAS_SUN b                                                         \r\n";
            SQL += "   WHERE 1 = 1                                                                                 \r\n";
            SQL += "    AND a.Ptno = '" + argPtno + "'                                                             \r\n";
            SQL += "    AND a.BDATE >= TO_DATE('" + argBDate + "','YYYY-MM-DD')                                    \r\n";
            SQL += "    AND a.BDATE <= TO_DATE('" + argBDate + "','YYYY-MM-DD')                                    \r\n";
            SQL += "    AND a.Nal >0                                                                               \r\n";
            SQL += "    AND a.SuCode = b.SUNEXT(+)                                                                 \r\n";          
            SQL += "    AND ( a.gbprn =' ' or a.gbprn <>'P' )                                                      \r\n";
            SQL += "    AND a.GbStatus IN  (' ','D+')                                                              \r\n";
            SQL += "    AND (a.ordersite is null or a.ordersite <>'OPDX' )                                         \r\n";

            SQL += "    AND TRIM(a.SuCode) IN (                                                                    \r\n";
            SQL += "                          SELECT TRIM(CODE)                                                    \r\n";
            SQL += "                            FROM  " + ComNum.DB_PMPA + "BAS_BCODE                              \r\n";
            SQL += "                             WHERE 1 = 1                                                       \r\n";
            SQL += "                              AND GUBUN ='XRAY_마약향정코드'                                   \r\n";
            SQL += "                              AND (DELDATE IS NULL OR DELDATE ='')                             \r\n";
            SQL += "                        )                                                                      \r\n";
            SQL += "  GROUP BY 1,A.SuCode, A.QTY, B.SUNAMEK,a.OrderNo, A.ROWID, B.UNITNEW1,B.UNITNEW4,A.REALQTY    \r\n";
            SQL += "   ORDER BY 2                                                                                  \r\n";
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

        public DataTable sel_Read_ctYAK(PsmhDb pDbCon, string argCode)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                 \r\n";
            SQL += "  SUNEXT, SUNAMEK                                                       \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_SUN                                   \r\n";
            SQL += "  WHERE 1 = 1                                                           \r\n";
            SQL += "   AND SUNEXT IN (" + argCode + ")                                      \r\n";

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
        /// 2019-10-16 안정수 추가, 향정,마약관련 주사를 읽어온다 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argROWID"></param>
        /// <returns></returns>
        public DataTable sel_Read_MayakHyang_I(PsmhDb pDbCon, string argPTNO, string argBDate, string argSeekDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                             \r\n";
            SQL += "  SUCODE, KOSMOS_OCS.FC_BAS_SUN_SUNAMEK(SUCODE) FC_NAME, NAL, DOSCODE, REALQTY      \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "OCS_IORDER                                             \r\n";
            SQL += "  WHERE 1 = 1                                                                       \r\n";
            SQL += "   AND PTNO = '" + argPTNO + "'                                                     \r\n";
            SQL += "   AND BDATE >= TO_DATE('" + argBDate + "', 'YYYY-MM-DD')                           \r\n";
            SQL += "   AND BDATE <= TO_DATE('" + argSeekDate + "', 'YYYY-MM-DD')                        \r\n";
            SQL += "   AND NAL > 0                                                                      \r\n";            
            SQL += "   AND DOSCODE IN (                                                                 \r\n";
            SQL += "                    SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE                      \r\n";
            SQL += "                    WHERE (DOSNAME LIKE '%영상의학%' OR DOSNAME LIKE '%방사선%')    \r\n";
            SQL += "                    AND DELDATE IS NULL                                             \r\n";
            SQL += "                   )                                                                \r\n";
            SQL += "   AND TRIM(SuCode) IN (                                                            \r\n";
            SQL += "                         SELECT TRIM(CODE)                                          \r\n";
            SQL += "                         FROM  KOSMOS_PMPA.BAS_BCODE                                \r\n";
            SQL += "                         WHERE 1=1                                                  \r\n";
            SQL += "                         AND GUBUN ='XRAY_마약향정코드'                             \r\n";
            SQL += "                         AND (DELDATE IS NULL OR DELDATE ='')                       \r\n";
            SQL += "                        )                                                           \r\n";
            SQL += "   AND GbStatus IN  (' ','D+')                                                      \r\n";
            SQL += "   AND (GBPRN =' ' or GBPRN <>'P')                                                  \r\n";
            SQL += "   AND (ORDERSITE IS NULL OR ORDERSITE <>'OPDX')                                    \r\n";

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

        public DataTable sel_Read_MayakHyang_O(PsmhDb pDbCon, string argPTNO, string argBDate, string argSeekDate) 
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                             \r\n";
            SQL += "  SUCODE, KOSMOS_OCS.FC_BAS_SUN_SUNAMEK(SUCODE) FC_NAME, NAL, DOSCODE, REALQTY      \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "OCS_OORDER                                             \r\n";
            SQL += "  WHERE 1 = 1                                                                       \r\n";
            SQL += "   AND PTNO = '" + argPTNO + "'                                                     \r\n";
            SQL += "   AND BDATE >= TO_DATE('" + argBDate + "', 'YYYY-MM-DD')                           \r\n";
            SQL += "   AND BDATE <= TO_DATE('" + argSeekDate + "', 'YYYY-MM-DD')                        \r\n";
            SQL += "   AND NAL > 0                                                                      \r\n";
            SQL += "   AND DOSCODE IN (                                                                 \r\n";
            SQL += "                    SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE                      \r\n";
            SQL += "                    WHERE (DOSNAME LIKE '%영상의학%' OR DOSNAME LIKE '%방사선%')    \r\n";
            SQL += "                    AND DELDATE IS NULL                                             \r\n";
            SQL += "                   )                                                                \r\n";
            SQL += "   AND TRIM(SuCode) IN (                                                            \r\n";
            SQL += "                         SELECT TRIM(CODE)                                          \r\n";
            SQL += "                         FROM  KOSMOS_PMPA.BAS_BCODE                                \r\n";
            SQL += "                         WHERE 1=1                                                  \r\n";
            SQL += "                         AND GUBUN ='XRAY_마약향정코드'                             \r\n";
            SQL += "                         AND (DELDATE IS NULL OR DELDATE ='')                       \r\n";
            SQL += "                        )                                                           \r\n";
            SQL += "   AND GBSUNAP IN ('0','1')                                                         \r\n";   

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
        /// 2020-02-03 안정수, 당일입원자 체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPTNO"></param>
        /// <param name="argBDate"></param>
        /// <param name="argSeekDate"></param>
        /// <returns></returns>
        public DataTable sel_Check_DangIp(PsmhDb pDbCon, string argPTNO, string argBDate, string argSeekDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                             \r\n";
            SQL += "    COUNT(*)                                                                        \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "OCS_IORDER                                             \r\n";
            SQL += "  WHERE 1 = 1                                                                       \r\n";
            SQL += "   AND PTNO = '" + argPTNO + "'                                                     \r\n";
            SQL += "   AND BDATE >= TO_DATE('" + argBDate + "', 'YYYY-MM-DD')                           \r\n";
            SQL += "   AND BDATE <= TO_DATE('" + argSeekDate + "', 'YYYY-MM-DD')                        \r\n";
            SQL += "   AND NAL > 0                                                                      \r\n";
            SQL += "   AND BUN IN ('72', '73')                                                          \r\n";
            SQL += "   AND GBORDER = 'M'                                                                \r\n";
            SQL += "   AND GbStatus IN  (' ','D+')                                                      \r\n";
            SQL += "   AND (GBPRN =' ' or GBPRN <>'P')                                                  \r\n";

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

        #region //영상의학과 접수 및 기타업무 및 건진영상

        /// <summary>
        /// 영상의학과 접수 메인 쿼리
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCls"></param>
        /// <returns></returns>
        public DataTable sel_XRAY_DETAIL(PsmhDb pDbCon, cXrayDetail argCls,bool bLog) 
        {
            DataTable dt = null;

            SQL = "";            
            SQL += " SELECT                                                                                                                             \r\n";
            //2018-08-10 안정수, 영상의학과 요청으로 a.Xjong, xcdc 추가
            SQL += "        a.Pano,c.SName,c.Obst,a.XJong, decode(trim(a.xcode),'XCDC','98','XDVDC','99',xjong) xcdc                                    \r\n";
            //SQL += "        a.Pano,c.SName,c.Obst                                                                                                       \r\n";

            SQL += "       ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                                                                         \r\n";
            SQL += "       ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SeekDate                                                                                   \r\n";
            //SQL += "       ,TO_CHAR(a.EnterDate,'YYYY-MM-DD') EnterDate                                                                                 \r\n";
            //SQL += "       ,a.xcode                                                                                                                     \r\n";
            //SQL += "       ,a.orderno                                                                                                                   \r\n";
            if (argCls.STS == "00")
            {                
                SQL += "       ,a.IPDOPD,c.Sex,a.DeptCode,a.WardCode,a.RoomCode,c.Jumin1 || '-' || c.Jumin2 AS Jumin                                    \r\n";                
                SQL += "       ,( SELECT COUNT(s.XCode)                                                                                                 \r\n";
                SQL += "           FROM " + ComNum.DB_PMPA + "XRAY_DETAIL s                                                                             \r\n";
                SQL += "            WHERE 1 = 1                                                                                                         \r\n";
                SQL += "              AND s.Pano=a.Pano                                                                                                 \r\n";
                SQL += "              AND s.DeptCode=a.DeptCode                                                                                         \r\n";
                if (argCls.Job =="자동접수")
                {
                    SQL += "              AND ( s.SeekDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                              \r\n";
                    SQL += "                    AND s.SeekDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                            \r\n";
                    SQL += "                   )                                                                                                        \r\n";
                }
                else
                {
                    SQL += "              AND ( s.SeekDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                              \r\n";
                    SQL += "                    AND s.SeekDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                            \r\n";
                    SQL += "                  OR  s.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                               \r\n";
                    SQL += "                    AND s.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                               \r\n";
                    SQL += "                   )                                                                                                        \r\n";
                }
                
                
                if (argCls.Tab == "1")
                {
                    SQL += "              AND ( s.XJong <= '9'  OR s.XJong ='Q' )                                                                       \r\n";
                }
                else if (argCls.Tab == "2")
                {
                    SQL += "              AND ( s.XJong <= '9'  OR s.XJong IN ('Q','F') )                                                               \r\n";
                }
                    
                if (argCls.Job == "OCS접수")
                {
                    SQL += "          AND s.ORDERNO > 0                                                                                                 \r\n";
                }
                else if (argCls.Job == "자동접수")
                {
                    SQL += "          AND ( s.OrderNo IS NULL OR s.OrderNo = 0 )                                                                        \r\n";
                    SQL += "          AND ( s.Gbend <> '1' OR s.Gbend IS NULL )                                                                         \r\n";
                }
                if (argCls.Job == "OCS접수" || argCls.Job == "자동접수")
                {
                    SQL += "          AND ( s.GbReserved = '1' OR s.GbReserved = '2' )                                                                  \r\n";
                    SQL += "          AND ( s.GbHIC IS NULL OR s.GbHIC <> 'Y' )                                                                         \r\n";
                }
                SQL += "         ) AS CNT1                                                                                                              \r\n";

                SQL += "       ,( SELECT KOSMOS_OCS.FC_XRAY_DETAIL_DRREMARK(s.Pano,'" + argCls.Date1 + "','" + argCls.Date2 + "',s.DeptCode)            \r\n";
                SQL += "           FROM " + ComNum.DB_PMPA + "XRAY_DETAIL s                                                                             \r\n";
                SQL += "            WHERE 1 = 1                                                                                                         \r\n";
                SQL += "              AND s.Pano=a.Pano                                                                                                 \r\n";
                SQL += "              AND s.DeptCode=a.DeptCode                                                                                         \r\n";
                if (argCls.Job == "자동접수")
                {
                    SQL += "              AND ( s.SeekDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                              \r\n";
                    SQL += "                    AND s.SeekDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                            \r\n";                    
                    SQL += "                   )                                                                                                        \r\n";
                }
                else
                {
                    SQL += "              AND ( s.SeekDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                              \r\n";
                    SQL += "                    AND s.SeekDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                            \r\n";
                    SQL += "                  OR  s.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                               \r\n";
                    SQL += "                    AND s.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                               \r\n";
                    SQL += "                   )                                                                                                        \r\n";
                }
                    
                SQL += "              AND  s.DrRemark > ' '                                                                                             \r\n";
                if (argCls.Tab == "1")
                {
                    SQL += "              AND ( s.XJong <= '9'  OR s.XJong ='Q' )                                                                       \r\n";
                }
                else if (argCls.Tab == "2")
                {
                    SQL += "              AND ( s.XJong <= '9'  OR s.XJong IN ('Q','F') )                                                               \r\n";
                }
                if (argCls.Job == "OCS접수")
                {
                    SQL += "          AND s.ORDERNO > 0                                                                                                 \r\n";
                }
                else if (argCls.Job == "자동접수")
                {
                    SQL += "          AND ( s.OrderNo IS NULL OR s.OrderNo = 0 )                                                                        \r\n";
                    SQL += "          AND ( s.Gbend <> '1' OR s.Gbend IS NULL )                                                                         \r\n";
                }
                if (argCls.Job == "OCS접수" || argCls.Job == "자동접수")
                {
                    SQL += "          AND ( s.GbReserved = '1' OR s.GbReserved = '2' )                                                                  \r\n";
                    SQL += "          AND ( s.GbHIC IS NULL OR s.GbHIC <> 'Y' )                                                                         \r\n";
                }
                SQL += "       GROUP BY s.Pano,s.DeptCode                                                                                               \r\n";
                SQL += "         ) AS Remark                                                                                                            \r\n";

            }
            else if (argCls.STS == "01")
            {
                SQL += "       ,DECODE(TRIM(a.XCode),'XCDC','CD','XDVDC','DVD','') COPY                                                                 \r\n";
                SQL += "       ,a.XJong,a.IPDOPD,c.Sex,a.DeptCode,a.DrCode,d.DrName,a.WardCode,a.RoomCode,a.ASA                                         \r\n";
                SQL += "       ,TO_CHAR(a.RDate,'YYYY-MM-DD') RDate                                                                                     \r\n";
                SQL += "       ,TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI') OrderDate                                                                     \r\n";
                SQL += "       ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_XRAY_접수종류',TRIM(a.XJong)) FC_XJong2                                            \r\n"; //종류체크                        
            }
            else if (argCls.STS == "02")
            {
                SQL += "       ,DECODE(TRIM(a.XCode),'XCDC','CD','XDVDC','DVD','') COPY                                                                 \r\n";
                SQL += "       ,a.XJong,a.IPDOPD,c.Sex,a.DeptCode,a.DrCode,d.DrName,a.WardCode,a.RoomCode,a.GbPortable,a.PickupRemark                   \r\n";
                SQL += "       ,a.XCode,a.XSubCode,a.Qty,a.Remark,a.DrRemark,a.OrderCode,a.OrderName,a.ASA,a.cRemark,a.ROWID                            \r\n";
                SQL += "       ,TO_CHAR(a.RDate,'YYYY-MM-DD') RDate                                                                                     \r\n";
                SQL += "       ,TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI') OrderDate                                                                     \r\n";
                SQL += "       ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_XRAY_접수종류',TRIM(a.XJong)) FC_XJong2                                            \r\n"; //종류체크
                SQL += "       ,KOSMOS_OCS.FC_IPD_NEW_MASTER_BEDNUM(a.Pano) FC_BedNo                                                                    \r\n"; //베드NO
                SQL += "       ,KOSMOS_OCS.FC_XRAY_CT_CONST_CHK(a.Pano,a.BDate) FC_CT_Const                                                             \r\n"; //CT constr
                
            }
            else if (argCls.STS == "03" || argCls.STS == "04")
            {
                SQL += "       ,a.Exid,a.XCode,a.OrderNo,a.Remark,a.DrRemark,a.PacsNo,a.OrderName,a.PacsStudyID                                         \r\n";
                SQL += "       ,b.OrderName OrderName2 ,a.Exinfo                                                                                        \r\n";
                SQL += "       ,a.ROWID                                                                                                                 \r\n";
                SQL += "       ,KOSMOS_OCS.FC_XRAY_CODE_NM(a.XCode) FC_XName                                                                            \r\n"; //코드명칭
            }
             
            ////function
            ////SQL += "   ,KOSMOS_OCS.FC_BAS_AUTO_MST_CHK(a.Ptno,a.BDate) autoSTS                                                                      \r\n"; //후불체크
            //SQL += "   ,KOSMOS_OCS.FC_NUR_HAPPYCALL_OPD(a.Pano,'05','ENDO_JUPMST',a.ROWID) FC_happycall                                             \r\n"; //해피콜체크
            ////SQL += "   ,KOSMOS_OCS.FC_BAS_BUSE_NAME(a.Buse) FC_BuseName                                                                             \r\n"; //부서이름
            
            //SQL += "   ,KOSMOS_OCS.FC_OPD_RESERVED_NEW_NEAR(a.Pano,a.DeptCode) FC_opdRes                                                            \r\n"; //예약접수정보
            ////SQL += "   ,KOSMOS_OCS.FC_OCS_ITRANSFER_CHK(a.Ptno,'MG') FC_Consult                                                                     \r\n"; //협진체크
            //SQL += "   ,KOSMOS_OCS.FC_OPD_SLIP_SUNAP_CHK(a.Pano,a.BDate,b.SuCode,a.OrderNo) FC_SUNAP                                                \r\n"; //수납체크
            //SQL += "   ,KOSMOS_OCS.FC_GET_AGE(TO_CHAR(c.Birth,'YYYY-MM-DD'),a.BDate) FC_age                                                             \r\n"; //나이체크
            SQL += "   ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,a.BDate) FC_infect                                                                  \r\n"; //감염체크
            if (argCls.STS != "02")
            {
                SQL += "   ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Pano,a.BDate) FC_infect_EX                                                            \r\n"; //감염체크
            }            
            SQL += "   ,KOSMOS_OCS.FC_BAS_AREA_WON(a.Pano,a.IPDOPD,a.BI,a.DrCode) FC_AREA                                                               \r\n"; //원거리       
            SQL += "   ,KOSMOS_OCS.FC_PREGNANT_CHK(a.IPDOPD,a.Pano,a.BDate) FC_PREG                                                                     \r\n"; //임신  
            SQL += "   ,KOSMOS_OCS.FC_GET_AGE2(a.Pano,a.BDate) FC_AGE2                                                                                  \r\n"; //나이  
            SQL += "   ,KOSMOS_OCS.FC_XRAY_JUSA_CHK(a.Pano,TRUNC(SYSDATE),'NULL') FC_JUSA                                                               \r\n"; //주사체크
            SQL += "   ,KOSMOS_OCS.FC_IPD_NEW_MASTER_EXAM(a.Pano) FC_EXAM                                                                               \r\n"; //입원정밀
            SQL += "   ,KOSMOS_OCS.FC_XRAY_COPY_CHK(a.Pano,a.BDate) FC_COPY                                                                             \r\n"; //CD복사
            SQL += "   ,KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(a.Pano,TRUNC(SYSDATE)) FC_Fall                                                                \r\n"; //낙상
            SQL += "   ,KOSMOS_OCS.FC_IPD_NEW_MASTER_SECRET(a.Pano) FC_IPD_SECRET                                                                       \r\n"; //입원사생활체
            SQL += "   ,KOSMOS_OCS.FC_MISU_GAINMST_CHK(a.Pano) FC_Misu                                                                                  \r\n"; //개인미수체크

            //2018-10-31 안정수, 메모리사용량 증가로인한 임시 Test로 주석처리함
            SQL += "   ,KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(a.Pano,a.BDate,a.DeptCode) FC_ErSTS                                                          \r\n"; //응급중증   

            //SQL += "   ,KOSMOS_OCS.FC_XRAY_ASA_CHK(a.Pano,a.BDate) FC_ASA_Suga                                                                          \r\n"; //진정수가        
            #region 2018-08-22 안정수, ER CP표기부분 추가
            //SQL += "   ,CASE WHEN KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(a.PANO, a.BDATE,a.DEPTCODE) IS NOT NULL THEN 'CP/'||KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(a.PANO, a.BDATE,a.DEPTCODE) ELSE '' END       AS ERP              --              \r\n";
            //SQL += "        ,  (SELECT MAX(ERPATIENT) FROM KOSMOS_PMPA.OPD_MASTER WHERE PANO = a.PANO AND DEPTCODE = 'ER' AND ACTDATE = TRUNC(SYSDATE))     AS ERP                          \r\n";

            //2018-10-31 안정수, 메모리사용량 증가로인한 임시 Test로 주석처리함
            SQL += "        ,  KOSMOS_OCS.FC_CP_RECORD_ER(a.PANO,a.BDATE) FC_CP_CHK                                                                    \r\n";
            #endregion

            //SQL += "   ,KOSMOS_OCS.FC_IPD_NEW_MASTER_JSTS2(KOSMOS_OCS.FC_IPD_NEW_MASTER_JSTS(a.Pano)) FC_Ipd_Info                                   \r\n"; //재원체크
            //SQL += "   ,KOSMOS_OCS.FC_OCS_ITRANSFER_CHK2(a.Pano,'MG') FC_Consult                                                                    \r\n"; //협진체크
            ////SQL += "   ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_ENDO_도착구분',a.sGubun2) FC_sGubun2                                                    \r\n"; //도착구분                        
            SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                                                                          \r\n";
            SQL += "       ," + ComNum.DB_MED + "OCS_ORDERCODE b                                                                                        \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_PATIENT c                                                                                         \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_DOCTOR d                                                                                          \r\n";
            SQL += "   WHERE 1 = 1                                                                                                                      \r\n";
            SQL += "    AND a.OrderCode = b.OrderCode(+)                                                                                                \r\n";
            SQL += "    AND a.DrCode    = d.Drcode(+)                                                                                                   \r\n";            
            SQL += "    AND a.PANO = c.PANO(+)                                                                                                          \r\n";
                       

            if (argCls.Pano != "")
            {
                SQL += "   AND a.Pano ='" + argCls.Pano + "'                                                                                            \r\n";
            }

            if (argCls.STS == "00" || argCls.STS == "01" || argCls.STS == "02")
            {
                if (argCls.Tab =="1")
                {
                    if (argCls.Job == "자동접수")
                    {
                        SQL += "  AND ( (a.SeekDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                     \r\n";
                        SQL += "        AND a.SeekDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI') )                                  \r\n";                        
                        SQL += "       )                                                                                                                \r\n";
                    }
                    else
                    {
                        SQL += "  AND ( (a.SeekDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                     \r\n";
                        SQL += "        AND a.SeekDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI') )                                  \r\n";
                        SQL += "      OR ( a.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                      \r\n";
                        SQL += "        AND a.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI') )                                     \r\n";
                        SQL += "       )                                                                                                                \r\n";
                    }
                        
                    #region //예약일자 설정된것중 오늘일자 이후건 제외
                    if (argCls.GbResv1 =="Y")
                    {
                        SQL += "   AND a.ROWID NOT IN (                                                                                                 \r\n";
                        SQL += "                        SELECT ROWID                                                                                    \r\n";
                        SQL += "                          FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                        \r\n";
                        SQL += "                            WHERE 1 = 1                                                                                 \r\n";
                        SQL += "                             AND RDate >= TRUNC(SYSDATE + 1)                                                            \r\n";
                        if (argCls.DeptCode != "**")
                        {
                            SQL += "                         AND DeptCode ='" + argCls.DeptCode + "'                                                    \r\n";
                        }
                        if (argCls.XJong == "*")
                        {
                            SQL += "                         AND ( XJong <= '9'  OR XJong ='Q' )                                                        \r\n";                 
                        }
                        else if (argCls.XJong == "A1")
                        {
                            SQL += "                         AND XJong IN ('6','7')                                                                     \r\n"; //BMD+RI
                        }
                        else                                 
                        {                                    
                            SQL += "                         AND XJong ='" + argCls.XJong + "'                                                          \r\n";
                        }
                        if (argCls.IPDOPD != "*")
                        {
                            SQL += "                         AND IPDOPD ='" + argCls.IPDOPD + "'                                                        \r\n";
                        }

                        if (argCls.GbPort == "Y")
                        {
                            SQL += "                         AND GbPortable ='M'                                                                        \r\n";
                        }
                        else
                        {
                            SQL += "                         AND (GbPortable IS NULL OR GbPortable <>'M')                                               \r\n";
                        }
                        if (argCls.GbHR == "Y")
                        {
                            SQL += "                         AND DeptCode IN ('HR','TO')                                                                \r\n";
                        }
                        else
                        {
                            if (argCls.Job == "OCS접수")
                            {
                                SQL += "                     AND ORDERNO > 0                                                                            \r\n";
                            }
                            else if (argCls.Job == "자동접수")
                            {
                                SQL += "                     AND ( OrderNo IS NULL OR OrderNo = 0 )                                                     \r\n";
                                SQL += "                     AND ( Gbend <> '1' OR Gbend IS NULL )                                                      \r\n";
                            }
                        }

                        if (argCls.Job == "OCS접수" || argCls.Job == "자동접수")
                        {
                            SQL += "                         AND ( GbReserved = '1' OR GbReserved = '2' )                                               \r\n";                       
                            SQL += "                         AND ( GbHIC IS NULL OR GbHIC <> 'Y' )                                                      \r\n";
                        }
                        SQL += "                             AND TRIM(XCode) NOT IN                                                                     \r\n";
                        SQL += "                                                    (                                                                   \r\n";
                        SQL += "                                                      SELECT TRIM(CODE)                                                 \r\n";                     
                        SQL += "                                                        FROM " + ComNum.DB_PMPA + "BAS_BCODE                            \r\n";                     
                        SQL += "                                                         WHERE 1=1                                                      \r\n";                     
                        SQL += "                                                          AND GUBUN ='XRAY_OCS명단제외코드'                             \r\n";                     
                        SQL += "                                                          AND ( DELDATE IS NULL OR DELDATE ='' )                        \r\n";                     
                        SQL += "                                                    )                                                                   \r\n";
                        SQL += "                      )                                                                                                 \r\n";
                    }
                    #endregion
                }
                else if (argCls.Tab == "2")
                {
                    SQL += "   AND a.SeekDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                           \r\n";
                    SQL += "   AND a.SeekDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                                             \r\n";
                }
                                               
            }
            else if (argCls.STS =="03")
            {
                SQL += "   AND a.ROWID IN (" + argCls.ROWID + ")                                                                                        \r\n";
            }
            else if (argCls.STS == "04")
            {                
                SQL += "   AND a.Exinfo  = " + argCls.WRTNO + "                                                                                         \r\n";
            }
            else
            {

            }

            if (argCls.DeptCode != "**")
            {
                SQL += "   AND a.DeptCode ='" + argCls.DeptCode + "'                                                                                    \r\n";
            }

            if (argCls.Search != "")
            {
                SQL += "   AND (a.Pano = '" + argCls.Search + "'                                                                                        \r\n";
                SQL += "         OR c.SName LIKE '%" + argCls.Search + "%'  )                                                                           \r\n";
            }

            if (argCls.Tab == "1")
            {
                if (argCls.XJong =="*")
                {
                    SQL += "   AND ( a.XJong <= '9'  OR a.XJong ='Q' )                                                                                  \r\n";
                }
                else if (argCls.XJong == "A1")
                {
                    SQL += "   AND a.XJong IN ('6','7')                                                                                                 \r\n"; //BMD+RI
                }
                else               
                {
                    SQL += "   AND a.XJong ='" + argCls.XJong + "'                                                                                      \r\n";
                }
            }
            else if (argCls.Tab == "2")
            {
                if (argCls.XJong == "*")
                {                    
                    SQL += "   AND ( a.XJong <= 'A'  OR a.XJong IN ('Q','F') )                                                                          \r\n";
                }
                else if (argCls.XJong == "A1")
                {
                    SQL += "   AND a.XJong IN ('6','7')                                                                                                 \r\n"; //BMD+RI
                }
                else
                {
                    SQL += "   AND a.XJong ='" + argCls.XJong + "'                                                                                      \r\n";
                }
            }

            if (argCls.IPDOPD != "*")
            {
                SQL += "   AND a.IPDOPD ='" + argCls.IPDOPD + "'                                                                                        \r\n";
            }

            if (argCls.GbPort == "Y")
            {
                SQL += "   AND a.GbPortable ='M'                                                                                                        \r\n";
            }
            else
            {
                //SQL += "   AND (a.GbPortable IS NULL OR a.GbPortable <>'M')                                                                             \r\n";
            }

            if (argCls.GbER == "Y")
            {
                SQL += "   AND (a.DeptCode ='ER' OR ( a.WardCode ='ER' AND a.RoomCode =100) )                                                           \r\n";
            }

            if (argCls.GbJusa =="Y")
            {
                //CT
                SQL += "   AND a.XJong IN ( '4' )                                                                                                       \r\n";
                SQL += "   AND a.Pano IN (                                                                                                              \r\n";
                SQL += "                  SELECT k.Pano                                                                                                 \r\n";
                SQL += "                    FROM " + ComNum.DB_PMPA + "OPD_SLIP k                                                                       \r\n";
                SQL += "                     WHERE 1 = 1                                                                                                \r\n";                
                SQL += "                        AND k.BDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                             \r\n";
                SQL += "                        AND k.BDate <= TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                                             \r\n";
                SQL += "                        AND k.DOSCODE in (                                                                                      \r\n";
                SQL += "                                        SELECT DOSCODE                                                                          \r\n"; 
                SQL += "                                          FROM " + ComNum.DB_MED + "OCS_ODOSAGE                                                 \r\n";
                SQL += "                                         WHERE DOSNAME LIKE '%주사실%'                                                          \r\n";
                SQL += "                                        UNION ALL                                                                               \r\n";
                SQL += "                                        SELECT DOSCODE                                                                          \r\n";
                SQL += "                                          FROM " + ComNum.DB_MED + "OCS_ODOSAGE                                                 \r\n";
                SQL += "                                         WHERE DOSNAME LIKE '%영상의학과%'                                                      \r\n";
                SQL += "                                        )                                                                                       \r\n";
                SQL += "                 )                                                                                                              \r\n";
            }

            if (argCls.Tab == "1")
            {
                if (argCls.GbHR == "Y")
                {
                    SQL += "   AND a.DeptCode IN ('HR','TO')                                                                                            \r\n";
                }
                else
                {
                    if (argCls.Job == "OCS접수")
                    {
                        SQL += "   AND a.ORDERNO > 0                                                                                                    \r\n";
                    }
                    else if (argCls.Job == "자동접수")
                    {
                        SQL += "   AND ( a.OrderNo IS NULL OR a.OrderNo = 0 )                                                                           \r\n";
                        SQL += "   AND ( a.Gbend <> '1' OR a.Gbend IS NULL )                                                                            \r\n";
                    }
                }

                if (argCls.Job == "OCS접수" || argCls.Job == "자동접수")
                {
                    SQL += "   AND ( a.GbReserved = '1' OR a.GbReserved = '2' )                                                                         \r\n";
                    SQL += "   AND ( a.GbHIC IS NULL OR GbHIC <> 'Y' )                                                                                  \r\n";
                }
            }
            else if (argCls.Tab == "2")
            {
                SQL += "   AND a.GbReserved IN ('6','7')                                                                                                \r\n";
                SQL += "   AND ( a.Gbend <> '1' OR a.Gbend IS NULL )                                                                                    \r\n";
                SQL += "   AND ( a.GbHIC IS NULL OR GbHIC <> 'Y' )                                                                                      \r\n";
                SQL += "   AND ( a.PacsStudyID IS NULL OR (a.ExInfo IS NULL OR a.ExInfo < 1000) )                                                       \r\n";
            }


            if (argCls.STS == "00" && argCls.Tab == "1")
            {

                SQL += "   AND (TRIM(a.XCode) NOT IN (                                                                                                      \r\n";
                SQL += "                            SELECT TRIM(CODE)                                                                                       \r\n";
                SQL += "                              FROM " + ComNum.DB_PMPA + "BAS_BCODE                                                                  \r\n";
                SQL += "                               WHERE 1=1                                                                                            \r\n";
                SQL += "                                AND GUBUN ='XRAY_OCS명단제외코드'                                                                   \r\n";
                SQL += "                                AND ( DELDATE IS NULL OR DELDATE ='' )                                                              \r\n";
                SQL += "                          ) AND TRIM(a.XCode) NOT IN ('EB521A', 'EB561'))                                                           \r\n";
            }
            else
            {
                SQL += "   AND TRIM(a.XCode) NOT IN (                                                                                                       \r\n";
                SQL += "                            SELECT TRIM(CODE)                                                                                       \r\n";
                SQL += "                              FROM " + ComNum.DB_PMPA + "BAS_BCODE                                                                  \r\n";
                SQL += "                               WHERE 1=1                                                                                            \r\n";
                SQL += "                                AND GUBUN ='XRAY_OCS명단제외코드'                                                                   \r\n";
                SQL += "                                AND ( DELDATE IS NULL OR DELDATE ='' )                                                              \r\n";
                SQL += "                          )                                                                                                         \r\n";
            }
            if (argCls.STS == "00" || argCls.STS == "01")
            {
                //2018-08-10 안정수, 영상의학과 요청으로 a.Xjong, xcdc 추가               
                SQL += "   GROUP BY a.Pano,c.SName,c.Obst,a.XJong,decode(trim(a.xcode),'XCDC','98','XDVDC','99',xjong)                                  \r\n";
                //SQL += "   GROUP BY a.Pano,c.SName,c.Obst                                                                                               \r\n";

                SQL += "            ,TO_CHAR(a.BDate,'YYYY-MM-DD')                                                                                      \r\n";
                SQL += "            ,TO_CHAR(a.SeekDate,'YYYY-MM-DD')                                                                                   \r\n";
                //SQL += "            ,TO_CHAR(a.EnterDate,'YYYY-MM-DD')                                                                                  \r\n";
                //SQL += "            ,a.xcode                                                                                                            \r\n";
                //SQL += "            ,a.orderno                                                                                                          \r\n";
                if (argCls.STS == "00")
                {
                    SQL += "            ,a.IPDOPD,c.Sex,a.DeptCode,a.WardCode,a.RoomCode,c.Jumin1 || '-' || c.Jumin2                                    \r\n";
                    SQL += "       ,KOSMOS_OCS.FC_BAS_AREA_WON(a.Pano,a.IPDOPD,a.BI,a.DrCode)                                                           \r\n"; //원거리
                    SQL += "       ,KOSMOS_OCS.FC_PREGNANT_CHK(a.IPDOPD,a.Pano,a.BDate)                                                                 \r\n"; //임신      
                    SQL += "       ,KOSMOS_OCS.FC_GET_AGE2(a.Pano,a.BDate)                                                                              \r\n"; //나이  
                }
                else if (argCls.STS == "01")
                {
                    SQL += "            ,DECODE(TRIM(a.XCode),'XCDC','CD','XDVDC','DVD','')                                                             \r\n";
                    SQL += "            ,a.XJong,a.IPDOPD,c.Sex,a.DeptCode,a.DrCode,d.DrName,a.WardCode,a.RoomCode,a.ASA                                \r\n";
                    SQL += "            ,TO_CHAR(a.RDate,'YYYY-MM-DD')                                                                                  \r\n";
                    SQL += "            ,TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI')                                                                      \r\n";
                    SQL += "            ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('XRAY_방사선종류',TRIM(a.XJong))                                              \r\n";                   
                }
                //SQL += "            ,KOSMOS_OCS.FC_GET_AGE(TO_CHAR(c.Birth,'YYYY-MM-DD'),a.BDate)                                                       \r\n";
                SQL += "            ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,a.BDate)                                                               \r\n";
                SQL += "            ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Pano,a.BDate)                                                            \r\n";
                SQL += "            ,KOSMOS_OCS.FC_BAS_AREA_WON(a.Pano,a.IPDOPD,a.BI,a.DrCode)                                                          \r\n"; //원거리       
                SQL += "            ,KOSMOS_OCS.FC_PREGNANT_CHK(a.IPDOPD,a.Pano,a.BDate)                                                                \r\n"; //임신  
                SQL += "            ,KOSMOS_OCS.FC_GET_AGE2(a.Pano,a.BDate)                                                                             \r\n"; //나이  
                SQL += "            ,KOSMOS_OCS.FC_XRAY_JUSA_CHK(a.Pano,TRUNC(SYSDATE),'NULL')                                                          \r\n"; //주사체크
                SQL += "            ,KOSMOS_OCS.FC_IPD_NEW_MASTER_EXAM(a.Pano)                                                                          \r\n"; //입원정밀
                SQL += "            ,KOSMOS_OCS.FC_XRAY_COPY_CHK(a.Pano,a.BDate)                                                                        \r\n"; //CD복사
                SQL += "            ,KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(a.Pano,TRUNC(SYSDATE))                                                           \r\n"; //낙상
                SQL += "            ,KOSMOS_OCS.FC_IPD_NEW_MASTER_SECRET(a.Pano)                                                                        \r\n"; //입원사생활
                SQL += "            ,KOSMOS_OCS.FC_MISU_GAINMST_CHK(a.Pano)                                                                             \r\n"; //개인미수체크
                SQL += "            ,KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(a.Pano,a.BDate,a.DeptCode)                                                      \r\n"; //응급중증   
                //SQL += "            ,KOSMOS_OCS.FC_XRAY_ASA_CHK(a.Pano,a.BDate)                                                                         \r\n"; //진정수가        
                SQL += "            ,KOSMOS_OCS.FC_CP_RECORD_ER(a.PANO, a.BDATE)                                                                        \r\n"; //ERCP 체크                        
            }

            if (argCls.STS == "00" || argCls.STS == "01")
            {
                if (argCls.GbPort =="Y")
                {
                    //SQL += "   ORDER BY 9,10,2,1,4                                                                                                      \r\n";
                    //2018-10-12 안정수, 영상의학과 요청으로 포터블체크시 병동, 병실 우선으로 정렬되도록 
                    SQL += "   ORDER BY 11,12,2,1,4                                                                                                      \r\n";
                }
                else
                {
                    SQL += "   ORDER BY 2,1,4                                                                                                           \r\n";
                }
                
            }
            else if (argCls.STS == "02")
            {
                SQL += "   ORDER BY 5                                                                                                                   \r\n";
            }
            else if (argCls.STS == "03")
            {
                SQL += "   ORDER BY 5                                                                                                                   \r\n";
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

        public DataTable sel_XRAY_DETAIL2(PsmhDb pDbCon, cXrayDetail argCls, bool bLog)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                                             \r\n";
            SQL += "        a.Pano,a.SName                                                                                                              \r\n";
            SQL += "       ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                                                                         \r\n";
            SQL += "       ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SeekDate                                                                                   \r\n";            
            SQL += "       ,DECODE(TRIM(a.XCode),'XCDC','CD','XDVDC','DVD','') COPY                                                                     \r\n";
            SQL += "       ,a.XJong,a.IPDOPD,a.Sex,a.DeptCode,a.DrCode,a.WardCode,a.RoomCode,a.ASA                                                      \r\n";
            SQL += "       ,TO_CHAR(a.RDate,'YYYY-MM-DD') RDate                                                                                         \r\n";
            SQL += "       ,TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI') OrderDate                                                                         \r\n";
            //SQL += "       ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_XRAY_접수종류',TRIM(a.XJong)) FC_XJong2                                                \r\n"; //종류체크   
            SQL += "       ,DECODE(TRIM(a.XCode),'XCDC','CD','XDVDC','DVD','') COPY                                                                     \r\n";
            SQL += "       ,a.XJong,a.IPDOPD,a.DeptCode,a.DrCode,a.WardCode,a.RoomCode,a.GbPortable,a.PickupRemark                                      \r\n";            
            SQL += "       ,TO_CHAR(a.RDate,'YYYY-MM-DD') RDate                                                                                         \r\n";
            SQL += "       ,TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI') OrderDate                                                                         \r\n";
            //SQL += "       ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_XRAY_접수종류',TRIM(a.XJong)) FC_XJong2                                                \r\n"; //종류체크
            //SQL += "       ,KOSMOS_OCS.FC_IPD_NEW_MASTER_BEDNUM(a.Pano) FC_BedNo                                                                        \r\n"; //베드NO
            //SQL += "       ,KOSMOS_OCS.FC_XRAY_CT_CONST_CHK(a.Pano,a.BDate) FC_CT_Const                                                                 \r\n"; //CT constr
            SQL += "       ,a.Exid,a.XCode,a.OrderNo,a.Remark,a.DrRemark,a.PacsNo,a.PacsStudyID                                                         \r\n";
            SQL += "       ,a.Exinfo                                                                                                                    \r\n";
            SQL += "       ,a.ROWID                                                                                                                     \r\n";
            SQL += "       ,KOSMOS_OCS.FC_XRAY_CODE_NM(a.XCode) FC_XName                                                                                \r\n"; //코드명칭       
            SQL += "       ,(  SELECT CASE WHEN COUNT(*) > 0 THEN   max(DECODE(trim(DispHeader),'',trim(OrderName),trim(DispHeader)))                   \r\n";
            SQL += "                ELSE ''                                                                                                             \r\n"; 
            SQL += "            END                                                                                                                     \r\n";
            SQL += "            FROM KOSMOS_OCS.OCS_ORDERCODE                                                                                           \r\n";
            SQL += "            WHERE ORDERCODE like a.OrderCode||'%') FC_OrderName                                                                     \r\n";
            //SQL += "       ,KOSMOS_OCS.FC_OCS_ORDERCODE_NAME2(a.OrderCode) FC_OrderName                                                                 \r\n"; //오더명칭       
            //SQL += "       ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,a.BDate) FC_infect                                                              \r\n"; //감염체크
            //SQL += "       ,KOSMOS_OCS.FC_BAS_AREA_WON(a.Pano,a.IPDOPD,a.BI,a.DrCode) FC_AREA                                                           \r\n"; //원거리       
            //SQL += "       ,KOSMOS_OCS.FC_PREGNANT_CHK(a.IPDOPD,a.Pano,a.BDate) FC_PREG                                                                 \r\n"; //임신  
            //SQL += "       ,KOSMOS_OCS.FC_GET_AGE2(a.Pano,a.BDate) FC_AGE2                                                                              \r\n"; //나이  
            //SQL += "       ,KOSMOS_OCS.FC_XRAY_JUSA_CHK(a.Pano,TRUNC(SYSDATE),'NULL') FC_JUSA                                                           \r\n"; //주사체크
            //SQL += "       ,KOSMOS_OCS.FC_IPD_NEW_MASTER_EXAM(a.Pano) FC_EXAM                                                                           \r\n"; //입원정밀
            //SQL += "       ,KOSMOS_OCS.FC_XRAY_COPY_CHK(a.Pano,a.BDate) FC_COPY                                                                         \r\n"; //CD복사
            //SQL += "       ,KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(a.Pano,TRUNC(SYSDATE)) FC_Fall                                                            \r\n"; //낙상
            //SQL += "       ,KOSMOS_OCS.FC_IPD_NEW_MASTER_SECRET(a.Pano) FC_IPD_SECRET                                                                   \r\n"; //입원사생활체
            //SQL += "       ,KOSMOS_OCS.FC_MISU_GAINMST_CHK(a.Pano) FC_Misu                                                                              \r\n"; //개인미수체크
            //SQL += "       ,KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(a.Pano,a.BDate,a.DeptCode) FC_ErSTS                                                      \r\n"; //응급중증   
            //SQL += "       ,KOSMOS_OCS.FC_XRAY_ASA_CHK(a.Pano,a.BDate) FC_ASA_Suga                                                                      \r\n"; //진정수가                       
            SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                                                                          \r\n";
            SQL += "   WHERE 1 = 1                                                                                                                      \r\n";
            
            if (argCls.Pano != "")
            {
                SQL += "   AND a.Pano ='" + argCls.Pano + "'                                                                                            \r\n";
            }
            if (argCls.Date1 != "" )
            {
                SQL += "   AND a.SeekDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                           \r\n";
                SQL += "   AND a.SeekDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                                             \r\n";
            }            
                       
            if (argCls.STS == "03")
            {
                SQL += "   AND a.ROWID IN (" + argCls.ROWID + ")                                                                                        \r\n";
            }
            else if (argCls.STS == "04")
            {
                SQL += "   AND a.Exinfo  = " + argCls.WRTNO + "                                                                                         \r\n";
            }
            else
            {

            }

            if (argCls.DeptCode != "**")
            {
                SQL += "   AND a.DeptCode ='" + argCls.DeptCode + "'                                                                                    \r\n";
            }

            if (argCls.Search != "")
            {
                SQL += "   AND (a.Pano = '" + argCls.Search + "'                                                                                        \r\n";
                SQL += "         OR a.SName LIKE '%" + argCls.Search + "%'  )                                                                           \r\n";
            }
                        

            if (argCls.IPDOPD != "*")
            {
                SQL += "   AND a.IPDOPD ='" + argCls.IPDOPD + "'                                                                                        \r\n";
            }

            if (argCls.GbPort == "Y")
            {
                SQL += "   AND a.GbPortable ='M'                                                                                                        \r\n";
            }
            else
            {
                //SQL += "   AND (a.GbPortable IS NULL OR a.GbPortable <>'M')                                                                             \r\n";
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

        public DataTable sel_XRAY_DETAIL_sName(PsmhDb pDbCon, cXrayDetail argCls,bool bLog)
        {
            DataTable dt = null;

            SQL = "";            
            SQL += " SELECT SNAME,COUNT(SName) CNT                                                                                                      \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT                                                                                           \r\n";
            SQL += "    WHERE 1=1                                                                                                                       \r\n";
            SQL += "      AND PANO IN (                                                                                                                 \r\n";
            SQL += "                   SELECT                                                                                                           \r\n";
            SQL += "                     a.Pano                                                                                                         \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                                                                          \r\n";
            SQL += "       ," + ComNum.DB_MED + "OCS_ORDERCODE b                                                                                        \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_PATIENT c                                                                                         \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_DOCTOR d                                                                                          \r\n";
            SQL += "   WHERE 1 = 1                                                                                                                      \r\n";
            SQL += "    AND a.OrderCode = b.OrderCode(+)                                                                                                \r\n";
            SQL += "    AND a.DrCode    = d.Drcode(+)                                                                                                   \r\n";
            SQL += "    AND a.PANO = c.PANO(+)                                                                                                          \r\n";
            
            if (argCls.Pano != "")
            {
                SQL += "   AND a.Pano ='" + argCls.Pano + "'                                                                                            \r\n";
            }

            if (argCls.STS == "00" || argCls.STS == "01" || argCls.STS == "02")
            {
                if (argCls.Tab == "1")
                {
                    SQL += "  AND ( (a.SeekDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                         \r\n";
                    SQL += "        AND a.SeekDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI') )                                      \r\n";
                    SQL += "      OR ( a.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                          \r\n";
                    SQL += "        AND a.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI') )                                         \r\n";
                    SQL += "       )                                                                                                                    \r\n";
                }
                else if (argCls.Tab == "2")
                {
                    SQL += "   AND a.SeekDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                           \r\n";
                    SQL += "   AND a.SeekDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                                             \r\n";
                }

            }
            else if (argCls.STS == "03")
            {
                SQL += "   AND a.ROWID IN (" + argCls.ROWID + ")                                                                                        \r\n";
            }
            else if (argCls.STS == "04")
            {
                SQL += "   AND a.Exinfo  = " + argCls.WRTNO + "                                                                                         \r\n";
            }
            else
            {

            }

            if (argCls.DeptCode != "**")
            {
                SQL += "   AND a.DeptCode ='" + argCls.DeptCode + "'                                                                                    \r\n";
            }

            if (argCls.Search != "")
            {
                SQL += "   AND (a.Pano = '" + argCls.Search + "'                                                                                        \r\n";
                SQL += "         OR c.SName LIKE '%" + argCls.Search + "%'  )                                                                           \r\n";
            }

            if (argCls.Tab == "1")
            {
                if (argCls.XJong == "*")
                {
                    SQL += "   AND ( a.XJong <= '9'  OR a.XJong ='Q' )                                                                                  \r\n";
                }
                else
                {
                    SQL += "   AND a.XJong ='" + argCls.XJong + "'                                                                                      \r\n";
                }
            }
            else if (argCls.Tab == "2")
            {
                if (argCls.XJong == "*")
                {
                    SQL += "   AND ( a.XJong <= 'A'  OR a.XJong IN ('Q','F') )                                                                          \r\n";
                }
                else
                {
                    SQL += "   AND a.XJong ='" + argCls.XJong + "'                                                                                      \r\n";
                }
            }

            if (argCls.IPDOPD != "*")
            {
                SQL += "   AND a.IPDOPD ='" + argCls.IPDOPD + "'                                                                                        \r\n";
            }

            if (argCls.GbPort == "Y")
            {
                SQL += "   AND a.GbPortable ='M'                                                                                                        \r\n";
            }

            if (argCls.GbER == "Y")
            {
                SQL += "   AND (a.DeptCode ='ER' OR ( a.WardCode ='ER' AND a.RoomCode =100) )                                                           \r\n";
            }

            if (argCls.Tab == "1")
            {
                if (argCls.GbHR == "Y")
                {
                    SQL += "   AND a.DeptCode IN ('HR','TO')                                                                                            \r\n";
                }
                else
                {
                    if (argCls.Job == "OCS접수")
                    {
                        SQL += "   AND a.ORDERNO > 0                                                                                                    \r\n";
                    }
                    else if (argCls.Job == "자동접수")
                    {
                        SQL += "   AND ( a.OrderNo IS NULL OR a.OrderNo = 0 )                                                                           \r\n";
                        SQL += "   AND ( a.Gbend <> '1' OR a.Gbend IS NULL )                                                                            \r\n";
                    }
                }

                if (argCls.Job == "OCS접수" || argCls.Job == "자동접수")
                {
                    SQL += "   AND ( a.GbReserved = '1' OR a.GbReserved = '2' )                                                                         \r\n";
                    SQL += "   AND ( a.GbHIC IS NULL OR GbHIC <> 'Y' )                                                                                  \r\n";
                }
            }
            else if (argCls.Tab == "2")
            {
                SQL += "   AND a.GbReserved IN ('6','7')                                                                                                \r\n";
                SQL += "   AND ( a.Gbend <> '1' OR a.Gbend IS NULL )                                                                                    \r\n";
                SQL += "   AND ( a.GbHIC IS NULL OR GbHIC <> 'Y' )                                                                                      \r\n";
                SQL += "   AND ( a.PacsStudyID IS NULL OR (a.ExInfo IS NULL OR a.ExInfo < 1000) )                                                       \r\n";
            }

            SQL += "   AND TRIM(a.XCode) NOT IN (                                                                                                        \r\n";
            SQL += "                            SELECT TRIM(CODE)                                                                                       \r\n";
            SQL += "                              FROM " + ComNum.DB_PMPA + "BAS_BCODE                                                                  \r\n";
            SQL += "                               WHERE 1=1                                                                                            \r\n";
            SQL += "                                AND GUBUN ='XRAY_OCS명단제외코드'                                                                   \r\n";
            SQL += "                                AND ( DELDATE IS NULL OR DELDATE ='' )                                                              \r\n";
            SQL += "                          )                                                                                                         \r\n";



            SQL += "                         )                                                                                                      \r\n";
            SQL += "   GROUP BY SNAME                                                                                                               \r\n";
            SQL += "    HAVING COUNT(SName) > 1                                                                                                     \r\n";


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
        /// 영상의학과 재료소모량 등록 메인쿼리
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCls"></param>
        /// <returns></returns>
        public DataTable sel_XRAY_DETAIL_MCode(PsmhDb pDbCon, cXrayDetail argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                 \r\n";
            //2018-11-22 안정수 WardCode,RoomCode, IPDOPD 추가
            SQL += "        Pano, Sname, Sex, Age, Wardcode, Roomcode                                                       \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                                \r\n";
            SQL += "   WHERE 1 = 1                                                                                          \r\n";            
            SQL += "    AND SeekDate >= TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD')                                     \r\n";
            SQL += "    AND SeekDate <= TO_DATE('" + argCls.SeekDate + " 23:59','YYYY-MM-DD HH24:MI')                       \r\n";
            if (argCls.Pano != "")
            {
                SQL += "   AND Pano ='" + argCls.Pano + "'                                                                  \r\n";
            }
            if (argCls.Search != "")
            {
                SQL += "   AND (Pano = '" + argCls.Search + "'                                                              \r\n";
                SQL += "         OR SName LIKE '%" + argCls.Search + "%'  )                                                 \r\n";
            }
            SQL += "   AND GbReserved IN ('6','7')                                                                          \r\n";

            if (argCls.Gubun =="0")
            {
                SQL += "   AND ( Gbend <> '1' OR Gbend IS NULL )                                                            \r\n";
            }
            else if (argCls.Gubun == "1")
            {
                SQL += "   AND  Gbend  ='1'                                                                                 \r\n";
            }
            
            if (argCls.XJong == "*")
            {
                SQL += "   AND ( XJong <= '9'  OR XJong ='Q' )                                                              \r\n";
            }
            else
            {
                SQL += "   AND XJong ='" + argCls.XJong + "'                                                                \r\n";
            }
            SQL += "   GROUP BY Pano, Sname, Sex, Age, Wardcode, Roomcode                                                   \r\n";
            SQL += "   ORDER BY SName,Pano                                                                                  \r\n";
            

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
        /// 영상의학과 재표소모량 등록 쿼리
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCls"></param>
        /// <returns></returns>
        public DataTable sel_XRAY_DETAIL_Code(PsmhDb pDbCon, cXrayDetail argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                 \r\n";
            SQL += "        a.XCode, b.XName, a.Qty, a.Agree,a.PacsNo,a.DeptCode,a.DrCode                                   \r\n";
            SQL += "       ,a.MgrNo, a.XJong, a.Exid, a.XRayRoom,a.XSubCode, b.SubCode                                      \r\n";
            SQL += "       ,a.OrderCode,a.orderno,a.Pano,a.Remark,a.DrRemark,a.WardCode,a.RoomCode                          \r\n";
            SQL += "       ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SeekDate                                                       \r\n";
            SQL += "       ,TO_CHAR(a.SeekDate,'HH24:MI') SeekTime                                                          \r\n";
            SQL += "       ,TO_CHAR(a.EnterDate,'YYYY-MM-DD') EnterDate                                                     \r\n";
            SQL += "       ,A.SName,a.N_STS AN_STS, A.N_REMARK AN_REMARK,a.IPDOPD GbIO                                      \r\n";
            SQL += "       ,a.ROWID                                                                                         \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                                              \r\n";
            SQL += "     , " + ComNum.DB_PMPA + "XRAY_CODE b                                                                \r\n";
            SQL += "   WHERE 1 = 1                                                                                          \r\n";
            SQL += "    AND a.XCode = b.XCode                                                                               \r\n";
            SQL += "    AND a.SeekDate >= TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD')                                   \r\n";
            SQL += "    AND a.SeekDate <= TO_DATE('" + argCls.SeekDate + " 23:59','YYYY-MM-DD HH24:MI')                     \r\n";
            if (argCls.Pano != "")
            {
                SQL += "   AND a.Pano ='" + argCls.Pano + "'                                                                \r\n";
            }
            SQL += "   AND a.GbReserved IN ('6','7')                                                                        \r\n";

            if (argCls.Gubun == "0")
            {
                SQL += "   AND ( a.Gbend <> '1' OR a.Gbend IS NULL )                                                        \r\n";
            }
            else if (argCls.Gubun == "1")
            {
                SQL += "   AND  a.Gbend  ='1'                                                                               \r\n";
            }

            if (argCls.XJong == "*")
            {
                SQL += "   AND ( a.XJong <= '9'  OR a.XJong ='Q' )                                                          \r\n";
            }
            else
            {
                SQL += "   AND a.XJong ='" + argCls.XJong + "'                                                              \r\n";
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

        public DataTable sel_XRAY_DETAIL_Copy(PsmhDb pDbCon, cXrayDetail argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                 \r\n";
            SQL += "        SUM(Qty) CNT                                                                                    \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                                              \r\n";            
            SQL += "   WHERE 1 = 1                                                                                          \r\n";            
            SQL += "    AND a.SeekDate >= TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD')                                   \r\n";
            SQL += "    AND a.SeekDate <= TO_DATE('" + argCls.SeekDate + " 23:59','YYYY-MM-DD HH24:MI')                     \r\n";
            if (argCls.Pano != "")
            {
                SQL += "   AND a.Pano ='" + argCls.Pano + "'                                                                \r\n";
            }
            if (argCls.IPDOPD != "")
            {
                SQL += "   AND a.IPDOPD ='" + argCls.IPDOPD + "'                                                            \r\n";
            }
            if (argCls.XCode != "")
            {
                SQL += "   AND a.XCode ='" + argCls.XCode + "'                                                              \r\n";
            }            
            SQL += "   AND a.GbReserved IN ('7')                                                                            \r\n";
            SQL += "   AND (a.GbHIC IS NULL OR a.GbHIC <> 'Y')                                                              \r\n";                       

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

        public DataTable sel_XRAY_DETAIL_ASA_a(PsmhDb pDbCon, cXrayDetail argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                 \r\n";
            SQL += "     a.Pano,c.SName,c.Obst                                                                              \r\n";
            SQL += "    ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                                                \r\n";
            SQL += "    ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SeekDate                                                          \r\n";
            SQL += "    ,TO_CHAR(a.RDate,'YYYY-MM-DD') RDate                                                                \r\n";
            SQL += "    ,TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI') OrderDate                                                \r\n";
            SQL += "    ,a.IPDOPD,c.Sex,a.DeptCode,c.Jumin1 || '-' || c.Jumin2 AS Jumin               \r\n";            
            SQL += "    ,a.XJong,c.Sex,a.DeptCode,a.DrCode,d.DrName                                                \r\n";
            SQL += "    ,a.WardCode,a.RoomCode,a.GbPortable,e.XName                                                         \r\n";
            SQL += "    ,a.XCode,a.XSubCode,a.Qty,a.Remark,a.DrRemark,a.OrderCode,a.OrderName,a.ASA,a.cRemark,a.ROWID       \r\n";            
            SQL += "    ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_XRAY_접수종류',TRIM(a.XJong)) FC_XJong2                       \r\n"; //종류체크
            SQL += "    ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,a.BDate) FC_infect                                     \r\n"; //감염체크
            SQL += "    ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Pano,a.BDate) FC_infect_EX                               \r\n"; //감염체크
            SQL += "    ,KOSMOS_OCS.FC_BAS_AREA_WON(a.Pano,a.IPDOPD,a.BI,a.DrCode) FC_AREA                                  \r\n"; //원거리       
            SQL += "    ,KOSMOS_OCS.FC_PREGNANT_CHK(a.IPDOPD,a.Pano,a.BDate) FC_PREG                                        \r\n"; //임신  
            SQL += "    ,KOSMOS_OCS.FC_GET_AGE2(a.Pano,a.BDate) FC_AGE2                                                     \r\n"; //나이  
            SQL += "    ,KOSMOS_OCS.FC_XRAY_JUSA_CHK(a.Pano,TRUNC(SYSDATE),'NULL') FC_JUSA                                  \r\n"; //주사체크
            SQL += "    ,KOSMOS_OCS.FC_IPD_NEW_MASTER_EXAM(a.Pano) FC_EXAM                                                  \r\n"; //입원정밀
            SQL += "    ,KOSMOS_OCS.FC_XRAY_COPY_CHK(a.Pano,TRUNC(SYSDATE)) FC_COPY                                         \r\n"; //CD복사
            SQL += "    ,KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(a.Pano,TRUNC(SYSDATE)) FC_Fall                                   \r\n"; //낙상
            SQL += "    ,KOSMOS_OCS.FC_IPD_NEW_MASTER_SECRET(a.Pano) FC_IPD_SECRET                                          \r\n"; //입원사생활체
            SQL += "    ,KOSMOS_OCS.FC_MISU_GAINMST_CHK(a.Pano) FC_Misu                                                     \r\n"; //개인미수체크
            SQL += "    ,KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(a.Pano,a.BDate,a.DeptCode) FC_ErSTS                             \r\n"; //응급중증 
            SQL += "    ,KOSMOS_OCS.FC_OCS_ORDERCODE_NAME(a.OrderCode) FC_OrderName                                         \r\n"; //오더명칭 
            SQL += "    ,KOSMOS_OCS.FC_XRAY_CT_CONTRAST_CHK(a.OrderCode) FC_CT_CONTRAST                                     \r\n"; //CT 조영제체크   

            SQL += "    ,( SELECT MAX(o.PTNO) AS ASA_Ptno                                                                   \r\n";
            SQL += "       FROM " + ComNum.DB_MED + "OCS_IORDER o                                                           \r\n";
            SQL += "         WHERE 1 = 1                                                                                    \r\n";
            SQL += "          AND o.Ptno = a.Pano                                                                           \r\n";
            SQL += "          AND o.BDate = a.BDate                                                                         \r\n";
            if (argCls.GbASA == "Y")
            {
                SQL += "          AND ( TRIM(o.SuCode) IN  ( SELECT TRIM(CODE)                                              \r\n";
                SQL += "                                    FROM " + ComNum.DB_PMPA + "BAS_BCODE                            \r\n";
                SQL += "                                      WHERE 1=1                                                     \r\n";
                SQL += "                                       AND Gubun ='마취_신체등급(ASA)_수가코드'                     \r\n";
                SQL += "                                       AND (DelDate IS NULL OR DelDate ='') ) OR                    \r\n";
                SQL += "               TRIM(o.SuCode) IN  ( SELECT TRIM(CODE)                                               \r\n";
                SQL += "                                    FROM " + ComNum.DB_PMPA + "BAS_BCODE                            \r\n";
                SQL += "                                      WHERE 1=1                                                     \r\n";
                SQL += "                                       AND Gubun ='마취_신체등급(ASA)_약수가코드'                   \r\n";
                SQL += "                                       AND (DelDate IS NULL OR DelDate ='') ) OR                    \r\n";
                SQL += "               TRIM(o.SuCode) IN  ( SELECT TRIM(CODE)                                               \r\n";
                SQL += "                                    FROM " + ComNum.DB_PMPA + "BAS_BCODE                            \r\n";
                SQL += "                                      WHERE 1=1                                                     \r\n";
                SQL += "                                       AND Gubun ='C#_Xray_진정관리_약코드'                         \r\n";
                SQL += "                                       AND (DelDate IS NULL OR DelDate ='') )                       \r\n";
                SQL += "                                  )                                                                 \r\n";
                SQL += "          AND TRIM(o.ASA) IN  ( SELECT TRIM(CODE)                                                   \r\n";
                SQL += "                                    FROM " + ComNum.DB_PMPA + "BAS_BCODE                            \r\n";
                SQL += "                                      WHERE 1=1                                                     \r\n";
                SQL += "                                       AND Gubun ='마취_신체등급(ASA)'                              \r\n";
                SQL += "                                       AND (DelDate IS NULL OR DelDate ='') )                       \r\n";
            }
            else if (argCls.GbCT == "Y")
            {
                SQL += "          AND o.Bun IN  ('72')                                                                      \r\n";
            }
            if (argCls.GbNotEndo =="Y")
            {
                SQL += "          AND o.Bun NOT IN  ('48','49')                                                             \r\n";
            }
            SQL += "          GROUP BY o.SuCode                                                                             \r\n";
            SQL += "           HAVING SUM(o.Qty*o.Nal) <> 0                                                                 \r\n";
            SQL += "      UNION                                                                                             \r\n";
            SQL += "      SELECT MAX(o.PTNO) AS ASA_Ptno                                                                   \r\n";
            SQL += "       FROM " + ComNum.DB_MED + "OCS_oORDER o                                                           \r\n";
            SQL += "         WHERE 1 = 1                                                                                    \r\n";
            SQL += "          AND o.Ptno = a.Pano                                                                           \r\n";
            SQL += "          AND o.BDate = a.BDate                                                                         \r\n";
            if (argCls.GbASA == "Y")
            {
                SQL += "          AND ( TRIM(o.SuCode) IN  ( SELECT TRIM(CODE)                                              \r\n";
                SQL += "                                    FROM " + ComNum.DB_PMPA + "BAS_BCODE                            \r\n";
                SQL += "                                      WHERE 1=1                                                     \r\n";
                SQL += "                                       AND Gubun ='마취_신체등급(ASA)_수가코드'                     \r\n";
                SQL += "                                       AND (DelDate IS NULL OR DelDate ='') ) OR                    \r\n";
                SQL += "               TRIM(o.SuCode) IN  ( SELECT TRIM(CODE)                                               \r\n";
                SQL += "                                    FROM " + ComNum.DB_PMPA + "BAS_BCODE                            \r\n";
                SQL += "                                      WHERE 1=1                                                     \r\n";
                SQL += "                                       AND Gubun ='마취_신체등급(ASA)_약수가코드'                   \r\n";
                SQL += "                                       AND (DelDate IS NULL OR DelDate ='') )                       \r\n";
                SQL += "                                  )                                                                 \r\n";
                SQL += "          AND TRIM(o.ASA) IN  ( SELECT TRIM(CODE)                                                   \r\n";
                SQL += "                                    FROM " + ComNum.DB_PMPA + "BAS_BCODE                            \r\n";
                SQL += "                                      WHERE 1=1                                                     \r\n";
                SQL += "                                       AND Gubun ='마취_신체등급(ASA)'                              \r\n";
                SQL += "                                       AND (DelDate IS NULL OR DelDate ='') )                       \r\n";
            }
            else if (argCls.GbCT == "Y")
            {
                SQL += "          AND o.Bun IN  ('72')                                                                      \r\n";
            }
            if (argCls.GbNotEndo == "Y")
            {
                SQL += "          AND o.Bun NOT IN  ('48','49')                                                             \r\n";
            }
            SQL += "          GROUP BY o.SuCode                                                                             \r\n";
            SQL += "           HAVING SUM(o.Qty*o.Nal) <> 0                                                                 \r\n";

            SQL += "     ) AS ASA_Ptno                                                                                      \r\n";

            SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                                              \r\n";            
            SQL += "       ," + ComNum.DB_MED + "OCS_ORDERCODE b                                                            \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_PATIENT c                                                             \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_DOCTOR d                                                              \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "XRAY_CODE e                                                               \r\n";
            SQL += "   WHERE 1 = 1                                                                                          \r\n";
            SQL += "    AND a.OrderCode = b.OrderCode(+)                                                                    \r\n";
            SQL += "    AND a.DrCode    = d.Drcode(+)                                                                       \r\n";
            SQL += "    AND a.PANO = c.PANO(+)                                                                              \r\n";
            SQL += "    AND a.XCode = e.XCode(+)                                                                            \r\n";
            SQL += "    AND a.SeekDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                      \r\n";
            SQL += "    AND a.SeekDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                        \r\n";
            if (argCls.Pano != "")
            {
                SQL += "   AND a.Pano ='" + argCls.Pano + "'                                                                \r\n";
            }
            if (argCls.Search != "")
            {
                SQL += "   AND (a.Pano = '" + argCls.Search + "'                                                            \r\n";
                SQL += "         OR c.SName LIKE '%" + argCls.Search + "%'  )                                               \r\n";
            }
            if (argCls.IPDOPD != "*")
            {
                SQL += "   AND a.IPDOPD ='" + argCls.IPDOPD + "'                                                            \r\n";
            }
            if (argCls.GbCT =="Y")
            {
                SQL += "   AND a.XJong IN ('4')                                                                             \r\n";
            }
            else if (argCls.GbASA == "Y")
            {
                if (argCls.GbNotEndo =="Y")
                {
                    SQL += "   AND a.XJong IN ('4','5')                                                                     \r\n";
                }
                else if (argCls.GbNotRi == "Y")
                {
                    SQL += "   AND a.XJong IN ('4','5','6')                                                                 \r\n";
                }
            }
            if (argCls.GbASA_Suga =="Y")
            {
                SQL += "   AND KOSMOS_OCS.FC_XRAY_ASA_CHK(A.Pano,a.BDate) ='Y'                                              \r\n";
            }
            SQL += "   AND a.PacsNo IS NOT NULL                                                                             \r\n";
            SQL += "   AND a.DeptCode NOT IN ('HR','TO','ER')                                                               \r\n";
            SQL += "   AND (a.GbHIC IS NULL OR a.GbHIC <> 'Y')                                                              \r\n";
            SQL += " ORDER BY a.SeekDate,a.BDate                                                                            \r\n";

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

        public DataTable sel_XRAY_DETAIL_ASA_b(PsmhDb pDbCon, cXrayDetail argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                 \r\n";
            SQL += "     a.Pano,c.SName,c.Obst                                                                              \r\n";
            SQL += "    ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                                                \r\n";
            SQL += "    ,'' SeekDate                                                                                        \r\n";
            SQL += "    ,'' RDate                                                                                           \r\n";
            SQL += "    ,'' OrderDate                                                                                       \r\n";
            SQL += "    ,DECODE(a.WardCode,'','O','I') IPDOPD,c.Sex,a.DeptCode,c.Jumin1 || '-' || c.Jumin2 AS Jumin         \r\n";            
            SQL += "    ,'' XJong,c.Sex,a.DeptCode,a.DrCode,d.DrName                                                        \r\n";
            SQL += "    ,a.WardCode,'' RoomCode,'' GbPortable,'' XName                                                      \r\n";
            SQL += "    ,'' XCode,'' XSubCode,'' Qty,a.Remark,a.Remark2,'' OrderCode,'' OrderName,'' ASA,'' cRemark,a.ROWID \r\n";
            SQL += "    ,KOSMOS_OCS.FC_NUR_HAPPYCALL_OPD3(a.Pano,'04',a.BDate,a.DeptCode) FC_HappyCallA                    \r\n"; //해피콜     
            SQL += "    ,KOSMOS_OCS.FC_NUR_HAPPYCALL_OPD3(a.Pano,'07',a.BDate,a.DeptCode) FC_HappyCallB                    \r\n"; //해피콜     
            SQL += "    ,'' FC_XJong2                                                                                       \r\n"; //종류체크
            SQL += "    ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,a.BDate) FC_infect                                     \r\n"; //감염체크
            SQL += "    ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Pano,a.BDate) FC_infect_EX                               \r\n"; //감염체크
            SQL += "    ,'' FC_AREA                                                                                         \r\n"; //원거리       
            SQL += "    ,'' FC_PREG                                                                                         \r\n"; //임신  
            SQL += "    ,KOSMOS_OCS.FC_GET_AGE2(a.Pano,a.BDate) FC_AGE2                                                     \r\n"; //나이  
            SQL += "    ,KOSMOS_OCS.FC_XRAY_JUSA_CHK(a.Pano,TRUNC(SYSDATE),'NULL') FC_JUSA                                  \r\n"; //주사체크
            SQL += "    ,KOSMOS_OCS.FC_IPD_NEW_MASTER_EXAM(a.Pano) FC_EXAM                                                  \r\n"; //입원정밀
            SQL += "    ,KOSMOS_OCS.FC_XRAY_COPY_CHK(a.Pano,TRUNC(SYSDATE)) FC_COPY                                         \r\n"; //CD복사
            SQL += "    ,KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(a.Pano,TRUNC(SYSDATE)) FC_Fall                                   \r\n"; //낙상
            SQL += "    ,KOSMOS_OCS.FC_IPD_NEW_MASTER_SECRET(a.Pano) FC_IPD_SECRET                                          \r\n"; //입원사생활체
            SQL += "    ,KOSMOS_OCS.FC_MISU_GAINMST_CHK(a.Pano) FC_Misu                                                     \r\n"; //개인미수체크
            SQL += "    ,KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(a.Pano,a.BDate,a.DeptCode) FC_ErSTS                             \r\n"; //응급중증 
            SQL += "    ,'' FC_OrderName                                                                                    \r\n"; //오더명칭   
            SQL += "    ,a.Gubun                                                                                            \r\n"; // "01" = ASA, "02" = 조영제
            SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_ASA a                                                                 \r\n";            
            SQL += "       ," + ComNum.DB_PMPA + "BAS_PATIENT c                                                             \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_DOCTOR d                                                              \r\n";            
            SQL += "   WHERE 1 = 1                                                                                          \r\n";            
            SQL += "    AND a.DrCode    = d.Drcode(+)                                                                       \r\n";
            SQL += "    AND a.PANO = c.PANO(+)                                                                              \r\n";            
            SQL += "    AND a.BDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                         \r\n";
            SQL += "    AND a.BDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                           \r\n";
            
            if (argCls.Pano != "")
            {
                SQL += "   AND a.Pano ='" + argCls.Pano + "'                                                                \r\n";
            }
            if (argCls.Search != "")
            {
                SQL += "   AND (a.Pano = '" + argCls.Search + "'                                                            \r\n";
                SQL += "         OR c.SName LIKE '%" + argCls.Search + "%'  )                                               \r\n";
            } 

            //2018-05-15 안정수, 권현선 간호사 요청으로 조건 추가
            if (argCls.GbASA != "")
            {                
                SQL += "   AND a.Gubun = '01'                                                                               \r\n";
            }

            else if (argCls.GbCT != "")
            {
                SQL += "   AND a.Gubun = '02'                                                                               \r\n";
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

        public DataTable sel_XRAY_DETAIL(PsmhDb pDbCon,string argJob, string argPano, string argPacsNo,string argROWID)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                         \r\n";
            SQL += "  MgrNo,ROWID                                                                   \r\n";
            SQL += "  ,Exid,XCode,XJong,OrderNo,Remark,DrRemark                                     \r\n";
            SQL += "  ,Pano,SName,Sex,Age,DeptCode,DrCode                                           \r\n";
            SQL += "  ,IpdOpd,WardCode,RoomCode,XrayRoom                                            \r\n";
            SQL += "  ,PacsNo,OrderCode,OrderName,PacsStudyID                                       \r\n";
            SQL += "  ,TO_CHAR(SeekDate,'YYYY-MM-DD') BDate                                         \r\n";
            SQL += "  ,TO_CHAR(SYSDATE,'YYYYMMDDHH24MI') EntDate                                    \r\n";
            SQL += "  ,TO_CHAR(SeekDate,'YYYYMMDDHH24MI') SeekDate                                  \r\n";
            SQL += "  ,KOSMOS_OCS.FC_OCS_DOCTOR_SABUN(DrCode) FC_DrSabun                            \r\n"; //의사사번 
            SQL += "  ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DrCode) FC_DrName                            \r\n"; //의사성명 
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                   \r\n";
            if (argJob =="00")                
            {
                SQL += "   AND Pano ='" + argPano + "'                                              \r\n";
                SQL += "   AND PacsNo = '" + argPacsNo + "'                                         \r\n";
                SQL += "   AND PacsStudyID IS NOT NULL                                              \r\n";
            }
            else if (argJob == "01")
            {
                if (argPano !="")
                {
                    SQL += "   AND Pano ='" + argPano + "'                                          \r\n";
                }
                if (argPacsNo != "")
                {
                    SQL += "   AND PacsNo ='" + argPacsNo + "'                                      \r\n";
                }
            }
            else if (argJob == "02")
            {                
                SQL += "   AND ROWID ='" + argROWID + "'                                            \r\n"; 
            }
            else if (argJob == "03")
            {
                SQL += "   AND PacsStudyID IS NOT NULL                                              \r\n";
                SQL += "   AND ROWID IN (" + argROWID + ")                                          \r\n";                
            }
            else if (argJob == "04")
            {
                SQL += "   AND PacsStudyID IS NULL                                                  \r\n";
                SQL += "   AND ROWID IN (" + argROWID + ")                                          \r\n";
            }
            if (argJob == "03" || argJob == "04")
            {
                SQL += "  ORDER BY PacsNo                                                           \r\n";
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

        public DataTable sel_XRAY_DETAIL_MaxMgrno(PsmhDb pDbCon, string argSeekDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                         \r\n";
            SQL += "  Max(MgrNo) + 1  Mgr                                                           \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                   \r\n";
            SQL += "   AND SeekDate >=TO_DATE('" + argSeekDate + "','YYYY-MM-DD')                   \r\n";
            SQL += "   AND SeekDate <=TO_DATE('" + argSeekDate + " 23:59','YYYY-MM-DD HH24:MI')     \r\n";

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
        /// 영상의학 미시행 관련
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCls"></param>
        /// <returns></returns>
        public DataTable sel_XRAY_DETAIL_NoEXE(PsmhDb pDbCon, cXrayDetail argCls)
        {
            DataTable dt = null;

            SQL = "";
                     
            SQL += " SELECT                                                                                                                         \r\n";
            SQL += "        a.Pano,c.SName,a.ROWID                                                                                                  \r\n";            
            SQL += "       ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                                                                     \r\n";
            SQL += "       ,TO_CHAR(a.SeekDate,'YYYY-MM-DD HH24:MI') SeekDate                                                                       \r\n";
            SQL += "       ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SeekDate2                                                                              \r\n";            
            SQL += "       ,TO_CHAR(a.RDate,'YYYY-MM-DD') RDate                                                                                     \r\n";
            SQL += "       ,TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI') OrderDate                                                                     \r\n";
            SQL += "       ,DECODE(a.GBSTS,'1','접수','2','예약','3','촬영전','S','호명','7','완료','D','삭제','미접수') STS                           \r\n";
            SQL += "       ,a.XJong,a.IPDOPD,a.DeptCode,a.DrCode,a.WardCode,a.RoomCode,a.ASA                                                        \r\n";
            SQL += "       ,a.OrderNo,a.DeptCode,a.WardCode,a.RoomCode,a.Remark,a.cRemark,a.gSabun                                                  \r\n";
            SQL += "       ,b.OrderCode,b.SuCode,b.OrderName,c.Sex,c.Obst,c.Jumin1 || '-' || c.Jumin2 AS Jumin,d.DrName                             \r\n";
            SQL += "       ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('XRAY_방사선종류',TRIM(a.XJong)) FC_XJong2                                              \r\n"; //종류체크
                       
            ////function
            ////SQL += "   ,KOSMOS_OCS.FC_BAS_AUTO_MST_CHK(a.Ptno,a.BDate) autoSTS                                                                      \r\n"; //후불체크
            //SQL += "   ,KOSMOS_OCS.FC_NUR_HAPPYCALL_OPD(a.Pano,'05','ENDO_JUPMST',a.ROWID) FC_happycall                                             \r\n"; //해피콜체크
            ////SQL += "   ,KOSMOS_OCS.FC_BAS_BUSE_NAME(a.Buse) FC_BuseName                                                                             \r\n"; //부서이름
            //SQL += "   ,KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(a.Pano,TRUNC(SYSDATE)) FC_Fall                                                            \r\n"; //낙상
            //SQL += "   ,KOSMOS_OCS.FC_OPD_RESERVED_NEW_NEAR(a.Pano,a.DeptCode) FC_opdRes                                                            \r\n"; //예약접수정보
            ////SQL += "   ,KOSMOS_OCS.FC_OCS_ITRANSFER_CHK(a.Ptno,'MG') FC_Consult                                                                     \r\n"; //협진체크
            //SQL += "   ,KOSMOS_OCS.FC_OPD_SLIP_SUNAP_CHK(a.Pano,a.BDate,b.SuCode,a.OrderNo) FC_SUNAP                                                \r\n"; //수납체크
            SQL += "   ,KOSMOS_OCS.FC_GET_AGE(TO_CHAR(c.Birth,'YYYY-MM-DD'),a.BDate) FC_age                                                         \r\n"; //나이체크
            SQL += "   ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,a.BDate) FC_infect                                                              \r\n"; //감염체크
            //SQL += "   ,KOSMOS_OCS.FC_IPD_NEW_MASTER_JSTS2(KOSMOS_OCS.FC_IPD_NEW_MASTER_JSTS(a.Pano)) FC_Ipd_Info                                   \r\n"; //재원체크
            //SQL += "   ,KOSMOS_OCS.FC_OCS_ITRANSFER_CHK2(a.Pano,'MG') FC_Consult                                                                    \r\n"; //협진체크
            ////SQL += "   ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_ENDO_도착구분',a.sGubun2) FC_sGubun2                                                    \r\n"; //도착구분                        
            SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                                                                      \r\n";
            SQL += "       ," + ComNum.DB_MED + "OCS_ORDERCODE b                                                                                    \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_PATIENT c                                                                                     \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_DOCTOR d                                                                                      \r\n";
            SQL += "   WHERE 1 = 1                                                                                                                  \r\n";
            SQL += "    AND a.OrderCode = b.OrderCode(+)                                                                                            \r\n";
            SQL += "    AND a.DrCode    = d.Drcode(+)                                                                                               \r\n";
            SQL += "    AND a.PANO = c.PANO(+)                                                                                                      \r\n";
            

            if (argCls.Pano != "")
            {
                SQL += "   AND a.Pano ='" + argCls.Pano + "'                                                                                        \r\n";
            }
                        
            SQL += "  AND ( (a.BDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                                \r\n";
            SQL += "        AND a.BDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI') )                                             \r\n";
            SQL += "      OR ( a.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                              \r\n";
            SQL += "        AND a.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI') )                                             \r\n";
            SQL += "       )                                                                                                                        \r\n";
               
            if (argCls.DeptCode == "**")
            {
                SQL += "   AND a.DeptCode NOT IN ('HR','TO')                                                                                        \r\n";
            }
            else
            {
                SQL += "   AND a.DeptCode ='" + argCls.DeptCode + "'                                                                                \r\n";
            }

            if (argCls.Search != "")
            {
                SQL += "   AND (a.Pano = '" + argCls.Search + "'                                                                                    \r\n";
                SQL += "         OR c.SName LIKE '%" + argCls.Search + "%'  )                                                                       \r\n";
            }
                    
            if (argCls.XJong == "*")
            {
                SQL += "   AND  a.XJong <= '9'                                                                                                      \r\n";
            }
            else
            {
                SQL += "   AND a.XJong ='" + argCls.XJong + "'                                                                                      \r\n";
            }           
            
            if (argCls.IPDOPD != "*")
            {
                SQL += "   AND a.IPDOPD ='" + argCls.IPDOPD + "'                                                                                    \r\n";
            }

            if (argCls.GbNoExe == "Y")
            {
                SQL += "   AND ( a.GBSTS IS NULL OR a.GBSTS <> '7' )                                                                                \r\n";
                SQL += "   AND ( a.DelDate IS NULL OR a.DelDate = '' )                                                                              \r\n";
            }            

            SQL += "   AND TRIM(a.XCode) NOT IN ( 'CAGCOPY','GR9701','CUSCOPY','US11M','G0400A','G04009','G0400')                                   \r\n";

            //SQL += "   AND TRIM(a.XCode) NOT IN (                                                                                                        \r\n";
            //SQL += "                            SELECT TRIM(CODE)                                                                                       \r\n";
            //SQL += "                              FROM " + ComNum.DB_PMPA + "BAS_BCODE                                                                  \r\n";
            //SQL += "                               WHERE 1=1                                                                                            \r\n";
            //SQL += "                                AND GUBUN ='XRAY_OCS명단제외코드'                                                                    \r\n";
            //SQL += "                                AND ( DELDATE IS NULL OR DELDATE ='' )                                                              \r\n";
            //SQL += "                          )                                                                                                         \r\n";

            SQL += "   ORDER BY a.SeekDate,a.Pano,a.DeptCode                                                                                        \r\n";
           

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
        /// 영상의학과 촬영 workList
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCls"></param>
        /// <returns></returns>
        public DataTable sel_XRAY_DETAIL_WorkList(PsmhDb pDbCon, cXrayDetail argCls,bool bLog)
        {
            DataTable dt = null; 

            SQL = "";
                       
            SQL += " SELECT                                                                                                                         \r\n";
            SQL += "        a.Pano,c.SName,c.Obst,c.Sex                                                                                             \r\n";
            SQL += "       ,a.XJong,a.GbPortable                                                                                                    \r\n";
            SQL += "       ,a.IPDOPD,c.JICODE,a.GbER,a.ASA                                                                                          \r\n";
            if (argCls.Job =="00")
            {
                SQL += "       ,MIN(a.PacsNo) PacsNo, COUNT(*) CNT                                                                                  \r\n";
                SQL += "       ,SUM(DECODE(a.DeptCode,'HR',1,0)) HrCNT                                                                              \r\n";
                SQL += "       ,SUM(DECODE(a.DeptCode,'TO',1,0)) ToCNT                                                                              \r\n";
                SQL += "       ,SUM(DECODE(a.DeptCode,'ER',1,0)) EmCNT                                                                              \r\n";
                SQL += "       ,SUM(DECODE(a.DRCode,'1107',1,'1125',1,0)) RaCNT                                                                     \r\n";
                SQL += "       ,SUM(DECODE(a.DeptCode,'RM',1,0)) RmCNT                                                                              \r\n";
                SQL += "       ,SUM(DECODE(a.DeptCode,'PD',1,0)) PdCNT                                                                              \r\n";

            }
            else if (argCls.Job == "01")
            {
                SQL += "       ,a.PacsNo,a.Remark,a.ROWID                                                                                           \r\n";
                SQL += "       ,a.DrRemark,a.OrderCode,a.OrderNo,a.OrderName,a.PickupRemark                                                         \r\n";
                SQL += "       ,TO_CHAR(a.SeekDate,'HH24:MI') SeekDate                                                                              \r\n";
                SQL += "       ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SeekDate2                                                                          \r\n";
                SQL += "       ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                                                                 \r\n";
                SQL += "       ,a.DeptCode,a.IpdOpd,DECODE(a.IpdOpd,'I','입원','외래') IpdOpd1                                                      \r\n";
                SQL += "       ,a.DrCode,d.DrName,a.Exinfo,a.RoomCode,a.PacsStudyID                                                                 \r\n";
                SQL += "       ,KOSMOS_OCS.FC_OCS_DOCTOR_SABUN(a.DRCODE) FC_DrSabun                                                                 \r\n"; //의사사번 
            }
            else if (argCls.Job == "02")
            {
                SQL += "       ,a.DeptCode,a.DrCode,d.DrName,a.ExInfo,a.PacsNo,a.XCode,a.OrderNo,a.Age                                              \r\n";
                SQL += "       ,a.PacsStudyID,a.OrderName,a.Remark,a.DrRemark,a.GbReserved,a.PickupRemark                                           \r\n";
                SQL += "       ,a.ExID,a.XRayRoom,a.GbRead,a.N_STS AN_STS, a.N_REMARK AN_REMARK,a.ROWID                                             \r\n";
                SQL += "       ,TO_CHAR(a.EnterDate,'YYYY-MM-DD') EnterDate                                                                         \r\n";
                SQL += "       ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SeekDate                                                                           \r\n";
                SQL += "       ,TO_CHAR(a.SeekDate,'HH24:MI') SeekTime                                                                              \r\n";
                //2019-01-03 안정수, 한창희s 요청으로 roomcode, wardcode 추가함
                SQL += "       ,a.WardCode, a.RoomCode                                                                                              \r\n";
                //2019-01-15 안정수, bdate 추가함
                //2019-02-01 안정수, 황정기c 요청으로 PICKUPREMARK 추가 
                SQL += "       ,a.bdate, a.PICKUPREMARK                                                                                             \r\n";
                //SQL += "       ,KOSMOS_OCS.FC_OCS_ORDERCODE_NAME2(a.OrderCode) FC_OrderName                                                         \r\n"; //오더명칭
                SQL += "       ,(  SELECT CASE WHEN COUNT(*) > 0 THEN   max(DECODE(trim(DispHeader),'',trim(OrderName),trim(DispHeader)))                   \r\n";
                SQL += "                ELSE ''                                                                                                             \r\n";
                SQL += "            END                                                                                                                     \r\n";
                SQL += "            FROM KOSMOS_OCS.OCS_ORDERCODE                                                                                           \r\n";
                SQL += "            WHERE ORDERCODE like a.OrderCode||'%') FC_OrderName                                                                     \r\n";
                //2019-08-23 안정수 추가
                SQL += "       ,KOSMOS_OCS.FC_XRAY_CT_CONST_CHK(a.Pano,a.BDate) FC_CT_Const                                                                 \r\n";  //CT constr            


            }   
            ////function
            ////SQL += "   ,KOSMOS_OCS.FC_BAS_AUTO_MST_CHK(a.Ptno,a.BDate) autoSTS                                                                      \r\n"; //후불체크
            //SQL += "   ,KOSMOS_OCS.FC_NUR_HAPPYCALL_OPD(a.Pano,'05','ENDO_JUPMST',a.ROWID) FC_happycall                                             \r\n"; //해피콜체크
            ////SQL += "   ,KOSMOS_OCS.FC_BAS_BUSE_NAME(a.Buse) FC_BuseName                                                                             \r\n"; //부서이름
            //SQL += "   ,KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(a.Pano,TRUNC(SYSDATE)) FC_Fall                                                            \r\n"; //낙상
            //SQL += "   ,KOSMOS_OCS.FC_OPD_RESERVED_NEW_NEAR(a.Pano,a.DeptCode) FC_opdRes                                                            \r\n"; //예약접수정보
            ////SQL += "   ,KOSMOS_OCS.FC_OCS_ITRANSFER_CHK(a.Ptno,'MG') FC_Consult                                                                     \r\n"; //협진체크
            //SQL += "   ,KOSMOS_OCS.FC_OPD_SLIP_SUNAP_CHK(a.Pano,a.BDate,b.SuCode,a.OrderNo) FC_SUNAP                                                \r\n"; //수납체크
            SQL += "   ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('XRAY_방사선종류',TRIM(a.XJong)) FC_XJong2                                                  \r\n"; //종류체크            
            SQL += "   ,KOSMOS_OCS.FC_GET_AGE(TO_CHAR(c.Birth,'YYYY-MM-DD'),a.BDate) FC_age                                                             \r\n"; //나이체크
            SQL += "   ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,a.BDate) FC_infect                                                                  \r\n"; //감염체크
            SQL += "   ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Pano,a.BDate) FC_infect_EX                                                            \r\n"; //감염체크
            SQL += "   ,KOSMOS_OCS.FC_IPD_NEW_MASTER_JSTS2(KOSMOS_OCS.FC_IPD_NEW_MASTER_JSTS(a.Pano)) FC_Ipd_Info                                   \r\n"; //재원체크
            //SQL += "   ,KOSMOS_OCS.FC_OCS_ITRANSFER_CHK2(a.Pano,'MG') FC_Consult                                                                    \r\n"; //협진체크
            ////SQL += "   ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_ENDO_도착구분',a.sGubun2) FC_sGubun2                                                    \r\n"; //도착구분       
            //SQL += "   ,KOSMOS_OCS.FC_OPD_MASTER_PREGNANT(a.Pano,a.BDate) FC_OPD_PREGNANT                                                               \r\n"; //외래임신체크     
            //SQL += "   ,KOSMOS_OCS.FC_IPD_NEW_MASTER_PREGNANT(a.Pano) FC_IPD_PREGNANT                                                                   \r\n"; //입원임신체크     
            //SQL += "   ,KOSMOS_OCS.FC_IPD_NEW_MASTER_SECRET(a.Pano) FC_IPD_SECRET                                                                       \r\n"; //입원사생활체크   

            SQL += "   ,KOSMOS_OCS.FC_BAS_AREA_WON(a.Pano,a.IPDOPD,a.BI,a.DrCode) FC_AREA                                                               \r\n"; //원거리       
            SQL += "   ,KOSMOS_OCS.FC_PREGNANT_CHK(a.IPDOPD,a.Pano,a.BDate) FC_PREG                                                                     \r\n"; //임신  
            SQL += "   ,KOSMOS_OCS.FC_GET_AGE2(a.Pano,a.BDate) FC_AGE2                                                                                  \r\n"; //나이  
            SQL += "   ,KOSMOS_OCS.FC_XRAY_JUSA_CHK(a.Pano,TRUNC(A.ENTERDATE),'NULL') FC_JUSA                                                              \r\n"; //주사체크
            //SQL += "   ,KOSMOS_OCS.FC_IPD_NEW_MASTER_EXAM(a.Pano) FC_EXAM                                                                               \r\n"; //입원정밀
            //SQL += "   ,KOSMOS_OCS.FC_XRAY_COPY_CHK(a.Pano,TRUNC(SYSDATE)) FC_COPY                                                                      \r\n"; //CD복사
            SQL += "   ,KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(a.Pano,TRUNC(SYSDATE)) FC_Fall                                                                \r\n"; //낙상
            SQL += "   ,KOSMOS_OCS.FC_IPD_NEW_MASTER_SECRET(a.Pano) FC_IPD_SECRET                                                                       \r\n"; //입원사생활체
            //SQL += "   ,KOSMOS_OCS.FC_MISU_GAINMST_CHK(a.Pano) FC_Misu                                                                                  \r\n"; //개인미수체크
            
            SQL += "   ,KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(a.Pano,a.BDate,a.DeptCode) FC_ErSTS                                                          \r\n"; //응급중증    
            //SQL += "   ,KOSMOS_OCS.FC_OCS_DOCTOR_SABUN(a.DRCODE) FC_DrSabun                                                                             \r\n"; //의사사번             
            SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                                                                          \r\n";
            SQL += "       ," + ComNum.DB_MED + "OCS_ORDERCODE b                                                                                        \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_PATIENT c                                                                                         \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_DOCTOR d                                                                                          \r\n";
            SQL += "   WHERE 1 = 1                                                                                                                      \r\n";
            SQL += "    AND a.OrderCode = b.OrderCode(+)                                                                                                \r\n";
            SQL += "    AND a.DrCode    = d.Drcode(+)                                                                                                   \r\n";
            SQL += "    AND a.PANO = c.PANO(+)                                                                                                          \r\n";
            
            if (argCls.Pano != "")
            {
                SQL += "   AND a.Pano ='" + argCls.Pano + "'                                                                                            \r\n";
            }

            if (argCls.Search != "")
            {
                SQL += "   AND (a.Pano = '" + argCls.Search + "'                                                                                        \r\n";
                SQL += "         OR c.SName LIKE '%" + argCls.Search + "%'  )                                                                           \r\n";
            }

            if (argCls.Job =="00" || argCls.Job =="01")
            {
                #region //구분에 따른 조건
                if (argCls.Gubun == "1")         //대기환자
                {
                    SQL += "   AND a.Pacs_End IS NULL                                                                                                       \r\n";
                    if (argCls.Job == "00")
                    {
                        SQL += "   AND a.XRayRoom NOT IN ('T','H')                                                                                          \r\n";
                    }                    
                    SQL += "   AND (a.GbEnd IS NULL OR GbEnd <> '1')                                                                                        \r\n";
                    SQL += "   AND (a.DEPTCODE NOT IN ('TO') AND (a.DEPTCODE NOT IN ('DT') OR a.IPDOPD <> 'O')                                              \r\n";
                    SQL += "   AND (a.DEPTCODE NOT IN ('MR') OR a.IPDOPD <> 'O') AND (a.DRCODE NOT IN ('1107','1125') OR a.IPDOPD <> 'O')                   \r\n";
                    //2019-01-22 안정수, 윤만식t 요청으로 MR 환자 일 경우, 촬영실 조건 2, 3 추가 
                    SQL += "         OR ((a.DEPTCODE IN ('DT','RM','PD','MR') OR a.DRCODE IN ('1107','1125')) AND a.XrayRoom IN ('1', '2', '3', '4', '7') AND a.IPDOPD ='O'))       \r\n";
                }
                else if (argCls.Gubun == "2")   //접수환자
                {
                    SQL += "   AND (a.Pacs_End IN ('Y','P') OR a.GbEnd='1')                                                                                 \r\n";
                }
                else if (argCls.Gubun == "3")   //호명환자
                {
                    SQL += "   AND a.Pacs_End = 'S' AND (GbEnd IS NULL OR GbEnd <> '1')                                                                     \r\n";
                }
                else if (argCls.Gubun == "4")   //종검환자
                {
                    SQL += "   AND a.Pacs_End IS NULL AND (a.GbEnd IS NULL OR GbEnd <> '1')                                                                 \r\n";
                    SQL += "   AND (a.Pano IN (                                                                                                             \r\n";
                    SQL += "                    SELECT Ptno                                                                                                 \r\n";
                    SQL += "                     FROM " + ComNum.DB_PMPA + "HEA_JEPSU                                                                       \r\n";
                    SQL += "                       WHERE 1=1                                                                                                \r\n";
                    SQL += "                        AND SDate=TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD')                                               \r\n";
                    SQL += "                        AND DELDATE IS NULL                                                                                     \r\n";
                    SQL += "                    )                                                                                                           \r\n";
                    SQL += "        OR a.XRayRoom='T'                                                                                                       \r\n";
                    SQL += "       )                                                                                                                        \r\n";
                    SQL += "   AND a.DEPTCODE IN ('TO','HR')                                                                                                \r\n";
                }
                else if (argCls.Gubun == "5")   //루가대기
                {
                    SQL += "   AND a.Pacs_End IS NULL AND (a.GbEnd IS NULL OR GbEnd <> '1')                                                                 \r\n";
                    SQL += "   AND ((A.DEPTCODE IN ('DT','MR','MP') OR A.DRCODE IN ('1107','1125'))                                                              \r\n";
                    SQL += "         AND A.IPDOPD = 'O'                                                                                                     \r\n";
                    //SQL += "         AND (a.XrayRoom <> '1' OR a.XrayRoom IS NULL ))                                                                        \r\n";                                        
                    //2018-12-20 안정수, 윤만식t 요청으로 내과전용촬영 보이도록 보완 
                    //SQL += "         AND (a.XrayRoom <> '1' OR a.XrayRoom = '7' ))                                                                           \r\n";
                    SQL += "         AND a.XrayRoom = '7')                                                                                                  \r\n";
                } 
                else if (argCls.Gubun == "6")   //접수+미영상
                {
                    SQL += "   AND a.Pacs_End IN ('Y','P')                                                                                                  \r\n";
                    SQL += "   AND (a.PacsStudyID IS NULL OR a.PacsStudyID ='' )                                                                            \r\n";
                }
                #endregion
            }
            else if (argCls.Job == "02")
            {
                #region //구분에 따른 조건
                if (argCls.Gubun == "1")
                {
                    SQL += "   AND a.Pacs_End IS NULL AND (a.GbEnd IS NULL OR GbEnd <> '1')                                                             \r\n";
                }
                else if (argCls.Gubun == "2")
                {
                    SQL += "   AND (a.Pacs_End IN ('Y','P') OR a.GbEnd='1')                                                                             \r\n";
                }
                #endregion
            }

            if (argCls.GbPort == "Y")
            {
                SQL += "   AND a.GbPortable ='M'                                                                                                        \r\n";
            }
            if (argCls.GbER == "Y")
            {
                SQL += "   AND a.DeptCode ='ER'                                                                                                         \r\n";
            }

            SQL += "   AND a.SeekDate >= TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD')                                                                \r\n";
            SQL += "   AND a.SeekDate <= TO_DATE('" + argCls.SeekDate + " 23:59','YYYY-MM-DD HH24:MI')                                                  \r\n";
            SQL += "   AND a.GbReserved >= '6'                                                                                                          \r\n";
            SQL += "   AND a.PACSNO IS NOT NULL                                                                                                         \r\n";
            if (argCls.XJong !="*")
            {
                SQL += "   AND a.XJong = '" + argCls.XJong + "'                                                                                         \r\n";
            }
                        
            if (argCls.Job == "00")
            {
                SQL += "   GROUP BY a.Pano,c.SName,c.Obst,c.Sex                                                                                         \r\n";
                SQL += "           ,a.XJong,a.GbPortable                                                                                                \r\n";
                SQL += "           ,a.IPDOPD,c.JICODE,a.GbER,a.ASA                                                                                      \r\n";
                SQL += "           ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('XRAY_방사선종류',TRIM(a.XJong))                                                   \r\n";
                SQL += "           ,KOSMOS_OCS.FC_GET_AGE(TO_CHAR(c.Birth,'YYYY-MM-DD'),a.BDate)                                                        \r\n";
                SQL += "           ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,a.BDate)                                                                \r\n";
                SQL += "           ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Pano,a.BDate)                                                             \r\n";
                SQL += "           ,KOSMOS_OCS.FC_BAS_AREA_WON(a.Pano,a.IPDOPD,a.BI,a.DrCode)                                                           \r\n"; //원거리   
                SQL += "           ,KOSMOS_OCS.FC_PREGNANT_CHK(a.IPDOPD,a.Pano,a.BDate)                                                                 \r\n"; //임신  
                SQL += "           ,KOSMOS_OCS.FC_GET_AGE2(a.Pano,a.BDate)                                                                              \r\n"; //나이           
                SQL += "           ,KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(a.Pano,TRUNC(SYSDATE))                                                            \r\n"; //낙상
                SQL += "           ,KOSMOS_OCS.FC_IPD_NEW_MASTER_SECRET(a.Pano)                                                                         \r\n"; //입원사생활체
                SQL += "           ,KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(a.Pano,a.BDate,a.DeptCode)                                                       \r\n"; //응급중증
                SQL += "           ,KOSMOS_OCS.FC_XRAY_JUSA_CHK(a.Pano,TRUNC(A.ENTERDATE),'NULL')                                                      \r\n"; //주사체크

                SQL += "   ORDER BY MIN(a.PacsNo),a.Pano,a.XJong                                                                                        \r\n";
            }
            else if (argCls.Job == "01")
            {
                SQL += "   ORDER BY a.PacsNo                                                                                                            \r\n";
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

        public DataTable sel_XRAY_DETAIL_WorkList_sName(PsmhDb pDbCon, cXrayDetail argCls)
        {
            DataTable dt = null;

            SQL = "";                        
            SQL += " SELECT SNAME,COUNT(SName) CNT                                                                                                      \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT                                                                                           \r\n";
            SQL += "    WHERE 1=1                                                                                                                       \r\n";
            SQL += "      AND PANO IN (                                                                                                                 \r\n";
            SQL += "                   SELECT                                                                                                           \r\n";
            SQL += "                     a.Pano                                                                                                         \r\n";
            SQL += "                FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                                                            \r\n";
            SQL += "                     ," + ComNum.DB_MED + "OCS_ORDERCODE b                                                                          \r\n";
            SQL += "                     ," + ComNum.DB_PMPA + "BAS_PATIENT c                                                                           \r\n";
            SQL += "                     ," + ComNum.DB_PMPA + "BAS_DOCTOR d                                                                            \r\n";
            SQL += "                 WHERE 1 = 1                                                                                                        \r\n";
            SQL += "                  AND a.OrderCode = b.OrderCode(+)                                                                                  \r\n";
            SQL += "                  AND a.DrCode    = d.Drcode(+)                                                                                     \r\n";
            SQL += "                  AND a.PANO = c.PANO(+)                                                                                            \r\n";

            if (argCls.Pano != "")
            {
                SQL += "    AND a.Pano ='" + argCls.Pano + "'                                                                                           \r\n";
            }

            if (argCls.Search != "")
            {
                SQL += "   AND (a.Pano = '" + argCls.Search + "'                                                                                        \r\n";
                SQL += "         OR c.SName LIKE '%" + argCls.Search + "%'  )                                                                           \r\n";
            }

            if (argCls.Job == "00" || argCls.Job == "01")
            {
                #region //구분에 따른 조건
                if (argCls.Gubun == "1")         //대기환자
                {
                    SQL += "   AND a.Pacs_End IS NULL                                                                                                   \r\n";
                    if (argCls.Job == "00")
                    {
                        SQL += "   AND a.XRayRoom NOT IN ('T','H')                                                                                      \r\n";
                    }
                    SQL += "   AND (a.GbEnd IS NULL OR GbEnd <> '1')                                                                                    \r\n";
                    SQL += "   AND (a.DEPTCODE NOT IN ('TO') AND (a.DEPTCODE NOT IN ('DT') OR a.IPDOPD <> 'O')                                          \r\n";
                    SQL += "   AND (a.DEPTCODE NOT IN ('MR') OR a.IPDOPD <> 'O') AND (a.DRCODE NOT IN ('1107','1125') OR a.IPDOPD <> 'O')               \r\n";
                    SQL += "         OR ((a.DEPTCODE IN ('DT','RM','PD','MR') OR a.DRCODE IN ('1107','1125')) AND a.XrayRoom ='1' AND a.IPDOPD ='O'))   \r\n";
                }
                else if (argCls.Gubun == "2")   //접수환자
                {
                    SQL += "   AND (a.Pacs_End IN ('Y','P') OR a.GbEnd='1')                                                                             \r\n";
                }
                else if (argCls.Gubun == "3")   //호명환자
                {
                    SQL += "   AND a.Pacs_End = 'S' AND (GbEnd IS NULL OR GbEnd <> '1')                                                                 \r\n";
                }
                else if (argCls.Gubun == "4")   //종검환자
                {
                    SQL += "   AND a.Pacs_End IS NULL AND (a.GbEnd IS NULL OR GbEnd <> '1')                                                             \r\n";
                    SQL += "   AND (a.Pano IN (                                                                                                         \r\n";
                    SQL += "                    SELECT Ptno                                                                                             \r\n";
                    SQL += "                     FROM " + ComNum.DB_PMPA + "HEA_JEPSU                                                                   \r\n";
                    SQL += "                       WHERE 1=1                                                                                            \r\n";
                    SQL += "                        AND SDate=TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD')                                           \r\n";
                    SQL += "                        AND DELDATE IS NULL                                                                                 \r\n";
                    SQL += "                    )                                                                                                       \r\n";
                    SQL += "        OR a.XRayRoom='T'                                                                                                   \r\n";
                    SQL += "       )                                                                                                                    \r\n";
                    SQL += "   AND a.DEPTCODE IN ('TO','HR')                                                                                            \r\n";
                }
                else if (argCls.Gubun == "5")   //루가대기
                {
                    SQL += "   AND a.Pacs_End IS NULL AND (a.GbEnd IS NULL OR GbEnd <> '1')                                                             \r\n";
                    SQL += "   AND ((A.DEPTCODE IN ('DT','MR') OR A.DRCODE IN ('1107','1125'))                                                          \r\n";
                    SQL += "         AND A.IPDOPD = 'O'                                                                                                 \r\n";
                    SQL += "         AND (a.XrayRoom <> '1' OR a.XrayRoom IS NULL ))                                                                    \r\n";
                }
                else if (argCls.Gubun == "6")   //접수+미영상
                {
                    SQL += "   AND a.Pacs_End IN ('Y','P')                                                                                              \r\n";
                    SQL += "   AND (a.PacsStudyID IS NULL OR a.PacsStudyID ='' )                                                                        \r\n";
                }
                #endregion
            }
            else if (argCls.Job == "02")
            {
                #region //구분에 따른 조건
                if (argCls.Gubun == "1")
                {
                    SQL += "   AND a.Pacs_End IS NULL AND (a.GbEnd IS NULL OR GbEnd <> '1')                                                             \r\n";
                }
                else if (argCls.Gubun == "2")
                {
                    SQL += "   AND (a.Pacs_End IN ('Y','P') OR a.GbEnd='1')                                                                             \r\n";
                }
                #endregion
            }

            if (argCls.GbPort == "Y")
            {
                SQL += "   AND a.GbPortable ='M'                                                                                                        \r\n";
            }

            SQL += "   AND a.SeekDate >= TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD')                                                                \r\n";
            SQL += "   AND a.SeekDate <= TO_DATE('" + argCls.SeekDate + " 23:59','YYYY-MM-DD HH24:MI')                                                  \r\n";
            SQL += "   AND a.GbReserved >= '6'                                                                                                          \r\n";
            SQL += "   AND a.PACSNO IS NOT NULL                                                                                                         \r\n";
            if (argCls.XJong != "*")
            {
                SQL += "   AND a.XJong = '" + argCls.XJong + "'                                                                                         \r\n";
            }


            SQL += "                         )                                                                                                          \r\n";
            SQL += "   GROUP BY SNAME                                                                                                                   \r\n";
            SQL += "    HAVING COUNT(SName) > 1                                                                                                         \r\n";


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

        public DataTable sel_XRAY_DETAIL_CLE(PsmhDb pDbCon, cXrayDetail argCls) 
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                 \r\n";
            if (argCls.Job == "00")
            {
                SQL += "        a.Pano, a.Sname, a.Sex ,a.age,b.Jumin1,b.Jumin2                             \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                          \r\n";
                SQL += "       ," + ComNum.DB_PMPA + "BAS_PATIENT b                                         \r\n";
                SQL += "   WHERE 1 = 1                                                                      \r\n";
                SQL += "    AND a.Pano = b.Pano(+)                                                          \r\n";
            }
            if (argCls.Job == "01")
            {
                SQL += "       a.XCode,a.XJong,a.OrderName,a.Qty,a.IpdOpd                                   \r\n";
                SQL += "      ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SDate                                       \r\n";
                SQL += "      ,TO_CHAR(a.SeekDate,'HH24:MI') STime                                          \r\n";
                SQL += "      ,a.DeptCode, a.ROWID                                                          \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                          \r\n";                
                SQL += "   WHERE 1 = 1                                                                      \r\n";                

            }
            if (argCls.Job == "02")
            {
                SQL += "       a.XCode,a.XJong,a.OrderName,a.Qty,a.IpdOpd                                   \r\n";
                SQL += "      ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SDate                                       \r\n";
                SQL += "      ,TO_CHAR(a.SeekDate,'HH24:MI') STime                                          \r\n";
                SQL += "      ,a.DeptCode, a.ROWID                                                          \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                          \r\n";
                SQL += "   WHERE 1 = 1                                                                      \r\n";

            }
            //2018-10-19 안정수, GS 인경우, GS초음파만 보이도록 추가함 
            if (argCls.Job == "03")
            {
                SQL += "       a.XCode,a.XJong,a.OrderName,a.Qty,a.IpdOpd                                   \r\n";
                SQL += "      ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SDate                                       \r\n";
                SQL += "      ,TO_CHAR(a.SeekDate,'HH24:MI') STime                                          \r\n";
                SQL += "      ,a.DeptCode, a.ROWID                                                          \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                          \r\n";
                SQL += "   WHERE 1 = 1                                                                      \r\n";

            }
            else
            {

            }                
            
            if (argCls.Pano != "")
            {
                SQL += "   AND a.Pano ='" + argCls.Pano + "'                                                \r\n";
            }
            if (argCls.Search != "")
            {
                SQL += "   AND (a.Pano = '" + argCls.Search + "'                                            \r\n";
                SQL += "         OR a.SName LIKE '%" + argCls.Search + "%'  )                               \r\n";
            }

            //2018-11-07 안정수, 접수취소 전 체크시 SeekDate가 들어가면 영상존재 또는 촬영여부 조건이 무조건 pass되므로 
            //                    Job이 02일 경우는 촬영일자 조건 제외
            if (argCls.Job != "02")
            {
                SQL += "   AND a.SeekDate >= TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD')                    \r\n";
                SQL += "   AND a.SeekDate <= TO_DATE('" + argCls.SeekDate + " 23:59','YYYY-MM-DD HH24:MI')      \r\n"; 
            }

            if (argCls.Job =="00")
            {
                if (argCls.XJong !="")
                {
                    SQL += "   AND a.XJong ='" + argCls.XJong + "'                                          \r\n";
                }
                else
                {
                    SQL += "   AND (a.XJong <= 'A'  OR a.XJong ='Q' OR a.XJONG ='F')                        \r\n";
                }
                SQL += "   AND (a.Gbend <> '1' OR a.Gbend IS NULL)                                          \r\n";
                SQL += "   AND a.GbReserved IN ('6','7')                                                    \r\n";
                SQL += "   AND (a.GbHIC IS NULL OR GbHIC <> 'Y')                                            \r\n";
                SQL += "   AND (a.PacsStudyID IS NULL OR (a.ExInfo IS NULL OR a.ExInfo < 1000))             \r\n";
                SQL += "   GROUP BY a.Pano, a.SName, a.Sex, a.Age,b.Jumin1,b.Jumin2                         \r\n";
                SQL += "   ORDER BY a.SName,a.Pano                                                          \r\n";
            }
            else if (argCls.Job == "01")
            {
                SQL += "   AND (a.XJong <= 'A'  OR a.XJong ='Q' OR a.XJONG ='F')                            \r\n";
                SQL += "   AND (a.Gbend <> '1' OR a.Gbend IS NULL)                                          \r\n";
                SQL += "   AND a.GbReserved IN ('6','7')                                                    \r\n";
                SQL += "   AND (a.GbHIC IS NULL OR GbHIC <> 'Y')                                            \r\n";
                SQL += "   AND (a.PacsStudyID IS NULL OR (a.ExInfo IS NULL OR a.ExInfo < 1000))             \r\n";

            }
            else if (argCls.Job == "02")
            {
                SQL += "   AND a.ROWID = '" + argCls.ROWID + "'                                             \r\n";
                SQL += "   AND (a.Gbend <> '1' OR a.Gbend IS NULL)                                          \r\n";                
                SQL += "   AND (a.PacsStudyID IS NOT NULL                                                   \r\n";
                SQL += "        OR  ExInfo>=1000                                                            \r\n";
                SQL += "        OR (XJong='1' AND Pacs_End IS NOT NULL                                      \r\n";
                SQL += "          AND DeptCode NOT IN ('TO','HR'))                                          \r\n";
                SQL += "       )                                                                            \r\n";

            }
            //2018-10-19 안정수, GS 인경우, GS초음파만 보이도록 추가함
            else if (argCls.Job == "03")
            {
                SQL += "   AND a.XJong = 'B'                                                                \r\n";
                SQL += "   AND (a.Gbend <> '1' OR a.Gbend IS NULL)                                          \r\n";
                SQL += "   AND a.GbReserved IN ('6','7')                                                    \r\n";
                SQL += "   AND (a.GbHIC IS NULL OR GbHIC <> 'Y')                                            \r\n";
                SQL += "   AND (a.PacsStudyID IS NULL OR (a.ExInfo IS NULL OR a.ExInfo < 1000))             \r\n";

            }
            else
            {
                SQL += "   AND (a.XJong <= 'A'  OR a.XJong ='Q' OR a.XJONG ='F')                            \r\n";
                SQL += "   AND (a.Gbend <> '1' OR a.Gbend IS NULL)                                          \r\n";
                SQL += "   AND a.GbReserved IN ('6','7')                                                    \r\n";
                SQL += "   AND (a.GbHIC IS NULL OR GbHIC <> 'Y')                                            \r\n";
                SQL += "   AND (a.PacsStudyID IS NULL OR (a.ExInfo IS NULL OR a.ExInfo < 1000))             \r\n";

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

        public DataTable sel_XRAY_DETAIL_back(PsmhDb pDbCon, cXrayDetail argCls)
        {
            DataTable dt = null;

            SQL = "";

            #region // 1.Xray_Detail SeekDate 기준

            SQL += " SELECT                                                                                                                             \r\n";
            SQL += "        a.Pano,c.SName,c.Obst                                                                                                       \r\n";
            SQL += "       ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                                                                         \r\n";
            SQL += "       ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SeekDate                                                                                   \r\n";
            if (argCls.STS == "00")
            {
                SQL += "       ,a.IPDOPD,c.Sex,a.DeptCode,a.WardCode,a.RoomCode,c.Jumin1 || '-' || c.Jumin2 AS Jumin                                    \r\n";

                SQL += "       ,( SELECT COUNT(s.XCode)                                                                                                 \r\n";
                SQL += "           FROM " + ComNum.DB_PMPA + "XRAY_DETAIL s                                                                             \r\n";
                SQL += "            WHERE 1 = 1                                                                                                         \r\n";
                SQL += "              AND s.Pano=a.Pano                                                                                                 \r\n";
                SQL += "              AND ( s.SeekDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                  \r\n";
                SQL += "                    AND s.SeekDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                                \r\n";
                SQL += "                  OR  s.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                  \r\n";
                SQL += "                    AND s.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                                   \r\n";
                SQL += "                   )                                                                                                            \r\n";
                SQL += "              AND ( s.XJong <= '9'  OR s.XJong ='Q' )                                                                           \r\n";
                if (argCls.Job == "OCS접수")
                {
                    SQL += "          AND s.ORDERNO > 0                                                                                                 \r\n";
                }
                else if (argCls.Job == "자동접수")
                {
                    SQL += "          AND ( s.OrderNo IS NULL OR s.OrderNo = 0 )                                                                        \r\n";
                    SQL += "          AND ( s.Gbend <> '1' OR s.Gbend IS NULL )                                                                         \r\n";
                }
                if (argCls.Job == "OCS접수" || argCls.Job == "자동접수")
                {
                    SQL += "          AND ( s.GbReserved = '1' OR s.GbReserved = '2' )                                                                  \r\n";
                    SQL += "          AND ( s.GbHIC IS NULL OR s.GbHIC <> 'Y' )                                                                         \r\n";
                }
                SQL += "         ) AS CNT1                                                                                                              \r\n";

            }
            else if (argCls.STS == "01")
            {
                SQL += "       ,a.XJong,a.IPDOPD,c.Sex,a.DeptCode,a.DrCode,d.DrName,a.WardCode,a.RoomCode,a.ASA                                         \r\n";
                SQL += "       ,TO_CHAR(a.RDate,'YYYY-MM-DD') RDate                                                                                     \r\n";
                SQL += "       ,TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI') OrderDate                                                                     \r\n";
                SQL += "       ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('XRAY_방사선종류',TRIM(a.XJong)) FC_XJong2                                              \r\n"; //종류체크

            }
            else if (argCls.STS == "02")
            {
                SQL += "       ,a.XJong,a.IPDOPD,c.Sex,a.DeptCode,a.DrCode,d.DrName,a.WardCode,a.RoomCode,a.GbPortable                                  \r\n";
                SQL += "       ,a.XCode,a.XSubCode,a.Qty,a.Remark,a.DrRemark,a.OrderCode,a.OrderName,a.ASA,a.cRemark,a.ROWID                            \r\n";
                SQL += "       ,TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI') OrderDate                                                                     \r\n";
                SQL += "       ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('XRAY_방사선종류',TRIM(a.XJong)) FC_XJong2                                              \r\n"; //종류체크
            }

            ////function
            ////SQL += "   ,KOSMOS_OCS.FC_BAS_AUTO_MST_CHK(a.Ptno,a.BDate) autoSTS                                                                      \r\n"; //후불체크
            //SQL += "   ,KOSMOS_OCS.FC_NUR_HAPPYCALL_OPD(a.Pano,'05','ENDO_JUPMST',a.ROWID) FC_happycall                                             \r\n"; //해피콜체크
            ////SQL += "   ,KOSMOS_OCS.FC_BAS_BUSE_NAME(a.Buse) FC_BuseName                                                                             \r\n"; //부서이름
            //SQL += "   ,KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(a.Pano,TRUNC(SYSDATE)) FC_Fall                                                            \r\n"; //낙상
            //SQL += "   ,KOSMOS_OCS.FC_OPD_RESERVED_NEW_NEAR(a.Pano,a.DeptCode) FC_opdRes                                                            \r\n"; //예약접수정보
            ////SQL += "   ,KOSMOS_OCS.FC_OCS_ITRANSFER_CHK(a.Ptno,'MG') FC_Consult                                                                     \r\n"; //협진체크
            //SQL += "   ,KOSMOS_OCS.FC_OPD_SLIP_SUNAP_CHK(a.Pano,a.BDate,b.SuCode,a.OrderNo) FC_SUNAP                                                \r\n"; //수납체크
            SQL += "   ,KOSMOS_OCS.FC_GET_AGE(TO_CHAR(c.Birth,'YYYY-MM-DD'),a.BDate) FC_age                                                             \r\n"; //나이체크
            SQL += "   ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,a.BDate) FC_infect                                                                  \r\n"; //감염체크
            //SQL += "   ,KOSMOS_OCS.FC_IPD_NEW_MASTER_JSTS2(KOSMOS_OCS.FC_IPD_NEW_MASTER_JSTS(a.Pano)) FC_Ipd_Info                                   \r\n"; //재원체크
            //SQL += "   ,KOSMOS_OCS.FC_OCS_ITRANSFER_CHK2(a.Pano,'MG') FC_Consult                                                                    \r\n"; //협진체크
            ////SQL += "   ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_ENDO_도착구분',a.sGubun2) FC_sGubun2                                                    \r\n"; //도착구분                        
            SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                                                                          \r\n";
            SQL += "       ," + ComNum.DB_MED + "OCS_ORDERCODE b                                                                                        \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_PATIENT c                                                                                         \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_DOCTOR d                                                                                          \r\n";
            SQL += "   WHERE 1 = 1                                                                                                                      \r\n";
            SQL += "    AND a.OrderCode = b.OrderCode(+)                                                                                                \r\n";
            SQL += "    AND a.DrCode    = d.Drcode(+)                                                                                                   \r\n";
            SQL += "    AND a.PANO = c.PANO(+)                                                                                                          \r\n";

            #region // 1.SeekDate 기준 AND 부분

            if (argCls.Pano != "")
            {
                SQL += "   AND a.Pano ='" + argCls.Pano + "'                                                                                            \r\n";
            }

            if (argCls.STS == "00" || argCls.STS == "01" || argCls.STS == "02")
            {
                if (argCls.Tab == "1")
                {
                    SQL += "  AND ( (a.SeekDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                         \r\n";
                    SQL += "        AND a.SeekDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI') )                                      \r\n";
                    SQL += "      OR ( a.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                          \r\n";
                    SQL += "        AND a.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI') )                                         \r\n";
                    SQL += "       )                                                                                                                    \r\n";
                }
                else if (argCls.Tab == "2")
                {
                    SQL += "   AND a.SeekDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                           \r\n";
                    SQL += "   AND a.SeekDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                                             \r\n";
                }

            }
            else
            {

            }

            if (argCls.DeptCode != "**")
            {
                SQL += "   AND a.DeptCode ='" + argCls.DeptCode + "'                                                                                    \r\n";
            }

            if (argCls.Search != "")
            {
                SQL += "   AND (a.Pano = '" + argCls.Search + "'                                                                                        \r\n";
                SQL += "         OR c.SName LIKE '%" + argCls.Search + "%'  )                                                                           \r\n";
            }

            if (argCls.Tab == "1")
            {
                if (argCls.XJong == "*")
                {
                    SQL += "   AND ( a.XJong <= '9'  OR a.XJong ='Q' )                                                                                  \r\n";
                }
                else
                {
                    SQL += "   AND a.XJong ='" + argCls.XJong + "'                                                                                      \r\n";
                }
            }
            else if (argCls.Tab == "2")
            {
                if (argCls.XJong == "*")
                {
                    SQL += "   AND ( a.XJong <= '9'  OR a.XJong ='Q' )                                                                                  \r\n";
                    //SQL += "   AND ( a.XJong <= 'A'  OR a.XJong ='Q' OR a.XJONG ='F' )                                                                  \r\n";
                }
                else
                {
                    SQL += "   AND a.XJong ='" + argCls.XJong + "'                                                                                      \r\n";
                }
            }

            if (argCls.IPDOPD != "*")
            {
                SQL += "   AND a.IPDOPD ='" + argCls.IPDOPD + "'                                                                                        \r\n";
            }

            if (argCls.GbPort == "Y")
            {
                SQL += "   AND a.GbPortable ='M'                                                                                                        \r\n";
            }

            if (argCls.GbER == "Y")
            {
                SQL += "   AND (a.DeptCode ='ER' OR ( a.WardCode ='ER' AND a.RoomCode =100) )                                                           \r\n";
            }

            if (argCls.Tab == "1")
            {
                if (argCls.GbHR == "Y")
                {
                    SQL += "   AND a.DeptCode IN ('HR','TO')                                                                                            \r\n";
                }
                else
                {
                    if (argCls.Job == "OCS접수")
                    {
                        SQL += "   AND a.ORDERNO > 0                                                                                                    \r\n";
                    }
                    else if (argCls.Job == "자동접수")
                    {
                        SQL += "   AND ( a.OrderNo IS NULL OR a.OrderNo = 0 )                                                                           \r\n";
                        SQL += "   AND ( a.Gbend <> '1' OR a.Gbend IS NULL )                                                                            \r\n";
                    }
                }

                if (argCls.Job == "OCS접수" || argCls.Job == "자동접수")
                {
                    SQL += "   AND ( a.GbReserved = '1' OR a.GbReserved = '2' )                                                                         \r\n";
                    SQL += "   AND ( a.GbHIC IS NULL OR GbHIC <> 'Y' )                                                                                  \r\n";
                }
            }
            else if (argCls.Tab == "2")
            {
                SQL += "   AND a.GbReserved IN ('6','7')                                                                                                \r\n";
                SQL += "   AND ( a.Gbend <> '1' OR a.Gbend IS NULL )                                                                                    \r\n";
                SQL += "   AND ( a.GbHIC IS NULL OR GbHIC <> 'Y' )                                                                                      \r\n";
                SQL += "   AND ( a.PacsStudyID IS NULL OR (a.ExInfo IS NULL OR a.ExInfo < 1000) )                                                       \r\n";
            }

            SQL += "   AND TRIM(a.XCode) NOT IN (                                                                                                        \r\n";
            SQL += "                            SELECT TRIM(CODE)                                                                                       \r\n";
            SQL += "                              FROM " + ComNum.DB_PMPA + "BAS_BCODE                                                                  \r\n";
            SQL += "                               WHERE 1=1                                                                                            \r\n";
            SQL += "                                AND GUBUN ='XRAY_OCS명단제외코드'                                                                    \r\n";
            SQL += "                                AND ( DELDATE IS NULL OR DELDATE ='' )                                                              \r\n";
            SQL += "                          )                                                                                                         \r\n";

            #endregion

            if (argCls.STS == "00" || argCls.STS == "01")
            {
                SQL += "   GROUP BY a.Pano,c.SName,c.Obst                                                                                               \r\n";
                SQL += "            ,TO_CHAR(a.BDate,'YYYY-MM-DD')                                                                                      \r\n";
                SQL += "            ,TO_CHAR(a.SeekDate,'YYYY-MM-DD')                                                                                   \r\n";
                if (argCls.STS == "00")
                {
                    SQL += "            ,a.IPDOPD,c.Sex,a.DeptCode,a.WardCode,a.RoomCode,c.Jumin1 || '-' || c.Jumin2                                    \r\n";
                }
                else if (argCls.STS == "01")
                {
                    SQL += "            ,a.XJong,a.IPDOPD,c.Sex,a.DeptCode,a.DrCode,d.DrName,a.WardCode,a.RoomCode,a.ASA                                \r\n";
                    SQL += "            ,TO_CHAR(a.RDate,'YYYY-MM-DD')                                                                                  \r\n";
                    SQL += "            ,TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI')                                                                      \r\n";
                    SQL += "            ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('XRAY_방사선종류',TRIM(a.XJong))                                               \r\n";
                }
                SQL += "            ,KOSMOS_OCS.FC_GET_AGE(TO_CHAR(c.Birth,'YYYY-MM-DD'),a.BDate)                                                       \r\n";
                SQL += "            ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,a.BDate)                                                               \r\n";
            }

            #endregion

            //SQL += "   UNION                                                                                                                            \r\n";

            #region // 2.Xray_Detail RDate 기준 사용안함

            //SQL += " SELECT                                                                                                                             \r\n";
            //SQL += "        a.Pano,c.SName,c.Obst                                                                                                       \r\n";
            //SQL += "       ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                                                                         \r\n";
            //SQL += "       ,TO_CHAR(a.RDate,'YYYY-MM-DD') SeekDate                                                                                      \r\n";
            //if (argCls.STS == "00")
            //{
            //    SQL += "       ,a.IPDOPD,c.Sex,a.DeptCode,a.WardCode,a.RoomCode,c.Jumin1 || '-' || c.Jumin2 AS Jumin                                    \r\n";

            //    //SQL += "       ,( SELECT COUNT(s.XCode)                                                                                                 \r\n";
            //    //SQL += "           FROM " + ComNum.DB_PMPA + "XRAY_DETAIL s                                                                             \r\n";
            //    //SQL += "            WHERE 1 = 2                                                                                                         \r\n";
            //    //SQL += "              AND s.Pano=a.Pano                                                                                                 \r\n";
            //    //SQL += "              AND s.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                       \r\n";
            //    //SQL += "              AND s.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                                         \r\n";
            //    //SQL += "              AND ( s.XJong <= 'A'  OR s.XJong ='Q' OR s.XJONG ='F' )                                                           \r\n";
            //    //if (argCls.Job == "OCS접수")
            //    //{
            //    //    SQL += "          AND s.ORDERNO > 0                                                                                                 \r\n";
            //    //}
            //    //else if (argCls.Job == "자동접수")
            //    //{
            //    //    SQL += "          AND ( s.OrderNo IS NULL OR s.OrderNo = 0 )                                                                        \r\n";
            //    //    SQL += "          AND ( s.Gbend <> '1' OR s.Gbend IS NULL )                                                                         \r\n";
            //    //}
            //    //if (argCls.Job == "OCS접수" || argCls.Job == "자동접수")
            //    //{
            //    //    SQL += "          AND ( s.GbReserved = '1' OR s.GbReserved = '2' )                                                                  \r\n";
            //    //    SQL += "          AND ( s.GbHIC IS NULL OR s.GbHIC <> 'Y' )                                                                         \r\n";
            //    //}
            //    //SQL += "         ) AS CNT1                                                                                                              \r\n";

            //}
            //else if (argCls.STS == "01")
            //{
            //    SQL += "       ,a.XJong,a.IPDOPD,c.Sex,a.DeptCode,a.DrCode,d.DrName,a.WardCode,a.RoomCode,a.ASA                                         \r\n";
            //    SQL += "       ,TO_CHAR(a.RDate,'YYYY-MM-DD') RDate                                                                                     \r\n";
            //    SQL += "       ,TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI') OrderDate                                                                     \r\n";
            //    SQL += "       ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('XRAY_방사선종류',TRIM(a.XJong)) FC_XJong2                                              \r\n"; //종류체크

            //}
            //else if (argCls.STS == "02")
            //{
            //    SQL += "       ,a.XJong,a.IPDOPD,c.Sex,a.DeptCode,a.DrCode,d.DrName,a.WardCode,a.RoomCode,a.GbPortable                                  \r\n";
            //    SQL += "       ,a.XCode,a.XSubCode,a.Qty,a.Remark,a.DrRemark,a.OrderCode,a.OrderName,a.ASA,a.cRemark,a.ROWID                            \r\n";
            //    SQL += "       ,TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI') OrderDate                                                                     \r\n";
            //    SQL += "       ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('XRAY_방사선종류',TRIM(a.XJong)) FC_XJong2                                              \r\n"; //종류체크
            //}

            //////function
            //////SQL += "   ,KOSMOS_OCS.FC_BAS_AUTO_MST_CHK(a.Ptno,a.BDate) autoSTS                                                                      \r\n"; //후불체크
            ////SQL += "   ,KOSMOS_OCS.FC_NUR_HAPPYCALL_OPD(a.Pano,'05','ENDO_JUPMST',a.ROWID) FC_happycall                                             \r\n"; //해피콜체크
            //////SQL += "   ,KOSMOS_OCS.FC_BAS_BUSE_NAME(a.Buse) FC_BuseName                                                                             \r\n"; //부서이름
            ////SQL += "   ,KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(a.Pano,TRUNC(SYSDATE)) FC_Fall                                                            \r\n"; //낙상
            ////SQL += "   ,KOSMOS_OCS.FC_OPD_RESERVED_NEW_NEAR(a.Pano,a.DeptCode) FC_opdRes                                                            \r\n"; //예약접수정보
            //////SQL += "   ,KOSMOS_OCS.FC_OCS_ITRANSFER_CHK(a.Ptno,'MG') FC_Consult                                                                     \r\n"; //협진체크
            ////SQL += "   ,KOSMOS_OCS.FC_OPD_SLIP_SUNAP_CHK(a.Pano,a.BDate,b.SuCode,a.OrderNo) FC_SUNAP                                                \r\n"; //수납체크
            //SQL += "   ,KOSMOS_OCS.FC_GET_AGE(TO_CHAR(c.Birth,'YYYY-MM-DD'),a.BDate) FC_age                                                             \r\n"; //나이체크
            //SQL += "   ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,a.BDate) FC_infect                                                                  \r\n"; //감염체크
            ////SQL += "   ,KOSMOS_OCS.FC_IPD_NEW_MASTER_JSTS2(KOSMOS_OCS.FC_IPD_NEW_MASTER_JSTS(a.Pano)) FC_Ipd_Info                                   \r\n"; //재원체크
            ////SQL += "   ,KOSMOS_OCS.FC_OCS_ITRANSFER_CHK2(a.Pano,'MG') FC_Consult                                                                    \r\n"; //협진체크
            //////SQL += "   ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_ENDO_도착구분',a.sGubun2) FC_sGubun2                                                    \r\n"; //도착구분                        
            //SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                                                                          \r\n";
            //SQL += "       ," + ComNum.DB_MED + "OCS_ORDERCODE b                                                                                        \r\n";
            //SQL += "       ," + ComNum.DB_PMPA + "BAS_PATIENT c                                                                                         \r\n";
            //SQL += "       ," + ComNum.DB_PMPA + "BAS_DOCTOR d                                                                                          \r\n";
            //SQL += "   WHERE 1 = " + argCls.Tab + "                                                                                                     \r\n";
            //SQL += "    AND a.OrderCode = b.OrderCode(+)                                                                                                \r\n";
            //SQL += "    AND a.DrCode    = d.Drcode(+)                                                                                                   \r\n";
            //SQL += "    AND a.PANO = c.PANO(+)                                                                                                          \r\n";

            #region // 2.RDate 기준 AND 부분

            //if (argCls.Pano != "")
            //{
            //    SQL += "   AND a.Pano ='" + argCls.Pano + "'                                                                                            \r\n";
            //}

            //if (argCls.STS == "00" || argCls.STS == "01" || argCls.STS == "02")
            //{

            //    SQL += "   AND a.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                                  \r\n";
            //    SQL += "   AND a.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                                                    \r\n";

            //}
            //else
            //{

            //}

            //if (argCls.DeptCode != "**")
            //{
            //    SQL += "   AND a.DeptCode ='" + argCls.DeptCode + "'                                                                                    \r\n";
            //}

            //if (argCls.Search != "")
            //{
            //    SQL += "   AND (a.Pano = '" + argCls.Search + "'                                                                                        \r\n";
            //    SQL += "         OR c.SName LIKE '%" + argCls.Search + "%'  )                                                                           \r\n";
            //}

            //if (argCls.Tab == "1")
            //{
            //    if (argCls.XJong != "*")
            //    {
            //        SQL += "   AND a.XJong ='" + argCls.XJong + "'                                                                                      \r\n";
            //    }
            //}
            //else if (argCls.Tab == "2")
            //{
            //    if (argCls.XJong == "*")
            //    {
            //        SQL += "   AND ( a.XJong <= 'A'  OR a.XJong ='Q' OR a.XJONG ='F' )                                                                  \r\n";
            //    }
            //    else
            //    {
            //        SQL += "   AND a.XJong ='" + argCls.XJong + "'                                                                                      \r\n";
            //    }
            //}

            //if (argCls.IPDOPD != "*")
            //{
            //    SQL += "   AND a.IPDOPD ='" + argCls.IPDOPD + "'                                                                                        \r\n";
            //}

            //if (argCls.GbPort == "Y")
            //{
            //    SQL += "   AND a.GbPortable ='M'                                                                                                        \r\n";
            //}

            //if (argCls.GbER == "Y")
            //{
            //    SQL += "   AND (a.DeptCode ='ER' OR ( a.WardCode ='ER' AND a.RoomCode =100) )                                                           \r\n";
            //}

            //if (argCls.Tab == "1")
            //{
            //    if (argCls.GbHR == "Y")
            //    {
            //        SQL += "   AND a.DeptCode IN ('HR','TO')                                                                                            \r\n";
            //    }
            //    else
            //    {
            //        if (argCls.Job == "OCS접수")
            //        {
            //            SQL += "   AND a.ORDERNO > 0                                                                                                    \r\n";
            //        }
            //        else if (argCls.Job == "자동접수")
            //        {
            //            SQL += "   AND ( a.OrderNo IS NULL OR a.OrderNo = 0 )                                                                           \r\n";
            //            SQL += "   AND ( a.Gbend <> '1' OR a.Gbend IS NULL )                                                                            \r\n";
            //        }
            //    }

            //    if (argCls.Job == "OCS접수" || argCls.Job == "자동접수")
            //    {
            //        SQL += "   AND ( a.GbReserved = '1' OR a.GbReserved = '2' )                                                                         \r\n";
            //        SQL += "   AND ( a.GbHIC IS NULL OR GbHIC <> 'Y' )                                                                                  \r\n";
            //    }
            //}
            //else if (argCls.Tab == "2")
            //{
            //    SQL += "   AND a.GbReserved IN ('6','7')                                                                                                \r\n";
            //    SQL += "   AND ( a.Gbend <> '1' OR a.Gbend IS NULL )                                                                                    \r\n";
            //    SQL += "   AND ( a.GbHIC IS NULL OR GbHIC <> 'Y' )                                                                                      \r\n";
            //    SQL += "   AND ( a.PacsStudyID IS NULL OR (a.ExInfo IS NULL OR a.ExInfo < 1000) )                                                       \r\n";
            //}

            //SQL += "   AND TRIM(XCode) NOT IN (                                                                                                         \r\n";
            //SQL += "                            SELECT TRIM(CODE)                                                                                       \r\n";
            //SQL += "                              FROM " + ComNum.DB_PMPA + "BAS_BCODE                                                                  \r\n";
            //SQL += "                               WHERE 1=1                                                                                            \r\n";
            //SQL += "                                AND GUBUN ='XRAY_OCS명단제외코드'                                                                    \r\n";
            //SQL += "                                AND ( DELDATE IS NULL OR DELDATE ='' )                                                              \r\n";
            //SQL += "                          )                                                                                                         \r\n";

            //if (argCls.STS == "00" || argCls.STS == "01")
            //{
            //    SQL += "   GROUP BY a.Pano,c.SName,c.Obst                                                                                               \r\n";
            //    SQL += "            ,TO_CHAR(a.BDate,'YYYY-MM-DD')                                                                                      \r\n";
            //    SQL += "            ,TO_CHAR(a.RDate,'YYYY-MM-DD')                                                                                      \r\n";
            //    if (argCls.STS == "00")
            //    {
            //        SQL += "            ,a.IPDOPD,c.Sex,a.DeptCode,a.WardCode,a.RoomCode,c.Jumin1 || '-' || c.Jumin2                                    \r\n";
            //    }
            //    else if (argCls.STS == "01")
            //    {
            //        SQL += "            ,a.XJong,a.IPDOPD,c.Sex,a.DeptCode,a.DrCode,d.DrName,a.WardCode,a.RoomCode,a.ASA                                \r\n";
            //        SQL += "            ,TO_CHAR(a.RDate,'YYYY-MM-DD')                                                                                  \r\n";
            //        SQL += "            ,TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI')                                                                      \r\n";
            //        SQL += "            ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('XRAY_방사선종류',TRIM(a.XJong))                                               \r\n";
            //    }
            //    SQL += "            ,KOSMOS_OCS.FC_GET_AGE(TO_CHAR(c.Birth,'YYYY-MM-DD'),a.BDate)                                                       \r\n";
            //    SQL += "            ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,a.BDate)                                                               \r\n";
            //}

            #endregion


            #endregion

            if (argCls.STS == "00" || argCls.STS == "01")
            {
                SQL += "   ORDER BY 1,2,4                                                                                                               \r\n";
            }
            else if (argCls.STS == "02")
            {
                SQL += "   ORDER BY 5                                                                                                                   \r\n";
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

        public DataTable sel_XRAY_DETAIL_ErrUpDate(PsmhDb pDbCon, string argPano, string argSName,string argDate1,string argDate2, string argJob) 
        {
            DataTable dt = null; 

            SQL = "";
            SQL += " SELECT                                                                     \r\n";
            SQL += "  Pano, PacsNo, ROWID                                                       \r\n"; 
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                   \r\n";
            SQL += "  WHERE 1 = 1                                                               \r\n";                        
            SQL += "   AND Pano ='" + argPano + "'                                              \r\n";
            SQL += "   AND SName ='" + argSName + "'                                            \r\n";

            if (argJob == "01")
            {
                SQL += "    AND ( PacsNo IS NOT NULL OR XJong ='C' )                                 \r\n";
            }

            SQL += "   AND SeekDate >=TO_DATE('" + argDate1 + "','YYYY-MM-DD')                  \r\n";
            SQL += "   AND SeekDate <=TO_DATE('" + argDate2 + " 23:59','YYYY-MM-DD HH24:MI')    \r\n"; 
            
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

        public DataTable sel_HIC_XRAY_RESULT_ErrUpDate(PsmhDb pDbCon, string argPano, string argSName, string argDate1, string argDate2)
        {
            DataTable dt = null;

            SQL = "";            
            SQL += " SELECT                                                                     \r\n";
            SQL += "  XrayNo,Pano,XCode,ROWID                                                   \r\n";
            SQL += "  ,TO_CHAR(JepDate,'YYYYMMDD') JepDate                                      \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "HIC_XRAY_RESULT                               \r\n";
            SQL += "  WHERE 1 = 1                                                               \r\n";
            SQL += "   AND PTno ='" + argPano + "'                                              \r\n";
            SQL += "   AND SName ='" + argSName + "'                                            \r\n";
            SQL += "   AND JepDate >=TO_DATE('" + argDate1 + "','YYYY-MM-DD')                   \r\n";
            SQL += "   AND JepDate <=TO_DATE('" + argDate2 + "','YYYY-MM-DD')                   \r\n";
            
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

        public DataTable sel_ENDO_JUPMST_ErrUpDate(PsmhDb pDbCon, string argPano, string argSName, string argDate1, string argDate2)
        {
            DataTable dt = null;

            SQL = "";            
            SQL += " SELECT                                                                     \r\n";
            SQL += "  Ptno, GBIO,DeptCode,DrCode,WardCode,RoomCode                             \r\n";
            SQL += "  ,TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') RDate                                \r\n";
            SQL += "  ,TO_CHAR(RDate,'YYYYMMDD') RDate2                                         \r\n";
            SQL += "  ,OrderCode,Sex,ROWID                                                      \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_JUPMST                                    \r\n";
            SQL += "  WHERE 1 = 1                                                               \r\n";
            SQL += "   AND PTno ='" + argPano + "'                                              \r\n";
            SQL += "   AND BDate >=TO_DATE('" + argDate1 + "','YYYY-MM-DD')                     \r\n";
            SQL += "   AND BDate <=TO_DATE('" + argDate2 + "','YYYY-MM-DD')                     \r\n";
            SQL += "   AND PACSUID IS NOT NULL                                                  \r\n";

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
        /// 영상의학과 xray_detail 쿼리.. class 사용
        /// </summary>
        /// <param name="argCls"></param>
        /// <param name="argCols"></param>
        /// <param name="argOrdBy"></param>
        /// <returns></returns>
        public DataTable sel_XrayDetail(PsmhDb pDbCon, cXrayDetail argCls, string argCols = "", string argOrdBy = "")
        {
            DataTable dt = null;


            SQL = "";
            SQL += " SELECT                                                                                         \r\n";
            if (argCols != "")
            {
                SQL += "  " + argCols + "                                                                           \r\n";
            }
            else
            {
                SQL += "  IPDOPD,GBRESERVED,PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE,WARDCODE                             \r\n";
                SQL += " ,ROOMCODE,XJONG,XSUBCODE,XCODE,EXINFO,QTY,EXMORE,EXID,GBEND,MGRNO,GBPORTABLE               \r\n";
                SQL += " ,REMARK,XRAYROOM,GBNGT,DRREMARK,ORDERNO,ORDERCODE,PACSNO,ORDERNAME,PACSSTUDYID             \r\n";
                SQL += " ,PACS_END,GBREAD,READ_SEND,READ_RECEIVE,READ_FLAG,AGREE,ORDERDATE                          \r\n";
                SQL += " ,GBPRINT,EXAM_WRTNO,DRDATE,DRWRTNO,STUDY_REF,IMAGE_BDATE                                   \r\n";
                SQL += " ,GBHIC,HIC_WRTNO,HIC_CODE,BI,CADEX_DEL,EMGWRTNO,GBSPC,RDATE,GBSTS                          \r\n";
                SQL += " ,CSABUN,CREMARK,N_STS,N_REMARK,GB_MANUAL,PICKUPREMARK                                      \r\n";
                SQL += " ,GSABUN,GBINFO,CVR,CVR_DATE,CVR_GUBUN,CVR_DRSABUN,CVR_SEND,GBER,ASA                        \r\n";
                SQL += " ,TO_CHAR(ENTERDATE,'YYYY-MM-DD HH24:MI') ENTERDATE                                         \r\n";
                SQL += " ,TO_CHAR(SEEKDATE,'YYYY-MM-DD HH24:MI') SEEKDATE                                           \r\n";
                SQL += " ,TO_CHAR(SEEKDATE,'YYYY-MM-DD') SEEKDATE_DATE                                              \r\n";
                SQL += " ,TO_CHAR(SEEKDATE,'HH24:MI') SEEKDATE_TIME                                                 \r\n";
                SQL += " ,TO_CHAR(SENDDATE,'YYYY-MM-DD HH24:MI') SENDDATE                                           \r\n";
                SQL += " ,TO_CHAR(XSENDDATE,'YYYY-MM-DD HH24:MI') XSENDDATE                                         \r\n";
                SQL += " ,TO_CHAR(PC_BACKDATE,'YYYY-MM-DD HH24:MI') PC_BACKDATE                                     \r\n";
                SQL += " ,TO_CHAR(DRDATE,'YYYY-MM-DD HH24:MI') DRDATE                                               \r\n";
                SQL += " ,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE                                                         \r\n";
                SQL += " ,TO_CHAR(CDATE,'YYYY-MM-DD') CDATE                                                         \r\n";
                SQL += " ,TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE                                                     \r\n";
                SQL += " ,TO_CHAR(N_ENTDATE,'YYYY-MM-DD') N_ENTDATE                                                 \r\n";
                SQL += " ,TO_CHAR(CON_DATE,'YYYY-MM-DD') CON_DATE                                                   \r\n";
                SQL += " ,TO_CHAR(GDATE,'YYYY-MM-DD') GDATE                                                         \r\n";
                SQL += " ,TO_CHAR(CVR_DATE,'YYYY-MM-DD') CVR_DATE                                                   \r\n";
                SQL += " ,TO_CHAR(CVR_CDATE,'YYYY-MM-DD') CVR_CDATE                                                 \r\n";
            }

            SQL += "  ,ROWID                                                                                        \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                                   \r\n";
            if (argCls.Job == "01")
            {
                SQL += "   AND Pano ='" + argCls.Pano + "'                                                          \r\n";
                SQL += "   AND SeekDate >=TO_DATE('" + VB.Left(argCls.RDate, 10) + "','YYYY-MM-DD')                 \r\n";
                SQL += "   AND SeekDate <=TO_DATE('" + VB.Left(argCls.RDate, 10) + " 23:59','YYYY-MM-DD HH24:MI')   \r\n";
                SQL += "   AND DrRemark ='" + argCls.DrRemark + "'                                                  \r\n";
                SQL += "   AND XCode ='" + argCls.XCode + "'                                                       \r\n";
            }
            else if (argCls.Job == "02")
            {
                SQL += "   AND BDate >=TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                 \r\n";
                SQL += "   AND BDate <=TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                                 \r\n";
                SQL += "   AND XCode ='" + argCls.XCode + "'                                                        \r\n";
            }
            else if (argCls.Job == "03")
            {
                SQL += "   AND (GbReserved = '6' OR GbReserved = '7')                                               \r\n";
                SQL += "   AND (GbEnd IS NULL OR GbEnd <> '1')                                                      \r\n";                
                SQL += "   AND (GbHIC IS NULL OR GbHIC <> 'Y')                                                      \r\n";
                SQL += "   AND PacsStudyID IS NULL                                                                  \r\n";
                //김재훈계장 요청 20210813
                SQL += "   AND XJong NOT IN ('E')                                                     \r\n";
                if (argCls.GbResvChk =="ALL")
                {
                }
                else
                {
                    //2019-01-09 안정수, 김재훈계장 요청으로 주석처리함 
                    //SQL += "   AND SeekDate >=TO_DATE('" + VB.Left(argCls.RDate, 10) + "','YYYY-MM-DD')             \r\n";
                }

                if (argCls.Pano != "")
                {
                    SQL += " AND  Pano = '" + argCls.Pano + "'                                                      \r\n";
                }
                if (argCls.SName != "")
                {
                    SQL += " AND  SName = '" + argCls.SName + "'                                                    \r\n";
                }
                if (argCls.XJong !="*")
                {
                    SQL += " AND XJong = '" + argCls.XJong + "'                                                     \r\n";
                }
            }
            else if (argCls.Job == "04")
            {
                SQL += "   AND SeekDate >=TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                              \r\n";
                SQL += "   AND SeekDate <TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                 \r\n";
                if (argCls.XJong != "*")
                {
                    SQL += " AND XJong = '" + argCls.XJong + "'                                                     \r\n";
                }
                if (argCls.GbCVR_Gubun1 =="비대상")
                {
                    SQL += " AND  (CVR IS NULL OR CVR NOT IN  ('Y','S') )                                           \r\n";
                }
                else if (argCls.GbCVR_Gubun1 == "대상")
                {
                    SQL += " AND  CVR IN ('Y','S')                                                                  \r\n";
                    if (argCls.GbCVR_Gubun2 == "촬영자")
                    {
                        SQL += " AND  CVR_Gubun ='1'                                                                \r\n";
                    }
                    else if (argCls.GbCVR_Gubun2 == "판독의")
                    {
                        SQL += " AND  CVR_Gubun ='2'                                                                \r\n";
                    }
                }                
                SQL += " AND DeptCode NOT IN ('HR','TO')                                                            \r\n";

            }
            else if (argCls.Job == "05")
            {
                SQL += " AND SeekDate >= TRUNC(SYSDATE - 2)                                                         \r\n";
                SQL += " AND SeekDate < TRUNC(SYSDATE -0.02)                                                        \r\n"; //'접수 30분 전                
                SQL += " AND GBRESERVED = '7'                                                                       \r\n";
                SQL += " AND PACSSTUDYID IS NULL                                                                    \r\n";
                SQL += " AND PACSNO IS NOT NULL                                                                     \r\n";                
                SQL += " AND DeptCode NOT IN ('HR','TO')                                                            \r\n";
                SQL += " AND XJong IN ('1','3','4','5','6','8')                                                     \r\n";

            }
            else if (argCls.Job == "06")
            {
                SQL += " AND SeekDate >=TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD')                             \r\n";
                SQL += " AND SeekDate <TO_DATE('" + argCls.SeekDate + " 23:59','YYYY-MM-DD HH24:MI')                \r\n";
                SQL += " AND Pano = '" + argCls.Pano + "'                                                           \r\n";
                SQL += " AND XJong  = '" + argCls.XJong + "'                                                        \r\n";

            }
            else if (argCls.Job == "07")
            {
                SQL += " AND Pano = '" + argCls.Pano + "'                                                           \r\n";
                SQL += " AND SeekDate >=TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD')                             \r\n";
                SQL += " AND SeekDate <TO_DATE('" + argCls.SeekDate + " 23:59','YYYY-MM-DD HH24:MI')                \r\n";
                SQL += " AND PacsStudyID > ' '                                                                      \r\n";
                if (argCls.GbCombine ==true)
                {
                    SQL += " AND XJong  = '1'                                                                       \r\n";
                }


            }
            if (argOrdBy != "")
            {
                SQL += "  ORDER BY " + argOrdBy + "                                                                 \r\n";
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

        public DataTable sel_XrayDetail(PsmhDb pDbCon, cXrayDetail argCls, bool bLog)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                         \r\n";            
            SQL += "  IPDOPD,GBRESERVED,PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE,WARDCODE                                 \r\n";
            SQL += " ,ROOMCODE,XJONG,XSUBCODE,XCODE,EXINFO,QTY,EXMORE,EXID,GBEND,MGRNO,GBPORTABLE                   \r\n";
            SQL += " ,REMARK,XRAYROOM,GBNGT,DRREMARK,ORDERNO,ORDERCODE,PACSNO,ORDERNAME,PACSSTUDYID                 \r\n";
            SQL += " ,PACS_END,GBREAD,READ_SEND,READ_RECEIVE,READ_FLAG,AGREE,ORDERDATE                              \r\n";
            SQL += " ,GBPRINT,EXAM_WRTNO,DRDATE,DRWRTNO,STUDY_REF,IMAGE_BDATE                                       \r\n";
            SQL += " ,GBHIC,HIC_WRTNO,HIC_CODE,BI,CADEX_DEL,EMGWRTNO,GBSPC,RDATE,GBSTS                              \r\n";
            SQL += " ,CSABUN,CREMARK,N_STS,N_REMARK,GB_MANUAL,PICKUPREMARK                                          \r\n";
            SQL += " ,GSABUN,GBINFO,CVR,CVR_DATE,CVR_GUBUN,CVR_DRSABUN,CVR_SEND,GBER,ASA                            \r\n";
            SQL += " ,TO_CHAR(ENTERDATE,'YYYY-MM-DD HH24:MI') ENTERDATE                                             \r\n";
            SQL += " ,TO_CHAR(SEEKDATE,'YYYY-MM-DD HH24:MI') SEEKDATE                                               \r\n";
            SQL += " ,TO_CHAR(SEEKDATE,'YYYY-MM-DD') SEEKDATE_DATE                                                  \r\n";
            SQL += " ,TO_CHAR(SEEKDATE,'HH24:MI') SEEKDATE_TIME                                                     \r\n";
            SQL += " ,TO_CHAR(SENDDATE,'YYYY-MM-DD HH24:MI') SENDDATE                                               \r\n";
            SQL += " ,TO_CHAR(XSENDDATE,'YYYY-MM-DD HH24:MI') XSENDDATE                                             \r\n";
            SQL += " ,TO_CHAR(PC_BACKDATE,'YYYY-MM-DD HH24:MI') PC_BACKDATE                                         \r\n";
            SQL += " ,TO_CHAR(DRDATE,'YYYY-MM-DD HH24:MI') DRDATE                                                   \r\n";
            SQL += " ,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE                                                             \r\n";
            SQL += " ,TO_CHAR(CDATE,'YYYY-MM-DD') CDATE                                                             \r\n";
            SQL += " ,TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE                                                         \r\n";
            SQL += " ,TO_CHAR(N_ENTDATE,'YYYY-MM-DD') N_ENTDATE                                                     \r\n";
            SQL += " ,TO_CHAR(CON_DATE,'YYYY-MM-DD') CON_DATE                                                       \r\n";
            SQL += " ,TO_CHAR(GDATE,'YYYY-MM-DD') GDATE                                                             \r\n";
            SQL += " ,TO_CHAR(CVR_DATE,'YYYY-MM-DD') CVR_DATE                                                       \r\n";
            SQL += " ,TO_CHAR(CVR_CDATE,'YYYY-MM-DD') CVR_CDATE                                                     \r\n";            

            SQL += "  ,ROWID                                                                                        \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                                   \r\n";                    
            if (argCls.Job == "01")
            {
                SQL += " AND Pano = '" + argCls.Pano + "'                                                           \r\n";
                if (argCls.GbViewGbn =="01")
                {
                    SQL += " AND SeekDate >=TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD')                         \r\n";
                    SQL += " AND SeekDate <TO_DATE('" + argCls.SeekDate + " 23:59','YYYY-MM-DD HH24:MI')            \r\n";
                }
                
                SQL += " AND PacsStudyID > ' '                                                                      \r\n";
                if (argCls.GbCombine == true)
                {
                    SQL += " AND XJong  = '1'                                                                       \r\n";
                }
                
            }
            
            if (argCls.Job == "00")
            {
                SQL += "   ORDER BY SeekDate DESC                                                                   \r\n";
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

        public DataTable sel_XrayDetail_order(PsmhDb pDbCon, cXrayDetail argCls, string argJob = "")
        {
            DataTable dt = null;


            SQL = "";
            SQL += " SELECT                                                                                         \r\n";
            SQL += "   'I' IpdOpd, a.PTNO,TO_CHAR(a.BDATE,'YYYY-MM-DD') BDate,a.DEPTCODE,c.SNAME                    \r\n";
            SQL += "    , a.SuCode,b.XCode,a.OrderNo,a.Staffid DrCode                                               \r\n";
            SQL += "    , A.WARDCODE, A.ROOMCODE, SUM(a.QTY*a.NAL) CNT                                              \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "OCS_IORDER a                                                       \r\n";
            SQL += "    , " + ComNum.DB_PMPA + "XRAY_DETAIL b                                                       \r\n";
            SQL += "    , " + ComNum.DB_PMPA + "BAS_PATIENT c                                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                                   \r\n";
            SQL += "   AND a.BDate >=TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                   \r\n";
            SQL += "   AND a.BDate <=TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                                   \r\n";
            if (argJob == "ANGIO")
            {
                SQL += "   AND a.SuCode IN (" + argCls.XCode + ")                                                   \r\n";
            }
            else
            {
                SQL += "   AND a.SuCode ='" + argCls.XCode + "'                                                     \r\n";
            }
            SQL += "   AND (a.GbSend = ' ' OR a.GbSend = '*')                                                       \r\n";
            SQL += "   AND a.Ptno=b.Pano(+)                                                                         \r\n";
            SQL += "   AND a.Ptno=c.Pano                                                                            \r\n";
            SQL += "   AND a.BDate=b.BDate(+)                                                                       \r\n";
            SQL += "   AND a.OrderCode=b.OrderCode(+)                                                               \r\n";
            SQL += "  GROUP BY 1,a.PTNO,TO_CHAR(a.BDATE,'YYYY-MM-DD'),a.DEPTCODE,c.SNAME,a.SuCode                   \r\n";
            SQL += "            ,b.XCode,a.OrderNo,a.Staffid,A.WARDCODE, A.ROOMCODE                                 \r\n";
            SQL += "  HAVING SUM(a.QTY* a.NAL) > 0                                                                            \r\n";
            SQL += " UNION ALL                                                                                      \r\n";
            SQL += " SELECT                                                                                         \r\n";
            SQL += "   'O' IpdOpd, a.PTNO,TO_CHAR(a.BDATE,'YYYY-MM-DD') BDate,a.DEPTCODE,c.SNAME                    \r\n";
            SQL += "    , a.SuCode,b.XCode,a.OrderNo,a.DrCode                                                       \r\n";
            SQL += "    , '' WardCode, '' ROOMCODE, SUM(a.QTY*a.NAL) CNT                                            \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "OCS_OORDER a                                                       \r\n";
            SQL += "    , " + ComNum.DB_PMPA + "XRAY_DETAIL b                                                       \r\n";
            SQL += "    , " + ComNum.DB_PMPA + "BAS_PATIENT c                                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                                   \r\n";
            SQL += "   AND a.BDate >=TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                   \r\n";
            SQL += "   AND a.BDate <=TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                                   \r\n";
            //SQL += "   AND a.SuCode ='" + argCls.XCode + "'                                                         \r\n";
            if (argJob == "ANGIO")
            {
                SQL += "   AND a.SuCode IN (" + argCls.XCode + ")                                                   \r\n";
            }
            else
            {
                SQL += "   AND a.SuCode ='" + argCls.XCode + "'                                                     \r\n";
            }
            SQL += "   AND (a.GbSunap = '0' OR a.GbSunap = '1' OR a.GbSunap = '2')                                  \r\n";
            SQL += "   AND a.Ptno=b.Pano(+)                                                                         \r\n";
            SQL += "   AND a.Ptno=c.Pano                                                                            \r\n";
            SQL += "   AND a.BDate=b.BDate(+)                                                                       \r\n";
            SQL += "   AND a.OrderCode=b.OrderCode(+)                                                               \r\n";
            SQL += "  GROUP BY 1,a.PTNO,TO_CHAR(a.BDATE,'YYYY-MM-DD'),a.DEPTCODE,c.SNAME                            \r\n";
            SQL += "            ,a.SuCode,b.XCode,a.OrderNo,a.DrCode                                                \r\n";
            SQL += "  HAVING  SUM(a.QTY*a.NAL) > 0                                                                  \r\n";

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
        /// 영상의학과 기사 조회 쿼리
        /// </summary>
        /// <param name="argSabun"></param>
        /// <param name="argSDate"></param>
        /// <param name="argTDate"></param>
        /// <returns></returns>
        public DataTable sel_XrayGisa(PsmhDb pDbCon, string argSabun, string argSDate, string argTDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                         \r\n";
            SQL += "  TO_CHAR(BDate,'DD') ILJA,SCH                                                  \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_GISA                                         \r\n";
            SQL += "  WHERE 1 = 1                                                                   \r\n";
            SQL += "   AND Sabun ='" + argSabun + "'                                                \r\n";
            SQL += "   AND BDate >= " + ComFunc.covSqlDate(argSDate, "YYYY-MM-DD", false) + "\r\n";
            SQL += "   AND BDate <= " + ComFunc.covSqlDate(argTDate + "", "YYYY-MM-DD", false) + "\r\n";
            SQL += "   ORDER BY TO_CHAR(BDate,'DD')                                                 \r\n";

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
        /// 영상테이블 기준으로 종검,건진 조인 하여 체크하는 쿼리
        /// </summary>
        /// <param name="argPano"></param>
        /// <param name="argBDate"></param>
        /// <param name="argDept"></param>
        /// <param name="argPacsno"></param>
        /// <returns></returns>
        public DataTable sel_XrayhicheaJepsu(PsmhDb pDbCon, string argPano, string argBDate, string argDept, string argPacsno)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                         \r\n";
            SQL += "  a.ROWID                                                                                                       \r\n";
            if (argDept == "HR")
            {
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a, " + ComNum.DB_PMPA + "HIC_JEPSU b                              \r\n";
            }
            else if (argDept == "TO")
            {
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a, " + ComNum.DB_PMPA + "HEA_JEPSU b                              \r\n";
            }

            SQL += "  WHERE 1 = 1                                                                                                   \r\n";
            SQL += "   AND a.Pano =b.Ptno(+)                                                                                        \r\n";
            SQL += "   AND a.Pano ='" + argPano + "'                                                                                \r\n";
            SQL += "   AND a.DeptCode ='" + argDept + "'                                                                            \r\n";                            
            SQL += "   AND a.SeekDate >= " + ComFunc.covSqlDate(argBDate, "YYYY-MM-DD", false);
            SQL += "   AND a.SeekDate <= " + ComFunc.covSqlDate(argBDate + " 23:59", "YYYY-MM-DD HH24:MI", false);
            SQL += "   AND a.PacsNo = '" + argPacsno + "'                                                                           \r\n";
            if (argDept == "HR")
            {
                SQL += "   AND TRUNC(a.SeekDate) =b.JEPDATE(+)                                                                      \r\n";
                SQL += "   AND b.GJJONG IN ('21','22','27','29','30','49')                                                          \r\n";
            }
            else if (argDept == "TO")
            {
                SQL += "   AND TRUNC(a.SeekDate) =b.SDATE(+)                                                                      \r\n";
                SQL += "   AND b.GbDaily ='Y'                                                                                       \r\n";
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
        /// 종검,건진 관련 사용 쿼리
        /// </summary>
        /// <param name="argPano"></param>
        /// <param name="argBDate"></param>
        /// <param name="argPacsno"></param>
        /// <returns></returns>
        public string read_XrayHicHeaJepsu(PsmhDb pDbCon, string argPano, string argBDate, string argPacsno)
        {
            DataTable dt = null;

            dt = sel_XrayhicheaJepsu(pDbCon, argPano, argBDate, "HR", argPacsno);


            if (dt.Rows.Count > 0)
            {
                return "검진채용";
            }

            dt = null;

            dt = sel_XrayhicheaJepsu(pDbCon, argPano, argBDate, "TO", argPacsno);

            if (dt.Rows.Count > 0)
            {
                return "종검상담";

            }
            else
            {
                return "";
            }



        }

        /// <summary>
        /// 영상의학과 일보에 사용되는 쿼리
        /// </summary>
        /// <param name="Job"></param>
        /// <param name="argIO"></param>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public DataTable sel_Xray_Detail_Tong(PsmhDb pDbCon, string Job, string argIO, string argDate)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                                         \r\n";
                if (Job == "과별")
                {
                    SQL += "  a.DeptCode,a.Pano,SUM(b.Ccnt * Qty) Cnt1,SUM(b.Bcnt * Qty) Cnt2           \r\n";
                }
                else if (Job == "기사별")
                {
                    SQL += "  a.DeptCode,a.Pano,a.EXID,SUM(b.Ccnt * Qty) Cnt1,SUM(b.Bcnt * Qty) Cnt2    \r\n";
                }
                else if (Job == "기사별2")
                {
                    SQL += "  a.EXID,SUM(b.Ccnt * Qty) Cnt1,SUM(b.Bcnt * Qty) Cnt2                      \r\n";
                }
                else if (Job == "촬영종류별1")
                {
                    SQL += "  a.XJong,a.XSubCode,SUM(b.Bcnt * a.Qty) Cnt1,SUM(b.Ccnt * a.Qty) Cnt2      \r\n";
                }
                else if (Job == "촬영종류별2")
                {
                    SQL += "  a.XJong,a.XSubCode,SUM(DECODE(a.Pano,'00000200',a.Qty,1)) Cnt1            \r\n";
                }

                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a, " + ComNum.DB_PMPA + "XRAY_CODE b  \r\n";
                SQL += "  WHERE 1 = 1                                                                   \r\n";
                SQL += "    AND a.SeekDate >= TO_DATE('" + argDate + "', 'YYYY-MM-DD')                  \r\n";
                SQL += "    AND a.SeekDate <= TO_DATE('" + argDate + " 23:59', 'YYYY-MM-DD HH24:MI')    \r\n";
                SQL += "    AND a.GbEnd = '1'                                                           \r\n";
                SQL += "    AND (a.GbHIC IS NULL OR a.GbHIC <> 'Y')                                     \r\n";
                SQL += "    AND a.Pano <> '88888888'                                                    \r\n";
                SQL += "    AND a.XCode = b.XCode                                                       \r\n";
                if (Job == "촬영종류별1")
                {
                    SQL += "    AND a.XCode <> 'G2702'                                                  \r\n";
                }
                else if (Job == "촬영종류별2")
                {
                    SQL += "    AND a.XCode = 'G2702'                                                   \r\n";
                }
                if (argIO != "") SQL += "    AND a.IpdOpd ='" + argIO + "'                                \r\n";
                if (Job == "과별")
                {
                    SQL += "  GROUP BY a.DeptCode,a.Pano                                                \r\n";
                    SQL += "  ORDER BY a.DeptCode,a.Pano                                                \r\n";
                }
                else if (Job == "기사별")
                {
                    SQL += "  GROUP BY a.DeptCode,a.Pano,a.ExiD                                         \r\n";
                    SQL += "  ORDER BY a.ExiD,a.Pano                                                    \r\n";
                }
                else if (Job == "기사별2")
                {
                    SQL += "  GROUP BY a.ExiD                                                           \r\n";
                    SQL += "  ORDER BY a.ExiD                                                           \r\n";
                }
                else if (Job == "촬영종류별1" || Job == "촬영종류별2")
                {
                    SQL += "  GROUP BY a.XJong,a.XSubCode                                               \r\n";
                    SQL += "  ORDER BY a.XJong,a.XSubCode                                               \r\n";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        /// <summary>
        /// 영상의학과 CD복사 리스트 쿼리
        /// </summary>
        /// <param name="argGubun"></param>
        /// <param name="argSDate"></param>
        /// <param name="argTDate"></param>
        /// <returns></returns>
        public DataTable sel_XRAY_CDCOPY(PsmhDb pDbCon, cXrayCdCopy argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT  ";
            if (argCls.Job == "03" || argCls.Job == "07" || argCls.Job == "09")
            {
                SQL += "   TO_CHAR(BDate,'YYYY-MM-DD') BDate,Pano,SName,WardCode,RoomCode               \r\n";
                SQL += "   ,EntSabun,CdQty,IPDOPD                                                       \r\n";
                SQL += "  ,CdMake,TO_CHAR(CopyTime,'YYYY-MM-DD HH24:MI') CopyTime,CDGubun,COUNT(*) CNT  \r\n";
            }            
            else if (argCls.Job == "05")
            {
                SQL += "   TO_CHAR(BDate,'YYYY-MM-DD') BDate,Pano,SName,CdQty,CDGubun,COUNT(*) CNT      \r\n";                
            }            
            else
            {
                SQL += "   PANO,SNAME,IPDOPD,XJONG,XCODE,XSUBCODE,XNAME,DEPTCODE,DRCODE                 \r\n";
                SQL += "  ,ROOMCODE,WARDCODE,PACSNO,CDQTY,CDMAKE,COPYTIME,ENTSABUN,ENTTIME,CDGUBUN      \r\n";
                SQL += "  ,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE                                            \r\n";
                SQL += "  ,TO_CHAR(SeekDate,'YYYY-MM-DD') SeekDate                                      \r\n";
                SQL += "  ,TO_CHAR(CopyTime,'YYYY-MM-DD HH24:MI') CopyTime                              \r\n";
                SQL += "  ,TO_CHAR(EntTime,'YYYY-MM-DD HH24:MI') EntTime                                \r\n";
                SQL += "  ,DELDATE,DELSABUN,ORDERNO,ROWID                                               \r\n";
                SQL += "  ,KOSMOS_OCS.FC_NUR_TEAM_TELNO(RoomCode) FC_TeamNo                             \r\n"; //팀번호       
            }            
            SQL += " FROM " + ComNum.DB_PMPA + "XRAY_CDCOPY                                             \r\n";
            SQL += "  WHERE 1=1                                                                         \r\n";
            if (argCls.Job == "00")
            {
                SQL += "   AND BDate>=TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                      \r\n";
                SQL += "   AND BDate<=TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                      \r\n";
                SQL += "   AND CdGubun ='" + argCls.CdGubun + "'                                        \r\n";
            }
            else if (argCls.Job == "01")
            {
                SQL += "   AND BDate=TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                       \r\n";                
                SQL += "   AND Pano ='" + argCls.Pano + "'                                              \r\n";
                SQL += "   AND CdGubun ='" + argCls.CdGubun + "'                                        \r\n";
                SQL += "   AND OrderNo =" + argCls.OrderNo + "                                          \r\n";
            }
            else if (argCls.Job == "02")
            {
                SQL += "   AND BDate=TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                       \r\n";                
                SQL += "   AND Pano ='" + argCls.Pano + "'                                              \r\n";                
                SQL += "   AND CdGubun IN ('1','2')                                                     \r\n";
                SQL += "   AND Pacsno ='" + argCls.PacsNo + "'                                          \r\n";
            }
            else if (argCls.Job == "03")
            {
                SQL += "   AND BDate<=TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                      \r\n";
                SQL += "   AND Pano ='" + argCls.Pano + "'                                              \r\n";
                SQL += "   AND CdGubun IN ('1','2')                                                     \r\n";
            }
            else if (argCls.Job == "04" || argCls.Job == "07" || argCls.Job == "08")
            {
                SQL += "   AND BDate=TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                       \r\n";
                SQL += "   AND Pano ='" + argCls.Pano + "'                                              \r\n";
                
                //2018-08-31안정수, CdGubun의 길이에 따라 in 방식으로 수정 
                if(argCls.CdGubun.Length == 1)
                {
                    SQL += "   AND CdGubun ='" + argCls.CdGubun + "'                                        \r\n";
                }
                if (argCls.CdGubun.Length >= 2)
                {
                    SQL += "   AND CdGubun IN ('" + argCls.CdGubun + "')                                    \r\n";
                }
                
                
            }
            else if (argCls.Job == "05")
            {
                SQL += "   AND BDate=TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                       \r\n";
                SQL += "   AND Pano ='" + argCls.Pano + "'                                              \r\n";
                SQL += "   AND CdGubun IN ('1','2')                                                     \r\n";
            }
            else if (argCls.Job == "06")
            {
                SQL += "   AND BDate=TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                       \r\n";
                SQL += "   AND Pano ='" + argCls.Pano + "'                                              \r\n";
                SQL += "   AND CdGubun ='" + argCls.CdGubun + "'                                        \r\n";
                SQL += "   AND CopyTime IS NOT NULL                                                     \r\n";
            }
            else if (argCls.Job == "09")
            {
                SQL += "   AND BDate=TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                       \r\n";
                SQL += "   AND CdGubun IN ('1','2')                                                     \r\n";
                SQL += "   AND XJong NOT IN ('C')                                                       \r\n";
                SQL += "   AND XCode NOT IN ('US-TEE','US-TEER','US-CADVR','US-CADVR')                  \r\n";

            }
            else if (argCls.Job == "10")
            {
                SQL += "   AND BDate>=TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                      \r\n";
                SQL += "   AND BDate<=TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                      \r\n";
                SQL += "   AND CdGubun IN ('1','2')                                                     \r\n";
                SQL += "   AND XJong IN ('Q', '9')                                                      \r\n";
                //SQL += "   AND XCode NOT IN ('US-TEE','US-TEER','US-CADVR','US-CADVR')                  \r\n";

            }
            ////2018-08-31 안정수 .Job == 07일경우 추가
            //else if (argCls.Job == "07")
            //{
            //    SQL += "   AND BDate=TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                       \r\n";
            //    SQL += "   AND CdGubun IN ('" + argCls.CdGubun + "')                                    \r\n";
            //}
            else
            {
                SQL += "   AND BDate=TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                       \r\n";
                SQL += "   AND CdGubun ='" + argCls.CdGubun + "'                                        \r\n";
            }

            SQL += "   AND (DelDate IS NULL OR DelDate ='')                                             \r\n";

            if (argCls.Job == "03" || argCls.Job == "07" || argCls.Job == "09")
            {
                SQL += "   GROUP BY BDate,Pano,SName,WardCode,RoomCode,EntSabun                         \r\n";
                SQL += "           ,CdQty,CdMake,CopyTime,CDGubun,IPDOPD                                \r\n";
            }
            else if (argCls.Job == "05")
            {
                SQL += "   GROUP BY BDate,Pano,SName,CdQty,CDGubun                                      \r\n";                
            }
            else
            {
                
            }

            if (argCls.Job =="03")
            {
                SQL += "   ORDER BY BDate DESC                                                          \r\n";
            }
            else if (argCls.Job == "04")
            {
                SQL += "   ORDER BY SeekDate,XJong                                                      \r\n";
            }
            else if (argCls.Job == "08")
            {
                SQL += "   ORDER BY SeekDate,XJong                                                      \r\n";
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

        public DataTable sel_XRAY_CONTRAST(PsmhDb pDbCon, cXrayContrast argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                          \r\n";
            SQL += "   PANO,SNAME,REMARK,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,EntSabun,ROWID           \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "XRAY_CONTRAST                                       \r\n";
            SQL += "  WHERE 1=1                                                                     \r\n";
            SQL += "   AND BDate >=TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                     \r\n";
            SQL += "   AND BDate <=TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                     \r\n";
            if (argCls.Search != "")
            {
                SQL += "   AND (SName LIKE '%" + argCls.Date2 + "%'                                 \r\n";
                SQL += "         OR Pano = '" + argCls.Date2 + "' )                                 \r\n";
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
        /// 영상의학과 CD복사 - 개인별 영상 목록 쿼리
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPano"></param>
        /// <returns></returns>
        public DataTable sel_XRAY_DETAIL_ENDO(PsmhDb pDbCon,string argPano)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                 \r\n";
            SQL += "    /*+ PUSH_SUBQ */ a.Pano,b.SName,TO_CHAR(a.SeekDate,'YYYYMMDD HH24:MI') SeekDate                     \r\n";
            SQL += "   ,TO_CHAR(a.SeekDate,'MM/DD HH24:MI') JepsuTime,a.DeptCode,a.XSubCode                                 \r\n";
            SQL += "   ,TO_CHAR(a.EnterDate,'YYYYMMDD HH24:MI') JDate, a.DrCode,c.DrName,d.XName,a.ExInfo                   \r\n";
            SQL += "   ,a.PacsNo,a.XCode,a.IpdOpd,a.OrderNo                                                                 \r\n";
            SQL += "   ,TO_CHAR(a.EnterDate,'YYYYMMDD HH24:MI') OrderDate                                                   \r\n";
            SQL += "   ,TO_CHAR(a.XSendDate,'YYYYMMDD HH24:MI') XSendDate                                                   \r\n";
            SQL += "   ,a.PacsStudyID,a.OrderName,a.Remark,a.XJong                                                          \r\n";
            SQL += "   ,'' ResultDate,a.ROWID                                                                               \r\n";
            SQL += "   ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_XRAY_접수종류',TRIM(a.XJong)) FC_XJong2                        \r\n"; //종류체크
            SQL += " FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                                               \r\n";
            SQL += "    , " + ComNum.DB_PMPA + "BAS_PATIENT b                                                               \r\n";
            SQL += "    , " + ComNum.DB_PMPA + "BAS_DOCTOR c                                                                \r\n";
            SQL += "    , " + ComNum.DB_PMPA + "XRAY_CODE d                                                                 \r\n";
            SQL += "  WHERE 1=1                                                                                             \r\n";
            SQL += "   AND a.Pano = '" +     argPano  + "'                                                                  \r\n";
            SQL += "   AND a.SeekDate<=TRUNC(SYSDATE+1)                                                                     \r\n";
            SQL += "   AND a.GbReserved >='6'                                                                               \r\n";
            SQL += "   AND a.Pano=b.Pano(+)                                                                                 \r\n";
            SQL += "   AND a.DrCode=c.DrCode(+)                                                                             \r\n";
            SQL += "   AND a.XCode=d.XCode(+)                                                                               \r\n";
            SQL += "   AND a.XJong NOT IN ('C')                                                                             \r\n";
            //2019-11-22 안정수, 'CAGCOPY' 추가 
            SQL += "   AND a.XCode NOT IN ('F12','F08','XCDC','XDVDC','CAGCOPY','F71C','F74C','FR71C','FR74C')              \r\n";
            SQL += "  UNION ALL                                                                                             \r\n";
            SQL += " SELECT                                                                                                 \r\n";
            SQL += "   /*+ PUSH_SUBQ */ a.PtNo Pano,b.SName,TO_CHAR(a.RDate,'YYYYMMDD HH24:MI') SeekDate                    \r\n";
            SQL += "   ,TO_CHAR(a.JDate,'MM/DD HH24:MI') JepsuTime, a.DeptCode,'' XSubCode                                  \r\n";
            SQL += "   ,TO_CHAR(a.JDate,'YYYYMMDD HH24:MI') JDate,  a.DrCode,c.DrName,d.OrderName XName,a.SeqNo ExInfo      \r\n";
            SQL += "   ,a.PacsNo,a.OrderCode XCode,a.GbIO IpdOpd,a.OrderNo                                                  \r\n";
            SQL += "   , '' OrderDate,'' XSendDate                                                                          \r\n";
            SQL += "   ,a.PacsUID PacsStudyID,d.OrderName,a.Remark,'D' XJong                                                \r\n";
            SQL += "   ,TO_CHAR(a.ResultDate,'YYYY-MM-DD') ResultDate,a.ROWID                                               \r\n";
            SQL += "   ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_XRAY_접수종류','D') FC_XJong2                                  \r\n"; //종류체크
            SQL += " FROM " + ComNum.DB_MED + "ENDO_JUPMST a                                                                \r\n";
            SQL += "    , " + ComNum.DB_PMPA + "BAS_PATIENT b                                                               \r\n";
            SQL += "    , " + ComNum.DB_PMPA + "BAS_DOCTOR c                                                                \r\n";
            SQL += "    , " + ComNum.DB_MED + "OCS_ORDERCODE d                                                              \r\n";
            SQL += "  WHERE 1=1                                                                                             \r\n";
            SQL += "   AND a.Ptno = '" + argPano + "'                                                                       \r\n";
            SQL += "   AND a.RDate<=TRUNC(SYSDATE+1)                                                                        \r\n";
            SQL += "   AND a.GbSunap IN  ('1','7')                                                                          \r\n";
            SQL += "   AND a.Ptno=b.Pano(+)                                                                                 \r\n";
            SQL += "   AND RTRIM(a.DrCode)=c.DrCode(+)                                                                      \r\n";
            SQL += "   AND a.OrderCode=d.OrderCode(+)                                                                       \r\n";
            SQL += "  ORDER BY 2,1,3 DESC                                                                                   \r\n";
            
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
                
        public class cXray_Pacs_Order
        {
            public string Job = "";            
            public string Patid = "";
            public string FALG = "";
            public string HISORDERID = "";
            public string ACDESSIONNO = "";            
            public string PacsNo = "";
            public string Operator = "";
            public string ROWID = "";
        }

        public DataTable sel_XRAY_PACS_ORDER(PsmhDb pDbCon, cXray_Pacs_Order argCls)
        {
            DataTable dt = null;


            SQL = "";
            SQL += " SELECT                                                                             \r\n";
            SQL += "  ACDESSIONNO                                                                       \r\n";            
            SQL += "   FROM " + ComNum.DB_PACS + "XRAY_PACS_ORDER                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                       \r\n";
            if (argCls.Job == "00")
            {
                SQL += "   AND PATID ='" + argCls.Patid + "'                                            \r\n";
                SQL += "   AND HISORDERID ='" + argCls.HISORDERID + "'                                  \r\n";
            }
            else
            {
                SQL += "   AND PATID ='" + argCls.Patid + "'                                            \r\n";
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

        public DataTable sel_HIC_EXCODE(PsmhDb pDbCon, string argCode)
        {
            DataTable dt = null;


            SQL = "";
            SQL += " SELECT                                                     \r\n";
            SQL += "  EName,YName                                               \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "HIC_EXCODE                    \r\n";
            SQL += "  WHERE 1 = 1                                               \r\n";
            SQL += "   AND Code ='" + argCode + "'                              \r\n";


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

        public DataTable sel_XRAY_WAIT(PsmhDb pDbCon, cXray_Wait argCls,bool bLog)
        {
            DataTable dt = null;
            
            SQL = "";
            SQL += " SELECT                                                                             \r\n";
            SQL += "  Gbn Gubun, DeptCode,SeqTime,SName,Pano                                            \r\n";
            SQL += "  ,DECODE(Gbn,'0','응급환자','1','어르신먼저','2','원거리환자','') Gubun2          \r\n";
            SQL += "  ,TO_CHAR(JepTime,'YY/MM/DD HH24:MI') JepTime                                      \r\n";
            SQL += "  ,ROWID                                                                            \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_WAIT                                             \r\n";
            SQL += "  WHERE 1 = 1                                                                       \r\n";
            if (argCls.Job =="00")
            {
                SQL += "   AND Pano ='" + argCls.PANO + "'                                              \r\n";
                SQL += "   AND GBROOM ='" + argCls.GBROOM + "'                                          \r\n";
                SQL += "   AND JDate =TO_DATE('" + argCls.JDATE + "','YYYY-MM-DD')                      \r\n";
            }
            else if (argCls.Job == "01")
            {
                SQL += "   AND Pano ='" + argCls.PANO + "'                                              \r\n";
                SQL += "   AND GBROOM ='" + argCls.GBROOM + "'                                          \r\n";
                SQL += "   AND OrderNo =" + argCls.ORDERNO + "                                          \r\n";

                if (argCls.GBEND == "0")
                {
                    SQL += "   AND GBEND ='" + argCls.GBEND + "'                                        \r\n";
                    SQL += "   AND JEPTIME >= trunc(SYSDATE)                                            \r\n";
                }

                else if (argCls.GBEND == "1")
                {
                    SQL += "   AND GBEND ='" + argCls.GBEND + "'                                        \r\n";
                    SQL += "   AND JEPTIME >= trunc(SYSDATE)                                            \r\n";
                }
            }
            else if (argCls.Job == "02")
            {
                SQL += "   AND Pano ='" + argCls.PANO + "'                                              \r\n";
                SQL += "   AND Part ='" + argCls.PART + "'                                              \r\n";

                if (argCls.STime.CompareTo("03:00") >= 0)
                {
                    SQL += "   AND JDate =TO_DATE('" + argCls.JDATE + "','YYYY-MM-DD')                  \r\n";
                }
                else
                {
                    SQL += "   AND JDate >=TO_DATE('" + argCls.JDATE + "','YYYY-MM-DD')                 \r\n";
                    SQL += "   AND JDate <=TO_DATE('" + argCls.JDATE + " 23:59','YYYY-MM-DD HH24:MI')   \r\n";
                }
             
            }
            else if (argCls.Job == "03")
            {
                SQL += "   AND JDate =TO_DATE('" + argCls.JDATE + "','YYYY-MM-DD')                      \r\n";         
                SQL += "   AND GBROOM ='" + argCls.GBROOM + "'                                          \r\n";                
                SQL += "   AND Part ='" + argCls.PART + "'                                              \r\n";
                if (argCls.GBEND !="")
                {
                    SQL += "   AND GbEnd ='" + argCls.GBEND + "'                                        \r\n";
                }
            }
            else if (argCls.Job == "TOTAL")
            {
                SQL += "   AND JDate =TO_DATE('" + argCls.JDATE + "','YYYY-MM-DD')                      \r\n";
                if (argCls.GBROOM == "1")
                {
                    SQL += "   AND GBROOM IN ('1','2')                                                  \r\n";
                }
                else
                {
                    SQL += "   AND GBROOM ='" + argCls.GBROOM + "'                                      \r\n";
                }
                SQL += "   AND Part ='" + argCls.PART + "'                                              \r\n";
                if (argCls.GBEND != "")
                {
                    SQL += "   AND GbEnd ='" + argCls.GBEND + "'                                        \r\n";
                }

            }
            else
            {
                SQL += "   AND Pano ='" + argCls.PANO + "'                                              \r\n";
                SQL += "   AND GBROOM ='" + argCls.GBROOM + "'                                          \r\n";
                SQL += "   AND JDate =TO_DATE('" + argCls.JDATE + "','YYYY-MM-DD')                      \r\n";
            }

            if (argCls.Job == "TOTAL")
            {
                SQL += "  ORDER BY Gbn,SeqTime,JepTime,SName                                            \r\n";
            }
            else if (argCls.Job == "03")
            {
                SQL += "  ORDER BY Gbn,SeqTime,JepTime,SName                                            \r\n";
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

        public bool XRAY_WAIT_CHK(PsmhDb pDbCon, cXray_Wait argCls)
        {
            DataTable dt = sel_XRAY_WAIT(pDbCon, argCls,false);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string XRAY_WAIT_INSERT(PsmhDb pDbCon,cXray_Wait argCls)
        {
            int intRowAffected = 0; //변경된 Row 받는 변수
                        
            argCls.GBROOM = "1";   //촬영실구분 1.번촬영 2.번촬영
            argCls.PART = "2";     //1.본관 2.루가            
            
            if (argCls.DEPTCODE =="DT" || argCls.DEPTCODE == "TO" || argCls.DEPTCODE == "RM" || argCls.DEPTCODE == "PD" || argCls.DEPTCODE == "MR")
            {
                if (argCls.GbIO == "O" && argCls.GBROOM == "1")
                {
                    argCls.PART = "1";
                }
                else if(argCls.GbIO !="O")
                {
                    argCls.PART = "1";
                }

            }
            else
            {
                argCls.PART = "1";
            }
            
            if (argCls.CHEST =="Y" && argCls.DEPTCODE !="ER" && argCls.DEPTCODE !="PD" )
            {
                argCls.GBROOM = "2";
            }            

            //2018-08-17 안정수, 응급 CT촬영 관련 조건 체크추가
            if (argCls.ERCT == "Y")
            {
                argCls.GBN = "응급";
                argCls.GBROOM = "C";
            }

            //환자구분 0.ER 1.어르신 2.원거리 9.일반            
            if (argCls.GBN =="응급")
            {
                argCls.GBN = "0";
            }
            else if (argCls.GBN == "어르신")
            {
                argCls.GBN = "1";
                if (argCls.GbIO =="I")
                {
                    argCls.GBN = "9";
                }
            }
            else if (argCls.GBN == "원거리")
            {
                argCls.GBN = "9";
            }
            else
            {
                argCls.GBN = "9";
            }


            SqlErr = "";

            if (argCls.PART =="2")
            {
                return "";
            }

            DataTable dt = sel_XRAY_WAIT(pDbCon, argCls,false);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                argCls.STS = "1";
                argCls.ROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                SqlErr = up_XRAY_WAIT(pDbCon, argCls, ref intRowAffected);
                                            
            }
            else
            {
                argCls.STS = "0";
                SqlErr = ins_XRAY_WAIT(pDbCon, argCls, ref intRowAffected);
            }


            return SqlErr;
        }

        public string XRAY_WAIT_UPDATE(PsmhDb pDbCon, cXray_Wait argCls)
        {
            int i = 0;
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strROWID = "";

            SqlErr = "";

            DataTable dt = sel_XRAY_WAIT(pDbCon, argCls,false);

            if (ComFunc.isDataTableNull(dt) == false)
            {

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strROWID += "'" + dt.Rows[0]["ROWID"].ToString().Trim() + "',";
                }

                if (strROWID != "")
                {
                    strROWID = VB.Left(strROWID, strROWID.Length - 1);

                    argCls.Job = "01"; //단순촬영
                    argCls.ROWID = strROWID;
                    SqlErr = up_XRAY_WAIT(pDbCon, argCls, ref intRowAffected);

                }

            }

            return SqlErr;
        }

        public string XRAY_WAIT_CT_UPDATE(PsmhDb pDbCon, cXray_Wait argCls) 
        {
            int i = 0;
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strROWID = "";

            SqlErr = "";

            DataTable dt = sel_XRAY_WAIT(pDbCon, argCls,false);

            if (ComFunc.isDataTableNull(dt) == false)
            {

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strROWID += "'" + dt.Rows[0]["ROWID"].ToString().Trim() + "',";
                }

                if (strROWID != "")
                {
                    strROWID = VB.Left(strROWID, strROWID.Length - 1);

                    argCls.Job = "02"; //CT
                    argCls.ROWID = strROWID;

                    SqlErr = up_XRAY_WAIT(pDbCon, argCls, ref intRowAffected);

                }

            }

            return SqlErr;
        }

        public string XRAY_WAIT_CT_INSERT(PsmhDb pDbCon, cXray_Wait argCls)
        {
            //int i = 0;
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strROWID = "";

            SqlErr = "";

            argCls.GBROOM = "C";  // 기본1번촬영
            argCls.PART = "2";    //'1.예약 2.당일 3.응급
            argCls.CHEST = "N";

            if  (VB.Left(argCls.RDATE,10).CompareTo(argCls.JDATE) > 0)
            {
                argCls.PART = "1";
            }
            else
            {
                argCls.PART = "2";
            }
                    
            if(argCls.GBN  == "자동")
            {
                if (argCls.GbIO =="O" && argCls.DEPTCODE =="ER")
                {
                    argCls.PART = "3";
                }
            }
            else if (argCls.GBN == "예약")
            {
                argCls.PART = "1";
            }
            else if (argCls.GBN == "당일")
            {
                argCls.PART = "2";
            }
            else if (argCls.GBN == "응급")
            {
                argCls.PART = "3";
            }

            argCls.GBEND = "0";
            if (argCls.GBN =="완료")
            {
                argCls.GBEND = "1"; //완료
            }

            argCls.SEQTIME = VB.Right(argCls.RDATE, 5).Replace(":", "") + "00";
            if ( VB.Right(argCls.RDATE,5) != argCls.RDATE2)
            {
                argCls.SEQTIME = VB.Right(argCls.RDATE2, 5).Replace(":", "") + "00";
            }

            argCls.GBN = "9";
            argCls.Job = "01";
            DataTable dt = sel_XRAY_WAIT(pDbCon, argCls,false);

            if (ComFunc.isDataTableNull(dt) == false)
            {                                
                strROWID = dt.Rows[0]["ROWID"].ToString().Trim() ;
                
                if (strROWID != "")
                {
                    strROWID = VB.Left(strROWID, strROWID.Length - 1);

                    argCls.Job = "02"; //CT
                    argCls.ROWID = strROWID;
                    argCls.STS = "1";
                    
                    SqlErr = up_XRAY_WAIT(pDbCon, argCls, ref intRowAffected);

                }

            }
            else
            {
                argCls.STS = "0";
                SqlErr = ins_XRAY_WAIT(pDbCon, argCls, ref intRowAffected);
            }

            return SqlErr;
        }

        public DataTable sel_CDCOPY_ORDER(PsmhDb pDbCon, clsComSupXraySQL.cXrayCdCopy argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                             \r\n";
            SQL += "  SUM(Nal * Qty) CNT                                                \r\n";
            if (argCls.GbIO =="I" || argCls.GbER =="Y")
            {
                SQL += "   FROM " + ComNum.DB_MED + "OCS_IORDER                         \r\n";
            }
            else
            {
                SQL += "   FROM " + ComNum.DB_MED + "OCS_OORDER                         \r\n";
            }
            
            SQL += "  WHERE 1 = 1                                                       \r\n";
            SQL += "   AND Ptno ='" + argCls.Pano + "'                                      \r\n";
            if (argCls.GbER == "Y")
            {
                SQL += "   AND BDate =TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')          \r\n";
            }
            else
            {
                SQL += "   AND BDate =TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')          \r\n";
            }
            
            if (argCls.CdGubun =="1")
            {
                //2019-11-22 안정수, CAGCOPY 추가 
                SQL += "   AND SuCode IN ('XCDC', 'CAGCOPY')                                \r\n";
            }
            else if (argCls.CdGubun == "2")
            {
                SQL += "   AND SuCode IN ('XDVDC')                                      \r\n";
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

        public string Read_CDCOPY_Order(PsmhDb pDbCon,clsComSupXraySQL.cXrayCdCopy argCls)
        {
            string strOK = "OK";

            if (argCls.GbER =="Y")
            {
                DataTable dt = sel_CDCOPY_ORDER(pDbCon, argCls);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    if (Convert.ToInt32(dt.Rows[0]["CNT"].ToString().Trim()) > 0)
                    {
                        if (argCls.CdQty != Convert.ToInt32(dt.Rows[0]["CNT"].ToString().Trim()))
                        {
                            ComFunc.MsgBox("시디 갯수가 틀립니다.. 확인후 다시 오더전송하십시오..");
                            strOK = "RETURN";
                        }
                        
                    }
                    else
                    {
                        ComFunc.MsgBox("오더내역이 없습니다. (XCDC,XDVDC)오더를 먼저 전송후 작업 하십시오..");
                        strOK = "RETURN";
                    }
                }
                else
                {
                    ComFunc.MsgBox("오더내역이 없습니다. (XCDC,XDVDC)오더를 먼저 전송후 작업 하십시오..");
                    strOK = "RETURN";
                }
            }
            else
            {
                DataTable dt = sel_CDCOPY_ORDER(pDbCon, argCls);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    if (dt.Rows[0]["CNT"].ToString().Trim() !="" && Convert.ToInt32(dt.Rows[0]["CNT"].ToString().Trim()) > 0)
                    {
                        string s = "이미 XCDC,XDVDC 오더(수량:" + dt.Rows[0]["CNT"].ToString().Trim() + ")가 발생했습니다" + "\r\n" + "\r\n";
                               s += "1.예(Y):CD목록 및 오더동시(XCDC..) 전송" +"\r\n" + "\r\n";
                               s += "2.아니오(N):CD목록만 전" ;
                        if (ComFunc.MsgBoxQ(s, "확인전송", MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            if (argCls.GbIO =="O")
                            {
                                strOK = "OK2";
                            }
                        }
                    }
                }
            }

            return strOK;
        }

        public DataTable sel_OCS_DOCTOR_insa(PsmhDb pDbCon,string argDrCode)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                 \r\n";
            SQL += "  a.DrName,b.HTEL,b.MSTEL                               \r\n";            
            SQL += "   FROM " + ComNum.DB_MED + "OCS_DOCTOR a               \r\n";
            SQL += "     ,  " + ComNum.DB_ERP + "INSA_MST b                 \r\n";
            SQL += "  WHERE 1 = 1                                           \r\n";
            SQL += "   AND a.Sabun = b.Sabun(+)                             \r\n";
            SQL += "   AND a.DrCode = '" + argDrCode + "'                   \r\n";
            
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

        public string Read_DrCode_Hphone(PsmhDb pDbCon, string argDrCode)
        {
            DataTable dt = sel_OCS_DOCTOR_insa(pDbCon, argDrCode);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                return dt.Rows[0]["DrName"].ToString().Trim();
            }
            else
            {
                return "";
            }
        }

        public DataTable sel_OCS_DOCTOR(PsmhDb pDbCon, string argSabun, string argDrCode, string argCols, string argWhere)
        {
            string SqlErr = string.Empty;

            DataTable dt = null;

            if (argSabun == "" && argDrCode == "")
            {
                return null ;
            }

            SQL = "";
            SQL += " SELECT                                                 \r\n";
            if (argCols !="")
            {
                SQL += "  " + argCols + "                                   \r\n";
            }
            else
            {
                SQL += "  DRNAME,DEPTCODE                                   \r\n";
            }            
            SQL += "  FROM " + ComNum.DB_MED + "OCS_DOCTOR                  \r\n";            
            SQL += "  WHERE 1=1                                             \r\n";
            if (argSabun != "")
            {
                SQL += "    AND Sabun = '" + argSabun + "'                  \r\n";
            }
            if (argDrCode != "")
            {
                SQL += "    AND DrCode = '" + argDrCode + "'                \r\n";
            }
            if (argWhere != "")
            {
                SQL += "      " + argWhere + "                              \r\n";
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

        public DataTable sel_BAS_PASS_insa(PsmhDb pDbCon)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                 \r\n";
            SQL += "  a.IDNumber || '.' || a.Name AS Names                  \r\n";
            SQL += "  ,a.IDNumber, a.Name                                   \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_PASS a                \r\n";
            SQL += "     ,  " + ComNum.DB_ERP + "INSA_MST b                 \r\n";
            SQL += "  WHERE 1 = 1                                           \r\n";
            SQL += "   AND a.IDNUMBER = b.SABUN                             \r\n";
            SQL += "   AND a.Charge = 'X'                                   \r\n";
            SQL += "   AND b.ToiDay IS NULL                                 \r\n";
            SQL += "     ORDER BY a.Name                                    \r\n";

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

        public DataTable sel_XRAY_DETAIL_Code_Ilgi(PsmhDb pDbCon,string Job ,string argIO, string argFDate, string argTDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                     \r\n";
            SQL += "  a.XJong,a.XSubCode                                                        \r\n";
            SQL += "  ,DECODE(a.DeptCode,'TO',1,'HR',1,2) DeptCode                              \r\n";
            SQL += "  ,a.XrayRoom,SUM(b.Ccnt * a.Qty) Cnt1                                      \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                 \r\n";
            SQL += "     ,  " + ComNum.DB_PMPA + "XRAY_CODE b                                   \r\n";
            SQL += "  WHERE 1 = 1                                                               \r\n";
            SQL += "   AND a.SeekDate >= TO_DATE('" + argFDate + "','YYYY-MM-DD')               \r\n";
            SQL += "   AND a.SeekDate <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI') \r\n";
            SQL += "   AND a.GbEnd = '1'                                                        \r\n";
            SQL += "   AND a.XSUBCODE <> '66'                                                   \r\n";
            SQL += "   AND a.Pano <> '88888888'                                                 \r\n";
            SQL += "   AND a.Pano <> '81000004'                                                 \r\n";
            if (Job == "00")
            {
                SQL += "   AND a.XCode NOT IN ( 'G2702','HA010' )                               \r\n";
            }
            else if (Job == "01")
            {
                SQL += "   AND a.XCode IN ( 'HA010' )                                           \r\n";
                SQL += "   AND a.XJong ='2'                                                     \r\n";
                SQL += "   AND a.XSubCode ='02'                                                 \r\n";
            }
            else if (Job == "02")
            {
                SQL += "   AND a.XCode IN ( 'G2702' )                                           \r\n";                
            }

            SQL += "   AND a.XCode = b.XCode                                                    \r\n";

            if (argIO == "I")
            {
                SQL += "   AND a.IpdOpd ='I'                                                    \r\n";
            }
            else if (argIO == "O")
            {
                SQL += "   AND a.IpdOpd ='O'                                                    \r\n";
            }
            SQL += "  GROUP BY a.XJong,a.XSubCode                                               \r\n";
            SQL += "            ,DECODE(a.DeptCode,'TO',1,'HR',1,2) ,a.XrayRoom                  \r\n";
            SQL += "  ORDER BY a.XJong,a.XSubCode                                               \r\n";

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

        public DataTable sel_XRAY_ILGI(PsmhDb pDbCon,string argDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                 \r\n";
            SQL += "  BDATE,SUNAME,CTNAME,MRINAME,RINAME,SONONAME,DDRNAME,ROOMNAME1,ROOMNAME2               \r\n";
            SQL += "  ,ROOMNAME3 , ROOMNAME4, ROOMNAME5, ENAME, NNAME, WORK, BUSEROOM, Item                 \r\n";
            SQL += "  ,WORK1,WORK2,WORK3,Work4, BUSEROOM1, Item1, HUNAME, GONAME, SSAIN1, SSAIN2, ROWID     \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_ILGI                                                 \r\n";            
            SQL += "  WHERE 1 = 1                                                                           \r\n";
            SQL += "   AND BDATE = TO_DATE('" + argDate  + "','YYYY-MM-DD')                                 \r\n";            

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

        public DataTable sel_XRAY_ILGI_ANGIO(PsmhDb pDbCon, string argDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                     \r\n";
            SQL += "  REMARK,CALL,CALL2,ROWID                                   \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_ILGI_ANGIO               \r\n";
            SQL += "  WHERE 1 = 1                                               \r\n";
            SQL += "   AND BDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')      \r\n"; 

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

        public DataTable sel_XRAY_ILGI_ANGIO_SUB(PsmhDb pDbCon, cXray_Ilgi_Angio_Sub argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                             \r\n";
            SQL += "  CODE,CNT,TCNT,DEPTCODE,DRNAME,DAMNAME,Remark                      \r\n";
            SQL += "  ,TO_CHAR(BDate,'YYYY-MM-DD') BDate                                \r\n";
            SQL += "  ,TO_CHAR(Call_Time1,'YYYY-MM-DD HH24:MI') Call_Time1              \r\n";
            SQL += "  ,TO_CHAR(Call_Time2,'YYYY-MM-DD HH24:MI') Call_Time2              \r\n";
            SQL += "  ,ROWID                                                            \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "XRAY_ILGI_ANGIO_SUB                     \r\n";
            SQL += "  WHERE 1 = 1                                                       \r\n";
            if (argCls.Job =="00")
            {
                SQL += "   AND BDATE = TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')     \r\n";
            }
            else if (argCls.Job == "01")
            {
                SQL += "   AND BDATE >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')    \r\n";
                SQL += "   AND BDATE <= TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')    \r\n";
            }
            SQL += "   AND (DELDATE IS NULL OR DELDATE ='')                             \r\n";
            if (argCls.Code !="")
            {
                if (argCls.Code =="X")
                {
                    SQL += "   AND SUBSTR(CODE,1,1) ='X'                                \r\n";
                }
                else
                {
                    SQL += "   AND CODE = '" + argCls.Code + "'                         \r\n";
                }
            }
            SQL += "  ORDER BY CODE                                                     \r\n";

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

        public DataTable sel_XRAY_ILGI_ANGIO_SUB_Tot(PsmhDb pDbCon,string argJob , string argDate,string argCode)
        {
            DataTable dt = null;

            string strStartDate =  VB.Left(argDate,8) + "01";
            string strEDate = Convert.ToDateTime(argDate).AddDays(-1).ToShortDateString();

            SQL = "";
            SQL += " SELECT                                                         \r\n";
            SQL += "  SUM(CNT) SumCNT,SUM(TCNT) SumTCNT                             \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_ILGI_ANGIO_SUB               \r\n";
            SQL += "  WHERE 1 = 1                                                   \r\n";
            SQL += "   AND BDATE >= TO_DATE('" + strStartDate + "','YYYY-MM-DD')    \r\n";
            SQL += "   AND BDATE <= TO_DATE('" + strEDate + "','YYYY-MM-DD')        \r\n";
            SQL += "   AND (DELDATE IS NULL OR DELDATE ='')                         \r\n";
            if (argJob =="00")
            {
                SQL += "   AND Code NOT IN (                                            \r\n";
                SQL += "                     SELECT TRIM(CODE)                          \r\n";
                SQL += "                      FROM KOSMOS_PMPA.BAS_BCODE                \r\n";
                SQL += "                       WHERE 1 = 1                              \r\n";
                SQL += "                        AND GUBUN='혈관조영_업무일지_코드4'     \r\n";
                SQL += "                    )                                           \r\n";
            }
            else if (argJob == "01" || argJob == "02") //조영재 사용/입고
            {
                SQL += "   AND Code IN (                                                \r\n";
                SQL += "                     SELECT TRIM(CODE)                          \r\n";
                SQL += "                      FROM KOSMOS_PMPA.BAS_BCODE                \r\n";
                SQL += "                       WHERE 1 = 1                              \r\n";
                SQL += "                        AND GUBUN='혈관조영_업무일지_코드4'     \r\n";
                SQL += "                    )                                           \r\n";
            }            

            if (argCode !="")
            {
                SQL += "   AND CODE ='" + argCode + "'                              \r\n";
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

        public DataTable sel_XRAY_ILGI_ANGIO_CODE(PsmhDb pDbCon)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                 \r\n";
            SQL += "  SORT,CODE,NAME                                        \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_BCODE                 \r\n";
            SQL += "  WHERE 1 = 1                                           \r\n";
            SQL += "   AND GUBUN='혈관조영_업무일지_코드1'                  \r\n";
            SQL += " UNION ALL                                              \r\n";
            SQL += " SELECT                                                 \r\n";
            SQL += "  SORT,CODE,NAME                                        \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_BCODE                 \r\n";
            SQL += "  WHERE 1 = 1                                           \r\n";
            SQL += "   AND GUBUN='혈관조영_업무일지_코드2'                  \r\n";
            SQL += " UNION ALL                                              \r\n";
            SQL += " SELECT                                                 \r\n";
            SQL += "  SORT,CODE,NAME                                        \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_BCODE                 \r\n";
            SQL += "  WHERE 1 = 1                                           \r\n";
            SQL += "   AND GUBUN='혈관조영_업무일지_코드3'                  \r\n";
            SQL += " UNION ALL                                              \r\n";
            SQL += " SELECT                                                 \r\n";
            SQL += "  SORT,CODE,NAME                                        \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_BCODE                 \r\n";
            SQL += "  WHERE 1 = 1                                           \r\n";
            SQL += "   AND GUBUN='혈관조영_업무일지_코드4'                  \r\n";
            SQL += " UNION ALL                                              \r\n";
            SQL += " SELECT                                                 \r\n";
            SQL += "  SORT,CODE,NAME                                        \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_BCODE                 \r\n";
            SQL += "  WHERE 1 = 1                                           \r\n";
            SQL += "   AND GUBUN='혈관조영_업무일지_코드5'                  \r\n";
            SQL += "  ORDER BY SORT,CODE                                    \r\n";

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

        public DataTable sel_HIC_EXJONG(PsmhDb pDbCon, string argJong)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT Code || '.' || Name Names                       \r\n"; 
            SQL += "     ,Code,Name                                         \r\n";

            SQL += "   FROM " + ComNum.DB_PMPA + "HIC_EXJONG                \r\n";
            SQL += "  WHERE 1 = 1                                           \r\n";
            if (argJong !="")
            {
                SQL += "   AND Code = '" + argJong + "'                     \r\n";
            }
            SQL += "    ORDER BY Code                                       \r\n";

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

        public DataTable sel_EXAM_ANATMST(PsmhDb pDbCon, string argDate1,string argDate2)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT a.PTNo,c.SName,a.GbIO,a.DeptCode                                        \r\n";
            SQL += "     ,b.OrderName, a.ROWID                                                      \r\n";
            SQL += "     ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                       \r\n";
            SQL += "     ,TO_CHAR(a.ResultDate,'YYYY-MM-DD') ResultDate                             \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_ANATMST a                                     \r\n";
            SQL += "      , " + ComNum.DB_MED + "OCS_ORDERCODE b                                    \r\n";
            SQL += "      , " + ComNum.DB_PMPA + "BAS_PATIENT c                                     \r\n";
            SQL += "  WHERE 1 = 1                                                                   \r\n";            
            SQL += "   AND a.ResultDate >= TO_DATE('" + argDate1 + "','YYYY-MM-DD')                 \r\n";
            SQL += "   AND a.ResultDate <= TO_DATE('" + argDate2 + " 23:59','YYYY-MM-DD HH24:MI')   \r\n";
            SQL += "   AND a.GbJob='V'                                                              \r\n"; //검사완료
            SQL += "   AND SUBSTR(a.AnatNo,1,1) = 'S'                                               \r\n"; //조직검사
            SQL += "   AND a.DeptCode <> 'TO'                                                       \r\n"; //종검제외
            SQL += "   AND a.OrderCode = b.OrderCode(+)                                             \r\n";
            SQL += "   AND (b.SendDept <> 'N' OR b.SendDept IS NULL)                                \r\n";
            SQL += "   AND a.PTno = c.Pano(+)                                                       \r\n";
            SQL += "    ORDER BY a.ResultDate DESC                                                  \r\n";

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

        public DataTable sel_EXAM_ANATMST(PsmhDb pDbCon, string argJob, string argROWID, string argPano)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT Result1,Result2                             \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_ANATMST           \r\n";            
            SQL += "  WHERE 1 = 1                                       \r\n";
            if (argJob =="00")
            {
                SQL += "   AND ROWID ='" + argROWID + "'                \r\n";
            }
            else if (argJob == "01")
            {
                SQL += "   AND Ptno ='" + argPano + "'                  \r\n";
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

        public class cXrayErGisaChk
        {
            public string Job = "";
            public string Part = "";
            public string Sabun = "";
            public string BDate = "";
            public string XRoom = "";
            public string sysdate = "";
            public string systime = "";
            public string chk1 = "";
        }

        public bool Read_ER_XRayGisa_Chk(PsmhDb pDbCon, cXrayErGisaChk argCls)
        {
            bool bOK = true;
            string s = string.Empty;
            string strGbRoom = string.Empty;

            DataTable dt = null;

            //오전 08:29 까지는 이전날 번표 읽음
            if (argCls.systime.CompareTo("08:30") < 0)
            {
                //2018-08-07 안정수, AddDay(1).AddDays(-1)부분 주석처리 후 AddDays(-1)만 남겨둠

                //argCls.BDate = Convert.ToString(Convert.ToDateTime(argCls.sysdate).AddDays(1).AddDays(-1).ToShortDateString());
                argCls.BDate = Convert.ToString(Convert.ToDateTime(argCls.sysdate).AddDays(-1).ToShortDateString());
                argCls.chk1 = "Y";
            }            

            //2018-11-07 안정수, JOB에 따라 응급촬영실 또는 응급CT촬영실을 설정
            if(argCls.Job == "01")
            {
                strGbRoom = "응급촬영실";
            }
            else if(argCls.Job == "02")
            {
                strGbRoom = "응급CT촬영실"; 
            }

            dt = sel_XRAY_Gisa_BCode(pDbCon, argCls);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                #region //기본데이타 세팅
                string strSCH = dt.Rows[0]["Sch"].ToString().Trim();
                string strTemp = dt.Rows[0]["Name"].ToString().Trim();

                string strSTime = clsComSup.setP(strTemp, "^^", 1).Trim();
                string strETime = clsComSup.setP(strTemp, "^^", 2).Trim();
                #endregion

                if (strSCH =="")
                {
                    #region //스케쥴 체크
                    if (argCls.XRoom == "1")
                    {
                        bOK = false;
                        s = strGbRoom + " 촬영기사 스케쥴 등록안됨!!.." + "\r\n" + "\r\n" + "스케쥴 등록하거나 촬영기사변경후 작업하세요";
                        ComFunc.MsgBox(s);
                        return bOK;
                    }
                    #endregion
                }
                else
                {
                    //다음날 나이트
                    if (argCls.chk1 =="Y")
                    {
                        #region //나이트 번표 체크후 종료시간으로 체크
                        if (argCls.XRoom == "1")
                        {
                            if (argCls.systime.CompareTo(strETime) < 0)
                            {                                
                            } 
                            else
                            {
                                bOK = false;
                                s = strGbRoom + " 촬영기사 스케쥴 시간 아닙니다..!! 현재시간:" + argCls.systime + "\r\n" + "\r\n" + "스케쥴 설정시간 :" + strSTime + "-" + strETime + "입니다.." + "\r\n" + "\r\n" + "촬영기사변경후 작업하세요";
                                ComFunc.MsgBox(s);
                                return bOK;
                            }                           
                        }
                        else
                        {
                            if (argCls.systime.CompareTo(strETime) < 0)
                            {
                                bOK = false;
                                s = strGbRoom + " 촬영기사 스케쥴입니다..!! 현재시간:" + argCls.systime + "\r\n" + "\r\n" + "스케쥴 설정시간 :" + strSTime + "-" + strETime + "입니다.." + "\r\n" + "\r\n" + "촬영기사변경후 작업하세요";
                                ComFunc.MsgBox(s);
                                return bOK;
                            }
                            else
                            {                                
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        //시작시간으로 체크
                        if (argCls.XRoom =="1")
                        {
                            #region //응급촬영실 N 스케쥴
                            if ( VB.Left( strSCH,1) == "N")
                            {
                                if (argCls.systime.CompareTo(strSTime) >= 0)
                                {
                                }
                                else
                                {
                                    bOK = false;
                                    s = strGbRoom + " 촬영기사 스케쥴 시간 아닙니다..!! 현재시간:" + argCls.systime + "\r\n" + "\r\n" + "스케쥴 설정시간 :" + strSTime + "-" + strETime + "입니다.." + "\r\n" + "\r\n" + "촬영기사변경후 작업하세요";
                                    ComFunc.MsgBox(s);
                                    return bOK;
                                }
                            }
                            else
                            {
                                if (argCls.systime.CompareTo(strSTime) >= 0 && argCls.systime.CompareTo(strETime) < 0)
                                {
                                }
                                else
                                {
                                    bOK = false;
                                    s = strGbRoom + " 촬영기사 스케쥴 시간 아닙니다..!! 현재시간:" + argCls.systime + "\r\n" + "\r\n" + "스케쥴 설정시간 :" + strSTime + "-" + strETime + "입니다.." + "\r\n" + "\r\n" + "촬영기사변경후 작업하세요";
                                    ComFunc.MsgBox(s);
                                    return bOK;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region //N 스케쥴
                            if (VB.Left(strSCH, 1) == "N")
                            {
                                if (argCls.systime.CompareTo(strSTime) >= 0)
                                {
                                    bOK = false;
                                    s = strGbRoom + " 촬영기사 스케쥴입니다..!! 현재시간:" + argCls.systime + "\r\n" + "\r\n" + "스케쥴 설정시간 :" + strSTime + "-" + strETime + "입니다.." + "\r\n" + "\r\n" + "촬영기사변경후 작업하세요";
                                    ComFunc.MsgBox(s);
                                    return bOK;
                                }
                                else
                                {                                    
                                }
                            }
                            else
                            {
                                if (argCls.systime.CompareTo(strSTime) >= 0 && argCls.systime.CompareTo(strETime) < 0)
                                {
                                    bOK = false;
                                    s = strGbRoom + " 촬영기사 스케쥴입니다..!! 현재시간:" + argCls.systime + "\r\n" + "\r\n" + "스케쥴 설정시간 :" + strSTime + "-" + strETime + "입니다.." + "\r\n" + "\r\n" + "촬영기사변경후 작업하세요";
                                    ComFunc.MsgBox(s);
                                    return bOK;
                                }
                                else
                                {                                    
                                }
                            }
                            #endregion
                        }

                    }

                }
                
            }
            else
            {
                if (argCls.XRoom == "1")
                {
                    bOK = false;
                    s = strGbRoom + " 촬영기사 스케쥴 등록안됨!!.." + "\r\n" + "\r\n" + "스케쥴 등록하거나 촬영기사변경후 작업하세요";
                    ComFunc.MsgBox(s);
                    return bOK;
                }
            }
            
            return bOK;
        }

        public DataTable sel_XRAY_Gisa_BCode(PsmhDb pDbCon, cXrayErGisaChk argCls )
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                 \r\n";
            SQL += "  a.SCH,b.Name                                                          \r\n";
            SQL += "  , a.ROWID                                                             \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_GISA a                               \r\n";
            SQL += "      , " + ComNum.DB_PMPA + "BAS_BCODE b                               \r\n";
            SQL += "  WHERE 1 = 1                                                           \r\n";
            SQL += "   AND a.SCH=b.Code(+)                                                  \r\n";
            SQL += "   AND a.BDate = TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')           \r\n";
            SQL += "   AND a.SABUN = '" + argCls.Sabun + "'                                 \r\n";
            SQL += "   AND a.PART = '" + argCls.Part + "'                                   \r\n";

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
        /// 2018-11-22 안정수, 워크리스트 및 재료입력에서 촬영기사를 읽기 위함
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argSabun"></param>
        /// <param name="argSDate"></param>
        /// <param name="argTDate"></param>
        /// <returns></returns>
        public DataTable sel_XrayGisaRead(PsmhDb pDbCon, string argSabun, string argTime = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                         \r\n";
            SQL += "    SABUN, SCH                                                                  \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_GISA                                         \r\n";
            SQL += "  WHERE 1 = 1                                                                   \r\n";
            SQL += "   AND SABUN <> '" + argSabun +"'                                               \r\n";
            SQL += "   AND SCH IS NOT NULL                                                          \r\n";

            //2019-09-20 안정수 조건 추가
            //Night 근무자 셋팅으로 하루전 스케줄을 읽는다
            if (argTime != "")
            {
                if (String.Compare(argTime, "00:00:00") >= 0 && String.Compare(argTime, "08:30:00") <= 0)
                {
                    SQL += "   AND BDate >= TRUNC(SYSDATE) -1                                       \r\n";
                    SQL += "   AND BDate <= TRUNC(SYSDATE) -1                                       \r\n";
                }
                else
                {
                    SQL += "   AND BDate >= TRUNC(SYSDATE)                                          \r\n";
                    SQL += "   AND BDate <= TRUNC(SYSDATE)                                          \r\n";
                }
            }
            else
            {
                SQL += "   AND BDate >= TRUNC(SYSDATE)                                              \r\n";
                SQL += "   AND BDate <= TRUNC(SYSDATE)                                              \r\n";
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

        #endregion

        #region //영상의학 PACS 뷰 쿼리

        public class cXray_PacsView
        {
            public string GbJob = "";
            public string GbViewGbn = "";
            public string GubunXray = "";
            public string GubunEndo = "";
            public string GubunHic = "";
            public string Pano = "";
            public string SeekDate = "";
            public string Date1 = "";
            public string Date2 = "";
            public string DeptCode = "";
            public string DrCode = "";
            public string WardCode = "";
            public string XJong = "";
            public string NRead = "";
            public string strPacsNo = "";
            public bool bHic = false;
            public bool GbCombine = false;
            public string ROWID = "";

        }

        public DataTable sel_Xray_PacsView(PsmhDb pDbCon, cXray_PacsView argCls,bool bLog)
        { 
            DataTable dt = null; 
            
            SQL = "";
            SQL += " SELECT                                                                                                                     \r\n";
            SQL += "  /*+ PUSH_SUBQ */                                                                                                          \r\n";
            SQL += "  'XRAY' GBN,a.Pano,b.SName,a.DeptCode                                                                                      \r\n";           
            SQL += "  ,a.DrCode,c.DrName,d.XName,a.ExInfo,a.PacsNo,a.XCode,a.IpdOpd,a.OrderNo                                                   \r\n";            
            SQL += "  ,a.PacsStudyID,a.OrderName,a.Remark,a.XJong,b.Sex                                                                         \r\n";
            SQL += "  ,a.GBEND,a.DrWrtno, a.ROWID                                                                                               \r\n";            
            SQL += "  ,TO_CHAR(a.SeekDate,'YYYYMMDD HH24:MI') SeekDate                                                                          \r\n";
            SQL += "  ,TO_CHAR(a.EnterDate,'YYYYMMDD HH24:MI') JDate                                                                            \r\n";
            SQL += "  ,TO_CHAR(a.DrDate,'YYYY-MM-DD') DRDATE                                                                                    \r\n";
            SQL += "  ,TO_CHAR(a.EnterDate,'YYYYMMDD HH24:MI') OrderDate                                                                        \r\n";
            SQL += "  ,TO_CHAR(a.XSendDate,'YYYYMMDD HH24:MI') XSendDate                                                                        \r\n";
            SQL += "  ,'' ResultDate                                                                                                            \r\n";
            //2018-08-06 안정수, 병동, 병실칼럼 추가 
            //SQL += "  ,a.Wardcode, a.Roomcode                                                                                                   \r\n";
            SQL += "  ,KOSMOS_OCS.FC_IPD_NEW_MASTER_JSTS2(KOSMOS_OCS.FC_IPD_NEW_MASTER_JSTS(a.Pano)) FC_Ipd_Info      \r\n";  //재원체크
            SQL += "  ,KOSMOS_OCS.FC_GET_AGE(TO_CHAR(b.Birth,'YYYY-MM-DD'),a.BDate) FC_age                                                      \r\n"; //나이체크
            SQL += "  ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_XRAY_접수종류',TRIM(a.XJong)) FC_XJong2                                             \r\n"; //종류체크
            //SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,a.BDate) FC_infect                                                           \r\n"; //감염체크
            SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Pano,a.BDate) FC_infect_EX                                                           \r\n"; //감염체크
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                                                                 \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_PATIENT b                                                                                 \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_DOCTOR c                                                                                  \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "XRAY_CODE d                                                                                   \r\n";            
            SQL += "  WHERE 1 = 1                                                                                                               \r\n";
            SQL += "   AND 1 = '" + argCls.GubunXray + "'                                                                                       \r\n";
            SQL += "   AND a.SeekDate >=TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                            \r\n";
            SQL += "   AND a.SeekDate < TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                                                            \r\n";
            SQL += "   AND a.XCode NOT IN ('F12','F08','XCDC','F71C','F74C','FR71C','FR74C')                                                    \r\n";
            SQL += "   AND ((a.GbHIC IS NULL OR a.GbHIC <> 'Y') OR (a.XCODE = 'HA434B' AND GBHIC = 'Y'))                                        \r\n";
            SQL += "   AND (a.HIC_WRTNO IS NULL OR a.HIC_WRTNO <> 1 )                                                                           \r\n";
            SQL += "   AND a.GbReserved >='6'                                                                                                   \r\n";
            SQL += "   AND a.Pano=b.Pano(+)                                                                                                     \r\n";
            SQL += "   AND a.DrCode=c.DrCode(+)                                                                                                 \r\n";
            SQL += "   AND a.XCode=d.XCode(+)                                                                                                   \r\n";
            SQL += "   AND a.XCode NOT IN ( 'GR9701')                                                                                           \r\n";
            
            if (argCls.GbJob =="1") //등록번호별
            {
                SQL += "   AND a.Pano ='" + argCls.Pano + "'                                                                                    \r\n";
            }
            else if (argCls.GbJob == "2") //당일 외래접수자
            {
                #region //당일 외래접수자
                SQL += "   AND a.Pano IN ( SELECT Pano                                                                                          \r\n";
                SQL += "                    FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                               \r\n";
                SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                SQL += "                      AND ActDate =TRUNC(SYSDATE)                                                                       \r\n";
                if (argCls.DeptCode !="**")
                {
                    SQL += "                  AND DeptCode ='" + argCls.DeptCode + "'                                                           \r\n";
                    if (argCls.DrCode != "****" && argCls.DrCode != "")
                    {
                        SQL += "              AND DrCode ='" + argCls.DrCode + "'                                                               \r\n";

                    }
                }
                SQL += "                  GROUP BY Pano                                                                                         \r\n";
                SQL += "                  )                                                                                                     \r\n";
                #endregion
            }
            else if (argCls.GbJob == "3") //재원자
            {
                #region //재원자
                SQL += "   AND a.Pano IN ( SELECT Pano                                                                                          \r\n";
                SQL += "                    FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                                           \r\n";
                SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                SQL += "                      AND (OutDate IS NULL OR OutDate>=TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD'))                    \r\n";
                SQL += "                      AND IpwonTime <  TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                                     \r\n";
                SQL += "                      AND Amset4 <> '3'                                                                                 \r\n";
                SQL += "                      AND Pano <   '90000000'                                                                           \r\n";
                SQL += "                      AND Pano <>  '81000004'                                                                           \r\n";
                if (argCls.WardCode != "**")
                {
                    SQL += "                  AND WardCode ='" + argCls.WardCode + "'                                                           \r\n";                    
                }
                if (argCls.DeptCode != "**")
                {
                    SQL += "                  AND DeptCode ='" + argCls.DeptCode + "'                                                           \r\n";
                    if (argCls.DrCode != "****" && argCls.DrCode != "")
                    {
                        SQL += "              AND DrCode ='" + argCls.DrCode + "'                                                               \r\n";

                    }
                }
                SQL += "                  GROUP BY Pano                                                                                         \r\n";
                SQL += "                  )                                                                                                     \r\n";
                #endregion
            }
            else if (argCls.GbJob == "5") //응급실 재원자
            {
                #region //응급실 재원자
                SQL += "   AND a.Pano IN ( SELECT Pano                                                                                          \r\n";
                SQL += "                    FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                               \r\n";
                SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                SQL += "                      AND ActDate >=TRUNC(SYSDATE-1)                                                                    \r\n";               
                SQL += "                      AND DeptCode ='ER'                                                                                \r\n";
                SQL += "                      AND OCSJIN = '*'                                                                                  \r\n";
                SQL += "                  GROUP BY Pano                                                                                         \r\n";
                SQL += "                  )                                                                                                     \r\n";
                #endregion
            }
            else if (argCls.GbJob == "6") //당일입원자
            {
                #region //당일입원자
                SQL += "   AND a.Pano IN ( SELECT Pano                                                                                          \r\n";
                SQL += "                    FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                                           \r\n";
                SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                SQL += "                      AND InDate>=TO_DATE('" + Convert.ToDateTime(argCls.Date2).AddDays(-2).ToShortDateString() + "','YYYY-MM-DD')          \r\n";
                SQL += "                      AND IpwonTime >= TO_DATE('" + Convert.ToDateTime(argCls.Date2).AddDays(-1).ToShortDateString() + "','YYYY-MM-DD')     \r\n";
                SQL += "                      AND IpwonTime <  TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                                     \r\n";
                SQL += "                      AND Amset4 <> '3'                                                                                 \r\n";
                SQL += "                      AND Pano <   '90000000'                                                                           \r\n";
                SQL += "                      AND Pano <>  '81000004'                                                                           \r\n";                
                if (argCls.DeptCode != "**")
                {
                    SQL += "                  AND DeptCode ='" + argCls.DeptCode + "'                                                           \r\n";
                    if (argCls.DrCode != "****" && argCls.DrCode != "")
                    {
                        SQL += "              AND DrCode ='" + argCls.DrCode + "'                                                               \r\n";

                    }
                }
                SQL += "                  GROUP BY Pano                                                                                         \r\n";
                SQL += "                  )                                                                                                     \r\n";
                #endregion
            }
            else if (argCls.GbJob == "7") //종합검진
            {
                SQL += "   AND a.DeptCode ='TO'                                                                                                 \r\n";
                SQL += "   AND c.DrName<>'신체검사'                                                                                             \r\n";
            }
            else if (argCls.GbJob == "8") //일반검진
            {
                SQL += "   AND a.DeptCode ='HR'                                                                                                 \r\n";
            }
            else if (argCls.GbJob == "9") //당일촬영자
            {
                #region //당일촬영자
                if (argCls.WardCode != "**")
                {
                    SQL += "   AND a.WardCode ='" + argCls.WardCode + "'                                                                        \r\n";
                }
                if (argCls.DeptCode != "**")
                {
                    SQL += "   AND a.DeptCode ='" + argCls.DeptCode + "'                                                                        \r\n";
                    if (argCls.DrCode != "****" && argCls.DrCode != "")
                    {
                        SQL += "   a.AND DrCode ='" + argCls.DrCode + "'                                                                        \r\n";

                    }
                }
                #endregion
            }
            else if (argCls.GbJob == "A") //영상누락자
            {
                #region //영상누락자
                SQL += "   AND a.PacsStudyID IS NULL                                                                                            \r\n";
                SQL += "   AND a.XJong NOT IN ('6','7','E','G')                                                                                 \r\n";
                SQL += "   AND a.XCode NOT IN (SELECT XCode FROM " + ComNum.DB_PMPA + "XRAY_CODE WHERE GbPacs='N')                              \r\n";
                #endregion
            }
            else if (argCls.GbJob == "B") //초음파검사자
            {
                #region //초음파검사자
                SQL += "   AND a.Pano IN ( SELECT Pano                                                                                          \r\n";
                SQL += "                    FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                              \r\n";
                SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                SQL += "                      AND SeekDate >=TRUNC(SYSDATE)                                                                     \r\n";
                SQL += "                      AND SeekDate < TRUNC(SYSDATE+1)                                                                   \r\n";
                SQL += "                      AND XJong ='3'                                                                                    \r\n";               
                SQL += "                  GROUP BY Pano                                                                                         \r\n";
                SQL += "                  )                                                                                                     \r\n";
                #endregion
            }
            else if (argCls.GbJob == "C") //수술예약자
            {
                #region //수술예약자
                SQL += "   AND a.Pano IN ( SELECT Pano                                                                                          \r\n";
                SQL += "                    FROM " + ComNum.DB_MED + "OCS_OPSCHE                                                                \r\n";
                SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                SQL += "                      AND OpDate =TRUNC(SYSDATE)                                                                        \r\n";
                SQL += "                      AND (GbDel <> '*' OR GbDel IS NULL)                                                               \r\n";
                if (argCls.DeptCode != "**")
                {
                    SQL += "                  AND DeptCode ='" + argCls.DeptCode + "'                                                           \r\n";        
                }
                SQL += "                  GROUP BY Pano                                                                                         \r\n";
                SQL += "                  )                                                                                                     \r\n";
                #endregion
            }
            else if (argCls.GbJob == "D") //70세이상환자
            {
                #region //70세이상환자
                SQL += "   AND a.AGE >= 70                                                                                                      \r\n";
                SQL += "   AND a.Pano <> '81000004'                                                                                             \r\n";
                SQL += "   AND a.GBRESERVED IN ('6','7')                                                                                        \r\n";
                #endregion
            }            
            if (argCls.XJong == "X")
            {
                SQL += "   AND a.XCODE IN ('HA464','HA464A','HA444','HA454','HA454A')                                                           \r\n";
            }
            else if (argCls.XJong != "*" && argCls.XJong != "")
            {
                SQL += "   AND a.XJong = '" + argCls.XJong + "'                                                                                 \r\n";
            }
            if (argCls.NRead =="Y")
            {
                SQL += "   AND (a.ExInfo < 1001 OR a.ExInfo IS NULL)                                                                            \r\n";
            }

            //내시경 UINON
            #region //내시경 UINON
            if ((argCls.GubunEndo == "1" ) || ( argCls.XJong =="*" && argCls.GbJob !="A"))
            {
                SQL += " UNION ALL                                                                                                                  \r\n";
                SQL += " SELECT                                                                                                                     \r\n";
                SQL += "  /*+ PUSH_SUBQ */                                                                                                          \r\n";
                SQL += "  'ENDO' GBN,a.Ptno Pano,b.SName,a.DeptCode                                                                                 \r\n";
                SQL += "  ,a.DrCode,c.DrName,d.OrderName XName,NVL(a.SeqNo,0) ExInfo,a.PacsNo,a.OrderCode XCode,a.GbIO IpdOpd,a.OrderNo             \r\n";
                SQL += "  ,a.PacsUID PacsStudyID,d.OrderName,a.Remark,'D' XJong,b.Sex                                                               \r\n";         
                SQL += "  ,'1' GBEND,0 DrWrtno, a.ROWID                                                                                             \r\n";
                SQL += "  ,TO_CHAR(a.RDate,'YYYYMMDD HH24:MI') SeekDate                                                                             \r\n";
                SQL += "  ,TO_CHAR(a.JDate,'YYYYMMDD HH24:MI') JDate                                                                                \r\n";
                SQL += "  ,'' DRDATE                                                                                                                \r\n";
                //SQL += "  ,'' OrderDate                                                                                                             \r\n";
                //SQL += "  ,'' XSendDate                                                                                                             \r\n";
                //2018-08-06 안정수, 검사지시시각 및 촬영완료 칼럼에 데이터 보이기 위해서 수정함
                SQL += "  ,TO_CHAR(a.ORDERDATE,'YYYYMMDD HH24:MI') OrderDate                                                                        \r\n";
                SQL += "  ,TO_CHAR(a.XSendDate,'YYYYMMDD HH24:MI') XSendDate                                                                        \r\n";
                SQL += "  ,TO_CHAR(a.ResultDate,'YYYY-MM-DD') ResultDate                                                                            \r\n";
                //2018-08-06 안정수, 상단에 병동 및 호실 칼럼 추가하면서 추가함 
                SQL += "  ,''                                                                                                                   \r\n";

                SQL += "  ,KOSMOS_OCS.FC_GET_AGE(TO_CHAR(b.Birth,'YYYY-MM-DD'),a.BDate) FC_age                                                      \r\n"; //나이체크   
                SQL += "  ,'ENDO' FC_XJong2                                                                                                         \r\n"; //종류체크
                //SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Ptno,a.BDate) FC_infect                                                           \r\n"; //감염체크
                SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Ptno,a.BDate) FC_infect_EX                                                           \r\n"; //감염체크
                SQL += "   FROM " + ComNum.DB_MED + "ENDO_JUPMST a                                                                                  \r\n";
                SQL += "       ," + ComNum.DB_PMPA + "BAS_PATIENT b                                                                                 \r\n";
                SQL += "       ," + ComNum.DB_PMPA + "BAS_DOCTOR c                                                                                  \r\n";
                SQL += "       ," + ComNum.DB_MED + "OCS_ORDERCODE d                                                                                \r\n";
                SQL += "  WHERE 1 = 1                                                                                                               \r\n";
                SQL += "   AND 1 = '" + argCls.GubunEndo + "'                                                                                       \r\n";
                SQL += "   AND a.RDate >=TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                               \r\n";
                SQL += "   AND a.RDate < TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                                                               \r\n";
                SQL += "   AND a.Ptno=b.Pano(+)                                                                                                     \r\n";
                SQL += "   AND RTrim(a.DrCode)=c.DrCode(+)                                                                                          \r\n";
                SQL += "   AND a.OrderCode=d.OrderCode(+)                                                                                           \r\n";
                SQL += "   AND a.GbSunap in ('1','7')                                                                                               \r\n";
                if (argCls.GbJob == "1") //등록번호별
                {
                    SQL += "   AND a.Ptno ='" + argCls.Pano + "'                                                                                    \r\n";
                }
                else if (argCls.GbJob == "2") //당일 외래접수자
                {
                    #region //당일 외래접수자
                    SQL += "   AND a.Ptno IN ( SELECT Pano                                                                                          \r\n";
                    SQL += "                    FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                               \r\n";
                    SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                    SQL += "                      AND ActDate =TRUNC(SYSDATE)                                                                       \r\n";
                    if (argCls.DeptCode != "**")
                    {
                        SQL += "                  AND DeptCode ='" + argCls.DeptCode + "'                                                           \r\n";
                        if (argCls.DrCode != "****" && argCls.DrCode != "")
                        {
                            SQL += "              AND DrCode ='" + argCls.DrCode + "'                                                               \r\n";

                        }
                    }
                    SQL += "                  GROUP BY Pano                                                                                         \r\n";
                    SQL += "                  )                                                                                                     \r\n";
                    #endregion
                }
                else if (argCls.GbJob == "3") //재원자
                {
                    #region //재원자
                    SQL += "   AND a.Ptno IN ( SELECT Pano                                                                                          \r\n";
                    SQL += "                    FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                                           \r\n";
                    SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                    SQL += "                      AND (OutDate IS NULL OR OutDate>=TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD'))                    \r\n";
                    SQL += "                      AND IpwonTime <  TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                                     \r\n";
                    SQL += "                      AND Amset4 <> '3'                                                                                 \r\n";
                    SQL += "                      AND Pano <   '90000000'                                                                           \r\n";
                    SQL += "                      AND Pano <>  '81000004'                                                                           \r\n";
                    if (argCls.WardCode != "**")
                    {
                        SQL += "                  AND WardCode ='" + argCls.WardCode + "'                                                           \r\n";
                    }
                    if (argCls.DeptCode != "**")
                    {
                        SQL += "                  AND DeptCode ='" + argCls.DeptCode + "'                                                           \r\n";
                        if (argCls.DrCode != "****" && argCls.DrCode != "")
                        {
                            SQL += "              AND DrCode ='" + argCls.DrCode + "'                                                               \r\n";

                        }
                    }
                    SQL += "                  GROUP BY Pano                                                                                         \r\n";
                    SQL += "                  )                                                                                                     \r\n";
                    #endregion
                }
                else if (argCls.GbJob == "5") //응급실 재원자
                {
                    #region //응급실 재원자
                    SQL += "   AND a.Ptno IN ( SELECT Pano                                                                                          \r\n";
                    SQL += "                    FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                               \r\n";
                    SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                    SQL += "                      AND ActDate >=TRUNC(SYSDATE-1)                                                                    \r\n";
                    SQL += "                      AND DeptCode ='ER'                                                                                \r\n";
                    SQL += "                      AND OCSJIN = '*'                                                                                  \r\n";
                    SQL += "                  GROUP BY Pano                                                                                         \r\n";
                    SQL += "                  )                                                                                                     \r\n";
                    #endregion
                }
                else if (argCls.GbJob == "6") //당일입원자
                {
                    #region //당일입원자
                    SQL += "   AND a.Ptno IN ( SELECT Pano                                                                                          \r\n";
                    SQL += "                    FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                                           \r\n";
                    SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                    SQL += "                      AND InDate>=TO_DATE('" + Convert.ToDateTime(argCls.Date2).AddDays(-2).ToShortDateString() + "','YYYY-MM-DD')          \r\n";
                    SQL += "                      AND IpwonTime >= TO_DATE('" + Convert.ToDateTime(argCls.Date2).AddDays(-1).ToShortDateString() + "','YYYY-MM-DD')     \r\n";
                    SQL += "                      AND IpwonTime <  TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                                     \r\n";
                    SQL += "                      AND Amset4 <> '3'                                                                                 \r\n";
                    SQL += "                      AND Pano <   '90000000'                                                                           \r\n";
                    SQL += "                      AND Pano <>  '81000004'                                                                           \r\n";
                    if (argCls.DeptCode != "**")
                    {
                        SQL += "                  AND DeptCode ='" + argCls.DeptCode + "'                                                           \r\n";
                        if (argCls.DrCode != "****" && argCls.DrCode != "")
                        {
                            SQL += "              AND DrCode ='" + argCls.DrCode + "'                                                               \r\n";

                        }
                    }
                    SQL += "                  GROUP BY Pano                                                                                         \r\n";
                    SQL += "                  )                                                                                                     \r\n";
                    #endregion
                }
                else if (argCls.GbJob == "7") //종합검진
                {
                    ///SQL += "   AND a.PTno ='zz'                                                                                                     \r\n";
                    SQL += "   AND a.DeptCode ='TO'                                                                                                \r\n";
                }
                else if (argCls.GbJob == "8") //일반검진
                {
                    ///SQL += "   AND a.PTno >' '                                                                                                      \r\n";
                    SQL += "   AND a.DeptCode ='HR'                                                                                                \r\n";
                }
                else if (argCls.GbJob == "9") //당일촬영자
                {
                    #region //당일촬영자
                    if (argCls.WardCode != "**")
                    {
                        SQL += "   AND WardCode ='" + argCls.WardCode + "'                                                           \r\n";
                    }
                    if (argCls.DeptCode != "**")
                    {
                        SQL += "   AND DeptCode ='" + argCls.DeptCode + "'                                                                          \r\n";
                        if (argCls.DrCode != "****" && argCls.DrCode != "")
                        {
                            SQL += "   AND DrCode ='" + argCls.DrCode + "'                                                                          \r\n";

                        }
                    }
                    #endregion
                }
                else if (argCls.GbJob == "A") //영상누락자
                {
                    #region //영상누락자
                    SQL += "   AND a.PacsUID IS NULL                                                                                                \r\n";                    
                    #endregion
                }
                else if (argCls.GbJob == "B") //초음파검사자
                {
                    #region //초음파검사자
                    SQL += "   AND a.Ptno IN ( SELECT Pano                                                                                          \r\n";
                    SQL += "                    FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                              \r\n";
                    SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                    SQL += "                      AND SeekDate >=TRUNC(SYSDATE)                                                                     \r\n";
                    SQL += "                      AND SeekDate < TRUNC(SYSDATE+1)                                                                   \r\n";
                    SQL += "                      AND XJong ='3'                                                                                    \r\n";
                    SQL += "                  GROUP BY Pano                                                                                         \r\n";
                    SQL += "                  )                                                                                                     \r\n";
                    #endregion
                }
                else if (argCls.GbJob == "C") //수술예약자
                {
                    #region //수술예약자
                    SQL += "   AND a.Ptno IN ( SELECT Pano                                                                                          \r\n";
                    SQL += "                    FROM " + ComNum.DB_MED + "OCS_OPSCHE                                                                \r\n";
                    SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                    SQL += "                      AND OpDate =TRUNC(SYSDATE)                                                                        \r\n";
                    SQL += "                      AND (GbDel <> '*' OR GbDel IS NULL)                                                               \r\n";
                    if (argCls.DeptCode != "**")
                    {
                        SQL += "                  AND DeptCode ='" + argCls.DeptCode + "'                                                           \r\n";
                    }
                    SQL += "                  GROUP BY Pano                                                                                         \r\n";
                    SQL += "                  )                                                                                                     \r\n";
                    #endregion
                }
                if (argCls.NRead == "Y")
                {
                    SQL += "   AND a.ResultDate IS NULL                                                                                             \r\n";
                }

            }
            #endregion

            //건진 UINON
            #region //건진 UINON
            if ( (argCls.GubunHic =="1") || (argCls.XJong == "1" || argCls.XJong ==""))
            {
                SQL += " UNION ALL                                                                                                                  \r\n";
                SQL += " SELECT                                                                                                                     \r\n";
                SQL += "  /*+ PUSH_SUBQ */                                                                                                          \r\n";
                SQL += "  'HIC' GBN,a.Ptno Pano,b.SName                                                                                             \r\n";
                SQL += "  ,DECODE(a.GjJong,'83','TO','HR') DeptCode                                                                                 \r\n";
                SQL += "  ,DECODE(a.GjJong,'83','7102','7101') DrCode,DECODE(a.GjJong,'83','종검','건진') DrName                                    \r\n";
                SQL += "  ,DECODE(a.GbRead ,'1', 'Chest PA(AP)','2','Chest-dust', d.HName ) XName                                                   \r\n";
                SQL += "  ,0 ExInfo,a.XrayNo PacsNo,a.XCode XCode,'O' IpdOpd,0 OrderNo                                                              \r\n";
                SQL += "  ,a.GbPacs PacsStudyID,DECODE(a.GbRead ,'1', 'Chest PA(AP)','2','Chest-dust', d.HName ) OrderName                          \r\n";
                SQL += "  ,'' Remark,'1' XJong,b.Sex                                                                                                \r\n";
                SQL += "  ,'1' GBEND,0 DrWrtno, a.ROWID                                                                                             \r\n";
                SQL += "  ,TO_CHAR(a.JepDate,'YYYYMMDD HH24:MI') SeekDate                                                                           \r\n";
                SQL += "  ,TO_CHAR(a.JepDate,'YYYYMMDD HH24:MI') JDate                                                                              \r\n";
                SQL += "  ,'' DRDATE                                                                                                                \r\n";
                SQL += "  ,'' OrderDate                                                                                                             \r\n";
                SQL += "  ,'' XSendDate                                                                                                             \r\n";
                SQL += "  ,TO_CHAR(a.ReadTime1,'YYYY-MM-DD') ResultDate                                                                             \r\n";

                //2018-08-06 안정수, 상단에 병동 및 호실 칼럼 추가하면서 추가함 
                SQL += "  ,''                                                                                                                  \r\n";

                SQL += "  ,KOSMOS_OCS.FC_GET_AGE(TO_CHAR(b.Birth,'YYYY-MM-DD'),a.JepDate) FC_age                                                    \r\n"; //나이체크
                SQL += "  ,'' FC_XJong2                                                                                                             \r\n"; //종류체크
                //SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Ptno,a.JepDate) FC_infect                                                         \r\n"; //감염체크
                SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Ptno,a.JepDate) FC_infect_EX                                                         \r\n"; //감염체크
                SQL += "   FROM " + ComNum.DB_PMPA + "HIC_XRAY_RESULT a                                                                             \r\n";
                SQL += "       ," + ComNum.DB_PMPA + "BAS_PATIENT b                                                                                 \r\n";           
                SQL += "       ," + ComNum.DB_PMPA + "HIC_EXCODE d                                                                                  \r\n";
                SQL += "  WHERE 1 = 1                                                                                                               \r\n";
                SQL += "   AND 1 = '" + argCls.GubunHic + "'                                                                                        \r\n";
                SQL += "   AND a.JepDate >=TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                             \r\n";
                SQL += "   AND a.JepDate < TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                                                             \r\n";
                SQL += "   AND a.Ptno=b.Pano(+)                                                                                                     \r\n";         
                SQL += "   AND a.XCode=d.Code(+)                                                                                                    \r\n";
                SQL += "   AND a.DelDate IS NULL                                                                                                    \r\n";
                if (argCls.GbJob == "1") //등록번호별
                {
                    SQL += "   AND a.Ptno ='" + argCls.Pano + "'                                                                                    \r\n";
                }
                else if (argCls.GbJob == "2") //당일 외래접수자
                {
                    #region //당일 외래접수자
                    SQL += "   AND a.Ptno IN ( SELECT Pano                                                                                          \r\n";
                    SQL += "                    FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                               \r\n";
                    SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                    SQL += "                      AND ActDate =TRUNC(SYSDATE)                                                                       \r\n";
                    if (argCls.DeptCode != "**")
                    {
                        SQL += "                  AND DeptCode ='" + argCls.DeptCode + "'                                                           \r\n";
                        if (argCls.DrCode != "****" && argCls.DrCode != "")
                        {
                            SQL += "              AND DrCode ='" + argCls.DrCode + "'                                                               \r\n";

                        }
                    }
                    SQL += "                  GROUP BY Pano                                                                                         \r\n";
                    SQL += "                  )                                                                                                     \r\n";
                    #endregion
                }
                else if (argCls.GbJob == "3") //재원자
                {
                    #region //재원자
                    SQL += "   AND a.Ptno IN ( SELECT Pano                                                                                          \r\n";
                    SQL += "                    FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                                           \r\n";
                    SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                    SQL += "                      AND (OutDate IS NULL OR OutDate>=TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD'))                    \r\n";
                    SQL += "                      AND IpwonTime <  TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                                     \r\n";
                    SQL += "                      AND Amset4 <> '3'                                                                                 \r\n";
                    SQL += "                      AND Pano <   '90000000'                                                                           \r\n";
                    SQL += "                      AND Pano <>  '81000004'                                                                           \r\n";
                    if (argCls.WardCode != "**")
                    {
                        SQL += "                  AND WardCode ='" + argCls.WardCode + "'                                                           \r\n";
                    }
                    if (argCls.DeptCode != "**")
                    {
                        SQL += "                  AND DeptCode ='" + argCls.DeptCode + "'                                                           \r\n";
                        if (argCls.DrCode != "****" && argCls.DrCode != "")
                        {
                            SQL += "              AND DrCode ='" + argCls.DrCode + "'                                                               \r\n";

                        }
                    }
                    SQL += "                  GROUP BY Pano                                                                                         \r\n";
                    SQL += "                  )                                                                                                     \r\n";
                    #endregion
                }
                else if (argCls.GbJob == "5") //응급실 재원자
                {
                    #region //응급실 재원자
                    SQL += "   AND a.Ptno IN ( SELECT Pano                                                                                          \r\n";
                    SQL += "                    FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                               \r\n";
                    SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                    SQL += "                      AND ActDate >=TRUNC(SYSDATE-1)                                                                    \r\n";
                    SQL += "                      AND DeptCode ='ER'                                                                                \r\n";
                    SQL += "                      AND OCSJIN = '*'                                                                                  \r\n";
                    SQL += "                  GROUP BY Pano                                                                                         \r\n";
                    SQL += "                  )                                                                                                     \r\n";
                    #endregion
                }
                else if (argCls.GbJob == "6") //당일입원자
                {
                    #region //당일입원자
                    SQL += "   AND a.Ptno IN ( SELECT Pano                                                                                          \r\n";
                    SQL += "                    FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                                           \r\n";
                    SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                    SQL += "                      AND InDate>=TO_DATE('" + Convert.ToDateTime(argCls.Date2).AddDays(-2).ToShortDateString() + "','YYYY-MM-DD')          \r\n";
                    SQL += "                      AND IpwonTime >= TO_DATE('" + Convert.ToDateTime(argCls.Date2).AddDays(-1).ToShortDateString() + "','YYYY-MM-DD')     \r\n";
                    SQL += "                      AND IpwonTime <  TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                                     \r\n";
                    SQL += "                      AND Amset4 <> '3'                                                                                 \r\n";
                    SQL += "                      AND Pano <   '90000000'                                                                           \r\n";
                    SQL += "                      AND Pano <>  '81000004'                                                                           \r\n";
                    if (argCls.DeptCode != "**")
                    {
                        SQL += "                  AND DeptCode ='" + argCls.DeptCode + "'                                                           \r\n";
                        if (argCls.DrCode != "****" && argCls.DrCode != "")
                        {
                            SQL += "              AND DrCode ='" + argCls.DrCode + "'                                                               \r\n";

                        }
                    }
                    SQL += "                  GROUP BY Pano                                                                                         \r\n";
                    SQL += "                  )                                                                                                     \r\n";
                    #endregion
                }
                else if (argCls.GbJob == "7") //종합검진
                {
                    SQL += "   AND a.PTno ='zz'                                                                                                     \r\n";
                }
                else if (argCls.GbJob == "8") //일반검진
                {
                    SQL += "   AND a.PTno >' '                                                                                                      \r\n";
                }
                else if (argCls.GbJob == "9") //당일촬영자
                {
                    #region //당일촬영자
                    if (argCls.WardCode != "**")
                    {
                        SQL += "   AND WardCode ='" + argCls.WardCode + "'                                                           \r\n";
                    }
                    if (argCls.DeptCode != "**")
                    {
                        SQL += "   AND DeptCode ='" + argCls.DeptCode + "'                                                                          \r\n";
                        if (argCls.DrCode != "****" && argCls.DrCode != "")
                        {
                            SQL += "   AND DrCode ='" + argCls.DrCode + "'                                                                          \r\n";
                        }
                    }
                    if (argCls.DeptCode !="**" || argCls.DeptCode != "HR")
                    {
                        SQL += "   AND a.PTno ='zz'                                                                                                 \r\n";
                    }
                    #endregion
                }
                else if (argCls.GbJob == "A") //영상누락자
                {
                    #region //영상누락자
                    SQL += "   AND a.XRayNo IS NULL                                                                                            \r\n";
                    #endregion
                }
                else if (argCls.GbJob == "B") //초음파검사자
                {
                    #region //초음파검사자
                    SQL += "   AND a.Ptno IN ( SELECT Pano                                                                                          \r\n";
                    SQL += "                    FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                              \r\n";
                    SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                    SQL += "                      AND SeekDate >=TRUNC(SYSDATE)                                                                     \r\n";
                    SQL += "                      AND SeekDate < TRUNC(SYSDATE+1)                                                                   \r\n";
                    SQL += "                      AND XJong ='3'                                                                                    \r\n";
                    SQL += "                  GROUP BY Pano                                                                                         \r\n";
                    SQL += "                  )                                                                                                     \r\n";
                    #endregion
                }
                else if (argCls.GbJob == "C") //수술예약자
                {
                    #region //수술예약자
                    SQL += "   AND a.Ptno IN ( SELECT Pano                                                                                          \r\n";
                    SQL += "                    FROM " + ComNum.DB_MED + "OCS_OPSCHE                                                                \r\n";
                    SQL += "                     WHERE 1 = 1                                                                                        \r\n";
                    SQL += "                      AND OpDate =TRUNC(SYSDATE)                                                                        \r\n";
                    SQL += "                      AND (GbDel <> '*' OR GbDel IS NULL)                                                               \r\n";
                    if (argCls.DeptCode != "**")
                    {
                        SQL += "                  AND DeptCode ='" + argCls.DeptCode + "'                                                           \r\n";
                    }
                    SQL += "                  GROUP BY Pano                                                                                         \r\n";
                    SQL += "                  )                                                                                                     \r\n";
                    #endregion
                }
                if (argCls.NRead == "Y")
                {
                    SQL += "   AND a.ReadTime1 IS NULL                                                                                              \r\n";
                }
            }
            #endregion            

            //2018-07-23 안정수, 정태란s 요청으로 촬영일자별로 조회시, 촬영일자 순으로 정렬되도록.. 
            if (argCls.GbJob == "9")
            {
                SQL += "   ORDER BY SeekDate DESC, PANO                                                                                         \r\n";
            }
            //2019-12-23 안정수, 정희정g 요청으로 등록번호별로 조회시, 등록번호 순으로 정렬되도록.. 
            else if (argCls.GbJob == "1")
            {
                SQL += "   ORDER BY PANO, SEEKDATE DESC                                                                                         \r\n";
            }

            else
            {
                SQL += "   ORDER BY 3,2                                                                                                         \r\n"; 
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

        public class cHic_Xray_Result
        {
            public string Job = "";
            public string Ptno = "";
            public string JepDate = "";
            public string GbPacs = "";
            public string GbViewGbn = "";
            public string ROWID = "";
        }

        public DataTable sel_Hic_Xray_Result(PsmhDb pDbCon, cHic_Xray_Result argCls, bool bLog)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                                     \r\n";
            SQL += " XrayNo PacsNo,Pano,PTno,GbConv                                                                                             \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "HIC_XRAY_RESULT                                                                               \r\n";
            SQL += "  WHERE 1 = 1                                                                                                               \r\n";
            if (argCls.Job =="01")
            {
                if (argCls.GbViewGbn =="01")
                {
                    SQL += "   AND JepDate >=TO_DATE('" + argCls.JepDate + "','YYYY-MM-DD')                                                     \r\n";
                    SQL += "   AND JepDate <= TO_DATE('" + argCls.JepDate + "','YYYY-MM-DD')                                                    \r\n";
                }
                
                SQL += "   AND GbPacs = '" + argCls.GbPacs + "'                                                                                 \r\n";
                
            }
            else if (argCls.Job == "02")
            {                                 
                SQL += "   AND ROWID = '" + argCls.ROWID + "'                                                                                  \r\n";
            }
            else
            {
                SQL += "   AND Ptno ='" + argCls.Ptno + "'                                                                                      \r\n";
            }

            if (argCls.Job == "01")
            {
                SQL += "   ORDER BY JepDate DESC                                                                                                \r\n";
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

        public DataTable sel_Xray_Detail_Name(PsmhDb pDbCon, string argPano, bool bLog)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                                     \r\n";
            SQL += "    SNAME                                                                                                                   \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                                                   \r\n";
            SQL += "  WHERE 1 = 1                                                                                                               \r\n";
            SQL += "   AND PANO = '" + argPano + "'                                                                                             \r\n";
            SQL += "UNION ALL                                                                                                                   \r\n";
            SQL += "SELECT                                                                                                                      \r\n";
            SQL += "   SNAME                                                                                                                    \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "HIC_XRAY_RESULT                                                                               \r\n";
            SQL += "  WHERE 1 = 1                                                                                                               \r\n";
            SQL += "   AND PTNO = '" + argPano + "'                                                                                             \r\n";        

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

        public DataTable sel_Patient_Name(PsmhDb pDbCon, string argPano, bool bLog)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                                     \r\n";
            SQL += "    B.SNAME                                                                                                                 \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_JUPMST A, " + ComNum.DB_PMPA + "BAS_PATIENT B                                             \r\n";
            SQL += "  WHERE 1 = 1                                                                                                               \r\n";
            SQL += "   AND B.PANO  = '" + argPano + "'                                                                                          \r\n";
            SQL += "   AND A.PTNO = B.PANO                                                                                                      \r\n";

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

        public DataTable sel_Xray_Detail_SeekDate(PsmhDb pDbCon, string argPano, string argJepDate, bool bLog)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                                  \r\n";
            SQL += "    TO_CHAR(A.SEEKDATE, 'YYYY-MM-DD') AS SEEKDATE                                                                        \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL A                                                                              \r\n";
            SQL += "  WHERE 1 = 1                                                                                                            \r\n";
            SQL += "   AND A.PANO  = '" + argPano + "'                                                                                       \r\n";
            SQL += "   AND A.GBRESERVED = '7'                                                                                                \r\n";
            SQL += "   AND A.XJONG = '4'                                                                                                     \r\n";
            SQL += "   AND A.PACSSTUDYID IS NOT NULL                                                                                         \r\n";
            SQL += "   AND A.GBEND = '1'                                                                                                     \r\n";
            SQL += "   AND A.SEEKDATE < TO_DATE('" + argJepDate + "', 'YYYY-MM-DD')                                                          \r\n";
            SQL += "   AND A.XCODE IN (                                                                                                      \r\n";
            SQL += "                        SELECT XCODE                                                                                     \r\n";
            SQL += "                        FROM " + ComNum.DB_PMPA + "XRAY_CODE                                                             \r\n";
            SQL += "                        WHERE 1=1                                                                                        \r\n";
            SQL += "                        AND XNAME LIKE '%CT Chest%')                                                                     \r\n";            
            SQL += "ORDER BY SEEKDATE DESC                                                                                                   \r\n";

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



        #region 트랜잭션 쿼리 + INSERT, UPDATE,DELETE .... 

        /// <summary>
        /// 영상의학과 기초코드 저장,갱신.. 사용
        /// </summary>
        public enum enum_XrayCode { XCode, XName, Class, SubClass, OptBun, Res, Pacs, Cnt1, Cnt2, Buse, Remark1, Remark2, Remark3, Remark4, DelDate, ROWID }

        /// <summary>
        /// 영상의학과 기초코드 대분류 저장,갱신.. 사용
        /// </summary>
        public enum enum_XrayCode2 { ClassCode, ClassName, ROWID }

        /// <summary>
        /// 영상의학과 기초코드 소분류 저장,갱신.. 사용
        /// </summary>
        public enum enum_XrayCode3 { SubCode, SubName, ClassCode, ClassName, ROWID }

        /// <summary>
        /// 영상의학과 기초코드 재료코드 저장,갱신.. 사용
        /// </summary>
        public enum enum_XrayMCode { MCdoe, MName, GbMCode, Qty, Unit, JepCode, PrintRanking, ROWID }

        /// <summary>
        /// 영상의학과 기초코드 기본사용량 저장,갱신.. 사용
        /// </summary>
        public enum enum_XrayBasUse { Change, XCode, Gubun2, Gubun2Name, MCode, MName, Qty, Agree, ROWID }


        /// <summary>
        /// 영상의학과 기초코드 관련
        /// </summary>
        /// <param name="argCode"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string del_Xray_Code(PsmhDb pDbCon, string argCode, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_PMPA + "XRAY_CODE    \r\n"; 
            SQL += "  WHERE 1=1                                     \r\n";
            SQL += "    AND XCode = '" + argCode + "'               \r\n";
            
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_Xray_Code(PsmhDb pDbCon, string[] arg, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_CODE  SET                                    \r\n";

            SQL += "    XCode = '" + arg[(int)enum_XrayCode.XCode] + "'                             \r\n";
            SQL += "   ,XName = '" + arg[(int)enum_XrayCode.XName] + "'                             \r\n";
            SQL += "   ,ClassCode = '" + VB.Left(arg[(int)enum_XrayCode.Class],2)+ "'                         \r\n";
            SQL += "   ,SubCode = '" + VB.Left(arg[(int)enum_XrayCode.SubClass],2) + "'                        \r\n";
            SQL += "   ,Exinfo = ''                                                                 \r\n";
            SQL += "   ,GbReserved = '" + arg[(int)enum_XrayCode.Res] + "'                          \r\n";
            SQL += "   ,BCnt = '" + arg[(int)enum_XrayCode.Cnt1] + "'                               \r\n";
            SQL += "   ,CCnt = '" + arg[(int)enum_XrayCode.Cnt2] + "'                               \r\n";
            SQL += "   ,GbPacs = '" + arg[(int)enum_XrayCode.Pacs] + "'                             \r\n";
            SQL += "   ,BUCODE = '" + arg[(int)enum_XrayCode.Buse] + "'                             \r\n";
            SQL += "   ,GBDATE = '" + arg[(int)enum_XrayCode.OptBun] + "'                           \r\n";
            SQL += "   ,DelDate = TO_DATE('" + arg[(int)enum_XrayCode.DelDate] + "','YYYY-MM-DD')   \r\n";
            SQL += "   ,REMARK1 = '" + arg[(int)enum_XrayCode.Remark1] + "'                         \r\n";
            SQL += "   ,REMARK2 = '" + arg[(int)enum_XrayCode.Remark2] + "'                         \r\n";
            SQL += "   ,REMARK3 = '" + arg[(int)enum_XrayCode.Remark3] + "'                         \r\n";
            SQL += "   ,REMARK4 = '" + arg[(int)enum_XrayCode.Remark4] + "'                         \r\n";
            
            SQL += "  WHERE 1=1                                                                     \r\n";
            SQL += "    AND ROWID = '" + arg[(int)enum_XrayCode.ROWID] + "'                         \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_Xray_Code(PsmhDb pDbCon, string[] arg, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_CODE                                    \r\n";
            SQL += "  (XCode, XName, ClassCode, SubCode, Exinfo,                                    \r\n";
            SQL += "   GbReserved, BCnt, CCnt,GbPacs, BUCODE,                                       \r\n";
            SQL += "   REMARK1, REMARK2, REMARK3,REMARK4, GBDATE)  VALUES                           \r\n";
            SQL += "   (                                                                            \r\n";
            SQL += "  '" + arg[(int)enum_XrayCode.XCode] + "'                                       \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayCode.XName] + "'                                      \r\n";
            SQL += "  ,'" + clsComSup.setP(arg[(int)enum_XrayCode.Class],".",1).Trim() + "'         \r\n";
            SQL += "  ,'" + clsComSup.setP(arg[(int)enum_XrayCode.SubClass], ".", 1).Trim() + "'    \r\n";
            SQL += "  ,''                                                                           \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayCode.Res] + "'                                        \r\n";
            SQL += "  ," + Convert.ToInt16(arg[(int)enum_XrayCode.Cnt1]) + "                        \r\n";
            SQL += "  ," + Convert.ToInt16(arg[(int)enum_XrayCode.Cnt2]) + "                        \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayCode.Pacs] + "'                                       \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayCode.Buse] + "'                                       \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayCode.Remark1] + "'                                    \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayCode.Remark2] + "'                                    \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayCode.Remark3] + "'                                    \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayCode.Remark4] + "'                                    \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayCode.OptBun] + "'                                     \r\n";
            SQL += "   )                                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 영상의학과 기초코드 대분류 관련
        /// </summary>
        /// <param name="argCode"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string del_Xray_Class(PsmhDb pDbCon, string argCode, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_PMPA + "XRAY_CLASS   \r\n";
            SQL += "  WHERE 1=1                                     \r\n";
            SQL += "    AND ClassCode = '" + argCode + "'           \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_Xray_Class(PsmhDb pDbCon, string[] arg, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_CLASS  SET                                   \r\n";
            SQL += "    ClassCode = '" + arg[(int)enum_XrayCode2.ClassCode] + "'                    \r\n";
            SQL += "   ,ClassName = '" + arg[(int)enum_XrayCode2.ClassName] + "'                    \r\n"; 
            SQL += "  WHERE 1=1                                                                     \r\n";
            SQL += "    AND ROWID = '" + arg[(int)enum_XrayCode2.ROWID] + "'                         \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_Xray_Class(PsmhDb pDbCon, string[] arg, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_CLASS                                   \r\n";
            SQL += "  (ClassCode, ClassName)  VALUES                                                \r\n";
            SQL += "   (                                                                            \r\n";
            SQL += "  '" + arg[(int)enum_XrayCode2.ClassCode] + "'                                  \r\n";         
            SQL += "  ,'" + arg[(int)enum_XrayCode2.ClassName] + "'                                 \r\n";            
            SQL += "   )                                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 영상의학과 기초코드 소분류 관련
        /// </summary>
        /// <param name="argCode"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string del_Xray_SubClass(PsmhDb pDbCon, string argClass,string argSuCode, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_PMPA + "XRAY_SubCLASS                        \r\n";
            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "    AND ClassCode = '" + argClass + "'                                  \r\n";
            SQL += "    AND SUBCODE = '" + argSuCode + "'                                   \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_Xray_SubClass(PsmhDb pDbCon, string[] arg, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_SubCLASS  SET                            \r\n";
            SQL += "    ClassCode = '" + arg[(int)enum_XrayCode3.ClassCode] + "'                \r\n";
            SQL += "   ,SUBCODE = '" + arg[(int)enum_XrayCode3.SubCode] + "'                    \r\n";
            SQL += "   ,SUBNAME = '" + arg[(int)enum_XrayCode3.SubName] + "'                    \r\n";
            SQL += "  WHERE 1=1                                                                 \r\n";
            SQL += "    AND ROWID = '" + arg[(int)enum_XrayCode3.ROWID] + "'                    \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_Xray_SubClass(PsmhDb pDbCon, string[] arg, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_SubCLASS                                \r\n";
            SQL += "  (ClassCode, SubCode, SubName)  VALUES                                         \r\n";
            SQL += "   (                                                                            \r\n";
            SQL += "  '" + arg[(int)enum_XrayCode3.ClassCode] + "'                                  \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayCode3.SubCode] + "'                                   \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayCode3.SubName] + "'                                   \r\n";
            SQL += "   )                                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 영상의학과 기초코드 재료코드 관련
        /// </summary>
        /// <param name="argCode"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string del_Xray_MCode(PsmhDb pDbCon, string argMCode, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_PMPA + "XRAY_MCODE                       \r\n";
            SQL += "  WHERE 1=1                                                         \r\n";
            SQL += "    AND MCode = '" + argMCode + "'                                  \r\n";            

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_Xray_MCode(PsmhDb pDbCon, string[] arg, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_MCODE  SET                           \r\n";
            SQL += "    MCode = '" + arg[(int)enum_XrayMCode.MCdoe] + "'                    \r\n";
            SQL += "   ,MName = '" + arg[(int)enum_XrayMCode.MName] + "'                    \r\n";
            SQL += "   ,GbMCode = '" + arg[(int)enum_XrayMCode.GbMCode] + "'                \r\n";
            SQL += "   ,Unit = '" + arg[(int)enum_XrayMCode.Unit] + "'                      \r\n";
            SQL += "   ,JepCode = '" + arg[(int)enum_XrayMCode.JepCode] + "'                \r\n";
            SQL += "   ,PrintRanking = '" + arg[(int)enum_XrayMCode.PrintRanking] + "'      \r\n";
            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "    AND ROWID = '" + arg[(int)enum_XrayMCode.ROWID] + "'                \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_Xray_MCode(PsmhDb pDbCon, string[] arg, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_MCODE                           \r\n";
            SQL += "  (MCode, MName, GbMCode, Qty, Unit, Jepcode,PrintRanking)  VALUES      \r\n";
            SQL += "   (                                                                    \r\n";
            SQL += "  '" + arg[(int)enum_XrayMCode.MCdoe] + "'                              \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayMCode.MName] + "'                             \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayMCode.GbMCode] + "'                           \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayMCode.Qty] + "'                               \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayMCode.Unit] + "'                              \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayMCode.JepCode] + "'                           \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayMCode.PrintRanking] + "'                      \r\n";            
            SQL += "   )                                                                    \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 영상의학과 기초코드 기본사용량  관련
        /// </summary>
        /// <param name="argCode"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string del_Xray_BasUse(PsmhDb pDbCon, string[] arg, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_PMPA + "XRAY_BASUSE                          \r\n";
            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "    AND ROWID = '" + arg[(int)enum_XrayBasUse.ROWID] + "'               \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_Xray_BasUse(PsmhDb pDbCon, string[] arg, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_BASUSE  SET                          \r\n";
            SQL += "    GbXcode2 = '" + arg[(int)enum_XrayBasUse.Gubun2] + "'               \r\n";
            SQL += "   ,MCode = '" + arg[(int)enum_XrayBasUse.MCode] + "'                   \r\n";
            SQL += "   ,Qty = '" + arg[(int)enum_XrayBasUse.Qty] + "'                       \r\n";
            SQL += "   ,Agree = '" + arg[(int)enum_XrayBasUse.Agree] + "'                   \r\n";           
            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "    AND ROWID = '" + arg[(int)enum_XrayBasUse.ROWID] + "'               \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_Xray_BasUse(PsmhDb pDbCon, string[] arg, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_BASUSE                          \r\n";
            SQL += "  (XCODE,GbXcode1,GbXCode2,Mcode,Qty,Agree)  VALUES                     \r\n";
            SQL += "   (                                                                    \r\n";
            SQL += "  '" + arg[(int)enum_XrayBasUse.XCode] + "'                             \r\n";            
            SQL += "  ,'0'                                                                  \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayBasUse.Gubun2] + "'                            \r\n";            
            SQL += "  ,'" + arg[(int)enum_XrayBasUse.MCode] + "'                            \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayBasUse.Qty] + "'                              \r\n";
            SQL += "  ,'" + arg[(int)enum_XrayBasUse.Agree] + "'                            \r\n";            
            SQL += "   )                                                                    \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string del_Xray_Use(PsmhDb pDbCon, long argMgrno, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_PMPA + "XRAY_USE                         \r\n";
            SQL += "  WHERE 1=1                                                         \r\n";
            SQL += "    AND MgrNo = '" + argMgrno + "'                                  \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_Hic_Xray_Result(PsmhDb pDbCon, clsComSupXrayRead.cHic_Xray_Result argC, ref int intRowAffected)
        {
            string SqlErr = string.Empty;            

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "Hic_Xray_Result  SET          \r\n";
            SQL += "    GBSTS = '2'                                             \r\n";  //판독여부
            SQL += "   ,RESULT4 = '" + argC.strResult1 + "'                     \r\n";  //결과1  
            SQL += "   ,CTDOSE = '" + argC.NODUCTDOSE + "'                      \r\n";  //CT선량 
            SQL += "   ,PASTCT = '" + argC.PASTCT + "'                          \r\n";  //이전CT 유무 
            SQL += "   ,PASTCTYYYY = '" + argC.PASTCTYYYY + "'                  \r\n";  //이전CT 년도
            SQL += "   ,PASTCTMM = '" + argC.PASTCTMM + "'                      \r\n";  //이전CT 월 
            SQL += "   ,READDOCT1 = '" + argC.READDOCT1 + "'                    \r\n";
            SQL += "   ,READTIME1 = '" + argC.READTIME1 + "'                    \r\n";

            SQL += "   ,NUDOYN_1 = '" + argC.NUDOYN_1 + "'                      \r\n";  //양성결절 유무, 1.무 2.유 3.석회화 또는 지방포함결절
            SQL += "   ,NUDOICON_1 = '" + argC.NUDOICON_1 + "'                  \r\n";  //결절성상 1.고형 2.부분고형 3.간유리(비고형)
            SQL += "   ,NUDOSITE_1 = '" + argC.NUDOSITE_1 + "'                  \r\n";  //결절위치 1.우상엽 2.우중엽 3.우하엽 4.좌상엽 5.좌하엽
            SQL += "   ,NUDOSIZE1_1 = '" + argC.NUDOSIZE1_1 + "'                \r\n";  //결절크기 평균
            SQL += "   ,NUDOSIZE2_1	= '" + argC.NUDOSIZE2_1 + "'                \r\n";  //고형크기평균
            SQL += "   ,NUDOPOSITIVE_1 = '" + argC.NUDOPOSITIVE_1 + "'          \r\n";  //1.폐암 시사소견, 2.양성결절 시사소견(2b), 3.해당없음
            SQL += "   ,NUDO4X_1 = '" + argC.NUDO4X_1 + "'                      \r\n";  //4X 종류
            SQL += "   ,NUDO4XETC_1 = '" + argC.NUDO4XETC_1 + "'                \r\n";  //4X 기타의견
            SQL += "   ,NUDOTRACECHK_1 = '" + argC.NUDOTRACECHK_1 + "'          \r\n";  //추척검사  1.변화없음 2.변화있음 3.해당없음
            SQL += "   ,NUDOTRACECHK2_1 = '" + argC.NUDOTRACECHK2_1 + "'        \r\n";  //추적검사  1.새로생김 2.커짐

            SQL += "   ,NUDOYN_2 = '" + argC.NUDOYN_2 + "'                      \r\n";
            SQL += "   ,NUDOICON_2 = '" + argC.NUDOICON_2 + "'                  \r\n";
            SQL += "   ,NUDOSITE_2 = '" + argC.NUDOSITE_2 + "'                  \r\n";
            SQL += "   ,NUDOSIZE1_2 = '" + argC.NUDOSIZE1_2 + "'                \r\n";
            SQL += "   ,NUDOSIZE2_2	= '" + argC.NUDOSIZE2_2 + "'                \r\n";
            SQL += "   ,NUDOPOSITIVE_2 = '" + argC.NUDOPOSITIVE_2 + "'          \r\n";
            SQL += "   ,NUDO4X_2 = '" + argC.NUDO4X_2 + "'                      \r\n";    
            SQL += "   ,NUDO4XETC_2 = '" + argC.NUDO4XETC_2 + "'                \r\n"; 
            SQL += "   ,NUDOTRACECHK_2 = '" + argC.NUDOTRACECHK_2 + "'          \r\n";
            SQL += "   ,NUDOTRACECHK2_2 = '" + argC.NUDOTRACECHK2_2 + "'        \r\n";

            SQL += "   ,NUDOYN_3 = '" + argC.NUDOYN_3 + "'                      \r\n";
            SQL += "   ,NUDOICON_3 = '" + argC.NUDOICON_3 + "'                  \r\n";
            SQL += "   ,NUDOSITE_3 = '" + argC.NUDOSITE_3 + "'                  \r\n";
            SQL += "   ,NUDOSIZE1_3 = '" + argC.NUDOSIZE1_3 + "'                \r\n";
            SQL += "   ,NUDOSIZE2_3	= '" + argC.NUDOSIZE2_3 + "'                \r\n";
            SQL += "   ,NUDOPOSITIVE_3 = '" + argC.NUDOPOSITIVE_3 + "'          \r\n";
            SQL += "   ,NUDO4X_3 = '" + argC.NUDO4X_3 + "'                      \r\n"; 
            SQL += "   ,NUDO4XETC_3 = '" + argC.NUDO4XETC_3 + "'                \r\n"; 
            SQL += "   ,NUDOTRACECHK_3 = '" + argC.NUDOTRACECHK_3 + "'          \r\n";
            SQL += "   ,NUDOTRACECHK2_3 = '" + argC.NUDOTRACECHK2_3 + "'        \r\n";

            SQL += "   ,NUDOYN_4 = '" + argC.NUDOYN_4 + "'                      \r\n";
            SQL += "   ,NUDOICON_4 = '" + argC.NUDOICON_4 + "'                  \r\n";
            SQL += "   ,NUDOSITE_4 = '" + argC.NUDOSITE_4 + "'                  \r\n";
            SQL += "   ,NUDOSIZE1_4 = '" + argC.NUDOSIZE1_4 + "'                \r\n";
            SQL += "   ,NUDOSIZE2_4	= '" + argC.NUDOSIZE2_4 + "'                \r\n";
            SQL += "   ,NUDOPOSITIVE_4 = '" + argC.NUDOPOSITIVE_4 + "'          \r\n";
            SQL += "   ,NUDO4X_4 = '" + argC.NUDO4X_4 + "'                      \r\n"; 
            SQL += "   ,NUDO4XETC_4 = '" + argC.NUDO4XETC_4 + "'                \r\n";   
            SQL += "   ,NUDOTRACECHK_4 = '" + argC.NUDOTRACECHK_4 + "'          \r\n";
            SQL += "   ,NUDOTRACECHK2_4 = '" + argC.NUDOTRACECHK2_4 + "'        \r\n";

            SQL += "   ,NUDOYN_5 = '" + argC.NUDOYN_5 + "'                      \r\n";
            SQL += "   ,NUDOICON_5 = '" + argC.NUDOICON_5 + "'                  \r\n";
            SQL += "   ,NUDOSITE_5 = '" + argC.NUDOSITE_5 + "'                  \r\n";
            SQL += "   ,NUDOSIZE1_5 = '" + argC.NUDOSIZE1_5 + "'                \r\n";
            SQL += "   ,NUDOSIZE2_5	= '" + argC.NUDOSIZE2_5 + "'                \r\n";
            SQL += "   ,NUDOPOSITIVE_5 = '" + argC.NUDOPOSITIVE_5 + "'          \r\n";
            SQL += "   ,NUDO4X_5 = '" + argC.NUDO4X_5 + "'                      \r\n"; 
            SQL += "   ,NUDO4XETC_5 = '" + argC.NUDO4XETC_5 + "'                \r\n";  
            SQL += "   ,NUDOTRACECHK_5 = '" + argC.NUDOTRACECHK_5 + "'          \r\n";
            SQL += "   ,NUDOTRACECHK2_5 = '" + argC.NUDOTRACECHK2_5 + "'        \r\n";

            SQL += "   ,NUDOYN_6 = '" + argC.NUDOYN_6 + "'                      \r\n";
            SQL += "   ,NUDOICON_6 = '" + argC.NUDOICON_6 + "'                  \r\n";
            SQL += "   ,NUDOSITE_6 = '" + argC.NUDOSITE_6 + "'                  \r\n";
            SQL += "   ,NUDOSIZE1_6 = '" + argC.NUDOSIZE1_6 + "'                \r\n";
            SQL += "   ,NUDOSIZE2_6	= '" + argC.NUDOSIZE2_6 + "'                \r\n";
            SQL += "   ,NUDOPOSITIVE_6 = '" + argC.NUDOPOSITIVE_6 + "'          \r\n";
            SQL += "   ,NUDO4X_6 = '" + argC.NUDO4X_6 + "'                      \r\n";   
            SQL += "   ,NUDO4XETC_6 = '" + argC.NUDO4XETC_6 + "'                \r\n";
            SQL += "   ,NUDOTRACECHK_6 = '" + argC.NUDOTRACECHK_6 + "'          \r\n";
            SQL += "   ,NUDOTRACECHK2_6 = '" + argC.NUDOTRACECHK2_6 + "'        \r\n";

            SQL += "   ,INDICATIOCHK = '" + argC.INDICATIOCHK + "'              \r\n";  //기관지내 병변 1.없음 2.있음
            SQL += "   ,INDICATIOETC = '" + argC.INDICATIOETC + "'              \r\n";  

            SQL += "   ,SISACHK = '" + argC.SISACHK + "'                        \r\n";  //폐암시사 소견
            SQL += "   ,SISAETC = '" + argC.SISAETC + "'                        \r\n"; 

            SQL += "   ,NUDOMEAN1 = '" + argC.NUDOMEAN1 + "'                    \r\n";  //의미있는 소견
            SQL += "   ,NUDOMEAN2 = '" + argC.NUDOMEAN2 + "'                    \r\n";
            SQL += "   ,NUDOMEAN2_1 = '" + argC.NUDOMEAN2_1 + "'                \r\n";
            SQL += "   ,NUDOMEAN3 = '" + argC.NUDOMEAN3 + "'                    \r\n";
            SQL += "   ,NUDOMEAN4 = '" + argC.NUDOMEAN4 + "'                    \r\n";
            SQL += "   ,NUDOMEAN5 = '" + argC.NUDOMEAN5 + "'                    \r\n";
            SQL += "   ,NUDOMEAN6 = '" + argC.NUDOMEAN6 + "'                    \r\n";
            SQL += "   ,NUDOMEAN7 = '" + argC.NUDOMEAN7 + "'                    \r\n";
            SQL += "   ,NUDOMEAN8 = '" + argC.NUDOMEAN8 + "'                    \r\n";
            SQL += "   ,NUDOMEAN9 = '" + argC.NUDOMEAN9 + "'                    \r\n";
            SQL += "   ,NUDOMEAN9_9 = '" + argC.NUDOMEAN9_9 + "'                \r\n";

            SQL += "   ,NUDOUNACTIVE = '" + argC.NUDOUNACTIVE + "'              \r\n";  //비활동성 폐결핵

            SQL += "   ,NUDOPANGU = '" + argC.NUDOPANGU + "'                    \r\n"; 
            SQL += "   ,NUDOPANGU2 = '" + argC.NUDOPANGU2 + "'                  \r\n";

            SQL += "   ,SIZE1_1 = '" + argC.SIZE1_1 + "'                        \r\n";  //고형 or 비고형 사이즈1
            SQL += "   ,SIZE1_2 = '" + argC.SIZE1_2 + "'                        \r\n";  //고형 or 비고형 사이즈2
            SQL += "   ,GOSIZE1_1 = '" + argC.GOSIZE1_1 + "'                    \r\n";  //부분고형 사이즈1
            SQL += "   ,GOSIZE1_2 = '" + argC.GOSIZE1_2 + "'                    \r\n";  //부분고형 사이즈2
            SQL += "   ,IMAGENO_1 = '" + argC.IMAGENO_1 + "'                    \r\n";  //영상번호
            SQL += "   ,CATEGORY1 = '" + argC.CATEGORY1 + "'                    \r\n";  //범주

            SQL += "   ,SIZE2_1 = '" + argC.SIZE2_1 + "'                        \r\n";
            SQL += "   ,SIZE2_2 = '" + argC.SIZE2_2 + "'                        \r\n";
            SQL += "   ,GOSIZE2_1 = '" + argC.GOSIZE2_1 + "'                    \r\n";
            SQL += "   ,GOSIZE2_2 = '" + argC.GOSIZE2_2 + "'                    \r\n";
            SQL += "   ,IMAGENO_2 = '" + argC.IMAGENO_2 + "'                    \r\n";
            SQL += "   ,CATEGORY2 = '" + argC.CATEGORY2 + "'                    \r\n";

            SQL += "   ,SIZE3_1 = '" + argC.SIZE3_1 + "'                        \r\n";
            SQL += "   ,SIZE3_2 = '" + argC.SIZE3_2 + "'                        \r\n";
            SQL += "   ,GOSIZE3_1 = '" + argC.GOSIZE3_1 + "'                    \r\n";
            SQL += "   ,GOSIZE3_2 = '" + argC.GOSIZE3_2 + "'                    \r\n";
            SQL += "   ,IMAGENO_3 = '" + argC.IMAGENO_3 + "'                    \r\n";
            SQL += "   ,CATEGORY3 = '" + argC.CATEGORY3 + "'                    \r\n";

            SQL += "   ,SIZE4_1 = '" + argC.SIZE4_1 + "'                        \r\n";
            SQL += "   ,SIZE4_2 = '" + argC.SIZE4_2 + "'                        \r\n";
            SQL += "   ,GOSIZE4_1 = '" + argC.GOSIZE4_1 + "'                    \r\n";
            SQL += "   ,GOSIZE4_2 = '" + argC.GOSIZE4_2 + "'                    \r\n";
            SQL += "   ,IMAGENO_4 = '" + argC.IMAGENO_4 + "'                    \r\n";
            SQL += "   ,CATEGORY4 = '" + argC.CATEGORY4 + "'                    \r\n";

            SQL += "   ,SIZE5_1 = '" + argC.SIZE5_1 + "'                        \r\n";
            SQL += "   ,SIZE5_2 = '" + argC.SIZE5_2 + "'                        \r\n";
            SQL += "   ,GOSIZE5_1 = '" + argC.GOSIZE5_1 + "'                    \r\n";
            SQL += "   ,GOSIZE5_2 = '" + argC.GOSIZE5_2 + "'                    \r\n";
            SQL += "   ,IMAGENO_5 = '" + argC.IMAGENO_5 + "'                    \r\n";
            SQL += "   ,CATEGORY5 = '" + argC.CATEGORY5 + "'                    \r\n";

            SQL += "   ,SIZE6_1 = '" + argC.SIZE6_1 + "'                        \r\n";
            SQL += "   ,SIZE6_2 = '" + argC.SIZE6_2 + "'                        \r\n";
            SQL += "   ,GOSIZE6_1 = '" + argC.GOSIZE6_1 + "'                    \r\n";
            SQL += "   ,GOSIZE6_2 = '" + argC.GOSIZE6_2 + "'                    \r\n";
            SQL += "   ,IMAGENO_6 = '" + argC.IMAGENO_6 + "'                    \r\n";
            SQL += "   ,CATEGORY6 = '" + argC.CATEGORY6 + "'                    \r\n";

            SQL += "   ,NUDOUNACTCHKETC = '" + argC.NUDOUNACTCHKETC + "'        \r\n";
            SQL += "   ,NUDOUNACTETCSOGEN = '" + argC.NUDOUNACTETCSOGEN + "'    \r\n";
            SQL += "   ,PASTCANCER = '" + argC.PASTCANCER + "'                  \r\n";
            SQL += "   ,NUDOMAXRESULT = '" + argC.NUDOMAXRESULT + "'            \r\n";

            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND ROWID = '" + argC.ROWID + "'                        \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 영상의학과 검사 메인 테이블 xray_detail
        /// </summary>
        public class cXrayDetail
        {
            public string Tab = ""; //1:접수명단 2:완료명단
            public string Job = "";
            public string Gubun = "";
            public string STS = "";
            public string sNames_Job = "";
            public string sNames_Name = "";
            public string Search = "";
            public string GbPort = "";
            public string GB_Manual = "";
            public string GbER = "";
            public string GbHR = "";
            public string GbNoExe = "";
            public string GbCVR = "";
            public string GbCVR_Gubun1 = "";
            public string GbCVR_Gubun2 = "";
            public string GbResv1 = "";  //조회시 사용          
            public string GbJusa = "";
            public string GbResvChk = ""; //조회시 사용
            public string GbASA = "";     //조회시 사용
            public string GbASA_Sel = "";//조회시 사용
            public string GbASA_Suga = "";//조회시 사용
            public string GbCT = "";     //조회시 사용
            public string GbAll = "";     //조회시 사용
            public string GbNotEndo = "";     //조회시 사용 - 내시경처방제외
            public string GbNotRi = "";     //조회시 사용   - RI검사제외
            public string GbViewGbn = "";
            public bool GbCombine = false;
            
            public string Date1 = "";
            public string Date2 = "";
            public string EnterDate ="";
            public string IPDOPD    ="";
            public string GbReserved="";
            public string SeekDate  ="";
            public string Pano      ="";
            public string SName     ="";
            public string Sex       ="";
            public int Age       =0;
            public string DeptCode  ="";
            public string DrCode    ="";
            public string DrName = "";
            public string WardCode = "";
            public string RoomCode = "";
            public string BedName = "";
            public int Exid     =0;
            public string XJong     ="";            
            public string XSubCode  ="";
            public string XCode     ="";
            public int QTY       =0;
            public string Remark    ="";
            public string DrRemark  ="";
            public string XrayRoom  ="";
            public string OrderCode ="";
            public string OrderName ="";
            public string BDate     ="";
            public string RDate     ="";
            public string GBSTS     ="";
            public string Sabun = "";
            public string GbInfo = "";
            public string GbNgt = "";
            public string Agree = "";
            public string PacsNo = "";
            public long MgrNo = 0;
            public string GbEND = "";
            public string Pacs_END = "";
            public string CDate = "";
            public string CSabun = "";
            public string CDGubun = "";
            public long WRTNO = 0;

            public string Jumin = "";

            public string PrtJusa = "";
            public string PrtMETFORMIN = "";
            public string PrtASA_Suga = "";
            public string PrtMsg = "";
            public string PrtErSTS = "";
            public string PrtYoil = "";
            public string PrtConst = "";
            public string PrtConst2 = "";

            public string ROWID = "";

            public string strGbnFall = "";
            public string strGbnInfect = "";

            public string JusaName = ""; //조영제 명을 담는다
            public string JusaCode = ""; //조영제 코드를 담는다

        }

        public string ins_Xray_Detail(PsmhDb pDbCon, cXrayDetail argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_DETAIL                                  \r\n";
            SQL += "  (PANO,ENTERDATE,IPDOPD,GBRESERVED,SEEKDATE,SNAME,SEX,AGE                      \r\n";
            SQL += "   ,DEPTCODE,DRCODE,Exid,XJONG,XSUBCODE,XCODE,QTY,REMARK,DrREMARK               \r\n";
            SQL += "   ,XRAYROOM,ORDERCODE,ORDERNAME,BDATE,RDATE,GBSTS,GB_MANUAL                    \r\n";
            SQL += "   ,GbNgt,AGREE,PacsNo,GbInfo)     VALUES                                       \r\n";
            SQL += "   (                                                                            \r\n";
            SQL += "  '" + argCls.Pano + "'                                                         \r\n";
            SQL += "  ,SYSDATE                                                                      \r\n";
            SQL += "  ,'" + argCls.IPDOPD + "'                                                      \r\n";
            SQL += "  ,'" + argCls.GbReserved + "'                                                  \r\n";
            SQL += "  ,TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD HH24:MI')                      \r\n";
            SQL += "  ,'" + argCls.SName + "'                                                       \r\n";
            SQL += "  ,'" + argCls.Sex + "'                                                         \r\n";
            SQL += "  ," + argCls.Age + "                                                           \r\n";
            SQL += "  ,'" + argCls.DeptCode + "'                                                    \r\n";
            SQL += "  ,'" + argCls.DrCode + "'                                                      \r\n";
            SQL += "  ,'" + argCls.Exid + "'                                                        \r\n";
            SQL += "  ,'" + argCls.XJong + "'                                                       \r\n";
            SQL += "  ,'" + argCls.XSubCode + "'                                                    \r\n";
            SQL += "  ,'" + argCls.XCode + "'                                                       \r\n";
            SQL += "  ," + argCls.QTY + "                                                           \r\n";
            SQL += "  ,'" + argCls.Remark + "'                                                      \r\n";
            SQL += "  ,'" + argCls.DrRemark + "'                                                    \r\n";
            SQL += "  ,'" + argCls.XrayRoom + "'                                                    \r\n";
            SQL += "  ,'" + argCls.OrderCode + "'                                                   \r\n";
            SQL += "  ,'" + argCls.OrderName + "'                                                   \r\n";
            SQL += "  ,TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                                 \r\n";
            SQL += "  ,TO_DATE('" + argCls.RDate + "','YYYY-MM-DD HH24:MI')                         \r\n";
            SQL += "  ,'" + argCls.GBSTS + "'                                                       \r\n";
            SQL += "  ,'" + argCls.GB_Manual + "'                                                   \r\n";
            SQL += "  ,'" + argCls.GbNgt + "'                                                       \r\n";
            SQL += "  ,'" + argCls.Agree + "'                                                       \r\n";
            SQL += "  ,'" + argCls.PacsNo + "'                                                      \r\n";
            SQL += "  ,'" + argCls.GbInfo + "'                                                      \r\n";
            SQL += "   )                                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon); 

            return SqlErr;
        }

        public string ins_Xray_Detail_del(PsmhDb pDbCon, string argJob, string argROWID, string argWhere, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argROWID == "")
            {
                return "ROWID 공백";
            }

            SQL = "";
            if (argJob == "00") //백업
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_DETAIL_DEL                                              \r\n";
                SQL += " ( ENTERDATE,IPDOPD,GBRESERVED,SEEKDATE,PANO,SNAME, SEX,AGE,DEPTCODE,DRCODE						\r\n";
                SQL += " 	, WARDCODE,ROOMCODE,XJONG,XSUBCODE,XCODE,EXINFO,QTY,EXMORE,EXID,GBEND,MGRNO,GBPORTABLE 	    \r\n";
                SQL += " 	, REMARK,XRAYROOM,GBNGT,DRREMARK,ORDERNO,ORDERCODE,PACSNO,ORDERNAME,PACSSTUDYID            	\r\n";
                SQL += " 	, PACS_END,GBREAD,READ_SEND,READ_RECEIVE,READ_FLAG,AGREE,ORDERDATE,SENDDATE                 \r\n";
                SQL += " 	, XSENDDATE,PC_BACKDATE,GBPRINT,EXAM_WRTNO,DRDATE,DRWRTNO,STUDY_REF,IMAGE_BDATE )          	\r\n";
                SQL += "  SELECT                                                                                        \r\n";
                SQL += "  ENTERDATE,IPDOPD,GBRESERVED,SEEKDATE,PANO,SNAME, SEX,AGE,DEPTCODE,DRCODE						\r\n";
                SQL += " 	, WARDCODE,ROOMCODE,XJONG,XSUBCODE,XCODE,EXINFO,QTY,EXMORE,EXID,GBEND,MGRNO,GBPORTABLE 	    \r\n";
                SQL += " 	, REMARK,XRAYROOM,GBNGT,DRREMARK,ORDERNO,ORDERCODE,PACSNO,ORDERNAME,PACSSTUDYID            	\r\n";
                SQL += " 	, PACS_END,GBREAD,READ_SEND,READ_RECEIVE,READ_FLAG,AGREE,ORDERDATE,SENDDATE                 \r\n";
                SQL += " 	, XSENDDATE,PC_BACKDATE,GBPRINT,EXAM_WRTNO,DRDATE,DRWRTNO,STUDY_REF,IMAGE_BDATE         	\r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                       \r\n";
            }
            else 
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_DETAIL_DEL                                              \r\n";
                SQL += " ( ENTERDATE,IPDOPD,GBRESERVED,SEEKDATE,PANO,SNAME, SEX,AGE,DEPTCODE,DRCODE						\r\n";
                SQL += " 	, WARDCODE,ROOMCODE,XJONG,XSUBCODE,XCODE,EXINFO,QTY,EXMORE,EXID,GBEND,MGRNO,GBPORTABLE 	    \r\n";
                SQL += " 	, REMARK,XRAYROOM,GBNGT,DRREMARK,ORDERNO,ORDERCODE,PACSNO,ORDERNAME,PACSSTUDYID            	\r\n";
                SQL += " 	, PACS_END,GBREAD,READ_SEND,READ_RECEIVE,READ_FLAG,AGREE,ORDERDATE,SENDDATE                 \r\n";
                SQL += " 	, XSENDDATE,PC_BACKDATE,GBPRINT,EXAM_WRTNO,DRDATE,DRWRTNO,STUDY_REF,IMAGE_BDATE )          	\r\n";
                SQL += "  SELECT                                                                                        \r\n";
                SQL += "  ENTERDATE,IPDOPD,GBRESERVED,SEEKDATE,PANO,SNAME, SEX,AGE,DEPTCODE,DRCODE						\r\n";
                SQL += " 	, WARDCODE,ROOMCODE,XJONG,XSUBCODE,XCODE,EXINFO,QTY,EXMORE,EXID,GBEND,MGRNO,GBPORTABLE 	    \r\n";
                SQL += " 	, REMARK,XRAYROOM,GBNGT,DRREMARK,ORDERNO,ORDERCODE,PACSNO,ORDERNAME,PACSSTUDYID            	\r\n";
                SQL += " 	, PACS_END,GBREAD,READ_SEND,READ_RECEIVE,READ_FLAG,AGREE,ORDERDATE,SENDDATE                 \r\n";
                SQL += " 	, XSENDDATE,PC_BACKDATE,GBPRINT,EXAM_WRTNO,DRDATE,DRWRTNO,STUDY_REF,IMAGE_BDATE         	\r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                       \r\n";
            }
            SQL += "    WHERE 1=1                                                                                       \r\n";
            SQL += "     AND ROWID = '" + argROWID + "'                                                                 \r\n";
            if (argWhere != "")
            {
                SQL += "      " + argWhere + "                                                                          \r\n";
            }


            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_Xray_Detail_del2(PsmhDb pDbCon, string argJob , string argROWID, string argWhere, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argROWID =="")
            {
                return "ROWID 공백";
            }

            SQL = "";
            if (argJob =="00") //백업
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_DETAIL_DEL2                                             \r\n";
                SQL += "  (ENTERDATE,IPDOPD,GBRESERVED,SEEKDATE,PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE                      \r\n";
                SQL += "   ,WARDCODE,ROOMCODE,XJONG,XSUBCODE,XCODE,EXINFO,QTY,EXMORE,EXID,GBEND,MGRNO,GBPORTABLE        \r\n";
                SQL += "   ,REMARK,XRAYROOM,GBNGT,DRREMARK,ORDERNO,ORDERCODE,PACSNO,ORDERNAME,PACSSTUDYID               \r\n";
                SQL += "   ,PACS_END,GBREAD,READ_SEND,READ_RECEIVE,READ_FLAG,AGREE,ORDERDATE,SENDDATE                   \r\n";
                SQL += "   ,XSENDDATE,PC_BACKDATE,GBPRINT,EXAM_WRTNO,DRDATE,DRWRTNO,STUDY_REF,IMAGE_BDATE               \r\n";
                SQL += "   ,GBHIC,HIC_WRTNO,HIC_CODE,BI,CADEX_DEL,BDATE,EMGWRTNO,GBSPC,RDATE,GBSTS,CDATE                \r\n";
                SQL += "   ,DELDATE , CSABUN, CREMARK,DELTIME,DELPART )                                                 \r\n";
                SQL += "  SELECT                                                                                        \r\n";
                SQL += "   ENTERDATE,IPDOPD,GBRESERVED,SEEKDATE,PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE                      \r\n";
                SQL += "   ,WARDCODE,ROOMCODE,XJONG,XSUBCODE,XCODE,EXINFO,QTY,EXMORE,EXID,GBEND,MGRNO,GBPORTABLE        \r\n";
                SQL += "   ,REMARK,XRAYROOM,GBNGT,DRREMARK,ORDERNO,ORDERCODE,PACSNO,ORDERNAME,PACSSTUDYID               \r\n";
                SQL += "   ,PACS_END,GBREAD,READ_SEND,READ_RECEIVE,READ_FLAG,AGREE,ORDERDATE,SENDDATE                   \r\n";
                SQL += "   ,XSENDDATE,PC_BACKDATE,GBPRINT,EXAM_WRTNO,DRDATE,DRWRTNO,STUDY_REF,IMAGE_BDATE               \r\n";
                SQL += "   ,GBHIC,HIC_WRTNO,HIC_CODE,BI,CADEX_DEL,BDATE,EMGWRTNO,GBSPC,RDATE,GBSTS,CDATE                \r\n";
                SQL += "   ,DELDATE , CSABUN, CREMARK,SYSDATE, '" + clsType.User.IdNumber + "'                          \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                       \r\n";
            }
            else if (argJob == "01") //백업
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_DETAIL                                                  \r\n";
                SQL += "   (ENTERDATE,IPDOPD,GBRESERVED,SEEKDATE,PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE                     \r\n";
                SQL += "   ,WARDCODE,ROOMCODE,XJONG,XSUBCODE,XCODE,EXINFO,QTY,EXMORE,EXID,GBEND,MGRNO,GBPORTABLE        \r\n";
                SQL += "   ,REMARK,XRAYROOM,GBNGT,DRREMARK,ORDERNO,ORDERCODE,PACSNO,ORDERNAME,PACSSTUDYID               \r\n";
                SQL += "   ,PACS_END,GBREAD,READ_SEND,READ_RECEIVE,READ_FLAG,AGREE,ORDERDATE,SENDDATE                   \r\n";
                SQL += "   ,XSENDDATE,PC_BACKDATE,GBPRINT,EXAM_WRTNO,DRDATE,DRWRTNO,STUDY_REF,IMAGE_BDATE               \r\n";
                SQL += "   ,GBHIC,HIC_WRTNO,HIC_CODE,BI,CADEX_DEL,BDATE,EMGWRTNO,GBSPC,RDATE,GBSTS                      \r\n";
                SQL += "   ,CDATE,CSABUN,CREMARK )                                                                      \r\n";                
                SQL += "  SELECT                                                                                        \r\n";
                SQL += "   ENTERDATE,IPDOPD,GBRESERVED,SEEKDATE,PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE                      \r\n";
                SQL += "   ,WARDCODE,ROOMCODE,XJONG,XSUBCODE,XCODE,EXINFO,QTY,EXMORE,EXID,GBEND,MGRNO,GBPORTABLE        \r\n";
                SQL += "   ,REMARK,XRAYROOM,GBNGT,DRREMARK,ORDERNO,ORDERCODE,PACSNO,ORDERNAME,PACSSTUDYID               \r\n";
                SQL += "   ,PACS_END,GBREAD,READ_SEND,READ_RECEIVE,READ_FLAG,AGREE,ORDERDATE,SENDDATE                   \r\n";
                SQL += "   ,XSENDDATE,PC_BACKDATE,GBPRINT,EXAM_WRTNO,DRDATE,DRWRTNO,STUDY_REF,IMAGE_BDATE               \r\n";
                SQL += "   ,GBHIC,HIC_WRTNO,HIC_CODE,BI,CADEX_DEL,BDATE,EMGWRTNO,GBSPC,RDATE,GBSTS                      \r\n";
                SQL += "   ,CDATE,CSABUN,CREMARK                                                                        \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL_DEL2                                                  \r\n";
            }                                    
            SQL += "    WHERE 1=1                                                                                       \r\n";
            SQL += "     AND ROWID = '" + argROWID + "'                                                                 \r\n";
            if (argWhere != "")
            {
                SQL += "      " + argWhere + "                                                                          \r\n";
            }


            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_Xray_Detail(PsmhDb pDbCon, cXrayDetail argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_DETAIL   SET                                     \r\n";
                       
            if (argCls.Job=="01")
            {
                
                SQL += "   SEEKDATE  = TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD HH24:MI')          \r\n";
                SQL += "  ,RDATE = TO_DATE('" + argCls.RDate + "','YYYY-MM-DD HH24:MI')                 \r\n";
                                
            }
            else if (argCls.Job == "02") //수동완료처리
            {
                SQL += "   GBSTS  = '" + argCls.GBSTS + "'                                              \r\n";
                SQL += "  ,CDate  = SYSDATE                                                             \r\n";
                SQL += "  ,CSABUN = '" + argCls.Sabun + "'                                              \r\n";                
            }
            else if (argCls.Job == "03") //수동삭제처리
            {
                SQL += "   GBSTS  = '" + argCls.GBSTS + "'                                              \r\n";
                SQL += "  ,DelDate  = SYSDATE                                                           \r\n";                
            }
            else if (argCls.Job == "05") //OCS접수
            {
                SQL += "   GBRESERVED = '" + argCls.GbReserved + "'                                     \r\n";
                SQL += "  ,SEEKDATE  = TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD HH24:MI')          \r\n";
                if (argCls.GBSTS =="2" && argCls.RDate !="")
                {                    
                    SQL += "  ,RDATE  = TO_DATE('" + argCls.RDate + "','YYYY-MM-DD HH24:MI')            \r\n";
                }
                SQL += "  ,Exid = '" + argCls.Exid + "'                                                 \r\n";
                SQL += "  ,XSubCode = '" + argCls.XSubCode + "'                                         \r\n";
                SQL += "  ,XrayRoom = '" + argCls.XrayRoom + "'                                         \r\n";
                SQL += "  ,Remark = '" + argCls.Remark + "'                                             \r\n";
                SQL += "  ,GbNgt = '" + argCls.GbNgt + "'                                               \r\n";
                SQL += "  ,Deptcode = '" + argCls.DeptCode + "'                                         \r\n";
                SQL += "  ,OrderName = '" + argCls.OrderName + "'                                       \r\n";
                SQL += "  ,PacsNo = '" + argCls.PacsNo + "'                                             \r\n";
                SQL += "  ,AGREE = '" + argCls.Agree + "'                                               \r\n";
                SQL += "  ,GbSTS = '" + argCls.GBSTS + "'                                               \r\n";
            }
            else if (argCls.Job == "06") //재료소모량 등록후 갱신
            {
                SQL += "   MgrNo = " + argCls.MgrNo + "                                                 \r\n";                
                SQL += "  ,Exid = '" + argCls.Exid + "'                                                 \r\n";
                //SQL += "  ,XSubCode = '" + argCls.XSubCode + "'                                         \r\n";
                SQL += "  ,XrayRoom = '" + argCls.XrayRoom + "'                                         \r\n";
                SQL += "  ,GbEnd = '" + argCls.GbEND + "'                                               \r\n";
                SQL += "  ,PACS_END = '" + argCls.Pacs_END + "'                                         \r\n";
                SQL += "  ,GBRESERVED = '" + argCls.GbReserved + "'                                     \r\n";                                
                SQL += "  ,GbSTS = '" + argCls.GBSTS + "'                                               \r\n";
                SQL += "  ,CDate = TO_DATE('" + argCls.CDate + "','YYYY-MM-DD HH24:MI')                 \r\n";
                SQL += "  ,CSabun = '" + argCls.CSabun + "'                                             \r\n";
            }
            else
            {
                SQL += "   PANO = '" + argCls.Pano + "'                                                 \r\n";
                SQL += "  ,ENTERDATE = SYSDATE                                                          \r\n";
                SQL += "  ,IPDOPD = '" + argCls.IPDOPD + "'                                             \r\n";
                SQL += "  ,GBRESERVED = '" + argCls.GbReserved + "'                                     \r\n";
                SQL += "  ,SEEKDATE  = TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD HH24:MI')          \r\n";
                SQL += "  ,SNAME = '" + argCls.SName + "'                                               \r\n";
                SQL += "  ,SEX = '" + argCls.Sex + "'                                                   \r\n";
                SQL += "  ,AGE = " + argCls.Age + "                                                     \r\n";
                SQL += "  ,DEPTCODE = '" + argCls.DeptCode + "'                                         \r\n";
                SQL += "  ,DRCODE = '" + argCls.DrCode + "'                                             \r\n";
                SQL += "  ,Exid = '" + argCls.Exid + "'                                                 \r\n";
                SQL += "  ,XJONG = '" + argCls.XJong + "'                                               \r\n";
                SQL += "  ,XSUBCODE = '" + argCls.XSubCode + "'                                         \r\n";
                SQL += "  ,XCODE = '" + argCls.XCode + "'                                               \r\n";
                SQL += "  ,QTY = " + argCls.QTY + "                                                     \r\n";
                SQL += "  ,REMARK = '" + argCls.Remark + "'                                             \r\n";
                SQL += "  ,DrREMARK = '" + argCls.DrRemark + "'                                         \r\n";
                SQL += "  ,XRAYROOM = '" + argCls.XrayRoom + "'                                         \r\n";
                SQL += "  ,ORDERCODE = '" + argCls.OrderCode + "'                                       \r\n";
                SQL += "  ,ORDERNAME = '" + argCls.OrderName + "'                                       \r\n";
                SQL += "  ,BDATE = TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                         \r\n";
                SQL += "  ,RDATE = TO_DATE('" + argCls.RDate + "','YYYY-MM-DD HH:24:MI')                \r\n";
                SQL += "  ,GBSTS = '" + argCls.GBSTS + "'                                               \r\n";
                SQL += "  ,GB_MANUAL = '" + argCls.GB_Manual + "'                                       \r\n";
            }

            SQL += "  WHERE 1=1                                                                         \r\n";
            SQL += "    AND ROWID = '" + argCls.ROWID + "'                                              \r\n";
            if (argCls.Job == "02")
            {
                SQL += "    AND (GBSTS IS NULL OR GBSTS <> '7')                                         \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon,250);

            return SqlErr;
        }

        /// <summary>
        /// XRAY_DETAIL 특정컬럼 갱신
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_Xray_Detail(PsmhDb pDbCon, string argROWID_in, string argROWID, string argPtno, string argUpCols, string argWhere, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argPtno == "" && argROWID == "" && argROWID_in =="")
            {
                return "자료갱신 오류!!";
            }

            if (argUpCols =="")
            {
                return "자료갱신 오류!!";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_DETAIL  SET          \r\n";

            SQL += "    " + argUpCols + "                                   \r\n";

            SQL += "  WHERE 1=1                                             \r\n";
            if (argROWID_in !="")
            {
                SQL += "    AND ROWID IN ( " + argROWID_in + " )            \r\n";
            }
            if (argROWID != "")
            {
                SQL += "    AND ROWID = '" + argROWID + "'                  \r\n";
            }
            if (argPtno != "")
            {
                SQL += "    AND Pano = '" + argPtno + "'                    \r\n";
            }
            if (argWhere != "")
            {
                SQL += "      " + argWhere + "                              \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon,250); 

            return SqlErr;
        }

        public string up_Xray_Detail(PsmhDb pDbCon, string argJob,  string argPtno, string argUpCols, string argWhere, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argJob == "")
            {
                return "자료갱신 오류!!";
            }

            if (argWhere == "" || argUpCols =="")
            {
                return "자료갱신 오류!!";
            }

            SQL = "";

            if (argJob =="00")
            {
                SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_DETAIL  SET          \r\n";
                SQL += "    " + argUpCols + "                                   \r\n";
                SQL += "  WHERE 1=1                                             \r\n";
                SQL += "    AND Pano = '" + argPtno + "'                        \r\n";
                if (argWhere != "")
                {
                    SQL += "      " + argWhere + "                              \r\n";
                }
            }
            else
            {

            }            

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 250);

            return SqlErr;
        }

        public string del_Xray_Detail(PsmhDb pDbCon, string argROWID, string argWhere, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argROWID =="")
            {
                return "ROWID 공백";
            }

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_PMPA + "XRAY_DETAIL              \r\n";            
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND ROWID = '" + argROWID + "'                          \r\n";
            if (argWhere != "")
            {
                SQL += "      " + argWhere + "                                  \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
                

        /// <summary>
        /// 영상의학과 CD복사 신청 테이블 class
        /// </summary>
        public class cXrayCdCopy
        {
            public string Job = "";
            public string Pano = "";
            public string SName = "";
            public string Sex = "";
            public int Age = 0;
            public string GbIO = "";
            public string Date1 = "";
            public string Date2 = "";
            public string SeekDate = "";
            public string BDate = "";
            public string XCode = "";
            public string XName = "";
            public string XJong = "";
            public string XSubCode = "";            
            public string DeptCode = "";
            public string DrCode = "";
            public string RoomCode = "";
            public string WardCode = "";
            public string PacsNo = "";
            public int CdQty = 0;
            public int CdMake = 0;
            public string CdCopyTime = "";
            public long EntSabun = 0;
            public string EntTime = "";
            public string CdGubun = "";
            public string DelDate = "";
            public long OrderNo = 0;
            public string GbER = "";
            public string Bi = "";

            public int nMaxRow = 0; //출력시 1장 표시 row
                     
            public string ROWID = "";

        }

        public string ins_Xray_CdCopy(PsmhDb pDbCon, cXrayCdCopy argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
                        
            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_CDCOPY                                  \r\n";
            SQL += "  (PANO,SNAME,IPDOPD,SEEKDATE,XJONG,XCODE,XSUBCODE,XNAME,DEPTCODE,DRCODE        \r\n";
            SQL += "   ,ROOMCODE,WARDCODE,PACSNO,CDQTY,CDMAKE,COPYTIME,ENTSABUN,ENTTIME,CDGUBUN     \r\n";
            SQL += "   ,BDATE,DELDATE,ORDERNO)  VALUES                                             \r\n";
            SQL += "   (                                                                            \r\n";
            SQL += "  '" + argCls.Pano + "'                                                         \r\n";
            SQL += "  ,'" + argCls.SName + "'                                                       \r\n";
            SQL += "  ,'" + argCls.GbIO + "'                                                        \r\n";            
            SQL += "  ,TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD HH24:MI')                      \r\n";
            SQL += "  ,'" + argCls.XJong + "'                                                       \r\n";
            SQL += "  ,'" + argCls.XCode + "'                                                       \r\n";
            SQL += "  ,'" + argCls.XSubCode + "'                                                    \r\n";
            SQL += "  ,'" + argCls.XName + "'                                                       \r\n";
            SQL += "  ,'" + argCls.DeptCode + "'                                                    \r\n";
            SQL += "  ,'" + argCls.DrCode + "'                                                      \r\n";
            SQL += "  ,'" + argCls.RoomCode + "'                                                    \r\n";
            SQL += "  ,'" + argCls.WardCode + "'                                                    \r\n";
            SQL += "  ,'" + argCls.PacsNo + "'                                                      \r\n";
            SQL += "  ," + argCls.CdQty + "                                                         \r\n";
            SQL += "  ," + argCls.CdMake + "                                                        \r\n";
            SQL += "  ,TO_DATE('" + argCls.CdCopyTime + "','YYYY-MM-DD HH24:MI')                    \r\n";
            SQL += "  ," + argCls.EntSabun + "                                                      \r\n";
            SQL += "  ,SYSDATE                                                                      \r\n";
            SQL += "  ,'" + argCls.CdGubun + "'                                                     \r\n";            
            SQL += "  ,TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                                 \r\n";
            SQL += "  ,''                                                                           \r\n";
            SQL += "  ," + argCls.OrderNo + "                                                       \r\n";            
            SQL += "   )                                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_Xray_CdCopy(PsmhDb pDbCon, cXrayCdCopy argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
                        
            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_CDCOPY  SET                                  \r\n";
            if (argCls.Job =="01")
            {
                SQL += "   IPDOPD = '" + argCls.GbIO + "'                                               \r\n";
                SQL += "  ,SEEKDATE = TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD HH24:MI')           \r\n";
                SQL += "  ,XJONG = '" + argCls.XJong + "'                                               \r\n";
                SQL += "  ,XCODE = '" + argCls.XCode + "'                                               \r\n";
                SQL += "  ,XSUBCODE = " + argCls.XSubCode + "                                           \r\n";
                SQL += "  ,XNAME = '" + argCls.XName + "'                                               \r\n";
                SQL += "  ,DEPTCODE ='" + argCls.DeptCode + "'                                          \r\n";
                SQL += "  ,DRCODE = '" + argCls.DrCode + "'                                             \r\n";
                SQL += "  ,ROOMCODE = '" + argCls.RoomCode + "'                                         \r\n";
                SQL += "  ,WARDCODE = '" + argCls.WardCode + "'                                         \r\n";
                SQL += "  ,PACSNO = '" + argCls.PacsNo + "'                                             \r\n";
                SQL += "  ,CDQTY =" + argCls.CdQty + "                                                  \r\n";
                SQL += "  ,CDMAKE = '" + argCls.CdMake + "'                                             \r\n";
                SQL += "  ,ENTSABUN = " + argCls.EntSabun + "                                           \r\n";
                SQL += "  ,ENTTIME = SYSDATE                                                            \r\n";
                SQL += "  ,CDGUBUN = '" + argCls.CdGubun + "'                                           \r\n";
                SQL += "  ,BDATE = TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                         \r\n";
                if (argCls.DelDate != "")
                {
                    SQL += "  ,DELDATE = TO_DATE('" + argCls.DelDate + "','YYYY-MM-DD')                 \r\n";
                    SQL += "  ,DELSABUN = " + argCls.EntSabun + "                                       \r\n";
                }
                SQL += "  ,ORDERNO = " + argCls.OrderNo + "                                             \r\n";
                SQL += "  WHERE 1=1                                                                     \r\n";
                SQL += "    AND Pano = '" + argCls.Pano + "'                                            \r\n";
                SQL += "    AND OrderNo = " + argCls.OrderNo + "                                        \r\n";
            }
            else if(argCls.Job == "02")
            {                
                SQL += "   ENTSABUN = " + argCls.EntSabun + "                                           \r\n";
                SQL += "  ,ENTTIME = SYSDATE                                                            \r\n";                
                SQL += "  ,DELDATE = TO_DATE('" + argCls.DelDate + "','YYYY-MM-DD')                     \r\n";                
                SQL += "  ,DELSABUN = " + argCls.EntSabun + "                                           \r\n";
                SQL += "  ,ORDERNO = " + argCls.OrderNo + "                                             \r\n";
                SQL += "  WHERE 1=1                                                                     \r\n";
                SQL += "    AND ROWID = '" + argCls.ROWID + "'                                          \r\n";
            }
            else if (argCls.Job == "03")
            {   
                SQL += "   CdQty = " + argCls.CdQty + "                                                 \r\n";                
                SQL += "  WHERE 1=1                                                                     \r\n";
                SQL += "    AND ROWID = '" + argCls.ROWID + "'                                          \r\n";
            }
            else if (argCls.Job == "04")
            {
                SQL += "  DELDATE = SYSDATE                                                             \r\n";
                SQL += "  ,DELSABUN = " + argCls.EntSabun + "                                           \r\n";
                SQL += "  WHERE 1=1                                                                     \r\n";
                SQL += "    AND ROWID = '" + argCls.ROWID + "'                                          \r\n";
            }
            else if (argCls.Job == "05")
            {                
                SQL += "  CdMake = " + argCls.CdMake + "                                                \r\n";
                SQL += "  WHERE 1=1                                                                     \r\n";
                SQL += "    AND ROWID = '" + argCls.ROWID + "'                                          \r\n";
                SQL += "    AND ( DelDate IS NULL OR DelDate ='')                                       \r\n";
            }
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// Xray_CdCopy 특정컬럼 갱신
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_Xray_CdCopy(PsmhDb pDbCon, string argROWID, string argPano, string argUpCols, string argWhere, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argPano == "" && argROWID == "")
            {
                return "자료갱신 오류!!";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_CDCOPY  SET          \r\n";

            SQL += "    " + argUpCols + "                                   \r\n";

            SQL += "  WHERE 1=1                                             \r\n";
            if (argROWID != "")
            {
                SQL += "    AND ROWID = '" + argROWID + "'                  \r\n";
            }
            if (argPano != "")
            {
                SQL += "    AND Pano = '" + argPano + "'                    \r\n";
            }
            if (argWhere != "")
            {
                SQL += "      " + argWhere + "                              \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public class cXrayContrast
        {
            public string Pano = "";
            public string BDate = "";
            public string Date1 = "";
            public string Date2 = "";
            public string Search = "";
            public string SName = "";
            public string Remark = "";
            public long EntSabun = 0;
            public string EntTime = "";
            public string ROWID = "";
        }

        public string ins_XRAY_CONTRAST(PsmhDb pDbCon, cXrayContrast argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_CONTRAST                                \r\n";
            SQL += "  (Pano,BDate,SName,Remark,EntSabun,EntTime)  VALUES                            \r\n";
            SQL += "   (                                                                            \r\n";
            SQL += "  '" + argCls.Pano + "'                                                         \r\n";
            SQL += "  ,TO_DATE('" + argCls.BDate + "','YYYY-MM-DD HH24:MI')                         \r\n";            
            SQL += "  ,'" + argCls.SName + "'                                                       \r\n";
            SQL += "  ,'" + argCls.Remark + "'                                                      \r\n";
            SQL += "  ," + argCls.EntSabun + "                                                      \r\n";
            SQL += "  ,SYSDATE                                                                      \r\n";            
            SQL += "   )                                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_XRAY_CONTRAST(PsmhDb pDbCon, cXrayContrast argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_CONTRAST  SET                                \r\n";
            SQL += "   SName = '" + argCls.SName + "'                                               \r\n";
            SQL += "  ,BDATE = TO_DATE('" + argCls.BDate + "','YYYY-MM-DD HH24:MI')                 \r\n";            
            SQL += "  ,Remark = '" + argCls.Remark + "'                                             \r\n";
            SQL += "  ,EntSabun = " + argCls.EntSabun + "                                           \r\n";
            SQL += "  ,ENTTIME = SYSDATE                                                            \r\n";            
            SQL += "  WHERE 1=1                                                                     \r\n";
            SQL += "    AND ROWID = '" + argCls.ROWID + "'                                            \r\n";
            
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string del_XRAY_CONTRAST(PsmhDb pDbCon, cXrayContrast argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_PMPA + "XRAY_CONTRAST                 \r\n";

            SQL += "  WHERE 1=1                                                 \r\n";
            if (argCls.ROWID != "")
            {
                SQL += "    AND ROWID = '" + argCls.ROWID + "'                  \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public class cXrayPacsSend2
        {
            public string Job = "";
            public string Pano = "";
            public string PacsNo = "";
            public string SendGbn = "";
            public string EntDate = "";
            public string SName = "";
            public string SName2 = "";
            public string EName = "";
            public string EName2 = "";
            public string IpdOpd = "";
            public string Jumin1 = "";
            public string Jumin2 = "";
            public string Birth = "";
            public string Sex = "";
            public string Age = "";
            public string DeptCode = "";
            public string DrCode = "";
            public string DrName = "";
            public string DrEName = "";
            public string WardCode = "";
            public string RoomCode = "";
            public string OrderCode = "";
            public string OrderName = "";
            public string Remark = "";
            public string SeekDate = "";
            public string XJong = "";
            public string XSubCode = "";
            public string XrayRoom = "";
            public string MsgControlid = "";
            public string Resource = "";
            public string Modality = "";
            public long ReadNo = 0;
            public long ReadDrSabun = 0;
            public string ReadDrName = "";
            public string DrRemark = "";
            public long ExID = 0;
            public string EndoChk = "";
            public string Emergency = "";
            public string Buse = ""; //'부서코드(6자리)
            public string XCode = ""; //'검사코드 2011-03-16
            public string Operator = ""; //'촬영자
            public string Disease = ""; //'질병명 2014-03-14
            public string PACS_Code = ""; //'2014-05-01  

            public string Result = ""; //판독결과 값
            public string ResultDate = "";  //판독결과일자
            public string GbInfo = "";

            public string INPS = "";
            public string INPT_DT = "";
            public string UPPS = "";
            public string UP_DT = "";

            public string ROWID = "";            

        }

        public string ins_XRAY_PACSSEND(PsmhDb pDbCon, string argJob,string argSendGbn, string argROWID, string argSabun, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
             
            SQL = "";
            if (argJob=="00")
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND  (                     \r\n";
                SQL += "    EntDate,PacsNo,SendGbn,Pano,SName                                   \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,ReadNo,INPS,INPT_DT )      \r\n";
                //SQL += "  SELECT SYSDATE,PacsNo,'5',Pano,SName                                  \r\n";
                SQL += "  SELECT SYSDATE,PacsNo,'" + argSendGbn + "',Pano,SName                 \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,ExInfo                     \r\n";
                SQL += "   ,'" + argSabun + "',SYSDATE                                          \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                \r\n";
                SQL += "   WHERE 1=1                                                            \r\n";
                SQL += "    AND ROWID = '" + argROWID + "'                                      \r\n";
                SQL += "    AND PacsNo IS NOT NULL                                              \r\n";
                SQL += "    AND ExInfo > 1000                                                   \r\n";
            }
            else if (argJob == "01")
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND  (                     \r\n";
                SQL += "    EntDate,PacsNo,SendGbn,Pano,SName                                   \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark                   \r\n";
                SQL += "   ,ReadNo,INPS,INPT_DT )                                               \r\n";
                //SQL += "  SELECT SYSDATE,PacsNo,'3',Pano,SName                                  \r\n";
                SQL += "  SELECT SYSDATE,PacsNo,'" + argSendGbn + "',Pano,SName                 \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,ExInfo            \r\n";
                SQL += "   ,'" + argSabun + "',SYSDATE                                          \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                \r\n";
                SQL += "   WHERE 1=1                                                            \r\n";
                SQL += "    AND ROWID = '" + argROWID + "'                                      \r\n";
                SQL += "    AND PacsNo IS NOT NULL                                              \r\n";                
            }
            else if (argJob == "02")
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND  (                     \r\n";
                SQL += "    EntDate,PacsNo,SendGbn,Pano,SName                                   \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark                   \r\n";
                SQL += "   ,ReadNo,INPS,INPT_DT )                                               \r\n";
                //SQL += "  SELECT SYSDATE,PacsNo,'4',Pano,SName                                  \r\n";
                SQL += "  SELECT SYSDATE,PacsNo,'" + argSendGbn + "',Pano,SName                 \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,ExInfo            \r\n";
                SQL += "   ,'" + argSabun + "',SYSDATE                                          \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                \r\n";
                SQL += "   WHERE 1=1                                                            \r\n";
                SQL += "    AND ROWID = '" + argROWID + "'                                      \r\n";
                SQL += "    AND PacsNo IS NOT NULL                                              \r\n";
            }
            else if (argJob == "03")
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND  (                     \r\n";
                SQL += "    EntDate,PacsNo,SendGbn,Pano,SName                                   \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark                   \r\n";
                SQL += "   ,ReadNo,INPS,INPT_DT )                                               \r\n";
                //SQL += "  SELECT SYSDATE,PacsNo,'2',Pano,SName                                  \r\n";
                SQL += "  SELECT SYSDATE,PacsNo,'" + argSendGbn + "',Pano,SName                 \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,ExInfo            \r\n";
                SQL += "   ,'" + argSabun + "',SYSDATE                                          \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                \r\n";
                SQL += "   WHERE 1=1                                                            \r\n";
                SQL += "    AND ROWID = '" + argROWID + "'                                      \r\n";
                SQL += "    AND PacsNo IS NOT NULL                                              \r\n";
                SQL += "    AND (GbHIC IS NULL OR GbHIC <> 'Y')                                 \r\n";
                SQL += "    AND XJong <> '1'                                                    \r\n";
            }
            else if (argJob == "05")
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND  (                     \r\n";
                SQL += "    EntDate,PacsNo,SendGbn,Pano,SName                                   \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark                   \r\n";
                SQL += "   ,XRayName,SendTime,ReadNo,INPS,INPT_DT )                             \r\n";
                SQL += "  SELECT SYSDATE,PacsNo,'" + argSendGbn + "' ,Pano,SName                \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark                   \r\n";
                SQL += "   ,OrderName,SYSDATE,Exinfo                                            \r\n";
                SQL += "   ,'" + argSabun + "',SYSDATE                                          \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                \r\n";
                SQL += "   WHERE 1=1                                                            \r\n";
                SQL += "    AND ROWID = '" + argROWID + "'                                      \r\n";             
            }
            else if (argJob == "06")
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND  (                     \r\n";
                SQL += "    EntDate,PacsNo,SendGbn,Pano,SName                                   \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark                   \r\n";
                SQL += "   ,ReadNo,INPS,INPT_DT )                                               \r\n";
                //SQL += "  SELECT SYSDATE,PacsNo,'2',Pano,SName                                  \r\n";
                SQL += "  SELECT SYSDATE,PacsNo,'" + argSendGbn + "',Pano,SName                 \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,Exinfo            \r\n";
                SQL += "   ,'" + argSabun + "',SYSDATE                                          \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                \r\n";
                SQL += "   WHERE 1=1                                                            \r\n";
                SQL += "    AND ROWID = '" + argROWID + "'                                      \r\n";
                SQL += "    AND PacsNo IS NOT NULL                                              \r\n";                
                SQL += "    AND XJong <> '1'                                                    \r\n";
            }
            
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }                

        public string ins_XRAY_PACSSEND(PsmhDb pDbCon, cXrayPacsSend2 argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
            
            SQL = "";

            if (argCls.Job =="00")
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND  (                                     \r\n";
                SQL += "    EntDate,PacsNo,SendGbn,Pano,SName                                                   \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode                     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,INPS,INPT_DT )                    \r\n";
                SQL += "  SELECT SYSDATE,PacsNo,'" + argCls.SendGbn + "',Pano,SName                             \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode                     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark                                   \r\n";
                SQL += "   ,'" + argCls.INPS +"',SYSDATE                                                        \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                \r\n";
                SQL += "   WHERE 1=1                                                                            \r\n";
                SQL += "    AND ROWID = '" + argCls.ROWID + "'                                                  \r\n";

            }
            else if (argCls.Job == "01")
            {
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND                                         \r\n";
                SQL += "   (EntDate,PacsNo,SendGbn,Pano,SName                                                   \r\n";
                SQL += "    ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode                    \r\n";
                SQL += "    XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,INPS,INPT_DT) VALUES              \r\n";
                SQL += "   (                                                                                    \r\n";
                SQL += "     SYSDATE                                                                            \r\n";
                SQL += "     ,'" + argCls.PacsNo + "'                                                           \r\n";
                SQL += "     ,'" + argCls.SendGbn + "'                                                          \r\n";
                SQL += "     ,'" + argCls.Pano + "'                                                             \r\n";
                SQL += "     ,'" + argCls.SName + "'                                                            \r\n";
                SQL += "     ,'" + argCls.Sex + "'                                                              \r\n";
                SQL += "     ,'" + argCls.Age + "'                                                              \r\n";
                SQL += "     ,'" + argCls.IpdOpd + "'                                                           \r\n";
                SQL += "     ,'" + argCls.DeptCode + "'                                                         \r\n";
                SQL += "     ,'" + argCls.DrCode + "'                                                           \r\n";
                SQL += "     ,'" + argCls.WardCode + "'                                                         \r\n";
                SQL += "     ,'" + argCls.RoomCode + "'                                                         \r\n";
                SQL += "     ,'" + argCls.XJong + "'                                                            \r\n";
                SQL += "     ,'" + argCls.XSubCode + "'                                                         \r\n";
                SQL += "     ,'" + argCls.XCode + "'                                                            \r\n";
                SQL += "     ,'" + argCls.OrderCode + "'                                                        \r\n";
                SQL += "     ,TO_DATE('" + argCls.SeekDate + "','YYYYMMDD')                                     \r\n";
                SQL += "     ,'" + argCls.Remark + "'                                                           \r\n";
                SQL += "     ,'" + argCls.XrayRoom + "'                                                         \r\n";
                SQL += "     ,'" + argCls.DrRemark + "'                                                         \r\n";
                SQL += "     ,'" + argCls.INPS + "'                                                             \r\n";
                SQL += "     ,SYSDATE                                                                           \r\n";
                SQL += "   )                                                                                    \r\n";
            }
            else if (argCls.Job == "02") //메뉴얼접수
            {
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND                                         \r\n";
                SQL += "   (EntDate,PacsNo,SendGbn,Pano,SName                                                   \r\n";
                SQL += "    ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode                    \r\n";
                SQL += "    ,XCode,OrderCode,SeekDate,Remark,XRayRoom,XRayName,Gbinfo,INPS,INPT_DT) VALUES      \r\n";
                SQL += "   (                                                                                    \r\n";
                SQL += "     SYSDATE                                                                            \r\n";
                SQL += "     ,'" + argCls.PacsNo + "'                                                           \r\n";
                SQL += "     ,'" + argCls.SendGbn + "'                                                          \r\n";
                SQL += "     ,'" + argCls.Pano + "'                                                             \r\n";
                SQL += "     ,'" + argCls.SName + "'                                                            \r\n";
                SQL += "     ,'" + argCls.Sex + "'                                                              \r\n";
                SQL += "     ,'" + argCls.Age + "'                                                              \r\n";
                SQL += "     ,'" + argCls.IpdOpd + "'                                                           \r\n";
                SQL += "     ,'" + argCls.DeptCode + "'                                                         \r\n";
                SQL += "     ,'" + argCls.DrCode + "'                                                           \r\n";
                SQL += "     ,'" + argCls.WardCode + "'                                                         \r\n";
                SQL += "     ,'" + argCls.RoomCode + "'                                                         \r\n";
                SQL += "     ,'" + argCls.XJong + "'                                                            \r\n";
                SQL += "     ,'" + argCls.XSubCode + "'                                                         \r\n";
                SQL += "     ,'" + argCls.XCode + "'                                                            \r\n";
                SQL += "     ,'" + argCls.OrderCode + "'                                                        \r\n";
                SQL += "     ,TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD HH24:MI')                           \r\n";                
                SQL += "     ,'" + argCls.Remark + "'                                                           \r\n";
                SQL += "     ,'" + argCls.XrayRoom + "'                                                         \r\n";
                SQL += "     ,'" + argCls.OrderName + "'                                                        \r\n";
                SQL += "     ,'" + argCls.GbInfo + "'                                                           \r\n";
                SQL += "     ,'" + argCls.INPS + "'                                                             \r\n";
                SQL += "     ,SYSDATE                                                                           \r\n";
                SQL += "   )                                                                                    \r\n";
            }
            else if (argCls.Job == "03") //OCS접수
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND  (                                     \r\n";
                SQL += "    EntDate,PacsNo,SendGbn,Pano,SName                                                   \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode                     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,ReadNo,INPS,INPT_DT )             \r\n";
                SQL += "  SELECT SYSDATE,PacsNo,'" + argCls.SendGbn + "',Pano,SName                             \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode                     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,ExInfo                            \r\n";
                SQL += "   ,'" + argCls.INPS + "',SYSDATE                                                       \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                \r\n";
                SQL += "   WHERE 1=1                                                                            \r\n";
                SQL += "    AND ROWID = '" + argCls.ROWID + "'                                                  \r\n";
                SQL += "    AND PacsNo IS NOT NULL                                                              \r\n";
                SQL += "    AND (XJong <> '1' OR (XJong = '1' AND XRayRoom='T'))                                \r\n";

            }
            else if (argCls.Job == "05") //판독에 사용 - ROWID IN 
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND  (                                     \r\n";
                SQL += "    EntDate,PacsNo,SendGbn,Pano,SName                                                   \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode                     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,ReadNo,INPS,INPT_DT )             \r\n";
                SQL += "  SELECT SYSDATE,PacsNo,'" + argCls.SendGbn + "',Pano,SName                             \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode                     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,ExInfo                            \r\n";
                SQL += "   ,'" + argCls.INPS + "',SYSDATE                                                       \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                \r\n";
                SQL += "   WHERE 1=1                                                                            \r\n";
                SQL += "    AND ROWID IN ( " + argCls.ROWID + " )                                               \r\n";
                SQL += "    AND PacsNo IS NOT NULL                                                              \r\n";

            }
            else if (argCls.Job == "06") //판독에 사용 - WRTNO = , '5'임시저장, '6' 저장
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND  (                                     \r\n";
                SQL += "    EntDate,PacsNo,SendGbn,Pano,SName                                                   \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode                     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,ReadNo,INPS,INPT_DT )             \r\n";
                SQL += "  SELECT SYSDATE,PacsNo,'" + argCls.SendGbn + "',Pano,SName                             \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode                     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,ExInfo                            \r\n";
                SQL += "   ,'" + argCls.INPS + "',SYSDATE                                                       \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                \r\n";
                SQL += "   WHERE 1=1                                                                            \r\n";
                SQL += "    AND Pano = '" + argCls.Pano + "'                                                    \r\n";
                SQL += "    AND ExInfo = " + argCls.ReadNo + "                                                  \r\n";
                SQL += "    AND PacsNo IS NOT NULL                                                              \r\n";
                
            }
            else if (argCls.Job == "07") //판독에서 판독삭제 - WRTNO = , '7'
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND  (                                     \r\n";
                SQL += "    EntDate,PacsNo,SendGbn,Pano,SName                                                   \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode                     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,ReadNo,INPS,INPT_DT )             \r\n";
                SQL += "  SELECT SYSDATE,PacsNo,'" + argCls.SendGbn + "',Pano,SName                             \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode                     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,ExInfo                            \r\n";
                SQL += "   ,'" + argCls.INPS + "',SYSDATE                                                       \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                \r\n";
                SQL += "   WHERE 1=1                                                                            \r\n";
                SQL += "    AND Pano = '" + argCls.Pano + "'                                                    \r\n";
                SQL += "    AND ExInfo = " + argCls.ReadNo + "                                                  \r\n";
                SQL += "    AND PacsNo IS NOT NULL                                                              \r\n";

            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon,250);

            return SqlErr;
        }                
        
        /// <summary>
        /// DORDER 특정컬럼 갱신
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_DORDER(PsmhDb pDbCon, string argROWID, string argPtno, string argUpCols, string argWhere, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argPtno == "" && argROWID == "")
            {
                return "자료갱신 오류!!";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PACS + "DORDER  SET               \r\n";

            SQL += "    " + argUpCols + "                                   \r\n";

            SQL += "  WHERE 1=1                                             \r\n";
            if (argROWID != "")
            {
                SQL += "    AND ROWID = '" + argROWID + "'                  \r\n";
            }
            if (argPtno != "")
            {
                SQL += "    AND PATID = '" + argPtno + "'                    \r\n";
            }
            if (argWhere != "")
            {
                SQL += "      " + argWhere + "                              \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public class cXray_Wait
        {
            public string Job = "";
            public string GbIO = "";
            public string JDATE = "";
            public string GBROOM = "";
            public string SEQTIME = "";
            public string GBN = "";
            public string GBEND = "";
            public string PANO = "";
            public string SNAME = "";
            public string JEPTIME = "";
            public string DEPTCODE = "";
            public string PART = "";
            public string STS = "";
            public string CHEST = "";
            public string RDATE = "";
            public string RDATE2 = "";
            public string REMARK = "";
            public long ORDERNO = 0;
            public string STime = "";
            public int nCnt = 0;
            public string ROWID = "";
            public string ERCT = ""; 
        }

        public string ins_XRAY_WAIT(PsmhDb pDbCon, cXray_Wait argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";

            SQL = " INSERT INTO " + ComNum.DB_PMPA + "XRAY_WAIT                                     \r\n";
            SQL += "   (JDATE,GBROOM,SEQTIME,GBN,GBEND,PANO,SNAME                                   \r\n";            
            SQL += "    ,JEPTIME,DEPTCODE,Part,STS,Chest,OrderNo) VALUES                            \r\n";
            SQL += "   (                                                                            \r\n";
            SQL += "     TO_DATE('" + argCls.JDATE + "','YYYY-MM-DD')                               \r\n";
            SQL += "     ,'" + argCls.GBROOM + "'                                                   \r\n";
            SQL += "     ,'" + argCls.SEQTIME + "'                                                  \r\n";
            SQL += "     ,'" + argCls.GBN + "'                                                      \r\n";
            SQL += "     ,'" + argCls.GBEND + "'                                                    \r\n";
            SQL += "     ,'" + argCls.PANO + "'                                                     \r\n";
            SQL += "     ,'" + argCls.SNAME + "'                                                    \r\n";
            SQL += "     ,SYSDATE                                                                   \r\n";
            SQL += "     ,'" + argCls.DEPTCODE + "'                                                 \r\n";
            SQL += "     ,'" + argCls.PART + "'                                                     \r\n";
            SQL += "     ,'" + argCls.STS + "'                                                      \r\n";
            SQL += "     ,'" + argCls.CHEST + "'                                                    \r\n";
            SQL += "     ,'" + argCls.ORDERNO + "'                                                  \r\n";
            SQL += "   )                                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon,120);

            return SqlErr;
        }

        public string up_XRAY_WAIT(PsmhDb pDbCon, cXray_Wait argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            
            if (argCls.Job =="00")
            {
                SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_WAIT  SET                                    \r\n";
                SQL += "   SEQTIME = '" + argCls.SEQTIME + "'                                           \r\n";
                SQL += "  ,STS = '" + argCls.STS + "'                                                   \r\n";
                SQL += "  ,GbEnd = '" + argCls.GBEND + "'                                               \r\n";
                SQL += "  ,Gbn = '" + argCls.GBN + "'                                                   \r\n";
                SQL += "  WHERE 1=1                                                                     \r\n";
                SQL += "    AND ROWID = '" + argCls.ROWID + "'                                          \r\n";
            }
            else if (argCls.Job == "01")
            {
                SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_WAIT  SET                                    \r\n";
                if (argCls.STS == "0")
                {
                    SQL += "   JepTIME = SYSDATE                                                        \r\n";
                    SQL += "  ,GbEnd = '1'                                                              \r\n";                    
                }
                else if (argCls.STS == "1")
                {
                    SQL += "   STS = '2'                                                                \r\n";
                    SQL += "  ,GbEnd = '0'                                                              \r\n";
                }
                else if (argCls.STS == "2")
                {
                    SQL += "   STS = '3'                                                                \r\n";
                    SQL += "  ,GbEnd = '*'                                                              \r\n";
                }
                SQL += "  WHERE 1=1                                                                     \r\n";
                SQL += "    AND ROWID IN  ( " + argCls.ROWID + " )                                      \r\n";
                if (argCls.STS =="0")
                {
                    SQL += "    AND GbEnd <> '1'                                                        \r\n";                    
                }
                else if (argCls.STS == "1")
                {
                    SQL += "    AND GbEnd = '1'                                                         \r\n";                    
                }
                else if (argCls.STS == "2")
                {                    
                    
                }
            }
            else if (argCls.Job == "02") //CT
            {
                SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_WAIT  SET                                    \r\n";
                if (argCls.STS == "0")
                {
                    SQL += "   JepTIME = SYSDATE                                                        \r\n";
                    SQL += "  ,GbEnd = '1'                                                              \r\n";
                }
                else if (argCls.STS == "1")
                {
                    SQL += "   STS = '2'                                                                \r\n";
                    SQL += "  ,GbEnd = '0'                                                              \r\n";
                }
                else if (argCls.STS == "2")
                {
                    SQL += "   STS = '3'                                                                \r\n";
                    SQL += "  ,GbEnd = '*'                                                              \r\n";
                }
                SQL += "  WHERE 1=1                                                                     \r\n";
                SQL += "    AND ROWID IN  ( " + argCls.ROWID + " )                                      \r\n";
                if (argCls.STS == "0")
                {
                    SQL += "    AND GbEnd <> '1'                                                        \r\n";
                    SQL += "    AND GbRoom = '" + argCls.GBROOM + "'                                    \r\n";
                }
                else if (argCls.STS == "1")
                {
                    SQL += "    AND GbEnd = '1'                                                         \r\n";
                    SQL += "    AND GbRoom = '" + argCls.GBROOM + "'                                    \r\n";
                }
                else if (argCls.STS == "2")
                {
                    SQL += "    AND GbRoom = '" + argCls.GBROOM + "'                                    \r\n";
                }
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon,120);

            return SqlErr;
        }

        public string up_XRAY_WAIT(PsmhDb pDbCon, string argROWID,  string argUpCols, string argWhere, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if ( argROWID == "")
            {
                return "자료갱신 오류!!";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_WAIT  SET             \r\n";

            SQL += "    " + argUpCols + "                                   \r\n";

            SQL += "  WHERE 1=1                                             \r\n";
            if (argROWID != "")
            {
                SQL += "    AND ROWID = '" + argROWID + "'                  \r\n";
            }            
            if (argWhere != "")
            {
                SQL += "      " + argWhere + "                              \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public class cOCS_ORDER
        {
            public string Job = "";
            public string GbIO = "";
            public string PTNO = "";
            public string BDATE = "";
            public string DEPTCODE = "";
            public string SEQNO = "";
            public string ORDERCODE = "";
            public string SUCODE = "";
            public string BUN = "";
            public string SLIPNO = "";
            public string RealQTY = "";
            public int QTY = 0;
            public int NAL = 0;
            public int RealNal = 0;
            public string GBDIV = "";
            public string DOSCODE = "";
            public string GBBOTH = "";
            public string GBINFO = "";
            public string GBER = "";
            public string GBSELF = "";
            public string GBSPC = "";
            public string BI = "";
            public string DRCODE = "";
            public string StaffID = "";
            public string REMARK = "";
            public string ENTDATE = "";
            public string GBSUNAP = "";
            public string TUYAKNO = "";
            public long ORDERNO = 0;
            public string MULTI = "";
            public string MULTIREMARK = "";
            public string DUR = "";
            public string RESV = "";
            public string SCODESAYU = "";
            public string SCODEREMARK = "";
            public string GBSEND = "";
            public string WardCode = "";
            public string RoomCode = "";
            public string GbStatus = "";
            public string GbPRN = "";
            public string Sabun = "";

        }

        public string ins_OCS_ORDER(PsmhDb pDbCon, cOCS_ORDER argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            if (argCls.GbIO =="O")
            {
                SQL = " INSERT INTO " + ComNum.DB_MED + "OCS_OORDER                                 \r\n";
            }
            else if (argCls.GbIO == "I" || argCls.GbIO == "E")
            {
                SQL = " INSERT INTO " + ComNum.DB_MED + "OCS_IORDER                                 \r\n";
            }
            SQL += "   (PTNO, BDATE, DEPTCODE, SEQNO, ORDERCODE, SUCODE, BUN                        \r\n";
            SQL += "    ,SLIPNO, REALQTY, QTY, NAL, GBDIV,DOSCODE, GBBOTH, GBINFO                   \r\n";
            SQL += "    ,GBER, GBSELF, GBSPC, BI, DRCODE, REMARK, ENTDATE                           \r\n";            
            SQL += "    ,ORDERNO, MULTI, MULTIREMARK, DUR                                           \r\n";

            if (argCls.GbIO =="O")
            {
                SQL += "    ,GBSUNAP,TUYAKNO,SCODESAYU,SCODEREMARK,RESV,Sabun                       \r\n";
            }
            else if (argCls.GbIO == "I" || argCls.GbIO == "E")
            {
                if (argCls.GbIO == "E")
                {
                    SQL += "    ,staffid,WardCode,RoomCode,GBSTATUS,GbPRN, GBIOE, ORDERSITE, GBACT                                \r\n";
                }
                else
                {
                    SQL += "    ,staffid,WardCode,RoomCode,GBSTATUS,GbPRN, IP                                \r\n";
                }
            }

            SQL += "    , GBSEND  )    VALUES                                                       \r\n";
            SQL += "   (                                                                            \r\n";
            SQL += "     '" + argCls.PTNO + "'                                                      \r\n";
            SQL += "     ,TO_DATE('" + argCls.BDATE + "','YYYY-MM-DD')                              \r\n";
            SQL += "     ,'" + argCls.DEPTCODE + "'                                                 \r\n";
            SQL += "     ,'" + argCls.SEQNO + "'                                                    \r\n";
            SQL += "     ,'" + argCls.ORDERCODE + "'                                                \r\n";
            SQL += "     ,'" + argCls.SUCODE + "'                                                   \r\n";
            SQL += "     ,'" + argCls.BUN + "'                                                      \r\n";
            SQL += "     ,'" + argCls.SLIPNO + "'                                                   \r\n";
            SQL += "     ,'" + argCls.RealQTY + "'                                                  \r\n";
            SQL += "     ,'" + argCls.QTY + "'                                                      \r\n";
            SQL += "     ,'" + argCls.NAL + "'                                                      \r\n";
            SQL += "     ,'" + argCls.GBDIV + "'                                                    \r\n";
            SQL += "     ,'" + argCls.DOSCODE + "'                                                  \r\n";
            SQL += "     ,'" + argCls.GBBOTH + "'                                                   \r\n";
            SQL += "     ,'" + argCls.GBINFO + "'                                                   \r\n";
            SQL += "     ,'" + argCls.GBER + "'                                                     \r\n";
            SQL += "     ,'" + argCls.GBSELF + "'                                                   \r\n";
            SQL += "     ,'" + argCls.GBSPC + "'                                                    \r\n";
            SQL += "     ,'" + argCls.BI + "'                                                       \r\n";
            if (argCls.GbIO == "O")
            {
                SQL += "     ,'" + argCls.DRCODE + "'                                                   \r\n";
            }
            else if (argCls.GbIO == "I" || argCls.GbIO == "E")
            {
                SQL += "     ,'" + argCls.StaffID + "'                                                   \r\n";
            }
            if (argCls.GbIO == "E")
            {
                SQL += "     ,''                                                   \r\n";
            }
            else
            {
                SQL += "     ,'" + argCls.REMARK + "'                                                   \r\n";
            }
                
            SQL += "     ,SYSDATE                                                                   \r\n";            
            SQL += "     ," + argCls.ORDERNO + "                                                    \r\n";
            SQL += "     ,'" + argCls.MULTI + "'                                                    \r\n";
            SQL += "     ,'" + argCls.MULTIREMARK + "'                                              \r\n";
            SQL += "     ,'" + argCls.DUR + "'                                                      \r\n";                        

            if (argCls.GbIO == "O")
            {
                SQL += "     ,'" + argCls.GBSUNAP + "'                                              \r\n";
                SQL += "     ,'" + argCls.TUYAKNO + "'                                              \r\n";
                SQL += "     ,'" + argCls.SCODESAYU + "'                                            \r\n";
                SQL += "     ,'" + argCls.SCODEREMARK + "'                                          \r\n";
                SQL += "     ,'" + argCls.RESV + "'                                                 \r\n";
                SQL += "     ,'" + argCls.StaffID + "'                                              \r\n";
                SQL += "     ,'" + argCls.GBSEND + "'                                                   \r\n";
            }
            else if (argCls.GbIO == "I" || argCls.GbIO == "E")
            {
                if (argCls.GbIO == "E")
                {
                    SQL += "     ,'" + argCls.DRCODE + "'                                               \r\n";
                    SQL += "     ,'" + argCls.WardCode + "'                                             \r\n";
                    SQL += "     ,'" + argCls.RoomCode + "'                                             \r\n";
                    SQL += "     ,'" + argCls.GbStatus + "'                                             \r\n";
                    SQL += "     ,'" + argCls.GbPRN + "'                                                \r\n";
                    SQL += "     ,'E'                                                \r\n";
                    SQL += "     ,'TEL'                                                \r\n";
                    SQL += "     ,'*'                                                \r\n";
                    SQL += "     ,'*'                                                \r\n";
                }
                else
                {
                    SQL += "     ,'" + argCls.DRCODE + "'                                               \r\n";
                    SQL += "     ,'" + argCls.WardCode + "'                                             \r\n";
                    SQL += "     ,'" + argCls.RoomCode + "'                                             \r\n";
                    SQL += "     ,'" + argCls.GbStatus + "'                                             \r\n";
                    SQL += "     ,'" + argCls.GbPRN + "'                                                \r\n";
                    SQL += "     ,'" + clsCompuInfo.gstrCOMIP + "'                                                \r\n";
                    SQL += "     ,'" + argCls.GBSEND + "'                                                   \r\n";
                }
            }
            SQL += "   )                                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
        
        public class cXray_Use
        {
            public string Job = "";
            public long MgrNo = 0;
            public string XCode = "";
            public string XRoom = "";
            public string MCode = "";
            public int Qty = 0;
            public int GbUse = 0;

            public string Pano = "";
            public long OrderNo = 0;
            public string PacsNo = "";
            public int Gisa = 0;
            public int Buwi = 0;
            public string SeekDate = "";
            
            public string ROWID = "";
        }
        
        public string ins_XRAY_USE(PsmhDb pDbCon, cXray_Use argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";

            SQL = " INSERT INTO " + ComNum.DB_PMPA + "XRAY_USE                                      \r\n";
            SQL += "   (MGRNO, SEEKDATE, XCODE, MCODE, QTY, GBUSE) VALUES                           \r\n";
            SQL += "   (                                                                            \r\n";            
            SQL += "      " + argCls.MgrNo + "                                                      \r\n";
            SQL += "     ,TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD')                           \r\n";
            SQL += "     ,'" + argCls.XCode + "'                                                    \r\n";
            SQL += "     ,'" + argCls.MCode + "'                                                    \r\n";
            SQL += "     ,'" + argCls.Qty + "'                                                      \r\n";
            SQL += "     ," + argCls.GbUse + "                                                      \r\n";            
            SQL += "   )                                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string del_XRAY_USE(PsmhDb pDbCon, cXray_Use argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
             
            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_PMPA + "XRAY_USE                                      \r\n";
            SQL += "  WHERE 1=1                                                                     \r\n";
            if (argCls.Job =="00")
            {
                SQL += "   AND MgrNo = " + argCls.MgrNo + "                                         \r\n";
                SQL += "   AND SeekDate = TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD')           \r\n";
            }
            else if (argCls.Job == "01")
            {            
                SQL += "   AND ROWID = '" + argCls.ROWID + "'                                       \r\n";               
            }
            else
            {            
                SQL += "   AND ROWID = '" + argCls.ROWID + "'                                       \r\n";                
            }            

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }                

        public string up_XRAY_PACS_ORDER(PsmhDb pDbCon, cXray_Pacs_Order argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PACS + "XRAY_PACS_ORDER  SET                              \r\n";
            SQL += "   FALG = '" + argCls.FALG + "'                                                 \r\n";
            SQL += "  ,Operator = '" + argCls.Operator + "'                                         \r\n";            
            SQL += "  WHERE 1=1                                                                     \r\n";
            SQL += "    AND PatID = '" + argCls.Patid + "'                                          \r\n";
            SQL += "    AND ACDESSIONNO = '" + argCls.ACDESSIONNO + "'                              \r\n";
            //SQL += "    AND WorkTime >=TRUNC(SYSDATE-7)                                             \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
             
            return SqlErr;
        }

        public class cXray_Ilgi
        {
            public string Job = "";

            public string BDATE = "";
            public string SUNAME = "";
            public string CTNAME = "";
            public string MRINAME = "";
            public string RINAME = "";
            public string SONONAME = "";
            public string DDRNAME = "";
            public string ROOMNAME1 = "";
            public string ROOMNAME2 = "";
            public string ROOMNAME3 = "";
            public string ROOMNAME4 = "";
            public string ROOMNAME5 = "";
            public string ENAME = "";
            public string NNAME = "";
            public string WORK = "";
            public string WORK1 = "";
            public string WORK2 = "";
            public string WORK3 = "";
            public string WORK4 = "";
            public string BUSEROOM = "";
            public string BUSEROOM1 = "";
            public string ITEM = "";
            public string ITEM1 = "";
            public string HUNAME = "";
            public string GONAME = "";
            public string SSAIN1 = "";
            public string SSAIN2 = "";

            public string ROWID = "";

        }

        public string ins_XRAY_ILGI(PsmhDb pDbCon, cXray_Ilgi argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";

            SQL = " INSERT INTO " + ComNum.DB_PMPA + "XRAY_ILGI                                         \r\n";
            SQL += "   (BDATE,SUNAME,CTNAME,MRINAME,RINAME,SONONAME,DDRNAME,ROOMNAME1,ROOMNAME2         \r\n";
            SQL += "    ,ROOMNAME3,ROOMNAME4,ROOMNAME5,ENAME,NNAME,WORK,WORK1,WORK2,WORK3,WORK4         \r\n";
            SQL += "    ,BUSEROOM,BUSEROOM1,ITEM,ITEM1,HUNAME,GONAME,SSAIN1,SSAIN2 )  VALUES            \r\n";            
            SQL += "   (                                                                                \r\n";
            SQL += "      TO_DATE('" + argCls.BDATE + "','YYYY-MM-DD')                                  \r\n";            
            SQL += "     ,'" + argCls.SUNAME + "'                                                       \r\n";
            SQL += "     ,'" + argCls.CTNAME + "'                                                       \r\n";
            SQL += "     ,'" + argCls.MRINAME + "'                                                      \r\n";
            SQL += "     ,'" + argCls.RINAME + "'                                                       \r\n";
            SQL += "     ,'" + argCls.SONONAME + "'                                                     \r\n";
            SQL += "     ,'" + argCls.DDRNAME + "'                                                      \r\n";
            SQL += "     ,'" + argCls.ROOMNAME1 + "'                                                    \r\n";
            SQL += "     ,'" + argCls.ROOMNAME2 + "'                                                    \r\n";
            SQL += "     ,'" + argCls.ROOMNAME3 + "'                                                    \r\n";
            SQL += "     ,'" + argCls.ROOMNAME4 + "'                                                    \r\n";
            SQL += "     ,'" + argCls.ROOMNAME5 + "'                                                    \r\n";
            SQL += "     ,'" + argCls.ENAME + "'                                                        \r\n";
            SQL += "     ,'" + argCls.NNAME + "'                                                        \r\n";
            SQL += "     ,'" + argCls.WORK + "'                                                         \r\n";
            SQL += "     ,'" + argCls.WORK1 + "'                                                        \r\n";
            SQL += "     ,'" + argCls.WORK2+ "'                                                         \r\n";
            SQL += "     ,'" + argCls.WORK3 + "'                                                        \r\n";
            SQL += "     ,'" + argCls.WORK4 + "'                                                        \r\n";
            SQL += "     ,'" + argCls.BUSEROOM + "'                                                     \r\n";
            SQL += "     ,'" + argCls.BUSEROOM1 + "'                                                    \r\n";
            SQL += "     ,'" + argCls.ITEM + "'                                                         \r\n";
            SQL += "     ,'" + argCls.ITEM1 + "'                                                        \r\n";
            SQL += "     ,'" + argCls.HUNAME + "'                                                       \r\n";
            SQL += "     ,'" + argCls.GONAME + "'                                                       \r\n";
            SQL += "     ,'" + argCls.SSAIN1 + "'                                                       \r\n";
            SQL += "     ,'" + argCls.SSAIN2 + "'                                                       \r\n";
            SQL += "   )                                                                                \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_XRAY_ILGI(PsmhDb pDbCon, cXray_Ilgi argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_ILGI  SET                    \r\n";
            SQL += "   BDATE      = TO_DATE('" + argCls.BDATE + "','YYYY-MM-DD')    \r\n";
            SQL += "  ,SUNAME     = '" + argCls.SUNAME + "'                         \r\n";
            SQL += "  ,CTNAME     = '" + argCls.CTNAME + "'                         \r\n";
            SQL += "  ,MRINAME    = '" + argCls.MRINAME + "'                        \r\n";
            SQL += "  ,RINAME     = '" + argCls.RINAME + "'                         \r\n";
            SQL += "  ,SONONAME   = '" + argCls.SONONAME + "'                       \r\n";
            SQL += "  ,DDRNAME    = '" + argCls.DDRNAME + "'                        \r\n";
            SQL += "  ,ROOMNAME1  = '" + argCls.ROOMNAME1 + "'                      \r\n";
            SQL += "  ,ROOMNAME2  = '" + argCls.ROOMNAME2 + "'                      \r\n";
            SQL += "  ,ROOMNAME3  = '" + argCls.ROOMNAME3 + "'                      \r\n";
            SQL += "  ,ROOMNAME4  = '" + argCls.ROOMNAME4 + "'                      \r\n";
            SQL += "  ,ROOMNAME5  = '" + argCls.ROOMNAME5 + "'                      \r\n";
            SQL += "  ,ENAME      = '" + argCls.ENAME + "'                          \r\n";
            SQL += "  ,NNAME      = '" + argCls.NNAME + "'                          \r\n";
            SQL += "  ,WORK       = '" + argCls.WORK + "'                           \r\n";
            SQL += "  ,WORK1      = '" + argCls.WORK1 + "'                          \r\n";
            SQL += "  ,WORK2      = '" + argCls.WORK2 + "'                          \r\n";
            SQL += "  ,WORK3      = '" + argCls.WORK3 + "'                          \r\n";
            SQL += "  ,WORK4      = '" + argCls.WORK4 + "'                          \r\n";
            SQL += "  ,BUSEROOM   = '" + argCls.BUSEROOM + "'                       \r\n";
            SQL += "  ,BUSEROOM1  = '" + argCls.BUSEROOM1 + "'                      \r\n";
            SQL += "  ,ITEM       = '" + argCls.ITEM + "'                           \r\n";
            SQL += "  ,ITEM1      = '" + argCls.ITEM1 + "'                          \r\n";
            SQL += "  ,HUNAME     = '" + argCls.HUNAME + "'                         \r\n";
            SQL += "  ,GONAME     = '" + argCls.GONAME + "'                         \r\n";
            SQL += "  ,SSAIN1     = '" + argCls.SSAIN1 + "'                         \r\n";
            SQL += "  ,SSAIN2     = '" + argCls.SSAIN2 + "'                         \r\n";
            SQL += "  WHERE 1=1                                                     \r\n";
            SQL += "    AND ROWID = '" + argCls.ROWID + "'                          \r\n";           

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public class cXray_Ilgi_Angio
        {
            public string Job = "";

            public string BDate = "";
            public string Call = "";
            public string Call2 = "";
            public string Remark = "";
            
            public string ROWID = "";
        }

        public string ins_XRAY_ILGI_ANGIO(PsmhDb pDbCon, cXray_Ilgi_Angio argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";

            SQL = " INSERT INTO " + ComNum.DB_PMPA + "XRAY_ILGI_ANGIO       \r\n";
            SQL += "   ( BDATE,REMARK,CALL,CALL2 )  VALUES                  \r\n";
            SQL += "   (                                                    \r\n";
            SQL += "      TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')       \r\n";
            SQL += "     ,'" + argCls.Remark + "'                           \r\n";
            SQL += "     ,'" + argCls.Call + "'                             \r\n";
            SQL += "     ,'" + argCls.Call2 + "'                            \r\n";
            SQL += "   )                                                    \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_XRAY_ILGI_ANGIO(PsmhDb pDbCon, cXray_Ilgi_Angio argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_ILGI_ANGIO  SET              \r\n";
            SQL += "   BDATE      = TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')    \r\n";
            SQL += "  ,REMARK     = '" + argCls.Remark + "'                         \r\n";
            SQL += "  ,CALL     = '" + argCls.Call + "'                             \r\n";
            SQL += "  ,CALL2    = '" + argCls.Call2 + "'                            \r\n";           
            SQL += "  WHERE 1=1                                                     \r\n";
            SQL += "    AND ROWID = '" + argCls.ROWID + "'                          \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public class cXray_Ilgi_Angio_Sub
        {
            public string Job = "";

            public string Code = "";
            public string Date1 = "";
            public string Date2 = "";
            public string BDate = "";
            public string DamName = "";
            public string SDate = "";
            public string EntDate = "";
            public string Call_Time1 = "";
            public string Call_Time2 = "";            
            public string Remark = "";

            public string ROWID = "";
        }

        public string ins_XRAY_ILGI_ANGIO_SUB(PsmhDb pDbCon, cXray_Ilgi_Angio_Sub argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL = " INSERT INTO " + ComNum.DB_PMPA + "XRAY_ILGI_ANGIO_SUB                           \r\n";
            SQL += "   ( BDATE,CODE,DAMNAME,SDATE,ENTDATE,CALL_TIME1,CALL_TIME2,REMARK )  VALUES    \r\n";
            SQL += "   (                                                                            \r\n";
            SQL += "      TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                              \r\n";
            SQL += "     ,'" + argCls.Code + "'                                                     \r\n";
            SQL += "     ,'" + argCls.DamName + "'                                                  \r\n";
            SQL += "     ,SYSDATE                                                                   \r\n";
            SQL += "     ,SYSDATE                                                                   \r\n";
            SQL += "     ,TO_DATE('" + argCls.Call_Time1 + "','YYYY-MM-DD HH24:MI')                 \r\n";
            SQL += "     ,TO_DATE('" + argCls.Call_Time2 + "','YYYY-MM-DD HH24:MI')                 \r\n";
            SQL += "     ,'" + argCls.Remark + "'                                                   \r\n";            
            SQL += "   )                                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_XRAY_ILGI_ANGIO_SUB(PsmhDb pDbCon, cXray_Ilgi_Angio_Sub argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_ILGI_ANGIO_SUB  SET                          \r\n";
            if (argCls.Job =="00")
            {
                SQL += "  DAMNAME     = '" + argCls.DamName + "'                                    \r\n";
                SQL += "  ,CALL_TIME1 = TO_DATE('" + argCls.Call_Time1 + "','YYYY-MM-DD HH24:MI')   \r\n";
                SQL += "  ,CALL_TIME2 = TO_DATE('" + argCls.Call_Time2 + "','YYYY-MM-DD HH24:MI')   \r\n";
                SQL += "  ,REMARK     = '" + argCls.Remark + "'                                     \r\n";
                SQL += "  ,ENTDATE = SYSDATE                                                        \r\n";
            }
            else if (argCls.Job == "01")
            {
                SQL += "   DELDATE = TRUNC(SYSDATE)                                                 \r\n";              
            }
            
            SQL += "  WHERE 1=1                                                                     \r\n";
            SQL += "    AND ROWID = '" + argCls.ROWID + "'                                          \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
                
        public class cBas_Z300Font
        {
            public string Job = "";
            public string Search = "";
            public string Z300CODE = "";
            public string Z300FONT = "";
            public string ENGNAME = "";
            public string ENGNAME_old = "";
            public string ROWID = "";

        }

        public string up_BAS_Z300FONT(PsmhDb pDbCon, cBas_Z300Font argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "BAS_Z300FONT  SET                 \r\n";            
            SQL += "  EngName     = '" + argCls.ENGNAME + "'                        \r\n";                  
            SQL += "  WHERE 1=1                                                     \r\n";
            SQL += "    AND ROWID = '" + argCls.ROWID + "'                          \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public class cEtc_PCSet
        {
            public string Job = "";
            public long Sabun = 0;
            public string FLAG_PRT = "0"; //프린트 : 판독결과 저장후 >  0.인쇄안됨 1.자동인쇄
            public string FLAG_CAN = "0"; //취소   :판독취소후 > 0.표시안함 1.표시함
            public string FLAG_SAVE = "3"; //저장   :판독결과 등록후 > 1.일자별명단 2.등록번호별 명단 3.현재화면 
            public string FLAG_VIEW = "Y"; //자동조회 : 판독결과 등록후 명단을 재갱신 여부
            public string FLAG_PACSVIEW = "N"; //팍스VIEW : 환자선택후 팍스영상 자동 조회
            public string FLAG_LINK_YN = "N"; //인피니트 링크여부
            public string FLAG_LINK_READ = "N"; //링크 -> 판독화면
            public string FLAG_LINK_RESULT = "N"; //링크 -> 기타결과

            public string ROWID = "";

        }

        public string ins_ETC_PCSET(PsmhDb pDbCon, cEtc_PCSet argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL = " INSERT INTO " + ComNum.DB_PMPA + "ETC_PCSET                     \r\n";
            SQL += "   ( Sabun,READ_Flag1,READ_Flag2,READ_Flag3                     \r\n";
            SQL += "    ,READ_Flag4,READ_Flag5,READ_Flag6                           \r\n";
            SQL += "    ,READ_Flag7,READ_Flag8 ) VALUES                             \r\n";
            SQL += "   (                                                            \r\n";                        
            SQL += "     " + argCls.Sabun + "                                       \r\n";
            SQL += "     ,'" + argCls.FLAG_PRT + "'                                 \r\n";
            SQL += "     ,'" + argCls.FLAG_CAN + "'                                 \r\n";
            SQL += "     ,'" + argCls.FLAG_SAVE + "'                                \r\n";
            SQL += "     ,'" + argCls.FLAG_VIEW + "'                                \r\n";
            SQL += "     ,'" + argCls.FLAG_PACSVIEW + "'                            \r\n";
            SQL += "     ,'" + argCls.FLAG_LINK_YN + "'                             \r\n";
            SQL += "     ,'" + argCls.FLAG_LINK_READ + "'                           \r\n";
            SQL += "     ,'" + argCls.FLAG_LINK_RESULT + "'                         \r\n";
            SQL += "   )                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_ETC_PCSET(PsmhDb pDbCon, cEtc_PCSet argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "ETC_PCSET  SET                    \r\n";            
            SQL += "  READ_FLAG1     = '" + argCls.FLAG_PRT + "'                    \r\n";                
            SQL += "  ,READ_FLAG2     = '" + argCls.FLAG_CAN + "'                   \r\n";
            SQL += "  ,READ_FLAG3     = '" + argCls.FLAG_SAVE + "'                  \r\n";
            SQL += "  ,READ_FLAG4     = '" + argCls.FLAG_VIEW + "'                  \r\n";
            SQL += "  ,READ_FLAG5     = '" + argCls.FLAG_PACSVIEW + "'              \r\n";
            SQL += "  ,READ_FLAG6     = '" + argCls.FLAG_LINK_YN + "'               \r\n";
            SQL += "  ,READ_FLAG7     = '" + argCls.FLAG_LINK_READ + "'             \r\n";
            SQL += "  ,READ_FLAG8     = '" + argCls.FLAG_LINK_RESULT + "'           \r\n";            
            SQL += "  WHERE 1=1                                                     \r\n";
            SQL += "    AND ROWID = '" + argCls.ROWID + "'                          \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }


        public class cXray_ASA
        {            
            public string Job ="";
            public string Pano = "";
            public string BDate = "";
            public string DeptCode = "";
            public string DrCode = "";
            public string WardCode = "";
            public string Remark = "";
            public string Remark2 = "";
            public string strTemp = "";
            public string ROWID = "";
            public string Gubun = "";   //01:ASA, 02:조영제
        }

        public string ins_XRAY_ASA(PsmhDb pDbCon, cXray_ASA argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL = " INSERT INTO " + ComNum.DB_PMPA + "XRAY_ASA                      \r\n";
            SQL += "   ( PANO,BDATE,DEPTCODE,DRCODE                                 \r\n";
            SQL += "    ,WARDCODE,REMARK,Remark2,GUBUN)  VALUES                     \r\n";
            SQL += "   (                                                            \r\n";
            SQL += "     '" + argCls.Pano + "'                                      \r\n";
            SQL += "     ,TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')              \r\n";
            SQL += "     ,'" + argCls.DeptCode + "'                                 \r\n";
            SQL += "     ,'" + argCls.DrCode + "'                                   \r\n";
            SQL += "     ,'" + argCls.WardCode + "'                                 \r\n";
            SQL += "     ,'" + argCls.Remark + "'                                   \r\n";
            SQL += "     ,'" + argCls.Remark2 + "'                                  \r\n";
            SQL += "     ,'" + argCls.Gubun + "'                                    \r\n";
            SQL += "   )                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_XRAY_ASA(PsmhDb pDbCon, cXray_ASA argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argCls.ROWID =="")
            {
                return "갱신오류";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_ASA  SET                     \r\n";
            SQL += "  PANO     = '" + argCls.Pano + "'                              \r\n";
            SQL += "  ,DEPTCODE     = '" + argCls.DeptCode + "'                     \r\n";
            SQL += "  ,DRCODE     = '" + argCls.DrCode + "'                         \r\n";            
            SQL += "  ,WARDCODE     = '" + argCls.WardCode + "'                     \r\n";
            SQL += "  ,REMARK     = '" + argCls.Remark + "'                         \r\n";
            SQL += "  ,REMARK2     = '" + argCls.Remark2 + "'                       \r\n";
            SQL += "  ,GUBUN     = '" + argCls.Gubun + "'                           \r\n";
            SQL += "  WHERE 1=1                                                     \r\n";
            SQL += "    AND ROWID = '" + argCls.ROWID + "'                          \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string del_XRAY_ASA(PsmhDb pDbCon, cXray_ASA argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argCls.ROWID == "")
            {
                return "자료삭제 오류!!";
            }

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_PMPA + "XRAY_ASA                 \r\n";
            
            SQL += "  WHERE 1=1                                                 \r\n";
            if (argCls.ROWID != "")
            {
                SQL += "    AND ROWID = '" + argCls.ROWID + "'                  \r\n";
            }
            
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        #endregion
         
    }
}
