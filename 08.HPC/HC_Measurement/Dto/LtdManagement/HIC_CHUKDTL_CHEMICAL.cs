namespace HC_Measurement.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HIC_CHUKDTL_CHEMICAL : BaseDto
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 공정코드
        /// </summary>
        public string PROCESS { get; set; }

        /// <summary>
        /// 공정코드
        /// </summary>
        public string PROCESS_K2BCD { get; set; }

        /// <summary>
        /// 공정코드
        /// </summary>
        public string PROCESS_NM { get; set; }

        /// <summary>
        /// 화학물질(상품명)
        /// </summary>
        public string PRODUCT_NM { get; set; }

        /// <summary>
        /// 제조 또는 사용여부
        /// </summary>
        public string GBUSE { get; set; }

        /// <summary>
        /// 사용용도
        /// </summary>
        public string USAGE { get; set; }

        /// <summary>
        /// 월취급량
        /// </summary>
        public long TREATMENT { get; set; }

        /// <summary>
        /// 월취급량 단위
        /// </summary>
        public string UNIT { get; set; }

        /// <summary>
        /// 비고
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 순번
        /// </summary>
        public long SEQNO { get; set; }

        /// <summary>
        /// 작업사번
        /// </summary>
        public long JOBSABUN { get; set; }

        /// <summary>
        /// 입력일자
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 삭제일자
        /// </summary>
        public DateTime? DELDATE { get; set; }

        /// <summary>
        /// Rowid
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string IsDelete { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string IsActive { get; set; }

        /// <summary>
        /// 건강검진 자료사전 Table
        /// </summary>
        public HIC_CHUKDTL_CHEMICAL()
        {
        }
    }
}
