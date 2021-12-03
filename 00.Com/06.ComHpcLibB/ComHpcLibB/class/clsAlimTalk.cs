using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public class clsAlimTalk
    {
        HicExjongService hicExjongService = null;
        HicLtdService hicLtdService = null;
        HicCodeService hicCodeService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicJepsuService hicJepsuService = null;
        EtcAlimTalkResultService etcAlimTalkResultService = null;
        EtcAlimTalkTemplateService etcAlimTalkTemplateService = null;
        EtcAlimTalkService etcAlimTalkService = null;
        HicCancerResv2Service hicCancerResv2Service = null;
        HeaJepsuPatientService heaJepsuPatientService = null;
        HicJepsuWorkService hicJepsuWorkService = null;
        BasDoctorService basDoctorService = null;
        EtcSmsService etcSmsService = null;
        HicJepsuPatientService hicJepsuPatientService = null;

        ComFunc CF = new ComFunc();

        public static string GstrSendKey;
        public static string FstrTalkRDate;
        public static string FstrTalkRTime;

        public clsAlimTalk()
        {
            hicExjongService = new HicExjongService();
            hicLtdService = new HicLtdService();
            hicCodeService = new HicCodeService();
            comHpcLibBService = new ComHpcLibBService();
            hicJepsuService = new HicJepsuService();
            etcAlimTalkTemplateService = new EtcAlimTalkTemplateService();
            etcAlimTalkResultService = new EtcAlimTalkResultService();
            etcAlimTalkService = new EtcAlimTalkService();
            hicCancerResv2Service = new HicCancerResv2Service();
            heaJepsuPatientService = new HeaJepsuPatientService();
            hicJepsuWorkService = new HicJepsuWorkService();
            basDoctorService = new BasDoctorService();
            etcSmsService = new EtcSmsService();
            hicJepsuPatientService = new HicJepsuPatientService();
        }

        public void Clear_ATK_Varient()
        {
            clsHcType.ATK.JobDate = "";
            clsHcType.ATK.SendType = "";
            clsHcType.ATK.TempCD = "";
            clsHcType.ATK.JobSabun = 0;
            clsHcType.ATK.Pano = "";
            clsHcType.ATK.sName = "";
            clsHcType.ATK.HPhone = "";
            clsHcType.ATK.LtdName = "";
            clsHcType.ATK.Dept = "";
            clsHcType.ATK.DrName = "";
            clsHcType.ATK.RDate = "";
            clsHcType.ATK.RetTel = "";
            clsHcType.ATK.SendUID = "";
            clsHcType.ATK.SmsMsg = "";
            clsHcType.ATK.ATMsg = "";
            clsHcType.ATK.GJNAME = "";
            clsHcType.ATK.WRTNO = 0;
            clsHcType.ATK.LINK = "";
        }

        public void Read_Talk_ReqDateTime(string argTempCD, string argTDate)
        {
            long nGAP = 0;
            long nDay = 0;
            long nBF = 0;

            FstrTalkRDate = "";
            FstrTalkRTime = "";

            ETC_ALIMTALK_TEMPLATE list = etcAlimTalkTemplateService.GetItembyTempCd(argTempCD);

            if (!list.IsNullOrEmpty())
            {
                nGAP = long.Parse(list.SENDGAP);
                nDay = list.SENDDAY;
                nBF = long.Parse(list.SENDBF);

                if (nGAP == 0)
                {
                    return;
                }
                else if (nGAP == 1)
                {
                    FstrTalkRDate = argTDate;

                    if (!list.SENDTIME.IsNullOrEmpty())
                    {
                        FstrTalkRTime = list.SENDTIME;
                    }
                    else
                    {
                        FstrTalkRTime = "";
                    }
                }
                else if (nGAP == 2)
                {
                    FstrTalkRDate = argTDate;

                    if (!list.SENDTIME.IsNullOrEmpty())
                    {
                        FstrTalkRTime = list.SENDTIME;
                    }
                    else
                    {
                        FstrTalkRTime = "";
                    }

                    if (nBF < 0)
                    {
                        nDay *= -1;
                    }

                    FstrTalkRDate = DateTime.Parse(argTDate).AddDays(nDay).ToShortDateString();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argGubun"></param>
        /// <param name="argResCode"></param>
        /// <returns></returns>
        public string Read_AlimTalk_Send_Result(string argGubun, string argResCode)
        {
            string rtnVal = "";

            if (argResCode.IsNullOrEmpty())
            {
                argResCode = "0000";
            }

            ETC_ALIMTALK_RESULT list = etcAlimTalkResultService.GetBigobyGubun(argGubun, argResCode);

            if (!list.IsNullOrEmpty())
            {
                rtnVal = list.BIGO;

            }

            return rtnVal;
        }

        public void MYSQL_ALIMTALK_INSERT()
        {
            string dt = "";
            string strMsg = "";
            string strHtel = "";
            string strDDD = "";
            string strTable = "";
            string strFTalk = "";
            string strSMTel = "";
            string strRettel = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            strDDD = VB.Left(clsHcType.ATK.HPhone, 3);
            strHtel = VB.Mid(clsHcType.ATK.HPhone, 4, clsHcType.ATK.HPhone.Length);
            strSMTel = clsHcType.ATK.HPhone.Replace("-", "");
            strRettel = clsHcType.ATK.RetTel.Replace("-", "");

            switch (strDDD)
            {
                case "010":
                    strHtel = "8210" + strHtel.Replace("-", "");
                    break;
                case "016":
                    strHtel = "8216" + strHtel.Replace("-", "");
                    break;
                case "017":
                    strHtel = "8217" + strHtel.Replace("-", "");
                    break;
                case "018":
                    strHtel = "8218" + strHtel.Replace("-", "");
                    break;
                case "019":
                    strHtel = "8219" + strHtel.Replace("-", "");
                    break;
                default:
                    break;
            }

            dt = clsPublic.GstrSysDate + clsPublic.GstrSysTime.Replace(" ", "").Replace(":", "").Replace("-", "");

            switch (clsHcType.ATK.SendType)
            {
                case "A":
                case "S":
                    //SQL = " INSERT INTO alimtalk.IMC_AT_BIZ_MSG "
                    //SQL = SQL & " (STATUS, PRIORITY, RESERVED_DATE, SENDER_KEY, PHONE_NUMBER, TEMPLATE_CODE, MESSAGE, ETC1,"
                    //SQL = SQL & "  RESEND_MT_TYPE, RESEND_MT_FROM, RESEND_MT_TO, RESEND_MT_MESSAGE_REUSE, RESEND_MT_MESSAGE ) VALUES "
                    //SQL = SQL & " ('1', 'N', '" & dt & "', '" & GstrSendKey & "', '" & strHtel & "', "
                    //SQL = SQL & " '" & ATK.TempCD & "', '" & ATK.ATMsg & "' ,'" & ATK.SendUID & "', "
                    //SQL = SQL & " 'SM','" & strRettel & "','" & strSMTel & "','N','" & ATK.SmsMsg & "')"
                    break;
                case "L":
                    //SQL = " INSERT INTO alimtalk.IMC_AT_BIZ_MSG "
                    //SQL = SQL & " (STATUS, PRIORITY, RESERVED_DATE, SENDER_KEY, PHONE_NUMBER, TEMPLATE_CODE, MESSAGE, ETC1,"
                    //SQL = SQL & "  RESEND_MT_TYPE, RESEND_MT_FROM, RESEND_MT_TO, RESEND_MT_MESSAGE_REUSE, RESEND_MT_MESSAGE ) VALUES "
                    //SQL = SQL & " ('1', 'N', '" & dt & "', '" & GstrSendKey & "', '" & strHtel & "', "
                    //SQL = SQL & " '" & ATK.TempCD & "', '" & ATK.ATMsg & "' ,'" & ATK.SendUID & "', "
                    //SQL = SQL & " 'LM','" & strRettel & "','" & strSMTel & "','N','" & ATK.SmsMsg & "')"
                    break;
                case "F":
                    //SQL = " INSERT INTO alimtalk.IMC_FT_BIZ_MSG "
                    //SQL = SQL & " (FT_TYPE, STATUS, PRIORITY, RESERVED_DATE, SENDER_KEY, PHONE_NUMBER, MESSAGE, ETC1) VALUES "
                    //SQL = SQL & " ('T', '1', 'N', '" & dt & "', '" & GstrSendKey & "', '" & strHtel & "', "
                    //SQL = SQL & " '" & ATK.SmsMsg & "','" & ATK.SendUID & "' )"
                    break;
                default:
                    break;
            }

            //웹으로 전송된 건 원내DB에 UPDATE
            //If Result = 0 Then
            //    SQL = " UPDATE ADMIN.ETC_ALIMTALK Set WEBSEND = 'Y' "
            //    SQL = SQL & " WHERE SENDUID ='" & ATK.SendUID & "' "
            //    Result = AdoExecute(SQL)
            //End If
        }

        public string READ_TEMPLATE_NAME(string argCode)
        {
            string rtnVal = "";

            ETC_ALIMTALK_TEMPLATE list = etcAlimTalkTemplateService.GetTitlebyTempCd(argCode);

            if (!list.IsNullOrEmpty())
            {
                rtnVal = list.TITLE;
            }

            return rtnVal;
        }

        public string READ_TEMPLATE_MESSAGE(string argCode)
        {
            string rtnVal = "";

            ETC_ALIMTALK_TEMPLATE list = etcAlimTalkTemplateService.GetMessagebyTempCd(argCode);

            if (!list.IsNullOrEmpty())
            {
                rtnVal = list.MESSAGE;
            }

            return rtnVal;
        }

        public string READ_TEMPLATE_SMS_MESSAGE(string argCode)
        {
            string rtnVal = "";

            ETC_ALIMTALK_TEMPLATE list = etcAlimTalkTemplateService.GetSendSmsbyTempCd(argCode);

            if (!list.IsNullOrEmpty())
            {
                rtnVal = list.SENDSMS;
            }

            return rtnVal;
        }

        public bool INSERT_ALIMTALK_MESSAGE()
        {
            bool rtnVal = false;
            int result = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            ETC_ALIMTALK item = new ETC_ALIMTALK();

            item.SENDFLAG = "1";
            item.SENDTYPE = clsHcType.ATK.SendType;
            item.TEMPCD = clsHcType.ATK.TempCD;
            item.ENTSABUN = clsHcType.ATK.JobSabun;
            item.PANO = clsHcType.ATK.Pano;
            item.SNAME = clsHcType.ATK.sName;
            item.HPHONE = clsHcType.ATK.HPhone;
            item.LTDNAME = clsHcType.ATK.LtdName;
            item.DEPTNAME = clsHcType.ATK.Dept;
            item.DRNAME = clsHcType.ATK.DrName;
            item.RDATE = clsHcType.ATK.RDate;
            item.RETTEL = clsHcType.ATK.RetTel;
            item.SENDUID = clsHcType.ATK.SendUID;
            item.WRTNO = clsHcType.ATK.WRTNO;
            item.GJNAME = clsHcType.ATK.GJNAME;
            item.QUESTLINK = clsHcType.ATK.LINK;

            result = etcAlimTalkService.Insert(item);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                rtnVal = false;
                return rtnVal;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            rtnVal = true;
            return rtnVal;
        }

        public void Work_SendTalk_Routine()
        {
            int nRead = 0;
            string strYoil = "";

            if (string.Compare(clsPublic.GstrSysTime, "07:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "18:20") <= 0)
            {
                HEA_TO_OneReserved_SMS_Send();        //종합검진예약-안내즉시
                HIC_CANCER_SMS_Send_New();            //일반건진 암예약 전송(접수당일 암검진 예약자 문자발송)
                HIC_RESULT_SU();                      //일반검진 결과지 방문수령 안내
            }

            if (string.Compare(clsPublic.GstrSysTime, "11:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "12:00") <= 0)
            {
                HIC_RESULT_PDF_SEND(); //일반검진결과지 발송
            }

            if (string.Compare(clsPublic.GstrSysTime, "11:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "18:20") <= 0)
            {
                HIC_CANCER_SMS_Send(); //일반건진 암예약 전송(검진 전날 예약문자 발송)
            }

            if (string.Compare(clsPublic.GstrSysTime, "11:30") >= 0 && string.Compare(clsPublic.GstrSysTime, "12:00") <= 0)
            {
                HIC_JEPSU_WORK_SMS_Send();        //일반검진 가접수예약 안내문자(예약작업 다음날 발송)
            }

            if (string.Compare(clsPublic.GstrSysTime, "12:30") >= 0 && string.Compare(clsPublic.GstrSysTime, "13:00") <= 0)
            {
                HIC_JEPSU_WORK_SMS_Send_1();     //일반검진 가접수예약 안내문자(예약작업 다음날 발송)-재검알림(특수검진)
                HIC_RESULT_TONGBO_SEND();        //일반검진 공단검진결과지 <발송>안내-일반우편
            }

            if (string.Compare(clsPublic.GstrSysTime, "14:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "15:00") <= 0)
            {
                HEA_TO_Reserved_SMS_Send();        //종검 예약자 안내문자(검진일자 바로전날 문자전송)
                HEA_TO_Reserved_SMS_3Day_Send();   //종검 예약자 안내문자(검진일자 3일전 문자전송)
                HIC_CANCER_SMS_3Day_Send();        //일반건진 암예약 전송(검진 3일전 예약문자 발송)
            }

            if (string.Compare(clsPublic.GstrSysTime, "12:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "12:35") <= 0)
            {
                YeYak_SMS_Move("1");
            }
            else if (string.Compare(clsPublic.GstrSysTime, "18:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "18:35") <= 0)
            {
                YeYak_SMS_Move("2");
            }

            if (string.Compare(clsPublic.GstrSysTime, "12:40") >= 0 && string.Compare(clsPublic.GstrSysTime, "13:15") <= 0)
            {
                Yeyak_SMS_Move3("1");
            }
            else if (string.Compare(clsPublic.GstrSysTime, "18:40") >= 0 && string.Compare(clsPublic.GstrSysTime, "19:15") <= 0)
            {
                Yeyak_SMS_Move3("2");
            }

            EDPS_Mobile_Call_Send(); //전산실 콜CALL당직

            //웹으로 미전송건 전송함
            List<ETC_ALIMTALK> list = etcAlimTalkService.GetItembySendFlag("1");

            nRead = list.Count;

            if (nRead > 0)
            {
                for (int i = 0; i < nRead; i++)
                {
                    Clear_ATK_Varient();
                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = string.Format("{0:yyyy-MM-dd HH:mm}", list[i].RDATE);
                    clsHcType.ATK.SendUID = list[i].SENDUID;
                    clsHcType.ATK.Pano = list[i].PANO;
                    clsHcType.ATK.sName = list[i].SNAME;
                    clsHcType.ATK.HPhone = list[i].HPHONE;
                    clsHcType.ATK.RetTel = list[i].RETTEL;
                    clsHcType.ATK.SendType = list[i].SENDTYPE;
                    clsHcType.ATK.TempCD = list[i].TEMPCD;
                    clsHcType.ATK.Dept = list[i].DEPTNAME;
                    clsHcType.ATK.DrName = list[i].DRNAME;
                    clsHcType.ATK.LtdName = list[i].LTDNAME;
                    clsHcType.ATK.JobSabun = list[i].ENTSABUN;
                    clsHcType.ATK.GJNAME = list[i].GJNAME;

                    //pdf결과지 즉시발송관련 추가
                    clsHcType.ATK.LINK = list[i].QUESTLINK;

                    clsHcType.ATK.ATMsg = READ_TEMPLATE_MESSAGE(clsHcType.ATK.TempCD);
                    clsHcType.ATK.SmsMsg = READ_TEMPLATE_SMS_MESSAGE(clsHcType.ATK.TempCD);

                    //치환하는 문자 확인해보기
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", clsHcType.ATK.sName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{사업장명}", clsHcType.ATK.LtdName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{검진종류명}", clsHcType.ATK.GJNAME);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(clsHcType.ATK.RDate, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(clsHcType.ATK.RDate, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(clsHcType.ATK.RDate, 9, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{HH}", VB.Left(VB.Pstr(clsHcType.ATK.RDate, " ", 2), 5));

                    //pdf결과지 즉시발송관련 추가
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{링크주소}", clsHcType.ATK.LINK);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{검진종류}", clsHcType.ATK.GJNAME);

                    //설문조사관련 추가
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{설문조사제목}", list[i].QUESTTITLE);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{설문조사내용}", list[i].QUESTMSG);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{설문조사링크}", list[i].QUESTLINK);

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", clsHcType.ATK.sName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{사업장명}", clsHcType.ATK.LtdName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{검진종류명}", clsHcType.ATK.GJNAME);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(clsHcType.ATK.RDate, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(clsHcType.ATK.RDate, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(clsHcType.ATK.RDate, 9, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{HH}", VB.Left(VB.Pstr(clsHcType.ATK.RDate, " ", 2), 5));

                    //pdf결과지 즉시발송관련 추가
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{링크주소}", clsHcType.ATK.LINK);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{검진종류}", clsHcType.ATK.GJNAME);

                    //설문조사관련 추가
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{설문조사제목}", list[i].QUESTTITLE);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{설문조사내용}", list[i].QUESTMSG);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{설문조사링크}", list[i].QUESTLINK);

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", clsHcType.ATK.sName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{사업장명}", clsHcType.ATK.LtdName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{검진종류명}", clsHcType.ATK.GJNAME);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(clsHcType.ATK.RDate, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(clsHcType.ATK.RDate, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(clsHcType.ATK.RDate, 9, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{HH}", VB.Left(VB.Pstr(clsHcType.ATK.RDate, " ", 2), 5));


                    //pdf결과지 즉시발송관련 추가
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{링크주소}", clsHcType.ATK.LINK);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{검진종류}", clsHcType.ATK.GJNAME);

                    //설문조사관련 추가
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{설문조사제목}", list[i].QUESTTITLE);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{설문조사내용}", list[i].QUESTMSG);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{설문조사링크}", list[i].QUESTLINK);

                    clsDbMySql.DBConnect("", "", "psmh", "psmh", "psmh2");
                    MYSQL_ALIMTALK_INSERT();
                    clsDbMySql.DisDBConnect();
                }
            }
        }

        /// <summary>
        /// 일반건진 암검사 예약한날 휴대폰 문자메세지 Table 형성
        /// </summary>
        public void HIC_CANCER_SMS_Send_New()
        {
            bool bJOB = false;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";
            string strPANO = "";
            string strName = "";
            string strTime = "";
            string strSTIME = "";
            string strTel = "";
            string strMinRTime = "";
            string strAmPm2 = "";
            string strTempCD = "";
            string strJONGNAME1 = "";
            string strJONGNAME2 = "";
            string[] strAMJONG = new string[5];

            ComFunc.ReadSysDate(clsDB.DbCon);

            //strTempCD = "C_MJ_001_02_13949"; //검진종류 추가 템플릿
            //C__MJ_001_02_13949 -> C_MJ_001_02_29466 템플릿 변경 (2021-08-11)
            strTempCD = "C_MJ_001_02_29466";

            strFDate = clsPublic.GstrSysDate;
            strTDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(1).ToShortDateString();

            //암검진 예약자 SMS 전송
            List<HIC_CANCER_RESV2> list = hicCancerResv2Service.GetItembyEntTime(strFDate, strTDate);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                bJOB = true;
                strName = "";
                strTel = "";
                strJONGNAME1 = "";
                strJONGNAME2 = "";
                for (int j = 0; j <= 5; j++)
                {
                    strAMJONG[j] = "";
                }

                strPANO = list[i].PANO;
                strName = list[i].SNAME;
                strTel = list[i].HPHONE;
                strTime = list[i].RTIME.ToString();
                strSTIME = list[i].RTIME2.ToString();

                if (list[i].GBBOHUM == "Y")
                {
                    strJONGNAME1 = "공단1차 검진,";  //1차검진
                }

                if (list[i].GBUGI == "Y" || list[i].GBGFS == "Y" || list[i].GBGFSH == "Y")
                {
                    strAMJONG[0] = "위암"; //위암
                }
                if (list[i].GBMAMMO == "Y")
                {
                    strAMJONG[1] = "유방암";  //유방암
                }
                if (list[i].GBRECUTM == "Y" || list[i].GBCOLON == "Y")
                {
                    strAMJONG[2] = "대장암";   // 대장암
                }
                if (list[i].GBSONO == "Y")
                {
                    strAMJONG[3] = "간초음파";  //간초음파
                }
                if (list[i].GBWOMB == "Y")
                {
                    strAMJONG[4] = "자궁경부암";  //자궁경부암
                }
                if (list[i].GBCT == "Y")
                {
                    strAMJONG[5] = "폐암";  //폐암
                }

                for (int j = 0; j <= 5; j++)
                {
                    if (!strAMJONG[j].IsNullOrEmpty())
                    {
                        strJONGNAME2 += strAMJONG[j] + ",";
                    }
                }

                if (VB.Right(strJONGNAME2, 2) == " ,")
                {
                    strJONGNAME2 = VB.Left(strJONGNAME2, strJONGNAME2.Length - 2);
                }

                strFDate = clsPublic.GstrSysDate;
                strTDate = clsPublic.GstrSysDate + " " + "23:59";

                //이미 자료를 넘겼는지 확인함
                ETC_ALIMTALK list2 = etcAlimTalkService.GetRDatebyJobDatePaNoHPhoneTempCd(strFDate, strTDate, strPANO, strTel, strTempCD);

                strMinRTime = "";
                if (!list2.IsNullOrEmpty())
                {
                    strMinRTime = list2.RDATE.ToString();
                }
                //이미 전송한 예약시간이 있으면 다시 전송 않함
                if (!strMinRTime.IsNullOrEmpty())
                {
                    bJOB = false;
                }

                //SMS 자료에 INSERT
                if (bJOB == true)
                {
                    Clear_ATK_Varient();
                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = strTime;
                    clsHcType.ATK.SendUID = strTel + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = strPANO;
                    clsHcType.ATK.sName = strName;
                    clsHcType.ATK.HPhone = strTel;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "A";
                    clsHcType.ATK.TempCD = strTempCD;
                    clsHcType.ATK.Dept = "HR";
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = "";
                    clsHcType.ATK.JobSabun = long.Parse(clsType.User.IdNumber);

                    clsHcType.ATK.ATMsg = READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", strName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(strTime, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{HH}", strSTIME);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{기본검진}", strJONGNAME1);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{암검진}", strJONGNAME2);

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", strName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(strTime, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{HH}", strSTIME);

                    if (INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        return;
                    }

                    MYSQL_ALIMTALK_INSERT();
                }
            }
        }

        /// <summary>
        /// 검진전날 일반건진 암검사 예약자 휴대폰 문자메세지 Table 형성
        /// </summary>
        void HIC_CANCER_SMS_Send()
        {
            bool bJOB = false;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";
            string strPANO = "";
            string strName = "";
            string strTime = "";
            string strSTIME = "";
            string strTel = "";
            string strMinRTime = "";
            string strAmPm2 = "";
            string strTempCD = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            //strTempCD = "C_MJ_001_02_13951";
            //C_MJ_001_02_13951 -> C_MJ_001_02_29468 템플릿 변경 (2021-08-11)
            strTempCD = "C_MJ_001_02_29468";

            strFDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(1).ToShortDateString();
            strTDate = DateTime.Parse(strFDate).AddDays(1).ToShortDateString();

            strTDate += " 23:59";

            //익일의 방사선예약자를 SMS 자료로 Update
            List<HIC_CANCER_RESV2> list = hicCancerResv2Service.GetItembyRTimeHPhone(strFDate, strTDate);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                bJOB = true;
                strName = "";
                strTel = "";

                strPANO = list[i].PANO;
                strName = list[i].SNAME;
                strTel = list[i].HPHONE;
                strTime = list[i].RTIME.ToString();
                strSTIME = list[i].RTIME2.ToString();

                //이미 자료를 넘겼는지 확인함
                ETC_ALIMTALK list2 = etcAlimTalkService.GetRDatebyJobDatePaNoHPhoneTempCd(strFDate, strTDate, strPANO, strTel, strTempCD);

                if (!list2.IsNullOrEmpty())
                {
                    strMinRTime = list2.RDATE.ToString();
                }
                //이미 전송한 예약시간이 있으면 다시 전송 않함
                if (!strMinRTime.IsNullOrEmpty())
                {
                    bJOB = false;
                }

                //SMS 자료에 INSERT
                if (bJOB == true)
                {
                    Clear_ATK_Varient();
                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = strTime;
                    clsHcType.ATK.SendUID = strTel + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = strPANO;
                    clsHcType.ATK.sName = strName;
                    clsHcType.ATK.HPhone = strTel;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "A";
                    clsHcType.ATK.TempCD = strTempCD;
                    clsHcType.ATK.Dept = "HR";
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = "";
                    clsHcType.ATK.JobSabun = long.Parse(clsType.User.IdNumber);

                    clsHcType.ATK.ATMsg = READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", strName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(strTime, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{HH}", strSTIME);

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", strName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(strTime, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{HH}", strSTIME);

                    if (INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        return;
                    }

                    MYSQL_ALIMTALK_INSERT();
                }
            }
        }

        //검진 3일전날 일반건진 암검사 예약자 휴대폰 문자메세지 Table 형성
        public void HIC_CANCER_SMS_3Day_Send()
        {
            bool bJOB = false;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";
            string strPANO = "";
            string strName = "";
            string strTime = "";
            string strSTIME = "";
            string strTel = "";
            string strMinRTime = "";
            string strAmPm2 = "";
            string strTempCD = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            //strTempCD = "C_MJ_001_02_13950";
            //C_MJ_001_02_13950 -> C_MJ_001_02_29467 템플릿 변경 (2021-08-11)
            strTempCD = "C_MJ_001_02_29467";

            strFDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(3).ToShortDateString();
            strTDate = DateTime.Parse(strFDate).AddDays(1).ToShortDateString();

            strTDate += " 23:59";

            //익일의 방사선예약자를 SMS 자료로 Update
            List<HIC_CANCER_RESV2> list = hicCancerResv2Service.GetItembyRTimeHPhone(strFDate, strTDate);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                bJOB = true;
                strName = "";
                strTel = "";

                strPANO = list[i].PANO;
                strName = list[i].SNAME;
                strTel = list[i].HPHONE;
                strTime = list[i].RTIME.ToString();
                strSTIME = list[i].RTIME2.ToString();

                //이미 자료를 넘겼는지 확인함
                ETC_ALIMTALK list2 = etcAlimTalkService.GetRDatebyJobDatePaNoHPhoneTempCd(strFDate, strTDate, strPANO, strTel, strTempCD);

                if (!list2.IsNullOrEmpty())
                {
                    strMinRTime = list2.RDATE.ToString();
                }
                //이미 전송한 예약시간이 있으면 다시 전송 않함
                if (!strMinRTime.IsNullOrEmpty())
                {
                    bJOB = false;
                }

                //SMS 자료에 INSERT
                if (bJOB == true)
                {
                    Clear_ATK_Varient();
                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = strTime;
                    clsHcType.ATK.SendUID = strTel + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = strPANO;
                    clsHcType.ATK.sName = strName;
                    clsHcType.ATK.HPhone = strTel;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "A";
                    clsHcType.ATK.TempCD = strTempCD;
                    clsHcType.ATK.Dept = "HR";
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = "";
                    clsHcType.ATK.JobSabun = long.Parse(clsType.User.IdNumber);

                    clsHcType.ATK.ATMsg = READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", strName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(strTime, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{HH}", strSTIME);

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", strName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(strTime, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{HH}", strSTIME);

                    if (INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        return;
                    }

                    MYSQL_ALIMTALK_INSERT();
                }
            }
        }

        /// <summary>
        /// 종검 당일 예약자 전체 전송(접수당일 예약문자전송)
        /// </summary>
        public void HEA_TO_OneReserved_SMS_Send()
        {
            bool bJOB = false;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";
            string strPANO = "";
            string strName = "";
            string strTime = "";
            string strSTIME = "";
            string strTel = "";
            string strMinRTime = "";
            string strAmPm2 = "";
            string strTempCD = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            strTempCD = "C_MJ_001_02_13954";

            strFDate = clsPublic.GstrSysDate;
            strTDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(1).ToString("yyyy-MM-dd");

       //     strTDate += " 23:59";

            //익일의 종검예약자를 SMS 자료로 Update
            List<HEA_JEPSU_PATIENT> list = heaJepsuPatientService.GetItembyJepDate(strFDate, strTDate);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                bJOB = true;
                strName = "";
                strTel = "";

                strPANO = list[i].PANO.ToString();
                strName = list[i].SNAME;
                strTel = list[i].HPHONE;
                strTime = list[i].YDATE.ToString();
                strSTIME = list[i].STIME.ToString();
                strTime += " " + strSTIME;
                strAmPm2 = list[i].AMPM2;

                //이미 자료를 넘겼는지 확인함
                ETC_ALIMTALK list2 = etcAlimTalkService.GetRDatebyJobDatePaNoHPhoneTempCd(strFDate, strTDate, strPANO, strTel, strTempCD);
                strMinRTime = "";
                if (!list2.IsNullOrEmpty())
                {
                    strMinRTime = list2.RDATE.ToString();
                }
                //이미 전송한 예약시간이 있으면 다시 전송 않함
                if (!strMinRTime.IsNullOrEmpty())
                {
                    bJOB = false;
                }

                //SMS 자료에 INSERT
                if (bJOB == true)
                {
                    Clear_ATK_Varient();
                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = strTime;
                    clsHcType.ATK.SendUID = strTel + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = strPANO;
                    clsHcType.ATK.sName = strName;
                    clsHcType.ATK.HPhone = strTel;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "A";
                    clsHcType.ATK.TempCD = strTempCD;
                    clsHcType.ATK.Dept = "TO";
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = "";
                    clsHcType.ATK.JobSabun = long.Parse(clsType.User.IdNumber);

                    clsHcType.ATK.ATMsg = READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", strName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(strTime, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{HH}", strSTIME);

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", strName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(strTime, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{HH}", strSTIME);

                    if (INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        return;
                    }

                    MYSQL_ALIMTALK_INSERT();
                }
            }
        }

        /// <summary>
        /// 익일 종검 예약자 휴대폰 문자메세지 Table 형성
        /// </summary>
        public void HEA_TO_Reserved_SMS_Send()
        {
            bool bJOB = false;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";
            string strPANO = "";
            string strName = "";
            string strTime = "";
            string strSTIME = "";
            string strTel = "";
            string strMinRTime = "";
            string strAmPm2 = "";
            string strTempCD = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            strTempCD = "C_MJ_001_02_13952";

            strFDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(1).ToShortDateString();
            strTDate = DateTime.Parse(strFDate).AddDays(1).ToShortDateString();

            strTDate += " 23:59";

            //익일의 종검예약자를 SMS 자료로 Update
            List<HEA_JEPSU_PATIENT> list = heaJepsuPatientService.GetItembyJepDate(strFDate, strTDate);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                bJOB = true;
                strName = "";
                strTel = "";

                strPANO = list[i].PANO.ToString();
                strName = list[i].SNAME;
                strTel = list[i].HPHONE;
                strTime = list[i].YDATE.ToString();
                strSTIME = list[i].STIME.ToString();
                strTime += " " + strSTIME;
                strAmPm2 = list[i].AMPM2;

                //이미 자료를 넘겼는지 확인함
                ETC_ALIMTALK list2 = etcAlimTalkService.GetRDatebyJobDatePaNoHPhoneTempCd(strFDate, strTDate, strPANO, strTel, strTempCD);
                strMinRTime = "";
                if (!list2.IsNullOrEmpty())
                {
                    strMinRTime = list2.RDATE.ToString();
                }
                //이미 전송한 예약시간이 적으면 다시 전송 않함
                if (!strMinRTime.IsNullOrEmpty() && string.Compare(strMinRTime, strTime) <= 0)
                {
                    bJOB = false;
                }

                //SMS 자료에 INSERT
                if (bJOB == true)
                {
                    Clear_ATK_Varient();
                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = strTime;
                    clsHcType.ATK.SendUID = strTel + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = strPANO;
                    clsHcType.ATK.sName = strName;
                    clsHcType.ATK.HPhone = strTel;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "A";
                    clsHcType.ATK.TempCD = strTempCD;
                    clsHcType.ATK.Dept = "TO";
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = "";
                    clsHcType.ATK.JobSabun = long.Parse(clsType.User.IdNumber);

                    clsHcType.ATK.ATMsg = READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", strName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(strTime, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{HH}", strSTIME);

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", strName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(strTime, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{HH}", strSTIME);

                    if (INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        return;
                    }

                    MYSQL_ALIMTALK_INSERT();
                }
            }
        }

        /// <summary>
        /// 종검 예약자 안내문자(검진일자 3일전 문자전송)
        /// </summary>
        public void HEA_TO_Reserved_SMS_3Day_Send()
        {
            bool bJOB = false;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";
            string strPANO = "";
            string strName = "";
            string strTime = "";
            string strSTIME = "";
            string strTel = "";
            string strMinRTime = "";
            string strAmPm2 = "";
            string strTempCD = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            strTempCD = "C_MJ_001_02_13953";

            strFDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(3).ToShortDateString();
            strTDate = DateTime.Parse(strFDate).AddDays(1).ToShortDateString();

            strTDate += " 23:59";

            //익일의 종검예약자를 SMS 자료로 Update
            List<HEA_JEPSU_PATIENT> list = heaJepsuPatientService.GetItembyJepDate(strFDate, strTDate);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                bJOB = true;
                strName = "";
                strTel = "";

                strPANO = list[i].PANO.ToString();
                strName = list[i].SNAME;
                strTel = list[i].HPHONE;
                strTime = list[i].YDATE.ToString();
                strSTIME = list[i].STIME.ToString();
                strTime += " " + strSTIME;
                strAmPm2 = list[i].AMPM2;

                //이미 자료를 넘겼는지 확인함
                ETC_ALIMTALK list2 = etcAlimTalkService.GetRDatebyJobDatePaNoHPhoneTempCd(strFDate, strTDate, strPANO, strTel, strTempCD);
                strMinRTime = "";
                if (!list2.IsNullOrEmpty())
                {
                    strMinRTime = list2.RDATE.ToString();
                }
                //이미 전송한 예약시간이 있으면 다시 전송 않함
                if (!strMinRTime.IsNullOrEmpty() && string.Compare(strMinRTime, strTime) <= 0)
                {
                    bJOB = false;
                }

                //SMS 자료에 INSERT
                if (bJOB == true)
                {
                    Clear_ATK_Varient();
                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = strTime;
                    clsHcType.ATK.SendUID = strTel + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = strPANO;
                    clsHcType.ATK.sName = strName;
                    clsHcType.ATK.HPhone = strTel;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "A";
                    clsHcType.ATK.TempCD = strTempCD;
                    clsHcType.ATK.Dept = "TO";
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = "";
                    clsHcType.ATK.JobSabun = long.Parse(clsType.User.IdNumber);

                    clsHcType.ATK.ATMsg = READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", strName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(strTime, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{HH}", strSTIME);

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", strName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(strTime, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{HH}", strSTIME);

                    if (INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        return;
                    }

                    MYSQL_ALIMTALK_INSERT();
                }
            }
        }

        public void Hic_Liver_Result(string argDate, string argTime)
        {
            return;

            //Dim strFDate          As String
            //Dim strTDate          As String
            //Dim strGDate          As String
            //Dim strTempCD         As String
            //Dim strTongDate       As String

            //Dim i                 As Integer
            //Dim j                 As Integer
            //Dim nWrtCnt           As Integer
            //Dim nREAD             As Integer

            //Dim strPANO           As String
            //Dim strTime           As String
            //Dim strJobTime        As String
            //Dim strName           As String
            //Dim strTel            As String
            //Dim strData           As String
            //Dim strMinRTime       As String
            //Dim strHtel           As String

            //'C_MJ_001_02_12488
            //strTempCD = "C_MJ_001_02_12488"

            //strFDate = GstrSysDate
            //strTDate = DATE_ADD(strFDate, 1)
            //strGDate = DATE_ADD(strFDate, -90)

            //'건진 결과지 전송
            //SQL = "SELECT a.Pano,a.SName,b.HPhone,TO_CHAR(a.TONGBODATE,'YYYY-MM-DD') TONGBODATE "
            //SQL = SQL & " FROM ADMIN.HIC_JEPSU a,  "
            //SQL = SQL & "      ADMIN.HIC_RESULT r, "
            //SQL = SQL & "      ADMIN.HIC_PATIENT b, "
            //SQL = SQL & "WHERE a.JepDate >= TO_DATE('" & strGDate & "','YYYY-MM-DD') "
            //SQL = SQL & "  AND a.TONGBODATE >= TO_DATE('" & strFDate & "','YYYY-MM-DD') "
            //SQL = SQL & "  AND a.TONGBODATE <  TO_DATE('" & strTDate & "','YYYY-MM-DD') "
            //SQL = SQL & "  AND a.GjJong = '31' "
            //SQL = SQL & "  AND a.DelDate IS NULL "
            //SQL = SQL & "  AND a.Pano = r.Wrtno "
            //SQL = SQL & "  AND a.Pano=b.Pano(+) "
            //SQL = SQL & "  AND r.EXCODE = 'A264' "
            //SQL = SQL & "  AND r.RESULT >= '9.1' "
            //'검진은 접수와 동시에 개인정보동의 활용이기 때문에 전송함
            //'''SQL = SQL & "  AND b.GbSMS = 'Y' "  'SMS 전송 신청자만 전송함
            //SQL = SQL & "  AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019') "  '종검은 HPhone 컬럼 사용 4.30 보스코수녀님요청
            //SQL = SQL & "ORDER BY a.TONGBODATE, a.SName, b.HPhone "
            //Call AdoOpenSet(AdoRes, SQL)
            //nREAD = RowIndicator
            //nWrtCnt = 0

            //'결과지 출력후 3일차부터
            //For i = 0 To nREAD -1
            //    If Trim(AdoGetString(AdoRes, "Pano", i)) = "" Then
            //        strPANO = "00000000"
            //    Else
            //        strPANO = Format(AdoGetString(AdoRes, "Pano", i), "00000000")
            //    End If

            //    strTime = strJobTime
            //    strName = "":  strTel = ""

            //    strData = Trim(AdoGetString(AdoRes, "Sname", i))
            //    For j = 1 To Len(strData)
            //        If Mid(strData, j, 1) <> " " Then
            //            strName = strName & Mid(strData, j, 1)
            //        End If
            //    Next j

            //    strData = Trim(AdoGetString(AdoRes, "HPhone", i))
            //    For j = 1 To Len(strData)
            //        If Mid(strData, j, 1) >= "0" And Mid(strData, j, 1) <= "9" Then
            //            strTel = strTel & Mid(strData, j, 1)
            //        End If
            //    Next j

            //    strTongDate = Trim(AdoGetString(AdoRes, "TONGBODATE", i))

            //    '이미 자료를 넘겼는지 확인함
            //    SQL = "SELECT MIN(TO_CHAR(RDate,'YYYY-MM-DD HH24:MI')) RTie "
            //    SQL = SQL & " FROM ADMIN.ETC_ALIMTALK "
            //    SQL = SQL & "WHERE JobDate>=TO_DATE('" & strFDate & "','YYYY-MM-DD') "
            //    SQL = SQL & "  AND JobDate< TO_DATE('" & strTDate & "','YYYY-MM-DD') "
            //    If strPANO<> "00000000" Then
            //        SQL = SQL & "  AND PANO = '" & strPANO & "' "
            //    End If
            //    SQL = SQL & "  AND HPhone ='" & strTel & "'"
            //    SQL = SQL & "  AND TEMPCD = 'C_MJ_001_02_12488' "
            //    Call AdoOpenSet(rs1, SQL)
            //    strMinRTime = ""
            //    If RowIndicator > 0 Then
            //        strMinRTime = Trim(AdoGetString(rs1, "RTime", 0))
            //    End If
            //    Call AdoCloseSet(rs1)
            //    '이미 전송한 예약시간이 적으면 다시 전송 않함
            //    If strMinRTime<> "" Then strTel = ""


            //    'SMS 자료에 INSERT
            //    If strName<> "" And strTel<> "" Then

            //        Call Clear_ATK_Varient
            //        Call Read_Talk_ReqDateTime(strTempCD, strTongDate)

            //        '------------( 자료를 DB에 INSERT )---------------------
            //        'ATK.RDate = Replace(Replace(Replace(GstrSysDate & GstrSysTime, " ", ""), ":", ""), "-", "")

            //        If FstrTalkRDate = "" Then
            //            ATK.RDate = ""   '없으면 즉시 전송
            //        Else
            //            ATK.RDate = FstrTalkRDate & " " & FstrTalkRTime
            //        End If

            //        ATK.SendUID = strHtel & GstrSysDate & Trim(Right(Now(), 8))
            //        ATK.SendUID = Replace(Replace(Replace(Replace(ATK.SendUID, ":", ""), "-", ""), " ", ""), ".", "")
            //        ATK.Pano = strPANO
            //        ATK.sName = strName
            //        ATK.HPhone = strTel
            //        ATK.RetTel = "054-260-8188"
            //        ATK.SendType = "A"
            //        ATK.TempCD = strTempCD
            //        ATK.Dept = "HR"
            //        ATK.DrName = ""
            //        ATK.LtdName = ""
            //        ATK.JobSabun = GnJobSabun
            //        ATK.ATMsg = READ_TEMPLATE_MESSAGE(strTempCD)
            //        ATK.SmsMsg = READ_TEMPLATE_SMS_MESSAGE(strTempCD)
            //        ATK.ATMsg = Replace(ATK.ATMsg, "#{수검자명}", strName)
            //        ATK.SmsMsg = Replace(ATK.SmsMsg, "#{수검자명}", strName)

            //        If INSERT_ALIMTALK_MESSAGE = False Then
            //            Exit Function
            //        End If

            //        'Call MyadoConnect("psmh", "psmh", "psmh2")
            //        Call MYSQL_ALIMTALK_INSERT
            //        'Call MyAdoDisConnect
            //    End If
            //Next i

            //Call AdoCloseSet(AdoRes)
        }

        /// <summary>
        /// 일반검진 가접수예약 안내문자(예약작업 다음날 발송)
        /// </summary>
        public void HIC_JEPSU_WORK_SMS_Send()
        {
            bool bJOB = false;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";
            string strPANO = "";
            string strName = "";
            string strTime = "";
            string strSTIME = "";
            string strTel = "";
            string strLtdName = "";
            string strMinRTime = "";
            string strAmPm2 = "";
            string strTempCD = "";
            string strGjjong = "";
            string strJONGNAME = "";
            string strCHUL = "";
            List<string> strjong = new List<string>();

            ComFunc.ReadSysDate(clsDB.DbCon);

            strTempCD = "C_MJ_001_02_18910";

            strFDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-1).ToShortDateString();
            strTDate = clsPublic.GstrSysDate;

            strTDate += " 23:59";

            //익일의 종검예약자를 SMS 자료로 Update
            strjong.Clear();
            strjong.Add("11");
            strjong.Add("23");
            List<HIC_JEPSU_WORK> list = hicJepsuWorkService.GetItembyJepDateGjJong(strFDate, strTDate, strjong);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                bJOB = true;
                strName = "";
                strTel = "";

                strPANO = list[i].PANO.ToString();
                strName = list[i].SNAME;
                strTel = list[i].HPHONE;
                strTime = list[i].YDATE.ToString();
                strLtdName = list[i].LTDNAME;
                strGjjong = list[i].GJJONG.ToString();
                strCHUL = list[i].GBCHUL;

                if (strGjjong == "11")
                {
                    strJONGNAME = "일반검진 및 특수검진";
                }
                else if (strGjjong == "23")
                {
                    strJONGNAME = "특수검진";
                }
                else
                {
                    strJONGNAME = "일반검진";
                }

                if (strTel.IsNullOrEmpty())
                {
                    strTel = READ_JEPSU_WORK_HPHONE(strPANO, strGjjong);
                }

                //이미 자료를 넘겼는지 확인함
                strFDate = clsPublic.GstrSysDate;
                strTDate = clsPublic.GstrSysDate + " " + "23:59";

                ETC_ALIMTALK list2 = etcAlimTalkService.GetRDatebyJobDatePaNoHPhoneTempCd(strFDate, strTDate, strPANO, strTel, strTempCD);
                strMinRTime = "";
                if (!list2.IsNullOrEmpty())
                {
                    strMinRTime = list2.RDATE.ToString();
                }
                //이미 전송한 예약시간이 있으면 다시 전송 않함
                if (!strMinRTime.IsNullOrEmpty())
                {
                    bJOB = false;
                }

                //SMS 자료에 INSERT
                if (bJOB == true)
                {
                    Clear_ATK_Varient();
                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = strTime;
                    clsHcType.ATK.SendUID = strTel + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = strPANO;
                    clsHcType.ATK.sName = strName;
                    clsHcType.ATK.HPhone = strTel;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "A";
                    clsHcType.ATK.TempCD = strTempCD;
                    clsHcType.ATK.Dept = "HR";
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = strLtdName;
                    clsHcType.ATK.JobSabun = long.Parse(clsType.User.IdNumber);
                    clsHcType.ATK.GJNAME = strJONGNAME;

                    clsHcType.ATK.ATMsg = READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", strName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(strTime, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2));

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", strName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(strTime, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2));

                    if (INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        return;
                    }

                    MYSQL_ALIMTALK_INSERT();
                }
            }
        }

        /// <summary>
        /// 일반검진 가접수예약 안내문자(예약작업 다음날 발송)-재검알림(특수검진)
        /// </summary>
        public void HIC_JEPSU_WORK_SMS_Send_1()
        {
            bool bJOB = false;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";
            string strPANO = "";
            string strName = "";
            string strTime = "";
            string strSTIME = "";
            string strTel = "";
            string strLtdName = "";

            string strMinRTime = "";
            string strAmPm2 = "";
            string strTempCD = "";
            string strGjjong = "";
            string strJONGNAME = "";
            string strTONGBODATE = "";

            string strSECOND = "";
            List<string> strjong = new List<string>();

            ComFunc.ReadSysDate(clsDB.DbCon);
            strTempCD = "C_MJ_001_02_14651";
            strFDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-1).ToShortDateString();
            strTDate = DateTime.Parse(strFDate).AddDays(-1).ToShortDateString();

            //익일의 종검예약자를 SMS 자료로 Update
            strjong.Clear();
            strjong.Add("16");
            List<HEA_JEPSU_PATIENT> list = heaJepsuPatientService.GetItembyJepDateGjJong(strFDate, strTDate, strjong);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                bJOB = true;
                strName = "";
                strTel = "";

                strPANO = list[i].PANO.ToString();
                strName = list[i].SNAME;
                strTel = list[i].HPHONE;
                strLtdName = list[i].LTDNAME;

                strSECOND = ""; strTime = ""; strGjjong = ""; strTONGBODATE = "";

                //1차 검진 데이터 확인
                strjong.Clear();
                strjong.Add("11");
                strjong.Add("23");
                HIC_JEPSU list2 = hicJepsuService.GetItembyPtnoGjJong(strPANO, strjong);

                if (!list2.IsNullOrEmpty())
                {
                    strTime = list2.YDATE.ToString();
                    strSECOND = READ_SECOND_Exams_Name(list2.SECOND_EXAMS);
                    strGjjong = list2.GJJONG;
                    strTONGBODATE = list2.TONGBODATE.ToString();
                }

                if (strGjjong == "11")
                {
                    strJONGNAME = "일반검진 및 특수검진";
                }
                else if (strGjjong == "23")
                {
                    strJONGNAME = "특수검진";
                }
                else
                {
                    strJONGNAME = "일반검진";
                }

                //이미 자료를 넘겼는지 확인함
                ETC_ALIMTALK list3 = etcAlimTalkService.GetRDatebyJobDatePaNoHPhoneTempCd(strFDate, strTDate, strPANO, strTel, strTempCD);
                strMinRTime = "";
                if (!list3.IsNullOrEmpty())
                {
                    strMinRTime = list3.RDATE.ToString();
                }
                //이미 전송한 예약시간이 있으면 다시 전송 않함
                if (!strMinRTime.IsNullOrEmpty() && string.Compare(strMinRTime, strTime) <= 0)
                {
                    bJOB = false;
                }
                //통보일자확인가 없을경우 전송 안함
                if (strTONGBODATE.IsNullOrEmpty())
                {
                    bJOB = false;
                }

                //SMS 자료에 INSERT
                if (bJOB == true)
                {
                    Clear_ATK_Varient();
                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = strTime;
                    clsHcType.ATK.SendUID = strTel + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = strPANO;
                    clsHcType.ATK.sName = strName;
                    clsHcType.ATK.HPhone = strTel;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "A";
                    clsHcType.ATK.TempCD = strTempCD;
                    clsHcType.ATK.Dept = "HR";
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = strLtdName;
                    clsHcType.ATK.JobSabun = long.Parse(clsType.User.IdNumber);
                    clsHcType.ATK.GJNAME = strJONGNAME;

                    clsHcType.ATK.ATMsg = READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", clsHcType.ATK.sName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{검진종류}", clsHcType.ATK.GJNAME);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(clsHcType.ATK.RDate, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(clsHcType.ATK.RDate, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(clsHcType.ATK.RDate, 9, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{2차 검진사유}", strSECOND);

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", clsHcType.ATK.sName);

                    if (INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        return;
                    }

                    MYSQL_ALIMTALK_INSERT();
                }
            }
        }

        /// <summary>
        /// 익일 예약자 휴대폰 문자메세지 Table 형성
        /// </summary>
        /// <param name="argGubun"></param>
        public void YeYak_SMS_Move(string argGubun)
        {
            /*============================================================
            '2010-10-29 김현욱
            '마감 프로그램에서 빼왔음.
            '하루 두번 생성할 예정임.
            '생성시간 정오 12:00, 오후 6시 18:00
            '2019-07-26 김해수
            'ETC_SMS 에서 ETC_ALIMTALK으로 프로그램 이동
        '==================================================================*/

            int nREAD = 0;
            int nREAD3 = 0;
            int nREAD4 = 0;
            int nWrtCnt = 0;
            string strFDate = "";
            string strTDate = "";
            string strDelDate = "";

            string strOldPano = "";
            string strPANO = "";
            string strName = "";
            string strDeptCode = "";
            string strDRCODE = "";
            string strTime = "";
            string strChangTime = "";
            string strSuDate = "";
            string strTel = "";
            string strRettel = "";
            string strData = "";
            string strDeptName = "";
            string strDeptNameS = "";
            string strMsg = "";
            string strJobTime = "";
            string strClass = "";

            string strTempMSG = "";
            string strDrname = "";
            string strMinRTime = "";
            bool bJOB = false;
            string strTempCD = ""; //알림톡 템플릿 변수

            int result = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            strTempCD = "C_MJ_001_02_18354"; //알림톡 진료과 문의 번호

            strFDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(1).ToShortDateString();    //내일
            strTDate = DateTime.Parse(strFDate).AddDays(1).ToShortDateString();                 //모래

            if (argGubun == "1")
            {
                strJobTime = clsPublic.GstrSysDate + " 13:00";
            }
            else
            {
                strJobTime = clsPublic.GstrSysDate + " 18:00";
            }

            List<COMHPC> list = comHpcLibBService.GetSmsbyDate(strFDate, strTDate);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                bJOB = true;
                strPANO = string.Format("{0:00000000}", list[i].PANO);
                strDeptCode = list[i].DEPTCODE;
                strDRCODE = list[i].DRCODE;
                strTime = list[i].YDATE.ToString();
                strName = "";
                strTel = "";
                strData = list[i].SNAME;
                strDrname = list[i].DRNAME;

                for (int j = 0; j < strData.Length; j++)
                {
                    if (string.Compare(VB.Mid(strData, j, 1), "0") >= 0 && string.Compare(VB.Mid(strData, j, 1), "9") <= 0)
                    {
                        strName += VB.Mid(strData, j, 1);
                    }
                }
                strData = list[i].HPHONE;
                for (int j = 0; j < strData.Length; j++)
                {
                    if (string.Compare(VB.Mid(strData, j, 1), "0") >= 0 && string.Compare(VB.Mid(strData, j, 1), "9") <= 0)
                    {
                        strTel += VB.Mid(strData, j, 1);
                    }
                }

                //입원환자 필터링(입원중이면 SMS전송 안되게)
                List<COMHPC> list2 = comHpcLibBService.GetIpdNewMasterbyPaNo(strPANO);

                nREAD3 = list2.Count;

                //이미 자료를 넘겼는지 확인함
                strFDate = clsPublic.GstrSysDate;
                strTDate = clsPublic.GstrSysDate + " 23:59";

                ETC_ALIMTALK list3 = etcAlimTalkService.GetRDatebyJobDatePaNoHPhoneTempCd(strFDate, strTDate, strPANO, strTel, strTempCD);

                strMinRTime = "";
                if (!list3.IsNullOrEmpty())
                {
                    strMinRTime = list3.RDATE.ToString();
                }
                //이미 전송한 예약시간이 있으면 다시 전송 않함
                if (!strMinRTime.IsNullOrEmpty() && string.Compare(strMinRTime, strTime) <= 0)
                {
                    bJOB = false;
                }

                //포스코예약자 확인
                if (comHpcLibBService.GetPoscoreservedbyPaNoExamres(strPANO, strFDate) > 0)
                {
                    bJOB = false;
                }

                //SMS 자료에 INSERT
                if (bJOB == true)
                {
                    //진료과별 회신번호 SET
                    List<BAS_DOCTOR> list4 = basDoctorService.GetItembyDrDept1DrCode(strDeptCode, strDRCODE);

                    nREAD4 = list4.Count;

                    if (nREAD4 > 0)
                    {
                        strRettel = list4[0].TELNO.Replace("-", "");
                        if (VB.Left(strRettel, 3) != "054")
                        {
                            strRettel = "054" + strRettel; //지역번호 추가(경북)
                        }
                        if (strRettel.IsNullOrEmpty())
                        {
                            strRettel = "0542720151"; //번호가 공백이면 병원대표번호로 지정
                        }
                    }
                    else
                    {
                        strRettel = "0542720151";
                    }

                    //진료과명을 READ
                    COMHPC list5 = comHpcLibBService.GetDeptNamebyDeptCode(strDeptCode);

                    strDeptName = list5.DEPTNAMEK;  //ex) 신관2층 외과, 본관1층 정형외과
                    strDeptNameS = list5.DEPTNAMES; //ex) 외과, 정형외과

                    if (list[i].DRCODE.Trim() == "1107" || list[i].DRCODE.Trim() == "1125")
                    {
                        strDeptName = "류마티스내과";
                        strDeptNameS = "류마티스내과";
                    }
                    else
                    {
                        strDeptName = strDeptName.Replace(" ", "");
                    }

                    Clear_ATK_Varient();
                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = strTime;
                    clsHcType.ATK.SendUID = strTel + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = strPANO;
                    clsHcType.ATK.sName = strName;
                    clsHcType.ATK.HPhone = strTel;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "L";
                    clsHcType.ATK.TempCD = strTempCD;
                    clsHcType.ATK.Dept = strDeptCode;   //ex)strDeptName = 신관2층외과 본관1층 정형외과 표시   StrDeptNameS = 이비인후과, 외과 표시
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = "";
                    clsHcType.ATK.JobSabun = long.Parse(clsType.User.IdNumber);

                    clsHcType.ATK.ATMsg = READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{환자명}", clsHcType.ATK.sName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{진료과명}", strDeptNameS); //진료과 ex)외과,안과,치과

                    strChangTime = VB.Left(strTime, 4) + "년 " + VB.Mid(strTime, 6, 2) + "월 " + VB.Mid(strTime, 9, 2) + "일 " + VB.Right(strTime, 5);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{날짜}", strChangTime);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{진료의사명}", clsHcType.ATK.DrName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{진료과위치}", strDeptName); //진료과위치 ex)신관2층 외과, 신관2층 안과
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{진료과번호}", VB.Left(clsHcType.ATK.RetTel, 3) + "-" + VB.Mid(clsHcType.ATK.RetTel, 4, 3) + "-" + VB.Right(clsHcType.ATK.RetTel, 4)); //진료과 번호
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{환자명}", clsHcType.ATK.sName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{진료과명}", strDeptName); //진료과 ex)신관2층 외과
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{HH}", VB.Right(strTime, 5));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{진료의사명}", clsHcType.ATK.DrName);

                    if (INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        return;
                    }

                    MYSQL_ALIMTALK_INSERT();

                    //------------( 예약자 SMS 형성 후 예약자 테이블(RESERVED_NEW)의 SMSBUILD 컬럼에 'Y' UPDATE --------------
                    result = comHpcLibBService.UpdateOpdReservedNewbyPaNoDate3DeptCodeDrCode(strPANO, strTime, strDeptCode, strDRCODE);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
            }

            //=========================ETC_SMS사용 차후 KT SMS 사용안할시 변경 로직=====================
            //2014-03-25 김민철 작성 VIP고객 예약전날 고객지원과장에게 안내문자 발송
            if (argGubun == "1")
            {
                strJobTime = clsPublic.GstrSysDate + " 13:00";
            }
            else
            {
                strJobTime = clsPublic.GstrSysDate + " 18:00";
            }
            strTel = "010-6524-3120";
            strRettel = "054-260-8003";

            //익일의 예약자를 SMS 자료로 Update
            List<COMHPC> list6 = comHpcLibBService.GetOpdReservedNewPatientbyDate3(strFDate, strTDate);

            nREAD = list6.Count;

            nWrtCnt = 0;
            for (int i = 0; i < nREAD; i++)
            {
                strPANO = string.Format("{0:00000000}", list6[i].PANO);
                strDeptCode = list6[i].DEPTCODE;
                strDRCODE = list6[i].DRCODE;
                strTime = list6[i].YDATE.ToString();
                strName = "";

                strData = list6[i].SNAME;
                for (int j = 0; j < strData.Length; j++)
                {
                    if (VB.Mid(strData, j, 1) != " ")
                    {
                        strName += VB.Mid(strData, j, 1);
                    }
                }

                //입원환자 필터링(입원중이면 SMS전송 안되게)
                List<COMHPC> list7 = comHpcLibBService.GetIpdNewMasterbyPaNo(strPANO);

                nREAD3 = list7.Count;
                strTime = VB.Left(strTime, 10);

                //이미 자료를 넘겼는지 확인함
                if (etcSmsService.GetCountbyRTimePanoDeptcode(strTime, strPANO, strDeptCode) > 0)
                {
                    strTel = "";
                }

                //SMS 자료에 INSERT
                if (!strName.IsNullOrEmpty() && !strTel.IsNullOrEmpty() && nREAD3 == 0)
                {
                    strMsg = "★VIP고객 예약알림★" + "\r\n";
                    strMsg += "성    명: " + list[i].SNAME + "\r\n";
                    strMsg += "참    고: " + list[i].GB_VIP_REMARK + "\r\n";
                    strMsg += "진 료 과: " + strDeptCode + "\r\n";
                    strMsg += "예약시각: 내일 ";

                    if (string.Compare(VB.Right(strTime, 5), "12:30") >= 0)
                    {
                        strMsg += "오후 " + VB.Right(strTime, 5);
                    }
                    else
                    {
                        strMsg += "오전 " + VB.Right(strTime, 5);
                    }

                    clsDB.setBeginTran(clsDB.DbCon);

                    //------------( 자료를 DB에 INSERT )---------------------
                    ETC_SMS item = new ETC_SMS();

                    item.JOBDATE = Convert.ToDateTime(strJobTime);
                    item.PANO = strPANO;
                    item.SNAME = strName;
                    item.HPHONE = strTel;
                    item.GUBUN = "53";
                    item.DEPTCODE = strDeptCode;
                    item.DRCODE = strDRCODE;
                    item.RTIME = Convert.ToDateTime(strTime);
                    item.RETTEL = strRettel;
                    item.SENDTIME = null;
                    item.SENDMSG = strMsg;

                    result = etcSmsService.InsertAll(item);
                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                }
            }
        }

        /// <summary>
        /// 3일전 예약자 휴대폰 문자메세지 Table 형성
        /// </summary>
        /// <param name="argGubun"></param>
        public void Yeyak_SMS_Move3(string argGubun)
        {
            /*============================================================
            '2010-10-29 김현욱
            '마감 프로그램에서 빼왔음.
            '하루 두번 생성할 예정임.
            '생성시간 정오 12:00, 오후 6시 18:00


            '2019-07-26 김해수
            'ETC_SMS 에서 ETC_ALIMTALK으로 프로그램 이동
            '
            '
            '===========================================================================


            Dim i, j              As Integer
            Dim nREAD, nREAD3     As Integer
            Dim nREAD4            As Integer
            Dim nWrtCnt           As Integer
            Dim strFDate          As String
            Dim strTDate          As String
            Dim strDelDate        As String

            Dim strOldPano        As String
            Dim strPANO           As String
            Dim strName           As String
            Dim strDeptCode       As String
            Dim strDRCODE         As String
            Dim strTime           As String
            Dim strChangTime      As String
            Dim strSuDate         As String
            Dim strTel            As String
            Dim strRettel         As String
            Dim strData           As String
            Dim strDeptName       As String
            Dim strDeptNameS      As String
            Dim strMsg            As String
            Dim strJobTime        As String

            Dim strClass          As String


            Dim strTempMSG        As String
            Dim strDrname           As String


            Dim strMinRTime     As String
            Dim bJOB            As Boolean


            '=================================
            '2010-10-29 김현욱 제외
            '사용안하고 있는 함수임
            ''ARS 전송제외번호 SET
            'Call ArsSms_Not_Send_TelNo_SET
            '================================


            Dim strTempCD       As String '알림톡 템플릿 변수


            Call READ_SYSDATE
            '2020-02-21(코로나바이러스로 템플릿 수정)
            'strTempCD = "C_MJ_001_02_13776" '알림톡 시리얼 가져와야함
            'strTempCD = "C_MJ_001_02_16882" '알림톡 시리얼 가져와야함
            'strTempCD = "C_MJ_001_02_17899" '알림톡 코로나문구 수정작업 및 날짜 작업
            strTempCD = "C_MJ_001_02_18353" '알림톡 진료과 번호 추가 작업



            strFDate = DATE_ADD(GstrSysDate, 3) '내일
            strTDate = DATE_ADD(strFDate, 1)    '모레


            '2010-10-29 김보미 과장 SMS 전송시간 변경 요청 의뢰서 작업
            If argGUBN = "1" Then
                strJobTime = GstrSysDate & " 13:00"
            Else
                strJobTime = GstrSysDate & " 18:00"
            End If

            '익일의 예약자를 SMS 자료로 Update
            SQL = "SELECT a.Pano,a.DeptCode,a.DrCode,TO_CHAR(a.DATE3,'YYYY-MM-DD HH24:MI') YDate,"
            SQL = SQL & " b.SName,b.HPhone, a.SMSBUILD, c.DRNAME "
            SQL = SQL & " FROM OPD_RESERVED_NEW a,BAS_PATIENT b, BAS_DOCTOR c "
            SQL = SQL & " WHERE a.DATE3 >= TO_DATE('" & strFDate & "','YYYY-MM-DD') "
            SQL = SQL & "  AND a.DATE3 <  TO_DATE('" & strTDate & "','YYYY-MM-DD') "
            SQL = SQL & "  AND TRANSDATE IS NULL "
            SQL = SQL & "  AND RETDATE IS NULL "
            SQL = SQL & "  AND a.Pano=b.Pano(+) "
            'SQL = SQL & "  AND a.Pano=10359385" '테스트 계정
            SQL = SQL & "  AND a.DRCODE = c.DRCODE"
            SQL = SQL & "  AND (b.GbSMS <> 'X' or b.GbSMS is null) "  '동의안한분을 제외한 모두에게 발송
            SQL = SQL & "  AND A.SMSBUILD IS NULL " '예약자 SMS형성 했는 사람 제외
            SQL = SQL & "  AND A.PANO NOT IN(SELECT PANO FROM ADMIN.NUR_STD_DEATH WHERE ACTDATE >= TRUNC(SYSDATE)-180)" '6개월이내 병원에서 사망환자 제외
            '======================================================================
            '2016-07-19 계장 김현욱 작업(예약시 문자 날아가지 않도록 외래에서 체크)
            SQL = SQL & "  AND NOT EXISTS ("
            SQL = SQL & "    SELECT * FROM ADMIN.ETC_SMS_RESNOTSEND SUB"
            SQL = SQL & "    WHERE A.PANO = SUB.PTNO"
            SQL = SQL & "        AND A.DATE3 = SUB.RDATE"
            SQL = SQL & "        AND A.DEPTCODE = SUB.DEPTCODE)"
            '======================================================================
            SQL = SQL & "  AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019') "
            SQL = SQL & "ORDER BY a.Pano,a.DeptCode "
            Call AdoOpenSet(AdoRes, SQL)
            nREAD = RowIndicator

            nWrtCnt = 0

            For i = 0 To nREAD -1
                bJOB = True


                strPANO = Format(AdoGetString(AdoRes, "Pano", i), "00000000")
                strDeptCode = Trim(AdoGetString(AdoRes, "DeptCode", i))
                strDRCODE = Trim(AdoGetString(AdoRes, "DrCode", i))

                strTime = AdoGetString(AdoRes, "YDate", i)
                strName = "":  strTel = ""

                strData = Trim(AdoGetString(AdoRes, "Sname", i))
                strDrname = Trim(AdoGetString(AdoRes, "Drname", i))


                For j = 1 To Len(strData)
                    If Mid(strData, j, 1) <> " " Then
                        strName = strName & Mid(strData, j, 1)
                    End If
                Next j
                strData = Trim(AdoGetString(AdoRes, "HPhone", i))
                For j = 1 To Len(strData)
                    If Mid(strData, j, 1) >= "0" And Mid(strData, j, 1) <= "9" Then
                        strTel = strTel & Mid(strData, j, 1)
                    End If
                Next j

                '입원환자 필터링(입원중이면 SMS전송 안되게)
                SQL = "SELECT PANO, INDATE, OUTDATE"
                SQL = SQL & vbCr & " FROM ADMIN.IPD_NEW_MASTER"
                SQL = SQL & vbCr & "  WHERE PANO = '" & strPANO & "'"
                SQL = SQL & vbCr & "  AND JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')" '입원중인 환자만 조회
                Call AdoOpenSet(rs2, SQL)
                nREAD3 = RowIndicator
                Call AdoCloseSet(rs2)

                '이미 자료를 넘겼는지 확인함
                SQL = "SELECT Min(TO_CHAR(RDate,'YYYY-MM-DD HH24:MI')) RDate "
                SQL = SQL & " FROM ETC_ALIMTALK "
                SQL = SQL & "WHERE JobDate>=TO_DATE('" & GstrSysDate & "','YYYY-MM-DD') "
                SQL = SQL & "  AND JobDate<=TO_DATE('" & GstrSysDate & " 23:59','YYYY-MM-DD HH24:MI') "
                '등록번호가 공백이 아닐경우(2019-06-15)
                If strPANO<> "" Then SQL = SQL & "  AND PANO = '" & strPANO & "' "
                SQL = SQL & "  AND HPhone ='" & strTel & "'"
                SQL = SQL & "  AND DEPTNAME ='" & strDeptCode & "'"
                SQL = SQL & "  AND TempCD = '" & strTempCD & "' " '외래 3일전 탬플릿


                Call AdoOpenSet(rs1, SQL)


                strMinRTime = ""


                If RowIndicator > 0 Then
                    strMinRTime = Trim(AdoGetString(rs1, "RDate", 0))
                End If
                Call AdoCloseSet(rs1)


                '이미 전송한 예약시간이 적으면 다시 전송 않함
                If strMinRTime<> "" Then bJOB = False


                '포스코예약자 확인
                SQL = "SELECT JDATE, PANO, SNAME, HPHONE,"
                SQL = SQL & vbCr & "TO_CHAR(EXAMRES1, 'YYYY-MM-DD HH24:MI') EXAMRES1, TO_CHAR(EXAMRES2, 'YYYY-MM-DD HH24:MI') EXAMRES2,"
                SQL = SQL & vbCr & "TO_CHAR(EXAMRES3, 'YYYY-MM-DD HH24:MI') EXAMRES3, TO_CHAR(EXAMRES4, 'YYYY-MM-DD HH24:MI') EXAMRES4,"
                SQL = SQL & vbCr & "TO_CHAR(EXAMRES6, 'YYYY-MM-DD HH24:MI') EXAMRES6, TO_CHAR(EXAMRES7, 'YYYY-MM-DD HH24:MI') EXAMRES7,"
                SQL = SQL & vbCr & "TO_CHAR(EXAMRES8, 'YYYY-MM-DD HH24:MI') EXAMRES8, TO_CHAR(EXAMRES9, 'YYYY-MM-DD HH24:MI') EXAMRES9,"
                SQL = SQL & vbCr & "TO_CHAR(EXAMRES10, 'YYYY-MM-DD HH24:MI') EXAMRES10, TO_CHAR(EXAMRES11, 'YYYY-MM-DD HH24:MI') EXAMRES11,"
                SQL = SQL & vbCr & "TO_CHAR(EXAMRES12, 'YYYY-MM-DD HH24:MI') EXAMRES12, TO_CHAR(EXAMRES13, 'YYYY-MM-DD HH24:MI') EXAMRES13,"
                SQL = SQL & vbCr & "TO_CHAR(EXAMRES14, 'YYYY-MM-DD HH24:MI') EXAMRES14, TO_CHAR(EXAMRES15, 'YYYY-MM-DD HH24:MI') EXAMRES15,"
                SQL = SQL & vbCr & "TO_CHAR(EXAMRES16, 'YYYY-MM-DD HH24:MI') EXAMRES16"
                SQL = SQL & vbCr & " FROM ADMIN.BAS_PATIENT_POSCO"
                SQL = SQL & vbCr & "  WHERE PANO = '" & strPANO & "'"
                SQL = SQL & vbCr & "  AND (TRUNC(EXAMRES1) = TO_DATE('" & strFDate & "', 'YYYY-MM-DD')"
                SQL = SQL & vbCr & "  OR TRUNC(EXAMRES2) = TO_DATE('" & strFDate & "', 'YYYY-MM-DD')"
                SQL = SQL & vbCr & "  OR TRUNC(EXAMRES3) = TO_DATE('" & strFDate & "', 'YYYY-MM-DD')"
                SQL = SQL & vbCr & "  OR TRUNC(EXAMRES4) = TO_DATE('" & strFDate & "', 'YYYY-MM-DD')"
                SQL = SQL & vbCr & "  OR TRUNC(EXAMRES6) = TO_DATE('" & strFDate & "', 'YYYY-MM-DD')"
                SQL = SQL & vbCr & "  OR TRUNC(EXAMRES7) = TO_DATE('" & strFDate & "', 'YYYY-MM-DD')"
                SQL = SQL & vbCr & "  OR TRUNC(EXAMRES8) = TO_DATE('" & strFDate & "', 'YYYY-MM-DD')"
                SQL = SQL & vbCr & "  OR TRUNC(EXAMRES9) = TO_DATE('" & strFDate & "', 'YYYY-MM-DD')"
                SQL = SQL & vbCr & "  OR TRUNC(EXAMRES10) = TO_DATE('" & strFDate & "', 'YYYY-MM-DD')"
                SQL = SQL & vbCr & "  OR TRUNC(EXAMRES11) = TO_DATE('" & strFDate & "', 'YYYY-MM-DD')"
                SQL = SQL & vbCr & "  OR TRUNC(EXAMRES12) = TO_DATE('" & strFDate & "', 'YYYY-MM-DD')"
                SQL = SQL & vbCr & "  OR TRUNC(EXAMRES13) = TO_DATE('" & strFDate & "', 'YYYY-MM-DD')"
                SQL = SQL & vbCr & "  OR TRUNC(EXAMRES14) = TO_DATE('" & strFDate & "', 'YYYY-MM-DD')"
                SQL = SQL & vbCr & "  OR TRUNC(EXAMRES15) = TO_DATE('" & strFDate & "', 'YYYY-MM-DD')"
                SQL = SQL & vbCr & "  OR TRUNC(EXAMRES16) = TO_DATE('" & strFDate & "', 'YYYY-MM-DD'))"
                Call AdoOpenSet(rs1, SQL)
                If RowIndicator > 0 Then strTel = ""
                Call AdoCloseSet(rs1)




                'SMS 자료에 INSERT
                If bJOB = True Then

                     strRettel = ""
        '==========================================================================================================
                    '진료과별 회신번호 SET
                    SQL = "SELECT DRCODE,DRDEPT1,DRNAME,TELNO,ROWID"
                    SQL = SQL & vbCr & " From ADMIN.BAS_DOCTOR"
                    SQL = SQL & vbCr & "  WHERE TOUR = 'N'"
                    SQL = SQL & vbCr & "  AND TELNO IS NOT NULL"
                    SQL = SQL & vbCr & "  AND DRDEPT1 = '" & strDeptCode & "'"
                    SQL = SQL & vbCr & "  AND DRCODE = '" & strDRCODE & "'"
                    Call AdoOpenSet(Rs, SQL)
                    nREAD4 = RowIndicator
                    If nREAD4 > 0 Then
                        strRettel = Replace(Trim(AdoGetString(Rs, "TELNO", 0)), "-", "")
                        If Left(strRettel, 3) <> "054" Then strRettel = "054" & strRettel '지역번호 추가(경북)
                        If strRettel = "" Then strRettel = "0542720151" '번호가 공백이면 병원대표번호로 지정
                    Else
                        strRettel = "0542720151"
                    End If
                    Call AdoCloseSet(Rs)


                    '------------( 휴대폰으로 전송할 문장을 SETTING )------------------
        '==============================================▼
                    '★포항성모병원★ 홍길동님 내과 예약 내일오전입니다 변경시 통화 누르세요"


                    '진료과명을 READ
                    SQL = " SELECT TOGO || ' ' || DeptNameK DEPTNAMEK, DeptNameK DeptNameS"
                    SQL = SQL & vbCr & " FROM ADMIN.BAS_CLINICDEPT A, ADMIN.BAS_CLINICDEPT_TOGO B"
                    SQL = SQL & vbCr & " WHERE A.DEPTCODE(+) = B.DEPTCODE"
                    SQL = SQL & vbCr & "      AND A.DEPTCODE ='" & strDeptCode & "' "


                    Call AdoOpenSet(rs1, SQL)


                    strDeptName = Trim(AdoGetString(rs1, "DeptNameK", 0)) 'ex) 신관2층 외과, 본관1층 정형외과
                    strDeptNameS = Trim(AdoGetString(rs1, "DeptNameS", 0)) 'ex) 외과, 정형외과


                    Call AdoCloseSet(rs1)


                    If Trim(AdoGetString(AdoRes, "DrCode", i)) = "1107" Or Trim(AdoGetString(AdoRes, "DrCode", i)) = "1125" Then
                        strDeptName = "류마티스내과"
                        strDeptNameS = "류마티스내과"
                    Else
                        strDeptName = tR(strDeptName, " ", "")
                    End If



                    Call Clear_ATK_Varient
                     '------------( 자료를 DB에 INSERT )---------------------


                    'strTel = "010-6666-7472" ' 테스트 작업


                    ATK.RDate = strTime
                    ATK.SendUID = strTel & GstrSysDate & Trim(Right(Now(), 8))
                    ATK.SendUID = Replace(Replace(Replace(Replace(ATK.SendUID, ":", ""), "-", ""), " ", ""), ".", "")
                    ATK.Pano = strPANO    '받는사람 등록번호
                    ATK.sName = strName    '받는사람이름
                    ATK.HPhone = strTel    '받는사람 전화번호
                    ATK.RetTel = Trim(strRettel) '발송 번호  진료과 번호
                    ATK.SendType = "L"
                    ATK.TempCD = strTempCD
                    ATK.Dept = strDeptCode     'ex)strDeptName = 신관2층외과 본관1층 정형외과 표시   StrDeptNameS = 이비인후과, 외과 표시
                    ATK.DrName = strDrname
                    'ATK.LtdName = "" '회사명 공란
                    ATK.JobSabun = GnJobSabun '작업자사번
                    'ATK.GJNAME = "일반검진 및 특수검진"


                    ATK.ATMsg = READ_TEMPLATE_MESSAGE(strTempCD)
                    ATK.SmsMsg = READ_TEMPLATE_SMS_MESSAGE(strTempCD)
                    ATK.ATMsg = Replace(ATK.ATMsg, "#{환자명}", ATK.sName)
                    ATK.ATMsg = Replace(ATK.ATMsg, "#{진료과명}", strDeptNameS) '진료과 ex)외과,안과,치과
                    'ATK.ATMsg = Replace(ATK.ATMsg, "#{사업장명}", ATK.LtdName)
                    'ATK.ATMsg = Replace(ATK.ATMsg, "#{YYYY}", Left(strTime, 4))
                    'ATK.ATMsg = Replace(ATK.ATMsg, "#{MM}", Mid(strTime, 6, 2))
                    'ATK.ATMsg = Replace(ATK.ATMsg, "#{DD}", Mid(strTime, 9, 2))
                    'ATK.ATMsg = Replace(ATK.ATMsg, "#{HH}", Right(strTime, 5))


                    strChangTime = Left(strTime, 4) & "년 " & Mid(strTime, 6, 2) & "월 " & Mid(strTime, 9, 2) & "일 " & Right(strTime, 5)
                    ATK.ATMsg = Replace(ATK.ATMsg, "#{날짜}", strChangTime)


                    ATK.ATMsg = Replace(ATK.ATMsg, "#{진료의사명}", ATK.DrName)
                    ATK.ATMsg = Replace(ATK.ATMsg, "#{진료과위치}", strDeptName) '진료과위치 ex)신관2층 외과, 신관2층 안과
                    ATK.ATMsg = Replace(ATK.ATMsg, "#{진료과번호}", Left(ATK.RetTel, 3) & "-" & Mid(ATK.RetTel, 4, 3) & "-" & Right(ATK.RetTel, 4)) '진료과번호


                    ATK.SmsMsg = Replace(ATK.SmsMsg, "#{환자명}", ATK.sName)
                    ATK.SmsMsg = Replace(ATK.SmsMsg, "#{진료과명}", strDeptName) '진료과위치 ex)신관2층 외과
                    'ATK.SmsMsg = Replace(ATK.SmsMsg, "#{사업장명}", ATK.LtdName)
                    'ATK.SmsMsg = Replace(ATK.SmsMsg, "#{YYYY}", Left(strTime, 4))
                    ATK.SmsMsg = Replace(ATK.SmsMsg, "#{MM}", Mid(strTime, 6, 2))
                    ATK.SmsMsg = Replace(ATK.SmsMsg, "#{DD}", Mid(strTime, 9, 2))
                    ATK.SmsMsg = Replace(ATK.SmsMsg, "#{HH}", Right(strTime, 5))
                    ATK.SmsMsg = Replace(ATK.SmsMsg, "#{진료의사명}", ATK.DrName)
                    'ATK.SmsMsg = Replace(ATK.SmsMsg, "#{진료과위치}", ATK.DeptName)


                    If INSERT_ALIMTALK_MESSAGE = False Then
                        Exit Function
                    End If


                    'Call MyadoConnect("psmh", "psmh", "psmh2")


                    Call MYSQL_ALIMTALK_INSERT

                     '2008-11-21 김현욱 작성
                     '------------( 예약자 SMS 형성 후 예약자 테이블(RESERVED_NEW)의 SMSBUILD 컬럼에 'Y' UPDATE --------------

                     SQL = " UPDATE OPD_RESERVED_NEW SET "
                     SQL = SQL & " SMSBUILD = 'Y' "
                     SQL = SQL & " WHERE PANO = '" & strPANO & "' "
                     SQL = SQL & "   AND DATE3 = TO_DATE('" & strTime & "','YYYY-MM-DD HH24:MI') "
                     SQL = SQL & "   AND DEPTCODE = '" & strDeptCode & "' "
                     SQL = SQL & "   AND DRCODE = '" & strDRCODE & "' "

                     Result = AdoExecute1(SQL)
                     If Result<> 0 Then
                         adoConnect.RollbackTrans
                         'Call ERROR_SEND("1") 미구현 작업
                         Exit Function
                     End If


                End If


            Next i

            Call AdoCloseSet(AdoRes)


        '================================================================================================
            If GstrSysDate >= "2014-03-31" Then
                '2014-03-25 김민철 작성 VIP고객 예약전날 고객지원과장에게 안내문자 발송
                If argGUBN = "1" Then
                    strJobTime = GstrSysDate & " 13:00"
                Else
                    strJobTime = GstrSysDate & " 18:00"
                End If
                strTel = "010-6524-3120"
                strRettel = "054-260-8003"


                '익일의 예약자를 SMS 자료로 Update
                SQL = "SELECT a.Pano,a.DeptCode,a.DrCode,TO_CHAR(a.DATE3,'YYYY-MM-DD HH24:MI') YDate,"
                SQL = SQL & " b.SName,b.HPhone, a.SMSBUILD,b.GB_VIP, b.GB_VIP_REMARK "
                SQL = SQL & " FROM OPD_RESERVED_NEW a,BAS_PATIENT b "
                SQL = SQL & "WHERE a.DATE3 >= TO_DATE('" & strFDate & "','YYYY-MM-DD') "
                SQL = SQL & "  AND a.DATE3 <  TO_DATE('" & strTDate & "','YYYY-MM-DD') "
                SQL = SQL & "  AND TRANSDATE IS NULL "
                SQL = SQL & "  AND RETDATE IS NULL "
                SQL = SQL & "  AND b.GB_VIP IS NOT NULL "
                SQL = SQL & "  AND a.Pano=b.Pano(+) "
                SQL = SQL & "ORDER BY a.Pano,a.DeptCode "
                Call AdoOpenSet(AdoRes, SQL)
                nREAD = RowIndicator


                nWrtCnt = 0
                For i = 0 To nREAD -1


                    strPANO = Format(AdoGetString(AdoRes, "Pano", i), "00000000")
                    strDeptCode = Trim(AdoGetString(AdoRes, "DeptCode", i))
                    strDRCODE = Trim(AdoGetString(AdoRes, "DrCode", i))


                    strTime = AdoGetString(AdoRes, "YDate", i)
                    strName = ""


                    strData = Trim(AdoGetString(AdoRes, "Sname", i))
                    For j = 1 To Len(strData)
                        If Mid(strData, j, 1) <> " " Then
                            strName = strName & Mid(strData, j, 1)
                        End If
                    Next j


                    '입원환자 필터링(입원중이면 SMS전송 안되게)
                    SQL = "SELECT PANO, INDATE, OUTDATE"
                    SQL = SQL & vbCr & " FROM ADMIN.IPD_NEW_MASTER"
                    SQL = SQL & vbCr & "  WHERE PANO = '" & strPANO & "'"
                    SQL = SQL & vbCr & "  AND JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')" '입원중인 환자만 조회
                    Call AdoOpenSet(rs2, SQL)
                    nREAD3 = RowIndicator
                    Call AdoCloseSet(rs2)


                    '이미 자료를 넘겼는지 확인함
                    SQL = "SELECT COUNT(*) CNT FROM ETC_SMS "
                    SQL = SQL & "WHERE TRUNC(Rtime)=TO_DATE('" & Left(strTime, 10) & "','YYYY-MM-DD') "
                    SQL = SQL & "  AND Pano='" & strPANO & "' "
                    SQL = SQL & "  AND DeptCode='" & strDeptCode & "' "
                    SQL = SQL & "  AND TRIM(Gubun)='53' " 'VIP 알림
                    Call AdoOpenSet(rs1, SQL)
                    If AdoGetNumber(rs1, "CNT", 0) > 0 Then strTel = ""
                    Call AdoCloseSet(rs1)


                    'SMS 자료에 INSERT
                    If strName<> "" And strTel<> "" And nREAD3 = 0 Then

                      strMsg = "★VIP고객 예약알림★" & vbLf
                        strMsg = strMsg & "성    명: " & Trim(AdoGetString(AdoRes, "SName", i)) & vbLf
                        strMsg = strMsg & "참    고: " & Trim(AdoGetString(AdoRes, "GB_VIP_REMARK", i)) & vbLf
                        strMsg = strMsg & "진 료 과: " & strDeptCode & vbLf
                        strMsg = strMsg & "예약시각: 내일 "


                        If Right(strTime, 5) >= "12:30" Then
                            strMsg = strMsg & "오후 " & Right(strTime, 5)
                        Else
                            strMsg = strMsg & "오전 " & Right(strTime, 5)
                        End If


                        adoConnect.BeginTrans


                         '------------( 자료를 DB에 INSERT )---------------------
                         SQL = "INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime,"
                         SQL = SQL & "   RetTel,SendTime,SendMsg) VALUES (TO_DATE('" & strJobTime & "','YYYY-MM-DD HH24:MI'),'"
                         SQL = SQL & strPANO & "','" & strName & "','" & strTel & "','53','" & strDeptCode & "','"
                         SQL = SQL & strDRCODE & "',TO_DATE('" & strTime & "','YYYY-MM-DD HH24:MI'),'"
                         SQL = SQL & strRettel & "','','" & strMsg & "') "


                         Result = AdoExecute1(SQL)
                         If Result<> 0 Then
                             adoConnect.RollbackTrans
                             'Call ERROR_SEND("1")
                            Exit Function
                         End If


                         adoConnect.CommitTrans
                    End If

                Next i

            End If
            */
        }

        public void EDPS_Mobile_Call_Send()
        {
            //Select Case READ_YOIL(GstrSysDate)
            //   Case "토요일":
            //        If GstrSysTime >= "13:00" And GstrSysTime <= "13:20" Then Call EDPS_DangJik_CALL_SEND
            //   Case "일요일":
            //        If GstrSysTime >= "10:00" And GstrSysTime <= "10:20" Then Call EDPS_DangJik_CALL_SEND
            //   Case Else:
            //        '평일 법정 공휴일
            //        SQL = " SELECT * FROM BAS_JOB "
            //        SQL = SQL & " WHERE JOBDATE = TO_DATE('" & GstrSysDate & "','YYYY-MM-DD') "
            //        SQL = SQL & "   AND HOLYDAY ='*' "
            //        Result = AdoOpenSet(Rs, SQL)


            //        If RowIndicator > 0 Then
            //            If GstrSysTime >= "10:00" And GstrSysTime <= "10:20" Then Call EDPS_DangJik_CALL_SEND
            //        Else
            //            If GstrSysTime >= "17:30" And GstrSysTime <= "18:00" Then Call EDPS_DangJik_CALL_SEND
            //        End If
            //        AdoCloseSet Rs
            //End Select
        }

        public void EDPS_DangJik_CALL_SEND()
        {
            //Dim i As Integer
            //Dim strHtel As String '받는사람번호
            //Dim strName As String '받는사람이름
            //Dim strRettel As String '보내는 사람 번호
            //Dim strDateTime As String
            //Dim strCheckTime As String '날짜가 있으면 bJOB = false
            //Dim strTempCD       As String '알림톡 템플릿 변수
            //Dim bJOB            As Boolean '발송유무
            //Dim strTime         As String '중복발송유무 날짜체크


            //Call READ_SYSDATE


            //strTempCD = "C_MJ_001_02_18547" '알림톡 진료과 번호 추가 작업


            //strDateTime = GstrSysDate & " " & GstrSysTime


            //SQL = "  SELECT DNAME1 , ROWID  "
            //SQL = SQL & "  FROM ADMIN.ETC_DANGJIK "
            //SQL = SQL & " WHERE GUBUN ='99' "
            //SQL = SQL & " AND TDATE =TRUNC(SYSDATE) "
            //SQL = SQL & "  AND SMS IS NULL "


            //Result = AdoOpenSet(AdoRes, SQL)


            //For i = 0 To RowIndicator -1


            //    SQL = " SELECT  HTEL, TO_CHAR(SYSDATE,'YYYY-MM-DD')TIME FROM ADMIN.INSA_MST "
            //    SQL = SQL & " WHERE KORNAME ='" & Trim(AdoRes!DNAME1 & "") & "' "
            //    SQL = SQL & "   AND BUSE ='077501' "  '전산정보과
            //    Result = AdoOpenSet(rs1, SQL)


            //    If RowIndicator > 0 Then

            //        bJOB = True


            //        strHtel = Replace(Trim(rs1!HTEL & ""), "-", "")
            //        strName = Trim(AdoRes!DNAME1 & "")
            //        strRettel = "054-260-8338"
            //        strDateTime = Replace(Mid(GstrSysDate, 6), "-", "/")
            //        strTime = Trim(rs1!Time & "")


            //        '이미 자료를 넘겼는지 확인함
            //        SQL = "SELECT Min(TO_CHAR(RDate,'YYYY-MM-DD HH24:MI')) RDate "
            //        SQL = SQL & " FROM ETC_ALIMTALK "
            //        SQL = SQL & "WHERE JobDate>=TO_DATE('" & GstrSysDate & "','YYYY-MM-DD') "
            //        SQL = SQL & "  AND JobDate<=TO_DATE('" & GstrSysDate & " 23:59','YYYY-MM-DD HH24:MI') "
            //        SQL = SQL & "  AND HPhone ='" & strHtel & "'"
            //        SQL = SQL & "  AND SNAME ='" & strName & "'"
            //        SQL = SQL & "  AND TempCD = '" & strTempCD & "' " '외래 3일전 탬플릿


            //        Call AdoOpenSet(rs2, SQL)


            //        strCheckTime = ""


            //        If RowIndicator > 0 Then
            //            strCheckTime = Trim(AdoGetString(rs2, "RDate", 0))
            //        End If
            //        Call AdoCloseSet(rs2)


            //        '이미 전송한 예약시간이 적으면 다시 전송 않함
            //        If strCheckTime<> "" Then bJOB = False


            //        If bJOB = True Then

            //            Call Clear_ATK_Varient


            //            ATK.RDate = strTime
            //            ATK.SendUID = strHtel & GstrSysDate & Trim(Right(Now(), 8))
            //            ATK.SendUID = Replace(Replace(Replace(Replace(ATK.SendUID, ":", ""), "-", ""), " ", ""), ".", "")
            //            ATK.sName = strName    '받는사람이름
            //            ATK.HPhone = strHtel    '받는사람 전화번호


            //            'ATK.HPhone = "01066667472" ' TEST


            //            ATK.RetTel = Trim(strRettel) '발송 번호  진료과 번호
            //            ATK.SendType = "L"
            //            ATK.TempCD = strTempCD
            //            ATK.JobSabun = GnJobSabun '작업자사번


            //            ATK.ATMsg = READ_TEMPLATE_MESSAGE(strTempCD)
            //            ATK.SmsMsg = READ_TEMPLATE_SMS_MESSAGE(strTempCD)
            //            ATK.ATMsg = Replace(ATK.ATMsg, "#{성명}", ATK.sName)
            //            ATK.ATMsg = Replace(ATK.ATMsg, "#{날짜}", strDateTime) '진료과 ex)외과,안과,치과


            //            ATK.SmsMsg = Replace(ATK.SmsMsg, "#{날짜}", strDateTime)


            //            If INSERT_ALIMTALK_MESSAGE = False Then
            //                Exit Function
            //            End If

            //            Call MYSQL_ALIMTALK_INSERT

            //        End If

            //    End If

            //    Call AdoCloseSet(rs1)


            //    AdoRes.MoveNext
            //Next i

            //Call AdoCloseSet(AdoRes)
        }

        public string READ_LTDNAME(string argCode)
        {
            string rtnVal = "";

            if (argCode.IsNullOrEmpty())
            {
                return rtnVal;
            }

            rtnVal = hicLtdService.GetNamebyCode(argCode);

            return rtnVal;
        }

        /// <summary>
        /// 일반검진 결과지전송
        /// </summary>
        public void HIC_RESULT_PDF_SEND()
        {
            bool bJOB = false;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";
            string strPANO = "";
            string strName = "";
            string strTime = "";
            string strSTIME = "";
            string strTel = "";
            string strLtdName = "";
            string strGjjong = "";
            string strSex = "";
            string strLINK = "";
            string strLINKJUSO = "";
            string strBIRTHDAY = "";
            string strJepDate = "";
            string strJumin = "";
            string strGubun = "";
            string strJong = "";
            string strMinRTime = "";
            string strAmPm2 = "";
            string strTempCD = "";
            long nCount = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            strTempCD = "C_MJ_001_02_13891";
            strFDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-1).ToShortDateString();
            strLINKJUSO = "https://pohangsmh.co.kr/result/view.php?param=";
            nCount = 0;

            //결과지전송 대상 조회
            List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetItembyWebPrintSend(strFDate, clsPublic.GstrSysDate);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                bJOB = true;
                strName = "";
                strTel = "";
                strJong = "";

                //파일생성시간
                strTime = list[i].JEPDATE;
                strPANO = list[i].PTNO;
                strName = list[i].SNAME;
                strTel = list[i].PHONE;
                strGubun = list[i].GUBUN;
                strJumin = VB.Left(list[i].JUMIN, 6);
                if (strGubun == "1")
                {
                    strGjjong = READ_JONG_NAME(list[i].GJJONG);
                    if (list[i].GJJONG == "11") strGjjong = "일반";
                    if (list[i].GJJONG == "31") strGjjong = "암";
                }
                else
                {
                    strGjjong = "종합";
                }

                //파일암호화 코드
                if (strGjjong == "종합")
                {
                    strJong = "83";
                }
                else
                {
                    strJong = list[i].GJJONG;
                }

                if (list[i].SEX == "M")
                {
                    strSex = "1";
                }
                else if (list[i].SEX == "F")
                {
                    strSex = "2";
                }

                strJepDate = list[i].JEPDATE;

                if (VB.Mid(clsAES.DeAES(list[i].JUMIN2), 7, 1) == "3" || VB.Mid(clsAES.DeAES(list[i].JUMIN2), 7, 1) == "4")
                {
                    strBIRTHDAY = "20" + strJumin;
                }
                else
                {
                    strBIRTHDAY = "19" + strJumin;
                }

                //이름^생년월일^성별(남자1 또는 여자2)^검진일자^환자번호
                strLINK = strLINKJUSO + clsAES.Base64Encode(strName + "^" + strBIRTHDAY + "^" + strSex + "^" + strJepDate + "^" + strPANO + "^" + strJong);

                strFDate = clsPublic.GstrSysDate;
                strTDate = clsPublic.GstrSysDate + " 23:59";

                //이미 자료를 넘겼는지 확인함
                ETC_ALIMTALK list2 = etcAlimTalkService.GetRDatebyJobDatePaNoHPhoneTempCd(strFDate, strTDate, strPANO, strTel, strTempCD);

                strMinRTime = "";
                if (!list2.IsNullOrEmpty())
                {
                    strMinRTime = list2.RDATE.ToString();
                }
                //이미 전송한 예약시간이 있으면 다시 전송 않함
                if (!strMinRTime.IsNullOrEmpty())
                {
                    bJOB = false;
                }

                //SMS 자료에 INSERT
                if (bJOB == true && nCount <= 10)
                {
                    Clear_ATK_Varient();
                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = strTime;
                    clsHcType.ATK.SendUID = strTel + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = strPANO;
                    clsHcType.ATK.sName = strName;
                    clsHcType.ATK.HPhone = strTel;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "L";
                    clsHcType.ATK.TempCD = strTempCD;
                    if (strGubun == "1")
                    {
                        clsHcType.ATK.Dept = "HR";
                    }
                    else
                    {
                        clsHcType.ATK.Dept = "TO";
                    }
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = "";
                    clsHcType.ATK.JobSabun = long.Parse(clsType.User.IdNumber);
                    clsHcType.ATK.LINK = strLINK;

                    clsHcType.ATK.ATMsg = READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", clsHcType.ATK.sName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{검진종류}", clsHcType.ATK.GJNAME);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{사업장명}", clsHcType.ATK.LtdName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(clsHcType.ATK.RDate, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(clsHcType.ATK.RDate, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(clsHcType.ATK.RDate, 9, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{링크주소}", clsHcType.ATK.LINK);

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", clsHcType.ATK.sName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{검진종류}", clsHcType.ATK.GJNAME);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{사업장명}", clsHcType.ATK.LtdName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(clsHcType.ATK.RDate, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(clsHcType.ATK.RDate, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(clsHcType.ATK.RDate, 9, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{링크주소}", clsHcType.ATK.LINK);

                    if (INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        return;
                    }

                    MYSQL_ALIMTALK_INSERT();

                    nCount += 1;
                }
            }
        }

        public string READ_JONG_NAME(string argGjJong)
        {
            string rtnVal = "";

            //건강진단 종류를 READ
            HIC_EXJONG list = hicExjongService.GetNamebyCode(argGjJong);

            if (!list.IsNullOrEmpty())
            {
                rtnVal = list.NAME;
            }

            return rtnVal;
        }

        public string READ_SECOND_Exams_Name(string argExams)
        {
            string rtnVal = "";
            long nCNT = 0;
            int nREAD = 0;
            string strCode = "";
            string strNames = "";

            nCNT = VB.L(argExams, ",");
            strNames = "";
            for (int i = 0; i < nCNT; i++)
            {
                strCode = VB.Pstr(argExams, ",", i);
                if (strCode != "3" && strCode != "6")
                {
                    HIC_CODE list = hicCodeService.GetItembyCode("53", strCode);

                    if (list.IsNullOrEmpty())
                    {
                        strNames += strCode + ",";
                    }
                    else
                    {
                        if (!list.GCODE1.IsNullOrEmpty())
                        {
                            strNames += list.GCODE1 + ",";
                        }
                        else
                        {
                            strNames += list.NAME + ",";
                        }
                    }
                }
            }

            if (!strNames.IsNullOrEmpty())
            {
                strNames = VB.Left(strNames, strNames.Length - 1);
            }

            rtnVal = strNames;

            return rtnVal;
        }

        /// <summary>
        /// 일반검진 공단검진결과지 <발송>안내-일반우편
        /// </summary>
        public void HIC_RESULT_TONGBO_SEND()
        {
            bool bJOB = false;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";
            string strPANO = "";
            string strName = "";
            string strTime = "";
            string strSTIME = "";
            string strTel = "";
            string strLtdName = "";

            string strMinRTime = "";
            string strAmPm2 = "";
            string strTempCD = "";
            string strGjjong = "";
            string strJONGNAME = "";
            string strCHUL = "";
            List<string> strJong = new List<string>();

            ComFunc.ReadSysDate(clsDB.DbCon);

            strTempCD = "C_MJ_001_02_14962";

            strFDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-2).ToShortDateString();
            strTDate = DateTime.Parse(strFDate).AddDays(1).ToShortDateString();

            strTDate += " 23:59";

            //익일의 방사선예약자를 SMS 자료로 Update
            strJong.Clear();
            strJong.Add("11");
            strJong.Add("13");
            List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetItembyTongBoDate(strFDate, strTDate, strJong);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                bJOB = true;
                strName = "";
                strTel = "";

                strPANO = list[i].PANO.ToString();
                strName = list[i].SNAME;
                strTel = list[i].HPHONE;
                strTime = list[i].RTIME.ToString();
                strLtdName = list[i].LTDNAME;
                strGjjong = list[i].GJJONG;
                strCHUL = list[i].GBCHUL;

                if (strGjjong == "11")
                {
                    strJONGNAME = "일반";
                }
                else if (strGjjong == "31")
                {
                    strJONGNAME = "암";
                }

                //이미 자료를 넘겼는지 확인함
                strFDate = clsPublic.GstrSysDate;
                strTDate = clsPublic.GstrSysDate + " 23:59";
                ETC_ALIMTALK list2 = etcAlimTalkService.GetRDatebyJobDatePaNoHPhoneTempCd(strFDate, strTDate, strPANO, strTel, strTempCD);

                if (!list2.IsNullOrEmpty())
                {
                    strMinRTime = list2.RDATE.ToString();
                }
                //이미 전송한 예약시간이 있으면 다시 전송 않함
                if (!strMinRTime.IsNullOrEmpty())
                {
                    bJOB = false;
                }

                //SMS 자료에 INSERT
                if (bJOB == true)
                {
                    Clear_ATK_Varient();
                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = strTime;
                    clsHcType.ATK.SendUID = strTel + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = strPANO;
                    clsHcType.ATK.sName = strName;
                    clsHcType.ATK.HPhone = strTel;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "A";
                    clsHcType.ATK.TempCD = strTempCD;
                    clsHcType.ATK.Dept = "HR";
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = strLtdName;
                    clsHcType.ATK.JobSabun = long.Parse(clsType.User.IdNumber);
                    clsHcType.ATK.GJNAME = strJONGNAME;

                    clsHcType.ATK.ATMsg = READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", clsHcType.ATK.sName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{검진종류명}", clsHcType.ATK.GJNAME);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(clsHcType.ATK.RDate, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2) + "일");

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", strName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{검진종류명}", clsHcType.ATK.GJNAME);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(clsHcType.ATK.RDate, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(clsHcType.ATK.RDate, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(clsHcType.ATK.RDate, 9, 2) + "일");

                    if (INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        return;
                    }

                    MYSQL_ALIMTALK_INSERT();
                }
            }
        }

        /// <summary>
        /// 일반검진 결과지 수령방문 <발송>안내-일반우편
        /// </summary>
        public void HIC_RESULT_SU()
        {
            bool bJOB = false;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";
            string strPANO = "";
            string strName = "";
            string strTime = "";
            string strSTIME = "";
            string strTel = "";
            string strLtdName = "";

            string strMinRTime = "";
            string strAmPm2 = "";
            string strTempCD = "";
            string strGjjong = "";
            string strJONGNAME = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            strTempCD = "C_MJ_001_02_14728";

            strFDate = clsPublic.GstrSysDate;
            strTDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(1).ToShortDateString();

            //익일의 방사선예약자를 SMS 자료로 Update
            List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetItembyRecvTime(strFDate, strTDate);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                bJOB = true;
                strName = "";
                strTel = "";

                strPANO = list[i].PANO.ToString();
                strName = list[i].SNAME;
                strTel = list[i].HPHONE;
                strTime = list[i].RTIME.ToString();
                strLtdName = list[i].LTDNAME;
                strGjjong = list[i].GJJONG;

                if (strGjjong == "11")
                {
                    strJONGNAME = "일반";
                }
                else if (strGjjong == "31")
                {
                    strJONGNAME = "암";
                }

                //이미 자료를 넘겼는지 확인함
                strFDate = clsPublic.GstrSysDate;
                strTDate = clsPublic.GstrSysDate + " 23:59";
                ETC_ALIMTALK list2 = etcAlimTalkService.GetRDatebyJobDatePaNoHPhoneTempCd(strFDate, strTDate, strPANO, strTel, strTempCD);

                if (!list2.IsNullOrEmpty())
                {
                    strMinRTime = list2.RDATE.ToString();
                }
                //이미 전송한 예약시간이 있으면 다시 전송 않함
                if (!strMinRTime.IsNullOrEmpty())
                {
                    bJOB = false;
                }

                //SMS 자료에 INSERT
                if (bJOB == true)
                {
                    Clear_ATK_Varient();
                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = strTime;
                    clsHcType.ATK.SendUID = strTel + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = strPANO;
                    clsHcType.ATK.sName = strName;
                    clsHcType.ATK.HPhone = strTel;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "A";
                    clsHcType.ATK.TempCD = strTempCD;
                    clsHcType.ATK.Dept = "HR";
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = strLtdName;
                    clsHcType.ATK.JobSabun = long.Parse(clsType.User.IdNumber);
                    clsHcType.ATK.GJNAME = strJONGNAME;

                    clsHcType.ATK.ATMsg = READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{사업장명}", clsHcType.ATK.LtdName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", clsHcType.ATK.sName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(clsHcType.ATK.RDate, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(strTime, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(strTime, 9, 2) + "일");

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", clsHcType.ATK.sName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(clsHcType.ATK.RDate, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(clsHcType.ATK.RDate, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(clsHcType.ATK.RDate, 9, 2) + "일");

                    if (INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        return;
                    }

                    MYSQL_ALIMTALK_INSERT();
                }
            }
        }

        public string READ_JEPSU_WORK_HPHONE(string argPtNo, string argGjJong)
        {
            string rtnVal = "";
            string strHPhone = "";

            strHPhone = hicJepsuWorkService.GetHphonebyPtNoGjJong(argPtNo, argGjJong);

            if (!strHPhone.IsNullOrEmpty())
            {
                rtnVal = strHPhone;
            }
            else
            {
                rtnVal = "";
            }

            return rtnVal;
        }
    }
}
