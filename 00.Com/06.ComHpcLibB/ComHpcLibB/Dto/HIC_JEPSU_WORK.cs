namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_WORK : BaseDto
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
        public string JEPDATE { get; set; } 

        /// <summary>
        /// 주민번호
        /// </summary>
        public string JUMINNO { get; set; } 

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
        /// 사원번호
        /// </summary>
		public string SABUN { get; set; } 

        /// <summary>
        /// 최초입사일자
        /// </summary>
		public string IPSADATE { get; set; } 

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
		public string BUSEIPSA { get; set; } 

        /// <summary>
        /// 전직종 근무 시작일
        /// </summary>
		public string OLDSDATE { get; set; } 

        /// <summary>
        /// 전직종 근무 종료일
        /// </summary>
		public string OLDEDATE { get; set; } 

        /// <summary>
        /// 2차검진일(2차검진일이 없을 경우 NULL)
        /// </summary>
		public string SECOND_DATE { get; set; } 

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
        /// 접수취소일자
        /// </summary>
		public string DELDATE { get; set; } 

        /// <summary>
        /// 작업자 사번
        /// </summary>
		public long JOBSABUN { get; set; } 

        /// <summary>
        /// 최종 작업 시각
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 바코드발행여부(Y/N or NULL)
        /// </summary>
		public string GBEXAM { get; set; } 

        /// <summary>
        /// 건강보험 문진 상태(NULL:비대상 N:미입력 Y:입력완료)
        /// </summary>
		public string GBMUNJIN1 { get; set; } 

        /// <summary>
        /// 구강검사 문진 상태(NULL:비대상 N:미입력 Y:입력완료)
        /// </summary>
		public string GBMUNJIN2 { get; set; } 

        /// <summary>
        /// 구강검사 여부(Y/N): 구강검사(ZD00)
        /// </summary>
		public string GBDENTAL { get; set; } 

        /// <summary>
        /// 인원통계구분
        /// </summary>
		public string GBINWON { get; set; } 

        /// <summary>
        /// 보건소
        /// </summary>
		public string BOGUNSO { get; set; } 

        /// <summary>
        /// 감액합계
        /// </summary>
		public long GAMTTOT { get; set; } 

        /// <summary>
        /// 조합부담액
        /// </summary>
		public long GAMTJOHAP { get; set; } 

        /// <summary>
        /// 회사부담액
        /// </summary>
		public long GAMTLTD { get; set; } 

        /// <summary>
        /// 본인부담액
        /// </summary>
		public long GAMTBON { get; set; } 

        /// <summary>
        /// 입금액
        /// </summary>
		public long IPGUMAMT { get; set; } 

        /// <summary>
        /// 차액
        /// </summary>
		public long CHAAMT { get; set; } 

        /// <summary>
        /// 청구서번호
        /// </summary>
		public string TEL { get; set; } 

        /// <summary>
        /// 2차 간염
        /// </summary>
		public string LIVER2 { get; set; } 

        /// <summary>
        /// 영업소
        /// </summary>
		public string YOUNGUPSO { get; set; } 

        /// <summary>
        /// 마일리지암 여부
        /// </summary>
		public string MILEAGEAM { get; set; } 

        /// <summary>
        /// 무료암 여부
        /// </summary>
		public string MURYOAM { get; set; } 

        /// <summary>
        /// 검진대상자여부
        /// </summary>
		public string GUMDAESANG { get; set; } 

        /// <summary>
        /// 접수여부
        /// </summary>
		public string JEPSUGBN { get; set; } 

        /// <summary>
        /// 마일리지암 구분
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
        /// 금액
        /// </summary>
		public long MAMT { get; set; } 

        /// <summary>
        /// 금액
        /// </summary>
		public long FAMT { get; set; } 

        /// <summary>
        /// 직원추천사번
        /// </summary>
		public long GBSABUN { get; set; } 

        /// <summary>
        /// 참고사항
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 전자우편
        /// </summary>
		public string EMAIL { get; set; } 

        /// <summary>
        /// 방사선과촬영번호
        /// </summary>
		public string XRAYNO { get; set; }

        /// <summary>
        /// 암종류
        /// </summary>
        public string GBAM { get; set; }

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
        /// 핸드폰
        /// </summary>
		public string HPHONE { get; set; } 

        /// <summary>
        /// 베포용 문진표 출력여부 12001번
        /// </summary>
		public string GBMUNJIN12001 { get; set; } 

        /// <summary>
        /// 베포용 문진표 출력여부 12005번
        /// </summary>
		public string GBMUNJIN12005 { get; set; } 

        /// <summary>
        /// 베포용 문진표 출력여부 20001번
        /// </summary>
		public string GBMUNJIN20001 { get; set; } 

        /// <summary>
        /// 베포용 문진표 출력여부 20002번
        /// </summary>
		public string GBMUNJIN20002 { get; set; } 

        /// <summary>
        /// 베포용 문진표 출력여부 20003번
        /// </summary>
		public string GBMUNJIN20003 { get; set; } 

        /// <summary>
        /// 베포용 문진표 출력여부 00000번
        /// </summary>
		public string GBMUNJIN00000 { get; set; } 

        /// <summary>
        /// 베포용 문진표 출력여부 12006번
        /// </summary>
		public string GBMUNJIN12006 { get; set; } 

        /// <summary>
        /// 주민번호 암호화
        /// </summary>
		public string JUMINNO2 { get; set; } 

        /// <summary>
        /// 추가판정 여부
        /// </summary>
		public string GBADDPAN { get; set; } 

        /// <summary>
        /// 베포용 문진표 출력여부 30001번(야간작업)
        /// </summary>
		public string GBMUNJIN30001 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string BUILDNO { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE2 { get; set; } 

        /// <summary>
        /// 수동가접수, 일괄가접수구분
        /// </summary>
		public string GUBUN { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        public DateTime? YDATE { get; set; }

        public string LTDNAME { get; set; }

        public string NAME { get; set; }

        /// <summary>
        /// 일반건진 가접수 Table
        /// </summary>
        public HIC_JEPSU_WORK()
        {
        }
    }
}
