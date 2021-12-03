namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_HYANG_APPROVE : BaseDto
    {
        
        /// <summary>
        /// 처방발급일자
        /// </summary>
		public string SDATE { get; set; } 

        /// <summary>
        /// 사용예정일자
        /// </summary>
		public string BDATE { get; set; } 

        /// <summary>
        /// 종검,건진 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 종검,건진 등록번호
        /// </summary>
		public long PANO { get; set; } 

        /// <summary>
        /// 성명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 1.향정 2.마약
        /// </summary>
		public string JONG { get; set; } 

        /// <summary>
        /// 사용처(1.건강증진센타 2.본관내시경실)
        /// </summary>
		public string GBSITE { get; set; } 

        /// <summary>
        /// 진료과(HR.일반건진 TO.종합건진)
        /// </summary>
		public string DEPTCODE { get; set; } 

        /// <summary>
        /// 향정/마약 수가코드
        /// </summary>
		public string SUCODE { get; set; } 

        /// <summary>
        /// 수량
        /// </summary>
		public string QTY { get; set; } 

        /// <summary>
        /// 실수량
        /// </summary>
		public double REALQTY { get; set; } 

        /// <summary>
        /// 오더수량(ml)
        /// </summary>
		public double ENTQTY { get; set; } 

        /// <summary>
        /// 처방전발급 간호사 사번
        /// </summary>
		public string NRSABUN { get; set; } 

        /// <summary>
        /// 환자마스타 등록번호
        /// </summary>
		public string PTNO { get; set; } 

        /// <summary>
        /// 주민등록번호
        /// </summary>
		public string JUMIN { get; set; } 

        /// <summary>
        /// 성별(M/F)
        /// </summary>
		public string SEX { get; set; } 

        /// <summary>
        /// 나이
        /// </summary>
		public long AGE { get; set; } 

        /// <summary>
        /// 처방전 발급의뢰 등록일시
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 사용승인 의사 사번
        /// </summary>
		public string DRSABUN { get; set; } 

        /// <summary>
        /// 사용승인 일시
        /// </summary>
		public string APPROVETIME { get; set; } 

        /// <summary>
        /// 인쇄여부(Y.인쇄 N.미인쇄)
        /// </summary>
		public string PRINT { get; set; } 

        /// <summary>
        /// 마감일자와 시각
        /// </summary>
		public DateTime? OCSSENDTIME { get; set; } 

        /// <summary>
        /// 실사용량(ml)
        /// </summary>
		public double ENTQTY2 { get; set; } 

        /// <summary>
        /// 검사시간(1.오전 2.오후)
        /// </summary>
		public string AMPM { get; set; } 

        /// <summary>
        /// 수면내시경 여부(Y/N)
        /// </summary>
		public string GBSLEEP { get; set; } 

        /// <summary>
        /// 주민번호 암호화
        /// </summary>
		public string JUMIN2 { get; set; } 

        /// <summary>
        /// 마감차수(무조건 1차수로 처리)
        /// </summary>
		public long CHASU { get; set; } 

        /// <summary>
        /// 삭제일자 및 시각
        /// </summary>
		public DateTime? DELDATE { get; set; } 

        /// <summary>
        /// 전사서명번호(2014-12-02 추가함)
        /// </summary>
		public long CERTNO { get; set; } 

        /// <summary>
        /// 주소(2014-12-10 추가)
        /// </summary>
		public string JUSO { get; set; }

        public string ROWID { get; set; }

        public string RID { get; set; }

        /// <summary>
        /// 건진센타 향정/마약 의사 승인 내역
        /// </summary>
        public HIC_HYANG_APPROVE()
        {
        }
    }
}
