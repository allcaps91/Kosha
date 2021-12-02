using ComBase; //기본 클래스
using ComDbB; //DB연결
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name : ComSupLibB.SupLbEx
    /// File Name : clsLbExSQL.cs
    /// Title or Description : 진단검사의학과 SQL
    /// Author : 김홍록
    /// Create Date : 2017-05-15
    /// Update History : 
    /// </summary>

    public class clsComSupLbExSQL : Com.clsMethod
    {
        string SQL = string.Empty;

        /// <summary>SPECMST 필드에 대한 값</summary> 
        public string[] gArrSPECMST;

        /// <summary>RESULTC 필드에 대한 값</summary>
        public string[] gArrEXAM_RESULTC;

        /// <summary>2017.03.13.김홍록:바코드출력</summary>
        public enum enmSelExamVerifyPrint { PANO, SNAME, AGE_SEX, WARD, DEPT_NAME, DR_NAME, DISEASE, DISDATE, ITEMS1, ITEMS2, VERIFY1, VERIFY2, VERIFY3, VERIFY4, VERIFY5, VERIFY6, COMMENTS, RECOMMENDATION, JDATE, RDR_NAME, RDR_BUNHO, JYYYY };

        /// <summary>2017.05.25.김홍록:PB 출력 대상</summary>
        public enum enmSelExamOrderPb { PANO, SNAME, JUMIN, DEPTCODE, DRCODE, WARDCODE, ROOMCODE, REQUEST1, REQUEST2, GBIO, DRNAME, BDATE };

        /// <summary>2017.05.29.김홍록:혈액신청서 출력 대상</summary>
        public enum enmSelExamResultcBlood { PANO, BI, SNAME, AGE_SEX, ROOM, DEPTCODE, DEPT_NAME, DRCODE, DR_NAME, BDATE, DRCOMMENT, ACT_DATE, SUBCODE, CNT, BLOOD_TYPE, DIAGNOSIS, BLOOD_HIS, BLOOD_NAME }

        /// <summary>2017.05.29.김홍록:혈액팔찌출력</summary>
        public enum enmSelExamResultcBloodBand { LINE_01, LINE_02_1, LINE_02_2, LINE_03, LINE_04 };


        /// <summary>2017.05.30.김홍록:panic체크</summary>
        public enum enmSelExamMasterPanic { CVMIN, CVMAX, CVVALUE, PANICFROM, PANICTO, MASTERCODE };

        /// <summary>2017.06.02.김홍록:의사스케쥴</summary>
        public enum enmSelOcsDoctorSch { SABUN, SETSABUN, HTEL };

        /// <summary>2017.06.02.김홍록:CVR 검사결과 조회</summary>
        public enum enmSelExamResultcCvAfb { PANO, MSG, SPECNO, SABUN, HTEL, IPDOPD };

        /// <summary>2017.06.02.김홍록:검사결과출력조회</summary>
        public enum enmSel_EXAM_RESULTC_Print { WORKSTS_NAME, PANO, SNAME, SEX_AGE, WARD_NAME, DEPT_NAME, DR_NAME, EXAMNAME, SPEC_NAME, RESULT, FOOTTYPE, REFER, REFER_VALUE, UNIT, REFER_RAG, RESULTSABUN, RESULT_NAME, BLOODDATE, RECEIVEDATE, RESULTDATE, EXAM_USER, PRINT, STATUS, MST_STATUS, MASTERCODE, SUBCODE, DELTA, PANIC };
        public enum enmSel_EXAM_RESULTC_Print_PB { ALL, PB, NONE };

        /// <summary>2017.06.06.김홍록:0결과, 1FootNote</summary>
        public enum enmFootNoteType { RESULT, FOOTNOTE };

        /// <summary>2017.05.25.김홍록:EXAM_SPECMST 필드</summary>
        public enum enmSPECMST { SPECNO, PANO, BI, SNAME, IPDOPD, AGE, AGEMM, SEX, DEPTCODE, WARD, ROOM, DRCODE, DRCOMMENT, STRT, WORKSTS, SPECCODE, TUBE, BDATE, BLOODDATE, RECEIVEDATE, RESULTDATE, STATUS, CANCEL, PRINT, ANATNO, GBORDERCODE, EMR, ORDERDATE, SENDDATE, SENDFLAG, GB_GWAEXAM, GBPRINT, HICNO, VIEWID, VIEWDATE, ORDERNO, WDATE, WGBN, WSEQ, WPART, HCV, WGB, WAMT, WHCODE, BCODE, WEXNAME, WSPECNAME, WJDATE, HCVREMARK, IMGWRTNO, GB_GWAEXAM2, GBMIC, GBMICORDER, WCNT, JEPSUSABUN, JEPSUNAME, JEPSUSABUN2, JEPSUNAME2 }

        /// <summary>2017.05.25.김홍록:EXAM_RESULTC 필드</summary>
        public enum enmSPEC_RESULTC { SPECNO, RESULTWS, EQUCODE, SEQNO, PANO, MASTERCODE, SUBCODE, STATUS, RESULT, RESULTDATE, RESULTSABUN, REFER, PANIC, DELTA, UNIT, PRINT, ANATNO, GBORDER, EQUCODE_INTER, HCODE, WGB, WHCODE, WAMT, WEXAMNAME, WSPECNAME, WJDATE, BCODE, IMGWRTNO, CV };

        /// <summary>2017.06.08.김홍록 : Sel_EXAM_EXBST_Cnt</summary>
        public enum enmSel_EXAM_EXBST_Cnt { WARDCODE, PANO, SNAME, DEPTCODE, BDATE, QTY };

        /// <summary>2017.06.08.김홍록 : Sel_EXAM_EXBST_Cnt</summary>  
        public string[] sSpd_Sel_EXAM_EXBST_Cnt = { "병동", "등록번호", "성명", "진료과", "처방일자", "수량" }; 

        /// <summary>2017.06.08.김홍록 : Sel_EXAM_EXBST_Cnt</summary>  
        public int[] nSpd_Sel_EXAM_EXBST_Cnt = { nCol_WARD, nCol_PANO, nCol_SNAME, nCol_DPNM, nCol_DATE, 50 };

        /// <summary>2017.06.09.김홍록 : 화면 조회 옵션</summary>
        public enum enmSel_EXAM_RESULTC_MicResultOptient { ALL, GROWTH, NO_GROWTH };

        /// <summary>2017.06.09.김홍록 :Sel_EXAM_RESULTC_MicResult</summary>
        public enum enmSel_EXAM_RESULTC_MicResult { PANO, SNAME, DEPTCODE, DR_NAME, BI, IPDOPD, BDATE, RESULTDATE, RESULT, SPECNO, TODAY_IN };

        /// <summary>2017.06.09.김홍록 :Sel_EXAM_RESULTC_MicResult</summary>
        public string[] sSpd_Sel_EXAM_RESULTC_MicResult = { "등록번호", "성명", "과", "의사", "자격", "I/O", "처방일자", "결과일시", "결과", "검체번호", "당일" };

        /// <summary>2017.06.09.김홍록 :Sel_EXAM_RESULTC_MicResult</summary>
        public int[] nSpd_Sel_EXAM_RESULTC_MicResult = { nCol_PANO, nCol_SNAME, nCol_DPCD, nCol_SNAME, nCol_AGE, nCol_AGE, nCol_DATE, nCol_TIME, 100, nCol_PANO, nCol_AGE };

        /// <summary>2017.06.13.김홍록 : enmSel_EXAM_SPECMST_Viewer</summary>
        public enum enmSel_EXAM_SPECMST_Viewer { SPECNO, RECEIVEDATE, SNAME, IPDOPD, DEPTCODE, ROOM, DRCODE, GBER, WORKSTS, EXAM_NAME, SPEC_NAME, STATUS, ORDERDATE, BDATE, RESULTDATE, PANO, PRINT, GB_GWAEXAM };
        /// <summary>2017.06.13.김홍록 : sSel_EXAM_SPECMST_Viewer</summary>
        public string[] sSel_EXAM_SPECMST_Viewer = { "검체번호", "채취일시", "성명", "구분", "과", "병실", "의사", "응급", "W/S", "검사명", "검체", "상태", "지시일시", "처방일자", "결과일시", "환자번호", "출력", "POCT" };
        /// <summary>2017.06.13.김홍록 : nSel_EXAM_SPECMST_Viewer</summary>
        ///                                               "검체번호",           "채취일시",            "성명",             "구분",                 "과",           "병실",            "의사",             "응급",           "W/S",         "검사명",          "검체",             "상태",           "지시일시",       "처방일자",           "결과일시",       "환자번호",          "출력",           "POCT" 
        public int[] nSel_EXAM_SPECMST_Viewer = { nCol_SPNO, nCol_TIME, nCol_SNAME, nCol_IOPD, nCol_DPCD, nCol_WARD, nCol_SNAME, nCol_IOPD, nCol_SEX, nCol_NAME, nCol_SEX, nCol_STAT, nCol_TIME, nCol_DATE, nCol_TIME, nCol_PANO, nCol_AGE, nCol_AGE };
        /// <summary>2017.06.16.김홍록:병동 미시행 검사 내역 조회</summary>
        public enum enmSel_IPD_NEW_MASTER_EXAM { ROOMCODE, PANO, SNAME, SEX, AGE, DEPTCODE, DRNAME, EXAM_CODE, EXAM_NAME, ACT_DATE, BDATE, CADEX_DEL, CDATE, CSABUN, EXAM_TYPE, WARTNO, ORDERNO };
        /// <summary>2017.06.16.김홍록:병동 미시행 검사 내역 조회</summary>
        public string[] sSel_IPD_NEW_MASTER_EXAM = { "병실", "등록번호", "성명", "성별", "나이", "과", "의사명", "검사코드", "검사명칭", "시행일자", "처방일자", "삭제", "삭제일자", "삭제자", "*검사정렬", "*참고정보", "*처방번호" };
        /// <summary>2017.06.16.김홍록:병동 미시행 검사 내역 조회</summary>
        public int[] nSel_IPD_NEW_MASTER_EXAM = { nCol_DPCD, nCol_PANO, nCol_SNAME, nCol_SEX, nCol_AGE, nCol_DPCD, nCol_SNAME, nCol_EXCD, nCol_NAME, nCol_DATE, nCol_DATE, nCol_CHEK, nCol_DATE, nCol_SNAME, 5, 5, 5 };
        /// <summary>조회조건</summary>
        public enum enmSel_IPD_NEW_MASTER_EXAM_Param { ALL, XRAY, ENDO, ETC, EXAM, BARCODE };
        /// <summary>Sel_EXAM_SPECMST_WardWorkList</summary>
        public enum enmSel_EXAM_SPECMST_WardWorkList { ROOM, PANO, SNAME, SEX_AGE, BC, PT, CBC, REMARK };
        /// <summary>EXAM_SPECODE_GUBUN</summary>
        public enum enmEXAM_SPECODE_GUBUN { WC = 11, WS = 12, EQU = 13, SPEC = 14, TUBE = 15, VOLUME = 16, COMMENT = 17, HELP = 18, FOOT = 19, UNIT = 20, CANCEL = 21, RESULT_CHANGE = 22, RESULT_CHANGE_ANAN = 23, SUB = 31, REF = 41, REMARK = 51,EXAM_INFO = 61, INTERFACE = 71, MICRO = 99, SLIP_SPECIMAN = 100,OCS_SUBCODE=101, BAS_SUT=102 };
        /// <summary>2017.06.16.김홍록:병동 미시행 검사 내역 조회</summary>
        public string[] sEXAM_SPECODE_GUBUN = { "코드", "이름", "약어","WS" };
        /// <summary>2017.06.16.김홍록:병동 미시행 검사 내역 조회</summary>
        public int[] nEXAM_SPECODE_GUBUN = { 80, 150, 100,100 };

        /// <summary>2017.06.16.김홍록:병동 미시행 검사 내역 조회</summary>
        public string[] sSel_EXAM_SPECMST_WardWorkList = { "병실", "등록번호", "성명", "성별/나이", "B.C", "PT", "CBC", "비고" };
        /// <summary>2017.06.16.김홍록:병동 미시행 검사 내역 조회</summary>
        public int[] nSel_EXAM_SPECMST_WardWorkList = { 80, nCol_PANO, nCol_SNAME, nCol_SEX + nCol_AGE, 50, 50, 50, nCol_NAME };

        public enum enmSel_EXAM_SPECODE_OCS_OSPECIMAN { CHK, CODE, NAME };
        public string[] sSel_EXAM_SPECODE_OCS_OSPECIMAN = { "C", "코드", "검체명칭" };
        public int[] nSel_EXAM_SPECODE_OCS_OSPECIMAN = { 20, 100, 200 };

        /// <summary>2017.06.16.김홍록:병동 ORDER SLIP별 검체번호</summary>
        public enum enmSel_OCS_OSPECIMAN { CHK, SPECCODE, SPECNAME, ROWID };
        public string[] sSel_OCS_OSPECIMAN = { "C", "코드", "명칭", "ROWID" };
        public int[] nSel_OCS_OSPECIMAN = { 20, 100, 250, 10 };

        public enum enmSel_EXAM_VERIFYUSES { JONG, USECODE, USENAME, ROWID };

        public enum enmSel_EXAM_HISRESULTC      {       CHK,      PANO,      SNAME, SPECNO, SEQNO, RESULTDATE, RESULTTIME, RESULT_NAME, GBN_NAME, SUBCODE, EXAMNAME, RESULT, STATUS, SAYU_NM, ROWID };
        public string[] sSel_EXAM_HISRESULTC =  {    "선택","등록번호",   "환자명", "검체번호", "NO", "전송일자", "전송시간", "작업자", "구분", "검사코드", "검사명", "결과", "상태", "변경사유", "ROWID" };
        public int[] nSel_EXAM_HISRESULTC =     { nCol_SCHK, nCol_PANO, nCol_SNAME, nCol_PANO, nCol_AGE, nCol_DATE, nCol_DATE, nCol_SNAME, nCol_AGE, nCol_EXCD, nCol_NAME, nCol_SPNO, nCol_AGE, nCol_NAME, nCol_CHEK };
       
        public enum enmSel_EXAM_INFECT     { PANO, SNAME, SPECNO, RESULTDATE, USERNAME, EXAMFNAME, RESULT, STATUS };
        public string[] sSel_EXAM_INFECT = { "등록번호"       ,            "성명",         "검체번호",           "결과일자",          "작업자",       "검사명칭",           "결과",          "상태" };
        public int[]    nSel_EXAM_INFECT = { nCol_PANO , nCol_SNAME, nCol_SPNO, nCol_TIME, nCol_SNAME, nCol_NAME, nCol_NAME, nCol_AGE };

        public enum enmSel_EXAM_RESULTC_CV {PANO,SNAME,WARD,SPECNO,SUBCODE,EXAMFNAME,RESULT,UNIT,RESULTDATE,RESULTSABUN,CHK,CHKDATE,ROWID };
        public string[] sSel_EXAM_RESULTC_CV ={ "등록번호"      ,            "성명",          "병동",         "검체번호",           "검사코드",       "검사명칭",           "결과", "단위",           "결과일자",          "입력자",          "확인", "CHKDATE","ROWID" };
        public int[]    nSel_EXAM_RESULTC_CV ={ nCol_PANO, nCol_SNAME, nCol_CHEK, nCol_SPNO, nCol_EXCD, nCol_NAME, nCol_NAME,     50, nCol_TIME, nCol_SNAME, nCol_CHEK,    5, 5   };
        public enum enmSel_EXAM_RESULTC_CV_STATUE {ALL,NONE,CONFIRM };

        public enum enmSel_EXAM_MASTER_SEARCH     { MASTERCODE             , EXAMNAME          , EXAMFNAME           , EXAMYNAME    ,  WSCODE1    , WS_NAME       , BCODENAME        };
        public string[] sSel_EXAM_MASTER_SEARCH = { "검사코드"             , "검사명칭"        , "검사상세명칭"      , "검사분류"   , "WSCODE"    , "워크스테이션", "바코드명"       };
        public int[] nSel_EXAM_MASTER_SEARCH    = { nCol_EXCD +20  , nCol_NAME  , nCol_NAME    ,  120         , 80          , 120            , 120             };

        public enum enmSel_OPD_SLIP { BUN, SUNEXT, SUCODE, SUNAMEK, QTY};
        public string[] sSel_OPD_SLIP = { "BUN", "수가코드", "품명코드", "수량", "수가명칭" };
        public int[] nSel_OPD_SLIP = { 5, nCol_EXCD, nCol_EXCD, nCol_AGE, nCol_NAME};

        public enum enmSel_EXAM_MATCH          {     SUCODE,    SUNAMEK,        BUN,    BUN_NAME, MASTERCODE  };
        public string[] sSel_EXAM_MATCH =      { "수가코드", "수가명칭", "분류CODE",  "분류명칭", "검사코드" };
        public int[] nSel_EXAM_MATCH    =      {  nCol_EXCD,  nCol_NAME,  nCol_EXCD,   nCol_NAME,           5 };

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public string[] sSel_EXAM_SPECODE_WC = { "삭제", "GUBUN", "코드", "WC 명칭", "바코드", "WSGROUP", "WSEQU", "정렬", "삭제일자", "사용여부", "입력자", "입력일시", "수정자", "수정일시", "C" };
        public int[] nSel_EXAM_SPECODE_WC =    { nCol_AGE, nCol_EXCD, nCol_EXCD, nCol_JUSO, nCol_EXCD, 5, 5, nCol_AGE, nCol_DATE, nCol_AGE, nCol_NAME, nCol_NAME, nCol_NAME, nCol_NAME, 0 };

        public string[] sSel_EXAM_SPECODE_WS = {   "삭제", "GUBUN",    "코드", "WS 명칭",  "바코드",   "GROUP", "검사장비", "정렬", "삭제일자", "사용여부", "입력자", "입력일시", "수정자", "수정일시", "C" };
        public int[] nSel_EXAM_SPECODE_WS    = { nCol_AGE, nCol_EXCD, nCol_EXCD, nCol_JUSO, nCol_EXCD, nCol_EXCD, nCol_JUSO, nCol_AGE, nCol_DATE, nCol_AGE, nCol_NAME, nCol_NAME, nCol_NAME, nCol_NAME, 0 };

        public string[] sSel_EXAM_SPECODE_EQU = {   "삭제", "GUBUN",    "코드", "장비명칭", "Interface Port", "WSGROUP", "WSEQU",   "정렬", "삭제일자", "사용여부", "입력자", "입력일시", "수정자", "수정일시", "C" };
        public int[] nSel_EXAM_SPECODE_EQU    = { nCol_AGE, nCol_EXCD, nCol_EXCD, nCol_ORDERNAME, nCol_ORDERNAME,         5,       5, nCol_AGE,  nCol_DATE, nCol_AGE, nCol_NAME, nCol_NAME, nCol_NAME, nCol_NAME, 0 };

        public string[] sSel_EXAM_SPECODE_SPEC = { "삭제", "GUBUN", "코드", "검체명칭", "약어", "색상", "WSEQU", "정렬", "삭제일자", "사용여부", "입력자", "입력일시", "수정자", "수정일시", "C" };
        public int[] nSel_EXAM_SPECODE_SPEC = { nCol_AGE, 5, nCol_EXCD, nCol_JUSO, nCol_NAME, nCol_EXCD, 5, nCol_AGE, nCol_DATE, nCol_AGE, nCol_NAME, nCol_NAME, nCol_NAME, nCol_NAME, 0 };

        public string[] sSel_EXAM_SPECODE_TUBE  = {   "삭제", "GUBUN",    "코드", "Collection Tube",     "약어", "WSGROUP", "WSEQU",   "정렬", "삭제일자", "사용여부", "입력자", "입력일시", "수정자", "수정일시", "C" };
        public int[] nSel_EXAM_SPECODE_TUBE     = { nCol_AGE, nCol_EXCD, nCol_EXCD,         nCol_JUSO,  nCol_NAME,         5,       5, nCol_AGE,  nCol_DATE, nCol_AGE, nCol_NAME, nCol_NAME, nCol_NAME, nCol_NAME, 0 };

        public string[] sSel_EXAM_SPECODE_UNIT  = { "삭제", "GUBUN", "코드", "단위 명칭", "약어", "WSGROUP", "WSEQU", "정렬", "삭제일자", "사용여부", "입력자", "입력일시", "수정자", "수정일시", "C" };
        public int[] nSel_EXAM_SPECODE_UNIT     = { nCol_AGE, nCol_EXCD, nCol_EXCD, nCol_JUSO, nCol_NAME, 5, 5, nCol_AGE, nCol_DATE, nCol_AGE, nCol_NAME, nCol_NAME, nCol_NAME, nCol_NAME, 0 };

        public string[] sSel_EXAM_SPECODE_VOLUME = { "삭제", "GUBUN", "코드", "채혈량 명칭", "약어", "WSGROUP", "WSEQU", "정렬", "삭제일자", "사용여부", "입력자", "입력일시", "수정자", "수정일시", "C" };
        public int[] nSel_EXAM_SPECODE_VOLUME    = { nCol_AGE, nCol_EXCD, nCol_EXCD, nCol_JUSO, nCol_NAME, 5, 5, nCol_AGE, nCol_DATE, nCol_AGE, nCol_NAME, nCol_NAME, nCol_NAME, nCol_NAME, 0 };

        public string[] sSel_EXAM_SPECODE_COMMENT = { "삭제", "GUBUN", "코드", "Comment", "약어", "WSGROUP", "WSEQU", "정렬", "삭제일자", "사용여부", "입력자", "입력일시", "수정자", "수정일시", "C" };
        public int[] nSel_EXAM_SPECODE_COMMENT    = { nCol_AGE, nCol_EXCD, nCol_EXCD, nCol_JUSO, nCol_NAME, 5, 5, nCol_AGE, nCol_DATE, nCol_AGE, nCol_NAME, nCol_NAME, nCol_NAME, nCol_NAME, 0 };

        public string[] sSel_EXAM_SPECODE_HELP   = { "삭제", "GUBUN", "코드", "HELP Comment", "약어", "WSGROUP", "WSEQU", "정렬", "삭제일자", "사용여부", "입력자", "입력일시", "수정자", "수정일시", "C" };
        public int[] nSel_EXAM_SPECODE_HELP      = { nCol_AGE, nCol_EXCD, nCol_EXCD, nCol_JUSO, nCol_NAME, 5, 5, nCol_AGE, nCol_DATE, nCol_AGE, nCol_NAME, nCol_NAME, nCol_NAME, nCol_NAME, 0 };

        public string[] sSel_EXAM_SPECODE_FOOT   = { "삭제", "GUBUN", "코드", "FOOT NOTE"   , "약어", "WSGROUP", "WSEQU", "정렬", "삭제일자", "사용여부", "입력자", "입력일시", "수정자", "수정일시", "C" };
        public int[] nSel_EXAM_SPECODE_FOOT      = { nCol_AGE, nCol_EXCD, nCol_EXCD, nCol_JUSO, nCol_NAME, 5, 5, nCol_AGE, nCol_DATE, nCol_AGE, nCol_NAME, nCol_NAME, nCol_NAME, nCol_NAME, 0 };

        public string[] sSel_EXAM_SPECODE_CANCEL = { "삭제", "GUBUN", "코드", "취소사유", "약어", "WSGROUP", "WSEQU", "정렬", "삭제일자", "사용여부", "입력자", "입력일시", "수정자", "수정일시", "C" };
        public int[] nSel_EXAM_SPECODE_CANCEL    = { nCol_AGE, nCol_EXCD, nCol_EXCD, nCol_JUSO, nCol_NAME, 5, 5, nCol_AGE, nCol_DATE, nCol_AGE, nCol_NAME, nCol_NAME, nCol_NAME, nCol_NAME, 0 };

        public string[] sSel_EXAM_SPECODE_RESULT_CHANGE_ANAN = { "삭제", "GUBUN", "코드", "병리결과 변경사유", "약어", "정렬", "WSGROUP", "WSEQU", "삭제일자", "사용여부", "입력자", "입력일시", "수정자", "수정일시", "C" };
        public int[] nSel_EXAM_SPECODE_RESULT_CHANGE_ANAN = { nCol_AGE, nCol_EXCD, nCol_EXCD, nCol_JUSO, nCol_NAME, 5, 5, nCol_AGE, nCol_DATE, nCol_AGE, nCol_NAME, nCol_NAME, nCol_NAME, nCol_NAME, 0 };

        public string[] sSel_EXAM_SPECODE_RESULT_CHANGE = { "삭제", "GUBUN", "코드", "결과 변경 사유", "약어", "WSGROUP", "WSEQU", "정렬", "삭제일자", "사용여부", "입력자", "입력일시", "수정자", "수정일시", "C" };
        public int[] nSel_EXAM_SPECODE_RESULT_CHANGE = { nCol_AGE, nCol_EXCD, nCol_EXCD, nCol_JUSO, nCol_NAME, 5, 5, nCol_AGE, nCol_DATE, nCol_AGE, nCol_NAME, nCol_NAME, nCol_NAME, nCol_NAME, 0 };

        public string[] sSel_EXAM_SPECODE_INTERFACE = { "삭제", "GUBUN", "장비코드", "인터페이스코드", "검사코드", "WSGROUP", "WSEQU", "순서", "DELDATE", "사용여부", "입력자", "입력일시", "수정자", "수정일시", "C" };
        public int[] nSel_EXAM_SPECODE_INTERFACE    = { nCol_AGE, nCol_EXCD, nCol_EXCD, nCol_EXCD, nCol_EXCD, 5, 5, nCol_AGE, 5, nCol_AGE, nCol_NAME, nCol_NAME, nCol_NAME, nCol_NAME, 0 };


        public string[] sSel_EXAM_SPECODE_ALL = {       "",    "구분",    "코드",    "명칭",    "약어",     "그룹", "검사장비",   "정렬", "삭제일자", "사용여부",  "입력자","입력일시",  "수정자", "수정일시","C" };
        public int[] nSel_EXAM_SPECODE_ALL =    { nCol_AGE, nCol_EXCD, nCol_EXCD, nCol_NAME, nCol_NAME,  nCol_EXCD,  nCol_NAME, nCol_AGE, nCol_DATE,    nCol_AGE, nCol_NAME, nCol_NAME, nCol_NAME,  nCol_NAME, 5 };
        public enum enmSel_EXAM_SPECODE_ALL      {     CHK,     GUBUN,      CODE,      NAME,     YNAME,    WSGROUP,      WSEQU,     SORT,   DELDATE ,    USER_YN,      INPS,    INPT_DT,     UPPS,       UPDT, CHANGE };
        

        public enum enmSel_EXAM_RACK_CODE     {          CHK,      CODE,      NAME,   ROWID  };
        public string[] sSel_EXAM_RACK_CODE = {       "삭제",    "분류코드",  "분류명",  "ROWID" };
        public int[] nSel_EXAM_RACK_CODE    = { nCol_SCHK+5, nCol_EXCD, nCol_NAME,       5  };

        public enum enmSel_EXAM_RACK_NO       {           CHK,     GUBUN,      NAME,        CODE,          SSEQNO,          ESEQNO,  ROWID };
        public string[] sSel_EXAM_RACK_NO  =  {           "삭제",    "분류",  "분류명", "RACK 코드", "RACK 시작번호", "RACK 종료번호", "ROWID"};
        public int[] nSel_EXAM_RACK_NO     =  { nCol_SCHK + 5, nCol_EXCD, nCol_NAME,   nCol_EXCD,       nCol_NAME,       nCol_NAME,     5  };

        public enum enmSel_EXAM_HISMST        { JOBDATE, JOBGBN, JOBSABUN, MASTERCODE, EXAMNAME, EXAMFNAME, EXAMYNAME, WSCODE1, WSCODE1POS, WSCODE2, WSCODE2POS, WSCODE3, WSCODE3POS, WSCODE4, WSCODE4POS, WSCODE5, WSCODE5POS, TURNTIME1, TURNTIME2, EXAMWEEK, TUBECODE, SPECCODE, VOLUMECODE, EQUCODE1, EQUCODE2, EQUCODE3, EQUCODE4, UNITCODE, DATATYPE, DATALENGTH, RESULTIN, PANICFROM, PANICTO, DELTAM, DELTAP, DDDPRDRP, BCODENAME, BCODEPRINT, KEYPAD, SERIES, PENDING, ENDDATE, MODIFYDATE, MODIFYID, SUB, SUCODE, SUBUN, MOTHER };

        public enum enmSel_EXAM_HISMSTSUB     {  JOBDATE, JOBGBN, JOBSABUN, MASTERCODE, GUBUN, SORT, NORMAL, SEX, AGEFROM, AGETO, REFVALFROM, REFVALTO, EXPIREDATE};

        public enum enmSel_EXAM_MASTER { MASTERCODE, EXAMNAME, EXAMFNAME, EXAMYNAME, WSCODE1, WS1_NM, WSCODE1POS, WSCODE2, WS2_NM, WSCODE2POS, WSCODE3, WS3_NM, WSCODE3POS, WSCODE4, WS4_NM, WSCODE4POS, WSCODE5, WS5_NM, WSCODE5POS, TURNTIME1, TURNTIME2, EXAMWEEK, TUBECODE, TUBE_NM, SPECCODE, SPEC_NM, VOLUMECODE, VOLUME_NM, EQUCODE1, EQU1_NM, EQUCODE2, EQU2_NM, EQUCODE3, EQU3_NM, EQUCODE4, EQU4_NM, UNITCODE, UNIT_NM, DATATYPE, DATALENGTH, RESULTIN, PANICFROM, PANICTO, DELTAM, DELTAP, DDDPRDRP, BCODENAME, BCODEPRINT, KEYPAD, SERIES, PENDING, ENDDATE, MODIFYDATE, MODIFYID, SUB, SUCODE, SUBUN, MOTHER, HELPCODE, SLIPNO, TONGBUN, TONGSEQNO, GBTAX, GBTAT, PIECE, GBBASE, BCODE, WAMT, WGB, WHCODE, VITEKCODE, CVMIN, CVMAX, CVVALUE, TONGDISP, GBTLA, GBTLASORT, ORDERINFO, ACODE, PART,ACODE_NM, ROWID };

        public enum enmSel_EXAM_MASTER_SUB_BASIC { MASTERCODE, GUBUN, SORT, NORMAL, SEX, AGEFROM, AGETO, REFVALFROM, REFVALTO, NAME, ROWID, EXPIREDATE };

       // public enum enmEXAM_RACK_TYPE {  C = 1, S = 2,  H = 3, B = 4, ABO = 5, PT = 6, M = 7, H_C = 8, H_H = 9, H_SLID = 10, ANA = 11, ANCA = 12, H_SH = 13, H_B = 14 };

        public enum enmSel_EXAM_RACK_DIS     {     SPECNO,       PANO,          SNAME,       SEX,           AGE,        DEPTCODE,          ROOM, BLOODDATE,      EXAMNAME, RACKNO,     PRENO,     ENTDATE,       DEL, MOVE_SPEC,       CHK, ROWID };                                                   
        public string[] sSel_EXAM_RACK_DIS = { "검체번호", "등록번호",         "성명",    "성별",        "나이",            "과",        "호실",    "요일",    "검사항목",  "RACK",      "NO",  "입력시간",    "삭제",    "이동",    "체크","ROWID" };
        public int[] nSel_EXAM_RACK_DIS    = {  nCol_PANO,  nCol_PANO, nCol_SNAME -10, nCol_SCHK,   nCol_AGE-10,    nCol_AGE -10,  nCol_AGE-10 , nCol_SCHK,     nCol_NAME-30, nCol_WARD , nCol_WARD,   nCol_DATE, nCol_SCHK, nCol_SCHK, nCol_SCHK, 5 };

        public enum enmSel_EXAM_RACK_CNT {RACKNO,CNT };

        public enum enmSel_EXAM_RACK_SEARCH     {    RACKNO,          PRENO,     SPECNO,       PANO,      SNAME,   EXAMNAME };
        public string[] sSel_EXAM_RACK_SEARCH = {    "RACK",           "NO", "검체번호", "환자번호",     "성명", "검사항목" };
        public int[] nSel_EXAM_RACK_SEARCH    = { nCol_WARD-10, nCol_WARD-20, nCol_PANO,   nCol_PANO, nCol_SNAME, nCol_TEL };

        //public enum enmSel_EXAM_MASTER_GBBASE      {               GBBASE, MASTERCODE,   EXAMNAME,  ROWID  };
        //public string[] sSel_EXAM_MASTER_GBBASE  = { "참고치사용", "검사코드", "검사명칭", "ROWID" };
        //public int[] nSel_EXAM_MASTER_GBBASE = { nCol_CHEK,  nCol_EXCD,  nCol_NAME,       5 };


        //public enum enmSel_EXAM_CHAMGO_GROUP     { EXAMCODE     , BDATE };
        //public string[] sSel_EXAM_CHAMGO_GROUP = { "검사코드"   , "작업일자" };
        //public int[] nSel_EXAM_CHAMGO_GROUP    = { nCol_EXCD +10   ,nCol_DATE + 10 };

        //public enum enmSel_EXAM_CHAMGO          {GBN,BDATE,EXAMCODE,EXAMNAME,SEQNO,RESULT,ROWID };

        //public enum enmSel_EXAM_RESULTC_CHAMGO     { RESULTDATE,       PANO,      SNAME,       SEX,      AGE,    RESULT,     REFER,   EXAMNAME,    SUBCODE  };
        //public string[] sSel_EXAM_RESULTC_CHAMGO = { "결과일자", "등록번호",     "성명",    "성별",   "나이",    "결과",       "R",   "검사명",  "검사코드" };
        //public int[] nSel_EXAM_RESULTC_CHAMGO    = {  nCol_DATE,  nCol_PANO, nCol_SNAME,  nCol_SEX, nCol_AGE, nCol_NAME, nCol_AGE ,  nCol_NAME,   nCol_SPNO };

        /// <summary>gArrSPECMST 변수 초기화</summary>
        public void clearArrSPECMST()
        {
            Type enmSpecMst = typeof(enmSPECMST);
            gArrSPECMST = new string[Enum.GetNames(enmSpecMst).Length];
        }

        /// <summary>2017.03.13.김홍록:바코드출력</summary>
        public enum enmSel_EXAM_SPECMST_BARCODE { SPECNO, IPDOPD, ABO, PANO, SNAME, DEPTCODE, WARD, ROOM, DRCODE, BI, AGE, SEX, WORKSTS, SPECCODE, SPECNM, TUBE, TUBENM, VLOLUME, STRT, BDATE, EDTA, WON, INFECT, TUBEMSG, BCODENAME, BCODENAME1, BCODENAME2, BCODENAME3, BCODEPRINT, HEPARIN, SODIUM, ERPAT, PRINT_ADD };

        /// <summary>검체정보</summary>
        /// <param name="strSpecNo">검체번호</param>
        /// <returns>검체정보</returns>
        public DataTable sel_EXAM_SPECMST_BARCODE(PsmhDb pDbCon, string strSpecNo)
        {
            DataTable dt = null;

            SQL = "";
            SQL = SQL + " SELECT      SPECNO                                                                                                                     \r\n";
            SQL = SQL + " 			, IPDOPD                                                                                                                     \r\n";
            SQL = SQL + " 			, ABO                                                                                                                        \r\n";
            SQL = SQL + " 			, PANO                                                                                                                       \r\n";
            SQL = SQL + " 			, SNAME                                                                                                                      \r\n";
            SQL = SQL + " 			, DEPTCODE                                                                                                                   \r\n";
            SQL = SQL + " 			, WARD                                                                                                                       \r\n";
            SQL = SQL + " 			, ROOM || DECODE(BED,'','','/'|| BED) AS ROOM                               \r\n";
            SQL = SQL + " 			, DRCODE                                                                                                                     \r\n";
            SQL = SQL + " 			, BI                                                                                                                         \r\n";
            SQL = SQL + " 			, AGE                                                                                                                        \r\n";
            SQL = SQL + " 			, SEX                                                                                                                        \r\n";
            SQL = SQL + " 			, WORKSTS                                                                                                                    \r\n";
            SQL = SQL + " 			, SPECCODE                                                                                                                   \r\n";
            SQL = SQL + " 			, SPECNM                                                                                                                     \r\n";
            SQL = SQL + " 			, TUBE                                                                                                                       \r\n";
            SQL = SQL + " 			, TUBENM                                                                                                                     \r\n";
            SQL = SQL + " 			, VLOLUME                                                                                                                    \r\n";
            SQL = SQL + " 			, STRT                                                                                                                       \r\n";
            SQL = SQL + " 			, BDATE                                                                                                                      \r\n";
            SQL = SQL + " 			, (CASE WHEN HEPARIN = '*' OR SODIUM = '*' THEN '*' ELSE '' END) AS  EDTA                                                                                                                       \r\n";
            SQL = SQL + " 			, WON                                                                                                                        \r\n";
            SQL = SQL + " 			, INFECT                                                                                                                     \r\n";
            SQL = SQL + " 			, SPECNM || ' / ' || TUBENM || ' / ' || CASE WHEN TUBE = '011' THEN                                                          \r\n";
            SQL = SQL + "                                                                          CASE WHEN VLOLUME > 5 THEN '5' || 'ml'                        \r\n";
            SQL = SQL + "                                                                               WHEN VLOLUME < 3 THEN '3' || 'ml'                        \r\n";
            SQL = SQL + "                                                                               ELSE VLOLUME || 'ml'                                     \r\n";
            SQL = SQL + "                                                                           END                                                          \r\n";
            SQL = SQL + "                                                ELSE VLOLUME || 'ml'                                                                    \r\n";
            SQL = SQL + "                                                 END                       AS TUBEMSG                                                   \r\n";
            SQL = SQL + " 			, BCODENAME                                                                                                                  \r\n";
            SQL = SQL + " 			, SUBSTR(BCODENAME,1,33)  AS BCODENAME1                                                                                      \r\n";
            SQL = SQL + " 			, SUBSTR(BCODENAME,34,66) AS BCODENAME2                                                                                      \r\n";
            SQL = SQL + " 			, (CASE WHEN LENGTH(BCODENAME) < 99 THEN SUBSTR(BCODENAME,67,98) ELSE SUBSTR(BCODENAME,67,98) || '(..)'  END) AS BCODENAME3  \r\n";
            SQL = SQL + " 			, BCODEPRINT                                                                                                                 \r\n";
            SQL = SQL + " 			, HEPARIN                                                                                                                    \r\n";
            SQL = SQL + " 			, SODIUM                                                                                                                     \r\n";
            SQL = SQL + " 			, ERPAT                                                                                                                      \r\n";
            SQL = SQL + " 			, PRINT_ADD                                                                                                                  \r\n";
            SQL = SQL + "   FROM (                                                                                                                               \r\n";
            SQL = SQL + " 			SELECT SPECNO                                                                                                                \r\n";
            SQL = SQL + " 			     , IPDOPD                                                                                                                \r\n";
            SQL = SQL + " 			     , KOSMOS_OCS.FC_EXAM_BLOOD_MASTER_ABO(PANO) 						AS ABO                                               \r\n";
            SQL = SQL + " 			     , PANO                                                             AS PANO                                              \r\n";
            SQL = SQL + " 			     , KOSMOS_OCS.FC_SET_EXAM_BABY(SNAME)	       				        AS SNAME                                             \r\n";
            SQL = SQL + " 			     , KOSMOS_OCS.FC_SET_EXAM_DEPT2(PANO,IPDOPD,DEPTCODE,WARD,ROOM,KOSMOS_OCS.FC_IPD_NEW_MASTER_BEDNUM(S.PANO)) AS DEPTCODE	 --  81000013 정도 관리 환자는 제외      \r\n";            
            SQL = SQL + " 			     , (CASE WHEN IPDOPD ='I' THEN (SELECT REPLACE(MAX(B.NAME),'격리','^') NAME                                   \r\n";
			SQL = SQL + " 			    	     	                      FROM KOSMOS_PMPA.IPD_NEW_MASTER 	A                                                    \r\n";
			SQL = SQL + " 			    	     	                         , KOSMOS_PMPA.BAS_BCODE 		B                                                    \r\n";
			SQL = SQL + " 			    	     	                     WHERE A.OUTDATE IS NULL                                                                 \r\n";
			SQL = SQL + " 			    	     	                       AND A.BEDNUM = B.CODE(+)                                                              \r\n";
			SQL = SQL + " 			    	     	                       AND B.GUBUN(+) = 'NUR_ICU_침상번호'                                                   \r\n";
			SQL = SQL + " 			    	     	                       AND A.PANO = S.PANO                                                                     \r\n";
			SQL = SQL + " 			    	     	                    )                                                                                        \r\n";
            SQL = SQL + " 			             ELSE '' END)                                               AS NAME                                              \r\n";
            SQL = SQL + " 			     , (CASE WHEN IPDOPD = 'I' AND WARD = 'IU' AND ROOM = '233' THEN 'SICU'                                                  \r\n";
 			SQL = SQL + "                        WHEN IPDOPD = 'I' AND WARD = 'IU' AND ROOM = '234' THEN 'MICU'                                                  \r\n";
 			SQL = SQL + "                        WHEN IPDOPD = 'I' THEN WARD                                                                                     \r\n";
             SQL = SQL + "                   END )                                                          AS WARD                                              \r\n";
            SQL = SQL + " 			     , ROOM                                                                                                                  \r\n";
            SQL = SQL + " 			     , KOSMOS_OCS.FC_IPD_NEW_MASTER_BEDNUM(PANO)						AS BED                                               \r\n";
            SQL = SQL + " 			     , DECODE(PANO,'81000013','', S.DRCODE) 							AS DRCODE		--  81000013 정도 관리 환자는 제외   \r\n";
            SQL = SQL + " 			     , DECODE(PANO,'81000013','', S.BI) 								AS BI  			--  81000013 정도 관리 환자는 제외   \r\n";
            SQL = SQL + " 				 , DECODE(PANO,'81000013','', S.AGE) 							    AS AGE			--  81000013 정도 관리 환자는 제외   \r\n";
            SQL = SQL + " 				 , DECODE(PANO,'81000013','', S.SEX) 							    AS SEX			--  81000013 정도 관리 환자는 제외   \r\n";
            SQL = SQL + " 			     , REPLACE(WORKSTS,',','')											AS WORKSTS                                           \r\n";
            SQL = SQL + " 			     , SPECCODE                                                         AS SPECCODE                                          \r\n";
            SQL = SQL + " 				 , KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',SPECCODE,'Y')		            AS SPECNM                                            \r\n";
            SQL = SQL + " 			     , TUBE                                                             AS TUBE                                              \r\n";
            SQL = SQL + " 				 , KOSMOS_OCS.FC_EXAM_SPECMST_NM('15',TUBE,'Y') 			        AS TUBENM                                            \r\n";
            SQL = SQL + " 				 , KOSMOS_OCS.FC_EXAM_RESULTC_VOLUME(SPECNO)		                AS VLOLUME                                           \r\n";
            SQL = SQL + " 				 , (CASE WHEN STRT = 'S' OR STRT = 'E'  THEN 'E' ELSE '' END )      AS STRT                                              \r\n";
            SQL = SQL + " 				 , TO_CHAR(BLOODDATE, 'HH24:MI') 							        AS BDATE                                             \r\n";
            SQL = SQL + " 				 , (CASE WHEN INSTR(WORKSTS,'H') > 0 THEN KOSMOS_OCS.FC_EXAM_PATIENT_2(PANO,'H') END) AS HEPARIN                         \r\n";
            SQL = SQL + " 				 , (CASE WHEN INSTR(WORKSTS,'H') > 0 THEN KOSMOS_OCS.FC_EXAM_PATIENT_2(PANO,'S') END) AS SODIUM                          \r\n";
            SQL = SQL + " 				 , KOSMOS_OCS.FC_BAS_AREA_won(PANO,IPDOPD,BI,DRCODE) 			    AS WON                                               \r\n";
            SQL = SQL + " 				 , KOSMOS_OCS.FC_EXAM_INFECT_MASTER_NM(PANO) 					    AS INFECT                                            \r\n";
            SQL = SQL + " 			     , KOSMOS_OCS.FC_EXAM_RESULTC_BCODENAME(S.SPECNO,S.DEPTCODE ) 		AS BCODENAME                                         \r\n";
            SQL = SQL + " 	   	         , KOSMOS_OCS.FC_EXAM_RESULTC_PRINTCNT(S.SPECNO)                    AS BCODEPRINT                                        \r\n";
            SQL = SQL + " 	   	         , KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(S.PANO,S.BDATE,S.DEPTCODE)	AS ERPAT                                             \r\n";
            SQL = SQL + " 	   	         , KOSMOS_OCS.FC_EXAM_RESULTC_EDTA(S.SPECNO, '')                    AS PRINT_ADD                                         \r\n";
            SQL = SQL + " 			FROM " + ComNum.DB_MED + "EXAM_SPECMST S                                                                                     \r\n";
            SQL = SQL + " 		   WHERE 1=1                                                                                                                     \r\n";
            SQL = SQL + " 			 AND SPECNO = " + ComFunc.covSqlstr(strSpecNo, false);
            SQL = SQL + " 	)                                                                                                                                    \r\n";

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


        public string sel_SPECNO_NEXTVAL(PsmhDb pDbCon, string strDATE)
        {
            string strSPECNO    = string.Empty;
            string strDATE_TMP  = string.Empty;

            DataTable dt = null;

            strDATE_TMP = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            SQL = "";

            if (strDATE == strDATE_TMP)
            {
                SQL = SQL + " SELECT   TO_CHAR(SYSDATE, 'YYMMDD') || TRIM(TO_CHAR(KOSMOS_PMPA.SEQ_EXAMSPECNO.NEXTVAL,'0000')) AS SPECNO   \r\n";
                SQL = SQL + "   FROM DUAL                                                                           \r\n";
            }
            else
            {
                strDATE = strDATE.Replace("-", "");
                strDATE = strDATE.Substring(2);

                SQL = SQL + " SELECT TO_CHAR(TO_NUMBER(NVL(MAX(SPECNO),'0')) +1) AS SPECNO    \r\n";
                SQL = SQL + "   FROM KOSMOS_OCS.EXAM_RESULTC  \r\n";
                SQL = SQL + "  WHERE 1=1                      \r\n";
                SQL = SQL + "    AND SPECNO BETWEEN '" + strDATE + "0000'   \r\n";
                SQL = SQL + "                   AND '" + strDATE + "9999'   \r\n";
            }

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return "";
                }

                strSPECNO = dt.Rows[0]["SPECNO"].ToString();
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }


            return strSPECNO;
        }

        public DataTable sel_EXAM_SPECODE_ALL(PsmhDb pDbCon, string strGUBUN="", string strUSER_YN = "")
        {
            DataTable dt = null;
            SQL = "";
            SQL = SQL + " SELECT                                                                                                                                                                                                                                                                                                       \r\n";
            SQL = SQL + "    '' AS CHK                      \r\n ";
            SQL = SQL + "     , GUBUN             --  11:Work Center  12.Work Station   13.장비 14.검체  15:용기 16:채혈/채취량 17:Comment 18:HELP  19:Foot Note 20:결과단위 21:취소사유  22: 결과변경사유  51.외래검사항목중 해당과검사(검사오더에 전달않함)     (CODE:수가분류 NAME:공란 YNAME:수가코드)  71:장비(INTERFACE 항목설정)     \r\n ";
            SQL = SQL + "     , CODE              -- 각항목의 코드,장비코드                       \r\n ";
            SQL = SQL + "     , NAME              -- 코드내용,장비에서 나오는 코드                \r\n ";
            SQL = SQL + "     , YNAME             -- 내용의약어,검사실에서 사용하는 코드, Work Center,채혈/채취량,Comment,Foot Note,결과단위  취소사유는 YName의 값이 존재 않함.  \r\n ";
            SQL = SQL + "     , WSGROUP           -- WsGroup,WsEqu는 Work Station에서만 사용.  \r\n ";
            SQL = SQL + "     , WSEQU             -- Work Station의 검사장비 표시(aaa,bbb,ccc식으로 저장) WsGroup,WsEqu는 Work Station에서만 사용.  \r\n ";
            SQL = SQL + "     , SORT              -- SORT 우선순위                         \r\n ";
            SQL = SQL + "     , DELDATE           -- 삭제일자                              \r\n ";
            SQL = SQL + "     , USE_YN           -- 사용여부                              \r\n ";
            SQL = SQL + "     , INPS              -- 입력자                                \r\n ";
            SQL = SQL + "     , INPT_DT           -- 입력일시                              \r\n ";
            SQL = SQL + "     , UPPS              -- 수정자                                \r\n ";
            SQL = SQL + "     , UPDT              -- 수정일시                              \r\n ";
            SQL = SQL + "     , '' AS CHANGE      -- 삭제일자                              \r\n ";
            SQL = SQL + "  FROM KOSMOS_OCS.EXAM_SPECODE /** 검사실 장비,검체,용기등 코드*/ \r\n";
            SQL = SQL + " WHERE 1=1                                                        \r\n";

            if (string.IsNullOrEmpty(strGUBUN) == false)
            {
                SQL = SQL + "  AND GUBUN = " + ComFunc.covSqlstr(strGUBUN, false);
            }

            if (string.IsNullOrEmpty(strUSER_YN) == false)
            {
                SQL = SQL + "  AND USE_YN = " + ComFunc.covSqlstr(strUSER_YN, false);
            }

            SQL = SQL + " ORDER BY GUBUN, CODE, NAME \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        //public DataSet sel_EXAM_RESULTC_CHAMGO(string strFDate, string strTDate, string strSUBCODE, string strGBN)
        //{
        //    DataSet ds = null;

        //    string strFAge = "20";
        //    string strTAge = "30";

        //    SQL = "";
        //    SQL += "  SELECT                                                        \r\n";
        //    SQL += "         TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') RESULTDATE          \r\n";
        //    SQL += "       , A.PANO                                                 \r\n";
        //    SQL += "       , A.SNAME                                                \r\n";
        //    SQL += "       , A.SEX                                                  \r\n";
        //    SQL += "       , A.AGE                                                  \r\n";
        //    SQL += "       , B.RESULT                                               \r\n";
        //    SQL += "       , B.REFER                                                \r\n";
        //    SQL += "       , C.EXAMNAME                                             \r\n";
        //    SQL += "       , B.SUBCODE                                              \r\n";
        //    SQL += "   FROM KOSMOS_OCS.EXAM_SPECMST  A                              \r\n";
        //    SQL += "      , KOSMOS_OCS.EXAM_RESULTC B                               \r\n";
        //    SQL += "      , KOSMOS_OCS.EXAM_MASTER C                                \r\n";
        //    SQL += "  WHERE 1=1                                                     \r\n";
        //    SQL += "    AND A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
        //    SQL += "                    AND " + ComFunc.covSqlDate(strTDate, false);
        //    SQL += "    AND B.STATUS    ='V'                                        \r\n";
        //    SQL += "    AND A.SPECNO    = B.SPECNO                                  \r\n";
        //    SQL += "    AND B.SUBCODE   = C.MASTERCODE (+)                          \r\n";
        //    SQL += "    AND B.SUBCODE   = " + ComFunc.covSqlstr(strSUBCODE, false);

        //    if (strGBN == "C1")
        //    {
        //        SQL += "      AND A.AGE BETWEEN " + ComFunc.covSqlstr(strFAge, false);
        //        SQL += "                    AND " + ComFunc.covSqlstr(strTAge, false);
        //        SQL += "      AND A.SEX  = 'M'                                      \r\n";
        //    }
        //    else if (strGBN == "C2")
        //    {
        //        SQL += "      AND A.AGE BETWEEN " + ComFunc.covSqlstr(strFAge, false);
        //        SQL += "                    AND " + ComFunc.covSqlstr(strTAge, false);
        //        SQL += "      AND A.SEX  = 'F'                                      \r\n";
        //    }
        //    else if (strGBN == "C3")
        //    {
        //        SQL += "      AND A.DEPTCODE ='PD'                                  \r\n";
        //    }
        //    else
        //    {
        //        SQL += "      AND A.AGE BETWEEN " + ComFunc.covSqlstr(strFAge, false);
        //        SQL += "                    AND " + ComFunc.covSqlstr(strTAge, false);
        //    }

        //    SQL += "    ORDER BY RESULTDATE DESC                                    \r\n";

        //    try
        //    {
        //        string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
        //            return null;
        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
        //        return null;
        //    }

        //    return ds;
        //}

        //public DataSet sel_EXAM_CHAMGO(string strGBN, string strBDATE, string strEXAMCODE)
        //{
        //    DataSet ds = null;

        //    SQL = "";
        //    SQL += " SELECT                                  \r\n";
        //    SQL += "         GBN                             \r\n";
        //    SQL += "       , BDATE                           \r\n";
        //    SQL += "       , EXAMCODE                        \r\n";
        //    SQL += "       , EXAMNAME                        \r\n";
        //    SQL += "       , SEQNO                           \r\n";
        //    SQL += "       , RESULT                          \r\n";
        //    SQL += "       , ROWID                           \r\n";
        //    SQL += "  FROM KOSMOS_OCS.EXAM_CHAMGO           \r\n";
        //    SQL += " WHERE 1=1                              \r\n";
        //    SQL += "   AND GBN      = " + ComFunc.covSqlstr(strGBN, false);
        //    SQL += "   AND BDATE    = " + ComFunc.covSqlstr(strBDATE, false);
        //    SQL += "   AND EXAMCODE = " + ComFunc.covSqlstr(strEXAMCODE, false);
        //    SQL += " ORDER BY EXAMCODE, BDATE,SEQNO         \r\n";

        //    try
        //    {
        //        string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
        //            return null;
        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
        //        return null;
        //    }

        //    return ds;
        //}

        //public DataSet sel_EXAM_CHAMGO_GROUP(string strGBN, string strEXAMCODE)
        //{
        //    DataSet ds = null;

        //    SQL = "";
        //    SQL += " SELECT                                 \r\n";
        //    SQL += "        EXAMCODE                        \r\n";
        //    SQL += "      , BDATE                           \r\n";
        //    SQL += "  FROM KOSMOS_OCS.EXAM_CHAMGO      \r\n";
        //    SQL += " WHERE 1=1                              \r\n";
        //    SQL += "   AND GBN      = " + ComFunc.covSqlstr(strGBN      , false);
        //    SQL += "   AND EXAMCODE = " + ComFunc.covSqlstr(strEXAMCODE , false);
        //    SQL += " GROUP BY EXAMCODE, BDATE                \r\n";
        //    SQL += " ORDER BY EXAMCODE, BDATE                \r\n";

        //    try
        //    {
        //        string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
        //            return null;
        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
        //        return null;
        //    }

        //    return ds;
        //}

        //public string up_EXAM_MASTER_GBBASE(string strRowid, string strGBBASE, ref int intRowAffected)
        //{
        //    string SqlErr = "";

        //    SQL = "";
        //    SQL += "UPDATE " + ComNum.DB_MED + "EXAM_MASTER SET GBBASE=" + ComFunc.covSqlstr(strGBBASE, false) + " \r\n";
        //    SQL += " WHERE ROWID = " + ComFunc.covSqlstr(strRowid, false);

        //    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //    return SqlErr;

        //}

        //public DataSet sel_EXAM_MASTER_GBBASE(string strWC, string strGBBASE = "")
        //{
        //    DataSet ds = null;

        //    SQL = "";
        //    SQL += " SELECT                                                 \r\n";
        //    SQL += "        DECODE(GBBASE,'*','True','') AS GBBASE          \r\n";
        //    SQL += "      , MASTERCODE                                      \r\n";
        //    SQL += "      , EXAMNAME                                        \r\n";
        //    SQL += "      , ROWID                                           \r\n";
        //    SQL += "  FROM KOSMOS_OCS.EXAM_MASTER /** 검사실 접수 Master*/  \r\n";
        //    SQL += " WHERE 1=1                                              \r\n";
        //    SQL += "   AND TONGBUN    = " + ComFunc.covSqlstr(strWC, false);

        //    if (string.IsNullOrEmpty(strGBBASE) == false)
        //    {
        //        SQL += "   AND GBBASE = '*'                                 \r\n";
        //    }
        //    SQL += "  ORDER BY MASTERCODE  \r\n";


        //    try
        //    {
        //        string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
        //            return null;
        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
        //        return null;
        //    }

        //    return ds;
        //}

        public enum enmSel_EXAM_SPECMST_ALL { SPECNO, PANO, BI, SNAME, IPDOPD, AGE, AGEMM, SEX, DEPTCODE, WARD, ROOM, DRCODE, DRCOMMENT, STRT, WORKSTS, SPECCODE, TUBE, BDATE, BLOODDATE, RECEIVEDATE, RESULTDATE, STATUS, CANCEL, PRINT, ANATNO, GBORDERCODE, EMR, ORDERDATE, SENDDATE, SENDFLAG, GB_GWAEXAM, GBPRINT, HICNO, VIEWID, VIEWDATE, ORDERNO, WDATE, WGBN, WSEQ, WPART, HCV, WGB, WAMT, WHCODE, BCODE, WEXNAME, WSPECNAME, WJDATE, HCVREMARK, IMGWRTNO, GB_GWAEXAM2, GBMIC, GBMICORDER, WCNT, JEPSUSABUN, JEPSUNAME, JEPSUSABUN2, JEPSUNAME2, EXAMNAME }

        public enum enmSel_EXAM_RESULTC_BLOOD {ABO,RH };
       
        public enum enmSel_EXAM_HISRESULTC_STS        {   RESULTWS,      NAME,       M01,       M02,       M03,       M04,       M05,       M06,       M07,       M08,       M09,       M10,       M11,       M12,      MTOT };
        public string[] sSel_EXAM_HISRESULTC_STS    = { "RESULTWS", "검사 WS",    "01월",    "02월",    "03월",    "04월",    "05월",    "06월",    "07월",    "08월",    "09월",    "10월",    "11월",    "12월",    "합계" };
        public int[] nSel_EXAM_HISRESULTC_STS       = {     5, nCol_ORDERNAME, nCol_PANO, nCol_PANO, nCol_PANO, nCol_PANO, nCol_PANO, nCol_PANO, nCol_PANO, nCol_PANO, nCol_PANO, nCol_PANO, nCol_PANO, nCol_PANO, nCol_PANO };

        public enum enmSel_EXAM_SPECODE_FOOTNOTE        {      CODE, NAME          };
        public string[] sSel_EXAM_SPECODE_FOOTNOTE =    {    "코드", "FOOT NOTE"   };
        public int[] nSel_EXAM_SPECODE_FOOTNOTE =       { nCol_PANO, nCol_ORDERNAME + 50};


        public DataTable sel_EXAM_AUTOSEND_CODE(PsmhDb pDbCon, string strPART)
        {
            DataTable dt = null;

            SQL = "";

            //SQL += " SELECT CODE || '.' || NAMEK || '(' || NAMEE || ')' CODE_NM \r\n";
            SQL += " SELECT NEWCODE || '.' || NAMEK || '(' || NAMEE || ')' CODE_NM \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_AUTOSEND_CODE                       \r\n";
            //SQL += "  WHERE PART   = " + ComFunc.covSqlstr(strPART, false);
            SQL += "  WHERE NEWPART   = " + ComFunc.covSqlstr(strPART, false);
            SQL += "   ORDER BY CODE                                            \r\n";
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

        public DataSet sel_EXAM_SPECODE_FOOTNOTE(PsmhDb pDbCon)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                            \r\n";
            SQL += "  		  CODE                      \r\n";
            SQL += "  		, NAME                      \r\n";
            SQL += "    FROM  KOSMOS_OCS.EXAM_SPECODE   \r\n";
            SQL += "  WHERE GUBUN = '19'                \r\n";
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


        public string up_EXAM_ORDER(PsmhDb pDbCon, string strSPECNO, string strROWID, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "UPDATE KOSMOS_OCS.EXAM_ORDER    \r\n" ;
            SQL += "   SET SPECNO = REPLACE(SPECNO, '" + strSPECNO.Trim() + "','') || " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "     , JDATE  = SYSDATE         \r\n";
            SQL += "     , UPPS   = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += "     , UPDT   = SYSDATE         \r\n";
            SQL += " WHERE 1=1                      \r\n";
            SQL += "   AND ROWID  = " + ComFunc.covSqlstr(strROWID, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;
        }

        public DataSet sel_EXAM_HISRESULTC_STS(PsmhDb pDbCon, string strYYYY, string strWS)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                              \r\n";     
            SQL += "  		RESULTWS                                                                      \r\n";
            SQL += "  		, DECODE(GROUPING(RESULTWS),0,MAX(B.NAME),'합  계') AS NAME                   \r\n";
            SQL += "  		--, B.SORT                                                                    \r\n";
            SQL += "  		, TO_CHAR(SUM(DECODE(TO_CHAR(JOBDate, 'YYYYMM'), '" + strYYYY + "01', QTY, 0))) M01    \r\n";
            SQL += "  		, TO_CHAR(SUM(DECODE(TO_CHAR(JOBDate, 'YYYYMM'), '" + strYYYY + "02', QTY, 0))) M02    \r\n";
            SQL += "  		, TO_CHAR(SUM(DECODE(TO_CHAR(JOBDate, 'YYYYMM'), '" + strYYYY + "03', QTY, 0))) M03    \r\n";
            SQL += "  		, TO_CHAR(SUM(DECODE(TO_CHAR(JOBDate, 'YYYYMM'), '" + strYYYY + "04', QTY, 0))) M04    \r\n";
            SQL += "  		, TO_CHAR(SUM(DECODE(TO_CHAR(JOBDate, 'YYYYMM'), '" + strYYYY + "05', QTY, 0))) M05    \r\n";
            SQL += "  		, TO_CHAR(SUM(DECODE(TO_CHAR(JOBDate, 'YYYYMM'), '" + strYYYY + "06', QTY, 0))) M06    \r\n";
            SQL += "  		, TO_CHAR(SUM(DECODE(TO_CHAR(JOBDate, 'YYYYMM'), '" + strYYYY + "07', QTY, 0))) M07    \r\n";
            SQL += "  		, TO_CHAR(SUM(DECODE(TO_CHAR(JOBDate, 'YYYYMM'), '" + strYYYY + "08', QTY, 0))) M08    \r\n";
            SQL += "  		, TO_CHAR(SUM(DECODE(TO_CHAR(JOBDate, 'YYYYMM'), '" + strYYYY + "09', QTY, 0))) M09    \r\n";
            SQL += "  		, TO_CHAR(SUM(DECODE(TO_CHAR(JOBDate, 'YYYYMM'), '" + strYYYY + "10', QTY, 0))) M10    \r\n";
            SQL += "  		, TO_CHAR(SUM(DECODE(TO_CHAR(JOBDate, 'YYYYMM'), '" + strYYYY + "11', QTY, 0))) M11    \r\n";
            SQL += "  		, TO_CHAR(SUM(DECODE(TO_CHAR(JOBDate, 'YYYYMM'), '" + strYYYY + "12', QTY, 0))) M12    \r\n";
            SQL += "  		, TO_CHAR(SUM(DECODE(TO_CHAR(JOBDate, 'YYYY')  , '" + strYYYY + "'  , QTY, 0))) MTOT   \r\n";
            SQL += "    FROM                                                                              \r\n";
            SQL += "    	  ( SELECT                                                                    \r\n";
            SQL += "    	    		 RESULTWS                                                         \r\n";
            SQL += "    	    	   , TRUNC(JOBDATE) JOBDATE                                           \r\n";
            SQL += "    	    	   , COUNT(*) 		QTY                                               \r\n";
            SQL += "             FROM KOSMOS_OCS.EXAM_HISRESULTC                                          \r\n";
            SQL += "            WHERE JOBDate BETWEEN TO_DATE('" + strYYYY + "-01-01', 'YYYY-MM-DD')      \r\n";
            SQL += "                              AND TO_DATE('" + strYYYY + "-12-31', 'YYYY-MM-DD')      \r\n";
            SQL += "              AND JOBGBN IN ('1')                                                     \r\n";

            if (string.IsNullOrEmpty(strWS.Trim()) == false && strWS.Equals("*") == false)
            {
                SQL += "              AND RESULTWS = " + ComFunc.covSqlstr(strWS, false);
            }

            SQL += "            GROUP BY RESULTWS, TRUNC(JOBDATE)                                         \r\n";
            SQL += "          ) A                                                                         \r\n";
            SQL += "      ,   (                                                                           \r\n";
            SQL += "      	  SELECT                                                                      \r\n";
            SQL += "                  YNAME                                                               \r\n";
            SQL += "                , NAME                                                                \r\n";
            SQL += "                , SORT                                                                \r\n";
            SQL += "             FROM KOSMOS_OCS.EXAM_SPECODE                                             \r\n";
            SQL += "            WHERE GUBUN ='12'                                                         \r\n";
            SQL += "              AND SUBSTR(CODE,3,1) ='0'                                               \r\n";
            SQL += "              AND YNAME IS NOT NULL                                                   \r\n";
            SQL += "           ) B                                                                        \r\n";
            SQL += "   WHERE A.RESULTWS = B.YNAME(+)                                                      \r\n";
            //2019-04-12 안정수, 우수희 팀장 요청으로 WS('Z') 제외처리
            SQL += "     AND RESULTWS <> 'Z'                                                              \r\n";
            SQL += "   GROUP BY ROLLUP(RESULTWS)                                                          \r\n";
            SQL += "   ORDER BY GROUPING(RESULTWS), MAX(RESULTWS)                                         \r\n";

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


        public string UP_EXAM_RACK_RACKNO(PsmhDb pDbCon, string strRACKCODE, string strRACKNO_OLD, string strRACKNO_NEW,ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "UPDATE KOSMOS_OCS.EXAM_RACK_" + strRACKCODE + "        \r\n";
            SQL += "   SET RACKNO  = " + ComFunc.covSqlstr(strRACKNO_NEW, false);
            SQL += " WHERE 1=1     \r\n";
            SQL += "   AND RACKNO  = " + ComFunc.covSqlstr(strRACKNO_OLD, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }


        public string del_EXAM_RACK_RACKNO(PsmhDb pDbCon, string strRACKCODE, string strRACKNO, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "DELETE KOSMOS_OCS.EXAM_RACK_" + strRACKCODE + "        \r\n";
            SQL += " WHERE 1=1     \r\n";
            SQL += "   AND RACKNO  = " + ComFunc.covSqlstr(strRACKNO, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public DataTable sel_EXAM_RACK_PANO(PsmhDb pDbCon, string strRACKCODE, string strRACKNO)
        {
            DataTable dt = null;

            SQL = "";

            SQL += " SELECT PANO                                            \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_RACK_" + strRACKCODE + "        \r\n";
            SQL += "  WHERE RACKNO   = " + ComFunc.covSqlstr(strRACKNO, false);
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

        public string del_EXAM_RACK(PsmhDb pDbCon, string strRACKCODE, string strROWID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            //SQL += " DELETE FROM KOSMOS_OCS.EXAM_RACK_" + string.Format("{0:00}", (int)pEnmEXAM_RACK_TYPE) + " \r\n";
            SQL += " DELETE FROM KOSMOS_OCS.EXAM_RACK_" + strRACKCODE + " \r\n";
            SQL += "   WHERE 1=1                                                                    \r\n";
            SQL += "     AND ROWID = " + ComFunc.covSqlstr(strROWID, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;
        }

        public string up_EXAM_RACK(PsmhDb pDbCon, string strRACKCODE, string strYN, string strROWID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            //SQL += " UPDATE KOSMOS_OCS.EXAM_RACK_" + string.Format("{0:00}", (int)pEnmEXAM_RACK_TYPE) + " \r\n";
            SQL += " UPDATE KOSMOS_OCS.EXAM_RACK_" + strRACKCODE + " \r\n";
            SQL += "    SET MOVE = " + ComFunc.covSqlstr(strYN == "True" ? "Y" : "", false);
            SQL += "   WHERE 1=1                                                                    \r\n";
            SQL += "     AND ROWID = " + ComFunc.covSqlstr(strROWID, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;
        }

        public string ins_EXAM_RACK(PsmhDb pDbCon, string strRACKCODE, string strRACKNO, int strPRENO, string strSPECNO, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO KOSMOS_OCS.EXAM_RACK_" + strRACKCODE + " \r\n";
            SQL += " (                                                                              \r\n";
            SQL += "        SPECNO                                                                  \r\n";
            SQL += "      , PANO                                                                    \r\n";
            SQL += "      , SNAME                                                                   \r\n";
            SQL += "      , EXAMNAME                                                                \r\n";
            SQL += "      , RACKNO                                                                  \r\n";
            SQL += "      , PRENO                                                                   \r\n";
            SQL += "      , ENTDATE                                                                 \r\n";
            SQL += " )                                                                              \r\n";
            SQL += " SELECT SPECNO                                                                  \r\n";
            SQL += "      , PANO                                                                    \r\n";
            SQL += "      , SNAME                                                                   \r\n";
            SQL += " 	  , TRIM(                                                                   \r\n";
            SQL += " 	 		KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(                                 \r\n";
            SQL += " 	 									  (                                     \r\n";
            SQL += " 	 									  	SELECT MASTERCODE                   \r\n";
            SQL += " 	 										  FROM KOSMOS_OCS.EXAM_RESULTC      \r\n";
            SQL += " 	 									     WHERE SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += " 	 									       AND SEQNO  = '001'               \r\n";
            SQL += " 	 									   )                                    \r\n";
            SQL += " 	 									   )  )AS EXAMNAME                      \r\n";
            SQL += "      , '" + strRACKNO + "' AS RACKNO                                           \r\n";
            SQL += "      ,  " + strPRENO  + "  AS PRENO                                            \r\n";
            SQL += "      , TRUNC(SYSDATE) AS ENTDATE                                               \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_SPECMST S                                               \r\n";
            SQL += "   WHERE 1=1                                                                    \r\n";
            SQL += "     AND SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;
        }

        public DataTable sel_EXAM_RESULTC_BLOOD(PsmhDb pDbCon, string strSPECNO)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT ABO                                                         \r\n";
            SQL += "      , RH                                                          \r\n";
            SQL += "   FROM (                                                           \r\n";
            SQL += " 		SELECT CASE WHEN MASTERCODE IN ('BB01','BB011') THEN RESULT \r\n";
            SQL += " 			    END ABO                                             \r\n";
            SQL += " 		     , CASE WHEN MASTERCODE IN ('BB05') THEN RESULT         \r\n";
            SQL += " 			    END RH                                              \r\n";
            SQL += " 		 FROM KOSMOS_OCS.EXAM_RESULTC   A                           \r\n";
            SQL += " 		WHERE MASTERCODE IN ('BB01','BB011','BB05')                 \r\n";
            SQL += " 		  AND SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += " )                                                                  \r\n";
            SQL += " GROUP BY ABO,RH                                                    \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        public DataTable sel_EXAM_SPECMST_ALL(PsmhDb pDbCon, string strSPECNO)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                         \r\n";
            SQL += "       SPECNO           -- 검체번호                                             \r\n";
            SQL += "     , PANO             -- 등록번호, 종합건진번호, 일반건진번호, 특수건진번호   \r\n";
            SQL += "     , BI               -- 환자구분(아래참조)                                   \r\n";
            SQL += "     , SNAME            -- 이름                                                 \r\n";
            SQL += "     , IPDOPD           -- 입원(I),외래(O)                                      \r\n";
            SQL += "     , AGE              -- 나이                                                 \r\n";
            SQL += "     , AGEMM            -- 유아 개월수                                          \r\n";
            SQL += "     , SEX              -- 성별(M:남,F:여)                                      \r\n";
            SQL += "     , DEPTCODE         -- 진료과                                               \r\n";
            SQL += "     , WARD             -- 병동                                                 \r\n";  
            SQL += "     , ROOM             -- 병실                                                 \r\n";  
            SQL += "     , DRCODE           -- 의사코드                                             \r\n";
            SQL += "     , DRCOMMENT        -- 의사 Comment(형태:검사코드+의사컴멘트)               \r\n";
            SQL += "     , STRT             -- 응급여부(S:응급, R:Routine)                          \r\n";
            SQL += "     , WORKSTS          -- BarCode에 인쇄될 문자                                \r\n";
            SQL += "     , SPECCODE         -- 검체코드                                             \r\n";
            SQL += "     , TUBE             -- 용기코드                                             \r\n";
            SQL += "     , BDATE            -- 처방일                                               \r\n";
            SQL += "     , BLOODDATE        -- 채혈일시                                             \r\n";
            SQL += "     , RECEIVEDATE      -- 접수일시                                             \r\n";
            SQL += "     , RESULTDATE       -- 결과일시/취소일시                                    \r\n";
            SQL += "     , STATUS           -- 상태(아래참조)                                       \r\n";
            SQL += "     , CANCEL           -- 취소사유(Code로 관리)                                \r\n";
            SQL += "     , PRINT            -- 출력횟수(검체별 결과가 완료된경우만 인쇄함)          \r\n";
            SQL += "     , ANATNO           -- 병리번호/취소자사번                                  \r\n";
            SQL += "     , GBORDERCODE      -- 오더코드                                             \r\n";
            SQL += "     , EMR              -- EMR(검사서식변환여부) 0.결과입력/변경 1.서식변환     \r\n";
            SQL += "     , ORDERDATE        -- 오더시간(검사지시시각)(2005/08/25부터)               \r\n";
            SQL += "     , SENDDATE         -- 오더전송시간(2005/08/25부터)                         \r\n";
            SQL += "     , SENDFLAG         -- 건진,종검 결과전송 Flag                              \r\n";
            SQL += "     , GB_GWAEXAM       -- 응급실 등 과검사(Y.과검사)                           \r\n";
            SQL += "     , GBPRINT          -- 병동에서 검사항목 바코드출력할때 => Y 표시함.        \r\n";
            SQL += "     , HICNO            -- 검진,종검 접수번호(결과전송용)                       \r\n";
            SQL += "     , VIEWID           -- 결과 확인자                                          \r\n";
            SQL += "     , VIEWDATE         -- 결과 확인일시                                        \r\n";
            SQL += "     , ORDERNO          -- ocs오더번호                                          \r\n";
            SQL += "     , WDATE            -- 외부의뢰일자                                         \r\n";
            SQL += "     , WGBN             -- 1.삼광의료재단 2.영대, 3.씨젠                        \r\n";
            SQL += "     , WSEQ             -- 외부의뢰순서                                         \r\n";
            SQL += "     , WPART            -- 1:일반, 2:조직                                       \r\n";
            SQL += "     , HCV              -- HCV 신고일자( 감염관리 프로그램:EXINFECT에서 값발생) \r\n";
            SQL += "     , WGB              -- 1.보험, 2.비보험, 3.개별(엑셀자료)                   \r\n";
            SQL += "     , WAMT             -- 외부(삼광) 단가(엑셀자료)                            \r\n";
            SQL += "     , WHCODE           -- 외부(삼광) 항목코드(엑셀자료)                        \r\n";
            SQL += "     , BCODE            -- 표준코드(엑셀자료)                                   \r\n";
            SQL += "     , WEXNAME          -- 외부(삼광)검사명(엑셀자료)                           \r\n";
            SQL += "     , WSPECNAME        -- 외부(삼광)검체명(엑셀자료)                           \r\n";
            SQL += "     , WJDATE           -- 외부(삼광)접수일(엑셀자료)                           \r\n";
            SQL += "     , HCVREMARK        -- HCV 참고사항(감염관리 프로그램:EXINFECT에서 값발생)  \r\n";
            SQL += "     , IMGWRTNO         -- EXAM_RESULT_IMG의 WRTNO 값                           \r\n";
            SQL += "     , GB_GWAEXAM2      -- 과검사                                               \r\n";
            SQL += "     , GBMIC            -- MIC수가(B4062D) 발생요청일                           \r\n";
            SQL += "     , GBMICORDER       -- MIC수가 오더발생일 OR 수납일                         \r\n";
            SQL += "     , WCNT             -- 외부의뢰 검체 건수                                   \r\n";
            SQL += "     , JEPSUSABUN       -- 검체접수                                             \r\n";
            SQL += "     , JEPSUNAME        -- 검체접수자명                                         \r\n";
            SQL += "     , JEPSUSABUN2      -- 검사실검체접수자사번                                 \r\n";
            SQL += "     , JEPSUNAME2       -- 검사실검체접수자명                                   \r\n";
            SQL += "     , KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(                                      \r\n";
            SQL += "                                    (                                           \r\n";
            SQL += "                                       SELECT MASTERCODE                               \r\n";
            SQL += "                                         FROM KOSMOS_OCS.EXAM_RESULTC                  \r\n";
            SQL += "                                        WHERE SPECNO = " + ComFunc.covSqlstr(strSPECNO,false);
            SQL += "                                          AND SEQNO = '001'                            \r\n";
            SQL += "                                    )                                           \r\n";
            SQL += "                               ) AS EXAMNAME                                    \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_SPECMST A                                                \r\n";
            SQL += " WHERE 1=1                                                                      \r\n";
            SQL += "   AND SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        public DataSet sel_EXAM_RACK_SEARCH(PsmhDb pDbCon, string strPTNO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " WITH T AS(                                                                                \r\n";
            SQL += " 		SELECT RACKNO, PRENO, SPECNO, PANO, SNAME, EXAMNAME FROM KOSMOS_OCS.EXAM_RACK_01   \r\n";
            SQL += " 		 UNION ALL                                                                         \r\n";
            SQL += " 		SELECT RACKNO, PRENO, SPECNO, PANO, SNAME, EXAMNAME FROM KOSMOS_OCS.EXAM_RACK_02   \r\n";
            SQL += " 		 UNION ALL                                                                         \r\n";
            SQL += " 		SELECT RACKNO, PRENO, SPECNO, PANO, SNAME, EXAMNAME FROM KOSMOS_OCS.EXAM_RACK_03   \r\n";
            SQL += " 		 UNION ALL                                                                         \r\n";
            SQL += " 		SELECT RACKNO, PRENO, SPECNO, PANO, SNAME, EXAMNAME FROM KOSMOS_OCS.EXAM_RACK_04   \r\n";
            SQL += " 		 UNION ALL                                                                         \r\n";
            SQL += " 		SELECT RACKNO, PRENO, SPECNO, PANO, SNAME, EXAMNAME FROM KOSMOS_OCS.EXAM_RACK_05   \r\n";
            SQL += " 		 UNION ALL                                                                         \r\n";
            SQL += " 		SELECT RACKNO, PRENO, SPECNO, PANO, SNAME, EXAMNAME FROM KOSMOS_OCS.EXAM_RACK_06   \r\n";
            SQL += " 		 UNION ALL                                                                         \r\n";
            SQL += " 		SELECT RACKNO, PRENO, SPECNO, PANO, SNAME, EXAMNAME FROM KOSMOS_OCS.EXAM_RACK_07   \r\n";
            SQL += " 		 UNION ALL                                                                         \r\n";
            SQL += " 		SELECT RACKNO, PRENO, SPECNO, PANO, SNAME, EXAMNAME FROM KOSMOS_OCS.EXAM_RACK_08   \r\n";
            SQL += " 		 UNION ALL                                                                         \r\n";
            SQL += " 		SELECT RACKNO, PRENO, SPECNO, PANO, SNAME, EXAMNAME FROM KOSMOS_OCS.EXAM_RACK_09   \r\n";
            SQL += " 		 UNION ALL                                                                         \r\n";
            SQL += " 		SELECT RACKNO, PRENO, SPECNO, PANO, SNAME, EXAMNAME FROM KOSMOS_OCS.EXAM_RACK_10   \r\n";
            SQL += " 		 UNION ALL                                                                         \r\n";
            SQL += " 		SELECT RACKNO, PRENO, SPECNO, PANO, SNAME, EXAMNAME FROM KOSMOS_OCS.EXAM_RACK_11   \r\n";
            SQL += " 		 UNION ALL                                                                         \r\n";
            SQL += " 		SELECT RACKNO, PRENO, SPECNO, PANO, SNAME, EXAMNAME FROM KOSMOS_OCS.EXAM_RACK_12   \r\n";
            SQL += " 		 UNION ALL                                                                         \r\n";
            SQL += " 		SELECT RACKNO, PRENO, SPECNO, PANO, SNAME, EXAMNAME FROM KOSMOS_OCS.EXAM_RACK_13   \r\n";
            SQL += " 		 UNION ALL                                                                         \r\n";
            SQL += " 		SELECT RACKNO, PRENO, SPECNO, PANO, SNAME, EXAMNAME FROM KOSMOS_OCS.EXAM_RACK_14   \r\n";
            SQL += " 		)                                                                                  \r\n";
            SQL += "  SELECT RACKNO, PRENO, SPECNO, PANO, SNAME, EXAMNAME                                      \r\n";
            SQL += "   FROM T                                                                                  \r\n";
            SQL += "  WHERE 1=1                                                                                \r\n";
            SQL += "    AND PANO = " + ComFunc.covSqlstr(strPTNO, false);
            SQL += "  ORDER BY RACKNO, PRENO,SPECNO                                                            \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

            return ds;
        }

        public string sel_EXAM_RACK_INFO(PsmhDb pDbCon, string strRACKCODE, string strSPECNO)
        {

            string strReturn = "";
            DataTable  dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                             \r\n";
            SQL += "         'Rack-Part: ' || (SELECT CODE ||'.'|| NAME FROM KOSMOS_OCS.EXAM_RACK_CODE WHERE CODE = '"+ strRACKCODE + "') || CHR(13)    \r\n";
            SQL += "      || 'Rack-NO  : ' || RACKNO || chr(13)                                                                         \r\n";
            SQL += "      || '     NO  : ' || PRENO  || chr(13)                                                                         \r\n";
            SQL += "      || '등록번호 : ' || PANO   || chr(13)                                                                         \r\n";
            SQL += "      || '환자명   : ' || SNAME  || chr(13) AS INFO                                                                 \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_RACK_" + strRACKCODE + " A                                       \r\n";
            SQL += "  WHERE SPECNO   = " + ComFunc.covSqlstr(strSPECNO, false);

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    return dt.Rows[0][0].ToString();
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }

            return strReturn;
        }

        public DataSet sel_EXAM_RACK_DIS(PsmhDb pDbCon, string strFDate, string strTDate, string strRACKNO, string strRACKCODE)            
        {
            DataSet ds = null;

            SQL = "";
            SQL += " WITH T AS                                                                                   \r\n";
            SQL += " (                                                                                           \r\n";
            SQL += "      SELECT                                                                                 \r\n";
            SQL += "            A.SPECNO                                                                         \r\n";
            SQL += "          , A.PANO                                                                           \r\n";
            SQL += "          , A.SNAME                                                                          \r\n";
            SQL += "          , B.SEX                                                                            \r\n";
            SQL += "          , B.AGE                                                                            \r\n";
            SQL += "          , B.DEPTCODE                                                                       \r\n";
            SQL += "          , DECODE(TO_CHAR(B.ROOM),'0','',TO_CHAR(B.ROOM))   AS ROOM                         \r\n";
            SQL += "          , TO_CHAR(B.BLOODDATE,'DY') 	                    AS BLOODDATE                     \r\n";
            SQL += "          , A.EXAMNAME                                                                       \r\n";
            SQL += "          , A.RACKNO                                                                         \r\n";
            SQL += "          , A.PRENO                                                                          \r\n";
            SQL += "          , TO_CHAR(A.ENTDATE,'YYYY-MM-DD')  AS ENTDATE                                      \r\n";
            SQL += "          , '' DEL                                                                           \r\n";
            SQL += "          , DECODE(MOVE,'Y','True')          AS MOVE_SPEC                                    \r\n";
            SQL += "          , '' CHK                                                                           \r\n";
            SQL += "          , A.ROWID                                                                          \r\n";
            SQL += "       FROM KOSMOS_OCS.EXAM_RACK_" + strRACKCODE + " A                                       \r\n";
            SQL += "          , KOSMOS_OCS.EXAM_SPECMST B                                                        \r\n";
            SQL += "      WHERE 1=1                                                                              \r\n";
            SQL += "        AND A.RACKNO = " + ComFunc.covSqlstr(strRACKNO, false);
            SQL += "        AND A.DELDATE IS NULL                                                                \r\n";
            SQL += "        AND A.ENTDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                          AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "        AND A.SPECNO = B.SPECNO(+)                                                          \r\n";
            SQL += " ORDER BY A.PRENO                                                                           \r\n";
            SQL += " )                                                                                          \r\n";
            SQL += " SELECT T.*                                                                                 \r\n";
            SQL += "   FROM T                                                                                   \r\n";
            SQL += " ORDER BY T.PRENO                                                                            \r\n";
            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

            return ds;

        }

        public DataSet sel_EXAM_RACK_NO(PsmhDb pDbCon, string strGUBUN)
        {
            DataSet ds = null;

            SQL = "";
            SQL = SQL + " SELECT                            \r\n";
            SQL = SQL + "    ''            AS CHK           \r\n";
            SQL = SQL + "     , A.GUBUN    AS GUBUN         \r\n";
            SQL = SQL + "     , B.NAME     AS NAME          \r\n";
            SQL = SQL + "     , A.CODE     AS CODE          \r\n";
            SQL = SQL + "     , A.SSEQNO   AS SSEQNO        \r\n";
            SQL = SQL + "     , A.ESEQNO   AS ESEQNO        \r\n";
            SQL = SQL + "     , A.ROWID                     \r\n";
            SQL = SQL + "  FROM KOSMOS_OCS.EXAM_RACK_NO   A \r\n";
            SQL = SQL + "     , KOSMOS_OCS.EXAM_RACK_CODE B \r\n";
            SQL = SQL + " WHERE 1=1                         \r\n";
            SQL = SQL + "   AND A.GUBUN = B.CODE            \r\n";

            

            SQL = SQL + "   AND A.GUBUN = " + ComFunc.covSqlstr(strGUBUN, false);
            SQL = SQL + " ORDER BY A.CODE                   \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

            return ds;
        }

        public DataTable sel_EXAM_HISMSTSUB(PsmhDb pDbCon)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                        \r\n";
            SQL += "       JOBDATE --변경일자및시각(SYSTEM Time)   \r\n ";
            SQL += "     , JOBGBN --작업구분(D.삭제 M.수정전)  \r\n ";
            SQL += "     , JOBSABUN --작업자 사번  \r\n ";
            SQL += "     , MASTERCODE --마스터 코드  \r\n ";
            SQL += "     , GUBUN --각항목 구분자(아래 참고사항 확인)  \r\n ";
            SQL += "     , SORT --SORT번호(001-999)  \r\n ";
            SQL += "     , NORMAL --각항목 코드,Comment  \r\n ";
            SQL += "     , SEX --성별(M:남, F:여)  \r\n ";
            SQL += "     , AGEFROM --나이(From)  \r\n ";
            SQL += "     , AGETO --나이(To)  \r\n ";
            SQL += "     , REFVALFROM --Reference Value(From)  \r\n ";
            SQL += "     , REFVALTO --Reference Value(To)  \r\n ";                        
            //2018-12-10 안정수 추가함
            SQL += "     , EXPIREDATE --유효기간 Date  \r\n ";
            SQL += "  FROM KOSMOS_OCS.EXAM_HISMSTSUB /** 검사코드 SUB 마스타 변경내역 History*/ \r\n";
            SQL += " WHERE ROWNUM = 1                                   \r\n";


            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        public DataTable sel_EXAM_HISMST(PsmhDb pDbCon)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT  \r\n";
            SQL += "       JOBDATE --변경일자및시각(SYSTEM Time)  \r\n ";
            SQL += "     , JOBGBN --구분(D.삭제 M.변경전)  \r\n ";
            SQL += "     , JOBSABUN --작업자사번  \r\n ";
            SQL += "     , MASTERCODE --Master Key  \r\n ";
            SQL += "     , EXAMNAME --검사명:통상명칭  \r\n ";
            SQL += "     , EXAMFNAME --검사명:Full Name  \r\n ";
            SQL += "     , EXAMYNAME --검사명:연보에 실린 이름  \r\n ";
            SQL += "     , WSCODE1 --Work Station 1  \r\n ";
            SQL += "     , WSCODE1POS --검사 항목 결과 위치  \r\n ";
            SQL += "     , WSCODE2 --Work Station 2  \r\n ";
            SQL += "     , WSCODE2POS --검사 항목 결과 위치  \r\n ";
            SQL += "     , WSCODE3 --Work Station 3  \r\n ";
            SQL += "     , WSCODE3POS --검사 항목 결과 위치  \r\n ";
            SQL += "     , WSCODE4 --Work Station 4  \r\n ";
            SQL += "     , WSCODE4POS --검사 항목 결과 위치  \r\n ";
            SQL += "     , WSCODE5 --Work Station 5  \r\n ";
            SQL += "     , WSCODE5POS --검사 항목 결과 위치  \r\n ";
            SQL += "     , TURNTIME1 --Turn Around Time(일)  \r\n ";
            SQL += "     , TURNTIME2 --Turn Around Time(분)  \r\n ";
            SQL += "     , EXAMWEEK --검사요일(일월화수목금토) (Y:검사, N:검사안함)  \r\n ";
            SQL += "     , TUBECODE --용기종류코드  \r\n ";
            SQL += "     , SPECCODE --검체종류코드  \r\n ";
            SQL += "     , VOLUMECODE --채혈량코드  \r\n ";
            SQL += "     , EQUCODE1 --장비코드  \r\n ";
            SQL += "     , EQUCODE2 --장비코드  \r\n ";
            SQL += "     , EQUCODE3 --장비코드  \r\n ";
            SQL += "     , EQUCODE4 --장비코드  \r\n ";
            SQL += "     , UNITCODE --결과단위코드  \r\n ";
            SQL += "     , DATATYPE --Data Type  \r\n ";
            SQL += "     , DATALENGTH --Data Length  \r\n ";
            SQL += "     , RESULTIN --결과입력 유무(1:결과 입력 안함, 0:결과 입력 함)  \r\n ";
            SQL += "     , PANICFROM --Panic Value(From)  \r\n ";
            SQL += "     , PANICTO --Panic Value(To)  \r\n ";
            SQL += "     , DELTAM --Delta Value(-)  \r\n ";
            SQL += "     , DELTAP --Delta value(+)  \r\n ";
            SQL += "     , DDDPRDRP --DD, DP, RD, RP  \r\n ";
            SQL += "     , BCODENAME --Barcode 출력명  \r\n ";
            SQL += "     , BCODEPRINT --Barcode 인쇄장수  \r\n ";
            SQL += "     , KEYPAD --KeyPad Position(Diff용)  \r\n ";
            SQL += "     , SERIES --연속검사  \r\n ";
            SQL += "     , PENDING --Pending Checking 여부(1:Check, 0:Check 안함)  \r\n ";
            SQL += "     , TO_CHAR(ENDDATE,'YYYY-MM-DD') AS ENDDATE --사용종료일(삭제일자)  \r\n ";
            SQL += "     , MODIFYDATE --변경일자  \r\n ";
            SQL += "     , MODIFYID --변경자사번  \r\n ";
            SQL += "     , SUB --오직 Sub로만 사용할 때 표시(1:Sub, 0:Mother)  \r\n ";
            SQL += "     , SUCODE --수가코드  \r\n ";
            SQL += "     , SUBUN --수가코드 분류  \r\n ";
            SQL += "     , MOTHER --모코드를 서브코드로 가진 것을 표시(1:존재, 0:없음)  \r\n ";
            SQL += "  FROM KOSMOS_OCS.EXAM_HISMST /** 검사코드 Master*/ \r\n";
            SQL += " WHERE ROWNUM = 1                                   \r\n";


            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        public DataTable sel_EXAM_RACK_CNT(PsmhDb pDbCon, string strRACKCODE)
        {
            DataTable dt = null;

            SQL = "";
            //SQL += " SELECT                            \r\n";
            //SQL += "       RACKNO                      \r\n";
            //SQL += "     , COUNT(SPECNO) CNT           \r\n";
            ////SQL += "  FROM KOSMOS_OCS.EXAM_RACK_" + string.Format("{0:00}", (int)pEnmEXAM_RACK_TYPE) + "    \r\n";                     
            //SQL += "  FROM KOSMOS_OCS.EXAM_RACK_" + strRACKCODE + "    \r\n";
            //SQL = SQL + " WHERE 1=1                         \r\n";
            //SQL = SQL + "   AND DELDATE IS NULL             \r\n";
            //SQL = SQL + "   AND PANO IS NOT NULL            \r\n";
            //SQL = SQL + " GROUP BY RACKNO                   \r\n";
            //SQL = SQL + " ORDER BY RACKNO                   \r\n";

            SQL += " SELECT CODE AS RACKNO                                                              \r\n";
            SQL += "       , (SELECT COUNT(1) FROM KOSMOS_OCS.EXAM_RACK_" + strRACKCODE + " WHERE RACKNO = CODE) CNT     \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_RACK_NO                                                     \r\n";
            SQL += "  WHERE GUBUN = " + ComFunc.covSqlstr(strRACKCODE, false);
            SQL += "  ORDER BY CODE                                                                     \r\n";
            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        public string del_EXAM_MASTER_SUB(PsmhDb pDbCon, string strMASTERCODE, ref int intRowAffected, string strGUBUN = "")
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_MED + "EXAM_MASTER_SUB                   \r\n";
            SQL += "  WHERE 1=1                                                         \r\n";
            SQL += "    AND MASTERCODE = " + ComFunc.covSqlstr(strMASTERCODE, false);

            if (string.IsNullOrEmpty(strGUBUN) == false)
            {
                SQL += "    AND GUBUN      = " + ComFunc.covSqlstr(strGUBUN, false);
            }
            
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_EXAM_MASTER_SUB(PsmhDb pDbCon, DataRow dr, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "EXAM_MASTER_SUB                         \r\n";
            SQL += "   (                                                                      \r\n";
            SQL += "       MASTERCODE   --마스터 코드  \r\n";
            SQL += "     , GUBUN        --각항목 구분자(아래 참고사항 확인)  \r\n";
            SQL += "     , SORT         --SORT번호(001-999)  \r\n";
            SQL += "     , NORMAL       --각항목 코드,Comment  \r\n";
            SQL += "     , SEX          --성별(M:남, F:여)  \r\n";
            SQL += "     , AGEFROM      --나이(From)  \r\n";
            SQL += "     , AGETO        --나이(To)  \r\n";
            SQL += "     , REFVALFROM   --Reference Value(From)  \r\n";
            SQL += "     , REFVALTO     --Reference Value(To)  \r\n";
            //2018-12-10 안정수 추가
            SQL += "     , EXPIREDATE   --유효기간                                            \r\n";
            SQL += "   )                                                                      \r\n";
            SQL += "   VALUES                                                                 \r\n";
            SQL += "   (                                                                      \r\n";

            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER_SUB_BASIC.MASTERCODE].ToString(), false);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER_SUB_BASIC.GUBUN].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER_SUB_BASIC.SORT].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER_SUB_BASIC.NORMAL].ToString().Replace("'", "`"), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER_SUB_BASIC.SEX].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER_SUB_BASIC.AGEFROM].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER_SUB_BASIC.AGETO].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER_SUB_BASIC.REFVALFROM].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER_SUB_BASIC.REFVALTO].ToString(), true);
            //2018-12-10 안정수 추가
            SQL += ", '" + VB.Left(dr[(int)enmSel_EXAM_MASTER_SUB_BASIC.EXPIREDATE].ToString(), 10) + "' ";
            SQL += "   )                                                                     \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string del_EXAM_MASTER(PsmhDb pDbCon, string strMASTERCODE, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_MED + "EXAM_MASTER  \r\n";
            SQL += "  WHERE 1=1                                     \r\n";
            SQL += "    AND MASTERCODE = " + ComFunc.covSqlstr(strMASTERCODE, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_EXAM_HISMST(PsmhDb pDbCon, DataRow dr, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "EXAM_HISMST                             \r\n";
            SQL += "   (                                                                      \r\n";
            SQL += "       JOBDATE      -- 변경일자및시각(SYSTEM Time)                        \r\n";
            SQL += "     , JOBGBN       -- 구분(D.삭제 M.변경전)                              \r\n";
            SQL += "     , JOBSABUN     -- 작업자사번                                         \r\n";
            SQL += "     , MASTERCODE   -- Master Key                                         \r\n";
            SQL += "     , EXAMNAME     -- 검사명:통상명칭                                    \r\n";
            SQL += "     , EXAMFNAME    -- 검사명:Full Name                                   \r\n";
            SQL += "     , EXAMYNAME    -- 검사명:연보에 실린 이름                            \r\n";
            SQL += "     , WSCODE1      -- Work Station 1                                     \r\n";
            SQL += "     , WSCODE1POS   -- 검사 항목 결과 위치                                \r\n";
            SQL += "     , WSCODE2      -- Work Station 2                                     \r\n";
            SQL += "     , WSCODE2POS   -- 검사 항목 결과 위치                                \r\n";
            SQL += "     , WSCODE3      -- Work Station 3                                     \r\n";
            SQL += "     , WSCODE3POS   -- 검사 항목 결과 위치                                \r\n";
            SQL += "     , WSCODE4      -- Work Station 4                                     \r\n";
            SQL += "     , WSCODE4POS   -- 검사 항목 결과 위치                                \r\n";
            SQL += "     , WSCODE5      -- Work Station 5                                     \r\n";
            SQL += "     , WSCODE5POS   -- 검사 항목 결과 위치                                \r\n";
            SQL += "     , TURNTIME1    -- Turn Around Time(일)                               \r\n";
            SQL += "     , TURNTIME2    -- Turn Around Time(분)                               \r\n";
            SQL += "     , EXAMWEEK     -- 검사요일(일월화수목금토) (Y:검사, N:검사안함)      \r\n";
            SQL += "     , TUBECODE     -- 용기종류코드                                       \r\n";
            SQL += "     , SPECCODE     -- 검체종류코드                                       \r\n";
            SQL += "     , VOLUMECODE   -- 채혈량코드                                         \r\n";
            SQL += "     , EQUCODE1     -- 장비코드                                           \r\n";
            SQL += "     , EQUCODE2     -- 장비코드                                           \r\n";
            SQL += "     , EQUCODE3     -- 장비코드                                           \r\n";
            SQL += "     , EQUCODE4     -- 장비코드                                           \r\n";
            SQL += "     , UNITCODE     -- 결과단위코드                                       \r\n";
            SQL += "     , DATATYPE     -- Data Type                                          \r\n";
            SQL += "     , DATALENGTH   -- Data Length                                        \r\n";
            SQL += "     , RESULTIN     -- 결과입력 유무(1:결과 입력 안함, 0:결과 입력 함)    \r\n";
            SQL += "     , PANICFROM    -- Panic Value(From)                                  \r\n";
            SQL += "     , PANICTO      -- Panic Value(To)                                    \r\n";
            SQL += "     , DELTAM       -- Delta Value(-)                                     \r\n";
            SQL += "     , DELTAP       -- Delta value(+)                                     \r\n";
            SQL += "     , DDDPRDRP     -- DD, DP, RD, RP                                     \r\n";
            SQL += "     , BCODENAME    -- Barcode 출력명                                     \r\n";
            SQL += "     , BCODEPRINT   -- Barcode 인쇄장수                                   \r\n";
            SQL += "     , KEYPAD       -- KeyPad Position(Diff용)                            \r\n";
            SQL += "     , SERIES       -- 연속검사                                           \r\n";
            SQL += "     , PENDING      -- Pending Checking 여부(1:Check, 0:Check 안함)       \r\n";
            SQL += "     , ENDDATE      -- 사용종료일(삭제일자)                               \r\n";
            SQL += "     , MODIFYDATE   -- 변경일자                                           \r\n";
            SQL += "     , MODIFYID     -- 변경자사번                                         \r\n";
            SQL += "     , SUB          -- 오직 Sub로만 사용할 때 표시(1:Sub, 0:Mother)       \r\n";
            SQL += "     , SUCODE       -- 수가코드                                           \r\n";
            SQL += "     , SUBUN        -- 수가코드 분류                                      \r\n";
            SQL += "     , MOTHER       -- 모코드를 서브코드로 가진 것을 표시(1:존재, 0:없음) \r\n";
            SQL += "   )                                                                      \r\n";
            SQL += "   VALUES                                                                 \r\n";
            SQL += "   (                                                                      \r\n";
            SQL += " SYSDATE                                                                  \r\n"; 
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.JOBGBN].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.JOBSABUN].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.MASTERCODE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.EXAMNAME].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.EXAMFNAME].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.EXAMYNAME].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.WSCODE1].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.WSCODE1POS].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.WSCODE2].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.WSCODE2POS].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.WSCODE3].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.WSCODE3POS].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.WSCODE4].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.WSCODE4POS].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.WSCODE5].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.WSCODE5POS].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.TURNTIME1].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.TURNTIME2].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.EXAMWEEK].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.TUBECODE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.SPECCODE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.VOLUMECODE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.EQUCODE1].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.EQUCODE2].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.EQUCODE3].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.EQUCODE4].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.UNITCODE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.DATATYPE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.DATALENGTH].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.RESULTIN].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.PANICFROM].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.PANICTO].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.DELTAM].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.DELTAP].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.DDDPRDRP].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.BCODENAME].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.BCODEPRINT].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.KEYPAD].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.SERIES].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.PENDING].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.ENDDATE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.MODIFYDATE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.MODIFYID].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.SUB].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.SUCODE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.SUBUN].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMST.MOTHER].ToString(), true);


            SQL += "   )                                                                     \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_EXAM_MASTER(PsmhDb pDbCon, DataRow dr, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "EXAM_MASTER                             \r\n";
            SQL += "   (                                                                      \r\n";
            SQL += "       MASTERCODE       -- Master Key  \r\n";
            SQL += "     , EXAMNAME         -- 검사명:통상명칭  \r\n";
            SQL += "     , EXAMFNAME        -- 검사명:Full Name  \r\n";
            SQL += "     , EXAMYNAME        -- 검사명:연보에 실린 이름  \r\n";
            SQL += "     , WSCODE1          -- Work Station 1  \r\n";
            SQL += "     , WSCODE1POS       -- 검사 항목 결과 위치  \r\n";
            SQL += "     , WSCODE2          -- Work Station 2  \r\n";
            SQL += "     , WSCODE2POS       -- 검사 항목 결과 위치  \r\n";
            SQL += "     , WSCODE3          -- Work Station 3  \r\n";
            SQL += "     , WSCODE3POS       -- 검사 항목 결과 위치  \r\n";
            SQL += "     , WSCODE4          -- Work Station 4  \r\n";
            SQL += "     , WSCODE4POS       -- 검사 항목 결과 위치  \r\n";
            SQL += "     , WSCODE5          -- Work Station 5  \r\n";
            SQL += "     , WSCODE5POS       -- 검사 항목 결과 위치  \r\n";
            SQL += "     , TURNTIME1        -- Turn Around Time(일)  \r\n";
            SQL += "     , TURNTIME2        -- Turn Around Time(분)  \r\n";
            SQL += "     , EXAMWEEK         -- 검사요일(일월화수목금토) (Y:검사, N:검사안함)  \r\n";
            SQL += "     , TUBECODE         -- 용기종류코드  \r\n";
            SQL += "     , SPECCODE         -- 검체종류코드  \r\n";
            SQL += "     , VOLUMECODE       -- 채혈량코드  \r\n";
            SQL += "     , EQUCODE1         -- 장비코드  \r\n";
            SQL += "     , EQUCODE2         -- 장비코드  \r\n";
            SQL += "     , EQUCODE3         -- 장비코드  \r\n";
            SQL += "     , EQUCODE4         -- 장비코드  \r\n";
            SQL += "     , UNITCODE         -- 결과단위코드  \r\n";
            SQL += "     , DATATYPE         -- Data Type  \r\n";
            SQL += "     , DATALENGTH       -- Data Length  \r\n";
            SQL += "     , RESULTIN         -- 결과입력 유무(1:결과 입력 안함, 0:결과 입력 함)  \r\n";
            SQL += "     , PANICFROM        -- Panic Value(From)  \r\n";
            SQL += "     , PANICTO          -- Panic Value(To)  \r\n";
            SQL += "     , DELTAM           -- Delta Value(-)  \r\n";
            SQL += "     , DELTAP           -- Delta value(+)  \r\n";
            SQL += "     , DDDPRDRP         --DD, DP, RD, RP  \r\n";
            SQL += "     , BCODENAME        --Barcode 출력명  \r\n";
            SQL += "     , BCODEPRINT       --Barcode 인쇄장수  \r\n";
            SQL += "     , KEYPAD           --KeyPad Position(Diff용)  \r\n";
            SQL += "     , SERIES           --연속검사(1.연속검사 0.연속검사아님)  \r\n";
            SQL += "     , PENDING          --Pending Checking 여부(1:Check, 0:Check 안함)  \r\n";
            SQL += "     , ENDDATE          --사용종료일(삭제일자)  \r\n";
            SQL += "     , MODIFYDATE       --변경일자  \r\n";
            SQL += "     , MODIFYID         --변경자사번  \r\n";
            SQL += "     , SUB              --SUB코드여부(1:Sub, 0:Mother) 화면에는 선택숨김  \r\n";
            SQL += "     , SUCODE           --수가코드  \r\n";
            SQL += "     , SUBUN            --수가코드 분류  \r\n";
            SQL += "     , MOTHER           --모코드를 서브코드로 가진 것을 표시(1:존재, 0:없음)  \r\n";
            SQL += "     , HELPCODE         --Help코드 존여여부(1.존재 0.없슴)  \r\n";
            SQL += "     , SLIPNO           --오더SLIP의 Slip종류(0010-0050)  \r\n";
            SQL += "     , TONGBUN          --연보 통계용 분류코드(C,F,H,...)  \r\n";
            SQL += "     , TONGSEQNO        --연보 통계시 분류별 일련번호(우선순위)  \r\n";
            SQL += "     , GBTAX --사용안함.  \r\n";
            SQL += "     , GBTAT --조직검사 TAT 소요시간  \r\n";
            SQL += "     , PIECE --바코드 개별 발행(1:묶음 발행 않함)  \r\n";
            SQL += "     , GBBASE --정상인참고치( * )  \r\n";
            SQL += "     , BCODE --표준코드  \r\n";
            SQL += "     , WAMT --외부단가  \r\n";
            SQL += "     , WGB --외부의뢰(1.보험,2.비보험,3.개별)  \r\n";
            SQL += "     , WHCODE --외부항목코드  \r\n";
            SQL += "     , VITEKCODE --vutek 항생제 약어  \r\n";
            SQL += "     , CVMIN --critical Value min 값  \r\n";
            SQL += "     , CVMAX --critical Value Max 값  \r\n";
            SQL += "     , CVVALUE --critical Value (Negative, Positive, Reactive)  \r\n";
            SQL += "     , TONGDISP --통계제외(Y)  \r\n";
            SQL += "     , GBTLA --TLA 여부  \r\n";
            SQL += "     , GBTLASORT --TLA 순서  \r\n";

            //SQL += "     , ACODE                    \r\n";
            SQL += "     , ACODE_NEW                \r\n";

            SQL += "     , INPS         --TLA 순서  \r\n";
            SQL += "     , INPT_DT      --TLA 순서  \r\n";
            SQL += "     , UPPS         --TLA 순서  \r\n";
            SQL += "     , UPDT         --TLA 순서  \r\n";


            SQL += "   )                                                                      \r\n";
            SQL += "   VALUES                                                                 \r\n";
            SQL += "   (                                                                      \r\n";
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.MASTERCODE].ToString(), false);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.EXAMNAME].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.EXAMFNAME].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.EXAMYNAME].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.WSCODE1].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.WSCODE1POS].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.WSCODE2].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.WSCODE2POS].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.WSCODE3].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.WSCODE3POS].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.WSCODE4].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.WSCODE4POS].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.WSCODE5].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.WSCODE5POS].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.TURNTIME1].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.TURNTIME2].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.EXAMWEEK].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.TUBECODE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.SPECCODE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.VOLUMECODE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.EQUCODE1].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.EQUCODE2].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.EQUCODE3].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.EQUCODE4].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.UNITCODE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.DATATYPE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.DATALENGTH].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.RESULTIN].ToString(), true);

            string PANICFROM = string.Format ("{0:0.###}", dr[(int)enmSel_EXAM_MASTER.PANICFROM].ToString());
            SQL += " " + ComFunc.covSqlstr(PANICFROM, true);

            string PANICTO = string.Format("{0:0.###}", dr[(int)enmSel_EXAM_MASTER.PANICTO].ToString());
            SQL += " " + ComFunc.covSqlstr(PANICTO, true);

            string DELTAM = string.Format("{0:0.###}", dr[(int)enmSel_EXAM_MASTER.DELTAM].ToString());
            SQL += " " + ComFunc.covSqlstr(DELTAM, true);

            string DELTAP = string.Format("{0:0.###}", dr[(int)enmSel_EXAM_MASTER.DELTAP].ToString());
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.DELTAP].ToString(), true);

            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.DDDPRDRP].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.BCODENAME].ToString(), true);

            string BCODEPRINT = string.Format( "{0:0}", dr[(int)enmSel_EXAM_MASTER.BCODEPRINT].ToString());
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.BCODEPRINT].ToString(), true);

            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.KEYPAD].ToString(), true);

            string SERIES = string.Format("{0:0}", dr[(int)enmSel_EXAM_MASTER.SERIES].ToString());
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.SERIES].ToString(), true);

            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.PENDING].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.ENDDATE].ToString(), true);
            SQL += " , SYSDATE                                              \r\n"; // MODIFY 일자

            SQL += " " + ComFunc.covSqlstr(clsType.User.IdNumber, true);

            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.SUB].ToString()       , true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.SUCODE].ToString()    , true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.SUBUN].ToString()     , true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.MOTHER].ToString()    , true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.HELPCODE].ToString()  , true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.SLIPNO].ToString()    , true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.TONGBUN].ToString()   , true);

            string TONGSEQNO = string.Format("{0:0}", dr[(int)enmSel_EXAM_MASTER.TONGSEQNO].ToString());
            SQL += " " + ComFunc.covSqlstr(TONGSEQNO, true);

            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.GBTAX].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.GBTAT].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.PIECE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.GBBASE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.BCODE].ToString(), true);

            string WAMT = string.Format("{0:0}", dr[(int)enmSel_EXAM_MASTER.WAMT].ToString());
            SQL += " " + ComFunc.covSqlstr(WAMT, true);

            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.WGB].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.WHCODE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.VITEKCODE].ToString(), true);

            string CVMIN = string.Format("{0:0.###}", dr[(int)enmSel_EXAM_MASTER.CVMIN].ToString());
            SQL += " " + ComFunc.covSqlstr(CVMIN, true);

            string CVMAX = string.Format("{0:0.###}", dr[(int)enmSel_EXAM_MASTER.CVMAX].ToString());
            SQL += " " + ComFunc.covSqlstr(CVMAX, true);
            
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.CVVALUE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.TONGDISP].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.GBTLA].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.GBTLASORT].ToString(), true);

            SQL += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.ACODE].ToString(), true);

            SQL += " " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += " , SYSDATE      \r\n";
            SQL += " " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += " , SYSDATE      \r\n";


            SQL += "   )                                                                     \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_EXAM_HISMSTSUB(PsmhDb pDbCon, DataRow dr, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "EXAM_HISMSTSUB                            \r\n";
            SQL += "   (                                                                        \r\n";
            SQL += "       JOBDATE      -- 변경일자및시각(SYSTEM Time)                          \r\n";
            SQL += "     , JOBGBN       -- 작업구분(D.삭제 M.수정전)  \r\n";
            SQL += "     , JOBSABUN     -- 작업자 사번  \r\n";
            SQL += "     , MASTERCODE   -- 마스터 코드  \r\n";
            SQL += "     , GUBUN        -- 각항목 구분자(아래 참고사항 확인)  \r\n";
            SQL += "     , SORT         -- SORT번호(001-999)  \r\n";
            SQL += "     , NORMAL       -- 각항목 코드,Comment  \r\n";
            SQL += "     , SEX          -- 성별(M:남, F:여)  \r\n";
            SQL += "     , AGEFROM      -- 나이(From)  \r\n";
            SQL += "     , AGETO        -- 나이(To)  \r\n";
            SQL += "     , REFVALFROM   -- Reference Value(From)  \r\n";
            SQL += "     , REFVALTO     -- Reference Value(To)  \r\n";
            //2018-12-10 안정수 추가
            SQL += "     , EXPIREDATE   -- 유효기간 Date  \r\n";
            SQL += "   )                                                                      \r\n";
            SQL += "   VALUES                                                                 \r\n";
            SQL += "   (                                                                      \r\n";
            SQL  += " SYSDATE                                                                 \r\n";
            SQL  += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMSTSUB.JOBGBN].ToString(), true);
            SQL  += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMSTSUB.JOBSABUN].ToString(), true);
            SQL  += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMSTSUB.MASTERCODE].ToString(), true);
            SQL  += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMSTSUB.GUBUN].ToString(), true);
            SQL  += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMSTSUB.SORT].ToString(), true);
            SQL  += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMSTSUB.NORMAL].ToString().Replace("'", "`"), true);
            SQL  += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMSTSUB.SEX].ToString(), true);
            SQL  += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMSTSUB.AGEFROM].ToString(), true);
            SQL  += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMSTSUB.AGETO].ToString(), true);
            SQL  += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMSTSUB.REFVALFROM].ToString(), true);
            SQL  += " " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HISMSTSUB.REFVALTO].ToString(), true);
            //2018-12-10 안정수 추가 
            SQL += ", '" + VB.Left(dr[(int)enmSel_EXAM_HISMSTSUB.EXPIREDATE].ToString(), 10) + "' ";
            SQL += "   )                                                                     \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
              
        public string ins_EXAM_SPECODE(PsmhDb pDbCon, List<string> dr, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "EXAM_SPECODE                            \r\n";
            SQL += "   (                                                                      \r\n";
            SQL += "       GUBUN    -- 각 기초코드의 구분자                                   \r\n";
            SQL += "     , CODE     -- 각항목의 코드,장비코드                                 \r\n";
            SQL += "     , NAME     -- 코드내용,장비에서 나오는 코드                          \r\n";
            SQL += "     , YNAME    -- 내용의약어,검사실에서 사용하는 코드                    \r\n";
            SQL += "     , WSGROUP  -- Work Station의 Group 표시, 검체의 색상 표시            \r\n";
            SQL += "     , WSEQU    -- Work Station의 검사장비 표시(aaa,bbb,ccc식으로 저장)   \r\n";
            SQL += "     , SORT     -- SORT 우선순위                                          \r\n";
            SQL += "     , DELDATE  -- 삭제일자                                               \r\n";

            SQL += "     , INPS     -- 입력자                                               \r\n";
            SQL += "     , INPT_DT  -- 입력일시                                               \r\n";
            SQL += "     , UPPS     -- 수정자                                               \r\n";
            SQL += "     , UPDT     -- 수정일시                                               \r\n";

            SQL += "   )                                                                      \r\n";
            SQL += "   VALUES                                                                 \r\n";
            SQL += "   (                                                                      \r\n";
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_SPECODE_ALL.GUBUN].ToString()  , false);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_SPECODE_ALL.CODE].ToString()   , true);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_SPECODE_ALL.NAME].ToString()   , true);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_SPECODE_ALL.YNAME].ToString()  , true);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_SPECODE_ALL.WSGROUP].ToString(), true);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_SPECODE_ALL.WSEQU].ToString()  , true);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_SPECODE_ALL.SORT].ToString()   , true);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_SPECODE_ALL.DELDATE].ToString(), true);

            SQL += ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "   ,SYSDATE                                                             \r\n";
            SQL += ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "   ,SYSDATE                                                             \r\n";


            SQL += "   )                                                                     \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string del_EXAM_SPECODE(PsmhDb pDbCon, string strGUBUN, string strCODE, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_MED + "EXAM_SPECODE  \r\n";
            SQL += "  WHERE 1=1                                     \r\n";
            SQL += "    AND GUBUN = " + ComFunc.covSqlstr(strGUBUN, false);

            if (string.IsNullOrEmpty(strCODE) == false)
            {
                SQL += "    AND CODE = " + ComFunc.covSqlstr(strCODE, false);
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_EXAM_RACK_NO(PsmhDb pDbCon, List<string>dr, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "EXAM_RACK_NO \r\n";
            SQL += "   (                                        \r\n";
            SQL += "    GUBUN                                   \r\n";
            SQL += "   ,CODE                                    \r\n";
            SQL += "   ,SSEQNO                                  \r\n";
            SQL += "   ,ESEQNO                                  \r\n";
            SQL += "   )                                        \r\n";
            SQL += "   VALUES                                   \r\n";
            SQL += "   (                                        \r\n";
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_RACK_NO.GUBUN].ToString()  , false);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_RACK_NO.CODE].ToString()   , true);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_RACK_NO.SSEQNO].ToString() , true);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_RACK_NO.ESEQNO].ToString() , true);
            SQL += "   )                                        \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string del_EXAM_RACK_NO(PsmhDb pDbCon, string strGUBUN, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_MED + "EXAM_RACK_NO  \r\n";
            SQL += "  WHERE 1=1                                     \r\n";
            SQL += "    AND GUBUN = " + ComFunc.covSqlstr(strGUBUN, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string del_EXAM_RACK_CODE(PsmhDb pDbCon, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_MED + "EXAM_RACK_CODE \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_EXAM_RACK_CODE(PsmhDb pDbCon, string strCODE, string strNAME, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "EXAM_RACK_CODE \r\n";
            SQL += "   (                                        \r\n";
            SQL += "    CODE                                    \r\n";
            SQL += "   ,NAME                                    \r\n";
            SQL += "   )                                        \r\n";
            SQL += "   VALUES                                   \r\n";
            SQL += "   (                                        \r\n";
            SQL += ComFunc.covSqlstr(strCODE, false);
            SQL += ComFunc.covSqlstr(strNAME, true);
            SQL += "   )                                        \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
       
        public DataSet sel_EXAM_RACK_CODE(PsmhDb pDbCon)
        {
            DataSet ds = null;

            SQL = "";
            SQL = SQL + " SELECT                            \r\n ";
            SQL = SQL + "       '' AS CHK                   \r\n ";
            SQL = SQL + "     , CODE                        \r\n ";            
            SQL = SQL + "     , NAME                        \r\n ";
            SQL = SQL + "     , ROWID                       \r\n ";
            SQL = SQL + "  FROM KOSMOS_OCS.EXAM_RACK_CODE   \r\n ";
            SQL = SQL + " ORDER BY CODE, NAME               \r\n ";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

            return ds;
        }

        /// <summary>EXAM_SPECODE 쿼리</summary>
        /// <returns></returns>
        public DataTable sel_EXAM_SPECODE_Code(PsmhDb pDbCon)
        {
            DataTable dt = null;


            SQL = "";
            SQL += " SELECT  CODE            -- 코드                \r\n";
            SQL += "       , NAME            -- 명칭                \r\n";
            SQL += "       , YNAME           -- 약어                \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_SPECODE       \r\n";
            SQL += "  WHERE 1 = 1                                   \r\n";
            SQL += "   AND GUBUN IN('14'     -- 검체코드            \r\n";
            SQL += "               ,'15'     -- 용기코드            \r\n";
            SQL += "               ,'12'     -- WS코드              \r\n";
            SQL += "               ,'20'     -- 결과단위            \r\n";
            SQL += "               )                                \r\n";
            SQL += "   AND DELDATE IS NULL                          \r\n";
            SQL += " ORDER BY GUBUN, SORT, CODE                     \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        /// <summary>sel_EXAM_SPECODE_MIC</summary>
        /// <param name="strGubun"></param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public DataSet sel_EXAM_SPECODE_CODE_HELP(PsmhDb pDbCon, enmEXAM_SPECODE_GUBUN pEnmExamSpecodeGubun, string strCode = "", string strSlipNO = "")
        {
            DataSet ds = null;
            string strSlip = "EXAM";

            if (pEnmExamSpecodeGubun == enmEXAM_SPECODE_GUBUN.SLIP_SPECIMAN)
            {
                strSlip = "SLIP";
            }
            else if (pEnmExamSpecodeGubun == enmEXAM_SPECODE_GUBUN.OCS_SUBCODE)
            {
                strSlip = "SUBCODE";
            }
            else if (pEnmExamSpecodeGubun == enmEXAM_SPECODE_GUBUN.SUB)
            {
                strSlip = "EXAM_MASTER";
            }

            SQL = "";
            SQL += " SELECT CODE                                \r\n";
            SQL += "      , NAME                                \r\n";
            SQL += "      , YNAME                               \r\n";
            SQL += "      , '' WS                               \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_SPECODE   \r\n";
            SQL += " WHERE 1=1                                  \r\n";

            SQL += "   AND 'EXAM' =" + ComFunc.covSqlstr(strSlip, false);

            if (pEnmExamSpecodeGubun == enmEXAM_SPECODE_GUBUN.MICRO)
            {
                SQL += "   AND GUBUN  ='18'                \r\n";
                SQL += "   AND CODE   >'ZZ000'             \r\n";

            }
            else
            {
                SQL += "   AND GUBUN  = " + ComFunc.covSqlstr(Convert.ToString((int)pEnmExamSpecodeGubun), false);

                if (string.IsNullOrEmpty(strCode) == false)
                {
                    SQL += "   AND CODE " + ComFunc.covSqlstr(strCode, false);
                }
            }

            SQL += " UNION ALL                                  \r\n";
            SQL += " SELECT SPECCODE AS CODE                    \r\n";
            SQL += "      , SPECNAME AS NAME                    \r\n";
            SQL += "      , ''       AS YNAME                   \r\n";
            SQL += "      , ''       AS WS                      \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "OCS_OSPECIMAN  \r\n";
            SQL += " WHERE 1=1                       \r\n";
            SQL += "   AND 'SLIP' =" + ComFunc.covSqlstr(strSlip, false);
            SQL += "   AND SLIPNO =" + ComFunc.covSqlstr(strSlipNO, false);

            SQL += " UNION ALL                                  \r\n";
            SQL += " SELECT ITEMCD   AS CODE                    \r\n";
            SQL += "      , SUBNAME  AS NAME                    \r\n";
            SQL += "      , TO_CHAR(SEQNO)    AS YNAME                   \r\n";
            SQL += "      , ''       AS WS                      \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "OCS_SUBCODE    \r\n";
            SQL += " WHERE 1=1                       \r\n";
            SQL += "   AND 'SUBCODE' =" + ComFunc.covSqlstr(strSlip, false);
            SQL += "   AND ORDERCODE =" + ComFunc.covSqlstr(strSlipNO, false);

            SQL += " UNION ALL                                  \r\n";
            SQL += " SELECT MASTERCODE   AS CODE                \r\n";
            SQL += "      , EXAMNAME     AS NAME                \r\n";
            SQL += "      , EXAMFNAME    AS YNAME               \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_SPECMST_NM('12', WSCODE1,'N')       AS WS                      \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_MASTER    \r\n";
            SQL += " WHERE 1=1                                  \r\n";
            SQL += "   AND 'EXAM_MASTER' =" + ComFunc.covSqlstr(strSlip, false);
            SQL += "   AND (ENDDATE IS NULL OR ENDDATE > TRUNC(SYSDATE)) \r\n";
            SQL += "   AND WSCODE1 IS NOT NULL          ";




            if (strSlip == "SUBCODE")
            {
                SQL += " ORDER BY YNAME                   \r\n";
            }
            else if (strSlip == "EXAM_MASTER")
            {
                SQL += " ORDER BY WS, CODE                   \r\n";
            }
            else if (strSlip == "EXAM")
            {
                SQL += " ORDER BY CODE                   \r\n";
            }
            else
            {
                SQL += " ORDER BY NAME                   \r\n";
            }


            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

            return ds;
        }

        public DataSet sel_EXAM_SPECODE_OCS_OSPECIMAN(PsmhDb pDbCon, string strGubun, string strSlipNo)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                         \r\n";
            SQL += "      '' CHK                                    \r\n";
            SQL += "      ,  CODE                                   \r\n";
            SQL += "      ,  NAME                                   \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_SPECODE       \r\n";
            SQL += " WHERE 1=1                                      \r\n";
            SQL += "   AND GUBUN = " + ComFunc.covSqlstr(strGubun, false);
            SQL += "   AND DELDATE IS NULL                                  \r\n";
            SQL += "   AND CODE NOT IN (                                    \r\n";
            SQL += "                    SELECT TRIM(SPECCODE)               \r\n";
            SQL += "                      FROM KOSMOS_OCS.OCS_OSPECIMAN     \r\n";
            SQL += "                     WHERE 1=1                          \r\n";
            SQL += "                      AND SLIPNO = " + ComFunc.covSqlstr(strSlipNo, false);
            SQL += "                      )                                 \r\n";

            SQL += " ORDER BY  CODE                                 \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

            return ds;
        }

        public string ins_EXAM_MATCH(PsmhDb pDbCon, DataRow dr, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "EXAM_MATCH  \r\n";
            SQL += "   (                                          \r\n";
            SQL += "     SUCODE                                   \r\n";
            SQL += "   , MASTERCODE                               \r\n";
            SQL += "   , BUN                                      \r\n";
            SQL += "   , OPDJEPSU                                 \r\n";
            SQL += "   )                                          \r\n";
            SQL += "   VALUES                                     \r\n";
            SQL += "   (                                          \r\n";
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MATCH.SUCODE].ToString(), false);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MATCH.MASTERCODE].ToString(), true);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MATCH.BUN].ToString(), true);
            SQL += "   , 'Y'                                      \r\n";
            SQL += "   )                                          \r\n";
           
            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }


            return SqlErr;
        }

        public string del_EXAM_MATCH(PsmhDb pDbCon, string strMASTERCODE, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE " + ComNum.DB_MED + "EXAM_MATCH  \r\n";
            SQL += "  WHERE 1=1                              \r\n";
            SQL += "    AND MASTERCODE = " + ComFunc.covSqlstr(strMASTERCODE, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public DataSet sel_EXAM_MATCH(PsmhDb pDbCon, string strMASTERCODE)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT  A.SUCODE           AS SUCODE                \r\n";
            SQL += "       , C.SUNAMEK          AS SUNAMEK               \r\n";
            SQL += "       , A.BUN              AS BUN                   \r\n";
            SQL += "       , D.NAME             AS BUN_NAME              \r\n";
            SQL += "       , A.MASTERCODE       AS MASTERCODE            \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_MATCH A                      \r\n";
            SQL += "      , KOSMOS_PMPA.BAS_SUT   B                      \r\n";
            SQL += "      , KOSMOS_PMPA.BAS_SUN   C                      \r\n";
            SQL += "      , KOSMOS_PMPA.BAS_BCODE D                      \r\n";
            SQL += "  WHERE 1=1                                          \r\n";
            SQL += "    AND A.MASTERCODE = " + ComFunc.covSqlstr(strMASTERCODE, false);
            SQL += "    AND A.SUCODE     = B.SUCODE                      \r\n";
            SQL += "    AND B.SUNEXT     = C.SUNEXT(+)                   \r\n";
            SQL += "    AND D.GUBUN      = 'BAS_수가분류코드'            \r\n";
            SQL += "    AND A.BUN        =  D.CODE                       \r\n";
            SQL += "    AND B.SUDATE     =                               \r\n";
            SQL += " 				   (                                 \r\n";
            SQL += " 	   				  SELECT MAX(SUDATE)             \r\n";
            SQL += " 	                    FROM KOSMOS_PMPA.BAS_SUT E   \r\n";
            SQL += " 	                   WHERE E.SUCODE = A.SUCODE     \r\n";
            SQL += "                    )                                \r\n";
            SQL += "  ORDER BY A.MASTERCODE                              \r\n";

            string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return ds;
        }

        public DataSet sel_EXAM_MASTER_SUB(PsmhDb pDbCon, string strMASTERCODE)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                                                          \r\n";
            SQL += "       MASTERCODE   -- 마스터 코드                                                                                \r\n";
            SQL += "     , GUBUN        -- 각항목 구분자 17:채취 Comment 18:HELP 31:SUB CODE 41:Ref Value 51:참고 Comment 61.검사정보 \r\n";
            SQL += "     , SORT         -- SORT번호(001-999)                                                                          \r\n";
            SQL += "     , NORMAL       -- 각항목 코드,Comment                                                                        \r\n";
            SQL += "     , SEX          -- 성별(M:남, F:여)                                                                           \r\n";
            SQL += "     , AGEFROM      -- 나이(From)                                                                                 \r\n";
            SQL += "     , AGETO        -- 나이(To)                                                                                   \r\n";
            SQL += "     , REFVALFROM   -- Reference Value(From)                                                                      \r\n";
            SQL += "     , REFVALTO     -- Reference Value(To)                                                                        \r\n";
            SQL += "     , CASE                                                                                                       \r\n";
            SQL += "            WHEN GUBUN = '31' THEN KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(NORMAL)                                     \r\n";
            SQL += "            WHEN GUBUN = '18' THEN KOSMOS_OCS.FC_EXAM_SPECMST_NM('18',NORMAL,'N')                                 \r\n";
            SQL += "            WHEN GUBUN = '17' THEN KOSMOS_OCS.FC_EXAM_SPECMST_NM('17',NORMAL,'N')                                 \r\n";
            SQL += "            ELSE ''                                                                                               \r\n";
            SQL += "         END  AS  NAME                                                                                            \r\n";
            SQL += "     , ROWID        -- ROWID                                                                                      \r\n";
            SQL += "     , EXPIREDATE        -- 유효기간                                                                              \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_MASTER_SUB /** 임상병리 검사마스타 SUB항목*/                                               \r\n";
            SQL += " WHERE 1=1                                                                                                        \r\n";
            SQL += "   AND MASTERCODE =" + ComFunc.covSqlstr(strMASTERCODE, false);
            SQL += " ORDER BY GUBUN, SORT,NORMAL                                                                                      \r\n";

            string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return ds;
        }

        public DataTable sel_EXAM_MASTER(PsmhDb pDbCon, string strMASTERCODE, string strWS = "", string strMASTER_SEARCH = "", string strWC = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                         \r\n";
            SQL += "        MASTERCODE  -- Master Key                                                               \r\n";
            SQL += "      , EXAMNAME    -- 검사명:통상명칭                                                          \r\n";   
            SQL += "      , EXAMFNAME   -- 검사명:Full Name                                                         \r\n";   
            SQL += "      , EXAMYNAME   -- 검사명:연보에 실린 이름                                                  \r\n";
            SQL += "      , WSCODE1     -- Work Station 1                                                           \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_SPECMST_NM('12',WSCODE1,'N')   AS WS1_NM      -- WS1명칭             \r\n";
            SQL += "      , WSCODE1POS  -- 검사 항목 결과 위치                                                      \r\n";
            SQL += "      , WSCODE2     -- Work Station 2                                                           \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_SPECMST_NM('12',WSCODE2,'N')   AS WS2_NM      -- WS2명칭             \r\n";
            SQL += "      , WSCODE2POS  -- 검사 항목 결과 위치                                                      \r\n";
            SQL += "      , WSCODE3     -- Work Station 3                                                           \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_SPECMST_NM('12',WSCODE3,'N')   AS WS3_NM      -- WS3명칭             \r\n";
            SQL += "      , WSCODE3POS  -- 검사 항목 결과 위치                                                      \r\n";
            SQL += "      , WSCODE4     -- Work Station 4                                                           \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_SPECMST_NM('12',WSCODE4,'N')   AS WS4_NM      -- WS4명칭             \r\n";
            SQL += "      , WSCODE4POS  -- 검사 항목 결과 위치                                                      \r\n";
            SQL += "      , WSCODE5     -- Work Station 5                                                           \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_SPECMST_NM('12',WSCODE5,'N')   AS WS5_NM      -- WS5명칭             \r\n";
            SQL += "      , WSCODE5POS  -- 검사 항목 결과 위치                                                      \r\n";
            SQL += "      , TURNTIME1   -- Turn Around Time(일)                                                     \r\n";
            SQL += "      , TURNTIME2   -- Turn Around Time(분)                                                     \r\n";
            SQL += "      , EXAMWEEK    -- 검사요일(일월화수목금토) (Y:검사, N:검사안함)                            \r\n";
            SQL += "      , TUBECODE    -- 용기종류코드                                                             \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_SPECMST_NM('15',TUBECODE,'N')   AS TUBE_NM    -- 용기명칭            \r\n";
            SQL += "      , SPECCODE    -- 검체종류코드                                                             \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',SPECCODE,'N')   AS SPEC_NM    -- 검체명칭            \r\n";
            SQL += "      , VOLUMECODE  -- 채혈량코드                                                               \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_SPECMST_NM('16',VOLUMECODE,'N') AS VOLUME_NM  -- 채혈량명칭          \r\n";

            SQL += "      , EQUCODE1    -- 장비코드                                                                 \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_SPECMST_NM('13',EQUCODE1,'N') AS EQU1_NM      -- 장비1명칭           \r\n";
            SQL += "      , EQUCODE2    -- 장비코드                                                                 \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_SPECMST_NM('13',EQUCODE2,'N') AS EQU2_NM      -- 장비2명칭           \r\n";
            SQL += "      , EQUCODE3    -- 장비코드                                                                 \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_SPECMST_NM('13',EQUCODE3,'N') AS EQU3_NM      -- 장비3명칭           \r\n";
            SQL += "      , EQUCODE4    -- 장비코드                                                                 \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_SPECMST_NM('13',EQUCODE4,'N') AS EQU4_NM      -- 장비4명칭           \r\n";

            SQL += "      , UNITCODE    -- 결과단위코드                                                             \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_SPECMST_NM('20',UNITCODE,'N') AS UNIT_NM  -- 단위명칭                \r\n";
            SQL += "      , DECODE(NVL(DATATYPE,'NULL'),'NULL',''                                                   \r\n";
            SQL += "                                          ,DATATYPE ||'.'|| KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_DATATYPE',DATATYPE)) AS DATATYPE   -- Data Type                                                                \r\n";
            SQL += "      , DATALENGTH  -- Data Length                                                              \r\n";
            SQL += "      , RESULTIN    -- 결과입력 유무(1:결과 입력 안함, 0:결과 입력 함)                          \r\n";
            SQL += "      , PANICFROM   -- Panic Value(From)                                                        \r\n";
            SQL += "      , PANICTO     -- Panic Value(To)                                                          \r\n";
            SQL += "      , DELTAM      -- Delta Value(-)                                                           \r\n";
            SQL += "      , DELTAP      -- Delta value(+)                                                           \r\n";
            SQL += "      , DDDPRDRP    -- DD, DP, RD, RP                                                           \r\n";
            SQL += "      , BCODENAME   -- Barcode 출력명                                                           \r\n";
            SQL += "      , BCODEPRINT  -- Barcode 인쇄장수                                                         \r\n";
            SQL += "      , KEYPAD      -- KeyPad Position(Diff용)                                                  \r\n";
            SQL += "      , SERIES      -- 연속검사(1.연속검사 0.연속검사아님)                                      \r\n";
            SQL += "      , PENDING     -- Pending Checking 여부(1:Check, 0:Check 안함)                             \r\n";
            SQL += "      , to_char(ENDDATE,'yyyy-mm-dd') as ENDDATE      -- 사용종료일(삭제일자)                                                     \r\n";
            SQL += "      , MODIFYDATE  -- 변경일자                                                                 \r\n";
            SQL += "      , MODIFYID    -- 변경자사번                                                               \r\n";
            SQL += "      , SUB         -- SUB코드여부(1:Sub, 0:Mother) 화면에는 선택숨김                           \r\n";
            SQL += "      , SUCODE      -- 수가코드                                                                 \r\n";
            SQL += "      , SUBUN       -- 수가코드 분류                                                            \r\n";
            SQL += "      , MOTHER      -- 모코드를 서브코드로 가진 것을 표시(1:존재, 0:없음)                       \r\n";
            SQL += "      , HELPCODE    -- Help코드 존여여부(1.존재 0.없슴)                                         \r\n";
            SQL += "      , SLIPNO      -- 오더SLIP의 Slip종류(0010-0050)                                           \r\n";
            SQL += "      , TONGBUN     -- 연보 통계용 분류코드(C,F,H,...)                                          \r\n";
            SQL += "      , TONGSEQNO   -- 연보 통계시 분류별 일련번호(우선순위)                                    \r\n";
            SQL += "      , GBTAX       -- 사용안함.                                                                \r\n";
            SQL += "      , GBTAT       -- 조직검사 TAT 소요시간                                                    \r\n";
            SQL += "      , PIECE       -- 바코드 개별 발행(1:묶음 발행 않함)                                       \r\n";
            SQL += "      , GBBASE      -- 정상인참고치( * )                                                        \r\n";
            SQL += "      , BCODE       -- 표준코드                                                                 \r\n";
            SQL += "      , WAMT        -- 외부단가                                                                 \r\n";
            SQL += "      , WGB         -- 외부의뢰(1.보험,2.비보험,3.개별)                                         \r\n";
            SQL += "      , WHCODE      -- 외부항목코드                                                             \r\n";
            SQL += "      , VITEKCODE   -- vutek 항생제 약어                                                        \r\n";
            SQL += "      , CVMIN       -- critical Value min 값                                                    \r\n";
            SQL += "      , CVMAX       -- critical Value Max 값                                                    \r\n";
            SQL += "      , CVVALUE     -- critical Value (Negative, Positive, Reactive)                            \r\n";
            SQL += "      , TONGDISP    -- 통계제외(Y)                                                              \r\n";
            SQL += "      , GBTLA       -- TLA 여부                                                                 \r\n";
            SQL += "      , GBTLASORT   -- TLA 순서                                                                 \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_MATCH_INFO(MASTERCODE) AS ORDERINFO   -- 처방에 대한 종합적인 정보   \r\n";
            //SQL += "      , ACODE                                                                               \r\n";
            SQL += "      , ACODE_NEW                                                                               \r\n";
            //SQL += "      , (SELECT PART FROM KOSMOS_OCS.EXAM_AUTOSEND_CODE A  WHERE A.CODE = ACODE)  AS PART                  \r\n";
            SQL += "      , (SELECT NEWPART FROM KOSMOS_OCS.EXAM_AUTOSEND_CODE A  WHERE A.NEWCODE = ACODE_NEW)  AS PART \r\n";
            //SQL += "      , (CASE WHEN NVL(TRIM(ACODE),'^%') != '^%' THEN ( SELECT CODE || '.' || NAMEK || '(' || NAMEE || ')'  FROM KOSMOS_OCS.EXAM_AUTOSEND_CODE A  WHERE A.CODE = ACODE) END )  AS ACODE_NM                  \r\n";
            SQL += "      , (CASE WHEN NVL(TRIM(ACODE_NEW),'^%') != '^%' THEN ( SELECT NEWCODE || '.' || NAMEK || '(' || NAMEE || ')'  FROM KOSMOS_OCS.EXAM_AUTOSEND_CODE A  WHERE A.NEWCODE = ACODE_NEW) END )  AS ACODE_NM                  \r\n";
            SQL += "      , ROWID       -- ROWID                                                                    \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_MASTER /** 검사실 접수 Master*/                                          \r\n";
            SQL += " WHERE 1=1                                                                                      \r\n";

            if (string.IsNullOrEmpty(strWS) == true && string.IsNullOrEmpty(strMASTER_SEARCH) == true)
            {
                SQL += "   AND MASTERCODE = " + ComFunc.covSqlstr(strMASTERCODE, false);
            }
            else if (string.IsNullOrEmpty(strMASTER_SEARCH) == true && string.IsNullOrEmpty(strWS) == false)
            {
                SQL += "   AND WSCODE1    = " + ComFunc.covSqlstr(strWS.Substring(0, 3), false);
            }
            else if (string.IsNullOrEmpty(strMASTER_SEARCH) == false)
            {
                SQL += "   AND (                                                                                   \r\n";
                SQL += "           MASTERCODE LIKE '%" + strMASTER_SEARCH + "%'                                    \r\n";
                SQL += "        OR EXAMNAME   LIKE '%" + strMASTER_SEARCH + "%'                                    \r\n";
                SQL += "        OR BCODENAME  LIKE '%" + strMASTER_SEARCH + "%'                                    \r\n";
                SQL += "        )                                                                                  \r\n";
            }
            else if (string.IsNullOrEmpty(strWC) == false)
            {
                SQL += "   AND TONGBUN    = " + ComFunc.covSqlstr(strWC, false);
            }
            else
            {
                SQL += "   AND MASTERCODE = " + ComFunc.covSqlstr(strMASTERCODE, false);
            }

            SQL += " ORDER BY WSCODE1, MASTERCODE                                              \r\n";


            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        /// <summary>gArrEXAM_RESULTC 변수 초기화</summary>
        public void clearArrEXAM_RESULTC()
        {
            Type enmSPEC_RESULTC = typeof(enmSPEC_RESULTC);
            gArrEXAM_RESULTC = new string[Enum.GetNames(enmSPEC_RESULTC).Length];
        }
        
        /// <summary></summary>
        /// <param name="eType"></param>
        /// <returns></returns>
        public DataSet sel_EXAM_MASTER_SEARCH(PsmhDb pDbCon, frmComSupLbExHELP03.enmMasterType eType)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                         \r\n";
            SQL += " 	  MASTERCODE                                                \r\n";
            SQL += " 	, EXAMNAME                                                  \r\n";
            SQL += " 	, EXAMFNAME                                                 \r\n";
            SQL += " 	, EXAMYNAME                                                 \r\n";
            SQL += " 	, WSCODE1                                                   \r\n";
            SQL += " 	, KOSMOS_OCS.FC_EXAM_SPECMST_NM('12',WSCODE1,'N') WS_NAME   \r\n";
            SQL += " 	, BCODENAME                                                 \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_MASTER A                                \r\n";
            SQL += "  WHERE 1=1                                                     \r\n";

            if (eType == frmComSupLbExHELP03.enmMasterType.PTHL)
            {

                SQL += "     AND (                                                  \r\n";
                SQL += "	 	   (WSCODE1  =  '711' )                             \r\n";
                SQL += "	 	OR (WSCODE1 ='801' AND MASTERCODE LIKE 'O-%')       \r\n";
                SQL += "	 	OR MASTERCODE LIKE 'Y%'                             \r\n";
                SQL += "	 	)                                                   \r\n";
            }

            SQL += "    AND ENDDATE IS NULL                                         \r\n";
            SQL += "  ORDER BY  WSCODE1  , MASTERCODE                               \r\n";

            string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return ds;
        }

        /// <summary>마스터 장비 갖고 오기</summary>
        /// <param name="strWSCODE"></param>
        /// <param name="isEQU"></param>
        /// <param name="strEQU"></param>
        /// <returns></returns>
        public DataTable sel_EXAM_MASTER_EQU(PsmhDb pDbCon, string strWSCODE, bool isEQU, string strEQU)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "  SELECT                                        \r\n";
            SQL += "  	    MASTERCODE AS CODE                      \r\n";
            SQL += "  	  , EXAMNAME   AS NAME                      \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_MASTER                 \r\n";
            SQL += "   WHERE 1=1                                    \r\n";
            SQL += "     AND SUB = '0'                              \r\n";

            SQL += "     AND                                        \r\n";
            SQL += "         (                                      \r\n";
            SQL += "            WSCODE1 IN (" + strWSCODE + ")      \r\n";
            SQL += "         OR WSCODE2 IN (" + strWSCODE + ")      \r\n";
            SQL += "         OR WSCODE3 IN (" + strWSCODE + ")      \r\n";
            SQL += "         OR WSCODE4 IN (" + strWSCODE + ")      \r\n";
            SQL += "         OR WSCODE5 IN (" + strWSCODE + ")      \r\n";
            SQL += "         )                                      \r\n";

            if (isEQU == true)
            {
                SQL += "     AND                                    \r\n";
                SQL += "         (                                  \r\n";
                SQL += "            EQUCODE1 IN (" + strEQU + ")    \r\n";
                SQL += "         OR EQUCODE2 IN (" + strEQU + ")    \r\n";
                SQL += "         OR EQUCODE3 IN (" + strEQU + ")    \r\n";
                SQL += "         OR EQUCODE4 IN (" + strEQU + ")    \r\n";
                SQL += "         )                                  \r\n";
            }

            SQL += "   GROUP BY MASTERCODE, EXAMNAME, WSCODE1, WSCODE2, WSCODE3, WSCODE4, WSCODE5                \r\n";
            //SQL += "   ORDER BY MASTERCODE, EXAMNAME,WSCODE1                \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        /// <summary> 종합 검증 보고서 출력
        /// </summary>
        /// <param name="strRowId"></param>
        /// <returns></returns>
        public DataTable sel_EXAM_VERIFY_Print(PsmhDb pDbCon, string strRowId)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "     SELECT                                                                                                                                              \r\n";
            SQL += "           PANO                                                  AS PANO                                                                                 \r\n";
            SQL += "         , SNAME                                                 AS SNAME                                                                                \r\n";
            SQL += "         , AGE || '/' || SEX                                     AS AGE_SEX                                                                              \r\n";
            SQL += "         , WARD                                                  AS WARD                                                                                 \r\n";
            SQL += "         , KOSMOS_OCS.FC_BAS_CLINICDEPT_DEPTNAMEK(DEPTCODE)      AS DEPT_NAME                                                                            \r\n";
            SQL += "         , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DRCODE)               AS DR_NAME                                                                              \r\n";
            SQL += "         , DISEASE                                               AS DISEASE --임상소견                                                                   \r\n";
            SQL += "         , (                                                                                                                                             \r\n";
            SQL += "               CASE WHEN TRIM(SDATE) = '' THEN ''                                                                                                        \r\n";
            SQL += "                    ELSE '(검사기간: ' || TO_CHAR(SDATE, 'YYYY-MM-DD') || '~' || TO_CHAR(EDATE, 'YYYY-MM-DD') || ')'                                     \r\n";
            SQL += "                    END                                                                                                                                  \r\n";
            SQL += "     	  ) 							                         AS DISDATE -- 검사일자                                                                  \r\n";
            SQL += "         , ITEMS1                                                AS ITEMS1  -- 검증항목                                                                  \r\n";
            SQL += "         , ITEMS2                                                AS ITEMS2  -- 비정상 결과 혹은 유의한 결과를 보이는 항목                                \r\n";
            SQL += "         , DECODE(VERIFY1, 'Y', '●Calibratrion Verification', '○Calibratrion Verification')              AS VERIFY1 --검증방법                         \r\n";
            SQL += "         , DECODE(VERIFY2, 'Y', '●Delta Check Verification' , '○Delta Check Verification')               AS VERIFY2 --검증방법                         \r\n";
            SQL += "         , DECODE(VERIFY3, 'Y', '●Repeat/Recheck'           , '○Repeat/Recheck')                         AS VERIFY3 --검증방법                         \r\n";
            SQL += "         , DECODE(VERIFY4, 'Y', '●Internal Quality Control' , '○Internal Quality Control')               AS VERIFY4 --검증방법                         \r\n";
            SQL += "         , DECODE(VERIFY5, 'Y', '●Panic/Alert Value Verification', '○Panic/Alert Value Verification')    AS VERIFY5 --검증방법                         \r\n";
            SQL += "         , DECODE(NVL(VERIFY6,'*'), '*','○Others ;' , '●Others ; ' ||   VERIFY6)                         AS VERIFY6 --검증방법                         \r\n";
            SQL += "         , COMMENTS                                                                                        AS COMMENTS-- 검증 / 판독 소견(cOMMENTS)      \r\n";
            SQL += "         , RECOMMENDATION                                                                                  AS RECOMMENDATION-- 추천                      \r\n";
            SQL += "         , TO_CHAR(JDATE, 'YYYY') || '년 '                                                                                                               \r\n";
            SQL += "          || TO_CHAR(JDATE, 'MM') || '월 '                                                                                                               \r\n";
            SQL += "          || TO_CHAR(JDATE, 'DD') || '일 '                        AS JDATE        --보고일자                                                             \r\n";
            SQL += "         , '진단검사의학전문의 ' ||                                                                                                                      \r\n";
            SQL += "          (                                                                                                                                              \r\n";
            SQL += "            CASE RESULTSABUN WHEN  9089  THEN '김성철'                                                                                                   \r\n";
            SQL += "                             WHEN  18210 THEN '은상진'                                                                                                   \r\n";
            SQL += "                             WHEN  39874 THEN '양성문'                                                                                                   \r\n";
            SQL += "            ELSE '의사오류'                                                                                                                              \r\n";
            SQL += "             END                                                                                                                                         \r\n";
            SQL += "          )				        		                        AS RDR_NAME                                                                              \r\n";
            SQL += " , '전문의 번호 (' ||                                                                                                                                    \r\n";
            SQL += "   (                                                                                                                                                     \r\n";
            SQL += "     CASE RESULTSABUN WHEN  9089  THEN '301'                                                                                                             \r\n";
            SQL += "                      WHEN  18210 THEN '424'                                                                                                             \r\n";
            SQL += "                      WHEN  39874 THEN '605'                                                                                                             \r\n";
            SQL += "     ELSE '***'                                                                                                                                          \r\n";
            SQL += "      END                                                                                                                                                \r\n";
            SQL += "     ) || ')'                                                    AS RDR_BUNHO                                                                            \r\n";
            SQL += " , '* 본 검사실은 ' || TO_CHAR(JDATE, 'YYYY')                                                                                                            \r\n";
            SQL += "   || '년 대한진단검사의학회 검사실 신임제도 종합검증 분야의 인증을 필하였습니다. 'AS JYYYY                                                              \r\n";
            SQL += "  FROM " + ComNum.DB_MED + "EXAM_VERIFY A                                                                                                                \r\n";
            SQL += " WHERE 1 = 1                                                                                                                                             \r\n";
            SQL += "   AND ROWID = " + ComFunc.covSqlstr(strRowId, false);

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        /// <summary> 검체번호 생성</summary>
        /// <returns></returns>
        public string sel_SpecNO(PsmhDb pDbCon)
        {
            string s = string.Empty;
            string sDate = string.Empty;
            DataTable dt = null;

            SQL = "SELECT  " + ComNum.DB_PMPA + "SEQ_EXAMSPECNO.NEXTVAL SPECNO FROM DUAL";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }


                if (dt != null && dt.Rows.Count > 0)
                {
                    sDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                    //string.Format("{0:00}"                    
                    s = sDate.Substring(2, 2) + sDate.Substring(5, 2) + sDate.Substring(8, 2) + string.Format("{0:0000}", Convert.ToInt32(dt.Rows[0]["SPECNO"].ToString()));
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return s;
            }

            return s;
        }

        /// <summary> 검체번호 생성</summary>
        /// <returns></returns>
        public string sel_SpecNo_HicChul(PsmhDb pDbCon)
        {
            string s = string.Empty;
            string sDate = string.Empty;
            DataTable dt = null;

            SQL = "SELECT  " + ComNum.DB_PMPA + "SEQ_EXAMSPECNO_HICCHUL.NEXTVAL SPECNO FROM DUAL";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }


                if (dt != null && dt.Rows.Count > 0)
                {
                    sDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                    s = sDate.Substring(2, 2) + sDate.Substring(5, 2) + sDate.Substring(8, 2) + dt.Rows[0]["SPECNO"].ToString();
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return s;
            }

            return s;
        }

        /// <summary>PB 출력 대상자</summary>
        /// <param name="strPano"></param>
        /// <param name="strBData"></param>
        /// <returns></returns>
        public DataTable sel_EXAM_ORDER_PB(PsmhDb pDbCon, string strPano, string strBData, string strIO = "", string strDeptCode = "", string strWard = "")
        {
            DataTable dt = null;


            //if (strIO == "입원" || strDeptCode == "ER")
            if (strIO == "입원")
            {
                SQL = "";
                SQL += " SELECT                                             \r\n";
                SQL += "      B.PANO                                        \r\n";
                SQL += " 	, B.SNAME                                       \r\n";
                SQL += " 	, B.JUMIN1 || '-' || JUMIN2 JUMIN               \r\n";
                SQL += "         , P.DEPTCODE                               \r\n";
                SQL += " 	, P.DRCODE                                      \r\n";
                //SQL += " 	, (SELECT WARD FROM KOSMOS_OCS.EXAM_ORDER WHERE PANO = P.PANO AND BDATE = P.BDATE AND WARD IS NOT NULL AND MASTERCODE = 'HR10' AND CANCEL IS NULL) AS WARDCODE                                    \r\n";
                //SQL += " 	, (SELECT ROOM FROM KOSMOS_OCS.EXAM_ORDER WHERE PANO = P.PANO AND BDATE = P.BDATE AND WARD IS NOT NULL AND MASTERCODE = 'HR10' AND CANCEL IS NULL) AS ROOMCODE                                    \r\n";
                SQL += " 	, C.WARDCODE                                    \r\n";
                SQL += " 	, C.ROOMCODE                                    \r\n";
                SQL += " 	, P.REQUEST1                                    \r\n";
                SQL += " 	, P.REQUEST2                                    \r\n";
                SQL += " 	, P.GBIO                                        \r\n";
                SQL += " 	, KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(P.DRCODE)   AS DRNAME    \r\n";
                SQL += "	, DECODE(P.BDATE ,NULL, TO_CHAR(SYSDATE,'YYYY-MM-DD'),  TO_CHAR(P.BDATE, 'YYYY-MM-DD'))   AS BDATE    \r\n";
                SQL += " FROM " + ComNum.DB_MED + "EXAM_ORDER_PB P          \r\n";
                SQL += "    , " + ComNum.DB_PMPA + "BAS_PATIENT   B         \r\n";
                //2019-08-06 안정수 , IPD_NEW_MASTER 추가 
                SQL += "    , " + ComNum.DB_PMPA + "IPD_NEW_MASTER   C      \r\n";
                SQL += " WHERE 1 = 1                                        \r\n";
                SQL += "   AND B.PANO  = " + ComFunc.covSqlstr(strPano, false);
                SQL += "   AND P.PANO(+)  = B.PANO                          \r\n";
                SQL += "   AND P.PANO  = C.PANO                             \r\n";
                //SQL += "   AND P.BDATE(+) = " + ComFunc.covSqlDate(strBData, false);
                //2019-11-04 안정수 조건변경
                SQL += "   AND P.BDATE(+) >= " + ComFunc.covSqlDate(strBData, false); 
                SQL += "   AND P.BDATE(+) <= TRUNC(SYSDATE)                 \r\n";
                if(strWard != "'")
                {
                    SQL += "   AND C.WARDCODE = '" + strWard + "'           \r\n";
                }
                //SQL += "   AND C.GBSTS IN ('0', '1', '2')                   \r\n";
            }
            else
            {
                SQL = "";
                SQL += " SELECT                                             \r\n";
                SQL += "      B.PANO                                        \r\n";
                SQL += " 	, B.SNAME                                       \r\n";
                SQL += " 	, B.JUMIN1 || '-' || JUMIN2 JUMIN               \r\n";
                SQL += "    , P.DEPTCODE                                    \r\n";
                SQL += " 	, P.DRCODE                                      \r\n";
                SQL += " 	, ''                                            \r\n";
                SQL += " 	, ''                                            \r\n";
                SQL += " 	, P.REQUEST1                                    \r\n";
                SQL += " 	, P.REQUEST2                                    \r\n";
                SQL += " 	, P.GBIO                                        \r\n";
                SQL += " 	, KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(P.DRCODE)   AS DRNAME    \r\n";
                SQL += "	, DECODE(P.BDATE ,NULL, TO_CHAR(SYSDATE,'YYYY-MM-DD'),  TO_CHAR(P.BDATE, 'YYYY-MM-DD'))   AS BDATE    \r\n";
                SQL += " FROM " + ComNum.DB_MED + "EXAM_ORDER_PB P          \r\n";
                SQL += "    , " + ComNum.DB_PMPA + "BAS_PATIENT   B         \r\n";                
                SQL += "    , " + ComNum.DB_PMPA + "OPD_MASTER   C          \r\n";
                SQL += " WHERE 1 = 1                                        \r\n";
                SQL += "   AND B.PANO  = " + ComFunc.covSqlstr(strPano, false);
                SQL += "   AND P.PANO(+)  = B.PANO                          \r\n";
                SQL += "   AND P.PANO  = C.PANO                             \r\n";
                //2019-11-04 안정수 조건변경
                //SQL += "   AND P.BDATE(+) = " + ComFunc.covSqlDate(strBData, false);                
                //SQL += "   AND C.ACTDATE(+) = " + ComFunc.covSqlDate(strBData, false);
                SQL += "   AND P.BDATE(+) >= " + ComFunc.covSqlDate(strBData, false);
                SQL += "   AND P.BDATE(+) <= TRUNC(SYSDATE)                 \r\n";
                //SQL += "   AND C.ACTDATE(+) >= " + ComFunc.covSqlDate(strBData, false);
                //SQL += "   AND C.ACTDATE(+) <= TRUNC(SYSDATE)               \r\n";
            }

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon); 

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


        public DataTable sel_EXAM_ORDER_COVID(PsmhDb pDbCon, string strPano, string strBData)
        {
            DataTable dt = null;


            SQL = "";
            SQL += ComNum.VBLF + "SELECT * ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "ETC_COVID";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND PANO = '" + strPano + "' ";
            SQL += ComNum.VBLF + "  AND BALDATE = '" + strBData + "'";


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

        /// <summary>혈액신청대상자</summary>
        /// <returns></returns>
        public DataTable sel_EXAM_RESULTC_Blood(PsmhDb pDbCon, string strSpecNo)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT S.PANO                                                                                                      \r\n";
            SQL += "      , S.BI                                                                                                        \r\n";
            SQL += "      , S.SNAME                                                                                                     \r\n";
            SQL += "      , S.AGE || '/' ||  S.SEX              AS AGE_SEX                                                              \r\n";
            SQL += "      , S.ROOM                                                                                                      \r\n";
            SQL += "      , S.DEPTCODE                                                                                                  \r\n";
            SQL += "      , KOSMOS_OCS.FC_BAS_CLINICDEPT_DEPTNAMEK(S.DEPTCODE)  AS DEPT_NAME                                            \r\n";
            SQL += "      , S.DRCODE                                                                                                    \r\n";
            SQL += "      , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(S.DRCODE)           AS DR_NAME                                              \r\n";
            SQL += "      , TO_CHAR(S.BDATE, 'YYYY-MM-DD')                      AS BDATE                                                \r\n";
            SQL += "      , S.DRCOMMENT                                                                                                 \r\n";
            SQL += "      , SUBSTR(S.DRCOMMENT, INSTR(S.DRCOMMENT, '[혈액사용예정일:', 1, 1) + LENGTH('[혈액사용예정일:'),10) as ACT_DATE  \r\n";
            SQL += "      , C.SUBCODE                                                                                                   \r\n";
            SQL += "      , COUNT(C.SUBCODE) CNT                                                                                        \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_BLOOD_MASTER_ABO(S.PANO)         AS BLOOD_TYPE                                           \r\n";
            SQL += "      , KOSMOS_OCS.FC_NUR_MASTER_DIAGNOSIS(S.PANO, S.BDATE) AS DIAGNOSIS                                            \r\n";
            SQL += "      , (SELECT CASE WHEN COUNT(PANO) > 0 THEN '유'                                                                 \r\n";
            SQL += "                     ELSE '무' END                                                                                  \r\n";
            SQL += "           FROM KOSMOS_OCS.EXAM_BLOOD_IO                                                                            \r\n";
            SQL += "           WHERE OUTDATE < S.BDATE                                                                                  \r\n";
            SQL += "             AND PANO    = S.PANO                                                                                   \r\n";
            SQL += "           )                       				            AS BLOOD_HIS                                            \r\n";
            SQL += "      ,	(                                                                                                           \r\n";
            SQL += "            Case    WHEN SUBCODE = 'BT021' THEN 'Packed RBC(확보)       400mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT011' THEN 'Whole Blood            400mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT023' THEN 'Platelet Concentrate   400mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT041' THEN 'Fresh Frozen Plasma    400mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT051' THEN 'Cryoprecipitate        400mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT31'  THEN 'Phlebotomy             400mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT081' THEN 'Flt Packed RBC         400mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT091' THEN 'Flt Platelet Concen.   400mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT08Z' THEN 'RBC filter             400mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT09Z' THEN 'Platelet filter        400mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT02'  THEN 'Packed RBC(확보)       320mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT01'  THEN 'Whole Blood            320mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT022' THEN 'Platelet Concentrate   320mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT04'  THEN 'Fresh Frozen Plasma    320mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT05'  THEN 'Cryoprecipitate        320mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT08'  THEN 'Phlebotomy             320mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT02B' THEN 'Packed RBC(완료)       400mL'                                      \r\n";
            SQL += "                    WHEN SUBCODE = 'BT02A' THEN 'Packed RBC(완료)       320mL'                                      \r\n";
            SQL += "                    ELSE 'Other Blood'                                                                              \r\n";
            SQL += "         END                                                                                                        \r\n";
            SQL += "	 )                                                   AS BLOOD_NAME                                              \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_RESULTC C                                                                                    \r\n";
            SQL += "     , KOSMOS_OCS.EXAM_SPECMST S                                                                                    \r\n";
            SQL += "  WHERE 1 = 1                                                                                                       \r\n";
            SQL += "    AND C.SPECNO = S.SPECNO                                                                                         \r\n";
            SQL += "    AND C.SPECNO = " + ComFunc.covSqlstr(strSpecNo, false);
            SQL += "    AND C.SUBCODE LIKE  'BT%'                                                                                       \r\n";
            SQL += "  GROUP BY S.PANO    , S.BI     , S.SNAME                       , S.AGE         , S.SEX     , S.ROOM                \r\n";
            SQL += "         , S.DEPTCODE, S.DRCODE , TO_CHAR(S.BDATE, 'YYYY-MM-DD'), S.DRCOMMENT   , C.SUBCODE , S.BDATE               \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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
       
        /// <summary>검체현황조회</summary>
        /// <param name="strPano"></param>
        /// <param name="strFDate"></param>
        /// <param name="strTDate"></param>
        /// <param name="strWS"></param>
        /// <returns></returns>
        public DataSet sel_EXAM_SPECMST_Viewer(PsmhDb pDbCon, string strPano, string strFDate, string strTDate, string strWS)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                                        \r\n";
            SQL += "         SPECNO                                                     AS SPECNO     --01          \r\n";
            SQL += "       , TO_CHAR(RECEIVEDATE, 'YYYY-MM-DD HH24:MI')                 AS RECEIVEDATE--02          \r\n";
            SQL += "       , SNAME                                                      AS SNAME      --03          \r\n";
            SQL += "       , CASE WHEN IPDOPD = 'I' THEN '입원'                                                     \r\n";
            SQL += "              ELSE CASE WHEN BI < 60 THEN '외래'                                                \r\n";
            SQL += "                        ELSE '건진' END                                                         \r\n";
            SQL += "          END                                                       AS IPDOPD     --04          \r\n";
            SQL += "       , DEPTCODE                                                   AS DEPTCODE   --05          \r\n";
            SQL += "       , ROOM                                                       AS ROOM       --06          \r\n";
            SQL += "       , DRCODE                                                     AS DRCODE     --07          \r\n";
            SQL += "       , CASE WHEN DEPTCODE IN ('EM', 'ER') THEN '응급' ELSE '' END AS GBER       --08          \r\n";
            SQL += "       , WORKSTS                                                    AS WORKSTS    --09/08       \r\n";
            SQL += "       , KOSMOS_OCS.FC_EXAM_RESULTC_EXAMNAME(SPECNO)                AS EXAM_NAME  --10/09       \r\n";
            SQL += "       , KOSMOS_OCS.FC_EXAM_SPECMST_NM('14', SPECCODE, 'Y')         AS SPEC_NAME  --11/10       \r\n";
            SQL += "       , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_STATUS', STATUS)    AS STATUS     --12/11       \r\n";
            SQL += "       , TO_CHAR(ORDERDATE, 'YYYY-MM-DD HH24:MI')                   AS ORDERDATE  --13/12       \r\n";
            SQL += "       , TO_CHAR(BDATE, 'YYYY-MM-DD')                               AS BDATE      --14/13       \r\n";
            SQL += "       , TO_CHAR(RESULTDATE, 'YYYY-MM-DD HH24:MI')                  AS RESULTDATE --15/14       \r\n";
            SQL += "       , PANO                                                                                   \r\n";
            SQL += "       , PRINT                                                                                  \r\n";
            SQL += "       , GB_GWAEXAM                                                                             \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_SPECMST                                                                  \r\n";
            SQL += " WHERE 1 = 1                                                                                    \r\n";
            SQL += "   AND PANO      = " + ComFunc.covSqlstr(strPano, false);
            SQL += "   AND BLOODDATE BETWEEN " + ComFunc.covSqlDate(strFDate + " 00:00", "YYYY-MM-DD HH24:MI", false);
            SQL += "                     AND " + ComFunc.covSqlDate(strTDate + " 23:59", "YYYY-MM-DD HH24:MI", false);

            if (strWS.IndexOf('*') == -1)
            {
                SQL += "   AND WORKSTS LIKE " + ComFunc.covSqlstr(strWS, false) + " || '%'                           \r\n";
            }

            if (string.IsNullOrEmpty(strWS) == false && strWS.IndexOf('*') == -1 && strWS.Substring(0, 1) == "M")
            {
                switch (int.Parse(strWS.Substring(1, 1)))
                {
                    case 1:
                        //혈액
                        SQL += "   AND SUBSTR(SPECCODE,1,2) IN ('01')                                                \r\n";
                        break;
                    case 2:
                        //체액
                        SQL += "   AND SUBSTR(SPECCODE,1,2) IN ('03','04')                                                \r\n";
                        break;
                    case 3:
                        //소변
                        SQL += "   AND SUBSTR(SPECCODE,1,2) IN ('02')                                                \r\n";
                        break;
                    case 4:
                        //기타
                        SQL += "   AND SUBSTR(SPECCODE,1,2) NOT IN ('01','02','03','04')                              \r\n";
                        break;
                    default:
                        break;
                }
            }

            SQL += " ORDER BY BLOODDATE,SPECNO                                                                      \r\n";

            string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);
            //string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return ds;

        }

        /// <summary>sel_OCS_DOCTOR</summary>
        /// <param name="strSabun"></param>
        /// <returns></returns>
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
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
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

            return strReturn;
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
            
            return strMSTEL;
        }

        /// <summary>sel_HEA_JEPSU</summary>
        /// <param name="strBi"></param>
        /// <param name="strWrtno"></param>
        /// <returns></returns>
        public DataTable sel_HEA_JEPSU(PsmhDb pDbCon, string strBi, string strWrtno)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT PANO                             \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "HEA_JEPSU-- 종검     \r\n";
            SQL += " WHERE 1 = 1                             \r\n";
            SQL += "    AND '61'  = " + ComFunc.covSqlstr(strBi, false);
            SQL += "    AND WRTNO = " + ComFunc.covSqlstr(strWrtno, false);
            SQL += "   UNION ALL                             \r\n";
            SQL += " SELECT PANO                             \r\n";
            SQL += "   FROM KOSMOS_PMPA.HIC_JEPSU-- 일반건진 \r\n";
            SQL += " WHERE 1 = 1                             \r\n";
            SQL += "    AND '62'  = " + ComFunc.covSqlstr(strBi, false);
            SQL += "    AND WRTNO = " + ComFunc.covSqlstr(strWrtno, false);

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        /// <summary>sel_EXAM_INFECT_MASTER</summary>
        /// <param name="strSpecNO"></param>
        /// <param name="strPano"></param>
        /// <param name="strSubCode"></param>
        /// <returns></returns>
        public DataTable sel_EXAM_INFECT_MASTER(PsmhDb pDbCon, string strSpecNO, string strPano, string strSubCode)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT ROWID FROM " + ComNum.DB_MED + "EXAM_INFECT_MASTER    \r\n";
            SQL += "   WHERE PANO = '" + strPano + "'                   \r\n";
            SQL += "   AND SPECNO = '" + strSpecNO + "'                 \r\n";
            SQL += "   AND GUBUN ='01'                                  \r\n";

            switch (strSubCode.Trim())
            {
                case "SE01":
                case "SE01A":
                case "SI001":
                    SQL += "  AND EXNAME = 'VDRL'       \r\n";
                    break;
                case "SI07":
                case "SI07A":
                    SQL += " AND EXNAME = 'Hepatitis B' \r\n";
                    break;
                case "SI081":
                case "SI08A":
                    SQL += " AND EXNAME = 'Hepatitis C' \r\n";
                    break;
                case "SI12":
                case "SI12D":
                    SQL += " AND EXNAME = 'HIV' \r\n";
                    break;
                default:
                    break;
            }


            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        /// <summary>검사결과지 출력</summary>
        /// <param name="strSpecNO"></param>
        /// <param name="isPB"></param>
        /// <returns></returns>
        public DataTable sel_EXAM_RESULTC_Print(PsmhDb pDbCon, string strSpecNO, enmSel_EXAM_RESULTC_Print_PB enmisBp, bool isResult)
        {
            DataTable dt = null; 

            SQL = "";
            SQL += " SELECT WORKSTS_NAME                                                                                                                \r\n";
            SQL += "      , PANO                                                                                                                        \r\n";
            SQL += "      , SNAME                                                                                                                       \r\n";
            SQL += "      , SEX_AGE                                                                                                                     \r\n";
            SQL += "      , WARD_NAME                                                                                                                   \r\n";
            SQL += "      , DEPT_NAME                                                                                                                   \r\n";
            SQL += "      , DR_NAME                                                                                                                     \r\n";
            SQL += "      , EXAMNAME                                                                                                                    \r\n";
            SQL += "      , SPEC_NAME                                                                                                                   \r\n";
            SQL += "      , TO_CHAR(RESULT) AS RESULT                                                                                                   \r\n";
            SQL += "      , FOOTTYPE                                                                                                                    \r\n";
            SQL += "      , REFER                                                                                                                       \r\n";
            SQL += "      , REFER_VALUE                                                                                                                 \r\n";
            SQL += "      , UNIT                                                                                                                        \r\n";
            SQL += "      , REFER_RAG                                                                                                                   \r\n";
            SQL += "      , RESULTSABUN                                                                                                                 \r\n";
            SQL += "      , KOSMOS_OCS.FC_BAS_PASS_NAME(RESULTSABUN)AS RESULT_NAME                                                                      \r\n";
            SQL += "      , BLOODDATE                                                                                                                   \r\n";
            SQL += "      , RECEIVEDATE                                                                                                                 \r\n";
            SQL += "      , RESULTDATE                                                                                                                  \r\n";
            SQL += "      , EXAM_USER                                                                                                                   \r\n";
            SQL += "      , PRINT                                                                                                                       \r\n";
            SQL += "      , STATUS                                                                                                                      \r\n";
            SQL += "      , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_STATUS',MST_STATUS)	AS MST_STATUS                                                   \r\n";
            SQL += "      , MASTERCODE                                                                                                                  \r\n";
            SQL += "      , SUBCODE                                                                                                                     \r\n";
            SQL += "      , DELTA                                                                                                                       \r\n";
            SQL += "      , PANIC                                                                                                                       \r\n";
            SQL += "  FROM(                                                                                                                             \r\n";
            SQL += "    SELECT                                                                                                                          \r\n";
            SQL += "          KOSMOS_OCS.FC_EXAM_SPECODE_GPNM(S.WORKSTS)                                            AS WORKSTS_NAME                     \r\n";
            SQL += "        , S.PANO                                                                                AS PANO                             \r\n";
            SQL += "        , S.SNAME                                                                               AS SNAME                            \r\n";
            SQL += "        , S.SEX || '/' || S.AGE                                                                 AS SEX_AGE                          \r\n";
            SQL += "        , DECODE(S.IPDOPD, 'I', TRIM(S.WARD) || '-' || TRIM(TO_CHAR(S.ROOM, '000')), '외래')    AS WARD_NAME                        \r\n";
            SQL += "        , (                                                                                                                         \r\n";
            SQL += "            CASE WHEN(S.DEPTCODE = 'EM' OR S.DEPTCODE = 'ER') THEN '응급실'                                                         \r\n";
            SQL += "                 ELSE KOSMOS_OCS.FC_BAS_CLINICDEPT_DEPTNAMEK(S.DEPTCODE)                                                            \r\n";
            SQL += "                 END                                                                                                                \r\n";
            SQL += "          )                                                                                     AS DEPT_NAME                        \r\n";
            SQL += "        , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(S.DRCODE)                                             AS DR_NAME                          \r\n";
            SQL += "        , R.SEQNO                                                                               AS SEQNO                            \r\n";
            SQL += "		, R.MASTERCODE                                                                          AS MASTERCODE                       \r\n";
            SQL += "		, R.SUBCODE                                                                             AS SUBCODE                          \r\n";
            SQL += "		, R.STATUS                                                                              AS STATUS                           \r\n";
            SQL += "		, M.EXAMNAME                                                                            AS EXAMNAME                         \r\n";
            SQL += "		, KOSMOS_OCS.FC_EXAM_SPECMST_NM('14', S.SPECCODE, 'N')                                  AS SPEC_NAME                        \r\n";
            SQL += "        , CASE WHEN R.STATUS = 'V' THEN R.RESULT WHEN R.RESULT IS NOT NULL THEN '검사중..' ELSE '' END                            AS RESULT                           \r\n";
            SQL += "		, '0'                                                                                   AS FOOTTYPE                         \r\n";
            SQL += "        , 0                                                                                     AS FOOTSORT                         \r\n";
            SQL += "        , R.REFER                                                                               AS REFER                            \r\n";
            SQL += "		, KOSMOS_OCS.FC_EXAM_MASTER_SUB_REF2('1', R.SUBCODE, S.SEX, S.AGE, R.RESULT, R.RESULTDATE)             AS REFER_VALUE --실제 REF           \r\n";
            SQL += "		, R.UNIT                                                                                AS UNIT                             \r\n";
            SQL += "		, KOSMOS_OCS.FC_EXAM_MASTER_SUB_REF2('0', R.SUBCODE, S.SEX, S.AGE, R.RESULT, R.RESULTDATE)             AS REFER_RAG    --REF 범위          \r\n";
            SQL += "		, R.PANIC                                                                               AS PANIC                            \r\n";
            SQL += "		, R.DELTA                                                                               AS DELTA                            \r\n";
            SQL += "		, R.RESULTSABUN                                                                         AS RESULTSABUN                      \r\n";
            SQL += "		, TO_CHAR(S.BLOODDATE  , 'YYMMDD-HH24:MI')                                              AS BLOODDATE    --채취일자          \r\n";
            SQL += "		, TO_CHAR(S.RECEIVEDATE, 'YYMMDD-HH24:MI')                                              AS RECEIVEDATE  --접수일자          \r\n";
            SQL += "		, TO_CHAR(R.RESULTDATE , 'YYMMDD-HH24:MI')                                              AS RESULTDATE   --보고일자          \r\n"; 

            if (enmisBp == enmSel_EXAM_RESULTC_Print_PB.PB)
            {
                SQL += "            , KOSMOS_OCS.FC_EXAM_RESULTC_SABUN(R.SPECNO, 'Y', S.GB_GWAEXAM, S.DEPTCODE)   AS EXAM_USER                          \r\n";
            }
            else if (enmisBp == enmSel_EXAM_RESULTC_Print_PB.NONE)
            {
                SQL += "            , KOSMOS_OCS.FC_EXAM_RESULTC_SABUN(R.SPECNO, 'N', S.GB_GWAEXAM, S.DEPTCODE)   AS EXAM_USER                          \r\n";
            }
            else if (enmisBp == enmSel_EXAM_RESULTC_Print_PB.ALL)
            {
                SQL += "            , ''                                                                          AS EXAM_USER                          \r\n";
            }
            SQL += "              , S.PRINT                                                                       AS PRINT                              \r\n";
            SQL += "              , S.STATUS																	  AS MST_STATUS                         \r\n";
            SQL += "    FROM " + ComNum.DB_MED + "EXAM_RESULTC R                                                                                        \r\n";
            SQL += "       , " + ComNum.DB_MED + "EXAM_MASTER  M                                                                                        \r\n";
            SQL += "       , " + ComNum.DB_MED + "EXAM_SPECMST S                                                                                        \r\n";
            SQL += "    WHERE 1 = 1                                                                                                                     \r\n";
            SQL += "      AND R.SPECNO      = " + ComFunc.covSqlstr(strSpecNO, false);
            SQL += "      AND R.SUBCODE     = M.MASTERCODE                                                                                              \r\n";
            SQL += "      AND R.SPECNO      = S.SPECNO                                                                                                  \r\n";

            if (enmisBp == enmSel_EXAM_RESULTC_Print_PB.PB)
            {
                SQL += "            AND R.MASTERCODE  = 'HR10'                                                                                          \r\n";
            }
            else if (enmisBp == enmSel_EXAM_RESULTC_Print_PB.NONE)
            {
                SQL += "            AND R.MASTERCODE  !='HR10'                                                                                          \r\n";
            }
            SQL += "   UNION ALL                                                                                                                        \r\n";
            SQL += "      SELECT                                                                                                                        \r\n";
            SQL += "          ''                                                                                    AS WORKSTS_NAME                     \r\n";
            SQL += "        , ''                                                                                    AS PANO                             \r\n";
            SQL += "        , ''                                                                                    AS SNAME                            \r\n";
            SQL += "        , ''                                                                                    AS SEX_AGE                          \r\n";
            SQL += "        , ''                                                                                    AS WARD_NAME                        \r\n";
            SQL += "        , ''                                                                                    AS DEPT_NAME                        \r\n";
            SQL += "        , ''                                                                                    AS DR_NAME                          \r\n";
            SQL += "        , R.SEQNO                                                                               AS SEQNO                            \r\n";
            SQL += "		, R.MASTERCODE                                                                          AS MASTERCODE                       \r\n";
            SQL += "		, R.SUBCODE                                                                             AS SUBCODE                          \r\n";
            SQL += "		, R.STATUS                                                                              AS STATUS                           \r\n";
            SQL += "		, ''                                                                                    AS EXAMNAME                         \r\n";
            SQL += "        , ''                                                                                    AS SPEC_NAME                        \r\n";
            SQL += "        , F.FOOTNOTE                                                                            AS RESULT                           \r\n";
            SQL += "		, '1'                                                                                   AS FOOTTYPE                         \r\n";
            SQL += "        , F.SORT                                                                                AS FOOTSORT                         \r\n";
            SQL += "		, R.REFER                                                                               AS REFER                            \r\n";
            SQL += "		, ''                                                                                    AS REFER_VALUE  --실제 REF          \r\n";
            SQL += "		, ''                                                                                    AS UNIT                             \r\n";
            SQL += "        , ''                                                                                    AS REFER_RAG    --REF 범위          \r\n";
            SQL += "		, R.PANIC                                                                               AS PANIC                            \r\n";
            SQL += "		, R.DELTA                                                                               AS DELTA                            \r\n";
            SQL += "		, R.RESULTSABUN                                                                         AS RESULTSABUN                      \r\n";
            SQL += "		, ''                                                                                    AS BLOODDATE    --채취일자          \r\n";
            SQL += "		, ''                                                                                    AS RECEIVEDATE  --접수일자          \r\n";
            SQL += "		, ''                                                                                    AS RESULTDATE   --보고일자          \r\n";
            SQL += "	    , ''                                                                                    AS EXAM_USER                        \r\n";
            SQL += "        , 0                                                                                     AS PRINT                            \r\n";
            SQL += "        ,''                                                                                     AS MST_STATUS                       \r\n";
            SQL += "    FROM " + ComNum.DB_MED + "EXAM_RESULTC  R                                                                                       \r\n";
            SQL += "       , " + ComNum.DB_MED + "EXAM_RESULTCF F                                                                                       \r\n";
            SQL += "    WHERE 1 = 1                                                                                                                     \r\n";
            SQL += "      AND R.SPECNO              = " + ComFunc.covSqlstr(strSpecNO, false);

            if (enmisBp == enmSel_EXAM_RESULTC_Print_PB.PB)
            {
                SQL += "      AND R.MASTERCODE  = 'HR10'                                                                                                \r\n";
            }
            else if (enmisBp == enmSel_EXAM_RESULTC_Print_PB.NONE)
            {
                SQL += "      AND R.MASTERCODE  !='HR10'                                                                                                \r\n";
            }

            SQL += "      AND R.SPECNO              = F.SPECNO                                                                                         \r\n";
            SQL += "      AND TO_NUMBER(R.SEQNO)    = F.SEQNO                                                                                          \r\n";
            SQL += "    ORDER BY SEQNO, FOOTTYPE, FOOTSORT                                                                                             \r\n";
            SQL += "	)                                                                                                                              \r\n";
            SQL += "WHERE 1 = 1                                                                                                                        \r\n";

            if (isResult == true)
            {
                //2017.12.11.김홍록:결과가 있는 것만 출력
                SQL += "  AND (STATUS ='H' OR (RESULT IS NOT NULL) OR MASTERCODE = SUBCODE)                                                            \r\n";
            }

            if (enmisBp != enmSel_EXAM_RESULTC_Print_PB.ALL)
            {
                SQL += "  AND (LENGTH(TRIM(RESULT)) > 0 OR MASTERCODE = SUBCODE OR STATUS = 'H')                                                      \r\n";
            }
            
            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        /// <summary>BST 사용현황</summary>
        /// <param name="strFDate">조회시작일자</param>
        /// <param name="strTDate">조회종료일자</param>
        /// <param name="strWard">사용병동</param>
        /// <param name="isFrns">불출여부</param>
        /// <returns></returns>
        public DataSet sel_EXAM_EXBST_Cnt(PsmhDb pDbCon, string strFDate, string strTDate, string strWard, bool isFrns)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                                            \r\n";
            SQL += "      CASE WHEN GROUPING(WARDCODE) = 1 AND  GROUPING(PANO) = 1 THEN '총계'                          \r\n";
            SQL += "           WHEN GROUPING(WARDCODE) = 0 AND  GROUPING(PANO) = 1 THEN '소계'                          \r\n";
            SQL += "           ELSE WARDCODE                                                                            \r\n";
            SQL += "       END                                                                     AS WARDCODE          \r\n";
            SQL += "    , CASE WHEN GROUPING(WARDCODE) = 1 AND  GROUPING(PANO) = 1 THEN ''                              \r\n";
            SQL += "           WHEN GROUPING(WARDCODE) = 0 AND  GROUPING(PANO) = 1 THEN TO_CHAR(SUM(QTY), '9,999,999')     \r\n";
            SQL += "           ELSE PANO                                                                                \r\n";
            SQL += "       END                                                                     AS PANO              \r\n";
            SQL += "    , CASE WHEN GROUPING(WARDCODE) = 1 AND  GROUPING(PANO) = 1 THEN ''                              \r\n";
            SQL += "           WHEN GROUPING(WARDCODE) = 0 AND  GROUPING(PANO) = 1 THEN ''                              \r\n";
            SQL += "           ELSE MAX(SNAME)                                                                          \r\n";
            SQL += "       END                                                                     AS SNAME             \r\n";
            SQL += "    , CASE WHEN GROUPING(WARDCODE) = 1 AND  GROUPING(PANO) = 1 THEN ''                              \r\n";
            SQL += "           WHEN GROUPING(WARDCODE) = 0 AND  GROUPING(PANO) = 1 THEN ''                              \r\n";
            SQL += "           ELSE MAX(DEPTCODE)                                                                       \r\n";
            SQL += "       END                                                                     AS DEPTCODE          \r\n";
            SQL += "    , CASE WHEN GROUPING(WARDCODE) = 1 AND  GROUPING(PANO) = 1 THEN ''                              \r\n";
            SQL += "           WHEN GROUPING(WARDCODE) = 0 AND  GROUPING(PANO) = 1 THEN ''                              \r\n";
            SQL += "           ELSE MAX(BDATE)                                                                          \r\n";
            SQL += "       END                                                                     AS BDATE             \r\n";
            SQL += "    , CASE WHEN GROUPING(WARDCODE) = 1 AND  GROUPING(PANO) = 1 THEN TO_CHAR(SUM(QTY), '9,999,999')     \r\n";
            SQL += "           WHEN GROUPING(WARDCODE) = 0 AND  GROUPING(PANO) = 1 THEN ''                              \r\n";
            SQL += "           ELSE TO_CHAR(SUM(QTY), '9,999,999')                                                         \r\n";
            SQL += "       END                                                                     AS QTY               \r\n";
            SQL += " FROM(                                                                                              \r\n";
            SQL += "        SELECT                                                                                      \r\n";
            SQL += "              DECODE(A.WARDCODE, 'IQ', 'NR', 'ND', 'NR', A.WARDCODE)    AS WARDCODE -- 2            \r\n";
            SQL += "            , A.PANO                                                    AS PANO     -- 3            \r\n";
            SQL += "            , A.SNAME                                                   AS SNAME    -- 4            \r\n";
            SQL += "            , A.DEPTCODE                                                AS DEPTCODE -- 5            \r\n";
            SQL += "            , TO_CHAR(A.BDATE, 'YYYY-MM-DD')                            AS BDATE    -- 1            \r\n";
            SQL += "            , SUM(A.QTY)                                                AS QTY      -- 6            \r\n";
            SQL += "         FROM KOSMOS_OCS.EXAM_EXBST A                                                               \r\n";
            SQL += "        WHERE 1 = 1                                                                                 \r\n";

            if (isFrns == true)
            {
                // 불출
                SQL += "          AND A.ODATE   BETWEEN " + ComFunc.covSqlDate(strFDate, false);
                SQL += "                            AND " + ComFunc.covSqlDate(strTDate, false);
                SQL += "          AND A.ODATE IS NOT NULL                                                               \r\n";
            }
            else
            {
                // 미불출
                SQL += "          AND A.ACTDATE   BETWEEN " + ComFunc.covSqlDate(strFDate, false);
                SQL += "                              AND " + ComFunc.covSqlDate(strTDate, false);
                SQL += "          AND A.ODATE IS NULL                                                                   \r\n";
            }

            SQL += "          AND A.QTY <> 0                                                                            \r\n";

            if (strWard.IndexOf('*') == -1)
            {
                if (strWard == "ND" || strWard == "NR")
                {
                    SQL += "          AND A.WARDCODE IN ('ND','IQ','NR')                                                \r\n";
                }
                else
                {
                    SQL += "          AND A.WARDCODE = " + ComFunc.covSqlstr(strWard, false);
                }
            }

            SQL += "        GROUP BY TO_CHAR(A.BDATE, 'YYYY-MM-DD'), A.WARDCODE, A.PANO, A.SNAME, A.DEPTCODE            \r\n";

            if (isFrns == true)
            {
                SQL += "        ORDER BY WARDCODE, PANO, BDATE                                                          \r\n";
            }
            else
            {
                SQL += "        ORDER BY BDATE ASC, WARDCODE, PANO                                                      \r\n";
            }

            SQL += "     )                                                                                              \r\n";
            SQL += " GROUP BY ROLLUP(WARDCODE, PANO)                                                                    \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

            return ds;
        }

        /// <summary>MIC 검사리스트 조회</summary>
        /// <param name="strFDate">시작일자</param>
        /// <param name="strTDate">종료일자</param>
        /// <param name="strPano">등록번호</param>
        /// <param name="enmOption">결과형태</param>
        /// <param name="gbio">환자진료구분</param>
        /// <returns></returns>
        public DataSet sel_EXAM_RESULTC_MicResult(PsmhDb pDbCon, string strFDate, string strTDate, string strPano, enmSel_EXAM_RESULTC_MicResultOptient enmOption, enmComParamGBIO gbio)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                                                                                           \r\n";
            SQL += "        A.PANO                                                                                                                    \r\n";
            SQL += "      , B.SNAME                                                                                                                   \r\n";
            SQL += "      , B.DEPTCODE                                                                                                                \r\n";
            SQL += "      , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(B.DRCODE)       AS DR_NAME                                                                \r\n";
            SQL += "      , B.BI                                            AS BI                                                                     \r\n";
            SQL += "      , B.IPDOPD                                        AS IPDOPD                                                                 \r\n";
            SQL += "      , TO_CHAR(B.BDATE, 'YYYY-MM-DD')                  AS BDATE                                                                  \r\n";
            SQL += "      , TO_CHAR(A.RESULTDATE, 'YYYY-MM-DD HH24:MI') 	AS RESULTDATE                                                             \r\n";
            SQL += "      , A.RESULT                                                                                                                  \r\n";
            SQL += "      , A.SPECNO                                                                                                                  \r\n";
            SQL += "      , DECODE(KOSMOS_OCS.FC_IPD_NEW_MASTER_GBSTS(TO_CHAR(B.BDATE, 'YYYY-MM-DD'), A.PANO), 'Y', '입원', '') AS TODAY_IN           \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_RESULTC A                                                                                                 \r\n";
            SQL += "    , KOSMOS_OCS.EXAM_SPECMST B                                                                                                   \r\n";
            SQL += "  WHERE 1 = 1                                                                                                                     \r\n";
            SQL += "    AND B.RESULTDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                         AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "    AND B.SPECNO     = A.SPECNO(+)                                                                                                \r\n";
            SQL += "    AND A.MASTERCODE = A.SUBCODE                                                                                                  \r\n";
            SQL += "    AND B.STATUS IN ('04'  , '05')                                                                                                \r\n";
            SQL += "    AND A.MASTERCODE IN ('MI35', 'MI351')                                                                                         \r\n";

            if (gbio == enmComParamGBIO.I)
            {
                SQL += "    AND B.IPDOPD  = 'I'                                                                                                       \r\n";
            }
            else if (gbio == enmComParamGBIO.O)
            {
                SQL += "    AND B.IPDOPD  = 'O'                                                                                                       \r\n";
            }

            if (string.IsNullOrEmpty(strPano) == false)
            {
                SQL += "    AND B.PANO  = " + ComFunc.covSqlstr(strPano, false);
            }

            if (enmOption == enmSel_EXAM_RESULTC_MicResultOptient.GROWTH)
            {
                SQL += "    AND (UPPER(A.RESULT) NOT LIKE 'NO GROWTH%' AND UPPER(A.RESULT) NOT  LIKE 'NORMAL FLORA%')                                  \r\n";
            }
            else if (enmOption == enmSel_EXAM_RESULTC_MicResultOptient.NO_GROWTH)
            {
                SQL += "    AND (UPPER(A.RESULT)     LIKE 'NO GROWTH%'  OR UPPER(A.RESULT)      LIKE 'NORMAL FLORA%')                                  \r\n";
            }

            SQL += "          ORDER BY A.RESULTDATE, A.PANO                                                                                            \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

            return ds;

        }

        /// <summary>병동미시행처방보기 </summary>
        /// <param name="strWard">병동</param>
        /// <param name="strPano">환자번호</param>
        /// <param name="strDeptCode">과코드</param>
        /// <param name="isOut">당일퇴원</param>
        /// <param name="nActingType">0:전체, 1:미시행, 2:시행</param>
        /// <param name="nSearchType">0:재원중,1:기간설정</param>
        /// <param name="strFDate">시작일자</param>
        /// <param name="strTDate">종료일자</param>
        /// <returns></returns>
        public DataSet sel_IPD_NEW_MASTER_EXAM(PsmhDb pDbCon, string strWard, string strPano, string strDeptCode, bool isOut, int nActingType, int nSearchType, string strFDate, string strTDate, int[] nChkExam)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " WITH T AS(                                                                                                                    \r\n";
            SQL += " 			SELECT                                                                                                             \r\n";
            SQL += " 					A.ROOMCODE									 		AS ROOMCODE	 --	01                                     \r\n";
            SQL += " 				  , A.PANO      								 		AS PANO    	 --	02                                     \r\n";
            SQL += " 				  , A.SNAME     								 		AS SNAME     --	03                                     \r\n";
            SQL += " 				  , A.SEX       								 		AS SEX    	 --	05                                     \r\n";
            SQL += " 				  , A.AGE       								 		AS AGE    	 --	04                                     \r\n";
            SQL += " 				  , A.DEPTCODE  								 		AS DEPTCODE  --	06                                     \r\n";
            SQL += " 				  , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE)    		AS DRNAME    --	07                                     \r\n";
            SQL += " 				  , A.INDATE									 		AS INDATE                                              \r\n";
            SQL += " 				  , A.OUTDATE									 		AS OUTDATE                                             \r\n";
            //SQL += " 				  , NVL(A.OUTDATE,TO_DATE('2999-12-31','YYYY-MM-DD'))	AS OUTDATE                                             \r\n";
            SQL += " 			  FROM KOSMOS_PMPA.IPD_NEW_MASTER A                                                                                \r\n";
            SQL += " 			  WHERE 1=1                                                                                                        \r\n";
            //SQL += " 			    AND (A.JDATE    = TO_DATE('1900-01-01','YYYY-MM-DD')		-- 재원환자                                            \r\n";

            ////2020-05-18 김욱동 추가 보험심사팀 당일환자 조회가능 의뢰서 처리건
            //SQL += " 			    OR A.OUTDATE    <= TRUNC(SYSDATE))		-- 당일 퇴원환자                                            \r\n";
            SQL += " 			    AND A.ACTDATE IS NULL		                                            \r\n";

            if (string.IsNullOrEmpty(strWard) == false)
            {
                SQL += " 			    AND A.WARDCODE = " + ComFunc.covSqlstr(strWard, false);
            }

            if (string.IsNullOrEmpty(strPano) == false)
            {
                SQL += " 			    AND A.PANO     = " + ComFunc.covSqlstr(strPano, false);
            }

            if (string.IsNullOrEmpty(strDeptCode) == false)
            {
                SQL += " 			    AND A.DEPTCODE = " + ComFunc.covSqlstr(strDeptCode, false);
            }

            if (isOut == true)
            {
                SQL += " 				AND A.PANO IN ( SELECT PANO							-- 당일예약                                                \r\n";
                SQL += " 				                  FROM KOSMOS_PMPA.NUR_MASTER                                                                  \r\n";
                SQL += " 				                 WHERE (ROUTDATE =TRUNC(SYSDATE)                                                               \r\n";
                SQL += " 				                     OR OUTDATE = TRUNC(SYSDATE)                                                               \r\n";
                SQL += " 			                            )                                                                                      \r\n";
                SQL += " 			                    )                                                                                              \r\n";
            }
            SQL += " )                                                                                                                             \r\n";
            SQL += " ----------<<영상>>----------------                                                                                            \r\n";
            SQL += " SELECT                                                                                                                        \r\n";
            SQL += " 		T.ROOMCODE																	AS ROOMCODE	  	-- 01                      \r\n";
            SQL += "      ,  T.PANO      																AS PANO	      	-- 02                      \r\n";
            SQL += "      ,  T.SNAME     																AS SNAME	  	-- 03                      \r\n";
            SQL += "      ,  T.SEX       																AS SEX	      	-- 05                      \r\n";
            SQL += "      ,  T.AGE       																AS AGE	      	-- 04                      \r\n";
            SQL += "      ,  T.DEPTCODE  																AS DEPTCODE	  	-- 06                      \r\n";
            SQL += "      ,  T.DRNAME    																AS DRNAME	  	-- 07                      \r\n";
            SQL += "      ,  A.XCODE     																AS EXAM_CODE	-- 08                      \r\n";
            SQL += "      ,  B.XNAME     																AS EXAM_NAME	-- 09                      \r\n";
            SQL += "      ,  CASE WHEN A.XJONG ='C' AND A.GBEND = '1' THEN TO_CHAR( A.SEEKDATE,'MM-DD')                                            \r\n";
            SQL += "              WHEN A.GBSTS = '7' THEN TO_CHAR(A.SEEKDATE,'YYYY-MM-DD')                                                         \r\n";
            SQL += "              ELSE ''                                                                                                          \r\n";
            SQL += "          END    																	AS ACT_DATE	 	-- 10                      \r\n";
            SQL += "      , A.BDATE																		AS BDATE		-- 11                      \r\n";
            SQL += " 	 ,	A.CADEX_DEL																	AS CADEX_DEL 	-- 11                      \r\n";
            SQL += " 	 ,	DECODE(A.CADEX_DEL,'1', A.CDATE  , '')										AS CDATE     	-- 12                      \r\n";
            SQL += " 	 ,	DECODE(A.CADEX_DEL,'1', A.CSABUN , '')										AS CSABUN    	-- 13                      \r\n";
            SQL += " 	 ,	'01'																		AS EXAM_TYPE 	-- 14                      \r\n";
            SQL += " 	 ,	TO_CHAR(A.EXINFO)															AS WARTNO 	 	-- 15                      \r\n";
            SQL += " 	 ,	A.ORDERNO																	AS ORDERNO   	-- 16                      \r\n";
            SQL += " 	 ,	''																	                                                   \r\n";
            SQL += "   FROM  T                                                                                                                     \r\n";
            SQL += "       , KOSMOS_PMPA.XRAY_DETAIL A                                                                                             \r\n";
            SQL += "       , KOSMOS_PMPA.XRAY_CODE   B                                                                                             \r\n";
            SQL += "  WHERE 1=1                                                                                                                    \r\n";
            SQL += "    AND '1'	   		= " + ComFunc.covSqlstr(nChkExam[(int)enmSel_IPD_NEW_MASTER_EXAM_Param.XRAY].ToString(), false);
            SQL += "    AND A.PANO 		= T.PANO                                                                                                   \r\n";
            SQL += "    AND A.IPDOPD 	= 'I'                                                                                                      \r\n";
            SQL += "    AND A.XCODE NOT IN ('F12','G04009')                                                                                        \r\n";
            SQL += "    AND B.BUCODE 	!= '056101'  --심장초음파실                                                                                \r\n";
            SQL += "    AND A.XCODE		=	B.XCODE(+)                                                                                             \r\n";

            //SQL += "    AND A.BDATE  BETWEEN T.INDATE                                                                                              \r\n";
            //SQL += "                     AND T.OUTDATE                                                                                             \r\n";
            SQL += "    AND A.BDATE  > T.INDATE - 1                                                                                                \r\n";
            SQL += "    AND (T.OUTDATE IS NULL                                                                                                     \r\n";
            SQL += "     OR T.OUTDATE IS NOT NULL AND A.BDATE <= T.OUTDATE)                                                 \r\n";            

            if (nSearchType == 1)
            {
                SQL += "    AND A.BDATE  BETWEEN " + ComFunc.covSqlDate(strFDate, false);
                SQL += "                     AND " + ComFunc.covSqlDate(strTDate, false);

            }

            if (nActingType == 1)
            {
                SQL += " -- 미실시                                                                                                                 \r\n";
                SQL += "    AND (                                                                                                                  \r\n";
                SQL += "    			(A.GBSTS IS NULL OR A.GBSTS NOT IN ('7','D') )                                                             \r\n";
                SQL += "    		AND (A.CADEX_DEL IS NULL OR A.CADEX_DEL = '0' OR A.CADEX_DEL = 'L' OR A.CADEX_DEL = '' OR A.CADEX_DEL = ' ')   \r\n";
                SQL += "    	   )                                                                                                               \r\n";

            }
            else if (nActingType == 2)
            {
                SQL += "    AND (A.GBSTS  IN ('7') OR A.CADEX_DEL = '1')                                                                           \r\n";
            }

            SQL += " 	UNION ALL                                                                                                                  \r\n";
            SQL += " ----------<<ENDO>>----------------                                                                                            \r\n";
            SQL += " SELECT                                                                                                                        \r\n";
            SQL += " 		T.ROOMCODE										AS ROOMCODE	  	-- 01                                                  \r\n";
            SQL += "      ,  T.PANO      									AS PANO	      	-- 02                                                  \r\n";
            SQL += "      ,  T.SNAME     									AS SNAME	  	-- 03                                                  \r\n";
            SQL += "      ,  T.SEX       									AS SEX	      	-- 05                                                  \r\n";
            SQL += "      ,  T.AGE       									AS AGE	      	-- 04                                                  \r\n";
            SQL += "      ,  T.DEPTCODE  									AS DEPTCODE	  	-- 06                                                  \r\n";
            SQL += "      ,  T.DRNAME    									AS DRNAME	  	-- 07                                                  \r\n";
            SQL += "      ,  A.ORDERCODE 									AS EXAM_CODE	-- 08                                                  \r\n";
            SQL += "      ,  B.ORDERNAME 									AS EXAM_NAME	-- 09                                                  \r\n";
            SQL += "      ,  TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') 			AS ACT_DATE	 	-- 10                                                  \r\n";
            SQL += "      ,  A.BDATE										AS BDATE		-- 11                                                  \r\n";
            SQL += " 	 ,	A.CADEX_DEL										AS CADEX_DEL 	-- 11                                                  \r\n";
            SQL += " 	 ,	DECODE(A.CADEX_DEL,'1', A.CDATE,'')				AS CDATE     	-- 12                                                  \r\n";
            SQL += " 	 ,	DECODE(A.CADEX_DEL,'1', TO_CHAR(A.CSABUN),'')	AS CSABUN    	-- 13                                                  \r\n";
            SQL += " 	 ,	'02'											AS EXAM_TYPE 	-- 14/12                                               \r\n";
            SQL += " 	 ,	A.SEQNO||','||A.GBJOB							AS WARTNO 	 	-- 15/13                                               \r\n";
            SQL += " 	 ,	A.ORDERNO										AS ORDERNO   	-- 16/14                                               \r\n";
            SQL += " 	 ,	''																	                                                   \r\n";
            SQL += "   FROM  T                                                                                                                     \r\n";
            SQL += "       , KOSMOS_OCS.ENDO_JUPMST A                                                                                              \r\n";
            SQL += "       , KOSMOS_OCS.OCS_ORDERCODE B                                                                                            \r\n";
            SQL += "  WHERE 1=1                                                                                                                    \r\n";
            SQL += "    AND '1'	   		= " + ComFunc.covSqlstr(nChkExam[(int)enmSel_IPD_NEW_MASTER_EXAM_Param.ENDO].ToString(), false);
            SQL += "    AND A.PTNO 		= T.PANO                                                                                                   \r\n";
            SQL += "    AND A.GBIO	 	= 'I'                                                                                                      \r\n";
            SQL += "    AND A.ORDERCODE  = B.ORDERCODE(+)                                                                                          \r\n";
            SQL += "    AND a.GBSUNAP 	!= '*'                                                                                                     \r\n";
            SQL += " -- 설정값                                                                                                                     \r\n";
            //SQL += "    AND A.BDATE  BETWEEN T.INDATE                                                                                              \r\n";
            //SQL += "                     AND T.OUTDATE                                                                                             \r\n";
            SQL += "    AND A.BDATE  > T.INDATE - 1                                                                                                \r\n";
            SQL += "    AND (T.OUTDATE IS NULL                                                                                                     \r\n";
            SQL += "     OR T.OUTDATE IS NOT NULL AND A.BDATE <= T.OUTDATE)                                                 \r\n";

            if (nSearchType == 1)
            {
                SQL += "    AND A.BDATE  BETWEEN " + ComFunc.covSqlDate(strFDate, false);
                SQL += "                     AND " + ComFunc.covSqlDate(strTDate, false);

            }

            if (nActingType == 1)
            {

                SQL += " -- 미실시                                                                                                                  \r\n";
                SQL += "    AND (                                                                                                                   \r\n";
                SQL += "    			    A.GBSUNAP NOT IN  ('7','*')                                                                             \r\n";
                SQL += "    		AND (                                                                                                           \r\n";
                SQL += "    				A.CADEX_DEL IS NULL                                                                                     \r\n";
                SQL += "    		  	 OR A.CADEX_DEL = '0'                                                                                       \r\n";
                SQL += "    		  	 OR A.CADEX_DEL = 'L'                                                                                       \r\n";
                SQL += "    		  	 OR A.CADEX_DEL = ''                                                                                        \r\n";
                SQL += "    		  	 OR A.CADEX_DEL = ' '                                                                                       \r\n";
                SQL += "    		  	)                                                                                                           \r\n";
                SQL += "    		)                                                                                                               \r\n";
            }
            else if (nActingType == 2)
            {
                SQL += " -- 시행검사                                                                                                               \r\n";
                SQL += "    AND (A.GBSUNAP IN ('7') OR A.CADEX_DEL = '1')                                                                          \r\n";
            }

            SQL += " 	UNION ALL                                                                                                                  \r\n";
            SQL += " ----------<<기타검사>>----------------                                                                                        \r\n";
            SQL += " SELECT                                                                                                                        \r\n";
            SQL += " 		T.ROOMCODE										AS ROOMCODE	  	-- 01                                                  \r\n";
            SQL += "      ,  T.PANO      									AS PANO	      	-- 02                                                  \r\n";
            SQL += "      ,  T.SNAME     									AS SNAME	  	-- 03                                                  \r\n";
            SQL += "      ,  T.SEX       									AS SEX	      	-- 05                                                  \r\n";
            SQL += "      ,  T.AGE       									AS AGE	      	-- 04                                                  \r\n";
            SQL += "      ,  T.DEPTCODE  									AS DEPTCODE	  	-- 06                                                  \r\n";
            SQL += "      ,  T.DRNAME    									AS DRNAME	  	-- 07                                                  \r\n";
            SQL += "      ,  A.ORDERCODE 									AS EXAM_CODE	-- 08                                                  \r\n";
            SQL += "      ,  B.ORDERNAME 									AS EXAM_NAME	-- 09                                                  \r\n";
            SQL += "      ,  CASE WHEN A.GBJOB = '3' THEN TO_CHAR( A.RDATE,'YYYY-MM-DD')                                                           \r\n";
            SQL += "              ELSE ''                                                                                                          \r\n";
            SQL += "          END    										AS ACT_DATE	 	-- 10                                                  \r\n";
            SQL += "      ,  A.BDATE										AS BDATE		-- 11                                                  \r\n";
            SQL += " 	 ,	A.CADEX_DEL										AS CADEX_DEL 	-- 11                                                  \r\n";
            SQL += " 	 ,	DECODE(A.CADEX_DEL,'1', A.CDATE,'')				AS CDATE     	-- 12                                                  \r\n";
            SQL += " 	 ,	DECODE(A.CADEX_DEL,'1', TO_CHAR(A.CSABUN),'')	AS CSABUN    	-- 13                                                  \r\n";
            SQL += " 	 ,	'03' 											AS EXAM_TYPE 	-- 14/12                                               \r\n";
            SQL += " 	 ,	''				  								AS WARTNO 	 	-- 15/13                                               \r\n";
            SQL += " 	 ,	A.ORDERNO										AS ORDERNO   	-- 16/14                                               \r\n";
            SQL += " 	 ,	''																	                                                   \r\n";
            SQL += "   FROM  T                                                                                                                     \r\n";
            SQL += "       , KOSMOS_OCS.ETC_JUPMST 		A                                                                                          \r\n";
            SQL += "       , KOSMOS_OCS.OCS_ORDERCODE 	B                                                                                          \r\n";
            SQL += "  WHERE 1=1                                                                                                                    \r\n";
            SQL += "    AND '1'	   		= " + ComFunc.covSqlstr(nChkExam[(int)enmSel_IPD_NEW_MASTER_EXAM_Param.ETC].ToString(), false);
            SQL += "    AND A.PTNO 		= T.PANO                                                                                                   \r\n";
            SQL += "    AND A.GBIO	 	= 'I'                                                                                                      \r\n";
            SQL += "    AND A.ORDERCODE  = B.ORDERCODE(+)                                                                                           \r\n";
            SQL += "    AND A.GBJOB 	   != '9'                                                                                                  \r\n";
            SQL += " -- 설정값                                                                                                                     \r\n";
            //SQL += "    AND A.BDATE  BETWEEN T.INDATE                                                                                              \r\n";
            //SQL += "                     AND T.OUTDATE                                                                                             \r\n";
            SQL += "    AND A.BDATE  > T.INDATE - 1                                                                                                \r\n";
            SQL += "    AND (T.OUTDATE IS NULL                                                                                                     \r\n";
            SQL += "     OR T.OUTDATE IS NOT NULL AND A.BDATE <= T.OUTDATE)                                                 \r\n";

            if (nSearchType == 1)
            {
                SQL += "    AND A.BDATE  BETWEEN " + ComFunc.covSqlDate(strFDate, false);
                SQL += "                     AND " + ComFunc.covSqlDate(strTDate, false);

            }

            if (nActingType == 1)
            {
                SQL += " -- 미실시                                                                                                                 \r\n";
                SQL += "    AND (                                                                                                                \r\n";
                SQL += "    				A.GBJOB IN ('1','2')                                                                                   \r\n";
                SQL += " 		   AND (                                                                                                           \r\n";
                SQL += " 		   			A.CADEX_DEL IS NULL                                                                                    \r\n";
                SQL += " 		   		 OR A.CADEX_DEL = '0'                                                                                      \r\n";
                SQL += " 		   		 OR A.CADEX_DEL = 'L'                                                                                      \r\n";
                SQL += " 		   		 OR A.CADEX_DEL = ''                                                                                       \r\n";
                SQL += " 		   		 OR A.CADEX_DEL = ' '                                                                                      \r\n";
                SQL += " 		   		 )                                                                                                         \r\n";
                SQL += " 		)                                                                                                                  \r\n";
            }
            else if (nActingType == 2)
            {

                SQL += " -- 시행검사                                                                                                                   \r\n";
                SQL += "    AND (A.GBJOB IN ('3') OR A.CADEX_DEL = '1')                                                                                \r\n";
            }
            SQL += " 	UNION ALL                                                                                                                  \r\n";
            SQL += " ----------<<진단검사>>----------------                                                                                        \r\n";
            SQL += " SELECT                                                                                                                        \r\n";
            SQL += " 		DISTINCT                                                                                                               \r\n";
            SQL += " 		T.ROOMCODE										AS ROOMCODE	  	-- 01                                                  \r\n";
            SQL += "      ,  T.PANO      									AS PANO	      	-- 02                                                  \r\n";
            SQL += "      ,  T.SNAME     									AS SNAME	  	-- 03                                                  \r\n";
            SQL += "      ,  T.SEX       									AS SEX	      	-- 05                                                  \r\n";
            SQL += "      ,  T.AGE       									AS AGE	      	-- 04                                                  \r\n";
            SQL += "      ,  T.DEPTCODE  									AS DEPTCODE	  	-- 06                                                  \r\n";
            SQL += "      ,  T.DRNAME    									AS DRNAME	  	-- 07                                                  \r\n";
            SQL += "      ,  B.ORDERCODE 									AS EXAM_CODE	-- 08                                                  \r\n";
            SQL += "      ,  CASE WHEN INSTR(KOSMOS_OCS.FC_EXAM_RESULTC_EXAMNAME(A.SPECNO),'*배양(호기)*') > 0 THEN                                \r\n";
            SQL += "                         '균배양검사(검체:' || KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',A.SPECCODE,'N') || ')'                       \r\n";
            SQL += "              ELSE KOSMOS_OCS.FC_EXAM_RESULTC_EXAMNAME(A.SPECNO)                                                               \r\n";
            SQL += "              END 										AS EXAM_NAME	-- 09                                                  \r\n";
            SQL += "      ,  TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD') 			AS ACT_DATE	 	-- 10                                                  \r\n";
            SQL += "      ,  A.BDATE											AS BDATE		-- 11                                              \r\n";
            SQL += " 	 ,	NULL      										AS CADEX_DEL 	-- 11                                                  \r\n";
            SQL += " 	 ,	NULL											AS CDATE     	-- 12                                                  \r\n";
            SQL += " 	 ,	NULL											AS CSABUN    	-- 13                                                  \r\n";
            SQL += " 	 ,	'04'											AS EXAM_TYPE 	-- 14/12                                               \r\n";
            SQL += " 	 ,	A.SPECCODE		  								AS WARTNO 	 	-- 15/13                                               \r\n";
            SQL += " 	 ,	A.ORDERNO										AS ORDERNO   	-- 16/14                                               \r\n";
            SQL += " 	 ,	A.SPECNO                                        AS SPECNO       -- 17							                       \r\n";
            SQL += "   FROM  T                                                                                                                     \r\n";
            SQL += "       , KOSMOS_OCS.EXAM_SPECMST		A                                                                                      \r\n";
            SQL += "       , KOSMOS_OCS.OCS_IORDER	 	B                                                                                          \r\n";
            SQL += "  WHERE 1=1                                                                                                                    \r\n";
            SQL += "    AND '1'	   		= " + ComFunc.covSqlstr(nChkExam[(int)enmSel_IPD_NEW_MASTER_EXAM_Param.EXAM].ToString(), false);
            SQL += "    AND A.PANO 		= T.PANO                                                                                                   \r\n";
            SQL += "    AND A.PANO 		= B.PTNO                                                                                                   \r\n";
            SQL += "    AND A.BDATE 		= B.BDATE                                                                                              \r\n";
            SQL += "    AND A.ORDERNO 	= B.ORDERNO                                                                                                \r\n";
            SQL += "    AND(A.CANCEL IS NULL OR A.CANCEL = '' OR A.CANCEL = ' ')                                                                   \r\n";
            SQL += "    AND A.IPDOPD	 	= 'I'                                                                                                  \r\n";

            SQL += " -- 설정값                                                                                                                     \r\n";
            //SQL += "    AND A.BDATE  BETWEEN T.INDATE                                                                                              \r\n";
            //SQL += "                     AND T.OUTDATE                                                                                             \r\n";
            SQL += "    AND A.BDATE  > T.INDATE - 1                                                                                                \r\n";
            SQL += "    AND (T.OUTDATE IS NULL                                                                                                     \r\n";
            SQL += "     OR T.OUTDATE IS NOT NULL AND A.BDATE <= T.OUTDATE)                                                 \r\n";

            if (nSearchType == 1)
            {
                SQL += "    AND A.BDATE  BETWEEN " + ComFunc.covSqlDate(strFDate, false);
                SQL += "                     AND " + ComFunc.covSqlDate(strTDate, false);

            }

            if (nActingType == 1)
            {
                SQL += " -- 미실시                                                                                                                 \r\n";
                SQL += "    AND A.STATUS = '00'                                                                                                    \r\n";
            }
            else if (nActingType == 2)
            {
                SQL += " -- 시행검사                                                                                                               \r\n";
                SQL += "    AND A.STATUS IN ('01','02','03','04','05')                                                                             \r\n";
            }

            SQL += " UNION ALL                                                                                                                     \r\n";
            SQL += " ----------<<진검바코드출력>>----------------                                                                                  \r\n";
            SQL += " SELECT                                                                                                                        \r\n";
            SQL += " 		DISTINCT                                                                                                               \r\n";
            SQL += " 		T.ROOMCODE										AS ROOMCODE	  	-- 01                                                  \r\n";
            SQL += "      ,  T.PANO      									AS PANO	      	-- 02                                                  \r\n";
            SQL += "      ,  T.SNAME     									AS SNAME	  	-- 03                                                  \r\n";
            SQL += "      ,  T.SEX       									AS SEX	      	-- 05                                                  \r\n";
            SQL += "      ,  T.AGE       									AS AGE	      	-- 04                                                  \r\n";
            SQL += "      ,  T.DEPTCODE  									AS DEPTCODE	  	-- 06                                                  \r\n";
            SQL += "      ,  T.DRNAME    									AS DRNAME	  	-- 07                                                  \r\n";
            SQL += "      ,  A.MASTERCODE 									AS EXAM_CODE	-- 08                                                  \r\n";
            SQL += "      ,  B.EXAMNAME 									AS EXAM_NAME	-- 09                                                  \r\n";
            SQL += "      ,  NULL								 			AS ACT_DATE	 	-- 10                                                  \r\n";
            SQL += "      ,  A.BDATE										AS BDATE		-- 11                                                  \r\n";
            SQL += " 	 ,	NULL      										AS CADEX_DEL 	-- 11                                                  \r\n";
            SQL += " 	 ,	NULL											AS CDATE     	-- 12                                                  \r\n";
            SQL += " 	 ,	NULL											AS CSABUN    	-- 13                                                  \r\n";
            SQL += " 	 ,	'06'	 										AS EXAM_TYPE 	-- 14/12                                               \r\n";
            SQL += " 	 ,	''				  								AS WARTNO 	 	-- 15/13                                               \r\n";
            SQL += " 	 ,	A.ORDERNO										AS ORDERNO   	-- 16/14                                               \r\n";
            //SQL += " 	 ,	''																	                                                   \r\n";
            SQL += " 	 ,	A.SPECNO                                        AS SPECNO       -- 17							                       \r\n";
            SQL += "   FROM  T                                                                                                                     \r\n";
            SQL += "       , KOSMOS_OCS.EXAM_ORDER		A                                                                                          \r\n";
            SQL += "       , KOSMOS_OCS.EXAM_MASTER		B                                                                                          \r\n";
            SQL += "  WHERE 1=1                                                                                                                    \r\n";
            SQL += "    AND '1'	   		    = " + ComFunc.covSqlstr(nChkExam[(int)enmSel_IPD_NEW_MASTER_EXAM_Param.BARCODE].ToString(), false);
            SQL += "    AND A.PANO 		    = T.PANO                                                                                               \r\n";
            SQL += "    AND A.IPDOPD	 	= 'I'                                                                                                  \r\n";
            SQL += "    AND A.MASTERCODE    = B.MASTERCODE                                                                                         \r\n";
            SQL += " -- 설정값                                                                                                                     \r\n";
            //SQL += "    AND A.BDATE  BETWEEN T.INDATE                                                                                              \r\n";
            //SQL += "                     AND T.OUTDATE                                                                                             \r\n";
            SQL += "    AND A.BDATE  > T.INDATE - 1                                                                                                \r\n";
            SQL += "    AND (T.OUTDATE IS NULL                                                                                                     \r\n";
            SQL += "     OR T.OUTDATE IS NOT NULL AND A.BDATE <= T.OUTDATE)                                                 \r\n";

            if (nSearchType == 1)
            {
                SQL += "    AND A.BDATE  BETWEEN " + ComFunc.covSqlDate(strFDate, false);
                SQL += "                     AND " + ComFunc.covSqlDate(strTDate, false);

            }

            SQL += "    AND NVL(A.SPECNO,'A') = 'A'                                                                                               \r\n";
            SQL += "    AND A.WARD  = " + ComFunc.covSqlstr(strWard, false);
            SQL += "    ORDER BY EXAM_TYPE,ROOMCODE, ACT_DATE,PANO                                                                                \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

            return ds;
        }

        /// <summary>EXAM_SPECMST_WardWorkList</summary>
        /// <param name="strWard">병동</param>
        /// <param name="strDate">일자</param>
        /// <returns></returns>
        public DataSet sel_EXAM_SPECMST_WardWorkList(PsmhDb pDbCon, string strWard, string strDate)
        {
            DataSet ds = null;

            SQL = "";

            SQL += "  SELECT                                                                                                         \r\n";
            SQL += "  	      ROOM                                                                                                  \r\n";
            SQL += "     	, PANO                                                                                                  \r\n";
            SQL += "     	, SNAME                                                                                                 \r\n";
            SQL += "     	, SEX_AGE                                                                                               \r\n";
            SQL += "     	, DECODE(BC,'','','[' || BC || ']')   BC                                                                \r\n";
            SQL += "  	    , DECODE(PT,'','','[' || PT || ']')   PT                                                                \r\n";
            SQL += "  	    , DECODE(CBC,'','','[' || CBC || ']') CBC                                                               \r\n";
            SQL += "  	    , ''                                  REMARK                                                            \r\n";
            SQL += "    FROM (                                                                                                       \r\n";
            SQL += "  	     SELECT                                                                                                 \r\n";
            SQL += "  	     	   X.ROOM                                                                                           \r\n";
            SQL += "  		     , X.SNAME                                                                                          \r\n";
            SQL += "  		     , X.PANO                                                                                           \r\n";
            SQL += "  		     , X.SEX || '/' || X.AGE SEX_AGE                                                                    \r\n";
            SQL += "  			 , CASE WHEN X.TUBECODE IN ('011') 				 THEN  SUM( X.BCODEPRINT)                           \r\n";
            SQL += "  		            END										AS BC                                               \r\n";
            SQL += "  		     , CASE WHEN X.TUBECODE IN ('002','002A','002B') THEN  SUM( X.BCODEPRINT)                           \r\n";
            SQL += "  		            END										AS PT                                               \r\n";
            SQL += "  			 , CASE WHEN X.TUBECODE IN ('001') 				 THEN  SUM( X.BCODEPRINT)                           \r\n";
            SQL += "  		            END										AS CBC                                              \r\n";
            SQL += "  	     FROM (  SELECT A.ROOM                                                                                  \r\n";
            SQL += "  	     			  , A.SNAME                                                                                 \r\n";
            SQL += "  	     			  , A.PANO                                                                                  \r\n";
            SQL += "  	     			  , A.AGE                                                                                   \r\n";
            SQL += "  	     			  , A.SEX                                                                                   \r\n";
            SQL += "  	     			  , C.TUBECODE                                                                              \r\n";
            SQL += "  	     			  , A.SPECNO                                                                                \r\n";
            SQL += "  	     			  , C.BCODEPRINT                                                                            \r\n";
            SQL += "  	     		FROM " + ComNum.DB_MED + "EXAM_SPECMST 	A                                                       \r\n";
            SQL += "  	     		   , " + ComNum.DB_MED + "EXAM_RESULTC 	B                                           \r\n";
            SQL += "  	     		   , " + ComNum.DB_MED + "EXAM_MASTER 	C                                           \r\n";
            SQL += "  	     		WHERE 1=1                                                                                       \r\n";

            if (string.IsNullOrEmpty(strDate) == true)
            {
                SQL += "  	     		  AND A.BDATE      = TRUNC(SYSDATE)                                                             \r\n";
            }
            else
            {
                SQL += "  	     		  AND A.BDATE      = " + ComFunc.covSqlDate(strDate, false);
            }

            if (string.IsNullOrEmpty(strWard) == false)
            {
                SQL += "  	     		  AND A.WARD       = " + ComFunc.covSqlstr(strWard, false);
            }

            SQL += "  	     		  AND A.STATUS     = '00'                                                                       \r\n";
            SQL += "  	     		  AND A.SPECNO     = B.SPECNO                                                                   \r\n";
            SQL += "  	     		  AND B.MASTERCODE = C.MASTERCODE                                                               \r\n";
            SQL += "  	     		  AND C.TUBECODE 	IN ('001','002','002A','002B','011')                                        \r\n";
            SQL += "  	     		  AND C.SPECCODE 	IN ('010','011','012','012A','012B','013','013A','013B','015','016')        \r\n";
            SQL += "  	     	    GROUP BY A.ROOM, A.SNAME, A.PANO, A.AGE, A.SEX, C.TUBECODE , A.SPECNO , C.BCODEPRINT            \r\n";
            SQL += "  	    ) X                                                                                                     \r\n";
            SQL += "  	    GROUP BY X.ROOM, X.SNAME, X.PANO, X.AGE, X.SEX, X.TUBECODE                                              \r\n";
            SQL += "  	    ORDER BY X.ROOM, X.SNAME, X.PANO, X.AGE, X.SEX, X.TUBECODE                                              \r\n";
            SQL += "  )                                                                                                              \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

            return ds;
        }

        public DataTable sel_EXAM_SPECMST_WardWorkList_dt(PsmhDb pDbCon, string strWard, string strDate)
        {
            DataTable dt = null;

            SQL = "";

            SQL += "  SELECT                                                                                                         \r\n";
            SQL += "  	      X.ROOM, X.SNAME, X.PANO, X.AGE, X.SEX, X.TUBECODE TUBE, COUNT(*) CNT, SUM( X.BCODEPRINT) CNT2 FROM                                                                                               \r\n";
            SQL += "     (  SELECT A.ROOM, A.SNAME, A.PANO, A.AGE, A.SEX, C.TUBECODE,  A.SPECNO, C.BCODEPRINT                                                                                  \r\n";
            SQL += "     	 FROM KOSMOS_OCS.EXAM_SPECMST A, KOSMOS_OCS.EXAM_RESULTC B, KOSMOS_OCS.EXAM_MASTER C                                                                                       \r\n";
            SQL += "     	WHERE A.BDATE = TRUNC(SYSDATE)                                                                           \r\n";

            switch (strWard)
            {
                case "":
                    //SQL += "  	     		  AND  A.Ward >' '                                                                       \r\n";
                    break;

                case "MICU":
                    SQL += "  	     		  AND  A.Room='234'                                                                      \r\n";
                    break;

                case "SICU":
                    SQL += "  	     		  AND  A.Room='233'                                                                      \r\n";
                    break;

                case "ND":
                    SQL += "  	     		  AND  A.Ward IN ('ND','IQ')                                                             \r\n";
                    break;

                default:
                    SQL += "  	     		  AND  A.Ward ='" + strWard + "'                                                       \r\n";
                    break;
            }

            SQL += "  	     		  AND A.STATUS     = '00'                                                                       \r\n";
            SQL += "  	     		  AND A.SPECNO     = B.SPECNO                                                                   \r\n";
            SQL += "  	     		  AND B.MASTERCODE = C.MASTERCODE                                                               \r\n";
            SQL += "  	     		  AND C.TUBECODE 	IN ('001','002','002A','002B','011')                                        \r\n";
            SQL += "  	     		  AND C.SPECCODE 	IN ('010','011','012','012A','012B','013','013A','013B','015','016')        \r\n";
            SQL += "  	     	    GROUP BY A.ROOM, A.SNAME, A.PANO, A.AGE, A.SEX, C.TUBECODE , A.SPECNO , C.BCODEPRINT            \r\n";
            SQL += "  	    ) X                                                                                                     \r\n";
            SQL += "  	    GROUP BY X.ROOM, X.SNAME, X.PANO, X.AGE, X.SEX, X.TUBECODE                                              \r\n";
            SQL += "  	    ORDER BY X.ROOM, X.SNAME, X.PANO, X.AGE, X.SEX, X.TUBECODE                                              \r\n";            

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

        /// <summary>슬립여부확인</summary>
        /// <param name="strPano"></param>
        /// <param name="strDept"></param>
        /// <param name="strBdate"></param>
        /// <returns></returns>
        public DataSet sel_OPD_SLIP(PsmhDb pDbCon, string strPano, string strDept, string strBdate, string strIOGu = "")
        {
            DataSet ds = null;
            if(strIOGu == "I")
            {
                SQL = "";
                SQL += " SELECT                                                               \r\n";
                SQL += " 	  A.BUN                                                           \r\n";
                SQL += " 	, A.SUNEXT                                                        \r\n";
                SQL += " 	, A.SUCODE                                                        \r\n";
                SQL += " 	, SUM(A.QTY * A.NAL) QTY                                          \r\n";
                SQL += " 	, B.SUNAMEK                                                       \r\n";
                SQL += "  FROM KOSMOS_PMPA.IPD_NEW_SLIP A                                         \r\n";
                SQL += "      ,KOSMOS_PMPA.BAS_SUN  B                                         \r\n";
                SQL += " WHERE 1=1                                                            \r\n";
                SQL += "   AND A.PANO 		= " + ComFunc.covSqlstr(strPano, false);
                SQL += "   AND A.DEPTCODE 	= " + ComFunc.covSqlstr(strDept, false);
                SQL += "   AND A.BDATE 		= " + ComFunc.covSqlDate(strBdate, false);
                SQL += "   AND A.SUNEXT 	= B.SUNEXT(+)                                     \r\n";
                SQL += "   AND (                                                              \r\n";
                SQL += "   		(A.BUN >= '51'AND A.BUN <= '64')                              \r\n";
                SQL += "      OR A.SUNEXT IN ('BFLY','URB','24U-PACK','BM5001UY', 'CX450005','BK7100HR') \r\n";
                SQL += "       )                                                              \r\n";
                SQL += "   GROUP BY A.BUN,A.SUCODE, A.SUNEXT,B.SUNAMEK                        \r\n";
                SQL += "   ORDER BY A.BUN, A.SUCODE, A.SUNEXT                                 \r\n";
            }
            else
            {
                SQL = "";
                SQL += " SELECT                                                               \r\n";
                SQL += " 	  A.BUN                                                           \r\n";
                SQL += " 	, A.SUNEXT                                                        \r\n";
                SQL += " 	, A.SUCODE                                                        \r\n";
                SQL += " 	, SUM(A.QTY * A.NAL) QTY                                          \r\n";
                SQL += " 	, B.SUNAMEK                                                       \r\n";
                SQL += "  FROM KOSMOS_PMPA.OPD_SLIP A                                         \r\n";
                SQL += "      ,KOSMOS_PMPA.BAS_SUN  B                                         \r\n";
                SQL += " WHERE 1=1                                                            \r\n";
                SQL += "   AND A.PANO 		= " + ComFunc.covSqlstr(strPano, false);
                SQL += "   AND A.DEPTCODE 	= " + ComFunc.covSqlstr(strDept, false);
                SQL += "   AND A.BDATE 		= " + ComFunc.covSqlDate(strBdate, false);
                SQL += "   AND A.SUNEXT 	= B.SUNEXT(+)                                     \r\n";
                SQL += "   AND (                                                              \r\n";
                SQL += "   		(A.BUN >= '51'AND A.BUN <= '64')                              \r\n";
                SQL += "      OR A.SUNEXT IN ('BFLY','URB','24U-PACK','BM5001UY', 'CX450005','BK7100HR') \r\n";
                SQL += "       )                                                              \r\n";
                SQL += "   GROUP BY A.BUN,A.SUCODE, A.SUNEXT,B.SUNAMEK                        \r\n";
                SQL += "   ORDER BY A.BUN, A.SUCODE, A.SUNEXT                                 \r\n";
            }


            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

            return ds;
        }

        /// <summary>sel_OCS_OSPECIMAN</summary>
        /// <param name="strSlipNo">슬립</param>
        /// <returns></returns>
        public DataSet sel_OCS_OSPECIMAN(PsmhDb pDbCon, string strSlipNo)
        {
            DataSet ds = null;

            SQL = "";

            SQL += " SELECT                             \r\n";
            SQL += " 	     '' CHK                     \r\n";
            SQL += " 	   , SPECCODE                   \r\n";
            SQL += " 	   , SPECNAME                   \r\n";
            SQL += " 	   , ROWID                      \r\n";
            SQL += "   FROM KOSMOS_OCS.OCS_OSPECIMAN    \r\n";
            SQL += "  WHERE 1=1                         \r\n";
            SQL += "    AND SLIPNO = " + ComFunc.covSqlstr(strSlipNo, false);
            SQL += "  ORDER BY SPECCODE                 \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

            return ds;
        }

        public DataTable sel_EXAM_VERIFYUSE(PsmhDb pDbCon, string strJong)
        {
            DataTable dt = null;

            SQL = "";
            SQL = SQL + " SELECT                                                                 \r\n";
            SQL = SQL + "       JONG        -- 종류(1.검증/판독 2.추천 상용구)                   \r\n";
            SQL = SQL + "     , USECODE     -- 상용코드                                          \r\n";
            SQL = SQL + "     , USENAME     -- 상용구 내역                                       \r\n";
            SQL = SQL + "     , ROWID       -- 상용구 내역                                       \r\n";
            SQL = SQL + "  FROM KOSMOS_OCS.EXAM_VERIFYUSE /** 검사실 종합검증및판독 상용구*/     \r\n";
            SQL = SQL + " WHERE 1=1                                                              \r\n";
            //SQL = SQL + "   AND JONG = " + ComFunc.covSqlstr(strJong, false);
            SQL = SQL + " ORDER BY USECODE                                                       \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        public DataTable sel_EXAM_VERIFYUSE_CHK(PsmhDb pDbCon, string strJong, string strUseCode)
        {
            DataTable dt = null;

            SQL = "";
            SQL = SQL + " SELECT                                                                 \r\n";
            SQL = SQL + "       ROWID       -- 상용구 내역                                       \r\n";
            SQL = SQL + "  FROM KOSMOS_OCS.EXAM_VERIFYUSE /** 검사실 종합검증및판독 상용구*/     \r\n";
            SQL = SQL + " WHERE 1=1                                                              \r\n";
            SQL = SQL + "   AND JONG    = " + ComFunc.covSqlstr(strJong, false);
            SQL = SQL + "   AND USECODE = " + ComFunc.covSqlstr(strUseCode, false);

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        /// <summary>검사수정내역</summary>
        /// <param name="strFDate">시작일자</param>
        /// <param name="strTDate">종료일자</param>
        /// <param name="isGbn">수정/재전송</param>
        /// <returns></returns>
        public DataSet sel_EXAM_HISRESULTC(PsmhDb pDbCon, string strFDate, string strTDate, bool isGbn, string strWs, string strSpecNo, string strPano)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                                             \r\n";
            SQL += " 	     ''											    AS CHK                      \r\n";
            SQL += " 	   , A.PANO											AS PANO                     \r\n";
            SQL += " 	   , B.SNAME                                        AS SNAME                    \r\n";
            SQL += " 	   , A.SPECNO                                       AS SPECNO                   \r\n";
            SQL += " 	   , to_char(TO_NUMBER(A.SEQNO))                    AS SEQNO                    \r\n";
            SQL += " 	   , TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') 			AS RESULTDATE               \r\n";
            SQL += " 	   , TO_CHAR(A.RESULTDATE,'HH24:MI') 				AS RESULTTIME               \r\n";
            SQL += " 	   , CASE WHEN JOBGBN = '3' THEN KOSMOS_OCS.FC_BAS_PASS_NAME(A.JOBSABUN)        \r\n";
            SQL += " 	          ELSE KOSMOS_OCS.FC_BAS_PASS_NAME(A.RESULTSABUN)                       \r\n";
            SQL += " 	       END										 	AS RESULT_NAME              \r\n";
            SQL += " 	   , DECODE(A.JOBGBN,'1','전', '후')				AS GBN_NAME                 \r\n";
            SQL += " 	   , A.SUBCODE                                      AS SUBCODE                  \r\n";
            SQL += " 	   , C.EXAMNAME                                                                 \r\n";
            SQL += " 	   , A.RESULT                                                                   \r\n";
            SQL += " 	   , A.STATUS                                                                   \r\n";
            SQL += " 	   , DECODE(NVL(A.SAYU,'*'),'*','', A.SAYU || '.' || KOSMOS_OCS.FC_EXAM_SPECMST_NM('22',A.SAYU,'N'))AS SAYU_NM \r\n";
            SQL += " 	   , A.ROWID                                                                    \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_HISRESULTC A                                                 \r\n";
            SQL += "     , KOSMOS_PMPA.BAS_PATIENT    B                                                 \r\n";
            SQL += "     , KOSMOS_OCS.EXAM_MASTER     C                                                 \r\n";
            SQL += " WHERE 1=1                                                                          \r\n";

            if (string.IsNullOrEmpty(strPano) == false)
            {
                SQL += "   AND A.PANO   =" + ComFunc.covSqlstr(strPano, false);
            }

            if (string.IsNullOrEmpty(strSpecNo) == false)
            {
                SQL += "   AND A.SPECNO   =" + ComFunc.covSqlstr(strSpecNo, false);
            }

            if (string.IsNullOrEmpty(strWs) == false && strWs.IndexOf('*') < 0)
            {
                SQL += "   AND A.RESULTWS =" + ComFunc.covSqlstr(strWs, false);
            }

            SQL += "   AND A.JOBDATE >= " + ComFunc.covSqlDate(strFDate, false);
            SQL += "   AND A.JOBDATE <  " + ComFunc.covSqlDate(Convert.ToDateTime(strTDate).AddDays(1).ToString("yyyy-MM-dd"), false);

            if (isGbn == true)
            {
                SQL += "   AND A.JOBGBN IN ('1','2')                                                    \r\n";
            }
            else
            {
                SQL += "   AND A.JOBGBN IN ('3')                                                        \r\n";
            }
            SQL += "   AND A.PANO = B.PANO(+)                                                           \r\n";
            SQL += "   AND A.SUBCODE = C.MASTERCODE(+)                                                  \r\n";
            SQL += " ORDER BY A.JOBDATE,A.SPECNO,A.SEQNO,A.JOBSABUN,A.JOBGBN, B.SNAME, C.EXAMNAME       \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

            return ds;

        }
              
        public DataSet sel_EXAM_INFECT(PsmhDb pDbCon, string strFDate, string strTDate, bool isInf)
        {
            DataSet ds = null;

            SQL = "";

            SQL += " SELECT                                                        \r\n";             
            SQL += "         A.PANO                                                \r\n";
            SQL += " 	   , C.SNAME                                               \r\n";
            SQL += "       , A.SPECNO                                              \r\n";
            SQL += " 	   , A.RESULTDATE                                          \r\n";
            SQL += "       , D.USERNAME                                            \r\n";
            SQL += " 	   , B.EXAMFNAME                                           \r\n";
            SQL += " 	   , A.RESULT                                              \r\n";
            SQL += " 	   , A.STATUS                                              \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_INFECT  A                              \r\n";
            SQL += "      , KOSMOS_OCS.EXAM_MASTER  B                              \r\n";
            SQL += "      , KOSMOS_PMPA.BAS_PATIENT C                              \r\n";
            SQL += "      , KOSMOS_PMPA.BAS_USER    D                              \r\n";
            SQL += "    WHERE A.RESULTDATE 	>= " + ComFunc.covSqlDate(strFDate, false);
            SQL += "      AND A.RESULTDATE 	<  " + ComFunc.covSqlDate(strTDate, false);

            if (isInf == true)
            {
                SQL += "      AND A.FLAG 		= " + ComFunc.covSqlstr("OK", false);
            }

            SQL += "      AND A.MASTERCODE 	= B.MASTERCODE(+)                      \r\n";
            SQL += "      AND A.PANO 		= C.PANO(+)                            \r\n";
            SQL += "      AND A.RESULTSABUN = D.IDNUMBER(+)                    \r\n";
            SQL += "    ORDER BY RESULTDATE DESC,SPECNO                            \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

            return ds;
        }

        public DataSet sel_EXAM_RESULTC_CV(PsmhDb pDbCon, string strDrCode, string strFDate
                                         , string strTDate, string strWs, string strPtno,  enmSel_EXAM_RESULTC_CV_STATUE enmStatus)
        {
            DataSet ds = null;

            SQL = "";

            SQL += " SELECT                                                         \r\n";
            SQL += "        B.PANO                                                  \r\n";
            SQL += "      , B.SNAME                                                 \r\n";
            SQL += "      , B.WARD                                                  \r\n";
            SQL += "      , B.SPECNO                                                \r\n";
            SQL += "      , A.SUBCODE                                               \r\n";
            SQL += "      , C.EXAMFNAME                                             \r\n";
            SQL += "      , A.RESULT                                                \r\n";
            SQL += "      , A.UNIT                                                  \r\n";
            SQL += " 	 , TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI') RESULTDATE    \r\n";
            SQL += " 	 , KOSMOS_OCS.FC_BAS_PASS_NAME(A.RESULTSABUN) RESULTSABUN   \r\n";
            SQL += " 	 , DECODE(A.CHKDATE,NULL, '','True')		  AS CHK        \r\n";
            SQL += "      , A.CHKDATE                                               \r\n";
            SQL += "      , A.ROWID                                                 \r\n";
            SQL += " FROM KOSMOS_OCS.EXAM_RESULTC_CV A                              \r\n";
            SQL += "    , KOSMOS_OCS.EXAM_SPECMST 	B                               \r\n";
            SQL += "    , KOSMOS_OCS.EXAM_MASTER 	C                               \r\n";
            SQL += "  WHERE 1=1                                                     \r\n";
            SQL += "   AND A.JOBDATE BETWEEN " + ComFunc.covSqlDate(strFDate,false);
            SQL += " 				     AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "   AND A.SPECNO  = B.SPECNO(+)                                   \r\n";
            SQL += "   AND A.SUBCODE = C.MASTERCODE(+)                              \r\n";
            SQL += "   AND B.DRCODE  = " + ComFunc.covSqlstr(strDrCode, false);
            SQL += "   AND A.GBN 	 IN ('1')                                        \r\n";

            if (string.IsNullOrEmpty(strPtno) == false)
            {
                SQL += "   AND B.PANO  = " + ComFunc.covSqlstr(strPtno, false);
            }

            if (enmStatus == enmSel_EXAM_RESULTC_CV_STATUE.CONFIRM)
            {
                SQL += "   AND A.CHKDATE IS NOT NULL                                        \r\n";
            }
            else if (enmStatus == enmSel_EXAM_RESULTC_CV_STATUE.NONE)
            {
                SQL += "   AND A.CHKDATE IS NULL                                        \r\n";
            }

            if (string.IsNullOrEmpty(strWs) == false && strWs.IndexOf('*') < 0)
            {
                SQL += "   AND C.WSCODE1 = " + ComFunc.covSqlstr(strWs, false);
            }
            
            SQL += "   ORDER BY RESULTDATE ASC                                      \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

            return ds;
        }

        public DataTable sel_READ_ORDERSLIP(PsmhDb pDbCon)
        {
            DataTable dt = null;

            SQL = "";
            SQL = SQL + " SELECT                                                                 \r\n";
            SQL = SQL + "       SlipNo,OrderName                                                 \r\n";            
            SQL = SQL + "  FROM KOSMOS_OCS.OCS_ORDERCODE                                         \r\n";
            SQL = SQL + " WHERE 1=1                                                              \r\n";
            SQL = SQL + "   AND SlipNo >= '0010'                                                 \r\n";
            SQL = SQL + "   AND SlipNo <= '0050'                                                 \r\n";
            SQL = SQL + "   AND SeqNo = 0                                                        \r\n";
            SQL = SQL + "   AND Nal > 0                                                          \r\n";
            SQL = SQL + " ORDER BY Nal,SlipNo                                                    \r\n";

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

        public DataTable sel_READ_EXAMCODE(PsmhDb pDbCon)
        {
            DataTable dt = null;

            SQL = "";
            SQL = SQL + " SELECT                                                         \r\n";
            SQL = SQL + "       Code,Name                                                \r\n";
            SQL = SQL + "  FROM KOSMOS_OCS.EXAM_SPECODE                                  \r\n";
            SQL = SQL + " WHERE 1=1                                                      \r\n";
            SQL = SQL + "   AND Gubun = '14'                                             \r\n";
            SQL = SQL + "   AND DelDate IS NULL                                          \r\n";            
            SQL = SQL + " ORDER BY Code                                                  \r\n";

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

        public DataTable sel_READ_OSPECIMAN(PsmhDb pDbCon, string argSlipNo = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL = SQL + " SELECT                                                                 \r\n";
            SQL = SQL + "   SlipNo,SpecCode,SpecName,ROWID                                       \r\n";
            SQL = SQL + "   ,KOSMOS_OCS.FC_EXAM_SPECMST_NM('14', SPECCODE, 'N') AS FC_NAME       \r\n";
            
            SQL = SQL + "  FROM KOSMOS_OCS.OCS_OSPECIMAN                                         \r\n";
            SQL = SQL + " WHERE 1=1                                                              \r\n";
            if(argSlipNo == "")
            {                
                SQL = SQL + " ORDER BY SlipNo,SpecCode                                           \r\n";
            }
            else
            {
                SQL = SQL + "   AND SlipNo = '" + argSlipNo + "'                                 \r\n";
                SQL = SQL + " ORDER BY SpecCode                                                  \r\n";
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

        #region INSERT / UPDATE

        /// <summary>종합 검증 출력 매수 증가</summary>
        /// <param name="strRowId">ROWID</param>
        /// <returns></returns>
        public string up_EXAM_VERIFY(PsmhDb pDbCon, string strRowId, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "EXAM_VERIFY \r\n";
            SQL += "    SET PRINT = PRINT +1                 \r\n";
            SQL += "  WHERE 1=1                              \r\n";
            SQL += "    AND ROWID = " + ComFunc.covSqlstr(strRowId, false);


            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
      

        /// <summary>Glucose 입력</summary>
        /// <param name="strSpecNo"></param>
        /// <param name="strResult"></param>
        /// <param name="strSabun"></param>
        /// <param name="strRef"></param>
        /// <param name="strPanic"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string ins_EXAM_REULTC_CR59B(PsmhDb pDbCon, string strSpecNo, string strResult, string strSabun, string strRef,string strPanic, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "    INSERT INTO KOSMOS_OCS.EXAM_RESULTC (   \r\n";
            SQL += "          SPECNO                            \r\n";  
            SQL += "        , RESULTWS                          \r\n";
            SQL += "        , EQUCODE                           \r\n";
            SQL += "        , SEQNO                             \r\n";
            SQL += "        , PANO                              \r\n";
            SQL += "        , MASTERCODE                        \r\n";
            SQL += "        , SUBCODE                           \r\n";
            SQL += "        , STATUS                            \r\n";
            SQL += "        , RESULT                            \r\n";
            SQL += "        , RESULTDATE                        \r\n";
            SQL += "        , RESULTSABUN                       \r\n";
            SQL += "        , REFER                             \r\n";
            SQL += "        , PANIC                             \r\n";
            SQL += "        , DELTA                             \r\n";
            SQL += "        , UNIT                              \r\n";
            SQL += "            )                               \r\n";
			SQL += "        SELECT                                      \r\n";
            SQL += "            " + ComFunc.covSqlstr(strSpecNo, false);
			SQL += "            ,'C' 					AS RESULTWS     \r\n";
			SQL += "            , '009'  				AS EQUCODE      \r\n";
			SQL += "            , '001'  				AS SEQNO        \r\n";
			SQL += "            ,  PANO                                 \r\n";
			SQL += "            ,  MASTERCODE                           \r\n";
			SQL += "            ,  MASTERCODE                           \r\n";
			SQL += "            , 'V' 					AS STATUS       \r\n";
			SQL += "            , '" + strResult + "'   AS RESULT       \r\n";
			SQL += "            , SYSDATE 				AS RESULTDATE   \r\n";
			SQL += "            , '" + strSabun  + "'   AS ESULTSABUN     \r\n";
            SQL += "            , KOSMOS_OCS.FC_EXAM_MASTER_SUB_REF('2',MASTERCODE,SEX,AGE,'" + strResult + "')   AS REFER        \r\n";
            SQL += "            , KOSMOS_OCS.FC_EXAM_MASTER_PANIC(MASTERCODE,'" + strResult + "')          		  AS PANIC        \r\n";
            SQL += "            , '' 					AS DELTA        \r\n";
            SQL += "            , 'mg/dL' 				AS UNIT         \r\n";
            SQL += "        FROM KOSMOS_OCS.EXAM_ORDER                  \r\n";
            SQL += "        WHERE  1=1                                  \r\n";
            SQL += "            AND ROWID = ''                          \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }


        /// <summary>ins_EXAM_INFECT</summary>
        /// <param name="strOk"></param>
        /// <param name="strPano"></param>
        /// <param name="strRowId"></param>
        /// <returns></returns>
        public string ins_EXAM_INFECT(PsmhDb pDbCon, string strOk, string strPano, string strRowId, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "INSERT INTO " + ComNum.DB_MED + "EXAM_INFECT ( SPECNO , PANO, MASTERCODE, SUBCODE, STATUS, RESULT, RESULTDATE, RESULTSABUN, FLAG ) \r\n";
            SQL += " SELECT SPECNO , '" + strPano + "', MASTERCODE, SUBCODE, Status, Result, RESULTDATE, RESULTSABUN, '" + strOk + "'                   \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_RESULTC                                                                                           \r\n";
            SQL += "  WHERE ROWID = '" + strRowId + "' ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPano"></param>
        /// <param name="strSubCode"></param>
        /// <param name="TRS"></param>
        /// <param name="nRow"></param>
        /// <param name="strVDRL"></param>
        /// <param name="strHCV_IGG"></param>
        /// <param name="strHBS_AG"></param>
        /// <param name="strHIV"></param>
        /// <param name="strInfluAG"></param>
        /// <param name="strInfluAPR"></param>
        /// <returns></returns>
        public string ins_EXAM_INFECTMASTER(PsmhDb pDbCon, string strPano, string strSubCode, ref int nRow, string strVDRL = "", string strHCV_IGG = "", string strHBS_AG = "", string strHIV = "", string strInfluAG = "", string strInfluAPR = "")
        {
            string SqlErr = "";

            SQL = "";

            SQL += " MERGE INTO " + ComNum.DB_MED + "EXAM_INFECTMASTER M \r\n";
            SQL += "   USING DUAL                              \r\n";
            SQL += "   ON(M.PANO = '" + strPano + "')          \r\n";
            SQL += "   WHEN MATCHED THEN                       \r\n";

            switch (strSubCode.Trim())
            {
                case "SE01":
                case "SE01A":
                case "SI001":
                    SQL += "   UPDATE SET M.VDRL = " + ComFunc.covSqlstr(strVDRL, false);
                    break;
                case "SI07":
                case "SI07A":
                    SQL += "   UPDATE SET M.HBS_AG = " + ComFunc.covSqlstr(strHBS_AG, false);
                    break;
                case "SI081":
                case "SI08A":
                    SQL += "   UPDATE SET M.HCV_IGG = " + ComFunc.covSqlstr(strHCV_IGG, false);
                    break;
                case "SI12":
                case "SI12D":
                    SQL += "   UPDATE SET M.HIV = " + ComFunc.covSqlstr(strHIV, false);
                    break;
                case "SI14":
                    SQL += "   UPDATE SET M.INFLUAG = " + ComFunc.covSqlstr(strInfluAG, false);
                    break;
                case "GP23":
                    SQL += "   UPDATE SET M.INFLUAPR = " + ComFunc.covSqlstr(strInfluAPR, false);
                    break;
                default:
                    break;
            }

            SQL += "   WHEN NOT MATCHED THEN                                                                        \r\n";
            SQL += "   INSERT( M.PANO, M.VDRL, M.HCV_IGG, M.HBS_AG, M.HIV, M.INFLUAG, M.INFLUAPR)                  \r\n";
            SQL += "   VALUES(  \r\n";
            SQL += ComFunc.covSqlstr(strPano, false);
            SQL += ComFunc.covSqlstr(strVDRL, true);
            SQL += ComFunc.covSqlstr(strHCV_IGG, true);
            SQL += ComFunc.covSqlstr(strHBS_AG, true);
            SQL += ComFunc.covSqlstr(strHIV, true);
            SQL += ComFunc.covSqlstr(strInfluAG, true);
            SQL += ComFunc.covSqlstr(strInfluAPR, true);
            SQL += "   )";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref nRow, pDbCon);
            return SqlErr;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strRowId"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string del_EXAM_INFECT_MASTER(PsmhDb pDbCon, string strRowId, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "DELETE " + ComNum.DB_MED + "EXAM_INFECT_MASTER WHERE ROWID = '" + strRowId + "' ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPano"></param>
        /// <param name="strGubun"></param>
        /// <param name="strSpecNo"></param>
        /// <param name="strSubCode"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string ins_EXAM_INFECT_MASTER(PsmhDb pDbCon, string strPano, string strGubun, string strSpecNo, string strSubCode, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " INSERT INTO KOSMOS_OCS.EXAM_INFECT_MASTER(RDATE, PANO, GUBUN, SPECNO, EXNAME, CODE) \r\n";
            SQL += " SELECT TRUNC(SYSDATE)                                                               \r\n";
            SQL += "       , '" + strPano + "' AS PANO                                                 \r\n";
            SQL += "       , '" + strGubun + "' AS GUBUN                                                \r\n";
            SQL += "       , '" + strSpecNo + "' AS SPECNO                                               \r\n";

            switch (strSubCode.Trim())
            {
                case "SE01":
                case "SE01A":
                case "SI001":
                    SQL += "   ,'VDRL'                                                                  \r\n";
                    break;
                case "SI07":
                case "SI07A":
                    SQL += "   ,'Hepatitis B'                                                            \r\n";
                    break;
                case "SI081":
                case "SI08A":
                    SQL += "   ,'Hepatitis C'                                                            \r\n";
                    break;
                case "SI12":
                case "SI12D":
                    SQL += "   ,'HIV'                                                                   \r\n";
                    break;
                default:
                    break;
            }

            switch (strSubCode.Trim())
            {
                case "SE01":
                case "SE01A":
                case "SI001":
                    SQL += "   , KOSMOS_OCS.FC_BAS_BCODE_CODE('INFACT_격리상세질환', 'VDRL')                                                                  \r\n";
                    break;
                case "SI07":
                case "SI07A":
                    SQL += "   , KOSMOS_OCS.FC_BAS_BCODE_CODE('INFACT_격리상세질환', 'Hepatitis B')                                                            \r\n";
                    break;
                case "SI081":
                case "SI08A":
                    SQL += "   , KOSMOS_OCS.FC_BAS_BCODE_CODE('INFACT_격리상세질환','Hepatitis C')                                                            \r\n";
                    break;
                case "SI12":
                case "SI12D":
                    SQL += "   , KOSMOS_OCS.FC_BAS_BCODE_CODE('INFACT_격리상세질환','HIV')                                                                   \r\n";
                    break;
                default:
                    break;
            }

            SQL += "   FROM DUAL                                                                         \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slipNo"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string del_OCS_OSPECIMAN(PsmhDb pDbCon, string strRowId, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "DELETE " + ComNum.DB_MED + "OCS_OSPECIMAN WHERE ROWID = '" + strRowId + "' ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSlipNo"></param>
        /// <param name="strCode"></param>
        /// <param name="strName"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string ins_OCS_OSPECIMAN(PsmhDb pDbCon, string strSlipNo, string strCode, string strName, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "INSERT INTO KOSMOS_OCS.OCS_OSPECIMAN(SLIPNO, SPECCODE, SPECNAME) VALUES ( \r\n";
            SQL += ComFunc.covSqlstr(strSlipNo, false);
            SQL += ComFunc.covSqlstr(strCode, true);
            SQL += ComFunc.covSqlstr(strName, true);
            SQL += ")                                                                         \r\n";


            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string del_EXAM_VERIFYUSE(PsmhDb pDbCon, string strRowId, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "DELETE " + ComNum.DB_MED + "EXAM_VERIFYUSE WHERE ROWID = '" + strRowId + "' ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string ins_EXAM_VERIFYUSE(PsmhDb pDbCon, string strJONG, string strUseCode, string strUseName, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "INSERT INTO " + ComNum.DB_MED + "EXAM_VERIFYUSE (JONG,USECODE,USENAME) VALUES ( \r\n";
            SQL += ComFunc.covSqlstr(strJONG, false);
            SQL += ComFunc.covSqlstr(strUseCode, true);
            SQL += ComFunc.covSqlstr(strUseName, true);
            SQL += ")                                                                               \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string up_EXAM_VERIFYUSE(PsmhDb pDbCon, string strRowid, string strUseName, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "UPDATE " + ComNum.DB_MED + "EXAM_VERIFYUSE SET USENAME=" + ComFunc.covSqlstr(strUseName, false) + " \r\n";
            SQL += " WHERE ROWID = " + ComFunc.covSqlstr(strRowid, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string UP_EXAM_HISRESULTC(PsmhDb pDbCon, string strRowId, string strSayu, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "UPDATE " + ComNum.DB_MED + "EXAM_HISRESULTC SET SAYU =" + ComFunc.covSqlstr(strSayu, false) + " \r\n";
            SQL += " WHERE ROWID = " + ComFunc.covSqlstr(strRowId, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;
        }

        public string UP_EXAM_RESULTC_CV(PsmhDb pDbCon, string strRowId, string strSabun, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "UPDATE " + ComNum.DB_MED + "EXAM_RESULTC_CV \r\n";
            SQL += "   SET CHKDATE  = SYSDATE                   \r\n";
            SQL += "     , CHKSABUN = " + ComFunc.covSqlstr(strSabun, false);
            SQL += " WHERE ROWID = " + ComFunc.covSqlstr(strRowId, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;
        }



        #endregion


    }
}
