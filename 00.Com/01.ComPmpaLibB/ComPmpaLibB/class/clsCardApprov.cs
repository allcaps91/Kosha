using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Text;

/// <summary>
/// Description : 신용카드 공용 모듈
/// Author : 김민철
/// Create Date : 2018.02.05
/// </summary>
/// <seealso cref="CardApprov_Daou.bas"/>
/// <history>
/// </history>
namespace ComPmpaLibB
{
    public class Card
    {
        //clsPmpaFunc cPF     = new clsPmpaFunc();

        #region //GBasCard(GBasCard.bas)
        public string gstrDbYN = "Y"; //승인DATA를 DB에 저장시
        public string gstrCardOpenDate = "2003-11-18"; //카드오픈일자
        //public string gstrHostIP = "127.0.0.1"; //병원서버CARD IP
        //public string gstrPort = "6578"; //병원서버CARD PORT

        public string gstrCdPtno;               // 병록번호
        public string gstrCdJumin;              // 주민등록번호
        public string gstrCdSName;              // 수진자명
        public string gstrCdDeptCode;           // 진료과
        public string gstrCdGbIo;               // 외래/입원
        public string gstrCdPCode;              // 어디에서 카드수납했는지구분
        public string gstrCdPart;               // Part
        public string gstrOriginDate;           // 원승인일자
        public string gstrCdRemark;             // Slip에 Remark에 들어가야할 내용
        public string gstrCdCardNo;             // 암호화되지안은 카드번호/유효기간
        public string gstrCdJup;                // 수납 예약금 카드 수납여부 또는 카드종류여부
        public string gstrCdDaeJup;             // 수납 대리접수비 카드 수납여부 또는 카드종류여부
        public string gstrCdSup;                // 수납 진료비 카드 수납여부 또는 카드종류여부
        public string gstrCdRemarkJup;          // Slip에 Remark에 들어가야할 내용(수납시 예약비)
        public string gstrCdRemarkDaeJup;       // Slip에 Remark에 들어가야할 내용(수납시 대리접수비)
        public string gstrCdRemarkSup;          // Slip에 Remark에 들어가야할 내용(수납시 영수비)
        public string[] gstrCdRemarkSupMaulti = new string[5];      // 여러개의 카드수납시 카드금액
        public string[] gstrCdRemarkCodeMaulti = new string[5];     // 여러개의 카드수납시 병원에서 사용하고 있는 카드코드
        public string gstrCdInsertFlag;         // 입원수납시 IPD_MASTER에 한번만 INSERT하기 위해서
        public long gnCdSupAmt_1;               // 영수 카드 금액(예약비 제외금액)
        public long gnCdSupCash;                // 영수 현금 금액
        public long gnCdDaeJup;                 // 대리접수비
        public long glngCdAmt;                  // 총금액
        public string gstrCdJupOriginno;        // 예약접수시 카드 승인번호
        public string gstrCdDaeJupOriginno;     // 예약접수시 카드 승인번호
        public string gstrCdSupOriginno;        // 접수또는수납시 카드 승인번호
        public string gstrRemark;               // 승인테이블에 들어가야할 내용
        public string gstrCdOriginDate;         // 승인일시
        public string gstrPrintYn;              // 매출전표 출력시
        public string gIngCdJupCardSeqNo;       // 수납예약시 카드일련번호
        public string gIngCdDaeJupCardSeqNo;    // 수납대리접수시 카드일련번호
        public string gIngCdSupCardSeqNo;       // 수납시 카드일련번호
        public int gIntCdIpdSeqno;              // 퇴원수납시의 영수증일련번호
        public int gIntCnt;                     // 같은승인번호에 해당되는 Slip에 Seqno(일련번호)
        public int intTimeout;                  // Timer 설정시 Time값
        public string GstrCardGubun;            // 승인.취소구분(1.승인,0.취소)
        public string GstrCardJob;              // 작업구분(Menual,Approv,Cancel)
        public string GstrCardJob2;             // 퇴원 영수증 카드 승인시 2번 출력함.
        public string GstrCenterGbn;            // 건강증진센타 구분 1.건진 2.종검
        public long GnHPano;                    // 건진번호
        public long GnHWRTNO;                   // 건진접수번호

        public static string GstrCashCard;      //현금영수증 확인번호
        public static long GnOgAmt;             //산전지원금액
        public static long GnOgJanAmt;          //산전지원잔액
        #endregion

        #region //Card_Sign_image.bas
        public static string GstrCardPrtChk;               //카드영수증 인쇄여부
        public static string GstrCardApprov_Info;          //카드승인전체정보
        #endregion

        public static string GstrServIP             = "127.0.0.1";      //신용카드 승인 서버 IP
        public static int    GnServPort             = 2373;             //신용카드 승인 서버 PORT
        public static string GstrTerminal_SID_Test  = "99999955";       //승인 단말기 번호 TID  (테스트용)    
        public static string GstrTerminal_SID       = "55009161";       //승인 단말기 번호 TID  (원무팀용)    
        public static string GstrTerminal_SID2      = "55009162";       //승인 단말기 번호 TID  (건진센터용)    
        public static string GstrTerminal_SID3      = "55009163";       //승인 단말기 번호 TID  (장례식장용)  
          
        public static string GstrpFile              = @"c:\cmc\card\svk_sign.bmp";  //싸인 이미지 저장 파일
        public static string GstrcFile              = @"c:\cmc\exe\cashdata\CashData.dat";  //현금영수증 번호 저장 파일
        public static string GstrCardCEO            = "최 순 호";       //대표자
        public static string GstrCardTel            = "054-272-0151";   //대표 전화번호
        public static string GstrMerchantNo         = "72117503";       //카드가맹점번호

        public static bool bCardPrt = false;        //신용카드 영수증 출력 여부
        public static bool bCardPrt2 = false;       //현금영수 영수증 출력 여부

        //변수형태 Class 선언
        public struct CardVariable
        {
            public string gstrCdPtno;
            public string gstrCdJumin;
            public string gstrCdSName;
            public string gstrCdDeptCode;
            public string gstrCdPCode;
            public string gstrCdPart;
            public string gstrRemark;
            public string gstrCdGbIo;
            public string gstrTradeKey;
            public string gstrGubun;
            public long gnHWrtno;
            public long gnHPano;
        }
        public static CardVariable CVar;

        /// <summary>
        /// Description : 카드에서 사용하는 전역변수 초기화
        /// Author : 박병규
        /// Create Date : 2017.08.15
        /// </summary>
        /// <seealso cref="GBasCard.bas"/> 
        public void CardVariable_Clear(ref clsPmpaType.AcctReqData RSD, ref clsPmpaType.AcctResData RD)
        {

            gstrCdPtno = "";
            gstrCdJumin = "";
            gstrCdSName = "";
            gstrCdDeptCode = "";
            gstrCdPCode = "";
            GstrCenterGbn = "";

            GnHPano = 0; GnHWRTNO = 0;

            //if (gstrCdGbIo.Trim() == "O")
            //{
            gstrCdGbIo = "";
            gstrCdPart = "";
            //}

            gstrRemark = "";
            gIntCnt = 0;
            glngCdAmt = 0;
            gnCdSupCash = 0;
            gnCdSupAmt_1 = 0;
            gstrCdJupOriginno = "";
            gstrCdSupOriginno = "";


            //카드승인요청
            RSD.VanGb = "";
            RSD.OrderGb = "";
            RSD.MDate = "";
            RSD.SEQNO = 0;
            RSD.OrderNo = "";
            RSD.ClientId = "";
            RSD.EntryMode = "";
            RSD.CardLength = 0;
            RSD.CardData = "";
            RSD.CardDivide = 0;
            RSD.TotAmt = 0;
            RSD.OldAppDate = "";
            RSD.OldAppTime = "";
            RSD.OldAppNo = "";
            RSD.SectionNo = "";
            RSD.SiteID = "";
            RSD.CardSeqNo = 0;
            RSD.Cardthru = "";
            RSD.ASaleDate = "";
            RSD.AApproveNo = "";
            RSD.CardData2 = "";
            RSD.Gubun = "";
            RSD.CashBun = "";
            RSD.OgAmt = 0;
            RSD.CanSayu = "";
            RSD.KeyIn = "";

            //카드승인응답
            RD.VanGb = "";
            RD.OrderGb = "";
            RD.MDate = "";
            RD.SEQNO = 0;
            RD.OrderNo = "";
            RD.ClientId = "";
            RD.ReplyStat = "";
            RD.ApprovalDate = "";
            RD.ApprovalTime = "";
            RD.ApprovalNo = "";
            RD.BankId = "";
            RD.BankName = "";
            RD.MemberNo = "";
            RD.PublishBank = "";
            RD.CardName = "";
            RD.Massage = "";
            RD.HPay = "";
            RD.Trade = "";
            RD.ReqMsg = "";
        }
        
        public StringBuilder insertLeftJustify(StringBuilder target, string item, int maxLen)
        {
            int myLen = maxLen;

            if (item.Length < maxLen)
            {
                target.Append(item);

                myLen = myLen - item.Length;

                for (int i = 0; i < myLen; i++)
                {
                    target.Append(" ");
                }

                return target;
            }
            else if (item.Length == maxLen)
            {
                target.Append(item);

                return target;
            }
            else
            {
                for (int i = 0; i < myLen; i++)
                {
                    target.Append(item[i]);
                }

                return target;
            }
        }

        public string Message(string input)
        {
            string output = string.Empty;

            switch (input)
            {
                case "K001":
                    output = "[" + input + "]" + "리더기 에러" + "\r\n";
                    break;
                case "K002":
                    output = "[" + input + "]" + "IC 카드 거래 불가" + "\r\n";
                    break;
                case "K003":
                    output = "[" + input + "]" + "M/S카드 거래 불가" + "\r\n";
                    break;
                case "K004":
                    output = "[" + input + "]" + "마그네틱 카드를 읽혀주세요(FALL-BACK)" + "\r\n";
                    break;
                case "K005":
                    output = "[" + input + "]" + "IC카드가 삽입되어 있습니다. 제거해주세요." + "\r\n";
                    break;
                case "K006":
                    output = "[" + input + "]" + "카드거절" + "\r\n";
                    break;
                case "K007":
                    output = "[" + input + "]" + "결제도중 카드를 제거하였습니다." + "\r\n";
                    break;
                case "K008":
                    output = "[" + input + "]" + "결제도중 카드를 제거하였습니다." + "\r\n";
                    break;
                case "K011":
                    output = "[" + input + "]" + "IC리더기 키 다운로드 요망" + "\r\n";
                    break;
                case "K012":
                    output = "[" + input + "]" + "리더기 템퍼 오류" + "\r\n";
                    break;
                case "K013":
                    output = "[" + input + "]" + "상호인증오류" + "\r\n";
                    break;
                case "K014":
                    output = "[" + input + "]" + "암/복호와 오류" + "\r\n";
                    break;
                case "K015":
                    output = "[" + input + "]" + "무결성 검사 실패(다우데이타 관리자에게 문의 하시기 바랍니다.)" + "\r\n";
                    break;
                case "K101":
                    output = "[" + input + "]" + "커맨드 전달 실패" + "\r\n";
                    break;
                case "K102":
                    output = "[" + input + "]" + "키트로닉스 모듈 타임아웃" + "\r\n";
                    break;
                case "K901":
                    output = "[" + input + "]" + "카드사 파라미터 데이터 오류" + "\r\n";
                    break;
                case "K902":
                    output = "[" + input + "]" + "카드사 파라미터 반영불가" + "\r\n";
                    break;
                case "K990":
                    output = "[" + input + "]" + "사용자 강제종료" + "\r\n";
                    break;
                case "K997":
                    output = "[" + input + "]" + "리더기에서 전달받은 데이터이상" + "\r\n";
                    break;
                case "K998":
                    output = "[" + input + "]" + "리더기 타임아웃" + "\r\n";
                    break;
                case "K999":
                    output = "[" + input + "]" + "전문 오류" + "\r\n";
                    break;
                case "K081":
                    output = "[" + input + "]" + "IC카드를 넣어주세요" + "\r\n";
                    break;
                case "K082":
                    output = "[" + input + "]" + "처리불가 상태" + "\r\n";
                    break;
                case "K083":
                    output = "[" + input + "]" + "카드 입력 취소" + "\r\n";
                    break;
                case "S000":
                    output = "[" + input + "]" + "외부 서명 데이터 입력" + "\r\n";
                    break;
                case "S001":
                    output = "[" + input + "]" + "외부 서명 데이터 입력 실패 ( Timou Out )" + "\r\n";
                    break;
                case "S100":
                    output = "[" + input + "]" + "외부 서명 데이터 입력 성공" + "\r\n";
                    break;
                case "S111":
                    output = "[" + input + "]" + "외부 서명 데이터 입력 취소" + "\r\n";
                    break;
                case "S052":
                    output = "[" + input + "]" + "신용 카드 거래 취소" + "\r\n";
                    break;
                case "S053":
                    output = "[" + input + "]" + "현금 영수증 거래 취소" + "\r\n";
                    break;
                case "S054":
                    output = "[" + input + "]" + "현금 IC 거래 취소" + "\r\n";
                    break;
                case "K110":
                    output = "[" + input + "]" + "KIOSK 카드 리딩 완료" + "\r\n";
                    break;
                default:
                    output = "[" + input + "]" + "없는로그" + "\r\n";
                    break;
            }

            if (input.Length > 8)
            {
                if (input.Substring(4, 4) == "C010")
                {
                    string ErrCode = input.Substring(0, 4);      // 에러 코드

                    string ErrCnt = input.Substring(8, 2);       // 에러 갯수              00 : 정상 , 1 이상 에러 발생 갯수
                    string DllCode = input.Substring(10, 2);     // DLL 에러               00 정상, 01 에러
                    string NetCode = input.Substring(12, 2);     // 네트워크 상태          00 정상, 01 에러
                    string AppCode = input.Substring(14, 2);     // 클라이언트 연결 상태   00 정상, 01 에러
                    string ReaderCode = input.Substring(16, 2);  // 리더기 상태            00 정상, 01 에러
                    string SignCode = input.Substring(18, 2);    // 서명패드 상태          00 정상, 01 에러
                    string IniCode = input.Substring(20, 2);     // INI 파일 상태          00 정상, 01 에러

                    string strResultStat = "에러: " + ErrCnt;

                    if (DllCode == "00")
                    {
                        strResultStat += " DLL 상태: OK" + "\n" + "\r";
                    }
                    else
                    {
                        strResultStat += " DLL 상태: FAIL" + "\n" + "\r";
                    }

                    if (NetCode == "00")
                    {
                        strResultStat += " 네트워크 상태: OK" + "\n" + "\r";
                    }
                    else
                    {
                        strResultStat += " 네트워크 상태: FAIL" + "\n" + "\r";
                    }
                    
                    if (AppCode == "00")
                    {
                        strResultStat += " 어플리케이션 상태: OK" + "\n";
                    }
                    else
                    {
                        strResultStat += " 어플리케이션 상태: FAIL" + "\n";
                    }

                    if (ReaderCode == "00")
                    {
                        strResultStat += " 리더기 상태: OK" + "\n";
                    }
                    else
                    {
                        strResultStat += " 리더기 상태: FAIL" + "\n";
                    }

                    if (SignCode == "00")
                    {
                        strResultStat += " 서명패드 상태: OK" + "\n";
                    }
                    else
                    {
                        strResultStat += " 서명패드 상태: FAIL" + "\n";
                    }

                    if (IniCode == "00")
                    {
                        strResultStat += " INI 상태: OK" + "\n" + "\r";
                    }
                    else
                    {
                        strResultStat += " INI 상태: FAIL" + "\n" + "\r";
                    }

                    output = strResultStat;
                }
            }

            return output;
        }

        public void SELECT_JUMIN(PsmhDb pDbCon, string ArgPtno)
        {
            DataTable Dt = null;
            clsPmpaFunc cPF = new clsPmpaFunc();

            //2018.06.13 박병규 : 변수 클리어추가
            CVar.gstrCdJumin = "";
            CVar.gstrCdPtno = "";
            CVar.gstrCdSName = "";

            Dt = cPF.Get_BasPatient(pDbCon, ArgPtno);

            if (Dt.Rows.Count > 0)
            {
                CVar.gstrCdJumin = Dt.Rows[0]["JUMIN1"].ToString().Trim() + "*******";
                CVar.gstrCdPtno = ArgPtno;
                CVar.gstrCdSName = Dt.Rows[0]["SNAME"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;
        }

        public bool CardApprov_Insert(PsmhDb pDbCon, string strJob)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strWDate = string.Empty;

            bool rtnVal = true;

            ComFunc.ReadSysDate(pDbCon);

            if (VB.Left(clsPmpaType.RD.MDate, 2) != "00")
            {
                clsPmpaType.RD.MDate = VB.Left(clsPmpaType.RD.MDate, 4) + "-" + VB.Mid(clsPmpaType.RD.MDate, 5, 2) + "-" + VB.Mid(clsPmpaType.RD.MDate, 7, 2) + " " + VB.Mid(clsPmpaType.RD.MDate, 9, 2) + ":" + VB.Mid(clsPmpaType.RD.MDate, 11, 2);
                strWDate = clsPmpaType.RD.MDate.Substring(0, 10);
            }

            try
            {
                SQL += "";
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "CARD_APPROV                                                     \r\n";
                SQL += "       (ActDate, CDSEQNO,  Pano, Jumin, SName, GbIo, DeptCode, PCode, Part                         \r\n";
                SQL += "        ,TranHeader,CardSeqNo, Remark, TeminalId, TranDate, InputMethod                            \r\n";
                SQL += "        ,TradeAmt, ApprovDate, FiName, AccepterName, FiCode                                  \r\n";
                SQL += "        ,AccountNo, OriginNo, OriginDate, CardNo, Period, FullCardNo, EntSabun                     \r\n";
                SQL += "        ,EnterDate,GUBUN,PtGubun,OriginNo2,HPay,IC                                         \r\n";
                if (strJob == "CARD")
                { 
                    SQL += "        ,InstPeriod,OgAmt                   \r\n";
                }
                else if (strJob == "CASH")
                {
                    SQL += "         ,CashCan                           \r\n";
                }
                SQL += " ) VALUES (                                                                                        \r\n";
                SQL += "        TO_DATE('" + clsPublic.GstrSysDate + "' ,'YYYY-MM-DD')                      --ActDate      \r\n";  //회계일자
                SQL += "       , " + clsPmpaType.RSD.SEQNO + "                                              --CDSEQNO      \r\n";  //거래일련번호
                SQL += "       ,'" + CVar.gstrCdPtno + "'                                                   --Pano         \r\n";  //등록번호
                SQL += "       ,'" + CVar.gstrCdJumin + "'                                                  --Jumin        \r\n";  //주민번호
                SQL += "       ,'" + CVar.gstrCdSName + "'                                                  --SName        \r\n";  //성명
                SQL += "       ,'" + CVar.gstrCdGbIo + "'                                                   --GbIo         \r\n";  //입/외
                SQL += "       ,'" + CVar.gstrCdDeptCode + "'                                               --DeptCode     \r\n";  //진료과
                SQL += "       ,'" + CVar.gstrCdPCode + "'                                                  --PCode        \r\n";  //카드수납구분
                SQL += "       ,'" + CVar.gstrCdPart + "'                                                   --Part         \r\n";  //입력조
                SQL += "       ,'" + clsPmpaType.RD.OrderGb + "'                                            --TranHeader   \r\n";  //거래구분
                SQL += "       , " + clsPmpaType.RSD.CardSeqNo + "                                          --CardSeqNo    \r\n";  //승인일련번호
                SQL += "       ,'" + CVar.gstrRemark + "'                                                   --Remark       \r\n";  //비고
                SQL += "       ,'" + clsPmpaType.RSD.SiteID + "'                                            --TeminalId    \r\n";  //단말기번호
                SQL += "       ,     SYSDATE                                                                --TranDate     \r\n";  //거래일시
                SQL += "       ,'" + clsPmpaType.RSD.EntryMode + "'                                         --InputMethod  \r\n";  //입력구분
                SQL += "       , " + clsPmpaType.RSD.TotAmt + "                                             --TradeAmt     \r\n";  //총금액
                SQL += "       ,     TO_DATE('" + clsPmpaType.RD.MDate + "' ,'YYYY-MM-DD HH24:MI')          --ApprovDate   \r\n";  //승인일시
                SQL += "       ,'" + clsPmpaType.RD.PublishBank.Trim() + "'                                 --FiName       \r\n";  //발급사명
                SQL += "       ,'" + clsPmpaType.RD.BankName.Trim() + "'                                    --AccepterName \r\n";  //매입처명
                SQL += "       ,'" + clsPmpaType.RD.BankId.Trim() + "'                                      --FiCode       \r\n";  //매입처코드
                SQL += "       ,'" + clsPmpaType.RD.MemberNo.Trim() + "'                                    --AccountNo    \r\n";  //가맹점호
                SQL += "       ,'" + clsPmpaType.RD.ApprovalNo.Trim() + "'                                  --OriginNo     \r\n";  //(원)승인번호
                SQL += "       ,     TO_DATE('" + strWDate + "' ,'YYYY-MM-DD')                              --OriginDate   \r\n";  //원거래일자
                SQL += "       ,'" + clsPmpaType.RSD.CardData.Trim() + "'                                   --CardNo       \r\n";  //카드번호
                SQL += "       ,''                                                                          --Period       \r\n";  //유효기간
                SQL += "       ,''                                                                          --FullCardNo   \r\n";  //전체카드번호
                SQL += "       ,'" + clsType.User.IdNumber + "'                                             --EntSabun     \r\n";  //입력자사번
                SQL += "       ,     SYSDATE                                                                --EnterDate    \r\n";  //입력일자
                SQL += "       ,'" + clsPmpaType.RSD.Gubun + "'                                             --GUBUN        \r\n";  //카드 현금 구분
                SQL += "       ,'3'                                                                         --PtGubun      \r\n";  //3.다우카드
                SQL += "       ,'" + clsPmpaType.RSD.AApproveNo.Trim() + "'                                 --OriginNo2    \r\n";  //원승인번호(취소시)
                
                SQL += "       ,'" + clsPmpaType.RD.HPay + "'                                               --HPay         \r\n";  //모바일페이 
                SQL += "       ,'Y'                                                                         --IC           \r\n";  //IC칩거래
                if (strJob == "CARD")
                {
                    SQL += "       ,'" + clsPmpaType.RSD.CardDivide + "'                                    --InstPeriod   \r\n";  //할부개월수
                    SQL += "       ,     0                                                                  --OgAmt        \r\n";  //산전승인금액
                }
                else if (strJob == "CASH")
                {
                    SQL += "       ,'" + clsPmpaType.RSD.CanSayu + "'                                       --CashCan      \r\n";  //현금취소사유
                }
                SQL += "       )                                                                                               ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    rtnVal = false;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        public bool CardApprov_Insert_Mobile(PsmhDb pDbCon, string strJob)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strWDate = string.Empty;

            bool rtnVal = true;

            ComFunc.ReadSysDate(pDbCon);

            if (VB.Left(clsPmpaType.RD.MDate, 2) != "00")
            {
                clsPmpaType.RD.MDate = VB.Left(clsPmpaType.RD.MDate, 4) + "-" + VB.Mid(clsPmpaType.RD.MDate, 5, 2) + "-" + VB.Mid(clsPmpaType.RD.MDate, 7, 2) + " " + VB.Mid(clsPmpaType.RD.MDate, 9, 2) + ":" + VB.Mid(clsPmpaType.RD.MDate, 11, 2);
                strWDate = clsPmpaType.RD.MDate.Substring(0, 10);
            }

            try
            {
                SQL += "";
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "CARD_APPROV                                                     \r\n";
                SQL += "       (ActDate, CDSEQNO,  Pano, Jumin, SName, GbIo, DeptCode, PCode, Part                         \r\n";
                SQL += "        ,TranHeader,CardSeqNo, Remark, TeminalId, TranDate, InputMethod                            \r\n";
                SQL += "        ,TradeAmt, ApprovDate, FiName, AccepterName, FiCode                                  \r\n";
                SQL += "        ,AccountNo, OriginNo, OriginDate, CardNo, Period, FullCardNo, EntSabun                     \r\n";
                SQL += "        ,EnterDate,GUBUN,PtGubun,OriginNo2,HPay,IC                                         \r\n";
                if (strJob == "CARD")
                {
                    SQL += "        ,InstPeriod,OgAmt                   \r\n";
                }
                else if (strJob == "CASH")
                {
                    SQL += "         ,CashCan                           \r\n";
                }
                SQL += " ) VALUES (                                                                                        \r\n";
                SQL += "        TO_DATE('" + clsPublic.GstrSysDate + "' ,'YYYY-MM-DD')                      --ActDate      \r\n";  //회계일자
                SQL += "       , " + clsPmpaType.RSD.SEQNO + "                                              --CDSEQNO      \r\n";  //거래일련번호
                SQL += "       ,'" + CVar.gstrCdPtno + "'                                                   --Pano         \r\n";  //등록번호
                SQL += "       ,'" + CVar.gstrCdJumin + "'                                                  --Jumin        \r\n";  //주민번호
                SQL += "       ,'" + CVar.gstrCdSName + "'                                                  --SName        \r\n";  //성명
                SQL += "       ,'" + CVar.gstrCdGbIo + "'                                                   --GbIo         \r\n";  //입/외
                SQL += "       ,'" + CVar.gstrCdDeptCode + "'                                               --DeptCode     \r\n";  //진료과
                SQL += "       ,'" + CVar.gstrCdPCode + "'                                                  --PCode        \r\n";  //카드수납구분
                SQL += "       ,'" + CVar.gstrCdPart + "'                                                   --Part         \r\n";  //입력조
                SQL += "       ,'" + clsPmpaType.RD.OrderGb + "'                                            --TranHeader   \r\n";  //거래구분
                SQL += "       , " + clsPmpaType.RSD.CardSeqNo + "                                          --CardSeqNo    \r\n";  //승인일련번호
                SQL += "       ,'" + CVar.gstrRemark + "'                                                   --Remark       \r\n";  //비고
                SQL += "       ,'" + clsPmpaType.RSD.SiteID + "'                                            --TeminalId    \r\n";  //단말기번호
                SQL += "       ,     SYSDATE                                                                --TranDate     \r\n";  //거래일시
                SQL += "       ,'" + clsPmpaType.RSD.EntryMode + "'                                         --InputMethod  \r\n";  //입력구분
                SQL += "       , " + clsPmpaType.RSD.TotAmt + "                                             --TradeAmt     \r\n";  //총금액
                SQL += "       ,     TO_DATE('" + clsPmpaType.RD.MDate + "' ,'YYYY-MM-DD HH24:MI')          --ApprovDate   \r\n";  //승인일시
                SQL += "       ,'" + clsPmpaType.RD.PublishBank.Trim() + "'                                 --FiName       \r\n";  //발급사명
                SQL += "       ,'" + clsPmpaType.RD.BankName.Trim() + "'                                    --AccepterName \r\n";  //매입처명
                SQL += "       ,'" + clsPmpaType.RD.BankId.Trim() + "'                                      --FiCode       \r\n";  //매입처코드
                SQL += "       ,'" + clsPmpaType.RD.MemberNo.Trim() + "'                                    --AccountNo    \r\n";  //가맹점호
                SQL += "       ,'" + clsPmpaType.RD.ApprovalNo.Trim() + "'                                  --OriginNo     \r\n";  //(원)승인번호
                SQL += "       ,     TO_DATE('" + strWDate + "' ,'YYYY-MM-DD')                              --OriginDate   \r\n";  //원거래일자
                SQL += "       ,'" + clsPmpaType.RSD.CardData.Trim() + "'                                   --CardNo       \r\n";  //카드번호
                SQL += "       ,''                                                                          --Period       \r\n";  //유효기간
                SQL += "       ,''                                                                          --FullCardNo   \r\n";  //전체카드번호
                SQL += "       ,'" + clsType.User.IdNumber + "'                                             --EntSabun     \r\n";  //입력자사번
                SQL += "       ,     SYSDATE                                                                --EnterDate    \r\n";  //입력일자
                SQL += "       ,'" + clsPmpaType.RSD.Gubun + "'                                             --GUBUN        \r\n";  //카드 현금 구분
                SQL += "       ,'4'                                                                         --PtGubun      \r\n";  //3.다우카드
                SQL += "       ,'" + clsPmpaType.RSD.AApproveNo.Trim() + "'                                 --OriginNo2    \r\n";  //원승인번호(취소시)

                SQL += "       ,'" + clsPmpaType.RD.HPay + "'                                               --HPay         \r\n";  //모바일페이 
                SQL += "       ,'Y'                                                                         --IC           \r\n";  //IC칩거래
                if (strJob == "CARD")
                {
                    SQL += "       ,'" + clsPmpaType.RSD.CardDivide + "'                                    --InstPeriod   \r\n";  //할부개월수
                    SQL += "       ,     0                                                                  --OgAmt        \r\n";  //산전승인금액
                }
                else if (strJob == "CASH")
                {
                    SQL += "       ,'" + clsPmpaType.RSD.CanSayu + "'                                       --CashCan      \r\n";  //현금취소사유
                }
                SQL += "       )                                                                                               ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    rtnVal = false;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        public bool CardApprov_Insert_HIC(PsmhDb pDbCon, string strJob)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strWDate = string.Empty;
            string strCenterGbn = string.Empty;

            strCenterGbn = CVar.gstrCdGbIo == "H" ? "1" : "2";
            bool rtnVal = true;

            ComFunc.ReadSysDate(pDbCon);

            if (VB.Left(clsPmpaType.RD.MDate, 2) != "00")
            {
                clsPmpaType.RD.MDate = VB.Left(clsPmpaType.RD.MDate, 4) + "-" + VB.Mid(clsPmpaType.RD.MDate, 5, 2) + "-" + VB.Mid(clsPmpaType.RD.MDate, 7, 2) + " " + VB.Mid(clsPmpaType.RD.MDate, 9, 2) + ":" + VB.Mid(clsPmpaType.RD.MDate, 11, 2);
                strWDate = clsPmpaType.RD.MDate.Substring(0, 10);
            }

            try
            {
                SQL = "";
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "CARD_APPROV_CENTER                                              \r\n";
                SQL += "       (ACTDATE, CDSEQNO,  PANO, HPANO, HWRTNO, JUMIN, SNAME, GBIO, DEPTCODE, PCODE, PART          \r\n";
                SQL += "        ,TRANHEADER,CARDSEQNO, REMARK, TEMINALID, TRANDATE, INPUTMETHOD                            \r\n";
                SQL += "        ,TRADEAMT, APPROVDATE, FINAME, ACCEPTERNAME, FICODE                                        \r\n";
                SQL += "        ,ACCOUNTNO, ORIGINNO, ORIGINDATE, CARDNO, PERIOD, FULLCARDNO, ENTSABUN                     \r\n";
                SQL += "        ,ENTERDATE, GUBUN, ORIGINNO2, IC, CENTERGBN, GUBUN1                                        \r\n";
                if (strJob == "CARD")
                {
                    SQL += "        ,INSTPERIOD                                                                            \r\n";
                }
                else if (strJob == "CASH")
                {
                    SQL += "         ,CASHCAN                                                                              \r\n";
                }
                SQL += " ) VALUES (                                                                                        \r\n";
                SQL += "        TO_DATE('" + clsPublic.GstrSysDate + "' ,'YYYY-MM-DD')                      --ACTDATE      \r\n";  //회계일자
                SQL += "       , " + clsPmpaType.RSD.SEQNO + "                                              --CDSEQNO      \r\n";  //거래일련번호
                SQL += "       ,'" + CVar.gstrCdPtno + "'                                                   --PANO         \r\n";  //등록번호
                SQL += "       , " + CVar.gnHPano + "                                                       --HPANO        \r\n";  //검진번호
                SQL += "       , " + CVar.gnHWrtno + "                                                      --HWRTNO       \r\n";  //접수번호
                SQL += "       ,'" + CVar.gstrCdJumin + "'                                                  --JUMIN        \r\n";  //주민번호
                SQL += "       ,'" + CVar.gstrCdSName + "'                                                  --SNAME        \r\n";  //성명
                SQL += "       ,'" + CVar.gstrCdGbIo + "'                                                   --GBIO         \r\n";  //입/외
                SQL += "       ,'" + CVar.gstrCdDeptCode + "'                                               --DEPTCODE     \r\n";  //진료과
                SQL += "       ,'" + CVar.gstrCdPCode + "'                                                  --PCODE        \r\n";  //카드수납구분
                SQL += "       ,'" + CVar.gstrCdPart + "'                                                   --PART         \r\n";  //입력조
                SQL += "       ,'" + clsPmpaType.RD.OrderGb + "'                                            --TRANHEADER   \r\n";  //거래구분
                SQL += "       , " + clsPmpaType.RSD.CardSeqNo + "                                          --CARDSEQNO    \r\n";  //승인일련번호
                SQL += "       ,'" + CVar.gstrRemark + "'                                                   --REMARK       \r\n";  //비고
                SQL += "       ,'" + clsPmpaType.RSD.SiteID + "'                                            --TEMINALID    \r\n";  //단말기번호
                SQL += "       ,     SYSDATE                                                                --TRANDATE     \r\n";  //거래일시
                SQL += "       ,'" + clsPmpaType.RSD.EntryMode + "'                                         --INPUTMETHOD  \r\n";  //입력구분
                SQL += "       , " + clsPmpaType.RSD.TotAmt + "                                             --TRADEAMT     \r\n";  //총금액
                SQL += "       ,     TO_DATE('" + clsPmpaType.RD.MDate + "' ,'YYYY-MM-DD HH24:MI')          --APPROVDATE   \r\n";  //승인일시
                SQL += "       ,'" + clsPmpaType.RD.PublishBank.Trim() + "'                                 --FINAME       \r\n";  //발급사명
                SQL += "       ,'" + clsPmpaType.RD.BankName.Trim() + "'                                    --ACCEPTERNAME \r\n";  //매입처명
                SQL += "       ,'" + clsPmpaType.RD.BankId.Trim() + "'                                      --FICODE       \r\n";  //매입처코드
                SQL += "       ,'" + clsPmpaType.RD.MemberNo.Trim() + "'                                    --ACCOUNTNO    \r\n";  //가맹점호
                SQL += "       ,'" + clsPmpaType.RD.ApprovalNo.Trim() + "'                                  --ORIGINNO     \r\n";  //(원)승인번호
                SQL += "       ,     TO_DATE('" + strWDate + "' ,'YYYY-MM-DD')                              --ORIGINDATE   \r\n";  //원거래일자
                SQL += "       ,'" + clsPmpaType.RSD.CardData.Trim() + "'                                   --CARDNO       \r\n";  //카드번호
                SQL += "       ,''                                                                          --PERIOD       \r\n";  //유효기간
                SQL += "       ,''                                                                          --FULLCARDNO   \r\n";  //전체카드번호
                SQL += "       ,'" + clsType.User.IdNumber + "'                                             --ENTSABUN     \r\n";  //입력자사번
                SQL += "       ,     SYSDATE                                                                --ENTERDATE    \r\n";  //입력일자
                SQL += "       ,'" + clsPmpaType.RSD.Gubun + "'                                             --GUBUN        \r\n";  //카드 현금 구분
                SQL += "       ,'" + clsPmpaType.RSD.AApproveNo.Trim() + "'                                 --ORIGINNO2    \r\n";  //원승인번호(취소시)
                SQL += "       ,'Y'                                                                         --IC           \r\n";  //IC칩거래
                SQL += "       ,'" + strCenterGbn + "'                                                      --CENTERGBN    \r\n";  //센터구분
                SQL += "       ,'" + CVar.gstrTradeKey + "'                                                 --GUBUN1       \r\n";  //거래구분
                //CVar.gstrCdGbIo
                if (strJob == "CARD")
                {
                    SQL += "       ,'" + clsPmpaType.RSD.CardDivide + "'                                    --INSTPERIOD   \r\n";  //할부개월수
                }
                else if (strJob == "CASH")
                {
                    SQL += "       ,'" + clsPmpaType.RSD.CanSayu + "'                                       --CASHCAN      \r\n";  //현금취소사유
                }
                SQL += "       )                                                                                               ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    rtnVal = false;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        public string gWordByByte(string src, int startCount, int byteCount)
        {
            Encoding myEncoding = Encoding.GetEncoding("ks_c_5601-1987");

            byte[] buf = myEncoding.GetBytes(src);

            return myEncoding.GetString(buf, startCount, byteCount);
        }

        public bool UpDate_Card_Refund(PsmhDb pDbCon, string strRowid)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strWDate = string.Empty;

            bool rtnVal = true;

            try
            {
                SQL += "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "CARD_APPROV                   \r\n";
                SQL += "    SET ORIGINNO2 = '" + clsPmpaType.RSD.AApproveNo + "'    \r\n";
                SQL += "  WHERE ROWID = '" + strRowid + "'                              ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    rtnVal = false;
                }
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        public bool UpDate_Card_Refund_HIC(PsmhDb pDbCon, string strRowid)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strWDate = string.Empty;

            bool rtnVal = true;

            try
            {
                SQL += "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "CARD_APPROV_CENTER                   \r\n";
                SQL += "    SET ORIGINNO2 = '" + clsPmpaType.RSD.AApproveNo + "'    \r\n";
                SQL += "  WHERE ROWID = '" + strRowid + "'                              ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    rtnVal = false;
                }
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        public DataTable sel_CardApprov_Data(PsmhDb pDbCon, string ArgPano, string ArgOrigin)
        {
            DataTable dt = null;

            string SQL = "";
            string SqlErr = "";
            
            try
            {
                SQL = "";
                SQL += " SELECT FINAME, INSTPERIOD, PERIOD, ORIGINNO,CardSeqNo, TRADEAMT,   \r\n";
                SQL += "        TO_CHAR(APPROVDATE,'YYYY-MM-DD HH24:MI') APPROVDATE,SNAME,  \r\n";
                SQL += "        PANO,AccepterName, TRANHEADER, CardNo                       \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "CARD_APPROV                           \r\n";
                SQL += "  WHERE PANO = '" + ArgPano + "'                                    \r\n";
                SQL += "    AND ORIGINNO = '" + ArgOrigin + "'                              ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return dt;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgCardSeqNo"></param>
        /// <param name="ArgPtno"></param>
        /// <returns></returns>
        /// <seealso cref="Card_Sign_info_Set"/>
        public string Card_Sign_Info_Set(PsmhDb pDbCon, long ArgCardSeqNo, string ArgPtno, string ArgGubun ="")
        {
            DataTable DtCard = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            if (ArgGubun == "CENTER")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.PANO,a.SNAME,a.CARDSEQNO, ";
                SQL += ComNum.VBLF + "        a.CardNo,a.Period, ";
                SQL += ComNum.VBLF + "        decode(a.TRANHEADER,'2','카드취소','카드승인') TRANHEADER,  ";
                SQL += ComNum.VBLF + "        a.TradeAmt,a.AccepterName,a.InstPeriod, ";
                SQL += ComNum.VBLF + "        a.OriginNo, ";
                SQL += ComNum.VBLF + "        TO_CHAR(a.APPROVDATE,'YYMMDD HH24:MI') APPROVDATE, ";
                SQL += ComNum.VBLF + "        decode(a.gubun, '1', '카드승인', '현금승인') gubun ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "CARD_APPROV_CENTER a ";
                SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
                SQL += ComNum.VBLF + "    AND a.CardSeqno   = " + ArgCardSeqNo + "  ";
                SQL += ComNum.VBLF + "    AND a.Pano        = '" + ArgPtno + "'  ";
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.PANO,a.SNAME,a.CARDSEQNO, ";
                SQL += ComNum.VBLF + "        a.CardNo,a.Period, ";
                SQL += ComNum.VBLF + "        decode(a.TRANHEADER,'2','카드취소','카드승인') TRANHEADER,  ";
                SQL += ComNum.VBLF + "        a.TradeAmt,a.AccepterName,a.InstPeriod, ";
                SQL += ComNum.VBLF + "        a.OriginNo, ";
                SQL += ComNum.VBLF + "        TO_CHAR(a.APPROVDATE,'YYMMDD HH24:MI') APPROVDATE, ";
                //2018.05.31 박병규 : 카드/현금 구분추가
                SQL += ComNum.VBLF + "        decode(a.gubun, '1', '카드승인', '현금승인') gubun ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "CARD_APPROV a, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "CARD_IMAGE b ";
                SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
                SQL += ComNum.VBLF + "    AND a.CardSeqno   = " + ArgCardSeqNo + "  ";
                SQL += ComNum.VBLF + "    AND a.Pano        = '" + ArgPtno + "'  ";
                //2018.05.27 박병규 : 조건절 변경
                //SQL += ComNum.VBLF + "    AND a.CARDSEQNO   = b.CARDSEQNO ";
                SQL += ComNum.VBLF + "    AND a.CARDSEQNO   = b.CARDSEQNO(+) ";
            }
            
            SqlErr = clsDB.GetDataTable(ref DtCard, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                DtCard.Dispose();
                DtCard = null;
                return null;
            }

            if (DtCard.Rows.Count > 0)
            {
                string strSname = DtCard.Rows[0]["SNAME"].ToString().Trim();
                string strPtno = DtCard.Rows[0]["Pano"].ToString().Trim();
                long nCardSeqNo = Convert.ToInt64(DtCard.Rows[0]["CardSeqno"].ToString().Trim());

                string strCardNo = VB.Left(DtCard.Rows[0]["CardNo"].ToString().Trim(), 8) + "********"; //카드번호
                string strCardName = DtCard.Rows[0]["AccepterName"].ToString().Trim();                      //카드종류
                string strGubun = DtCard.Rows[0]["TRANHEADER"].ToString().Trim();                           //거래유형
                string strDiv = DtCard.Rows[0]["InstPeriod"].ToString().Trim();                             //할부개월
                string strPeriod = DtCard.Rows[0]["Period"].ToString().Trim();                              //거래기간
                string strApprovNo = DtCard.Rows[0]["OriginNo"].ToString().Trim();                          //승인번호
                string strApprovDate = DtCard.Rows[0]["ApprovDate"].ToString().Trim();                      //승인일시
                long nTradeAmt = Convert.ToInt64(DtCard.Rows[0]["TradeAmt"].ToString().Trim());             //승인금액
                string strCCGubun = DtCard.Rows[0]["gubun"].ToString().Trim();                              //거래유형(카드,현금)


                if (strGubun == "카드취소")
                    nTradeAmt = nTradeAmt * -1;

                rtnVal = "2.회원번호" + "{}" + strCardNo + "{}" + "4.카드종류" + "{}" + strCardName + "{}" + "6.거래유형" + "{}" + strGubun + "{}" + "8.할부개월" + "{}" + strDiv + "개월" + "{}" + "10.거래기간" + "{}" + strPeriod + "(년월)" + "{}" + "12.승인번호" + "{}" + strApprovNo + "{}" + "14.거래일시" + "{}" + strApprovDate + "{}" + "16.성      명" + "{}" + strSname + "{}" + "18.외래번호" + "{}" + strPtno + "{}" + "20.승인금액" + "{}" + string.Format("{0:#,##0}", nTradeAmt) + "{}" + "22.카드Seqno" + "{}" + nCardSeqNo + "{}" + "23.카드/현금" + "{}" + strCCGubun + "{}";
            }

            DtCard.Dispose();
            DtCard = null;

            return rtnVal;
        }
    }
}
