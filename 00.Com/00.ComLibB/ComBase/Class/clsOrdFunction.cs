using ComBase.Controls;
using ComBase.Mvc;
using ComDbB; //DB연결
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ComBase
{
    public class clsOrdFunction : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SP.Dispose();
                CF.Dispose();
            }

            this.GnSELECTSlips = null;
            this.GstrSELECTSlipsno = null;
            this.GnChk = null;
            this.GnRe = null;

            base.Dispose(disposing);
        }

        public static string Gstr전송대상오더;    //baggage.bas에서 가져옴

        public static string Gstroutdate;       //상병조회에서 사용(2017.07.10 이상훈 추가)
        public static int GDosDblClickRow;      //처방화면에서 용법 선택 Row
        public static string GdosLoadLocation;  //용법코드폼 로드시 위치 정보

        public static string GstrLoadFlag = ""; //용법
        public static string GstrLoadFlag2 = "";

        public static string GstrSystemEndYN;   //DashBoard에서 종료 버튼 클릭시 처방화면 종료 
        public static bool bOK;

        public static string GstrSetOrderGubun; //약속처방 입력시 오더구분(Adm, PreOp, PostOp....)
        public static string GstrSetGbAct; //약속처방 입력
        public static int GstrSetSort; //약속처방 입력

        public static string GstrCPCode;   //약속처방 등록 화면 사용
        public static string GstrCPName;
        public static int GnCPDay;
        public static string GStrOrderGubun;    //약속처방 구분
        public static string GStrDeptDR;    //약속처방 (개인,과)

        public static string GstrCPOrderChk;    //GstrCP처방Chk

        public static string GstrSetRegYN;      //약속처방 작성 여부
        public static string GstrSetOldYN;      //이전처방 활용 여부 : 2018-11-09 박웅규 추가

        public static string GstrBloodRsvDateFlag;  //마취신체등급창 로드 여부
        public static string GstrASAFlag;           //ASA 입력창 로드 여부

        public static string GstrExitYN;            //DashBorad 종료시 FrmMedOrders 폼 종료

        public static string GstrJinYN;         //2018.06.01 추가 진료 여부(1:진료대기/2:진료/3:재진)

        public static string GstrAmPmSet;       //환자리스트 외래 (전체/오전/오후 Set)

        public static string Set_Screen;        //FrmOcsTray 화면복귀 여부
        public static string Set_BELL;          //FrmOcsTray 알림시 벨소리 여부

        public static string GstrGbIO;          //환자리스트 선택구분(OPD/ER/IPD)

        public static string GSelfMedOrd;       //자가약 입력 여부
        public static string GSelfMedDosCode;   //자가약 용법 코드
        public static string GSelfMedDiv;       //자가약 Div
        public static string GSelfMedPlusName;  //자가약 용법
        public static string GSelfMedQty;       //자가약 수량

        public static string GstrCallForm;      //prn 처방 상세 호출 경로(PRN 처방 화면 / 처방화면)
        public static string GstrPrnDosCode;    //prn 처방 용법 코드

        public static string GstrGbConsult;     //Consult 화면 격리신청 여부

        public static string GstrMessage;       //병동 처방 1일이상 일수 제한 메시지


        public static string GstrPatNo; //처방조회 환자검색 사용
        public static string GstrRsvCancelFlag; //예약 취소 여부

        public static string GstrMayakFlag;     //1Time 처방 마약여부

        public static string GstrAgreePrt;      //수혈 동의서 출력 여부

        public static string GstrViewSlipCallPosition;  //오더판넬 호출 폼

        public static string GstrCon_ONOFF;     //병동처방 협의오더 체크 여부
        public static string GstrCon_DeptCode;  //병동처방 협의오더 과
        public static string GstrCon_DrCode;    //병동처방 협의오더 의사
        public static string GstrCon_WardCode;  //병동처방 협의오더 병동

        public static string GstrDosER;         //응급OCS : ER 용법 체크 여부
        public static string GstrSlipSearch;    //slip 내 검색 여부

        //clsDB DC = new clsDB();
        clsSpread SP = new clsSpread();
        //AdoConst AC = new AdoConst();
        ComFunc CF = new ComFunc();

        //long rowcounter;

        //DataTable dt = null;
        //DataTable dt1 = null;
        //DataTable dt2 = null;
        //string SQL = "";    //Query문
        //string SqlErr = ""; //에러문 받는 변수
        ////string strValue;
        //int intRowAffected = 0; //변경된 Row 받는 변수

        public static string GstrGbPrm;

        public static int GnDashCnt;

        public static string GstrGbJob; //작업구분(외래/응급/입원/수술 (OPD/ER/IPD/OPR))

        public static long GnHeight; //화면 최대크기

        public static long GnIpdNO_OCS;
        public static long GnTrsNO_OCS;
        //public static string GstrICUWard;
        public static string GstrSlipChek;     // Slip 찾기에서 찾기폼에서 바로 오더 입력하기 버턴 만들기
        public static int GstrC;
        public static int GstrR;


        #region 제한항생제
        // FrmAnti 에 사용
        public static string GstrAntiSucode;
        public static string GstrAntiOrderName;
        public static string GstrAntiPlusName;
        public static long GnAntiContents;
        public static int GnAnitRealQty;
        public static int GnAntiDiv;
        public static int GnAntiNal;
        public static string GstrAntiDosCode;
        public static string GstrAntiOrderCode;
        #endregion

        public static long GnIPDNO;

        public static string GstrMsgNur;           // 간호사에게 오더전송의 메제지 전달
        public static string GstrMsgNurYN;         // 오더전송 전송여부
        public static string GstrPrmDrCode;        // 손동현 추가 병동에서 전공의들의 약속처방 및 Remark, 상병을 위해서 추가

        //public int[][] GnScale(900, 540)        ;
        public static int[,] GnScale = new int[900, 540];
        public static int GnPrintCnt;

        public static int GFirstOrders;        //처음 Order 화면시 True Else False
        public object GOrderFORM;       // 현재의 Order Form
        public static string GstrSelPatient;
        public static string GstrTransforPro;  // Transfor 구분
        public static string GstrLogin;
        public static string GstrBDate;
        public static string GstrBDate1;    //2018.10.10 처방화면에서 처방일 변경 처방전송시 원 처방 일자
        public static string GstrGbOrder;
        public static string GstrHolyDay;

        //public static string  GstrJobMan;
        public static string GstrJobGbreg;
        public static string GstrSelectIpdDept = "";  //2018-11-28 박웅규 : 전공의 선택한 진료과
        public static string GstrSelectIpdDrCode = ""; //2019-02-12 박웅규 : 전공의 선택한 의사
        public static string GstrDrCode;
        public static string GstrDrName;
        //public static string  GstrDeptCode;
        public static string GstrLoadDeptCode;
        public static string GstrDeptName;
        public static string GstrGbTrans;
        public static string GstrGbNST;
        public static string GstrGbEMSMS;
        public static string GstrPatSts;       // 컨설트 환자상태  2012-09-05 0:거동가능 1:거동불가능

        public static string GstrDrIdNumber;    //OneTime 처방 의사 사번

        public static string GstrKGubun;       // 컨설트+격리 정보
        public static string GstrKDate1;       // 격리 시작일
        public static string GstrKDate2;       // 격리 종료일

        public static string GstrAntSave;      // 제한항생제 저장체크

        public static string GstrTransDept;
        public static string GstrTransDr;
        public static string GstrTransDrName;
        public static int GnTransRow;
        public static string GstrTransRemark;
        public static string GstrDrugChk;
        public static string GstrResultChk;
        public static string GstrMayakRemark;
        public static string GstrMayakSuCode;
        public static string GstrIllCode;

        public static string GstrAnatCode;
        public static string GstrAnatName;

        public static string GstrConsultROWID;
        public static string GstrConsultAuto;        //2013-08-26
        public static string GstrConsultAuto2;       //2013-11-29

        public static string GstrConsultkekROWID;    //2013-12-10
        public static string GstrConsultkekSTS;      //2013-12-10

        public static string Gstr부가세사용과;       //2014-02-21
        public static string Gstr부가세;             //2014-02-25

        public static string Gstr항생제POP;          //2014-04-17
        public static string Gstr연속오더체크;       //2014-07-22
        public static string GstrOrderTitle;         //2014-08-05
        public static long GnOrderType;              //2014-08-05

        public static string GstrPatientSort;

        public static int GstrIllSort;
        public static int GnIllSort;

        public static string GstrActDate;
        public static string GstrFOrderTime;         // 정규 Order 시작 시간(18:00)
        public static string GstrTOrderTime;         // 정규 Order 종료 시간(01:00)

        //public string[] GstrSETBis = new string[65];
        //public string[] GstrSETChojaes = new string[5];
        //public string[] GstrSETGbSpcs = new string[9];
        //public string[] GstrSETGameks = new string[20];         //감액구분       안내 Tables
        public static string GstrSeluPano;                             //증명서등록번호
        public static string GstrSeluDeptCode;                         //증명서진료과목
        public static string GstrSeluIO;                               //증명서입/외
        public static string GstrSeluDiagnosis;                        //상병

        public int[] GnSELECTSlips = new int[53];               //선택된 Slips(true/false)
        public string[] GstrSELECTSlipsno = new string[53];     // Slip no
        public static int GnSELECTSlipsCurrent;                        // 현재 Order 중인 SLIP Index
        public static string GstrSELECTSlipnos;                        //현재 Order 중인 Slip 번호
        public static string GstrSELECTPxName;                         // 현재의 선택된 약속처방명
        public static string GstrOLDPxName;                            // 현재의 선택된 약속처방명
        public static string GstrSELECTOrderCode;                      //현재 선택한 Order Code
        public static string GstrSELECTRemark;
        public static string GstrSELECTBun;
        public static string GstrSELECTDosCode;
        public static string GstrSELECTSpecCode;                        // OrderCode 의 SpecCode
        public static string GstrSELECTGbDiv;                          // Divide
        public static string GnSELECTDosLabel;                            //하위 항목 수량 일수 적용 유무
        public static bool GnSELECTDosLabelY;
        public static string GstrSELECTDosName;
        public static string GstrSELECTGbInfo;
        public static string GstrSELECTSuCode;
        public static string GstrSELECTIllcode;                        // 선택한 상병코드(Order 화면이 안인 경우 즉 상병 하나만 입력할 경우)
        public static string GstrSELECTPtno;
        public static string GstrSELECTDate;
        public static string GstrSELECTGbImiv;                         //1.골수소견,  2.내시경소견, 3. Biopsy or Cytology 소견
        public static string[] GstrDeptCodes = new string[50];
        //public string[]  GstrWardCodeS[50];
        public static string[] GstrWardCodeSx = new string[50];

        public static int GnGbOrderSave;           //1.Write 2.Write + Print 3.Print
        public static string GnGbOrderSaveYn;           //처방전송시 폼을 닫을 경우 처리하기 위해서 : 2018-11-11 박웅규

        public static int GnReadOrder;             //진료(수납)본 환자의 변경 Order 발생시 기존 Order의 갯수
        public static int GnReadOrder2;            //'일괄후불처리한 Order의 갯수 2012-08-17  
        public static int GnReadIlls;              //진료(수납)본 환자의 변경 Order 발생시 기존 상병의 갯수
        public static int GnDeptSlipno;
        public static int GnJaSlipNo;              //2015-08-21
        public static string GstrTransfor = "";
        public static string GnSort = "";

        public static string GstrCalReturn = "";
        public static Single GnCalReturn;
        public static string GstrCalStatus = "";         // QTY:수량입력, NAL:날수입력
        public static int GnCalLabel;              // 하위 항목 수량 날수 적용 유무
        public static string GstrMachChk = "";           //2013-06-11 마취분입력 여부

        public static string GstrGbViewSlip = "";        // AD, PR, PO Order 구분

        public static string GstrStaff = "";
        public static string GstrStaffName = "";

        public static string GstrOrdersViewOrder = "";
        public static string GstrConsultInpID = "";

        public static string GstrJumin2 = "";
        public static string GstrSysDate_1 = "";

        public static string GstrSabun = "";
        public static string GstrSabunName = "";
        public static long GnMgrNo;

        public static string GstrSucode = "";
        public static string GstrTEST = "False";
        public static string GstrSimCode = "";
        public static string GstrSimFlag = "";
        public static string GstrSimYN = "";

        public static string GstrSubName = "";
        public static string GstrPlusDosName = "";

        public string[] GnChk = new string[82];             //영양실 협진의뢰서(dietorder.bas에 있음)
        public string[] GnRe = new string[71];//영양실 협진의뢰서(dietorder.bas에 있음)

        public static int GnAnnounceGetCount;

        public static long[] GnaAnnounceMgrNOs;
        public static long[] GnaAnnouncePerson;
        public string[] GsaAnnounceMemos;
        public string[] GsaAnnounceDateTime;
        public string[] GsaAnnounceGroup;

        //public static string  GstrPassID;
        public static string GstrPassClass = "";
        public static string GstrPassIDnumber = "";
        //public static string  GstrPassWord;
        public static string GstrPacsSW = "";
        public static string GstrWebPacs = "";
        public static string GstrWebReady = "";

        public static int GnLoadNum;

        public static string GstrDrCode_N = "";          // 컨설트 환자 조회시 사용(로그인 의사코드)


        public static string GstrDoctMsg = "";           // 심사계전달사항입니다.
        public static string GstrDurMsg = "";            // dur 전달사항

        public static string GstrDurCancelGrantNo = "";       //DUR 점검취소 처방전 교부번호
        public static string GstrDurPrscAdmSym = "";          //DUR 점검취소 처방전발행기관기호
        public static bool GstrDurCheckCancel;          //DUR 점검취소 여부
        public static string GstrDurCancelReasonCd = "";      //DUR 점검취소 사유코드
        public static string GstrDurCancelPrscDd = "";        //DUR 점검취소 처방일자
        public static string GstrDurCancelReasonMsg = "";     //DUR 점검취소 사유

        public static string GstrFlag = "";              // nrinfo00.bas에 있는 공용변수입니다. 충돌시 삭제해도 됩니다.
        public static string GstrPoint = "";             // APACHE 점수

        public static string GstrDeptPC_Doct = "";       // 마취의사 체크    

        public static string GstrKekConSultHelpDr = "";      // 대리격립협진 의사명 - 의사명 값있으면 적용됨
        public static string GstrKekConSultHelpDept = "";      // 대리격립협진 과

        //-----마약처방전 변수 선언----------/
        public static string GstrMaBDATE = "";
        public static string GstrMaPtNo = "";
        public static string GstrMaSName = "";
        public static string GstrMaBi = "";
        public static string GstrMaWardCode = "";
        public static string GstrMaRoomCode = "";
        public static string GstrMaEntDate = "";
        public static string GstrMaDeptCode = "";
        public static string GstrMaDRSabun = "";
        public static string GstrMaIO = "";
        public static string GstrMaSuCode = "";
        public static string GstrMaQTY = "";
        public static string GstrMaRowid = "";
        public static double GnMaRealQty;
        public static int GnMaNAL;
        public static string GstrMaDosCode;
        public static string GstrMaRemark1;
        public static string GstrMaRemark2;
        public static string GstrMaSeqNo;
        public static string GstrMaPrint;
        public static double GnMaOrderNo;


        //과거 외래 처방 오더관련 변수-------------
        public static string GstrOldDept;  //원 접수과
        public static string GstrOldDr;  //원 진료의사
        public static string GstrEndo;  //내시경 환자여부
        public static string GstrJaeJin;  //재진갱신여부 2013-03-22
        //--------------------------------------

        //////public GstrSabun ; * 5
        public static string GstrDrCode_Dae;
        public static string GstrDrSabun; //2013-09-03 의사사번

        public static string GstrToday_Ipwon;     //2012-12-18 당일입원체크
        public static string GstrHD처방OK;     //2013-09-14

        public static string GstrDrName_Dae;
        public static string GstrJupsuDept1;
        public static string GstrJupsuDept2;


        public static string GstrSELECTGbSelf;       //비급여 여부표시  
        public static int GnTuyakno;

        public static string GnCalBun;       //현재수가 분류 2013-01-30

        public static string GstrSubRate;
        public static string GstrBunExamInfo;
        public static string GstrOrderExamInfo;
        public static string GstrIngredient;
        //public static string GstrOrderCode;
        public static string GstrBun1;

        public static string GstrPoscoExamPano;       //포스코 결과 등록번호

        public static int GnActiveRow;
        public static string GstrShowDrugFLAG;

        public static string GstrMR고가약;  //2014-02-04
        public static string GstrOCS문자구분;  //2014-02-18

        //public static string GstrDate;
        public static string GstrIllBon; //본인부담 특례대상 여부(상병으로 check) jjy(2003-12-30)추가

        public static string GstrReserved;
        public static string GstrOrderRequest;

        public static string GstrFormCheck;

        public static int GnResvGbn; //예약구분(0.안함 1.10분 2.15분 3.20분 4.30분)
        public static int GnResvInwon; //단위시간당 인원

        public static string GstrSelfTest = "True";
        public static string GstrDoctMsg2; //코니네이트실 메세지 전달
        public static string GstrDoctMsg3; //2013-03-12
        public static string GstrDoctMsg2Rowid; //코니네이트실 메세지 전달 rowid
        public static string GstrDoctMsgTable; //테이블 이름

        public static string GstrViewGbn; //조회구분 2012-06-15
        public static string GstrPtView; //물리치료대상조회구분 2012-07-25

        //////public static string GstrPassWord;
        public static string GstrPassGrade;
        public static string GstrSubClass;
        public static string GstrPassName;
        public static string GstrPassRank;
        public static string GstrPassPart;
        public static string GstrSubPart;
        public static string GstrPassDept;
        public static string GstrIdnumber;

        public static bool GnJinSeqFLAG;


        public static string GstrGBStation;

        //public static string GstrJobMan;  //2014-08-19
        public static string Gstr간호사사용;  //2014-09-11
        public static string Gstr간호사_Sabun;  //2014-09-11
        public static string Gstr간호사_DeptCode;  //2014-09-11
        public static string Gstr간호사_DrCode;  //2014-09-11

        public static string Gstr간호사_USabun;  //2014-09-11
        public static string Gstr간호사_UName;  //2014-09-11

        public static string GstrHD환자구분;  //2014-03-11
        public static int Gn오전오후구분; //2014-12-10
        public static int Gn환자대기new; //2015-05-12
        public static string Gstr자동조회;  //2015-05-21

        public static int GnDurCnt;
        //----------------------------------------------------

        public static string[] GstrMCode;     // 저함량 코드
        public static string[] GstrMName;     // 저함량 코드명
        public static double[] GnDivQty;         // 저함량 누적 1회 투약량 // 일별 투적 투약량
        public static double[] GnUnit;           // 배수( 1 회 투약량)
        public static string[] GstrMSubCode;  // 고함량 코드
        public static string[] GstrMSubName;  // 고함량 코드명
        public static int GnDIVCnt;           // 저함량 등록 코드 갯수

        public static string GstrSpecNm;
        public static string GstrSpecCd;
        public static string GstrSelSendDeptOrder;
        public static string GstrSelSendDeptPrint;

        public static string GstrMaxDate;     //외래환자 선택에서 사용(2017.08.28)
        public static int GnDiagnoCnt;        //처방조회에서 사용(기 진단 카운트)  

        public static string GstrDupOrderOK;  //처방선택시 중복처방 체크로직

        public static int GnOrderCount;       //약속처방에서 사용(기처방 RowCount)
        public static string GstrRtnValue;

        public static string GstrCDSSYN;    //CDSS 이용 처방 선택 유무

        //public string DurClient2 = new HiraDur.IHIRAClient;
        //public string DurPrescription2 = new HiraDur.IHIRAPrescription;
        //public string DurResultSet2 = new HiraDur.IHIRAResultSet;


        //public string DurClient3 = new HiraDur.IHIRAClient;
        //public string DurPrescription3 = new HiraDur.IHIRAPrescription;
        //public string DurResultSet3 = new HiraDur.IHIRAResultSet;

        //##############################################################
        //clsiorderProcessB 전역변수 이동
        public static int GstrStaffListIndex;

        public static object GstrObjectOp;
        public static string GstrSelectPRN;

        public static string GstrGbOrderPRM;
        public static int GnOpIllsIndex;

        public static string GstrDept;

        //public static string GstrSubRate; // OCS_OrderCode의 SubRate

        public static string GstrConsultOrd;   // 기타 Consult Order
        public static string GstrConsultChk;
        public static string GstrDischargeChk;
        public static string GstrOutDate;
        public static string GstrOutDept;
        public static int GnOpNoteIndex;
        public static int GnIllIndex;

        public static string GstrVerbalDrCode;
        public static string GstrSchedual;
        public static string GstrPickUp;
        public static string GstrNoMessage;
        public static string GstrNoMessage1;
        public static string GstrOrderTimeChk;

        //public static int GnActiveRow;

        public static string l_str;  // 한글자르기
        public static int l_Len;

        public static string GstrOrderName;
        public static string GstrOrderCode;
        //public static string GstrBun1;

        public static string GstrCheckJin;

        public static string GstrUpInCheck;
        public static int GnIndex;
        public static string GstrDeptCode1;
        public static string GstrDate;
        public static string GstrDrCode1;
        public static string GstrDrName1;
        public static string GstrJinIll;
        public static string GstrJinName;
        public static string GstrOpIll;
        public static string GstrOpIllName;
        public static string GGbMagam;

        public static string GstrTFlag;
        public static string GstrOrdDis;
        public static string GstrRepeat;

        public static int GnAnHH;
        public static int GnAnMi;
        public static string GstrAnNgt;
        //##############################################################


        //################################################################//
        // 개인별 환경설정 변수
        //################################################################//
        public static string GEnvSet_Item01 = string.Empty;    //환자리스트 선택(1:외래, 2:응급, 3:입원)
        public static string GEnvSet_Item02 = string.Empty;    //처방판넬 선택(1:개인, 2:과, 3:전체) 
        public static string GEnvSet_Item03 = string.Empty;
        public static string GEnvSet_Item04 = string.Empty;
        public static string GEnvSet_Item05 = string.Empty;
        public static string GEnvSet_Item06 = string.Empty;
        public static string GEnvSet_Item07 = string.Empty;
        public static string GEnvSet_Item08 = string.Empty;
        public static string GEnvSet_Item09 = string.Empty;
        public static string GEnvSet_Item10 = string.Empty;
        public static string GEnvSet_Item11 = string.Empty;
        public static string GEnvSet_Item12 = string.Empty;
        public static string GEnvSet_Item13 = string.Empty;
        public static string GEnvSet_Item14 = string.Empty;
        public static string GEnvSet_Item15 = string.Empty;
        public static string GEnvSet_Item16 = string.Empty;
        public static string GEnvSet_Item17 = string.Empty;
        public static string GEnvSet_Item18 = string.Empty;
        public static string GEnvSet_Item19 = string.Empty;
        public static string GEnvSet_Item20 = string.Empty;
        public static string GEnvSet_Item21 = string.Empty;
        public static string GEnvSet_Item22 = string.Empty;

        public static string GEnvSet_Item51 = string.Empty;
        public static string GEnvSet_Item52 = string.Empty;
        public static string GEnvSet_Item53 = string.Empty;
        public static string GEnvSet_Item54 = string.Empty;
        public static string GEnvSet_Item55 = string.Empty;
        public static string GEnvSet_Item56 = string.Empty;
        public static string GEnvSet_Item57 = string.Empty;
        public static string GEnvSet_Item58 = string.Empty;
        public static string GEnvSet_Item59 = string.Empty;
        public static string GEnvSet_Item60 = string.Empty;
        public static string GEnvSet_Item61 = string.Empty;
        public static string GEnvSet_Item62 = string.Empty;
        public static string GEnvSet_Item63 = string.Empty;
        public static string GEnvSet_Item64 = string.Empty;
        public static string GEnvSet_Item65 = string.Empty;
        public static string GEnvSet_Item66 = string.Empty;
        public static string GEnvSet_Item67 = string.Empty;
        public static string GEnvSet_Item68 = string.Empty;
        public static string GEnvSet_Item69 = string.Empty;
        public static string GEnvSet_Item70 = string.Empty;
        //################################################################//

        public static string GstrRDate;
        public static string GstrRTime;
        public static string GstrRDrCode;
        public static string GstrResMemo;

        //2020-08-19 추가
        public static string GstrCTChange; 

        public struct Select_Ptno
        {
            public long IPDNO;
            public string PtNo;
            public string sName;
            public string JUMIN;
            public string Birth;
            public int Age;
            public int AgeDays;
            public string Sex;
            public string GbSpc;
            public string DeptCode;
            public string DrCode;
            public string Staffid;
            public string Bi;
            public string BiName;
            public string INDATE;
            public string InTime;
            public string IpwonTime;
            public string RDATE;
            public string RTime;
            public string RDrCode;
            public string Remark1;// 골수검사 임상소견
            public string Remark2;//골수검사 주요병력
            public string Remark3;// 내시경
            public string Remark4;// Biopsy Or Cytology
            public string Tel;
            public string AmSet1;
            public string AmSet3;
            public string AmSet7;// 입원경로
            public string EntDate;
            public string WardCode;
            public string RoomCode;
            public string DrName;
            public string GbGamek;
            public string GBSTS;
            public long TRSNO;
            public string OutDate;

            //--------CP 오더 관련변수
            public string CPCode;
            public int CPday;
            public string CPName;
            public string CPRepeat;
            public string Pregnant;
            public string GbIO;         //I:입원, O:외래, E:응급실

            public string JTime;
            public string GbChojae;
            public string GbSheet;
            public string VCode;     //Y.암등록환자/N.암미등록환자
            public string Exam;      //Y.검사결과확인예약
            public string PNEUMONIA; //페렴
            public string MCODE;     //희귀난치성등록자구분
            public string Thyroid;   //갑상선여부
            public string res;       //내시경 예약
            public string Insulin;   //인슐린투여환자
            public string MCODE_OPD; //차상위 희귀
            public string VCODE_OPD;
            public string bunup;     //분업 
            public string ResMemo;
            public string ResSMSNot; //2016-06-16(예약문자 형성 안함)

            public string Address;  //주소
            public string GamF;     //직원 감액 정보

            // 응급실 Eorder.bas
            public string InDate;
            public string BDate;
            public string RDate;
            public string Pneumonia;       //폐렴
            public string ErPatient;       //응급중증 뇌질환표시
            public string Jumin;
            public string Mst_ROWID;       //접수mst rowid 
            public string ODrCode;

            public string KIHO;     //기관기호
            public string GKIHO;    //개인기호

            public string KTASLEVL;
            public string OPDNO;

            public string WEIGHT;
            public string HEIGHT;
            public string REMARK;
            public string PATINFO;

        }
        public static Select_Ptno Pat;

        public struct PAT_INFO
        {
            public double WRTNO;
            public double IPDNO;
            public string Pano;
            public string sName;
            public string Sex;
            public int Age;
            public string INDATE;
            public string DeptCode;
            public string DrCode;
            public string WardCode;
            public string RoomCode;
            public string RDATE;
            public string DIAGNOSIS;
            public string BDate;
            public string DRSABUN;
            public string NRSABUN;
            public string PMSABUN;
            public string DTSABUN;
            public string ORDERNO;
            public string ROWID;
            public string OPDNO;
        }

        public static PAT_INFO PATi;

        //처방조제 지원 시스템---------------------------------------------------
        public struct Select_Dur
        {
            public string Gubun; //DUR 구분
            public string SuCode_A;
            public double Qty_A;
            public double Nal_A;
            public double Div_A;
            public string SuCode_B;
            public double Qty_B;
            public double Nal_B;
            public double Div_B;
        }
        public static Select_Dur[] Dur = new Select_Dur[200];


        public static string[] Pat_IllCode;


        //<항생제>--------------------------------------------
        public static string GstrANTI_ROWID;
        public static string GstrState;

        //<영양검색>------------------------------------------ 2009-01-20 김현욱

        public struct Diet_Food_Search
        {
            public string New;
            public string Pano;
            public string sName;
            public string RoomCode;
            public string IpwonDay;
            public string Sex;
            public int Age;
            public string DIAGNOSIS;
            public string DeptCode;
            public string DietName;
            public string Height;
            public string Weight;
            public string HWeight;
            public string IBW;
            public string LabALB;
            public string LabTLC;
            public string LabHB;
            public string LabTcho;
            public string Cnt;
            public string DrCode;
            public string WardCode;
            public string sDate;  //환자관리 등록일자.
            public long IPDNO;
            public string Warning;
            public string UDATE;
        }
        public static Diet_Food_Search dst;

        public long nHeight;    //화면 사이즈 로그인할때
        public static string GstrDept_M;       //응급실에서 외래 진료과선택
        public static string GstrDrCode_M;       //응급실에서 외래 의사선택
        public static string Gstr약속처방DrCode;  //2014-03-19

        public static int GnViewPanoGbn; //1.진료중 2.응급실입원 3.퇴원자        
        //2016-08-26 계장 김현욱 추가
        //GnviewPanoGbn = 6 일 경우 환자 목록이 활성화 될 때 GnviewPanoGbn2 값으로 치환해줌
        public static int GnViewPanoGbn2;

        public static string GstrSelSlipno;         //처방선택(2017.04.18) 
        //======================================//
        // 환경설정 변수
        //======================================//
        //public static string Set_OrderName;
        //public static string Set_PrevOrderView;
        //public static string Set_AutouSearch;
        //public static string Set_LastOrderSet;
        //public static string Set_ErNextReceipt;
        //public static string Set_EmrOrderDisp;
        //public static string Set_XrayLisResult;
        //public static string Set_OrderNurseUse;
        //public static string Set_ContentsUse;
        //public static string Set_DosQtyUse;
        //public static string Set_DosFormLoad;
        //public static string Set_ColorRemark;
        //public static string Set_PersonOrder;
        //public static string Set_AutoDiag;
        //public static string Set_DIAGNAME;
        //public static string Set_NIP;
        //public static string Set_AdminDiagnosis;

        //public static string Set_WardCode;
        //======================================//

        public static string GstrOrderSelect;       //오더코드 선택시 일괄입력 : ORD,  OR 더블클릭(개별입력) : SELORD
        public static string GstrSelOrderCode;
        public static string GstrselOrderCode;
        public static string GstrSelPlusName;
        public static double GnQty;
        public static int GnDiv;
        public static int GnNal;

        public static string GstrNameEng;
        public static string GstrORDERNAMEK;
        public static string GstrSelSClass;         //분류
        public static string GstrSpecGubun;         //검체선택구분

        public static string GstrSendDeptOrder;     //전송부서

        public static string strOrderDate;

        //public static string GstrOrderCode;
        public static int GnSunapOrdCount;          //수납/진행된 처방카운터
        public static int GnJinOrdCount;            //진료(처방)여부 전회처방 Display 여부 판단(기존 GnReadOrder = 9999 / GnReadOrder = 0)

        public static string GstrPlusName;          //오더코드 일괄 입력시 용법란에 ※추가정보선택요망 문구 Dislplay 위함.

        public static string GstrYakGubun;          //2020-04-22, 약처방 체크 위함
        public static void Init_Select_Ptno()
        {
            clsOrdFunction.Pat.IPDNO = 0;
            clsOrdFunction.Pat.PtNo = "";
            clsOrdFunction.Pat.sName = "";
            clsOrdFunction.Pat.JUMIN = "";
            clsOrdFunction.Pat.Birth = "";
            clsOrdFunction.Pat.Age = 0;
            clsOrdFunction.Pat.AgeDays = 0;
            clsOrdFunction.Pat.Sex = "";
            clsOrdFunction.Pat.GbSpc = "";
            clsOrdFunction.Pat.DeptCode = "";
            clsOrdFunction.Pat.DrCode = "";
            clsOrdFunction.Pat.Staffid = "";
            clsOrdFunction.Pat.Bi = "";
            clsOrdFunction.Pat.BiName = "";
            clsOrdFunction.Pat.INDATE = "";
            clsOrdFunction.Pat.InTime = "";
            clsOrdFunction.Pat.IpwonTime = "";
            clsOrdFunction.Pat.RDATE = "";
            clsOrdFunction.Pat.RTime = "";
            clsOrdFunction.Pat.RDrCode = "";
            clsOrdFunction.Pat.Remark1 = "";    // 골수검사 임상소견
            clsOrdFunction.Pat.Remark2 = "";    //골수검사 주요병력
            clsOrdFunction.Pat.Remark3 = "";    // 내시경
            clsOrdFunction.Pat.Remark4 = "";    // Biopsy Or Cytology
            clsOrdFunction.Pat.Tel = "";
            clsOrdFunction.Pat.AmSet1 = "";
            clsOrdFunction.Pat.AmSet3 = "";
            clsOrdFunction.Pat.AmSet7 = "";     // 입원경로
            clsOrdFunction.Pat.EntDate = "";
            clsOrdFunction.Pat.WardCode = "";
            clsOrdFunction.Pat.RoomCode = "";
            clsOrdFunction.Pat.DrName = "";
            clsOrdFunction.Pat.GbGamek = "";
            clsOrdFunction.Pat.GBSTS = "";
            clsOrdFunction.Pat.TRSNO = 0;
            clsOrdFunction.Pat.OutDate = "";

            //--------CP 오더 관련변수
            clsOrdFunction.Pat.CPCode = "";
            clsOrdFunction.Pat.CPday = 0;
            clsOrdFunction.Pat.CPName = "";
            clsOrdFunction.Pat.CPRepeat = "";
            clsOrdFunction.Pat.Pregnant = "";
            clsOrdFunction.Pat.GbIO = "";         //I:입원, O:외래, E:응급실
            clsOrdFunction.Pat.JTime = "";
            clsOrdFunction.Pat.GbChojae = "";
            clsOrdFunction.Pat.GbSheet = "";
            clsOrdFunction.Pat.VCode = "";     //Y.암등록환자/N.암미등록환자
            clsOrdFunction.Pat.Exam = "";      //Y.검사결과확인예약
            clsOrdFunction.Pat.PNEUMONIA = ""; //페렴
            clsOrdFunction.Pat.MCODE = "";     //희귀난치성등록자구분
            clsOrdFunction.Pat.Thyroid = "";   //갑상선여부
            clsOrdFunction.Pat.res = "";       //내시경 예약
            clsOrdFunction.Pat.Insulin = "";   //인슐린투여환자
            clsOrdFunction.Pat.MCODE_OPD = ""; //차상위 희귀
            clsOrdFunction.Pat.VCODE_OPD = "";
            clsOrdFunction.Pat.bunup = "";     //분업 
            clsOrdFunction.Pat.ResMemo = "";
            clsOrdFunction.Pat.ResSMSNot = ""; //2016-06-16(예약문자 형성 안함)
            clsOrdFunction.Pat.Address = "";  //주소
            clsOrdFunction.Pat.GamF = "";     //직원 감액 정보

            // 응급실 Eorder.bas
            clsOrdFunction.Pat.InDate = "";
            clsOrdFunction.Pat.BDate = "";
            clsOrdFunction.Pat.RDate = "";
            clsOrdFunction.Pat.Pneumonia = "";       //폐렴
            clsOrdFunction.Pat.ErPatient = "";       //응급중증 뇌질환표시
            clsOrdFunction.Pat.Jumin = "";
            clsOrdFunction.Pat.Mst_ROWID = "";       //접수mst rowid 
            clsOrdFunction.Pat.ODrCode = "";
            clsOrdFunction.Pat.KIHO = "";     //기관기호
            clsOrdFunction.Pat.GKIHO = "";    //개인기호
            clsOrdFunction.Pat.KTASLEVL = "";
            clsOrdFunction.Pat.OPDNO = "";
            clsOrdFunction.Pat.WEIGHT = "";
            clsOrdFunction.Pat.HEIGHT = "";
            clsOrdFunction.Pat.REMARK = "";
            clsOrdFunction.Pat.PATINFO = "";
        }


        /// <summary>
        /// 심사팀에서 오더 판넬쪽에 수가변환 매핑 하였을때 해당 수가로 반환해주는 함수입니다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ORDERCODE">오더코드</param>
        /// <param name="SUCODE">수가코드</param>
        /// <param name="BDATE">처방일자</param>
        /// <param name="DEPTCODE">환자 접수된 과</param>
        /// <returns></returns>
        public string Mapping_SuCode(PsmhDb pDbCon, string ORDERCODE, string SUCODE, string GBINFO, string BDATE, string DEPTCODE)
        {
            string rtnVal = string.Empty;
            OracleDataReader reader = null;
            string SQL = string.Empty;
            

            if (ORDERCODE.IsNullOrEmpty() || SUCODE.IsNullOrEmpty())
            {
                return rtnVal;
            }

            ORDERCODE = ORDERCODE.PadRight(12, ' ');
            SUCODE = SUCODE.PadRight(12, ' ');

            try
            {
                #region 쿼리
                SQL = "SELECT ADMIN.FC_SUGA_MAPPING('" + ORDERCODE + "', " + "'" + SUCODE + "'" + ", '" + GBINFO + "'" + ", '" + BDATE + "'"+ ", '" + DEPTCODE + "')       \r";
                SQL += " FROM DUAL                                                                                                                       \r";
                #endregion

                string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr.NotEmpty())
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }
                else
                {
                    rtnVal = SUCODE;
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return rtnVal;
            }
        }

        /// <summary>
        ///  2021-07-12 외래, ER 경유 입원 당일날 중복검사 관련 팝업
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ORDERCODE"></param>
        /// <param name="MSG"></param>
        /// <returns></returns>
        public bool EXAM_DUAL_CHK(PsmhDb pDbCon, string ORDERCODE, ref string MSG)
        {
            //DateTime dtpSysDate = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();
            bool rtnVal = true;

            //선택날짜가 입원일일때만
            if (GstrBDate.To<DateTime>().Date != Pat.INDATE.To<DateTime>().Date)
            {
                return rtnVal;
            }

            OracleDataReader reader = null;
            string SQL = string.Empty;

            try
            {
                #region 쿼리
                SQL = "SELECT '---------------중복 검사 발생 알림---------------\r\n' || A.SEQNO || '번째 위치에 \r\n' || (SELECT TRIM(ORDERNAME) || '(' || TRIM(ORDERCODE) || ')' FROM ADMIN.OCS_ORDERCODE WHERE ORDERCODE = A.ORDERCODE) || '\r\n처방이 이미 있습니다.\r\n확인해주세요!' AS MSG                        \r";
                SQL += "  FROM ADMIN.OCS_IORDER A                                                                                                                                                                                                                                                              \r";
                SQL += " WHERE BDATE = TO_DATE('" + GstrBDate + "', 'YYYY-MM-DD')                                                                                                                                                                                                                                    \r";
                SQL += "   AND TRIM(SUCODE) IN                                                                                                                                                                                                                                                                      \r";
                SQL += "    (                                                                                                                                                                                                                                                                                       \r";
                SQL += "    SELECT CODE                                                                                                                                                                                                                                                                             \r";
                SQL += "      FROM ADMIN.BAS_BCODE bb                                                                                                                                                                                                                                                         \r";
                SQL += "     WHERE GUBUN = 'OCS_중복검사차단코드'                                                                                                                                                                                                                                                      \r";
                SQL += "    )                                                                                                                                                                                                                                                                                       \r";
                SQL += "   AND PTNO      = '" + clsOrdFunction.Pat.PtNo + "'                                                                                                                                                                                                                                        \r";
                SQL += "   AND ORDERCODE = '" + ORDERCODE + "'                                                                                                                                                                                                                                                      \r";
                SQL += "   AND ORDERSITE IN('OPD', 'OPDX', 'ERO')                                                                                                                                                                                                                                                   \r";
                SQL += " GROUP BY A.SEQNO, A.ORDERNO, A.ORDERCODE                                                                                                                                                                                                                                                   \r";
                SQL += " HAVING SUM(QTY * NAL) > 0                                                                                                                                                                                                                                                                  \r";
                #endregion

                string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr.NotEmpty())
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    MSG = reader.GetValue(0).ToString().Trim();
                    rtnVal = false;
                }

                reader.Dispose();
                reader = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return rtnVal;
            }

            return rtnVal;
        }


        /// <summary>
        /// CHECK_OORDER_IN
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgOrderCode"></param>
        /// <param name="argPTNO"></param>
        /// <returns></returns>
        public string CHECK_OORDER_IN(PsmhDb pDbCon, string ArgOrderCode, string argPTNO)
        {
            if (ArgOrderCode == "S/O")
            {
                return "";
            }

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            long rowcounter;            

            try
            {
                SQL = "";
                SQL += " SELECT '1' GBN, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,DEPTCODE             \r";
                SQL += "   FROM " + ComNum.DB_MED + "OCS_OORDER                                 \r";
                SQL += "  WHERE PTNO = '" + argPTNO + "'                                        \r";
                SQL += "    AND BDATE >=TRUNC(SYSDATE-1)                                        \r";
                SQL += "   AND ORDERCODE = '" + ArgOrderCode + "'                               \r";                
                //SQL += "    AND GBSUNAP =  '1'                                                  \r"; //수납처리된 것만 조회
                //add 2013-03-21
                SQL += "  UNION ALL                                                             \r";
                SQL += " SELECT /*+ index( " + ComNum.DB_MED + "ocs_iorder INXOCS_IORDER1) */   \r";
                SQL += "       '2' GBN, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,DEPTCODE              \r";
                SQL += "   FROM " + ComNum.DB_MED + "OCS_IORDER                                 \r";
                SQL += "  WHERE PTNO = '" + argPTNO + "'                                        \r";
                SQL += "     AND BDATE >= TRUNC(SYSDATE-1)                                      \r";
                SQL += "    AND ORDERCODE = '" + ArgOrderCode + "'                              \r";
                SQL += "     AND GbStatus IN  (' ')                                             \r";                
                SQL += "   GROUP BY TO_CHAR(BDATE,'YYYY-MM-DD') ,DEPTCODE                       \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "CHECK_OORDER_IN " + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");

                    return "OK";
                }
                rowcounter = dt.Rows.Count;

                int i = 0;

                if (rowcounter > 0)
                {
                    clsPublic.GstrMsgList = "<<당일 혹은 전일 동일처방이 있습니다,날짜와 오더를 확인 하십시오>" + "\r\r";

                    for (int nchk = 0; nchk < rowcounter; nchk++)
                    {
                        clsPublic.GstrMsgList += nchk + 1 + ".이미 " + (dt.Rows[i]["GBN"].ToString().Trim()) == "1" ? "외래" : "입원(ER포함)" + " OCS에서 " +
                                          dt.Rows[i]["BDATE"].ToString().Trim() + "  " + dt.Rows[i]["DEPTCODE"].ToString().Trim() +
                                          " 과에서 Order Code가 있습니다" + "\r\r";
                        i++;
                    }

                    clsPublic.GstrMsgList += "[처방 조회 및 검사 결과 확인] 창에서 확인 바랍니다." + "\r\r";

                    clsPublic.GstrMsgList += "추가를 하시겠습니까 ??" + "\r\r";

                    if (MessageBox.Show(clsPublic.GstrMsgList, "중복처방사용여부", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        dt.Dispose();
                        dt = null;
                        return "NO";
                    }
                    else
                    {
                        dt.Dispose();
                        dt = null;
                        return "OK";
                    }
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    return "OK";
                }
            }

            catch (OracleException ex)
            {
                ComFunc.MsgBox("함수명 : " + "CHECK_OORDER_IN " + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "CHECK_OORDER_IN " + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }
        }

        public string CHECK_OORDER_IN2(PsmhDb pDbCon, string ArgOrderCode, string argPTNO, FarPoint.Win.Spread.FpSpread ArgSS, int argROW, string ArgChk)
        {
            int nnx = 0;
            int nny = 0;
            string strChk1 = "";
            string strChk2 = "";
            string strRtn = "";
            string strMsg = "";

            strRtn = "OK";

            //오더내 체크
            if (ArgChk == "1")
            {
                for (int i = 0; i < ArgSS.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (i != argROW)
                    {
                        if (ArgSS.ActiveSheet.Cells[argROW, 0].Text != "True")
                        {
                            if (ArgSS.ActiveSheet.Cells[argROW, 1].Text == ArgOrderCode.Trim())
                            {
                                strChk1 = "1";
                                strMsg = "<<현재오더화면에 [" + ArgOrderCode + "] 오더가 이미 있습니다,확인 하십시오>" + "\r\n\r\n";
                                strMsg += "추가를 하시겠습니까 ??" + "\r\n\r\n";
                                if (MessageBox.Show(strMsg, "추가확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                                {
                                    strRtn = "NO";
                                }
                                break;
                            }
                        }
                    }
                }
            }

            if (strChk1 == "1") return strRtn;

            //try
            //{
            //    SQL = "";
            //    SQL += " SELECT DEPTCODE, ROWID                     \r";
            //    SQL += "   FROM ADMIN.OCS_OORDER               \r";
            //    SQL += "  WHERE PTNO = '" + argPTNO + "'            \r";
            //    //SQL += "    AND BDATE = TRUNC(SYSDATE)              \r";
            //    SQL += "    AND BDATE = to_date('" + clsOrdFunction.GstrBDate + "', 'yyyy-mm-dd')   \r";
            //    SQL += "    AND ORDERCODE = '" + ArgOrderCode + "'  \r";
            //    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
            //    if (SqlErr != "")
            //    {
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //        return strRtn;
            //    }

            //    if (dt.Rows.Count > 0)
            //    {   
            //        strMsg = "<<당일 외래처방이 있습니다,확인 하십시오>" + "\r\n\r\n";
            //        strMsg += "이미 외래 OCS에서 " + dt.Rows[0]["DEPTCODE"].ToString().Trim() + "" + " 과에서 " + "\r\n\r\n" + "[" + ArgOrderCode + "] Order Code가 있습니다" + "\r\n\r\n";
            //        strMsg += "추가를 하시겠습니까 ??" + "\r\n\r\n";
            //        if (MessageBox.Show(strMsg, "추가확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //        {
            //            strRtn = "NO";
            //        }
            //    }

            //    dt.Dispose();
            //    dt = null;
            //}
            //catch (Exception ex)
            //{
            //    ComFunc.MsgBox(ex.Message);
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            //}

            return strRtn;
        }

        public string READ_MAYAK(PsmhDb pDbCon, string ArgSuCode)
        {
            string strValue = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt1 = null;

            try
            {
                //마약은 처방소견반드시 등록
                SQL = "";
                SQL += " SELECT JEPCODE                                 \r";
                SQL += "   FROM ADMIN.DRUG_JEP                     \r";
                SQL += "  WHERE JEPCODE = '" + ArgSuCode.Trim() + "'    \r";
                SQL += "    AND CHENGGU = '09'                          \r";
                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strValue;
                }
                if (dt1.Rows.Count == 0)
                {
                    dt1.Dispose();
                    dt1 = null;
                    return strValue;
                }

                if (dt1.Rows.Count > 0)
                {
                    strValue = "*";
                }

                dt1.Dispose();
                dt1 = null;

                return strValue;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strValue;
            }
        }

        public void READ_DUR_MULTI(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            long rowcounter;

            try
            {
                SQL = "";
                SQL += " SELECT /*+rule*/ D.SUNEXT SUNEXTA, B.PNAME PNAMEA      \r";
                SQL += "      , A.UNIT_CNT, E.SUNEXT SUNEXTB, C.PNAME PNAMEB    \r";
                SQL += "  FROM " + ComNum.DB_PMPA + "HIRA_TBJBD52 A             \r";
                SQL += "     , " + ComNum.DB_PMPA + "EDI_SUGA     B             \r";
                SQL += "     , " + ComNum.DB_PMPA + "EDI_SUGA     C             \r";
                SQL += "     , " + ComNum.DB_PMPA + "BAS_SUN      D             \r";
                SQL += "     , " + ComNum.DB_PMPA + "BAS_SUN      E             \r";
                SQL += " WHERE A.LOW_IQTY_MEDC_CD = B.CODE                      \r";
                SQL += "   AND A.HIGH_IQTY_MEDC_CD = C.CODE                     \r";
                SQL += "   AND A.LOW_IQTY_MEDC_CD = D.BCODE                     \r";
                SQL += "   AND A.HIGH_IQTY_MEDC_CD = E.BCODE(+)                 \r";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }
                rowcounter = dt.Rows.Count;

                GnDIVCnt = (int)rowcounter;

                Array.Resize(ref GstrMCode, GnDIVCnt);
                Array.Resize(ref GstrMName, GnDIVCnt);
                Array.Resize(ref GnDivQty, GnDIVCnt);
                Array.Resize(ref GnUnit, GnDIVCnt);
                Array.Resize(ref GstrMSubCode, GnDIVCnt);
                Array.Resize(ref GstrMSubName, GnDIVCnt);

                for (int i = 0; i < GnDIVCnt; i++)
                {
                    GstrMCode[i] = "";
                    GstrMName[i] = "";
                    GnDivQty[i] = 0;
                    GnUnit[i] = 0;
                    GstrMSubCode[i] = "";
                    GstrMSubName[i] = "";
                }

                for (int i = 0; i < rowcounter; i++)
                {
                    GstrMCode[i] = dt.Rows[i]["SUNEXTA"].ToString().Trim();
                    GstrMName[i] = dt.Rows[i]["PNAMEA"].ToString().Trim(); //Trim(Rsdur!PNAMEA + "")
                    GstrMSubCode[i] = dt.Rows[i]["SUNEXTB"].ToString().Trim(); //Trim(Rsdur!SUNEXTB + "")
                    GstrMSubName[i] = dt.Rows[i]["PNAMEB"].ToString().Trim(); //Trim(Rsdur!PNAMEB + "")
                    GnUnit[i] = Convert.ToInt32(VB.Val(dt.Rows[i]["UNIT_CNT"].ToString().Trim()));  //Val(Trim(Rsdur!UNIT_CNT + ""))
                }
                dt.Dispose();
                dt = null;
            }
            catch (OracleException OE)
            {
                ComFunc.MsgBox("함수명 : " + "READ_DUR_MULTI" + ComNum.VBLF + OE.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "READ_DUR_MULTI" + ComNum.VBLF + OE.Message, SQL, pDbCon); //에러로그 저장
            }
        }

        public string READ_POWDER(PsmhDb pDbCon, string ArgSuCode)
        {
            string strValue = "N";
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT POWDER                                      \r";
                SQL += "   FROM " + ComNum.DB_MED + "OCS_DRUGINFO_new       \r";
                SQL += "  WHERE SUNEXT = '" + ArgSuCode + "'                \r";
                SQL += "    AND POWDER = '1'                                \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strValue;
                }

                if (reader.HasRows)
                {
                    strValue = "Y";
                }

                reader.Dispose();
                reader = null;
                return strValue;

            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strValue;
            }
        }

        /// <summary>
        /// 소아 진정 가능 처방일 경우
        /// 2018-12-28 박웅규
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSuCode"></param>
        /// <returns></returns>
        public string READ_SEDATION(PsmhDb pDbCon, string ArgCode)
        {
            string strValue = "N";

            if (string.Compare(clsPublic.GstrSysDate, "2019-01-01") < 0) return strValue;

            string SQL = "";
            string SqlErr = "";            
            long rowcounter;

            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT CODE                                      \r";
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_BCODE       \r";
                SQL += "  WHERE GUBUN = 'DRUG_소아진정수가코드'                \r";
                SQL += "    AND CODE  = '" + ArgCode + "'                \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strValue;
                }

                if (reader.HasRows)
                {
                    strValue = "Y";
                }

                reader.Dispose();
                reader = null;

                return strValue;

            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strValue;
            }
        }

        /// <summary>
        /// 진정관리 코드를 배열에 저장한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        public string[] READ_SEDATION_ARRY(PsmhDb pDbCon)
        {
            string[] strValue = null;

            if (string.Compare(clsPublic.GstrSysDate, "2019-01-01") < 0) return strValue;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            long rowcounter;

            try
            {
                SQL = "";
                SQL += " SELECT CODE                                      \r";
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_BCODE       \r";
                SQL += "  WHERE GUBUN = 'DRUG_소아진정수가코드'                \r";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strValue;
                }

                if (dt.Rows.Count > 0)
                {
                    strValue = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strValue[i] = dt.Rows[i]["CODE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                return strValue;

            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strValue;
            }
        }

        /// <summary>
        /// Vb_OCS_Order1.bas 이동
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSuCode"></param>
        /// <returns></returns>
        public string READ_POWDER_NO_USE_CHK(PsmhDb pDbCon, string ArgSuCode)
        {
            string strValue = "NO";
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT ROWID                                       \r";
                SQL += "   FROM " + ComNum.DB_MED + "OCS_DRUGINFO_new       \r";
                SQL += "  WHERE SUNEXT = '" + ArgSuCode + "'                \r";
                SQL += "    AND POWDER = '0'                                \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strValue;
                }

                if (reader.HasRows)
                {
                    strValue = "OK";
                }

                reader.Dispose();
                reader = null;

                return strValue;

            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strValue;
            }
        }

        public string READ_EMR(PsmhDb pDbCon, string ArgPano)
        {
            string strValue = "";
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT PATID                                 \r";
                SQL += "   FROM " + ComNum.DB_EMR + "EMR_TREATT       \r";
                SQL += "  WHERE PATID   = '" + ArgPano + "'           \r";
                SQL += "    AND CLASS   ='O'                          \r";
                SQL += "    AND CHECKED ='1'                          \r";
                SQL += "    AND EMR     ='1'                          \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strValue;
                }

                if (reader.HasRows)
                {
                    strValue = "S";
                }

                reader.Dispose();
                reader = null;

                return strValue;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strValue;
            }
        }

        //public string Select_Return(string sObject)
        //{
        //    int nCnt = 0;
        //    string sText = "";
        //    for (int i = 0; i < rowcounter; i++)
        //    {
        //        switch (sObject.Substring(nCnt, 1))
        //        {
        //            case "\r": sText += " "; break;
        //            default: sText += sObject.Substring(nCnt, 1); break;
        //        }
        //    }

        //    return sText;
        //}

        public string READ_DRUG_TRANS(PsmhDb pDbCon, string ArgSuCode)
        {
            //string strTemp = "";
            string strJDate = "";
            string strSUCODENEW = "";
            string strSayu = "";
            string strValue = "";

            if (ArgSuCode == "") return "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            long rowcounter;

            try
            {
                SQL = "";
                SQL += " SELECT SUCODEOLD, SUCODENEW, GBN, TO_CHAR(JDATE,'YYYY-MM-DD') JDATE    \r";
                SQL += "   FROM " + ComNum.DB_MED + "OCS_DRUG_TRANS                             \r";
                SQL += "  WHERE SUCODEOLD = '" + ArgSuCode + "'                                \r";
                SQL += "    AND DELDATE IS NULL                                                 \r";
                SQL += "  ORDER BY JDATE DESC                                                   \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strValue;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return strValue;
                }

                rowcounter = 0;
                rowcounter = dt.Rows.Count;

                if (rowcounter > 0)
                {
                    switch (dt.Rows[0]["GBN"].ToString().Trim())
                    {
                        case "1": strSayu = "대체"; break;
                        case "2": strSayu = "변경"; break;
                        case "3": strSayu = "사용중지"; break;
                        case "4": strSayu = "생산중단"; break;
                        case "5": strSayu = "코드삭제"; break;
                        case "6": strSayu = "원외처방전용으로 변경"; break;
                        case "7": strSayu = "소모부진"; break;
                        case "8": strSayu = "장기품절"; break;
                        default: strSayu = ""; break;
                    }

                    strJDate = dt.Rows[0]["JDATE"].ToString().Trim();
                    strSUCODENEW = dt.Rows[0]["SUCODENEW"].ToString().Trim();
                    strValue = "◈ 이 약품은 ( " + strSayu + " ) 되어 ";
                    strValue += strJDate.Substring(0, 4) + "년 " + VB.Format(strJDate.Substring(6, 2), "0") + "월 " +
                               VB.Format(VB.Right(strJDate, 2), "0") + "일 부터 사용이 불가합니다." + "\r";
                    strValue += "대체사용 가능약품은 ( 수가코드 : " + strSUCODENEW + " ) 입니다. (문의 : 약제과 8051)";

                    dt.Dispose();
                    dt = null;

                    return strValue;
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    return strValue;
                }
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strValue;
            }
        }

        public void Verbal_Remark(FarPoint.Win.Spread.FpSpread SpdNm, string sGBIO, long argROW, ComboBox cboBox) 
        {
            if (sGBIO == "ER")
            {
                if (clsOrdFunction.GstrVerbalDrCode != "")
                {
                    if (clsPublic.Gstr구두Chk == "OK")
                    {
                        if (clsPublic.GstrJobMan == "간호사" && clsPublic.Gstr간호처방STS == "구두처방")
                        {
                            SpdNm.ActiveSheet.Cells[(int)argROW, 20].Text += "   " + ComFunc.LeftH(cboBox.Text, 10).Trim() + " (구두처방)";
                        }
                    }
                    else
                    {
                        if (clsPublic.GstrJobMan != "의사")
                        {
                            SpdNm.ActiveSheet.Cells[(int)argROW, 20].Text += "   " + ComFunc.LeftH(cboBox.Text, 10).Trim() + " (Verbal)";
                        }
                    }
                }
            }
            else if (sGBIO == "IPD")
            {
                if (clsOrdFunction.GstrVerbalDrCode != null && clsOrdFunction.GstrVerbalDrCode != "")
                {
                    if (clsPublic.Gstr구두Chk == "OK")
                    {
                        SpdNm.ActiveSheet.Cells[(int)argROW, 21].Text = SpdNm.ActiveSheet.Cells[(int)argROW, 21].Text + "   " + ComFunc.LeftH(cboBox.Text, 10).Trim() + " (구두처방)";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[(int)argROW, 21].Text = SpdNm.ActiveSheet.Cells[(int)argROW, 21].Text + "   " + ComFunc.LeftH(cboBox.Text, 10).Trim() + " (Verbal)";
                    }
                }

                if (SpdNm.ActiveSheet.Cells[(int)argROW, 16].Text == "0005")
                {
                    if (VB.Left(SpdNm.ActiveSheet.Cells[(int)argROW, 34].Text, 2).Trim() == "W-")
                    {
                        if (SpdNm.ActiveSheet.Cells[(int)argROW, 12].Text == "") //Remark 여부
                        {
                            SpdNm.ActiveSheet.Cells[(int)argROW, 12].Text = "#";
                        }
                        SpdNm.ActiveSheet.Cells[(int)argROW, 21].Text += " " + "AST";
                    }
                }

                string SQL = "";
                string SqlErr = "";
                DataTable dt = null;

                //마약은 처방소견반드시 등록
                try
                {
                    SQL = "";
                    SQL += " SELECT JEPCODE                                         \r";
                    SQL += "   FROM ADMIN.DRUG_JEP                             \r";
                    SQL += "  WHERE JEPCODE =  '" + SpdNm.ActiveSheet.Cells[(int)argROW, 14].Text + "'    \r";
                    SQL += "    AND CHENGGU = '09'                                  \r";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 오류가 발생했습니다");
                        clsDB.SaveSqlErrLog("함수명 : " + "Verbal_Remark " + ComNum.VBLF + SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        //마약
                        GstrMayakFlag = "OK";
                        SpdNm.ActiveSheet.Cells[(int)argROW, 21].Text = "";
                    }
                    else
                    {
                        GstrMayakFlag = "";
                    }

                    dt.Dispose();
                    dt = null;
                }
                catch (Exception e)
                {
                    MessageBox.Show("함수명 : " + "Verbal_Remark " + ComNum.VBLF + e.Message);
                    return;
                }
            }
        }

        public static string READ_BONE_Result_Check(PsmhDb pDbCon, string sPtno, string sCode)
        {
            OracleDataReader reader = null;

            DataTable dt1 = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            GstrDoctMsg = "";

            try
            {
                SQL = "";
                SQL += " SELECT GBBONE                              \r";
                SQL += "   FROM ADMIN.BAS_SUN                 \r";
                SQL += "  WHERE  sunext = '" + sCode.Trim() + "'    \r";
                SQL += "    AND  GBBONE = 'Y'                       \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (reader.HasRows)
                {
                    SQL = "";
                    SQL += " SELECT MAX(TO_CHAR(A.ENTERDATE,'YYYY-MM-DD')) ENTERDATE, A.EXINFO  \r";
                    SQL += "  , (SELECT RESULT FROM ADMIN.XRAY_RESULTNEW WHERE WRTNO = A.EXINFO ) AS RESULT       \r";
                    SQL += "   FROM ADMIN.XRAY_DETAIL A                               \r";
                    SQL += "  WHERE A.PANO = '" + sPtno + "'                                  \r";
                    SQL += "    AND A.XCODE IN ('HC341','HC342','HC342A')                     \r";
                    SQL += "    AND A.EXINFO <> '1'                                           \r";
                    SQL += "  GROUP BY A.EXINFO                                               \r";
                    SQL += "  ORDER BY ENTERDATE DESC                                         \r";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 오류가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return "";
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        if (dt1.Rows[0]["RESULT"].ToString().Trim().NotEmpty())
                        {
                            GstrDoctMsg = "<골다공증 검사결과>" + "\r\n\r\n" + "최근검사일:" + dt1.Rows[0]["ENTERDATE"].ToString() + " 결과값:" + dt1.Rows[0]["RESULT"].ToString();
                        }

                        //SQL = "";
                        //SQL += " SELECT RESULT                                              \r";
                        //SQL += "   FROM ADMIN.XRAY_RESULTNEW                          \r";
                        //SQL += "  WHERE WRTNO = " + dt1.Rows[0]["EXINFO"].ToString() + "    \r";
                        //SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, pDbCon);

                        //if (SqlErr != "")
                        //{
                        //    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                        //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        //    return "";
                        //}

                        //if (dt2.Rows.Count > 0)
                        //{
                        //GstrDoctMsg = "<골다공증 검사결과>" + "\r\n\r\n" + "최근검사일:" + dt1.Rows[0]["ENTERDATE"].ToString() + " 결과값:" + dt2.Rows[0]["RESULT"].ToString();
                        //}
                        //dt2.Dispose();
                        //dt2 = null;
                    }

                    dt1.Dispose();
                    dt1 = null;
                }

                reader.Dispose();
                reader = null;
                return GstrDoctMsg;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }
        }

        /// <summary>
        /// CHK_OCS_진료의사본과접수 함수를 SelfOrder_Check() 으로 변경
        /// </summary>
        /// <param name="sPtno"></param>
        /// <param name="sCode"></param>
        /// <returns></returns>
        public static bool SelfOrder_Check(PsmhDb pDbCon, string sPano, string sDrCode)
        {
            OracleDataReader reader = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            bool blnRtn = false;

            try
            {
                SQL = "";
                SQL += " SELECT c.Pano                          \r";
                SQL += "   From ADMIN.BAS_DOCTOR a        \r";
                SQL += "      , ADMIN.OCS_DOCTOR  b        \r";
                SQL += "      , ADMIN.INSA_MST    c        \r";
                SQL += "  Where a.DrCode = '" + sDrCode + "'    \r";
                SQL += "    AND a.DrCode = b.DrCode(+)          \r";
                SQL += "    AND b.Sabun = c.Sabun               \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return blnRtn;
                }

                if (reader.HasRows && reader.Read() && reader.GetValue(0).ToString().Trim().Equals(sPano))
                {
                    blnRtn = true;
                }

                reader.Dispose();
                reader = null;
                return blnRtn;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return blnRtn;
            }
        }

        /// <summary>
        /// 제한항생제 승인권한
        /// <param name="sCode1"></param>
        /// <param name="sCode2">
        /// 2017.09
        /// 제한항생제승인체크.bas\READ_제한항생제_사용권한
        /// <returns></returns>
        /// </summary>
        public static string Read_AntiMed_Approval_Authority(PsmhDb pDbCon, string sCode1, string sCode2)
        {
            OracleDataReader reader = null;
            //DataTable dt = null;
            //DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strRtn = "";

            try
            {
                #region 이전 로직 주석
                //SQL = "";
                //SQL += " SELECT CODE                                \r";
                //SQL += "   FROM ADMIN.BAS_BCODE               \r";
                //SQL += "  WHERE GUBUN ='OCS_제한항생제승인관리'         \r";
                //SQL += "    AND TRIM(CODE) = '" + sCode1 + "'       \r";
                //SQL += "    AND (DELDATE IS NULL OR DELDATE = '')   \r";
                //SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                //if (SqlErr != "")
                //{
                //    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                //    return strRtn;
                //}

                //if (dt.Rows.Count > 0)
                //{
                //    //strRrn = "OK";
                //    SQL = "";
                //    SQL += " SELECT CODE                                            \r";
                //    SQL += "   FROM ADMIN.BAS_BCODE                           \r";
                //    SQL += "  WHERE GUBUN = 'OCS_제한항생제승인관리'                \r";
                //    SQL += "    AND TRIM(CODE) = '" + sCode2.ToString().Trim() + "' \r";
                //    SQL += "    AND (DELDATE IS NULL OR DELDATE = '')               \r";
                //    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, pDbCon);

                //    if (SqlErr != "")
                //    {
                //        ComFunc.MsgBox("조회중 오류가 발생했습니다");
                //        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                //        return strRtn;
                //    }

                //    if (dt1.Rows.Count > 0)
                //    {
                //        strRtn = "OK";
                //    }
                //    dt1.Dispose();
                //    dt1 = null;
                //}
                //dt.Dispose();
                //dt = null;

                #endregion

                #region 신규 로직
                SQL = "";
                SQL += " SELECT 1                                   \r";
                SQL += "   FROM DUAL                                \r";
                SQL += "  WHERE EXISTS                              \r";
                SQL += "  (                                         \r";
                SQL += " SELECT CODE                                \r";
                SQL += "   FROM ADMIN.BAS_BCODE               \r";
                SQL += "  WHERE GUBUN ='OCS_제한항생제승인관리'         \r";
                SQL += "    AND TRIM(CODE) = '" + sCode1 + "'       \r";
                SQL += "    AND (DELDATE IS NULL OR DELDATE = '')   \r";
                SQL += "  )                                         \r";
                SQL += "    AND EXISTS                              \r";
                SQL += "  (                                         \r";
                SQL += " SELECT CODE                                \r";
                SQL += "   FROM ADMIN.BAS_BCODE               \r";
                SQL += "  WHERE GUBUN ='OCS_제한항생제승인관리'        \r";
                SQL += "    AND TRIM(CODE) = '" + sCode2 + "'       \r";
                SQL += "    AND (DELDATE IS NULL OR DELDATE = '')   \r";
                SQL += "  )                                         \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strRtn;
                }

                if (reader.HasRows)
                {
                    strRtn = "OK";
                }
                reader.Dispose();
                reader = null;

                #endregion

                if (sCode2.ToString().Trim() == "35104" || sCode2.ToString().Trim() == "38732" || sCode2.ToString().Trim() == "44346" || sCode2.ToString().Trim() == "48087")
                {
                    strRtn = "OK";
                }
                if (sCode1 == "192.168.2.77")
                {
                    strRtn = "OK";
                }

                return strRtn;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strRtn;
            }
        }
        /// <summary>
        /// VbOcs_Res.bas 파일의 READ_DEPT_CHOJEA2 함수를 Read_Dept_Chojae() 으로 변경
        /// </summary>
        /// <param name="sPano"></param>
        /// <param name="sDeptCode"></param>
        /// <param name="sDrCode"></param>
        /// <returns></returns>
        public static string Read_Dept_Chojae(PsmhDb pDbCon, string sPano, string sDeptCode, string sDrCode)
        {
            OracleDataReader reader = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strRtn = "";

            try
            {
                SQL = "";
                SQL += " SELECT ROWID                                   \r";
                SQL += "   FROM ADMIN.OPD_MASTER                  \r";
                SQL += "  WHERE PANO = '" + sPano + "'                  \r";
                SQL += "    AND DEPTCODE = '" + sDeptCode + "'          \r";
                SQL += "    AND ACTDATE < TRUNC(SYSDATE)                \r";
                                                                        
                if (sDeptCode == "MD")                                  
                {                                                       
                    if (sDrCode == "1107")                              
                    {                                                   
                        SQL += "    AND DRCODE = '1107'                 \r";
                    }                                                   
                    else if (sDrCode == "1125")                         
                    {                                                   
                        SQL += "    AND DRCODE = '1125'                 \r";
                    }
                    else
                    {   
                        SQL += "    AND DRCODE NOT IN ('1107','1125')   \r";
                    }
                }

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strRtn;
                }

                if (reader.HasRows)
                {
                    strRtn = "";
                }
                else
                {
                    strRtn = "과초진";
                }

                reader.Dispose();
                reader = null;
                return strRtn;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strRtn;
            }
        }

        /// <summary>
        /// VbOcs_Res.bas 파일의 RES_ORDER_INSERT 함수를 Read_Dept_Chojae() 으로 변경
        /// 예약오더 생성 루틴
        /// </summary>
        /// <param name="sPano"></param>
        /// <param name="sDeptCode"></param>
        /// <param name="sDrCode"></param>
        /// <returns></returns>
        public static string Res_Order_Insert(PsmhDb pDbCon, string sPano, string sDeptCode, string sDrCode, string sRDate, string sRTime,
                                              string sBi, string sExam, string sRemark, string sRowId, string sResSMSNot)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strRtn = "OK";
            int intRowAffected = 0; //변경된 Row 받는 변수

            long nOorderNo = 0;
            string strBDATE;
            string strResSMSNot;

            ComFunc.ReadSysDate(clsDB.DbCon);

            if (sRowId.NotEmpty())
            {
                try
                {

                    //예약변경
                    SQL = "";
                    SQL += " SELECT BDATE                           \r";
                    SQL += "   FROM ADMIN.OCS_OORDER           \r";
                    SQL += "  WHERE ROWID = '" + sRowId + "'        \r";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 오류가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return "NO";
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strBDATE = dt.Rows[0]["BDATE"].ToString();
                    }
                    else
                    {
                        strBDATE = clsPublic.GstrSysDate;
                    }
                    dt.Dispose();
                    dt = null;

                    clsBagage.RES_SMS_NOTSEND_PROCESS(clsDB.DbCon, sPano, strBDATE, sDeptCode, sRDate, sRTime, sDrCode, "UPDATE", sResSMSNot, "OK");

                    clsDB.setBeginTran(clsDB.DbCon);

                    try
                    {
                        SQL = "";
                        SQL += " UPDATE ADMIN.OCS_OORDER           \r";
                        SQL += "    SET OrderCode =  '" + sRDate + "'   \r";
                        SQL += "      , SuCode    = '" + sRTime + "'    \r";
                        SQL += "      , DrCode    = '" + sDrCode + "'   \r";
                        SQL += "      , GbInfo    = '" + sExam + "'     \r";
                        SQL += "      , Remark    = '" + sRemark + "'   \r";
                        SQL += "      , EntDate   = SYSDATE             \r";
                        SQL += " WHERE ROWID      ='" + sRowId + "'     \r";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return "";
                        }

                        clsDB.setCommitTran(pDbCon);
                        return strRtn;
                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                        return "NO";
                    }
                }

                catch (OracleException ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                    return "NO";
                }
            }
            else
            {
                try
                {
                    //예약변경
                    SQL = "";
                    SQL += " SELECT SEQ_ORDERNO.NextVal nNEXTVAL FROM DUAL \r";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 오류가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return "NO";
                    }

                    if (dt.Rows.Count > 0)
                    {
                        nOorderNo = long.Parse(dt.Rows[0]["nNEXTVAL"].ToString());
                    }
                    else
                    {
                        strBDATE = clsPublic.GstrSysDate;
                    }
                    dt.Dispose();
                    dt = null;

                    clsBagage.RES_SMS_NOTSEND_PROCESS(clsDB.DbCon, sPano, clsPublic.GstrSysDate, sDeptCode, sRDate, sRTime, sDrCode, "INSERT", sResSMSNot, "OK");

                    clsDB.setBeginTran(clsDB.DbCon);

                    try
                    {
                        SQL = "";
                        SQL += " INSERT INTO ADMIN.OCS_OORDER                                                          \r";
                        SQL += "       (Ptno, BDate, DeptCode, SeqNo, OrderCode, SuCode, Bun, SlipNo, RealQty               \r";
                        SQL += "     , Qty, Nal, GbDiv, DosCode,   GbBoth, GbInfo, GbEr, GbSelf, GbSpc, Bi, DrCode          \r";
                        SQL += "     , EntDate,Remark,  GbSunap, TuyakNo, OrderNo, Multi, MultiRemark                       \r";
                        SQL += "     , DUR, RESV, ScodeSayu, ScodeRemark , RES, GBSPC_NO, GBAUTOSEND, OCSDRUG,GbFM          \r";
                        SQL += "     , Sabun, GbTax, CORDERCODE, CSUCODE, CBUN)                                             \r";
                        SQL += " VALUES                                                                                     \r";
                        SQL += "      ('" + sPano + "', TRUNC(SYSDATE), '" + sDeptCode + "', '0', '" + sRDate.Replace("-", "") + "'\r";
                        SQL += "     , '" + sRTime + "', '', '0000',  '',  0, 0, 1                                          \r";
                        SQL += "     , '', '', '" + sExam + "', '', '', ' ', '" + sBi + "', '" + sDrCode + "'               \r";
                        SQL += "     , SYSDATE,  '" + sRemark + "','0',  '0',  " + nOorderNo + ",  ''                       \r";
                        SQL += "     , '' , '' ,'' , '','' ,'' , '' ,'' , '','','',''                                       \r";
                        SQL += "     , '" + sRDate + "','" + sRTime + "','')                                                \r";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return "NO";
                        }

                        clsDB.setCommitTran(pDbCon);
                        return strRtn;
                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                        return "NO";
                    }
                }

                catch (OracleException ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                    return "NO";
                }
            }
        }

        /// <summary>
        /// SimSaGiJun_Check
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="sSimsaFlag"></param>
        /// <param name="sSimCode"></param>
        /// <returns></returns>
        public static string SimSaGiJun_Check(PsmhDb pDbCon, string sSimsaFlag, string sSimCode, string GBIO)
        {
            OracleDataReader reader = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strRtn = "";

            if (clsOrdFunction.GstrTEST == "False") return strRtn;
            if (sSimsaFlag.Trim() == "Y") return strRtn;
            if (sSimCode == null || sSimCode.Trim() == "") return strRtn;

            try
            {
                SQL = "";
                SQL += " SELECT SUCODE                              \r";
                SQL += "   FROM ADMIN.JSIM_GIJUN              \r";
                SQL += "  WHERE SUCODE = '" + sSimCode.Trim() + "'  \r";
                SQL += "    AND (DISPLAY <> 'I' OR DISPLAY IS NULL) \r";
                if (GBIO == "IPD")
                {
                    SQL += "    AND BCHECK ='Y' \r";
                }
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strRtn;
                }

                if (reader.HasRows && reader.Read())
                {
                    clsOrdFunction.GstrSucode = reader.GetValue(0).ToString().Trim();
                    strRtn = "Y";
                }

                reader.Dispose();
                reader = null;
                return strRtn;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strRtn;
            }
        }

        /// <summary>
        /// 특수문자나 숫자만 있는경우 체크
        /// </summary>
        /// <param name="Ctl"></param>
        /// <returns></returns>
        public static string fn_SpecialCharCheck(string strText)
        {
            string strRtn = "NO";

            for (int i = 0; i < strText.Length; i++)
            {
                if ((VB.Asc(VB.Mid(strText, i, 1)) >= 65 && VB.Asc(VB.Mid(strText, i, 1)) <= 90) ||
                    (VB.Asc(VB.Mid(strText, i, 1)) >= 97 && VB.Asc(VB.Mid(strText, i, 1)) <= 122))
                {
                    strRtn = "OK";
                    break;
                }
            }

            //2020-07-23 안정수, 한글로 입력하는 경우도 있다고하여 한글입력인경우도 넘어가도록 보완 
            if(Regex.IsMatch(strText, @"[ㄱ-ㅎ|ㅏ-ㅣ|가-힣]") == true)
            {
                strRtn = "OK";
            }

            return strRtn;
        }

        /// <summary>
        /// 진단명 읽어오기
        /// </summary>
        /// <param name="sPtno"></param>
        /// <param name="sBDate"></param>
        /// <returns></returns>
        public static string Read_illName(string sPtno, string sBDate, string sDept)
        {
            OracleDataReader reader = null;
            string SQL = "";
            string SqlErr = "";     //에러문 받는 변수

            string strRtn = "";

            try
            {
                SQL = "";
                SQL += " SELECT nvl(b.illnamee, b.illnamek) illname             \r";
                SQL += "   FROM ADMIN.OCS_OILLS a                          \r";
                SQL += "      , ADMIN.BAS_ILLS b                          \r";
                SQL += "  WHERE PTNO = '" + sPtno + "'                          \r";
                SQL += "    AND BDATE = to_date('" + sBDate + "', 'yyyy-mm-dd') \r";
                SQL += "    AND DEPTCODE = '" + sDept + "'                      \r";
                SQL += "    AND SEQNO = 1                                       \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }

                if (reader.HasRows && reader.Read())
                {
                    strRtn = reader.GetValue(0).ToString().Trim();
                }
                else
                {
                    strRtn = "";
                }

                reader.Dispose();
                reader = null;
                return strRtn;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strRtn;
            }
        }

        public static string SlipNo_Gubun(string strSlipNo, string strDosCode, string strBun)
        {
            string rtnVal = "";

            switch (strSlipNo.Trim())
            {
                case "0003":
                case "0004":
                    //rtnVal = "Med      999";
                    rtnVal = "Med       15";
                    break;
                case "0005":
                    if (strDosCode == "97" || strDosCode == "99")
                    {
                        //rtnVal = "Med      23";
                        rtnVal = "Med      15";
                    }
                    else
                    {
                        //rtnVal = "Med      24";
                        rtnVal = "Med      15";
                    }
                    break;
                case "0010":
                case "0011":
                case "0012":
                case "0013":
                case "0014":
                case "0015":
                case "0016":
                case "0017":
                case "0018":
                case "0019":
                case "0020":
                case "0021":
                case "0022":
                case "0023":
                case "0024":
                case "0025":
                case "0026":
                case "0027":
                case "0028":
                case "0029":
                case "0030":
                case "0031":
                case "0032":
                case "0033":
                case "0034":
                case "0035":
                case "0036":
                case "0037":
                case "0038":
                case "0039":
                case "0040":
                case "0041":
                case "0042":
                    //rtnVal = "Lab      17";
                    rtnVal = "Lab      24";
                    break;
                case "0060":
                case "0061":
                case "0062":
                case "0063":
                case "0064":
                case "0065":
                case "0067":
                case "0069":
                case "0070":
                case "0071":
                case "0072":
                case "0073":
                case "0074":
                case "0075":
                case "0076":
                case "0077":
                case "0078":
                case "0079":
                case "0080":
                    //rtnVal = "Xray     14";
                    rtnVal = "Xray     21";
                    break;
                case "0066":
                    //rtnVal = "RI       15";
                    rtnVal = "RI       22";
                    break;
                case "0068":
                    //rtnVal = "Sono     16";
                    rtnVal = "Sono     23";
                    break;
                case "A1":
                    rtnVal = "V/S      11";
                    break;
                case "A2":
                    rtnVal = "S/O      12";
                    break;
                case "A4":
                    rtnVal = "S/O      13";
                    break;
                case "OR1":
                case "OR2":
                    rtnVal = "OR       22";
                    break;
                case "0044":
                    if (strBun.Trim() == "78")
                    {
                        //rtnVal = "Bmd      19";
                        rtnVal = "Bmd      26";
                    }
                    else
                    {
                        //rtnVal = "Endo     18";
                        rtnVal = "Endo     25";
                    }
                    break;
                case "TEL":
                    //rtnVal = "TEL      21";
                    rtnVal = "TEL      28";
                    break;
                case "0106":
                    //rtnVal = "JAGA     25";
                    rtnVal = "JAGA     18";
                    break;
                default:
                    //rtnVal = "Etc      20";
                    rtnVal = "Etc      27";
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// <seealso cref="vbfunc.bas : READ_OCS_Doctor_2DrName"/>
        /// </summary>
        /// <param name="ArgDrCode"></param>
        /// <returns></returns>
        public static string READ_OCS_Doctor_2DrName(PsmhDb pDbCon, string ArgDrCode)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + " DRNAME";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_DOCTOR";
            SQL += ComNum.VBLF + " WHERE Sabun='" + ArgDrCode + "'";

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return "";
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();
            reader = null;
            return rtnVal;
        }

        public static string Read_Drug_SuCode(PsmhDb pDbCon, string strSuCode)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            SQL = "";
            SQL += " SELECT a.ROWID                                                                         \r";
            SQL += "   FROM ADMIN.OCS_DRUGINFO_NEW A                                                   \r";
            SQL += "      , ADMIN.DRUG_MASTER2     B                                                   \r";
            SQL += "      , ADMIN.DRUG_JEP         C                                                   \r";
            SQL += "  WHERE A.SUNEXT = B.JEPCODE(+)                                                         \r";
            SQL += "    AND A.SUNEXT = C.JEPCODE                                                            \r";
            SQL += "    AND a.SuNext = '" + strSuCode.Trim() + "'                                           \r";
            SQL += "    AND C.CHENGGU IN ('09','08')                                                        \r";
            SQL += "    AND EXISTS (                                                                        \r";
            SQL += "                SELECT 'X' FROM ADMIN.BAS_SUT S                                   \r";
            SQL += "                 WHERE DELDATE Is Null                                                  \r";
            SQL += "                   AND (S.SUGBJ IN ('2','3','4') OR (S.BUN = '23' AND S.SUGBJ = '0'))   \r";
            SQL += "                   AND A.SUNEXT = S.SUNEXT)                                             \r";
            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return "";
            }

            if (reader.HasRows)
            {
                rtnVal = "OK";
            }
            else
            {
                rtnVal = "";
            }

            reader.Dispose();
            reader = null;
            return rtnVal;
        }

        public void INSERT_SVIP_MST(string sPano, string sSName, string sRemark, string sYear)
        {
            string strRowId = "";

            string SQL = "";
            string sSQL = "";
            DataTable dt = null;
            string SqlErr = "";     //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                SQL = "";
                SQL += " SELECT ROWID                               \r";
                SQL += "   FROM ADMIN.BAS_SVIP_MST            \r";
                SQL += "  WHERE PANO ='" + sPano + "'               \r";
                SQL += "    AND (DELDATE IS NULL OR DELDATE ='')    \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strRowId = dt.Rows[0]["ROWID"].ToString().Trim();

                    if (strRowId == "")
                    {

                        clsDB.setBeginTran(clsDB.DbCon);

                        try
                        {
                            sSQL = "";
                            sSQL += " INSERT INTO ADMIN.BAS_SVIP_MS                                    \r";
                            sSQL += "        (PANO, SNAME, YEAR, JUMSU, GUBUN, ENTDATE, ENTDATE2, ENTSABUN)  \r";
                            sSQL += "  VALUES \r";
                            sSQL += "        ('" + sPano + "', '" + sSName + "', '" + sYear + "', 0, '01'    \r";
                            sSQL += "        , SYSDATE,SYSDATE, " + clsType.User.Sabun + ")                  \r";
                            SqlErr = clsDB.ExecuteNonQuery(sSQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, sSQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            clsDB.setCommitTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                        }
                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, sSQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += " UPDATE ADMIN.BAS_PATIENT    \r";
                SQL += "    SET GB_SVIP ='Y'               \r";
                SQL += "  WHERE PANO = '" + sPano + "'     \r";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                MessageBox.Show("감사고객추천 등록완료!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        public string fn_Read_Bas_Jumin2(string sPano)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += " SELECT JUMIN1 ,JUMIN2 ,JUMIN3  \r";
                SQL += "   FROM ADMIN.BAS_PATIENT \r";
                SQL += "  WHERE PANO = '" + sPano + "'  \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["JUMIN3"].ToString().NotEmpty())
                    {
                        rtnVal = dt.Rows[0]["JUMIN1"].ToString() + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString());
                    }
                    else
                    {
                        rtnVal = dt.Rows[0]["Jumin1"].ToString() + dt.Rows[0]["JUMIN2"].ToString();
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            return rtnVal;
        }

        public string fn_READ_OCS_Doctor_DrBunho(string ArgDrCode)
        {
            int i = 0;
            string rtnVal = "";

            OracleDataReader reader = null;
            string SQL = "";
            string SqlErr = "";

            if (ArgDrCode == "")
            {
                return rtnVal;
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT                                                           ";
            SQL = SQL + ComNum.VBLF + " DrBunho                                                         ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_DOCTOR                            ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1                                                        ";
            SQL = SQL + ComNum.VBLF + "  AND DrCode='" + ArgDrCode + "'                                 ";

            try
            {
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }

                if (reader.HasRows)
                {
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            reader.Dispose();
            reader = null;

            return rtnVal;
        }

        /// <summary>
        /// MedOrder\class\clsOcsSMS.cs 삭제하고 이동
        /// </summary>
        /// <param name="sIO"></param>
        /// <param name="sGubun"></param>
        /// <param name="sPano"></param>
        /// <param name="sBDate"></param>
        /// <param name="sDeptCode"></param>
        public void fn_Read_OCS_SMS_MSG2(string sIO, string sGubun, string sPano, string sBDate, string sDeptCode)
        {
            string strToDate;
            string strToDate2;
            string strBDATE;
            string strBDate2;
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            ComFunc.ReadSysDate(clsDB.DbCon);

            strToDate = clsPublic.GstrSysDate;
            strToDate2 = DateTime.Parse(strToDate).AddDays(30).ToString();

            strBDATE = sBDate.Substring(0, 8) + "01";
            strBDate2 = clsVbfunc.LastDay(int.Parse(strBDATE.Substring(0, 4)), int.Parse(strBDATE.Substring(5, 2)));

            if (sIO == "외래")
            {
                switch (sGubun)
                {
                    case "001":
                        break;
                    case "002":
                        try
                        {
                            SQL = "";
                            SQL += " SELECT PANO,SNAME,GUBUN,TO_CHAR(RTIME,'YYYY-MM-DD HH24:MI') RTIME,SENDMSG      \r";
                            SQL += "   FROM ADMIN.ETC_SMS                                                     \r";
                            SQL += "  WHERE Pano = '" + sPano + "'                                                  \r";
                            SQL += "    AND (                                                                       \r";
                            SQL += "        (JOBDATE >=TO_DATE('" + strToDate + "','YYYY-MM-DD')                    \r";
                            SQL += "         AND JOBDATE <=TO_DATE('" + DateTime.Parse(strToDate2).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ) \r";
                            SQL += "     OR (JOBDATE >=TO_DATE('" + strBDATE + "','YYYY-MM-DD')                     \r";
                            SQL += "         AND JOBDATE <=TO_DATE('" + DateTime.Parse(strToDate2).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ) \r";
                            SQL += "        )                                                                       \r";
                            SQL += "   AND Gubun IN ('51')                                                          \r";
                            SQL += "   AND DeptCode ='" + sDeptCode + "'                                            \r";

                            SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            }

                            if (reader.HasRows)
                            {
                                MessageBox.Show("귀하의 혈액검사,소변검사" + "\r\n\r\n" + "정기검진 할 시기가 다가옵니다..", "정기검사확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            reader.Dispose();
                            reader = null;
                        }
                        catch (OracleException ex)
                        {
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        }
                        break;
                    case "003":
                        try
                        {
                            SQL = "";
                            SQL += " SELECT PANO,SNAME,GUBUN,TO_CHAR(RTIME,'YYYY-MM-DD HH24:MI') RTIME,SENDMSG      \r";
                            SQL += "   FROM ADMIN.ETC_SMS                                                     \r";
                            SQL += "  WHERE Pano = '" + sPano + "'                                                  \r";
                            SQL += "    AND (                                                                       \r";
                            SQL += "        (JOBDATE >=TO_DATE('" + strToDate + "','YYYY-MM-DD')                    \r";
                            SQL += "         AND JOBDATE <=TO_DATE('" + DateTime.Parse(strToDate2).AddDays(1).ToString() + "','YYYY-MM-DD') ) \r";
                            SQL += "     OR (JOBDATE >=TO_DATE('" + strBDATE + "','YYYY-MM-DD')                     \r";
                            SQL += "         AND JOBDATE <=TO_DATE('" + DateTime.Parse(strToDate2).AddDays(1).ToString() + "','YYYY-MM-DD') ) \r";
                            SQL += "        )                                                                       \r";
                            SQL += "   AND Gubun IN ('52')                                                          \r";
                            SQL += "   AND DeptCode ='" + sDeptCode + "'                                            \r";

                            SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            }

                            if (reader.HasRows)
                            {
                                MessageBox.Show("귀하의 안과검사" + "\r\n\r\n" + "정기검진 할 시기가 다가옵니다..", "정기검사확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            reader.Dispose();
                            reader = null;
                        }
                        catch (OracleException ex)
                        {
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        }
                        break;
                    case "004":
                        try
                        {
                            SQL = "";
                            SQL += " SELECT PANO,SNAME,GUBUN,TO_CHAR(RTIME,'YYYY-MM-DD HH24:MI') RTIME,SENDMSG      \r";
                            SQL += "   FROM ADMIN.ETC_SMS                                                     \r";
                            SQL += "  WHERE Pano = '" + sPano + "'                                                  \r";
                            SQL += "    AND (                                                                       \r";
                            SQL += "        (JOBDATE >=TO_DATE('" + strToDate + "','YYYY-MM-DD')                    \r";
                            SQL += "         AND JOBDATE <=TO_DATE('" + DateTime.Parse(strToDate2).AddDays(1).ToString() + "','YYYY-MM-DD') ) \r";
                            SQL += "     OR (JOBDATE >=TO_DATE('" + strBDATE + "','YYYY-MM-DD')                     \r";
                            SQL += "         AND JOBDATE <=TO_DATE('" + DateTime.Parse(strToDate2).AddDays(1).ToString() + "','YYYY-MM-DD') ) \r";
                            SQL += "        )                                                                       \r";
                            SQL += "   AND Gubun IN ('59')                                                          \r";
                            SQL += "   AND DeptCode ='" + sDeptCode + "'                                            \r";

                            SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            }

                            if (reader.HasRows)
                            {
                                MessageBox.Show("귀하의 골다공증검사" + "\r\n\r\n" + "정기검진 할 시기가 다가옵니다..", "정기검사확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            reader.Dispose();
                            reader = null;
                        }
                        catch (OracleException ex)
                        {
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        }
                        break;
                    case "005":
                        try
                        {
                            SQL = "";
                            SQL += " SELECT PANO,SNAME,GUBUN,TO_CHAR(RTIME,'YYYY-MM-DD HH24:MI') RTIME,SENDMSG      \r";
                            SQL += "   FROM ADMIN.ETC_SMS                                                     \r";
                            SQL += "  WHERE Pano = '" + sPano + "'                                                  \r";
                            SQL += "    AND (                                                                       \r";
                            SQL += "        (JOBDATE >=TO_DATE('" + strToDate + "','YYYY-MM-DD')                    \r";
                            SQL += "         AND JOBDATE <=TO_DATE('" + DateTime.Parse(strToDate2).AddDays(1).ToString() + "','YYYY-MM-DD') ) \r";
                            SQL += "     OR (JOBDATE >=TO_DATE('" + strBDATE + "','YYYY-MM-DD')                     \r";
                            SQL += "         AND JOBDATE <=TO_DATE('" + DateTime.Parse(strToDate2).AddDays(1).ToString() + "','YYYY-MM-DD') ) \r";
                            SQL += "        )                                                                       \r";
                            SQL += "   AND Gubun IN ('60')                                                          \r";
                            SQL += "   AND DeptCode ='" + sDeptCode + "'                                            \r";

                            SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            }

                            if (reader.HasRows)
                            {
                                MessageBox.Show("귀하의 치매검사" + "\r\n\r\n" + "정기검진 할 시기가 다가옵니다..", "정기검사확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            reader.Dispose();
                            reader = null;
                        }
                        catch (OracleException ex)
                        {
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (sIO == "입원")
            {
                switch (sGubun)
                {
                    case "001":
                        break;
                    case "002":
                        break;
                    case "003":
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 파우더 가능한지 체크
        /// clsOrderEtc.CHK_POWDER_SUCODE_CHK 와 동시에 변경 필수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strSuCode"></param>
        /// <returns></returns>
        public static string Read_Powder_SuCode_New(PsmhDb pDbCon, string strSuCode)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            try
            {
                //산재불가약 체크후 -체크박스 표시
                SQL = "";
                SQL += " SELECT NOT_POWDER, NOT_POWDER_SUB              \r";
                SQL += "   FROM ADMIN.DRUG_MASTER4                 \r";
                SQL += "  WHERE JepCode = '" + strSuCode.Trim() + "'    \r";
                SQL += "    AND NOT_POWDER = '1'                        \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    reader.Dispose();
                    reader = null;
                    rtnVal = "";
                    return rtnVal;
                }

                reader.Dispose();
                reader = null;

                SQL = "";
                SQL += " SELECT ROWID                                   \r";
                SQL += "   FROM ADMIN.DRUG_MASTER1                 \r";
                SQL += "  WHERE JepCode = '" + strSuCode.Trim() + "'    \r";
                //전산업무의뢰서 2020-1621 포장단위 = 'T' -> 수가단위 'T' 변경
                //SQL += "    AND POJANG3 = 'T'                           \r";
                SQL += "    AND SUGA_UNIT3 = 'T'                        \r";
                SQL += "    AND (GBSUGA1 IS NULL OR GBSUGA1 = '0')      \r"; //'산제가능약

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public static string Read_Powder_SuCode_New2(PsmhDb pDbCon, string strSuCode)
        {
            //산제불가코드 체크
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            try
            {
                //산재불가약 체크후 - 체크박스 표시
                SQL = "";
                SQL += " SELECT NOT_POWDER,NOT_POWDER_SUB       \r";
                SQL += "   FROM ADMIN.DRUG_MASTER4         \r";
                SQL += "  WHERE JepCode = '" + strSuCode + "'   \r";
                SQL += "    AND Not_Powder = '1'                \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }
                else
                {
                    rtnVal = "";
                }

                reader.Dispose();
                reader = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public void ssOrder_Button_Down(FarPoint.Win.Spread.FpSpread SpdNm, string strGubun)
        {
            if (strGubun == "Diagno")
            {
                if ((int)SpdNm.ActiveSheet.ActiveRowIndex < 0) return;

                SP.sprRowDown(ref SpdNm, (int)SpdNm.ActiveSheet.ActiveRowIndex, 0);
                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    if (clsOrdFunction.GEnvSet_Item21 != null && clsOrdFunction.GEnvSet_Item21 == "2")
                    {
                        SpdNm.ActiveSheet.SetActiveCell(SpdNm.ActiveSheet.ActiveRowIndex, 3);
                    }
                    else
                    {
                        SpdNm.ActiveSheet.SetActiveCell(SpdNm.ActiveSheet.ActiveRowIndex, 1);
                    }                        
                }
                else
                {
                    SpdNm.ActiveSheet.SetActiveCell(SpdNm.ActiveSheet.ActiveRowIndex, 2);
                }
                SpdNm.ShowRow(0, SpdNm.ActiveSheet.ActiveRowIndex, FarPoint.Win.Spread.VerticalPosition.Nearest);
            }
            else
            {
                if ((int)SpdNm.ActiveSheet.ActiveRowIndex < (int)clsOrdFunction.GnReadOrder) return;

                SP.sprRowDown(ref SpdNm, (int)SpdNm.ActiveSheet.ActiveRowIndex, (int)clsOrdFunction.GnReadOrder);
                SpdNm.ActiveSheet.SetActiveCell(SpdNm.ActiveSheet.ActiveRowIndex, 1);
                SpdNm.ShowRow(0, SpdNm.ActiveSheet.ActiveRowIndex, FarPoint.Win.Spread.VerticalPosition.Nearest);
            }
        }

        public void ssOrder_Button_Up(FarPoint.Win.Spread.FpSpread SpdNm, string strGubun)
        {
            if (strGubun == "Diagno")
            {
                if ((int)SpdNm.ActiveSheet.ActiveRowIndex <= 0) return;
                SP.sprRowUp(ref SpdNm, (int)SpdNm.ActiveSheet.ActiveRowIndex, 0);
                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    if (clsOrdFunction.GEnvSet_Item21 != null && clsOrdFunction.GEnvSet_Item21 == "2")
                    {
                        SpdNm.ActiveSheet.SetActiveCell(SpdNm.ActiveSheet.ActiveRowIndex, 3);
                    }
                    else
                    {
                        SpdNm.ActiveSheet.SetActiveCell(SpdNm.ActiveSheet.ActiveRowIndex, 1);
                    }
                }
                else
                {
                    SpdNm.ActiveSheet.SetActiveCell(SpdNm.ActiveSheet.ActiveRowIndex, 2);
                }
                SpdNm.ShowRow(0, SpdNm.ActiveSheet.ActiveRowIndex, FarPoint.Win.Spread.VerticalPosition.Nearest);
            }
            else
            {
                if ((int)SpdNm.ActiveSheet.ActiveRowIndex <= (int)clsOrdFunction.GnReadOrder) return;
                SP.sprRowUp(ref SpdNm, (int)SpdNm.ActiveSheet.ActiveRowIndex, (int)clsOrdFunction.GnReadOrder);
                SpdNm.ActiveSheet.SetActiveCell(SpdNm.ActiveSheet.ActiveRowIndex, 1);
                SpdNm.ShowRow(0, SpdNm.ActiveSheet.ActiveRowIndex, FarPoint.Win.Spread.VerticalPosition.Nearest);
            }
        }

        public void ssOrder_Button_First(FarPoint.Win.Spread.FpSpread SpdNm, string strGubun)
        {
            if (strGubun == "Diagno")
            {
                if ((int)SpdNm.ActiveSheet.ActiveRowIndex <= 0) return;
                SP.sprRowUp_First(ref SpdNm, (int)SpdNm.ActiveSheet.ActiveRowIndex, 0);
                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    SpdNm.ActiveSheet.SetActiveCell(0, 1);
                }
                else
                {
                    SpdNm.ActiveSheet.SetActiveCell(0, 2);
                }

                SpdNm.ShowRow(0, SpdNm.ActiveSheet.ActiveRowIndex, FarPoint.Win.Spread.VerticalPosition.Nearest);
            }
            else
            {
                //if ((int)SpdNm.ActiveSheet.ActiveRowIndex <= (int)clsOrdFunction.GnSunapOrdCount) return;
                if ((int)SpdNm.ActiveSheet.ActiveRowIndex <= (int)clsOrdFunction.GnReadOrder) return;
                SP.sprRowUp_First(ref SpdNm, (int)SpdNm.ActiveSheet.ActiveRowIndex, (int)clsOrdFunction.GnReadOrder);
                SpdNm.ActiveSheet.SetActiveCell((int)clsOrdFunction.GnReadOrder, 1);
                SpdNm.ShowRow(0, SpdNm.ActiveSheet.ActiveRowIndex, FarPoint.Win.Spread.VerticalPosition.Nearest);
            }
        }

        public void ssOrder_Button_Last(FarPoint.Win.Spread.FpSpread SpdNm, string strGubun)
        {
            if (strGubun == "Diagno")
            {
                if ((int)SpdNm.ActiveSheet.ActiveRowIndex < 0) return;

                SP.sprRowDown_Last(ref SpdNm, (int)SpdNm.ActiveSheet.ActiveRowIndex, (int)SpdNm.ActiveSheet.NonEmptyRowCount);
                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    SpdNm.ActiveSheet.SetActiveCell(SpdNm.ActiveSheet.NonEmptyRowCount - 1, 1);
                }
                else
                {
                    SpdNm.ActiveSheet.SetActiveCell(SpdNm.ActiveSheet.NonEmptyRowCount - 1, 2);
                }
                SpdNm.ShowRow(0, SpdNm.ActiveSheet.ActiveRowIndex, FarPoint.Win.Spread.VerticalPosition.Nearest);
            }
            else
            {
                if ((int)SpdNm.ActiveSheet.ActiveRowIndex < (int)clsOrdFunction.GnReadOrder) return;
                SP.sprRowDown_Last(ref SpdNm, (int)SpdNm.ActiveSheet.ActiveRowIndex, SpdNm.ActiveSheet.NonEmptyRowCount);
                SpdNm.ActiveSheet.SetActiveCell(SpdNm.ActiveSheet.NonEmptyRowCount - 1, 1);
                SpdNm.ShowRow(0, SpdNm.ActiveSheet.ActiveRowIndex, FarPoint.Win.Spread.VerticalPosition.Nearest);
            }
        }

        /// <summary>
        /// READ_CHK_암표지자검사(vb_opd_exam_chk.bas)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="sPano"></param>
        /// <param name="sBDate"></param>
        public void READ_CHK_CANCER_MARKER_TEST(PsmhDb pDbCon, string sPano, string sBDate)
        {
            string strDate1;
            string strDate2;
            string strRowId;
            string strChk;
            string strMsg;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            int intRowAffected = 0; //변경된 Row 받는 변수

            strDate1 = DateTime.Parse(sBDate).AddDays(-120).ToShortDateString();
            strDate2 = DateTime.Parse(sBDate).ToShortDateString();

            strRowId = "";
            strChk = "";

            try
            {
                //산재불가약 체크후 - 체크박스 표시
                SQL = "";
                SQL += " SELECT ROWID,CHK                                       \r";
                SQL += "   FROM ADMIN.ETC_EXAM_CHK                        \r";
                SQL += "  WHERE BDATE = TO_DATE('" + sBDate + "','YYYY-MM-DD') \r";
                SQL += "    AND PANO = '" + sPano + "'                          \r";
                SQL += "    AND GUBUN = '01'                                    \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strRowId = dt.Rows[0]["ROWID"].ToString().Trim();
                    if (dt.Rows[0]["CHK"].ToString().Trim() == "Y")
                    {
                        strChk = "OK";
                    }
                }

                if (strChk != "OK")
                {
                    SQL = "";
                    SQL += " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,TO_CHAR(Receivedate, 'YYYY-MM-DD') SeekDATE       \r";
                    SQL += "      , TO_CHAR(ResultDate, 'YYYY-MM-DD') ResultDate                                        \r";
                    SQL += "      , Pano,SName,Sex,DrCode,'' as GbSunap, '' as sucode,Specno                            \r";
                    SQL += "   From ADMIN.EXAM_SPECMST                                                             \r";
                    SQL += "  WHERE (Pano,Orderno) IN ( SELECT Ptno,orderno                                             \r";
                    SQL += "                              FROM ADMIN.OCS_IORDER                                    \r";
                    SQL += "                             WHERE  BDATE >= TO_DATE('" + strDate1 + "','YYYY-MM-DD')       \r";
                    SQL += "                               AND  BDATE <= TO_DATE('" + strDate2 + "','YYYY-MM-DD')       \r";
                    SQL += "                               AND trim(SuCode) in ( Select code                            \r";
                    SQL += "                                                       from ADMIN.bas_bcode           \r";
                    SQL += "                                                      where gubun = 'ETC_암표지자검사수가') \r";
                    SQL += "                           )                                                                \r";
                    SQL += "      AND BDATE >=TO_DATE('" + strDate1 + "','YYYY-MM-DD')                                  \r";
                    SQL += "      AND BDATE <=TO_DATE('" + strDate2 + "','YYYY-MM-DD')                                  \r";
                    SQL += "      AND Pano = '" + sPano + "'                                                            \r";
                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            SQL = "";
                            SQL += " SELECT R.Status, R.MasterCode,R.SubCode, R.Result, R.Refer, R.Panic    \r";
                            SQL += "      , R.IMGWRTNO, R.Delta, R.Unit, R.SeqNo, M.ExamName                \r";
                            SQL += "      , TO_CHAR(R.RESULTDATE,'YYYY-MM-DD') RESULTDATE                   \r";
                            SQL += "   FROM ADMIN.Exam_ResultC R                                       \r";
                            SQL += "      , ADMIN.Exam_Master  M                                       \r";
                            SQL += "  WHERE SpecNo = '" + dt1.Rows[i]["Specno"].ToString().Trim() + "'      \r";
                            SQL += "    AND R.SubCode = M.MasterCode(+)                                     \r";
                            SQL += "  ORDER BY R.SeqNo                                                      \r";
                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return;
                            }

                            if (dt2.Rows.Count > 0)
                            {
                                if (dt2.Rows[0]["REFER"].ToString().Trim() != "")
                                {
                                    clsDB.setBeginTran(pDbCon);

                                    try
                                    {
                                        if (strRowId == "")
                                        {
                                            SQL = "";
                                            SQL += " INSERT INTO ADMIN.ETC_EXAM_CHK                                       \r";
                                            SQL += "        (PANO,BDATE,REMARK,GUBUN,ENTDATE,ENTPART,CHK)                       \r";
                                            SQL += "  VALUES                                                                    \r";
                                            SQL += "        ('" + sPano + "', TRUNC(SYSDATE)                                    \r";
                                            SQL += "       , '암표지가검사', '01', SYSDATE, " + clsPublic.GnJobSabun + ", '')   \r";
                                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                                            if (SqlErr != "")
                                            {
                                                clsDB.setRollbackTran(pDbCon);
                                                ComFunc.MsgBox(SqlErr);
                                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                                Cursor.Current = Cursors.Default;
                                                return;
                                            }

                                            strMsg = "";
                                            strMsg += "암표지자 검사명 : " + dt2.Rows[0]["EXAMNAME"].ToString().Trim();
                                            strMsg += "\r\n\r\n" + "검사결과 : " + dt2.Rows[0]["RESULT"].ToString().Trim();
                                            MessageBox.Show(strMsg, "암표지가 이상결과 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            strMsg = "";
                                            strMsg += "암표지자 검사명 : " + dt2.Rows[0]["EXAMNAME"].ToString().Trim();
                                            strMsg += "\r\n\r\n" + "검사결과 : " + dt2.Rows[0]["RESULT"].ToString().Trim();
                                            strMsg += "\r\n\r\n" + "해당 결과 팝업 정보를 그만 보시겠습니까?";

                                            if (MessageBox.Show(strMsg, "암표지가 이상결과 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                            {
                                                if (strRowId != "")
                                                {
                                                    SQL = "";
                                                    SQL += " UPDATE ADMIN.ETC_EXAM_CHK    \r";
                                                    SQL += "    SET CHK   = 'Y'                 \r";
                                                    SQL += "  WHERE ROWID = '" + strRowId + "'  \r";
                                                    SQL += "    AND PANO  = '" + sPano + "'     \r";
                                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                                                    if (SqlErr != "")
                                                    {
                                                        clsDB.setRollbackTran(pDbCon);
                                                        ComFunc.MsgBox(SqlErr);
                                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                                        Cursor.Current = Cursors.Default;
                                                        return;
                                                    }
                                                }
                                            }
                                        }
                                        clsDB.setCommitTran(pDbCon);
                                        Cursor.Current = Cursors.Default;
                                    }
                                    catch (Exception ex)
                                    {
                                        clsDB.setRollbackTran(pDbCon);
                                        ComFunc.MsgBox(ex.Message);
                                        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                    }
                                    break;
                                }
                            }
                            dt2.Dispose();
                            dt2 = null;
                        }
                    }
                    dt1.Dispose();
                    dt1 = null;
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return;
            }
        }

        public DataTable fn_Set_illCode_Read()
        {
            DataTable RtnDt = null;
            string SQL = "";
            string SqlErr = "";
            DataTable dt1 = null;

            try
            {

                SQL = "";
                SQL += " SELECT ILLCODE, ILLNAMEK , ILLNAMEE            \r";
                SQL += "      , nvl(ILLNAMEK, ILLNAMEE) ILLNAME         \r";
                SQL += "      , NOUSE                                   \r";
                SQL += "   FROM ADMIN.BAS_ILLS                    \r";
                SQL += "  WHERE IllClass = '1'                          \r";
                SQL += "    AND (NOUSE <> 'N' OR NOUSE IS NULL)         \r";
                SQL += "    AND DDATE IS NULL                           \r";
                SQL += "  ORDER BY ILLCODE, ILLNAMEE                    \r";
                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }

                if (dt1.Rows.Count > 0)
                {
                    RtnDt = dt1;
                }

                dt1.Dispose();
                dt1 = null;
                return RtnDt;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }
        }

        public DataTable fn_Set_illCode_Read_New(string strSearchWord)
        {
            DataTable RtnDt = null;
            string SQL = "";
            string SqlErr = "";
            DataTable dt1 = null;
            DataTable dt2 = null;
            string strILLMSG = "";

            try
            {
                //2020-12-31 의료정보팀 요청으로 삭제코드 입력시 대체코드 팝업 추가 
                #region
                SQL = "";
                SQL = "SELECT NVL(ILLNAMEE, ILLNAMEK) AS ILLNAMEE, NOUSE, DDATE, REPCODE ";
                SQL = SQL + ComNum.VBLF + ",  (SELECT NVL(ILLNAMEE,ILLNAMEK)  FROM " + ComNum.DB_PMPA + "BAS_ILLS WHERE ILLCODE = A.REPCODE AND ROWNUM = 1) AS REPNAME ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_ILLS A ";
                SQL = SQL + ComNum.VBLF + " WHERE ILLCODE = '" + strSearchWord + "' ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;                    
                }

                if(dt1.Rows.Count > 0)
                {
                    //if (dt1.Rows[0]["REPCODE"].ToString().Trim().NotEmpty())
                    //{
                    //    strILLMSG = string.Format("삭제코드 : {0} => 대체코드 : {1}입니다.\r\n대체코드로 사용 해주세요.", strSearchWord, dt1.Rows[0]["REPCODE"].ToString().Trim().ToUpper());

                    //    dt1.Dispose();
                    //    dt1 = null;

                    //    ComFunc.MsgBox(strILLMSG, "확인");

                    //    return null;
                    //}
                    //else if (dt1.Rows[0]["DDATE"].ToString().Trim() != "")
                    if (dt1.Rows[0]["REPCODE"].ToString().Trim().NotEmpty())
                    {
                        strILLMSG = "[삭제상병]";
                        strILLMSG = strILLMSG + strSearchWord + "(" + dt1.Rows[0]["ILLNAMEE"].ToString().Trim() + ") 는 삭제상병입니다. " + ComNum.VBLF;

                        strILLMSG = strILLMSG + "사용가능 상병 아래 상병참조 바랍니다." + ComNum.VBLF;
                        strILLMSG = strILLMSG + "=======================================" + ComNum.VBLF;
                        strILLMSG = strILLMSG + VB.Left(VB.Trim(dt1.Rows[0]["REPCODE"].ToString().Trim()) + VB.Space(12), 12) + " " + dt1.Rows[0]["REPNAME"].ToString().Trim() + ComNum.VBLF;
                        strILLMSG = strILLMSG + "=======================================" + ComNum.VBLF;

                        ComFunc.MsgBox(strILLMSG, "확인");
                        //SQL = "";
                        //SQL = "SELECT REPCODE, (SELECT  ";
                        //SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_ILLS ";
                        //SQL = SQL + ComNum.VBLF + " WHERE ILLCODE  LIKE '" + VB.Left(VB.Trim(strSearchWord), VB.Len(VB.Trim(strSearchWord)) - 1) + "%' ";
                        //SQL = SQL + ComNum.VBLF + "   AND LENGTH(ILLCODE) <= 6 ";
                        //SQL = SQL + ComNum.VBLF + "   AND (NOUSE <>'N' OR NOUSE IS NULL) ";
                        //SQL = SQL + ComNum.VBLF + "   AND ILLCLASS ='1' ";
                        //SQL = SQL + ComNum.VBLF + "   AND REPCODE IS NOT NULL ";
                        //SQL = SQL + ComNum.VBLF + "   AND DDATE IS NULL ";

                        //SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                        //if (SqlErr != "")
                        //{
                        //    Cursor.Current = Cursors.Default;
                        //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        //    return null;
                        //}

                        //if (dt2.Rows.Count > 0)
                        //{
                        //    strILLMSG = strILLMSG + "사용가능 상병 아래 상병참조 바랍니다." + ComNum.VBLF;
                        //    strILLMSG = strILLMSG + "=======================================" + ComNum.VBLF;

                        //    for (int i = 0; i < dt2.Rows.Count; i++)
                        //    {
                        //        strILLMSG = strILLMSG + VB.Left(VB.Trim(dt2.Rows[i]["iLLCode"].ToString().Trim()) + VB.Space(12), 12) + " " + dt2.Rows[i]["ILLNAMEK"].ToString().Trim() + ComNum.VBLF;
                        //    }
                        //}

                        //dt2.Dispose();
                        //dt2 = null;



                        dt1.Dispose();
                        dt1 = null;
                        return null;
                    }
                }

                dt1.Dispose();
                dt1 = null;
                #endregion

                if (clsOrdFunction.GstrGbJob == "IPD")
                {
                    SQL = "";
                    SQL += " SELECT ILLCODE, ILLNAMEK , ILLNAMEE                \r";
                    SQL += "      , nvl(ILLNAMEK, ILLNAMEE) ILLNAME             \r";
                    SQL += "      , NOUSE                                       \r";
                    SQL += "   FROM ADMIN.BAS_ILLS                        \r";
                    SQL += "  WHERE IllClass = '1'                              \r";
                    SQL += "    AND (NOUSE <> 'N' OR NOUSE IS NULL)             \r";
                    SQL += "    AND DDATE IS NULL                               \r";
                    SQL += "    AND KCD8 = '*'                                  \r";
                    //의뢰서 2021-925
                    SQL += "    AND (UPPER(ILLCODE) LIKE '%" + strSearchWord + "%'     \r";
                    SQL += "        OR UPPER(ILLNAMEK) LIKE '%" + strSearchWord + "%'  \r";
                    SQL += "        OR UPPER(ILLNAMEE) LIKE '%" + strSearchWord + "%') \r";
                    SQL += "  ORDER BY ILLCODE, ILLNAMEE                        \r";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return null;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        RtnDt = dt1;
                    }
                }
                else
                {
                    SQL = "";
                    SQL += " SELECT ILLCODE, ILLNAMEK , ILLNAMEE                \r";
                    SQL += "      , nvl(ILLNAMEK, ILLNAMEE) ILLNAME             \r";
                    SQL += "      , NOUSE                                       \r";
                    SQL += "   FROM ADMIN.BAS_ILLS                        \r";
                    SQL += "  WHERE IllClass = '1'                              \r";
                    SQL += "    AND (NOUSE <> 'N' OR NOUSE IS NULL)             \r";
                    SQL += "    AND DDATE IS NULL                               \r";
                    SQL += "    AND KCD8 = '*'                                  \r";
                    //의뢰서 2021-925
                    SQL += "    AND (UPPER(ILLCODE) = '" + strSearchWord + "'     \r";
                    SQL += "        OR UPPER(ILLNAMEK) = '" + strSearchWord + "'  \r";
                    SQL += "        OR UPPER(ILLNAMEE) = '" + strSearchWord + "') \r";
                    SQL += "  ORDER BY ILLCODE, ILLNAMEE                        \r";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return null;
                    }

                    if (dt1.Rows.Count == 1)
                    {
                        RtnDt = dt1;
                    }
                    else
                    {
                        dt1.Dispose();
                        dt1 = null;

                        SQL = "";
                        SQL += " SELECT ILLCODE, ILLNAMEK , ILLNAMEE                \r";
                        SQL += "      , nvl(ILLNAMEK, ILLNAMEE) ILLNAME             \r";
                        SQL += "      , NOUSE                                       \r";
                        SQL += "   FROM ADMIN.BAS_ILLS                        \r";
                        SQL += "  WHERE IllClass = '1'                              \r";
                        SQL += "    AND (NOUSE <> 'N' OR NOUSE IS NULL)             \r";
                        SQL += "    AND DDATE IS NULL                               \r";
                        SQL += "    AND KCD8 = '*'                                  \r";
                        //의뢰서 2021-925
                        SQL += "    AND (UPPER(ILLCODE) LIKE '%" + strSearchWord + "%'     \r";
                        SQL += "        OR UPPER(ILLNAMEK) LIKE '%" + strSearchWord + "%'  \r";
                        SQL += "        OR UPPER(ILLNAMEE) LIKE '%" + strSearchWord + "%') \r";
                        SQL += "  ORDER BY ILLCODE, ILLNAMEE                        \r";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return null;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            RtnDt = dt1;
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;
                }

                return RtnDt;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }
        }

        public DataTable fn_Set_OrderCode_Read()
        {
            DataTable RtnDt = null;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {

                SQL = "";
                SQL += " SELECT A.ORDERCODE CORDERCODE, upper(A.OrderName) ORDERNAME, upper(A.OrderNameS) ORDERNAMES \r";
                SQL += "      , A.OrderName cOrderName, A.OrderNameS cOrderNames                                     \r";
                SQL += "      , A.GBBOTH CGBBOTH, A.DISPHEADER CDISPHEADER, A.GBINFO CGBINFO, A.SLIPNO               \r";
                SQL += "   FROM ADMIN.OCS_ORDERCODE A, ADMIN.BAS_SUT B                                    \r";
                SQL += "  WHERE 1=1                                                                                  \r";
                SQL += "    AND (senddept != 'N' or senddept is null)                                                \r";
                SQL += "    AND seqno <> 0                                                                           \r";
                SQL += "    AND A.SUCODE = B.SUCODE(+)                                                               \r";
                SQL += "    AND (B.DELDATE IS NULL OR B.DELDATE > TRUNC(SYSDATE))                                    \r";
                SQL += "    AND GBINPUT = '1'                                                                        \r";
                SQL += "  UNION ALL                                                                                  \r";
                SQL += " SELECT A.SuNext CORDERCODE, UPPER(A.SName) ORDERNAME, UPPER(A.HName) ORDERNAMES             \r";
                SQL += "      , A.SNAME AS CORDERNAME, A.HNAME CORDERNAMES                                           \r";
                SQL += "      , '' CGBBOTH, '' CDISPHEADER, ''  CGBINFO, ''  SLIPNO                                  \r";
                SQL += "   FROM ADMIN.OCS_DRUGINFO_new  A                                                       \r";
                SQL += "      , ADMIN.DRUG_JEP          B                                                       \r";
                SQL += "  WHERE A.SUNEXT = B.JEPCODE(+)                                                              \r";
                SQL += "    AND B.DELDATE IS NULL                                                                    \r";
                //SQL += "    AND A.SUNEXT NOT IN (SELECT SUCODE                                                       \r";
                //SQL += "                           FROM ADMIN.BAS_SUT                                          \r";
                //SQL += "                          WHERE DELDATE IS NOT NULL)                                         \r";
                SQL += "  ORDER BY ORDERNAME, CORDERCODE                                                             \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnDt = dt;
                }

                dt.Dispose();
                dt = null;
                return RtnDt;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }
        }

        public DataTable fn_Set_OrderCode_Read_New(string strSearchWord)
        {
            DataTable RtnDt = null;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strSearchWord = strSearchWord.Replace("'", "`");

            try
            {
                SQL = "";
                SQL += " select cordercode,  max(BUN) AS BUN, max(ordername) ordername, max(ordernames) ordernames                            \r";
                SQL += "      , max(cordername) cordername, max(cordernames) cordernames                                    \r";
                SQL += "      , max(cgbboth) cgbboth, max(cdispheader) cdispheader, max(cgbinfo) cgbinfo                    \r";
                SQL += "      , max(slipno) slipno, max(subrate) subrate , max(deldate) deldate                             \r";
                SQL += "      , max(gbout) gbout, max(gbin) gbin                                                            \r";
                SQL += "  from(                                                                                             \r";
                SQL += "       SELECT A.BUN, ORDERCODE CORDERCODE, upper(OrderName) ORDERNAME, upper(OrderNameS) ORDERNAMES        \r";
                SQL += "            , OrderName cOrderName, OrderNameS cOrderNames                                          \r";
                SQL += "            , GBBOTH CGBBOTH, DISPHEADER CDISPHEADER, GBINFO CGBINFO, SLIPNO, a.SUBRATE SUBRATE     \r";
                SQL += "            , '' DELDATE                                                                            \r";
                SQL += "            , (select distinct '원외전용'                                                           \r";
                SQL += "                 from ADMIN.bas_sut                                                           \r";
                SQL += "                where sucode = b.sucode                                                             \r";
                SQL += "                  and sugbj = '1') GBOUT                                                            \r";
                SQL += "            , (select distinct '입원전용'                                                           \r";
                SQL += "                 from ADMIN.bas_sut                                                           \r";
                SQL += "                where sucode = b.sucode                                                             \r";
                SQL += "                  and sugbj = '2') GBIN                                                             \r";
                SQL += "         FROM ADMIN.OCS_ORDERCODE a                                                            \r";
                SQL += "            , ADMIN.BAS_SUT      b                                                            \r";
                SQL += "        WHERE (senddept != 'N' or senddept is null)                                                 \r";
                SQL += "          AND seqno <> 0                                                                            \r";
                SQL += "          AND a.SuCode    = b.SuCode(+)                                                             \r";
                SQL += "          AND (b.DELDATE IS NULL OR b.DELDATE > trunc(sysdate))                                     \r";
                // 전산업무의뢰서 2019-1405
                SQL += "          AND a.GBINPUT = '1'                                                                      \r";
                if (clsOrdFunction.GstrGbJob != "OPD")
                {
                    //SQL += "          AND (a.GbSub <> '1' OR a.GbSub IS NULL)                                                   \r";
                }
                if (clsPublic.GstrDeptCode == "PC" || clsPublic.GstrDeptCode == "AN")
                {
                    SQL += "    AND a.SlipNo   NOT IN ('0106', 'MD', '0105','0106','0075')              \r";
                }
                else
                {
                    SQL += "    AND a.SlipNo   NOT IN ('0106', 'MD', '0105','0106','0074','0075')       \r";
                }
                //SQL += "          AND SLIPNO NOT IN ('0106')                                                          \r";    // , 'MD'자가약 제외
                SQL += "          AND SLIPNO NOT IN(select trim(deptcode) from ADMIN.bas_clinicdept where gbjupsu = '1') \r"; //과처방 제외
                SQL += "          AND upper(ORDERCODE) = '" + strSearchWord.ToUpper() + "'                                  \r";
                SQL += "        UNION ALL                                                                                   \r";
                SQL += "       SELECT '11' AS BUN, A.SuNext CORDERCODE, UPPER(A.SName) ORDERNAME, UPPER(A.HName) ORDERNAMES              \r";
                SQL += "            , A.SNAME AS CORDERNAME, A.HNAME CORDERNAMES                                            \r";
                SQL += "            , '' CGBBOTH, '' CDISPHEADER, ''  CGBINFO                                               \r";
                SQL += "            , (select slipno from ADMIN.ocs_ordercode where ordercode = a.sunext) SLIPNO       \r";
                SQL += "            , (select subrate from ADMIN.ocs_ordercode where ordercode = a.sunext) SUBRATE     \r";
                SQL += "            , TO_CHAR(A.DELDATE, 'YYYY-MM-DD') DELDATE                                              \r";
                SQL += "            , (select distinct '원외전용'                                                           \r";
                SQL += "                 from ADMIN.bas_sut                                                           \r";
                SQL += "                where sucode = a.sunext                                                             \r";
                SQL += "                  and sugbj = '1') GBOUT                                                            \r";
                SQL += "            , (select distinct '입원전용'                                                           \r";
                SQL += "                 from ADMIN.bas_sut                                                           \r";
                SQL += "                where sucode = a.sunext                                                             \r";
                SQL += "                  and sugbj = '2') GBIN                                                             \r";
                SQL += "         FROM ADMIN.OCS_DRUGINFO_new  A                                                        \r";
                SQL += "            , ADMIN.DRUG_JEP          B                                                        \r";
                SQL += "        WHERE A.SUNEXT = B.JEPCODE(+)                                                               \r";
                SQL += "          AND B.DELDATE IS NULL                                                                     \r";
                SQL += "          AND upper(A.SuNext) = '" + strSearchWord.ToUpper() + "'                                   \r";
                SQL += "        ORDER BY ORDERNAME, CORDERCODE                                                              \r";
                SQL += "      ) A                                                                                           \r";
                SQL += "   WHERE CORDERCODE IS NOT NULL                                                                     \r";
                #region 2021-11-24 삭제코드 표시 옵션 처리 1이 미표시 이고 밑 쿼리는 판넬에 있을경우만 보여줍니다.
                if (GEnvSet_Item22.Equals("2"))
                {
                    SQL += "     AND EXISTS                                                                                     \r";
                    SQL += "     (                                                                                              \r";
                    SQL += "        SELECT 1                                                                                    \r";
                    SQL += "          FROM ADMIN.OCS_ORDERCODE                                                             \r";
                    SQL += "         WHERE ORDERCODE = A.CORDERCODE                                                             \r";
                    SQL += "           AND (SENDDEPT != 'N' OR SENDDEPT IS NULL)                                                \r";
                    SQL += "     )                                                                                              \r";
                }
                #endregion

                SQL += "   group by cordercode                                                                              \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog("함수명 : " + "fn_Set_OrderCode_Read_New " + ComNum.VBLF + SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }
                if (dt.Rows.Count == 1)
                {
                    RtnDt = dt;
                }
                else
                {
                    SQL = "";
                    SQL += " select cordercode,  max(BUN) AS BUN, max(ordername) ordername, max(ordernames) ordernames                            \r";
                    SQL += "      , max(cordername) cordername, max(cordernames) cordernames                                    \r";
                    SQL += "      , max(cgbboth) cgbboth, max(cdispheader) cdispheader, max(cgbinfo) cgbinfo                    \r";
                    SQL += "      , max(slipno) slipno, max(subrate) subrate, max(deldate) deldate                              \r";
                    SQL += "      , max(gbout) gbout, max(gbin) gbin                                                            \r";
                    SQL += "  from(                                                                                             \r";
                    SQL += "       SELECT A.BUN, ORDERCODE CORDERCODE, upper(OrderName) ORDERNAME, upper(OrderNameS) ORDERNAMES        \r";
                    SQL += "            , OrderName cOrderName, OrderNameS cOrderNames                                          \r";
                    SQL += "            , GBBOTH CGBBOTH, DISPHEADER CDISPHEADER, GBINFO CGBINFO, SLIPNO, a.SUBRATE SUBRATE     \r";
                    SQL += "            , '' DELDATE                                                                            \r";
                    SQL += "            , (select distinct '원외전용'                                                           \r";
                    SQL += "                 from ADMIN.bas_sut                                                           \r";
                    SQL += "                where sucode = b.sucode                                                             \r";
                    SQL += "                  and sugbj = '1') GBOUT                                                            \r";
                    SQL += "            , (select distinct '입원전용'                                                           \r";
                    SQL += "                 from ADMIN.bas_sut                                                           \r";
                    SQL += "                where sucode = b.sucode                                                             \r";
                    SQL += "                  and sugbj = '2') GBIN                                                             \r";
                    SQL += "         FROM ADMIN.OCS_ORDERCODE a                                                            \r";
                    SQL += "            , ADMIN.BAS_SUT      b                                                            \r";
                    SQL += "        WHERE (senddept != 'N' or senddept is null)                                                 \r";
                    SQL += "          AND seqno <> 0                                                                            \r";
                    SQL += "          AND a.SuCode    = b.SuCode(+)                                                             \r";
                    SQL += "          AND (b.DELDATE IS NULL OR b.DELDATE > trunc(sysdate))                                     \r";
                    // 전산업무의뢰서 2019-1405
                    SQL += "          AND a.GBINPUT = '1'                                                                      \r";
                    if (clsOrdFunction.GstrGbJob != "OPD")
                    {
                        //SQL += "           AND (a.GbSub <> '1' OR a.GbSub IS NULL)                                                  \r";
                    }
                    SQL += "           AND SLIPNO NOT IN ('0106')                                                         \r";    //, 'MD' 자가약 제외
                    SQL += "           AND SLIPNO NOT IN(select trim(deptcode) from ADMIN.bas_clinicdept where gbjupsu = '1') \r"; //과처방 제외
                    SQL += "           AND (upper(ORDERCODE) = '" + strSearchWord.ToUpper() + "'                                 \r";
                    SQL += "            or upper(ORDERNAME) = '" + strSearchWord.ToUpper() + "'                                  \r";
                    SQL += "            or upper(ORDERNAMES) = '" + strSearchWord.ToUpper() + "')                                \r";
                    //SQL += "    AND BUN NOT IN('11', '12', '20')                                                                \r";
                    SQL += "        UNION ALL                                                                                   \r";
                    SQL += "       SELECT '11' AS BUN, A.SuNext CORDERCODE, UPPER(A.SName) ORDERNAME, UPPER(A.HName) ORDERNAMES              \r";
                    SQL += "            , A.SNAME AS CORDERNAME, A.HNAME CORDERNAMES                                            \r";
                    SQL += "            , '' CGBBOTH, '' CDISPHEADER, ''  CGBINFO                                               \r";
                    SQL += "            , (select slipno from ADMIN.ocs_ordercode where ordercode = a.sunext) SLIPNO       \r";
                    SQL += "            , (select subrate from ADMIN.ocs_ordercode where ordercode = a.sunext) SUBRATE     \r";
                    SQL += "            , TO_CHAR(A.DELDATE, 'YYYY-MM-DD') DELDATE                                              \r";
                    SQL += "            , (select distinct '원외전용'                                                           \r";
                    SQL += "                 from ADMIN.bas_sut                                                           \r";
                    SQL += "                where sucode = a.sunext                                                             \r";
                    SQL += "                  and sugbj = '1') GBOUT                                                            \r";
                    SQL += "            , (select distinct '입원전용'                                                           \r";
                    SQL += "                 from ADMIN.bas_sut                                                           \r";
                    SQL += "                where sucode = a.sunext                                                             \r";
                    SQL += "                  and sugbj = '2') GBIN                                                             \r";
                    SQL += "         FROM ADMIN.OCS_DRUGINFO_new  A                                                        \r";
                    SQL += "            , ADMIN.DRUG_JEP          B                                                        \r";
                    SQL += "        WHERE A.SUNEXT = B.JEPCODE(+)                                                               \r";
                    SQL += "          AND B.DELDATE IS NULL                                                                     \r";
                    SQL += "          AND (upper(A.SName) = '" + strSearchWord.ToUpper() + "'                                   \r";
                    SQL += "           or upper(A.SuNext) = '" + strSearchWord.ToUpper() + "'                                   \r";
                    SQL += "           or upper(A.HName) = '" + strSearchWord.ToUpper() + "'                                    \r";
                    SQL += "           or upper(A.ENAME) = '" + strSearchWord.ToUpper() + "')                                   \r";
                    SQL += "        ORDER BY ORDERNAME, CORDERCODE                                                              \r";
                    SQL += "      )  A                                                                                           \r";
                    SQL += "   WHERE CORDERCODE IS NOT NULL                                                                     \r";
                    #region 2021-11-24 삭제코드 표시 옵션 처리 1이 미표시 이고 밑 쿼리는 판넬에 있을경우만 보여줍니다.
                    if (GEnvSet_Item22.Equals("2"))
                    {
                        SQL += "     AND EXISTS                                                                                     \r";
                        SQL += "     (                                                                                              \r";
                        SQL += "        SELECT 1                                                                                    \r";
                        SQL += "          FROM ADMIN.OCS_ORDERCODE                                                             \r";
                        SQL += "         WHERE ORDERCODE = A.CORDERCODE                                                             \r";
                        SQL += "           AND (SENDDEPT != 'N' OR SENDDEPT IS NULL)                                                \r";
                        SQL += "     )                                                                                              \r";
                    }
                    #endregion
                    SQL += "   group by cordercode                                                                              \r";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog("함수명 : " + "fn_Set_OrderCode_Read_New " + ComNum.VBLF + SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return null;
                    }
                    if (dt.Rows.Count == 1)
                    {
                        RtnDt = dt;
                    }
                    else
                    {
                        dt.Dispose();
                        dt = null;

                        SQL = "";
                        SQL += " select cordercode, max(BUN) AS BUN, max(ordername) ordername, max(ordernames) ordernames                            \r";
                        SQL += "      , max(cordername) cordername, max(cordernames) cordernames                                    \r";
                        SQL += "      , max(cgbboth) cgbboth, max(CDISPHEADER) CDISPHEADER, max(cgbinfo) cgbinfo                    \r";
                        SQL += "      , max(slipno) slipno, max(subrate) subrate, max(deldate) deldate                              \r";
                        SQL += "      , max(gbout) gbout, max(gbin) gbin                                                            \r";
                        SQL += "  from (                                                                                            \r";
                        SQL += "        SELECT A.BUN, ORDERCODE CORDERCODE, upper(OrderName) ORDERNAME, upper(OrderNameS) ORDERNAMES       \r";
                        SQL += "             , OrderName cOrderName, OrderNameS cOrderNames                                         \r";
                        SQL += "             , GBBOTH CGBBOTH, DISPHEADER CDISPHEADER, GBINFO CGBINFO, SLIPNO, a.SUBRATE SUBRATE    \r";
                        SQL += "            , '' DELDATE                                                                            \r";
                        SQL += "            , (select distinct '원외전용'                                                            \r";
                        SQL += "                 from ADMIN.bas_sut                                                           \r";
                        SQL += "                where sucode = b.sucode                                                             \r";
                        SQL += "                  and sugbj = '1') GBOUT                                                            \r";
                        SQL += "            , (select distinct '입원전용'                                                           \r";
                        SQL += "                 from ADMIN.bas_sut                                                           \r";
                        SQL += "                where sucode = b.sucode                                                             \r";
                        SQL += "                  and sugbj = '2') GBIN                                                             \r";
                        SQL += "          FROM ADMIN.OCS_ORDERCODE a                                                           \r";
                        SQL += "            , ADMIN.BAS_SUT      b                                                            \r";
                        SQL += "         WHERE (senddept != 'N' or senddept is null)                                                \r";
                        SQL += "           AND seqno <> 0                                                                           \r";
                        SQL += "           AND a.SuCode    = b.SuCode(+)                                                            \r";
                        SQL += "           AND (b.DELDATE IS NULL OR b.DELDATE > trunc(sysdate))                                    \r";
                        // 전산업무의뢰서 2019-1405
                        SQL += "           AND a.GBINPUT = '1'                                                                      \r";
                        if (clsOrdFunction.GstrGbJob != "OPD")
                        {
                            //SQL += "           AND (a.GbSub <> '1' OR a.GbSub IS NULL)                                                  \r";
                        }
                        SQL += "           AND (upper(ORDERCODE) LIKE '" + "%" + strSearchWord.ToUpper() + "%" + "'                 \r";
                        SQL += "            or upper(ORDERNAME) LIKE '" + "%" + strSearchWord.ToUpper() + "%" + "'                  \r";
                        SQL += "            or upper(ORDERNAMES) LIKE '" + "%" + strSearchWord.ToUpper() + "%" + "'                 \r";
                        SQL += "            or upper(DISPHEADER) LIKE '" + "%" + strSearchWord.ToUpper() + "%" + "')                \r";
                        //SQL += "           AND BUN NOT IN('11', '12', '20')                                                         \r";
                        SQL += "           AND SLIPNO NOT IN ('0106')                                                         \r";    //, 'MD' 자가약 제외
                        SQL += "           AND trim(SLIPNO) NOT IN(select trim(deptcode) from ADMIN.bas_clinicdept where gbjupsu = '1') \r";//과처방 제외
                        SQL += "         UNION ALL                                                                                  \r";
                        SQL += "        SELECT '11' AS BUN, A.SuNext CORDERCODE, UPPER(A.SName) ORDERNAME, UPPER(A.HName) ORDERNAMES             \r";
                        SQL += "             , A.SNAME AS CORDERNAME, A.HNAME CORDERNAMES                                           \r";
                        SQL += "             , '' CGBBOTH, '' CDISPHEADER, ''  CGBINFO                                              \r";
                        SQL += "             , (select slipno from ADMIN.ocs_ordercode where ordercode = a.sunext) SLIPNO      \r";
                        SQL += "             , (select subrate from ADMIN.ocs_ordercode where ordercode = a.sunext) SUBRATE    \r";
                        SQL += "            , TO_CHAR(A.DELDATE, 'YYYY-MM-DD') DELDATE                                              \r";
                        SQL += "            , (select distinct '원외전용'                                                           \r";
                        SQL += "                 from ADMIN.bas_sut                                                           \r";
                        SQL += "                where sucode = a.sunext                                                             \r";
                        SQL += "                  and sugbj = '1') GBOUT                                                            \r";
                        SQL += "            , (select distinct '입원전용'                                                           \r";
                        SQL += "                 from ADMIN.bas_sut                                                           \r";
                        SQL += "                where sucode = a.sunext                                                             \r";
                        SQL += "                  and sugbj = '2') GBIN                                                             \r";
                        SQL += "          FROM ADMIN.OCS_DRUGINFO_new  A                                                       \r";
                        SQL += "             , ADMIN.DRUG_JEP          B                                                       \r";
                        SQL += "         WHERE A.SUNEXT = B.JEPCODE(+)                                                              \r";
                        SQL += "           AND B.DELDATE IS NULL                                                                    \r";
                        SQL += "           AND (upper(A.SName) LIKE '" + "%" + strSearchWord.ToUpper() + "%" + "'                   \r";
                        SQL += "            or upper(A.SuNext) LIKE '" + "%" + strSearchWord.ToUpper() + "%" + "'                   \r";
                        SQL += "            or upper(A.HName) LIKE '" + "%" + strSearchWord.ToUpper() + "%" + "'                    \r";
                        SQL += "            or upper(A.ENAME) LIKE '" + "%" + strSearchWord.ToUpper() + "%" + "')                   \r";
                        SQL += "         ORDER BY ORDERNAME, CORDERCODE                                                             \r";
                        SQL += "        ) A                                                                                          \r";
                        SQL += "   WHERE CORDERCODE IS NOT NULL                                                                     \r";
                        #region 2021-11-24 삭제코드 표시 옵션 처리 1이 미표시 이고 밑 쿼리는 판넬에 있을경우만 보여줍니다.
                        if (GEnvSet_Item22.Equals("2"))
                        {
                            SQL += "     AND EXISTS                                                                                     \r";
                            SQL += "     (                                                                                              \r";
                            SQL += "        SELECT 1                                                                                    \r";
                            SQL += "          FROM ADMIN.OCS_ORDERCODE                                                             \r";
                            SQL += "         WHERE ORDERCODE = A.CORDERCODE                                                             \r";
                            SQL += "           AND (SENDDEPT != 'N' OR SENDDEPT IS NULL)                                                \r";
                            SQL += "     )                                                                                              \r";
                        }
                        #endregion
                        SQL += "  group by cordercode                                                                               \r";
                        SQL += "  order by slipno, cordercode                                                                       \r";
                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog("함수명 : " + "fn_Set_OrderCode_Read_New " + ComNum.VBLF + SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return null;
                        }

                        RtnDt = dt;
                    }

                    dt.Dispose();
                    dt = null;
                }

                return RtnDt;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox("함수명 : " + "fn_Set_OrderCode_Read_New " + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "fn_Set_OrderCode_Read_New " + ComNum.VBLF + ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }
        }

        public bool APACHE_24_CHECK(PsmhDb pDbCon, long nIpdNo)
        {
            bool rtnValue;

            rtnValue = true;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //아파치 스코어 입력이 안되어 있을 경우
            try
            {
                SQL = "";
                SQL += " SELECT SAPS_ENTDATE                    \r";
                SQL += "   FROM ADMIN.NUR_MASTER          \r";
                SQL += "  WHERE IPDNO = " + nIpdNo + "          \r";
                SQL += "    AND SAPS_ENTDATE IS NULL            \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnValue;
                }

                dt.Dispose();
                dt = null;

                //이실일시를 통해서 24시간 초과 여부 검색
                SQL = "";
                SQL += " SELECT trunc(TO_CHAR(SYSDATE - MAX(TRSDATE)) * 24) DATETIME    \r";
                SQL += "   FROM ADMIN.IPD_TRANSFOR                                \r";
                SQL += "  WHERE TOWARD IN ('33','35')                                   \r";
                SQL += "    AND IPDNO = " + nIpdNo + "                                  \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    rtnValue = false;
                    return rtnValue;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["DATETIME"].ToString().Trim() != "")
                    {
                        if (int.Parse(dt.Rows[0]["DATETIME"].ToString()) >= 24)
                        {
                            rtnValue = false;
                            dt.Dispose();
                            dt = null;
                            return rtnValue;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //바로 입원하였을 경우 입원일시를 통해서 24시간 초과 여부 검색
                SQL = "";
                SQL += " SELECT CEIL(TO_CHAR(SYSDATE - A.INDATE) * 24) DATETIME   \r";
                SQL += "   FROM ADMIN.IPD_NEW_MASTER A                \r";
                SQL += "      , ADMIN.NUR_MASTER     B                \r";
                SQL += "  WHERE A.IPDNO = " + nIpdNo + "                    \r";
                SQL += "    AND A.IPDNO = B.IPDNO                           \r";
                SQL += "    AND B.INWARD IN ('33','35')                     \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnValue;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["DATETIME"].ToString().Trim() != "")
                    {
                        if (double.Parse(dt.Rows[0]["DATETIME"].ToString()) >= 24)
                        {
                            rtnValue = false;
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                return rtnValue;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnValue;
            }
        }

        public string Read_GamekName(PsmhDb pDbCon, string strPano)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            SQL = "";
            SQL += " SELECT ADMIN.FC_OCS_GAMEKNM(GBGAMEK) GAMEKNAME    \r";
            SQL += "   FROM ADMIN.BAS_PATIENT                         \r";
            SQL += "  WHERE PANO = '" + strPano + "'                        \r";

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return "";
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();
            reader = null;
            return rtnVal;
        }

        /// <summary>
        /// 수술경과일수
        /// 사용하지 마시용 : 전혀 맞지 않습니다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPano"></param>
        /// <returns></returns>
        public string Read_POD(PsmhDb pDbCon, string strPano, string strInDate)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT OPDATE - TO_DATE('" + DateTime.Parse(strInDate).ToShortDateString() + "', 'YYYY-MM-DD') POD \r";
            SQL += "   FROM ADMIN.ORAN_MASTER                                                                     \r";
            SQL += "  WHERE PANO = '" + strPano + "'                                                                    \r";
            SQL += "    AND OPDATE >= TO_DATE('" + DateTime.Parse(strInDate).ToShortDateString() + "', 'YYYY-MM-DD')    \r";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return "";
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["POD"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        /// <summary>
        /// 입원후 혹은 전체 수술의 POD 읽기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPano"></param>
        /// <param name="strInDate"></param>
        /// <returns></returns>
        public int Read_POD_Ex(PsmhDb pDbCon, string strPano, string strInDate, string strToDay)
        {
            int rtnVal = -1;
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            SQL = "";
            SQL += " SELECT OPDATE \r";
            SQL += "   FROM ADMIN.ORAN_MASTER                                                                     \r";
            SQL += "  WHERE PANO = '" + strPano + "'                                                                    \r";
            if (strInDate != "")
            {
                SQL += "    AND OPDATE >= TO_DATE('" + DateTime.Parse(strInDate).ToShortDateString() + "', 'YYYY-MM-DD')    \r";
            }
            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (reader.HasRows == false)
            {
                reader.Dispose();
                reader = null;
                return rtnVal;
            }

            SQL = "";
            //SQL += " SELECT (TRUNC(SYSDATE) - MAX(OPDATE)) AS POD \r";
            SQL += " SELECT (TO_DATE('" + strToDay + "','YYYY-MM-DD') - MAX(OPDATE)) AS POD                             \r";
            SQL += "   FROM ADMIN.ORAN_MASTER                                                                     \r";
            SQL += "  WHERE PANO = '" + strPano + "'                                                                    \r";
            if (strInDate != "")
            {
                SQL += "    AND OPDATE >= TO_DATE('" + DateTime.Parse(strInDate).ToShortDateString() + "', 'YYYY-MM-DD')    \r";
            }

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).To<int>(0);
            }

            reader.Dispose();
            reader = null;
            return rtnVal;
        }

        /// <summary>
        /// 최종처방일자(병동처방)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPano"></param>
        /// <returns></returns>
        public string Read_LastOrdDate(PsmhDb pDbCon, string strPano)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            SQL = "";
            SQL += " select max(bdate) bdate                                \r";
            SQL += "   from ADMIN.ocs_iorder                           \r";
            SQL += "  where ptno = '" + strPano + "'                        \r";
            SQL += "    and gbstatus in(' ', 'D+')                          \r";
            SQL += "    and(gbioe not in('EI', 'E') or gbioe is null)       \r";

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return "";
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();
            reader = null;
            return rtnVal;
        }

        public int Order_Row_Count(FarPoint.Win.Spread.FpSpread fprSpr, int nStartRowCount, int nMaxRowCount)
        {
            int j = 0;

            for (int i = nStartRowCount; i < nMaxRowCount; i++)
            {
                if (fprSpr.ActiveSheet.Cells[i, 2].Text.Trim() != "")
                {
                    j += 1;
                }
                else
                {
                    break;
                }
            }
            return j;
        }

        /// <summary>
        /// 처방 수술환자 선택 화면에서 사용(외래 수술인데 재원중인지 확인)
        /// </summary>
        /// <param name="ArgPano"></param>
        /// <param name="argOpDate"></param>
        /// <returns></returns>
        public bool OPD_SUSUL_Check(PsmhDb pDbCon, string ArgPano, string argOpDate)
        {
            bool blnValue;
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            blnValue = true;

            try
            {
                SQL = "";
                SQL += " SELECT TO_CHAR(InDate,'YYYY-MM-DD') InDate,TO_CHAR(OutDate,'YYYY-MM-DD') OutDate   \r";
                SQL += "   FROM ADMIN.IPD_NEW_MASTER                                                  \r";
                SQL += "  WHERE PANO = '" + ArgPano + "'                                                    \r";
                SQL += "    AND TO_CHAR(InDate,'YYYY-MM-DD') <= '" + argOpDate + "'                         \r";
                SQL += "    AND (OutDate IS NULL OR OutDate  >= TO_DATE('" + argOpDate + "','YYYY-MM-DD'))  \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return true;
                }
                if (reader.HasRows)
                { 
                    blnValue = false;
                }

                reader.Dispose();
                reader = null;

                return blnValue;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return blnValue;
            }
        }

        public string Read_Nur_Master_Powder(PsmhDb pDbCon, long nIpdNo, string strPano, string strErInDate)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            SQL = "";
            SQL += " SELECT ROWID                       \r";
            SQL += "   FROM ADMIN.NUR_MASTER      \r";
            SQL += "  WHERE IPDNO = '" + nIpdNo + "'    \r";
            SQL += "    AND GbPOWDER = 'Y'              \r";

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return "";
            }

            if (reader.HasRows)
            {
                rtnVal = "OK";
            }

            reader.Dispose();
            reader = null;

            if (strPano.NotEmpty())
            {
                SQL = "";
                SQL += " SELECT ROWID                                                               \r";
                SQL += "   FROM ADMIN.NUR_MASTER                                              \r";
                SQL += "  WHERE Pano = '" + strPano + "'                                            \r";
                SQL += "    AND TRUNC(WARDINDate) >= TO_DATE('" + strErInDate + "','YYYY-MM-DD')    \r";
                SQL += "    AND (OUTDATE IS NULL OR OUTDATE = '')                                   \r";
                SQL += "    AND GbPOWDER = 'Y'                                                      \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (reader.HasRows)
                {
                    rtnVal = "OK";
                }

                reader.Dispose();
                reader = null;
            }

            return rtnVal;
        }

        /// <summary>
        /// 외래 마스트에서 Power 여부를 읽는다
        /// 2018-12-25 박웅규
        /// Vb_OCS_Order1.bas  READ_OPD_MASTER_POWDER
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPano"></param>
        /// <param name="strBDate"></param>
        /// <param name="strDeptCode"></param>
        /// <returns></returns>
        public string Read_Opd_Master_Powder(PsmhDb pDbCon, string strPano, string strBDate, string strDeptCode)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";

            OracleDataReader reader = null;

            //if (string.Compare(strBDate, "2019-01-01") < 0) return "";

            SQL = "";
            SQL += " SELECT ROWID                       \r";
            SQL += "   FROM ADMIN.OPD_MASTER      \r";
            SQL += "  WHERE PANO = '" + strPano + "'    \r";
            SQL += "    AND BDATE = TO_DATE('" + strBDate + "', 'YYYY-MM-DD')    \r";
            SQL += "    AND DEPTCODE = '" + strDeptCode + "'    \r";
            SQL += "    AND GbPOWDER = 'Y'              \r";

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return "";
            }

            if (reader.HasRows)
            {
                rtnVal = "OK";
            }

            reader.Dispose();
            reader = null;

            return rtnVal;
        }

        /// <summary>
        /// 10세 이하 파우더 자동 체크
        /// 2018-12-25 박웅규
        /// Vb_OCS_Order1.bas  READ_POWDER_AUTO_CHK
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPano"></param>
        /// <param name="strBDate"></param>
        /// <param name="intAge"></param>
        /// <returns></returns>
        public string Read_Powder_Auto_Chk(PsmhDb pDbCon, string strPano, string strBDate, int intAge)
        {
            string rtnVal = "";

            if (string.Compare(strBDate, "2019-01-01") < 0) return "";

            //【소아환자 Powder 기본 체크 설정 나이 : 기본 10】
            string strEmrOption = EmrGetUserOption_Ord(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "POWDERAGE");
            int intAgeTmp = 10;

            if (strEmrOption != "")
            {
                intAgeTmp = (int)VB.Val(strEmrOption);
            }

            if (intAge <= intAgeTmp)
            {
                rtnVal = "OK";
            }

            return rtnVal;
        }

        /// <summary>
        /// EMR 사용자 옵션 쿼리
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strUseId"></param>
        /// <param name="strOPTCD"></param>
        /// <param name="strOPTGB"></param>
        /// <returns></returns>
        public static string EmrGetUserOption_Ord(PsmhDb pDbCon, string strUseId, string strOPTCD, string strOPTGB)
        {
            OracleDataReader reader = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVal = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT OPTVALUE ";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.EMRUSEROPTION";
            SQL = SQL + ComNum.VBLF + "WHERE USEID = '" + strUseId + "'";
            SQL = SQL + ComNum.VBLF + "  AND OPTCD = '" + strOPTCD + "'";
            SQL = SQL + ComNum.VBLF + "  AND OPTGB = '" + strOPTGB + "'";

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }
            reader.Dispose();
            reader = null;
            return rtnVal;
        }

        public void fn_ClearMemory(Form FormName)
        {
            if (FormName != null)
            {
                FormName.Dispose();
                FormName = null;
            }
            clsApi.FlushMemory();
        }

        /// <summary>
        /// Description : 응급실 KTAS LEVEL (RTN_KTAS_LEVEL)
        /// Author : 이상훈
        /// Create Date : 2018.03.13
        /// <param name="sPano"></param>
        /// <param name="sBDate"></param>
        /// </summary>
        /// <seealso cref=""/>
        public string fn_RTN_KTAS_LEVEL(PsmhDb pDbCon, string sPano, string sBDate)
        {
            string strRtn = "";

            ComFunc.ReadSysDate(clsDB.DbCon);
            string SQL = "";
            string SqlErr = "";

            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT /*+ INDEX(NUR_ER_PATIENT index_nurerpatient4) */MIN(KTASLEVL) KTASLEVL  \r";
                SQL += "   FROM ADMIN.NUR_ER_PATIENT                                              \r";
                SQL += "  WHERE PANO   = '" + sPano + "'                                                \r";
                SQL += "    AND JDATE >= TO_DATE('" + sBDate + "','YYYY-MM-DD')                         \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog("함수명 : " + "fn_RTN_KTAS_LEVEL " + ComNum.VBLF + SqlErr, SQL, pDbCon);
                    return strRtn;
                }

                if (reader.HasRows)
                {
                    strRtn = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                reader = null;
                return strRtn;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox("함수명 : " + "fn_RTN_KTAS_LEVEL " + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "fn_RTN_KTAS_LEVEL " + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                return strRtn;
            }
        }

        /// <summary>
        /// CONSULT 미처리 건수
        /// </summary>
        /// <param name="txt"></param>
        public void fn_Consult_Cnt(TextBox txt)
        {
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT COUNT(*)  CNT                                                                       \r";
                SQL += "   FROM ADMIN.OCS_ITRANSFER   A                                                        \r";
                SQL += "      , ADMIN.IPD_NEW_MASTER B                                                        \r";
                SQL += "  WHERE A.ToDeptCode = '" + clsPublic.GstrDeptCode + "'                                     \r";
                if (clsPublic.GstrDeptCode == "PC" || clsPublic.GstrDeptCode == "EN" || clsPublic.GstrDeptCode == "OT")
                {
                    SQL += "    AND A.TODRCODE IN ('" + clsOrdFunction.GstrDrCode_N + "', '" + VB.Left(clsOrdFunction.GstrDrCode_N, 2) + "99" + "')  \r";
                }
                else if(clsOrdFunction.GstrDrCode_N.Equals("0402"))
                {
                    SQL += "    AND A.TODRCODE IN ('0401', '" + clsOrdFunction.GstrDrCode_N + "')  \r";
                }
                else
                {
                    SQL += "    AND A.TODRCODE = '" + clsOrdFunction.GstrDrCode_N + "'                              \r";
                }
                SQL += "    AND A.GbDEL    <> '*'                                                                   \r";
                SQL += "    AND A.GbFlag    = '1'                                                                   \r";
                SQL += "    AND ( A.GBCONFIRM     = ' ' OR A.GBCONFIRM     = 'T' OR A.GBCONFIRM IS NULL)            \r"; //미처리
                SQL += "    AND A.Ptno      = B.Pano                                                                \r";
                SQL += "    AND B.GBSTS IN ( '0','1','2','3','4')                                                   \r";
                SQL += "    AND A.BDATE >= TO_DATE('2008-07-01','YYYY-MM-DD')                                       \r";
                SQL += "    AND A.BDATE <= TRUNC(SYSDATE)                                                           \r";
                SQL += "    AND A.GBSEND <> '*'                                                                     \r";
                SQL += "  ORDER BY B.RoomCode, A.FrDeptCode                                                         \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    txt.Text = reader.GetValue(0).ToString().Trim();
                }
                else
                {
                    txt.Text = "0";
                }
                reader.Dispose();
                reader = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// 진료회신 미처리 건수
        /// </summary>
        /// <param name="txt"></param>
        public void fn_JinReturn_Cnt(TextBox txt)
        {
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT COUNT(Pano) CNT                                                     \r";
                SQL += "   FROM ADMIN.ETC_RETURN     b                                        \r";
                SQL += "      , ADMIN.OCS_MCCERTIFI12 a                                        \r";
                SQL += "  WHERE b.Pano = a.Ptno(+)                                                  \r";
                SQL += "    AND b.Actdate >= TRUNC(SYSDATE - 31)                                    \r";
                SQL += "    AND b.Actdate = a.BDate(+)                                              \r";
                SQL += "    AND b.DrCode IN ('" + fn_ADD_DRCODE(clsOrdFunction.GstrDrCode_N) + "')  \r";
                SQL += "    AND ( b.GbSend ='Y' OR  b.GbSend ='' )                                  \r";
                SQL += "    AND ( a.Opinion IS NULL OR  a.Opinion ='' )                             \r";  //미처리    
                SQL += "    AND (b.Agree IS NULL OR b.Agree ='Y')                                   \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    txt.Text = reader.GetValue(0).ToString();
                }
                else
                {
                    txt.Text = "0";
                }
                reader.Dispose();
                reader = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// 포스코 미처리 건수
        /// </summary>
        /// <param name="txt"></param>
        public void fn_Posco_Cnt(TextBox txt)
        {
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT COUNT(Ptno) CNT                                 \r";
                SQL += "   FROM ADMIN.OCS_MCCERTIFI28                      \r";
                SQL += "  WHERE JDate >= TRUNC(SYSDATE - 180)                   \r";
                SQL += "    AND DrCode IN ('" + fn_ADD_DRCODE(clsOrdFunction.GstrDrCode) + "')  \r";
                SQL += "    AND ( Result_Date IS NULL OR Result_Date = '')      \r";  //미처리

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    txt.Text = reader.GetValue(0).ToString();
                }
                else
                {
                    txt.Text = "0";
                }
                reader.Dispose();
                reader = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// 사본발급신청 미처리
        /// </summary>
        /// <param name="txt"></param>
        public void fn_Read_MCCopy_Cnt(TextBox txt)
        {
            string strDrDept = "";
            string strDeptCode = "";
            string strDrCd = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += " SELECT DEPTCODE                        \r";
                SQL += "   FROM ADMIN.BAS_CLINICDEPT      \r";
                SQL += "  WHERE DEPTCODE LIKE 'M%'              \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strDeptCode += "'" + dt.Rows[i]["DEPTCODE"].ToString() + "',";
                    }
                    strDeptCode = VB.Mid(strDeptCode, 1, strDeptCode.Length - 1);
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL += " SELECT DRCODE, DEPTCODE, GRADE                 \r";
                SQL += "   FROM ADMIN.OCS_DOCTOR                   \r";
                SQL += "  WHERE DOCCODE = " + clsPublic.GnJobSabun + "  \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strDrCd = dt.Rows[0]["DRCODE"].ToString().Trim();
                    if (dt.Rows[0]["GRADE"].ToString().Trim() == "1")
                    {
                        strDrDept = "";
                    }
                    else
                    {
                        strDrDept = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL += " SELECT LSDATE, PTNO, DEPTCODE, DRCODE, ROWID   \r";
                SQL += "   FROM ADMIN.OCS_MCCERTIFI_REQUEST        \r";
                SQL += "  WHERE SIGNDATE IS NULL                        \r";
                switch (strDrDept)
                {
                    case "MD":
                        SQL += "   AND DEPTCODE IN (" + strDeptCode + ")    \r";
                        break;
                    case "OS":
                    case "GS":
                        SQL += "   AND DEPTCODE = '" + strDrDept + "'       \r";
                        break;
                    default:
                        SQL += "   AND DRCODE = '" + strDrCd + "'           \r";
                        break;
                }
                SQL += "    AND WRITEDATE >= TO_DATE('" + DateTime.Parse(clsPublic.GstrSysDate).AddDays(-60).ToShortDateString() + " 00:00','YYYY-MM-DD HH24:MI') \r";
                SQL += "  ORDER BY LSDATE DESC, DEPTCODE, DRCODE            \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txt.Text = dt.Rows.Count.ToString();
                }
                else
                {
                    txt.Text = "0";
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// 격리신청 미처리 건수
        /// </summary>
        public void fn_Read_isolate_Cnt(TextBox txt)
        {
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT COUNT(*)  CNT                                                                       \r";
                SQL += "   FROM ADMIN.OCS_ITRANSFER_KEK A                                                      \r";
                SQL += "      , ADMIN.IPD_NEW_MASTER   B                                                      \r";
                SQL += "  WHERE A.ToDeptCode = '" + clsPublic.GstrDeptCode + "'                                     \r";
                if (clsPublic.GstrDeptCode == "PC")
                {
                    SQL += "    AND A.TODRCODE IN ( '" + clsOrdFunction.GstrDrCode_N + "', '" + VB.Left(clsOrdFunction.GstrDrCode_N, 2) + "99" + "'  )  \r";
                }
                else
                {
                    SQL += "    AND A.TODRCODE = '" + clsOrdFunction.GstrDrCode_N + "'                              \r";
                }
                SQL += "    AND A.GbDEL    <> '*'                                                                   \r";
                SQL += "    AND ( A.CONFIRM     = ' ' OR A.CONFIRM     = 'T' OR A.CONFIRM IS NULL)                  \r"; //미처리
                SQL += "    AND A.Ptno      = B.Pano                                                                \r";
                SQL += "    AND B.GBSTS IN ( '0','1','2','3','4')                                                   \r";
                SQL += "    AND A.BDATE >= TO_DATE('" + DateTime.Parse(clsOrdFunction.GstrBDate).AddDays(-90).ToShortDateString() + "','YYYY-MM-DD')    \r";
                SQL += "    AND A.BDATE <= TRUNC(SYSDATE)                                                           \r";
                SQL += "  ORDER BY B.RoomCode, A.FrDeptCode                                                         \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    txt.Text = reader.GetValue(0).ToString();
                }
                else
                {
                    txt.Text = "0";
                }
                reader.Dispose();
                reader = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        public string fn_ADD_DRCODE(string strDrCode)
        {
            string strRtn = "";

            switch (strDrCode)
            {
                case "0115":
                    strRtn = "0101', '0115";
                    break;
                case "0103":
                    strRtn = "1123', '0103";
                    break;
                case "0301":
                    strRtn = "1122', '0301";
                    break;
                case "0501":
                    strRtn = "1108', '0501";
                    break;
                case "0104":
                    strRtn = "1109', '0104";
                    break;
                case "0101":
                    strRtn = "1104', '0101";
                    break;
                case "0901":
                    strRtn = "1107', '0901";
                    break;
                case "0201":
                    strRtn = "1126', '0201";
                    break;
                case "0401":
                    strRtn = "1102', '0401', '0402";
                    break;
                case "0402":
                    strRtn = "1102', '0401', '0402";
                    break;
                case "0202":
                    strRtn = "1127', '0202";
                    break;
                case "0102":
                    strRtn = "1114', '0102";
                    break;
                case "0902":
                    strRtn = "1125', '0902";
                    break;
                default:
                    strRtn = strDrCode;
                    break;
            }

            return strRtn;
        }

        /// <summary>
        /// CHK_주진단_금지상병
        /// </summary>
        /// <param name="sDiagCode"></param>
        /// <returns></returns>
        public bool CHK_MAINDIAGNOSYS_NO(PsmhDb pDbCon, string sDiagCode)
        {
            bool blRtn = false;
            string sMsg = "";
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT CODE                                \r";
                SQL += "   FROM ADMIN.BAS_BCODE               \r";
                SQL += "  WHERE GUBUN = 'OCS_주진단금지상병'           \r";
                SQL += "    AND CODE = '" + sDiagCode.Trim() + "'   \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return blRtn;
                }

                if (reader.HasRows)
                {
                    blRtn = true;
                    sMsg = "";
                    sMsg += "★현재 주진단으로 입력하신 질병코드는" + "\r\n";
                    sMsg += "  주진단으로 사용할 수 없는 질병코드입니다." + "\r\n";
                    sMsg += " ※ 문의 : 보험심사과";
                    MessageBox.Show(sMsg, "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                reader.Dispose();
                reader = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }

            return blRtn;
        }

        public string Msg001()
        {
            string rtnVal = "";

            string strDurgCntMsg = "";

            strDurgCntMsg = " [ 1종 이상 처방 되었습니다.  처방 불가 ]   " + "\r\n\r\n";
            strDurgCntMsg += " 약제: PTF. SAMI. SAM30 " + "\r\n";
            strDurgCntMsg += " 뇌혈류 개선제는 1종이상 처방 불가합니다." + "\r\n\r\n";
            strDurgCntMsg += " *****************************************************" + "\r\n";
            strDurgCntMsg += " 기타 문의사항 : 심사과 내선(8031) 문의 바랍니다. " + "\r\n";

            rtnVal = strDurgCntMsg;

            return rtnVal;
        }

        public string Msg0002()
        {
            string rtnVal = "";

            string strDurgCntMsg = "";

            strDurgCntMsg = " [ 1종 이상 처방 되었습니다.  처방 불가 ]   " + "\r\n\r\n";
            strDurgCntMsg += " 약제: DUXIL. ATERO.PIRXAN.COMEL. NIMOV. KET10.NIMO30. NIMOV10 BRIL90 " + "\r\n";
            strDurgCntMsg += " 뇌증상 개선제는 1종이상 처방 불가합니다." + "\r\n\r\n";
            strDurgCntMsg += " *****************************************************" + "\r\n";
            strDurgCntMsg += " 기타 문의사항 : 심사과 내선(8031) 문의 바랍니다." + "\r\n";

            rtnVal = strDurgCntMsg;

            return rtnVal;
        }

        public string Msg0003()
        {
            string rtnVal = "";

            string strDurgCntMsg = "";

            strDurgCntMsg = " Aspirin 과 항혈전제 병용투여시 아래 고위험에서만 보험됩니다." + "\r\n";
            strDurgCntMsg += "Aspirin 약제: ASA1/ASA100/ASA5/ASP1/AST100" + "\r\n";
            strDurgCntMsg += "항혈전  약제: CSZ/CSZ1/SEREN/CLOAT/PLAVI/PLA75/PILGREL/PIDOGL/IBUST/DIS300/SAL/MESOC/" + "\r\n";
            strDurgCntMsg += "              ATERO/VESEL/UCRID/BERAST/BERA/SARP/DUXIL/ANPRAN/EFF10/EFF5/SUPERPY/PLAVI-A/CLOSONE" + "\r\n";
            strDurgCntMsg += " 해당 상병코드를 등록해주세요" + "\r\n\r\n";
            strDurgCntMsg += " - 고위험군 -" + "\r\n";
            strDurgCntMsg += "  ST 분절 상승 심근경색증 - 병명: I219" + "\r\n";
            strDurgCntMsg += "  급성관상동맥증후군      - 병명: I200" + "\r\n";
            strDurgCntMsg += "  재발성 또는 중증 뇌졸증 - 병명: I609/I619/I629/I639" + "\r\n";
            strDurgCntMsg += "  STENT 삽입 환자         - 병명: Z958" + "\r\n";
            strDurgCntMsg += "  심방세동 고위험군에서 와파린 사용할 수 없는경우 - 병명: I480" + "\r\n";
            strDurgCntMsg += " *****************************************************" + "\r\n";
            strDurgCntMsg += " 기타 문의사항 : 심사과 내선(8031) 문의 바랍니다. " + "\r\n";

            rtnVal = strDurgCntMsg;

            return rtnVal;
        }

        public string Msg0004()
        {
            string rtnVal = "";

            string strDurgCntMsg = "";

            strDurgCntMsg = " Aspirin 과 항혈전제 병용투여시 아래 고위험에서만 보험됩니다." + "\r\n";
            strDurgCntMsg += "Aspirin 약제: ASA1/ASA100/ASA5/ASP1/AST100" + "\r\n";
            strDurgCntMsg += "항혈전  약제: CSZ/CSZ1/SEREN/CLOAT/PLAVI/PLA75/PILGREL/PIDOGL/IBUST/DIS300/SAL/MESOC/" + "\r\n";
            strDurgCntMsg += "              ATERO/VESEL/UCRID/BERAST/BERA/SARP/DUXIL/ANPRAN/EFF10/EFF5/SUPERPY/PLAVI-A/CLOSONE" + "\r\n";
            strDurgCntMsg += " 해당 상병코드를 등록해주세요" + "\r\n\r\n";
            strDurgCntMsg += " - 고위험군 -" + "\r\n";
            strDurgCntMsg += "  ST 분절 상승 심근경색증 - 병명: I219" + "\r\n";
            strDurgCntMsg += "  급성관상동맥증후군      - 병명: I200" + "\r\n";
            strDurgCntMsg += "  STENT 삽입 환자         - 병명: Z958" + "\r\n";
            strDurgCntMsg += " *****************************************************" + "\r\n";
            strDurgCntMsg += " 기타 문의사항 : 심사과 내선(8031) 문의 바랍니다." + "\r\n";

            rtnVal = strDurgCntMsg;

            return rtnVal;
        }

        //2019-07-12 심사팀 이민주 요청, 팝업메세지 신규로 변경함.
        public string Msg0005()
        {
            string rtnVal = "";
            string strDurgCntMsg = "";

            strDurgCntMsg += "Aspirin과 항혈전제 병용투여시 아래 고위험군만 보험됩니다...                          " + "\r\n";
            strDurgCntMsg += "※ Aspirin약제: ASA1,ASA100,ASA5,ASP1,AST100 등                                      " + "\r\n";
            strDurgCntMsg += "※ Cilostazol약제: CSZ,CSZ1,LINEXIN,CQSTA,CSZ1SR,CSZ2SR,CILO200 등                   " + "\r\n";
            strDurgCntMsg += "※ Clopidogrel약제: SEREN,CLOAT,PLAVI,PLA75,SUPERPY,CLOSONE,PLAVI - A,               " + "\r\n";
            strDurgCntMsg += "                    PIDOGL,PILGREL,PREGREL 등                                        " + "\r\n";
            strDurgCntMsg += "※ 그외 항혈전제 : ATERO,IBUST,DIS300,SAL,MESOC,VESEL,UCRID,BERAST,BERA,             " + "\r\n";
            strDurgCntMsg += "                  SARP,TICO,EFF10,EFF5,BRIL60,BRIL90,ANPLONE,ANP - SR,               " + "\r\n";
            strDurgCntMsg += "                  SAPOG 등                                                           " + "\r\n";
            strDurgCntMsg += "                                                                                     " + "\r\n";
            strDurgCntMsg += "1.Aspirin약제 포함한 2제 병용 요법시: 심혈관 질환ㆍ뇌혈관질환ㆍ말초동맥성 질환 중    " + "\r\n";
            strDurgCntMsg += "   ST분절 상승 심근경색증, 급성관상동맥증후군, 재발성 뇌졸중, 중증 뇌졸중, 스텐트    " + "\r\n";
            strDurgCntMsg += "   (Stent) 삽입환자                                                                  " + "\r\n";
            strDurgCntMsg += "   => 병명:I210~I214,I219,I200,I201,I2080,I2088,I209,Z958,I609,I619,I629,I639,       " + "\r\n";
            strDurgCntMsg += "          I7022,I7023,I7024,I7025,I7029,I70990,I70991,                               " + "\r\n";
            strDurgCntMsg += "                                                                                     " + "\r\n";
            strDurgCntMsg += "2. 3제 요법(Aspirin + Cilostazol + Clopidogrel)시: 관상동맥 스텐트 시술한 경우로서   " + "\r\n";
            strDurgCntMsg += "   당뇨병 환자의 재협착 방지, 재협착 병변환자, 다혈관 협착으로 다수의 스텐트를 시술  " + "\r\n";
            strDurgCntMsg += "   (Multiple - stenting)한 환자                                                      " + "\r\n";
            strDurgCntMsg += "     => 병명:I210~I214,I219,I200,I201,I2080,I2088,I209,Z958,                         " + "\r\n";
            strDurgCntMsg += "                                                                                     " + "\r\n";
            strDurgCntMsg += "3.Clopidogrel과 Aspirin의 병용요법:심방세동 환자 중 고위험군에서 와파린을 사용할     " + "\r\n";
            strDurgCntMsg += "   수 없는 경우: 와파린에 과민반응, 금기, 국제정상화비율(INR)조절실패 등             " + "\r\n";
            strDurgCntMsg += "   ◎고위험군 기준                                                                   " + "\r\n";
            strDurgCntMsg += "      : 뇌졸중, 일과성허혈발작, 혈전색전증의 과거력이 있거나 75세 이상 환자          " + "\r\n";
            strDurgCntMsg += "      : 6가지 위험인자(심부전, 고혈압, 당뇨, 혈관성질환, 65 - 74세, 여성) 중 2가지 이" + "\r\n";
            strDurgCntMsg += "                                                                                     " + "\r\n";
            strDurgCntMsg += "      상의 조건을 가지고 있는 환자                                                   " + "\r\n";
            strDurgCntMsg += "    => 병명:I480                                                                     " + "\r\n";
            strDurgCntMsg += "                                                                                     " + "\r\n";
            strDurgCntMsg += "4.간헐성 파행을 동반한 만성동맥폐색증(버거씨병, 폐색성 동맥경화증, 당뇨병성 말초     " + "\r\n";
            strDurgCntMsg += "                                                                                     " + "\r\n";
            strDurgCntMsg += " 혈관병증 등)으로 Cilostazol 투여 하면서 다른 항혈전제를 병용하여 2제 요법시         " + "\r\n";
            strDurgCntMsg += "  각각의 기준으로 처방                                                               " + "\r\n";
            strDurgCntMsg += "   => 병명:I7022,E1050,E1150,E1250,E1350,E1450 ,I731 + I739 와 다른 항혈전제         " + "\r\n";
            strDurgCntMsg += "             상병 동시기재                                                           " + "\r\n";
            strDurgCntMsg += "                                                                                     " + "\r\n";
            strDurgCntMsg += "◈◈ 각각의 상병 입력해 주시면 처방전달 가능합니다                                   " + "\r\n";
            strDurgCntMsg += " ************************************************************************************" + "\r\n";
            strDurgCntMsg += " 기타 문의사항 : 심사과 내선(8031) 문의 바랍니다.                                    " + "\r\n";

            rtnVal = strDurgCntMsg;

            return rtnVal;
        }

        public string READ_IPD_BED_NUMBER(string strWard, string strCode)
        {
            string SQL = "";
            OracleDataReader reader = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     Name";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'NUR_ICU_침상번호'";
                SQL = SQL + ComNum.VBLF + "         AND TRIM(CODE) = '" + strCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND (DELDATE IS NULL OR DELDATE = '') ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "READ_IPD_BED_NUMBER" + ComNum.VBLF + SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Dispose();
                    reader = null;
                }

                ComFunc.MsgBox("함수명 : " + "READ_IPD_BED_NUMBER" + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "READ_IPD_BED_NUMBER" + ComNum.VBLF + ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// 항생제 대상체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSS"></param>
        /// <param name="argROW"></param>
        /// <param name="argForm"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgInDate"></param>
        /// <param name="ArgBDate"></param>
        /// <returns></returns>
        public long CHK_HYANG_MST_USE(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread ArgSS, int argROW, Form argForm, string ArgPano, string ArgInDate, string ArgBDate, int nSugaCol)
        {
            long rtnVal = 0;
            int nDay;

            for (int i = argROW; i < ArgSS.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (ArgSS.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    //수가코드체크
                    if (CHK_HYANG_SuCode_USE(pDbCon, ArgSS.ActiveSheet.Cells[argROW, nSugaCol].Text.Trim()) == "OK")
                    {
                        nDay = CHK_HYANG_MST_DAY(pDbCon, ArgPano, ArgInDate, ArgBDate, ArgSS.ActiveSheet.Cells[argROW, nSugaCol].Text.Trim());

                        #region 전산업무 의뢰서 2021-334
                        if (nDay == 15 || nDay == 28)
                        {
                            clsOrdFunction.Gstr항생제POP = "수가코드 [" + ArgSS.ActiveSheet.Cells[argROW, nSugaCol].Text.Trim() + "] " + nDay + "일 사용중!!";

                            //clsOrderEtc.LONG_ANTI_USED_SMS(clsDB.DbCon, ArgPano, nDay, ArgSS.ActiveSheet.Cells[argROW, nSugaCol].Text.Trim());
                            argForm.ShowDialog();
                        }
                        else if(nDay > 15 && (nDay % 3 == 0))
                        {
                            clsOrdFunction.Gstr항생제POP = "수가코드 [" + ArgSS.ActiveSheet.Cells[argROW, nSugaCol].Text.Trim() + "] " + nDay + "일 사용중!!";
                            ComFunc.MsgBox(clsOrdFunction.Gstr항생제POP);
                            //clsOrderEtc.LONG_ANTI_USED_SMS(clsDB.DbCon, ArgPano, nDay, ArgSS.ActiveSheet.Cells[argROW, nSugaCol].Text.Trim());
                            //argForm.ShowDialog();
                        }
                        #endregion
                    }

                    if (clsOrdFunction.Gstr항생제POP == "NO")
                    {
                        return rtnVal;
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 항생제 대상코드 일수 체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgInDate"></param>
        /// <param name="ArgBDate"></param>
        /// <param name="ArgSuCode"></param>
        /// <returns></returns>
        public int CHK_HYANG_MST_DAY(PsmhDb pDbCon, string ArgPano, string ArgInDate, string ArgBDate, string ArgSuCode)
        {
            int rtnVal = 0;

            string strMinDate;
            string strLastCDate = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt2 = null;

            try
            {
                SQL = "";
                SQL += " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE                       \r";
                SQL += "   FROM ADMIN.BAS_PATIENT_ANTI_MST                        \r";
                SQL += "  WHERE Pano = '" + ArgPano + "'                                \r";
                SQL += "    AND BDATE >= TO_DATE('" + ArgInDate + "','YYYY-MM-DD')      \r"; //입원시점부터
                SQL += "    AND SuNext = '" + ArgSuCode + "'                            \r";
                SQL += "  ORDER BY BDATE DESC                                           \r";
                SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog("함수명 : " + "CHK_HYANG_MST_DAY" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt2.Rows.Count > 0)
                {
                    strLastCDate = dt2.Rows[0]["BDATE"].ToString().Trim();   //확인최종일
                }

                dt2.Dispose();
                dt2 = null;

                //당일 확인건
                SQL = "";
                SQL += " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE                               \r";
                SQL += "   FROM ADMIN.BAS_PATIENT_ANTI_MST                                \r";
                SQL += "  WHERE Pano = '" + ArgPano + "'                                        \r";
                SQL += "    AND BDATE = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')   \r"; //확인건 있으면 패스함
                SQL += "    AND SuNext = '" + ArgSuCode + "'                                    \r";
                SQL += "  ORDER BY BDATE                                                         \r";
                SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog("함수명 : " + "CHK_HYANG_MST_DAY" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt2.Rows.Count > 0)
                {
                    dt2.Dispose();
                    dt2 = null;
                    return rtnVal;
                }

                dt2.Dispose();
                dt2 = null;

                //BDATE가 gstrsysdate 같거나 작아야함 - 그렇지않으면 미래처방임
                if (DateTime.Parse(ArgBDate) > DateTime.Parse(clsPublic.GstrSysDate))
                {
                    return rtnVal;
                }

                //수가체크
                SQL = "";
                SQL += " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE                   \r";
                SQL += "   FROM ADMIN.BAS_PATIENT_ANTI                        \r";
                SQL += "  WHERE Pano = '" + ArgPano + "'                            \r";
                SQL += "    AND BDATE >= TO_DATE('" + ArgInDate + "','YYYY-MM-DD')  \r";
                SQL += "    AND BDATE <= TO_DATE('" + ArgBDate + "','YYYY-MM-DD')   \r";
                SQL += "    AND SuNext = '" + ArgSuCode + "'                        \r";
                SQL += "  ORDER BY BDATE                                            \r";
                SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog("함수명 : " + "CHK_HYANG_MST_DAY" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt2.Rows.Count > 0)
                {
                    strMinDate = dt2.Rows[0]["BDATE"].ToString().Trim();
                    if (strLastCDate != "")
                    {
                        strMinDate = strLastCDate;  //최종확인일자 재설정해줌
                    }
                    rtnVal = CF.DATE_ILSU(clsDB.DbCon, ArgBDate, strMinDate) + 1;
                }

                dt2.Dispose();
                dt2 = null;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox("함수명 : " + "CHK_HYANG_MST_DAY" + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "CHK_HYANG_MST_DAY" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
            }

            return rtnVal;
        }

        public string CHK_HYANG_SuCode_USE(PsmhDb pDbCon, string ArgSuCode)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  CODE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL += ComNum.VBLF + "WHERE GUBUN ='OCS_장기항생제코드' AND (DELDATE IS NULL OR DELDATE ='')";
            SQL += ComNum.VBLF + "AND TRIM(CODE) = '" + ArgSuCode + "'";

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if(reader.HasRows)
            {
                rtnVal = "OK";
            }

            if(rtnVal != "OK" && ArgSuCode.Left(1).Equals("W"))
            {
                rtnVal = "OK";
            }

            reader.Dispose();
            reader = null;

            return rtnVal;
        }

        public static string fn_Data_Send(PsmhDb pDbCon, string sPtNo, string sSName, string sSex, int nAge, string sBi, string sGbSpc, string sWardCode, string sEntDate)
        {
            //병동 OCS이외의 Program에서 처리시 주위사항 : ADMIN.OCS_IORDER의 GbSend에는 '*' Setting할 것
            //Global Argment GstrActDate, PAT.Sname, PAT.Sex, PAT.Age
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string rtnVal = "";

            int nCNT;
            int nLab;
            string strOK;
            string strPtNo;
            string strSName;
            string strSex;
            string strBi;
            int Age = 0;
            string strBun;
            string strItemCd;

            strPtNo = sPtNo;
            strSName = sSName;
            strSex = sSex;
            Age = nAge;
            strBi = sBi;
            rtnVal = "OK";

            string[] strAills = new string[6];

            try
            {
                SQL = "";
                SQL += " SELECT IllCode                                                                                     \r";
                SQL += "   FROM ADMIN.OCS_IILLS                                                                        \r";
                SQL += "  WHERE Ptno    = '" + strPtNo.Trim() + "'                                                          \r";
                SQL += "    AND EntDate = TO_DATE('" + DateTime.Parse(sEntDate).ToShortDateString() + "','YYYY-MM-DD')      \r";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "fn_Data_Send" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < (dt.Rows.Count > 6 ? 6 : dt.Rows.Count); i++)
                    {
                        strAills[i] = dt.Rows[i]["ILLCODE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL += " UPDATE ADMIN.IPD_TRANS Set                                   \r";
                SQL += "        IllCode1 = '" + strAills[0] + "'                            \r";
                SQL += "      , IllCode2 = '" + strAills[1] + "'                            \r";
                SQL += "      , IllCode3 = '" + strAills[2] + "'                            \r";
                SQL += "      , IllCode4 = '" + strAills[3] + "'                            \r";
                SQL += "      , IllCode5 = '" + strAills[4] + "'                            \r";
                SQL += "      , IllCode6 = '" + strAills[5] + "'                            \r";
                SQL += "  WHERE Pano     = '" + strPtNo + "'                                \r";
                SQL += "    AND TRSNO IN (                                                  \r";
                SQL += "                   SELECT TRSNO FROM ADMIN.IPD_NEW_MASTER     \r";
                SQL += "                    WHERE Pano    = '" + strPtNo + "'               \r";
                SQL += "                      AND GBSTS IN ('0','2')                        \r";
                SQL += "                      AND OUTDATE IS NULL)                          \r";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("함수명 : " + "fn_Data_Send" + ComNum.VBLF + SqlErr + " 처방 전송 Flag Error, 전산실 연락 요망", "처방입력작업이 취소됩니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "fn_Data_Send" + ComNum.VBLF + SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    rtnVal = "";
                }
                rtnVal = "OK";
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("함수명 : " + "fn_Data_Send" + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "fn_Data_Send" + ComNum.VBLF + ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }
        }

        /// <summary>
        /// READ_중증상병여부
        /// </summary>
        /// <param name="strills"></param>
        public bool fn_Read_SeriousIllsYN(string strPano, string strBDate)
        {
            bool rtnval = false;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += " SELECT A.ILLCODE                                           \r";
                SQL += "   FROM ADMIN.BAS_ILLS A, ADMIN.OCS_EILLS B      \r";
                SQL += "  WHERE A.ILLCODE = B.ILLCODE                               \r";
                SQL += "    AND B.BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD')  \r";
                SQL += "    AND B.PTNO = '" + strPano + "'                          \r";
                SQL += "    AND A.GBER = '*'                                        \r";
                SQL += "    AND ROWNUM = 1                                          \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog("함수명 : " + "fn_Read_SeriousIllsYN" + ComNum.VBLF + SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnval;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnval = true;
                }
                dt.Dispose();
                dt = null;
                return rtnval;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox("함수명 : " + "fn_Read_SeriousIllsYN" + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "fn_Read_SeriousIllsYN" + ComNum.VBLF + ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnval;
            }
        }

        /// <summary>
        /// READ_중증상병여부2
        /// </summary>
        /// <param name="strills"></param>
        public bool fn_Read_SeriousIllsYN_2(string strills)
        {
            bool rtnval = false;
            string SQL = "";
            string SqlErr = "";
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT ILLCODE                         \r";
                SQL += "  FROM ADMIN.BAS_ILLS             \r";
                SQL += "  WHERE ILLCODE = '" + strills + "'     \r";
                SQL += "    AND GBER = '*'                      \r";
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog("함수명 : " + "fn_Read_SeriousIllsYN_2" + ComNum.VBLF + SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnval;
                }

                if (reader.HasRows)
                {
                    rtnval = true;
                }
                reader.Dispose();
                reader = null;
                return rtnval;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox("함수명 : " + "fn_Read_SeriousIllsYN_2" + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "fn_Read_SeriousIllsYN_2" + ComNum.VBLF + ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnval;
            }
        }

        /// <summary>
        /// 전자인증
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strGbJob"></param>
        /// <param name="strBDate"></param>
        /// <param name="strPtno"></param>
        /// <param name="strDeptCode"></param>
        /// <param name="strDrCode"></param>
        public string fn_Cert_Order(PsmhDb pDbCon, string strGbJob, string strBDate, string strPtno, string strDeptCode = "", string strDrCode = "")
        {
            DataTable dtCert = null;
            string rtnVal = "NO";

            string strToiday;   // 퇴사구분
            double nCertno = 0;
            string strName;
            string strSid;
            string strPW;
            string strEMRData = string.Empty;
            string strHashdata = string.Empty;
            string strSignData = string.Empty;
            string strCertData = string.Empty;
            string strRowid = string.Empty;
            long nOrderNo;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int intRowAffected = 0; //변경된 Row 받는 변수

            string TBL_NAME = string.Empty;


            if (strPtno.Length == 8 && strPtno.Substring(0, 6).Equals("810000"))
            {
                return "OK";
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            try
            {
                #region // QUERY
                if (strGbJob == "OPD")
                {
                    SQL = "";
                    SQL += " SELECT PTNO, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, DEPTCODE, DRCODE, SEQNO,ORDERCODE,SUCODE,BUN            \r";
                    SQL += "      , SLIPNO,REALQTY, QTY,NAL,GBDIV,DOSCODE,GBBOTH                                                     \r";
                    SQL += "      , GBINFO,GBER,GBSELF,GBSPC,BI,DRCODE,REMARK, TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE         \r";
                    SQL += "      , GBSUNAP,TUYAKNO,ORDERNO,MULTI,MULTIREMARK,DUR,RESV                                               \r";
                    SQL += "      , SCODESAYU,SCODEREMARK,GBSEND,AUTO_SEND,RES ,GBSPC_NO,WRTNO,ROWID                                 \r";
                    SQL += "      , (SELECT MAX(SABUN) FROM ADMIN.OCS_DOCTOR WHERE DRCODE = A.DRCODE) AS SABUN                  \r";
                    SQL += "      , TUYEOPOINT, TUYEOTIME                                                                            \r";
                    SQL += "   FROM ADMIN.OCS_OORDER A                                                                          \r";
                    SQL += "  WHERE BDATE = TO_DATE('" + strBDate + "', 'YYYY-MM-DD')                                                \r";
                    SQL += "    AND PTNO = '" + strPtno + "'                                                                         \r";
                    SQL += "    AND DEPTCODE = '" + strDeptCode.Trim() + "'                                                          \r";
                    SQL += "    AND DRCODE = '" + strDrCode.Trim() + "'                                                              \r";
                    SQL += "    AND (CERTNO IS NULL OR CERTNO = 0)                                                                   \r";
                    SQL += "    AND SABUN IS NOT NULL                                                                               \r";
                }
                else if (strGbJob == "IPD")
                {
                    SQL = "";
                    SQL += " SELECT PTNO, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE    \r";
                    SQL += "      , BUN, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT      \r";
                    SQL += "      , GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBSEND, GBPOSITION, GBSTATUS, NURSEID                         \r";
                    SQL += "      , TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK           \r";
                    SQL += "      , TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, GBGROUP, GBPORT, ORDERSITE, MULTI, MULTIREMARK, POWDER    \r";
                    SQL += "      , VERBC,PRN_INS_GBN,PRN_INS_UNIT, TO_CHAR(PRN_INS_SDATE,'YYYY-MM-DD') PRN_INS_SDATE                \r";
                    SQL += "      , TO_CHAR(PRN_INS_EDATE,'YYYY-MM-DD') PRN_INS_EDATE                                                \r";
                    SQL += "      , POWDER_SAYU,PRN_INS_MAX,ASA , ROWID                                                              \r";
                    SQL += "      , TUYEOPOINT, TUYEOTIME                                                                            \r";
                    SQL += "   FROM ADMIN.OCS_IORDER                                                                            \r";
                    SQL += "  WHERE BDATE = TO_DATE('" + strBDate + "', 'YYYY-MM-DD')                                                \r";
                    SQL += "    AND PTNO = '" + strPtno + "'                                                                         \r";
                    SQL += "    AND DEPTCODE != 'ER'                                                                                 \r";
                    //SQL += "    AND DEPTCODE = '" + strDeptCode.Trim() + "'                                                          \r";
                    //SQL += "    AND DRCODE = '" + strDrCode.Trim() + "'                                                              \r";
                    SQL += "    AND (CERTNO IS NULL OR CERTNO = 0)                                                                   \r";
                }
                else if (strGbJob == "ER")
                {
                    SQL = "";
                    SQL += " SELECT PTNO, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE    \r";
                    SQL += "      , BUN, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT      \r";
                    SQL += "      , GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBSEND, GBPOSITION, GBSTATUS, NURSEID                         \r";
                    SQL += "      , TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK           \r";
                    SQL += "      , TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, GBGROUP, GBPORT, ORDERSITE, MULTI, MULTIREMARK, POWDER    \r";
                    SQL += "      , VERBC,PRN_INS_GBN,PRN_INS_UNIT, TO_CHAR(PRN_INS_SDATE,'YYYY-MM-DD') PRN_INS_SDATE                \r";
                    SQL += "      , TO_CHAR(PRN_INS_EDATE,'YYYY-MM-DD') PRN_INS_EDATE                                                \r";
                    SQL += "      , POWDER_SAYU,PRN_INS_MAX,ASA , ROWID                                                              \r";
                    SQL += "      , TUYEOPOINT, TUYEOTIME                                                                            \r";
                    SQL += "   FROM ADMIN.OCS_IORDER                                                                            \r";
                    SQL += "  WHERE BDATE = TO_DATE('" + strBDate + "', 'YYYY-MM-DD')                                                \r";
                    SQL += "    AND PTNO = '" + strPtno + "'                                                                         \r";
                    SQL += "    AND DEPTCODE = 'ER'                                                                                  \r";
                    //SQL += "    AND DRCODE = '" + strDrCode.Trim() + "'                                                              \r";
                    SQL += "    AND (CERTNO IS NULL OR CERTNO = 0)                                                                   \r";
                }
                else if (strGbJob == "VERBAL")
                {
                    SQL = "";
                    SQL += " SELECT PTNO, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE    \r";
                    SQL += "      , BUN, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT      \r";
                    SQL += "      , GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBSEND, GBPOSITION, GBSTATUS, NURSEID                         \r";
                    SQL += "      , TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK           \r";
                    SQL += "      , TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, GBGROUP, GBPORT, ORDERSITE, MULTI, MULTIREMARK, POWDER    \r";
                    SQL += "      , VERBC,PRN_INS_GBN,PRN_INS_UNIT, TO_CHAR(PRN_INS_SDATE,'YYYY-MM-DD') PRN_INS_SDATE                \r";
                    SQL += "      , TO_CHAR(PRN_INS_EDATE,'YYYY-MM-DD') PRN_INS_EDATE                                                \r";
                    SQL += "      , POWDER_SAYU,PRN_INS_MAX,ASA , ROWID                                                              \r";
                    SQL += "      , TUYEOPOINT, TUYEOTIME                                                                            \r";
                    SQL += "   FROM ADMIN.OCS_IORDER                                                                            \r";
                    SQL += "  WHERE BDATE = TO_DATE('" + strBDate + "', 'YYYY-MM-DD')                                                \r";
                    SQL += "    AND PTNO = '" + strPtno + "'                                                                         \r";
                    SQL += "    AND DEPTCODE = '" + strDeptCode.Trim() + "'                                                          \r";
                    SQL += "    AND DRCODE = '" + strDrCode.Trim() + "'                                                              \r";
                    SQL += "    AND CERTNO2 IS NULL                                                                                  \r";
                    SQL += "    AND VerbC = 'C'                                                                                      \r";                    
                }
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dtCert, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog("함수명 : " + "fn_Cert_Order" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dtCert.Rows.Count > 0)
                {
                    string SABUN = string.Empty;

                    for (int i = 0; i < dtCert.Rows.Count; i++)
                    {
                        
                        if (strGbJob == "OPD")
                        {
                            TBL_NAME = clsCertWork.OCS_OORDER;
                            SABUN = dtCert.Rows[i]["SABUN"].ToString().Trim().To<int>(0).ToString("00000");

                            #region // 외래 EMRDATA 생성
                            strRowid = dtCert.Rows[i]["ROWID"].ToString().Trim();
                            strDrCode = dtCert.Rows[i]["DRCODE"].ToString().Trim();
                            nOrderNo = long.Parse(dtCert.Rows[i]["ORDERNO"].ToString().Trim());
                            strEMRData = dtCert.Rows[i]["PTNO"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["BDATE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["SEQNO"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["ORDERCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["SUCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["BUN"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["SLIPNO"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["REALQTY"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["QTY"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["NAL"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBDIV"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["DOSCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBBOTH"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBINFO"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBER"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBSELF"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBSPC"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["BI"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["DRCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["REMARK"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["ENTDATE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBSUNAP"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["TUYAKNO"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["ORDERNO"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["MULTI"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["MULTIREMARK"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["DUR"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["RESV"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["SCODESAYU"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["SCODEREMARK"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBSEND"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["AUTO_SEND"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["RES"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBSPC_NO"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["WRTNO"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["TUYEOPOINT"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["TUYEOTIME"].ToString().Trim();
                            #endregion
                        }
                        else if (strGbJob == "IPD" || strGbJob == "ER")
                        {
                            TBL_NAME = clsCertWork.OCS_IORDER;
                            SABUN = dtCert.Rows[i]["DRCODE"].ToString().Trim().To<int>(0).ToString("00000");

                            #region // 병동,응급 EMRDATA 생성
                            strRowid = dtCert.Rows[i]["ROWID"].ToString().Trim();
                            strDrCode = dtCert.Rows[i]["DRCODE"].ToString().Trim();
                            nOrderNo = long.Parse(dtCert.Rows[i]["ORDERNO"].ToString().Trim());
                            strEMRData = dtCert.Rows[i]["ptno"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["bdate"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["DRCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["STAFFID"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["SLIPNO"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["ORDERCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["SUCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["BUN"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["Contents"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["BCONTENTS"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["REALQTY"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["QTY"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["REALNAL"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["NAL"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["DOSCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBINFO"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBSELF"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBSPC"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBNGT"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBER"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBPRN"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBDIV"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBBOTH"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBACT"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBSEND"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBPOSITION"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBSTATUS"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["NURSEID"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["ENTDATE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["WARDCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["ROOMCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["BI"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["ORDERNO"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["REMARK"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["ACTDATE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBGROUP"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBPORT"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["ORDERSITE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["MULTI"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["MULTIREMARK"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["POWDER"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["TUYEOPOINT"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["TUYEOTIME"].ToString().Trim();
                            #endregion
                        }
                        else if (strGbJob == "VERBAL")
                        {
                            TBL_NAME = clsCertWork.OCS_IORDER;
                            SABUN = dtCert.Rows[i]["DRCODE"].ToString().Trim().To<int>(0).ToString("00000");

                            #region // 구두처방 EMRDATA 생성
                            strRowid = dtCert.Rows[i]["ROWID"].ToString().Trim();
                            strDrCode = dtCert.Rows[i]["DRCODE"].ToString().Trim();
                            nOrderNo = long.Parse(dtCert.Rows[i]["ORDERNO"].ToString().Trim());
                            strEMRData = dtCert.Rows[i]["ptno"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["bdate"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["DRCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["STAFFID"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["SLIPNO"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["ORDERCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["SUCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["BUN"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["Contents"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["BCONTENTS"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["REALQTY"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["QTY"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["REALNAL"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["NAL"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["DOSCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBINFO"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBSELF"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBSPC"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBNGT"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBER"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBPRN"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBDIV"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBBOTH"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBACT"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBSEND"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBPOSITION"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBSTATUS"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["NURSEID"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["ENTDATE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["WARDCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["ROOMCODE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["BI"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["ORDERNO"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["REMARK"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["ACTDATE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBGROUP"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["GBPORT"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["ORDERSITE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["MULTI"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["MULTIREMARK"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["POWDER"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["DRORDERVIEW"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["VERBC"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["PRN_INS_GBN"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["PRN_INS_UNIT"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["PRN_INS_SDATE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["PRN_INS_EDATE"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["POWDER_SAYU"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["PRN_INS_MAX"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["ASA"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["TUYEOPOINT"].ToString().Trim() + "|";
                            strEMRData += dtCert.Rows[i]["TUYEOTIME"].ToString().Trim();
                            #endregion
                        }

                        string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
                        // 전자인증
                        if (clsCertWork.CertWorkBacth(pDbCon, SABUN, CERTDATE, TBL_NAME, strPtno, strEMRData, ref strHashdata, ref strSignData, ref strCertData, ref nCertno).Equals("OK"))
                        {
                            rtnVal = "OK";
                        }

                        if (rtnVal == "OK")
                        {
                            if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, TBL_NAME, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
                            {
                                rtnVal = "NO";
                            }
                        }
                    }
                }

                dtCert.Dispose();
                dtCert = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                //ComFunc.MsgBox("함수명 : " + "fn_Cert_Order" + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "fn_Cert_Order" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// 화상가산 대상여부
        /// </summary>
        /// <param name="strSuCode"></param>
        /// <returns></returns>
        public string fn_SugbAD_Check(string strSuCode)
        {
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT SUGBAD                          \r";
                SQL += "   FROM ADMIN.BAS_SUN             \r";
                SQL += "  WHERE SUNEXT = '" + strSuCode + "'    \r";
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "fn_SugbAD_Check" + ComNum.VBLF + SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (reader.HasRows)
                {
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Dispose();
                    reader = null;
                }

                ComFunc.MsgBox("함수명 : " + "fn_SugbAD_Check" + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "fn_SugbAD_Check" + ComNum.VBLF + ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public string fn_Pat_AddressRead(PsmhDb pDbCon, string strPtno)
        {
            DataTable dtAddress = null;
            DataTable dtAddress1 = null;
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";

            try
            {
                //마약은 처방소견반드시 등록
                SQL = "";
                SQL += " SELECT A.JUMIN1 || A.JUMIN2 JUMIN                                  \r";
                SQL += "      , A.ZIPCODE1 || A.ZIPCODE2  ZIPCODE , A.JUSO                  \r";
                SQL += "      , A.TEL, A.HPHONE, A.EMAIL, A.BIRTH, B.MAILJUSO, B.MAILDONG   \r";
                SQL += "   FROM ADMIN.BAS_PATIENT A                                   \r";
                SQL += "      , ADMIN.BAS_MAILNEW B                                   \r";
                SQL += "  WHERE A.PANO = '" + strPtno + "'                                  \r";
                SQL += "    AND A.ZIPCODE1 || A.ZIPCODE2 = B.MAILCODE                       \r";
                SqlErr = clsDB.GetDataTableEx(ref dtAddress, SQL, pDbCon);

                if (SqlErr != "")
                {
                    //ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "fn_Pat_AddressRead" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    rtnVal = "";
                    return rtnVal;
                }

                if (dtAddress.Rows.Count > 0)
                {
                    rtnVal = dtAddress.Rows[0]["MAILJUSO"].ToString().Trim() + " " + dtAddress.Rows[0]["JUSO"].ToString().Trim();
                }
                else
                {
                    SQL = "";
                    SQL += " SELECT A.JUMIN1 || A.JUMIN2 JUMIN                      \r";
                    SQL += "      , A.ZIPCODE1 || A.ZIPCODE2  ZIPCODE , A.JUSO      \r";
                    SQL += "      , A.TEL, A.HPHONE, A.EMAIL, A.BIRTH               \r";
                    SQL += "   FROM ADMIN.BAS_PATIENT       A                 \r";
                    SQL += "      , ADMIN.VIEW_PATIENT_JUSO B                 \r";
                    SQL += "  WHERE A.PANO = '" + strPtno + "'                      \r";
                    SQL += "    AND A.PANO = B.PANO                                 \r";
                    SqlErr = clsDB.GetDataTableEx(ref dtAddress1, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        //ComFunc.MsgBox("조회중 오류가 발생했습니다");
                        clsDB.SaveSqlErrLog("함수명 : " + "fn_Pat_AddressRead" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                        rtnVal = "";
                        return rtnVal;
                    }

                    if (dtAddress1.Rows.Count > 0)
                    {
                        rtnVal = dtAddress1.Rows[0]["JUSO"].ToString().Trim();
                    }
                    dtAddress1.Dispose();
                    dtAddress1 = null;
                }
                dtAddress.Dispose();
                dtAddress = null;

                return rtnVal;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox("함수명 : " + "fn_Pat_AddressRead" + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "fn_Pat_AddressRead" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        //2018.09.08
        public void fn_SS_Edit_ER_MRI(FarPoint.Win.Spread.FpSpread SpdNm, int nRow, int nOrderCodeCol, int nSuCodeCol, int nSelfCol, int nBunCol)
        {
            for (int i = 0; i < nRow; i++)
            {
                if (SpdNm.ActiveSheet.Cells[i, nBunCol].Text.Trim() == "73")
                {
                    SpdNm.ActiveSheet.Cells[i, nBunCol].Locked = false;
                }
            }
        }

        public string fn_GetSlipName(PsmhDb pDbCon, string strSlipNo)
        {
            OracleDataReader reader = null;
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";

            try
            {
                //마약은 처방소견반드시 등록
                SQL = "";
                SQL += " select ordername                       \r";
                SQL += "   from ADMIN.OCS_ORDERCODE        \r";
                SQL += "  where slipno = '" + strSlipNo + "'    \r";
                SQL += "    and seqno = 0                       \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                if (SqlErr != "")
                {
                    //ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "fn_GetSlipName" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    rtnVal = "";
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox("함수명 : " + "fn_GetSlipName" + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "fn_GetSlipName" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public string fn_GetOrderName(PsmhDb pDbCon, string strOrderCode)
        {
            OracleDataReader reader = null;
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";

            try
            {                
                SQL = "";
                SQL += " SELECT ORDERNAME                           \r";
                SQL += "   FROM ADMIN.OCS_ORDERCODE            \r";
                SQL += "  WHERE ORDERCODE = '" + strOrderCode + "'  \r";
                
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    //ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "fn_GetOrderName" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    rtnVal = "";
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox("함수명 : " + "fn_GetOrderName" + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "fn_GetOrderName" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public class cOCS_ORDER
        {
            public string Job = "";
            public string GbIO = "";
            public string PTNO = "";
            public string BDATE = "";
            public string DEPTCODE = "";
            public string SEQNO = "";
            public string ORDERCODE = "";
            public string SUCODE = "";
            public string BUN = "";
            public string SLIPNO = "";
            public string RealQTY = "";
            public int QTY = 0;
            public int NAL = 0;
            public int RealNal = 0;
            public string GBDIV = "";
            public string DOSCODE = "";
            public string GBBOTH = "";
            public string GBINFO = "";
            public string GBER = "";
            public string GBSELF = "";
            public string GBSPC = "";
            public string BI = "";
            public string DRCODE = "";
            public string StaffID = "";
            public string REMARK = "";
            public string ENTDATE = "";
            public string GBSUNAP = "";
            public string TUYAKNO = "";
            public long ORDERNO = 0;
            public string MULTI = "";
            public string MULTIREMARK = "";
            public string DUR = "";
            public string RESV = "";
            public string SCODESAYU = "";
            public string SCODEREMARK = "";
            public string GBSEND = "";
            public string WardCode = "";
            public string RoomCode = "";
            public string GbStatus = "";
            public string GbPRN = "";
            public string Sabun = "";

        }

        /// <summary>
        /// 알부민 검사 패스 등록번호
        /// </summary>
        /// <returns></returns>
        public static bool IS_ALBUMIN_PTNO(string PTNO)
        {
            MParameter parameter = new MParameter();
            bool rtnVal = false;

            try
            {
                parameter.AppendSql("SELECT '1'                                        ");
                parameter.AppendSql("  FROM DUAL                                       ");
                parameter.AppendSql(" WHERE EXISTS                                     ");
                parameter.AppendSql(" (                                                ");
                parameter.AppendSql("      SELECT 1                                    ");
                parameter.AppendSql("        FROM ADMIN.BAS_BCODE                ");
                parameter.AppendSql("       WHERE GUBUN = 'ME_ALBUMIN_PTNO'            ");
                parameter.AppendSql("         AND CODE  = :PTNO                        ");
                parameter.AppendSql(" )                                                ");

                parameter.Add("PTNO", PTNO);

                if (clsDB.ExecuteScalar<string>(parameter, clsDB.DbCon) != null)
                {
                    rtnVal = true;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, parameter.NativeQuerySql, clsDB.DbCon);
            }

            return rtnVal;
        }

        /// <summary>
        /// 내분비 내과 외래 처방에서 미량 알부민검사시
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="PTNO"></param>
        /// <returns></returns>
        public string CHECK_ME_ALBUMIN_EXAM(PsmhDb pDbCon, string PTNO, FarPoint.Win.Spread.FpSpread ArgSS)
        {
            string strRtn = "OK";
            if (IS_ALBUMIN_PTNO(PTNO))
                return strRtn;

            MParameter mParameter = new MParameter();

            for(int i = 0; i< ArgSS.ActiveSheet.RowCount; i++)
            {
                if (ArgSS.ActiveSheet.Cells[i, 0].Text.Equals("True"))
                    continue;

                if (ArgSS.ActiveSheet.Cells[i, (int)BaseOrderInfo.OpdOrderCol.SUCODE].Text.Trim().Equals("C2302"))
                {
                    #region 2년내 B0030R 처방 기록 없으면 NO!
                    mParameter = new MParameter();
                    mParameter.AppendSql("SELECT 1                                                                      ");
                    mParameter.AppendSql("  FROM DUAL                                                                   ");
                    mParameter.AppendSql(" WHERE EXISTS                                                                 ");
                    mParameter.AppendSql(" (                                                                            ");
                    mParameter.AppendSql("      SELECT ORDERNO                                                          ");
                    mParameter.AppendSql("        FROM ADMIN.OCS_OORDER                                            ");
                    mParameter.AppendSql("       WHERE PTNO  = :PTNO                                                    ");
                    mParameter.AppendSql("         AND BDATE >= TO_DATE(:BDATE, 'YYYY-MM-DD') - 730                     ");
                    mParameter.AppendSql("         AND BDATE <  TO_DATE(:BDATE, 'YYYY-MM-DD')                           ");
                    mParameter.AppendSql("         AND SUCODE = :SUCODE                                                 ");
                    mParameter.AppendSql("       GROUP BY ORDERNO                                                       ");
                    mParameter.AppendSql("      HAVING SUM(QTY * NAL) > 0                                               ");
                    mParameter.AppendSql(" )                                                                            ");

                    mParameter.Add("PTNO", PTNO, OracleDbType.Char);
                    mParameter.Add("BDATE", GstrBDate);
                    mParameter.Add("SUCODE", "B0030R", OracleDbType.Char);

                    if (clsDB.ExecuteScalar<int>(mParameter, pDbCon) == 0)
                    {
                        strRtn = "U.Pro 검사결과가 없어\r\nC2302(미량알부민(MICROALBUMIN)검사(정량))\r\n처방 불가합니다";
                        return strRtn;
                    }
                    else
                    {
                        #region 데이터 있음 가장 최근 검사 판독
                        mParameter = new MParameter();
                        mParameter.AppendSql(" SELECT    C.RESULTDATE                                                   ");
                        mParameter.AppendSql("  	,	 C.RESULT                                                       ");
                        mParameter.AppendSql("  	,	 A.ORDERNO                                                      ");
                        mParameter.AppendSql("  	,    TO_CHAR(A.BDATE, 'YYYY-MM-DD') BDATE                           ");
                        mParameter.AppendSql("   FROM ADMIN.EXAM_ORDER A                                           ");
                        mParameter.AppendSql("  INNER JOIN ADMIN.EXAM_SPECMST B                                    ");
                        mParameter.AppendSql("     ON A.SPECNO = B.SPECNO                                               ");
                        mParameter.AppendSql("  INNER JOIN ADMIN.EXAM_RESULTC C                                    ");
                        mParameter.AppendSql("     ON B.SPECNO = C.SPECNO                                               ");
                        mParameter.AppendSql("    AND C.MASTERCODE = 'UR02'                                             ");
                        mParameter.AppendSql("    AND C.SUBCODE = 'UR02I'                                               ");
                        mParameter.AppendSql("  WHERE A.ORDERNO IN                                                      ");
                        mParameter.AppendSql("  	 (                                                                  ");
                        mParameter.AppendSql("          SELECT ORDERNO                                                  ");
                        mParameter.AppendSql("	          FROM ADMIN.OCS_OORDER                                    ");
                        mParameter.AppendSql("	         WHERE PTNO  = :PTNO                                            ");
                        mParameter.AppendSql("	           AND BDATE >= TO_DATE(:BDATE, 'YYYY-MM-DD') - 730             ");
                        mParameter.AppendSql("	           AND BDATE <  TO_DATE(:BDATE, 'YYYY-MM-DD')                   ");
                        mParameter.AppendSql("	           AND SUCODE = :SUCODE                                         ");
                        mParameter.AppendSql("	         GROUP BY ORDERNO                                               ");
                        mParameter.AppendSql("	        HAVING SUM(QTY * NAL) > 0                                       ");
                        mParameter.AppendSql("  	 )                                                                  ");
                        mParameter.AppendSql(" ORDER BY C.RESULTDATE DESC                                               ");

                        mParameter.Add("PTNO", PTNO, OracleDbType.Char);
                        mParameter.Add("BDATE", GstrBDate);
                        mParameter.Add("SUCODE", "B0030R", OracleDbType.Char);

                        List<Dictionary<string, object>> dt = clsDB.ExecuteReader(mParameter, pDbCon);
                        if (dt.Count > 0)
                        {
                            if (dt[0]["RESULT"].To<string>("").IndexOf("+") == -1)
                            {
                                strRtn = "OK";
                                return strRtn;
                            }
                            else 
                            {
                                strRtn = string.Format("<{0}   U.Pro {1} 로\r\nC2302(미량알부민(MICROALBUMIN)검사(정량))\r\n처방 불가합니다>", dt[0]["BDATE"], dt[0]["RESULT"]);
                                return strRtn;
                            }
                        }
                        #endregion
                    }
                    #endregion


                }

            }

            return strRtn;
        }


        /// <summary>
        /// 신경인지기능검사 처방갯수 조회
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgOrderCode"></param>
        /// <param name="argPTNO"></param>
        /// <param name="ArgSS"></param>
        /// <param name="argROW"></param>
        /// <param name="ArgChk"></param>
        /// <returns></returns>
        public string CHECK_COUNT_NEUROCOGNITIVE_FUNCTION_TEST(PsmhDb pDbCon, string strIO, FarPoint.Win.Spread.FpSpread ArgSS)
        {
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int strChk1 = 0;
            int strChk2 = 0;
            string strRtn = "";
            string strMsg = "";

            int cSucode = 0;
            int cSlipNo = 0;

            strRtn = "OK";

            if (strIO == "OPD")
            {
                cSucode = 12;
                cSlipNo = 14;
            }
            else
            {
                cSucode = 14;
                cSlipNo = 16;
            }

            for (int i = 0; i < ArgSS.ActiveSheet.NonEmptyRowCount; i++)
            {
                // DC처방이 아니고 SLIPNO = NP
                if (ArgSS.ActiveSheet.Cells[i, 0].Text != "True" && ArgSS.ActiveSheet.Cells[i, cSlipNo].Text.Trim() == "NP")
                {

                    #region // 사용안함
                    // SUCODE
                    //switch (ArgSS.ActiveSheet.Cells[i, cSucode].Text.Trim())
                    //{

                    //    case "A001":    //유형 I - 무시증후군검사(Neglect Syndrome Test)
                    //    case "A003":    //유형 I - 숫자-기호바꾸기 검사(Digit Symbol Test)
                    //    case "A013":    //유형 I - 범주(또는 의미) 유창성 검사
                    //    case "A014":    //유형 I - 글자(또는 음소) 유창성 검사
                    //    case "A023":    //유형 I - 의미모양 색깔 속성/이름대기 검사
                    //    case "A004":    //유형 I -  좌 우 구분검사(Right-Left Orientation)
                    //    case "A005":    //유형 I - 손가락 이름 대기검사 (Finger Naming)
                    //    case "A009":    //유형 I - 보속성검사 (Perseverance Test)
                    //    case "A010":    //유형 I - 운동 지속불능증(Motor Impersistence)
                    //    case "A011":    //유형 I - 주먹 손날 손바닥 검사 (Fist -Edge-Palm)
                    //    case "A012":    //유형 I - 양손 교차 운동검사
                    //        strChk1++;
                    //        break;
                    //    case "B007A":   //유형 II - 연속수행력검사 [시각]
                    //    case "B008A":   //유형 II - 연속수행력검사 [청각]
                    //    case "B013A":   //유형 II -CNT 숫자따라 말하기 검사
                    //    case "B016A":   //유형 II -CNT  시각단기 기억검사
                    //    case "B037":    //유형 II - 선로잇기 검사 (Trail  Making Test)
                    //    case "B038":    //유형 II -CNT  단어색채 검사(Word Color Test)
                    //        strChk2++;
                    //        break;
                    //}
                    #endregion

                    // 유형 I 체크
                    SQL = "";
                    SQL += "SELECT SUCODE FROM ADMIN.OCS_ORDERCODE                               ";
                    SQL += " WHERE SLIPNO = 'NP'                                                      ";
                    SQL += "   AND ORDERCODE LIKE 'A%'                                                ";
                    SQL += "   AND ORDERNAME LIKE '유형 I%'                                           ";
                    SQL += "   AND SUCODE = '" + ArgSS.ActiveSheet.Cells[i, cSucode].Text.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 오류가 발생했습니다");
                        return "NO";
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        strChk1++;
                    }

                    dt1.Dispose();
                    dt1 = null;



                    // 유형 II 체크
                    SQL = "";
                    SQL += "SELECT SUCODE FROM ADMIN.OCS_ORDERCODE                               ";
                    SQL += " WHERE SLIPNO = 'NP'                                                      ";
                    SQL += "   AND ORDERCODE LIKE 'B%'                                                ";
                    SQL += "   AND ORDERNAME LIKE '유형 II%'                                          ";
                    SQL += "   AND SUCODE = '" + ArgSS.ActiveSheet.Cells[i, cSucode].Text.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 오류가 발생했습니다");
                        return "NO";
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        strChk2++;
                    }

                    dt2.Dispose();
                    dt2 = null;

                }
            }

            if (strChk1 > 0 && strChk1 < 3)
            {
                strMsg = "신경인지기능검사(Neurocognitive Function Test) 유형-1 처방은" + "\r\n\r\n";
                strMsg += "최소 3개이상부터 처방이 가능합니다." + "\r\n\r\n";

                ComFunc.MsgBox(strMsg, "확인");
                strRtn = "NO";
            }

            return strRtn;
        }



        /// <summary>
        /// 신경인지기능검사 처방
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSS"></param>
        /// <returns></returns>
        public string CHECK_ORDER_NEUROCOGNITIVE_FUNCTION_TEST(PsmhDb pDbCon, string strIO, FarPoint.Win.Spread.FpSpread ArgSS)
        {
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int strChk1 = 0;
            int strChk2 = 0;
            string strRtn = "";
            string strMsg = "";
            string strCode = "";

            int cSucode = 0;
            int cSlipNo = 0;

            strRtn = "OK";

            if (strIO == "OPD")
            {
                cSucode = 12;
                cSlipNo = 14;
            }
            else
            {
                cSucode = 14;
                cSlipNo = 16;
            }


            for (int i = 0; i < ArgSS.ActiveSheet.NonEmptyRowCount; i++)
            {
                // DC처방이 아니고 SLIPNO = NP
                if (ArgSS.ActiveSheet.Cells[i, 0].Text != "True" && ArgSS.ActiveSheet.Cells[i, cSlipNo].Text.Trim() == "NP")
                {

                    #region // 사용안함
                    // SUCODE
                    //switch (ArgSS.ActiveSheet.Cells[i, cSucode].Text.Trim())
                    //{

                    //    case "A001":    //유형 I - 무시증후군검사(Neglect Syndrome Test)
                    //    case "A003":    //유형 I - 숫자-기호바꾸기 검사(Digit Symbol Test)
                    //    case "A013":    //유형 I - 범주(또는 의미) 유창성 검사
                    //    case "A014":    //유형 I - 글자(또는 음소) 유창성 검사
                    //    case "A023":    //유형 I - 의미모양 색깔 속성/이름대기 검사
                    //    case "A004":    //유형 I -  좌 우 구분검사(Right-Left Orientation)
                    //    case "A005":    //유형 I - 손가락 이름 대기검사 (Finger Naming)
                    //    case "A009":    //유형 I - 보속성검사 (Perseverance Test)
                    //    case "A010":    //유형 I - 운동 지속불능증(Motor Impersistence)
                    //    case "A011":    //유형 I - 주먹 손날 손바닥 검사 (Fist -Edge-Palm)
                    //    case "A012":    //유형 I - 양손 교차 운동검사
                    //        strChk1++;
                    //        break;
                    //    case "B007A":   //유형 II - 연속수행력검사 [시각]
                    //    case "B008A":   //유형 II - 연속수행력검사 [청각]
                    //    case "B013A":   //유형 II -CNT 숫자따라 말하기 검사
                    //    case "B016A":   //유형 II -CNT  시각단기 기억검사
                    //    case "B037":    //유형 II - 선로잇기 검사 (Trail  Making Test)
                    //    case "B038":    //유형 II -CNT  단어색채 검사(Word Color Test)
                    //        strChk2++;
                    //        break;
                    //}
                    #endregion

                    // 유형 I 체크
                    SQL = "";
                    SQL += "SELECT SUCODE FROM ADMIN.OCS_ORDERCODE                               ";
                    SQL += " WHERE SLIPNO = 'NP'                                                      ";
                    SQL += "   AND ORDERCODE LIKE 'A%'                                                ";
                    SQL += "   AND ORDERNAME LIKE '유형 I%'                                           ";
                    SQL += "   AND SUCODE = '" + ArgSS.ActiveSheet.Cells[i, cSucode].Text.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 오류가 발생했습니다");
                        return "NO";
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        strChk1++;
                    }

                    dt1.Dispose();
                    dt1 = null;



                    // 유형 II 체크
                    SQL = "";
                    SQL += "SELECT SUCODE FROM ADMIN.OCS_ORDERCODE                               ";
                    SQL += " WHERE SLIPNO = 'NP'                                                      ";
                    SQL += "   AND ORDERCODE LIKE 'B%'                                                ";
                    SQL += "   AND ORDERNAME LIKE '유형 II%'                                          ";
                    SQL += "   AND SUCODE = '" + ArgSS.ActiveSheet.Cells[i, cSucode].Text.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 오류가 발생했습니다");
                        return "NO";
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        strChk2++;
                    }

                    dt2.Dispose();
                    dt2 = null;

                }
            }

            // 유형-1 처방발생
            if (strChk1 >= 3)
            {
                if (strChk1 >= 3 && strChk1 <= 5)
                {
                    strCode = "FB011";
                }
                else if (strChk1 >= 6 && strChk1 <= 8)
                {
                    strCode = "FB012";
                }
                else if (strChk1 >= 9)
                {
                    strCode = "FB013";
                }

                // 기발생 수가코드 DC처리
                SqlErr = delete_OCS_ORDER(pDbCon, ref intRowAffected, strIO, "'FB011', 'FB012', 'FB013'", strCode);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return "NO";
                }

                cOCS_ORDER argCls = move_OCS_ORDER(pDbCon, GstrGbJob == "OPD" ? "O" : "I", clsOrdFunction.Pat.PtNo, clsOrdFunction.GstrBDate, clsOrdFunction.Pat.DeptCode, clsOrdFunction.Pat.DrCode, clsOrdFunction.Pat.Bi, strCode, clsOrdFunction.Pat.WardCode, clsOrdFunction.Pat.RoomCode);

                // 유형-1 수가코드 발생
                SqlErr = ins_OCS_ORDER(pDbCon, argCls, ref intRowAffected);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return "NO";
                }
            }


            // 유형-2 처방발생
            if (strChk2 >= 1)
            {
                if (strChk2 >= 1 && strChk2 <= 3)
                {
                    strCode = "FB021";
                }
                else if (strChk2 >= 4)
                {
                    strCode = "FB022";
                }

                // 기발생 수가코드 DC처리
                SqlErr = delete_OCS_ORDER(pDbCon, ref intRowAffected, strIO, "'FB021', 'FB022'", strCode);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return "NO";
                }

                cOCS_ORDER argCls = move_OCS_ORDER(pDbCon, GstrGbJob == "OPD" ? "O" : "I", clsOrdFunction.Pat.PtNo, clsOrdFunction.GstrBDate, clsOrdFunction.Pat.DeptCode, clsOrdFunction.Pat.DrCode, clsOrdFunction.Pat.Bi, strCode, clsOrdFunction.Pat.WardCode, clsOrdFunction.Pat.RoomCode);

                // 유형-2 수가코드 발생
                SqlErr = ins_OCS_ORDER(pDbCon, argCls, ref intRowAffected);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return "NO";
                }
            }

            return "OK";
        }

        public cOCS_ORDER move_OCS_ORDER(PsmhDb pDbCon, string argIO, string argPTNO, string argBDATE, string argDEPTCODE, string argDRCODE, string argBI, string argORDERCODE, string argWARDCODE, string argROOMCODE)
        {
            cOCS_ORDER argCls = new cOCS_ORDER();
            argCls.GbIO = argIO;
            argCls.PTNO = argPTNO;
            argCls.BDATE = argBDATE;
            argCls.DEPTCODE = argDEPTCODE;
            argCls.DRCODE = argDRCODE;
            argCls.SEQNO = "99";
            
            argCls.ORDERCODE = argORDERCODE;
            argCls.SUCODE = argORDERCODE;
            
            argCls.BUN = "27";
            argCls.SLIPNO = "NP";
            argCls.RealQTY = "1";
            argCls.QTY = 1;
            argCls.RealNal = 1;
            argCls.NAL = 1;
            argCls.GBDIV = "1";
            argCls.DOSCODE = "";
            argCls.GBBOTH = "0";
            argCls.GBINFO = "";
            argCls.GBER = "";
            argCls.GBSELF = "0";
            argCls.GBSPC = "";
            argCls.BI = argBI;
            argCls.REMARK = "";
            argCls.GBSUNAP = "0";
            argCls.TUYAKNO = "0";
            argCls.ORDERNO = Convert.ToInt32(ComQuery.GetSequencesNo(pDbCon, "KOSMOS_OCS", "SEQ_ORDERNO")); //오더넘버생성
            argCls.MULTI = "";
            argCls.MULTIREMARK = "";
            argCls.DUR = "";
            argCls.RESV = "";
            argCls.SCODESAYU = "";
            argCls.SCODEREMARK = "";
            argCls.GBSEND = "";
            if (argCls.GbIO == "I")
            {
                argCls.GBSEND = "*";
            }

            argCls.Sabun = clsType.User.Sabun;
            argCls.StaffID = clsType.User.Sabun;

            argCls.WardCode = argWARDCODE;
            argCls.RoomCode = argROOMCODE;
            argCls.GbStatus = " ";
            argCls.GbPRN = " ";

            return argCls;
        }

        public string ins_OCS_ORDER(PsmhDb pDbCon, cOCS_ORDER argCls, ref int intRowAffected)
        {
            DataTable dt = null;
            string SqlErr = string.Empty;
            string SQL = "";

            if (argCls.GbIO == "O")
            {
                SQL = "";
                SQL += " SELECT SUCODE, ORDERNO, GBSUNAP FROM ADMIN.OCS_OORDER         \r\n";
                SQL += "  WHERE PTNO     = '" + argCls.PTNO + "'                            \r\n";
                SQL += "    AND BDATE    = TO_DATE('" + argCls.BDATE + "','YYYY-MM-DD')     \r\n";
                SQL += "    AND DEPTCODE = '" + argCls.DEPTCODE + "'                        \r\n";
                SQL += "    AND SUCODE IN ('" + argCls.SUCODE + "')                         \r\n";
                SQL += "    AND GBSUNAP IN ('0', '1')                                       \r\n";
                SQL += "    AND NAL > 0                                                     \r\n";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    return "NO";
                }

                if (dt.Rows.Count == 0)
                {
                    #region // INSERT QUERY
                    SQL = "";
                    if (argCls.GbIO == "O")
                    {
                        SQL = " INSERT INTO " + ComNum.DB_MED + "OCS_OORDER                                 \r\n";
                    }
                    else if (argCls.GbIO == "I" || argCls.GbIO == "E")
                    {
                        SQL = " INSERT INTO " + ComNum.DB_MED + "OCS_IORDER                                 \r\n";
                    }
                    SQL += "   (PTNO, BDATE, DEPTCODE, SEQNO, ORDERCODE, SUCODE, BUN                        \r\n";
                    SQL += "    ,SLIPNO, REALQTY, QTY, NAL, GBDIV,DOSCODE, GBBOTH, GBINFO                   \r\n";
                    SQL += "    ,GBER, GBSELF, GBSPC, BI, DRCODE, REMARK, ENTDATE                           \r\n";
                    SQL += "    ,ORDERNO, MULTI, MULTIREMARK, DUR                                           \r\n";

                    if (argCls.GbIO == "O")
                    {
                        SQL += "    ,GBSUNAP,TUYAKNO,SCODESAYU,SCODEREMARK,RESV,Sabun                       \r\n";
                    }
                    else if (argCls.GbIO == "I" || argCls.GbIO == "E")
                    {
                        if (argCls.GbIO == "E")
                        {
                            SQL += "    ,staffid,WardCode,RoomCode,GBSTATUS,GbPRN, GBIOE, ORDERSITE, GBACT                                \r\n";
                        }
                        else
                        {
                            SQL += "    ,staffid,WardCode,RoomCode,GBSTATUS,GbPRN, IP                                \r\n";
                        }
                    }

                    SQL += "    , GBSEND  )    VALUES                                                       \r\n";
                    SQL += "   (                                                                            \r\n";
                    SQL += "     '" + argCls.PTNO + "'                                                      \r\n";
                    SQL += "     ,TO_DATE('" + argCls.BDATE + "','YYYY-MM-DD')                              \r\n";
                    SQL += "     ,'" + argCls.DEPTCODE + "'                                                 \r\n";
                    SQL += "     ,'" + argCls.SEQNO + "'                                                    \r\n";
                    SQL += "     ,'" + argCls.ORDERCODE + "'                                                \r\n";
                    SQL += "     ,'" + argCls.SUCODE + "'                                                   \r\n";
                    SQL += "     ,'" + argCls.BUN + "'                                                      \r\n";
                    SQL += "     ,'" + argCls.SLIPNO + "'                                                   \r\n";
                    SQL += "     ,'" + argCls.RealQTY + "'                                                  \r\n";
                    SQL += "     ,'" + argCls.QTY + "'                                                      \r\n";
                    SQL += "     ,'" + argCls.NAL + "'                                                      \r\n";
                    SQL += "     ,'" + argCls.GBDIV + "'                                                    \r\n";
                    SQL += "     ,'" + argCls.DOSCODE + "'                                                  \r\n";
                    SQL += "     ,'" + argCls.GBBOTH + "'                                                   \r\n";
                    SQL += "     ,'" + argCls.GBINFO + "'                                                   \r\n";
                    SQL += "     ,'" + argCls.GBER + "'                                                     \r\n";
                    SQL += "     ,'" + argCls.GBSELF + "'                                                   \r\n";
                    SQL += "     ,'" + argCls.GBSPC + "'                                                    \r\n";
                    SQL += "     ,'" + argCls.BI + "'                                                       \r\n";
                    if (argCls.GbIO == "O")
                    {
                        SQL += "     ,'" + argCls.DRCODE + "'                                                   \r\n";
                    }
                    else if (argCls.GbIO == "I" || argCls.GbIO == "E")
                    {
                        SQL += "     ,'" + argCls.StaffID + "'                                                   \r\n";
                    }
                    if (argCls.GbIO == "E")
                    {
                        SQL += "     ,''                                                   \r\n";
                    }
                    else
                    {
                        SQL += "     ,'" + argCls.REMARK + "'                                                   \r\n";
                    }

                    SQL += "     ,SYSDATE                                                                   \r\n";
                    SQL += "     ," + argCls.ORDERNO + "                                                    \r\n";
                    SQL += "     ,'" + argCls.MULTI + "'                                                    \r\n";
                    SQL += "     ,'" + argCls.MULTIREMARK + "'                                              \r\n";
                    SQL += "     ,'" + argCls.DUR + "'                                                      \r\n";

                    if (argCls.GbIO == "O")
                    {
                        SQL += "     ,'" + argCls.GBSUNAP + "'                                              \r\n";
                        SQL += "     ,'" + argCls.TUYAKNO + "'                                              \r\n";
                        SQL += "     ,'" + argCls.SCODESAYU + "'                                            \r\n";
                        SQL += "     ,'" + argCls.SCODEREMARK + "'                                          \r\n";
                        SQL += "     ,'" + argCls.RESV + "'                                                 \r\n";
                        SQL += "     ,'" + argCls.StaffID + "'                                              \r\n";
                        SQL += "     ,'" + argCls.GBSEND + "'                                                   \r\n";
                    }
                    else if (argCls.GbIO == "I" || argCls.GbIO == "E")
                    {
                        if (argCls.GbIO == "E")
                        {
                            SQL += "     ,'" + argCls.DRCODE + "'                                               \r\n";
                            SQL += "     ,'" + argCls.WardCode + "'                                             \r\n";
                            SQL += "     ,'" + argCls.RoomCode + "'                                             \r\n";
                            SQL += "     ,'" + argCls.GbStatus + "'                                             \r\n";
                            SQL += "     ,'" + argCls.GbPRN + "'                                                \r\n";
                            SQL += "     ,'E'                                                \r\n";
                            SQL += "     ,'TEL'                                                \r\n";
                            SQL += "     ,'*'                                                \r\n";
                            SQL += "     ,'*'                                                \r\n";
                        }
                        else
                        {
                            SQL += "     ,'" + argCls.DRCODE + "'                                               \r\n";
                            SQL += "     ,'" + argCls.WardCode + "'                                             \r\n";
                            SQL += "     ,'" + argCls.RoomCode + "'                                             \r\n";
                            SQL += "     ,'" + argCls.GbStatus + "'                                             \r\n";
                            SQL += "     ,'" + argCls.GbPRN + "'                                                \r\n";
                            SQL += "     ,'" + clsPublic.GstrIP + "'                                                \r\n";
                            SQL += "     ,'" + argCls.GBSEND + "'                                                   \r\n";
                        }
                    }
                    SQL += "   )                                                                            \r\n";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    #endregion
                }

                dt.Dispose();
                dt = null;
            }
            else
            {
                SQL = "";
                SQL += "SELECT SUCODE, ORDERNO, GBSTATUS FROM ADMIN.OCS_IORDER         \r\n";
                SQL += " WHERE PTNO     = '" + argCls.PTNO + "'                             \r\n";
                SQL += "   AND BDATE    = TO_DATE('" + argCls.BDATE + "','YYYY-MM-DD')      \r\n";
                SQL += "   AND DEPTCODE = '" + argCls.DEPTCODE + "'                         \r\n";
                SQL += "   AND SUCODE IN ('" + argCls.SUCODE + "')                          \r\n";                
                SQL += "   AND GBSTATUS NOT IN('D', 'D-')                                   \r\n";
                SQL += "   AND (NurseID   IS NULL OR NurseId = ' ')                         \r\n";
                SQL += "   AND NAL > 0                                                      \r\n";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    return "NO";
                }

                if (dt.Rows.Count == 0)
                {
                    #region // INSERT QUERY
                    SQL = "";
                    if (argCls.GbIO == "O")
                    {
                        SQL = " INSERT INTO " + ComNum.DB_MED + "OCS_OORDER                                 \r\n";
                    }
                    else if (argCls.GbIO == "I" || argCls.GbIO == "E")
                    {
                        SQL = " INSERT INTO " + ComNum.DB_MED + "OCS_IORDER                                 \r\n";
                    }
                    SQL += "   (PTNO, BDATE, DEPTCODE, SEQNO, ORDERCODE, SUCODE, BUN                        \r\n";
                    SQL += "    ,SLIPNO, REALQTY, QTY, NAL, GBDIV,DOSCODE, GBBOTH, GBINFO                   \r\n";
                    SQL += "    ,GBER, GBSELF, GBSPC, BI, DRCODE, REMARK, ENTDATE                           \r\n";
                    SQL += "    ,ORDERNO, MULTI, MULTIREMARK, DUR                                           \r\n";

                    if (argCls.GbIO == "O")
                    {
                        SQL += "    ,GBSUNAP,TUYAKNO,SCODESAYU,SCODEREMARK,RESV,Sabun                       \r\n";
                    }
                    else if (argCls.GbIO == "I" || argCls.GbIO == "E")
                    {
                        if (argCls.GbIO == "E")
                        {
                            SQL += "    ,staffid,WardCode,RoomCode,GBSTATUS,GbPRN, GBIOE, ORDERSITE, GBACT                                \r\n";
                        }
                        else
                        {
                            SQL += "    ,staffid,WardCode,RoomCode,GBSTATUS,GbPRN, IP                                \r\n";
                        }
                    }

                    SQL += "    , GBSEND  )    VALUES                                                       \r\n";
                    SQL += "   (                                                                            \r\n";
                    SQL += "     '" + argCls.PTNO + "'                                                      \r\n";
                    SQL += "     ,TO_DATE('" + argCls.BDATE + "','YYYY-MM-DD')                              \r\n";
                    SQL += "     ,'" + argCls.DEPTCODE + "'                                                 \r\n";
                    SQL += "     ,'" + argCls.SEQNO + "'                                                    \r\n";
                    SQL += "     ,'" + argCls.ORDERCODE + "'                                                \r\n";
                    SQL += "     ,'" + argCls.SUCODE + "'                                                   \r\n";
                    SQL += "     ,'" + argCls.BUN + "'                                                      \r\n";
                    SQL += "     ,'" + argCls.SLIPNO + "'                                                   \r\n";
                    SQL += "     ,'" + argCls.RealQTY + "'                                                  \r\n";
                    SQL += "     ,'" + argCls.QTY + "'                                                      \r\n";
                    SQL += "     ,'" + argCls.NAL + "'                                                      \r\n";
                    SQL += "     ,'" + argCls.GBDIV + "'                                                    \r\n";
                    SQL += "     ,'" + argCls.DOSCODE + "'                                                  \r\n";
                    SQL += "     ,'" + argCls.GBBOTH + "'                                                   \r\n";
                    SQL += "     ,'" + argCls.GBINFO + "'                                                   \r\n";
                    SQL += "     ,'" + argCls.GBER + "'                                                     \r\n";
                    SQL += "     ,'" + argCls.GBSELF + "'                                                   \r\n";
                    SQL += "     ,'" + argCls.GBSPC + "'                                                    \r\n";
                    SQL += "     ,'" + argCls.BI + "'                                                       \r\n";
                    if (argCls.GbIO == "O")
                    {
                        SQL += "     ,'" + argCls.DRCODE + "'                                                   \r\n";
                    }
                    else if (argCls.GbIO == "I" || argCls.GbIO == "E")
                    {
                        SQL += "     ,'" + argCls.StaffID + "'                                                   \r\n";
                    }
                    if (argCls.GbIO == "E")
                    {
                        SQL += "     ,''                                                   \r\n";
                    }
                    else
                    {
                        SQL += "     ,'" + argCls.REMARK + "'                                                   \r\n";
                    }

                    SQL += "     ,SYSDATE                                                                   \r\n";
                    SQL += "     ," + argCls.ORDERNO + "                                                    \r\n";
                    SQL += "     ,'" + argCls.MULTI + "'                                                    \r\n";
                    SQL += "     ,'" + argCls.MULTIREMARK + "'                                              \r\n";
                    SQL += "     ,'" + argCls.DUR + "'                                                      \r\n";

                    if (argCls.GbIO == "O")
                    {
                        SQL += "     ,'" + argCls.GBSUNAP + "'                                              \r\n";
                        SQL += "     ,'" + argCls.TUYAKNO + "'                                              \r\n";
                        SQL += "     ,'" + argCls.SCODESAYU + "'                                            \r\n";
                        SQL += "     ,'" + argCls.SCODEREMARK + "'                                          \r\n";
                        SQL += "     ,'" + argCls.RESV + "'                                                 \r\n";
                        SQL += "     ,'" + argCls.StaffID + "'                                              \r\n";
                        SQL += "     ,'" + argCls.GBSEND + "'                                                   \r\n";
                    }
                    else if (argCls.GbIO == "I" || argCls.GbIO == "E")
                    {
                        if (argCls.GbIO == "E")
                        {
                            SQL += "     ,'" + argCls.DRCODE + "'                                               \r\n";
                            SQL += "     ,'" + argCls.WardCode + "'                                             \r\n";
                            SQL += "     ,'" + argCls.RoomCode + "'                                             \r\n";
                            SQL += "     ,'" + argCls.GbStatus + "'                                             \r\n";
                            SQL += "     ,'" + argCls.GbPRN + "'                                                \r\n";
                            SQL += "     ,'E'                                                \r\n";
                            SQL += "     ,'TEL'                                                \r\n";
                            SQL += "     ,'*'                                                \r\n";
                            SQL += "     ,'*'                                                \r\n";
                        }
                        else
                        {
                            SQL += "     ,'" + argCls.DRCODE + "'                                               \r\n";
                            SQL += "     ,'" + argCls.WardCode + "'                                             \r\n";
                            SQL += "     ,'" + argCls.RoomCode + "'                                             \r\n";
                            SQL += "     ,'" + argCls.GbStatus + "'                                             \r\n";
                            SQL += "     ,'" + argCls.GbPRN + "'                                                \r\n";
                            SQL += "     ,'" + clsPublic.GstrIP + "'                                                \r\n";
                            SQL += "     ,'" + argCls.GBSEND + "'                                                   \r\n";
                        }
                    }
                    SQL += "   )                                                                            \r\n";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    #endregion
                }

                dt.Dispose();
                dt = null;
            }

            return SqlErr;
        }

        public string delete_OCS_ORDER(PsmhDb pDbCon, ref int intRowAffected, string strIO, string strCode1, string strCode2)
        {
            DataTable dt = null;
            string SqlErr = string.Empty;
            string SQL = "";
            string rtnVal = "";

            if (strIO == "OPD")
            {
                SQL = "";
                SQL += " SELECT SUCODE, ORDERNO, GBSUNAP FROM ADMIN.OCS_OORDER                   \r\n";
                SQL += "  WHERE PTNO     = '" + clsOrdFunction.Pat.PtNo + "'                          \r\n";
                SQL += "    AND BDATE    = TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')   \r\n";
                SQL += "    AND DEPTCODE = '" + clsOrdFunction.Pat.DeptCode.Trim() + "'               \r\n";
                SQL += "    AND SUCODE IN (" + strCode1 + ")                                          \r\n";
                SQL += "    AND SUCODE NOT IN ('" + strCode2 + "')                                    \r\n";
                SQL += "    AND GBSUNAP IN ('0', '1')                                                 \r\n";
                SQL += "    AND NAL > 0                                                               \r\n";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    rtnVal = "NO";
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["GBSUNAP"].ToString().Trim() == "0")
                        {
                            #region // 수납전 처방
                            SQL = "";
                            SQL += " INSERT INTO ADMIN.OCS_OORDER_DEL ( PTNO,BDATE,DEPTCODE,SEQNO,ORDERCODE,SUCODE,BUN,SLIPNO,REALQTY,QTY,NAL      \r";
                            SQL += "      , GBDIV,DOSCODE,GBBOTH,GBINFO,GBER,GBSELF,GBSPC,BI,DRCODE,REMARK,ENTDATE,GBSUNAP                              \r";
                            SQL += "      , TUYAKNO,ORDERNO,MULTI,MULTIREMARK,DUR,RESV,SCODESAYU,SCODEREMARK,GBSEND,AUTO_SEND                           \r";
                            SQL += "      , res,GBSPC_NO,WRTNO,CERTNO,GBAUTOSEND,OCSDRUG,RESAMT,GBCOPY,Sabun,GBAUTOSEND2,DELTIME                        \r";
                            SQL += "      , GbTax,IP,ASA, PCHASU, SUBUL_WARD, POWDER, SEDATION)                                                         \r";
                            SQL += " SELECT PTNO,BDATE,DEPTCODE,SEQNO,ORDERCODE,SUCODE,BUN,SLIPNO,REALQTY,QTY,NAL                                       \r";
                            SQL += "      , GBDIV,DOSCODE,GBBOTH,GBINFO,GBER,GBSELF,GBSPC,BI,DRCODE,REMARK,ENTDATE,GBSUNAP                              \r";
                            SQL += "      , TUYAKNO,ORDERNO,MULTI,MULTIREMARK,DUR,RESV,SCODESAYU,SCODEREMARK,GBSEND,AUTO_SEND                           \r";
                            SQL += "      , res,GBSPC_NO,WRTNO,CERTNO,GBAUTOSEND,OCSDRUG,RESAMT,GBCOPY,Sabun,GBAUTOSEND2,SYSDATE                        \r";
                            SQL += "      , GbTax,IP,ASA, PCHASU, SUBUL_WARD, POWDER, SEDATION                                                          \r";
                            SQL += "   FROM ADMIN.OCS_OORDER                                                                                       \r";
                            SQL += "  WHERE Ptno     = '" + clsOrdFunction.Pat.PtNo + "'                                                                \r";
                            SQL += "    AND BDate    = TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')                                         \r";
                            SQL += "    AND DeptCode = '" + clsOrdFunction.Pat.DeptCode.Trim() + "'                                                     \r";
                            SQL += "    AND GbSunap  = '0'                                                                                              \r";
                            SQL += "    AND Orderno  = '" + dt.Rows[i]["ORDERNO"] + "'                                                                   \r";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr + " 기존자료백업시 오류가 발생되었습니다");
                                rtnVal = "NO";
                                return rtnVal;
                            }

                            //수납전 외래처방 삭제
                            SQL = "";
                            SQL += " DELETE ADMIN.OCS_OORDER                                                                                       \r";
                            SQL += "  WHERE Ptno     = '" + clsOrdFunction.Pat.PtNo + "'                                                                \r";
                            SQL += "    AND BDate    = TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')                                         \r";
                            SQL += "    AND DeptCode = '" + clsOrdFunction.Pat.DeptCode.Trim() + "'                                                     \r";
                            SQL += "    AND GbSunap  = '0'                                                                                              \r";
                            SQL += "    AND Orderno  = '" + dt.Rows[i]["ORDERNO"] + "'                                                                  \r";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr + " 기존자료(수납전 외래처방)를 삭제하는데 오류가 발생되었습니다");
                                rtnVal = "NO";
                                return rtnVal;
                            }
                            #endregion
                        }
                        else if (dt.Rows[i]["GBSUNAP"].ToString().Trim() == "1")
                        {
                            #region // 수납후 처방
                            SQL = "";
                            SQL += " UPDATE ADMIN.OCS_OORDER SET                                                                                   \r";
                            SQL += "        GbSuNap = '2'                                                                                               \r";
                            SQL += "  WHERE Ptno     = '" + clsOrdFunction.Pat.PtNo + "'                                                                \r";
                            SQL += "    AND BDate    = TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')                                         \r";
                            SQL += "    AND DeptCode = '" + clsOrdFunction.Pat.DeptCode.Trim() + "'                                                     \r";
                            SQL += "    AND GbSunap  = '1'                                                                                              \r";
                            SQL += "    AND Orderno  = '" + dt.Rows[i]["ORDERNO"] + "'                                                                  \r";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr + " 기존자료(수납전 외래처방)를 삭제하는데 오류가 발생되었습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                        
                                rtnVal = "NO";
                                return rtnVal;
                            }

                            SQL = "";
                            SQL += " INSERT INTO ADMIN.OCS_OORDER (PTNO, BDATE, DEPTCODE, SEQNO, ORDERCODE, SUCODE                                 \r";
                            SQL += "      , BUN, SLIPNO, REALQTY, QTY, NAL, GBDIV, DOSCODE, GBBOTH, GBINFO, GBER, GBSELF                                \r";
                            SQL += "      , GBSPC, BI, DRCODE, REMARK, ENTDATE, GBSUNAP, TUYAKNO, ORDERNO, Res, WRTNO                                   \r";
                            SQL += "      , GBAUTOSEND, OCSDRUG, GBCOPY,Sabun, IP, ASA, PCHASU, SUBUL_WARD                                              \r";
                            SQL += "      , CORDERCODE, CSUCODE, CBUN, OPDNO, POWDER, SEDATION )                                                        \r";
                            SQL += " SELECT Ptno, BDate, DeptCode,  SeqNo,     OrderCode, SuCode                                                        \r";
                            SQL += "      , Bun,         SlipNO,    RealQty,   Qty,       Nal * -1                                                      \r";
                            SQL += "      , GbDiv,       DosCode,   GbBoth,    GbInfo,    GbER                                                          \r";
                            SQL += "      , GbSelf,      GbSpc,     Bi,        DrCode,    RemaRK                                                        \r";
                            SQL += "      , SysDate,     '0'                                                                                            \r";
                            SQL += "      , TuyakNo,   OrderNo, res, WRTNO, GBAUTOSEND , OCSDRUG, GBCOPY, Sabun                                         \r";
                            SQL += "      , '" + clsPublic.GstrIpAddress + "',ASA, PCHASU, SUBUL_WARD                                                   \r";
                            SQL += "      , CORDERCODE, CSUCODE, CBUN, OPDNO, POWDER, SEDATION                                                          \r";
                            SQL += "   FROM ADMIN.OCS_OORDER                                                                                       \r";
                            SQL += "  WHERE Ptno     = '" + clsOrdFunction.Pat.PtNo + "'                                                                \r";
                            SQL += "    AND BDate    = TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')                                         \r";
                            SQL += "    AND DeptCode = '" + clsOrdFunction.Pat.DeptCode.Trim() + "'                                                     \r";
                            //SQL += "    AND GbSunap  = '1'                                                                                              \r";
                            SQL += "    AND Orderno  = '" + dt.Rows[i]["ORDERNO"] + "'                                                                  \r";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr + " DC자료를 저장하는데 오류가 발생되었습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                        
                                rtnVal = "NO";
                                return rtnVal;
                            }
                            #endregion
                        }
                    }
                }

                dt.Dispose();
                dt = null;
            }
            else
            {
                SQL = "";
                SQL += "SELECT SUCODE, ORDERNO, GBSTATUS FROM ADMIN.OCS_IORDER                 \r\n";
                SQL += " WHERE PTNO = '" + clsOrdFunction.Pat.PtNo + "'                             \r\n";
                SQL += "   AND BDATE = TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')     \r\n";
                SQL += "   AND DEPTCODE = '" + clsOrdFunction.Pat.DeptCode.Trim() + "'              \r\n";
                SQL += "   AND SUCODE IN (" + strCode1 + ")                                         \r\n";
                SQL += "   AND SUCODE NOT IN ('" + strCode2 + "')                                   \r\n";
                SQL += "   AND GBSTATUS NOT IN('D', 'D-')                                           \r\n";
                SQL += "   AND (NurseID   IS NULL OR NurseId = ' ')                                 \r\n";
                SQL += "   AND NAL > 0                                                              \r\n";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    rtnVal = "NO";
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SQL = "";
                        SQL += " UPDATE ADMIN.OCS_IORDER SET                                       \r";
                        SQL += "        GbStatus = 'D'                                                  \r";
                        SQL += "      , GbSend   = ' '                                                  \r";
                        SQL += "  WHERE ROWID    = '" + dt.Rows[i]["ORDERNO"].ToString().Trim() + "'    \r";
                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                        
                            rtnVal = "NO";
                            return rtnVal;
                        }

                        SQL = "";
                        SQL += " INSERT INTO ADMIN.OCS_IORDER                                              \r";
                        SQL += "        (PTNO, BDATE, SEQNO, DEPTCODE, DRCODE                                   \r";
                        SQL += "      , STAFFID, SLIPNO, ORDERCODE, SUCODE, BUN, GBORDER, CONTENTS, BCONTENTS   \r";
                        SQL += "      , REALQTY, QTY, REALNAL, NAL, DOSCODE                                     \r";
                        SQL += "      , GBINFO, GBSELF, GBSPC, GBNGT, GBER, GBPRN                               \r";
                        SQL += "      , GBDIV, GBBOTH, GBACT, GBTFLAG, GBSEND, GBPOSITION                       \r";
                        SQL += "      , GBSTATUS, NURSEID, ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO             \r";
                        SQL += "      , REMARK, ACTDATE, GBGROUP, GBPORT, ORDERSITE, MULTI, MULTIREMARK, DUR    \r";
                        SQL += "      , GBPICKUP, PICKUPSABUN, PICKUPDATE,EMRSET ,GBIOE,MAYAK,POWDER, SEDATION            \r";
                        SQL += "      , MAYAKREMARK, VER, IP, ENTDATE2, DrCode2, GbTax, GbChk, GbVerb           \r";
                        SQL += "      , Verbal, V_ORDERNO, ASA, PCHASU, SUBUL_WARD, ER24, GSADD                 \r";
                        SQL += "      , CORDERCODE, CSUCODE, CBUN                                               \r"; //2017-10-25 CORDERCODE, CSUCODE, CBUN 추가
                        SQL += "      , BURNADD, OPGUBUN)                                                       \r";
                        SQL += " SELECT Ptno, BDate, Seqno,  DeptCode                                           \r";                        
                        SQL += "      , '" + string.Format("{0:00000}", clsPublic.GnJobSabun) + "'              \r";                        
                        SQL += "      , StaffID,  Slipno, OrderCode, SuCode, Bun, GbOrder, Contents, BContents  \r";
                        SQL += "      , RealQty,     Qty,    RealNal * -1,      Nal * -1, DosCode               \r";
                        SQL += "      , GbInfo,      GbSelf, GbSpc,    GbNgt,   GbEr,     GbPRN                 \r";
                        SQL += "      , GbDiv,       GbBoth, GbAct,    GbTFlag, '*',      GbPosition            \r";
                        SQL += "      , 'D-'                                                                    \r";                        
                        SQL += "      , ' '                                                                     \r";                        
                        SQL += "      , SysDate,     WardCode,RoomCode, Bi,     Orderno,  Remark                \r";
                        SQL += "      , TO_DATE('" + clsOrdFunction.GstrActDate + "','YYYY-MM-DD'), GbGroup     \r";
                        SQL += "      , GbPort,      OrderSite, Multi, MultiRemark, DUR                         \r";                        
                        SQL += "      , '', '', ''                                                              \r";                        
                        SQL += "      , EMRSET ,GBIOE,MAYAK,POWDER, SEDATION, MAYAKREMARK, '" + clsPublic.GstrVer + "'    \r";                        
                        SQL += "      , '" + clsPublic.GstrIpAddress + "', SYSDATE, DrCode2, GbTax, 'D1'        \r";                        
                        SQL += "      , GbVerb, Verbal, V_ORDERNO, ASA, PCHASU, SUBUL_WARD, ER24, GSADD         \r";
                        SQL += "      , CORDERCODE, CSUCODE, CBUN                                               \r"; //2017-10-25
                        SQL += "      , BURNADD, OPGUBUN                                                        \r";
                        SQL += "   FROM ADMIN.OCS_IORDER                                                   \r";
                        SQL += "  WHERE ROWID = '" + dt.Rows[i]["ORDERNO"].ToString().Trim() + "'               \r";
                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr + " 기존자료를 삭제하는데 오류가 발생되었습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);//에러로그 저장                        
                            rtnVal = "NO";
                            return rtnVal;
                        }
                    }                    
                }

                dt.Dispose();
                dt = null;
            }

            return rtnVal;
        }

        public string CHECK_OORDER_IN_REPEATE(PsmhDb pDbCon, string ArgOrderCode, string argPTNO, string argBDate, string argDosCode)
        {
            if (ArgOrderCode == "S/O")  
            {
                return "";
            }

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            long rowcounter;
            GstrYakGubun = "";

            try
            {
                //SQL = "";
                //SQL += " SELECT '1' GBN, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,DEPTCODE             \r";
                //SQL += "   FROM " + ComNum.DB_MED + "OCS_OORDER                                 \r";
                //SQL += "  WHERE PTNO = '" + argPTNO + "'                                        \r";
                //SQL += "   AND BDATE >=TRUNC(SYSDATE-1)                                         \r";
                //SQL += "   AND ORDERCODE = '" + ArgOrderCode + "'                               \r";
                //SQL += "   AND BUN IN ('11', '12', '20')                                        \r";
                ////SQL += "    AND GBSUNAP =  '1'                                                  \r"; //수납처리된 것만 조회
                //add 2013-03-21
                //SQL += "  UNION ALL                                                             \r";
                SQL += " SELECT /*+ index( " + ComNum.DB_MED + "ocs_iorder INXOCS_IORDER1) */   \r";
                SQL += "       '2' GBN, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,DEPTCODE              \r";
                SQL += "   FROM " + ComNum.DB_MED + "OCS_IORDER                                 \r";
                SQL += "  WHERE PTNO = '" + argPTNO + "'                                        \r";
                //SQL += "   AND BDATE = TRUNC(SYSDATE)                                           \r";
                SQL += "   AND BDATE = TO_DATE('" + argBDate + "', 'YYYY-MM-DD')                \r";
                SQL += "   AND ORDERCODE = '" + ArgOrderCode + "'                               \r";
                SQL += "   AND GbStatus IN  (' ')                                               \r";
                SQL += "   AND BUN IN ('11', '12', '20')                                        \r";                
                SQL += "   AND GBPRN <> 'P'                                                     \r";
                SQL += "   AND GBVERB <> 'Y'                                                    \r";
                SQL += "   AND GBTFLAG NOT IN ('T', 'O')                                        \r";
                SQL += "   AND (GBIOE IS NULL OR  GBIOE ='I' OR GBIOE ='EI' )                   \r";
                //2020-05-26 용법 조건 추가
                SQL += "   AND DOSCODE = '" + argDosCode + "'                                   \r";
                SQL += "   AND DOSCODE NOT IN (SELECT DOSCODE                                   \r";
                SQL += "                       FROM ADMIN.OCS_ODOSAGE                      \r";
                SQL += "                       WHERE 1=1                                        \r";
                SQL += "                       AND DOSNAME LIKE 'Others/1time%'                 \r"; 
                SQL += "                       AND DELDATE IS NULL)                             \r";
                //2020-08-27 안정수 조건 추가 전산의뢰 <2020-2165>
                SQL += "   AND TRIM(ORDERCODE) NOT IN (SELECT CODE                              \r";
                SQL += "                               FROM " + ComNum.DB_ERP + "DRUG_BCODE     \r";
                SQL += "                               WHERE 1=1                                \r";
                SQL += "                                 AND GUBUN = '처방전출력제외약품코드'   \r";
                SQL += "                               UNION ALL                                \r";
                SQL += "                               SELECT TRIM(JEPCODE)                     \r";
                SQL += "                               FROM " + ComNum.DB_ERP + "DRUG_MASTER2   \r";
                SQL += "                               WHERE 1=1                                \r";
                SQL += "                                 AND EFFECTBUN = '03'                   \r";
                SQL += "                               )\r";                
                SQL += "   GROUP BY TO_CHAR(BDATE,'YYYY-MM-DD') ,DEPTCODE                       \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "CHECK_OORDER_IN " + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");

                    return GstrYakGubun;
                }
                rowcounter = dt.Rows.Count;

                int i = 0;

                if (rowcounter > 0)
                {
                    clsPublic.GstrMsgList = "<<당일 혹은 전일 동일처방이 있습니다,날짜와 오더를 확인 하십시오>" + "\r\r";

                    for (int nchk = 0; nchk < rowcounter; nchk++)
                    {
                        clsPublic.GstrMsgList += nchk + 1 + ".이미 " + (dt.Rows[i]["GBN"].ToString().Trim()) == "1" ? "외래" : "입원(ER포함)" + " OCS에서 " +
                                          dt.Rows[i]["BDATE"].ToString().Trim() + "  " + dt.Rows[i]["DEPTCODE"].ToString().Trim() +
                                          " 과에서 Order Code가 있습니다" + "\r\r";
                        i++;
                    }

                    clsPublic.GstrMsgList += "[처방 조회 및 검사 결과 확인] 창에서 확인 바랍니다." + "\r\r";

                    clsPublic.GstrMsgList += "추가를 하시겠습니까 ??" + "\r\r";

                    if (MessageBox.Show(clsPublic.GstrMsgList, "중복처방사용여부", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        dt.Dispose();
                        dt = null;
                        return GstrYakGubun;
                    }
                    else
                    {
                        dt.Dispose();
                        dt = null;
                        GstrYakGubun = "Y";
                        return GstrYakGubun;
                    }
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    return GstrYakGubun;
                }
            }

            catch (OracleException ex)
            {
                ComFunc.MsgBox("함수명 : " + "CHECK_OORDER_IN_REPEATE " + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "CHECK_OORDER_IN_REPEATE " + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }
        }
    }
}
