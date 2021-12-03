using ComBase; //기본 클래스
using ComDbB; //DB연결
using System;
using System.Data;
//추가네임스페이스
using System.Drawing;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace ComSupLibB.SupLbEx
{

    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : clsLbExBarCodePrint.cs
    /// Description     : SpecNoBar
    /// Author          : 김홍록
    /// Create Date     : 2017-06-22
    /// Update History  : 
    /// </summary>
    /// <history>       
    ///                 
    /// </history>
    /// <seealso cref= "d:\psmh\exam\exmain\EXMAIN19.frm" />
    public class clsLbExBarCodePrint: Com.clsMethod
    {

        /// <summary>바코드 종류별 분류</summary>
        public enum enmPrintType { USB, COM_PORT, SERIAL };

        public enum enmPrintPart { OC, EXAM};

        int nBCodeName;

        ComPrintApi comPrintApi = new ComPrintApi();
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        clsComSupLbExQCSQL lbExQcSQL = new clsComSupLbExQCSQL();
        clsPrint ClsPrint = new clsPrint();
        //clsComSupLbEx clsCSL = new clsComSupLbEx();

        string PrintName = string.Empty;
        string slideNo = string.Empty;

        BarCodeParam gBarCodeParam = new BarCodeParam();        
        Com.clsParam comParam = new Com.clsParam();

        clsComSupLbExRcpSQL rcpSQL = new clsComSupLbExRcpSQL();

        DataTable gDT_BARCODE = null;

        string gHEPARIN = "";
        string gSODIUM = "";
        string gPRINT_ADD = "";

        //                                1         2       3        4      5         6           7       8     9         10      11        12        13       14      15           16         17          18     19       20    21      22 
        //public enum enmDT_BARCODE { BAR_CNT, SPECCODE, WS_GRP,TUBECODE,WSCODE, STRWSPOS, MASTERCODE, SPECNO, STRT, DRCOMMENT, WS_YAK, RESULTIN, UNITCODE, EQUCODE, SUCODE, WSGRP_TITLE, BLOODTIME, GB_GWAEXAM, PIECE, ORDERNO,  PRT, GBTLA };

        //void setDT_BARCODE()
        //{
        //    this.gDT_BARCODE = new DataTable();

        //    foreach (enmDT_BARCODE item in Enum.GetValues(typeof(enmDT_BARCODE)))
        //    {
        //        string strCOLUMN = item.ToString();
        //        this.gDT_BARCODE.Columns.Add(new DataColumn(strCOLUMN, typeof(string)));
        //    }
        //}

        //  SERIAL_NO,   SPECCODE,   WSGRPNM,   TUBECODE,   WSCODE1,   WSCODE1POS,   SUBCODE,    SPECNO,      STRT,   DRCOMMENT,      WSNM,   RESULTIN,   UNITCODE,   EQUCODE1,   ROW_ID,     WSGRP,    BLOODTIME,   CHKGWA,    PIECE,    ORDERNO,      PRT,     GBTLA
        public enum enmSel_EXAM_MASTER_SUB      {   SERIAL_NO,   SPECCODE,   WSGRPNM,   TUBECODE,   WSCODE1,   WSCODE1POS,   SUBCODE,    SPECNO,      STRT,   DRCOMMENT,      WSNM,   RESULTIN,   UNITCODE,   EQUCODE1,    ROW_ID,     WSGRP,   BLOODTIME,   CHKGWA,      PIECE,   ORDERNO,       PRT,    GBTLA, MASTERCODE };
        public string[] sSel_EXAM_MASTER_SUB =  { "SERIAL_NO", "SPECCODE", "WSGRPNM", "TUBECODE", "WSCODE1", "WSCODE1POS", "SUBCODE",  "SPECNO",    "STRT", "DRCOMMENT",    "WSNM", "RESULTIN", "UNITCODE", "EQUCODE1",  "ROW_ID",   "WSGRP", "BLOODTIME",  "CHKGWA",   "PIECE", "ORDERNO",     "PRT",   "GBTLA", "MASTERCODE" };
        public int[] nSel_EXAM_MASTER_SUB =     {   nCol_WARD,  nCol_WARD, nCol_WARD,  nCol_WARD, nCol_WARD,    nCol_WARD, nCol_WARD, nCol_WARD, nCol_WARD,   nCol_WARD, nCol_WARD,  nCol_WARD,  nCol_WARD,  nCol_WARD, nCol_WARD, nCol_WARD,   nCol_WARD, nCol_WARD, nCol_WARD, nCol_WARD, nCol_WARD, nCol_WARD, nCol_WARD };
        
        public enum enmSel_EXAM_RESULTC_BloodBand { LINE_01, LINE_02_1, LINE_02_2, LINE_03, LINE_04 };

        /// <summary>혈액팔찌용 출력</summary>
        /// <param name="strSpecNo">검체번호</param>
        /// <returns></returns>
        public DataTable sel_EXAM_RESULTC_BloodBand(PsmhDb pDbCon, string strSpecNo)
        {
            DataTable dt = null;
            string SQL = "";

            SQL = "";
            SQL += "SELECT PANO  || ' ' || SEX || '/' || AGE || ' ' || DEPTCODE AS LINE_01          \r\n";
            SQL += "     , BLOOD_TYPE                                           AS LINE_02_1        \r\n";
            SQL += "     , SNAME || ' 수혈'                                     AS LINE_02_2        \r\n";
            SQL += "     , 'Date :' || TO_CHAR(SYSDATE, ' YYYY-MM-DD HH24:MI')  AS LINE_03          \r\n";
            SQL += "     , 'Pohang ST.Mary`s Hospital'                          AS LINE_04          \r\n";
            SQL += "  FROM                                                                          \r\n";
            SQL += "  (                                                                             \r\n";
            SQL += "    SELECT IPDOPD                                       AS IPDOPD               \r\n";
            SQL += "        , PANO                                          AS PANO                 \r\n";
            SQL += "        , KOSMOS_OCS.FC_SET_EXAM_BABY(SNAME)            AS SNAME                \r\n";
            SQL += "        , KOSMOS_OCS.FC_EXAM_BLOOD_MASTER_ABO(PANO)     AS BLOOD_TYPE           \r\n";
            SQL += "        , DECODE(PANO, '81000013', '', AGE)             AS AGE                  \r\n";
            SQL += "        , DECODE(PANO, '81000013', '', SEX)             AS SEX                  \r\n";
            SQL += "        , KOSMOS_OCS.FC_SET_EXAM_DEPT(PANO,IPDOPD,DEPTCODE,WARD,ROOM)  AS DEPTCODE  \r\n";
            SQL += "		, DRCODE                                                                \r\n";
            SQL += "		, WORKSTS                                                               \r\n";
            SQL += "		, SPECCODE                                                              \r\n";
            SQL += "		, TUBE                                                                  \r\n";
            SQL += "		, STRT                                                                  \r\n";
            SQL += "		, TO_CHAR(BLOODDATE, 'HH24:MI') BDATE                                   \r\n";
            SQL += "      FROM " + ComNum.DB_MED + "EXAM_SPECMST                                    \r\n";
            SQL += "     WHERE 1 = 1                                                                \r\n";
            SQL += "        AND SPECNO = " + ComFunc.covSqlstr(strSpecNo, false);
            SQL += "  )                                                                \r\n";

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

        /// <summary>2017.05.23.김홍록:Exam 처방의 세부 구분자</summary>
        public enum enmSelExamOrderDetailOpt { InPatient, Consulting, Receipt, Order, MENUAL };

        /// <summary>2018.02.06버젼</summary>
        //public enum enmSel_EXAM_ORDER_Detail { PANO, BI, SNAME, IPDOPD, AGE, AGEMM, SEX, DEPTCODE, WARD, DRCODE, DR_NM, ROOM, SPECNO, BDATE, QTY, CHKGWA, STRT, DR_SPECCODE, SPECCODE, MASTERCODE, DRCOMMENT, ROW_ID, RDATE, SDATE, ORDERDATE, SENDDATE, ORDERNO, CHKIO, EXAMNAME, SPECNM, TUBECODE, TUBENAME, UNITCODE, WSGRP, WSCODE1, WSCODE1POS, WSGRPNM, WSNM, RESULTIN, MOTHER, EQUCODE1, PIECE, GBTLA, SERIES, HICNO, JOB, SUB_CODE };
        public enum enmSel_EXAM_ORDER_Detail{ PIECE, SPILITE, CHKGWA,   BL_PLT, SPECODE, GBTLA, TUBECODE, O_MASTERCODE, MASTERCODE, SUBCODE, PANO, BI, SNAME, IPDOPD, AGE, AGEMM, SEX, DEPTCODE, WARD, DRCODE, DR_NM, ROOM, SPECNO, BDATE, QTY, STRT, DRCOMMENT, ROW_ID, RDATE, SDATE, ORDERDATE, SENDDATE, ORDERNO, CHKIO, EXAMNAME, TUBENAME, UNITCODE, UNITNM, WSCODE1, WSCODE1POS, WSGRP, WSGRPNM, WSNM, RESULTIN, EQUCODE1, SERIES, DR_SPECCODE, SPEC_SPECCODE, JOB, SPEC_DR_COMMENTS,SPEC_WS_NM, SPEC_RECEIVEDATE, SPEC_STATUS, SPEC_BLOODDATE, RSLT_SEQNO };

        public string ins_EXAM_SPECMST(PsmhDb pDbCon, DataRow dr, string strDEPT, string nOrderNo, string strSpecNo, string strSTRT, string strSpecCode, string strTubeCode, string strBloodTime, ref string strWorkSTS, ref string strDrComment, string strGbGwaExam, string strGbGwaExam2, ref int intRowAffected, string strMastercode = "")
        {
            if (string.IsNullOrEmpty(strSpecNo.Trim()) == true)
            {
                return "";
            }
           
            string SQL = "";
            string SqlErr = string.Empty;
            string strSNAME = dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.SNAME].ToString();
            string strChkBLD = "";

            //2018-06-28 안정수, 이도경c 요청으로 검체가 'BLD'인경우 체크하는 로직 추가
            if (EXAM_CHECK_BLD(pDbCon, strSpecCode) == true)
            {
                strChkBLD = "Y";
            }

            if (string.IsNullOrEmpty(strWorkSTS.Trim()) == false)
            {
                strWorkSTS = strWorkSTS.Substring(0, strWorkSTS.Trim().Length - 1);
            }

            if (strDrComment.Trim().Length > 500)
            {
                strDrComment = strDrComment.Substring(0, 499);
            }

            if (strSNAME.Trim().Length > 10)
            {
                strSNAME = strSNAME.Substring(0, 10);
            }

            SQL = "";
            SQL += "";
            SQL += " INSERT INTO KOSMOS_OCS.EXAM_SPECMST( \r\n";
            SQL += "       SPECNO                         \r\n";
            SQL += "     , PANO                           \r\n";
            SQL += "     , BI                             \r\n";
            SQL += "     , SNAME                          \r\n";
            SQL += "     , IPDOPD                         \r\n";
            SQL += "     , AGE                            \r\n";
            SQL += "     , AGEMM                          \r\n";
            SQL += "     , SEX                            \r\n";
            SQL += "     , DEPTCODE                       \r\n";
            SQL += "     , WARD                           \r\n";
            SQL += "     , ROOM                           \r\n";
            SQL += "     , DRCODE                         \r\n";
            SQL += "     , DRCOMMENT                      \r\n";
            SQL += "     , STRT                           \r\n";
            SQL += "     , SPECCODE                       \r\n";
            SQL += "     , TUBE                           \r\n";
            SQL += "     , WORKSTS                        \r\n";
            SQL += "     , BDATE                          \r\n";
            SQL += "     , BLOODDATE                      \r\n";
            SQL += "     , RECEIVEDATE                    \r\n";
            SQL += "     , STATUS                         \r\n";
            SQL += "     , EMR                            \r\n";
            SQL += "     , ORDERDATE                      \r\n";
            SQL += "     , SENDDATE                       \r\n";
            SQL += "     , GB_GWAEXAM                     \r\n";
            SQL += "     , GB_GWAEXAM2                    \r\n";    //2018-06-15 안정수, GB-GWAEXAM2 추가
            SQL += "     , HICNO                          \r\n";
            SQL += "     , ORDERNO                        \r\n";
            SQL += "     , INPS                           \r\n";
            SQL += "     , INPT_DT                        \r\n";
            SQL += "     , UPPS                           \r\n";
            SQL += "     , UPDT                           \r\n";
            SQL += " )                                    \r\n";
            SQL += " VALUES(                              \r\n";

            SQL += ComFunc.covSqlstr(strSpecNo, false);
            SQL += ComFunc.covSqlstr(dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.PANO].ToString(), true);
            SQL += ComFunc.covSqlstr(dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.BI].ToString(), true);
            SQL += ComFunc.covSqlstr(strSNAME, true);

            if (strDEPT.Trim().Equals("LIS") == true)
            {
                SQL += ComFunc.covSqlstr("O", true);
            }
            else
            {
                SQL += ComFunc.covSqlstr(dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.IPDOPD].ToString(), true);
            }
            
            SQL += ComFunc.covSqlstr(dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.AGE].ToString(), true);
            SQL += ComFunc.covSqlstr(dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.AGEMM].ToString(), true);
            SQL += ComFunc.covSqlstr(dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.SEX].ToString(), true);
            SQL += ComFunc.covSqlstr(dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.DEPTCODE].ToString(), true);
            SQL += ComFunc.covSqlstr(dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.WARD].ToString(), true);
            SQL += ComFunc.covSqlstr(dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.ROOM].ToString(), true);
            SQL += ComFunc.covSqlstr(dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.DRCODE].ToString(), true);

            if (strDrComment.Length > 400)
            {
                strDrComment = strDrComment.Substring(0, 399);
            }

            SQL += ComFunc.covSqlstr(strDrComment, true);
            SQL += ComFunc.covSqlstr(strSTRT, true);
            SQL += ComFunc.covSqlstr(strSpecCode, true);
            SQL += ComFunc.covSqlstr(strTubeCode, true);
            SQL += ComFunc.covSqlstr(strWorkSTS, true);
            SQL += ComFunc.covSqlDate(dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.BDATE].ToString(), true);
            SQL += ComFunc.covSqlDate(strBloodTime, "YYYY-MM-DD HH24:MI", true);

            if (strGbGwaExam.Trim().Equals("Y") == true)
            {
                //2018-12-13 안정수, 김은경s 요청으로 ABGA에 대해서 자동접수 안되도록 조건 추가
                if (strSpecCode == "011" && strTubeCode == "003" && strWorkSTS == "C" && strSTRT == "R")
                {
                    //SQL += " , '' \r\n";
                    //SQL += ComFunc.covSqlstr("00", true);
                    SQL += ComFunc.covSqlDate(strBloodTime, "YYYY-MM-DD HH24:MI", true);
                    SQL += ComFunc.covSqlstr("01", true);
                }
                else
                {
                    if( (dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.MASTERCODE].ToString() == "CR59" || dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.MASTERCODE].ToString() == "CR59B" || dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.MASTERCODE].ToString() == "CR60I" || strMastercode == "CR59" || strMastercode == "CR59B" || strMastercode == "CR60I") && dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.IPDOPD].ToString() == "O")
                    {
                        SQL += ComFunc.covSqlDate(strBloodTime, "YYYY-MM-DD HH24:MI", true);
                        SQL += ComFunc.covSqlstr("01", true);
                    }
                    else
                    {
                        SQL += " , '' \r\n";
                        SQL += ComFunc.covSqlstr("00", true);
                    }
                }
            }

            else
            {
                if (strDEPT.Trim().Equals("LIS") == false) 
                {
                    // 입원, 신검, 종검, 산부인과
                    SQL += " , '' \r\n";
                    SQL += ComFunc.covSqlstr("00", true);
                }


                else if (strDEPT.Trim().Equals("LIS") == true && strWorkSTS.Trim().Equals("C") == true && (strSpecCode.Trim().Equals("025") == true || strSpecCode.Trim().Equals("025A") == true || strSpecCode.Trim().Equals("026") == true))
                {
                    // 생화학 24시간 Urine
                    SQL += " , '' \r\n";
                    SQL += ComFunc.covSqlstr("00", true);
                }
                else if (strDEPT.Trim().Equals("LIS") == true && strWorkSTS.Trim().Equals("W") == true && (strSpecCode.Trim().Equals("081") == true || strSpecCode.Trim().Equals("025") == true || strSpecCode.Trim().Equals("026") == true))
                {
                    // 외부의뢰 객담
                    SQL += " , '' \r\n";
                    SQL += ComFunc.covSqlstr("00", true);
                }
                else if (strDEPT.Trim().Equals("LIS") == true && (dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.MASTERCODE].ToString() == "YU01" || dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.MASTERCODE].ToString() == "YR01" || strMastercode == "YU01" || strMastercode == "YR01"))
                {
                    // 병리과 박지나s 요청 YR01 YU01 미접수로 되게 작업요청 20210728
                    SQL += " , '' \r\n";
                    SQL += ComFunc.covSqlstr("00", true);
                }
                else if (strDEPT.Trim().Equals("LIS") == true && strWorkSTS.Trim().Equals("U") == true)
                {
                    // 소변
                    SQL += " , '' \r\n";
                    SQL += ComFunc.covSqlstr("00", true);
                }
                else if (strDEPT.Trim().Equals("LIS") == true && strWorkSTS.Trim().Equals("M") == true)
                {
                    // 미생물

                    //// 2018-06-28 안정수, 이도경c 요청으로 외래이면서 (ER)제외, M이고 검체가 BLD인경우는 접수가 되도록
                    if (dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.DEPTCODE].ToString() != "ER" && strChkBLD == "Y")
                    {
                        SQL += ComFunc.covSqlDate(strBloodTime, "YYYY-MM-DD HH24:MI", true);
                        SQL += ComFunc.covSqlstr("01", true);
                    }

                    else
                    {
                        SQL += " , '' \r\n";
                        SQL += ComFunc.covSqlstr("00", true);
                    }
                }
                else if (strDEPT.Trim().Equals("LIS") == true && strWorkSTS.Trim().Equals("P,M") == true)
                {
                    // 대변,미생물                 
                    SQL += " , '' \r\n";
                    SQL += ComFunc.covSqlstr("00", true);

                }
                else if (strDEPT.Trim().Equals("LIS") == true && strWorkSTS.Trim().Equals("P") == true)
                {
                    // 대변
                    SQL += " , '' \r\n";
                    SQL += ComFunc.covSqlstr("00", true);
                }
                else if (strDEPT.Trim().Equals("LIS") == true && strWorkSTS.Trim().Equals("E") == true && strSpecCode.Trim().Equals("084") == true)
                {
                    //if (ComFunc.MsgBoxQ("대변검사입니다. 접수를 하겠습니까? 예:접수 아니오:미접수", null, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    //{
                    //    SQL += ComFunc.covSqlDate(strBloodTime, "YYYY-MM-DD HH24:MI", true);
                    //    SQL += ComFunc.covSqlstr("01", true);
                    //}
                    //else
                    //{
                    //    SQL += " , '' \r\n";
                    //    SQL += ComFunc.covSqlstr("00", true);
                    //}

                    // 면역 대변
                    SQL += " , '' \r\n"; 
                    SQL += ComFunc.covSqlstr("00", true);
                }
                else
                {
                    SQL += ComFunc.covSqlDate(strBloodTime, "YYYY-MM-DD HH24:MI", true);
                    SQL += ComFunc.covSqlstr("01", true);
                }

            }
            SQL += ComFunc.covSqlstr("0", true);
            SQL += ComFunc.covSqlDate(dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.ORDERDATE].ToString(), "YYYY-MM-DD HH24:MI", true);
            SQL += ComFunc.covSqlDate(dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.SENDDATE].ToString(),  "YYYY-MM-DD HH24:MI", true);

            if(dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.IPDOPD].ToString() == "O" && string.IsNullOrEmpty(dr[(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.WARD].ToString()) == true)
            {
                SQL += " , '', '' \r\n";
            }
            else
            {
                SQL += ComFunc.covSqlstr(strGbGwaExam, true);
                SQL += ComFunc.covSqlstr(strGbGwaExam2, true);  //2018-06-15 안정수, GB_GWAEXAM2 추가
            }
            

            SQL += ComFunc.covSqlstr("0", true);
            SQL += ComFunc.covSqlstr(nOrderNo, true);
            SQL += ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "  	     ,  SYSDATE                                                                              \r\n";
            SQL += ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "  	     ,  SYSDATE                                                                              \r\n";
            SQL += ") ";
             
            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 200);

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

        public string ins_EXAM_REULTC(PsmhDb pDbCon, string strSpecNo, string strWsBar, string strEquCode, int nSeqNo, string strPano, string strUnitCode, string strSubCode, string strMasterCode, string strResultIn, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = string.Empty;

            SQL = "";

            SQL += "    INSERT INTO KOSMOS_OCS.EXAM_RESULTC (SPECNO,RESULTWS,EQUCODE,SEQNO,PANO,MASTERCODE,SUBCODE,UNIT,STATUS, INPS, INPT_DT, UPPS, UPDT) \r\n";
            SQL += "  	SELECT                                                                                                  \r\n";
            SQL += "  		   '" + strSpecNo + "'   				AS SPECNO                                                   \r\n";
            SQL += "  		 , '" + strWsBar + "'    				AS RESULTWS                                                 \r\n";
            SQL += "  		 , '" + strEquCode + "'    				AS EQUCODE                                                  \r\n";
            SQL += "  		 , TRIM(TO_CHAR(ROWNUM + " + nSeqNo + " ,'000')) 	AS SEQNO                                        \r\n";
            SQL += "  		 , '" + strPano + "'					AS PANO                                                     \r\n";
            SQL += "  		 , '" + strMasterCode + "' 				AS MASTERCODE                                               \r\n";
            SQL += "  	     ,  SUBCODE                     AS SUBCODE                                                          \r\n";
            SQL += "  	     ,  UNIT                        AS UNIT                                                             \r\n";
            SQL += "  	     ,  STATUS                      AS STATUS                                                           \r\n";
            SQL += "  	     ,  '" + clsType.User.IdNumber + "'  AS INPS                                                        \r\n";
            SQL += "  	     ,  SYSDATE                     AS INPT_DT                                                          \r\n";
            SQL += "  	     ,  '" + clsType.User.IdNumber + "'  AS UPPS                                                        \r\n";
            SQL += "  	     ,  SYSDATE                     AS UPDT                                                             \r\n";
            SQL += "  	FROM (                                                                                                  \r\n";
            SQL += "  			SELECT                                                                                          \r\n";
            SQL += "  			         A.MASTERCODE	 	AS SUBCODE                                                          \r\n";
            SQL += "  			       , A.EQUCODE1 	 	AS EQUCODE                                                          \r\n";
            SQL += "  			       , KOSMOS_OCS.FC_EXAM_SPECMST_NM('20','" + strUnitCode + "', 'N') AS UNIT                 \r\n";
            SQL += "  			       , CASE WHEN '" + strResultIn + "' ='1' THEN 'H' ELSE 'N' END	 AS STATUS                  \r\n";
            SQL += "  			       , 0													 AS SORT                            \r\n";
            SQL += "  			    FROM KOSMOS_OCS.EXAM_MASTER 		A                                                       \r\n";
            SQL += "  			   WHERE 1=1                                                                                    \r\n";
            SQL += "  			     AND A.MASTERCODE = " + ComFunc.covSqlstr(strMasterCode, false);
            SQL += "  			UNION ALL                                                                                       \r\n";
            SQL += "  			SELECT                                                                                          \r\n";
            SQL += "  			         A.MASTERCODE	 	AS SUBCODE                                                          \r\n";
            SQL += "  			       , A.EQUCODE1 	 	AS EQUCODE                                                          \r\n";
            SQL += "  			       , KOSMOS_OCS.FC_EXAM_SPECMST_NM('20',A.UNITCODE, 'N') AS UNIT                            \r\n";
            SQL += "  			       , CASE WHEN A.RESULTIN ='1' THEN 'H' ELSE 'N' END	 AS STATUS                          \r\n";
            SQL += "  			       , B.SORT +1											 AS SORT                            \r\n";
            SQL += "  			    FROM KOSMOS_OCS.EXAM_MASTER 		A                                                       \r\n";
            SQL += "  			       , KOSMOS_OCS.EXAM_MASTER_SUB 	B                                                       \r\n";
            SQL += "  			   WHERE 1=1                                                                                    \r\n";
            SQL += "  			     AND A.MASTERCODE = TRIM(B.NORMAL)                                                          \r\n";
            SQL += "  			     AND B.MASTERCODE = " + ComFunc.covSqlstr(strMasterCode, false);
            SQL += "  			     AND B.GUBUN = '31'                                                                         \r\n";
            SQL += "  			     AND B.NORMAL > ' '                                                                         \r\n";
            SQL += "  			  ORDER BY SORT                                                                                 \r\n";
            SQL += "  	)                                                                                                       \r\n";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 200);

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

        public string ins_EXAM_ORDER_MENUAL(PsmhDb pDbCon, DataTable dt_EXAM_ORDER_Detail, ref int intRowAffected)
        {
            string SqlErr = "";
            string SQL = "";


            for (int i = 0; i < dt_EXAM_ORDER_Detail.Rows.Count; i++)
            {
                SQL = "";
                SQL += "    INSERT INTO KOSMOS_OCS.EXAM_ORDER  (                                          \r\n";
                SQL += "                IPDOPD          -- 입원,외래(I.입원 O.외래)                       \r\n";
                SQL += "              , BDATE           -- 진료일자                                       \r\n";
                SQL += "              , PANO            -- 등록번호                                       \r\n";
                SQL += "              , BI              -- 환자종류                                       \r\n";
                SQL += "              , SNAME           -- 환자성명                                       \r\n";
                SQL += "              , AGE             -- 나이                                           \r\n";
                SQL += "              , AGEMM           -- 유아 개월수                                    \r\n";
                SQL += "              , SEX             -- 성별(M:남, F:여)                               \r\n";
                SQL += "              , DEPTCODE        -- 진료과                                         \r\n";
                SQL += "              , DRCODE          -- 의사코드                                       \r\n";
                SQL += "              , WARD            -- 병동                                           \r\n";
                SQL += "              , ROOM            -- 병실                                           \r\n";
                SQL += "              , MASTERCODE      -- 검사코드(외래일때는 수가코드)                  \r\n";
                SQL += "              , QTY             -- 수가코드 수량(외래에서만 사용)                 \r\n";
                SQL += "              , SPECCODE        -- 검체코드                                       \r\n";
                SQL += "              , STRT            -- 응급여부(S.응급 E.STAT, R.Routine)             \r\n";
                SQL += "              , DRCOMMENT       -- 의사 Comment                                   \r\n";
                SQL += "              , ORDERNO         -- 입원검사 ORDER번호                             \r\n";
                SQL += "              , ORDERDATE       -- 검사지시 일시(2005/08/25부터)                  \r\n";
                SQL += "              , INPS            -- 입력자                                         \r\n";
                SQL += "              , INPT_DT         -- 입력일시                                       \r\n";
                SQL += "              , UPPS            -- 수정자                                         \r\n";
                SQL += "              , UPDT            -- 수정일시                                       \r\n";

                SQL += "   ) VALUES (                                                                     \r\n ";
                SQL += "             " + ComFunc.covSqlstr(dt_EXAM_ORDER_Detail.Rows[i][(int)enmSel_EXAM_ORDER_Detail.IPDOPD].ToString(), false);
                SQL += "             " + ComFunc.covSqlDate(dt_EXAM_ORDER_Detail.Rows[i][(int)enmSel_EXAM_ORDER_Detail.BDATE].ToString(), true);
                SQL += "             " + ComFunc.covSqlstr(dt_EXAM_ORDER_Detail.Rows[i][(int)enmSel_EXAM_ORDER_Detail.PANO  ].ToString(), true);
                SQL += "             " + ComFunc.covSqlstr(dt_EXAM_ORDER_Detail.Rows[i][(int)enmSel_EXAM_ORDER_Detail.BI    ].ToString(), true);
                SQL += "             " + ComFunc.covSqlstr(dt_EXAM_ORDER_Detail.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SNAME ].ToString(), true);
                SQL += "             " + ComFunc.covSqlstr(dt_EXAM_ORDER_Detail.Rows[i][(int)enmSel_EXAM_ORDER_Detail.AGE   ].ToString(), true);
                SQL += "             " + ComFunc.covSqlstr(dt_EXAM_ORDER_Detail.Rows[i][(int)enmSel_EXAM_ORDER_Detail.AGEMM ].ToString(), true);
                SQL += "             " + ComFunc.covSqlstr(dt_EXAM_ORDER_Detail.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SEX   ].ToString(), true);
                SQL += "             " + ComFunc.covSqlstr(dt_EXAM_ORDER_Detail.Rows[i][(int)enmSel_EXAM_ORDER_Detail.DEPTCODE].ToString(), true);
                SQL += "             " + ComFunc.covSqlstr(dt_EXAM_ORDER_Detail.Rows[i][(int)enmSel_EXAM_ORDER_Detail.DRCODE].ToString(), true);
                SQL += "             " + ComFunc.covSqlstr(dt_EXAM_ORDER_Detail.Rows[i][(int)enmSel_EXAM_ORDER_Detail.WARD  ].ToString(), true);
                SQL += "             " + ComFunc.covSqlstr(dt_EXAM_ORDER_Detail.Rows[i][(int)enmSel_EXAM_ORDER_Detail.ROOM  ].ToString(), true);
                SQL += "             " + ComFunc.covSqlstr(dt_EXAM_ORDER_Detail.Rows[i][(int)enmSel_EXAM_ORDER_Detail.MASTERCODE].ToString(), true);
                SQL += "             " + ComFunc.covSqlstr(dt_EXAM_ORDER_Detail.Rows[i][(int)enmSel_EXAM_ORDER_Detail.QTY   ].ToString(), true);
                SQL += "             " + ComFunc.covSqlstr(dt_EXAM_ORDER_Detail.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SPEC_SPECCODE].ToString(), true);
                SQL += "             " + ComFunc.covSqlstr(dt_EXAM_ORDER_Detail.Rows[i][(int)enmSel_EXAM_ORDER_Detail.STRT  ].ToString(), true);
                SQL += "             , '메뉴얼접수'                                                       \r\n";
                SQL += "             , 999 -- 메뉴얼접수                                                  \r\n";
                SQL += "             , SYSDATE                                                            \r\n";

                SQL += "             " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
                SQL += "             , SYSDATE                                                            \r\n";
                SQL += "             " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
                SQL += "             , SYSDATE                                                            \r\n";

                SQL += "   )                                                                      \r\n ";

                try
                {
                    SqlErr += clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

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
            }

            return SqlErr;
        }

        public bool EXAM_SPEC_WRITE_TLA2(DataTable dt_EXAM_ORDER_Detail, bool isMENUAL, string gStrDEPT, ref DataTable dt_EXAM_ORDER_RETURN, DataTable dt_EXAM_ORDER)
        {
            bool b = true;

            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;

            PsmhDb pDbCon = null;
            pDbCon = clsDB.DBConnect();

            //2018.04.08.김홍록:  기존 스프레드 정렬 방식을 datatable로 변경함.
            DataTable dt_BARCODE = rcpSQL.sel_EXAM_MASTER_BARCODE(pDbCon, true, "", "", "", "", "", "", "", "", "");

            dt_EXAM_ORDER_RETURN = dt_BARCODE.Clone();

            clsDB.setBeginTran(pDbCon);

            if (isMENUAL == true)
            {
                if (ComFunc.isDataTableNull(dt_EXAM_ORDER) == true)
                {
                    ComFunc.MsgBox("메뉴얼 접수시 처방 정보 입력 오류 발생 [dt_EXAM_ORDER]");
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.DisDBConnect(pDbCon);
                    pDbCon = null;
                    return false;
                }

                SqlErr = ins_EXAM_ORDER_MENUAL(pDbCon, dt_EXAM_ORDER, ref intRowAffected);

                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.DisDBConnect(pDbCon);
                    pDbCon = null;
                    return false;
                }

                dt_EXAM_ORDER_Detail = null;

                DataSet ds = rcpSQL.sel_EXAM_ORDER_DETAIL(pDbCon, dt_EXAM_ORDER.Rows[0][(int)enmSel_EXAM_ORDER_Detail.PANO].ToString().Trim()
                                                                          , dt_EXAM_ORDER.Rows[0][(int)enmSel_EXAM_ORDER_Detail.BDATE].ToString().Trim()
                                                                          , dt_EXAM_ORDER.Rows[0][(int)enmSel_EXAM_ORDER_Detail.IPDOPD].ToString().Trim()
                                                                          , dt_EXAM_ORDER.Rows[0][(int)enmSel_EXAM_ORDER_Detail.DEPTCODE].ToString().Trim()
                                                                          , clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_STATUS.READY
                                                                          , true
                                                                          , clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_PT_STATUS.OPT
                                                                          , "LIS"
                                                                          , ""
                                                                          );

                if (ComFunc.isDataSetNull(ds) == true)
                {
                    ComFunc.MsgBox("메뉴얼 접수시 처방 정보 입력 오류 발생 [sel_EXAM_ORDER_DETAIL]");
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.DisDBConnect(pDbCon);
                    pDbCon = null;
                    return false;
                }

                dt_EXAM_ORDER_Detail = ds.Tables[0].Clone();

                dt_EXAM_ORDER_Detail = ds.Tables[0];

            }



            DateTime sysdate        = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-"));
            string GstrSysDate      = sysdate.ToString("yyyy-MM-dd");
            string GstrSysTime      = sysdate.ToString("HH:mm");

            string strMasterCode    = ""; // Trim(P(TORD.Order(i), "^", 1))' 검사코드
            string strDrSpec        = ""; // Trim(P(TORD.Order(i), "^", 2))' 의사선택 검체코드
            string strSTRT          = ""; // Trim(P(TORD.Order(i), "^", 3))' 응급여부
            string strDrComment     = ""; // Trim(P(TORD.Order(i), "^", 4))' 의사컴멘트                                                                                                                                                        
            string strSuCode        = ""; // Trim(P(TORD.Order(i), "^", 5))' 수가코드,오더코드(rowid)
            string strOTime         = ""; // Trim(P(TORD.Order(i), "^", 6))' 의사채혈희망시각
            string strOrderDate     = ""; // Trim(P(TORD.Order(i), "^", 7))' 오더시각
            string strSendDate      = ""; // Trim(P(TORD.Order(i), "^", 8))' 전송시각
            string nOrderNo         = ""; // Trim(P(TORD.Order(i), "^", 9)))'orderno
            string strWS_GRP        = ""; // READ_BasCode("WS그룹", strWsCode) 'WS그룹 00
            string strWsBar         = ""; // READ_BasCode("WS약어", strWsCode)     'BarCode Work Station H
            string strWSGRP_TITLE   = ""; // EXAM_SPEC_WRITE_WsGrp_SET 대체
            string strBloodTime     = GstrSysDate + " " + GstrSysTime;
            string strGbGwaExam     = "N";
            string strGbGwaExam2    = ""; // 2018-06-15 안정수, 과검사 유무인지 Check 후 어느 과에서 시행하는지 담는 변수 
            string strSpecCode      = "";
            string strPrt           = "Y";
            string strWsCode        = "";
            string strWsPos         = "";
            string strTubeCode      = "";
            string strResultIn      = "";
            string strUnitCode      = "";
            string strMomo          = "";
            string strEquCode       = "";
            string strPiece         = "";
            string strGBTLA         = "";
            string strSerialOK      = "";

            string strPANO          = dt_EXAM_ORDER_Detail.Rows[0][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.PANO   ].ToString().Trim();
            string strJOB           = dt_EXAM_ORDER_Detail.Rows[0][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.IPDOPD ].ToString().Trim();
            string strSNAME         = dt_EXAM_ORDER_Detail.Rows[0][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.SNAME  ].ToString().Trim();

            try
            {
                for (int i = 0; i < dt_EXAM_ORDER_Detail.Rows.Count; i++)
                {
                    #region 1. Serial, Mother, 낱개를 dt_BARCODE에 넣기

                    strMasterCode   = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.MASTERCODE ].ToString().Trim(); // Trim(P(TORD.Order(i), "^", 1))' 검사코드                    
                    strDrSpec       = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.DRSPEC     ].ToString().Trim(); // Trim(P(TORD.Order(i), "^", 2))' 의사선택 검체코드
                    strSTRT         = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.STRT       ].ToString().Trim(); // Trim(P(TORD.Order(i), "^", 3))' 응급여부
                    strDrComment    = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.DRCOMMENT  ].ToString().Trim(); // Trim(P(TORD.Order(i), "^", 4))' 의사컴멘트                                                                                                                                                                
                    strSuCode       = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.ROWID_R    ].ToString().Trim(); // Trim(P(TORD.Order(i), "^", 5))' 수가코드,오더코드(rowid)
                    strOTime        = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.BLOODDATE  ].ToString().Trim(); // Trim(P(TORD.Order(i), "^", 6))' 의사채혈희망시각
                    strOrderDate    = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.ORDERDATE  ].ToString().Trim(); // Trim(P(TORD.Order(i), "^", 7))' 오더시각
                    strSendDate     = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.SENDDATE   ].ToString().Trim(); // Trim(P(TORD.Order(i), "^", 8))' 전송시각
                    nOrderNo        = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.ORDERNO    ].ToString().Trim(); // Trim(P(TORD.Order(i), "^", 9)))'orderno
                    strWS_GRP = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.WS_GRP           ].ToString().Trim(); // READ_BasCode("WS그룹", strWsCode) 'WS그룹 00
                    strWsBar = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.WS_YAK            ].ToString().Trim(); // READ_BasCode("WS약어", strWsCode)     'BarCode Work Station H
                    strWSGRP_TITLE = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.WSGRP_TITLE ].ToString().Trim(); // EXAM_SPEC_WRITE_WsGrp_SET 대체

                    if (string.IsNullOrEmpty(strOTime.Trim()) == false)
                    {
                        strBloodTime = GstrSysDate + " " + strOTime;
                    }

                    if (string.IsNullOrEmpty(strDrSpec.Trim()) == true)
                    {
                        strSpecCode = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.SPECCODE].ToString().Trim();
                    }
                    else
                    {
                        strSpecCode = strDrSpec.Trim();
                    }

                    strWsCode = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.WSCODE1      ].ToString().Trim();    // Trim(AdoGetString(RsExma, "WsCode1", 0))
                    strWsPos = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.WSCODE1POS    ].ToString().Trim();    // Format(AdoGetNumber(RsExma, "WsCode1Pos", 0), "00000")
                    strTubeCode = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.TUBECODE   ].ToString().Trim();    // Trim(AdoGetString(RsExma, "TubeCode", 0))
                    strResultIn = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.RESULTIN   ].ToString().Trim();    // Trim(AdoGetString(RsExma, "ResultIn", 0))
                    strUnitCode = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.UNITCODE   ].ToString().Trim();    // Trim(AdoGetString(RsExma, "UnitCode", 0))

                    //2018.04.07.김홍록: 2014년 이후 발생하지 않는 코드.
                    //If strMasterCode = "BF10" And strDrSpec = "031" Then
                    //   strUnitCode = "043"
                    //End If

                    strMomo = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.MOTHER         ].ToString().Trim(); // Trim(AdoGetString(RsExma, "Mother", 0))
                    strEquCode = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.EQUCODE1    ].ToString().Trim(); // Trim(AdoGetString(RsExma, "EquCode1", 0))
                    strPiece = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.PIECE         ].ToString().Trim(); // Trim(AdoGetString(RsExma, "Piece", 0))
                    strGBTLA = dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.GBTLA         ].ToString().Trim(); // Trim(AdoGetString(RsExma, "GBTLA", 0))
                    strSerialOK = "";   //연속검사 여부

                    if (string.IsNullOrEmpty(dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.SERIES].ToString().Trim()) == false)
                    {
                        strSerialOK = "OK";
                    }

                    //과검사(응급실) 여부
                    if (dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.GWA_EXAM_CHECK].ToString().Trim().Equals("Y") == true)
                    {
                        strGbGwaExam = "Y";

                        //2018-06-15 안정수 추가
                        strGbGwaExam2 = gStrDEPT;
                        if(strGbGwaExam2 == "")
                        {
                            if (strMasterCode == "UC15E")
                            {
                                strGbGwaExam2 = "ER";
                            }

                            if (dt_EXAM_ORDER_Detail.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.DEPTCODE].ToString().Trim() == "ER")
                            {
                                strGbGwaExam2 = "ER";
                            }
                        }
                        //strSpecCode = "080"; 
                        //strTubeCode = "110";
                    }
                    else
                    { 
                        strGbGwaExam = "";
                        strGbGwaExam2 = "";
                    }

                    ////2019-01-29 안정수 
                    //if(dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_ORDER_DETAIL.MASTERCODE].ToString().Trim() == "UC15E")
                    //{

                    //}

                    if (strSerialOK.Equals("OK") == true || strMomo.Equals("1") == true)
                    {
                        DataTable dt = null;
                        if (strSerialOK.Equals("OK") == true)
                        {
                            dt = rcpSQL.sel_EXAM_MASTER_BARCODE(pDbCon, true, strDrSpec, strMasterCode, strSuCode, strSTRT, strDrComment, strBloodTime, strGbGwaExam, nOrderNo, strPrt);
                        }
                        else
                        {
                            dt = rcpSQL.sel_EXAM_MASTER_BARCODE(pDbCon, false, strDrSpec, strMasterCode, strSuCode, strSTRT, strDrComment, strBloodTime, strGbGwaExam, nOrderNo, strPrt);
                        }

                        if (ComFunc.isDataTableNull(dt) == true)
                        {
                            // TODO : *************************************************** 다시 점검
                            string strMsg = strMasterCode + "의 SUB코드가 검사 마스터에 등록되지 않았습니다.";
                            ComFunc.MsgBox(strMasterCode);
                            strWsCode = "";
                            strTubeCode = "";
                        }
                        else
                        {
                            for (int q = 0; q < dt.Rows.Count; q++)
                            {
                                dt_BARCODE.ImportRow(dt.Rows[q]);
                            }
                        }
                    }
                    else
                    {
                        DataRow row = dt_BARCODE.NewRow();

                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.BAR_CNT.ToString()       ] = "00";           // spBarCode.Col = 1:  spBarCode.Text = "00"            ' 바코드장수
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.SPECCODE.ToString()      ] = strSpecCode;    // spBarCode.Col = 2:  spBarCode.Text = strSpecCode     '검체코드    011
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.WS_GRP.ToString()        ] = strWS_GRP;      // spBarCode.Col = 3:  spBarCode.Text = READ_BasCode("WS그룹", strWsCode) 'WS그룹 00
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.TUBECODE.ToString()      ] = strTubeCode;    // spBarCode.Col = 4:  spBarCode.Text = strTubeCode     ' 용기코드 001
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.WSCODE.ToString()        ] = strWsCode;      // spBarCode.Col = 5:  spBarCode.Text = strWsCode       ' WS코드   101
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.WSPOS.ToString()         ] = strWsPos;       // spBarCode.Col = 6:  spBarCode.Text = strWsPos        ' WS POS   00001
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.MASTERCODE.ToString()    ] = strMasterCode;  // spBarCode.Col = 7:  spBarCode.Text = strMasterCode
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.SPECNO.ToString()        ] = "";             // spBarCode.Col = 8:  spBarCode.Text = ""              ' 검체번호
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.STRT.ToString()          ] = strSTRT;        // spBarCode.Col = 9:  spBarCode.Text = strSTRT
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.DRCOMMENT.ToString()     ] = strDrComment;   // spBarCode.Col = 10: spBarCode.Text = strDrComment    ' 의사컴멘트
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.WSBAR.ToString()         ] = strWsBar;       // spBarCode.Col = 11: spBarCode.Text = READ_BasCode("WS약어", strWsCode)     'BarCode Work Station
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.RESULTIN.ToString()      ] = strResultIn;    // spBarCode.Col = 12: spBarCode.Text = strResultIn     ' 결과입력
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.UNITCODE.ToString()      ] = strUnitCode;    // spBarCode.Col = 13: spBarCode.Text = strUnitCode     ' 결과단위
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.EQUCODE.ToString()       ] = strEquCode;     // spBarCode.Col = 14: spBarCode.Text = strEquCode      ' 장비코드
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.SUCODE.ToString()        ] = strSuCode;      // spBarCode.Col = 15: spBarCode.Text = strSuCode       ' ROWID
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.WSGRP_TITLE.ToString()   ] = strWSGRP_TITLE; // spBarCode.Col = 16: spBarCode.Text = strWsGrp        ' WS대표코드  100
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.BLOODTIME.ToString()     ] = strBloodTime;   // spBarCode.Col = 17: spBarCode.Text = strBloodTime    ' 채혈시각
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.GB_GWAEXAM.ToString()    ] = strGbGwaExam;   // spBarCode.Col = 18: spBarCode.Text = strGbGwaExam    ' 과검사여부(Y/N)
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.PIECE.ToString()         ] = strPiece;       // spBarCode.Col = 19: spBarCode.Text = strPiece        ' 개별발행여부
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.ORDERNO.ToString()       ] = nOrderNo;       // spBarCode.Col = 20: spBarCode.Text = nOrderNo        ' 오더번호
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.PRT.ToString()           ] = strPrt;         // spBarCode.Col = 21: spBarCode.Text = strPrt          ' 실제발행여부
                        row[clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.GBTLA.ToString()         ] = strGBTLA;       // spBarCode.Col = 22: spBarCode.Text = strGBTLA        ' TLA

                        dt_BARCODE.Rows.Add(row);
                    }

                    #endregion
                }

                dt_BARCODE.DefaultView.Sort = "BAR_CNT,MASTERCODE,SPECCODE,GB_GWAEXAM";
                dt_BARCODE = dt_BARCODE.DefaultView.ToTable();

                //'연속검사,검사코드,검체코드가 동일한것이 있으면 01,02,03, ... 번호를 부여하여 바코드를 각각 인쇄함

                int j = 0;

                string strOldData = "";
                string strNewData = "";

                for (int i = 0; i < dt_BARCODE.Rows.Count; i++)
                {
                    strNewData = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.BAR_CNT].ToString().Trim();

                    if (Convert.ToInt32(strNewData) < 21)
                    {
                        #region 2. BT022, BT023, PIECE 독립적으로 출력 되도록

                        string strMASTERCODE = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.MASTERCODE].ToString().Trim();

                        if (strMASTERCODE.Equals("BT022") == true || strMASTERCODE.Equals("BT023") == true)
                        {
                            strNewData += "BT" + string.Format("{0:000}", i);
                        }
                        else
                        {
                            strNewData += strMASTERCODE;        //00HR01
                        }

                        if (dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.PIECE].ToString().Trim().Equals("1") == true)
                        {
                            strNewData += dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.SPECCODE].ToString().Trim();
                        }

                        strNewData += dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.SPECCODE].ToString().Trim();   // 00HR01011

                        //2019-04-08 안정수, 읽기전용 오류로 인하여 추가함 
                        dt_BARCODE.Columns[(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.BAR_CNT].ReadOnly = false;

                        if (strOldData.Trim() != strNewData.Trim())
                        {
                            dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.BAR_CNT] = "01";
                            strOldData = strNewData;
                            j = 1;
                        }
                        else
                        {
                            j += 1;
                            dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.BAR_CNT] = string.Format("{0:00}", j);
                        }

                        # endregion  
                    }
                }

                dt_BARCODE.DefaultView.Sort = "BAR_CNT,SPECCODE,GBTLA,WS_GRP,TUBECODE,WSGRP_TITLE,WSPOS,MASTERCODE,GB_GWAEXAM";
                dt_BARCODE = dt_BARCODE.DefaultView.ToTable();

                strOldData      = "";
                strSTRT         = "R";
                strDrComment    = "";
                strGbGwaExam    = "";
                strGBTLA        = "";

                //string gsBarSpecNo = "";
                string strSpecNo = "";
                string strWorkSTS = "";
                int nSeqNo = 0;
                //string strGbOrder = "";

                string strNewDataGu = "";
                string strOldDataGu = "";

                for (int i = 0; i < dt_BARCODE.Rows.Count; i++)
                {
                    #region 3. EXAM_REULTC, EXAM_SPECMST, EXAM_ORDER 입력

                    strNewData  = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.BAR_CNT].ToString().Trim(); //01
                    strNewData += dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.SPECCODE].ToString().Trim(); //01011
                    

                    if (dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.GBTLA].ToString().Equals("1") == true)
                    {
                        strNewData += "TLA";
                    }
                    else
                    {
                        strNewData += dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.WS_GRP].ToString().Trim(); //0101100
                    }

                    strNewData += dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.TUBECODE].ToString().Trim(); //0101100001
                    strNewData += dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.GB_GWAEXAM].ToString().Trim(); //0101100001N
                   
                    if (dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.PIECE].ToString().Trim().Equals("1") == true)
                    {
                        strNewData += dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.SPECCODE].ToString().Trim() + i.ToString();
                    }

                    #region 2018-10-11 안정수, ER요청으로 VBGA검사와 HCG검사 분리를 위함

                    if (strGbGwaExam2 == "ER" 
                        && strOldData == strNewData 
                        && (
                                dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.MASTERCODE].ToString().Trim() == "UC15E"
                                || dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.MASTERCODE].ToString().Trim() == "CR617(ER")
                           )
                    {
                        strNewDataGu = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.MASTERCODE].ToString().Trim(); 

                        if (strOldDataGu != strNewDataGu)
                        {
                            strNewData += dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.MASTERCODE].ToString().Trim();
                        }

                        strOldDataGu = strNewDataGu;
                    }
                    #endregion


                    //#endregion

                    if (strOldData.Trim() != strNewData.Trim())
                    {

                        if (i == 0)
                        {
                            nOrderNo = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.ORDERNO].ToString().Trim(); //01
                        }
                        else
                        {
                            nOrderNo = dt_BARCODE.Rows[i-1][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.ORDERNO].ToString().Trim(); //01
                        }


                        //2018-06-15 안정수, 파라메타 strGbGwaExam2 추가
                        SqlErr = ins_EXAM_SPECMST(pDbCon, dt_EXAM_ORDER_Detail.Rows[0], gStrDEPT, nOrderNo, strSpecNo, strSTRT, strSpecCode, strTubeCode, strBloodTime, ref strWorkSTS, ref strDrComment, strGbGwaExam, strGbGwaExam2, ref intRowAffected, "");
                        

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            clsDB.DisDBConnect(pDbCon);
                            pDbCon = null;

                            return false;
                        }

                        strSpecNo = lbExSQL.sel_SpecNO(pDbCon);

                        strSpecCode     = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.SPECCODE].ToString().Trim();
                        strTubeCode     = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.TUBECODE].ToString().Trim();
                        strGbGwaExam    = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.GB_GWAEXAM].ToString().Trim();                        

                        nSeqNo          = 0;
                        strWorkSTS      = "";
                        strDrComment    = "";
                        strOldData      = strNewData;
                     
                    }

                    //2019-04-08 안정수, 읽기전용 오류로 인하여 추가함 
                    dt_BARCODE.Columns[(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.SPECNO].ReadOnly = false;
                    dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.SPECNO] = strSpecNo;

                    if (dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.STRT].ToString().Equals("응급") == true)
                    {
                        strSTRT = "S";
                    }
                    else
                    {
                        strSTRT = "R";
                    }

                    strWsBar = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.WSBAR].ToString().Trim();

                    if (string.IsNullOrEmpty(strWsBar.Trim()) == false)
                    {
                        if (strWorkSTS.IndexOf(strWsBar) < 0)
                        {
                            strWorkSTS += strWsBar + ",";
                        }
                    }

                    strWsCode       = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.WSCODE     ].ToString().Trim();
                    strMasterCode   = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.MASTERCODE ].ToString().Trim();
                    strResultIn     = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.RESULTIN   ].ToString().Trim();
                    strUnitCode     = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.UNITCODE   ].ToString().Trim();
                    strEquCode      = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.EQUCODE    ].ToString().Trim();
                    strSuCode       = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.SUCODE    ].ToString().Trim();
                    strBloodTime    = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.BLOODTIME  ].ToString().Trim();

                    if (string.IsNullOrEmpty(dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.DRCOMMENT].ToString().Trim()) == false)
                    {
                        if (string.IsNullOrEmpty(strDrComment.Trim()) == false)
                        {
                            strDrComment += ", \r\n";
                        }
                        strDrComment += strMasterCode + ":" + dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.DRCOMMENT].ToString().Trim();
                    }

                    //strGbOrder = "1";
                    //string strSubCode = "";

                    //nOrderNo = dt_BARCODE.Rows[i][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.ORDERNO].ToString().Trim(); //01

                    SqlErr = ins_EXAM_REULTC(pDbCon, strSpecNo, strWsBar, strEquCode, nSeqNo, strPANO, strUnitCode, strMasterCode, strMasterCode, strResultIn, ref intRowAffected);
                    if (b == false)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        clsDB.DisDBConnect(pDbCon);
                        pDbCon = null;

                        return false; 

                    }

                    //**************************************

                    nSeqNo += intRowAffected;

                    //**************************************
                    //if (strJOB.Trim().Equals("O") == false || string.IsNullOrEmpty(strSuCode.Trim()) == true || string.IsNullOrEmpty(strSpecNo) == true)
                    if (string.IsNullOrEmpty(strSuCode.Trim()) == true || string.IsNullOrEmpty(strSpecNo) == true)
                    {
                        ComFunc.MsgBox("검체번호오류-clsLbExBarCodePrint");
                        clsDB.setRollbackTran(pDbCon);
                        clsDB.DisDBConnect(pDbCon);
                        pDbCon = null;
                        return false;
                    }

                    SqlErr = lbExSQL.up_EXAM_ORDER(pDbCon, strSpecNo, strSuCode, ref intRowAffected);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        clsDB.DisDBConnect(pDbCon);
                        pDbCon = null;

                        return false;
                    }

                    #endregion
                }

                nOrderNo = dt_BARCODE.Rows[dt_BARCODE.Rows.Count-1][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.ORDERNO].ToString().Trim(); //01

                //2018-06-15 안정수, 파라메타 strGbGwaExam2 추가

                if(strBloodTime == "")
                {
                    strBloodTime = GstrSysDate + " " + GstrSysTime;
                }

                SqlErr = ins_EXAM_SPECMST(pDbCon, dt_EXAM_ORDER_Detail.Rows[0],gStrDEPT, nOrderNo, strSpecNo, strSTRT, strSpecCode, strTubeCode, strBloodTime, ref strWorkSTS, ref strDrComment, strGbGwaExam, strGbGwaExam2,ref intRowAffected, dt_BARCODE.Rows[dt_BARCODE.Rows.Count - 1][(int)clsComSupLbExRcpSQL.enmSel_EXAM_MASTER_BARCODE.MASTERCODE].ToString().Trim());
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.DisDBConnect(pDbCon);
                    pDbCon = null;  

                    return false;  
                }

                clsDB.setCommitTran(pDbCon);
                pDbCon = null;

                dt_EXAM_ORDER_RETURN = dt_BARCODE;

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.ToString());
                clsDB.setRollbackTran(pDbCon);
                clsDB.DisDBConnect(pDbCon);
                pDbCon = null;
                return false;
            } 

            //return b;
        }
        
        public int sel_EXAM_RESULTC_PB(PsmhDb pDbCon, string strSPECNO)
        {
            DataTable dt = null;

            int nReturn = -1;

            string SqlErr = ""; //에러문 받는 변수

            string SQL = string.Empty;

            SQL = "";

            SQL += "  SELECT COUNT(*) CNT FROM KOSMOS_OCS.EXAM_RESULTC                \r\n";
            SQL += "  WHERE SPECNO      = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "    AND MASTERCODE  IN ('HR10')";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return -1;
                }

                nReturn = Convert.ToInt32(dt.Rows[0][0].ToString());
                
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return -1;
            }

            return nReturn;

        }

        /// <summary>insert EXAM_REULTC</summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string ins_EXAM_REULTC(PsmhDb pDbCon, DataRow dr, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "    INSERT INTO KOSMOS_OCS.EXAM_RESULTC(    \r\n";
            SQL += "      SPECNO                                \r\n";
            SQL += "    , RESULTWS                              \r\n";
            SQL += "    , EQUCODE                               \r\n";
            SQL += "    , SEQNO                                 \r\n";
            SQL += "    , PANO                                  \r\n";
            SQL += "    , MASTERCODE                            \r\n";
            SQL += "    , SUBCODE                               \r\n";
            SQL += "    , UNIT                                  \r\n";
            SQL += "    , STATUS                                \r\n";
            SQL += "    )                                       \r\n";
            SQL += " VALUES(                              \r\n";
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ORDER_Detail.SPECNO    ].ToString(), false);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ORDER_Detail.WSNM      ].ToString(), true);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ORDER_Detail.EQUCODE1  ].ToString(), true);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ORDER_Detail.RSLT_SEQNO].ToString(), true);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ORDER_Detail.PANO      ].ToString(), true);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ORDER_Detail.MASTERCODE].ToString(), true);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ORDER_Detail.SUBCODE   ].ToString(), true);
            SQL += ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ORDER_Detail.UNITNM  ].ToString(), true);
            if (dr[(int)enmSel_EXAM_ORDER_Detail.RESULTIN].ToString().Trim().Equals("1"))
            {
                SQL += ", 'H' -- STATUS                                                                                             \r\n";
            }
            else
            {
                SQL += ", 'N' -- STATUS                                                                                             \r\n";
            }
            SQL += ") ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
        
        /// <summary>insert EXAM_SPECMST/// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string ins_EXAM_SPECMST(PsmhDb pDbCon, DataRow dr, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "EXAM_SPECMST( \r\n";
            SQL += "       SPECNO                         \r\n";
            SQL += "     , PANO                           \r\n";
            SQL += "     , BI                             \r\n";
            SQL += "     , SNAME                          \r\n";
            SQL += "     , IPDOPD                         \r\n";
            SQL += "     , AGE                            \r\n";
            SQL += "     , AGEMM                          \r\n";
            SQL += "     , SEX                            \r\n";
            SQL += "     , DEPTCODE                       \r\n";
            SQL += "     , WARD                           \r\n";
            SQL += "     , ROOM                           \r\n";
            SQL += "     , DRCODE                         \r\n";
            SQL += "     , DRCOMMENT                      \r\n";
            SQL += "     , STRT                           \r\n";
            SQL += "     , SPECCODE                       \r\n";
            SQL += "     , TUBE                           \r\n";
            SQL += "     , WORKSTS                        \r\n";
            SQL += "     , BDATE                          \r\n";
            SQL += "     , BLOODDATE                      \r\n";
            SQL += "     , RECEIVEDATE                    \r\n";
            SQL += "     , STATUS                         \r\n";
            SQL += "     , EMR                            \r\n";
            SQL += "     , ORDERDATE                      \r\n";
            SQL += "     , SENDDATE                       \r\n";
            SQL += "     , GB_GWAEXAM                     \r\n";
            SQL += "     , ORDERNO                        \r\n";
            SQL += " )                                    \r\n";
            SQL += " VALUES(                              \r\n";
            
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.SPECNO            ].ToString(), false);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.PANO              ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.BI                ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.SNAME             ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.IPDOPD            ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.AGE               ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.AGEMM             ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.SEX               ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.DEPTCODE          ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.WARD              ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.ROOM              ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.DRCODE            ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.DRCOMMENT         ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.STRT              ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.SPECODE           ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.TUBECODE          ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.SPEC_WS_NM        ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.BDATE             ].ToString(), true);
            SQL += ComFunc.covSqlDate(dr[(int)enmSel_EXAM_ORDER_Detail.SPEC_BLOODDATE    ].ToString(), "YYYY-MM-DD HH24:MI", true);
            SQL += ComFunc.covSqlDate(dr[(int)enmSel_EXAM_ORDER_Detail.SPEC_RECEIVEDATE  ].ToString(), "YYYY-MM-DD HH24:MI", true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.SPEC_STATUS       ].ToString(), true);
            SQL += ", '0' -- EMR                                                                                                    \r\n";
            SQL += ComFunc.covSqlDate(dr[(int)enmSel_EXAM_ORDER_Detail.ORDERDATE         ].ToString(), "YYYY-MM-DD HH24:MI", true);
            SQL += ComFunc.covSqlDate(dr[(int)enmSel_EXAM_ORDER_Detail.SENDDATE          ].ToString(), "YYYY-MM-DD HH24:MI", true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.CHKGWA            ].ToString(), true);
            SQL += ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ORDER_Detail.ORDERNO           ].ToString(), true);
            SQL += ") ";

            try
            {
                SqlErr += clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }

            return SqlErr;
        }

        public DataTable sel_EXAM_ORDER_Detail(PsmhDb pDbCon, string strBDATE, string strPANO, string strDEPTCODE, string strIPDOPD, string strJOB, string strExcepEXAM_CODE)
        {
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            string SQL = string.Empty;

            SQL = "";

            SQL += "  WITH T AS  (                                                                                                                                                                                  \r\n";
            SQL += "  		SELECT                                                                                                                                                                                  \r\n";
            SQL += "  			   O.PANO                                                                          AS PANO                                                                                          \r\n";
            SQL += "  	  		 , O.BI                                                                            AS BI                                                                                            \r\n";
            SQL += "  	  		 , O.SNAME                                                                         AS SNAME                                                                                         \r\n";
            SQL += "  	  		 , O.IPDOPD                                                                        AS IPDOPD                                                                                        \r\n";
            SQL += "  	  		 , O.AGE                                                                           AS AGE                                                                                           \r\n";
            SQL += "  	  		 , O.AGEMM                                                                         AS AGEMM                                                                                         \r\n";
            SQL += "  	  		 , O.SEX                                                                           AS SEX                                                                                           \r\n";
            SQL += "  	  		 , O.DEPTCODE                                                                      AS DEPTCODE                                                                                      \r\n";
            SQL += "  	  		 , O.WARD                                                                          AS WARD                                                                                          \r\n";
            SQL += "  	  		 , O.DRCODE                                                                        AS DRCODE                                                                                        \r\n";
            SQL += "  	  		 , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(O.DRCODE)		                               AS DR_NM                                                                                         \r\n";
            SQL += "  	  		 , O.ROOM                                                                          AS ROOM                                                                                          \r\n";
            SQL += "  	  		 , TO_CHAR(O.BDATE,'YYYY-MM-DD')					                               AS BDATE                                                                                         \r\n";
            SQL += "  	  		 , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_응급실검사', M.MASTERCODE)               AS CHKGWA                                                                                        \r\n";
            SQL += "  	  		 , O.STRT                                                                          AS STRT                                                                                        \r\n";
            SQL += "  	  		 , O.SPECCODE										                               AS DR_SPECCODE                                                                                   \r\n";
            SQL += "   	  		 , M.SPECCODE																	   AS SPECCODE                                                                                      \r\n";
            SQL += "  		     , M.MASTERCODE																	   AS MASTERCODE                                                                                    \r\n";
            SQL += "  		     , O.DRCOMMENT																	   AS DRCOMMENT                                                                                     \r\n";
            SQL += "  		     , O.ROWID                                      								   AS ROW_ID                                                                                        \r\n";
            SQL += "  		     , TO_CHAR(O.RDATE,'YYYY-MM-DD')												   AS RDATE                                                                                         \r\n";
            SQL += "  		     , TO_CHAR(O.SDATE,'YYYY-MM-DD')												   AS SDATE                                                                                         \r\n";
            SQL += "     		 , TO_CHAR(O.ORDERDATE,'YYYY-MM-DD')											   AS ORDERDATE                                                                                     \r\n";
            SQL += "     		 , TO_CHAR(O.SENDDATE,'YYYY-MM-DD')												   AS SENDDATE                                                                                      \r\n";
            SQL += "     		 , TO_CHAR(ORDERNO)																   AS ORDERNO                                                                                       \r\n";
            SQL += "  			 , KOSMOS_OCS.FC_IPD_NEW_MASTER_GBSTS('', O.PANO)								   AS CHKIO                                                                                         \r\n";
            SQL += "  			 , M.EXAMNAME																	   AS EXAMNAME                                                                                      \r\n";
            SQL += "             , M.TUBECODE                                                                      AS TUBECODE     -- 31(마스터)                                                                    \r\n";
            SQL += "             , KOSMOS_OCS.FC_EXAM_SPECMST_NM('15', M.TUBECODE, 'Y')                            AS TUBENAME     -- 32(마스터)                                                                    \r\n";
            SQL += "             , M.UNITCODE																	   AS UNITCODE     -- 33(마스터)                                                                    \r\n";
            SQL += "             , KOSMOS_OCS.FC_WSGRP(M.WSCODE1)                                                  AS WSGRP        -- 34(마스터)                                                                    \r\n";
            SQL += "             , M.WSCODE1                                                                       AS WSCODE1      -- 35(마스터)                                                                    \r\n";
            SQL += "             , TO_CHAR(M.WSCODE1POS, '00000')                                                  AS WSCODE1POS   -- 36(마스터)                                                                    \r\n";
            SQL += "             , KOSMOS_OCS.FC_EXAM_SPECMST_NM('12', M.WSCODE1, 'W')                             AS WSGRPNM      --                                                                               \r\n";
            SQL += "             , KOSMOS_OCS.FC_EXAM_SPECMST_NM('12', M.WSCODE1, 'Y')                             AS WSNM         -- 11                                                                            \r\n";
            SQL += "             , M.RESULTIN                                                                      AS RESULTIN     -- 37(마스터)     결과입력 유무(1:결과 입력 안함, 0:결과 입력 함)                \r\n";
            SQL += "             , NVL((SELECT MOTHER FROM KOSMOS_OCS.EXAM_MASTER WHERE MASTERCODE = O.MASTERCODE),'0')       AS MOTHER       -- 38(마스터)                                                                    \r\n";
            SQL += "             , M.EQUCODE1                                                                      AS EQUCODE1     -- 39(마스터)                                                                    \r\n";
            SQL += "             , TO_CHAR(NVL(TRIM(M.PIECE),'0'))                                                 AS PIECE        -- 40(마스터)                                                                    \r\n";
            SQL += "             , DECODE(TRIM(M.GBTLA),'1', M.GBTLA, KOSMOS_OCS.FC_EXAM_SPECMST_NM('12', M.WSCODE1, 'W') ) 			   AS GBTLA        -- 41(마스터)                                                                   \r\n";
            SQL += "             , NVL(M.SERIES,0)                                                                 AS SERIES       -- 42(마스터)                                                                    \r\n";
            SQL += "  		     , CASE WHEN M.MASTERCODE IN ('BT022','BT023') THEN 'BT' ELSE '' END			   AS BL_PLT                                                                                        \r\n";
            SQL += "             , '0'                                                                             AS LV                                                                                                                                                                        \r\n";
            SQL += "  		     , M.MASTERCODE		                                                               AS MAIN_CODE                                                                                                                                                    \r\n";
            SQL += "  		     , M.MASTERCODE		                                                               AS SUB_CODE                                                                                                                                                     \r\n";
            SQL += "  		     , O.MASTERCODE		                                                               AS O_MASTERCODE                                                                                                                                                 \r\n";
            SQL += "  		 FROM KOSMOS_OCS.EXAM_MASTER       M                                                                                                                                                    \r\n";
            SQL += "  		    , KOSMOS_OCS.EXAM_ORDER 	   O                                                                                                                                                    \r\n";
            SQL += "  		WHERE 1 = 1                                                                                                                                                                             \r\n";
            SQL += "  		  AND O.PANO 		= " + ComFunc.covSqlstr(strPANO, false);
            SQL += "  		  AND O.BDATE 		= " + ComFunc.covSqlDate(strBDATE, "YYYY-MM-DD", false);
            SQL += "  		  AND O.IPDOPD 		= " + ComFunc.covSqlstr(strIPDOPD, false);
            SQL += "  		  AND O.DEPTCODE 	= " + ComFunc.covSqlstr(strDEPTCODE, false);

            if (string.IsNullOrEmpty(strExcepEXAM_CODE) == false)
            {
                SQL += "  		  AND O.MASTERCODE 	NOT IN (" + strDEPTCODE + ")    \r\n";
            }

            SQL += "  		  AND NVL(TRIM(O.CANCEL),'*') = '*'                                                                                                                                                     \r\n";
            SQL += "                                                                                                                                                                                                \r\n";
            SQL += "  		  AND M.MOTHER      != '1'                                                                                                                                                              \r\n";
            SQL += "  		  AND M.MASTERCODE   = O.MASTERCODE                                                                                                                                                     \r\n";
            SQL += "                                                                                                                                                                                                \r\n";
            SQL += "  		UNION                                                                                                                                                                                   \r\n";
            SQL += "  		SELECT                                                                                                                                                                                  \r\n";
            SQL += "  			   O.PANO                                                                          AS PANO                                                                                          \r\n";
            SQL += "  	  		 , O.BI                                                                            AS BI                                                                                            \r\n";
            SQL += "  	  		 , O.SNAME                                                                         AS SNAME                                                                                         \r\n";
            SQL += "  	  		 , O.IPDOPD                                                                        AS IPDOPD                                                                                        \r\n";
            SQL += "  	  		 , O.AGE                                                                           AS AGE                                                                                           \r\n";
            SQL += "  	  		 , O.AGEMM                                                                         AS AGEMM                                                                                         \r\n";
            SQL += "  	  		 , O.SEX                                                                           AS SEX                                                                                           \r\n";
            SQL += "  	  		 , O.DEPTCODE                                                                      AS DEPTCODE                                                                                      \r\n";
            SQL += "  	  		 , O.WARD                                                                          AS WARD                                                                                          \r\n";
            SQL += "  	  		 , O.DRCODE                                                                        AS DRCODE                                                                                        \r\n";
            SQL += "  	  		 , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(O.DRCODE)		                               AS DR_NM                                                                                         \r\n";
            SQL += "  	  		 , O.ROOM                                                                          AS ROOM                                                                                          \r\n";
            SQL += "  	  		 , TO_CHAR(O.BDATE,'YYYY-MM-DD')					                               AS BDATE                                                                                         \r\n";
            SQL += "  	  		 , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_응급실검사', M.MASTERCODE)               AS CHKGWA                                                                                        \r\n";
            SQL += "  	  		 , O.STRT                                                                          AS STRT                                                                                        \r\n";
            SQL += "  	  		 , O.SPECCODE										                               AS DR_SPECCODE                                                                                   \r\n";
            SQL += "   	  		 , M.SPECCODE																	   AS SPECCODE                                                                                      \r\n";
            SQL += "  		     , M.MASTERCODE																	   AS MASTERCODE                                                                                    \r\n";
            SQL += "  		     , O.DRCOMMENT																	   AS DRCOMMENT                                                                                     \r\n";
            SQL += "  		     , O.ROWID                                      								   AS ROW_ID                                                                                        \r\n";
            SQL += "  		     , TO_CHAR(O.RDATE,'YYYY-MM-DD')												   AS RDATE                                                                                         \r\n";
            SQL += "  		     , TO_CHAR(O.SDATE,'YYYY-MM-DD')												   AS SDATE                                                                                         \r\n";
            SQL += "     		 , TO_CHAR(O.ORDERDATE,'YYYY-MM-DD')											   AS ORDERDATE                                                                                     \r\n";
            SQL += "     		 , TO_CHAR(O.SENDDATE,'YYYY-MM-DD')												   AS SENDDATE                                                                                      \r\n";
            SQL += "     		 , TO_CHAR(ORDERNO)																   AS ORDERNO                                                                                       \r\n";
            SQL += "  			 , KOSMOS_OCS.FC_IPD_NEW_MASTER_GBSTS('', O.PANO)								   AS CHKIO                                                                                         \r\n";
            SQL += "  			 , M.EXAMNAME																	   AS EXAMNAME                                                                                      \r\n";
            SQL += "             , M.TUBECODE                                                                      AS TUBECODE     -- 31(마스터)                                                                    \r\n";
            SQL += "             , KOSMOS_OCS.FC_EXAM_SPECMST_NM('15', M.TUBECODE, 'Y')                            AS TUBENAME     -- 32(마스터)                                                                    \r\n";
            SQL += "             , M.UNITCODE																	   AS UNITCODE     -- 33(마스터)                                                                    \r\n";
            SQL += "             , KOSMOS_OCS.FC_WSGRP(M.WSCODE1)                                                  AS WSGRP        -- 34(마스터)                                                                    \r\n";
            SQL += "             , M.WSCODE1                                                                       AS WSCODE1      -- 35(마스터)                                                                    \r\n";
            SQL += "             , TO_CHAR(M.WSCODE1POS, '00000')                                                  AS WSCODE1POS   -- 36(마스터)                                                                    \r\n";
            SQL += "             , KOSMOS_OCS.FC_EXAM_SPECMST_NM('12', M.WSCODE1, 'W')                             AS WSGRPNM      --                                                                               \r\n";
            SQL += "             , KOSMOS_OCS.FC_EXAM_SPECMST_NM('12', M.WSCODE1, 'Y')                             AS WSNM         -- 11                                                                            \r\n";
            SQL += "             , M.RESULTIN                                                                      AS RESULTIN     -- 37(마스터)     결과입력 유무(1:결과 입력 안함, 0:결과 입력 함)                \r\n";
            SQL += "             , NVL((SELECT MOTHER FROM KOSMOS_OCS.EXAM_MASTER WHERE MASTERCODE = O.MASTERCODE),'0')       AS MOTHER       -- 38(마스터)                                                                    \r\n";
            SQL += "             , M.EQUCODE1                                                                      AS EQUCODE1     -- 39(마스터)                                                                    \r\n";
            SQL += "             , TO_CHAR(NVL(TRIM(M.PIECE),'0'))                                                 AS PIECE        -- 40(마스터)                                                                    \r\n";
            SQL += "             , DECODE(TRIM(M.GBTLA),'1', M.GBTLA, KOSMOS_OCS.FC_EXAM_SPECMST_NM('12', M.WSCODE1, 'W') ) 			   AS GBTLA        -- 41(마스터)                                                                   \r\n";
            SQL += "             , NVL(M.SERIES,0)                                                                 AS SERIES       -- 42(마스터)                                                                    \r\n";
            SQL += "  		     , CASE WHEN M.MASTERCODE IN ('BT022','BT023') THEN 'BT' ELSE '' END			   AS BL_PLT                                                                                        \r\n";
            SQL += "             , '1' AS LV                                                                                                                                                                        \r\n";
            SQL += "  		     , (CASE WHEN NVL((SELECT MOTHER FROM KOSMOS_OCS.EXAM_MASTER WHERE MASTERCODE = O.MASTERCODE),'0') = '1' THEN M.MASTERCODE                                                          \r\n";
            SQL += "  		             ELSE S.MASTERCODE END)													   AS MAIN_CODE                                                                                     \r\n"; 
            SQL += "  	         , M.MASTERCODE 	                                                               AS SUB_CODE                                                                                      \r\n";
            SQL += "  		     , O.MASTERCODE		                                                               AS O_MASTERCODE                                                                                  \r\n";
            SQL += "  		 FROM KOSMOS_OCS.EXAM_MASTER       M                                                                                                                                                    \r\n";
            SQL += "  		    , KOSMOS_OCS.EXAM_MASTER_SUB   S                                                                                                                                                    \r\n";
            SQL += "  		    , KOSMOS_OCS.EXAM_ORDER 	   O                                                                                                                                                    \r\n";
            SQL += "  		WHERE 1 = 1                                                                                                                                                                             \r\n";
            SQL += "  		  AND O.PANO 		= " + ComFunc.covSqlstr(strPANO     , false);
            SQL += "  		  AND O.BDATE 		= " + ComFunc.covSqlDate(strBDATE    , "YYYY-MM-DD", false);
            SQL += "  		  AND O.IPDOPD 		= " + ComFunc.covSqlstr(strIPDOPD       , false);
            SQL += "  		  AND O.DEPTCODE 	= " + ComFunc.covSqlstr(strDEPTCODE     , false);

            if (string.IsNullOrEmpty(strExcepEXAM_CODE) == false)
            {
                SQL += "  		  AND O.MASTERCODE 	NOT IN (" + strDEPTCODE + ")    \r\n";
            }

            SQL += "  		  AND NVL(TRIM(O.CANCEL),'*') = '*'                                                                                                                                                     \r\n";
            SQL += "  		  AND S.MASTERCODE  = O.MASTERCODE                                                                                                                                                      \r\n";
            SQL += "  		  AND S.NORMAL 		= M.MASTERCODE                                                                                                                                                      \r\n";
            SQL += "  		  AND S.GUBUN 		= '31'                                                                                                                                                              \r\n";
            SQL += "  		UNION                                                                                                                                                                                   \r\n";
            SQL += "  		SELECT                                                                                                                                                                                  \r\n";
            SQL += "  			   O.PANO                                                                          AS PANO                                                                                          \r\n";
            SQL += "  	  		 , O.BI                                                                            AS BI                                                                                            \r\n";
            SQL += "  	  		 , O.SNAME                                                                         AS SNAME                                                                                         \r\n";
            SQL += "  	  		 , O.IPDOPD                                                                        AS IPDOPD                                                                                        \r\n";
            SQL += "  	  		 , O.AGE                                                                           AS AGE                                                                                           \r\n";
            SQL += "  	  		 , O.AGEMM                                                                         AS AGEMM                                                                                         \r\n";
            SQL += "  	  		 , O.SEX                                                                           AS SEX                                                                                           \r\n";
            SQL += "  	  		 , O.DEPTCODE                                                                      AS DEPTCODE                                                                                      \r\n";
            SQL += "  	  		 , O.WARD                                                                          AS WARD                                                                                          \r\n";
            SQL += "  	  		 , O.DRCODE                                                                        AS DRCODE                                                                                        \r\n";
            SQL += "  	  		 , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(O.DRCODE)		                               AS DR_NM                                                                                         \r\n";
            SQL += "  	  		 , O.ROOM                                                                          AS ROOM                                                                                          \r\n";
            SQL += "  	  		 , TO_CHAR(O.BDATE,'YYYY-MM-DD')					                               AS BDATE                                                                                         \r\n";
            SQL += "  	  		 , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_응급실검사', M.MASTERCODE)               AS CHKGWA                                                                                        \r\n";
            SQL += "  	  		 , O.STRT                                                                          AS STRT                                                                                        \r\n";
            SQL += "  	  		 , O.SPECCODE										                               AS DR_SPECCODE                                                                                   \r\n";
            SQL += "   	  		 , M.SPECCODE																	   AS SPECCODE                                                                                      \r\n";
            SQL += "  		     , M.MASTERCODE																	   AS MASTERCODE                                                                                    \r\n";
            SQL += "  		     , O.DRCOMMENT																	   AS DRCOMMENT                                                                                     \r\n";
            SQL += "  		     , O.ROWID                                      								   AS ROW_ID                                                                                        \r\n";
            SQL += "  		     , TO_CHAR(O.RDATE,'YYYY-MM-DD')												   AS RDATE                                                                                         \r\n";
            SQL += "  		     , TO_CHAR(O.SDATE,'YYYY-MM-DD')												   AS SDATE                                                                                         \r\n";
            SQL += "     		 , TO_CHAR(O.ORDERDATE,'YYYY-MM-DD')											   AS ORDERDATE                                                                                     \r\n";
            SQL += "     		 , TO_CHAR(O.SENDDATE,'YYYY-MM-DD')												   AS SENDDATE                                                                                      \r\n";
            SQL += "     		 , TO_CHAR(ORDERNO)																   AS ORDERNO                                                                                       \r\n";
            SQL += "  			 , KOSMOS_OCS.FC_IPD_NEW_MASTER_GBSTS('', O.PANO)								   AS CHKIO                                                                                         \r\n";
            SQL += "  			 , M.EXAMNAME																	   AS EXAMNAME                                                                                      \r\n";
            SQL += "             , M.TUBECODE                                                                      AS TUBECODE     -- 31(마스터)                                                                    \r\n";
            SQL += "             , KOSMOS_OCS.FC_EXAM_SPECMST_NM('15', M.TUBECODE, 'Y')                            AS TUBENAME     -- 32(마스터)                                                                    \r\n";
            SQL += "             , M.UNITCODE																	   AS UNITCODE     -- 33(마스터)                                                                    \r\n";
            SQL += "             , KOSMOS_OCS.FC_WSGRP(M.WSCODE1)                                                  AS WSGRP        -- 34(마스터)                                                                    \r\n";
            SQL += "             , M.WSCODE1                                                                       AS WSCODE1      -- 35(마스터)                                                                    \r\n";
            SQL += "             , TO_CHAR(M.WSCODE1POS, '00000')                                                  AS WSCODE1POS   -- 36(마스터)                                                                    \r\n";
            SQL += "             , KOSMOS_OCS.FC_EXAM_SPECMST_NM('12', M.WSCODE1, 'W')                             AS WSGRPNM      --                                                                               \r\n";
            SQL += "             , KOSMOS_OCS.FC_EXAM_SPECMST_NM('12', M.WSCODE1, 'Y')                             AS WSNM         -- 11                                                                            \r\n";
            SQL += "             , M.RESULTIN                                                                      AS RESULTIN     -- 37(마스터)     결과입력 유무(1:결과 입력 안함, 0:결과 입력 함)                \r\n";
            SQL += "             , NVL((SELECT MOTHER FROM KOSMOS_OCS.EXAM_MASTER WHERE MASTERCODE = O.MASTERCODE),'0')       AS MOTHER       -- 38(마스터)                                                                    \r\n";
            SQL += "             , M.EQUCODE1                                                                      AS EQUCODE1     -- 39(마스터)                                                                    \r\n";
            SQL += "             , TO_CHAR(NVL(TRIM(M.PIECE),'0'))                                                 AS PIECE        -- 40(마스터)                                                                    \r\n";
            SQL += "             , DECODE(TRIM(M.GBTLA),'1', M.GBTLA, KOSMOS_OCS.FC_EXAM_SPECMST_NM('12', M.WSCODE1, 'W') ) 			   AS GBTLA        -- 41(마스터)                                                                   \r\n";
            SQL += "             , NVL(M.SERIES,0)                                                                 AS SERIES       -- 42(마스터)                                                                    \r\n";
            SQL += "  		     , CASE WHEN M.MASTERCODE IN ('BT022','BT023') THEN 'BT' ELSE '' END			   AS BL_PLT                                                                                        \r\n";
            SQL += "  		     , '2' AS LV                                                                                                                                                                        \r\n";
            SQL += "  		     , S.MASTERCODE	    AS MAIN_CODE                                                                                                                                                    \r\n";
            SQL += "  		     , M.MASTERCODE 	AS SUB_CODE                                                                                                                                                     \r\n";
            SQL += "  		     , O.MASTERCODE		AS O_MASTERCODE                                                                                                                                                 \r\n";
            SQL += "  		 FROM KOSMOS_OCS.EXAM_MASTER       M                                                                                                                                                    \r\n";
            SQL += "  		    , KOSMOS_OCS.EXAM_MASTER_SUB   S                                                                                                                                                    \r\n";
            SQL += "  		    , KOSMOS_OCS.EXAM_ORDER 	   O                                                                                                                                                    \r\n";
            SQL += "  		WHERE 1 = 1                                                                                                                                                                             \r\n";
            SQL += "  		  AND O.PANO 		= " + ComFunc.covSqlstr(strPANO, false);
            SQL += "  		  AND O.BDATE 		= " + ComFunc.covSqlDate(strBDATE, "YYYY-MM-DD", false);
            SQL += "  		  AND O.IPDOPD 		= " + ComFunc.covSqlstr(strIPDOPD, false);
            SQL += "  		  AND O.DEPTCODE 	= " + ComFunc.covSqlstr(strDEPTCODE, false);

            if (string.IsNullOrEmpty(strExcepEXAM_CODE) == false)
            {
                SQL += "  		  AND O.MASTERCODE 	NOT IN (" + strDEPTCODE + ")    \r\n";
            }

            SQL += "  		  AND (S.MASTERCODE, O.ROWID) 	IN (                                                                                                                                                    \r\n";
            SQL += "  							                SELECT S.NORMAL, O1.ROWID                                                                                                                           \r\n";
            SQL += "  								              FROM KOSMOS_OCS.EXAM_MASTER       M                                                                                                               \r\n";
            SQL += "  								                 , KOSMOS_OCS.EXAM_MASTER_SUB   S                                                                                                               \r\n";
            SQL += "  					   		                     , KOSMOS_OCS.EXAM_ORDER 	   O1                                                                                                               \r\n";
            SQL += "  								             WHERE 1 = 1                                                                                                                                        \r\n";
            SQL += "  		                                       AND O1.PANO 		= " + ComFunc.covSqlstr(strPANO, false);
            SQL += "  		                                       AND O1.BDATE 	= " + ComFunc.covSqlDate(strBDATE, "YYYY-MM-DD", false);
            SQL += "  		                                       AND O1.IPDOPD 	= " + ComFunc.covSqlstr(strIPDOPD, false);
            SQL += "  		                                       AND O1.DEPTCODE 	= " + ComFunc.covSqlstr(strDEPTCODE, false);

            if (string.IsNullOrEmpty(strExcepEXAM_CODE) == false)
            {
                SQL += "  		                        AND O1.MASTERCODE 	NOT IN (" + strDEPTCODE + ")    \r\n";
            }

            SQL += "                                                                                                                                                                                                \r\n";
            SQL += "  								  AND S.MASTERCODE 	= O1.MASTERCODE                                                                                                                             \r\n";
            SQL += "                                                                                                                                                                                                \r\n";
            SQL += "  								  AND S.NORMAL 		= M.MASTERCODE                                                                                                                              \r\n";
            SQL += "  								  AND S.GUBUN 		= '31'                                                                                                                                      \r\n";
            SQL += "  								)                                                                                                                                                               \r\n";
            SQL += "  		  AND S.NORMAL 		= M.MASTERCODE                                                                                                                                                      \r\n";
            SQL += "  		  AND S.GUBUN 		= '31'                                                                                                                                                              \r\n";
            SQL += "  	    ORDER BY MOTHER,PIECE,O_MASTERCODE,SUB_CODE,SPECCODE, CHKGWA,ORDERNO                                                                                                                    \r\n";
            SQL += "  )                                                                                                                                                                                             \r\n";
            SQL += "  SELECT                                                                                                                                                                                        \r\n";
            SQL += "  	     PIECE                                                                                                                                                                                  \r\n";
            SQL += "  	   , DECODE(MOTHER,'1', ROW_ID,'') || DECODE(PIECE,'1', TO_CHAR(ROWNUM),'0') || DECODE(TRIM(CHKGWA),'','N','Y') || BL_PLT || T.SPECCODE || GBTLA || TUBECODE        AS SPILITE              \r\n";            
            SQL += "  	   , DECODE(TRIM(CHKGWA),'','N','Y')                                                                                                                                AS CHKGWA               \r\n";            
            SQL += "  	   , BL_PLT                                                                                                                                                         AS BL_PLT               \r\n";
            SQL += "       , DECODE (NVL(TRIM(DR_SPECCODE),'*'), '*', DECODE(NVL(TRIM(CHKGWA),'*'), '*', SPECCODE      , '080'), DECODE(NVL(TRIM(CHKGWA),'*'), '*', DR_SPECCODE   , '080')) AS SPECODE              \r\n";
            SQL += "  	   , GBTLA                                                                                                                                                                                  \r\n";
            SQL += "  	   , DECODE(NVL(TRIM(CHKGWA),'*'), '*', TUBECODE, '110')    AS TUBECODE       -- 기타                                                                                                       \r\n";
            SQL += "  	   , O_MASTERCODE 												AS O_MASTERCODE                                                                                                             \r\n";
            SQL += "  	   , MAIN_CODE 												AS MASTERCODE                                                                                                                   \r\n";
            SQL += "  	   , SUB_CODE					  							AS SUBCODE                                                                                                                      \r\n";
            SQL += "  	   , PANO                                                                                                                                                                                   \r\n";
            SQL += "  	   , BI                                                                                                                                                                                     \r\n";
            SQL += "  	   , SNAME                                                                                                                                                                                  \r\n";
            SQL += "  	   , IPDOPD                                                                                                                                                                                 \r\n";
            SQL += "  	   , AGE                                                                                                                                                                                    \r\n";
            SQL += "  	   , AGEMM                                                                                                                                                                                  \r\n";
            SQL += "  	   , SEX                                                                                                                                                                                    \r\n";
            SQL += "  	   , DEPTCODE                                                                                                                                                                               \r\n";
            SQL += "  	   , WARD                                                                                                                                                                                   \r\n";
            SQL += "  	   , DRCODE                                                                                                                                                                                 \r\n";
            SQL += "  	   , DR_NM                                                                                                                                                                                  \r\n";
            SQL += "  	   , ROOM                                                                                                                                                                                   \r\n";
            SQL += "  	   , ''                                                   AS SPECNO                                                                                                                         \r\n";
            SQL += "  	   , BDATE                                                                                                                                                                                  \r\n";
            SQL += "  	   , '1'                                                  AS QTY                                                                                                                            \r\n";
            SQL += "  	   , STRT                                                                                                                                                                                   \r\n";
            SQL += "  	   , DRCOMMENT                                                                                                                                                                              \r\n";
            SQL += "  	   , ROW_ID                                                                                                                                                                                 \r\n";
            SQL += "  	   , RDATE                                                                                                                                                                                  \r\n";
            SQL += "  	   , SDATE                                                                                                                                                                                  \r\n";
            SQL += "  	   , ORDERDATE                                                                                                                                                                              \r\n";
            SQL += "  	   , SENDDATE                                                                                                                                                                               \r\n";
            SQL += "  	   , ORDERNO                                                                                                                                                                                \r\n";
            SQL += "  	   , CHKIO                                                                                                                                                                                  \r\n";
            SQL += "  	   , EXAMNAME                                                                                                                                                                               \r\n";
            SQL += "  	   , TUBENAME                                                                                                                                                                               \r\n";
            SQL += "  	   , UNITCODE                                                                                                                                                                               \r\n";
            SQL += "  	   , KOSMOS_OCS.FC_EXAM_SPECMST_NM('20', UNITCODE, 'N') AS UNITNM                                                                                                                           \r\n";
            SQL += "  	   , WSCODE1                                                                                                                                                                                \r\n";
            SQL += "  	   , WSCODE1POS                                                                                                                                                                             \r\n";
            SQL += "       , WSGRP         -- TLA와 WSGRPM 같이 있음.                                                                                                                                               \r\n";
            SQL += "  	   , WSGRPNM                                                                                                                                                                                \r\n";
            SQL += "  	   , WSNM                                                                                                                                                                                   \r\n";
            SQL += "  	   , RESULTIN                                                                                                                                                                               \r\n";
            SQL += "  	   , (SELECT EQUCODE1 FROM KOSMOS_OCS.EXAM_MASTER WHERE MASTERCODE = MAIN_CODE) AS EQUCODE1                                                                                                                                                                                \r\n";
            SQL += "  	   , DECODE(SERIES,0,'','Y')                             AS SERIES                                                                                                                          \r\n";
            SQL += "  	   , DR_SPECCODE										 AS DR_SPECCODE                                                                                                                     \r\n";
            SQL += "  	   , SPECCODE											 AS SPEC_SPECCODE                                                                                                                   \r\n";
            //2019-04-29 안정수, JOB 칼럼 MaxLength 오류로 인한 조건 추가 
            if(strJOB == "")
            {
                SQL += "  	   , '                                                  'AS JOB                                                                                                                             \r\n";
            }
            else
            {
                SQL += "  	   , '" + strJOB + "'       							 AS JOB                                                                                                                             \r\n";
            }
            SQL += "  	   , '" + strJOB + "'       							 AS JOB                                                                                                                             \r\n";
            SQL += "  	   , ''                                                  AS SPEC_DR_COMMENTS                                                                                                                \r\n";
            SQL += "  	   , ''                                                  AS SPEC_WS_NM                                                                                                                      \r\n";
            SQL += "  	   , ''                                                  AS SPEC_RECEIVEDATE                                                                                                                \r\n";
            SQL += "  	   , ''                                                  AS SPEC_STATUS                                                                                                                     \r\n";
            SQL += "  	   , ''                                                  AS BLOODDATE                                                                                                                       \r\n";
            SQL += "  	   , ''                                                  AS RSLT_SEQNO                                                                                                                      \r\n";
            SQL += "    FROM T                                                                                                                                                                                      \r\n";
            SQL += "  ORDER BY DECODE(MOTHER,'1', ROW_ID,'') || DECODE(PIECE,'1', TO_CHAR(ROWNUM),'0') || DECODE(TRIM(CHKGWA),'','N','Y') || BL_PLT || T.SPECCODE || GBTLA || TUBECODE , MASTERCODE,SUBCODE         \r\n";


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
       
        string setSPECNO(DataTable dt, PsmhDb pDbCon)
        {
            string SqlErr = string.Empty;
            string SQL = string.Empty;

            string SPECNO   = "";
            string strSEQNO = "";
            string SPILITE_TMP = "";
            string SPILITE = "";
            int intRowAffected = 0;

            int nSEQNO = 0;
            string strSPEC_WS_NM = "";
            string strSPEC_DR_COMMENTS = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nSEQNO += 1;
                strSEQNO         = string.Format("{0:000}", nSEQNO);
                dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.RSLT_SEQNO] = strSEQNO;
                SPILITE         = dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SPILITE].ToString();


                if (string.IsNullOrEmpty(dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SPEC_DR_COMMENTS].ToString()) == false)
                {
                    strSPEC_DR_COMMENTS += dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SUBCODE].ToString().Trim() + ":" + dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.DRCOMMENT].ToString() + ",";
                }

                if (i == 0)
                {
                    if (strSPEC_WS_NM.IndexOf(dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.WSNM].ToString().Trim()) < 0 || string.IsNullOrEmpty(strSPEC_WS_NM.Trim()) == true)
                    {
                        strSPEC_WS_NM += dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.WSNM].ToString().Trim() + ",";
                    }


                    SPECNO = lbExSQL.sel_SpecNO(pDbCon);
                    dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SPECNO] = SPECNO;
                    SqlErr = ins_EXAM_REULTC(pDbCon, dt.Rows[i], ref intRowAffected);
                    if (SqlErr != "") return SqlErr;

                    SqlErr = lbExSQL.up_EXAM_ORDER(pDbCon, dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SPECNO].ToString().Trim(), dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.ROW_ID].ToString(), ref intRowAffected);
                    if (SqlErr != "") return SqlErr;

                    SPILITE_TMP = SPILITE;
                }
                else if (i == dt.Rows.Count - 1)
                {
                    // 마지막 로우
                    //8. 1111
                    //9. 1111  

                    if (SPILITE == SPILITE_TMP)
                    {
                        if (strSPEC_WS_NM.IndexOf(dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.WSNM].ToString().Trim()) < 0 || string.IsNullOrEmpty(strSPEC_WS_NM.Trim()) == true)
                        {
                            strSPEC_WS_NM += dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.WSNM].ToString().Trim() + ",";
                        }

                        dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SPECNO] = SPECNO;
                        SqlErr = ins_EXAM_REULTC(pDbCon, dt.Rows[i], ref intRowAffected);
                        if (SqlErr != "") return SqlErr;

                        SqlErr = lbExSQL.up_EXAM_ORDER(pDbCon, dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SPECNO].ToString().Trim(), dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.ROW_ID].ToString(), ref intRowAffected);
                        if (SqlErr != "") return SqlErr;


                        dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SPECNO] = SPECNO;
                        SqlErr = SaveMaster(pDbCon, dt.Rows[i], ref strSPEC_WS_NM, ref strSPEC_DR_COMMENTS);
                        if (SqlErr != "") return SqlErr;
                    }
                    else
                    {
                        dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SPECNO] = SPECNO;
                        SqlErr = SaveMaster(pDbCon, dt.Rows[i - 1], ref strSPEC_WS_NM, ref strSPEC_DR_COMMENTS);
                        if (SqlErr != "") return SqlErr;

                        if (strSPEC_WS_NM.IndexOf(dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.WSNM].ToString().Trim()) < 0 || string.IsNullOrEmpty(strSPEC_WS_NM.Trim()) == true)
                        {
                            strSPEC_WS_NM += dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.WSNM].ToString().Trim() + ",";
                        }

                        SPILITE_TMP = SPILITE;
                        nSEQNO = 1;

                        strSEQNO = string.Format("{0:000}", nSEQNO);
                        dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.RSLT_SEQNO] = strSEQNO;

                        SPECNO = lbExSQL.sel_SpecNO(pDbCon);
                        dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SPECNO] = SPECNO;

                        SqlErr = ins_EXAM_REULTC(pDbCon, dt.Rows[i], ref intRowAffected);
                        if (SqlErr != "") return SqlErr;

                        SqlErr = lbExSQL.up_EXAM_ORDER(pDbCon, dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SPECNO].ToString().Trim(), dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.ROW_ID].ToString(), ref intRowAffected);
                        if (SqlErr != "") return SqlErr;

                        strSPEC_WS_NM = dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.WSNM].ToString().Trim();
                        SqlErr = SaveMaster(pDbCon, dt.Rows[i], ref strSPEC_WS_NM, ref strSPEC_DR_COMMENTS);
                        if (SqlErr != "") return SqlErr;
                    }
                }
                else
                {
                    if (SPILITE == SPILITE_TMP)
                    {
                        if (strSPEC_WS_NM.IndexOf(dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.WSNM].ToString().Trim()) < 0 || string.IsNullOrEmpty(strSPEC_WS_NM.Trim()) == true)
                        {
                            strSPEC_WS_NM += dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.WSNM].ToString().Trim() + ",";
                        }

                        dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SPECNO] = SPECNO;
                        SqlErr = ins_EXAM_REULTC(pDbCon, dt.Rows[i], ref intRowAffected);
                        if (SqlErr != "") return SqlErr;

                        SqlErr = lbExSQL.up_EXAM_ORDER(pDbCon, dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SPECNO].ToString().Trim(), dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.ROW_ID].ToString(), ref intRowAffected);
                        if (SqlErr != "") return SqlErr;

                    }
                    else
                    {
                        dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SPECNO] = SPECNO;
                        SqlErr = SaveMaster(pDbCon, dt.Rows[i - 1], ref strSPEC_WS_NM, ref strSPEC_DR_COMMENTS);
                        if (SqlErr != "") return SqlErr;


                        if (strSPEC_WS_NM.IndexOf(dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.WSNM].ToString().Trim()) < 0 || string.IsNullOrEmpty(strSPEC_WS_NM.Trim()) == true)
                        {
                            strSPEC_WS_NM += dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.WSNM].ToString().Trim() + ",";
                        }


                        SPECNO = lbExSQL.sel_SpecNO(pDbCon);
                        dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SPECNO] = SPECNO;

                        SPILITE_TMP = SPILITE;
                        nSEQNO = 1;

                        strSEQNO = string.Format("{0:000}", nSEQNO);
                        dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.RSLT_SEQNO] = strSEQNO;

                        SqlErr = ins_EXAM_REULTC(pDbCon, dt.Rows[i], ref intRowAffected);
                        if (SqlErr != "") return SqlErr;

                        SqlErr = lbExSQL.up_EXAM_ORDER(pDbCon, dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.SPECNO].ToString().Trim(), dt.Rows[i][(int)enmSel_EXAM_ORDER_Detail.ROW_ID].ToString(), ref intRowAffected);
                        if (SqlErr != "") return SqlErr;

                    }
                }
            
                
        }

            return SqlErr;
        }

        string SaveMaster(PsmhDb pDbCon, DataRow dr, ref string strSPEC_WS_NM, ref string strSPEC_DR_COMMENTS)
        {
            string SqlErr       = "";

            //string SPECNO       = "";
            string STATUS       = "";
            string RECEIVEDATE  = "";
            string BLOODDATE    = "";

            int intRowAffected = 0;


            if (strSPEC_WS_NM.Length > 1)
            {
                strSPEC_WS_NM = strSPEC_WS_NM.Substring(0, strSPEC_WS_NM.Length - 1);
                dr[(int)enmSel_EXAM_ORDER_Detail.SPEC_WS_NM] = strSPEC_WS_NM;
            }
            else
            {
                dr[(int)enmSel_EXAM_ORDER_Detail.SPEC_WS_NM] = strSPEC_WS_NM;
            }

            strSPEC_WS_NM = "";

            dr[(int)enmSel_EXAM_ORDER_Detail.SPEC_DR_COMMENTS] = strSPEC_DR_COMMENTS;

            strSPEC_DR_COMMENTS = "";

            setSTATUS(dr, ref STATUS, ref RECEIVEDATE, ref BLOODDATE);

            dr[(int)enmSel_EXAM_ORDER_Detail.SPEC_STATUS]       = STATUS;
            dr[(int)enmSel_EXAM_ORDER_Detail.SPEC_RECEIVEDATE]  = RECEIVEDATE;
            dr[(int)enmSel_EXAM_ORDER_Detail.SPEC_BLOODDATE]    = BLOODDATE;


            SqlErr = ins_EXAM_SPECMST(pDbCon, dr, ref intRowAffected);
            if (SqlErr != "") return SqlErr;



            return SqlErr;
        }

        void setSTATUS (DataRow dr, ref string STATUS, ref string RECEIVEDATE, ref string BLOODDATE)
        {
            DateTime sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-"));
            sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + " " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", ":"));
            string strSYSDATE = sysdate.ToString("yyyy-MM-dd HH:mm");

            BLOODDATE = strSYSDATE;

            string CHKGWA       = dr[(int)enmSel_EXAM_ORDER_Detail.CHKGWA].ToString().Trim();
            string strJOB       = dr[(int)enmSel_EXAM_ORDER_Detail.JOB].ToString().Trim();
            string strSPECODE   = dr[(int)enmSel_EXAM_ORDER_Detail.SPECODE].ToString().Trim();
            string strOPDIPD    = dr[(int)enmSel_EXAM_ORDER_Detail.IPDOPD].ToString().Trim();
            string strWORKSTS   = dr[(int)enmSel_EXAM_ORDER_Detail.SPEC_WS_NM].ToString().Trim();

            // 과검사 자동 접수
            if (CHKGWA.Equals("Y") == true || strJOB.Equals("MENUAL") == true)
            {
                RECEIVEDATE = strSYSDATE;
                STATUS = "01";
            }
            else
            {
                RECEIVEDATE = "";
                STATUS = "00";

                if (strOPDIPD.Equals("O") && strWORKSTS == "M")
                {
                    if (ComFunc.MsgBoxQ("미생물검사입니다. 접수를 하겠습니까?", "예:접수 아니오:미접수") == System.Windows.Forms.DialogResult.Yes)
                    {
                        RECEIVEDATE = strSYSDATE;
                        STATUS = "01";
                    }
                }
                else if (strOPDIPD.Equals("O") && strWORKSTS == "P,M")
                {
                    if (ComFunc.MsgBoxQ("대변 검사입니다. 접수를 하겠습니까?", "예:접수 아니오:미접수") == System.Windows.Forms.DialogResult.Yes)
                    {
                        RECEIVEDATE = strSYSDATE;
                        STATUS = "01";
                    }
                }
                else if (strOPDIPD.Equals("O") && strWORKSTS == "P")
                {
                    if (ComFunc.MsgBoxQ("대변 검사입니다. 접수를 하겠습니까?", "예:접수 아니오:미접수") == System.Windows.Forms.DialogResult.Yes)
                    {
                        RECEIVEDATE = strSYSDATE;
                        STATUS = "01";
                    }

                }
                else if (strOPDIPD.Equals("O") && strSPECODE == "084")
                {
                    if (ComFunc.MsgBoxQ("대변 검사입니다. 접수를 하겠습니까?", "예:접수 아니오:미접수") == System.Windows.Forms.DialogResult.Yes)
                    {
                        RECEIVEDATE = strSYSDATE;
                        STATUS = "01";
                    }
                }
                else if (strOPDIPD.Equals("O"))
                {
                    RECEIVEDATE = "";
                    STATUS = "00";

                }
                else if (strOPDIPD.Equals("I"))
                {
                    RECEIVEDATE = "";
                    STATUS = "00";
                }
            }
        }

        /// <summary>바코드 출력
        /// </summary>
        /// <param name="barCodeParamng"></param>
        /// <returns></returns>
        public bool printQCSpecimenBarCode(PsmhDb pDbCon, string strJOBDATE, string strCODE, string strLOTNO, string strNOR, enmPrintType printType = enmPrintType.COM_PORT)
        {

            bool b = true;

            DataTable dtSpecNo = null;
            dtSpecNo = lbExQcSQL.sel_EXAM_JUNMST_BARCODE(pDbCon, strJOBDATE, strCODE,strLOTNO,strNOR);


            if (ComFunc.isDataTableNull(dtSpecNo) == true)
            {
                ComFunc.MsgBox("검체번호에 맞는 내용이 존재 하지 않습니다.", "검체번호 오류");
                return false;
            }

            string strPrintName = ClsPrint.getLabBarCodePrinter(BARCODE_PRINTER_NAME);

            if (ClsPrint.isLabBarCodePrinter(strPrintName) == false)
            {
                ComFunc.MsgBox("설정된 바코드 프린터기가 없습니다.", "검체번호 오류");
                return false;
            }

            nBCodeName = 1;

            if (printType == enmPrintType.COM_PORT)
            {
                string s;
                s = setBarCodeQCSpecimen(dtSpecNo);

                if (s.Length > 10)
                {
                    for (int i = 0; i < nBCodeName; i++)
                    {
                        ComPrintApi.SendStringToPrinter(strPrintName, s);
                    }
                }
                else
                {
                    b = false;
                }
            }

            return b;
        }
     
        //bool isPrintOffLine(string strPrintName)
        //{
        //    bool b = false;

        //    this.printServer = new PrintServer();

        //    printQueues = this.printServer.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections });
        //     printerDescriptions = new List<PrinterDescription>();

        //    foreach (PrintQueue printQueue in printQueues)
        //    {
        //        // The OneNote printer driver causes crashes in 64bit OSes so for now just don't include it.
        //        // Also redirected printer drivers cause crashes for some printers. Another WPF issue that cannot be worked around.
        //        if (printQueue.Name.ToUpperInvariant().Contains("ONENOTE") || printQueue.Name.ToUpperInvariant().Contains("REDIRECTED"))
        //        {
        //            continue;
        //        }

        //        string status = printQueue.QueueStatus.ToString();

        //        try
        //        {
        //            PrinterDescription printerDescription = new PrinterDescription()
        //            {
        //                Name = printQueue.Name,
        //                FullName = printQueue.FullName,
        //                Status = status == Strings.Printing_PrinterStatus_NoneTxt ? Strings.Printing_PrinterStatus_ReadyTxt : status,
        //                ClientPrintSchemaVersion = printQueue.ClientPrintSchemaVersion,
        //                DefaultPrintTicket = printQueue.DefaultPrintTicket,
        //                PrintCapabilities = printQueue.GetPrintCapabilities(),
        //                PrintQueue = printQueue
        //            };

        //            printerDescriptions.Add(printerDescription);
        //        }
        //        catch (PrintQueueException ex)
        //        {
        //            // ... Logging removed
        //        }
        //    }



        //    return b;
        //}

        /// <summary>바코드 출력
        /// </summary>
        /// <param name="barCodeParamng"></param>
        /// <returns></returns>
        public bool printSpecimenBarCode(PsmhDb pDbCon, string specNo, enmPrintType printType = enmPrintType.COM_PORT, string strCOMPORT = "COM1" , string strWARDGU = "")
        {

            bool b = false;
            string strPrintName = "";
            DataTable dtSpecNo = null;

            dtSpecNo = lbExSQL.sel_EXAM_SPECMST_BARCODE(pDbCon, specNo);
            

            if (ComFunc.isDataTableNull(dtSpecNo) == true)
            {
                ComFunc.MsgBox("검체번호에 맞는 내용이 존재 하지 않습니다.", "검체번호 오류");
                return false;
            }
            if(strWARDGU == "85")
            {
                strPrintName = ClsPrint.getLabBarCodePrinter("혈액환자정보2");
            }
            else
            {
                strPrintName = ClsPrint.getLabBarCodePrinter(BARCODE_PRINTER_NAME);
            }
            

            if (ClsPrint.isLabBarCodePrinter(strPrintName) == false)
            {
                return false;
            }
            
            if (isPrinterOffLine(strPrintName) ==true)
            {
                ComFunc.MsgBox("검체 바코드 프린터가 오프 라인 상태 입니다. 전원 및 라인을 확인 하세요.", "프린터 오류");
                return false;
            }

            if (printType == enmPrintType.COM_PORT)
            {
                try
                {
                    string strBARCODE;
                    strBARCODE = setBarCodeSpecimen(dtSpecNo,"");

                    if (strBARCODE.Length > 10)
                    {

                        int nPB = sel_EXAM_RESULTC_PB(pDbCon, specNo);

                        int nPrint = 1;

                        if (nPB >0)
                        {
                            nPrint = 2;
                        }

                        for (int j = 0; j < nPrint; j++)
                        {

                            this.nBCodeName = Int32.Parse(dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.BCODEPRINT.ToString()].ToString());

                            for (int i = 0; i < nBCodeName; i++)
                            {
                                ComPrintApi.SendStringToPrinter(strPrintName, strBARCODE);
                                Thread.Sleep(100);
                            }

                            this.gHEPARIN = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.HEPARIN.ToString()].ToString();
                            this.gSODIUM = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.SODIUM.ToString()].ToString();
                            this.gPRINT_ADD = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.PRINT_ADD.ToString()].ToString();

                            if (gPRINT_ADD.Equals("Y") == true)
                            {
                                if (gSODIUM.Equals("*") == true)
                                {
                                    strBARCODE = setBarCodeSpecimen(dtSpecNo, "P / C / 2.7 ml");
                                    ComPrintApi.SendStringToPrinter(strPrintName, strBARCODE);
                                    Thread.Sleep(100);
                                }

                                if (gHEPARIN.Equals("*") == true)
                                {
                                    strBARCODE = setBarCodeSpecimen(dtSpecNo, "heparin");
                                    ComPrintApi.SendStringToPrinter(strPrintName, strBARCODE);
                                    Thread.Sleep(100);
                                }
                            }
                        }                  
                    }
                    else
                    {
                        b = false;
                    }
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.ToString());
                }

        }
            else if (printType == enmPrintType.USB)
            {
                b = setDrawingSpecimenBarCode();
            }
            else if (printType == enmPrintType.SERIAL)
            {
                string s = setBarCodeSpecimen(dtSpecNo, "",true);
                //string s = setBarCodeTest();

                try
                {
                    SerialPort sp = new SerialPort(strCOMPORT, 9600, Parity.None, 8, StopBits.One);
                   
                    if (sp.IsOpen == false)
                    {
                        sp.Open();

                        sp.Write(s);
                    }
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.ToString());
                }


            }


            return b;
        }

        /// <summary>혈액팔찌 출력</summary>
        /// <param name="specNo"></param>
        /// <param name="printType"></param>
        /// <returns></returns>
        public bool printBloodBand(PsmhDb pDbCon, string specNo, enmPrintType printType = enmPrintType.COM_PORT)
        {
            bool b = true;

            DataTable dtSpecNo;

            dtSpecNo = sel_EXAM_RESULTC_BloodBand(pDbCon, specNo);

            if (ComFunc.isDataTableNull(dtSpecNo) == true)
            {
                ComFunc.MsgBox("검체번호에 맞는 내용이 존재 하지 않습니다.", "검체번호 오류");
                return false;
            }

            string strPrintName = ClsPrint.getLabBarCodePrinter(BARCODE_PRINTER_NAME);

            if (ClsPrint.isLabBarCodePrinter(strPrintName) == false)
            {
                ComFunc.MsgBox("설정된 바코드 프린터기가 없습니다.", "검체번호 오류");
                return false;
            }

            // 1: USB 2: DRAWING 3: SERIAL COM PORT
            if (printType == enmPrintType.COM_PORT)
            {
                String s;
                s = setBarCodeBLOOD(dtSpecNo);

                if (s.Length > 10)
                {
                    try
                    {
                        ComPrintApi.SendStringToPrinter(strPrintName, s);
                    }
                    catch (Exception ex)
                    {
                        b = false;
                        ComFunc.MsgBox(ex.ToString());
                    }
                    
                }
                else
                {
                    b = false;
                }
            }
            else if (printType == enmPrintType.USB)
            {
                //b = setBarCodeDrawing();
            }

            return b;
        }

        string setBarCodeSpecimen(DataTable dtSpecNo, string strEDIT_R, bool isSerial = false)
        {

            string sPano, sAge, sSex, sDep, sDoctNo, sIO, sName, sRoom
                    , sWS, sER, sSpecNo, sInfect, sBdate, sSpecNm, sTubeNm
                    , sVolume, sTubeMsg, sEDTA, sCode1, sCode2, sCode3, sAbo, sWON, sERPAT;
            

            sRoom = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.ROOM.ToString()].ToString();
            sPano = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.PANO.ToString()].ToString();
            sAge = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.AGE.ToString()].ToString();
            sSex = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.SEX.ToString()].ToString();
            sDep = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.DEPTCODE.ToString()].ToString();
            sDoctNo = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.DRCODE.ToString()].ToString();
            sIO = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.IPDOPD.ToString()].ToString();
            sName = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.SNAME.ToString()].ToString();
            sWS = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.WORKSTS.ToString()].ToString();
            sER = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.STRT.ToString()].ToString();

            sWON = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.WON.ToString()].ToString();

            sSpecNo = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.SPECNO.ToString()].ToString();
            sInfect = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.INFECT.ToString()].ToString();
            sBdate = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.BDATE.ToString()].ToString();
            sSpecNm = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.SPECNM.ToString()].ToString();
            sTubeNm = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.TUBENM.ToString()].ToString();
            sVolume = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.VLOLUME.ToString()].ToString();
            sTubeMsg = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.TUBEMSG.ToString()].ToString();
            sEDTA = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.EDTA.ToString()].ToString();

            sCode1 = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.BCODENAME1.ToString()].ToString();
            sCode2 = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.BCODENAME2.ToString()].ToString();
            sCode3 = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.BCODENAME3.ToString()].ToString();
            sAbo = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.ABO.ToString()].ToString();

            sERPAT = dtSpecNo.Rows[0][clsComSupLbExSQL.enmSel_EXAM_SPECMST_BARCODE.ERPAT.ToString()].ToString();



            string Prdata = "";

            //string ls_PrintSpeed = "5";

            Prdata = Prdata + "^XA^BY2,2.0^FS";

            Prdata = Prdata + "^SEE:UHANGUL.DAT^FS";
            Prdata = Prdata + "^CW1,E:KFONT3.FNT^FS";
            Prdata = Prdata + "^FO10,25^CI26^A1N,30,30^FD" + sName + "^FS";

            if (sPano == "81000013") //정도관리
            {
                Prdata = Prdata + "^FO160,30^A0N,30,25^FD" + sPano + "^FS";
            }
            //  환경검사.
            else if (sPano.Equals("81000014") || sPano.Equals("11077917"))
            {
                DataTable dt = GetEnvironmentInfo(sSpecNo);

                if(dt == null || dt.Rows.Count == 0)
                {
                    return string.Empty;
                }
                Prdata = "";
                Prdata = Prdata + "^XA^BY2,2.0^FS^SEE:UHANGUL.DAT^FS^CW1,E:KFONT3.FNT^FS";
                Prdata = Prdata + "^FO10,25^CI26^A1N,30,30^FD" + dt.Rows[0]["DEPTNAME"].ToString() + "^FS";
                Prdata = Prdata + "^FO180,30^A1N,20,20^FD";
                Prdata = Prdata + dt.Rows[0]["GRADENAME2"].ToString() + "^FS";
                Prdata = Prdata + "^FO10,70^A1N,43,43^FD";
                Prdata = Prdata + "감^FS";
                Prdata = Prdata + "^FO15,155^A0N,45,45^FD^FS";
                Prdata = Prdata + "^FO60,60^B3N,N,48,N,N^BY2,2:1,83^FD";
                Prdata = Prdata + sSpecNo + "^FS";
                Prdata = Prdata + "^FO30,148^A1N,25,20^FD";
                Prdata = Prdata + sSpecNo.Insert(6, "-");
                Prdata = Prdata + "  " + ComFunc.FormatStrToDate(dt.Rows[0]["BARCODETIME"].ToString(), "M");
                Prdata = Prdata + "  " + dt.Rows[0]["GRADENAME3"].ToString() + "^FS";
                Prdata = Prdata + "^FO35,173^A0N,20,20^FD";
                Prdata = Prdata + dt.Rows[0]["BARCODE_EXAM"].ToString() + "^FS";
                Prdata = Prdata + "^FO65,193^A0N,20,20^FD^FS";
                Prdata = Prdata + "^FO65,213^A0N,20,20^FD^FS";
                Prdata = Prdata + "^XZ";
            }
            else
            {
                if (sIO == "O") //외래
                {
                    if (string.IsNullOrEmpty(sERPAT) == false && sDep.Equals("ER") == true)
                    {
                        //Prdata = Prdata + "^FO125,30^A0N,30,23^FD" + sPano + "  " + sAge + "/" + sSex + "  " + sDep + " " + sERPAT + "^FS";
                        Prdata = Prdata + "^FO115,30^GB260,25,20^FS";
                        Prdata = Prdata + "^FO125,30^A0N,30,23^FR^FD" + sPano + "  " + sAge + "/" + sSex + "  " + sDep + " " + sERPAT + "^FS";
                    }
                    else
                    {
                        if (sName.ToString().Length <= 3)
                        {
                            Prdata = Prdata + "^FO110,30^A0N,30,23^FD" + sPano + "  " + sAge + "/" + sSex + "  " + sDep + " " + sDoctNo + "^FS";
                        }
                        else if (sName.Length <= 5)
                        {
                            Prdata = Prdata + "^FO150,30^A0N,30,23^FD" + sPano + "  " + sAge + "/" + sSex + "  " + sDep + "^FS";
                        }
                        else
                        {
                            Prdata = Prdata + "^FO170,30^A0N,30,23^FD" + sPano + "  " + sAge + "/" + sSex + "  " + sDep + "^FS";
                        }
                    }
                }
                else
                {
                    if (sName.ToString().Length <= 3)
                    {
                        Prdata = Prdata + "^FO110,30^A0N,30,20^FD" + sPano + "  " + sAge + "/" + sSex + "  " + sDep  + " " + sDoctNo + "^FS";
                    }
                    else if (sName.Length <= 5)
                    {
                        Prdata = Prdata + "^FO150,30^A0N,30,20^FD" + sPano + "  " + sAge + "/" + sSex + "  " + sDep + " " + sDoctNo + "^FS";
                    }
                    else
                    {
                        Prdata = Prdata + "^FO170,30^A0N,30,20^FD" + sPano + "  " + sAge + "/" + sSex + "  " + sDep + " " + sRoom + "^FS";
                    }
                }

                Prdata = Prdata + "^FO15,70^A0N,43,43^FD" + sWS.Substring(0, 1) + "^FS";    // 첫째 ws

                if (sWS.Trim().Length > 1)
                {
                    Prdata = Prdata + "^FO15,115^A0N,32,32^FD" + sWS.Substring(1) + "^FS";  // 두째,세째 ws
                }
                
                Prdata = Prdata + "^FO15,155^A0N,45,45^FD" + sER + "^FS";                   // 응급(E)

                
                //------------<바코드시작>
                Prdata = Prdata + "^FO50,60";                                               // 바코드 인쇄 (10자리)
                Prdata = Prdata + "^B3N,N,48,N,N";
                Prdata = Prdata + "^BY2,2:1,83";
                Prdata = Prdata + "^FD" + sSpecNo + "^FS";
                //------------<바코드끝>

                if (string.IsNullOrEmpty(sInfect)==false)
                {   
                    Prdata = Prdata + "^FO20,150^GB40,30,20^FS";                            //  혈액감염내역
                    Prdata = Prdata + "^FO20,150^A0N,30,20^FR^FD" + sInfect.Trim() + "^FS";    //  혈액감염내역
                    Prdata = Prdata + "^FO60,148^A0N,25,20^FD" + sSpecNo.Substring(0, 6) + "-" + sSpecNo.Substring(6, 4) + " " + sBdate;
                }
                else
                {
                    Prdata = Prdata + "^FO60,148^A0N,25,20^FD" + sSpecNo.Substring(0, 6) + "-" + sSpecNo.Substring(6, 4) + " " + sBdate;
                }

                if (string.IsNullOrEmpty(strEDIT_R.Trim()) == true)
                {
                    Prdata = Prdata + "  " + sTubeMsg + "^FS";
                }
                else
                {
                    Prdata = Prdata + "  " + strEDIT_R + "^FS";
                }

                if (sWON == "#")
                {
                    Prdata = Prdata + "^FO368,60^GB10,20,20^FS";
                    Prdata = Prdata + "^FO368,60^A0N,20,20^FR^FD" + "#" + "^FS";
                }

                if (sEDTA == "*")
                {
                    Prdata = Prdata + "^FO15,168^GB42,20,20^FS";
                    Prdata = Prdata + "^FO15,168^A0N,20,20^FR^FD" + "EDTA" + "^FS";
                }

                Prdata = Prdata + "^FO65,173^A0N,20,20^FD" + sCode1 + "^FS";
                Prdata = Prdata + "^FO65,193^A0N,20,20^FD" + sCode2 + "^FS";
                Prdata = Prdata + "^FO65,213^A0N,20,20^FD" + sCode3 + "^FS";

                //'ws가B 인 경우  혈액형 표시
                if (sWS.Replace("B", "").Length != sWS.Length)
                {
                    Prdata = Prdata + "^FO300,160^A0N,45,45^FD" + sAbo + "^FS";
                }

                Prdata = Prdata + "^XZ";
            }
            
            return Prdata;
        }

        /// <summary>
        /// 환경검사 관련 정보조회
        /// </summary>
        /// <param name="specNo"></param>
        /// <returns></returns>
        DataTable GetEnvironmentInfo(string specNo)
        {
            StringBuilder sql = null;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                sql = new StringBuilder();
                sql.Append("SELECT A.BARCODEDATE                                                                ").Append("\n");
                sql.Append("     , A.BARCODETIME                                                                ").Append("\n");
                sql.Append("     , A.BARCODE                                                                    ").Append("\n");
                sql.Append("     , G.NAME AS DEPTNAME                                                           ").Append("\n");
                sql.Append("     , D.CODENAME AS GRADENAME2                                                     ").Append("\n");
                sql.Append("     , E.CODENAME AS GRADENAME3                                                     ").Append("\n");
                sql.Append("     , F.CODENAME AS GRADENAME4                                                     ").Append("\n");
                sql.Append("     , (                                                                            ").Append("\n");
                sql.Append("            SELECT LISTAGG(DD.BCODENAME, ',') WITHIN GROUP(ORDER BY DD.BCODENAME)   ").Append("\n");
                sql.Append("              FROM ENVIRONMENT_ORDER AA                                             ").Append("\n");
                sql.Append("              INNER JOIN ENVIRONMENT_EXAM_MASTER BB                                 ").Append("\n");
                sql.Append("                      ON AA.ENVIRONMENTCODE = BB.CODE                               ").Append("\n");
                sql.Append("              INNER JOIN ENVIRONMENT_EXAM_DETAIL CC                                 ").Append("\n");
                sql.Append("                      ON BB.CODE = CC.EXAMMASTERCODE                                ").Append("\n");
                sql.Append("              INNER JOIN KOSMOS_OCS.EXAM_MASTER DD                                  ").Append("\n");
                sql.Append("                      ON CC.EXAMCODE = DD.MASTERCODE                                ").Append("\n");
                sql.Append("             WHERE AA.ORDERNO = A.ORDERNO                                           ").Append("\n");
                sql.Append("       ) AS BARCODE_EXAM                                                            ").Append("\n");
                sql.Append("  FROM ENVIRONMENT_ORDER A                                                          ").Append("\n");
                sql.Append("  INNER JOIN ENVIRONMENT_EXAM_MASTER B                                              ").Append("\n");
                sql.Append("          ON A.ENVIRONMENTCODE = B.CODE                                             ").Append("\n");
                sql.Append("  INNER JOIN BAS_GRADE_ENVIRONMENT C                                                ").Append("\n");
                sql.Append("          ON B.GRADE1   = C.CODE                                                    ").Append("\n");
                sql.Append("         AND C.GRADE    = 1                                                         ").Append("\n");
                sql.Append("  INNER JOIN BAS_GRADE_ENVIRONMENT D                                                ").Append("\n");
                sql.Append("          ON B.GRADE2   = D.CODE                                                    ").Append("\n");
                sql.Append("         AND D.GRADE    = 2                                                         ").Append("\n");
                sql.Append("  INNER JOIN BAS_GRADE_ENVIRONMENT E                                                ").Append("\n");
                sql.Append("          ON B.GRADE3   = E.CODE                                                    ").Append("\n");
                sql.Append("         AND E.GRADE    = 3                                                         ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT F                                           ").Append("\n");
                sql.Append("               ON B.GRADE4   = F.CODE                                               ").Append("\n");
                sql.Append("              AND F.GRADE    = 4                                                    ").Append("\n");
                sql.Append("  INNER JOIN BAS_BUCODE G                                                           ").Append("\n");
                sql.Append("          ON C.BUCODE = G.BUCODE                                                    ").Append("\n");
                sql.Append(" WHERE A.SPECNO = '" + specNo + "'                                                  ").Append("\n");
                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    return null;
                }
            }
            catch(Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, sql.ToString(), clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return dt;
        }

        string setBarCodeQCSpecimen(DataTable dtSpecNo)
        {
            string Prdata = "";

            string strPTNAME        = dtSpecNo.Rows[0][(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNMST_BARCODE.PTNAME].ToString();
            string strEQUCODE       = dtSpecNo.Rows[0][(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNMST_BARCODE.EQUCODE].ToString();
            string strNOR           = dtSpecNo.Rows[0][(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNMST_BARCODE.NOR].ToString();
            string strWC            = dtSpecNo.Rows[0][(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNMST_BARCODE.WC].ToString();
            string strEXAMCODE      = "";
            string strLOTNO         = dtSpecNo.Rows[0][(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNMST_BARCODE.LOTNO].ToString();
            string strSPECNO        = dtSpecNo.Rows[0][(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNMST_BARCODE.SPECNO].ToString();

            Prdata = Prdata + "^XA^LH0,0^FS";
            Prdata = Prdata + "^SEE:UHANGUL.DAT^FS";
            Prdata = Prdata + "^CW1,E:KFONT3.FNT^FS";
            //Prdata = Prdata + "^FO20,25^CI26^A1N,30,30^FD" + strPTNAME + "^FS";
            Prdata = Prdata + "^FO20,20^CI26^A1N,30,30^FD" + strPTNAME + "^FS";

            strEQUCODE += VB.Space(14);
            strEQUCODE = strEQUCODE.Substring(0, 14);

            //Prdata = Prdata + "^FO150,30^A0N,30,25^FD" + strEQUCODE + " " + strNOR + "^FS";
            Prdata = Prdata + "^FO150,20^A0N,30,25^FD" + strEQUCODE + " " + strNOR + "^FS";

            Prdata = Prdata + "^FO20,70^A0N,45,45^FD" + strWC + "^FS";    // 첫째 ws

            //------------<바코드시작>

            //Prdata = Prdata + "^FO50,60";                                           // 바코드 인쇄 (10자리)
            //Prdata = Prdata + "^B3N,N,50,N,N";
            //Prdata = Prdata + "^BY2,2:1,60";
            Prdata = Prdata + "^FO50,50";                                           // 바코드 인쇄 (10자리)
            Prdata = Prdata + "^B3N,N,48,N,N";
            Prdata = Prdata + "^BY2,2:1,85";
            Prdata = Prdata + "^FD" + strSPECNO.Replace("-","").Trim() + "^FS";

            //------------<바코드끝>


            //Prdata = Prdata + "^FO50,130^A0N,25,20^FD" + strSPECNO + "   Lot : " + strLOTNO + "^FS";
            Prdata = Prdata + "^FO50,140^A0N,25,20^FD" + strSPECNO + "   Lot : " + strLOTNO + "^FS";

            for (int i = 0; i < dtSpecNo.Rows.Count; i++)
            {
                strEXAMCODE += dtSpecNo.Rows[i][(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNMST_BARCODE.EXAMCODE].ToString() + ",";
            }

            strEXAMCODE = strEXAMCODE.Substring(0, strEXAMCODE.Length - 1);

            //Prdata = Prdata + "^FO50,160^A0N,18,18^FD" + strEXAMCODE + "^FS";
            Prdata = Prdata + "^FO50,165^A0N,18,18^FD" + strEXAMCODE + "^FS";
            Prdata = Prdata + "^XZ";
            return Prdata;
        }

        string setBarCodeBLOOD(DataTable dtSpecNo)
        {
            string Prdata = "";

            string strLINE_01   = dtSpecNo.Rows[0][(int)enmSel_EXAM_RESULTC_BloodBand.LINE_01].ToString();
            string strLINE_02_1 = dtSpecNo.Rows[0][(int)enmSel_EXAM_RESULTC_BloodBand.LINE_02_1].ToString();
            string strLINE_02_2 = dtSpecNo.Rows[0][(int)enmSel_EXAM_RESULTC_BloodBand.LINE_02_2].ToString();
            string strLINE_03   = dtSpecNo.Rows[0][(int)enmSel_EXAM_RESULTC_BloodBand.LINE_03].ToString();
            string strLINE_04   = dtSpecNo.Rows[0][(int)enmSel_EXAM_RESULTC_BloodBand.LINE_04].ToString();

            Prdata = Prdata + "^XA^BY2,2.0^FS";

            Prdata = Prdata + "^SEE:UHANGUL.DAT^FS";
            Prdata = Prdata + "^CW1,E:KFONT3.FNT^FS";

            Prdata = Prdata + "^FO15,30^A0N,32,37^FD" + "  " + strLINE_01 + "^FS";                    
            Prdata = Prdata + "^FO15,80^A0N,45,45^FD" + "    " + strLINE_02_1 + "^FS";    // 첫째 ws
            Prdata = Prdata + "^FO95,80^CI26^A1N,38,34^FD" + "    " + strLINE_02_2 + "^FS";
            Prdata = Prdata + "^FO15,135^A0N,20,27^FD" + "    " + strLINE_03 + "^FS";    // 첫째 ws
            Prdata = Prdata + "^FO15,165^A0N,20,27^FD" + "    " + strLINE_04 + "^FS";    // 첫째 ws              



            Prdata = Prdata + "^XZ";
            
            return Prdata;
        }

        bool setDrawingSpecimenBarCode()
        {

            bool b;

            b = true;

            try
            {
                PrintDocument pd;
                pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = this.PrintName;
                pd.PrinterSettings.DefaultPageSettings.PaperSize = barCodeSize;

                pd.PrintPage += new PrintPageEventHandler(ePrintPage);
                pd.Print();    //프린트
            }
            catch (Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show(Ex.ToString());

                b = false;
            }

            return b;


        }

        bool setDrawingBloodBarCode()
        {

            bool b;

            b = true;

            try
            {
                PrintDocument pd;
                pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = this.PrintName;
                pd.PrinterSettings.DefaultPageSettings.PaperSize = barCodeSize;

                pd.PrintPage += new PrintPageEventHandler(eBloodPring);
                pd.Print();    //프린트
            }
            catch (Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show(Ex.ToString());
                b = false;
            }

            return b;


        }

        void eBloodPring(object sender, PrintPageEventArgs e)
        {
            string str = "";

            string a = null;

            Font printFont;

            printFont = new Font("굴림체", 10, FontStyle.Bold);

            if (this.gBarCodeParam.inFect != null && this.gBarCodeParam.inFect.Trim().Length > 0)
            {
                str = "*Jeil Hosp" + "  " + "*Jeil Hosp";
            }
            else
            {
                str = "Jeil Hosp" + "    " + "Jeil Hosp";
            }

            e.Graphics.DrawString(str, printFont, _textColor, 15, 8, new StringFormat());

            printFont = new Font("굴림체", 10, FontStyle.Bold);
            str = " " + this.gBarCodeParam.wc.Substring(0, 1) + a + "       " + this.gBarCodeParam.wc.Substring(0, 1) + a;
            e.Graphics.DrawString(str, printFont, _textColor, 15, 25, new StringFormat());
        }

        void ePrintPage(object sender, PrintPageEventArgs ev)
        {
            string str = "";

            string str_1 = null;
            string str_2 = null;

            string a = null;

            Font printFont;

            if (this.gBarCodeParam.wc.Substring(0, 1) == "T")
            {
                a = "(PAP)";
            }
            else
            {
                a = "(H&E)";
            }

            if (this.slideNo != null)
            {

                printFont = new Font("굴림체", 10, FontStyle.Bold);

                if (this.gBarCodeParam.inFect != null && this.gBarCodeParam.inFect.Trim().Length > 0)
                {
                    str = "*Jeil Hosp" + "  " + "*Jeil Hosp";
                }
                else
                {
                    str = "Jeil Hosp" + "    " + "Jeil Hosp";
                }

                ev.Graphics.DrawString(str, printFont, _textColor, 15, 8, new StringFormat());

                printFont = new Font("굴림체", 10, FontStyle.Bold);
                str = " " + this.gBarCodeParam.wc.Substring(0, 1) + a + "       " + this.gBarCodeParam.wc.Substring(0, 1) + a;
                ev.Graphics.DrawString(str, printFont, _textColor, 15, 25, new StringFormat());


                printFont = new Font("굴림체", 10, FontStyle.Bold);
                str = this.gBarCodeParam.specNo.Substring(3, 7).ToString() + "-" + this.gBarCodeParam.specNo.Substring(0, 2).ToString() + "-" + this.gBarCodeParam.specNo.Substring(2, 1).ToString() + " " +
                    this.gBarCodeParam.specNo.Substring(3, 7).ToString() + "-" + this.gBarCodeParam.specNo.Substring(0, 2).ToString() + "-" + this.gBarCodeParam.specNo.Substring(2, 1).ToString();
                ev.Graphics.DrawString(str, printFont, _textColor, 5, 40, new StringFormat());

                printFont = new Font("굴림체", 10, FontStyle.Bold);
                str = this.slideNo;
                ev.Graphics.DrawString(str, printFont, _textColor, 5, 75, new StringFormat());

                this.gBarCodeParam.inFect = null;
            }
            else
            {
                if (this.gBarCodeParam.gbio == "I")
                {
                    str = this.gBarCodeParam.ptName + "" + this.gBarCodeParam.ptNo + " " + this.gBarCodeParam.age + "/" + this.gBarCodeParam.sex + "/" + this.gBarCodeParam.ward;
                }
                else
                {
                    str = this.gBarCodeParam.ptName + "" + this.gBarCodeParam.ptNo + " " + this.gBarCodeParam.age + "/" + this.gBarCodeParam.sex + "/" + this.gBarCodeParam.dept;
                }
                printFont = new Font("굴림체", 11, FontStyle.Bold);
                ev.Graphics.DrawString(str, printFont, _textColor, 5, 5, new StringFormat());

                printFont = new Font("굴림체", 18);
                str = this.gBarCodeParam.wc;
                ev.Graphics.DrawString(str, printFont, _textColor, 1, 22, new StringFormat());


                printFont = new Font("굴림체", 18);
                str = this.gBarCodeParam.er;
                ev.Graphics.DrawString(str, printFont, _textColor, 5, 47, new StringFormat());

                printFont = new Font("Code39(2:3)", 22);

                if (this.gBarCodeParam.wc == "E")
                {
                    str = "*" + this.gBarCodeParam.ptNo + "*";
                }
                else
                {
                    str = "*" + this.gBarCodeParam.specNo + "*";
                }
                ev.Graphics.DrawString(str, printFont, _textColor, 22, 22, new StringFormat());

                printFont = new Font("굴림체", 8);

                if (this.gBarCodeParam.wc == "A" || this.gBarCodeParam.wc == "T" || this.gBarCodeParam.wc == "W" || this.gBarCodeParam.wc == "V")
                {
                    str = this.gBarCodeParam.specNo.Substring(0, 2).ToString() + "-" + this.gBarCodeParam.specNo.Substring(2, 1).ToString() + "-" + this.gBarCodeParam.specNo.Substring(3, 7).ToString() + " " + this.gBarCodeParam.receipTime + " " + this.gBarCodeParam.drName.Trim();
                }
                else
                {
                    str = this.gBarCodeParam.specNo.Substring(0, 6).ToString() + "-" + this.gBarCodeParam.specNo.Substring(6, 4).ToString() + " " + this.gBarCodeParam.receipTime + " " + this.gBarCodeParam.drName.Trim();
                }
                ev.Graphics.DrawString(str, printFont, _textColor, 25, 57, new StringFormat());

                string[] s = gBarCodeParam.barCodeName.Split('/');
                int nLine_Return = 7;

                if (s.Length > 7)
                {
                    printFont = new Font("굴림체", 5, FontStyle.Bold);

                    for (int i = 0; i < s.Length; i++)
                    {
                        if (i <= nLine_Return)
                        {
                            str_1 += s[i].ToString().Trim() + "/";
                        }
                        else if (i > nLine_Return && i < nLine_Return * 2)
                        {
                            str_2 += s[i].ToString().Trim() + "/";
                        }
                    }

                    str_1 = str_1.Substring(0, str_1.Length - 1);

                    if (str_2 != null && str_2.Trim().Length > 0)
                    {
                        str_2 = str_2.Substring(0, str_2.Length - 1);
                    }

                    ev.Graphics.DrawString(str_1, printFont, _textColor, 10, 69, new StringFormat());

                    if (s.Length > nLine_Return) ev.Graphics.DrawString(str_2, printFont, _textColor, 10, 82, new StringFormat());
                }
                else if (s.Length > 5 && s.Length <= 10)
                {
                    printFont = new Font("굴림체", 7, FontStyle.Bold);
                    str = gBarCodeParam.barCodeName;
                    ev.Graphics.DrawString(str, printFont, _textColor, 10, 67, new StringFormat());

                }
                else
                {
                    printFont = new Font("굴림체", 8, FontStyle.Bold);

                    str = gBarCodeParam.barCodeName;
                    ev.Graphics.DrawString(str, printFont, _textColor, 10, 67, new StringFormat());
                }

                printFont = new Font("굴림체", 7, FontStyle.Bold);
                str = this.gBarCodeParam.speciMan;
                if (this.gBarCodeParam.inFect != null && this.gBarCodeParam.inFect.Trim().Length > 0)
                {

                    str += " 주사침주의!";

                    if (this.gBarCodeParam.inFect.Trim() == "JEIL산모 B" || this.gBarCodeParam.inFect.Trim() == "JEIL  B" || this.gBarCodeParam.inFect.Trim() == "JEILB")
                    {
                        str = str + " HBsAg(+)";
                    }
                    if (this.gBarCodeParam.inFect.Trim() == "JEILC")
                    {
                        str = str + "HCV-Ah(+)";
                    }
                    if (this.gBarCodeParam.inFect.Trim() == "JEILV")
                    {
                        str = str + "VDRL(+)";
                    }
                    if (this.gBarCodeParam.inFect.Trim() == "JEILA")
                    {
                        str = str + "HIV(+)";
                    }
                }

                ev.Graphics.DrawString(str, printFont, _textColor, 10, 82, new StringFormat());
            }
        }

        public bool EXAM_CHECK_BLD(PsmhDb pDbCon, string argSpecCode)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  YNAME";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_SPECODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND GUBUN = '14' ";
            SQL += ComNum.VBLF + "  AND CODE = '" + argSpecCode + "' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["YNAME"].ToString().Trim() == "BLD")
                {
                    rtnVal = true;
                }

                else
                {
                    rtnVal = false;
                }
            }

            dt.Dispose();
            dt = null;
            return rtnVal;
        }


    }
}
