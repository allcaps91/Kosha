namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_RES_BOHUM2_JEPSU : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string IPSADATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JEPDATE { get; set; }

        /// <summary>
        /// 접수일련번호
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 검진년도
        /// </summary>
        public string GJYEAR { get; set; }

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
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// NAME (HIC_EXJONG 건강검진 명칭)
        /// </summary>
		public string NAME { get; set; }

        /// <summary>
        /// 그룹접수일련번호
        /// </summary>
		public long GWRTNO { get; set; }

        /// <summary>
        /// 검진 접수구분 (1:공단, 2:특수, 3:암, 4:종검, 5:기타)
        /// </summary>
		public string JEPBUN { get; set; }

        /// <summary>
        /// 상담의사명
        /// </summary>
		public string SANGDAMDRNAME { get; set; }

        /// <summary>
        /// 내시경구분
        /// </summary>
		public string ENDOGBN { get; set; }

        /// <summary>
        /// 사업장명
        /// </summary>
		public string LTDNAME { get; set; }

        /// <summary>
        /// 주민번호
        /// </summary>
		public string JUMINNO { get; set; }

        /// <summary>
        /// 개인정보동의일자
        /// </summary>
		public string PRIVACY_DATE { get; set; }

        /// <summary>
        /// 인터넷문진 여부
        /// </summary>
		public string GBIEMUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? MINDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? MAXDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? STARTDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? ENDDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long CNT { get; set; }

        /// <summary>
        /// 흉부질환 2차대상(Y/N)
        /// </summary>
        public string GBCHEST { get; set; }

        /// <summary>
        /// 순환기계질환 2차 대상(Y/N)
        /// </summary>
        public string GBCYCLE { get; set; }

        /// <summary>
        /// 고지혈증 2차대상(Y/N)
        /// </summary>
        public string GBGOJI { get; set; }

        /// <summary>
        /// 간장질환 2차대상(Y/N)
        /// </summary>
        public string GBLIVER { get; set; }

        /// <summary>
        /// 신장질환 2차대상(Y/N)
        /// </summary>
        public string GBKIDNEY { get; set; }

        /// <summary>
        /// 빈혈증 2차 대상(Y/N)
        /// </summary>
        public string GBANEMIA { get; set; }

        /// <summary>
        /// 당뇨질환 2차 대상(Y/N)
        /// </summary>
        public string GBDIABETES { get; set; }

        /// <summary>
        /// 기타질환 2차 대상(Y/N)
        /// </summary>
        public string GBETC { get; set; }

        /// <summary>
        /// 흉부방사선검사
        /// </summary>
        public string CHEST1 { get; set; }

        /// <summary>
        /// 결핵균집균도말검사(1.음성 2.양성)
        /// </summary>
        public string CHEST2 { get; set; }

        /// <summary>
        /// 폐결핵검사소견(1.정상 4.유질환)
        /// </summary>
        public string CHEST3 { get; set; }

        /// <summary>
        /// 흉부검사소견(1.정상A 2.정상B 3.건강주의 4.유질환)
        /// </summary>
        public string CHEST_RES { get; set; }

        /// <summary>
        /// 고혈압(혈압:최고)
        /// </summary>
        public string CYCLE1 { get; set; }

        /// <summary>
        /// 고혈압(혈압:최저)
        /// </summary>
        public string CYCLE2 { get; set; }

        /// <summary>
        /// 정밀안저검사(1.정상 2.안정도1~2 3.안전도3 4.안전도4)
        /// </summary>
        public string CYCLE3 { get; set; }

        /// <summary>
        /// 심전도검사(아래참조)
        /// </summary>
        public string CYCLE4 { get; set; }

        /// <summary>
        /// 고혈압검사소견(1.정상A 2.정상B 3.건강주의 4.유질환)
        /// </summary>
        public string CYCLE_RES { get; set; }

        /// <summary>
        /// 고지혈증(트리그리세라이드)
        /// </summary>
        public string GOJI1 { get; set; }

        /// <summary>
        /// 고지혈증(HDL콜레스테롤)
        /// </summary>
        public string GOJI2 { get; set; }

        /// <summary>
        /// 고지혈증소견(1.정상A 2.정상B 3.건강주의 4.유질환)
        /// </summary>
        public string GOJI_RES { get; set; }

        /// <summary>
        /// 간장질환(알부민)
        /// </summary>
        public string LIVER11 { get; set; }

        /// <summary>
        /// 간장질환(총단백정량)
        /// </summary>
        public string LIVER12 { get; set; }

        /// <summary>
        /// 간장질환(총빌리루빈)
        /// </summary>
        public string LIVER13 { get; set; }

        /// <summary>
        /// 간장질환(직접빌리루빈)
        /// </summary>
        public string LIVER14 { get; set; }

        /// <summary>
        /// 간장질환(알카리포스타파제)
        /// </summary>
        public string LIVER15 { get; set; }

        /// <summary>
        /// 간장질환(유산탈수효소(LDH)
        /// </summary>
        public string LIVER16 { get; set; }

        /// <summary>
        /// 간장질환(알파휘토단백) (아래참조)
        /// </summary>
        public string LIVER17 { get; set; }

        /// <summary>
        /// 간장질환(간염검사항원) :1.음성 2.양성
        /// </summary>
        public string LIVER18 { get; set; }

        /// <summary>
        /// 간장질환(간염검사항체):1.음성 2.양성
        /// </summary>
        public string LIVER19 { get; set; }

        /// <summary>
        /// 간염검사결과: 1.간염보균자 2.면역자 3.접종대상자
        /// </summary>
        public string LIVER20 { get; set; }

        /// <summary>
        /// 간장질환소견(1.정상A 2.정상B 3.건강주의 4.유질환)
        /// </summary>
        public string LIVER_RES { get; set; }

        /// <summary>
        /// 신장질환(요침사현미경검사(RBC))
        /// </summary>
        public string KIDNEY1 { get; set; }

        /// <summary>
        /// 신장질환(요침사현미경검사(WBC))
        /// </summary>
        public string KIDNEY2 { get; set; }

        /// <summary>
        /// 신장질환(요소질소)
        /// </summary>
        public string KIDNEY3 { get; set; }

        /// <summary>
        /// 신장질환(크레아티닌)
        /// </summary>
        public string KIDNEY4 { get; set; }

        /// <summary>
        /// 신장질환(요산)
        /// </summary>
        public string KIDNEY5 { get; set; }

        /// <summary>
        /// 신장질환소견(1.정상A 2.정상B 3.건강주의 4.유질환)
        /// </summary>
        public string KIDNEY_RES { get; set; }

        /// <summary>
        /// 빈혈증(헤파토크리트)
        /// </summary>
        public string ANEMIA1 { get; set; }

        /// <summary>
        /// 빈혈증(백혈구수)
        /// </summary>
        public string AMEMIA2 { get; set; }

        /// <summary>
        /// 빈혈증(적혈구수)
        /// </summary>
        public string AMEMIA3 { get; set; }

        /// <summary>
        /// 빈혈증소견(1.정상A 2.정상B 3.건강주의 4.유질환)
        /// </summary>
        public string AMEMIA_RES { get; set; }

        /// <summary>
        /// 당뇨질환(혈당:최고) 식후
        /// </summary>
        public string DIABETES1 { get; set; }

        /// <summary>
        /// 당뇨질환(혈당:최저) 식전
        /// </summary>
        public string DIABETES2 { get; set; }

        /// <summary>
        /// 당뇨질환(정밀안저검사) ** 아래참조 **
        /// </summary>
        public string DIABETES3 { get; set; }

        /// <summary>
        /// 당뇨질환소견(1.정상A 2.정상B 3.건강주의 4.유질환)
        /// </summary>
        public string DIABETES_RES { get; set; }

        /// <summary>
        /// 기타질환 검사소견
        /// </summary>
        public string ETC_RES { get; set; }

        /// <summary>
        /// 기타질환 검사내용
        /// </summary>
        public string ETC_EXAM { get; set; }

        /// <summary>
        /// 종합판정(1.정상A 2.정상B 3.건강주의 4.유질환)
        /// </summary>
        public string PANJENG { get; set; }

        /// <summary>
        /// 종합판정유질환(직업병D1):판정이 "5"인 경우(2002년검진용)
        /// </summary>
        public string PANJENG_D1 { get; set; }

        /// <summary>
        /// 질병유소견자 일반질병
        /// </summary>
        public string PANJENG_SO1 { get; set; }

        /// <summary>
        /// 질병유소견자 직업병
        /// </summary>
        public string PANJENG_SO2 { get; set; }

        /// <summary>
        /// 결과 통보방법(1.사업장 2.주소지 3.내원)
        /// </summary>
        public string TONGBOGBN { get; set; }

        /// <summary>
        /// 소견 및 조치사항
        /// </summary>
        public string SOGEN { get; set; }

        /// <summary>
        /// 검진일자
        /// </summary>
        public string GUNDATE { get; set; }

        /// <summary>
        /// 건강주의 사후관리소견
        /// </summary>
        public string PANJENG_SO3 { get; set; }

        /// <summary>
        /// 종합판정유질환(직업병D1):판정이 "5"인 경우(2003년검진용)
        /// </summary>
        public string PANJENG_D11 { get; set; }

        /// <summary>
        /// 종합판정유질환(직업병D1):판정이 "5"인 경우(2003년검진용)
        /// </summary>
        public string PANJENG_D12 { get; set; }

        /// <summary>
        /// 업무적합성여부(**) 공단코드:13 일반+특수만 사용함
        /// </summary>
        public string WORKYN { get; set; }

        /// <summary>
        /// 배양 및 약제감수성검사 (1:의뢰 2:미의뢰)
        /// </summary>
        public string CHEST4 { get; set; }

        /// <summary>
        /// 유질환 - 일반질병 D2 (K,J,A,)
        /// </summary>
        public string PANJENG_D2 { get; set; }

        /// <summary>
        /// 간장질환 - C형간염표면항체 검사-1일반, 2정밀
        /// </summary>
        public string LIVER21 { get; set; }

        /// <summary>
        /// 간장질환 - C형간염표면항체 결과-1.음성 2.양성
        /// </summary>
        public string LIVER22 { get; set; }

        /// <summary>
        /// 당뇨병-치료계획(아래참조)
        /// </summary>
        public string DIABETES_RES_CARE { get; set; }

        /// <summary>
        /// 고혈압-치료계획(아래참조)
        /// </summary>
        public string CYCLE_RES_CARE { get; set; }

        /// <summary>
        /// 인지기능판정(1:특이소견없음0-5점 ,2:인지기능저하~ 6점이상)
        /// </summary>
        public string T66_MEM { get; set; }

        /// <summary>
        /// 흡연-니코틴 의존도
        /// </summary>
        public string T_SMOKE1 { get; set; }

        /// <summary>
        /// 흡연-처방
        /// </summary>
        public string T_SMOKE2 { get; set; }

        /// <summary>
        /// 흡연-평가점수
        /// </summary>
        public long T_SMOKE3 { get; set; }

        /// <summary>
        /// 음주-평가
        /// </summary>
        public string T_DRINK1 { get; set; }

        /// <summary>
        /// 음주-처방
        /// </summary>
        public string T_DRINK2 { get; set; }

        /// <summary>
        /// 음주-평가점수
        /// </summary>
        public long T_DRINK3 { get; set; }

        /// <summary>
        /// 운동-평가
        /// </summary>
        public string T_HELTH1 { get; set; }

        /// <summary>
        /// 운동-처방(종류)
        /// </summary>
        public string T_HELTH2 { get; set; }

        /// <summary>
        /// 운동-처방(시간)
        /// </summary>
        public string T_HELTH3 { get; set; }

        /// <summary>
        /// 운동-처방(빈도,횟수)
        /// </summary>
        public string T_HELTH4 { get; set; }

        /// <summary>
        /// 운동-평가점수
        /// </summary>
        public long T_HELTH5 { get; set; }

        /// <summary>
        /// 영양-평가
        /// </summary>
        public string T_DIET1 { get; set; }

        /// <summary>
        /// 영양-권장음식(유제품)
        /// </summary>
        public string T_DIET2 { get; set; }

        /// <summary>
        /// 영양-권장음식(단백질류)
        /// </summary>
        public string T_DIET3 { get; set; }

        /// <summary>
        /// 영양-권장음식(야채와과일)
        /// </summary>
        public string T_DIET4 { get; set; }

        /// <summary>
        /// 영양-제한음식(지방)
        /// </summary>
        public string T_DIET5 { get; set; }

        /// <summary>
        /// 영양-제한음식(단순당)
        /// </summary>
        public string T_DIET6 { get; set; }

        /// <summary>
        /// 영양-제한음식(염분,소금)
        /// </summary>
        public string T_DIET7 { get; set; }

        /// <summary>
        /// 영양-올바른식사습관(아침식사)
        /// </summary>
        public string T_DIET8 { get; set; }

        /// <summary>
        /// 영양-올바른식사습관(골고루먹기)
        /// </summary>
        public string T_DIET9 { get; set; }

        /// <summary>
        /// 영양-처방연계(영양교육)
        /// </summary>
        public string T_DIET10 { get; set; }

        /// <summary>
        /// 영양-평가점수
        /// </summary>
        public long T_DIET11 { get; set; }

        /// <summary>
        /// 비만-체질량지수
        /// </summary>
        public string T_BIMAN1 { get; set; }

        /// <summary>
        /// 비만-허리둘레
        /// </summary>
        public string T_BIMAN2 { get; set; }

        /// <summary>
        /// 비만-처방(1.식사량을 줄이십시오)
        /// </summary>
        public string T_BIMAN3 { get; set; }

        /// <summary>
        /// 비만-처방(2.간식과 야식을 줄이십시오)
        /// </summary>
        public string T_BIMAN4 { get; set; }

        /// <summary>
        /// 비만-처방(3.음주량과 횟수를 줄이십시오)
        /// </summary>
        public string T_BIMAN5 { get; set; }

        /// <summary>
        /// 비만-처방(4.외식이나 패스트푸드를 줄이십시오)
        /// </summary>
        public string T_BIMAN6 { get; set; }

        /// <summary>
        /// 비만-처방(5.운동처방을 참고하십시오)
        /// </summary>
        public string T_BIMAN7 { get; set; }

        /// <summary>
        /// 비만-처방(6연계(비만클리닉))
        /// </summary>
        public string T_BIMAN8 { get; set; }

        /// <summary>
        /// 비만-처방(7.기타)
        /// </summary>
        public string T_BIMAN9 { get; set; }

        /// <summary>
        /// 우울증CES-D
        /// </summary>
        public string T_CESD { get; set; }

        /// <summary>
        /// 노인성 우울증 GDS단축판
        /// </summary>
        public string T_GDS { get; set; }

        /// <summary>
        /// 인지기능장애 KDSQ-C
        /// </summary>
        public string T_KDSQC { get; set; }

        /// <summary>
        /// 생애상담여부(Y or Null)
        /// </summary>
        public string T_SANGDAM { get; set; }

        /// <summary>
        /// 사후관리(R1-C)
        /// </summary>
        public string PANJENGSAHU1 { get; set; }

        /// <summary>
        /// 사후관리(D2)
        /// </summary>
        public string PANJENGSAHU2 { get; set; }

        /// <summary>
        /// 1차 건강진단 결과 요약(생애)
        /// </summary>
        public string T_SANGDAM_1 { get; set; }

        /// <summary>
        /// 종합소견(적극적인 관리):2015-01-08 추가
        /// </summary>
        public string SOGENB { get; set; }

        /// <summary>
        /// 토.공휴일 가산료 여부(Y/N)
        /// </summary>
        public string GBGONGHU { get; set; }

        public HIC_RES_BOHUM2_JEPSU()
        {
        }
    }
}
