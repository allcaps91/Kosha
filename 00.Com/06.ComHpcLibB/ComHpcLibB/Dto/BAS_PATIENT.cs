namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class BAS_PATIENT : BaseDto
    {
        
        /// <summary>
        /// 병록번호,환자 ID
        /// </summary>
		public string PANO { get; set; } 

        /// <summary>
        /// 수진자명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 성별
        /// </summary>
		public string SEX { get; set; } 

        /// <summary>
        /// 주민번호1
        /// </summary>
		public string JUMIN1 { get; set; } 

        /// <summary>
        /// 주민번호2
        /// </summary>
		public string JUMIN2 { get; set; } 

        /// <summary>
        /// 최초내원일자
        /// </summary>
		public string STARTDATE { get; set; } 

        /// <summary>
        /// 최종내원일자
        /// </summary>
		public string LASTDATE { get; set; } 

        /// <summary>
        /// 우편번호1
        /// </summary>
		public string ZIPCODE1 { get; set; } 

        /// <summary>
        /// 우편번호2
        /// </summary>
		public string ZIPCODE2 { get; set; } 

        /// <summary>
        /// 주소
        /// </summary>
		public string JUSO { get; set; } 

        /// <summary>
        /// 지역코드
        /// </summary>
		public string JICODE { get; set; } 

        /// <summary>
        /// 전화번호
        /// </summary>
		public string TEL { get; set; } 

        /// <summary>
        /// 직원사번 or 계약처코드
        /// </summary>
		public string SABUN { get; set; } 

        /// <summary>
        /// 엠보싱 출력여부
        /// </summary>
		public string EMBPRT { get; set; } 

        /// <summary>
        /// 환자구분
        /// </summary>
		public string BI { get; set; } 

        /// <summary>
        /// 피보성명
        /// </summary>
		public string PNAME { get; set; } 

        /// <summary>
        /// 관계
        /// </summary>
		public string GWANGE { get; set; } 

        /// <summary>
        /// 기관기호
        /// </summary>
		public string KIHO { get; set; } 

        /// <summary>
        /// 개인기호(증번호)
        /// </summary>
		public string GKIHO { get; set; } 

        /// <summary>
        /// 최종진료과목
        /// </summary>
		public string DEPTCODE { get; set; } 

        /// <summary>
        /// 최종진료의사
        /// </summary>
		public string DRCODE { get; set; } 

        /// <summary>
        /// 특진여부
        /// </summary>
		public string GBSPC { get; set; } 

        /// <summary>
        /// 감액구분
        /// </summary>
		public string GBGAMEK { get; set; } 

        /// <summary>
        /// 180일 관련 진료일수
        /// </summary>
		public long JINILSU { get; set; } 

        /// <summary>
        /// 180일 관련 청구금액
        /// </summary>
		public long JINAMT { get; set; } 

        /// <summary>
        /// 180일 관련 최종투약과
        /// </summary>
		public string TUYAKGWA { get; set; } 

        /// <summary>
        /// 180일 관련 최종투약월
        /// </summary>
		public string TUYAKMONTH { get; set; } 

        /// <summary>
        /// 180일 관련 투약줄리앙일자
        /// </summary>
		public long TUYAKJULDATE { get; set; } 

        /// <summary>
        /// 180일 관련 투약일수
        /// </summary>
		public long TUYAKILSU { get; set; } 

        /// <summary>
        /// 보훈환자여부
        /// </summary>
		public string BOHUN { get; set; } 

        /// <summary>
        /// 비고
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 종교
        /// </summary>
		public string RELIGION { get; set; } 

        /// <summary>
        /// 메세지 Flag
        /// </summary>
		public string GBMSG { get; set; } 

        /// <summary>
        /// XRay 봉투(1.일반 2.Sono 3.CT 4.MRI 5.RI 6.기타)
        /// </summary>
		public string XRAYBARCODE { get; set; } 

        /// <summary>
        /// 전화예약후 불성실 여부(Y:불성실)
        /// </summary>
		public string ARSCHK { get; set; } 

        /// <summary>
        /// 의약분업 예외환자 구분(아래참조)
        /// </summary>
		public string BUNUP { get; set; } 

        /// <summary>
        /// 생년월일
        /// </summary>
		public string BIRTH { get; set; } 

        /// <summary>
        /// 양(+)/음(-)
        /// </summary>
		public string GBBIRTH { get; set; } 

        /// <summary>
        /// E-MAIL
        /// </summary>
		public string EMAIL { get; set; } 

        /// <summary>
        /// 환자 메세지 FLAG NEW
        /// </summary>
		public string GBINFOR { get; set; } 

        /// <summary>
        /// 직업코드(아래참조)
        /// </summary>
		public string JIKUP { get; set; } 

        /// <summary>
        /// 휴대폰번호
        /// </summary>
		public string HPHONE { get; set; } 

        /// <summary>
        /// 주거구분(1.자택 2.전세 3.월세 4.기타)
        /// </summary>
		public string GBJUGER { get; set; } 

        /// <summary>
        /// 정보동의여부(2012) SMS동의:Y  SMS동의안함:X   SMS요청하기:N
        /// </summary>
		public string GBSMS { get; set; } 

        /// <summary>
        /// 주소확인 확인완료:Y
        /// </summary>
		public string GBJUSO { get; set; } 

        /// <summary>
        /// 자격확일자(0901) 저장됨.
        /// </summary>
		public string BICHK { get; set; } 

        /// <summary>
        /// 핸드폰2
        /// </summary>
		public string HPHONE2 { get; set; } 

        /// <summary>
        /// 주사투약시 참조사항
        /// </summary>
		public string JUSAMSG { get; set; } 

        /// <summary>
        /// EKG 참자사항
        /// </summary>
		public string EKGMSG { get; set; } 

        /// <summary>
        /// 자격 취득일자
        /// </summary>
		public string BIDATE { get; set; } 

        /// <summary>
        /// 전화잘모등록
        /// </summary>
		public string MISSINGCALL { get; set; } 

        /// <summary>
        /// AI 플루 동의서(S:동의서대상, P:동의서 스캔완료)
        /// </summary>
		public string AIFLU { get; set; } 

        /// <summary>
        /// 핸드폰번호가 잘못되어 있는 경우 N 로 표시됨
        /// </summary>
		public string TEL_CONFIRM { get; set; } 

        /// <summary>
        /// 약 안부문자여부(*: 약 안부동의)
        /// </summary>
		public string GBSMS_DRUG { get; set; } 

        /// <summary>
        /// 환자Flag-등록일+등록자
        /// </summary>
		public string GBINFO_DETAIL { get; set; } 

        /// <summary>
        /// 환자 메시지 - 문제환자용(재원자메시지)
        /// </summary>
		public string GBINFOR2 { get; set; } 

        /// <summary>
        /// 도로주소구분 Y 20120704
        /// </summary>
		public string ROAD { get; set; } 

        /// <summary>
        /// 도로주소 도로명칭 20120704
        /// </summary>
		public string ROADDONG { get; set; } 

        /// <summary>
        /// 주민2(암호화)
        /// </summary>
		public string JUMIN3 { get; set; } 

        /// <summary>
        /// 외국인여부(Y:외국인)
        /// </summary>
		public string GBFOREIGNER { get; set; } 

        /// <summary>
        /// 외국인 여권 영문이름
        /// </summary>
		public string ENAME { get; set; } 

        /// <summary>
        /// 현금영수증 동의거부(Y:거부, N or Null )
        /// </summary>
		public string CASHYN { get; set; } 

        /// <summary>
        /// VIP고객구분 - BAS_VIP_구분코드 참조
        /// </summary>
		public string GB_VIP { get; set; } 

        /// <summary>
        /// VIP고객 참고사항
        /// </summary>
		public string GB_VIP_REMARK { get; set; } 

        /// <summary>
        /// VIP등록사번
        /// </summary>
		public long GB_VIP_SABUN { get; set; } 

        /// <summary>
        /// VIP수정일
        /// </summary>
		public DateTime? GB_VIP_DATE { get; set; } 

        /// <summary>
        /// 도로명 상세주소
        /// </summary>
		public string ROADDETAIL { get; set; } 

        /// <summary>
        /// 이전 vip
        /// </summary>
		public string GB_VIP2 { get; set; } 

        /// <summary>
        /// 이전 vip 참고
        /// </summary>
		public string GB_VIP2_REAMRK { get; set; } 

        /// <summary>
        /// 감사고객구분 Y
        /// </summary>
		public string GB_SVIP { get; set; } 

        /// <summary>
        /// WEBSEND
        /// </summary>
		public string WEBSEND { get; set; } 

        /// <summary>
        /// WEBSENDDATE
        /// </summary>
		public DateTime? WEBSENDDATE { get; set; } 

        /// <summary>
        /// 메르스 접촉환자
        /// </summary>
		public string GBMERS { get; set; } 

        /// <summary>
        /// 장애구분(0 or Null: 없음, 1: 시각장애, 2:청각장애, 3.언어장애)
        /// </summary>
		public string OBST { get; set; } 

        /// <summary>
        /// 신규우편번호 5자리
        /// </summary>
		public string ZIPCODE3 { get; set; } 

        /// <summary>
        /// 도로명주소 건물관리번호
        /// </summary>
		public string BUILDNO { get; set; } 

        /// <summary>
        /// 재활치료 메모
        /// </summary>
		public string PT_REMARK { get; set; } 

        /// <summary>
        /// 천주교 신자 본당
        /// </summary>
		public string TEMPLE { get; set; } 

        /// <summary>
        /// 천주교 세례명
        /// </summary>
		public string C_NAME { get; set; } 

        /// <summary>
        /// 국적코드 (BAS_외국인환자국적,KR:대한민국)
        /// </summary>
		public string GBCOUNTRY { get; set; } 

        /// <summary>
        /// GBGAMEKC
        /// </summary>
		public string GBGAMEKC { get; set; } 

        /// <summary>
        /// SNAME2
        /// </summary>
		public string SNAME2 { get; set; } 

        /// <summary>
        /// PNAME2
        /// </summary>
		public string PNAME2 { get; set; } 

        /// <summary>
        /// 예방접종-다음접종알림
        /// </summary>
		public string INJ_SMS1 { get; set; } 

        /// <summary>
        /// 예방접종-알러지관련
        /// </summary>
		public string INJ_SMS2 { get; set; }

        public string RID { get; set; }

        /// <summary>
        /// 환자 Master
        /// </summary>
        public BAS_PATIENT()
        {
        }
    }
}
