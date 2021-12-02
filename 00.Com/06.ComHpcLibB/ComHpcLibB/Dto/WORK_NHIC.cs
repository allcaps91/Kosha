namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class WORK_NHIC : BaseDto
    {
        
        /// <summary>
        /// 요청일자 및 시각
        /// </summary>
		public DateTime? RTIME { get; set; } 

        /// <summary>
        /// 결과전달 일자 및 시각
        /// </summary>
		public DateTime? CTIME { get; set; } 

        /// <summary>
        /// 구분(B.보험자격 H.건강검진 C.건진가접수, A:암자격확인)
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 등록번호
        /// </summary>
		public string PANO { get; set; } 

        /// <summary>
        /// 환자성명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 주민등록번호
        /// </summary>
		public string JUMIN { get; set; } 

        /// <summary>
        /// 환자종류
        /// </summary>
		public string BI { get; set; } 

        /// <summary>
        /// 피보험자명
        /// </summary>
		public string PNAME { get; set; } 

        /// <summary>
        /// 조합기호
        /// </summary>
		public string KIHO { get; set; } 

        /// <summary>
        /// 증번호
        /// </summary>
		public string GKIHO { get; set; } 

        /// <summary>
        /// 자격취득일자
        /// </summary>
		public string BDATE { get; set; } 

        /// <summary>
        /// 관계
        /// </summary>
		public string REL { get; set; } 

        /// <summary>
        /// 지사명
        /// </summary>
		public string JISA { get; set; } 

        /// <summary>
        /// 검진 사업년도
        /// </summary>
		public string YEAR { get; set; } 

        /// <summary>
        /// 검진 자격변동여부
        /// </summary>
		public string TRANS { get; set; } 

        /// <summary>
        /// 검진 1차검진
        /// </summary>
		public string FIRST { get; set; } 

        /// <summary>
        /// 검진 심전도
        /// </summary>
		public string EKG { get; set; } 

        /// <summary>
        /// 검진 - B형간염
        /// </summary>
		public string LIVER { get; set; } 

        /// <summary>
        /// 위암   부담율
        /// </summary>
		public string CANCER11 { get; set; } 

        /// <summary>
        /// 위암   치료비지원
        /// </summary>
		public string CANCER12 { get; set; } 

        /// <summary>
        /// 유방암 부담율
        /// </summary>
		public string CANCER21 { get; set; } 

        /// <summary>
        /// 유방암 치료비지원
        /// </summary>
		public string CANCER22 { get; set; } 

        /// <summary>
        /// 대장암 부담율
        /// </summary>
		public string CANCER31 { get; set; } 

        /// <summary>
        /// 대장암 치료비지원
        /// </summary>
		public string CANCER32 { get; set; } 

        /// <summary>
        /// 간암(상반기) 부담율
        /// </summary>
		public string CANCER41 { get; set; } 

        /// <summary>
        /// 간암(상반기) 치료비지원
        /// </summary>
		public string CANCER42 { get; set; } 

        /// <summary>
        /// 자궁경부암 부담율
        /// </summary>
		public string CANCER51 { get; set; } 

        /// <summary>
        /// 자궁경부암 치료비지원
        /// </summary>
		public string CANCER52 { get; set; } 

        /// <summary>
        /// 오류여부(Y.정상 N.오류)
        /// </summary>
		public string GBERROR { get; set; } 

        /// <summary>
        /// 국가암조기검진대상 관할보건소
        /// </summary>
		public string CANCER53 { get; set; } 

        /// <summary>
        /// 자격 상실일
        /// </summary>
		public string DDATE { get; set; } 

        /// <summary>
        /// 중증등록번호
        /// </summary>
		public string JBUNHO { get; set; } 

        /// <summary>
        /// 적용시작일자
        /// </summary>
		public string FDATE { get; set; } 

        /// <summary>
        /// 적용종료일자
        /// </summary>
		public string TDATE { get; set; } 

        /// <summary>
        /// 암접수지사
        /// </summary>
		public string CANJISA { get; set; } 

        /// <summary>
        /// 암등록상병
        /// </summary>
		public string ILLCODE1 { get; set; } 

        /// <summary>
        /// 참고사항
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 건진종류
        /// </summary>
		public string GJONG { get; set; } 

        /// <summary>
        /// 1차-검진일자
        /// </summary>
		public string GBCHK01 { get; set; } 

        /// <summary>
        /// 1차-검진출장
        /// </summary>
		public string GBCHK01_CHUL { get; set; } 

        /// <summary>
        /// 1차-검진기관
        /// </summary>
		public string GBCHK01_NAME { get; set; } 

        /// <summary>
        /// 2차-검진일자
        /// </summary>
		public string GBCHK02 { get; set; } 

        /// <summary>
        /// 2차-검진출장
        /// </summary>
		public string GBCHK02_CHUL { get; set; } 

        /// <summary>
        /// 2차-검진기관
        /// </summary>
		public string GBCHK02_NAME { get; set; } 

        /// <summary>
        /// 구강-검진일자
        /// </summary>
		public string GBCHK03 { get; set; } 

        /// <summary>
        /// 구강-검진출장
        /// </summary>
		public string GBCHK03_CHUL { get; set; } 

        /// <summary>
        /// 구강-검진기관
        /// </summary>
		public string GBCHK03_NAME { get; set; } 

        /// <summary>
        /// 위암-검진일자
        /// </summary>
		public string GBCHK04 { get; set; } 

        /// <summary>
        /// 위암-검진출장
        /// </summary>
		public string GBCHK04_CHUL { get; set; } 

        /// <summary>
        /// 위암-검진기관
        /// </summary>
		public string GBCHK04_NAME { get; set; } 

        /// <summary>
        /// 대장암-검진일자
        /// </summary>
		public string GBCHK05 { get; set; } 

        /// <summary>
        /// 대장암-검진출장
        /// </summary>
		public string GBCHK05_CHUL { get; set; } 

        /// <summary>
        /// 대장암-검진기관
        /// </summary>
		public string GBCHK05_NAME { get; set; } 

        /// <summary>
        /// 유방암-검진일자
        /// </summary>
		public string GBCHK06 { get; set; } 

        /// <summary>
        /// 유방암-검진출장
        /// </summary>
		public string GBCHK06_CHUL { get; set; } 

        /// <summary>
        /// 유방암-검진기관
        /// </summary>
		public string GBCHK06_NAME { get; set; } 

        /// <summary>
        /// 자궁암-검진일자
        /// </summary>
		public string GBCHK07 { get; set; } 

        /// <summary>
        /// 자궁암-검진출장
        /// </summary>
		public string GBCHK07_CHUL { get; set; } 

        /// <summary>
        /// 자궁암-검진기관
        /// </summary>
		public string GBCHK07_NAME { get; set; } 

        /// <summary>
        /// 간암(상반기)-검진일자
        /// </summary>
		public string GBCHK08 { get; set; } 

        /// <summary>
        /// 간암(상반기)-검진출장
        /// </summary>
		public string GBCHK08_CHUL { get; set; } 

        /// <summary>
        /// 간암(상반기)-검진기관
        /// </summary>
		public string GBCHK08_NAME { get; set; } 

        /// <summary>
        /// 자격확인결과(1:완료, 2:에러)
        /// </summary>
		public string GBSTS { get; set; } 

        /// <summary>
        /// 2차간염대상자 여부(Y or Null/N)
        /// </summary>
		public string LIVER2 { get; set; } 

        /// <summary>
        /// 검진 2차검진
        /// </summary>
		public string SECOND { get; set; } 

        /// <summary>
        /// 주민등록번호 암호화
        /// </summary>
		public string JUMIN2 { get; set; } 

        /// <summary>
        /// 간암(하반기) 부담율
        /// </summary>
		public string CANCER61 { get; set; } 

        /// <summary>
        /// 간암(하반기) 치료비지원
        /// </summary>
		public string CANCER62 { get; set; } 

        /// <summary>
        /// 간암(하반기)-검진일자
        /// </summary>
		public string GBCHK09 { get; set; } 

        /// <summary>
        /// 간암(하반기)-검진출장
        /// </summary>
		public string GBCHK09_CHUL { get; set; } 

        /// <summary>
        /// 간암(하반기)-검진기관
        /// </summary>
		public string GBCHK09_NAME { get; set; } 

        /// <summary>
        /// 검진 - C형간염
        /// </summary>
		public string LIVERC { get; set; } 

        /// <summary>
        /// 검진 - 1차 추가검사
        /// </summary>
		public string FIRSTADD { get; set; } 

        /// <summary>
        /// 검진 - 2차 추가검사
        /// </summary>
		public string SECONDADD { get; set; } 

        /// <summary>
        /// 검진 - 구강검진
        /// </summary>
		public string DENTAL { get; set; } 

        /// <summary>
        /// 검진 - 이상지질혈증
        /// </summary>
		public string EXAMA { get; set; } 

        /// <summary>
        /// 검진 - 골밀도
        /// </summary>
		public string EXAMD { get; set; } 

        /// <summary>
        /// 검진 - 인지기능장애
        /// </summary>
		public string EXAME { get; set; } 

        /// <summary>
        /// 검진 - 정신건강검사
        /// </summary>
		public string EXAMF { get; set; } 

        /// <summary>
        /// 검진 - 생활습관평가
        /// </summary>
		public string EXAMG { get; set; } 

        /// <summary>
        /// 검진 - 노인신체기능
        /// </summary>
		public string EXAMH { get; set; } 

        /// <summary>
        /// 검진 - 치면세균막
        /// </summary>
		public string EXAMI { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string LUNG { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GBCHK10 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GBCHK10_CHUL { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GBCHK10_NAME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CANCER71 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CANCER72 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBCHK15 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBCHK15_CHUL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBCHK15_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBCHK16 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBCHK16_CHUL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBCHK16_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBCHK17 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBCHK17_CHUL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBCHK17_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 건강보험 자격확인 요청 및 결과 Table
        /// </summary>
        public WORK_NHIC()
        {
        }
    }
}
