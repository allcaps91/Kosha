//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;


/// <summary>
/// Description : 
/// Author : 박병규
/// Create Date : 2017.08.15
/// </summary>
/// <history>
/// clsBasCard 위치를 ComPmpaLibB의 clsCardApprov로 이동시킴 2018.02.09  K.M.C
/// </history>
/// <seealso cref="gBasCard.bas"/> 

//namespace ComBase
//{

//    public class clsBasCard
//    {
//        [DllImport("AuthComm.dll")]
//        public static extern void SetServer(string strIP, long nPort);


//        #region //GBasCard(GBasCard.bas)
//        public string gstrDbYN = "Y"; //승인DATA를 DB에 저장시
//        public string gstrCardOpenDate = "2003-11-18"; //카드오픈일자
//        public string gstrHostIP = "127.0.0.1"; //병원서버CARD IP
//        public string gstrPort = "6578"; //병원서버CARD PORT

//        public string gstrCdPtno;               // 병록번호
//        public string gstrCdJumin;              // 주민등록번호
//        public string gstrCdSName;              // 수진자명
//        public string gstrCdDeptCode;           // 진료과
//        public string gstrCdGbIo;               // 외래/입원
//        public string gstrCdPCode;              // 어디에서 카드수납했는지구분
//        public string gstrCdPart;               // Part
//        public string gstrOriginDate;           // 원승인일자
//        public string gstrCdRemark;             // Slip에 Remark에 들어가야할 내용
//        public string gstrCdCardNo;             // 암호화되지안은 카드번호/유효기간
//        public string gstrCdJup;                // 수납 예약금 카드 수납여부 또는 카드종류여부
//        public string gstrCdDaeJup;             // 수납 대리접수비 카드 수납여부 또는 카드종류여부
//        public string gstrCdSup;                // 수납 진료비 카드 수납여부 또는 카드종류여부
//        public string gstrCdRemarkJup;          // Slip에 Remark에 들어가야할 내용(수납시 예약비)
//        public string gstrCdRemarkDaeJup;       // Slip에 Remark에 들어가야할 내용(수납시 대리접수비)
//        public string gstrCdRemarkSup;          // Slip에 Remark에 들어가야할 내용(수납시 영수비)
//        public string[] gstrCdRemarkSupMaulti = new string[5];      // 여러개의 카드수납시 카드금액
//        public string[] gstrCdRemarkCodeMaulti = new string[5];     // 여러개의 카드수납시 병원에서 사용하고 있는 카드코드
//        public string gstrCdInsertFlag;         // 입원수납시 IPD_MASTER에 한번만 INSERT하기 위해서
//        public long gnCdSupAmt_1;               // 영수 카드 금액(예약비 제외금액)
//        public long gnCdSupCash;                // 영수 현금 금액
//        public long gnCdDaeJup;                 // 대리접수비
//        public long glngCdAmt;                  // 총금액
//        public string gstrCdJupOriginno;        // 예약접수시 카드 승인번호
//        public string gstrCdDaeJupOriginno;     // 예약접수시 카드 승인번호
//        public string gstrCdSupOriginno;        // 접수또는수납시 카드 승인번호
//        public string gstrRemark;               // 승인테이블에 들어가야할 내용
//        public string gstrCdOriginDate;         // 승인일시
//        public string gstrPrintYn;              // 매출전표 출력시
//        public string gIngCdJupCardSeqNo;       // 수납예약시 카드일련번호
//        public string gIngCdDaeJupCardSeqNo;    // 수납대리접수시 카드일련번호
//        public string gIngCdSupCardSeqNo;       // 수납시 카드일련번호
//        public int gIntCdIpdSeqno;              // 퇴원수납시의 영수증일련번호
//        public int gIntCnt;                     // 같은승인번호에 해당되는 Slip에 Seqno(일련번호)
//        public int intTimeout;                  // Timer 설정시 Time값
//        public string GstrCardGubun;            // 승인.취소구분(1.승인,0.취소)
//        public string GstrCardJob;              // 작업구분(Menual,Approv,Cancel)
//        public string GstrCardJob2;             // 퇴원 영수증 카드 승인시 2번 출력함.
//        public string GstrCenterGbn;            // 건강증진센타 구분 1.건진 2.종검
//        public long GnHPano;                    // 건진번호
//        public long GnHWRTNO;                   // 건진접수번호

//        public static string GstrCashCard; //현금영수증 확인번호
//        public static long GnOgAmt; //산전지원금액
//        public static long GnOgJanAmt; //산전지원잔액
//        #endregion



//        /// <summary>
//        /// Description : 카드에서 사용하는 전역변수 초기화
//        /// Author : 박병규
//        /// Create Date : 2017.08.15
//        /// </summary>
//        /// <seealso cref="GBasCard.bas"/> 
//        public void CardVariable_Clear()
//        {
//            clsType.AcctReqData RSD = new clsType.AcctReqData();
//            clsType.AcctResData RD = new clsType.AcctResData();

//            gstrCdPtno = "";
//            gstrCdJumin = "";
//            gstrCdSName = "";
//            gstrCdDeptCode = "";
//            gstrCdPCode = "";
//            GstrCenterGbn = "";

//            GnHPano = 0; GnHWRTNO = 0;

//            //if (gstrCdGbIo.Trim() == "O")
//            //{
//                gstrCdGbIo = "";
//                gstrCdPart = "";
//            //}

//            gstrRemark = "";
//            gIntCnt = 0;
//            glngCdAmt = 0;
//            gnCdSupCash = 0;
//            gnCdSupAmt_1 = 0;
//            gstrCdJupOriginno = "";
//            gstrCdSupOriginno = "";


//            //카드승인요청
//            RSD.VanGb = "";
//            RSD.OrderGb = "";
//            RSD.MDate = "";
//            RSD.SEQNO = 0;
//            RSD.OrderNo = "";
//            RSD.ClientId = "";
//            RSD.EntryMode = "";
//            RSD.CardLength = 0;
//            RSD.CardData = "";
//            RSD.CardDivide = 0;
//            RSD.TotAmt = 0;
//            RSD.OldAppDate = "";
//            RSD.OldAppTime = "";
//            RSD.OldAppNo = "";
//            RSD.SectionNo = "";
//            RSD.SiteID = "";
//            RSD.CardSeqNo = 0;
//            RSD.Cardthru = "";
//            RSD.ASaleDate = "";
//            RSD.AApproveNo = "";
//            RSD.CardData2 = "";
//            RSD.Gubun = "";
//            RSD.CashBun = "";
//            RSD.OgAmt = 0;
//            RSD.CanSayu = "";
//            RSD.KeyIn = "";

//            //카드승인응답
//            RD.VanGb = "";
//            RD.OrderGb = "";
//            RD.MDate = "";
//            RD.SEQNO = 0;
//            RD.OrderNo = "";
//            RD.ClientId = "";
//            RD.ReplyStat = "";
//            RD.ApprovalDate = "";
//            RD.ApprovalTime = "";
//            RD.ApprovalNo = "";
//            RD.BankId = "";
//            RD.BankName = "";
//            RD.MemberNo = "";
//            RD.PublishBank = "";
//            RD.CardName = "";
//            RD.Massage = "";
//            RD.HPay = "";
//        }

//        #region //Card_Sign_image.bas
//        public static string GstrCardPrtChk;               //카드영수증 인쇄여부
//        public static string GstrCardApprov_Info;          //카드승인전체정보
//        #endregion




//    }
//}
