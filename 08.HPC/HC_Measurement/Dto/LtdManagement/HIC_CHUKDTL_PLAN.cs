namespace HC_Measurement.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HIC_CHUKDTL_PLAN : BaseDto
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 순번
        /// </summary>
        public long SEQNO { get; set; }

        /// <summary>
        /// 공정코드
        /// </summary>
        public string PROCESS { get; set; }

        /// <summary>
        /// K2B 공정코드
        /// </summary>
        public string PROCESS_K2B { get; set; }

        /// <summary>
        /// 공정명
        /// </summary>
        public string PROCESS_NM { get; set; }

        /// <summary>
        /// 유해물질코드
        /// </summary>
        public string MCODE { get; set; }

        /// <summary>
        /// K2B 유해물질코드
        /// </summary>
        public string MCODE_K2B { get; set; }

        /// <summary>
        /// 유해물질명
        /// </summary>
        public string MCODE_NM { get; set; }

        /// <summary>
        /// 발생주기
        /// </summary>
        public string JUGI { get; set; }

        /// <summary>
        /// 근로자수
        /// </summary>
        public string INWON { get; set; }

        /// <summary>
        /// 작업시간
        /// </summary>
        public long JTIME { get; set; }

        /// <summary>
        /// 폭로시간
        /// </summary>
        public long PTIME { get; set; }

        /// <summary>
        /// 측정방법(개인/지역)
        /// </summary>
        public string CHKWAY { get; set; }

        /// <summary>
        /// 예상측정건수
        /// </summary>
        public long CHKCOUNT { get; set; }

        /// <summary>
        /// 입력시각
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 입력사번
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 삭제일자
        /// </summary>
        public DateTime? DELDATE { get; set; }

        /// <summary>
        /// 측정방법 코드
        /// </summary>
        public string CHKWAY_CD { get; set; }

        /// <summary>
        /// 측정방법명
        /// </summary>
        public string CHKWAY_NM { get; set; }

        /// <summary>
        /// 분석방법명
        /// </summary>
        public string ANALWAY_NM { get; set; }

        /// <summary>
        /// Rowid
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 삭제 유무
        /// </summary>
		public string IsDelete { get; set; }

        /// <summary>
        /// 선택 유무
        /// </summary>
		public string IsActive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HIC_CHUKDTL_PLAN()
        {
        }
    }
}
