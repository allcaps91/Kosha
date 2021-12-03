using ComBase.Controls;
using ComDbB; //기본 클래스
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComBase
{
    /// <summary>
    /// TODO : OrderEtc.bas
    /// 처방 관련 모듈
    /// </summary>
    public class clsOrderEtc : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public static string GstrNewOrderI;
        public static string GstrNewOrderU;
        public static string GstrNewOrderD;
        
        /// <summary>
        /// TODO : OrderEtc.bas : CHECK_PNEUMONIA_ILL
        /// </summary>
        /// <returns></returns>
        public static bool CHECK_PNEUMONIA_ILL(PsmhDb pDbCon, string strCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     CODE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'OCS_폐렴대상_상병코드'";
                SQL = SQL + ComNum.VBLF + "         AND CODE IN (SELECT ILLCODED FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                SQL = SQL + ComNum.VBLF + "                             WHERE ILLCODE = '" + strCode + "')";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = true;
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHECK_PNEUMONIA
        /// </summary>
        /// <returns></returns>
        public static bool CHECK_PNEUMONIA(PsmhDb pDbCon, string strPtNo, int intIpdNo, string strPneumonia)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            bool rtnVal = false;

            try
            {
                //추가 보완 사항 완료되면 함께 시행
                if (strPneumonia == "N")
                {
                    return rtnVal;
                }

                if (strPtNo == "")
                {
                    return rtnVal;
                }

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     ILLCODE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IILLS";
                SQL = SQL + ComNum.VBLF + "     WHERE PTNO = '" + strPtNo + "'";
                SQL = SQL + ComNum.VBLF + "         AND IPDNO = " + intIpdNo;
                SQL = SQL + ComNum.VBLF + "         AND ILLCODE IN (SELECT A.ILLCODE FROM " + ComNum.DB_PMPA + "BAS_ILLS A, " + ComNum.DB_PMPA + "BAS_BCODE B";
                SQL = SQL + ComNum.VBLF + "                                 WHERE A.ILLCODED = B.CODE";
                SQL = SQL + ComNum.VBLF + "                                     AND B.GUBUN = 'OCS_폐렴대상_상병코드')";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }

                dt.Dispose();
                dt = null;
                
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : INFECT_MSGSEND_HIV
        /// </summary>
        /// <returns></returns>
        public static void INFECT_MSGSEND_HIV(PsmhDb pDbCon, string strPtNo, string strWard, string strRoom, string strName, int intSabun)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;

            string strTel = "";
            string strMsg = "";
            string strDrName = "";
            int intRowAffected = 0; //변경된 Row 받는 변수

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL = " SELECT USERNAME FROM " + ComNum.DB_PMPA + "BAS_USER";
                SQL = SQL + ComNum.VBLF + "     WHERE IDNUMBER = " + intSabun;

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    strDrName = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                reader = null;
                 
                strTel = "01066052045";

                strMsg = "";
                strMsg = "HIV 신고서(";
                strMsg = strMsg + strDrName;
                strMsg = strMsg + ") 접수/" + strPtNo + "/" + strName + "/" + strWard + "(" + strRoom + ")/" + strDrName;

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel,SendTime,SendMsg)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         SYSDATE , ";
                SQL = SQL + ComNum.VBLF + "         '" + strPtNo + "', ";
                SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                SQL = SQL + ComNum.VBLF + "         '" + strTel + "', ";
                SQL = SQL + ComNum.VBLF + "         '22', ";
                SQL = SQL + ComNum.VBLF + "         '', ";
                SQL = SQL + ComNum.VBLF + "         '', ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE,";           //구분22 감염등록
                SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                SQL = SQL + ComNum.VBLF + "         '', ";
                SQL = SQL + ComNum.VBLF + "         '" + strMsg + "' ";
                SQL = SQL + ComNum.VBLF + "     ) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                strTel = "01065213013";

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS ";
                SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         SYSDATE,";
                SQL = SQL + ComNum.VBLF + "         '" + strPtNo + "', ";
                SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                SQL = SQL + ComNum.VBLF + "         '" + strTel + "', ";
                SQL = SQL + ComNum.VBLF + "         '22', ";
                SQL = SQL + ComNum.VBLF + "         '', ";
                SQL = SQL + ComNum.VBLF + "         '', ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE,";           //구분22 감염등록
                SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                SQL = SQL + ComNum.VBLF + "         '', ";
                SQL = SQL + ComNum.VBLF + "         '" + strMsg + "' ";
                SQL = SQL + ComNum.VBLF + "     ) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //2018-10-01 안정수, 감염관리 요청으로 박수진 추가

                strTel = "01028170176";

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS ";
                SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         SYSDATE,";
                SQL = SQL + ComNum.VBLF + "         '" + strPtNo + "', ";
                SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                SQL = SQL + ComNum.VBLF + "         '" + strTel + "', ";
                SQL = SQL + ComNum.VBLF + "         '22', ";
                SQL = SQL + ComNum.VBLF + "         '', ";
                SQL = SQL + ComNum.VBLF + "         '', ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE,";           //구분22 감염등록
                SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                SQL = SQL + ComNum.VBLF + "         '', ";
                SQL = SQL + ComNum.VBLF + "         '" + strMsg + "' ";
                SQL = SQL + ComNum.VBLF + "     ) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon); 

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : INFECT_MSGSEND
        /// </summary>
        /// <returns></returns>
        public static void INFECT_MSGSEND(PsmhDb pDbCon, string strINFECT1, string strINFECT2, string strINFECT3, string strINFECT4, string strINFECT4ETC, 
                                string strINFECT5, string strINFECT5ETC, string strPTNO, string strWard, string strRoom, string strName, string strSABUN, 
                                string strDrname, string strJumin, string strTel, string strJobName, string strJuso, 
                                string strBDATE, string strJDate, string strSDate, string strExResult, string strGbio, string strPatBun, string strSaMang)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strHTEL = "";
            string strMSG  = "";

            if (strExResult == "0") { strExResult = "양성"; }
            if (strExResult == "1") { strExResult = "음성"; }
            if (strExResult == "2") { strExResult = "검사진행중"; }
            if (strExResult == "3") { strExResult = "검사미실시"; }

            if (strGbio == "1") { strGbio = "외래"; }
            if (strGbio == "2") { strGbio = "입원"; }
            if (strGbio == "3") { strGbio = "기타"; }

            if (strPatBun == "0") { strPatBun = "환자"; }
            if (strPatBun == "1") { strPatBun = "의사환자"; }
            if (strPatBun == "2") { strPatBun = "병원체보유자"; }

            if (strSaMang == "0") { strSaMang = "생존"; }
            if (strSaMang == "1") { strSaMang = "사망"; }

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     USERNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_USER";
                SQL = SQL + ComNum.VBLF + "     WHERE SABUN = '" + strSABUN + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strDrname = dt.Rows[0]["USERNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                
                strHTEL = "01066052045";

                if (strINFECT1.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT1.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "1군법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE,";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT2.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT2.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "2군법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "','감염등록','" + strHTEL + "','22','','',SYSDATE,";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "'054-260-8019','','" + strMSG + "') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT3.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_법정전염병'";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT3.Trim() + "'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "3군법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT4.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT4.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "4군법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT4ETC.Trim() != "")
                {
                    strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                    strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                    strMSG = strMSG + "4군신종(" + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                    strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                    SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                    SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                    SQL = SQL + ComNum.VBLF + "         '22', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                    SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                if (strINFECT5.Trim() == "Y")
                {
                    strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                    strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                    strMSG = strMSG + "지정감염병(" + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                    strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                    SQL = "";
                    SQL = "INSERT INTO KOSMOS_PMPA.ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime,";
                    SQL = SQL + "   RetTel,SendTime,SendMsg) VALUES ( SYSDATE ,'";
                    SQL = SQL + strPTNO + "','감염등록','" + strHTEL + "','22','','',SYSDATE,";           //구분22 감염등록
                    SQL = SQL + "'054-260-8019','','" + strMSG + "') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                //2019-04-24 안정수, 정지미 추가 

                strHTEL = "01065827819";

                if (strINFECT1.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT1.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "1군법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE,";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT2.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_법정전염병'";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT2.Trim() + "'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "2군법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT3.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT3.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "3군법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "INSERT INTO KOSMOS_PMPA.ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime,";
                        SQL = SQL + "   RetTel,SendTime,SendMsg) VALUES ( SYSDATE ,'";
                        SQL = SQL + strPTNO + "','감염등록','" + strHTEL + "','22','','',SYSDATE,";           //구분22 감염등록
                        SQL = SQL + "'054-260-8019','','" + strMSG + "') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT4.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_법정전염병'";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT4.Trim() + "'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "4군법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT4ETC.Trim() != "")
                {
                    strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                    strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                    strMSG = strMSG + "4군신종(" + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                    strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                    SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                    SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                    SQL = SQL + ComNum.VBLF + "         '22', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                    SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                if (strINFECT5.Trim() == "Y")
                {
                    strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                    strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                    strMSG = strMSG + "지정감염병(" + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                    strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                    SQL = SQL + ComNum.VBLF + "     (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime,RetTel,SendTime,SendMsg)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                    SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                    SQL = SQL + ComNum.VBLF + "         '22', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                    SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                strHTEL = "01027764163";

                if (strINFECT1.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT1.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "1군법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE,";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT2.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_법정전염병'";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT2.Trim() + "'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "2군법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT3.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT3.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "3군법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime,RetTel,SendTime,SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT4.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT4.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "4군법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT4ETC.Trim() != "")
                {
                    strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                    strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                    strMSG = strMSG + "4군신종(" + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                    strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                    SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                    SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                    SQL = SQL + ComNum.VBLF + "         '22', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE,";           //구분22 감염등록
                    SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                if (strINFECT5.Trim() == "Y")
                {
                    strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                    strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                    strMSG = strMSG + "지정감염병(" + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                    strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                    SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                    SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                    SQL = SQL + ComNum.VBLF + "         '22', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE,";           //구분22 감염등록
                    SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                //2018-10-08 안정수, 감염관리팀 직원 추가   
                strHTEL = "01028170176";

                if (strINFECT1.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT1.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "1군법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE,";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT2.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_법정전염병'";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT2.Trim() + "'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "2군법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT3.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT3.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "3군법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime,RetTel,SendTime,SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT4.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT4.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "4군법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT4ETC.Trim() != "")
                {
                    strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                    strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                    strMSG = strMSG + "4군신종(" + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                    strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                    SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                    SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                    SQL = SQL + ComNum.VBLF + "         '22', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE,";           //구분22 감염등록
                    SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                if (strINFECT5.Trim() == "Y")
                {
                    strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                    strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                    strMSG = strMSG + "지정감염병(" + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                    strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                    SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                    SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                    SQL = SQL + ComNum.VBLF + "         '22', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE,";           //구분22 감염등록
                    SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(pDbCon);
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        public static void INFECT_MSGSEND_NEW(PsmhDb pDbCon, string strINFECT1, string strINFECT1ETC, string strINFECT2, string strINFECT3, string strINFECT4, string strINFECT4ETC,
                                string strINFECT5, string strINFECT5ETC, string strPTNO, string strWard, string strRoom, string strName, string strSABUN,
                                string strDrname, string strJumin, string strTel, string strJobName, string strJuso,
                                string strBDATE, string strJDate, string strSDate, string strExResult, string strGbio, string strPatBun, string strSaMang)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strHTEL = "";
            string strMSG = "";

            if (strExResult == "0") { strExResult = "양성"; }
            if (strExResult == "1") { strExResult = "음성"; }
            if (strExResult == "2") { strExResult = "검사진행중"; }
            if (strExResult == "3") { strExResult = "검사미실시"; }

            if (strGbio == "1") { strGbio = "외래"; }
            if (strGbio == "2") { strGbio = "입원"; }
            if (strGbio == "3") { strGbio = "기타"; }

            if (strPatBun == "0") { strPatBun = "환자"; }
            if (strPatBun == "1") { strPatBun = "의사환자"; }
            if (strPatBun == "2") { strPatBun = "병원체보유자"; }

            if (strSaMang == "0") { strSaMang = "생존"; }
            if (strSaMang == "1") { strSaMang = "사망"; }

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     USERNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_USER";
                SQL = SQL + ComNum.VBLF + "     WHERE SABUN = '" + strSABUN + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strDrname = dt.Rows[0]["USERNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                strHTEL = "01066052045";

                if (strINFECT1.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFECT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT1.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "제1급법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE,";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT1ETC.Trim() != "")
                {
                    strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                    strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                    strMSG = strMSG + "제1급신종(" + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                    strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                    SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                    SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                    SQL = SQL + ComNum.VBLF + "         '22', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                    SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                if (strINFECT2.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFECT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT2.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "제2급법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "','감염등록','" + strHTEL + "','22','','',SYSDATE,";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "'054-260-8019','','" + strMSG + "') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT3.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFECT_법정전염병'";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT3.Trim() + "'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "제3급법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }                

                //2019-04-24 안정수, 정지미 추가 
                strHTEL = "01065827819";

                if (strINFECT1.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFECT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT1.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "제1급법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE,";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT1ETC.Trim() != "")
                {
                    strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                    strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                    strMSG = strMSG + "제1급신종(" + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                    strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                    SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                    SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                    SQL = SQL + ComNum.VBLF + "         '22', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                    SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                if (strINFECT2.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFECT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT2.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "제2급법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "','감염등록','" + strHTEL + "','22','','',SYSDATE,";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "'054-260-8019','','" + strMSG + "') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT3.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFECT_법정전염병'";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT3.Trim() + "'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "제3급법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                strHTEL = "01027764163";
                if (strINFECT1.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFECT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT1.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "제1급법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE,";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT1ETC.Trim() != "")
                {
                    strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                    strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                    strMSG = strMSG + "제1급신종(" + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                    strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                    SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                    SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                    SQL = SQL + ComNum.VBLF + "         '22', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                    SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                if (strINFECT2.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFECT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT2.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "제2급법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "','감염등록','" + strHTEL + "','22','','',SYSDATE,";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "'054-260-8019','','" + strMSG + "') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT3.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFECT_법정전염병'";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT3.Trim() + "'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "제3급법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
               
                //2018-10-08 안정수, 감염관리팀 직원 추가   
                strHTEL = "01028170176";

                if (strINFECT1.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFECT_법정전염병'";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT1.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "제1급법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE,";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT1ETC.Trim() != "")
                {
                    strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                    strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                    strMSG = strMSG + "제1급신종(" + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                    strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                    SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                    SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                    SQL = SQL + ComNum.VBLF + "         '22', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                    SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                if (strINFECT2.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFECT_법정전염병' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT2.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "제2급법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "','감염등록','" + strHTEL + "','22','','',SYSDATE,";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "'054-260-8019','','" + strMSG + "') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strINFECT3.Trim() != "")
                {
                    strMSG = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFECT_법정전염병'";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '" + strINFECT3.Trim() + "'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strMSG = "환자명:" + strName + "/" + strWard + "(" + strRoom + ")" + "/";
                        strMSG = strMSG + "의사명:" + strDrname + "/" + strJumin + "/" + strTel + "/직업:" + strJobName + "/" + strJuso + "/";
                        strMSG = strMSG + "제3급법정감염병(" + dt.Rows[0]["NAME"].ToString().Trim() + ")/발병일(" + strBDATE + ")/진단일(" + strJDate + ")/신고일(" + strSDate + ")/";
                        strMSG = strMSG + strExResult + "/" + strGbio + "/" + strPatBun + "/" + strSaMang;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                        SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '감염등록', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHTEL + "', ";
                        SQL = SQL + ComNum.VBLF + "         '22', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";           //구분22 감염등록
                        SQL = SQL + ComNum.VBLF + "         '054-260-8019', ";
                        SQL = SQL + ComNum.VBLF + "         '', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strMSG + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                clsDB.setCommitTran(pDbCon);
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_NST_MESSAGE
        /// </summary>
        /// <returns></returns>
        public static void READ_NST_MESSAGE(PsmhDb pDbCon, string strPtNo, string strIpdNo, string strSabun)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = "SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.DIET_NST_PROGRESS";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + strIpdNo;
                SQL = SQL + ComNum.VBLF + "   AND ORDERNO IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND OK_SABUN IS NULL";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (ComFunc.MsgBoxQ("★확인★ 영양집중지원팀(NST)에서 전달한 Recommendation이 있으니 확인하시기 바랍니다."
                                    + ComNum.VBLF + "해당 메세지를 다음번에도 띄우시겠습니까?",
                                    "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                    {
                        clsDB.setBeginTran(pDbCon);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            SQL = "";
                            SQL = "UPDATE KOSMOS_PMPA.DIET_NST_PROGRESS";
                            SQL = SQL + ComNum.VBLF + "     SET ";
                            SQL = SQL + ComNum.VBLF + "         OK_SABUN = " + strSabun;
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "'";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        clsDB.setCommitTran(pDbCon);
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_METFORMIN
        /// </summary>
        /// <returns></returns>
        public static bool READ_METFORMIN(PsmhDb pDbCon, string strPtNo, string strIO, string strBDate)
        {
            //2012-08-07 김현욱 작성
            //metformin 현재 metformin 사용 여부 확인
            //입원일 경우 argIO : "I",  argIPDNO : 값 넣어줘야 합니다.
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            bool rtnVal = false;
            int i = 0;

            string strOrderCode = "";
            string strMedFrDate = "";

            try
            {
                SQL = "";
                SQL = "SELECT JEPCODE FROM (";
                //약국관리에서 Metformin 제제 코드 등록되어 있는 약품
                SQL = SQL + ComNum.VBLF + "SELECT JEPCODE FROM KOSMOS_ADM.DRUG_SETCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '12'";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                //약품정보 내에 Metformin 제제 항목 체크 되어 있는 약품
                SQL = SQL + ComNum.VBLF + " SELECT SUNEXT JEPCODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_DRUGINFO_new";
                SQL = SQL + ComNum.VBLF + " WHERE METFORMIN = '1' ";
                SQL = SQL + ComNum.VBLF + " ) GROUP BY JEPCODE ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i < (dt.Rows.Count - 1))
                        {
                            strOrderCode = strOrderCode + "'" + dt.Rows[i]["JEPCODE"].ToString().Trim() + "', ";
                        }
                        else
                        {
                            strOrderCode = strOrderCode + "'" + dt.Rows[i]["JEPCODE"].ToString().Trim() + "'";
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                if (strOrderCode.Trim() == "")
                {
                    return rtnVal;
                }

                if (strIO == "I")
                {
                    SQL = "";
                    SQL = "SELECT TO_CHAR(INDATE,'YYYYMMDD') INDATE ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE GBSTS NOT IN ('7','9')";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strMedFrDate = dt.Rows[0]["INDATE"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    if (strMedFrDate.Trim() == "")
                    {
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = "SELECT PTNO ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_OORDER    ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE >= TO_DATE('" + Convert.ToDateTime(strBDate).AddMonths(-3).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE <= TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND SUCODE IN (" + strOrderCode + ")";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.Dispose();
                        dt = null;

                        rtnVal = true;
                        return rtnVal;
                    }

                    dt.Dispose();
                    dt = null;

                    SQL = "";

                    for (i = 1; i <= 7; i++)
                    {
                        SQL = SQL + ComNum.VBLF + " SELECT PTNO ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER    ";
                        SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(i * -1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "   AND SUCODE IN (" + strOrderCode + ")";

                        if (i < 7)
                        {
                            SQL = SQL + ComNum.VBLF + " UNION ALL";
                        }
                    }

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.Dispose();
                        dt = null;

                        rtnVal = true;
                        return rtnVal;
                    }

                    dt.Dispose();
                    dt = null;

                    //자가약 중 metformin 사용 여부
                    //회신서에서 Metformin 제제  체크 -> 자가약 등록 시 회신서 내용에서 Metformin 제제 일 경우 비고에 `Metformin 제제` 라고 저장이 됨.
                    SQL = "";
                    SQL = " SELECT A.PTNO ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMR_CADEX_SELFMED A, KOSMOS_EMR.EMR_CADEX_SELFMED_ACT B";
                    SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE >= '" + Convert.ToDateTime(strBDate).AddDays(-7).ToString("yyyyMMdd") + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE <= '" + strBDate.Replace("-", "") + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND A.BIGO = 'Metformin 제제'";
                    SQL = SQL + ComNum.VBLF + "  AND A.SEQNO = B.SEQNO ";
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE >= TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(-7).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND B.BDATE <= TO_DATE('" + strBDate + "','YYYY-MM-DD')";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.Dispose();
                        dt = null;

                        rtnVal = true;
                        return rtnVal;
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    SQL = "";
                    SQL = "SELECT PTNO ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_OORDER    ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE >= TO_DATE('" + Convert.ToDateTime(strBDate).AddMonths(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE <= TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND SUCODE IN (" + strOrderCode + ")";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.Dispose();
                        dt = null;

                        rtnVal = true;
                        return rtnVal;
                    }

                    dt.Dispose();
                    dt = null;

                    //자가약 외래
                    SQL = "";
                    SQL = "SELECT PANO FROM KOSMOS_ADM.DRUG_HOISLIP";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND BDATE >= TO_DATE('" + Convert.ToDateTime(strBDate).AddMonths(-1).ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "    AND BDATE <= TO_DATE('" + strBDate + " 23:59','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "    AND METFORMIN  = '1'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.Dispose();
                        dt = null;

                        rtnVal = true;
                        return rtnVal;
                    }

                    dt.Dispose();
                    dt = null;

                    //외래에서 입원퇴원약 30전조회
                    SQL = "";
                    SQL = "SELECT PTNO ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER    ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE >= TO_DATE('" + Convert.ToDateTime(strBDate).AddMonths(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE <= TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND SUCODE IN (" + strOrderCode + ")";
                    SQL = SQL + ComNum.VBLF + "   AND GbTFlag  ='T' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.Dispose();
                        dt = null;

                        rtnVal = true;
                        return rtnVal;
                    }

                    dt.Dispose();
                    dt = null;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수 : READ_METFORMIN " + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_XRAY_CONTRAST
        /// </summary>
        /// <returns></returns>
        public static string READ_XRAY_CONTRAST(PsmhDb pDbCon, string strPtNo)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            StringBuilder rtnVal = new StringBuilder();

            try
            {
                SQL = "";
                SQL = "SELECT REMARK ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.XRAY_CONTRAST ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + strPtNo + "' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "READ_XRAY_CONTRAST " + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal.ToString().Trim();
                }

                if (reader.HasRows)
                {
                    rtnVal.AppendLine("조영제 부작용 history 환자입니다..");
                    rtnVal.AppendLine( "등록내용 -->>");

                    while(reader.Read())
                    {
                        rtnVal.AppendLine(reader.GetValue(0).ToString().Trim());
                    }
               
                    Cursor.Current = Cursors.Default;
                }
               

                reader.Dispose();
                reader = null;

                return rtnVal.ToString().Trim();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("함수명 : " + "READ_XRAY_CONTRAST " + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "READ_XRAY_CONTRAST " + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal.ToString().Trim();
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_OCS_XRAY_USE_CONTRAST
        /// </summary>
        /// <returns></returns>
        public static string READ_OCS_XRAY_USE_CONTRAST(PsmhDb pDbCon, string strSuCode, int intGuBun)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null; 
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT Code ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='OCS_조영제수가' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(CODE) ='" + strSuCode.Trim() + "' ";

                if (intGuBun != 0)
                {
                    SQL = SQL + ComNum.VBLF + "   AND Sort =" + intGuBun;
                }

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수 : READ_OCS_XRAY_USE_CONTRAST " + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;

                return rtnVal;
            }
        }

        /// <summary>
        /// 처방코드 삭제 여부 체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strOrderCode"></param>
        /// <returns></returns>
        public static string Read_Chk_DelOrder(PsmhDb pDbCon, string strOrderCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dtOrdDel = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT TRIM(ORDERCODE) ORDERCODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_ORDERCODE ";
                SQL = SQL + ComNum.VBLF + "  WHERE ORDERCODE = '" + strOrderCode + "' ";
                SQL = SQL + ComNum.VBLF + "    AND TRIM(SENDDEPT) = 'N' ";
                SqlErr = clsDB.GetDataTableREx(ref dtOrdDel, SQL, pDbCon);

                if (SqlErr != "")
                {
                    //ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dtOrdDel.Rows.Count > 0)
                {
                //    MessageBox.Show("처방코드 : " + dtOrdDel.Rows[0]["ORDERCODE"].ToString().Trim() + " 는 삭제 된 코드 입니다. 제외 후 처방 전송 바랍니다.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rtnVal = "NO";
                }

                dtOrdDel.Dispose();
                dtOrdDel = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;

                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_OCS_ANTI_EXAM_CHK
        /// </summary>
        /// <returns></returns>
        public static string READ_OCS_ANTI_EXAM_CHK(PsmhDb pDbCon, string strPtNo, FarPoint.Win.Spread.FpSpread ssSpread, int intStart, int intRow, string strBDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "";
            int i = 0;

            string strChk = "";
            string strMaxDate = "";

            try
            {
                for (i = intStart; i < intRow; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text.Trim().Equals("True"))
                    {
                        SQL = "";
                        SQL = "SELECT Code ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                        SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='OCS_항생제배양코드' ";
                        SQL = SQL + ComNum.VBLF + "   AND TRIM(CODE) ='" + ssSpread.ActiveSheet.Cells[i, 14].Text + "' ";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            strChk = "OK";
                        }

                        dt.Dispose();
                        dt = null;
                    }
                }

                //오더발생체크
                if (strChk == "OK")
                {
                    SQL = "";
                    SQL = "SELECT * FROM " + ComNum.DB_PMPA + "BAS_PATIENT_ANTI_POP_MST ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO ='" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND BDate =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.Dispose();
                        dt = null;
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    dt.Dispose();
                    dt = null;

                    //7일째 다시 확인
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     Max(BDate) AS MaxDate";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT_ANTI_POP_MST ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO ='" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND BDate <TO_DATE('" + strBDate + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strMaxDate = dt.Rows[0]["MAXDATE"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    if (strMaxDate == "")
                    {
                        rtnVal = "OK";
                    }
                    else
                    {
                        if (VB.DateDiff("d", Convert.ToDateTime(strBDate), Convert.ToDateTime(strMaxDate)) == 7)
                        {
                            ComFunc.MsgBox("<<< 항생제 처방 관련>>" + ComNum.VBLF + ComNum.VBLF + "항생제 처방전에 혈액 배양 검사를 하셨나요?");
                        }
                    }
                }
                else
                {
                    //7일째 다시 확인
                    SQL = "";
                    SQL = "SELECT * FROM " + ComNum.DB_PMPA + "BAS_PATIENT_ANTI_POP_MST ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO ='" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND BDate >=TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(-7).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND BDate <=TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(-7).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ComFunc.MsgBox("<<< 항생제 처방 관련>>" + ComNum.VBLF + ComNum.VBLF + "항생제 처방전에 혈액 배양 검사를 하셨나요?");
                    }

                    dt.Dispose();
                    dt = null;
                }
                
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_CT_XRAY_CONTRAST_CHK
        /// 2014-10-02
        /// </summary>
        /// <returns></returns>
        public static void READ_CT_XRAY_CONTRAST_CHK(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread ssSpread, int intRow, int intCol, string strPtNo)
        {
            int i = 0;
            string strChk = "";
            
            for (i = intRow; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    if (READ_OCS_XRAY_USE_CONTRAST(pDbCon, ssSpread.ActiveSheet.Cells[i, intCol].Text.Trim(), 1) == "OK")
                    {
                        strChk = READ_XRAY_CONTRAST(pDbCon, strPtNo);

                        if (strChk != "")
                        {
                            ComFunc.MsgBox(strChk, "CT 조영제 부작용 확인");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_SUGA_MR_고가약
        /// 류마티스 고가약
        /// </summary>
        /// <returns></returns>
        public static string READ_SUGA_MR_EXPENSIVE_MEDICINE(PsmhDb pDbCon, string strSuCode, string strDept)
        {
            string rtnVal = string.Empty;

            //2020-05-11, 사용안하는것으로 확인되어 바로 빠져나가도록 수정 
            return rtnVal;

            #region 미사용 주석

            //string SQL = "";
            //string SqlErr = ""; //에러문 받는 변수
            //DataTable dt = null;

            //if (strDept != "MR")
            //{
            //    return rtnVal;
            //}

            //try
            //{
            //    SQL = "";
            //    SQL = "SELECT Code ";
            //    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
            //    SQL = SQL + ComNum.VBLF + "  WHERE GUBUN = 'ETC_고가약문자용'  ";
            //    SQL = SQL + ComNum.VBLF + "   AND TRIM(CODE) ='" + strSuCode.Trim() + "'  ";
            //    SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL or DelDate ='')    ";

            //    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //    if (SqlErr != "")
            //    {
            //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //        Cursor.Current = Cursors.Default;
            //        return rtnVal;
            //    }

            //    if (dt.Rows.Count > 0)
            //    {
            //        rtnVal = "OK";
            //    }

            //    dt.Dispose();
            //    dt = null;

            //    return rtnVal;
            //}
            //catch (Exception ex)
            //{
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            //    ComFunc.MsgBox(ex.Message);
            //    Cursor.Current = Cursors.Default;
            //    return rtnVal;
            //}
            #endregion
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_SUGA_부가가치세
        /// 부가가치세 수가체크 2014-02-25
        /// </summary>
        /// <returns></returns>
        public static string READ_SUGA_VALUE_ADDED_TAX(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수            
            string rtnVal = "";

            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL = "SELECT SuNext ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUN ";
                SQL = SQL + ComNum.VBLF + "  WHERE SuNext = '" + strSuCode + "'  ";
                SQL = SQL + ComNum.VBLF + "   AND GbTax ='Y' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;
                
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : SET_ORDER_부가가치세
        /// 부가세 수가발생시 오더전체 체크
        /// </summary>
        /// <returns></returns>
        public static void SET_ORDER_VALUE_ADDED_TAX(FarPoint.Win.Spread.FpSpread ssSpread, int intRow, int intCol)
        {
            int i = 0;

            for (i = intRow; i < ssSpread.ActiveSheet.RowCount; i++)
            {
                ssSpread.ActiveSheet.Cells[i, intCol].Text = "1";
            }
        }

        /// <summary>
        /// OrderETC    READ_SUGA_항혈전수가
        /// </summary>
        /// <returns></returns>
        public static string READ_SUGA_ANTIBLOOD(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT JEPCODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_SETCODE ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN = '13'  ";
                SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL or DelDate ='')    ";
                SQL = SQL + ComNum.VBLF + "   AND JepCode ='" + strSuCode.Trim() + "'  ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;
                
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_항혈전대상자체크
        /// </summary>
        /// <returns></returns>
        public static string READ_ANTIBLOOD_CHK(PsmhDb pDbCon, string strPtNo)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "";

            //string strOK = "";

            try
            {
                //항현전제 약제 조회 100 입원,외래   입원데이타는 2012-11-26일부터 생성됨
                SQL = "";
                SQL = "SELECT MAX(TO_CHAR(BDATE,'YYYY-MM-DD'))  BDATE FROM  KOSMOS_PMPA.BAS_PATIENT_ANTITHROMBOTIC ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND BDate >=TRUNC(SYSDATE -100) ";     // 100전자료까지
                SQL = SQL + ComNum.VBLF + "  AND Gubun ='01' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    //1하루전까지 데이타 생성된것 먼저 쿼리
                    if (dt.Rows[0]["BDATE"].ToString().Trim() != "")
                    {
                        //strOK = "OK";
                        rtnVal = "항혈전제:" + dt.Rows[0]["BDATE"].ToString().Trim();

                        dt.Dispose();
                        dt = null;

                        return rtnVal;
                    }
                }

                dt.Dispose();
                dt = null;

                //자가약 체크 오늘이전
                SQL = "";
                SQL = "SELECT MAX(TO_CHAR(BDATE,'YYYY-MM-DD'))  BDATE FROM  KOSMOS_ADM.DRUG_HOISLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND BDate >=TRUNC(SYSDATE -100) ";  //100전자료까지
                SQL = SQL + ComNum.VBLF + "  AND BLOOD ='1' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    //1하루전까지 데이타 생성된것 먼저 쿼리
                    if (dt.Rows[0]["BDATE"].ToString().Trim() != "")
                    {
                        //strOK = "OK";
                        rtnVal = "항혈전제:" + dt.Rows[0]["BDATE"].ToString().Trim();

                        dt.Dispose();
                        dt = null;

                        return rtnVal;
                    }
                }

                dt.Dispose();
                dt = null;

                //항혈전제 약제 1일전까지  - 마감생성이 하루전발생
                SQL = "";
                SQL = "SELECT MAX(TO_CHAR(BDATE,'YYYY-MM-DD')) BDATE FROM KOSMOS_PMPA.OPD_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE =TRUNC(SYSDATE) ";  //위의자료 형성 하루지난다음날 발생때문
                SQL = SQL + ComNum.VBLF + "   AND TRIM(SUNEXT) IN ( SELECT TRIM(JEPCODE)  FROM KOSMOS_ADM.DRUG_SETCODE  WHERE (DELDATE IS NULL or DelDate ='')   AND GUBUN = '13' )  ";
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT MAX(TO_CHAR(BDATE,'YYYY-MM-DD')) BDATE FROM KOSMOS_PMPA.IPD_NEW_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE =TRUNC(SYSDATE) ";  //위의자료 형성 하루지난다음날 발생때문
                SQL = SQL + ComNum.VBLF + "   AND TRIM(SUNEXT) IN ( SELECT TRIM(JEPCODE)  FROM KOSMOS_ADM.DRUG_SETCODE  WHERE (DELDATE IS NULL or DelDate ='')   AND GUBUN = '13' )  ";
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT MAX(TO_CHAR(BDATE,'YYYY-MM-DD')) BDATE FROM  KOSMOS_ADM.DRUG_HOISLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BLOOD ='1' ";
                SQL = SQL + ComNum.VBLF + "   AND TRUNC(BDATE) =TRUNC(SYSDATE) ";  //위의자료 형성 하루지난다음날 발생때문
                SQL = SQL + ComNum.VBLF + " ORDER BY 1 ASC ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    //1하루전까지 데이타 생성된것 먼저 쿼리
                    if (dt.Rows[0]["BDATE"].ToString().Trim() != "")
                    {
                        rtnVal = "항혈전제:" + dt.Rows[0]["BDATE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_면역억제대상자체크
        /// </summary>
        /// <returns></returns>
        public static string READ_IMMUNOSUPPRESSION_CHK(PsmhDb pDbCon, string strPtNo)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "";

            try
            {
                //약제 조회 100 입원,외래   입원데이타는 2012-11-26일부터 생성됨
                SQL = "";
                SQL = "SELECT MAX(BDATE)  BDATE FROM  KOSMOS_PMPA.BAS_PATIENT_ANTITHROMBOTIC ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND BDate >=TRUNC(SYSDATE -100) ";     // 100전자료까지
                SQL = SQL + ComNum.VBLF + "  AND Gubun ='02' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0 && dt.Rows[0]["BDATE"].ToString().Trim() != "")
                {
                    rtnVal = "면역억제제:" + dt.Rows[0]["BDATE"].ToString().Trim();

                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    //약제 1일전까지  - 마감생성이 하루전발생
                    SQL = "";
                    SQL = "SELECT MAX(BDATE) BDATE FROM KOSMOS_PMPA.OPD_SLIP ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE =TRUNC(SYSDATE) ";  //위의자료 형성 하루지난다음날 발생때문
                    SQL = SQL + ComNum.VBLF + "   AND TRIM(SUNEXT) IN ( SELECT TRIM(JEPCODE)  FROM KOSMOS_ADM.DRUG_SPECIAL_JEPCODE  WHERE SEQNO = 7  )  ";
                    SQL = SQL + ComNum.VBLF + " UNION ALL ";
                    SQL = SQL + ComNum.VBLF + " SELECT MAX(BDATE) BDATE FROM KOSMOS_PMPA.IPD_NEW_SLIP ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE =TRUNC(SYSDATE) ";  //위의자료 형성 하루지난다음날 발생때문
                    SQL = SQL + ComNum.VBLF + "   AND TRIM(SUNEXT) IN ( SELECT TRIM(JEPCODE)  FROM KOSMOS_ADM.DRUG_SPECIAL_JEPCODE  WHERE SEQNO = 7  )  ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0 && dt.Rows[0]["BDATE"].ToString().Trim() != "")
                    { 
                        rtnVal = "면역억제제:" + dt.Rows[0]["BDATE"].ToString().Trim();
                    }
                    else
                    {
                        rtnVal = "";
                    }

                    dt.Dispose();
                    dt = null;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_S_DRUG_MESSAGE
        /// 특수약 메시지 통합
        /// </summary>
        /// <returns></returns>
        public static string CHK_S_DRUG_MESSAGE(PsmhDb pDbCon, string strPtNo)
        {
            string strTemp1 = "";
            string strTemp2 = "";

            string rtnVal = "";

            strTemp1 = READ_ANTIBLOOD_CHK(pDbCon, strPtNo);
            strTemp2 = READ_IMMUNOSUPPRESSION_CHK(pDbCon, strPtNo);

            if (strTemp1 != "")
            {
                rtnVal = strTemp1;
            }

            if (strTemp2 != "")
            {
                rtnVal = strTemp2;
            }

            if (strTemp1 != "" && strTemp2 != "")
            {
                rtnVal = "항혈전+면역억제";
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_SUGA_COMPONENT
        /// 혼돈약
        /// </summary>
        /// <returns></returns>
        public static string READ_SUGA_COMPONENT(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT 1 AS CNT ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_DRUGINFO_COMPONENT ";
                SQL = SQL + ComNum.VBLF + "  WHERE TRIM(GRPNAME) in (SELECT TRIM(GRPNAME) FROM KOSMOS_OCS.OCS_DRUGINFO_COMPONENT WHERE TRIM(SUNEXT) ='" + strSuCode + "') ";
                SQL = SQL + ComNum.VBLF + " HAVING COUNT(GRPNAME) > 0";  //2건이상인것  - 일단풀어줌 2012-12-12

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_SUGA_COMPONENT_VIEW
        /// 혼돈약 보기
        /// </summary>
        /// <returns></returns>
        public static string READ_SUGA_COMPONENT_VIEW(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int i = 0;

            StringBuilder rtnVal = new StringBuilder();

            try
            {
                SQL = "";
                SQL = "SELECT a.SuNext,b.SuNameK ";
                SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_OCS.OCS_DRUGINFO_COMPONENT a, KOSMOS_PMPA.BAS_SUN b  ";
                SQL = SQL + ComNum.VBLF + "  WHERE  TRIM(a.SuNext) =TRIM(b.SuNext(+)) ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(a.GRPNAME) in (SELECT TRIM(GRPNAME )";
                SQL = SQL + ComNum.VBLF + "                             FROM KOSMOS_OCS.OCS_DRUGINFO_COMPONENT";
                SQL = SQL + ComNum.VBLF + "                                 WHERE  TRIM(SuNext) = '" + strSuCode + "'";
                SQL = SQL + ComNum.VBLF + "                             GROUP BY GRPNAME   ) ";
                SQL = SQL + ComNum.VBLF + "   GROUP BY a.SuNext,b.SuNameK ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal.ToString().Trim();
                }

                if (reader.HasRows)
                {
                    rtnVal.AppendLine("혼돈약 확인!!");

                    while (reader.Read())
                    {
                        rtnVal.AppendLine(reader.GetValue(0).To<string>().Trim() + "(" + reader.GetValue(1).To<string>().Trim() + ")");
                    }
                }

                reader.Dispose();
                reader = null;

                return rtnVal.ToString().Trim();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal.ToString().Trim();
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_SUGA_SAME
        /// 유사코드,유사약품
        /// </summary>
        /// <returns></returns>
        public static string READ_SUGA_SAME(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT COUNT(JEPCODE) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_SETCODE ";
                SQL = SQL + ComNum.VBLF + "  WHERE TRIM(COMMENTS) in (SELECT TRIM(COMMENTS)";
                SQL = SQL + ComNum.VBLF + "                             FROM KOSMOS_ADM.DRUG_SETCODE";
                SQL = SQL + ComNum.VBLF + "                                 WHERE GUBUN ='14'";
                SQL = SQL + ComNum.VBLF + "                                 AND TRIM(JEPCODE) ='" + strSuCode + "') ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN ='14' ";
                SQL = SQL + ComNum.VBLF + " HAVING COUNT(JEPCODE) > 1";     // 2건이상인것

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_SUGA_SAME_VIEW
        /// 유사코드,유사약품 보기
        /// </summary>
        /// <returns></returns>
        public static string READ_SUGA_SAME_VIEW(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int i = 0;

            StringBuilder rtnVal = new StringBuilder();

            try
            {
                SQL = "";
                SQL = "SELECT a.JEPCODE,b.SuNameK ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_SETCODE a, KOSMOS_PMPA.BAS_SUN b  ";
                SQL = SQL + ComNum.VBLF + "  WHERE TRIM(a.JepCode) =TRIM(b.SuNext(+)) ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(a.COMMENTS) in (SELECT TRIM(COMMENTS)";
                SQL = SQL + ComNum.VBLF + "                             FROM KOSMOS_ADM.DRUG_SETCODE";
                SQL = SQL + ComNum.VBLF + "                                 WHERE GUBUN ='14'";
                SQL = SQL + ComNum.VBLF + "                                 AND TRIM(JEPCODE) ='" + strSuCode + "'";
                SQL = SQL + ComNum.VBLF + "                             GROUP BY COMMENTS  ) ";
                SQL = SQL + ComNum.VBLF + "   AND a.GUBUN ='14' ";
                SQL = SQL + ComNum.VBLF + "   GROUP BY a.JEPCODE,b.SuNameK ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal.ToString().Trim();
                }

                if (reader.HasRows)
                {
                    rtnVal.AppendLine("유사코드,유사약품 확인!!");

                    while(reader.Read())
                    {
                        rtnVal.AppendLine(reader.GetValue(0).To<string>().Trim()  + "(" + reader.GetValue(1).To<string>().Trim() + ")");
                    }
                }

                reader.Dispose();
                reader = null;

                return rtnVal.ToString().Trim();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal.ToString().Trim();
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_SUGA_FM_VIEW
        /// 가정의학과 수가코드 제한
        /// </summary>
        /// <returns></returns>
        public static string READ_SUGA_FM_VIEW(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT a.JepCode ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_SPECIAL_JEPCODE a, KOSMOS_ADM.DRUG_SPECIAL_SABUN b ";
                SQL = SQL + ComNum.VBLF + "  WHERE a.SEQNO = b.SEQNO ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(a.JepCode) ='" + strSuCode + "'  ";
                SQL = SQL + ComNum.VBLF + "   AND b.SABUN  IN ('34625','32158','34902','36531')  ";
                SQL = SQL + ComNum.VBLF + "   AND a.Seqno =6         ";     // 2014-08-07

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_Infect_stats
        /// </summary>
        /// <returns></returns>
        public static string READ_Infect_stats(PsmhDb pDbCon, string strPtNo, string strBDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            int i = 0;

            string rtnVal = "";

            try
            {
                //감염관련 -------------------------------------------------------------------------------------- start
                SQL = "";
                SQL = "SELECT VDRL, HCV_IGG, HBS_AG, HIV";
                SQL = SQL + ComNum.VBLF + "     FROM KOSMOS_OCS.EXAM_INFECTMASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["VDRL"].ToString().Trim() != "" || dt.Rows[0]["HCV_IGG"].ToString().Trim() != ""
                        || dt.Rows[0]["HBS_AG"].ToString().Trim() != "" || dt.Rows[0]["HIV"].ToString().Trim() != "")
                    {
                        rtnVal = rtnVal + "A";
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT  GUBUN,TO_CHAR(RDATE,'YYYY-MM-DD') RDATE";
                SQL = SQL + ComNum.VBLF + "     FROM KOSMOS_OCS.EXAM_INFECT_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPtNo + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["GUBUN"].ToString().Trim() == "01")
                        {
                            rtnVal = rtnVal + "A";
                        }
                        else if (dt.Rows[i]["GUBUN"].ToString().Trim() == "02")
                        {
                            if (Convert.ToDateTime(dt.Rows[i]["RDATE"].ToString().Trim()) >= Convert.ToDateTime(strBDate).AddMonths(-1))
                            {
                                rtnVal = rtnVal + "B";
                            }
                        }
                        else if (dt.Rows[i]["GUBUN"].ToString().Trim() == "01")
                        {
                            if (Convert.ToDateTime(dt.Rows[i]["RDATE"].ToString().Trim()) >= Convert.ToDateTime(strBDate).AddMonths(-1))
                            {
                                rtnVal = rtnVal + "C";
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_ORDER_Result_AUTO
        /// 2013-04-17
        /// 오더체크 ( 골다공증, 치매약, 형간염약제 ) - 결과치 체크하여 급여비급여
        /// </summary>
        /// <returns></returns>
        public static string CHK_ORDER_Result_AUTO(PsmhDb pDbCon, string strGubun, string strPtNo, string strBDate, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt1 = null;
            string rtnVal = "";

            string strMsg = "";
            double dblValues = 0;

            try
            {
                //골다공증(골다공증 약사용을 위해서 골밀도 검사처방이 있어야 가능.검사가 있으면 처방기준 365일 안에 약 사용 가능. 1년 지나면 약 사용하기 위해서 골밀도 처방 있어야함)
                if (strGubun == "1")
                {
                    SQL = "";
                    SQL = "SELECT GBBONE FROM KOSMOS_PMPA.BAS_SUN ";
                    SQL = SQL + ComNum.VBLF + "     WHERE SUNEXT ='" + strSuCode + "'";
                    SQL = SQL + ComNum.VBLF + "     AND GBBONE ='Y'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        rtnVal = "OK";
                        return rtnVal;
                    }

                    dt.Dispose();
                    dt = null;

                    //2016-05-23 계장 김현욱
                    //골다공증 골절 진단일자가 들어가 있으면 보험 대상임
                    //(원래는 최대 3년까지 보험대상인데 심사과정희정쌤이 기간 제한은 두지 말자고 했음)
                    SQL = "";
                    SQL = "SELECT PANO";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.ETC_OCS_RESULT";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "'";
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN = '03'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.Dispose();
                        dt = null;

                        rtnVal = "OK";
                        return rtnVal;
                    }

                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "SELECT MAX(TO_CHAR(SeekDate,'YYYY-MM-DD')) ENTERDATE, EXINFO,'1' Gbn FROM KOSMOS_PMPA.XRAY_DETAIL ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND XCODE IN ('HC341','HC342','HC342A') ";
                    SQL = SQL + ComNum.VBLF + "   AND EXINFO NOT IN ( 0,1)  ";
                    SQL = SQL + ComNum.VBLF + "   AND SeekDate <=TO_DATE('" + strBDate + " 23:59','YYYY-MM-DD  HH24:MI') "; //2014-11-24 심사과장 통화후
                    SQL = SQL + ComNum.VBLF + " GROUP BY EXINFO ";
                    SQL = SQL + ComNum.VBLF + " UNION ALL ";
                    SQL = SQL + ComNum.VBLF + " SELECT MAX(TO_CHAR(BDATE,'YYYY-MM-DD')) ENTERDATE, EXINFO,'2' Gbn FROM KOSMOS_PMPA.ETC_OCS_RESULT ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND BDate <=TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN ='01' ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY EXINFO ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY 1 DESC ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        //2014-12-10 마지막 검사일자후 1년안 체크
                        //if (VB.DateDiff("d", Convert.ToDateTime(strBDate), DateTime.Parse(dt.Rows[0]["ENTERDATE"].ToString().Trim()).ToShortDateString()) <= 365)
                        if (fn_DATE_ILSU(clsDB.DbCon, strBDate, DateTime.Parse(dt.Rows[0]["ENTERDATE"].ToString().Trim()).ToShortDateString()) <= 365)
                        {
                            if (dt.Rows[0]["GBN"].ToString().Trim() == "1")
                            {
                                SQL = "";
                                SQL = "SELECT RESULT FROM KOSMOS_PMPA.XRAY_RESULTNEW ";
                                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + dt.Rows[0]["EXINFO"].ToString().Trim();
                            }
                            else if (dt.Rows[0]["GBN"].ToString().Trim() == "2")
                            {
                                SQL = "";
                                SQL = "SELECT RESULT FROM KOSMOS_PMPA.ETC_OCS_RESULT  ";
                                SQL = SQL + ComNum.VBLF + " WHERE BDATE >=TO_DATE('" + Convert.ToDateTime(strBDate).AddYears(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "  AND GUBUN ='01' ";
                                SQL = SQL + ComNum.VBLF + "  AND Pano ='" + strPtNo + "' ";
                                SQL = SQL + ComNum.VBLF + " ORDER BY BDATE DESC ";
                            }

                            SqlErr = "";
                            SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                dblValues = VB.Val(dt1.Rows[0]["RESULT"].ToString().Trim());

                                strMsg = "최종검사일:" + dt.Rows[0]["ENTERDATE"].ToString().Trim() + " 결과값:" + dt1.Rows[0]["RESULT"].ToString().Trim();

                                //nValues = -2.5
                                //검사수치 -2.5 보다 작아야 급여 ( 상태안좋은사람  -값은 클수록 작다 )
                                if (dblValues <= -2.5)
                                {
                                    rtnVal = "OK";
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                //치매
                else if (strGubun == "2")
                {
                    SQL = "";
                    SQL = "SELECT GBBONE FROM KOSMOS_PMPA.BAS_SUN";
                    SQL = SQL + ComNum.VBLF + "     WHERE SUNEXT ='" + strSuCode + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND gbdementia  ='Y'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        rtnVal = "OK";

                        dt.Dispose();
                        dt = null;

                        return rtnVal;
                    }

                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "SELECT TO_CHAR(BDate,'YYYY-MM-DD') ENTERDATE, '1' Gbn FROM KOSMOS_OCS.ETC_RESULT_DEMENTIA ";
                    SQL = SQL + ComNum.VBLF + " WHERE PtNO = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND Gubun = '1' ";  //접수된것
                    SQL = SQL + ComNum.VBLF + "   AND BDate >=TO_DATE('" + Convert.ToDateTime(strBDate).AddYears(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " UNION ALL ";
                    SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') ENTERDATE, '2' Gbn FROM KOSMOS_PMPA.ETC_OCS_RESULT ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND BDate >=TO_DATE('" + Convert.ToDateTime(strBDate).AddYears(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN ='02' ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY 1 DESC ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strMsg = "최종검사일:" + dt.Rows[0]["ENTERDATE"].ToString().Trim();

                        //치매는 1년이내 검사있으면 보험
                        rtnVal = "OK";
                    }

                    dt.Dispose();
                    dt = null;
                }
                //황간염약제
                else if (strGubun == "3")
                {
                    SQL = "";
                    SQL = "SELECT Code FROM KOSMOS_PMPA.BAS_BCode";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN ='ETC_B형간염약제체크'";
                    SQL = SQL + ComNum.VBLF + "     AND TRIM(CODE) ='" + strSuCode + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND (DELDATE IS NULL OR DELDATE ='')";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        rtnVal = "OK";

                        dt.Dispose();
                        dt = null;

                        return rtnVal;
                    }

                    dt.Dispose();
                    dt = null;

                    //4개월의 데이타 체크 - 당일제외
                    SQL = "";
                    SQL = "SELECT TO_CHAR(BDate,'YYYY-MM-DD') ENTERDATE FROM KOSMOS_PMPA.OPD_SLIP ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND BDate >=TO_DATE('" + Convert.ToDateTime(strBDate).AddMonths(-4).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND BDate <TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSuCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND GbSelf ='0' ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY 1 DESC ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strMsg = "최종검사일:" + dt.Rows[0]["ENTERDATE"].ToString().Trim();

                        rtnVal = "OK";
                    }

                    dt.Dispose();
                    dt = null;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "CHK_ORDER_Result_AUTO" + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_ORDER_SONO_BOHUM
        /// 오더체크 - 초음파 급여코드 점검 2013-09-27
        /// </summary>
        /// <returns></returns>
        public static void CHK_ORDER_SONO_BOHUM(PsmhDb pDbCon, string strGubun, string strPtNo, string strBDate, string strSuCode, string strBun)
        {
            if (strBun != "49")
            {
                return;
            }

            if (READ_JinDan_SONO_CHK2(pDbCon, strSuCode) == "OK")
            {
                return;
            }
            //2019-02-01 
            //ComFunc.MsgBox("4대 중증 초음파 보험 코드 [" + strSuCode + "]  발생..!!" + ComNum.VBLF + "대상이 맞는지 확인후 전송하십시오!!" + ComNum.VBLF + "대상자 : 중증암환자(V193),  희귀난치환*/자(@V001~@V245)" + ComNum.VBLF + "입원 뇌혈관(V191),  입원 심혈관 수술(V192)", "4대중증초음파확인");
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_ORDER_DOSCODE
        /// 특정코드 용법체크
        /// </summary>
        /// <returns></returns>
        public static string CHK_ORDER_DOSCODE(string strSuCode, string strBun, FarPoint.Win.Spread.FpSpread ssSpread, int intCol, int intSugaCol)
        {
            string rtnVal = "";
            int i = 0;

            rtnVal = "OK";

            for (i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
            {
                if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    if (ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim() == "C8050010")
                    {
                        if (ssSpread.ActiveSheet.Cells[i, intCol].Text.Trim() == "")
                        {
                            rtnVal = "NO";

                            ComFunc.MsgBox("수가명 : " + strSuCode + " " + ssSpread.ActiveSheet.Cells[i, intCol].Text.Trim()
                                            + ComNum.VBLF + "해당 Order에 대한 용법코드를 반드시 입력하셔야 합니다.",
                                            "용법코드 입력요망(처방 전송 취소)");
                        }
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_ORDER_SONO_BOHUM2
        /// 오더체크 - 초음파 급여코드 점검 2015-02-27
        /// </summary>
        /// <returns></returns>
        public static string CHK_ORDER_SONO_BOHUM2(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread ssSpread, string strPtNo, string strBDate, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "";

            string strDate = Convert.ToDateTime(strBDate).AddYears(-1).ToString("yyyy-MM-dd");

            rtnVal = "OK";

            try
            {
                if (strSuCode == "E9451" || strSuCode == "E9452" || strSuCode == "E9451U" || strSuCode == "E9451B" || strSuCode == "E9452C")
                {
                    SQL = "";
                    SQL = "SELECT BDATE ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_OORDER ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PTNO ='" + strPtNo + "'  ";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE >=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ORDERCODE IN ( 'E9451','E9452','E9451U','E9452U','E9451B','E9452C')";
                    SQL = SQL + ComNum.VBLF + "   AND GBSELF='0' ";
                    SQL = SQL + ComNum.VBLF + "   AND GBSUNAP ='1' ";
                    SQL = SQL + ComNum.VBLF + "  GROUP BY BDATE  ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("함수명 : " + "CHK_ORDER_SONO_BOHUM2" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog("함수명 : " + "CHK_ORDER_SONO_BOHUM2" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count >= 2)
                    {
                        rtnVal = "NO";

                        ComFunc.MsgBox("초음파 보험여부 참고하세요!!" + ComNum.VBLF + "E9451,E9452,E9451U,E9452U,E9451B,E9452C 1년안에 2번 처방가능합니다..", "확인");
                    }

                    dt.Dispose();
                    dt = null;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("함수명 : " + "CHK_ORDER_SONO_BOHUM2" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "CHK_ORDER_SONO_BOHUM2" + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_ORDER_PD_JUSA_CHK
        /// 오더체크 - 초음파 급여코드 점검 2015-02-27
        /// </summary>
        public static string CHK_ORDER_PD_JUSA_CHK(PsmhDb pDbCon, string strPtNo, string strBDate, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            rtnVal = "OK";

            try
            {
                if (strBDate == ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "D"), "D", "-"))
                {
                    return rtnVal;
                }

                if (strSuCode == "INFAN" || strSuCode == "MMR2")
                {
                    SQL = "";
                    SQL = "SELECT BDATE                                                                                                                         ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_OORDER                                                                                      ";
                    SQL = SQL + ComNum.VBLF + "WHERE PTNO ='" + strPtNo + "'                                                                                    ";
                    SQL = SQL + ComNum.VBLF + "  AND BDATE >=TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(-29).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')   ";
                    SQL = SQL + ComNum.VBLF + "  AND BDATE <TO_DATE('" + strBDate + "','YYYY-MM-DD')                                                            ";  //당일제외
                    SQL = SQL + ComNum.VBLF + "  AND SuCode  =' " + strSuCode +                                                                                 "' ";
                    SQL = SQL + ComNum.VBLF + "  AND GBSUNAP ='1'                                                                                               ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY BDATE                                                                                                   ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("함수명 : " + "CHK_ORDER_PD_JUSA_CHK" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog("함수명 : " + "CHK_ORDER_PD_JUSA_CHK" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (reader.HasRows)
                    {
                        rtnVal = "NO";

                        ComFunc.MsgBox(strSuCode + " 접종코드는 30일 이내에 처방불가합니다..!!", "확인");
                    }

                    reader.Dispose();
                    reader = null;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("함수명 : " + "CHK_ORDER_PD_JUSA_CHK" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "CHK_ORDER_PD_JUSA_CHK" + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// READ_보험하복부초음파_CHK
        /// 2019-02-01
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtNo"></param>
        /// <param name="strBDate"></param>
        /// <param name="strSuCode"></param>
        /// <returns></returns>
        public static string CHK_BOHUM_LOWDER_ADM_SONO_CHK(PsmhDb pDbCon, string ArgPano, string strBDate, string ArgSuCode, string ArgSelf)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "";

            string strCdate = "";

            rtnVal = "OK";

            try
            {
                if (string.Compare(strBDate,"2019-02-01") < 0)
                {
                    return rtnVal;
                }

                if (ComQuery.READ_BCODE_Name_ALL(pDbCon, "00", "OCS_급여_초음파관련", ArgSuCode, "", true) == "")
                {
                    return rtnVal;
                }

                if (ArgSuCode == "") return rtnVal;

                SQL = " SELECT MIN(BDATE) BDATE ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.OPD_SLIP";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "    AND SUNEXT = '" + ArgSuCode + "' ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE >= TO_DATE('2019-02-01','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND BDATE < TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  UNION ALL        ";
                SQL = SQL + ComNum.VBLF + " SELECT MIN(BDATE) BDATE ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.IPD_NEW_SLIP ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "    AND SUNEXT = '" + ArgSuCode + "' ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE >= TO_DATE('2019-02-01','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND BDATE < TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  ORDER BY BDATE ASC ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("함수명 : " + "CHK_BOHUM_LOWDER_ADM_SONO_CHK" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "CHK_BOHUM_LOWDER_ADM_SONO_CHK" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strCdate = dt.Rows[0]["BDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strCdate != "")
                {
                    if (ArgSelf != "2")
                    {
                        if (ComFunc.MsgBoxQ("★ 최초 처방일 : " + strCdate + ComNum.VBLF + " 2회 촬영부터는 선별 급여 본인부담 80% 입니다." + ComNum.VBLF + " s항에 2로 처방 해주세요" + ComNum.VBLF + " s항 변경없이 그대로 전송하시겠습니까??", "PSMH") == DialogResult.No)
                        {
                            rtnVal = "NO";
                        }
                    }
                }
                
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("함수명 : " + "CHK_BOHUM_LOWDER_ADM_SONO_CHK" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "CHK_BOHUM_LOWDER_ADM_SONO_CHK" + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// OrderEtc.bas : CHK_BAS_MSELF_ORDER
        /// ocs 제한사항 2013-04-22
        /// </summary>
        /// <returns></returns>
        public static string CHK_BAS_MSELF_ORDER(PsmhDb pDbCon, string strIO, string strGbn, string strPtNo, string strBDate, string strJumin, string strBirth, string strSuCode,
                                                    string strBun, string strDept, FarPoint.Win.Spread.FpSpread ssSpread, int intSelfCol, int intSugaCol, int intBunCol, string strVerBal)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt1 = null;

            OracleDataReader reader = null;
            int i = 0;
            int k = 0;
            int p = 0;

            double dblnn = 0;
            int intCnt = 0;
            int intNal = 0;

            string rtnVal = "";

            int intAge = 0;     //세
            int intAge2 = 0;    //주
            int intAge3 = 0;    //개월

            double dblValue1 = 0;

            string strValue1 = "";
            string strValue2 = "";
            string strValue3 = "";

            string strSDate = "";

            string strTSelf = "";
            string strTSuGa = "";
            string strTBun = "";

            string strSugaTT = "";

            string strGBIOE = "";

            string strJumin1 = VB.Left(strJumin, 6);
            string strJumin2 = VB.Right(strJumin, 7);

            //strBDate = strBDate.Replace("-", "");

            rtnVal = "OK";

            try
            {
                #region 연령금기 10
                //연령금기10, 연령금기08 - 표준코드X
                if (strGbn.Equals("10"))
                {
                    intAge = clsVbfunc.AGE_YEAR_GESAN2(strJumin, strBDate);     //년
                    intAge2 = CHK_WEEK_CNT_GESAN(pDbCon, strBDate, strBirth);   //주
                    intAge3 = CHK_AGE_MONTH_GESAN(pDbCon, strJumin, strBDate);  //개월

                    //약만
                    if (strBun.Equals("11") || strBun.Equals("12") || strBun.Equals("20"))
                    {
                        SQL = "";
                        SQL = "SELECT SUCODE,FIELDA,FIELDB ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF ";
                        SQL = SQL + ComNum.VBLF + "  WHERE ( (GUBUNA ='1' AND GUBUNB ='0') OR (GUBUNA ='0' AND GUBUNB ='8')   )   ";
                        SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSuCode + "' ";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                dblValue1 = VB.Val(dt.Rows[i]["FIELDA"].ToString().Trim());
                                strValue2 = dt.Rows[i]["FIELDB"].ToString().Trim();

                                switch (strValue2)
                                {
                                    case "세미만":
                                        if (intAge < dblValue1)
                                        {
                                            ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " " + dblValue1 + " 세미만 연령금기[전송불가]!!", "연령금기[약제팀]");
                                            rtnVal = dblValue1 + " 세미만 연령금기!!";
                                        }
                                        break;
                                    case "세이상":
                                        if (intAge >= dblValue1)
                                        {
                                            ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " " + dblValue1 + " 세이상 연령금기[전송불가]!!", "연령금기[약제팀]");
                                            rtnVal = dblValue1 + " 세이상 연령금기!!";
                                        }
                                        break;
                                    case "세이하":
                                        if (intAge <= dblValue1)
                                        {
                                            ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " " + dblValue1 + " 세이하 연령금기[전송불가]!!", "연령금기[약제팀]");
                                            rtnVal = dblValue1 + " 세이하 연령금기!!";
                                        }
                                        break;
                                    case "주미만":
                                        if (intAge < 1)
                                        {
                                            if (intAge2 < dblValue1)
                                            {
                                                ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " " + dblValue1 + " 주미만 연령금기[전송불가]!!", "연령금기[약제팀]");
                                                rtnVal = dblValue1 + " 주미만 연령금기!!";
                                            }
                                        }
                                        break;
                                    case "개월미만":
                                        if (intAge < 1)
                                        {
                                            if (intAge3 < dblValue1)
                                            {
                                                ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " " + dblValue1 + " 개월미만 연령금기[전송불가]!!", "연령금기[약제팀]");
                                                rtnVal = dblValue1 + " 개월미만 연령금기!!";
                                            }
                                        }
                                        break;
                                    case "개월이하":
                                        if (intAge < 1)
                                        {
                                            if (intAge3 <= dblValue1)
                                            {
                                                ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " " + dblValue1 + " 개월이하 연령금기[전송불가]!!", "연령금기[약제팀]");
                                                rtnVal = dblValue1 + " 개월이하 연령금기!!";
                                            }
                                        }
                                        break;
                                }
                            }
                        }

                        dt.Dispose();
                        dt = null;
                    }
                }
                #endregion

                #region 병용금기 09
                //병용금기
                else if (strGbn.Equals("09"))
                {
                    if (strBun.Equals("11") || strBun.Equals("12") || strBun.Equals("20"))
                    {
                        SQL = "";
                        SQL = "SELECT FIELDA                            ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF ";
                        SQL = SQL + ComNum.VBLF + "  WHERE GUBUNA ='0' ";
                        SQL = SQL + ComNum.VBLF + "   AND GUBUNB ='9'     ";
                        SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSuCode + "' ";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                strValue1 = reader.GetValue(0).ToString().Trim();

                                for (k = 0; k < ssSpread.ActiveSheet.NonEmptyRowCount; k++)
                                {
                                    if (ssSpread.ActiveSheet.Cells[k, 0].Text != "True")
                                    {
                                        strTSelf = ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim() == "" ? "0" : ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim();
                                        strTSuGa = ssSpread.ActiveSheet.Cells[k, intSugaCol].Text.Trim();
                                        strTBun = ssSpread.ActiveSheet.Cells[k, intBunCol].Text.Trim();

                                        //현재수가제외
                                        if (strSuCode != strValue1 && strTSelf == "0" && strValue1 == strTSuGa)
                                        {
                                            ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " + " + strValue1 + "  병용금기!!" + ComNum.VBLF + "처방수정후 작업하십시오[전송불가]!!", "병용금기[약제팀]");
                                            rtnVal = strSuCode + " + " + strValue1 + " 병용금기!!";
                                            reader.Dispose();
                                            reader = null;
                                            return rtnVal;
                                        }
                                    }
                                }
                            }
                        }

                        reader.Dispose();
                        reader = null;
                    }
                }
                #endregion

                #region 21(특정과만 급여)
                else if (strGbn.Equals("21"))
                {
                    SQL = "";
                    SQL = "SELECT FIELDA                                              ";
                    if (strIO.Equals("입원"))
                    {
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF_I      ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF        ";
                    }

                    SQL = SQL + ComNum.VBLF + "WHERE GUBUNA = '2'                     ";
                    SQL = SQL + ComNum.VBLF + "  AND GUBUNB = '1'                     ";
                    SQL = SQL + ComNum.VBLF + "  AND SUCODE = '" + strSuCode + "'     ";
                    SQL = SQL + ComNum.VBLF + "  AND FIELDA = '" + strDept + "'     ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (reader.HasRows)
                    {
                        for (k = 0; k < ssSpread.ActiveSheet.NonEmptyRowCount; k++)
                        {
                            strTSuGa = ssSpread.ActiveSheet.Cells[k, intSugaCol].Text.Trim();
                            strTSelf = ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim() == "" ? "0" : ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim();

                            if (ssSpread.ActiveSheet.Cells[k, 0].Text.Equals("True") == false && strTSuGa.Equals(strSuCode.Trim()) && strTSelf.Equals("0") == false)
                            {
                                ssSpread.ActiveSheet.Cells[k, intSelfCol].Text = "0";
                                reader.Dispose();
                                reader = null;
                                rtnVal = "OK";
                                return rtnVal;
                            }
                        }
                    }

                    reader.Dispose();
                    reader = null;
                }
                #endregion

                #region 22(특정과만 비급여)
                else if (strGbn.Equals("22"))
                {
                    SQL = "";
                    SQL = "SELECT FIELDA                                              ";
                    if (strIO.Equals("입원"))
                    {
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF_I      ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF        ";
                    }

                    SQL = SQL + ComNum.VBLF + "WHERE GUBUNA = '2'                     ";
                    SQL = SQL + ComNum.VBLF + "  AND GUBUNB = '2'                     ";
                    SQL = SQL + ComNum.VBLF + "  AND SUCODE = '" + strSuCode + "'     ";
                    SQL = SQL + ComNum.VBLF + "  AND FIELDA = '" + strDept + "'       ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (reader.HasRows)
                    {
                        for (k = 0; k < ssSpread.ActiveSheet.NonEmptyRowCount; k++)
                        {
                            strTSuGa = ssSpread.ActiveSheet.Cells[k, intSugaCol].Text.Trim();
                            strTSelf = ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim() == "" ? "0" : ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim();

                            if (ssSpread.ActiveSheet.Cells[k, 0].Text.Equals("True") == false && strTSuGa.Equals(strSuCode.Trim()) && strTSelf.Equals("0"))
                            {
                                //ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " + " + strValue1 + "  비급여만 가능!!" + ComNum.VBLF + "처방수정후 작업하십시오[전송불가]!!" + ComNum.VBLF  +
                                //    "문의 8037(심사팀)", "특정과는 비급여[심사팀] - 8037");
                                //rtnVal = strSuCode + " + " + "특정과는 비급여";
                                //reader.Dispose();
                                //reader = null;
                                //return rtnVal;

                                ssSpread.ActiveSheet.Cells[k, intSelfCol].Text = "2";//비급여;
                                reader.Dispose();
                                reader = null;
                                rtnVal = "OK";
                                return rtnVal;
                            }
                        }
                    }

                    reader.Dispose();
                    reader = null;
                }
                #endregion

                #region 71, 72 남, 여 특정과 비급여
                //71 남, 72 여 특정과 비급여
                else if (strGbn.Equals("71") || strGbn.Equals("72"))
                {
                    if (strIO.Equals("입원"))
                    {
                        SQL = "";
                        SQL = "SELECT SUCODE,FIELDA,FIELDB ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF_I ";
                        SQL = SQL + ComNum.VBLF + "  WHERE GUBUNA ='" + VB.Left(strGbn, 1) + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND GUBUNB ='" + VB.Right(strGbn, 1) + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND (FIELDA ='" + strDept + "' OR FIELDA ='**' )  ";  //2014-06-23
                        SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSuCode + "' ";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (reader.HasRows)
                        {
                            if (strGbn.Equals("71"))
                            {
                                ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + "수가는  남자 + " + strDept + "과 일경우 비급여처방으로 !!" + ComNum.VBLF + "처방수정[비급여 S항2 ]후 전송하십시오[전송불가]!!", "성별 특정과 비급여[심사과]");
                                rtnVal = strSuCode + " + " + strValue1 + " 성별 특정과는 비급여 처방!!";
                                reader.Dispose();
                                reader = null;
                                return rtnVal;
                            }
                            else if (strGbn.Equals("72"))
                            {
                                ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + "수가는  여자 + " + strDept + "과 일경우 비급여처방으로 !!" + ComNum.VBLF + "처방수정[비급여 S항2 ]후 전송하십시오[전송불가]!!", "성별 특정과 비급여[심사과]");
                                rtnVal = strSuCode + " + " + strValue1 + " 성별 특정과는 비급여 처방!!";
                                reader.Dispose();
                                reader = null;
                                return rtnVal;
                            }
                        }

                        reader.Dispose();
                        reader = null;
                    }

                    if (strIO.Equals("외래") || strIO.Equals("응급"))
                    {
                        SQL = "";
                        SQL = "SELECT SUCODE,FIELDA,FIELDB ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF ";
                        SQL = SQL + ComNum.VBLF + "  WHERE GUBUNA ='" + VB.Left(strGbn, 1) + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND GUBUNB ='" + VB.Right(strGbn, 1) + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND (FIELDA ='" + strDept + "' OR FIELDA ='**' )  ";  //2014-06-23
                        SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSuCode + "' ";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        if (reader.HasRows)
                        {
                            if (strGbn.Equals("71"))
                            {
                                ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + "수가는  남자 일경우 비급여처방으로 !!" + ComNum.VBLF + "처방수정[비급여 S항2 ]후 전송하십시오[전송불가]!!", "성별 특정과 비급여[심사과]");
                                rtnVal = strSuCode + " + " + strValue1 + " 성별 특정과는 비급여 처방!!";
                                reader.Dispose();
                                reader = null;
                                return rtnVal;
                            }
                            else if (strGbn.Equals("72"))
                            {
                                ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + "수가는  여자 일경우 비급여처방으로 !!" + ComNum.VBLF + "처방수정[비급여 S항2 ]후 전송하십시오[전송불가]!!", "성별 특정과 비급여[심사과]");
                                rtnVal = strSuCode + " + " + strValue1 + " 성별 특정과는 비급여 처방!!";
                                reader.Dispose();
                                reader = null;
                                return rtnVal;
                            }
                        }

                        reader.Dispose();
                        reader = null;
                    }

                    //버발시 체크
                    if (strVerBal.Equals("간호사") && strIO.Equals("입원"))
                    {
                        SQL = "";
                        SQL = "SELECT SUCODE ";

                        #region ㅅ버ㅡ쿼리
                        SQL = SQL + ComNum.VBLF + ", CASE WHEN EXISTS (                                     ";
                        SQL = SQL + ComNum.VBLF + "SELECT SUCODE,FIELDA,FIELDB                              ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF_I                            ";
                        SQL = SQL + ComNum.VBLF + "  WHERE GUBUNA ='" + VB.Left(strGbn, 1) + "'             ";
                        SQL = SQL + ComNum.VBLF + "   AND GUBUNB ='" + VB.Right(strGbn, 1) + "'             ";
                        SQL = SQL + ComNum.VBLF + "   AND (FIELDA ='" + strDept + "' OR FIELDA ='**' )      ";  //2014-06-23
                        SQL = SQL + ComNum.VBLF + "   AND SUCODE = A.SUCODE                                 ";
                        SQL = SQL + ComNum.VBLF + ") THEN '1' END SU_CHK                                    ";
                        #endregion

                        SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_IORDER A";
                        SQL = SQL + ComNum.VBLF + "    WHERE Ptno     = '" + strPtNo + "' ";
                        SQL = SQL + ComNum.VBLF + "     AND GbStatus  IN  (' ','D+','D')";
                        SQL = SQL + ComNum.VBLF + "     AND BDate      = TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     AND ( ( GBIOE IN ( 'EI' ,'E') AND GBACT ='*') OR   (GBACT <> '*' OR GBACT IS NULL) )";
                        SQL = SQL + ComNum.VBLF + "     AND (NurseID   IS NULL OR NurseId = ' '    OR OrderSite IN ('','TEL','OPD','DRUG','IPD','ER','" + strDept + "')) ";
                        SQL = SQL + ComNum.VBLF + "     AND Bun IN ('11','12','20') ";
                        SQL = SQL + ComNum.VBLF + "     AND GbSelf IN ('','0',' ') ";
                        SQL = SQL + ComNum.VBLF + "     AND Nal >0 ";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                #region 이전 로직 주석(2021-05-18)
                                //strSugaTT = reader.GetValue(0).ToString().Trim(); //dt.Rows[0]["SUCODE"].ToString().Trim();

                                //SQL = "";
                                //SQL = "SELECT SUCODE,FIELDA,FIELDB ";
                                //SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF_I ";
                                //SQL = SQL + ComNum.VBLF + "  WHERE GUBUNA ='" + VB.Left(strGbn, 1) + "' ";
                                //SQL = SQL + ComNum.VBLF + "   AND GUBUNB ='" + VB.Right(strGbn, 1) + "' ";
                                //SQL = SQL + ComNum.VBLF + "   AND (FIELDA ='" + strDept + "' OR FIELDA ='**' )  ";  //2014-06-23
                                //SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSugaTT + "' ";

                                //SqlErr = "";
                                //SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                                //if (SqlErr != "")
                                //{
                                //    ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                                //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                //    Cursor.Current = Cursors.Default;
                                //    return rtnVal;
                                //}

                                //if (dt1.Rows.Count > 0)
                                //{
                                //    if (strGbn == "71")
                                //    {
                                //        ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + "수가는  남자 + " + strDept + "과 일경우 비급여처방으로 !!" + ComNum.VBLF + "처방수정[비급여 S항2 ]후 전송하십시오[전송불가]!!", "성별 특정과 비급여[심사과]");
                                //        rtnVal = strSuCode + " + " + strValue1 + " 성별 특정과는 비급여 처방!!";
                                //        return rtnVal;
                                //    }
                                //    else if (strGbn == "72")
                                //    {
                                //        ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + "수가는  여자 + " + strDept + "과 일경우 비급여처방으로 !!" + ComNum.VBLF + "처방수정[비급여 S항2 ]후 전송하십시오[전송불가]!!", "성별 특정과 비급여[심사과]");
                                //        rtnVal = strSuCode + " + " + strValue1 + " 성별 특정과는 비급여 처방!!";
                                //        return rtnVal;
                                //    }
                                //}

                                //dt1.Dispose();
                                //dt1 = null;
                                #endregion

                                strSugaTT = reader.GetValue(0).ToString().Trim();

                                if (reader.GetValue(1).ToString().Trim().Equals("1"))
                                {
                                    if (strGbn.Equals("71"))
                                    {
                                        ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSugaTT, "1") + "수가는  남자 + " + strDept + "과 일경우 비급여처방으로 !!" + ComNum.VBLF + "처방수정[비급여 S항2 ]후 전송하십시오[전송불가]!!", "성별 특정과 비급여[심사과]");
                                        rtnVal = strSuCode + " + " + strValue1 + " 성별 특정과는 비급여 처방!!";
                                        reader.Dispose();
                                        reader = null;
                                        return rtnVal;
                                    }
                                    else if (strGbn.Equals("72"))
                                    {
                                        ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSugaTT, "1") + "수가는  여자 + " + strDept + "과 일경우 비급여처방으로 !!" + ComNum.VBLF + "처방수정[비급여 S항2 ]후 전송하십시오[전송불가]!!", "성별 특정과 비급여[심사과]");
                                        rtnVal = strSuCode + " + " + strValue1 + " 성별 특정과는 비급여 처방!!";
                                        reader.Dispose();
                                        reader = null;
                                        return rtnVal;
                                    }
                                }
                            }
                        }

                        reader.Dispose();
                        reader = null;
                    }
                }
                #endregion

                #region 80 동시처방불가(검사)
                //81 동시처방불가
                else if (strGbn.Equals("80") && strIO.Equals("외래"))
                {
                    SQL = "";
                    SQL = "SELECT FIELDA                                            ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF ";
                    SQL = SQL + ComNum.VBLF + "  WHERE GUBUNA = '8' ";
                    SQL = SQL + ComNum.VBLF + "    AND GUBUNB = '0'     ";
                    SQL = SQL + ComNum.VBLF + "    AND SUCODE = '" + strSuCode + "' ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //strValue1 = dt.Rows[i]["FIELDA"].ToString().Trim();     //수가
                            strValue1 = reader.GetValue(0).ToString().Trim();     //수가

                            for (k = 0; k < ssSpread.ActiveSheet.NonEmptyRowCount; k++)
                            {
                                if (ssSpread.ActiveSheet.Cells[k, 0].Text != "True")
                                {
                                    strTSelf = ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim() == "" ? "0" : ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim();
                                    strTSuGa = ssSpread.ActiveSheet.Cells[k, intSugaCol].Text.Trim();
                                    strTBun = ssSpread.ActiveSheet.Cells[k, intBunCol].Text.Trim();

                                    //현재수가제외
                                    if (strSuCode != strValue1 && strTSelf == "0" && strValue1 == strTSuGa)
                                    {
                                        ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " + " + strValue1 + "  동시처방불가!!" + ComNum.VBLF + "처방수정후 작업하십시오[전송불가]!!", "동시처방[심사과]");
                                        rtnVal = strSuCode + " + " + strValue1 + " 동시처방불가(검사)!!";
                                        reader.Dispose();
                                        reader = null;
                                        return rtnVal;
                                    }
                                }
                            }
                        }
                    }

                    reader.Dispose();
                    reader = null;
                }
                #endregion

                #region 81 동시처방불가(약제)
                //81 동시처방불가
                else if (strGbn.Equals("81"))
                {
                    SQL = "";
                    SQL = "SELECT FIELDA                                            ";
                    if (strIO.Equals("입원"))
                    {
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF_I ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF ";
                    }
                    SQL = SQL + ComNum.VBLF + "  WHERE GUBUNA = '8' ";
                    SQL = SQL + ComNum.VBLF + "    AND GUBUNB = '1'     ";
                    SQL = SQL + ComNum.VBLF + "    AND SUCODE = '" + strSuCode + "' ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //strValue1 = dt.Rows[i]["FIELDA"].ToString().Trim();     //수가
                            strValue1 = reader.GetValue(0).ToString().Trim();     //수가

                            for (k = 0; k < ssSpread.ActiveSheet.NonEmptyRowCount; k++)
                            {
                                if (ssSpread.ActiveSheet.Cells[k, 0].Text != "True")
                                {
                                    strTSelf = ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim() == "" ? "0" : ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim();
                                    strTSuGa = ssSpread.ActiveSheet.Cells[k, intSugaCol].Text.Trim();
                                    strTBun = ssSpread.ActiveSheet.Cells[k, intBunCol].Text.Trim();

                                    if (strIO.Equals("입원"))
                                    {
                                        //2021-01-12 변경
                                        //strGBIOE = ssSpread.ActiveSheet.Cells[k, 56].Text.Trim();
                                        strGBIOE = ssSpread.ActiveSheet.Cells[k, 59].Text.Trim();
                                    }

                                    //현재수가제외
                                    if (strSuCode != strValue1 && strTSelf == "0" && strValue1 == strTSuGa && strGBIOE == "")
                                    {
                                        ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " + " + strValue1 + "  동시처방불가!!" + ComNum.VBLF + "처방수정후 작업하십시오[전송불가]!!", "동시처방[심사과]");
                                        rtnVal = strSuCode + " + " + strValue1 + " 병용금기!!";
                                        reader.Dispose();
                                        reader = null;
                                        return rtnVal;
                                    }
                                }
                            }
                        }
                    }

                    reader.Dispose();
                    reader = null;

                    //버발시 체크
                    if (strVerBal.Equals("간호사") && strIO.Equals("입원"))
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT FIELDA                                           ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF_I A                          ";
                        SQL = SQL + ComNum.VBLF + "  WHERE GUBUNA = '8'                                     ";
                        SQL = SQL + ComNum.VBLF + "   AND GUBUNB  = '1'                                     ";
                        SQL = SQL + ComNum.VBLF + "   AND SUCODE  = A.SUCODE                                ";
                        SQL = SQL + ComNum.VBLF + "   AND EXISTS                                            ";
                        SQL = SQL + ComNum.VBLF + "   (                                                     ";
                        SQL = SQL + ComNum.VBLF + "SELECT SUCODE ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER A";
                        SQL = SQL + ComNum.VBLF + "WHERE Ptno     = '" + strPtNo + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND GbStatus  IN  (' ','D+','D')";
                        SQL = SQL + ComNum.VBLF + "  AND BDate      = TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "  AND ( ( GBIOE IN ( 'EI' ,'E') AND GBACT ='*') OR   (GBACT <> '*' OR GBACT IS NULL) )";
                        SQL = SQL + ComNum.VBLF + "  AND (NurseID   IS NULL OR NurseId = ' '    OR OrderSite IN ('','TEL','OPD','DRUG','IPD','ER','" + strDept.Trim() + "')) ";
                        SQL = SQL + ComNum.VBLF + "  AND Bun IN ('11','12','20') ";
                        SQL = SQL + ComNum.VBLF + "  AND (GbSelf IN ('','0',' ') OR GbSelf IS NULL)  ";
                        SQL = SQL + ComNum.VBLF + "  AND Nal >0 ";
                        SQL = SQL + ComNum.VBLF + "  AND SUCODE = A.SUCODE";
                        SQL = SQL + ComNum.VBLF + "   )                                                     ";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                #region 이전 로직 주석(2021-05-18)

                                //    SQL = "";
                                //    SQL = "SELECT SUCODE,FIELDA,FIELDB ";
                                //    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF_I ";
                                //    SQL = SQL + ComNum.VBLF + "  WHERE GUBUNA ='8' ";
                                //    SQL = SQL + ComNum.VBLF + "   AND GUBUNB ='1'     ";
                                //    SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSugaTT + "' ";

                                //    SqlErr = "";
                                //    SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                                //    if (SqlErr != "")
                                //    {
                                //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                //        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                //        Cursor.Current = Cursors.Default;
                                //        return rtnVal;
                                //    }

                                //    if (dt1.Rows.Count > 0)
                                //    {
                                //        for (k = 0; k < dt1.Rows.Count; k++)
                                //        {
                                //            strValue1 = dt1.Rows[k]["FIELDA"].ToString().Trim();    //수가

                                //            for (p = 0; p < ssSpread.ActiveSheet.RowCount; p++)
                                //            {
                                //                if (ssSpread.ActiveSheet.Cells[p, 0].Text != "True")
                                //                {
                                //                    strTSelf = VB.IIf(ssSpread.ActiveSheet.Cells[p, intSelfCol].Text.Trim() == "", "0", ssSpread.ActiveSheet.Cells[p, intSelfCol].Text.Trim()).ToString();
                                //                    strTSuGa = ssSpread.ActiveSheet.Cells[p, intSugaCol].Text.Trim();
                                //                    strTBun = ssSpread.ActiveSheet.Cells[p, intBunCol].Text.Trim();

                                //                    //현재수가제외
                                //                    if (strSuCode != strValue1 && strTSelf == "0" && strValue1 == strTSuGa)
                                //                    {
                                //                        ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " + " + strValue1 + "  동시처방불가!!" + ComNum.VBLF + "처방수정후 작업하십시오[전송불가]!!", "동시처방[심사과]");
                                //                        rtnVal = strSuCode + " + " + strValue1 + " 병용금기!!";
                                //                        return rtnVal;
                                //                    }
                                //                }
                                //            }
                                //        }
                                //    }

                                //    dt1.Dispose();
                                //    dt1 = null;
                                //}
                                #endregion

                                #region 신규 로직

                                strValue1 = reader.GetValue(0).ToString().Trim();   //수가
                                for (p = 0; p < ssSpread.ActiveSheet.RowCount; p++)
                                {
                                    if (ssSpread.ActiveSheet.Cells[p, 0].Text != "True")
                                    {
                                        strTSelf = ssSpread.ActiveSheet.Cells[p, intSelfCol].Text.Trim() == "" ? "0" : ssSpread.ActiveSheet.Cells[p, intSelfCol].Text.Trim();
                                        strTSuGa = ssSpread.ActiveSheet.Cells[p, intSugaCol].Text.Trim();
                                        strTBun = ssSpread.ActiveSheet.Cells[p, intBunCol].Text.Trim();

                                        //현재수가제외
                                        if (strSuCode != strValue1 && strTSelf == "0" && strValue1 == strTSuGa)
                                        {
                                            ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " + " + strValue1 + "  동시처방불가!!" + ComNum.VBLF + "처방수정후 작업하십시오[전송불가]!!", "동시처방[심사과]");
                                            rtnVal = strSuCode + " + " + strValue1 + " 병용금기!!";
                                            reader.Dispose();
                                            reader = null;
                                            return rtnVal;
                                        }
                                    }
                                }

                            }

                            #endregion

                        }

                        reader.Dispose();
                        reader = null;
                    }
                }
                #endregion

                #region 87 기간별 갯수 제한
                //87 기간별 갯수 제한--------------------------------------------------------------------
                else if (strGbn == "87")
                {
                    SQL = "";
                    SQL = "SELECT SUCODE,FIELDA,FIELDB,FIELDC                   ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF      ";
                    SQL = SQL + ComNum.VBLF + "WHERE GUBUNA = '8'               ";
                    SQL = SQL + ComNum.VBLF + "  AND GUBUNB = '7'               ";
                    SQL = SQL + ComNum.VBLF + "  AND SUCODE = '" + strSuCode + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strValue1 = dt.Rows[i]["FIELDA"].ToString().Trim();     //기간
                            strValue2 = dt.Rows[i]["FIELDB"].ToString().Trim();     //갯수
                            strValue3 = dt.Rows[i]["FIELDC"].ToString().Trim();     //2015-02-13

                            dblnn = VB.Val(strValue1) * -1;

                            //현재오더 체크
                            for (k = 0; k < ssSpread.ActiveSheet.NonEmptyRowCount; k++)
                            {
                                if (ssSpread.ActiveSheet.Cells[k, 0].Text != "True")
                                {
                                    strTSelf = ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim() == "" ? "0" : ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim();
                                    strTSuGa = ssSpread.ActiveSheet.Cells[k, intSugaCol].Text.Trim();
                                    strTBun = ssSpread.ActiveSheet.Cells[k, intBunCol].Text.Trim();

                                    //현재수가제외
                                    if (strTSelf == "0" && strSuCode == strTSuGa)
                                    {
                                        //2015-02-13
                                        if (strValue3 == "**")
                                        {
                                            intCnt = intCnt + 1;
                                        }
                                        else
                                        {
                                            if (strDept == strValue3)
                                            {
                                                intCnt = intCnt + 1;
                                            }
                                        }
                                    }
                                }
                            }

                            //하루전오더까지 체크
                            if (strValue3 == "**")
                            {
                                //strBDate = VB.Left(strBDate, 4) + "-" + VB.Mid(strBDate, 5, 2) + "-" + VB.Right(strBDate, 2);

                                if (strIO == "응급")
                                {
                                    SQL = "";
                                    SQL = "SELECT  TO_CHAR(BDate,'YYYY-MM-DD') BDATE ,SUCODE,SUM(QTY*NAL) NCNT,SUM(qty) Cnt  ";
                                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_iORDER ";
                                    SQL = SQL + ComNum.VBLF + "  WHERE Ptno ='" + strPtNo + "'  ";
                                    SQL = SQL + ComNum.VBLF + "   AND BDate >=TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(dblnn).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "   AND BDate <TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "   AND SuCode ='" + strSuCode + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND GbSend = ' ' ";
                                    SQL = SQL + ComNum.VBLF + "   AND GBIOE IN ('E') ";
                                    SQL = SQL + ComNum.VBLF + "   AND GbAct =' ' ";
                                    SQL = SQL + ComNum.VBLF + "   AND GbSelf ='0' ";
                                    SQL = SQL + ComNum.VBLF + "  GROUP BY TO_CHAR(BDate,'YYYY-MM-DD') ,SUCODE ";
                                    SQL = SQL + ComNum.VBLF + "   HAVING SUM(QTY*NAL) <> 0 ";
                                    SQL = SQL + ComNum.VBLF + "  ORDER BY BDATE ASC ";
                                }
                                else
                                {
                                    SQL = "";
                                    SQL = "SELECT  TO_CHAR(BDate,'YYYY-MM-DD') BDATE ,SUCODE,SUM(QTY*NAL) NCNT,SUM(qty) Cnt  ";
                                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_OORDER ";
                                    SQL = SQL + ComNum.VBLF + "  WHERE Ptno ='" + strPtNo + "'  ";
                                    SQL = SQL + ComNum.VBLF + "   AND BDate >=TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(dblnn).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "   AND BDate <TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "   AND SuCode ='" + strSuCode + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND GbSunap ='1'";
                                    SQL = SQL + ComNum.VBLF + "   AND GbSelf ='0' ";
                                    SQL = SQL + ComNum.VBLF + "  GROUP BY TO_CHAR(BDate,'YYYY-MM-DD') ,SUCODE ";
                                    SQL = SQL + ComNum.VBLF + "   HAVING SUM(QTY*NAL) <> 0 ";
                                    SQL = SQL + ComNum.VBLF + "  ORDER BY BDATE ASC ";
                                }

                                SqlErr = "";
                                SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }

                                if (dt1.Rows.Count > 0)
                                {
                                    for (k = 0; k < dt1.Rows.Count; k++)
                                    {
                                        intCnt = intCnt + Convert.ToInt32(VB.Val(dt1.Rows[k]["CNT"].ToString().Trim()));
                                    }
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }
                            else
                            {
                                if (strDept == strValue3)
                                {
                                    SQL = "";
                                    SQL = "SELECT  TO_CHAR(BDate,'YYYY-MM-DD') BDATE ,SUCODE,SUM(QTY*NAL) NCNT,SUM(qty) Cnt  ";
                                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_OORDER ";
                                    SQL = SQL + ComNum.VBLF + "  WHERE Ptno ='" + strPtNo + "'  ";
                                    SQL = SQL + ComNum.VBLF + "   AND BDate >=TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(dblnn).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "   AND BDate <TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "   AND SuCode ='" + strSuCode + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND GbSunap ='1'";
                                    SQL = SQL + ComNum.VBLF + "   AND GbSelf ='0' ";
                                    SQL = SQL + ComNum.VBLF + "   AND DeptCode ='" + strValue3 + "' ";
                                    SQL = SQL + ComNum.VBLF + "  GROUP BY TO_CHAR(BDate,'YYYY-MM-DD') ,SUCODE ";
                                    SQL = SQL + ComNum.VBLF + "   HAVING SUM(QTY*NAL) <> 0 ";
                                    SQL = SQL + ComNum.VBLF + "  ORDER BY BDATE ASC ";

                                    SqlErr = "";
                                    SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }

                                    if (dt1.Rows.Count > 0)
                                    {
                                        for (k = 0; k < dt1.Rows.Count; k++)
                                        {
                                            intCnt = intCnt + Convert.ToInt32(VB.Val(dt1.Rows[k]["CNT"].ToString().Trim()));
                                        }
                                    }

                                    dt1.Dispose();
                                    dt1 = null;
                                }
                            }

                            if (intCnt > VB.Val(strValue2))
                            {
                                ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " + " + strValue1 + "일동안 : " + strValue2 + " 수량제한 !!!" + ComNum.VBLF + "처방수정후 작업하십시오[전송불가]!!", "동시처방[심사과]");
                                rtnVal = strSuCode + " + " + strValue1 + "기간별 수량제한!!";
                                dt.Dispose();
                                dt = null;
                                return rtnVal;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                #endregion

                #region OCS_처방제한_기간별수가제한
                else if (strGbn == "OCS_처방제한_기간별수가제한")
                {
                    SQL = "";
                    SQL = "SELECT Code AS SUCODE,Sort AS FIELDA,Cnt AS FIELDB,Gubun2 AS FIELDC ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                    SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='" + strGbn + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND CODE = '" + strSuCode + "'   ";
                    SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE ='') ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strValue1 = dt.Rows[i]["FIELDA"].ToString().Trim();     //기간
                            strValue2 = dt.Rows[i]["FIELDB"].ToString().Trim();     //갯수
                            strValue3 = dt.Rows[i]["FIELDC"].ToString().Trim();     //2015-02-13

                            dblnn = VB.Val(strValue1) * -1;

                            //현재오더 체크
                            for (k = 0; k < ssSpread.ActiveSheet.NonEmptyRowCount; k++)
                            {
                                if (ssSpread.ActiveSheet.Cells[k, 0].Text != "True")
                                {
                                    strTSelf = ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim() == "" ? "0" : ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim();
                                    strTSuGa = ssSpread.ActiveSheet.Cells[k, intSugaCol].Text.Trim();
                                    strTBun = ssSpread.ActiveSheet.Cells[k, intBunCol].Text.Trim();

                                    //현재수가제외
                                    if (strTSelf == "0" && strSuCode == strTSuGa)
                                    {
                                        //2015-02-13
                                        if (strValue3 == "**")
                                        {
                                            intCnt = intCnt + 1;
                                        }
                                        else
                                        {
                                            if (strDept == strValue3)
                                            {
                                                intCnt = intCnt + 1;
                                            }
                                        }
                                    }
                                }
                            }

                            if (strIO == "응급" || strIO == "입원")
                            {
                                SQL = "";
                                SQL += " SELECT  TO_CHAR(BDate,'YYYY-MM-DD') BDATE ,SUCODE,SUM(QTY*NAL) NCNT,SUM(qty) Cnt   \r";
                                SQL += "   FROM KOSMOS_OCS.OCS_iORDER                                                       \r";
                                SQL += "  WHERE Ptno = '" + strPtNo + "'                                                    \r";
                                SQL += "    AND BDate >= TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(dblnn).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')   \r";
                                SQL += "    AND BDate <  TO_DATE('" + strBDate + "','YYYY-MM-DD')                           \r";
                                SQL += "    AND SuCode ='" + strSuCode + "'                                                 \r";
                                SQL += "    AND GbSend = ' '                                                                \r";
                                if (strIO == "응급")
                                {
                                    SQL += "    AND GBIOE IN ('E')                                                          \r";
                                }
                                else
                                {
                                    SQL += "    AND (GBIOE IS NULL OR GBIOE NOT IN ('E'))                                   \r";
                                }
                                SQL += "    AND GbAct = ' '                                                                 \r";
                                SQL += "    AND GbSelf = '0'                                                                \r";
                                if (strValue3 != "**")
                                {
                                    SQL += "    AND DeptCode = '" + strValue3 + "'                                          \r";
                                }
                                SQL += "  GROUP BY TO_CHAR(BDate,'YYYY-MM-DD') ,SUCODE                                      \r";
                                SQL += " HAVING SUM(QTY*NAL) <> 0                                                           \r";
                                SQL += "  ORDER BY BDATE ASC                                                                \r";
                            }
                            else
                            {
                                SQL = "";
                                SQL += " SELECT TO_CHAR(BDate,'YYYY-MM-DD') BDATE ,SUCODE,SUM(QTY*NAL) NCNT,SUM(qty) Cnt    \r";
                                SQL += "   FROM KOSMOS_OCS.OCS_OORDER                                                       \r";
                                SQL += "   WHERE Ptno = '" + strPtNo + "'                                                   \r";
                                SQL += "    AND BDate >= TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(dblnn).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')   \r";
                                SQL += "    AND BDate <  TO_DATE('" + strBDate + "','YYYY-MM-DD')                           \r";
                                SQL += "   AND SuCode = '" + strSuCode + "'                                                 \r";
                                SQL += "   AND GbSunap = '1'                                                                \r";
                                SQL += "   AND GbSelf = '0'                                                                 \r";
                                if (strValue3 != "**")
                                {
                                    SQL += "    AND DeptCode = '" + strValue3 + "'                                          \r";
                                }
                                SQL += "  GROUP BY TO_CHAR(BDate,'YYYY-MM-DD') ,SUCODE                                      \r";
                                SQL += "   HAVING SUM(QTY*NAL) <> 0                                                         \r";
                                SQL += "  ORDER BY BDATE ASC                                                                \r";
                            }

                            SqlErr = "";
                            SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                for (k = 0; k < dt1.Rows.Count; k++)
                                {
                                    intCnt = intCnt + Convert.ToInt32(VB.Val(dt1.Rows[k]["CNT"].ToString().Trim()));
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;


                            if (intCnt > VB.Val(strValue2))
                            {
                                if (MessageBox.Show(READ_Suga_NameK(pDbCon, strSuCode, "1") + " + " + strValue1 + "일동안 : " + strValue2 + " 수량제한 !!!" + ComNum.VBLF + "이대로 전송하시겠습니까?", "기간별 수량제한!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                {
                                    rtnVal = strSuCode + " + " + strValue1 + "기간별 수량제한!!";
                                }

                                dt.Dispose();
                                dt = null;
                                return rtnVal;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                #endregion

                #region 88기간제한 입원?(테이블은 외래인데..?))
                //88 기간 제한 - 입원
                else if (strGbn == "880")
                {
                    intNal = 0;
                    intCnt = 0;

                    SQL = "";
                    SQL = "SELECT SUCODE,FIELDA,FIELDB,FIELDC ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF ";
                    SQL = SQL + ComNum.VBLF + "WHERE GUBUNA = '8' ";
                    SQL = SQL + ComNum.VBLF + "  AND GUBUNB  = '8'     ";
                    SQL = SQL + ComNum.VBLF + "  AND SUCODE  = '" + strSuCode + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strValue1 = dt.Rows[i]["FIELDA"].ToString().Trim();     //일수
                            strValue3 = dt.Rows[i]["FIELDC"].ToString().Trim();     //2015-02-13

                            if (strValue3 == "")
                            {
                                strValue3 = "**";       //전체
                            }

                            //현재오더 체크
                            for (k = 0; k < ssSpread.ActiveSheet.NonEmptyRowCount; k++)
                            {
                                if (ssSpread.ActiveSheet.Cells[k, 0].Text != "True")
                                {
                                    strTSelf = ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim() == "" ? "0" : ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim();
                                    intNal = Convert.ToInt32(VB.Val(ssSpread.ActiveSheet.Cells[k, intSelfCol - 4].Text.Trim()));
                                    strTSuGa = ssSpread.ActiveSheet.Cells[k, intSugaCol].Text.Trim();
                                    strTBun = ssSpread.ActiveSheet.Cells[k, intBunCol].Text.Trim();

                                    //현재수가제외
                                    if (strTSelf == "0" && strSuCode == strTSuGa)
                                    {
                                        //2015-02-13
                                        if (strValue3 == "**")
                                        {
                                            intCnt = intCnt + intNal;
                                        }
                                        else
                                        {
                                            if (strDept == strValue3)
                                            {
                                                intCnt = intCnt + intNal;
                                            }
                                        }
                                    }
                                }
                            }

                            //하루전오더까지 체크
                            if (strValue3 == "**")
                            {
                                SQL = "";
                                SQL = "SELECT  TO_CHAR(BDate,'YYYY-MM-DD') BDATE ,SUCODE,SUM(NAL) NCNT,SUM(qty) Cnt  ";
                                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_iORDER ";
                                SQL = SQL + ComNum.VBLF + "  WHERE Ptno ='" + strPtNo + "'  ";
                                SQL = SQL + ComNum.VBLF + "   AND BDate >=TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(dblnn).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "   AND BDate <TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "   AND SuCode ='" + strSuCode + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND GbSend =' ' ";
                                SQL = SQL + ComNum.VBLF + "   AND GbSelf ='0' ";
                                SQL = SQL + ComNum.VBLF + "  GROUP BY TO_CHAR(BDate,'YYYY-MM-DD') ,SUCODE ";
                                SQL = SQL + ComNum.VBLF + "  ORDER BY BDATE ASC ";

                                SqlErr = "";
                                SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    for (k = 0; k < dt1.Rows.Count; k++)
                                    {
                                        intCnt = intCnt + Convert.ToInt32(VB.Val(dt1.Rows[k]["NCNT"].ToString().Trim()));
                                    }
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }
                            else
                            {
                                if (strDept == strValue3)
                                {
                                    SQL = "";
                                    SQL = "SELECT  TO_CHAR(BDate,'YYYY-MM-DD') BDATE ,SUCODE,SUM(NAL) NCNT,SUM(qty) Cnt  ";
                                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_iORDER ";
                                    SQL = SQL + ComNum.VBLF + "  WHERE Ptno ='" + strPtNo + "'  ";
                                    SQL = SQL + ComNum.VBLF + "   AND BDate >=TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(dblnn).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "   AND BDate <TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "   AND SuCode ='" + strSuCode + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND GbSend =' ' ";
                                    SQL = SQL + ComNum.VBLF + "   AND GbSelf ='0' ";
                                    SQL = SQL + ComNum.VBLF + "   AND DeptCode ='" + strValue3 + "' ";
                                    SQL = SQL + ComNum.VBLF + "  GROUP BY TO_CHAR(BDate,'YYYY-MM-DD') ,SUCODE ";
                                    SQL = SQL + ComNum.VBLF + "  ORDER BY BDATE ASC ";

                                    SqlErr = "";
                                    SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }

                                    if (dt1.Rows.Count > 0)
                                    {
                                        for (k = 0; k < dt1.Rows.Count; k++)
                                        {
                                            intCnt = intCnt + Convert.ToInt32(VB.Val(dt1.Rows[k]["NCNT"].ToString().Trim()));
                                        }
                                    }

                                    dt1.Dispose();
                                    dt1 = null;
                                }
                            }

                            if (intCnt > VB.Val(strValue1))
                            {
                                ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " + " + strValue1 + "일동안 : " + strValue2 + " 수량제한 !!!" + ComNum.VBLF + "처방수정후 작업하십시오[전송불가]!! 문의: 심사과", "동시처방[심사과]");
                                rtnVal = strSuCode + " + " + strValue1 + "기간별 수량제한!!";
                                dt.Dispose();
                                dt = null;
                                return rtnVal;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                #endregion

                #region 88 외래 기간제한
                //88 기간 제한
                else if (strGbn == "88")
                {
                    intNal = 0;
                    intCnt = 0;

                    SQL = "";
                    SQL = "SELECT SUCODE,FIELDA,FIELDB,FIELDC ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF ";
                    SQL = SQL + ComNum.VBLF + "  WHERE GUBUNA ='8' ";
                    SQL = SQL + ComNum.VBLF + "   AND GUBUNB ='8'     ";
                    SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSuCode + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strValue1 = dt.Rows[i]["FIELDA"].ToString().Trim();     //일수
                        strValue3 = dt.Rows[i]["FIELDC"].ToString().Trim();     //전체

                        dblnn = VB.Val(strValue1) * -1;

                        #region // 2019-10-18 심사팀에서 내용착오로 의뢰서 잘못올려서 다시 살림
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strValue1 = dt.Rows[i]["FIELDA"].ToString().Trim();     //일수
                            strValue3 = dt.Rows[i]["FIELDC"].ToString().Trim();     //전체

                            if (strValue3 == "")
                            {
                                strValue3 = "**";       //전체
                            }

                            dblnn = -730;       //2년자료 체크

                            //현재오더 체크
                            for (k = 0; k < ssSpread.ActiveSheet.RowCount; k++)
                            {
                                if (ssSpread.ActiveSheet.Cells[k, 0].Text != "True")
                                {
                                    strTSelf = ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim() == "" ? "0" : ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim();
                                    intNal = Convert.ToInt32(VB.Val(ssSpread.ActiveSheet.Cells[k, intSelfCol - 4].Text.Trim()));
                                    strTSuGa = ssSpread.ActiveSheet.Cells[k, intSugaCol].Text.Trim();
                                    strTBun = ssSpread.ActiveSheet.Cells[k, intBunCol].Text.Trim();

                                    //현재수가 제외
                                    if (strTSelf == "0" && strSuCode == strTSuGa)
                                    {
                                        //2015-02-13
                                        if (strValue3 == "**")
                                        {
                                            intCnt = intCnt + intNal;
                                        }
                                        else
                                        {
                                            if (strDept == strValue3)
                                            {
                                                intCnt = intCnt + intNal;
                                            }
                                        }
                                    }
                                }
                            }

                            //하루전오더까지 체크
                            if (strValue3 == "**")
                            {
                                SQL = "";
                                SQL = "SELECT  TO_CHAR(BDate,'YYYY-MM-DD') BDATE ,SUCODE,SUM(NAL) NCNT,SUM(qty) Cnt  ";
                                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_OORDER ";
                                SQL = SQL + ComNum.VBLF + "  WHERE Ptno ='" + strPtNo + "'  ";
                                SQL = SQL + ComNum.VBLF + "   AND BDate >=TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(dblnn).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "   AND BDate <TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "   AND SuCode ='" + strSuCode + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND GbSunap ='1' ";
                                SQL = SQL + ComNum.VBLF + "   AND GbSelf ='0' ";
                                SQL = SQL + ComNum.VBLF + "  GROUP BY TO_CHAR(BDate,'YYYY-MM-DD') ,SUCODE ";
                                SQL = SQL + ComNum.VBLF + "  ORDER BY BDATE ASC ";

                                SqlErr = "";
                                SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }

                                if (dt1.Rows.Count > 0)
                                {
                                    for (k = 0; k < dt1.Rows.Count; k++)
                                    {
                                        intCnt = intCnt + Convert.ToInt32(VB.Val(dt1.Rows[k]["NCNT"].ToString().Trim()));
                                    }
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }
                            else
                            {
                                if (strDept == strValue3)
                                {
                                    SQL = "";
                                    SQL = "SELECT  TO_CHAR(BDate,'YYYY-MM-DD') BDATE ,SUCODE,SUM(NAL) NCNT,SUM(qty) Cnt  ";
                                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_OORDER ";
                                    SQL = SQL + ComNum.VBLF + "  WHERE Ptno ='" + strPtNo + "'  ";
                                    SQL = SQL + ComNum.VBLF + "   AND BDate >=TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(dblnn).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "   AND BDate <TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "   AND SuCode ='" + strSuCode + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND GbSunap ='1' ";
                                    SQL = SQL + ComNum.VBLF + "   AND GbSelf ='0' ";
                                    SQL = SQL + ComNum.VBLF + "   AND DeptCode ='" + strValue3 + "' ";
                                    SQL = SQL + ComNum.VBLF + "  GROUP BY TO_CHAR(BDate,'YYYY-MM-DD') ,SUCODE ";
                                    SQL = SQL + ComNum.VBLF + "  ORDER BY BDATE ASC ";

                                    SqlErr = "";
                                    SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }

                                    if (dt1.Rows.Count > 0)
                                    {
                                        for (k = 0; k < dt1.Rows.Count; k++)
                                        {
                                            intCnt = intCnt + Convert.ToInt32(VB.Val(dt1.Rows[k]["NCNT"].ToString().Trim()));
                                        }
                                    }

                                    dt1.Dispose();
                                    dt1 = null;
                                }
                            }

                            if (intCnt > VB.Val(strValue1))
                            {
                                ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " + " + strValue1 + "일동안 : " + strValue2 + " 수량제한 !!!" + ComNum.VBLF + "처방수정후 작업하십시오[전송불가]!! 문의: 심사과", "동시처방[심사과]");
                                rtnVal = strSuCode + " + " + strValue1 + "기간별 수량제한!!";
                                dt.Dispose();
                                dt = null;
                                return rtnVal;
                            }
                        }
                        #endregion


                        #region //2019-09-06 전산업무의뢰서 2019-1063  // 2019-10-18 심사팀에서 내용착오로 의뢰서 잘못올려서 주석처리함
                        //SQL = "";
                        //SQL = "SELECT  TO_CHAR(BDate,'YYYY-MM-DD') BDATE ";
                        //SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_OORDER ";
                        //SQL = SQL + ComNum.VBLF + "  WHERE Ptno ='" + strPtNo + "'  ";
                        //SQL = SQL + ComNum.VBLF + "   AND BDate >=TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(dblnn).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        //SQL = SQL + ComNum.VBLF + "   AND BDate <TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        //SQL = SQL + ComNum.VBLF + "   AND SuCode ='" + strSuCode + "' ";
                        //SQL = SQL + ComNum.VBLF + "   AND GbSunap ='1' ";
                        //SQL = SQL + ComNum.VBLF + "   AND GbSelf ='0' ";
                        //if (strValue3 != "**")
                        //{
                        //    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strValue3 + "' ";
                        //}
                        ////SQL = SQL + ComNum.VBLF + "  GROUP BY TO_CHAR(BDate,'YYYY-MM-DD')";
                        //SQL = SQL + ComNum.VBLF + "  ORDER BY BDATE ASC ";

                        //SqlErr = "";
                        //SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                        //if (SqlErr != "")
                        //{
                        //    ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                        //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        //    Cursor.Current = Cursors.Default;
                        //    return rtnVal;
                        //}

                        //if (dt1.Rows.Count > 0)
                        //{
                        //    ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " + " + strValue1 + "일동안 : " + strValue2 + " 수량제한 !!!" + ComNum.VBLF + "처방수정후 작업하십시오[전송불가]!! 문의: 심사과", "동시처방[심사과]");
                        //    rtnVal = strSuCode + " + " + strValue1 + "기간별 수량제한!!";
                        //    return rtnVal;
                        //}
                        #endregion
                    }

                    dt.Dispose();
                    dt = null;
                }
                #endregion

                #region 89 입원 기간제한
                //89 기간 제한
                else if (strGbn == "89")
                {
                    intNal = 0;
                    intCnt = 0;

                    SQL = "";
                    SQL = "SELECT SUCODE,FIELDA,FIELDB ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF_i ";
                    SQL = SQL + ComNum.VBLF + "  WHERE GUBUNA ='8' ";
                    SQL = SQL + ComNum.VBLF + "   AND GUBUNB ='9'     ";
                    SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSuCode + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strValue1 = dt.Rows[i]["FIELDA"].ToString().Trim();     //일수

                            dblnn = -30;

                            strSDate = "";

                            //하루전오더까지 체크
                            SQL = "";
                            SQL = "SELECT  TO_CHAR(BDate,'YYYY-MM-DD') BDATE ,SUCODE,SUM(NAL) NCNT,SUM(qty) Cnt  ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_iORDER ";
                            SQL = SQL + ComNum.VBLF + "  WHERE Ptno ='" + strPtNo + "'  ";
                            SQL = SQL + ComNum.VBLF + "   AND BDate >=TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(dblnn).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND BDate <TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND SuCode ='" + strSuCode + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND (GbSelf ='0' OR GbSelf IS NULL)  ";
                            SQL = SQL + ComNum.VBLF + "  GROUP BY TO_CHAR(BDate,'YYYY-MM-DD') ,SUCODE ";
                            SQL = SQL + ComNum.VBLF + "  ORDER BY BDATE ASC ";

                            SqlErr = "";
                            SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                strSDate = dt1.Rows[0]["BDATE"].ToString().Trim();
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        if (strSDate != "" && VB.Val(strValue1) != 0)
                        {
                            if (Convert.ToDateTime(strSDate).AddDays(VB.Val(strValue1)) > Convert.ToDateTime(strBDate))
                            {
                                ComFunc.MsgBox("마지막처방 : " + strSDate + ComNum.VBLF + READ_Suga_NameK(pDbCon, strSuCode, "1") + " >> " + strValue1 + "일 간격 : " + strValue2 + " 기간제한 !!!" + ComNum.VBLF + "처방가능일자 : " + Convert.ToDateTime(strSDate).AddDays(VB.Val(strValue1)) + ComNum.VBLF + "처방수정후 작업하십시오[전송불가]!! 문의: 심사과", "처방간격제한[심사과]");
                                rtnVal = strSuCode + " + " + strValue1 + "기간별 수량제한!!";
                                dt.Dispose();
                                dt = null;
                                return rtnVal;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                #endregion

                #region 91입원 성별제한
                //91 입원 성별 제한 2014-11-07
                else if (strGbn == "91")
                {
                    SQL = "";
                    SQL = "SELECT SUCODE,FIELDA,FIELDB ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF_I ";
                    SQL = SQL + ComNum.VBLF + "  WHERE GUBUNA ='9' ";
                    SQL = SQL + ComNum.VBLF + "   AND GUBUNB ='1'     ";
                    SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSuCode + "' ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (reader.HasRows)
                    {
                        ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + "남자에게 사용할수 없는 수가입니다..!!!" + ComNum.VBLF + "처방수정후 작업하십시오[전송불가]!!", "성별체크[심사과]");
                        rtnVal = strSuCode + "성별제한!!";

                        reader.Dispose();
                        reader = null;

                        return rtnVal;
                    }

                    reader.Dispose();
                    reader = null;
                }
                #endregion

                #region 외래 특정과 제한
                //23 특정과 제한 2015-05-28
                else if (strGbn == "23")
                {
                    SQL = "";
                    SQL = "SELECT SUCODE                                                ";
                    SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_MSELF            ";
                    SQL = SQL + ComNum.VBLF + "  WHERE GUBUNA  = '2'                    ";
                    SQL = SQL + ComNum.VBLF + "    AND GUBUNB  = '3'                    ";
                    SQL = SQL + ComNum.VBLF + "    AND SUCODE  = '" + strSuCode + "'    ";
                    SQL = SQL + ComNum.VBLF + "    AND FIELDA  = '" + strDept + "'      ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (reader.HasRows)
                    {
                        ComFunc.MsgBox(READ_Suga_NameK(pDbCon, strSuCode, "1") + " " + strDept + " 과에서 사용할수 없는 수가입니다..!!!" + ComNum.VBLF + "처방수정후 작업하십시오[전송불가]!!", "성별체크[심사과]");
                        rtnVal = strSuCode + "특정과제한!!";

                        reader.Dispose();
                        reader = null;

                        return rtnVal;
                    }

                    reader.Dispose();
                    reader = null;
                }
                #endregion

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("CHK_BAS_MSELF_ORDER : " + strBDate + strSDate + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("CHK_BAS_MSELF_ORDER : " + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_BAS_MSELF_ORDER_ETC
        /// ocs 제한사항 2015-01-07
        /// </summary>
        /// <returns></returns>
        public static string CHK_BAS_MSELF_ORDER_ETC(PsmhDb pDbCon, string strGbn, string strPtNo, string strBDate, string strJumin, string strBirth, string strSuCode,
                                                        string strBun, string strDept, FarPoint.Win.Spread.FpSpread ssSpread, int intSelfCol, int intSugaCol,
                                                        int intBunCol, string strVerBal, string strInDate, int intGbloeCol)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            OracleDataReader reader = null;
            string rtnVal = "";
            int i = 0;
            int k = 0;

            string strSugaTT = "";
            string strChk = "";
            string strTSelf = "";
            string strRemark = "";
            string strTSuga = "";
            string strTBun = "";
            string strGBIOE = "";

            double dblValue1 = 0;

            int intNal = 0;

            string strJumin1 = VB.Left(strJumin, 6);
            string strJumin2 = VB.Right(strJumin, 7);

            strBDate = strBDate.Replace("-", "");

            rtnVal = "OK";

            try
            {
                if (strGbn == "86")
                {
                    //현재까지 사용수가 체크
                    SQL = "";
                    SQL = "SELECT SUCODE,FIELDA,FIELDB ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF_I ";
                    SQL = SQL + ComNum.VBLF + "  WHERE GUBUNA ='8' ";
                    SQL = SQL + ComNum.VBLF + "   AND GUBUNB ='6' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strSugaTT = dt.Rows[i]["SUCODE"].ToString().Trim();
                            dblValue1 = VB.Val(dt.Rows[i]["FIELDA"].ToString().Trim());
                            strChk = dt.Rows[i]["FIELDB"].ToString().Trim();

                            intNal = 0;

                            for (k = 0; k < ssSpread.ActiveSheet.NonEmptyRowCount; k++)
                            {
                                if (ssSpread.ActiveSheet.Cells[k, 0].Text.Trim().Equals("True"))
                                    continue;

                                strTSelf = ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim().IsNullOrEmpty() ? "0" : ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim();
                                strRemark = ssSpread.ActiveSheet.Cells[k, intSelfCol + 1].Text.Trim();
                                strTSuga = ssSpread.ActiveSheet.Cells[k, intSugaCol].Text.Trim();
                                strTBun = ssSpread.ActiveSheet.Cells[k, intBunCol].Text.Trim();

                                strGBIOE = ssSpread.ActiveSheet.Cells[k, intGbloeCol].Text.Trim();

                                if (strGBIOE == "")
                                {
                                    //시트수가체크
                                    if (strChk == "**")
                                    {
                                        //급여, 비급여 포함
                                        if (strSugaTT == strTSuga && strRemark != "P")
                                        {
                                            //intNal = intNal + Convert.ToInt32(VB.Val(ssSpread.ActiveSheet.Cells[k, 7].Text));
                                            //2020-08-10, dc된 처방은 포함안되도록 조건추가
                                            if (String.Compare(ssSpread.ActiveSheet.Cells[k, 7].Text, "0") > 0)
                                            {
                                                intNal = intNal + Convert.ToInt32(VB.Val(ssSpread.ActiveSheet.Cells[k, 7].Text));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (strTSelf == "0" && strSugaTT == strTSuga && strRemark != "P") 
                                        {
                                            //intNal = intNal + Convert.ToInt32(VB.Val(ssSpread.ActiveSheet.Cells[k, 7].Text));
                                            //intNal = Convert.ToInt32(VB.Val(ssSpread.ActiveSheet.Cells[k, 7].Text));
                                            //2020-08-10, dc된 처방은 포함안되도록 조건추가
                                            if (String.Compare(ssSpread.ActiveSheet.Cells[k, 7].Text, "0") > 0)
                                            {
                                                intNal = Convert.ToInt32(VB.Val(ssSpread.ActiveSheet.Cells[k, 7].Text));
                                            }
                                        }
                                    }

                                    if (intNal > 0 && strSugaTT == strTSuga && strRemark != "P")
                                    {
                                        //if (intNal > dblValue1)
                                        //{
                                        //    ComFunc.MsgBox(strSugaTT + "수가는 재원기간중 " + dblValue1 + "일만 사용가능합니다..", "재원기간 날수제한");
                                        //    rtnVal = "NO";
                                        //    return rtnVal;
                                        //}

                                        //BDATE 전까지 재원수가 체크
                                        SQL = "";
                                        SQL = "SELECT PTNO,BDATE,SUCODE,SUM(QTY*NAL) CNT ";
                                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER ";
                                        SQL = SQL + ComNum.VBLF + "WHERE BDate >= TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                                        SQL = SQL + ComNum.VBLF + "  AND BDate < TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                                        SQL = SQL + ComNum.VBLF + "  AND SUCODE ='" + strSugaTT + " '";
                                        SQL = SQL + ComNum.VBLF + "  AND PTNO ='" + strPtNo + "' ";
                                        SQL = SQL + ComNum.VBLF + "  AND GbPrn =' ' ";
                                        SQL = SQL + ComNum.VBLF + "  AND GbStatus  IN  (' ','D+','D','D-')";
                                        SQL = SQL + ComNum.VBLF + "  AND (GbSelf IS NULL OR GbSelf ='0')";
                                        SQL = SQL + ComNum.VBLF + "  AND (GbIOE IS NULL OR GbIOE='') ";
                                        SQL = SQL + ComNum.VBLF + "  AND ( ORDERSITE NOT IN ('CAN','NDC') OR ORDERSITE IS NULL)";
                                        SQL = SQL + ComNum.VBLF + "GROUP BY PTNO,BDATE,SUCODE";
                                        SQL = SQL + ComNum.VBLF + "HAVING SUM(QTY * NAL) > 0";

                                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                            Cursor.Current = Cursors.Default;
                                            return rtnVal;
                                        }

                                        if (reader.HasRows == false)
                                        {
                                            reader.Dispose();
                                            reader = null;
                                            Cursor.Current = Cursors.Default;
                                        }
                                        else
                                        {
                                            while(reader.Read())
                                            {
                                                intNal += 1;
                                            }
                                            reader.Dispose();
                                            reader = null;

                                            if (intNal > dblValue1)
                                            {
                                                ComFunc.MsgBox(strSugaTT + "수가는 재원기간중 " + dblValue1 + "일만 사용가능합니다..", "입원 OCS 재원중 일수제한");
                                                rtnVal = "NO";
                                                return rtnVal;
                                            }
                                        }

                                        if (reader != null)
                                        {
                                            reader.Dispose();
                                            reader = null;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_ORDER_SUGA_ETC
        /// ocs 제한사항 2015-01-20
        /// </summary>
        /// <returns></returns>
        public static string CHK_ORDER_SUGA_ETC(PsmhDb pDbCon, string strGbn, string strPtNo, string strBDate, string strJumin, string strBirth, string strSuCode, string strBun,
                                                    string strDept, FarPoint.Win.Spread.FpSpread ssSpread, int intSelfCol, int intSugaCol, int intBunCol, string strVerBal, string strInDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            int i = 0;
            int k = 0;

            int intCnt = 0;

            string strTemp = "";

            string strSugaTT = "";

            string strTSelf = "";
            string strTSuGa = "";
            string strTBun = "";

            string strJumin1 = "";
            string strJumin2 = "";

            strJumin1 = VB.Left(strJumin, 6);
            strJumin2 = VB.Right(strJumin, 7);

            strBDate = strBDate.Replace("-", "");

            rtnVal = "OK";

            try
            {
                //입원제한
                if (strGbn == "OCS_암표지자수가")
                {
                    //현재까지 사용수가 체크
                    SQL = "";
                    SQL = "SELECT Code ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCode ";
                    SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='" + strGbn + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND (DelDate IS NULL OR DelDate ='')";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            strSugaTT = reader.GetValue(0).To<string>();

                            for (k = 0; k < ssSpread.ActiveSheet.NonEmptyRowCount; k++)
                            {
                                if (ssSpread.ActiveSheet.Cells[k, 0].Text != "True")
                                {
                                    strTSelf = ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim().IsNullOrEmpty() ? "0" : ssSpread.ActiveSheet.Cells[k, intSelfCol].Text.Trim();
                                    strTSuGa = ssSpread.ActiveSheet.Cells[k, intSugaCol].Text.Trim();
                                    strTBun = ssSpread.ActiveSheet.Cells[k, intBunCol].Text.Trim();

                                    //시트수가체크
                                    if (strTSelf.Equals("0") && strSugaTT.Equals(strTSuGa))
                                    {
                                        intCnt = intCnt + 1;
                                        strTemp = strTSuGa + " ";
                                    }
                                }
                            }
                        }
                    }
                    reader.Dispose();
                    reader = null;
                }

                if (intCnt > 3)
                {
                    rtnVal = intCnt + "발생!!";
                    ComFunc.MsgBox("암표지가 검사는 3종이상 처방할수 없습니다.." + ComNum.VBLF + "수가정보:" + strTemp.Trim(), "암표지자 검사제한");
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "CHK_ORDER_SUGA_ETC" + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_SUGA_ORDER_CHK1
        /// 2013-12-07
        /// </summary>
        /// <returns></returns>
        public static string CHK_SUGA_ORDER_CHK1(PsmhDb pDbCon, string strPtNo, string strInDate, int intIpdNo)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            int i = 0;

            StringBuilder strMsg = new StringBuilder();

            rtnVal = "OK";

            try
            {
                //입원기간중 일자별 건수
                SQL = "";
                SQL = "SELECT BDATE,SUCODE,SUM(QTY*NAL) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER ";
                SQL = SQL + ComNum.VBLF + "  WHERE BDate >= TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND BDate <= TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "   AND SUCODE ='KERO' ";
                SQL = SQL + ComNum.VBLF + "   AND PTNO ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GbPrn =' ' ";
                SQL = SQL + ComNum.VBLF + "   AND GbStatus  IN  (' ','D+','D','D-')";
                SQL = SQL + ComNum.VBLF + "   AND (GbSelf ='0' OR GbSelf IS NULL ) ";  //2015-05-15
                SQL = SQL + ComNum.VBLF + "   AND ( ORDERSITE NOT IN ('CAN','NDC') OR ORDERSITE IS NULL)";
                SQL = SQL + ComNum.VBLF + "  GROUP BY PTNO,BDATE,SUCODE";
                SQL = SQL + ComNum.VBLF + "   HAVING SUM(QTY * NAL) > 0";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        strMsg.AppendLine(reader.GetValue(0).ToString().Trim() + " " + reader.GetValue(1).ToString().Trim() + " 사용량: " + reader.GetValue(2).ToString().Trim());
                    }
                }

                reader.Dispose();
                reader = null;

                if (i >= 2)
                {
                    ComFunc.MsgBox(strMsg.ToString().Trim() + "KERO 약코드 , 입원기간중 2일만 사용가능합니다..!!" + ComNum.VBLF + "2일 이상 처방이 불가합니다...KERO오더를 빼고 전송하십시오.." + ComNum.VBLF + "문의사항 보험심사과:8032", "처방확인");
                    rtnVal = "NO";
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_AGE_MONTH_GESAN
        /// strJumin : 생년월일(6) + 주민번호(7)
        /// strDate : 나이를 계산할 기준일자 (YYYY-MM-DD)
        /// *** 주민번호가 오류인 경우 12개월로 처리함 ***
        /// </summary>
        /// <returns></returns>
        public static int CHK_AGE_MONTH_GESAN(PsmhDb pDbCon, string strJumin, string strDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int rtnVal = 0;

            int intJuminLen = 0;
            decimal intAge = 0;

            string strSex = "";
            string strBirth = "";

            try
            {
                //주민번호가 7보다 적으면 오류
                //기준일자는 반드시 'YYYY - MM - DD' Type이여야 함
                intJuminLen = VB.Len(strJumin);

                if (intJuminLen < 7)
                {
                    rtnVal = 12;
                    return rtnVal;
                }

                if (VB.Len(strDate) != 10)
                {
                    rtnVal = 12;
                    return rtnVal;
                }

                //성별을 Setting
                strSex = "1";

                if (intJuminLen > 6)
                {
                    strSex = ComFunc.MidH(strJumin, 7, 1);
                }

                if (strSex == "-")
                {
                    if (intJuminLen > 7)
                    {
                        strSex = ComFunc.MidH(strJumin, 8, 1);
                    }
                    else
                    {
                        strSex = "1";
                    }
                }

                //생년월일을 YYYY-MM-DD Type으로 변경
                if (strSex == "1" || strSex == "2" || strSex == "5" || strSex == "6") 
                {
                    strBirth = "19" + ComFunc.LeftH(strJumin, 2) + "-" + ComFunc.MidH(strJumin, 3, 2) + "-" + ComFunc.MidH(strJumin, 5, 2);
                }
                else if (strSex == "3" || strSex == "4" || strSex == "7" || strSex == "8")
                {
                    strBirth = "20" + ComFunc.LeftH(strJumin, 2) + "-" + ComFunc.MidH(strJumin, 3, 2) + "-" + ComFunc.MidH(strJumin, 5, 2);
                }
                else if (strSex == "0" || strSex == "9")
                {
                    strBirth = "18" + ComFunc.LeftH(strJumin, 2) + "-" + ComFunc.MidH(strJumin, 3, 2) + "-" + ComFunc.MidH(strJumin, 5, 2);
                }
                else
                {
                    rtnVal = 12;
                    return rtnVal;
                }

                //주민번호가 오류이면 12개월 처리
                if (VB.IsDate(strBirth) == false)
                {
                    rtnVal = 12;
                    return rtnVal;
                }

                //기준일자가 생년월일보다 적으면 12개월 처리
                if (Convert.ToDateTime(strBirth) > Convert.ToDateTime(strDate))
                {
                    rtnVal = 12;
                    return rtnVal;
                }

                SQL = "";
                SQL = "SELECT CEIL(MONTHS_BETWEEN(TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + "       TO_DATE('" + strBirth + "','YYYY-MM-DD'))) cAge FROM DUAL";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows && reader.Read())
                {
                    intAge = Math.Truncate(reader.GetValue(0).To<decimal>(0));
                }

                reader.Dispose();
                reader = null;

                rtnVal = intAge.To<int>(0);
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_WEEK_CNT_GESAN
        /// </summary>
        /// <returns></returns>
        public static int CHK_WEEK_CNT_GESAN(PsmhDb pDbCon, string strBDate, string strSDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int rtnVal = 999;

            strSDate = DateTime.Parse(strSDate).ToShortDateString();

            try
            {
                SQL = "";
                SQL = "SELECT CEIL((TO_DATE('" + strBDate + "' ,'YYYY-MM-DD') - TO_DATE('" + strSDate + "','YYYY-MM-DD') ) /7) AS WEEKS";
                SQL = SQL + ComNum.VBLF + "     From DUAL";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).To<int>(0);
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : ALL_OCS_SUGA_CHK
        /// 2013-04-24 통합수가체크
        /// </summary>
        /// <returns></returns>
        public static string ALL_OCS_SUGA_CHK(PsmhDb pDbCon, string strGbn, string strPtNo, string strBDate, string strDeptCode, string strDrCode, string strBi, string strOk,
                                                string strSuCode, string strSelf, string strBun, string strJumin, string strBirth, string strSex,
                                                FarPoint.Win.Spread.FpSpread ssSpread, int intSelfCol, int intSugaCol, int intBunCol, string intPRN_Info1, string intPRN_Info2, string intPRN_Info3 = "")
        {
            string strSugaT = "";
            string rtnVal = "";

            strSugaT = strSuCode;   //2015-01-05

            rtnVal = strOk;

            if (strGbn == "외래")
            {
                #region //외래
                //골다공증체크는 수가제한으로 사용안함..의뢰서요청
                if (strOk == "OK" && strSelf == "0" && (strBun == "11" || strBun == "12" || strBun == "20"))
                {
                    if (CHK_ORDER_Result_AUTO(pDbCon, "1", strPtNo, strBDate, strSugaT) != "OK")
                    {
                        ComFunc.MsgBox("골다공증 수가[" + strSugaT + "]!! 급여 불가능!!" + "\r\n\r\n" + "골다공증골절 진단일이 없는 경우 또는 1년이내 검사결과가 없거나, 결과값이 -2.5이하 일경우 비급여처리 문의)심사과" + "\r\n\r\n" + "골다 공증 골절 진단일이 있거나 타병원 결과 있을경우 수동결과등록 후 전송하십시오!!", "검사체크확인");
                        strOk = "NO";
                    }
                }

                #region 2021-05-20 21, 22, 80 누락 추가
                if (strOk == "OK")
                {
                    strOk = CHK_BAS_MSELF_ORDER(pDbCon, strGbn, "21", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");    // 심사과 특정과 제한
                }

                if (strOk == "OK")
                {
                    strOk = CHK_BAS_MSELF_ORDER(pDbCon, strGbn, "22", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");    // 심사과 특정과 제한
                }

                if (strOk == "OK")
                {
                    strOk = CHK_BAS_MSELF_ORDER(pDbCon, strGbn, "22", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");    // 심사과 특정과 제한
                }
                #endregion

                if (strOk == "OK" && strSelf == "0")
                {
                    if (strSex == "M")
                    {
                        strOk = CHK_BAS_MSELF_ORDER(pDbCon, "외래", "71", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");        // 심사과 남자 특정과 , 특정수가 비급여
                    }
                    else if (strSex == "F")
                    {
                        strOk = CHK_BAS_MSELF_ORDER(pDbCon, "외래", "72", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");     // 심사과 여자 특정과 , 특정수가 비급여
                    }
                }

                //2013-04-29 입원전용약 체크
                if (strOk == "OK" && (strBun == "11" || strBun == "12" || strBun == "20"))
                {
                    strOk = Chk_Suga_Drug_No_Use(pDbCon, strOk, strSugaT);
                }

                //2014-03-04
                if (strOk == "OK" && strDeptCode == "NP")
                {
                    //2021-01-12 변경
                    //strOk = Chk_Suga_NP_NAL_CHK(pDbCon, strOk, strSugaT, ssSpread, 0, 7);
                    strOk = Chk_Suga_NP_NAL_CHK(pDbCon, strOk, strSugaT, ssSpread, 0, 8);
                }

                //초음파 보험코드 점검 메시지 2013 - 09 - 27
                CHK_ORDER_SONO_BOHUM(pDbCon, "", strPtNo, strBDate, strSugaT, strBun);

                //2014-07-21
                if (strOk == "OK")
                {
                    strOk = CHK_BAS_MSELF_ORDER(pDbCon, "외래", "87", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");        //심사과 기간별 갯수제한
                }

                //2015-03-23
                if (strOk == "OK")
                {
                    strOk = CHK_BAS_MSELF_ORDER(pDbCon, "외래", "88", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");        //심사과 기간별 갯수제한
                }

                //2015-05-28
                if (strOk == "OK")
                {
                    strOk = CHK_BAS_MSELF_ORDER(pDbCon, "외래", "23", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");        // 심사과 특정과 제한
                }

                //2015-02-27
                if (strOk == "OK" && strSelf == "0")
                {
                    CHK_ORDER_SONO_BOHUM2(pDbCon, ssSpread, strPtNo, strBDate, strSugaT);
                }

                //2015-05-04 용법체크
                if (strOk == "OK")
                {
                    if (strSuCode == "C8050010")
                    {
                        strOk = CHK_ORDER_DOSCODE(strSuCode, strBun, ssSpread, 17, intSugaCol);
                    }
                }

                if (strOk == "OK")
                {
                    if (strSuCode == "INFAN" || strSuCode == "MMR2")
                    {
                        strOk = CHK_ORDER_PD_JUSA_CHK(pDbCon, strPtNo, strBDate, strSuCode);
                    }
                }

                //2018.10.23
                if (strOk == "OK")
                {
                    strOk = CHK_BAS_MSELF_ORDER(pDbCon, "외래", "OCS_처방제한_기간별수가제한", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, clsPublic.GstrJobMan);       
                }

                //if (strOk == "OK")
                //{
                //    strOk = CHK_BAS_MSELF_ORDER(pDbCon, "외래", "OCS_처방제한_기간별수가제한", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, clsPublic.GstrJobMan);
                //}

                if (strOk == "OK" && string.Compare(strBDate, "2019-02-01") >= 0)
                {
                    //2020-12-10 안정수, 심사팀요청으로 초음파 관련 안내팝업 안뜨도록 주석처리 
                    //strOk = CHK_BOHUM_LOWDER_ADM_SONO_CHK(pDbCon, strPtNo, strBDate, strSugaT, strSelf);
                }

                #endregion //외래
            }

            if (strGbn == "입원")
            {
                #region //입원
                if (strOk == "OK" && strSelf == "0" && (strBun == "11" || strBun == "12" || strBun == "20"))
                {
                    //if (CHK_ORDER_Result_AUTO(pDbCon, "1", strPtNo, strBDate, strSugaT) != "OK")
                    //{
                    //    ComFunc.MsgBox("골다공증 수가[" + strSugaT + "]!! 급여 불가능!!" + ComNum.VBLF + "1년이내 검사결과가 없거나, 결과값이 -2.5이하 일경우 비급여처리 문의)심사과" + ComNum.VBLF + "타병원 결과 있을경우 수동결과등록후 전송하십시오!!", "검사체크확인");
                    //    strOk = "NO";
                    //}
                }

                #region 2021-05-20 21, 22 누락 추가
                if (strOk == "OK")
                {
                    strOk = CHK_BAS_MSELF_ORDER(pDbCon, strGbn, "21", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");    // 심사과 특정과 제한
                }

                if (strOk == "OK")
                {
                    strOk = CHK_BAS_MSELF_ORDER(pDbCon, strGbn, "22", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");    // 심사과 특정과 제한
                }
                #endregion

                if (strOk == "OK" && strSelf == "0")
                {
                    if (strSex == "M")
                    {
                        strOk = CHK_BAS_MSELF_ORDER(pDbCon, "입원", "71", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, clsPublic.GstrJobMan);       //심사과 남자 특정과 , 특정수가 비급여
                    }
                    else if (strSex == "F")
                    {
                        strOk = CHK_BAS_MSELF_ORDER(pDbCon, "입원", "72", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, clsPublic.GstrJobMan);       // 심사과 여자 특정과 , 특정수가 비급여
                    }

                    if (strOk == "OK")
                    {
                        strOk = CHK_BAS_MSELF_ORDER(pDbCon, "입원", "81", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, clsPublic.GstrJobMan);       // 심사과 동시처방불가 2014-03-17
                    }

                    //2016-01-18
                    if (strOk == "OK")
                    {
                        strOk = CHK_BAS_MSELF_ORDER(pDbCon, "입원", "89", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, clsPublic.GstrJobMan);       // 심사과 재원기간중 특정수가기준 기간제한 2014-03-17
                    }

                    if (strSex == "M" && strOk == "OK")
                    {
                        strOk = CHK_BAS_MSELF_ORDER(pDbCon, "입원", "91", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, clsPublic.GstrJobMan);       // 심사과 동시처방불가 2014-03-17
                    }
                }

                if (strOk == "OK" && clsPublic.GstrPRNChk == "OK" && clsPublic.Gstr간호처방STS == "PRN" && strSuCode != "")     //vb60_new\ocs\vb_ocs_all.bas
                {
                    //2020-11-27 intPRN_Info3 추가
                    strOk = READ_PRN_ORDER_CHK(pDbCon, strPtNo, strDeptCode, strBDate, strSuCode, intPRN_Info1, intPRN_Info2, intPRN_Info3);
                }

                if (strOk == "OK" && clsPublic.Gstr구두Chk == "OK" && clsPublic.Gstr간호처방STS == "구두처방" && (strBun == "11" || strBun == "12" || strBun == "20") && strSuCode != "")
                {
                    strOk = Read_Verb_2015_Suga_Chk(pDbCon, strSuCode);
                }

                if (strOk == "OK")
                {
                    strOk = CHK_BAS_MSELF_ORDER(pDbCon, "입원", "880", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");       // 심사과 기간별 갯수제한
                }

                //2018.10.23
                if (strOk == "OK")
                {
                    strOk = CHK_BAS_MSELF_ORDER(pDbCon, "입원", "OCS_처방제한_기간별수가제한", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, clsPublic.GstrJobMan);                    
                }

                if (strOk == "OK" && string.Compare(strBDate, "2019-02-01") >= 0)
                {
                    ////2020-12-10 안정수, 심사팀요청으로 초음파 관련 안내팝업 안뜨도록 주석처리 
                    //strOk = CHK_BOHUM_LOWDER_ADM_SONO_CHK(pDbCon, strPtNo, strBDate, strSugaT, strSelf);   
                }
                #endregion //입원
            }

            if (strGbn == "응급")
            {
                #region //응급
                //2014-12-04 최선택과장 요청
                if (strOk == "OK" && strPtNo == "07815297" && strBun == "72")
                {
                    ComFunc.MsgBox("CT처방시 확인후 오더를 넣으십시오" + ComNum.VBLF + "최근몇년 주기적으로 CT내역이 많은 환자입니다 참고하십시오!!", "오더참고");
                }

                //vb60_new\ocs\vb_ocs_all.bas
                if (strOk == "OK" && clsPublic.GstrPRNChk == "OK" && clsPublic.Gstr간호처방STS == "PRN" && strSuCode != "")
                {
                    strOk = READ_PRN_ORDER_CHK(pDbCon, strPtNo, strDeptCode, strBDate, strSuCode, intPRN_Info1, intPRN_Info2);
                }

                if (strOk == "OK" && clsPublic.Gstr구두Chk == "OK" && clsPublic.Gstr간호처방STS == "구두처방" && (strBun == "11" || strBun == "12" || strBun == "20") && strSuCode != "")
                {
                    strOk = Read_Verb_2015_Suga_Chk(pDbCon, strSuCode);
                }

                //2016-01-18
                if (strOk == "OK" && strSelf == "0")
                {
                    if (strSex == "M")
                    {
                        strOk = CHK_BAS_MSELF_ORDER(pDbCon, "응급", "71", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");     //심사과 남자 특정과 , 특정수가 비급여
                    }
                    else if (strSex == "F")
                    {
                        strOk = CHK_BAS_MSELF_ORDER(pDbCon, "응급", "72", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");     //심사과 여자 특정과 , 특정수가 비급여
                    }
                }

                #region 2021-05-20 21, 22 누락 추가
                if (strOk == "OK")
                {
                    strOk = CHK_BAS_MSELF_ORDER(pDbCon, strGbn, "21", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");    // 심사과 특정과 제한
                }

                if (strOk == "OK")
                {
                    strOk = CHK_BAS_MSELF_ORDER(pDbCon, strGbn, "22", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");    // 심사과 특정과 제한
                }
                #endregion

                //2016-01-18
                if (strOk == "OK")
                {
                    strOk = CHK_BAS_MSELF_ORDER(pDbCon, "응급", "23", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");    // 심사과 특정과 제한
                }

                //2018.10.23
                if (strOk == "OK")
                {
                    strOk = CHK_BAS_MSELF_ORDER(pDbCon, "응급", "OCS_처방제한_기간별수가제한", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, clsPublic.GstrJobMan);
                }

                if (strOk == "OK" && string.Compare(strBDate, "2019-02-01") >= 0)
                {
                    //2020-12-10 안정수, 심사팀요청으로 초음파 관련 안내팝업 안뜨도록 주석처리 
                    //strOk = CHK_BOHUM_LOWDER_ADM_SONO_CHK(pDbCon, strPtNo, strBDate, strSugaT, strSelf);
                }
                #endregion //응급
            }
            else
            {
                if (strOk == "OK" && strSelf == "0" && (strBun == "11" || strBun == "12" || strBun == "20"))
                {
                    //2019-11-15 신규 DUR 개발로 인해 약제팀 DUR점검 제외처리
                    //strOk = CHK_BAS_MSELF_ORDER(pDbCon, "", "10", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");      //연령금기 약제과용
                }

                if (strOk == "OK" && strSelf == "0" && (strBun == "11" || strBun == "12" || strBun == "20"))
                {
                    //2019-11-15 신규 DUR 개발로 인해 약제팀 DUR점검 제외처리
                    //strOk = CHK_BAS_MSELF_ORDER(pDbCon, "", "09", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "");       // 병용금기 약제과용
                }
            }

            rtnVal = strOk;
            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_PRN_ORDER_CHK
        /// </summary>
        /// <returns></returns>
        public static string READ_PRN_ORDER_CHK(PsmhDb pDbCon, string strPtNo, string strDept, string strBDate, string strSuCode, string strPRN_Info, string strPRN_Info2, string strPRN_Info3 = "")
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "";

            string strOK2 = "";

            double dbl1Time1 = 0;       //1회 최대용량
            double dbl1Time2 = 0;       //1회 최대일투량
            double dbl24Time1 = 0;      //24시간 최대용량
            double dbl24Time2 = 0;      //24시간 최대일투량

            int intMax = 0;

            string strInsulin = "";

            string strPRN_Date1 = "";
            string strPRN_Date2 = "";

            rtnVal = "OK";

            try
            {
                //PRN 수가 정보 체크
                SQL = "";
                SQL = "SELECT d.MAXQTY_1TIME_CONTENTS, d.MAXQTY_1TIME_UNIT, d.MAXQTY_1TIME_QTY, d.MAXQTY_1DAY_CONTENTS, d.MAXQTY_1DAY_UNIT,";
                SQL = SQL + ComNum.VBLF + "     d.MAXQTY_1DAY_QTY, d.MAXQTY_CNT, d.MAXQTY_GUBUN1, d.MAXQTY_GUBUN2,d.MaxQty_Required ,d.MaxQty_Insulin  ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_ADM.DRUG_MASTER3 c, KOSMOS_ADM.DRUG_MASTER4 d ";
                SQL = SQL + ComNum.VBLF + "     WHERE c.JEPCODE = d.JEPCODE ";
                SQL = SQL + ComNum.VBLF + "     AND c.PRN ='1' ";
                SQL = SQL + ComNum.VBLF + "     AND TRIM(c.JepCode)  ='" + strSuCode.Trim() + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("함수명 : " + "READ_PRN_ORDER_CHK" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "READ_PRN_ORDER_CHK" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strOK2 = "OK";

                    dbl1Time1 = VB.Val(dt.Rows[0]["MAXQTY_1TIME_CONTENTS"].ToString().Trim());
                    dbl1Time2 = VB.Val(dt.Rows[0]["MAXQTY_1TIME_QTY"].ToString().Trim());
                    dbl24Time1 = VB.Val(dt.Rows[0]["MAXQTY_1DAY_CONTENTS"].ToString().Trim());
                    dbl24Time2 = VB.Val(dt.Rows[0]["MAXQTY_1DAY_QTY"].ToString().Trim());

                    intMax = Convert.ToInt32(VB.Val(dt.Rows[0]["MAXQTY_CNT"].ToString().Trim()));

                    if (dt.Rows[0]["MAXQTY_Insulin"].ToString().Trim() == "1")
                    {
                        //인슐린
                        strInsulin = "OK";

                        if (VB.Val(strPRN_Info) > 0)
                        {
                            intMax = Convert.ToInt32(VB.Val(strPRN_Info));
                        }

                        strPRN_Date1 = VB.Pstr(strPRN_Info2, "^^", 1);
                        strPRN_Date2 = VB.Pstr(strPRN_Info2, "^^", 2);

                        if (strPRN_Date2 == "")
                        {
                            strPRN_Date2 = clsPublic.GstrSysDate;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                if (clsPublic.GstrPRNChk_new == "OK" && strInsulin != "OK")
                {
                    intMax = Convert.ToInt32(VB.Val(strPRN_Info));      //실제 입력한 제한횟수
                }

                if (strOK2 == "OK")
                {
                    //코드 체크
                    SQL = "";
                    SQL = "SELECT PTNO,BDATE,SuCode";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PTNO ='" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND SUCODE ='" + strSuCode + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND DEPTCODE ='" + strDept + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND NURSEID <> ' '  ";     // PRN 처방 조건
                    SQL = SQL + ComNum.VBLF + "  AND GBACT ='*' ";      // PRN 처방 조건
                    SQL = SQL + ComNum.VBLF + "  AND (GBPRN IS NULL OR GBPRN <> 'P') ";
                    SQL = SQL + ComNum.VBLF + "  AND  gbstatus NOT IN ('D','D-') ";
                    SQL = SQL + ComNum.VBLF + "  AND ORDERSITE NOT IN('NDC', 'CAN') ";

                    //2020-11-27 추가
                    if(strPRN_Info3 != "")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND GBGROUP = '" + strPRN_Info3 + "'";
                    }

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("함수명 : " + "READ_PRN_ORDER_CHK" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog("함수명 : " + "READ_PRN_ORDER_CHK" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (intMax < dt.Rows.Count)
                    {
                        rtnVal = "";
                        ComFunc.MsgBox("[" + strSuCode + "] >>  PRN 최대투여 횟수를 사용하였습니다.." + "\r\n\r\n" + "최대투여횟수 : " + intMax, "처방불가");
                    }
                    else
                    {
                        ComFunc.MsgBox("[" + strSuCode + "] >>  PRN 최대투여 횟수정보.." + "\r\n\r\n" + "최대투여횟수 : " + intMax + "\r\n\r\n" + "현재까지 투여한 횟수 : " + dt.Rows.Count, "정보");
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    rtnVal = "";
                    ComFunc.MsgBox("PRN 수가정보가 없습니다... 확인후 작업하십시오", "확인");
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("함수명 : " + "READ_PRN_ORDER_CHK" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "READ_PRN_ORDER_CHK" + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_치매약제_CHK
        /// MTSIORDER / MTSOORDER
        /// </summary>
        /// <returns></returns>
        public static void READ_DEMENTIA_MEDICINE_CHK(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread ssSpread, string strPano)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int i = 0;

            string strSuCode = "";
            string strOK = "";

            try
            {
                SQL = "";
                SQL = "SELECT NAME";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'OCS_치매약제_시행'";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '시행'";
                SQL = SQL + ComNum.VBLF + "   AND NAME = 'Y'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    reader = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                reader.Dispose();
                reader = null;

                SQL = "";
                SQL = "SELECT SUNEXT";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_SUN ";
                SQL = SQL + ComNum.VBLF + " WHERE GBDEMENTIA  ='Y' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (reader.HasRows)
                {
                    strSuCode = "SUCODE";
                    //for (i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    strSuCode = strSuCode + "'" + dt.Rows[i]["SUNEXT"].ToString().Trim() + "',";
                    //}

                    //strSuCode = VB.Mid(strSuCode, 1, VB.Len(strSuCode) - 1);
                }

                reader.Dispose();
                reader = null;

                if (strSuCode == "")
                {
                    return;
                }

                //switch(VB.UCase(App.EXEName))
                //{
                //    case "MTSIORDER":
                //    case "MTSOORDER":
                //        break;
                //    default:
                //        return;
                //}

                for (i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        //if(VB.UCase(App.EXEName) == "MTSOORDER")
                        //{
                        //    strTSuGa = ssSpread.ActiveSheet.Cells[i, 12].Text.Trim();
                        //}
                        //else if(VB.UCase(App.EXEName) == "MTSIORDER")
                        //{
                        //    strTSuGa = ssSpread.ActiveSheet.Cells[i, 14].Text.Trim();
                        //}
                    }
                }

                if (strOK != "OK")
                {
                    return;
                }

                if (strOK == "OK")
                {
                    SQL = "";
                    SQL = "SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.ETC_RESULT_DEMENTIA ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO ='" + strPano + "' ";      //주석해제해야함
                    SQL = SQL + ComNum.VBLF + " ORDER BY BDATE DESC  ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (reader.HasRows && reader.Read())
                    {
                        ComFunc.MsgBox("치매검사 최근 처방일자 : " + reader.GetValue(0).ToString().Trim() + "입니다..", "확인");
                    }

                    reader.Dispose();
                    reader = null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_근막근통유발점주사_CHK
        /// MTSIORDER / MTSOORDER
        /// </summary>
        /// <returns></returns>
        public static string READ_FASCIA_MYALGIA_POINT_JUSA_CHK(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread ssSpread, string strDrCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int i = 0;

            string rtnVal = "";

            string strTSuGa = "";

            string strOK = "";

            //근막근통유발점주사 자격 병동처방에서는 시행하지 않습니다.

            //if(VB.UCase(App.EXEName) == "MTSIORDER")
            //{
            //    rtnVal = "OK";
            //    return rtnVal;
            //}

            try
            {
                SQL = "";
                SQL = "SELECT NAME";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'OCS_근막통주사자격_시행'";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '시행'";
                SQL = SQL + ComNum.VBLF + "   AND NAME = 'Y'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows == false)
                {
                    rtnVal = "OK";
                    reader.Dispose();
                    reader = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                reader.Dispose();
                reader = null;

                //switch(VB.UCase(App.EXEName))
                //{
                //    case "MTSIORDER":
                //    case "MTSOORDER":
                //        break;
                //    default:
                //        rtnVal = "OK";
                //        return rtnVal;
                //}

                for (i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        //if(VB.UCase(App.EXEName) == "MTSOORDER")
                        //{
                        //    strTSuGa = ssSpread.ActiveSheet.Cells[i, 12].Text.Trim();
                        //}
                        //else if(VB.UCase(App.EXEName) == "MTSIORDER")
                        //{
                        //    strTSuGa = ssSpread.ActiveSheet.Cells[i, 14].Text.Trim();
                        //}

                        switch (strTSuGa)
                        {
                            case "MM131":
                            case "MM132":
                                strOK = "OK";
                                break;
                        }

                        if (strOK == "OK")
                        {
                            break;
                        }
                    }
                }

                if (strOK != "OK")
                {
                    rtnVal = "OK";
                    return rtnVal;
                }

                //의사코드로 조회합니다.
                if (strOK == "OK")
                {
                    SQL = "";
                    SQL = "SELECT NAME ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                    SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'OCS_근막근통유발점주사_자격'";
                    SQL = SQL + ComNum.VBLF + "   AND CODE = '" + strDrCode + "' ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (reader.HasRows == false)
                    {
                        rtnVal = "NO";
                    }
                    else
                    {
                        rtnVal = "OK";
                    }

                    reader.Dispose();
                    reader = null;
                }

                if (rtnVal != "OK")
                {
                    ComFunc.MsgBox("근막근통유발점주사자극치료(MM131, MM132)는 " + ComNum.VBLF +
                                    "동통재활분야 교육이수한경우에만 처방가능합니다.", "확인");
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_항생제_상병_CHK
        /// MTSIORDER / MTSOORDER
        /// </summary>
        /// <returns></returns>
        public static string READ_ANTIBIOTICS_ILL_CHK(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread ssSpread, FarPoint.Win.Spread.FpSpread ssSpread2)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int i = 0;
            int k = 0;

            string rtnVal = "";

            string strOK1 = "";     //항생제 수가 존재 여부
            string strOK2 = "";     //상병 중 J0 ~ J06(급성 상기도 감염 여부
            List<string> strCode = null;

            string strTSuGa = "";
            string strIllCode = "";

            //근막근통유발점주사 자격 병동처방에서는 시행하지 않습니다.

            //if(VB.UCase(App.EXEName) != "MTSOORDER")
            //{
            //    rtnVal = "OK";
            //    return rtnVal;
            //}

            try
            {
                SQL = "";
                SQL = "SELECT NAME";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'OCS_항생제_상병_시행'";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '시행'";
                SQL = SQL + ComNum.VBLF + "   AND NAME = 'Y'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows == false)
                {
                    rtnVal = "OK";

                    reader.Dispose();
                    reader = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                reader.Dispose();
                reader = null;

                strOK1 = "";
                strOK2 = "";

                SQL = "";
                SQL = "SELECT SUCODE ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_ANTI_SIMSA";
                SQL = SQL + ComNum.VBLF + " WHERE DELDATE IS NULL ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows == false)
                {
                    rtnVal = "OK";

                    reader.Dispose();
                    reader = null;
                    Cursor.Current = Cursors.Default;

                    return rtnVal;
                }

                strCode = new List<string>();

                while(reader.Read())
                {
                    strCode.Add(reader.GetValue(0).ToString().Trim());
                }

                reader.Dispose();
                reader = null;

                for (i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        //2021-01-12 변경
                        //strTSuGa = ssSpread.ActiveSheet.Cells[i, 12].Text.Trim();
                        strTSuGa = ssSpread.ActiveSheet.Cells[i, 17].Text.Trim();

                        if (strCode.Count > 0 && strOK1.IsNullOrEmpty())
                        {
                            if (strCode.Any(d => d.Equals(strTSuGa)))
                            {
                                strOK1 = "OK";
                            }
                        }
                    }
                }

                if (strOK1 != "OK")
                {
                    rtnVal = "OK";
                    return rtnVal;
                }

                if (clsOrdFunction.GEnvSet_Item21 != null && clsOrdFunction.GEnvSet_Item21 == "2")
                {
                    strIllCode = ssSpread2.ActiveSheet.Cells[0, 2].Text.Trim();
                }
                else
                {
                    strIllCode = ssSpread2.ActiveSheet.Cells[0, 0].Text.Trim();
                }
                    

                switch (VB.Left(strIllCode, 3))
                {
                    case "J00":
                    case "J01":
                    case "J02":
                    case "J03":
                    case "J04":
                    case "J05":
                        strOK2 = "OK";
                        break;
                    case "J06":
                        if (strIllCode == "J06")
                        {
                            strOK2 = "OK";
                        }
                        break;
                }

                if (strOK2 != "OK")
                {
                    rtnVal = "OK";
                    return rtnVal;
                }

                if (strOK1 == "OK" && strOK2 == "OK")
                {
                    if (ComFunc.MsgBoxQ(" ★★ 항생제 적성평가 대상자입니다 ★★ " + ComNum.VBLF +
                                           " 감기나 상기도 감염 상병(J00~ J06)에는" +
                                           " 항생제가 권장되지 않습니다." + ComNum.VBLF +
                                           " 처방이 필요한경우  주상병코드를 변경 해주세요." + ComNum.VBLF +
                                           " ★★ 상병이나 처방을 수정하시겠습니까? ★★",
                                           "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        rtnVal = "NO";
                    }
                    else
                    {
                        rtnVal = "OK";
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;

                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_임신주수_초음파코드
        /// </summary>
        /// <returns></returns>
        public static bool READ_PREGNANCY_WEEK_NUMBER_USG_CODE(PsmhDb pDbCon, string strCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수            
            bool rtnVal = false;

            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL = "SELECT CODE ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'OCS_임신주수입력_오더코드'";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '" + strCode + "'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = true;
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_분만수가
        /// </summary>
        /// <returns></returns>
        public static bool READ_CHILDBIRTH_SUGA(PsmhDb pDbCon, string strCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int i = 0;

            bool rtnVal = false;

            StringBuilder SQLSuCode = new StringBuilder();
            string strSuCode = string.Empty;

            try
            {
                //SQL = "";
                //SQL = "SELECT CODE ";
                //SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE";
                //SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'OCS_분만수가코드'";

                //SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                //if (SqlErr != "")
                //{
                //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                //    Cursor.Current = Cursors.Default;
                //    return rtnVal;
                //}
                //if (reader.HasRows)
                //{
                //    while(reader.Read())
                //    {
                //        SQLSuCode.Append("'" + reader.GetValue(0).ToString().Trim() + "',");
                //    }

                //    strSuCode = SQLSuCode.ToString().Trim();
                //}

                //reader.Dispose();
                //reader = null;

                SQL = "";
                SQL = "SELECT ORDERCODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_ORDERCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE TRIM(SUCODE) IN ";
                SQL = SQL + ComNum.VBLF + " ( ";
                SQL = SQL + ComNum.VBLF + "     SELECT CODE ";
                SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "      WHERE GUBUN = 'OCS_분만수가코드'";
                SQL = SQL + ComNum.VBLF + " ) ";
                SQL = SQL + ComNum.VBLF + "   AND ORDERCODE = '" + strCode.Trim() + "' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = true;
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_4대중증초음파_CHK
        /// MTSOORDER
        /// </summary>
        /// <returns></returns>
        public static string READ_FOUR_SERIOUS_CHK(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread ssSpread, string strBi, string strResult)
        {
            //string SQL = "";
            //string SqlErr = ""; //에러문 받는 변수
            //DataTable dt = null;
            //int i = 0;

            //string strTSuGa = "";

            //string strOK = "";      //@V코드 여부
            //string strOK2 = "";     //4대중증 초음파

            //string strEB441 = "";
            //string strEB442 = "";

            //string rtnVal = "";

            //if (VB.InStr(strResult, "EB441") > 0)
            //{
            //    strEB441 = "OK";
            //}

            //if (VB.InStr(strResult, "EB442") > 0)
            //{
            //    strEB442 = "OK";
            //}

            //rtnVal = "OK";
            //return rtnVal;

            return "OK";

            //2019-02-01 외래도 시행하지 않는다

            //4대중증초음파 체크 병동처방에서는 시행하지 않습니다.
            //if (Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "D"), "D", "-")) < Convert.ToDateTime("2016-10-01"))
            //{
            //    rtnVal = "OK";
            //    return rtnVal;
            //}

            //if (clsOrdFunction.GstrGbJob != "OPD")
            //{
            //    rtnVal = "OK";
            //    return rtnVal;
            //}

            ////=================================================
            ////2016-12-21 보험심사과장 의뢰서(자보,산재는 제외)
            //switch (strBi)
            //{
            //    //산재
            //    case "31":
            //    case "32":
            //    case "33":
            //        rtnVal = "OK";
            //        return rtnVal;
            //    //자보
            //    case "52":
            //    case "55":
            //        rtnVal = "OK";
            //        return rtnVal;
            //}

            //try
            //{
            //    SQL = "";
            //    SQL = "SELECT NAME";
            //    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE";
            //    SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'OCS_4대중증초음파_시행'";
            //    SQL = SQL + ComNum.VBLF + "   AND CODE = '시행'";
            //    SQL = SQL + ComNum.VBLF + "   AND NAME = 'Y'";
            //    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //    if (SqlErr != "")
            //    {
            //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //        Cursor.Current = Cursors.Default;
            //        return rtnVal;
            //    }
            //    if (dt.Rows.Count == 0)
            //    {
            //        rtnVal = "OK";

            //        dt.Dispose();
            //        dt = null;
            //        Cursor.Current = Cursors.Default;
            //        return rtnVal;
            //    }

            //    dt.Dispose();
            //    dt = null;

            //    switch (clsOrdFunction.GstrGbJob)
            //    {
            //        case "OPD":
            //            break;
            //        default:
            //            rtnVal = "OK";
            //            return rtnVal;
            //    }

            //    for (i = 0; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
            //    {
            //        if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
            //        {
            //            if(clsOrdFunction.GstrGbJob == "OPD")
            //            {
            //                strTSuGa = ssSpread.ActiveSheet.Cells[i, 1].Text.Trim();

            //                switch (VB.Left(strTSuGa, 2))
            //                {
            //                    case "@V":
            //                        strOK = "OK";
            //                        break;
            //                    default:
            //                        if (READ_FOUR_SERIOUS_ILLNESS_USG(pDbCon, strTSuGa, strEB441, strEB442))
            //                        {
            //                            strOK2 = "OK";
            //                        }
            //                        break;
            //                }
            //            }
            //        }
            //    }

            //    //4대중증초음파 처방이 포함되고 @V코드가 없는 경우
            //    if (strOK2 == "OK" && strOK != "OK")
            //    {
            //        rtnVal = "NO";
            //    }
            //    else
            //    {
            //        rtnVal = "OK";
            //    }

            //    if (rtnVal != "OK")
            //    {
            //        ComFunc.MsgBox("4대중증환자는 '@V___' 처방" + ComNum.VBLF +
            //                        "4대중증 의심환자는 '@V999' 처방해주세요." + ComNum.VBLF +
            //                        "그 이외 환자는 비급여 판넬 초음파에서 처방 해주세요.", "확인");
            //    }

            //    return rtnVal;
            //}
            //catch (Exception ex)
            //{
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            //    ComFunc.MsgBox(ex.Message);
            //    Cursor.Current = Cursors.Default;
            //    return rtnVal;
            //}
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_내시경_급여구분_CHK
        /// MTSOORDER / MTSIORDER / EORDER
        /// </summary>
        /// <param name="ssSpread"></param>
        /// <param name="Pt_Info"></param>
        /// <returns></returns>
        public static string READ_ENDO_BOHOGUBUN_CHK(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread ssSpread, string strPtno)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int i = 0;
            int k = 0;

            string rtnVal = "";

            string strTSuGa = "";

            string strOK = "";      //@V코드 여부
            string strOK2 = "";     //비급여 수면 묶음코드 여부  @V 있으면 처방 안됨
            string strOK3 = "";     //급여 수면 묶음코드 여부    @V 없으면 처방 안됨

            List<string> strCode2 = null;      //비급여 수면 코드
            List<string> strCode3 = null;      //급여 수면 코드

            if (Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "D"), "D", "-")) < Convert.ToDateTime("2017-02-01"))
            {
                rtnVal = "OK";
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL = "SELECT CODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'BAS_V코드비필요내시경'";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    strCode2 = new List<string>();
                    while(reader.Read())
                    {
                        strCode2.Add(reader.GetValue(0).ToString().Trim());
                    }
                }

                reader.Dispose();
                reader = null;

                SQL = "";
                SQL = "SELECT CODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'BAS_V코드필요내시경'";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    strCode3 = new List<string>();
                    while (reader.Read())
                    {
                        strCode3.Add(reader.GetValue(0).ToString().Trim());
                    }
                }

                reader.Dispose();
                reader = null;

                //if(VB.UCase(App.EXEName) == "MTSIORDER")
                //{
                //    rtnVal = "OK";
                //    return rtnVal;
                //}

                for (i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        //수가코드 기준
                        //if(VB.UCase(App.EXEName) == "MTSOORDER")
                        //{
                        //    strTSuGa = ssSpread.ActiveSheet.Cells[i, 12].Text.Trim();
                        //}
                        //else if(VB.UCase(App.EXEName) == "EORDER")
                        //{
                        //    strTSuGa = ssSpread.ActiveSheet.Cells[i, 13].Text.Trim();
                        //}
                    }

                    switch (strTSuGa.Left(2))
                    {
                        //@V코드 여부
                        case "@V":
                            strOK = "OK";
                            break;
                        default:
                            //비급여 코드 비교
                            if (strCode2.Count > 0 && strOK2.IsNullOrEmpty())
                            {
                                if (strCode2.Any(d => d.Equals(strTSuGa)))
                                {
                                    strOK2 = "OK";
                                }
                            }

                            //급여 코드 비교
                            if (strCode3.Count > 0 && strOK3.IsNullOrEmpty())
                            {
                                if (strCode3.Any(d => d.Equals(strTSuGa)))
                                {
                                    strOK3 = "OK";
                                }
                            }
                            break;
                    }
                }

                if (strOK.IsNullOrEmpty())
                {
                    //보험코드 일 경우 차단
                    if (strOK3 == "OK")
                    {
                        //2017-02-04
                        SQL = "";
                        SQL = "SELECT TO_CHAR(TDATE,'YYYY-MM-DD') TDATE  ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_CANCER ";
                        SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + strPtno + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND GUBUN IN ('1','2') "; // 산정특례
                        SQL = SQL + ComNum.VBLF + "  ORDER BY FDATE DESC ";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        if (reader.HasRows == false)
                        {
                            rtnVal = "NO";
                            ComFunc.MsgBox("4대 중증 질환에만 해당되는 코드 입니다. 해당 산정 특례 코드를 처방해주세요.", "확인");

                            reader.Dispose();
                            reader = null;
                            Cursor.Current = Cursors.Default;
                        }
                        else
                        {
                            rtnVal = "OK";
                        }

                        reader.Dispose();
                        reader = null;
                    }
                    //둘다 해당 없으면 통과
                    else if (strOK2 == "" && strOK3 == "")
                    {
                        rtnVal = "OK";
                    }
                    //비급여 코드일 경우 통과
                    else if (strOK2 == "OK" && strOK3 == "")
                    {
                        rtnVal = "OK";
                    }
                }
                else if (strOK == "OK")
                {
                    //비급여 코드일 경우 차단
                    if (strOK2 == "OK")
                    {
                        rtnVal = "NO";
                        ComFunc.MsgBox("★4대 중증 질환 내시경을 선택해서 처방해주세요.", "확인");
                    }
                    //급여 코드일 경우 통과
                    else if (strOK2 == "" && strOK3 == "OK")
                    {
                        rtnVal = "OK";
                    }
                    //둘다 해당 없으면 통과
                    else if (strOK2 == "" && strOK3 == "")
                    {
                        rtnVal = "OK";
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_4대중증초음파
        /// 2018-05-30 박웅규 수정(2081-04-01 상복부 초음파 급여 전환)
        /// </summary>
        /// <returns></returns>
        public static bool READ_FOUR_SERIOUS_ILLNESS_USG(PsmhDb pDbCon, string strCode, string argEB441, string argEB442)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "SELECT ORDERCODE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_ORDERCODE";
                SQL = SQL + ComNum.VBLF + " WHERE (SEVERE = '1' OR SLIPNO = '0069')";
                SQL = SQL + ComNum.VBLF + "   AND ORDERCODE = '" + strCode + "'";
                if (argEB441 == "OK" && argEB442 == "OK")
                {
                    SQL = SQL + ComNum.VBLF + "   AND ORDERCODE NOT IN ('EB441','EB442')";
                }
                else if (argEB441 == "OK")
                {
                    SQL = SQL + ComNum.VBLF + "   AND ORDERCODE NOT IN ('EB441')";
                }
                else if(argEB442 == "OK")
                {
                    SQL = SQL + ComNum.VBLF + "   AND ORDERCODE NOT IN ('EB442')";
                }

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    reader = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                reader.Dispose();
                reader = null;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_TUBER_APPLY_CHK
        /// MTSIORDER / MTSOORDER
        /// </summary>
        /// <returns></returns>
        public static string READ_TUBER_APPLY_CHK(PsmhDb pDbCon, string strPtNo, string strDept, string strBDate, FarPoint.Win.Spread.FpSpread ssSpread, string strBi)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int i = 0;

            string strTSuGa = "";
            string strOK = "";

            string rtnVal = "";

            //switch(VB.UCase(App.EXEName))
            //{
            //    case "MTSIORDER":
            //    case "MTSOORDER":
            //        break;
            //    default:
            //        rtnVal = "OK";
            //        return rtnVal;
            //}

            try
            {
                SQL = "";
                SQL = "SELECT NAME";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN = 'OCS_결핵점검_사용여부'";
                SQL = SQL + ComNum.VBLF + "     AND CODE = 'USE'";
                SQL = SQL + ComNum.VBLF + "     AND NAME = 'N'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = "OK";

                    reader.Dispose();
                    reader = null;

                    return rtnVal;
                }

                reader.Dispose();
                reader = null;

                for (i = 0; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        //if(VB.UCase(App.EXEName) == "MTSOORDER")
                        //{
                        //    strTSuGa = ssSpread.ActiveSheet.Cells[i, 12].Text.Trim();
                        //}
                        //else if(VB.UCase(App.EXEName) == "MTSIORDER")
                        //{
                        //    strTSuGa = ssSpread.ActiveSheet.Cells[i, 14].Text.Trim();
                        //}

                        if (strTSuGa == "@V000")
                        {
                            strOK = "OK";
                            break;
                        }
                    }
                }

                if (strOK != "OK")
                {
                    rtnVal = "OK";
                    return rtnVal;
                }

                if (strOK == "OK")
                {
                    switch (strBi)
                    {
                        case "11":
                        case "12":
                        case "13":
                        case "21":
                        case "22":
                        case "23":
                        case "24":
                            break;
                        default:
                            rtnVal = "BI";
                            return rtnVal;
                    }

                    SQL = "";
                    SQL = "SELECT GBEND_DATE ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_STD_INFECT3";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "'";
                    SQL = SQL + ComNum.VBLF + "   AND TUBER24 <= '" + strBDate.Replace("-", "") + "'";
                    SQL = SQL + ComNum.VBLF + " ORDER BY TUBER24 DESC";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (reader.HasRows && reader.Read())
                    {
                        if (Convert.ToDateTime(reader.GetValue(0).ToString().Trim()) >= Convert.ToDateTime(strBDate)
                            || reader.GetValue(0).ToString().Trim().IsNullOrEmpty())
                        {
                            rtnVal = "OK";
                        }
                        else
                        {
                            rtnVal = "NO";
                        }
                    }
                    else
                    {
                        //신고서가 있을 경우에만 체크한다.
                        rtnVal = "OK";
                    }

                    reader.Dispose();
                    reader = null;
                }
                else
                {
                    rtnVal = "OK";
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_CHECK_V161
        /// </summary>
        /// <returns></returns>
        public static string READ_CHECK_V161(PsmhDb pDbCon, string strPtNo)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "NO";

            try
            {
                SQL = "";
                SQL = "SELECT SAYU ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_CHECK_V161 ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_조현병_V161_CHK
        /// MTSOORDER
        /// </summary>
        /// <returns></returns>
        public static string READ_SCHIZOPHRENIA_V161_CHK(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread ssSpread, string strILL, string strPtno, string strDeptCode, string strBi)
        {
            string strOrdOK = "";
            string strTSuGa = "";
            string rtnVal = "";
            int i = 0;

            if (Convert.ToDateTime(clsPublic.GstrSysDate) < Convert.ToDateTime("2017-03-13"))
            {
                rtnVal = "OK";
                return rtnVal;
            }

            //switch(VB.UCase(App.EXEName))
            //{
            //    case "MTSOORDER":
            //        break;
            //    default:
            //        rtnVal = "OK";
            //        return rtnVal;
            //}

            if (strDeptCode == "NP" && strBi == "22" && VB.Left(strILL, 2) == "F2" && (VB.Val(VB.Right(strILL, 3)) < 290 && VB.IsNumeric(VB.Right(strILL, 3)) == true))
            {

            }
            else
            {
                rtnVal = "OK";
                return rtnVal;
            }

            //사유가 있으면 pass
            if (READ_CHECK_V161(pDbCon, strPtno) == "OK")
            {
                rtnVal = "OK";
                return rtnVal;
            }

            for (i = 0; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    strTSuGa = ssSpread.ActiveSheet.Cells[i, 12].Text.Trim();

                    if (strTSuGa == "@V161")
                    {
                        strOrdOK = "OK";
                        break;
                    }
                }
            }

            //V161 코드가 입력되어 있으면 PASS
            if (strOrdOK == "OK")
            {
                rtnVal = "OK";
                return rtnVal;
            }

            //그외에는 멘트 후 처방 불가!
            ComFunc.MsgBox("★ 의료급여 2종 NP 환자 중 주 상병이 조현병(F20~F29) 일 경우 @V161을 반드시 입력해주시기 바랍니다." + ComNum.VBLF +
              "상단 메뉴 '기타작업 및 조회 -> @V161 사유입력' 에 사유를 등록하시면 해당 메시지가 표출되지 않습니다.", "확인");

            rtnVal = "NO";
            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_PT_MM101_102
        /// MTSIORDER / MTSOORDER
        /// </summary>
        /// <returns></returns>
        public static string READ_PT_MM101_102(string strGubun, FarPoint.Win.Spread.FpSpread ssSpread)
        {
            string strTSuGa = "";
            string strMM101 = "";
            string strMM102 = "";

            string rtnVal = "";

            int i = 0;

            //2017-02-07     MM101과 MM102 동시 처방 불가능하도록 보완 요청
            //switch(VB.UCase(App.EXEName))
            //{
            //    case "MTSIORDER":
            //    case "MTSOORDER":
            //        break;
            //    default:
            //        rtnVal = "OK";
            //        return rtnVal;
            //}

            for (i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
            {
                if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    if (strGubun == "OPD")
                    {
                        strTSuGa = ssSpread.ActiveSheet.Cells[i, (int)BaseOrderInfo.OpdOrderCol.SUCODE].Text.Trim();
                    }
                    else if (strGubun == "IPD")
                    {
                        strTSuGa = ssSpread.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text.Trim();
                    }
                    
                    if (strTSuGa == "MM101")
                    {
                        strMM101 = "OK";
                    }

                    if (strTSuGa == "MM102")
                    {
                        strMM102 = "OK";
                    }

                    //if (strMM101 == "OK")
                    //{
                    //    strMM102 = "OK";
                    //    break;
                    //}
                }
            }

            if (strMM101 == "OK" && strMM102 == "OK")
            {
                ComFunc.MsgBox("★ MM101(운동간단), MM102(운동복잡) 동시처방 불가능합니다." + ComNum.VBLF + "★MM101나 MM102 중 하나만 입력바랍니다!!!!", "확인");
                rtnVal = "NO";
                return rtnVal;
            }

            rtnVal = "OK";
            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_BAS_CANCER_CHK
        /// </summary>
        /// <returns></returns>
        public static string READ_BAS_CANCER_CHK(PsmhDb pDbCon, string strPtNo, string strDept, string strBDate, FarPoint.Win.Spread.FpSpread ssSpread)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;

            string strTSuGa = "";
            string strOK = "";
            string rtnVal = "";

            for (int i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
            {
                if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    strTSuGa = ssSpread.ActiveSheet.Cells[i, 12].Text.Trim();

                    if (VB.Left(strTSuGa, 2) == "@V")
                    {
                        strOK = "OK";
                        break;
                    }
                }
            }

            try
            {
                if (strOK == "OK")
                {
                    SQL = "";
                    SQL = "SELECT TO_CHAR(TDATE,'YYYY-MM-DD') TDATE  ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_CANCER ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN ='2' ";     //산정특례
                    SQL = SQL + ComNum.VBLF + "  ORDER BY FDATE DESC ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (reader.HasRows == false)
                    {
                        rtnVal = "";
                    }
                    else
                    {
                        if (reader.Read() && Convert.ToDateTime(reader.GetValue(0).ToString().Trim()) >= Convert.ToDateTime(strBDate))
                        {
                            rtnVal = "OK";
                        }
                        else
                        {
                            rtnVal = "NO";
                        }
                    }

                    reader.Dispose();
                    reader = null;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : ALL_OCS_SUGA_CHK_ETC
        /// 2015-01-07 통합수가체크
        /// </summary>
        /// <returns></returns> 
        public static string ALL_OCS_SUGA_CHK_ETC(PsmhDb pDbCon, string strGBN, string strPtNo, string strBDate, string strDeptCode, string strDrCode, string strBi,
                                                    string strOK, string strSuCode, string strSelf, string strBun, string strJumin, string strBirth,
                                                    string strSex, FarPoint.Win.Spread.FpSpread ssSpread, int intSelfCol, int intSugaCol, int intBunCol,
                                                    string strInDate, FarPoint.Win.Spread.FpSpread ssSpread2, int intMRow, int intDosCol, int intDosCol2,
                                                    int intE, int intR, int intEDateCol, int intPWSayuCol, int intQtyCol, int intDivCol, long intIpdNo,
                                                    long intTRSNO, int intAge, Form frm, int intPRN_MaxCol, Form frm2, int intRemarkCol, int intPRN_NotifyCol,
                                                    FarPoint.Win.Spread.FpSpread ssillSpread)
        {
            string strSugaT = "";
            string rtnVal = "";

            strSugaT = strSuCode;       //2015-01-05

            rtnVal = strOK;

            if (strGBN == "외래")
            {
                #region //외래
                strOK = CHK_ORDER_SUGA_ETC(pDbCon, "OCS_암표지자수가", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "", strInDate);     // 심사과 재원기간중 일수제한

                //2015-08-31
                if (Convert.ToDateTime(strBDate) >= Convert.ToDateTime("2015-09-01"))
                {
                    strOK = READ_JinDan_SONO_CHK(pDbCon, ssSpread, ssSpread2);
                }


                if (strOK == "OK" && Convert.ToDateTime(strBDate) >= Convert.ToDateTime("2015-10-01"))
                {
                    strOK = READ_VaccFlu_ORDER_CHK(pDbCon, "외래", ssSpread, intSugaCol, strJumin, strBDate, strBirth);
                }

                //심사과 세팅 수가 점검 2015-09-10
                if (strOK == "OK" && Convert.ToDateTime(strBDate) >= Convert.ToDateTime("2015-09-29"))
                {
                    strOK = CHK_OCS_ORDER_SUGA_CHK(pDbCon, "외래", "", ssSpread, intSugaCol, intSelfCol);
                }


                if (strOK == "OK")
                {
                    strOK = READ_OPD_BOHOJA_SUGA_CHK(ssSpread);     //2015-10-15
                }


                if (strOK == "OK")
                {
                    strOK = CHK_RA_SONO_ORDER_CHK(pDbCon, "외래", strPtNo, strDeptCode, ssSpread, intMRow, intSugaCol);
                }

                //2015-10-15
                if (strOK == "OK" && clsPublic.GstrNotDiv == "OK")
                {
                    strOK = CHK_NotDIV_ORDER_CHK(pDbCon, "외래", strPtNo, strSugaT, strBun, strDeptCode, ssSpread, intMRow, intSelfCol, intBunCol, intSugaCol, intQtyCol, intDivCol);
                }


                if (strOK == "OK" && clsPublic.GstrASA진정Chk == "OK")
                {
                    clsPublic.GnASA진정Row = 0;

                    if (strOK == "OK")
                    {
                        strOK = CHK_ASA_ORDER_CHK(pDbCon, "외래", strPtNo, strDeptCode, ssSpread, intMRow, intSugaCol);

                        //if (clsPublic.GnASA진정Row > 0 && strOK != "OK")
                        if (strOK != "OK")
                        {
                            clsOrdFunction.GstrASAFlag = "OK";
                            //frm.Show();

                            //if (VB.Len(clsPublic.Gstr마취신체등급) == 1)
                            //{
                            //    ssSpread.ActiveSheet.Cells[clsPublic.GnASA진정Row, 53].Text = clsPublic.Gstr마취신체등급;
                            //}

                            //clsPublic.Gstr마취신체등급 = "";
                        }
                    }
                }

                //2015-11-07
                if (strOK == "OK" && clsPublic.Gstr혈액사용예정일Chk == "OK")
                {
                    strOK = CHK_BLOOD_RDate_ORDER_CHK("외래", strPtNo, strDeptCode, ssSpread, intMRow, intSugaCol, intBunCol, intRemarkCol);

                    if (clsPublic.Gn혈액사용예정일Row > 0 && strOK != "OK")
                    {
                        //clsPublic.Gstr혈액사용예정일Date = "";
                        //frm2.ShowDialog();
                        clsOrdFunction.GstrBloodRsvDateFlag = "OK";

                        //ssSpread.ActiveSheet.Cells[clsPublic.Gn혈액사용예정일Row, intRemarkCol].Text = ssSpread.ActiveSheet.Cells[clsPublic.Gn혈액사용예정일Row, intRemarkCol].Text + "[혈액사용예정일:" + clsPublic.Gstr혈액사용예정일Date + "]";
                        //ssSpread.ActiveSheet.Cells[clsPublic.Gn혈액사용예정일Row, intR].Text = "#";
                    }
                }

                //2015-11-19
                if (strOK == "OK")
                {
                    strOK = READ_ORDER_ALLERGY_SUGA_CHK(pDbCon, "외래", strPtNo, ssSpread, intMRow, intSugaCol);
                }


                if (strOK == "OK" && Convert.ToDateTime(clsPublic.GstrSysDate) >= Convert.ToDateTime("2099-01-01"))
                {
                    strOK = CHK_ills_Code(pDbCon, ssillSpread, strBDate, "");
                }

                //2016-01-27
                if (strOK == "OK")
                {
                    strOK = CHK_MNHD_SUGA_ORDER_CHK("외래", strPtNo, strDeptCode, ssSpread, intMRow, intSugaCol, intSelfCol);
                }

                //2016-02-22
                if (strOK == "OK")
                {
                    strOK = CHK_CANCER_ORDER_CHK(pDbCon, "외래", strPtNo, strDeptCode, ssSpread, intMRow, intSugaCol);
                }

                //2019-02-01
                if (strOK == "OK" && string.Compare(strBDate,"2019-02-01") >= 0)
                {
                    strOK = CHK_LOWER_SONO_ORDER_CHK(pDbCon, ssSpread, intMRow, intSugaCol);
                }
                #endregion //외래
            }
            else if (strGBN == "입원")
            {
                #region //입원
                if (strOK == "OK" && VB.Val(strBi) <= 22)
                {
                    //외래제한
                    strOK = CHK_BAS_MSELF_ORDER_ETC(pDbCon, "86", strPtNo, strBDate, strJumin, strBirth, strSugaT, strBun, strDeptCode, ssSpread, intSelfCol, intSugaCol, intBunCol, "", strInDate, 56);    //심사과 재원기간중 일수제한
                }

                if (strOK == "OK" && clsPublic.GstrPRNChk == "OK")
                {
                    //strOK = CHK_PRN_ORDER_CHK(pDbCon, clsPublic.GstrJobMan, strPtNo, ssSpread, strBDate, 14, 12, 65, intMRow, intDosCol, intEDateCol, intPRN_MaxCol, intPRN_NotifyCol);
                    //2021-01-12
                    strOK = CHK_PRN_ORDER_CHK(pDbCon, clsPublic.GstrJobMan, strPtNo, ssSpread, strBDate, 17, 15, 68, intMRow, intDosCol, intEDateCol, intPRN_MaxCol, intPRN_NotifyCol);
                }

                if (strOK == "OK" && clsPublic.Gstr산제Chk == "OK")
                {
                    //strOK = CHK_POWDER_ORDER_CHK(clsPublic.GstrJobMan, strPtNo, ssSpread, strBDate, 14, 12, 65, intMRow, intDosCol, intEDateCol, intBunCol, intPWSayuCol);
                    //2021-01-12
                    strOK = CHK_PRN_ORDER_CHK(pDbCon, clsPublic.GstrJobMan, strPtNo, ssSpread, strBDate, 17, 15, 68, intMRow, intDosCol, intEDateCol, intPRN_MaxCol, intPRN_NotifyCol);
                }

                if (strOK == "OK")
                {
                    strOK = READ_VaccFlu_ORDER_CHK(pDbCon, "입원", ssSpread, intSugaCol, strJumin, strBDate, strBirth);
                }

                //입원 용법체크
                if (strOK == "OK")
                {
                    strOK = Read_Dos_Chk_STS_ER_A(pDbCon, ssSpread, intMRow, intSelfCol, strBun, intDosCol2, intE, intR);
                }

                //2015-10-15
                if (strOK == "OK" && clsPublic.GstrNotDiv == "OK")
                {
                    strOK = CHK_NotDIV_ORDER_CHK(pDbCon, "입원", strPtNo, strSugaT, strBun, strDeptCode, ssSpread, intMRow, intSelfCol, intBunCol, intSugaCol, intQtyCol, intDivCol);
                }

                if (strOK == "OK" && clsPublic.GstrASA진정Chk == "OK")
                {
                    clsPublic.GnASA진정Row = 0;

                    if (strOK == "OK")
                    {
                        strOK = CHK_ASA_ORDER_CHK(pDbCon, "입원", strPtNo, strDeptCode, ssSpread, intMRow, intSugaCol);

                        //if (clsPublic.GnASA진정Row > 0 && strOK != "OK")
                        if (strOK != "OK")
                        { 
                            clsOrdFunction.GstrASAFlag = "OK";
                            //frm.Show();

                            //if (VB.Len(clsPublic.Gstr마취신체등급) == 1)
                            //{
                            //    ssSpread.ActiveSheet.Cells[clsPublic.GnASA진정Row, 53].Text = clsPublic.Gstr마취신체등급;
                            //}

                            //clsPublic.Gstr마취신체등급 = "";
                        }
                    }
                }

                //2015-11-07
                if (strOK == "OK" && clsPublic.Gstr혈액사용예정일Chk == "OK")
                {
                    strOK = CHK_BLOOD_RDate_ORDER_CHK("외래", strPtNo, strDeptCode, ssSpread, intMRow, intSugaCol, intBunCol, intRemarkCol);

                    if (clsPublic.Gn혈액사용예정일Row > 0 && strOK != "OK")
                    {
                        //clsPublic.Gstr혈액사용예정일Date = "";
                        //frm2.ShowDialog();
                        clsOrdFunction.GstrBloodRsvDateFlag = "OK";

                        //ssSpread.ActiveSheet.Cells[clsPublic.Gn혈액사용예정일Row, intRemarkCol].Text = ssSpread.ActiveSheet.Cells[clsPublic.Gn혈액사용예정일Row, intRemarkCol].Text + "[혈액사용예정일:" + clsPublic.Gstr혈액사용예정일Date + "]";
                        //ssSpread.ActiveSheet.Cells[clsPublic.Gn혈액사용예정일Row, intR].Text = "#";
                    }
                }

                if (strOK == "OK")
                {
                    strOK = CHK_RA_SONO_ORDER_CHK(pDbCon, "입원", strPtNo, strDeptCode, ssSpread, intMRow, intSugaCol);
                }

                //DRG세팅
                if (Convert.ToDateTime(strBDate) >= Convert.ToDateTime("2015-11-06") && (strBi == "11" || strBi == "12" || strBi == "13") && strDeptCode == "OT")
                {
                    DRG_Code_Set_OCS(pDbCon, strDeptCode, strBDate, strPtNo, intIpdNo, intTRSNO, intAge, ssSpread, intMRow, intSugaCol);
                }

                //2016-01-27
                if (strOK == "OK")
                {
                    strOK = CHK_MNHD_SUGA_ORDER_CHK("입원", strPtNo, strDeptCode, ssSpread, intMRow, intSugaCol, intSelfCol);
                }

                //2019-02-01
                if (strOK == "OK" && string.Compare(strBDate, "2019-02-01") >= 0)
                {
                    strOK = CHK_LOWER_SONO_ORDER_CHK(pDbCon, ssSpread, intMRow, intSugaCol);
                }
                #endregion //입원
            }
            else if (strGBN == "응급")
            {
                #region //응급
                if (strOK == "OK" && clsPublic.GstrPRNChk == "OK")
                {
                    strOK = CHK_PRN_ORDER_CHK(pDbCon, clsPublic.GstrJobMan, strPtNo, ssSpread, strBDate, 13, 11, 65, intMRow, intDosCol, intEDateCol, intPRN_MaxCol, intPRN_NotifyCol);
                }

                if (strOK == "OK" && clsPublic.GstrASA진정Chk == "OK")
                {
                    clsPublic.GnASA진정Row = 0;

                    if (strOK == "OK")
                    {
                        strOK = CHK_ASA_ORDER_CHK(pDbCon, "응급", strPtNo, strDeptCode, ssSpread, intMRow, intSugaCol);

                        if (clsPublic.GnASA진정Row > 0 && strOK != "OK")
                        {
                            frm.Show();
                            frm.FormClosed += Frm2_FormClosed;

                            if (clsPublic.Gstr마취신체등급.Length == 1)
                            {
                                ssSpread.ActiveSheet.Cells[clsPublic.GnASA진정Row, 73].Text = clsPublic.Gstr마취신체등급;       //입원
                            }

                            clsPublic.Gstr마취신체등급 = "";
                        }
                    }
                }

                //2015-10-15
                if (strOK == "OK" && clsPublic.GstrNotDiv == "OK")
                {
                    strOK = CHK_NotDIV_ORDER_CHK(pDbCon, "응급", strPtNo, strSugaT, strBun, strDeptCode, ssSpread, intMRow, intSelfCol, intBunCol, intSugaCol, intQtyCol, intDivCol);
                }

                //2015-11-07
                if (strOK == "OK" && clsPublic.Gstr혈액사용예정일Chk == "OK")
                {
                    strOK = CHK_BLOOD_RDate_ORDER_CHK("응급", strPtNo, strDeptCode, ssSpread, intMRow, intSugaCol, intBunCol, intRemarkCol);

                    if (clsPublic.Gn혈액사용예정일Row > 0 && strOK != "OK")
                    {
                        clsPublic.Gstr혈액사용예정일Date = "";
                        frm2.Show();
                        frm2.FormClosed += Frm2_FormClosed;

                        ssSpread.ActiveSheet.Cells[clsPublic.Gn혈액사용예정일Row, intRemarkCol].Text = ssSpread.ActiveSheet.Cells[clsPublic.Gn혈액사용예정일Row, intRemarkCol].Text + "[혈액사용예정일:" + clsPublic.Gstr혈액사용예정일Date + "]";
                        ssSpread.ActiveSheet.Cells[clsPublic.Gn혈액사용예정일Row, intR].Text = "#";
                    }
                }

                //2019-02-01
                if (strOK == "OK" && string.Compare(strBDate, "2019-02-01") >= 0)
                {
                    strOK = CHK_LOWER_SONO_ORDER_CHK(pDbCon, ssSpread, intMRow, intSugaCol);
                }
                #endregion //응급
            }

            rtnVal = strOK;
            return rtnVal;
        }

  

        private static void Frm2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form frm = (sender as Form);
            if (frm.IsDisposed == false)
            {
                frm.Dispose();
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_ills_Code
        /// 상병 체크
        /// </summary>
        /// <returns></returns>
        public static string CHK_ills_Code(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread ssSpread, string strBDate, string strOutDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int i = 0;

            string strCode = "";

            string rtnVal = "OK";

            try
            {                
                for (i = 0; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (clsOrdFunction.GEnvSet_Item21 != null && clsOrdFunction.GEnvSet_Item21 == "2")
                    {
                        strCode = ssSpread.ActiveSheet.Cells[i, 2].Text.Trim();
                    }
                    else
                    {
                        strCode = ssSpread.ActiveSheet.Cells[i, 0].Text.Trim();
                    }

                    SQL = "";
                    SQL = "SELECT ROWID,TO_CHAR(DDATE,'YYYY-MM-DD') DDATE FROM KOSMOS_PMPA.BAS_ILLS";
                    SQL = SQL + ComNum.VBLF + "  WHERE ILLCODE ='" + strCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND IllClass ='1' ";

                    if (Convert.ToDateTime(strBDate) < Convert.ToDateTime("2016-01-01"))
                    {
                        SQL = SQL + ComNum.VBLF + "  AND  ( KCDOLD ='*' OR KCD6  ='*' ) ";
                    }

                    if (Convert.ToDateTime(strBDate) >= Convert.ToDateTime("2016-01-01"))
                    {
                        SQL = SQL + ComNum.VBLF + " AND  ( KCDOLD ='*' OR KCD6  ='*' OR  KCD7 ='*') ";
                    }

                    //2020-12-31 안정수 추가 
                    if (Convert.ToDateTime(strBDate) >= Convert.ToDateTime("2021-01-01"))
                    {
                        SQL = SQL + ComNum.VBLF + " AND  ( KCDOLD ='*' OR KCD6  ='*' OR  KCD7 ='*' OR  KCD8 ='*') ";
                    }

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (reader.HasRows == false)
                    {
                        rtnVal = "NO";
                        ComFunc.MsgBox(strCode + " 이 상병코드는 사용할수 없는 상병입니다..", "상병확인");

                        reader.Dispose();
                        reader = null;
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (reader.Read())
                    {
                        string DDATE = reader.GetValue(0).ToString().Trim();
                        if (DDATE.NotEmpty())
                        {
                            if (Convert.ToDateTime(strBDate) > Convert.ToDateTime(DDATE))
                            {
                                rtnVal = "NO";

                                ComFunc.MsgBox(strCode + " 이 상병코드는 " + DDATE + " 삭제상병 입니다...", "상병확인");

                                reader.Dispose();
                                reader = null;

                                return rtnVal;
                            }
                        }
                    }

                    reader.Dispose();
                    reader = null;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_JinDan_SONO_CHK
        /// </summary>
        /// <returns></returns>
        public static string READ_JinDan_SONO_CHK(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread ssSpread, FarPoint.Win.Spread.FpSpread ssSpread2)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int i = 0;

            string rtnVal = "";

            string strOK2 = "";
            string strOK3 = "";

            string strTemp = "";
            string strCode = "";

            rtnVal = "OK";

            try
            {
                for (i = 0; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        //급여구분
                        //if (ssSpread.ActiveSheet.Cells[i, 8].Text.Trim() == "" || ssSpread.ActiveSheet.Cells[i, 8].Text.Trim() == "0")
                        //2021-01-12 변경
                        if (ssSpread.ActiveSheet.Cells[i, 9].Text.Trim() == "" || ssSpread.ActiveSheet.Cells[i, 9].Text.Trim() == "0")
                        {
                            //진단 초음파 급여 체크 2015-08-31
                            SQL = "";
                            SQL = "SELECT ROWID ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF ";
                            SQL = SQL + ComNum.VBLF + "  WHERE GUBUNA='J' ";
                            SQL = SQL + ComNum.VBLF + "    AND GUBUNB ='1' ";
                            //SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + ssSpread.ActiveSheet.Cells[i, 12].Text.Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "    AND SUCODE ='" + ssSpread.ActiveSheet.Cells[i, 17].Text.Trim() + "' ";

                            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("함수명 : " + "READ_JinDan_SONO_CHK" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog("함수명 : " + "READ_JinDan_SONO_CHK" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            if (reader.HasRows)
                            {
                                strOK2 = "OK";
                                rtnVal = "NO";

                                reader.Dispose();
                                reader = null;
                                Cursor.Current = Cursors.Default;

                                break;
                            }

                            reader.Dispose();
                            reader = null;
                        }
                    }
                }

                //상병체크 
                strOK3 = "";
                if (strOK2 == "OK")
                {
                    for (i = 0; i < ssSpread2.ActiveSheet.NonEmptyRowCount; i++)
                    {
                        SQL = "";
                        SQL = "SELECT ROWID ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_ills_h ";

                        if (clsOrdFunction.GEnvSet_Item21 != null && clsOrdFunction.GEnvSet_Item21 == "2")
                        {
                            SQL = SQL + ComNum.VBLF + "  WHERE illcode = '" + ssSpread2.ActiveSheet.Cells[i, 2].Text.Trim() + "' ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "  WHERE illcode = '" + ssSpread2.ActiveSheet.Cells[i, 0].Text.Trim() + "' ";
                        }
                            
                        SQL = SQL + ComNum.VBLF + "   AND Gubun IN ('3','4') ";
                        SQL = SQL + ComNum.VBLF + "   AND illcode NOT IN ('D66','D67') ";
                        SQL = SQL + ComNum.VBLF + "   AND illcode NOT IN ( select illcode from KOSMOS_PMPA.BAS_ills_h where illcode like 'F2%' )";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("함수명 : " + "READ_JinDan_SONO_CHK" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog("함수명 : " + "READ_JinDan_SONO_CHK" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        if (reader.HasRows)
                        {
                            strOK3 = "OK";

                            reader.Dispose();
                            reader = null;
                            Cursor.Current = Cursors.Default;
                            break;
                        }

                        reader.Dispose();
                        reader = null;
                        
                        if (clsOrdFunction.GEnvSet_Item21 != null && clsOrdFunction.GEnvSet_Item21 == "2")
                        {
                            strCode = VB.Left(ssSpread2.ActiveSheet.Cells[i, 2].Text.Trim(), 3);
                        }
                        else
                        {
                            strCode = VB.Left(ssSpread2.ActiveSheet.Cells[i, 0].Text.Trim(), 3);
                        }
                        
                        //3자리체크                        
                        switch(strCode)
                        {
                            case "C00":
                            case "C01":
                            case "C02":
                            case "C03":
                            case "C04":
                            case "C05":
                            case "C06":
                            case "C07":
                            case "C08":
                            case "C09":
                            case "C10":
                            case "C11":
                            case "C12":
                            case "C13":
                            case "C14":
                            case "C15":
                            case "C16":
                            case "C17":
                            case "C18":
                            case "C19":
                            case "C20":
                            case "C21":
                            case "C22":
                            case "C23":
                            case "C24":
                            case "C25":
                            case "C26":
                            case "C27":
                            case "C28":
                            case "C29":
                            case "C30":
                            case "C31":
                            case "C32":
                            case "C33":
                            case "C34":
                            case "C35":
                            case "C36":
                            case "C37":
                            case "C38":
                            case "C39":
                            case "C40":
                            case "C41":
                            case "C42":
                            case "C43":
                            case "C44":
                            case "C45":
                            case "C46":
                            case "C47":
                            case "C48":
                            case "C49":
                            case "C50":
                            case "C51":
                            case "C52":
                            case "C53":
                            case "C54":
                            case "C55":
                            case "C56":
                            case "C57":
                            case "C58":
                            case "C59":
                            case "C60":
                            case "C61":
                            case "C62":
                            case "C63":
                            case "C64":
                            case "C65":
                            case "C66":
                            case "C67":
                            case "C68":
                            case "C69":
                            case "C70":
                            case "C71":
                            case "C72":
                            case "C73":
                            case "C74":
                            case "C75":
                            case "C76":
                            case "C77":
                            case "C78":
                            case "C79":
                            case "C80":
                            case "C81":
                            case "C82":
                            case "C83":
                            case "C84":
                            case "C85":
                            case "C86":
                            case "C87":
                            case "C88":
                            case "C89":
                            case "C90":
                            case "C91":
                            case "C92":
                            case "C93":
                            case "C94":
                            case "C95":
                            case "C96":
                            case "C97":
                            case "D00":
                            case "D01":
                            case "D02":
                            case "D03":
                            case "D04":
                            case "D05":
                            case "D06":
                            case "D07":
                            case "D08":
                            case "D09":
                            case "D32":
                            case "D33":
                            case "D37":
                            case "D38":
                            case "D39":
                            case "D40":
                            case "D41":
                            case "D42":
                            case "D43":
                            case "D44":
                            case "D45":
                            case "D46":
                            case "D47":
                            case "D48":
                            case "I60":
                            case "I61":
                            case "I62":
                            case "I63":
                            case "I64":
                            case "I65":
                            case "I66":
                            case "I67":
                            case "I05":
                            case "I06":
                            case "I07":
                            case "I08":
                            case "I09":
                            case "I10":
                            case "I11":
                            case "I12":
                            case "I13":
                            case "I14":
                            case "I15":
                            case "I16":
                            case "I17":
                            case "I18":
                            case "I19":
                            case "I20":
                            case "I21":
                            case "I22":
                            case "I23":
                            case "I24":
                            case "I25":
                            case "I30":
                            case "I31":
                            case "I32":
                            case "I33":
                            case "I34":
                            case "I35":
                            case "I36":
                            case "I37":
                            case "I38":
                            case "I39":
                            case "I40":
                            case "I41":
                            case "I42":
                            case "I43":
                            case "I44":
                            case "I45":
                            case "I46":
                            case "I47":
                            case "I48":
                            case "I49":
                            case "I50":
                            case "I51":
                            case "Q20":
                            case "Q21":
                            case "Q22":
                            case "Q23":
                            case "Q24":
                            case "Q25":
                            case "S25":
                            case "S26":
                                strOK3 = "OK";
                                break;
                        }

                        if (clsOrdFunction.GEnvSet_Item21 != null && clsOrdFunction.GEnvSet_Item21 == "2")
                        {
                            strCode = VB.Left(ssSpread2.ActiveSheet.Cells[i, 2].Text.Trim(), 4);
                        }
                        else
                        {
                            strCode = VB.Left(ssSpread2.ActiveSheet.Cells[i, 0].Text.Trim(), 4);
                        }
                        //4자리체크
                        switch (strCode)
                        {
                            case "D151":
                            case "I700":
                            case "I720":
                            case "I770":
                            case "I790":
                            case "I791":
                            case "M314":
                            case "Q260":
                            case "Q261":
                            case "Q263":
                            case "Q264":
                            case "Q268":
                            case "Q269":
                            case "Q280":
                            case "Q281":
                            case "Q282":
                            case "Q283":
                                strOK3 = "OK";
                                break;
                        }
                    }

                    strTemp = "1.희귀난치질환 상병 전체";
                    strTemp = strTemp + ComNum.VBLF + "2. 기타상병 아래참조 ";
                    strTemp = strTemp + ComNum.VBLF + " C00-C97 , D00-D09 , D32-D33 , D37-D48, I60-I67  ";
                    strTemp = strTemp + ComNum.VBLF + " D151, I700, I720, I770, I790  ";
                    strTemp = strTemp + ComNum.VBLF + " M314, Q260, I261, Q263, Q264, Q268, Q269   ";
                    strTemp = strTemp + ComNum.VBLF + " Q280-Q283   ";
                    strTemp = strTemp + ComNum.VBLF + " S06, I01,I05-I09, I20-I25   ";
                    strTemp = strTemp + ComNum.VBLF + " I26, I28 ,I30-I51, I71    ";
                    strTemp = strTemp + ComNum.VBLF + " Q20-Q25, S25-S26    ";
                    strTemp = strTemp + ComNum.VBLF + " 1번 혹은 2번의 상병이 있어야 초음파 급여도 전송가능합니다..";

                    if (strOK3 == "OK")
                    {
                        rtnVal = "OK";
                    }
                    else
                    {
                        ComFunc.MsgBox("진단 초음파 급여코드 사용시 상병코드 필요합니다..!! 아래참조" + ComNum.VBLF + ComNum.VBLF + strTemp, "상병참고");
                    }
                }
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("함수명 : " + "READ_JinDan_SONO_CHK" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "READ_JinDan_SONO_CHK" + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_OPD_보호자내원수가_CHK
        /// </summary>
        /// <returns></returns>
        public static string READ_OPD_BOHOJA_SUGA_CHK(FarPoint.Win.Spread.FpSpread ssSpread)
        {
            int i = 0;

            string rtnVal = "OK";

            string strOK2 = "";
            string strOK3 = "";

            for (i = 0; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    //수가
                    if (ssSpread.ActiveSheet.Cells[i, 12].Text.Trim() == "$$42")
                    {
                        strOK2 = "OK";
                        break;
                    }
                }
            }

            if (strOK2 == "OK")
            {
                for (i = 0; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        //수가
                        if (ssSpread.ActiveSheet.Cells[i, 12].Text.Trim() == "##19")
                        {
                            strOK3 = "OK";
                            break;
                        }
                    }
                }
            }

            if (strOK3 == "OK")
            {
                ComFunc.MsgBox("보호자내원 코드  [ $$42 ]와  [ ##19 ] 수가는 사용할수 없습니다...", "확인");
                rtnVal = "";
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_ORDER_알러지수가_CHK
        /// </summary>
        /// <returns></returns>
        public static string READ_ORDER_ALLERGY_SUGA_CHK(PsmhDb pDbCon, string strIO, string strPtNo, FarPoint.Win.Spread.FpSpread ssSpread, int intMRow, int intSugaCol)
        {
            int i = 0;
            int intStart = 0;
            string rtnVal = "OK";

            string strTemp = "";

            if (intMRow == 0)
            {
                intStart = 1;
            }
            else
            {
                intStart = intMRow + 1;
            }

            for (i = 0; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (ssSpread.ActiveSheet.Cells[i, 0].Text.Trim() != "True")
                {
                    //수가
                    if (ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim() != "")
                    {
                        strTemp = Read_ALLERGY_MEDICINE_CHK(pDbCon, strPtNo, ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim());

                        if (strTemp != "OK")
                        {
                            if (ComFunc.MsgBoxQ("★★ 알러지등록 정보 ★★ "
                                + ComNum.VBLF + ComNum.VBLF + "--------------------------------------------------------------------------------"
                                + ComNum.VBLF + "알러지 입력내용 : " + strTemp
                                + ComNum.VBLF + "--------------------------------------------------------------------------------"
                                + ComNum.VBLF + ComNum.VBLF + "이대로 처방을 전송하시겠습니까??",
                                "★알러지정보확인★", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                            {
                                rtnVal = "";
                                return rtnVal;
                            }
                        }
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_JinDan_SONO_CHK2
        /// </summary>
        /// <returns></returns>
        public static string READ_JinDan_SONO_CHK2(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                //진단 초음파 급여 체크 2015-08-31
                SQL = "";
                SQL = "SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUNA='J' ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUNB ='1' ";
                SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSuCode + "' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("함수명 : " + "READ_JinDan_SONO_CHK2" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "READ_JinDan_SONO_CHK2" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("함수명 : " + "READ_JinDan_SONO_CHK2" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "READ_JinDan_SONO_CHK2" + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_VaccFlu_ORDER_CHK
        /// </summary>
        /// <returns></returns>
        public static string READ_VaccFlu_ORDER_CHK(PsmhDb pDbCon, string strIO, FarPoint.Win.Spread.FpSpread ssSpread, int intSuCol, string strJumin, string strBDate, string strBirth)
        {
            string rtnVal = "OK";
            string strSuCode = "";
            int i = 0;

            int intAge = 0;

            try
            {
                if (strIO == "외래")
                {
                    intAge = ComFunc.AgeCalcEx(strJumin, strBDate);

                    for (i = 0; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
                    {
                        if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                        {
                            strSuCode = ssSpread.ActiveSheet.Cells[i, intSuCol].Text.Trim();

                            //영유가 독감
                            //2016-10-04시행 체크사항
                            if (strSuCode == "BOFLU3-0")
                            {
                                if (string.Compare(strBDate, "2016-10-04") >= 0)
                                {
                                    if (ComFunc.MsgBoxQ(strSuCode + " 영유아독감(BOFLU3-0)수가는 "
                                        + ComNum.VBLF + "생후 6~12개월 미만 영아(2015.10.01~2016.06.30 출생아"
                                        + ComNum.VBLF + "주민번호없는 내국인 지원불가"
                                        + ComNum.VBLF + "단, 반드시 생후 6개월 경과하여야함(그 달 생일 일자가 지나야함)"
                                        + ComNum.VBLF + "2016년 4월생은 6개월이 경과한 10월부터,"
                                        + ComNum.VBLF + " 5월생은 11월부터, 6월생은 12월 부터 가능!!"
                                        + ComNum.VBLF + "이대로 전송하시겠습니까??",
                                        "참고사항 - 생년월일 체크", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                                    {
                                        rtnVal = "";
                                        return rtnVal;
                                    }
                                }
                                else
                                {
                                    rtnVal = "";
                                    ComFunc.MsgBox(strSuCode + " 영유아독감 수가는" + ComNum.VBLF + " 2016-10-04일 부터 사용가능합니다.", "수가확인");
                                    return rtnVal;
                                }
                            }
                            //노인독감
                            else if (strSuCode == "FLUTF-0")
                            {
                                if (string.Compare(strBDate, "2017-09-26") >= 0)
                                {
                                    //if (string.Compare(strBirth, "1941-12-31") >= 0)                                    
                                    //{
                                    //    if (string.Compare(strBirth, "1951-12-31") < 0)
                                    //    {
                                    //        if (string.Compare(strBDate, "2017-09-26") < 0)
                                    //        {
                                    //            if (ComFunc.MsgBoxQ(strSuCode + " 접종수가는 65세이상 대상은 2016-10-10부터 가능합니다..."
                                    //                + ComNum.VBLF + "단, 접종예외지역(포항,영덕,울진,울릉,영천 등...) "
                                    //                + ComNum.VBLF + "등록된 65세이상도 가능합니다.." + ComNum.VBLF + "이대로 전송하시겠습니까??",
                                    //                "접종수가 및 생년월일 체크", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                                    //            {
                                    //                rtnVal = "";
                                    //                return rtnVal;
                                    //            }
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        rtnVal = "";
                                    //        ComFunc.MsgBox(strSuCode + "접종수가는 65세이상 대상만 사용가능한 수가입니다...", "수가확인");
                                    //        return rtnVal;
                                    //    }
                                    //}

                                    // 2019-10-15 수정
                                    if (intAge < 65)
                                    {
                                        rtnVal = "";
                                        ComFunc.MsgBox(strSuCode + "접종수가는 65세이상 대상만 사용가능한 수가입니다...", "수가확인");
                                        return rtnVal;
                                    }                                    
                                }
                                else
                                {
                                    rtnVal = "";
                                    ComFunc.MsgBox(strSuCode + " 2017-10-12 부터 사용 가능한 수가입니다...", "수가확인");
                                    return rtnVal;
                                }
                            }
                            else if (strSuCode == "SKFLU-A")
                            {
                                if (string.Compare(strBDate,"2015-10-01") >= 0 && string.Compare(strBDate ,"2015-12-31") <= 0)
                                {
                                }
                                else
                                {
                                    rtnVal = "";
                                    ComFunc.MsgBox(strSuCode + " 2015-10-12 ~ 2015-12-31 까지 사용 가능한 수가입니다...", "수가확인");
                                    return rtnVal;
                                }
                            }
                            else if (strSuCode == "SKFLU-P")
                            {
                                if (string.Compare(strBDate, "2015-10-01") >= 0 && string.Compare(strBDate, "2015-12-31") <= 0)
                                {
                                }
                                else
                                {
                                    rtnVal = "";
                                    ComFunc.MsgBox(strSuCode + " 2015-10-12 ~ 2015-12-31 까지 사용 가능한 수가입니다...", "수가확인");
                                    return rtnVal;
                                }
                            }
                        }
                    }
                }
                else if (strIO == "입원")
                {
                    for (i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
                    {
                        if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                        {
                            strSuCode = ssSpread.ActiveSheet.Cells[i, intSuCol].Text.Trim();

                            switch (strSuCode)
                            {
                                case "GCFLU-0":
                                    ComFunc.MsgBox(strSuCode + " 병동에서는 사용 할 수 없는  수가입니다...", "수가확인");
                                    rtnVal = "";
                                    return rtnVal;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("함수명 : " + "READ_VaccFlu_ORDER_CHK" + ComNum.VBLF + ex.Message, "", clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "READ_VaccFlu_ORDER_CHK" + ComNum.VBLF + ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_CHK_DRUG_CNTCHK
        /// 2014-04-01
        /// </summary>
        /// <returns></returns>
        public static string READ_CHK_DRUG_CNTCHK(PsmhDb pDbCon, string strPtNo, string strInDate, string strBDate, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            string rtnVal = "OK";

            try
            {
                //2014-11-03
                SQL = "";
                SQL = "SELECT BDate,SUM(Nal) Cnt ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER ";
                SQL = SQL + ComNum.VBLF + "  WHERE PTNO ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE >=TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE <=TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSuCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND (GbPrn =' '  OR GbPrn <> 'P' ) ";     //2014-05-12
                SQL = SQL + ComNum.VBLF + "   AND OrderSite NOT IN ('NDC') ";       //2014-12-02
                SQL = SQL + ComNum.VBLF + "  GROUP BY BDate ";
                SQL = SQL + ComNum.VBLF + "   HAVING SUM(Nal) <> 0 ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 6)
                {
                    rtnVal = strSuCode + " 수가5일 제한";
                    ComFunc.MsgBox(strSuCode + " 수가는 재원중 6일이상 사용할수 없습니다.." + ComNum.VBLF + "반환처리후 전송하십시오!!", "확인");
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수 : READ_CHK_DRUG_CNTCHK " + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_CHK_당뇨상병2_수가
        /// </summary>
        /// <returns></returns>
        public static void READ_CHK_DIABETES_ILL2_SUGA(PsmhDb pDbCon, string strJob, string strPtNo, string strBDate, FarPoint.Win.Spread.FpSpread ssSpread)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int i = 0;

            string strChk = "";

            try
            {
                //기존등록된 자료 체크
                SQL = "";
                SQL = "SELECT * FROM KOSMOS_PMPA.ETC_SMS";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN ='51'";
                SQL = SQL + ComNum.VBLF + "     AND PANO ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "     AND (SENDTIME IS NULL OR SENDTIME ='')";
                SQL = SQL + ComNum.VBLF + "     AND  TRUNC(JOBDATE) >=TO_DATE('" + strBDate + "','YYYY-MM-DD')";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (reader.HasRows)
                {
                    reader.Dispose();
                    reader = null;
                    Cursor.Current = Cursors.Default;

                    return;
                }

                reader.Dispose();
                reader = null;

                if (strJob == "Y")
                {
                    for (i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
                    {
                        if (ssSpread.ActiveSheet.Cells[i, 0].Text.Trim() != "True")
                        {
                            switch (ssSpread.ActiveSheet.Cells[i, 1].Text.Trim())
                            {
                                case "BC6":
                                case "B2561":
                                case "B2903":
                                case "B3120":
                                case "A33":
                                    ComFunc.MsgBox("적정성평가(당뇨) 대상자 입니다.. 참고하십시오!!" + ComNum.VBLF + "상병+수가점검", "확인");
                                    return;
                            }
                        }
                    }
                }
                else
                {
                    //검사수가발생시
                    for (i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
                    {
                        if (ssSpread.ActiveSheet.Cells[i, 0].Text.Trim() != "True")
                        {
                            switch (ssSpread.ActiveSheet.Cells[i, 1].Text.Trim())
                            {
                                case "BC6":
                                case "B2561":
                                case "B2903":
                                case "B3120":
                                case "A33":
                                    strChk = "OK";
                                    break;
                            }

                            if (strChk == "OK")
                            {
                                break;
                            }
                        }
                    }

                    if (strChk == "OK")
                    {
                        for (i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
                        {
                            if (ssSpread.ActiveSheet.Cells[i, 0].Text.Trim() != "True")
                            {
                                switch (ssSpread.ActiveSheet.Cells[i, 1].Text.Trim())
                                {
                                    case "GLIM1":
                                    case "GLI2":
                                    case "GLI4":
                                    case "MFO5":
                                    case "MFO10":
                                    case "ZANUVI":
                                    case "GAVUS":
                                    case "ONGLY2.5":
                                    case "ONGLY5":
                                    case "H-LEVEM":
                                    case "H-LAN":
                                    case "H-MIX":
                                    case "H-HUMIX":
                                        ComFunc.MsgBox("적정성평가(당뇨) 대상자 입니다.. 참고하십시오!!" + ComNum.VBLF + "상병+수가점검", "확인");
                                        return;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_CHK_예방접종_수가
        /// </summary>
        /// <returns></returns>
        public static void READ_CHK_VACCINATION_SUGA(PsmhDb pDbCon, string strPtNo, string strBDate, FarPoint.Win.Spread.FpSpread ssSpread)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int i = 0;

            try
            {
                for (i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text.Trim() != "True")
                    {
                        //주사만
                        if (ssSpread.ActiveSheet.Cells[i, 13].Text.Trim() == "20")
                        {
                            //6개월전 같은 예방접종 코드 체크
                            SQL = "";
                            SQL = "SELECT  a.Ptno  ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_SUN b ";
                            SQL = SQL + ComNum.VBLF + " WHERE  TRIM(a.SUCODE) =TRIM(b.SUNEXT) ";
                            SQL = SQL + ComNum.VBLF + "  AND a.PTNO ='" + strPtNo + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND b.GBYEBANG ='Y' ";
                            SQL = SQL + ComNum.VBLF + "  AND a.SUCODE ='" + ssSpread.ActiveSheet.Cells[i, 13].Text.Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND a.BDATE>=TO_DATE('" + Convert.ToDateTime(strBDate).AddMonths(-6).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "  AND a.BDATE<TO_DATE('" + strBDate + "','YYYY-MM-DD') ";

                            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            if (reader.HasRows)
                            {
                                ComFunc.MsgBox(ssSpread.ActiveSheet.Cells[i, 13].Text.Trim() + " 예방접종 수가는 6개월전 이미 발생했습니다.." + ComNum.VBLF + "처방시 확인 하세요!!", "확인");

                                reader.Dispose();
                                reader = null;
                                Cursor.Current = Cursors.Default;

                                break;
                            }

                            reader.Dispose();
                            reader = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_CHK_안저상병2_수가
        /// </summary>
        /// <returns></returns>
        public static void READ_CHK_EYEGROUND_ILL2_SUGA(PsmhDb pDbCon, string strJob, string strPtNo, string strBDate, FarPoint.Win.Spread.FpSpread ssSpread)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int i = 0;

            try
            {
                SQL = "";
                SQL = "SELECT 1 FROM KOSMOS_PMPA.ETC_SMS";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN ='52'";
                SQL = SQL + ComNum.VBLF + "     AND PANO ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "     AND (SENDTIME IS NULL OR SENDTIME ='')";
                SQL = SQL + ComNum.VBLF + "     AND  TRUNC(JOBDATE) >=TO_DATE('" + strBDate + "','YYYY-MM-DD')";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (reader.HasRows)
                {
                    reader.Dispose();
                    reader = null;
                    Cursor.Current = Cursors.Default;

                    return;
                }

                reader.Dispose();
                reader = null;

                if (strJob == "Y")
                {
                    for (i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
                    {
                        if (ssSpread.ActiveSheet.Cells[i, 0].Text.Trim() != "True")
                        {
                            switch (ssSpread.ActiveSheet.Cells[i, 1].Text.Trim())
                            {
                                case "OT0010":
                                    ComFunc.MsgBox("적정성평가(안저) 대상자 입니다.. 참고하십시오!!" + ComNum.VBLF + "상병+수가점검", "확인");
                                    return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : ALL_OCS_SUGA_MESSAGE
        /// 2013-04-24 약제과 통합수가 메시지
        /// </summary>
        /// <returns></returns>
        public static void ALL_OCS_SUGA_MESSAGE(PsmhDb pDbCon, string strGBN, string strSuCode, FarPoint.Win.Spread.FpSpread ssSpread, int intCol, int intROW, string strText)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            int i = 0;
            int intCount = 0;

            string[] strSeqNo = null;     //시퀀스
            string[] strTitle = null;     //타이틀
            string[] strMessage = null;     //메시지
            string[] strExPress = null;     //특수문자
            string[] strBColor = null;     //배경색
            string[] strFColor = null;     //글자색
            string[] strBold = null;     //굵기

            string strMsg = "";

            try
            {
                SQL = "";
                SQL = "SELECT a.SEQNO,a.TITLE,a.MESSAGE,a.EXPRESSION,a.BACKCOLOR,a.FORECOLOR,a.BOLD ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_SPECIAL a, KOSMOS_ADM.DRUG_SPECIAL_JEPCODE b  ";
                SQL = SQL + ComNum.VBLF + "  WHERE a.SEQNO=b.SEQNO ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(b.JEPCODE) ='" + strSuCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ( a.DelDate IS NULL OR a.DelDate ='' ) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY a.SEQNO ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    intCount = dt.Rows.Count;

                    strSeqNo = new string[intCount];
                    strTitle = new string[intCount];
                    strMessage = new string[intCount];
                    strExPress = new string[intCount];
                    strBColor = new string[intCount];
                    strFColor = new string[intCount];
                    strBold = new string[intCount];

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSeqNo[i] = dt.Rows[i]["SEQNO"].ToString().Trim();
                        strTitle[i] = dt.Rows[i]["TITLE"].ToString().Trim();
                        strMessage[i] = dt.Rows[i]["MESSAGE"].ToString().Trim();
                        strExPress[i] = dt.Rows[i]["EXPRESSION"].ToString().Trim();
                        strBColor[i] = dt.Rows[i]["BACKCOLOR"].ToString().Trim();
                        strFColor[i] = dt.Rows[i]["FORECOLOR"].ToString().Trim();
                        strBold[i] = dt.Rows[i]["BOLD"].ToString().Trim();
                    }

                    if (strSeqNo[0] != "7" && strSeqNo[0] != "11")
                    {
                        dt.Dispose();
                        dt = null;
                        return;
                    }

                    if (intCount == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return;
                    }

                    //최대 3개 표현
                    for (i = 0; i < intCount; i++)
                    {
                        if (strExPress[i] != "" && strTitle[i] != "")
                        {
                            strMsg = strMsg + strExPress[i] + strTitle[i] + " ";
                        }
                        else if (strExPress[i] != "")
                        {
                            strMsg = strMsg + strExPress[i] + " ";
                        }
                        else if (strTitle[i] != "")
                        {
                            strMsg = strMsg + strTitle[i] + " ";
                        }
                    }

                    //특수문자 및 타이틀
                    if (strMsg != "")
                    {
                        ssSpread.ActiveSheet.Cells[intROW, intCol].Text = strMsg + ssSpread.ActiveSheet.Cells[intROW, intCol].Text + strText;
                    }
                    else
                    {
                        ssSpread.ActiveSheet.Cells[intROW, intCol].Text = ssSpread.ActiveSheet.Cells[intROW, intCol].Text + strText;
                    }

                    //첫번째 조건 기준
                    if (strBColor[0] != "")
                    {
                        ssSpread.ActiveSheet.Cells[intROW, intCol].BackColor = System.Drawing.ColorTranslator.FromWin32(Convert.ToInt32(strBColor[0]));
                    }

                    if (strFColor[0] != "")
                    {
                        ssSpread.ActiveSheet.Cells[intROW, intCol].ForeColor = System.Drawing.ColorTranslator.FromWin32(Convert.ToInt32(strFColor[0]));
                    }

                    if (strBold[0] == "Y")
                    {
                        ssSpread.ActiveSheet.Cells[intROW, intCol].Font = new System.Drawing.Font(ssSpread.ActiveSheet.Cells[intROW, intCol].Font.FontFamily, ssSpread.ActiveSheet.Cells[intROW, intCol].Font.Size, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129))); ;
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : ALL_OCS_SUGA_MESSAGE_POP
        /// 2013-04-24 약제과 통합수가 메시지 팝업
        /// </summary>
        /// <returns></returns>
        public static void ALL_OCS_SUGA_MESSAGE_POP(PsmhDb pDbCon, string strGBN, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            int i = 0;

            int intCount = 0;

            string[] strSeqNo = null;       //시퀀스
            string[] strTitle = null;       //타이틀
            string[] strMessage = null;       //메시지
            string[] strExPress = null;       //특수문자
            string[] strBColor = null;       //배경색
            string[] strFColor = null;       //글자색
            string[] strBold = null;       //굵기
            string[] strSuName = null;

            string strMsg = "";

            try
            {
                SQL = "";
                SQL = "SELECT a.SEQNO,a.TITLE,a.MESSAGE,a.EXPRESSION,a.BACKCOLOR,a.FORECOLOR,a.BOLD,c.SUNAMEK ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_SPECIAL a, KOSMOS_ADM.DRUG_SPECIAL_JEPCODE b, KOSMOS_PMPA.BAS_SUN c ";
                SQL = SQL + ComNum.VBLF + "  WHERE a.SEQNO=b.SEQNO ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(b.JEPCODE)=TRIM(c.SUNEXT(+)) ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(b.JEPCODE) ='" + strSuCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ( a.DelDate IS NULL OR a.DelDate ='' ) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY a.SEQNO ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    intCount = dt.Rows.Count;

                    strSeqNo = new string[intCount];
                    strTitle = new string[intCount];
                    strMessage = new string[intCount];
                    strExPress = new string[intCount];
                    strBColor = new string[intCount];
                    strFColor = new string[intCount];
                    strBold = new string[intCount];
                    strSuName = new string[intCount];

                    for (i = 0; i < intCount; i++)
                    {
                        strSeqNo[i] = dt.Rows[i]["SEQNO"].ToString().Trim();
                        strMessage[i] = dt.Rows[i]["MESSAGE"].ToString().Trim();
                        strExPress[i] = dt.Rows[i]["EXPRESSION"].ToString().Trim();
                        strBColor[i] = dt.Rows[i]["BACKCOLOR"].ToString().Trim();
                        strFColor[i] = dt.Rows[i]["FORECOLOR"].ToString().Trim();
                        strBold[i] = dt.Rows[i]["BOLD"].ToString().Trim();
                        strSuName[i] = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                if (strSeqNo[0] == "0")
                {
                    return;
                }

                if (intCount == 0)
                {
                    return;
                }

                //최대 3개 표현
                for (i = 0; i < intCount; i++)
                {
                    if (strTitle[i] != "")
                    {
                        strMsg = strMsg + strExPress[i] + strTitle[i] + ComNum.VBLF;
                        strMsg = strMsg + strSuCode + " " + strSuName[i] + ComNum.VBLF;
                    }
                }

                //특수문자 및 타이틀
                ComFunc.MsgBox(strMsg, "약제정보");
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Chk_Suga_Drug_No_Use
        /// 수가읽어 입원전용약 제한 수가J항 2
        /// </summary>
        /// <returns></returns>
        public static string Chk_Suga_Drug_No_Use(PsmhDb pDbCon, string strOk, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            rtnVal = strOk;

            try
            {
                SQL = "";
                SQL = "SELECT SUCODE FROM KOSMOS_PMPA.BAS_SUT ";
                SQL = SQL + ComNum.VBLF + " WHERE SUCODE ='" + strSuCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND sugbj ='2' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("함수명 : " + "Chk_Suga_Drug_No_Use" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "Chk_Suga_Drug_No_Use" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = "약-입원전용";
                    ComFunc.MsgBox(strSuCode + " 입원전용 약입니다.. 외래처방불가[전송불가]1!", "수가확인");
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("함수명 : " + "Chk_Suga_Drug_No_Use" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "Chk_Suga_Drug_No_Use" + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Chk_Suga_NP_NAL_CHK
        /// 수가읽어 정신건강의학 요법오더 수량체크
        /// </summary>
        /// <returns></returns>
        public static string Chk_Suga_NP_NAL_CHK(PsmhDb pDbCon, string strOk, string strSuCode, FarPoint.Win.Spread.FpSpread ssSpread, int intRow, int intCol)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int i = 0;
            int nOrderNal = 0;

            string rtnVal = "";

            rtnVal = strOk;


            try
            {
                for (i = 0; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        nOrderNal = (int)VB.Val(ssSpread.ActiveSheet.Cells[i, intCol - 1].Text);
                        //2021-01-12 변경
                        //if (ssSpread.ActiveSheet.Cells[i, 12].Text.Trim() == strSuCode)
                        if (ssSpread.ActiveSheet.Cells[i, 17].Text.Trim() == strSuCode)
                        {
                            SQL = "";
                            SQL = "SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE  ";
                            SQL = SQL + ComNum.VBLF + " WHERE GUBUN ='OCS_NP_요법코드' ";
                            SQL = SQL + ComNum.VBLF + "   AND TRIM(CODE) ='" + strSuCode + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE ='')";

                            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("함수명 : " + "Chk_Suga_NP_NAL_CHK" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog("함수명 : " + "Chk_Suga_NP_NAL_CHK" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                            if (reader.HasRows && nOrderNal > 1)
                            {
                                rtnVal = "정신요법처방 날수점검!!";
                                ComFunc.MsgBox("요법수가 수량확인하십시오!! 외래처방불가[전송불가]", "수가확인");
                            }

                            reader.Dispose();
                            reader = null;
                        }
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("함수명 : " + "Chk_Suga_NP_NAL_CHK" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "Chk_Suga_NP_NAL_CHK" + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_OP_MACH_DRUG_CHK
        /// 마취코드인지 체크 - 수술실 코드기준
        /// </summary>
        /// <returns></returns>
        public static string READ_OP_MACH_DRUG_CHK(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;

            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT Code FROM KOSMOS_PMPA.OPR_CODE ";
                SQL = SQL + ComNum.VBLF + " WHERE Gubun ='C' ";
                SQL = SQL + ComNum.VBLF + " AND TRIM(Code) = '" + strSuCode + "' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : OG_SONO_ORDER_INSERT
        /// </summary>
        /// <returns></returns>
        public static bool OG_SONO_ORDER_INSERT(PsmhDb pDbCon, string strPtNo, string strDept, string strDrCode, string strSName, string strBi,
            string strSex, int intAge, string strSuCode, string strOrderCode, double dblOrderNo, string strOrderName)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            bool rtnVal = false;
            OracleDataReader reader = null;

            //clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL = "SELECT PANO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.XRAY_DETAIL";
                SQL = SQL + ComNum.VBLF + "  WHERE BDATE >= TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "    AND BDATE <= TRUNC(SYSDATE) ";
                //SQL = SQL + ComNum.VBLF + "   AND ORDERNO =" + dblOrderNo + " ";
                //SQL = SQL + ComNum.VBLF + "   AND XCODE ='" + strSuCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(XCODE) ='" + strSuCode.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   AND XJong ='G' ";     //산부인과 초음파
                SQL = SQL + ComNum.VBLF + "   AND DeptCode ='" + strDept + "' ";
                SQL = SQL + ComNum.VBLF + "   AND Pano ='" + strPtNo + "' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    reader = null;

                    //산부인과 초음파인것만
                    SQL = "";
                    SQL = "SELECT SUNEXT FROM KOSMOS_PMPA.BAS_SUN";
                    SQL = SQL + ComNum.VBLF + "     WHERE SUNEXT ='" + strSuCode + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND DTLBUN ='3604'";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (reader.HasRows)
                    {
                        SQL = "";
                        SQL = "INSERT INTO KOSMOS_PMPA.XRAY_DETAIL ";
                        SQL = SQL + ComNum.VBLF + " (ENTERDATE, IPDOPD, GBRESERVED, SEEKDATE, PANO, SNAME, SEX, AGE, DEPTCODE, DRCODE,";
                        SQL = SQL + ComNum.VBLF + "  WARDCODE, ROOMCODE, XJONG, XSUBCODE, XCODE, EXINFO, QTY, EXMORE, EXID, GBEND, MGRNO,";
                        SQL = SQL + ComNum.VBLF + "  GBPORTABLE , REMARK, XRAYROOM, GBNGT, DRREMARK, ORDERNO, ORDERCODE,";
                        SQL = SQL + ComNum.VBLF + "   ORDERDATE, SENDDATE , XSENDDATE, BI, BDATE,GbSPC,GbSTS ) ";
                        SQL = SQL + ComNum.VBLF + " VALUES ";
                        SQL = SQL + ComNum.VBLF + " (SYSDATE, 'O', '1', SYSDATE, '" + strPtNo + "', '" + strSName + "',        ";
                        SQL = SQL + ComNum.VBLF + "  '" + strSex + "', " + intAge + ", '" + strDept + "', '" + strDrCode + "', ";
                        SQL = SQL + ComNum.VBLF + "  '', '', 'G', '00', '" + strSuCode + "', 1, 1, ";
                        SQL = SQL + ComNum.VBLF + "  '', 0,  '', '', '', '" + strOrderName + "', '', '',  ";
                        SQL = SQL + ComNum.VBLF + "  'auto_send', " + dblOrderNo + ",'" + strOrderCode + "', ";
                        SQL = SQL + ComNum.VBLF + " SYSDATE, SYSDATE,'', '" + strBi + "' , TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ,'0','0'  )            ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                reader.Dispose();
                reader = null;

                //clsDB.setCommitTran(pDbCon);

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : MR_SONO_ORDER_INSERT
        /// </summary>
        /// <returns></returns>
        public static bool MR_SONO_ORDER_INSERT(PsmhDb pDbCon, string strPtNo, string strDept, string strDrCode, string strSName, string strBi,
            string strSex, int intAge, string strSuCode, string strOrderCode, double dblOrderNo, string strOrderName)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            OracleDataReader reader = null;
            bool rtnVal = false;
            
            //clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL = "SELECT PANO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.XRAY_DETAIL";
                SQL = SQL + ComNum.VBLF + "  WHERE BDATE >= TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "    AND BDATE <= TRUNC(SYSDATE) ";
                //SQL = SQL + ComNum.VBLF + "   AND ORDERNO =" + dblOrderNo + " ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(XCODE) ='" + strSuCode.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   AND XJong ='R' ";   //MR 초음파
                SQL = SQL + ComNum.VBLF + "   AND DeptCode ='" + strDept + "' ";
                SQL = SQL + ComNum.VBLF + "   AND Pano ='" + strPtNo + "' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    reader = null;

                    //MR 초음파인것만
                    SQL = "";
                    SQL = "SELECT Code FROM KOSMOS_PMPA.BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + "     WHERE Gubun ='OCS_MR_초음파코드'";
                    SQL = SQL + ComNum.VBLF + "     AND TRIM(CODE) ='" + strSuCode + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND (DELDATE IS NULL OR DELDATE ='')";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (reader.HasRows)
                    {
                        SQL = "";
                        SQL = "INSERT INTO KOSMOS_PMPA.XRAY_DETAIL ";
                        SQL = SQL + ComNum.VBLF + " (ENTERDATE, IPDOPD, GBRESERVED, SEEKDATE, PANO, SNAME, SEX, AGE, DEPTCODE, DRCODE,";
                        SQL = SQL + ComNum.VBLF + "  WARDCODE, ROOMCODE, XJONG, XSUBCODE, XCODE, EXINFO, QTY, EXMORE, EXID, GBEND, MGRNO,";
                        SQL = SQL + ComNum.VBLF + "  GBPORTABLE , REMARK, XRAYROOM, GBNGT, DRREMARK, ORDERNO, ORDERCODE,";
                        SQL = SQL + ComNum.VBLF + "   ORDERDATE, SENDDATE , XSENDDATE, BI, BDATE,GbSPC,GbSTS ) ";
                        SQL = SQL + ComNum.VBLF + " VALUES ";
                        SQL = SQL + ComNum.VBLF + " (SYSDATE, 'O', '1', SYSDATE, '" + strPtNo + "', '" + strSName + "',        ";
                        SQL = SQL + ComNum.VBLF + "  '" + strSex + "', " + intAge + ", '" + strDept + "', '" + strDrCode + "', ";
                        SQL = SQL + ComNum.VBLF + "  '', '', 'R', '00', '" + strSuCode + "', 1, 1, ";
                        SQL = SQL + ComNum.VBLF + "  '', 0,  '', '', '', '" + strOrderName + "', '', '',  ";
                        SQL = SQL + ComNum.VBLF + "  'auto_send', " + dblOrderNo + ",'" + strOrderCode + "', ";
                        SQL = SQL + ComNum.VBLF + " SYSDATE, SYSDATE,'', '" + strBi + "' , TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ,'0','0'  )            ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                reader.Dispose();
                reader = null;

                //clsDB.setCommitTran(pDbCon);

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : OT_XRAY_ORDER_INSERT
        /// 안과검사 팍스연동 - 안저,시신경
        /// </summary>
        /// <returns></returns> 
        public static bool OT_XRAY_ORDER_INSERT(PsmhDb pDbCon, string strIO, string strPtNo, string strDept, string strDrCode, string strSName,
            string strBi, string strSex, int intAge, string strSuCode, string strOrderCode, double dblOrderNo, string strOrderName)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            OracleDataReader reader = null;
            bool rtnVal = false;

            string strXJong = "";

            
            //clsDB.setBeginTran(pDbCon);

            if(strSuCode == null || strSuCode.Trim() == "") 
            {
                return false;
            }

            switch (strSuCode.Trim())
            {
                case "E6670":
                case "E6671":
                case "E6674":
                case "E6674A":
                //2021-11-18 안과 요청으로 추가
                case "E6672":
                case "E6672A":
                    strXJong = "F";
                    break;
                case "EZ796":
                case "EZ796A":
                case "EZ796B":
                case "EZ796C":
                    strXJong = "N";
                    break;
                //2020-08-26 추가
                case "EB411":
                case "EB412":
                    strXJong = "3";
                    break;
                default:
                    return false;
            }

            string SUBCODE = "01";
            if (strSuCode.Trim().Equals("E6674A") ||
                strSuCode.Trim().Equals("E6672A"))
            {
                SUBCODE = "00";
            }


            try
            {
                //기존날짜 오더 체크
                if (strXJong != "N")
                {
                    SQL = "";
                    SQL = "SELECT PANO ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.XRAY_DETAIL";
                    SQL = SQL + ComNum.VBLF + "  WHERE BDATE >= TRUNC(SYSDATE)";
                    SQL = SQL + ComNum.VBLF + "    AND BDATE <= TRUNC(SYSDATE) ";
                    //SQL = SQL + ComNum.VBLF + "   AND ORDERNO = " + dblOrderNo + " ";
                    SQL = SQL + ComNum.VBLF + "    AND TRIM(XCODE) = '" + strSuCode + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND DeptCode ='" + strDept + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND Pano ='" + strPtNo + "' ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장 
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (reader.HasRows == false)
                    {
                        SQL = "";
                        SQL = "INSERT INTO KOSMOS_PMPA.XRAY_DETAIL ";
                        SQL = SQL + ComNum.VBLF + " (ENTERDATE, IPDOPD, GBRESERVED, SEEKDATE, PANO, SNAME, SEX, AGE, DEPTCODE, DRCODE,";
                        SQL = SQL + ComNum.VBLF + "  WARDCODE, ROOMCODE, XJONG, XSUBCODE, XCODE, EXINFO, QTY, EXMORE, EXID, GBEND, MGRNO,";
                        SQL = SQL + ComNum.VBLF + "  GBPORTABLE , REMARK, XRAYROOM, GBNGT, DRREMARK, ORDERNO, ORDERCODE,";
                        SQL = SQL + ComNum.VBLF + "   ORDERDATE, SENDDATE , XSENDDATE, BI, BDATE,GbSPC,GbSTS ) ";
                        SQL = SQL + ComNum.VBLF + " VALUES ";
                        SQL = SQL + ComNum.VBLF + " (SYSDATE, '" + strIO + "', '1', SYSDATE, '" + strPtNo + "', '" + strSName + "',        ";
                        SQL = SQL + ComNum.VBLF + "  '" + strSex + "', " + intAge + ", '" + strDept + "', '" + strDrCode + "', ";
                        SQL = SQL + ComNum.VBLF + "  '', '', '" + strXJong + "', '" + SUBCODE + "', '" + strSuCode + "', 1, 1, ";
                        SQL = SQL + ComNum.VBLF + "  '', 0,  '', '', '', '" + strOrderName + "', '', '',  ";
                        SQL = SQL + ComNum.VBLF + "  'auto_send', " + dblOrderNo + ",'" + strOrderCode + "', ";
                        SQL = SQL + ComNum.VBLF + " SYSDATE, SYSDATE,'', '" + strBi + "' , TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ,'0','0'  )            ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                    }

                    reader.Dispose();
                    reader = null;
                }
                else
                {
                    SQL = "";
                    SQL = "INSERT INTO KOSMOS_PMPA.XRAY_DETAIL ";
                    SQL = SQL + ComNum.VBLF + " (ENTERDATE, IPDOPD, GBRESERVED, SEEKDATE, PANO, SNAME, SEX, AGE, DEPTCODE, DRCODE,";
                    SQL = SQL + ComNum.VBLF + "  WARDCODE, ROOMCODE, XJONG, XSUBCODE, XCODE, EXINFO, QTY, EXMORE, EXID, GBEND, MGRNO,";
                    SQL = SQL + ComNum.VBLF + "  GBPORTABLE , REMARK, XRAYROOM, GBNGT, DRREMARK, ORDERNO, ORDERCODE,";
                    SQL = SQL + ComNum.VBLF + "   ORDERDATE, SENDDATE , XSENDDATE, BI, BDATE,GbSPC,GbSTS ) ";
                    SQL = SQL + ComNum.VBLF + " VALUES ";
                    SQL = SQL + ComNum.VBLF + " (SYSDATE, '" + strIO + "', '1', SYSDATE, '" + strPtNo + "', '" + strSName + "',        ";
                    SQL = SQL + ComNum.VBLF + "  '" + strSex + "', " + intAge + ", '" + strDept + "', '" + strDrCode + "', ";
                    SQL = SQL + ComNum.VBLF + "  '', '', '" + strXJong + "', '" + SUBCODE + "', '" + strSuCode + "', 1, 1, ";
                    SQL = SQL + ComNum.VBLF + "  '', 0,  '', '', '', '" + strOrderName + "', '', '',  ";
                    SQL = SQL + ComNum.VBLF + "  'auto_send', " + dblOrderNo + ",'" + strOrderCode + "', ";
                    SQL = SQL + ComNum.VBLF + " SYSDATE, SYSDATE,'', '" + strBi + "' , TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ,'0','0'  )            ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                //clsDB.setCommitTran(pDbCon);

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_PT_BDate_CHK
        /// </summary>
        /// <returns></returns>
        public static string Read_PT_BDate_CHK(PsmhDb pDbCon, string strPtNo, string strBDate, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE  ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PT_BDATE ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SuCode ='" + strSuCode + "' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).ToString().Trim();

                    //2년경과
                    if (VB.DateDiff("d", Convert.ToDateTime(strBDate), Convert.ToDateTime(rtnVal)) > 730)
                    {
                        if (strSuCode == "MM105A")
                        {
                            rtnVal = "+" + rtnVal;
                            ComFunc.MsgBox(strSuCode + " 해당수가는 처음발병 2년경과 하였습니다..참고하십시오!!", "오더확인");
                        }
                        else
                        {
                            rtnVal = "#" + rtnVal;
                            ComFunc.MsgBox(strSuCode + " 해당수가는 처음발병 2년경과 하였습니다..오더불가!!", "오더불가");
                        }
                    }
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_CHK_TA52
        /// 2014-04-17
        /// 교통 52, 55 종 일경우 HA471M , BC6 재확인 메시지
        /// 공용모듈이라서 입원,외래 컬럼값 틀림
        /// </summary>
        /// <returns></returns>
        public static string READ_CHK_TA52(PsmhDb pDbCon, string strGBN, FarPoint.Win.Spread.FpSpread ssSpread)
        {
            string rtnVal = "";
            string strDataChk = "";
            int intCol = 0;
            //int i = 0;

            rtnVal = "OK";

            if (strGBN == "I" || strGBN == "O" || strGBN == "E")
            {
                for (int i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text.Trim() != "True")
                    {
                        //intCol = 14;        //입원
                        //2021-01-12 변경
                        intCol = 17;

                        if (strGBN == "O")
                        {
                            //intCol = 12;        //외래
                            //2021-01-12 변경
                            intCol = 17;
                        }

                        if (ssSpread.ActiveSheet.Cells[i, intCol].Text.Trim() == "HA471M"
                            || ssSpread.ActiveSheet.Cells[i, intCol].Text.Trim() == "BC6"
                            || ssSpread.ActiveSheet.Cells[i, intCol].Text.Trim() == "BZ073"
                            || ssSpread.ActiveSheet.Cells[i, intCol].Text.Trim() == "MM261"
                            || ssSpread.ActiveSheet.Cells[i, intCol].Text.Trim() == "MM261A")
                        {
                            strDataChk = "OK";
                            break;
                        }
                    }
                }

                if (strDataChk == "OK")
                {
                    if (ComFunc.MsgBoxQ("자동차보험 52,55종일경우 HA471M,BC6,BZ073,MM261,MM261A 수가재확인!! "
                        + ComNum.VBLF + "원무 자보담당자 요청건(청구삭감때문)!! 문의 ☎260-8102 "
                        + ComNum.VBLF + "이대로 전송하시겠습니까??", "수가재확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                    {
                        rtnVal = "NO";
                    }
                }

                if (rtnVal == "NO")
                {
                    return rtnVal;
                }

                strDataChk = "";

                for (int i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text.Trim() != "True")
                    {
                        //intCol = 14;        //입원
                        //2021-01-12 변경
                        intCol = 17;

                        if (strGBN == "O")
                        {
                            //intCol = 12;        //외래
                            //2021-01-12 변경
                            intCol = 17;
                        }

                        if (strGBN == "E")
                        {
                            intCol = 13;        //응급
                        }

                        if (ssSpread.ActiveSheet.Cells[i, intCol].Text.Trim() == "HYAL"
                            || ssSpread.ActiveSheet.Cells[i, intCol].Text.Trim() == "HYAL-F")
                        {
                            strDataChk = "OK";
                            break;
                        }
                    }
                }

                if (strDataChk == "OK")
                {
                    if (ComFunc.MsgBoxQ("산재,자보환자입니다!!! HYAL,HYAL-F 수가코드 본인부담을 설명하셨습니까?"
                        + ComNum.VBLF + "원무 자보담당자 요청건(청구삭감때문)!! "
                        + ComNum.VBLF + "이대로 전송하시겠습니까??", "수가재확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                    {
                        rtnVal = "NO";
                    }
                }

                if (rtnVal == "NO")
                {
                    return rtnVal;
                }

                strDataChk = "";

                for (int i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text.Trim() != "True")
                    {
                        //2021-01-12 변경
                        //intCol = 14;        //입원
                        intCol = 17;

                        if (strGBN == "O")
                        {
                            //2021-01-12 변경
                            //intCol = 12;        //외래
                            intCol = 17;
                        }

                        if (strGBN == "E")
                        {
                            intCol = 13;        //응급
                        }

                        //==============================================================================
                        //2016-08-16 계장 김현욱
                        //자보환자 MRI 제한 코드 원무과에서 적용 가능하도록 작업
                        if (READ_WONMU_JABO_MRI(pDbCon, ssSpread.ActiveSheet.Cells[i, intCol].Text.Trim()) == "OK")
                        {
                            strDataChk = "OK";
                            break;
                        }
                    }
                }

                if (strDataChk == "OK")
                {
                    ComFunc.MsgBox("자보환자 임의 MRI오더 발생안됨!!!" + ComNum.VBLF + "원무과 자보당담자에게 문의하십시오!!", "전송불가!!");
                    rtnVal = "NO";
                }
            }

            //2015-10-15
            if (rtnVal == "NO")
            {
                return rtnVal;
            }

            for (int i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
            {
                if (ssSpread.ActiveSheet.Cells[i, 0].Text.Trim() != "True")
                {
                    //2021-01-12 변경
                    //intCol = 14;        //입원
                    intCol = 17;

                    if (strGBN == "O")
                    {
                        //2021-01-12 변경
                        //intCol = 12;        //외래
                        intCol = 17;
                    }

                    if (strGBN == "E")
                    {
                        intCol = 13;        //응급
                    }

                    if (ssSpread.ActiveSheet.Cells[i, intCol].Text.Trim() == "BM5000RQ")
                    {
                        strDataChk = "OK";
                        break;
                    }
                }
            }

            if (strDataChk == "OK")
            {
                ComFunc.MsgBox("자보환자자격시 재료대 코드 BM5000RQ 처방불가!! [청구삭감]" + ComNum.VBLF + "원무과 자보당담자에게 문의하십시오!!", "전송불가!!");
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_원무과_자보_MRI
        /// </summary>
        /// <returns></returns>
        public static string READ_WONMU_JABO_MRI(PsmhDb pDbCon, string strCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT CODE";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN = 'OCS_MRI_원무과_제어_코드'";
                SQL = SQL + ComNum.VBLF + "    AND CODE = '" + strCode + "'";
                SQL = SQL + ComNum.VBLF + "    AND DELDATE IS NULL";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("함수명 : " + "READ_WONMU_JABO_MRI" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "READ_WONMU_JABO_MRI" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_DRUG_OUTCHK
        /// 2014-06-09
        /// </summary>
        /// <returns></returns>
        public static string READ_DRUG_OUTCHK(PsmhDb pDbCon, string strSuCode, string strBun)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";
            string strOK = "";

            try
            {
                if (strBun != "")
                {
                    if (strBun != "11" && strBun != "12" && strBun != "13")
                    {
                        return rtnVal;
                    }
                }

                SQL = "";
                SQL = "SELECT TO_CHAR(A.DELDATE,'YYYY-MM-DD') DELDATE ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_DRUGINFO_new  A , KOSMOS_ADM.DRUG_JEP B";
                SQL = SQL + ComNum.VBLF + " WHERE A.SUNEXT = B.JEPCODE(+) ";
                SQL = SQL + ComNum.VBLF + "   AND a.SuNext ='" + strSuCode + "' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows && reader.Read())
                {
                    if (reader.GetValue(0).To<string>().NotEmpty())
                    {
                        rtnVal = "삭제";
                        strOK = "OK";
                    }
                }

                reader.Dispose();
                reader = null;

                if (strOK == "OK")
                {
                    return rtnVal;
                }

                SQL = "";
                SQL = "SELECT SUGBJ FROM KOSMOS_PMPA.BAS_SUT ";
                SQL = SQL + ComNum.VBLF + " WHERE SUCODE ='" + strSuCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SUGBJ ='1' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = "원외";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;

                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : LONG_ANTI_USED_SMS
        /// </summary>
        /// <returns></returns>
        public static void LONG_ANTI_USED_SMS(PsmhDb pDbCon, string strPtNo, int intDay, string strCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            DataTable dt = null;

            string strTel = "";
            string strMsg = "";

            string strSname = "";
            string strWARD = "";
            string strRoom = "";
            string strDrCode = "";

            //장기항생제 알람 뜰 경우 주치의에게 문자 날리기
            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL = "SELECT PANO FROM KOSMOS_PMPA.ETC_SMS ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '58'";
                SQL = SQL + ComNum.VBLF + "   AND JOBDATE >= TO_DATE('" + clsPublic.GstrSysDate + " 00:00','YYYY-MM-DD HH24:MI')  ";
                SQL = SQL + ComNum.VBLF + "   AND JOBDATE <= TO_DATE('" + clsPublic.GstrSysDate + " 23:59','YYYY-MM-DD HH24:MI')  ";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SNAME = '" + strCode + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;

                    return;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT WARDCODE, ROOMCODE, SNAME, DRCODE";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "    AND (JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') OR OUTDATE = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')) ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strSname = dt.Rows[0]["SNAME"].ToString().Trim();
                    strWARD = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    strRoom = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    strDrCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strDrCode == "")
                {
                    return;
                }

                SQL = "";
                SQL = "SELECT A.HTEL";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + " WHERE A.SABUN = B.SABUN";
                SQL = SQL + ComNum.VBLF + "      AND B.DRCODE = '" + strDrCode + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strTel = dt.Rows[0]["HTEL"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strTel == "")
                {
                    return;
                }

                strTel = strTel.Replace("-", "");

                //장기항생제사용알림(15일째,W-CTX) 65(659호) 홍길동1234567

                strMsg = "장기항생제사용알림(" + intDay + "일째, " + strCode + ") " + strWARD + "(" + strRoom + "호) " + strSname + "(" + strPtNo + ")";

                SQL = "";
                SQL = "INSERT INTO KOSMOS_PMPA.ETC_SMS (";
                SQL = SQL + ComNum.VBLF + " JOBDATE, PANO, SNAME, HPHONE, ";
                SQL = SQL + ComNum.VBLF + " GUBUN, DEPTCODE, DRCODE, RTIME, ";
                SQL = SQL + ComNum.VBLF + " RETTEL,SENDTIME, SENDMSG) VALUES ( ";
                SQL = SQL + ComNum.VBLF + " SYSDATE ,'" + strPtNo + "','" + strCode + "','" + strTel + "',";
                SQL = SQL + ComNum.VBLF + "'58','','',SYSDATE,";
                SQL = SQL + ComNum.VBLF + "'0542608019','','" + strMsg + "') ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_ORDER_입퇴원기록지체크
        /// 오더저장시 입원기록지 24시간전 체크, 퇴원오더시 퇴원기록지 유무
        /// </summary>
        /// <returns></returns>
        public static string READ_ORDER_IPD_CHART_CHK(PsmhDb pDbCon, string strGubun, string strPtNo, string strDeptCode, double dblIpdNo, string strSTS)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            OracleDataReader reader = null;

            string rtnVal = "OK";
            
            if (strSTS == "ON")
            {
                return rtnVal;
            }

            ComFunc.ReadSysDate(pDbCon);

            string strDate = clsPublic.GstrSysDate.Replace("-", "");
            string strTime = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;   //현재시각

            string strInDate_ER = "";
            string strInDate = "";
            string strInDate2 = "";
            string strTime2 = "";
            string strERChk = "";
            string strER = "";


            //2020-09-28 안정수, 의료정보팀 요청으로 추석연휴기간동안 체크 안하도록 
            //2021-02-10 안정수, 의료정보팀 요청으로   설연휴기간동안 체크 안하도록 
            if (Convert.ToDateTime(strTime) > Convert.ToDateTime("2021-02-11 00:00") &&
                Convert.ToDateTime(strTime) < Convert.ToDateTime("2021-02-15 09:00")) 
            {
                return "OK";
            }

            //2021-09-16 이현종, 의료정보팀 요청으로 추석연휴기간동안 체크 안하도록 
            if (Convert.ToDateTime(strTime) > Convert.ToDateTime("2021-09-18 00:00") &&
                Convert.ToDateTime(strTime) < Convert.ToDateTime("2021-09-24 09:00"))
            {
                return "OK";
            }

            try
            {
                //예외처리체크
                SQL = "";
                SQL = "SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='OCS_차트체크_예외처리' ";
                SQL = SQL + ComNum.VBLF + "  AND TRIM(CODE) ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='') ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    reader.Dispose();
                    reader = null;
                    return rtnVal;
                }

                reader.Dispose();
                reader = null;

                //재원체크 2015-06-02 기록실장 - 입원시간변경요청
                SQL = "";
                SQL = "SELECT AmSet7,TO_CHAR(InDate,'YYYY-MM-DD') InDate,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(IPWONTIME,'YYYY-MM-DD') IpWonTime1 ,TO_CHAR(IPWONTIME,'YYYY-MM-DD HH24:MI') IpWonTime2 ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND IPDNO =" + dblIpdNo + " ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strInDate_ER = dt.Rows[0]["INDATE"].ToString().Replace("-", "").Trim();
                    strInDate = dt.Rows[0]["IpWonTime1"].ToString().Replace("-", "").Trim();
                    strTime2 = dt.Rows[0]["IpWonTime2"].ToString().Trim();

                    strERChk = dt.Rows[0]["AmSet7"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;


                if (strERChk == "3" || strERChk == "4" || strERChk == "5")
                {
                    strER = "OK";
                }

                if (strGubun == "입원기록지")
                {
                    using (ComFunc ComFuncX = new ComFunc())
                    {
                        //입원기준 1일 지난경우
                        if (ComFuncX.DATE_TIME(pDbCon, strTime2, strTime) > 720)
                        //if (VB.DateDiff("n", Convert.ToDateTime(strTime2), Convert.ToDateTime(strTime)) > 720)
                        {
                            SQL = "";
                            SQL = "SELECT Ptno,CHARTDATE, CHARTTIME ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST";

                            if (strER == "OK")
                            {
                                //전산업무 의뢰서 2020-19 입원기록지 입원전 작성에 따른 오더제한 풀어주세요
                                SQL = SQL + ComNum.VBLF + "  WHERE CHARTDATE >= '" + Convert.ToString((int)VB.Val(strInDate_ER) - 2) + "' ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "  WHERE CHARTDATE >= '" + strInDate + "' ";
                            }

                            SQL = SQL + ComNum.VBLF + "   AND Ptno ='" + strPtNo + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND FORMNO IN ( SELECT FORMNO FROM KOSMOS_EMR.EMRFORM WHERE UPPER(FORMNAME) LIKE '%ADMISSION NOTE%') ";

                            SQL = SQL + ComNum.VBLF + "UNION ALL ";
                            SQL = SQL + ComNum.VBLF + "SELECT Ptno,CHARTDATE, CHARTTIME ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST";
                            if (strER == "OK")
                            {
                                //전산업무 의뢰서 2020-19 입원기록지 입원전 작성에 따른 오더제한 풀어주세요
                                SQL = SQL + ComNum.VBLF + "  WHERE CHARTDATE >= '" + Convert.ToString((int)VB.Val(strInDate_ER) - 2) + "' ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "  WHERE CHARTDATE >= '" + strInDate + "' ";
                            }

                            SQL = SQL + ComNum.VBLF + "   AND Ptno ='" + strPtNo + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND FORMNO IN ( SELECT FORMNO FROM KOSMOS_EMR.AEMRFORM WHERE UPPER(FORMNAME) LIKE '%ADMISSION NOTE%' AND OLDGB = '0') ";


                            SQL = SQL + ComNum.VBLF + " ORDER BY CHARTDATE DESC, CHARTTIME DESC  ";

                            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                            if (reader.HasRows == false)
                            {
                                reader.Dispose();
                                reader = null;
                                rtnVal = "입원기록지누락!!";
                                ComFunc.MsgBox("12시간이내 입원기록지가 작성되지 않았습니다." + "\r\n\r\n" + "작성 후 처방전달 바랍니다." + "\r\n\r\n" + "문의:의무기록실 이동춘(8041)", "확인");
                                return rtnVal;
                            }

                            reader.Dispose();
                            reader = null;
                        }
                    }

                    //2019-03-21 유진호
                    //의료정보팀장 요청으로 의무원장은 경과기록지 3일제한 제외함.
                    //2020-05-20 안정수
                    //의료정보팀장 요청으로 이종훈과장은 경과기록지 3일제한 제외함.
                    if (clsType.User.Sabun == "46037" || clsType.User.Sabun == "50880")
                    {

                    }
                    else
                    {
                        //3일 입원경과 기록 체크
                        //마지막 입원경과기록체크
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT CHARTDATE, CHARTTIME ";
                        SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTMST";
                        SQL = SQL + ComNum.VBLF + " WHERE CHARTDATE >= '" + strInDate + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND Ptno ='" + strPtNo + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND InOutCls ='I' ";      // 입원
                        SQL = SQL + ComNum.VBLF + "   AND FORMNO = 963 ";
                        SQL = SQL + ComNum.VBLF + "UNION ALL ";
                        SQL = SQL + ComNum.VBLF + "SELECT CHARTDATE, CHARTTIME ";
                        SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.EMRXMLMST";
                        SQL = SQL + ComNum.VBLF + " WHERE CHARTDATE >= '" + strInDate + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND Ptno     ='" + strPtNo + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND InOutCls ='I' ";      // 입원
                        SQL = SQL + ComNum.VBLF + "   AND FORMNO   = 963 ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY CHARTDATE DESC, CHARTTIME DESC  ";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        if (reader.HasRows && reader.Read())
                        {
                            strInDate2 = ComFunc.FormatStrToDateEx(reader.GetValue(0).ToString().Trim(), "D", "-") + " " + ComFunc.FormatStrToDateEx(reader.GetValue(1).ToString().Trim(), "M", ":");
                        }

                        reader.Dispose();
                        reader = null;

                        using (ComFunc ComFuncX = new ComFunc())
                        {
                            if (strInDate2 == "")
                            {
                                if (ComFuncX.DATE_TIME(pDbCon, strTime2, strTime) > 3600)
                                {
                                    rtnVal = "입원경과기록누락!!";
                                    ComFunc.MsgBox("경과기록지가 3일이내 작성되지 않았습니다." + "\r\n\r\n" + "작성 후 처방전달 바랍니다." + "\r\n\r\n" + "문의:의무기록실 이동춘(8041)", "확인");
                                    return rtnVal;
                                }
                            }
                            else
                            {
                                if (ComFuncX.DATE_TIME(pDbCon, strInDate2, strTime) > 3600)
                                {
                                    rtnVal = "입원경과기록누락!!";
                                    ComFunc.MsgBox("경과기록지가 3일이내 작성되지 않았습니다." + "\r\n\r\n" + "작성 후 처방전달 바랍니다." + "\r\n\r\n" + "문의:의무기록실 이동춘(8041)", "확인");
                                    return rtnVal;
                                }
                            }
                        }
                    }                   
                }
                //약이 있으면 점검
                else if (strGubun == "외래기록지")
                {
                    SQL = "";
                    SQL = "SELECT Ptno,CHARTDATE, CHARTTIME ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + " WHERE CHARTDATE = '" + strInDate + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND Ptno ='" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND MedDeptCD  ='" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND InOutCls ='O' ";  //외래
                    SQL = SQL + ComNum.VBLF + "   AND FORMNO = 963 ";
                    SQL = SQL + ComNum.VBLF + "UNION ALL ";
                    SQL = SQL + ComNum.VBLF + "SELECT Ptno,CHARTDATE, CHARTTIME ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTMST";
                    SQL = SQL + ComNum.VBLF + " WHERE CHARTDATE = '" + strInDate + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND Ptno ='" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND MedDeptCD  ='" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND InOutCls ='O' ";      // 외래
                    SQL = SQL + ComNum.VBLF + "   AND FORMNO = 963 ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY CHARTDATE DESC, CHARTTIME DESC  ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (reader.HasRows == false)
                    {
                        reader.Dispose();
                        reader = null;

                        SQL = "";
                        SQL = "SELECT Ptno,CHARTDATE, CHARTTIME ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST";
                        SQL = SQL + ComNum.VBLF + "  WHERE CHARTDATE = '" + strInDate + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND Ptno ='" + strPtNo + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND MedDeptCD  ='" + strDeptCode + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND InOutCls ='O' ";  //외래
                        SQL = SQL + ComNum.VBLF + "   AND FORMNO IN (  SELECT FORMNO FROM KOSMOS_EMR.EMRFORM WHERE FORMNAME LIKE '%초진%'  ) ";
                        SQL = SQL + ComNum.VBLF + "UNION ALL ";
                        SQL = SQL + ComNum.VBLF + "SELECT Ptno,CHARTDATE, CHARTTIME ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST";
                        SQL = SQL + ComNum.VBLF + "  WHERE CHARTDATE >= '" + strInDate + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND Ptno ='" + strPtNo + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND InOutCls ='O' ";      // 외래
                        SQL = SQL + ComNum.VBLF + "   AND FORMNO IN (  SELECT FORMNO FROM KOSMOS_EMR.AEMRFORM WHERE FORMNAME LIKE '%초진%'  ) ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY CHARTDATE DESC, CHARTTIME DESC  ";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        if (reader.HasRows == false)
                        {
                            reader.Dispose();
                            reader = null;
                            rtnVal = "외래경과기록누락!!";
                            ComFunc.MsgBox("외래경과기록이 작성되지 않았습니다." + "\r\n\r\n" + "외래경과기록 작성 후 처방전달 바랍니다." + "\r\n\r\n" + "문의:의무기록실 이동춘(8041)", "확인");
                            return rtnVal;
                        }
                    }

                    reader.Dispose();
                    reader = null;
                }
                else if (strGubun == "퇴원기록지")
                {
                    SQL = "";
                    SQL = "SELECT Ptno,CHARTDATE, CHARTTIME ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + " WHERE CHARTDATE >= '" + strInDate + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND Ptno ='" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND FORMNO IN ( SELECT FORMNO FROM KOSMOS_EMR.EMRFORM WHERE FORMNAME LIKE '%입퇴원%') ";
                    SQL = SQL + ComNum.VBLF + "UNION ALL ";
                    SQL = SQL + ComNum.VBLF + "SELECT Ptno,CHARTDATE, CHARTTIME ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTMST";
                    SQL = SQL + ComNum.VBLF + " WHERE CHARTDATE >= '" + strInDate + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND Ptno ='" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND FORMNO IN (  SELECT FORMNO FROM KOSMOS_EMR.AEMRFORM WHERE FORMNAME LIKE '%입퇴원%'  ) ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (reader.HasRows == false)
                    {
                        reader.Dispose();
                        reader = null;
                        rtnVal = "퇴원기록지누락!!";
                        ComFunc.MsgBox("퇴원요약지가 작성되지 않았습니다." + "\r\n\r\n" + "작성 후 처방전달 바랍니다." + "\r\n\r\n" + "문의:의무기록실 이동춘(8041)", "확인");
                        return rtnVal;
                    }

                    reader.Dispose();
                    reader = null;
                }
                else if (strGubun == "재평가기록지")  //2019-10-04 전산업무의뢰서 2019-1193
                {
                    string strDate1 = "";
                    string strDate2 = "";

                    ComFunc.ReadSysDate(pDbCon);

                    int HD = 0;
                    int QUOTIENT = 0;
                    int REMAINDER = 0;

                    using (ComFunc CF = new ComFunc())
                    {
                        HD = CF.DATE_ILSU(clsDB.DbCon, clsPublic.GstrSysDate, ComFunc.FormatStrToDateEx(strInDate, "D", "-"), "");
                    }

                    QUOTIENT = HD / 30;
                    REMAINDER = HD % 30;

                    if (REMAINDER >= 29)
                    {
                        strDate1 = Convert.ToDateTime(ComFunc.FormatStrToDateEx(strInDate, "D", "-")).AddDays(QUOTIENT * 30).ToString("yyyyMMdd");
                        strDate2 = Convert.ToDateTime(ComFunc.FormatStrToDateEx(strInDate, "D", "-")).AddDays((QUOTIENT + 1) * 30).ToString("yyyyMMdd");

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT EMRNO FROM KOSMOS_EMR.EMRXML         ";
                        SQL = SQL + ComNum.VBLF + "  WHERE INOUTCLS = 'I'                       ";
                        SQL = SQL + ComNum.VBLF + "    AND FORMNO = 2464                        ";
                        SQL = SQL + ComNum.VBLF + "    AND PTNO = '" + strPtNo + "'             ";
                        SQL = SQL + ComNum.VBLF + "    AND CHARTDATE >= '" + strDate1 + "'      ";
                        SQL = SQL + ComNum.VBLF + "    AND CHARTDATE <= '" + strDate2 + "'      ";
                        SQL = SQL + ComNum.VBLF + " UNION ALL                                   ";
                        SQL = SQL + ComNum.VBLF + " SELECT EMRNO FROM KOSMOS_EMR.AEMRCHARTMST   ";
                        SQL = SQL + ComNum.VBLF + "  WHERE INOUTCLS = 'I'                       ";
                        SQL = SQL + ComNum.VBLF + "    AND FORMNO = 2464                        ";
                        SQL = SQL + ComNum.VBLF + "    AND PTNO = '" + strPtNo + "'             ";
                        SQL = SQL + ComNum.VBLF + "    AND CHARTDATE >= '" + strDate1 + "'      ";
                        SQL = SQL + ComNum.VBLF + "    AND CHARTDATE <= '" + strDate2 + "'      ";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        if (reader.HasRows == false)
                        {
                            reader.Dispose();
                            reader = null;
                            rtnVal = "재평가기록지누락!!";
                            ComFunc.MsgBox("재원기간 " + (QUOTIENT * 30) + "일이 경과하여 재평가 기록지를 작성하여야 합니다." + "\r\n\r\n" + "작성 후 처방전달 바랍니다." + "\r\n\r\n" + "문의:의무기록실 이동춘(8041)", "확인");
                            return rtnVal;
                        }

                        reader.Dispose();
                        reader = null;
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("READ_ORDER_IPD_CHART_CHK : " + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("READ_ORDER_IPD_CHART_CHK : " + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_ORDER_중환자실입실체크
        /// </summary>
        /// <returns></returns>
        public static string READ_ORDER_ICU_IN_CHK(PsmhDb pDbCon, string strPtNo, double dblIpdNo)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "OK";

            string strTime = "";
            string strTime2 = "";
            string strDate = "";

            //-기준 정립
            //중환자실 바로 입원할경우
            //일반병동에서 중환자실로 갈경우
            //중환자실 있다가 일반갓다가 다시 중환자실 갈경우

            ComFunc.ReadSysDate(pDbCon);

            strDate = clsPublic.GstrSysDate.Replace("-", "");

            strTime = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;  //현재시각
            strTime2 = strTime;

            try
            {
                //재원체크 2015-06-02 기록실장 - 입원시간변경요청
                SQL = "";
                SQL = "SELECT TO_CHAR(TrsDate,'YYYY-MM-DD') TrsDate , TO_CHAR(TrsDate,'YYYY-MM-DD HH24:MI') TrsTime ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_TRANSFOR ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND IPDNO =" + dblIpdNo + " ";
                SQL = SQL + ComNum.VBLF + "ORDER BY TrsDate DESC  ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strTime2 = dt.Rows[0]["TrsTime"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (VB.DateDiff("n", Convert.ToDateTime(strTime2), Convert.ToDateTime(strTime)) > 360)
                {
                    //6시간 경화 중환자실 입실기준 기록지 점검
                    SQL = "";
                    SQL = "SELECT Ptno,CHARTDATE, CHARTTIME ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE CHARTDATE >= '" + strDate + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND Ptno ='" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND FORMNO IN ( SELECT FORMNO FROM KOSMOS_EMR.EMRFORM WHERE FormNo = 2286   ) ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY CHARTDATE DESC, CHARTTIME DESC  ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        rtnVal = "중환자실 입실기준 기록지 누락!!";
                        ComFunc.MsgBox("6시간 이내 중환자실 입실기준 기록지가 작성되지 않았습니다." + "\r\n\r\n" + "작성 후 처방전달 바랍니다." + "\r\n\r\n" + "문의:의무기록실 이동춘(8041)", "확인");
                    }

                    dt.Dispose();
                    dt = null;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_ORDER_중환자실APACHE체크
        /// </summary>
        /// <returns></returns>
        public static string READ_ORDER_ICU_APACHE_CHK(PsmhDb pDbCon, string strPtNo, double dblIpdNo)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "OK";

            string strIpTime = "";
            string strTsTime = "";

            string strInWard = "";

            string strInDate = "";

            string strTime = "";
            string strTime2 = "";
            string strDate = "";

            ComFunc.ReadSysDate(pDbCon);

            try
            {
                SQL = "";
                SQL = "SELECT CODE ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'OCS_APACHE작성점검'";
                SQL = SQL + ComNum.VBLF + "  AND CODE = '시행'";
                SQL = SQL + ComNum.VBLF + "  AND NAME = 'Y'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;

                    return rtnVal;
                }

                dt.Dispose();
                dt = null;

                strDate = clsPublic.GstrSysDate.Replace("-", "");

                strTime = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;     //현재시각

                SQL = "";
                SQL = "SELECT INWARD ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + dblIpdNo;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strInWard = dt.Rows[0]["INWARD"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT AmSet7,TO_CHAR(InDate,'YYYY-MM-DD') InDate,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(IPWONTIME,'YYYY-MM-DD') IpWonTime1 ,TO_CHAR(IPWONTIME,'YYYY-MM-DD HH24:MI') IpWonTime2 ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND IPDNO =" + dblIpdNo;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strIpTime = dt.Rows[0]["IPWONTIME2"].ToString().Trim();
                    strInDate = dt.Rows[0]["INDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT TO_CHAR(MIN(TrsDate),'YYYY-MM-DD HH24:MI') TrsTime ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_TRANSFOR ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND IPDNO =" + dblIpdNo;
                SQL = SQL + ComNum.VBLF + "  AND TOWARD IN ('33','35')";
                SQL = SQL + ComNum.VBLF + "  AND TRSDATE >= TO_DATE('" + Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-3).ToString("yyyy-MM-dd") + " " + clsPublic.GstrSysTime + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "ORDER BY TrsDate DESC  ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strTsTime = dt.Rows[0]["TRSTIME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strInWard == "33" || strInWard == "35")
                {
                    strTime2 = strIpTime;
                }
                else
                {
                    strTime2 = strTsTime;

                    if (strTime2 == "")
                    {
                        strTime2 = strIpTime;
                    }
                }

                if ((Convert.ToDateTime(strTime2) - Convert.ToDateTime(strTime)).TotalMinutes > 1440)
                {
                    SQL = "";
                    SQL = "SELECT Ptno,CHARTDATE, CHARTTIME ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE MEDFRDATE = '" + strInDate.Replace("-", "") + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND PTNO ='" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND FORMNO = '2597'";

                    SQL = SQL + ComNum.VBLF + " UNION ALL";
                    SQL = SQL + ComNum.VBLF + " SELECT Ptno,CHARTDATE, CHARTTIME";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE MEDFRDATE = '" + strInDate.Replace("-", "") + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND PTNO ='" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND FORMNO = 2597"; 

                    SQL = SQL + ComNum.VBLF + " ORDER BY CHARTDATE DESC, CHARTTIME DESC  ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        rtnVal = "중환자실 ASAPS 3 SCORE 기록지 누락!!";
                        ComFunc.MsgBox(" ◈ SAPS 3 스코어 작성후 오더지시하시기 바랍니다 ◈ " + "\r\n\r\n" + " - 문의:의무기록실 이동춘(8041)", "확인");
                    }

                    dt.Dispose();
                    dt = null;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : ILLS_CHK_ORDER
        /// 2015-01-26
        /// </summary>
        /// <returns></returns>
        public static string ILLS_CHK_ORDER(PsmhDb pDbCon, string strGubun, string strBi, string strDept, FarPoint.Win.Spread.FpSpread ssSpread)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "OK";
            int i = 0;

            string strIll = "";

            if (strGubun != "D1")
            {
                return rtnVal;
            }

            if (strBi != "11" && strBi != "12" && strBi != "13" && strBi != "21" && strBi != "22")
            {
                return rtnVal;
            }

            try
            {
                for (i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
                {
                    if (clsOrdFunction.GEnvSet_Item21 != null && clsOrdFunction.GEnvSet_Item21 == "2")
                    {
                        strIll = ssSpread.ActiveSheet.Cells[i, 2].Text.Trim().ToUpper();
                    }
                    else
                    {
                        strIll = ssSpread.ActiveSheet.Cells[i, 0].Text.Trim().ToUpper();
                    }
                        

                    SQL = "";
                    SQL = "SELECT Remark FROM KOSMOS_PMPA.BAS_MSELF ";
                    SQL = SQL + ComNum.VBLF + " WHERE GubunA ='D' ";
                    SQL = SQL + ComNum.VBLF + "   AND GubunB ='1'  ";
                    SQL = SQL + ComNum.VBLF + "   AND (FieldA ='" + strDept + "' OR FieldA='**' ) ";
                    SQL = SQL + ComNum.VBLF + "   AND SuCode ='" + strIll + "' ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (reader.HasRows && reader.Read())
                    {
                        rtnVal = "D1 체크";
                        ComFunc.MsgBox(strIll + "상병은 보험불가 상병입니다..상병변경 및 자격변경 부탁합니다." + "\r\n\r\n" + "(참고:" + reader.GetValue(0).ToString().Trim() + ")", "상병체크");
                    }
                    reader.Dispose();
                    reader = null;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_Suga_NameK
        /// 2015-05-11
        /// </summary>
        /// <returns></returns>
        public static string READ_Suga_NameK(PsmhDb pDbCon, string strSuCode, string strGBN = "")
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT SUNAMEK FROM KOSMOS_PMPA.BAS_SUN ";
                SQL = SQL + ComNum.VBLF + " WHERE SUNEXT ='" + strSuCode + "' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows && reader.Read())
                {
                    if (strGBN == "")
                    {
                        rtnVal = reader.GetValue(0).ToString().Trim();
                    }
                    else if (strGBN == "1")
                    {
                        rtnVal = strSuCode + " (" + reader.GetValue(0).ToString().Trim() + ")";
                    }
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_PRN_Suga_Chk
        /// </summary>
        /// <returns></returns>
        public static string Read_PRN_Suga_Chk(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT ROWID FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN ='OCS_PRN_CODE' ";
                SQL = SQL + ComNum.VBLF + "  AND TRIM(Code) ='" + strSuCode + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_Verb_2015_Suga_Chk
        /// </summary>
        /// <returns></returns>
        public static string Read_Verb_2015_Suga_Chk(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "OK";

            try
            {
                //기본테이블 체크
                SQL = "";
                SQL = "SELECT ROWID FROM KOSMOS_ADM.DRUG_MASTER4  ";
                SQL = SQL + ComNum.VBLF + " WHERE NOT_VERBAL ='1' ";
                SQL = SQL + ComNum.VBLF + "  AND TRIM(JepCode) ='" + strSuCode + "' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("함수명 : " + "Read_Verb_2015_Suga_Chk" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "Read_Verb_2015_Suga_Chk" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = "구두처방 제한코드입니다";
                    ComFunc.MsgBox(strSuCode + " 구두처방 금지의약품!!  처방 불가합니다..", "확인");
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("함수명 : " + "Read_Verb_2015_Suga_Chk" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "Read_Verb_2015_Suga_Chk" + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_NotDiv_2015_Suga_Chk
        /// 분할불가 수가체크
        /// </summary>
        /// <returns></returns>
        public static string Read_NotDiv_2015_Suga_Chk(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                //기본테이블 체크
                SQL = "";
                SQL = "SELECT ROWID FROM KOSMOS_ADM.DRUG_MASTER4  ";
                SQL = SQL + ComNum.VBLF + " WHERE NOT_DIV ='1' ";
                SQL = SQL + ComNum.VBLF + "  AND TRIM(JepCode) ='" + strSuCode + "' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("함수명 : " + "Read_NotDiv_2015_Suga_Chk" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "Read_NotDiv_2015_Suga_Chk" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("함수명 : " + "Read_NotDiv_2015_Suga_Chk" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "Read_NotDiv_2015_Suga_Chk" + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_NotDiv_2015_Suga_Replace
        /// 분할불가 대체약 수가
        /// </summary>
        /// <returns></returns>
        public static string Read_NotDiv_2015_Suga_Replace(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            int i = 0;

            string rtnVal = "";

            try
            {
                //대체테이블 체크
                SQL = "";
                SQL = "SELECT R_JEPCODE                                                         ";
                SQL = SQL + ComNum.VBLF + "  , (SELECT SUNAMEK FROM KOSMOS_PMPA.BAS_SUN         ";
                SQL = SQL + ComNum.VBLF + "      WHERE SUNEXT = A.JEPCODE                       ";
                SQL = SQL + ComNum.VBLF + "    ) AS SUNAMEK                                     ";

                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.DRUG_MASTER4_REPLACE_NOTDIV A       ";
                SQL = SQL + ComNum.VBLF + " WHERE JEPCODE ='" + strSuCode.PadRight(8, ' ') + "' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = strSuCode + " 수가 분할불가코드" + "\r\n\r\n" + "< 대체약 정보 >";

                    while(reader.Read())
                    {
                        //rtnVal = rtnVal + ComNum.VBLF + " >> " + reader.GetValue(0).ToString().Trim() + " [" + READ_Suga_NameK(pDbCon, reader.GetValue(0).ToString().Trim()) + "]";
                        rtnVal = rtnVal + ComNum.VBLF + " >> " + reader.GetValue(0).ToString().Trim() + " [" + reader.GetValue(1).ToString().Trim() + "]";
                    }
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_ASA_Suga
        /// ASA 수가
        /// </summary>
        /// <returns></returns>
        public static string Read_ASA_Suga(PsmhDb pDbCon, string strGun, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE ";

                if (strGun == "01")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE Gubun ='마취_신체등급(ASA)_수가코드' ";
                }
                else if (strGun == "02")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE Gubun ='마취_신체등급(ASA)_약수가코드' ";
                }

                SQL = SQL + ComNum.VBLF + " AND TRIM(Code) ='" + strSuCode + "' ";
                SQL = SQL + ComNum.VBLF + " AND (DelDate IS NULL OR DelDate ='') ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("함수명 : " + "Read_ASA_Suga" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "Read_ASA_Suga" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("함수명 : " + "Read_ASA_Suga" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "Read_ASA_Suga" + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_MR_SONO_Suga
        /// MR SONO 수가
        /// </summary>
        /// <returns></returns>
        public static string Read_MR_SONO_Suga(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                //대체테이블 체크
                SQL = "";
                SQL = "SELECT ROWID FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE Gubun ='OCS_류마티스전송_SONO' ";
                SQL = SQL + ComNum.VBLF + " AND TRIM(Code) ='" + strSuCode + "' ";
                SQL = SQL + ComNum.VBLF + " AND (DelDate IS NULL OR DelDate ='') ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("함수명 : " + "Read_MR_SONO_Suga" + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "Read_MR_SONO_Suga" + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_Insulin_2015_Suga_Chk
        /// 인슐린 PRN 수가체크
        /// </summary>
        /// <returns></returns>
        public static string Read_Insulin_2015_Suga_Chk(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT ROWID FROM KOSMOS_ADM.DRUG_MASTER4  ";
                SQL = SQL + ComNum.VBLF + " WHERE MAXQTY_INSULIN ='1' ";
                SQL = SQL + ComNum.VBLF + "  AND TRIM(JepCode) ='" + strSuCode + "' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_PRN_Insulin_2015_Suga_Chk
        /// 인슐린 PRN 수가체크
        /// </summary>
        /// <returns></returns>
        public static string Read_PRN_Insulin_2015_Suga_Chk(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                //기본테이블 체크
                SQL = "";
                SQL = "SELECT b.ROWID FROM KOSMOS_ADM.DRUG_MASTER3 a, KOSMOS_ADM.DRUG_MASTER4 b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.JepCode=b.JepCode";
                SQL = SQL + ComNum.VBLF + "  AND a.PRN='1' ";
                SQL = SQL + ComNum.VBLF + "  AND b.MAXQTY_INSULIN ='1' ";
                SQL = SQL + ComNum.VBLF + "  AND TRIM(b.JepCode) ='" + strSuCode + "' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_Insulin_Manual_2015_Suga_Chk
        /// 인슐린 메뉴얼 수가체크
        /// </summary>
        /// <returns></returns>
        public static string Read_Insulin_Manual_2015_Suga_Chk(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                //기본테이블 체크
                SQL = "";
                SQL = "SELECT ROWID FROM KOSMOS_ADM.DRUG_MASTER4  ";
                SQL = SQL + ComNum.VBLF + " WHERE MAXQTY_INSULIN ='1' ";
                SQL = SQL + ComNum.VBLF + "  AND Insulin_scale ='2' ";
                SQL = SQL + ComNum.VBLF + "  AND TRIM(JepCode) ='" + strSuCode + "' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_PRN_ORDER_CHK
        /// </summary>
        /// <returns></returns>
        public static string CHK_PRN_ORDER_CHK(PsmhDb pDbCon, string strJob, string strPtno, FarPoint.Win.Spread.FpSpread ssSpread, string strBDate,
            int intSuCol, int intPCol, int intRemarkCol, int intRowS, int intDosCol, int intEDateCol, int intPRN_MaxCol, int intPRN_NotifyCol)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            OracleDataReader reader = null;
            string rtnVal = "OK";

            int i = 0;
            int intStart = 0;

            string strSuCode = "";

            string strInsulin = "";
            string strTemp = "";

            string strChk1 = "";
            string strChk2 = "";

            double dbl1Time2 = 0;
            double dbl24Time2 = 0;

            double dblMax = 0;

            if (strJob == "의사")
            {
                intStart = intRowS;
            }
            else
            {
                intStart = 0;
            }

            try
            {
                for (i = intStart; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text.Trim() != "True")
                    {
                        strSuCode = ssSpread.ActiveSheet.Cells[i, intSuCol].Text.Trim();

                        if (strSuCode != "")
                        {
                            if (strJob == "의사")
                            {
                                //if (ssSpread.ActiveSheet.Cells[i, intPCol].Text.Trim() == "P")
                                //2020-08-14 BST PRN일 경우 안타도록.. 
                                if (ssSpread.ActiveSheet.Cells[i, intPCol].Text.Trim() == "P"
                                    && strSuCode != "C3710" )
                                {
                                    SQL = "";
                                    SQL = "SELECT d.MAXQTY_1TIME_CONTENTS, d.MAXQTY_1TIME_UNIT, d.MAXQTY_1TIME_QTY, d.MAXQTY_1DAY_CONTENTS, d.MAXQTY_1DAY_UNIT,";
                                    SQL = SQL + ComNum.VBLF + "    d.MAXQTY_1DAY_QTY, d.MAXQTY_CNT, d.MAXQTY_GUBUN1, d.MAXQTY_GUBUN2,d.MaxQty_Required  ";
                                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.DRUG_MASTER3 c, KOSMOS_ADM.DRUG_MASTER4 d ";
                                    SQL = SQL + ComNum.VBLF + " WHERE  c.JEPCODE = d.JEPCODE ";
                                    SQL = SQL + ComNum.VBLF + "  AND c.PRN ='1' ";
                                    SQL = SQL + ComNum.VBLF + "  AND TRIM(c.JepCode)  ='" + strSuCode + "' ";

                                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }

                                    if (reader.HasRows == false)
                                    {
                                        rtnVal = "";
                                        ComFunc.MsgBox(strSuCode + " 이 수가는 PRN에 등록된 수가가 아닙니다.." + ComNum.VBLF + "PRN판넬에서 선택후 처방하십시오!!", "PRN오더확인");

                                        reader.Dispose();
                                        reader = null;
                                        Cursor.Current = Cursors.Default;

                                        return rtnVal;
                                    }

                                    reader.Dispose();
                                    reader = null;

                                    //PRN remark 체크
                                    if (ssSpread.ActiveSheet.Cells[i, intRemarkCol].Text.Trim() == "")
                                    {
                                        rtnVal = "";
                                        ComFunc.MsgBox(strSuCode + " 이 수가는 PRN 투여기준이 없습니다.. " + ComNum.VBLF + "PRN판넬에서 선택후 처방하십시오!!", "PRN오더확인");
                                        return rtnVal;
                                    }

                                    if (ssSpread.ActiveSheet.Cells[i, intEDateCol].Text.Trim() != "")
                                    {
                                        if (Read_Insulin_2015_Suga_Chk(pDbCon, strSuCode) == "OK")
                                        {
                                            if (Convert.ToDateTime(ssSpread.ActiveSheet.Cells[i, intEDateCol].Text.Trim()) < Convert.ToDateTime(strBDate))
                                            {
                                                rtnVal = "";
                                                ComFunc.MsgBox(strSuCode + " 이 수가는 PRN 투여 종료일자가 지났습니다..!!" + ComNum.VBLF + "PRN판넬에서 선택후 처방하십시오!!", "PRN오더확인");
                                                return rtnVal;
                                            }
                                        }
                                    }

                                    //PRN시 체크 추가
                                    if (clsPublic.GstrPRNChk_new == "OK")
                                    {
                                        if (Read_Insulin_2015_Suga_Chk(pDbCon, strSuCode) != "OK")
                                        {
                                            //일반 PRN
                                            if (ssSpread.ActiveSheet.Cells[i, intPRN_NotifyCol].Text.Trim() == "")
                                            {
                                                rtnVal = "";
                                                ComFunc.MsgBox(strSuCode + " PRN 상세설정이 누락된 처방입니다.. 다시처방하십시오 !!", "PRN오더확인");

                                                return rtnVal;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //PRN 용법일경우
                                    if (VB.I(VB.LCase(ssSpread.ActiveSheet.Cells[i, intDosCol].Text.Trim()), "PRN") > 1)
                                    {
                                        //약제과 요청 - 퇴원약일경우 PRN용법허용 2015-10-21
                                        if (ssSpread.ActiveSheet.Cells[i, intPCol].Text.Trim() != "T")
                                        {
                                            //인슐린판넬사용시 허용함
                                            if (Read_Insulin_Manual_2015_Suga_Chk(pDbCon, strSuCode) != "OK")
                                            {
                                                rtnVal = "";
                                                ComFunc.MsgBox(strSuCode + " 이 수가는 PRN 용법으로는 PRN처방 할 수 없습니다.. " + ComNum.VBLF + "PRN판넬에서 선택후 처방하십시오!!", "PRN오더확인");

                                                return rtnVal;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //간호사 PRN 풀때
                                if (clsPublic.Gstr간호처방STS == "PRN")
                                {
                                    strSuCode = ssSpread.ActiveSheet.Cells[i, intSuCol].Text.Trim();

                                    strInsulin = "";

                                    if (clsPublic.Gstr인슐린Chk == "OK")
                                    {
                                        if (Read_Insulin_2015_Suga_Chk(pDbCon, strSuCode) == "OK")
                                        {
                                            strInsulin = "OK";

                                            //종료일 설정시
                                            if (ssSpread.ActiveSheet.Cells[i, intEDateCol].Text.Trim() != "")
                                            {
                                                if (Convert.ToDateTime(strBDate) > Convert.ToDateTime(ssSpread.ActiveSheet.Cells[i, intEDateCol].Text.Trim()))
                                                {
                                                    rtnVal = "";
                                                    ComFunc.MsgBox(strSuCode + " Insulin 수가는  종료일 " + ssSpread.ActiveSheet.Cells[i, intEDateCol].Text.Trim() + "까지 입니다.." + ComNum.VBLF + "처방전송실패!! 의사창에서 다시 PRN 처방하십시오!!", "PRN전송불가");

                                                    return rtnVal;
                                                }
                                            }

                                            //용량체크 - 의사 PRN 용량확정
                                            strTemp = ssSpread.ActiveSheet.Cells[i, intEDateCol - 2].Text.Trim();       //의사설정 용량제한

                                            //scale 은 체크안함
                                            if (strTemp != "0" && strTemp != "")
                                            {
                                                if (ssSpread.ActiveSheet.Cells[i, intDosCol - 1].Text.Trim() != strTemp)
                                                {
                                                    rtnVal = "";
                                                    ComFunc.MsgBox(strSuCode + " Insulin 수가는 용량 ( " + strTemp + " ) 으로 설정 되어 있습니다..." + ComNum.VBLF + "처방전송실패!! 용량을 같이하여 전송하십시오!!", "PRN전송불가");

                                                    return rtnVal;
                                                }
                                            }

                                            //scale 선택에 따른 현재 용량 체크
                                            strTemp = ssSpread.ActiveSheet.Cells[i, intDosCol + 1].Text.Trim();

                                            switch (ssSpread.ActiveSheet.Cells[i, intEDateCol - 3].Text.Trim())
                                            {
                                                case "1":
                                                    switch (strTemp)
                                                    {
                                                        case "4":
                                                        case "8":
                                                        case "12":
                                                        case "16":
                                                            break;
                                                        default:
                                                            rtnVal = "";
                                                            ComFunc.MsgBox(strSuCode + " Insulin 수가는" + ComNum.VBLF + "[Normal Scale 용량 : 4, 8, 12, 16] 선택!!." + ComNum.VBLF + "처방전송실패!! 용량을 변경후 전송하십시오!!", "PRN전송불가");

                                                            return rtnVal;
                                                    }
                                                    break;
                                                case "2":
                                                    switch (strTemp)
                                                    {
                                                        case "2":
                                                        case "4":
                                                        case "6":
                                                        case "8":
                                                            break;
                                                        default:
                                                            rtnVal = "";
                                                            ComFunc.MsgBox(strSuCode + " Insulin 수가는" + ComNum.VBLF + "[Half Scale 용량 : 2, 4, 6, 8] 선택!!." + ComNum.VBLF + "처방전송실패!! 용량을 변경후 전송하십시오!!", "PRN전송불가");
                                                            break;
                                                    }
                                                    break;
                                            }
                                        }
                                    }


                                    //PRN수가 체크
                                    SQL = "   SELECT  d.MAXQTY_1DAY_QTY, d.MAXQTY_1TIME_QTY";
                                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.DRUG_MASTER3 c, KOSMOS_ADM.DRUG_MASTER4 d ";
                                    SQL = SQL + ComNum.VBLF + " WHERE  c.JEPCODE = d.JEPCODE ";

                                    if (strInsulin == "OK")
                                    {
                                        SQL = SQL + ComNum.VBLF + "  AND c.PRN ='1' ";
                                        SQL = SQL + ComNum.VBLF + "  AND MAXQTY_INSULIN = '1' ";
                                    }
                                    else
                                    {
                                        SQL = SQL + ComNum.VBLF + "  AND c.PRN ='1' ";
                                        SQL = SQL + ComNum.VBLF + "  AND (MAXQTY_INSULIN IS NULL OR MAXQTY_INSULIN <>'1') ";
                                    }

                                    SQL = SQL + ComNum.VBLF + "  AND TRIM(c.JepCode)  ='" + strSuCode + "' ";

                                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }

                                    if (reader.HasRows == false)
                                    {
                                        rtnVal = "";
                                        ComFunc.MsgBox(strSuCode + " 이 수가는 PRN에 등록된 수가가 아닙니다.." + ComNum.VBLF + "PRN판넬에서 선택후 처방하십시오!!", "PRN오더확인");

                                        reader.Dispose();
                                        reader = null;
                                        Cursor.Current = Cursors.Default;

                                        return rtnVal;
                                    }
                                    else
                                    {
                                        if (reader.Read())
                                        {
                                            //dbl24Time2 = VB.Val(dt.Rows[0]["MAXQTY_1DAY_QTY"].ToString().Trim());
                                            //strChk1 = dt.Rows[0]["MAXQTY_1TIME_QTY"].ToString().Trim();

                                            dbl24Time2 = VB.Val(reader.GetValue(0).ToString().Trim());
                                            strChk1 = reader.GetValue(1).ToString().Trim();
                                        }
                                    }

                                    reader.Dispose();
                                    reader = null;

                                    //PRN remark 체크
                                    if (ssSpread.ActiveSheet.Cells[i, intRemarkCol].Text.Trim() == "")
                                    {
                                        rtnVal = "";
                                        ComFunc.MsgBox(strSuCode + " 이 수가는 PRN 투여기준이 없습니다.. " + ComNum.VBLF + "오더를 다시 PRN판넬에서 선택후 처방하십시오!!", "PRN오더확인");

                                        return rtnVal;
                                    }
                                    else
                                    {
                                        //PRN 일투량 체크
                                        if (strInsulin != "OK")
                                        {
                                            strChk2 = ssSpread.ActiveSheet.Cells[i, intDosCol + 2].Text.Trim();
                                            dbl1Time2 = VB.Val(strChk2);

                                            if (VB.Val(strChk1) < VB.Val(strChk2))
                                            {
                                                rtnVal = "";
                                                ComFunc.MsgBox(strSuCode + " 이 수가는 PRN시 일투량 " + strChk1 + " 이상 처방 할 수 없습니다.." + ComNum.VBLF + "일투량 변경후 처방하십시오!!", "PRN오더확인");

                                                return rtnVal;
                                            }

                                            //2015-11-12 1일최대횟수
                                            dblMax = VB.Val(ssSpread.ActiveSheet.Cells[i, intPRN_MaxCol].Text.Trim());

                                            SQL = "";
                                            SQL = "SELECT Qty";
                                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER ";
                                            SQL = SQL + ComNum.VBLF + "  WHERE PTNO ='" + strPtno + "' ";
                                            SQL = SQL + ComNum.VBLF + "  AND BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                                            SQL = SQL + ComNum.VBLF + "  AND SUCODE ='" + strSuCode + "' ";
                                            SQL = SQL + ComNum.VBLF + "  AND NURSEID <> ' '  ";     // PRN 처방 조건
                                            SQL = SQL + ComNum.VBLF + "  AND GBACT ='*' ";      // PRN 처방 조건
                                            SQL = SQL + ComNum.VBLF + "  AND (GBPRN IS NULL OR GBPRN <> 'P') ";
                                            SQL = SQL + ComNum.VBLF + "  AND  gbstatus NOT IN ('D','D-') ";
                                            SQL = SQL + ComNum.VBLF + "  AND ORDERSITE NOT IN('NDC', 'CAN') ";

                                            //2021-02-22 안정수 추가
                                            if (intSuCol == 17
                                                && ssSpread.ActiveSheet.Cells[i, 10].Text.Trim().Length == 1 
                                                && ssSpread.ActiveSheet.Cells[i, 10].Text.Trim() != "")
                                            {
                                                SQL = SQL + ComNum.VBLF + "  AND GBGROUP = '" + ssSpread.ActiveSheet.Cells[i, 10].Text.Trim() + "'";
                                            }

                                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon); 

                                            if (SqlErr != "")
                                            {
                                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                                Cursor.Current = Cursors.Default;

                                                return rtnVal;
                                            }
                                            if (dt.Rows.Count > 0)
                                            {
                                                //2015-11-12
                                                if (dblMax < (dt.Rows.Count + 1))
                                                {
                                                    rtnVal = "";
                                                    ComFunc.MsgBox("[" + strSuCode + "] >>  PRN 최대일투량을 사용하였습니다.." + ComNum.VBLF + "최대일투량 : " + dblMax, "처방불가");
                                                }

                                                dt.Dispose();
                                                dt = null;
                                                Cursor.Current = Cursors.Default;

                                                return rtnVal;
                                            }

                                            dt.Dispose();
                                            dt = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_POWDER_ORDER_CHK
        /// MTSIORDER / EORDER
        /// TODO : vb60_new\ocs\ipdocs\iorder\vb_powder.bas : READ_POWDER_SuCode_NEW2 / READ_POWDER_SuCode_NEW3
        /// </summary>
        /// <returns></returns>
        public static string CHK_POWDER_ORDER_CHK(string strJob, string strPtNo, FarPoint.Win.Spread.FpSpread ssSpread, string strBDate,
            int intSuCol, int intPCol, int intRemarkCol, int intRowS, int intDosCol, int intEDateCol, int intBunCol, int intPWSayuCol)
        {
            string rtnVal = "OK";

            int i = 0;
            int intStart = 0;

            string strSuCode = "";

            string strTemp = "";

            int intPWCol = 0;

            if (strJob == "의사")
            {
                intStart = intRowS;
            }
            else
            {
                intStart = 0;
            }

            if (clsOrdFunction.GstrGbJob == "IPD")
            {
                intPWCol = (int)BaseOrderInfo.IpdOrderCol.POWDER;
            }
            else if (clsOrdFunction.GstrGbJob == "ER")
            {
                return rtnVal;
            }

            //파우더 대상이 아닌경우
            if (clsPublic.Gstr파우더New_STS != "Y")
            {
                for (i = intStart; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text.Trim() != "True")
                    {
                        strSuCode = ssSpread.ActiveSheet.Cells[i, intSuCol].Text.Trim();

                        if (clsOrdFunction.GstrGbJob == "IPD")
                        {
                            if (ssSpread.ActiveSheet.Cells[i, intPWCol].Text.Trim() == "1")
                            {
                                if (ssSpread.ActiveSheet.Cells[i, intBunCol].Text.Trim() != "11")
                                {
                                    ComFunc.MsgBox(strSuCode + " 수가는 POWDER 체크는 경구약만 가능합니다..", "확인");
                                    rtnVal = "";

                                    return rtnVal;
                                }
                                else
                                {
                                    ComFunc.MsgBox(strSuCode + " 수가코드" + ComNum.VBLF + "POWDER 대상으로 설정되지 않았는데"
                                        + ComNum.VBLF + "수가에 POWDER 선택이 되어있습니다.."
                                        + ComNum.VBLF + "1.파우더 체크 해지후 전달하거나,"
                                        + ComNum.VBLF + "2.POWDER 대상으로 설정후 전송하십시오!!", "확인");
                                    rtnVal = "";

                                    return rtnVal;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //파우더 대상인경우
                for (i = intStart; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        strSuCode = ssSpread.ActiveSheet.Cells[i, intSuCol].Text.Trim();

                        if (clsOrdFunction.GstrGbJob == "IPD")
                        {
                            if (ssSpread.ActiveSheet.Cells[i, intPWCol].Text.Trim() == "1")
                            {
                                if (ssSpread.ActiveSheet.Cells[i, intBunCol].Text.Trim() != "11")
                                {
                                    ComFunc.MsgBox(strSuCode + " 수가는 POWDER 체크는 경구약만 가능합니다..", "확인");
                                    rtnVal = "";

                                    return rtnVal;
                                }
                            }
                        }

                        //2019-08-08 임시로 막음
                        if (CHK_POWDER_SUCODE_CHK_2(clsDB.DbCon, strSuCode) == "OK")
                        {
                            //산제불가수가

                            //산제체크 한것중
                            strTemp = CHK_POWDER_SUCODE_CHK_3(clsDB.DbCon, strSuCode);

                            //if (ssSpread.ActiveSheet.Cells[i, intPWCol].Text != "True")
                            if (ssSpread.ActiveSheet.Cells[i, intPWCol].Text == "True")
                            {
                                if (ssSpread.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.POWDERSAYU].Text != "")
                                {
                                    continue;
                                }

                                //산제체크 여부
                                if (strTemp == "1")
                                {                                    
                                    ComFunc.MsgBox("POWDER 대상 환자입니다.." + ComNum.VBLF + "[" + strSuCode + " ]  산제불가 수가코드" + ComNum.VBLF + "이대로 전송하면 산제[POWDER]로 처방안됩니다.." + ComNum.VBLF + "해당수가 삭제후 산제불가 사유를 넣고 전송 하십시오!!", "수가확인");                                    
                                }
                                else if (strTemp == "2")
                                {
                                    ComFunc.MsgBox("POWDER 대상 환자입니다.." + ComNum.VBLF + "[" + strSuCode + " ]  산제불가 수가코드" + ComNum.VBLF + "이대로 전송하면 산제[POWDER]로 처방안됩니다.." + ComNum.VBLF + "해당수가 삭제후 산제불가 대체약을 선택후 전송 하십시오!!", "수가확인");
                                }

                                rtnVal = "";
                                return rtnVal;
                            }
                        }
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_NotDIV_ORDER_CHK
        /// </summary>
        /// <returns></returns>
        public static string CHK_NotDIV_ORDER_CHK(PsmhDb pDbCon, string strIO, string strPtNo, string ArgSuCode, string strBun, string strDept, FarPoint.Win.Spread.FpSpread ssSpread, int intMRow, int intSelfCol, int intBunCol, int intSugaCol, int intQtyCol, int intDivCol)
        {
            string rtnVal = "OK";

            int i = 0;
            int intDiv = 0;
            int intStart = 0;

            string strSuCode = "";

            double dblQty = 0;
            double dblDiv = 0;

            //string strTemp = "";

            string strChk1 = "";
            //string strChk2 = "";

            if (intMRow == 0)
            {
                intStart = 0;
            }
            else
            {
                intStart = intMRow;
            }

            try
            {
                for (i = intStart; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        strSuCode = ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim();

                        if ((ssSpread.ActiveSheet.Cells[i, intBunCol].Text.Trim() != "11"
                            || ssSpread.ActiveSheet.Cells[i, intBunCol].Text.Trim() != "12"
                            || ssSpread.ActiveSheet.Cells[i, intBunCol].Text.Trim() != "20")
                            && VB.Left(strSuCode, 2) != "@V")
                        {
                            strSuCode = ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim();

                            switch (ssSpread.ActiveSheet.Cells[i, intQtyCol].Text.Trim())
                            {
                                case "1/2": dblQty = 0.5; break;
                                case "1/3": dblQty = 0.33; break;
                                case "2/3": dblQty = 0.66; break;
                                case "1/4": dblQty = 0.25; break;
                                case "3/4": dblQty = 0.75; break;
                                case "1/5": dblQty = 0.2; break;
                                case "2/5": dblQty = 0.4; break;
                                case "3/5": dblQty = 0.6; break;
                                case "4/5": dblQty = 0.8; break;
                                default: dblQty = VB.Val(ssSpread.ActiveSheet.Cells[i, intQtyCol].Text.Trim()); break;
                            }

                            intDiv = Convert.ToInt32(VB.Val(ssSpread.ActiveSheet.Cells[i, intDivCol].Text.Trim()));

                            dblDiv = dblDiv == 0 ? 1 : dblDiv;

                            //2015-11-04                            
                            //if ((dblQty / dblDiv) != VB.Fix(Convert.ToInt32(dblQty / dblDiv)))
                            //2020-06-13 안정수, 조건 추가, 분할불가약제에 대해서 (일투량 / DIV) 체크하여 1보다 작을경우 막히도록
                            if ((dblQty / dblDiv) != VB.Fix(Convert.ToInt32(dblQty / dblDiv)) || (dblQty / intDiv) < 1 )
                            {
                                if (Read_NotDiv_2015_Suga_Chk(pDbCon, strSuCode) == "OK")
                                {
                                    strChk1 = Read_NotDiv_2015_Suga_Replace(pDbCon, strSuCode);

                                    if (strChk1 != "")
                                    {
                                        ComFunc.MsgBox(strChk1, "처방확인");

                                        //2015-10-27 약제 과장 요청
                                        if (strIO == "외래")
                                        {
                                            switch (strDept)
                                            {
                                                case "NP":
                                                case "ME":
                                                case "MN":
                                                case "MC":
                                                case "MG":
                                                case "MP":
                                                case "MR":
                                                case "MI":
                                                    break;
                                                default:
                                                    rtnVal = "분할불가";
                                                    return rtnVal;
                                            }
                                        }
                                        else
                                        {
                                            rtnVal = "분할불가";
                                            return rtnVal;
                                        }
                                    }
                                    else
                                    {
                                        ComFunc.MsgBox(strSuCode + " 수가는 분할불가 약입니다... 처방확인후 전송하십시오", "처방확인");

                                        //2015-10-27 약제 과장 요청
                                        if (strIO == "외래")
                                        {
                                            switch (strDept)
                                            {
                                                case "NP":
                                                case "ME":
                                                case "MN":
                                                case "MC":
                                                case "MG":
                                                case "MP":
                                                case "MR":
                                                case "MI":
                                                    break;
                                                default:
                                                    rtnVal = "분할불가";
                                                    return rtnVal;
                                            }
                                        }
                                        else
                                        {
                                            rtnVal = "분할불가";
                                            return rtnVal;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox("함수명 : " + "CHK_NotDIV_ORDER_CHK" + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "CHK_NotDIV_ORDER_CHK" + ComNum.VBLF + ex.Message, "", pDbCon); //에러로그 저장
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_ASA_ORDER_CHK
        /// </summary>
        /// <returns></returns>
        public static string CHK_ASA_ORDER_CHK(PsmhDb pDbCon, string strIO, string strPtNo, string strDept, FarPoint.Win.Spread.FpSpread ssSpread, int intMRow, int intSugaCol)
        {
            string rtnVal = "OK";

            int i = 0;
            int intStart = 0;
            int intCol = 0;

            string strOrderNo = "";
            int nOrderNoCol = 0;

            string strTemp = "";

            if (intMRow == 0)
            {
                intStart = 0;
            }
            else
            {
                intStart = intMRow;
            }

            if (strIO == "외래")
            {
                //intCol = 53;
                //nOrderNoCol = 27;

                //2021-01-12 수정
                intCol = 58;
                nOrderNoCol = 32;
            }
            else if (strIO == "입원")
            {
                //intCol = 73;    //입원, ER
                //nOrderNoCol = 29;

                //2021-01-12 수정
                intCol = 76;    //입원, ER
                nOrderNoCol = 32;
            }
            else if (strIO == "응급")
            {
                intCol = 73;    //입원, ER
                nOrderNoCol = 28;
            }

            for (i = intStart; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    strOrderNo = ssSpread.ActiveSheet.Cells[i, nOrderNoCol].Text.Trim();

                    strTemp = ssSpread.ActiveSheet.Cells[i, intCol].Text.Trim();

                    if (strOrderNo.Trim() == "" || strOrderNo.Trim() == "0")
                    {
                        if (strTemp == "")
                        {
                            if (ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim() != "")
                            {
                                if (Read_ASA_Suga(pDbCon, "01", ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim()) == "OK")
                                {
                                    ComFunc.MsgBox("[" + ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim() + "] 수가코드는 진정(ASA) 처방 수가대상입니다.." + ComNum.VBLF + "진정등급(ASA)을 입력후 처방전송하십시오!!" + ComNum.VBLF + "(오더화면 ASA더블클릭후 입력)", "확인");
                                    rtnVal = "";

                                    clsPublic.GnASA진정Row = i;
                                    return rtnVal;
                                }
                            }
                        }
                    }
                }
            }

            for (i = intStart; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                {   
                    strOrderNo = ssSpread.ActiveSheet.Cells[i, nOrderNoCol].Text.Trim();

                    strTemp = ssSpread.ActiveSheet.Cells[i, intCol].Text.Trim();

                    if (strOrderNo.Trim() == "" || strOrderNo.Trim() == "0")
                    {
                        if (strTemp == "")
                        {
                            if (ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim() != "")
                            {
                                if (Read_ASA_Suga(pDbCon, "02", ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim()) == "OK")
                                {
                                    if (ComFunc.MsgBoxQ("[" + ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim() + "] 수가코드는 진정검사 대상약물입니다."
                                        + ComNum.VBLF + "진정검사용으로 사용할경우 ASA기준 평가후 전송하십시오!!"
                                        + ComNum.VBLF + "진정검사약물(CT,MRI 등검사)로 사용 하시겠습니까??"
                                        + ComNum.VBLF + "(오더화면 ASA더블클릭후 입력)",
                                        "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        rtnVal = "";
                                        clsPublic.GnASA진정Row = i;

                                        return rtnVal;
                                    }
                                    else
                                    {
                                        return rtnVal;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_RA_SONO_ORDER_CHK
        /// </summary>
        /// <returns></returns>
        public static string CHK_RA_SONO_ORDER_CHK(PsmhDb pDbCon, string strIO, string strPtNo, string strDept, FarPoint.Win.Spread.FpSpread ssSpread, int intMRow, int intSugaCol)
        {
            string rtnVal = "OK";

            int i = 0;

            int intStart = 0;

            if (intMRow == 0)
            {
                intStart = 0;
            }
            else
            {
                intStart = intMRow;
            }

            //류마티스는 점검 안함
            if (strDept == "MR")
            {
                return rtnVal;
            }

            for (i = intStart; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    if (ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim() != "")
                    {
                        if (Read_MR_SONO_Suga(pDbCon, ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim()) == "OK")
                        {
                            ComFunc.MsgBox("[" + ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim() + "] 수가코드는 류마티스내과 전용 수가입니다.." + ComNum.VBLF + "처방확인후 전송하십시오!!", "확인");
                            rtnVal = "";
                            return rtnVal;
                        }
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_MNHD_SUGA_ORDER_CHK
        /// 2016-01-27 HD,MN 과 만 처방가능 + 본인 100%
        /// </summary>
        /// <returns></returns>
        public static string CHK_MNHD_SUGA_ORDER_CHK(string strIO, string strPtNo, string strDept, FarPoint.Win.Spread.FpSpread ssSpread, int intMRow, int intSugaCol, int intSelfCol)
        {
            string rtnVal = "OK";

            int i = 0;
            int intStart = 0;

            string strSuCode = "";

            string strV193 = "";

            if (intMRow == 0)
            {
                intStart = 0;
            }
            else
            {
                intStart = intMRow;
            }

            for (i = intStart; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim() == "@V193")
                {
                    strV193 = "OK";
                    break;
                }
            }

            if (strIO == "외래")
            {
                if (strV193 != "OK")
                {
                    for (i = intStart; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
                    {
                        if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                        {
                            strSuCode = ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim();

                            //2017-03-30
                            if (strSuCode == "BOME10-2" || strSuCode == "BOME5-F2")
                            {
                                if (strDept == "MN" || strDept == "HD")
                                {
                                    if (ssSpread.ActiveSheet.Cells[i, intSelfCol].Text.Trim() != "2")
                                    {
                                        ComFunc.MsgBox("[" + strSuCode + "] 수가코드는 신장내과,인공신장실 전용 수가로 보험 100% [S항 2] .." + ComNum.VBLF + "처방확인후 전송하십시오!!", "확인");
                                        rtnVal = "";

                                        return rtnVal;
                                    }
                                }
                                else
                                {
                                    ComFunc.MsgBox("[" + strSuCode + "] 수가코드는 신장내과,인공신장실에서 가능한 수가입니다.." + ComNum.VBLF + "처방확인후 전송하십시오!!", "확인");
                                    rtnVal = "";

                                    return rtnVal;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (i = intStart; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        strSuCode = ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim();

                        if (strPtNo == "09736880")
                        {
                            //2016-02-29 강제제외요청
                        }
                        else
                        {
                            if (strSuCode == "MEGAS10" || strSuCode == "MEGAS-F")
                            {
                                if (strDept == "MN" || strDept == "HD")
                                {
                                    if (ssSpread.ActiveSheet.Cells[i, intSelfCol].Text.Trim() != "2")
                                    {
                                        ComFunc.MsgBox("[" + strSuCode + "] 수가코드는 신장내과,인공신장실 전용 수가로 보험 100% [S항 2] .." + ComNum.VBLF + "처방확인후 전송하십시오!!", "확인");
                                        rtnVal = "";

                                        return rtnVal;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_BLOOD_RDate_ORDER_CHK
        /// </summary>
        /// <returns></returns>
        public static string CHK_BLOOD_RDate_ORDER_CHK(string strIO, string strPtNo, string strDept, FarPoint.Win.Spread.FpSpread ssSpread, int intMRow, int intSugaCol, int intBunCol, int intRemarkCol)
        {
            string rtnVal = "OK";

            int i = 0;
            int intStart = 0;

            string strOrderNo = "";
            int nOrderNoCol = 0;

            clsPublic.Gn혈액사용예정일Row = 0;
            clsPublic.Gstr혈액사용예정일Date = "";

            if (intMRow == 0)
            {
                intStart = 0;
            }
            else
            {
                intStart = intMRow;
            }

            if (strIO == "외래")
            {
                //2021-01-12 변경
                //nOrderNoCol = 27;
                nOrderNoCol = 32;
            }
            else if (strIO == "입원")
            {
                //2021-01-12 변경
                //nOrderNoCol = 29;
                nOrderNoCol = 32;
            }
            else if (strIO == "응급")
            {
                nOrderNoCol = 28;
            }

            for (i = intStart; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    strOrderNo = ssSpread.ActiveSheet.Cells[i, nOrderNoCol].Text.Trim();
                    if (strOrderNo.Trim() == "" || strOrderNo.Trim() == "0")
                    {
                        if (ssSpread.ActiveSheet.Cells[i, intBunCol].Text.Trim() == "37")
                        {
                            if (VB.I(ssSpread.ActiveSheet.Cells[i, intRemarkCol].Text.Trim(), "[혈액사용예정일:") <= 1)
                            {
                                ComFunc.MsgBox("[" + ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim() + "] 혈액사용예정일을 입력하십시오..", "혈액수가확인");
                                rtnVal = "";
                                clsPublic.Gn혈액사용예정일Row = i;

                                return rtnVal;
                            }
                        }
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_CANCER_ORDER_CHK
        /// 암,희귀난치 등록후 코드 체크
        /// </summary>
        /// <returns></returns>
        public static string CHK_CANCER_ORDER_CHK(PsmhDb pDbCon, string strIO, string strPtNo, string strDept, FarPoint.Win.Spread.FpSpread ssSpread, int intMRow, int intSugaCol)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;

            string rtnVal = "OK";

            int i = 0;
            int intStart = 0;

            string strGbn = "";
            string strTemp = "";
            string strChk1 = "";

            if (intMRow == 0)
            {
                intStart = 0;
            }
            else
            {
                intStart = intMRow;
            }

            try
            {
                SQL = "";
                SQL = "SELECT GUBUN FROM KOSMOS_PMPA.BAS_CANCER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND TRUNC(IDATE) =TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "  AND Dept1 ='" + strDept + "' ";
                SQL = SQL + ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='' )";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    strChk1 = "OK";

                    if (reader.GetValue(0).To<string>().Trim().Equals("1"))
                    {
                        strGbn = "암등록";
                    }
                    else
                    {
                        strGbn = "희귀난치등록";
                    }
                }

                reader.Dispose();
                reader = null;

                //당일 신청서 있는것만
                if (strChk1 == "OK")
                {
                    for (i = intStart; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
                    {
                        if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                        {
                            if (VB.Left(ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim(), 2) == "@V")
                            {
                                strTemp = "OK";
                                break;
                            }
                        }
                    }

                    if (strTemp != "OK")
                    {
                        ComFunc.MsgBox("당일 " + strGbn + " 신청됨!! @V 코드를 확인하세요!!", "확인");
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 비급여 하복부 초음파 검사 관련 코드 조회
        /// 2019-02-01 박웅규
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strIO"></param>
        /// <param name="strPtNo"></param>
        /// <param name="strDept"></param>
        /// <param name="ssSpread"></param>
        /// <param name="intMRow"></param>
        /// <param name="intSugaCol"></param>
        /// <returns></returns>
        public static string CHK_LOWER_SONO_ORDER_CHK(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread ssSpread, int intMRow, int intSugaCol)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;

            string rtnVal = "OK";
            bool isCode = false;

            try
            {
                //산재불가약 체크후 -체크박스 표시
                SQL = "";
                SQL += " SELECT CODE                                                                                                  \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_BCODE                                                                                 \r";
                SQL += "  WHERE GUBUN = 'OCS_하복부비급여'                                                                               \r";
                SQL += "    AND CODE = '" + ssSpread.ActiveSheet.Cells[intMRow, intSugaCol].Text.Trim() +"'                            \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("함수명 : " + "CHK_LOWER_SONO_ORDER_CHK" + ComNum.VBLF + SqlErr);
                    clsDB.SaveSqlErrLog("함수명 : " + "CHK_LOWER_SONO_ORDER_CHK" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    isCode = true;
                }
                reader.Dispose();
                reader = null;

                if (isCode == true)
                {
                    ComFunc.MsgBox("비급여 하복부 초음파 검사시 설명후 동의서를 받아 주세요.");
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox("함수명 : " + "CHK_LOWER_SONO_ORDER_CHK" + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "CHK_LOWER_SONO_ORDER_CHK" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_Verb_Doctror_ORDER_Chk
        /// </summary>
        /// <returns></returns>
        public static string READ_Verb_Doctror_ORDER_Chk(PsmhDb pDbCon, string strIO, string strDpet, string strPtNo, string strInDate, string strDrCode, string strSabun)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL = "SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER      ";
                SQL = SQL + ComNum.VBLF + "WHERE Ptno     = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GbStatus IN (' ','D','D+')  ";
                SQL = SQL + ComNum.VBLF + "  AND BDate >= TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND BDate <= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND NurseID <> ' ' ";      // 간호사처방만
                SQL = SQL + ComNum.VBLF + "  AND GbVerb ='Y' ";     // 구두처방대상건만
                SQL = SQL + ComNum.VBLF + "  AND Bun IN ('11','12','20') ";
                SQL = SQL + ComNum.VBLF + "  AND DRORDERVIEW IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND (GbSend  = ' ' OR GbSend IS NULL) ";       // 전송된것
                SQL = SQL + ComNum.VBLF + "  AND OrderSite Not Like 'DC%' ";
                SQL = SQL + ComNum.VBLF + "  AND OrderSite <>  'CAN' ";

                if (strIO == "응급")
                {
                    SQL = SQL + ComNum.VBLF + "  AND DrCode ='" + strSabun + "'     ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY BDate, Seqno ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_Verb_Doctror_ORDER_Chk2
        /// MTSOORDER
        /// </summary>
        /// <returns></returns>
        public static string READ_Verb_Doctror_ORDER_Chk2(PsmhDb pDbCon, string strDrCode, string strDrCode1, string strDrCode2)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "0";

            try
            {
                SQL = "";
                SQL = " SELECT a.PTNO Ptno ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER a ";
                SQL = SQL + ComNum.VBLF + "WHERE Ptno IN ( ";
                SQL = SQL + ComNum.VBLF + " SELECT PANO ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                //2016-05-16 김현욱 추가함 ========================================
                SQL = SQL + ComNum.VBLF + "  WHERE JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND GBSTS NOT IN ('9','7') ";
                //=================================================================
                SQL = SQL + ComNum.VBLF + " )";
                SQL = SQL + ComNum.VBLF + "  AND a.GbStatus IN (' ','D','D+')  ";

                //2016-05-16 김현욱 추가함(조회기간 15일에서 5일로) ========================================
                SQL = SQL + ComNum.VBLF + "  AND a.BDate >= TO_DATE('" + Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-5).ToShortDateString() + "','YYYY-MM-DD') ";
                //=================================================================

                SQL = SQL + ComNum.VBLF + "  AND a.BDate <= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.NurseID <> ' ' "; //간호사처방만
                SQL = SQL + ComNum.VBLF + "  AND a.GbVerb ='Y' ";    //구두처방대상건만
                SQL = SQL + ComNum.VBLF + "  AND a.Bun IN ('11','12','20') ";
                SQL = SQL + ComNum.VBLF + "  AND a.DRORDERVIEW IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND (a.GbSend  = ' ' OR a.GbSend IS NULL) ";  //전송된것
                SQL = SQL + ComNum.VBLF + "  AND a.OrderSite Not Like 'DC%' ";
                SQL = SQL + ComNum.VBLF + "  AND a.OrderSite <>  'CAN' ";
                SQL = SQL + ComNum.VBLF + "  AND a.staffid ='" + strDrCode + "' ";  //의사코드 제한
                SQL = SQL + ComNum.VBLF + " GROUP BY a.PTNO ";
                SQL = SQL + ComNum.VBLF + " UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT a.PANO Ptno";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_MASTER a ";
                SQL = SQL + ComNum.VBLF + "  WHERE BDATE >=TO_DATE('" + Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-7).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE <=TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='ER' ";
                SQL = SQL + ComNum.VBLF + "   AND Pano IN (  ";
                SQL = SQL + ComNum.VBLF + "               SELECT a.PTNO ";
                SQL = SQL + ComNum.VBLF + "                FROM KOSMOS_OCS.OCS_IORDER a ";
                SQL = SQL + ComNum.VBLF + "                 WHERE a.GbStatus IN (' ','D','D+')  ";
                SQL = SQL + ComNum.VBLF + "                  AND a.BDate >= TO_DATE('" + Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-7).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "                  AND a.BDate <= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "                  AND a.NurseID <> ' ' "; //간호사처방만
                SQL = SQL + ComNum.VBLF + "                  AND a.GbVerb ='Y' ";    //구두처방대상건만
                SQL = SQL + ComNum.VBLF + "                  AND a.Bun IN ('11','12','20') ";
                SQL = SQL + ComNum.VBLF + "                  AND a.DRORDERVIEW IS NULL ";
                SQL = SQL + ComNum.VBLF + "                  AND (a.GbSend  = ' ' OR a.GbSend IS NULL) ";       // 전송된것
                SQL = SQL + ComNum.VBLF + "                  AND a.OrderSite Not Like 'DC%' ";
                SQL = SQL + ComNum.VBLF + "                  AND a.OrderSite <>  'CAN' ";

                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    SQL = SQL + ComNum.VBLF + "                  AND a.StaffID ='" + strDrCode1 + "' ";     // 의사코드 제한
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                  AND a.StaffID ='" + strDrCode2 + "' ";     // 의사코드 제한
                }

                SQL = SQL + ComNum.VBLF + "                 ) ";
                SQL = SQL + ComNum.VBLF + "   GROUP BY  a.Pano   ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows.Count.ToString();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_Verb_Doctror_ORDER_Chk3
        /// </summary>
        /// <returns></returns>
        public static string READ_Verb_Doctror_ORDER_Chk3(PsmhDb pDbCon, string strDrCode, string strPtNo, string strInDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "0";

            try
            {
                SQL = "";
                SQL = "SELECT a.PTNO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER a ";
                SQL = SQL + ComNum.VBLF + "WHERE Ptno ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND a.GbStatus IN (' ','D','D+')  ";
                SQL = SQL + ComNum.VBLF + "  AND a.BDate >= TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.BDate <= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.NurseID <> ' ' "; //간호사처방만
                SQL = SQL + ComNum.VBLF + "  AND a.GbVerb ='Y' ";    //구두처방대상건만
                SQL = SQL + ComNum.VBLF + "  AND a.Bun IN ('11','12','20') ";
                SQL = SQL + ComNum.VBLF + "  AND a.DRORDERVIEW IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND (a.GbSend  = ' ' OR a.GbSend IS NULL) ";       // 전송된것
                SQL = SQL + ComNum.VBLF + "  AND a.OrderSite Not Like 'DC%' ";
                SQL = SQL + ComNum.VBLF + "  AND a.OrderSite <>  'CAN' ";
                SQL = SQL + ComNum.VBLF + "  AND a.StaffID ='" + strDrCode + "' ";      // 의사코드 제한
                SQL = SQL + ComNum.VBLF + " GROUP BY a.PTNO ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows.Count.ToString();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_PRN_INS_Doctror_ORDER_Chk
        /// </summary>
        /// <returns></returns>
        public static string READ_PRN_INS_Doctror_ORDER_Chk(PsmhDb pDbCon, string strIO, string strDept, string strPtNo, string strInDate, string strDrCode, string strSabun)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER      ";
                SQL = SQL + ComNum.VBLF + "WHERE Ptno     = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GbStatus IN (' ','D','D+')  ";
                SQL = SQL + ComNum.VBLF + "  AND BDate >= TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND BDate <= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND GbPRN =' ' ";
                SQL = SQL + ComNum.VBLF + "  AND GbAct ='*' ";  //간호사가 PRN푼것
                SQL = SQL + ComNum.VBLF + "  AND SuCode IN ( SELECT JEPCODE FROM KOSMOS_ADM.DRUG_MASTER4  WHERE Insulin_scale ='1'  ) ";
                SQL = SQL + ComNum.VBLF + "  AND (GbSend  = ' ' OR GbSend IS NULL) ";  //전송된것
                SQL = SQL + ComNum.VBLF + "  AND OrderSite Not Like 'DC%' ";
                SQL = SQL + ComNum.VBLF + "  AND OrderSite <>  'CAN' ";
                SQL = SQL + ComNum.VBLF + "  AND (PRN_INS_CDate IS NULL OR PRN_INS_CDate ='' )";

                if (strIO == "응급")
                {
                    SQL = SQL + ComNum.VBLF + "  AND DrCode ='" + strSabun + "'     ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY BDate, Seqno ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_PRN_INS_Doctror_ORDER_Chk2
        /// MTSOORDER
        /// </summary>
        /// <returns></returns>
        public static string READ_PRN_INS_Doctror_ORDER_Chk2(PsmhDb pDbCon, string strDrCode, string strDrCode1, string strDrCode2)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "0";

            try
            {
                #region 기존 쿼리 주석
                //SQL = "";
                //SQL = "SELECT a.PTNO Ptno ";
                //SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER a ";
                //SQL = SQL + ComNum.VBLF + "WHERE Ptno IN ( SELECT Pano FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                //SQL = SQL + ComNum.VBLF + "  WHERE JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND GBSTS NOT IN ('9','7') AND DRCODE ='" + strDrCode + "' )";
                //SQL = SQL + ComNum.VBLF + "  AND a.GbStatus IN (' ','D','D+')  ";
                //SQL = SQL + ComNum.VBLF + "  AND a.BDate >= TO_DATE('" + Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-5).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                //SQL = SQL + ComNum.VBLF + "  AND a.BDate <= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                //SQL = SQL + ComNum.VBLF + "  AND GbPRN =' ' ";
                //SQL = SQL + ComNum.VBLF + "  AND GbAct ='*' "; //간호사가 PRN푼것
                //SQL = SQL + ComNum.VBLF + "  AND SuCode IN ( SELECT JEPCODE FROM KOSMOS_ADM.DRUG_MASTER4  WHERE Insulin_scale ='1'  ) ";
                //SQL = SQL + ComNum.VBLF + "  AND (a.GbSend  = ' ' OR a.GbSend IS NULL) ";  //전송된것
                //SQL = SQL + ComNum.VBLF + "  AND a.OrderSite Not Like 'DC%' ";
                //SQL = SQL + ComNum.VBLF + "  AND a.OrderSite <>  'CAN' ";
                //SQL = SQL + ComNum.VBLF + "  AND (a.PRN_INS_CDate IS NULL OR a.PRN_INS_CDate ='' )";
                //SQL = SQL + ComNum.VBLF + "  AND a.StaffID ='" + strDrCode + "' ";  //의사코드 제한
                //SQL = SQL + ComNum.VBLF + " GROUP BY a.PTNO ";
                //SQL = SQL + ComNum.VBLF + " UNION ALL";
                //SQL = SQL + ComNum.VBLF + " SELECT a.PANO Ptno";
                //SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_MASTER a ";
                //SQL = SQL + ComNum.VBLF + "  WHERE BDATE >=TO_DATE('" + Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-7).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                //SQL = SQL + ComNum.VBLF + "   AND BDATE <=TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                //SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='ER' ";
                //SQL = SQL + ComNum.VBLF + "   AND Pano IN (  ";
                //SQL = SQL + ComNum.VBLF + "               SELECT a.PTNO ";
                //SQL = SQL + ComNum.VBLF + "                FROM KOSMOS_OCS.OCS_IORDER a ";
                //SQL = SQL + ComNum.VBLF + "                 WHERE a.GbStatus IN (' ','D','D+')  ";
                //SQL = SQL + ComNum.VBLF + "                  AND a.BDate >= TO_DATE('" + Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-7).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                //SQL = SQL + ComNum.VBLF + "                  AND a.BDate <= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                //SQL = SQL + ComNum.VBLF + "                  AND GbPRN =' ' ";
                //SQL = SQL + ComNum.VBLF + "                  AND GbAct ='*' ";  //간호사가 PRN푼것
                //SQL = SQL + ComNum.VBLF + "                  AND SuCode IN ( SELECT JEPCODE FROM KOSMOS_ADM.DRUG_MASTER4  WHERE Insulin_scale ='1'  ) ";
                //SQL = SQL + ComNum.VBLF + "                  AND (a.GbSend  = ' ' OR a.GbSend IS NULL) ";  //전송된것
                //SQL = SQL + ComNum.VBLF + "                  AND a.OrderSite Not Like 'DC%' ";
                //SQL = SQL + ComNum.VBLF + "                  AND a.OrderSite <>  'CAN' ";
                //SQL = SQL + ComNum.VBLF + "                  AND (a.PRN_INS_CDate IS NULL OR a.PRN_INS_CDate ='' )";

                //if (clsOrdFunction.GstrGbJob == "OPD")
                //{
                //    SQL = SQL + ComNum.VBLF + "                  AND a.StaffID ='" + strDrCode1 + "' ";  //의사코드 제한
                //}
                //else
                //{
                //    SQL = SQL + ComNum.VBLF + "                  AND a.StaffID ='" + strDrCode2 + "' ";  //의사코드 제한
                //}

                //SQL = SQL + ComNum.VBLF + "                 ) ";
                //SQL = SQL + ComNum.VBLF + "   GROUP BY  a.Pano   ";
                #endregion

                #region 신규 쿼리
                SQL = "";
                SQL += ComNum.VBLF + "SELECT COUNT(*) CNT";
                SQL += ComNum.VBLF + "FROM ( ";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  '입원' AS IO ,PANO, InDate, SEX, Age, WARDCODE, ROOMCODE, IPDNO, DRCODE, SName";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL += ComNum.VBLF + "WHERE JDATE =TO_DATE('1900-01-01','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  AND GBSTS NOT IN ('7','9')";
                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    if (strDrCode1.Equals("0402"))
                    {
                        SQL += ComNum.VBLF + "  AND DrCode    IN ('0401',  '" + strDrCode1 + "')                                                                      \r";
                    }
                    else if (strDrCode1.Equals("0115"))
                    {
                        SQL += ComNum.VBLF + "  AND DrCode    IN ('0101',  '" + strDrCode1 + "')                                                                      \r";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "  AND DRCODE ='" + strDrCode1 + "'";
                    }
                }
                else
                {
                    if (strDrCode2.Equals("0402"))
                    {
                        SQL += ComNum.VBLF + "  AND DrCode    IN ('0401',  '" + strDrCode2 + "')                                                                      \r";
                    }
                    else if (strDrCode2.Equals("0115"))
                    {
                        SQL += ComNum.VBLF + "  AND DrCode    IN ('0101',  '" + strDrCode2 + "')                                                                      \r";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "  AND DRCODE ='" + strDrCode2 + "'";
                    }
                }
                SQL += ComNum.VBLF + "GROUP BY PANO, InDate, SEX, Age, WARDCODE, ROOMCODE, IPDNO, DRCODE, SName";
                SQL += ComNum.VBLF + "UNION ALL";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  '응급' AS IO ,PANO, BDATE AS InDate, SEX, Age, 'ER' WARDCODE, 100 ROOMCODE, 0 IPDNO, DRCODE, SName";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                SQL += ComNum.VBLF + "WHERE BDATE >= TRUNC(SYSDATE) - 7";
                SQL += ComNum.VBLF + "  AND BDATE <= TRUNC(SYSDATE)";
                SQL += ComNum.VBLF + "  AND DEPTCODE ='ER'";
                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    SQL += ComNum.VBLF + "  AND DRCODE = '" + strDrCode1 + "'";    //의사코드 제한
                }
                else
                {
                    SQL += ComNum.VBLF + "  AND DRCODE = '" + strDrCode2 + "'";  //의사코드 제한
                }
                SQL += ComNum.VBLF + "GROUP BY PANO, BDATE, SEX,Age,  DRCODE, SName ";
                SQL += ComNum.VBLF + ") A";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_OCS.OCS_IORDER B";
                SQL += ComNum.VBLF + "     ON A.PANO  = B.PTNO                                                                         ";
                SQL += ComNum.VBLF + "    AND B.BDATE >= A.INDATE                                                                     ";
                SQL += ComNum.VBLF + "    AND B.BDATE <= TRUNC(SYSDATE)                                                               ";
                SQL += ComNum.VBLF + "    AND B.GbStatus IN (' ','D','D+')                                                            ";
                SQL += ComNum.VBLF + "    AND B.GbPRN = ' '                                                                           ";
                SQL += ComNum.VBLF + "    AND B.GbAct = '*'                                                                           ";
                SQL += ComNum.VBLF + "    AND B.SuCode IN ( SELECT JEPCODE FROM KOSMOS_ADM.DRUG_MASTER4  WHERE Insulin_scale ='1'  )  ";
                SQL += ComNum.VBLF + "    AND (B.GbSend  = ' ' OR B.GbSend IS NULL)                                                   ";
                SQL += ComNum.VBLF + "    AND B.OrderSite Not Like 'DC%'                                                              ";
                SQL += ComNum.VBLF + "    AND B.OrderSite <>  'CAN'                                                                   ";
                SQL += ComNum.VBLF + "    AND (B.PRN_INS_CDate IS NULL OR B.PRN_INS_CDate ='' )                                       ";
                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    if (strDrCode1.Equals("0402"))
                    {
                        SQL += ComNum.VBLF + "    AND B.StaffID    IN ('0401',  '" + strDrCode1 + "')                                                                      \r";
                    }
                    else if (strDrCode1.Equals("0115"))
                    {
                        SQL += ComNum.VBLF + "    AND B.StaffID    IN ('0101',  '" + strDrCode1 + "')                                                                      \r";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "    AND B.StaffID ='" + strDrCode1 + "'";    //의사코드 제한
                    }
                }
                else
                {
                    if (strDrCode1.Equals("0402"))
                    {
                        SQL += ComNum.VBLF + "    AND B.StaffID    IN ('0401',  '" + strDrCode1 + "')                                                                      \r";
                    }
                    else if (strDrCode1.Equals("0115"))
                    {
                        SQL += ComNum.VBLF + "    AND B.StaffID    IN ('0101',  '" + strDrCode1 + "')                                                                      \r";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "    AND B.StaffID ='" + strDrCode2 + "'";  //의사코드 제한
                    }
                }
                SQL += ComNum.VBLF + "GROUP BY PANO";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows.Count.ToString();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHK_OCS_ORDER_SUGA_CHK
        /// 심사과 수가제한 사항 2015-09-10
        /// </summary>
        /// <returns></returns>
        public static string CHK_OCS_ORDER_SUGA_CHK(PsmhDb pDbCon, string strJob, string strSTS, FarPoint.Win.Spread.FpSpread ssSpread, int intSuCol, int intSelfCol)
        {
            string rtnVal = "OK";

            if (clsOrdFunction.Pat.PtNo.IsNullOrEmpty())
                return rtnVal;

            if (clsOrdFunction.Pat.DeptCode.Equals("MG"))
                return rtnVal;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;

            int i = 0;
            int k = 0;

            //int intCnt1 = 0;
            //int intCnt1_1 = 0;
            //int intCnt1A = 0;

            //int intCnt2 = 0;
            //int intCnt2_1 = 0;
            //int intCnt3 = 0;
            //int intCnt3_1 = 0;

            //string[] strSuCode1 = new string[30];
            //string[] strSuCode2 = new string[30];
            //string[] strSuCode3 = new string[30];

            List<string> strAttack1 = new List<string>();
            List<string> strAttack2 = new List<string>();
            List<string> strDefense1 = new List<string>();

            string strSuCode = "";


            try
            {
                for (i = 0; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text.Trim().Equals("True") == false)
                    {
                        if (ssSpread.ActiveSheet.Cells[i, intSelfCol].Text.Trim().IsNullOrEmpty() ||
                            ssSpread.ActiveSheet.Cells[i, intSelfCol].Text.Trim().Equals("0"))
                        {
                            strSuCode = ssSpread.ActiveSheet.Cells[i, intSuCol].Text.Trim();

                            #region 공격인자1
                            SQL = "";
                            SQL = "SELECT CODE ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                            SQL = SQL + ComNum.VBLF + "WHERE GUBUN ='소화성궤양용제_공격인자' ";
                            SQL = SQL + ComNum.VBLF + "  AND TRIM(CODE) ='" + strSuCode + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='') ";

                            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            if (reader.HasRows)
                            {
                                if(strAttack1.IndexOf(strSuCode) == -1)
                                {
                                    strAttack1.Add(strSuCode);
                                }

                                #region 이전로직 주석 2021-06-16
                                //    intCnt1 = intCnt1 + 1;

                                //    if (intCnt1 <= 30)
                                //    {
                                //        strSuCode1[intCnt1] = strSuCode;
                                //    }

                                //    for (k = 0; k < intCnt1; k++)
                                //    {
                                //        if (i != intCnt1)
                                //        {
                                //            if (strSuCode1[k] == strSuCode)
                                //            {
                                //                intCnt1_1 = intCnt1_1 + 1;
                                //            }
                                //        }
                                //    }
                                //}
                                #endregion
                            }

                            reader.Dispose();
                            reader = null;

                            #endregion

                            #region 공격인자2
                            SQL = "";
                            SQL = "SELECT CODE ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                            SQL = SQL + ComNum.VBLF + "WHERE GUBUN ='소화성궤양용제_공격인자2' ";
                            SQL = SQL + ComNum.VBLF + "  AND TRIM(CODE) ='" + strSuCode + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='') ";

                            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            if (reader.HasRows)
                            {
                                if (strAttack2.IndexOf(strSuCode) == -1)
                                {
                                    strAttack2.Add(strSuCode);
                                }
                                //intCnt1A = intCnt1A + 1;
                            }

                            reader.Dispose();
                            reader = null;
                            #endregion

                            #region 이전로직 주석 21-06-16
                            //if ((intCnt1 - intCnt1_1) >= 2)
                            //{
                            //    ComFunc.MsgBox("소화성궤양용제_공격인자 수가는 2가지 이상 사용할 수 없습니다..", "확인");
                            //    rtnVal = "";

                            //    return rtnVal;
                            //}
                            //else
                            //{
                            //    //공격인자1 케이스
                            //    if (intCnt1 >= 1)
                            //    {
                            //        if (intCnt1A >= 1)
                            //        {
                            //            ComFunc.MsgBox("소화성궤양용제_공격인자 수가는 2가지 이상 사용할 수 없습니다..", "확인");
                            //            rtnVal = "";
                            //            return rtnVal;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        if (intCnt1A >= 3)
                            //        {
                            //            ComFunc.MsgBox("소화성궤양용제_공격인자 수가는 2가지 이상 사용할 수 없습니다..", "확인");
                            //            rtnVal = "";
                            //            return rtnVal;
                            //        }
                            //    }
                            //}

                            //if ((intCnt2 - intCnt2_1) >= 2)
                            //{
                            //    ComFunc.MsgBox("소화성궤양용제_방어인자 수가는 2가지 이상 사용할 수 없습니다..", "확인");
                            //    rtnVal = "";

                            //    return rtnVal;
                            //}

                            //if ((intCnt3 - intCnt3_1) >= 2)
                            //{
                            //    ComFunc.MsgBox("비스테로이드성_소염진통제 수가는 2가지 이상 사용할 수 없습니다..", "확인");
                            //    rtnVal = "";

                            //    return rtnVal;
                            //}
                            #endregion

                            #region 방어인자
                            SQL = "";
                            SQL = "SELECT CODE ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                            SQL = SQL + ComNum.VBLF + "WHERE GUBUN ='소화성궤양용제_방어인자' ";
                            SQL = SQL + ComNum.VBLF + "  AND TRIM(CODE) ='" + strSuCode + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='') ";

                            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            if (reader.HasRows)
                            {
                                if (strDefense1.IndexOf(strSuCode) == -1)
                                {
                                    strDefense1.Add(strSuCode);
                                }
                                //intCnt1A = intCnt1A + 1;
                            }

                            reader.Dispose();
                            reader = null;
                            #endregion

                            #region 신규로직 2021-06-16

                            if (strAttack1.Count > 0 && strAttack2.Count > 0 
                                && strAttack1.Count + strAttack2.Count >= 2 || strAttack1.Count >= 2)
                            {
                                ComFunc.MsgBox("소화성궤양용제_공격인자 수가는 2가지 이상 사용할 수 없습니다.\r\n문의 심사팀(8035)", "확인");
                                rtnVal = "";
                                return rtnVal;
                            }

                            if (strDefense1.Count >= 2)
                            {
                                ComFunc.MsgBox("소화성궤양용제_방어인자 수가는 2가지 이상 사용할 수 없습니다.\r\n문의 심사팀(8035)", "확인");
                                rtnVal = "";
                                return rtnVal;
                            }

                            #endregion

                        }
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("함수명 : " + "CHK_OCS_ORDER_SUGA_CHK" + ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("함수명 : " + "CHK_OCS_ORDER_SUGA_CHK" + ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_Dos_Chk_STS_ER_A
        /// 2015-09-14
        /// </summary>
        /// <returns></returns>
        public static string Read_Dos_Chk_STS_ER_A(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread ssSpread, int intSRow, int intSelfCol, string strBun, int intDoscCol, int intE, int intR)
        {
            string rtnVal = "OK";

            int i = 0;
            int intStart = 0;

            string strEchk = "";
            string strRchk = "";
            string strDosCode = "";

            //약만 체크
            if (strBun != "11" && strBun != "12" && strBun != "20")
            {
                return rtnVal;
            }

            intStart = intSRow;

            if (intSRow == 0)
            {
                intStart = 0;
            }

            for (i = intStart; i < ssSpread.ActiveSheet.RowCount; i++)
            {
                if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    strEchk = "";
                    strRchk = "";

                    if (ssSpread.ActiveSheet.Cells[i, intE].Text.Trim() == "E")
                    {
                        strEchk = "OK";
                    }

                    if (ssSpread.ActiveSheet.Cells[i, intR].Text.Trim() == "A")
                    {
                        strRchk = "OK";
                    }

                    if (strEchk == "OK" || strRchk == "OK")
                    {
                        strDosCode = ssSpread.ActiveSheet.Cells[i, intDoscCol].Text.Trim();

                        if (strDosCode != "")
                        {
                            //2015-10-22
                            if (Read_DOSCODE_DIV_Chk(pDbCon, strDosCode) != "OK")
                            {
                                if (strEchk == "OK")
                                {
                                    ComFunc.MsgBox("응급약 처방은 1회투여 용법만 가능합니다.", "용법확인");
                                    rtnVal = "";

                                    return rtnVal;
                                }
                                else if (strRchk == "OK")
                                {
                                    ComFunc.MsgBox("아침 먼저약 처방은 1회투여 용법만 가능합니다.", "용법확인");
                                    rtnVal = "";

                                    return rtnVal;
                                }
                            }
                        }
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_DOSCODE_DIV_Chk
        /// </summary>
        /// <returns></returns>
        public static string Read_DOSCODE_DIV_Chk(PsmhDb pDbCon, string strCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT GbDiv  ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_ODOSAGE ";
                SQL = SQL + ComNum.VBLF + " WHERE DosCode ='" + strCode + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GbDiv ='1' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_JAGA_Drug_Chk
        /// </summary>
        /// <returns></returns>
        public static string Read_JAGA_Drug_Chk(PsmhDb pDbCon, string strPtNo, string strBDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT a.WRTNO  ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_HOimst a, KOSMOS_ADM.DRUG_HOislip b";
                SQL = SQL + ComNum.VBLF + " WHERE a.Pano ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND a.WRTNO=b.WRTNO(+) ";
                SQL = SQL + ComNum.VBLF + "  AND a.Bun ='2' ";
                SQL = SQL + ComNum.VBLF + "  AND a.ipdopd ='I'";
                SQL = SQL + ComNum.VBLF + "  AND a.BDate >=TO_DATE('" + strBDate + "','YYYY-MM-DD')";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : DRG_Code_Set_OCS
        /// </summary>
        /// <returns></returns>
        public static string DRG_Code_Set_OCS(PsmhDb pDbCon, string strDept, string strBDate, string strPtNo, long dblIpdNo, long dblTRSNO,
                                                int intAge, FarPoint.Win.Spread.FpSpread ssSpread, int intMRow, int intSugaCol)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            int i = 0;
            int k = 0;
            int p = 0;
            int intStart = 0;
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strOK = "";
            string strCode = "";
            string strSuCode = "";
            string strDrg = "";
            //string strGbA = "";
            //string strGbB = "";
            string strADC03 = "";
            string strADC04 = "";
            string strSname = "";

            if (strDept != "OT")
            {
                return rtnVal;
            }

            //수술여부체크
            strOK = "";

            
            clsDB.setBeginTran(pDbCon);


            try
            {
                SQL = "";
                SQL = "SELECT SName FROM KOSMOS_PMPA.ORAN_MASTER  ";
                SQL = SQL + ComNum.VBLF + "  WHERE Pano = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND OPDate =   TO_DATE('" + strBDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    strOK = "OK";
                    //strSname = dt.Rows[0]["SNAME"].ToString().Trim();
                    strSname = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                reader = null;

                if (strOK == "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }

                if (intMRow == 0)
                {
                    intStart = 0;
                }
                else
                {
                    intStart = intMRow;
                }

                for (i = intStart; i < ssSpread.ActiveSheet.RowCount; i++)
                {
                    if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        strSuCode = ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim();

                        //strGbA = "";
                        //strGbB = "";
                        strDrg = "";

                        SQL = "";
                        SQL = "SELECT GBN,DCode FROM KOSMOS_PMPA.DRG_MAP_SUGA ";
                        SQL = SQL + ComNum.VBLF + "  WHERE SuNext = '" + strSuCode + "' ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY GBN,DCode  ";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (reader.HasRows)
                        {
                            //for (k = 0; k < dt.Rows.Count; k++)
                            while(reader.Read())
                            {
                                //strCode = dt.Rows[k]["DCODE"].ToString().Trim();
                                strCode = reader.GetValue(1).ToString().Trim();

                                #region 점검하는 로직 없어서 주석 처리(2021-05-18)
                                //OracleDataReader reader2 = null;
                                //SQL = "";
                                //SQL = "SELECT GBN FROM KOSMOS_PMPA.DRG_MAP_SUGA ";
                                //SQL = SQL + ComNum.VBLF + "  WHERE SuNext = '" + strSuCode + "' ";
                                //SQL = SQL + ComNum.VBLF + " GROUP BY GBN ";

                                //SqlErr = "";
                                //SqlErr = clsDB.GetAdoRs(ref reader2, SQL, pDbCon);

                                //if (SqlErr != "")
                                //{
                                //    clsDB.setRollbackTran(pDbCon);
                                //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                //    Cursor.Current = Cursors.Default;
                                //    return rtnVal;
                                //}
                                //if (reader2.HasRows)
                                //{
                                //    for (p = 0; p < dt1.Rows.Count; p++)
                                //    {
                                //        if (reader2.Rows[p]["GBN"].ToString().Trim() == "A")
                                //        {
                                //            //strGbA = "OK";
                                //            //strDrg = "OK";
                                //        }

                                //        if (reader2.Rows[p]["GBN"].ToString().Trim() == "B")
                                //        {
                                //            //strGbB = "OK";
                                //            //strDrg = "OK";
                                //        }
                                //    }
                                //}

                                //reader2.Dispose();
                                //reader2 = null;

                                #endregion

                                if (strDrg == "OK" && strCode != "")
                                {
                                    OracleDataReader reader2 = null;

                                    SQL = "";
                                    SQL = "SELECT DAGE_MIN, DAGE_MAX FROM KOSMOS_PMPA.DRG_CODE_NEW ";
                                    SQL = SQL + ComNum.VBLF + "    WHERE DCODE = '" + strCode + "' ";
                                    SQL = SQL + ComNum.VBLF + "    AND DAGE_MIN IS NOT NULL";
                                    SQL = SQL + ComNum.VBLF + "    AND DAGE_MAX IS NOT NULL";
                                    SQL = SQL + ComNum.VBLF + "GROUP BY DAGE_MIN, DAGE_MAX";

                                    SqlErr = "";
                                    SqlErr = clsDB.GetAdoRs(ref reader2, SQL, pDbCon);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(pDbCon);
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }

                                    if (reader2.HasRows && reader2.Read())
                                    {
                                        if (VB.Val(reader2.GetValue(0).ToString()) >= intAge
                                            && VB.Val(reader2.GetValue(1).ToString()) <= intAge)
                                        {
                                            strDrg = "NO";
                                        }
                                    }

                                    reader2.Dispose();
                                    reader2 = null;
                                }
                            }
                        }

                        reader.Dispose();
                        reader = null;

                        if (strDrg == "OK")
                        {
                            break;
                        }
                    }
                }

                //복강경 여부 표시
                if (strDrg == "OK")
                {
                    for (i = intStart; i < ssSpread.ActiveSheet.RowCount; i++)
                    {
                        if (ssSpread.ActiveSheet.Cells[i, 0].Text != "True")
                        {
                            strCode = ssSpread.ActiveSheet.Cells[i, intSugaCol].Text.Trim();

                            if (strSuCode == "N0031001")
                            {
                                strADC03 = "ADC03";
                            }

                            if (strSuCode == "Q2755A" || strSuCode == "Q2756A")
                            {
                                strADC04 = "ADC04";
                            }
                        }
                    }

                    SQL = "";
                    SQL = "UPDATE KOSMOS_PMPA.IPD_NEW_MASTER";
                    SQL = SQL + ComNum.VBLF + "     SET";
                    SQL = SQL + ComNum.VBLF + "         DRGCODE = '" + strCode + "'  , ";
                    SQL = SQL + ComNum.VBLF + "         GBDRG ='D'   ";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO = '" + dblIpdNo + "' ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = "UPDATE KOSMOS_PMPA.IPD_TRANS";
                    SQL = SQL + ComNum.VBLF + "     SET";
                    SQL = SQL + ComNum.VBLF + "         DRGCODE = '" + strCode + "' ,";
                    SQL = SQL + ComNum.VBLF + "         GBDRG ='D',";
                    SQL = SQL + ComNum.VBLF + "         DRGADC1 ='" + strADC03 + "',";
                    SQL = SQL + ComNum.VBLF + "         DRGADC2 ='" + strADC04 + "'    ";
                    SQL = SQL + ComNum.VBLF + "WHERE IPDNO = '" + dblIpdNo + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND TRSNO = '" + dblTRSNO + "' ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                }

                if (INSERT_DRG_HISTORY_OCS(pDbCon, strPtNo, dblTRSNO, dblIpdNo, strCode, strADC03, "", "", "", "", "1", strSname) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }

                clsDB.setCommitTran(pDbCon);

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : INSERT_DRG_HISTORY_OCS
        /// 외부 트랜젝션 통해서 함수 삽입할 것.
        /// </summary>
        /// <returns></returns>
        public static bool INSERT_DRG_HISTORY_OCS(PsmhDb pDbCon, string strPtNo, double dblTRSNO, double dblIpdNo, string strDRGCode, string strDrgADC1, string strDrgADC2, string strDrgADC3, string strDrgADC4, string strDrgADC5, string strJob, string strSName)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            bool rtnVal = false;
            
            SQL = "";
            SQL = "INSERT INTO KOSMOS_PMPA.DRG_SET_HISTORY ";
            SQL = SQL + ComNum.VBLF + "     (TRSNO,IPDNO,PANO,SNAME,GBSTS,DRGCODE,DRGADC1,DRGADC2,DRGADC3,DRGADC4,";
            SQL = SQL + ComNum.VBLF + "     DRGADC5 , ENTDATE, ENTSABUN )";
            SQL = SQL + ComNum.VBLF + "VALUES ";
            SQL = SQL + ComNum.VBLF + "     (" + dblTRSNO + "," + dblIpdNo + ",'" + strPtNo + "','" + strSName + "',";
            SQL = SQL + ComNum.VBLF + "     '" + strJob + "','" + strDRGCode + "','" + strDrgADC1 + "','" + strDrgADC2 + "',";
            SQL = SQL + ComNum.VBLF + "     '" + strDrgADC3 + "','" + strDrgADC4 + "','" + strDrgADC5 + "',SYSDATE,1111)";

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            rtnVal = true;
            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_SET_LOGOUT_TIME
        /// </summary>
        /// <returns></returns>
        public static int READ_SET_LOGOUT_TIME(PsmhDb pDbCon, string strGubun)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            int rtnVal = 0;

            //BAS_LOGOUT_초설정

            try
            {
                SQL = "";
                SQL = "SELECT NAME ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='" + strGubun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE ='') ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = Convert.ToInt32(VB.Val(dt.Rows[0]["NAME"].ToString().Trim()));
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_PRN_IV_CHK
        /// PRN 투여경로 체크
        /// </summary>
        /// <returns></returns>
        public static string Read_PRN_IV_CHK(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT a.Bun,";
                SQL = SQL + ComNum.VBLF + "  g.TUYAKPATH_11_1, g.TUYAKPATH_11_2, ";
                SQL = SQL + ComNum.VBLF + "  g.TUYAKPATH_12_1, g.TUYAKPATH_12_2, g.TUYAKPATH_12_3, g.TUYAKPATH_12_4, g.TUYAKPATH_12_5, g.TUYAKPATH_12_6, g.TUYAKPATH_12_ETC, ";
                SQL = SQL + ComNum.VBLF + "  g.TUYAKPATH_11_ETC,g.TUYAKPATH_20_1 , g.TUYAKPATH_20_2, g.TUYAKPATH_20_3, g.TUYAKPATH_20_4, g.TUYAKPATH_20_ETC";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_ORDERCODE a, KOSMOS_ADM.DRUG_MASTER1 g ";
                SQL = SQL + ComNum.VBLF + " WHERE TRIM(a.SuCODE) = TRIM(g.JEPCODE) ";
                SQL = SQL + ComNum.VBLF + "  AND JEPCODE ='" + strSuCode.Trim() + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["BUN"].ToString().Trim())
                    {
                        case "11":
                            if (dt.Rows[0]["TUYAKPATH_11_1"].ToString().Trim() == "1")
                            {
                                rtnVal = rtnVal + "경구,";
                            }

                            if (dt.Rows[0]["TUYAKPATH_11_2"].ToString().Trim() == "1")
                            {
                                rtnVal = rtnVal + "설하,";
                            }
                            break;
                        case "12":
                            if (dt.Rows[0]["TUYAKPATH_12_1"].ToString().Trim() == "1")
                            {
                                rtnVal = rtnVal + "피부,";
                            }

                            if (dt.Rows[0]["TUYAKPATH_12_2"].ToString().Trim() == "1")
                            {
                                rtnVal = rtnVal + "눈,";
                            }

                            if (dt.Rows[0]["TUYAKPATH_12_3"].ToString().Trim() == "1")
                            {
                                rtnVal = rtnVal + "코,";
                            }

                            if (dt.Rows[0]["TUYAKPATH_12_4"].ToString().Trim() == "1")
                            {
                                rtnVal = rtnVal + "직장,";
                            }

                            if (dt.Rows[0]["TUYAKPATH_12_5"].ToString().Trim() == "1")
                            {
                                rtnVal = rtnVal + "폐,";
                            }

                            if (dt.Rows[0]["TUYAKPATH_12_6"].ToString().Trim() == "1")
                            {
                                rtnVal = rtnVal + "질,";
                            }
                            break;
                        case "20":
                            if (dt.Rows[0]["TUYAKPATH_20_1"].ToString().Trim() == "1")
                            {
                                rtnVal = rtnVal + "IM,";
                            }

                            if (dt.Rows[0]["TUYAKPATH_20_2"].ToString().Trim() == "1")
                            {
                                rtnVal = rtnVal + "IV,";
                            }

                            if (dt.Rows[0]["TUYAKPATH_20_3"].ToString().Trim() == "1")
                            {
                                rtnVal = rtnVal + "IV infusion,";
                            }

                            if (dt.Rows[0]["TUYAKPATH_20_4"].ToString().Trim() == "1")
                            {
                                rtnVal = rtnVal + "SC,";
                            }
                            break;
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : PRN_IV_2_SET
        /// </summary>
        /// <returns></returns>
        public static string PRN_IV_2_SET(string strChk, string strDosCode, string strValue)
        {
            string rtnVal = "";

            rtnVal = strDosCode;

            switch (strValue)
            {
                case "경구":
                    rtnVal = "010501";
                    break;
                case "설하":
                    rtnVal = "490301";
                    break;
                case "IM":
                    rtnVal = "910120";
                    break;
                case "IV":
                    rtnVal = "920120";
                    break;
                case "IV infusion":
                    rtnVal = "930120";
                    break;
                case "SC":
                    rtnVal = "950120";
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_처방버전_CHK
        /// </summary>
        /// <returns></returns>
        public static string READ_처방버전_CHK(string strGubun, string strVarsion)
        {
            return "";
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_DISP_Time
        /// </summary>
        /// <returns></returns>
        public static string Read_DISP_Time(int intSetTime, int intCTime)
        {
            return "";
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_알러지약체크
        /// </summary>
        /// <returns></returns>
        public static string Read_ALLERGY_MEDICINE_CHK(PsmhDb pDbCon, string strPtNo, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "OK";

            try
            {
                SQL = "";
                SQL = "SELECT REMARK ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_ALLERGY_MST";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ( UPPER(Remark) LIKE '" + VB.UCase(strSuCode) + "%' OR  LOWER(Remark) LIKE '" + VB.UCase(strSuCode) + "%' )  ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).To<string>().Trim();
                }

                reader.Dispose();
                reader= null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : 자동로그아웃파일체크
        /// </summary>
        /// <returns></returns>
        public static string 자동로그아웃파일체크(string strGubun)
        {
            return "";
        }

        /// <summary>
        /// TODO : OrderEtc.bas : SET_LOGOUT_LOG
        /// </summary>
        /// <returns></returns>
        public static void SET_LOGOUT_LOG(PsmhDb pDbCon, string strGubun, string strEXE, double dblSabun, string strIPADD)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            
            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_LOGOUT_LOG";
                SQL = SQL + ComNum.VBLF + "     (BDATE,SABUN,OUTTIME,IPADDR,GUBUN,EXENAME)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         TRUNC(SYSDATE),";
                SQL = SQL + ComNum.VBLF + "         " + dblSabun + ", ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "         '" + strIPADD + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strGubun + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strEXE + "' ";
                SQL = SQL + ComNum.VBLF + "     ) ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_긴급약_분류
        /// </summary>
        /// <returns></returns>
        public static string Read_EMERGENCY_MEDICINE_GROUP(string strBun)
        {
            string rtnVal = "";

            switch (strBun)
            {
                case "11":
                case "12":
                case "20":
                    rtnVal = "OK";
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : OrderEtc.bas : Read_감염자격체크
        /// </summary>
        /// <returns></returns>
        public static string Read_INFECTION_NHIC_CHK(PsmhDb pDbCon, string strPtNo, string strBDate, string strRemark)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_NHIC";
                SQL = SQL + ComNum.VBLF + "   WHERE BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND PANO ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "    AND JOB_STS ='2' ";
                SQL = SQL + ComNum.VBLF + "    AND MESSAGE LIKE '%" + strRemark + "%' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_DIFF_RESERVED
        /// </summary>
        /// <returns></returns>
        public static bool READ_DIFF_RESERVED(PsmhDb pDbCon, string strPtNo, string strRDate, string strDept)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            int i = 0;

            string strDIFF = "";
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "SELECT NAME";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'OCS_테스트_입원예약'";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '시행'";
                SQL = SQL + ComNum.VBLF + "   AND NAME = 'Y'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT DEPTCODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_RESERVED_NEW";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPtNo + "'";
                SQL = SQL + ComNum.VBLF + "    AND DATE3 > TO_DATE('" + strRDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND DATE3 < TO_DATE('" + Convert.ToDateTime(strRDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE NOT IN ('" + strDept + "')";
                SQL = SQL + ComNum.VBLF + "    AND RETDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND TRANSDATE IS NULL";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDept = strDept + "'" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "',";
                    }

                    strDept = VB.Mid(strDept, 1, VB.Len(strDept) - 1);

                    ComFunc.MsgBox(" - 해당일은 아래와 같이 예약이 되어 있어습니다." + ComNum.VBLF + "※예약일 : " + strRDate + ComNum.VBLF + "※예약과 : " + strDIFF, "DRG 예약 확인");

                    rtnVal = true;
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_NEWORDER
        /// </summary>
        /// <returns></returns>
        public static bool READ_NEWORDER(PsmhDb pDbCon, string strPtNo, string strBDate, string strGBN)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "SELECT PTNO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_ORDER_UPDATE ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ORDGBN = '" + strGBN + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_OCS_시행여부
        /// </summary>
        /// <returns></returns>
        public static bool READ_OCS_CODE_YN(PsmhDb pDbCon, string strGubun)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "SELECT NAME";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '" + strGubun + "'";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '시행'";
                SQL = SQL + ComNum.VBLF + "   AND NAME = 'Y'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = true;
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : READ_ADHD_MCADMIN
        /// </summary>
        /// <returns></returns>
        public static bool READ_ADHD_MCADMIN(PsmhDb pDbCon, string strPtNo)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "SELECT PTNO ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_MCCERTIFI_BOHUM4";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = true;
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : OrderEtc.bas : CHECK_F0913
        /// </summary>
        /// <returns></returns>
        public static bool CHECK_F0913(string strOrdCode, string strDeptCode, string strSex, int nAge)
        {
            //string strMsg = "";
            bool rtnVal = false;
            
            if (strOrdCode.Trim() != "PD0800") return rtnVal;

            if (strDeptCode != "PD") return rtnVal;

            //2017-01-19 오전 9시 12분 이향숙 쌤 전화 와서 해당 멘트 표시되지 않도록 막아달라고 하심.
            return rtnVal;

            /*
            if (strSex == "F" && nAge >= 9)
            {
                strMsg = "OK";
            }
            else if (strSex == "M" && nAge >= 10)
            {
                strMsg = "OK";
            }

            if (strMsg == "OK")
            {
                MessageBox.Show("★ 처방코드 : PD0800 " + "\r\n\r\n" +
                                "★ 수가코드 : F0913 " + "\r\n\r\n" +
                                "★ 수가명칭 : 뇌하수체전엽기능검사- FSH. LH 5 회 측정시" + "\r\n\r\n" +
                                " 해당 처방은 남성 10세 이상, 여성 9세 이상 일 경우 비급여로 처방하시기 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rtnVal = true;
            }

            return rtnVal;
            */
        }

        public static bool Make_Jaga_Auto_OrderCode(PsmhDb pDbCon, string strBCode, string strOName1, string strOName2, string strDoscode, ref string strJAGA_OCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strSuga = "";
            string strBun2 = "";  //수가분류
            string strORDERCODE = "";
            string strORDERCODE2 = "";
            string strOrderHead = "";
            string strOrderName = "";
            //string strSname = "";  //성분명
            string strTITLE = "";  //타이틀
            double dblSeqNo = 0;    //seqno
            string strSlipNo = "";
            string strDisRGB = "";
            //string strTemp1 = "";
            //string strTemp2 = "";
            //string strTemp3 = "";
            //string strOrdChk= "";

            strSuga = "JAGA";       //자가약코드

            //strOrdChk = "OK";

            if (strBCode == "" ||strBCode == "XXXXXXXXX")
            {
                strBCode = "XXXXXXXXX";
                
                strOrderName = ComFunc.LeftH(strOName1.Trim(), 50);
            }
            else
            {
                strOrderName = ComFunc.LeftH((strOName1 + " " + strOName2).Trim(), 50);
            }

            strDisRGB = "800080"; //"FFFF00" '"800000"
            strBun2 = "";

            strSlipNo = "0106";  //자가약

            strDisRGB = "800080"; //"FFFF00" '"800000"
            strBun2 = "11";

            dblSeqNo = 0;
            
            try
            {
                //표준코드
                if (strBCode != "XXXXXXXXX")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     OrderCode, SeqNo ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ORDERCODE ";
                    SQL = SQL + ComNum.VBLF + "     WHERE SLIPNO ='0106' ";
                    SQL = SQL + ComNum.VBLF + "         AND SeqNo > 0  ";
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(OrderCode, 1, 2) NOT IN ('J1','J2','J3','J4','J5','J6','J7','J8','J9') ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY OrderCode DESC ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dblSeqNo = 1;
                        strORDERCODE2 = "00001";
                        strOrderHead = "JA_";
                    }
                    else
                    {
                        dblSeqNo = VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim()) + 1;
                        strORDERCODE2 = (VB.Val(VB.Mid(dt.Rows[0]["OrderCode"].ToString().Trim(), 4, 5)) + 1).ToString("00000");
                        strOrderHead = VB.Mid(dt.Rows[0]["ORDERCODE"].ToString().Trim(), 1, 3);
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     OrderCode, SeqNo ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ORDERCODE ";
                    SQL = SQL + ComNum.VBLF + "     WHERE SLIPNO ='0106' ";
                    SQL = SQL + ComNum.VBLF + "         AND SeqNo > 0  ";
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(OrderCode, 1, 2) IN ('J1','J2','J3','J4','J5','J6','J7','J8','J9') ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY OrderCode DESC ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dblSeqNo = 1;
                        strORDERCODE2 = "00001";
                        strOrderHead = "J1_";
                    }
                    else
                    {
                        dblSeqNo = VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim()) + 1;
                        strORDERCODE2 = (VB.Val(VB.Mid(dt.Rows[0]["OrderCode"].ToString().Trim(), 4, 5)) + 1).ToString("00000");
                        strOrderHead = VB.Mid(dt.Rows[0]["ORDERCODE"].ToString().Trim(), 1, 3);
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (dblSeqNo >= 99999) { dblSeqNo = 99999; }

                if (strORDERCODE2 == "99999")
                {
                    if (strOrderHead == "JZ_" || strOrderHead == "J9_")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("자가약 오더코드 생성 오류...전산실 연락요망!!");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    switch (strOrderHead)
                    {
                        case "JA_": strOrderHead = "JB_"; break; 
                        case "JB_": strOrderHead = "JC_"; break;
                        case "JC_": strOrderHead = "JD_"; break;
                        case "JD_": strOrderHead = "JE_"; break;
                        case "JE_": strOrderHead = "JF_"; break;
                        case "JF_": strOrderHead = "JG_"; break;
                        case "JG_": strOrderHead = "JH_"; break;
                        case "JH_": strOrderHead = "JI_"; break;
                        case "JI_": strOrderHead = "JJ_"; break;
                        case "JJ_": strOrderHead = "JK_"; break;
                        case "JK_": strOrderHead = "JL_"; break;
                        case "JL_": strOrderHead = "JM_"; break;
                        case "JM_": strOrderHead = "JN_"; break;
                        case "JN_": strOrderHead = "JO_"; break;
                        case "JO_": strOrderHead = "JP_"; break;
                        case "JP_": strOrderHead = "JQ_"; break;
                        case "JQ_": strOrderHead = "JR_"; break;
                        case "JR_": strOrderHead = "JS_"; break;
                        case "JS_": strOrderHead = "JT_"; break;
                        case "JT_": strOrderHead = "JU_"; break;
                        case "JU_": strOrderHead = "JV_"; break;
                        case "JV_": strOrderHead = "JW_"; break;
                        case "JW_": strOrderHead = "JX_"; break;
                        case "JX_": strOrderHead = "JY_"; break;
                        case "JY_": strOrderHead = "JZ_"; break;

                        case "J1_": strOrderHead = "J2_"; break;
                        case "J2_": strOrderHead = "J3_"; break;
                        case "J3_": strOrderHead = "J4_"; break;
                        case "J4_": strOrderHead = "J5_"; break;
                        case "J5_": strOrderHead = "J6_"; break;
                        case "J6_": strOrderHead = "J7_"; break;
                        case "J7_": strOrderHead = "J8_"; break;
                        case "J8_": strOrderHead = "J9_"; break;
                    }
                }

                strORDERCODE = strOrderHead + strORDERCODE2;   //JA_00001 ~ JA_99999 " 8자리

                //2015-10-26 표준코드 없는것 생성시 오더코드 return
                strJAGA_OCode = "";

                if (strBCode == "XXXXXXXXX")
                {
                    strJAGA_OCode = strORDERCODE;
                }

                //JX_->자가약 미확인 오더코드용
                    
                //코드 유무체크
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     OrderCode, SeqNo ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ORDERCODE ";
                SQL = SQL + ComNum.VBLF + "     WHERE SLIPNO = '0106' ";
                SQL = SQL + ComNum.VBLF + "         AND OrderCode = '" + strORDERCODE + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("이미 " + strORDERCODE + " 중복 오더코드가 있습니다... 다시 작업하세요!!", "코드체크");
                    Cursor.Current = Cursors.Default;
                    //strOrdChk = "";
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;

                if (strBCode != "XXXXXXXXX")
                {
                    //표준코드 체크
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     OrderCode, SeqNo, SpecCode, ROWID ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ORDERCODE ";
                    SQL = SQL + ComNum.VBLF + "     WHERE SLIPNO = '0106' ";
                    SQL = SQL + ComNum.VBLF + "         AND BCode = '" + strBCode + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        //기존 오더코드의 용법이 공란이면 현재용법을 갱신함
                        if (dt.Rows[0]["SPECCODE"].ToString().Trim() == "" && strDoscode != "")
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_ORDERCODE";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         SpecCode = '" + strDoscode + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "     AND SLIPNO = '0106' ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                        
                        dt.Dispose();
                        dt = null;

                        rtnVal = true;
                        return rtnVal;
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    //미확인약 체크 - 표준코드 없는 오더코드
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     OrderCode, SeqNo ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ORDERCODE ";
                    SQL = SQL + ComNum.VBLF + "     WHERE SLIPNO = '0106' ";
                    SQL = SQL + ComNum.VBLF + "         AND OrderName = '" + strOrderName.Trim() + "'  ";
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(OrderCode,1,2) IN ('J1','J2','J3','J4','J5','J6','J7','J8','J9') ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strJAGA_OCode = dt.Rows[0]["ORDERCODE"].ToString().Trim();
                        
                        dt.Dispose();
                        dt = null;

                        rtnVal = true;
                        return rtnVal;
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (dblSeqNo > 0)
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_ORDERCODE";
                    SQL = SQL + ComNum.VBLF + "     (Slipno, Seqno, OrderCode, OrderName, Qty, Nal, DispHeader, DispSpace, ";
                    SQL = SQL + ComNum.VBLF + "     DispRGB, GbBoth, GbInfo, GbInput, GbQty, GbDosage, SpecCode, SuCode, ";
                    SQL = SQL + ComNum.VBLF + "     Bun, GbIMIV, NextCode, SendDept, OrderNameS, ItemCD, SubRate, GbSub, GbGume, ODosCode, IDosCode, BCode)";
                    SQL = SQL + ComNum.VBLF + "VALUES ";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         '" + strSlipNo + "', ";
                    SQL = SQL + ComNum.VBLF + "         " + dblSeqNo + ", ";
                    SQL = SQL + ComNum.VBLF + "         '" + strORDERCODE + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOrderName.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         1, 1, ";
                    SQL = SQL + ComNum.VBLF + "         '" + strTITLE + "', ";
                    SQL = SQL + ComNum.VBLF + "         '0', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strDisRGB + "', ";
                    SQL = SQL + ComNum.VBLF + "         '0', '0', '0', '1', '1', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strDoscode + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSuga + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strBun2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '0', '', ' ', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strBCode + "', ";
                    SQL = SQL + ComNum.VBLF + "         '', '', '0', '0', '', '', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strBCode + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 파우더 가능한지 체크
        /// vb60_new\ocs\ipdocs\iorder\vb_powder.bas (READ_POWDER_SuCode_NEW) 옮김.
        /// clsOrdFunction.Read_Powder_SuCode_New 와 동시에 변경 필수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strSucode"></param>
        /// <returns></returns>
        public static string CHK_POWDER_SUCODE_CHK(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            
            string rtnVal = "";

            try
            {
                #region 2021-06-15 마약,향정 일경우 파우더 안되게 수정.
                SQL = "";
                SQL += " SELECT 1 AS CNT                            \r";
                SQL += "   FROM DUAL                                \r";
                SQL += "  WHERE EXISTS                              \r";
                SQL += "  (                                         \r";
                SQL += "        SELECT JEPCODE                      \r";
                SQL += "        FROM KOSMOS_ADM.DRUG_MASTER2        \r";
                SQL += "        WHERE JEPCODE = '" + strSuCode + "' \r";
                SQL += "          AND SUGABUN = '11'                \r";
                SQL += "          AND EFFECTBUN IN ('04', '05')     \r";
                SQL += "          AND SUB IN ('16', '17')           \r";
                SQL += "  )                                         \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    reader.Dispose();
                    reader = null;
                    return rtnVal;
                }

                reader.Dispose();
                reader = null;
                #endregion

                //산재불가약 체크후 -체크박스 표시
                SQL = "";
                SQL += " SELECT NOT_POWDER, NOT_POWDER_SUB              \r";
                SQL += "   FROM KOSMOS_ADM.DRUG_MASTER4                 \r";
                SQL += "  WHERE JepCode = '" + strSuCode.Trim() + "'    \r";
                SQL += "    AND NOT_POWDER = '1'                        \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    reader.Dispose();
                    reader = null;
                    rtnVal = "";
                    return rtnVal;
                }

                reader.Dispose();
                reader = null;

                SQL = "";
                SQL += " SELECT ROWID                                   \r";
                SQL += "   FROM KOSMOS_ADM.DRUG_MASTER1                 \r";
                SQL += "  WHERE JepCode = '" + strSuCode.Trim() + "'    \r";
                SQL += "    AND POJANG3 = 'T'                           \r";
                SQL += "    AND (GBSUGA1 IS NULL OR GBSUGA1 = '0')      \r"; //'산제가능약

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;
              
                return rtnVal;
                
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// vb60_new\ocs\ipdocs\iorder\vb_powder.bas (READ_POWDER_SuCode_NEW2) 옮김.
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strSucode"></param>
        /// <returns></returns>
        public static string CHK_POWDER_SUCODE_CHK_2(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;

            string rtnVal = "";

            try
            {

                #region 2021-06-15 마약,향정 일경우 파우더 안되게 수정.
                SQL = "";
                SQL += " SELECT 1 AS CNT                            \r";
                SQL += "   FROM DUAL                                \r";
                SQL += "  WHERE EXISTS                              \r";
                SQL += "  (                                         \r";
                SQL += "        SELECT JEPCODE                      \r";
                SQL += "        FROM KOSMOS_ADM.DRUG_MASTER2        \r";
                SQL += "        WHERE JEPCODE = '" + strSuCode + "' \r";
                SQL += "          AND SUGABUN = '11'                \r";
                SQL += "          AND EFFECTBUN IN ('04', '05')     \r";
                SQL += "          AND SUB IN ('16', '17')           \r";
                SQL += "  )                                         \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    reader.Dispose();
                    reader = null;
                    return rtnVal;
                }

                reader.Dispose();
                reader = null;
                #endregion

                //산재불가약 체크후 -체크박스 표시
                SQL = "";
                SQL += " SELECT NOT_POWDER, NOT_POWDER_SUB      \r";
                SQL += " FROM KOSMOS_ADM.DRUG_MASTER4           \r";
                SQL += "  WHERE JepCode = '" + strSuCode + "'   \r";
                SQL += "   AND Not_Powder = '1'                 \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// vb60_new\ocs\ipdocs\iorder\vb_powder.bas (READ_POWDER_SuCode_NEW3) 옮김.
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strSucode"></param>
        /// <returns></returns>
        public static string CHK_POWDER_SUCODE_CHK_3(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;
            string rtnVal = "";

            try
            {
                //산재불가약 체크후 -체크박스 표시
                SQL = "";
                SQL += " SELECT CASE WHEN TRIM(NOT_POWDER_ETC) = '1' THEN '2' ELSE '1' END RESULT   \r";
                SQL += "   FROM KOSMOS_ADM.DRUG_MASTER4                                             \r";
                SQL += "  WHERE JepCode = '" + strSuCode + "'                                       \r";
                SQL += "    AND Not_Powder = '1'                                                    \r";
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).To<string>().Trim();
                    //rtnVal = "1";
                    ////대체약구분
                    //if (dt.Rows[0]["NOT_POWDER_ETC"].ToString().Trim() == "1")
                    //{
                    //    rtnVal = "2";
                    //}
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// READ_권역수가여부
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strSuCode"></param>
        /// <returns></returns>
        public static bool Read_ZoneEm_Suga_Status(PsmhDb pDbCon, string strOrderCode, string strKTAS, string strBDate)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            if (Convert.ToDateTime(strBDate) < Convert.ToDateTime(clsPublic.GstrZoneEmergencyStartDate))
            {
                return rtnVal;
            }

            if (VB.Val(strKTAS) > 3 || VB.Val(strKTAS) == 0 || strKTAS.Trim() == "0")
            {
                return rtnVal;
            }

            strOrderCode = strOrderCode.Trim();

            try
            {
                SQL = "";
                SQL += " SELECT A.SUCODE                                \r";
                SQL += "   FROM KOSMOS_OCS.OCS_ORDERCODE A              \r";
                SQL += "      , KOSMOS_PMPA.BAS_SUN      B              \r";
                SQL += "  Where a.SUCODE = b.SUNEXT                     \r";
                SQL += "    AND A.ORDERCODE = '" + strOrderCode + "'    \r";
                SQL += "    AND B.SUGBAA = '3'                          \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "Read_ZoneEm_Suga_Status " + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = true;
                }

                reader.Dispose();
                reader = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox("함수명 : " + "Read_ZoneEm_Suga_Status " + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "Read_ZoneEm_Suga_Status " + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// 권역응급의료시작일
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strOrderCode"></param>
        /// <param name="strKTAS"></param>
        /// <param name="strBDate"></param>
        /// <returns></returns>
        public static void Read_ZoneEm_StartDate(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT NAME                        \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_BCODE       \r";
                SQL += "  WHERE GUBUN = 'OCS_권역시행일'      \r";
                SQL += "    AND CODE = 'DATE'               \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }

                if (reader.HasRows && reader.Read())
                {
                    clsPublic.GstrZoneEmergencyStartDate = reader.GetValue(0).ToString().Trim();
                }
                else
                {
                    clsPublic.GstrZoneEmergencyStartDate = "";
                }

                reader.Dispose();
                reader = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }
        }

        public static string READ_ER_INTIME(PsmhDb pDbCon, string strPano, string strInDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;

            string rtnVal = "";

            try
            {
                //산재불가약 체크후 -체크박스 표시
                SQL = "";
                SQL += " SELECT TO_CHAR(INTIME,'YYYY-MM-DD HH24:MI') INTIME                                 \r";
                SQL += "   FROM (SELECT PANO, INTIME, OUTTIME                                               \r";
                SQL += "           FROM KOSMOS_PMPA.NUR_ER_PATIENT                                          \r";
                SQL += "          WHERE PANO = '" + strPano + "'                                            \r";
                SQL += "            AND INTIME <= TO_DATE('" + strInDate + " 23:59','YYYY-MM-DD HH24:MI')   \r";
                SQL += "          ORDER BY INTIME DESC)                                                     \r";
                SQL += "   WHERE ROWNUM <= 3                                                                \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }
                else
                {
                    rtnVal = "응급실 내원시간 읽어오기 오류!";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// CVR_검체미확인(Exam_CVR.bas)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strDept"></param>
        /// <param name="strDrCode"></param>
        /// <param name="strGbn" 전문의(1), 전공의(2) 구분></param>
        /// <returns></returns>
        public static int CVR_ITEM_COFIRM(PsmhDb pDbCon, string strDept, string strDrCode, string strGbn)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            int rtnVal = 0;
            string strGrade = "";
            
            strGrade = strGbn == "4" ? "3" : strGbn;

            try
            {
                //산재불가약 체크후 -체크박스 표시
                SQL = "";
                SQL += " SELECT B.SPECNO,B.PANO, B.SNAME, C.EXAMFNAME, B.DRCODE                                                 \r";
                SQL += "      , B.WARD, B.Room, B.DEPTCODE, B.IPDOPD                                                            \r";
                SQL += "   FROM KOSMOS_OCS.EXAM_RESULTC_CV A                                                                    \r";
                SQL += "      , KOSMOS_OCS.EXAM_SPECMST    B                                                                    \r";
                SQL += "      , KOSMOS_OCS.EXAM_MASTER     C                                                                    \r";
                SQL += "  WHERE A.JOBDATE >= TO_DATE('" + DateTime.Parse(clsPublic.GstrSysDate).AddDays(-7).ToShortDateString() + "','YYYY-MM-DD')  \r";
                if (strDept.Trim() == "MD")
                {
                    SQL += "    AND SUBSTR(B.DEPTCODE,1,1) = '" + VB.Left(strDept, 1) + "'                                      \r";
                }
                else
                {
                    SQL += "    AND B.DEPTCODE = '" + strDept + "'                                                              \r";
                }
                //전문의 경우만
                if (strGrade == "1")
                {
                    SQL += "    AND B.DrCode = '" + strDrCode + "'                                                              \r";
                }
                SQL += "    AND A.SPECNO = B.SPECNO(+)                                                                          \r";
                SQL += "    AND A.SUBCODE=  C.MASTERCODE(+)                                                                     \r";
                SQL += "    AND A.GBN IN ('1')                                                                                  \r";
                SQL += "    AND A.CHKDATE IS NULL                                                                               \r";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows.Count;
                }
                else
                {
                    rtnVal = 0;
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// 법정감염병 신고대상자 여부
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="SpdOrdNm"></param>
        /// <param name="SpdDiagNm"></param>
        /// <returns></returns>
        public bool CHECK_INFECT(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread SpdOrdNm, FarPoint.Win.Spread.FpSpread SpdDiagNm)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            ComFunc CF = new ComFunc();
            
            string strOKIll;
            string strOKOrd;

            string strIlls;
            string strORDS;

            bool rtnVal = false;
            string strBDate;

            string COVIDGBN = "N";
            string strBDateAdd = CF.DATE_ADD(clsDB.DbCon, VB.Left(clsOrdFunction.Pat.InDate, 10), 1); 

            if (clsOrdFunction.Pat.DeptCode == "PD") return rtnVal;    //소아과 제외

            strOKIll = "NO";
            strOKOrd = "NO";

            try
            {   
                SQL = "";
                SQL += " SELECT CODE                                        \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_BCODE                       \r";
                SQL += "  WHERE GUBUN = 'OCS_감염병신고대상_시행여부'       \r";
                SQL += "    AND CODE = 'USE'                                \r";
                SQL += "    AND NAME = 'Y'                                  \r";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }
                else
                {
                    rtnVal = false;
                }

                dt.Dispose();
                dt = null;

                //당일 감염병 신고서 작성 내역이 있으면 하단 메시지 안띄움
                //strChk = "NO";
                SQL = "";
                SQL += " SELECT PANO                                                                            \r";
                SQL += "   FROM KOSMOS_PMPA.NUR_STD_INFECT2                                                     \r";
                SQL += "  WHERE PANO = '" + clsOrdFunction.Pat.PtNo + "'                                        \r";

                if (clsOrdFunction.GstrGbJob == "ER" && clsOrdFunction.Pat.InDate != "")
                {
                    SQL += "    AND SDATE >= '" + VB.Left(clsOrdFunction.Pat.InDate, 10).Replace("-", "") + "'  \r";
                    SQL += "    AND SDATE <= '" + VB.Left(strBDateAdd, 10).Replace("-", "") + "'                \r";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;

                    rtnVal = false;
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            //2020-11-06 추가, COVID-19 시행여부체크 (법정감염병신고서 및 검체 시험의뢰서 작성유도 관련) 
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT CODE";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "  AND GUBUN = 'C#_COVID_시행여부'";
                SQL += ComNum.VBLF + "  AND DELDATE IS NULL";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["CODE"].ToString().Trim() == "Y")
                    {
                        COVIDGBN = "Y";                        
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            strIlls = "";
            for (int i = 0; i < SpdDiagNm.ActiveSheet.NonEmptyRowCount; i++)
            {
                //if (SpdDiagNm.ActiveSheet.Cells[i, 2].Text.Trim() != "")
                //{
                    if (clsOrdFunction.GstrGbJob == "OPD")
                    {
                        strBDate = clsOrdFunction.GstrBDate;
                    }
                    else
                    {                        
                        strBDate = SpdDiagNm.ActiveSheet.Cells[i, 0].Text.Trim();                            
                    }

                    if (strBDate != clsPublic.GstrSysDate &&
                        SpdDiagNm.ActiveSheet.Cells[i, 0].Text.Trim() != "" &&
                        clsOrdFunction.GstrGbJob == "IPD")
                    {
                    }
                    else
                    {
                        switch (clsOrdFunction.GstrGbJob)
                        {
                            case "IPD":
                                strIlls += "'" + SpdDiagNm.ActiveSheet.Cells[i, 2].Text.Trim() + "',";
                                break;
                            case "OPD":
                                if (clsOrdFunction.GEnvSet_Item21 != null && clsOrdFunction.GEnvSet_Item21 == "2")
                                {
                                    strIlls += "'" + SpdDiagNm.ActiveSheet.Cells[i, 2].Text.Trim() + "',";
                                }
                                else
                                {
                                    strIlls += "'" + SpdDiagNm.ActiveSheet.Cells[i, 0].Text.Trim() + "',";
                                }
                                
                                break;
                            case "ER":
                                strIlls += "'" + SpdDiagNm.ActiveSheet.Cells[i, 2].Text.Trim() + "',";
                                break;
                            default:
                                return false;
                        }
                    }
                //}
            }

            if (strIlls != "")
            {                
                strIlls = VB.Mid(strIlls, 1, strIlls.Length - 1);
                string[] str = strIlls.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    SQL = "";
                    SQL += " SELECT CODE                                \r";
                    SQL += "   FROM KOSMOS_PMPA.BAS_BCODE               \r";
                    SQL += "  WHERE GUBUN = 'EXAM_감염병신고_상병'      \r";
                    SQL += "    AND DELDATE IS NULL                     \r";
                    SQL += "    AND CODE IN (" + strIlls + ")           \r";
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strOKIll = "OK";
                    }

                    dt.Dispose();
                    dt = null;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox("함수명 : " + "CHECK_INFECT" + ComNum.VBLF + ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                // 전산업무의뢰서 2020-1487
                if (strOKIll == "OK" && str.Length == 1)
                {
                    // 이중상병은 단일상병일때 대상제외
                    switch (str[0])
                    {
                        case "B963":
                        case "B953":
                        case "U830":
                        case "U8280":
                            strOKIll = "";
                            break;
                    }
                }
            }

            strORDS = "";
            for (int i = clsOrdFunction.GnSunapOrdCount; i < SpdOrdNm.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (SpdOrdNm.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    switch (clsOrdFunction.GstrGbJob)
                    {
                        case "IPD":
                            //strORDS += "'" + SpdOrdNm.ActiveSheet.Cells[i, 34].Text.Trim() + "',";
                            //2021-01-12 변경
                            strORDS += "'" + SpdOrdNm.ActiveSheet.Cells[i, 37].Text.Trim() + "',";
                            break;
                        case "OPD":
                            strORDS += "'" + SpdOrdNm.ActiveSheet.Cells[i, 1].Text.Trim() + "',";
                            break;
                        case "ER":
                            strORDS += "'" + SpdOrdNm.ActiveSheet.Cells[i, 33].Text.Trim() + "',";
                            break;
                        default:
                            return false;
                    }
                }
            }

            if (strORDS != "")
            {
                strORDS = VB.Mid(strORDS, 1, strORDS.Length - 1);

                try
                {
                    SQL = "";
                    SQL += " SELECT CODE                                \r";
                    SQL += "   FROM KOSMOS_PMPA.BAS_BCODE               \r";
                    SQL += "  WHERE GUBUN = 'EXAM_감염병신고_검사코드'  \r";
                    SQL += "    AND DELDATE IS NULL                     \r";
                    SQL += "    AND CODE IN (" + strORDS + ")           \r";
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strOKOrd = "OK";
                    }

                    dt.Dispose();
                    dt = null;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
            }

            if (strOKOrd == "OK" && strOKIll == "OK")
            {
                string sMsg = "";
                sMsg = "감염병 신고 대상 처방과 상병이 발생하였습니다." + "\r\n";
                sMsg += "감염병 신고를 하시려면 [예] 버튼을 클릭하시고" + "\r\n";
                sMsg += "이미 신고한 내역이 있거나 신고를 하지 않으실 경우 [아니요] 버튼을 클릭하십시요.";
                
                if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    rtnVal = true;
                    return rtnVal;
                } 
            }
            //2020-11-05 안정수 추가, COVID-19 검사일경우 무조건 작성 후 처방전송
            else if (strOKOrd == "OK" && (strORDS.Contains("'NCOV-1'") || strORDS.Contains("'NCOV'")) && COVIDGBN == "Y")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT PANO";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT2";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "  AND PANO = '" + clsOrdFunction.Pat.PtNo + "'";
                SQL += ComNum.VBLF + "  AND SDATE >= '" + VB.Left(clsOrdFunction.Pat.InDate, 10).Replace("-", "") + "'";
                SQL += ComNum.VBLF + "  AND SDATE <= '" + VB.Left(strBDateAdd, 10).Replace("-", "") + "'";
                SQL += ComNum.VBLF + "  AND INFECT1 = '1Q'";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장   
                    return rtnVal;
                }

                if(dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    rtnVal = false;
                    return rtnVal;
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    string sMsg = "";
                    sMsg = "감염병 신고 대상 처방(COIVD-19)이 발생하였습니다." + "\r\n";
                    sMsg += "감염병 신고서를 작성해주시기 바랍니다." + "\r\n";

                    ComFunc.MsgBox(sMsg, "법정감염병신고서 작성안내");
                    rtnVal = true;
                    return rtnVal;
                }

                //if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                //    rtnVal = true;
                //    return rtnVal;
                //}
            }
            else if (strOKOrd == "OK")
            {
                string sMsg = "";
                sMsg = "감염병 신고 대상 처방이 발생하였습니다." + "\r\n";
                sMsg += "감염병 신고를 하시려면 [예] 버튼을 클릭하시고" + "\r\n";
                sMsg += "이미 신고한 내역이 있거나 신고를 하지 않으실 경우 [아니요] 버튼을 클릭하십시요.";

                if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    rtnVal = true;
                    return rtnVal;
                }
            }
            else if (strOKIll == "OK")
            {
                string sMsg = "";
                sMsg = "감염병 신고 대상 상병이 발생하였습니다." + "\r\n";
                sMsg += "감염병 신고를 하시려면 [예] 버튼을 클릭하시고" + "\r\n";
                sMsg += "이미 신고한 내역이 있거나 신고를 하지 않으실 경우 [아니요] 버튼을 클릭하십시요.";

                if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    rtnVal = true;
                    return rtnVal;
                }
            }

            rtnVal = false;
            return rtnVal;
        }

        public bool CHECK_BLOOD_CULTURE(FarPoint.Win.Spread.FpSpread sPdNm, string strBDate)
        {
            bool rtnVal = false;
            int nCol;
            string sMsg = "";

            //string strChk = "";

            for (int i = 0; i < sPdNm.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (sPdNm.ActiveSheet.Cells[i, 0].Text == "False")
                {
                    switch (clsOrdFunction.GstrGbJob)
                    {
                        case "IPD":
                            //2021-01-12 변경
                            //nCol = 34;
                            nCol = 37;
                            break;
                        case "OPD":
                            nCol = 1;
                            break;
                        case "ER":                            
                            nCol = 33;
                            break;
                        default:
                            return false;
                    }

                    switch (sPdNm.ActiveSheet.Cells[i, nCol].Text)
                    {
                        case "B4051E":
                        case "B4051I":
                        case "B4051R":
                        case "B4051K":
                        case "B4051M":
                        case "B4051B":
                        case "B4051C":
                        case "B4062AA":
                        case "B4052R":
                        case "B4051D":
                            if (string.Compare(strBDate, "2017-09-01") >= 0)
                            {
                                sMsg = "";
                                sMsg += "★처방불가코드 : " + sPdNm.ActiveSheet.Cells[i, nCol].Text + "\r\n\r\n";
                                sMsg += " 해당 미생물검사 처방은 2017년 8월 30일까지만 처방이 가능합니다." + "\r\n\r\n";
                                sMsg += " 변경된 미생물검사 처방코드를 입력하십시요.";
                                MessageBox.Show(sMsg, "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                rtnVal = true;
                                return rtnVal;
                            }
                            break;
                        case "#G10":
                        case "B4141":
                        case "B4142":
                        case "B4143":
                        case "B4144":
                        case "B4144-1":
                        case "B4145":
                        case "B4142VRE":
                        case "B4142CRE":
                            if (string.Compare(strBDate, "2017-09-01") >= 0)
                            {
                                sMsg = "";
                                sMsg += "★처방불가코드 : " + sPdNm.ActiveSheet.Cells[i, nCol].Text + "\r\n\r\n";
                                sMsg += " 해당 미생물검사 처방은 2017년 9월 1일부터 처방이 가능합니다." + "\r\n\r\n";
                                sMsg += " 이전 미생물검사 처방코드를 입력하십시요.";
                                MessageBox.Show(sMsg, "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                rtnVal = true;
                                return rtnVal;
                            }
                            break;    
                        default:
                            break;
                    }
                }
            }

            return rtnVal;
        }

        public string fn_Read_Suga_Fm_View(string sSuCode)
        {
            //가정의학과 수가코드 제한
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;

            string rtnVal = "";

            try
            {
                SQL = "";
                SQL += " SELECT a.JepCode                                               \r";
                SQL += "   FROM KOSMOS_ADM.DRUG_SPECIAL_JEPCODE a                       \r";
                SQL += "      , KOSMOS_ADM.DRUG_SPECIAL_SABUN   b                       \r";
                SQL += "  WHERE a.SEQNO = b.SEQNO                                       \r";
                SQL += "    AND TRIM(a.JepCode) = '" + sSuCode + "'                     \r";
                SQL += "    AND b.SABUN  IN ('34625','32158','34902','36531')           \r";
                SQL += "    AND a.Seqno = 6                                             \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return rtnVal;
        }

        public string fn_Read_Chk_TA52(string sGubun, FarPoint.Win.Spread.FpSpread SpdNm)
        {
            //자보 52, 55 종 일경우 HA471M , BC6 재확인 메시지
            int nCol = 0;
            string strDataChk = "";

            string rtnVal = "OK";

            if (sGubun == "I" || sGubun == "O")
            {
                for (int i = 0; i < SpdNm.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (SpdNm.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        if (sGubun == "O")  //외래
                        {
                            //nCol = 12;
                            //2021-01-12 변경
                            nCol = 17;
                        }
                        if (sGubun == "E")  //응급
                        {
                            nCol = 13;
                        }
                        else
                        {
                            //입원
                            //nCol = 14;
                            //2021-01-12 변경
                            nCol = 17;
                        }
                        
                        if (SpdNm.ActiveSheet.Cells[i, nCol].Text.Trim() == "HA471M" || SpdNm.ActiveSheet.Cells[i, nCol].Text.Trim() == "BC6" ||
                            SpdNm.ActiveSheet.Cells[i, nCol].Text.Trim() == "BZ073" || SpdNm.ActiveSheet.Cells[i, nCol].Text.Trim() == "NN261" ||
                            SpdNm.ActiveSheet.Cells[i, nCol].Text.Trim() == "MM261A")
                        {
                            strDataChk = "OK";
                            break;
                        }
                        
                    }
                }

                if (strDataChk == "OK")
                {
                    if (MessageBox.Show("자동차보험 52,55종일경우 HA471M,BC6,BZ073,MM261,MM261A 수가재확인!! " + "\r\n\r\n" + "원무 자보담당자 요청건(청구삭감때문)!! 문의 ☎260-8102 " + "\r\n\r\n" + "이대로 전송하시겠습니까??", "수가재확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        rtnVal = "NO";
                    }
                }

                if (rtnVal == "NO")
                {
                    return rtnVal;
                }

                strDataChk = "";
                for (int i = 0; i < SpdNm.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (SpdNm.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        if (sGubun == "O")  //외래
                        {
                            //nCol = 12;
                            //2021-01-12 변경
                            nCol = 17;
                        }
                        if (sGubun == "E")  //응급
                        {
                            nCol = 13;
                        }
                        else
                        {
                            //입원

                            //nCol = 14;
                            //2021-01-12 변경
                            nCol = 17;
                        }

                        if (SpdNm.ActiveSheet.Cells[i, nCol].Text == "HYAL" || SpdNm.ActiveSheet.Cells[i, nCol].Text == "HYAL-F")
                        {
                            strDataChk = "OK";
                            break;
                        }
                    }
                }

                if (strDataChk == "OK")
                {
                    if(MessageBox.Show("산재,자보환자입니다!!! HYAL,HYAL-F 수가코드 본인부담을 설명하셨습니까?" + "\r\n\r\n" + "원무 자보담당자 요청건(청구삭감때문)!! " + "\r\n\r\n" + "이대로 전송하시겠습니까??", "수가재확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        rtnVal = "NO";
                    }
                }

                if (rtnVal == "NO")
                {
                    return rtnVal;
                }

                strDataChk = "";
                for (int i = 0; i < SpdNm.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (SpdNm.ActiveSheet.Cells[i, 0].Text.Trim() != "True")
                    {
                        if (sGubun == "O")  //외래
                        {
                            //nCol = 12;
                            nCol = 17;
                        }
                        if (sGubun == "E")  //응급
                        {
                            nCol = 13;
                        }
                        else  //입원
                        {
                            //nCol = 14;
                            nCol = 17;
                        }

                        //자보환자 MRI 제한 코드 원무과에서 적용 가능하도록 작업
                        if (fn_Read_Adm_TA_Mri(SpdNm.ActiveSheet.Cells[i, nCol].Text) == "OK")
                        {
                            strDataChk = "OK";
                            break;
                        }
                    }
                }

                if (strDataChk == "OK")
                {
                    MessageBox.Show("자보환자 임의 MRI오더 발생안됨!!!" + "\r\n\r\n" + "원무과 자보당담자에게 문의하십시오!!", "전송불가", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    rtnVal = "NO";
                }
            }

            if (rtnVal == "NO")
            {
                return rtnVal;
            }

            strDataChk = "";
            for (int i = 0; i < SpdNm.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (SpdNm.ActiveSheet.Cells[i, 1].Text != "True")
                {
                    if (sGubun == "O")  //외래
                    {
                        //2021-01-12 변경
                        //nCol = 12;
                        nCol = 17;
                    }
                    if (sGubun == "E")  //응급
                    {
                        nCol = 13;
                    }
                    else
                    {
                        //입원
                        //nCol = 14;
                        //2021-01-12 변경
                        nCol = 17;
                    }

                    if (SpdNm.ActiveSheet.Cells[i, nCol].Text.Trim() == "BM5000RQ")
                    {
                        strDataChk = "OK";
                        break;
                    }
                }
            }

            if (strDataChk == "OK")
            {
                MessageBox.Show("자보환자자격시 재료대 코드 BM5000RQ 처방불가!! [청구삭감]" + "\r\n\r\n" + "원무과 자보당담자에게 문의하십시오!!", "전송불가", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //rtnVal = "NO";
            }
           
            return rtnVal;
        }

        public string fn_Read_Adm_TA_Mri(string strCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;

            string rtnVal = "";
            try
            {
                SQL = "";
                SQL += " SELECT CODE                                \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_BCODE               \r";
                SQL += "  WHERE GUBUN = 'OCS_MRI_원무과_제어_코드'     \r";
                SQL += "    AND CODE = '" + strCode + "'            \r";
                SQL += "    AND DELDATE IS NULL                     \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }
                reader.Dispose();
                reader = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public void INIT_NewOrdCheck()
        {
            GstrNewOrderI = "";
            GstrNewOrderU = "";
            GstrNewOrderD = "";
        }

        public static void Update_Order_DML(PsmhDb pDbCon, string argOrdGbn, string argDMLGbn)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            
            try
            {
                SQL = "";
                SQL += " INSERT INTO KOSMOS_OCS.OCS_ORDER_UPDATE                                \r";
                SQL += "        (PTNO, BDATE, DEPTCODE, DRCODE                                  \r";
                SQL += "      , ORDGBN, DMLGBN, WSABUN, WDATE                                   \r";
                SQL += "      , CSABUN, CDATE)                                                  \r";
                SQL += " VALUES (                                                               \r";
                SQL += "        '" + clsOrdFunction.Pat.PtNo.Trim() + "'                        \r";
                SQL += "      , TO_DATE('" + clsOrdFunction.Pat.BDate.Trim() + "','YYYY-MM-DD') \r";
                SQL += "      , '" + clsOrdFunction.Pat.DeptCode.Trim() + "',''                 \r";
                SQL += "      , '" + argOrdGbn + "','" + argDMLGbn + "'                         \r";
                SQL += "      , " + double.Parse(clsType.User.Sabun) + "                        \r";
                SQL += "      , SYSDATE, 0, TO_DATE('1900-01-01','YYYY-MM-DD'))                 \r";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        /// <summary>
        /// READ_보험상복부초음파_CHK
        /// </summary>
        /// <param name="SpdNm"></param>
        /// <returns></returns>
        public string fn_Read_Self_AbdmSono_Chk(FarPoint.Win.Spread.FpSpread SpdNm)
        {
            string rtnVal = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;

            //string strCan;
            string strTSuGa = "";
            string strCdate;    // 최종 시행일

            string strOk = "";
            string strOk2 = "";  // EB441 여부(최초 1회만 보험, 이후 부터 80%)
            string strOk3 = "";  // EB442 여부(연2회 보험, 3회부터 80%)

            string strResult = "";
            string sMsg = "";


            //결과값이 YES 면 처방 전송 중지
            //결과값이 EB441이 포함이면 4대 중증초음파 CHK에서 EB441 제회
            //결과값이 EB442이 포함이면 4대 중증초음파 CHK에서 EB442 제회
            
            if (string.Compare(clsOrdFunction.GstrBDate, "2018-04-01") < 0)
            {
                return rtnVal;
            }

            //=================================================
            //2018-03-26 보험심사팀장과 통화 후 제외 처리(자보,산재는 제외)

            switch (clsOrdFunction.Pat.Bi)
            {
                case "31":
                case "32":
                case "33":
                case "52":
                case "55":
                    return rtnVal;
                default:
                    break;
            }

            try
            {
                SQL = "";
                SQL += " SELECT NAME                                \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_BCODE               \r";
                SQL += "  WHERE GUBUN = 'READ_보험상복부초음파_CHK' \r";
                SQL += "    AND CODE = '시행'                       \r";
                SQL += "    AND NAME = 'Y'                          \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                }
                else
                {
                    reader.Dispose();
                    reader = null;
                    return rtnVal;
                }

                reader.Dispose();
                reader = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            strOk = "";
            strOk2 = "";
            strOk3 = "";

            for (int i = 0; i < SpdNm.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (SpdNm.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    if (clsOrdFunction.GstrGbJob == "OPD")
                    {
                        //strTSuGa = SpdNm.ActiveSheet.Cells[i, 12].Text.Trim();
                        //2021-01-12 변경
                        strTSuGa = SpdNm.ActiveSheet.Cells[i, 17].Text.Trim();
                    }
                    else if (clsOrdFunction.GstrGbJob == "ER")
                    {
                        strTSuGa = SpdNm.ActiveSheet.Cells[i, 13].Text.Trim();
                    }
                    else if (clsOrdFunction.GstrGbJob == "IPD")
                    {
                        //strTSuGa = SpdNm.ActiveSheet.Cells[i, 14].Text.Trim();
                        //2021-01-12 변경
                        strTSuGa = SpdNm.ActiveSheet.Cells[i, 17].Text.Trim();
                    }
                }

                switch (VB.Left(strTSuGa, 2))
                {
                    case "@V":
                        strOk = "OK";
                        break;
                    default:
                        if (strTSuGa == "EB441")
                        {
                            strOk2 = "OK";
                        }
                        else if (strTSuGa == "EB442")
                        {
                            strOk3 = "OK";
                        }
                        break;
                }
            }

            //@V코드가 있는 경우 4대 중증 초음파 로직 타기
            if (strOk == "OK")
            {
                rtnVal = "";
                return rtnVal;
            }

            strResult = "";

            //@V없이 EB441 처방만 있을 경우
            strCdate = "";

            if (strOk2 == "OK" && strOk != "OK")
            {
                try
                {
                    SQL = "";
                    SQL += " SELECT MIN(BDATE) BDATE                                                    \r";
                    SQL += "   FROM KOSMOS_PMPA.OPD_SLIP                                                \r";
                    SQL += "  WHERE PANO = '" + clsOrdFunction.Pat.PtNo + "'                            \r";
                    SQL += "    AND SUNEXT = 'EB441'                                                    \r";
                    SQL += "    AND BDATE >= TO_DATE('2018-04-01','YYYY-MM-DD')                         \r";
                    SQL += "    AND BDATE < TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')    \r";
                    SQL += "  UNION ALL                                                                 \r";
                    SQL += " SELECT MIN(BDATE) BDATE                                                    \r";
                    SQL += "   FROM KOSMOS_PMPA.IPD_NEW_SLIP                                            \r";
                    SQL += "  WHERE PANO = '" + clsOrdFunction.Pat.PtNo + "'                            \r";
                    SQL += "    AND SUNEXT = 'EB441'                                                    \r";
                    SQL += "    AND BDATE >= TO_DATE('2018-04-01','YYYY-MM-DD')                         \r";
                    SQL += "    AND BDATE < TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')    \r";
                    SQL += "  ORDER BY BDATE ASC                                                        \r";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (reader.HasRows && reader.Read())
                    {
                        strCdate = reader.GetValue(0).ToString().Trim();
                    }

                    if (strCdate != "")
                    {
                        //sMsg = "";
                        //sMsg = "★ 최초 처방일 : " + "\r\n\r\n";
                        //sMsg += " 담낭 용종 F/U은 년1회 추가 보험적용" + "\r\n";
                        //sMsg += " 새로운 질환 검사 보험 적용" + "\r\n";
                        //sMsg += " 기존 검사 질환 F/U 인 경우 본인 부담 80%입니다." + "\r\n";
                        //sMsg += " (기존검사 F/U인 경우 급여항에 2.비급여(보험100%)로 처방 해 주세요.)" + "\r\n";
                        //if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        //{
                        //    rtnVal = "YES";
                        //    return rtnVal;
                        //}
                    }
                    strResult = "EB441";

                    reader.Dispose();
                    reader = null;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
            }

            //@V없이 EB442 처방만 있을 경우
            strCdate = "";
            if (strOk3 == "OK" && strOk != "OK")
            {
                try
                {
                    #region 사용 안하는듯.. 주석처리 2021-05-18
                    //SQL = "";
                    //SQL += " SELECT PANO, BDATE, SUNEXT                                                                             \r";
                    //SQL += "   FROM (                                                                                               \r";
                    //SQL += "         SELECT PANO, BDATE, SUNEXT, SUM(QTY*NAL) CNT                                                   \r";
                    //SQL += "           FROM KOSMOS_PMPA.IPD_NEW_SLIP                                                                \r";
                    //SQL += "          WHERE BDATE >= TO_DATE('" + VB.Left(clsOrdFunction.GstrBDate, 4) + "-01-01','YYYY-MM-DD')     \r";
                    //SQL += "            AND BDATE < TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')                           \r";
                    //SQL += "            AND BDATE >= TO_DATE('2018-04-01','YYYY-MM-DD')                                             \r";
                    //SQL += "            AND SUNEXT = 'EB442'                                                                        \r";
                    //SQL += "            AND PANO = '" + clsOrdFunction.Pat.PtNo + "'                                                \r";
                    //SQL += "          GROUP BY PANO, BDATE, SUNEXT                                                                  \r";
                    //SQL += "          Having Sum(QTY * NAL) > 0                                                                     \r";
                    //SQL += "           Union All                                                                                    \r";
                    //SQL += "          SELECT PANO, BDATE, SUNEXT, SUM(QTY*NAL) CNT                                                  \r";
                    //SQL += "           From KOSMOS_PMPA.OPD_SLIP                                                                    \r";
                    //SQL += "          WHERE BDATE >= TO_DATE('" + VB.Left(clsOrdFunction.GstrBDate, 4) + "-01-01','YYYY-MM-DD')     \r";
                    //SQL += "            AND BDATE < TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')                           \r";
                    //SQL += "            AND BDATE >= TO_DATE('2018-04-01','YYYY-MM-DD')                                             \r";
                    //SQL += "            AND SUNEXT = 'EB442'                                                                        \r";
                    //SQL += "            AND PANO = '" + clsOrdFunction.Pat.PtNo + "'                                                \r";
                    //SQL += "          GROUP BY PANO, BDATE, SUNEXT                                                                  \r";
                    //SQL += "          HAVING SUM(QTY*NAL) > 0)                                                                      \r";
                    //SQL += "  ORDER BY BDATE                                                                                        \r";
                    //SqlErr = clsDB.GetDataTableREx(ref dtSono, SQL, clsDB.DbCon);
                    //if (SqlErr != "")
                    //{
                    //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    //    return rtnVal;
                    //}

                    //if (dtSono.Rows.Count >= 2)
                    //{                        
                    //    //sMsg = "";
                    //    //sMsg = "★ 최초 처방일 : " + dtSono.Rows[0]["BDATE"].ToString().Trim() + "\r\n";
                    //    //sMsg += " 연2회 보험적용 " + dtSono.Rows.Count + 1 + "차 처방입니다." + "\r\n";
                    //    //sMsg += " 3회 부터 본인부담 80%입니다. 급여항에 2.비급여(보험100%)로 처방해주세요" + "\r\n";
                    //    //sMsg += " 처방을 변경하시겠습니까?" + "\r\n";
                    //    //if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //    //{
                    //    //    rtnVal = "YES";
                    //    //    dtSono.Dispose();
                    //    //    dtSono = null;

                    //    //    if (strResult != "")
                    //    //    {
                    //    //        rtnVal = strResult;
                    //    //    }
                    //    //    return rtnVal;
                    //    //}
                    //}
                    //dtSono.Dispose();
                    //dtSono = null;

                    #endregion

                    strResult += "EB442";
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
            }
            rtnVal = strResult;
            return rtnVal; 
        }

        public string RETURN_READ_MAGAM(string strPano, string sBi, string sMCode)
        {
            //마감여부 확인
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;

            string rtnVal = "";

            //if ((sBi == "11" || sBi == "12" || sBi == "13") && (sMCode == "" || sMCode == null))
            //2018.10.17 윤조연 계장 요청 "BI = 51" 추가
            if ((sBi == "11" || sBi == "12" || sBi == "13" || sBi == "51") &&  string.IsNullOrWhiteSpace(sMCode))
            {
                try
                {
                    SQL = "";
                    SQL += " SELECT ROWID                                                   \r";
                    SQL += "   FROM KOSMOS_PMPA.ETC_RETURN                                  \r";
                    SQL += "  WHERE PANO = '" + strPano + "'                                \r";
                    SQL += "    AND DEPTCODE = '" + clsOrdFunction.Pat.DeptCode + "'        \r";    //2018.07.20 진료과 체크 추가.
                    SQL += "    AND MAGAM = '-'                                             \r";
                    SQL += "    AND H_CODE IN (SELECT CODE                                  \r";
                    SQL += "                     FROM KOSMOS_PMPA.ETC_RETURN_CODE           \r";
                    SQL += "                    WHERE CHARGE IS NOT NULL                    \r";
                    SQL += "                      AND GUBUN = '01'                          \r";
                    SQL += "                      AND (H_GUBUN2 IS NULL OR H_GUBUN2 != 'Y') \r";
                    SQL += "                      AND DELDATE IS NULL                       \r";
                    SQL += "                   )                                            \r";
                    //SQL += "    AND H_CODE IN ('0080','0347','0557','0583','0579','0438',   \r";
                    //SQL += "                 '0439','0119','0560','0540','0596','0518')     \r";
                    //    SQL += "   AND (    RNAME LIKE ' % 코아이비인후과 % '"            \r";
                    //    SQL += "         OR RNAME LIKE ' % 메디칼닥터스당 % '"            \r";
                    //    SQL += "         OR RNAME LIKE ' % 속시원내과북구점 % '"          \r";
                    //    SQL += "         OR RNAME LIKE ' % 오학윤 % '"                    \r";
                    //    SQL += "         OR RNAME LIKE ' % 포항요양 % '"                  \r";
                    //    SQL += "         OR RNAME LIKE ' % 바른정형 % '"                  \r";
                    //    SQL += "         OR RNAME LIKE ' % 포항여성 % '"                  \r";
                    //    SQL += "         OR RNAME LIKE ' % 서울아동 % '"                  \r";
                    //    SQL += "         OR RNAME LIKE ' % 여성아이 % '"                  \r";
                    //    SQL += "         OR RNAME LIKE ' % 송라요양 % '"                  \r";
                    //    SQL += "         OR RNAME LIKE ' % 도담지 % '"                    \r";
                    //    SQL += "         OR RNAME LIKE ' % 로뎀 % ' )"                    \r";  
                    SQL += "  ORDER BY ACTDATE ASC                                          \r";
                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        rtnVal = reader.GetValue(0).ToString().Trim();
                    }
                    reader.Dispose();
                    reader = null;
                    return rtnVal;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
            }
            return rtnVal;
        }

        public string RETURN_CHECK_SUGA(FarPoint.Win.Spread.FpSpread SpdNm)
        {
            //마감여부 확인
            string rtnVal = "";

            for (int i = 0; i < SpdNm.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (SpdNm.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    if (clsOrdFunction.GstrGbJob == "OPD")
                    {
                        if (SpdNm.ActiveSheet.Cells[i, 1].Text.Trim() == "IA231")
                        {
                            rtnVal = "OK";
                            break;
                        }
                    }
                    else if (clsOrdFunction.GstrGbJob == "IPD")
                    {
                        if (SpdNm.ActiveSheet.Cells[i, 14].Text.Trim() == "IA221")
                        {
                            rtnVal = "OK";
                            break;
                        }
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 진료의뢰료 체크 로직 
        /// </summary>
        /// <param name="SpdNm"></param>
        /// <returns></returns>
        public string CHK_REQUEST213(FarPoint.Win.Spread.FpSpread SpdNm) 
        {
            //IA213 코드 확인
            string rtnVal = "";

            for (int i = 0; i < SpdNm.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (SpdNm.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    if (clsOrdFunction.GstrGbJob == "OPD")
                    {
                        if (SpdNm.ActiveSheet.Cells[i, 1].Text.Trim() == "IA213")
                        {
                            rtnVal = "OK";
                            break;
                        }
                    }
                    //else if (clsOrdFunction.GstrGbJob == "IPD")
                    //{
                    //    if (SpdNm.ActiveSheet.Cells[i, 14].Text.Trim() == "IA213")
                    //    {
                    //        rtnVal = "OK";
                    //        break;
                    //    }
                    //}
                }
            }

            return rtnVal;
        }

        public string CHK_COVID_JONG(FarPoint.Win.Spread.FpSpread SpdNm)
        {            
            string rtnVal = "";
            string SysDate = "";
            string SQL = "";
            string SqlErr = "";
            string[] strCovCode = new string[5];
            DataTable dt = null;

            #region 시행여부 체크
            SQL = "";            
            SQL += ComNum.VBLF + "SELECT CODE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND GUBUN = 'C#_COVID_시행여부'";
            SQL += ComNum.VBLF + "  AND DELDATE IS NULL";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if(dt.Rows.Count == 0)
            {
                rtnVal = "OK";
                return rtnVal;
            }

            if(dt.Rows.Count > 0)
            {
                if(dt.Rows[0]["CODE"].ToString().Trim() == "N")
                {
                    rtnVal = "OK";
                    return rtnVal;
                }
            }

            dt.Dispose();
            dt = null;
            #endregion

            SQL = "";
            SQL += ComNum.VBLF + "SELECT CODE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND GUBUN = 'C#_COVID_종류'";
            SQL += ComNum.VBLF + "  AND DELDATE IS NULL";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                
            }

            if (dt.Rows.Count > 0)
            {
                if (strCovCode.Length < dt.Rows.Count) Array.Resize(ref strCovCode, dt.Rows.Count);

                for(int j = 0; j < dt.Rows.Count; j++)
                {
                    strCovCode[j] = dt.Rows[j]["CODE"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;

            for (int i = 0; i < SpdNm.ActiveSheet.NonEmptyRowCount; i++)
            {
                for (int j = 0; j < strCovCode.Length; j++)
                {
                    if (SpdNm.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        if (clsOrdFunction.GstrGbJob == "OPD")
                        {
                            if (SpdNm.ActiveSheet.Cells[i, 1].Text.Trim() == strCovCode[j])
                            {
                                rtnVal = "OK";
                                break;
                            }
                        }
                        else if (clsOrdFunction.GstrGbJob == "IPD")
                        {
                            //if (SpdNm.ActiveSheet.Cells[i, 34].Text.Trim() == strCovCode[j])
                            //2021-01-12 수정
                            if (SpdNm.ActiveSheet.Cells[i, 37].Text.Trim() == strCovCode[j])
                            {
                                rtnVal = "OK";
                                break;
                            }
                        }
                        else if (clsOrdFunction.GstrGbJob == "ER")
                        {
                            if (SpdNm.ActiveSheet.Cells[i, 33].Text.Trim() == strCovCode[j])
                            {
                                rtnVal = "OK";
                                break;
                            }
                        }
                    }
                }
            }

            if(rtnVal == "OK")
            {
                if(clsOrdFunction.GstrGbJob == "ER")
                {
                    SysDate = VB.Left(clsOrdFunction.Pat.InDate, 10);
                }
                else
                {
                    SysDate = VB.Left(clsOrdFunction.GstrBDate, 10);
                }                

                SQL = "";
                SQL += ComNum.VBLF + "SELECT * ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "ETC_COVID";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "  AND PANO = '" + clsOrdFunction.Pat.PtNo + "' ";
                SQL += ComNum.VBLF + "  AND BALDATE >= TO_DATE('" + SysDate + "', 'YYYY-MM-DD')";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    rtnVal = "";
                }

                if( dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
                }
                else
                {
                    rtnVal = "NO";
                }

                dt.Dispose();
                dt = null;
            }

            return rtnVal;
        }

        public void RETURN_UPDATE(PsmhDb pDbCon, string strRowId)
        {
            //마감여부 확인
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += " UPDATE KOSMOS_PMPA.ETC_RETURN SET              \r";
                SQL += "        MAGAM = 'Y'                             \r";
                SQL += "      , MAGAMDATE = SYSDATE                     \r";
                SQL += "      , MAGAMSABUN = " + clsType.User.Sabun + " \r";
                SQL += "  WHERE ROWID = '" + strRowId + "'              \r";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                clsDB.setCommitTran(pDbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        public struct OCS_CP_RECORD
        {
            public double CPNO;
            public string PtNo;
            public string sName;
            public string GbIO;
            public int Age;
            public string Sex;
            public string DeptCode;
            public string RoomCode;
            public string CP_DEPT;  //CP코드의 과
            public string Bi;
            public string InTime;
            public string BDate;
            public string CP_CODE;
            public string CP_Name;
            public string CP_SAYU;
            public string CP_ROWID;
            public string OPD_BDate;
            public string OPD_InTime;
            public string OPD_ROWID;
            public string CP_STS;
            public int CP_CNT;
            public bool CP_SELECT;
            public bool CP_NEW;

            public string ER_PATIENT_InDate;
            public string ER_PATIENT_InTime;
            public string ER_PATIENT_CPNO;
            public string ER_PATIENT_ROWID;
        }
        public OCS_CP_RECORD CPOCR;
        
        public void Clear_OCS_CP_RECORD(ref clsOrderEtc.OCS_CP_RECORD OCR)
        {
            OCR.CPNO = 0;
            OCR.PtNo = "";
            OCR.sName = "";
            OCR.GbIO = "";
            OCR.Age = 0;
            OCR.Sex = "";
            OCR.DeptCode = "";
            OCR.CP_DEPT = "";  //CP코드의 과
            OCR.Bi = "";
            OCR.InTime = "";
            OCR.BDate = "";
            OCR.CP_CODE = "";
            OCR.CP_Name = "";
            OCR.CP_SAYU = "";
            OCR.CP_ROWID = "";
            OCR.OPD_BDate = "";
            OCR.OPD_InTime = "";
            OCR.OPD_ROWID = "";
            OCR.CP_STS = "";
            OCR.CP_CNT = 0;
            OCR.CP_SELECT = false;
            OCR.CP_NEW = false;

            OCR.ER_PATIENT_InDate = "";
            OCR.ER_PATIENT_InTime = "";
            OCR.ER_PATIENT_CPNO = "";
            OCR.ER_PATIENT_ROWID = "";
        }

        /// <summary>
        /// CP 처방명 가져오기
        /// </summary>
        /// <param name="strCode">CP CODE</param>
        /// <param name="strBaseName">구분자</param>
        /// <returns>CP 처방명</returns>
        public string READ_CP_NAME(string strCode, string strBaseName)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     BasCd, BasName ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD ";
                SQL = SQL + ComNum.VBLF + "     WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "         AND GRPCDB = 'CP관리' ";
                SQL = SQL + ComNum.VBLF + "         AND GRPCD = 'CP코드관리' ";
                SQL = SQL + ComNum.VBLF + "         AND BasName1 = '" + strBaseName + "' ";
                SQL = SQL + ComNum.VBLF + "         AND BasCd = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["BASNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public void Read_ERPat_Info(ref OCS_CP_RECORD OCR)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //pat.Mst_ROWID
                //내원 INTIME 체크
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     Pano, DeptCode, Bi, SName, Sex, Age, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(JTIME,'HH24:MI') AS JTIME, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + "     WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + OCR.PtNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND ROWID = '" + OCR.OPD_ROWID + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    OCR.sName = dt.Rows[0]["SNAME"].ToString().Trim();
                    OCR.Age = (int)VB.Val(dt.Rows[0]["AGE"].ToString().Trim());
                    OCR.Sex = dt.Rows[0]["SEX"].ToString().Trim();
                    OCR.OPD_InTime = dt.Rows[0]["JTIME"].ToString().Trim();
                    OCR.BDate = dt.Rows[0]["BDATE"].ToString().Trim();
                    OCR.DeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    OCR.Bi = dt.Rows[0]["BI"].ToString().Trim();
                    OCR.CP_STS = OCR.PtNo + " " + dt.Rows[0]["SNAME"].ToString().Trim() + " CP 상태 (";
                }

                dt.Dispose();
                dt = null;

                //nur_er_patient 체크
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PANO, CPNO, TO_CHAR(INTIME,'YYYY-MM-DD') AS INDATE, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(INTIME, 'HH24:MI') AS INTIME, ";
                SQL = SQL + ComNum.VBLF + "     ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_ER_PATIENT ";
                SQL = SQL + ComNum.VBLF + "     WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "         AND Pano = '" + OCR.PtNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND JDATE = TO_DATE('" + OCR.BDate + "','YYYY-MM-DD')";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    OCR.ER_PATIENT_InDate = dt.Rows[0]["INDATE"].ToString().Trim();
                    OCR.ER_PATIENT_InTime = dt.Rows[0]["INTIME"].ToString().Trim();
                    OCR.ER_PATIENT_CPNO = dt.Rows[0]["CPNO"].ToString().Trim();
                    OCR.ER_PATIENT_ROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        public bool CP_ER_Save(ref OCS_CP_RECORD OCR, string strJob, string strCPCode)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "");
            string strTIME = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", "");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (strJob == "CP activation" || strJob == "CP deactivation" || strJob == "예비 CP" || strJob == "시술"
                    || strJob == "SMS 예비 CP" || strJob == "SMS CP activation" || strJob == "SMS CP deactivation" || strJob == "SMS 시술")
                {
                    rtnVal = true;

                    OCR.CP_CODE = strCPCode;
                    OCR.CP_SAYU = "99";

                    //CP등록
                    if (strJob == "CP activation")
                    {
                        if (OCR.CP_CODE == "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("CP명칭이 공란입니다.");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (OCR.CP_ROWID != "")
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_CP_RECORD";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         CPCODE = '" + OCR.CP_CODE + "' ";
                            SQL = SQL + ComNum.VBLF + "         ,StartDate = '" + strDate + "' ";
                            SQL = SQL + ComNum.VBLF + "         ,StartTime = '" + strTIME + "' ";
                            SQL = SQL + ComNum.VBLF + "         ,StartSabun = '" + clsType.User.Sabun + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE 1 = 1 ";
                            SQL = SQL + ComNum.VBLF + "     AND ROWID = '" + OCR.CP_ROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                        else
                        {
                            OCR.CPNO = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_MED.Replace(".", ""), "SEQ_CPNO");

                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_CP_RECORD";
                            SQL = SQL + ComNum.VBLF + "     (CPNO, PTNO, GBIO, BDATE, DEPTCODE, BI, Sex, Age, INTIME, ";
                            SQL = SQL + ComNum.VBLF + "     CPCODE, PtName, DropGb, StartDate, StartTime, StartSabun)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";
                            SQL = SQL + ComNum.VBLF + "         " + OCR.CPNO + ", ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.PtNo + "', ";
                            SQL = SQL + ComNum.VBLF + "         'E', ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.BDate + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.DeptCode + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.Bi + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.Sex + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.Age + "', ";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + OCR.ER_PATIENT_InDate + " " + OCR.ER_PATIENT_InTime + "','YYYY-MM-DD HH24:MI'), ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.CP_CODE + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.sName + "', ";
                            SQL = SQL + ComNum.VBLF + "         '00', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strDate + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strTIME + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + clsType.User.Sabun + "' ";
                            SQL = SQL + ComNum.VBLF + "     )";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }

                        ComFunc.MsgBox("CP activation 정상 처리됨");
                    }
                    //CP해지
                    else if (strJob == "CP deactivation")
                    {
                        if (OCR.CP_ROWID != "")
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_CP_RECORD";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         DropCD = '" + OCR.CP_SAYU + "' ";
                            SQL = SQL + ComNum.VBLF + "         ,DropDate = '" + strDate + "' ";
                            SQL = SQL + ComNum.VBLF + "         ,DropTime = '" + strTIME + "' ";
                            SQL = SQL + ComNum.VBLF + "         ,DropSabun = '" + clsType.User.Sabun + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE 1 = 1 ";
                            SQL = SQL + ComNum.VBLF + "     AND ROWID = '" + OCR.CP_ROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            ComFunc.MsgBox("CP deactivation 정상 처리됨");
                        }
                    }
                    //예비 CP
                    else if (strJob == "예비 CP")
                    {
                        if (OCR.CP_ROWID != "")
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_CP_RECORD";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         WarmDate = '" + strDate + "' ";
                            SQL = SQL + ComNum.VBLF + "         ,WarmTime = '" + strTIME + "' ";
                            SQL = SQL + ComNum.VBLF + "         ,WarmSabun = '" + clsType.User.Sabun + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE 1 = 1 ";
                            SQL = SQL + ComNum.VBLF + "     AND ROWID ='" + OCR.CP_ROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            ComFunc.MsgBox("예비 CP 정상 처리됨");
                        }
                        else
                        {
                            OCR.CPNO = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_MED.Replace(".", ""), "SEQ_CPNO");

                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_CP_RECORD";
                            SQL = SQL + ComNum.VBLF + "     (CPNO, PTNO, GBIO, BDATE, DEPTCODE, BI, Sex, Age, INTIME, ";
                            SQL = SQL + ComNum.VBLF + "     CPCODE, PtName, DropGb, WarmDate, WarmTime, WarmSabun)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";
                            SQL = SQL + ComNum.VBLF + "         " + OCR.CPNO + ", ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.PtNo + "', ";
                            SQL = SQL + ComNum.VBLF + "         'E', ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.BDate + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.DeptCode + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.Bi + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.Sex + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.Age + "', ";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + OCR.ER_PATIENT_InDate + " " + OCR.ER_PATIENT_InTime + "','YYYY-MM-DD HH24:MI'), ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.CP_CODE + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.sName + "', ";
                            SQL = SQL + ComNum.VBLF + "         '00', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strDate + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strTIME + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + clsType.User.Sabun + "' ";
                            SQL = SQL + ComNum.VBLF + "     )";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            ComFunc.MsgBox("예비 CP 정상 처리됨");
                        }
                    }
                    //시술
                    else if (strJob == "시술")
                    {
                        if (OCR.CP_ROWID != "")
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_CP_RECORD";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         ActDate = '" + strDate + "' ";
                            SQL = SQL + ComNum.VBLF + "         ,ActTime = '" + strTIME + "' ";
                            SQL = SQL + ComNum.VBLF + "         ,ActSabun = '" + clsType.User.Sabun + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE 1 = 1 ";
                            SQL = SQL + ComNum.VBLF + "     AND ROWID = '" + OCR.CP_ROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            ComFunc.MsgBox("시술 정상 처리됨");
                        }
                    }
                    //예비 CP
                    else if (strJob == "SMS 예비 CP")
                    {
                        if (OCR.CP_ROWID != "")
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_CP_RECORD";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         Call_Warm_Date = '" + strDate + "' ";
                            SQL = SQL + ComNum.VBLF + "         ,Call_Warm_Time = '" + strTIME + "' ";
                            SQL = SQL + ComNum.VBLF + "         ,Call_Warm_Sabun = '" + clsType.User.Sabun + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE 1 = 1 ";
                            SQL = SQL + ComNum.VBLF + "     AND ROWID = '" + OCR.CP_ROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }
                    else if (strJob == "SMS CP activation")
                    {
                        if (OCR.CP_ROWID != "")
                        {
                            SQL = "";
                            SQL = " UPDATE KOSMOS_OCS.OCS_CP_RECORD SET";
                            SQL = SQL + ComNum.VBLF + "  Call_Acti_Date ='" + strDate + "' ";
                            SQL = SQL + ComNum.VBLF + "  ,Call_Acti_Time ='" + strTIME + "' ";
                            SQL = SQL + ComNum.VBLF + "  ,Call_Acti_Sabun ='" + clsType.User.Sabun + "' ";
                            SQL = SQL + ComNum.VBLF + " WHERE 1=1 ";
                            SQL = SQL + ComNum.VBLF + "  AND ROWID ='" + OCR.CP_ROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }
                    else if (strJob == "SMS CP deactivation")
                    {
                        if (OCR.CP_ROWID != "")
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_CP_RECORD";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         Call_DeActi_Date = '" + strDate + "' ";
                            SQL = SQL + ComNum.VBLF + "         ,Call_DeActi_Time = '" + strTIME + "' ";
                            SQL = SQL + ComNum.VBLF + "         ,Call_DeActi_Sabun = '" + clsType.User.Sabun + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE 1 = 1 ";
                            SQL = SQL + ComNum.VBLF + "     AND ROWID = '" + OCR.CP_ROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }
                    else if (strJob == "SMS 시술")
                    {
                        if (OCR.CP_ROWID != "")
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_CP_RECORD";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         Call_Act_Date = '" + strDate + "' ";
                            SQL = SQL + ComNum.VBLF + "         ,Call_Act_Time = '" + strTIME + "' ";
                            SQL = SQL + ComNum.VBLF + "         ,Call_Act_Sabun = '" + clsType.User.Sabun + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE 1 = 1 ";
                            SQL = SQL + ComNum.VBLF + "     AND ROWID = '" + OCR.CP_ROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        public bool SEND_DeActivation_SMS(string strPano, double dblCpNo, string strSendMsg)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                SQL = SQL + ComNum.VBLF + "     (Pano, SName, JobDate, Hphone, Gubun, DeptCode, ";
                SQL = SQL + ComNum.VBLF + "     Rettel, SendMsg, Bigo, EntSabun, EntDate)";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     Pano, '02', SYSDATE, Hphone, Gubun, DeptCode,";
                SQL = SQL + ComNum.VBLF + "     Rettel, '" + strSendMsg + "', Bigo, " + clsType.User.Sabun + ", SYSDATE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_SMS ";
                SQL = SQL + ComNum.VBLF + "     WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND SNAME = '01'  ";
                SQL = SQL + ComNum.VBLF + "         AND Bigo = '" + dblCpNo + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS_CP";
                SQL = SQL + ComNum.VBLF + "     (Pano, SName, JobDate, Hphone, Gubun, DeptCode, ";
                SQL = SQL + ComNum.VBLF + "     Rettel, SendMsg, Bigo, EntSabun, EntDate)";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     Pano, '02', SYSDATE, Hphone, Gubun, DeptCode, ";
                SQL = SQL + ComNum.VBLF + "     Rettel, '" + strSendMsg + "', Bigo, " + clsType.User.Sabun + ", SYSDATE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_SMS ";
                SQL = SQL + ComNum.VBLF + "     WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND SNAME = '01'  ";
                SQL = SQL + ComNum.VBLF + "         AND Bigo = '" + dblCpNo + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("CP 등록된 문자 취소 자동 전송되었습니다.");
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        public DataTable Read_CP_ReCord_Chk(OCS_CP_RECORD OCR)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            SQL = "";
            SQL = "SELECT";
            SQL = SQL + ComNum.VBLF + "     CPNO, Ptno, GbIO, DeptCode, Bi, ";
            SQL = SQL + ComNum.VBLF + "     CpCode, ROWID, ";
            SQL = SQL + ComNum.VBLF + "     TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE, ";
            SQL = SQL + ComNum.VBLF + "     TO_CHAR(InTIME,'YYYY-MM-DD HH24:MI') AS InTIME, ";
            SQL = SQL + ComNum.VBLF + "     WarmDate, WarmTime, WarmSabun, ";           //예비CP
            SQL = SQL + ComNum.VBLF + "     StartDate, StartTime, StartSabun, ";        //CP등록
            SQL = SQL + ComNum.VBLF + "     ActDate, ActTime, ActSabun, ";              //시술
            SQL = SQL + ComNum.VBLF + "     DropDate, DropTime, DropSabun, ";           //CP제외
            SQL = SQL + ComNum.VBLF + "     CancerDate, CancerTime, CancerSabun, ";     //CP중단
            SQL = SQL + ComNum.VBLF + "     CallDate, CallTime, CallSabun ";            //의사콜
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_CP_RECORD ";
            SQL = SQL + ComNum.VBLF + "     WHERE 1 = 1 ";
            SQL = SQL + ComNum.VBLF + "         AND PTNO = '" + OCR.PtNo + "' ";
            SQL = SQL + ComNum.VBLF + "         AND BDate = TO_DATE('" + OCR.BDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "         AND GbIO = 'E' ";
            SQL = SQL + ComNum.VBLF + "         AND InTime = TO_DATE('" + OCR.ER_PATIENT_InDate + " " + OCR.ER_PATIENT_InTime + "','YYYY-MM-DD HH24:MI') ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return dt;
            }

            return dt;
        }

        public static int fn_DATE_ILSU(PsmhDb pDbCon, string ArgTdate, string ArgFdate, string ArgGb = "")
        {
            int rtnVal = 0;
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            if (VB.Len(ArgFdate.Trim()) != 10 || VB.IsDate(ArgFdate) == false
                                              || VB.Len(ArgTdate.Trim()) != 10 || VB.IsDate(ArgFdate) == false)
            {
                return rtnVal;
            }

            if (String.Compare(ArgFdate, ArgTdate) > 0)
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  TO_DATE('" + ArgTdate + "','YYYY-MM-DD') - ";
                SQL += ComNum.VBLF + "  TO_DATE('" + ArgFdate + "','YYYY-MM-DD') Gigan";
                SQL += ComNum.VBLF + "FROM DUAL";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).To<int>();
                }

                if (ArgGb != "ALL")
                {
                    if (rtnVal >= 1000) // 일수 계산 제한 옵션으로 풀도록 함수 수정
                    {
                        rtnVal = 999;
                    }
                }

                reader.Dispose();
                reader = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }


            return rtnVal;
        }

        /// <summary>
        /// 처방의 ITEMCD 가져오기 : 외래/입원/응급실 중복 제거
        /// 2019-02-01
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strOrdCode"></param>
        /// <returns></returns>
        public static string fn_Get_ItemCD(PsmhDb pDbCon, string strOrdCode)
        {
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT ITEMCD                                  \r";
                SQL += "   FROM KOSMOS_OCS.OCS_OrderCode                \r";
                SQL += "  WHERE OrderCode = '" + strOrdCode.Trim() + "' \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).To<string>();
                }
                reader.Dispose();
                reader = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// 처방의 기본 용량 가지고 오기
        /// 2019-02-01 박웅규
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strOrdCode"></param>
        /// <returns></returns>
        public static double fn_Get_BContents(PsmhDb pDbCon, string strOrdCode)
        {
            string SQL = "";
            string SqlErr = "";
            double rtnVal = 0;
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT NEXTCODE AS BCONTENTS                                 \r";
                SQL += "   FROM KOSMOS_OCS.OCS_OrderCode                \r";
                SQL += "  WHERE OrderCode = '" + strOrdCode.Trim() + "' \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).To<double>();
                }
                reader.Dispose();
                reader = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// 산정특례 특정기호 비교체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strBi"></param>
        /// <param name="strVCode"></param>
        /// <param name="ssSpread"></param>
        /// <returns></returns>
        public static string ILLSH_CHK_VCODE(PsmhDb pDbCon, string strIO, string strBdate, string strBi, FarPoint.Win.Spread.FpSpread ssSpread, FarPoint.Win.Spread.FpSpread ssOrder)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "OK";
            int i = 0;
            int j = 0;

            string strIll = "";
            string strGUBUN = "";
            string strCheck = "";

            if (strBi == "11" || strBi == "12" || strBi == "13")
            {
                strGUBUN = "3";
            }
            else if (strBi == "21" || strBi == "22")
            {
                strGUBUN = "4";
            }
            else
            {
                return rtnVal;
            }

            try
            {
                for (i = 0; i < ssSpread.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (strIO == "OPD")
                    {
                        strIll = VB.UCase(ssSpread.ActiveSheet.Cells[i, 0].Text.Trim());
                    }
                    else if (strIO == "IPD")
                    {
                        if (ssSpread.ActiveSheet.Cells[i, 0].Text.Trim() != strBdate)
                        {
                            continue;
                        }

                        strIll = VB.UCase(ssSpread.ActiveSheet.Cells[i, 2].Text.Trim());
                    }
                    else if (strIO == "ER")
                    {
                        strIll = VB.UCase(ssSpread.ActiveSheet.Cells[i, 2].Text.Trim());
                    }
                    

                    SQL = "";                    
                    SQL = SQL + ComNum.VBLF + "SELECT A.*, B.* ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_ILLS A ";
                    SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_PMPA.BAS_ILLS_H2 B ";
                    SQL = SQL + ComNum.VBLF + "    ON A.ILLCODE = REPLACE(B.ILLCODE, '.', '') ";
                    SQL = SQL + ComNum.VBLF + "   AND (A.GBVCODE1 = '*' OR A.GBVCODE2 = '*') ";
                    SQL = SQL + ComNum.VBLF + " WHERE B.PART IN ('1', '2') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.ILLCODE = '" + strIll + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND B.GUBUN = '" + strGUBUN + "' ";
                    //SQL = SQL + ComNum.VBLF + "   AND VCODE = '" + strVCode + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strCheck = "";

                        for (j = 0; j < ssOrder.ActiveSheet.NonEmptyRowCount; j++)
                        {
                            if (strIO == "OPD")
                            {
                                if (VB.Left(ssOrder.ActiveSheet.Cells[j, (int)BaseOrderInfo.OpdOrderCol.SUCODE].Text.Trim(), 2) == "@V")
                                {
                                    if (dt.Rows[0]["VCODE"].ToString().Trim() == ssOrder.ActiveSheet.Cells[j, (int)BaseOrderInfo.OpdOrderCol.SUCODE].Text.Replace("@", "").Trim())
                                    {
                                        strCheck = "OK";
                                        break;
                                    }
                                }
                            }
                            else if (strIO == "IPD")
                            {
                                if (VB.Left(ssOrder.ActiveSheet.Cells[j, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text.Trim(), 2) == "@V")
                                {
                                    if (dt.Rows[0]["VCODE"].ToString().Trim() == ssOrder.ActiveSheet.Cells[j, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text.Replace("@", "").Trim())
                                    {
                                        strCheck = "OK";
                                        break;
                                    }
                                }
                            }
                            else if (strIO == "ER")
                            {
                                if (VB.Left(ssOrder.ActiveSheet.Cells[j, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text.Trim(), 2) == "@V")
                                {
                                    if (dt.Rows[0]["VCODE"].ToString().Trim() == ssOrder.ActiveSheet.Cells[j, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text.Replace("@", "").Trim())
                                    {
                                        strCheck = "OK";
                                        break;
                                    }
                                }
                            }                                                     
                        }

                        if (strCheck != "OK")
                        {
                            rtnVal = "NO";
                            ComFunc.MsgBox("산정특례 기호가 일치하지 않습니다." + ComNum.VBLF + strIll + " 상병의 산정특례 특정기호는 " + dt.Rows[0]["VCODE"].ToString().Trim() + " 입니다.","심사팀 [☎8035]");
                        }                        
                    }

                    dt.Dispose();
                    dt = null;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }
    }
}
