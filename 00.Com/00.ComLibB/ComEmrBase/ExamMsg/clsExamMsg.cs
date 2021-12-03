using ComBase; //기본 클래스
using ComDbB; //DB연결
using System;
using System.Data;

namespace ComEmrBase
{
    public class clsExamMsg
    {
        int intRowAffected = 0; //변경된 Row 받는 변수
        string SQL = string.Empty;

        /// <summary>2017.05.30.김홍록:CV체크</summary>
        public enum enmSelExamResultcCvMsg { SABUN, SUBCODE, ROWID, MSG, PANO, HTEL, GBN, IPDOPD, TEL };

        public enum enmSel_EXAM_SMS { MSG, SEND_TEL, EXAM_TEL, PANO };
        /// <summary>2017.06.02.김홍록:의사스케쥴</summary>
        public enum enmSelOcsDoctorSch { SABUN, SETSABUN, HTEL };


        /// <summary>Exam_SMS_SEND_CV</summary>
        /// <param name="strSpecNo"></param>
        /// <param name="strMsg"></param>
        /// <param name="strPart"></param>
        /// <param name="isMid"></param>
        public string Exam_SMS_SEND_CV(PsmhDb pDbCon, string strSpecNo, string strMsg, string strPart, bool isMid, string isSMS = "")
        {
            DataTable dt;
            DataTable dtDrSch;

            string SqlErr = "";

            try
            {
                //1. CV ='C'인 것 전송, 간호사에게 전송
                SqlErr = ins_EXAM_RESULTC_CV_3(pDbCon, strSpecNo, ref intRowAffected);

                if (SqlErr != "")
                {
                    return SqlErr;
                }

                //2. isMid = false 일때만 CV ='C' 주치의에게
                dt = sel_EXAM_RESUlTC_CV_MSG(pDbCon, strSpecNo, strMsg, isMid, isSMS);

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SqlErr = Exam_SMS_SEND_CV_SMS(pDbCon
                                            , "1"  // 주치의
                                            , dt.Rows[i][enmSelExamResultcCvMsg.ROWID.ToString()].ToString().Trim()
                                            , dt.Rows[i][enmSelExamResultcCvMsg.SABUN.ToString()].ToString().Trim()
                                            , dt.Rows[i][enmSelExamResultcCvMsg.PANO.ToString()].ToString().Trim()
                                            , dt.Rows[i][enmSelExamResultcCvMsg.HTEL.ToString()].ToString().Trim()
                                            , dt.Rows[i][enmSelExamResultcCvMsg.TEL.ToString()].ToString().Trim()
                                            , dt.Rows[i][enmSelExamResultcCvMsg.MSG.ToString()].ToString().Trim()
                                            , isMid
                                            );
                        if (SqlErr != "")
                        {
                            return SqlErr;
                        }

                        dtDrSch = sel_OCS_DOCTOR_SCH(pDbCon, dt.Rows[i][enmSelExamResultcCvMsg.SABUN.ToString()].ToString().Trim());

                        if (dtDrSch != null && dtDrSch.Rows.Count > 0)
                        {
                            for (int z = 0; z < dtDrSch.Rows.Count; z++) //2019-04-29, 김해수 , CVR 문자 추가 작업
                            {
                                SqlErr = Exam_SMS_SEND_CV_SMS(pDbCon
                                              , "2"   // 전공의
                                              , dt.Rows[i][enmSelExamResultcCvMsg.ROWID.ToString()].ToString().Trim()
                                              , dtDrSch.Rows[z][enmSelOcsDoctorSch.SETSABUN.ToString()].ToString().Trim()
                                              , dt.Rows[i][enmSelExamResultcCvMsg.PANO.ToString()].ToString().Trim()
                                              , dtDrSch.Rows[z][enmSelOcsDoctorSch.HTEL.ToString()].ToString().Trim()
                                              , dt.Rows[i][enmSelExamResultcCvMsg.TEL.ToString()].ToString().Trim()
                                              , dt.Rows[i][enmSelExamResultcCvMsg.MSG.ToString()].ToString().Trim()
                                              , isMid
                                              );
                                if (SqlErr != "")
                                {
                                    return SqlErr;
                                }
                            }

                            dtDrSch.Dispose();
                            dtDrSch = null;
                        }

                        if (strPart == "Y" && isMid == false)
                        {
                            //감염 김은정
                            SqlErr = ins_ETC_SMS(pDbCon, dt.Rows[i][enmSelExamResultcCvMsg.PANO.ToString()].ToString().Trim()
                                            , "01066052045", "054-260-8261", "N", dt.Rows[i][enmSelExamResultcCvMsg.MSG.ToString()].ToString().Trim(), ref intRowAffected);
                            if (SqlErr != "")
                            {
                                return SqlErr;
                            }

                            //감염 고수현
                            ins_ETC_SMS(pDbCon, dt.Rows[i][enmSelExamResultcCvMsg.PANO.ToString()].ToString().Trim()
                                            , "01027764163", "054-260-8261", "N", dt.Rows[i][enmSelExamResultcCvMsg.MSG.ToString()].ToString().Trim(), ref intRowAffected);

                            if (SqlErr != "")
                            {
                                return SqlErr;
                            }

                            //감염 박수진   
                            ins_ETC_SMS(pDbCon, dt.Rows[i][enmSelExamResultcCvMsg.PANO.ToString()].ToString().Trim()
                                            , "01028170176", "054-260-8261", "N", dt.Rows[i][enmSelExamResultcCvMsg.MSG.ToString()].ToString().Trim(), ref intRowAffected);

                            if (SqlErr != "")
                            {
                                return SqlErr;
                            }

                            //감염 정지미   
                            ins_ETC_SMS(pDbCon, dt.Rows[i][enmSelExamResultcCvMsg.PANO.ToString()].ToString().Trim()
                                            , "01065827819", "054-260-8261", "N", dt.Rows[i][enmSelExamResultcCvMsg.MSG.ToString()].ToString().Trim(), ref intRowAffected);

                            if (SqlErr != "")
                            {
                                return SqlErr;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                if (SqlErr != "")
                {
                    return ex.Message.ToString();
                }
            }

            return SqlErr;
        }
        public string ins_EXAM_RESULTC_CV_3(PsmhDb pDbCon, string strSpecNo, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " INSERT INTO KOSMOS_OCS.EXAM_RESULTC_CV(JOBDATE, GBN, SPECNO, Pano, MASTERCODE, SUBCODE, Result, RESULTDATE, RESULTSABUN, UNIT) \r\n";
            SQL += " SELECT TRUNC(SYSDATE), '3', C.SPECNO, C.PANO, C.MASTERCODE, C.SUBCODE, C.RESULT, C.RESULTDATE, C.RESULTSABUN, C.UNIT           \r\n";
            SQL += " FROM  " + ComNum.DB_MED + "EXAM_RESULTC C                                                                                      \r\n";
            SQL += "     , " + ComNum.DB_MED + "EXAM_SPECMST M                                                                                      \r\n";
            SQL += " WHERE 1 = 1                                                                                                                    \r\n";
            SQL += "   AND C.SPECNO = M.SPECNO                                                                                                      \r\n";
            SQL += "   AND C.SPECNO = " + ComFunc.covSqlstr(strSpecNo, false);
            SQL += "   AND M.IPDOPD = 'I'                                                                                                           \r\n";
            SQL += "   AND C.CV = 'C'                                                                                                               \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            if (string.IsNullOrEmpty(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            }


            return SqlErr;

        }
        public DataTable sel_EXAM_RESUlTC_CV_MSG(PsmhDb pDbCon, string strSpecNo, string strMsg, bool isMid, string isSMS)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                                                                     \r\n";
            SQL += "        B.SABUN                                                                                                                                             \r\n";
            SQL += "      , C.SUBCODE                                                                                                                                           \r\n";
            SQL += "      , C.ROWID                                                                                                                                             \r\n";
            if (isMid == false)
            {
                //SQL += "      , CASE WHEN LENGTH('" + strMsg.Trim() + "') > 0  THEN '" + strMsg.Trim() + "' || A.PANO || '/' || A.SNAME || '/' || D.EXAMYNAME                              \r\n";
                //2018-10-23 안정수, 김은정t 요청으로 문자양식 변경함 
                //SQL += "      , CASE WHEN LENGTH('" + strMsg.Trim() + "') > 0  THEN '" + strMsg.Trim() + "' || '/ ' || A.DEPTCODE || '/ ' || A.PANO || '/ ' || A.SNAME || '/ ' || TO_CHAR(A.RECEIVEDATE, 'YYYY.MM.DD') || '/ ' || KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',A.SPECCODE,'N') || '/ ' || C.RESULT  \r\n";
                //2019-12-24 안정수, 김은정t 요청으로 문자양식 변경함(외래, 입원 구분 추가) 
                SQL += "      , CASE WHEN LENGTH('" + strMsg.Trim() + "') > 0  THEN '" + strMsg.Trim() + "' || '/ ' || DECODE(A.IPDOPD, 'O', 'O', 'I', 'I' || ' / ' || A.ROOM) || '/ ' || A.DEPTCODE || '/ ' || A.PANO || '/ ' || A.SNAME || '/ ' || TO_CHAR(A.RECEIVEDATE, 'YYYY.MM.DD') || '/ ' || KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',A.SPECCODE,'N') || '/ ' || C.RESULT  \r\n";
                //SQL += "              ELSE '★긴급(CV)★' || DECODE(TRIM(A.WARD), '', '', A.WARD || '병동') || A.PANO || '/' || A.SNAME || '/' || d.EXAMYNAME || '/' || C.RESULT    \r\n";
                //2020-02-13 안정수, 약어 누락되는 경우 있어 조건 추가함
                SQL += "              ELSE CASE WHEN D.EXAMYNAME <> '' THEN '★긴급(CV)★' || DECODE(TRIM(A.WARD), '', '', A.WARD || '병동') || A.PANO || '/' || A.SNAME || '/' || D.EXAMYNAME || '/' || C.RESULT    \r\n";
                SQL += "                   ELSE '★긴급(CV)★' || DECODE(TRIM(A.WARD), '', '', A.WARD || '병동') || A.PANO || '/' || A.SNAME || '/' || D.EXAMNAME || '/' || C.RESULT    \r\n";
                SQL += "                    END                                                                                                                                  \r\n";
                SQL += "         END           AS MSG                                                                                                                            \r\n";
            }
            else
            {
                //SQL += "      , '★긴급(CV)중간보고★' || DECODE(TRIM(A.WARD), '', '', A.WARD || '병동') || A.PANO || '/' || A.SNAME || '/' || d.EXAMYNAME || '/' || '" + strMsg + "' AS MSG \r\n";
                //2018.03.14.김홍록: 중간 보고의 msg는 결과값이였다.
                //SQL += "      , '★긴급(CV)중간보고★' || DECODE(TRIM(A.WARD), '', '', A.WARD || '병동') || A.PANO || '/' || A.SNAME || '/' || d.EXAMYNAME || '/' || C.RESULT  AS MSG \r\n";
                SQL += "      , CASE WHEN D.EXAMYNAME <> '' THEN '★긴급(CV)중간보고★' || DECODE(TRIM(A.WARD), '', '', A.WARD || '병동') || A.PANO || '/' || A.SNAME || '/' || d.EXAMYNAME || '/' || C.RESULT  \r\n";
                SQL += "             ELSE '★긴급(CV)중간보고★' || DECODE(TRIM(A.WARD), '', '', A.WARD || '병동') || A.PANO || '/' || A.SNAME || '/' || d.EXAMNAME || '/' || C.RESULT   \r\n";
                SQL += "         END           AS MSG                                                                                                                            \r\n";
            }
            SQL += "      , A.PANO                                                                                                                                              \r\n";
            SQL += "      , I.HTEL                                                                                                                                              \r\n";
            SQL += "      ,'1'                  AS GBN                                                                                                                           \r\n";
            SQL += "      , A.IPDOPD                                                                                                                                            \r\n";
            SQL += "      , CASE WHEN TRIM(C.RESULTWS) = 'B' THEN '054-260-8258'                                                                                                \r\n";
            SQL += "             WHEN TRIM(C.RESULTWS) = 'C' THEN '054-260-8259'                                                                                                \r\n";
            SQL += "             WHEN TRIM(C.RESULTWS) = 'H' THEN '054-260-8260'                                                                                                \r\n";
            SQL += "             WHEN TRIM(C.RESULTWS) = 'E' OR TRIM(C.RESULTWS) = 'W' THEN '054-260-8261'                                                                      \r\n";
            SQL += "             WHEN TRIM(C.RESULTWS) = 'S' THEN '054-260-8261'                                                                                                \r\n";
            SQL += "             WHEN TRIM(C.RESULTWS) = 'U' THEN '054-260-8258'                                                                                                \r\n";
            SQL += "             WHEN TRIM(C.RESULTWS) = 'M' THEN '054-260-8262'                                                                                                \r\n";
            SQL += "         ELSE '054-260-8261'                                                                                                                                \r\n";
            SQL += "         END TEL                                                                                                                                            \r\n";
            SQL += " FROM " + ComNum.DB_MED + "EXAM_SPECMST A                                                                                                                             \r\n";
            SQL += "    , " + ComNum.DB_MED + "EXAM_RESULTC C                                                                                                                             \r\n";
            SQL += "    , " + ComNum.DB_MED + "EXAM_MASTER  D                                                                                                                             \r\n";
            SQL += "    , " + ComNum.DB_MED + "OCS_DOCTOR   B                                                                                                                             \r\n";
            SQL += "    , " + ComNum.DB_ERP + "INSA_MST     I                                                                                                                             \r\n";
            SQL += " WHERE 1 = 1                                                                                                                                                \r\n";
            SQL += " AND A.SPECNO  = " + ComFunc.covSqlstr(strSpecNo, false);
            SQL += " AND A.DRCODE  = B.DRCODE                                                                                                                                   \r\n";
            SQL += " AND B.GBOUT   = 'N'                                                                                                                                        \r\n";
            SQL += " AND B.SABUN   = I.SABUN                                                                                                                                    \r\n";
            SQL += " AND B.SABUN   <> '48748'                                                                                                                                    \r\n"; //2019-11-07 박병화 선생님이 요청
            SQL += " AND A.SPECNO  = C.SPECNO                                                                                                                                   \r\n";
            SQL += " AND C.SUBCODE = D.MASTERCODE                                                                                                                            \r\n";

            if (isMid == false && isSMS == "")
            {
                SQL += " AND C.CV      = 'C'                                                                                                                                        \r\n";
                SQL += " AND C.STATUS  = 'V'                                                                                                                                         \r\n";
            }

            if (isMid == false && isSMS == "Y")
            {
                SQL += " AND C.CV      = 'C'                                                                                                                                        \r\n";
            }

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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
        /// <summary>Exam_SMS_Send_CV_SMS</summary>
        /// <param name="strGbn"></param>
        /// <param name="strRowId"></param>
        /// <param name="strSabun"></param>
        /// <param name="strPano"></param>
        /// <param name="strHpone"></param>
        /// <param name="strTel"></param>
        /// <param name="strGubun"></param>
        /// <param name="strMsg"></param>
        public string Exam_SMS_SEND_CV_SMS(PsmhDb pDbCon, string strGbn, string strRowId, string strSabun, string strPano, string strHpone, string strTel, string strMsg, bool isMid = false)
        {
            string strSMSOK = sel_OCS_DOCTOR(pDbCon, strSabun);
            string SqlErr = "";

            //2019-02-14 전담간호사인 경우 SMSOK 구분이 없으므로 강제 설정함
            string strMSTel = sel_NURSE_MSTEL(pDbCon, strSabun);

            if (strMSTel != "")
            {
                strSMSOK = "Y";         //SMS 수신여부 강제 Y 설정
                strHpone = strMSTel;    //SMS 수신번호는 MSTEL 번호로 설정
                strGbn = "3";           //SMS 구분은 '3' 간호사로 설정
            }

            if (strSMSOK == "Y" && string.IsNullOrEmpty(strHpone) == false)
            {
                try
                {
                    SqlErr = ins_ETC_SMS(pDbCon, strPano, strHpone, strTel, "68", strMsg, ref intRowAffected);

                    if (string.IsNullOrEmpty(SqlErr) == false)
                    {
                        return SqlErr;
                    }

                    // 2018.03.14.김홍록 : isMid는 중간 결과 
                    if (isMid == false)
                    {
                        SqlErr = ins_EXAM_RESULTC_CV(pDbCon, strGbn, strRowId, ref intRowAffected);

                        if (string.IsNullOrEmpty(SqlErr) == false)
                        {
                            return SqlErr;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
            }

            return SqlErr;
        }
        public string sel_OCS_DOCTOR(PsmhDb pDbCon, string strSabun)
        {
            string strReturn = string.Empty;

            DataTable dt = null;

            SQL = "";
            SQL = SQL + " SELECT  SMS_EXAMOK                                \r\n";
            SQL = SQL + "   FROM " + ComNum.DB_MED + "OCS_DOCTOR S          \r\n";
            SQL = SQL + "  WHERE 1=1                                        \r\n";
            SQL = SQL + " 	 AND SABUN  = " + ComFunc.covSqlstr(strSabun, false);
            SQL = SQL + " 	 AND GBOUT  = 'N'";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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
                return "";
            }

            if (dt.Rows.Count > 0)
            {
                strReturn = "Y";
            }
            else
            {
                strReturn = "";
            }

            dt.Dispose();
            dt = null;

            return strReturn;
        }

        /// <summary>
        /// 인사마스터에서 간호사이면 MSTEL 번호 읽음
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strSabun"></param>
        /// <returns></returns>
        public string sel_NURSE_MSTEL(PsmhDb pDbCon, string strSabun)
        {
            string strMSTEL = string.Empty;

            DataTable dt = null;

            SQL = "";
            SQL = SQL + " SELECT MSTEL                                \r\n";
            SQL = SQL + "   FROM " + ComNum.DB_ERP + "INSA_MST        \r\n";
            SQL = SQL + "  WHERE 1=1                                  \r\n";
            SQL = SQL + " 	 AND SABUN  = " + ComFunc.covSqlstr(strSabun, false);
            SQL = SQL + " 	 AND JIKJONG  = '41'    ";  //직종구분 간호사

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strMSTEL;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strMSTEL;
            }

            if (dt.Rows.Count > 0)
            {
                strMSTEL = dt.Rows[0]["MSTEL"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            return strMSTEL;
        }

        /// <summary>ins_ETC_SMS</summary>
        /// <param name="strPano"></param>
        /// <param name="strHpone"></param>
        /// <param name="strTel"></param>
        /// <param name="strGubun"></param>
        /// <param name="strMsg"></param> 
        /// <returns></returns>
        public string ins_ETC_SMS(PsmhDb pDbCon, string strPano, string strHpone, string strTel, string strGubun, string strMsg, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS(JOBDATE, PANO, SNAME, HPHONE, GUBUN, RETTEL, SENDMSG)  VALUES (    \r\n";
            SQL += " SYSDATE                                                                                            \r\n";
            SQL += ComFunc.covSqlstr(strPano, true);
            SQL += ComFunc.covSqlstr("진단검사의학과", true);
            SQL += ComFunc.covSqlstr(strHpone, true);
            SQL += ComFunc.covSqlstr(strGubun, true);
            SQL += ComFunc.covSqlstr(strTel, true);
            SQL += ComFunc.covSqlstr(strMsg, true);
            SQL += " )                                                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            if (string.IsNullOrEmpty(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            }

            return SqlErr;
        }

        /// <summary>ins_EXAM_RESULTC_CV</summary>
        /// <param name="strGbn"></param>
        /// <param name="strRowId"></param>
        /// <returns></returns>
        public string ins_EXAM_RESULTC_CV(PsmhDb pDbCon, string strGbn, string strRowId, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "EXAM_RESULTC_CV ( JOBDATE, GBN,   SPECNO, Pano, MASTERCODE,SUBCODE,Result,RESULTDATE,RESULTSABUN,UNIT , SMSSEND )         \r\n";
            SQL += " SELECT TRUNC(SYSDATE), '" + strGbn + "', SPECNO, Pano, MASTERCODE,SUBCODE,Result,RESULTDATE,RESULTSABUN,UNIT , SYSDATE                                     \r\n";
            SQL += "   FROM  " + ComNum.DB_MED + "EXAM_RESULTC                                                                                                                  \r\n";
            SQL += "  WHERE ROWID = " + ComFunc.covSqlstr(strRowId, false);
            SQL += "    AND CV ='C'                                                                                                                                             \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            if (string.IsNullOrEmpty(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            }


            return SqlErr;

        }
        /// <summary>sel_OCS_DOCTOR_SCH</summary>
        /// <param name="strSabun"></param>
        /// <returns></returns>
        public DataTable sel_OCS_DOCTOR_SCH(PsmhDb pDbCon, string strSabun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT A.SABUN                                         \r\n";
            SQL += "      , A.SETSABUN                                      \r\n";
            SQL += "      , B.HTEL                                          \r\n";
            SQL += " FROM " + ComNum.DB_MED + "OCS_DOCTOR_SCH A                       \r\n";
            SQL += "    , " + ComNum.DB_ERP + "INSA_MST B                             \r\n";
            SQL += " WHERE 1 = 1                                            \r\n";
            SQL += "   AND A.SABUN      = " + ComFunc.covSqlstr(strSabun, false);
            SQL += "   AND A.SETSABUN   = B.SABUN                           \r\n";
            SQL += "   AND A.YYMM = (SELECT MAX(YYMM)                       \r\n";
            SQL += "                   FROM KOSMOS_OCS.OCS_DOCTOR_SCH       \r\n";
            SQL += "                  WHERE SABUN = " + ComFunc.covSqlstr(strSabun, false);
            SQL += "                 )                                      \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

    }
}
