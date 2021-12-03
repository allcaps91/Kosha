using ComBase; //기본 클래스
using ComDbB; //DB연결
using ComSupLibB.Com;
using System.Data;

namespace ComSupLibB.SupIjrm
{
    /// <summary>
    /// Class Name : ComSupLibB.clsIjrmSQL
    /// File Name : clsIjrmSQL.cs
    /// Title or Description : 주사실 SQL 
    /// Author : 김홍록
    /// Create Date : 2017-07-11
    /// Update History : 
    /// </summary>
    public class clsComSupIjrmSQL : Com.clsMethod 
    {
        clsComSQL comSql = new clsComSQL();

        string SQL = string.Empty;

        public enum enmSel_ETC_JUSACODE     {          CHK,              JEPCODE,          JEPNAME,       GUBUN_NAME,       VCN_NAME  ,              DELDATE,   ROWID, MODI };
        public string[] sSel_ETC_JUSACODE = {        "A",           "주사코드",         "약품명",         "주사명",      " 백신종류",             "종료일자",  "ROWID", "MODI" };
        public int[] nSel_ETC_JUSACODE = { nCol_CHEK, nCol_EXCD, nCol_NAME, nCol_NAME, nCol_NAME, nCol_DATE    ,        5,       5};

        public enum enmSel_VACCINE_TLOTNO_S     {          ENDATE,             USED,              LOTNO,         LOPPERID,                UNIT,            VCNCOD,               VACODE,         VANAME  ,           VENDORCODE,    VENDORNAME,ROWID  };
        public string[] sSel_VACCINE_TLOTNO_S = {      "종료일자",       "본원사용",         "제조번호",       "유효기간",          "약품규격",        "백신종류",           "백신코드",         "백신명",           "제조코드",   "제조사명","ROWID" };
        public int[] nSel_VACCINE_TLOTNO_S = { nCol_DATE, nCol_CHEK, nCol_PANO, nCol_DATE, nCol_EXCD, nCol_SNAME, nCol_EXCD, nCol_NAME, nCol_EXCD, nCol_SNAME, 5 };

        public enum enmSel_VACCINE_TPATIENT { PANO, PPERID, PNAME, HPERID, RELA, PTELNO1, PTELNO2, PTELNO3, PLNO, PADD1, PADD2, EMAIL, MPHONE1, MPHONE2, MPHONE3, BABYGUBUN, BIRTHDAY, LAST_UPDATE, PINFOUSEDYON, ANAME, DOCTOR_NAME, HNAME, PPERID_OLD, PPERID2, HPERID2, PPERID_OLD2, GUBUN, PPERID_OLD3, JINGUBUN, NEXTCALL, ALLERGY, INJ_STS };

        public enum enmVACCINE_TTPREVEN_RCV { ACTION = 0 , PPERID = 1, BABYGUBUN = 2, HPERID = 3, WRITE_GCODE = 4,      VCODE = 5, VKIND = 6, VDATE = 7, HOSPI_NAME = 8,    PLNO = 9,   LOTNO = 10, LOPPERID = 11,  VACODE = 12,  VANAME = 13, VENDORCODE = 14, VENDORNAME = 15, UNIT = 16, AMETHOD = 17,       APART = 18,       VOLUMN = 19,    P_AGE = 20, P_MONTH = 21,     BILLYON = 22,     ANAME = 23, DOCTOR_NAME = 24, DEV_NAME = 25,    PPERID2 = 26, BABYGUBUN2 = 27, HPERID2 = 28, WRITE_GCODE2 = 29, VCODE2 = 30, VKIND2 = 31, VDATE2 = 32, FLAG = 33, STATUS = 34, STATUS2 = 35, NEXTCALL = 36, ALLERGY=37 };
        public enum enmVACCINE_TPATIENT_RCV { ACTION = 0 , PPERID = 1, BABYGUBUN = 2, HPERID = 3,  HOSPI_CODE = 4, HOSPI_NAME = 5, PNAME = 6, HNAME = 7,    PTELNO1 = 8, PTELNO2 = 9, PTELNO3 = 10,  MPHONE1 = 11, MPHONE2 = 12, MPHONE3 = 13,       PLNO = 14,      PADD2 = 15, RELA = 16,   EMAIL = 17, LAST_UPDATE = 18, PINFOUSEDYON = 19, BIRTHDAY = 20,   ANAME = 21, DOCTOR_NAME = 22, DEV_NAME2 = 23,       PADD1 = 24,  PPERID2 = 25, BABYGUBUN2 = 26,    HPERID2 = 27,    FLAG = 28, JINGUBUN = 29, NEXTCALL = 30, ALLERGY = 31 };

        public enum enmSel_OCS_IORDER_VCC     {     WARDCODE,      BDATE,       PTNO,      SNAME,  DEPTCODE,     DRNAME,   ALLERGY,      ROTA,     TDATE,        ORDERNM,    DOSNAME,       QTY,      NAL,    REMARK,     MSGYN,    MSG,     VCNCOD,         BI,    BI_NAME,  ROW_ID  };
        public string[] sSel_OCS_IORDER_VCC = {       "병동", "처방일자", "등록번호",     "성명",      "과",   "의사명",    "알러",    "로타",    "전송",       "처방명",     "용법",    "수량",   "날수",    "비고",    "메모", "메모", "벡신코드", "환자유형", "환자유형", "ROW_ID" };
        public int[] nSel_OCS_IORDER_VCC    = {    nCol_PANO,  nCol_DATE,  nCol_PANO, nCol_SNAME, nCol_DPCD, nCol_SNAME, nCol_SCHK, nCol_SCHK, nCol_SCHK, nCol_ORDERNAME,  nCol_SPNO, nCol_AGE , nCol_AGE, nCol_NAME, nCol_CHEK,      5, nCol_EXCD ,          5, nCol_SNAME,       5  };

        /// <summary>주사실 접수 파트</summary> 
        public enum enmOCS_OORDER_InjecMain_Part { ALL, IJRM, XRAY, FLU_LTD, CANCER};
        /// <summary>주사실 구분</summary>
        public enum enmOCS_OORDER_InjecMain_Type { RECIPT, ACTING, MULTI, WARD };
        
        ///// <summary>주사 액팅을 위한 세부 항목</summary>
        //public enum enm_ACT_TYPE { MULTI, RECIPT_MULTI, RECIPT_VACC, RECIPT_SING }

        /// <summary>주사실메인조회 쿼리 컬럼 물리명</summary>
        public enum enmSel_OCS_OORDER_InjecMain     {      BDATE,    ACTDATE,    ACTTIME,  INFECT_INFO, INFECT_IMG,    ALLERGY,     KK059,         PTNO,       SNAME,   SAMENAME,    SEX,    AGE, DEPTCODE,   DRNAME,      SUCODE,   ORDERNM,    DOSNAME,  GBNAL,    QTY,     ACTNAL,    NAL,   CONTENTS,  GBGROUP,   CHK,    REMARK,   CHASU,   CHKTTPR,  JEPCODE,     CHKJIN,     SUNAP,     CHKVACC,  CHKCZ394,   MSGYN,     MSG,    VCNCODE,  CHKIV,      JUMIN,     ROW_ID,    UNITNEW1,   UNITNEW2 ,   BI,   ORDERNO, CHKOLDFLU };
        public string[] sSel_OCS_OORDER_InjecMain = { "처방일자", "액팅일자", "액팅시간",   "감염정보",  "감염정보",    "알러",    "재료",   "등록번호",    "환자명",     "동명", "성별", "나이",     "과", "의사명",  "수가코드",  "처방명",     "용법", "구분", "수량", "액팅일수", "일수",   "용량",    "그룹" ,   "A",   "REMARK",  "차수",    "전송","JEPCODE",     "진찰",    "후불",      "필수",    "독감",  "메모",   "MSG", "벡신코드", "구분", "주민번호",    "ROWID",  "UNITNEW1", "UNITNEW2" , "BI", "ORDERNO", "CHKOLDFLU" };
        public int[] nSel_OCS_OORDER_InjecMain    = {  nCol_DATE,  nCol_DATE, nCol_PANO,            40,          40, nCol_SCHK, nCol_SCHK,    nCol_PANO,  nCol_SNAME,  nCol_SCHK,     20,     25,       25,       60,   nCol_EXCD,       180,  nCol_SPNO,     30,     30,        40,    30,      50,       25,         20,       180,      45, nCol_SCHK,       15,  nCol_SCHK, nCol_SCHK,   nCol_SCHK, nCol_SCHK,      15,       5,         40,     30,         80,          5,           5,          5 ,    5,         5,          5  };

        /// <summary>날수구분방법</summary>
        public enum enmInJecMain_GBNAL  {
            ///<summary>OCS_OORDER 시점에서 날수가 1일 처방</summary>
              S
            ///<summary>OCS_OORDER 시점에서 날수가 1일 이상 처방 (Multi 처방)</summary>
            , D
            ///<summary>ETC_JUSAMST 들어가 있는 Multi 처방 </summary>
            , M

        };

        public enum enmSel_ETC_JUSASUB_STT { TIT, INWON, QTY};

        public string[] sSel_ETC_JUSASUB_STT_DEPT = { "과"        , "인원" , "수량" };
        public string[] sSel_ETC_JUSASUB_STT_DR   = { "진료의사"  , "인원" , "수량" };
        public string[] sSel_ETC_JUSASUB_STT_DOS  = { "용법"      , "인원" , "수량" };
        public string[] sSel_ETC_JUSASUB_STT_ORDER = { "주사별"   , "인원" , "수량" };
        public string[] sSel_ETC_JUSASUB_STT_STT   = { "구분"     , "인원" , "수량" };

        public int[] nSel_ETC_JUSASUB_STT_1 = { 100, 45, 45, }; 
        public int[] nSel_ETC_JUSASUB_STT_2 = { 320, 60, 60, };

        public enum enmNoInject { NO, MG10060, REMARK_BLOOD, E7230AA , GCFLU0}; 

        public enum enmSel_OCS_OORDER_NoInject   {         BDATE,       PTNO,      SNAME,      SEX,      AGE,  DEPTCODE,     DRNAME,     SUCODE, ORDERNAME,    GBNAL,   DOSNAME,      QTY,      NAL,    REMARK, CHANGE  ,  HAPY_CAL,      JUMIN1,      JUMIN2,   JUMIN3,     TEL,       HPHONE,      JUSO,  ROWID};
        public string[] sSel_OCS_OORDER_NoInject  = { "처방일자", "등록번호", "환자성명",   "성별",   "나이",      "과", "의사성명", "수가코드",  "처방명",   "구분",    "용법",   "수량",   "날수",    "비고", "변경"  ,  "해피콜", "주민번호1", "주민번호2", "주민번호2", "전화번호", "핸드폰번호",    "주소", "ROWID"};
        public int[]    nSel_OCS_OORDER_NoInject  = { nCol_SNAME,  nCol_PANO, nCol_SNAME, nCol_SEX, nCol_AGE, nCol_DPCD, nCol_SNAME,  nCol_EXCD, nCol_NAME, nCol_AGE, nCol_SPNO, nCol_AGE, nCol_AGE, nCol_SPNO, 18, nCol_SPNO,  nCol_JUMN1,        nCol_JUMN1, nCol_JUMN1,   nCol_TEL,     nCol_TEL, nCol_JUSO,     5 };

        public enum enmsel_OCS_OORDER_OLDMAN     {      BDATE,     SUCODE,       PTNO,      SNAME,       JUMIN1,      JUMIN3,        TEL,       HPHONE,  JUSO};
        public string[] sSel_OCS_OORDER_OLDMAN = { "처방일자", "수가코드", "등록번호", "환자성명",  "주민번호1", "주민번호2", "전화번호", "핸드폰번호", "주소" };
        public int[] nSel_OCS_OORDER_OLDMAN    = {  nCol_DATE,  nCol_PANO,  nCol_PANO, nCol_SNAME, nCol_AGE, nCol_DPCD, nCol_SNAME, nCol_EXCD, nCol_NAME, nCol_AGE, nCol_SPNO, nCol_AGE, nCol_AGE, nCol_SPNO, 18, nCol_SPNO, nCol_JUMN1, nCol_JUMN1, nCol_JUMN1, nCol_TEL, nCol_TEL, nCol_JUSO, 5 };
        

        public string sel_OCS_OORDER_NoInject(PsmhDb pDbCon, string strPTNO)
        {
            DataSet ds = null;

            string strRETURN = "";

            SQL = "";
            SQL += "  SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD')|| ' ' || D.DOSNAME || ' ' || A.SUCODE || ' ' || C.ORDERNAME || ' ' || D.DOSNAME AS ORDER_INFO    \r\n";
            SQL += "	FROM KOSMOS_OCS.OCS_OORDER  	A                                                                                                   \r\n";
            SQL += "	   , KOSMOS_OCS.OCS_ODOSAGE 	D                                                                                                   \r\n";
            SQL += "	   , KOSMOS_OCS.OCS_ORDERCODE   C                                                                                                   \r\n";
            SQL += "	WHERE 1=1                                                                                                                           \r\n";
            SQL += "	  AND A.BDATE BETWEEN TO_DATE(TO_CHAR(SYSDATE,'YYYY')||'-01-01','YYYY-MM-DD')                                                       \r\n";
            SQL += "	                  AND TRUNC(SYSDATE)-1                                                                                                \r\n";
            SQL += "      AND A.PTNO  		= " + ComFunc.covSqlstr(strPTNO, false);
            SQL += "	  AND A.BUN 		= '20'                                                                                                          \r\n";
            SQL += "	  AND A.ORDERCODE 	= C.ORDERCODE                                                                                                   \r\n";
            SQL += "	  AND (RTRIM(A.DOSCODE) LIKE '9%1' OR RTRIM(A.DOSCODE) = '999999' OR RTRIM(A.DOSCODE) = '930116' OR RTRIM(A.DOSCODE) = '930115')    \r\n";
            SQL += "	  AND A.GBBOTH NOT IN ('3', 'J','5')                                                                                                \r\n";
            SQL += "	  AND A.GBSUNAP 	= '1'                                                                                                           \r\n";
            SQL += "	  AND A.NAL     	> 0                                                                                                             \r\n";
            SQL += "	  AND A.SEQNO   	> 0                                                                                                             \r\n";
            SQL += "	  AND D.DOSCODE 	= A.DOSCODE                                                                                                     \r\n";
            SQL += "    ORDER BY A.BDATE DESC                                                                                                               \r\n";
                        
            try
            {
                string SqlErr = clsDB.GetDataSetEx(ref ds, SQL,pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return "";
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }

            if (ComFunc.isDataSetNull(ds) == false)
            {
                strRETURN = "======================================================" + "\r\n";
                strRETURN += "주사 ACTING 하지 않은 내역이 있습니다." + "\r\n";

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strRETURN += ds.Tables[0].Rows[i]["ORDER_INFO"].ToString() + "\r\n";
                }

                strRETURN += "======================================================" + "\r\n";
                strRETURN += "주사 ACTTING 을 계속하시겠습니까?" + "\r\n";
                strRETURN += "======================================================" + "\r\n";
            }
            else
            {
                return "";
            }

            return strRETURN;
        }

        public DataSet sel_OCS_OORDER_OLDMAN(PsmhDb pDbCon, string strFDate, string strTDate, string strPTNO = "")
        {
            DataSet ds = null;

            SQL = "";

            SQL += " SELECT                                                                                                                                                             \r\n";
            SQL += "		  BDATE                                                                                                                                                     \r\n";
            SQL += "		, SUCODE                                                                                                                                                    \r\n";
            SQL += "		, PTNO                                                                                                                                                      \r\n";
            SQL += "		, SNAME                                                                                                                                                     \r\n";
            SQL += "		, JUMIN1                                                                                                                                                    \r\n";
            SQL += "		, JUMIN3                                                                                                                                                    \r\n";
            SQL += "		, TEL                                                                                                                                                       \r\n";
            SQL += "		, HPHONE                                                                                                                                                    \r\n";
            SQL += "		, JUSO                                                                                                                                                      \r\n";
            SQL += "  FROM (                                                                                                                                                            \r\n";
            SQL += "	  SELECT                                                                                                                                                        \r\n";
            SQL += "	       	     TO_CHAR(A.BDATE,'YYYY-MM-DD') 												    AS BDATE                                                            \r\n";
            SQL += "	       	     , A.SUCODE                                                                                                                                         \r\n";
            SQL += "	       	     , KOSMOS_OCS.FC_ETC_JUSACODE(A.SUCODE,'', 'N')									AS VCNCODE                                                          \r\n";
            SQL += "	       	     , TO_NUMBER(SUBSTR(TRUNC(SYSDATE),1,4)) - TO_NUMBER(SUBSTR(KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(A.PTNO),1,4)) AGE                                                             \r\n";
            SQL += "	             , A.PTNO                                                                                                                                           \r\n";
            SQL += "	        	 , B.SNAME                                                                                                                                          \r\n";
            SQL += "	        	 , F.JUMIN1                                                                                                                                         \r\n";
            SQL += "	        	 , F.JUMIN2                                                                                                                                         \r\n";
            SQL += "	        	 , F.JUMIN3                                                                                                                                         \r\n";
            SQL += "	        	 , F.TEL                                                                                                                                            \r\n";
            SQL += "	        	 , F.HPHONE                                                                                                                                         \r\n";
            SQL += "	        	 , KOSMOS_OCS.FC_BAS_PATIENT_JUSO(A.PTNO) 	  	 		                            AS JUSO                                                         \r\n";
            SQL += "	            , A.ROWID                                                                                                                                           \r\n";
            SQL += "	        FROM KOSMOS_OCS.OCS_OORDER 		 A                                                                                                                      \r\n";
            SQL += "	           , KOSMOS_PMPA.OPD_MASTER 	 B                                                                                                                      \r\n";
            SQL += "	           , KOSMOS_PMPA.BAS_DOCTOR 	 C                                                                                                                      \r\n";
            SQL += "	           , KOSMOS_OCS.OCS_ODOSAGE 	 D                                                                                                                      \r\n";
            SQL += "	           , KOSMOS_PMPA.BAS_PATIENT  	 F                                                                                                                      \r\n";
            SQL += "	           , KOSMOS_OCS.OCS_ORDERCODE 	 G                                                                                                                      \r\n";
            SQL += "	      WHERE 1=1                                                                                                                                                 \r\n";
            SQL += "            AND A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "        	   		        AND " + ComFunc.covSqlDate(strTDate, false);

            if (string.IsNullOrEmpty(strPTNO) == false)
            {
                SQL += "        AND A.PTNO    	= " + ComFunc.covSqlstr(strPTNO, false);
            }


            SQL += "	        AND A.PTNO    	= B.PANO                                                                                                                                \r\n";
            SQL += "	        AND A.BDATE   	= B.ACTDATE                                                                                                                             \r\n";
            SQL += "	        AND A.DEPTCODE 	= B.DEPTCODE                                                                                                                            \r\n";
            SQL += "	        AND A.DRCODE  	= C.DRCODE                                                                                                                              \r\n";
            SQL += "	        AND D.DOSCODE 	= A.DOSCODE                                                                                                                             \r\n";
            SQL += "	        AND A.PTNO	 	= F.PANO                                                                                                                                \r\n";
            SQL += "	        AND A.ORDERCODE = G.ORDERCODE                                                                                                                           \r\n";
            SQL += "	         AND A.GBBOTH = '3'                                                                                                                                     \r\n";
            SQL += "	        AND A.NAL     > 0                                                                                                                                       \r\n";
            SQL += "	        AND A.SEQNO   > 0                                                                                                                                       \r\n";
            SQL += "	        )                                                                                                                                                       \r\n";
            SQL += "  WHERE 1=1                                                                                                                                                         \r\n";
            if (string.IsNullOrEmpty(strPTNO) == true)
            {
                SQL += "    and  AGE > 64                                                                                                                                                      \r\n";
                SQL += "    AND VCNCODE IS NOT NULL     \r\n";

            }
            SQL += "    ORDER BY SNAME              \r\n";
            SQL += "                                \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSetEx(ref ds, SQL,pDbCon);
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

        public DataSet sel_OCS_OORDER_NoInject(PsmhDb pDbCon, enmNoInject enmNoINJECT_type
                                             , string strFDate, string strTDate, string strPTNO = "")
        {
            DataSet ds = null;

            SQL = "";

            SQL += "  SELECT                                                                                                  \r\n";
            SQL += "       	     TO_CHAR(A.BDATE,'YYYY-MM-DD') 												    AS BDATE      \r\n";
            SQL += "             , A.PTNO                                                                                     \r\n";
            SQL += "        	 , B.SNAME                                                                                    \r\n";
            SQL += "        	 , DECODE(B.SEX, 'M', '남', 'F', '여') 										    AS SEX        \r\n";
            SQL += "        	 , B.AGE                                                                                      \r\n";
            SQL += "        	 , A.DEPTCODE                                                                                 \r\n";
            SQL += "  		     , C.DRNAME                                                                                   \r\n";
            SQL += "        	 , A.SUCODE                                                                                   \r\n";
            SQL += "        	 , G.ORDERNAME || DECODE (NVL(G.ORDERNAMES,'NVL'),'NVL','',' ▶' || ORDERNAMES) AS ORDERNAME  \r\n";
            SQL += "        	 , DECODE(A.NAL, 1, 'S', 0, '0', 'D') 										    AS GBNAL      \r\n";
            if (enmNoINJECT_type == enmNoInject.E7230AA)
            {
                SQL += "        	 , ''                                                                                  \r\n";
            }
            else
            {
                SQL += "        	 , D.DOSNAME                                                                                  \r\n";
            }
            //SQL += "        	 , A.QTY                                                                                      \r\n";
            //실수량으로 변경
            SQL += "        	 , A.REALQTY                                                                                      \r\n";
            SQL += "        	 , A.NAL                                                                                      \r\n";
            SQL += "        	 , A.REMARK                                                                                   \r\n";
            SQL += "        	 , '' CHANGE                                                                                  \r\n";
            SQL += "             ,                                                                                            \r\n";
            SQL += "               (                                                                                          \r\n";
            SQL += "             	SELECT B.CODE ||'.' || B.NAME                                                             \r\n";
            SQL += "            	  FROM KOSMOS_PMPA.BAS_BCODE B                                                            \r\n";
            SQL += "            	 WHERE 1=1                                                                                \r\n";
            SQL += "            	   AND GUBUN = 'NUR_간호해피콜_GUBUN2'                                                    \r\n";
            SQL += "            	   AND CODE  =                                                                            \r\n";
            SQL += "            	                (                                                                         \r\n";
            SQL += "            	             		SELECT H.GUBUN2                                                       \r\n";
            SQL += "            	             		  FROM KOSMOS_PMPA.NUR_HAPPYCALL_OPD H                                \r\n";
            SQL += "            	             	     WHERE 1=1                                                            \r\n";
            SQL += "            	             	       AND H.GUBUN 	  = '02'                                              \r\n";
            SQL += "            	             	       AND H.BDATE 	  = A.BDATE                                           \r\n";
            SQL += "            	             	       AND H.PANO  	  = A.PTNO                                            \r\n";
            SQL += "            	             	       AND H.DEPTCODE = A.DEPTCODE                                        \r\n";
            SQL += "            	             	  )                                                                       \r\n";
            SQL += "                                                                                                          \r\n";
            SQL += "            	)																			AS HAPY_CAL   \r\n";
            SQL += "        	 , F.JUMIN1                                                                                   \r\n";
            SQL += "        	 , F.JUMIN2                                                                                   \r\n";
            SQL += "        	 , F.JUMIN3                                                                                   \r\n";
            SQL += "        	 , F.TEL                                                                                      \r\n";
            SQL += "        	 , F.HPHONE                                                                                   \r\n";
            SQL += "        	 , KOSMOS_OCS.FC_BAS_PATIENT_JUSO(A.PTNO) 			                            AS JUSO       \r\n";
            SQL += "            , A.ROWID                               										              \r\n";
            SQL += "        FROM KOSMOS_OCS.OCS_OORDER 		 A                                                                \r\n";
            SQL += "           , KOSMOS_PMPA.OPD_MASTER 	 B                                                                \r\n";
            SQL += "           , KOSMOS_PMPA.BAS_DOCTOR 	 C                                                                \r\n";
            if (enmNoINJECT_type != enmNoInject.E7230AA)
            {
                SQL += "           , KOSMOS_OCS.OCS_ODOSAGE 	 D                                                                \r\n";
            }
            
            SQL += "           , KOSMOS_PMPA.BAS_PATIENT  	 F                                                                \r\n";
            SQL += "           , KOSMOS_OCS.OCS_ORDERCODE 	 G                                                                \r\n";

            if (enmNoINJECT_type == enmNoInject.GCFLU0)
            {
                SQL += "               , (                                                                                    \r\n";   
           	    SQL += "                   SELECT PANO                                                                        \r\n";
		        SQL += "                      FROM KOSMOS_PMPA.VACCINE_TPATIENT  A                                             \r\n";
			    SQL += "                	 WHERE 1=1                                                                        \r\n";
                SQL += "                                                                                                      \r\n";
			    SQL += "                	   AND JINGUBUN IN ('3','4','5')                                                      \r\n";    //2020-10-12 '5' 추가
			    SQL += "                	   AND LAST_UPDATE = (                                                            \r\n";
			    SQL += "                	                      SELECT MAX(LAST_UPDATE)                                     \r\n";
			    SQL += "                	                        FROM KOSMOS_PMPA.VACCINE_TPATIENT B                       \r\n";
                SQL += "                	                        WHERE A.PANO = B.PANO                                     \r\n";
                SQL += "                						   )                                                          \r\n";
                SQL += "                  ) H                                                                                 \r\n";    
            }                                                                                                                
                                                                                                                             

            SQL += "      WHERE 1=1                                                                                           \r\n";

            //if (string.IsNullOrEmpty(strPTNO) == false)
            //{
            //    SQL += "        AND A.PTNO    	= " + ComFunc.covSqlstr(strPTNO, false);
            //}
            //else
            //{
                SQL += "        AND A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
                SQL += "        			    AND " + ComFunc.covSqlDate(strTDate, false);
            //}
            SQL += "        AND A.PTNO    	= B.PANO                                                                          \r\n";
            SQL += "        AND A.SUCODE NOT IN ('NSA-CT')                                                                    \r\n";
            SQL += "        AND A.BDATE   	= B.ACTDATE                                                                       \r\n";
            SQL += "        AND A.DEPTCODE 	= B.DEPTCODE                                                                      \r\n";
            SQL += "        AND A.DRCODE  	= C.DRCODE                                                                        \r\n";
            if (enmNoINJECT_type != enmNoInject.E7230AA)
            {
                SQL += "        AND D.DOSCODE 	= A.DOSCODE                                                                       \r\n";
            }
            SQL += "        AND A.PTNO	 	= F.PANO                                                                          \r\n";
            SQL += "        AND A.ORDERCODE = G.ORDERCODE                                                                     \r\n";
            

            if (enmNoINJECT_type == enmNoInject.NO)
            {
                SQL += "        AND A.BUN 	 	= '20'                                                                        \r\n";
                SQL += "        AND (                                                                                         \r\n";
                SQL += "        		   RTRIM(A.DOSCODE) LIKE '9%1'                                                        \r\n";
                SQL += "        	 	OR RTRIM(A.DOSCODE) = '999999'                                                        \r\n";
                SQL += "        	 	OR RTRIM(A.DOSCODE) = '930116'                                                        \r\n";
                SQL += "        	 	OR RTRIM(A.DOSCODE) = '930115'                                                        \r\n";
                SQL += "        	 	)                                                                                     \r\n";
                SQL += "        AND A.GBBOTH NOT IN ('3','J','5')                                                             \r\n";
                SQL += "        AND A.GBSUNAP = '1'                                                                           \r\n";
            }
            else if (enmNoINJECT_type == enmNoInject.MG10060)
            {
                SQL += "        AND A.ORDERCODE = 'MG10060'                                                                   \r\n";
                SQL += "        AND A.GBSUNAP = '1'                                                                           \r\n";
            }
            else if (enmNoINJECT_type == enmNoInject.E7230AA)
            {
                //SQL += "        AND A.ORDERCODE = 'E7230AA'                                                                   \r\n";
                SQL += "        AND A.ORDERCODE = 'AC302'                                                                     \r\n";
                SQL += "        AND A.GBSUNAP = '1'                                                                           \r\n";
            }
            else if (enmNoINJECT_type == enmNoInject.GCFLU0)
            {
                //2017.09.18.김홍록:노인독감
                SQL += "        AND TRIM(A.SUCODE)  = (                                                                      \r\n";
                SQL += "                       SELECT TRIM(CODE)                                                             \r\n";
                SQL += "                         FROM KOSMOS_PMPA.BAS_BCODE                                                  \r\n";
                SQL += "                        WHERE 1=1                                                                    \r\n";
                SQL += "                          AND GUBUN     = 'OCS_노인독감'                                              \r\n";
                SQL += "                          AND DELDATE IS NULL                                                        \r\n";
                SQL += "                     )                                                                               \r\n";
                //SQL += "                          AND A.BDATE BETWEEN JDATE                                                  \r\n";
                //SQL += "                                          AND DELDATE                                                \r\n";


                if (string.IsNullOrEmpty(strPTNO) == false)
                {
                    SQL += "        AND A.PTNO    	= " + ComFunc.covSqlstr(strPTNO, false);
                }

                SQL += "       AND A.PTNO      = H.PANO                                                                      \r\n";
                SQL += "       AND A.GBBOTH = '3'                                                                            \r\n";
                //SQL += "       AND A.SUCODE = 'SKFLU-0'                                                                     \r\n";

                //2017.09.19.김홍록: 필요 없을 듯 하여 막음.
                //SQL += "        AND A.PTNO IN ( SELECT J.PTNO                                                                \r\n";
                //SQL += "                          FROM KOSMOS_OCS.ETC_JUSASUB J                                              \r\n";
                //SQL += "                         WHERE 1=1                                                                   \r\n";

                //if (string.IsNullOrEmpty(strPTNO) == false)
                //{
                //    SQL += "                        AND J.PTNO    	= " + ComFunc.covSqlstr(strPTNO, false);
                //}
                //else
                //{
                //    SQL += "                        AND J.BDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
                //    SQL += "        			                  AND " + ComFunc.covSqlDate(strTDate, false);
                //}

                //SQL += "                           AND J.GBACT   = 'Y'                                                       \r\n";
                //SQL += "                           AND A.SUCODE  = (                                                         \r\n";
                //SQL += "                                          SELECT CODE                                                \r\n";
                //SQL += "                                            FROM KOSMOS_PMPA.BAS_BCODE                               \r\n";
                //SQL += "                                           WHERE 1=1                                                 \r\n";
                //SQL += "                                             AND GUBUN     = 'OCS_노인독감'                          \r\n";
                //SQL += "                                             AND J.BDATE BETWEEN JDATE                               \r\n";
                //SQL += "                                                             AND DELDATE                             \r\n";
                //SQL += "                     )                                                                               \r\n";
                //SQL += "                         GROUP BY PTNO )                                                             \r\n";
            }
            else if (enmNoINJECT_type == enmNoInject.REMARK_BLOOD)
            {
                SQL += "        AND A.GBSUNAP   = '1'                                                                         \r\n";
                SQL += "        AND A.BUN 	 	= '20'                                                                        \r\n";
                SQL += "        AND A.REMARK    LIKE '%수혈%'                                                                 \r\n";
            }

            //SQL += "        AND A.GBSUNAP = '1'                                                                              \r\n";
            SQL += "        AND A.NAL     > 0                                                                                \r\n";
            SQL += "        AND A.SEQNO   > 0                                                                                \r\n";
            //SQL += "     ORDER BY A.BDATE, A.PTNO                                                                             \r\n";
            SQL += "     ORDER BY B.SNAME                                                                             \r\n";
            try
            {
                string SqlErr = clsDB.GetDataSetEx(ref ds, SQL,pDbCon); 
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

        public DataSet sel_ETC_JUSASUB_STT2(PsmhDb pDbCon, enmOCS_OORDER_InjecMain_Part ePART, string strFDate, string strTDate)
        {
            DataSet ds = null;

            SQL = "";                       
            SQL += "  WITH T AS(                                                                                                                        \r\n";
            SQL += "  			SELECT                                                                                                                  \r\n";
            SQL += "  				   0 SEQ                                                                                                            \r\n";
            SQL += "  				 , DECODE(SUBSTR(DOSCODE, 2,1), '1', 'IM', '2', 'IV', '3', 'IV(M)','5', 'SC') AS TIT                                \r\n";
            SQL += "  			     , COUNT(PTNO) 		AS INWON                                                                                        \r\n";
            SQL += "  			     , SUM(QTY * NAL) 	AS QTY                                                                                          \r\n";
            SQL += "  			  FROM KOSMOS_OCS.ETC_JUSASUB   A                                                                                       \r\n";
            SQL += "  			 WHERE 1=1                                                                                                              \r\n";
            SQL += "               AND A.ACTDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                                 AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "  			   AND RTRIM(DOSCODE) IN ('910101', '910201', '920101', '920201', '950101', '950201', '930101','930201')                \r\n";
            SQL += "  			   AND GBACT = 'Y'                                                                                                      \r\n";

            if (ePART == enmOCS_OORDER_InjecMain_Part.IJRM)
            {
                SQL += "   AND A.PART     IN ('1','4') -- 본관 + 회사접종                                                                                 \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.XRAY)
            {
                SQL += "   AND A.PART     IN ('3')  -- 영사의학과                                                                                         \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.FLU_LTD)
            {
                SQL += "   AND A.PART     IN ('4')  -- 산업채                                                                                           \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.CANCER)
            {
                SQL += "   AND A.PART     IN ('5')  -- 항암주사실                                                                                           \r\n";
            }

            SQL += "  			 GROUP BY DECODE(SUBSTR(DOSCODE, 2,1), '1', 'IM', '2', 'IV', '3', 'IV(M)','5', 'SC')                                    \r\n";
            SQL += "  			 UNION ALL                                                                                                              \r\n";
            SQL += "  			 SELECT 1 SEQ                                                                                                           \r\n";
            SQL += "  			 	  , TRIM(SUCODE) || '/' || KOSMOS_OCS.FC_BAS_SUN_SUNAMEK(SUCODE) AS TIT                                             \r\n";
            SQL += "  			      , COUNT(PTNO) AS INWON                                                                                            \r\n";
            SQL += "  			      , SUM(QTY) 	AS QTY                                                                                              \r\n";
            SQL += "  			   FROM KOSMOS_OCS.ETC_JUSASUB    A                                                                                     \r\n";
            SQL += "  			  WHERE 1=1                                                                                                             \r\n";
            SQL += "               AND A.ACTDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                                 AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "  			    AND SUCODE IN                                                                                                       \r\n";
            SQL += "  			    				(                                                                                                   \r\n";
            SQL += "  			     				SELECT TCODE                                                                                        \r\n";
            SQL += "  			     				  FROM KOSMOS_OCS.OCS_OPDTONGCODE                                                                   \r\n";
            SQL += "  			     				 WHERE CANCEL = 'Y'                                                                                 \r\n";
            SQL += "  			     				   AND GUBUN = '01'                                                                                 \r\n";
            SQL += "  			    				)                                                                                                   \r\n";
            SQL += "  			   AND GBACT = 'Y'                                                                                                      \r\n";
            if (ePART == enmOCS_OORDER_InjecMain_Part.IJRM)
            {
                SQL += "   AND A.PART     IN ('1','4') -- 본관 + 회사접종                                                                               \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.XRAY)
            {
                SQL += "   AND A.PART     IN ('3')  -- 영사의학과                                                                                       \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.FLU_LTD)
            {
                SQL += "   AND A.PART     IN ('4')  -- 산업채                                                                                           \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.CANCER)
            {
                SQL += "   AND A.PART     IN ('5')  -- 항암주사실                                                                                           \r\n";
            }
            SQL += "  			 GROUP BY SUCODE                                                                                                        \r\n";
            SQL += "  			 UNION ALL                                                                                                              \r\n";
            SQL += "  		    SELECT                                                                                                                  \r\n";
            SQL += "  		    	 2 SEQ                                                                                                              \r\n";
            SQL += "  		    	, '기타(수가코드: ' || TRIM(SUCODE) || '//' || KOSMOS_OCS.FC_BAS_SUN_SUNAMEK(SUCODE) || ')' TIT                     \r\n";
            SQL += "  		    	, COUNT(PTNO) INWON                                                                                                 \r\n";
            SQL += "  		    	, SUM(QTY * NAL) QTY                                                                                                \r\n";
            SQL += "  		     FROM KOSMOS_OCS.ETC_JUSASUB     A                                                                                      \r\n";
            SQL += "  		    WHERE 1=1                                                                                                               \r\n";
            SQL += "               AND A.ACTDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                                 AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "  		      AND RTRIM(DOSCODE) NOT IN ('910101', '910201', '920101', '920201', '950101', '950201', '930101', '930201')            \r\n";
            SQL += "  		      AND GBACT = 'Y'                                                                                                       \r\n";

            if (ePART == enmOCS_OORDER_InjecMain_Part.IJRM)
            {
                SQL += "   AND A.PART     IN ('1','4') -- 본관 + 회사접종                                                                                 \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.XRAY)
            {
                SQL += "   AND A.PART     IN ('3')  -- 영사의학과                                                                                         \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.FLU_LTD)
            {
                SQL += "   AND A.PART     IN ('4')  -- 산업채                                                                                         \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.CANCER)
            {
                SQL += "   AND A.PART     IN ('5')  -- 항암주사실                                                                                     \r\n";
            }
            SQL += "  		      AND SUCODE NOT IN                                                                                                     \r\n";
            SQL += "  							   (                                                                                                    \r\n";
            SQL += "  			          				SELECT TCODE                                                                                    \r\n";
            SQL += "  			          				  FROM KOSMOS_OCS.OCS_OPDTONGCODE                                                               \r\n";
            SQL += "  			          				 WHERE CANCEL = 'Y'                                                                             \r\n";
            SQL += "  			          				   AND GUBUN = '01'                                                                             \r\n";
            SQL += "  		          				)                                                                                                   \r\n";
            SQL += "                                                                                                                                    \r\n";
            SQL += "  		       GROUP BY SUCODE                                                                                                      \r\n";
            SQL += "  )                                                                                                                                 \r\n";
            SQL += "  SELECT DECODE(GROUPING_ID(TIT),0,TIT,'합계') 					AS TIT                                                              \r\n";
            SQL += "       , TO_CHAR(SUM(INWON),'999,999,999') 						AS INWON                                                            \r\n";
            SQL += "       , REPLACE(TO_CHAR(SUM(QTY),'FM999,999,999.0'),'.0','')		AS QTY                                                          \r\n";
            SQL += "    FROM T                                                                                                                          \r\n";
            SQL += "    GROUP BY ROLLUP(TIT)                                                                                                            \r\n";
            SQL += "    ORDER BY MAX(SEQ),TIT                                                                                                           \r\n";

            try                                                                                                                             
            {                                                                                                                               
                string SqlErr = clsDB.GetDataSetEx(ref ds, SQL,pDbCon);
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

        //아래 sel_ETC_JUSASUB_STT2번으로 사용안함 - 2018-09-07
        public DataSet sel_ETC_JUSASUB_STT(PsmhDb pDbCon, enmOCS_OORDER_InjecMain_Part ePART, string strFDate, string strTDate)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                                                                                             \r\n";
            SQL += " 	'DEPT'					                                   AS GUBUN                                                             \r\n";
            SQL += "     , DECODE(GROUPING_ID(B.DEPTNAMEK),0,B.DEPTNAMEK,1,'합계') AS TIT                                                               \r\n";
            SQL += "     , TO_CHAR(COUNT(A.PTNO)				,'999,999,999')    AS INWON                                                             \r\n";
            SQL += "     , REPLACE(TO_CHAR(SUM(A.QTY * A.NAL)  	,'FM999,999,999.0'),'.0','')  	AS QTY                                                  \r\n";
            SQL += "  FROM KOSMOS_OCS.ETC_JUSASUB     A                                                                                                 \r\n";
            SQL += "     , KOSMOS_PMPA.BAS_CLINICDEPT B                                                                                                 \r\n";
            SQL += " WHERE 1=1                                                                                                                          \r\n";
            SQL += "   AND A.ACTDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                     AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "   AND A.GBACT = 'Y'                                                                                                                \r\n";
            SQL += "   AND A.DEPTCODE = B.DEPTCODE                                                                                                      \r\n";

            if (ePART == enmOCS_OORDER_InjecMain_Part.IJRM)
            {
                SQL += "   AND A.PART     IN ('1','4') -- 본관 + 회사접종                                                                                 \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.XRAY)
            {
                SQL += "   AND A.PART     IN ('3')  -- 영사의학과                                                                                         \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.FLU_LTD)
            {
                SQL += "   AND A.PART     IN ('4')  -- 산업채                                                                                         \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.CANCER)
            {
                SQL += "   AND A.PART     IN ('5')  --  항암주사실                                                                                     \r\n";
            }
            SQL += " GROUP BY ROLLUP(B.DEPTNAMEK)                                                                                                       \r\n";
            SQL += " UNION ALL                                                                                                                          \r\n";
            SQL += " SELECT                                                                                                                             \r\n";
            SQL += " 	  'DR'				 AS GUBUN                                                                                                   \r\n";
            SQL += " 	, DECODE(GROUPING_ID(B.DRNAME),0,B.DRNAME,1,'합계') AS TIT                                                                      \r\n";
            SQL += "     , TO_CHAR(COUNT(A.PTNO)				,'999,999,999')    AS INWON                                                             \r\n";
            SQL += "     , REPLACE(TO_CHAR(SUM(A.QTY * A.NAL)  	,'FM999,999,999.0'),'.0','')  	AS QTY                                                  \r\n";
            SQL += "  FROM KOSMOS_OCS.ETC_JUSASUB 	A                                                                                                   \r\n";
            SQL += "     , KOSMOS_PMPA.BAS_DOCTOR 	B                                                                                                   \r\n";
            SQL += " WHERE 1=1                                                                                                                          \r\n";
            SQL += "   AND A.ACTDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                     AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "   AND A.GBACT = 'Y'                                                                                                                \r\n";
            SQL += "   AND A.DRCODE = B.DRCODE                                                                                                          \r\n";

            if (ePART == enmOCS_OORDER_InjecMain_Part.IJRM)
            {
                SQL += "   AND A.PART     IN ('1','4') -- 본관 + 회사접종                                                                                 \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.XRAY)
            {
                SQL += "   AND A.PART     IN ('3')  -- 영사의학과                                                                                         \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.FLU_LTD)
            {
                SQL += "   AND A.PART     IN ('4')  -- 산업채                                                                                         \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.CANCER)
            {
                SQL += "   AND A.PART     IN ('5')  -- 항암주사실                                                                                        \r\n";
            }

            SQL += " GROUP BY ROLLUP(B.DRNAME)                                                                                                          \r\n";
            SQL += " UNION ALL                                                                                                                          \r\n";
            SQL += " SELECT                                                                                                                             \r\n";
            SQL += " 	   'ORDER'  				AS GUBUN                                                                                            \r\n";
            SQL += "      , DECODE (                                                                                                                    \r\n";
            SQL += "      			GROUPING_ID(DECODE(NVL(B.ORDERNAMES,'NULL'),'NULL', B.ORDERNAME, B.ORDERNAME || ':' || B.ORDERNAMES)), 0 ,          \r\n";
            SQL += " 				            DECODE(NVL(B.ORDERNAMES,'NULL'),'NULL', B.ORDERNAME, B.ORDERNAME || ':' || B.ORDERNAMES) , 1 , '합계'   \r\n";
            SQL += "               )	 				AS TIT                                                                                          \r\n";
            SQL += "     , TO_CHAR(COUNT(A.PTNO)				,'999,999,999')    AS INWON                                                             \r\n";
            SQL += "     , REPLACE(TO_CHAR(SUM(A.QTY * A.NAL)  	,'FM999,999,999.0'),'.0','')  	AS QTY                                                   \r\n";
            SQL += "   FROM KOSMOS_OCS.ETC_JUSASUB A                                                                                                    \r\n";
            SQL += "      , KOSMOS_OCS.OCS_ORDERCODE B                                                                                                             \r\n";
            SQL += "  WHERE 1=1                                                                                                                         \r\n";
            SQL += "   AND A.ACTDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                     AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "    AND A.GBACT = 'Y'                                                                                                               \r\n";
            SQL += "    AND A.ORDERCODE = B.ORDERCODE                                                                                                   \r\n";

            if (ePART == enmOCS_OORDER_InjecMain_Part.IJRM)
            {
                SQL += "   AND A.PART     IN ('1','4') -- 본관 + 회사접종                                                                                 \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.XRAY)
            {
                SQL += "   AND A.PART     IN ('3')  -- 영사의학과                                                                                         \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.FLU_LTD)
            {
                SQL += "   AND A.PART     IN ('4')  -- 산업채                                                                                         \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.CANCER)
            {
                SQL += "   AND A.PART     IN ('5')  -- 항암주사실                                                                                         \r\n";
            }

            SQL += "  GROUP BY ROLLUP(DECODE(NVL(B.ORDERNAMES,'NULL'),'NULL', B.ORDERNAME, B.ORDERNAME || ':' || B.ORDERNAMES))                         \r\n";
            SQL += "  UNION ALL                                                                                                                         \r\n";
            SQL += "  SELECT                                                                                                                            \r\n";
            SQL += "         'DOS'				 AS GUBUN                                                                                               \r\n";
            SQL += "       , DECODE(GROUPING_ID(B.DOSNAME),0,B.DOSNAME,'합계') 			 AS TIT                                                         \r\n";
            SQL += "     , TO_CHAR(COUNT(A.PTNO)				,'999,999,999')    AS INWON                                                             \r\n";
            SQL += "     , REPLACE(TO_CHAR(SUM(A.QTY * A.NAL)  	,'FM999,999,999.0'),'.0','')  	AS QTY                                                  \r\n";
            SQL += "   FROM KOSMOS_OCS.ETC_JUSASUB A                                                                                                    \r\n";
            SQL += "      , KOSMOS_OCS.OCS_ODOSAGE B                                                                                                    \r\n";
            SQL += "  WHERE 1=1                                                                                                                         \r\n";
            SQL += "   AND A.ACTDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                     AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "    AND A.GBACT = 'Y'                                                                                                               \r\n";
            SQL += "    AND A.DOSCODE = B.DOSCODE(+)                                                                                                    \r\n";

            if (ePART == enmOCS_OORDER_InjecMain_Part.IJRM)
            {
                SQL += "   AND A.PART     IN ('1','4') -- 본관 + 회사접종                                                                                 \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.XRAY)
            {
                SQL += "   AND A.PART     IN ('3')  -- 영사의학과                                                                                         \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.FLU_LTD)
            {
                SQL += "   AND A.PART     IN ('4')  -- 산업채                                                                                         \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.CANCER)
            {
                SQL += "   AND A.PART     IN ('5')  -- 항암주사실                                                                                         \r\n";
            }
            SQL += "  GROUP BY ROLLUP( B.DOSNAME)                                                                                                       \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSetEx(ref ds, SQL,pDbCon);
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

        public DataSet sel_ETC_JUSASUB_STT2(PsmhDb pDbCon, enmOCS_OORDER_InjecMain_Part ePART, string strFDate, string strTDate, string strSYSDATE , bool bMR)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                                                                                             \r\n";
            SQL += " 	'DEPT'					                                   AS GUBUN                                                             \r\n";
            SQL += "     , DECODE(GROUPING_ID(B.DEPTNAMEK),0,B.DEPTNAMEK,1,'합계') AS TIT                                                               \r\n";
            SQL += "     , TO_CHAR(COUNT(A.PTNO)				,'999,999,999')    AS INWON                                                             \r\n";
            SQL += "     , REPLACE(TO_CHAR(SUM(A.QTY * A.NAL)  	,'FM999,999,999.0'),'.0','')  	AS QTY                                                  \r\n";
            SQL += "  FROM KOSMOS_OCS.ETC_JUSASUB     A                                                                                                 \r\n";
            SQL += "     , KOSMOS_PMPA.BAS_CLINICDEPT B                                                                                                 \r\n";
            SQL += " WHERE 1=1                                                                                                                          \r\n";
            SQL += "   AND A.ACTDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                     AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "   AND A.GBACT = 'Y'                                                                                                                \r\n";
            SQL += "   AND A.DEPTCODE = B.DEPTCODE                                                                                                      \r\n";

            if (ePART == enmOCS_OORDER_InjecMain_Part.IJRM)
            {
                SQL += "   AND A.PART     IN ('1','4') -- 본관 + 회사접종                                                                                 \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.XRAY)
            {
                SQL += "   AND A.PART     IN ('3')  -- 영사의학과                                                                                         \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.FLU_LTD)
            {
                SQL += "   AND A.PART     IN ('4')  -- 산업채                                                                                         \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.CANCER)
            {
                SQL += "   AND A.PART     IN ('5')  -- 항암주사실                                                                                         \r\n";
            }

            SQL += " GROUP BY ROLLUP(B.DEPTNAMEK)                                                                                                       \r\n";
            SQL += " UNION ALL                                                                                                                          \r\n";
            SQL += " SELECT                                                                                                                             \r\n";
            SQL += " 	  'DR'				 AS GUBUN                                                                                                   \r\n";
            SQL += " 	, DECODE(GROUPING_ID(B.DRNAME),0,B.DRNAME,1,'합계') AS TIT                                                                      \r\n";
            SQL += "     , TO_CHAR(COUNT(A.PTNO)				,'999,999,999')    AS INWON                                                             \r\n";
            SQL += "     , REPLACE(TO_CHAR(SUM(A.QTY * A.NAL)  	,'FM999,999,999.0'),'.0','')  	AS QTY                                                  \r\n";
            SQL += "  FROM KOSMOS_OCS.ETC_JUSASUB 	A                                                                                                   \r\n";
            SQL += "     , KOSMOS_PMPA.BAS_DOCTOR 	B                                                                                                   \r\n";
            SQL += " WHERE 1=1                                                                                                                          \r\n";
            SQL += "   AND A.ACTDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                     AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "   AND A.GBACT = 'Y'                                                                                                                \r\n";
            SQL += "   AND A.DRCODE = B.DRCODE                                                                                                          \r\n";

            if (ePART == enmOCS_OORDER_InjecMain_Part.IJRM)
            {
                SQL += "   AND A.PART     IN ('1','4') -- 본관 + 회사접종                                                                                 \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.XRAY)
            {
                SQL += "   AND A.PART     IN ('3')  -- 영사의학과                                                                                         \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.FLU_LTD)
            {
                SQL += "   AND A.PART     IN ('4')  -- 산업채                                                                                         \r\n";
            }
            else if (ePART == enmOCS_OORDER_InjecMain_Part.CANCER)
            {
                SQL += "   AND A.PART     IN ('5')  -- 항암주사실                                                                                         \r\n";
            }
            SQL += " GROUP BY ROLLUP(B.DRNAME)                                                                                                          \r\n";

            SQL += " UNION ALL                                                                                                                          \r\n";
            SQL += " SELECT                                                                                                                             \r\n";
            SQL += " 	   'ORDER'  				AS GUBUN                                                                                            \r\n";
            SQL += "      , DECODE (                                                                                                                    \r\n";
            SQL += "      			GROUPING_ID(DECODE(NVL(B.ORDERNAMES,'NULL'),'NULL', B.ORDERNAME, B.ORDERNAME || ':' || B.ORDERNAMES)), 0 ,          \r\n";
            SQL += " 				            DECODE(NVL(B.ORDERNAMES,'NULL'),'NULL', B.ORDERNAME, B.ORDERNAME || ':' || B.ORDERNAMES) , 1 , '합계'   \r\n";
            SQL += "               )	 				AS TIT                                                                                          \r\n";
            SQL += "     , TO_CHAR(COUNT(A.PTNO)				,'999,999,999')    AS INWON                                                             \r\n";
            //2020-02-10
            //SQL += "     , REPLACE(TO_CHAR(SUM(A.QTY * A.NAL)  	,'FM999,999,999.0'),'.0','')  	AS QTY                                                   \r\n";
            if (bMR == true)
            {
                SQL += "     , REPLACE(TO_CHAR(SUM(A.REALQTY * A.NAL)  	,'FM999,999,999.0'),'.0','')  	AS QTY                                                   \r\n";//2020-02-10
                SQL += "   FROM KOSMOS_OCS.OCS_OORDER A                                                                                                    \r\n";
                SQL += "      , KOSMOS_OCS.OCS_ORDERCODE B                                                                                                             \r\n";
                SQL += "  WHERE 1=1                                                                                                                         \r\n";
                SQL += "   AND A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
                SQL += "                     AND " + ComFunc.covSqlDate(strTDate, false);                
                SQL += "    AND A.ORDERCODE = B.ORDERCODE                                                                                                   \r\n";
                SQL += "    AND A.Bun IN ('20','23')                                                                                                        \r\n";
                SQL += "    AND A.GbSunap ='1'                                                                                                              \r\n";
                SQL += "    AND SUBSTR(a.DosCode,5,2) ='02'                                                                                                  \r\n";
                //if (strSYSDATE.CompareTo("2013-11-01") = false then
                //{

                //}
                SQL += "    AND A.DrCode IN ('1107','1125','0901','0902')                                                                                   \r\n";

                if (ePART == enmOCS_OORDER_InjecMain_Part.IJRM)
                {
                    SQL += "   AND A.PART     IN ('1','4') -- 본관 + 회사접종                                                                                 \r\n";
                }
                else if (ePART == enmOCS_OORDER_InjecMain_Part.XRAY)
                {
                    SQL += "   AND A.PART     IN ('3')  -- 영사의학과                                                                                         \r\n";
                }
                else if (ePART == enmOCS_OORDER_InjecMain_Part.FLU_LTD)
                {
                    SQL += "   AND A.PART     IN ('4')  -- 산업채                                                                                         \r\n";
                }
                else if (ePART == enmOCS_OORDER_InjecMain_Part.CANCER)
                {
                    SQL += "   AND A.PART     IN ('5')  -- 항암주사실                                                                                         \r\n";
                }
            }
            else
            {
                SQL += "     , REPLACE(TO_CHAR(SUM(A.QTY * A.NAL)  	,'FM999,999,999.0'),'.0','')  	AS QTY                                                   \r\n";//2020-02-10 
                SQL += "   FROM KOSMOS_OCS.ETC_JUSASUB A                                                                                                    \r\n";
                SQL += "      , KOSMOS_OCS.OCS_ORDERCODE B                                                                                                             \r\n";
                SQL += "  WHERE 1=1                                                                                                                         \r\n";
                SQL += "   AND A.ACTDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
                SQL += "                     AND " + ComFunc.covSqlDate(strTDate, false);
                SQL += "    AND A.GBACT = 'Y'                                                                                                               \r\n";
                SQL += "    AND A.ORDERCODE = B.ORDERCODE                                                                                                   \r\n";

                if (ePART == enmOCS_OORDER_InjecMain_Part.IJRM)
                {
                    SQL += "   AND A.PART     IN ('1','4') -- 본관 + 회사접종                                                                                 \r\n";
                }
                else if (ePART == enmOCS_OORDER_InjecMain_Part.XRAY)
                {
                    SQL += "   AND A.PART     IN ('3')  -- 영사의학과                                                                                         \r\n";
                }
                else if (ePART == enmOCS_OORDER_InjecMain_Part.FLU_LTD)
                {
                    SQL += "   AND A.PART     IN ('4')  -- 산업채                                                                                         \r\n";
                }
                else if (ePART == enmOCS_OORDER_InjecMain_Part.CANCER)
                {
                    SQL += "   AND A.PART     IN ('5')  -- 항암주사실                                                                                         \r\n";
                }

            }
            SQL += "  GROUP BY ROLLUP(DECODE(NVL(B.ORDERNAMES,'NULL'),'NULL', B.ORDERNAME, B.ORDERNAME || ':' || B.ORDERNAMES))                         \r\n";

            SQL += "  UNION ALL                                                                                                                         \r\n";
            SQL += "  SELECT                                                                                                                            \r\n";
            SQL += "         'DOS'				 AS GUBUN                                                                                               \r\n";
            SQL += "       , DECODE(GROUPING_ID(B.DOSNAME),0,B.DOSNAME,'합계') 			 AS TIT                                                         \r\n";
            SQL += "     , TO_CHAR(COUNT(A.PTNO)				,'999,999,999')    AS INWON                                                             \r\n";
            SQL += "     , REPLACE(TO_CHAR(SUM(A.QTY * A.NAL)  	,'FM999,999,999.0'),'.0','')  	AS QTY                                                  \r\n";
            SQL += "   FROM KOSMOS_OCS.ETC_JUSASUB A                                                                                                    \r\n";
            SQL += "      , KOSMOS_OCS.OCS_ODOSAGE B                                                                                                    \r\n";
            SQL += "  WHERE 1=1                                                                                                                         \r\n";
            SQL += "   AND A.ACTDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                     AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "    AND A.GBACT = 'Y'                                                                                                               \r\n";
            SQL += "    AND A.DOSCODE = B.DOSCODE(+)                                                                                                    \r\n";

            if (bMR == true)
            {
                SQL += "    AND A.DeptCode = 'MR'                                                                                                       \r\n";
            }
            else
            {
                if (ePART == enmOCS_OORDER_InjecMain_Part.IJRM)
                {
                    SQL += "   AND A.PART     IN ('1','4') -- 본관 + 회사접종                                                                             \r\n";
                }
                else if (ePART == enmOCS_OORDER_InjecMain_Part.XRAY)
                {
                    SQL += "   AND A.PART     IN ('3')  -- 영사의학과                                                                                    \r\n";
                }
                else if (ePART == enmOCS_OORDER_InjecMain_Part.FLU_LTD)
                {
                    SQL += "   AND A.PART     IN ('4')  -- 산업채                                                                                         \r\n";
                }
                else if (ePART == enmOCS_OORDER_InjecMain_Part.CANCER)
                {
                    SQL += "   AND A.PART     IN ('5')  -- 항암주사실                                                                                         \r\n";
                }
            }
            
            SQL += "  GROUP BY ROLLUP( B.DOSNAME)                                                                                                       \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSetEx(ref ds, SQL, pDbCon);
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

        /// <summary>주사실메인</summary>
        /// <param name="strFr">시작일자</param>
        /// <param name="strTo">종료일자</param>
        /// <param name="isFlu">회사예방접종</param>
        /// <param name="type">주사실 작업 종류</param>
        /// <param name="strPtno">환자번호</param>
        /// <param name="isVcc">벡신여부</param>
        /// <param name="part">주사실 프로그램 사용부서</param>
        /// <param name="isCOVID">코로나예방접종 여부</param>
        /// <returns></returns>
        public DataSet sel_OCS_OORDER_InjecMain(PsmhDb pDbCon, string strFr, string strTo, bool isFlu
                                                , enmOCS_OORDER_InjecMain_Type type, string strPtno = ""
                                                , bool isVcc = false, enmOCS_OORDER_InjecMain_Part part = enmOCS_OORDER_InjecMain_Part.ALL, bool isCOVID = false)
        {

            DataSet ds = new DataSet(); 
            string sType = type.ToString();

            SQL = "";
            SQL = SQL + "SELECT 	BDATE                                                   -- 31 /H01 처방일자                                            \r\n";
            SQL = SQL + "		,	ACTDATE                                                 --    /H02 액팅일자                                            \r\n";
            SQL = SQL + "		,	ACTTIME                                                 --    /H02 액팅일자                                            \r\n";
            SQL = SQL + "		,	KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(PTNO,BDATE)          AS INFECT_INFO        --    / 03 INFECT_INFO              \r\n";
            SQL = SQL + "		,	''                                                         AS INFECT_IMG                                             \r\n";
            SQL = SQL + "		,	DECODE(KOSMOS_OCS.FC_ETC_ALLERGY_MST_CHK(PTNO),'Y','알','')  AS ALLERGY     --    / 03 ALLERGY                         \r\n";
            SQL = SQL + "		,	KK059                                                   --    / 05 수기료 여부                                         \r\n";
            SQL = SQL + "		,	PTNO                                                    -- 01 / 10 환자번호                                            \r\n";            
            SQL = SQL + "		,	SNAME                                                   -- 02 / 11 환자성명                                            \r\n";
            SQL = SQL + "		,	''                                                          AS SAMENAME      -- 동명이인                               \r\n";
            SQL = SQL + "		,	SEX                                                     -- 04 / 12 성별                                                \r\n";
            SQL = SQL + "		,	AGE                                                     -- 03 / 13 나이                                                \r\n";
            SQL = SQL + "		,	DEPTCODE                                                -- 05 / 14 과                                                  \r\n";
            SQL = SQL + "		,	DRNAME                                                  -- 06 / 15 의사명                                              \r\n";

            SQL = SQL + "		,   SUCODE                                                  --    / 09 접종여부                                            \r\n";
            SQL = SQL + "		,	ORDERNM                                                 --07  / 16 처방명                                              \r\n";
            SQL = SQL + "		,	DOSNAME                                                 --08  / 17 DOS명                                               \r\n";
            SQL = SQL + "		,	GBNAL                                                   --10  / 18 NAL : 1일처방, D: Multi처방                         \r\n";
            SQL = SQL + "		,	QTY                                                     --11  / 19                                                     \r\n";
            SQL = SQL + "		,	TO_CHAR(ACTNAL) AS ACTNAL                               --    / 21                                                     \r\n";
            SQL = SQL + "		,	TO_CHAR(NAL)    AS NAL                                  --    / 20                                                     \r\n";
            SQL = SQL + "		,	TO_CHAR(CONTENTS)    AS CONTENTS                        --    /  용량                                                \r\n";
            SQL = SQL + "		,	GBGROUP                                                 --    /  믹스그룹                                            \r\n";

            SQL = SQL + "		,	CHK                                                     --12  / 22 CHK                                                 \r\n";
            SQL = SQL + "		,	REMARK                                                  --14  / 23 REMARK                                              \r\n";           
            SQL = SQL + "		,	DECODE(VCNCODE,'','',SUBSTR(REMARK,1,1))     AS CHASU   --16  / 24 차수                                                \r\n";
            SQL = SQL + "		,   CHKTTPR                                                 --    / 09 접종여부                                            \r\n";
            SQL = SQL + "		,	JEPCODE                                                 --17  /H25 JEPCODE                                             \r\n";

            SQL = SQL + "		,	CHKJIN                                                  --    / 03 진찰료 수납여부  ** RGB(128, 255, 255)              \r\n";
            SQL = SQL + "		,	SUNAP                                                   --    / 04 수납여부  ** 환불하면 OORDER에 마킹이 안돼는지...   \r\n";
            
            SQL = SQL + "		,	CHKVACC                                                 --    / 06 예방접종여부                                        \r\n";
            SQL = SQL + "		,	CHKCZ394                                                --    / 08 독감여부                                            \r\n";
            

            SQL = SQL + "		,	DECODE(NVL(JUSAMSG,'NULL'),'NULL','','◎')   AS MSGYN   --17  /H25 JEPCODE                                             \r\n";
            SQL = SQL + "		,	JUSAMSG                                      AS MSG     --19  / 26 JUSA MSG                                            \r\n";
            SQL = SQL + "		,	VCNCODE                                                 --20  / 26 VCNCODE                                             \r\n";
            SQL = SQL + "		,	CASE WHEN CHKIV = '1' THEN '수혈'                                                                                      \r\n";
            SQL = SQL + "                WHEN CHKIV = '2' THEN '수액'                                                                                      \r\n";
            SQL = SQL + "            ELSE '일반'                                                                                                           \r\n";
            SQL = SQL + "             END                                        AS CHKIV   --25  / 27 IV(M) 수혈                                          \r\n";
            SQL = SQL + "		,	JUMIN                                                   --22  / 28 주민번호                                            \r\n";
            SQL = SQL + "		,	ROW_ID                                                  --13  / 30 ROWID                                               \r\n";
            SQL = SQL + "		,   UNITNEW1                                                --    / 31 UNITNEW1                                            \r\n";
            SQL = SQL + "		,   UNITNEW2                                                --    / 32 UNITNEW2                                            \r\n";
            SQL = SQL + "		,   BI                                                      --    / 33 BI                                                  \r\n";
            SQL = SQL + "		,   ORDERNO                                                 --    / 34 ORDERNO                                             \r\n";
            SQL = SQL + "		,   KOSMOS_OCS.FC_BAS_BCODE_OLDFLUCHK(SUCODE,BDATE,PTNO) AS CHKOLDFLU --    / 35 C                                         \r\n";
            SQL = SQL + "  FROM (                                                                                                                          \r\n";
            // 일반주사 대기자 (특정일자만)
            SQL = SQL + "	 SELECT                                                                                                                        \r\n";
            SQL = SQL + " 			  TO_CHAR(A.BDATE,'YYYY-MM-DD')											                                            AS BDATE	-- 31 /H01 처방일자 \r\n";
            SQL = SQL + " 			, ''																					                            AS ACTDATE	--    /H02 액팅일자 \r\n";
            SQL = SQL + " 			, ''																					                            AS ACTTIME	--    /H02 액팅일자 \r\n";
            SQL = SQL + "		    , A.PTNO  																			                                AS PTNO 	-- 01 / 03 환자번호 \r\n";
            SQL = SQL + "		    , B.SNAME																			                                AS SNAME  	-- 02 / 04 환자성명 \r\n";
            SQL = SQL + "		    , KOSMOS_OCS.FC_GET_AGE( KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(A.PTNO), TRUNC(SYSDATE))	                                AS AGE		-- 03 / 05 나이     \r\n";
            SQL = SQL + "           , DECODE(B.SEX, 'M', '남', 'F', '여') 													                            AS SEX		-- 04 / 06 성별     \r\n";
            SQL = SQL + "           , A.DEPTCODE																			                            AS DEPTCODE -- 05 / 07 과       \r\n";
            SQL = SQL + "			, (CASE WHEN A.DRCODE = '99' THEN KOSMOS_OCS.FC_INSA_MST_KORNAME(A.SABUN) ELSE C.DRNAME END )	                    AS DRNAME   -- 06 / 08 의사명   \r\n";
            SQL = SQL + "			, KOSMOS_OCS.FC_OPD_MASTER_JIN(A.PTNO,A.BDATE)																        AS CHKJIN 	--    / 09 진찰료 수납여부  ** RGB(128, 255, 255)              \r\n";
            SQL = SQL + "		    , KOSMOS_OCS.FC_OPD_SLIP_SUNAP(A.GBAUTOSEND2,A.SUCODE,A.PTNO,A.DEPTCODE,A.BDATE,'Y')                                AS SUNAP 	--    / 10 수납여부  ** 환불하면 OORDER에 마킹이 안돼는지...   \r\n";
            SQL = SQL + "		    , KOSMOS_OCS.FC_OPD_SLIP_KK059(A.SUCODE,A.PTNO,A.DEPTCODE,A.BDATE)                                                  AS KK059    --    / 11 수기료 여부                                         \r\n";
            SQL = SQL + "		    , KOSMOS_OCS.FC_OPD_SLIP_VACC(A.PTNO,A.BDATE, A.SUCODE)									                            AS CHKVACC  --    / 12 예방접종여부                                        \r\n";
            SQL = SQL + "		    , DECODE(KOSMOS_OCS.FC_OCS_OORDER_SUCODE(A.PTNO, A.BDATE,'CZ394'),'Y','독')   									    AS CHKCZ394 --    / 14 독감여부                                            \r\n";
            SQL = SQL + "           , ''                                                                                                                AS CHKTTPR  --    / 15 접종여부                                            \r\n";
            SQL = SQL + "           , A.SUCODE                                                                                  	    		        AS SUCODE   --    / 신규        \r\n";
            SQL = SQL + "           , TRIM(F.ORDERNAME) || DECODE(NVL(F.ORDERNAMES,'NVL'),'NVL','', ' ▶' ||  TRIM(F.ORDERNAMES) ) 	    		        AS ORDERNM  -- 16 처방명        \r\n";
            SQL = SQL + "           , D.DOSNAME																											AS DOSNAME  --08  / 17 DOS명                                               \r\n";
            SQL = SQL + "           , DECODE(A.NAL, 1, 'S', 0, '0', 'D') 																				AS GBNAL	--10  / 18 NAL : 1일처방, D: Multi처방                         \r\n";
            //SQL = SQL + "           , KOSMOS_OCS.FC_QTY(TO_CHAR(A.QTY))																					AS QTY		--11  / 19                                                     \r\n";
            //2020-02-10 실사용량으로 변경
            SQL = SQL + "           , KOSMOS_OCS.FC_QTY(TO_CHAR(A.REALQTY))																					AS QTY		--11  / 19                                                     \r\n";
            SQL = SQL + "           , A.CONTENTS                       																					AS CONTENTS		--11  / 19                                                     \r\n";
            SQL = SQL + "           , A.GBGROUP                       																					AS GBGROUP		--11  / 19                                                     \r\n";
            SQL = SQL + "           , 'False'																										    AS CHK	    --12  / 20 CHK                                                 \r\n";
            SQL = SQL + "           , A.REMARK                                                                                                          AS REMARK   --14 / 21 REMARK                                               \r\n";
            SQL = SQL + "           , KOSMOS_OCS.FC_ETC_JUSACODE(A.SUCODE,A.ORDERCODE,'Y')	   															AS JEPCODE	--17 /H23 JEPCODE                                              \r\n";
            SQL = SQL + "			, TRIM(B.JUSAMSG)		        																					AS JUSAMSG	--19 / 24 JUSA MSG                                             \r\n";
            SQL = SQL + "           , KOSMOS_OCS.FC_ETC_JUSACODE(A.SUCODE,'', 'N')																		AS VCNCODE  --20 / 25 VCNCODE                                              \r\n";
            SQL = SQL + "			, (CASE WHEN INSTR(D.DOSNAME,'IV(m)') > 0 THEN '2'  WHEN INSTR(A.REMARK ,'수혈')  > 0 THEN '1' ELSE 'T' END) AS CHKIV	        --25 / 26 주사(IV) 수혈                                        \r\n";
            SQL = SQL + "           , B.JUMIN1 || B.JUMIN2 																								AS JUMIN	--22 / 25 주민번호                                             \r\n";
            SQL = SQL + "           , A.ROWID 																											AS row_id	--13 / 29 ROWID                                                \r\n";
            SQL = SQL + "           , E.UNITNEW1                                                                                                        AS UNITNEW1 --  / 30 UNITNEW1                                             \r\n";
            SQL = SQL + "           , E.UNITNEW2                                                                                                        AS UNITNEW2 --  / 31 UNITNEW2                                             \r\n";
            SQL = SQL + "           , 1                                                                                                                 AS ACTNAL   --  / 32 ACTNAL                                               \r\n";
            SQL = SQL + "           , A.NAL                                                                                                             AS NAL      --  / 33 NAL                                                  \r\n";
            SQL = SQL + "           , A.BI                                                                                                              AS BI       --  / 34 BI                                                   \r\n";
            SQL = SQL + "           , A.ORDERNO                                                                                                         AS ORDERNO  --  / 35 ORDERNO                                              \r\n";
            SQL = SQL + "        FROM " + ComNum.DB_MED  + "OCS_OORDER    A  \r\n";
            SQL = SQL + "           , " + ComNum.DB_PMPA + "BAS_PATIENT   B  \r\n";
            SQL = SQL + "           , " + ComNum.DB_PMPA + "BAS_DOCTOR    C  \r\n";
            SQL = SQL + "           , " + ComNum.DB_MED  + "OCS_ODOSAGE   D  \r\n";
            SQL = SQL + "           , " + ComNum.DB_PMPA + "BAS_SUN       E  \r\n";
            SQL = SQL + "           , " + ComNum.DB_MED  + "OCS_ORDERCODE F  \r\n";
            SQL = SQL + "       WHERE 1=1                                                                   \r\n";
            SQL = SQL + "         AND 'RECIPT'      = '" + sType + "'                                       \r\n";
            SQL = SQL + "         AND A.SUCODE NOT IN ('NSA-CT')                                    \r\n";
            SQL = SQL + "         AND A.PTNO    	= B.PANO                                                \r\n";
            SQL = SQL + "         AND A.ORDERCODE 	= F.ORDERCODE                                           \r\n";
            SQL = SQL + "         AND A.GBBOTH 	    NOT IN ('3','J')                                        \r\n";
            SQL = SQL + "         AND (A.GBSUNAP     = '1'  OR (A.GBSUNAP ='0' AND A.GBAUTOSEND2 ='2'  ) OR (A.GBSUNAP = '0' AND (A.SUCODE = 'CO19-AZ1' OR A.SUCODE = 'CO19-PF1' OR A.SUCODE = 'CO19-PF2' OR A.SUCODE = 'CO19-MO1' OR A.SUCODE = 'BOOST-M1' OR A.SUCODE = 'BOOST-M2') ) )   \r\n";
            SQL = SQL + "         AND A.DRCODE      = C.DRCODE                                              \r\n";
            SQL = SQL + "         AND A.SUCODE      = E.SUNEXT                                              \r\n";
            SQL = SQL + "         AND A.DOSCODE     = D.DOSCODE(+)                                          \r\n";
            SQL = SQL + "         AND A.NAL         > 0                                                     \r\n";
            SQL = SQL + "         AND A.SEQNO       > 0                                                     \r\n";
            //if (isCOVID == true)
            //{
            //    SQL = SQL + "   AND A.SUCODE IN ('CO19-AZ2') ";
            //}
            //else
            //{
            //    SQL = SQL + "   AND A.SUCODE NOT IN ('CO19-AZ2') ";
            //}
            if (isFlu == true)
            {
                SQL = SQL + "         AND A.PTNO IN ( SELECT PANO                                                             \r\n";
                SQL = SQL + "                           FROM  " + ComNum.DB_PMPA + "OPD_MASTER                                \r\n";
                SQL = SQL + "                          WHERE ACTDATE BETWEEN " + ComFunc.covSqlDate(strFr, false);
                SQL = SQL + "                                            AND " + ComFunc.covSqlDate(strFr, false);
                SQL = SQL + "                            AND GBFLU_LTD IN ('Y','C')                                                  \r\n";
                SQL = SQL + "                        )                                                                       \r\n";
            }
            else
            {
                SQL = SQL + "         AND A.PTNO NOT IN ( SELECT PANO                                                        \r\n";
                SQL = SQL + "                               FROM  " + ComNum.DB_PMPA + "OPD_MASTER                           \r\n";
                SQL = SQL + "                              WHERE ACTDATE BETWEEN " + ComFunc.covSqlDate(strFr, false);
                SQL = SQL + "                                                AND " + ComFunc.covSqlDate(strFr, false);
                SQL = SQL + "                                AND GBFLU_LTD  IN ('Y','C')                                             \r\n";
                SQL = SQL + "                        )                                                                      \r\n";
            }

            if (string.IsNullOrEmpty(strPtno))
            {
                SQL = SQL + "         AND A.BDATE   	BETWEEN " + ComFunc.covSqlDate(strFr, false);
                SQL = SQL + "                               AND " + ComFunc.covSqlDate(strFr, false);
            }
            else
            {
                SQL = SQL + "         AND A.PTNO =" + ComFunc.covSqlstr(strPtno, false);
            }

            SQL = SQL + "         AND(                                                                      \r\n";
            SQL = SQL + "                A.BUN 		= '20'                                                  \r\n";
            SQL = SQL + "             OR A.ORDERCODE IN ('MG10060','A-POCS','A-MACA','A-POCSY','A-POSY5')                         \r\n";
            SQL = SQL + "             OR (                                                                  \r\n";
            SQL = SQL + "		                A.DEPTCODE = 'PD'                                           \r\n";
            SQL = SQL + "                   AND A.ORDERCODE IN (                                            \r\n";
            SQL = SQL + "		     	 			             SELECT JEPCODE                             \r\n";
            SQL = SQL + "		     	 			               FROM KOSMOS_ADM.DRUG_SETCODE             \r\n";
            SQL = SQL + "		     	 				          WHERE GUBUN = '20'                        \r\n";
            SQL = SQL + "		     	 				            AND DELDATE IS NULL                     \r\n";
            SQL = SQL + "		     	 				        )                                           \r\n";
            SQL = SQL + "             )                                                                     \r\n";
            SQL = SQL + "         )                                                                         \r\n";

            if (part == enmOCS_OORDER_InjecMain_Part.XRAY)
            {
                //2017.01.03.김홍록: POCS를 영상의학과에서 조히 가능하도록 OR A.DOSCODE IN ('010501') 추가함
                SQL = SQL + "         AND (    A.DOSCODE IN ( SELECT DOSCODE FROM " + ComNum.DB_MED + "OCS_ODOSAGE WHERE WARDCODE ='RD')   \r\n";
                SQL = SQL + "              OR (A.DOSCODE IN ('499915')) OR A.DOSCODE IN ('010501') )                                                                  \r\n";
            }
            else if (part == enmOCS_OORDER_InjecMain_Part.IJRM)
            {
                SQL = SQL + "         AND A.DRCODE NOT IN ('1107','1125','6108','6109')                                                                                                                  \r\n";
                SQL = SQL + "         AND A.DOSCODE NOT IN ( SELECT DOSCODE FROM " + ComNum.DB_MED + "OCS_ODOSAGE WHERE DOSFULLCODE LIKE '%항암주사실%' OR DOSFULLCODE LIKE '%약국조제(항암)%' )                    \r\n";
                SQL = SQL + "         AND  (                            \r\n";
                SQL = SQL + "               RTRIM(A.DOSCODE) LIKE '9%1' \r\n";
                SQL = SQL + "            OR RTRIM(A.DOSCODE) = '999999' \r\n";
                //SQL = SQL + "            OR RTRIM(A.DOSCODE) = '930116' \r\n";
                SQL = SQL + "            OR RTRIM(A.DOSCODE) = '930115' \r\n";
                SQL = SQL + "            OR RTRIM(A.DOSCODE) = '930101' \r\n";
                SQL = SQL + "            OR CASE WHEN A.DEPTCODE = 'PD' \r\n";
                SQL = SQL + "                     AND A.ORDERCODE  IN (                                 \r\n";
                SQL = SQL + "                                         SELECT JEPCODE                    \r\n";
                SQL = SQL + "                                            FROM KOSMOS_ADM.DRUG_SETCODE   \r\n";
                SQL = SQL + "                                            WHERE GUBUN = '20'             \r\n";
                SQL = SQL + "                                              AND DELDATE IS NULL          \r\n";
                SQL = SQL + "									       )  THEN  NVL(A.DOSCODE,'@@##@@') \r\n";
                SQL = SQL + "			          ELSE '@@##'                                           \r\n";
                SQL = SQL + "		        END             = NVL(A.DOSCODE,'@@##@@')                   \r\n";
                SQL = SQL + "            )                                                              \r\n";
            }
            else if (part == enmOCS_OORDER_InjecMain_Part.CANCER)
            {
                //SQL = SQL + "         AND A.DOSCODE IN ( SELECT DOSCODE FROM " + ComNum.DB_MED + "OCS_ODOSAGE WHERE DOSFULLCODE LIKE '%항암주사실%')   \r\n";
                SQL = SQL + "         AND A.DOSCODE IN ( SELECT DOSCODE FROM " + ComNum.DB_MED + "OCS_ODOSAGE WHERE DOSFULLCODE LIKE '%항암주사실%' OR DOSFULLCODE LIKE '%약국조제(항암)%' )                    \r\n";
            }
            SQL = SQL + "UNION ALL                                                                          \r\n";
            // 멀티주사
            SQL = SQL + "		  SELECT                                                                    \r\n";
            SQL = SQL + " 				TO_CHAR(A.BDATE,'YYYY-MM-DD')														                    AS BDATE	-- 31 /H01 처방일자  \r\n";
            SQL = SQL + " 			    , ''																				                    AS ACTDATE	--    /H02 액팅일자  \r\n";
            SQL = SQL + " 			    , ''																					                AS ACTTIME	--    /H02 액팅일자 \r\n";
            SQL = SQL + "		        , A.PTNO  																					            AS PTNO	    -- 01 / 03           \r\n";
            SQL = SQL + "		        , B.SNAME																					            AS SNAME 	-- 02 / 04           \r\n";
            SQL = SQL + "		        , KOSMOS_OCS.FC_GET_AGE( KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(A.PTNO), TRUNC(SYSDATE))	                    AS AGE		-- 03 / 05           \r\n";
            SQL = SQL + "               , DECODE(B.SEX, 'M', '남', 'F', '여') 												                    AS SEX		-- 04 / 06           \r\n";
            SQL = SQL + "               , A.DEPTCODE																				            AS DEPTCODE -- 05 / 07           \r\n";
            SQL = SQL + "				, KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE)																AS DRNAME   -- 06 / 08           \r\n";
            SQL = SQL + "				, KOSMOS_OCS.FC_OPD_MASTER_JIN(A.PTNO,A.BDATE)															AS CHKJIN	--    / 09 진찰료 수납여부  ** RGB(128, 255, 255)   \r\n";
            SQL = SQL + "		        , KOSMOS_OCS.FC_OPD_SLIP_SUNAP(C.GBAUTOSEND2,A.SUCODE,A.PTNO,A.DEPTCODE,A.BDATE,'Y')               	    AS SUNAP 	--    / 10 수납여부                                 \r\n";
            SQL = SQL + "		        , KOSMOS_OCS.FC_OPD_SLIP_KK059 (A.SUCODE,A.PTNO,A.DEPTCODE,A.BDATE)                                   	AS KK059    --    / 11 수기료 여부                              \r\n";
            SQL = SQL + "		        , KOSMOS_OCS.FC_OPD_SLIP_VACC(A.PTNO,A.BDATE, A.SUCODE)							AS CHKVACC  --    / 12 예방접종여부                             \r\n";
            SQL = SQL + "		        , DECODE(KOSMOS_OCS.FC_OCS_OORDER_SUCODE(A.PTNO, A.BDATE,'CZ394'),'Y','독')				  			    AS CHKCZ394 --    / 14 독감여부                                 \r\n";
            SQL = SQL + "              , ''                                                                                                     AS CHKTTPR  --    / 15 접종여부                                            \r\n";
            SQL = SQL + "              , A.SUCODE                                                                         	    		        AS SUCODE   --    / 신규        \r\n";
            SQL = SQL + "              , TRIM(F.ORDERNAME) || DECODE(NVL(F.ORDERNAMES,'NVL'),'NVL','', ' ▶' ||  TRIM(F.ORDERNAMES) )	        AS ORDERNM  -- 16 처방명        \r\n";
            SQL = SQL + "              , D.DOSNAME																								AS DOSNAME  -- 08 / 17 DOS명                                    \r\n";
            SQL = SQL + "              , 'M'																 									AS GBNAL	-- 10 / 18 NAL                                      \r\n";
            SQL = SQL + "              , KOSMOS_OCS.FC_QTY(TO_CHAR(A.QTY))																		AS QTY		--11  / 19                                          \r\n";
            SQL = SQL + "           , A.CONTENTS                       																					AS CONTENTS		--11  / 19                                                     \r\n";
            SQL = SQL + "           , A.GBGROUP                       																					AS GBGROUP		--11  / 19                                                     \r\n";
            SQL = SQL + "              , 'False'																								AS CHK		--12  / 20 CHK                                      \r\n";            
            SQL = SQL + "           , A.REMARK                                                                                                  AS REMARK   --14 / 21 REMARK                                               \r\n";
            SQL = SQL + "           , KOSMOS_OCS.FC_ETC_JUSACODE(A.SUCODE,A.ORDERCODE,'Y')								  						AS JEPCODE	--17  /H23 JEPCODE                                  \r\n";
            SQL = SQL + "			, TRIM(B.JUSAMSG)					   																        AS JUSAMSG 	--19  / 24 JUSA MSG                                 \r\n";
            SQL = SQL + "           , KOSMOS_OCS.FC_ETC_JUSACODE(A.SUCODE,'','N')																AS VCNCODE	--20  / 25 VCNCODE                                  \r\n";
            SQL = SQL + "			, (CASE WHEN INSTR(D.DOSNAME,'IV(m)') > 0 THEN '2'  WHEN INSTR(A.REMARK ,'수혈')  > 0 THEN '1' ELSE 'T' END)																								        AS CHKIV	--25  / 26 주사(IV) 수혈                            \r\n";
            SQL = SQL + "           , B.JUMIN1 || B.JUMIN2 																						AS JUMIN	--22  / 27 주민번호                                 \r\n";
            SQL = SQL + "           , A.ROWID 																									AS row_id	--13  /H29 ROWID                                    \r\n";
            SQL = SQL + "           , E.UNITNEW1 AS UNITNEW1                                                                                                --   / 30 UNITNEW1                                  \r\n";
            SQL = SQL + "           , E.UNITNEW2 AS UNITNEW2                                                                                                --   / 31 UNITNEW2                                  \r\n";
            SQL = SQL + "           , A.ACTNAL                                                                                              AS ACTNAL   --  / 32 ACTNAL                                         \r\n";
            SQL = SQL + "           , A.NAL                                                                                                     AS NAL      --  / 33 NAL                                        \r\n";
            SQL = SQL + "           , C.BI                                                                                                      AS BI       --  / 34 BI                                         \r\n";
            SQL = SQL + "           , A.ORDERNO                                                                                                 AS ORDERNO  --  / 35 ORDERNO                                              \r\n";
            SQL = SQL + "      FROM " + ComNum.DB_MED  + "ETC_JUSAMST 	A   \r\n";
            SQL = SQL + "         , " + ComNum.DB_PMPA + "BAS_PATIENT 	B   \r\n";
            SQL = SQL + "         , " + ComNum.DB_MED  + "OCS_OORDER 	C   \r\n";
            SQL = SQL + "         , " + ComNum.DB_MED  + "OCS_ODOSAGE 	D   \r\n";
            SQL = SQL + "         , " + ComNum.DB_PMPA + "BAS_SUN 		E   \r\n";
            SQL = SQL + "         , " + ComNum.DB_MED  + "OCS_ORDERCODE F   \r\n";
            SQL = SQL + "     WHERE 1=1                                     \r\n";
            SQL = SQL + "       AND 'MULTI'         = '" + sType + "'       \r\n";
            SQL = SQL + "       AND F.SUCODE NOT IN ('NSA-CT')                                    \r\n";
            SQL = SQL + "       AND A.PTNO          = B.PANO                \r\n";
            SQL = SQL + "       AND A.ORDERCODE 	= F.ORDERCODE           \r\n";

            //if (isCOVID == true)
            //{
            //    SQL = SQL + "   AND F.SUCODE IN ('CO19-AZ2') ";
            //}
            //else
            //{
            //    SQL = SQL + "   AND F.SUCODE NOT IN ('CO19-AZ2') ";
            //}

            if (isFlu == true)
            {
                SQL = SQL + "         AND A.PTNO IN ( SELECT PANO                                                \r\n";
                SQL = SQL + "                           FROM  " + ComNum.DB_PMPA + "OPD_MASTER                   \r\n";
                SQL = SQL + "                          WHERE ACTDATE BETWEEN " + ComFunc.covSqlDate(strFr, false);
                SQL = SQL + "                                            AND " + ComFunc.covSqlDate(strTo, false);
                SQL = SQL + "                            AND GBFLU_LTD IN ('Y','C')                                     \r\n";
                SQL = SQL + "                        ) \r\n";
            }
            else
            {
                SQL = SQL + "         AND A.PTNO NOT IN ( SELECT PANO                                                \r\n";
                SQL = SQL + "                               FROM  " + ComNum.DB_PMPA + "OPD_MASTER                   \r\n";
                SQL = SQL + "                              WHERE ACTDATE BETWEEN " + ComFunc.covSqlDate(strFr, false);
                SQL = SQL + "                                                AND " + ComFunc.covSqlDate(strTo, false);
                SQL = SQL + "                                AND GBFLU_LTD IN ('Y','C')                                      \r\n";
                SQL = SQL + "                        ) \r\n";
            }

            // 환자번호가 존재할 경우 기간에 상관없이 조회함.
            if (string.IsNullOrEmpty(strPtno) == true)
            {
                SQL = SQL + "         AND A.BDATE  < " + ComFunc.covSqlDate(strFr, false);
            }
            else
            {
                SQL = SQL + "         AND A.PTNO =" + ComFunc.covSqlstr(strPtno, false);
            }

            SQL = SQL + "       AND A.GBEND = 'N'                           \r\n";
            SQL = SQL + "       AND A.NAL 	> A.ACTNAL                      \r\n";
            SQL = SQL + "       AND (A.GBDATE 	< " + ComFunc.covSqlDate(strFr, false);
            SQL = SQL + "         OR A.GBDATE = '' OR A.GBDATE IS NULL)     \r\n";
            SQL = SQL + "       AND A.DOSCODE = D.DOSCODE(+)                \r\n";
            SQL = SQL + "       AND A.PTNO 	  = C.PTNO                      \r\n";
            SQL = SQL + "       AND A.BDATE   = C.BDATE                     \r\n";
            SQL = SQL + "       AND A.DEPTCODE = C.DEPTCODE                 \r\n";
            SQL = SQL + "       AND (C.GBSUNAP = '1' OR (C.GBSUNAP = '0' AND (C.SUCODE = 'CO19-AZ1' OR C.SUCODE = 'CO19-PF1' OR C.SUCODE = 'CO19-PF2' OR C.SUCODE = 'BOOST-M1' OR C.SUCODE = 'BOOST-M2' ) ) )                       \r\n";
            SQL = SQL + "       AND A.ORDERNO = C.ORDERNO                   \r\n";
            SQL = SQL + "       AND C.SUCODE  = E.SUNEXT                    \r\n";

            if (part == enmOCS_OORDER_InjecMain_Part.XRAY)
            {
                //2017.01.03.김홍록: POCS를 영상의학과에서 조히 가능하도록 OR A.DOSCODE IN ('010501') 추가함
                SQL = SQL + "         AND (A.DOSCODE IN ( SELECT DOSCODE FROM " + ComNum.DB_MED + "OCS_ODOSAGE WHERE WARDCODE ='RD') OR (A.DOSCODE IN ('499915')) OR A.DOSCODE IN ('010501') )                                     \r\n";
            }
            else if (part == enmOCS_OORDER_InjecMain_Part.IJRM)
            {
                SQL = SQL + "        AND A.DRCODE NOT IN ('1107','1125','6108','6109')                                                                                                                  \r\n";
                SQL = SQL + "         AND  (                            \r\n";
                SQL = SQL + "               RTRIM(A.DOSCODE) LIKE '9%1' \r\n";
                SQL = SQL + "            OR RTRIM(A.DOSCODE) = '999999' \r\n";
                //항암주사실로 분류변경(2021-01-05)
                //SQL = SQL + "            OR RTRIM(A.DOSCODE) = '930116' \r\n";
                SQL = SQL + "            OR RTRIM(A.DOSCODE) = '930115' \r\n";
                SQL = SQL + "            OR RTRIM(A.DOSCODE) = '930101' \r\n";
                
                //2017.08.25.김홍록: 멀티 처방에서는 추가적으로 나올 수 없는 경우가 있다고 하여 멀티 처방 조회에서는 제외함. - 속도가 안나와서
                //SQL = SQL + "            OR CASE WHEN A.DEPTCODE = 'PD' \r\n";
                //SQL = SQL + "                     AND A.ORDERCODE  IN (                                 \r\n";
                //SQL = SQL + "                                           SELECT JEPCODE                  \r\n";
                //SQL = SQL + "                                             FROM KOSMOS_ADM.DRUG_SETCODE  \r\n";
                //SQL = SQL + "                                            WHERE GUBUN = '20'             \r\n";
                //SQL = SQL + "                                              AND DELDATE IS NULL          \r\n";
                //SQL = SQL + "									       )  THEN  NVL(A.DOSCODE,'@@##@@') \r\n";
                //SQL = SQL + "			          ELSE '@@##'                                           \r\n";
                //SQL = SQL + "		        END             = NVL(A.DOSCODE,'@@##@@')                   \r\n";
                SQL = SQL + "            )                                                              \r\n";                
            }
            else if (part == enmOCS_OORDER_InjecMain_Part.CANCER)
            {
                //항암주사실 용법
                SQL = SQL + "         AND A.DOSCODE IN ( SELECT DOSCODE FROM " + ComNum.DB_MED + "OCS_ODOSAGE WHERE DOSFULLCODE LIKE '%항암주사실%' OR DOSFULLCODE LIKE '%약국조제(항암)%' )                    \r\n";
            }
                

            SQL = SQL + "UNION ALL                                                                                            \r\n";
            //기존액팅 현황조회
            SQL = SQL + "   SELECT                                                                                            \r\n";
            SQL = SQL + "	    TO_CHAR(A.BDATE,'YYYY-MM-DD')																        AS BDATE	--H01 처방일자 \r\n";
            SQL = SQL + "     , TO_CHAR(A.ACTDATE2,'YYYY-MM-DD') 													                AS ACTDATE	-- 02 액팅일자       \r\n";
            SQL = SQL + " 	  , TO_CHAR(A.ACTDATE2,'HH24:MI')							 						                    AS ACTTIME	--    /H02 액팅일자 \r\n";
            SQL = SQL + "     , A.PTNO                                           											        AS PTNO		-- 03 환자번호      \r\n";
            SQL = SQL + "     , F.SNAME																						        AS SNAME	-- 04 환자성명      \r\n";
            SQL = SQL + "	  , KOSMOS_OCS.FC_GET_AGE( KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(A.PTNO), TRUNC(SYSDATE))				        AS AGE		-- 05 나이          \r\n";
            SQL = SQL + "     , DECODE(F.SEX, 'M', '남', 'F', '여') 														        AS SEX		-- 06 성별          \r\n";
            SQL = SQL + "     , A.DEPTCODE                                                                       			        AS DEPTCODE	-- 07 과            \r\n";
            SQL = SQL + "	  ,( CASE WHEN A.DRCODE = '99' THEN KOSMOS_OCS.FC_INSA_MST_KORNAME(A.SABUN)ELSE G.DRNAME END ) 	        AS DRNAME   -- 08 의사명        \r\n";
            SQL = SQL + "	  , KOSMOS_OCS.FC_OPD_MASTER_JIN(A.PTNO,A.BDATE) 													    AS CHKJIN  	-- 09 진찰료 수납여부  ** RGB(128, 255, 255)             \r\n";
            SQL = SQL + "	  , ''                                                                                                  AS SUNAP 	-- 10 수납여부  ** 환불하면 OORDER에 마킹이 안돼는지...  \r\n";
            SQL = SQL + "	  , KOSMOS_OCS.FC_OPD_SLIP_KK059 (A.SUCODE,A.PTNO,A.DEPTCODE,A.ACTDATE)                                 AS KK059    -- 11 수기료 여부                                        \r\n";
            SQL = SQL + "	  , KOSMOS_OCS.FC_OPD_SLIP_VACC(A.PTNO,A.BDATE, A.SUCODE)							AS CHKVACC  -- 12 예방접종여부  \r\n";
            SQL = SQL + "	  , DECODE(KOSMOS_OCS.FC_OCS_OORDER_SUCODE(A.PTNO, A.BDATE,'CZ394'),'Y','독')		    				AS CHKCZ394 -- 14 독감여부      \r\n";
            SQL = SQL + "     , KOSMOS_OCS.FC_VACCINE_TTPREVEN_CHK(A.PTNO,TO_CHAR(A.ACTDATE2,'YYYYMMDD'),KOSMOS_OCS.FC_ETC_JUSACODE(A.SUCODE,'','N')) AS CHKTTPR  --    / 15 접종여부                                            \r\n";
            SQL = SQL + "     , A.SUCODE                                                                      	    		        AS SUCODE   --    / 신규        \r\n";
            SQL = SQL + "     , TRIM(B.ORDERNAME) || DECODE(NVL(B.ORDERNAMES,'NVL'),'NVL','', ' ▶' ||  TRIM(B.ORDERNAMES) )	    AS ORDERNM  -- 16 처방명        \r\n";
            SQL = SQL + "     , C.DOSNAME																							AS DOSNAME  -- 17 DOS명         \r\n";
            SQL = SQL + "     , DECODE(NVL(A.GBMASTER,'NULL'),'2','M','1','D','S')                    							    AS GBNAL	-- 18 NAL           \r\n";
            SQL = SQL + "     , TO_CHAR(A.QTY)																						AS QTY		-- 19 QTY           \r\n";
            SQL = SQL + "           , A.CONTENTS                       																					AS CONTENTS		--11  / 19                                                     \r\n";
            SQL = SQL + "           , A.GBGROUP                       																					AS GBGROUP		--11  / 19                                                     \r\n";
            SQL = SQL + "     , 'False'  																							AS CHK		-- 20 CHK           \r\n";
            SQL = SQL + "     , A.REMARK                                                                                            AS REMARK   --14 / 21 REMARK                                               \r\n";
            SQL = SQL + "	  , KOSMOS_OCS.FC_ETC_JUSACODE(A.SUCODE,A.ORDERCODE,'Y')								  				AS JEPCODE	--H23 JEPCODE       \r\n";
            SQL = SQL + "	  , TRIM(F.JUSAMSG)																					    AS JUSAMSG 	-- 24 JUSA MSG      \r\n";
            SQL = SQL + "	  , KOSMOS_OCS.FC_ETC_JUSACODE(A.SUCODE,'','N')														    AS VCNCODE	-- 25 VCNCODE       \r\n";
            SQL = SQL + "	  , (CASE WHEN INSTR(C.DOSNAME,'IV(m)') > 0 THEN '2'  WHEN INSTR(A.REMARK ,'수혈')  > 0 THEN '1' ELSE 'T' END)																								    AS CHKIV	-- 26 주사(IV) 수혈 \r\n";
            SQL = SQL + "	  , F.JUMIN1 || F.JUMIN2 																			    AS JUMIN	-- 27 주민번호      \r\n";
            SQL = SQL + "     , A.ROWID 																							AS row_id	--H29 ROWID         \r\n";
            SQL = SQL + "     , E.UNITNEW1                                                                                          AS UNITNEW1 --   / 30 UNITNEW1  \r\n";
            SQL = SQL + "     , E.UNITNEW2                                                                                          AS UNITNEW2 --   / 31 UNITNEW2  \r\n";
            SQL = SQL + "     , 1                                                                                                   AS ACTNAL   --  / 32 ACTNAL     \r\n";
            SQL = SQL + "     , A.NAL                                                                                               AS NAL      --  / 33 NAL        \r\n";
            SQL = SQL + "     , ''                                                                                                  AS BI       --  / 34 BI         \r\n";
            SQL = SQL + "     , A.ORDERNO                                                                                           AS ORDERNO  --  / 35 ORDERNO                                              \r\n";
            SQL = SQL + " FROM  " + ComNum.DB_MED  + "ETC_JUSASUB 	A   \r\n";
            SQL = SQL + " 	  , " + ComNum.DB_MED  + "OCS_ORDERCODE B   \r\n";
            SQL = SQL + " 	  , " + ComNum.DB_MED  + "OCS_ODOSAGE 	C   \r\n";
            SQL = SQL + " 	  , " + ComNum.DB_PMPA + "BAS_SUN 		E   \r\n";
            SQL = SQL + " 	  , " + ComNum.DB_PMPA + "BAS_PATIENT 	F   \r\n";
            SQL = SQL + "	  , " + ComNum.DB_PMPA + "BAS_DOCTOR  	G   \r\n";
            SQL = SQL + " WHERE 1=1                                                               \r\n";

            SQL = SQL + "       AND 'ACTING' = '" + sType + "'                                      \r\n";
            SQL = SQL + "       AND B.SUCODE NOT IN ('NSA-CT')                                    \r\n";


            if (string.IsNullOrEmpty(strPtno))
            {
                SQL = SQL + "         AND A.ACTDATE   	BETWEEN " + ComFunc.covSqlDate(strFr, false);
                SQL = SQL + "                               AND " + ComFunc.covSqlDate(strTo, false);
            }
            else
            {
                SQL = SQL + "         AND A.PTNO =" + ComFunc.covSqlstr(strPtno, false);
            }
            //if (isCOVID == true)
            //{
            //    SQL = SQL + "   AND B.SUCODE IN ('CO19-AZ2') ";
            //}
            //else
            //{
            //    SQL = SQL + "   AND B.SUCODE NOT IN ('CO19-AZ2') ";
            //}

            if (isFlu == true)
            {
                SQL = SQL + "         AND A.PTNO IN ( SELECT PANO                                                \r\n";
                SQL = SQL + "                           FROM  " + ComNum.DB_PMPA + "OPD_MASTER                   \r\n";
                SQL = SQL + "                          WHERE ACTDATE BETWEEN " + ComFunc.covSqlDate(strFr, false);
                SQL = SQL + "                                            AND " + ComFunc.covSqlDate(strTo, false);
                SQL = SQL + "                            AND GBFLU_LTD IN ('Y','C')                                                 \r\n";
                SQL = SQL + "                        ) \r\n";
            }
            else
            {
                SQL = SQL + "         AND A.PTNO NOT IN ( SELECT PANO                                                \r\n";
                SQL = SQL + "                               FROM  " + ComNum.DB_PMPA + "OPD_MASTER                   \r\n";
                SQL = SQL + "                              WHERE ACTDATE BETWEEN " + ComFunc.covSqlDate(strFr, false);
                SQL = SQL + "                                                AND " + ComFunc.covSqlDate(strTo, false);
                SQL = SQL + "                                AND GBFLU_LTD IN ('Y','C')                                                  \r\n";
                SQL = SQL + "                        ) \r\n";
            }
            SQL = SQL + "	AND A.GBACT      	= 'Y'                                            \r\n";
            SQL = SQL + "   AND A.ORDERCODE 	= B.ORDERCODE                                    \r\n";
            SQL = SQL + "   AND A.DOSCODE 		= C.DOSCODE(+)                                   \r\n";

            // 예방접종일경우
            if (isVcc == true)
            {
                SQL = SQL + "   AND A.SUCODE     IN (                                            \r\n";
                SQL = SQL + "   					 SELECT JEPCODE                              \r\n";
                SQL = SQL + "   					   FROM KOSMOS_OCS.ETC_JUSACODE              \r\n";
                SQL = SQL + "   					  GROUP BY JEPCODE                           \r\n";
                SQL = SQL + "						)                                            \r\n";
            }

            SQL = SQL + "   AND A.PTNO 			= F.PANO                                         \r\n";
            SQL = SQL + "   AND (                                                                \r\n";
            SQL = SQL + "          B.BUN 	   = '20'                                            \r\n";
            SQL = SQL + "       OR B.ORDERCODE IN ('MG10060','A-POCS','A-MACA','A-POCSY','A-POSY5')                  \r\n";
            SQL = SQL + "       OR B.ORDERCODE IN (                                              \r\n";
            SQL = SQL + "						    SELECT JEPCODE                               \r\n";
            SQL = SQL + "						      FROM KOSMOS_ADM.DRUG_SETCODE               \r\n";
            SQL = SQL + "						      WHERE GUBUN = '20'                         \r\n";
            SQL = SQL + "						        AND DELDATE IS NULL                      \r\n";
            SQL = SQL + "				          )                                              \r\n";
            SQL = SQL + "		)                                                                \r\n";
            SQL = SQL + "	 AND B.SUCODE  = E.SUNEXT                                            \r\n";
            SQL = SQL + "	 AND A.DRCODE  = G.DRCODE                                            \r\n";
            

            if (part == enmOCS_OORDER_InjecMain_Part.XRAY)
            {

                //2017.01.03.김홍록: POCS를 영상의학과에서 조히 가능하도록 OR A.DOSCODE IN ('010501') 추가함
                SQL = SQL + "         AND (A.DOSCODE IN ( SELECT DOSCODE FROM " + ComNum.DB_MED + "OCS_ODOSAGE WHERE WARDCODE ='RD') OR (A.DOSCODE IN ('499915')) OR A.DOSCODE IN ('010501') )                                     \r\n";
                SQL = SQL + "	      AND PART IN('3')                                           \r\n";
            }
            else if (part == enmOCS_OORDER_InjecMain_Part.IJRM)
            {
                SQL = SQL + "         AND A.DRCODE NOT IN ('1107','1125','6108','6109')                                                                                                                  \r\n";
                SQL = SQL + "         AND A.DOSCODE NOT IN ( SELECT DOSCODE FROM " + ComNum.DB_MED + "OCS_ODOSAGE WHERE DOSFULLCODE LIKE '%항암주사실%' OR DOSFULLCODE LIKE '%약국조제(항암)%' )                    \r\n";
                SQL = SQL + "         AND  (                            \r\n";
                SQL = SQL + "               RTRIM(A.DOSCODE) LIKE '9%1' \r\n";
                SQL = SQL + "            OR RTRIM(A.DOSCODE) = '999999' \r\n";
                //SQL = SQL + "            OR RTRIM(A.DOSCODE) = '930116' \r\n";
                SQL = SQL + "            OR RTRIM(A.DOSCODE) = '930115' \r\n";
                SQL = SQL + "            OR RTRIM(A.DOSCODE) = '930101' \r\n";
                SQL = SQL + "            OR CASE WHEN A.DEPTCODE = 'PD' \r\n";
                SQL = SQL + "                     AND A.ORDERCODE  IN (                                 \r\n";
                SQL = SQL + "                                         SELECT JEPCODE                    \r\n";
                SQL = SQL + "                                            FROM KOSMOS_ADM.DRUG_SETCODE   \r\n";
                SQL = SQL + "                                            WHERE GUBUN = '20'             \r\n";
                SQL = SQL + "                                              AND DELDATE IS NULL          \r\n";
                SQL = SQL + "									       )  THEN  NVL(A.DOSCODE,'@@##@@') \r\n";
                SQL = SQL + "			          ELSE '@@##'                                           \r\n";
                SQL = SQL + "		        END             = NVL(A.DOSCODE,'@@##@@')                   \r\n";
                SQL = SQL + "            )                                                              \r\n";

            }
            else if (part == enmOCS_OORDER_InjecMain_Part.CANCER)
            {
                //SQL = SQL + "         AND A.DOSCODE IN ( SELECT DOSCODE FROM " + ComNum.DB_MED + "OCS_ODOSAGE WHERE DOSFULLCODE LIKE '%항암주사실%')                    \r\n";
                SQL = SQL + "         AND A.DOSCODE IN ( SELECT DOSCODE FROM " + ComNum.DB_MED + "OCS_ODOSAGE WHERE DOSFULLCODE LIKE '%항암주사실%' OR DOSFULLCODE LIKE '%약국조제(항암)%' )                    \r\n";
            }
            SQL = SQL + "	 )                                                                          \r\n"; 

            if (type == enmOCS_OORDER_InjecMain_Type.RECIPT)
            {
                SQL = SQL + "	 ORDER BY SNAME                                                         \r\n";
            }
            else if (type == enmOCS_OORDER_InjecMain_Type.ACTING)
            {
                SQL = SQL + "	 ORDER BY ACTDATE,ACTTIME, SNAME                                        \r\n";
            }
            else if (type == enmOCS_OORDER_InjecMain_Type.MULTI)
            {
                SQL = SQL + "	 ORDER BY BDATE, SNAME                                                  \r\n";
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
      
        public DataSet sel_OCS_IORDER_VCC(PsmhDb pDbCon, string strFDATE, string strTDATE, bool isNotPD = false   )
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                                                                                                 \r\n";
            SQL += " 	    A.WARDCODE					-- 01                                                                                               \r\n";
            SQL += " 	  , TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE					-- 01                                                                                                   \r\n";
            SQL += "      , A.PTNO                   -- 02                                                                                                  \r\n";
            SQL += "      , E.SNAME                  -- 03                                                                                                  \r\n";
            SQL += "      , A.DEPTCODE               -- 04                                                                                                  \r\n";
            SQL += "      , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.STAFFID)                      AS DRNAME   -- 05                                               \r\n";
            SQL += "	  ,	DECODE(KOSMOS_OCS.FC_ETC_ALLERGY_MST_CHK(A.PTNO),'Y','알','')   AS ALLERGY  -- 03 ALLERGY                                       \r\n";
            SQL += "      , DECODE(TRIM(A.ORDERCODE),'PD0511','Y','')                       AS ROTA     -- 06                                               \r\n";
            SQL += "      , (                                                                                                                               \r\n";
            SQL += "            SELECT CASE WHEN COUNT(VDATE) > 0 THEN 'Y'                                                                                  \r\n";
            SQL += "                        ELSE  ''  END                                                                                                   \r\n";
            SQL += "              FROM KOSMOS_PMPA.VACCINE_TTPREVEN T                                                                                       \r\n";
            SQL += "             WHERE T.PANO  = A.PTNO                                                                                                     \r\n";
            SQL += "               AND T.VCODE = D.VCNCOD                                                                                                   \r\n";
            SQL += "               AND T.VDATE = A.BDATE                                                                                                    \r\n";
            SQL += "      	) 										    AS TDATE                                                                            \r\n";
            SQL += "      , CASE WHEN NVL(B.ORDERNAMES,'Q') ='Q' THEN TRIM(A.ORDERCODE) || ' ▶ ' || TRIM(B.ORDERNAME)                                      \r\n";
            SQL += "      	    ELSE                                  TRIM(A.ORDERCODE) || ' ▶ ' || TRIM(B.ORDERNAME) || ' ▶ ' || TRIM(B.ORDERNAMES)      \r\n";
            SQL += "      	    END   	AS ORDERNM   -- 06                                                                                                  \r\n";
            SQL += "      , C.DOSNAME				 -- 07                                                                                                  \r\n";
            SQL += "      , A.QTY                    -- 08                                                                                                  \r\n";
            SQL += "      , A.NAL                    -- 09                                                                                                  \r\n";
            SQL += "      , DECODE(A.REMARK,NULL,'(예방)',A.REMARK || ' (예방)') AS REMARK                                                                  \r\n";
            SQL += "      , DECODE(NVL(E.JUSAMSG,'Q'),'Q','','◎')  			 AS MSGYN                                                                   \r\n";
            SQL += "      , E.JUSAMSG											 AS MSG    --15                                                             \r\n";
            SQL += "      , D.VCNCOD                                                                                                                        \r\n";
            SQL += "      , A.BI                                                                                                                            \r\n";
            SQL += "      , KOSMOS_OCS.FC_BI_NM(A.BI) AS BI_NAME -- 05                                                                                      \r\n";
            SQL += "      , A.ROWID      AS ROW_ID                                                                                                          \r\n";
            SQL += "   FROM KOSMOS_OCS.OCS_IORDER 	         A                                                                                              \r\n";
            SQL += "      , KOSMOS_OCS.OCS_ORDERCODE         B                                                                                              \r\n";
            SQL += "      , KOSMOS_OCS.OCS_ODOSAGE 	         C                                                                                              \r\n";
            SQL += "      ,( SELECT JEPCODE                                                                                                                 \r\n";
            SQL += "              , VCNCOD                                                                                                                  \r\n";
            SQL += "           FROM KOSMOS_OCS.ETC_JUSACODE                                                                                                 \r\n";
            SQL += "           GROUP BY JEPCODE, VCNCOD)     D                                                                                              \r\n";
            SQL += "      , KOSMOS_PMPA.BAS_PATIENT          E                                                                                              \r\n";
            SQL += "  WHERE 1=1                                                                                                                             \r\n";
            SQL += "    AND A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
            SQL += "                    AND " + ComFunc.covSqlDate(strTDATE, false);
            SQL += "    AND A.ORDERCODE = B.ORDERCODE                                                                                                       \r\n";
            SQL += "    AND A.DOSCODE   = C.DOSCODE                                                                                                         \r\n";
            SQL += "    AND A.SUCODE    = D.JEPCODE                                                                                                         \r\n";
            SQL += "    AND A.PTNO	   = E.PANO                                                                                                             \r\n";
            SQL += "    AND (A.GBIOE NOT IN ('E','EI') OR A.GBIOE IS NULL)                                                                                  \r\n";

            if (isNotPD == true)
            {
                SQL += "    AND A.DEPTCODE NOT IN ('PD')                                                                                                    \r\n";
            }

            SQL += "    AND (A.BUN = '20' OR A.ORDERCODE IN (                                                                                               \r\n";
            SQL += " 										 SELECT JEPCODE                                                                                 \r\n";
            SQL += " 										   FROM KOSMOS_ADM.DRUG_SETCODE                                                                 \r\n";
            SQL += " 										  WHERE GUBUN = '20'                                                                            \r\n";
            SQL += " 										    AND DELDATE IS NULL                                                                         \r\n";
            SQL += " 										)                                                                                               \r\n";
            SQL += "  	   )                                                                                                                                \r\n";
            SQL += "  ORDER BY A.BDATE, A.PTNO                                                                                                              \r\n";

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

        public DataSet sel_VACCINE_TPATIENT(PsmhDb pDbCon, string strPano)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                     \r\n";
            SQL += "          PANO         -- 등록번호                           \r\n ";
            SQL += "        , PPERID       -- 피접종자주민번호                   \r\n ";
            SQL += "        , PNAME        -- 피접종자성명                       \r\n ";
            SQL += "        , HPERID       -- 보호자주민번호                     \r\n ";
            SQL += "        , RELA         -- 보호자와의관계                     \r\n ";
            SQL += "        , PTELNO1      -- 전화번호(지역)                     \r\n ";
            SQL += "        , PTELNO2      -- 전화번호(국번)                     \r\n ";
            SQL += "        , PTELNO3      -- 전화번호(번호)                     \r\n ";
            SQL += "        , PLNO         -- 우편번호                           \r\n ";
            SQL += "        , PADD1        -- 우편번호주소                       \r\n ";
            SQL += "        , PADD2        -- 상세주소                           \r\n ";
            SQL += "        , EMAIL        -- 전자우편                           \r\n ";
            SQL += "        , MPHONE1      -- 핸드폰(식별)                       \r\n ";
            SQL += "        , MPHONE2      -- 핸드폰(국번)                       \r\n ";
            SQL += "        , MPHONE3      -- 핸드폰(번호)                       \r\n ";
            SQL += "        , BABYGUBUN    -- 쌍둥이구분                         \r\n ";
            SQL += "        , BIRTHDAY     -- 실제생년월일                       \r\n ";
            SQL += "        , LAST_UPDATE  -- 최종수정일자                       \r\n ";
            SQL += "        , PINFOUSEDYON -- 개인정보 동의여부                  \r\n ";
            SQL += "        , ANAME        -- 접종행위자                         \r\n ";
            SQL += "        , DOCTOR_NAME  -- 예진의사명                         \r\n ";
            SQL += "        , HNAME        -- 보호자성명                         \r\n ";
            SQL += "        , PPERID_OLD   -- 신생아 주민번호(old)               \r\n ";
            SQL += "        , PPERID2      -- 피접종자주민번호 암호화            \r\n ";
            SQL += "        , HPERID2      -- 보호자주민번호 암호화              \r\n ";
            SQL += "        , PPERID_OLD2  -- 신생아 주민번호(old) 암호화        \r\n ";
            SQL += "        , GUBUN        -- 임시구분                           \r\n ";
            SQL += "        , PPERID_OLD3  -- 신생아 주민번호(old) 암호화 백업   \r\n ";
            SQL += "        , JINGUBUN     -- 노인접종대상 (2017.09.19.김홍록: 고시에 의해 추가함                      \r\n ";
            SQL += "        , NEXTCALL     -- 다음예방접종 문자 체크 (2019-01-29. 김경동: 고시에 의해 추가함                      \r\n ";
            SQL += "        , ALLERGY      -- 알러지관련 문자 체크 (2019-01-29. 김경동: 고시에 의해 추가함                      \r\n ";


            SQL += "  FROM KOSMOS_PMPA.VACCINE_TPATIENT /** 피접종자 인적정보*/ \r\n";
            SQL += " WHERE 1=1                                                  \r\n";
            SQL += "   AND PANO = " + ComFunc.covSqlstr(strPano,false);
            SQL += "   AND LAST_UPDATE = (                                      \r\n";
            SQL += "                      SELECT MAX(LAST_UPDATE)               \r\n";
            SQL += "                        FROM KOSMOS_PMPA.VACCINE_TPATIENT   \r\n";
            SQL += "                       WHERE PANO = " + ComFunc.covSqlstr(strPano, false);
            SQL += "                     )                                      \r\n";




            // 2017.07.13. 김홍록 : VB에서는 VACCINE_TPATIENT 값이 없으면 BAS_PATIENT 에서 값을 갖고 오지만 그럴 필요가 없는 다고 판단함 질병관리 본부로 보낼 때 값이 존재 하지 않으면  데이터가 를 못보냄 고로 BAS_PATIENT에서 값을 읽어 오는 것은 의미가 없음.
            //SQL = "";
            //SQL += "  WITH T AS                                                                                                         \r\n";
            //SQL += "  (                                                                                                                 \r\n";
            //SQL += "         SELECT                                                                                                     \r\n";
            //SQL += "               PANO                 -- 등록번호                                                                     \r\n";
            //SQL += "             , PNAME                -- 피접종자성명                                                                 \r\n";
            //SQL += "             , DECODE(NVL(TRIM(PPERID2),'N'),'N',PPERID,PPERID2) 			AS PPERID2 	   -- 피접종자주민번호      \r\n";
            //SQL += "             , ''															AS JUMIN1                               \r\n";
            //SQL += "             , ''															AS JUMIN2                               \r\n";
            //SQL += "             , ''															AS JUMIN3                               \r\n";
            //SQL += "             , DECODE(NVL(TRIM(PPERID_OLD2),'N'),'N',PPERID_OLD,PPERID_OLD2) AS PPERID_OLD2 -- 신생아주민번호       \r\n";
            //SQL += "             , BIRTHDAY             -- 실제생년월일                                                                 \r\n";
            //SQL += "             , PTELNO1              -- 전화번호(지역)                                                               \r\n";
            //SQL += "             , PTELNO2              -- 전화번호(국번)                                                               \r\n";
            //SQL += "             , PTELNO3              -- 전화번호(번호)                                                               \r\n";
            //SQL += "             , DECODE(NVL(TRIM(HPERID2),'N'),'N',HPERID,HPERID2) 			AS HPERID2 -- 보호자주민번호            \r\n";
            //SQL += "             , HNAME                -- 보호자성명                                                                   \r\n";
            //SQL += "             , RELA                 -- 보호자와의관계                                                               \r\n";
            //SQL += "             , BABYGUBUN            -- 쌍둥이구분                                                                   \r\n";
            //SQL += "             , MPHONE1              -- 핸드폰(식별)                                                                 \r\n";
            //SQL += "             , MPHONE2              -- 핸드폰(국번)                                                                 \r\n";
            //SQL += "             , MPHONE3              -- 핸드폰(번호)                                                                 \r\n";
            //SQL += "             , PLNO                 -- 우편번호                                                                     \r\n";
            //SQL += "             , PADD1                -- 우편번호주소                                                                 \r\n";
            //SQL += "             , PADD2                -- 상세주소                                                                     \r\n";
            //SQL += "             , ''															AS JUSO                                 \r\n";
            //SQL += "             , EMAIL                -- 전자우편                                                                     \r\n";
            //SQL += "             , PINFOUSEDYON         -- 개인정보 동의여부                                                            \r\n";
            //SQL += "          FROM KOSMOS_PMPA.VACCINE_TPATIENT /** 피접종자 인적정보*/                                                 \r\n";
            //SQL += "         WHERE 1=1                                                                                                  \r\n";
            //SQL += "           AND  PANO = " + ComFunc.covSqlstr(strPano, false);
            //SQL += "         UNION ALL                                                                                                  \r\n";
            //SQL += "         SELECT                                                                                                     \r\n";
            //SQL += "         	  A.PANO                                                                                                \r\n";
            //SQL += "         	, A.SNAME                                                                                               \r\n";
            //SQL += "         	, ''	 AS PPERID2 	   -- 피접종자주민번호                                                          \r\n";
            //SQL += "         	, JUMIN1 AS JUMIN2   	   -- 피접종자주민번호                                                          \r\n";
            //SQL += "         	, JUMIN2 AS JUMIN2   	   -- 피접종자주민번호                                                          \r\n";
            //SQL += "         	, JUMIN3 AS JUMIN3   	   -- 피접종자주민번호                                                          \r\n";
            //SQL += "         	, ''																		AS PPERID_OLD2              \r\n";
            //SQL += "         	, TO_CHAR(A.BIRTH,'YYYYMMDD')												AS BIRTHDAY                 \r\n";
            //SQL += "         	, '' AS PTELNO1                                                                                         \r\n";
            //SQL += "         	, '' AS PTELNO2                                                                                         \r\n";
            //SQL += "         	, '' AS PTELNO3                                                                                         \r\n";
            //SQL += "         	, '' AS HPERID2 -- 보호자주민번호                                                                       \r\n";
            //SQL += "         	, '' AS HNAME                -- 보호자성명                                                              \r\n";
            //SQL += "         	, '' AS RELA                 -- 보호자와의관계                                                          \r\n";
            //SQL += "         	, '' AS BABYGUBUN            -- 쌍둥이구분                                                              \r\n";
            //SQL += "             , '' AS MPHONE1              -- 핸드폰(식별)                                                            \r\n";
            //SQL += "             , '' AS MPHONE2              -- 핸드폰(국번)                                                            \r\n";
            //SQL += "             , '' AS MPHONE3              -- 핸드폰(번호)                                                            \r\n";
            //SQL += "         	, A.ZIPCODE1 || A.ZIPCODE2  AS PLNO                                                                     \r\n";
            //SQL += "         	, ''						AS PADD1                                                                    \r\n";
            //SQL += "         	, '' 						AS PADD2                                                                    \r\n";
            //SQL += "         	, A.JUSO					AS JUSO                                                                     \r\n";
            //SQL += "         	, A.EMAIL   				AS EMAIL                                                                    \r\n";
            //SQL += "         	, ''						AS PINFOUSEDYON                                                             \r\n";
            //SQL += "            FROM KOSMOS_PMPA.BAS_PATIENT A                                                                           \r\n";
            //SQL += "              ,  KOSMOS_PMPA.BAS_MAILNEW B                                                                           \r\n";
            //SQL += "           WHERE A.PANO = " + ComFunc.covSqlstr(strPano, false);
            //SQL += "             AND A.ZIPCODE1 || A.ZIPCODE2 = B.MAILCODE(+)                                                            \r\n";
            //SQL += "      )                                                                                                              \r\n";
            //SQL += "  SELECT                                                                                                             \r\n";
            //SQL += "  	  PANO                AS PANO                                                                                   \r\n";
            //SQL += "  	, PNAME               AS PNAME                                                                                  \r\n";
            //SQL += "  	, MAX(PPERID2		) AS PPERID2		                                                                        \r\n";
            //SQL += "  	, MAX(JUMIN1        ) AS JUMIN1                                                                                 \r\n";
            //SQL += "  	, MAX(JUMIN2        ) AS JUMIN2                                                                                 \r\n";
            //SQL += "  	, MAX(JUMIN3        ) AS JUMIN3                                                                                 \r\n";
            //SQL += "  	, MAX(PPERID_OLD2   ) AS PPERID_OLD2                                                                            \r\n";
            //SQL += "  	, MAX(BIRTHDAY      ) AS BIRTHDAY                                                                               \r\n";
            //SQL += "  	, MAX(PTELNO1       ) AS PTELNO1                                                                                \r\n";
            //SQL += "  	, MAX(PTELNO2       ) AS PTELNO2                                                                                \r\n";
            //SQL += "  	, MAX(PTELNO3       ) AS PTELNO3                                                                                \r\n";
            //SQL += "  	, MAX(HPERID2       ) AS HPERID2                                                                                \r\n";
            //SQL += "  	, MAX(HNAME         ) AS HNAME                                                                                  \r\n";
            //SQL += "  	, MAX(RELA          ) AS RELA                                                                                   \r\n";
            //SQL += "  	, MAX(BABYGUBUN     ) AS BABYGUBUN                                                                              \r\n";
            //SQL += "  	, MAX(MPHONE1       ) AS MPHONE1                                                                                \r\n";
            //SQL += "  	, MAX(MPHONE2       ) AS MPHONE2                                                                                \r\n";
            //SQL += "  	, MAX(MPHONE3       ) AS MPHONE3                                                                                \r\n";
            //SQL += "  	, MAX(PLNO          ) AS PLNO                                                                                   \r\n";
            //SQL += "  	, MAX(PADD1         ) AS PADD1                                                                                  \r\n";
            //SQL += "  	, MAX(PADD2         ) AS PADD2                                                                                  \r\n";
            //SQL += "  	, MAX(JUSO          ) AS JUSO                                                                                   \r\n";
            //SQL += "  	, MAX(EMAIL         ) AS EMAIL                                                                                  \r\n";
            //SQL += "  	, MAX(PINFOUSEDYON  ) AS PINFOUSEDYON                                                                           \r\n";
            //SQL += "    FROM T                                                                                                          \r\n";
            //SQL += "    GROUP BY PANO, PNAME                                                                                            \r\n";            

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


        public DataTable sel_VACCINE_TPATIENT_LASTUPDATE(PsmhDb pDbCon, string strPano)
        {

            DataTable dt = null;

            SQL = "";
            SQL += " SELECT MAX(LAST_UPDATE) LSDATE         \r\n";
            SQL += "   FROM KOSMOS_PMPA.VACCINE_TPATIENT    \r\n";
            SQL += "  WHERE 1=1                             \r\n";
            SQL += "    AND PANO = " + ComFunc.covSqlstr(strPano, false);

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

        public DataSet sel_ETC_JUSACODE(PsmhDb pDbCon, bool isDelete = false)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                                                         \r\n";
            SQL += "       CASE WHEN DELDATE IS NULL THEN ''                                                        \r\n";
            SQL += "            ELSE 'True' END	  													  AS CHK        \r\n";
            SQL += "      , JEPCODE                                                                   AS JEPCODE    \r\n";
            SQL += "      , JEPNAME                                                                   AS JEPNAME    \r\n";
            SQL += "      , GUBUN || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('ETC_JUSA_주사명',GUBUN) AS GUBUN_NAME \r\n";
            SQL += "      , DECODE(                                                                                 \r\n";
            SQL += "      		  NVL(VCNCOD,'D')                                                                   \r\n";
            SQL += "      		 ,'D',''                                                                            \r\n";
            SQL += "              , VCNCOD|| '.' || (                                                               \r\n";
            SQL += " 		     					 SELECT VCNNAM                                                  \r\n";
            SQL += " 		                           FROM KOSMOS_PMPA.VACCINE_PCKIND P                            \r\n";
            SQL += " 		                          WHERE 1=1                                                     \r\n";
            SQL += " 		                            AND P.VCNCOD = J.VCNCOD                                     \r\n";
            SQL += " 						 		)                                                               \r\n";
            SQL += " 		)																		 AS VCN_NAME    \r\n";
            SQL += "      , TO_CHAR(DELDATE,'YYYY-MM-DD') 											 AS DELDATE     \r\n";
            SQL += "      , ROWID                                                                                   \r\n";
            SQL += "      , ''                                                                       AS MODI        \r\n";
            SQL += "      FROM " + ComNum.DB_MED+ "ETC_JUSACODE J                                                   \r\n";
            SQL += "     WHERE 1=1                                                                                  \r\n";

            if (isDelete == false)
            {
                SQL += "       AND DELDATE IS NULL                                                                  \r\n";
            }
            
            SQL += "     ORDER BY GUBUN, JEPCODE                                                                    \r\n";
                                                                                                        
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

        public DataSet sel_DRUG_JEP(PsmhDb pDbCon, string strCode)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "SELECT                                      \r\n";
            SQL += "       0					AS SEQ          \r\n";
            SQL += "	  ,JEPNAME 				AS CODENAME     \r\n";
            SQL += "  FROM KOSMOS_ADM.DRUG_JEP                  \r\n";
            SQL += " WHERE 1=1                                  \r\n";
            SQL += "   AND JEPCODE = " + ComFunc.covSqlstr(strCode, false);
            SQL += "UNION ALL                                   \r\n";
            SQL += "SELECT                                      \r\n";
            SQL += "	    1					AS SEQ          \r\n";
            SQL += "	 ,	B.SUNAMEK           AS CODENAME     \r\n";
            SQL += "  FROM KOSMOS_ADM.DRUG_CONVRATE A           \r\n";
            SQL += "     , KOSMOS_PMPA.BAS_SUN 		B           \r\n";
            SQL += " WHERE 1=1                                  \r\n";
            SQL += "   AND B.SUNEXT = A.SUNEXT(+)               \r\n";
            SQL += "   AND B.SUNEXT = " + ComFunc.covSqlstr(strCode, false);
            SQL += "   ORDER BY SEQ                             \r\n";          
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

        public DataSet sel_VACCINE_TLOTNO_S(PsmhDb pDbCon, string strCode)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                        \r\n ";
            SQL += "       ENDATE              --본원사용종료일자   \r\n ";
            SQL += "     , USED                --본원사용여부       \r\n ";
            SQL += "     , LOTNO               --로트번호           \r\n ";
            SQL += "     , LOPPERID            --백신유효기간       \r\n ";
            SQL += "     , UNIT                --약품규격           \r\n ";
            SQL += "     , VCNCOD              --접종코드           \r\n ";
            SQL += "     , VACODE              --백신코드           \r\n ";
            SQL += "     , VANAME              --백신명             \r\n ";
            SQL += "     , VENDORCODE          --제조사코드         \r\n ";
            SQL += "     , VENDORNAME          --제조사명           \r\n ";
            SQL += "     , ROWID               --ROWID              \r\n ";
            SQL += "  FROM KOSMOS_PMPA.VACCINE_TLOTNO_S /** 로트번호*/ \r\n";
            SQL += " WHERE 1=1                                      \r\n ";
            SQL += "   AND LOTNO  = " + ComFunc.covSqlstr(strCode,false);
            SQL += " ORDER BY VCNCOD, LOTNO, VACODE                 \r\n ";

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

        public string sel_VACCINE_TPATIENT_RowID(PsmhDb pDbCon, string[] arrUserInfo)
        {
            string s = string.Empty;

            DataTable dt = null;

            string strPPERID = clsAES.AES(arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.PPERID]);
            string strHPERID2 = clsAES.AES(arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.HPERID]);


            SQL = "";
            SQL += " SELECT ROWID                           \r\n";
            SQL += "   FROM KOSMOS_PMPA.VACCINE_TPATIENT    \r\n";
            SQL += "  WHERE 1=1                             \r\n";

            if (arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.PPERID2].Length == 13)
            {
                SQL += "    AND PPERID2   = " + ComFunc.covSqlstr(strPPERID, false);
            }
            else
            {
                SQL += "    AND (PPERID2  = '" + strPPERID  + "' OR SUBSTR(PPERID,1,7) = '" + arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.PPERID].Substring(0,7) + "') \r\n ";
                SQL += "    AND (HPERID2  = '" + strHPERID2 + "' OR HPERID             = '" + arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.HPERID]                + "') \r\n ";
                SQL += "    AND BABYGUBUN = " + ComFunc.covSqlstr(arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.BABYGUBUN], false);
            }

            SQL += "    AND LAST_UPDATE = (                                               \r\n";
            SQL += "                       SELECT MAX(LAST_UPDATE)                        \r\n";
            SQL += "                         FROM KOSMOS_PMPA.VACCINE_TPATIENT            \r\n";
            SQL += "                        WHERE 1=1                                     \r\n";

            if (arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.PPERID2].Length == 13)
            {
                SQL += "    AND PPERID2   = " + ComFunc.covSqlstr(strPPERID, false);
            }
            else
            {
                SQL += "    AND (PPERID2  = '" + strPPERID + "' OR SUBSTR(PPERID,1,7) = '" + arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.PPERID].Substring(0, 7) + "') \r\n ";
                SQL += "    AND (HPERID2  = '" + strHPERID2 + "' OR HPERID             = '" + arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.HPERID] + "') \r\n ";
                SQL += "    AND BABYGUBUN = " + ComFunc.covSqlstr(arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.BABYGUBUN], false);
            }

            SQL += "                        )                  \r\n";



            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return "";
                }

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    s = dt.Rows[0]["ROWID"].ToString();
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }

            return s;
        }
        
        public string sel_VACCINE_TTPREVEN_RowID(PsmhDb pDbCon, string[] arrVcnIns)
        {
            string s = string.Empty;

            DataTable dt = null;

            string strPPERID    = clsAES.AES(arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.PPERID]);
            string strVCODE     = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.VCODE];
            string strVDATE     = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.VDATE];
            string strVKIND     = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.VKIND];
            string strHPERID    = clsAES.AES(arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.HPERID]);
            string strBABYGUBUN = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.BABYGUBUN];


            SQL = "";
            SQL += " SELECT  ROWID                          \r\n";
            SQL += "   FROM KOSMOS_PMPA.VACCINE_TTPREVEN    \r\n";
            SQL += "  WHERE 1=1                             \r\n";

            if (arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.PPERID2].Length == 13)
            {
                SQL += "    AND PPERID2             = " + ComFunc.covSqlstr(strPPERID, false);       // 피접종자

            }
            else
            {
                SQL += "    AND SUBSTR(PPERID,1,7)  = " + ComFunc.covSqlstr(arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.PPERID].Substring(0,7), false);       // 피접종자
            }

            SQL += "    AND HPERID2     = " + ComFunc.covSqlstr(strHPERID, false);      // 보호자
            SQL += "    AND VCODE       = " + ComFunc.covSqlstr(strVCODE, false);       // 접종코드
            SQL += "    AND VKIND       = " + ComFunc.covSqlstr(strVKIND, false);       // 접종차수
            SQL += "    AND VDATE       = " + ComFunc.covSqlstr(strVDATE, false);       // 접종일자
            SQL += "    AND BABYGUBUN   = " + ComFunc.covSqlstr(strBABYGUBUN, false);   // 아이구분

            //if (arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.PPERID].Length == 7)
            //{
            //    SQL += "    AND (PPERID2  = '" + strPPERID2 + "' OR SUBSTR(PPERID,1,7)  = '" + arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.PPERID] + "') \r\n";
            //    SQL += "    AND VCODE     = " + ComFunc.covSqlstr(strVCODE, false);
            //    SQL += "    AND VDATE     = " + ComFunc.covSqlstr(strVDATE, false);
            //    SQL += "    AND (HPERID2   = '" + strHPERID2 + "' OR HPERID = '" + arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.HPERID] + "') \r\n";
            //    SQL += "    AND BABYGUBUN = " + ComFunc.covSqlstr(strBABYGUBUN, false);
            //}
            //else
            //{
            //    SQL += "    AND (PPERID2   = '" + strPPERID2 + "' OR PPERID  = '" + arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.PPERID] + "') \r\n";
            //    SQL += "    AND VCODE     = " + ComFunc.covSqlstr(strVCODE, false);
            //    SQL += "    AND VDATE     = " + ComFunc.covSqlstr(strVDATE, false);
            //}

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return "";
                }

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    s = dt.Rows[0]["ROWID"].ToString();
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }

            return s;
        }

        public string save_VACCINE_TTPREVEN(PsmhDb pDbCon, string strPANO, string strJumin, string[] arrVcnIns, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
            string strROWID = sel_VACCINE_TTPREVEN_RowID(pDbCon, arrVcnIns);

            string strVCODE         = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.VCODE];
            string strVDATE         = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.VDATE];
            string strPPERID        = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.PPERID];
            string strHPERID        = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.HPERID];
            string strVKIND         = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.VKIND]; 
            string strVACODE        = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.VACODE];
            string strVENDORCODE    = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.VENDORCODE]; 
            string strLOTNO         = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.LOTNO];
            string strLOPPERID      = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.LOPPERID];
            string strANAME         = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.ANAME];
            string strAMETHOD       = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.AMETHOD];
            string strAPART         = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.APART];
            string strBABYGUBUN     = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.BABYGUBUN];
            string strVOLUMN        = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.VOLUMN];
            string strP_AGE         = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.P_AGE];
            string strP_MONTH       = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.P_MONTH];
            string strPLNO          = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.PLNO];
            string strDOCTOR_NAME   = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.DOCTOR_NAME];
            string strBILLYON       = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.BILLYON];
            string strVANAME        = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.VANAME];
            string strVENDORNAME    = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.VENDORNAME];
            string strUNIT          = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.UNIT];
            string strSENDYON       = "Y";
            string strGBSTS         = "V";
            string strREVCNYON      = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.FLAG];
            //string strNEXTCALL      = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.NEXTCALL];
            //string strALLERGY       = arrVcnIns[(int)enmVACCINE_TTPREVEN_RCV.ALLERGY];

            SQL = "";
            if (string.IsNullOrEmpty(strROWID) == false)
            {

                SQL += " UPDATE KOSMOS_PMPA.VACCINE_TTPREVEN            \r\n";
                SQL += "    SET VKIND       = " + ComFunc.covSqlstr(strVKIND         ,false) ;// 접종차수 
                SQL += "      , VACODE      = " + ComFunc.covSqlstr(strVACODE        ,false) ;// 백신코드
                SQL += "      , VENDORCODE  = " + ComFunc.covSqlstr(strVENDORCODE    ,false) ;// 제조사코드
                SQL += "      , LOTNO       = " + ComFunc.covSqlstr(strLOTNO         ,false) ;// 로트 번호
                SQL += "      , LOPPERID    = " + ComFunc.covSqlstr(strLOPPERID      ,false) ;// 백신 유효 기간
                SQL += "      , ANAME       = " + ComFunc.covSqlstr(strANAME         ,false) ;// 접종 행위자
                SQL += "      , AMETHOD     = " + ComFunc.covSqlstr(strAMETHOD       ,false) ;// 접종방법
                SQL += "      , APART       = " + ComFunc.covSqlstr(strAPART         ,false) ;// 접종부위
                SQL += "      , VOLUMN      = " + ComFunc.covSqlstr(strVOLUMN        ,false) ;// 접종용량
                SQL += "      , P_AGE       = " + ComFunc.covSqlstr(strP_AGE         ,false) ;// 환자 연령
                SQL += "      , P_MONTH     = " + ComFunc.covSqlstr(strP_MONTH       ,false) ;// 환자 개월수
                SQL += "      , PLNO        = " + ComFunc.covSqlstr(strPLNO          ,false) ;// 우편 번호
                SQL += "      , DOCTOR_NAME = " + ComFunc.covSqlstr(strDOCTOR_NAME   ,false) ;// 예진의사명
                SQL += "      , BILLYON     = " + ComFunc.covSqlstr(strBILLYON       ,false) ;// 청구여부
                SQL += "      , VANAME      = " + ComFunc.covSqlstr(strVANAME        ,false) ;// 백신명
                SQL += "      , VENDORNAME  = " + ComFunc.covSqlstr(strVENDORNAME    ,false) ;// 제조사명
                SQL += "      , UNIT        = " + ComFunc.covSqlstr(strUNIT          ,false) ;// 적정 사용량
                SQL += "      , SENDYON     = " + ComFunc.covSqlstr(strSENDYON       ,false) ;// 전송여부
                SQL += "      , REVCNYON    = " + ComFunc.covSqlstr(strREVCNYON     , false) ;// 재접종여부
                //SQL += "      , NEXTCALL    = " + ComFunc.covSqlstr(strNEXTCALL, false);// 다음접종알림 SMS 동의여부
                //SQL += "      , ALLERGY     = " + ComFunc.covSqlstr(strALLERGY, false);// 알러지반응관련 SMS 동의여부 
                SQL += "  WHERE ROWID       = " + ComFunc.covSqlstr(strROWID        , false);

            }
            else
            {
                SQL += " INSERT INTO KOSMOS_PMPA.VACCINE_TTPREVEN ( \r\n";
                SQL += "         PANO                               \r\n";    
                SQL += "       , PPERID                             \r\n";
                SQL += "       , PPERID2                            \r\n";
                SQL += "       , HPERID                             \r\n";
                SQL += "       , HPERID2                            \r\n";
                SQL += "       , VCODE                              \r\n";
                SQL += "       , VDATE                              \r\n";
                SQL += "       , VKIND                              \r\n";
                SQL += "       , VACODE                             \r\n";
                SQL += "       , VENDORCODE                         \r\n";
                SQL += "       , LOTNO                              \r\n";
                SQL += "       , LOPPERID                           \r\n";
                SQL += "       , ANAME                              \r\n";
                SQL += "       , AMETHOD                            \r\n";
                SQL += "       , APART                              \r\n";
                SQL += "       , VOLUMN                             \r\n";
                SQL += "       , P_AGE                              \r\n";    
                SQL += "       , P_MONTH                            \r\n";
                SQL += "       , PLNO                               \r\n";
                SQL += "       , DOCTOR_NAME                        \r\n";
                SQL += "       , BABYGUBUN                          \r\n";
                SQL += "       , BILLYON                            \r\n";
                SQL += "       , VANAME                             \r\n";
                SQL += "       , VENDORNAME                         \r\n";
                SQL += "       , UNIT                               \r\n";
                SQL += "       , SENDYON                            \r\n";
                SQL += "       , REVCNYON                           \r\n";
                SQL += "       , GBSTS)                              \r\n";
                //SQL += "       , NEXTCALL                           \r\n";
                //SQL += "       , ALLERGY )                          \r\n";
                SQL += "       VALUES (                             \r\n";

                SQL += "       " + ComFunc.covSqlstr(strPANO, false);

                if (strJumin.Length == 13)
                {
                    SQL += "       " + ComFunc.covSqlstr(strJumin.Substring(0, 7) + "******", true);
                    SQL += "       " + ComFunc.covSqlstr(clsAES.AES(strJumin), true);
                }
                else
                {
                    SQL += "       " + ComFunc.covSqlstr(strPPERID.Substring(0, 7) + "******", true);
                    SQL += "       " + ComFunc.covSqlstr(clsAES.AES(strPPERID), true);
                }

                SQL += "       " + ComFunc.covSqlstr(strHPERID.Substring(0, 7) + "******", true);
                SQL += "       " + ComFunc.covSqlstr(clsAES.AES(strHPERID), true);
                SQL += "       " + ComFunc.covSqlstr(strVCODE, true) ;
                SQL += "       " + ComFunc.covSqlstr(strVDATE, true);
                SQL += "       " + ComFunc.covSqlstr(strVKIND, true);
                SQL += "       " + ComFunc.covSqlstr(strVACODE, true);
                SQL += "       " + ComFunc.covSqlstr(strVENDORCODE, true);
                SQL += "       " + ComFunc.covSqlstr(strLOTNO, true);
                SQL += "       " + ComFunc.covSqlstr(strLOPPERID, true);
                SQL += "       " + ComFunc.covSqlstr(strANAME, true);
                SQL += "       " + ComFunc.covSqlstr(strAMETHOD, true);
                SQL += "       " + ComFunc.covSqlstr(strAPART, true);
                SQL += "       " + ComFunc.covSqlstr(strVOLUMN, true);
                SQL += "       " + ComFunc.covSqlstr(strP_AGE, true);
                SQL += "       " + ComFunc.covSqlstr(strP_MONTH, true);
                SQL += "       " + ComFunc.covSqlstr(strPLNO, true);
                SQL += "       " + ComFunc.covSqlstr(strDOCTOR_NAME, true);
                SQL += "       " + ComFunc.covSqlstr(strBABYGUBUN, true);
                SQL += "       " + ComFunc.covSqlstr(strBILLYON, true);
                SQL += "       " + ComFunc.covSqlstr(strVANAME, true);
                SQL += "       " + ComFunc.covSqlstr(strVENDORNAME, true);
                SQL += "       " + ComFunc.covSqlstr(strUNIT, true);
                SQL += "       " + ComFunc.covSqlstr(strSENDYON, true);
                SQL += "       " + ComFunc.covSqlstr(strREVCNYON, true);
                SQL += "       " + ComFunc.covSqlstr(strGBSTS, true);
                //SQL += "       " + ComFunc.covSqlstr(strNEXTCALL, true);
                //SQL += "       " + ComFunc.covSqlstr(strALLERGY, true);
                SQL += "                    )               \r\n";

            }


           SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

           return SqlErr;
        }

        public string save_VACCINE_TPATIENT(PsmhDb pDbCon, string strPANO, string[] arrUserInfo, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
            string strRowid = sel_VACCINE_TPATIENT_RowID(pDbCon, arrUserInfo);

            string strPPERID       = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.PPERID].Substring(0, 7);
            string strBABYGUBUN    = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.BABYGUBUN];
            string strHPERID       = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.HPERID].Substring(0, 7);
            string strHPERID2      = clsAES.AES(arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.HPERID]);
            string strPPERID2      = clsAES.AES(arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.PPERID2]);
            string strPNAME        = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.PNAME];
            string strHNAME        = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.HNAME];
            string strRELA         = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.RELA];
            string strPTELNO1      = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.PTELNO1];
            string strPTELNO2      = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.PTELNO2];
            string strPTELNO3      = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.PTELNO3];
            string strPLNO         = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.PLNO];
            string strPADD1        = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.PADD1];
            string strPADD2        = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.PADD2];
            string strEMAIL        = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.EMAIL];
            string strMPHONE1      = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.MPHONE1];
            string strMPHONE2      = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.MPHONE2];
            string strMPHONE3      = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.MPHONE3];
            string strBIRTHDAY     = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.BIRTHDAY];
            string strLAST_UPDATE  = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.LAST_UPDATE];
            string strPINFOUSEDYON = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.PINFOUSEDYON];
            string strANAME        = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.ANAME];
            string strDOCTOR_NAME  = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.DOCTOR_NAME];

            string strNEXTCALL = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.NEXTCALL];
            string strALLERGY = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.ALLERGY];

            //if (arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.JINGUBUN] != null)
            //{
            //    string strOLDMAN = arrUserInfo[(int)enmVACCINE_TPATIENT_RCV.JINGUBUN]; // 2017.09.23.김홍록: 시행할 경우 가능 할 것으로 생각됨.
            //}

            string strGUBUN        = "N3";



            SQL = "";
            if (string.IsNullOrEmpty(strRowid) == false)
            {
                
                SQL += " UPDATE KOSMOS_PMPA.VACCINE_TPATIENT                                                             \r\n";
                SQL += "    SET PPERID       = " + ComFunc.covSqlstr(strPPERID + "******", false);
                SQL += "      , BABYGUBUN    = " + ComFunc.covSqlstr(strBABYGUBUN, false);
                SQL += "      , HPERID       = " + ComFunc.covSqlstr(strHPERID + "******", false);
                SQL += "      , HPERID2      = " + ComFunc.covSqlstr(strHPERID2, false);
                SQL += "      , PPERID2      = " + ComFunc.covSqlstr(strPPERID2, false);
                SQL += "      , PNAME        = " + ComFunc.covSqlstr(strPNAME, false);
                SQL += "      , HNAME        = " + ComFunc.covSqlstr(strHNAME, false);
                SQL += "      , RELA         = " + ComFunc.covSqlstr(strRELA, false);
                SQL += "      , PTELNO1      = " + ComFunc.covSqlstr(strPTELNO1, false);
                SQL += "      , PTELNO2      = " + ComFunc.covSqlstr(strPTELNO2, false);
                SQL += "      , PTELNO3      = " + ComFunc.covSqlstr(strPTELNO3, false);
                SQL += "      , PLNO         = " + ComFunc.covSqlstr(strPLNO, false);
                SQL += "      , PADD1        = " + ComFunc.covSqlstr(strPADD1, false);
                SQL += "      , PADD2        = " + ComFunc.covSqlstr(strPADD2, false);
                SQL += "      , EMAIL        = " + ComFunc.covSqlstr(strEMAIL, false);
                SQL += "      , MPHONE1      = " + ComFunc.covSqlstr(strMPHONE1, false);
                SQL += "      , MPHONE2      = " + ComFunc.covSqlstr(strMPHONE2, false);
                SQL += "      , MPHONE3      = " + ComFunc.covSqlstr(strMPHONE3, false);
                SQL += "      , BIRTHDAY     = " + ComFunc.covSqlstr(strBIRTHDAY, false);
                SQL += "      , LAST_UPDATE  = " + ComFunc.covSqlstr(strLAST_UPDATE, false);
                SQL += "      , PINFOUSEDYON = " + ComFunc.covSqlstr(strPINFOUSEDYON, false);
                SQL += "      , ANAME        = " + ComFunc.covSqlstr(strANAME, false);
                SQL += "      , DOCTOR_NAME  = " + ComFunc.covSqlstr(strDOCTOR_NAME, false);
                SQL += "      , NEXTCALL     = " + ComFunc.covSqlstr(strNEXTCALL, false);
                SQL += "      , ALLERGY      = " + ComFunc.covSqlstr(strALLERGY, false);
                SQL += "  WHERE ROWID        = " + ComFunc.covSqlstr(strRowid, false); ;
            }
            else
            {
                SQL += " INSERT INTO KOSMOS_PMPA.VACCINE_TPATIENT (  \r\n";
                SQL += "        PANO                                 \r\n";
                SQL += "      , PPERID                               \r\n";
                SQL += "      , PPERID2                              \r\n";
                SQL += "      , PNAME                                \r\n";
                SQL += "      , HPERID                               \r\n";
                SQL += "      , HPERID2                              \r\n";
                SQL += "      , HNAME                                \r\n";
                SQL += "      , RELA                                 \r\n";
                SQL += "      , PTELNO1                              \r\n";
                SQL += "      , PTELNO2                              \r\n";
                SQL += "      , PTELNO3                              \r\n";
                SQL += "      , PLNO                                 \r\n";
                SQL += "      , PADD1                                \r\n";
                SQL += "      , PADD2                                \r\n";
                SQL += "      , EMAIL                                \r\n";
                SQL += "      , MPHONE1                              \r\n";
                SQL += "      , MPHONE2                              \r\n";
                SQL += "      , MPHONE3                              \r\n";
                SQL += "      , BABYGUBUN                            \r\n";        
                SQL += "      , BIRTHDAY                             \r\n";
                SQL += "      , LAST_UPDATE                          \r\n";
                SQL += "      , PINFOUSEDYON                         \r\n";
                SQL += "      , ANAME                                \r\n";
                SQL += "      , DOCTOR_NAME                          \r\n";
                SQL += "      , GUBUN                                \r\n";
                SQL += "      , NEXTCALL                             \r\n";
                SQL += "      , ALLERGY                              \r\n";
                SQL += "      )                                      \r\n";
                SQL += "      VALUES (                               \r\n";
                SQL += "      " + ComFunc.covSqlstr(strPANO        , false);
                SQL += "      " + ComFunc.covSqlstr(strPPERID + "******", true);
                SQL += "      " + ComFunc.covSqlstr(strPPERID2     , true);
                SQL += "      " + ComFunc.covSqlstr(strPNAME       , true);
                SQL += "      " + ComFunc.covSqlstr(strHPERID + "******", true);
                SQL += "      " + ComFunc.covSqlstr(strHPERID2     , true);
                SQL += "      " + ComFunc.covSqlstr(strHNAME       , true);
                SQL += "      " + ComFunc.covSqlstr(strRELA        , true);
                SQL += "      " + ComFunc.covSqlstr(strPTELNO1     , true);
                SQL += "      " + ComFunc.covSqlstr(strPTELNO2     , true);
                SQL += "      " + ComFunc.covSqlstr(strPTELNO3     , true);
                SQL += "      " + ComFunc.covSqlstr(strPLNO        , true);
                SQL += "      " + ComFunc.covSqlstr(strPADD1       , true);
                SQL += "      " + ComFunc.covSqlstr(strPADD2       , true);
                SQL += "      " + ComFunc.covSqlstr(strEMAIL       , true);
                SQL += "      " + ComFunc.covSqlstr(strMPHONE1     , true);
                SQL += "      " + ComFunc.covSqlstr(strMPHONE2     , true);
                SQL += "      " + ComFunc.covSqlstr(strMPHONE3     , true);
                SQL += "      " + ComFunc.covSqlstr(strBABYGUBUN   , true);
                SQL += "      " + ComFunc.covSqlstr(strBIRTHDAY    , true);
                SQL += "      " + ComFunc.covSqlstr(strLAST_UPDATE , true);
                SQL += "      " + ComFunc.covSqlstr(strPINFOUSEDYON, true);
                SQL += "      " + ComFunc.covSqlstr(strANAME       , true);
                SQL += "      " + ComFunc.covSqlstr(strDOCTOR_NAME , true);
                SQL += "      " + ComFunc.covSqlstr(strGUBUN       , true);
                SQL += "      " + ComFunc.covSqlstr(strNEXTCALL    , true);
                SQL += "      " + ComFunc.covSqlstr(strALLERGY     , true);
                SQL += "      )                                             \r\n";


            }


            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        string[] getPHONE (PsmhDb pDbCon, string[] arrS_Tmp)
        {

            string[] arrS = null;

            if (arrS_Tmp == null)
            {
                arrS = new string[3];
                arrS[0] = "000";
                arrS[1] = "0000";
                arrS[2] = "0000";
            }
            else
            {
                if (arrS_Tmp.Length == 1)
                {
                    arrS = new string[3];
                    arrS[0] = arrS_Tmp[0];
                    arrS[1] = "0000";
                    arrS[2] = "0000";
                }
                else if (arrS_Tmp.Length == 2)
                {
                    arrS = new string[3];
                    arrS[0] = arrS_Tmp[0];
                    arrS[1] = arrS_Tmp[1];
                    arrS[2] = "0000";
                }
                else if (arrS_Tmp.Length == 3)
                {
                    arrS = arrS_Tmp;
                }
            }


            return arrS;
        }

        public string ins_NUR_HAPPYCALL_OPD(PsmhDb pDbCon, string strPANO      , string strBDATE   , string strDEPTCODE
                                         , string strGUBUN      , string strGUBUN2  , string strSABUN, string strTABLE
                                         , string strTABLE_ROWID, string strCONTEXT , string strGUBUN3
                                         , ref int intRowAffected)
        {
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            // 멀티 TAB에서 액팅시
            SQL += "     MERGE INTO " + ComNum.DB_PMPA + "NUR_HAPPYCALL_OPD S   \r\n";
            SQL += "     USING(                                                 \r\n";
            SQL += "               SELECT * FROM DUAL                           \r\n ";
            SQL += "           )                                                \r\n ";
            SQL += "        ON (                                                \r\n ";
            SQL += "                S.GUBUN     = " + ComFunc.covSqlstr(strGUBUN    , false);
            SQL += "           AND  S.PANO      = " + ComFunc.covSqlstr(strPANO     , false);
            SQL += "           AND  S.BDATE     = " + ComFunc.covSqlDate(strBDATE   , false);
            SQL += "           AND  S.DEPTCODE  = " + ComFunc.covSqlstr(strDEPTCODE , false);
            SQL += "          )                                                 \r\n ";
            SQL += "    WHEN MATCHED THEN                                       \r\n ";
            SQL += "        UPDATE                                              \r\n ";
            SQL += "           SET  S.GUBUN2     = " + ComFunc.covSqlstr(strGUBUN2  , false);
            SQL += "              , S.WRITEDATE  = SYSDATE                      \r\n ";
            SQL += "              , S.WRITESABUN = " + ComFunc.covSqlstr(strSABUN   , false);
            SQL += "    WHEN NOT MATCHED THEN                                   \r\n ";
            SQL += "            INSERT(                                         \r\n ";
            SQL += "                   GUBUN        -- 구분 (BAS_BCODE : GUBUN = NUR_간호해피콜_GUBUN)  \r\n";
            SQL += "                 , PANO         -- 등록번호                                         \r\n";
            SQL += "                 , TABLENAME    -- 테이블명                                         \r\n";
            SQL += "                 , TROWID       -- 테이블 RWOID                                     \r\n";
            SQL += "                 , WRITEDATE    -- 작성일자                                         \r\n";
            SQL += "                 , WRITESABUN   -- 작성사번                                         \r\n";            
            SQL += "                 , CONTEXT      -- 내용                                             \r\n";
            SQL += "                 , BDATE        -- 처방일자                                         \r\n";
            SQL += "                 , GUBUN2       -- 구분 (BAS_BCODE : GUBUN = NUR_간호해피콜_GUBUN2) \r\n";
            SQL += "                 , DEPTCODE     -- 진료과                                           \r\n";
            SQL += "                 , GUBUN3       -- 검사종류(EKG 실)                                 \r\n";
            SQL += "                  )                                             \r\n ";
            SQL += "            VALUES(                                             \r\n ";
            SQL += "                " + ComFunc.covSqlstr(strGUBUN       , false);
            SQL += "                " + ComFunc.covSqlstr(strPANO        , true);
            SQL += "                " + ComFunc.covSqlstr(strTABLE       , true);
            SQL += "                " + ComFunc.covSqlstr(strTABLE_ROWID , true);
            SQL += "                 , SYSDATE      -- 작성일자                                         \r\n";
            SQL += "                " + ComFunc.covSqlstr(strSABUN       , true);
            SQL += "                " + ComFunc.covSqlstr(strCONTEXT     , true);
            SQL += "                " + ComFunc.covSqlDate(strBDATE      , true);
            SQL += "                " + ComFunc.covSqlstr(strGUBUN2      , true);
            SQL += "                " + ComFunc.covSqlstr(strDEPTCODE    , true);
            SQL += "                " + ComFunc.covSqlstr(strGUBUN3      , true);
            SQL += "                  )                                             \r\n ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;
        }

        public string up_VACCINE_TPATIENT(PsmhDb pDbCon, string strPANO, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            DataTable dt = null;
            string strLSDATE = string.Empty;
            string[] arrHPHOME = null;
            string[] arrTEL = null;

            string[] arrHPHOME_Tmp = null;
            string[] arrTEL_Tmp = null;


            string strSNAME;
            string strJUMIN1;
            string strJUMIN2;
            string strJUMIN3;

            dt = comSql.sel_BAS_PATIENT(pDbCon, strPANO);

            if (ComFunc.isDataTableNull(dt) == true)
            {
                ComFunc.MsgBox("환자정보가 존재 하지 않습니다.");
                return "환자정보가 존재 하지 않습니다.";
            }
            else
            {
                strSNAME = dt.Rows[0][(int)clsComSQL.enmSel_BAS_PATIENT.SNAME].ToString();
                strJUMIN1 = dt.Rows[0][(int)clsComSQL.enmSel_BAS_PATIENT.JUMIN1].ToString();
                strJUMIN2 = dt.Rows[0][(int)clsComSQL.enmSel_BAS_PATIENT.JUMIN2].ToString();
                strJUMIN3 = clsAES.DeAES(dt.Rows[0][(int)clsComSQL.enmSel_BAS_PATIENT.JUMIN3].ToString());

                arrTEL_Tmp = dt.Rows[0][(int)clsComSQL.enmSel_BAS_PATIENT.TEL].ToString().Split('-');
                arrTEL = getPHONE(pDbCon, arrTEL_Tmp);

                arrHPHOME_Tmp = dt.Rows[0][(int)clsComSQL.enmSel_BAS_PATIENT.HPHONE].ToString().Split('-');
                arrHPHOME = getPHONE(pDbCon, arrHPHOME_Tmp);


                SQL = "";
                SQL += " UPDATE KOSMOS_PMPA.VACCINE_TPATIENT                   \r\n";
                SQL += "    SET PPERID      =" + ComFunc.covSqlstr(strJUMIN1 + strJUMIN2, false);
                SQL += "      , PPERID_OLD2 =" + ComFunc.covSqlstr(clsAES.AES(strJUMIN1 + strJUMIN3), false);
                SQL += "      , PTELNO1     =" + ComFunc.covSqlstr(arrTEL[0], false);
                SQL += "      , PTELNO2     =" + ComFunc.covSqlstr(arrTEL[1], false);
                SQL += "      , PTELNO3     =" + ComFunc.covSqlstr(arrTEL[2], false);
                SQL += "      , MPHONE1     =" + ComFunc.covSqlstr(arrHPHOME[0], false);
                SQL += "      , MPHONE2     =" + ComFunc.covSqlstr(arrHPHOME[1], false);
                SQL += "      , MPHONE3     =" + ComFunc.covSqlstr(arrHPHOME[2], false);
                SQL += "      , PNAME       =" + ComFunc.covSqlstr(strSNAME, false);

                SQL += " WHERE 1=1                                                          \r\n";
                SQL += "   AND PANO         =" + ComFunc.covSqlstr(strPANO, false);
                SQL += "   AND LAST_UPDATE  =(                                              \r\n";
                SQL += "                       SELECT MAX(LAST_UPDATE)                      \r\n";
                SQL += "                         FROM KOSMOS_PMPA.VACCINE_TPATIENT          \r\n";
                SQL += "                        WHERE PANO = " + ComFunc.covSqlstr(strPANO, false);
                SQL += "                       )                                            \r\n";


                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            }

            return SqlErr;
        }

        public string up_VACCINE_TLOTNO_S(PsmhDb pDbCon, string strENDATE, string strUSED, string strRowID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "VACCINE_TLOTNO_S   \r\n";
            SQL += "    SET ENDATE = " + ComFunc.covSqlstr(strENDATE, false);
            SQL += "      , USED   = " + ComFunc.covSqlstr(strUSED  , false);
            SQL += "  WHERE 1=1                                     \r\n";
            SQL += "    AND ROWID = " + ComFunc.covSqlstr(strRowID, false);


            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string save_ETC_JUSACODE(PsmhDb pDbCon, enmComParamSave enmSave, string strRowID
                                      , ref int intRowAffected
                                      , string strGubun = null, string strJepCode = null, string strJepName = null, string strVCNCOD = null)
        {
            string SqlErr = string.Empty;

            SQL = "";

            if (enmSave == enmComParamSave.Delete || enmSave == enmComParamSave.Update)
            {


                SQL += " UPDATE " + ComNum.DB_MED + "ETC_JUSACODE   \r\n";

                if (enmSave == enmComParamSave.Delete)
                {
                    SQL += "    SET DELDATE = TRUNC(SYSDATE)                   \r\n";
                }
                else if (enmSave == enmComParamSave.Update)
                {
                    SQL += "    SET GUBUN   = " + ComFunc.covSqlstr(strGubun, false);
                    SQL += "      , JEPCODE = " + ComFunc.covSqlstr(strJepCode, false);
                    SQL += "      , JEPNAME = " + ComFunc.covSqlstr(strJepName, false);
                    SQL += "      , VCNCOD  = " + ComFunc.covSqlstr(strVCNCOD, false);

                }

                SQL += "  WHERE 1=1                                     \r\n";
                SQL += "    AND ROWID = " + ComFunc.covSqlstr(strRowID, false);
            }
            else if (enmSave == enmComParamSave.Create)
            {
                SQL += "  INSERT INTO " + ComNum.DB_MED + "ETC_JUSACODE(     \r\n";
                SQL += "                GUBUN           \r\n";
                SQL += "              , JEPCODE         \r\n";
                SQL += "              , JEPNAME         \r\n";
                SQL += "              , VCNCOD          \r\n";
                SQL += "              ) VALUES(         \r\n";
                SQL += "             " + ComFunc.covSqlstr(strGubun, false);
                SQL += "             " + ComFunc.covSqlstr(strJepCode, true);
                SQL += "             " + ComFunc.covSqlstr(strJepName, true);
                SQL += "             " + ComFunc.covSqlstr(strVCNCOD, true);
                SQL += "             )"; 
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string save_ETC_JUSAMST(PsmhDb pDbCon, DataRow dr, bool isCancel, ref int intRowAffected, bool isMultiAllAction = false)
        {
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            // 멀티 TAB에서 액팅시
            SQL += "     MERGE INTO " + ComNum.DB_MED + "ETC_JUSAMST S                                \r\n";
            SQL += "     USING(                                                                       \r\n";
            SQL += "           SELECT                                                                 \r\n";
            SQL += "                   BDATE            AS BDATE      -- 발생일자                     \r\n";
            SQL += "                 , PTNO             AS PTNO       -- 환자등록번호                 \r\n";
            SQL += "                 , ORDERCODE        AS ORDERCODE  -- 오더코드                     \r\n";
            SQL += "                 , ORDERNO          AS ORDERNO    -- 오더번호                     \r\n";
            SQL += "                 , DEPTCODE         AS DEPTCODE   -- 진료과코드                   \r\n";
            SQL += "                 , DRCODE           AS DRCODE     -- 진료의사 코드                \r\n";
            SQL += "                 , TRIM(DOSCODE)    AS DOSCODE    -- 용법코드                     \r\n";
            //SQL += "                 , QTY              AS QTY        -- 수량                         \r\n";
            //2020-02-10 실사용량으로 변경
            SQL += "                 , REALQTY              AS QTY        -- 수량                         \r\n";
            SQL += "                 , CONTENTS             AS CONTENTS        -- 실용량                    \r\n";
            SQL += "                 , BCONTENTS             AS BCONTENTS        -- 기본용량                    \r\n";
            SQL += "                 , GBGROUP             AS GBGROUP        -- 기본용량                    \r\n";
            SQL += "                 , NAL              AS NAL        -- 날수                         \r\n";

            if (isMultiAllAction == true)
            {
                SQL += "                 , NAL              AS ACTNAL     -- 행위날수                     \r\n";
                SQL += "                 , 'Y'              AS GBEND      -- 행위종료(Y:종료, N:미종료)   \r\n";
            }
            else
            {
                SQL += "                 , 1                AS ACTNAL     -- 행위날수                     \r\n";
                SQL += "                 , 'N'              AS GBEND      -- 행위종료(Y:종료, N:미종료)   \r\n";
            }

            SQL += "                 , REMARK           AS REMARK     -- 비고(소견)                       \r\n";
            SQL += "                 , TRUNC(SYSDATE)   AS GBDATE     -- 마지막으로 주사 맞은 날짜        \r\n";
            SQL += "                 , SUCODE           AS SUCODE     -- 수가코드                         \r\n";
            SQL += "              FROM " + ComNum.DB_MED + "OCS_OORDER                                    \r\n ";                   
            SQL += "             WHERE 1=1                                                                \r\n";
            SQL += "               AND PTNO     = " + ComFunc.covSqlstr(dr[(int)enmSel_OCS_OORDER_InjecMain.PTNO].ToString(), false);
            SQL += "               AND BDATE    = " + ComFunc.covSqlDate(dr[(int)enmSel_OCS_OORDER_InjecMain.BDATE].ToString(), false);
            SQL += "               AND ORDERNO  = " + dr[(int)enmSel_OCS_OORDER_InjecMain.ORDERNO].ToString() + "     \r\n";            
            SQL += "            ) O                                             \r\n ";
            SQL += "        ON (                                                \r\n ";
            SQL += "                S.ORDERNO   = O.ORDERNO                     \r\n ";
            SQL += "           AND  S.BDATE     = O.BDATE                       \r\n "; // 중복을 막기 위해서 사용
            SQL += "           AND  S.PTNO      = O.PTNO                        \r\n "; // 중복을 막기 위해서 사용
            SQL += "          )                                                 \r\n ";
            SQL += "    WHEN MATCHED THEN                                       \r\n ";

            if (isCancel == true)
            {

                SQL += "        UPDATE SET S.ACTNAL = S.ACTNAL-1                    \r\n";
                SQL += "                 , S.GBEND  = CASE WHEN (S.ACTNAL-1 <=S.NAL) THEN 'N' ELSE 'Y' END                    \r\n";
                SQL += "                 , GBDATE   = (                                                                                                            \r\n";
                SQL += "                                SELECT MAX(ACTDATE)                                                                                         \r\n";
                SQL += "                                  FROM " + ComNum.DB_MED + "ETC_JUSASUB                                                                     \r\n";
                SQL += "                                 WHERE 1=1                                                                                                  \r\n";
                SQL += "                                   AND BDATE    = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.BDATE.ToString()].ToString(), false);
                SQL += "                                   AND PTNO     = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.PTNO.ToString()].ToString(), false);
                SQL += "                                   AND ORDERNO  = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.ORDERNO.ToString()].ToString(), false);
                SQL += "                              )                                                                                                           \r\n";


            }
            else
            {
                SQL += "        UPDATE SET S.ACTNAL = S.ACTNAL+1                    \r\n";
                SQL += "                 , S.GBEND  = CASE WHEN (S.ACTNAL+1 >=S.NAL) THEN 'Y' ELSE 'N' END                    \r\n";
                SQL += "                 , S.GBDATE = TRUNC(SYSDATE)  \r\n";
            }

            SQL += "    WHEN NOT MATCHED THEN                                       \r\n";
            SQL += "            INSERT(                                             \r\n";
            SQL += "                   S.BDATE      -- 발생일자                     \r\n";
            SQL += "                 , S.PTNO       -- 환자등록번호                 \r\n";
            SQL += "                 , S.ORDERCODE  -- 오더코드                     \r\n";
            SQL += "                 , S.ORDERNO    -- 오더번호                     \r\n";
            SQL += "                 , S.DEPTCODE   -- 진료과코드                   \r\n";
            SQL += "                 , S.DRCODE     -- 진료의사 코드                \r\n";
            SQL += "                 , S.DOSCODE    -- 용법코드                     \r\n";
            SQL += "                 , S.QTY        -- 수량                         \r\n";
            SQL += "                 , S.CONTENTS   -- 실용량                       \r\n";
            SQL += "                 , S.BCONTENTS  -- 기본용량                     \r\n";
            SQL += "                 , S.GBGROUP    -- 그룹                         \r\n";
            SQL += "                 , S.NAL        -- 날수                         \r\n";
            SQL += "                 , S.ACTNAL     -- 행위날수                     \r\n";
            SQL += "                 , S.GBEND      -- 행위종료(Y:종료, N:미종료)   \r\n";
            SQL += "                 , S.REMARK     -- 비고(소견)                   \r\n";
            SQL += "                 , S.GBDATE     -- 마지막으로 주사 맞은 날짜    \r\n";
            SQL += "                 , S.SUCODE     -- 수가코드                     \r\n";
            SQL += "                  )                                             \r\n ";
            SQL += "            VALUES(                                             \r\n ";
            SQL += "                   O.BDATE      -- 발생일자                     \r\n";
            SQL += "                 , O.PTNO       -- 환자등록번호                 \r\n";
            SQL += "                 , O.ORDERCODE  -- 오더코드                     \r\n";
            SQL += "                 , O.ORDERNO    -- 오더번호                     \r\n";
            SQL += "                 , O.DEPTCODE   -- 진료과코드                   \r\n";
            SQL += "                 , O.DRCODE     -- 진료의사 코드                \r\n";
            SQL += "                 , O.DOSCODE    -- 용법코드                     \r\n";
            SQL += "                 , O.QTY        -- 수량                         \r\n";
            SQL += "                 , O.CONTENTS   -- 실용량                       \r\n";
            SQL += "                 , O.BCONTENTS  -- 기본용량                     \r\n";
            SQL += "                 , O.GBGROUP    -- 그룹                         \r\n";
            SQL += "                 , O.NAL        -- 날수                         \r\n";
            SQL += "                 , O.ACTNAL     -- 행위날수                     \r\n";
            SQL += "                 , O.GBEND      -- 행위종료(Y:종료, N:미종료)   \r\n";
            SQL += "                 , O.REMARK     -- 비고(소견)                   \r\n";
            SQL += "                 , O.GBDATE     -- 마지막으로 주사 맞은 날짜    \r\n";
            SQL += "                 , O.SUCODE     -- 수가코드                     \r\n";
            SQL += "                  )                                             \r\n ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;
        }
        
        /// <summary>주사메인</summary>
        /// <param name="dr">넘길값들</param>
        /// <param name="tabType">주사 액팅을 위한 세부 항목</param>
        /// <param name="part">주사실 접수 파트</param>
        /// <returns></returns>
        public string save_ETC_JUSASUB(PsmhDb pDbCon, DataRow dr
                                    , string strSABUN
                                    , enmOCS_OORDER_InjecMain_Type tabType
                                    , enmOCS_OORDER_InjecMain_Part part
                                    , ref int intRowAffected, bool isMultiAllAction = false)
        {
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";

            string sPart = string.Empty;

            if (part == enmOCS_OORDER_InjecMain_Part.IJRM)
            {
                sPart = "1";
            }
            else if (part == enmOCS_OORDER_InjecMain_Part.XRAY)
            {
                sPart = "3";
            }
            else if (part == enmOCS_OORDER_InjecMain_Part.CANCER)
            {
                sPart = "5";
            }
            if (tabType == enmOCS_OORDER_InjecMain_Type.MULTI)
            {
                // 멀티 TAB에서 액팅시
                SQL += "     MERGE INTO " + ComNum.DB_MED + "ETC_JUSASUB S                    \r\n ";
                SQL += "     USING(                                                 \r\n ";
                SQL += "           SELECT  BDATE               AS BDATE             \r\n ";
                SQL += "                 , TRUNC(SYSDATE)      AS ACTDATE           \r\n ";
                SQL += "                 , SYSDATE             AS ACTDATE2          \r\n ";
                SQL += "                 , PTNO                AS PTNO              \r\n ";
                SQL += "                 , ORDERCODE           AS ORDERCODE         \r\n ";
                SQL += "                 , ORDERNO             AS ORDERNO           \r\n ";
                SQL += "                 , DEPTCODE            AS DEPTCODE          \r\n ";
                SQL += "                 , DRCODE              AS DRCODE            \r\n ";
                SQL += "                 , DOSCODE             AS DOSCODE           \r\n ";
                SQL += "                 , QTY                 AS QTY               \r\n ";
                SQL += "                 , CONTENTS            AS CONTENTS          \r\n ";
                SQL += "                 , BCONTENTS           AS BCONTENTS         \r\n ";
                SQL += "                 , GBGROUP             AS GBGROUP           \r\n ";
                SQL += "                 , QTY                 AS QTY               \r\n ";
                SQL += "                 , 1                   AS NAL               \r\n ";
                SQL += "                 , 'Y'                 AS GBACT             \r\n ";
                SQL += "                 , REMARK              AS REMARK            \r\n ";
                SQL += "                 , '2'                 AS GBMASTER          \r\n "; // 멀티처방이 기존에 있는 것에 대해서 액팅할 경우
                SQL += "                 , SUCODE              AS SUCODE            \r\n ";
                SQL += "                 , '" + sPart + "'     AS PART              \r\n ";
                SQL += "                 , '" + strSABUN + "'  AS SABUN             \r\n ";
                SQL += "              FROM " + ComNum.DB_MED + "ETC_JUSAMST                   \r\n ";
                SQL += "             WHERE ROWID    = " + ComFunc.covSqlstr(dr[(int)enmSel_OCS_OORDER_InjecMain.ROW_ID].ToString(),false);
                SQL += "            ) O                                             \r\n ";
                SQL += "        ON  (                                               \r\n ";
                SQL += "                S.ORDERNO   = O.ORDERNO                     \r\n ";
                SQL += "           AND  S.ACTDATE   = O.ACTDATE                     \r\n "; // 오늘일자에 접수된 것이 있는지...
                SQL += "           AND  1           = 2                             \r\n "; // 2017.08.28.김홍록: 무조건 insert
                SQL += "          )                                                 \r\n ";
                SQL += "    WHEN MATCHED THEN                                       \r\n ";
                SQL += "        UPDATE SET REMARK = REMARK                          \r\n ";
                SQL += "    WHEN NOT MATCHED THEN                                   \r\n ";
                SQL += "            INSERT(                                         \r\n ";
                SQL += "                    BDATE                                   \r\n ";
                SQL += "                 , ACTDATE                                  \r\n ";
                SQL += "                 , ACTDATE2                                 \r\n ";
                SQL += "                 , PTNO                                     \r\n ";
                SQL += "                 , ORDERCODE                                \r\n ";
                SQL += "                 , ORDERNO                                  \r\n ";
                SQL += "                 , DEPTCODE                                 \r\n ";
                SQL += "                 , DRCODE                                   \r\n ";
                SQL += "                 , DOSCODE                                  \r\n ";
                SQL += "                 , QTY                                      \r\n ";
                SQL += "                 , CONTENTS                                 \r\n ";
                SQL += "                 , BCONTENTS                                \r\n ";
                SQL += "                 , GBGROUP                                  \r\n ";
                SQL += "                 , NAL                                      \r\n ";
                SQL += "                 , GBACT                                    \r\n ";
                SQL += "                 , REMARK                                   \r\n ";
                SQL += "                 , GBMASTER                                 \r\n ";
                SQL += "                 , SUCODE                                   \r\n ";
                SQL += "                 , PART                                     \r\n ";
                SQL += "                 , SABUN)                                   \r\n ";
                SQL += "            VALUES(                                         \r\n ";
                SQL += "                   O.BDATE                                  \r\n ";
                SQL += "                 , O.ACTDATE                                \r\n ";
                SQL += "                 , O.ACTDATE2                               \r\n ";
                SQL += "                 , O.PTNO                                   \r\n ";
                SQL += "                 , O.ORDERCODE                              \r\n ";
                SQL += "                 , O.ORDERNO                                \r\n ";
                SQL += "                 , O.DEPTCODE                               \r\n ";
                SQL += "                 , O.DRCODE                                 \r\n ";
                SQL += "                 , O.DOSCODE                                \r\n ";
                SQL += "                 , O.QTY                                    \r\n ";
                SQL += "                 , O.CONTENTS                               \r\n ";
                SQL += "                 , O.BCONTENTS                              \r\n ";
                SQL += "                 , O.GBGROUP                                \r\n ";
                SQL += "                 , O.NAL                                    \r\n ";
                SQL += "                 , O.GBACT                                  \r\n ";
                SQL += "                 , O.REMARK                                \r\n ";
                SQL += "                 , O.GBMASTER                               \r\n ";
                SQL += "                 , O.SUCODE                                 \r\n ";
                SQL += "                 , O.PART                                   \r\n ";
                SQL += "                 , O.SABUN)                                 \r\n ";
            }
            else
            {
                // 대기자 TAB
                SQL += "     MERGE INTO " + ComNum.DB_MED + "ETC_JUSASUB S                    \r\n ";
                SQL += "     USING(                                                 \r\n ";
                SQL += "           SELECT  BDATE               AS BDATE             \r\n ";
                SQL += "                 , TRUNC(SYSDATE)      AS ACTDATE           \r\n ";
                SQL += "                 , SYSDATE             AS ACTDATE2          \r\n ";
                SQL += "                 , PTNO                AS PTNO              \r\n ";
                SQL += "                 , ORDERCODE           AS ORDERCODE         \r\n ";
                SQL += "                 , ORDERNO             AS ORDERNO           \r\n ";
                SQL += "                 , DEPTCODE            AS DEPTCODE          \r\n ";
                SQL += "                 , DRCODE              AS DRCODE            \r\n ";
                SQL += "                 , RTRIM(DOSCODE)      AS DOSCODE           \r\n ";
                //SQL += "                 , QTY                 AS QTY               \r\n ";
                SQL += "                 , REALQTY                 AS QTY               \r\n ";     //2020-02-10 실사용량 표시로 변경
                SQL += "                 , CONTENTS            AS CONTENTS          \r\n ";
                SQL += "                 , BCONTENTS           AS BCONTENTS         \r\n ";
                SQL += "                 , GBGROUP             AS GBGROUP           \r\n ";
                SQL += "                 , 'Y'                 AS GBACT             \r\n ";
                SQL += "                 , SUCODE              AS SUCODE            \r\n ";

                if (isMultiAllAction == true && dr[(int)enmSel_OCS_OORDER_InjecMain.GBNAL].ToString().Equals("D"))
                {
                    SQL += "                 , NAL                   AS NAL               \r\n ";
                }
                else
                {
                    SQL += "                 , 1                   AS NAL                 \r\n ";
                }
                
                if (dr[(int)enmSel_OCS_OORDER_InjecMain.GBNAL].ToString().Equals("D"))
                {
                    // strGb = "D"
                    SQL += "                 , REMARK              AS REMARK        \r\n ";
                    SQL += "                 , '1'                 AS GBMASTER      \r\n ";
                }
                else if (dr[(int)enmSel_OCS_OORDER_InjecMain.GBNAL].ToString().Equals("S") && string.IsNullOrEmpty(dr[(int)enmSel_OCS_OORDER_InjecMain.VCNCODE].ToString()) == false)
                {
                    // strGb = "S" 이면서 예방접종
                    SQL += "                 , " + dr[enmSel_OCS_OORDER_InjecMain.CHASU.ToString()].ToString() + "              AS REMARK            \r\n ";
                    SQL += "                 , ''                  AS GBMASTER      \r\n ";
                }
                else if (dr[(int)enmSel_OCS_OORDER_InjecMain.GBNAL].ToString().Equals("S") && string.IsNullOrEmpty(dr[(int)enmSel_OCS_OORDER_InjecMain.VCNCODE].ToString()) == true)
                {
                    // strGb = "S" 이면서 일반주사
                    SQL += "                 , REMARK              AS REMARK        \r\n ";
                    SQL += "                 , ''                  AS GBMASTER      \r\n ";
                }

                SQL += "                 , '" + sPart + "'     AS PART              \r\n ";
                SQL += "                 , SABUN               AS SABUN             \r\n ";
                SQL += "              FROM " + ComNum.DB_MED + "OCS_OORDER                    \r\n ";
                SQL += "             WHERE ROWID = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.ROW_ID.ToString()].ToString(), false);
                SQL += "            ) O                                             \r\n ";
                SQL += "        ON(                                                 \r\n ";
                SQL += "                S.ORDERNO = O.ORDERNO                       \r\n ";
                SQL += "           AND  S.ACTDATE = O.ACTDATE                       \r\n "; // 당일 2중 접수를 막기 위한 루틴
                SQL += "          )                                                 \r\n ";
                SQL += "    WHEN MATCHED THEN                                       \r\n ";
                SQL += "        UPDATE SET REMARK = REMARK                          \r\n ";
                SQL += "    WHEN NOT MATCHED THEN                                   \r\n ";
                SQL += "            INSERT(                                         \r\n ";
                SQL += "                    BDATE                                   \r\n ";
                SQL += "                 , ACTDATE                                  \r\n ";
                SQL += "                 , ACTDATE2                                 \r\n ";
                SQL += "                 , PTNO                                     \r\n ";
                SQL += "                 , ORDERCODE                                \r\n ";
                SQL += "                 , ORDERNO                                  \r\n ";
                SQL += "                 , DEPTCODE                                 \r\n ";
                SQL += "                 , DRCODE                                   \r\n ";
                SQL += "                 , DOSCODE                                  \r\n ";
                SQL += "                 , QTY                                      \r\n ";
                SQL += "                 , CONTENTS                                 \r\n ";
                SQL += "                 , BCONTENTS                                \r\n ";
                SQL += "                 , GBGROUP                                  \r\n ";
                SQL += "                 , NAL                                      \r\n ";
                SQL += "                 , GBACT                                    \r\n ";
                SQL += "                 , REMARK                                   \r\n ";
                SQL += "                 , GBMASTER                                 \r\n ";
                SQL += "                 , SUCODE                                   \r\n ";
                SQL += "                 , PART                                     \r\n ";
                SQL += "                 , SABUN                                    \r\n ";
                SQL += "                 )                                          \r\n ";
                SQL += "            VALUES(                                         \r\n ";
                SQL += "                    O.BDATE                                 \r\n ";
                SQL += "                 , O.ACTDATE                                \r\n ";
                SQL += "                 , O.ACTDATE2                               \r\n ";
                SQL += "                 , O.PTNO                                   \r\n ";
                SQL += "                 , O.ORDERCODE                              \r\n ";
                SQL += "                 , O.ORDERNO                                \r\n ";
                SQL += "                 , O.DEPTCODE                               \r\n ";
                SQL += "                 , O.DRCODE                                 \r\n ";
                SQL += "                 , O.DOSCODE                                \r\n ";
                SQL += "                 , O.QTY                                    \r\n ";
                SQL += "                 , O.CONTENTS                               \r\n ";
                SQL += "                 , O.BCONTENTS                              \r\n ";
                SQL += "                 , O.GBGROUP                                \r\n ";
                SQL += "                 , O.NAL                                    \r\n ";
                SQL += "                 , O.GBACT                                  \r\n ";
                SQL += "                 , O.REMARK                                 \r\n ";
                SQL += "                 , O.GBMASTER                               \r\n ";
                SQL += "                 , O.SUCODE                                 \r\n ";
                SQL += "                 , O.PART                                   \r\n ";
                SQL += "                 , O.SABUN)                                 \r\n ";

            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;
        }

        /// <summary>ETC_JUSAMST</summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string up_ETC_JUSAMST(PsmhDb pDbCon, DataRow dr, bool isCancel, ref int intRowAffected)
        {
            string SqlErr = ""; //에러문 받는 변수
          
            SQL = "   UPDATE " + ComNum.DB_MED + "ETC_JUSAMST   \r\n";
            SQL += "     SET  GBEND   = " + ComFunc.covSqlstr("Y", false);
            SQL += "        , GBDATE  = TRUNC(SYSDATE)          \r\n";
            SQL += "    WHERE 1=1                                                                                                                       \r\n";
            SQL += "      AND ROWID   = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.ROW_ID.ToString()].ToString(), false);
         
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string del_ETC_JUSAMST(PsmhDb pDbCon, DataRow dr, ref int intRowAffected)
        {
            string SqlErr = ""; //에러문 받는 변수
            string sEnd = string.Empty;

            SQL = "";
            SQL += "   DELETE " + ComNum.DB_MED + "ETC_JUSAMST   \r\n";
            SQL += "    WHERE 1=1                                \r\n";
            SQL += "      AND BDate    = " + ComFunc.covSqlDate(dr[enmSel_OCS_OORDER_InjecMain.BDATE.ToString()].ToString(), false);
            SQL += "      AND PTNO     = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.PTNO.ToString()].ToString(), false);
            SQL += "      AND ORDERNO  = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.ORDERNO.ToString()].ToString(), false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;
        }

        public string del_ETC_JUSASUB(PsmhDb pDbCon, DataRow dr, ref int intRowAffected)
        {
            string SqlErr = ""; //에러문 받는 변수
            string sEnd = string.Empty;

            SQL = "";
            SQL += "   DELETE " + ComNum.DB_MED + "ETC_JUSASUB   \r\n";
            SQL += "    WHERE 1=1                                \r\n";
            SQL += "      AND PTNO                        = " + ComFunc.covSqlstr( dr[enmSel_OCS_OORDER_InjecMain.PTNO.ToString()].ToString(), false);
            SQL += "      AND ORDERNO                     = " + ComFunc.covSqlstr( dr[enmSel_OCS_OORDER_InjecMain.ORDERNO.ToString()].ToString(), false);
            SQL += "      AND ACTDATE                     = " + ComFunc.covSqlDate(dr[enmSel_OCS_OORDER_InjecMain.ACTDATE.ToString()].ToString(), false);
            SQL += "      AND TO_CHAR(ACTDATE2,'HH24:MI') = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.ACTTIME.ToString()].ToString(), false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;
        }

        public string up_BAS_PATIENT_JUSAMSG(PsmhDb pDbCon, string strPano, string strMemo, ref int intRowAffected)
        {
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "   UPDATE KOSMOS_PMPA.BAS_PATIENT                           \r\n";
            SQL += "     SET  JUSAMSG = " + ComFunc.covSqlstr(strMemo, false);
            SQL += "    WHERE PANO    = " + ComFunc.covSqlstr(strPano, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;

        }

        public string up_OCS_OORDER (PsmhDb pDbCon, DataRow dr,bool isCancel,  ref int intRowAffected, bool isJ = false)
        {
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "   UPDATE " + ComNum.DB_MED + "OCS_OORDER \r\n";

            if (isJ == false)
            {
                if (isCancel == false)
                {
                    SQL += "      SET GBBOTH  = '3'                      \r\n";
                    SQL += "    WHERE 1=1                                \r\n";
                    SQL += "      AND BDATE   = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.BDATE.ToString()].ToString(), false);
                    SQL += "      AND PTNO    = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.PTNO.ToString()].ToString(), false);
                    SQL += "      AND ORDERNO = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.ORDERNO.ToString()].ToString(), false);
                    SQL += "      AND GBBOTH != '3'";
                }
                else
                {
                    SQL += "      SET GBBOTH  = '0'                     \r\n";
                    SQL += "    WHERE 1=1                               \r\n";
                    SQL += "      AND PTNO    = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.PTNO.ToString()].ToString(), false);
                    SQL += "      AND BDATE   = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.BDATE.ToString()].ToString(), false);
                    SQL += "      AND ORDERNO = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.ORDERNO.ToString()].ToString(), false);
                    SQL += "      AND GBBOTH  = '3'                     \r\n";
                    SQL += "      AND NOT EXISTS                        \r\n";
                    SQL += "          ( SELECT 1                        \r\n";
                    SQL += "             FROM KOSMOS_OCS.ETC_JUSAMST A  \r\n";
                    SQL += "             WHERE 1=1                      \r\n";
                    SQL += "               AND A.PTNO    = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.PTNO.ToString()].ToString(), false);
                    SQL += "               AND A.BDATE   = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.BDATE.ToString()].ToString(), false);
                    SQL += "               AND A.ORDERNO = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.ORDERNO.ToString()].ToString(), false);
                    SQL += "        )                                   \r\n";
                }
            }
            else
            {
                SQL += "      SET GBBOTH   = 'J'                       \r\n";
                SQL += "    WHERE 1=1                                  \r\n";
                SQL += "      AND BDATE   = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.BDATE.ToString()].ToString(), false);
                SQL += "      AND PTNO    = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.PTNO.ToString()].ToString(), false);
                SQL += "      AND ORDERNO = " + ComFunc.covSqlstr(dr[enmSel_OCS_OORDER_InjecMain.ORDERNO.ToString()].ToString(), false);
                SQL += "      AND GBBOTH  != '3'                       \r\n";
                SQL += "      AND GBSUNAP != '1'";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;

        }

    }
}
