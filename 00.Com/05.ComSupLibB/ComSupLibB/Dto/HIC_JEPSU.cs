namespace ComSupLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU : BaseDto
    {
        
        /// <summary>
        /// 접수일련번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 검진년도
        /// </summary>
		public string GJYEAR { get; set; } 

        /// <summary>
        /// 접수(건강진단)일자
        /// </summary>
		public DateTime? JEPDATE { get; set; } 

        /// <summary>
        /// 등록번호
        /// </summary>
		public long PANO { get; set; } 

        /// <summary>
        /// 검진자명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 성별(M.남자 F.여자)
        /// </summary>
		public string SEX { get; set; } 

        /// <summary>
        /// 나이(검진일자기준 만 나이)
        /// </summary>
		public long AGE { get; set; } 

        /// <summary>
        /// 건진종류 코드
        /// </summary>
		public string GJJONG { get; set; } 

        /// <summary>
        /// 검진차수(1.1차 2.2차 3.기타검진)
        /// </summary>
		public string GJCHASU { get; set; } 

        /// <summary>
        /// 검진분기(1.전반기 2.하반기 3.기타)
        /// </summary>
		public string GJBANGI { get; set; } 

        /// <summary>
        /// 출장검진여부(Y.출장검진 N.내원검진)
        /// </summary>
		public string GBCHUL { get; set; } 

        /// <summary>
        /// 작업상태(아래참조)
        /// </summary>
		public string GBSTS { get; set; } 

        /// <summary>
        /// 인원통계구분
        /// </summary>
		public string GBINWON { get; set; } 

        /// <summary>
        /// 접수취소일자
        /// </summary>
		public DateTime? DELDATE { get; set; } 

        /// <summary>
        /// 회사코드
        /// </summary>
		public long LTDCODE { get; set; } 

        /// <summary>
        /// 우편번호
        /// </summary>
		public string MAILCODE { get; set; } 

        /// <summary>
        /// 주소1
        /// </summary>
		public string JUSO1 { get; set; } 

        /// <summary>
        /// 주소2
        /// </summary>
		public string JUSO2 { get; set; } 

        /// <summary>
        /// 전화번호
        /// </summary>
		public string TEL { get; set; } 

        /// <summary>
        /// 병원 등록번호
        /// </summary>
		public string PTNO { get; set; } 

        /// <summary>
        /// 부담율 코드(아래참조)
        /// </summary>
		public string BURATE { get; set; } 

        /// <summary>
        /// 건강보험 지사코드
        /// </summary>
		public string JISA { get; set; } 

        /// <summary>
        /// 사업장기호
        /// </summary>
		public string KIHO { get; set; } 

        /// <summary>
        /// 증번호
        /// </summary>
		public string GKIHO { get; set; } 

        /// <summary>
        /// 직종구분(1.생산직 2.특수직 3.사무직)
        /// </summary>
		public string JIKGBN { get; set; } 

        /// <summary>
        /// 유해인자(여러개가 입력 가능:컴마로 분리)
        /// </summary>
		public string UCODES { get; set; } 

        /// <summary>
        /// 선택검사 코드
        /// </summary>
		public string SEXAMS { get; set; } 

        /// <summary>
        /// 직종코드
        /// </summary>
		public string JIKJONG { get; set; } 

        /// <summary>
        /// 사원번호(사업장)
        /// </summary>
		public string SABUN { get; set; } 

        /// <summary>
        /// 최초입사일자
        /// </summary>
		public DateTime? IPSADATE { get; set; } 

        /// <summary>
        /// 현작업부서명
        /// </summary>
		public string BUSENAME { get; set; } 

        /// <summary>
        /// 전직종코드
        /// </summary>
		public string OLDJIKJONG { get; set; } 

        /// <summary>
        /// 현직전입일자
        /// </summary>
		public DateTime? BUSEIPSA { get; set; } 

        /// <summary>
        /// 전직종 근무 시작일
        /// </summary>
		public DateTime? OLDSDATE { get; set; } 

        /// <summary>
        /// 전직종 근무 종료일
        /// </summary>
		public DateTime? OLDEDATE { get; set; } 

        /// <summary>
        /// 수첩소지여부(Y/N)
        /// </summary>
		public string GBSUCHEP { get; set; } 

        /// <summary>
        /// 발급년도
        /// </summary>
		public string BALYEAR { get; set; } 

        /// <summary>
        /// 발급년도별 일련번호
        /// </summary>
		public long BALSEQ { get; set; } 

        /// <summary>
        /// 바코드발행여부(Y/N or NULL)
        /// </summary>
		public string GBEXAM { get; set; } 

        /// <summary>
        /// 보건소
        /// </summary>
		public string BOGUNSO { get; set; } 

        /// <summary>
        /// 접수구분(1.정상접수 2.가접수)
        /// </summary>
		public string JEPSUGBN { get; set; } 

        /// <summary>
        /// 영업소코드
        /// </summary>
		public string YOUNGUPSO { get; set; } 

        /// <summary>
        /// 청구서번호
        /// </summary>
		public string JEPNO { get; set; } 

        /// <summary>
        /// 특수검진 문진입력 여부(Y:입력 N.미입력)
        /// </summary>
		public string GBSPCMUNJIN { get; set; } 

        /// <summary>
        /// 결과지 인쇄여부(사용않함).. 이양재
        /// </summary>
		public string GBPRINT { get; set; } 

        /// <summary>
        /// 직원추천사번
        /// </summary>
		public long GBSABUN { get; set; } 

        /// <summary>
        /// 일반진찰상담여부(Y:진찰 NULL:미진찰)
        /// </summary>
		public string GBJINCHAL { get; set; } 

        /// <summary>
        /// 건강보험 문진 상태(NULL:비대상 N:미입력 Y:입력완료)
        /// </summary>
		public string GBMUNJIN1 { get; set; } 

        /// <summary>
        /// 구강검사 문진 상태(NULL:비대상 N:미입력 Y:입력완료)
        /// </summary>
		public string GBMUNJIN2 { get; set; } 

        /// <summary>
        /// 특수검진 문진 상태(NULL:비대상 N:미입력 Y:입력완료)
        /// </summary>
		public string GBMUNJIN3 { get; set; } 

        /// <summary>
        /// 구강검사 여부(Y/N): 구강검사(ZD00)
        /// </summary>
		public string GBDENTAL { get; set; } 

        /// <summary>
        /// 2차검진대상 구분 (NULL:미정 N:비대상 Y:대상)
        /// </summary>
		public string SECOND_FLAG { get; set; } 

        /// <summary>
        /// 2차검진일(2차검진일이 없을 경우 NULL)
        /// </summary>
		public DateTime? SECOND_DATE { get; set; } 

        /// <summary>
        /// 2차검진대상 연락일자
        /// </summary>
		public DateTime? SECOND_TONGBO { get; set; } 

        /// <summary>
        /// 2차검진 검사항목(ex,1,2,L,...)
        /// </summary>
		public string SECOND_EXAMS { get; set; } 

        /// <summary>
        /// 2차검진 사유(고혈압의심,직촬재촬,...)
        /// </summary>
		public string SECOND_SAYU { get; set; } 

        /// <summary>
        /// 청구구분(Y.청구, " " 미청구)
        /// </summary>
		public string CHUNGGUYN { get; set; } 

        /// <summary>
        /// 구강검사청구(Y.청구, " " 미청구)
        /// </summary>
		public string DENTCHUNGGUYN { get; set; } 

        /// <summary>
        /// 암검사청구(Y.청구, " " 미청구)
        /// </summary>
		public string CANCERCHUNGGUYN { get; set; } 

        /// <summary>
        /// 2차 간염
        /// </summary>
		public string LIVER2 { get; set; } 

        /// <summary>
        /// 금액
        /// </summary>
		public long MAMT { get; set; } 

        /// <summary>
        /// 금액
        /// </summary>
		public long FAMT { get; set; } 

        /// <summary>
        /// 마일리지암 구분
        /// </summary>
		public string MILEAGEAM { get; set; } 

        /// <summary>
        /// 무료암대상 여부
        /// </summary>
		public string MURYOAM { get; set; } 

        /// <summary>
        /// 검진대상    예) 1.직장가입자 2.지역,피부양자
        /// </summary>
		public string GUMDAESANG { get; set; } 

        /// <summary>
        /// 종검접수여부
        /// </summary>
		public string JONGGUMYN { get; set; } 

        /// <summary>
        /// 자료전송 여부
        /// </summary>
		public string SEND { get; set; } 

        /// <summary>
        /// 마일리지암구분
        /// </summary>
		public string MILEAGEAMGBN { get; set; } 

        /// <summary>
        /// 무료암구분
        /// </summary>
		public string MURYOGBN { get; set; } 

        /// <summary>
        /// 회사청구 유무
        /// </summary>
		public DateTime? NEGODATE { get; set; } 

        /// <summary>
        /// 작업자 사번
        /// </summary>
		public long JOBSABUN { get; set; } 

        /// <summary>
        /// 최종 작업 시각
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 2차검진 미실시 사유
        /// </summary>
		public string SECOND_MISAYU { get; set; } 

        /// <summary>
        /// 건강보험1,2차 청구번호
        /// </summary>
		public long MIRNO1 { get; set; } 

        /// <summary>
        /// 구강검진 청구번호
        /// </summary>
		public long MIRNO2 { get; set; } 

        /// <summary>
        /// 암검진   청구번호
        /// </summary>
		public long MIRNO3 { get; set; } 

        /// <summary>
        /// E-mail(문진표 작성시 동의한 사람만 입력)
        /// </summary>
		public string EMAIL { get; set; } 

        /// <summary>
        /// 참고사항
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 안전공단 특수검진 전송 일련번호
        /// </summary>
		public long OHMSNO { get; set; } 

        /// <summary>
        /// 회사청구 미수번호
        /// </summary>
		public long MISUNO1 { get; set; } 

        /// <summary>
        /// 2차검사인 경우 1차판정 의사 면허번호
        /// </summary>
		public long FIRSTPANDRNO { get; set; } 

        /// <summary>
        /// 급한 환자 표시
        /// </summary>
		public string ERFLAG { get; set; } 

        /// <summary>
        /// 암검진 보건소청구번호
        /// </summary>
		public long MIRNO4 { get; set; } 

        /// <summary>
        /// 청구 제외 사유
        /// </summary>
		public string MIRSAYU { get; set; } 

        /// <summary>
        /// 급한 환자 통보일자
        /// </summary>
		public DateTime? ERTONGBO { get; set; } 

        /// <summary>
        /// 공단청구 미수번호
        /// </summary>
		public long MISUNO2 { get; set; } 

        /// <summary>
        /// 의료급여암 대상 : 2005년
        /// </summary>
		public string GUBDAESANG { get; set; } 

        /// <summary>
        /// 암검진 의료급여암청구번호
        /// </summary>
		public long MIRNO5 { get; set; } 

        /// <summary>
        /// 공단청구 구강미수번호
        /// </summary>
		public long MISUNO3 { get; set; } 

        /// <summary>
        /// 방사선촬영번호()2005.07.13추가,2009년부터 직촬외래번호통합됨
        /// </summary>
		public string XRAYNO { get; set; } 

        /// <summary>
        /// 신용카드 승인일련번호(1~9999999999)
        /// </summary>
		public long CARDSEQNO { get; set; } 

        /// <summary>
        /// 69종 추가판정여부
        /// </summary>
		public string GBADDPAN { get; set; } 

        /// <summary>
        /// 암검진구분 예)1,1,0,1,1,0 (위UGI,위GFS,대장암,간암,유방,자궁)
        /// </summary>
		public string GBAM { get; set; } 

        /// <summary>
        /// 결과발송일자
        /// </summary>
		public DateTime? BALDATE { get; set; } 

        /// <summary>
        /// 최초통보일자-결과지출력일자
        /// </summary>
		public DateTime? TONGBODATE { get; set; } 

        /// <summary>
        /// 수진등록여부 (Y.자동등록)
        /// </summary>
		public string GBSUJIN_SET { get; set; } 

        /// <summary>
        /// 가내원여부(Y:출장을 가내원처리 N or Null: 해당사항없음)
        /// </summary>
		public string GBCHUL2 { get; set; } 

        /// <summary>
        /// 구강진찰상담여부(Y:진찰 NULL:미진찰)
        /// </summary>
		public string GBJINCHAL2 { get; set; } 

        /// <summary>
        /// 상담의사 사번
        /// </summary>
		public long SANGDAMDRNO { get; set; } 

        /// <summary>
        /// 상담일자
        /// </summary>
		public DateTime? SANGDAMDATE { get; set; } 

        /// <summary>
        /// 원무과 전달 메세지
        /// </summary>
		public string SENDMSG { get; set; } 

        /// <summary>
        /// 구분(1:초등,2:중,고등)
        /// </summary>
		public string GBN { get; set; } 

        /// <summary>
        /// 학년
        /// </summary>
		public long CLASS { get; set; } 

        /// <summary>
        /// 학반
        /// </summary>
		public long BAN { get; set; } 

        /// <summary>
        /// 학번
        /// </summary>
		public long BUN { get; set; } 

        /// <summary>
        /// HEMS Data 생성번호
        /// </summary>
		public long HEMSNO { get; set; } 

        /// <summary>
        /// HEMS DATA 제외 사유
        /// </summary>
		public string HEMSMIRSAYU { get; set; } 

        /// <summary>
        /// 자동접수 구분 (Y/N or Null)
        /// </summary>
		public string AUTOJEP { get; set; } 

        /// <summary>
        /// 보건소청구 미수번호
        /// </summary>
		public long MISUNO4 { get; set; } 

        /// <summary>
        /// 포스코 청구서 고유번호
        /// </summary>
		public long P_WRTNO { get; set; } 

        /// <summary>
        /// 판정일자(2014-08-02)
        /// </summary>
		public DateTime? PANJENGDATE { get; set; } 

        /// <summary>
        /// 판정의사 면허번호(2014-08-02)
        /// </summary>
		public long PANJENGDRNO { get; set; } 

        /// <summary>
        /// 2차 정밀청력 검사 대상자(Y/N)
        /// </summary>
		public string GBAUDIO2EXAM { get; set; } 

        /// <summary>
        /// 문진표 인쇄구분(1.인터넷 2.작성제출 3.인쇄 NULL:미인쇄)
        /// </summary>
		public string GBMUNJINPRINT { get; set; } 

        /// <summary>
        /// 대기순번관련 전달사항
        /// </summary>
		public string WAITREMARK { get; set; } 

        /// <summary>
        /// 휴대폰 결과 조회용 PDF 파일명
        /// </summary>
		public string PDFPATH { get; set; } 

        /// <summary>
        /// 웹전송여부
        /// </summary>
		public string WEBSEND { get; set; } 

        /// <summary>
        /// 웹전송일시
        /// </summary>
		public DateTime? WEBSENDDATE { get; set; } 

        /// <summary>
        /// 낙상주의 여부(Y/N)
        /// </summary>
		public string GBNAKSANG { get; set; } 

        /// <summary>
        /// 1차없이 구강검진만 실시여부(Y/N)
        /// </summary>
		public string GBDENTONLY { get; set; } 

        /// <summary>
        /// 자동판정구분(Y:정상A자동판정,N:대상아님,NULL:미점검)
        /// </summary>
		public string GBAUTOPAN { get; set; } 

        /// <summary>
        /// 인터넷문진번호
        /// </summary>
		public long IEMUNNO { get; set; } 

        /// <summary>
        /// 웹결과지인쇄 신청일시(NULL:미신청)
        /// </summary>
		public DateTime? WEBPRINTREQ { get; set; } 

        /// <summary>
        /// 공단 수진등록 확인 여부(Y/N)
        /// </summary>
		public string GBSUJIN_CHK { get; set; } 

        /// <summary>
        /// 웹결과지인쇄 전송일시(NULL:미전송)
        /// </summary>
		public DateTime? WEBPRINTSEND { get; set; } 

        /// <summary>
        /// LTDCODE2
        /// </summary>
		public string LTDCODE2 { get; set; } 

        /// <summary>
        /// 구강공단청구금액
        /// </summary>
		public long DENTAMT { get; set; } 

        /// <summary>
        /// 휴일가산여부(Y/N)
        /// </summary>
		public string GBHUADD { get; set; } 

        /// <summary>
        /// 액팅화면 참고사항
        /// </summary>
		public string EXAMREMARK { get; set; } 

        /// <summary>
        /// 결과지인쇄 사번
        /// </summary>
		public long PRTSABUN { get; set; } 

        /// <summary>
        /// 종검내시경실 여부(Y/N)
        /// </summary>
		public string GBHEAENDO { get; set; } 

        /// <summary>
        /// 자격정보1(01:사업장 02:공무원 03:성인병, 04:의료급여)
        /// </summary>
		public string GBCHK1 { get; set; } 

        /// <summary>
        /// 자격정보2(01:생애의료급여)
        /// </summary>
		public string GBCHK2 { get; set; } 

        /// <summary>
        /// 임시
        /// </summary>
		public string GBCHK3 { get; set; } 

        /// <summary>
        /// 생활습관접수
        /// </summary>
		public string GBLIFE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GBJUSO { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long GWRTNO { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string JEPBUN { get; set; } 

        
        /// <summary>
        /// 건강증진센타 접수 마스타
        /// </summary>
        public HIC_JEPSU()
        {
        }
    }
}
