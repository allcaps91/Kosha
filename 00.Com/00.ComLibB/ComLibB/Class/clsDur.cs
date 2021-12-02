using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComDbB;
using ComBase;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using HiraDur;

namespace ComLibB
{
    public class clsDur
    {
        public static string GstrDurILL;

        public static int nMediCnt;         //약품 점검갯수
        public static string GstrDurIN;     //원내조제
        public static string GstrDurOUT;    //원외처방

        public static string strHIRADURrunPath;

        public static string m_strMprscType;
        public static string szLog;
        public static int nTotResultCnt;
        public static int nCheckType;

        public static string gstrDur_취소;

        public static HiraDur.IHIRAClient DurClient = default(HiraDur.IHIRAClient);
        public static HiraDur.IHIRAPrescription DurPrescription = default(HiraDur.IHIRAPrescription);
        public static HiraDur.IHIRAResultSet DurResultSet = default(HiraDur.IHIRAResultSet);

        //DUR2 -----------------------------------------------------------
        public static HiraDur.IHIRAClient DurClient3 = default(HiraDur.IHIRAClient);
        public static HiraDur.IHIRAResultSet DurResultSet3 = default(HiraDur.IHIRAResultSet);
        //----------------------------------------------------------------

        clsOrdFunction OF = new clsOrdFunction();

        public struct DurPatientInfo
        {
            public string strPtno;          //등록번호
            public string strsName;         //환자이름
            public string strBi;            //자격
            public string strJumin;         //주민번호
            public string strDeptCode;      //진료과
            public string strTuyakNo;       //투약번호
            public string strTuyakDate;     //투약일자(조제)
            public string strTuyakTime;     //투약시간(조제)
            public string strIO;            //퇴원약/외래약
            public string strBDate;         //처방일자
            public string strBTime;         //처방시간
            public string strDrCode;        //의사코드
            public string strDrSabun;       //의사사번
            public string strDrBunho;       //의사면허번호
            public string strDrName;        //의사이름
            public string strSend;          //전송여부
        }

        public struct ResultInfo
        {
            public string m_nIndex;  //점검결과 인덱스 번호

            public string m_strMedcCDA; // 입력약품코드
            public string m_strMedcNMA; // 입력약품명
            public string m_strGnlNMCDA; // 입력성분코드
            public string m_strGnlNMA; // 입력성분명
            public string m_fDDMqtyFreqA; // 입력1회투여량
            public string m_fDDExecFreqA; // 입력1일투여회수
            public string m_nMdcnExecFreqA; // 입력총투여일수

            public string m_nType;    // 점검종류코드(00-병용금기등)
            public string m_nLevel; // 점검결과등급(A, B, C ,D )
            public string m_strExamTypeCD; // 처방점검 / 복용점검 구분 (M/P/S/L)
            public string m_strNotice; // 부작용정보
            public string m_strMessage; // 점검내용
            public string m_strReasonCD; // 현재 요양기관에서 입력하는 사유코드
            public string m_strReason; // 현재 요양기관에서 입력하는 사유

            public string m_strDpPrscMake; // 중복처방조제구분
            public string m_strDpPrscYYMMDD; // 중복처방일자
            public string m_strDpPrscHMMSS; // 중복처방시간
            public string m_strDpPrscAdminCode; // 중복처방기관기호
            public string m_strDpPrscGrantNo; // 중복처방전교부번호
            public string m_strDpPrscAdminName; // 중복처방기관명
            public string m_strDpPrscTel; // 중복처방기관전화
            public string m_strDpPrscFax; // 중복처방기관팩스
            public string m_strDpPrscName; // 중복처방의사명
            public string m_strDpPrscLic; // 중복처방의사면허번호

            public string m_strDpMakeYYMMDD; // 중복조제일자
            public string m_strDpMakeHMMSS; // 중복조제시간
            public string m_strDpMakeAdminCode; // 중복조제기관기호
            public string m_strDpMakeAdminName; // 중복조제기관명
            public string m_strDpMakeTel; // 중복조제기관전화
            public string m_strDpMakeName; // 중복조제의사명
            public string m_strDpMakeLic; // 중복조제의사면허번호

            public string m_strMedcCDB; // 중복약품코드
            public string m_strMedcNMB; // 중복약품명
            public string m_strGnlNMCDB; // 중복성분코드
            public string m_strGnlNMB; // 중복성분명
            public string m_fDDMqtyFreqB; // 중복1회투여량
            public string m_fDDExecFreqB; // 중복1일투여회수
            public string m_nMdcnExecFreqB; // 중복총투여일수
            public string m_fDDTotalMqtyB; // 중복1일투여량(1회투여량x1일투여회수)

            public string strExamType;
        }
        public static ResultInfo[] RInfo = new ResultInfo[100];


        public struct AddMedicine
        {
            public string strprscType;        //분류유형코드(1.수가,3,약가, 5,원료, 8.재료) 현재 약가, 원료만처리
            public string strmedcCD;          //약품코드(성분명처방일 경우 NULL가능)
            public string strmedcNM;          //약품명(성분명처방일 경우 NULL 가능)
            public string strgnINMCD;         //성분코드
            public string strgnINM;           //성분명
            public double dblddMqtyFreq;      //1회 투여량
            public double dblddExecFreq;      //1일 투여횟수
            public int intmdcnexecfreq;       //총 투여일수
            public string strinsudmdtype;     //보험 적용구분(A:보험, B.비보험, C: 100/100 , D.일반판매약-2011.7월공시예정)
            public string strioHsp;           //원외(1),원내(2) 구분
            public string strPrscUsage;       //처방용법(NULL 가능-최대40byte)
        }

        public static void Clear_DurPatientInfo(DurPatientInfo DurAtc)
        {
            DurAtc.strPtno = "";          //등록번호
            DurAtc.strsName = "";         //환자이름
            DurAtc.strBi = "";            //자격
            DurAtc.strJumin = "";         //주민번호
            DurAtc.strDeptCode = "";      //진료과
            DurAtc.strTuyakNo = "";       //투약번호
            DurAtc.strTuyakDate = "";     //투약일자(조제)
            DurAtc.strTuyakTime = "";     //투약시간(조제)
            DurAtc.strIO = "";            //퇴원약/외래약
            DurAtc.strBDate = "";         //처방일자
            DurAtc.strBTime = "";         //처방시간
            DurAtc.strDrCode = "";        //의사코드
            DurAtc.strDrSabun = "";       //의사사번
            DurAtc.strDrBunho = "";       //의사면허번호
            DurAtc.strDrName = "";        //의사이름
            DurAtc.strSend = "";          //전송여부
        }

        public static void Clear_AddMedicine(AddMedicine DurMed)
        {
            DurMed.strprscType = "";        //분류유형코드(1.수가,3,약가, 5,원료, 8.재료) 현재 약가, 원료만처리
            DurMed.strmedcCD = "";          //약품코드(성분명처방일 경우 NULL가능)
            DurMed.strmedcNM = "";          //약품명(성분명처방일 경우 NULL 가능)
            DurMed.strgnINMCD = "";         //성분코드
            DurMed.strgnINM = "";           //성분명
            DurMed.dblddMqtyFreq = 0;      //1회 투여량
            DurMed.dblddExecFreq = 0;      //1일 투여횟수
            DurMed.intmdcnexecfreq = 0;       //총 투여일수
            DurMed.strinsudmdtype = "";     //보험 적용구분(A:보험, B.비보험, C: 100/100 , D.일반판매약-2011.7월공시예정)
            DurMed.strioHsp = "";           //원외(1),원내(2) 구분
            DurMed.strPrscUsage = "";       //처방용법(NULL 가능-최대40byte)
        }

        public static void Set_HiraDur()
        {
            if (System.Diagnostics.Debugger.IsAttached == false)
            {
                System.Type DurClient_t = System.Type.GetTypeFromProgID("HiraDur.Client", true);
                DurClient = (HiraDur.IHIRAClient)System.Activator.CreateInstance(DurClient_t, true);
                System.Type DurPrescription_t = System.Type.GetTypeFromProgID("HiraDur.Prescription", true);
                DurPrescription = (HiraDur.IHIRAPrescription)System.Activator.CreateInstance(DurPrescription_t, true);
                System.Type DurResultSet_t = System.Type.GetTypeFromProgID("HiraDur.ResultSet", true);
                DurResultSet = (HiraDur.IHIRAResultSet)System.Activator.CreateInstance(DurResultSet_t, true);
            }
        }

        public static bool DUR_CHECK(PsmhDb pDbCon, DurPatientInfo DurAtc, AddMedicine[] DurMed, string strAdminType, string strIO = "")
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strSlipNo = "";
            string m_strPrscAdmSym = "37100068";
            string strDur_Cancel = "";
            string strMsg = "";
            int intResult = 0;

            try
            {
                //DUR_전송한 내역은 존재 하나 당일 처방에 약제가 없는 경우 취소 창을 띄움니다.-----------------------------------------------------------
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SLIPNO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "DUR_MASTER ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + DurAtc.strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "         AND BDATE = TO_DATE('" + DurAtc.strBDate + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND DEPTCODE ='" + DurAtc.strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND DRCODE = '" + DurAtc.strDrCode + "' ";

                if (DurAtc.strTuyakNo != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND TUYAKNO = '" + DurAtc.strTuyakNo + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND TUDATE = TO_DATE('" + DurAtc.strTuyakDate + "','YYYY-MM-DD') ";
                }

                SQL = SQL + ComNum.VBLF + "         AND DDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "         AND DRUG = '1'";
                SQL = SQL + ComNum.VBLF + "         AND GbIO ='" + DurAtc.strIO + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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
                    strSlipNo = dt.Rows[0]["SLIPNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                //취소 로직 다시 만들기
                //2018-02-06
                if (DurAtc.strSend == "Y")
                {
                    rtnVal = DUR_CANCEL_AUTO(pDbCon, DurAtc, ref strDur_Cancel);

                    //퇴원약일경우 자동취소 후 재전송됨 dur_master
                    if (strDur_Cancel == "Y")
                    {
                        strSlipNo = "";
                    }
                }

                if (DurMed.Length == 0)
                {
                    clsDB.setRollbackTran(pDbCon);
                    //ComFunc.MsgBox("점검할 약품을 추가해주십시오!");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //2018.07.02 
                DurClear();
                DurPrescription.ClearMedicine();
                DurPrescription.AdminType = strAdminType;                       //처방조제구분                
                DurPrescription.JuminNo = DurAtc.strJumin;                      //수진자주민번호
                DurPrescription.PatNm = DurAtc.strsName;                        //수진자성명
                DurPrescription.PatTelNo = "";                                  //수진자전화번호

                switch (DurAtc.strBi)
                {
                    case "11": case "12": case "13": DurPrescription.InsurerType = "04"; break;  //건강보험
                    case "21": case "22": case "23": DurPrescription.InsurerType = "05"; break;  //의료급여"
                    case "31": case "32": case "33": DurPrescription.InsurerType = "10"; break;  //산재보험
                    case "52": case "55": DurPrescription.InsurerType = "11"; break;  //자동차보험
                    default: DurPrescription.InsurerType = "09"; break;  //일반
                }

                DurPrescription.PregWmnYN = "";                                       //수진자임부여부
                DurPrescription.MprscIssueAdmin = m_strPrscAdmSym;                    //처방기관기호
                DurPrescription.mprscGrantNo = DurAtc.strTuyakDate.Replace("-", "") + ComFunc.LPAD(DurAtc.strTuyakNo, 5, "0").Trim();   
                DurPrescription.PrscAdminName = "포항성모병원";                       //처방기관명
                DurPrescription.PrscTel = "0542720151";                               //처방기관전화번호
                DurPrescription.PrscFax = "0542772072";                               //처방기관팩스번호
                DurPrescription.PrscPresDt = DurAtc.strBDate;                         //처방일자
                DurPrescription.PrscPresTm = DurAtc.strBTime;                         //처방시간
                DurPrescription.PrscLicType = "AA";                                   //처방면허종별
                DurPrescription.DrLicNo = DurAtc.strDrBunho;                          //의사면허번호
                DurPrescription.PrscName = DurAtc.strDrName;                          //처방의료인성명
                DurPrescription.PrscMdFee = "";                                       //처방특정기호
                DurPrescription.Dsbjt = READ_DEPT_CODE(DurAtc.strDeptCode);           //진료과목코드
                DurPrescription.MainSick = "";                                        //주상병

                //처방조제유형코드
                if (DurAtc.strIO == "I")    //2015-04-02
                {
                    DurPrescription.PrscClCode = "06";                                //퇴원약
                }
                else if (DurAtc.strIO == "O")
                {
                    DurPrescription.PrscClCode = "05";                                //외래 원내조제
                }

                DurPrescription.PrscRef = "";                                    //처방조제참고사항
                DurPrescription.PrscIjCtm = "";                                  //처방조제참고사항
                DurPrescription.PrscPeriod = "7";                                //처방전사용일수
                DurPrescription.PrscUsage = "";                                  //처방용법
                DurPrescription.MakerIssueAdmin = m_strPrscAdmSym;               //조제기관기호
                DurPrescription.MakerAdminName = "포항성모병원";                  //조제기관명
                DurPrescription.MakerTel = "0542720151";                         //조제기관전화번호
                DurPrescription.MakerDate = DurAtc.strTuyakDate;                 //조제일자
                DurPrescription.MakerTime = DurAtc.strTuyakTime;                 //조제시간
                DurPrescription.MakerLic = "";                                   //조제약사면허번호
                DurPrescription.MakerName = "";                                  //조제약사성명
                DurPrescription.MakerUsage = "";                                 //조제기타내용
                DurPrescription.AppIssueAdmin = m_strPrscAdmSym;                 //청구소프트웨어 업체코드

                if (strSlipNo == "")
                {
                    DurPrescription.PrscYN = "N";                                //점검구분(N)
                }
                else
                {
                    DurPrescription.PrscYN = "M";                                //수정구분(M)
                }

                DurPrescription.MprscIssueAdmin = m_strPrscAdmSym;
                DurPrescription.AppIssueCode = "D09128112011202411037056720112"; //인증코드

                HiraDur.PrscType pType = HiraDur.PrscType.durPrscNone;

                for (i = 0; i < DurMed.Length; i++)
                {
                    switch (DurMed[i].strprscType)
                    {
                        case "0":
                            pType = HiraDur.PrscType.durPrscNone;
                            break;
                        case "1":
                            pType = HiraDur.PrscType.durPrscSuga;
                            break;
                        case "3":
                            pType = HiraDur.PrscType.durPrscYakga;
                            break;
                        case "5":
                            pType = HiraDur.PrscType.durPrscWonRyo;
                            break;
                        case "8":
                            pType = HiraDur.PrscType.durPrscJaeRyo;
                            break;
                    }

                    intResult = DurPrescription.AddMedicine(pType, DurMed[i].strmedcCD, DurMed[i].strmedcNM, DurMed[i].strgnINMCD,
                                                          DurMed[i].strgnINM, (float)DurMed[i].dblddMqtyFreq, (float)DurMed[i].dblddExecFreq, DurMed[i].intmdcnexecfreq,
                                                          DurMed[i].strinsudmdtype, DurMed[i].strioHsp, DurMed[i].strPrscUsage.Trim());
                    if (intResult != 0)
                    {  
                        ComFunc.MsgBox("오류 : AddMedicine 실패 : " + intResult);
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                DurResultSet.ClearReport();
                DurResultSet.ClearResult();

                if (rtnVal == true)
                {
                    DurClient.AdminCode = m_strPrscAdmSym;
                    DurPrescription.MprscIssueAdmin = m_strPrscAdmSym;
                    intResult = DurClient.Check(DurPrescription, DurResultSet);

                    if (intResult == 16023)     //사유전송이 COM모듈에서 완료됨
                    {
                        strMsg = "점검 완료 : " + DurClient.LastErrorMsg;
                        //nCheckType = 1 ' 사유 전송 완료
                        rtnVal = true;
                    }
                    else if (intResult == 16025)    //IHIRADUR COM 모듈에서 사유 전송 취소된 점검 결과
                    {
                        strMsg = "점검 완료 : " + DurClient.LastErrorMsg;
                        //nCheckType = 3
                        rtnVal = false;
                    }
                    else if (intResult != 0)
                    {
                        strMsg = "점검 실패[" + intResult + "] : " + DurClient.LastErrorMsg;

                        if (intResult == 53016)
                        {
                            ComFunc.MsgBox("약국에서 조제 완료된 오더입니다. " + ComNum.VBLF + " 처방 수정 및 추가처방은 동일한 의사로 처방 하여야합니다.");
                        }
                        else if (intResult == 50468)
                        {
                            ComFunc.MsgBox("접수구분 OPD_MASTER MCODE 특정기호 오류!!!");  //2012-02-22
                        }
                        else
                        {
                            ComFunc.MsgBox(strMsg);
                        }

                        rtnVal = false;
                    }
                    else
                    {
                        //nCheckType = 0 ' 사유 전송 미완료
                    }

                    if (rtnVal == true)
                    {
                        if (strSlipNo == "")
                        {
                            clsDB.setBeginTran(pDbCon);

                            //DB 등록
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "DUR_MASTER";
                            SQL = SQL + ComNum.VBLF + "     (ACTDATE, BDATE, PANO, DEPTCODE, ";
                            SQL = SQL + ComNum.VBLF + "     Bi, SLIPNO, ENTDATE, DRCODE, ";
                            SQL = SQL + ComNum.VBLF + "     GBIO, DRUG, TUYAKNO, TUDATE)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";
                            SQL = SQL + ComNum.VBLF + "         TRUNC(SYSDATE), ";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + DurAtc.strBDate + "','YYYY-MM-DD'), ";
                            SQL = SQL + ComNum.VBLF + "         '" + DurAtc.strPtno + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + DurAtc.strDeptCode + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + DurAtc.strBi + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + DurPrescription.mprscGrantNo + "', ";
                            SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                            SQL = SQL + ComNum.VBLF + "         '" + DurAtc.strDrCode + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + DurAtc.strIO + "', ";
                            SQL = SQL + ComNum.VBLF + "         '1', ";
                            SQL = SQL + ComNum.VBLF + "         " + DurAtc.strTuyakNo + ", ";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + DurAtc.strTuyakDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "     )";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            clsDB.setCommitTran(pDbCon);
                        }

                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        public static bool DUR_CHECK_ORDER(PsmhDb pDbCon, DurPatientInfo DurAtc, AddMedicine[] DurMed, string strAdminType, string strIO = "")
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = true;
            int i = 0;

            string strSlipNo = "";
            string m_strPrscAdmSym = "37100068";
            string strDur_Cancel = "";
            string strMsg = "";
            int intResult = 0;

            try
            {
                //DUR_전송한 내역은 존재 하나 당일 처방에 약제가 없는 경우 취소 창을 띄움니다.-----------------------------------------------------------
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SLIPNO ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "DUR_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + DurAtc.strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE = TO_DATE('" + DurAtc.strBDate + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE ='" + DurAtc.strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "    AND DRUG IS NULL      ";
                if (DurAtc.strIO == "" || DurAtc.strIO == "O")
                {
                    SQL = SQL + ComNum.VBLF + "         AND DRCODE = '" + clsOrdFunction.GstrDrCode + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         AND DRCODE = '" + clsOrdFunction.GstrDrCode_N + "' ";
                }
                SQL = SQL + ComNum.VBLF + "         AND DDATE IS NULL ";
                if (DurAtc.strIO != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND GbIO ='" + DurAtc.strIO + "' ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("DUR Master 조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    strSlipNo = dt.Rows[0]["SLIPNO"].ToString().Trim();
                }

                if (DurAtc.strIO == "T" || DurAtc.strIO == "I")
                {
                    //퇴원약이면 취소 후 재전송
                    rtnVal = DUR_CANCEL_AUTO_ORDER(pDbCon, DurAtc, ref strDur_Cancel);

                    if (strDur_Cancel == "Y")
                    {
                        strSlipNo = "";
                    }
                }
                else
                {
                    //if (strSlipNo != "" || nMediCnt == 0)
                    //{
                    //    rtnVal = DUR_CANCEL_AUTO_ORDER(pDbCon, DurAtc, ref strDur_Cancel);
                    //    return rtnVal;
                    //}

                    //2018.06.26 아래로 수정
                    if (strSlipNo != "")
                    {
                        rtnVal = DUR_CANCEL_AUTO_ORDER(pDbCon, DurAtc, ref strDur_Cancel);

                        strSlipNo = "";
                        //return rtnVal;
                    }
                }

                dt.Dispose();
                dt = null;

                if (nMediCnt == 0)
                {
                    return rtnVal;
                }
                
                if (DurMed.Length == 0)
                {
                    clsDB.setRollbackTran(pDbCon);
                    //ComFunc.MsgBox("점검할 약품을 추가해주십시오!");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //2018.07.02 
                DurClear();

                DurPrescription.ClearMedicine();
                DurPrescription.AdminType = strAdminType;                       //처방조제구분                
                DurPrescription.JuminNo = DurAtc.strJumin;                      //수진자주민번호
                DurPrescription.PatNm = DurAtc.strsName;                        //수진자성명
                DurPrescription.PatTelNo = "";                                  //수진자전화번호
                
                switch (DurAtc.strBi)
                {
                    case "11": case "12": case "13": DurPrescription.InsurerType = "04"; break;  //건강보험
                    case "21": case "22": case "23": DurPrescription.InsurerType = "05"; break;  //의료급여"
                    case "31": case "32": case "33": DurPrescription.InsurerType = "10"; break;  //산재보험
                    case "52": case "55": DurPrescription.InsurerType = "11"; break;  //자동차보험
                    default: DurPrescription.InsurerType = "09"; break;  //일반
                }
                if (clsOrdFunction.Pat.Pregnant == "Y")
                {
                    DurPrescription.PregWmnYN = "Y";                                       //수진자임부여부
                }
                else
                {
                    DurPrescription.PregWmnYN = "N";                                       //수진자임부여부
                }
                DurPrescription.MprscIssueAdmin = m_strPrscAdmSym;                    //처방기관기호
                //DurPrescription.mprscGrantNo = DurAtc.strTuyakDate.Replace("-", "") + ComFunc.LPAD(DurAtc.strTuyakNo, 5, "0").Trim();                    //처방전교부번호
                DurPrescription.mprscGrantNo = Read_SlipNo(clsDB.DbCon, strIO);         //처방전 교부번호   
                DurPrescription.PrscAdminName = "포항성모병원";                       //처방기관명
                DurPrescription.PrscTel = "0542720151";                               //처방기관전화번호
                DurPrescription.PrscFax = "0542772072";                               //처방기관팩스번호
                DurPrescription.PrscPresDt = DurAtc.strBDate.Replace("-","");         //처방일자
                DurPrescription.PrscPresTm = DurAtc.strBTime;                         //처방시간
                DurPrescription.PrscLicType = "AA";                                   //처방면허종별
                DurPrescription.DrLicNo = DurAtc.strDrBunho;                          //의사면허번호
                DurPrescription.PrscName = DurAtc.strDrName;                          //처방의료인성명
                DurPrescription.PrscMdFee = "";                                       //처방특정기호
                DurPrescription.Dsbjt = READ_DEPT_CODE(DurAtc.strDeptCode);           //진료과목코드
                DurPrescription.MainSick = "";                                        //주상병

                //처방조제유형코드
                if (DurAtc.strIO == "T")    //2015-04-02
                {
                    DurPrescription.PrscClCode = "06";                                //퇴원약
                }
                else if (GstrDurIN == "Y" && GstrDurOUT == "Y")
                {
                    DurPrescription.PrscClCode = "08";                                //외래 원외처방 및 원내처방
                }
                else if (GstrDurIN == "Y" && GstrDurOUT != "Y")
                {
                    DurPrescription.PrscClCode = "05";                                //외래 원내조제
                }
                else
                {
                    DurPrescription.PrscClCode = "02";                                //외래 원외처방
                }

                DurPrescription.PrscRef = "";                                    //처방조제참고사항
                DurPrescription.PrscIjCtm = "";                                  //처방조제참고사항
                DurPrescription.PrscPeriod = "7";                                //처방전사용일수
                DurPrescription.PrscUsage = "";                                  //처방용법
                DurPrescription.MakerIssueAdmin = m_strPrscAdmSym;               //조제기관기호
                DurPrescription.MakerAdminName = "포항성모병원";                  //조제기관명
                DurPrescription.MakerTel = "0542720151";                         //조제기관전화번호
                DurPrescription.MakerDate = DurAtc.strTuyakDate;                 //조제일자
                DurPrescription.MakerTime = DurAtc.strTuyakTime;                 //조제시간
                DurPrescription.MakerLic = "";                                   //조제약사면허번호
                DurPrescription.MakerName = "";                                  //조제약사성명
                DurPrescription.MakerUsage = "";                                 //조제기타내용
                DurPrescription.AppIssueAdmin = m_strPrscAdmSym;                 //청구소프트웨어 업체코드

                if (strSlipNo == "")
                {
                    DurPrescription.PrscYN = "N";                                //점검구분(N)
                }
                else
                {
                    DurPrescription.PrscYN = "M";                                //수정구분(M)
                }

                DurPrescription.MprscIssueAdmin = m_strPrscAdmSym;
                DurPrescription.AppIssueCode = "D09128112011202411037056720112"; //인증코드

                HiraDur.PrscType pType = HiraDur.PrscType.durPrscNone;

                for (i = 0; i < DurMed.Length; i++)
                {
                    if (DurMed[i].strprscType != null)
                    {
                        switch (DurMed[i].strprscType)
                        {
                            case "0":
                                pType = HiraDur.PrscType.durPrscNone;
                                break;
                            case "1":
                                pType = HiraDur.PrscType.durPrscSuga;
                                break;
                            case "3":
                                pType = HiraDur.PrscType.durPrscYakga;
                                break;
                            case "5":
                                pType = HiraDur.PrscType.durPrscWonRyo;
                                break;
                            case "8":
                                pType = HiraDur.PrscType.durPrscJaeRyo;
                                break;
                        }


                        intResult = DurPrescription.AddMedicine(pType, DurMed[i].strmedcCD, DurMed[i].strmedcNM, DurMed[i].strgnINMCD,
                                                              DurMed[i].strgnINM, (float)DurMed[i].dblddMqtyFreq, (float)DurMed[i].dblddExecFreq, DurMed[i].intmdcnexecfreq,
                                                              DurMed[i].strinsudmdtype, DurMed[i].strioHsp, DurMed[i].strPrscUsage.Trim());
                        if (intResult != 0)
                        {
                            //clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox("오류 : AddMedicine 실패 : " + intResult);
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                    
                }

                DurResultSet.ClearReport();
                DurResultSet.ClearResult();

                if (rtnVal == true)
                {
                    DurClient.AdminCode = m_strPrscAdmSym;
                    DurPrescription.MprscIssueAdmin = m_strPrscAdmSym;
                    intResult = DurClient.Check(DurPrescription, DurResultSet);

                    if (intResult == 16023)     //사유전송이 COM모듈에서 완료됨
                    {
                        strMsg = "점검 완료 : " + DurClient.LastErrorMsg;
                        //nCheckType = 1 ' 사유 전송 완료
                        //rtnVal = false;
                        rtnVal = true;
                    }
                    else if (intResult == 16025)    //IHIRADUR COM 모듈에서 사유 전송 취소된 점검 결과
                    {
                        strMsg = "점검 완료 : " + DurClient.LastErrorMsg;
                        //nCheckType = 3
                        //rtnVal = false;
                        rtnVal = true;
                    }
                    else if (intResult != 0)
                    {
                        strMsg = "점검 실패[" + intResult + "] : " + DurClient.LastErrorMsg;

                        if (intResult == 53016)
                        {
                            ComFunc.MsgBox("약국에서 조제 완료된 오더입니다. " + ComNum.VBLF + " 처방 수정 및 추가처방은 동일한 의사로 처방 하여야합니다.");
                        }
                        else if (intResult == 50468)
                        {
                            ComFunc.MsgBox("접수구분 OPD_MASTER MCODE 특정기호 오류!!!");  //2012-02-22
                        }
                        else if (intResult == 16009)    //점검약품 없음)
                        {
                            rtnVal = true;
                        }
                        else
                        {
                            ComFunc.MsgBox(strMsg);
                        }
                        //rtnVal = false;
                        rtnVal = true;
                    }
                    else if (intResult == 16009)    //점검약품 없음
                    {
                        rtnVal = true;
                    }
                    else
                    {
                        //nCheckType = 0 ' 사유 전송 미완료
                    } 

                    if (rtnVal == true)
                    {
                        if (strSlipNo == "")
                        {
                            clsDB.setBeginTran(pDbCon);

                            //DB 등록
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "DUR_MASTER";
                            SQL = SQL + ComNum.VBLF + "     (ACTDATE, BDATE, PANO, DEPTCODE, ";
                            SQL = SQL + ComNum.VBLF + "     Bi, SLIPNO, ENTDATE, DRCODE, ";
                            SQL = SQL + ComNum.VBLF + "     GBIO)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";
                            SQL = SQL + ComNum.VBLF + "         TRUNC(SYSDATE), ";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + DurAtc.strBDate + "','YYYY-MM-DD'), ";
                            SQL = SQL + ComNum.VBLF + "         '" + DurAtc.strPtno + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + DurAtc.strDeptCode + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + DurAtc.strBi + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + DurPrescription.mprscGrantNo + "', ";
                            SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                            SQL = SQL + ComNum.VBLF + "         '" + DurAtc.strDrCode + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + DurAtc.strIO + "' ";
                            SQL = SQL + ComNum.VBLF + "     )";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                //return false;
                                return true;
                            }

                            clsDB.setCommitTran(pDbCon);
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            return rtnVal;
        }

        public static void DurClear()
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(DurPrescription);
            System.Type DurPrescription_t = System.Type.GetTypeFromProgID("HiraDur.Prescription", true);
            DurPrescription = (HiraDur.IHIRAPrescription)System.Activator.CreateInstance(DurPrescription_t, true);
        }

        public static string READ_DEPT_CODE(string strDept)
        {
            string rtnVal = "";

            switch (strDept)   //외래는 무조건 진료과통일
            {
                case "MD": rtnVal = "00"; break;        //통합            : 00
                case "MG": rtnVal = "01"; break;        //소화기내과      : 01
                case "MC": rtnVal = "02"; break;        //순환기내과      : 02
                case "MP": rtnVal = "03"; break;        //호흡기내과      : 03
                case "ME": rtnVal = "04"; break;        //내분비,대사내과 : 04
                case "MN": rtnVal = "05"; break;        //신장내과        : 05
                case "MI": rtnVal = "07"; break;        //감염내과 2013-11-14
                //혈액종양내과    : 06
                //감염내과        : 07
                //알레르기내과    : 08
                case "MR": rtnVal = "09"; break;        //류마티스내과    : 09
                case "HD": rtnVal = "05"; break;
                case "NE": rtnVal = "02"; break;
                case "NP": rtnVal = "03"; break;
                case "GS": rtnVal = "04"; break;
                case "OS": rtnVal = "05"; break;
                case "NS": rtnVal = "06"; break;
                case "CS": rtnVal = "07"; break;
                case "PS": rtnVal = "08"; break;
                case "AN": rtnVal = "09"; break;
                case "RT": case "PC": rtnVal = "09"; break;
                case "OB": case "OG": rtnVal = "10"; break;
                case "PD": rtnVal = "11"; break;
                case "OT": rtnVal = "12"; break;
                case "EN": rtnVal = "13"; break;
                case "DM": rtnVal = "14"; break;
                case "UR": rtnVal = "15"; break;
                case "PT": case "RM": rtnVal = "21"; break;
                case "FM": rtnVal = "23"; break;
                case "EM": case "ER": rtnVal = "24"; break;
                case "DN": case "DT": rtnVal = "55"; break;
            }

            return rtnVal;
        }

        public static bool DUR_CANCEL_AUTO(PsmhDb pDbCon, DurPatientInfo DurAtc, ref string strDru_Cancel)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strSlipNo = "";
            int intResult = 0;

            strDru_Cancel = "N";

            try
            {
                //당일 처방 번호가 있는지 점검
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SLIPNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "DUR_MASTER ";
                SQL = SQL + ComNum.VBLF + "     WHERE BDATE = TO_DATE('" + DurAtc.strBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + DurAtc.strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "         AND DEPTCODE= '" + DurAtc.strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND GbIO = '" + DurAtc.strIO + "' ";  //2015-02-12
                SQL = SQL + ComNum.VBLF + "         AND DRCODE = '" + DurAtc.strDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND TUYAKNO = '" + DurAtc.strTuyakNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND TUDATE = TO_DATE('" + DurAtc.strTuyakDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND DRUG = '1' ";
                SQL = SQL + ComNum.VBLF + "         AND DDATE IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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
                    strSlipNo = dt.Rows[0]["SLIPNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strSlipNo == "") { return rtnVal; }

                DurClient.AdminCode = "37100068";
                
                intResult = DurClient.MprscCancel("M", DurAtc.strJumin, DurAtc.strBDate.Replace("-", ""), "37100068", strSlipNo, "M5", "처방 또는 조제일자 착오입력", "");

                if (intResult == 0)
                {
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_PMPA + "DUR_MASTER";
                    SQL = SQL + ComNum.VBLF + "     SET ";
                    SQL = SQL + ComNum.VBLF + "         DDATE = SYSDATE ";
                    SQL = SQL + ComNum.VBLF + "   WHERE PANO = '" + DurAtc.strPtno + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND DEPTCODE= '" + DurAtc.strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND GbIO ='" + DurAtc.strIO + "' ";  //2015-02-12
                    SQL = SQL + ComNum.VBLF + "     AND DRCODE = '" + DurAtc.strDrCode + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND TUYAKNO = '" + DurAtc.strTuyakNo + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND TUDATE = TO_DATE('" + DurAtc.strTuyakDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND DRUG = '1' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    strDru_Cancel = "Y";
                    rtnVal = true;
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "DUR_MASTER";
                    SQL = SQL + ComNum.VBLF + "     SET ";
                    SQL = SQL + ComNum.VBLF + "         DDATE = SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         CDate = SYSDATE ";
                    SQL = SQL + ComNum.VBLF + "   WHERE PANO = '" + DurAtc.strPtno + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND DEPTCODE= '" + DurAtc.strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND GbIO ='" + DurAtc.strIO + "' ";  //2015-02-12
                    SQL = SQL + ComNum.VBLF + "     AND DRCODE = '" + DurAtc.strDrCode + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND DRUG = '1' ";
                    SQL = SQL + ComNum.VBLF + "     AND TUYAKNO = '" + DurAtc.strTuyakNo + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND TUDATE = TO_DATE('" + DurAtc.strTuyakDate + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    strDru_Cancel = "N";
                    rtnVal = false;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        public static bool DUR_CANCEL_AUTO_ORDER(PsmhDb pDbCon, DurPatientInfo DurAtc, ref string strDru_Cancel)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = true;

            string strSlipNo = "";
            int intResult = 0;

            strDru_Cancel = "N";

            try
            {
                //당일 처방 번호가 있는지 점검
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SLIPNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "DUR_MASTER ";
                SQL = SQL + ComNum.VBLF + "     WHERE BDATE = TO_DATE('" + DurAtc.strBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "       AND PANO = '" + DurAtc.strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "       AND DEPTCODE= '" + DurAtc.strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "       AND DDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "       AND DRUG is NULL  ";
                if (DurAtc.strIO != "")
                { 
                    SQL = SQL + ComNum.VBLF + "       AND GbIO = '" + DurAtc.strIO + "' ";
                }

                if (DurAtc.strIO != "" || DurAtc.strIO != "O")
                {
                    SQL = SQL + ComNum.VBLF + "       AND DRCODE = '" + clsOrdFunction.GstrDrCode + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "       AND DRCODE = '" + clsOrdFunction.GstrDrCode_N + "' ";
                }                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("DUR MASTER 조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strSlipNo = dt.Rows[0]["SLIPNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strSlipNo == "") { return rtnVal; }

                DurClient.AdminCode = "37100068";
                intResult = DurClient.MprscCancel("M", DurAtc.strJumin, DurAtc.strBDate.Replace("-", ""), "37100068", strSlipNo, "M5", "처방 또는 조제일자 착오입력", "");

                if (intResult == 0)
                {
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_PMPA + "DUR_MASTER";
                    SQL = SQL + ComNum.VBLF + "     SET ";
                    SQL = SQL + ComNum.VBLF + "         DDATE = SYSDATE ";
                    SQL = SQL + ComNum.VBLF + "   WHERE PANO = '" + DurAtc.strPtno + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND SLIPNO = '" + strSlipNo + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    strDru_Cancel = "Y";
                    rtnVal = true;
                }
                else
                {
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_PMPA + "DUR_MASTER";
                    SQL = SQL + ComNum.VBLF + "     SET ";
                    SQL = SQL + ComNum.VBLF + "         DDATE = SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         CDate = SYSDATE ";
                    SQL = SQL + ComNum.VBLF + "   WHERE PANO = '" + DurAtc.strPtno + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND DEPTCODE= '" + DurAtc.strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND SLIPNO = '" + strSlipNo + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    strDru_Cancel = "N";
                    //rtnVal = false;
                    rtnVal = true;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        public static bool DUR_CANCEL(PsmhDb pDbCon, DurPatientInfo DurAtc, ref string strDru_Cancel)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = true;

            string strSlipNo = "";
            int intResult = 0;

            strDru_Cancel = "N";

            try
            {
                //당일 처방 번호가 있는지 점검
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SLIPNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "DUR_MASTER ";
                SQL = SQL + ComNum.VBLF + "     WHERE BDATE = TO_DATE('" + DurAtc.strBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + DurAtc.strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "         AND DEPTCODE= '" + DurAtc.strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND DDATE IS NULL ";
                if (DurAtc.strIO != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND GbIO = '" + DurAtc.strIO + "' ";  //2015-02-12
                }

                if (DateTime.Parse(DurAtc.strBDate) >= DateTime.Parse("2011-12-13"))
                {
                    SQL = SQL + ComNum.VBLF + "         AND DRCODE = '" + clsOrdFunction.GstrDrCode + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         AND DRCODE = '" + clsOrdFunction.GstrDrCode_N + "' "; 
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    //ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strSlipNo = dt.Rows[0]["SLIPNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strSlipNo == "") { return rtnVal; }

                FrmMedDurCheckCancel f = new FrmMedDurCheckCancel(clsOrdFunction.GstrBDate, "37100068", strSlipNo);
                f.ShowDialog();
                //OF.fn_ClearMemory(f);
                
                
                if (clsOrdFunction.bOK == true)
                {
                    long nResult;

                    string m_strPrscDd;
                    m_strPrscDd = clsOrdFunction.GstrDurCancelPrscDd;
                    m_strPrscDd = clsOrdFunction.GstrDurCancelPrscDd.Replace("-", "");

                    DurClient.AdminCode = "37100068";

                    nResult = DurClient.MprscCancel("M", clsOrdFunction.Pat.JUMIN, m_strPrscDd, clsOrdFunction.GstrDurPrscAdmSym, clsOrdFunction.GstrDurCancelGrantNo, clsOrdFunction.GstrDurCancelReasonCd, clsOrdFunction.GstrDurCancelReasonMsg, "");

                    if (nResult == 0)
                    {
                        try
                        {
                            clsDB.setBeginTran(clsDB.DbCon);

                            SQL = "";
                            SQL = " UPDATE KOSMOS_PMPA.DUR_MASTER SET DDATE =SYSDATE WHERE PANO ='" + clsOrdFunction.Pat.PtNo + "' AND SLIPNO = '" + strSlipNo + "' ";
                            if (DurAtc.strIO != "")
                            {
                                SQL += "  AND GbIO = '" + DurAtc.strIO + "'    ";
                            }
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
                            Cursor.Current = Cursors.Default;
                        }
                        catch (Exception ex)
                        {
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        }
                    }
                    else
                    {
                        szLog = "점검 취소 실패[" + nResult + "] : " + DurClient.LastErrorMsg;
                        MessageBox.Show(szLog, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtnVal = false;
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// DUR_퇴원약_시행()
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        public static bool Dur_OutMedYN(PsmhDb pDbCon)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL += " SELECT NAME                        \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_BCODE       \r";
                SQL += "  WHERE GUBUN = 'DUR_퇴원약_시행'   \r";
                SQL += "    AND CODE = 'USE'                \r";
                SQL += "    AND NAME = 'Y'                  \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// DUR_응급실_시행()
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        public static bool Dur_ErMedYN(PsmhDb pDbCon)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL += " SELECT NAME                        \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_BCODE       \r";
                SQL += "  WHERE GUBUN = 'DUR_응급실_시행'   \r";
                SQL += "    AND CODE = 'USE'                \r";
                SQL += "    AND NAME = 'Y'                  \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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
        /// DUR_예외처리_등록번호
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtno"></param>
        /// <returns></returns>
        public static bool Dur_Exception_Ptno(PsmhDb pDbCon, string strPtno)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL += " SELECT NAME                                \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_BCODE               \r";
                SQL += "  WHERE GUBUN = 'DUR_예외처리_등록번호''    \r";
                SQL += "    AND CODE = 'USE'                        \r";
                SQL += "    AND NAME = 'Y'                          \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        public static string DUR_CHECK_Mers(PsmhDb pDbCon, string strJumin, string sSName, string strBDate)
        {
            string m_strPrscAdmSym = "37100068";
            int intResult = 0;
            string rtnVal = "NO";
            
            DurPrescription.AdminType = "";                                 //' 처방조제구분
            DurPrescription.JuminNo = strJumin.Replace("-", "");            //' 수진자주민번호
            DurPrescription.PatNm = sSName;                                 //' 수진자성명
            DurPrescription.MprscIssueAdmin = m_strPrscAdmSym;              //' 처방기관기호
            DurPrescription.PrscAdminName = "포항성모병원";                 //' 처방기관명
            DurPrescription.PrscPresDt = strBDate.Replace("-", "");         //' 처방일자
            DurPrescription.AppIssueAdmin = m_strPrscAdmSym;                //' 청구소프트웨어 업체코드
            DurPrescription.MprscIssueAdmin = m_strPrscAdmSym;
            DurPrescription.AppIssueCode = "D09128112011202411037056720112";//'인증코드

            DurClient.AdminCode = m_strPrscAdmSym;
            DurPrescription.MprscIssueAdmin = m_strPrscAdmSym;
            
            intResult = DurClient.ParticularDiseaseCheck(DurPrescription, DurResultSet);

            if (intResult != 0)
            {
                if (clsOrdFunction.GstrGbJob == "ER")
                {
                    szLog = "DUR Mers 점검실패[" + intResult + "] : " + DurClient.LastErrorMsg;
                }
                rtnVal = "NO";
            }
            else
            {
                if (clsOrdFunction.GstrGbJob == "ER")
                {
                    MessageBox.Show("점검완료", "DUR점검", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                rtnVal = "OK";
            }
            
            return rtnVal;
        }

        public static string DUR_CHECK_Mers_New(PsmhDb pDbCon, string strJumin, string sSName, string strBDate)
        {
            string m_strPrscAdmSym = "37100068";
            int intResult = 0;

            long nTotResultCnt;
            string strMsg = "";

            string rtnVal = "NO";

            DurPrescription.AdminType = "";                                 //' 처방조제구분
            DurPrescription.JuminNo = strJumin.Replace("-", "");            //' 수진자주민번호
            DurPrescription.PatNm = sSName;                                 //' 수진자성명
            DurPrescription.MprscIssueAdmin = m_strPrscAdmSym;              //' 처방기관기호
            DurPrescription.PrscAdminName = "포항성모병원";                 //' 처방기관명
            DurPrescription.PrscPresDt = strBDate.Replace("-", "");         //' 처방일자
            DurPrescription.AppIssueAdmin = m_strPrscAdmSym;                //' 청구소프트웨어 업체코드
            DurPrescription.MprscIssueAdmin = m_strPrscAdmSym;
            DurPrescription.AppIssueCode = "D09128112011202411037056720112";//'인증코드

            DurClient.AdminCode = m_strPrscAdmSym;
            DurPrescription.MprscIssueAdmin = m_strPrscAdmSym;

            intResult = DurClient.ParticularDiseaseCheck(DurPrescription, DurResultSet);

            if (intResult != 0)
            {
                if (clsOrdFunction.GstrGbJob == "ER")
                {
                    szLog = "DUR Mers 점검실패[" + intResult + "] : " + DurClient.LastErrorMsg;
                }
                rtnVal = "NO";
            }
            else
            {
                if (clsOrdFunction.GstrGbJob == "ER")
                {
                    MessageBox.Show("점검완료", "DUR점검", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (DurResultSet.Totalcnt > 0)
                {
                    nTotResultCnt = DurResultSet.Totalcnt;

                    DurResultSet.NextResult();
                    strMsg = "";
                    for (var i = 0; i < nTotResultCnt; i++)
                    {
                        if (i == 0)
                        {
                            strMsg = DurResultSet.Message;
                        }
                        else
                        {
                            strMsg += "\r\n" + DurResultSet.Message;
                        }
                    }

                    if (strMsg.Trim() != "")
                    {
                        fn_Send_ITS(strMsg.Trim());
                    }
                }
                rtnVal = "OK";
            }

            return rtnVal;
        }

        public static void fn_Send_ITS(string msgITS)   // ITS : International Traveler Information System)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0; //변경된 Row 받는 변수

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += "   merge into KOSMOS_OCS.EXAM_INFECT_MASTER a                                   \r";
                SQL += "   using dual b                                                                 \r";
                SQL += "      on (a.PTNO = '" + clsOrdFunction.Pat.PtNo + "'                            \r";
                SQL += "     and  a.GUBUN = '06'                                                        \r";
                SQL += "     and  a.CODE = 'F99'                                                        \r";
                SQL += "     and  a.RDATE <= TRUNC(SYSDATE) - 30)                                       \r";
                SQL += "    when matched then                                                           \r";
                SQL += "  update set                                                                    \r";
                SQL += "         GUBUN = '06'                                                           \r";    //의미없는 update
                SQL += "    when not matched then                                                       \r";
                SQL += "  insert                                                                        \r";
                SQL += "        (RDATE, PANO, GUBUN, SPECNO, EXNAME, ODATE, OSABUN, CODE, INFO)         \r";
                SQL += "  values                                                                        \r";
                SQL += "        (to_date('" + clsOrdFunction.GstrBDate + "', 'yyyy-mm-dd')              \r";
                SQL += "       , '" + clsOrdFunction.Pat.PtNo + "'                                      \r";
                SQL += "       , '06'                                                                   \r";
                SQL += "       , ''                                                                     \r";
                SQL += "       , ''                                                                     \r";
                SQL += "       , ''                                                                     \r";
                SQL += "       , ''                                                                     \r";
                SQL += "       , 'F99'                                                                  \r";
                SQL += "       , '" + msgITS + "')                                                      \r";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr + " 신종 감염병 점검내역 저장중 오류 발생!!!");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message + " 신종 감염병 점검내역 저장중 오류 발생!!!");
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        public static string Read_SlipNo(PsmhDb pDbCon, string sGBIO)
        {
            string rtnVal = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            //당일 처방 번호가 있는지 점검
            try
            {
                SQL = "";
                SQL += " SELECT SLIPNO FROM KOSMOS_PMPA.DUR_MASTER                              \r";
                SQL += "  WHERE BDATE =TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD') \r";
                SQL += "    AND PANO = '" + clsOrdFunction.Pat.PtNo + "'                        \r";
                SQL += "    AND DEPTCODE= '" + clsOrdFunction.Pat.DeptCode + "'                 \r";
                SQL += "    AND DDATE IS NULL                                                   \r";
                SQL += "    AND DRUG IS NULL                                                    \r";
                if (sGBIO != "")
                {
                    SQL += "    AND GbIO ='" + sGBIO + "'                                       \r";
                }
                if (sGBIO == "" || sGBIO == "O")
                {
                    SQL += "    AND DRCODE = '" + clsOrdFunction.GstrDrCode + "'                \r";
                }
                else
                {
                    SQL += "    AND DRCODE = '" + clsOrdFunction.GstrDrCode_N + "'              \r";
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SLIPNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (rtnVal != "")
                {
                    return rtnVal;
                }

                //당일 없으면 처방번호 생성
                SQL = "";
                SQL += " SELECT  KOSMOS_PMPA.SEQ_DUR_SLIPNO.NEXTVAL  NVAL FROM DUAL \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                rtnVal = clsPublic.GstrSysDate.Replace("-", "") + ComFunc.LPAD(dt.Rows[0]["NVAL"].ToString(), 5, "0");

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
        /// 심평원 DUR 본인약 내역 조회 ( 최근3개월자료 ) - 심평원 홈페이지에 DUR 정보동의 환자 만
        /// </summary>
        /// <param name="sPano"></param>
        /// <param name="sDrCode"></param>
        public string DUR_CHECK_DrugHis(string sPano, string sDrCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";     //에러문 받는 변수

            string strOK = "";
            string m_strPrscAdmSym = "";
            long nResult3 = 0;
            string strJumin = "";
            long nLicense = 0;
            string strMessage = "";

            string strRtn = "";

            strMessage = "[이용절차] " + "\r\n" + "\r\n" + "1.건강보험심사평가원 홈페이지 접속" + "\r\n\r\n" +
                         "2.의료정보 클릭 " + "\r\n\r\n" + "3.개인정보 수집동의 " + "\r\n\r\n" +
                         "4.주민번호번호입력 및 공인인증서 로그인 " + "\r\n\r\n" +
                         "5.제3자 정보제공 동의 및 폰번호 등록" + "\r\n\r\n" +
                         "위의 절차(1~5)를 완료한 대상자만 정보조회가 가능합니다.";

            if (MessageBox.Show("심평원 DUR 서비스[건강보험심사평가원]" + "\r\n\r\n" + "내가 먹는약 한눈에 확인!!" + "\r\n\r\n" + "개인 투약이력 서비스" + "\r\n\r\n" + strMessage + "\r\n\r\n" + "자료 조회를 하시겠습니까??", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return strRtn;
            }
            
            m_strPrscAdmSym = "37100068";
            strRtn = "OK";

            nLicense = long.Parse(OF.fn_READ_OCS_Doctor_DrBunho(sDrCode));
            strJumin = OF.fn_Read_Bas_Jumin2(sPano);
            ///TODO : 이상훈 오류 확인
            nResult3 = DurClient3.CheckMediHistory(strJumin, m_strPrscAdmSym, nLicense.ToString());

            return strRtn;
        }
    }
}
