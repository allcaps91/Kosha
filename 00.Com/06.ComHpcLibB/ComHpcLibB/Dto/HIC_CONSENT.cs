namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_CONSENT : BaseDto
    {
        
        /// <summary>
        /// 등록일자
        /// </summary>
		public string BDATE { get; set; } 

        /// <summary>
        /// 수진일자
        /// </summary>
		public string SDATE { get; set; } 

        /// <summary>
        /// 외래 환자 등록번호
        /// </summary>
		public string PTNO { get; set; } 

        /// <summary>
        /// 건진,종검 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 건진,종검 환자번호
        /// </summary>
		public long PANO { get; set; } 

        /// <summary>
        /// 성명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 과(HR.건진 TO.종검)
        /// </summary>
		public string DEPTCODE { get; set; } 

        /// <summary>
        /// 동의서 서식코드
        /// </summary>
		public string FORMCODE { get; set; } 

        /// <summary>
        /// 페이지 건수
        /// </summary>
		public long PAGECNT { get; set; } 

        /// <summary>
        /// 삭제일자
        /// </summary>
		public DateTime? DELDATE { get; set; } 

        /// <summary>
        /// 동의의뢰 일시
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 동의의뢰 작업자 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 동의 일자 및 시각
        /// </summary>
		public string CONSENT_TIME { get; set; } 

        /// <summary>
        /// 의사사번
        /// </summary>
		public long DRSABUN { get; set; }

        /// <summary>
        /// 의사명
        /// </summary>
        public string DRNAME { get; set; }

        /// <summary>
        /// 의사서명일시
        /// </summary>
        public DateTime? DOCTSIGN { get; set; } 

        /// <summary>
        /// EMR 전송일자(YYYY-MM-DD)
        /// </summary>
		public DateTime? SENDTIME { get; set; } 

        /// <summary>
        /// 1페이지 파일명
        /// </summary>
		public string FILENAME1 { get; set; } 

        /// <summary>
        /// 2페이지 파일명
        /// </summary>
		public string FILENAME2 { get; set; } 

        /// <summary>
        /// 3페이지 파일명
        /// </summary>
		public string FILENAME3 { get; set; } 

        /// <summary>
        /// 1페이지 파일 전사서명값
        /// </summary>
		public long CERTNO1 { get; set; } 

        /// <summary>
        /// 2페이지 파일 전사서명값
        /// </summary>
		public long CERTNO2 { get; set; } 

        /// <summary>
        /// 3페이지 파일 전사서명값
        /// </summary>
		public long CERTNO3 { get; set; } 

        /// <summary>
        /// 수면내시경 신체등급(ASA)
        /// </summary>
		public string ASA { get; set; } 

        /// <summary>
        /// ASA 등록 사번
        /// </summary>
		public long ASASABUN { get; set; } 

        /// <summary>
        /// ASA 등록 일시
        /// </summary>
		public DateTime? ASATIME { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
		public string RID { get; set; }

        /// <summary>
        /// FORMNAME
        /// </summary>
		public string FORMNAME { get; set; }      
        
        /// <summary>
        /// 작성된 전자동의서 시퀀스
        /// </summary>
        public long EMRNO { get; set; }

        /// <summary>
        /// 일반건진,종합건진 동의서
        /// </summary>
        public HIC_CONSENT()
        {
        }
    }
}
