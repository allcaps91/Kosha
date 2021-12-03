using ComBase.Controls;
using ComDbB;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ComBase
{
    class clsEmrFunc
    {
        #region EmrPatient
        public class EmrPatient
        {
            /// <summary>
            /// 등록번호
            /// </summary>
            public string ptNo;
            /// <summary>
            /// 성명
            /// </summary>
            public string ptName;
            /// <summary>
            /// 주민번호 앞
            /// </summary>
            public string ssno1;
            /// <summary>
            /// 주민번호 뒤
            /// </summary>
            public string ssno2;
            /// <summary>
            /// 생일
            /// </summary>
            public string birthdate;

            /// <summary>
            /// 성별
            /// </summary>
            public string sex;
            /// <summary>
            /// 나이
            /// </summary>
            public string age;

            /// <summary>
            /// 전화번호
            /// </summary>
            public string tel;          //
            /// <summary>
            /// 핸드폰
            /// </summary>
            public string celphno;      //
            /// <summary>
            /// 우편번호(Old)
            /// </summary>
            public string zipcd;        //
            /// <summary>
            /// 주소(Old)
            /// </summary>
            public string addr;         //
            /// <summary>
            /// 상세주소(Old)
            /// </summary>
            public string address;      //
            /// <summary>
            /// 도로명 우편번호
            /// </summary>
            public string zipcdLoad;    //
            /// <summary>
            /// 도로명 주소
            /// </summary>
            public string addrLoad;     //
            /// <summary>
            /// 도로명 상세주소
            /// </summary>
            public string addressLoad;  //


            // 2017-06-09 법정감염병신고땜에 추가함
            /// <summary>
            /// 도로명주소
            /// </summary>
            public string rdnmAddr;         // 
            /// <summary>
            /// 도로명주소 상세
            /// </summary>
            public string rdnmAddr_dtl;     // 
            /// <summary>
            /// 피보험자 이름
            /// </summary>
            public string pibo_name;        // 

            /// <summary>
            /// 접수번호
            /// </summary>
            public string acpNo;            //
            /// <summary>
            /// 외래(O), 입원(I)
            /// </summary>
            public string inOutCls;         //
            /// <summary>
            /// 내원(입원)일자
            /// </summary>
            public string medFrDate;        //

            /// <summary>
            /// 동국대 경주병원만 사용함
            /// </summary>
            public string medIP_INDATE;     //

            /// <summary>
            /// 내원(입원)시간
            /// </summary>
            public string medFrTime;        //
            /// <summary>
            /// 퇴원일자
            /// </summary>
            public string medEndDate;       //
            /// <summary>
            /// 퇴원시간
            /// </summary>
            public string medEndTime;       //
            /// <summary>
            /// 퇴원예고시간
            /// </summary>
            public string medEndexDate;     //
            /// <summary>
            /// 현재 진료과
            /// </summary>
            public string medDeptCd;        //
            /// <summary>
            /// 현재 진료의
            /// </summary>
            public string medDrCd;          //
            /// <summary>
            /// 진료과 명
            /// </summary>
            public string medDeptKorName;   //
            /// <summary>
            /// 의사명
            /// </summary>
            public string medDrName;        //
            /// <summary>
            /// 병동
            /// </summary>
            public string ward;             //
            /// <summary>
            /// 병실
            /// </summary>
            public string room;             //
            /// <summary>
            /// 비고
            /// </summary>
            public string remark;           //

            /// <summary>
            /// 환자(보험)구분
            /// </summary>
            public string bi;               //
            /// <summary>
            /// 환자(보험)구분명
            /// </summary>
            public string biname;           //
            /// <summary>
            /// 증번호
            /// </summary>
            public string g_Kiho;           //

            /// <summary>
            /// 서식지번호
            /// </summary>
            public long formNo;             //
            /// <summary>
            /// 서식지 버전 번호
            /// </summary>
            public int updateNo;            //
            /// <summary>
            /// 출력여부
            /// </summary>
            public string printyn;          //
            /// <summary>
            /// 차트 작성날짜
            /// </summary>
            public string chartDate;        //
            /// <summary>
            /// 차트 작성시간
            /// </summary>
            public string chartTime;        //
            /// <summary>
            /// 작성자 사번
            /// </summary>
            public string writeSabun;        //
            /// <summary>
            /// 작성자 이름
            /// </summary>
            public string writeName;        //
            /// <summary>
            /// 작성자 날짜
            /// </summary>
            public string writeDate;        //
            /// <summary>
            /// 작성자 시간
            /// </summary>
            public string writeTime;        //
            /// <summary>
            /// 산모(엄마) 명
            /// </summary>
            public string MomName; //
            /// <summary>
            /// 아빠 이름
            /// </summary>
            public string DadName;  //
            /// <summary>
            /// XML형태의 이전 기록지(1:Old, 0:New)
            /// </summary>
            public string oldYn;            //

            /// <summary>
            /// 수술기록지관련해서 수술번호 전달
            /// </summary>
            public string opno;             //
            /// <summary>
            /// 수술일자
            /// </summary>
            public string opdate;           //
            /// <summary>
            /// 동국대경주병원사용(당일 두개 수술일 경우)
            /// </summary>
            public string opdegree;         //
            /// <summary>
            /// 수술과
            /// </summary>
            public string opdept;           //

            /// <summary>
            /// EMR차트 작성당시 환자 진료과
            /// </summary>
            public string nowdeptcd; //
            /// <summary>
            /// EMR차트 작성당시 환자 진료의사
            /// </summary>
            public string nowdrcd;   // 
            /// <summary>
            /// EMR차트 작성당시 환자 병실
            /// </summary>
            public string nowroomno;    // 

            /// <summary>
            /// 병동 입실일자
            /// </summary>
            public string wardDate; //
            /// <summary>
            /// /병동 입실시간
            /// </summary>
            public string wardTime; //

            /// <summary>
            /// EMR차트 작성당시 사용자 작성자 과
            /// </summary>
            public string cur_Dept;     // 
            /// <summary>
            /// EMR차트 작성당시 사용자 작성자 Grade
            /// </summary>
            public string cur_Grade;    // 

            //포항성모병원 외래/입원 접수 번호
            /// <summary>
            /// 외래접수번호
            /// </summary>
            public string acpNoOut;            //
            /// <summary>
            /// 입원접수번호
            /// </summary>
            public string acpNoIn;            //

        }
        #endregion

        #region EMRFORM
        public class EmrForm
        {
            /// <summary>
            /// 기록지 번호
            /// </summary>
            public long FmFORMNO;
            /// <summary>
            /// 기록지 업데이트 번호
            /// </summary>
            public int FmUPDATENO;
            /// <summary>
            /// 1: 이전서식지(XML), 0: 신규서식지
            /// </summary>
            public int FmOLDGB;

        }
        #endregion

        #region FormFlowSheet
        public class FormFlowSheet
        {
            public string ItemCode;
            public string CellType;
        }
        #endregion

        #region SaveChartMastHis
        /// <summary>
        /// 기록지 내용을 백업한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="dblEmrHisNo"></param>
        /// <param name="strCurDate"></param>
        /// <param name="strCurTime"></param>
        /// <param name="strFlag"></param>
        /// <returns></returns>
        public static string SaveChartMastHis(PsmhDb pDbCon, string strEmrNo, double dblEmrHisNo, string strCurDate, string strCurTime, string strFlag, string strSaveFlag)
        {
            string rtnVal = "OK";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            try
            {
                //AEMRCHARTMST
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTMSTHIS ";
                SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, ACPNO,  ";
                SQL = SQL + ComNum.VBLF + "    FORMNO, UPDATENO, CHARTDATE,  ";
                SQL = SQL + ComNum.VBLF + "    CHARTTIME, CHARTUSEID, WRITEDATE,  ";
                SQL = SQL + ComNum.VBLF + "    WRITETIME, COMPUSEID, COMPDATE,  ";
                SQL = SQL + ComNum.VBLF + "    COMPTIME, PRNTYN, SAVEGB,  ";
                SQL = SQL + ComNum.VBLF + "    SAVECERT, FORMGB, PTNO, ";
                SQL = SQL + ComNum.VBLF + "    INOUTCLS, MEDFRDATE, MEDFRTIME,  ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDATE, MEDENDTIME, MEDDEPTCD,  ";
                SQL = SQL + ComNum.VBLF + "    MEDDRCD, OPDATE, OPDEGREE,  ";
                SQL = SQL + ComNum.VBLF + "    OP_DEPT, DEPTCDNOW, DRCDNOW,  ";
                SQL = SQL + ComNum.VBLF + "    ROOM_NO, ACPNOOUT, CURDEPT,  ";
                SQL = SQL + ComNum.VBLF + "    CURGRADE, CODEUSEID, CODEDATE, CODETIME,  CERTNO, CERTDATE, ";
                SQL = SQL + ComNum.VBLF + "    DCEMRNOHIS, DCUSEID,  ";
                SQL = SQL + ComNum.VBLF + "    DCDATE, DCTIME )";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    EMRNO, EMRNOHIS, ACPNO,  ";
                SQL = SQL + ComNum.VBLF + "    FORMNO, UPDATENO, CHARTDATE, ";
                SQL = SQL + ComNum.VBLF + "    CHARTTIME, CHARTUSEID, WRITEDATE,  ";
                SQL = SQL + ComNum.VBLF + "    WRITETIME, COMPUSEID, COMPDATE, ";
                SQL = SQL + ComNum.VBLF + "    COMPTIME, PRNTYN, SAVEGB, ";
                SQL = SQL + ComNum.VBLF + "    SAVECERT, FORMGB, PTNO,  ";
                SQL = SQL + ComNum.VBLF + "    INOUTCLS, MEDFRDATE, MEDFRTIME,  ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDATE, MEDENDTIME, MEDDEPTCD,  ";
                SQL = SQL + ComNum.VBLF + "    MEDDRCD, OPDATE, OPDEGREE,  ";
                SQL = SQL + ComNum.VBLF + "    OP_DEPT, DEPTCDNOW, DRCDNOW,  ";
                SQL = SQL + ComNum.VBLF + "    ROOM_NO, ACPNOOUT, CURDEPT,  ";
                SQL = SQL + ComNum.VBLF + "    CURGRADE, CODEUSEID, CODEDATE, CODETIME, CERTNO, CERTDATE, ";
                SQL = SQL + ComNum.VBLF + dblEmrHisNo + " AS DCEMRNOHIS, ";
                if (strSaveFlag == "SAVE")
                {
                    SQL = SQL + ComNum.VBLF + "      CHARTUSEID,";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    '" + clsType.User.IdNumber + "',";   //DCUSEID
                }
                SQL = SQL + ComNum.VBLF + "    '" + strCurDate + "',";   //DCDATE
                SQL = SQL + ComNum.VBLF + "    '" + strCurTime + "' ";   //DCTIME
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTMST";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    rtnVal = SqlErr;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "AEMRCHARTMST";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    rtnVal = SqlErr;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                //AEMRCHARTROW
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROWHIS ";
                SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUTGB, INPUSEID, INPDATE, INPTIME)";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUTGB, INPUSEID, INPDATE, INPTIME";
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTROW";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                //SQL = SQL + ComNum.VBLF + "   AND EMRNOHIS = " + dblEmrHisNoOld;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    rtnVal = SqlErr;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "AEMRCHARTROW";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                //SQL = SQL + ComNum.VBLF + "   AND EMRNOHIS = " + dblEmrHisNoOld;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    rtnVal = SqlErr;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                //AEMRCHARTDRAW
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTDRAWHIS ";
                SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, FORMNO, UPDATENO, DRAW, FILENAME, IMAGE, IMAGENAME, ITEMNAME)";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    EMRNO, EMRNOHIS, FORMNO, UPDATENO, DRAW, FILENAME, IMAGE, IMAGENAME, ITEMNAME";
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTDRAW";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    rtnVal = SqlErr;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "AEMRCHARTDRAW";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    rtnVal = SqlErr;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                //간호기록

                //AEMRNURSRECORD
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRNURSRECORDHIS ";
                SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, WARDCODE, ROOMCODE, PROBLEMCODE, PROBLEM, TYPE, NRRECODE) ";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    EMRNO, EMRNOHIS, WARDCODE, ROOMCODE, PROBLEMCODE, PROBLEM, TYPE, NRRECODE ";
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRNURSRECORD ";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    rtnVal = SqlErr;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "AEMRNURSRECORD";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    rtnVal = SqlErr;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                rtnVal = SqlErr;
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }
        #endregion

        #region SaveChartMstOnly
        /// <summary>
        /// ChartMst에만 등록
        /// </summary>
        /// <param name="dblEmrNoNew"></param>
        /// <param name="po"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="strCHARTUSEID"></param>
        /// <param name="strCOMPUSEID"></param>
        /// <param name="strSAVEGB"></param>
        /// <param name="strFORMGB"></param>
        /// <returns></returns>
        public static bool SaveChartMstOnly(PsmhDb pDbCon, double dblEmrNoNew, double dblEmrHisNo, EmrPatient po,
                                string strFormNo, string strUpdateNo, string strChartDate, string strChartTime,
                                string strCHARTUSEID, string strCOMPUSEID, string strSAVEGB, string strSAVECERT, string strFORMGB,
                                string strCurDate, string strCurTime, string strSaveFlag)
        {
            bool rtnVal = true;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTMST ";
                SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, ACPNO,  ";
                SQL = SQL + ComNum.VBLF + "    FORMNO, UPDATENO, CHARTDATE,  ";
                SQL = SQL + ComNum.VBLF + "    CHARTTIME, CHARTUSEID, WRITEDATE,  ";
                SQL = SQL + ComNum.VBLF + "    WRITETIME, COMPUSEID, COMPDATE,  ";
                SQL = SQL + ComNum.VBLF + "    COMPTIME, PRNTYN, SAVEGB,  ";
                SQL = SQL + ComNum.VBLF + "    SAVECERT, FORMGB, PTNO, ";
                SQL = SQL + ComNum.VBLF + "    INOUTCLS, MEDFRDATE, MEDFRTIME,  ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDATE, MEDENDTIME, MEDDEPTCD,  ";
                SQL = SQL + ComNum.VBLF + "    MEDDRCD, OPDATE, OPDEGREE,  ";
                SQL = SQL + ComNum.VBLF + "    OP_DEPT, DEPTCDNOW, DRCDNOW,  ";
                SQL = SQL + ComNum.VBLF + "    ROOM_NO, ACPNOOUT, CURDEPT, CURGRADE, ";
                SQL = SQL + ComNum.VBLF + "    CODEUSEID, CODEDATE, CODETIME) ";
                SQL = SQL + ComNum.VBLF + " VALUES (";
                SQL = SQL + ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                SQL = SQL + ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                SQL = SQL + ComNum.VBLF + po.acpNo + ",";   //ACPNO  po.acpNo
                SQL = SQL + ComNum.VBLF + strFormNo + ","; //FORMNO
                SQL = SQL + ComNum.VBLF + strUpdateNo + ",";   //UPDATENO
                SQL = SQL + ComNum.VBLF + "'" + strChartDate + "',";   //CHARTDATE
                SQL = SQL + ComNum.VBLF + "'" + strChartTime + "',";   //CHARTTIME
                SQL = SQL + ComNum.VBLF + "'" + strCHARTUSEID + "',";   //CHARTUSEID
                SQL = SQL + ComNum.VBLF + "'" + strCurDate + "',";   //WRITEDATE
                SQL = SQL + ComNum.VBLF + "'" + strCurTime + "',";   //WRITETIME
                if (strSAVEGB == "1")
                {
                    SQL = SQL + ComNum.VBLF + "'" + strCOMPUSEID + "',";   //COMPUSEID
                    SQL = SQL + ComNum.VBLF + "'" + strCurDate + "',";   //COMPDATE
                    SQL = SQL + ComNum.VBLF + "'" + strCurTime + "',";   //COMPTIME
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "'',";   //COMPUSEID
                    SQL = SQL + ComNum.VBLF + "'',";   //COMPDATE
                    SQL = SQL + ComNum.VBLF + "'',";   //COMPTIME
                }
                //if (VB.Left(strCHARTUSEID, 1) == "A") //교수 ID일 경우는 확인 사인을 박는다.
                //{
                //    SQL = SQL + ComNum.VBLF + "'" + strCHARTUSEID + "',";   //COMPUSEID
                //    SQL = SQL + ComNum.VBLF + "'" + strCurDate + "',";   //COMPDATE
                //    SQL = SQL + ComNum.VBLF + "'" + strCurTime + "',";   //COMPTIME
                //}
                //else
                //{
                //    SQL = SQL + ComNum.VBLF + "'',";   //COMPUSEID
                //    SQL = SQL + ComNum.VBLF + "'',";   //COMPDATE
                //    SQL = SQL + ComNum.VBLF + "'',";   //COMPTIME
                //}

                SQL = SQL + ComNum.VBLF + "'',";   //PRNTYN
                SQL = SQL + ComNum.VBLF + "'" + strSAVEGB + "',";   //SAVEGB
                SQL = SQL + ComNum.VBLF + "'" + strSAVECERT + "',";   //SAVECERT
                SQL = SQL + ComNum.VBLF + "'" + strFORMGB + "',";   //FORMGB
                SQL = SQL + ComNum.VBLF + "'" + po.ptNo + "',";   //PTNO    po.ptNo
                SQL = SQL + ComNum.VBLF + "'" + po.inOutCls + "',";   //INOUTCLS   
                SQL = SQL + ComNum.VBLF + "'" + po.medFrDate + "',";   //MEDFRDATE   
                SQL = SQL + ComNum.VBLF + "'" + po.medFrTime + "',";   //MEDFRTIME   
                SQL = SQL + ComNum.VBLF + "'" + po.medEndDate + "',";   //MEDENDDATE   
                SQL = SQL + ComNum.VBLF + "'" + po.medEndTime + "',";   //MEDENDTIME   
                SQL = SQL + ComNum.VBLF + "'" + po.medDeptCd + "',";   //MEDDEPTCD   
                SQL = SQL + ComNum.VBLF + "'" + po.medDrCd + "',";   //MEDDRCD 
                if (po.opdate != "")
                {
                    SQL = SQL + ComNum.VBLF + "TO_DATE('" + po.opdate + "','YYYY-MM-DD') ,";   //OPDATE    po.OPDATE
                    SQL = SQL + ComNum.VBLF + "'" + po.opdegree + "',";   //OPDEGREE    
                    SQL = SQL + ComNum.VBLF + "'" + po.opdept + "' , ";   //OP_DEPT   
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "'' ,";   //OPDATE    po.OPDATE
                    SQL = SQL + ComNum.VBLF + "'',";   //OPDEGREE    
                    SQL = SQL + ComNum.VBLF + "'' , ";   //OP_DEPT   
                }

                SQL = SQL + ComNum.VBLF + "'" + po.nowdeptcd + "',";   //DEPTCDNOW    
                SQL = SQL + ComNum.VBLF + "'" + po.nowdrcd + "',";   //DRCDNOW    
                SQL = SQL + ComNum.VBLF + "'" + po.nowroomno + "', ";   //ROOM_NO  
                SQL = SQL + ComNum.VBLF + " 0, ";   //ACPNOOUT  


                //2017-03-31 작성당시 과 및 그레이트 의사 전공의 간호사 간호조무사 등 
                if (po.cur_Dept != "" && po.cur_Dept != null)
                {
                    SQL = SQL + ComNum.VBLF + " '" + po.cur_Dept + "'  , ";   //인턴은 입력시 들어오게  작성당시 과 .부서 병동 등    
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " '" + clsType.User.DeptCode + "',   ";   //작성당시 과 .부서 병동 등    
                }
                SQL = SQL + ComNum.VBLF + "     '" + "" + "'   ";   //clsType.User.strJobRght GRADE 의사 간호사 전공의  등     

                if (strSaveFlag == "SAVE")
                {
                    SQL = SQL + ComNum.VBLF + ",'" + clsType.User.IdNumber + "'";   //CODEUSEID
                    SQL = SQL + ComNum.VBLF + ",'" + strCurDate + "'";   //CODEDATE
                    SQL = SQL + ComNum.VBLF + ",'" + strCurTime + "'";   //CODETIME
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + ",'', '', ''";
                }

                SQL = SQL + ComNum.VBLF + " )  ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }
        #endregion

        #region FLOW SAVE
        /// <summary>
        /// 신규 간호 기록 관련 EMR 저장
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pForm"></param>
        /// <param name="AcpEmr">환자정보</param>
        /// <param name="pForm">기록지 정보</param>
        /// <param name="strContent">내용</param>
        /// <returns></returns>
        public static double SaveNurChartFlow(PsmhDb pDbCon, Form ptForm, double dblEmrNoNew, EmrPatient AcpEmr, EmrForm pForm, string strChartDate, string strChartTime, Dictionary<string, string> strContent)
        {

            #region 변수
            //double dblEmrNoNew = 0;
            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;

            string mFLOWGB = string.Empty;
            int mFLOWITEMCNT = 0;
            int mFLOWHEADCNT = 0;

            FormFlowSheet[] mFormFlowSheet = null;
            #endregion

            //clsDB.setBeginTran(pDbCon);

            try
            {
                GetSetDate_AEMRFLOWXML(pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(), ref mFLOWGB, ref mFLOWITEMCNT, ref mFLOWHEADCNT, ref mFormFlowSheet);

                if (mFormFlowSheet == null ||
                    mFormFlowSheet != null && strContent != null && mFormFlowSheet.Length != strContent.Count &&
                    strContent == null || strContent != null && strContent.Count == 0)
                    return dblEmrNoNew;

                DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(pDbCon, "S"));
                double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));
                if (dblEmrNoNew > 0)
                {
                    SaveChartMastHis(pDbCon, dblEmrNoNew.ToString(), dblEmrHisNo, dtpSys.ToString("yyyyMMdd"), dtpSys.ToString("HH:mm"), "C", "");
                }
                else
                {
                    dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                }

                #region //저장 CHRATMAST
                string strSaveFlag = string.Empty;

                if (SaveChartMstOnly(pDbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(),
                                    strChartDate, strChartTime,
                                    clsType.User.IdNumber, clsType.User.IdNumber, "1", "1", "0",
                                    dtpSys.ToString("yyyyMMdd"), dtpSys.ToString("HHmm"), strSaveFlag) == false)
                {
                    //clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBoxEx(ptForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    Cursor.Current = Cursors.Default;
                    return dblEmrNoNew;
                }
                #endregion //저장 CHRATMAST

                #region //CHARTROW
                for (int i = 0; i < mFormFlowSheet.Length; i++)
                {
                    string ItemVal = string.Empty;
                    if (strContent.TryGetValue(mFormFlowSheet[i].ItemCode, out ItemVal))
                    {
                        string ItemType = string.Empty;
                        switch (mFormFlowSheet[i].CellType)
                        {
                            case "TextCellType":
                            case "ComboBoxCellType":
                                ItemType = "TEXT";
                                break;
                            case "CheckBoxCellType":
                                ItemType = "CHECK";
                                break;
                        }

                        SQL = string.Empty;
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                        SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1 )";
                        SQL += ComNum.VBLF + "VALUES (";
                        SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                        SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                        SQL += ComNum.VBLF + "'" + mFormFlowSheet[i].ItemCode + "',";   //ITEMCD
                        SQL += ComNum.VBLF + "'" + mFormFlowSheet[i].ItemCode + "',"; //ITEMNO
                        SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                        SQL += ComNum.VBLF + "'" + ItemType + "',";   //ITEMTYPE
                        SQL += ComNum.VBLF + "'" + ItemVal.Replace("'", "`") + "',";   //ITEMVALUE
                        SQL += ComNum.VBLF + i + ",";   //DSPSEQ
                        SQL += ComNum.VBLF + "NULL";   //ITEMVALUE1
                        SQL += ComNum.VBLF + ")";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pDbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(ptForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                            return dblEmrNoNew;
                        }
                    }
                }

                #endregion //CHARTROW

                //clsDB.setCommitTran(pDbCon);

                #region //전자인증 하기
                if (System.Diagnostics.Debugger.IsAttached == false)
                {
                    bool blnCert = true;
                    if (dblEmrNoNew > 0)
                    {
                        if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == true)
                        {
                            blnCert = SaveEmrCert(pDbCon, dblEmrNoNew, false);
                        }
                    }
                }
                #endregion

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                //clsDB.setRollbackTran(pDbCon);
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(ptForm, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            return dblEmrNoNew;
        }
        #endregion

        #region 전자인증 #
        /// <summary>
        /// EMR 전자 인증을 한다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="dblEmrNoNew"></param>
        /// <returns></returns>
        public static bool SaveEmrCert(PsmhDb pDbCon, double dblEmrNoNew, bool Trans = true)
        {
            DataTable dt = null;
            int i = 0;
            string SQL = "";
            string SqlErr = "";

            double nCertno = 0;
            string strPTNO = string.Empty;
            string strDRSABUN = string.Empty;
            string strEMRDATA = string.Empty;
            string strSIGNDATA = string.Empty;
            string strHASHDATA = string.Empty;
            string strCERTDATA = string.Empty;
            string strSignData = string.Empty;
            string strROWID = string.Empty;

            ComFunc.ReadSysDate(pDbCon);
            string CERTDATE = ComQuery.CurrentDateTime(pDbCon, "D");

            if (Trans)
            {
                clsDB.setBeginTran(pDbCon);
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT C.PTNO, C.FORMNO, C.UPDATENO, C.EMRNO, C.CHARTUSEID, C.ROWID, WRITEDATE  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
                SQL = SQL + ComNum.VBLF + " WHERE C.EMRNO = " + dblEmrNoNew;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    return false;
                }

                strPTNO = dt.Rows[0]["PTNO"].ToString().Trim();
                strEMRDATA = GetNewEmrJsonData(clsDB.DbCon, dt.Rows[i]["FORMNO"].ToString().Trim(), dt.Rows[i]["EMRNO"].ToString().Trim());
                strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                strDRSABUN = dt.Rows[0]["CHARTUSEID"].ToString().Trim().To<int>().ToString("00000");

                if (string.IsNullOrEmpty(strEMRDATA) == true)
                {
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    return false;
                }

                if (strPTNO.Length == 8 && strPTNO.Substring(0, 6).Equals("810000"))
                {
                    if (Trans)
                    {
                        clsDB.setCommitTran(pDbCon);
                    }
                    return true;
                }


                // 전자인증 시퀀스 생성
                SQL = " SELECT " + ComNum.DB_PMPA + "SEQ_CERTPOOL.NEXTVAL  NVAL FROM DUAL  ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    clsDB.SaveSqlErrLog("함수명 : " + "SaveEmrCert" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    nCertno = Double.Parse(dt.Rows[0]["NVAL"].ToString());
                }
                dt.Dispose();
                dt = null;

                // 전자인증
                //if (clsCertWork.CertWorkEx(pDbCon, clsType.User.Sabun.Trim(), strEMRDATA, ref strHASHDATA, ref strCERTDATA) == true)
                //{
                //    if (Trans)
                //    {
                //        clsDB.setRollbackTran(pDbCon);
                //    }
                //    return false;
                //}

                // 전자인증
                if (clsCertWork.CertWorkBacth(pDbCon, strDRSABUN, CERTDATE, clsCertWork.AEMRCHARTMST, strPTNO, strEMRDATA, ref strHASHDATA, ref strSignData, ref strCERTDATA, ref nCertno) != "OK")
                {
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    return false;
                }

                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_EMR, clsCertWork.AEMRCHARTMST, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strROWID) == false)
                {
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    return false;
                }

                if (Trans)
                {
                    clsDB.setCommitTran(pDbCon);
                }

                return true;
            }
            catch (Exception ex)
            {
                if (Trans)
                {
                    clsDB.setRollbackTran(pDbCon);
                }
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장                
                return false;
            }
        }


        public static string GetNewEmrJsonData(PsmhDb pDbCon, string strFormNo, string strEmrNo)
        {
            string rtnVal = "";
            OracleDataReader reader = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string quote = "\"";

            try
            {
                if (strFormNo.Equals("1568"))
                {
                    // 1568 : 마취기록지
                    SQL = "";
                    SQL += "WITH TEMP_TABLE AS( \r";
                    SQL += "SELECT \r";
                    SQL += "    A.EMRNO, \r";
                    SQL += "    '{' || '\"ITEMCD\"' || ':\"' || B.TITLE || '\", ' || '\"ITEMVALUE\"' || ':\"' || B.CONTROLVALUE || '\"}' AS ITEM \r";
                    SQL += "FROM KOSMOS_EMR.AEMRCHARTMST A \r";
                    SQL += "INNER JOIN KOSMOS_EMR.AEMRANCHART B \r";
                    SQL += "   ON A.EMRNO = B.EMRNO \r";
                    SQL += "  AND A.EMRNOHIS = B.EMRNOHIS \r";
                    SQL += "WHERE A.EMRNO = " + strEmrNo + "    \r";
                    SQL += ") \r";
                    SQL += "SELECT WM_CONCAT(ITEM) JSON \r";
                    SQL += "  FROM TEMP_TABLE \r";
                    SQL += " GROUP BY EMRNO \r";
                }
                else if (strFormNo.Equals("965") || strFormNo.Equals("2049") || strFormNo.Equals("2137"))
                {
                    // 965 : 간호기록지
                    // 2049 : 응급 간호기록지(ER)
                    // 2137 : 회복실 간호기록지(RE)
                    SQL = "";
                    SQL += "SELECT 'WARDCODE : ' || WARDCODE || ' | ' || \r";
                    SQL += "       'ROOMCODE : ' || ROOMCODE || ' | ' || \r";
                    SQL += "       'PROBLEMCODE : ' || PROBLEMCODE || ' | ' || \r";
                    SQL += "       'PROBLEM : ' || PROBLEM || ' | ' || \r";
                    SQL += "       'TYPE : ' || TYPE || ' | ' || \r";
                    SQL += "       'NRRECODE : ' || NRRECODE AS JSON \r";
                    SQL += " FROM KOSMOS_EMR.AEMRCHARTMST A \r";
                    SQL += "INNER JOIN KOSMOS_EMR.AEMRNURSRECORD B \r";
                    SQL += "   ON A.EMRNO = B.EMRNO \r";
                    SQL += "WHERE A.EMRNO = " + strEmrNo + "    \r";
                }
                else if (strFormNo.Equals("1575"))
                {
                    SQL = "";
                    SQL += "WITH TEMP_TABLE AS( \r";
                    SQL += "SELECT \r";
                    SQL += "    A.EMRNO, \r";
                    SQL += "    TO_CLOB('{' || '\"ITEMCD\"' || ':\"' || B.ITEMCD || '\", ' || '\"ITEMVALUE\"' || ':\"' || B.ITEMVALUE || '\", ' || '\"ITEMVALUE1\"' || ':\"' || B.ITEMVALUE1 || '\", ITEMVALUE2\"' || ':\"' || B.ITEMVALUE2 || '\"}') AS ITEM \r";
                    SQL += "FROM KOSMOS_EMR.AEMRCHARTMST A \r";
                    SQL += "INNER JOIN KOSMOS_EMR.AEMRCHARTROW B \r";
                    SQL += "   ON A.EMRNO = B.EMRNO \r";
                    SQL += "  AND A.EMRNOHIS = B.EMRNOHIS \r";
                    SQL += "  AND B.ITEMCD > CHR(0) \r";
                    SQL += "WHERE A.EMRNO = " + strEmrNo + "    \r";
                    SQL += "ORDER BY B.DSPSEQ \r";
                    SQL += ") \r";
                    SQL += "SELECT WM_CONCAT(ITEM) JSON \r";
                    SQL += "  FROM TEMP_TABLE \r";
                    SQL += " GROUP BY EMRNO \r";
                }
                else
                {
                    #region 4천바이트 넘는게 있는지 확인
                    SQL = "";
                    SQL += "WITH TEMP_TABLE AS(                                                                                     \r";
                    SQL += "SELECT  LENGTHB(B.ITEMVALUE) SIZE1                                                                      \r";
                    SQL += "    ,   LENGTHB(B.ITEMVALUE1) SIZE2                                                                     \r";
                    SQL += "    ,   LENGTHB(B.ITEMVALUE2) SIZE3                                                                     \r";
                    SQL += "    ,   B.ITEMCD                                                                                        \r";
                    SQL += "    ,   B.ITEMVALUE                                                                                     \r";
                    SQL += "    ,   B.ITEMVALUE1                                                                                    \r";
                    SQL += "    ,   B.ITEMVALUE2                                                                                    \r";
                    SQL += "FROM KOSMOS_EMR.AEMRCHARTMST A                                                                          \r";
                    SQL += "INNER JOIN KOSMOS_EMR.AEMRCHARTROW B                                                                    \r";
                    SQL += "   ON A.EMRNO = B.EMRNO                                                                                 \r";
                    SQL += "  AND A.EMRNOHIS = B.EMRNOHIS                                                                           \r";
                    SQL += "  AND B.ITEMCD > CHR(0)                                                                                 \r";
                    SQL += "WHERE A.EMRNO = " + strEmrNo + "                                                                        \r";
                    SQL += ")                                                                                                       \r";
                    SQL += "  SELECT                                                                                                \r";
                    SQL += "          ITEMCD                                                                                        \r";
                    SQL += "    ,     ITEMVALUE                                                                                     \r";
                    SQL += "    ,     ITEMVALUE1                                                                                    \r";
                    SQL += "    ,     ITEMVALUE2                                                                                    \r";
                    SQL += "    FROM TEMP_TABLE                                                                                     \r";
                    SQL += "   WHERE (NVL(SIZE1, 0) + NVL(SIZE2, 0)  + NVL(SIZE3, 0)) >= 3500                                       \r";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        return rtnVal;
                    }
                    #endregion


                    if (reader.HasRows)
                    {
                        StringBuilder builder = new StringBuilder();
                        while (reader.Read())
                        {
                            builder.Append("{").Append(quote).Append("ITEMCD").Append(quote).Append(":");
                            builder.Append(quote).Append(reader.GetValue(0).ToString().Trim()).Append(quote).Append(", ");

                            builder.Append(quote).Append("ITEMVALUE").Append(quote).Append(":");
                            builder.Append(quote).Append(reader.GetValue(1).ToString().Trim()).Append(quote).Append(", ");

                            builder.Append(quote).Append("ITEMVALUE1").Append(quote).Append(":");
                            builder.Append(quote).Append(reader.GetValue(2).ToString().Trim()).Append(quote).Append(", ");

                            builder.Append(quote).Append("ITEMVALUE2").Append(quote).Append(":");
                            builder.Append(quote).Append(reader.GetValue(3).ToString().Trim()).Append(quote).Append("}");
                        }

                        rtnVal = builder.ToString().Trim();

                    }
                    else
                    {
                        reader.Dispose();
                        reader = null;

                        SQL = "";
                        SQL += "WITH TEMP_TABLE AS( \r";
                        SQL += "SELECT \r";
                        SQL += "    A.EMRNO, \r";
                        SQL += "    TO_CLOB('{' || '\"ITEMCD\"' || ':\"' || B.ITEMCD || '\", ' || '\"ITEMVALUE\"' || ':\"' || B.ITEMVALUE || '\"}') AS ITEM \r";
                        SQL += "FROM KOSMOS_EMR.AEMRCHARTMST A \r";
                        SQL += "INNER JOIN KOSMOS_EMR.AEMRCHARTROW B \r";
                        SQL += "   ON A.EMRNO = B.EMRNO \r";
                        SQL += "  AND A.EMRNOHIS = B.EMRNOHIS \r";
                        SQL += "  AND B.ITEMCD > CHR(0) \r";
                        SQL += "WHERE A.EMRNO = " + strEmrNo + "    \r";
                        SQL += "ORDER BY B.DSPSEQ \r";
                        SQL += ") \r";
                        SQL += "SELECT WM_CONCAT(ITEM) JSON \r";
                        SQL += "  FROM TEMP_TABLE \r";
                        SQL += " GROUP BY EMRNO \r";
                    }
                }

                if (rtnVal.IsNullOrEmpty())
                {
                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        return rtnVal;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        //rtnVal = dt.Rows[0]["JSON"].ToString().Trim();
                        rtnVal = reader.GetValue(0).ToString().Trim();
                        //rtnVal = rtnVal.Substring(0, rtnVal.Length > 3000 ? 3000 : rtnVal.Length);
                    }
                }

                if (reader != null)
                {
                    reader.Dispose();
                    reader = null;
                }

            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }
        #endregion

        #region SetEmrForm
        /// <summary>
        /// FORMNO로 최신 기록지 정보를 가지고 온다
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <returns></returns>
        public static EmrForm SerEmrFormUpdateNo(PsmhDb pDbCon, string strFormNo)
        {

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            EmrForm f = new EmrForm();

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + "    FORMNO         ,  UPDATENO       ,  OLDGB";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFORM ";
                SQL = SQL + ComNum.VBLF + "      WHERE FORMNO = " + VB.Val(strFormNo);
                SQL = SQL + ComNum.VBLF + "        AND UPDATENO = (SELECT MAX(UPDATENO) FROM " + ComNum.DB_EMR + "AEMRFORM ";
                SQL = SQL + ComNum.VBLF + "                            WHERE FORMNO = " + VB.Val(strFormNo);
                //if (clsCompuInfo.gstrCOMIP.Equals("192.168.0.110") == false)
                //{
                    SQL = SQL + ComNum.VBLF + "                              AND USECHECK = 1 --사용체크 한것";
                //}
                SQL = SQL + ComNum.VBLF + "                       )";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return null;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return null;
                }

                f.FmFORMNO = (long)VB.Val(dt.Rows[0]["FORMNO"].ToString().Trim());
                f.FmUPDATENO = (int)VB.Val(dt.Rows[0]["UPDATENO"].ToString().Trim());
                f.FmOLDGB = (int)VB.Val(dt.Rows[0]["OLDGB"].ToString().Trim());
                dt.Dispose();
                dt = null;
                return f;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
        }
        #endregion

        #region SetOcsInfo
        /// <summary>
        /// OCS 접수정보로 조회
        /// </summary>
        /// <param name="strPTNO"></param>
        /// <param name="strInOutCls"></param>
        /// <param name="strMedFrDate"></param>
        /// <param name="strMedDeptCd"></param>
        /// <returns></returns>
        public static EmrPatient SetEmrPatInfoOcs(PsmhDb pDbCon, string strPTNO, string strInOutCls, string strMedFrDate, string strMedDeptCd = "")
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            EmrPatient p = new EmrPatient();

            try
            {
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    ACPNO, ";
                SQL = SQL + ComNum.VBLF + "    PTNO, ";
                SQL = SQL + ComNum.VBLF + "    PTNAME,  ";
                SQL = SQL + ComNum.VBLF + "    SSNO1, ";
                SQL = SQL + ComNum.VBLF + "    SSNO2, ";
                SQL = SQL + ComNum.VBLF + "    TEL, ";
                SQL = SQL + ComNum.VBLF + "    CELPHON, ";
                SQL = SQL + ComNum.VBLF + "    ZIPCD, ";
                SQL = SQL + ComNum.VBLF + "    ADDR1, ";
                SQL = SQL + ComNum.VBLF + "    BIRTHDATE, ";
                SQL = SQL + ComNum.VBLF + "    ADDRESS, ";
                SQL = SQL + ComNum.VBLF + "    ZIPCDLOAD, ";
                SQL = SQL + ComNum.VBLF + "    ADDRLOAD, ";
                SQL = SQL + ComNum.VBLF + "    ADDRESSLOAD, ";
                SQL = SQL + ComNum.VBLF + "    INOUTCLS, ";
                SQL = SQL + ComNum.VBLF + "    IP_INDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDFRDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDFRTIME, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDTIME, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDEXDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDDEPTCD, ";
                SQL = SQL + ComNum.VBLF + "    MEDDRCD, ";
                SQL = SQL + ComNum.VBLF + "    WARD_DATE, ";
                SQL = SQL + ComNum.VBLF + "    WARD_TIME, ";
                SQL = SQL + ComNum.VBLF + "    CNCLYN, ";
                SQL = SQL + ComNum.VBLF + "    MEDDEPTKORNAME, ";
                SQL = SQL + ComNum.VBLF + "    MEDDRNAME, ";
                SQL = SQL + ComNum.VBLF + "    WARD, ";
                SQL = SQL + ComNum.VBLF + "    ROOM, ";
                SQL = SQL + ComNum.VBLF + "    REMARK, ";
                SQL = SQL + ComNum.VBLF + "    ADDR, ";
                SQL = SQL + ComNum.VBLF + "    acpnoin, ";
                SQL = SQL + ComNum.VBLF + "    BI -- 접수시 BI";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AVIEWACPOCS ";
                SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + strPTNO + "'";
                SQL = SQL + ComNum.VBLF + "     AND INOUTCLS = '" + strInOutCls + "'";
                SQL = SQL + ComNum.VBLF + "     AND MEDFRDATE = '" + strMedFrDate.Replace("-", "") + "'";
                if (strMedDeptCd != "")
                {
                    SQL = SQL + ComNum.VBLF + "     AND MEDDEPTCD = '" + strMedDeptCd + "'";
                }
                //SQL = SQL + ComNum.VBLF + "     AND MEDDRCD = '" + p.medDrCd + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return null;
                }

                p.acpNo = dt.Rows[0]["ACPNO"].ToString().Trim();
                if (p.acpNo.Trim() == "")
                {
                    p.acpNo = "0";
                }
                p.ptNo = dt.Rows[0]["PTNO"].ToString().Trim();
                p.ptName = dt.Rows[0]["PTNAME"].ToString().Trim();
                p.ssno1 = dt.Rows[0]["SSNO1"].ToString().Trim();
                p.ssno2 = dt.Rows[0]["SSNO2"].ToString().Trim();
                p.birthdate = dt.Rows[0]["BIRTHDATE"].ToString().Trim();
                p.sex = ComFunc.SexCheck(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO2"].ToString().Trim(), "1");
                p.age = Convert.ToString(ComFunc.AgeCalcX1(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO2"].ToString().Trim(), ComQuery.CurrentDateTime(pDbCon, "D")));

                p.tel = dt.Rows[0]["TEL"].ToString().Trim();
                p.celphno = dt.Rows[0]["CELPHON"].ToString().Trim();
                p.zipcd = dt.Rows[0]["ZIPCD"].ToString().Trim();
                p.addr = dt.Rows[0]["ADDR"].ToString().Trim();
                //TODO 도로명 주소 해결
                //p.address = GetNewAddr(pDbCon, dt.Rows[0]["JI_CODE"].ToString().Trim(), dt.Rows[0]["ADDR"].ToString().Trim(), dt.Rows[0]["JUSO"].ToString().Trim());
                p.zipcdLoad = dt.Rows[0]["ZIPCDLOAD"].ToString().Trim();
                p.addrLoad = dt.Rows[0]["ADDRLOAD"].ToString().Trim();
                p.addressLoad = dt.Rows[0]["ADDRESSLOAD"].ToString().Trim();
                p.inOutCls = dt.Rows[0]["INOUTCLS"].ToString().Trim();

                #region //신규데이타일 경우
                //2016-07-17 병동입원일자로 사용함 
                p.medFrDate = dt.Rows[0]["MEDFRDATE"].ToString().Trim();

                //2016-07-17  실제로 입원일자를 사용 함 
                p.medIP_INDATE = dt.Rows[0]["IP_INDATE"].ToString().Trim();

                p.medFrTime = dt.Rows[0]["MEDFRTIME"].ToString().Trim();
                p.medEndDate = dt.Rows[0]["MEDENDDATE"].ToString().Trim();
                if (dt.Rows[0]["MEDENDTIME"].ToString().Trim() == "000000")
                {
                    p.medEndTime = "";
                }
                else
                {
                    if (dt.Rows[0]["MEDENDTIME"].ToString().Trim().Length == 4)
                    {
                        p.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim() + "00";
                    }
                    else
                    {
                        p.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim();
                    }
                }
                p.medEndexDate = dt.Rows[0]["MEDENDDEXDATE"].ToString().Trim();
                p.medDeptCd = dt.Rows[0]["MEDDEPTCD"].ToString().Trim();
                p.medDrCd = dt.Rows[0]["MEDDRCD"].ToString().Trim();
                p.medDeptKorName = dt.Rows[0]["MEDDEPTKORNAME"].ToString().Trim();
                p.medDrName = dt.Rows[0]["MEDDRNAME"].ToString().Trim();
                #endregion //신규데이타일 경우

                p.ward = dt.Rows[0]["WARD"].ToString().Trim();
                p.room = dt.Rows[0]["ROOM"].ToString().Trim();
                p.remark = dt.Rows[0]["REMARK"].ToString().Trim();
                p.wardDate = dt.Rows[0]["WARD_DATE"].ToString().Trim();
                p.wardTime = dt.Rows[0]["WARD_TIME"].ToString().Trim();

                p.bi = "";
                p.biname = "";
                //p.opdate = "";
                //p.opdegree = "";

                p.acpNoOut = "0";
                p.acpNoIn = dt.Rows[0]["acpnoin"].ToString().Trim();
                p.bi = dt.Rows[0]["BI"].ToString().Trim();
                p.biname = clsVbfunc.GetBiName(dt.Rows[0]["BI"].ToString().Trim());

                dt.Dispose();
                dt = null;

                //TODO 환자정보 기타
                //if (p.inOutCls == "I")
                //{
                //    GetMedFrTime(pDbCon, p.ptNo, p.medIP_INDATE, ref p.medFrTime, ref p.medFrDate);
                //}

                //GetRoom(pDbCon, p.inOutCls, p.ptNo, p.medIP_INDATE, ref p.ward, ref p.room);
                //GetPtBi(pDbCon, p.inOutCls, p.ptNo, p.medIP_INDATE, p.medDeptCd, ref p.bi, ref p.biname);
                //GetMOTHER(pDbCon, p.ptNo, p.medIP_INDATE, ref p.MomName, ref p.DadName);


                return p;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
        }
        #endregion

        #region //AEMRFLOWXML

        /// <summary>
        /// AEMRFLOWXML 조회
        /// </summary>
        /// <param name="pFormNo"></param>
        /// <param name="pUpdateNo"></param>
        /// <returns></returns>
        public static string Query_AEMRFLOWXML(string pFormNo, string pUpdateNo)
        {
            string strQuery = "";

            string SQL = "";    //Query문

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT                                     ";
            SQL = SQL + ComNum.VBLF + "   FORMNO, UPDATENO, ITEMNUMBER,           ";
            SQL = SQL + ComNum.VBLF + "   ITEMNO, ITEMNAME, CELLTYPE,             ";
            SQL = SQL + ComNum.VBLF + "   HALIGN, VALIGN, SIZEWIDTH,              ";
            SQL = SQL + ComNum.VBLF + "   SIZEHEIGHT, MULTILINE, CHECKALIGN,      ";
            SQL = SQL + ComNum.VBLF + "   USERMCRO, USERFUNC, USERFUNCNM          ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFLOWXML      ";
            SQL = SQL + ComNum.VBLF + "WHERE FORMNO =  " + pFormNo;
            SQL = SQL + ComNum.VBLF + "     AND UPDATENO =  " + pUpdateNo;
            SQL = SQL + ComNum.VBLF + "ORDER BY FORMNO, UPDATENO, ITEMNUMBER";

            strQuery = SQL;
            return strQuery;
        }

        /// <summary>
        /// AEMRFLOWHEADXML 조회
        /// </summary>
        /// <param name="pFormNo"></param>
        /// <param name="pUpdateNo"></param>
        /// <returns></returns>
        public static string Query_AEMRFLOWHEADXML(string pFormNo, string pUpdateNo)
        {
            string strQuery = "";

            string SQL = "";    //Query문

            SQL = SQL + ComNum.VBLF + "SELECT                                             ";
            SQL = SQL + ComNum.VBLF + "   FORMNO, UPDATENO, ITEMNUMBER,                   ";
            SQL = SQL + ComNum.VBLF + "   HEADNUMBER, HEADTEXT, HROW,                     ";
            SQL = SQL + ComNum.VBLF + "   HCOL, VROW, VCOL,                               ";
            SQL = SQL + ComNum.VBLF + "   SIZEWIDTH, SIZEHEIGHT, MULTILINE,               ";
            SQL = SQL + ComNum.VBLF + "   FONTNAME, FONTSIZE, FONTBOLD,                   ";
            SQL = SQL + ComNum.VBLF + "   HALIGN, VALIGN, SPANROW,                        ";
            SQL = SQL + ComNum.VBLF + "   SPANCOL                                         ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFLOWHEADXML          ";
            SQL = SQL + ComNum.VBLF + "WHERE FORMNO =  " + pFormNo;
            SQL = SQL + ComNum.VBLF + "     AND UPDATENO =  " + pUpdateNo;
            SQL = SQL + ComNum.VBLF + "ORDER BY FORMNO, UPDATENO, ITEMNUMBER, HEADNUMBER";
            strQuery = SQL;
            return strQuery;
        }

        public static void GetSetDate_AEMRFLOWXML(string pFormNo, string pUpdateNo, ref string pFLOWGB, ref int pFLOWITEMCNT, ref int pFLOWHEADCNT,
                                                 ref FormFlowSheet[] pFormFlowSheet)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  ";
                SQL = SQL + ComNum.VBLF + "     FLOWGB, FLOWITEMCNT, FLOWHEADCNT, FLOWINPUTSIZE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFORM ";
                SQL = SQL + ComNum.VBLF + "WHERE FORMNO = " + pFormNo;
                SQL = SQL + ComNum.VBLF + "     AND UPDATENO = " + pUpdateNo;
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }
                pFLOWGB = dt.Rows[i]["FLOWGB"].ToString().Trim();
                pFLOWITEMCNT = (int)VB.Val(dt.Rows[i]["FLOWITEMCNT"].ToString().Trim());
                pFLOWHEADCNT = (int)VB.Val(dt.Rows[i]["FLOWHEADCNT"].ToString().Trim());
                dt.Dispose();
                dt = null;

                pFormFlowSheet = new FormFlowSheet[pFLOWITEMCNT];

                SQL = "";
                SQL = Query_AEMRFLOWXML(pFormNo, pUpdateNo);

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }


                for (i = 0; i < pFLOWITEMCNT; i++)
                {
                    int intItemNum = (int)VB.Val(dt.Rows[i]["ITEMNUMBER"].ToString().Trim());
                    pFormFlowSheet[intItemNum] = new FormFlowSheet();
                    pFormFlowSheet[intItemNum].ItemCode = dt.Rows[i]["ITEMNO"].ToString().Trim();
                    pFormFlowSheet[intItemNum].CellType = dt.Rows[i]["CELLTYPE"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch
            {

            }
        }


        #endregion //AEMRFLOWXML
    }
}
