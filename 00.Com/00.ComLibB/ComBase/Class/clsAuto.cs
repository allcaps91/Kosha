using System;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Data;
using System.Windows.Forms;

namespace ComBase
{
    /// <summary>
    /// VB의 '무인수납.bas' Class
    /// 2018-01-03 박창욱
    /// </summary>
    public class clsAuto
    {
        public static string GstrAR_Part = "";  //무인수납 고유번호                                             Gstr무인수납_Part
        public static string GstrAREquipUse = "";  //여부       Gstr무인수납장비사용여부
        public static string GstrAREquipUsePlace = "";  //장소 5001,5002 9(소아) 5003,5004,5005,5006 8(본관)    Gstr무인수납장비사용장소
        public static string GstrAREquipCheckSentence = "";  //점검문구                                        Gstr무인수납장비점검문구
        public static string GstrAREquipObstacle = "";  //장애                                                Gstr무인수납장비장애여부
        public static string GstrAREquipProgression = "";  //진행상태                                          Gstr무인수납장비진행상태
        public static string GstrAREquipExceptionCheck = "";  //예외처리                                       Gstr무인수납장비예외체크
        public static string GstrAREquipButtonClick = "";  //버튼상황                                          Gstr무인수납장비버튼클릭
        public static string GstrAREquipAutoEnd = "";   //자동종료 여부
        public static string GstrAREquipEndTime = "";  //자동종료시간(평일)                                     Gstr무인수납장비종료시간
        public static string GstrAREquipEndTime_DayOff = "";  //자동종료시간(휴일)                              Gstr무인수납장비종료시간_휴일
        public static string GstrAROperateStartTime = "";  //운영시작시간(평일)                                 Gstr무인수납운영시작시간
        public static string GstrAROperateEndTime = "";  //운영종료시간(평일)                                   Gstr무인수납운영종료시간
        public static string GstrAROperateStartTime_DayOff = "";  //운영시작시간(휴일)                          Gstr무인수납운영시작시간_휴일
        public static string GstrAROperateEndTime_DayOff = "";  //운영종료시간(휴일)                            Gstr무인수납운영종료시간_휴일

        public static string GstrAREquipErrorSign = "";  //오류메시지표시관련                                   Gstr무인수납장비오류표시

        public static string GstrAREquipLock = "";  //LOCK 체크                                              Gstr무인수납장비LOCK
        public static string GstrAREquipQualification = "";  //의료급여자격상태                                Gstr무인수납장비자격조회


        public static string GstrAR_Pano = "";                                                             //Gstr무인수납_Pano
        public static string GstrAR_Dept = "";                                                             //Gstr무인수납_Dept
        public static string GstrAR_DrCode = "";                                                           //Gstr무인수납_DrCode
        public static string[] GstrAR_Dept2 = new string[4]; //여러과저장                                      Gstr무인수납_Dept2(3)
        public static string[] GstrAR_DrCode2 = new string[4]; //여러의사저장                                  Gstr무인수납_DrCode2(3)
        public static string GstrAR_SName = "";                                                            //Gstr무인수납_SName
        public static string GstrAR_Jumin = "";                                                            //Gstr무인수납_Jumin
        public static string GstrAR_OthersDeptCheck = "";                                                  //Gstr무인수납_타과체크
        public static string GstrAR_BDate = ""; //처방일자                                                    Gstr무인수납_BDate
        public static string GstrAR_RDate = ""; //예약일자                                                    Gstr무인수납_RDate

        public static double GnAR_AMT_Chk = 0;  //결제된금액확인
        public static double GnAR_AMT = 0;  //총본인부담금- 결재금액(본인+예약)    Gn무인수납_AMT
        public static double GnAR_AMT1 = 0;  //총본인부담금- 결재금액(본인)                                    Gn무인수납_AMT1
        public static double GnAR_AMT2 = 0;  //총본인부담금- 결재금액(예약)                                    Gn무인수납_AMT2
        public static double GnAR_AMT_temp = 0;  //총본인부담금                                              Gn무인수납_AMT_temp

        public static string GstrAR_NameCheck = "";  //성명체크                                             Gstr무인수납_성명체크

        public static int GnAR_DRUG_OUT = 0;   //원외처방전번호                                              Gn무인수납_DRUG_OUT
        public static int GnAR_DRUG_ATC = 0;   //원내투약번호                                                Gn무인수납_DRUG_ATC


        public static string GstrAR_CardState = "";                                                        //Gstr무인수납_카드상태
        public static string GstrAR_PopUpState = "";                                                       //Gstr무인수납_팝업상태

        public static string GstrAR_PopUpVariable = ""; //2103-09-27                                       Gstr무인수납_팝업변수
        public static string GstrAR_OrderCheck = ""; //2103-10-10                                          Gstr무인수납_오더창점검
        public static string GstrAR_OrderCheck2 = ""; //2103-11-11                                         Gstr무인수납_오더창점검2

        public static string GstrAR_OrderCheck_New = "";

        public static string GstrOfficeCheck = "";  //무인수납출근체크상태                                    Gstr출근CHK
        public static string GstrOfficeCheck2 = "";  //무인수납출근체크상태                                   Gstr출근CHK2

        public static string GstrOldActDate = "";  //일자변경체크                                            GstrOLDActdate
        public static string GstrPhysicalTherapyReceive = "";   //2015-10-20                               Gstr물리치료수납

        //public static string GstrEmrDoct = "";      //EmrPrt.bas
        //public static string GstrEmrViewDoct = "";  //EmrPrt.bas

        


        //================================
        public static int GnDBOPEN = 1; //OumSad2:GnDBOPEN
        public static string GstrOLDActdate = "";   //무인수납:GstrOLDActdate
        public static string GstrMirFLAG = "";      //BASACCT:GstrMirFLAG
        public static int GstrSerial = 0;       //NWSerial
        public static string GAutoCard = ""; //Card_Sign_image

        public static string Gstr100Suga = ""; //VbSugaRead_new1 : Gstr100수가적용

        public static string GstrDurgNoPrint = ""; // Drug_out_atc : Gstr약번호출력 :
        public static int GnOutDrugAuto = 0;    // Drug_out_atc : Gn원외처방번호_auto
        public static string GnDrugPatInfo1 = "";    // Drug_out_atc : Gstr환자정보1

        #region //vb접수예외처리.bas
        public static string GstrFoldChexkData = "";    //Gstr폴더체크데이타
        public static string GstJupsuExceptionGb = "";  // Gstr접수예외처리구분
        public static string GstAutoSunapUseYnb = "";  // Gstr무인수납사용여부
        public static string GstAutoMassageYn = "";  // Gstr메시지YN
        #endregion //vb접수예외처리.bas

        #region Bas_MpMaster
        //-----------------------------------------------------------------
        //>> 병원 기본정보와 서버및 FTP경로  : Bas_MpMaster
        //-----------------------------------------------------------------
        public static string g_MpFile_Name  = "";
        public static string g_MpDB_Server  = "";
        public static string g_Mp_Database  = "";
        public static string g_Mp_DbUid  = "";
        public static string g_Mp_DbPwd  = "";

        public static string g_MpHid  = "";
        public static string g_Mp_Title  = "";
        public static string g_MpFTP_SERVER  = "";
        public static string g_MpFTP_USER  = "";
        public static string g_MpFTP_PASSWORD  = "";
        public static string g_MpFtpDownload_Path  = "";
        public static string g_MpFtpUpload_path  = "";
        //-----------------------------------------------------------------
        public static string g_MpStrSysDate  = "";
        public static string g_MpStrSysTime  = "";


        public static double GnUBPayIAMT = 0;
        public static double GnUBPayAMT = 0;
        public static double GnUBPayTAMT = 0;
        public static string GnUBPayErcode = "0000";
        //'승인관련 변수
        public static string GstrApp_Date = "";                   //'승인일자
        public static string GstrApprovNo = "";                   //'승인번호
        public static string GstrInstPeriod = "";                   //'할부개월수
        public static string GstrFiName = "";                   //'발급사명
        public static string GstrFiCode = "";                   //'매입사코드
        public static string GstrAccepter = "";                   //'매입사명
        public static string GstrRMessage = "";                   //'응답메세지
        public static string GstrStatus = "";                   //'결제구분 ( 1: 결제가능여부확인, 2: 결제처리요청 )
        public static string GstrDrSabun = "";                      //'의사사번
        public static int GstrMpSerNo = 0;                          //'하렉스 번호
        public static string GstrApp_Fg = "";                       //'하렉스 번호
        public static string GstrMpHp = "";                         //'결제 헨드폰 번호
       
        #endregion Bas_MpMaster



        /// <summary>
        /// VB - READ_OCS_무인장비예외처리_CHK
        /// 2018-01-03 박창욱
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPano"></param>
        /// <param name="strBDate"></param>
        /// <param name="strDeptCode"></param>
        /// <param name="GnJobSabun"></param>
        /// <returns></returns>
        public static string Read_OCS_AutomaticEquip_Exception_Chk(PsmhDb pDbCon, string strPano, string strBDate, string strDeptCode, long GnJobSabun)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            int nAge = 0;
            int nAmt1 = 0;
            string StrJin = "";
            string strJinDtl = "";
            string strBi = "";
            string strRes = "";
            string StrDrCode = "";
            string strMCode = "";
            string strVCode = "";
            string strGelCode = "";
            string strGam = "";
            string strDementia = "";
            string strSex = "";
            string strJumin = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.Jin,a.JinDtl,a.Reserved,a.DrCode,a.MCode,a.VCode,";
                SQL = SQL + ComNum.VBLF + "       a.GelCode,a.GbGameK,a.GbDementia,a.Bi,a.Age,a.Sex, ";
                SQL = SQL + ComNum.VBLF + "       b.Jumin1,b.Jumin2,b.Jumin3, a.Amt1 ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER a, " + ComNum.DB_PMPA + "BAS_PATIENT b";
                SQL = SQL + ComNum.VBLF + " WHERE a.Pano=b.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "   AND a.PANO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE ='" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.ACTDATE =TRUNC(SYSDATE) ";  //당일접수건만

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    StrJin = dt.Rows[0]["Jin"].ToString().Trim();
                    strJinDtl = dt.Rows[0]["JinDtl"].ToString().Trim();
                    strBi = dt.Rows[0]["Bi"].ToString().Trim();
                    strRes = dt.Rows[0]["Reserved"].ToString().Trim();
                    StrDrCode = dt.Rows[0]["DrCode"].ToString().Trim();
                    strMCode = dt.Rows[0]["MCode"].ToString().Trim();
                    strVCode = dt.Rows[0]["VCode"].ToString().Trim();
                    strGelCode = dt.Rows[0]["GelCode"].ToString().Trim();
                    strGam = dt.Rows[0]["GbGameK"].ToString().Trim();
                    strDementia = dt.Rows[0]["GbDementia"].ToString().Trim();
                    strSex = dt.Rows[0]["Sex"].ToString().Trim();
                    nAge = (int)VB.Val(dt.Rows[0]["Age"].ToString().Trim());
                    nAmt1 = (int)VB.Val(dt.Rows[0]["Amt1"].ToString().Trim());

                    if (dt.Rows[0]["Jumin3"].ToString().Trim() != "")
                    {
                        strJumin = dt.Rows[0]["Jumin1"].ToString().Trim() + clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                    }
                    else
                    {
                        strJumin = dt.Rows[0]["Jumin1"].ToString().Trim() + dt.Rows[0]["Jumin2"].ToString().Trim();
                    }

                    rtnVar = Read_AutomaticEquip_Exception_Chk1(clsDB.DbCon, "ORDER", StrJin, strJinDtl, strRes, strPano, ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), strDeptCode, StrDrCode, strBi, nAge, strMCode, strVCode, strGelCode, strGam, "", "", strDementia, strJumin, nAmt1, GnJobSabun);
                }
                else
                {
                    //무인수납대상 불가
                    rtnVar = "NO";
                }

                dt.Dispose();
                dt = null;

                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }

        /// <summary>
        /// VB - Read_무인장비예외처리_CHK1
        /// 2018-01-03 박창욱
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strChk"></param>
        /// <param name="strJin"></param>
        /// <param name="strJinDtl"></param>
        /// <param name="strRes"></param>
        /// <param name="strPano"></param>
        /// <param name="strActDate"></param>
        /// <param name="strDeptCode"></param>
        /// <param name="strDrCode"></param>
        /// <param name="strBi"></param>
        /// <param name="argAge"></param>
        /// <param name="strMCode"></param>
        /// <param name="strVCode"></param>
        /// <param name="strGelCode"></param>
        /// <param name="strGam"></param>
        /// <param name="strRDate"></param>
        /// <param name="strRTime"></param>
        /// <param name="strDementia"></param>
        /// <param name="strJumin"></param>
        /// <param name="nAmt1"></param>
        /// <param name="GnJobSabun"></param>
        /// <returns></returns>
        /// 
      
        public static string Read_AutomaticEquip_Exception_Chk1(PsmhDb pDbCon, string strChk, string strJin, string strJinDtl, string strRes, string strPano, string strActDate, string strDeptCode, string strDrCode, string strBi, int argAge, string strMCode, string strVCode, string strGelCode, string strGam, string strRDate, string strRTime, string strDementia, string strJumin, int nAmt1, long GnJobSabun)
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nAge = 0;
            int nBuCount = 0;
            int nBuCount3 = 0;
            string StrTemp = "";
            string strTemp2 = "";
            string strChojae = "";
            string strMsg = "";
            string strSysTime = "";
            string strSysDate = "";
            string rtnVar = "";

            int GnDrugNal = 0;  //TODO : 전역변수

            ComFunc cf = new ComFunc();

        
            rtnVar = "";
            GstrAREquipExceptionCheck = ""; //메시지
            GstrAREquipQualification = "";
            GstrAR_RDate = "";
            GstrAREquipCheckSentence = "무인수납 불가";

            //약 관련
            GnAR_DRUG_OUT = 0;
            GnAR_DRUG_ATC = 0;

            strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            try
            {
                if (VB.Left(strPano, 6) != "810000")
                {
                    if (GstrOfficeCheck == "휴일" || GstrOfficeCheck == "일요일")
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "무인수납 사용시간 아님";
                        return rtnVar;
                    }
                    else if (GstrOfficeCheck == "토요일")
                    {
                        if (GnJobSabun == 5005)
                        {
                            if (string.Compare(strSysTime, "17:30") > 0)
                            {
                                rtnVar = "NO";
                                GstrAREquipExceptionCheck = "무인수납 사용시간 아님";
                                return rtnVar;
                            }
                        }
                        else
                        {
                            if (string.Compare(strSysTime, GstrAREquipEndTime) > 0)
                            {
                                rtnVar = "NO";
                                GstrAREquipExceptionCheck = "무인수납 사용시간 아님";
                                return rtnVar;
                            }
                        }
                    }
                    else if (GstrOfficeCheck == "평일")
                    {
                        if (GnJobSabun == 5005)
                        {
                            if (string.Compare(strSysTime, "17:30") > 0)
                            {
                                rtnVar = "NO";
                                GstrAREquipExceptionCheck = "무인수납 사용시간 아님";
                                return rtnVar;
                            }
                        }
                        else
                        {
                            if (string.Compare(strSysTime, GstrAREquipEndTime) > 0)
                            {
                                rtnVar = "NO";
                                GstrAREquipExceptionCheck = "무인수납 사용시간 아님";
                                return rtnVar;
                            }
                        }
                    }
                }

                //무인수납 장비 사용여부, 점검 체크
                StrTemp = AREquip_Obstacle_Check(clsDB.DbCon);
                if (StrTemp == "OK")
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "무인수납 장애서비스 점검중";
                    return rtnVar;
                }
                else if (StrTemp == "OK2")
                {
                    GstrAREquipQualification = "OK";
                }

                //물리치료 무인수납 장애체크
                StrTemp = PhysicalTherapy_AR_Check(clsDB.DbCon);
                if (StrTemp == "OK")
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "물리치료 무인수납 장애 점검중";
                    return rtnVar;
                }
                else if (StrTemp == "OK2")
                {
                    GstrAREquipQualification = "OK";
                }

                StrTemp = "";

                //LOCK 확인 - 무인수납 Lock 제외
                if (strChk == "")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT Remark, TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') EntTime";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OLOCK ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND Remark <>'수납중입니다(auto)' ";
                    SQL = SQL + ComNum.VBLF + "   AND TRUNC(ENTDATE) = TRUNC(SYSDATE) ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "처방수정 및 입력중";
                        dt.Dispose();
                        dt = null;
                        return rtnVar;
                    }
                    dt.Dispose();
                    dt = null;
                }

                //신생아 주민번호 체크
                if (strJumin != "")
                {
                    switch (VB.Right(strJumin, 7))
                    {
                        case "0000000":
                        case "1000000":
                        case "1000001":
                        case "2000000":
                        case "2000001":
                        case "3000000":
                        case "3000001":
                        case "4000000":
                        case "5000000":
                        case "6000000":
                        case "7000000":
                        case "8000000":
                        case "9000000":
                            if (VB.Left(strPano, 6) != "810000")
                            {
                                rtnVar = "NO";
                                GstrAREquipExceptionCheck = "주민번호 뒷자리 오류";
                                return rtnVar;
                            }
                            break;
                    }
                }

                if (string.Compare(VB.Left(strBi, 1), "3") < 0)
                {
                    if (Convert.ToDateTime(strActDate) <= Convert.ToDateTime("2014-11-24") && VB.Left(strDeptCode, 1) == "M" &&
                       VB.Right(strDrCode, 2) != "99")
                    {
                        if (strJin == "E")
                        {
                            if (cf.Check_Dept_Certified(clsDB.DbCon, strDrCode, strActDate) == false)
                            {
                                rtnVar = "NO";
                                GstrAREquipExceptionCheck = "비인증전문의 전화접수";
                                return rtnVar;
                            }
                        }

                        //비인증전문의 경우
                        if (cf.Check_Dept_Certified(clsDB.DbCon, strDrCode, strActDate) == false)
                        {
                            if (nAmt1 > 0 || (strRes == "1") || (strJin == "5")) 
                            {
                                strMsg = cf.Check_Dept_ReceiveHis(clsDB.DbCon, strPano, strActDate, "비인증", strDeptCode, strDrCode, nAmt1);
                                //비인증전문의 진료비가 있다면
                                if (strMsg != "")
                                {
                                    rtnVar = "NO";
                                    GstrAREquipExceptionCheck = "비인증전문의 진료";
                                    return rtnVar;
                                }
                            }

                            //인증전문의 진료비가 있는지 -> 현재것 진료비 산정안함
                            if (nAmt1 > 0 ||  (strRes == "1") || (strJin == "5"))
                            {
                                strMsg = cf.Check_Dept_ReceiveHis(clsDB.DbCon, strPano, strActDate, "인증", strDeptCode, strDrCode, nAmt1);
                                //비인증전문의 진료비가 있다면
                                if (strMsg != "")
                                {
                                    rtnVar = "NO";
                                    GstrAREquipExceptionCheck = "비인증전문의 진료";
                                    return rtnVar;
                                }
                            }
                        }
                        else
                        {
                            //인증전문의 경우
                            //비인증전문의 진료비가 있다면
                            strMsg = cf.Check_Dept_ReceiveHis(clsDB.DbCon, strPano, strActDate, "비인증", strDeptCode, strDrCode, nAmt1);
                            //비인증전문의 진료비가 있다면
                            if (strMsg != "")
                            {
                                rtnVar = "NO";
                                GstrAREquipExceptionCheck = "비인증전문의 진료";
                                return rtnVar;
                            }
                        }
                    }
                }

                //선택진료관련 긴급예외추가  - 프로그램 보완 추 삭제요망
                if (strRes == "1")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI') DATE3, DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND DATE1 < TO_DATE('2014-08-01','YYYY-MM-DD') ";  //8/1일 이전건
                    SQL = SQL + ComNum.VBLF + "   AND TRUNC(TRANSDATE) =TRUNC(SYSDATE) ";  //당일 접수전환
                    SQL = SQL + ComNum.VBLF + "   AND (RETDATE IS NULL OR RETDATE = '') "; //환불 안된것
                    SQL = SQL + ComNum.VBLF + "   AND AMT2 > 0 ";  //선택금액 발생건

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT ROWID ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER  ";
                        SQL = SQL + ComNum.VBLF + " WHERE Ptno= '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDeptCode + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND GBSunap ='0' ";  //미수납만
                        SQL = SQL + ComNum.VBLF + "   AND SUBSTR(SuCode,1,2) <> '$$' ";  //보호자 내원코드 점검
                        SQL = SQL + ComNum.VBLF + "   AND SUCODE IS NOT NULL ";
                        SQL = SQL + ComNum.VBLF + "   AND BUN NOT IN ('11','12') ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return rtnVar;
                        }

                        if (dt1.Rows.Count == 0)
                        {
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "9월 1일 이전 예약";
                            dt.Dispose();
                            dt = null;

                            dt1.Dispose();
                            dt1 = null;

                            return rtnVar;
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }

                    dt.Dispose();
                    dt = null;
                }

                StrTemp = "";
                //B항 수가항목 체크
                for (i = 1; i < 4; i++)
                {
                    StrTemp = i.ToString();
                    if (i == 1)
                    {
                        StrTemp = "";
                    }

                    strMsg = cf.Check_SugbB(clsDB.DbCon, "ETC_B_SUCHK" + StrTemp, strPano, strDeptCode);
                    if (strMsg != "")
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = strMsg;
                        return rtnVar;
                    }
                }

                //진료의사 본과접수 안됨(병원장 지시사항)
                if (ComFunc.CHK_Practitioner_RegularDeptReceive(clsDB.DbCon, strPano, strDrCode) == true)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "진료의사 본과접수";
                    return rtnVar;
                }

                //대리접수인데 소아6세 미만 제외
                if (strJin == "5" && argAge < 6)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "소아 만 6세 미만 대리접수";
                    return rtnVar;
                }

                //소아과 소아6세 미만 제외
                if (strJin != "8" && strJin != "U" && strJin != "S" && strJin != "T" && strJin != "R" && argAge < 6)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "소아 만6세미만 소아접수R";
                    return rtnVar;
                }

                //FM/DT는 일반자격 가능하게 함
                if (strDeptCode != "DT" && strDeptCode != "FM")
                {
                    if (strBi != "11" && strBi != "12" && strBi != "13")
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "무인수납은 건강보험자격만가능";
                        return rtnVar;
                    }
                }
                else
                {
                    if (strBi != "11" && strBi != "12" && strBi != "13" && strBi != "51")
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "FM/DT무인수납은 건보,일반자격만가능";
                        return rtnVar;
                    }
                }

                //의료급여제외 + 의료급여 승인에러시
                if (strBi == "21" || strBi == "22" || GstrAREquipQualification == "OK")
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "의료급여점검";
                    return rtnVar;
                }

                //정신과 원내조제의 경우 접수구분에 F003 확인
                if (strDeptCode == "NP")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT a.PTNO, a.DEPTCODE, a.SuCode, b.SName ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                    SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                    SQL = SQL + ComNum.VBLF + "   AND a.SuCode IN ( '##14' ) ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (strBi == "21" || strBi == "22")
                        {
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "NP의료급여-##14코드점검";
                            dt.Dispose();
                            dt = null;
                            return rtnVar;
                        }

                        if (strVCode.Trim() != "F003")
                        {
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "NP원내조제-F003코드점검";
                            dt.Dispose();
                            dt = null;
                            return rtnVar;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }


                if (strBi == "31" || strBi == "52" || strBi == "33" || strBi == "55")
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "산재,교통점검";
                    return rtnVar;
                }

                //F003 제외 
              //  if (strVCode == "F003")
              //  {
              //      rtnVar = "NO";
              //      GstrAREquipExceptionCheck = "F003점검";
              //      return rtnVar;
              //  }

                //차상위 E, F, I, J 대상 체크
                if (strMCode == "E000" || strMCode == "F000" && (strJin != "I" && strJin != "J"))
                {
                    if (M3_HIC_13_Cha2_AutoCHK(clsDB.DbCon, strPano, strBi, strDeptCode, strSysDate, strMCode, strVCode) == "OK")
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "차상위환자I,J점검";
                        return rtnVar;
                    }
                }

                //접수2제외,신생아,진단서,후불,결핵쿠폰
                if (strJin == "2" || strJin == "3" || strJin == "4" || strJin == "1" || strJin == "L")
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "(" + strJin + ") 접수2,후불,결핵,전화..점검";
                    return rtnVar;
                }

                StrTemp = "";

                //전화예약 수납가능구분
                if (strJin == "E")
                {
                    //전화예약자는 중증,특수자격 제외함
                    if (strVCode != "" || strMCode != "" || (strJinDtl != "" && strJinDtl != "05" ))
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "전화예약점검ETC";
                        return rtnVar;
                    }

                    if (CHK_AR_TelephoneBooking(clsDB.DbCon, strPano, strActDate, strDeptCode) != "OK")
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "전화예약점검C";
                        return rtnVar;
                    }
                }

                StrTemp = "";

                ////초음파급여 오더체크
                //SQL = "";
                //SQL = SQL + ComNum.VBLF + "SELECT SUCODE,SUM(QTY*NAL)";
                //SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER ";
                //SQL = SQL + ComNum.VBLF + " WHERE Ptno     = '" + strPano + "' ";
                //SQL = SQL + ComNum.VBLF + "   AND BDate    = TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                //SQL = SQL + ComNum.VBLF + "   AND DeptCode = '" + strDeptCode + "' ";
                //SQL = SQL + ComNum.VBLF + "   AND GbSunap  = '0' ";
                //SQL = SQL + ComNum.VBLF + "   AND Nal      >  0  ";
                //SQL = SQL + ComNum.VBLF + "   AND BUN IN ('49') ";
                //SQL = SQL + ComNum.VBLF + " GROUP BY SUCODE ";
                //SQL = SQL + ComNum.VBLF + "HAVING SUM(QTY*NAL) > 0 ";

                //SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                //if (SqlErr != "")
                //{
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //    return rtnVar;
                //}
                //if (dt.Rows.Count > 0)
                //{
                //    if (!(strMCode == "V000" || strVCode == "V193"))
                //    {
                //        rtnVar = "NO";
                //        GstrAREquipExceptionCheck = "초음파급여코드점검";
                //        dt.Dispose();
                //        dt = null;
                //        return rtnVar;
                //    }
                //}

                //dt.Dispose();
                //dt = null;


                //XCDC(의무기록사본) 오더체크
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SUCODE,SUM(QTY*NAL)";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE Ptno     = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDate    = TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GbSunap  = '0' ";
                SQL = SQL + ComNum.VBLF + "   AND Nal      >  0  ";
                SQL = SQL + ComNum.VBLF + "   AND (BUN IN ('75') OR SUCODE ='XCDC' OR SUCODE = 'XDVDC' OR SUCODE = 'CUSCOPY') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY SUCODE ";
                SQL = SQL + ComNum.VBLF + "HAVING SUM(QTY*NAL) > 0 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "제증명오더점검(의무기록사본)";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                //계약처 일시 제외
                if (strGelCode == "H023" || strGelCode == "H027" || strGelCode == "H122" || strGelCode == "H128")
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "계약처점검";
                    return rtnVar;
                }

                //ER
                if (strDeptCode == "ER")
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "응급실점검";
                    return rtnVar;
                }

                //HD 점검
                if (strDeptCode == "HD")
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "인공신장접수점검";
                    return rtnVar;
                }

                //오더없음 체크
                if (strJin != "8" && strJin != "U" && strJin != "G" && strJin != "T")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT a.PTNO, a.DEPTCODE, a.SuCode, b.SName ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                    SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                    SQL = SQL + ComNum.VBLF + "   AND a.SuCode IS NOT NULL ";  //수가만
                    SQL = SQL + ComNum.VBLF + "   AND a.OrderCode != 'PT######'   ";
                    SQL = SQL + ComNum.VBLF + "   AND a.OrderCode != 'NSA'       ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }
                    if (dt.Rows.Count == 0 && strRDate == "")
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "오더발생없음";
                        dt.Dispose();
                        dt = null;
                        return rtnVar;
                    }

                    dt.Dispose();
                    dt = null;
                }

                //오더 체크
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                SQL = SQL + ComNum.VBLF + "   AND a.NAL < 0 ";
                SQL = SQL + ComNum.VBLF + "   AND a.SuCode IS NOT NULL ";  //수가만

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "-오더발생점검";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                //부가세 수가체크
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                SQL = SQL + ComNum.VBLF + "   AND a.GbTax  ='1' ";
                SQL = SQL + ComNum.VBLF + "   AND a.SuCode IS NOT NULL ";  //수가만

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "부가세수가발생";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                //당일과 약처방전 발생한후 추가 약처방있을경우 제외(재정산 가능성있어서)
                if (VB.Left(strPano, 6) != "810000")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT Pano ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
                    SQL = SQL + ComNum.VBLF + " WHERE SlipDate=TO_DATE('" + strActDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND Pano ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDeptCode + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                        SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                        SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                        SQL = SQL + ComNum.VBLF + "   AND a.NAL > 0 ";
                        SQL = SQL + ComNum.VBLF + "   AND a.Bun IN ('11','12','20')  "; //약만
                        SQL = SQL + ComNum.VBLF + "   AND a.SuCode IS NOT NULL ";  //수가만

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return rtnVar;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "당일 약처방전발생후 약오더발생";
                            dt.Dispose();
                            dt = null;
                            dt1.Dispose();
                            dt1 = null;
                            return rtnVar;
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }

                    dt.Dispose();
                    dt = null;
                }


                //예방접종 코드 발생하면 인적정보 있어야함
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                SQL = SQL + ComNum.VBLF + "   AND a.SuCode IN ( SELECT JEPCODE FROM " + ComNum.DB_MED + "ETC_JUSACODE WHERE";
                SQL = SQL + ComNum.VBLF + "                     (DELDATE IS NULL  OR DELDATE ='') )  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT PANO ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "VACCINE_TPATIENT ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND LAST_UPDATE >='" + VB.Replace(strActDate, "-", "").Trim() + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }

                    if (dt1.Rows.Count == 0)
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "예방접종기초정보점검";
                        dt.Dispose();
                        dt = null;
                        dt1.Dispose();
                        dt1 = null;
                        return rtnVar;
                    }

                    dt1.Dispose();
                    dt1 = null;
                }

                dt.Dispose();
                dt = null;


                //필수예방접종 코드 발생하면 제외
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                SQL = SQL + ComNum.VBLF + "   AND a.SuCode IN ( SELECT SuCode FROM " + ComNum.DB_PMPA + "BAS_VACC_MST WHERE  GUBUN ='1'  AND";
                SQL = SQL + ComNum.VBLF + "                     (DELDATE IS NULL  OR DELDATE ='') )  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "필수예방접종점검";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;



                //중증코드 점검
                if (strVCode == "V193")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                    SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                    SQL = SQL + ComNum.VBLF + "   AND a.SuCode ='@V193' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "접수V193-@V193점검";
                        dt.Dispose();
                        dt = null;
                        return rtnVar;
                    }

                    dt.Dispose();
                    dt = null;
                }


                //중증코드 점검
                if (strVCode == "")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                    SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                    SQL = SQL + ComNum.VBLF + "   AND a.SuCode ='@V193' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "접수V193-@V193점검";
                        dt.Dispose();
                        dt = null;
                        return rtnVar;
                    }

                    dt.Dispose();
                    dt = null;
                }



                //$$ 특정코드 무인수납 점검
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                SQL = SQL + ComNum.VBLF + "   AND a.SuCode IN ('$$20','$$35','$$33','$$34','$$52','$$21','$$14','$$11','$$56','$$45','$$60') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "$$특정기호 코드 점검(" + dt.Rows[0]["SUCODE"].ToString().Trim() + ")";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;

                //진찰료수납전 보호자내원 코드 발생 점검

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                SQL = SQL + ComNum.VBLF + "   AND a.SuCode IN ('$$42') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count == 0  && strJinDtl == "05")
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "보호자내원 코드 누락 점검($$42)";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                //진찰료수납후 보호자내원 코드 발생 점검
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                SQL = SQL + ComNum.VBLF + "   AND a.SuCode IN ('$$42') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0  && nAmt1 > 0 && strJinDtl !="05" )
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "진찰료수납후 보호자내원 코드 발생 점검($$42)";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                //@F010 결핵지원금 점검
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                SQL = SQL + ComNum.VBLF + "   AND a.SuCode IN ('@F010','@F015','@F016') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "@F코드점검(" + dt.Rows[0]["SUCODE"].ToString().Trim() + ")";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                //FM가정의학과 이상엽과장만 - 오더시 분리수납일경우, 처방시 $$코드점검
                if (strDeptCode == "FM" && strDrCode == "1404")
                { 
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                    SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                    SQL = SQL + ComNum.VBLF + "   AND a.GbFM ='1' ";   //분리수납건

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "FM-분리수납점검";
                        dt.Dispose();
                        dt = null;
                        return rtnVar;
                    }

                    dt.Dispose();
                    dt = null;
                }


                //$$40, $$41 점검
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                SQL = SQL + ComNum.VBLF + "   AND a.SuCode IN ('$$40','$$41') ";  //보험총액43,일반자격51 수납요청

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count == 1)
                {
                    StrTemp = dt.Rows[0]["SuCode"].ToString().Trim();

                    if (StrTemp == "$$40" && strBi != "43")
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "접수구분변경-43종아님($$40)";
                        dt.Dispose();
                        dt = null;
                        return rtnVar;
                    }
                    else if (StrTemp == "$$41" && strBi != "51")
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "접수구분변경-51종아님($$41)";
                        dt.Dispose();
                        dt = null;
                        return rtnVar;
                    }
                }
                else if (dt.Rows.Count > 1)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "접수구분변경_수가확인($$40,$$41)";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;




                //당일 ER오더있으면 제외됨
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_iORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.GbIOE='E' "; //ER 오더
                SQL = SQL + ComNum.VBLF + "   AND a.GbAct ='*' ";  //미수납만

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "당일응급실오더발생";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;



                //중증화상코드 점검
                if (strVCode == "V247" || strVCode == "V248" || strVCode == "V249" || strVCode == "V250")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                    SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                    SQL = SQL + ComNum.VBLF + "   AND a.SuCode IN ('@V247','@V248','@V249','@V250' ) ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "접수화상-V코드점검";
                        dt.Dispose();
                        dt = null;
                        return rtnVar;
                    }

                    dt.Dispose();
                    dt = null;
                }



                //결핵접수체크
                if ((strBi == "11" || strBi == "12" || strBi == "13") && strVCode != "EV01" && strVCode != "V001")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                    SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                    SQL = SQL + ComNum.VBLF + "   AND a.SuCode IN ('@V206','@V246','@V000','@V010' ) ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "접수기본-결핵수가발생점검";
                        dt.Dispose();
                        dt = null;
                        return rtnVar;
                    }

                    dt.Dispose();
                    dt = null;
                }



                //상병특례 + 희귀난치 체크
                nBuCount = 0;
                nBuCount3 = 0;

                if (strBi == "11" || strBi == "12" || strBi == "13")
                {
                    //상병특례
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                    SQL = SQL + ComNum.VBLF + "  WHERE a.Ptno=b.Pano ";
                    SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                    SQL = SQL + ComNum.VBLF + "   AND a.SuCode IN (  SELECT SUNEXT FROM " + ComNum.DB_PMPA + "BAS_SUN WHERE";
                    SQL = SQL + ComNum.VBLF + "                    (GBRARE <> 'Y' OR GBRARE IS NULL)  AND SUBSTR(SUNEXT,1,2) = '@V'  AND";
                    SQL = SQL + ComNum.VBLF + "                    SUNEXT NOT IN ('@V193','@V194','@V247','@V248','@V249','@V250','@V252','@V352') ) ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        nBuCount = 1;
                    }

                    dt.Dispose();
                    dt = null;


                    //희귀.난치성질환자
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                    SQL = SQL + ComNum.VBLF + "  WHERE a.Ptno=b.Pano ";
                    SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                    SQL = SQL + ComNum.VBLF + "   AND a.SuCode IN (  SELECT SUNEXT FROM " + ComNum.DB_PMPA + "BAS_SUN WHERE GBRARE = 'Y' AND";
                    SQL = SQL + ComNum.VBLF + "       SUBSTR(SUNEXT,1,2) = '@V'  AND SUNEXT NOT IN ('@V247','@V248','@V249','@V250','@V252','@V352') ) ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        nBuCount3 = 1;
                    }

                    dt.Dispose();
                    dt = null;



                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                    SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                    SQL = SQL + ComNum.VBLF + "   AND a.SuCode IN ('@V193','@V252') ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        if (strJin != "F" && strJin != "G" && strJin != "A" && strJin != "C" && argAge >= 6)
                        {
                            if (strMCode != "C000" && strMCode == "E000" && strMCode == "F000")
                            {
                                rtnVar = "NO";
                                GstrAREquipExceptionCheck = "상병특례수가발생-접수점검";
                                dt.Dispose();
                                dt = null;
                                return rtnVar;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;


                    //희귀, 난치성 질환자
                    if (strMCode == "H000")
                    {
                        if (strJin != "F" && strJin != "G" && strJin != "S" && strJin != "T" && strJin != "A" && strJin != "C" && strJin != "6")
                        {
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "희귀난치H접수구분점검";
                            return rtnVar;
                        }
                        else if (nBuCount3 == 0)
                        {
                            //희귀, 난치성 수가코드 없음
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "희귀난치H코드점검";
                            return rtnVar;
                        }
                        else if (nBuCount >= 1)
                        {
                            //상병특례 수가코드 발생
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "희귀난치H-상병특례코드발생점검";
                            return rtnVar;
                        }
                        else if (nBuCount >= 1 && nBuCount3 >= 1)
                        {
                            //희귀.난치성수가코드와 상병특례 수가코드
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "희귀난치H코드+상병특례코드발생점검";
                            return rtnVar;
                        }
                    }
                    else if (strMCode == "")
                    {
                        if (nBuCount == 0 && (strJin == "F" || strJin == "G" || strJin == "S" || strJin == "T" || strJin == "C"))
                        {
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "희귀난치상병특례수가점검1";
                            return rtnVar;
                        }
                        else if (nBuCount >= 1 && strJin != "F" && strJin != "G" && strJin != "S" && strJin != "T" && strJin == "C")
                        {
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "희귀난치상병특례수가점검2";
                            return rtnVar;
                        }
                        else if (nBuCount3 >= 1)
                        {
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "희귀난치상병특례수가발생점검";
                            return rtnVar;
                        }
                    }


                    //희귀난치질환 점검
                    if (strMCode == "V000")     //희귀, 난치성 질환자
                    {
                        if (strJin != "F" && strJin != "G" && strJin != "S" && strJin != "T" && strJin != "A" && strJin != "6" && strJin != "9" && strJin != "8")
                        {
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "희귀난치V-접수구분점검";
                            return rtnVar;
                        }
                        else if (nBuCount3 == 0)    //희귀, 난치성수가코드 없음
                        {
                            if (strDeptCode != "HD")
                            {
                                rtnVar = "NO";
                                GstrAREquipExceptionCheck = "희귀난치V점검";
                                return rtnVar;
                            }
                        }
                        else if (nBuCount >= 1 && nBuCount3 >= 1)
                        {
                            //희귀, 난치성수가코드와 상병특례 수가코드
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "희귀난치V+상병특례발생점검";
                            return rtnVar;
                        }
                        else if (nBuCount >= 1)
                        {
                            //상병특례 수가코드 발생
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "희귀난치V-상병특례발생점검";
                            return rtnVar;
                        }
                    }
                    else if (strMCode == "")
                    {
                        if (nBuCount >= 1 && strJin != "F" && strJin != "G" && strJin != "S" && strJin != "T" && strJin != "C")
                        {
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "상병특례발생점검A";
                            return rtnVar;
                        }
                    }
                }



                //협력병원 예외적용
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ETC_RETURN ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND H_CODE IN ( SELECT CODE FROM " + ComNum.DB_PMPA + "ETC_RETURN_CODE";
                SQL = SQL + ComNum.VBLF + "                    WHERE GUBUN ='01' AND H_GUBUN ='Y' ) ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDeptCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "협력병의원의뢰환자점검";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                //예약자 자격점검
                StrTemp = "";
                if (strRes == "1" && strActDate == strSysDate && VB.Left(strPano, 6) != "810000")
                {
                    StrTemp = READ_OPD_NHIC_Qualification_2(clsDB.DbCon, strSysDate, strPano, strBi, strDeptCode, "", "1");
                    //StrTemp = READ_OPD_NHIC_Qualification_2_BACKUP(clsDB.DbCon, strSysDate, strPano, strBi, strDeptCode, "", "1");

                    if (StrTemp != "")
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "예약자자격점검";
                        return rtnVar;
                    }

                    if (argAge == 5 && (strBi == "11" || strBi == "12" || strBi == "13"))
                    {
                        nAge = ComFunc.AgeCalcEx(strJumin, strActDate);

                        if (nAge > argAge && nAge == 6)
                        {
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "예약자나이(소아)체크";
                            return rtnVar;
                        }
                    }
                }


                //접수된 것 감액을 실제감액 점검
                switch (strGam)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                    case "26":
                    case "27":
                        if (Gam_Pano_Search_2(clsDB.DbCon, strGam, VB.Left(strJumin, 6), VB.Right(strJumin, 7)) != "OK")
                        {
                            if (VB.Left(strPano, 6) != "810000")
                            {
                                rtnVar = "NO";
                                GstrAREquipExceptionCheck = "감액대상체크";
                                return rtnVar;
                            }
                        }
                        break;
                }


                StrTemp = "";

                //예약있을경우 예약일자 시간 점검
                //토요일 12시 이 후에는 예약 안 됨.
                //if (strRDate.Trim() != "")
                //{
                //    //공휴일에는 예약이 안 됨
                //    if (clsVbfunc.ChkDateHuIl(clsDB.DbCon, strRDate) == true)
                //    {
                //        rtnVar = "NO";
                //        GstrAREquipExceptionCheck = "공휴일예약점검";
                //        return rtnVar;
                //    }
                //}


                //의료급여 승인조 점검
                if (strBi == "21" || strBi == "22")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT SUM(AMT)AMT, PART";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "CARD_APPROV_BI ";
                    SQL = SQL + ComNum.VBLF + " WHERE BDATE = TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND PANO  = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND GbBun='1' ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY PART ";
                    SQL = SQL + ComNum.VBLF + "HAVING SUM(AMT) <> 0 ";
                    SQL = SQL + ComNum.VBLF + " UNION ALL ";
                    SQL = SQL + ComNum.VBLF + "       SELECT SUM(AMT1)AMT, PART ";
                    SQL = SQL + ComNum.VBLF + "         FROM " + ComNum.DB_PMPA + "CARD_APPROV_BI ";
                    SQL = SQL + ComNum.VBLF + "        WHERE BDATE = TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "          AND PANO  = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "          AND DEPTCODE = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "          AND GbBun='2' ";
                    SQL = SQL + ComNum.VBLF + "        GROUP BY PART ";
                    SQL = SQL + ComNum.VBLF + "       HAVING SUM(AMT1) <> 0 ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["PART"].ToString().Trim() != GnJobSabun.ToString().Trim())
                        {
                            rtnVar = "NO";
                            GstrAREquipExceptionCheck = "의료급여-금액발생작업조점검";
                            dt.Dispose();
                            dt = null;
                            return rtnVar;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }


                //접수비 점검
                //의료급여 1,2 차상위 E,F 치매검사
                if (strBi == "21" || (strBi == "22" && strJin != "2" && strMCode != "B099") || ((strBi == "11" || strBi == "12" || strBi == "13") && (strMCode == "E000" || strMCode == "F000" || strDementia == "Y")))
                {
                    if (((strBi == "11" || strBi == "12" || strBi == "13") && (strMCode == "E000" || strMCode == "F000" || strDementia == "Y")))
                    {
                        //차상위 E, F
                    }
                    else
                    {
                        //의료급여
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT SUM(NAL) CNT";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDeptCode + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND Bi ='" + strBi + "'";
                        SQL = SQL + ComNum.VBLF + "   AND SUNEXT IN (SELECT SUNEXT FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV ";
                        SQL = SQL + ComNum.VBLF + "                   WHERE GUBUN = '1' ";
                        SQL = SQL + ComNum.VBLF + "                     AND SDATE <=TO_DATE('" + strActDate + "','YYYY-MM-DD')) ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return rtnVar;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            if (VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) == 0)
                            {
                                rtnVar = "NO";
                                GstrAREquipExceptionCheck = "후불진찰료점검";
                                dt.Dispose();
                                dt = null;
                                return rtnVar;
                            }
                        }

                        dt.Dispose();
                        dt = null;
                    }
                }



                //B형 간염 바이러스제 점검
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.GBSELF ='0' ";  //급여만 해당됨
                SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                SQL = SQL + ComNum.VBLF + "   AND TRIM(a.SUCODE) IN ( SELECT TRIM(CODE) FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "                           WHERE GUBUN ='ETC_B형간염약제체크'  AND ( DELDATE ='' OR DELDATE IS NULL ) ) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "B형간염바이러스제체크";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                //고가약관리 점검(뇌하수체 호르몬제, 알부민주사, 혈우병약제)
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.GBSELF ='0' ";  //급여만 해당됨
                SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                SQL = SQL + ComNum.VBLF + "   AND TRIM(a.SUCODE) IN ( SELECT TRIM(CODE) FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "                           WHERE GUBUN ='ETC_고가약제관리'  AND ( DELDATE ='' OR DELDATE IS NULL ) ) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "고가약제관리체크";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                //항암제
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.Ptno=b.Pano ";
                SQL = SQL + ComNum.VBLF + "   AND a.Ptno= '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.GBSELF ='0' ";   //급여만 해당됨
                SQL = SQL + ComNum.VBLF + "   AND a.GBSunap ='0' ";  //미수납만
                SQL = SQL + ComNum.VBLF + "   AND TRIM(a.SUCODE) IN ( SELECT TRIM(CODE) FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "                           WHERE GUBUN ='ETC_특수항암제체크'  AND ( DELDATE ='' OR DELDATE IS NULL ) ) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "특수항암제체크";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                //추가사항

                //내시경 예약오더 및 당일대체 체크
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND RES ='1' ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='MG' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "당일내시경예약오더점검";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                //내시경예약선수금 있다면
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GbEnd ='N' ";
                SQL = SQL + ComNum.VBLF + "   AND TransDate IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN ='01' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "내시경선수금대상자";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;



                //당일 대체 했다면
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRUNC(TRANSDATE) =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN ='01' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "당일내시경대체자점검";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                //치과 완전틀니
                if (strDeptCode == "DT" && strJinDtl == "02")
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "완전틀니대상점검";
                    return rtnVar;
                }



                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(SUCODE) IN ( SELECT trim(SUNEXT) FROM ADMIN.BAS_SUN WHERE DTLBUN ='4004')  ";  //틀니수가

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "치과틀니수가점검";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(SUCODE) IN ( 'SLIDE0','SLIDE1','SLIDE2','SLIDE3')  ";  

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "병리 슬라이드 대여수가점검";  //''의무기록 사본 및 제증명 발급신청서"를 작성
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(SUCODE) IN ( '@V810')  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "@V810 수가점검";  //''의무기록 사본 및 제증명 발급신청서"를 작성
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;



                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(SUCODE) IN ( SELECT trim(SUNEXT) FROM " + ComNum.DB_PMPA + "BAS_SUN WHERE DTLBUN ='4003')  ";  //임플란트수가

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "치과임플란트수가점검";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                //장루, 요루
                if (strJinDtl == "01")
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "장루,요루대상점검";
                    return rtnVar;
                }



                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(SUCODE) IN ( SELECT TRIM(SUNEXT) FROM " + ComNum.DB_PMPA + "BAS_SUN WHERE DTLBUN ='1001')  ";  //장루,요루

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "장루,요루대상점검";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;



                //치매
                if (strDementia == "Y")
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "치매대상점검";
                    return rtnVar;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(SUCODE) IN ( SELECT TRIM(SUNEXT) FROM " + ComNum.DB_PMPA + "BAS_SUN WHERE GBDEMENTIA ='Y' )";  //치매
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "치매대상점검";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;



                //안저
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GBOT ='Y'  ";  //안과검진

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "NO";
                    GstrAREquipExceptionCheck = "안과검진점검";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;



                //개인 미수금잔액 있는분 (미수구분 11,13,14는 제외함)
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT JAmt";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "Misu_GAINMST ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano  = '" + strPano + "'";
                SQL = SQL + ComNum.VBLF + "   AND JAmt > 0 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT MISUDTL";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "Misu_GAINSLIP ";
                    SQL = SQL + ComNum.VBLF + " WHERE Pano  = '" + strPano + "'";
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN1 = '1' ";
                    SQL = SQL + ComNum.VBLF + "   AND SUBSTR(MISUDTL,4,2) NOT IN ('11','13','14','15') ";
                    SQL = SQL + ComNum.VBLF + "   AND FLAG <> '*' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        rtnVar = "NO";
                        GstrAREquipExceptionCheck = "개인미수금점검";
                        dt1.Dispose();
                        dt1 = null;
                        dt.Dispose();
                        dt = null;
                        return rtnVar;
                    }

                    dt1.Dispose();
                    dt1 = null;
                }

                dt.Dispose();
                dt = null;



                //만성질환대상자 무인수납체크
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT CHOJAE,Amt1";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDeptCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    GnDrugNal = 0;  //이전처방중 최대 약처방 일수

                    strChojae = dt.Rows[0]["ChoJae"].ToString().Trim();

                    if (strChojae == "1" && string.Compare(strActDate, "2014-11-01") >= 0)
                    {
                        StrTemp = cf.CHK_ChronicIll(clsDB.DbCon, strPano, strActDate, strDeptCode, ref GnDrugNal);

                        if (StrTemp != "")
                        {
                            strTemp2 = VB.Pstr(StrTemp, "@@", 1);   //이전 처방일자

                            if (Convert.ToDateTime(strTemp2).AddDays(GnDrugNal+90) > Convert.ToDateTime(strActDate))
                            {
                                rtnVar = "NO";
                                GstrAREquipExceptionCheck = "만성질환자 재진료점검";
                                dt.Dispose();
                                dt = null;
                                return rtnVar;
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;



                //예약시간 변수세팅
                if (rtnVar == "")
                {
                    GstrAR_RDate = (strRDate + " " + strRTime).Trim();
                }

                if (rtnVar != "NO" && GstrAREquipExceptionCheck == "")
                {
                    GstrAREquipCheckSentence = "무인수납 가능";
                }

                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }

        /// <summary>
        /// VB - 무인수납장비장애체크
        /// 2018-01-03 박창욱
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        public static string AREquip_Obstacle_Check(PsmhDb pDbCon)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            GstrAREquipObstacle = "";

            try
            {
                //무인수납장비 수납 전체 사용여부
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE Gubun ='무인수납장비장애구분' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE ='001' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(NAME) ='Y'  ";
                SQL = SQL + ComNum.VBLF + "   AND ( DELDATE IS NULL OR DELDATE ='') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count == 0)
                {
                    rtnVar = "OK";
                    GstrAREquipExceptionCheck = "OK";
                }

                dt.Dispose();
                dt = null;

                //무인수납장비 수납 의료급여 자격조회 에러발생시 사용여부
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE Gubun ='무인수납장비장애구분' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE ='002' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(NAME) ='Y'  ";
                SQL = SQL + ComNum.VBLF + "   AND ( DELDATE IS NULL OR DELDATE ='') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count == 0)
                {
                    rtnVar = "OK2";
                    GstrAREquipExceptionCheck = "OK2";
                }

                dt.Dispose();
                dt = null;

                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }

        /// <summary>
        /// VB - 물리치료무인수납장애체크
        /// 2018-01-03 박창욱
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        public static string PhysicalTherapy_AR_Check(PsmhDb pDbCon)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            GstrPhysicalTherapyReceive = "";

            try
            {
                //무인수납장비 수납 전체 사용여부
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE Gubun ='물리치료무인수납가능여부' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE ='001' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(NAME) ='Y'  ";
                SQL = SQL + ComNum.VBLF + "   AND ( DELDATE IS NULL OR DELDATE ='') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count == 0)
                {
                    rtnVar = "OK";
                    GstrPhysicalTherapyReceive = "OK";
                }

                dt.Dispose();
                dt = null;

                //무인수납장비 수납 의료급여 자격조회 에러발생시 사용여부
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE Gubun ='물리치료무인수납가능여부' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE ='002' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(NAME) ='Y'  ";
                SQL = SQL + ComNum.VBLF + "   AND ( DELDATE IS NULL OR DELDATE ='') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count == 0)
                {
                    rtnVar = "OK2";
                    GstrPhysicalTherapyReceive = "OK2";
                }

                dt.Dispose();
                dt = null;

                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }

        /// <summary>
        /// VB - M3_HIC_13_차상위2_무인CHK
        /// 차상위2종 만성질환(장애인) 및 만18세 대상자 수납, 부분취소
        /// 2018-01-04 박창욱
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPano"></param>
        /// <param name="argBi"></param>
        /// <param name="argDept"></param>
        /// <param name="argBDate"></param>
        /// <param name="argMCode"></param>
        /// <param name="argVCode"></param>
        /// <returns></returns>
        public static string M3_HIC_13_Cha2_AutoCHK(PsmhDb pDbCon, string argPano, string argBi, string argDept, string argBDate, string argMCode, string argVCode)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nNal = 0;
            int nNal2 = 0;
            double nJangAmt4 = 0;
            string strChk1 = "";
            string strChk2 = "";
            string strBunup = "";

            int nBuCount = 0;   //특정기호 V001~
            int nBuCount2 = 0;   //중증환자 V193

            string strDrugBunup2 = "";
            string strBunup2 = "";
            string strJupsuCode2 = "";
            string strGemsa2 = "";
            string strSucode = "";
            string strBun = "";
            string strSelf = "";
            string strSunap = "";

            string rtnVar = "";

            //GM3_HIC(1):진료형태 GM3_HIC(2):입(내)원일수 GM3_HIC(3):투약일수 GM3_HIC(4):본인일부부담금
            //GM3_HIC(5):건강생활유지비청구액 GM3_HIC(6):기관부담금 GM3_HIC(7):주상병분류기호
            //GM3_HIC(8):진료일자 GM3_HIC(9):처방전교부번호 GM3_HIC(10):본인부담여부 GM3_HIC(11):타기관의료여부(Y/N)

            //strBunup2 => 원외처방전 발생
            //strDrugBunup2 => 원내조제(병원약국에서 조제함)
            //strJupsuCode2 => 접수비 코드(진찰료 코드)
            //strGemsa => 검사 발생

            if (argBi != "13" && argBi != "12" && argBi != "11")
            {
                return rtnVar;
            }

            if (argMCode != "E000" && argMCode != "F000")
            {
                return rtnVar;
            }

            if (string.Compare(argBDate, "2009-10-01") >= 0)
            {
                //차상위E,F는 진찰료 수납시 입력. 추가수납시 진찰코드 안넣기 때문에 기본 OK로 세팅.
                strJupsuCode2 = "OK";
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SUCODE,BUN,GBSUNAP,GbSelf ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO ='" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + argBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + argDept + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBun = dt.Rows[i]["Bun"].ToString().Trim();
                    strSucode = dt.Rows[i]["SuCode"].ToString().Trim();
                    strSelf = dt.Rows[i]["GbSelf"].ToString().Trim();
                    strSunap = dt.Rows[i]["GbSunap"].ToString().Trim();

                    if (strSucode != "")
                    {
                        switch (strSucode)
                        {
                            case "@V001":
                            case "@V003":
                            case "@V005":
                            case "@V117":
                            case "@V027":
                            case "@V009":
                                nBuCount += 1;
                                break;
                            case "@V012":
                            case "@V013":
                            case "@V014":
                            case "@V015":
                            case "@V193":
                            case "@V194":
                                nBuCount += 1;
                                break;
                            case "@V191":
                            case "@V192":
                                nBuCount += 1;
                                break;
                        }

                        if (string.Compare(strBun, "80") < 0 && (VB.Left(strSucode, 2) != "$$" && VB.Left(strSucode, 2) != "##" && VB.Left(strSucode, 2) != "@V") && (strBun != "11" && strBun != "12" && strBun != "20"))
                        {
                            strGemsa2 = "OK";
                        }

                        //미수납만 점검
                        if (strSunap == "0")
                        {
                            if (argVCode == "EV00" || argVCode == "V206" || argVCode == "V231" || argVCode == "V246")
                            {
                                if (READ_RareIncurableVCode_2(clsDB.DbCon, strSucode) == "OK")
                                {
                                    nBuCount += 1;
                                }
                            }

                            if (strSucode == "@V193" || strSucode == "@V194")
                            {
                                nBuCount2 = 1;
                            }

                            //약 발생
                            if (strBun == "11" || strBun == "12" || strBun == "20")
                            {
                                strBunup2 = "OK";
                            }
                            else if ((strSucode == "E7660" || strSucode == "E7660S" || strSucode == "E7630" || strSucode == "E7630S") &&
                                      string.Compare(argBDate, "2009-10-01") > 0)
                            {
                                //원내 조제
                                strDrugBunup2 = "OK";
                            }
                            else if (strSucode == "O9991" || strSucode == "O9992" )
                            {
                                //원내조제(HD환자 O9991이 수가코드 발생시 원내조제에 들어감)
                                strDrugBunup2 = "OK";
                            }
                            else if (strBun == "02" || strBun == "01" || strBun == "75")    //진찰료, 증명료
                            {
                                strJupsuCode2 = "OK";
                            }
                            else if (strSucode == "AY100")  //가정간호 기본방문료가 진찰료임.
                            {
                                strJupsuCode2 = "OK";
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SUM(QTY*NAL) CNT FROM ADMIN.OPD_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE  = '" + argDept + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + argBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND BUN IN ('11','12') ";
                SQL = SQL + ComNum.VBLF + "   AND GBBUNUP = '0'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    if (VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) > 0)
                    {
                        strBunup = "Y";
                    }
                    else
                    {
                        strBunup = "N";
                    }
                }
                else
                {
                    strBunup = "N";
                }

                dt.Dispose();
                dt = null;

                if (nBuCount == 0)
                {
                    //특정기호나 중증코드가 없으면 해당사항없음 (접수구분 JIN ( I,J ))
                    return rtnVar;
                }

                if (strBunup2 == "OK")
                {
                    rtnVar = "OK";
                }
                else if (strJupsuCode2 == "OK" && strDrugBunup2 == "" && strGemsa2 == "")
                {
                    rtnVar = "OK";
                }
                else if (strJupsuCode2 == "OK" && strDrugBunup2 == "OK")
                {
                    rtnVar = "OK";
                }
                else if (strJupsuCode2 == "OK" && strGemsa2 == "OK")
                {
                    rtnVar = "OK";
                }
                else
                {
                    rtnVar = "OK";
                }
                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }

        /// <summary>
        /// VB - READ_희귀난치VCode_2
        /// 2018-01-04 박창욱
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public static string READ_RareIncurableVCode_2(PsmhDb pDbCon, string argCode)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            rtnVar = "NO";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SuNext";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                SQL = SQL + ComNum.VBLF + " WHERE GBRARE = 'Y'  ";
                SQL = SQL + ComNum.VBLF + "  AND  SuNext ='" + argCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "OK";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;
                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }

        /// <summary>
        /// VB - 무인수납_전화예약_CHK
        /// 2018-01-04 박창욱
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPano"></param>
        /// <param name="argActDate"></param>
        /// <param name="argDeptCode"></param>
        /// <returns></returns>
        public static string CHK_AR_TelephoneBooking(PsmhDb pDbCon, string argPano, string argActDate, string argDeptCode)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strTEMP1 = "";
            string strTemp2 = "";

            string rtnVar = "";

            rtnVar = "OK";

            try
            {
                if (VB.Left(argPano, 6) != "810000")
                {
                    //병원신환만 제외함
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT ROWID ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + "  WHERE Pano= '" + argPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE <TO_DATE('" + argActDate + "','YYYY-MM-DD') "; //오늘이전

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        rtnVar = "NO";
                        dt.Dispose();
                        dt = null;
                        return rtnVar;
                    }
                    dt.Dispose();
                    dt = null;
                }

                //당일자격 점검 확인
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT M2_GKiho,M2_DISREG3,M2_SANGSIL,M2_DISREG4 ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_NHIC ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano= '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND actDATE =TO_DATE('" + argActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + argDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND Job_STS ='2' "; //정상처리된 자료
                SQL = SQL + ComNum.VBLF + "   AND ReqType ='M1' ";
                SQL = SQL + ComNum.VBLF + "   AND M2_Jagek NOT IN ('7','8') "; //의료급여자격 제외
                SQL = SQL + ComNum.VBLF + " ORDER BY SendTime DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    strTEMP1 = dt.Rows[0]["M2_GKiho"].ToString().Trim();    //자격조회된 증번호
                    strTemp2 = dt.Rows[0]["M2_DISREG3"].ToString().Trim();  //차상위 자격여부
                }
                else
                {
                    rtnVar = "NO";
                    return rtnVar;
                }

                if (VB.Mid(strTemp2, 21, 1).Trim() == "1" && VB.Mid(strTemp2, 1, 1) == "C")
                {   // '차상위1종(희귀질환자)
                    rtnVar = "NO";
                    return rtnVar;
                }
                else if (dt.Rows[0]["M2_SANGSIL"].ToString().Trim() != "")
                {   //'무자격자"
                    rtnVar = "NO";
                    return rtnVar;
                }
                else if (dt.Rows[0]["M2_DISREG4"].ToString().Trim() != "")
                {   //'산정특례"
                    rtnVar = "NO";
                    return rtnVar;
                }
                else if (VB.Mid(strTemp2, 21, 1).Trim() == "2" && VB.Mid(strTemp2, 1, 1) == "E")
                {   //'차상위2종(만성질환자 및 18세미만)"
                    rtnVar = "NO";
                    return rtnVar;
                }
                else if (VB.Mid(strTemp2, 21, 1).Trim() == "2" && VB.Mid(strTemp2, 1, 1) == "F")
                {   //'차상위2종(장애인 만성질환자 및 18세미만)"
                    rtnVar = "NO";
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                if (VB.Left(argPano, 6) != "810000")
                {
                    //전화예약당시 자격점검
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT b.GKiho ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_TELRESV a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.Pano=b.Pano ";
                    SQL = SQL + ComNum.VBLF + "   AND a.Pano= '" + argPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.RDATE =TO_DATE('" + argActDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE = '" + argDeptCode + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (strTEMP1 != VB.Replace(dt.Rows[0]["GKiho"].ToString().Trim(), "-", "") && VB.Left(argPano, 6) != "810000")
                        {
                            rtnVar = "NO";
                        }
                    }
                    else
                    {
                        rtnVar = "NO";
                    }
                    dt.Dispose();
                    dt = null;
                }

                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }


        public static string READ_OPD_NHIC_Qualification_2_BACKUP(PsmhDb pDbCon, string argDate, string argPano, string argBi, string argDept, string argType, string argGubun)
        {
            //백업용 사용안함(2021-07-27)
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;

            //OPD_MASTER 정보
            string strM_Jin = "";
            string strM_Bi = "";
            string strM_MCode = "";
            string strM_VCode = "";

            string strRegs1 = "";
            string strRegs2 = "";
            string strRegs3 = "";
            string strRegs4 = "";
            string strRegs1_2 = "";
            string strRegs2_2 = "";
            string strRegs3_2 = "";
            string strRegs4_2 = "";
            string str_NHIC_MCODE = "";
            string strNHIC = "";
            string strSangSil = "";

            string strOK = "";
            string strOK2 = "";

            string rtnVar = "";

            //의료급여자격은 점검 안 함
            if (argBi == "21" || argBi == "22")
            {
                return rtnVar;
            }

            strOK = "OK";

            try
            {
                //예약인지 체크함
                if (argGubun == "1")
                {
                    strOK = "";
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT Pano,Bi,MCode,VCode,Jin ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + argPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE =TO_DATE('" + argDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + argDept + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND RESERVED ='1' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strOK = "OK";
                        strM_Jin = dt.Rows[0]["Jin"].ToString().Trim();
                        strM_Bi = dt.Rows[0]["Bi"].ToString().Trim();
                        strM_MCode = dt.Rows[0]["MCode"].ToString().Trim();
                        strM_VCode = dt.Rows[0]["VCode"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strOK != "OK")
                {
                    return rtnVar;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT PANO,M2_JAGEK,M2_CDATE,M2_SUJIN_NAME,                ";
                SQL = SQL + ComNum.VBLF + "       M2_SEDAE_NAME,M2_KIHO,M2_GKIHO,M2_SANGSIL,           ";
                SQL = SQL + ComNum.VBLF + "       M2_BONIN,M2_GJAN_AMT,M2_CHULGUK,M2_JANG_DATE,        ";
                SQL = SQL + ComNum.VBLF + "       M2_SHOSPITAL1,M2_SHOSPITAL2,M2_SHOSPITAL3,           ";
                SQL = SQL + ComNum.VBLF + "       M2_SHOSPITAL4,M2_SHOSPITAL_NAME1,M2_SHOSPITAL_NAME2, ";
                SQL = SQL + ComNum.VBLF + "       M2_SHOSPITAL_NAME3,M2_SHOSPITAL_NAME4,JOB_STS,       ";
                SQL = SQL + ComNum.VBLF + "       M2_DISREG1,M2_DISREG2,M2_DISREG3,M2_DISREG4,         ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, M2_REMAMT ,M2_DISREG2_B        ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_NHIC                       ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + argPano + "'                              ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE =TO_DATE('" + argDate + "','YYYY-MM-DD')     ";
                SQL = SQL + ComNum.VBLF + "   AND JOB_STS ='2'                                         "; //자격조회 점검완료된 것
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + argDept + "'                          ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SENDTIME DESC                                     ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    strSangSil = dt.Rows[0]["M2_SANGSIL"].ToString().Trim();

                    //희귀난치 대상자 H0000
                    strRegs1 = dt.Rows[0]["M2_DISREG1"].ToString().Trim();
                    if (strRegs1 != "")
                    {
                        str_NHIC_MCODE += VB.Left(strRegs1, 1) + "^^";
                        strRegs1_2 = VB.Left(strRegs1, 4).Trim() + "@@";
                        strRegs1_2 += VB.Mid(strRegs1, 5, 8).Trim() + "@@";
                        strRegs1_2 += ComFunc.FormatStrToDate(VB.Mid(strRegs1, 5, 8), "D").Trim() + "@@";
                        strRegs1_2 += VB.Mid(strRegs1, 13, 8).Trim();

                        for (i = 21; i < 46; i += 5)
                        {
                            strRegs1_2 += VB.Mid(strRegs1, i, 5).Trim() + "@@";
                            k += 1;
                        }
                    }

                    //산정특례 (희귀)등록 + 중증희귀) 대상자 V000
                    strRegs2 = dt.Rows[0]["M2_DISREG2"].ToString().Trim() + dt.Rows[0]["M2_DISREG2_B"].ToString().Trim();
                    if (strRegs2 != "")
                    {
                        str_NHIC_MCODE += VB.Left(strRegs2, 1) + "^^";
                        strRegs2_2 = VB.Left(strRegs2, 4).Trim() + "@@";
                        strRegs2_2 += VB.Mid(strRegs2, 20, 8).Trim() + "@@";
                        strRegs2_2 += ComFunc.FormatStrToDate(VB.Mid(strRegs2, 20, 8), "D").Trim() + "@@";
                        strRegs2_2 += VB.Mid(strRegs2, 28, 8).Trim() + "@@";
                        strRegs2_2 += VB.Mid(strRegs2, 5, 15).Trim() + "@@";
                    }

                    //차상위대상자 C000,E000,F000
                    strRegs3 = dt.Rows[0]["M2_DISREG3"].ToString().Trim();
                    if (strRegs3 != "")
                    {
                        str_NHIC_MCODE += VB.Left(strRegs3, 1) + "^^";
                        strRegs3_2 = VB.Left(strRegs3, 4).Trim() + "@@";
                        strRegs3_2 += VB.Mid(strRegs3, 5, 8).Trim() + "@@";
                        strRegs3_2 += ComFunc.FormatStrToDate(VB.Mid(strRegs3, 5, 8), "D").Trim() + "@@";
                        strRegs3_2 += VB.Mid(strRegs3, 13, 8).Trim() + "@@";

                        strRegs3_2 += VB.Mid(strRegs3, 21, 1).Trim() + "@@";    //차상위구분 1,2
                        strRegs3_2 += VB.Mid(strRegs3, 1, 1).Trim() + "@@";     //차상위기호
                    }

                    //중증암환자
                    strRegs4 = dt.Rows[0]["M2_DISREG4"].ToString().Trim();
                    if (strRegs4 != "")
                    {
                        strRegs4_2 = VB.Left(strRegs4, 4).Trim() + "@@";
                        strRegs4_2 += VB.Mid(strRegs4, 20, 8).Trim() + "@@";
                        strRegs4_2 += VB.Mid(strRegs4, 28, 8).Trim() + "@@";
                        strRegs4_2 += VB.Mid(strRegs4, 36, 5).Trim() + "@@";
                        strRegs4_2 += VB.Mid(strRegs4, 5, 15).Trim() + "@@";
                    }

                    if (str_NHIC_MCODE != "")
                    {
                        for (i = 1; i <= VB.L(str_NHIC_MCODE, "^^") - 1; i++)
                        {
                            strNHIC += VB.Pstr(str_NHIC_MCODE, "^^", i).Trim() + "000 ";
                        }
                    }

                    //차상위, 희귀 자격체크 루틴 시작 지점
                    if (str_NHIC_MCODE == "")
                    {
                        //자격조회 시 값이 없는데 예약 당시에는 산특/지병 등등이 있을 경우(무조건 무인/모바일 불가! 금액 달라짐)
                        if (VB.Left(strM_MCode, 1) != str_NHIC_MCODE)
                        {
                            rtnVar += "예약환자 자격점검 [ 예약당시 자격과 현재접수 자격 비교 ]" + ComNum.VBLF + ComNum.VBLF;
                            rtnVar += "예약당시[" + strM_MCode + "] 자격과 현재자격[" + strNHIC + "]이 다릅니다..자격확인후 처리하십시오.." + ComNum.VBLF;
                        }
                    }
                    else
                    {
                        //자격조회에 값이 있을 경우(자격이 한가지)
                        if (VB.Val((VB.L(str_NHIC_MCODE, "^^") - 1).ToString()) == 1)
                        {
                            //자격 1개
                            if (VB.Left(strM_MCode, 1) != VB.TR(str_NHIC_MCODE, "^^", "").Trim())
                            {
                                rtnVar += "예약환자 자격점검 [ 예약당시 자격과 현재접수 자격 비교 ]" + ComNum.VBLF + ComNum.VBLF;
                                rtnVar += "예약당시[" + strM_MCode + "] 자격과 현재자격[" + strNHIC + "]이 다릅니다..자격확인후 처리하십시오.." + ComNum.VBLF;
                            }
                        }
                        else
                        {
                            //자격 2개 이상
                            if (strM_MCode == "")
                            {
                                //OPD_MSTER 자격이 없다면
                                rtnVar += "예약환자 자격점검 [ 예약당시 자격과 현재접수 자격 비교 ]" + ComNum.VBLF + ComNum.VBLF;
                                rtnVar += "예약당시[" + strM_MCode + "] 자격과 현재자격[" + strNHIC + "]이 다릅니다..자격확인후 처리하십시오.." + ComNum.VBLF;
                            }
                            else
                            {
                                strOK2 = "";
                                for (i = 1; i <= VB.L(str_NHIC_MCODE, "^^") - 1; i++)
                                {
                                    if (VB.Left(strM_MCode, 1) == VB.Pstr(str_NHIC_MCODE, "^^", i).Trim())
                                    {
                                        strOK2 = "OK";
                                    }
                                }

                                if (strOK2 == "")
                                {
                                    //2건이상 자격인데 OPD_MSTER 자격이 없는경우
                                    rtnVar += "예약환자 자격점검 [ 예약당시 자격과 현재접수 자격 비교 ]" + ComNum.VBLF + ComNum.VBLF;
                                    rtnVar += "예약당시[" + strM_MCode + "] 자격과 현재자격[" + strNHIC + "]이 다릅니다..자격확인후 처리하십시오.." + ComNum.VBLF;
                                }
                                else
                                {
                                    //2건이상 자격인데 OPD_MASTER 자격이 다를경우
                                    rtnVar += "예약환자 자격점검 [ 예약당시 자격과 현재접수 자격 비교 ]" + ComNum.VBLF + ComNum.VBLF;
                                    rtnVar += "예약당시[" + strM_MCode + "] 자격과 현재자격[" + strNHIC + "]이 다릅니다..자격확인후 처리하십시오.." + ComNum.VBLF;
                                }
                            }
                        }
                    }

                    //중증코드 체크 F003 의약분업코드는 제외
                    if (strM_VCode != "F003" && strM_VCode != VB.Left(strRegs4, 4))
                    {
                        rtnVar += "예약당시 중증코드[" + strM_VCode + "] 와 자격조회후 중증코드[" + VB.Left(strRegs4, 4) + "] 가 불일치합니다..";
                    }

                    if (strSangSil != "" && string.Compare(strSangSil, VB.TR(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), "-", "").Trim()) > 0)
                    {
                        if (rtnVar != "")
                        {
                            rtnVar += ComNum.VBLF + ComNum.VBLF;
                            rtnVar += "자격상실자입니다..반드시 접수확인하세요 !!";
                        }
                        else
                        {
                            rtnVar = "자격상실자입니다..반드시 접수확인하세요 !!";
                        }
                    }
                }
                else
                {
                    rtnVar = "당일 예약자인데 당일 자격조회 자료가 없습니다..자격조회후 다시 수납하세요";
                }

                dt.Dispose();
                dt = null;
                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }

        /// <summary>
        /// VB - READ_OPD_NHIC_자격점검_2
        /// 2018-01-05 박창욱
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argDate">날짜</param>
        /// <param name="argPano">등록번호</param>
        /// <param name="argBi">보험</param>
        /// <param name="argDept">과</param>
        /// <param name="argType">자격점검</param>
        /// <param name="argGubun">예약구분1</param>
        /// <returns></returns>

        public static string READ_OPD_NHIC_Qualification_2(PsmhDb pDbCon, string argDate, string argPano, string argBi, string argDept, string argType, string argGubun)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;

            //OPD_MASTER 정보
            string strM_Jin = "";
            string strM_Bi = "";
            string strM_MCode = "";
            string strM_VCode = "";

            string strRegs1 = "";
            string strRegs2 = "";
            string strRegs3 = "";
            string strRegs4 = "";
            string strRegs1_2 = "";
            string strRegs2_2 = "";
            string strRegs3_2 = "";
            string strRegs4_2 = "";
            string str_NHIC_MCODE = "";
            string strNHIC = "";
            string strSangSil = "";

            string strOK = "";
            string strOK2 = "";

            string rtnVar = "";

            //의료급여자격은 점검 안 함
            if (argBi == "21" || argBi == "22")
            {
                return rtnVar;
            }

            strOK = "OK";

            try
            {
                //예약인지 체크함
                if (argGubun == "1")
                {
                    strOK = "";
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT Pano,Bi,MCode,VCode,Jin ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + argPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE =TO_DATE('" + argDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + argDept + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND RESERVED ='1' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVar;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strOK = "OK";
                        strM_Jin = dt.Rows[0]["Jin"].ToString().Trim();
                        strM_Bi = dt.Rows[0]["Bi"].ToString().Trim();
                        strM_MCode = dt.Rows[0]["MCode"].ToString().Trim();
                        strM_VCode = dt.Rows[0]["VCode"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strOK != "OK")
                {
                    return rtnVar;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT PANO,M2_JAGEK,M2_CDATE,M2_SUJIN_NAME,                ";
                SQL = SQL + ComNum.VBLF + "       M2_SEDAE_NAME,M2_KIHO,M2_GKIHO,M2_SANGSIL,           ";
                SQL = SQL + ComNum.VBLF + "       M2_BONIN,M2_GJAN_AMT,M2_CHULGUK,M2_JANG_DATE,        ";
                SQL = SQL + ComNum.VBLF + "       M2_SHOSPITAL1,M2_SHOSPITAL2,M2_SHOSPITAL3,           ";
                SQL = SQL + ComNum.VBLF + "       M2_SHOSPITAL4,M2_SHOSPITAL_NAME1,M2_SHOSPITAL_NAME2, ";
                SQL = SQL + ComNum.VBLF + "       M2_SHOSPITAL_NAME3,M2_SHOSPITAL_NAME4,JOB_STS,       ";
                SQL = SQL + ComNum.VBLF + "       M2_DISREG1,M2_DISREG2,M2_DISREG3,M2_DISREG4,         ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, M2_REMAMT ,M2_DISREG2_B        ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_NHIC                       ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + argPano + "'                              ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE =TO_DATE('" + argDate + "','YYYY-MM-DD')     ";
                SQL = SQL + ComNum.VBLF + "   AND JOB_STS ='2'                                         "; //자격조회 점검완료된 것
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + argDept + "'                          ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SENDTIME DESC                                     ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    strSangSil = dt.Rows[0]["M2_SANGSIL"].ToString().Trim();

                    //희귀난치 대상자 H0000
                    strRegs1 = dt.Rows[0]["M2_DISREG1"].ToString().Trim();
                    if (strRegs1 != "")
                    {
                        str_NHIC_MCODE += VB.Left(strRegs1, 1) + "^^";
                        strRegs1_2 = VB.Left(strRegs1, 4).Trim() + "@@";
                        strRegs1_2 += VB.Mid(strRegs1, 5, 8).Trim() + "@@";
                        strRegs1_2 += ComFunc.FormatStrToDate(VB.Mid(strRegs1, 5, 8), "D").Trim() + "@@";
                        strRegs1_2 += VB.Mid(strRegs1, 13, 8).Trim();

                        for (i = 21; i < 46; i += 5)
                        {
                            strRegs1_2 += VB.Mid(strRegs1, i, 5).Trim() + "@@";
                            k += 1;
                        }
                    }

                    //산정특례 (희귀)등록 + 중증희귀) 대상자 V000
                    strRegs2 = dt.Rows[0]["M2_DISREG2"].ToString().Trim() + dt.Rows[0]["M2_DISREG2_B"].ToString().Trim();
                    if (strRegs2 != "")
                    {
                        str_NHIC_MCODE += VB.Left(strRegs2, 1) + "^^";
                        strRegs2_2 = VB.Left(strRegs2, 4).Trim() + "@@";
                        strRegs2_2 += VB.Mid(strRegs2, 20, 8).Trim() + "@@";
                        strRegs2_2 += ComFunc.FormatStrToDate(VB.Mid(strRegs2, 20, 8), "D").Trim() + "@@";
                        strRegs2_2 += VB.Mid(strRegs2, 28, 8).Trim() + "@@";
                        strRegs2_2 += VB.Mid(strRegs2, 5, 15).Trim() + "@@";
                    }

                    //차상위대상자 C000,E000,F000
                    strRegs3 = dt.Rows[0]["M2_DISREG3"].ToString().Trim();
                    if (strRegs3 != "")
                    {
                        str_NHIC_MCODE += VB.Left(strRegs3, 1) + "^^";
                        strRegs3_2 = VB.Left(strRegs3, 4).Trim() + "@@";
                        strRegs3_2 += VB.Mid(strRegs3, 5, 8).Trim() + "@@";
                        strRegs3_2 += ComFunc.FormatStrToDate(VB.Mid(strRegs3, 5, 8), "D").Trim() + "@@";
                        strRegs3_2 += VB.Mid(strRegs3, 13, 8).Trim() + "@@";

                        strRegs3_2 += VB.Mid(strRegs3, 21, 1).Trim() + "@@";    //차상위구분 1,2
                        strRegs3_2 += VB.Mid(strRegs3, 1, 1).Trim() + "@@";     //차상위기호
                    }

                    //중증암환자
                    strRegs4 = dt.Rows[0]["M2_DISREG4"].ToString().Trim();
                    if (strRegs4 != "")
                    {
                        strRegs4_2 = VB.Left(strRegs4, 4).Trim() + "@@";
                        strRegs4_2 += VB.Mid(strRegs4, 20, 8).Trim() + "@@";
                        strRegs4_2 += VB.Mid(strRegs4, 28, 8).Trim() + "@@";
                        strRegs4_2 += VB.Mid(strRegs4, 36, 5).Trim() + "@@";
                        strRegs4_2 += VB.Mid(strRegs4, 5, 15).Trim() + "@@";
                    }

                    if (str_NHIC_MCODE != "")
                    {
                        for (i = 1; i <= VB.L(str_NHIC_MCODE, "^^") - 1; i++)
                        {
                            strNHIC += VB.Pstr(str_NHIC_MCODE, "^^", i).Trim() + "000 ";
                        }
                    }

                    //차상위, 희귀 자격체크 루틴 시작 지점
                    if (str_NHIC_MCODE == "")
                    {
                        //자격조회 시 값이 없는데 예약 당시에는 산특/지병 등등이 있을 경우(무조건 무인/모바일 불가! 금액 달라짐)
                        if (VB.Left(strM_MCode, 1) != str_NHIC_MCODE)
                        {
                            rtnVar += "예약환자 자격점검 [ 예약당시 자격과 현재접수 자격 비교 ]" + ComNum.VBLF + ComNum.VBLF;
                            rtnVar += "예약당시[" + strM_MCode + "] 자격과 현재자격[" + strNHIC + "]이 다릅니다..자격확인후 처리하십시오.." + ComNum.VBLF;
                        }
                    }
                    else
                    {
                        //자격조회에 값이 있을 경우(자격이 한가지)
                        if (VB.Val((VB.L(str_NHIC_MCODE, "^^") - 1).ToString()) == 1)
                        {
                            //자격 1개
                            if (VB.Left(strM_MCode, 1) != VB.TR(str_NHIC_MCODE, "^^", "").Trim())
                            {
                                //(2021-07-15) 예약 자격엔 정보 없는데 희귀난치나 산정특례 있을 경우 예외처리 안되도록 보완.
                                if (strM_MCode.Trim() == "" && strRegs3.Trim() == "" && (strRegs1.Trim() != "" || strRegs2.Trim() != ""))
                                {

                                }
                                else
                                {
                                    rtnVar += "예약환자 자격점검 [ 예약당시 자격과 현재접수 자격 비교 ]" + ComNum.VBLF + ComNum.VBLF;
                                    rtnVar += "예약당시[" + strM_MCode + "] 자격과 현재자격[" + strNHIC + "]이 다릅니다..자격확인후 처리하십시오.." + ComNum.VBLF;
                                }
                            }
                        }
                        else
                        {
                            //자격이 두가지 이상이며 예약 시 값이 없는 경우
                            if (strM_MCode == "")
                            {
                                //(2021-07-15) 예약 자격엔 정보 없는데 희귀난치나 산정특례 있을 경우 예외처리 안되도록 보완.
                                if (strRegs3.Trim() == "" && (strRegs1.Trim() != "" || strRegs2.Trim() != ""))
                                {
                                }
                                else
                                {
                                    //OPD_MSTER 자격이 없다면
                                    rtnVar += "예약환자 자격점검 [ 예약당시 자격과 현재접수 자격 비교 ]" + ComNum.VBLF + ComNum.VBLF;
                                    rtnVar += "예약당시[" + strM_MCode + "] 자격과 현재자격[" + strNHIC + "]이 다릅니다..자격확인후 처리하십시오.." + ComNum.VBLF;
                                }
                            }
                            else
                            {
                                strOK2 = "";
                                for (i = 1; i <= VB.L(str_NHIC_MCODE, "^^") - 1; i++)
                                {
                                    if (VB.Left(strM_MCode, 1) == VB.Pstr(str_NHIC_MCODE, "^^", i).Trim())
                                    {
                                        strOK2 = "OK";
                                    }
                                }

                                if (strOK2 == "")
                                {
                                    //2건이상 자격인데 OPD_MSTER 자격이 없는경우
                                    rtnVar += "예약환자 자격점검 [ 예약당시 자격과 현재접수 자격 비교 ]" + ComNum.VBLF + ComNum.VBLF;
                                    rtnVar += "예약당시[" + strM_MCode + "] 자격과 현재자격[" + strNHIC + "]이 다릅니다..자격확인후 처리하십시오.." + ComNum.VBLF;
                                }
                                else
                                {
                                    //2건이상 자격인데 OPD_MASTER 자격이 다를경우
                                    rtnVar += "예약환자 자격점검 [ 예약당시 자격과 현재접수 자격 비교 ]" + ComNum.VBLF + ComNum.VBLF;
                                    rtnVar += "예약당시[" + strM_MCode + "] 자격과 현재자격[" + strNHIC + "]이 다릅니다..자격확인후 처리하십시오.." + ComNum.VBLF;
                                }
                            }                        
                        }
                    }
                    //(2021-07-15) 예약당시 VCODE 값 없는데 자격조회 값 있는 경우 수납 가능하도록 예외처리
                    if (strM_VCode.Trim() == "" && strRegs4.Trim() != "")
                    {
                    }
                    else
                    {
                        //중증코드 체크 F003 의약분업코드는 제외
                        if (strM_VCode != "F003" && strM_VCode != VB.Left(strRegs4, 4))
                        {
                            rtnVar += "예약당시 중증코드[" + strM_VCode + "] 와 자격조회후 중증코드[" + VB.Left(strRegs4, 4) + "] 가 불일치합니다..";
                        }
                    }

                    if (strSangSil != "" && string.Compare(strSangSil, VB.TR(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), "-", "").Trim()) > 0)
                    {
                        if (rtnVar != "")
                        {
                            rtnVar += ComNum.VBLF + ComNum.VBLF;
                            rtnVar += "자격상실자입니다..반드시 접수확인하세요 !!";
                        }
                        else
                        {
                            rtnVar = "자격상실자입니다..반드시 접수확인하세요 !!";
                        }
                    }
                }
                else
                {
                    rtnVar = "당일 예약자인데 당일 자격조회 자료가 없습니다..자격조회후 다시 수납하세요";
                }
                
                dt.Dispose();
                dt = null;
                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }

        /// <summary>
        /// VB - Gam_Pano_Search_2
        /// 2018-01-05 박창욱
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argGam"></param>
        /// <param name="argJumin1"></param>
        /// <param name="argJumin2"></param>
        /// <returns></returns>
        public static string Gam_Pano_Search_2(PsmhDb pDbCon, string argGam, string argJumin1, string argJumin2)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strSysDate = "";

            string rtnVar = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            rtnVar = "NO";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT gamjumin";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_GAMF";
                SQL = SQL + ComNum.VBLF + " WHERE GAMJUMIN3 = '" + clsAES.AES((argJumin1 + argJumin2).Trim()) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GAMCODE ='" + argGam + "'";
                SQL = SQL + ComNum.VBLF + "   AND (GAMEND >= TO_DATE('" + strSysDate + "','YYYY-MM-DD') OR GAMEND IS NULL) ";
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + "       SELECT code ";
                SQL = SQL + ComNum.VBLF + "         FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "        WHERE GUBUN ='원무강제퇴사자감액' ";
                SQL = SQL + ComNum.VBLF + "          AND TRIM(CODE) = '" + argJumin1 + argJumin2 + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY 1  DESC  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = "OK";
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;

                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }

        /// <summary>
        /// CHK_무인수납_History 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argGam"></param>
        /// <param name="argJumin1"></param>
        /// <param name="argJumin2"></param>
        /// <returns></returns>
        public static string chk_AutomaticEquip_History(PsmhDb pDbCon, string ArgGubun, string ArgPano, string ArgJin, string ArgBi, string ArgDept, string ArgDrCode, string ArgMCode, string ArgVCode, string ArgMsg)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string rtnVal = "OK";

            try
            {
                SQL = "";
                SQL += " INSERT INTO ADMIN.ETC_SUNAP_AHIS                                                                                                                        \r";
                SQL += "            (BDATE,PANO,GUBUN,DEPTCODE,DRCODE,JIN,BI,MCODE,VCODE,MSG,ENTSABUN,ENTDATE )                                         \r";
                SQL += " VALUES                                                                                                                                                                                \r";
                SQL += "            (TRUNC(SYSDATE),'" + ArgPano + "','" + ArgGubun + "','" + ArgDept + "','" + ArgDrCode + "','" + ArgJin + "','" + ArgBi + "'    \r"; 
                SQL += "          , '" + ArgMCode + "','" + ArgVCode + "','" + ArgMsg.Replace("'", "`") + "'," + clsType.User.Sabun + ",SYSDATE  )                     \r"; 
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "NO";
                }

                clsDB.setCommitTran(pDbCon);
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "NO";
            }
        }
    }
}
