namespace HC_Measurement.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HIC_CHUKDTL_SUBLTD : BaseDto
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 측정사업장 코드
        /// </summary>
        public long LTDCODE { get; set; }

        /// <summary>
        /// 협력업체 사업장 코드
        /// </summary>
        public long SUB_LTDCODE { get; set; }

        /// <summary>
        /// 협력업체 사업장명
        /// </summary>
        public string SUB_LTDNAME { get; set; }

        /// <summary>
        /// 등록사번
        /// </summary>
        public long JOBSABUN { get; set; }

        /// <summary>
        /// 등록일자
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 삭제일자
        /// </summary>
        public DateTime? DELDATE { get; set; }

        /// <summary>
        /// 비고
        /// </summary>
        public string REMARK { get; set; }

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
        public HIC_CHUKDTL_SUBLTD()
        {
        }
    }
}
