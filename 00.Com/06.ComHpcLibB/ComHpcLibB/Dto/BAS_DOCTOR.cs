namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class BAS_DOCTOR : BaseDto
    {
        
        /// <summary>
        /// 의사 Code
        /// </summary>
		public string DRCODE { get; set; } 

        /// <summary>
        /// 의사 서열 번호,각 과별 의사서열 순서
        /// </summary>
		public long PRINTRANKING { get; set; } 

        /// <summary>
        /// 의사명
        /// </summary>
		public string DRNAME { get; set; } 

        /// <summary>
        /// 진료과목 Code 1,BAS_CLINICDEPT(Dept Code)
        /// </summary>
		public string DRDEPT1 { get; set; } 

        /// <summary>
        /// 진료과목 Code 2
        /// </summary>
		public string DRDEPT2 { get; set; } 

        /// <summary>
        /// 최종일련변호(AM),외래접수시 의사별 접수순번 관리
        /// </summary>
		public long SEQAM { get; set; } 

        /// <summary>
        /// 최종일련변호(PM),외래마감시 "0"으로 Clear
        /// </summary>
		public long SEQPM { get; set; } 

        /// <summary>
        /// 출장 및 퇴직여부,"y" or "" or "Null" or "N"
        /// </summary>
		public string TOUR { get; set; } 

        /// <summary>
        /// 예약구분(0.않함 1.10분 2.15분 3.20분 4.30분단위)
        /// </summary>
		public string YTIMEGBN { get; set; } 

        /// <summary>
        /// 예약인원(예약단위별 최대인원수) => 오전예약인원
        /// </summary>
		public long YINWON { get; set; } 

        /// <summary>
        /// 진료분야
        /// </summary>
		public string BUNYA { get; set; } 

        /// <summary>
        /// 전화접수인원(예약단위별 최대인원수)
        /// </summary>
		public long TELCNT { get; set; } 

        /// <summary>
        /// 오전 예약 시작시간
        /// </summary>
		public string AMTIME { get; set; } 

        /// <summary>
        /// 오후 예약 시작시간
        /// </summary>
		public string PMTIME { get; set; } 

        /// <summary>
        /// 진료인원(예약단위별 최대인원수)
        /// </summary>
		public long TOTJIN { get; set; } 

        /// <summary>
        /// 의사 영문 성명
        /// </summary>
		public string DRENAME { get; set; } 

        /// <summary>
        /// 전화번호
        /// </summary>
		public string TELNO { get; set; } 

        /// <summary>
        /// 스케쥴표시여부(N:표시않함)
        /// </summary>
		public string SCHEDULE { get; set; } 

        /// <summary>
        /// 오후예약인원
        /// </summary>
		public long YINWON2 { get; set; } 

        /// <summary>
        /// 오전종료시간
        /// </summary>
		public string AMTIME2 { get; set; } 

        /// <summary>
        /// 오후종료시간
        /// </summary>
		public string PMTIME2 { get; set; } 

        /// <summary>
        /// 진구분
        /// </summary>
		public string JIN { get; set; } 

        /// <summary>
        /// 미비챠트 전송여부(`n`, Null or `y`)
        /// </summary>
		public string SMS_MIBI { get; set; } 

        /// <summary>
        /// EMR 대기순번 표시 순서
        /// </summary>
		public long EMRPRTS { get; set; } 

        /// <summary>
        /// 검사여부
        /// </summary>
		public string GBEXAM { get; set; } 

        /// <summary>
        /// EMR 대기순번 표시 순서 2
        /// </summary>
		public long EMRPRTS2 { get; set; } 

        /// <summary>
        /// 선택진료(Y)
        /// </summary>
		public string GBCHOICE { get; set; } 

        /// <summary>
        /// 의사구분(1.전문의, 2.전공의)
        /// </summary>
		public string GBGRADE { get; set; } 

        /// <summary>
        /// EMR 대기순번 표시 순서 3
        /// </summary>
		public long EMRPRTS3 { get; set; } 

        /// <summary>
        /// 오전 진료시간(평균설정)
        /// </summary>
		public double AMJIN { get; set; } 

        /// <summary>
        /// 오후 진료시간(평균설정)
        /// </summary>
		public double PMJIN { get; set; } 

        /// <summary>
        /// 오전 진료시작시간
        /// </summary>
		public string AMJINSTIME { get; set; } 

        /// <summary>
        /// 오후 진료시작시간
        /// </summary>
		public string PMJINSTIME { get; set; } 

        /// <summary>
        /// 내시경실 정렬순서
        /// </summary>
		public long ENDO_SEQ { get; set; } 

        /// <summary>
        /// 진료접수가능 진료의사  default 1
        /// </summary>
		public string GBJUPSU { get; set; } 

        
        /// <summary>
        /// 진료과장(전문의)의 고유번호 관리
        /// </summary>
        public BAS_DOCTOR()
        {
        }
    }
}
