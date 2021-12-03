namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_RESV_EXAM : BaseDto
    {
        
        /// <summary>
        /// 예약일자 및 시간
        /// </summary>
		public string RTIME { get; set; }

        /// <summary>
        /// 처방일자
        /// </summary>
		public string BDATE { get; set; }

        /// <summary>
        /// 예약시간
        /// </summary>
        public string STIME { get; set; }

        /// <summary>
        /// 종검번호
        /// </summary>
        public long PANO { get; set; } 

        /// <summary>
        /// 수검자명
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 등록번호
        /// </summary>
        public string PTNO { get; set; }

        /// <summary>
        /// 선택검사 구분 (HEA_CODE 13번 참조)
        /// </summary>
        public string GBEXAM { get; set; } 

        /// <summary>
        /// 선택검사명
        /// </summary>
		public string EXAMNAME { get; set; } 

        /// <summary>
        /// 통보일자
        /// </summary>
		public DateTime? TONGBODATE { get; set; } 

        /// <summary>
        /// 통보사번
        /// </summary>
		public long TONGBOSABUN { get; set; } 

        /// <summary>
        /// 확인
        /// </summary>
		public string CONFIRM { get; set; } 

        /// <summary>
        /// 실수검일자
        /// </summary>
		public string SDATE { get; set; }

        /// <summary>
        /// 예약일자
        /// </summary>
        public string RDATE { get; set; }

        /// <summary>
        /// 최종입력사번
        /// </summary>
        public long ENTSABUN { get; set; } 

        /// <summary>
        /// 최종입력일자
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 검사코드
        /// </summary>
		public string EXCODE { get; set; } 

        /// <summary>
        /// 오전/오후 구분  (A: 오전, P: 오후)
        /// </summary>
		public string AMPM { get; set; } 

        /// <summary>
        /// 삭제일자
        /// </summary>
		public DateTime? DELDATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long PANO2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long AMCNT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long PMCNT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long CNT { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
		public string RID { get; set; }

        /// <summary>
        /// 접수번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 종합검진 선택검사 일정 및 예약관리 Table
        /// </summary>
        public HEA_RESV_EXAM()
        {
        }
    }
}
