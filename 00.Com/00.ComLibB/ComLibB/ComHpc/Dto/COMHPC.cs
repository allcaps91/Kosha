namespace ComLibB.Dto
{
    using ComBase.Mvc;
    using System;

    public class COMHPC : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 접수일자
        /// </summary>
        public string JEPDATE { get; set; }

        /// <summary>
        /// 생일
        /// </summary>
        public string BIRTHDAY { get; set; }

        /// <summary>
        /// 검진(수진)일자(YYYY-MM-DD)
        /// </summary>
        public string SDATE { get; set; }

        /// <summary>
        /// 검진(수진)일자(YYYY-MM-DD)
        /// </summary>
        public string SDATE1 { get; set; }

        /// <summary>
        /// 인터뷰일자(YYYY-MM-DD)
        /// </summary>
        public string IDATE { get; set; }

        /// <summary>
        /// 검진 등록번호
        /// </summary>
        public long PANO { get; set; }

        /// <summary>
        /// 성명
        /// </summary>
        public string SNAME { get; set; }

        /// <summary>
        /// 성별(M.남자 F.여자)
        /// </summary>
        public string SEX { get; set; }

        /// <summary>
        /// 성별/나이
        /// </summary>
		public string SEXAGE { get; set; }


        /// <summary>
        /// 나이
        /// </summary>
        public long AGE { get; set; }

        /// <summary>
        /// 우편번호(지역별통계용)
        /// </summary>
        public string MAILCODE { get; set; }

        /// <summary>
        /// 진행상태(아래참조)
        /// </summary>
        public string GBSTS { get; set; }

        /// <summary>
        /// 검진유형 (TYPE)
        /// </summary>
		public string TYPE { get; set; }

        /// <summary>
        /// 진행상태명칭
        /// </summary>
        public string GBSTS_NM { get; set; }

        /// <summary>
        /// 종검종류
        /// </summary>
        public string GJJONG { get; set; }

        /// <summary>
        /// 개인/단체 구분
        /// </summary>
        public string JONG_GB { get; set; }

        /// <summary>
        /// 종검종류명
        /// </summary>
        public string GJNAME { get; set; }

        /// <summary>
        /// 종검종류명
        /// </summary>
        public string FAMILLY { get; set; }

        /// <summary>
        /// 검진비 부담율 구분(아래참조)
        /// </summary>
        public string BURATE { get; set; }

        /// <summary>
        /// 회사(거래처)코드
        /// </summary>
        public long LTDCODE { get; set; }

        /// <summary>
        /// 감액계정코드
        /// </summary>
        public string GAMCODE { get; set; }

        /// <summary>
        /// 소개직원 사번(소개가 아닌것은 NULL)
        /// </summary>
        public string SABUN { get; set; }

        /// <summary>
        /// 상담여부(Y.방문상담 N.결과우편물발송) , 도착처리(도착:A , 미도착:B)
        /// </summary>
        public string GBSANGDAM { get; set; }

        /// <summary>
        /// 판정 결과코드(A,B,C,D)
        /// </summary>
        public string PANCODE { get; set; }

        /// <summary>
        /// 종합 판정소견
        /// </summary>
        public string PANREMARK { get; set; }

        /// <summary>
        /// 판정의사 사번
        /// </summary>
        public long DRSABUN { get; set; }

        /// <summary>
        /// 판정의사 성명
        /// </summary>
        public string DRNAME { get; set; }

        /// <summary>
        /// 예약취소 일자
        /// </summary>
        public string DELDATE { get; set; }

        /// <summary>
        /// 병원 등록번호(OCS와 연동을 위해 사용함)
        /// </summary>
        public string PTNO { get; set; }

        /// <summary>
        /// 인쇄일자및시각(미인쇄는 NULL)
        /// </summary>
        public DateTime? PRTDATE { get; set; }

        /// <summary>
        /// 결과 우편으로 발송일자
        /// </summary>
        public string MAILDATE { get; set; }

        /// <summary>
        /// 검사실에 전달할 검사항목
        /// </summary>
        public string SEXAMS { get; set; }

        /// <summary>
        /// 접수시 참고사항
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 접수자사번
        /// </summary>
        public long JOBSABUN { get; set; }

        /// <summary>
        /// 접수자성명
        /// </summary>
        public string JOBNAME { get; set; }

        /// <summary>
        /// 최종입력일자
        /// </summary>
        public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// 최종입력일자 HHH:MI
        /// </summary>
        public string ENTTIME_HHMI { get; set; }

        /// <summary>
        /// 바코드발행여부
        /// </summary>
        public string GBEXAM { get; set; }

        /// <summary>
        /// 종검 판정소견 코드
        /// </summary>
        public string PANRECODE { get; set; }

        /// <summary>
        /// 회사미수 자동형성 번호
        /// </summary>
        public long MISUNO { get; set; }

        /// <summary>
        /// 미수청구차액: 사업장1차 접수번호
        /// </summary>
        public long HIC_WRTNO { get; set; }

        /// <summary>
        /// 미수청구차액: 사업장1차 조합부담액
        /// </summary>
        public long HIC_AMT { get; set; }

        /// <summary>
        /// 할인권적용여부(Y,N)
        /// </summary>
        public string HALINYN { get; set; }

        /// <summary>
        /// 신용카드 승인일련번호(1~9999999999)
        /// </summary>
        public long CARDSEQNO { get; set; }

        /// <summary>
        /// 상담여부
        /// </summary>
        public string SANGDAMGBN { get; set; }

        /// <summary>
        /// BMD 2부위구분(Y)
        /// </summary>
        public string GBNBMD { get; set; }

        /// <summary>
        /// 전화상담(시각^^작업자^^)
        /// </summary>
        public string SANGDAMTEL { get; set; }

        /// <summary>
        /// 대장내시경동시(Y)
        /// </summary>
        public string GBENDO { get; set; }

        /// <summary>
        /// 본인상담만 (중요내용일경우) Y
        /// </summary>
        public string SANGDAM_ONE { get; set; }

        /// <summary>
        /// Chest-Dust(분진) 촬영여부(Y)
        /// </summary>
        public string GBDUST { get; set; }

        /// <summary>
        /// 회사수신 동의여부 (Y/N or Null)
        /// </summary>
        public string RESULTSEND { get; set; }

        /// <summary>
        /// 수검자 연락부재 등록(시각^^작업자^^)
        /// </summary>
        public string SANGDAMOUT { get; set; }

        /// <summary>
        /// * : ekg abnormal 결과 표시
        /// </summary>
        public string GBEKG { get; set; }

        /// <summary>
        /// 판정시 간단메모
        /// </summary>
        public string PANMEMO { get; set; }

        /// <summary>
        /// 액팅시 참고사항
        /// </summary>
        public string ACTMEMO { get; set; }

        /// <summary>
        /// 당일상담여부
        /// </summary>
        public string GBDAILY { get; set; }

        /// <summary>
        /// 전화상담
        /// </summary>
        public string GBDAILY2 { get; set; }

        /// <summary>
        /// 내시경장소 구분(1:종검 2:외래)
        /// </summary>
        public string ENDOGBN { get; set; }

        /// <summary>
        /// 수납 완료 여부(Y/N)
        /// </summary>
        public string SUNAP { get; set; }

        /// <summary>
        /// 검사요령
        /// </summary>
        public string EXAMREMARK { get; set; }

        /// <summary>
        /// 수검전날 안내전화 등록(날자 시각  작업자)
        /// </summary>
        public string GUIDETEL { get; set; }

        /// <summary>
        /// 내원상담 Null/1.오전/2.오후
        /// </summary>
        public string AMPM { get; set; }

        /// <summary>
        /// 검사변경정보
        /// </summary>
        public string EXAMCHANGE { get; set; }

        /// <summary>
        /// 가판정 사번
        /// </summary>
        public long NRSABUN { get; set; }

        /// <summary>
        /// 가판정 직원명
        /// </summary>
        public string NRNAME { get; set; }

        /// <summary>
        /// 최종상담 여부(Y / N or NULL)
        /// </summary>
        public string SANGDAMYN { get; set; }

        /// <summary>
        /// 판정일자
        /// </summary>
        public string PANDATE { get; set; }

        /// <summary>
        /// 판정정보(일자+의사성명)
        /// </summary>
        public string PAN_INFO { get; set; }

        /// <summary>
        /// 가판정일자
        /// </summary>
        public string GAPANDATE { get; set; }

        /// <summary>
        /// 가판정정보(일자+판정자이름)
        /// </summary>
        public string GAPAN_INFO { get; set; }

        /// <summary>
        /// 결과지 방문 수령일자
        /// </summary>
        public string RECVDATE { get; set; }

        /// <summary>
        /// 방문 수령 정보
        /// </summary>
        public string RECV_INFO { get; set; }

        /// <summary>
        /// 암구분 ex(1,1,1, ,,, 1)
        /// </summary>
        public string GBAM { get; set; }

        /// <summary>
        /// 기타암명칭
        /// </summary>
        public string ETCAM { get; set; }

        /// <summary>
        /// 감액계정코드2
        /// </summary>
        public string GAMCODE2 { get; set; }

        /// <summary>
        /// 위내시경 검사일자
        /// </summary>
        public string ENDODATE_S { get; set; }

        /// <summary>
        /// 대장내시경 검사일자
        /// </summary>
        public string ENDODATE_C { get; set; }

        /// <summary>
        /// 최초접수시간
        /// </summary>
        public string CDATE { get; set; }

        /// <summary>
        /// 번호표발행시각
        /// </summary>
        public string WAITDATE { get; set; }


        /// <summary>
        /// <사용안함>:2014-03-18 마스타로 이관함
        /// </summary>
        public string VIPREMARK { get; set; }

        /// <summary>
        /// InBody 전송여부
        /// </summary>
        public string INBODY { get; set; }

        /// <summary>
        /// 수검구분(1.오전/2.오후)
        /// </summary>
        public string AMPM2 { get; set; }

        /// <summary>
        /// 특별할인금액
        /// </summary>
        public long GAMAMT { get; set; }

        /// <summary>
        /// 개인정보동의일자(사용안함:2014-02-13)
        /// </summary>
        public string GBPRIVACY { get; set; }

        /// <summary>
        /// 공단검진 대상자
        /// </summary>
        public string GONGDAN { get; set; }

        /// <summary>
        /// 종합 판정소견2
        /// </summary>
        public string PANREMARK2 { get; set; }

        /// <summary>
        /// 회사지원금
        /// </summary>
        public long LTDSAMT { get; set; }

        /// <summary>
        /// 942.건진항목감액 (직원소개,직원본인 등과 감액 계산을 위해)
        /// </summary>
        public long GONGDANAMT { get; set; }

        /// <summary>
        /// 종검초대권 번호
        /// </summary>
        public long TICKET { get; set; }

        /// <summary>
        /// 직원본인여부(Y.직원 N.배우자 등 가족)
        /// </summary>
        public string GBJIKWON { get; set; }

        /// <summary>
        /// 상담자 사번(2014-06-12 추가)
        /// </summary>
        public long SANGSABUN { get; set; }

        /// <summary>
        /// 등기우편물 중량(g)
        /// </summary>
        public long MAILWEIGHT { get; set; }

        /// <summary>
        /// PDF파일 생성요청(Y.요청)
        /// </summary>
        public string GBPDF { get; set; }

        /// <summary>
        /// PDF파일명
        /// </summary>
        public string PDFPATH { get; set; }

        /// <summary>
        /// 웹전송여부
        /// </summary>
        public string WEBSEND { get; set; }

        /// <summary>
        /// 웹전송일시
        /// </summary>
        public string WEBSENDDATE { get; set; }

        /// <summary>
        /// 예약시각(08:00 등등)
        /// </summary>
        public string STIME { get; set; }

        /// <summary>
        /// 낙상주의 여부(Y/N)
        /// </summary>
        public string GBNAKSANG { get; set; }

        /// <summary>
        /// 문진표종류(1.인터넷문진표 2.종이문진표)
        /// </summary>
        public string GBMUNJIN { get; set; }

        /// <summary>
        /// 인터넷문진번호(HIC_IE_MUNJIN_NEW의 접수번호)
        /// </summary>
        public long IEMUNNO { get; set; }

        /// <summary>
        /// 웹결과지인쇄 신청일시(NULL=미신청)
        /// </summary>
        public string WEBPRINTREQ { get; set; }

        /// <summary>
        /// 웹결과지인쇄 전송일시(NULL:미전송)
        /// </summary>
        public string WEBPRINTSEND { get; set; }

        /// <summary>
        /// 회사코드(종전코드)
        /// </summary>
        public string LTDCODE2 { get; set; }

        /// <summary>
        /// 옷장키번호(M01~70,F01~30)
        /// </summary>
        public string KEYNO { get; set; }

        /// <summary>
        /// 2017-09-01 통합이전 사용하던 등록번호
        /// </summary>
        public long PANO2 { get; set; }

        /// <summary>
        /// 주소①
        /// </summary>
        public string JUSO1 { get; set; }

        /// <summary>
        /// 주소②
        /// </summary>
        public string JUSO2 { get; set; }

        /// <summary>
        /// 주소
        /// </summary>
        public string JUSO { get; set; }

        /// <summary>
        /// 판정거절 등록시간
        /// </summary>
        public string SANGDAMNOT { get; set; }

        /// <summary>
        /// 결과지인쇄 사번
        /// </summary>
        public long PRTSABUN { get; set; }

        /// <summary>
        /// 결과지 출력 정보
        /// </summary>
        public string PRT_INFO { get; set; }

        /// <summary>
        /// 결과지 방문수령일자 등록사번
        /// </summary>
        public long RECVSABUN { get; set; }

        /// <summary>
        /// E-MAIL 주소
        /// </summary>
		public string EMAIL { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 회사명
        /// </summary>
        public string LTDNAME { get; set; }

        /// <summary>
        /// 성별/나이
        /// </summary>
        public string AGESEX { get; set; }

        /// <summary>
        /// 오전 접수인원 합계
        /// </summary>
        public long AMCNT { get; set; }

        /// <summary>
        /// 오후 접수인원 합계
        /// </summary>
        public long PMCNT { get; set; }

        /// <summary>
        /// 접수인원 합계
        /// </summary>
        public long WCNT { get; set; }

        /// <summary>
        /// 성별/나이
        /// </summary>
        public string RTIME { get; set; }

        /// <summary>
        /// 명품, VIP 검진유무
        /// </summary>
        public string VIPYN { get; set; }

        /// <summary>
        /// 조직수납 완료여부
        /// </summary>
		public string CLOSUNAP { get; set; }

        /// <summary>
        /// 대장내시경
        /// </summary>
		public string COLON { get; set; }

        /// <summary>
        /// 문진표 내역
        /// </summary>
		public string IEMUNJIN { get; set; }

        /// <summary>
        /// 휴대폰번호
        /// </summary>
		public string HPHONE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JEPBUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBCHK3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBJUSO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long ENTSABUN { get; set; }
        /// <summary>
        /// 종검상담내역
        /// </summary>
		public string SANGDAM { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EXCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string XRAYCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string XNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string XJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string XREMARK { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string XSUBCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string XORDERCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SENDBUSE1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SENDBUSE2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RESULT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PART { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RESULTTYPE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBCODEUSE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBUSE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RESCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ENTPART { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PANJENG { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MIN_M { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MIN_F { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MAX_M { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MAX_F { get; set; }

        public string MIN_M1 { get; set; }

        public string MAX_M1 { get; set; }

        public string MIN_M2 { get; set; }

        public string MAX_M2 { get; set; }

        public string MIN_F1 { get; set; }

        public string MAX_F1 { get; set; }

        public string MIN_F2 { get; set; }

        public string MAX_F2 { get; set; }

        public string MAX_M3 { get; set; }

        public string MAX_F3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ENDOGUBUN1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ENDOGUBUN2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ENDOGUBUN3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ENDOGUBUN4 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ENDOGUBUN5 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string YNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string HEAPART { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBAUTOSEND { get; set; }

        public string GROUPCODE { get; set; }

        public string EXSORT { get; set; }

        public string UNIT { get; set; }

        public string DMIN { get; set; }
        public string DMAX { get; set; }


        public string NAME { get; set; }
        public string JUSODETAIL { get; set; }
        public string SANGHO { get; set; }

        public string KIHO { get; set; }
        public string INWON { get; set; }
        public string TEL { get; set; }


        public COMHPC()
        {
        }
    }
}
