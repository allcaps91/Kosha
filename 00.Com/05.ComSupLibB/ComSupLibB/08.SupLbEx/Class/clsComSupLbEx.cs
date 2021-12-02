using ComBase; //기본 클래스
using ComDbB; //DB연결
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name : ComSupLibB.SupLbEx
    /// File Name : clsLbEx.cs
    /// Title or Description : 진단검사의학과 Biz 
    /// Author : 김홍록  
    /// Create Date : 2017-05-15 
    /// Update History : 
    /// </summary> 
    public class clsComSupLbEx : Com.clsMethod
    {
        string SQL                      = string.Empty;
        int intRowAffected              = 0; //변경된 Row 받는 변수

        DateTime sysdate                = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-"));

        clsComSupLbExSQL    lbExSQL     = new clsComSupLbExSQL();
        clsComSupLbExOUTSQL lbExOUTSQL  = new clsComSupLbExOUTSQL();
        clsComSupLbExCVSQL lbExCVSQL    = new clsComSupLbExCVSQL();
        clsMethod method                = new clsMethod();
        clsComSQL comSql                = new clsComSQL();


        clsLbExBarCodePrint lbExBARCODE = new clsLbExBarCodePrint(); 

        /// <summary>검체기본정보</summary>
        public DataTable gDtSpecCode;

        public List<string> set_EXAM_DEPT()
        {
            List<string> arr = new List<string>();

            DataTable dt = comSql.sel_BAS_WARD_COMBO(clsDB.DbCon);

            arr.Add("LIS.진단검사");

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    arr.Add(dt.Rows[i][0].ToString().Trim());
                }
            }

            arr.Add("HD.인공신장실");
            arr.Add("ER.응급실");
            arr.Add("OP.수술실");
            arr.Add("OB.산부인과");
            arr.Add("GS.일반외과");

            arr.Add("ENDO.내시경실");
            arr.Add("ENHR.신검");
            arr.Add("ENTO.종검");
            arr.Add("XRAY.영상");
            
            return arr;
        }

        public void viewEXAM_OUT_IMG(string strWRTNO)
        {
            DataTable dt = lbExOUTSQL.sel_EXAM_RESULT_IMG(clsDB.DbCon, strWRTNO);

            delDirctoryFile(EXAM_IMG_TMP_PATH);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    method.setImage(EXAM_IMG_TMP_PATH + "EXAM" + Convert.ToInt32(dt.Rows[i][(int)clsComSupLbExOUTSQL.enmSel_EXAM_RESULT_IMG.SEQNO].ToString()).ToString() + ".jpg", dt, i, (int)clsComSupLbExOUTSQL.enmSel_EXAM_RESULT_IMG.IMAGE);                    
                }
            }

            
            method.runImag(EXAM_IMG_TMP_PATH + "EXAM" + Convert.ToInt32(dt.Rows[0][(int)clsComSupLbExOUTSQL.enmSel_EXAM_RESULT_IMG.SEQNO].ToString()).ToString() + ".jpg");

        }

        /// <summary>검사오더</summary>
        public clsComSupLbEx()
        {
            BASIC_CODE_SET(clsDB.DbCon);
        }

        public int READ_BLOOD_AMT()
        {
            int n = 0;




            return n;
        }

        /// <summary>검체기본정보에 대한 생성자</summary>
        public void BASIC_CODE_SET(PsmhDb pDbCon)
        {
            gDtSpecCode = lbExSQL.sel_EXAM_SPECODE_Code(pDbCon);
        }
      
        public string Exam_SMS_SEND(PsmhDb pDbCon, string strSPECNO)
        {
            DataTable dt;
            string SqlErr = "";

            dt = lbExCVSQL.sel_EXAM_SMS(pDbCon, strSPECNO);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                try
                {
                    for (int i =  0; i < dt.Rows.Count; i++)
                    {

                        SqlErr = lbExCVSQL.ins_ETC_SMS(
                                      pDbCon
                                    , dt.Rows[i][clsComSupLbExCVSQL.enmSel_EXAM_SMS.PANO.ToString()].ToString().Trim()
                                    , dt.Rows[i][clsComSupLbExCVSQL.enmSel_EXAM_SMS.SEND_TEL.ToString()].ToString().Trim()
                                    , dt.Rows[i][clsComSupLbExCVSQL.enmSel_EXAM_SMS.EXAM_TEL.ToString()].ToString().Trim()
                                    , "N"                                    
                                    , dt.Rows[i][clsComSupLbExCVSQL.enmSel_EXAM_SMS.MSG.ToString()].ToString().Trim()
                                    , ref intRowAffected);

                        if (SqlErr != "")
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
            string strSMSOK = lbExSQL.sel_OCS_DOCTOR(pDbCon, strSabun);
            string SqlErr = "";

            //2019-02-14 전담간호사인 경우 SMSOK 구분이 없으므로 강제 설정함
            string strMSTel = lbExSQL.sel_NURSE_MSTEL(pDbCon, strSabun);

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
                    SqlErr = lbExCVSQL.ins_ETC_SMS(pDbCon, strPano, strHpone, strTel, "68", strMsg,ref intRowAffected);

                    if (string.IsNullOrEmpty(SqlErr) == false)
                    {
                        return SqlErr;
                    }

                    // 2018.03.14.김홍록 : isMid는 중간 결과 
                    if (isMid == false)
                    {
                        SqlErr = lbExCVSQL.ins_EXAM_RESULTC_CV(pDbCon, strGbn, strRowId, ref intRowAffected);

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
                SqlErr = lbExCVSQL.ins_EXAM_RESULTC_CV_3(pDbCon, strSpecNo, ref intRowAffected);

                if (SqlErr != "")
                {
                    return SqlErr;
                }

                //2. isMid = false 일때만 CV ='C' 주치의에게
                dt = lbExCVSQL.sel_EXAM_RESUlTC_CV_MSG(pDbCon, strSpecNo, strMsg, isMid, isSMS); 

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SqlErr = Exam_SMS_SEND_CV_SMS(pDbCon
                                            , "1"  // 주치의
                                            , dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.ROWID.ToString()].ToString().Trim()
                                            , dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.SABUN.ToString()].ToString().Trim()
                                            , dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.PANO.ToString()].ToString().Trim()
                                            , dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.HTEL.ToString()].ToString().Trim()
                                            , dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.TEL.ToString()].ToString().Trim()
                                            , dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.MSG.ToString()].ToString().Trim()
                                            , isMid
                                            );
                        if (SqlErr != "")
                        {
                            return SqlErr;
                        }

                        dtDrSch = lbExSQL.sel_OCS_DOCTOR_SCH(pDbCon, dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.SABUN.ToString()].ToString().Trim());

                        if (dtDrSch != null && dtDrSch.Rows.Count > 0)
                        {
                            for(int z = 0; z < dtDrSch.Rows.Count; z++) //2019-04-29, 김해수 , CVR 문자 추가 작업
                            {
                                SqlErr = Exam_SMS_SEND_CV_SMS(pDbCon
                                              , "2"   // 전공의
                                              , dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.ROWID.ToString()].ToString().Trim()
                                              , dtDrSch.Rows[z][clsComSupLbExSQL.enmSelOcsDoctorSch.SETSABUN.ToString()].ToString().Trim()
                                              , dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.PANO.ToString()].ToString().Trim()
                                              , dtDrSch.Rows[z][clsComSupLbExSQL.enmSelOcsDoctorSch.HTEL.ToString()].ToString().Trim()
                                              , dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.TEL.ToString()].ToString().Trim()
                                              , dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.MSG.ToString()].ToString().Trim()
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
                            SqlErr = lbExCVSQL.ins_ETC_SMS(pDbCon, dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.PANO.ToString()].ToString().Trim()
                                            , "01066052045", "054-260-8261", "N", dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.MSG.ToString()].ToString().Trim(),ref intRowAffected);
                            if (SqlErr != "")
                            {
                                return SqlErr;
                            }

                            //감염 고수현
                            lbExCVSQL.ins_ETC_SMS(pDbCon, dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.PANO.ToString()].ToString().Trim()
                                            , "01027764163", "054-260-8261", "N", dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.MSG.ToString()].ToString().Trim(), ref intRowAffected);

                            if (SqlErr != "")
                            {
                                return SqlErr;
                            }

                            //감염 박수진   
                            lbExCVSQL.ins_ETC_SMS(pDbCon, dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.PANO.ToString()].ToString().Trim()
                                            , "01028170176", "054-260-8261", "N", dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.MSG.ToString()].ToString().Trim(), ref intRowAffected);

                            if (SqlErr != "")
                            {
                                return SqlErr;
                            }
                             
                            //감염 정지미   
                            lbExCVSQL.ins_ETC_SMS(pDbCon, dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.PANO.ToString()].ToString().Trim()
                                            , "01065827819", "054-260-8261", "N", dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.MSG.ToString()].ToString().Trim(), ref intRowAffected);

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

        /// <summary>Exam_SMS_SEND_AFB</summary>
        /// <param name="strSpecNo"></param>
        /// <param name="strPano"></param>
        public bool Exam_SMS_SEND_AFB(PsmhDb pDbCon, string strSpecNo,string strPano,string strResult = "")
        {
            DataTable dt;
            DataTable dtDrSch;

            bool b = true;

            if (strResult == "")
            {
                dt = lbExCVSQL.sel_EXAM_RESUlTC_CV_AFB(pDbCon, strSpecNo);
            }
            else
            {
                dt = lbExCVSQL.sel_EXAM_RESUlTC_CV_AFB(pDbCon, strSpecNo,strResult);
            }

            //clsDB.setBeginTran(pDbCon);
            string SqlErr = "";

            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        //호흡기결핵일 경우 결핵코디네이터(김나현)에게도 문자 전송
                        SqlErr = lbExCVSQL.ins_ETC_SMS(pDbCon, strPano, "01096859026", "054-260-8261", "N", dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.MSG.ToString()].ToString(), ref intRowAffected);

                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                            return false ;
                        }

                        //감염관리담당자
                        SqlErr = lbExCVSQL.ins_EXAM_SMSSEND(pDbCon, dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.SPECNO.ToString()].ToString()
                                        , strPano
                                        , dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.MSG.ToString()].ToString()
                                        , "01096859026"
                                        , "30260"
                                        ,  ref intRowAffected);

                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                            return false;
                        }

                        if (string.IsNullOrEmpty(dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.HTEL.ToString()].ToString()) == false)
                        {
                            SqlErr = lbExCVSQL.ins_ETC_SMS(pDbCon, strPano, dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.HTEL.ToString()].ToString(), "054-260-8261", "N", dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.MSG.ToString()].ToString(),  ref intRowAffected);

                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                                return false;
                            }

                            SqlErr = lbExCVSQL.ins_EXAM_SMSSEND(pDbCon, dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.SPECNO.ToString()].ToString()
                                            , strPano
                                            , dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.MSG.ToString()].ToString()
                                            , dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.HTEL.ToString()].ToString()
                                            , dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.SABUN.ToString()].ToString()
                                            ,  ref intRowAffected);
                            if (SqlErr != "")
                            {
                                // clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                                return false;
                            }
                        }

                        if (dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.IPDOPD.ToString()].ToString() == "I")
                        {
                            dtDrSch = lbExSQL.sel_OCS_DOCTOR_SCH(pDbCon, dt.Rows[i][clsComSupLbExCVSQL.enmSelExamResultcCvMsg.SABUN.ToString()].ToString().Trim());

                            if (dtDrSch != null && dtDrSch.Rows.Count > 0)
                            {
                                //2019-04-19, 김해수, 1명 문자 발송 부분 수정작업 의사스케줄 등록되어있는 사람 다가게.
                                for (int h = 0; h < dtDrSch.Rows.Count; h++)
                                {
                                    SqlErr = lbExCVSQL.ins_ETC_SMS(pDbCon, strPano, dtDrSch.Rows[h][clsComSupLbExSQL.enmSelOcsDoctorSch.HTEL.ToString()].ToString().Trim(), "054-260-8261", "N", dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.MSG.ToString()].ToString(), ref intRowAffected);

                                    if (SqlErr != "")
                                    {
                                        // clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                        return false;
                                    }
                                }

                                SqlErr = lbExCVSQL.ins_EXAM_SMSSEND(pDbCon, dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.SPECNO.ToString()].ToString()
                                                    , strPano
                                                    , dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.MSG.ToString()].ToString()
                                                    , dtDrSch.Rows[i][clsComSupLbExSQL.enmSelOcsDoctorSch.HTEL.ToString()].ToString().Trim()
                                                    , dtDrSch.Rows[i][clsComSupLbExSQL.enmSelOcsDoctorSch.SETSABUN.ToString()].ToString()
                                                    , ref intRowAffected);

                                if (SqlErr != "")
                                {
                                    //clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return false;
                                }


                                //TODO : 2017.05.30.김홍록: 왜 2번 보내는지 모르겠다.

                                //2019-04-19, 김해수, 중복 소스 주석처리
                                //SqlErr = lbExCVSQL.ins_ETC_SMS(pDbCon, strPano, dtDrSch.Rows[i][clsComSupLbExSQL.enmSelOcsDoctorSch.HTEL.ToString()].ToString().Trim(), "054-260-8261", "N", dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.MSG.ToString()].ToString(), ref intRowAffected);

                                //if (SqlErr != "")
                                //{
                                //    //clsDB.setRollbackTran(clsDB.DbCon);
                                //    ComFunc.MsgBox(SqlErr);
                                //    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                                //    return false;
                                //}

                                //SqlErr = lbExCVSQL.ins_EXAM_SMSSEND(pDbCon, dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.SPECNO.ToString()].ToString()
                                //                , strPano
                                //                , dt.Rows[i][clsComSupLbExSQL.enmSelExamResultcCvAfb.MSG.ToString()].ToString()
                                //                , dtDrSch.Rows[i][clsComSupLbExSQL.enmSelOcsDoctorSch.HTEL.ToString()].ToString().Trim()
                                //                , dtDrSch.Rows[i][clsComSupLbExSQL.enmSelOcsDoctorSch.SETSABUN.ToString()].ToString()
                                //                , ref intRowAffected);

                                //if (SqlErr != "")
                                //{
                                //    //clsDB.setRollbackTran(clsDB.DbCon);
                                //    ComFunc.MsgBox(SqlErr);
                                //    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                                //    return false;
                                //}
                            }
                        }
                    }                    
                }
            }

            catch (Exception ex)
            {

                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }

            return b;
        }

        public bool EXAM_INFECTMASTER(PsmhDb pDbCon, string strPano, string strBi, string strResult, string strRowId, string strSubCode, string strSpecNo)
        {

            string strOk = "OK";
            string SqlErr = "";
            string strVDRL = "";
            string strHCV_IGG = "";
            string strHBS_AG = "";
            string strHIV = "";
            string strInfluAG = "";
            string strInfluAPR = "";

            bool b = true;

            if (string.IsNullOrEmpty(strPano) == false)
            {

                if (strBi == "61" || strBi == "62")
                {
                    DataTable dt = lbExSQL.sel_HEA_JEPSU(pDbCon, strPano, strBi);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        strPano = dt.Rows[0]["PANO"].ToString();
                    }
                }

                if (strResult.ToUpper().IndexOf("NON") >-1)
                {
                    strOk = "NO";
                }
                else if (strResult.ToUpper().IndexOf("NEGATIVE") > -1)
                {
                    strOk = "NO";
                }
                else if (strResult.ToUpper().IndexOf("INFLU B") > -1)
                {
                    strOk = "NO";
                }

                try
                {
                    SqlErr = lbExSQL.ins_EXAM_INFECT(pDbCon, strOk, strPano, strRowId, ref intRowAffected);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                        return false;
                    }

                    if (strOk == "OK")
                    {
                        switch (strSubCode.Trim())
                        {
                            case "SE01":
                            case "SE01A":
                            case "SI001":
                                strVDRL = strSpecNo.Trim();
                                break;
                            case "SI07":
                            case "SI07A":
                                strHBS_AG = strSpecNo.Trim();
                                break;
                            case "SI081":
                            case "SI08A":
                                strHCV_IGG = strSpecNo.Trim();
                                break;
                            case "SI12":
                            case "SI12D":
                                strHIV = strSpecNo.Trim();
                                break;
                            case "SI14":
                                strInfluAG = strSpecNo.Trim();
                                break;
                            case "GP23":
                                strInfluAPR = strSpecNo.Trim();
                                break;
                            default:
                                break;
                        }

                        ComFunc.MsgBox("혈액 감염성 관리 대상 환자로 등록 되었습니다. \r\n\r\n 혈액 감염성 환자로 등록되었는지 확인바랍니다.");
                    }

                    SqlErr = lbExSQL.ins_EXAM_INFECTMASTER(pDbCon, strPano, strSubCode, ref intRowAffected, strVDRL, strHCV_IGG, strHBS_AG, strHIV, strInfluAG, strInfluAPR);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                        return false;
                    }


                    DataTable dtInfMst = lbExSQL.sel_EXAM_INFECT_MASTER(pDbCon, strSpecNo, strPano,strSubCode);

                    if (dtInfMst != null && dtInfMst.Rows.Count > 0)
                    {
                        if (strOk != "OK")
                        {
                            SqlErr = lbExSQL.del_EXAM_INFECT_MASTER(pDbCon, dtInfMst.Rows[0]["ROWID"].ToString(), ref intRowAffected);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                                return false;
                            }

                        }
                    }
                    else
                    {
                        if (strOk == "OK")
                        {
                            SqlErr = lbExSQL.ins_EXAM_INFECT_MASTER(pDbCon, strPano, "01", strSpecNo, strSubCode, ref intRowAffected);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                                return false;
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                    return false;
                }



            }

            return b;

        }        
       
    }
}
