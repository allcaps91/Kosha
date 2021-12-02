using ComBase; //기본 클래스
using ComDbB; //DB연결
using System;
using System.Collections.Generic;
using System.Data;

namespace ComSupLibB.SupBlod
{

    /// <summary>
    /// Class Name              : ComSupLibB.SupLbEx
    /// File Name               : clsSupBlodSQL.cs 
    /// Title or Description    : 혈액은행 SQL
    /// Author                  : 김홍록
    /// Create Date             : 2017-11-29
    /// Update History          : 
    /// </summary>
    /// 
    public class clsComSupBlodSQL : Com.clsMethod
    {

        string SQL = string.Empty;

        public enum enmGBSTATUS { D, R, C, M, A };

        public enum enmSel_EXAM_BLOOD_IO_CHK { CHK, BLOODNO, COMPONENT, CAPACITY, ABO };
        public string[] sSel_EXAM_BLOOD_IO_CHK = { "일치", "혈액번호", "재재코드", "혈액량", "ABO" };
        public int[] nSel_EXAM_BLOOD_IO_CHK = { nCol_WARD, nCol_TIME, nCol_WARD, nCol_WARD, nCol_WARD };

        public enum enmSel_EXAM_BLOOD_COMM { CODE, NAME, CHANGE, ROWID };
        public string[] sSel_EXAM_BLOOD_COMM = { "거래처코드", "거래처명", "변경", "ROWID" };
        public int[] nSel_EXAM_BLOOD_COMM = { nCol_TIME, nCol_TIME, nCol_SCHK, 5 };

        public enum enmSel_EXAM_BLOOD_IO_STOCK { GBSTATUS, SDATE, BLOODNO, COMPONENT, STORE, EXDATE, STOREPERSON, ABO, RH, CAPACITY };
        public string[] sSel_EXAM_BLOOD_IO_STOCK = { "매칭", "입고일자", "혈액번호", "성분", "입고처", "유효일자", "입고자", "ABO", "RH", "혈액량" };
        public int[] nSel_EXAM_BLOOD_IO_STOCK = { nCol_CHEK, nCol_DATE, nCol_JUMN1, nCol_NAME, nCol_JUMN1, nCol_DATE, nCol_CHEK, nCol_IOPD, nCol_SCHK, nCol_WARD };

        public enum enmSel_EXAM_BLOOD_IO_OUT { CHK, ENTDATE, WARD, DEPT, PANO, SNAME, EMR_SCAN, BLOODNO, PTAR, DNAR, COMP, COMPONENT, CAPACITY, ENTSABUN, NURSE };
        public string[] sSel_EXAM_BLOOD_IO_OUT = { "P", "불출일자", "병동", "과", "등록번호", "성명", "스캔", "혈액번호", "PtABO", "DnABO", "교차시험결과", "혈액제제", "혈액량", "불출자", "수령자" };
        public int[] nSel_EXAM_BLOOD_IO_OUT = { nCol_SCHK, nCol_TIME, nCol_AGE, nCol_AGE, nCol_PANO, nCol_SNAME, nCol_SCHK, nCol_JUMN1, nCol_IOPD, nCol_IOPD, nCol_IOPD, nCol_TIME, nCol_IOPD, nCol_IOPD, nCol_IOPD };

        public enum enmSel_EXAM_BLOOD_ID_CROSS { CHK, SNAME, PANO, EMR_SCAN, SEX, AGE, ROOM, ABO_N, SERUM, RH, JOBTYPE, BLOODNO, COMPONENT, CAPACITY, ANTIB, SALINE, ALBUMN, ANTIG, GUMSAJA, GUMSATIME, OUTDATE, NURSE, ROWID };
        public string[] sSel_EXAM_BLOOD_ID_CROSS = { "P", "성명", "등록번호", "스캔", "성별", "나이", "병실", "혈구", "혈청", "RH", "작업구분", "혈액번호", "혈액제제", "혈액량", "AntiBodyScreening", "Saline", "37C Album", "Anti Globuli", "검사자", "혈액검사시각", "혈액불출시각", "수령인", "ROWID" };
        public int[] nSel_EXAM_BLOOD_ID_CROSS = { nCol_SCHK, nCol_PANO, nCol_PANO, nCol_SCHK, nCol_SCHK, nCol_AGE, nCol_AGE, nCol_AGE, nCol_IOPD, nCol_SCHK, nCol_IOPD, nCol_IOPD, nCol_TIME, nCol_IOPD, nCol_PANO + 10, nCol_IOPD + 10, nCol_IOPD + 10, nCol_IOPD, nCol_IOPD, nCol_TIME, nCol_TIME, nCol_IOPD, 5 };

        public enum enmSel_EXAM_BLOOD_IO_JAN { CHK, BLOODNO, COMPONENT, OUTDATE, JANDATE, JANSABUN, ABO, CAPACITY, JANSU, JANPAE, JANREMARK, PANO, SNAME, WARD, ROOM };
        public string[] sSel_EXAM_BLOOD_IO_JAN = { "P", "혈액번호", "혈액제제", "불출일자", "폐기일자", "폐기자", "ABO", "혈액량", "수혈량", "폐기량", "폐기사유", "등록번호", "성명", "병동", "병실" };
        public int[] nSel_EXAM_BLOOD_IO_JAN = { nCol_SCHK, nCol_PANO, nCol_PANO, nCol_DATE, nCol_DATE, nCol_PANO, nCol_AGE, nCol_AGE, nCol_AGE, nCol_AGE, nCol_TIME, nCol_PANO, nCol_PANO, nCol_PANO, nCol_PANO };

        public enum enmSel_EXAM_BLOODTRANS_STORE { BDATE, BLOODNO, ENTSABUN, COMPONENT, CAPACITY };

        public enum enmSel_EXAM_BLOODTRANS_SUM { BLOOD_IN, BLOOD_OUT, BLOOD_MATCH, BLOOD_RANTAL, BLOOD_DESTROY, BLOOD_RETURN, BLOOD_OUT_RETURN, BLOOD_SUM };
        public string[] sSel_EXAM_BLOODTRANS_SUM = { "입고수량", "출고수량", "매칭수량", "대여수량", "폐기수량", "입고반납수량", "출고반납수량", "총수량" };
        public int[] nSel_EXAM_BLOODTRANS_SUM = { nCol_SPNO, nCol_SPNO, nCol_SPNO, nCol_SPNO, nCol_SPNO, nCol_SPNO, nCol_SPNO, nCol_SPNO };

        public enum enmSel_EXAM_BLOOD_IO_ING { SDATE, BLOODNO, COMPONENT, STORE, DRAWDATE, EXDATE, STOREPERSON, ABO, RH, CAPACITY, GBSTATUS, AMT };
        public string[] sSel_EXAM_BLOOD_IO_ING = { "입고일자", "혈액번호", "성분", "입고처", "채혈일자", "유효일자", "입고자", "ABO", "RH", "혈액량", "상태", "단가" };
        public int[] nsel_EXAM_BLOOD_IO_ING = { nCol_DATE, nCol_JUMN1, nCol_NAME, nCol_JUMN1 - 10, nCol_DATE, nCol_DATE, nCol_CHEK, nCol_SCHK + 10, nCol_SCHK, nCol_AGE, nCol_WARD, nCol_WARD };

        public enum enmSel_EXAM_BLOOD_IO_EXCEL { BLOODNO, COM_CAPA, COMPONENT, GBSTATUS, SDATE, DRAWDATE, STORETIME, ABO_EXCEL, ABO, STOREPERSON };
        public string[] sSel_EXAM_BLOOD_IO_EXCEL = { "혈액번호", "혈액제제코드", "혈액제제명", "입고구분", "입고일자", "채혈일자", "입고시간", "혈액형코드", "혈액형명", "입고자" };
        public int[] nsel_EXAM_BLOOD_IO_EXCEL = { nCol_JUMN1, nCol_AGE, nCol_JUMN1, nCol_AGE, nCol_DATE, nCol_DATE, nCol_WARD, nCol_AGE, nCol_AGE, nCol_WARD };

        public enum enmSel_EXAM_BLOODCROSSM_CANCEL { CHK, JOBTYPE, GUMSADATE_FULL, GUMSADATE_FR_FULL, PANO, SNAME, SEX, SPECNO, DEPT, WARD, ROOM, COMPONENT, BLOODNO, CAPACITY, DONNORABO, DONNORRH, PTABO, PTRH, ANTIB, MAJOR, SALINE, ALBUMN, ANTIG, GUMSAJA_NM, CANCAUSE, CNCL_PS, GBSTATUS, ROWID, SEQNO };
        public string[] sSel_EXAM_BLOODCROSSM_CANCEL = { "선택", "작업구분", "종료일시", "시작일시", "등록번호", "환자명", "성별", "검체번호", "진료과", "병동", "병실", "혈액제제", "혈액번호", "혈액량", "DonABO", "DonRh", "PtABO", "PtRh", "AntiBody Screening", "Cross Matching", "Saline", "37℃ Albumin", "Anti Globulin", "검사자", "취소사유", "취소자", "GBSTATUS", "ROWID", "SEQNO" };
        public int[] nSel_EXAM_BLOODCROSSM_CANCEL = { nCol_SCHK, nCol_CHEK, nCol_TIME - 10, nCol_TIME - 10, nCol_PANO - 10, nCol_SNAME - 10, nCol_SCHK, nCol_SPNO - 5, nCol_SCHK + 10, nCol_IOPD - 10, nCol_IOPD - 10, nCol_CHEK + 20, nCol_JUMN1, nCol_CHEK - 30, nCol_CHEK - 20, nCol_CHEK - 20, nCol_DPCD + 10, nCol_CHEK - 20, nCol_CHEK + 40, nCol_WARD + 20, nCol_WARD + 10, nCol_WARD + 20, nCol_WARD + 20, nCol_SNAME, nCol_SNAME, nCol_WARD, 5, 5, 5 };

        public enum enmEXAM_BLOODCROSSM { JUBSUDATE, PANO, ORDERGUBUN, BLOODNO, ORDERNO, PTABO, PTRH, DONNORABO, DONNORRH, MAJOR, MINOR, GUMSADATE, GUMSAHH, GUMSAMM, GUMSAJA, PRINTYN, COMPONENT, GBSTATUS, GBCHA, DEPT, WARD, ROOM, SALINE, ALBUMN, ANTIG, ANTIB, CAPACITY, CANCAUSE, GUMSADATE_FR, GUMSAHH_FR, GUMSAMM_FR, SPECNO, SEQNO, INPS, INPT_DT, UPPS, UPDT, CANCAUSE_CD, CNCL_PS, JOBTYPE };

        public enum enmSel_EXAM_BLOODCROSSM_TAT { CHK, SPECNO, PANO, SNAME, DEPT, WARD, ROOM, COMPONENT, BLOODNO, CAPACITY, DONNORABO, DONNORRH, GUMSADATE_FULL, GUMSADATE_FR_FULL, DIFF, OVERTIME_REMARK };
        public string[] sSel_EXAM_BLOODCROSSM_TAT = { "선택", "검체번호", "등록번호", "환자성명", "진료과", "병동", "병실", "혈액제제", "혈액번호", "혈액량", "ABO", "Rh", "검사종료시간", "검사시작시간", "소요시간(분)", "비고" };
        public int[] nSel_EXAM_BLOODCROSSM_TAT = { nCol_SCHK, nCol_PANO, nCol_PANO, nCol_SNAME - 10, nCol_DPCD, nCol_DPCD, nCol_DPCD, nCol_JUMN1, nCol_PANO, nCol_AGE - 10, nCol_AGE - 10, nCol_AGE - 10, nCol_TIME, nCol_TIME, nCol_PANO, nCol_NAME - 20 };

        public enum enmSel_EXAM_BLOODTRANS_OUT { BDATE, PANO, SNAME, SEX, AGE, WARD, ROOM, PTABO, PTRH, BLOODNO, COMPONENT_NM, CAPACITY, DONNORABO, DNAR, ENTSABUN, NURSE, OUTDATE };
        public string[] sSel_EXAM_BLOODTRANS_OUT = { "불출일자", "등록번호", "환자성명", "성별", "나이", "병실", "병동", "환자ABO", "환자Rh", "혈액번호", "혈액제제", "혈액량", "팩ABO", "팩Rh", "불출자", "수령자", "불출시간" };
        public int[] nSel_EXAM_BLOODTRANS_OUT = { nCol_DATE, nCol_PANO - 10, nCol_SNAME - 10, nCol_SCHK, nCol_AGE - 10, nCol_DPCD - 10, nCol_DPCD, nCol_AGE, nCol_AGE, nCol_PANO + 10, nCol_NAME - 10, nCol_AGE - 10, nCol_AGE, nCol_AGE - 10, nCol_PANO - 10, nCol_PANO - 10, nCol_TIME };

        public enum enmSel_EXAM_BLOODTRANS_EXCEL { BLOODNO, COM_CAPA, COMPONENT, GBSTATUS, BDATE, ENTDATE, ABO_EXCEL, ABO, ENTSABUN, SEX, BIRTH, PTAR, ZIPNAME, DEPT };
        public string[] sSel_EXAM_BLOODTRANS_EXCEL = { "혈액번호", "혈액제제코드", "혈액제제명", "출고구분", "출고일자", "출고시간", "혈액형코드", "혈액형명", "출고자", "수혈자 성별", "출고자 출생년도", "수혈자 혈액형", "수혈자 거주지역", "수혈자 진료과목" };
        public int[] nSel_EXAM_BLOODTRANS_EXCEL = { nCol_PANO + 20, nCol_DPCD + 10, nCol_NAME - 10, nCol_AGE + 10, nCol_DATE, nCol_DATE, nCol_AGE + 20, nCol_AGE, nCol_SNAME, nCol_DPCD + 20, nCol_PANO + 20, nCol_PANO + 20, nCol_PANO + 30, nCol_PANO + 30 };

        public enum enmSel_EXAM_BLOOD_IO_INFO { BLOODNO, ABO, COMPONENT, CAPACITY, EXPIRE, STOREDATE, STOREPERSON, STORE, AMT, DRAWDATE, GBSTATUS, STOREPERSON_NM, DAEYUDATE, DESTROYDATE, JANDATE, OUTDATE, OUTPERSON, OUTPERSON_NM, PANO, SNAME, DEPT, WARD, ROOM, DR, DR_NM, NURSE, NURSE_NM };

        public enum enmSel_EXAM_BLOOD_IO { SDATE, BLOODNO, COMPONENT, STORE, EXDATE, STOREPERSON, ABO, RH, CAPACITY, STORERDATE, STORERPERSON, STORERCAUSE, DAEYUDATE, DAEYU, DAEYU_SABUN, DESTROYDATE, DESTROYPERSON, DESTROYCAUSE, JANDATE, JANSU, JANPAE, JANSABUN, JANREMARK, OUTRDATE, OUTRPERSON, OUTRCASE, PANO, SNAME };
        public string[] sSel_EXAM_BLOOD_IO = { "입고일자", "혈액번호", "성분", "입고처", "유효일자", "입고자", "ABO", "RH", "혈액량", "반납일자", "반납자", "반납사유", "대여일자", "대여처", "대여자", "폐기일자", "폐기자", "폐기사유", "잔량폐기일자", "잔량사용량", "잔량폐기량", "잔량폐기자", "잔량폐기사유", "출고반납일자", "출고반납자", "출고반납사유", "등록번호", "환자성명" };
        public int[] nSel_EXAM_BLOOD_IO = { nCol_DATE, nCol_JUMN1, nCol_NAME, nCol_JUMN1 - 10, nCol_DATE, nCol_CHEK, nCol_SCHK + 10, nCol_SCHK, nCol_AGE, nCol_DATE, nCol_CHEK, nCol_NAME, nCol_DATE, nCol_DATE, nCol_CHEK, nCol_DATE, nCol_CHEK, nCol_NAME, nCol_DATE, nCol_CHEK + 10, nCol_CHEK + 10, nCol_CHEK + 10, nCol_NAME, nCol_DATE, nCol_CHEK, nCol_NAME, nCol_PANO, nCol_NAME };

        public enum enmSel_EXAM_BLOODCROSSM { BDATE, PANO, SNAME, SEX, AGE, AGEMM, IPDOPD, DEPTCODE, WARD, ROOM, DRCODE, DR_NM, DRCOMMENT, ORDERNO, ANTIBDATE, ANTIB_BLOODMST, ABO_N, SERUM, RH, SPECNO, SEQNO, SUBCODE, EXAMYNAME, WSCODE1, COMPONENT, COMPONENT_NM, BLOODNO, CAPACITY, DONNORABO, DONNORRH, PTABO, PTRH, MAJOR, SALINE, ALBUMN, ANTIG, ANTIB, GUMSADATE_FULL, GUMSADATE, GUMSAHH, GUMSAMM, GUMSADATE_FR_FULL, GUMSADATE_FR, GUMSAHH_FR, GUMSAMM_FR, GUMSAJA, GUMSAJA_NM, GBSTATUS, JOBTYPE, SPEC_STATUS, BLOODDATE, DIFF, EXPIRE};

        public string up_EXAM_SPECMST_STATUS(PsmhDb pDbCon, string strSPECNO, bool isCancel, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  UPDATE KOSMOS_OCS.EXAM_SPECMST     \r\n";

                if (isCancel == false)
                {
                    SQL += "     SET STATUS       = '05'     \r\n";
                    SQL += "       , RESULTDATE   = SYSDATE  \r\n";
                }
                else
                {
                    SQL += "     SET STATUS       = '01'     \r\n";
                    SQL += "       , RESULTDATE   = NULL     \r\n";
                }

                SQL += "   WHERE 1=1                         \r\n";
                SQL += "     AND SPECNO        = " + ComFunc.covSqlstr(strSPECNO, false);

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }

            return SqlErr;
        }

        public string up_EXAM_RESULTC_STATUS(PsmhDb pDbCon, string strSPECNO, bool isCancel, string strBLOODNO, string strSEQNO, string strREULTSABUN, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  UPDATE KOSMOS_OCS.EXAM_RESULTC        \r\n";

                if (isCancel == false)
                {
                    SQL += "     SET STATUS        = 'V'         \r\n";
                    SQL += "       , RESULT        = " + ComFunc.covSqlstr(strBLOODNO, false);
                    SQL += "       , RESULTDATE    = SYSDATE \r\n ";
                    SQL += "       , RESULTSABUN   = " + ComFunc.covSqlstr(strREULTSABUN, false);
                }
                else
                {
                    SQL += "     SET STATUS       = ''        \r\n";
                    SQL += "       , RESULT       = ''        \r\n";
                }

                SQL += "   WHERE 1=1                            \r\n";
                SQL += "     AND SPECNO        = " + ComFunc.covSqlstr(strSPECNO, false);
                SQL += "     AND SEQNO         = " + ComFunc.covSqlstr(strSEQNO, false);

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }


        public DataTable sel_EXAM_BLOODCROSSM(PsmhDb pDbCon, string strSPECNO)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " WITH T AS (";
            SQL += " SELECT  TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE                          \r\n";
            SQL += " 	  , A.PANO                                                           \r\n";
            SQL += " 	  , B.SNAME                                                          \r\n";
            SQL += " 	  , B.SEX                                                            \r\n";
            SQL += " 	  , A.AGE                                                            \r\n";
            SQL += " 	  , A.AGEMM                                                          \r\n";
            SQL += " 	  , A.IPDOPD                                                         \r\n";
            SQL += " 	  , A.DEPTCODE                                                       \r\n";
            SQL += " 	  , A.WARD                                                           \r\n";
            SQL += " 	  , A.ROOM                                                           \r\n";
            SQL += " 	  , A.DRCODE                                                         \r\n";
            SQL += " 	  , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE) AS DR_NM               \r\n";
            SQL += " 	  , A.DRCOMMENT                                                      \r\n";
            SQL += "      , A.ORDERNO                                                        \r\n";
            SQL += "      , TO_CHAR(C.ANTIBDATE,'YYYY-MM-DD') AS ANTIBDATE                   \r\n";
            SQL += "      , C.ANTIB					       AS ANTIB_BLOODMST                 \r\n";
            SQL += "      , C.ABO_N                                                          \r\n";
            SQL += "      , C.SERUM                                                          \r\n";
            SQL += "      , C.RH                                                             \r\n";
            SQL += "      , A.SPECNO                                                         \r\n";
            SQL += "      , TO_CHAR(D.SEQNO)                   AS SEQNO                      \r\n";
            SQL += " 	  , D.SUBCODE                                                        \r\n";
            SQL += " 	  , E.EXAMYNAME                                                      \r\n";
            SQL += " 	  , E.WSCODE1                                                        \r\n";
            SQL += " 	  , TRIM(F.COMPONENT) AS COMPONENT                                   \r\n";
            SQL += " 	  , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_COMPONENT',F.COMPONENT) AS COMPONENT_NM                                                     \r\n";
            SQL += " 	  , TRIM(F.BLOODNO)   AS BLOODNO                                     \r\n";
            SQL += " 	  , TRIM(F.CAPACITY)  AS CAPACITY                                    \r\n";
            SQL += " 	  , TRIM(F.DONNORABO) AS DONNORABO                                   \r\n";
            SQL += " 	  , TRIM(F.DONNORRH)  AS DONNORRH                                    \r\n";
            SQL += " 	  , TRIM(F.PTABO   )  AS PTABO                                       \r\n";
            SQL += " 	  , TRIM(F.PTRH    )  AS PTRH                                        \r\n";
            SQL += " 	  , TRIM(F.MAJOR   )  AS MAJOR                                       \r\n";
            SQL += " 	  , TRIM(F.SALINE  )  AS SALINE                                      \r\n";
            SQL += " 	  , TRIM(F.ALBUMN  )  AS ALBUMN                                      \r\n";
            SQL += " 	  , TRIM(F.ANTIG   )  AS ANTIG                                       \r\n";
            SQL += " 	  , TRIM(F.ANTIB   )  AS ANTIB                                       \r\n";

            SQL += " 	  , DECODE(GUMSADATE, NULL,'', TRIM(TO_CHAR(GUMSADATE, 'YYYY-MM-DD')) || ' ' || TRIM(TO_CHAR(GUMSAHH,'00')) || ':' || TRIM(TO_CHAR(GUMSAMM,'00'))) AS GUMSADATE_FULL               \r\n";

            SQL += " 	  , TO_CHAR(GUMSADATE,'YYYY-MM-DD')     AS GUMSADATE                 \r\n";
            SQL += " 	  , TO_CHAR(GUMSAHH, '99')		        AS GUMSAHH                   \r\n";
            SQL += " 	  , TO_CHAR(GUMSAMM, '99')			    AS GUMSAMM                   \r\n";

            SQL += " 	  , DECODE(GUMSADATE_FR, NULL,'', TRIM(TO_CHAR(GUMSADATE_FR, 'YYYY-MM-DD')) || ' ' || TRIM(TO_CHAR(GUMSAHH_FR,'00')) || ':' || TRIM(TO_CHAR(GUMSAMM_FR,'00'))) AS GUMSADATE_FR_FULL              \r\n";

            SQL += " 	  , TO_CHAR(GUMSADATE_FR,'YYYY-MM-DD')  AS GUMSADATE_FR              \r\n";
            SQL += " 	  , TO_CHAR(GUMSAHH_FR, '99')		   	AS GUMSAHH_FR                \r\n";  
            SQL += " 	  , TO_CHAR(GUMSAMM_FR, '99')		   	AS GUMSAMM_FR                \r\n";

            SQL += "       , GUMSAJA                                                         \r\n";
            SQL += "       , KOSMOS_OCS.FC_BAS_USER_NAME(GUMSAJA) AS GUMSAJA_NM              \r\n";
            SQL += "       , F.GBSTATUS                                                      \r\n";
            SQL += "       , F.JOBTYPE                                                       \r\n";
            SQL += "       , A.STATUS							AS SPEC_STATUS               \r\n";
            //2019-06-03 안정수, BLOODDATE 추가
            SQL += "       , (SELECT TO_CHAR(DRAWDATE, 'YYYY-MM-DD') FROM KOSMOS_OCS.EXAM_BLOOD_IO WHERE BLOODNO = F.BLOODNO AND GBSTATUS = 'M') AS BLOODDATE           \r\n";
            SQL += "   FROM  KOSMOS_OCS.EXAM_SPECMST A                                       \r\n";
            SQL += "       , KOSMOS_PMPA.BAS_PATIENT B                                       \r\n";
            SQL += "       , (                                                               \r\n";
            SQL += " 	        SELECT C.*                                                   \r\n";
            SQL += " 	          FROM KOSMOS_OCS.EXAM_BLOOD_MASTER C                        \r\n";
            SQL += " 	         WHERE TO_CHAR(MODIFYDATE,'YYYY-MM-DD HH24:MI') = (SELECT TO_CHAR(MAX(MODIFYDATE),'YYYY-MM-DD HH24:MI') \r\n";
            SQL += " 	                               FROM KOSMOS_OCS.EXAM_BLOOD_MASTER                                                \r\n";
            SQL += " 	                              WHERE PANO = C.PANO)                                                              \r\n";
            SQL += "          ) 					  C                                                                                 \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_RESULTC D                                                                                 \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_MASTER  E                                                                                 \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_BLOODCROSSM F                                                                             \r\n";
            SQL += "  WHERE 1=1                                                                                                         \r\n";
            SQL += "    AND A.SPECNO      = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "    AND A.SPECNO      = D.SPECNO                                                                                    \r\n";
            SQL += "    AND A.PANO 	      = B.PANO                                                                                      \r\n";
            SQL += "    AND A.PANO        = C.PANO(+)                                                                                   \r\n";
            SQL += "    AND D.SUBCODE     = E.MASTERCODE                                                                                \r\n";
            SQL += "    AND D.SPECNO      = F.SPECNO(+)                                                                                 \r\n";
            SQL += "    AND D.SEQNO	      = F.SEQNO(+)                                                                                  \r\n";
            SQL += "    AND F.GBSTATUS(+)!= '3'                                                                                     \r\n";
            SQL += "  ORDER BY D.SEQNO                                                                                                  \r\n";
            SQL += "   )                                                                                                                \r\n";
            SQL += "  SELECT T.*                                                                                                        \r\n";
            SQL += "       , CASE WHEN NVL(TRIM(GUMSADATE_FULL),'@') != '@' AND NVL(TRIM(GUMSADATE_FR_FULL),'@') != '@' THEN ROUND((TO_DATE(GUMSADATE_FULL,'YYYY-MM-DD HH24:MI') -  TO_DATE(GUMSADATE_FR_FULL,'YYYY-MM-DD HH24:MI'))*1440,2) END AS DIFF    \r\n";
            SQL += "       , (SELECT TO_CHAR(EXPIRE,'YYYY-MM-DD') AS EXPIRE FROM KOSMOS_OCS.EXAM_BLOOD_IO WHERE BLOODNO = T.BLOODNO AND COMPONENT = T.COMPONENT)                                                                                 AS EXPIRE  \r\n";
            SQL += "     FROM T                                                                                                        \r\n";
             
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

        public string up_EXAM_BLOOD_IO_JAN(PsmhDb pDbCon, string strBLOODNO, string strCOMPONENT, string strDATE, string strPERSON, string strCAUSE, string strCAUSE_CD, string strJANSU, string strJANPAE, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  UPDATE KOSMOS_OCS.EXAM_BLOOD_IO       \r\n";
                SQL += "     SET JANDATE             = " + ComFunc.covSqlDate(strDATE, false);
                SQL += "       , JANSABUN            = " + ComFunc.covSqlstr(strPERSON.Trim(), false);
                SQL += "       , JANREMARK           = " + ComFunc.covSqlstr(strCAUSE.Trim(), false);
                SQL += "       , JANCASE_CD          = " + ComFunc.covSqlstr(strCAUSE_CD.Trim(), false);
                SQL += "       , JANSU               = " + ComFunc.covSqlstr(strJANSU.Trim(), false);
                SQL += "       , JANPAE              = " + ComFunc.covSqlstr(strJANPAE.Trim(), false);
                SQL += "   WHERE 1=1                            \r\n";
                SQL += "     AND BLOODNO             = " + ComFunc.covSqlstr(strBLOODNO.Trim(), false);
                SQL += "     AND COMPONENT           = " + ComFunc.covSqlstr(strCOMPONENT.Trim(), false);

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public DataSet sel_EXAM_BLOOD_IO(PsmhDb pDbCon, string strFDATE, string strTDATE, string strGBJOB, string s)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                                                                                                                 \r\n";
            SQL += "           TO_CHAR(B.STOREDATE,'YYYY-MM-DD')                                                                               					    AS SDATE                 \r\n";
            SQL += "         , SUBSTR(B.BLOODNO,1,2) ||'-'|| SUBSTR(B.BLOODNO,3,2) || '-' || SUBSTR(B.BLOODNO,5)                               					    AS BLOODNO               \r\n";
            SQL += "         , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_COMPONENT', B.COMPONENT) 			                                    					    AS COMPONENT             \r\n";
            SQL += "         , (SELECT NAME FROM KOSMOS_OCS.EXAM_BLOOD_COMM WHERE TRIM(CODE) = B.STORE)                                        					    AS STORE                 \r\n";
            SQL += "         , TO_CHAR(B.EXPIRE,'YYYY-MM-DD')    											                                    					AS EXDATE                \r\n";
            SQL += "         , DECODE(KOSMOS_OCS.FC_BAS_USER_NAME(TRIM(B.STOREPERSON)),'',B.STOREPERSON,KOSMOS_OCS.FC_BAS_USER_NAME(TRIM(B.STOREPERSON)))	    	AS STOREPERSON           \r\n";
            SQL += "         , SUBSTR(B.ABO,1,LENGTH(B.ABO)-1)												                                    					AS ABO                   \r\n";
            SQL += "   	     , SUBSTR(B.ABO,LENGTH(B.ABO),1)													                                    				AS RH                    \r\n";
            SQL += "   	     , B.CAPACITY																	                                    					AS CAPACITY              \r\n";
            SQL += "         , TO_CHAR(B.STORERDATE,'YYYY-MM-DD')                                                                              					    AS STORERDATE            \r\n";
            SQL += "         , DECODE(KOSMOS_OCS.FC_BAS_USER_NAME(TRIM(B.STORERPERSON)),'',B.STORERPERSON,KOSMOS_OCS.FC_BAS_USER_NAME(TRIM(B.STORERPERSON)))	    AS STORERPERSON          \r\n";
            SQL += "         , B.STORERCAUSE                                                                                                     					AS STORERCAUSE           \r\n";
            SQL += "         , TO_CHAR(B.DAEYUDATE,'YYYY-MM-DD')                                                                                 					AS DAEYUDATE             \r\n";
            SQL += "         , (SELECT NAME FROM KOSMOS_OCS.EXAM_BLOOD_COMM WHERE TRIM(CODE) = TRIM(B.DAEYU))                                    					AS DAEYU                 \r\n";
            SQL += "  	     , DECODE(KOSMOS_OCS.FC_BAS_USER_NAME(TRIM(TO_CHAR(A.ENTSABUN))),'',TO_CHAR(A.ENTSABUN),KOSMOS_OCS.FC_BAS_USER_NAME(TRIM(TO_CHAR(A.ENTSABUN))))	  AS DAEYU_SABUN \r\n";
            SQL += "         , TO_CHAR(DESTROYDATE,'YYYY-MM-DD')                                                                               					    AS DESTROYDATE           \r\n";
            SQL += "    	 , DECODE(KOSMOS_OCS.FC_BAS_USER_NAME(TRIM(B.DESTROYPERSON)),'',B.DESTROYPERSON,KOSMOS_OCS.FC_BAS_USER_NAME(TRIM(B.DESTROYPERSON)))	    AS DESTROYPERSON         \r\n";
            SQL += "    	 , B.DESTROYCAUSE                                                                                                    					AS DESTROYCAUSE          \r\n";
            SQL += "         , TO_CHAR(B.JANDATE,'YYYY-MM-DD')                                                                                   					AS JANDATE               \r\n";
            SQL += "         , B.JANSU                                                                                                           					AS JANSU                 \r\n";
            SQL += "         , B.JANPAE                                                                                                          					AS JANPAE                \r\n";
            SQL += "         , DECODE(KOSMOS_OCS.FC_BAS_USER_NAME(TRIM(B.JANSABUN)),'',B.JANSABUN,KOSMOS_OCS.FC_BAS_USER_NAME(TRIM(B.JANSABUN)))	                AS JANSABUN              \r\n";
            SQL += "         , B.JANREMARK                                                                                                       					AS JANREMARK             \r\n";
            SQL += "         , TO_CHAR(B.OUTRDATE,'YYYY-MM-DD')   																									AS OUTRDATE              \r\n";
            SQL += "         , DECODE(KOSMOS_OCS.FC_BAS_USER_NAME(TRIM(B.OUTRPERSON)),'',B.OUTRPERSON,KOSMOS_OCS.FC_BAS_USER_NAME(TRIM(B.OUTRPERSON)))	            AS OUTRPERSON            \r\n";
            SQL += "         , B.OUTRCAUSE                                                                                                                         AS OUTRCASE               \r\n";
            SQL += "         , B.PANO                                                                                                                                                        \r\n";
            SQL += "         , B.SNAME                                                                                                                                                       \r\n";
            SQL += "      FROM KOSMOS_OCS.EXAM_BLOODTRANS A                                                                                                                                  \r\n";
            SQL += "         , KOSMOS_OCS.EXAM_BLOOD_IO   B                                                                                                                                  \r\n";
            SQL += "     WHERE 1=1                                                                                                                                                           \r\n";
            if (strGBJOB.Equals("9"))
            {
                SQL += "       AND B.JANDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                SQL += "                         AND " + ComFunc.covSqlDate(strTDATE, false);
                SQL += "       AND A.GBJOB 		=  '4'  \r\n"; 
            }
            else
            {
                SQL += "       AND A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                SQL += "                       AND " + ComFunc.covSqlDate(strTDATE, false);
                SQL += "       AND A.GBJOB 		=  " + ComFunc.covSqlstr(strGBJOB, false);
            }
            SQL += "       AND A.BLOODNO 	= B.BLOODNO                                                                                                                                  \r\n";
            SQL += "       AND A.COMPONENT 	= B.COMPONENT                                                                                                                                    \r\n";
            SQL += "     ORDER BY B.STORETIME, B.EXPIRE, B.BLOODNO                                                                                                                           \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

            return ds;
        }

        public string up_EXAM_BLOODCROSSM_OUT_R(PsmhDb pDbCon, string strBLOODNO, string strCOMPONENT, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  UPDATE KOSMOS_OCS.EXAM_BLOODCROSSM    \r\n";
                SQL += "     SET GBSTATUS       = '1'           \r\n";
                SQL += "   WHERE 1=1                         \r\n";
                SQL += "     AND BLOODNO        = " + ComFunc.covSqlstr(strBLOODNO, false);
                SQL += "     AND COMPONENT      = " + ComFunc.covSqlstr(strCOMPONENT, false);

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public DataTable sel_EXAM_BLOOD_IO_INFO(PsmhDb pDbCon, string strBLOODNO, string strCOMPONENT)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                              \r\n";
            SQL += "        BLOODNO                                      \r\n";
            SQL += "      , ABO                                          \r\n";
            SQL += "      , COMPONENT                                    \r\n";
            SQL += "      , CAPACITY                                     \r\n";
            SQL += "      , TO_CHAR(EXPIRE,'YYYY-MM-DD')    AS EXPIRE    \r\n";
            SQL += "      , TO_CHAR(STOREDATE,'YYYY-MM-DD') AS STOREDATE \r\n";
            SQL += "      , STOREPERSON                                  \r\n";
            SQL += "      , STORE                                        \r\n";
            SQL += "      , AMT                                          \r\n";
            SQL += "      , TO_CHAR(DRAWDATE,'YYYY-MM-DD') AS DRAWDATE   \r\n";
            SQL += "      , GBSTATUS                                     \r\n";
            SQL += "  	  , DECODE(KOSMOS_OCS.FC_BAS_USER_NAME(STOREPERSON),'',STOREPERSON,KOSMOS_OCS.FC_BAS_USER_NAME(STOREPERSON)) AS STOREPERSON_NM       \r\n";
            SQL += "      , TO_CHAR(DAEYUDATE,'YYYY-MM-DD')             AS DAEYUDATE    \r\n";
            SQL += "      , TO_CHAR(DESTROYDATE,'YYYY-MM-DD')           AS DESTROYDATE  \r\n";
            SQL += "      , TO_CHAR(JANDATE,'YYYY-MM-DD')               AS JANDATE      \r\n";
            SQL += "      , TO_CHAR(OUTDATE,'YYYY-MM-DD HH24:MI')       AS OUTDATE      \r\n";
            SQL += "  	  , OUTPERSON                                   AS OUTPERSON       \r\n";
            SQL += "  	  , DECODE(KOSMOS_OCS.FC_BAS_USER_NAME(OUTPERSON),'',OUTPERSON,KOSMOS_OCS.FC_BAS_USER_NAME(OUTPERSON)) AS OUTPERSON_NM       \r\n";
            SQL += "  	  , PANO                                        AS PANO       \r\n";
            SQL += "  	  , SNAME                                       AS SNAME      \r\n";
            SQL += "  	  , DEPT                                        AS DEPT       \r\n";
            SQL += "  	  , WARD                                        AS WARD       \r\n";
            SQL += "  	  , ROOM                                        AS ROOM       \r\n";
            SQL += "  	  , DR                                          AS DR         \r\n";
            SQL += "  	  , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DR)         AS DR_NM      \r\n";
            SQL += "  	  , NURSE                                       AS NURSE         \r\n";
            SQL += "  	  , KOSMOS_OCS.FC_BAS_USER_NAME(NURSE)          AS NURSE_NM   \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_BLOOD_IO                                   \r\n";
            SQL += " WHERE 1=1                                                        \r\n";
            SQL += "   AND BLOODNO      = " + ComFunc.covSqlstr(strBLOODNO, false);
            SQL += "   AND COMPONENT    = " + ComFunc.covSqlstr(strCOMPONENT, false);

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

        public DataSet sel_EXAM_BLOODTRANS_EXCEL(PsmhDb pDbCon, string strFDATE, string strTDATE)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "     SELECT                                                                                                                                     \r\n";
            SQL += "          SUBSTR(A.BLOODNO,1,2) ||'-'|| SUBSTR(A.BLOODNO,3,2) || '-' || SUBSTR(A.BLOODNO,5) 	AS BLOODNO --1                                  \r\n";
            SQL += "     	  , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_EXCEL_CAPA', A.COMPONENT || A.CAPACITY)		AS COM_CAPA --2                                 \r\n";
            SQL += "     	  , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_COMPONENT' , A.COMPONENT) 					AS COMPONENT   --3                              \r\n";
            SQL += "        ,                                                                                                                                       \r\n";
            SQL += "          (                                                                                                                                     \r\n";
            SQL += "   	   	    CASE WHEN B.GBSTATUS = 'C' THEN '1'                                                                                                 \r\n";
            SQL += "   	   	         WHEN B.GBSTATUS = 'P' THEN '2'                                                                                                 \r\n";
            SQL += "   	   	         WHEN B.GBSTATUS = ''  THEN '3'                                                                                                 \r\n";
            SQL += "   	             ELSE '' END                                                                                                                    \r\n";
            SQL += "   		) 																					    AS GBSTATUS  	--4                                 \r\n";
            SQL += "     	  ,	TO_CHAR(A.BDATE,'YYYY-MM-DD') 														AS BDATE		--5                             \r\n";
            SQL += "     	  , TO_CHAR(A.ENTDATE,'HH24:MM') 														AS ENTDATE  	--6                             \r\n";
            SQL += "        , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_EXCEL_ABO', B.ABO)						    AS ABO_EXCEL	--7                                 \r\n";
            SQL += "        , SUBSTR(B.ABO,1,LENGTH(B.ABO)-1) ||'(' || SUBSTR(B.ABO,LENGTH(B.ABO),1) || ')'       AS ABO          --8                               \r\n";
            SQL += "     	  , DECODE(KOSMOS_OCS.FC_BAS_USER_NAME(A.ENTSABUN),'',TO_CHAR(A.ENTSABUN),KOSMOS_OCS.FC_BAS_USER_NAME(A.ENTSABUN)) AS ENTSABUN --9      \r\n";
            SQL += "     	  , D.SEX																				AS SEX			-- 10                           \r\n";
            SQL += "     	  , SUBSTR(KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(A.PANO),0,4)                     			AS BIRTH		-- 11                           \r\n";
            SQL += "        , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_EXCEL_ABO', TRIM(C.PTABO) || TRIM(C.PTRH))	AS PTAR         -- 12                               \r\n";
            SQL += "     	  , DECODE(NVL(TRIM(D.ZIPCODE3),'*'),'*','999'                                                                                          \r\n";
            SQL += "     	  , DECODE(KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_ZIPNAME',F.ZIPNAME1),'','999'                                                         \r\n";
            SQL += "     	    	  ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_ZIPNAME',F.ZIPNAME1))) 			    AS ZIPNAME 		-- 13                           \r\n";
            SQL += "         , DECODE(KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_DEPT',B.DEPT),'','099'                                                                    \r\n";
            SQL += "   	  		  ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_DEPT',B.DEPT)) 					    AS DEPT 		-- 14                               \r\n";
            SQL += "        , TO_CHAR(C.GUMSADATE,'YYYY-MM-DD')													AS GUMSADATE                                        \r\n";
            SQL += "      FROM KOSMOS_OCS.EXAM_BLOODTRANS 	A                                                                                                       \r\n";
            SQL += "      	  , KOSMOS_OCS.EXAM_BLOOD_IO 	B                                                                                                       \r\n";
            SQL += "      	  , KOSMOS_OCS.EXAM_BLOODCROSSM C                                                                                                       \r\n";
            SQL += "      	  , KOSMOS_PMPA.BAS_PATIENT		D                                                                                                       \r\n";
            SQL += "  	      ,(SELECT SEX, AGE, PANO                                                                                                               \r\n";
            SQL += "  	           FROM KOSMOS_OCS.EXAM_BLOOD_MASTER D                                                                                              \r\n";
            SQL += "  	          WHERE 1=1                                                                                                                         \r\n";
            SQL += "  	            AND TO_CHAR(MODIFYDATE,'YYYY-MM-DD HH24:MI') = (SELECT TO_CHAR(MAX(MODIFYDATE),'YYYY-MM-DD HH24:MI')                            \r\n";
            SQL += "  	                                                              FROM KOSMOS_OCS.EXAM_BLOOD_MASTER                                             \r\n";
            SQL += "  	                                                              WHERE PANO = D.PANO)                                                          \r\n";
            SQL += "  	        ) E                                                                                                                                 \r\n";
            SQL += "            , (                                                                                                                                 \r\n";
            SQL += "         	      SELECT DISTINCT ZIPCODE, ZIPNAME1                                                                                             \r\n";
            SQL += "         	        FROM KOSMOS_PMPA.BAS_ZIPS_ROAD                                                                                              \r\n";
            SQL += "         	    )F                                                                                                                              \r\n";
            SQL += "     WHERE 1=1                                                                                                                                  \r\n";
            SQL += "     AND A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
            SQL += "     			     AND " + ComFunc.covSqlDate(strTDATE, false);
            SQL += "       AND A.GBJOB  	    = '4'                                                                                                               \r\n";
            SQL += "       AND A.BLOODNO 	    = B.BLOODNO                                                                                                         \r\n";
            SQL += "       AND A.COMPONENT    = B.COMPONENT                                                                                                         \r\n";
            SQL += "       AND A.BLOODNO 	    = C.BLOODNO                                                                                                         \r\n";
            SQL += "       AND A.PANO    	    = C.PANO                                                                                                            \r\n";
            SQL += "       AND A.PANO		    = D.PANO                                                                                                            \r\n";
            SQL += "       	 AND A.PANO		  = E.PANO                                                                                                              \r\n";
            SQL += "       AND D.ZIPCODE3     = F.ZIPCODE(+)                                                                                                        \r\n";
            SQL += "       AND B.BLOODNO      = C.BLOODNO                                                                                                           \r\n";
            SQL += "       AND B.COMPONENT    = C.COMPONENT                                                                                                         \r\n";
            SQL += "       AND C.GBSTATUS     = '2'                                                                                                                 \r\n";
            SQL += "     ORDER BY C.GUMSADATE                                                                                                                       \r\n";


            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

            return ds;
        }

        public DataSet sel_EXAM_BLOODTRANS_OUT(PsmhDb pDbCon, string strDATE_FR, string strDATE_TO, string strABO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "   SELECT                                                                                                               \r\n";
            SQL += "   		TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE                                                                             \r\n";
            SQL += "   	  , A.PANO                                                                                                          \r\n";
            SQL += "   	  , B.SNAME                                                                                                         \r\n";
            SQL += "   	  , D.SEX                                                                                                           \r\n";
            SQL += "   	  , D.AGE                                                                                                           \r\n";
            SQL += "   	  , B.WARD                                                                                                          \r\n";
            SQL += "   	  , B.ROOM                                                                                                          \r\n";
            SQL += "   	  , C.PTABO                                                                                                         \r\n";
            SQL += "   	  , C.PTRH                                                                                                          \r\n";
            SQL += "   	  , SUBSTR(A.BLOODNO,1,2) ||'-'|| SUBSTR(A.BLOODNO,3,2) || '-' || SUBSTR(A.BLOODNO,5)    AS BLOODNO                 \r\n";
            SQL += "   	  , B.COMPONENT || '.' ||KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_COMPONENT', B.COMPONENT) AS COMPONENT_NM            \r\n";
            SQL += "   	  , B.CAPACITY                                                                                                      \r\n";
            SQL += "   	  , C.DONNORABO                                                                                                     \r\n";
            SQL += "   	  , C.DONNORRH DNAR                                                                                                 \r\n";
            SQL += "   	  , KOSMOS_OCS.FC_BAS_USER_NAME(A.ENTSABUN)  AS ENTSABUN                                                            \r\n";
            SQL += "   	  , KOSMOS_OCS.FC_BAS_USER_NAME(A.NURSE) 	 AS NURSE                                                               \r\n";
            SQL += "   	  , TO_CHAR(B.OUTDATE, 'YYYY-MM-DD HH24:MI') AS OUTDATE                                                             \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_BLOODTRANS 	         A                                                                          \r\n";
            SQL += "      , KOSMOS_OCS.EXAM_BLOOD_IO   	         B                                                                          \r\n";
            SQL += "      , KOSMOS_OCS.EXAM_BLOODCROSSM 	     C                                                                          \r\n";
            SQL += "      ,(SELECT SEX, AGE, PANO                                                                                           \r\n";
            SQL += "           FROM KOSMOS_OCS.EXAM_BLOOD_MASTER D                                                                          \r\n";
            SQL += "          WHERE 1=1                                                                                                     \r\n";
            SQL += "            AND TO_CHAR(MODIFYDATE,'YYYY-MM-DD HH24:MI') = (SELECT TO_CHAR(MAX(MODIFYDATE),'YYYY-MM-DD HH24:MI')        \r\n";
            SQL += "                                                              FROM KOSMOS_OCS.EXAM_BLOOD_MASTER                         \r\n";
            SQL += "                                                              WHERE PANO = D.PANO)                                      \r\n";
            SQL += "        ) D                                                                                                             \r\n";
            SQL += "   WHERE 1=1                                                                                                            \r\n";
            SQL += "     AND A.BDATE BETWEEN " + ComFunc.covSqlDate(strDATE_FR, false);
            SQL += "                     AND " + ComFunc.covSqlDate(strDATE_TO, false);
            SQL += "     AND A.GBJOB  	  = '4'                                                                                             \r\n";

            if (!strABO.Equals("*"))
            {
                SQL += "     AND  B.ABO IN ('" + strABO + "+','" + strABO + "-')        \r\n";
            }

            SQL += "     AND A.BLOODNO 	  = B.BLOODNO                                                                                       \r\n";
            SQL += "     AND A.COMPONENT  = B.COMPONENT                                                                                     \r\n";
            SQL += "     AND A.BLOODNO 	  = C.BLOODNO                                                                                       \r\n";
            SQL += "     AND A.PANO    	  = C.PANO                                                                                          \r\n";
            SQL += "     AND A.PANO		  = D.PANO                                                                                          \r\n";
            //2018.01.10.김홍록:크로스매칭 한것은 무조건 나간다
            SQL += "     AND B.BLOODNO    = C.BLOODNO                                                                                       \r\n";
            SQL += "     AND B.COMPONENT  = C.COMPONENT                                                                                     \r\n";
            SQL += "     AND C.GBSTATUS   = '2'                                                                                             \r\n";
            //------------------------------------------------------------------------------------------------------------------------------------
            SQL += "   ORDER BY C.GUMSADATE                                                                                                 \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

            return ds;
        }

        public DataSet sel_EXAM_BLOODCROSSM_TAT(PsmhDb pDbCon, string strGUMSADATE_FR, string strGUMSADATE_TO, string strJOBTYPE)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " WITH T AS (                                                                                                                                                                                               \r\n";
            SQL += " 	SELECT A.SPECNO                                                                                                                                                                                         \r\n";
            SQL += " 	     , A.PANO                                                                                                                                                                                           \r\n";
            SQL += " 	     , B.SNAME                                                                                                                                                                                          \r\n";
            SQL += " 	     , A.DEPT                                                                                                                                                                                           \r\n";
            SQL += " 	     , A.WARD                                                                                                                                                                                           \r\n";
            SQL += " 	     , A.ROOM                                                                                                                                                                                           \r\n";
            SQL += " 	     , COMPONENT || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_COMPONENT'	,A.COMPONENT) 							AS COMPONENT                                                                    \r\n";
            SQL += " 	     , A.BLOODNO                                                                                                                                                                                        \r\n";
            SQL += " 	     , A.CAPACITY                                                                                                                                                                                       \r\n";
            SQL += " 	     , A.DONNORABO                                                                                                                                                                                      \r\n";
            SQL += " 	     , A.DONNORRH                                                                                                                                                                                       \r\n";
            SQL += " 	     , DECODE(A.GUMSADATE		, NULL, '', TRIM(TO_CHAR(A.GUMSADATE   , 'YYYY-MM-DD')) || ' ' || TRIM(TO_CHAR(A.GUMSAHH   ,'00')) || ':' || TRIM(TO_CHAR(A.GUMSAMM   ,'00'))) AS GUMSADATE_FULL        \r\n";
            SQL += " 	     , DECODE(A.GUMSADATE_FR 	, NULL, '', TRIM(TO_CHAR(A.GUMSADATE_FR, 'YYYY-MM-DD')) || ' ' || TRIM(TO_CHAR(A.GUMSAHH_FR,'00')) || ':' || TRIM(TO_CHAR(A.GUMSAMM_FR,'00'))) AS GUMSADATE_FR_FULL     \r\n";
            SQL += " 	     , OVERTIME_REAMRK                                                                                                                                                                                  \r\n";
            SQL += " 	  FROM KOSMOS_OCS.EXAM_BLOODCROSSM A                                                                                                                                                                    \r\n";
            SQL += " 	     , KOSMOS_PMPA.BAS_PATIENT	   B                                                                                                                                                                    \r\n";
            SQL += " 	 WHERE 1=1                                                                                                                                                                                              \r\n";
            SQL += " 	   AND A.PANO = B.PANO                                                                                                                                                                                  \r\n";
            SQL += " 	   AND GUMSADATE BETWEEN " + ComFunc.covSqlDate(strGUMSADATE_FR, false);
            SQL += " 					 	 AND " + ComFunc.covSqlDate(strGUMSADATE_TO, false);
            SQL += " 	   AND A.JOBTYPE = " + ComFunc.covSqlstr(strJOBTYPE, false);
            SQL += " 	ORDER BY GUMSADATE DESC, GUMSAHH DESC                                                                                                                                                                   \r\n";
            SQL += " )                                                                                                                                                                                                         \r\n";
            SQL += " SELECT                                                                                                                                                                                                    \r\n";
            SQL += " 		  '' CHK                                                                                                                                                                                            \r\n";
            SQL += " 		, T.SPECNO                                                                                                                                                                                          \r\n";
            SQL += " 		, T.PANO                                                                                                                                                                                            \r\n";
            SQL += " 		, T.SNAME                                                                                                                                                                                           \r\n";
            SQL += " 		, T.DEPT                                                                                                                                                                                            \r\n";
            SQL += " 		, T.WARD                                                                                                                                                                                            \r\n";
            SQL += " 		, T.ROOM                                                                                                                                                                                            \r\n";
            SQL += " 		, T.COMPONENT                                                                                                                                                                                       \r\n";
            SQL += " 		, TRIM(T.BLOODNO) AS BLOODNO                                                                                                                                                                        \r\n";
            SQL += " 		, T.CAPACITY                                                                                                                                                                                        \r\n";
            SQL += " 		, T.DONNORABO                                                                                                                                                                                       \r\n";
            SQL += " 		, T.DONNORRH                                                                                                                                                                                        \r\n";
            SQL += " 		, T.GUMSADATE_FULL                                                                                                                                                                                  \r\n";
            SQL += " 		, T.GUMSADATE_FR_FULL                                                                                                                                                                               \r\n";
            SQL += " 		, (                                                                                                                                                                                                 \r\n";
            SQL += " 		    CASE WHEN NVL(TRIM(GUMSADATE_FULL),'@') != '@' AND NVL(TRIM(GUMSADATE_FR_FULL),'@') != '@'                                                                                                    \r\n";
            SQL += " 			     THEN ROUND((TO_DATE(GUMSADATE_FULL,'YYYY-MM-DD HH24:MI') -  TO_DATE(GUMSADATE_FR_FULL,'YYYY-MM-DD HH24:MI'))*1440,2)                                                                     \r\n";
            SQL += " 		     END                                                                                                                                                                                          \r\n";
            SQL += " 		  )	AS DIFF                                                                                                                                                                                       \r\n";
            SQL += " 	    , OVERTIME_REAMRK                                                                                                                                                                                 \r\n";

            SQL += "   FROM T                                                                                                                                                                                                 \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

            return ds;
        }

        public bool sel_Bloodno_Exisist(PsmhDb pDbCon, string argBloodNo)
        {
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT BLOODNO";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_BLOOD_IO";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND BLOODNO = '" + argBloodNo + "' ";            

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;                    
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        public string up_BAS_BCODE(PsmhDb pDbCon, string strCODE, string strGUBUN2, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  UPDATE KOSMOS_PMPA.BAS_BCODE          \r\n";
                SQL += "     SET GUBUN2 = " + ComFunc.covSqlstr(strGUBUN2, false);
                SQL += "   WHERE 1=1                             \r\n";
                SQL += "     AND GUBUN = 'BLOD_CROSS_JOBTYPE'    \r\n";
                SQL += "     AND CODE  = " + ComFunc.covSqlstr(strCODE, false);

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public string ins_EXAM_BLOODORDER(PsmhDb pDbCon, string strPANO, string strITEMCODE, string strORDERNO, string strORDERCODE, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  INSERT INTO KOSMOS_OCS.EXAM_BLOODORDER (      \r\n";
                SQL += "       JUBSUDATE    --접수일자                  \r\n";
                SQL += "     , PANO         --환자번호                  \r\n";
                SQL += "     , ORDERGUBUN   --Order 구분                \r\n";
                SQL += "     , ITEMCODE     --Item Code(MasterCode)     \r\n";
                SQL += "     , ORDERNO      --Order No                  \r\n";
                SQL += "     , ORDERCODE    --OrderCode                 \r\n";
                SQL += "  	) VALUES (                                                    \r\n";
                SQL += " TO_DATE(TO_CHAR(SYSDATE,'YYYY-MM-DD'),'YYYY-MM-DD')              \r\n";
                SQL += " " + ComFunc.covSqlstr(strPANO, true);
                SQL += " " + ComFunc.covSqlstr("C", true);
                SQL += " " + ComFunc.covSqlstr(strITEMCODE, true);
                SQL += " " + ComFunc.covSqlstr(strORDERNO, true);
                SQL += " " + ComFunc.covSqlstr(strORDERCODE, true);
                SQL += "  	)                                                                           \r\n";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public string save_EXAM_BLOOD_MASTER(PsmhDb pDbCon, string strPANO, string strSNAME, string strAGE
                                           , string strSEX, string strWARD, string strROOM, string strABO
                                           , string strSABUN, string strBI, string strABO_N, string strRH, string strSERUM, string strANTIB, ref int intRowAffected)
        {
            string SqlErr = "";

            try {
                SQL = "";
                SQL += "  MERGE INTO KOSMOS_OCS.EXAM_BLOOD_MASTER  A                                \r\n";
                SQL += "       USING DUAL                                                           \r\n";
                SQL += "  	    ON (                                                                \r\n";
                SQL += "  	    	    A.PANO   = " + ComFunc.covSqlstr(strPANO, false);
                SQL += "  	        )                                                               \r\n";
                SQL += "  		WHEN     MATCHED THEN                                               \r\n";
                SQL += "  		UPDATE                                                              \r\n";
                SQL += "  	       SET SNAME       = " + ComFunc.covSqlstr(strSNAME, false);
                SQL += "             , AGE         = " + ComFunc.covSqlstr(strAGE, false);
                SQL += "             , SEX         = " + ComFunc.covSqlstr(strSEX, false);
                SQL += "             , WARD        = " + ComFunc.covSqlstr(strWARD, false);
                SQL += "             , ROOM        = " + ComFunc.covSqlstr(strROOM, false);
                SQL += "             , ABO         = " + ComFunc.covSqlstr(strABO, false);
                SQL += "             , MODIFYDATE  = SYSDATE                                        \r\n";
                SQL += "             , SABUN       = " + ComFunc.covSqlstr(strSABUN, false);
                SQL += "             , BI          = " + ComFunc.covSqlstr(strBI, false);
                SQL += "             , ABO_N       = " + ComFunc.covSqlstr(strABO_N, false);
                SQL += "             , RH          = " + ComFunc.covSqlstr(strRH, false);
                SQL += "             , SERUM       = " + ComFunc.covSqlstr(strSERUM, false);


                //2018.03.19.김홍록: 같은 시간에 2개의 로우가 생성되는 일이 있었다. 내쪽에서 생성되었다기 보다는 결과 입력시 생성된 것으로 생각되어서 아래 루틴을 추가함.
                SQL += "             , UPPS       = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
                SQL += "             , UPDT_DT    = SYSDATE \r\n";

                if (string.IsNullOrEmpty(strANTIB.Trim()) == false)
                {
                    SQL += "             , ANTIB       = " + ComFunc.covSqlstr(strANTIB, false);
                    SQL += "             , ANTIBDATE   = TO_DATE(TO_CHAR(SYSDATE,'YYYY-MM-DD'),'YYYY-MM-DD')   \r\n";
                }

                SQL += "  		WHEN NOT MATCHED THEN                                               \r\n";
                SQL += "            INSERT                                                          \r\n";
                SQL += "              (                                                             \r\n";
                SQL += "                      PANO          \r\n";
                SQL += "                    , SNAME         \r\n";
                SQL += "                    , AGE           \r\n";
                SQL += "                    , SEX           \r\n";
                SQL += "                    , WARD          \r\n";
                SQL += "                    , ROOM          \r\n";
                SQL += "                    , ABO           \r\n";
                SQL += "                    , MODIFYDATE    \r\n";
                SQL += "                    , SABUN         \r\n";
                SQL += "                    , BI            \r\n";
                SQL += "                    , ABO_N         \r\n";
                SQL += "                    , RH            \r\n";
                SQL += "                    , SERUM         \r\n";

                SQL += "                    , INPS         \r\n";
                SQL += "                    , INPT_DT         \r\n";
                SQL += "                    , UPPS         \r\n";
                SQL += "                    , UPDT_DT         \r\n";

                if (string.IsNullOrEmpty(strANTIB) == false)
                {

                    SQL += "                    , ANTIB         \r\n";
                    SQL += "                    , ANTIBDATE     \r\n";
                    
                }
                SQL += "              )                                                             \r\n";
                SQL += "              VALUES (                                                      \r\n";
                SQL += "              " + ComFunc.covSqlstr(strPANO, false);
                SQL += "              " + ComFunc.covSqlstr(strSNAME, true);
                SQL += "              " + ComFunc.covSqlstr(strAGE, true);
                SQL += "              " + ComFunc.covSqlstr(strSEX, true);
                SQL += "              " + ComFunc.covSqlstr(strWARD, true);
                SQL += "              " + ComFunc.covSqlstr(strROOM, true);
                SQL += "              " + ComFunc.covSqlstr(strABO, true);
                SQL += "              , SYSDATE                                 \r\n";
                SQL += "              " + ComFunc.covSqlstr(strSABUN, true);
                SQL += "              " + ComFunc.covSqlstr(strBI, true);
                SQL += "              " + ComFunc.covSqlstr(strABO_N, true);
                SQL += "              " + ComFunc.covSqlstr(strRH, true);
                SQL += "              " + ComFunc.covSqlstr(strSERUM, true);

                SQL += "              " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
                SQL += "             , SYSDATE \r\n";
                SQL += "              " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
                SQL += "             , SYSDATE \r\n";


                if (string.IsNullOrEmpty(strANTIB) == false)
                {
                    SQL += "              " + ComFunc.covSqlstr(strANTIB, true);
                    SQL += "              , SYSDATE                                 \r\n";
                }
                SQL += "              )                                                             \r\n";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }
            return SqlErr;
        }

        public string up_EXAM_BLOODCROSSM(PsmhDb pDbCon, string[] arrDATA, bool isCNCL, bool isModify, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  UPDATE KOSMOS_OCS.EXAM_BLOODCROSSM    \r\n";
                SQL += "     SET PTABO          = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.PTABO].ToString().Trim(), false);  /* 환자 ABO*/
                SQL += "       , PTRH           = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.PTRH].ToString().Trim(), false);  /* 환자 RH*/
                SQL += "       , MAJOR          = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.MAJOR].ToString().Trim(), false);  /* Major*/

                if (string.IsNullOrEmpty(arrDATA[(int)enmEXAM_BLOODCROSSM.GUMSAJA].ToString().Trim()) == false)
                {
                    SQL += "       , GUMSAJA        = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.GUMSAJA].ToString().Trim(), false);  /* 검사자*/
                }

                if (string.IsNullOrEmpty(arrDATA[(int)enmEXAM_BLOODCROSSM.GUMSADATE_FR].ToString().Trim()) == false)
                {
                    SQL += "       , GUMSADATE_FR   = " + ComFunc.covSqlDate(arrDATA[(int)enmEXAM_BLOODCROSSM.GUMSADATE_FR].ToString().Trim(), false);  /* */
                    SQL += "       , GUMSAHH_FR     = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.GUMSAHH_FR].ToString().Trim(), false);  /* */
                    SQL += "       , GUMSAMM_FR     = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.GUMSAMM_FR].ToString().Trim(), false);  /* */
                }
                else
                {
                    SQL += "       , GUMSADATE_FR   = NULL  \r\n";
                    SQL += "       , GUMSAHH_FR     = NULL   \r\n";
                    SQL += "       , GUMSAMM_FR     = NULL   \r\n";

                }

                if (string.IsNullOrEmpty(arrDATA[(int)enmEXAM_BLOODCROSSM.GUMSADATE].ToString().Trim()) == false)
                {
                    SQL += "       , GUMSADATE      = " + ComFunc.covSqlDate(arrDATA[(int)enmEXAM_BLOODCROSSM.GUMSADATE].ToString().Trim(), false);  /* 검사일자*/
                    SQL += "       , GUMSAHH        = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.GUMSAHH].ToString().Trim(), false);  /* 검사시간(시)*/
                    SQL += "       , GUMSAMM        = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.GUMSAMM].ToString().Trim(), false);  /* 검사시간(분)*/
                }
                else
                {
                    SQL += "       , GUMSADATE      = NULL \r\n";
                    SQL += "       , GUMSAHH        = NULL \r\n";
                    SQL += "       , GUMSAMM        = NULL \r\n";
                }

                if (isModify == false)
                {
                    SQL += "       , GBSTATUS       = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.GBSTATUS].ToString().Trim(), false);  /* 혈액상태(1.매칭 2.출고 3.매칭취소)*/
                }
                
                SQL += "       , GBCHA          = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.GBCHA].ToString().Trim(), false);  /* 검체차수*/
                SQL += "       , SALINE         = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.SALINE].ToString().Trim(), false);  /* Saline*/
                SQL += "       , ALBUMN         = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.ALBUMN].ToString().Trim(), false);  /* 37C Albumin*/
                SQL += "       , ANTIG          = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.ANTIG].ToString().Trim(), false);  /* Anti Globulin*/
                SQL += "       , ANTIB          = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.ANTIB].ToString().Trim(), false);  /* AntiBody Screening*/

                SQL += "       , UPPS           = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.UPPS].ToString().Trim(), false);  /* 수정자*/
                SQL += "       , UPDT           = SYSDATE   \r\n";
                SQL += "       , JOBTYPE        = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.JOBTYPE].ToString().Trim(), false);  /* AntiBody Screening*/

                if (isCNCL == true)
                {
                    SQL += "       , CANCAUSE       = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.CANCAUSE].ToString().Trim(), false);  /* 매칭 취소사유*/
                    SQL += "       , CANCAUSE_CD    = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.CANCAUSE_CD].ToString().Trim(), false);  /* */
                    SQL += "       , CNCL_PS        = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.CNCL_PS].ToString().Trim(), false);  /* */
                }

                SQL += "   WHERE 1=1                         \r\n";
                SQL += "     AND PANO           = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.PANO].ToString().Trim(), false);
                SQL += "     AND BLOODNO        = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.BLOODNO].ToString().Trim(), false);
                SQL += "     AND COMPONENT      = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.COMPONENT].ToString().Trim(), false);

                if (string.IsNullOrEmpty(arrDATA[(int)enmEXAM_BLOODCROSSM.SPECNO].ToString().Trim()) == false)
                {
                    SQL += "     AND SPECNO         = " + ComFunc.covSqlstr(arrDATA[(int)enmEXAM_BLOODCROSSM.SPECNO].ToString().Trim(), false);
                }

                if (isCNCL == true)
                {
                    //취소는 반드시 매칭 단계만....
                    SQL += "     AND GBSTATUS       IN ( '1')              \r\n";
                }
                else
                {
                    SQL += "     AND GBSTATUS       NOT IN ( '3')     \r\n";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public string up_EXAM_BLOODCROSSM(PsmhDb pDbCon, string strOVERTIME_REMARK, string strPANO, string strSPECNO, string strBLOODNO, string strCOMPONENT, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  UPDATE KOSMOS_OCS.EXAM_BLOODCROSSM    \r\n";
                SQL += "     SET OVERTIME_REAMRK          = " + ComFunc.covSqlstr(strOVERTIME_REMARK, false);  /* 환자 ABO*/

                SQL += "   WHERE 1=1                         \r\n";
                SQL += "     AND PANO           = " + ComFunc.covSqlstr(strPANO, false);
                SQL += "     AND BLOODNO        = " + ComFunc.covSqlstr(strBLOODNO, false);
                SQL += "     AND COMPONENT      = " + ComFunc.covSqlstr(strCOMPONENT, false);

                if (string.IsNullOrEmpty(strSPECNO) == false)
                {
                    SQL += "     AND SPECNO         = " + ComFunc.covSqlstr(strSPECNO, false);
                }

                SQL += "     AND GBSTATUS       IN ( '1', '2')              \r\n";


                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public string up_EXAM_BLOODCROSSM_OUT(PsmhDb pDbCon, string strBLOODNO, string strCOMPONENT, string strPANO, string strSPECNO, string strGBSATUS, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  UPDATE KOSMOS_OCS.EXAM_BLOODCROSSM    \r\n";
                SQL += "     SET GBSTATUS       = " + ComFunc.covSqlstr(strGBSATUS, false);
                SQL += "   WHERE 1=1                         \r\n";
                SQL += "     AND PANO           = " + ComFunc.covSqlstr(strPANO, false);
                SQL += "     AND BLOODNO        = " + ComFunc.covSqlstr(strBLOODNO, false);
                SQL += "     AND COMPONENT      = " + ComFunc.covSqlstr(strCOMPONENT, false);
                SQL += "     AND GBSTATUS       NOT IN ( '3')     \r\n";

                if (string.IsNullOrEmpty(strSPECNO.Trim()) == false)
                {
                    SQL += "     AND SPECNO         = " + ComFunc.covSqlstr(strSPECNO.Trim(), false);
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public string del_EXAM_BLOODCROSSM(PsmhDb pDbCon, string[] arrEXAM_BLOODCROSSM, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  DELETE KOSMOS_OCS.EXAM_BLOODCROSSM    \r\n";
                SQL += "   WHERE 1=1                            \r\n";
                SQL += "     AND PANO      = " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.PANO].ToString(), false);
                SQL += "     AND BLOODNO   = " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.BLOODNO].ToString(), false);
                SQL += "     AND COMPONENT = " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.COMPONENT].ToString(), false);
                SQL += "     AND SPECNO    = " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.SPECNO].ToString(), false);
                SQL += "     AND GBSTATUS  = '4'                \r\n";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public string del_EXAM_BLOODCROSSM_INS(PsmhDb pDbCon, string[] arrEXAM_BLOODCROSSM, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  DELETE KOSMOS_OCS.EXAM_BLOODCROSSM    \r\n";
                SQL += "   WHERE 1=1                            \r\n";
                SQL += "     AND PANO      = " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.PANO].ToString(), false);
                SQL += "     AND BLOODNO   = " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.BLOODNO].ToString(), false);
                SQL += "     AND COMPONENT = " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.COMPONENT].ToString(), false);
                SQL += "     AND SPECNO    = " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.SPECNO].ToString(), false);

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public string ins_EXAM_BLOODCROSSM(PsmhDb pDbCon, string[] arrEXAM_BLOODCROSSM, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  INSERT INTO KOSMOS_OCS.EXAM_BLOODCROSSM (                       \r\n";
                SQL += "  	  JUBSUDATE   -- 접수일자                                     \r\n";
                SQL += "  	, PANO        -- 환자번호                                     \r\n";
                SQL += "  	, ORDERGUBUN  -- Order 구분                                   \r\n";
                SQL += "  	, BLOODNO     -- Blood No                                     \r\n";
                SQL += "  	, ORDERNO     -- Order No                                     \r\n";
                SQL += "  	, PTABO       -- 환자 ABO                                     \r\n";
                SQL += "  	, PTRH        -- 환자 RH                                      \r\n";
                SQL += "  	, DONNORABO   -- 혈액 ABO                                     \r\n";
                SQL += "  	, DONNORRH    -- 혈액 RH                                      \r\n";
                SQL += "  	, MAJOR       -- Major                                        \r\n";
                SQL += "  	, GUMSAJA     -- 검사자                                       \r\n";
                SQL += "  	, COMPONENT   -- 성분                                         \r\n";
                SQL += "  	, GBSTATUS    -- 혈액상태(1.매칭 2.출고 3.매칭취소 4.검사중)  \r\n";
                SQL += "  	, GBCHA       -- 검체차수                                     \r\n";
                SQL += "  	, DEPT        -- 진료과                                       \r\n";
                SQL += "  	, WARD        -- 병동                                         \r\n";
                SQL += "  	, ROOM        -- 호실                                         \r\n";
                SQL += "  	, SALINE      -- Saline                                       \r\n";
                SQL += "  	, ALBUMN      -- 37C Albumin                                  \r\n";
                SQL += "  	, ANTIG       -- Anti Globulin                                \r\n";
                SQL += "  	, ANTIB       -- AntiBody Screening                           \r\n";
                SQL += "  	, CAPACITY    -- 혈액량                                       \r\n";
                SQL += "  	, CANCAUSE    -- 매칭 취소사유                                \r\n";
                SQL += "  	, GUMSADATE_FR --                                             \r\n";
                SQL += "  	, GUMSAHH_FR  --                                              \r\n";
                SQL += "  	, GUMSAMM_FR  --                                              \r\n";
                SQL += "  	, SPECNO      --                                              \r\n";
                SQL += "  	, SEQNO       --                                              \r\n";
                SQL += "  	, INPS        -- 입력자                                       \r\n";
                SQL += "  	, INPT_DT     -- 입력일시                                     \r\n";
                SQL += "  	, UPPS        -- 수정자                                       \r\n";
                SQL += "  	, UPDT        -- 수정일시                                     \r\n";
                SQL += "  	, CANCAUSE_CD --                                              \r\n";
                SQL += "  	, CNCL_PS --                                                  \r\n";
                SQL += "  	, JOBTYPE --                                                  \r\n";
                SQL += "  	) VALUES (                                                    \r\n";
                SQL += " TO_DATE(TO_CHAR(SYSDATE,'YYYY-MM-DD'),'YYYY-MM-DD')              \r\n";
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.PANO].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.ORDERGUBUN].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.BLOODNO].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.ORDERNO].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.PTABO].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.PTRH].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.DONNORABO].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.DONNORRH].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.MAJOR].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.GUMSAJA].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.COMPONENT].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.GBSTATUS].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.GBCHA].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.DEPT].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.WARD].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.ROOM].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.SALINE].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.ALBUMN].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.ANTIG].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.ANTIB].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.CAPACITY].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.CANCAUSE].ToString(), true);
                SQL += " , TO_DATE(TO_CHAR(SYSDATE,'YYYY-MM-DD'),'YYYY-MM-DD')                \r\n";
                SQL += " , TO_CHAR(SYSDATE,'HH24')                                            \r\n";
                SQL += " , TO_CHAR(SYSDATE,'MI')                                              \r\n";
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.SPECNO].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.SEQNO].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.INPS].ToString(), true);
                SQL += " , SYSDATE  \r\n";
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.UPPS].ToString(), true);
                SQL += " , SYSDATE  \r\n";
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.CANCAUSE_CD].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.CNCL_PS].ToString(), true);
                SQL += " " + ComFunc.covSqlstr(arrEXAM_BLOODCROSSM[(int)enmEXAM_BLOODCROSSM.JOBTYPE].ToString(), true);
                SQL += "  	)                                                                           \r\n";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public DataSet sel_EXAM_BLOODCROSSM_CANCEL(PsmhDb pDbCon, string strGUMSADATE_FR, string strGUMSADATE_TO, string strGBSTATUS, string strTXT)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                                                                                                                                                \r\n";
            SQL += "  		 '' AS CHK                                                                                                                                                                                      \r\n";
            SQL += "  	    , TRIM(A.JOBTYPE) || '.' || TRIM(KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_CROSS_JOBTYPE',A.JOBTYPE)) AS JOBTYPE                                                                                   \r\n";
            SQL += "        , DECODE(A.GUMSADATE		, NULL, '', TRIM(TO_CHAR(A.GUMSADATE   , 'YYYY-MM-DD')) || ' ' || TRIM(TO_CHAR(A.GUMSAHH   ,'00')) || ':' || TRIM(TO_CHAR(A.GUMSAMM   ,'00'))) AS GUMSADATE_FULL    \r\n";
            SQL += "        , DECODE(A.GUMSADATE_FR 	, NULL, '', TRIM(TO_CHAR(A.GUMSADATE_FR, 'YYYY-MM-DD')) || ' ' || TRIM(TO_CHAR(A.GUMSAHH_FR,'00')) || ':' || TRIM(TO_CHAR(A.GUMSAMM_FR,'00'))) AS GUMSADATE_FR_FULL \r\n";
            SQL += "        , A.PANO                                                                                                                                                                                        \r\n";
            SQL += "        , B.SNAME                                                                                                                                                                                       \r\n";
            SQL += "        , B.SEX                                                                                                                                                                                         \r\n";
            SQL += "        , A.SPECNO                                                                                                                                                                                      \r\n";
            SQL += "        , A.DEPT                                                                                                                                                                                        \r\n";
            SQL += "        , A.WARD                                                                                                                                                                                        \r\n";
            SQL += "        , A.ROOM                                                                                                                                                                                        \r\n";
            SQL += "        , A.COMPONENT || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_COMPONENT',A.COMPONENT)  AS COMPONENT                                                                                            \r\n";
            SQL += "        , SUBSTR(A.BLOODNO,1,2) ||'-'|| SUBSTR(A.BLOODNO,3,2) || '-' || SUBSTR(A.BLOODNO,5)     AS BLOODNO                                                                                              \r\n";
            SQL += "        , A.CAPACITY -- 혈액량                                                                                                                                                                          \r\n";
            SQL += "        , TRIM(A.DONNORABO) AS DONNORABO                                                                                                                                                                \r\n";
            SQL += "        , TRIM(A.DONNORRH)  AS DONNORRH                                                                                                                                                                 \r\n";
            SQL += "        , TRIM(A.PTABO)     AS PTABO                                                                                                                                                                    \r\n";
            SQL += "  	    , TRIM(A.PTRH)      AS PTRH                                                                                                                                                                     \r\n";
            SQL += "  	    , TRIM(A.ANTIB)     AS ANTIB	-- AntiBody Screening                                                                                                                                           \r\n";
            SQL += "  	    , TRIM(A.MAJOR)     AS MAJOR	-- Major                                                                                                                                                        \r\n";
            SQL += "  	    , TRIM(A.SALINE)    AS SALINE	-- Saline                                                                                                                                                       \r\n";
            SQL += "  	    , TRIM(A.ALBUMN)    AS ALBUMN	-- 37C Albumin                                                                                                                                                  \r\n";
            SQL += "  	    , TRIM(A.ANTIG)     AS ANTIG	-- Anti Globulin                                                                                                                                                \r\n";
            SQL += "  	    , KOSMOS_OCS.FC_BAS_USER_NAME(GUMSAJA) AS GUMSAJA                                                                                                                                               \r\n";
            SQL += "  	    , CANCAUSE                                                                                                                                                                                      \r\n";
            SQL += "  	    , KOSMOS_OCS.FC_BAS_USER_NAME(CNCL_PS) AS CNCL_PS                                                                                                                                               \r\n";
            SQL += "  	    , GBSTATUS                                                                                                                                                                                      \r\n";
            SQL += "  	    , A.ROWID                                                                                                                                                                                       \r\n";
            SQL += "  	    , A.SEQNO                                                                                                                                                                                       \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_BLOODCROSSM A                                                                                                                                                                  \r\n";
            SQL += "       , KOSMOS_PMPA.BAS_PATIENT     B                                                                                                                                                                  \r\n";
            SQL += "   WHERE 1=1                                                                                                                                                                                            \r\n";


            if (string.IsNullOrEmpty(strTXT) == false)
            {
                SQL += "     AND (A.PANO LIKE '%" + strTXT + "%' OR SPECNO LIKE '%" + strTXT + "%' OR BLOODNO LIKE '%" + strTXT + "%')                                                                                     \r\n";
            }
            else
            {
                SQL += "     AND A.JUBSUDATE BETWEEN " + ComFunc.covSqlDate(strGUMSADATE_FR, false);
                SQL += "     				     AND " + ComFunc.covSqlDate(strGUMSADATE_TO, false);
            }

            SQL += "     AND A.PANO	 	= B.PANO                                                                                                                                                                            \r\n";

            SQL += "     AND A.GBSTATUS =  " + ComFunc.covSqlstr(strGBSTATUS, false);
            SQL += "   ORDER BY A.GUMSADATE, A.PANO                                                                                                                                                                         \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

            return ds;
        }

        public string up_EXAM_BLOOD_IO(PsmhDb pDbCon, string strBLOODNO, string strCOMPONENT, string strGBSTATUS, string strDATE, string strPERSON, string strCAUSE, string strCAUSE_CD, string strDAEYU, bool isOUT, ref int intRowAffected, string strTDate = "")
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  UPDATE KOSMOS_OCS.EXAM_BLOOD_IO       \r\n";



                // 입고반납
                if (strGBSTATUS.Equals("R"))
                {
                    SQL += "     SET GBSTATUS        = " + ComFunc.covSqlstr(strGBSTATUS.Trim(), false);
                    SQL += "       , STORERDATE      = " + ComFunc.covSqlDate(strDATE, false);
                    SQL += "       , STORERPERSON    = " + ComFunc.covSqlstr(strPERSON.Trim(), false);
                    SQL += "       , STORERCAUSE     = " + ComFunc.covSqlstr(strCAUSE.Trim(), false);
                    SQL += "       , STORERCASE_CD   = " + ComFunc.covSqlstr(strCAUSE_CD.Trim(), false);
                }

                // 대여
                else if (strGBSTATUS.Equals("D"))
                {
                    SQL += "     SET GBSTATUS        = " + ComFunc.covSqlstr(strGBSTATUS.Trim(), false);
                    SQL += "       , DAEYUDATE       = " + ComFunc.covSqlDate(strDATE, false);
                    SQL += "       , DAEYU           = " + ComFunc.covSqlstr(strDAEYU.Trim(), false);
                }
                // 폐기
                else if (strGBSTATUS.Equals("P"))
                {
                    SQL += "     SET GBSTATUS        = " + ComFunc.covSqlstr(strGBSTATUS.Trim(), false);
                    SQL += "       , DESTROYDATE     = " + ComFunc.covSqlDate(strDATE, false);
                    SQL += "       , DESTROYPERSON   = " + ComFunc.covSqlstr(strPERSON.Trim(), false);
                    SQL += "       , DESTROYCAUSE    = " + ComFunc.covSqlstr(strCAUSE.Trim(), false);
                    SQL += "       , DESTROYCASE_CD = " + ComFunc.covSqlstr(strCAUSE_CD.Trim(), false);
                }
                // 매칭, 매칭취소
                else if (strGBSTATUS.Equals("M") || strGBSTATUS.Equals("A"))
                {
                    // SQL += "       , DESTROYDATE     = " + ComFunc.covSqlDate(strDATE, false);
                    SQL += "     SET GBSTATUS        = " + ComFunc.covSqlstr(strGBSTATUS.Trim(), false);
                }

                else if (strGBSTATUS.Equals("C") && isOUT == true)
                { 
                    //2018.01.16.김홍록: 알수 없다. 입고 반납과, 출고 반납의 경계를 모르겠음.
                    SQL += "     SET GBSTATUS      = 'R'";
                    if (strTDate != "")
                    {
                        //SQL += "       , OUTRDATE      = '" + strTDate + "'";
                        //2019-06-05 안정수 수정 
                        SQL += "       , OUTRDATE      = TO_DATE('" + strTDate + "', 'YYYY-MM-DD HH24:MI')";
                    }
                    else
                    {
                        
                        SQL += "       , OUTRDATE      = " + ComFunc.covSqlDate(strDATE, false);                        
                    }
                    SQL += "       , OUTRPERSON    = " + ComFunc.covSqlstr(strPERSON.Trim(), false);
                    SQL += "       , OUTRCAUSE     = " + ComFunc.covSqlstr(strCAUSE.Trim(), false);
                    SQL += "       , OUTRCASE_CD   = " + ComFunc.covSqlstr(strCAUSE_CD.Trim(), false);
                }

                SQL += "   WHERE 1=1                            \r\n";
                SQL += "     AND BLOODNO             = " + ComFunc.covSqlstr(strBLOODNO.Trim(), false);
                SQL += "     AND COMPONENT           = " + ComFunc.covSqlstr(strCOMPONENT.Trim(), false);

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public string up_EXAM_BLOOD_IO_OUT(PsmhDb pDbCon, string strBLOODNO, string strCOMPONENT, string strDATE, string strPERSON, string strPANO, string strSNAME, string strWARD, string strROOM, string strNURSE, string strOUTBUSE, string strER, string strSAMOUT, string strDEPT, string strDR, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  UPDATE KOSMOS_OCS.EXAM_BLOOD_IO       \r\n";
                SQL += "     SET GBSTATUS       = 'C'          \r\n";
                SQL += "       , OUTDATE        = TO_DATE('" + strDATE + "', 'YYYY-MM-DD HH24:MI')   \r\n";
                SQL += "       , OUTPERSON      = " + ComFunc.covSqlstr(strPERSON.Trim(), false);
                SQL += "       , PANO           = " + ComFunc.covSqlstr(strPANO.Trim(), false);
                SQL += "       , SNAME          = " + ComFunc.covSqlstr(strSNAME.Trim(), false);
                SQL += "       , WARD           = " + ComFunc.covSqlstr(strWARD.Trim(), false);
                SQL += "       , ROOM           = " + ComFunc.covSqlstr(strROOM.Trim(), false);
                SQL += "       , NURSE          = " + ComFunc.covSqlstr(strNURSE.Trim(), false);
                SQL += "       , DEPT           = " + ComFunc.covSqlstr(strDEPT.Trim(), false);
                SQL += "       , DR             = " + ComFunc.covSqlstr(strDR.Trim(), false);
                SQL += "       , OUTBUSE        = " + ComFunc.covSqlstr(strOUTBUSE.Trim(), false);
                SQL += "       , EMERGENCY      = " + ComFunc.covSqlstr(strER.Trim(), false);
                SQL += "       , SAMEOUT        = " + ComFunc.covSqlstr(strSAMOUT.Trim(), false);


                SQL += "   WHERE 1=1                            \r\n";
                SQL += "     AND BLOODNO             = " + ComFunc.covSqlstr(strBLOODNO.Trim(), false);
                SQL += "     AND COMPONENT           = " + ComFunc.covSqlstr(strCOMPONENT.Trim(), false);

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public DataSet sel_EXAM_BLOOD_IO_EXCEL(PsmhDb pDbCon, string strFDATE, string strTDATE, string ABO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                                                    \r\n";
            SQL += "  	    SUBSTR(BLOODNO,1,2) ||'-'|| SUBSTR(BLOODNO,3,2) || '-' || SUBSTR(BLOODNO,5) AS BLOODNO              \r\n";
            SQL += "  	  , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_EXCEL_CAPA', COMPONENT||CAPACITY)	AS COM_CAPA             \r\n";
            SQL += "  	  , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_COMPONENT', COMPONENT) 				AS COMPONENT            \r\n";
            SQL += "  	  , DECODE(GBSTATUS,'P','N','Y')											    AS GBSTATUS             \r\n";
            SQL += "        , TO_CHAR(A.STOREDATE,'YYYY-MM-DD')                                         AS SDATE                \r\n";
            SQL += "        , TO_CHAR(A.DRAWDATE ,'YYYY-MM-DD')                                         AS DRAWDATE             \r\n";
            SQL += "  	  , TO_CHAR(A.STORETIME ,'HH24:MI')                                             AS STORETIME            \r\n";
            SQL += "  	  , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_EXCEL_ABO', ABO)    				    AS ABO_EXCEL            \r\n";
            SQL += "  	  , SUBSTR(ABO,1,LENGTH(ABO)-1) ||'(' || SUBSTR(ABO,LENGTH(ABO),1) || ')'       AS ABO                  \r\n";
            SQL += "  	  , DECODE(KOSMOS_OCS.FC_BAS_USER_NAME(STOREPERSON),''                                                  \r\n";
            SQL += "  	  		  ,STOREPERSON,KOSMOS_OCS.FC_BAS_USER_NAME(STOREPERSON))				AS STOREPERSON          \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_BLOOD_IO A                                                                         \r\n";
            SQL += "   WHERE 1=1                                                                                                \r\n";

            if (!ABO.Equals("*"))
            {
                SQL += "     AND  ABO IN ('" + ABO + "+','" + ABO + "-')        \r\n";
            }

            SQL += "       AND  STOREDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
            SQL += "                          AND " + ComFunc.covSqlDate(strTDATE, false);
            SQL += "     ORDER BY STOREDATE, EXPIRE, BLOODNO                                                          \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

            return ds;
        }

        public DataSet sel_EXAM_BLOODTRANS_SUM(PsmhDb pDbCon, string strFDATE, string strTDATE)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  WITH T AS(                                                     \r\n";
            SQL += "  SELECT DECODE(GBJOB, '1', COUNT(BLOODNO),'0') AS BLOOD_IN      \r\n";
            SQL += "       , DECODE(GBJOB, '4', COUNT(BLOODNO),'0') AS BLOOD_OUT     \r\n";
            SQL += "       , DECODE(GBJOB, '3', COUNT(BLOODNO),'0') AS BLOOD_MATCH   \r\n";
            SQL += "       , DECODE(GBJOB, '6', COUNT(BLOODNO),'0') AS BLOOD_RANTAL  \r\n";

            SQL += "       , DECODE(GBJOB, '7', COUNT(BLOODNO),'0') AS BLOOD_DESTROY \r\n";
            SQL += "       , DECODE(GBJOB, '2', COUNT(BLOODNO),'0') AS BLOOD_RETURN  \r\n";
            SQL += "       , DECODE(GBJOB, '5', COUNT(BLOODNO),'0') AS BLOOD_OUT_RETURN  \r\n";

            SQL += "    FROM KOSMOS_OCS.EXAM_BLOODTRANS                              \r\n";
            SQL += "   WHERE 1=1                                                     \r\n";
            SQL += "     AND  BDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
            SQL += "                    AND " + ComFunc.covSqlDate(strTDATE, false);
            SQL += "  GROUP BY GBJOB                                                 \r\n";
            SQL += "  )                                                              \r\n";
            SQL += "  SELECT                                                         \r\n";
            SQL += "       TO_CHAR(MAX(BLOOD_IN)) 		AS BLOOD_IN                          \r\n";
            SQL += "  	 , TO_CHAR(MAX(BLOOD_OUT)) 		AS BLOOD_OUT                         \r\n";
            SQL += "  	 , TO_CHAR(MAX(BLOOD_MATCH)) 	AS BLOOD_MATCH                       \r\n";
            SQL += "  	 , TO_CHAR(MAX(BLOOD_RANTAL)) 	AS BLOOD_RANTAL                      \r\n";
            SQL += "  	 , TO_CHAR(MAX(BLOOD_DESTROY)) 	AS BLOOD_DESTROY                     \r\n";
            SQL += "  	 , TO_CHAR(MAX(BLOOD_RETURN)) 	AS BLOOD_RETURN                      \r\n";
            SQL += "  	 , TO_CHAR(MAX(BLOOD_OUT_RETURN)) 	AS BLOOD_OUT_RETURN                      \r\n";
            SQL += "  	 , TO_CHAR(MAX(BLOOD_IN) + MAX(BLOOD_OUT) + MAX(BLOOD_MATCH) + MAX(BLOOD_RANTAL) + MAX(BLOOD_DESTROY) + MAX(BLOOD_RETURN)) AS BLOOD_SUM                      \r\n";
            SQL += "    FROM T                                                       \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

            return ds;
        }

        public DataSet sel_EXAM_BLOOD_IO_ING(PsmhDb pDbCon, string strFDATE, string strTDATE, string ABO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                                           \r\n";
            SQL += "  	   	TO_CHAR(A.STOREDATE,'YYYY-MM-DD')                                           AS SDATE       \r\n";
            SQL += "  	  , SUBSTR(BLOODNO,1,2) ||'-'|| SUBSTR(BLOODNO,3,2) || '-' || SUBSTR(BLOODNO,5) AS BLOODNO     \r\n";
            SQL += "  	  , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_COMPONENT', COMPONENT) 				AS COMPONENT   \r\n";
            SQL += "  	  , (SELECT NAME FROM KOSMOS_OCS.EXAM_BLOOD_COMM WHERE TRIM(CODE) = STORE)      AS STORE       \r\n";
            SQL += "  	  , TO_CHAR(A.DRAWDATE,'YYYY-MM-DD')    										AS DRAWDATE      \r\n";
            SQL += "  	  , TO_CHAR(A.EXPIRE,'YYYY-MM-DD')    											AS EXDATE      \r\n";
            SQL += "  	  , DECODE(KOSMOS_OCS.FC_BAS_USER_NAME(STOREPERSON),'',STOREPERSON,KOSMOS_OCS.FC_BAS_USER_NAME(STOREPERSON))									AS STOREPERSON \r\n";
            SQL += "  	  , SUBSTR(ABO,1,LENGTH(ABO)-1)													AS ABO         \r\n";
            SQL += " 	  , SUBSTR(ABO,LENGTH(ABO),1)													AS RH          \r\n";
            SQL += " 	  , CAPACITY																	AS CAPACITY    \r\n";
            SQL += "      , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_GBSTATUS', GBSTATUS)				    AS GBSTATUS    \r\n";
            SQL += "      , TRIM(TO_CHAR(TO_NUMBER(AMT),'9,999,999'))									AS AMT         \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_BLOOD_IO A                                                                \r\n";
            SQL += "   WHERE 1=1                                                                                       \r\n";

            if (!ABO.Equals("*"))
            {
                SQL += "     AND  ABO IN ('" + ABO + "+','" + ABO + "-')        \r\n";
            }

            SQL += "       AND  STOREDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
            SQL += "                          AND " + ComFunc.covSqlDate(strTDATE, false);
            SQL += "     ORDER BY STOREDATE, EXPIRE, BLOODNO                                                          \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

            return ds;
        }

        public string del_EXAM_BLOOD_IO(PsmhDb pDbCon, string strBLOODNO, string strCOMPONENT, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  DELETE KOSMOS_OCS.EXAM_BLOOD_IO     \r\n";
                SQL += "   WHERE BLOODNO    = " + ComFunc.covSqlstr(strBLOODNO, false);
                SQL += "     AND COMPONENT  = " + ComFunc.covSqlstr(strCOMPONENT, false);


                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public string del_EXAM_BLOODTRANS(PsmhDb pDbCon, string strBLOODNO, string strCOMPONENT, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  DELETE KOSMOS_OCS.EXAM_BLOODTRANS     \r\n";
                SQL += "   WHERE BLOODNO    = " + ComFunc.covSqlstr(strBLOODNO, false);
                SQL += "     AND COMPONENT  = " + ComFunc.covSqlstr(strCOMPONENT, false);


                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        //public string ins_EXAM_BLOODTRANS(PsmhDb pDbCon, string strBDATE, string strGBJOB, string strBLOODNO, string strENTSABUN, string strCOMPONENT, string strCAPACITY, string strREMARK, string strPANO, string strGUMSADATE, string strNURSE, ref int intRowAffected)
        //{
        //    string SqlErr = "";

        //    try
        //    {
        //        SQL = "";                
        //        SQL += "  MERGE INTO KOSMOS_OCS.EXAM_BLOODTRANS             \r\n";
        //        SQL += "       USING DUAL                                   \r\n";
        //        SQL += "          ON (                                      \r\n";
        //        SQL += "          	    COMPONENT   = " + ComFunc.covSqlstr(strCOMPONENT, false);
        //        SQL += "            AND BLOODNO     = " + ComFunc.covSqlstr(strBLOODNO, false);
        //        SQL += "            AND GBJOB       = " + ComFunc.covSqlstr(strGBJOB, false);
        //        SQL += "             )                                      \r\n";
        //        SQL += " 	WHEN MATCHED THEN                               \r\n";
        //        SQL += " 		UPDATE SET REMARK   = " + ComFunc.covSqlstr(strREMARK, false);
        //        SQL += " 		         , ENTDATE  = SYSDATE               \r\n";
        //        SQL += " 		         , ENTSABUN = " + ComFunc.covSqlstr(strENTSABUN, false);
        //        SQL += "    WHEN NOT MATCHED THEN                           \r\n";

        //        if (string.IsNullOrEmpty(strGUMSADATE) == false)
        //        {
        //            SQL += "  INSERT (BDATE, GBJOB, BLOODNO, ENTDATE, ENTSABUN, COMPONENT, CAPACITY, REMARK, PANO, NURSE, GUMSADATE ) VALUES (       \r\n";
        //        }
        //        else
        //        {
        //            SQL += "  INSERT (BDATE, GBJOB, BLOODNO, ENTDATE, ENTSABUN, COMPONENT, CAPACITY, REMARK, PANO, NURSE) VALUES (       \r\n";
        //        }

        //        SQL += "  " + ComFunc.covSqlDate(strBDATE, false);
        //        SQL += "  " + ComFunc.covSqlstr(strGBJOB, true);
        //        SQL += "  " + ComFunc.covSqlstr(strBLOODNO, true);
        //        SQL += "  , SYSDATE                                         \r\n";
        //        SQL += "  " + ComFunc.covSqlstr(strENTSABUN, true);
        //        SQL += "  " + ComFunc.covSqlstr(strCOMPONENT, true);
        //        SQL += "  " + ComFunc.covSqlstr(strCAPACITY, true);
        //        SQL += "  " + ComFunc.covSqlstr(strREMARK, true);
        //        if (string.IsNullOrEmpty(strPANO) == true)
        //        {
        //            SQL += "  , (SELECT PANO FROM KOSMOS_OCS.EXAM_BLOODTRANS WHERE 1=1              \r\n";
        //            SQL += "                  AND BLOODNO   = " + ComFunc.covSqlstr(strBLOODNO, false);
        //            SQL += "                  AND COMPONENT = " + ComFunc.covSqlstr(strCOMPONENT, false);
        //            SQL += "                  AND GBJOB     = '4'                                   \r\n";
        //            SQL += "                  AND ENTDATE   = (SELECT MAX(ENTDATE)                  \r\n";
        //            SQL += "                                     FROM KOSMOS_OCS.EXAM_BLOODTRANS    \r\n";
        //            SQL += "                                    WHERE 1=1                           \r\n";
        //            SQL += "                                      AND BLOODNO   = " + ComFunc.covSqlstr(strBLOODNO, false);
        //            SQL += "                                      AND COMPONENT = " + ComFunc.covSqlstr(strCOMPONENT, false);
        //            SQL += "                                      AND GBJOB     = '4'               \r\n";
        //            SQL += "                                   )                                    \r\n";
        //            SQL += "    )                                                                   \r\n";

        //        }
        //        else
        //        {
        //            SQL += "  " + ComFunc.covSqlstr(strPANO, true);
        //        }

        //        SQL += "  " + ComFunc.covSqlstr(strNURSE, true);

        //        if (string.IsNullOrEmpty(strGUMSADATE) == false)
        //        {
        //            SQL += "  " + ComFunc.covSqlDate(strGUMSADATE, true);
        //        }

        //        SQL += "  )                                                                     \r\n";


        //        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

        //        if (SqlErr != "")
        //        {
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
        //            return SqlErr;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
        //        return ex.Message.ToString();
        //    }


        //    return SqlErr;
        //}

        public string ins_EXAM_BLOODTRANS(PsmhDb pDbCon, string strBDATE, string strGBJOB, string strBLOODNO, string strENTSABUN, string strCOMPONENT, string strCAPACITY, string strREMARK, string strPANO, string strGUMSADATE, string strNURSE, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";

                if (string.IsNullOrEmpty(strGUMSADATE) == false)
                {
                    SQL += "  INSERT INTO KOSMOS_OCS.EXAM_BLOODTRANS (BDATE, GBJOB, BLOODNO, ENTDATE, ENTSABUN, COMPONENT, CAPACITY, REMARK, PANO, NURSE, GUMSADATE ) VALUES (       \r\n";
                }
                else
                {
                    SQL += "  INSERT INTO KOSMOS_OCS.EXAM_BLOODTRANS (BDATE, GBJOB, BLOODNO, ENTDATE, ENTSABUN, COMPONENT, CAPACITY, REMARK, PANO, NURSE) VALUES (       \r\n";
                }

                SQL += "  " + ComFunc.covSqlDate(strBDATE, false);
                SQL += "  " + ComFunc.covSqlstr(strGBJOB, true);
                SQL += "  " + ComFunc.covSqlstr(strBLOODNO, true);
                SQL += "  , SYSDATE                                         \r\n";
                SQL += "  " + ComFunc.covSqlstr(strENTSABUN, true);
                SQL += "  " + ComFunc.covSqlstr(strCOMPONENT, true);
                SQL += "  " + ComFunc.covSqlstr(strCAPACITY, true);
                SQL += "  " + ComFunc.covSqlstr(strREMARK, true);

                if (string.IsNullOrEmpty(strPANO) == true)
                {
                    SQL += "  , (SELECT PANO FROM KOSMOS_OCS.EXAM_BLOODTRANS WHERE 1=1              \r\n";
                    SQL += "                  AND BLOODNO   = " + ComFunc.covSqlstr(strBLOODNO, false);
                    SQL += "                  AND COMPONENT = " + ComFunc.covSqlstr(strCOMPONENT, false);
                    SQL += "                  AND GBJOB     = '4'                                   \r\n";
                    SQL += "                  AND ENTDATE   = (SELECT MAX(ENTDATE)                  \r\n";
                    SQL += "                                     FROM KOSMOS_OCS.EXAM_BLOODTRANS    \r\n";
                    SQL += "                                    WHERE 1=1                           \r\n";
                    SQL += "                                      AND BLOODNO   = " + ComFunc.covSqlstr(strBLOODNO, false);
                    SQL += "                                      AND COMPONENT = " + ComFunc.covSqlstr(strCOMPONENT, false);
                    SQL += "                                      AND GBJOB     = '4'               \r\n";
                    SQL += "                                   )                                    \r\n";
                    SQL += "    )                                                                   \r\n";

                }
                else
                {
                    SQL += "  " + ComFunc.covSqlstr(strPANO, true);
                }

                SQL += "  " + ComFunc.covSqlstr(strNURSE, true);

                if (string.IsNullOrEmpty(strGUMSADATE) == false)
                {
                    SQL += "  " + ComFunc.covSqlDate(strGUMSADATE, true);
                }

                SQL += "  )                                                                     \r\n";


                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public string ins_EXAM_BLOODTRANS_1(PsmhDb pDbCon, DataRow dr, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  INSERT INTO KOSMOS_OCS.EXAM_BLOODTRANS (BDATE, GBJOB, BLOODNO, ENTDATE, ENTSABUN, COMPONENT, NURSE, CAPACITY) VALUES (       \r\n";
                SQL += "  " + ComFunc.covSqlDate(dr[(int)enmSel_EXAM_BLOODTRANS_STORE.BDATE].ToString(), false);
                SQL += "  , '1'         \r\n";
                SQL += "  " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_BLOODTRANS_STORE.BLOODNO].ToString(), true);
                SQL += "  , SYSDATE     \r\n";
                SQL += "  " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_BLOODTRANS_STORE.ENTSABUN].ToString(), true);
                SQL += "  " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_BLOODTRANS_STORE.COMPONENT].ToString(), true);
                SQL += "  , 0           \r\n";
                SQL += "  " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_BLOODTRANS_STORE.CAPACITY].ToString(), true);
                SQL += "  )                                                                                 \r\n";


                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public string ins_EXAM_BLOOD_IO(PsmhDb pDbCon, DataRow dr, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  INSERT INTO KOSMOS_OCS.EXAM_BLOOD_IO (BLOODNO, ABO, COMPONENT, CAPACITY, EXPIRE, STOREDATE, STOREPERSON, STORE, AMT, DRAWDATE, STORETIME) VALUES (       \r\n";
                SQL += "  " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_BLOOD_IO_INFO.BLOODNO].ToString().Trim(), false);
                SQL += "  " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_BLOOD_IO_INFO.ABO].ToString().Trim(), true);
                SQL += "  " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_BLOOD_IO_INFO.COMPONENT].ToString().Trim(), true);
                SQL += "  " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_BLOOD_IO_INFO.CAPACITY].ToString().Trim(), true);
                SQL += "  " + ComFunc.covSqlDate(dr[(int)enmSel_EXAM_BLOOD_IO_INFO.EXPIRE].ToString().Trim(), true);
                SQL += "  " + ComFunc.covSqlDate(dr[(int)enmSel_EXAM_BLOOD_IO_INFO.STOREDATE].ToString().Trim(), true);
                SQL += "  " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_BLOOD_IO_INFO.STOREPERSON].ToString().Trim(), true);
                SQL += "  " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_BLOOD_IO_INFO.STORE].ToString().Trim(), true);
                SQL += "  " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_BLOOD_IO_INFO.AMT].ToString().Trim(), true);
                SQL += "  " + ComFunc.covSqlDate(dr[(int)enmSel_EXAM_BLOOD_IO_INFO.DRAWDATE].ToString().Trim(), true);
                SQL += "  , SYSDATE                                                                         \r\n";
                SQL += "  )                                                                                 \r\n";


                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public DataTable sel_EXAM_BLOODTRANS_STORE(PsmhDb pDbCon, string strBLOODNO, string strCOMPONENT)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                           \r\n";
            SQL += "        TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE      \r\n";
            SQL += "      , BLOODNO                                   \r\n";
            SQL += "      , ENTSABUN                                  \r\n";
            SQL += "      , COMPONENT                                 \r\n";
            SQL += "      , CAPACITY                                  \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_BLOODTRANS                 \r\n";
            SQL += " WHERE 1=1                                        \r\n";
            SQL += "   AND BLOODNO      = " + ComFunc.covSqlstr(strBLOODNO, false);
            SQL += "   AND COMPONENT    = " + ComFunc.covSqlstr(strCOMPONENT, false);

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

        public DataSet sel_EXAM_BLOOD_IO_JAN(PsmhDb pDbCon, string strIO, string strFDATE, string strTDATE, string strCOMPONENT, string strPANO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                                                                                                                     \r\n";
            SQL += "  		  '' CHK                                                        \r\n";
            SQL += "  		, BLOODNO                                                       \r\n";
            SQL += "  		, COMPONENT                                                     \r\n";
            SQL += "  		, TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE                         \r\n";
            SQL += "  		, TO_CHAR(JANDATE,'YYYY-MM-DD') JANDATE                         \r\n";
            SQL += "  		, KOSMOS_OCS.FC_BAS_USER_NAME(TRIM(JANSABUN)) AS JANSABUN       \r\n";
            SQL += "  		, ABO                                                           \r\n";
            SQL += "  		, CAPACITY  || 'ml'        AS CAPACITY                          \r\n";
            SQL += "  		, JANSU                                                         \r\n";
            SQL += "  		, JANPAE                                                        \r\n";
            SQL += "  		, JANREMARK                                                     \r\n";
            SQL += "  		, PANO                                                          \r\n";
            SQL += "  		, SNAME                                                         \r\n";
            SQL += "  		, WARD                                                          \r\n";
            SQL += "  		, ROOM                                                          \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_BLOOD_IO                                        \r\n";
            SQL += "   WHERE 1=1                                                                                                                                                \r\n";
            SQL += "     AND JANDATE BETWEEN TO_DATE('" + strFDATE + "','YYYY-MM-DD')                                                                                             \r\n";
            SQL += " 			       AND TO_DATE('" + strTDATE + "','YYYY-MM-DD')                                                                                             \r\n";

            if (strIO.Equals("I"))
            {
                SQL += "     AND ROOM <> 0    \r\n";
            }
            else if (strIO.Equals("O"))
            {
                SQL += "     AND ROOM = 0    \r\n";
            }

            if (string.IsNullOrEmpty(strPANO) == false)
            {
                SQL += "     AND PANO = " + ComFunc.covSqlstr(strPANO, false);
            }

            if (strCOMPONENT.Equals("*") == false)
            {
                SQL += "     AND COMPONENT = " + ComFunc.covSqlstr(strCOMPONENT, false);
            }


            SQL += "  ORDER BY BLOODNO              \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

            return ds;

        }

        public DataSet sel_EXAM_BLOOD_IO_CROSS(PsmhDb pDbCon, string strGBJOB, string strIO, string strFDATE, string strTDATE, string strCOMPONENT, string strPANO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                                                                                                                     \r\n";
            SQL += " 		  '' CHK  --1                                                                                                                                       \r\n";
            SQL += " 		, E.SNAME --2                                                                                                                                       \r\n";
            SQL += " 		, A.PANO  --3                                                                                                                                       \r\n";
            SQL += " 		, KOSMOS_OCS.FC_EMR_TREATT_ISNULL(A.PANO, DECODE(NVL(B.WARD,'*'),'*','O','I'), TO_CHAR(A.ENTDATE,'YYYY-MM-DD HH24:MI'),B.DEPT) 	AS EMR_SCAN --4     \r\n";
            SQL += " 		, D.SEX                         -- 5                                                                                                                \r\n";
            SQL += " 		, KOSMOS_OCS.FC_GET_AGE(KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(A.PANO), SYSDATE)		    AS AGE						-- 6                                \r\n";
            SQL += " 		, TO_CHAR(KOSMOS_OCS.FC_IPD_NEW_MASTER_JROOM(A.PANO))   							AS ROOM						-- 6                                \r\n";
            SQL += " 		, D.ABO_N	--7                                                                                                                                     \r\n";
            SQL += " 		, D.SERUM   --8                                                                                                                                     \r\n";
            SQL += " 		, D.RH      --9                                                                                                                                     \r\n";
            SQL += " 		, C.JOBTYPE ||'.'||KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_CROSS_JOBTYPE',C.JOBTYPE) AS JOBTYPE                                                      \r\n";
            SQL += " 		, SUBSTR(A.BLOODNO,1,2) ||'-'|| SUBSTR(A.BLOODNO,3,2) || '-' || SUBSTR(A.BLOODNO,5) AS BLOODNO	--10                                                \r\n";
            SQL += " 		, KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_COMPONENT', B.COMPONENT)		   			AS COMPONENT--11                                                \r\n";
            SQL += "        , B.CAPACITY || ' ml'																AS CAPACITY --11                                                \r\n";
            SQL += "        , C.ANTIB	-- 12                                                                                                                                   \r\n";
            SQL += " 		, C.SALINE	-- 13                                                                                                                                   \r\n";
            SQL += " 		, C.ALBUMN  -- 14                                                                                                                                   \r\n";
            SQL += " 		, C.ANTIG	-- 15                                                                                                                                   \r\n";
            SQL += " 		, KOSMOS_OCS.FC_BAS_USER_NAME(C.GUMSAJA)										   AS GUMSAJA --16                                                  \r\n";
            SQL += "        , TO_CHAR(C.GUMSADATE,'YYYY-MM-DD') || ' ' || C.GUMSAHH || ':' || C.GUMSAMM        AS GUMSATIME --17                                                \r\n";
            SQL += " 		, TO_CHAR(B.OUTDATE,'YYYY-MM-DD HH24:MI') 										   AS OUTDATE   --18                                                \r\n";
            SQL += " 		, KOSMOS_OCS.FC_BAS_USER_NAME(B.NURSE)											   AS NURSE                                                         \r\n";
            SQL += " 		, C.ROWID                                                                                                                                           \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_BLOODTRANS   A                                                                                                                      \r\n";
            SQL += "   	  , KOSMOS_OCS.EXAM_BLOOD_IO     B                                                                                                                      \r\n";
            SQL += "   	  , KOSMOS_OCS.EXAM_BLOODCROSSM  C                                                                                                                      \r\n";
            SQL += "   	  , KOSMOS_OCS.EXAM_BLOOD_MASTER D                                                                                                                      \r\n";
            SQL += "   	  , KOSMOS_PMPA.BAS_PATIENT      E                                                                                                                      \r\n";
            SQL += "   WHERE 1=1                                                                                                                                                \r\n";
            SQL += "     AND BDATE BETWEEN TO_DATE('" + strFDATE + "','YYYY-MM-DD')                                                                                             \r\n";
            SQL += " 			       AND TO_DATE('" + strTDATE + "','YYYY-MM-DD')                                                                                             \r\n";

            if (strGBJOB.Equals("*"))
            {
                SQL += "     AND A.GBJOB  		IN ('4','5','7')                                                                                                                \r\n";
            }
            else
            {
                SQL += "     AND A.GBJOB  		= " + ComFunc.covSqlstr(strGBJOB, false);
            }

            if (strIO.Equals("I"))
            {
                SQL += "     AND B.ROOM <> 0    \r\n";
            }
            else if (strIO.Equals("O"))
            {
                SQL += "     AND B.ROOM = 0    \r\n";
            }

            if (string.IsNullOrEmpty(strPANO) == false)
            {
                SQL += "     AND A.PANO = " + ComFunc.covSqlstr(strPANO, false);
            }

            if (strCOMPONENT.Equals("*") == false)
            {
                SQL += "     AND B.COMPONENT = " + ComFunc.covSqlstr(strCOMPONENT, false);
            }



            SQL += "    AND A.BLOODNO = B.BLOODNO       \r\n";
            SQL += "    AND A.COMPONENT = B.COMPONENT   \r\n";
            SQL += "    AND A.PANO = E.PANO(+)          \r\n";
            SQL += "    AND A.PANO = D.PANO(+)          \r\n";
            SQL += "    AND A.BLOODNO = C.BLOODNO(+)    \r\n";
            SQL += "    AND A.PANO    = C.PANO(+)       \r\n";
            SQL += "  ORDER BY C.GUMSADATE              \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

            return ds;

        }

        public DataSet sel_EXAM_BLOOD_IO_OUT(PsmhDb pDbCon, string strGBJOB, string strIO, string strFDATE, string strTDATE, string strCOMPONENT, string strPANO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                                                                                                                     \r\n";
            SQL += " 		 ''	CHK	--1                                                                                                                                         \r\n";
            SQL += "         , TO_CHAR(A.ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE --2                                                                                              \r\n";
            SQL += " 		, B.WARD	-- 3                                                                                                                                    \r\n";
            SQL += " 		, B.DEPT    -- 4                                                                                                                                    \r\n";
            SQL += " 		, C.PANO    -- 5                                                                                                                                    \r\n";
            SQL += " 		, D.SNAME   -- 6                                                                                                                                    \r\n";
            SQL += " 		, KOSMOS_OCS.FC_EMR_TREATT_ISNULL(A.PANO, DECODE(NVL(B.WARD,'*'),'*','O','I'), TO_CHAR(A.ENTDATE,'YYYY-MM-DD HH24:MI'),B.DEPT) 	AS EMR_SCAN --7     \r\n";
            SQL += "         , SUBSTR(B.BLOODNO,1,2) ||'-'|| SUBSTR(B.BLOODNO,3,2) || '-' || SUBSTR(B.BLOODNO,5) 											AS BLOODNO	--8     \r\n";
            SQL += " 		, C.PTABO     || C.PTRH     																									AS PTAR     --9     \r\n";
            SQL += " 		, C.DONNORABO || C.DONNORRH 																									AS DNAR		--10    \r\n";
            SQL += " 		, DECODE(NVL(A.PANO,'*'), '*','','Comp')																						AS COMP		--11    \r\n";
            SQL += " 		, KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_COMPONENT', B.COMPONENT)		   													    AS COMPONENT--12    \r\n";
            SQL += " 		, B.CAPACITY || ' ml'																											AS CAPACITY --13    \r\n";
            SQL += " 		, KOSMOS_OCS.FC_BAS_USER_NAME(A.ENTSABUN)																						AS ENTSABUN --14    \r\n";
            SQL += " 		, KOSMOS_OCS.FC_BAS_USER_NAME(B.NURSE)																							AS NURSE    --15    \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_BLOODTRANS  A                                                                                                                      \r\n";
            SQL += "       , KOSMOS_OCS.EXAM_BLOOD_IO    B                                                                                                                      \r\n";
            SQL += "       , KOSMOS_OCS.EXAM_BLOODCROSSM C                                                                                                                      \r\n";
            SQL += "       , KOSMOS_PMPA.BAS_PATIENT     D                                                                                                                      \r\n";
            SQL += "   WHERE 1=1                                                                                                                                                \r\n";
            SQL += "     AND BDATE BETWEEN TO_DATE('" + strFDATE + "','YYYY-MM-DD')                                                                                             \r\n";
            SQL += " 			       AND TO_DATE('" + strTDATE + "','YYYY-MM-DD')                                                                                             \r\n";
            SQL += "     AND A.GBJOB  		= " + ComFunc.covSqlstr(strGBJOB, false);

            if (strIO.Equals("I"))
            {
                SQL += "     AND B.ROOM <> 0    \r\n";
            }
            else if (strIO.Equals("O"))
            {
                SQL += "     AND B.ROOM = 0    \r\n";
            }

            if (string.IsNullOrEmpty(strPANO) == false)
            {
                SQL += "     AND A.PANO = " + ComFunc.covSqlstr(strPANO, false);
            }

            if (strCOMPONENT.Equals("*") == false)
            {
                SQL += "     AND B.COMPONENT = " + ComFunc.covSqlstr(strCOMPONENT, false);
            }



            SQL += "     AND A.BLOODNO 		= B.BLOODNO                                                                                                                         \r\n";
            SQL += "     AND A.COMPONENT 	= B.COMPONENT                                                                                                                       \r\n";
            SQL += "     AND C.PANO 		= D.PANO(+)                                                                                                                         \r\n";
            SQL += "     AND A.BLOODNO 		= C.BLOODNO(+)                                                                                                                      \r\n";
            SQL += "     AND A.PANO    		= C.PANO(+)                                                                                                                         \r\n";
            SQL += "     AND A.COMPONENT  	= C.COMPONENT(+)                                                                                                                    \r\n";

            if (strGBJOB.Trim().Equals("4") == true || strGBJOB.Trim().Equals("5") == true || strGBJOB.Trim().Equals("7") == true)
            {
                //SQL += "     AND C.GBSTATUS(+) = '2'                                                                                                                          \r\n";
                //2018-08-18 안정수 조건수정함..
                SQL += "     AND C.GBSTATUS(+) 	in ('2', '3')                                                                                                                   \r\n";
            }
            else if (strGBJOB.Trim().Equals("8") == true)
            {
                SQL += "     AND C.GBSTATUS(+) 	= '3'                                                                                                                           \r\n";
            }
            
            SQL += "   ORDER BY A.BDATE, A.PANO,A.BLOODNO, C.GUMSADATE                                                                                                          \r\n";                                   
                        
            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

            return ds;

        }

        public DataSet sel_EXAM_BLOOD_IO_STOCK(PsmhDb pDbCon, string strDATE, string ABO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                                                                                                                \r\n";
            SQL += "         KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_GBSTATUS', GBSTATUS)				AS GBSTATUS                                                                     \r\n";
            SQL += "  	  ,	TO_CHAR(A.STOREDATE,'YYYY-MM-DD')                                           AS SDATE                                                                                                                         \r\n";
            SQL += "  	  , SUBSTR(BLOODNO,1,2) ||'-'|| SUBSTR(BLOODNO,3,2) || '-' || SUBSTR(BLOODNO,5) AS BLOODNO                                                                          \r\n";
            SQL += "  	  , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BLOD_COMPONENT', COMPONENT) 				AS COMPONENT                                                                        \r\n";
            SQL += "  	  , (SELECT NAME FROM KOSMOS_OCS.EXAM_BLOOD_COMM WHERE TRIM(CODE) = STORE)      AS STORE                                                                            \r\n";
            SQL += "  	  , TO_CHAR(A.EXPIRE,'YYYY-MM-DD')    											AS EXDATE                                                                           \r\n";
            SQL += "  	  , KOSMOS_OCS.FC_BAS_USER_NAME(STOREPERSON)									AS STOREPERSON                                                                      \r\n";
            SQL += "  	  , SUBSTR(ABO,1,LENGTH(ABO)-1)													AS ABO                                                                              \r\n";
            SQL += " 	  , SUBSTR(ABO,LENGTH(ABO),1)													AS RH                                                                               \r\n";
            SQL += " 	  , CAPACITY																	AS CAPACITY                                                                         \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_BLOOD_IO A                                                                                                                                                \r\n";
            SQL += "   WHERE 1=1                                                                                                                                                            \r\n";

            if (ABO.Equals("A"))
            {
                SQL += "     AND  ABO IN ('A+','A-')        \r\n";
            }
            else if (ABO.Equals("B"))
            {
                SQL += "     AND  ABO IN ('B+','B-')        \r\n";
            }

            else if (ABO.Equals("AB"))
            {
                SQL += "     AND  ABO IN ('AB+','AB-')        \r\n";
            }
            else if (ABO.Equals("O"))
            {
                SQL += "     AND  ABO IN ('O+','O-')        \r\n";
            }

            SQL += "     AND  STOREDATE < TO_DATE('"+ strDATE+"','YYYY-MM-DD')                                                                                                                \r\n";
            SQL += "     AND  ( OUTDATE     IS NULL OR   OUTDATE    >= TO_DATE('" + strDATE + "','YYYY-MM-DD') OR ( OUTRDATE <= TO_DATE('" + strDATE + "','YYYY-MM-DD') AND OUTDATE  <= OUTRDATE ))   \r\n";
            SQL += "     AND  ( STORERDATE  IS NULL OR STORERDATE   >= TO_DATE('" + strDATE + "','YYYY-MM-DD'))                                                                                  \r\n";
            SQL += "     AND  ( DESTROYDATE IS NULL OR DESTROYDATE  >= TO_DATE('" + strDATE + "','YYYY-MM-DD'))                                                                                  \r\n";
            SQL += "     AND  ( DAEYUDATE   IS NULL OR DAEYUDATE    >= TO_DATE('" + strDATE + "','YYYY-MM-DD'))                                                                                  \r\n";
            SQL += "     ORDER BY COMPONENT, STOREDATE, EXPIRE, ABO, BLOODNO                                                                                                                \r\n";
                                                                                                                                                                                            
            try                                                                                                                                                                             
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

            return ds;
        }

        public string save_EXAM_BLOOD_COMM(PsmhDb pDbCon, string strROWID, string strCODE, string strNAME, ref int intRowAffected)
        {
            string SqlErr = "";

            try
            {
                if (string.IsNullOrEmpty(strROWID) == true)
                {
                    SQL = "";
                    SQL += "  INSERT INTO KOSMOS_OCS.EXAM_BLOOD_COMM (CODE, NAME) VALUES (       \r\n";
                    SQL += "  " + ComFunc.covSqlstr(strCODE, false);
                    SQL += "  " + ComFunc.covSqlstr(strNAME, true);
                    SQL += "  )                                                       \r\n";
                }
                else
                {
                    SQL = "";
                    SQL += "  UPDATE KOSMOS_OCS.EXAM_BLOOD_COMM                   \r\n";
                    SQL += "    SET  NAME   = " + ComFunc.covSqlstr(strNAME, false);
                    SQL += "  WHERE 1=1                                           \r\n";
                    SQL += "    AND ROWID   = " + ComFunc.covSqlstr(strROWID, false);

                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }


            return SqlErr;
        }

        public DataSet sel_EXAM_BLOOD_COMM(PsmhDb pDbCon)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT  CODE            \r\n";
            SQL += "        , NAME            \r\n";
            SQL += "        , ''    AS CHANGE \r\n";
            SQL += "        , ROWID           \r\n";            
            SQL += "   FROM KOSMOS_OCS.EXAM_BLOOD_COMM  \r\n";
            SQL += "  WHERE 1=1                         \r\n";
            SQL += "  ORDER BY CODE                     \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

            return ds;
        }

        public DataSet sel_EXAM_BLOOD_IO_CHK(PsmhDb pDbCon, string strFDATE, string strTDATE, string strGBJOB)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT  '' AS CHK               \r\n";
            SQL += "        , BLOODNO                 \r\n";
            SQL += "        , COMPONENT               \r\n";
            SQL += "        , CAPACITY                \r\n";
            SQL += "        , ABO                     \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_BLOOD_IO  \r\n";
            SQL += "  WHERE 1=1                       \r\n";

            if (strGBJOB == "1" )
            {
                SQL += "  AND  STOREDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                SQL += "                     AND " + ComFunc.covSqlDate(strTDATE, false);
            }
            else if (strGBJOB == "4")
            {
                SQL += "  AND  OUTDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                SQL += "                   AND " + ComFunc.covSqlDate(strTDATE, false);
            }
            else if (strGBJOB == "2" || strGBJOB == "5") // 반납
            {
                SQL += "  AND  STORERDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                SQL += "                      AND " + ComFunc.covSqlDate(strTDATE, false);
            }
            else if (strGBJOB == "7" ) 
            {
                SQL += "  AND  DESTROYDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                SQL += "                       AND " + ComFunc.covSqlDate(strTDATE, false);
            }


            SQL += "  ORDER BY BLOODNO                \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

            return ds;
        }

        public DataSet sel_EXAM_BLOODTRANS_CHK(PsmhDb pDbCon, string strFDATE, string strTDATE, string strGBJOB)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT  '' AS CHK                 \r\n";
            SQL += "        , BLOODNO                   \r\n";
            SQL += "        , COMPONENT                 \r\n";
            SQL += "        , CAPACITY                  \r\n";
            SQL += "        , ABO                       \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_BLOODTRANS  \r\n";
            SQL += "  WHERE 1=1                         \r\n";
            SQL += "    AND  BDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
            SQL += "                   AND " + ComFunc.covSqlDate(strTDATE, false);


            if (strGBJOB == "1" || strGBJOB == "4" || strGBJOB == "7")
            {
                SQL += "    AND  GBJOB = " + ComFunc.covSqlstr(strGBJOB, false);
            }
            else if (strGBJOB == "2" || strGBJOB == "5") // 반납
            {
                SQL += "    AND  GBJOB IN ('2','5')     \r\n";
            }

            SQL += "  ORDER BY BLOODNO                \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

            return ds;
        }
    }
}
