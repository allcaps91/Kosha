namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_PATIENT : BaseDto
    {
        
        /// <summary>
        /// 등록번호
        /// </summary>
		public long PANO { get; set; } 

        /// <summary>
        /// 성명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 주민등록번호
        /// </summary>
		public string JUMIN { get; set; } 

        /// <summary>
        /// 성별(M/F)
        /// </summary>
		public string SEX { get; set; }

        /// <summary>
        /// 성별(M/F) / 나이
        /// </summary>
        public string S_AGE { get; set; }

        /// <summary>
        /// 우편번호
        /// </summary>
        public string MAILCODE { get; set; } 

        /// <summary>
        /// 주소①
        /// </summary>
		public string JUSO1 { get; set; } 

        /// <summary>
        /// 주소②
        /// </summary>
		public string JUSO2 { get; set; } 

        /// <summary>
        /// 전화번호
        /// </summary>
		public string TEL { get; set; } 

        /// <summary>
        /// 회사코드
        /// </summary>
		public long LTDCODE { get; set; }

        /// <summary>
        /// 회사명
        /// </summary>
        public string LTDNAME { get; set; }

        /// <summary>
        /// 직종구분(1.생산직 2.특수직 3.사무직)
        /// </summary>
        public string JIKGBN { get; set; } 

        /// <summary>
        /// 직종(공단코드)
        /// </summary>
		public string JIKJONG { get; set; } 

        /// <summary>
        /// 사원번호
        /// </summary>
		public string SABUN { get; set; } 

        /// <summary>
        /// 부서명칭
        /// </summary>
		public string BUSENAME { get; set; } 

        /// <summary>
        /// 공정코드(공단코드)
        /// </summary>
		public string GONGJENG { get; set; } 

        /// <summary>
        /// 회사 입사일자
        /// </summary>
		public string IPSADATE { get; set; } 

        /// <summary>
        /// 현직 전입일자
        /// </summary>
		public string BUSEIPSA { get; set; } 

        /// <summary>
        /// 건강보험 지사코드
        /// </summary>
		public string JISA { get; set; } 

        /// <summary>
        /// 증번호
        /// </summary>
		public string GKIHO { get; set; } 

        /// <summary>
        /// 수첩소지여부(Y/N)
        /// </summary>
		public string GBSUCHEP { get; set; } 

        /// <summary>
        /// 원무행정의 환자 등록번호
        /// </summary>
		public string PTNO { get; set; } 

        /// <summary>
        /// 참고사항
        /// </summary>
		public string REMARK { get; set; }

        /// <summary>
        /// 참고사항
        /// </summary>
        public string PAT_REMARK { get; set; }

        /// <summary>
        /// 사업장기호
        /// </summary>
        public string KIHO { get; set; } 

        /// <summary>
        /// 보건소
        /// </summary>
		public string BOGUNSO { get; set; } 

        /// <summary>
        /// 영업소
        /// </summary>
		public string YOUNGUPSO { get; set; } 

        /// <summary>
        /// 2차간염대상자
        /// </summary>
		public string LIVER2 { get; set; } 

        /// <summary>
        /// 검진대상    예) 1.직장가입자 2.지역,피부양자
        /// </summary>
		public string GUMDAESANG { get; set; } 

        /// <summary>
        /// E-mail(문진표 작성시 동의한 사람만 입력)
        /// </summary>
		public string EMAIL { get; set; } 

        /// <summary>
        /// 핸드폰번호
        /// </summary>
		public string HPHONE { get; set; } 

        /// <summary>
        /// 인터넷문진여부(Y:했음 / N or Null:해당없음)
        /// </summary>
		public string GBIEMUNJIN { get; set; } 

        /// <summary>
        /// 문자수신동의여부(Y:동의함 / N or Null:동의안함)
        /// </summary>
		public string GBSMS { get; set; } 

        /// <summary>
        /// 핸드폰번호가 잘못되어 있는 경우 N 로 표시됨
        /// </summary>
		public string TEL_CONFIRM { get; set; } 

        /// <summary>
        /// 주민번호 암호화
        /// </summary>
		public string JUMIN2 { get; set; } 

        /// <summary>
        /// 개인정보동의서(동의일자)
        /// </summary>
		public string GBPRIVACY { get; set; } 

        /// <summary>
        /// 우편번호용 건물번호
        /// </summary>
		public string BUILDNO { get; set; } 

        /// <summary>
        /// <사용안함>
        /// </summary>
		public string LTDCODE2 { get; set; } 

        /// <summary>
        /// 생일
        /// </summary>
		public string BIRTHDAY { get; set; } 

        /// <summary>
        /// 생일구분(1.양력 2.음력)
        /// </summary>
		public string GBBIRTH { get; set; } 

        /// <summary>
        /// 회사 전화번호
        /// </summary>
		public string LTDTEL { get; set; } 

        /// <summary>
        /// 감액계정(최종 사용된것)
        /// </summary>
		public string GAMCODE { get; set; } 

        /// <summary>
        /// 종교구분코드(아래참조)
        /// </summary>
		public string RELIGION { get; set; } 

        /// <summary>
        /// 최초검진일자
        /// </summary>
		public string STARTDATE { get; set; } 

        /// <summary>
        /// 최종검진일자
        /// </summary>
		public string LASTDATE { get; set; } 

        /// <summary>
        /// 검진횟수
        /// </summary>
		public long JINCOUNT { get; set; } 

        /// <summary>
        /// 가족성명
        /// </summary>
		public string FAMILLY { get; set; } 

        /// <summary>
        /// 감액계정2 (최종 사용된것)
        /// </summary>
		public string GAMCODE2 { get; set; } 

        /// <summary>
        /// 시청/교육청 검진확인서용 소속명칭(00초등학교,남구청)
        /// </summary>
		public string SOSOK { get; set; } 

        /// <summary>
        /// VIP관련 참고사항
        /// </summary>
		public string VIPREMARK { get; set; } 

        /// <summary>
        /// 직원본인여부(Y.직원 N.배우자 등 가족)
        /// </summary>
		public string GBJIKWON { get; set; }

        public string GBFOREIGNER { get; set; }
        public string ENAME { get; set; }
        public string SNAME2 { get; set; }
        public string FOREIGNERNUM { get; set; }
        public string GBPRIVACY_NEW { get; set; }
        /// <summary>
        /// 컨트롤에서 받아오는 주민번호 앞 6자리
        /// </summary>
		public string CT_JUMIN1 { get; set; }

        /// <summary>
        /// 컨트롤에서 받아오는 주민번호 뒤 7자리
        /// </summary>
		public string CT_JUMIN2 { get; set; }

        /// <summary>
        /// 매개변수로 값을 넘길때 나이 (신환환자 생성용)
        /// </summary>
        public int P_AGE { get; set; }

        public decimal CNT { get; set; }

        public long WRTNO { get; set; }

        /// <summary>
        /// 건강증진센타 환자마스타
        /// </summary>
        public HIC_PATIENT()
        {
        }
    }
}
