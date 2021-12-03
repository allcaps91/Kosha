namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class INSA_MST : BaseDto
    {
        
        /// <summary>
        /// 사원번호
        /// </summary>
		public string SABUN { get; set; } 

        /// <summary>
        /// 성명(한글)
        /// </summary>
		public string KORNAME { get; set; } 

        /// <summary>
        /// 성명(한문)
        /// </summary>
		public string CHINAME { get; set; } 

        /// <summary>
        /// 성명(영문)
        /// </summary>
		public string ENGNAME { get; set; } 

        /// <summary>
        /// 주민등록번호
        /// </summary>
		public string JUMIN { get; set; } 

        /// <summary>
        /// 성별(1:남  2:여 3:수녀님)
        /// </summary>
		public string SEX { get; set; } 

        /// <summary>
        /// 최초근무일자
        /// </summary>
		public DateTime? KUNDAY { get; set; } 

        /// <summary>
        /// 시용일자
        /// </summary>
		public DateTime? IPSADAY { get; set; } 

        /// <summary>
        /// 발령일자
        /// </summary>
		public DateTime? BALDAY { get; set; } 

        /// <summary>
        /// 부서
        /// </summary>
		public string BUSE { get; set; } 

        /// <summary>
        /// 직책
        /// </summary>
		public string JIK { get; set; } 

        /// <summary>
        /// 직종
        /// </summary>
		public string JIKJONG { get; set; } 

        /// <summary>
        /// 본적
        /// </summary>
		public string BONJUK { get; set; } 

        /// <summary>
        /// 주소
        /// </summary>
		public string JUSO { get; set; } 

        /// <summary>
        /// 우편번호(1)
        /// </summary>
		public string ZIPCODE1 { get; set; } 

        /// <summary>
        /// 우편번호(2)
        /// </summary>
		public string ZIPCODE2 { get; set; } 

        /// <summary>
        /// 전화번호
        /// </summary>
		public string TEL { get; set; } 

        /// <summary>
        /// 호주
        /// </summary>
		public string HOJU { get; set; } 

        /// <summary>
        /// 호주관계
        /// </summary>
		public string HOJU_KWAN { get; set; } 

        /// <summary>
        /// 최종학력(0:대학원,1:대학교,2:대중퇴,3:전문대,4:고등,5:중학교,6:초등)
        /// </summary>
		public string HAKCODE { get; set; } 

        /// <summary>
        /// 입사경위
        /// </summary>
		public string IPSAGU { get; set; } 

        /// <summary>
        /// 타사경력(년)
        /// </summary>
		public long TAKUNG_YY { get; set; } 

        /// <summary>
        /// 타사경력(월)
        /// </summary>
		public long TAKUNG_MM { get; set; } 

        /// <summary>
        /// 전직처명
        /// </summary>
		public string JUNJIK { get; set; } 

        /// <summary>
        /// 혼인여부(미혼:0/기혼:1/수녀님:2)
        /// </summary>
		public string MARRYGU { get; set; } 

        /// <summary>
        /// 결혼일
        /// </summary>
		public DateTime? MARRYDAY { get; set; } 

        /// <summary>
        /// 생일
        /// </summary>
		public DateTime? BIRTHDAY { get; set; } 

        /// <summary>
        /// 생일구분(0:양렬   1:음력)
        /// </summary>
		public string BIRTHGU { get; set; } 

        /// <summary>
        /// 세례명
        /// </summary>
		public string SERENAME { get; set; } 

        /// <summary>
        /// 소속본당
        /// </summary>
		public string BONDANG { get; set; } 

        /// <summary>
        /// 세례일자
        /// </summary>
		public DateTime? SEREDAY { get; set; } 

        /// <summary>
        /// 축일(월)
        /// </summary>
		public string CHUK_MM { get; set; } 

        /// <summary>
        /// 축일(일)
        /// </summary>
		public string CHUK_DD { get; set; } 

        /// <summary>
        /// 근속(년)
        /// </summary>
		public long GUNSOK_YY { get; set; } 

        /// <summary>
        /// 근속(월)
        /// </summary>
		public long GUNSOK_MM { get; set; } 

        /// <summary>
        /// 재직구분(0:재직  1:휴직  2:퇴직)
        /// </summary>
		public string JAEGU { get; set; } 

        /// <summary>
        /// 퇴직일자
        /// </summary>
		public DateTime? TOIDAY { get; set; } 

        /// <summary>
        /// 퇴직사유
        /// </summary>
		public string TOISAYU { get; set; } 

        /// <summary>
        /// 휴직기간(부터)
        /// </summary>
		public DateTime? HUDAY_FR { get; set; } 

        /// <summary>
        /// 휴직기간(까지)
        /// </summary>
		public DateTime? HUDAY_TO { get; set; } 

        /// <summary>
        /// 휴직사유
        /// </summary>
		public string HUSAYU { get; set; } 

        /// <summary>
        /// 학교코드(간호부용)
        /// </summary>
		public string HAK_CODE { get; set; } 

        /// <summary>
        /// 면허코드(간호부용)
        /// </summary>
		public string MYEN_CODE { get; set; } 

        /// <summary>
        /// 면허번호(간호부용)
        /// </summary>
		public string MYEN_BUNHO { get; set; } 

        /// <summary>
        /// 휴직기간2(부터)
        /// </summary>
		public DateTime? HUDAY_FR_2 { get; set; } 

        /// <summary>
        /// 휴직기간2(까지)
        /// </summary>
		public DateTime? HUDAY_TO_2 { get; set; } 

        /// <summary>
        /// 휴직사유2
        /// </summary>
		public string HUSAYU_2 { get; set; } 

        /// <summary>
        /// 정식직원, 임시직, 영일 구분(사용하지 않음)
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 개인평가 부여점수계
        /// </summary>
		public long MJEMSU1 { get; set; } 

        /// <summary>
        /// 개인평가 사용점수계
        /// </summary>
		public long MJEMSU2 { get; set; } 

        /// <summary>
        /// 개인평가 잔여점수계
        /// </summary>
		public long MJEMSU3 { get; set; } 

        /// <summary>
        /// 휴대폰번호
        /// </summary>
		public string HTEL { get; set; } 

        /// <summary>
        /// 인사변경시 Web서버에 전송(*.미전송 NULL.전송)
        /// </summary>
		public string WEBSEND { get; set; } 

        /// <summary>
        /// 2004년부터 사용
        /// </summary>
		public string HUGA { get; set; } 

        /// <summary>
        /// 개인 e-mail 주소
        /// </summary>
		public string EMAIL { get; set; } 

        /// <summary>
        /// 스케줄등록 관리자 1.관리 0.미관리
        /// </summary>
		public string SCHE { get; set; } 

        /// <summary>
        /// 자료 등록 권한
        /// </summary>
		public string UPPRIVATE { get; set; } 

        /// <summary>
        /// 실근무부서(조직도용)
        /// </summary>
		public string SIL_BUSE { get; set; } 

        /// <summary>
        /// 이원화 관리(이원화 관리할 경우 `1`, 아닐경우 `0` 또는 공백)
        /// </summary>
		public string DUALMANAGMENT { get; set; } 

        /// <summary>
        /// 이원화 관리적용 부서코드
        /// </summary>
		public string BUSEDUAL { get; set; } 

        /// <summary>
        /// 재직증명서에서 UPDATE된 주소
        /// </summary>
		public string JUSO2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TEL_CONFIRM { get; set; } 

        /// <summary>
        /// 국적(인사코드 구분6 참조)
        /// </summary>
		public string KUKJEK { get; set; } 

        /// <summary>
        /// 프리존번호 - SMS전송용 (예:010-7777-8888)
        /// </summary>
		public string MSTEL { get; set; } 

        /// <summary>
        /// 암호화된 주민번호
        /// </summary>
		public string JUMIN3 { get; set; } 

        /// <summary>
        /// 등록번호(BAS_PATIENT 연동)
        /// </summary>
		public string PANO { get; set; } 

        /// <summary>
        /// 개명전 성명
        /// </summary>
		public string RENAME_BEFORE { get; set; } 

        /// <summary>
        /// 개명후 성명
        /// </summary>
		public string RENAME_AFTER { get; set; } 

        /// <summary>
        /// 개명일자
        /// </summary>
		public DateTime? RENAME_DATE { get; set; } 

        /// <summary>
        /// 부서 이동여부
        /// </summary>
		public string BUSE_MOVE { get; set; } 

        /// <summary>
        /// 사번2=> 공백에 0이 들어가지 않는 사번
        /// </summary>
		public string SABUN2 { get; set; } 

        /// <summary>
        /// 사번3 => 숫자형 사번
        /// </summary>
		public long SABUN3 { get; set; } 

        /// <summary>
        /// 공인인증등록 패스(2015-01-30)
        /// </summary>
		public string CERTPASS { get; set; } 

        /// <summary>
        /// 확인대상-성범죄경력조회
        /// </summary>
		public string CHECK_GUBUN_1 { get; set; } 

        /// <summary>
        /// 확인대상-자격면허확인
        /// </summary>
		public string CHECK_GUBUN_2 { get; set; } 

        /// <summary>
        /// 확인대상-보수교육이수
        /// </summary>
		public string CHECK_GUBUN_3 { get; set; } 

        /// <summary>
        /// 확인대상-면허신고
        /// </summary>
		public string CHECK_GUBUN_4 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public DateTime? INPDT { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string INPS { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public DateTime? UPDT { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string UPPS { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CHECK_GUBUN_5 { get; set; } 

        
        /// <summary>
        /// 인사기본사항 TABEL <INSA_MST>
        /// </summary>
        public INSA_MST()
        {
        }
    }
}
